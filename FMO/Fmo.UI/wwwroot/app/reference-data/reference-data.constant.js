//(function (window) {
//    window.__env = window.__env || {};
//    window.__env.apiUrl = 'http://localhost:34583/api';
//}(this));

var referenceDataConstants = {
    PostalAddressStatus: "Postal Address Status",
    AccessLinkType: "Access Link Type",
    NetworkNodeType: "Network Node Type",
    NotificationType: "Notification Type",
    NotificationPriority: "Notification Priority",
    
    DeliveryRouteOperationalStatus: "Delivery Route Operational Status",
    DeliveryRouteMethodType: "Delivery Route Method Type",
    //---changes in val--//
    DeliveryRouteTransportType: "Delivery Route Transport Type",
    UnitLocationType: "Unit Location Type",
    NetworkLinkType: "Network Link Type",
    DataProvider: "Data Provider",
    DeliveryUnitStatus: "Delivery Unit Status",
    DeliveryPointOperationalStatus: "Delivery Point Operational Status",
    AccessLinkStatus: "Access Link Status",
    PostalAddressType: "Postal Address Type",
    AccessLinkDirection: "Access Link Direction",
    OperationalObjectType: "Operational Object Type",
    DeliveryPointType: "UI_DeliveryPoint_Type",
    DeliveryPointUseIndicator: "DeliveryPoint Use Indicator",
    UIRouteLogSearchSelectionType: "UI_RouteLogSearch_SelectionType",
    AccessLinkParameters: "Access Link Parameters",
    UISearchRules: "UI_SearchRules"
};

angular.module('FMOApp')
.constant("referenceDataConstants", referenceDataConstants);
