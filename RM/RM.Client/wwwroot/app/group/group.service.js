'use strict';
angular.module('group')
    .factory('groupService', groupService);
groupService.$inject = [
    'referencedataApiService',
    '$q'];

function groupService(
    referencedataApiService,
    $q
    ) {
	return {
	    initialize: initialize,
	    DeliveryGroupTypes: DeliveryGroupTypes
	}

	function initialize() {
	    var deferred = $q.defer();

	    var result = { "DeliveryGroupType": null, "ServicePointType": null };
	    $q.all([
            DeliveryGroupTypes(),
            ServicePointTypes()
	    ]).then(function (response) {
	        result.DeliveryGroupType = response[0];
	        result.ServicePointType = response[1];
	        deferred.resolve(result)
	    })
	    return deferred.promise;
	}

	function DeliveryGroupTypes() {
	    var deferred = $q.defer();
	    referencedataApiService.getSimpleListsReferenceData(referenceDataConstants.DeliveryGroupType.DBCategoryName).then(function (response) {
	        deferred.resolve(response.listItems);
	    });
	    return deferred.promise;
	}

	function ServicePointTypes() {
	    var deferred = $q.defer();
	    referencedataApiService.getSimpleListsReferenceData(referenceDataConstants.ServicePointType.DBCategoryName).then(function (response) {
	        deferred.resolve(response.listItems);
	    });
	    return deferred.promise;
	}
}