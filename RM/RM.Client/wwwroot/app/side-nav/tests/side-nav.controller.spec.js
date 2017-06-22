describe('SideNav: Controller', function () {

    var ctrl;
    var $scope, $state, $stateParams, $mdSidenav, $mdDialog;
    var sideNavCloseMock;

    var MockSideNavController = {
        initialize: function () { return [] },
        closeSideNav: function () { return [] },
        fetchActions: function () { return [] },
    };

    var MockCommonConstants = {
        TitleContextPanel: function () { return [] }
    };

    beforeEach(module('ui.router'));
    beforeEach(module('ngMaterial'));

    beforeEach(module("sideNav", function ($provide) {
     sideNavCloseMock = jasmine.createSpy();
        $provide.factory('$mdSidenav', function () {
            return function (sideNavId) {
                return { close: sideNavCloseMock };
            };
        });
        $provide.factory('popUpSettingService', function ($q) {
            function printMap() {
                deferred = $q.defer();
                return deferred.promise;
            }
            return {
               printMap: printMap
            };
        });
        //$provide.factory('$mdDialog', function ($q) {
        //    return function show(content) {
        //        deferred = $q.defer();
        //        return deferred.promise;
        //    }
        //});
        $provide.value('CommonConstants', MockCommonConstants);
    }));
 
    beforeEach(inject(function ($rootScope, $state, $stateParams, popUpSettingService, $mdSidenav, $mdDialog, sideNavService, CommonConstants, $controller) {
        mockpopUpSettingService = popUpSettingService;
        mocksideNavService = sideNavService;
        mockCommonConstants = CommonConstants;
        mockmdDialog = $mdDialog;
        ctrl = $controller('SideNavController', {
            $state: $state,
            $stateParams: $stateParams,
            popUpSettingService: mockpopUpSettingService,
            $mdSidenav: $mdSidenav,
            $mdDialog: $mdDialog,
            sideNavService: mocksideNavService,
            CommonConstants: CommonConstants,
            $scope: $rootScope.$new(),
        });
    }));

     describe('Should close the dialog-window', function () {
         it('Should close window', function () {
               ctrl.closeSideNav();
               expect(sideNavCloseMock).toHaveBeenCalled();
               expect(sideNavCloseMock).toHaveBeenCalledTimes(1);
         });
     });

     describe('Should perform action with respect to the selected item', function () {
         it('If user selects `Print Map` : Check the pop up setting function is been called', function () {
             spyOn(mockpopUpSettingService, 'printMap').and.callThrough();
             ctrl.fetchActions('PrintMap');
             expect(mockpopUpSettingService.printMap).not.toBeUndefined();
             expect(mockpopUpSettingService.printMap).toHaveBeenCalled();
             expect(mockpopUpSettingService.printMap).toHaveBeenCalledTimes(1);
         });

         it('If user selects `Print Map` : Check the fetch action function is been called with correct parameter', function () {
             spyOn(ctrl, 'fetchActions');
             ctrl.fetchActions('PrintMap');
             expect(ctrl.fetchActions).not.toBeUndefined();
             expect(ctrl.fetchActions).toHaveBeenCalledWith('PrintMap');
             expect(ctrl.fetchActions).toHaveBeenCalled();
             expect(ctrl.fetchActions).toHaveBeenCalledTimes(1);
         });

         it('If user selects `Print Map` : Check the angular modal dialog is shown', function () {
             spyOn(mockmdDialog, 'show');
             ctrl.fetchActions('PrintMap');
             expect(mockmdDialog.show).not.toBeUndefined();
             expect(mockmdDialog.show).toHaveBeenCalled();
             expect(mockmdDialog.show).toHaveBeenCalledTimes(1);
         });
     });
});

