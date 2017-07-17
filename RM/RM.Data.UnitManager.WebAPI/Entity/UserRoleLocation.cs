namespace RM.DataManagement.UnitManager.WebAPI.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.UserRoleLocation")]
    public partial class UserRoleLocation
    {
        public Guid ID { get; private set; }

        public Guid UserID { get; private set; }

        public Guid RoleID { get; private set; }

        public Guid LocationID { get; private set; }

        public virtual Location Location { get; private set; }
    }
}
