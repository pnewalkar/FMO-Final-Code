using Fmo.DTO.FileProcessing;
using Fmo.MessageBrokerCore.Messaging;
using Fmo.NYBLoader.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Fmo.Common.Constants;
using System.Xml;
using System.Xml.Schema;
using Fmo.Common.Interface;
using System.Reflection;

namespace Fmo.NYBLoader
{
    /// <summary>
    /// Load third party file, process and add to MSMQ
    /// </summary>
    public class USRLoader : IUSRLoader
    {
        private readonly IMessageBroker<AddressLocationUSRDTO> msgBroker;
        private readonly IFileMover fileMover;
        private readonly IExceptionHelper exceptionHelper;
        private readonly ILoggingHelper loggingHelper;
        private readonly IConfigurationHelper configHelper;
        private string XSD_LOCATION;
        private string PROCESSED;
        private string ERROR;
        private bool enableLogging = false;


        public USRLoader(IMessageBroker<AddressLocationUSRDTO> messageBroker,
                         IFileMover fileMover, IExceptionHelper exceptionHelper,
                         ILoggingHelper loggingHelper,
                         IConfigurationHelper configHelper)
        {
            this.msgBroker = messageBroker;
            this.fileMover = fileMover;
            this.exceptionHelper = exceptionHelper;
            this.loggingHelper = loggingHelper;
            this.configHelper = configHelper;
            this.XSD_LOCATION = configHelper.ReadAppSettingsConfigurationValues(Constants.XSDLOCATIONCONFIG);
            this.PROCESSED = configHelper.ReadAppSettingsConfigurationValues(Constants.USRPROCESSEDFILEPATHCONFIG);
            this.ERROR = configHelper.ReadAppSettingsConfigurationValues(Constants.USRERRORFILEPATHCONFIG);
            this.enableLogging = Convert.ToBoolean(configHelper.ReadAppSettingsConfigurationValues(Constants.EnableLogging));
        }

        /// <summary>
        /// Load the XML data from file to Message Queue.
        /// </summary>
        /// <param name="strPath"></param>
        public void LoadTPFDetailsFromXML(string strPath)
        {
            string destinationPath = string.Empty;
            List<AddressLocationUSRDTO> lstUSRFiles = null;
            List<AddressLocationUSRDTO> lstUSRInsertFiles = null;
            List<AddressLocationUSRDTO> lstUSRUpdateFiles = null;
            List<AddressLocationUSRDTO> lstUSRDeleteFiles = null;

            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted);

