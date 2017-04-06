namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.SqlServer;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Entities;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using MappingConfiguration;
    using Entity = Fmo.Entities;


    using System.Linq;
    using System.Data.SqlTypes;
    using Microsoft.SqlServer.Types;
    using Newtonsoft.Json.Linq;
    using System.Text;
    using System.Data.Entity.Spatial;
    using Common.Constants;

    public class DeliveryPointsRepository : RepositoryBase<Entity.DeliveryPoint, FMODBContext>, IDeliveryPointsRepository
    {
        public DeliveryPointsRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public DeliveryPointDTO GetDeliveryPointByUDPRN(int uDPRN)
        {
            try
            {
                DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO();
                DeliveryPoint objDeliveryPoint = DataContext.DeliveryPoints.Where(n => n.UDPRN == uDPRN).SingleOrDefault();
                return deliveryPointDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint)
        {
            bool saveFlag = false;
            try
            {
                if (objDeliveryPoint != null)
                {
                    var deliveryPoint = new DeliveryPoint();
                    GenericMapper.Map(objDeliveryPoint, deliveryPoint);

                    DataContext.DeliveryPoints.Add(deliveryPoint);
                    DataContext.SaveChangesAsync();
                    saveFlag = true;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return saveFlag;
        }

        public IEnumerable<DeliveryPoint> GetData(string query, params object[] parameters)
        {
            string x1 = Convert.ToString(parameters[0]);
            string y1 = Convert.ToString(parameters[1]);
            string x2 = Convert.ToString(parameters[2]);
            string y2 = Convert.ToString(parameters[3]);

            string coordinates = "POLYGON((" + x1 + " " + y1 + ", " + x1 + " " + y2 + ", " + x2 + " " + y2 + ", " + x2 + " " + y1 + ", " + x1 + " " + y1 + "))";

            System.Data.Entity.Spatial.DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(coordinates.ToString(), 27700);

            return DataContext.DeliveryPoints.Where(dp => dp.LocationXY.Intersects(extent));
        }

        public MemoryStream GetDeliveryPoints()
        {
            string json;

            using (StreamReader r = new StreamReader(@"D:\Richa\FMO-AD\FMO\Fmo.DataServices\TestData\deliveryPoint.json"))
            {
                json = r.ReadToEnd();
            }

            var resultBytes = Encoding.UTF8.GetBytes(json);
            return new MemoryStream(resultBytes);

            //GenericRepository gUoW = new GenericRepository();

            /*List<DeliveryPoint> result = GetData(null, parameters).ToList();

            //gUoW.DpRep.GetData().ToList<OpenLayersWebAPI.ViewModels.deliveryPoint>();

            var geoJson = new GeoJson
            {
                features = new List<Feature>()
            };

            foreach (var point in result)
            {
                SqlGeometry sqlGeo = SqlGeometry.STGeomFromWKB(new SqlBytes(point.LocationXY.AsBinary()), 0);

                var feature = new Feature
                {
                    id = point.DeliveryPoint_Id,
                    properties = new Dictionary<string, JToken>
                    {
                        { "name", point.PostalAddress.BuildingName },
                        { "number", point.PostalAddress.BuildingNumber },
                        { "postcode", point.PostalAddress.Postcode },
                        { "street_name", point.PostalAddress.BuildingName },
                        { "type", "deliverypoint" }
                    },
                    geometry = new Geometry
                    {
                        coordinates = new Coordinates(sqlGeo)
                    }
                };
                geoJson.features.Add(feature);
            }

            var resultBytes = System.Text.Encoding.UTF8.GetBytes(geoJson.getJson().ToString());

            return new MemoryStream(resultBytes);*/
        }

        public async Task<List<DeliveryPointDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText)
        {
            var result = await DataContext.DeliveryPoints.Where(x => x.PostalAddress.OrganisationName.Contains(searchText)
                                || x.PostalAddress.BuildingName.Contains(searchText)
                                || x.PostalAddress.SubBuildingName.Contains(searchText)
                                || SqlFunctions.StringConvert((double)x.PostalAddress.BuildingNumber).StartsWith(searchText)
                                || x.PostalAddress.Thoroughfare.Contains(searchText)
                                || x.PostalAddress.DependentLocality.Contains(searchText)).ToListAsync();

            return GenericMapper.MapList<DeliveryPoint, DeliveryPointDTO>(result);
        }

        public async Task<List<DeliveryPointDTO>> FetchDeliveryPointsForBasicSearch(string searchText)
        {
            int takeCount = 5;
            var result = await DataContext.DeliveryPoints.Where(x => x.PostalAddress.OrganisationName.Contains(searchText)
                                || x.PostalAddress.BuildingName.Contains(searchText)
                                || x.PostalAddress.SubBuildingName.Contains(searchText)
                                || SqlFunctions.StringConvert((double)x.PostalAddress.BuildingNumber).StartsWith(searchText)
                                || x.PostalAddress.Thoroughfare.Contains(searchText)
                                || x.PostalAddress.DependentLocality.Contains(searchText))
                .Take(takeCount)
                .ToListAsync();

            return GenericMapper.MapList<DeliveryPoint, DeliveryPointDTO>(result);
        }

        public async Task<int> GetDeliveryPointsCount(string searchText)
        {
            return await DataContext.DeliveryPoints.CountAsync(x => x.PostalAddress.OrganisationName.Contains(searchText)
                              || x.PostalAddress.BuildingName.Contains(searchText)
                              || x.PostalAddress.SubBuildingName.Contains(searchText)
                              || SqlFunctions.StringConvert((double)x.PostalAddress.BuildingNumber).StartsWith(searchText)
                              || x.PostalAddress.Thoroughfare.Contains(searchText)
                              || x.PostalAddress.DependentLocality.Contains(searchText));
        }
        public async Task<int> UpdateDeliveryPointLocationOnUDPRN(int uDPRN, decimal latitude, decimal longitude, DbGeometry locationXY)
        {
            try
            {
                DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO();

                DeliveryPoint deliveryPoint = DataContext.DeliveryPoints.Where(dp => ((int)dp.UDPRN).Equals(uDPRN)).SingleOrDefault();

                GenericMapper.Map(deliveryPoint, deliveryPointDTO);

                deliveryPointDTO.Longitude = longitude;
                deliveryPointDTO.Latitude = latitude;
                deliveryPointDTO.LocationXY = locationXY;
                deliveryPointDTO.LocationProvider = Constants.USR_LOC_PROVIDER;

                GenericMapper.Map(deliveryPointDTO, deliveryPoint);

                return await DataContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}