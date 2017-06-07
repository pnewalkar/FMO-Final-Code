namespace RM.CommonLibrary.EntityFramework.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.Vw_GetAccessFunction")]
    public partial class AccessFunction
    {
        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string FunctionName { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string ActionName { get; set; }

        [Column(Order = 2)]
        [StringLength(50)]
        public string UserName { get; set; }

        [Column(Order = 0)]
        [StringLength(50)]
        public string RoleName { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid Unit_GUID { get; set; }

        [Key]
        [Column(Order = 5)]
        public Guid UserId { get; set; }
    }
}
