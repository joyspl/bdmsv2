	var app = angular.module('myApp', []);
	function report_template_controller($scope, $http,$timeout){

	    $scope.editMode = false;
	    $scope.position = '';
	    
	    $scope.getAllTodos1 = function(){
	    	//alert("kk");
		    $http.get('mastermanagercontroller/getSubReportList').success(function(response){
	            $scope.reports = response;
	               }).error(function() {
	            $scope.setError('Could not display all todos');
	        });
	    };
	    $scope.getAllTodos2 = function(){
	    	//alert("kk");
		    $http.get('mastermanagercontroller/getExmReportList').success(function(response){
	            $scope.reports1 = response;
	               }).error(function() {
	            $scope.setError('Could not display all todos');
	        });
	    };
	    $scope.getAllTodos3 = function(){
	    	//alert("kk");
		    $http.get('mastermanagercontroller/getAllStudentTotal').success(function(response){
	            $scope.students = response;
	               }).error(function() {
	            $scope.setError('Could not display all todos');
	        });
	    };
	    $scope.getAllTodos4 = function(){
	    	//alert("kk");
		    $http.get('mastermanagercontroller/getAllExam').success(function(response){
	            $scope.exams = response;
	               }).error(function() {
	            $scope.setError('Could not display all todos');
	        });
	    };
	    $scope.getAllTodos4();
	    $scope.getAllTodos3();
	    $scope.getAllTodos1();
	    $scope.getAllTodos2();
	   
	}