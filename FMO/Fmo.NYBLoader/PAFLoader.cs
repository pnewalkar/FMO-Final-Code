using Fmo.NYBLoader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DTO;
using System.IO;
using System.IO.Compression;
using Ninject;
using System.Xml.Serialization;
using Fmo.MessageBrokerCore.Messaging;
using System.Configuration;
using Fmo.MessageBrokerCore;
using Fmo.Common;
using Fmo.Common.Fmo.Constants;

namespace Fmo.NYBLoader
{
    public class PAFLoader : IPAFLoader
    {
        private static string strProcessedFilePath = ConfigurationSettings.AppSettings["ProcessedFilePath"].ToString();
        private static string strErrorFilePath = ConfigurationSettings.AppSettings["ErrorFilePath"].ToString();
        //private readonly IKernel kernal;
        private readonly IMessageBroker<PostalAddressDTO> msgBroker;

        public PAFLoader(IMessageBroker<PostalAddressDTO> messageBroker)
        {
            this.msgBroker = messageBroker;
        }

        public void LoadPAFDetailsFromCSV(string strPath)
        {
            List<PostalAddressDTO> lstAddressDetails = null;
            try
            {
                using (ZipArchive zip = ZipFile.OpenRead(strPath))
                {
                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        string strLine = string.Empty;
                        string strfileName = string.Empty;

                        using (Stream stream = entry.Open())
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                strLine = reader.ReadToEnd();
                            }
                        }
                        strfileName = entry.Name;

                        string[] arrPAFDetails = strLine.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                        Array.Resize(ref arrPAFDetails, arrPAFDetails.Length - 1);

                        if (arrPAFDetails.Count() > 0 && ValidateFile(arrPAFDetails))
                        {
                            //lstAddressDetails = arrPAFDetails.Select(v => MapPAFDetailsToDTO(v)).ToList();
                            lstAddressDetails = new List<PostalAddressDTO>
                        {
                            new PostalAddressDTO() {UDPRN =1, DepartmentName="A", AmendmentType="B"},
                            new PostalAddressDTO() {UDPRN =1, DepartmentName="B", AmendmentType="C"},
                            new PostalAddressDTO() {UDPRN =1, DepartmentName="C", AmendmentType="D"},
                            new PostalAddressDTO() {UDPRN =1, DepartmentName="D", AmendmentType="I"},
                            new PostalAddressDTO() {UDPRN =2, DepartmentName="E", AmendmentType="I"},
                            new PostalAddressDTO() {UDPRN =3, DepartmentName="F", AmendmentType="I"},
                            new PostalAddressDTO() {UDPRN =4, DepartmentName="G", AmendmentType="I"},
                        };


                            if (lstAddressDetails != null && lstAddressDetails.Count > 0)
                            {

                                //Validate PAF Details
                                ValidatePAFDetails(lstAddressDetails);

                                //Remove Channel Island and Isle of Man Addresses are ones where the Postcode starts with one of: GY, JE or IM and Invalid records
                                lstAddressDetails = lstAddressDetails.SkipWhile(n => (n.Postcode.StartsWith("GY") || n.Postcode.StartsWith("JE") || n.Postcode.StartsWith("IM"))).ToList();

                                //Remove duplicate PAF events which have create and delete instance for same UDPRN
                                lstAddressDetails = lstAddressDetails
                                                        .SkipWhile(n => (n.UDPRN.Equals("B")))
                                                        .GroupBy(x => x.UDPRN)
                                                        .Where(g => g.Count() == 1)
                                                        .SelectMany(g => g.Select(o => o))
                                                        .ToList();

                                var invalidRecordCount = lstAddressDetails.Where(n => n.IsValidData == false).ToList().Count;

                                if (invalidRecordCount > 0)
                                {
                                    File.WriteAllText(Path.Combine(strErrorFilePath, AppendTimeStamp(strfileName)), strLine);
                                }
                                else
                                {
                                    foreach (var addDetail in lstAddressDetails)
                                    {
                                        string strXml = SerializeObject<PostalAddressDTO>(addDetail);
                                        IMessage msg = msgBroker.CreateMessage(strXml, Constants.QUEUE_PAF, Constants.QUEUE_PATH);
                                        msgBroker.SendMessage(msg);
                                    }
                                    File.WriteAllText(Path.Combine(strProcessedFilePath, AppendTimeStamp(strfileName)), strLine);
                                }
                            }
                        }
                        else
                        {
                            File.WriteAllText(Path.Combine("Error file", strfileName), strLine);
                            //TO DO
                            //Log error
                        }
                    }
                }


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

        private string SerializeObject<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        private T DeserializeXMLFileToObject<T>(string xmlFilename)
        {
            T returnObject = default(T);
            if (string.IsNullOrEmpty(xmlFilename))
            {
                return default(T);
            }

            StreamReader xmlStream = new StreamReader(xmlFilename);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            returnObject = (T)serializer.Deserialize(xmlStream);

            return returnObject;
        }

