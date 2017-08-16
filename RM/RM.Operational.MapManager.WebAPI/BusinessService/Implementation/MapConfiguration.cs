using RM.CommonLibrary.Reporting.Pdf;

namespace RM.Operational.MapManager.WebAPI.BusinessService
{
    /// <summary>
    /// Configuration options for the Map report
    /// </summary>
    internal class MapConfiguration
    {
        /// <summary>
        /// Constructor
        /// Assigns default options
        /// </summary>
        public MapConfiguration()
        {
            // Set up the default configuration
            OutputTo = ReportFactoryHelper.ReportOutputToOption.A4Landscape;
        }





        /// <summary>
        /// Gets or sets the output to option 
        /// </summary>
        public ReportFactoryHelper.ReportOutputToOption OutputTo { get; set; }
    }
}
