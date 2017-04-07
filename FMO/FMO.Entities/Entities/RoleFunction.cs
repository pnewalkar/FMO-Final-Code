namespace Fmo.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.RoleFunction")]
    public partial class RoleFunction
    {
        public Guid ID { get; set; }

        public Guid RoleID { get; set; }

        public Guid FunctionID { get; set; }

        public virtual Function Function { get; set; }

        public virtual Role Role { get; set; }
    }
}