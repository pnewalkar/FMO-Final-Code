namespace Fmo.Batch.FileLoader
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;
    using System.Xml.Serialization;
    using Fmo.Common.ConfigurationManagement;
    using Fmo.Common.EmailManagement;
    using Fmo.Common.ExceptionManagement;
    using Fmo.Common.Interface;
    using Fmo.Common.LoggingManagement;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.DTO.FileProcessing;
    using Fmo.MessageBrokerCore.Messaging;
    using Fmo.NYBLoader;
    using Fmo.NYBLoader.Common;
    using Fmo.NYBLoader.Interfaces;
    using Ninject;

    public partial class FileLoader : ServiceBase
    {
        private static string dateTimeFormat = "{0:-yyyy-MM-d-HH-mm-ss}";
        private string strProcessedFilePath = string.Empty;
        private string strErrorFilePath = string.Empty;        
        private readonly IKernel kernal;
        private List<FileSystemWatcher> listFileSystemWatcher;
        private List<CustomFolderSettings> listFolders;
        private INYBLoader nybLoader = default(INYBLoader);
        private IPAFLoader pafLoader = default(IPAFLoader);
        private ITPFLoader tpfLoader = default(ITPFLoader);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IEmailHelper emailHelper = default(IEmailHelper);
        private IFileProcessingLogRepository fileProcessingLogRepository = default(IFileProcessingLogRepository);
        private IDatabaseFactory<FMODBContext> databaseFactory = default(IDatabaseFactory<FMODBContext>);

        private IFileMover fileMover = default(IFileMover);
        private IConfigurationHelper configurationHelper;

        /// <summary>
        /// Append timestamp to filename before writing the file to specified folder
        /// </summary>
        /// <param name="strfileName"></param>
        /// <returns></returns>
        private static string AppendTimeStamp(string strfileName)
        {
            try
            {
                return string.Concat(Path.GetFileNameWithoutExtension(strfileName), string.Format(dateTimeFormat, DateTime.Now), Path.GetExtension(strfileName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FileLoader()
        {
            // this.strProcessedFilePath = ConfigurationManager.AppSettings["ProcessedFilePath"].ToString();
            // this.strErrorFilePath = ConfigurationManager.AppSettings["ErrorFilePath"].ToString();
            kernal = new StandardKernel();
            Register(kernal);
            InitializeComponent();
            this.strProcessedFilePath = configurationHelper.ReadAppSettingsConfigurationValues("ProcessedFilePath");
            this.strErrorFilePath = configurationHelper.ReadAppSettingsConfigurationValues("ErrorFilePath");
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected void Register(IKernel kernel)
        {
            kernel.Bind<INYBLoader>().To<NYBLoader>();
            kernel.Bind<IPAFLoader>().To<PAFLoader>();
            kernel.Bind<ITPFLoader>().To<TPFLoader>();
            kernel.Bind<IFileProcessingLogRepository>().To<FileProcessingLogRepository>();
            kernel.Bind<IMessageBroker<PostalAddressDTO>>().To<MessageBroker<PostalAddressDTO>>();
            kernel.Bind<IMessageBroker<AddressLocationUSRDTO>>().To<MessageBroker<AddressLocationUSRDTO>>().InSingletonScope();
            kernel.Bind<IHttpHandler>().To<HttpHandler>();
            kernel.Bind<ILoggingHelper>().To<LoggingHelper>();
            kernel.Bind<IConfigurationHelper>().To<ConfigurationHelper>().InSingletonScope();
            kernel.Bind<IFileMover>().To<FileMover>().InSingletonScope();
            kernel.Bind<IEmailHelper>().To<EmailHelper>();
            kernel.Bind<IDatabaseFactory<FMODBContext>>().To<DatabaseFactory<FMODBContext>>();

            databaseFactory = kernel.Get<IDatabaseFactory<FMODBContext>>();
            fileProcessingLogRepository = kernel.Get<IFileProcessingLogRepository>();
            nybLoader = kernel.Get<INYBLoader>();
            pafLoader = kernel.Get<IPAFLoader>();
            tpfLoader = kernel.Get<ITPFLoader>();
            loggingHelper = kernel.Get<ILoggingHelper>();
            fileMover = kernel.Get<IFileMover>();
            emailHelper = kernal.Get<IEmailHelper>();
            configurationHelper = kernel.Get<IConfigurationHelper>();
        }

        /// <summary>Event automatically fired when the service is started by Windows</summary>
        /// <param name="args">array of arguments</param>
        protected override void OnStart(string[] args)
        {
            Start();
        }

        /// <summary>Event automatically fired when the service is stopped by Windows</summary>
        protected override void OnStop()
        {
            if (listFileSystemWatcher != null)
            {
                foreach (FileSystemWatcher fsw in listFileSystemWatcher)
                {
                    // Stop listening
                    fsw.EnableRaisingEvents = false;

                    // Dispose the Object
                    fsw.Dispose();
                }

                // Clean the list
                listFileSystemWatcher.Clear();
            }
        }

        private void Start()
        {
            // Initialize the list of FileSystemWatchers based on the XML configuration file
            PopulateListFileSystemWatchers();

            // Start the file system watcher for each of the file specification
            // and folders found on the List<>
            StartFileSystemWatcher();
        }

        /// <summary>Reads an XML file and populates a list of <CustomFolderSettings> </summary>
        private void PopulateListFileSystemWatchers()
        {
            // Get the XML file name from the App.config file
            string fileNameXML = ConfigurationManager.AppSettings["XMLFileFolderSettings"];

            // Create an instance of XMLSerializer
            XmlSerializer deserializer = new XmlSerializer(typeof(List<CustomFolderSettings>));
            TextReader reader = new StreamReader(fileNameXML);
            object obj = deserializer.Deserialize(reader);

            // Close the TextReader object
            reader.Close();

            // Obtain a list of CustomFolderSettings from XML Input data
            listFolders = obj as List<CustomFolderSettings>;
        }

        /// <summary>Start the file system watcher for each of the file
        /// specification and folders found on the List<>/// </summary>
        private void StartFileSystemWatcher()
        {
            // Creates a new instance of the list
            this.listFileSystemWatcher = new List<FileSystemWatcher>();

            // Loop the list to process each of the folder specifications found
            foreach (CustomFolderSettings customFolder in listFolders)
            {
                DirectoryInfo dir = new DirectoryInfo(customFolder.FolderPath);

                // Checks whether the folder is enabled and
                // also the directory is a valid location
                if (customFolder.FolderEnabled && dir.Exists)
                {
                    // Creates a new instance of FileSystemWatcher
                    FileSystemWatcher fileSWatch = new FileSystemWatcher();

                    // Sets the filter
                    fileSWatch.Filter = customFolder.FolderFilter;

                    // Sets the folder location
                    fileSWatch.Path = customFolder.FolderPath;

                    // Sets the action to be executed
                    StringBuilder actionToExecute = new StringBuilder(
                      customFolder.ExecutableFile);

                    // List of arguments
                    StringBuilder actionArguments = new StringBuilder(
                      customFolder.ExecutableArguments);

                    // Subscribe to notify filters
                    fileSWatch.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName |
                      NotifyFilters.DirectoryName;

                    // Associate the event that will be triggered when a new file
                    // is added to the monitored folder, using a lambda expression
                    // fileSWatch.Created += (senderObj, fileSysArgs) =>
                    //  fileSWatch_Created(senderObj, fileSysArgs, actionToExecute.ToString(), actionArguments.ToString());
                    fileSWatch.Created += new FileSystemEventHandler((senderObj, fileSysArgs) => FileSWatch_Created(senderObj, fileSysArgs, actionToExecute.ToString(), actionArguments.ToString()));
                    fileSWatch.Error += OnFileSystemWatcherError;

                    // Begin watching
                    fileSWatch.EnableRaisingEvents = true;

                    // Add the systemWatcher to the list
                    listFileSystemWatcher.Add(fileSWatch);

                    // Record a log entry into Windows Event Log

                    // CustomLogEvent(String.Format(
                    //  "Starting to monitor files with extension ({0}) in the folder ({1})",
                    //  fileSWatch.Filter, fileSWatch.Path));
                }
            }
        }

        private void OnFileSystemWatcherError(object sender, ErrorEventArgs e)
        {
            var watcher = (FileSystemWatcher)sender;
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();

            // Log error
            Start();
        }

        /// <summary>This event is triggered when a file with the specified
        /// extension is created on the monitored folder</summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">List of arguments - FileSystemEventArgs</param>
        /// <param name="action_Exec">The action to be executed upon detecting a change in the File system</param>
        /// <param name="action_Args">arguments to be passed to the executable (action)</param>
        private void FileSWatch_Created(object sender, FileSystemEventArgs e, string action_Exec, string action_Args)
        {
            string fileName = e.FullPath;
            if (!string.IsNullOrEmpty(action_Args))
            {
                switch (action_Args)
                {
                    case "PAF":
                        this.pafLoader.LoadPAF(fileName);
                        break;

                    case "NYB":
                        LoadNYBDetails(fileName);
                        break;

                    case "TPF":
                        this.tpfLoader.LoadTPFDetailsFromXML(fileName);
                        break;
                }
            }

            // ExecuteProcess(fileName);
        }

        /// <summary>
        /// Read files from zip file and call NYBLoader Assembly to validate and save records
        /// </summary>
        /// <param name="fileName">Input file name as a param</param>
        private void LoadNYBDetails(string fileName)
        {
            try
            {
                using (ZipArchive zip = ZipFile.OpenRead(fileName))
                {
                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        string strLine = string.Empty;
                        string strfileName = string.Empty;

                        Stream stream = entry.Open();
                        var reader = new StreamReader(stream);
                        strLine = reader.ReadToEnd();
                        strfileName = entry.Name;
                        List<PostalAddressDTO> lstNYBDetails = this.nybLoader.LoadNYBDetailsFromCSV(strLine.Trim());
                        if (lstNYBDetails != null && lstNYBDetails.Count > 0)
                        {
                            var invalidRecordsCount = lstNYBDetails.Where(n => n.IsValidData == false).ToList().Count;

                            if (invalidRecordsCount > 0)
                            {
                                File.WriteAllText(Path.Combine(strErrorFilePath, AppendTimeStamp(strfileName)), strLine);
                            }
                            else
                            {
                                File.WriteAllText(Path.Combine(strProcessedFilePath, AppendTimeStamp(strfileName)), strLine);
                                this.nybLoader.SaveNYBDetails(lstNYBDetails, strfileName);
                            }
                        }
                        else
                        {
                            this.loggingHelper.LogInfo("Load NYB Error Message : NYB File ins not valid File Name :" + strfileName + " : Log Time :" + DateTime.Now.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                loggingHelper.LogError(ex);
            }
        }
    }
}