namespace RM.DataManagement.AccessLink.WebAPI.Entities
{
    using System.Data.Entity;

    public partial class AccessLinkDBContext : AuditContext
    {
        public AccessLinkDBContext()
            : base("name=AccessLink")
        {
        }

        public virtual DbSet<AccessLink> AccessLinks { get; set; }
        public virtual DbSet<AccessLinkStatus> AccessLinkStatus { get; set; }
        public virtual DbSet<DeliveryPoint> DeliveryPoints { get; set; }
        public virtual DbSet<NetworkLink> NetworkLinks { get; set; }
        public virtual DbSet<OSRoadLink> OSRoadLinks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessLink>()
                .Property(e => e.WorkloadLengthMeter)
                .HasPrecision(18, 8);

            modelBuilder.Entity<AccessLink>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<AccessLink>()
                .HasMany(e => e.AccessLinkStatus)
                .WithRequired(e => e.AccessLink)
                .HasForeignKey(e => e.NetworkLinkID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DeliveryPoint>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

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
                .HasOptional(e => e.AccessLink)
                .WithRequired(e => e.NetworkLink);

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
        }
    }
}