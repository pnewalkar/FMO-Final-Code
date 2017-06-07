using System;

namespace RM.CommonLibrary.EntityFramework.DTO
{
    public class BlockSequenceDTO
    {
        public decimal? OrderIndex { get; set; }

        public Guid ID { get; set; }

        public Guid Block_GUID { get; set; }

        public Guid? OperationalObjectType_GUID { get; set; }

        public Guid? DeliveryGroup_GUID { get; set; }

        public Guid? OperationalObject_GUID { get; set; }
    }
}