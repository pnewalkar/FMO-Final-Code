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
    vm.deliveryPointList = [{
        locality: "BN1 Dadar",
        addressGuid: 1,
        isPostioned: false, udprn: null
    },
                           {
                               locality: "BN2 Dadar",
                               addressGuid: 2,
                               isPostioned: false, udprn: null
                           },
                           {
                               locality: "BN3 Dadar",
                               addressGuid: 3,
                               isPostioned: false, udprn: null
                           }
    ];

    vm.positionedDeliveryPointList = $stateParams.positionedDeliveryPointList;
    vm.createDeliveryPoint = createDeliveryPoint;
    vm.positionedThirdPartyDeliveryPointList = $stateParams.positionedThirdPartyDeliveryPointList;

    $scope.$watch(function () { return coordinatesService.getCordinates() }, function (newValue, oldValue) {
        if (newValue[0] !== oldValue[0] || newValue[1] !== oldValue[1])
            openAlert();
    }, true);

    function toggle(item) {
        var idx = $filter('filter')(vm.deliveryPointList, { addressGuid: item.addressGuid });
        if (idx.length > 0) {
            vm.deliveryPointList.splice(idx, 1);

            var positionedDeliveryPointListTemp = [];

            if (vm.positionedDeliveryPointList != null) {
                positionedDeliveryPointListTemp = vm.positionedDeliveryPointList;
            }
            positionedDeliveryPointListTemp.push(item);
            vm.positionedDeliveryPointList = positionedDeliveryPointListTemp;
            setDP();
            //item.udprn = '54471821';
            //locateDeliveryPoint(item.udprn);
        }
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
            vm.positionedCoOrdinates.push(coordinatesService.getCordinates());
        }, function () {
        });
    };

    referenceData();

    function setDP() {
        var shape = mapToolbarService.getShapeForButton('point');
        $scope.$emit('mapToolChange', { "name": 'deliverypoint', "shape": shape, "enabled": true });
    }

    function savePositionedDeliveryPoint() {
        var coordinates = vm.positionedCoOrdinates;
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

            if (response && response == "Delivery Point created successfully") {
                setDeliveryPoint();
                vm.onCloseDeliveryPoint();
            }
            else {
                vm.errorMessageDisplay = true;
                vm.errorMessage = response;
            }
        });

    }

    function setDeliveryPoint() {
        if (vm.postalAddressData.udprn) {


            var buildingNumber = vm.nybaddress.buildingNumber != null && vm.nybaddress.buildingNumber !== undefined ? vm.nybaddress.buildingNumber : '';
            var buildingName = vm.nybaddress.buildingName != null && vm.nybaddress.buildingName !== undefined ? vm.nybaddress.buildingName : '';
            var subBuildingName = vm.nybaddress.subBuildingName != null && vm.nybaddress.subBuildingName !== undefined ? vm.nybaddress.subBuildingName : '';
            var street = vm.postalAddressData.thoroughfare != null && vm.postalAddressData.thoroughfare !== undefined ? vm.postalAddressData.thoroughfare : '';
            var postCode = vm.postalAddressData.postcode != null && vm.postalAddressData.postcode !== undefined ? vm.postalAddressData.postcode : '';
            var departmentName = vm.nybaddress.departmentName != null && vm.nybaddress.departmentName !== undefined ? vm.nybaddress.departmentName : '';
            var organisationName = vm.nybaddress.organisationName != null && vm.nybaddress.organisationName !== undefined ? vm.nybaddress.organisationName : '';

            var address = buildingNumber + ' ' + buildingName + ' ' + subBuildingName + ' ' + organisationName + ' ' + departmentName + ' ' + street + ' ' + postCode;
            var positionedThirdPartyDeliverypointObj = {
                locality: address, addressGuid: vm.postalAddressData.id
            };

            var positionedThirdPartyDeliveryPointListTemp = [];

            if (vm.positionedThirdPartyDeliveryPointList != null) {
                positionedThirdPartyDeliveryPointListTemp = vm.positionedThirdPartyDeliveryPointList;
            }


            positionedThirdPartyDeliveryPointListTemp.push(positionedThirdPartyDeliverypointObj);

            vm.positionedThirdPartyDeliveryPointList = positionedThirdPartyDeliveryPointListTemp;

            locateDeliveryPoint(vm.postalAddressData.udprn);

            $state.go("deliveryPoint", { positionedThirdPartyDeliveryPointList: vm.positionedThirdPartyDeliveryPointList })
        }
        else {
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


    function locateDeliveryPoint(selectedItem) {
        deliveryPointApiService.GetAddressLocation(selectedItem)
            .then(function (response) {
                var lat = response.features[0].geometry.coordinates[1];
                var long = response.features[0].geometry.coordinates[0];
                mapFactory.locateDeliveryPoint(long, lat);
            });
    }

    function onCloseDeliveryPoint() {
        $mdDialog.hide(vm.close);
    }

};
