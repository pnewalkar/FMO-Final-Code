namespace RM.Integration.PostalAddress.NYBLoader.Utils
{
    public static class NYBLoaderConstants
    {
        public const string NoOfCharactersForNYB = "NoOfCharactersForNYB";
        public const string MaxCharactersForNYB = "maxCharactersForNYB";
        public const string CsvValuesForNYB = "csvValuesForNYB";
        internal const string ProcessedFilePath = "ProcessedFilePath";
        internal const string ErrorFilePath = "ErrorFilePath";
        internal const string DeliveryPointSuffix = "1A";
        internal const string XMLFileFolderSettings = "XMLFileFolderSettings";
        internal const string CRLF = "\r\n";
        internal const string NEWLINE = "\n";
        internal const string POSTALADDRESSDETAILS = "Postal Address Details : ";
        internal const string BatchServiceName = "ServiceName";
        internal const string LOADNYBDETAILSLOGMESSAGE = "Load NYB Error Message : Invalid NYB file, File Name : {0} : Log Time : {1}";
        internal const string LOADNYBINVALIDDETAILS = "Load NYB Error Message : NYB file contains invalid data, File Name : {0} : Log Time : {1}";
        internal const string FMOWebAPIName = "FMOWebAPIName";
        internal const string NYBFLATFILENAME = "CSV Not Yet Built";
        internal const int NYBPostcode = 0;
        internal const int NYBPostTown = 1;
        internal const int NYBDependentLocality = 2;
        internal const int NYBDoubleDependentLocality = 3;
        internal const int NYBThoroughfare = 4;
        internal const int NYBDependentThoroughfare = 5;
        internal const int NYBBuildingNumber = 6;
        internal const int NYBBuildingName = 7;
        internal const int NYBSubBuildingName = 8;
        internal const int NYBPOBoxNumber = 9;
        internal const int NYBDepartmentName = 10;
        internal const int NYBOrganisationName = 11;
        internal const int NYBUDPRN = 12;
        internal const int NYBPostcodeType = 13;
        internal const int NYBSmallUserOrganisationIndicator = 14;
        internal const int NYBDeliveryPointSuffix = 15;
        internal const string PAFZIPFILENAME = "^Y([0-9]{2})M(0[1-9]|1[012])D(0[1-9]|[12][0-9]|3[01])$";
        internal const string DATETIMEFORMAT = "{0:-dd-MM-yyyy-HH-mm-ss}";
    }
}