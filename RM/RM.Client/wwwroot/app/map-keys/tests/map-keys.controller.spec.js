'use strict'
describe('Map Key: Controller', function() {
  var vm;  
  var mapKeyService;
  var MockMapKeyService = {
        initialize: function() {},
        showKey: function(id){}
     };

    beforeEach(function(){
      module('mapKey');
      module(function ($provide) {
        $provide.value('mapKeyService', MockMapKeyService);
      });    

      inject(function(_$controller_,_mapKeyService_) {
        mapKeyService = _mapKeyService_;
        vm = _$controller_('MapKeyController', {mapKeyService: _mapKeyService_});            
      });

    });

    it('should call `mapKeyService.showKey` method showKey method called', function(){ 
        spyOn(mapKeyService,'showKey');
        vm.showKey('2165981ca-454648945-54486046-684');
        expect(mapKeyService.showKey).toHaveBeenCalledWith('2165981ca-454648945-54486046-684');
    });

});