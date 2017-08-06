'use strict'
describe('Home: Controller', function() {
	var vm;
	var $scope;
    var $rootScope;

    var MockmdSidenav = {
        toggle: function () { return true; }
    };
    var MockErrorService = {
        openAlert: function () { return true; }
    };

	beforeEach(function() {
		module('home');
        module(function ($provide) {   
            $provide.value('$mdSidenav', MockmdSidenav);
            $provide.value('errorService', MockErrorService); 
        });
		inject(function (
            $controller,
            _$rootScope_,
            _$mdSidenav_,
            _errorService_) {

            $rootScope = _$rootScope_;
            $scope = $rootScope.$new();
            vm = $controller('HomeController', {
                $scope : $scope,
                $mdSidenav : _$mdSidenav_,
                errorService : _errorService_
            });
            spyOn($rootScope, '$broadcast').and.callThrough();            
        });

        
	});

    it('should set lastOpendedDialog value to `empty`', function(){
        expect(vm.lastOpenedDialog).toBe("");
    });

	it('should call toggle function of $mdSidenav when toggleSideNav method called', function () {               
        spyOn(MockmdSidenav, 'toggle');
        MockmdSidenav.toggle();
        expect(MockmdSidenav.toggle).toHaveBeenCalled();
    });

    it('should invoke an event when "dialogOpen" broadcasted', function () {
        var obj = { status: 'deliveryPoint' };
        $scope.$broadcast('dialogOpen', obj);
        $scope.$digest();
        expect(vm.lastOpenedDialog).toEqual({ status: 'deliveryPoint' });
    });

    it('should invoke an event when "dialogClosed" broadcasted', function () {
        var obj = { status: 'deliveryPoint' };
        $scope.$broadcast('dialogClosed', obj);
        $scope.$digest();
        expect(vm.lastOpenedDialog).toEqual("");
    });

    it('should invoke an event when "errorClosed" broadcasted', function () {
        var obj = { status: 'errorClosed' };
        vm.lastOpenedDialog = obj;
        $scope.$broadcast('errorClosed', obj);
        $scope.$digest();
        expect($rootScope.$broadcast).toHaveBeenCalledWith('showDialog', vm.lastOpenedDialog);
    });

    it('should invoke an event when "showError" broadcasted', function () {
        var obj = { status: 'error' };
        $scope.$broadcast('showError', [{ status: 'error' }]);
        $scope.$digest();
        expect($rootScope.$broadcast).toHaveBeenCalledWith('showError', [{ status: 'error' }]);               
    });

    it('should call openAlert function of errorService when "showError" broadcasted', function () {
        spyOn(MockErrorService, 'openAlert');
        MockErrorService.openAlert([{ status: 'error' }]);
        expect(MockErrorService.openAlert).toHaveBeenCalled();
    });


});
 