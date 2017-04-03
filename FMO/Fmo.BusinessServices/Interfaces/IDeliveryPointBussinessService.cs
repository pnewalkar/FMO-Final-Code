using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using System.IO;

namespace Fmo.BusinessServices.Interfaces
{
    public interface IDeliveryPointBussinessService
    {
        List<DeliveryPointDTO> SearchDelievryPoints();

        MemoryStream GetDeliveryPoints(string bbox);
    }
}
