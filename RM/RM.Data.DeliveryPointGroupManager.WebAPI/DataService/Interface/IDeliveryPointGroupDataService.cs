using System.Threading.Tasks;

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
    }
}