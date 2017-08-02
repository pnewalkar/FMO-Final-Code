namespace RM.Common.ActionManager.WebAPI.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.RoleFunction")]
    public partial class RoleFunction
    {
        public Guid ID { get; set; }

        public Guid RoleID { get; set; }

        public Guid FunctionID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual Function Function { get; set; }

        public virtual Role Role { get; set; }
    }
}
