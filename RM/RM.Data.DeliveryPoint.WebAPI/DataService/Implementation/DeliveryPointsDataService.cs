namespace RM.DataManagement.DeliveryPoint.WebAPI.DataService
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Spatial;
    using System.Data.Entity.SqlServer;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using AutoMapper;
    using CommonLibrary.LoggingMiddleware;
    using CommonLibrary.Utilities.HelperMiddleware;
    using Data.DeliveryPoint.WebAPI.DTO;
    using RM.CommonLibrary.ExceptionMiddleware;
    using RM.CommonLibrary.HelperMiddleware;
    using RM.Data.DeliveryPoint.WebAPI.Entities;

    /// <summary>
    /// This class contains methods used for fetching/Inserting Delivery Points data.
    /// </summary>
    public class DeliveryPointsDataService : DataServiceBase<DeliveryPoint, DeliveryPointDBContext>, IDeliveryPointsDataService
    {
        private const string ROWVERSION = "RowVersion";
        private const string UnSequenced = "U";
        private const string PRIMARYROUTE = "Primary - ";
        private const string SECONDARYROUTE = "Secondary - ";
        private const int BNGCOORDINATESYSTEM = 27700;
        private const string SearchResultCount = "SearchResultCount";
        private const string DpUseResidential = "Residential";
        private const string DpUseOrganisation = "Organisation";

        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public DeliveryPointsDataService(IDatabaseFactory<DeliveryPointDBContext> databaseFactory, ILoggingHelper loggingHelper)
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
        public async Task<DeliveryPointDataDTO> GetDeliveryPointByUDPRN(int udprn)
        {
            try
            {
                var objDeliveryPoint = await DataContext.DeliveryPoints
                    .AsNoTracking().Include(s => s.PostalAddress).Include(x => x.NetworkNode).Include(x => x.NetworkNode.Location).Where(n => n.PostalAddress.UDPRN == udprn).SingleOrDefaultAsync();

                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<DeliveryPoint, DeliveryPointDTO>();
                    cfg.CreateMap<Location, LocationDataDTO>();
                    cfg.CreateMap<NetworkNode, NetworkNodeDataDTO>();
                    cfg.CreateMap<PostalAddress, PostalAddressDataDTO>();
                });

                Mapper.Configuration.CreateMapper();

                return Mapper.Map<DeliveryPoint, DeliveryPointDataDTO>(objDeliveryPoint);
            }
            catch (InvalidOperationException ex)
            {
                ex.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
                throw new SystemException(ErrorConstants.Err_InvalidOperationExceptionForCountAsync, ex);
            }
        }

        /// <summary>
        /// This method is used to insert delivery point.
        /// </summary>
        /// <param name="objDeliveryPoint">Delivery point dto as object</param>
        /// <returns>bool</returns>
        public async Task<bool> InsertDeliveryPoint(DeliveryPointDataDTO objDeliveryPoint)
        {
            bool isDeliveryPointInserted = false;
            DeliveryPoint deliveryPoint = new DeliveryPoint();
            DeliveryPointStatus deliveryPointStatus = new DeliveryPointStatus();
            NetworkNode networkNode = new NetworkNode();
            Location location = new Location();
            using (loggingHelper.RMTraceManager.StartTrace("Data.InsertDeliveryPoint"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodEntryEventId, LoggerTraceConstants.Title);
                if (objDeliveryPoint != null)
                {
                    try
                    {
                        objDeliveryPoint.ID = Guid.NewGuid();
                        deliveryPoint.ID = objDeliveryPoint.ID;
                        deliveryPoint.PostalAddressID = objDeliveryPoint.PostalAddress.ID;
                        deliveryPoint.DeliveryPointUseIndicatorGUID = objDeliveryPoint.DeliveryPointUseIndicatorGUID;

                        deliveryPointStatus.ID = Guid.NewGuid();
                        deliveryPointStatus.LocationID = objDeliveryPoint.ID;
                        deliveryPointStatus.DeliveryPointStatusGUID = objDeliveryPoint.DeliveryPointStatus.First().OperationalStatusGUID;
                        deliveryPointStatus.RowCreateDateTime = DateTime.UtcNow;
                        deliveryPointStatus.StartDateTime = DateTime.UtcNow;
                        deliveryPoint.DeliveryPointStatus.Add(deliveryPointStatus);

                        networkNode.ID = objDeliveryPoint.ID;
                        networkNode.DataProviderGUID = objDeliveryPoint.NetworkNode.DataProviderGUID;
                        networkNode.RowCreateDateTime = DateTime.UtcNow;
                        networkNode.DeliveryPoint = deliveryPoint;
                        networkNode.NetworkNodeType_GUID = objDeliveryPoint.NetworkNode.NetworkNodeType_GUID;

                        location.Shape = objDeliveryPoint.NetworkNode.Location.Shape;
                        location.ID = objDeliveryPoint.ID;
                        location.NetworkNode = networkNode;
                        location.RowCreateDateTime = DateTime.UtcNow;
                        DataContext.Locations.Add(location);

                        DataContext.DeliveryPoints.Add(deliveryPoint);

                        await DataContext.SaveChangesAsync();
                        isDeliveryPointInserted = true;
                    }
                    catch (Exception dbUpdateException)
                    {
                        isDeliveryPointInserted = false;
                        DataContext.Entry(deliveryPoint).State = EntityState.Unchanged;
                        loggingHelper.Log(dbUpdateException, TraceEventType.Error);
                        throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("Delivery Point for addressId:", deliveryPoint.PostalAddressID)));
                    }
                }
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return isDeliveryPointInserted;
            }
        }

        public async Task<bool> UpdatePAFIndicator(Guid addressGuid, Guid pafIndicator)
        {
            bool isDeliveryPointUpdated = false;
            List<DeliveryPoint> deliveryPoints = new List<DeliveryPoint>();
            using (loggingHelper.RMTraceManager.StartTrace("Data.UpdatePAFIndicator"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodEntryEventId, LoggerTraceConstants.Title);
                try
                {
                    deliveryPoints = DataContext.DeliveryPoints.Where(dp => dp.PostalAddressID == addressGuid).ToList();
                    deliveryPoints.ForEach(dp => dp.DeliveryPointUseIndicatorGUID = pafIndicator);
                    await DataContext.SaveChangesAsync();
                    isDeliveryPointUpdated = true;
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                }
                catch (Exception dbUpdateException)
                {
                    isDeliveryPointUpdated = false;
                    DataContext.Entry(deliveryPoints).State = EntityState.Unchanged;
                    loggingHelper.Log(dbUpdateException, TraceEventType.Error);
                    throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("Delivery Point for addressId:", addressGuid)));
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return isDeliveryPointUpdated;
            }
        }

        /// <summary>
        /// This method is used to fetch delivery points for advance search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>Task List of Delivery Point Dto</returns>
        public async Task<List<DeliveryPointDataDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText, Guid unitGuid)
        {
            DbGeometry polygon = DataContext.Locations.AsNoTracking().Where(x => x.ID == unitGuid).Select(x => x.Shape).SingleOrDefault();

            var result = await DataContext.DeliveryPoints.AsNoTracking()
                .Include(l => l.PostalAddress)
                .Where(x => x.NetworkNode.Location.Shape.Intersects(polygon) && (x.PostalAddress.OrganisationName.Contains(searchText)
                                ||
                                x.PostalAddress.BuildingName.Contains(searchText)
                                || x.PostalAddress.SubBuildingName.Contains(searchText)
                                || SqlFunctions.StringConvert((double)x.PostalAddress.BuildingNumber).StartsWith(searchText)
                                || x.PostalAddress.Thoroughfare.Contains(searchText)
                                || x.PostalAddress.DependentLocality.Contains(searchText)))
                                .Select(l => new DeliveryPointDataDTO
                                {
                                    PostalAddress = new PostalAddressDataDTO
                                    {
                                        OrganisationName = l.PostalAddress.OrganisationName,
                                        BuildingName = l.PostalAddress.BuildingName,
                                        SubBuildingName = l.PostalAddress.SubBuildingName,
                                        BuildingNumber = l.PostalAddress.BuildingNumber,
                                        Thoroughfare = l.PostalAddress.Thoroughfare,
                                        DependentLocality = l.PostalAddress.DependentLocality,
                                        UDPRN = l.PostalAddress.UDPRN
                                    }
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
        public async Task<List<DeliveryPointDataDTO>> FetchDeliveryPointsForBasicSearch(string searchText, Guid unitGuid)
        {
            if (string.IsNullOrWhiteSpace(searchText) || Guid.Empty.Equals(unitGuid))
            {
                throw new ArgumentNullException(searchText, string.Format(ErrorConstants.Err_ArgumentmentNullException, string.Concat(searchText, unitGuid)));
            }

            int takeCount = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings[SearchResultCount]);
            searchText = searchText ?? string.Empty;

            DbGeometry polygon = DataContext.Locations.AsNoTracking().Where(x => x.ID == unitGuid)
                .Select(x => x.Shape).SingleOrDefault();

            var result = await DataContext.DeliveryPoints.AsNoTracking()
                .Include(l => l.PostalAddress)
                .Where(x => x.NetworkNode.Location.Shape.Intersects(polygon) && (x.PostalAddress.OrganisationName.Contains(searchText)
                                                                 || x.PostalAddress.BuildingName.Contains(searchText)
                                                                 || x.PostalAddress.SubBuildingName.Contains(searchText)
                                                                 || SqlFunctions.StringConvert((double)x.PostalAddress
                                                                     .BuildingNumber).StartsWith(searchText)
                                                                 || x.PostalAddress.Thoroughfare.Contains(searchText)
                                                                 || x.PostalAddress.DependentLocality.Contains(
                                                                     searchText)))
                .Select(l => new DeliveryPointDataDTO
                {
                    PostalAddress = new PostalAddressDataDTO
                    {
                        OrganisationName = l.PostalAddress.OrganisationName,
                        BuildingName = l.PostalAddress.BuildingName,
                        SubBuildingName = l.PostalAddress.SubBuildingName,
                        BuildingNumber = l.PostalAddress.BuildingNumber,
                        Thoroughfare = l.PostalAddress.Thoroughfare,
                        DependentLocality = l.PostalAddress.DependentLocality,
                        UDPRN = l.PostalAddress.UDPRN
                    }
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
                DbGeometry polygon = DataContext.Locations.AsNoTracking().Where(x => x.ID == unitGuid).Select(x => x.Shape).SingleOrDefault();

                var result = await DataContext.DeliveryPoints.AsNoTracking()
                  .Include(l => l.PostalAddress)
                  .Where(x => x.NetworkNode.Location.Shape.Intersects(polygon) && (x.PostalAddress.OrganisationName.Contains(searchText)
                                  || x.PostalAddress.BuildingName.Contains(searchText)
                                  || x.PostalAddress.SubBuildingName.Contains(searchText)
                                  || SqlFunctions.StringConvert((double)x.PostalAddress.BuildingNumber).StartsWith(searchText)
                                  || x.PostalAddress.Thoroughfare.Contains(searchText)
                                  || x.PostalAddress.DependentLocality.Contains(searchText)))
                  .CountAsync();

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
        public List<DeliveryPointDataDTO> GetDeliveryPoints(string boundingBoxCoordinates, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Data.GetDeliveryPoints"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                List<DeliveryPointDataDTO> deliveryPointDto = GetDeliveryPointsCoordinatesDatabyBoundingBox(boundingBoxCoordinates, unitGuid).ToList();

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
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
            //string methodName = MethodBase.GetCurrentMethod().Name;

            ////using (loggingHelper.RMTraceManager.StartTrace(LoggerTraceConstants.DataLayer + methodName))
            ////{
            //loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetRouteForDeliveryPointPriority, LoggerTraceConstants.GetRouteForDeliveryPointDataMethodEntryEventId, LoggerTraceConstants.Title);
            //string routeName = string.Empty;
            //var result = (from dp in DataContext.DeliveryPoints.AsNoTracking()
            //              join bs in DataContext.BlockSequences.AsNoTracking() on dp.ID equals bs.OperationalObject_GUID
            //              join b in DataContext.Blocks.AsNoTracking() on bs.Block_GUID equals b.ID
            //              join drb in DataContext.DeliveryRouteBlocks.AsNoTracking() on b.ID equals drb.Block_GUID
            //              join dr in DataContext.DeliveryRoutes.AsNoTracking() on drb.DeliveryRoute_GUID equals dr.ID
            //              join pa in DataContext.PostalAddresses.AsNoTracking() on dp.Address_GUID equals pa.ID
            //              where dp.ID == deliveryPointId && b.BlockType == UnSequenced
            //              select new
            //              {
            //                  RouteName = dr.RouteName,
            //                  RouteId = dr.ID,
            //                  PostcodeId = pa.PostCodeGUID
            //              }).SingleOrDefault();
            //if (result != null)
            //{
            //    var isPrimaryRoute = (from drp in DataContext.DeliveryRoutePostcodes.AsNoTracking() where drp.Postcode_GUID == result.PostcodeId && drp.DeliveryRoute_GUID == result.RouteId select drp.IsPrimaryRoute).ToList();
            //    if (isPrimaryRoute != null && isPrimaryRoute.Count > 0)
            //    {
            //        routeName = isPrimaryRoute[0] == true ? PRIMARYROUTE + result.RouteName.Trim() : SECONDARYROUTE + result.RouteName.Trim();
            //    }
            //    else
            //    {
            //        routeName = result.RouteName.Trim();
            //    }
            //}

            //loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetRouteForDeliveryPointPriority, LoggerTraceConstants.GetRouteForDeliveryPointDataMethodExitEventId, LoggerTraceConstants.Title);
            //return routeName;

            //// }

            return null;
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

            using (loggingHelper.RMTraceManager.StartTrace("Data.GetDPUse"))
            {
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetDPUsePriority, LoggerTraceConstants.GetDPUseDataMethodEntryEventId, LoggerTraceConstants.Title);
                string dpUsetype = string.Empty;

                var dpUse = from dp in DataContext.DeliveryPoints.AsNoTracking()
                            where dp.ID == deliveryPointId
                            select new { DeliveryPointUseIndicator_GUID = dp.DeliveryPointUseIndicatorGUID };

                List<Guid> deliveryPointUseIndicator = dpUse.Select(n => n.DeliveryPointUseIndicator_GUID).ToList();
                if (deliveryPointUseIndicator.Count > 0)
                {
                    if (deliveryPointUseIndicator[0] == operationalObjectTypeForDpOrganisation)
                    {
                        dpUsetype = DpUseOrganisation;
                    }
                    else if (deliveryPointUseIndicator[0] == operationalObjectTypeForDpResidential)
                    {
                        dpUsetype = DpUseResidential;
                    }
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetDPUsePriority, LoggerTraceConstants.GetDPUseDataMethodExitEventId, LoggerTraceConstants.Title);
                return dpUsetype;
            }
        }

        /// <summary>
        /// This method updates delivery point location using UDPRN
        /// </summary>
        /// <param name="deliveryPointDto">deliveryPointDto as DTO</param>
        /// <returns>updated delivery point</returns>
        public async Task<int> UpdateDeliveryPointLocationOnUDPRN(DeliveryPointDataDTO deliveryPointDto)
        {
            int status = 0;
            try
            {
                DeliveryPoint deliveryPoint =
                    DataContext.DeliveryPoints.SingleOrDefault(dp => dp.PostalAddress.UDPRN == deliveryPointDto.PostalAddress.UDPRN);

                if (deliveryPoint != null)
                {
                    deliveryPoint.NetworkNode.Location.Shape = deliveryPointDto.NetworkNode.Location.Shape;
                    deliveryPoint.NetworkNode.DataProviderGUID = deliveryPointDto.NetworkNode.DataProviderGUID;
                    status = await DataContext.SaveChangesAsync();
                }

                return status;
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
        public async Task<Guid> UpdateDeliveryPointLocationOnID(DeliveryPointDataDTO deliveryPointDto)
        {
            try
            {
                DeliveryPoint deliveryPoint =
                    DataContext.DeliveryPoints.SingleOrDefault(dp => dp.ID == deliveryPointDto.ID);

                if (deliveryPoint != null)
                {
                    deliveryPoint.NetworkNode.Location.Shape = deliveryPointDto.NetworkNode.Location.Shape;
                    deliveryPoint.NetworkNode.DataProviderGUID = deliveryPointDto.NetworkNode.DataProviderGUID;

                    DataContext.Entry(deliveryPoint).State = EntityState.Modified;
                    DataContext.Entry(deliveryPoint).OriginalValues[ROWVERSION] = deliveryPointDto.RowVersion;
                    await DataContext.SaveChangesAsync();
                }

                return deliveryPoint.ID;
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
        /// This method fetches delivery point data for given UDPRN
        /// </summary>
        /// <param name="udprn">udprn as int</param>
        /// <returns>deliveryPoint DTO</returns>
        public List<DeliveryPointDataDTO> GetDeliveryPointListByUDPRN(int udprn)
        {
            List<DeliveryPoint> deliveryPoints = DataContext.DeliveryPoints.Include(s => s.PostalAddress).Include(x => x.NetworkNode).Include(x => x.NetworkNode.Location)
.Where(dp => dp.PostalAddress.UDPRN == udprn).ToList();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DeliveryPoint, DeliveryPointDTO>();
                cfg.CreateMap<Location, LocationDataDTO>();
                cfg.CreateMap<NetworkNode, NetworkNodeDataDTO>();
                cfg.CreateMap<PostalAddress, PostalAddressDataDTO>();
            });

            Mapper.Configuration.CreateMapper();
            var deliveryPointDto = Mapper.Map<List<DeliveryPoint>, List<DeliveryPointDataDTO>>(deliveryPoints);

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
            //var deliveryPoints = (from dp in DataContext.DeliveryPoints.AsNoTracking()
            //                      join pa in DataContext.PostalAddresses.AsNoTracking() on dp.PostalAddressID equals pa.ID
            //                      join al in DataContext.NetworkNodes.AsNoTracking() on pa.UDPRN equals al.UDPRN
            //                       dp.pos.UDPRN == udprn
            //                      select new
            //                      {
            //                          DeliveryPoint = dp,
            //                          PostalAddress = pa,
            //                          AddressLocation = al,
            //                      }).SingleOrDefault();

            //Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<DeliveryPoint, DeliveryPointDTO>();
            //    cfg.CreateMap<PostalAddress, PostalAddressDTO>();
            //    cfg.CreateMap<AddressLocation, AddressLocationDTO>();
            //});

            //Mapper.Configuration.CreateMapper();

            //if (deliveryPoints != null)
            //{
            //    addDeliveryPointDto = new AddDeliveryPointDTO()
            //    {
            //        DeliveryPointDTO = Mapper.Map<DeliveryPoint, DeliveryPointDTO>(deliveryPoints.DeliveryPoint),
            //        AddressLocationDTO = Mapper.Map<AddressLocation, AddressLocationDTO>(deliveryPoints.AddressLocation),
            //        PostalAddressDTO = Mapper.Map<PostalAddress, PostalAddressDTO>(deliveryPoints.PostalAddress)
            //    };
            //}

            return addDeliveryPointDto;
        }

        /// <summary>
        /// Get the delivery points by the Postal Address Guid
        /// </summary>
        /// <param name = "addressId" > Postal Address Guid to find corresponding delivery point</param>
        /// <returns>DeliveryPointDTO object</returns>
        public DeliveryPointDataDTO GetDeliveryPointByPostalAddress(Guid addressId)
        {
            DeliveryPoint deliveryPoint = DataContext.DeliveryPoints.Include(s => s.PostalAddress).Include(x => x.NetworkNode).Include(x => x.NetworkNode.Location).AsNoTracking().Where(dp => dp.PostalAddressID == addressId)
                .SingleOrDefault();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DeliveryPoint, DeliveryPointDataDTO>();
                cfg.CreateMap<Location, LocationDataDTO>();
                cfg.CreateMap<NetworkNode, NetworkNodeDataDTO>();
                cfg.CreateMap<PostalAddress, PostalAddressDataDTO>();
            });

            Mapper.Configuration.CreateMapper();
            var deliveryPointDto = Mapper.Map<DeliveryPoint, DeliveryPointDataDTO>(deliveryPoint);
            return deliveryPointDto;
        }

        public async Task<DeliveryPointDataDTO> GetDeliveryPointByPostalAddressWithLocation(Guid addressId)
        {
            DeliveryPoint deliveryPoint = await (from dp in DataContext.DeliveryPoints
                                                 join nn in DataContext.NetworkNodes
                                                 on dp.ID equals nn.ID
                                                 join l in DataContext.Locations
                                                 on dp.ID equals l.ID
                                                 where dp.PostalAddressID == addressId
                                                 select dp).SingleOrDefaultAsync();

            if (deliveryPoint == null)
            {
                return null;
            }

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DeliveryPoint, DeliveryPointDataDTO>();
                cfg.CreateMap<Location, LocationDataDTO>();
                cfg.CreateMap<NetworkNode, NetworkNodeDataDTO>();
            });

            Mapper.Configuration.CreateMapper();

            DeliveryPointDataDTO deliveryPointDatabaseDTO = Mapper.Map<DeliveryPoint, DeliveryPointDataDTO>(deliveryPoint);

            return deliveryPointDatabaseDTO;
        }

        /// <summary>
        /// This method checks delivery point for given UDPRN exists or not
        /// </summary>
        /// <param name="udprn">uDPRN as int</param>
        /// <returns>boolean value true or false</returns>
        public async Task<bool> DeliveryPointExists(int udprn)
        {
            return await DataContext.DeliveryPoints.AsNoTracking().Where(dp => ((int)dp.PostalAddress.UDPRN).Equals(udprn)).AnyAsync();
        }

        /// <summary>
        /// Calculte distance between two points
        /// </summary>
        /// <param name="deliveryPointDTO">deliveryPoint DTO</param>
        /// <param name="newPoint">newPoint as DbGeometry</param>
        /// <returns>distance as double</returns>
        public double? GetDeliveryPointDistance(DeliveryPointDataDTO deliveryPointDTO, DbGeometry newPoint)
        {
            double? distance = 0;
            if (deliveryPointDTO != null)
            {
                distance = deliveryPointDTO.NetworkNode.Location.Shape.Distance(newPoint);
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

        public async Task<bool> DeleteDeliveryPoint(Guid id)
        {
            try
            {
                Location location = await DataContext.Locations.Include(l => l.NetworkNode).Include(l => l.NetworkNode.DeliveryPoint).Include(l => l.NetworkNode.DeliveryPoint.DeliveryPointStatus).Where(l => l.ID == id).SingleOrDefaultAsync();

                if (location != null)
                {
                    DataContext.DeliveryPointStatus.RemoveRange(location.NetworkNode.DeliveryPoint.DeliveryPointStatus);
                    DataContext.DeliveryPoints.Remove(location.NetworkNode.DeliveryPoint);
                    DataContext.NetworkNodes.Remove(location.NetworkNode);
                    DataContext.Locations.Remove(location);
                }

                await DataContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// This method is used to fetch Delivery Point by unique identifier.
        /// </summary>
        /// <param name="deliveryPointGuid">Delivery point unique identifier.</param>
        /// <returns>DeliveryPointDTO</returns>
        public DeliveryPointDataDTO GetDeliveryPoint(Guid deliveryPointGuid)
        {
            var objDeliveryPoint = DataContext.DeliveryPoints.Include(x => x.NetworkNode).Include(x => x.NetworkNode.Location).Include(x => x.PostalAddress).AsNoTracking().Single(n => n.ID == deliveryPointGuid);

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DeliveryPoint, DeliveryPointDataDTO>();
                cfg.CreateMap<Location, LocationDataDTO>();
                cfg.CreateMap<NetworkNode, NetworkNodeDataDTO>();
                cfg.CreateMap<PostalAddress, PostalAddressDataDTO>();
            });

            Mapper.Configuration.CreateMapper();

            return Mapper.Map<DeliveryPoint, DeliveryPointDataDTO>(objDeliveryPoint);
        }

        /// <summary> This method is used to get the delivery points crossing the operationalObject
        /// </summary> <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="accessLink">access link coordinate array</param> <returns>List<DeliveryPointDTO></returns>
        public List<DeliveryPointDataDTO> GetDeliveryPointsCrossingOperationalObject(string boundingBoxCoordinates, DbGeometry operationalObject)
        {
            List<DeliveryPointDataDTO> deliveryPointDTOs = new List<DeliveryPointDataDTO>();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DeliveryPoint, DeliveryPointDataDTO>();
                cfg.CreateMap<Location, LocationDataDTO>();
                cfg.CreateMap<NetworkNode, NetworkNodeDataDTO>();
                cfg.CreateMap<PostalAddress, PostalAddressDataDTO>();
            });

            Mapper.Configuration.CreateMapper();
            DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(boundingBoxCoordinates.ToString(), BNGCOORDINATESYSTEM);
            List<DeliveryPoint> crossingDeliveryPoints = DataContext.DeliveryPoints.AsNoTracking().Where(dp => dp.NetworkNode.Location.Shape != null && dp.NetworkNode.Location.Shape.Intersects(extent) && dp.NetworkNode.Location.Shape.Crosses(operationalObject)).ToList();
            List<DeliveryPointDataDTO> crossingAccessLinkDTOs = Mapper.Map<List<DeliveryPoint>, List<DeliveryPointDataDTO>>(crossingDeliveryPoints);
            deliveryPointDTOs.AddRange(crossingAccessLinkDTOs);
            List<DeliveryPoint> overLappingDeliveryPoints = DataContext.DeliveryPoints.AsNoTracking().Where(dp => dp.NetworkNode.Location.Shape != null && dp.NetworkNode.Location.Shape.Intersects(extent) && dp.NetworkNode.Location.Shape.Overlaps(operationalObject)).ToList();
            List<DeliveryPointDataDTO> overLappingAccessLinkDTOs = Mapper.Map<List<DeliveryPoint>, List<DeliveryPointDataDTO>>(overLappingDeliveryPoints);
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
        private List<DeliveryPointDataDTO> GetDeliveryPointsCoordinatesDatabyBoundingBox(string boundingBoxCoordinates, Guid unitGuid)
        {
            List<DeliveryPointDataDTO> deliveryPointDTOs = default(List<DeliveryPointDataDTO>);
            using (loggingHelper.RMTraceManager.StartTrace("Data.GetDeliveryPointsCoordinatesDatabyBoundingBox"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);
                DbGeometry polygon = default(DbGeometry);
                if (!string.IsNullOrEmpty(boundingBoxCoordinates))
                {
                    using (CommonLibrary.EntityFramework.Entities.RMDBContext rmDbContext = new CommonLibrary.EntityFramework.Entities.RMDBContext())
                    {
                        polygon = rmDbContext.UnitLocations.Where(u => u.ID == unitGuid).Select(x => x.UnitBoundryPolygon).SingleOrDefault();
                    }

                    DbGeometry extent = DbGeometry.FromText(boundingBoxCoordinates.ToString(), BNGCOORDINATESYSTEM);

                    var deliveryPoints = DataContext.DeliveryPoints.Include(x => x.PostalAddress)
                        .Include(x => x.NetworkNode)
                        .Include(x => x.NetworkNode.Location)
                        .Where(dp => dp.NetworkNode.Location.Shape.Intersects(extent) && dp.NetworkNode.Location.Shape.Intersects(polygon)).ToList();

                    Mapper.Initialize(cfg =>
                    {
                        cfg.CreateMap<DeliveryPoint, DeliveryPointDataDTO>();
                        cfg.CreateMap<NetworkNode, NetworkNodeDataDTO>();
                        cfg.CreateMap<Location, LocationDataDTO>();
                        cfg.CreateMap<PostalAddress, PostalAddressDataDTO>();
                    });

                    Mapper.Configuration.CreateMapper();

                    deliveryPointDTOs = Mapper.Map<List<DeliveryPoint>, List<DeliveryPointDataDTO>>(deliveryPoints);

                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                }

                return deliveryPointDTOs;
            }
        }

        #endregion Private Methods
    }
}