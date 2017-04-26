using System;
using System.Collections.Generic;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    /// <summary>
    /// This class contains methods used to fetch access Link data.
    /// </summary>
    public interface IAccessLinkRepository
    {
        /// <summary>
        /// This method is used to fetch access Link data.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>List of Access Link dto</returns>
        List<AccessLinkDTO> GetAccessLinks(string boundingBoxCoordinates, Guid unitGuid);
    }
}