using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.Common.Constants
{
    public static class Constants
    {

        #region File Processing

        #region Common
        public const string DATETIMEFORMAT = "{0:-yyyy-MM-d-HH-mm-ss}";
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
        #endregion

        #region NYB
        public const string LOADNYBDETAILSLOGMESSAGE = "Load NYB Error Message : NYB File ins not valid File Name : {0} : Log Time : {1}";
        public const string QUEUENYB = "QUEUE_NYB";
        public const string FMOWebAPIName = "FMOWebAPIName";
        #endregion

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
        public const int USRNOTIFICATIONDUE = 24;
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
        public const string USRCHANGETYPE = "changeType";
        #endregion

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
        #endregion

        #endregion

        public const string FeatureType = "Feature";
    }
}