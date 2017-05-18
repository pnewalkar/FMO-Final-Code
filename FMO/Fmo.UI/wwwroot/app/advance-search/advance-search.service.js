
angular.module('advanceSearch')
        .service('advanceSearchBusinessService', advanceSearchBusinessService);
advanceSearchBusinessService.$inject = [
 'advanceSearchApiService', 'popUpSettingService', '$q', 'searchApiService', 'mapFactory'
];

function advanceSearchBusinessService(advanceSearchApiService, popUpSettingService, $q, searchApiService, mapFactory) {
    return {
        queryAdvanceSearch: queryAdvanceSearch,
        onChangeItem: onChangeItem
    };
    var results = null;
    var deliveryPointObj = null;
    var postCodeObj = null;
    var streetnameObj = null;
    var deliveryRouteobj = null;
    var route = [];
    var obj = null;

    function queryAdvanceSearch(query) {
        var deferred = $q.defer();
        var arrDeliverypoints = [];
        var arrPostCodes = [];
        var arrStreetNames = [];
        var arrDeliveryRoutes = [];
        var arrRoutes = [];

        advanceSearchApiService.advanceSearch(query).then(function (response) {
            results = response.data;
            for (var i = 0; i < results.searchResultItems.length; i++) {
                route = results.searchResultItems[i];
                obj;
                if (route.type == 'DeliveryPoint') {
                    obj = { 'displayText': route.displayText, 'UDPRN': route.udprn, 'type': "DeliveryPoint" }
                    arrDeliverypoints.push(obj);
                }
                else if (route.type == 'Postcode') {
                    obj = { 'displayText': route.displayText }
                    arrPostCodes.push(obj);
                }
                else if (route.type == 'StreetNetwork') {
                    obj = { 'displayText': route.displayText }
                    arrStreetNames.push(obj);
                }
                else if (route.type == 'Route') {
                    obj = { 'displayText': route.displayText }
                    arrDeliveryRoutes.push(obj);
                }
            }

            if (arrDeliverypoints.length == 1) {
                deliveryPointObj = { 'type': 'DeliveryPoint', 'name': arrDeliverypoints, 'open': true };
            }
            else {
                deliveryPointObj = { 'type': 'DeliveryPoint', 'name': arrDeliverypoints, 'open': false };
            }

            if (arrPostCodes.length == 1) {
                postCodeObj = {
                    'type': 'PostCode', 'name': arrPostCodes, 'open': true
                };
            }
            else {
                postCodeObj = {
                    'type': 'PostCode', 'name': arrPostCodes, 'open': false
                };
            }
            if (arrStreetNames.length == 1) {
                streetnameObj = { 'type': 'StreetNetwork', 'name': arrStreetNames, 'open': true };
            }
            else {
                streetnameObj = { 'type': 'StreetNetwork', 'name': arrStreetNames, 'open': false };
            }
            if (arrDeliveryRoutes.length == 1) {
                deliveryRouteobj = { 'type': 'Route', 'name': arrDeliveryRoutes, 'open': true };
            }
            else {
                deliveryRouteobj = { 'type': 'Route', 'name': arrDeliveryRoutes, 'open': false };
            }

            if (arrDeliverypoints.length > 0) {
                arrRoutes.push(deliveryPointObj);
            }
            if (arrPostCodes.length > 0) {
                arrRoutes.push(postCodeObj);
            }
            if (arrStreetNames.length > 0) {
                arrRoutes.push(streetnameObj);
            }
            if (arrDeliveryRoutes.length > 0) {
                arrRoutes.push(deliveryRouteobj);
            }
            deferred.resolve(arrRoutes);
        });
        return deferred.promise;
    }

    function onChangeItem(selectedItem) {
        var deferred = $q.defer();
        var data = null;
        if (selectedItem.type === "DeliveryPoint") {
            searchApiService.GetDeliveryPointByUDPRN(selectedItem.UDPRN)
                .then(function (response) {
                    data = response.data;
                    lat = data.features[0].geometry.coordinates[1];
                    long = data.features[0].geometry.coordinates[0];
                    mapFactory.setDeliveryPoint(long, lat);
                    deferred.resolve(data);
                });
            return deferred.promise;
        }
    }
}