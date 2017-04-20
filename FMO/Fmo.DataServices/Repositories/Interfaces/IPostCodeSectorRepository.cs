using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    /// <summary>
    /// IPostCodeSectorRepository interface to abstract away the PostCodeSectorRepository implementation
    /// </summary>
    public interface IPostcodeSectorRepository
    {
        /// <summary>
        /// Get the postcode sector by the UDPRN id
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>PostCodeSectorDTO object</returns>
        PostCodeSectorDTO GetPostCodeSectorByUDPRN(int uDPRN);
    }
}
