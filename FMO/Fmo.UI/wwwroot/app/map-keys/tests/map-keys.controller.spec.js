//describe('Testing Languages Service', function(){
//    var mapKeyService;/*
//    var mapStylesFactory;
//    var mapService;*/
//
//  beforeEach(function(){
//    module('mapKey');
//    inject(function(_$injector_){
//        mapKeyService = _$injector_.get('mapKeyService');/*
//        mapStylesFactory = $injector.get('mapStylesFactory');
//        mapService = $injector.get('mapService');*/
//    });
//  });
//
//  it('should return available languages', function() {
//    var languages = mapKeyService.initialize();
//    expect(languages[1].id).toContain('deliverypoint');/*
//    expect(languages).toContain('es');
//    expect(languages).toContain('fr');*/
//    expect(languages.length).toEqual(1);
//  });
//});

describe('myFactory', function () {
    
    var mapKeyService;
  // Load your module.
  beforeEach(module('mapKey'));
    
    

  // Setup the mock service in an anonymous module.
  beforeEach(module(function ($provide) {
    $provide.value('mapStylesFactory', {
        someVariable: 1
    });
      $provide.value('mapService', {
        someVariable: 1
    });
      


    
/*    inject(function($injector){
       mapKeyService = $injector.get('mapKeyService');
    });*/
      
        }));
   /* mapKeyService = $injector.get('mapKeyService');*/

  it('can get an instance of my-key service', inject(function(mapKeyService) {
    expect(mapKeyService).toBeDefined();
  }));
    
    it('should return available languages', inject(function(mapKeyService) {
    var languages = mapKeyService.initialize();
    expect(languages[1].id).toContain('deliverypoint');
  }));
});