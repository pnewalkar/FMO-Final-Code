using Fmo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Spatial;

namespace Fmo.Helpers.Interface
{
    public interface ICreateOtherLayersObjects
    {
        Feature getAccessLinks(Geometry geometry, DbGeometry resultCoordinates);
    }
}
