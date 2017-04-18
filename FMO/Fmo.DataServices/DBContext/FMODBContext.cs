namespace Fmo.DataServices.DBContext
{
    using System.Configuration;
    using System.Data.Entity;
    using Fmo.Entities;

    public partial class FMODBContext : DbContext
    {
        public FMODBContext()
            : base(ConfigurationManager.ConnectionStrings["FMODBContext"].ConnectionString)
        {
        }

        public virtual DbSet<AccessLink> AccessLinks { get; set; }
        public virtual DbSet<Action> Actions { get; set; }
        public virtual DbSet<AddressLocation> AddressLocations { get; set; }
        public virtual DbSet<AMUChangeRequest> AMUChangeRequests { get; set; }
        public virtual DbSet<AreaHazard> AreaHazards { get; set; }
        public virtual DbSet<Block> Blocks { get; set; }
        public virtual DbSet<BlockSequence> BlockSequences { get; set; }
        public virtual DbSet<CollectionRoute> CollectionRoutes { get; set; }
        public virtual DbSet<DeliveryGroup> DeliveryGroups { get; set; }
        public virtual DbSet<DeliveryPoint> DeliveryPoints { get; set; }
        public virtual DbSet<DeliveryRoute> DeliveryRoutes { get; set; }
        public virtual DbSet<DeliveryRouteActivity> DeliveryRouteActivities { get; set; }
        public virtual DbSet<DeliveryRouteBlock> DeliveryRouteBlocks { get; set; }
        public virtual DbSet<DeliveryRouteNetworkLink> DeliveryRouteNetworkLinks { get; set; }
        public virtual DbSet<Function> Functions { get; set; }
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
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleFunction> RoleFunctions { get; set; }
        public virtual DbSet<Scenario> Scenarios { get; set; }
        public virtual DbSet<SpecialInstruction> SpecialInstructions { get; set; }
        public virtual DbSet<StreetName> StreetNames { get; set; }
        public virtual DbSet<StreetNameNetworkLink> StreetNameNetworkLinks { get; set; }
        public virtual DbSet<UnitLocation> UnitLocations { get; set; }
        public virtual DbSet<UnitLocationPostcode> UnitLocationPostcodes { get; set; }
        public virtual DbSet<UnitPostcodeSector> UnitPostcodeSectors { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRoleUnit> UserRoleUnits { get; set; }
        public virtual DbSet<TempPostalAddress> TempPostalAddresses { get; set; }

        public virtual DbSet<AccessFunction> AccessFunctions { get; set; }

        public virtual DbSet<FileProcessingLog> FileProcessingLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessLink>()
                .Property(e => e.ActualLengthMeter)
                .HasPrecision(18, 8);

            modelBuilder.Entity<AccessLink>()
                .Property(e => e.WorkloadLengthMeter)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Action>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Action>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Action>()
                .Property(e => e.DisplayText)
                .IsUnicode(false);

            modelBuilder.Entity<Action>()
                .HasMany(e => e.Functions)
                .WithRequired(e => e.Action)
                .HasForeignKey(e => e.Action_GUID)
                .WillCascadeOnDelete(false);

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
                .HasForeignKey(e => e.PairedBlock_GUID);

            modelBuilder.Entity<Block>()
                .HasMany(e => e.BlockSequences)
                .WithRequired(e => e.Block)
                .HasForeignKey(e => e.Block_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Block>()
                .HasMany(e => e.DeliveryRouteActivities)
                .WithOptional(e => e.Block)
                .HasForeignKey(e => e.Block_GUID);

            modelBuilder.Entity<Block>()
                .HasMany(e => e.DeliveryRouteBlocks)
                .WithRequired(e => e.Block)
                .HasForeignKey(e => e.Block_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BlockSequence>()
                .Property(e => e.OrderIndex)
                .HasPrecision(16, 8);

            modelBuilder.Entity<CollectionRoute>()
                .Property(e => e.RouteName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CollectionRoute>()
                .Property(e => e.RouteNumber)
                .IsFixedLength()
                .IsUnicode(false);

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
                .HasMany(e => e.BlockSequences)
                .WithOptional(e => e.DeliveryGroup)
                .HasForeignKey(e => e.DeliveryGroup_GUID);

            modelBuilder.Entity<DeliveryGroup>()
                .HasMany(e => e.DeliveryPoints)
                .WithOptional(e => e.DeliveryGroup)
                .HasForeignKey(e => e.DeliveryGroup_GUID);

            modelBuilder.Entity<DeliveryGroup>()
                .HasMany(e => e.DeliveryRouteActivities)
                .WithOptional(e => e.DeliveryGroup)
                .HasForeignKey(e => e.DeliveryGroup_GUID);

            modelBuilder.Entity<DeliveryGroup>()
                .HasMany(e => e.GroupHazards)
                .WithRequired(e => e.DeliveryGroup)
                .HasForeignKey(e => e.DeliveryGroup_GUID)
                .WillCascadeOnDelete(false);

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
                .WithOptional(e => e.DeliveryPoint)
                .HasForeignKey(e => e.OperationalObject_GUID);

            modelBuilder.Entity<DeliveryPoint>()
                .HasMany(e => e.RMGDeliveryPoints)
                .WithRequired(e => e.DeliveryPoint)
                .HasForeignKey(e => e.DeliveryPoint_GUID)
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

            modelBuilder.Entity<DeliveryRoute>()
                .HasMany(e => e.DeliveryRouteBlocks)
                .WithRequired(e => e.DeliveryRoute)
                .HasForeignKey(e => e.DeliveryRoute_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DeliveryRoute>()
                .HasMany(e => e.DeliveryRouteActivities)
                .WithOptional(e => e.DeliveryRoute)
                .HasForeignKey(e => e.DeliveryRoute_GUID);

            modelBuilder.Entity<DeliveryRouteActivity>()
                .Property(e => e.RouteActivityOrderIndex)
                .HasPrecision(8, 8);

            modelBuilder.Entity<DeliveryRouteActivity>()
                .HasMany(e => e.DeliveryRouteNetworkLinks)
                .WithOptional(e => e.DeliveryRouteActivity)
                .HasForeignKey(e => e.DeliveryRouteActivity_GUID);

            modelBuilder.Entity<DeliveryRouteNetworkLink>()
                .Property(e => e.LinkOrderIndex)
                .HasPrecision(8, 8);

            modelBuilder.Entity<Function>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Function>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Function>()
                .HasMany(e => e.RoleFunctions)
                .WithRequired(e => e.Function)
                .HasForeignKey(e => e.Function_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GroupHazard>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<NetworkLink>()
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NetworkLink>()
                .Property(e => e.LinkLength)
                .HasPrecision(18, 4);

            modelBuilder.Entity<NetworkLink>()
                .HasMany(e => e.AccessLinks)
                .WithRequired(e => e.NetworkLink)
                .HasForeignKey(e => e.NetworkLink_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NetworkLink>()
                .HasMany(e => e.DeliveryRouteNetworkLinks)
                .WithOptional(e => e.NetworkLink)
                .HasForeignKey(e => e.NetworkLink_GUID);

            modelBuilder.Entity<NetworkLink>()
                .HasMany(e => e.NetworkLinkReferences)
                .WithRequired(e => e.NetworkLink)
                .HasForeignKey(e => e.NetworkLink_GUID)
                .WillCascadeOnDelete(false);

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
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NetworkNode>()
                .HasMany(e => e.NetworkLinks)
                .WithOptional(e => e.NetworkNode)
                .HasForeignKey(e => e.StartNode_GUID);

            modelBuilder.Entity<NetworkNode>()
                .HasMany(e => e.NetworkLinks1)
                .WithOptional(e => e.NetworkNode1)
                .HasForeignKey(e => e.EndNode_GUID);

            modelBuilder.Entity<NetworkNode>()
                .HasMany(e => e.NetworkReferences)
                .WithOptional(e => e.NetworkNode)
                .HasForeignKey(e => e.NetworkNode_GUID);

            modelBuilder.Entity<NetworkNode>()
                .HasOptional(e => e.OSConnectingNode)
                .WithRequired(e => e.NetworkNode);

            modelBuilder.Entity<NetworkNode>()
                .HasOptional(e => e.OSPathNode)
                .WithRequired(e => e.NetworkNode);

            modelBuilder.Entity<NetworkNode>()
                .HasOptional(e => e.OSRoadNode)
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
                .HasForeignKey(e => e.NetworkReference_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NetworkReference>()
                .HasMany(e => e.OSAccessRestrictions)
                .WithOptional(e => e.NetworkReference)
                .HasForeignKey(e => e.NetworkReference_GUID);

            modelBuilder.Entity<NetworkReference>()
                .HasMany(e => e.OSRestrictionForVehicles)
                .WithOptional(e => e.NetworkReference)
                .HasForeignKey(e => e.NetworkReference_GUID);

            modelBuilder.Entity<NetworkReference>()
                .HasMany(e => e.OSTurnRestrictions)
                .WithOptional(e => e.NetworkReference)
                .HasForeignKey(e => e.NetworkReference_GUID);

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

            modelBuilder.Entity<OSPathNode>()
                .HasMany(e => e.OSConnectingLinks)
                .WithOptional(e => e.OSPathNode)
                .HasForeignKey(e => e.PathNode_GUID);

            modelBuilder.Entity<OSPathNode>()
                .HasMany(e => e.OSPathLinks)
                .WithOptional(e => e.OSPathNode)
                .HasForeignKey(e => e.StartNode_GUID);

            modelBuilder.Entity<OSPathNode>()
                .HasMany(e => e.OSPathLinks1)
                .WithOptional(e => e.OSPathNode1)
                .HasForeignKey(e => e.EndNode_GUID);

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

            modelBuilder.Entity<OSRoadLink>()
                .HasMany(e => e.NetworkLinkReferences)
                .WithOptional(e => e.OSRoadLink)
                .HasForeignKey(e => e.OSRoadLink_GUID);

            modelBuilder.Entity<OSRoadLink>()
                .HasMany(e => e.OSConnectingNodes)
                .WithOptional(e => e.OSRoadLink)
                .HasForeignKey(e => e.RoadLink_GUID);

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

            modelBuilder.Entity<OSRoadNode>()
                .HasMany(e => e.OSRoadLinks)
                .WithOptional(e => e.OSRoadNode)
                .HasForeignKey(e => e.StartNode_GUID);

            modelBuilder.Entity<OSRoadNode>()
                .HasMany(e => e.OSRoadLinks1)
                .WithOptional(e => e.OSRoadNode1)
                .HasForeignKey(e => e.EndNode_GUID);

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
                .HasForeignKey(e => e.Polygon_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Polygon>()
                .HasMany(e => e.DeliveryGroups)
                .WithRequired(e => e.Polygon)
                .HasForeignKey(e => e.Polygon_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Polygon>()
                .HasMany(e => e.PolygonObjects)
                .WithRequired(e => e.Polygon)
                .HasForeignKey(e => e.Polygon_GUID)
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
                .HasForeignKey(e => e.CurrentAddress_GUID);

            modelBuilder.Entity<PostalAddress>()
                .HasMany(e => e.AMUChangeRequests1)
                .WithOptional(e => e.PostalAddress1)
                .HasForeignKey(e => e.NewAddress_GUID);

            modelBuilder.Entity<PostalAddress>()
                .HasMany(e => e.DeliveryPoints)
                .WithRequired(e => e.PostalAddress)
                .HasForeignKey(e => e.Address_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PostalAddress>()
                .HasMany(e => e.POBoxes)
                .WithOptional(e => e.PostalAddress)
                .HasForeignKey(e => e.POBoxLinkedAddress_GUID);

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
                .HasForeignKey(e => e.RequestPostcode_GUID);

            modelBuilder.Entity<Postcode>()
                .HasMany(e => e.PostalAddresses)
                .WithRequired(e => e.Postcode1)
                .HasForeignKey(e => e.PostCodeGUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Postcode>()
                .HasMany(e => e.UnitLocationPostcodes)
                .WithRequired(e => e.Postcode)
                .HasForeignKey(e => e.PoscodeUnit_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PostcodeArea>()
                .Property(e => e.Area)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostcodeArea>()
                .HasMany(e => e.PostcodeDistricts)
                .WithRequired(e => e.PostcodeArea)
                .HasForeignKey(e => e.AreaGUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PostcodeDistrict>()
                .Property(e => e.District)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostcodeDistrict>()
                .Property(e => e.Area)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostcodeDistrict>()
                .HasMany(e => e.PostcodeSectors)
                .WithRequired(e => e.PostcodeDistrict)
                .HasForeignKey(e => e.DistrictGUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PostcodeSector>()
                .Property(e => e.Sector)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostcodeSector>()
                .Property(e => e.District)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostcodeSector>()
                .HasMany(e => e.Postcodes)
                .WithRequired(e => e.PostcodeSector)
                .HasForeignKey(e => e.SectorGUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PostcodeSector>()
                .HasMany(e => e.UnitPostcodeSectors)
                .WithRequired(e => e.PostcodeSector1)
                .HasForeignKey(e => e.PostcodeSector_GUID)
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
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.LinkStatus_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.AccessLinks1)
                .WithOptional(e => e.ReferenceData1)
                .HasForeignKey(e => e.AccessLinkType_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.AccessLinks2)
                .WithOptional(e => e.ReferenceData2)
                .HasForeignKey(e => e.LinkDirection_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.AccessLinks3)
                .WithOptional(e => e.ReferenceData3)
                .HasForeignKey(e => e.OperationalObjectType_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.AMUChangeRequests)
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.ChangeRequestType_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.AMUChangeRequests1)
                .WithOptional(e => e.ReferenceData1)
                .HasForeignKey(e => e.ChangeRequestStatus_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.AMUChangeRequests2)
                .WithOptional(e => e.ReferenceData2)
                .HasForeignKey(e => e.ChangeReasonCode_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.AreaHazards)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.Category_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.AreaHazards1)
                .WithRequired(e => e.ReferenceData1)
                .HasForeignKey(e => e.SubCategory_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.BlockSequences)
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.OperationalObjectType_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.DeliveryGroups)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.GroupType_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.DeliveryGroups1)
                .WithOptional(e => e.ReferenceData1)
                .HasForeignKey(e => e.ServicePointType_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.DeliveryPoints)
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.LocationProvider_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.DeliveryPoints1)
                .WithOptional(e => e.ReferenceData1)
                .HasForeignKey(e => e.OperationalStatus_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.DeliveryRoutes)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.OperationalStatus_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.DeliveryRoutes1)
                .WithRequired(e => e.ReferenceData1)
                .HasForeignKey(e => e.RouteMethodType_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.DeliveryRoutes2)
                .WithOptional(e => e.ReferenceData2)
                .HasForeignKey(e => e.TravelOutTransportType_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.DeliveryRoutes3)
                .WithOptional(e => e.ReferenceData3)
                .HasForeignKey(e => e.TravelInTransportType_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.DeliveryRouteActivities)
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.OperationalObjectType_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.DeliveryRouteActivities1)
                .WithOptional(e => e.ReferenceData1)
                .HasForeignKey(e => e.ActivityType_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.GroupHazards)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.Category_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.GroupHazards1)
                .WithRequired(e => e.ReferenceData1)
                .HasForeignKey(e => e.SubCategory_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.NetworkLinks)
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.NetworkLinkType_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.NetworkLinks1)
                .WithOptional(e => e.ReferenceData1)
                .HasForeignKey(e => e.DataProvider_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.NetworkNodes)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.NetworkNodeType_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.Notifications)
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.NotificationType_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.Notifications1)
                .WithOptional(e => e.ReferenceData1)
                .HasForeignKey(e => e.NotificationPriority_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.POBoxes)
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.POBoxType_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.PointHazards)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.Category_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.PointHazards1)
                .WithRequired(e => e.ReferenceData1)
                .HasForeignKey(e => e.SubCategory_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.PointHazards2)
                .WithRequired(e => e.ReferenceData2)
                .HasForeignKey(e => e.OperationalObjectType_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.Polygons)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.PolygonType_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.PolygonObjects)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.OperationalObjectType_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.PostalAddresses)
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.AddressStatus_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.PostalAddresses1)
                .WithRequired(e => e.ReferenceData1)
                .HasForeignKey(e => e.AddressType_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.ReferenceData1)
                .WithOptional(e => e.ReferenceData2)
                .HasForeignKey(e => e.DataParent_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.Scenarios)
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.OperationalState_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.SpecialInstructions)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.OperationalObjectType_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.SpecialInstructions1)
                .WithOptional(e => e.ReferenceData1)
                .HasForeignKey(e => e.InstructionType_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.UnitLocations)
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.LocationType_GUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.UnitPostcodeSectors)
                .WithRequired(e => e.ReferenceData)
                .HasForeignKey(e => e.UnitStatus_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceDataCategory>()
                .Property(e => e.CategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceDataCategory>()
                .HasMany(e => e.ReferenceDatas)
                .WithRequired(e => e.ReferenceDataCategory)
                .HasForeignKey(e => e.ReferenceDataCategory_GUID)
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

            modelBuilder.Entity<RoadName>()
                .HasMany(e => e.NetworkLinks)
                .WithOptional(e => e.RoadName)
                .HasForeignKey(e => e.RoadName_GUID);

            modelBuilder.Entity<Role>()
                .Property(e => e.RoleName)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.RoleFunctions)
                .WithRequired(e => e.Role)
                .HasForeignKey(e => e.Role_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.UserRoleUnits)
                .WithRequired(e => e.Role)
                .HasForeignKey(e => e.Role_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Scenario>()
                .Property(e => e.ScenarioName)
                .IsUnicode(false);

            modelBuilder.Entity<Scenario>()
                .HasMany(e => e.DeliveryRoutes)
                .WithOptional(e => e.Scenario)
                .HasForeignKey(e => e.DeliveryScenario_GUID);

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

            modelBuilder.Entity<StreetName>()
                .HasMany(e => e.NetworkLinks)
                .WithOptional(e => e.StreetName)
                .HasForeignKey(e => e.StreetName_GUID);

            modelBuilder.Entity<StreetNameNetworkLink>()
                .Property(e => e.USRN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<StreetNameNetworkLink>()
                .Property(e => e.RoadLinkTOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<UnitLocation>()
                .Property(e => e.ExternalId)
                .IsUnicode(false);

            modelBuilder.Entity<UnitLocation>()
                .Property(e => e.UnitName)
                .IsUnicode(false);

            modelBuilder.Entity<UnitLocation>()
                .HasMany(e => e.Scenarios)
                .WithOptional(e => e.UnitLocation)
                .HasForeignKey(e => e.Unit_GUID);

            modelBuilder.Entity<UnitLocation>()
                .HasMany(e => e.UnitLocationPostcodes)
                .WithRequired(e => e.UnitLocation)
                .HasForeignKey(e => e.Unit_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UnitLocation>()
                .HasMany(e => e.UnitPostcodeSectors)
                .WithRequired(e => e.UnitLocation)
                .HasForeignKey(e => e.Unit_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UnitLocation>()
                .HasMany(e => e.UserRoleUnits)
                .WithRequired(e => e.UnitLocation)
                .HasForeignKey(e => e.Unit_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UnitLocationPostcode>()
                .Property(e => e.PostcodeUnit)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<UnitPostcodeSector>()
                .Property(e => e.PostcodeSector)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserRoleUnits)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_GUID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AccessFunction>()
                    .Property(e => e.FunctionName)
                    .IsUnicode(false);

            modelBuilder.Entity<AccessFunction>()
                    .Property(e => e.ActionName)
                    .IsUnicode(false);

            modelBuilder.Entity<AccessFunction>()
                    .Property(e => e.RoleName)
                    .IsUnicode(false);

        }
    }
}