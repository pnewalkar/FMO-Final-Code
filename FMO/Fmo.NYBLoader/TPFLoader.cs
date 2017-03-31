using Fmo.DTO;
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
        private readonly IMessageBroker<addressLocation> msgBroker;
        private const string XSD_LOCATION = @"D:\Software\Jitendra\FMO-AD\FMO\Fmo.NYBLoader\ReferenceSchema\USRFileSchema.xsd";


        public TPFLoader(IMessageBroker<addressLocation> messageBroker)
        {
            //kernal = new StandardKernel();
            //Register(kernal);
            this.msgBroker = messageBroker;
        }

        protected static void Register(IKernel kernel)
        {
            //kernel.Bind<IMessageBroker>().To<MessageBroker>().InSingletonScope();
        }
        //protected T Get<T>()
        //{
        //    return kernal.Get<T>();
        //}

        public void LoadTPFDetailsFromXML(string strPath)
        {
            List<addressLocation> lstUSRFiles = null;
            List<addressLocation> lstUSRInsertFiles = null;
            List<addressLocation> lstUSRUpdateFiles = null;
            List<addressLocation> lstUSRDeleteFiles = null;


            try
            {
                XmlSerializer fledeserializer = new XmlSerializer(typeof(object), new XmlRootAttribute(Constants.USR_XML_ROOT));
                XmlDocument validXmlDocument = new XmlDocument();
                XmlNode rootNode = validXmlDocument.CreateNode(XmlNodeType.Element, Constants.USR_XML_ROOT, null);
                validXmlDocument.AppendChild(rootNode);


                using (TextReader reader = new StreamReader(strPath))
                {
                    //lstUSRFiles = (List<USR>)fledeserializer.Deserialize(reader);
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
                        lstUSRFiles = (List<addressLocation>)(new XmlSerializer(typeof(List<addressLocation>), new XmlRootAttribute(Constants.USR_XML_ROOT)).Deserialize(xmlReader));    
                    }
                };

                lstUSRInsertFiles = lstUSRFiles.Where(insertFiles => insertFiles.changeType == Constants.INSERT).ToList();
                lstUSRUpdateFiles = lstUSRFiles.Where(updateFiles => updateFiles.changeType == Constants.UPDATE).ToList();
                lstUSRDeleteFiles = lstUSRFiles.Where(deleteFiles => deleteFiles.changeType == Constants.DELETE).ToList();

                lstUSRInsertFiles.ForEach(addressLocation =>
                {
                    string xmlUSR = SerializeObject<addressLocation>(addressLocation);
                    IMessage USRMsg = msgBroker.CreateMessage(xmlUSR, Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH);
                    msgBroker.SendMessage(USRMsg);
                });


                //Code to be uncommented after 
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
                throw;
            }
        }

        private bool IsXmlValid(string xsdFile, XmlNode xNode)
        {

            try
            {
                bool result = true;
                XDocument xDoc = XDocument.Load(new XmlNodeReader(xNode));
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add("http://tempuri.org/AddressLocationSchema.xsd", xsdFile);
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
