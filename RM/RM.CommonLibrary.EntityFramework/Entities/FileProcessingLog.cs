namespace RM.CommonLibrary.EntityFramework.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.FileProcessingLog")]
    public partial class FileProcessingLog
    {
        [Key]
        public Guid FileID { get; set; }

        public int UDPRN { get; set; }

        [Required]
        [StringLength(25)]
        public string FileType { get; set; }

        public DateTime FileProcessing_TimeStamp { get; set; }

        [StringLength(256)]
        public string FileName { get; set; }

        [Required]
        public string ErrorMessage { get; set; }

        [Required]
        [StringLength(1)]
        public string AmendmentType { get; set; }

        public string Comments { get; set; }

        [Required]
        public bool SuccessFlag { get; set; }
    }
}
