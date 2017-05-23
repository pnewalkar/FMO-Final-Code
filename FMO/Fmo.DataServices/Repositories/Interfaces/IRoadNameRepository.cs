using System;
using System.Collections.Generic;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    /// <summary>
    /// This interface contains declaration of methods for Road Links data.
    /// </summary>
    public interface IRoadNameRepository
    {
        /// <summary>
        /// This method is used to fetch Road Link data as per boundingBox.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>Network Link Dto</returns>
        List<NetworkLinkDTO> GetRoadRoutes(string boundingBoxCoordinates, Guid unitGuid);
    }
}