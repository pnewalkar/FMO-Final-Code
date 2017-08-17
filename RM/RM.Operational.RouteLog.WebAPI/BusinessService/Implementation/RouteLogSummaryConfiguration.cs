using RM.CommonLibrary.Reporting.Pdf;

namespace RM.Operational.RouteLog.WebAPI.BusinessService
{
    /// <summary>
    /// Configuration options for the Route Log Summary report
    /// </summary>
    internal class RouteLogSummaryConfiguration
    {
        /// <summary>
        /// Constructor
        /// Assigns default options
        /// </summary>
        public RouteLogSummaryConfiguration()
        {
            // Set up the default configuration
            OutputTo = ReportFactoryHelper.ReportOutputToOption.A4Portrait;
            PageHeaderShowPageNumbers = false;
            PageFooterShowPageNumbers = true;
            UseTableBorders = false;
            UseTableShading = true;
        }





        /// <summary>
        /// Gets or sets the output to option
        /// </summary>
        public ReportFactoryHelper.ReportOutputToOption OutputTo { get; set; }



        /// <summary>
        /// Gets or sets the page footer show page numbers option
        /// </summary>
        public bool PageFooterShowPageNumbers { get; set; }



        /// <summary>
        /// Gets or sets the page header show page numbers option
        /// </summary>
        public bool PageHeaderShowPageNumbers { get; set; }



        /// <summary>
        /// Gets or sets the use table borders option
        /// </summary>
        public bool UseTableBorders { get; set; }



        /// <summary>
        /// Gets or sets the use table shading option
        /// </summary>
        public bool UseTableShading { get; set; }
    }
}