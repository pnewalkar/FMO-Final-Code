
MyApp.config(function ($stateProvider, $urlRouterProvider) {

    $urlRouterProvider.otherwise('/');
        $stateProvider.state('home', {
            url: '/',
            templateUrl: '/app/index.html'
           
        })
        .state('delivery-point', {
            url: '/home',
            templateUrl: '/app/delivery-point/delivery-point.template.html',
            controller: 'deliveryPointController',
            controllerAs: 'dp'
        })
        .state('delivery-group', {
            url: '/home',
            templateUrl: '/app/delivery-group/delivery-group.template.html',
            controller: 'deliveryGroupController',
            controllerAs: 'dg'

        })
        .state('address-point', {
            url: '/home',
            templateUrl: '/app/address-point/address-point.template.html',
            controller: 'addressPointController',
            controllerAs: 'ap'
        })
        .state('harzards', {
            url: '/home',
            templateUrl: '/app/hazards/hazards.template.html',
            controller: 'harzardsController',
            controllerAs: 'hz'
        })
        .state('simulation', {
            url: '/home',
            templateUrl: '/home.html',
            controller: 'simulationController',
            controllerAs: 'sm'
        })
        .state('user', {
            url: '/fmo',
            templateUrl: '/app/fmo/fmo.html',
            controller: 'fmocontroller'
        });   
});

