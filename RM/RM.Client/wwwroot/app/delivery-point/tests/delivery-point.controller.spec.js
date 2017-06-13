/*'use strict';

describe('deliveryPoint Test', function () {
    
    beforeEach(module('deliveryPoint'));
    
    describe('delivery-point controller test', function() {
        var $controller;
    
        beforeEach(angular.mock.inject(function(_$controller_, _mapToolbarService_){
          $controller = _$controller_;
          mapToolbarService = _mapToolbarService_;
        }));
        
        describe('resultSet', function(){
            it('query length less than 3', function(){
                var $scope={};
                var controller=$controller('DeliveryPointController',
                                           {$scope:$scope}
                                          );
                var query = 'kcgggjg';
                expect($scope.resultscount[0]).toEqual(0);
            });
        });
    });
});*/