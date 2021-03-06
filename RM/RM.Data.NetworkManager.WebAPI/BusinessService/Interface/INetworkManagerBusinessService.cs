﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using RM.DataManagement.NetworkManager.WebAPI.DTO;

namespace RM.DataManagement.NetworkManager.WebAPI.BusinessService
{
    public interface INetworkManagerBusinessService
    {
        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <param name="streetName">Street name.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        Tuple<NetworkLinkDTO, SqlGeometry> GetNearestNamedRoad(DbGeometry operationalObjectPoint, string streetName);

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        List<Tuple<NetworkLinkDTO, SqlGeometry>> GetNearestSegment(DbGeometry operationalObjectPoint);

        /// <summary>
        /// Get the street DTO for operational object.
        /// </summary>
        /// <param name="networkLinkID">networkLink unique identifier Guid.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        NetworkLinkDTO GetNetworkLink(Guid networkLinkID);

        /// <summary> This method is used to get the road links crossing the access link </summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="accessLinkCoordinates">access link coordinate array</param> <returns>List<NetworkLinkDTO></returns>
        List<NetworkLinkDTO> GetCrossingNetworkLinks(string boundingBoxCoordinates, DbGeometry accessLinkCoordinates);

        /// <summary>
        /// This method is used to fetch data for OSRoadLink.
        /// </summary>
        /// <param name="toid">toid unique identifier for OSRoadLink</param>
        /// <returns>returns Route Hierarchy as string</returns>
        Task<string> GetOSRoadLink(string toid);

        /// <summary>
        /// Gets the road routes.
        /// </summary>
        /// <param name="boundarybox">The boundarybox.</param>
        /// <param name="uniGuid">The uni unique identifier.</param>
        /// <param name="currentUserUnitType">Current user unit type.</param>
        /// <returns></returns>
        string GetRoadRoutes(string boundarybox, Guid uniGuid, string currentUserUnitType);

        /// <summary>
        /// Fetches the street names for basic search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns></returns>
        Task<List<StreetNameDTO>> GetStreetNamesForBasicSearch(string searchText, Guid userUnit, string currentUserUnitType);

        /// <summary>
        /// Gets the street name count.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns></returns>
        Task<int> GetStreetNameCount(string searchText, Guid userUnit, string currentUserUnitType);

        /// <summary>
        /// Fetches the street names for advance search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns></returns>
        Task<List<StreetNameDTO>> GetStreetNamesForAdvanceSearch(string searchText, Guid unitGuid, string currentUserUnitType);
    }
}