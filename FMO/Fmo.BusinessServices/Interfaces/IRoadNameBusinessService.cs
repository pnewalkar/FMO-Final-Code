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
        Task<List<RoadNameDTO>> FetchRoadName();

        string GetRoadRoutes(string boundaryBox);
    }
}
