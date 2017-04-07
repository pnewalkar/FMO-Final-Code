namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.Function")]
    public partial class Function
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Function()
        {
            RoleFunctions = new HashSet<RoleFunction>();
        }

        public Guid ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        public Guid ActionContextID { get; set; }

        public virtual Action Action { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleFunction> RoleFunctions { get; set; }
    }
}