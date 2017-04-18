using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
  public interface IPostCodeBussinessService
    {
        Task<List<PostCodeDTO>> FetchPostCodeUnit(string searchText);
    }
}
