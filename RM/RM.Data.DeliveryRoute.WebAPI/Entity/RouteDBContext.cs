using System.Data.Entity;

namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    public partial class RouteDBContext : DbContext
    {
        public RouteDBContext()
            : base("name=RouteDBContext")
        {
        }

        public virtual DbSet<Block> Blocks { get; set; }

        public virtual DbSet<BlockSequence> BlockSequences { get; set; }

        public virtual DbSet<DeliveryPoint> DeliveryPoints { get; set; }

        public virtual DbSet<Location> Locations { get; set; }

        public virtual DbSet<NetworkNode> NetworkNodes { get; set; }

        public virtual DbSet<PostalAddress> PostalAddresses { get; set; }

        public virtual DbSet<Postcode> Postcodes { get; set; }

        public virtual DbSet<Route> Routes { get; set; }

        public virtual DbSet<RouteActivity> RouteActivities { get; set; }

        public virtual DbSet<RouteNetworkLink> RouteNetworkLinks { get; set; }

        public virtual DbSet<RouteStatus> RouteStatus { get; set; }

        public virtual DbSet<Scenario> Scenarios { get; set; }

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
                .HasOptional(e => e.NetworkNode)
                .WithRequired(e => e.Location);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.Scenarios)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NetworkNode>()
                .Property(e => e.TOID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NetworkNode>()
                .HasOptional(e => e.DeliveryPoint)
                .WithRequired(e => e.NetworkNode);

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
                .HasMany(e => e.DeliveryPoints)
                .WithRequired(e => e.PostalAddress)
                .WillCascadeOnDelete(false);

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
                .HasMany(e => e.Postcodes)
                .WithOptional(e => e.Route)
                .HasForeignKey(e => e.PrimaryRouteGUID);

            modelBuilder.Entity<Route>()
                .HasMany(e => e.Postcodes1)
                .WithOptional(e => e.Route1)
                .HasForeignKey(e => e.SecondaryRouteGUID);

            modelBuilder.Entity<Route>()
                .HasMany(e => e.Route1)
                .WithOptional(e => e.Route2)
                .HasForeignKey(e => e.PairedRouteID);

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
                .HasPrecision(16, 8);

            modelBuilder.Entity<RouteNetworkLink>()
                .Property(e => e.OrderIndex)
                .HasPrecision(8, 8);

            modelBuilder.Entity<Scenario>()
                .Property(e => e.ScenarioName)
                .IsUnicode(false);

            modelBuilder.Entity<Scenario>()
                .HasMany(e => e.ScenarioRoutes)
                .WithRequired(e => e.Scenario)
                .WillCascadeOnDelete(false);
        }
    }
}