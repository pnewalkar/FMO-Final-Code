angular
    .module('printMap')
    .controller("PrintMapController", PrintMapController);

PrintMapController.$inject = [
    'printMapService',
    '$scope',
    '$mdDialog',
    'mapService',
    'mapFactory'
];

function PrintMapController(
    printMapService,
    $scope,
    $mdDialog,
    mapService,
    mapFactory) {

    var vm = this;
    vm.closeWindow = closeWindow;
    vm.loadPdfSize = loadPdfSize;
    vm.initialize = initialize;
    vm.getMapDpi = getMapDpi;
    vm.getMapPerInch = getMapPerInch;
    vm.getImageHeight = getImageHeight;
    vm.getImageWidth = getImageWidth;
    vm.printMap = printMap;
    vm.mapSize;
    vm.printMapDto = { "MapTitle": "Title", "PrintTime": null, "CurrentScale": "200", "PdfOrientation": "Landscape", "PdfSize": "A4", "MapScale": 25, "EncodedString": "sdfsdf" };
    vm.printOptions = { orientation: "Landscape" };
    vm.initialize();

    function initialize() {
        vm.getMapDpi();
        vm.getMapPerInch();
        vm.getImageHeight();
        vm.getImageWidth();
        vm.loadPdfSize();
        vm.map = mapFactory.getMap();
        vm.mapSize = vm.map.getSize();
        vm.map.addControl(new ol.control.ScaleLine());
        document.getElementsByClassName('ol-overlaycontainer-stopevent')[0].style.visibility = "hidden";
    }
    function closeWindow() {
        $mdDialog.cancel();
    }
    function printMap() {
        var imageWidthmm = getMapImageWidth(vm.printOptions.size);
        var imageHeightmm = getMapImageHeight(vm.printOptions.size);
        var mapWidth = Math.round((imageWidthmm * parseInt(vm.printMapDPI)) / parseInt(vm.printMapmmPerInch));
        var mapHeight = Math.round((imageHeightmm * parseInt(vm.printMapDPI)) / parseInt(vm.printMapmmPerInch));
        vm.map.setSize([mapWidth, mapHeight]);

        vm.map.once('postcompose', function (event) {
            writeScaletoCanvas(event);
        });
        vm.map.renderSync();
    }
    function displayPdf(pdfFileName) {
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
    function writeScaletoCanvas(e) {
        var ctx = e.context;
        var canvas = e.context.canvas;
        //get the Scaleline div container the style-width property
        var olscale = document.getElementsByClassName('ol-scale-line-inner')[0];
        //Scaleline thicknes
        var line1 = 6;
        //Offset from the left
        var x_offset = 10;
        //offset from the bottom
        var y_offset = 30;
        var fontsize1 = 15;
        var font1 = fontsize1 + 'px Arial';
        // how big should the scale be (original css-width multiplied)
        var multiplier = 2;
        var scalewidth = parseInt(olscale.style.width, 10) * multiplier;
        var scale = olscale.innerHTML;
        var scalenumber = parseInt(scale, 10);
        var scaleunit = scale.match(/[Aa-zZ]{1,}/g);

        var calculatedScale = setScaleUnit(scalenumber, scaleunit);
        //Scale Text
        ctx.beginPath();
        ctx.textAlign = "left";
        ctx.strokeStyle = "#ffffff";
        ctx.fillStyle = "#000000";
        ctx.lineWidth = 5;
        ctx.font = font1;
        ctx.strokeText([calculatedScale], x_offset + fontsize1 / 2, canvas.height - y_offset - fontsize1 / 2);
        ctx.fillText([calculatedScale], x_offset + fontsize1 / 2, canvas.height - y_offset - fontsize1 / 2);

        //Scale Dimensions
        var xzero = scalewidth + x_offset;
        var yzero = canvas.height - y_offset;
        var xfirst = x_offset + scalewidth * 1 / 4;
        var xsecond = xfirst + scalewidth * 1 / 4;
        var xthird = xsecond + scalewidth * 1 / 4;
        var xfourth = xthird + scalewidth * 1 / 4;

        // Stroke
        ctx.beginPath();
        ctx.lineWidth = line1 + 2;
        ctx.strokeStyle = "#000000";
        ctx.fillStyle = "#ffffff";
        ctx.moveTo(x_offset, yzero);
        ctx.lineTo(xzero + 1, yzero);
        ctx.stroke();

        //sections black/white
        ctx.beginPath();
        ctx.lineWidth = line1;
        ctx.strokeStyle = "#000000";
        ctx.moveTo(x_offset, yzero);
        ctx.lineTo(xfirst, yzero);
        ctx.stroke();

        ctx.beginPath();
        ctx.lineWidth = line1;
        ctx.strokeStyle = "#FFFFFF";
        ctx.moveTo(xfirst, yzero);
        ctx.lineTo(xsecond, yzero);
        ctx.stroke();

        ctx.beginPath();
        ctx.lineWidth = line1;
        ctx.strokeStyle = "#000000";
        ctx.moveTo(xsecond, yzero);
        ctx.lineTo(xthird, yzero);
        ctx.stroke();

        ctx.beginPath();
        ctx.lineWidth = line1;
        ctx.strokeStyle = "#FFFFFF";
        ctx.moveTo(xthird, yzero);
        ctx.lineTo(xfourth, yzero);
        ctx.stroke();

        var resolution = vm.map.getView().getResolution();

        vm.printMapDto = {
            "MapTitle": vm.printOptions.title,
            "CurrentScale": '1 : ' + Math.round(mapFactory.getScaleFromResolution(resolution)),
            "PdfOrientation": vm.printOptions.orientation,
            "PdfSize": vm.printOptions.size,
            "MapScale": 25,
            "EncodedString": canvas.toDataURL('image/png')
        };
        var dataURI = canvas.toDataURL('image/png')
        vm.map.setSize(vm.mapSize);
        mapService.refreshLayers();
        printMapService.printMap(vm.printMapDto).then(function (response) {
            generatePdf(response);
        });
        window.open(dataURI, "_blank");
    }
    function generatePdf(data) {
        printMapService.mapPdf(data).then(function (response) {
            displayPdf(response);
        });
    }
    function setScaleUnit(scalenumber, scaleunit) {
        if (scaleunit == 'km') {
            var scale = scalenumber * 0.621371;
            if (scale < 1) {
                scale = scalenumber * 1000
                return Math.round(scale) + ' m';
            }
            else {
                return Math.round(scale) + ' mi';
            }
        }
        else if (scaleunit == 'm') {
            var scale = scalenumber * 0.000621371;
            if (scale < 1) {
                return scalenumber + ' m';
            }
            else {
                return Math.round(scale) + ' mi';
            }
        }
    }
    function loadPdfSize() {
        printMapService.loadPdfSize().then(function (response) {
            vm.pdfSize = response;
        })
    }
    function getMapDpi() {
        printMapService.getReferencedata('PrintMap_DPI').then(function (response) {
            if (response) {
                vm.printMapDPI = response[0].Value;
            }
        });
    }
    function getMapPerInch() {
        return 25.4;
        //printMapService.getReferencedata('PrintMap_mmPerInch').then(function (response) {
        //    if (response) {
        //        vm.printMapmmPerInch = response[0].Value;
        //    }
        //});
    }
    function getImageHeight() {
        printMapService.getReferencedata('PrintMap_ImageHeightmm').then(function (response) {
            vm.imageHeight = response;
        });
    }
    function getImageWidth() {
        printMapService.getReferencedata('PrintMap_ImageWidthmm').then(function (response) {
            vm.imageWidth = response;
        });
    }
    function getMapImageWidth(printSize) {
        var categoryName;
        switch (printSize) {
            case 'A0':
                categoryName = 'PrintMap_ImageWidthmm_A0';
                break;
            case 'A1':
                categoryName = 'PrintMap_ImageWidthmm_A1';
                break;
            case 'A2':
                categoryName = 'PrintMap_ImageWidthmm_A2';
                break;
            case 'A3':
                categoryName = 'PrintMap_ImageWidthmm_A3';
                break;
            case 'A4':
                categoryName = 'PrintMap_ImageWidthmm_A4';
                break;
        }

        var result = vm.imageWidth.filter(function (e) {
            return (e.Name === categoryName);
        });
        return result[0].Value;
    }
    function getMapImageHeight(printSize) {
        var categoryName;
        switch (printSize) {
            case 'A0':
                categoryName = 'PrintMap_ImageHeightmm_A0';
                break;
            case 'A1':
                categoryName = 'PrintMap_ImageHeightmm_A1';
                break;
            case 'A2':
                categoryName = 'PrintMap_ImageHeightmm_A2';
                break;
            case 'A3':
                categoryName = 'PrintMap_ImageHeightmm_A3';
                break;
            case 'A4':
                categoryName = 'PrintMap_ImageHeightmm_A4';
                break;
        }

        var result = vm.imageHeight.filter(function (e) {
            return (e.Name === categoryName);
        });
        return result[0].Value;
    }
}






