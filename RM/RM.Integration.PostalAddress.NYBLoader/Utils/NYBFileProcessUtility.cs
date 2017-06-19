namespace RM.Integration.PostalAddress.NYBLoader.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web.Script.Serialization;
    using CommonLibrary.Utilities.HelperMiddleware;
    using RM.CommonLibrary.ConfigurationMiddleware;
    using RM.CommonLibrary.EntityFramework.DTO;
    using RM.CommonLibrary.ExceptionMiddleware;
    using RM.CommonLibrary.HelperMiddleware;
    using RM.CommonLibrary.Interfaces;
    using RM.CommonLibrary.LoggingMiddleware;
    using RM.Integration.PostalAddress.NYBLoader.Utils.Interfaces;

    /// <summary>
    /// Load and process NYb files
    /// </summary>
    public class NYBFileProcessUtility : INYBFileProcessUtility
    {
        #region private member declaration

        private static string dateTimeFormat = Constants.DATETIMEFORMAT;
        private static int noOfCharacters = default(int);
        private static int maxCharacters = default(int);
        private static int csvValues = default(int);
        private string strFMOWebAPIName = string.Empty;
        private IHttpHandler httpHandler;
        private ILoggingHelper loggingHelper;
        private IExceptionHelper exceptionHelper;
        private string strProcessedFilePath = string.Empty;
        private string strErrorFilePath = string.Empty;
        private string nybMessage = Constants.LOADNYBDETAILSLOGMESSAGE;
        private string nybInvalidDetailMessage = Constants.LOADNYBINVALIDDETAILS;

        #endregion private member declaration

        #region constructor

        public NYBFileProcessUtility(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper, IExceptionHelper exceptionHelper)
        {
            this.httpHandler = httpHandler;
            this.strFMOWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.FMOWebAPIName).ToString() : string.Empty;
            this.loggingHelper = loggingHelper;
            this.exceptionHelper = exceptionHelper;
            this.strProcessedFilePath = configurationHelper.ReadAppSettingsConfigurationValues(Constants.ProcessedFilePath);
            this.strErrorFilePath = configurationHelper.ReadAppSettingsConfigurationValues(Constants.ErrorFilePath);
            noOfCharacters = configurationHelper != null ? Convert.ToInt32(configurationHelper.ReadAppSettingsConfigurationValues(Constants.NoOfCharactersForNYB)) : default(int);
            maxCharacters = configurationHelper != null ? Convert.ToInt32(configurationHelper.ReadAppSettingsConfigurationValues(Constants.maxCharactersForNYB)) : default(int);
            csvValues = configurationHelper != null ? Convert.ToInt32(configurationHelper.ReadAppSettingsConfigurationValues(Constants.csvValuesForNYB)) : default(int);
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
                                        this.loggingHelper.Log(string.Format(nybInvalidDetailMessage, strfileName, DateTime.Now.ToString()), TraceEventType.Information, null);
                                    }
                                    else
                                    {
                                        File.WriteAllText(Path.Combine(strProcessedFilePath, AppendTimeStamp(strfileName)), strLine);
                                        var result = SaveNybDetails(lstNYBDetails, strfileName);
                                    }
                                }
                                else
                                {
                                    File.WriteAllText(Path.Combine(strErrorFilePath, AppendTimeStamp(strfileName)), strLine);
                                    this.loggingHelper.Log(string.Format(nybMessage, strfileName, DateTime.Now.ToString()), TraceEventType.Information, null);
                                }
                            }
                        }
                    }
                }
            }

            LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
        }

        /// <summary>
        /// Reads data from CSV file and maps to postalAddressDTO object
        /// </summary>
        /// <param name="line">Line read from CSV File</param>
        /// <returns>Postal Address DTO</returns>
        public List<PostalAddressDTO> LoadNybDetailsFromCsv(string line)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);
            List<PostalAddressDTO> lstAddressDetails = null;

            string[] arrPAFDetails = line.Split(new string[] { Constants.CRLF, Constants.NEWLINE }, StringSplitOptions.None);

            if (arrPAFDetails.Count() > 0 && ValidateFile(arrPAFDetails))
            {
                lstAddressDetails = arrPAFDetails.Select(v => MapNybDetailsToDTO(v)).ToList();

                if (lstAddressDetails != null && lstAddressDetails.Count > 0)
                {
                    // Validate NYB Details ,validates each property of PostalAddressDTO as per
                    // the business rule and set the Value of IsValid property to either true or
                    // false.Depending on the count of IsValid property data wil either will be
                    // saved in DB or file will be moved to error folder.
                    ValidateNybDetails(lstAddressDetails);

                    // Remove Channel Island and Isle of Man Addresses are ones where the
                    // Postcode starts with one of: GY, JE or IM and Invalid records
                    lstAddressDetails = lstAddressDetails.Where(n => !n.Postcode.StartsWith(PostCodePrefix.GY.ToString(), StringComparison.OrdinalIgnoreCase) && !n.Postcode.StartsWith(PostCodePrefix.JE.ToString(), StringComparison.OrdinalIgnoreCase) && !n.Postcode.StartsWith(PostCodePrefix.IM.ToString(), StringComparison.OrdinalIgnoreCase)).ToList();
                }
            }

            LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);

            return lstAddressDetails;
        }

        /// <summary>
        /// Web API call to save postalAddress to PostalAddress table
        /// </summary>
        /// <param name="lstAddress">List of mapped address dto to validate each records</param>
        /// <param name="fileName">File Name</param>
        /// <returns>If success returns true else returns false</returns>
        public async Task<bool> SaveNybDetails(List<PostalAddressDTO> lstAddress, string fileName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Service.SaveNybDetails"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);
                bool isNybDetailsInserted = false;
                try
                {
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NYBPriority, LoggerTraceConstants.NYBLoaderMethodEntryEventId, LoggerTraceConstants.Title);
                    isNybDetailsInserted = true;
                    var result = await httpHandler.PostAsJsonAsync(strFMOWebAPIName + fileName, lstAddress, isBatchJob: true);

                    if (!result.IsSuccessStatusCode)
                    {
                        var responseContent = result.ReasonPhrase;
                        this.loggingHelper.Log(responseContent, TraceEventType.Error);
                        isNybDetailsInserted = false;
                    }
                }
                catch (Exception ex)
                {
                    this.loggingHelper.Log(ex, TraceEventType.Error);
                    isNybDetailsInserted = false;
                }
                finally
                {
                    LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.NYBPriority, LoggerTraceConstants.NYBLoaderMethodExitEventId, LoggerTraceConstants.Title);
                }

                return isNybDetailsInserted;
            }
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

            LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);

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

            PostalAddressDTO objAddDTO = new PostalAddressDTO();
            string[] values = csvLine.Split(',');
            if (values.Count() == csvValues)
            {
                objAddDTO.Postcode = values[Constants.NYBPostcode].Trim();
                objAddDTO.PostTown = values[Constants.NYBPostTown];
                objAddDTO.DependentLocality = values[Constants.NYBDependentLocality];
                objAddDTO.DoubleDependentLocality = values[Constants.NYBDoubleDependentLocality];
                objAddDTO.Thoroughfare = values[Constants.NYBThoroughfare];
                objAddDTO.DependentThoroughfare = values[Constants.NYBDependentThoroughfare];
                objAddDTO.BuildingNumber = !string.IsNullOrEmpty(values[Constants.NYBBuildingNumber]) && !string.IsNullOrWhiteSpace(values[Constants.NYBBuildingNumber]) ? Convert.ToInt16(values[Constants.NYBBuildingNumber]) : Convert.ToInt16(0);
                objAddDTO.BuildingName = values[Constants.NYBBuildingName];
                objAddDTO.SubBuildingName = values[Constants.NYBSubBuildingName];
                objAddDTO.POBoxNumber = values[Constants.NYBPOBoxNumber];
                objAddDTO.DepartmentName = values[Constants.NYBDepartmentName];
                objAddDTO.OrganisationName = values[Constants.NYBOrganisationName];
                objAddDTO.UDPRN = !string.IsNullOrEmpty(values[Constants.NYBUDPRN]) || !string.IsNullOrWhiteSpace(values[Constants.NYBUDPRN]) ? Convert.ToInt32(values[Constants.NYBUDPRN]) : 0;
                objAddDTO.PostcodeType = values[Constants.NYBPostcodeType];
                objAddDTO.SmallUserOrganisationIndicator = values[Constants.NYBSmallUserOrganisationIndicator];
                objAddDTO.DeliveryPointSuffix = values[Constants.NYBDeliveryPointSuffix].Trim();
                objAddDTO.IsValidData = true;
            }

            LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
            return objAddDTO;
        }

        /// <summary>
        /// Perform business validation on postalAddressDTO object
        /// </summary>
        /// <param name="lstAddress">List of mapped address dto to validate each records</param>
        private void ValidateNybDetails(List<PostalAddressDTO> lstAddress)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);

            foreach (PostalAddressDTO objAdd in lstAddress)
            {
                if (string.IsNullOrEmpty(objAdd.Postcode) || !ValidatePostCode(objAdd.Postcode))
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

                if (!string.Equals(objAdd.SmallUserOrganisationIndicator, PostcodeType.Y.ToString(), StringComparison.OrdinalIgnoreCase) && objAdd.SmallUserOrganisationIndicator != " ")
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

                    if (characters.Count() == 2)
                    {
                        if (!char.IsLetter(characters[1]) || !char.IsNumber(characters[0]))
                        {
                            objAdd.IsValidData = false;
                        }
                    }
                    else
                    {
                        objAdd.IsValidData = false;
                    }
                }
            }

            LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
        }

        /// <summary>
        /// PostCode validation i.e start and end character should be numeric , fourth last character
        /// should be whitespace etc.
        /// </summary>
        /// <param name="strPostCode">Postcode read from csv file</param>
        /// <returns>state of post validation</returns>
        private bool ValidatePostCode(string strPostCode)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);

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

            LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
            return isValid;
        }

        /// <summary>
        /// Method level entry exit logging.
        /// </summary>
        /// <param name="methodName">Function Name</param>
        /// <param name="logMessage">Message</param>
        /// <param name="seperator">seperator</param>
        private void LogMethodInfoBlock(string methodName, string logMessage, string seperator)
        {
            loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressDetailsPriority, LoggerTraceConstants.GetPostalAddressDetailsBusinessMethodEntryEventId, LoggerTraceConstants.Title);
        }

        /// <summary>
        /// Check file name is valid
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="regex">regular expression to pass</param>
        /// <returns>bool</returns>
        private bool CheckFileName(string fileName, string regex)
        {
            fileName = Path.GetFileNameWithoutExtension(fileName);
            Regex reg = new Regex(regex);
            return reg.IsMatch(fileName);
        }

        #endregion private methods
    }
}