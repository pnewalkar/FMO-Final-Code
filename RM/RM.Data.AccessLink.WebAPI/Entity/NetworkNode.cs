namespace RM.DataManagement.AccessLink.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.NetworkNode")]
    public partial class NetworkNode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Justification")]
        public NetworkNode()
        {
            NetworkLinks = new HashSet<NetworkLink>();
            NetworkLinks1 = new HashSet<NetworkLink>();
        }

        public Guid ID { get; set; }

        public Guid NetworkNodeType_GUID { get; set; }

        [StringLength(20)]
        public string TOID { get; set; }

        public Guid? DataProviderGUID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual DeliveryPoint DeliveryPoint { get; set; }

        public virtual Location Location { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification ="justification")]
        public virtual ICollection<NetworkLink> NetworkLinks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification ="justification")]
        public virtual ICollection<NetworkLink> NetworkLinks1 { get; set; }
    }
}
