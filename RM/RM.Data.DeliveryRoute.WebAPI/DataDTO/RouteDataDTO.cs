using System;

namespace RM.DataManagement.DeliveryRoute.WebAPI.DataDTO
{
    public class RouteDataDTO
    {
        public Guid ID { get; set; }

        public int? GeoRouteID { get; set; }

        public string RouteName { get; set; }

        public string RouteNumber { get; set; }

        public decimal? SpanTimeMinute { get; set; }

        public string RouteBarcode { get; set; }

        public Guid RouteMethodTypeGUID { get; set; }

        public double? TotalDistanceMeter { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public DateTime? LastModifiedDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public Guid? UnSequencedBlockID { get; set; }

        public string DisplayText
        {
            get
            {
                if (!string.IsNullOrEmpty(RouteNumber) && !string.IsNullOrEmpty(RouteName))
                {
                    return RouteName + "(" + RouteNumber.Trim() + ")";
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public Guid MethodReferenceGuid { get; set; }

        public string Method { get; set; }

        public string DeliveryOffice { get; set; }

        public int Aliases { get; set; }

        public int Blocks { get; set; }

        public string ScenarioName { get; set; }

        public int DPs { get; set; }

        public int BusinessDPs { get; set; }

        public int ResidentialDPs { get; set; }

        public Guid? TravelOutTransportType_GUID { get; set; }

        public Guid? TravelInTransportType_GUID { get; set; }

        public string AccelarationIn { get; set; }

        public string AccelarationOut { get; set; }

        public string PairedRoute { get; set; }

        public string Totaltime { get; set; }
    }
}