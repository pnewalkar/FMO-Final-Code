using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IPostalAddressRepository
    {
        Task<List<PostalAddressDTO>> FetchPostalAddressforBasicSearch(string searchText);

        Task<List<PostalAddressDTO>> FetchPostalAddressforAdvanceSearch(string searchText);

        Task<int> GetPostalAddressCount(string searchText);
    }
}