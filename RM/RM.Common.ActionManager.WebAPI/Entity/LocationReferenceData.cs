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
        public Guid ID { get; private set; }

        public Guid LocationID { get; private set; }

        public Guid ReferenceDataID { get; private set; }

        public DateTime RowCreateDateTime { get; private set; }

        public virtual ReferenceData ReferenceData { get; private set; }
    }
}
