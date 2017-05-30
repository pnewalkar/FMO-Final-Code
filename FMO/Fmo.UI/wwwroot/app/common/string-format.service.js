angular.module('FMOApp')
    .factory('stringFormatService', stringFormatService)


function stringFormatService() {
    var stringFormatService = {};
    return {
        Stringformat: Stringformat
    };


    function Stringformat () {
           
           var theString = arguments[0];

          
           for (var i = 1; i < arguments.length; i++) {
               
               var regEx = new RegExp("\\{" + (i - 1) + "\\}", "gm");
               theString = theString.replace(regEx, arguments[i]);
           }

           return theString;
        };

    return stringFormatService;
};