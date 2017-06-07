using System;
using System.Linq;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;

using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.DataMiddleware;
using System.Threading.Tasks;
using System.Data.Entity;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    public class PostCodeSectorDataService : DataServiceBase<PostcodeSector, RMDBContext>, IPostCodeSectorDataService
    {
        public PostCodeSectorDataService(IDatabaseFactory<RMDBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// Get the postcode sector by the UDPRN id
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>PostCodeSectorDTO object</returns>
        public async Task<PostCodeSectorDTO> GetPostCodeSectorByUDPRN(int uDPRN)
        {
            try
            {
                PostalAddress postalAddress = await DataContext.PostalAddresses.AsNoTracking().Where(pa => pa.UDPRN == uDPRN).SingleOrDefaultAsync();
                PostcodeSector postCodeSector = postalAddress.Postcode1.PostcodeSector;
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