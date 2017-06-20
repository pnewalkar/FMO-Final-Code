using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.DataManagement.UnitManager.WebAPI.BusinessService.Interface;

namespace RM.DataManagement.UnitManager.WebAPI.BusinessService.Implementation
{
    /// <summary>
    /// This class contains methods for fetching Delivery unit data.
    /// </summary>
    /// <seealso cref="Fmo.BusinessServices.Interfaces.IUnitLocationBusinessService" />
    public class UnitLocationBusinessService : IUnitLocationBusinessService
    {
        private IUnitLocationDataService unitLocationRespository;
        private IPostCodeSectorDataService postcodeSectorDataService = default(IPostCodeSectorDataService);
        private IPostCodeDataService postCodeDataService = default(IPostCodeDataService);
        private IScenarioDataService scenarioDataService = default(IScenarioDataService);

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitLocationBusinessService"/> class.
        /// </summary>
        /// <param name="unitLocationRespository"> The unit location respository</param>
        /// <param name="postcodeSectorDataService">post code sector data service</param>
        /// <param name="postCodeDataService">post code data service</param>
        /// <param name="scenarioDataService">scenario data service</param>
        public UnitLocationBusinessService(IUnitLocationDataService unitLocationRespository, IPostCodeSectorDataService postcodeSectorDataService, IPostCodeDataService postCodeDataService, IScenarioDataService scenarioDataService)
        {
            this.unitLocationRespository = unitLocationRespository;
            this.postcodeSectorDataService = postcodeSectorDataService;
            this.postCodeDataService = postCodeDataService;
            this.scenarioDataService = scenarioDataService;
        }

        /// <summary>
        /// Fetch the Delivery unit.
        /// </summary>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// The <see cref="UnitLocationDTO" />.
        /// </returns>
        public UnitLocationDTO FetchDeliveryUnit(Guid unitGuid)
        {
            var unitLocationDTO = unitLocationRespository.FetchDeliveryUnit(unitGuid);

            return unitLocationDTO;
        }

        /// <summary>
        /// Fetch the Delivery units for user.
        /// </summary>
        /// <param name="userId">The unit unique identifier.</param>
        /// <returns>
        /// List of <see cref="UnitLocationDTO" />.
        /// </returns>
        public List<UnitLocationDTO> FetchDeliveryUnitsForUser(Guid userId)
        {
            var unitLocationDTOList = unitLocationRespository.FetchDeliveryUnitsForUser(userId);

            foreach (var unitLocationDTO in unitLocationDTOList)
            {
                // take the unit boundry plus 1 mile envelope
                var unitBoundary = SqlGeometry.STPolyFromWKB(new SqlBytes(unitLocationDTO.UnitBoundryPolygon.Envelope.Buffer(1609.34).Envelope.AsBinary()), Constants.BNGCOORDINATESYSTEM).MakeValid();

                unitLocationDTO.BoundingBoxCenter = new List<double> { unitBoundary.STCentroid().STPointN(1).STX.Value, unitBoundary.STCentroid().STPointN(1).STY.Value };

                unitLocationDTO.BoundingBox = new List<double> { unitBoundary.STPointN(1).STX.Value, unitBoundary.STPointN(1).STY.Value, unitBoundary.STPointN(3).STX.Value, unitBoundary.STPointN(3).STY.Value };

                unitLocationDTO.UnitBoundaryGeoJSONData = GeUnitBoundaryJsonData(unitLocationDTO);
            }

            return unitLocationDTOList;
        }


        public UnitLocationDTO FetchUnitDetails(Guid unitGuid)
        {
            return unitLocationRespository.FetchUnitDetails(unitGuid);

        }

        /// <summary>
        /// Get the postcode sector by the UDPRN id
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>PostCodeSectorDTO object</returns>
        public async Task<PostCodeSectorDTO> GetPostCodeSectorByUDPRN(int uDPRN)
        {
            return await postcodeSectorDataService.GetPostCodeSectorByUDPRN(uDPRN);
        }

        /// <summary>
        /// Fetch the post code for Basic Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>
        /// Task
        /// </returns>
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForBasicSearch(string searchText, Guid userUnit)
        {
            return await postCodeDataService.FetchPostCodeUnitForBasicSearch(searchText, userUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the count of post code
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of post code</returns>
        public async Task<int> GetPostCodeUnitCount(string searchText, Guid userUnit)
        {
            return await postCodeDataService.GetPostCodeUnitCount(searchText, userUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// Fetch the post code for advanced Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>
        /// Task
        /// </returns>
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForAdvanceSearch(string searchText, Guid userUnit)
        {
            return await postCodeDataService.FetchPostCodeUnitForAdvanceSearch(searchText, userUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// Get post code ID by passing post code.
        /// </summary>
        /// <param name="postCode"> Post Code</param>
        /// <returns>Post code ID</returns>
        public async Task<Guid> GetPostCodeID(string postCode)
        {
            return await postCodeDataService.GetPostCodeID(postCode);
        }

        /// <summary>
        /// Fetch the Delivery Scenario.
        /// </summary>
        /// <param name="operationStateID">The operationstate id.</param>
        /// <param name="deliveryScenarioID">The delivery scenario id.</param>
        /// <returns>List</returns>
        public List<ScenarioDTO> FetchDeliveryScenario(Guid operationStateID, Guid deliveryScenarioID)
        {
            return scenarioDataService.FetchScenario(operationStateID, deliveryScenarioID);
        }

        /// <summary>
        /// Gets the boundary for an unit.
        /// </summary>
        /// <param name="unitLocationDTO">The <see cref="UnitLocationDTO"/>.</param>
        /// <returns>Json object containing boundary.</returns>
        private string GeUnitBoundaryJsonData(UnitLocationDTO unitLocationDTO)
        {
            string jsonData = string.Empty;
            if (unitLocationDTO != null)
            {
                var geoJson = new GeoJson
                {
                    features = new List<Feature>()
                };

                SqlGeometry sqlGeo = null;

                Geometry geometry = new Geometry();

                var resultCoordinates = unitLocationDTO.UnitBoundryPolygon;

                geometry.coordinates = new object();

                if (unitLocationDTO.UnitBoundryPolygon.SpatialTypeName == OpenGisGeometryType.Polygon.ToString())
                {
                    geometry.type = OpenGisGeometryType.Polygon.ToString();

                    sqlGeo = SqlGeometry.STPolyFromWKB(new SqlBytes(resultCoordinates.AsBinary()), Constants.BNGCOORDINATESYSTEM).MakeValid();
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
                else if (unitLocationDTO.UnitBoundryPolygon.SpatialTypeName == OpenGisGeometryType.MultiPolygon.ToString())
                {
                    geometry.type = OpenGisGeometryType.MultiPolygon.ToString();

                    sqlGeo = SqlGeometry.STMPolyFromWKB(new SqlBytes(resultCoordinates.AsBinary()), Constants.BNGCOORDINATESYSTEM).MakeValid();
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
                    id = unitLocationDTO.ID.ToString(),
                    geometry = geometry
                };

                geoJson.features.Add(feature);

                jsonData = JsonConvert.SerializeObject(geoJson);
            }

            return jsonData;
        }

        public async Task<List<PostCodeDTO>> GetPostCodes(Guid unitGuid, List<Guid> postcodeGuids)
        {
            return await unitLocationRespository.GetPostCodes(postcodeGuids, unitGuid);
        }

        public async Task<PostCodeDTO> GetSelectedPostCode(Guid unitGuid, Guid postcodeGuid)
        {
            return await unitLocationRespository.GetSelectedPostcode(postcodeGuid, unitGuid);
        }
    }
}