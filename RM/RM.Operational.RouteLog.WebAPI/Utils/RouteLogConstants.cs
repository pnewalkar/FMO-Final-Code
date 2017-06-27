using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.Operational.RouteLog.WebAPI.Utils
{
    public static class RouteLogConstants
    {
        internal const string DeliveryRouteManagerWebAPIName = "DeliveryRouteManagerWebAPIName";
        internal const string PDFGeneratorWebAPIName = "PDFGeneratorWebAPIName";
        internal const string Street = "Street";

        internal const string XSLTFilePath = "XSLTFilePath";
        internal const string Report = "report";

        internal const string PageHeader = "pageHeader";
        internal const string PageFooter = "pageFooter";
        internal const string Content = "content";
        internal const string Heading1 = "heading1";
        internal const string Heading2 = "heading2";
        internal const string PdfOutPut = "outputTo";
        internal const string A4Portrait = "A4Portrait";
        internal const string Caption = "caption";
        internal const string PageNumber = "pageNumbers";
        internal const string Section = "section";
        internal const string SectionColumn = "sectionColumn";
        internal const string Width = "width";
        internal const string Paragraph = "paragraph";
        internal const string Table = "table";
        internal const string Columns = "columns";
        internal const string Column = "column";
        internal const string Borders = "borders";
        internal const string UseShading = "useShading";
        internal const string Row = "row";
        internal const string Shade = "shade";
        internal const string Cell = "cell";
        internal const string Header = "header";
        internal const string Source = "source";
        internal const string Heading1CenterAligned = "heading1CenterAligned";
        internal const string Image = "image";
        internal const string RouteSummaryAlias = "* All Alias, Hazards/Area Hazards and Special Instructions Information is shown on the detailed route log and hazard card.";
        internal const string RouteSummarySequencedPoints = "Sequenced Points";
        internal const string RouteSummaryHeader = "Route Log Summary";

        /*Route Summary PDF Column Name*/
        internal const string RouteSummaryName = "Name";
        internal const string Number = "Number";
        internal const string RouteMethod = "Method";
        internal const string DeliveryOffice = "Delivery Office";
        internal const string Aliases = "Aliases*";
        internal const string Block = "Blocks";
        internal const string Scenario = "Scenario";

        internal const string CollectionPoint = "CPs";
        internal const string DeliveryPoint = "DPs";
        internal const string BusinessDeliveryPoint = "Business DPs";
        internal const string ResidentialDeliveryPoint = "Residential DPs";
        internal const string AccelerationIn = "Acceleration In";
        internal const string AccelerationOut = "Acceleration Out";
        internal const string PairedRoute = "Paired Route";

        internal const string NoD2D = "No D2D";
        internal const string DPExemptions = "DP Exemptions";
        internal const string MultipleOccupancy = "Multiple Occupancy";
        internal const string SpecialInstructions = "Special Instructions*";
        internal const string AreaHazards = "Hazards/Area Hazards*";
    }
}
