namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.GroupHazard")]
    public partial class GroupHazard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Hazard_Id { get; set; }

        public int DeliveryGroup_Id { get; set; }

        public int Category_Id { get; set; }

        public int SubCategory_Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Description { get; set; }

        public virtual DeliveryGroup DeliveryGroup { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        public virtual ReferenceData ReferenceData1 { get; set; }
    }
}
