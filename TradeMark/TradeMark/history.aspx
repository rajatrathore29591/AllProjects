<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="history.aspx.cs" Inherits="TradeMark.history" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/dataTables.bootstrap.css" rel="stylesheet" />
     <script src="Scripts/jquery.dataTables.js"></script>

    <script>
        $(document).ready(function () {

            drawdatatable();
        });
        function drawdatatable() {
            $.ajax({
                
                "url": 'AjaxHandler.aspx?status=GetUserSearchResult',// this for calling the web method function in cs code.
                
                "dataType": "json",
                success: function (response) {
                   
                    var data = response.Response;
                   
                    if(response.length>0)
                    {
                        var html = "<thead><tr><th>Title</th><th>Date</th></thead>";
                        for (var i = 0; i < response.length; i++)
                        {
                            var StartDate = ConvertJsonDateString(response[i].SearchDate);
                            var url = 'Search.aspx?Guid=' + response[i].SearchGuid;
                          
                            html += "<tr><td><a href=" + url + ">" + response[i].Title + "</a></td><td>" + StartDate + "</td></tr>";
                        }
                        $("#eventlisttbl").html(html);
                    }
                   
                    if (typeof dTable == 'undefined') {
                        dtable = $('#eventlisttbl').DataTable({
                            "lengthMenu": [[ 10, 25, 50, -1], [ 10, 25, 50, "All"]],
                             "aoColumnDefs": [
          { 'bSortable': false, 'aTargets': [ 1 ] }]
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
        //function json date format to string
        function ConvertJsonDateString(jsonDate) {
          
            if (jsonDate.length > 0)
                var date = jsonDate.substring(0, 10);
            var tDate = date.substring(8, 10);
            var month = date.substring(5, 7);
            var year = date.substring(0, 4);
          
           
            return month + "/" + tDate + "/" + year;
        };
        //function to get month to create date from jason date format
        function getthemonth(month)

        {
            switch(month) {
                case "01":
                   return "Jan"
                    break;
                case "02":
                    return "Feb"
                    break;
                case "03":
                    return "Mar"
                    break;
                case "04":
                    return "Apr"
                    break;
                case "05":
                    return "May"
                    break;
               
                case "06":
                    return "Jun"
                    break;
                case "07":
                    return "Jul"
                    break;
                case "08":
                    return "Aug"
                    break;
                case "09":
                    return "Sep"
                    break;
                case "10":
                    return "Oct"
                    break;
                case "11":
                    return "Nov"
                    break;
                case "12":
                    return "Dec"
                    break;
            }
        }
    </script>
   
        
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!--History Content Start-->
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="heading">
                    <h2><span>History</span></h2>
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
                         <table id="eventlisttbl" class="table table-striped table-bordered dt-responsive nowrap"></table>
                        
                            </div>
                    </div>
                    <!--Panel Body End-->
                </div>
                <!--Panel Shadow Box End-->
            </div>
        </div>
    </div>
  <!--History Content End-->


</asp:Content>
