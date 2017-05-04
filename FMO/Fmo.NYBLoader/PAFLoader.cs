namespace Fmo.NYBLoader
{
    using Fmo.Common.Constants;
    using Fmo.Common.Enums;
    using Fmo.Common.Interface;
    using Fmo.DTO;
    using Fmo.MessageBrokerCore.Messaging;
    using Fmo.NYBLoader.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Load, Validate and process file
    /// </summary>
    public class PAFLoader : IPAFLoader
    {
        #region private member declaration

        private static string dateTimeFormat = Constants.DATETIMEFORMAT;
        private string strPAFProcessedFilePath = string.Empty;
        private string strPAFErrorFilePath = string.Empty;
        private readonly IMessageBroker<PostalAddressDTO> msgBroker;
        private IConfigurationHelper configurationHelper;
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private bool enableLogging = false;

        #endregion private member declaration

        #region constructor

        public PAFLoader(IMessageBroker<PostalAddressDTO> messageBroker, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.msgBroker = messageBroker;
            this.configurationHelper = configurationHelper;
            this.loggingHelper = loggingHelper;
            this.strPAFProcessedFilePath = configurationHelper.ReadAppSettingsConfigurationValues(Constants.PAFProcessedFilePath);
            this.strPAFErrorFilePath = configurationHelper.ReadAppSettingsConfigurationValues(Constants.PAFErrorFilePath);
        }

        #endregion constructor

        #region public methods

        /// <summary>
        /// Read files from zip file and validate and save records
        /// </summary>
        /// <param name="fileName">Input file name as a param</param>
        public void LoadPAF(string fileName)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);
            try
            {
                if (CheckFileName(new FileInfo(fileName).Name, Constants.PAFZIPFILENAME.ToString()))
                {
                    using (var file = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        using (ZipArchive zip = new ZipArchive(file, ZipArchiveMode.Read))
                        {
                            foreach (ZipArchiveEntry entry in zip.Entries)
                            {
                                string strLine = string.Empty;
                                string strfileName = string.Empty;
                                using (Stream stream = entry.Open())
                                {
                                    var reader = new StreamReader(stream);
                                    strLine = reader.ReadToEnd();
                                    strfileName = entry.Name;

                                    if (CheckFileName(new FileInfo(strfileName).Name, Constants.PAFFLATFILENAME))
                                    {
                                        List<PostalAddressDTO> lstPAFDetails = ProcessPAF(strLine, strfileName);
                                        if (lstPAFDetails != null && lstPAFDetails.Count > 0)
                                        {
                                            var invalidRecordsCount = lstPAFDetails.Where(n => n.IsValidData == false).ToList().Count;

                                            if (invalidRecordsCount > 0)
                                            {
                                                File.WriteAllText(Path.Combine(strPAFErrorFilePath, AppendTimeStamp(strfileName)), strLine);
                                                this.loggingHelper.LogInfo(string.Format(Constants.LOGMESSAGEFORPAFDATAVALIDATION, strfileName, DateTime.UtcNow.ToString()));
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
                                                    this.loggingHelper.LogInfo(string.Format(Constants.ERRORLOGMESSAGEFORPAFMSMQ, strfileName, DateTime.UtcNow.ToString()));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            File.WriteAllText(Path.Combine(strPAFErrorFilePath, AppendTimeStamp(strfileName)), strLine);
                                            this.loggingHelper.LogInfo(string.Format(Constants.LOGMESSAGEFORPAFWRONGFORMAT, strfileName, DateTime.UtcNow.ToString()));
                                        }
                                    }
                                    /* Code filev move on FileName Error
                                    else
                                    {
                                        File.WriteAllText(Path.Combine(strPAFErrorFilePath, AppendTimeStamp(strfileName)), strLine);
                                        this.loggingHelper.LogInfo(string.Format(Constants.ERRORLOGMESSAGEFORPAFFileName, strfileName, DateTime.UtcNow.ToString()));
                                    }*/
                                }
                            }
                        }
                    }
                }
                
            }
            catch (System.Exception)
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
        /// <param name="strFileName">FileName required to track the error in db against each records</param>
        /// <returns>Postal Address DTO</returns>
        public List<PostalAddressDTO> ProcessPAF(string line, string strFileName)
        {
            List<PostalAddressDTO> lstAddressDetails = null;
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);
            try
            {
                string[] arrPAFDetails = line.Split(new string[] { Constants.CRLF, Constants.NEWLINE }, StringSplitOptions.None);

                if (arrPAFDetails.Count() > 0 && ValidateFile(arrPAFDetails))
                {
                    lstAddressDetails = arrPAFDetails.Select(v => MapPAFDetailsToDTO(v, strFileName)).ToList();

                    if (lstAddressDetails != null && lstAddressDetails.Count > 0)
                    {
                        //Validate PAF Details
                        ValidatePAFDetails(lstAddressDetails);

                        //Remove Channel Island and Isle of Man Addresses are ones where the Postcode starts with one of: GY, JE or IM and Invalid records
                        lstAddressDetails = lstAddressDetails
                            .Where(n => !n.Postcode.StartsWith(PostCodePrefix.GY.ToString(), StringComparison.OrdinalIgnoreCase) && !n.Postcode.StartsWith(PostCodePrefix.JE.ToString(), StringComparison.OrdinalIgnoreCase) && !n.Postcode.StartsWith(PostCodePrefix.IM.ToString(), StringComparison.OrdinalIgnoreCase)).ToList();

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
        /// Sent PAF create event to MSMQ
        /// </summary>
        /// <param name="lstPostalAddress">List of mapped address dto to validate each records</param>
        /// <returns>If success returns true else returns false</returns>
        public bool SavePAFDetails(List<PostalAddressDTO> lstPostalAddress)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);
            bool isMessageQueued = false;
            try
            {
                isMessageQueued = true;

                var lstPAFInsertEvents = lstPostalAddress.Where(insertFiles => insertFiles.AmendmentType == Constants.PAFINSERT).ToList();

                lstPAFInsertEvents.ForEach(postalAddress =>
                    {
                        IMessage msg = msgBroker.CreateMessage(postalAddress, Constants.QUEUEPAF, Constants.QUEUEPATH);
                        msgBroker.SendMessage(msg);
                    });
            }
            catch (Exception ex)
            {
                this.loggingHelper.LogError(ex);
                isMessageQueued = false;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
            }
            return isMessageQueued;
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
            return string.Concat(
                Path.GetFileNameWithoutExtension(strfileName),
               string.Format(dateTimeFormat, DateTime.UtcNow),
                Path.GetExtension(strfileName)
                );
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

        /// <summary>
        /// Validates string i.e. no of comma's should be 19 and max characters per line should be 534
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
                    if (line.Count(n => n == ',') != Constants.NoOfCharactersForPAF)
                    {
                        isFileValid = false;
                        break;
                    }
                    if (line.ToCharArray().Count() > Constants.MaxCharactersForPAF)
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
        private PostalAddressDTO MapPAFDetailsToDTO(string csvLine, string FileName)
        {
            PostalAddressDTO objAddDTO = new PostalAddressDTO();
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);
            try
            {
                string[] values = csvLine.Split(',');
                if (values.Count() == Constants.CsvPAFValues)
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
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON );
            }
            return objAddDTO;
        }

        /// <summary>
        /// Perform business validation on postalAddressDTO object
        /// </summary>
        /// <param name="lstAddress">List of mapped address dto to validate each records</param>
        private void ValidatePAFDetails(List<PostalAddressDTO> lstAddress)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);
            try
            {
                foreach (PostalAddressDTO objAdd in lstAddress)
                {
                    if (string.IsNullOrEmpty(objAdd.AmendmentType) || !(System.Enum.IsDefined(typeof(AmmendmentType), objAdd.AmendmentType)))
                    {
                        objAdd.IsValidData = false;
                    }
                    if (string.IsNullOrEmpty(objAdd.PostTown))
                    {
                        objAdd.IsValidData = false;
                    }
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
                    if (string.IsNullOrEmpty(Convert.ToString(objAdd.UDPRN == 0 ? null : objAdd.UDPRN)))
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
            bool isValid = true;
            try
            {
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
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
            }
            return isValid;
        }

        /// <summary>
        /// Method level entry exit logging.
        /// </summary>
        /// <param name="methodName">Function Name</param>
        /// <param name="logMessage">Message</param>
        /// <param name="seperator">Seperator used</param>
        private void LogMethodInfoBlock(string methodName, string logMessage, string seperator)
        {
            this.loggingHelper.LogInfo(methodName + seperator + logMessage, this.enableLogging);
        }

        #endregion private methods
    }
}