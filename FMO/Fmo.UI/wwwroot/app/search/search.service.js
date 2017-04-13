angular.module('search')
    .factory('searchApiService',
    ['$http', 'GlobalSettings', '$q', function ($http, GlobalSettings, $q) {
        var searchApiService = {};
 

        searchApiService.basicSearch = function (searchText) {
            // return $http.get(GlobalSettings.apiUrl + '/Search/BasicSearch?searchText=' + searchText);


            // perform some asynchronous operation, resolve or reject the promise when appropriate.
            //return $q(function (resolve, reject) {
            //    setTimeout(function (response) {
            //        resolve(response);
            //    }, 1000);
            //});

            //return $http.get(GlobalSettings.apiUrl + '/deliveryPoints/FetchDeliveryPoints?searchText=' + searchText)
            //   .then(function (response) {
            //       return response.data;
            //      // deferred.resolve(response.data);
            //   });
            return $http.get(GlobalSettings.apiUrl + '/Search/BasicSearch?searchText=' + searchText);
            
        };

        searchApiService.advanceSearch = function (searchText) {
            // return $http.get(GlobalSettings.apiUrl + '/Search/BasicSearch?searchText=' + searchText);


            // perform some asynchronous operation, resolve or reject the promise when appropriate.
            //return $q(function (resolve, reject) {
            //    setTimeout(function (response) {
            //        resolve(response);
            //    }, 1000);
            //});

            //return $http.get(GlobalSettings.apiUrl + '/deliveryPoints/FetchDeliveryPoints?searchText=' + searchText)
            //   .then(function (response) {
            //       return response.data;
            //      // deferred.resolve(response.data);
            //   });
            return $http.get(GlobalSettings.apiUrl + '/Search/AdvanceSearch?searchText=' + searchText);

        };
        return searchApiService;

    }]);