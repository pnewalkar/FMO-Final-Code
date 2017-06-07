using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.CommonLibrary.EntityFramework.DTO.ReferenceData
{
    public class NameValuePair
    {
        // ReferenceData.ID
        public Guid Id { get; set; }

        // ReferenceDataCategpry.CategoryName
        public string Group { get; set; }

        // ReferenceData.ReferenceDataName
        public string Name { get; set; }

        // ReferenceData.ReferenceDataValue
        public string Value { get; set; }

        // ReferenceData.DisplayText
        public string DisplayText { get; set; }

        // ReferenceData.DataDescription
        public string Description { get; set; }

        // ReferenceDataCategory.Maintainable
        public bool maintainable { get; set; }
    }
}
