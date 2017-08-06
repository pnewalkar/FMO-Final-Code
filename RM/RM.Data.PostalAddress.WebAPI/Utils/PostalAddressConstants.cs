namespace RM.Data.PostalAddress.WebAPI.Utils
{
    public static class PostalAddressConstants
    {
        internal const string TASKPAFACTION = "Position new DP Location";
        internal const string DeliveryPointUseIndicatorPAF = "Organisation";
        
        internal const string PAFTaskBodyPreText = "Please position the DP ";
        internal const string PAFNOTIFICATIONLINK = "http://fmoactionlinkurl/?={0}";
        internal const int NOTIFICATIONDUE = 24;
        internal const int BNGCOORDINATESYSTEM = 27700;
        internal const string EXTERNAL = "External";
        internal const string TASKACTION = "Action required";
        internal const string TASKSOURCE = "SYSTEM";
        internal const string PostalAddressStatus = "Postal Address Status";
        internal const string PostalAddressType = "Postal Address Type";
        internal const string Comma = ", ";
        internal const string PendingDeleteInFMO = "Pending Delete in FMO";

        internal const string LiveAddressStatus = "Live";
        internal const string OperationalStatusGUIDLive = "Live";
        internal const string OperationalStatusGUIDLivePending = "Live Pending Location";
        internal const string NetworkNodeTypeRMGServiceNode = "RMG Service Node";
        internal const string NOTIFICATIONCLOSED = "Closed";

        internal const string INSERT = "I";
        internal const string UPDATE = "C";
        internal const string DELETE = "D";
        internal const string NYBErrorMessageForDelete = "AddressType is NYB and have an associated Delivery Point for UDPRN: {0}";
        internal const string DeliveryPointAlias = "Delivery Point Alias";

        internal const string ResponseContent = "Status Code: {0} Reason: {1} ";
        internal const string ReferenceDataWebAPIName = "ReferenceDataWebAPIName";
        internal const string UnitManagerDataWebAPIName = "UnitManagerDataWebAPIName";
        internal const string DeliveryPointManagerDataWebAPIName = "DeliveryPointManagerDataWebAPIName";
        internal const string NotificationManagerDataWebAPIName = "NotificationManagerDataWebAPIName";
        internal const string AddressLocationManagerDataWebAPIName = "AddressLocationManagerDataWebAPIName";
        internal const string NoMatchToAddressOnUDPRN = "No Match to Address on UDPRN : {0}";
        internal const string WrongAddressType = "Wrong Address Type UDPRN : {0}";
        internal const string NoMatchingDP = "No matching Delivery Point for UDPRN :{0}";

        internal const string PAFErrorMessageForDPDelete = "PAF Delete record : Associated Delivery Point for PostalAddresId is not deleted : {0}";
        internal const string PAFErrorMessageForDPNotFoundDelete = "PAF Delete record : Associated Delivery Point not found for PostalAddresId : {0}";
        internal const string PAFErrorMessageForUnmatchedDeliveryPointForUSRType = "Delivery point not present for Postal address whose address type is <USR>";
        internal const string PAFErrorMessageForAddressTypeNYBNotFound = "Address Type of the selected Postal Address record is not <NYB>";
        internal const string PAFErrorMessageForAddressTypeUSRNotFound = "Address Type of the selected Postal Address record is not <USR>";

        internal const string PAFERRORLOGMESSAGE = "Load PAF Error Message : {0}, UDPRN : {1}, Ammendment Type : {2}, File Name : {3}, File Name : {4}, Log Time : {5}";
        internal const string NYBERRORLOGMESSAGE = "Load NYB Error Message : {0}, UDPRN : {1}, Ammendment Type : {2}, File Name : {3}, File Name : {4}, Log Time : {5}";
    }
}