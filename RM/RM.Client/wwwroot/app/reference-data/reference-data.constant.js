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
    OperationalStatus: { DBCategoryName: "Operational Status", AppCategoryName: "OperationalStatus", ReferenceDataNames: [] },
    AccessLinkStatus: { DBCategoryName: "Access Link Status", AppCategoryName: "AccessLinkStatus", ReferenceDataNames: [] },
    PostalAddressType: { DBCategoryName: "Postal Address Type", AppCategoryName: "PostalAddressType", ReferenceDataNames: [] },
    AccessLinkDirection: { DBCategoryName: "Access Link Direction", AppCategoryName: "AccessLinkDirection", ReferenceDataNames: [] },
    OperationalObjectType: { DBCategoryName: "Operational Object Type", AppCategoryName: "OperationalObjectType", ReferenceDataNames: [] },
    UI_DeliveryPoint_Type: { DBCategoryName: "UI_DeliveryPoint_Type", AppCategoryName: "UI_DeliveryPoint_Type", ReferenceDataNames: [] },
    DeliveryPointUseIndicator: { DBCategoryName: "DeliveryPoint Use Indicator", AppCategoryName: "DeliveryPointUseIndicator", ReferenceDataNames: [] },
    UI_RouteLogSearch_SelectionType: { DBCategoryName: "UI_RouteLogSearch_SelectionType", AppCategoryName: "UI_RouteLogSearch_SelectionType", ReferenceDataNames: [] },
    UI_SearchRules: { DBCategoryName: "UI_SearchRules", AppCategoryName: "UI_SearchRules", ReferenceDataNames: [{ AppReferenceDataName: "UI_MinNumSearchCharacters", DBReferenceDataName: "UI_MinNumSearchCharacters" }] },
    UI_RouteLog_Status: { DBCategoryName: "UI_RouteLog_Status", AppCategoryName: "UI_RouteLog_Status", ReferenceDataNames: [] },

};

angular.module('RMApp')
.constant("referenceDataConstants", referenceDataConstants);
