namespace RM.DataManagement.NetworkManager.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSAccessRestriction")]
    public partial class OSAccessRestriction
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(20)]
        public string TOID { get; set; }

        [StringLength(21)]
        public string RestrictionValue { get; set; }

        [Column(TypeName = "text")]
        public string InclusionVehicleQualifier { get; set; }

        [Column(TypeName = "text")]
        public string ExclusionVehicleQualifier { get; set; }

        [Column(TypeName = "text")]
        public string TimeInterval { get; set; }

        public Guid? NetworkReferenceID { get; set; }

        public virtual NetworkReference NetworkReference { get; set; }
    }
}
