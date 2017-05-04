'use strict';
angular
    .module('referencedata')
    .controller("ReferenceDataController",
               ['$scope',
                'referencedataApiService',
                '$filter',
                'referenceDataConstants',
                 ReferenceDataController])

function ReferenceDataController(
    $scope,
    referencedataApiService,
    $filter,
    referenceDataConstants
) {

    var vm = this;
    vm.initialize = initialize;
    vm.referenceData = referenceData;
    vm.initialize();

    function initialize() {
        vm.referenceData();
    }

    function referenceData() {
        referencedataApiService.getReferenceData().success(function (response) {
            $scope.filtereditems = $filter('filter')(response, { categoryName: referenceDataConstants.DeliveryPointOperationalStatus });
        });
    }
}