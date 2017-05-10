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
    vm.onBlur = onBlur;
    vm.bindAddressDetails = bindAddressDetails;
    vm.savePositionedDeliveryPoint = savePositionedDeliveryPoint;
    vm.display = false;
    vm.disable = true;
    vm.openAlert = openAlert;
    vm.toggle = toggle;
    vm.alias = null;
    vm.exists = exists;
    vm.positionedCoOrdinates = [];
    vm.selectedItem = null;
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

                var positionedDeliveryPointListTemp = [];

                if (vm.positionedDeliveryPointList != null) {
                    positionedDeliveryPointListTemp = vm.positionedDeliveryPointList;
                }
                positionedDeliveryPointListTemp.push(vm.selectedItem);
                vm.positionedDeliveryPointList = positionedDeliveryPointListTemp;
                vm.positionedCoOrdinates.push(coordinatesService.getCordinates());
                var shape = mapToolbarService.getShapeForButton('point');
                $scope.$emit('mapToolChange', {
                    "name": 'select', "shape": shape, "enabled": true
                });
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
                    "DeliveryPointUseIndicator_GUID": '178EDCAD-9431-E711-83EC-28D244AEF9ED'
                }, "AddressLocationDTO": null
            };
        deliveryPointApiService.CreateDeliveryPoint(addDeliveryPointDTO).then(function (response) {
            vm.nybaddress = "";
            vm.errorMessage = response;

            if (vm.postalAddressData.udprn) {
                getAddressLocation(vm.postalAddressData.udprn);                

                var buildingNumber = vm.nybaddress.buildingNumber != null && vm.nybaddress.buildingNumber !== undefined ? vm.nybaddress.buildingNumber : '';
                var buildingName = vm.nybaddress.buildingName != null && vm.nybaddress.buildingName !== undefined ? vm.nybaddress.buildingName : '';
                var subBuildingName = vm.nybaddress.subBuildingName != null && vm.nybaddress.subBuildingName !== undefined ? vm.nybaddress.subBuildingName : '';
                var street = vm.postalAddressData.thoroughfare != null && vm.postalAddressData.thoroughfare !== undefined ? vm.postalAddressData.thoroughfare : '';
                var postCode = vm.postalAddressData.postcode != null && vm.postalAddressData.postcode !== undefined ? vm.postalAddressData.postcode : '';
                var departmentName = vm.nybaddress.departmentName != null && vm.nybaddress.departmentName !== undefined ? vm.nybaddress.departmentName : '';
                var organisationName = vm.nybaddress.organisationName != null && vm.nybaddress.organisationName !== undefined ? vm.nybaddress.organisationName : '';
                
                var address = buildingNumber + ' ' + buildingName + ' ' + subBuildingName + ' ' + organisationName + ' ' + departmentName + ' ' + street + ' ' + postCode;
                var positionedDeliverypointObj = {
                    locality: address, addressGuid: vm.postalAddressData.id
                };

                var positionedDeliveryPointListTemp = [];

                if (vm.positionedDeliveryPointList != null) {
                    positionedDeliveryPointListTemp = vm.positionedDeliveryPointList;
                }
                
                
                positionedDeliveryPointListTemp.push(positionedDeliverypointObj);

                vm.positionedDeliveryPointList = positionedDeliveryPointListTemp;

                $state.go("deliveryPoint", { positionedDeliveryPointList: vm.positionedDeliveryPointList })

            }
            else {
                setDP();
            }
        });
    }

    function createDeliveryPointDTO() {
        if (!vm.nybaddress) {
            return vm.nybaddress = {
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
            return vm.nybaddress = {
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

    function onBlur() {
        $timeout(function () {
            vm.results = {
            };
        }, 1000);
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
        vm.searchText = selectedItem;
        vm.results = {
        };
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
            vm.dpUse = $filter('filter')(response, { categoryName: referenceDataConstants.DeliveryPointUseIndicator })[0];
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
                   if (vm.nybaddress && !(vm.nybaddress.organisationName)) {
                       vm.dpUse = $filter('filter')(vm.dpUse.referenceDatas, {
                           displayText: "Residential"
                       });
                   }
                   else {
                       vm.dpUse = $filter('filter')(vm.dpUse.referenceDatas, {
                           displayText: "Commercial"
                       });
               }
        });
    }
    function locateDeliveryPoint(selectedItem) {
        deliveryPointApiService.GetAddressLocation(selectedItem)
            .then(function (response) {
                var data = response;
                var lat = data.features[0].geometry.coordinates[1];
                var long = data.features[0].geometry.coordinates[0];
                mapFactory.locateDeliveryPoint(long, lat);
            });
    }
};
