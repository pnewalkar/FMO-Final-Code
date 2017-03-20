using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DataServices.Repositories.Interface;
using Fmo.DataServices.Entities;

namespace Fmo.BusinessServices.Interfaces
{
    public interface IDeliveryPointBussinessService
    {
        List<DeliveryPoint> SearchDelievryPoints();
    }
}
