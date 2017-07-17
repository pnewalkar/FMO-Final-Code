namespace RM.DataManagement.UnitManager.WebAPI.Entity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class UnitManagerDbContext : DbContext
    {
        public UnitManagerDbContext()
            : base("name=UnitManagerDbContext")
        {
        }

        //command
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LocationOffering> LocationOfferings { get; set; }
        public virtual DbSet<LocationPostcodeHierarchy> LocationPostcodeHierarchies { get; set; }
        public virtual DbSet<LocationReferenceData> LocationReferenceDatas { get; set; }
        public virtual DbSet<LocationRelationship> LocationRelationships { get; set; }
        public virtual DbSet<NetworkNode> NetworkNodes { get; set; }
        public virtual DbSet<PostalAddress> PostalAddresses { get; set; }
        public virtual DbSet<PostalAddressIdentifier> PostalAddressIdentifiers { get; set; }
        public virtual DbSet<PostcodeHierarchy> PostcodeHierarchies { get; set; }
        public virtual DbSet<Scenario> Scenarios { get; set; }
        public virtual DbSet<ScenarioDayOfTheWeek> ScenarioDayOfTheWeeks { get; set; }
        public virtual DbSet<ScenarioStatu> ScenarioStatus { get; set; }

        // Query
        public virtual DbSet<Postcode> Postcodes { get; set; }
        public virtual DbSet<UserRoleLocation> UserRoleLocations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>()
                .HasMany(e => e.LocationOfferings)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.LocationPostcodeHierarchies)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.LocationReferenceDatas)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.LocationRelationships)
                .WithRequired(e => e.Location)
                .HasForeignKey(e => e.LocationID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.LocationRelationships1)
                .WithRequired(e => e.Location1)
                .HasForeignKey(e => e.RelatedLocationID)
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

            modelBuilder.Entity<PostalAddressIdentifier>()
                .Property(e => e.ExternalID)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddressIdentifier>()
                .Property(e => e.Name)
                .IsUnicode(false);

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

            modelBuilder.Entity<PostcodeHierarchy>()
                .Property(e => e.Postcode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostcodeHierarchy>()
                .Property(e => e.ParentPostcode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PostcodeHierarchy>()
                .HasMany(e => e.LocationPostcodeHierarchies)
                .WithRequired(e => e.PostcodeHierarchy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PostcodeHierarchy>()
                .HasOptional(e => e.Postcode1)
                .WithRequired(e => e.PostcodeHierarchy);
        }
    }
}
