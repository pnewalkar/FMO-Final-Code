namespace FMO.Batch.FileLoader
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.ServiceProcess;
    using System.Text;
    using System.Xml.Serialization;
    using Fmo.DTO;
    using Fmo.MessageBrokerCore.Messaging;
    using Fmo.NYBLoader;
    using Fmo.NYBLoader.Interfaces;
    using Ninject;
    using Ninject.Parameters;

    public partial class FileLoader : ServiceBase
    {
        private static string strFMOWEbApiURL = ConfigurationManager.AppSettings["FMOWebAPIURL"].ToString();
        private readonly IKernel kernal;
        private List<FileSystemWatcher> listFileSystemWatcher;
        private List<CustomFolderSettings> listFolders;
        private INYBLoader nybLoader = default(INYBLoader);
        private IPAFLoader pafLoader = default(IPAFLoader);
        private ITPFLoader tpfLoader = default(ITPFLoader);

        public FileLoader()
        {
            kernal = new StandardKernel();
            Register(kernal);
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected void Register(IKernel kernel)
        {
            kernel.Bind<INYBLoader>().To<NYBLoader>().InSingletonScope();
            kernel.Bind<IPAFLoader>().To<PAFLoader>().InSingletonScope();
            kernel.Bind<ITPFLoader>().To<TPFLoader>().InSingletonScope();
            kernel.Bind<IMessageBroker<PostalAddressDTO>>().To<MessageBroker<PostalAddressDTO>>().InSingletonScope();
            kernel.Bind<IMessageBroker<addressLocation>>().To<MessageBroker<addressLocation>>().InSingletonScope();

            nybLoader = kernel.Get<INYBLoader>(new[] { new ConstructorArgument("_client", new HttpClient()), new ConstructorArgument("_strFMOWEbApiURL", strFMOWEbApiURL) });
            pafLoader = kernel.Get<IPAFLoader>();
            tpfLoader = kernel.Get<ITPFLoader>();
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
                    fileSWatch.Created += new FileSystemEventHandler((senderObj, fileSysArgs) => fileSWatch_Created(senderObj, fileSysArgs, actionToExecute.ToString(), actionArguments.ToString()));
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
        private void fileSWatch_Created(object sender, FileSystemEventArgs e, string action_Exec, string action_Args)
        {
            string fileName = e.FullPath;
            if (!string.IsNullOrEmpty(action_Args))
            {
                switch (action_Args)
                {
                    case "PAF":
                        this.pafLoader.LoadPAFDetailsFromCSV(fileName);
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

        private void LoadNYBDetails(string fileName)
        {
            try
            {
                List<PostalAddressDTO> lstNYBDetails = this.nybLoader.LoadNYBDetailsFromCSV(fileName);
                if (lstNYBDetails != null && lstNYBDetails.Count > 0)
                {
                    var invalidRecordsCount = lstNYBDetails.Where(n => n.IsValidData == false).ToList().Count;
                    if (invalidRecordsCount == 0)
                    {
                        this.nybLoader.SaveNYBDetails(lstNYBDetails);
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /*
        private string SerializeObject<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        private T DeserializeXMLFileToObject<T>(string XmlFilename)
        {
            T returnObject = default(T);
            if (string.IsNullOrEmpty(XmlFilename)) return default(T);

            try
            {
                StreamReader xmlStream = new StreamReader(XmlFilename);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                returnObject = (T)serializer.Deserialize(xmlStream);
            }
            catch (Exception ex)
            {
            }
            return returnObject;
        }
        */
    }
}