angular
    .module('printMap')
    .controller("PrintMapController", PrintMapController);

PrintMapController.$inject = [
    '$scope',
    '$mdDialog'
];

function PrintMapController(
    $scope,
    $mdDialog) {

    var vm = this;
    vm.closeWindow = closeWindow;
    //{A4, A3, A2*, A1*, A0*}
    vm.sizeList = [
        { id: '1', value: 'A4' },
        { id: '2', value: 'A3' },
        { id: '3', value: 'A2*' },
        { id: '4', value: 'A1*' },
        { id: '5', value: 'A0*' }]
    vm.printOptions = { orientation: "Landscape" };

    function closeWindow() {
        $mdDialog.cancel();
    }
}

