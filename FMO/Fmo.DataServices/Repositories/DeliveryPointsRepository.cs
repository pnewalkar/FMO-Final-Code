using System.Collections;
using Fmo.Common.ExceptionManagement;
using Fmo.Common.ResourceFile;

namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Spatial;
    using System.Data.Entity.SqlServer;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Common.Constants;
    using Common.Interface;
    using Entities;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using MappingConfiguration;
    using MappingExtensions;
    using Entity = Fmo.Entities;

    /// <summary>
    /// This class contains methods used for fetching/Inserting Delivery Points data.
    /// </summary>
    public class DeliveryPointsRepository : RepositoryBase<Entity.DeliveryPoint, FMODBContext>, IDeliveryPointsRepository
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public DeliveryPointsRepository(IDatabaseFactory<FMODBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// This method is used to fetch Delivery Point by udprn.
        /// </summary>
        /// <param name="udprn">udprn as int</param>
        /// <returns>DeliveryPointDTO</returns>
        public DeliveryPointDTO GetDeliveryPointByUDPRN(int udprn)
        {
            try
            {
                var objDeliveryPoint = DataContext.DeliveryPoints.AsNoTracking().Where(n => n.UDPRN == udprn).SingleOrDefault();

                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<DeliveryPoint, DeliveryPointDTO>();
                    cfg.CreateMap<PostalAddress, PostalAddressDTO>().IgnoreAllUnmapped();
                });

                Mapper.Configuration.CreateMapper();

                return Mapper.Map<DeliveryPoint, DeliveryPointDTO>(objDeliveryPoint);
            }
            catch (Exception ex)
            {
                this.loggingHelper.LogError(ex);
                return null;
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

                this.loggingHelper.LogError(ex);
            }

            return isDeliveryPointUpdated;
        }

        /// <summary>
        /// This method is used to insert delivery point.
        /// </summary>
        /// <param name="objDeliveryPoint"> Delivery point dto as object</param>
        /// <returns>bool</returns>
        public bool InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint)
        {
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
                    DataContext.SaveChanges();
                    isDeliveryPointInserted = true;
                }
                catch (Exception ex)
                {
                    isDeliveryPointInserted = false;
                    if (objDeliveryPoint != null)
                    {
                        DataContext.Entry(newDeliveryPoint).State = EntityState.Unchanged;
                    }

                    this.loggingHelper.LogError(ex);
                }
            }

            return isDeliveryPointInserted;
        }

        /// <summary>
        /// This method is used to fetch delivery points for advance search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// Task List of Delivery Point Dto
        /// </returns>
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
        /// <returns>
        /// The result set of delivery point.
        /// </returns>
        public async Task<List<DeliveryPointDTO>> FetchDeliveryPointsForBasicSearch(string searchText, Guid unitGuid)
        {
            if (string.IsNullOrWhiteSpace(searchText) || Guid.Empty.Equals(unitGuid))
            {
                throw new ArgumentNullException(searchText, string.Format(ErrorMessageIds.Err_ArgumentmentNullException, string.Concat(searchText, unitGuid)));
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
                                                                 || SqlFunctions.StringConvert((double) x.PostalAddress
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
        /// <returns>
        /// The total count of delivery points
        /// </returns>
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
                throw new SystemException(ErrorMessageIds.Err_InvalidOperationExceptionForSingleorDefault, ex);
            }
            catch (OverflowException overflow)
            {
                throw new SystemException(ErrorMessageIds.Err_OverflowException, overflow);
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

            return deliveryPointDto;
        }

        /// <summary>
        /// This Method provides Route Name for a single DeliveryPoint
        /// </summary>
        /// <param name="deliveryPointId">deliveryPointId as GUID</param>
        /// <returns>Route Name for a single DeliveryPoint</returns>
        public string GetRouteForDeliveryPoint(Guid deliveryPointId)
        {
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

            return routeName;
        }

        /// <summary>
        /// This Method fetches DPUse value for the DeliveryPoint
        /// </summary>
        /// <param name="referenceDataCategoryDtoList"> referenceDataCategoryDtoList as List of ReferenceDataCategoryDTO </param>
        /// <param name="deliveryPointId">deliveryPointId as GUID </param>
        /// <returns>DPUse value as string</returns>
        public string GetDPUse(List<ReferenceDataCategoryDTO> referenceDataCategoryDtoList, Guid deliveryPointId)
        {
            string dpUsetype = string.Empty;
            Guid operationalObjectTypeForDpOrganisation = referenceDataCategoryDtoList
               .Where(x => x.CategoryName == ReferenceDataCategoryNames.DeliveryPointUseIndicator)
               .SelectMany(x => x.ReferenceDatas)
               .Where(x => x.ReferenceDataValue == ReferenceDataValues.Organisation).Select(x => x.ID)
               .SingleOrDefault();

            Guid operationalObjectTypeForDpResidential = referenceDataCategoryDtoList
                .Where(x => x.CategoryName == ReferenceDataCategoryNames.DeliveryPointUseIndicator)
                .SelectMany(x => x.ReferenceDatas)
                .Where(x => x.ReferenceDataValue == ReferenceDataValues.Residential).Select(x => x.ID)
                .SingleOrDefault();

            var dpUse = from dp in DataContext.DeliveryPoints.AsNoTracking()
                        where dp.ID == deliveryPointId
                        select new { DeliveryPointUseIndicator_GUID = dp.DeliveryPointUseIndicator_GUID };

            List<Guid> deliveryPointUseIndicator = dpUse.Select(n => n.DeliveryPointUseIndicator_GUID).ToList();
            if (deliveryPointUseIndicator[0] == operationalObjectTypeForDpOrganisation)
            {
                dpUsetype = Constants.DpUseOrganisation;
            }
            else if (deliveryPointUseIndicator[0] == operationalObjectTypeForDpResidential)
            {
                dpUsetype = Constants.DpUseResidential;
            }

            return dpUsetype;
        }

        /// <summary>
        /// This method updates delivery point location using UDPRN
        /// </summary>
        /// <param name="deliveryPointDto">deliveryPointDto as DTO</param>
        /// <returns>updated delivery point</returns>
        public async Task<Guid> UpdateDeliveryPointLocationOnUDPRN(DeliveryPointDTO deliveryPointDto)
        {
            try
            {
                using (FMODBContext fmoDBContext = new FMODBContext())
                {
                    DeliveryPoint deliveryPoint =
                        fmoDBContext.DeliveryPoints.SingleOrDefault(dp => dp.ID == deliveryPointDto.ID);

                    if (deliveryPoint != null)
                    {
                        deliveryPoint.Longitude = deliveryPointDto.Longitude;
                        deliveryPoint.Latitude = deliveryPointDto.Latitude;
                        deliveryPoint.LocationXY = deliveryPointDto.LocationXY;
                        deliveryPoint.LocationProvider_GUID = deliveryPointDto.LocationProvider_GUID;
                        deliveryPoint.Positioned = deliveryPointDto.Positioned;

                        fmoDBContext.Entry(deliveryPoint).State = EntityState.Modified;
                        fmoDBContext.Entry(deliveryPoint).OriginalValues[Constants.ROWVERSION] = deliveryPointDto.RowVersion;
                        await fmoDBContext.SaveChangesAsync();
                    }

                    return deliveryPoint.ID;
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbConcurrencyException(ErrorMessageIds.Err_Concurrency);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new DataAccessException(dbUpdateException, string.Format(ErrorMessageIds.Err_SqlUpdateException, string.Concat("delivery point location for:", deliveryPointDto.ID)));
            }
            catch (NotSupportedException notSupportedException)
            {
                notSupportedException.Data.Add("userFriendlyMessage", ErrorMessageIds.Err_Default);
                throw new InfrastructureException(notSupportedException, ErrorMessageIds.Err_NotSupportedException);
            }
            catch (ObjectDisposedException disposedException)
            {
                disposedException.Data.Add("userFriendlyMessage", ErrorMessageIds.Err_Default);
                throw new ServiceException(disposedException, ErrorMessageIds.Err_ObjectDisposedException);
            }
        }

        /// <summary>
        /// This method fetches delivery point data for given UDPRN
        /// </summary>
        /// <param name="udprn">udprn as int</param>
        /// <returns>deliveryPoint DTO</returns>
        public List<DeliveryPointDTO> GetDeliveryPointListByUDPRN(int udprn)
        {
            List<DeliveryPoint> deliveryPoints = DataContext.DeliveryPoints.Where(dp => dp.UDPRN == udprn).ToList();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DeliveryPoint, DeliveryPointDTO>();
                cfg.CreateMap<PostalAddress, PostalAddressDTO>().IgnoreAllUnmapped();
            });

            Mapper.Configuration.CreateMapper();
            var deliveryPointDto = Mapper.Map<List<DeliveryPoint>, List<DeliveryPointDTO>>(deliveryPoints);

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
            DeliveryPoint deliveryPoint = DataContext.DeliveryPoints.Where(dp => dp.Address_GUID == addressId)
                .SingleOrDefault();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DeliveryPoint, DeliveryPointDTO>();
                cfg.CreateMap<PostalAddress, PostalAddressDTO>().IgnoreAllUnmapped();
            });

            Mapper.Configuration.CreateMapper();
            var deliveryPointDto = Mapper.Map<DeliveryPoint, DeliveryPointDTO>(deliveryPoint);
            return deliveryPointDto;
        }

        /// <summary>
        /// This method checks delivery point for given UDPRN exists or not
        /// </summary>
        /// <param name="udprn">uDPRN as int</param>
        /// <returns>boolean value true or false</returns>
        public bool DeliveryPointExists(int udprn)
        {
            if (DataContext.DeliveryPoints.AsNoTracking().Where(dp => ((int) dp.UDPRN).Equals(udprn)).Any())
            {
                return true;
            }

            return false;
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
        /// This method is used to Get delivery Point boundingBox data.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">unit unique identifier.</param>
        /// <returns>List of Delivery Point Entity</returns>
        private IEnumerable<DeliveryPoint> GetDeliveryPointsCoordinatesDatabyBoundingBox(string boundingBoxCoordinates, Guid unitGuid)
        {
            IEnumerable<DeliveryPoint> deliveryPoints = default(IEnumerable<DeliveryPoint>);
            if (!string.IsNullOrEmpty(boundingBoxCoordinates))
            {
                DbGeometry polygon = DataContext.UnitLocations.AsNoTracking().Where(x => x.ID == unitGuid).Select(x => x.UnitBoundryPolygon).SingleOrDefault();

                DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(boundingBoxCoordinates.ToString(), Constants.BNGCOORDINATESYSTEM);

                deliveryPoints = DataContext.DeliveryPoints.AsNoTracking().Where(dp => dp.LocationXY.Intersects(extent) && dp.LocationXY.Intersects(polygon));
            }

            return deliveryPoints;
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
        public void UpdateDeliveryPointAccessLinkCreationStatus(DeliveryPointDTO deliveryPointDTO)
        {
            using (FMODBContext fmoDbContext = new FMODBContext())
            {
                DeliveryPoint deliveryPoint = fmoDbContext.DeliveryPoints.SingleOrDefault(dp => dp.ID == deliveryPointDTO.ID);

                if (deliveryPoint != null)
                {
                    deliveryPoint.AccessLinkPresent = deliveryPointDTO.AccessLinkPresent;

                    fmoDbContext.Entry(deliveryPoint).State = EntityState.Modified;
                    fmoDbContext.Entry(deliveryPoint).OriginalValues[Constants.ROWVERSION] = deliveryPointDTO.RowVersion;
                    fmoDbContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// This method is used to get the delivery points crossing the created access link
        /// </summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param>
        /// <param name="accessLink">access link coordinate array</param>
        /// <returns>List<DeliveryPointDTO></returns>
        public List<DeliveryPointDTO> GetDeliveryPointsCrossingManualAccessLink(string boundingBoxCoordinates, DbGeometry accessLink)
        {
            List<DeliveryPointDTO> deliveryPointDTOs = new List<DeliveryPointDTO>();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<PostalAddress, PostalAddressDTO>();
                cfg.CreateMap<DeliveryPoint, DeliveryPointDTO>();
            });

            Mapper.Configuration.CreateMapper();
            DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(boundingBoxCoordinates.ToString(), Constants.BNGCOORDINATESYSTEM);
            List<DeliveryPoint> crossingDeliveryPoints = DataContext.DeliveryPoints.Where(dp => dp.LocationXY != null && dp.LocationXY.Intersects(extent) && dp.LocationXY.Crosses(accessLink)).ToList();
            List<DeliveryPointDTO> crossingAccessLinkDTOs = Mapper.Map<List<DeliveryPoint>, List<DeliveryPointDTO>>(crossingDeliveryPoints);
            deliveryPointDTOs.AddRange(crossingAccessLinkDTOs);
            List<DeliveryPoint> overLappingDeliveryPoints = DataContext.DeliveryPoints.Where(dp => dp.LocationXY != null && dp.LocationXY.Intersects(extent) && dp.LocationXY.Overlaps(accessLink)).ToList();
            List<DeliveryPointDTO> overLappingAccessLinkDTOs = Mapper.Map<List<DeliveryPoint>, List<DeliveryPointDTO>>(overLappingDeliveryPoints);
            deliveryPointDTOs.AddRange(overLappingAccessLinkDTOs);
            return deliveryPointDTOs;
        }
    }
}