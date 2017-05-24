using System;
using System.Collections.Generic;
using Fmo.DTO;
using System.Data.Entity.Spatial;

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

        /// <summary>
        /// Creates access link.
        /// </summary>
        /// <param name="accessLinkDto">Access link data object.</param>
        /// <returns>Success.</returns>
        bool CreateAccessLink(AccessLinkDTO accessLinkDto);

        /// <summary>
        /// This method is used to get the access links crossing the created access link
        /// </summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param>
        /// <param name="accessLink">access link coordinate array</param>
        /// <returns>List<AccessLinkDTO> </returns>
        List<AccessLinkDTO> GetAccessLinksCrossingManualAccessLink(string boundingBoxCoordinates, DbGeometry accessLink);
    }
}