using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.UnitManager.WebAPI.DataDTO;
using RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces;
using RM.DataManagement.UnitManager.WebAPI.Entity;

namespace RM.DataManagement.UnitManager.WebAPI.DataService
{
    /// <summary>
    /// Data service to handle CRUD operations on PostcodeHierarchy and related entites
    /// </summary>
    public class PostcodeSectorDataService : DataServiceBase<PostcodeHierarchy, UnitManagerDbContext>, IPostcodeSectorDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        /// <summary>
        /// Parameterised Constructor
        /// </summary>
        /// <param name="databaseFactory"></param>
        /// <param name="loggingHelper"></param>
        public PostcodeSectorDataService(IDatabaseFactory<UnitManagerDbContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            // Store injected dependencies
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Get the postcode sector by the UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>PostcodeSectorDataDTO</returns>
        public async Task<PostcodeSectorDataDTO> GetPostcodeSectorByUdprn(int udprn)
        {
            string methodName = typeof(UnitLocationDataService) + "." + nameof(GetPostcodeSectorByUdprn);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostcodeSectorByUdprn"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeSectorDataServiceMethodEntryEventId);

                var postcodeSector = await (from ph in DataContext.PostcodeHierarchies.AsNoTracking()
                                            join pc in DataContext.Postcodes.AsNoTracking() on ph.ID equals pc.ID
                                            join pa in DataContext.PostalAddresses on ph.Postcode equals pa.Postcode
                                            where pa.UDPRN == udprn
                                            select ph.ParentPostcode.Trim()).FirstOrDefaultAsync();

                var postcodeDistrict = await (from ph in DataContext.PostcodeHierarchies.AsNoTracking()
                                              join ph2 in DataContext.PostcodeHierarchies.AsNoTracking() on ph.ParentPostcode equals ph2.Postcode
                                              join pc in DataContext.Postcodes.AsNoTracking() on ph.ID equals pc.ID
                                              join pa in DataContext.PostalAddresses on ph.Postcode equals pa.Postcode
                                              where pa.UDPRN == udprn
                                              select ph2.ParentPostcode.Trim()).FirstOrDefaultAsync();

                PostcodeSectorDataDTO postCodeSectorDataDTO = new PostcodeSectorDataDTO
                {
                    District = postcodeDistrict,
                    Sector = postcodeSector
                };

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeSectorDataServiceMethodExitEventId);
                return postCodeSectorDataDTO;
            }
        }
    }
}