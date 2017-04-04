namespace Fmo.DataServices.DBContext
{
    using System.Data.Entity;
    using Fmo.Entities;

    public partial class FMODBContext : AuditContext
    {
        public FMODBContext()
            : base("name=FMODBContext")
        {
        }

        public FMODBContext(string connString)
            : base(connString)
        {
        }

        public virtual DbSet<AccessLink> AccessLinks { get; set; }

        public virtual DbSet<AddressLocation> AddressLocations { get; set; }

        public virtual DbSet<AMUChangeRequest> AMUChangeRequests { get; set; }

        public virtual DbSet<AreaHazard> AreaHazards { get; set; }

        public virtual DbSet<Block> Blocks { get; set; }

        public virtual DbSet<BlockSequence> BlockSequences { get; set; }

        public virtual DbSet<DeliveryGroup> DeliveryGroups { get; set; }

        public virtual DbSet<DeliveryPoint> DeliveryPoints { get; set; }

        public virtual DbSet<DeliveryRoute> DeliveryRoutes { get; set; }

        public virtual DbSet<DeliveryRouteActivity> DeliveryRouteActivities { get; set; }

        public virtual DbSet<DeliveryUnitLocation> DeliveryUnitLocations { get; set; }

        public virtual DbSet<DeliveryUnitPostcodeSector> DeliveryUnitPostcodeSectors { get; set; }

        public virtual DbSet<GroupHazard> GroupHazards { get; set; }

        public virtual DbSet<NetworkLink> NetworkLinks { get; set; }

        public virtual DbSet<NetworkLinkReference> NetworkLinkReferences { get; set; }

        public virtual DbSet<NetworkNode> NetworkNodes { get; set; }

        public virtual DbSet<NetworkReference> NetworkReferences { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }

        public virtual DbSet<OSAccessRestriction> OSAccessRestrictions { get; set; }

        public virtual DbSet<OSConnectingLink> OSConnectingLinks { get; set; }

        public virtual DbSet<OSConnectingNode> OSConnectingNodes { get; set; }

        public virtual DbSet<OSPathLink> OSPathLinks { get; set; }

        public virtual DbSet<OSPathNode> OSPathNodes { get; set; }

        public virtual DbSet<OSRestrictionForVehicle> OSRestrictionForVehicles { get; set; }

        public virtual DbSet<OSRoadLink> OSRoadLinks { get; set; }

        public virtual DbSet<OSRoadNode> OSRoadNodes { get; set; }

        public virtual DbSet<OSTurnRestriction> OSTurnRestrictions { get; set; }

        public virtual DbSet<POBox> POBoxes { get; set; }

        public virtual DbSet<PointHazard> PointHazards { get; set; }

        public virtual DbSet<Polygon> Polygons { get; set; }

        public virtual DbSet<PolygonObject> PolygonObjects { get; set; }

        public virtual DbSet<PostalAddress> PostalAddresses { get; set; }

        public virtual DbSet<Postcode> Postcodes { get; set; }

        public virtual DbSet<PostcodeArea> PostcodeAreas { get; set; }

        public virtual DbSet<PostcodeDistrict> PostcodeDistricts { get; set; }

        public virtual DbSet<PostcodeSector> PostcodeSectors { get; set; }

        public virtual DbSet<ReferenceData> ReferenceDatas { get; set; }

        public virtual DbSet<ReferenceDataCategory> ReferenceDataCategories { get; set; }

        public virtual DbSet<RMGDeliveryPoint> RMGDeliveryPoints { get; set; }

        public virtual DbSet<RMGLink> RMGLinks { get; set; }

        public virtual DbSet<RMGNode> RMGNodes { get; set; }

        public virtual DbSet<RoadName> RoadNames { get; set; }

        public virtual DbSet<Scenario> Scenarios { get; set; }

        public virtual DbSet<SpecialInstruction> SpecialInstructions { get; set; }

        public virtual DbSet<StreetName> StreetNames { get; set; }

        //public virtual DbSet<AdvanceSearch> AdvanceSearch { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessLink>()
                .Property(e => e.ActualLengthMeter)
                .HasPrecision(18, 8);

            modelBuilder.Entity<AccessLink>()
                .Property(e => e.WorkloadLengthMeter)
                .HasPrecision(18, 8);

            modelBuilder.Entity<AddressLocation>()
                .Property(e => e.Lattitude)
                .HasPrecision(38, 8);

            modelBuilder.Entity<AddressLocation>()
                .Property(e => e.Longitude)
                .HasPrecision(38, 8);

            modelBuilder.Entity<AMUChangeRequest>()
                .Property(e => e.UnitCommentText)
                .IsUnicode(false);

            modelBuilder.Entity<AMUChangeRequest>()
                .Property(e => e.AddressChanges)
                .IsUnicode(false);

            modelBuilder.Entity<AMUChangeRequest>()
                .Property(e => e.AMUClarificationText)
                .IsUnicode(false);

            modelBuilder.Entity<AMUChangeRequest>()
                .Property(e => e.RequestPostcode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Block>()
                .Property(e => e.BlockType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Block>()
                .Property(e => e.BlockSpanInMinutes)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Block>()
                .HasMany(e => e.Block1)
                .WithOptional(e => e.Block2)
                .HasForeignKey(e => e.PairedBlock_Id);

            modelBuilder.Entity<Block>()
                .HasMany(e => e.BlockSequences)
                .WithRequired(e => e.Block)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Block>()
                .HasMany(e => e.DeliveryRoutes)
                .WithMany(e => e.Blocks)
                .Map(m => m.ToTable("DeliveryRouteBlock").MapLeftKey("Block_Id").MapRightKey("DeliveryRoute_Id"));

            modelBuilder.Entity<BlockSequence>()
                .Property(e => e.OrderIndex)
                .HasPrecision(8, 8);

            modelBuilder.Entity<DeliveryGroup>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<DeliveryGroup>()
                .Property(e => e.WorkloadOverrideReason)
                .IsUnicode(false);

            modelBuilder.Entity<DeliveryGroup>()
                .HasOptional(e => e.AccessLink)
                .WithRequired(e => e.DeliveryGroup);

            modelBuilder.Entity<DeliveryGroup>()
                .HasMany(e => e.GroupHazards)
                .WithRequired(e => e.DeliveryGroup)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DeliveryPoint>()
                .Property(e => e.LocationProvider)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DeliveryPoint>()
                .Property(e => e.OperationalStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DeliveryPoint>()
                .Property(e => e.Latitude)
                .HasPrecision(38, 8);

            modelBuilder.Entity<DeliveryPoint>()
                .Property(e => e.Longitude)
                .HasPrecision(38, 8);

            modelBuilder.Entity<DeliveryPoint>()
                .Property(e => e.DeliveryPointUseIndicator)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DeliveryPoint>()
                .HasMany(e => e.AccessLinks)
                .WithRequired(e => e.DeliveryPoint)
                .HasForeignKey(e => e.OperationalObjectId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DeliveryPoint>()
                .HasMany(e => e.RMGDeliveryPoints)
                .WithRequired(e => e.DeliveryPoint)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DeliveryRoute>()
                .Property(e => e.RouteName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DeliveryRoute>()
                .Property(e => e.RouteNumber)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DeliveryRoute>()
                .Property(e => e.TravelOutTimeMin)
                .HasPrecision(10, 2);

            modelBuilder.Entity<DeliveryRoute>()
                .Property(e => e.TravelInTimeMin)
                .HasPrecision(10, 2);

            modelBuilder.Entity<DeliveryRoute>()
                .Property(e => e.SpanTimeMin)
                .HasPrecision(10, 2);

            modelBuilder.Entity<DeliveryRoute>()
                .Property(e => e.DeliveryRouteBarcode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DeliveryRouteActivity>()
                .Property(e => e.RouteActivityOrderIndex)
                .HasPrecision(8, 8);

            modelBuilder.Entity<DeliveryUnitLocation>()
                .Property(e => e.ExternalId)
                .IsUnicode(false);

            modelBuilder.Entity<DeliveryUnitLocation>()
                .Property(e => e.UnitName)
                .IsUnicode(false);

            modelBuilder.Entity<DeliveryUnitLocation>()
                .HasMany(e => e.DeliveryUnitPostcodeSectors)
                .WithRequired(e => e.DeliveryUnitLocation)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DeliveryUnitPostcodeSector>()
                .Property(e => e.DeliveryUnitStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DeliveryUnitPostcodeSector>()
                .Property(e => e.PostcodeSector)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GroupHazard>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<NetworkLink>()
                .Property(e => e.NetworkLinkType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NetworkLink>()
                .Property(e => e.ExternalTOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NetworkLink>()
                .Property(e => e.DataProvider)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NetworkLink>()
                .Property(e => e.LinkLength)
                .HasPrecision(18, 4);

            modelBuilder.Entity<NetworkLink>()
                .HasMany(e => e.AccessLinks)
                .WithRequired(e => e.NetworkLink)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NetworkLink>()
                .HasMany(e => e.NetworkReferences)
                .WithOptional(e => e.NetworkLink)
                .HasForeignKey(e => e.PointReferenceNetworkLink_Id);

            modelBuilder.Entity<NetworkLink>()
                .HasMany(e => e.NetworkLinkReferences)
                .WithRequired(e => e.NetworkLink)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NetworkLink>()
                .HasOptional(e => e.OSConnectingLink)
                .WithRequired(e => e.NetworkLink);

            modelBuilder.Entity<NetworkLink>()
                .HasOptional(e => e.OSPathLink)
                .WithRequired(e => e.NetworkLink);

            modelBuilder.Entity<NetworkLink>()
                .HasOptional(e => e.OSRoadLink)
                .WithRequired(e => e.NetworkLink);

            modelBuilder.Entity<NetworkLink>()
                .HasOptional(e => e.RMGLink)
                .WithRequired(e => e.NetworkLink);

            modelBuilder.Entity<NetworkLinkReference>()
                .Property(e => e.OSRoadLinkTOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NetworkNode>()
                .Property(e => e.NetworkNodeType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NetworkNode>()
                .Property(e => e.DataProvider)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NetworkNode>()
                .HasMany(e => e.NetworkLinks)
                .WithRequired(e => e.NetworkNode)
                .HasForeignKey(e => e.StartNode_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NetworkNode>()
                .HasMany(e => e.NetworkLinks1)
                .WithRequired(e => e.NetworkNode1)
                .HasForeignKey(e => e.EndNode_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NetworkNode>()
                .HasMany(e => e.NetworkReferences)
                .WithOptional(e => e.NetworkNode)
                .HasForeignKey(e => e.NodeReferenceNetworkNode_Id);

            modelBuilder.Entity<NetworkNode>()
                .HasOptional(e => e.OSConnectingNode)
                .WithRequired(e => e.NetworkNode);

            modelBuilder.Entity<NetworkNode>()
                .HasOptional(e => e.OSPathNode)
                .WithRequired(e => e.NetworkNode);

            modelBuilder.Entity<NetworkNode>()
                .HasOptional(e => e.OSRoadNode)
                .WithRequired(e => e.NetworkNode);

            modelBuilder.Entity<NetworkNode>()
                .HasOptional(e => e.RMGNode)
                .WithRequired(e => e.NetworkNode);

            modelBuilder.Entity<NetworkReference>()
                .Property(e => e.ReferenceType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NetworkReference>()
                .Property(e => e.NodeReferenceTOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NetworkReference>()
                .Property(e => e.PointReferenceRoadLinkTOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NetworkReference>()
                .Property(e => e.ExternalNetworkRef)
                .IsUnicode(false);

            modelBuilder.Entity<NetworkReference>()
                .HasMany(e => e.NetworkLinkReferences)
                .WithRequired(e => e.NetworkReference)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.Notification_Heading)
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.Notification_Message)
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.NotificationSource)
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.PostcodeDistrict)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.PostcodeSector)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSAccessRestriction>()
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSAccessRestriction>()
                .Property(e => e.RestrictionValue)
                .IsUnicode(false);

            modelBuilder.Entity<OSConnectingLink>()
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSConnectingLink>()
                .Property(e => e.ConnectingNodeTOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSConnectingLink>()
                .Property(e => e.PathNodeTOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSConnectingNode>()
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSConnectingNode>()
                .Property(e => e.RoadLinkTOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSPathLink>()
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSPathLink>()
                .Property(e => e.Ficticious)
                .IsFixedLength();

            modelBuilder.Entity<OSPathLink>()
                .Property(e => e.FormOfWay)
                .IsUnicode(false);

            modelBuilder.Entity<OSPathLink>()
                .Property(e => e.PathName)
                .IsUnicode(false);

            modelBuilder.Entity<OSPathLink>()
                .Property(e => e.AlternateName)
                .IsUnicode(false);

            modelBuilder.Entity<OSPathLink>()
                .Property(e => e.LengthInMeters)
                .HasPrecision(38, 8);

            modelBuilder.Entity<OSPathLink>()
                .Property(e => e.StartNodeTOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSPathLink>()
                .Property(e => e.EndNodeTOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSPathLink>()
                .Property(e => e.FormPartOf)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSPathNode>()
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSPathNode>()
                .Property(e => e.formOfRoadNode)
                .IsUnicode(false);

            modelBuilder.Entity<OSPathNode>()
                .Property(e => e.Classification)
                .IsUnicode(false);

            modelBuilder.Entity<OSPathNode>()
                .Property(e => e.ReasonForChange)
                .IsUnicode(false);

            modelBuilder.Entity<OSRestrictionForVehicle>()
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSRestrictionForVehicle>()
                .Property(e => e.SourceofMeasure)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSRestrictionForVehicle>()
                .Property(e => e.Structure)
                .IsUnicode(false);

            modelBuilder.Entity<OSRestrictionForVehicle>()
                .Property(e => e.TrafficSign)
                .IsUnicode(false);

            modelBuilder.Entity<OSRoadLink>()
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSRoadLink>()
                .Property(e => e.RoadClassificaton)
                .IsUnicode(false);

            modelBuilder.Entity<OSRoadLink>()
                .Property(e => e.RouteHierarchy)
                .IsUnicode(false);

            modelBuilder.Entity<OSRoadLink>()
                .Property(e => e.FormOfWay)
                .IsUnicode(false);

            modelBuilder.Entity<OSRoadLink>()
                .Property(e => e.TrunkRoad)
                .IsFixedLength();

            modelBuilder.Entity<OSRoadLink>()
                .Property(e => e.PrimaryRoute)
                .IsFixedLength();

            modelBuilder.Entity<OSRoadLink>()
                .Property(e => e.RoadClassificationNumber)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSRoadLink>()
                .Property(e => e.RoadName)
                .IsUnicode(false);

            modelBuilder.Entity<OSRoadLink>()
                .Property(e => e.AlternateName)
                .IsUnicode(false);

            modelBuilder.Entity<OSRoadLink>()
                .Property(e => e.Directionality)
                .IsUnicode(false);

            modelBuilder.Entity<OSRoadLink>()
                .Property(e => e.LengthInMeters)
                .HasPrecision(38, 8);

            modelBuilder.Entity<OSRoadLink>()
                .Property(e => e.StartNodeTOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSRoadLink>()
                .Property(e => e.EndNodeTOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSRoadLink>()
                .Property(e => e.OperationalState)
                .IsUnicode(false);

            modelBuilder.Entity<OSRoadNode>()
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSRoadNode>()
                .Property(e => e.formOfRoadNode)
                .IsUnicode(false);

            modelBuilder.Entity<OSRoadNode>()
                .Property(e => e.Classification)
                .IsUnicode(false);

            modelBuilder.Entity<OSRoadNode>()
                .Property(e => e.access)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSRoadNode>()
                .Property(e => e.junctionName)
                .IsUnicode(false);

            modelBuilder.Entity<OSRoadNode>()
                .Property(e => e.JunctionNumber)
                .IsUnicode(false);

            modelBuilder.Entity<OSRoadNode>()
                .Property(e => e.ReasonForChange)
                .IsUnicode(false);

            modelBuilder.Entity<OSTurnRestriction>()
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSTurnRestriction>()
                .Property(e => e.Restriction)
                .IsUnicode(false);

            modelBuilder.Entity<OSTurnRestriction>()
                .Property(e => e.inclusion)
                .IsUnicode(false);

            modelBuilder.Entity<OSTurnRestriction>()
                .Property(e => e.Exclusion)
                .IsUnicode(false);

            modelBuilder.Entity<OSTurnRestriction>()
                .Property(e => e.TimeInterval)
                .IsUnicode(false);

            modelBuilder.Entity<PointHazard>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Polygon>()
                .HasMany(e => e.AreaHazards)
                .WithRequired(e => e.Polygon)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Polygon>()
                .HasMany(e => e.DeliveryGroups)
                .WithRequired(e => e.Polygon)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Polygon>()
                .HasMany(e => e.PolygonObjects)
                .WithRequired(e => e.Polygon)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PolygonObject>()
                .Property(e => e.GroupOrderIndex)
                .HasPrecision(8, 8);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.PostcodeType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.OrganisationName)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.DepartmentName)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.BuildingName)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.SubBuildingName)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.Thoroughfare)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.DependentThoroughfare)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.DependentLocality)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.DoubleDependentLocality)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.PostTown)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.Postcode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.DeliveryPointSuffix)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.SmallUserOrganisationIndicator)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .Property(e => e.POBoxNumber)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddress>()
                .HasMany(e => e.AMUChangeRequests)
                .WithOptional(e => e.PostalAddress)
                .HasForeignKey(e => e.CurrentAddress_Id);

            modelBuilder.Entity<PostalAddress>()
                .HasMany(e => e.AMUChangeRequests1)
                .WithOptional(e => e.PostalAddress1)
                .HasForeignKey(e => e.NewAddress_Id);

            modelBuilder.Entity<PostalAddress>()
                .HasMany(e => e.DeliveryPoints)
                .WithRequired(e => e.PostalAddress)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PostalAddress>()
                .HasMany(e => e.POBoxes)
                .WithOptional(e => e.PostalAddress)
                .HasForeignKey(e => e.POBoxLinkedAddress_Id);

            modelBuilder.Entity<Postcode>()
                .Property(e => e.PostcodeUnit)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Postcode>()
                .Property(e => e.OutwardCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Postcode>()
                .Property(e => e.InwardCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Postcode>()
                .Property(e => e.Sector)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Postcode>()
                .HasMany(e => e.AMUChangeRequests)
                .WithOptional(e => e.Postcode)
                .HasForeignKey(e => e.RequestPostcode);

            modelBuilder.Entity<Postcode>()
                .HasMany(e => e.PostalAddresses)
                .WithRequired(e => e.Postcode1)
                .HasForeignKey(e => e.Postcode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PostcodeArea>()
                .Property(e => e.Area)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostcodeDistrict>()
                .Property(e => e.District)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostcodeDistrict>()
                .Property(e => e.Area)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostcodeSector>()
                .Property(e => e.Sector)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostcodeSector>()
                .Property(e => e.District)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostcodeSector>()
                .HasMany(e => e.DeliveryUnitPostcodeSectors)
                .WithRequired(e => e.PostcodeSector1)
                .HasForeignKey(e => e.PostcodeSector)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PostcodeSector>()
                .HasMany(e => e.Postcodes)
                .WithRequired(e => e.PostcodeSector)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .Property(e => e.ReferenceDataName)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceData>()
                .Property(e => e.ReferenceDataValue)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceData>()
                .Property(e => e.DataDescription)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceData>()
                .Property(e => e.DisplayText)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.AccessLinks)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.LinkStatus_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.AccessLinks1)
                .WithRequired(e => e.ReferenceData1)
                .HasForeignKey(e => e.AccessLinkType_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.AccessLinks2)
                .WithRequired(e => e.ReferenceData2)
                .HasForeignKey(e => e.LinkDirection_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.AccessLinks3)
                .WithRequired(e => e.ReferenceData3)
                .HasForeignKey(e => e.OperationalObjectType_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.AMUChangeRequests)
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.ChangeRequestType_Id);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.AMUChangeRequests1)
                .WithOptional(e => e.ReferenceData1)
                .HasForeignKey(e => e.ChangeRequestStatus_Id);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.AMUChangeRequests2)
                .WithOptional(e => e.ReferenceData2)
                .HasForeignKey(e => e.ChangeReasonCode_Id);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.AreaHazards)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.Category_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.AreaHazards1)
                .WithRequired(e => e.ReferenceData1)
                .HasForeignKey(e => e.SubCategory_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.BlockSequences)
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.OperationalObjectType_Id);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.DeliveryGroups)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.GroupType_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.DeliveryGroups1)
                .WithOptional(e => e.ReferenceData1)
                .HasForeignKey(e => e.ServicePointType_Id);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.DeliveryRoutes)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.OperationalStatus_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.DeliveryRoutes1)
                .WithRequired(e => e.ReferenceData1)
                .HasForeignKey(e => e.RouteMethodType_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.DeliveryRoutes2)
                .WithOptional(e => e.ReferenceData2)
                .HasForeignKey(e => e.TravelOutTransportType_Id);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.DeliveryRoutes3)
                .WithOptional(e => e.ReferenceData3)
                .HasForeignKey(e => e.TravelInTransportType_Id);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.DeliveryRouteActivities)
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.OperationalObjectType_Id);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.DeliveryRouteActivities1)
                .WithOptional(e => e.ReferenceData1)
                .HasForeignKey(e => e.ActivityType_Id);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.GroupHazards)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.Category_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.GroupHazards1)
                .WithRequired(e => e.ReferenceData1)
                .HasForeignKey(e => e.SubCategory_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.Notifications)
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.NotificationType_Id);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.Notifications1)
                .WithOptional(e => e.ReferenceData1)
                .HasForeignKey(e => e.NotificationPriority_Id);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.POBoxes)
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.POBoxType_Id);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.PointHazards)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.Category_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.PointHazards1)
                .WithRequired(e => e.ReferenceData1)
                .HasForeignKey(e => e.SubCategory_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.PointHazards2)
                .WithRequired(e => e.ReferenceData2)
                .HasForeignKey(e => e.OperationalObjectType_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.Polygons)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.PolygonType_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.PolygonObjects)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.OperationalObjectType_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.PostalAddresses)
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.AddressStatus_Id);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.PostalAddresses1)
                .WithOptional(e => e.ReferenceData1)
                .HasForeignKey(e => e.AddressType_Id);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.Scenarios)
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.OperationalState_Id);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.ReferenceData1)
                .WithOptional(e => e.ReferenceData2)
                .HasForeignKey(e => e.DataParentId);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.Scenarios1)
                .WithOptional(e => e.ReferenceData1)
                .HasForeignKey(e => e.ScenarioUnitType_Id);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.SpecialInstructions)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.OperationalObjectType_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.SpecialInstructions1)
                .WithOptional(e => e.ReferenceData1)
                .HasForeignKey(e => e.InstructionType_Id);

            modelBuilder.Entity<ReferenceDataCategory>()
                .Property(e => e.CategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceDataCategory>()
                .HasMany(e => e.ReferenceDatas)
                .WithRequired(e => e.ReferenceDataCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RMGDeliveryPoint>()
                .Property(e => e.Latitude)
                .HasPrecision(38, 8);

            modelBuilder.Entity<RMGDeliveryPoint>()
                .Property(e => e.Longitude)
                .HasPrecision(38, 8);

            modelBuilder.Entity<RMGDeliveryPoint>()
                .HasOptional(e => e.AccessLink)
                .WithRequired(e => e.RMGDeliveryPoint);

            modelBuilder.Entity<RMGLink>()
                .Property(e => e.LinkType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RMGLink>()
                .Property(e => e.StartNodeReference)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RMGLink>()
                .Property(e => e.StartNodeType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RMGLink>()
                .Property(e => e.EndNodeType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RMGLink>()
                .Property(e => e.EndNodeReference)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RMGNode>()
                .Property(e => e.OSLinkReference)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RMGNode>()
                .Property(e => e.OSLinkType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RoadName>()
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RoadName>()
                .Property(e => e.NationalRoadCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RoadName>()
                .Property(e => e.roadClassification)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RoadName>()
                .Property(e => e.DesignatedName)
                .IsUnicode(false);

            modelBuilder.Entity<Scenario>()
                .Property(e => e.ScenarioName)
                .IsUnicode(false);

            modelBuilder.Entity<SpecialInstruction>()
                .Property(e => e.InstructionText)
                .IsUnicode(false);

            modelBuilder.Entity<StreetName>()
                .Property(e => e.USRN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<StreetName>()
                .Property(e => e.NationalRoadCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<StreetName>()
                .Property(e => e.DesignatedName)
                .IsUnicode(false);

            modelBuilder.Entity<StreetName>()
                .Property(e => e.LocalName)
                .IsUnicode(false);

            modelBuilder.Entity<StreetName>()
                .Property(e => e.Descriptor)
                .IsUnicode(false);

            modelBuilder.Entity<StreetName>()
                .Property(e => e.RoadClassification)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<StreetName>()
                .Property(e => e.StreetType)
                .IsUnicode(false);

            modelBuilder.Entity<StreetName>()
                .Property(e => e.StreetNameProvider)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<StreetName>()
                .Property(e => e.OperationalState)
                .IsUnicode(false);

            modelBuilder.Entity<StreetName>()
                .Property(e => e.OperationalStateReason)
                .IsUnicode(false);

            modelBuilder.Entity<StreetName>()
                .Property(e => e.Locality)
                .IsUnicode(false);

            modelBuilder.Entity<StreetName>()
                .Property(e => e.Town)
                .IsUnicode(false);

            modelBuilder.Entity<StreetName>()
                .Property(e => e.AdministrativeArea)
                .IsUnicode(false);
        }
    }
}