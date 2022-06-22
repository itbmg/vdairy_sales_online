<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="editduetransactions.aspx.cs" Inherits="editduetransactions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="Js/jquery.blockUI.js?v=3006" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            FillSalesOffice();
        });
        function FillSalesOffice() {
            var data = { 'operation': 'GetPlantSalesOffice' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindSalesOffice(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindSalesOffice(msg) {
            var ddlsalesOffice = document.getElementById('ddlSalesOffice');
            var length = ddlsalesOffice.options.length;
            ddlsalesOffice.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Sales Office";
            ddlsalesOffice.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlsalesOffice.appendChild(opt);
                }
            }
        }

        function ddlSalesOfficeChange(ID) {
            var BranchID = ID.value;
            var data = { 'operation': 'GetSalesRoutes', 'BranchID': BranchID };
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
            document.getElementById('ddlRouteName').options.length = "";
            var veh = document.getElementById('ddlRouteName');
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
        function ddlRouteNameChange(id) {
            FillAgentName(id.value);
        }
        function FillAgentName(RouteID) {
            var data = { 'operation': 'GetAgents', 'RouteID': RouteID };
            var s = function (msg) {
                if (msg) {
                    BindAgentName(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindAgentName(msg) {
            document.getElementById('ddlAgentName').options.length = "";
            var ddlAgentName = document.getElementById('ddlAgentName');
            var length = ddlAgentName.options.length;
            for (i = length - 1; i >= 0; i--) {
                ddlAgentName.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Agent Name";
            opt.value = "";
            ddlAgentName.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlAgentName.appendChild(opt);
                }
            }
        }

        function Getduetranactions() {
            var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
            if (ddlSalesOffice == "Select Sales Office" || ddlSalesOffice == "") {
                alert("Please Select Sales Office");
                return false;
            }

            var ddlRouteName = document.getElementById('ddlRouteName').value;
            if (ddlRouteName == "Select Route Name") {
                alert("Select Route Name");
                return false;
            }
            var AgentID = document.getElementById('ddlAgentName').value;
            if (AgentID == "Select Agent Name") {
                alert("Select Agent Name");
                return false;
            }



            var txtindentdate = document.getElementById('txtindentdate').value;
            if (txtindentdate == "") {
                alert("Please Select indent Date");
                return false;
            }
            var txtToindentDate = document.getElementById('txtToindentDate').value;
            if (txtToindentDate == "") {
                alert("Please Select To Date");
                return false;
            }
            var data = { 'operation': 'Getduetranacctions', 'branchid': AgentID, 'indentdate': txtindentdate, 'txtToindentDate': txtToindentDate };
            var s = function (msg) {
                if (msg) {
                    var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" id="table_due_transaction_details">';
                    results += '<thead><tr><th scope="col">Agent Name</th><th scope="col">Indent Date</th><th scope="col">OppBalance</th><th scope="col">ReceivedAmount</th><th scope="col">Clo bal</th></tr></thead></tbody>';
                    for (var i = 0; i < msg.length; i++) {
                        results += '<tr>';
                        results += '<td scope="row" class="1" >' + msg[i].branchname + '</td>';
                        results += '<td data-title="Capacity" class="2">' + msg[i].indentdate + '</td>';
                        results += '<td data-title="Capacity" class="3"><input type="text" id="txtclobal" class="form-control" value="' + msg[i].OppBalance + '"/></td>';
                        results += '<td data-title="Capacity" class="3"><input type="text" id="txtclobal" class="form-control" value="' + msg[i].ReceivedAmount + '"/></td>';
                        results += '<td data-title="Capacity" class="3"><input type="text" id="txtclobal" class="form-control" value="' + msg[i].closingbalance + '"/></td>';
                        results += '<td  style="display:none" data-title="Capacity" class="4">' + msg[i].salesofficeid + '</td>';
                        results += '<td style="display:none" data-title="Capacity" class="5">' + msg[i].routeid + '</td>';
                        results += '<td style="display:none" class="6">' + msg[i].agentid + '</td></tr>';
                    }
                    results += '</table></div>';
                    $("#divFillScreen").html(results);
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
        function btn_due_transactions_saveClick() {
            var rows = $("#table_due_transaction_details tr:gt(0)");
            var Indentdetails = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtProductQty').val() != "") {
                    Indentdetails.push({ BranchID: $(this).find('.4').text(), routeid: $(this).find('.5').text(), agentid: $(this).find('.6').text(), clobal: $(this).find('#txtclobal').val() });
                }
            });
            var txtindentdate = document.getElementById('txtindentdate').value;
            if (txtindentdate == "") {
                alert("Please Select indent Date");
                return false;
            }

            var data = { 'operation': 'btn_due_transactions_saveClick', 'data': Indentdetails, 'indentdate': txtindentdate };
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Edit Due Transactions<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Edit Due Transactions</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Due Transactions Details
                </h3>
            </div>
            <div class="box-body">
                <table align="center">
                    <tr>
                        <td>
                            <label for="lblBranch">
                                Sales Office</label>
                        </td>
                        <td>
                            <select id="ddlSalesOffice" class="form-control" onchange="ddlSalesOfficeChange(this);">
                            </select>
                        </td>
                        <td>
                            <label>
                                Route Name</label>
                        </td>
                        <td style="height: 40px;">
                            <select id="ddlRouteName" class="form-control" onchange="ddlRouteNameChange(this);">
                            </select>
                        </td>
                        <td>
                            <label>
                                Agent Name</label>
                        </td>
                        <td style="height: 40px;">
                            <select id="ddlAgentName" class="form-control" onchange="ddlAgentNameChange(this);">
                            </select>
                        </td>
                    <td style="width: 2px;">
                    </td>
                    <td nowrap>
                        <label for="lblBranch">
                            Date</label>
                    </td>
                    <td style="height: 40px;">
                        <input type="date" id="txtindentdate" placeholder="DD-MM-YYYY" class="form-control" />
                    </td>
                    <td style="width: px;">
                    </td>
                    <td nowrap>
                        <label for="lblBranch">
                            Date</label>
                    </td>
                    <td style="height: 40px;">
                        <input type="date" id="txtToindentDate" placeholder="DD-MM-YYYY" class="form-control" />
                    </td>
                    <td style="width: px;">
                    </td>
                    <td>
                        <input type="button" id="Button1" value="Get Due Transactions" class="btn btn-primary"
                            onclick="Getduetranactions();" />
                    </td>
                    </tr>
                </table>
                <div id="divFillScreen">
                </div>
                <table align="center">
                    <tr>
                        <td>
                            <input type="button" id="Button2" value="Save Transactions" class="btn btn-primary"
                                onclick="btn_due_transactions_saveClick();" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </section>
</asp:Content>
