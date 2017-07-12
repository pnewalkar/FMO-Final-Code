using System;

namespace RM.DataManagement.UnitManager.WebAPI.DTO
{
    public class ScenarioDTO
    {
        public int DeliveryScenario_Id { get; set; }

        public string ScenarioName { get; set; }

        public int? DeliveryUnit_Id { get; set; }

        public int? OperationalState_Id { get; set; }

        public Guid ID { get; set; }

        public Guid? DeliveryUnit_GUID { get; set; }

        public Guid? OperationalState_GUID { get; set; }
    }
}