        private bool ValidateFile(string[] arrLines)
        {
            bool isFileValid = true;
            try
            {
                foreach (string line in arrLines)
                {
                    if (line.Count(n => n == ',') != 19)
                    {
                        isFileValid = false;
                        break;
                    }
                    if (line.ToCharArray().Count() > 534)
                    {
                        isFileValid = false;
                        break;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return isFileValid;
        }

        private PostalAddressDTO MapPAFDetailsToDTO(string csvLine)
        {
            PostalAddressDTO objAddDTO = new PostalAddressDTO();
            try
            {
                string[] values = csvLine.Split(',');
                if (values.Count() == 20)
                {
                    objAddDTO.Date = values[0];
                    objAddDTO.Time = values[1];
                    objAddDTO.AmendmentType = values[2];
                    objAddDTO.AmendmentDesc = values[3];
                    objAddDTO.Postcode = values[4];
                    objAddDTO.PostTown = values[5];
                    objAddDTO.DependentLocality = values[6];
                    objAddDTO.DoubleDependentLocality = values[7];
                    objAddDTO.Thoroughfare = values[8];
                    objAddDTO.DependentThoroughfare = values[9];
                    objAddDTO.BuildingNumber = !string.IsNullOrEmpty(values[10]) && !string.IsNullOrWhiteSpace(values[10]) ? Convert.ToInt16(values[10]) : Convert.ToInt16(0);
                    objAddDTO.BuildingName = values[11];
                    objAddDTO.SubBuildingName = values[12];
                    objAddDTO.POBoxNumber = values[13];
                    objAddDTO.DepartmentName = values[14];
                    objAddDTO.OrganisationName = values[15];
                    objAddDTO.UDPRN = !string.IsNullOrEmpty(values[16]) && !string.IsNullOrWhiteSpace(values[16]) ? Convert.ToInt32(values[16]) : 0;
                    objAddDTO.PostcodeType = values[17];
                    objAddDTO.SmallUserOrganisationIndicator = values[18];
                    objAddDTO.DeliveryPointSuffix = values[19];
                    objAddDTO.IsValidData = true;
                }

            }
            catch (Exception)
            {

                throw;
            }



            return objAddDTO;
        }

        private void ValidatePAFDetails(List<PostalAddressDTO> lstAddress)
        {

            try
            {
                foreach (PostalAddressDTO objAdd in lstAddress)
                {
                    if ((string.IsNullOrEmpty(objAdd.Postcode)) && !ValidatePostCode(objAdd.Postcode))
                    {
                        objAdd.IsValidData = false;
                    }
                    if (string.IsNullOrEmpty(objAdd.PostTown))
                    {
                        objAdd.IsValidData = false;
                    }
                    if (string.IsNullOrEmpty(objAdd.PostcodeType) && (objAdd.PostcodeType != "S" || objAdd.PostcodeType != "L"))
                    {
                        objAdd.IsValidData = false;
                    }
                    if (string.IsNullOrEmpty(Convert.ToString(objAdd.UDPRN)))
                    {
                        objAdd.IsValidData = false;
                    }
                    if (string.IsNullOrEmpty(objAdd.SmallUserOrganisationIndicator) && (objAdd.PostcodeType != "Y" || objAdd.PostcodeType != " "))
                    {
                        objAdd.IsValidData = false;
                    }
                    if (string.IsNullOrEmpty(objAdd.DeliveryPointSuffix))
                    {
                        objAdd.IsValidData = false;
                    }
                    if (!string.IsNullOrEmpty(objAdd.DeliveryPointSuffix))
                    {
                        char[] characters = objAdd.DeliveryPointSuffix.ToCharArray();
                        if (objAdd.PostcodeType == "L" && objAdd.DeliveryPointSuffix != "1A")
                        {
                            objAdd.IsValidData = false;
                        }
                        if (characters.Count() != 2)
                        {
                            objAdd.IsValidData = false;
                        }
                        else if (characters.Count() == 2)
                        {
                            if (!char.IsLetter(characters[1]) && !char.IsNumber(characters[0]))
                            {
                                objAdd.IsValidData = false;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool ValidatePostCode(string strPostCode)
        {
            bool isValid = true;
            try
            {
                if (!string.IsNullOrEmpty(strPostCode))
                {
                    char[] chrCodes = strPostCode.ToCharArray();
                    if (chrCodes.Length > 6)
                    {
                        int length = chrCodes.Length;
                        if (!char.IsWhiteSpace(chrCodes[0]) && !char.IsNumber(chrCodes[0]) && char.IsLetter(chrCodes[0])
                            && !char.IsNumber(chrCodes[length - 1]) && !char.IsNumber(chrCodes[length - 2]) && char.IsWhiteSpace(chrCodes[length - 4]))
                        {
                            isValid = true;
                        }
                        else
                        {
                            isValid = false;
                        }

                    }
                    else
                    {
                        isValid = false;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return isValid;
        }
    }
}
