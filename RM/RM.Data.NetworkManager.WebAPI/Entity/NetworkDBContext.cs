namespace RM.DataManagement.NetworkManager.WebAPI.Entities
{
    using System.Data.Entity;

    public partial class NetworkDBContext : DbContext
    {
        public NetworkDBContext()
            : base("name=NetworkDBContext")
        {
        }

        public virtual DbSet<Location> Locations { get; set; }

        public virtual DbSet<NetworkLink> NetworkLinks { get; set; }

        public virtual DbSet<NetworkLinkReference> NetworkLinkReferences { get; set; }

        public virtual DbSet<NetworkNode> NetworkNodes { get; set; }

        public virtual DbSet<NetworkReference> NetworkReferences { get; set; }

        public virtual DbSet<OSAccessRestriction> OSAccessRestrictions { get; set; }

        public virtual DbSet<OSConnectingLink> OSConnectingLinks { get; set; }

        public virtual DbSet<OSConnectingNode> OSConnectingNodes { get; set; }

        public virtual DbSet<OSPathLink> OSPathLinks { get; set; }

        public virtual DbSet<OSPathNode> OSPathNodes { get; set; }

        public virtual DbSet<OSRestrictionForVehicle> OSRestrictionForVehicles { get; set; }

        public virtual DbSet<OSRoadLink> OSRoadLinks { get; set; }

        public virtual DbSet<OSRoadNode> OSRoadNodes { get; set; }

        public virtual DbSet<OSTurnRestriction> OSTurnRestrictions { get; set; }

        public virtual DbSet<RMGLink> RMGLinks { get; set; }

        public virtual DbSet<RMGNode> RMGNodes { get; set; }

        public virtual DbSet<RoadName> RoadNames { get; set; }

        public virtual DbSet<StreetName> StreetNames { get; set; }

        public virtual DbSet<StreetNameNetworkLink> StreetNameNetworkLinks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>()
                .HasOptional(e => e.NetworkNode)
                .WithRequired(e => e.Location);

            modelBuilder.Entity<NetworkLink>()
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NetworkLink>()
                .Property(e => e.LinkLength)
                .HasPrecision(18, 4);

            modelBuilder.Entity<NetworkLink>()
                .Property(e => e.LinkName)
                .IsUnicode(false);

            modelBuilder.Entity<NetworkLink>()
                .HasMany(e => e.NetworkReferences)
                .WithOptional(e => e.NetworkLink)
                .HasForeignKey(e => e.PointReferenceNetworkLinkID);

            modelBuilder.Entity<NetworkLink>()
                .HasMany(e => e.NetworkLinkReferences)
                .WithRequired(e => e.NetworkLink)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NetworkLinkReference>()
                .Property(e => e.OSRoadLinkTOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NetworkNode>()
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NetworkNode>()
                .HasMany(e => e.NetworkLinks)
                .WithOptional(e => e.NetworkNode)
                .HasForeignKey(e => e.StartNodeID);

            modelBuilder.Entity<NetworkNode>()
                .HasMany(e => e.NetworkLinks1)
                .WithOptional(e => e.NetworkNode1)
                .HasForeignKey(e => e.EndNodeID);

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
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NetworkReference>()
                .HasMany(e => e.OSRestrictionForVehicles)
                .WithOptional(e => e.NetworkReference)
                .HasForeignKey(e => e.NetworkReference_GUID);

            modelBuilder.Entity<OSAccessRestriction>()
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSAccessRestriction>()
                .Property(e => e.RestrictionValue)
                .IsUnicode(false);

            modelBuilder.Entity<OSAccessRestriction>()
                .Property(e => e.InclusionVehicleQualifier)
                .IsUnicode(false);

            modelBuilder.Entity<OSAccessRestriction>()
                .Property(e => e.ExclusionVehicleQualifier)
                .IsUnicode(false);

            modelBuilder.Entity<OSAccessRestriction>()
                .Property(e => e.TimeInterval)
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

            modelBuilder.Entity<OSConnectingNode>()
                .HasMany(e => e.OSConnectingLinks)
                .WithRequired(e => e.OSConnectingNode)
                .HasForeignKey(e => e.ConnectingNodeID)
                .WillCascadeOnDelete(false);

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
                .HasForeignKey(e => e.PathNodeID);

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
                .Property(e => e.RestrictionType)
                .IsUnicode(false);

            modelBuilder.Entity<OSRestrictionForVehicle>()
                .Property(e => e.SourceofMeasure)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OSRestrictionForVehicle>()
                .Property(e => e.Inclusion)
                .IsUnicode(false);

            modelBuilder.Entity<OSRestrictionForVehicle>()
                .Property(e => e.Exclusion)
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
                .HasMany(e => e.OSConnectingNodes)
                .WithOptional(e => e.OSRoadLink)
                .HasForeignKey(e => e.RoadLinkID);

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
                .HasForeignKey(e => e.RoadNameGUID);

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
                .HasForeignKey(e => e.StreetNameGUID);

            modelBuilder.Entity<StreetNameNetworkLink>()
                .Property(e => e.USRN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<StreetNameNetworkLink>()
                .Property(e => e.RoadLinkTOID)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}