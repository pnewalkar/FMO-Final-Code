using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Integration.ThirdPartyAddressLocation.Loader.Utils
{
    public static class ThirdPartyLoaderConstants
    {
        internal const string DATETIMEFORMAT = "{0:-dd-MM-yyyy-HH-mm-ss}";
        internal const string INSERT = "I";
        internal const string UPDATE = "U";
        internal const string QUEUEPATH = @".\Private$\";
        internal const string USRFILENAME = "OSABP_E[0-9]{3}$";

        internal const string QUEUETHIRDPARTY = "QUEUE_THIRD_PARTY";
        internal const string USRXMLROOT = "USR";
        internal const string ADDRESSLOCATIONXMLROOT = "addressLocation";
        internal const string XSDLOCATIONCONFIG = "XSDLocation";
        internal const string USRPROCESSEDFILEPATHCONFIG = "USRProcessedFilePath";
        internal const string USRERRORFILEPATHCONFIG = "USRErrorFilePath";
        internal const string USRUDPRN = "udprn";
        internal const string LOGMESSAGEFORUSRDATAVALIDATION = "Load USR Error Message : USR File Data is not valid, File Name : {0} : Log Time : {1} ";
    }
}
