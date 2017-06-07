using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DTO.Model;

namespace RM.DataManagement.AccessLink.WebAPI.BusinessService.Interface
{
    public interface IAccessLinkBusinessService
    {
        /// <summary>
        /// Create automatic access link creation after delivery point creation.
        /// </summary>
        /// <param name="operationalObjectId">Operational Object unique identifier.</param>
        /// <param name="operationObjectTypeId">Operational Object type unique identifier.</param>
        /// <returns>bool</returns>
        bool CreateAccessLink(Guid operationalObjectId, Guid operationObjectTypeId);

        /// <summary>
        /// This method is used to create manual Access Link .
        /// </summary>
        /// <param name="accessLinkDto">access link object to be stored</param>
        /// <returns>If <true>then access link creation succeeded,else failure.</true></returns>
        bool CreateAccessLink(AccessLinkManualCreateModelDTO accessLinkDto);

        /// <summary>
        /// This method fetches data for AccsessLinks
        /// </summary>
        /// <param name="boundaryBox">boundaryBox as string</param>
        /// <param name="unitGuid">Unit unique identifier.</param>
        /// <returns>AccsessLink object</returns>
        string GetAccessLinks(string boundaryBox, Guid unitGuid);

        /// <summary> This method is used to calculate path length. </summary> <param
        /// name="accessLinkDto">access link object to be stored</param> <returns>returns calculated
        /// path length as <double>.</true></returns>
        decimal GetAdjPathLength(AccessLinkManualCreateModelDTO accessLinkDto);

        /// <summary>
        /// This method is used to check whether an access link is valid
        /// </summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param>
        /// <param name="accessLinkCoordinates">access link coordinate array</param>
        /// <returns>bool</returns>
        bool CheckManualAccessLinkIsValid(string boundingBoxCoordinates, string accessLinkCoordinates);
    }
}