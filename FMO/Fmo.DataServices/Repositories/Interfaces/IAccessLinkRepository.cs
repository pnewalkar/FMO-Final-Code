namespace Fmo.DataServices.Repositories.Interfaces
{
    using System.Collections.Generic;
    using Fmo.DTO;
    using Fmo.Entities;

    /// <summary>
    /// This class contains methods used to fetch access Link data.
    /// </summary>
    public interface IAccessLinkRepository
    {
        /// <summary>
        /// This method is used to fetch Access Link data.
        /// </summary>
        /// <returns>List of Access Link Dto</returns>
        List<AccessLinkDTO> SearchAccessLink();

        /// <summary>
        /// This method is used to fetch access Link data.
        /// </summary>
        /// <param name="coordinates">coordinates as string</param>
        /// <returns>List of Access Link dto</returns>
        List<AccessLinkDTO> GetAccessLinks(string coordinates);

        /// <summary>
        /// This method is used to fetch Access Link data as per coordinates.
        /// </summary>
        /// <param name="coordinates">coordinates as string</param>
        /// <returns>IEnumerable Access Link </returns>
        IEnumerable<AccessLink> GetData(string coordinates);
    }
}
