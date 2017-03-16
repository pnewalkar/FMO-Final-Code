MyApp.factory('fmoService',['$http', function ($http) {
    var fmoService = {};

   
    //api.deleteUser = deleteUser;

    
    //$http({   
    //    url: 'http://localhost:62032/api/Home/getheroes/',   
    //    dataType: 'json',   
    //    method: 'POST',   
    //    data: { employee: employee },
    //    headers: {   
    //        "Content-Type": "application/json"   
    //    }   
    //}).success(function (response) {   
    //    $scope.value = response;   
    //})   
    //.error(function (error) {   
    //    alert(error);   
    //});   

  
    var getUsers = function (objEmployee) {

    
        //objEmployee = encodeURIComponent(objEmployee);
        
        //return $http.post('http://localhost:62032/api/Home/getheroes', JSON.stringify({ name: "jitendra", address: "shirpur", city: "shirpur", mobileno: "9619183061" }), { headers: {"Content-Type":"application/json"}});
        return $http.post('http://localhost:62032/api/Home/GetHeroes', objEmployee);
      
    };

    fmoService.getUsers = getUsers;

    return fmoService;
  
}]);