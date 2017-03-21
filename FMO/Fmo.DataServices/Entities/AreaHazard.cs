namespace Fmo.DataServices.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.AreaHazard")]
    public partial class AreaHazard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Hazard_Id { get; set; }

        public int Category_Id { get; set; }

        public int SubCategory_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ValidUntilDate { get; set; }

        public int Polygon_Id { get; set; }

        public virtual Polygon Polygon { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        public virtual ReferenceData ReferenceData1 { get; set; }
    }
}
