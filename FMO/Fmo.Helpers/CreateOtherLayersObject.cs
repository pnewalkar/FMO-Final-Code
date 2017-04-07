using System;
using System.Collections.Generic;
using System.Reflection;
using System.Dynamic;
using System.IO;
using Fmo.DTO;
using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;
using Fmo.Helpers.Interface;
using System.Data.Entity.Spatial;

namespace Fmo.Helpers
{
    public class CreateOtherLayerObjects : ICreateOtherLayersObjects
    {

        public Feature getAccessLinks(Geometry geometry, DbGeometry resultCoordinates)
        {

            List<Feature> lstFeatures = new List<Feature>();
            SqlGeometry sqlGeo = null;
            if (geometry.type == "LineString")
            {
                sqlGeo = SqlGeometry.STLineFromWKB(new SqlBytes(resultCoordinates.AsBinary()), 27700).MakeValid();

                List<double[]> cords = new List<double[]>();

                for (int pt = 1; pt <= sqlGeo.STNumPoints().Value; pt++)
                {
                    double[] coordinatesval = new double[] { sqlGeo.STPointN(pt).STX.Value, sqlGeo.STPointN(pt).STY.Value };
                    cords.Add(coordinatesval);
                }

                geometry.coordinates = cords;
            }
            else
            {
                sqlGeo = SqlGeometry.STGeomFromWKB(new SqlBytes(resultCoordinates.AsBinary()), 27700).MakeValid();
                double[] coordinatesval = new double[] { sqlGeo.STX.Value, sqlGeo.STY.Value };
                geometry.coordinates = coordinatesval;
            }

            Feature features = new Feature();
            features.geometry = geometry;

            features.type = "Feature";
            //  features.properties = new Properties { type = "accesslink" };

            //lstFeatures.Add(features);

            return features;
        }

    }
}
