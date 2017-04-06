using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IStreetNetworkRepository
    {
        Task<List<StreetNameDTO>> FetchStreetNamesforBasicSearch(string searchText);

        Task<List<StreetNameDTO>> FetchStreetNamesforAdvanceSearch(string searchText);

        Task<int> GetStreetNameCount(string searchText);
    }
}