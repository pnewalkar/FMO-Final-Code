using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface ISearchDeliveryPointsRepository
    {
        List<DeliveryPoint> SearchDelievryPoints();
    }
}
