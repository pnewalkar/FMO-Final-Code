app.config(function ($stateProvider, $urlRouterProvider) {

    $urlRouterProvider.otherwise('/');
    $stateProvider.state('home', {
        url: '/',
        templateUrl: '/app/index.html'     

    })
      .state('user', {
        url: '/fmo',
        templateUrl: '/app/fmo/fmo.html',
        controller: 'fmocontroller'
    });
});


