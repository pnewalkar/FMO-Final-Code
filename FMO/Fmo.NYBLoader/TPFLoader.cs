using Fmo.DTO.FileProcessing;
using Fmo.MessageBrokerCore.Messaging;
using Fmo.NYBLoader.Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Fmo.Common.Constants;
using System.Xml;
using System.Xml.Schema;
using System.Configuration;
using Fmo.Common.Interface;
using Fmo.Common.Enums;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

namespace Fmo.NYBLoader
{
    public class TPFLoader : ITPFLoader
    {
        private readonly IMessageBroker<AddressLocationUSRDTO> msgBroker;
        private readonly IFileMover fileMover;
        private readonly IExceptionHelper exceptionHelper;
        private readonly ILoggingHelper loggingHelper;
        private readonly IConfigurationHelper configHelper;
        private IFileProcessingLogRepository fileProcessingLogRepository = default(IFileProcessingLogRepository);
        private string XSD_LOCATION;
        private string PROCESSED;
        private string ERROR;


        public TPFLoader(IMessageBroker<AddressLocationUSRDTO> messageBroker, IFileMover fileMover, IExceptionHelper exceptionHelper, ILoggingHelper loggingHelper, IConfigurationHelper configHelper, IFileProcessingLogRepository fileProcessingLogRepository)
        {
            this.msgBroker = messageBroker;
            this.fileMover = fileMover;
            this.exceptionHelper = exceptionHelper;
            this.loggingHelper = loggingHelper;
            this.configHelper = configHelper;
            this.fileProcessingLogRepository = fileProcessingLogRepository;
            this.XSD_LOCATION = configHelper.ReadAppSettingsConfigurationValues("XSDLocation");
            this.PROCESSED = configHelper.ReadAppSettingsConfigurationValues("TPFProcessedFilePath");
            this.ERROR = configHelper.ReadAppSettingsConfigurationValues("TPFErrorFilePath");
        }

