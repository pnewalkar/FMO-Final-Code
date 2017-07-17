namespace RM.Data.ThirdPartyAddressLocation.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.PostcodeHierarchy")]
    public partial class PostcodeHierarchy
    {
        public Guid ID { get; private set; }

        [Required]
        [StringLength(8)]
        public string Postcode { get; private set; }

        [StringLength(8)]
        public string ParentPostcode { get; private set; }

        public Guid PostcodeTypeGUID { get; private set; }

        public DateTime RowCreateDateTime { get; private set; }
    }
}
