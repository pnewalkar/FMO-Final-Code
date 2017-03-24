using Fmo.NYBLoader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DTO;
using System.IO;
using System.IO.Compression;

namespace Fmo.NYBLoader
{
    public class NYBLoader : INYBLoader
    {
        public void LoadNYBDetailsFromCSV(string strPath)
        {
            List<PostalAddress> lstAddressDetails = null;
            try
            {
                ZipArchive zip = ZipFile.OpenRead(strPath);


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
                        lstAddressDetails = arrPAFDetails.Select(v => MapNYBDetailsToDTO(v)).ToList();


                        if (lstAddressDetails != null && lstAddressDetails.Count > 0)
                        {

                            //Validate NYB Details
                            ValidateNYBDetails(lstAddressDetails);

                            //Remove Channel Island and Isle of Man Addresses are ones where the Postcode starts with one of: GY, JE or IM and Invalid records

                            lstAddressDetails = lstAddressDetails.SkipWhile(n => (n.Postcode.StartsWith("GY") || n.Postcode.StartsWith("JE") || n.Postcode.StartsWith("IM"))).ToList();

                            var invalidRecordCount = lstAddressDetails.Where(n => n.IsValidData == false).ToList().Count;

                            if (invalidRecordCount > 0)
                            {
                                File.WriteAllText(Path.Combine("Error file", AppendTimeStamp(strfileName)), strLine);
                            }
                            else
                            {
                                File.WriteAllText(Path.Combine("Processed file", AppendTimeStamp(strfileName)), strLine);
                            }

                        }
                    }
                    else
                    {
                        //TO DO
                        //Log error
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool ValidateFile(string[] arrLines)
        {
            bool isFileValid = true;
            try
            {
                foreach (string line in arrLines)
                {
                    if (line.Count(n => n == ',') != 15)
                    {
                        isFileValid = false;
                        break;
                    }
                    if (line.ToCharArray().Count() > 507)
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

        private PostalAddress MapNYBDetailsToDTO(string csvLine)
        {
            PostalAddress objAddDTO = new PostalAddress();
            try
            {
                string[] values = csvLine.Split(',');
                if (values.Count() == 16)
                {
                    objAddDTO.Postcode = values[0];
                    objAddDTO.PostTown = values[1];
                    objAddDTO.DependentLocality = values[2];
                    objAddDTO.DoubleDependentLocality = values[3];
                    objAddDTO.Thoroughfare = values[4];
                    objAddDTO.DependentThoroughfare = values[5];
                    objAddDTO.BuildingNumber = !string.IsNullOrEmpty(values[6]) && !string.IsNullOrWhiteSpace(values[6]) ? Convert.ToInt16(values[6]) : Convert.ToInt16(0);
                    objAddDTO.BuildingName = values[7];
                    objAddDTO.SubBuildingName = values[8];
                    objAddDTO.POBoxNumber = values[9];
                    objAddDTO.DepartmentName = values[10];
                    objAddDTO.OrganisationName = values[11];
                    objAddDTO.UDPRN = !string.IsNullOrEmpty(values[12]) && !string.IsNullOrWhiteSpace(values[12]) ? Convert.ToInt32(values[12]) : 0;
                    objAddDTO.PostcodeType = values[13];
                    objAddDTO.SmallUserOrganisationIndicator = values[14];
                    objAddDTO.DeliveryPointSuffix = values[15];
                    objAddDTO.IsValidData = true;
                }

            }
            catch (Exception)
            {

                throw;
            }



            return objAddDTO;
        }

        private void ValidateNYBDetails(List<PostalAddress> lstAddress)
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
