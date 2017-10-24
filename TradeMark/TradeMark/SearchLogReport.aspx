<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="SearchLogReport.aspx.cs" Inherits="TradeMark.SearchLogReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="Content/dataTables.bootstrap.css" rel="stylesheet" />
    <script src="Scripts/jquery.dataTables.js"></script>

    <!-- start js datatable js files -->
    <script src="//cdnjs.cloudflare.com/ajax/libs/wysihtml5/0.3.0/wysihtml5.min.js"></script>
    <script src="//cdn.datatables.net/buttons/1.2.4/js/dataTables.buttons.min.js"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js"></script>
    <script src="//cdn.datatables.net/buttons/1.2.4/js/buttons.html5.min.js"></script>
    <link href="//cdn.datatables.net/buttons/1.2.4/css/buttons.dataTables.min.css" rel="stylesheet" type="text/css">
    <!--end js datatable js files -->

    <script src="Scripts/bootstrap-datepicker.js"></script>
    <script src="Scripts/bootstrap-datepicker.min.js"></script>
    <link href="Css/bootstrap-datepicker.css" rel="stylesheet" />


    <script>
        $(document).ready(function () {

            drawdatatable();
            $("#Startdate").datepicker({
                format: "mm/dd/yyyy",
                endDate: new Date,
                autoclose: true
            }).change(function () {

                var date = $("#Startdate").val();
                $("#Enddate").val("");
                $("#Enddate").datepicker({
                    format: "mm/dd/yyyy",
                    startDate: date,
                    endDate: new Date,
                    autoclose: true
                })
            })
            defaultDates();
        });

        //function to get the default From date and To date
        function defaultDates() {
            //Showing last one month data by default
            //default From date:
            var dateArry = new Date(Date.now() - 30 * 24 * 60 * 60 * 1000).toJSON().slice(0, 10).split('-');
            var fromDate = dateArry[1] + "/" + dateArry[2] + "/" + dateArry[0];

            $("#Startdate").val(fromDate);
            //default To date:
            var date = new Date();
            var toDatestring = ("0" + (date.getMonth() + 1).toString()).substr(-2) + "/" + ("0" + date.getDate().toString()).substr(-2) + "/" + (date.getFullYear().toString());
            $("#Enddate").val(toDatestring);
        }

        function GetSearchReport() {
            $('#spnTodate').html("");
            if ($("#Startdate").val() == "" && $("#Enddate").val() == "") {
                defaultDates();
            }
            if ($("#Startdate").val() == "" && $("#Enddate").val() != "") {
                //defaultDates();
                document.getElementById('spnTodate').style.display = 'block';
                $('#spnTodate').html("Please enter from date");
                return false;
            }

            if ($("#Startdate").val() != "" && $("#Enddate").val() == "") {
                $('#spnTodate').html("Please enter to date");
                document.getElementById('spnTodate').style.display = 'block';
                return false;
            }
            else {
                //spnTodate
                document.getElementById('spnTodate').style.display = 'none';
            }
            $("#hdnSearchReport").val("true");
            drawdatatable();
        }

        function drawdatatable() {

            var html;
            var Startdate, Enddate, isValid = true;
            var startdtArray, enddtArray;
            var startDt = 'null';
            var endDt = 'null';

            if (($("#Startdate").val() == null || $("#Startdate").val() == "") && ($("#Enddate").val() == null || $("#Enddate").val() == "")) {
                var serachReportStatus = $("#hdnSearchReport").val();
                // check search button is click or not 
                if (serachReportStatus == "true") {

                    if (($("#Startdate").val() == null || $("#Startdate").val() == "") && ($("#Enddate").val() == null || $("#Enddate").val() == "")) {
                        $("#eventlisttbl").dataTable().fnDestroy();
                    }
                }
                Startdate = null;
                Enddate = null;
            }
            else {
                $("#eventlisttbl").dataTable().fnDestroy();

                Startdate = $("#Startdate").val();
                Enddate = $("#Enddate").val();

                startdtArray = Startdate.split("/");
                enddtArray = Enddate.split("/");
                startDt = startdtArray[0] + "/" + startdtArray[1] + "/" + startdtArray[2];
                endDt = enddtArray[0] + "/" + enddtArray[1] + "/" + enddtArray[2];


                if (new Date(Startdate) > new Date(Enddate)) {

                    isValid = false;
                }

            }

            if (isValid == true)
                $.ajax({


                    "url": 'AjaxHandler.aspx?status=GetSearchLogReport&Startdate=' + startDt + '&Enddate=' + endDt,// this for calling the web method function in cs code.

                    "dataType": "json",
                    success: function (response) {

                        var data = response.Response;

                        if (response.length > 0) {
                            html = "<thead><tr><th>User Name</th><th>Trademark</th><th>Date</th> <th>Description</th> </thead>";
                            for (var i = 0; i < response.length; i++) {
                                var searchDate = ConvertJsonDateString(response[i].SearchDate);


                                html += "<tr><td>" + response[i].FullName + "</td><td>" + response[i].SearchText + "</td><td>" + searchDate + "</td> <td>" + response[i].USClassDescription + "</td></tr>";
                            }
                            $("#eventlisttbl").html(html);
                        }
                        else {
                            $("#eventlisttbl").html("");

                        }

                        if (typeof dTable == 'undefined') {
                            $(document).ready(function () {

                                // Invoke Buttons plugin (Bfrtip...)
                                $('#eventlisttbl').DataTable({
                                    dom: 'Bfrtip',
                                    buttons: [
                                        {
                                            extend: 'excelHtml5',
                                            title: 'SearchLog'
                                        }
                                    ]
                                });
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
        function getthemonth(month) {
            switch (month) {
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

    <!--History Content Start-->
    <div class="container">
        <form role="form" name="add_factory_form" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="heading">
                        <h2><span>Search Log Report</span></h2>
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
                            <div class="clearfix">
                                <div class="col-md-12">
                                    <label><span class="error" id="spnTodate" style="display: none;"></span></label>
                                </div>
                                <div class="form-inline text-right m-b25">
                                    <div class="form-group">
                                        <label class="sr-only" for="inputEmail">From:</label>
                                        <input type="hidden" value="false" id="hdnSearchReport" />
                                        <input id="Startdate" type="text" class="form-control" />
                                    </div>
                                    <div class="form-group">
                                        <label class="sr-only" for="inputPassword">To:</label>
                                        <input id="Enddate" type="text" class="form-control" />
                                    </div>

                                    <input type="button" name="serach" class="btn btn-primary btn-mrn" value="Search" onclick="GetSearchReport()" />
                                </div>
                            </div>

                            <table id="eventlisttbl" class="table table-striped table-bordered dt-responsive nowrap"></table>
                            <span class="clearfix"></span>
                        </div>
                        <!--Panel Body End-->
                    </div>
                    <!--Panel Shadow Box End-->
                </div>
            </div>
        </form>
    </div>

</asp:Content>
