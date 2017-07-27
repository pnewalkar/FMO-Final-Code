namespace RM.Data.PostalAddress.WebAPI.Utils
{
    public static class PostalAddressConstants
    {
        internal const string TASKPAFACTION = "Position new DP Location";
        internal const string DeliveryPointUseIndicatorPAF = "Organisation";
        internal const string PAFErrorMessageForUnmatchedDeliveryPointForUSRType = "Delivery point not present for Postal address whose address type is <USR>";
        internal const string PAFErrorMessageForAddressTypeNYBNotFound = "Address Type of the selected Postal Address record is not <NYB>";
        internal const string PAFErrorMessageForAddressTypeUSRNotFound = "Address Type of the selected Postal Address record is not <USR>";
        internal const string PAFTaskBodyPreText = "Please position the DP ";
        internal const string PAFNOTIFICATIONLINK = "http://fmoactionlinkurl/?={0}";
        internal const int NOTIFICATIONDUE = 24;
        internal const string EXTERNAL = "External";
        internal const string TASKACTION = "Action required";
        internal const string TASKSOURCE = "SYSTEM";
        internal const string PostalAddressStatus = "Postal Address Status";
        internal const string PostalAddressType = "Postal Address Type";
        internal const string PendingDeleteInFMO = "Pending Delete in FMO";

        internal const string LiveAddressStatus = "Live";
        internal const string OperationalStatusGUIDLive = "Live";
        internal const string OperationalStatusGUIDLivePending = "Live Pending Location";
        internal const string NetworkNodeTypeRMGServiceNode = "RMG Service Node";
        internal const string NOTIFICATIONCLOSED = "Closed";

        internal const string INSERT = "I";
        internal const string UPDATE = "U";
        internal const string DELETE = "D";
        internal const string NYBErrorMessageForDelete = "Load NYB Error Message : AddressType is NYB and have an associated Delivery Point for UDPRN: {0}";

        internal const string ResponseContent = "Status Code: {0} Reason: {1} ";
        internal const string ReferenceDataWebAPIName = "ReferenceDataWebAPIName";
        internal const string UnitManagerDataWebAPIName = "UnitManagerDataWebAPIName";
        internal const string DeliveryPointManagerDataWebAPIName = "DeliveryPointManagerDataWebAPIName";
        internal const string NotificationManagerDataWebAPIName = "NotificationManagerDataWebAPIName";
        internal const string AddressLocationManagerDataWebAPIName = "AddressLocationManagerDataWebAPIName";
        internal const string NoMatchToAddressOnUDPRN = "No Match to Address on UDPRN : {0}";
        internal const string WrongAddressType = "Wrong Address Type UDPRN : {0}";
    }
}