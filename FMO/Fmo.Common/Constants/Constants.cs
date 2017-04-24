﻿namespace Fmo.Common.Constants
{
    public static class Constants
    {
        #region File Processing

        #region Common

        public const int NOTIFICATIONDUE = 24;
        public const string DATETIMEFORMAT = "{0:-dd-MM-yyyy-HH-mm-ss}";
        public const string INSERT = "I";
        public const string UPDATE = "U";
        public const string DELETE = "D";
        public const string QUEUEPATH = @".\Private$\";
        public const string PROCESSEDFOLDER = "Processed";
        public const string ErrorFOLDER = "Error";
        public const string USRLOCPROVIDER = "E";
        public const string MSGQUEUEPERMISSION = "Everyone";
        public const string NETWORKLINKDATAPROVIDER = "Data Provider";
        public const string EXTERNAL = "External";
        public const string TASKNOTIFICATION = "Notification Type";
        public const string TASKACTION = "Action required";
        public const string TASKSOURCE = "SYSTEM";
        public const string DeliveryPointSuffix = "1A";
        public const int BNGCOORDINATESYSTEM = 27700;
        public const string PostalAddressStatus = "Postal Address Status";
        public const string PostalAddressType = "Postal Address Type";
        public const string XMLFileFolderSettings = "XMLFileFolderSettings";
        public const string MethodExecutionStarted = " Method exceution started";
        public const string MethodExecutionCompleted = " Method exceution completed";
        public const string EnableLogging = "EnableLogging";
        public const string CRLF = "\r\n";
        public const string NEWLINE = "\n";
        public const string COLON = " : ";
        public const string POSTALADDRESSDETAILS = "Postal Address Details : ";
        public const string Comma = ", ";
        public const string BuildingName = "name";
        public const string BuildingNumber = "number";
        public const string Postcode = "postcode";
        public const string StreetName = "street_name";
        public const string SearchResultCount = "SearchResultCount";
        public const string EmptyString = "";
        public const string DeliveryPointFormat = "{0},{1},{2},{3},{4},{5}";
        public const string StreetNameFormat = "{0},{1}";
        public const string SMTPHOSTNAME = "localhost";
        public const int SMTPPORT = 25;
        public const string AccessToken = "access_token";
        public const string Bearer = "bearer";
        #endregion Common

        #region NYB

        public const string LOADNYBDETAILSLOGMESSAGE = "Load NYB Error Message : NYB File is not valid, File Name : {0} : Log Time : {1} : UDPRN : {2}";
        public const string QUEUENYB = "QUEUE_NYB";
        public const string FMOWebAPIName = "FMOWebAPIName";
        public const string FMOTokenGenerationUrl = "FMOTokenGenerationUrl";
        public const string FMOWebAPIUser = "FMOWebAPIUser";
        public const string ProcessedFilePath = "ProcessedFilePath";
        public const string ErrorFilePath = "ErrorFilePath";
        public const string UserName = "username";
        public const string NYBErrorMessageForDelete = "Load NYB Error Message : AddressType is NYB and have an associated Delivery Point for UDPRN: {0}";

        #endregion NYB

        #region USR

        public const string QUEUETHIRDPARTY = "QUEUE_THIRD_PARTY";
        public const string USRXMLROOT = "USR";
        public const string ADDRESSLOCATIONXMLROOT = "addressLocation";
        public const int TOLERANCEDISTANCEINMETERS = 10;
        public const string USRNOTIFICATIONSOURCE = "SYSTEM";
        public const string USRACTION = "Check updated DP Location";
        public const string USRBODY = "Please check the proposed new Location of the DP Latitude: {0}, Longitude: {1}, X: {2}, Y: {3}";
        public const string USRGEOMETRYPOINT = "POINT({0} {1})";
        public const string USRCATEGORY = "Notification Type";
        public const string USRREFERENCEDATANAME = "Action required";
        public const string USREMAILFROMEMAIL = "USRFromEmail";
        public const string USREMAILSUBJECT = "USRSubject";
        public const string USREMAILBODY = "USRBody";
        public const string USREMAILTOEMAIL = "USRFromEmail";
        public const string USRWEBAPIURL = "USRWebApiurl";
        public const string USRWEBAPINAME = "USRWebApiName";
        public const string XSDLOCATIONCONFIG = "XSDLocation";
        public const string USRPROCESSEDFILEPATHCONFIG = "USRProcessedFilePath";
        public const string USRERRORFILEPATHCONFIG = "USRErrorFilePath";
        public const string USRUDPRN = "udprn";
        public const string USRXCOORDINATE = "xCoordinate";
        public const string USRYCOORDINATE = "yCoordinate";
        public const string USRLATITUDE = "latitude";
        public const string USRLONGITITUDE = "longitude";
        public const string USRCHANGETYPE = "changeType";
        public const string REQUESTLOG = "udprn: {0} xCoordinate: {1} yCoordinate:{2} latitude:{3} longitude:{4} changeType:{5}";
        public const string USRNOTIFICATIONLINK = "http://fmoactionlinkurl/?={0}";

        #endregion USR

        #region PAF

        public const int NoOfCharactersForPAF = 19;
        public const int MaxCharactersForPAF = 534;
        public const string PAFWEBAPIURL = "PAFWebApiurl";
        public const string PAFWEBAPINAME = "PAFWebApiName";
        public const string TASKPAFACTION = "Position new DP";
        public const string PAFNOACTION = "B";
        public const string PAFINSERT = "I";
        public const string PAFUPDATE = "C";
        public const string PAFDELETE = "D";
        public const int CsvPAFValues = 20;
        public const string QUEUEPAF = "QUEUE_PAF";
        public const string DeliveryPointUseIndicatorPAF = "B";
        public const string PAFProcessedFilePath = "PAFProcessedFilePath";
        public const string PAFErrorFilePath = "PAFErrorFilePath";
        public const string PAFErrorMessageForUDPRNNotUpdated = "Postal Address for Selected UDPRN not updated";
        public const string PAFErrorMessageForAddressTypeNYBNotFound = "Address Type of the selected Postal Address record is not <NYB>";
        public const string PAFErrorMessageForAddressTypeUSRNotFound = "Address Type of the selected Postal Address record is not <USR>";
        public const string PAFTaskBodyPreText = "Please position the DP ";
        public const string DeliveryPoints = "DeliveryPoints";
        public const string PAFNOTIFICATIONLINK = "http://fmoactionlinkurl/?={0}";

        #endregion PAF

        #endregion File Processing

        #region OtherLayers

        public const string FeatureType = "Feature";
        public const string LayerType = "type";
        public const string Polygon = "POLYGON(({0} {1}, {2} {3}, {4} {5}, {6} {7}, {8} {9}))";

        #endregion OtherLayers
    }
}