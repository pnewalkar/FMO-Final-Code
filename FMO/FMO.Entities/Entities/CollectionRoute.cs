namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.CollectionRoute")]
    public partial class CollectionRoute
    {
        public int CollectionRoute_Id { get; set; }

        [StringLength(30)]
        public string RouteName { get; set; }

        [StringLength(10)]
        public string RouteNumber { get; set; }

        public Guid ID { get; set; }
    }
}