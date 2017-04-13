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
        string GetDeliveryPoints(string boundarybox);

        string GetDeliveryPointByUDPRN(int udprn);
    }
}
