using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.DeliveryRoute.WebAPI.Entities;
using RM.DataManagement.DeliveryRoute.WebAPI.DataDTO;

namespace RM.DataManagement.DeliveryRoute.WebAPI.DataService
{
    public class PostCodeDataService : DataServiceBase<Postcode, RouteDBContext>, IPostCodeDataService
    {
        private int priority = LoggerTraceConstants.DeliveryRouteAPIPriority;
        private int entryEventId = LoggerTraceConstants.PostCodeDataServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.PostCodeDataServiceMethodExitEventId;
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockSequenceDataService"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public PostCodeDataService(IDatabaseFactory<RouteDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Get postcode details by passing postcode
        /// </summary>
        /// <param name="postCodeUnit">Postcode</param>
        /// <returns></returns>
        public async Task<PostCodeDataDTO> GetPostCode(string postCodeUnit)
        {
            string methodName = typeof(DeliveryRouteDataService) + "." + nameof(GetPostCode);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostCode"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var postCode = await (from postcode in DataContext.Postcodes.AsNoTracking()
                                      where postcode.PostcodeUnit == postCodeUnit
                                      select new PostCodeDataDTO
                                      {
                                          PostcodeUnit = postcode.PostcodeUnit,
                                          PrimaryRouteGUID = postcode.PrimaryRouteGUID,
                                          SecondaryRouteGUID = postcode.SecondaryRouteGUID
                                      }).SingleOrDefaultAsync();

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return postCode;
            }
        }
    }
}