using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DTO;

namespace RM.Operational.MapManager.WebAPI.BusinessService
{
    public interface IMapBusinessService
    {
        /// <summary>
        /// Method to save captured map image
        /// </summary>
        /// <param name="printMapDTO">printMapDTO</param>
        /// <returns>printMapDTO</returns>
        PrintMapDTO SaveImage(PrintMapDTO printMapDTO);

        /// <summary>
        /// Generate map report
        /// </summary>
        /// <param name="printMapDTO"></param>
        /// <returns></returns>
        Task<string> GenerateMapPdfReport(PrintMapDTO printMapDTO);
    }
}