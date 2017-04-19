namespace Fmo.DataServices.Repositories.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fmo.DTO;
    using Fmo.Entities;

    /// <summary>
    /// This interface contains declaration of methods for Road Links data.
    /// </summary>
    public interface IRoadNameRepository
    {
        /// <summary>
        /// This method is used to Road Link data.
        /// </summary>
        /// <returns>List of Road Link dto</returns>
        Task<List<RoadNameDTO>> FetchRoadName();

        /// <summary>
        /// This method is used to fetch Road Link data as per coordinates.
        /// </summary>
        /// <param name="coordinates">coordinates as string</param>
        /// <returns>OsRoad Link Dto</returns>
        List<OsRoadLinkDTO> GetRoadRoutes(string coordinates);

        /// <summary>
        /// This method is used to fetch Road Link data as per coordinates for Polygon creation
        /// </summary>
        /// <param name="coordinates">coordinates as string</param>
        /// <returns>IEnumerable OSRoadLink</returns>
        IEnumerable<OSRoadLink> GetData(string coordinates);

    }
}
