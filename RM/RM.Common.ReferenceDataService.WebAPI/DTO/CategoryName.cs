using System.Collections.Generic;
using System.Xml.Serialization;

namespace RM.Common.ReferenceData.WebAPI.DTO
{
    public class CategoryName
    {
        public CategoryName()
        {
            this.ReferenceDataNames = new List<ReferenceDataName>();
        }

        public string AppCategoryName { get; set; }

        public string DBCategoryName { get; set; }

        [XmlElement("ReferenceDataName")]
        public List<ReferenceDataName> ReferenceDataNames { get; set; }
    }
}