using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;
using Fmo.NYBLoader.Interfaces;
using Fmo.Common.Enums;
using Fmo.Common.Constants;
using Fmo.Common.Interface;

namespace Fmo.NYBLoader
{
    /// <summary>
    /// Load and process NYb files 
    /// </summary>
    public class NYBLoader : INYBLoader
    {
        #region private member declaration
        private static int noOfCharacters = 15;
        private static int maxCharacters = 507;
        private static int csvValues = 16;
        private string strFMOWebAPIName = string.Empty;
        private IHttpHandler httpHandler;
        private ILoggingHelper loggingHelper;
        private IExceptionHelper exceptionHelper;
        #endregion

        #region constructor
        public NYBLoader(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper, IExceptionHelper exceptionHelper)
        {
            this.httpHandler = httpHandler;
            this.strFMOWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.FMOWebAPIName).ToString() : string.Empty;
            this.loggingHelper = loggingHelper;
            this.exceptionHelper = exceptionHelper;
        }
        #endregion

        #region public methods
        /// <summary>
        /// Reads data from CSV file and maps to postalAddressDTO object
        /// </summary>
        /// <param name="strLine">Line read from CSV File</param>
        /// <returns>Postal Address DTO</returns>
        public List<PostalAddressDTO> LoadNybDetailsFromCSV(string line)
        {
            List<PostalAddressDTO> lstAddressDetails = null;
            try
            {
                string[] arrPAFDetails = line.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

                if (arrPAFDetails.Count() > 0 && ValidateFile(arrPAFDetails))
                {
                    lstAddressDetails = arrPAFDetails.Select(v => MapNybDetailsToDTO(v)).ToList();

                    if (lstAddressDetails != null && lstAddressDetails.Count > 0)
                    {
                        //Validate NYB Details ,validates each property of PostalAddressDTO as per the business rule and set the Value of IsValid property to either true 
                        //or false.Depending on the count of IsValid property data wil either will be saved in DB or file will be moved to error folder.
                        ValidateNybDetails(lstAddressDetails);

                        //Remove Channel Island and Isle of Man Addresses are ones where the Postcode starts with one of: GY, JE or IM and Invalid records

                        lstAddressDetails = lstAddressDetails.SkipWhile(n => (n.Postcode.StartsWith(PostCodePrefix.GY.ToString()) || n.Postcode.StartsWith(PostCodePrefix.JE.ToString()) || n.Postcode.StartsWith(PostCodePrefix.IM.ToString()))).ToList();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lstAddressDetails;
        }

        /// <summary>
        /// Web API call to save postalAddress to PostalAddress table
        /// </summary>
        /// <param name="lstAddress">List of mapped address dto to validate each records</param>
        /// <returns>If success returns true else returns false</returns>
        public async Task<bool> SaveNybDetails(List<PostalAddressDTO> lstAddress, string fileName)
        {
            bool saveflag = false;
            try
            {
                saveflag = true;
                var result = await httpHandler.PostAsJsonAsync(strFMOWebAPIName + fileName, lstAddress);
            }
            catch (Exception ex)
            {
                this.loggingHelper.LogError(ex);
                saveflag = false;
            }
            return saveflag;
        }
        #endregion

        #region private methods
        /// <summary>
        /// Validates string i.e. no of comma's should be 15 and max characters per line should be 507
        /// </summary>
        /// <param name="arrLines">Array of string read from CSV file</param>
        /// <returns>boolean value after validating all the lines</returns>
        private static bool ValidateFile(string[] arrLines)
        {
            bool isFileValid = true;
            foreach (string line in arrLines)
            {
                if (line.Count(n => n == ',') != noOfCharacters)
                {
                    isFileValid = false;
                    break;
                }
                if (line.ToCharArray().Count() > maxCharacters)
                {
                    isFileValid = false;
                    break;
                }
            }
            return isFileValid;
        }

        /// <summary>
        /// Mapping comma separated value to postalAddressDTO object
        /// </summary>
        /// <param name="csvLine">Line read from CSV File</param>
        /// <returns>Returns mapped DTO</returns>
        private static PostalAddressDTO MapNybDetailsToDTO(string csvLine)
        {
            PostalAddressDTO objAddDTO = new PostalAddressDTO();
            string[] values = csvLine.Split(',');
            if (values.Count() == csvValues)
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
                objAddDTO.UDPRN = !string.IsNullOrEmpty(values[12]) || !string.IsNullOrWhiteSpace(values[12]) ? Convert.ToInt32(values[12]) : 0;
                objAddDTO.PostcodeType = values[13];
                objAddDTO.SmallUserOrganisationIndicator = values[14];
                objAddDTO.DeliveryPointSuffix = values[15];
                objAddDTO.IsValidData = true;
            }
            return objAddDTO;
        }

        /// <summary>
        /// Perform business validation on postalAddressDTO object
        /// </summary>
        /// <param name="lstAddress">List of mapped address dto to validate each records</param>
        private static void ValidateNybDetails(List<PostalAddressDTO> lstAddress)
        {
            foreach (PostalAddressDTO objAdd in lstAddress)
            {
                if ((string.IsNullOrEmpty(objAdd.Postcode)) || !ValidatePostCode(objAdd.Postcode))
                {
                    objAdd.IsValidData = false;
                }
                if (string.IsNullOrEmpty(objAdd.PostTown))
                {
                    objAdd.IsValidData = false;
                }
                if (string.IsNullOrEmpty(objAdd.PostcodeType) || (!string.Equals(objAdd.PostcodeType, PostcodeType.S.ToString(), StringComparison.OrdinalIgnoreCase) && !string.Equals(objAdd.PostcodeType, PostcodeType.L.ToString(), StringComparison.OrdinalIgnoreCase)))
                {
                    objAdd.IsValidData = false;
                }
                if (objAdd.UDPRN == 0)
                {
                    objAdd.IsValidData = false;
                }
                if ((!string.Equals(objAdd.SmallUserOrganisationIndicator, PostcodeType.Y.ToString(), StringComparison.OrdinalIgnoreCase) && objAdd.SmallUserOrganisationIndicator != " "))
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
                    if (string.Equals(objAdd.PostcodeType, PostcodeType.L.ToString(), StringComparison.OrdinalIgnoreCase) && !string.Equals(objAdd.DeliveryPointSuffix, Constants.DeliveryPointSuffix, StringComparison.OrdinalIgnoreCase))
                    {
                        objAdd.IsValidData = false;
                    }
                    if (characters.Count() != 2)
                    {
                        objAdd.IsValidData = false;
                    }
                    else if (characters.Count() == 2)
                    {
                        if (!char.IsLetter(characters[1]) || !char.IsNumber(characters[0]))
                        {
                            objAdd.IsValidData = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// PostCode validation i.e start and end character should be numeric , fourth last character should be whitespace etc.
        /// </summary>
        /// <param name="strPostCode">Postcode read from csv file</param>
        /// <returns></returns>
        private static bool ValidatePostCode(string strPostCode)
        {
            bool isValid = true;
            if (!string.IsNullOrEmpty(strPostCode))
            {
                char[] chrCodes = strPostCode.ToCharArray();
                if (chrCodes.Length >= 5)
                {
                    int length = chrCodes.Length;
                    if (char.IsLetter(chrCodes[0]) && char.IsLetter(chrCodes[length - 1]) && char.IsLetter(chrCodes[length - 2]) && char.IsNumber(chrCodes[length - 3]) && char.IsWhiteSpace(chrCodes[length - 4]))
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
        #endregion

    }
}