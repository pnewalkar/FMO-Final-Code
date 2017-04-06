﻿using Fmo.DTO;
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

namespace Fmo.NYBLoader
{
    public class TPFLoader : ITPFLoader
    {
        private readonly IMessageBroker<AddressLocationUSRDTO> msgBroker;
        private readonly IFileMover fileMover;
        private string XSD_LOCATION = ConfigurationSettings.AppSettings["XSDLocation"];
        private string PROCESSED = ConfigurationSettings.AppSettings["ProcessedFilePath"];
        private string ERROR = ConfigurationSettings.AppSettings["ErrorFilePath"];


        public TPFLoader(IMessageBroker<AddressLocationUSRDTO> messageBroker, IFileMover fileMover)
        {
            this.msgBroker = messageBroker;
            this.fileMover = fileMover;
        }


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


                fileMover.MoveFile(new string[] { strPath }, new string[] { PROCESSED, new FileInfo(strPath).Name });

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
                throw ex;
            }
        }

        public List<AddressLocationUSRDTO> GetValidRecords(string strPath)
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
                        if (IsXmlValid(XSD_LOCATION, xmlNode))
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
            catch (Exception ex)
            {
                fileMover.MoveFile(new string[] { strPath }, new string[] { ERROR, new FileInfo(strPath).Name });

                throw ex;
            }

        }

        private bool IsXmlValid(string xsdFile, XmlNode xNode)
        {

            try
            {
                bool result = true;
                XDocument xDoc = XDocument.Load(new XmlNodeReader(xNode));
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add("", XmlReader.Create(xsdFile));

                xDoc.Validate(schemas, (o, e) =>
                {
                    //logger code to write schema mismatch exception 
                    result = false;
                });

                return result;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
