
namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Entities;
    using Fmo.MappingConfiguration;
    using System.Data.Entity;
    using System.IO;
    using Microsoft.SqlServer.Types;
    using System.Data.SqlTypes;
    using System.Text;
    using Newtonsoft.Json;

    public class RoadNameRepository : RepositoryBase<RoadName, FMODBContext>, IRoadNameRepository
    {

        public RoadNameRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public async Task<List<RoadNameDTO>> FetchRoadName()
        {
            try
            {
                var result = await DataContext.RoadNames.ToListAsync();
                return GenericMapper.MapList<RoadName, RoadNameDTO>(result.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<OSRoadLink> GetData(string query, params object[] parameters)
        {
            string x1 = Convert.ToString(parameters[0]);
            string y1 = Convert.ToString(parameters[1]);
            string x2 = Convert.ToString(parameters[2]);
            string y2 = Convert.ToString(parameters[3]);


            string coordinates = "POLYGON((" + x1 + " " + y1 + ", " + x1 + " " + y2 + ", " + x2 + " " + y2 + ", " + x2 + " " + y1 + ", " + x1 + " " + y1 + "))";

            System.Data.Entity.Spatial.DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(coordinates.ToString(), 27700);

            return DataContext.OSRoadLinks.Where(dp => dp.CentreLineGeometry != null && dp.CentreLineGeometry.Intersects(extent)).ToList();
        }

        public MemoryStream GetRoadRoutes(string[] parameters)
        {
            //GenericRepository gUoW = new GenericRepository();

            RoadName routeLinkFeatureCollections = new RoadName();
            List<OSRoadLink> osRoadLinkList = GetData(null, parameters).ToList();
List<Feature> lstFeatures = new List<Feature>();

            string json = string.Empty;

            foreach (var res in osRoadLinkList)
            {
               Geometry geometry = new Geometry();

                geometry.type = res.CentreLineGeometry.SpatialTypeName;

                var resultCoordinates = res.CentreLineGeometry;

                geometry.coordinates = new object();

                SqlGeometry sqlGeo = null;
                if (geometry.type == "LineString")
                {
                    sqlGeo = SqlGeometry.STLineFromWKB(new SqlBytes(resultCoordinates.AsBinary()), 27700).MakeValid();

                    List<double[]> cords = new List<double[]>();

                    for (int pt = 1; pt <= sqlGeo.STNumPoints().Value; pt++)
                    {
                        double[] coordinates = new double[] { sqlGeo.STPointN(pt).STX.Value, sqlGeo.STPointN(pt).STY.Value };
                        cords.Add(coordinates);
                    }

                    geometry.coordinates = cords;
                }
                else
                {
                    sqlGeo = SqlGeometry.STGeomFromWKB(new SqlBytes(resultCoordinates.AsBinary()), 27700).MakeValid();
                    double[] coordinates = new double[] { sqlGeo.STX.Value, sqlGeo.STY.Value };
                    geometry.coordinates = coordinates;
                }

             Feature features = new Feature();
                features.geometry = geometry;

                features.type = "Feature";
              //  features.properties = new Properties { type = "roadlink" };

                lstFeatures.Add(features);
            }

            //routeLinkFeatureCollections.features = lstFeatures;
            //routeLinkFeatureCollections.type = "FeatureCollection";
            json = JsonConvert.SerializeObject(routeLinkFeatureCollections,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            var resultBytes = Encoding.UTF8.GetBytes(json);
            return new MemoryStream(resultBytes);
        }

    }
}
