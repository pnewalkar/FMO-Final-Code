using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RM.Common.ReferenceData.WebAPI.DTO;

namespace RM.Common.ReferenceData.WebAPI.DTO
{
    /// <summary>
    /// This class represents data transfer object for Name Value JSON DTO
    /// </summary>
    public class ReferenceDataNameValueDTO
    {
        public Guid Id { get; set; }

        public string Group { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string DisplayText { get; set; }

        public string Description { get; set; }

        public bool Maintainable { get; set; }
    }
}
