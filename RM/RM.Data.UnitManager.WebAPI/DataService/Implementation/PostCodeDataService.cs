using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.DataManagement.UnitManager.WebAPI.Entity;
using RM.DataManagement.UnitManager.WebAPI.DataDTO;
using RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces;

namespace RM.DataManagement.UnitManager.WebAPI.DataService
{
    /// <summary>
    /// DataService to interact with postal address entity and handle CRUD operations.
    /// </summary>
    public class PostCodeDataService : DataServiceBase<Postcode, UnitManagerDbContext>, IPostCodeDataService
    {
        private const int searchResultCount = 5;
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public PostCodeDataService(IDatabaseFactory<UnitManagerDbContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Gets first five postcodeunits for an unit for a given search text
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="unitlocationId"></param>
        /// <param name="postcodeTypeGUID"></param>
        /// <returns>list</returns>
        public async Task<List<PostCodeDataDTO>> GetPostCodeUnitForBasicSearch(string searchText, Guid unitlocationId, Guid postcodeTypeGUID)
        {
            string methodName = typeof(UnitLocationDataService) + "." + nameof(GetPostCodeUnitForBasicSearch);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostCodeUnitForBasicSearch"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodEntryEventId);

                searchText = searchText ?? string.Empty;
                var postCodeDataDto = await (from p in DataContext.PostcodeHierarchies.AsNoTracking()
                                                join s in DataContext.LocationPostcodeHierarchies.AsNoTracking() on p.ID equals s.PostcodeHierarchyID
                                                join l in DataContext.Locations.AsNoTracking() on s.LocationID equals l.ID
                                                where p.Postcode.StartsWith(searchText) && p.PostcodeTypeGUID == postcodeTypeGUID
                                                 && l.ID == unitlocationId
                                                select new PostCodeDataDTO
                                                {
                                                    PostcodeUnit = p.Postcode,
                                                    ID = p.ID
                                                }).Take(searchResultCount).ToListAsync();

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodExitEventId);
                return postCodeDataDto;
            }
        }

        /// <summary>
        /// Gets count of postcodeunits for an unit for a given search text
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="unitlocationId"></param>
        /// <param name="postcodeTypeGUID"></param>
        /// <returns>count of postcodeunits</returns>
        public async Task<int> GetPostCodeUnitCount(string searchText, Guid unitlocationId, Guid postcodeTypeGUID)
        {
            string methodName = typeof(UnitLocationDataService) + "." + nameof(GetPostCodeUnitCount);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostCodeUnitCount"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodEntryEventId);

                searchText = searchText ?? string.Empty;
                var postCodeDetailsDto = await (from p in DataContext.PostcodeHierarchies.AsNoTracking()
                                                join s in DataContext.LocationPostcodeHierarchies.AsNoTracking() on p.ID equals s.PostcodeHierarchyID
                                                join l in DataContext.Locations.AsNoTracking() on s.LocationID equals l.ID
                                                where p.Postcode.StartsWith(searchText) && p.PostcodeTypeGUID == postcodeTypeGUID //TODO : Call reference data service - Postcode- 9813B8BE-83FB-48D0-A8A2-703B2524002B
                                                 && l.ID == unitlocationId
                                                select new PostCodeDataDTO
                                                {
                                                    PostcodeUnit = p.Postcode,
                                                    ID = p.ID
                                                }).CountAsync();

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodExitEventId);
                return postCodeDetailsDto;
            }
        }

        /// <summary>
        /// Gets all postcodeunits for an unit for a given search text
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="unitlocationId"></param>
        /// <param name="postcodeTypeGUID"></param>
        /// <returns>list</returns>
        public async Task<List<PostCodeDataDTO>> GetPostCodeUnitForAdvanceSearch(string searchText, Guid unitlocationId, Guid postcodeTypeGUID)
        {
            string methodName = typeof(UnitLocationDataService) + "." + nameof(GetPostCodeUnitForAdvanceSearch);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostCodeUnitForAdvanceSearch"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodEntryEventId);

                searchText = searchText ?? string.Empty;
                var postCodeDataDto = await (from p in DataContext.PostcodeHierarchies.AsNoTracking()
                                             join s in DataContext.LocationPostcodeHierarchies.AsNoTracking() on p.ID equals s.PostcodeHierarchyID
                                             join l in DataContext.Locations.AsNoTracking() on s.LocationID equals l.ID
                                             where p.Postcode.StartsWith(searchText) && p.PostcodeTypeGUID == postcodeTypeGUID //TODO : Call reference data service - Postcode- 9813B8BE-83FB-48D0-A8A2-703B2524002B
                                              && l.ID == unitlocationId
                                             select new PostCodeDataDTO
                                             {
                                                 PostcodeUnit = p.Postcode,
                                                 ID = p.ID
                                             }).Take(searchResultCount).ToListAsync();

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodExitEventId);
                return postCodeDataDto;
            }
        }

        /// <summary>
        /// Get post code ID by passing post code.
        /// </summary>
        /// <param name="postCode"> Post Code</param>
        /// <returns>Post code ID</returns>
        public async Task<Guid> GetPostCodeID(string postCode)
        {
            string methodName = typeof(UnitLocationDataService) + "." + nameof(GetPostCodeID);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostCodeID"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodEntryEventId);

                var postCodeDetail = await DataContext.Postcodes.Where(l => l.PostcodeUnit.Trim().Equals(postCode, StringComparison.OrdinalIgnoreCase)).SingleOrDefaultAsync();

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodExitEventId);
                return postCodeDetail != null ? postCodeDetail.ID : Guid.Empty;
            }
        }
    }
}