namespace RM.Common.ActionManager.WebAPI.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.Vw_GetAccessFunction")]
    public partial class AccessFunction
    {
        [StringLength(50)]
        public string RoleName { get; set; }

        [Key]
        [Column(Order = 0)]
        public Guid LocationID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string UserName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string FunctionName { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string ActionName { get; set; }

        [Key]
        [Column(Order = 4)]
        public Guid UserId { get; set; }
    }
}
