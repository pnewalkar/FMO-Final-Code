using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Integration.PostalAddress.PAFLoader.Utils
{
    public static class PAFLoaderConstants
    {
        internal const string DATETIMEFORMAT = "{0:-dd-MM-yyyy-HH-mm-ss}";
        internal const string QUEUEPATH = @".\Private$\";
        internal const string DeliveryPointSuffix = "1A";
        internal const string CRLF = "\r\n";
        internal const string NEWLINE = "\n";
        internal const string POSTALADDRESSDETAILS = "Postal Address Details : ";
        internal const char CommaChar = ',';

        internal const string LOGMESSAGEFORPAFDATAVALIDATION = "Load PAF Error Message : PAF File Data is not valid, File Name : {0} : Log Time : {1} ";
        internal const string LOGMESSAGEFORPAFWRONGFORMAT = "Load PAF Error Message : PAF File is not valid due to wrong file format or empty records, File Name : {0} : Log Time : {1}";
        internal const string ERRORLOGMESSAGEFORPAFMSMQ = "Load PAF Error Message : Error occurred while processing it to messaging queue, File Name : {0} : Log Time : {1}";
        internal const string NoOfCharactersForPAF = "NoOfCharactersForPAF";
        internal const string MaxCharactersForPAF = "MaxCharactersForPAF";
        internal const string CsvPAFValues = "CsvPAFValues";
        internal const string PAFNOACTION = "B";
        internal const string PAFINSERT = "I";
        internal const string PAFUPDATE = "C";
        internal const string PAFDELETE = "D";
        internal const string QUEUEPAF = "QUEUE_PAF";
        internal const string PAFProcessedFilePath = "PAFProcessedFilePath";
        internal const string PAFErrorFilePath = "PAFErrorFilePath";
        internal const string PAFFLATFILENAME = "CSV PAF Changes Single";
        internal const string PAFZIPFILENAME = "^Y([0-9]{2})M(0[1-9]|1[012])D(0[1-9]|[12][0-9]|3[01])$";

        internal const int PAFDate = 0;
        internal const int PAFTime = 1;
        internal const int PAFAmendmentType = 2;
        internal const int PAFAmendmentDesc = 3;
        internal const int PAFPostcode = 4;
        internal const int PAFPostTown = 5;
        internal const int PAFDependentLocality = 6;
        internal const int PAFDoubleDependentLocality = 7;
        internal const int PAFThoroughfare = 8;
        internal const int PAFDependentThoroughfare = 9;
        internal const int PAFBuildingNumber = 10;
        internal const int PAFBuildingName = 11;
        internal const int PAFSubBuildingName = 12;
        internal const int PAFPOBoxNumber = 13;
        internal const int PAFDepartmentName = 14;
        internal const int PAFOrganisationName = 15;
        internal const int PAFUDPRN = 16;
        internal const int PAFPostcodeType = 17;
        internal const int PAFSmallUserOrganisationIndicator = 18;
        internal const int PAFDeliveryPointSuffix = 19;
    }
}
