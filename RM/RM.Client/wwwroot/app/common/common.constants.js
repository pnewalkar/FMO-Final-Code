
var CommonConstants = {

    RouteLogActionName: "Route Log",
    RouteSimulationActionName: "Route Simulation",
    DeliveryPointActionName: "Delivery Point",
    AccessLinkActionName: "Access Link",
    TitleContextPanel: "Context Panel",
    TitleSimulation: "Simulation",
    DetailsOfDeliveryPoint: "Details of Delivery Point",
    GetSessionStorageItemType: "roleAccessData",
    EntityType: { DeliveryPoint: "DeliveryPoint", StreetNetwork: "StreetNetwork", Route: "Route", Postcode: "Postcode" },
    ButtonShapeType: { point: "point", line: "line", accesslink: "accesslink", select: "select", area: "area", del: "delete" },
    GeometryType: { Point: "Point", LineString: "LineString" , Polygon : "Polygon"},
    pointTypes: { DeliveryPoint: { text: "Delivery Point", value: 'deliverypoint', style: "deliverypoint" }, AcessLink: { text: "Access Link", value: 'accesslink', style: "accesslink" }, Road: { text: "Road", value: 'roadlink', style: "roadlink" }, Selected: { text: "Selected", value: '', style: "deliverypoint" } },
    RouteLogSelectionType: { Single: "Single", Multiple: "Multiple" },
    SearchLessThanThreeCharactersErrorMessage: "At least three characters must be input for a Search",
    SearchErrorType: "Warning",
    RouteName: "ROUTENAME",
    DpUse: "DPUSE",
    PrintMapActionName: "Print Map",
    PrintMapmmPerInch: 25.4,
    UserType: { DeliveryUser: "Delivery User", CollectionUser: "Collection User", ManagerUser: "Manager User" }
};

angular.module('RMApp')
.constant("CommonConstants", CommonConstants);
