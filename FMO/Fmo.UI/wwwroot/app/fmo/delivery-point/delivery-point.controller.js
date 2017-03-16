
MyApp.controller("deliveryPointController", ['$scope', 'fmoService', '$http', function ($scope, fmoService, $http) {


    $scope.mydata = {};

    var Employee = {
            Name: 'jitendra',
            Address: 'shirpur',
            City: 'shirpur',
            MobileNo: '9619183061'
    };
    debugger;
   
    //var request = $http({
    //    method: "post",
    //    url: "http://localhost:62032/api/Home/GetHeroes",
    //    data: Employee
    //});

    fmoService.getUsers(Employee).then(function (response) {
        debugger;
        $scope.mydata = response.data;
        alert(response);

    });


}]);