        /// <summary>
        /// Load the XML data from file to Message Queue.
        /// </summary>
        /// <param name="strPath"></param>
        public void LoadTPFDetailsFromXML(string strPath)
        {
            string destinationPath = string.Empty;
            List <AddressLocationUSRDTO> lstUSRFiles = null;
            List<AddressLocationUSRDTO> lstUSRInsertFiles = null;
            List<AddressLocationUSRDTO> lstUSRUpdateFiles = null;
            List<AddressLocationUSRDTO> lstUSRDeleteFiles = null;


            try
            {
                lstUSRFiles = GetValidRecords(strPath);

                lstUSRInsertFiles = lstUSRFiles.Where(insertFiles => insertFiles.changeType == Constants.INSERT).ToList();
                lstUSRUpdateFiles = lstUSRFiles.Where(updateFiles => updateFiles.changeType == Constants.UPDATE).ToList();
                lstUSRDeleteFiles = lstUSRFiles.Where(deleteFiles => deleteFiles.changeType == Constants.DELETE).ToList();

                lstUSRInsertFiles.ForEach(addressLocation =>
                {
                    IMessage USRMsg = msgBroker.CreateMessage(addressLocation, Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH);
                    msgBroker.SendMessage(USRMsg);
                });


                fileMover.MoveFile(new string[] { strPath }, new string[] { PROCESSED, AppendTimeStamp(new FileInfo(strPath).Name) });

                //Code to be uncommented after confirmation
                /*lstUSRUpdateFiles.ForEach(addressLocation =>
                {
                    string xmlUSR = SerializeObject<addressLocation>(addressLocation);
                    IMessage USRMsg = msgBroker.CreateMessage(xmlUSR, Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH);
                    msgBroker.SendMessage(USRMsg);
                });

                lstUSRDeleteFiles.ForEach(addressLocation =>
                {
                    string xmlUSR = SerializeObject<addressLocation>(addressLocation);
                    IMessage USRMsg = msgBroker.CreateMessage(xmlUSR, Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH);
                    msgBroker.SendMessage(USRMsg);
                });*/

            }

            catch (Exception ex)
            {
                Exception newException;
                exceptionHelper.HandleException(ex, ExceptionHandlingPolicy.LogAndWrap, out newException);
                bool rethrow = exceptionHelper.HandleException(ex, ExceptionHandlingPolicy.LogAndWrap, out newException);
                if (rethrow)
                {
                    if (newException == null)
                    {
                        throw;

                    }
                    else
                    {
                        throw newException;
                    }
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Return the valid records after file validation.
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        private List<AddressLocationUSRDTO> GetValidRecords(string strPath)
        {

            try
            {
                List<AddressLocationUSRDTO> lstUSRFiles = new List<AddressLocationUSRDTO>();

                XmlSerializer fledeserializer = new XmlSerializer(typeof(object), new XmlRootAttribute(Constants.USR_XML_ROOT));
                XmlDocument validXmlDocument = new XmlDocument();
                XmlNode rootNode = validXmlDocument.CreateNode(XmlNodeType.Element, Constants.USR_XML_ROOT, null);
                validXmlDocument.AppendChild(rootNode);


                using (TextReader reader = new StreamReader(strPath))
                {
                    List<XmlNode> xmlNodes = ((XmlNode[])fledeserializer.Deserialize(reader)).ToList();
                    List<XmlNode> validXmlNodes = new List<XmlNode>();

                    xmlNodes.ForEach(xmlNode =>
                    {
                        if (IsXmlValid(new FileInfo(strPath).Name, XSD_LOCATION, xmlNode))
                        {
                            validXmlNodes.Add(xmlNode);
                        }

                    });
                    validXmlNodes.ForEach(xmlNode =>
                    {
                        XmlNode newNode = validXmlDocument.ImportNode(xmlNode, true);
                        rootNode.AppendChild(newNode);
                    });

                    using (XmlReader xmlReader = XmlReader.Create(new StringReader(validXmlDocument.OuterXml)))
                    {
                        xmlReader.MoveToContent();
                        lstUSRFiles = (List<AddressLocationUSRDTO>)(new XmlSerializer(typeof(List<AddressLocationUSRDTO>), new XmlRootAttribute(Constants.USR_XML_ROOT)).Deserialize(xmlReader));    
                    }
                };

                return lstUSRFiles;
            }
            catch (Exception)
            {
                fileMover.MoveFile(new string[] { strPath }, new string[] { ERROR, AppendTimeStamp(new FileInfo(strPath).Name) });
                throw ;
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

            try
            {
                bool result = true;
                XDocument xDoc = XDocument.Load(new XmlNodeReader(xNode));
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add("", XmlReader.Create(xsdFile));

                xDoc.Validate(schemas, (o, e) =>
                {
                    FileProcessingLogDTO objFileProcessingLog = new FileProcessingLogDTO();
                    objFileProcessingLog.FileID = Guid.NewGuid();
                    objFileProcessingLog.UDPRN = Convert.ToInt32(xDoc.Element(XName.Get("addressLocation")).Element(XName.Get("udprn")).Value);
                    objFileProcessingLog.AmendmentType = xDoc.Element(XName.Get("addressLocation")).Element(XName.Get("changeType")).Value;
                    objFileProcessingLog.FileName = fileName;
                    objFileProcessingLog.FileProcessing_TimeStamp = DateTime.Now;
                    objFileProcessingLog.FileType = FileType.Usr.ToString();

                    if (e.Severity == XmlSeverityType.Warning)
                    {
                        objFileProcessingLog.NatureOfError = e.Message;
                    }
                    else if(e.Severity == XmlSeverityType.Error)
                    {
                        objFileProcessingLog.NatureOfError = e.Message;
                    }

                    fileProcessingLogRepository.LogFileException(objFileProcessingLog);
                    //logger code to write schema mismatch exception 
                    result = false;
                });

                return result;
            }

            catch (Exception)
            {
                throw;
            }
        }

        private string AppendTimeStamp(string strfileName)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(strfileName),
               string.Format("{0:-yyyy-MM-d-HH-mm-ss}", DateTime.Now),
                Path.GetExtension(strfileName)
                );
        }
    }
}
