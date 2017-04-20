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

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryRouteBusinessService"/> class and other classes.
        /// </summary>
        /// <param name="deliveryRouteRepository">IDeliveryRouteRepository reference</param>
        /// <param name="referenceDataCategoryRepository">IReferenceDataCategoryRepository reference</param>
        /// <param name="scenarioRepository">IScenarioRepository reference</param>
        /// <param name="deliveryUnitLocationRespository">IDeliveryUnitLocationRepository reference</param>
        public DeliveryRouteBusinessService(IDeliveryRouteRepository deliveryRouteRepository, IReferenceDataCategoryRepository referenceDataCategoryRepository, IScenarioRepository scenarioRepository, IDeliveryUnitLocationRepository deliveryUnitLocationRespository)
        {
            this.deliveryRouteRepository = deliveryRouteRepository;
            this.referenceDataCategoryRepository = referenceDataCategoryRepository;
            this.scenarioRepository = scenarioRepository;
            this.deliveryUnitLocationRespository = deliveryUnitLocationRespository;
        }

        /// <summary>
        /// Fetch the Delivery Route by passing operationStateID and deliveryScenarioID.
        /// </summary>
        /// <param name="operationStateID">The operationstate id.</param>
        /// <param name="deliveryScenarioID">The delivery scenario id.</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>
        /// List
        /// </returns>
        public List<DeliveryRouteDTO> FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID, Guid userUnit)
        {
            return deliveryRouteRepository.FetchDeliveryRoute(operationStateID, deliveryScenarioID, userUnit);
        }

        /// <summary>
        /// Fetch the Route Log Status.
        /// </summary>
        /// <returns>List</returns>
        public List<ReferenceDataDTO> FetchRouteLogStatus()
        {
            return referenceDataCategoryRepository.RouteLogStatus();
        }

        /// <summary>
        /// Fetch the Route Log Selection Type.
        /// </summary>
        /// <returns>List</returns>
        public List<ReferenceDataDTO> FetchRouteLogSelectionType()
        {
            return referenceDataCategoryRepository.RouteLogSelectionType();
        }

        /// <summary>
        /// Fetch the Delivery Scenario.
        /// </summary>
        /// <param name="operationStateID">The operationstate id.</param>
        /// <param name="deliveryScenarioID">The delivery scenario id.</param>
        /// <returns>List</returns>
        public List<ScenarioDTO> FetchDeliveryScenario(Guid operationStateID, Guid deliveryScenarioID)
        {
            return scenarioRepository.FetchScenario(operationStateID, deliveryScenarioID);
        }

        /// <summary>
        /// Fetch the Delivery Route for Basic Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>
        /// Task
        /// </returns>
        public async Task<List<DeliveryRouteDTO>> FetchDeliveryRouteforBasicSearch(string searchText, Guid userUnit)
        {
            return await deliveryRouteRepository.FetchDeliveryRouteForBasicSearch(searchText, userUnit);
        }

        /// <summary>
        /// Fetch the Delivery unit.
        /// </summary>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// List of <see cref="DeliveryUnitLocationDTO" />.
        /// </returns>
        public List<DeliveryUnitLocationDTO> FetchDeliveryUnit(Guid unitGuid)
        {
            var deliveryUnitLocationDTOList = deliveryUnitLocationRespository.FetchDeliveryUnit(unitGuid);

            foreach (var deliveryUnitLocationDTO in deliveryUnitLocationDTOList)
            {
                // take the unit boundry plus 1 mile envelope
                var unitBoundary = SqlGeometry.STPolyFromWKB(new SqlBytes(deliveryUnitLocationDTO.UnitBoundryPolygon.Envelope.Buffer(1609.34).Envelope.AsBinary()), 27700).MakeValid();

                deliveryUnitLocationDTO.BoundingBoxCenter = new List<double> { unitBoundary.STCentroid().STPointN(1).STX.Value, unitBoundary.STCentroid().STPointN(1).STY.Value };

                deliveryUnitLocationDTO.BoundingBox = new List<double> { unitBoundary.STPointN(1).STX.Value, unitBoundary.STPointN(1).STY.Value, unitBoundary.STPointN(3).STX.Value, unitBoundary.STPointN(3).STY.Value };

                deliveryUnitLocationDTO.UnitBoundaryGeoJSONData = GeUnitBoundaryJsonData(deliveryUnitLocationDTO);
            }

            return deliveryUnitLocationDTOList;
        }

        /// <summary>
        /// Fetch Delivery Route for Advance Search
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// Task
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText, Guid unitGuid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the boundary for an unit.
        /// </summary>
        /// <param name="deliveryUnitLocationDTO">The <see cref="DeliveryUnitLocationDTO"/>.</param>
        /// <returns>Json object containing boundary.</returns>
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

                var resultCoordinates = deliveryUnitLocationDTO.UnitBoundryPolygon;

                geometry.coordinates = new object();

                if (deliveryUnitLocationDTO.UnitBoundryPolygon.SpatialTypeName == OpenGisGeometryType.Polygon.ToString())
                {
                    geometry.type = OpenGisGeometryType.Polygon.ToString();

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
                    geometry.type = OpenGisGeometryType.MultiPolygon.ToString();

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
    }
}