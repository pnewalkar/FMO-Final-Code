using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO
{
    public class FileProcessingLogDTO
    {
        public Guid FileID { get; set; }

        public int UDPRN { get; set; }

        public string FileType { get; set; }

        public DateTime FileProcessing_TimeStamp { get; set; }

        public string FileName { get; set; }

        public string NatureOfError { get; set; }

        public string AmendmentType { get; set; }

        public string Comments { get; set; }
    }
}
