angular
    .module('mapPanel')
    .controller("MapPanelController", MapPanelController)
function MapPanelController($scope, $timeout) {
        var vm = this;
        vm.collapsed=true;
        $scope.togglePannel=function(){
            $scope.collapsedPanel=!$scope.collapsedPanel
        }
        if (vm.oncreate)
            $timeout(vm.oncreate);
    };