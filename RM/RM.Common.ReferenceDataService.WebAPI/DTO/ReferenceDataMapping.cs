using System.Collections.Generic;
using System.Xml.Serialization;

namespace RM.Common.ReferenceData.WebAPI.DTO
{
    public class ReferenceDataMapping
    {
        public ReferenceDataMapping()
        {
            this.CategoryNames = new List<CategoryName>();
        }

        [XmlElement("CategoryName")]
        public List<CategoryName> CategoryNames { get; set; }
    }
}