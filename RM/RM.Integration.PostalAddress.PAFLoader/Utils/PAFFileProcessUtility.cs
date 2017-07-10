namespace RM.Integration.PostalAddress.PAFLoader.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web.Script.Serialization;
    using CommonLibrary.ConfigurationMiddleware;
    using CommonLibrary.EntityFramework.DTO;
    using CommonLibrary.HelperMiddleware;
    using CommonLibrary.LoggingMiddleware;
    using CommonLibrary.MessageBrokerMiddleware;
    using Interfaces;

    /// <summary>
    /// Load, Validate and process file
    /// </summary>
    public class PAFFileProcessUtility : IPAFFileProcessUtility
    {
        #region private member declaration

        private static string dateTimeFormat = PAFLoaderConstants.DATETIMEFORMAT;
        private static int noOfCharactersForPAF = default(int); // Constants.NoOfCharactersForNYB; // 15;
        private static int maxCharactersForPAF = default(int); // Constants.maxCharactersForNYB; // 507;
        private static int csvPAFValues = default(int); // Constants.csvValuesForNYB; // 16;
        private readonly IMessageBroker<PostalAddressDTO> msgBroker;
        private string strPAFProcessedFilePath = string.Empty;
        private string strPAFErrorFilePath = string.Empty;
        private IConfigurationHelper configurationHelper;
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        #endregion private member declaration

        #region constructor

        public PAFFileProcessUtility(IMessageBroker<PostalAddressDTO> messageBroker, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.msgBroker = messageBroker;
            this.configurationHelper = configurationHelper;
            this.loggingHelper = loggingHelper;
            this.strPAFProcessedFilePath = configurationHelper.ReadAppSettingsConfigurationValues(PAFLoaderConstants.PAFProcessedFilePath);
            this.strPAFErrorFilePath = configurationHelper.ReadAppSettingsConfigurationValues(PAFLoaderConstants.PAFErrorFilePath);
            noOfCharactersForPAF = configurationHelper != null ? Convert.ToInt32(configurationHelper.ReadAppSettingsConfigurationValues(PAFLoaderConstants.NoOfCharactersForPAF)) : default(int);
            maxCharactersForPAF = configurationHelper != null ? Convert.ToInt32(configurationHelper.ReadAppSettingsConfigurationValues(PAFLoaderConstants.MaxCharactersForPAF)) : default(int);
            csvPAFValues = configurationHelper != null ? Convert.ToInt32(configurationHelper.ReadAppSettingsConfigurationValues(PAFLoaderConstants.CsvPAFValues)) : default(int);
        }

        #endregion constructor

        #region public methods

        /// <summary>
        /// Waits until a file can be opened with write permission
        /// <param name="fileName">file to be processed</param>
        /// </summary>
        public void WaitReady(string fileName)
        {
            bool isPAFFileProcessed = false;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);

            while (true)
            {
                try
                {
                    if (CheckFileName(new FileInfo(fileName).Name, Constants.PAFZIPFILENAME.ToString()))
                    {
                        using (ZipArchive zip = ZipFile.OpenRead(fileName))
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
                                        List<PostalAddressDTO> lstPAFDetails = ProcessPAF(strLine.Trim(), strfileName);
                                        string postaLAddress = serializer.Serialize(lstPAFDetails);
                                        LogMethodInfoBlock(methodName, Constants.POSTALADDRESSDETAILS + postaLAddress, Constants.COLON);

                                        if (lstPAFDetails != null && lstPAFDetails.Count > 0)
                                        {
                                            var invalidRecordsCount = lstPAFDetails.Where(n => n.IsValidData == false).ToList().Count;

                                            if (invalidRecordsCount > 0)
                                            {
                                                File.WriteAllText(Path.Combine(strPAFErrorFilePath, AppendTimeStamp(strfileName)), strLine);
                                                this.loggingHelper.Log(string.Format(Constants.LOGMESSAGEFORPAFDATAVALIDATION, strfileName, DateTime.UtcNow.ToString()), TraceEventType.Information, null);
                                            }
                                            else
                                            {
                                                if (SavePAFDetails(lstPAFDetails))
                                                {
                                                    File.WriteAllText(Path.Combine(strPAFProcessedFilePath, AppendTimeStamp(strfileName)), strLine);
                                                    isPAFFileProcessed = true;
                                                }
                                                else
                                                {
                                                    File.WriteAllText(Path.Combine(strPAFErrorFilePath, AppendTimeStamp(strfileName)), strLine);
                                                    this.loggingHelper.Log(string.Format(Constants.ERRORLOGMESSAGEFORPAFMSMQ, strfileName, DateTime.UtcNow.ToString()), TraceEventType.Information, null);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            File.WriteAllText(Path.Combine(strPAFErrorFilePath, AppendTimeStamp(strfileName)), strLine);
                                            this.loggingHelper.Log(string.Format(Constants.LOGMESSAGEFORPAFWRONGFORMAT, strfileName, DateTime.UtcNow.ToString()), TraceEventType.Information, null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (FileNotFoundException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} not yet ready ({1})", fileName, ex.Message));
                }
                catch (IOException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} not yet ready ({1})", fileName, ex.Message));
                }
                catch (UnauthorizedAccessException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("Output file {0} not yet ready ({1})", fileName, ex.Message));
                }

                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Read files from zip file and validate and save records
        /// </summary>
        /// <param name="fileName">Input file name as a param</param>
        /// <returns>File Name</returns>
        public bool LoadPAF(string fileName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Service.LoadPAF"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PAFPriority, LoggerTraceConstants.PAFLoaderMethodEntryEventId, LoggerTraceConstants.Title);

                bool isPAFFileProcessed = false;
                JavaScriptSerializer serializer = new JavaScriptSerializer() { MaxJsonLength = 5000000 };

                if (CheckFileName(new FileInfo(fileName).Name, PAFLoaderConstants.PAFZIPFILENAME.ToString()))
                {
                    using (ZipArchive zip = ZipFile.OpenRead(fileName))
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

                                if (CheckFileName(new FileInfo(strfileName).Name, PAFLoaderConstants.PAFFLATFILENAME))
                                {
                                    List<PostalAddressDTO> lstPAFDetails = ProcessPAF(strLine.Trim(), strfileName);
                                    string postaLAddress = serializer.Serialize(lstPAFDetails);
                                    LogMethodInfoBlock(methodName, PAFLoaderConstants.POSTALADDRESSDETAILS + postaLAddress, LoggerTraceConstants.COLON);

                                    if (lstPAFDetails != null && lstPAFDetails.Count > 0)
                                    {
                                        var invalidRecordsCount = lstPAFDetails.Where(n => n.IsValidData == false).ToList().Count;

                                        if (invalidRecordsCount > 0)
                                        {
                                            File.WriteAllText(Path.Combine(strPAFErrorFilePath, AppendTimeStamp(strfileName)), strLine);
                                            this.loggingHelper.Log(string.Format(PAFLoaderConstants.LOGMESSAGEFORPAFDATAVALIDATION, strfileName, DateTime.UtcNow.ToString()), TraceEventType.Information, null);
                                        }
                                        else
                                        {
                                            if (SavePAFDetails(lstPAFDetails))
                                            {
                                                File.WriteAllText(Path.Combine(strPAFProcessedFilePath, AppendTimeStamp(strfileName)), strLine);
                                                isPAFFileProcessed = true;
                                            }
                                            else
                                            {
                                                File.WriteAllText(Path.Combine(strPAFErrorFilePath, AppendTimeStamp(strfileName)), strLine);
                                                this.loggingHelper.Log(string.Format(PAFLoaderConstants.ERRORLOGMESSAGEFORPAFMSMQ, strfileName, DateTime.UtcNow.ToString()), TraceEventType.Information, null);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        File.WriteAllText(Path.Combine(strPAFErrorFilePath, AppendTimeStamp(strfileName)), strLine);
                                        this.loggingHelper.Log(string.Format(PAFLoaderConstants.LOGMESSAGEFORPAFWRONGFORMAT, strfileName, DateTime.UtcNow.ToString()), TraceEventType.Information, null);
                                    }
                                }
                            }
                        }
                    }
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PAFPriority, LoggerTraceConstants.PAFLoaderMethodExitEventId, LoggerTraceConstants.Title);

                return isPAFFileProcessed;
            }
        }

        /// <summary>
        /// Reads data from CSV file and maps to PostalAddressDTO object
        /// </summary>
        /// <param name="line">Line read from CSV File</param>
        /// <param name="strFileName">FileName required to track the error in db against each records</param>
        /// <returns>Postal Address DTO</returns>
        public List<PostalAddressDTO> ProcessPAF(string line, string strFileName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Service.LoadPAF"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PAFPriority, LoggerTraceConstants.PAFLoaderMethodEntryEventId, LoggerTraceConstants.Title);

                List<PostalAddressDTO> lstAddressDetails = null;
                string[] arrPAFDetails = line.Split(new string[] { PAFLoaderConstants.CRLF, PAFLoaderConstants.NEWLINE }, StringSplitOptions.None);

                if (arrPAFDetails.Count() > 0 && ValidateFile(arrPAFDetails))
                {
                    lstAddressDetails = arrPAFDetails.Select(v => MapPAFDetailsToDTO(v, strFileName)).ToList();

                    if (lstAddressDetails != null && lstAddressDetails.Count > 0)
                    {
                        // Validate PAF Details ,validates each property of PostalAddressDTO as per
                        // the business rule and set the Value of IsValid property to either true or
                        // false.Depending on the count of IsValid property data wil either will be
                        // saved in DB or file will be moved to error folder.
                        ValidatePAFDetails(lstAddressDetails);

                        // Remove Channel Island and Isle of Man Addresses are ones where the
                        // Postcode starts with one of: GY, JE or IM and Invalid records
                        lstAddressDetails = lstAddressDetails
                            .Where(n => !n.Postcode.StartsWith(PostCodePrefix.GY.ToString(), StringComparison.OrdinalIgnoreCase) && !n.Postcode.StartsWith(PostCodePrefix.JE.ToString(), StringComparison.OrdinalIgnoreCase) && !n.Postcode.StartsWith(PostCodePrefix.IM.ToString(), StringComparison.OrdinalIgnoreCase)).ToList();

                        // Remove duplicate PAF events which have create and delete instance for same UDPRN
                        lstAddressDetails = lstAddressDetails
                                                .SkipWhile(n => n.UDPRN.Equals(PAFLoaderConstants.PAFNOACTION))
                                                .GroupBy(x => x.UDPRN)
                                                .Where(g => g.Count() == 1)
                                                .SelectMany(g => g.Select(o => o))
                                                .ToList();
                    }
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PAFPriority, LoggerTraceConstants.PAFLoaderMethodExitEventId, LoggerTraceConstants.Title);

                return lstAddressDetails;
            }
        }

        /// <summary>
        /// Sent PAF create event to MSMQ
        /// </summary>
        /// <param name="lstPostalAddress">List of mapped address dto to validate each records</param>
        /// <returns>If success returns true else returns false</returns>
        public bool SavePAFDetails(List<PostalAddressDTO> lstPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Service.SavePAFDetails"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                bool isMessageQueued = false;
                try
                {
                    isMessageQueued = true;
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PAFPriority, LoggerTraceConstants.PAFLoaderMethodEntryEventId, LoggerTraceConstants.Title);
                    var lstPAFInsertEvents = lstPostalAddress.Where(insertFiles => insertFiles.AmendmentType == PAFLoaderConstants.PAFINSERT).ToList();

                    lstPAFInsertEvents.ForEach(postalAddress =>
                        {
                            // Message is created and the Postal Address DTO is passed as the object to
                            // be queued along with the queue name and queue path where the object needs
                            // to be queued.
                            IMessage msg = msgBroker.CreateMessage(postalAddress, PAFLoaderConstants.QUEUEPAF, PAFLoaderConstants.QUEUEPATH);

                            // The messge object created in the above code is then pushed onto the queue.
                            // This internally uses the MSMQ Send function to push the message to the queue.
                            msgBroker.SendMessage(msg);
                        });
                }
                catch (Exception ex)
                {
                    this.loggingHelper.Log(ex, TraceEventType.Error);
                    isMessageQueued = false;
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PAFPriority, LoggerTraceConstants.PAFLoaderMethodExitEventId, LoggerTraceConstants.Title);

                return isMessageQueued;
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
            return string.Concat(
                Path.GetFileNameWithoutExtension(strfileName),
               string.Format(dateTimeFormat, DateTime.UtcNow),
                Path.GetExtension(strfileName));
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

        /// <summary>
        /// Validates string i.e. no of comma's should be 19 and max characters per line should be 534
        /// </summary>
        /// <param name="arrLines">Array of string read from CSV file</param>
        /// <returns>boolean value after validating all the lines</returns>
        private bool ValidateFile(string[] arrLines)
        {
            bool isFileValid = true;
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, LoggerTraceConstants.MethodExecutionStarted, LoggerTraceConstants.COLON);

            foreach (string line in arrLines)
            {
                if (line.Count(n => n == PAFLoaderConstants.CommaChar) != noOfCharactersForPAF)
                {
                    isFileValid = false;
                    break;
                }

                if (line.ToCharArray().Count() > maxCharactersForPAF)
                {
                    isFileValid = false;
                    break;
                }
            }

            LogMethodInfoBlock(methodName, LoggerTraceConstants.MethodExecutionCompleted, LoggerTraceConstants.COLON);
            return isFileValid;
        }

        /// <summary>
        /// Mapping comma separated value to PostalAddressDTO object
        /// </summary>
        /// <param name="csvLine">Line read from CSV File</param>
        /// <param name="fileName">File Name</param>
        /// <returns>Returns mapped DTO</returns>
        private PostalAddressDTO MapPAFDetailsToDTO(string csvLine, string fileName)
        {
            PostalAddressDTO objAddDTO = new PostalAddressDTO();
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, LoggerTraceConstants.MethodExecutionStarted, LoggerTraceConstants.COLON);

            string[] values = csvLine.Split(PAFLoaderConstants.CommaChar);
            if (values.Count() == csvPAFValues)
            {
                objAddDTO.Date = values[PAFLoaderConstants.PAFDate] = values[PAFLoaderConstants.PAFDate] = string.IsNullOrEmpty(values[PAFLoaderConstants.PAFDate]) ? null : values[PAFLoaderConstants.PAFDate];
                objAddDTO.Time = values[PAFLoaderConstants.PAFTime] = values[PAFLoaderConstants.PAFTime] = string.IsNullOrEmpty(values[PAFLoaderConstants.PAFTime]) ? null : values[PAFLoaderConstants.PAFTime];
                objAddDTO.AmendmentType = values[PAFLoaderConstants.PAFAmendmentType] = values[PAFLoaderConstants.PAFAmendmentType] = string.IsNullOrEmpty(values[PAFLoaderConstants.PAFAmendmentType]) ? null : values[PAFLoaderConstants.PAFAmendmentType];
                objAddDTO.AmendmentDesc = values[PAFLoaderConstants.PAFAmendmentDesc] = values[PAFLoaderConstants.PAFAmendmentDesc] = string.IsNullOrEmpty(values[PAFLoaderConstants.PAFAmendmentDesc]) ? null : values[PAFLoaderConstants.PAFAmendmentDesc];
                objAddDTO.Postcode = values[PAFLoaderConstants.PAFPostcode] = values[PAFLoaderConstants.PAFPostcode] = string.IsNullOrEmpty(values[PAFLoaderConstants.PAFPostcode]) ? null : values[PAFLoaderConstants.PAFPostcode];
                objAddDTO.PostTown = values[PAFLoaderConstants.PAFPostTown] = values[PAFLoaderConstants.PAFPostTown] = string.IsNullOrEmpty(values[PAFLoaderConstants.PAFPostTown]) ? null : values[PAFLoaderConstants.PAFPostTown];
                objAddDTO.DependentLocality = values[PAFLoaderConstants.PAFDependentLocality] = values[PAFLoaderConstants.PAFDependentLocality] = string.IsNullOrEmpty(values[PAFLoaderConstants.PAFDependentLocality]) ? null : values[PAFLoaderConstants.PAFDependentLocality];
                objAddDTO.DoubleDependentLocality = values[PAFLoaderConstants.PAFDoubleDependentLocality] = values[PAFLoaderConstants.PAFDoubleDependentLocality] = string.IsNullOrEmpty(values[PAFLoaderConstants.PAFDoubleDependentLocality]) ? null : values[PAFLoaderConstants.PAFDoubleDependentLocality];
                objAddDTO.Thoroughfare = values[PAFLoaderConstants.PAFThoroughfare] = values[PAFLoaderConstants.PAFThoroughfare] = string.IsNullOrEmpty(values[PAFLoaderConstants.PAFThoroughfare]) ? null : values[PAFLoaderConstants.PAFThoroughfare];
                objAddDTO.DependentThoroughfare = values[PAFLoaderConstants.PAFDependentThoroughfare] = values[PAFLoaderConstants.PAFDependentThoroughfare] = string.IsNullOrEmpty(values[PAFLoaderConstants.PAFDependentThoroughfare]) ? null : values[PAFLoaderConstants.PAFDependentThoroughfare];
                objAddDTO.BuildingNumber = (!string.IsNullOrEmpty(values[PAFLoaderConstants.PAFBuildingNumber]) && !string.IsNullOrWhiteSpace(values[PAFLoaderConstants.PAFBuildingNumber])) ? Convert.ToInt16(values[PAFLoaderConstants.PAFBuildingNumber]) as short? : null;
                objAddDTO.BuildingName = values[PAFLoaderConstants.PAFBuildingName] = values[PAFLoaderConstants.PAFBuildingName] = string.IsNullOrEmpty(values[PAFLoaderConstants.PAFBuildingName]) ? null : values[PAFLoaderConstants.PAFBuildingName];
                objAddDTO.SubBuildingName = values[PAFLoaderConstants.PAFSubBuildingName] = values[PAFLoaderConstants.PAFSubBuildingName] = string.IsNullOrEmpty(values[PAFLoaderConstants.PAFSubBuildingName]) ? null : values[PAFLoaderConstants.PAFSubBuildingName];
                objAddDTO.POBoxNumber = values[PAFLoaderConstants.PAFPOBoxNumber] = values[PAFLoaderConstants.PAFPOBoxNumber] = string.IsNullOrEmpty(values[PAFLoaderConstants.PAFPOBoxNumber]) ? null : values[PAFLoaderConstants.PAFPOBoxNumber];
                objAddDTO.DepartmentName = values[PAFLoaderConstants.PAFDepartmentName] = values[PAFLoaderConstants.PAFDepartmentName] = string.IsNullOrEmpty(values[PAFLoaderConstants.PAFDepartmentName]) ? null : values[PAFLoaderConstants.PAFDepartmentName];
                objAddDTO.OrganisationName = values[PAFLoaderConstants.PAFOrganisationName] = values[PAFLoaderConstants.PAFOrganisationName] = string.IsNullOrEmpty(values[PAFLoaderConstants.PAFOrganisationName]) ? null : values[PAFLoaderConstants.PAFOrganisationName];
                objAddDTO.UDPRN = !string.IsNullOrEmpty(values[PAFLoaderConstants.PAFUDPRN]) && !string.IsNullOrWhiteSpace(values[PAFLoaderConstants.PAFUDPRN]) ? Convert.ToInt32(values[PAFLoaderConstants.PAFUDPRN]) : 0;
                objAddDTO.PostcodeType = values[PAFLoaderConstants.PAFPostcodeType] = values[PAFLoaderConstants.PAFPostcodeType] = string.IsNullOrEmpty(values[PAFLoaderConstants.PAFPostcodeType]) ? null : values[PAFLoaderConstants.PAFPostcodeType];
                objAddDTO.SmallUserOrganisationIndicator = values[PAFLoaderConstants.PAFSmallUserOrganisationIndicator] = values[PAFLoaderConstants.PAFSmallUserOrganisationIndicator] = string.IsNullOrEmpty(values[PAFLoaderConstants.PAFSmallUserOrganisationIndicator]) ? null : values[PAFLoaderConstants.PAFSmallUserOrganisationIndicator];
                objAddDTO.DeliveryPointSuffix = values[PAFLoaderConstants.PAFDeliveryPointSuffix] = values[PAFLoaderConstants.PAFDeliveryPointSuffix] = string.IsNullOrEmpty(values[PAFLoaderConstants.PAFDeliveryPointSuffix]) ? null : values[PAFLoaderConstants.PAFDeliveryPointSuffix];
                objAddDTO.IsValidData = true;
                objAddDTO.FileName = fileName;
            }

            LogMethodInfoBlock(methodName, LoggerTraceConstants.MethodExecutionCompleted, LoggerTraceConstants.COLON);
            return objAddDTO;
        }

        /// <summary>
        /// Perform business validation on PostalAddressDTO object
        /// </summary>
        /// <param name="lstAddress">List of mapped address dto to validate each records</param>
        private void ValidatePAFDetails(List<PostalAddressDTO> lstAddress)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, LoggerTraceConstants.MethodExecutionStarted, LoggerTraceConstants.COLON);

            foreach (PostalAddressDTO objAdd in lstAddress)
            {
                if (string.IsNullOrEmpty(objAdd.AmendmentType) ||
                    !new string[] { PAFLoaderConstants.PAFNOACTION, PAFLoaderConstants.PAFINSERT, PAFLoaderConstants.PAFUPDATE, PAFLoaderConstants.PAFDELETE }.Any(s => objAdd.AmendmentType.Contains(s)))
                {
                    objAdd.IsValidData = false;
                }

                if (string.IsNullOrEmpty(objAdd.PostTown))
                {
                    objAdd.IsValidData = false;
                }

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

                if (string.IsNullOrEmpty(Convert.ToString(objAdd.UDPRN == 0 ? null : objAdd.UDPRN)))
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
                    if (string.Equals(objAdd.PostcodeType, PostcodeType.L.ToString(), StringComparison.OrdinalIgnoreCase) && !string.Equals(objAdd.DeliveryPointSuffix, PAFLoaderConstants.DeliveryPointSuffix, StringComparison.OrdinalIgnoreCase))
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

            LogMethodInfoBlock(methodName, LoggerTraceConstants.MethodExecutionCompleted, LoggerTraceConstants.COLON);
        }

        /// <summary>
        /// PostCode validation i.e start and end character should be numeric , fourth last character
        /// should be whitespace etc.
        /// </summary>
        /// <param name="strPostCode">Postcode read from csv file</param>
        /// <returns>Post Code Validation state</returns>
        private bool ValidatePostCode(string strPostCode)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, LoggerTraceConstants.MethodExecutionStarted, LoggerTraceConstants.COLON);
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

            LogMethodInfoBlock(methodName, LoggerTraceConstants.MethodExecutionCompleted, LoggerTraceConstants.COLON);
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
            loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressDetailsPriority, LoggerTraceConstants.GetPostalAddressDetailsBusinessMethodEntryEventId, LoggerTraceConstants.Title);
        }

        #endregion private methods
    }
}