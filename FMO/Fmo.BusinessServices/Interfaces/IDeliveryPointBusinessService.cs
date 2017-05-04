using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
    /// <summary>
    /// This interface contains declaration of methods for fetching Delivery Points data.
    /// </summary>
    public interface IDeliveryPointBusinessService
    {
        /// <summary>
        /// This method is used to fetch delivery points data on the basis of coordinates.
        /// </summary>
        /// <param name="boundaryBox">Boundarybox as string</param>
        /// <param name="unitGuid">Unit unique identifier.</param>
        /// <returns>delivery points data as object</returns>
        object GetDeliveryPoints(string boundaryBox, Guid unitGuid);

        /// <summary>
        /// This method is used to fetch delivery points data on the basis of udprn.
        /// </summary>
        /// <param name="udprn">udprn as string</param>
        /// <returns>delivery points data as object</returns>
        object GetDeliveryPointByUDPRN(int udprn);

        /// <summary>
        /// This method is used to fetch address location data for positioning of Delivery Points on the basis of udprn.
        /// </summary>
        /// <param name="udprn">udprn as string</param>
        /// <returns>delivery points data as object</returns>
        AddressLocationDTO GetAddressLocationByUDPRN(int udprn);

        /// <summary>
        /// Fetch the Delivery point for Basic Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>
        /// Task
        /// </returns>
        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForBasicSearch(string searchText, Guid userUnit);

        /// <summary>
        /// Get the count of delivery point
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of delivery point</returns>
        Task<int> GetDeliveryPointsCount(string searchText, Guid userUnit);

        /// <summary>
        /// This method is used to fetch delivery points for advance search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// Task List of Delivery Point Dto
        /// </returns>
        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText, Guid unitGuid);
    }
}