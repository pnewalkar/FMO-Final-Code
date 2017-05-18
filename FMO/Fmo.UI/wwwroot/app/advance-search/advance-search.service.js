
angular.module('advanceSearch')
        .service('advanceSearchService', advanceSearchService);
advanceSearchService.$inject = ['advanceSearchApiService',
                                '$q',                               
                                'searchApiService',
                                'mapFactory'];

function advanceSearchService(advanceSearchApiService,
                              $q,
                              searchApiService,
                              mapFactory) {
    
    
    var deliveryPointObj = null;
    var postCodeObj = null;
    var streetnameObj = null;
    var deliveryRouteobj = null;
    var route = [];
    var obj = null;

    return {
        queryAdvanceSearch: queryAdvanceSearch,
        onChangeItem: onChangeItem
    };

    function queryAdvanceSearch(query) {
        var deferred = $q.defer();
        var advanceSearchResults = null;
        var arrDeliverypoints = [];
        var arrPostCodes = [];
        var arrStreetNames = [];
        var arrDeliveryRoutes = [];
        var arrRoutes = [];

        advanceSearchApiService.advanceSearch(query).then(function (response) {
            advanceSearchResults = response;
            angular.forEach(advanceSearchResults.searchResultItems, function (value, key) {
                if (value.type == "DeliveryPoint")
                {
                    obj = { 'displayText': value.displayText, 'UDPRN': value.udprn, 'type': "DeliveryPoint" }
                    arrDeliverypoints.push(obj);
                }
               else if (value.type == "Postcode") {
                   obj = { 'displayText': value.displayText }
                    arrPostCodes.push(obj);
                }
               else if (value.type == "StreetNetwork") {
                   obj = { 'displayText': value.displayText }
                   arrStreetNames.push(obj);
                }
                if (value.type == "Route") {
                    obj = { 'displayText': value.displayText }
                    arrDeliveryRoutes.push(obj);
                }
            });

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
        var coordinatesData = null;
        if (selectedItem.type === "DeliveryPoint") {
            searchApiService.GetDeliveryPointByUDPRN(selectedItem.UDPRN)
                .then(function (response) {
                    coordinatesData = response.data;
                    lat = coordinatesData.features[0].geometry.coordinates[1];
                    long = coordinatesData.features[0].geometry.coordinates[0];
                    mapFactory.setDeliveryPoint(long, lat);
                    deferred.resolve(coordinatesData);
                });
            return deferred.promise;
        }
    }
}