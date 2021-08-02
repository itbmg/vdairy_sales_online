<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EditDcDate.aspx.cs" Inherits="EditDcDate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <style type="text/css">
        .ddlsize
        {
            width: 230px;
            height: 30px;
            font-size: 16px;
            border: 1px solid gray;
            border-radius: 7px 7px 7px 7px;
        }
        .datepicker
        {
            border: 1px solid gray;
            background: url("Images/CalBig.png") no-repeat scroll 99%;
            width: 70%;
            top: 0;
            left: 0;
            height: 20px;
            font-weight: 700;
            font-size: 12px;
            cursor: pointer;
            border: 1px solid gray;
            margin: .5em 0;
            padding: .6em 20px;
            border-radius: 10px 10px 10px 10px;
            filter: Alpha(Opacity=0);
            box-shadow: 3px 3px 3px #ccc;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#datepicker").datepicker({ dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
                onSelect: function (selectedDate) {
                    GetEditIndentValues();
                }
            });

            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#datepicker').val(today);

            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#txtTodate').val(today);
        });
        function btnupdateDcNumberDates() {
            var refdcno = document.getElementById("txtDcNo").value;
            if (refdcno == "") {
                alert("Please enter Dc Number");
                return false;
            }
            var fromdate = document.getElementById("datepicker").value;
            if (fromdate == "") {
                alert("Please select date");
                return false;
            }
            var todate = document.getElementById("txtTodate").value;
            if (todate == "") {
                alert("Please select To Date");
                return false;
            }
            var data = { 'operation': 'btnupdate_DcNumber_Dates', 'refdcno': refdcno, 'fromdate': fromdate, 'todate': todate };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById("txtDcNo").value = "";
                    document.getElementById("datepicker").value = "";
                    document.getElementById("txtTodate").value = "";
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function callHandler(d, s, e) {
            $.ajax({
                url: 'DairyFleet.axd',
                data: d,
                type: 'GET',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Edit Receipt<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Edit Receipt</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Edit Receipt Details
                </h3>
            </div>
            <div class="box-body">
                <table align="center">
                    <tr>
                        <td>
                            <label for="lblSalesType">
                                Ref Dc No:</label>
                        </td>
                        <td style="height: 40px;">
                            <input type="text" id="txtDcNo" class="form-control" placeholder="Enter Enter Dc Numbers"/>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap>
                            <label for="lblBranch">
                               From Date</label>
                        </td>
                        <td style="height: 40px;">
                            <input type="date" id="datepicker" placeholder="DD-MM-YYYY" class="form-control" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="lbltodate">
                               To Date</label>
                        </td>
                        <td style="height: 40px;">
                            <input type="date" id="txtTodate" placeholder="DD-MM-YYYY" class="form-control" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <input type="button" id="btnclick" value="Edit DcDate" class="btn btn-primary" onclick="btnupdateDcNumberDates();" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </section>
</asp:Content>


