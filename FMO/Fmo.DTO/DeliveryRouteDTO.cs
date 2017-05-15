using System;

namespace Fmo.DTO
{
    public class DeliveryRouteDTO
    {
        public Guid ID { get; set; }

        public int? ExternalId { get; set; }

        public string RouteName { get; set; }

        public string RouteNumber { get; set; }

        public int OperationalStatus_Id { get; set; }

        public int RouteMethodType_Id { get; set; }

        public int? TravelOutTransportType_Id { get; set; }

        public int? TravelInTransportType_Id { get; set; }

        public decimal? TravelOutTimeMin { get; set; }

        public decimal? TravelInTimeMin { get; set; }

        public decimal? SpanTimeMin { get; set; }

        public int? DeliveryScenario_Id { get; set; }

        public string DeliveryRouteBarcode { get; set; }

        public string RouteNameNumber
        {
            get
            {
                return "(" + RouteNumber.Trim() + ")" + RouteName;
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
        public decimal? Totaltime { get; set; }
    }
}