angular
    .module('validateNumber')
    .controller("ValidateNumberController", SearchableDropdownController);

ValidateNumberController.$inject = [
    '$scope'
];

function ValidateNumberController($scope) {
    var vm = this;
    vm.maxlength = 7;

    vm.validateNumber = validateNumber;

    function validateNumber(element, keyboardEvent) {
        var charCode = (keyboardEvent.which) ? keyboardEvent.which : event.keyCode;
        console.log(charCode)
        var number = element.value.split('.');
        if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
            console.log(charCode)
            return false;
        }
        //just one dot
        if (number.length > 1 && charCode == 46) {
            return false;
        }
        //get the cursor position
        var cursorPos = getSelectionStart(element);
        var dotPos = element.value.indexOf(".");
        if (cursorPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
            return false;
        }
        return true;
    }

    function getSelectionStart(element) {
        if (element.createTextRange) {
            var range = document.selection.createRange().duplicate()
            range.moveEnd('character', element.value.length)
            if (range.text == '') return element.value.length
            return element.value.lastIndexOf(range.text)
        } else return element.selectionStart
    }

}