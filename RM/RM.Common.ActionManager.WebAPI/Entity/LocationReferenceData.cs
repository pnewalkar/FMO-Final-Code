namespace RM.Common.ActionManager.WebAPI.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.LocationReferenceData")]
    public partial class LocationReferenceData
    {
        public Guid ID { get; set; }

        public Guid LocationID { get; set; }

        public Guid ReferenceDataID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }
    }
}
