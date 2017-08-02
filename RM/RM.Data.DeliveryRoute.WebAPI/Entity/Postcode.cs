using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    [Table("FMO.Postcode")]
    public partial class Postcode
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(8)]
        public string PostcodeUnit { get; set; }

        [Required]
        [StringLength(4)]
        public string OutwardCode { get; set; }

        [Required]
        [StringLength(3)]
        public string InwardCode { get; set; }

        public Guid? PrimaryRouteGUID { get; set; }

        public Guid? SecondaryRouteGUID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual Route Route { get; set; }

        public virtual Route Route1 { get; set; }
    }
}