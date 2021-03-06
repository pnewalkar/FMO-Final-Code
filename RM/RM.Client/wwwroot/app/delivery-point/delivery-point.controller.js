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
        'CommonConstants',
        '$rootScope',
        '$translate',
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
    CommonConstants,
    $rootScope,
    $translate,
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
    vm.setRangeValidation = setRangeValidation;
    vm.createDeliveryPointsRange = createDeliveryPointsRange;
    vm.setDeliveryPointUseIndicator = setDeliveryPointUseIndicator;

    vm.isMultiple = $stateParams.isMultiple;
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
    vm.postalAddressAliases = [];
    vm.rangeOptionsSelected = GlobalSettings.defaultRangeOption;
    vm.dpIsChecked = false;
    vm.selectedType = null;
    vm.single = GlobalSettings.single;
    vm.range = GlobalSettings.range;
    vm.subBuilding = GlobalSettings.subBuilding;
    vm.numberInName = GlobalSettings.numberInName;
    vm.displayRangeFromMessage = false;
    vm.displayRangeToMessage = false;
    vm.maintainState = false;
    vm.defaultDeliveryType = GlobalSettings.single;    
    vm.selectedItems = [];
    vm.isReadyForPositioning = false;

    $scope.$watchCollection(function () { return coordinatesService.getCordinates() }, function (newValue, oldValue) {
        if (newValue !== '' && (newValue[0] !== oldValue[0] || newValue[1] !== oldValue[1]))
            if (vm.isReadyForPositioning) {
                openAlert();
            }
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
        vm.rangeOptionsSelected = GlobalSettings.defaultRangeOption;
        deliveryPointService.closeModalPopup();
        vm.results = {};
        vm.postalAddressAliases = [];
        vm.alias = "";
        vm.maintainState = false;
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

        if (!vm.maintainState) {
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
            vm.rangeOptionsSelected = GlobalSettings.defaultRangeOption;
            vm.selectedType = vm.single;
            vm.rangeFrom = "";
            vm.rangeTo = "";
            vm.mailvol = "";
            vm.multiocc = "";
            vm.subBuildingType = "";
        }
        else {
            vm.notyetBuilt = vm.selectedNYBDetail;
            vm.selectedType = vm.selectedDeliveryPointType
            vm.defaultDeliveryType = vm.selectedDeliveryPointType;
            vm.maintainState = false;
        }
    }

    function setOrganisation() {
        if (vm.selectedType === vm.single && !angular.isUndefined(vm.addressDetails)) {
            deliveryPointService.setOrganisation(vm.addressDetails, vm.dpUseType).then(function (response) {
                vm.dpUse = response.dpUse;
                vm.selectedDPUse = response.selectedDPUse;
            });
        }
    }

    function setDeliveryPointUseIndicator()
    {
        vm.dpUse = vm.dpUseType;
        vm.selectedDPUse = "";
    }

    function toggle(item) {
        item.isChecked = !item.isChecked;

        if (item.isChecked) {
            vm.selectedItems.push(item);
        }
        else {
            if (vm.selectedItems.length > 0) {
                var filter = $filter('filter')(vm.selectedItems, { id: item.id });

                if (filter.length > 0)
                    vm.selectedItems.splice(filter, 1);
            }            
        }

        if (vm.selectedItems && vm.selectedItems.length === 1) {
            setDP();
        }
        else if (vm.selectedItems && vm.selectedItems.length === 0) {
            resetDP();
        }

        if (vm.positionedDeliveryPointList) {
            if (vm.positionedDeliveryPointList.length > 0) {
                angular.forEach(vm.positionedDeliveryPointList, function (positionedDeliveryPoint, count) {
                    positionedDeliveryPoint.isChecked = false;
                })
            }
        }
    };

    function openAlert() {
        var confirm = undefined;

        if (vm.selectedItems.length == 1) {
            confirm = $mdDialog.confirm()
              .clickOutsideToClose(true)
              .title('Confirm Position')
              .textContent('Are you sure you want to position this point here?')
              .ariaLabel('Position Delivery Point')
              .ok('Yes')
              .cancel('No')
        }
        else {
            confirm = $mdDialog.confirm()
              .clickOutsideToClose(true)
              .title('Confirm Position')
              .textContent('Are you sure you want to position these points here?')
              .ariaLabel('Position Delivery Points')
              .ok('Yes')
              .cancel('No')
        }

        $mdDialog.show(confirm).then(function () {            
            angular.forEach(vm.selectedItems, function (selectedItem, count) {
                var idx = getPositionedDPIndex(selectedItem);
                if (selectedItem) {
                    vm.deliveryPointList.splice(idx, 1);

                    if (!vm.positionedDeliveryPointList)
                        vm.positionedDeliveryPointList = [];
                    vm.positionedCoOrdinates.push(coordinatesService.getCordinates());
                    var latlong = ol.proj.transform(vm.positionedCoOrdinates[0], 'EPSG:27700', 'EPSG:4326');
                    var positionedDeliveryPointListObj = {
                        udprn: selectedItem.udprn,
                        locality: selectedItem.locality,
                        addressGuid: selectedItem.addressGuid,
                        id: selectedItem.id,
                        xCoordinate: vm.positionedCoOrdinates[0][0],
                        yCoordinate: vm.positionedCoOrdinates[0][1],
                        latitude: latlong[1],
                        longitude: latlong[0],
                        rowversion: selectedItem.rowversion,
                        isChecked: true
                    };

                    vm.positionedDeliveryPointList.push(positionedDeliveryPointListObj);
                }
            });

            $state.go("deliveryPoint", {
                deliveryPointList: vm.deliveryPointList,
                positionedDeliveryPointList: vm.positionedDeliveryPointList
            })

            resetDP();

            mapService.clearPlacedDP();
            vm.selectedItems = [];

        }
        , function () {
            mapService.removePlacedDP();
        }
        );
    };

    function getPositionedDPIndex(item) {

        for (var itemIndex = 0; itemIndex < vm.deliveryPointList.length; itemIndex++) {
            if (item.id === vm.deliveryPointList[itemIndex].id) {
                return itemIndex;
            }
        }
    }


    function setDP() {
        var shape = mapToolbarService.getShapeForButton('point');
        $scope.$emit('mapToolChange', { "name": 'deliverypoint', "shape": shape, "enabled": true });
        $scope.$emit('setSelectedButton', { "name": 'point' });
        vm.isReadyForPositioning = true;
    }

    function resetDP() {
        var shape = mapToolbarService.getShapeForButton('point');
        $scope.$emit('mapToolChange', { "name": 'select', "shape": shape, "enabled": true });
        $scope.$emit('setSelectedButton', { "name": 'select' });
        vm.isReadyForPositioning = false;
    }

    function savePositionedDeliveryPoint() {
        vm.isMultiple = $stateParams.isMultiple;

        if (vm.isMultiple) {
            vm.isOnceClicked = true;
            vm.positionedDeliveryPointList = $stateParams.positionedDeliveryPointList
            deliveryPointService.UpdateDeliverypointForRange(vm.positionedDeliveryPointList)
            .finally(function () {
                vm.isOnceClicked = false;
                mapService.clearDrawingLayer(true);
            });
            vm.positionedDeliveryPointList = null;
        }
        else {
            vm.isOnceClicked = true;
            vm.positionedDeliveryPointList = $stateParams.positionedDeliveryPointList
            deliveryPointService.UpdateDeliverypoint(vm.positionedDeliveryPointList)
            .finally(function () {
                vm.isOnceClicked = false;
                mapService.clearDrawingLayer(true);
            });
            vm.positionedDeliveryPointList = null;
        }
    }

    function createDeliveryPoint() {
        vm.isOnceClicked = true;
        var addDeliveryPointDTO = getDeliveryPointDTO();

        deliveryPointAPIService.CreateDeliveryPoint(addDeliveryPointDTO).then(function (response) {
            if (response.isMultiple) {
                var title = "Duplicates Found";
                var css = "info-dialog";

                if (response.hasAllDuplicates) {
                    $rootScope.$broadcast("InfoPopup", response.message, title, css);
                    vm.maintainState = true;
                    vm.selectedNYBDetail = vm.notyetBuilt;
                    vm.selectedDeliveryPointType = vm.selectedType;
                }
                else if (response.hasDuplicates) {
                    saveDeliveryPointsPartially(response.message, response.postalAddressDTOs, title, css);
                }
                else {
                    angular.forEach(response.createDeliveryPointModelDTOs, function (createDeliveryPointModelDTO, count) {
                        setDeliveryPoint(createDeliveryPointModelDTO.id, createDeliveryPointModelDTO.rowVersion, createDeliveryPointModelDTO.postalAddressDTO, createDeliveryPointModelDTO.isAddressLocationAvailable, response.isMultiple);
                    });
                    vm.closeWindow();
                    vm.hide = true;
                }
            }
            else {
                if (response.message && (response.message == "Delivery Point created successfully" || response.message == "Delivery Point created successfully without access link")) {
                    setDeliveryPoint(response.id, response.rowVersion, vm.addressDetails, true, response.isMultiple);
                    mapService.setDeliveryPoint(response.xCoordinate, response.yCoordinate);
                    guidService.setGuid(response.id);
                    mapFactory.setAccessLink();
                    mapService.refreshLayers();
                    vm.closeWindow();
                    vm.hide = true;
                }
                else if (response.message && response.message == "Delivery Point created successfully without location") {
                    setDeliveryPoint(response.id, response.rowVersion, vm.addressDetails, false, response.isMultiple);                    
                    vm.hide = true;
                    mapService.refreshLayers();
                    vm.closeWindow();
                }
                else {
                    vm.isError = true;
                    vm.isDisable = true;
                    vm.errorMessage = response.message;
                    vm.errorMessageTitle = "Duplicates found";
                }
            }
            $rootScope.$broadcast('disablePrintMap', {
                disable: true
            });
        }).finally(function () {
            vm.isOnceClicked = false;
        });
    }

    function getDeliveryPointDTO() {
        var addDeliveryPointDTO;
        if (vm.selectedType === vm.single) {
            addDeliveryPointDTO =
              {
                  "PostalAddressDTO": vm.addressDetails,
                  "DeliveryPointDTO":
                  {
                      "MultipleOccupancyCount": vm.multiocc,
                      "MailVolume": vm.mailvol,
                      "DeliveryPointAliasDTO": vm.items,
                      "DeliveryPointUseIndicator_GUID": vm.dpUse[0].id,
                      "DeliveryRoute_Guid": vm.routeId
                  },
                  "AddressLocationDTO": null,
                  "DeliveryPointType": vm.selectedType,
                  "PostalAddressAliasDTOs": vm.postalAddressAliases
              };
        }
        else {
            addDeliveryPointDTO =
                          {
                              "PostalAddressDTO": vm.addressDetails,
                              "DeliveryPointDTO":
                              {
                                  "MultipleOccupancyCount": vm.multiocc,
                                  "MailVolume": vm.mailvol,
                                  "DeliveryPointAliasDTO": vm.items,
                                  "DeliveryPointUseIndicator_GUID": vm.dpUse[0].id,
                                  "DeliveryRoute_Guid": vm.routeId
                              },
                              "AddressLocationDTO": null,
                              "PostalAddressAliasDTOs": vm.postalAddressAliases,
                              "DeliveryPointType": vm.selectedType,
                              "RangeType": vm.rangeOptionsSelected,
                              "FromRange": vm.rangeFrom,
                              "ToRange": vm.rangeTo,
                              "SubBuildingType": vm.subBuildingType,
                          };
        }
        return addDeliveryPointDTO;
    }

    function saveDeliveryPointsPartially(message, postalAddressDetails, title, css) {
        var alert = $mdDialog.alert()
            .parent()
            .clickOutsideToClose(false)
            .title($translate.instant(title))
            .textContent(message)
            .ariaLabel('Duplicates Found')
            .css(css)
            .ok($translate.instant('GENERAL.BUTTONS.OK'))

        $mdDialog.show(alert)
            .then(function () {
                createDeliveryPointsRange(postalAddressDetails);
            });
    };

    function setDeliveryPoint(id, rowversion, postalAddress, hasLocation, isMultiple) {
        if (vm.selectedDPUse.value === CommonConstants.DpUseType.Residential) {
            var address = deliveryPointService.isUndefinedOrNull(postalAddress.buildingNumber)
                        + ' ' + deliveryPointService.isUndefinedOrNull(postalAddress.buildingName)
                        + ' ' + deliveryPointService.isUndefinedOrNull(postalAddress.subBuildingName)
                        + ' ' + deliveryPointService.isUndefinedOrNull(postalAddress.thoroughfare)
                        + ' ' + deliveryPointService.isUndefinedOrNull(postalAddress.postcode);
        }
        else if (vm.selectedDPUse.value === CommonConstants.DpUseType.Organisation) {
            var address = deliveryPointService.isUndefinedOrNull(postalAddress.buildingNumber)
                        + ' ' + deliveryPointService.isUndefinedOrNull(postalAddress.buildingName)
                        + ' ' + deliveryPointService.isUndefinedOrNull(postalAddress.subBuildingName)
                        + ' ' + deliveryPointService.isUndefinedOrNull(postalAddress.organisationName)
                        + ' ' + deliveryPointService.isUndefinedOrNull(postalAddress.departmentName)
                        + ' ' + deliveryPointService.isUndefinedOrNull(postalAddress.thoroughfare)
                        + ' ' + deliveryPointService.isUndefinedOrNull(postalAddress.postcode);
        }
        if (vm.addressDetails.udprn && hasLocation) {
            locateDeliveryPoint(vm.addressDetails.udprn, address, vm.addressDetails.id, id, rowversion);
        }
        else {
            manualDeliveryPointPosition(vm.addressDetails.udprn, address, vm.addressDetails.id, id, rowversion, isMultiple);
        }
    }

    function getAddressLocation(udprn) {
        deliveryPointAPIService.GetAddressLocation(udprn)
                .then(function (response) {
                    vm.addressLocationData = response.data;
                });
    }

    function addAlias() {
        if (vm.alias !== "" && vm.alias !== null && angular.isDefined(vm.alias)) {
            vm.postalAddressAliases.push({
                PreferenceOrderIndex: 0,
                AliasName: vm.alias
            });
            vm.alias = "";
            vm.isAliasDisabled = true;
        }
    };

    function removeAlias() {
        //var lastItem = vm.postalAddressAliases.length - 1;
        //vm.postalAddressAliases.splice(lastItem);
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
                        positionedThirdPartyDeliveryPointList: vm.positionedThirdPartyDeliveryPointList
                    })

                    mapFactory.locateDeliveryPoint(long, lat);
                }
            });
    }

    function manualDeliveryPointPosition(udprn, locality, addressGuid, deliveryPointGuid, rowversion, isMultiple) {
        var deliveryPointListObj = {
            udprn: udprn,
            locality: locality,
            addressGuid: addressGuid,
            id: deliveryPointGuid,
            xCoordinate: null,
            yCoordinate: null,
            latitude: null,
            longitude: null,
            rowversion: rowversion,
            isChecked: false
        };
        if (vm.deliveryPointList == null) {
            vm.deliveryPointList = [];
        }
        vm.deliveryPointList.push(deliveryPointListObj);
        $state.go("deliveryPoint", {
            deliveryPointList: vm.deliveryPointList,
            isMultiple: isMultiple
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

    function setRangeValidation(rangeFrom, rangeTo, rangeType) {
        if (parseInt(rangeFrom) > parseInt(rangeTo)) {
            if (rangeType == "RangeFrom") {
                vm.displayRangeFromMessage = true;
                vm.displayRangeToMessage = false;
            }
            else {
                vm.displayRangeFromMessage = false;
                vm.displayRangeToMessage = true;
            }
        }
        else {
            vm.displayRangeFromMessage = false;
            vm.displayRangeToMessage = false;
        }
    }

    function createDeliveryPointsRange(postalAddressDetails) {

        deliveryPointService.createDeliveryPointsRange(postalAddressDetails)
                   .then(function (response) {
                       angular.forEach(response.createDeliveryPointModelDTOs, function (createDeliveryPointModelDTO, count) {
                           vm.closeWindow();
                           setDeliveryPoint(createDeliveryPointModelDTO.id, createDeliveryPointModelDTO.rowVersion, createDeliveryPointModelDTO.postalAddressDTO, createDeliveryPointModelDTO.isAddressLocationAvailable);
                       });
                   });
    }

};