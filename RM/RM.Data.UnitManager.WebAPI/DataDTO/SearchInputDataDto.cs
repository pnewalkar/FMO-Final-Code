using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.DataManagement.UnitManager.WebAPI.DataDTO
{
    /// <summary>
    /// Input dto for search methods.
    /// </summary>
    public class SearchInputDataDto
    {
        public string SearchText { get; set; }
        public Guid UserUnitLocationId { get; set; }
        public Guid PostcodeTypeGUID { get; set; }
        public int SearchResultCount { get; set; }
    }
}
