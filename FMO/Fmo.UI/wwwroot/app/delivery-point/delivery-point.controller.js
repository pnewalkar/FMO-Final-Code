angular
    .module('deliveryPoint')
    .controller("DeliveryPointController",
    [
        'mapToolbarService',
        '$scope',
        '$mdDialog',
        'deliveryPointService',
        'deliveryPointApiService',
        'referencedataApiService',
        '$filter',
        'referenceDataConstants',
        '$timeout',
        'mapFactory',
        'coordinatesService',
        '$state',
        '$stateParams'
, DeliveryPointController])
function DeliveryPointController(
    mapToolbarService,    
    $scope,
    $mdDialog,
    deliveryPointService,
    deliveryPointApiService,
    referencedataApiService,
    $filter,
    referenceDataConstants,
    $timeout,
    mapFactory,
    coordinatesService,
    $state,
    $stateParams
) {
    var vm = this;
    vm.resultSet = resultSet;
    vm.querySearch = querySearch;
    vm.deliveryPoint = deliveryPoint;
    vm.openModalPopup = openModalPopup;
    vm.closeWindow = closeWindow;
    vm.OnChangeItem = OnChangeItem;
    vm.getPostalAddress = getPostalAddress;
    vm.getAddressLocation = getAddressLocation;
    vm.addAlias = addAlias;
    vm.removeAlias = removeAlias;
    vm.bindAddressDetails = bindAddressDetails;
    vm.savePositionedDeliveryPoint = savePositionedDeliveryPoint;
    vm.display = false;
    vm.disable = true;
    vm.openAlert = openAlert;
    vm.toggle = toggle;
    vm.alias = null;
    vm.exists = exists;
    vm.positionedCoOrdinates = [];
    vm.setOrganisation = setOrganisation;
    vm.onCloseDeliveryPoint = onCloseDeliveryPoint;
    vm.errorMessageDisplay = false;
    vm.BuildingName = "";
    vm.selectedItem = null;
    vm.deliveryPointList = $stateParams.deliveryPointList;
    vm.positionedSaveDeliveryPointList = [];
    

    vm.positionedDeliveryPointList = $stateParams.positionedDeliveryPointList;
    vm.createDeliveryPoint = createDeliveryPoint;
    vm.positionedThirdPartyDeliveryPointList = $stateParams.positionedThirdPartyDeliveryPointList;
    vm.positionedThirdPartyDeliveryPoint = [];
    vm.accessLink = accessLink;

    $scope.$watch(function () { return coordinatesService.getCordinates() }, function (newValue, oldValue) {
        if (newValue[0] !== oldValue[0] || newValue[1] !== oldValue[1])
            openAlert();
    }, true);

    function toggle(item) {
        vm.selectedItem = item;
        setDP();
    };

    function exists(item, list) {
        return list.indexOf(item) > -1;
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
                    udprn: vm.selectedItem.udprn, locality: vm.selectedItem.locality, addressGuid: vm.selectedItem.addressGuid, deliveryPointGuid: vm.selectedItem.deliveryPointGuid, xCoordinate: vm.positionedCoOrdinates[0][0], yCoordinate: vm.positionedCoOrdinates[0][1], latitude: latlong[1], longitude: latlong[0], rowversion: vm.selectedItem.rowversion
                };

                vm.positionedDeliveryPointList.push(positionedDeliveryPointListObj);
                
                $state.go("deliveryPoint", {
                    deliveryPointList : vm.deliveryPointList,
                    positionedDeliveryPointList: vm.positionedDeliveryPointList
                })

                var shape = mapToolbarService.getShapeForButton('point');
                $scope.$emit('mapToolChange', {
                    "name": 'select', "shape": shape, "enabled": true
                });
                accessLink();
            }
        }, function () {
        });
    };

    referenceData();

    function setDP() {
        var shape = mapToolbarService.getShapeForButton('point');
        $scope.$emit('mapToolChange', { "name": 'deliverypoint', "shape": shape, "enabled": true });
    }

    function savePositionedDeliveryPoint() {
        
        vm.positionedDeliveryPointList = $stateParams.positionedDeliveryPointList

        var deliveryPointModelDTO = {
            "ID": vm.positionedDeliveryPointList[0].deliveryPointGuid,
            "UDPRN": vm.positionedDeliveryPointList[0].udprn,
            "Latitude": vm.positionedDeliveryPointList[0].latitude,
            "Longitude": vm.positionedDeliveryPointList[0].longitude,
            "XCoordinate": vm.positionedDeliveryPointList[0].xCoordinate,
            "YCoordinate": vm.positionedDeliveryPointList[0].yCoordinate,
            "RowVersion": vm.positionedDeliveryPointList[0].rowversion
        }
        deliveryPointApiService.UpdateDeliverypoint(deliveryPointModelDTO).then(function (result) {
            vm.positionedDeliveryPointList = null;
            $state.go('deliveryPoint', { positionedDeliveryPointList: vm.positionedDeliveryPointList });
          
        });
    }

    function querySearch(query) {
        deliveryPointApiService.GetDeliveryPointsResultSet(query).then(function (response) {
            vm.results = response;
        });
    }

    function createDeliveryPoint() {

        var postalAddress = createDeliveryPointDTO();
        var addDeliveryPointDTO =
            {
                "PostalAddressDTO": postalAddress,
                "DeliveryPointDTO":
                {
                    "MultipleOccupancyCount": vm.mailvol,
                    "MailVolume": vm.multiocc,
                    "DeliveryPointAliasDTO": vm.items,
                    "DeliveryPointUseIndicator_GUID": vm.dpUse[0].id
                },
                "AddressLocationDTO": null
            };
        deliveryPointApiService.CreateDeliveryPoint(addDeliveryPointDTO).then(function (response) {

            if (response.message && response.message == "Delivery Point created successfully") {
                setDeliveryPoint(response.id, response.rowVersion, postalAddress, true);
                vm.onCloseDeliveryPoint();
            }
            else if (response.message && response.message == "Delivery Point created successfully without location") {
                setDeliveryPoint(response.id, response.rowVersion, postalAddress, false);
                setDP();
                vm.onCloseDeliveryPoint();
            }
            else {
              
                vm.errorMessageDisplay = true;
                vm.errorMessage = response.message;
            }
        });

    }

    function setDeliveryPoint(id, rowversion, postalAddress, hasLocation) {

        if (vm.nybaddress.udprn && hasLocation) {
            var buildingNumber = vm.nybaddress.buildingNumber != null && vm.nybaddress.buildingNumber !== angular.isUndefined(undefined)  ? vm.nybaddress.buildingNumber: '';
            var buildingName = vm.nybaddress.buildingName != null && vm.nybaddress.buildingName !== angular.isUndefined(undefined)  ? vm.nybaddress.buildingName: '';
            var subBuildingName = vm.nybaddress.subBuildingName != null && vm.nybaddress.subBuildingName !== angular.isUndefined(undefined)  ? vm.nybaddress.subBuildingName: '';
            var street = vm.nybaddress.thoroughfare != null && vm.nybaddress.thoroughfare !== angular.isUndefined(undefined)  ? vm.nybaddress.thoroughfare: '';
            var postCode = vm.nybaddress.postcode != null && vm.nybaddress.postcode !== angular.isUndefined(undefined)  ? vm.nybaddress.postcode: '';
            var departmentName = vm.nybaddress.departmentName != null && vm.nybaddress.departmentName !== angular.isUndefined(undefined)  ? vm.nybaddress.departmentName: '';
            var organisationName = vm.nybaddress.organisationName != null && vm.nybaddress.organisationName !== angular.isUndefined(undefined)  ? vm.nybaddress.organisationName: '';

            var address = buildingNumber + ' ' +buildingName + ' ' +subBuildingName + ' ' + organisationName + ' ' +departmentName + ' ' +street + ' ' +postCode;
            locateDeliveryPoint(vm.nybaddress.udprn, address, vm.nybaddress.id, id, rowversion);
        }
        else {

            var buildingNumber = vm.nybaddress.buildingNumber != null && vm.nybaddress.buildingNumber !== angular.isUndefined(undefined)  ? vm.nybaddress.buildingNumber: '';
            var buildingName = vm.nybaddress.buildingName != null && vm.nybaddress.buildingName !== angular.isUndefined(undefined)  ? vm.nybaddress.buildingName: '';
            var subBuildingName = vm.nybaddress.subBuildingName != null && vm.nybaddress.subBuildingName !== angular.isUndefined(undefined)  ? vm.nybaddress.subBuildingName: '';
            var street = postalAddress.Thoroughfare != null && postalAddress.Thoroughfare !== angular.isUndefined(undefined)  ? postalAddress.Thoroughfare : '';
            var postCode = postalAddress.Postcode != null && postalAddress.Postcode !== angular.isUndefined(undefined)  ? postalAddress.Postcode : '';
            var departmentName = vm.nybaddress.departmentName != null && vm.nybaddress.departmentName !== angular.isUndefined(undefined)  ? vm.nybaddress.departmentName: '';
            var organisationName = vm.nybaddress.organisationName != null && vm.nybaddress.organisationName !== angular.isUndefined(undefined)  ? vm.nybaddress.organisationName: '';

            var address = buildingNumber + ' ' +buildingName + ' ' +subBuildingName + ' ' +organisationName + ' ' +departmentName + ' ' +street + ' ' +postCode;
            
            manualDeliveryPointPosition(vm.nybaddress.udprn, address, vm.nybaddress.id, id, rowversion)
            setDP();
        }
    }

    function createDeliveryPointDTO() {
        if (!vm.nybaddress) {
            return {
                "OrganisationName": vm.nybaddress.organisationName,
                "DepartmentName": vm.nybaddress.departmentName,
                "BuildingName": vm.nybaddress.buildingName,
                "BuildingNumber": vm.nybaddress.buildingNumber,
                "SubBuildingName": vm.nybaddress.subBuildingName,
                "Thoroughfare": vm.postalAddressData.thoroughfare,
                "DependentLocality": vm.postalAddressData.dependentLocality,
                "Postcode": vm.postalAddressData.postcode,
                "PostTown": vm.postalAddressData.postTown,
                "PostcodeType": vm.postalAddressData.postcodeType
            }
        }
        else if (vm.nybaddress && !vm.nybaddress.id) {
            return {
                "OrganisationName": vm.nybaddress.organisationName,
                "DepartmentName": vm.nybaddress.departmentName,
                "BuildingName": vm.nybaddress.buildingName,
                "BuildingNumber": vm.nybaddress.buildingNumber,
                "SubBuildingName": vm.nybaddress.subBuildingName,
                "Thoroughfare": vm.postalAddressData.thoroughfare,
                "DependentLocality": vm.postalAddressData.dependentLocality,
                "Postcode": vm.postalAddressData.postcode,
                "PostTown": vm.postalAddressData.postTown,
                "PostcodeType": vm.postalAddressData.postcodeType
            }
        }
        else {
            return vm.nybaddress;
        }

    }



    function deliveryPoint() {
        var deliveryPointTemplate = deliveryPointService.deliveryPoint();
        vm.openModalPopup(deliveryPointTemplate);
    }

    function resultSet(query) {
        if (query.length >= 3) {
            querySearch(query);
        }
        else {
            vm.results = {
            };
            vm.resultscount = {
                0: {
                    count: 0
                }
            };
        }
    }



    function getPostalAddress(selectedItem) {
        var arrSelectedItem = selectedItem.split(',');
        var postCode;
        if (arrSelectedItem.length == 2) {
            postCode = arrSelectedItem[1].trim();
        }
        else {
            postCode = arrSelectedItem[0].trim();
        }

        deliveryPointApiService.GetAddressByPostCode(postCode).then(function (response) {
            vm.nybaddress = "";
            vm.postalAddressData = response;
            if (vm.postalAddressData) {
                vm.display = true;
                vm.disable = false;
            }
            else {
                vm.display = false;
                vm.disable = true;
            }
        });
    }

    function getAddressLocation(udprn) {
        deliveryPointApiService.GetAddressLocation(udprn)
                .then(function (response) {
                    vm.addressLocationData = response.data;
                });
    }

    function OnChangeItem(selectedItem) {
        if (selectedItem) {
            vm.searchText = selectedItem;
            getPostalAddress(selectedItem);
            vm.results = {
            };
        }
    }

    function openModalPopup(modalSetting) {
        var popupSetting = modalSetting;
        $mdDialog.show(popupSetting)
    }

    function closeWindow() {
        $mdDialog.hide(vm.close);
    }

    function referenceData() {
        referencedataApiService.getReferenceData().success(function (response) {
            vm.deliveryPointTypes = $filter('filter')(response, {
                categoryName: referenceDataConstants.DeliveryPointType
            });
            vm.dpUseType = $filter('filter')(response, { categoryName: referenceDataConstants.DeliveryPointUseIndicator })[0];
        });
    }

    vm.items = [];

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

    function bindAddressDetails() {

        deliveryPointApiService.GetPostalAddressByGuid(vm.notyetBuilt)
               .then(function (response) {
                   vm.nybaddress = response;
                   if (!(vm.nybaddress.organisationName)) {
                       vm.dpUse = $filter('filter')(vm.dpUseType.referenceDatas, {
                           displayText: "Residential"
                       });
                   }
                   else {
                       vm.dpUse = $filter('filter')(vm.dpUseType.referenceDatas, {
                           displayText: "Commercial"
                       });
                   }
               });
    }

    function setOrganisation(val) {
        if (val) {
            vm.dpUse = $filter('filter')(vm.dpUseType.referenceDatas, {
                displayText: "Commercial"
            });
        }
        else {
            vm.dpUse = $filter('filter')(vm.dpUseType.referenceDatas, {
                displayText: "Residential"
            });
        }
    }


    function locateDeliveryPoint(udprn, locality, addressGuid, deliveryPointGuid, rowversion) {
        deliveryPointApiService.GetAddressLocation(udprn)
            .then(function (response) {
                if (response.features.length > 0) {

                    var lat = response.features[0].geometry.coordinates[1];
                    var long = response.features[0].geometry.coordinates[0];


                    var positionedThirdPartyDeliverypointObj = {
                        udprn: udprn, locality: locality, addressGuid: addressGuid, deliveryPointGuid: deliveryPointGuid, xCoordinate: long, yCoordinate: lat, latitude: response.features[0].properties.latitude, longitude: response.features[0].properties.longitude, rowversion: rowversion
                    };

                    vm.positionedThirdPartyDeliveryPoint.push(positionedThirdPartyDeliverypointObj);

                    var positionedThirdPartyDeliveryPointListTemp = [];

                    if (vm.positionedThirdPartyDeliveryPointList != null) {
                        positionedThirdPartyDeliveryPointListTemp = vm.positionedThirdPartyDeliveryPointList;
                    }

                    positionedThirdPartyDeliveryPointListTemp.push(positionedThirdPartyDeliverypointObj);

                    vm.positionedThirdPartyDeliveryPointList = positionedThirdPartyDeliveryPointListTemp;

                    $state.go("deliveryPoint", { positionedThirdPartyDeliveryPointList: vm.positionedThirdPartyDeliveryPointList })

                    mapFactory.locateDeliveryPoint(long, lat);
                }
            });
    }

    function manualDeliveryPointPosition(udprn, locality, addressGuid, deliveryPointGuid, rowversion) {
        if (udprn == null) {
            var deliveryPointListObj = {
                        udprn: udprn, locality: locality, addressGuid: addressGuid, deliveryPointGuid: deliveryPointGuid, xCoordinate: null, yCoordinate: null, latitude: null, longitude: null, rowversion: rowversion
            };
            if (vm.deliveryPointList == null)
            {
                vm.deliveryPointList = [];
            }
            vm.deliveryPointList.push(deliveryPointListObj);
            $state.go("deliveryPoint", { deliveryPointList: vm.deliveryPointList })
        }
    }

    function onCloseDeliveryPoint() {
        $mdDialog.hide(vm.close);
    }

 function accessLink(selectedDeliveryUnit) {
        vm.contextTitle = "Access Link";
        $state.go("accessLink");
    }

};
