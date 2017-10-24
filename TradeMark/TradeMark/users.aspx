<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="users.aspx.cs" Inherits="TradeMark.users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="Content/dataTables.bootstrap.css" rel="stylesheet" />
    <script src="Scripts/jquery.dataTables.js"></script>
    <script src="Scripts/bootstrap-datepicker.js"></script>
    <script src="Scripts/bootstrap-datepicker.min.js"></script>

    <script>
        $(document).ready(function () {
            drawdatatable();
        });

        //function for create js datable of all user list
        function drawdatatable() {
            var html;
            $.ajax({
                "url": 'AjaxHandler.aspx?status=GetAllUsers',// this for calling the web method function in cs code.
                "dataType": "json",
                success: function (response) {
                    var data = response.Response;
                    if (response.length > 0) {
                        html = "<thead><tr><th>SNo.</th><th>First Name</th><th>Last Name</th><th>Company Name</th><th>Title</th><th>Street Address</th><th>Email Id</th><th>Registered Date</th><th>Contact No</th><th class='nosort'>Action</th></thead>";
                        var sno = 1;
                        for (var i = 0; i < response.length; i++) {
                            //check when these are null then show - on the column 
                            
                            var companyName = response[i].companyname != '' ? response[i].companyname : '-';
                            var title = response[i].title != '' ? response[i].title : '-';
                            var streetAddress = response[i].streetaddress != '' ? response[i].streetaddress : '-';
                            var formatRegisterDate = ConvertJsonDateString(response[i].RegisteredDate);
                            var isActive = response[i].IsActive == true ? 'De-Active' : 'Active';
                            //bind tr into table
                            html += "<tr><td>" + sno + "</td><td>" + response[i].firstname + "</td><td>" + response[i].lastname + "</td><td>" + companyName + "</td><td>" + title + "</td><td>" + streetAddress + "</td><td>" + response[i].emailid + "</td><td>" + formatRegisterDate + "</td><td>" + response[i].contactNo + "</td><td class='td-action'>";
                            //html += "<ul class=''><li><a href='edituser.aspx?ID=" + response[i].UserId + "'>Edit</a></li><li><a href='javascript:void(0);' data-toggle='modal' data-target='#delete_model' onclick='DeleteUserById(&#39;" + response[i].UserId + "&#39;);'>Delete</a></li><li><a href='javascript:void(0);' onclick='UpdateUserStatus(&#39;" + response[i].UserId + "&#39;," + response[i].IsActive + ");'>" + isActive + "</a></li></ul></td></tr>";
                            html += "<div class='dropdown'><a data-toggle='dropdown' href='#' class='dropdown-toggle' aria-expanded='false'><i class='fa fa-bars'></i><span class='caret'></span></a><ul class='dropdown-menu hdropdown notification animated flipInX'><li><a href='edituser.aspx?ID=" + response[i].userId + "'><i aria-hidden='true' class='fa fa-pencil-square-o'></i> Edit</a></li><li><a href='javascript:void(0);' data-toggle='modal' data-target='#delete_model' onclick='DeleteUserById(&#39;" + response[i].userId + "&#39;);'><i aria-hidden='true' class='fa fa-trash'></i> Delete</a></li><li><a href='javascript:void(0);' onclick='UpdateUserStatus(&#39;" + response[i].userId + "&#39;," + response[i].IsActive + ");'> <i aria-hidden='true' class='fa fa-check-square-o'></i>" + isActive + "</a></li></ul></div>";
                            sno++;
                        }
                        $("#UserListTbl").html(html);
                    }
                    else {
                        $("#UserListTbl").html("");

                    }

                    if (typeof dTable == 'undefined') {                       
                        dtable = $('#UserListTbl').DataTable({
                            "scrollX": true,
                            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                            "aoColumnDefs": [
         { 'bSortable': false, 'aTargets': ['nosort'] }]
                        });
                    }
                    else {                       
                        dtable.fnDraw();
                    }
                },
                failure: function (response) {
                    alert("Something went wrong");
                }
            })

        };

        //function for get particular user open model for delete confirm
        function DeleteUserById(userId) {
            alert(userId);
            $("#btn_delete").attr("title", userId);
            $("#delete_model").modal('#delete_model').show();
        }

        //function for Delete user
        function DeleteUser(userId) {
            $.ajax({
                "url": 'AjaxHandler.aspx?status=DeleteUser&UserId=' + userId,// this for calling the web method function in cs code.
                "dataType": "json",
                success: function (response) {
                    $("#delete_model").modal('hide');
                    $('#UserListTbl').DataTable().destroy();

                    drawdatatable();
                },
                failure: function (response) {
                    alert("Something went wrong");
                }
            })

        }

        function UpdateUserStatus(userId, userStatus) {
            $.ajax({
                "url": 'AjaxHandler.aspx?status=EditUserStatus&UserId=' + userId + '&UserStatus=' + userStatus,// this for calling the web method function in cs code.
                "dataType": "json",
                success: function (response) {
                    $("#delete_model").modal('hide');
                    $('#UserListTbl').DataTable().destroy();

                    drawdatatable();
                },
                failure: function (response) {
                    alert("Something went wrong");
                }
            })

        }

        //function json date format to string
        function ConvertJsonDateString(jsonDate) {

            if (jsonDate.length > 0)
                var date = jsonDate.substring(0, 10);
            var tDate = date.substring(8, 10);
            var month = date.substring(5, 7);
            var year = date.substring(0, 4);
            return month + "/" + tDate + "/" + year;
        };
    </script>

         <!--History Content Start-->
    <div class="container">
        <form role="form" name="add_factory_form" runat="server">
        <div class="row">
            <div class="col-md-12">
                <div class="heading">
                    <h2><span>User List</span></h2>
                </div>
                <!--Panel Shadow Box Start-->
                <div class="panel-shadow-box clearfix">
                    <!--Panel Heading Start-->
                    <%--<div class="panel-heading">
                        <h3>Disclaimer</h3>
                    </div>--%>
                    <!--Panel Heading End-->
                    <!--Panel Body End-->
                    <div class="panel-body-content">
                        <div class="content-info">
                          <table id="UserListTbl" class="table table-striped table-bordered dt-responsive"></table>
                        <span class="clearfix"></span>
                    </div>
                    <!--Panel Body End-->
                </div>
                <!--Panel Shadow Box End-->
            </div>
        </div>
            </form>
    </div>
  <!--History Content End-->


    <!-- delete confirmation modal-->
    <div id="delete_model" class="modal fade">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body" id="modelViewlog">
                    <p class="text-center" id="viewLogDetail">
                        Do you want to delete this user?
                    </p>
                    <div class="text-center mt-20">
                        <a href="javascript:void(0);" id="btn_delete" class="btn btn-primary" title="Delete" onclick="DeleteUser(this.title);">Delete</a>
                        <button title="Cancel" class="btn bg-orange" type="button" data-dismiss="modal">Cancel</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
