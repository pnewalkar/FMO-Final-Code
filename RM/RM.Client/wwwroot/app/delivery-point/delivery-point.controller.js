angular
    .module('deliveryPoint')
    .controller("DeliveryPointController", DeliveryPointController)

DeliveryPointController.$inject = [
        'mapToolbarService',
        'mapService',
        '$scope',
        '$mdDialog',
        'popUpSettingService',
        'deliveryPointAPIService',
        '$filter',
        'mapFactory',
        'coordinatesService',
        'guidService',
        '$state',
        '$stateParams',
        'deliveryPointService',
        '$rootScope',
        'GlobalSettings'];

function DeliveryPointController(
    mapToolbarService,
    mapService,
    $scope,
    $mdDialog,
    popUpSettingService,
    deliveryPointAPIService,
    $filter,
    mapFactory,
    coordinatesService,
    guidService,
    $state,
    $stateParams,
    deliveryPointService,
    $rootScope,
    GlobalSettings
) {
    var vm = this;
    vm.resultSet = resultSet;
    vm.deliveryPoint = deliveryPoint;
    vm.closeWindow = closeWindow;
    vm.OnChangeItem = OnChangeItem;
    vm.getAddressLocation = getAddressLocation;
    vm.addAlias = addAlias;
    vm.removeAlias = removeAlias;
    vm.bindAddressDetails = bindAddressDetails;
    vm.savePositionedDeliveryPoint = savePositionedDeliveryPoint;
    vm.openAlert = openAlert;
    vm.toggle = toggle;
    vm.setOrganisation = setOrganisation;
    vm.getCommaSeparatedVale = getCommaSeparatedVale;
    vm.createDeliveryPoint = createDeliveryPoint;
    vm.Ok = Ok;
    vm.initialize = initialize;

    vm.positionedThirdPartyDeliveryPointList = $stateParams.positionedThirdPartyDeliveryPointList;
    vm.positionedDeliveryPointList = $stateParams.positionedDeliveryPointList;
    vm.deliveryPointList = $stateParams.deliveryPointList;
    vm.positionedThirdPartyDeliveryPoint = [];
    vm.isError = false;
    vm.isDisable = false;
    vm.positionedSaveDeliveryPointList = [];
    vm.defaultNYBValue = "00000000-0000-0000-0000-000000000000";
    vm.errorMessageDisplay = false;
    vm.selectedItem = null;
    vm.positionedCoOrdinates = [];
    vm.alias = null;
    vm.display = false;
    vm.disable = true;
    vm.rangeOptionsSelected = "Odds";
    vm.items = [];
    vm.dpIsChecked = false;
    vm.selectedType = null;
    vm.single = GlobalSettings.single;
    vm.range = GlobalSettings.range;
    vm.subBuilding = GlobalSettings.subBuilding;
    vm.numberInName = GlobalSettings.numberInName;

    $scope.$watch(function () { return coordinatesService.getCordinates() }, function (newValue, oldValue) {
        if (newValue !== '' && (newValue[0] !== oldValue[0] || newValue[1] !== oldValue[1]))
            openAlert();
    }, true);
    vm.initialize();

    $scope.$on("showDialog", function (event, args) {
        if (args === "deliveryPoint") {
            deliveryPoint();
        }
    });
    function initialize() {
        deliveryPointService.initialize().then(function (response) {
            vm.deliveryPointTypes = response.DeliveryPointTypes;
            vm.dpUseType = response.DpUseType;
            vm.subBuildingTypes = response.SubBuildingTypes;
            vm.rangeOptions = response.RangeOptions;
        });
    }

    function deliveryPoint() {
        deliveryPointService.openModalPopup(popUpSettingService.deliveryPoint(vm));
        $scope.$emit("dialogOpen", "deliveryPoint");
    }

    function closeWindow() {
        vm.hide = false;
        vm.display = false;
        vm.searchText = "";
        vm.mailvol = "";
        vm.multiocc = "";
        deliveryPointService.closeModalPopup();
    }

    function resultSet(query) {
        deliveryPointService.resultSet(query).then(function (response) {
            vm.results = response;
        });
    }

    function OnChangeItem(selectedItem) {
        if (selectedItem) {
            vm.dpUse = "";
            vm.routeId = "";
            vm.notyetBuilt = "";
            vm.searchText = selectedItem;
            vm.results = {};
            deliveryPointService.getPostalAddress(selectedItem).then(function (response) {
                vm.addressDetails = response.postalAddressData;
                vm.nybAddressDetails = response.postalAddressData.nybAddressDetails;
                vm.routeDetails = response.postalAddressData.routeDetails;
                vm.display = response.display;
                vm.selectedValue = response.selectedValue;
            });
        }
    }

    function bindAddressDetails() {
        if (vm.notyetBuilt !== vm.defaultNYBValue) {
            deliveryPointService.bindAddressDetails(vm.notyetBuilt)
                   .then(function (response) {
                       vm.addressDetails = response;
                       setOrganisation();
                   });
        }
        else {
            vm.addressDetails.id = vm.defaultNYBValue;
            vm.addressDetails.udprn = "";
            vm.addressDetails.buildingNumber = "";
            vm.addressDetails.buildingName = "";
            vm.addressDetails.subBuildingName = "";
            vm.addressDetails.organisationName = "";
            vm.addressDetails.departmentName = "";
            vm.dpUse = "";
        }
    }

    function setOrganisation() {
        deliveryPointService.setOrganisation(vm.addressDetails, vm.dpUseType).then(function (response) {
            vm.dpUse = response.dpUse;
            vm.selectedDPUse = response.selectedDPUse;
        });
    }

    function toggle(item) {
        vm.dpIsChecked = !vm.dpIsChecked;
        if (vm.dpIsChecked === true) {
            vm.selectedItem = item;
            setDP();
        }
        else {
            resetDP();
        }
    };

    function openAlert() {
        var confirm =
          $mdDialog.confirm()
            .clickOutsideToClose(true)
            .title('Confirm Position')
            .textContent('Are you sure you want to position this point here?')
            .ariaLabel('Left to right demo')
            .ok('Yes')
            .cancel('No')

        $mdDialog.show(confirm).then(function () {
            var idx = $filter('filter')(vm.deliveryPointList, { addressGuid: vm.selectedItem.addressGuid });
            if (idx.length > 0) {
                vm.deliveryPointList.splice(idx, 1);

                vm.positionedDeliveryPointList = [];
                vm.positionedCoOrdinates.push(coordinatesService.getCordinates());
                var latlong = ol.proj.transform(vm.positionedCoOrdinates[0], 'EPSG:27700', 'EPSG:4326');
                var positionedDeliveryPointListObj = {
                    udprn: vm.selectedItem.udprn, locality: vm.selectedItem.locality, addressGuid: vm.selectedItem.addressGuid, id: vm.selectedItem.id, xCoordinate: vm.positionedCoOrdinates[0][0], yCoordinate: vm.positionedCoOrdinates[0][1], latitude: latlong[1], longitude: latlong[0], rowversion: vm.selectedItem.rowversion
                };

                vm.positionedDeliveryPointList.push(positionedDeliveryPointListObj);

                $state.go("deliveryPoint", {
                    deliveryPointList: vm.deliveryPointList,
                    positionedDeliveryPointList: vm.positionedDeliveryPointList
                })

                var shape = mapToolbarService.getShapeForButton('point');
                $scope.$emit('mapToolChange', {
                    "name": 'select', "shape": shape, "enabled": true
                });
            }
        }, function () {
            mapService.clearDrawingLayer(true);
        });
    };

    function setDP() {
        var shape = mapToolbarService.getShapeForButton('point');
        $scope.$emit('mapToolChange', { "name": 'deliverypoint', "shape": shape, "enabled": true });
    }

    function resetDP() {
        var shape = mapToolbarService.getShapeForButton('point');
        $scope.$emit('mapToolChange', { "name": 'select', "shape": shape, "enabled": true });
    }

    function savePositionedDeliveryPoint() {
        vm.isOnceClicked = true;
        vm.positionedDeliveryPointList = $stateParams.positionedDeliveryPointList
        deliveryPointService.UpdateDeliverypoint(vm.positionedDeliveryPointList)
        .finally(function () {
            vm.isOnceClicked = false;
        });
        vm.positionedDeliveryPointList = null;
    }

    function createDeliveryPoint() {
        vm.isOnceClicked = true;

        var addDeliveryPointDTO =
            {
                "PostalAddressDTO": vm.addressDetails,
                "DeliveryPointDTO":
                {
                    "MultipleOccupancyCount": vm.mailvol,
                    "MailVolume": vm.multiocc,
                    "DeliveryPointAliasDTO": vm.items,
                    "DeliveryPointUseIndicator_GUID": vm.dpUse[0].id,
                    "DeliveryRoute_Guid": vm.routeId
                },
                "AddressLocationDTO": null
            };
        deliveryPointAPIService.CreateDeliveryPoint(addDeliveryPointDTO).then(function (response) {
            if (response.message && (response.message == "Delivery Point created successfully" || response.message == "Delivery Point created successfully without access link")) {
                setDeliveryPoint(response.id, response.rowVersion, vm.addressDetails, true);
                mapFactory.setDeliveryPoint(response.xCoordinate, response.yCoordinate);
                guidService.setGuid(response.id);
                mapFactory.setAccessLink();
                vm.closeWindow();
                vm.hide = true;
            }
            else if (response.message && response.message == "Delivery Point created successfully without location") {
                setDeliveryPoint(response.id, response.rowVersion, vm.addressDetails, false);
                //    setDP();
                vm.hide = true;
                vm.closeWindow();
            }
            else {
                vm.isError = true;
                vm.isDisable = true;
                vm.errorMessage = response.message;
                vm.errorMessageTitle = "Duplicates found";
            }
            $rootScope.$broadcast('disablePrintMap', {
                disable: true
            });
        }).finally(function () {
            vm.isOnceClicked = false;
        });
    }

    function setDeliveryPoint(id, rowversion, postalAddress, hasLocation) {
        var address = deliveryPointService.isUndefinedOrNull(postalAddress.buildingNumber)
            + ' ' + deliveryPointService.isUndefinedOrNull(postalAddress.buildingName)
            + ' ' + deliveryPointService.isUndefinedOrNull(postalAddress.subBuildingName)
            + ' ' + deliveryPointService.isUndefinedOrNull(postalAddress.organisationName)
            + ' ' + deliveryPointService.isUndefinedOrNull(postalAddress.departmentName)
            + ' ' + deliveryPointService.isUndefinedOrNull(postalAddress.thoroughfare)
            + ' ' + deliveryPointService.isUndefinedOrNull(postalAddress.postcode);

        if (vm.addressDetails.udprn && hasLocation) {
            locateDeliveryPoint(vm.addressDetails.udprn, address, vm.addressDetails.id, id, rowversion);
        }
        else {
            manualDeliveryPointPosition(vm.addressDetails.udprn, address, vm.addressDetails.id, id, rowversion)
            //  setDP();
        }
    }

    function getAddressLocation(udprn) {
        deliveryPointAPIService.GetAddressLocation(udprn)
                .then(function (response) {
                    vm.addressLocationData = response.data;
                });
    }

    function addAlias() {
        vm.items.push({
            Preferred: false,
            DPAlias: vm.alias
        });
        vm.alias = "";
    };

    function removeAlias() {
        var lastItem = vm.items.length - 1;
        vm.items.splice(lastItem);
    }

    function locateDeliveryPoint(udprn, locality, addressGuid, deliveryPointGuid, rowversion) {
        deliveryPointAPIService.GetAddressLocation(udprn)
            .then(function (response) {
                if (response.features.length > 0) {
                    var lat = response.features[0].geometry.coordinates[1];
                    var long = response.features[0].geometry.coordinates[0];

                    var positionedThirdPartyDeliverypointObj = {
                        udprn: udprn, locality: locality, addressGuid: addressGuid, id: deliveryPointGuid, xCoordinate: long, yCoordinate: lat, latitude: response.features[0].properties.latitude, longitude: response.features[0].properties.longitude, rowversion: rowversion
                    };

                    vm.positionedThirdPartyDeliveryPoint.push(positionedThirdPartyDeliverypointObj);

                    var positionedThirdPartyDeliveryPointListTemp = [];

                    if (vm.positionedThirdPartyDeliveryPointList != null) {
                        positionedThirdPartyDeliveryPointListTemp = vm.positionedThirdPartyDeliveryPointList;
                    }

                    positionedThirdPartyDeliveryPointListTemp.push(positionedThirdPartyDeliverypointObj);

                    vm.positionedThirdPartyDeliveryPointList = positionedThirdPartyDeliveryPointListTemp;

                    $state.go("deliveryPoint", {
                        positionedThirdPartyDeliveryPointList: vm.positionedThirdPartyDeliveryPointList,
                        /* hide: true*/
                    })

                    mapFactory.locateDeliveryPoint(long, lat);
                }
            });
    }

    function manualDeliveryPointPosition(udprn, locality, addressGuid, deliveryPointGuid, rowversion) {
        var deliveryPointListObj = {
            udprn: udprn, locality: locality, addressGuid: addressGuid, id: deliveryPointGuid, xCoordinate: null, yCoordinate: null, latitude: null, longitude: null, rowversion: rowversion
        };
        if (vm.deliveryPointList == null) {
            vm.deliveryPointList = [];
        }
        vm.deliveryPointList.push(deliveryPointListObj);
        $state.go("deliveryPoint", {
            deliveryPointList: vm.deliveryPointList,
            /*hide: true*/
        })
    }

    function getCommaSeparatedVale(value1, value2) {
        if (value1 && value2) {
            return value1 + ", " + value2;
        }
        else if (value1 && !value2) {
            return value1;
        }
        else {
            return value2;
        }
    }

    function Ok() {
        vm.isError = false;
        vm.isDisable = false;
    }
};