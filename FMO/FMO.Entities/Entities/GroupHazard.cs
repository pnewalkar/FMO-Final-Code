namespace Fmo.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.GroupHazard")]
    public partial class GroupHazard
    {
        public int Hazard_Id { get; set; }

        public int DeliveryGroup_Id { get; set; }

        public int Category_Id { get; set; }

        public int SubCategory_Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Description { get; set; }

        public Guid ID { get; set; }

        public Guid DeliveryGroup_GUID { get; set; }

        public Guid Category_GUID { get; set; }

        public Guid SubCategory_GUID { get; set; }

        public virtual DeliveryGroup DeliveryGroup { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        public virtual ReferenceData ReferenceData1 { get; set; }
    }
}