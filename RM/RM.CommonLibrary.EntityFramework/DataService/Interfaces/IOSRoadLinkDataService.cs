using System.Threading.Tasks;

namespace RM.CommonLibrary.EntityFramework.DataService.Interfaces
{
    /// <summary>
    /// This interface contains methods for fetching OSRoadLink data
    /// </summary>
    public interface IOSRoadLinkDataService
    {
        /// <summary>
        /// This method is used to fetch data for OSRoadLink.
        /// </summary>
        /// <param name="toid">toid unique identifier for OSRoadLink</param>
        /// <returns>returns Route Hierarchy as string</returns>
        Task<string> GetOSRoadLink(string toid);
    }
}