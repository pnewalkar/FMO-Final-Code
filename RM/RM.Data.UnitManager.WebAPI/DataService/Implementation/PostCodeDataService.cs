﻿using System;
using System.Collections.Generic;
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
    /// DataService to interact with postal address entity and handle CRUD operations.
    /// </summary>
    public class PostcodeDataService : DataServiceBase<Postcode, UnitManagerDbContext>, IPostcodeDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public PostcodeDataService(IDatabaseFactory<UnitManagerDbContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            // Store injected dependencies
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Gets first five postcodeunits for an unit for a given search text
        /// </summary>
        /// <param name="searchInputs">SearchInputDataDto</param>
        /// <returns>collection of PostcodeDataDTO</returns>
        public async Task<IEnumerable<PostcodeDataDTO>> GetPostcodeUnitForBasicSearch(SearchInputDataDto searchInputs)
        {
            string methodName = typeof(UnitLocationDataService) + "." + nameof(GetPostcodeUnitForBasicSearch);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostcodeUnitForBasicSearch"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodEntryEventId);

                var postCodeDataDto = await (from p in DataContext.PostcodeHierarchies.AsNoTracking()
                                             join s in DataContext.LocationPostcodeHierarchies.AsNoTracking() on p.ID equals s.PostcodeHierarchyID
                                             join l in DataContext.Locations.AsNoTracking() on s.LocationID equals l.ID
                                             where p.Postcode.StartsWith(searchInputs.SearchText ?? string.Empty) && p.PostcodeTypeGUID == searchInputs.PostcodeTypeGUID
                                              && l.ID == searchInputs.UserUnitLocationId
                                             select new PostcodeDataDTO
                                             {
                                                 PostcodeUnit = p.Postcode,
                                                 ID = p.ID
                                             }).Take(searchInputs.SearchResultCount).ToListAsync();

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodExitEventId);
                return postCodeDataDto;
            }
        }

        /// <summary>
        /// Gets count of postcodeunits for an unit for a given search text
        /// </summary>
        /// <param name="searchInputs">SearchInputDataDto</param>
        /// <returns>count of PostcodeUnit</returns>
        public async Task<int> GetPostcodeUnitCount(SearchInputDataDto searchInputs)
        {
            string methodName = typeof(UnitLocationDataService) + "." + nameof(GetPostcodeUnitCount);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostcodeUnitCount"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodEntryEventId);

                var postCodeDataDto = await (from p in DataContext.PostcodeHierarchies.AsNoTracking()
                                             join s in DataContext.LocationPostcodeHierarchies.AsNoTracking() on p.ID equals s.PostcodeHierarchyID
                                             join l in DataContext.Locations.AsNoTracking() on s.LocationID equals l.ID
                                             where p.Postcode.StartsWith(searchInputs.SearchText ?? string.Empty) && p.PostcodeTypeGUID == searchInputs.PostcodeTypeGUID
                                              && l.ID == searchInputs.UserUnitLocationId
                                             select new PostcodeDataDTO
                                             {
                                                 PostcodeUnit = p.Postcode,
                                                 ID = p.ID
                                             }).CountAsync();

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodExitEventId);
                return postCodeDataDto;
            }
        }

        /// <summary>
        /// Gets all postcodeunits for an unit for a given search text
        /// </summary>
        /// <param name="searchInputs">SearchInputDataDto</param>
        /// <returns>collection of PostcodeDataDTO</returns>
        public async Task<IEnumerable<PostcodeDataDTO>> GetPostcodeUnitForAdvanceSearch(SearchInputDataDto searchInputs)
        {
            string methodName = typeof(UnitLocationDataService) + "." + nameof(GetPostcodeUnitForAdvanceSearch);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostcodeUnitForAdvanceSearch"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodEntryEventId);

                var postCodeDataDto = await (from p in DataContext.PostcodeHierarchies.AsNoTracking()
                                             join s in DataContext.LocationPostcodeHierarchies.AsNoTracking() on p.ID equals s.PostcodeHierarchyID
                                             join l in DataContext.Locations.AsNoTracking() on s.LocationID equals l.ID
                                             where p.Postcode.StartsWith(searchInputs.SearchText ?? string.Empty) && p.PostcodeTypeGUID == searchInputs.PostcodeTypeGUID
                                              && l.ID == searchInputs.UserUnitLocationId
                                             select new PostcodeDataDTO
                                             {
                                                 PostcodeUnit = p.Postcode,
                                                 ID = p.ID
                                             }).ToListAsync();

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodExitEventId);
                return postCodeDataDto;
            }
        }

        /// <summary>
        /// Get post code ID by passing postcode.
        /// </summary>
        /// <param name="postCode"> Post Code</param>
        /// <returns>PostcodeDataDTO</returns>
        public async Task<PostcodeDataDTO> GetPostcodeID(string postCode)
        {
            string methodName = typeof(UnitLocationDataService) + "." + nameof(GetPostcodeID);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostcodeID"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodEntryEventId);

                var postCodeDetail = await DataContext.Postcodes.Where(l => l.PostcodeUnit.Trim().Equals(postCode, StringComparison.OrdinalIgnoreCase))
                    .Select(l => new PostcodeDataDTO
                    {
                        ID = l.ID
                    }).SingleOrDefaultAsync();

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodExitEventId);
                return postCodeDetail;
            }
        }
    }
}