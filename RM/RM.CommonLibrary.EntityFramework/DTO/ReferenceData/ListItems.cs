using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.CommonLibrary.EntityFramework.DTO.ReferenceData
{
    public class ListItems
    {
        // ReferenceData.ID
        public Guid Id { get; set; }

        // ReferenceData.ReferenceDataName
        public string Name { get; set; }

        // ReferenceData.ReferenceDataValue
        public string Value { get; set; }

        // ReferenceData.DisplayText
        public string DisplayText { get; set; }

        // ReferenceData.DataDescription
        public string Description { get; set; }
    }
}
