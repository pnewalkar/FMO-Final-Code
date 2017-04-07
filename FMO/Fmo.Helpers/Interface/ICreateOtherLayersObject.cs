using Fmo.DTO;
using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.Helpers.Interface
{
    public interface ICreateOtherLayersObjects
    {
        Feature getAccessLinks(Geometry geometry, DbGeometry resultCoordinates);
    }
}
