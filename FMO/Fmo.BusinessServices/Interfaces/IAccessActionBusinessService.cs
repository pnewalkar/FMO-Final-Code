namespace Fmo.BusinessServices.Interfaces
{
    using System.Collections.Generic;
    using Fmo.DTO;

    /// <summary>
    /// This interface contains declaration of methods for fetching Access actions.
    /// </summary>
    public interface IAccessActionBusinessService
    {
        /// <summary>
        /// This method is used to AccessActions List
        /// </summary>
        /// <returns>Access Actions List</returns>
        List<AccessActionDTO> FetchAccessActions();
    }
}