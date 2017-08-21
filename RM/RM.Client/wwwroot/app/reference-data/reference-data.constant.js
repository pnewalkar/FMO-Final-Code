var referenceDataConstants = {
    PostalAddressStatus: { DBCategoryName: "Postal Address Status", AppCategoryName: "PostalAddressStatus", ReferenceDataNames: [] },
    AccessLinkType: { DBCategoryName: "Access Link Type", AppCategoryName: "AccessLinkType", ReferenceDataNames: [] },
    NetworkNodeType: { DBCategoryName: "Network Node Type", AppCategoryName: "NetworkNodeType", ReferenceDataNames: [] },
    NotificationType: { DBCategoryName: "Notification Type", AppCategoryName: "NotificationType", ReferenceDataNames: [] },
    NotificationPriority: { DBCategoryName: "Notification Priority", AppCategoryName: "NotificationPriority", ReferenceDataNames: [] },
    DeliveryRouteOperationalStatus: { DBCategoryName: "Delivery Route Operational Status", AppCategoryName: "DeliveryRouteOperationalStatus", ReferenceDataNames: [] },
    DeliveryRouteMethodType: { DBCategoryName: "Delivery Route Method Type", AppCategoryName: "DeliveryRouteMethodType", ReferenceDataNames: [] },
    TransportType: { DBCategoryName: "Transport Type", AppCategoryName: "TransportType", ReferenceDataNames: [] },
    UnitLocationType: { DBCategoryName: "Unit Location Type", AppCategoryName: "UnitLocationType", ReferenceDataNames: [] },
    NetworkLinkType: { DBCategoryName: "Network Link Type", AppCategoryName: "NetworkLinkType", ReferenceDataNames: [] },
    DataProvider: { DBCategoryName: "Data Provider", AppCategoryName: "DataProvider", ReferenceDataNames: [] },
    DeliveryUnitStatus: { DBCategoryName: "Delivery Unit Status", AppCategoryName: "DeliveryUnitStatus", ReferenceDataNames: [] },
    DeliveryPointOperationalStatus: { DBCategoryName: "Delivery Point Operational Status", AppCategoryName: "DeliveryPointOperationalStatus", ReferenceDataNames: [] },
    AccessLinkStatus: { DBCategoryName: "Access Link Status", AppCategoryName: "AccessLinkStatus", ReferenceDataNames: [] },
    PostalAddressType: { DBCategoryName: "Postal Address Type", AppCategoryName: "PostalAddressType", ReferenceDataNames: [] },
    AccessLinkDirection: { DBCategoryName: "Access Link Direction", AppCategoryName: "AccessLinkDirection", ReferenceDataNames: [] },
    OperationalObjectType: { DBCategoryName: "Operational Object Type", AppCategoryName: "OperationalObjectType", ReferenceDataNames: [] },
    UI_DeliveryPoint_Type: { DBCategoryName: "UI_DeliveryPoint_Type", AppCategoryName: "UI_DeliveryPoint_Type", ReferenceDataNames: [] },
    DeliveryPointUseIndicator: { DBCategoryName: "DeliveryPoint Use Indicator", AppCategoryName: "DeliveryPointUseIndicator", ReferenceDataNames: [] },
    UI_RouteLogSearch_SelectionType: { DBCategoryName: "UI_RouteLogSearch_SelectionType", AppCategoryName: "UI_RouteLogSearch_SelectionType", ReferenceDataNames: [] },
    UI_SearchRules: { DBCategoryName: "UI_SearchRules", AppCategoryName: "UI_SearchRules", ReferenceDataNames: [{ AppReferenceDataName: "UI_MinNumSearchCharacters", DBReferenceDataName: "UI_MinNumSearchCharacters" }] },
    PDF_PageSize: { AppCategoryName: "PDF_PageSize", DBCategoryName: "PDF_PageSize", ReferenceDataNames: [{ AppReferenceDataName: "PDF_PageSize_A0", DBReferenceDataName: "PDF_PageSize_A0" }, { AppReferenceDataName: "PDF_PageSize_A1", DBReferenceDataName: "PDF_PageSize_A1" }, { AppReferenceDataName: "PDF_PageSize_A2", DBReferenceDataName: "PDF_PageSize_A2" }, { AppReferenceDataName: "PDF_PageSize_A3", DBReferenceDataName: "PDF_PageSize_A3" }, { AppReferenceDataName: "PDF_PageSize_A4", DBReferenceDataName: "PDF_PageSize_A4" }] },
    PrintMap_DPI: { AppCategoryName: "PrintMap_DPI", DBCategoryName: "PrintMap_DPI", ReferenceDataNames: [{ AppReferenceDataName: "PrintMap_DPI", DBReferenceDataName: "PrintMap_DPI" }] },
    Transparency: { DBCategoryName: "Object Transparency", AppCategoryName: "ObjectTransparency" },
    PrintMap_mmPerInch: {
        AppCategoryName: "PrintMap_mmPerInch",
        DBCategoryName: "PrintMap_mmPerInch",
        ReferenceDataNames: [
          {
              AppReferenceDataName: "PrintMap_mmPerInch",
              DBReferenceDataName: "PrintMap_mmPerInch"
          }
        ]
    },
    PrintMap_ImageWidthmm: {
        AppCategoryName: "PrintMap_ImageWidthmm",
        DBCategoryName: "PrintMap_ImageWidthmm",
        ReferenceDataNames: [
          {
              AppReferenceDataName: "PrintMap_ImageWidthmm_A0",
              DBReferenceDataName: "PrintMap_ImageWidthmm_A0"
          },
          {
              AppReferenceDataName: "PrintMap_ImageWidthmm_A1",
              DBReferenceDataName: "PrintMap_ImageWidthmm_A1"
          },
          {
              AppReferenceDataName: "PrintMap_ImageWidthmm_A3",
              DBReferenceDataName: "PrintMap_ImageWidthmm_A3"
          },
          {
              AppReferenceDataName: "PrintMap_ImageWidthmm_A4",
              DBReferenceDataName: "PrintMap_ImageWidthmm_A4"
          },
          {
              AppReferenceDataName: "PrintMap_ImageWidthmm_A2",
              DBReferenceDataName: "PrintMap_ImageWidthmm_A2"
          }
        ]
    },
    PrintMap_ImageHeightmm: {
        AppCategoryName: "PrintMap_ImageHeightmm",
        DBCategoryName: "PrintMap_ImageHeightmm",
        ReferenceDataNames: [
          {
              AppReferenceDataName: "PrintMap_ImageHeightmm_A0",
              DBReferenceDataName: "PrintMap_ImageHeightmm_A0"
          },
          {
              AppReferenceDataName: "PrintMap_ImageHeightmm_A1",
              DBReferenceDataName: "PrintMap_ImageHeightmm_A1"
          },
          {
              AppReferenceDataName: "PrintMap_ImageHeightmm_A2",
              DBReferenceDataName: "PrintMap_ImageHeightmm_A2"
          },
          {
              AppReferenceDataName: "PrintMap_ImageHeightmm_A3",
              DBReferenceDataName: "PrintMap_ImageHeightmm_A3"
          },
          {
              AppReferenceDataName: "PrintMap_ImageHeightmm_A4",
              DBReferenceDataName: "PrintMap_ImageHeightmm_A4"
          }
        ]
    },
    SenarioOperationState: { AppCategoryName: "SenarioOperationState", DBCategoryName: "Senario Operation State", ReferenceDataNames: [] },
    DeliveryPointColor: { DBCategoryName: "Delivery Point Color", AppCategoryName: "DeliveryPointColor", ReferenceDataNames: [] },
    SubBuildingType: { DBCategoryName: "Sub Building Type", AppCategoryName: "SubBuildingType", ReferenceDataNames: [] },

    DeliveryGroupType: { DBCategoryName: "Delivery Group Type", AppCategoryName: "DeliveryGroupType", ReferenceDataNames: [] },
    ServicePointType: { DBCategoryName: "Service Point Type", AppCategoryName: "ServicePointType", ReferenceDataNames: [] },
    UI_Range_Options: { DBCategoryName: "UI_Range_Options", AppCategoryName: "UI_Range_Options", ReferenceDataNames: [] },
    UI_Delete_DP_ReasonCode: { DBCategoryName: "UI_Delete_DP_ReasonCode", AppCategoryName: "UIDeleteDPReasonCode", ReferenceDataNames: [] },

};

angular.module('RMApp')
.constant("referenceDataConstants", referenceDataConstants);