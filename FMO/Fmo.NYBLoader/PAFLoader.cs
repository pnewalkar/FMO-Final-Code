using Fmo.NYBLoader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DTO;
using System.IO;

namespace Fmo.NYBLoader
{
    public class PAFLoader : IPAFLoader
    {
        public List<PostalAddress> LoadPAFDetailsFromCSV(string strPath)
        {
            List<PostalAddress> lstAddressDetails = null;
            try
            {
                //  string strPAFDetails = File.ReadAllText(strPath, Encoding.ASCII);
                string[] arrPAFDetails = File.ReadAllLines(strPath, Encoding.ASCII);//strNybDetails.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

                if (arrPAFDetails.Count() > 0 && ValidateFile(arrPAFDetails))
                {
                    lstAddressDetails = arrPAFDetails.Select(v => MapPAFDetailsToDTO(v)).ToList();


                    if (lstAddressDetails != null && lstAddressDetails.Count > 0)
                    {

                        //Validate PAF Details
                        ValidatePAFDetails(lstAddressDetails);

                        //Remove Channel Island and Isle of Man Addresses are ones where the Postcode starts with one of: GY, JE or IM and Invalid records

                        lstAddressDetails = lstAddressDetails.SkipWhile(n => (n.Postcode.StartsWith("GY") || n.Postcode.StartsWith("JE") || n.Postcode.StartsWith("IM")) && n.IsValidData == false).ToList();

                    }
                }
                else
                {
                    //TO DO
                    //Log error
                }
            }
            catch (Exception)
            {

                throw;
            }
            return lstAddressDetails;
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

        private PostalAddress MapPAFDetailsToDTO(string csvLine)
        {
            PostalAddress objAddDTO = new PostalAddress();
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
                }

            }
            catch (Exception)
            {

                throw;
            }



            return objAddDTO;
        }

        private void ValidatePAFDetails(List<PostalAddress> lstAddress)
        {

            try
            {
                foreach (PostalAddress objAdd in lstAddress)
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
