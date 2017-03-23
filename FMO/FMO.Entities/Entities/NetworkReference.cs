namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.NetworkReference")]
    public partial class NetworkReference
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NetworkReference()
        {
            NetworkLinkReferences = new HashSet<NetworkLinkReference>();
            OSAccessRestrictions = new HashSet<OSAccessRestriction>();
            OSRestrictionForVehicles = new HashSet<OSRestrictionForVehicle>();
            OSTurnRestrictions = new HashSet<OSTurnRestriction>();
        }

        [Key]
        public int NetworkReference_Id { get; set; }

        [StringLength(10)]
        public string ReferenceType { get; set; }

        public int? NodeReferenceNetworkNode_Id { get; set; }

        [StringLength(20)]
        public string NodeReferenceTOID { get; set; }

        public DbGeometry NodeReferenceLocation { get; set; }

        public DbGeometry PointReferenceLocation { get; set; }

        public int? PointReferenceNetworkLink_Id { get; set; }

        [StringLength(20)]
        public string PointReferenceRoadLinkTOID { get; set; }

        [StringLength(50)]
        public string ExternalNetworkRef { get; set; }

        public virtual NetworkLink NetworkLink { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NetworkLinkReference> NetworkLinkReferences { get; set; }

        public virtual NetworkNode NetworkNode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OSAccessRestriction> OSAccessRestrictions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OSRestrictionForVehicle> OSRestrictionForVehicles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OSTurnRestriction> OSTurnRestrictions { get; set; }
    }
}
