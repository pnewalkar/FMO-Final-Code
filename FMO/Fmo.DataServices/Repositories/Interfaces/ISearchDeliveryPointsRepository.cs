using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Fmo.DataServices.Entities;

namespace Fmo.DataServices.Repositories.Interface
{
    public interface ISearchDeliveryPointsRepository
    {
        List<DeliveryPoint> SearchDelievryPoints();
    }
}
