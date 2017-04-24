using System;

namespace Fmo.DataServices.Repositories.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fmo.DTO;

    /// <summary>
    /// This interface contains declaration of methods for Road Links data.
    /// </summary>
    public interface IRoadNameRepository
    {
        ///// <summary>
        ///// This method is used to Road Link data.
        ///// </summary>
        ///// <returns>List of Road Link dto</returns>
        //Task<List<RoadNameDTO>> FetchRoadName();

        /// <summary>
        /// This method is used to fetch Road Link data as per boundingBox.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>OsRoad Link Dto</returns>
        List<OsRoadLinkDTO> GetRoadRoutes(string boundingBoxCoordinates, Guid unitGuid);
    }
}