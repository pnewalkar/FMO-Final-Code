namespace Fmo.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.UserRoleUnit")]
    public partial class UserRoleUnit
    {
        public Guid ID { get; set; }

        public Guid UserID { get; set; }

        public Guid RoleID { get; set; }

        public Guid UnitID { get; set; }

        public virtual DeliveryUnitLocation DeliveryUnitLocation { get; set; }

        public virtual Role Role { get; set; }

        public virtual User User { get; set; }
    }
}