namespace Fmo.BusinessServices.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fmo.DTO;

    /// <summary>
    /// This interface contains declaration of methods for fetching Road Links data.
    /// </summary>
    public interface IRoadNameBusinessService
    {
        /// <summary>
        /// This method is used to fetch Road Link data.
        /// </summary>
        /// <returns>List of Road Link Dto</returns>
        Task<List<RoadNameDTO>> FetchRoadName();

        /// <summary>
        /// This method is used to Fetch Road Links data as per coordinates
        /// </summary>
        /// <param name="boundaryBox">boundaryBox as string</param>
        /// <returns>string of Road Link data</returns>
        string GetRoadRoutes(string boundaryBox);
    }
}
