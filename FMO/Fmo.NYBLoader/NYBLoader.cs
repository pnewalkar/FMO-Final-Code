using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Fmo.Common.Constants;
using Fmo.Common.Enums;
using Fmo.Common.Interface;
using Fmo.DTO;
using Fmo.NYBLoader.Interfaces;

namespace Fmo.NYBLoader
{
    /// <summary>
    /// Load and process NYb files
    /// </summary>
    public class NYBLoader : INYBLoader
    {
        #region private member declaration

        private static string dateTimeFormat = Constants.DATETIMEFORMAT;
        private static int noOfCharacters = 15;
        private static int maxCharacters = 507;
        private static int csvValues = 16;
        private string strFMOWebAPIName = string.Empty;
        private IHttpHandler httpHandler;
        private ILoggingHelper loggingHelper;
        private IExceptionHelper exceptionHelper;
        private string strProcessedFilePath = string.Empty;
        private string strErrorFilePath = string.Empty;
        private bool enableLogging = false;
        private string nybMessage = Constants.LOADNYBDETAILSLOGMESSAGE;
        private string nybInvalidDetailMessage = Constants.LOADNYBINVALIDDETAILS;

        #endregion private member declaration

        #region constructor

        public NYBLoader(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper, IExceptionHelper exceptionHelper)
        {
            this.httpHandler = httpHandler;
            this.strFMOWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.FMOWebAPIName).ToString() : string.Empty;
            this.loggingHelper = loggingHelper;
            this.exceptionHelper = exceptionHelper;
            this.strProcessedFilePath = configurationHelper.ReadAppSettingsConfigurationValues(Constants.ProcessedFilePath);
            this.strErrorFilePath = configurationHelper.ReadAppSettingsConfigurationValues(Constants.ErrorFilePath);
            this.enableLogging = Convert.ToBoolean(configurationHelper.ReadAppSettingsConfigurationValues(Constants.EnableLogging));
            this.enableLogging = Convert.ToBoolean(configurationHelper.ReadAppSettingsConfigurationValues(Constants.EnableLogging));
        }

        #endregion constructor

        #region public methods

