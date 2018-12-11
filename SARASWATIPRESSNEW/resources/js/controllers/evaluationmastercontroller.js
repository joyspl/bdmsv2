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
	    $scope.paramMaker = function(){
	    	//alert("kk");
	    	
		    $http.get('questionTemplateManager/getAllSubjectsParam?params='+document.getElementById("subject_id").value).success(function(response){
	           // alert(response);
		    	$scope.subjects = response;
	               }).error(function() {
	            $scope.setError('Could not display all todos');
	        });
	    };
	    $scope.paramValues = function(){
	    alert("kk");
	    	
		    $http.get('questionTemplateManager/getParamValues?params='+document.getElementById("subjectparam").value).success(function(response2){
	           // alert(response);
		    	$scope.paramValues = response2;
	               }).error(function() {
	            $scope.setError('Could not display all todos');
	        });
	    };
	    $scope.getAllTodos();
	}