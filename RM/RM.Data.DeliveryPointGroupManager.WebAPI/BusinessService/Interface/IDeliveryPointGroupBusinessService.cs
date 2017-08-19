using System;
using System.Threading.Tasks;
using RM.Data.DeliveryPointGroupManager.WebAPI.DTO;

namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.BusinessService
{
    /// <summary>
    /// This interface contains declaration of methods for Delivery Points Group data.
    /// </summary>
    public interface IDeliveryPointGroupBusinessService
    {
        /// <summary>
        /// Create delivery point group.
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>string</returns>
        Task<DeliveryPointGroupDTO> CreateDeliveryPointGroup(DeliveryPointGroupDTO deliveryPointGroupDto);
        string GetDeliveryPointGroups(string boundaryBox, Guid unitGuid);

        DeliveryPointGroupDTO UpdateDeliveryGroup(DeliveryPointGroupDTO deliveryPointGroupDto);
    }
}