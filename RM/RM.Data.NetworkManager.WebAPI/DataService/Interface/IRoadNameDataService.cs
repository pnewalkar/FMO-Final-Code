using RM.CommonLibrary.EntityFramework.DTO;
using System;
using System.Collections.Generic;

namespace RM.DataManagement.NetworkManager.WebAPI.DataService.Interfaces
{
    /// <summary>
    /// This interface contains declaration of methods for Road Links data.
    /// </summary>
    public interface IRoadNameDataService
    {
        /// <summary>
        /// This method is used to fetch Road Link data as per boundingBox.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>Network Link Dto</returns>
        List<NetworkLinkDTO> GetRoadRoutes(string boundingBoxCoordinates, Guid locationID, List<ReferenceDataCategoryDTO> referenceDataCategoryList);
    }
}