namespace Fmo.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.DeliveryUnitLocationPostcode")]
    public partial class DeliveryUnitLocationPostcode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeliveryUnitLocationPostcodeId { get; set; }

        public int DeliveryUnit_Id { get; set; }

        [Required]
        [StringLength(8)]
        public string PostcodeUnit { get; set; }

        public Guid ID { get; set; }

        public Guid DeliveryUnit_GUID { get; set; }

        public Guid PoscodeUnit_GUID { get; set; }

        public virtual DeliveryUnitLocation DeliveryUnitLocation { get; set; }

        public virtual Postcode Postcode { get; set; }
    }
}