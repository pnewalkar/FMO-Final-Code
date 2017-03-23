namespace FMO.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.NetworkLinkReference")]
    public partial class NetworkLinkReference
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NetworkReference_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NetworkLink_Id { get; set; }

        [StringLength(20)]
        public string OSRoadLinkTOID { get; set; }

        public virtual NetworkLink NetworkLink { get; set; }

        public virtual NetworkReference NetworkReference { get; set; }
    }
}
