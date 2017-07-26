'use strict';
describe('SideNav: Controller', function () {
    var vm;
    var $scope;
    var $rootScope;
    var $state;
    var $stateParams;
    var $mdSidenav;
    var $mdDialog;
    var sideNavService;
    var CommonConstants;


    beforeEach(function() {
        module('SideNav');
        module(function($provide){

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
            $scope = _$rootScope_.new();
            $state = _$state_;
            $stateParams = _$stateParams_;
            popUpSettingService = _popUpSettingService_;
            $mdSidenav = _$mdSidenav_;
            $mdDialog = _$mdDialog_;
            sideNavService = _sideNavService_;
            CommonConstants = _CommonConstants_;

            vm  = _$controller_('',{

            });
        });
    });
});

