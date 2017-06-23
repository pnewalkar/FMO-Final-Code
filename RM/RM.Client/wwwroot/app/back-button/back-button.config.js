angular
	.module('backButton')
    .run(function ($rootScope) {
        var vm = this;
        vm.storedState = [];
        $rootScope.$on('$stateChangeStart', function (evt, toState, toParams, fromState, fromParams) {
            vm.storedState.push(fromState);
        })
    })