using System.Data.Entity;

namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    public partial class RouteDBContext : DbContext
    {
        public RouteDBContext() : base("name=RouteDBContext")
        {
        }

        public virtual DbSet<Block> Blocks { get; set; }

        public virtual DbSet<BlockSequence> BlockSequences { get; set; }

        public virtual DbSet<DeliveryPoint> DeliveryPoints { get; set; }

        public virtual DbSet<Location> Locations { get; set; }

        public virtual DbSet<LocationOffering> LocationOfferings { get; set; }

        public virtual DbSet<NetworkNode> NetworkNodes { get; set; }

        public virtual DbSet<Offering> Offerings { get; set; }

        public virtual DbSet<Route> Routes { get; set; }

        public virtual DbSet<RouteActivity> RouteActivities { get; set; }

        public virtual DbSet<RouteNetworkLink> RouteNetworkLinks { get; set; }

        public virtual DbSet<RouteStatus> RouteStatus { get; set; }

        public virtual DbSet<ScenarioRoute> ScenarioRoutes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Block>()
                .Property(e => e.BlockSpanMinute)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Block>()
                .HasMany(e => e.Block1)
                .WithOptional(e => e.Block2)
                .HasForeignKey(e => e.PairedBlockID);

            modelBuilder.Entity<Block>()
                .HasMany(e => e.BlockSequences)
                .WithRequired(e => e.Block)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BlockSequence>()
                .Property(e => e.OrderIndex)
                .HasPrecision(16, 8);

            modelBuilder.Entity<DeliveryPoint>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Location>()
                .HasMany(e => e.BlockSequences)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.LocationOfferings)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasOptional(e => e.NetworkNode)
                .WithRequired(e => e.Location);

            modelBuilder.Entity<NetworkNode>()
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NetworkNode>()
                .HasOptional(e => e.DeliveryPoint)
                .WithRequired(e => e.NetworkNode);

            modelBuilder.Entity<Offering>()
                .Property(e => e.OfferingDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Offering>()
                .HasMany(e => e.LocationOfferings)
                .WithRequired(e => e.Offering)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Route>()
                .Property(e => e.RouteName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Route>()
                .Property(e => e.RouteNumber)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Route>()
                .Property(e => e.SpanTimeMinute)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Route>()
                .Property(e => e.RouteBarcode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Route>()
                .HasMany(e => e.RouteActivities)
                .WithRequired(e => e.Route)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Route>()
                .HasMany(e => e.RouteStatus)
                .WithRequired(e => e.Route)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Route>()
                .HasMany(e => e.ScenarioRoutes)
                .WithRequired(e => e.Route)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RouteActivity>()
                .Property(e => e.RouteActivityOrderIndex)
                .HasPrecision(8, 8);

            modelBuilder.Entity<RouteNetworkLink>()
                .Property(e => e.OrderIndex)
                .HasPrecision(8, 8);
        }
    }
}