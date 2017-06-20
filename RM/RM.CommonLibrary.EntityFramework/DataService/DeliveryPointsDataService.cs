namespace RM.CommonLibrary.EntityFramework.DataService
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Spatial;
    using System.Data.Entity.SqlServer;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using AutoMapper;
    using CommonLibrary.Utilities.HelperMiddleware;
    using ExceptionMiddleware;
    using HelperMiddleware;
    using LoggingMiddleware;
    using RM.CommonLibrary.DataMiddleware;
    using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
    using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
    using RM.CommonLibrary.EntityFramework.DTO;
    using RM.CommonLibrary.EntityFramework.Entities;

    /// <summary>
    /// This class contains methods used for fetching/Inserting Delivery Points data.
    /// </summary>
    public class DeliveryPointsDataService : DataServiceBase<DeliveryPoint, RMDBContext>, IDeliveryPointsDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public DeliveryPointsDataService(IDatabaseFactory<RMDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        #region Public Methods

        /// <summary>
        /// This method is used to fetch Delivery Point by udprn.
        /// </summary>
        /// <param name="udprn">udprn as int</param>
        /// <returns>DeliveryPointDTO</returns>
        public async Task<DeliveryPointDTO> GetDeliveryPointByUDPRN(int udprn)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetDeliveryPointByUDPRN"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                try
                {
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                    var objDeliveryPoint = await DataContext.DeliveryPoints.AsNoTracking().Where(n => n.UDPRN == udprn).SingleOrDefaultAsync();

                    Mapper.Initialize(cfg =>
                    {
                        cfg.CreateMap<DeliveryPoint, DeliveryPointDTO>();
                        cfg.CreateMap<PostalAddress, PostalAddressDTO>().IgnoreAllUnmapped();
                    });

                    Mapper.Configuration.CreateMapper();

                    return Mapper.Map<DeliveryPoint, DeliveryPointDTO>(objDeliveryPoint);
                }
                catch (InvalidOperationException ex)
                {
                    ex.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
                    throw new SystemException(ErrorConstants.Err_InvalidOperationExceptionForCountAsync, ex);
                }
                finally
                {
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                }
            }
        }

        /// <summary>
        /// This method is used to update UDPRN of Delivery Point by matching udprn of postal address id.
        /// </summary>
        /// <param name="addressId">Postal address guid</param>
        /// <param name="udprn">UDPRN id of postal address</param>
        /// <returns>DeliveryPointDTO</returns>
        public bool UpdateDeliveryPointByAddressId(Guid addressId, int udprn)
        {
            bool isDeliveryPointUpdated = false;
            var objDeliveryPoint = DataContext.DeliveryPoints.Where(n => n.Address_GUID == addressId).SingleOrDefault();
            try
            {
                if (objDeliveryPoint != null)
                {
                    objDeliveryPoint.UDPRN = udprn;
                    DataContext.SaveChanges();
                    isDeliveryPointUpdated = true;
                }
                else
                {
                    isDeliveryPointUpdated = false;
                }
            }
            catch (Exception ex)
            {
                isDeliveryPointUpdated = false;
                if (objDeliveryPoint != null)
                {
                    DataContext.Entry(objDeliveryPoint).State = EntityState.Unchanged;
                }

                this.loggingHelper.Log(ex, TraceEventType.Error);
            }

            return isDeliveryPointUpdated;
        }

        /// <summary>
        /// This method is used to insert delivery point.
        /// </summary>
        /// <param name="objDeliveryPoint">Delivery point dto as object</param>
        /// <returns>bool</returns>
        public async Task<bool> InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.InsertDeliveryPoint"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                bool isDeliveryPointInserted = false;
                DeliveryPoint newDeliveryPoint = new DeliveryPoint();
                if (objDeliveryPoint != null)
                {
                    try
                    {
                        newDeliveryPoint.ID = objDeliveryPoint.ID;
                        newDeliveryPoint.Address_GUID = objDeliveryPoint.Address_GUID;
                        newDeliveryPoint.UDPRN = objDeliveryPoint.UDPRN;
                        newDeliveryPoint.LocationXY = objDeliveryPoint.LocationXY;
                        newDeliveryPoint.Latitude = objDeliveryPoint.Latitude;
                        newDeliveryPoint.Longitude = objDeliveryPoint.Longitude;
                        newDeliveryPoint.LocationProvider_GUID = objDeliveryPoint.LocationProvider_GUID;
                        newDeliveryPoint.DeliveryPointUseIndicator_GUID = objDeliveryPoint.DeliveryPointUseIndicator_GUID;
                        DataContext.DeliveryPoints.Add(newDeliveryPoint);
                        await DataContext.SaveChangesAsync();
                        isDeliveryPointInserted = true;
                    }
                    catch (Exception dbUpdateException)
                    {
                        isDeliveryPointInserted = false;
                        DataContext.Entry(newDeliveryPoint).State = EntityState.Unchanged;
                        throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("Delivery Point for UDPRN:", newDeliveryPoint.UDPRN)));
                    }
                }
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return isDeliveryPointInserted;
            }
        }

        /// <summary>
        /// This method is used to fetch delivery points for advance search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>Task List of Delivery Point Dto</returns>
        public async Task<List<DeliveryPointDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText, Guid unitGuid)
        {
            DbGeometry polygon = DataContext.UnitLocations.AsNoTracking().Where(x => x.ID == unitGuid).Select(x => x.UnitBoundryPolygon).SingleOrDefault();

            var result = await DataContext.DeliveryPoints.AsNoTracking()
                .Include(l => l.PostalAddress)
                .Where(x => x.LocationXY.Intersects(polygon) && (x.PostalAddress.OrganisationName.Contains(searchText)
                                || x.PostalAddress.BuildingName.Contains(searchText)
                                || x.PostalAddress.SubBuildingName.Contains(searchText)
                                || SqlFunctions.StringConvert((double)x.PostalAddress.BuildingNumber).StartsWith(searchText)
                                || x.PostalAddress.Thoroughfare.Contains(searchText)
                                || x.PostalAddress.DependentLocality.Contains(searchText)))
                                .Select(l => new DeliveryPointDTO
                                {
                                    PostalAddress = new PostalAddressDTO
                                    {
                                        OrganisationName = l.PostalAddress.OrganisationName,
                                        BuildingName = l.PostalAddress.BuildingName,
                                        SubBuildingName = l.PostalAddress.SubBuildingName,
                                        BuildingNumber = l.PostalAddress.BuildingNumber,
                                        Thoroughfare = l.PostalAddress.Thoroughfare,
                                        DependentLocality = l.PostalAddress.DependentLocality,
                                        UDPRN = l.PostalAddress.UDPRN
                                    },
                                    ID = l.ID
                                })
                                .ToListAsync();

            return result;
        }

        /// <summary>
        /// Fetch Delivery point for Basic Search
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>The result set of delivery point.</returns>
        public async Task<List<DeliveryPointDTO>> FetchDeliveryPointsForBasicSearch(string searchText, Guid unitGuid)
        {
            if (string.IsNullOrWhiteSpace(searchText) || Guid.Empty.Equals(unitGuid))
            {
                throw new ArgumentNullException(searchText, string.Format(ErrorConstants.Err_ArgumentmentNullException, string.Concat(searchText, unitGuid)));
            }

            int takeCount = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.SearchResultCount]);
            searchText = searchText ?? string.Empty;

            DbGeometry polygon = DataContext.UnitLocations.AsNoTracking().Where(x => x.ID == unitGuid)
                .Select(x => x.UnitBoundryPolygon).SingleOrDefault();

            var result = await DataContext.DeliveryPoints.AsNoTracking()
                .Include(l => l.PostalAddress)
                .Where(x => x.LocationXY.Intersects(polygon) && (x.PostalAddress.OrganisationName.Contains(searchText)
                                                                 || x.PostalAddress.BuildingName.Contains(searchText)
                                                                 || x.PostalAddress.SubBuildingName.Contains(searchText)
                                                                 || SqlFunctions.StringConvert((double)x.PostalAddress
                                                                     .BuildingNumber).StartsWith(searchText)
                                                                 || x.PostalAddress.Thoroughfare.Contains(searchText)
                                                                 || x.PostalAddress.DependentLocality.Contains(
                                                                     searchText)))
                .Select(l => new DeliveryPointDTO
                {
                    PostalAddress = new PostalAddressDTO
                    {
                        OrganisationName = l.PostalAddress.OrganisationName,
                        BuildingName = l.PostalAddress.BuildingName,
                        SubBuildingName = l.PostalAddress.SubBuildingName,
                        BuildingNumber = l.PostalAddress.BuildingNumber,
                        Thoroughfare = l.PostalAddress.Thoroughfare,
                        DependentLocality = l.PostalAddress.DependentLocality,
                        UDPRN = l.UDPRN
                    },
                    ID = l.ID
                })
                .Take(takeCount)
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// Get the count of delivery points
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>The total count of delivery points</returns>
        public async Task<int> GetDeliveryPointsCount(string searchText, Guid unitGuid)
        {
            try
            {
                searchText = searchText ?? string.Empty;
                DbGeometry polygon = DataContext.UnitLocations.AsNoTracking().Where(x => x.ID == unitGuid).Select(x => x.UnitBoundryPolygon).SingleOrDefault();

                var result = await DataContext.DeliveryPoints.AsNoTracking()
                  .Include(l => l.PostalAddress)
                  .Where(x => x.LocationXY.Intersects(polygon) && (x.PostalAddress.OrganisationName.Contains(searchText)
                                  || x.PostalAddress.BuildingName.Contains(searchText)
                                  || x.PostalAddress.SubBuildingName.Contains(searchText)
                                  || SqlFunctions.StringConvert((double)x.PostalAddress.BuildingNumber).StartsWith(searchText)
                                  || x.PostalAddress.Thoroughfare.Contains(searchText)
                                  || x.PostalAddress.DependentLocality.Contains(searchText))).CountAsync();

                return result;
            }
            catch (InvalidOperationException ex)
            {
                ex.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
                throw new SystemException(ErrorConstants.Err_InvalidOperationExceptionForSingleorDefault, ex);
            }
            catch (OverflowException overflow)
            {
                overflow.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
                throw new SystemException(ErrorConstants.Err_OverflowException, overflow);
            }
        }

        /// <summary>
        /// This method is used to Get Delivery Points Dto as data.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">unit unique identifier.</param>
        /// <returns>List of Delivery Point Dto</returns>
        public List<DeliveryPointDTO> GetDeliveryPoints(string boundingBoxCoordinates, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetDeliveryPoints"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                List<DeliveryPoint> deliveryPoints = GetDeliveryPointsCoordinatesDatabyBoundingBox(boundingBoxCoordinates, unitGuid).ToList();

                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<DeliveryPoint, DeliveryPointDTO>();
                    cfg.CreateMap<PostalAddress, PostalAddressDTO>().IgnoreAllUnmapped();
                });

                Mapper.Configuration.CreateMapper();
                var deliveryPointDto = Mapper.Map<List<DeliveryPoint>, List<DeliveryPointDTO>>(deliveryPoints);

                deliveryPointDto.ForEach(dpDTO =>
                {
                    dpDTO.PostalAddress = GenericMapper.Map<PostalAddress, PostalAddressDTO>(deliveryPoints.Where(dp => dp.ID == dpDTO.ID).SingleOrDefault().PostalAddress);
                });
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodExitEventId, LoggerTraceConstants.Title);

                return deliveryPointDto;
            }
        }

        /// <summary>
        /// This Method provides Route Name for a single DeliveryPoint
        /// </summary>
        /// <param name="deliveryPointId">deliveryPointId as GUID</param>
        /// <returns>Route Name for a single DeliveryPoint</returns>
        public string GetRouteForDeliveryPoint(Guid deliveryPointId)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            using (loggingHelper.RMTraceManager.StartTrace(LoggerTraceConstants.DataLayer + methodName))
            {
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetRouteForDeliveryPointPriority, LoggerTraceConstants.GetRouteForDeliveryPointDataMethodEntryEventId, LoggerTraceConstants.Title);
                string routeName = string.Empty;
                var result = (from dp in DataContext.DeliveryPoints.AsNoTracking()
                              join bs in DataContext.BlockSequences.AsNoTracking() on dp.ID equals bs.OperationalObject_GUID
                              join b in DataContext.Blocks.AsNoTracking() on bs.Block_GUID equals b.ID
                              join drb in DataContext.DeliveryRouteBlocks.AsNoTracking() on b.ID equals drb.Block_GUID
                              join dr in DataContext.DeliveryRoutes.AsNoTracking() on drb.DeliveryRoute_GUID equals dr.ID
                              join pa in DataContext.PostalAddresses.AsNoTracking() on dp.Address_GUID equals pa.ID
                              where dp.ID == deliveryPointId && b.BlockType == Constants.UnSequenced
                              select new
                              {
                                  RouteName = dr.RouteName,
                                  RouteId = dr.ID,
                                  PostcodeId = pa.PostCodeGUID
                              }).SingleOrDefault();
                if (result != null)
                {
                    var isPrimaryRoute = (from drp in DataContext.DeliveryRoutePostcodes.AsNoTracking() where drp.Postcode_GUID == result.PostcodeId && drp.DeliveryRoute_GUID == result.RouteId select drp.IsPrimaryRoute).ToList();
                    if (isPrimaryRoute != null && isPrimaryRoute.Count > 0)
                    {
                        routeName = isPrimaryRoute[0] == true ? Constants.PRIMARYROUTE + result.RouteName.Trim() : Constants.SECONDARYROUTE + result.RouteName.Trim();
                    }
                    else
                    {
                        routeName = result.RouteName.Trim();
                    }
                }

                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetRouteForDeliveryPointPriority, LoggerTraceConstants.GetRouteForDeliveryPointDataMethodExitEventId, LoggerTraceConstants.Title);
                return routeName;

            }
        }

        /// <summary>
        /// This Method fetches DPUse value for the DeliveryPoint
        /// </summary>
        /// <param name="referenceDataCategoryDtoList">
        /// referenceDataCategoryDtoList as List of ReferenceDataCategoryDTO
        /// </param>
        /// <param name="deliveryPointId">deliveryPointId as GUID</param>
        /// <returns>DPUse value as string</returns>
        public string GetDPUse(Guid deliveryPointId, Guid operationalObjectTypeForDpOrganisation, Guid operationalObjectTypeForDpResidential)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            using (loggingHelper.RMTraceManager.StartTrace(LoggerTraceConstants.DataLayer + methodName))
            {
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetDPUsePriority, LoggerTraceConstants.GetDPUseDataMethodEntryEventId, LoggerTraceConstants.Title);
                string dpUsetype = string.Empty;

                var dpUse = from dp in DataContext.DeliveryPoints.AsNoTracking()
                            where dp.ID == deliveryPointId
                            select new { DeliveryPointUseIndicator_GUID = dp.DeliveryPointUseIndicator_GUID };

                List<Guid> deliveryPointUseIndicator = dpUse.Select(n => n.DeliveryPointUseIndicator_GUID).ToList();
                if (deliveryPointUseIndicator.Count > 0)
                {
                    if (deliveryPointUseIndicator[0] == operationalObjectTypeForDpOrganisation)
                    {
                        dpUsetype = Constants.DpUseOrganisation;
                    }
                    else if (deliveryPointUseIndicator[0] == operationalObjectTypeForDpResidential)
                    {
                        dpUsetype = Constants.DpUseResidential;
                    }
                }

                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetDPUsePriority, LoggerTraceConstants.GetDPUseDataMethodExitEventId, LoggerTraceConstants.Title);
                return dpUsetype;
            }
        }

        /// <summary>
        /// This method updates delivery point location using UDPRN
        /// </summary>
        /// <param name="deliveryPointDto">deliveryPointDto as DTO</param>
        /// <returns>updated delivery point</returns>
        public async Task<int> UpdateDeliveryPointLocationOnUDPRN(DeliveryPointDTO deliveryPointDto)
        {
            int status = 0;
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("DataService.UpdateDeliveryPointLocationOnUDPRN"))
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                    DeliveryPoint deliveryPoint =
                        DataContext.DeliveryPoints.SingleOrDefault(dp => dp.UDPRN == deliveryPointDto.UDPRN);

                    if (deliveryPoint != null)
                    {
                        deliveryPoint.Longitude = deliveryPointDto.Longitude;
                        deliveryPoint.Latitude = deliveryPointDto.Latitude;
                        deliveryPoint.LocationXY = deliveryPointDto.LocationXY;
                        deliveryPoint.LocationProvider_GUID = deliveryPointDto.LocationProvider_GUID;
                        deliveryPoint.Positioned = deliveryPointDto.Positioned;
                        status = await DataContext.SaveChangesAsync();
                    }
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                    return status;
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbConcurrencyException(ErrorConstants.Err_Concurrency);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlUpdateException, string.Concat("delivery point location for:", deliveryPointDto.ID)));
            }
            catch (NotSupportedException notSupportedException)
            {
                notSupportedException.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
                throw new InfrastructureException(notSupportedException, ErrorConstants.Err_NotSupportedException);
            }
            catch (ObjectDisposedException disposedException)
            {
                disposedException.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
                throw new ServiceException(disposedException, ErrorConstants.Err_ObjectDisposedException);
            }
        }

        /// <summary>
        /// This method updates delivery point location using ID
        /// </summary>
        /// <param name="deliveryPointDto">deliveryPointDto as DTO</param>
        /// <returns>updated delivery point</returns>
        public async Task<Guid> UpdateDeliveryPointLocationOnID(DeliveryPointDTO deliveryPointDto)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("DataService.UpdateDeliveryPointLocationOnID"))
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                    DeliveryPoint deliveryPoint =
                        DataContext.DeliveryPoints.SingleOrDefault(dp => dp.ID == deliveryPointDto.ID);

                    if (deliveryPoint != null)
                    {
                        deliveryPoint.Longitude = deliveryPointDto.Longitude;
                        deliveryPoint.Latitude = deliveryPointDto.Latitude;
                        deliveryPoint.LocationXY = deliveryPointDto.LocationXY;
                        deliveryPoint.LocationProvider_GUID = deliveryPointDto.LocationProvider_GUID;
                        deliveryPoint.Positioned = deliveryPointDto.Positioned;

                        DataContext.Entry(deliveryPoint).State = EntityState.Modified;
                        DataContext.Entry(deliveryPoint).OriginalValues[Constants.ROWVERSION] = deliveryPointDto.RowVersion;
                        await DataContext.SaveChangesAsync();
                    }

                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                    return deliveryPoint != null ? deliveryPoint.ID : Guid.Empty;
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbConcurrencyException(ErrorConstants.Err_Concurrency);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlUpdateException, string.Concat("delivery point location for:", deliveryPointDto.ID)));
            }
            catch (NotSupportedException notSupportedException)
            {
                notSupportedException.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
                throw new InfrastructureException(notSupportedException, ErrorConstants.Err_NotSupportedException);
            }
            catch (ObjectDisposedException disposedException)
            {
                disposedException.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
                throw new ServiceException(disposedException, ErrorConstants.Err_ObjectDisposedException);
            }
        }

        /// <summary>
        /// This method fetches delivery point data for given Guid
        /// </summary>
        /// <param name="Guid">Guid as unique Identifier</param>
        /// <returns>deliveryPoint DTO</returns>
        public List<DeliveryPointDTO> GetDeliveryPointListByGuid(Guid id)
        {
            List<DeliveryPoint> deliveryPoints = DataContext.DeliveryPoints.Where(dp => dp.ID == id).ToList();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DeliveryPoint, DeliveryPointDTO>();
                cfg.CreateMap<PostalAddress, PostalAddressDTO>().IgnoreAllUnmapped();
            });

            Mapper.Configuration.CreateMapper();
            var deliveryPointDto = Mapper.Map<List<DeliveryPoint>, List<DeliveryPointDTO>>(deliveryPoints);

            deliveryPointDto.ForEach(dpDTO =>
            {
                dpDTO.PostalAddress = GenericMapper.Map<PostalAddress, PostalAddressDTO>(deliveryPoints.Where(dp => dp.ID == dpDTO.ID).SingleOrDefault().PostalAddress);
            });

            return deliveryPointDto;
        }

        /// <summary>
        /// This method fetches delivery point data for given UDPRN
        /// </summary>
        /// <param name="udprn">udprn as int</param>
        /// <returns>addDeliveryPointDto</returns>
        public AddDeliveryPointDTO GetDetailDeliveryPointByUDPRN(int udprn)
        {
            AddDeliveryPointDTO addDeliveryPointDto = default(AddDeliveryPointDTO);
            var deliveryPoints = (from dp in DataContext.DeliveryPoints.AsNoTracking()
                                  join pa in DataContext.PostalAddresses.AsNoTracking() on dp.Address_GUID equals pa.ID
                                  join al in DataContext.AddressLocations.AsNoTracking() on pa.UDPRN equals al.UDPRN
                                  where dp.UDPRN == udprn
                                  select new
                                  {
                                      DeliveryPoint = dp,
                                      PostalAddress = pa,
                                      AddressLocation = al,
                                  }).SingleOrDefault();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DeliveryPoint, DeliveryPointDTO>();
                cfg.CreateMap<PostalAddress, PostalAddressDTO>();
                cfg.CreateMap<AddressLocation, AddressLocationDTO>();
            });

            Mapper.Configuration.CreateMapper();

            if (deliveryPoints != null)
            {
                addDeliveryPointDto = new AddDeliveryPointDTO()
                {
                    DeliveryPointDTO = Mapper.Map<DeliveryPoint, DeliveryPointDTO>(deliveryPoints.DeliveryPoint),
                    AddressLocationDTO = Mapper.Map<AddressLocation, AddressLocationDTO>(deliveryPoints.AddressLocation),
                    PostalAddressDTO = Mapper.Map<PostalAddress, PostalAddressDTO>(deliveryPoints.PostalAddress)
                };
            }

            return addDeliveryPointDto;
        }

        /// <summary>
        /// Get the delivery points by the Postal Address Guid
        /// </summary>
        /// <param name="addressId">Postal Address Guid to find corresponding delivery point</param>
        /// <returns>DeliveryPointDTO object</returns>
        public DeliveryPointDTO GetDeliveryPointByPostalAddress(Guid addressId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetDeliveryPointByPostalAddress"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                DeliveryPoint deliveryPoint = DataContext.DeliveryPoints.Where(dp => dp.Address_GUID == addressId)
                .SingleOrDefault();

                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<DeliveryPoint, DeliveryPointDTO>();
                    cfg.CreateMap<PostalAddress, PostalAddressDTO>().IgnoreAllUnmapped();
                });

                Mapper.Configuration.CreateMapper();
                var deliveryPointDto = Mapper.Map<DeliveryPoint, DeliveryPointDTO>(deliveryPoint);
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodExitEventId, LoggerTraceConstants.Title);

                return deliveryPointDto;
            }
        }

        /// <summary>
        /// This method checks delivery point for given UDPRN exists or not
        /// </summary>
        /// <param name="udprn">uDPRN as int</param>
        /// <returns>boolean value true or false</returns>
        public async Task<bool> DeliveryPointExists(int udprn)
        {
            return await DataContext.DeliveryPoints.AsNoTracking().Where(dp => ((int)dp.UDPRN).Equals(udprn)).AnyAsync();
        }

        /// <summary>
        /// Calculte distance between two points
        /// </summary>
        /// <param name="deliveryPointDTO">deliveryPoint DTO</param>
        /// <param name="newPoint">newPoint as DbGeometry</param>
        /// <returns>distance as double</returns>
        public double? GetDeliveryPointDistance(DeliveryPointDTO deliveryPointDTO, DbGeometry newPoint)
        {
            double? distance = 0;
            if (deliveryPointDTO != null)
            {
                distance = deliveryPointDTO.LocationXY.Distance(newPoint);
            }

            return distance;
        }

        /// <summary>
        /// Get the delivery point row version
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns>byte[]</returns>
        public byte[] GetDeliveryPointRowVersion(Guid id)
        {
            byte[] rowVersion = default(byte[]);
            DeliveryPoint deliveryPoint = DataContext.DeliveryPoints.Where(dp => dp.ID == id).SingleOrDefault();
            if (deliveryPoint != null)
            {
                rowVersion = deliveryPoint.RowVersion;
            }

            return rowVersion;
        }

        /// <summary>
        /// This method is used to fetch Delivery Point by unique identifier.
        /// </summary>
        /// <param name="deliveryPointGuid">Delivery point unique identifier.</param>
        /// <returns>DeliveryPointDTO</returns>
        public DeliveryPointDTO GetDeliveryPoint(Guid deliveryPointGuid)
        {
            var objDeliveryPoint = DataContext.DeliveryPoints.Include(x => x.PostalAddress).AsNoTracking().Single(n => n.ID == deliveryPointGuid);

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<PostalAddress, PostalAddressDTO>();
                cfg.CreateMap<DeliveryPoint, DeliveryPointDTO>();
            });

            Mapper.Configuration.CreateMapper();

            return Mapper.Map<DeliveryPoint, DeliveryPointDTO>(objDeliveryPoint);
        }

        /// <summary>
        /// This method updates delivery point access link status
        /// </summary>
        /// <param name="deliveryPointDTO">deliveryPointDto as DTO</param>
        public bool UpdateDeliveryPointAccessLinkCreationStatus(DeliveryPointDTO deliveryPointDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.UpdateDeliveryPointAccessLinkCreationStatus"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                bool isDeliveryPointUpdated = false;
                using (RMDBContext rmDbContext = new RMDBContext())
                {
                    DeliveryPoint deliveryPoint = rmDbContext.DeliveryPoints.SingleOrDefault(dp => dp.ID == deliveryPointDTO.ID);

                    if (deliveryPoint != null)
                    {
                        deliveryPoint.AccessLinkPresent = deliveryPointDTO.AccessLinkPresent;

                        rmDbContext.Entry(deliveryPoint).State = EntityState.Modified;
                        rmDbContext.Entry(deliveryPoint).OriginalValues[Constants.ROWVERSION] = deliveryPointDTO.RowVersion;
                        isDeliveryPointUpdated = rmDbContext.SaveChanges() > 0;
                    }
                }
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return isDeliveryPointUpdated;
            }
        }

        /// <summary> This method is used to get the delivery points crossing the operationalObject for a given extent</summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param>
        /// <param name="accessLink">access link coordinate array</param>
        /// <returns>List<DeliveryPointDTO></returns>
        public List<DeliveryPointDTO> GetDeliveryPointsCrossingOperationalObject(string boundingBoxCoordinates, DbGeometry operationalObject)
        {
            List<DeliveryPointDTO> deliveryPointDTOs = new List<DeliveryPointDTO>();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<PostalAddress, PostalAddressDTO>();
                cfg.CreateMap<DeliveryPoint, DeliveryPointDTO>();
            });

            Mapper.Configuration.CreateMapper();
            DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(boundingBoxCoordinates.ToString(), Constants.BNGCOORDINATESYSTEM);

            List<DeliveryPoint> crossingDeliveryPoints = DataContext.DeliveryPoints.AsNoTracking().Where(dp => dp.LocationXY != null && dp.LocationXY.Intersects(extent) && dp.LocationXY.Crosses(operationalObject)).ToList();
            List<DeliveryPointDTO> crossingAccessLinkDTOs = Mapper.Map<List<DeliveryPoint>, List<DeliveryPointDTO>>(crossingDeliveryPoints);
            deliveryPointDTOs.AddRange(crossingAccessLinkDTOs);

            List<DeliveryPoint> overLappingDeliveryPoints = DataContext.DeliveryPoints.AsNoTracking().Where(dp => dp.LocationXY != null && dp.LocationXY.Intersects(extent) && dp.LocationXY.Overlaps(operationalObject)).ToList();
            List<DeliveryPointDTO> overLappingAccessLinkDTOs = Mapper.Map<List<DeliveryPoint>, List<DeliveryPointDTO>>(overLappingDeliveryPoints);
            deliveryPointDTOs.AddRange(overLappingAccessLinkDTOs);

            return deliveryPointDTOs;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// This method is used to Get delivery Point boundingBox data.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">unit unique identifier.</param>
        /// <returns>List of Delivery Point Entity</returns>
        private IEnumerable<DeliveryPoint> GetDeliveryPointsCoordinatesDatabyBoundingBox(string boundingBoxCoordinates, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetDeliveryPointsCoordinatesDatabyBoundingBox"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                IEnumerable<DeliveryPoint> deliveryPoints = default(IEnumerable<DeliveryPoint>);
                if (!string.IsNullOrEmpty(boundingBoxCoordinates))
                {
                    DbGeometry polygon = DataContext.UnitLocations.AsNoTracking().Where(x => x.ID == unitGuid).Select(x => x.UnitBoundryPolygon).SingleOrDefault();

                    DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(boundingBoxCoordinates.ToString(), Constants.BNGCOORDINATESYSTEM);

                    deliveryPoints = DataContext.DeliveryPoints.AsNoTracking().Where(dp => dp.LocationXY.Intersects(extent) && dp.LocationXY.Intersects(polygon));
                }

                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return deliveryPoints;
            }
        }

        #endregion Private Methods
    }
}