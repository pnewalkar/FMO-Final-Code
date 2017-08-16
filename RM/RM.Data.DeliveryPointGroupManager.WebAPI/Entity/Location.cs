namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.Location")]
    public partial class Location
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Location()
        {
            LocationOfferings = new HashSet<LocationOffering>();
            LocationRelationships = new HashSet<LocationRelationship>();
            LocationRelationships1 = new HashSet<LocationRelationship>();
        }

        public Guid ID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AlternateID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        [Required]
        public DbGeometry Shape { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocationOffering> LocationOfferings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocationRelationship> LocationRelationships { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocationRelationship> LocationRelationships1 { get; set; }

        public virtual NetworkNode NetworkNode { get; set; }
    }
}