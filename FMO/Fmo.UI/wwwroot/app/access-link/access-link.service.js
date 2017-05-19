angular.module('accessLink')
    .service('accessLinkService', function () {

        this.accessLink = function () {
            return {
                templateUrl: './access-link/acccess-link.template.html',
                clickOutsideToClose: false,
                controller: "AccessLinkController as vm"
            }
        };
    });

