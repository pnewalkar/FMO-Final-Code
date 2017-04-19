namespace Fmo.BusinessServices.Interfaces
{
    using System.Collections.Generic;
    using Fmo.DTO;

    /// <summary>
    /// This interface contains methods for fetching Access Links data
    /// </summary>
    public interface IAccessLinkBusinessService
    {
        /// <summary>
        /// This method is used to fetch Access Link data.
        /// </summary>
        /// <returns>List of Access Link Dto</returns>
        List<AccessLinkDTO> SearchAccessLink();

        /// <summary>
        /// This method is used to fetch data for Access Links.
        /// </summary>
        /// <param name="boundaryBox">boundaryBox as string</param>
        /// <returns>Access Links as string</returns>
        string GetAccessLinks(string boundaryBox);
    }
}
