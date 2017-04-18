app.config(function ($stateProvider, $urlRouterProvider) {

    $urlRouterProvider.otherwise('/manageaccess');
    $stateProvider.state('home', {
        url: '/',
        templateUrl: '/app/index.html'     
    })
       .state('manageaccess', {
           url: '/',
           templateUrl: '/app/manage-access/manage-access.html',
           controller: 'manageAccessController as vm',
           params: { username: null, }
       })
      .state('user', {
        url: '/fmo',
        templateUrl: '/app/fmo/fmo.html',
        controller: 'fmocontroller'
    });
});


