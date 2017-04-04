using Fmo.DTO;
using System.Collections.Generic;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IDeliveryUnitLocationRepository
    {
        List<DeliveryUnitLocationDTO> FetchDeliveryUnit();
    }
}
