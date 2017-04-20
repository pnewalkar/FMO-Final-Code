namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Data.Entity.SqlServer;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Entities;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Entity = Fmo.Entities;

    /// <summary>
    /// This class contains methods used for fetching/Inserting Delivery Points data.
    /// </summary>
    public class DeliveryPointsRepository : RepositoryBase<Entity.DeliveryPoint, FMODBContext>, IDeliveryPointsRepository
    {
        public DeliveryPointsRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// This method is used to fetch Delivery Point by udprn.
        /// </summary>
        /// <param name="uDPRN">udprn as int</param>
        /// <returns>DeliveryPointDTO</returns>
        public DeliveryPointDTO GetDeliveryPointByUDPRN(int uDPRN)
        {
            try
            {
                var objDeliveryPoint = DataContext.DeliveryPoints.Where(n => n.UDPRN == uDPRN).SingleOrDefault();

                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<DeliveryPoint, DeliveryPointDTO>();
                    cfg.CreateMap<PostalAddress, PostalAddressDTO>().IgnoreAllUnmapped();
                });

                Mapper.Configuration.CreateMapper();

                return Mapper.Map<DeliveryPoint, DeliveryPointDTO>(objDeliveryPoint);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to insert delivery point.
        /// </summary>
        /// <param name="objDeliveryPoint"> Delivery point dto as object</param>
        /// <returns>bool</returns>
        public bool InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint)
        {
            bool saveFlag = false;
            try
            {
                if (objDeliveryPoint != null)
                {
                    DeliveryPoint newDeliveryPoint = new DeliveryPoint();
                    newDeliveryPoint.ID = objDeliveryPoint.ID;
                    newDeliveryPoint.Address_GUID = objDeliveryPoint.Address_GUID;
                    newDeliveryPoint.UDPRN = objDeliveryPoint.UDPRN;
                    newDeliveryPoint.Address_Id = objDeliveryPoint.Address_Id;
                    newDeliveryPoint.LocationXY = objDeliveryPoint.LocationXY;
                    newDeliveryPoint.Latitude = objDeliveryPoint.Latitude;
                    newDeliveryPoint.Longitude = objDeliveryPoint.Longitude;
                    DataContext.DeliveryPoints.Add(newDeliveryPoint);
                    DataContext.SaveChangesAsync();
                    saveFlag = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return saveFlag;
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
            DbGeometry polygon = DataContext.UnitLocations.Where(x => x.ID == unitGuid).Select(x => x.UnitBoundryPolygon).SingleOrDefault();
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
            int takeCount = Convert.ToInt32(ConfigurationManager.AppSettings["SearchResultCount"]);
            searchText = searchText ?? string.Empty;

            DbGeometry polygon = DataContext.UnitLocations.Where(x => x.ID == unitGuid).Select(x => x.UnitBoundryPolygon).SingleOrDefault();

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
            searchText = searchText ?? string.Empty;
            DbGeometry polygon = DataContext.UnitLocations.Where(x => x.ID == unitGuid).Select(x => x.UnitBoundryPolygon).SingleOrDefault();

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

        /// <summary>
        /// This method is used to Get delivery Point coordinates data.
        /// </summary>
        /// <param name="coordinates">coordinates as string</param>
        /// <returns>List of Delivery Point Entity</returns>
        public IEnumerable<DeliveryPoint> GetData(string coordinates)
        {
            DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(coordinates.ToString(), 27700);

            return DataContext.DeliveryPoints.Where(dp => dp.LocationXY.Intersects(extent));
        }

        /// <summary>
        /// This method is used to Get Delivery Points Dto as data.
        /// </summary>
        /// <param name="coordinates">coordinates as string</param>
        /// <returns>List of Delivery Point Dto</returns>
        public List<DeliveryPointDTO> GetDeliveryPoints(string coordinates)
        {
            List<DeliveryPoint> deliveryPoints = this.GetData(coordinates).ToList();

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
        /// Updates the delivery point location on udprn.
        /// </summary>
        /// <param name="deliveryPointDTO">The delivery point dto.</param>
        /// <returns>int</returns>
        public async Task<int> UpdateDeliveryPointLocationOnUDPRN(DeliveryPointDTO deliveryPointDTO)
        {
            try
            {
                DeliveryPoint deliveryPoint = DataContext.DeliveryPoints.Where(dp => ((int)dp.UDPRN) == deliveryPointDTO.UDPRN).SingleOrDefault();

                deliveryPoint.Longitude = deliveryPointDTO.Longitude;
                deliveryPoint.Latitude = deliveryPointDTO.Latitude;
                deliveryPoint.LocationXY = deliveryPointDTO.LocationXY;
                deliveryPoint.LocationProvider_GUID = deliveryPointDTO.LocationProvider_GUID;

                return await DataContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the delivery point list by udprn.
        /// </summary>
        /// <param name="udprn">The udprn.</param>
        /// <returns>List<DeliveryPointDTO></returns>
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
        /// Check if the delivery point exists for a given UDPRN id
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>
        /// boolean value
        /// </returns>
        public bool DeliveryPointExists(int uDPRN)
        {
            try
            {
                if (DataContext.DeliveryPoints.Where(dp => ((int)dp.UDPRN).Equals(uDPRN)).Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public double? GetDeliveryPointDistance(DeliveryPointDTO deliveryPointDTO, DbGeometry newPoint)
        {
            try
            {
                return deliveryPointDTO.LocationXY.Distance(newPoint);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Extention class for auto mapper TODO: to mappingConfiguration
    /// </summary>
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDest> IgnoreAllUnmapped<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }
    }
}