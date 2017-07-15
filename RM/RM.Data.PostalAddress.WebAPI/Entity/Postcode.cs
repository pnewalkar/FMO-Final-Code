namespace RM.DataManagement.PostalAddress.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.Postcode")]
    public partial class Postcode
    {
        public Guid ID { get; private set; }

        [Required]
        [StringLength(8)]
        public string PostcodeUnit { get; private set; }

        [Required]
        [StringLength(4)]
        public string OutwardCode { get; private set; }

        [Required]
        [StringLength(3)]
        public string InwardCode { get; private set; }

        public Guid? PrimaryRouteGUID { get; private set; }

        public Guid? SecondaryRouteGUID { get; private set; }

        public DateTime RowCreateDateTime { get; private set; }
        public virtual PostcodeHierarchy PostcodeHierarchy { get; set; }
    }
}