            try
            {
                if (IsFileValid(strPath))
                {
                    lstUSRFiles = GetValidRecords(strPath);

                    lstUSRInsertFiles = lstUSRFiles.Where(insertFiles => insertFiles.ChangeType == Constants.INSERT).ToList();
                    lstUSRUpdateFiles = lstUSRFiles.Where(updateFiles => updateFiles.ChangeType == Constants.UPDATE).ToList();
                    lstUSRDeleteFiles = lstUSRFiles.Where(deleteFiles => deleteFiles.ChangeType == Constants.DELETE).ToList();

                    lstUSRInsertFiles.ForEach(addressLocation =>
                    {
                        IMessage USRMsg = msgBroker.CreateMessage(addressLocation, Constants.QUEUETHIRDPARTY, Constants.QUEUEPATH);
                        msgBroker.SendMessage(USRMsg);
                    });


                    fileMover.MoveFile(new string[] { strPath }, new string[] { PROCESSED, AppendTimeStamp(new FileInfo(strPath).Name) });
                }
                else
                {
                    fileMover.MoveFile(new string[] { strPath }, new string[] { ERROR, AppendTimeStamp(new FileInfo(strPath).Name) });
                }
            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted);
            }
        }

        /// <summary>
        /// Checks whether the file is valid or not
        /// </summary>
        /// <returns></returns>
        private bool IsFileValid(string strPath)
        {
            bool isFilevalid = true;
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted);

            try
            {
                List<AddressLocationUSRDTO> lstUSRFiles = new List<AddressLocationUSRDTO>();

                XmlSerializer fledeserializer = new XmlSerializer(typeof(object), new XmlRootAttribute(Constants.USRXMLROOT));
                XmlDocument validXmlDocument = new XmlDocument();
                XmlNode rootNode = validXmlDocument.CreateNode(XmlNodeType.Element, Constants.USRXMLROOT, null);
                validXmlDocument.AppendChild(rootNode);


                using (TextReader reader = new StreamReader(strPath))
                {
                    List<XmlNode> xmlNodes = ((XmlNode[])fledeserializer.Deserialize(reader)).ToList();
                    List<XmlNode> validXmlNodes = new List<XmlNode>();

                    xmlNodes.ForEach(xmlNode =>
                    {
                        if (!IsXmlValid(XSD_LOCATION, xmlNode))
                        {
                            isFilevalid = false;
                        }
                    });
                };

                return isFilevalid;
            }
            catch (Exception)
            {
                fileMover.MoveFile(new string[] { strPath }, new string[] { ERROR, AppendTimeStamp(new FileInfo(strPath).Name) });
                isFilevalid = false;
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted);
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
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted);

            try
            {
                List<AddressLocationUSRDTO> lstUSRFiles = new List<AddressLocationUSRDTO>();

                XmlSerializer fledeserializer = new XmlSerializer(typeof(object), new XmlRootAttribute(Constants.USRXMLROOT));
                XmlDocument validXmlDocument = new XmlDocument();
                XmlNode rootNode = validXmlDocument.CreateNode(XmlNodeType.Element, Constants.USRXMLROOT, null);
                validXmlDocument.AppendChild(rootNode);


                using (TextReader reader = new StreamReader(strPath))
                {
                    List<XmlNode> xmlNodes = ((XmlNode[])fledeserializer.Deserialize(reader)).ToList();
                    List<XmlNode> validXmlNodes = new List<XmlNode>();

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
                                                                                      new XmlRootAttribute(Constants.USRXMLROOT)).Deserialize(xmlReader));
                    }
                };

                return lstUSRFiles;
            }
            catch (Exception)
            {
                fileMover.MoveFile(new string[] { strPath }, new string[] { ERROR, AppendTimeStamp(new FileInfo(strPath).Name) });
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted);
            }
        }

        /// <summary>
        /// Validate the XML file against the XSD file to check the sequence and data type.
        /// </summary>
        /// <param name="xsdFile"></param>
        /// <param name="xNode"></param>
        /// <returns></returns>
        private bool IsXmlValid(string xsdFile, XmlNode xNode)
        {

            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted);
            try
            {
                bool result = true;
                XDocument xDoc = XDocument.Load(new XmlNodeReader(xNode));
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add("", XmlReader.Create(xsdFile));

                xDoc.Validate(schemas, (o, e) =>
                {
                    int UDPRN = 0;
                    if (!string.IsNullOrEmpty(xDoc.Element(XName.Get(Constants.ADDRESSLOCATIONXMLROOT)).Element(XName.Get(Constants.USRUDPRN)).Value))
                    {
                        UDPRN = Convert.ToInt32(xDoc.Element(XName.Get(Constants.ADDRESSLOCATIONXMLROOT))
                                                            .Element(XName.Get(Constants.USRUDPRN)).Value);
                    }

                    loggingHelper.LogError(e.Exception);
                    //logger code to write schema mismatch exception 
                    result = false;
                });

                return result;
            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted);
            }
        }

        private string AppendTimeStamp(string strfileName)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(strfileName),
               string.Format(Constants.DATETIMEFORMAT, DateTime.Now),
                Path.GetExtension(strfileName)
                );
        }

        /// <summary>
        /// Method level entry exit logging.
        /// </summary>
        /// <param name="methodName">Function Name</param>
        /// <param name="logMessage">Message</param>
        private void LogMethodInfoBlock(string methodName, string logMessage)
        {
            this.loggingHelper.LogInfo(methodName + Constants.COLON + logMessage, this.enableLogging);
        }
    }
}
