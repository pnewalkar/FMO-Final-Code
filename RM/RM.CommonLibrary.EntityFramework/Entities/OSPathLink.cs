namespace RM.CommonLibrary.EntityFramework.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSPathLink")]
    public partial class OSPathLink
    {
        [Required]
        [StringLength(20)]
        public string TOID { get; set; }

        public DbGeometry CentreLineGeometry { get; set; }

        [MaxLength(1)]
        public byte[] Ficticious { get; set; }

        [StringLength(42)]
        public string FormOfWay { get; set; }

        [StringLength(255)]
        public string PathName { get; set; }

        [StringLength(255)]
        public string AlternateName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal LengthInMeters { get; set; }

        [StringLength(20)]
        public string StartNodeTOID { get; set; }

        [StringLength(20)]
        public string EndNodeTOID { get; set; }

        public byte StartGradeSeparation { get; set; }

        public byte EndGradeSeparation { get; set; }

        [StringLength(20)]
        public string FormPartOf { get; set; }

        public Guid ID { get; set; }

        public Guid? StartNode_GUID { get; set; }

        public Guid? EndNode_GUID { get; set; }

        public virtual OSPathNode OSPathNode { get; set; }

        public virtual OSPathNode OSPathNode1 { get; set; }
    }
}
