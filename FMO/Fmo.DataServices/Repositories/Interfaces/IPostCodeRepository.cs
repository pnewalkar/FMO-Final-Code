using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IPostCodeRepository
    {
        Task<List<PostCodeDTO>> FetchPostCodeUnitforBasicSearch(string searchText);

        Task<int> GetPostCodeUnitCount(string searchText);

        Task<List<PostCodeDTO>> FetchPostCodeUnitForAdvanceSearch(string searchText);
    }
}