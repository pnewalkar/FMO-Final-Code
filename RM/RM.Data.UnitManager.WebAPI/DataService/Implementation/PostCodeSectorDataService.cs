using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.UnitManager.WebAPI.DataDTO;
using RM.DataManagement.UnitManager.WebAPI.Entity;

namespace RM.DataManagement.UnitManager.WebAPI.DataService
{
    /// <summary>
    /// Data service to handle CRUD operations on PostcodeHierarchy and related entites
    /// </summary>
    public class PostCodeSectorDataService : DataServiceBase<PostcodeHierarchy, UnitManagerDbContext>//, IPostCodeSectorDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public PostCodeSectorDataService(IDatabaseFactory<UnitManagerDbContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Get the postcode sector by the UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <param name="postCodeSectorTypeGuid">postCodeSectorType Guid</param>
        /// <param name="postCodeDistrictTypeGuid">postCodeDistrictType Guid</param>
        /// <returns>PostCodeSectorDataDTO</returns>
        public async Task<PostCodeSectorDataDTO> GetPostCodeSectorByUdprn(int udprn, Guid postCodeSectorTypeGuid, Guid postCodeDistrictTypeGuid)
        {
            string methodName = typeof(UnitLocationDataService) + "." + nameof(GetPostCodeSectorByUdprn);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostCodes"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeSectorDataServiceMethodEntryEventId);

                var postcodeSector = await (from ph in DataContext.PostcodeHierarchies.AsNoTracking()
                                            join pa in DataContext.PostalAddresses on ph.Postcode equals pa.Postcode
                                            where ph.PostcodeTypeGUID == postCodeSectorTypeGuid && pa.UDPRN == udprn
                                            select ph.ParentPostcode).FirstOrDefaultAsync();

                var postcodeDistrict = await (from ph in DataContext.PostcodeHierarchies.AsNoTracking()
                                              join pa in DataContext.PostalAddresses on ph.Postcode equals pa.Postcode
                                              where ph.PostcodeTypeGUID == postCodeDistrictTypeGuid && pa.UDPRN == udprn
                                              select ph.ParentPostcode).FirstOrDefaultAsync();

                PostCodeSectorDataDTO PostCodeSectorDataDTO = new PostCodeSectorDataDTO
                {
                    District = postcodeDistrict,
                    Sector = postcodeSector
                };

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeSectorDataServiceMethodExitEventId);
                return PostCodeSectorDataDTO;
            }
        }
    }
}