        /// <summary>
        /// Read files from zip file and call NYBLoader Assembly to validate and save records
        /// </summary>
        /// <param name="fileName">Input file name as a param</param>
        public void LoadNYBDetails(string fileName)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);
            try
            {
                if (CheckFileName(new FileInfo(fileName).Name, Constants.PAFZIPFILENAME))
                {
                    using (ZipArchive zip = ZipFile.OpenRead(fileName))
                    {
                        foreach (ZipArchiveEntry entry in zip.Entries)
                        {
                            using (Stream stream = entry.Open())
                            {
                                var reader = new StreamReader(stream);
                                string strLine = reader.ReadToEnd();
                                string strfileName = entry.Name;
                                if (CheckFileName(new FileInfo(strfileName).Name, Constants.NYBFLATFILENAME))
                                {
                                    List<PostalAddressDTO> lstNYBDetails = LoadNybDetailsFromCsv(strLine.Trim());
                                    string postaLAddress = serializer.Serialize(lstNYBDetails);
                                    LogMethodInfoBlock(methodName, Constants.POSTALADDRESSDETAILS + postaLAddress, Constants.COLON);

                                    if (lstNYBDetails != null && lstNYBDetails.Count > 0)
                                    {
                                        var invalidRecordsCount = lstNYBDetails.Where(n => n.IsValidData == false).ToList().Count;

                                        if (invalidRecordsCount > 0)
                                        {
                                            File.WriteAllText(Path.Combine(strErrorFilePath, AppendTimeStamp(strfileName)), strLine);
                                            this.loggingHelper.LogInfo(string.Format(nybInvalidDetailMessage, strfileName, DateTime.Now.ToString()));
                                        }
                                        else
                                        {
                                            File.WriteAllText(Path.Combine(strProcessedFilePath, AppendTimeStamp(strfileName)), strLine);
                                            SaveNybDetails(lstNYBDetails, strfileName);
                                        }
                                    }
                                    else
                                    {
                                        File.WriteAllText(Path.Combine(strErrorFilePath, AppendTimeStamp(strfileName)), strLine);
                                        this.loggingHelper.LogInfo(string.Format(nybMessage, strfileName, DateTime.Now.ToString()));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
            }
        }

        /// <summary>
        /// Reads data from CSV file and maps to postalAddressDTO object
        /// </summary>
        /// <param name="strLine">Line read from CSV File</param>
        /// <returns>Postal Address DTO</returns>
        public List<PostalAddressDTO> LoadNybDetailsFromCsv(string line)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);
            List<PostalAddressDTO> lstAddressDetails = null;
            try
            {
                string[] arrPAFDetails = line.Split(new string[] { Constants.CRLF, Constants.NEWLINE }, StringSplitOptions.None);

                if (arrPAFDetails.Count() > 0 && ValidateFile(arrPAFDetails))
                {
                    lstAddressDetails = arrPAFDetails.Select(v => MapNybDetailsToDTO(v)).ToList();

                    if (lstAddressDetails != null && lstAddressDetails.Count > 0)
                    {
                        //Validate NYB Details ,validates each property of PostalAddressDTO as per the business rule and set the Value of IsValid property to either true
                        //or false.Depending on the count of IsValid property data wil either will be saved in DB or file will be moved to error folder.
                        ValidateNybDetails(lstAddressDetails);

                        //Remove Channel Island and Isle of Man Addresses are ones where the Postcode starts with one of: GY, JE or IM and Invalid records

                        lstAddressDetails = lstAddressDetails.Where(n => !n.Postcode.StartsWith(PostCodePrefix.GY.ToString(), StringComparison.OrdinalIgnoreCase) && !n.Postcode.StartsWith(PostCodePrefix.JE.ToString(), StringComparison.OrdinalIgnoreCase) && !n.Postcode.StartsWith(PostCodePrefix.IM.ToString(), StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
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
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);
            bool isNybDetailsInserted = false;
            try
            {
                isNybDetailsInserted = true;
                var result = await httpHandler.PostAsJsonAsync(strFMOWebAPIName + fileName, lstAddress);
            }
            catch (Exception ex)
            {
                this.loggingHelper.LogError(ex);
                isNybDetailsInserted = false;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
            }
            return isNybDetailsInserted;
        }

        #endregion public methods

        #region private methods

        /// <summary>
        /// Append timestamp to filename before writing the file to specified folder
        /// </summary>
        /// <param name="strfileName">path</param>
        /// <returns>Filename with timestamp appended</returns>
        private static string AppendTimeStamp(string strfileName)
        {
            return string.Concat(Path.GetFileNameWithoutExtension(strfileName), string.Format(dateTimeFormat, DateTime.Now), Path.GetExtension(strfileName));
        }

        /// <summary>
        /// Validates string i.e. no of comma's should be 15 and max characters per line should be 507
        /// </summary>
        /// <param name="arrLines">Array of string read from CSV file</param>
        /// <returns>boolean value after validating all the lines</returns>
        private bool ValidateFile(string[] arrLines)
        {
            bool isFileValid = true;
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);
            try
            {
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
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
            }

            return isFileValid;
        }

        /// <summary>
        /// Mapping comma separated value to postalAddressDTO object
        /// </summary>
        /// <param name="csvLine">Line read from CSV File</param>
        /// <returns>Returns mapped DTO</returns>
        private PostalAddressDTO MapNybDetailsToDTO(string csvLine)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);
            try
            {
                PostalAddressDTO objAddDTO = new PostalAddressDTO();
                string[] values = csvLine.Split(',');
                if (values.Count() == csvValues)
                {
                    objAddDTO.Postcode = values[0].Trim();
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
                    objAddDTO.DeliveryPointSuffix = values[15].Trim();
                    objAddDTO.IsValidData = true;
                }
                return objAddDTO;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
            }
        }

        /// <summary>
        /// Perform business validation on postalAddressDTO object
        /// </summary>
        /// <param name="lstAddress">List of mapped address dto to validate each records</param>
        private void ValidateNybDetails(List<PostalAddressDTO> lstAddress)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);
            try
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
            catch (Exception)
            {
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
            }
        }

        /// <summary>
        /// PostCode validation i.e start and end character should be numeric , fourth last character should be whitespace etc.
        /// </summary>
        /// <param name="strPostCode">Postcode read from csv file</param>
        /// <returns></returns>
        private bool ValidatePostCode(string strPostCode)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);
            try
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
            catch (Exception)
            {
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
            }
        }

        /// <summary>
        /// Method level entry exit logging.
        /// </summary>
        /// <param name="methodName">Function Name</param>
        /// <param name="logMessage">Message</param>
        /// <param name="seperator">seperator</param>
        private void LogMethodInfoBlock(string methodName, string logMessage, string seperator)
        {
            this.loggingHelper.LogInfo(methodName + seperator + logMessage, this.enableLogging);
        }

        /// <summary>
        /// Check file name is valid
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="regex">regular expression to pass</param>
        /// <returns>bool</returns>
        private bool CheckFileName(string fileName, string regex)
        {
            try
            {
                fileName = Path.GetFileNameWithoutExtension(fileName);
                Regex reg = new Regex(regex);
                return reg.IsMatch(fileName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion private methods
    }
}