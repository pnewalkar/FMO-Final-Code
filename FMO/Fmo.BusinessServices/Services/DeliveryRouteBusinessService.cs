using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Helpers;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;

namespace Fmo.BusinessServices.Services
{
    public class DeliveryRouteBusinessService : IDeliveryRouteBusinessService
    {
        private IDeliveryRouteRepository deliveryRouteRepository;
        private IReferenceDataCategoryRepository referenceDataCategoryRepository;
        private IScenarioRepository scenarioRepository;
        private IDeliveryUnitLocationRepository deliveryUnitLocationRespository;

        public DeliveryRouteBusinessService(IDeliveryRouteRepository deliveryRouteRepository, IReferenceDataCategoryRepository referenceDataCategoryRepository, IScenarioRepository scenarioRepository, IDeliveryUnitLocationRepository deliveryUnitLocationRespository)
        {
            this.deliveryRouteRepository = deliveryRouteRepository;
            this.referenceDataCategoryRepository = referenceDataCategoryRepository;
            this.scenarioRepository = scenarioRepository;
            this.deliveryUnitLocationRespository = deliveryUnitLocationRespository;
        }

        public List<DeliveryRouteDTO> FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID)
        {
            return deliveryRouteRepository.FetchDeliveryRoute(operationStateID, deliveryScenarioID);
        }

        public List<ReferenceDataDTO> FetchRouteLogStatus()
        {
            return referenceDataCategoryRepository.RouteLogStatus();
        }

        public List<ScenarioDTO> FetchDeliveryScenario(Guid operationStateID, Guid deliveryScenarioID)
        {
            return scenarioRepository.FetchScenario(operationStateID, deliveryScenarioID);
        }

        public async Task<List<DeliveryRouteDTO>> FetchDeliveryRouteforBasicSearch(string searchText)
        {
            return await deliveryRouteRepository.FetchDeliveryRouteForBasicSearch(searchText);
        }

        public List<DeliveryUnitLocationDTO> FetchDeliveryUnit()
        {
            var deliveryUnitLocationDTOList = deliveryUnitLocationRespository.FetchDeliveryUnit();

            foreach (var deliveryUnitLocationDTO in deliveryUnitLocationDTOList)
            {
                // take the unit boundry plus 1 mile envelope
                var unitBoundary = SqlGeometry.STPolyFromWKB(new SqlBytes(deliveryUnitLocationDTO.UnitBoundryPolygon.Envelope.Buffer(1609.34).Envelope.AsBinary()), 27700).MakeValid();

                deliveryUnitLocationDTO.BoundingBoxCenter = unitBoundary.STCentroid().STPointN(1).STX + "," + unitBoundary.STCentroid().STPointN(1).STY;

                deliveryUnitLocationDTO.BoundingBox = unitBoundary.STPointN(1).STX.Value + "," + unitBoundary.STPointN(1).STY.Value + "," + unitBoundary.STPointN(3).STX.Value + "," + unitBoundary.STPointN(3).STY.Value;

                deliveryUnitLocationDTO.UnitBoundaryGeoJSONData = GeUnitBoundaryJsonData(deliveryUnitLocationDTO);
            }

            return deliveryUnitLocationDTOList;
        }

        private string GeUnitBoundaryJsonData(DeliveryUnitLocationDTO deliveryUnitLocationDTO)
        {
            string jsonData = string.Empty;
            if (deliveryUnitLocationDTO != null)
            {
                var geoJson = new GeoJson
                {
                    features = new List<Feature>()
                };

                SqlGeometry sqlGeo = null;

                Geometry geometry = new Geometry();

                geometry.type = OpenGisGeometryType.Polygon.ToString();

                var resultCoordinates = deliveryUnitLocationDTO.UnitBoundryPolygon;

                geometry.coordinates = new object();

                if (deliveryUnitLocationDTO.UnitBoundryPolygon.SpatialTypeName == OpenGisGeometryType.Polygon.ToString())
                {
                    sqlGeo = SqlGeometry.STPolyFromWKB(new SqlBytes(resultCoordinates.AsBinary()), 27700).MakeValid();
                    List<List<double[]>> listCords = new List<List<double[]>>();
                    List<double[]> cords = new List<double[]>();

                    for (int pt = 1; pt <= sqlGeo.STNumPoints().Value; pt++)
                    {
                        double[] coordinates = new double[] { sqlGeo.STPointN(pt).STX.Value, sqlGeo.STPointN(pt).STY.Value };
                        cords.Add(coordinates);
                    }

                    listCords.Add(cords);

                    geometry.coordinates = listCords;
                }
                else if (deliveryUnitLocationDTO.UnitBoundryPolygon.SpatialTypeName == OpenGisGeometryType.MultiPolygon.ToString())
                {
                    sqlGeo = SqlGeometry.STMPolyFromWKB(new SqlBytes(resultCoordinates.AsBinary()), 27700).MakeValid();
                    List<List<List<double[]>>> listCords = new List<List<List<double[]>>>();

                    List<List<double[]>> cords = new List<List<double[]>>();
                    for (int i = 1; i <= sqlGeo.STNumGeometries(); i++)
                    {
                        List<double[]> cordsPolygon = new List<double[]>();
                        for (int pt = 1; pt <= sqlGeo.STGeometryN(i).STNumPoints().Value; pt++)
                        {
                            double[] coordinates = new double[] { sqlGeo.STGeometryN(i).STPointN(pt).STX.Value, sqlGeo.STGeometryN(i).STPointN(pt).STY.Value };
                            cordsPolygon.Add(coordinates);
                        }

                        cords.Add(cordsPolygon);
                    }

                    listCords.Add(cords);

                    geometry.coordinates = listCords;
                }

                var feature = new Feature
                {
                    id = deliveryUnitLocationDTO.DeliveryUnit_Id,
                    geometry = geometry
                };

                geoJson.features.Add(feature);

                jsonData = JsonConvert.SerializeObject(geoJson);
            }

            return jsonData;
        }

        public Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}