using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO.FileProcessing;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.MessageBrokerMiddleware;
using RM.Integration.ThirdPartyAddressLocation.Loader.Utils.Interfaces;

namespace RM.Integration.ThirdPartyAddressLocation.Loader.Utils
{
    /// <summary>
    /// Load third party file, process and add to MSMQ
    /// </summary>
    public class ThirdPartyFileProcessUtility : IThirdPartyFileProcessUtility
    {
        private readonly IMessageBroker<AddressLocationUSRDTO> msgBroker;
        private readonly IFileMover fileMover;
        private readonly ILoggingHelper loggingHelper;
        private readonly IConfigurationHelper configHelper;
        private string xsdLocation;
        private string processed;
        private string error;

        public ThirdPartyFileProcessUtility(IMessageBroker<AddressLocationUSRDTO> messageBroker,
                         IFileMover fileMover, IExceptionHelper exceptionHelper,
                         ILoggingHelper loggingHelper,
                         IConfigurationHelper configHelper)
        {
            this.msgBroker = messageBroker;
            this.fileMover = fileMover;
            this.loggingHelper = loggingHelper;
            this.configHelper = configHelper;
            this.xsdLocation = configHelper.ReadAppSettingsConfigurationValues(ThirdPartyLoaderConstants.XSDLOCATIONCONFIG);
            this.processed = configHelper.ReadAppSettingsConfigurationValues(ThirdPartyLoaderConstants.USRPROCESSEDFILEPATHCONFIG);
            this.error = configHelper.ReadAppSettingsConfigurationValues(ThirdPartyLoaderConstants.USRERRORFILEPATHCONFIG);
        }

        /// <summary>
        /// Load the XML data from file to Message Queue.
        /// </summary>
        /// <param name="strPath"></param>
        public void LoadUSRDetailsFromXML(string strPath)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Service.LoadUSRDetailsFromXML"))
            {
                List<AddressLocationUSRDTO> lstUSRFiles = null;
                List<AddressLocationUSRDTO> lstUSRInsertFiles = null;

                string methodName = MethodBase.GetCurrentMethod().Name;
                LogMethodInfoBlock(methodName, LoggerTraceConstants.MethodExecutionStarted, LoggerTraceConstants.COLON);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyPriority, LoggerTraceConstants.ThirdPartyLoaderMethodEntryEventId, LoggerTraceConstants.Title);

                if (CheckFileName(new FileInfo(strPath).Name))
                {
                    if (IsFileValid(strPath))
                    {
                        lstUSRFiles = GetValidRecords(strPath);

                        lstUSRInsertFiles = lstUSRFiles.Where(insertFiles => insertFiles.ChangeType == ThirdPartyLoaderConstants.INSERT || insertFiles.ChangeType == ThirdPartyLoaderConstants.DELETE || insertFiles.ChangeType == ThirdPartyLoaderConstants.UPDATE).ToList();

                        lstUSRInsertFiles.ForEach(addressLocation =>
                        {
                            //Message is created and the Postal Address DTO is passed as the object to be queued along with the queue name and queue path where the object
                            //needs to be queued.
                            IMessage USRMsg = msgBroker.CreateMessage(addressLocation, ThirdPartyLoaderConstants.QUEUETHIRDPARTY, ThirdPartyLoaderConstants.QUEUEPATH);

                            //The messge object created in the above code is then pushed onto the queue. This internally uses the MSMQ Send function to push the message
                            //to the queue.
                            msgBroker.SendMessage(USRMsg);
                        });

                        fileMover.MoveFile(new string[] { strPath }, new string[] { processed, AppendTimeStamp(new FileInfo(strPath).Name) });
                    }
                    else
                    {
                        loggingHelper.Log(string.Format(ThirdPartyLoaderConstants.LOGMESSAGEFORUSRDATAVALIDATION, new FileInfo(strPath).Name, DateTime.UtcNow.ToString()), TraceEventType.Information, null);
                        fileMover.MoveFile(new string[] { strPath }, new string[] { error, AppendTimeStamp(new FileInfo(strPath).Name) });
                    }
                }

