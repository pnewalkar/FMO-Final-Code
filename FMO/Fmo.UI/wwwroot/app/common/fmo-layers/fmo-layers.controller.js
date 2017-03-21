/*angular.module('fmoLayers')
    .controller('fmoLayersCtrl',
  function($scope) {
	$scope.accordianData = [
		{
			"heading" : "HOLDEN",
			"content" : "GM Holden Ltd, commonly known as Holden, is an Australian automaker that operates in Australasia and is headquartered in Port Melbourne, Victoria. The company was founded in 1856 as a saddlery manufacturer in South Australia."
		}
	];
	
	$scope.collapse = function(data) {
	   for(var i in $scope.accordianData) {
		   if($scope.accordianData[i] != data) {
			   $scope.accordianData[i].expanded = false;   
		   }
	   }
	   data.expanded = !data.expanded;
	};
	
});*/