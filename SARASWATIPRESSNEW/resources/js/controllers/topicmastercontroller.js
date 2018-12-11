	var app = angular.module('myApp', []);
	function topic_template_controller($scope, $http,$timeout){

	    $scope.editMode = false;
	    $scope.position = '';
	    
	    $scope.getAllTodos1 = function(){
	    	//alert("kk");
		    $http.get('mastermanagercontroller/getAllTopic?param='+document.getElementById("subject_id").value).success(function(response){
	            $scope.topics = response;
	               }).error(function() {
	            $scope.setError('Could not display all todos');
	        });
	    };
	    $scope.getAllTodos = function(){
	    	//alert("kk");
		    $http.get('mastermanagercontroller/getAllSubjectsTotal').success(function(response){
	            $scope.subjects = response;
	               }).error(function() {
	            $scope.setError('Could not display all todos');
	        });
	    };
	   
	   
	    $scope.getAllTodos();
	   
	}