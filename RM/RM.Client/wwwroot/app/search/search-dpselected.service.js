angular.module('search')
    .service('searchDPSelectedService', function () {
        var info = '';
        return {
            getSelectedDP: function () {
                return info;
            },
            setSelectedDP: function (value) {
                info = value;
            }
        }
    });