'use strict';
describe('Map Key: Service', function() {
  var mapStylesFactory;
  var mapService;
  var CommonConstants;
  var mapKeyService;     
  var MockMapStyleFactory = {      
        getStyle: function(){
          return function test(){}
        },
        styleTypes:function(){
          return{
            SELECTEDSTYLE: ''
          } 
        }
      };      
  var MockCommonConstants = {
      pointTypes: { 
        DeliveryPoint: { text: "Delivery Point", value: 'deliverypoint', style: "deliverypoint" },             
        AcessLink: { text: "Access Link", value: 'accesslink',style: "accesslink" },
        Road: { text: "Road",value: 'roadlink', style: "roadlink" },    
        Selected: {text: "Selected", value: '',style: "deliverypoint"}               
      }
    };  

  beforeEach(function(){
    module('mapKey');
    module(function($provide) {
      $provide.value('CommonConstants', MockCommonConstants);
      $provide.value('mapStylesFactory',MockMapStyleFactory);
      $provide.factory('mapService', function(){
          return {
            mapLayers: function(){
              return {
                filter:function(){
                  return {
                    map:function(){
                      return 'aasdfd-5a5df5sdf-5fdfsdf';
                    }
                  }
                }
                
              }
            }
          }
      });      
    });    

    inject(function(_mapKeyService_,_mapStylesFactory_,_mapService_,_CommonConstants_) {
      mapKeyService = _mapKeyService_;
      mapStylesFactory = _mapStylesFactory_;
      mapService = _mapService_;
      CommonConstants = _CommonConstants_;
    });
  });

  it('should return true if id is empty', function() {
    expect(mapKeyService.showKey('')).toBe(true);
  });
  
  it('should return false if id is not-empty', function() {    

    expect(mapKeyService.showKey('aasdfd-5a5df5sdf-5fdfsdf')).toBe(false);
  });

  it('should return pointTypes when initialize method called', function() {      
      var pointTypes = [{text:'Selected',id:'',style:undefined},{text:'Delivery Point',id:'deliverypoint',style:undefined},{text:'Access Link',id:'accesslink',style:undefined},{text:'Road',id:'roadlink',style:undefined}]
      expect(mapKeyService.initialize()).toEqual(pointTypes);
  });
});