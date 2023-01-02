<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Indent_Monitering.aspx.cs" Inherits="Indent_Monetering" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=300,height=300,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.write('<link rel="stylesheet" type="text/css" href="Css/print.css" />');
            newWin.document.close();
        }
    </script>
    <script type="text/javascript">
        $(function () {
            FillRoutes();
            var date = new Date();
            var fromday = date.getDate() - 2;
            var today = date.getDate() - 1;
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (fromday < 10) fromday = "0" + fromday;
            var fromdate = year + "-" + month + "-" + fromday;
            var todate = year + "-" + month + "-" + today;
            $('#txtfromdate').val(fromdate);
            $('#txttodate').val(todate);
        });
        function FillRoutes() {
            var data = { 'operation': 'GetPlantSalesOffice' };
            var s = function (msg) {
                if (msg) {
                    bindRoutes(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function bindRoutes(msg) {
            var ddlRouteName = document.getElementById('ddlRouteName');
            var length = ddlRouteName.options.length;
            ddlRouteName.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select";
            ddlRouteName.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlRouteName.appendChild(opt);
                }
            }
        }
        function fillRouteNames() {
            var salesofficeid = document.getElementById('ddlRouteName').value;
            var data = { 'operation': 'GetSalesRoutes', 'BranchID': salesofficeid };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindRouteName(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function BindRouteName(msg) {
            document.getElementById('ddl_dispatchname').options.length = "";
            var veh = document.getElementById('ddl_dispatchname');
            var length = veh.options.length;
            for (i = length - 1; i >= 0; i--) {
                veh.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Route Name";
            opt.value = "";
            veh.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].RouteName;
                    opt.value = msg[i].rid;
                    veh.appendChild(opt);
                }
            }
        }
        function GetIndentValues() {
            var ddlRouteName = document.getElementById('ddlRouteName').value;
            if (ddlRouteName == "Select" || ddlRouteName == "") {
                alert("Please Select Salesoffice Name");
                return false;
            }
            var ddldispatchname = document.getElementById('ddl_dispatchname').value;
            if (ddldispatchname == "Select" || ddldispatchname == "") {
                alert("Please Select Route Name");
                return false;
            }
            var fromdate = document.getElementById('txtfromdate').value;
            if (fromdate == "") {
                alert("Please Select From Date");
                return false;
            }
            var todate = document.getElementById('txttodate').value;
            if (todate == "") {
                alert("Please Select To Date");
                return false;
            }
            document.getElementById('spnfromdate').innerHTML = fromdate;
            document.getElementById('spntodate').innerHTML = todate;
            var data = { 'operation': 'GetIndentDetails', 'RouteID': ddlRouteName, 'ddldispatchname': ddldispatchname, 'fromdate': fromdate, 'todate': todate };
            var s = function (msg) {
                if (msg) {
                    $('#divFillScreen').removeTemplate();
                    $('#divFillScreen').setTemplateURL('Indent_Approval.htm');
                    $('#divFillScreen').processTemplate(msg);

                    $('#btnsave').css("display", "block");
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);

        }
        function btnApproveSaveClick() {
            var rows = $("#table_Indent_Approval_details tr:gt(0)");
            var Indentdetails = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtProductName').text() != "") {
                    Indentdetails.push({ Agentid: $(this).find('#hdnagentid').val(), Routeid: $(this).find('#hdnrouteid').val(), Indent_Yesterday: $(this).find('#txt_indent_yesterday').text(), Indent_Today: $(this).find('#txt_indent_today').text(), Indent_Increase: $(this).find('#txtincreasepercent').text(), Indent_Decrease: $(this).find('#txtdecreasepercent').text(), Reason: $(this).find('#ddlreason').val(), Remarks: $(this).find('#txtremarks').val() });
                }
            });
            var data = { 'operation': 'btnIndentApproveSaveClick', 'data': Indentdetails };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(data, s, e);
        }
        function CallHandlerUsingJson(d, s, e) {
            $.ajax({
                type: "GET",
                url: "DairyFleet.axd?json=",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(d),
                async: true,
                cache: true,
                success: s,
                error: e
            });
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
            Indent Monitoring<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Indent Monitoring</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Indent Details
                </h3>
            </div>
            <div class="box-body">
                <table>
                    <tr>
                    </tr>
                    <tr>
                        <td nowrap>
                            <label for="lblBranch">
                                SalesOffice</label>
                        </td>
                        <td>
                            <select id="ddlRouteName" class="form-control" onchange="fillRouteNames();">
                            </select>
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                            <lable id="lblroutename">Route</lable>
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                            <select id="ddl_dispatchname" class="form-control">
                            </select>
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                            <label>
                                From Date:</label>
                        </td>
                        <td>
                            <input type="date" id="txtfromdate" placeholder="DD-MM-YYYY" class="form-control" />
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                            <label>
                                To Date:</label>
                        </td>
                        <td>
                            <input type="date" id="txttodate" placeholder="DD-MM-YYYY" class="form-control" />
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                            <input type="button" id="Button1" value="Get Indent Details" class="btn btn-primary"
                                onclick="GetIndentValues();" />
                        </td>
                    </tr>
                </table>
                <div id="divPrint">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                From Date
                            </td>
                            <td>
                                <span id="spnfromdate"></span>
                            </td>
                            <td>
                                To Date
                            </td>
                            <td>
                                <span id="spntodate"></span>
                            </td>
                        </tr>
                    </table>
                    <div id="divFillScreen">
                    </div>
                    <div id="signature">
                    </div>
                </div>
            </div>
            <div align="center">
                <input type="button" id="btnsave" value="Save" onclick="btnApproveSaveClick();" style="display: none;"
                    class="btn btn-primary" />
            </div>
            <br />
            <br />
            <div>
                <input type="button" class="btn btn-primary" name="submit" id="btnPrint" value='Print'
                    onclick="javascript:CallPrint('divPrint');" />
                <br />
                <br />
            </div>
        </div>
    </section>
</asp:Content>
