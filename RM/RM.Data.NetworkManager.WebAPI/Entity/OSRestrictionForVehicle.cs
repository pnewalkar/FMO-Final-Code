namespace RM.DataManagement.NetworkManager.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSRestrictionForVehicles")]
    public partial class OSRestrictionForVehicle
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(20)]
        public string TOID { get; set; }

        public double? MeasureInMeters { get; set; }

        [Column(TypeName = "text")]
        public string RestrictionType { get; set; }

        [StringLength(10)]
        public string SourceofMeasure { get; set; }

        [Column(TypeName = "text")]
        public string Inclusion { get; set; }

        [Column(TypeName = "text")]
        public string Exclusion { get; set; }

        [StringLength(50)]
        public string Structure { get; set; }

        [StringLength(120)]
        public string TrafficSign { get; set; }

        public Guid? NetworkReference_GUID { get; set; }

        public virtual NetworkReference NetworkReference { get; set; }
    }
}
