angular.module('RMApp')

    .run(function ($rootScope) {
        $rootScope.$on('$stateChangeStart', function (evt, toState, toParams, fromState, fromParams) {

            $rootScope.previousData = fromState.name;
        /* console.log("$stateChangeStart " + fromState.name + JSON.stringify(fromParams) + " -> " + toState.name + JSON.stringify(toParams));*/
        })
    })

  .directive('previousPage', ['$state', '$rootScope', function ($state, $rootScope) {
      return {
          restrict: 'E',
          replace: true,
          template: `<i class ="fa fa-arrow-left" aria-hidden="true" ng-click="go_back()"></i>`,

          link: function (scope, elem, attrs) {
              scope.go_back = function () {
                  $state.go($rootScope.previousData);
              }
          }
      }
  }]);