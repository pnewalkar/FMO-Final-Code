namespace Fmo.DataServices.Repositories.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fmo.DTO;
    using Fmo.Entities;

    public interface IRoadNameRepository
    {
        Task<List<RoadNameDTO>> FetchRoadName();

        List<OsRoadLinkDTO> GetRoadRoutes(string coordinates);

        IEnumerable<OSRoadLink> GetData(string coordinates);

    }
}
