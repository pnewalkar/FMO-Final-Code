namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Linq;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Entities;
    using Fmo.MappingConfiguration;

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
                PostcodeSector postCodeSector = DataContext.PostalAddresses.AsNoTracking().Where(postalAddress => postalAddress.UDPRN == uDPRN).SingleOrDefault().Postcode1.PostcodeSector;
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
