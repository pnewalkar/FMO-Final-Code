namespace RM.CommonLibrary.EntityFramework.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.CollectionRoute")]
    public partial class CollectionRoute
    {
        [StringLength(30)]
        public string RouteName { get; set; }

        [StringLength(10)]
        public string RouteNumber { get; set; }

        public Guid ID { get; set; }

        public Guid? CollectionScenario_GUID { get; set; }

        public virtual Scenario Scenario { get; set; }
    }
}