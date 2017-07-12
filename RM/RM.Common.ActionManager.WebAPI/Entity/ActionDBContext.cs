namespace RM.Common.ActionManager.WebAPI.Entity
{
    using System.Data.Entity;

    public partial class ActionDBContext : DbContext
    {
        public ActionDBContext()
            : base("name=ActionDBContext")
        {
        }

        public virtual DbSet<Action> Actions { get; set; }

        public virtual DbSet<Function> Functions { get; set; }

        public virtual DbSet<LocationReferenceData> LocationReferenceDatas { get; set; }

        public virtual DbSet<PostalAddressIdentifier> PostalAddressIdentifiers { get; set; }

        public virtual DbSet<ReferenceData> ReferenceDatas { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<RoleFunction> RoleFunctions { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserRoleLocation> UserRoleLocations { get; set; }

        public virtual DbSet<AccessFunction> AccessFunctions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Function>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Function>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Function>()
                .HasMany(e => e.RoleFunctions)
                .WithRequired(e => e.Function)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PostalAddressIdentifier>()
                .Property(e => e.ExternalID)
                .IsUnicode(false);

            modelBuilder.Entity<PostalAddressIdentifier>()
                .Property(e => e.Name)
                .IsUnicode(false);

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
                .HasMany(e => e.LocationReferenceDatas)
                .WithRequired(e => e.ReferenceData)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.PostalAddressIdentifiers)
                .WithOptional(e => e.ReferenceData)
                .HasForeignKey(e => e.IdentifierTypeGUID);

            modelBuilder.Entity<ReferenceData>()
                .HasMany(e => e.ReferenceData1)
                .WithOptional(e => e.ReferenceData2)
                .HasForeignKey(e => e.ParentReferenceDataID);

            modelBuilder.Entity<Role>()
                .Property(e => e.RoleName)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.RoleFunctions)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.UserRoleLocations)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

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
                .HasMany(e => e.UserRoleLocations)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AccessFunction>()
                .Property(e => e.RoleName)
                .IsUnicode(false);

            modelBuilder.Entity<AccessFunction>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<AccessFunction>()
                .Property(e => e.FunctionName)
                .IsUnicode(false);

            modelBuilder.Entity<AccessFunction>()
                .Property(e => e.ActionName)
                .IsUnicode(false);
        }
    }
}