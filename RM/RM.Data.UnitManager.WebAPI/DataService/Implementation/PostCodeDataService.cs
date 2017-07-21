using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
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

        /// <summary>
        /// Parameterised Constructor 
        /// </summary>
        /// <param name="databaseFactory"></param>
        /// <param name="loggingHelper"></param>
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

        /// <summary>
        /// Gets approx location based on the postal code.
        /// </summary>
        /// <param name="postcode">Postal code</param>
        /// <param name="unitId">Unique identifier for unit.</param>
        /// <returns>The approx location for the given postal code.</returns>
        public async Task<DbGeometry> GetApproxLocation(string postcode, Guid unitId)
        {
            DbGeometry approxLocation = null;
            string methodName = typeof(UnitLocationDataService) + "." + nameof(GetApproxLocation);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetApproxLocation"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodEntryEventId);

                // get approx loaction for that post code
                var approxLocationForPostCode = await DataContext.DeliveryPoints.FirstOrDefaultAsync(x => x.PostalAddress.Postcode == postcode);

                if (approxLocationForPostCode != null)
                {
                    approxLocation = approxLocationForPostCode.NetworkNode.Location.Shape;
                }
                else
                {
                    var postcodeSector = (from ph in DataContext.PostcodeHierarchies.AsNoTracking()
                                          where ph.Postcode == postcode
                                          select ph.ParentPostcode.Trim()).FirstOrDefault();

                    // get approx location for the pose code sector
                    var approxLocationForPostCodeSector = await DataContext.DeliveryPoints.FirstOrDefaultAsync(x => x.PostalAddress.Postcode.Replace(" ","").StartsWith(postcodeSector));

                    if (approxLocationForPostCodeSector != null)
                    {
                        approxLocation = approxLocationForPostCodeSector.NetworkNode.Location.Shape;
                    }
                    else
                    {
                        var postcodeDistrict = (from ph in DataContext.PostcodeHierarchies.AsNoTracking()
                                                where ph.Postcode == postcodeSector
                                                select ph.ParentPostcode.Trim()).FirstOrDefault();

                        // get approx location for the pose code sector
                        var approxLocationForPostCodeDistrict = DataContext.DeliveryPoints.FirstOrDefault(x => x.PostalAddress.Postcode.Replace(" ", "").StartsWith(postcodeDistrict));

                        if (approxLocationForPostCodeDistrict != null)
                        {
                            approxLocation = approxLocationForPostCodeDistrict.NetworkNode.Location.Shape;
                        }
                        else
                        {
                            // get approx location for that unit
                            var dpForUnit = DataContext.DeliveryPoints.FirstOrDefault(dp => DataContext.PostalAddressIdentifiers.Any(pa => pa.ID == unitId && pa.PostalAddressID == dp.PostalAddressID));

                            if (dpForUnit != null)
                            {
                                approxLocation = dpForUnit.NetworkNode.Location.Shape;
                            }
                        }
                    }
                }

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostCodeDataServiceMethodExitEventId);

                return approxLocation;
            }
        }
    }
}