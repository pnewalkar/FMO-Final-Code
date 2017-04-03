using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Entity = Fmo.Entities;
using Fmo.DTO;
using Fmo.MappingConfiguration;

namespace Fmo.BusinessServices.Services
{
  public class PostCodeBussinessService : IPostCodeBussinessService
    {
        private IPostCodeRepository postCodeRepository = default(IPostCodeRepository);

        public async Task<List<PostCodeDTO>> FetchPostCodeUnit(string searchText)
        {
            return await postCodeRepository.FetchPostCodeUnit(searchText);
        }
    }
}
