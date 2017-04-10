using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;
using System;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IPostCodeRepository
    {
        Task<List<PostCodeDTO>> FetchPostCodeUnitForBasicSearch(string searchText);

        Task<int> GetPostCodeUnitCount(string searchText);

        Task<List<PostCodeDTO>> FetchPostCodeUnitForAdvanceSearch(string searchText);

        Guid GetPostCodeID(string postCode);
    }
}