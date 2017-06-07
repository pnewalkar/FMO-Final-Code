angular
    .module('home')
    .run(function($rootScope){
    $rootScope.$on('$stateChangeStart', function(evt, toState, toParams, fromState, fromParams) {
      $rootScope.previousState = fromState.name;
    })
});