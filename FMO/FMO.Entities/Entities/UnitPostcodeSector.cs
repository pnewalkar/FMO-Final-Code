namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.UnitPostcodeSector")]
    public partial class UnitPostcodeSector
    {
        [Required]
        [StringLength(5)]
        public string PostcodeSector { get; set; }

        public Guid PostcodeSector_GUID { get; set; }

        public Guid Unit_GUID { get; set; }

        public Guid UnitStatus_GUID { get; set; }

        public Guid ID { get; set; }

        public virtual PostcodeSector PostcodeSector1 { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        public virtual UnitLocation UnitLocation { get; set; }
    }
}
