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
using Fmo.Common.Constants;
using Fmo.Common.Enums;
using Fmo.Common.Interface;

namespace Fmo.NYBLoader
{
    public class PAFLoader : IPAFLoader
    {
        private string strPAFProcessedFilePath = string.Empty;
        private string strPAFErrorFilePath = string.Empty;
        private readonly IMessageBroker<PostalAddressDTO> msgBroker;
        private IConfigurationHelper configurationHelper;
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public PAFLoader(IMessageBroker<PostalAddressDTO> messageBroker, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.msgBroker = messageBroker;
            this.configurationHelper = configurationHelper;
            this.loggingHelper = loggingHelper;
            this.strPAFProcessedFilePath = configurationHelper.ReadAppSettingsConfigurationValues("PAFProcessedFilePath");
            this.strPAFErrorFilePath    = configurationHelper.ReadAppSettingsConfigurationValues("PAFErrorFilePath");
        }

        public void LoadPAF(string fileName)
        {
            try
            {
                using (ZipArchive zip = ZipFile.OpenRead(fileName))
                {
                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        string strLine = string.Empty;
                        string strfileName = string.Empty;
                        Stream stream = entry.Open();
                        var reader = new StreamReader(stream);
                        strLine = reader.ReadToEnd();
                        strfileName = entry.Name;

                        List<PostalAddressDTO> lstPAFDetails = ProcessPAF(strLine, strfileName);
                        if (lstPAFDetails != null && lstPAFDetails.Count > 0)
                        {
                            var invalidRecordsCount = lstPAFDetails.Where(n => n.IsValidData == false).ToList().Count;

                            if (invalidRecordsCount > 0)
                            {
                                File.WriteAllText(Path.Combine(strPAFErrorFilePath, AppendTimeStamp(strfileName)), strLine);
                            }
                            else
                            {
                                if (SavePAFDetails(lstPAFDetails))
                                {
                                    File.WriteAllText(Path.Combine(strPAFProcessedFilePath, AppendTimeStamp(strfileName)), strLine);
                                }
                                else
                                {
                                    File.WriteAllText(Path.Combine(strPAFErrorFilePath, AppendTimeStamp(strfileName)), strLine);
                                }
                            }
                        }
                        else
                        {
                            File.WriteAllText(Path.Combine(strPAFErrorFilePath, AppendTimeStamp(strfileName)), strLine);
                            //TO DO Log error... File validation error
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public List<PostalAddressDTO> ProcessPAF(string strLine, string strFileName)
        {
            List<PostalAddressDTO> lstAddressDetails = null;
            try
            {
                string[] arrPAFDetails = strLine.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                if (string.IsNullOrEmpty(arrPAFDetails[arrPAFDetails.Length - 1]))
                {
                    Array.Resize(ref arrPAFDetails, arrPAFDetails.Length - 1);
                }

                if (arrPAFDetails.Count() > 0 && ValidateFile(arrPAFDetails))
                {
                    lstAddressDetails = arrPAFDetails.Select(v => MapPAFDetailsToDTO(v, strFileName)).ToList();

                    if (lstAddressDetails != null && lstAddressDetails.Count > 0)
                    {

                        //Validate PAF Details
                        ValidatePAFDetails(lstAddressDetails);

                        //Remove Channel Island and Isle of Man Addresses are ones where the Postcode starts with one of: GY, JE or IM and Invalid records
                        lstAddressDetails = lstAddressDetails.SkipWhile(n => (n.Postcode.StartsWith("GY") || n.Postcode.StartsWith("JE") || n.Postcode.StartsWith("IM"))).ToList();

                        //Remove duplicate PAF events which have create and delete instance for same UDPRN
                        lstAddressDetails = lstAddressDetails
                                                .SkipWhile(n => (n.UDPRN.Equals(Constants.PAFNOACTION)))
                                                .GroupBy(x => x.UDPRN)
                                                .Where(g => g.Count() == 1)
                                                .SelectMany(g => g.Select(o => o))
                                                .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                loggingHelper.LogError(ex);
            }
            return lstAddressDetails;            
        }

        public bool SavePAFDetails(List<PostalAddressDTO> lstPostalAddress)
        {
            bool saveflag = false;
            try
            {
                saveflag = true;
                
                var lstPAFInsertEvents = lstPostalAddress.Where(insertFiles => insertFiles.AmendmentType == Constants.PAFINSERT).ToList();  
                var lstPAFUpdateEvents = lstPostalAddress.Where(updateFiles => updateFiles.AmendmentType == Constants.PAFUPDATE).ToList();
                var lstPAFDeleteEvents = lstPostalAddress.Where(deleteFiles => deleteFiles.AmendmentType == Constants.PAFDELETE).ToList();

                lstPAFInsertEvents.ForEach(postalAddress =>
                    {
                        IMessage msg = msgBroker.CreateMessage(postalAddress, Constants.QUEUE_PAF, Constants.QUEUE_PATH);
                        msgBroker.SendMessage(msg);
                    });

                //Sprint 1- Only create events has to be executed
                /*
                lstPAFUpdateEvents.ForEach(postalAddress =>
                {
                    IMessage msg = msgBroker.CreateMessage(postalAddress, Constants.QUEUE_PAF, Constants.QUEUE_PATH);
                    msgBroker.SendMessage(msg);
                });

                lstPAFDeleteEvents.ForEach(postalAddress =>
                {
                    IMessage msg = msgBroker.CreateMessage(postalAddress, Constants.QUEUE_PAF, Constants.QUEUE_PATH);
                    msgBroker.SendMessage(msg);
                });*/
            }
            catch (Exception)
            {
                saveflag = false;
            }
            return saveflag;
        }

        private static string AppendTimeStamp(string strfileName)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(strfileName),
               string.Format("{0:-yyyy-MM-d-HH-mm-ss}", DateTime.Now),
                Path.GetExtension(strfileName)
                );
        }

        private static bool ValidateFile(string[] arrLines)
        {
            bool isFileValid = true;
            foreach (string line in arrLines)
            {
                if (line.Count(n => n == ',') != Constants.noOfCharactersForPAF)
                {
                    isFileValid = false;
                    break;
                }
                if (line.ToCharArray().Count() > Constants.maxCharactersForPAF)
                {
                    isFileValid = false;
                    break;
                }
            }
            return isFileValid;
        }

        private static PostalAddressDTO MapPAFDetailsToDTO(string csvLine, string FileName)
        {
            PostalAddressDTO objAddDTO = new PostalAddressDTO();
            string[] values = csvLine.Split(',');
            if (values.Count() == Constants.csvPAFValues)
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
                objAddDTO.FileName = FileName;
            }
            return objAddDTO;
        }

        private static void ValidatePAFDetails(List<PostalAddressDTO> lstAddress)
        {
            foreach (PostalAddressDTO objAdd in lstAddress)
            {
                if (string.IsNullOrEmpty(objAdd.AmendmentType) || !(System.Enum.IsDefined(typeof(AmmendmentType), objAdd.AmendmentType)))
                {
                    objAdd.IsValidData = false;
                    objAdd.InValidRemarks = objAdd.InValidRemarks + "" + ",";
                }
                if (string.IsNullOrEmpty(objAdd.PostTown))
                {
                    objAdd.IsValidData = false;
                    objAdd.InValidRemarks = objAdd.InValidRemarks + "PostTown is " + ",";
                }
                if ((string.IsNullOrEmpty(objAdd.Postcode)) || !ValidatePostCode(objAdd.Postcode))
                {
                    objAdd.IsValidData = false;
                    objAdd.InValidRemarks = objAdd.InValidRemarks + "" + ",";
                }
                if (string.IsNullOrEmpty(objAdd.PostTown))
                {
                    objAdd.IsValidData = false;
                    objAdd.InValidRemarks = objAdd.InValidRemarks + "" + ",";
                }
                if (string.IsNullOrEmpty(objAdd.PostcodeType) || (!string.Equals(objAdd.PostcodeType, PostcodeType.S.ToString(), StringComparison.OrdinalIgnoreCase) && !string.Equals(objAdd.PostcodeType, PostcodeType.L.ToString(), StringComparison.OrdinalIgnoreCase)))
                {
                    objAdd.IsValidData = false;
                    objAdd.InValidRemarks = objAdd.InValidRemarks + "" + ",";
                }
                if (string.IsNullOrEmpty(Convert.ToString(objAdd.UDPRN == 0 ? null : objAdd.UDPRN)))
                {
                    objAdd.IsValidData = false;
                    objAdd.InValidRemarks = objAdd.InValidRemarks + "" + ",";
                }
                if ((!string.Equals(objAdd.SmallUserOrganisationIndicator, PostcodeType.Y.ToString(), StringComparison.OrdinalIgnoreCase) && objAdd.SmallUserOrganisationIndicator != " "))
                {
                    objAdd.IsValidData = false;
                    objAdd.InValidRemarks = objAdd.InValidRemarks + "" + ",";
                }
                if (string.IsNullOrEmpty(objAdd.DeliveryPointSuffix))
                {
                    objAdd.IsValidData = false;
                    objAdd.InValidRemarks = objAdd.InValidRemarks + "" + ",";
                }
                if (!string.IsNullOrEmpty(objAdd.DeliveryPointSuffix))
                {
                    char[] characters = objAdd.DeliveryPointSuffix.ToCharArray();
                    if (string.Equals(objAdd.PostcodeType, PostcodeType.L.ToString(), StringComparison.OrdinalIgnoreCase) && !string.Equals(objAdd.DeliveryPointSuffix, Constants.DeliveryPointSuffix, StringComparison.OrdinalIgnoreCase))
                    {
                        objAdd.IsValidData = false;
                        objAdd.InValidRemarks = objAdd.InValidRemarks + "" + ",";
                    }
                    if (characters.Count() != 2)
                    {
                        objAdd.IsValidData = false;
                        objAdd.InValidRemarks = objAdd.InValidRemarks + "" + ",";
                    }
                    else if (characters.Count() == 2)
                    {
                        if (!char.IsLetter(characters[1]) && !char.IsNumber(characters[0]))
                        {
                            objAdd.IsValidData = false;
                            objAdd.InValidRemarks = objAdd.InValidRemarks + "" + ",";
                        }
                    }
                }
            }
        }

        private static bool ValidatePostCode(string strPostCode)
        {
            bool isValid = true;
            if (!string.IsNullOrEmpty(strPostCode))
            {
                char[] chrCodes = strPostCode.ToCharArray();
                if (chrCodes.Length >= 5)
                {
                    int length = chrCodes.Length;
                    if (char.IsLetter(chrCodes[0]) && char.IsLetter(chrCodes[length - 1]) && char.IsLetter(chrCodes[length - 2]) 
                            && char.IsNumber(chrCodes[length - 3]) && char.IsWhiteSpace(chrCodes[length - 4]))
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
            return isValid;
        }
    }
}
