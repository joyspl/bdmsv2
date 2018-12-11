	var app = angular.module('myApp', []);
	function test_template_controller($scope, $http,$timeout){

	    $scope.editMode = false;
	    $scope.position = '';
	    
	    $scope.getAllTodos1 = function(){
	    	//alert("kk");
		    $http.get('mastermanagercontroller/getAllTest?param='+document.getElementById("test_subject_id").value).success(function(response){
	            $scope.tests = response;
	               }).error(function() {
	            $scope.setError('Could not display all todos');
	        });
	    };
	    $scope.getAllTodos = function(){
	    	alert("kk");
		    $http.get('mastermanagercontroller/getAllSubjectsTotal').success(function(response){
	            $scope.subjects = response;
	               }).error(function() {
	            $scope.setError('Could not display all todos');
	        });
	    };
	   
	    $scope.getAllExams = function(){
	    	//alert("kk");
		    $http.get('mastermanagercontroller/getAllExam').success(function(response){
	            $scope.exams = response;
	               }).error(function() {
	            $scope.setError('Could not display all todos');
	        });
	    };
	    $scope.getAllTodos();
	    $scope.getAllExams();
	   
	}