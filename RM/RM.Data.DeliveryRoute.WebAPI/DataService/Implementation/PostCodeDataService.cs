using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.DeliveryRoute.WebAPI.Entities;
using RM.DataManagement.DeliveryRoute.WebAPI.DataDTO;

namespace RM.DataManagement.DeliveryRoute.WebAPI.DataService
{
    public class PostcodeDataService : DataServiceBase<Postcode, RouteDBContext>, IPostcodeDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockSequenceDataService"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public PostcodeDataService(IDatabaseFactory<RouteDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Get postcode details by passing postcode
        /// </summary>
        /// <param name="postCodeUnit">Postcode</param>
        /// <returns></returns>
        public async Task<PostcodeDataDTO> GetPostcode(string postcodeUnit)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostcode"))
            {
                string methodName = typeof(DeliveryRouteDataService) + "." + nameof(GetPostcode);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodEntryEventId);

                var postcode = await (from postcodeData in DataContext.Postcodes.AsNoTracking()
                                      where postcodeData.PostcodeUnit == postcodeUnit
                                      select new PostcodeDataDTO
                                      {
                                          PostcodeUnit = postcodeData.PostcodeUnit,
                                          PrimaryRouteGUID = postcodeData.PrimaryRouteGUID,
                                          SecondaryRouteGUID = postcodeData.SecondaryRouteGUID
                                      }).SingleOrDefaultAsync();

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodExitEventId);

                return postcode;
            }
        }
    }
}