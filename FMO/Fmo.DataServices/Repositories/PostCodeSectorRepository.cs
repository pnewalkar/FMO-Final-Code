using System;
using System.Linq;
using Fmo.Common.Constants;
using Fmo.Common.ExceptionManagement;
using Fmo.Common.ResourceFile;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;
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
                PostCodeSectorDTO postCodeSectorDto = default(PostCodeSectorDTO);
                var singleOrDefault = DataContext.PostalAddresses.AsNoTracking().Where(postalAddress => postalAddress.UDPRN == uDPRN).SingleOrDefault();
                if (singleOrDefault != null)
                {
                    PostcodeSector postCodeSector = singleOrDefault.Postcode1.PostcodeSector;
                    postCodeSectorDto = new PostCodeSectorDTO();
                    GenericMapper.Map(postCodeSector, postCodeSectorDto);
                }

                return postCodeSectorDto;
            }
            catch (InvalidOperationException ex)
            {
                throw new SystemException(ErrorMessageIds.Err_InvalidOperationExceptionForSingleorDefault, ex);
            }
        }
    }
}