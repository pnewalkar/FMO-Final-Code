using System.Threading.Tasks;

namespace RM.Operational.MapManager.WebAPI.IntegrationService
{
    public interface IMapIntegrationService
    {
        /// <summary>
        /// Method to generate pdf
        /// </summary>
        /// <param name="xml">xml</param>
        /// <param name="fileName">fileName</param>
        /// <returns></returns>
        Task<string> GenerateReportWithMap(string xml, string fileName);
    }
}