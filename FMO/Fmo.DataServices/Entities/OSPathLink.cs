namespace Fmo.DataServices.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSPathLink")]
    public partial class OSPathLink
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NetworkLink_Id { get; set; }

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

        public virtual NetworkLink NetworkLink { get; set; }
    }
}
