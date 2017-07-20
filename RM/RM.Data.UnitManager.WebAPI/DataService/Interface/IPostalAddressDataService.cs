using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using RM.Data.UnitManager.WebAPI.DataDTO;
using RM.DataManagement.UnitManager.WebAPI.DataDTO;

namespace RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces
{
    /// <summary>
    /// Interface to perfrom CRUD operations on postcode
    /// </summary>
    public interface IPostalAddressDataService
    {
        /// <summary>
        /// Gets first five postcodeunits for an unit for a given search text
        /// </summary>
        /// <param name="searchInputs">SearchInputDataDto</param>
        /// <returns>collection of PostcodeDataDTO</returns>
        Task<IEnumerable<PostcodeDataDTO>> GetPostcodeUnitForBasicSearch(SearchInputDataDto searchInputs);

        Task<List<string>> GetPostalAddressSearchDetails(string searchText, Guid unitGuid, List<Guid> addresstypeIDs);

        Task<List<PostalAddressDataDTO>> GetPostalAddressDetails(string selectedItem, Guid unitGuid);


    }
}