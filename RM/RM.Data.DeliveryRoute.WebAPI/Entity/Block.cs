using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    [Table("FMO.Block")]
    public partial class Block
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Auto Generated")]
        public Block()
        {
            Block1 = new HashSet<Block>();
            BlockSequences = new HashSet<BlockSequence>();
            RouteActivities = new HashSet<RouteActivity>();
        }

        public Guid ID { get; set; }

        public Guid BlockTypeGUID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BlockSpanMinute { get; set; }

        public Guid? PairedBlockID { get; set; }

        public int? GeoRouteBlockID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<Block> Block1 { get; set; }

        public virtual Block Block2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<BlockSequence> BlockSequences { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<RouteActivity> RouteActivities { get; set; }
    }
}