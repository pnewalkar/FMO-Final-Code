// Just for reference 
angular
    .module('fmoCommonHome')
    .controller("fmoCommonHomeCtrl", fmoCommonHomeCtrl)
function fmoCommonHomeCtrl($scope, fmoService) {

    $scope.User = { name: "jitendra", address: "shirpur", city: "shirpur", mobileno: "9619183061" };
    fmoService.getUsers($scope.User).success(function (response) {
        $scope.value = response;
    })
        .error(function (error) {
            alert(error);
        });

};