angular.module('licensingInfo')
    .service('licensingInformationAccessorService', function () {

        var info = '';
        return {
            getLicensingInformation: function () {
                return info;
            },
            setLicensingInformation: function (value) {
                info = value;
            }
        }
    });