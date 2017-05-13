using System;

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
    }
}