using System;

namespace Fmo.DTO
{
    public class ReferenceDataDTO
    {
        public string ReferenceDataName { get; set; }

        public string ReferenceDataValue { get; set; }

        public string DataDescription { get; set; }

        public string DisplayText { get; set; }

        public int? DataParentId { get; set; }

        public Guid ID { get; set; }

        public Guid ReferenceDataCategory_GUID { get; set; }

        public Guid? DataParent_GUID { get; set; }
    }
}