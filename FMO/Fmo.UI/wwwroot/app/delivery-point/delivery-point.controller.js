angular
    .module('deliveryPoint')
    .controller("DeliveryPointController", ['mapToolbarService', '$scope', '$mdDialog', 'deliveryPointService', 'deliveryPointApiService', 'referencedataApiService',
                '$filter',
                'referenceDataConstants', '$timeout'
, DeliveryPointController])
function DeliveryPointController(mapToolbarService, $scope, $mdDialog, deliveryPointService, deliveryPointApiService, referencedataApiService,
    $filter,
    referenceDataConstants, $timeout
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
    vm.display = false;
    vm.disable = true;
    vm.openAlert = openAlert;
    vm.toggle = toggle;
    vm.alias = null;
    vm.exists = exists;
    vm.setOrganisation = setOrganisation;
    vm.deliveryPointList = [{
        locality: "BN1 Dadar",
        addressGuid: 1,
        isPostioned: false
    },
                           {
                               locality: "BN2 Dadar",
                               addressGuid: 2,
                               isPostioned: false
                           },
                           {
                               locality: "BN3 Dadar",
                               addressGuid: 3,
                               isPostioned: false
                           }
    ];

    vm.positioneddeliveryPointList = [];
    vm.createDeliveryPoint = createDeliveryPoint;

    function toggle(item) {
        var idx = $filter('filter')(vm.deliveryPointList, { addressGuid: item.addressGuid });

        //var idx = vm.deliveryPointList.indexOf(item);
        if (idx.length > 0) {
            //$scope.$emit('mapToolChange', { "name": button, "shape": shape, "enabled": true });
            vm.deliveryPointList.splice(idx, 1);
            vm.positioneddeliveryPointList.push(item);
        }
    };

    function exists(item, list) {
        return list.indexOf(item) > -1;
    };

    function openAlert(ev, item) {
        var confirm =
          $mdDialog.confirm()
            .clickOutsideToClose(true)
            .title('Confirm Position')
            .textContent('Are you sure you want to position this point here?')
            .ariaLabel('Left to right demo')
            .ok('Yes')
            .cancel('No')

        $mdDialog.show(confirm).then(function () {
            setDP();

            vm.toggle(item);
        }, function () {
            alert("no");
        });
    };

    referenceData();

    function setDP() {
        var shape = mapToolbarService.getShapeForButton('point');
        $scope.$emit('mapToolChange', { "name": 'deliverypoint', "shape": shape, "enabled": true });
    }

    function querySearch(query) {
        deliveryPointApiService.GetDeliveryPointsResultSet(query).then(function (response) {
            vm.results = response;
        });
    }

    function createDeliveryPoint() {
        debugger;
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
                "Postcode": vm.postalAddressData.postcode
            }
        }
        else if (vm.nybaddress && !vm.nybaddress.id) {
             return vm.nybaddress = {
                "OrganisationName": vm.nybaddress.organisationName,
                "DepartmentName" : vm.nybaddress.departmentName,
                "BuildingName": vm.nybaddress.buildingName,
                "BuildingNumber": vm.nybaddress.buildingNumber,
                "SubBuildingName": vm.nybaddress.subBuildingName,
                "Thoroughfare" : vm.postalAddressData.thoroughfare,
                "DependentLocality": vm.postalAddressData.dependentLocality,
                "Postcode": vm.postalAddressData.postcode
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
                0: { count: 0
            }
        };
    }
}

    function onBlur() {
        $timeout(function () {
            vm.results = {
        };
            // vm.searchText="";
        }, 1000);
}

    function getPostalAddress(selectedItem) {
        //if (selectedItem.length >= 3) {
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
        //  }

        //}
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
        var lastItem = vm.items.length -1;
        vm.items.splice(lastItem);
}

    function bindAddressDetails() {
        debugger;
        deliveryPointApiService.GetPostalAddressByGuid(vm.notyetBuilt)
               .then(function (response) {
                   vm.nybaddress = response;
                   if (vm.nybaddress && !(vm.nybaddress.organisationName)) {
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
        if(val)
        {
            vm.dpUse[0].displayText = "Commercial";
        }
    }
};
