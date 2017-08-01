'use strict';
describe('SideNav: Controller', function () {
    var vm;
    var $scope;
    var $rootScope;
    var $state;
    var $stateParams;
    var popUpSettingService;
    var $mdSidenav;
    var $mdDialog;
    var sideNavService;
    var sideNavCloseMock;
    var CommonConstantsMock = {
        TitleSimulation: "Simulation",
        RouteLogActionName: "Route Log",
        RouteSimulationActionName: "Route Simulation",
        DeliveryPointActionName: "Delivery Point",
        AccessLinkActionName: "Access Link",
        PrintMapActionName: "Print Map"
    };    
    var stateMockData = {"selectedUnit":{"displayText":"BN    Worthing  Office","ID":"b51aa229-c984-4ca6-9c12-510187b81050","icon":"fa-map-marker delivery","$$mdSelectId":1,"$$hashKey":"object:114"}};
    var stateParamsMockData = {
      hide:true,
      positionedThirdPartyDeliveryPointList: 'thirdparty',
      positionedDeliveryPointList: 'firstPointList',
      deliveryPointList: '2ndpointlist'
    };

    beforeEach(function() {
        module('sideNav');
        module(function ($provide) {
            $provide.value('CommonConstants', CommonConstantsMock);                        
            $provide.value('$stateParams', {});
            $provide.value('$state', { go: function(state, args){}});
            $provide.factory('$mdDialog', function() {
                return {hide:jasmine.createSpy(), show:jasmine.createSpy()};
            });
            sideNavCloseMock = jasmine.createSpy();
            $provide.factory('$mdSidenav', function() {
                return function(sideNavId) {
                  return {close: sideNavCloseMock }
                }
            });
            $provide.service('sideNavService', function() {                
                function fetchActionItems(){
                    return {
                          RolesActionResult: function(){}                 
                      }
                }
                return {
                    fetchActionItems: fetchActionItems
                };
            });
            $provide.factory('popUpSettingService', function(){
                function routeLog() {}
                function printMap(){}
                return {
                  routeLog: routeLog,
                  printMap: printMap
                }
            });
        });

        inject(function(
            _$rootScope_,
            _$controller_,
            _$state_,
            _$stateParams_,
            _popUpSettingService_,
            _$mdSidenav_,
            _$mdDialog_,
            _sideNavService_,
            _CommonConstants_){

            $rootScope = _$rootScope_;
            $scope = _$rootScope_.$new();
            $state = _$state_;
            $stateParams = _$stateParams_;
            popUpSettingService = _popUpSettingService_;
            $mdSidenav = _$mdSidenav_;
            $mdDialog = _$mdDialog_;
            sideNavService = _sideNavService_;
            CommonConstants = _CommonConstants_;

            vm = _$controller_('SideNavController',{
                $state: $state,
                $stateParams: $stateParams,
                popUpSettingService: popUpSettingService,
                $mdSidenav: $mdSidenav,
                $mdDialog: $mdDialog,
                sideNavService: sideNavService,
                CommonConstants: CommonConstants
            });
        });        
    });


    it('should close left side nav bar', function() {
        vm.closeSideNav();
        expect(sideNavCloseMock).toHaveBeenCalled();
    });

    it('should call `closeSideNav` once FetchActions method called', function() {
        spyOn(vm,'closeSideNav');

        vm.fetchActions('Print Map');

        expect(vm.closeSideNav).toHaveBeenCalled();
    });

    it('should Dialog show when RouteLogActionName is `Route Log`', function() {        
        vm.selectedDeliveryUnit = true;
        spyOn(popUpSettingService,'routeLog');

        vm.fetchActions('Route Log');

        expect($mdDialog.show).toHaveBeenCalled();
        expect(popUpSettingService.routeLog).toHaveBeenCalledWith(true);

    });

    it('should redirect to routeSimulation when RouteSimulationActionName is `Route Simulation`', function() {        
        vm.selectedDeliveryUnit = true;
        spyOn($state,'go');

        vm.fetchActions('Route Simulation');

        expect(vm.contextTitle).toEqual('Simulation');
        expect($state.go).toHaveBeenCalledWith("routeSimulation", { selectedUnit: true });
    });

    it('should redirect to deliveryPoint when DeliveryPointActionName is `Delivery Point`', function() {        
        vm.selectedDeliveryUnit = true;
        spyOn($state,'go');

        vm.fetchActions('Delivery Point');

        expect(vm.contextTitle).toEqual('Delivery Point');
        expect($state.go).toHaveBeenCalledWith("deliveryPoint", { selectedUnit: true });
    });

    it('should redirect to referenceData when AccessLinkActionName is `Access Link`', function() {        
        spyOn($state,'go');

        vm.fetchActions('Access Link');

        expect(vm.contextTitle).toEqual('Access Link');
        expect($state.go).toHaveBeenCalledWith("referenceData");
    });

    it('should Dialog show when PrintMapActionName is `Print Map`', function() {        
        spyOn(popUpSettingService,'printMap');

        vm.fetchActions('Print Map');

        expect($mdDialog.show).toHaveBeenCalled();
        expect(popUpSettingService.printMap).toHaveBeenCalled();

    });
});

