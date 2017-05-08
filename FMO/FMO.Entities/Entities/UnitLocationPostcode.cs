namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.UnitLocationPostcode")]
    public partial class UnitLocationPostcode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UnitLocationPostcodeId { get; set; }

        public int Unit_Id { get; set; }

        [Required]
        [StringLength(8)]
        public string PostcodeUnit { get; set; }

        public Guid ID { get; set; }

        public Guid Unit_GUID { get; set; }

        public Guid PoscodeUnit_GUID { get; set; }

        public virtual Postcode Postcode { get; set; }

        public virtual UnitLocation UnitLocation { get; set; }
    }
}
