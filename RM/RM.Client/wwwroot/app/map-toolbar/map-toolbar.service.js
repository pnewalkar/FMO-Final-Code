angular
    .module('mapToolbar')
    .factory('mapToolbarService', mapToolbarService);

mapToolbarService.$inject = [
    'CommonConstants'];

function mapToolbarService(
    CommonConstants) {

    var vm = this;
    vm.selectedButton = null;
    vm.dpSelected = false;
    vm.mapButtons = ["select", "point", "line", "accesslink", "area", "modify", "delete"];
    return {
        setSelectedButton: setSelectedButton,
        deselectButton: deselectButton,
        getShapeForButton: getShapeForButton,
        showButton: showButton,
        autoSelect: autoSelect,
        getMapButtons: getMapButtons,
        selectDP: selectDP,
        getMapButtonsList: getMapButtonsList
    };

    function getMapButtonsList() {
        return vm.mapButtons;
    }

    function getMapButtons(isObjectSelected, isGroupAction) {
        if (angular.isDefined(isObjectSelected)) {
            if (isObjectSelected === false && vm.mapButtons.indexOf(CommonConstants.ButtonShapeType.del) > -1) {
                deleteButton(CommonConstants.ButtonShapeType.del);
            }
            else if (isObjectSelected === true && vm.mapButtons.indexOf(CommonConstants.ButtonShapeType.del) === -1) {
                addButton(CommonConstants.ButtonShapeType.del);
            }
        }

        if (angular.isDefined(isGroupAction)) {
            if (isGroupAction) {
                vm.mapButtons = vm.mapButtons.map(function (x) { return x.replace('area', 'group'); });
            }
        }
        return vm.mapButtons;
    }

    function setSelectedButton(button, selectedButton) {
        var shape = getShapeForButton(button);

        if (button === vm.selectedButton && vm.mapButtons.length !== 1) {
            deselectButton(button);
            return false;
        }
        else if (button !== vm.selectedButton) {
            vm.selectedButton = button;
            return true;
        }


    }

    function deselectButton(button) {
        var emitSelect = false;
        if (button === vm.selectedButton) {
            if (vm.mapButtons.indexOf(CommonConstants.ButtonShapeType.select) >= 0) {
                vm.selectedButton = CommonConstants.ButtonShapeType.select;
                emitSelect = true;
                return emitSelect;
            }
            else {
                vm.selectedButton = null;
                emitSelect = false;
                return emitSelect;
            }
        }
    }

    function getShapeForButton(button) {
        switch (button) {
            case CommonConstants.ButtonShapeType.point:
                return CommonConstants.GeometryType.Point;
            case CommonConstants.ButtonShapeType.line:
                return CommonConstants.GeometryType.LineString;
            case CommonConstants.ButtonShapeType.accesslink:
                return CommonConstants.GeometryType.LineString;
            case CommonConstants.ButtonShapeType.area:
            case CommonConstants.ButtonShapeType.group:
                return CommonConstants.GeometryType.Polygon;
            default:
                return undefined;

        }
    }

    function showButton(button) {
        return vm.mapButtons.indexOf(button) != -1;
    }

    function autoSelect() {
        setSelectedButton(vm.mapButtons[0], CommonConstants.ButtonShapeType.select);
    }

    function selectDP(isSelected) {
        return isSelected;
    }

    function deleteButton(element) {
        vm.mapButtons.splice(vm.mapButtons.indexOf(element), 1);
    }

    function addButton(element) {
        vm.mapButtons.push(element);
    }
}