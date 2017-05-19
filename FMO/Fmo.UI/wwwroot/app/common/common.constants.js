
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
GeometryType: { Point: "Point", LineString: "LineString" } ,
pointTypes: { DeliveryPoint: { text: "Delivery Point", value: 'deliverypoint', style: "deliverypoint" }, AcessLink: { text: "Access Link", value: 'accesslink', style: "accesslink" }, Road: { text: "Road", value: 'roadlink', style: "roadlink" }, Selected: { text: "Selected", value: '', style: "deliverypoint" } },
RouteLogSelectionType: { Single: "Single", Multiple: "Multiple" }
};

angular.module('FMOApp')
.constant("CommonConstants", CommonConstants);
