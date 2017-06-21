angular
    .module('printMap')
    .controller("PrintMapController", PrintMapController);

PrintMapController.$inject = [
    'printMapService',
    'mapService'
];

function PrintMapController(
    printMapService,
    mapService
    ) {

    var vm = this;
    vm.closeWindow = closeWindow;
    vm.initialize = initialize;
    vm.printMap = printMap;
    vm.printOptions = { orientation: "Landscape" };
    vm.initialize();

    function initialize() {
        printMapService.initialize().then(function (response) {
            vm.printMapDPI = response.MapDpi;
            vm.imageHeight = response.ImageHeight;
            vm.imageWidth = response.ImageWidth;
            vm.pdfSize = response.pdfSize;
        });
    }

    function closeWindow() {
        printMapService.closeWindow();
    }

    function printMap() {
        printMapService.generateMapPDF(vm.printMapDPI, vm.printOptions.size, vm.imageWidth, vm.imageHeight, mapService.getResolution(), vm.printOptions);
    }
 }






