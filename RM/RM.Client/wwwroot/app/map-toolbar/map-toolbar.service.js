angular
    .module('mapToolbar')
    .factory('mapToolbarService', mapToolbarService);

mapToolbarService.$inject = [
    'mapService',
    'CommonConstants'];

function mapToolbarService(
    mapService,
    CommonConstants) {

    var vm = this;
       vm.selectedButton = null;
       vm.dpSelected = false;
       return {
           setSelectedButton: setSelectedButton,
           deselectButton: deselectButton,
           getShapeForButton: getShapeForButton,
           showButton: showButton,
           autoSelect: autoSelect,
           getMapButtons: getMapButtons,
           selectDP: selectDP
       };

    function getMapButtons()
    {
        vm.mapButtons = mapService.getMapButtons();
        return vm.mapButtons;
    }

    function setSelectedButton(button,selectedButton)
    {
        var shape = getShapeForButton(button);

        if (button == vm.selectedButton && mapService.getMapButtons().length != 1)
        {
            deselectButton(button);
            return false;
        }
        else if (button != vm.selectedButton)
        {
            vm.selectedButton = button;
            return true;
        }


    }

    function deselectButton(button)
    {
        var emitSelect = false;
        if (button == vm.selectedButton)
        {
            if (mapService.getMapButtons().indexOf(CommonConstants.ButtonShapeType.select) >= 0)
            {
                vm.selectedButton = CommonConstants.ButtonShapeType.select;
                emitSelect = true;
                return emitSelect;
            }
            else
            {
                vm.selectedButton = null;
                emitSelect = false;
                return emitSelect;
            }
        }
    }

    function getShapeForButton(button)
    {
        switch (button) {
            case CommonConstants.ButtonShapeType.point:
                return CommonConstants.GeometryType.Point;
            case CommonConstants.ButtonShapeType.line:
                return CommonConstants.GeometryType.LineString;
            case CommonConstants.ButtonShapeType.accesslink:
                return CommonConstants.GeometryType.LineString;
            default:
                return undefined;

        }
    }

    function showButton(button)
    {
        return mapService.getMapButtons().indexOf(button) != -1;
    }

    function autoSelect()
    {      
        setSelectedButton(mapService.getMapButtons()[0], CommonConstants.ButtonShapeType.select);
    }

    function selectDP(isSelected) {
        debugger;
      return isSelected;                             
    }
}