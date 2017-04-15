namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.ReferenceData")]
    public partial class ReferenceData
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReferenceData()
        {
            AccessLinks = new HashSet<AccessLink>();
            AccessLinks1 = new HashSet<AccessLink>();
            AccessLinks2 = new HashSet<AccessLink>();
            AccessLinks3 = new HashSet<AccessLink>();
            AMUChangeRequests = new HashSet<AMUChangeRequest>();
            AMUChangeRequests1 = new HashSet<AMUChangeRequest>();
            AMUChangeRequests2 = new HashSet<AMUChangeRequest>();
            AreaHazards = new HashSet<AreaHazard>();
            AreaHazards1 = new HashSet<AreaHazard>();
            BlockSequences = new HashSet<BlockSequence>();
            DeliveryGroups = new HashSet<DeliveryGroup>();
            DeliveryGroups1 = new HashSet<DeliveryGroup>();
            DeliveryPoints = new HashSet<DeliveryPoint>();
            DeliveryPoints1 = new HashSet<DeliveryPoint>();
            DeliveryRoutes = new HashSet<DeliveryRoute>();
            DeliveryRoutes1 = new HashSet<DeliveryRoute>();
            DeliveryRoutes2 = new HashSet<DeliveryRoute>();
            DeliveryRoutes3 = new HashSet<DeliveryRoute>();
            DeliveryRouteActivities = new HashSet<DeliveryRouteActivity>();
            DeliveryRouteActivities1 = new HashSet<DeliveryRouteActivity>();
            GroupHazards = new HashSet<GroupHazard>();
            GroupHazards1 = new HashSet<GroupHazard>();
            NetworkLinks = new HashSet<NetworkLink>();
            NetworkLinks1 = new HashSet<NetworkLink>();
            NetworkNodes = new HashSet<NetworkNode>();
            Notifications = new HashSet<Notification>();
            Notifications1 = new HashSet<Notification>();
            POBoxes = new HashSet<POBox>();
            PointHazards = new HashSet<PointHazard>();
            PointHazards1 = new HashSet<PointHazard>();
            PointHazards2 = new HashSet<PointHazard>();
            Polygons = new HashSet<Polygon>();
            PolygonObjects = new HashSet<PolygonObject>();
            PostalAddresses = new HashSet<PostalAddress>();
            PostalAddresses1 = new HashSet<PostalAddress>();
            ReferenceData1 = new HashSet<ReferenceData>();
            Scenarios = new HashSet<Scenario>();
            SpecialInstructions = new HashSet<SpecialInstruction>();
            SpecialInstructions1 = new HashSet<SpecialInstruction>();
            UnitLocations = new HashSet<UnitLocation>();
            UnitPostcodeSectors = new HashSet<UnitPostcodeSector>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReferenceData_Id { get; set; }

        public int ReferenceDataCategory_Id { get; set; }

        [StringLength(100)]
        public string ReferenceDataName { get; set; }

        [StringLength(100)]
        public string ReferenceDataValue { get; set; }

        [StringLength(300)]
        public string DataDescription { get; set; }

        [StringLength(100)]
        public string DisplayText { get; set; }

        public int? DataParentId { get; set; }

        public Guid ID { get; set; }

        public Guid ReferenceDataCategory_GUID { get; set; }

        public Guid? DataParent_GUID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessLink> AccessLinks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessLink> AccessLinks1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessLink> AccessLinks2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessLink> AccessLinks3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AMUChangeRequest> AMUChangeRequests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AMUChangeRequest> AMUChangeRequests1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AMUChangeRequest> AMUChangeRequests2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AreaHazard> AreaHazards { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AreaHazard> AreaHazards1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlockSequence> BlockSequences { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryGroup> DeliveryGroups { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryGroup> DeliveryGroups1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryPoint> DeliveryPoints { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryPoint> DeliveryPoints1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryRoute> DeliveryRoutes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryRoute> DeliveryRoutes1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryRoute> DeliveryRoutes2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryRoute> DeliveryRoutes3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryRouteActivity> DeliveryRouteActivities { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryRouteActivity> DeliveryRouteActivities1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupHazard> GroupHazards { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupHazard> GroupHazards1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NetworkLink> NetworkLinks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NetworkLink> NetworkLinks1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NetworkNode> NetworkNodes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notification> Notifications { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notification> Notifications1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<POBox> POBoxes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PointHazard> PointHazards { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PointHazard> PointHazards1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PointHazard> PointHazards2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Polygon> Polygons { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PolygonObject> PolygonObjects { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostalAddress> PostalAddresses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostalAddress> PostalAddresses1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReferenceData> ReferenceData1 { get; set; }

        public virtual ReferenceData ReferenceData2 { get; set; }

        public virtual ReferenceDataCategory ReferenceDataCategory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Scenario> Scenarios { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SpecialInstruction> SpecialInstructions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SpecialInstruction> SpecialInstructions1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnitLocation> UnitLocations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnitPostcodeSector> UnitPostcodeSectors { get; set; }
    }
}
