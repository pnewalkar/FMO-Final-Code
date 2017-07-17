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
    using System.Threading.Tasks;
    using AutoMapper;
    using CommonLibrary.ConfigurationMiddleware;
    using CommonLibrary.LoggingMiddleware;
    using Data.DeliveryPoint.WebAPI.DataDTO;
    using RM.CommonLibrary.ExceptionMiddleware;
    using RM.CommonLibrary.HelperMiddleware;
    using RM.Data.DeliveryPoint.WebAPI.Entities;
    using Utils;

    /// <summary>
    /// This class contains methods used for fetching/Inserting Delivery Points data.
    /// </summary>
    public class DeliveryPointsDataService : DataServiceBase<DeliveryPoint, DeliveryPointDBContext>, IDeliveryPointsDataService
    {
        private int priority = LoggerTraceConstants.DeliveryPointAPIPriority;
        private int entryEventId = LoggerTraceConstants.DeliveryPointDataServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.DeliveryPointDataServiceMethodExitEventId;

        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);

        public DeliveryPointsDataService(IDatabaseFactory<DeliveryPointDBContext> databaseFactory, ILoggingHelper loggingHelper, IConfigurationHelper configurationHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
            this.configurationHelper = configurationHelper;
        }

        #region Public Methods

        /// <summary>
        /// This method is used to fetch Delivery Point by udprn.
        /// </summary>
        /// <param name="udprn">udprn as int</param>
        /// <returns>The delivery point based on provided udprn.</returns>
        public async Task<DeliveryPointDataDTO> GetDeliveryPointByUDPRN(int udprn)
        {
            try
            {
                var objDeliveryPoint = await DataContext.DeliveryPoints
                    .AsNoTracking().Include(s => s.PostalAddress).Include(x => x.NetworkNode).Include(x => x.NetworkNode.Location).Where(n => n.PostalAddress.UDPRN == udprn).SingleOrDefaultAsync();

                ConfigureMapper();

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
        /// <returns>Unique identifier of delivery point.</returns>
        public async Task<Guid> InsertDeliveryPoint(DeliveryPointDataDTO objDeliveryPoint)
        {
            Guid deliveryPointId = Guid.Empty;
            DeliveryPoint deliveryPoint = new DeliveryPoint();
            DeliveryPointStatus deliveryPointStatus = new DeliveryPointStatus();
            NetworkNode networkNode = new NetworkNode();
            Location location = new Location();
            using (loggingHelper.RMTraceManager.StartTrace("Data.InsertDeliveryPoint"))
            {
                string methodName = typeof(DeliveryPointsDataService) + "." + nameof(InsertDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                if (objDeliveryPoint != null)
                {
                    try
                    {
                        objDeliveryPoint.ID = Guid.NewGuid();
                        deliveryPoint.ID = objDeliveryPoint.ID;
                        deliveryPoint.PostalAddressID = objDeliveryPoint.PostalAddressID;
                        deliveryPoint.DeliveryPointUseIndicatorGUID = objDeliveryPoint.DeliveryPointUseIndicatorGUID;

                        deliveryPointStatus.ID = Guid.NewGuid();
                        deliveryPointStatus.LocationID = objDeliveryPoint.ID;
                        deliveryPointStatus.DeliveryPointStatusGUID = objDeliveryPoint.DeliveryPointStatus.First().DeliveryPointStatusGUID;
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
                        deliveryPointId = deliveryPoint.ID;
                    }
                    catch (Exception dbUpdateException)
                    {
                        deliveryPointId = Guid.Empty;
                        DataContext.Entry(deliveryPoint).State = EntityState.Unchanged;
                        loggingHelper.Log(dbUpdateException, TraceEventType.Error);
                        throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("Delivery Point for addressId:", deliveryPoint.PostalAddressID)));
                    }
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return deliveryPointId;
            }
        }

        public async Task<bool> UpdatePAFIndicator(Guid addressGuid, Guid pafIndicator)
        {
            bool isDeliveryPointUpdated = false;
            DeliveryPoint deliveryPoint = new DeliveryPoint();
            using (loggingHelper.RMTraceManager.StartTrace("Data.UpdatePAFIndicator"))
            {
                string methodName = typeof(DeliveryPointsDataService) + "." + nameof(UpdatePAFIndicator);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                try
                {
                    deliveryPoint = DataContext.DeliveryPoints.Single(dp => dp.PostalAddressID == addressGuid);
                    deliveryPoint.DeliveryPointUseIndicatorGUID = pafIndicator;
                    await DataContext.SaveChangesAsync();
                    isDeliveryPointUpdated = true;
                }
                catch (Exception dbUpdateException)
                {
                    isDeliveryPointUpdated = false;
                    DataContext.Entry(deliveryPoint).State = EntityState.Unchanged;
                    loggingHelper.Log(dbUpdateException, TraceEventType.Error);
                    throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("Delivery Point for addressId:", addressGuid)));
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return isDeliveryPointUpdated;
            }
        }

        /// <summary>
        /// This method is used to fetch delivery points for advance search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>Task List of Delivery Point Dto</returns>
        public async Task<List<DeliveryPointDataDTO>> GetDeliveryPointsForAdvanceSearch(string searchText, Guid unitGuid)
        {
            if (searchText == null)
            {
                throw new ArgumentNullException(nameof(searchText), string.Format(ErrorConstants.Err_ArgumentmentNullException, searchText));
            }

            if (Guid.Empty.Equals(unitGuid))
            {
                throw new ArgumentNullException(nameof(unitGuid), string.Format(ErrorConstants.Err_ArgumentmentNullException, unitGuid));
            }

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
        public async Task<List<DeliveryPointDataDTO>> GetDeliveryPointsForBasicSearch(string searchText, Guid unitGuid)
        {
            if (searchText == null)
            {
                throw new ArgumentNullException(nameof(searchText), string.Format(ErrorConstants.Err_ArgumentmentNullException, searchText));
            }

            if (Guid.Empty.Equals(unitGuid))
            {
                throw new ArgumentNullException(nameof(unitGuid), string.Format(ErrorConstants.Err_ArgumentmentNullException, unitGuid));
            }

            int takeCount = Convert.ToInt32(configurationHelper.ReadAppSettingsConfigurationValues(DeliveryPointConstants.SearchResultCount));

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
                if (searchText == null)
                {
                    throw new ArgumentNullException(nameof(searchText), string.Format(ErrorConstants.Err_ArgumentmentNullException, searchText));
                }

                if (Guid.Empty.Equals(unitGuid))
                {
                    throw new ArgumentNullException(nameof(unitGuid), string.Format(ErrorConstants.Err_ArgumentmentNullException, unitGuid));
                }

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
                string methodName = typeof(DeliveryPointsDataService) + "." + nameof(GetDeliveryPoints);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<DeliveryPointDataDTO> deliveryPointDto = GetDeliveryPointsCoordinatesDatabyBoundingBox(boundingBoxCoordinates, unitGuid).ToList();

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return deliveryPointDto;
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
                    DataContext.Entry(deliveryPoint).OriginalValues[DeliveryPointConstants.ROWVERSION] = deliveryPointDto.RowVersion;
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

            ConfigureMapper();

            var deliveryPointDto = Mapper.Map<List<DeliveryPoint>, List<DeliveryPointDataDTO>>(deliveryPoints);

            return deliveryPointDto;
        }

        /// <summary>
        /// This method fetches delivery point data for given UDPRN
        /// </summary>
        /// <param name="udprn">udprn as int</param>
        /// <returns>addDeliveryPointDto</returns>
        public RM.Data.DeliveryPoint.WebAPI.DTO.AddDeliveryPointDTO GetDetailDeliveryPointByUDPRN(int udprn)
        {
            RM.Data.DeliveryPoint.WebAPI.DTO.AddDeliveryPointDTO addDeliveryPointDto = default(RM.Data.DeliveryPoint.WebAPI.DTO.AddDeliveryPointDTO);
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

            ConfigureMapper();

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

            ConfigureMapper();

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

            ConfigureMapper();

            return Mapper.Map<DeliveryPoint, DeliveryPointDataDTO>(objDeliveryPoint);
        }

        /// <summary> This method is used to get the delivery points crossing the operationalObject
        /// </summary> <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="accessLink">access link coordinate array</param> <returns>List<DeliveryPointDTO></returns>
        public List<DeliveryPointDataDTO> GetDeliveryPointsCrossingOperationalObject(string boundingBoxCoordinates, DbGeometry operationalObject)
        {
            List<DeliveryPointDataDTO> deliveryPointDTOs = new List<DeliveryPointDataDTO>();

            ConfigureMapper();

            DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(boundingBoxCoordinates.ToString(), DeliveryPointConstants.BNGCOORDINATESYSTEM);
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
                string methodName = typeof(DeliveryPointsDataService) + "." + nameof(GetDeliveryPointsCoordinatesDatabyBoundingBox);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                DbGeometry polygon = default(DbGeometry);
                if (!string.IsNullOrEmpty(boundingBoxCoordinates))
                {
                    polygon = DataContext.Locations.Where(u => u.ID == unitGuid).Select(x => x.Shape).SingleOrDefault();

                    DbGeometry extent = DbGeometry.FromText(boundingBoxCoordinates.ToString(), DeliveryPointConstants.BNGCOORDINATESYSTEM);

                    var deliveryPoints = DataContext.DeliveryPoints.Include(x => x.PostalAddress)
                        .Include(x => x.NetworkNode)
                        .Include(x => x.NetworkNode.Location)
                        .Where(dp => dp.NetworkNode.Location.Shape.Intersects(extent) && dp.NetworkNode.Location.Shape.Intersects(polygon)).ToList();

                    ConfigureMapper();

                    deliveryPointDTOs = Mapper.Map<List<DeliveryPoint>, List<DeliveryPointDataDTO>>(deliveryPoints);
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return deliveryPointDTOs;
            }
        }

        private static void ConfigureMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DeliveryPoint, DeliveryPointDataDTO>();
                cfg.CreateMap<NetworkNode, NetworkNodeDataDTO>();
                cfg.CreateMap<Location, LocationDataDTO>();
                cfg.CreateMap<PostalAddress, PostalAddressDataDTO>();
                cfg.CreateMap<DeliveryPointStatus, DeliveryPointStatusDataDTO>();
                cfg.CreateMap<SupportingDeliveryPoint, SupportingDeliveryPointDataDTO>();
            });

            Mapper.Configuration.CreateMapper();
        }

        #endregion Private Methods
    }
}