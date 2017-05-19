angular.module('mapKey')
        .factory('mapKeyService', mapToolbarService);
mapToolbarService.$inject = ['mapStylesFactory',
                             'mapService',
                              'CommonConstants'];

function mapToolbarService(mapStylesFactory,
                           mapService,
                           CommonConstants) {
   
    return {
        showKey: showKey,
        initialize: initialize
    };

    function showKey(id) {
        if (id == "") {
            return true;
        }
        var keys = mapService.mapLayers()
            .filter(function (l) { return l.selected; })
            .map(function (l) { return l.keys; });

        var distinct = [];

        for (var i = 0; i < keys.length; i++) {
            for (var j = 0; j < keys[i].length; j++) {
                if (distinct.indexOf(keys[i][j]) == -1) {
                    distinct.push(keys[i][j]);
                }
            }
        }
        return distinct.indexOf(id) != -1;
    }

    function initialize() {
        var activeStyle = mapStylesFactory.getStyle();
        var pointTypes = [
             
            {
               
                "text": CommonConstants.pointTypes.Selected.text,
                "id": "",
                "style": mapStylesFactory.getStyle(mapStylesFactory.styleTypes.SELECTEDSTYLE)(CommonConstants.pointTypes.Selected.style)
            }, {
                "text": CommonConstants.pointTypes.DeliveryPoint.text,
                "id": CommonConstants.pointTypes.DeliveryPoint.value,
                "style": activeStyle(CommonConstants.pointTypes.DeliveryPoint.style)
            },
             {
                 "text": CommonConstants.pointTypes.AcessLink.text,
                 "id": CommonConstants.pointTypes.AcessLink.value,
                 "style": activeStyle(CommonConstants.pointTypes.AcessLink.style)
             },
            {
                "text": CommonConstants.pointTypes.Road.text,
                "id": CommonConstants.pointTypes.Road.value,
                "style": activeStyle(CommonConstants.pointTypes.Road.style)
            }
        ];
        return pointTypes;
      
    }
  
}