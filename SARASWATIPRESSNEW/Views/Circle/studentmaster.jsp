<?xml version="1.0" encoding="ISO-8859-1" ?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html  ng-app="myApp">
 <head>        
        <!-- META SECTION -->
        <title>Admin</title>      
           <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <meta http-equiv="X-UA-Compatible" content="IE=edge" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        @*<! <link rel="icon" href="favicon.ico" type="image/x-icon" />*@  
        <script src="resources/js/lib/angular/angular.min.js"></script>
        <script src="resources/js/lib/angular/angular-route.min.js"></script>
        <script type='text/javascript' src='resources/js/plugins/datatables/jquery.dataTables.min.js'></script>
        <script type="text/javascript" src="resources/js/demo_tables.js"></script> 
     <script src="resources/js/controllers/studentmastercontroller1.js"></script>
      <script type="text/javascript" src="resources/js/plugins/bootstrap/bootstrap-select.js"></script>
    <script type="text/javascript" src="resources/js/jquery.min.js"></script>
        <!-- EOF CSS INCLUDE -->                                    
     <link rel="stylesheet" type="text/css" id="theme" href="resources/css/theme-default.css"/>
   
    </head>
    <body  data-ng-controller="question_template_controller">
    
<div class="page-container" >  
               ${menu}
                <div class="message-box animated fadeIn" data-sound="alert" id="mb-signout">
            <div class="mb-container" >
                <div class="mb-middle">
                    <div class="mb-title"><span class="fa fa-sign-out"></span> Log <strong>Out</strong> ?</div>
                    <div class="mb-content">
                        <p>Are you sure you want to log out?</p>                    
                        <p>Press No if you want to continue work. Press Yes to logout current user.</p>
                    </div>
                    <div class="mb-footer">
                        <div class="pull-right">
                            <a href="logout" class="btn btn-success btn-lg">Yes</a>
                            <button class="btn btn-default btn-lg mb-control-close">No</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>   
              <div class="page-content">
               ${notification} 
             
                <!-- End of header -->
                 <div class="page-content-wrap">
                
                    <div class="row">
                        <div class="col-md-12">
                            
                            <form class="form-horizontal" action="mastermanagercontroller/createStudent" method="post" modelAttribute="studentmaster">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title"><strong>One Column</strong> Layout</h3>
                                    <ul class="panel-controls">
                                        <li><a href="#" class="panel-remove"><span class="fa fa-times"></span></a></li>
                                    </ul>
                                </div>
                                <div class="panel-body">
                                    <p>This is non libero bibendum, scelerisque arcu id, placerat nunc. Integer ullamcorper rutrum dui eget porta. Fusce enim dui, pulvinar a augue nec, dapibus hendrerit mauris. Praesent efficitur, elit non convallis faucibus, enim sapien suscipit mi, sit amet fringilla felis arcu id sem. Phasellus semper felis in odio convallis, et venenatis nisl posuere. Morbi non aliquet magna, a consectetur risus. Vivamus quis tellus eros. Nulla sagittis nisi sit amet orci consectetur laoreet. Vivamus volutpat erat ac vulputate laoreet. Phasellus eu ipsum massa.</p>
                                </div>
                                 
                                <div class="panel-body">                                                                        
                                    
                                   <div class="form-group">
                                        <label class="col-md-3 col-xs-12 control-label">Student Name</label>
                                        <div class="col-md-6 col-xs-12"> 
                                                                                                                            
                                             <input type="text" class="form-control" name="student_name" id="student_name" value=""/>
                                           
                                              
                                            <span class="help-block">Select box example</span>
                                        </div>
                                    </div> 
                                </div>
                                 <div class="panel-body">                                                                        
                                    
                                   <div class="form-group">
                                        <label class="col-md-3 col-xs-12 control-label">Student Contact No.</label>
                                        <div class="col-md-6 col-xs-12"> 
                                                                                                                            
                                             <input type="text" class="form-control" name="student_contact_no" id="student_contact_no" value=""/>
                                           
                                              
                                            <span class="help-block">Select box example</span>
                                        </div>
                                    </div> 
                                </div>
                                 <div class="panel-body">                                                                        
                                    
                                   <div class="form-group">
                                        <label class="col-md-3 col-xs-12 control-label">Email ID</label>
                                        <div class="col-md-6 col-xs-12"> 
                                                                                                                            
                                             <input type="text" class="form-control" name="student_email" id="student_email" value=""/>
                                           
                                              
                                            <span class="help-block">Select box example</span>
                                        </div>
                                    </div> 
                                </div>
                                 <div class="panel-body">                                                                        
                                    
                                   <div class="form-group">
                                        <label class="col-md-3 col-xs-12 control-label">Address</label>
                                        <div class="col-md-6 col-xs-12"> 
                                                                                                                            
                                             <input type="text" class="form-control" name="student_address" id="student_address" value=""/>
                                           
                                              
                                            <span class="help-block">Select box example</span>
                                        </div>
                                    </div> 
                                </div>
                                <div class="panel-footer">
                                    <button class="btn btn-primary pull-right">Add Student</button>
                                </div>
                            </div>
                            </form>
                            <form action="StudentSheetUpload" method="post" enctype="multipart/form-data" modelAttribute="uploadForm" >
                              <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title"><strong>Upload Student Sheet</strong></h3>
                                    <ul class="panel-controls">
                                        <li><a href="#" class="panel-remove"><span class="fa fa-times"></span></a></li>
                                    </ul>
                                </div>
                                
                                <div class="panel-body">
                                                                                                      
                                   
                                   <div class="form-group">
                                        <label class="col-md-3 col-xs-12 control-label">Select Class</label>
                                        <div class="col-md-6 col-xs-12"> 
                                                                                                                            
                                            <select  name="class_id" id="class_id"   >
                                                 
                                               <option ng-repeat="todo in sclass" value="{{todo.class_id}}">{{todo.class_name}}</option>
                                               
                                               
                                            </select>
                                              
                                            <span class="help-block">Select Class</span>
                                        </div>
                                    </div>
                                    <br></br>
                                    <div class="form-group">
                                        <label class="col-md-3 col-xs-12 control-label">Select Section</label>
                                        <div class="col-md-6 col-xs-12"> 
                                               <select  name="section_id" id="section_id"   >
                                                 
                                               <option ng-repeat="todo in section" value="{{todo.section_id}}">{{todo.section_name}}</option>
                                               
                                               
                                            </select>
                                            <span class="help-block">Select Section</span>
                                        </div>
                                    </div>
                                    <br></br>
                                   <div class="form-group">
                                   
                                          <label class="col-md-3 col-xs-12 control-label">Select Student-Sheet</label>
                                       
                                        <div class="col-md-6 col-xs-12"> 
                                                                                                                            
                                            <input type="file" name="files" id="files">
									
                                              
                                            <span class="help-block">Select Student Sheet</span>
                                        </div>
                                    </div>
                                    

                                </div>
                                <div class="panel-footer">
                                    <button class="btn btn-default">Clear Form</button>                                    
                                    <button class="btn btn-primary pull-right">Upload Answer-Sheet</button>
                                </div>
                            </div>
                            </form>
                        </div>
                    </div>                    
                     <div class="panel panel-default">
                                  <div class="panel-heading">                                
                                    <h3 class="panel-title">Question Template Format</h3>
                                    <input type="text" class="form-control" style="width: 250px;margin-left: 150px;" placeholder="Search" ng-model="search.q" ng-keyup="admissionSearch(search)">
                                    <ul class="panel-controls">
                                        <li><a href="#" class="panel-collapse"><span class="fa fa-angle-down"></span></a></li>
                                        <li><a href="#" class="panel-refresh"><span class="fa fa-refresh"></span></a></li>
                                        <li><a href="#" class="panel-remove"><span class="fa fa-times"></span></a></li>
                                    </ul>
                                    <div class="btn-group pull-right">
                                        <button class="btn btn-danger dropdown-toggle" data-toggle="dropdown"><i class="fa fa-bars"></i> Export Data</button>
                                        <ul class="dropdown-menu">   
