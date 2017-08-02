angular
    .module('backButton')
    .controller("BackButtonController",
    [
        '$scope',
        '$rootScope',
        '$state',
         BackButtonController]);

function BackButtonController(
    $scope,
    $rootScope,
    $state
) {
    vm = this;
    vm.go_back = go_back;
    var count = 0;

    function go_back(){
        onClick();
        function onClick() {
            if(count>0){
                storedState.pop();
                }
            count= 1;
            };
        $state.go(storedState.pop(), {}, {inherit: false});
    }
};