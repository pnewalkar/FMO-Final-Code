using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    [Table("FMO.BlockSequence")]
    public partial class BlockSequence
    {
        public Guid ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? OrderIndex { get; set; }

        public Guid BlockID { get; set; }

        public Guid LocationID { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual Block Block { get; set; }

        public virtual Location Location { get; set; }
    }
}