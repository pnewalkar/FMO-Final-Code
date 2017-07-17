using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;

namespace RM.DataManagement.UnitManager.WebAPI.DataService
{
    public class PostCodeSectorDataService : DataServiceBase<PostcodeSector, RMDBContext>//, IPostCodeSectorDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public PostCodeSectorDataService(IDatabaseFactory<RMDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Get the postcode sector by the UDPRN id
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>PostCodeSectorDTO object</returns>
        public async Task<PostCodeSectorDTO> GetPostCodeSectorByUDPRN(int uDPRN)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostCodeSectorByUDPRN"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostCodePriority, LoggerTraceConstants.PostCodeSectorDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                PostalAddress postalAddress = await DataContext.PostalAddresses.AsNoTracking().Where(pa => pa.UDPRN == uDPRN).SingleOrDefaultAsync();
                PostcodeSector postCodeSector = postalAddress.Postcode1.PostcodeSector;
                PostCodeSectorDTO postCodeSectorDTO = new PostCodeSectorDTO();
                GenericMapper.Map(postCodeSector, postCodeSectorDTO);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostCodePriority, LoggerTraceConstants.PostCodeSectorDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return postCodeSectorDTO;
            }
        }
    }
}