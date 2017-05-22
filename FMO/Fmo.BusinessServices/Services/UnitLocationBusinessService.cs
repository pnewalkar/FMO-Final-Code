using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common.Constants;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Helpers;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;

namespace Fmo.BusinessServices.Services
{
    /// <summary>
    /// This class contains methods for fetching Delivery unit data.
    /// </summary>
    /// <seealso cref="Fmo.BusinessServices.Interfaces.IUnitLocationBusinessService" />
    public class UnitLocationBusinessService : IUnitLocationBusinessService
    {
        private IUnitLocationRepository unitLocationRespository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitLocationBusinessService"/> class.
        /// </summary>
        /// <param name="unitLocationRespository">The unit location respository.</param>
        public UnitLocationBusinessService(IUnitLocationRepository unitLocationRespository)
        {
            this.unitLocationRespository = unitLocationRespository;
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
    }
}