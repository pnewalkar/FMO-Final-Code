namespace RM.DataManagement.PostalAddress.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.PostcodeHierarchy")]
    public partial class PostcodeHierarchy
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(8)]
        public string Postcode { get; set; }

        [StringLength(8)]
        public string ParentPostcode { get; set; }

        public Guid PostcodeTypeGUID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual Postcode Postcode1 { get; set; }
    }
}
