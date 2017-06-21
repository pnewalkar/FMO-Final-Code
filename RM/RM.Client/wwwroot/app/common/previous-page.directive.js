angular.module('RMApp')

    .run(function ($rootScope) {
        var vm = this;
        vm.storedState = [];
        $rootScope.$on('$stateChangeStart', function (evt, toState, toParams, fromState, fromParams) {
            vm.storedState.push(fromState);
        })
    })

  .directive('previousPage', ['$state', '$rootScope', function ($state, $rootScope) {
      return {
          restrict: 'E',
          replace: true,
          template: `<i class ="fa fa-arrow-left" aria-hidden="true" ng-click="go_back()"></i>`,
          link: function(scope, elem, attrs) {
              var count = 0;
              scope.go_back = function () {
                  onClick();
                  function onClick() {
                      if(count>0){
                          storedState.pop();
                      }
                      count= 1;
                  };
                  $state.go(storedState.pop());
              }
          }

          //link: function (scope, elem, attrs) {
          //    scope.go_back = function () {
          //        $state.go($rootScope.previousData);
          //    }
          //}
      }
  }]);