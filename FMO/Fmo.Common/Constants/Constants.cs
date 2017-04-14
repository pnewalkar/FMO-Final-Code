using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.Common.Constants
{
    public static class Constants
    {
        public const string USR_XML_ROOT = "USR";
        public const string ADDRESS_LOCATION_XML_ROOT = "addressLocation";
        public const string INSERT = "I";
        public const string UPDATE = "U";
        public const string DELETE = "D";
        public const string QUEUE_NYB = "QUEUE_NYB";
        public const string QUEUE_PAF = "QUEUE_PAF";
        public const string QUEUE_THIRD_PARTY = "QUEUE_THIRD_PARTY";
        public const string QUEUE_PATH = @".\Private$\";
        public const string PROCESSED_FOLDER = "Processed";
        public const string Error_FOLDER = "Error";
        public const string DeliveryPointSuffix = "1A";
        public const int BNG_COORDINATE_SYSTEM = 27700;
        public const int noOfCharactersForPAF = 19;
        public const int maxCharactersForPAF = 534;
        public const string USR_LOC_PROVIDER = "E";
        public const int TOLERANCE_DISTANCE_IN_METERS = 10;
        public const string USR_NOTIFICATION_SOURCE = "SYSTEM";
        public const string USR_ACTION = "Check updated DP Location";
        public const string Postal_Address_Status = "Postal Address Status";
        public const string Postal_Address_Type = "Postal Address Type";
        public const string USR_BODY = "Please check the proposed new Location of the DP Latitude: {0}, Longitude: {1}, X: {2}, Y: {3}";
        public const string USR_GEOMETRY_POINT = "POINT({0} {1})";
        public const string USR_CATEGORY = "Notification Type";
        public const string USR_REFERENCE_DATA_NAME = "Action required";
        public const string NETWORK_LINK_DATA_PROVIDER = "Network Link Data Provider";
        public const string EXTERNAL = "External";

        public const string PAFNOACTION = "B";
        public const string PAFINSERT = "I";
        public const string PAFUPDATE = "C";
        public const string PAFDELETE = "D";

    }
}