                LogMethodInfoBlock(methodName, LoggerTraceConstants.MethodExecutionCompleted, LoggerTraceConstants.COLON);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyPriority, LoggerTraceConstants.ThirdPartyLoaderMethodExitEventId, LoggerTraceConstants.Title);
            }
        }

        /// <summary>
        /// Check file name is valid
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <returns></returns>
        private bool CheckFileName(string fileName)
        {
            fileName = Path.GetFileNameWithoutExtension(fileName);
            Regex reg = new Regex(ThirdPartyLoaderConstants.USRFILENAME);
            return reg.IsMatch(fileName);
        }

        /// <summary>
        /// Checks whether the file is valid or not
        /// </summary>
        /// <returns></returns>
        private bool IsFileValid(string strPath)
        {
            bool isFilevalid = true;
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, LoggerTraceConstants.MethodExecutionStarted, LoggerTraceConstants.COLON);

            try
            {
                XmlSerializer fledeserializer = new XmlSerializer(typeof(object), new XmlRootAttribute(ThirdPartyLoaderConstants.USRXMLROOT));
                XmlDocument validXmlDocument = new XmlDocument();
                XmlNode rootNode = validXmlDocument.CreateNode(XmlNodeType.Element, ThirdPartyLoaderConstants.USRXMLROOT, null);
                validXmlDocument.AppendChild(rootNode);

                using (TextReader reader = new StreamReader(strPath))
                {
                    List<XmlNode> xmlNodes = ((XmlNode[])fledeserializer.Deserialize(reader)).ToList();

                    xmlNodes.ForEach(xmlNode =>
                    {
                        if (!IsXmlValid(strPath, xsdLocation, xmlNode))
                        {
                            isFilevalid = false;
                        }
                    });
                };

                return isFilevalid;
            }
            catch (Exception)
            {
                fileMover.MoveFile(new string[] { strPath }, new string[] { error, AppendTimeStamp(new FileInfo(strPath).Name) });
                isFilevalid = false;
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, LoggerTraceConstants.MethodExecutionCompleted, LoggerTraceConstants.COLON);
            }
        }

        /// <summary>
        /// Return the valid records after file validation.
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        private List<AddressLocationUSRDTO> GetValidRecords(string strPath)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, LoggerTraceConstants.MethodExecutionStarted, LoggerTraceConstants.COLON);

            try
            {
                List<AddressLocationUSRDTO> lstUSRFiles = new List<AddressLocationUSRDTO>();

                XmlSerializer fledeserializer = new XmlSerializer(typeof(object), new XmlRootAttribute(ThirdPartyLoaderConstants.USRXMLROOT));
                XmlDocument validXmlDocument = new XmlDocument();
                XmlNode rootNode = validXmlDocument.CreateNode(XmlNodeType.Element, ThirdPartyLoaderConstants.USRXMLROOT, null);
                validXmlDocument.AppendChild(rootNode);

                using (TextReader reader = new StreamReader(strPath))
                {
                    List<XmlNode> xmlNodes = ((XmlNode[])fledeserializer.Deserialize(reader)).ToList();

                    xmlNodes.ForEach(xmlNode =>
                    {
                        XmlNode newNode = validXmlDocument.ImportNode(xmlNode, true);
                        rootNode.AppendChild(newNode);
                    });

                    using (XmlReader xmlReader = XmlReader.Create(new StringReader(validXmlDocument.OuterXml)))
                    {
                        xmlReader.MoveToContent();
                        lstUSRFiles = (List<AddressLocationUSRDTO>)(new XmlSerializer(
                                                                                      typeof(List<AddressLocationUSRDTO>),
                                                                                      new XmlRootAttribute(ThirdPartyLoaderConstants.USRXMLROOT)).Deserialize(xmlReader));
                    }
                };

                return lstUSRFiles;
            }
            catch (Exception)
            {
                fileMover.MoveFile(new string[] { strPath }, new string[] { error, AppendTimeStamp(new FileInfo(strPath).Name) });
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, LoggerTraceConstants.MethodExecutionCompleted, LoggerTraceConstants.COLON);
            }
        }

        /// <summary>
        /// Validate the XML file against the XSD file to check the sequence and data type.
        /// </summary>
        /// <param name="xsdFile"></param>
        /// <param name="xNode"></param>
        /// <returns></returns>
        private bool IsXmlValid(string fileName, string xsdFile, XmlNode xNode)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, LoggerTraceConstants.MethodExecutionStarted, LoggerTraceConstants.COLON);

            bool result = true;
            XDocument xDoc = XDocument.Load(new XmlNodeReader(xNode));
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("", XmlReader.Create(xsdFile));

            xDoc.Validate(schemas, (o, e) =>
            {
                int UDPRN = 0;
                if (!string.IsNullOrEmpty(xDoc.Element(XName.Get(ThirdPartyLoaderConstants.ADDRESSLOCATIONXMLROOT)).Element(XName.Get(ThirdPartyLoaderConstants.USRUDPRN)).Value))
                {
                    UDPRN = Convert.ToInt32(xDoc.Element(XName.Get(ThirdPartyLoaderConstants.ADDRESSLOCATIONXMLROOT))
                                                        .Element(XName.Get(ThirdPartyLoaderConstants.USRUDPRN)).Value);
                }

                //logger code to write schema mismatch exception
                result = false;
            });

            LogMethodInfoBlock(methodName, LoggerTraceConstants.MethodExecutionCompleted, LoggerTraceConstants.COLON);
            return result;
        }

        /// <summary>
        /// Append timestamp to the file being moved.
        /// </summary>
        /// <param name="strfileName">File Name</param>
        /// <returns></returns>
        private string AppendTimeStamp(string strfileName)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(strfileName),
               string.Format(ThirdPartyLoaderConstants.DATETIMEFORMAT, DateTime.Now),
                Path.GetExtension(strfileName)
                );
        }

        /// <summary>
        /// Method level entry exit logging.
        /// </summary>
        /// <param name="methodName">Function Name</param>
        /// <param name="logMessage">Message</param>
        /// <param name="seperator">Seperator used</param>
        private void LogMethodInfoBlock(string methodName, string logMessage, string seperator)
        {
            loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.SavePostalAddressPriority, LoggerTraceConstants.SavePostalAddressBusinessMethodExitEventId, LoggerTraceConstants.Title);
        }
    }
}