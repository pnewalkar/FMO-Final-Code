namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.UserRoleUnit")]
    public partial class UserRoleUnit
    {
        public Guid ID { get; set; }

        public Guid User_GUID { get; set; }

        public Guid Role_GUID { get; set; }

        public Guid Unit_GUID { get; set; }

        public virtual Role Role { get; set; }

        public virtual UnitLocation UnitLocation { get; set; }

        public virtual User User { get; set; }
    }
}
