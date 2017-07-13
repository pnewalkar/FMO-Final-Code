using RM.CommonLibrary.EntityFramework.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using RM.DataManagement.AccessLink.WebAPI.DTO;
using RM.Data.AccessLink.WebAPI.DTO;

namespace RM.DataManagement.AccessLink.WebAPI.DataService.Interfaces
{
    /// <summary>
    /// This class contains methods used to fetch access Link data.
    /// </summary>
    public interface IAccessLinkDataService
    {
        /// <summary>
        /// This method is used to fetch access Link data.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>List of Access Link dto</returns>
        List<AccessLinkDataDTO> GetAccessLinks(string boundingBoxCoordinates, Guid unitGuid);

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
        List<AccessLinkDTO> GetAccessLinksCrossingOperationalObject(string boundingBoxCoordinates, DbGeometry accessLink);
    }
}