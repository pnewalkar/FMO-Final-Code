angular
    .module('error')
    .controller("ErrorController", ErrorController);
ErrorController.$inject = [
     '$scope',
     'message',
     '$mdDialog','$rootScope'];

function ErrorController(
 $scope,
 message,
 $mdDialog,
 $rootScope

) {
    var vm = this;
    vm.openAlert = openAlert;
    vm.message = message;
    vm.closeWindow = closeWindow;
 
    function closeWindow() {
        $mdDialog.hide();
    }

    function openAlert() {
        var confirm = $mdDialog.confirm({
            controller: 'ErrorController as vm',
            templateUrl: './common/error/error-popup.html'
            
        })
        $mdDialog.show(confirm).then(function () {
            console.log(vm.message);
        });
    }
};
