	var app = angular.module('myApp', []);
	function question_template_controller($scope, $http,$timeout){

	    $scope.editMode = false;
	    $scope.position = '';
	    $scope.getAllTodos = function(){
	    	//alert("kk");
		    $http.get('questionTemplateManager/getAllTemplate').success(function(response){
	            $scope.todos = response;
	           // alert($scope.todos[0].subject_name);
	        }).error(function() {
	            $scope.setError('Could not display all todos');
	        });
	    };
	    $scope.getAllTodos1 = function(){
	    	//alert("kk");
		    $http.get('questionTemplateManager/getAllSubjects').success(function(response){
	            $scope.subjects = response;
	               }).error(function() {
	            $scope.setError('Could not display all todos');
	        });
	    };
	    $scope.getAllTest = function(){
	    	//alert("kk");
		    $http.get('questionTemplateManager/getAllTest').success(function(response){
	            $scope.tests = response;
	           // alert($scope.todos[0].subject_name);
	        }).error(function() {
	            $scope.setError('Could not display all todos');
	        });
	    };
	    $scope.getAllTodos();
	    $scope.getAllTodos1();
	    $scope.getAllTest();
	}