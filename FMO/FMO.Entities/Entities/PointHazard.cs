namespace Fmo.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.PointHazard")]
    public partial class PointHazard
    {
        public int Hazard_Id { get; set; }

        public int Category_Id { get; set; }

        public int SubCategory_Id { get; set; }

        public int OperationalObject_Id { get; set; }

        public int OperationalObjectType_Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Description { get; set; }

        public Guid ID { get; set; }

        public Guid Category_GUID { get; set; }

        public Guid SubCategory_GUID { get; set; }

        public Guid OperationalObjectType_GUID { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        public virtual ReferenceData ReferenceData1 { get; set; }

        public virtual ReferenceData ReferenceData2 { get; set; }
    }
}