using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.Data.ThirdPartyAddressLocation.WebAPI.Utils
{
    public static class ThirdPartyAddressLocationConstants
    {
        internal const int TOLERANCEDISTANCEINMETERS = 10;
        internal const string USRNOTIFICATIONSOURCE = "SYSTEM";
        internal const string USRACTION = "Check updated DP Location";
        internal const string USRGEOMETRYPOINT = "POINT({0} {1})";
        internal const string USRCATEGORY = "Notification Type";
        internal const string USRREFERENCEDATANAME = "Action required";
        internal const string USRNOTIFICATIONLINK = "http://fmoactionlinkurl/?={0}";
        internal const string PAFTaskBodyPreText = "Please position the DP ";
        internal const string NETWORKLINKDATAPROVIDER = "Data Provider";
        internal const string EXTERNAL = "External";
        internal const int BNGCOORDINATESYSTEM = 27700;
        internal const string Comma = ", ";
        internal const string TASKPAFACTION = "Position new DP Location";
        internal const string USRNOTIFICATIONBODYPREFIX = "Please check the proposed new Location of the DP ";

        internal const string DeliveryPointExists = "DeliveryPointExists";
        internal const string GetDeliveryPointByUDPRNForThirdParty = "GetDeliveryPointByUDPRNForThirdParty";
        internal const string GetReferenceDataId = "GetReferenceDataId";
        internal const string UpdateDeliveryPointLocationOnUDPRN = "UpdateDeliveryPointLocationOnUDPRN";
        internal const string CheckIfNotificationExists = "CheckIfNotificationExists";
        internal const string DeleteNotificationbyUDPRNAndAction = "DeleteNotificationbyUDPRNAndAction";
        internal const string AddNewNotification = "AddNewNotification";
        internal const string GetPostCodeSectorByUDPRN = "GetPostCodeSectorByUDPRN";
        internal const string GetPostalAddress = "GetPostalAddress";

        internal const string UDPRN = "udprn";
        internal const string Latitude = "latitude";
        internal const string Longitude = "longitude";

        internal const int NOTIFICATIONDUE = 24;
        internal const string DeliveryPointUseIndicator = "DeliveryPoint Use Indicator";
        internal const string OperationalStatusGUIDLive = "Live";
        internal const string NetworkNodeTypeRMGServiceNode = "RMG Service Node";
        internal const string TASKNOTIFICATION = "Notification Type";
        internal const string TASKACTION = "Action required";
        internal const string TASKSOURCE = "SYSTEM";
        internal const string DeliveryPointUseIndicatorPAF = "Organisation";
        internal const string NOTIFICATIONCLOSED = "Closed";
        internal const string APPROXLOCATION = "Live pending location";
    }
}
