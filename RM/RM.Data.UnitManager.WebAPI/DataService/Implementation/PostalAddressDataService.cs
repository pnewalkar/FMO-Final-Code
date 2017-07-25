using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.UnitManager.WebAPI.DataDTO;
using RM.DataManagement.UnitManager.WebAPI.DataDTO;
using RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces;
using RM.DataManagement.UnitManager.WebAPI.Entity;

namespace RM.DataManagement.UnitManager.WebAPI.DataService
{
    /// <summary>
    /// DataService to interact with postal address entity and handle CRUD operations.
    /// </summary>
    public class PostalAddressDataService : DataServiceBase<Postcode, UnitManagerDbContext>, IPostalAddressDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        /// <summary>
        /// Parameterised Constructor
        /// </summary>
        /// <param name="databaseFactory"></param>
        /// <param name="loggingHelper"></param>
        public PostalAddressDataService(IDatabaseFactory<UnitManagerDbContext> databaseFactory, ILoggingHelper loggingHelper)
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
        /// Filter PostalAddress based on the search text
        /// </summary>
        /// <param name="searchText">searchText</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of Postcodes</returns>
        public async Task<List<string>> GetPostalAddressSearchDetails(string searchText, Guid unitGuid, List<Guid> addresstypeIDs)
        {
            string methodName = typeof(UnitLocationDataService) + "." + nameof(GetPostalAddressSearchDetails);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostalAddressSearchDetails"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId);

                var searchresults = await (from lph in DataContext.LocationPostcodeHierarchies.AsNoTracking()
                                           join pch in DataContext.PostcodeHierarchies.AsNoTracking() on lph.PostcodeHierarchyID equals pch.ID
                                           join pa in DataContext.PostalAddresses.AsNoTracking() on pch.Postcode equals pa.Postcode
                                           where addresstypeIDs.Contains(pa.AddressType_GUID) &&
                                           (pa.Thoroughfare.Contains(searchText) || pa.Postcode.Contains(searchText)) &&
                                           lph.LocationID == unitGuid
                                           select new { SearchResult = string.IsNullOrEmpty(pa.Thoroughfare) ? pa.Postcode : pa.Thoroughfare + "," + pa.Postcode }).Distinct().OrderBy(x => x.SearchResult).ToListAsync();

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId);
                return searchresults.Select(n => n.SearchResult).Distinct().ToList();
            }
        }

        public async Task<List<PostalAddressDataDTO>> GetPostalAddressDetails(string selectedItem, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostalAddressDetails(string, Guid)"))
            {
                try
                {
                    string methodName = typeof(UnitLocationDataService) + "." + nameof(GetPostalAddressDetails);
                    loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId);

                    List<string> lstPocodes = new List<string>();
                    List<PostalAddressDataDTO> postalAddressDTO = new List<PostalAddressDataDTO>();
                    string[] selectedItems = selectedItem.Split(',');
                    string postCode = string.Empty;
                    string streetName = string.Empty;
                    List<PostalAddress> postalAddress = null;

                    if (selectedItems.Count() == 2)
                    {
                        postCode = selectedItems[1].Trim();
                        streetName = selectedItems[0].Trim();
                        postalAddress = await (from lph in DataContext.LocationPostcodeHierarchies.AsNoTracking()
                                               join pch in DataContext.PostcodeHierarchies.AsNoTracking() on lph.PostcodeHierarchyID equals pch.ID
                                               join pa in DataContext.PostalAddresses.AsNoTracking() on pch.Postcode equals pa.Postcode
                                               where pa.Postcode == postCode && pa.Thoroughfare == streetName &&
                                               lph.LocationID == unitGuid
                                               select pa).ToListAsync();
                    }
                    else
                    {
                        postCode = selectedItems[0].Trim();
                        postalAddress = await (from lph in DataContext.LocationPostcodeHierarchies.AsNoTracking()
                                               join pch in DataContext.PostcodeHierarchies.AsNoTracking() on lph.PostcodeHierarchyID equals pch.ID
                                               join pa in DataContext.PostalAddresses.AsNoTracking() on pch.Postcode equals pa.Postcode
                                               where pa.Postcode == postCode &&
                                               lph.LocationID == unitGuid
                                               select pa).ToListAsync();
                    }

                    Mapper.Initialize(cfg =>
                    {
                        cfg.CreateMap<PostalAddress, PostalAddressDataDTO>();
                    });

                    postalAddressDTO = Mapper.Map<List<PostalAddress>, List<PostalAddressDataDTO>>(postalAddress);

                    /*postalAddress.ForEach(p => p.Postcode1.DeliveryRoutePostcodes.ToList().ForEach(d =>
                    {
                        if (d.IsPrimaryRoute)
                        {
                            postalAddressDTO.Where(pa => pa.Postcode == d.Postcode.PostcodeUnit).Select(pa => pa).ToList().ForEach(paDTO =>
                            {
                                if (paDTO.RouteDetails == null)
                                {
                                    paDTO.RouteDetails = new List<BindingEntity>();
                                }

                                if (paDTO.RouteDetails.All(b => b.DisplayText != PRIMARYROUTE + d.DeliveryRoute.RouteName.Trim()))
                                {
                                    paDTO.RouteDetails.Add(new BindingEntity() { DisplayText = PRIMARYROUTE + d.DeliveryRoute.RouteName.Trim(), Value = d.DeliveryRoute.ID });
                                }
                            });
                        }
                        else
                        {
                            postalAddressDTO.Where(pa => pa.Postcode == d.Postcode.PostcodeUnit).Select(pa => pa).ToList().ForEach(paDTO =>
                            {
                                if (paDTO.RouteDetails == null)
                                {
                                    paDTO.RouteDetails = new List<BindingEntity>();
                                }

                                if (paDTO.RouteDetails.All(b => b.DisplayText != SECONDARYROUTE + d.DeliveryRoute.RouteName.Trim()))
                                {
                                    paDTO.RouteDetails.Add(new BindingEntity() { DisplayText = SECONDARYROUTE + d.DeliveryRoute.RouteName.Trim(), Value = d.DeliveryRoute.ID });
                                }
                            });
                        }
                    }));

                    var postCodes = await DataContext.UnitLocationPostcodes.AsNoTracking().Where(p => p.Unit_GUID == unitGuid).Select(s => s.PoscodeUnit_GUID).Distinct().ToListAsync();
                    if (postalAddressDTO != null && postalAddressDTO.Count > 0 && (postalAddressDTO[0].RouteDetails == null || postalAddressDTO[0].RouteDetails.Count == 0))
                    {
                        List<BindingEntity> routeDetails = new List<BindingEntity>();
                        var routes = await DataContext.DeliveryRoutePostcodes.AsNoTracking().Where(dr => postCodes.Contains(dr.Postcode_GUID)).ToListAsync();
                        routes.ForEach(r =>
                        {
                            if (!routeDetails.Where(rd => rd.Value == r.DeliveryRoute.ID).Any())
                            {
                                routeDetails.Add(new BindingEntity() { DisplayText = r.DeliveryRoute.RouteName, Value = r.DeliveryRoute.ID });
                            }
                        });
                        postalAddressDTO[0].RouteDetails = new List<BindingEntity>(routeDetails.Distinct().OrderBy(n => n.DisplayText));
                    }*/

                    loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId);
                    return postalAddressDTO;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}