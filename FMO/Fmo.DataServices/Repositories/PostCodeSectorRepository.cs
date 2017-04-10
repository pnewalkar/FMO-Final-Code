using Fmo.DataServices.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DTO;
using Fmo.DataServices.DBContext;
using Fmo.Entities;
using Fmo.DataServices.Infrastructure;
using Fmo.MappingConfiguration;

namespace Fmo.DataServices.Repositories
{
    public class PostCodeSectorRepository : RepositoryBase<PostcodeSector, FMODBContext>, IPostCodeSectorRepository
    {
        public PostCodeSectorRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public PostCodeSectorDTO GetPostCodeSectorByUDPRN(int uDPRN)
        {
            try
            {
                PostcodeSector postCodeSector = DataContext.PostalAddresses.Where(postalAddress => postalAddress.UDPRN == uDPRN).SingleOrDefault().Postcode1.PostcodeSector;
                PostCodeSectorDTO postCodeSectorDTO = new PostCodeSectorDTO();
                GenericMapper.Map(postCodeSector, postCodeSectorDTO);
                return postCodeSectorDTO;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
