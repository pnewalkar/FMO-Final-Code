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

namespace Fmo.NYBLoader
{
    public class TPFLoader : ITPFLoader
    {
        private readonly IMessageBroker<AddressLocationUSRDTO> msgBroker;
        private const string XSD_LOCATION = @"C:\Workspace\FMO\FMO\Fmo.NYBLoader\Schemas\USRFileSchema.xsd";


        public TPFLoader(IMessageBroker<AddressLocationUSRDTO> messageBroker)
        {
            this.msgBroker = messageBroker;
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
                    string xmlUSR = SerializeObject<AddressLocationUSRDTO>(addressLocation);
                    IMessage USRMsg = msgBroker.CreateMessage(xmlUSR, Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH);
                    msgBroker.SendMessage(USRMsg);
                });

                destinationPath = Path.Combine(new FileInfo(strPath).Directory.FullName, Constants.PROCESSED_FOLDER, new FileInfo(strPath).Name);

                if (!Directory.Exists(Path.Combine(new FileInfo(strPath).Directory.FullName, Constants.PROCESSED_FOLDER)))
                {
                    Directory.CreateDirectory(Path.Combine(new FileInfo(strPath).Directory.FullName, Constants.PROCESSED_FOLDER));
                }

                File.Move(strPath, destinationPath);

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
                string destinationPath = Path.Combine(new FileInfo(strPath).Directory.FullName, Constants.Error_FOLDER, new FileInfo(strPath).Name);

                if (!Directory.Exists(Path.Combine(new FileInfo(strPath).Directory.FullName, Constants.Error_FOLDER)))
                {
                    Directory.CreateDirectory(Path.Combine(new FileInfo(strPath).Directory.FullName, Constants.Error_FOLDER));
                }

                File.Move(strPath, destinationPath);

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

        private string SerializeObject<T>(T toSerialize)
        {
            try
            {

                XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, toSerialize);
                    return textWriter.ToString();
                }

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
