﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Xml.Serialization;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Integration.ThirdPartyAddressLocation.Loader.Utils;
using RM.Integration.ThirdPartyAddressLocation.Loader.Utils.Interfaces;

namespace RM.Integration.ThirdPartyAddressLocation.Loader
{
    public partial class ThirdPartyImport : ServiceBase
    {
        private const string XMLFileFolderSettings = "XMLFileFolderSettings";
        private const string BatchServiceName = "ServiceName";

        #region Property Declarations

        private List<FileSystemWatcher> listFileSystemWatcher;
        private List<CustomFolderSettings> listFolders;
        private IThirdPartyFileProcessUtility usrLoader = default(IThirdPartyFileProcessUtility);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IConfigurationHelper configurationHelper;

        #endregion Property Declarations

        #region Constructor

        public ThirdPartyImport(IThirdPartyFileProcessUtility usrLoader, ILoggingHelper loggingHelper, IConfigurationHelper configurationHelper)
        {
            this.usrLoader = usrLoader;
            this.loggingHelper = loggingHelper;
            this.configurationHelper = configurationHelper;
            this.ServiceName = configurationHelper.ReadAppSettingsConfigurationValues(BatchServiceName);
        }

        #endregion Constructor

        #region On Debug test method

        /// <summary>
        /// Test method for debugging service on local
        /// </summary>
        public void OnDebug()
        {
            OnStart(null);
        }

        #endregion On Debug test method

        #region OnStart

        /// <summary>
        /// Event automatically fired when the service is started by Windows
        /// </summary>
        /// <param name="args">array of arguments</param>
        protected override void OnStart(string[] args)
        {
            Start();
        }

        #endregion OnStart

        #region OnStop

        /// <summary>
        /// Event automatically fired when the service is stopped by Windows
        /// </summary>
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

        #endregion OnStop

        #region Start

        /// <summary>
        /// Initialize Watchers
        /// </summary>
        private void Start()
        {
            // Initialize the list of FileSystemWatchers based on the XML configuration file
            PopulateListFileSystemWatchers();

            // Start the file system watcher for each of the file specification and folders found on
            // the List<>
            StartFileSystemWatcher();
        }

        #endregion Start

        #region File Watchers and handlers

        /// <summary>Reads an XML file and populates a list of <CustomFolderSettings> </summary>
        private void PopulateListFileSystemWatchers()
        {
            // Get the XML file name from the App.config file
            string fileNameXML = ConfigurationManager.AppSettings[XMLFileFolderSettings];

            // Create an instance of XMLSerializer
            XmlSerializer deserializer = new XmlSerializer(typeof(List<CustomFolderSettings>));
            using (TextReader reader = new StreamReader(fileNameXML))
            {
                object obj = deserializer.Deserialize(reader);

                // Obtain a list of CustomFolderSettings from XML Input data
                listFolders = obj as List<CustomFolderSettings>;
            }
        }

        /// <summary>Start the file system watcher for each of the file specification and folders
        /// found on the List<>/// </summary>
        private void StartFileSystemWatcher()
        {
            // Creates a new instance of the list
            this.listFileSystemWatcher = new List<FileSystemWatcher>();

            // Loop the list to process each of the folder specifications found
            foreach (CustomFolderSettings customFolder in listFolders)
            {
                DirectoryInfo dir = new DirectoryInfo(customFolder.FolderPath);

                // Checks whether the folder is enabled and also the directory is a valid location
                if (customFolder.FolderEnabled && dir.Exists)
                {
                    // Creates a new instance of FileSystemWatcher
                    FileSystemWatcher fileSWatch = new FileSystemWatcher();

                    // Sets the filter
                    fileSWatch.Filter = customFolder.FolderFilter;

                    // Sets the folder location
                    fileSWatch.Path = customFolder.FolderPath;

                    // List of arguments
                    StringBuilder actionArguments = new StringBuilder(
                      customFolder.ExecutableArguments);

                    // Subscribe to notify filters
                    fileSWatch.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName |
                      NotifyFilters.DirectoryName;

                    // Associate the event that will be triggered when a new file is added to the
                    // monitored folder, using a lambda expression fileSWatch.Created += (senderObj,
                    // fileSysArgs) => fileSWatch_Created(senderObj, fileSysArgs,
                    // actionToExecute.ToString(), actionArguments.ToString());
                    fileSWatch.Error += new ErrorEventHandler(OnFileSystemWatcherError); // OnFileSystemWatcherError;
                    fileSWatch.Created += new FileSystemEventHandler((senderObj, fileSysArgs) => FileSWatch_Created(senderObj, fileSysArgs, actionArguments.ToString()));

                    // Begin watching
                    fileSWatch.EnableRaisingEvents = true;

                    // Add the systemWatcher to the list
                    listFileSystemWatcher.Add(fileSWatch);
                }
            }
        }

        #region Error Handler

        private void OnFileSystemWatcherError(object sender, ErrorEventArgs e)
        {
            try
            {
                var watcher = (FileSystemWatcher)sender;
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
            catch (Exception ex)
            {
                loggingHelper.Log(ex, TraceEventType.Error);
            }
            finally
            {
                Start();
            }
        }

        #endregion Error Handler

        /// <summary>
        /// This event is triggered when a file with the specified extension is created on the
        /// monitored folder
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">List of arguments - FileSystemEventArgs</param>
        /// <param name="action_Args">arguments to be passed to the executable (action)</param>
        private void FileSWatch_Created(object sender, FileSystemEventArgs e, string action_Args)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, LoggerTraceConstants.MethodExecutionStarted, LoggerTraceConstants.COLON);
            string fileName = e.FullPath;
            try
            {
                this.usrLoader.LoadUSRDetailsFromXML(fileName);
            }
            catch (Exception ex)
            {
                loggingHelper.Log(ex, TraceEventType.Error);
            }
            finally
            {
                LogMethodInfoBlock(methodName, LoggerTraceConstants.MethodExecutionCompleted, LoggerTraceConstants.COLON);
            }
        }

        #endregion File Watchers and handlers

        /// <summary>
        /// Method level entry exit logging.
        /// </summary>
        /// <param name="methodName">Function Name</param>
        /// <param name="logMessage">Message</param>
        /// <param name="separator">separator</param>
        private void LogMethodInfoBlock(string methodName, string logMessage, string separator)
        {
            loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.SavePostalAddressPriority, LoggerTraceConstants.SavePostalAddressBusinessMethodExitEventId, LoggerTraceConstants.Title);
        }
    }
}