	var app = angular.module('myApp', []);
	function question_template_controller($scope, $http,$timeout){

	    $scope.editMode = false;
	    $scope.position = '';
	    
	    $scope.getAllTodos1 = function(){
	    	//alert("kk");
		    $http.get('mastermanagercontroller/getAllStudentTotal').success(function(response){
	            $scope.students = response;
	               }).error(function() {
	            $scope.setError('Could not display all todos');
	        });
	    };
	    $scope.getAllClass = function(){
	    //alert("kk0");
		    $http.get('mastermanagercontroller/getAllClass').success(function(response){
	            $scope.sclass = response;
	            //alert($scope.tests.class_name);
	               }).error(function() {
	            $scope.setError('Could not display all todos');
	        });
	    };
	    $scope.getAllSection = function(){
		    //alert("kk0");
			    $http.get('mastermanagercontroller/getAllSection').success(function(response){
		            $scope.section = response;
		            //alert($scope.tests.class_name);
		               }).error(function() {
		            $scope.setError('Could not display all todos');
		        });
		    };
		    
		    $scope.getAllClass();
	    
	    $scope.getAllSection();
	    
	    
	    $scope.getAllTodos1();
	   
	}