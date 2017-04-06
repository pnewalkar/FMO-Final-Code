using Microsoft.SqlServer.Types;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO
{
  
    public class Coordinates
    {
        private double[][] coordinates;

        public Coordinates(SqlGeometry geog)
        {
            coordinates = new double[][] { new double[] { geog.STX.Value, geog.STY.Value } };
        }

        public Coordinates(double[] coords)
        {
            if (coords.Length != 2)
            {
                throw new ArgumentException("Coordinates must have exactly 2 values");
            }
            coordinates = new double[][] { coords };
        }

        public Coordinates(double[][] coords)
        {
            if (coords.Any(c => c.Length != 2))
            {
                throw new ArgumentException("Coordinates must have exactly 2 values");
            }
            coordinates = coords;
        }

        public JArray getJson()
        {
            var obj = new JArray();

            switch (coordinates.Length)
            {
                case 0:
                    obj.Add(new JArray());
                    break;
                case 1:
                    obj.Add(coordinates[0]);
                    break;
                default:
                    obj.Add(coordinates.Select(c => JArray.FromObject(c)));
                    break;
            }

            return obj;
        }
    }
}
