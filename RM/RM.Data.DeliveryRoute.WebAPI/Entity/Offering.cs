using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    [Table("FMO.Offering")]
    public partial class Offering
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Offering()
        {
            LocationOfferings = new HashSet<LocationOffering>();
        }

        public Guid ID { get; private set; }

        [StringLength(50)]
        public string OfferingDescription { get; private set; }

        public DateTime RowCreateDateTime { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocationOffering> LocationOfferings { get; private set; }
    }
}