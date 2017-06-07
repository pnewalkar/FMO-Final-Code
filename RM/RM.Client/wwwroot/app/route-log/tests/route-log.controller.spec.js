
﻿describe('Route-Log: Controller', function () {

    var mockService, controller, deferred;
    var items;
    var vm;
    beforeEach(function () {

        angular.mock.module('routeLog');

        angular.mock.module(function ($provide) {
            $provide.factory('routeLogService', function ($q) {
                function loadSelectionType() {
                    deferred = $q.defer();
                    return deferred.promise;
                }
                function loadRouteLogStatus() {
                    deferred = $q.defer();
                    return deferred.promise;
                }
                function closeWindow() { return [] }
                function loadScenario() {
                    deferred = $q.defer();
                    return deferred.promise;
                }
                function loadDeliveryRoute() {
                    deferred = $q.defer();
                    return deferred.promise;
                }
                return {
                    loadSelectionType: loadSelectionType,
                    loadRouteLogStatus: loadRouteLogStatus,
                    closeWindow: closeWindow,
                    loadScenario: loadScenario,
                    loadDeliveryRoute: loadDeliveryRoute
                };
            });
        });

        angular.mock.inject(function ($controller, routeLogService, _$rootScope_) {
            $rootScope = _$rootScope_;
            vm = $rootScope.$new();
            mockService = routeLogService;
            controller = $controller('RouteLogController', {
                routeLogService: mockService,
                items: items,
                $scope: vm
            })
        });
    });

    describe('Should be call inner funtion', function () {
        it('Should be call initialize function', function () {
            spyOn(controller, 'loadSelectionType').and.callThrough();
            spyOn(controller, 'loadRouteLogStatus').and.callThrough();
            controller.initialize();
            expect(controller.loadSelectionType).toHaveBeenCalled();
            expect(controller.loadRouteLogStatus).toHaveBeenCalled();
        });
    });

    describe('Should close the dialog-window', function () {
        it('Should close window', function () {
            spyOn(mockService, 'closeWindow').and.callThrough();
            controller.closeWindow();
            expect(mockService.closeWindow).toHaveBeenCalled();
        });
    });

    describe('Change selected values', function () {
        it('Should clear fields on changing selection type', function () {
            controller.selectedRouteStatusObj = 'route status';
            controller.selectedRouteScenario = 'route scenario';
            controller.selectedRoute = 'selected route';
            controller.isSelectionType = true;
            controller.isRouteScenarioDisabled = false;
            controller.isDeliveryRouteDisabled = false;
            controller.isShowMultiSelectionRoute = true;
            controller.deliveryRoute = 'fake';
            controller.selectionTypeChange();
            expect(controller.deliveryRoute).toBe(null);
        });
    });

    describe('Call loadSelectionType function', function () {
        it('Should be call loadSelectionType function', function () {
            spyOn(mockService, 'loadSelectionType').and.callThrough();
            controller.loadSelectionType();
            expect(mockService.loadSelectionType).toHaveBeenCalled();
        });
    });

    describe('Call loadScenario function', function () {
        it('Should be call loadScenario function', function () {
            vm.isSelectionType = false;
            spyOn(mockService, 'loadRouteLogStatus').and.callThrough();
            controller.loadRouteLogStatus();
            expect(mockService.loadRouteLogStatus).toHaveBeenCalled();
        });
    });

    describe('Call loadRouteLogStatus function', function () {
        it('Should be call loadRouteLogStatus function', function () {
            spyOn(mockService, 'loadRouteLogStatus').and.callThrough();
            spyOn(controller, 'loadScenario').and.callThrough();
            controller.loadRouteLogStatus();
            expect(mockService.loadRouteLogStatus).toHaveBeenCalled();
        });
    });
     
});
