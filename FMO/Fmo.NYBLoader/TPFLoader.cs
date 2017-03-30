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

        private readonly IKernel kernal;
        private readonly IMessageBroker<USR> msgBroker;
        private const string XSD_LOCATION = @"D:\Software\Jitendra\FMO-AD\FMO\Fmo.NYBLoader\ReferenceSchema\XMLSchema1.xsd";
        private bool isDataValid = false;


        public TPFLoader(IMessageBroker<USR> messageBroker)
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
            List<USR> lstUSRFiles = null;
            List<USR> lstUSRInsertFiles = null;
            List<USR> lstUSRUpdateFiles = null;
            List<USR> lstUSRDeleteFiles = null;


            try
            {
                XmlSerializer fledeserializer = new XmlSerializer(typeof(List<USR>), new XmlRootAttribute(Constants.USR_XML_ROOT));

                using (TextReader reader = new StreamReader(strPath))
                {
                    lstUSRFiles = (List<USR>)fledeserializer.Deserialize(reader);
                    object o = fledeserializer.Deserialize(reader);

                    ValidateXml(XSD_LOCATION, strPath);

                    if (!isDataValid)
                    {
                        return;
                    }

                    lstUSRInsertFiles = lstUSRFiles.Where(insertFiles => insertFiles.CHANGE_TYPE == Constants.INSERT).ToList();
                    lstUSRUpdateFiles = lstUSRFiles.Where(updateFiles => updateFiles.CHANGE_TYPE == Constants.UPDATE).ToList();
                    lstUSRDeleteFiles = lstUSRFiles.Where(deleteFiles => deleteFiles.CHANGE_TYPE == Constants.DELETE).ToList();
                };

                lstUSRInsertFiles.ForEach(USR =>
                {
                    string xmlUSR = SerializeObject<USR>(USR);
                    IMessage USRMsg = msgBroker.CreateMessage(xmlUSR, Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH);
                    msgBroker.SendMessage(USRMsg);
                });

                lstUSRUpdateFiles.ForEach(USR =>
                {
                    string xmlUSR = SerializeObject<USR>(USR);
                    IMessage USRMsg = msgBroker.CreateMessage(xmlUSR, Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH);
                    msgBroker.SendMessage(USRMsg);
                });

                lstUSRDeleteFiles.ForEach(USR =>
                {
                    string xmlUSR = SerializeObject<USR>(USR);
                    IMessage USRMsg = msgBroker.CreateMessage(xmlUSR, Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH);
                    msgBroker.SendMessage(USRMsg);
                });

            }

            catch (Exception ex)
            {
                throw;
            }
        }

        private void ValidateXml(string xsdFile, string xmlFile)
        {

            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add("http://tempuri.org/AddressLocationSchema.xsd", xsdFile);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(xmlFile, settings);

            // Parse the file. 
            while (reader.Read()) ;

            isDataValid = true;
        }

        private void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
                Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + e.Message);
            else if (e.Severity == XmlSeverityType.Error)
                Console.WriteLine("\tValidation error: " + e.Message);
        }

        private string SerializeObject<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
    }
}
