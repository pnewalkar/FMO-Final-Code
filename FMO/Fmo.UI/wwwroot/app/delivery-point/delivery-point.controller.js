angular
    .module('deliveryPoint')
    .controller("DeliveryPointController", ['$scope', '$mdDialog', 'deliveryPointService', 'deliveryPointApiService', 'referencedataApiService',
                '$filter',
                'referenceDataConstants', '$timeout'
, DeliveryPointController])
function DeliveryPointController($scope, $mdDialog, deliveryPointService, deliveryPointApiService, referencedataApiService,
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
    vm.onBlur = onBlur;
    vm.display = false;
    vm.disable = true;
    vm.openAlert = openAlert;
    vm.toggle=toggle;
    vm.exists =exists;
    vm.deliveryPointList= [{locality:"BN1 Dadar", isPostioned : false},
                           {locality:"BN2 Dadar", isPostioned : false},
                           {locality:"BN3 Dadar", isPostioned : false}
                          ];
    
    vm.positioneddeliveryPointList = [];
    
    function toggle (item, list) {
        var idx = list.indexOf(item);
        if (idx > -1) {
          list.splice(idx, 1);
        }
        else {
          list.push(item);
        }
      };

      function exists (item, list) {
        return list.indexOf(item) > -1;
      };
    
     function openAlert(ev){
    var confirm = 
      $mdDialog.confirm()
        .clickOutsideToClose(true)
        .title('Confirm Position')
        .textContent('Are you sure you want to position this point here?')
        .ariaLabel('Left to right demo')
        .ok('Yes')
        .cancel('No')

        $mdDialog.show(confirm).then(function() {
       alert("Yes");
    }, function() {
      alert("no");
    });
  };

    referenceData();

    function querySearch(query) {
        deliveryPointApiService.GetDeliveryPointsResultSet(query).then(function (response) {
            vm.results = response.data;
        });
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
            vm.results = {};
            vm.resultscount = { 0: { count: 0 } };
        }
    }

    function onBlur() {
        $timeout(function () {
            vm.results = {};
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
               
                    vm.postalAddressData = response.data;
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
        vm.results = {};
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
            vm.deliveryPointTypes = $filter('filter')(response, { categoryName: referenceDataConstants.DeliveryPointType });
        });
    }

};
