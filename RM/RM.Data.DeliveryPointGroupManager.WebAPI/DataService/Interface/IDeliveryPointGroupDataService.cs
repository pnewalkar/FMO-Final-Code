using System.Threading.Tasks;

using RM.Data.DeliveryPointGroupManager.WebAPI.DTO;

namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.DataService
{
    /// <summary>
    /// This interface contains declarations of methods for fetching, Insertnig Delivery Group data.
    /// </summary>
    public interface IDeliveryPointGroupDataService
    {

        /// <summary>
        /// Add delivery point group
        /// </summary>
        /// <returns></returns>
        Task CreateDeliveryGroup();
        DeliveryPointGroupDTO UpdateDeliveryGroup(DeliveryPointGroupDTO deliveryPointGroupDto);
    }
}