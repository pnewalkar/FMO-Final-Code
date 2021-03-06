﻿namespace RM.CommonLibrary.HelperMiddleware
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
        public const string CRLF = "\r\n";
        public const string NEWLINE = "\n";
        public const string COLON = " : ";
        public const string POSTALADDRESSDETAILS = "Postal Address Details : ";
        public const string Comma = ", ";
        public const string CommaWithoutSpace = ",";
        public const string SingleSpace = " ";
        public const string OpenSquareBracket = "[";
        public const string CloseSquareBracket = "]";
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
        public const string USRFILENAME = "OSABP_E[0-9]{3}$";
        public const string NotShown = "Not Shown";
        public const string DeliveryPointUseIndicator = "DeliveryPoint Use Indicator";
        public const string OperationalStatusGUIDLive = "Live";
        public const string NetworkNodeTypeRMGServiceNode = "RMG Service Node";
        public const string DependentLocality = "dependentLocality";
        public const string DoubleDependentLocality = "doubleDependentLocality";
        public const string Thoroughfare = "thoroughfare";
        public const string DependentThoroughfare = "dependentThoroughfare";
        public const string SubBuildingName = "subBuildingName";
        public const string OrganisationName = "organisationName";
        public const string DepartmentName = "departmentName";
        public const string MailVolume = "mailVolume";
        public const string MultipleOccupancyCount = "multipleOccupancyCount";
        public const string Locality = "locality";
        public const string DeliveryPointId = "deliveryPointId";
        public const string Street = "street";
        public const string DpUseResidential = "Residential";
        public const string DpUseOrganisation = "Organisation";
        public const string Space = " ";
        public const string RouteName = "ROUTENAME";
        public const string DpUse = "DPUSE";
        public const string LiveAddressStatus = "Live";
        public const string ServiceName = "ServiceName";
        public const string EnableLogging = "EnableLogging";
        public const string referenceDataWebAPIName = "";
        public const int ReferenceDataCategoryTypeForNameValuePair = 1;
        public const int ReferenceDataCategoryTypeForSimpleList = 2;
        public const int ReferenceDataCategoryTypeForHierarchicalLists = 3;
        public const string NameValuePairs = "nameValuePairs";
        public const string SimpleList = "simpleList";
        public const string NameValuePair = "nameValuePair";
        public const string XsltFilePath = "XSLTFilePath";
        public const string RefDataXMLFileName = "RefDataXMLFileName";
        public const string TimerIntervalInSeconds = "TimerIntervalInSeconds";
        #endregion Common

        #region NYB

        public const string LOADNYBDETAILSLOGMESSAGE = "Load NYB Error Message : Invalid NYB file, File Name : {0} : Log Time : {1}";
        public const string LOADNYBINVALIDDETAILS = "Load NYB Error Message : NYB file contains invalid data, File Name : {0} : Log Time : {1}";
        public const string QUEUENYB = "QUEUE_NYB";
        public const string FMOWebAPIName = "FMOWebAPIName";
        public const string FMOTokenGenerationUrl = "FMOTokenGenerationUrl";
        public const string FMOWebAPIUser = "FMOWebAPIUser";
        public const string ProcessedFilePath = "ProcessedFilePath";
        public const string ErrorFilePath = "ErrorFilePath";
        public const string UserName = "username";
        public const string TokenResponse = "FMO FileLoader:Request token recieved from authorization: ";
        public const string NYBFLATFILENAME = "CSV Not Yet Built";
        public const string NYBErrorMessageForDelete = "Load NYB Error Message : AddressType is NYB and have an associated Delivery Point for UDPRN: {0}";

        #endregion NYB

        #region USR

        public const string NoOfCharactersForNYB = "NoOfCharactersForNYB";
        public const string maxCharactersForNYB = "maxCharactersForNYB";
        public const string csvValuesForNYB = "csvValuesForNYB";

        public const string QUEUETHIRDPARTY = "QUEUE_THIRD_PARTY";
        public const string USRXMLROOT = "USR";
        public const string ADDRESSLOCATIONXMLROOT = "addressLocation";
        public const int TOLERANCEDISTANCEINMETERS = 10;
        public const string USRNOTIFICATIONSOURCE = "SYSTEM";
        public const string USRACTION = "Check updated DP Location";
        public const string USRBODY = "Please check the proposed new Location of the DP ";
        public const string USRGEOMETRYPOINT = "POINT({0} {1})";
        public const string USRCATEGORY = "Notification Type";
        public const string USRREFERENCEDATANAME = "Action required";
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
        public const string LOGMESSAGEFORUSRDATAVALIDATION = "Load USR Error Message : USR File Data is not valid, File Name : {0} : Log Time : {1} ";
        public const string NOTIFICATIONCLOSED = "Closed";        

        #endregion USR

        #region PAF

        public const string LOGMESSAGEFORPAFDATAVALIDATION = "Load PAF Error Message : PAF File Data is not valid, File Name : {0} : Log Time : {1} ";
        public const string LOGMESSAGEFORPAFWRONGFORMAT = "Load PAF Error Message : PAF File is not valid due to wrong file format or empty records, File Name : {0} : Log Time : {1}";
        public const string ERRORLOGMESSAGEFORPAFMSMQ = "Load PAF Error Message : Error occurred while processing it to messaging queue, File Name : {0} : Log Time : {1}";
        public const string ERRORLOGMESSAGEFORPAFFileName = "Load PAF Error Message : PAF File Name is not valid, File Name : {0} : Log Time : {1} ";
        public const string NoOfCharactersForPAF = "NoOfCharactersForPAF";
        public const string MaxCharactersForPAF = "MaxCharactersForPAF";
        public const string CsvPAFValues = "CsvPAFValues";
        public const string PAFWEBAPIURL = "PAFWebApiurl";
        public const string PAFWEBAPINAME = "PAFWebApiName";
        public const string TASKPAFACTION = "Position new DP Location";
        public const string PAFNOACTION = "B";
        public const string PAFINSERT = "I";
        public const string PAFUPDATE = "C";
        public const string PAFDELETE = "D";
        public const string PAFUPDATEFORNYB = "NYB";
        public const string PAFUPDATEFORUSR = "USR";

        public const string QUEUEPAF = "QUEUE_PAF";
        public const string DeliveryPointUseIndicatorPAF = "Organisation";
        public const string PAFProcessedFilePath = "PAFProcessedFilePath";
        public const string PAFErrorFilePath = "PAFErrorFilePath";
        public const string PAFErrorMessageForUDPRNNotUpdated = "Postal Address for Selected UDPRN not updated";
        public const string PAFErrorMessageForUnmatchedDeliveryPointForUSRType = "Delivery point not present for Postal address whose address type is <USR>";
        public const string PAFErrorMessageForMatchedDeliveryPointNotUpdatedForUSRType = "Delivery point not updated for Postal address whose address type is <USR>";
        public const string PAFErrorMessageForAddressTypeNYBNotFound = "Address Type of the selected Postal Address record is not <NYB>";
        public const string PAFErrorMessageForAddressTypeUSRNotFound = "Address Type of the selected Postal Address record is not <USR>";
        public const string PAFTaskBodyPreText = "Please position the DP ";
        public const string DeliveryPoints = "DeliveryPoints";
        public const string PAFNOTIFICATIONLINK = "http://fmoactionlinkurl/?={0}";
        public const string PAFFLATFILENAME = "CSV PAF Changes Single";
        public const string PAFZIPFILENAME = "^Y([0-9]{2})M(0[1-9]|1[012])D(0[1-9]|[12][0-9]|3[01])$";

        #endregion PAF

        #endregion File Processing

        #region OtherLayers

        public const string FeatureType = "Feature";
        public const string LayerType = "type";
        public const string Polygon = "POLYGON(({0} {1}, {2} {3}, {4} {5}, {6} {7}, {8} {9}))";
        public const string Linestring = "LINESTRING({0})";
        public const string Point = "POINT({0})";
        public const string PRIMARYROUTE = "Primary - ";
        public const string SECONDARYROUTE = "Secondary - ";
        public const string SELECT = "Select";
        public const string DEFAULTGUID = "00000000-0000-0000-0000-000000000000";
        public const string UDPRN = "udprn";
        public const string Latitude = "latitude";
        public const string Longitude = "longitude";
        public const string LinestringObject = "LINESTRING";
        public const string PointObject = "POINT";
        #endregion OtherLayers

        #region DeliveryPoints

        public const string DUPLICATEDELIVERYPOINT = "There is a duplicate of this Delivery Point in the system";

        public const string DUPLICATENYBRECORDS = "This address is in the NYB file under the postcode ";
        public const string DELIVERYPOINTCREATED = "Delivery Point created successfully";
        public const string DELIVERYPOINTCREATEDWITHOUTACCESSLINK = "Delivery Point created successfully without access link";
        public const string DELIVERYPOINTCREATEDWITHOUTLOCATION = "Delivery Point created successfully without location";
        public const string INTERNAL = "Internal";
        public const string ROWVERSION = "RowVersion";
        public const string UnSequenced = "U";

        #endregion DeliveryPoints

        #region MyRegion

        public const string ConcurrencyMessage = "Data has been already updated by another user";
        public const string ResponseContent = "Status Code: {0} Reason: {1} ";

        #endregion MyRegion

        #region Integration Service URI
        public const string ReferenceDataWebAPIName = "ReferenceDataWebAPIName";
        public const string AccessLinkWebAPIName = "AccessLinkWebAPIName";
        public const string NetworkManagerDataWebAPIName = "NetworkManagerDataWebAPIName";
        public const string DeliveryRouteManagerWebAPIName = "DeliveryRouteManagerWebAPIName";
        public const string UnitManagerDataWebAPIName = "UnitManagerDataWebAPIName";
        public const string PostalAddressManagerWebAPIName = "PostalAddressManagerWebAPIName";
        public const string BlockSequenceWebAPIName = "BlockSequenceWebAPIName";
        public const string DeliveryPointManagerDataWebAPIName = "DeliveryPointManagerDataWebAPIName";
        public const string PDFGeneratorWebAPIName = "PDFGeneratorWebAPIName";
        public const string NotificationManagerWebAPIName = "NotificationManagerWebAPIName";
        public const string XSLTFilePath = "XSLTFilePath";

        public const string NotificationManagerDataWebAPIName = "NotificationManagerDataWebAPIName";
        public const string AddressLocationManagerDataWebAPIName = "AddressLocationManagerDataWebAPIName";
        #endregion Integration Service URI

        public const string DeliveryPointExists = "DeliveryPointExists";
        public const string GetDeliveryPointByUDPRNForThirdParty = "GetDeliveryPointByUDPRNForThirdParty";
        public const string GetReferenceDataId = "GetReferenceDataId";
        public const string UpdateDeliveryPointLocationOnUDPRN = "UpdateDeliveryPointLocationOnUDPRN";
        public const string CheckIfNotificationExists = "CheckIfNotificationExists";
        public const string DeleteNotificationbyUDPRNAndAction = "DeleteNotificationbyUDPRNAndAction";
        public const string AddNewNotification = "AddNewNotification";
        public const string GetPostCodeSectorByUDPRN = "GetPostCodeSectorByUDPRN";
        public const string GetPostalAddress = "GetPostalAddress";
        public const string GetPAFAddress = "GetPAFAddress";
        public const string GetDeliveryPointByPostalAddress = "GetDeliveryPointByPostalAddress";
        public const string DeleteDeliveryPoint = "DeleteDeliveryPoint";
        public const string InsertDeliveryPoint = "InsertDeliveryPoint";
        public const string ReferenceSimpleList = "ReferenceSimpleList";
        public const string UpdateNotificationByUDPRN = "UpdateNotificationByUDPRN";

        #region Third Party Config params

        #endregion Third Party Config params

        #region

        public const string DefaultLoggingCategory = "General";
        public const int DefaultLoggingPriority = 0;
        public const int DefaultLoggingEventId = 0;
        public const string DefaultLoggingTitle = "";

        #endregion
    }
}