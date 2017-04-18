angular.module('mapToolbar')
        .service('mapToolbarService', ['mapService', mapToolbarService])

function mapToolbarService(mapService) {
    var vm = this;
       vm.selectedButton = null;
   
    return{
        setSelectedButton:setSelectedButton,
        deselectButton:deselectButton,
        getShapeForButton:getShapeForButton,
        showButton : showButton,
        autoSelect: autoSelect,
        getMapButtons: getMapButtons
    }
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
            if (mapService.getMapButtons().indexOf('select') >= 0)
            {
                vm.selectedButton = 'select';
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
            case 'point':
                return 'Point';
            case 'line':
                return 'LineString';
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
        if (mapService.getMapButtons().length== 1)
        {
            vm.setSelectedButton(mapService.getMapButtons()[0]);
        }
    }
}