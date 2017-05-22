using System;
using Fmo.DTO;
using Fmo.DTO.Model;

namespace Fmo.BusinessServices.Interfaces
{
    /// <summary>
    /// This interface contains methods for fetching Access Links data
    /// </summary>
    public interface IAccessLinkBusinessService
    {
        /// <summary>
        /// This method is used to fetch data for Access Links.
        /// </summary>
        /// <param name="boundaryBox">boundaryBox as string.</param>
        /// <param name="unitGuid">Unit unique identifier.</param>
        /// <returns>Access Links as string</returns>
        string GetAccessLinks(string boundaryBox, Guid unitGuid);

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
        /// <returns>If <true> then access link creation succeeded,else failure.</true></returns>
        bool CreateAccessLink(AccessLinkManualCreateModelDTO accessLinkDto);

        /// <summary>
        /// This method is used to calculate path length.
        /// </summary>
        /// <param name="accessLinkDto">access link object to be stored</param>
        /// <returns>returns calculated path length as <double>.</true></returns>
        decimal GetAdjPathLength(AccessLinkManualCreateModelDTO accessLinkDto);
    }
}