<li><a href="#" onClick ="$('#customers2').tableExport({type:'json',escape:'false'});">JSON</a></li>
<li><a href="#" onClick ="$('#customers2').tableExport({type:'excel',escape:'false'});">XLS</a></li>
<li><a href="javascript:void(0)" onClick ="$('#customers2').tableExport({type:'csv',escape:'false'});">CSV</a></li>
<li><a href="javascript:void(0)" onClick ="$('#customers2').tableExport({type:'pdf',escape:'false'});">PDF</a></li>    
                                            <li class="divider"></li>
                                            </ul>
                                    </div>                                 
                                </div>
                                <div class="panel-body">
                                <table class="table" id="customers2">
                                        <thead>
                                            <tr>
                                                <th>Student ID</th>
                                                <th>Student Name</th>
                                                 <th>Contact Number</th>
                                                <th>Action</th>
                                                
                                                
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr ng-repeat="todo in students">
                                            
                                               <td>{{todo.student_id}}</td>
                                               <td>{{todo.student_name}}</td>
                                              <td>{{todo.student_contact_no}}</td>
                                                  <td><a class="btn btn-mini btn-danger" href="" >Delete</a>
                                                 </td>
                                            </tr>
                                                       </tbody>
                                    </table>
                                      <ul class="pagination pagination-sm">
            <li ng-class="{active:0}"><a href="#" ng-click="firstPage()">First</a>

            </li>
            <li ng-repeat="n in range(ItemsByPage.length)"> <a href="#" ng-click="setPage()" ng-bind="n+1">1</a>

            </li>
            <li><a href="#" ng-click="lastPage()">Last</a>

            </li>
        </ul>
                                    
                                </div>
                                </div>
                </div>
                 </div>   
           </div>
                  ${csslink}      
    </body>
</html>