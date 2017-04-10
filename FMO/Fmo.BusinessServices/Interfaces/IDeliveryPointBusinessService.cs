namespace Fmo.BusinessServices.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using System.Net.Http;

    public interface IDeliveryPointBusinessService
    {
        //   Task<List<DeliveryPointDTO>> SearchDelievryPoints();

        object GetDeliveryPoints();

        DeliveryPointDTO GetDeliveryPoints1(string bbox);

        string GetData(string query, params object[] parameters);
    }
}
