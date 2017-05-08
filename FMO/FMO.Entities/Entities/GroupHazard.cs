namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.GroupHazard")]
    public partial class GroupHazard
    {
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
