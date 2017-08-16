namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.LocationRelationship")]
    public partial class LocationRelationship
    {
        public Guid ID { get; set; }

        public Guid LocationID { get; set; }

        public Guid RelatedLocationID { get; set; }

        public Guid RelationshipTypeGUID { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual Location Location { get; set; }

        public virtual Location Location1 { get; set; }
    }
}