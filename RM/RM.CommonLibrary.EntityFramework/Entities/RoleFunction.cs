namespace RM.CommonLibrary.EntityFramework.Entities
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

        public Guid Role_GUID { get; set; }

        public Guid Function_GUID { get; set; }

        public virtual Function Function { get; set; }

        public virtual Role Role { get; set; }
    }
}
