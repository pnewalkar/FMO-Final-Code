
var CommonConstants = {
RouteLogActionName:"Route Log",
RouteSimulationActionName:"Route Simulation",
DeliveryPointActionName: "Delivery Point",
AccessLinkActionName: "Access Link",
TitleContextPanel: "Context Panel",
TitleSimulation: "Simulation",
GetSessionStorageItemType:"roleAccessData",
EntityType: { DeliveryPoint: "DeliveryPoint", StreetNetwork: "StreetNetwork", Route: "Route", Postcode: "Postcode" },
ButtonShapeType: { point: "point", line: "line", accesslink: "accesslink", select: "select" },
GeometryType: { Point: "Point", LineString: "LineString" }
    //,
//PointType: { DeP: { text: "DP", value: 'DeliveryPoint' } }
};

angular.module('FMOApp')
.constant("CommonConstants", CommonConstants);
