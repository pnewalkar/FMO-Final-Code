using System;
using System.Collections.Generic;

namespace RM.Common.ReferenceData.WebAPI.DTO
{
    /// <summary>
    /// This class represents data transfer object for ReferenceDataCategory entity
    /// </summary>
    public class ReferenceDataCategoryDTO
    {
        public Guid Id { get; set; }

        public string CategoryName { get; set; }

        public bool Maintainable { get; set; }

        public int CategoryType { get; set; }

        public virtual ICollection<ReferenceDataDTO> ReferenceDatas { get; set; }
    }
}