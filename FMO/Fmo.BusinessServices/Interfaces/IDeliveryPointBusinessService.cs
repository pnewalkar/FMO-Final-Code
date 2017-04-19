namespace Fmo.BusinessServices.Interfaces
{
    /// <summary>
    /// This interface contains declaration of methods for fetching Delivery Points data.
    /// </summary>
    public interface IDeliveryPointBusinessService
    {
        /// <summary>
        /// This method is used to fetch delivery points data on the basis of coordinates.
        /// </summary>
        /// <param name="boundaryBox">boundarybox as string</param>
        /// <returns>delivery points data as object</returns>
        object GetDeliveryPoints(string boundaryBox);

        /// <summary>
        /// This method is used to fetch delivery points data on the basis of udprn.
        /// </summary>
        /// <param name="udprn">udprn as string</param>
        /// <returns>delivery points data as object</returns>
        object GetDeliveryPointByUDPRN(int udprn);
    }
}