angular
    .module('mapPanel')
    .controller("MapPanelController", MapPanelController)
    function MapPanelController ($scope) {
        var vm = this;
        vm.collapsed=true;
        $scope.togglePannel=function(){
            $scope.collapsedPanel=!$scope.collapsedPanel
        }
    };