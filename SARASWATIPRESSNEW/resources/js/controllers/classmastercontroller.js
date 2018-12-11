	var app = angular.module('myApp', []);
	function test_template_controller($scope, $http,$timeout){

	    $scope.editMode = false;
	    $scope.position = '';
	    
	    $scope.getAllTodos1 = function(){
	    	//alert("kk");
		    $http.get('mastermanagercontroller/getAllClass').success(function(response){
	            $scope.tests = response;
	               }).error(function() {
	            $scope.setError('Could not display all todos');
	        });
	    };
	    
	    $scope.getAllTodos1();
	    
	}