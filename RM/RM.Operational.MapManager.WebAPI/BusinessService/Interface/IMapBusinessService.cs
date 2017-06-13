using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DTO;

namespace RM.Operational.MapManager.WebAPI.BusinessService
{
    public interface IMapBusinessService
    {
        /// <summary>
        /// Method to retrieve map details
        /// </summary>
        /// <param name="printMapDTO">printMapDTO</param>
        /// <returns>printMapDTO</returns>
        Task<string> GenerateReportWithMap(PrintMapDTO printMapDTO);
    }
}