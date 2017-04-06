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
        public const string USR_LOC_PROVIDER = "<E>";
        public const int TOLERANCE_DISTANCE_IN_METERS = 10;
    }
}

