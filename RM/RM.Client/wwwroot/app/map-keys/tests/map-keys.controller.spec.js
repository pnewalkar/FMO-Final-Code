describe('mapKey: Controller', function () {

    var ctrl;
    var mapKeyService;
    var $rootScope;

    //MockSet
    var MockMapKeyService = {
        initialize: function () { return [] },
        showKey: function (id) {
            if (id == "") {
                return true;
            } else {
                return id;
            }
        }
    };

    beforeEach(module('mapKey'));

    //load to our provider
    beforeEach(function () {
        module(function ($provide) {
            $provide.value('mapKeyService', MockMapKeyService);
        });
    });
    beforeEach(inject(function (_$rootScope_, $controller, _mapKeyService_) {
        ctrl = $controller('MapKeyController', {
            mapKeyService: _mapKeyService_,
            $rootScope: $rootScope
        });

    }));

    it('should be return a value showKey method', function () {
        spyOn(ctrl, 'showKey');

        ctrl.showKey(2);
        expect(ctrl.showKey).toHaveBeenCalled();
        expect(ctrl.showKey).toHaveBeenCalledWith(2);

        //Call to MockObje
        var KeyShowVal = MockMapKeyService.showKey(2);
        expect(KeyShowVal).toBe(2);

    });

});