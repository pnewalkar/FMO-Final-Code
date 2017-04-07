using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IStreetNetworkRepository
    {
        Task<List<StreetNameDTO>> FetchStreetNamesForBasicSearch(string searchText);

        Task<List<StreetNameDTO>> FetchStreetNamesForAdvanceSearch(string searchText);

        Task<int> GetStreetNameCount(string searchText);
    }
}