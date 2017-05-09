using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
    /// <summary>
    /// This interface contains methods for fetching Address Location data
    /// </summary>
    public interface IAddressLocationBusinessService
    {
        /// <summary>
        /// This method is used to fetch data for Access Links.
        /// </summary>
        /// <param name="uDPRN">UDPRN</param>
        /// <returns>Address Location DTO</returns>
        object GetAddressLocationByUDPRN(int uDPRN);
    }
}