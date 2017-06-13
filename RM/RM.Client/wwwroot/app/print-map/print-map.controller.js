angular
    .module('printMap')
    .controller("PrintMapController", PrintMapController);

PrintMapController.$inject = [
    'printMapService',
    '$scope',
    '$mdDialog'
];

function PrintMapController(
    printMapService,
    $scope,
    $mdDialog) {

    var vm = this;
    vm.closeWindow = closeWindow;
    vm.printMap = printMap;
    vm.printMapDto = { "MapTitle": "Title", "PrintTime": null, "CurrentScale": "200", "PdfOrientation": "Landscape", "PdfSize": "A4", "MapScale": 25, "EncodedString": "sdfsdf" };
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
    function printMap() {
        printMapService.printMap(vm.printMapDto).then(function (response) {
            generatePdf(response.result);
        });
    }
    function generatePdf(pdfFileName) {
        if (pdfFileName) {
            printMapService.generatePdf(pdfFileName).then(function (response) {
                if (window.navigator && window.navigator.msSaveOrOpenBlob) {
                    var byteCharacters = atob(response.data);
                    var byteNumbers = new Array(byteCharacters.length);
                    for (var i = 0; i < byteCharacters.length; i++) {
                        byteNumbers[i] = byteCharacters.charCodeAt(i);
                    }
                    var byteArray = new Uint8Array(byteNumbers);
                    var blob = new Blob([byteArray], {
                        type: 'application/pdf'
                    });
                    window.navigator.msSaveOrOpenBlob(blob, response.fileName);
                } else {
                    var base64EncodedPDF = response.data;
                    var dataURI = "data:application/pdf;base64," + base64EncodedPDF;
                    window.open(dataURI, "_blank");
                }
            });
        }

    }
}

