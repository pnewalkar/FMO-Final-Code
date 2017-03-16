MyApp.controller("fmocontroller", ['$scope', 'fmoService', function ($scope, fmoService) {

    
    $scope.Employee={
        name:'jitendra',
        address:'shirpur',
        city:'shirpur',
        mobileno:'9619183061'   
    };
   

    $scope.name = "jitendra";

    //fmoService.getUsers($scope.Employee).then(function (response) {
    //    debugger;
    //    alert(response);

    //});


   

}]);