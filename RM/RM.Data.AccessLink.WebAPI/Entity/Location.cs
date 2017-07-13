namespace RM.DataManagement.AccessLink.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.Location")]
    public partial class Location
    {
        public Guid ID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AlternateID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        [Required]
        public DbGeometry Shape { get; set; }

        public virtual NetworkNode NetworkNode { get; set; }
    }
}
