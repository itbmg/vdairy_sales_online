<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="NewDelivery_Report.aspx.cs" Inherits="NewDelivery_Report" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="JIC/JIC.js?v=101" type="text/javascript"></script>
    <script src="JSF/imagezoom.js" type="text/javascript"></script>
    <script src="Plant/Script/fleetscript.js?v=3006" type="text/javascript"></script>
    <script type="text/javascript">
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
        $(function () {
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd
            }
            if (mm < 10) {
                mm = '0' + mm
            }
            var hrs = today.getHours();
            var mnts = today.getMinutes();
            $('#txtdate').val(yyyy + '-' + mm + '-' + dd);
            FillSalesOffice();
            var type = '<%=Session["salestype"] %>';
            if (type == "Plant") {
                $('#div_PBranch').css('display', 'block');
            }
            else {
                $('#div_PBranch').css('display', 'none');
            }
            document.getElementById('lblTitle').innerHTML = '<%=Session["TitleName"] %>';
        });
        function FillSalesOffice() {
            var data = { 'operation': 'get_fillsaleoffice_details_despatch' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindSalesOffice(msg);
                    ddlSalesOffice_SelectedIndexChanged();
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
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlsalesOffice.appendChild(opt);
                }
            }
        }
        function ddlSalesOffice_SelectedIndexChanged() {
            var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
            var data = { 'operation': 'get_fillroute_details_despatch', 'ddlSalesOffice': ddlSalesOffice };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    Bindroutedetails(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function Bindroutedetails(msg) {
            var ddlsalesOffice = document.getElementById('ddlRouteName');
            var length = ddlsalesOffice.options.length;
            ddlsalesOffice.options.length = null;
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlsalesOffice.appendChild(opt);
                }
            }
        }
        function get_delavery_report_details() {
            var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
            var ddlRouteName = document.getElementById('ddlRouteName').value;
            var txtdate = document.getElementById('txtdate').value;
            var data = { 'operation': 'get_delavery_report_details', 'ddlSalesOffice': ddlSalesOffice, 'ddlRouteName': ddlRouteName, 'txtdate': txtdate };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    if (msg == "Deliveries Not Found") {
                        alert(msg);
                        $("#div_grdReports1").empty();
                        $("#div_grdReports2").empty();
                        $("#div_grdReports3").empty();
                        $("#div_grdReports4").empty();
                        $("#div_grdReports5").empty();
                        $("#div_grdReports6").empty();
                    }
                    else {
                        document.getElementById('lblRoute').innerHTML = $("#ddlRouteName :selected").text();
                        document.getElementById('lblDate').innerHTML = document.getElementById('txtdate').value;

                        filldespprod(msg);
                        filldespagent(msg);
                        filldespinv(msg);
                        filldespsaletype(msg);
                        filldespamt(msg);
                        filldespdenom(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function filldespagent(msg) {
            var results = '<div class="divcontainer" style="overflow:auto;"><table id="tbl_grdReports1" class="table table-bordered table-hover dataTable no-footer" border="2" style="width:100%;">';
            results += '<thead><tr ><th scope="col" style="text-align:center;">Variety</th><th scope="col" style="text-align:center;">Quantity</th><th scope="col" style="text-align:center;" >Dispatch Quantity</th><th scope="col" style="text-align:center;" >Returns</th><th scope="col" style="text-align:center;">Leaks</th><th scope="col" style="text-align:center;" >Short</th><th scope="col" style="text-align:center;" >FreeMilk</th><th scope="col" style="text-align:center;" >Sales</th><th scope="col" style="text-align:center;" >Sales Value</th></tr></thead></tbody>';
            if (msg.length > 0) {
            var msg = msg[0].delaveryprod;
//            if (msg.length > 0) {
                for (var i = 0; i < msg.length; i++) {
                    var Variety = msg[i].Variety;
                    if (Variety == "Total") {
                        results += '<tr>';
                        results += '<td class="1" style="font-size:14px;font-weight:bold;color:#006400;">' + msg[i].Variety + '</td>';
                        results += '<td class="6" style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + msg[i].Qty + '</div></td>';
                        results += '<td class="2" style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + msg[i].DispQty + '</div></td>';
                        results += '<td class="3" style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + msg[i].Returns + '</div></td>';
                        results += '<td class="4" style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + msg[i].Leaks + '</div></td>';
                        results += '<td class="7" style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + msg[i].Short + '</div></td>';
                        results += '<td class="5" style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + msg[i].FreeMilk + '</div></td>';
                        results += '<td class="5" style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + msg[i].Sales + '</div></td>';
                        results += '<td class="5" style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + parseFloat(msg[i].SalesValue).toFixed(2) + '</div></td></tr>';
                    }
                    else {
                        results += '<tr>';
                        results += '<td class="1" >' + msg[i].Variety + '</td>';
                        results += '<td class="6"><div style="float: right;">' + msg[i].Qty + '</div></td>';
                        results += '<td class="2 "><div style="float: right;">' + msg[i].DispQty + '</div></td>';
                        results += '<td class="3"><div style="float: right;">' + msg[i].Returns + '</div></td>';
                        results += '<td class="4"><div style="float: right;">' + msg[i].Leaks + '</div></td>';
                        results += '<td class="7"><div style="float: right;">' + msg[i].Short + '</div></td>';
                        results += '<td class="5"><div style="float: right;">' + msg[i].FreeMilk + '</div></td>';
                        results += '<td class="5"><div style="float: right;">' + msg[i].Sales + '</div></td>';
                        results += '<td class="5"><div style="float: right;">' + parseFloat(msg[i].SalesValue).toFixed(2) + '</div></td></tr>';
                    }
                }
                results += '</table></div>';
                $("#div_grdReports1").html(results);
            }
        }
        function filldespprod(msg) {
            var k = 1;
            var results = '<div class="divcontainer" style="overflow:auto;"><table id="tbl_grdReports2" class="table table-bordered table-hover dataTable no-footer" border="2" style="width:100%;">';
            results += '<thead><tr ><th scope="col" style="text-align:center;">SNo</th><th scope="col" style="text-align:center;">Agent Code</th><th scope="col" style="text-align:center;">Agent Name</th><th scope="col" style="text-align:center;">Crates</th><th scope="col" style="text-align:center;">Oppening Balance</th><th scope="col" style="text-align:center;" >Sale Value</th><th scope="col" style="text-align:center;" >Amount To Be Paid</th><th scope="col" style="text-align:center;" >Paid Amount</th><th scope="col"  style="text-align:center;">ToDay Due</th><th scope="col"  style="text-align:center;">Due Amount</th></tr></thead></tbody>';
            if (msg.length > 0) {
                var msg = msg[0].delaveryagent;
                for (var i = 0; i < msg.length; i++) {
                    if (msg[i].OppeningBalance == "" && msg[i].SaleValue == "" && msg[i].AmountToBePaid == "" && msg[i].PaidAmount == "" && msg[i].DueAmount == "" && msg[i].ToDayDueAmount == "") {
                        results += '<tr style="background-color: darksalmon;" >';
                        results += '<td style="border:none" class="1" ></td>';
                        results += '<td style="border:none"  class="2 "></td>';
                        results += '<td style="border:none" class="3"></td>';
                        results += '<td style="border:none" class="4"></td>';
                        results += '<td style="border:none;font-size:14px;font-weight:bold;color:#2623ea;" class="6">' + msg[i].AgentCode + '</td>';
                        results += '<td style="border:none"  class="7"></td>';
                        results += '<td  style="border:none" class="5"></td>';
                        results += '<td style="border:none" class="5"></td>';
                        results += '<td style="border:none" class="5"></td>';
                        results += '<td style="border:none" class="5"></td></tr>';
                    }
                    else {
                        var Crates = msg[i].Crates;
                        var OppeningBalance = parseFloat(msg[i].OppeningBalance) || 0;
                        var SaleValue = parseFloat(msg[i].SaleValue) || 0;
                        var AmountToBePaid = parseFloat(msg[i].AmountToBePaid) || 0;
                        var PaidAmount = parseFloat(msg[i].PaidAmount) || 0;
                        var DueAmount = parseFloat(msg[i].DueAmount) || 0;
                        var ToDayDueAmount = parseFloat(msg[i].ToDayDueAmount) || 0;
                        var AgentName = msg[i].AgentName;
                        if (AgentName == "Total") {
                            results += '<tr>';
                            results += '<td class="1" ></td>';
                            results += '<td class="6" style="font-size:14px;font-weight:bold;color:#006400;">' + msg[i].AgentCode + '</td>';
                            results += '<td class="2" style="font-size:14px;font-weight:bold;color:#006400;">' + msg[i].AgentName + '</td>';
                            results += '<td class="3" style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + msg[i].Crates + '</div></td>';
                            results += '<td class="4" style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + parseFloat(OppeningBalance).toFixed(2) + '</div></td>';
                            results += '<td class="7" style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + parseFloat(SaleValue).toFixed(2) + '</div></td>';
                            results += '<td class="5" style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + parseFloat(AmountToBePaid).toFixed(2) + '</div></td>';
                            results += '<td class="5" style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + parseFloat(PaidAmount).toFixed(2) + '</div></td>';
                            results += '<td class="5" style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + parseFloat(ToDayDueAmount).toFixed(2) + '</div></td>';
                            results += '<td class="5" style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + parseFloat(DueAmount).toFixed(2) + '</div></td></tr>';
                        }
                        else {
                            results += '<tr>';
                            results += '<td class="1" >' + (k++) + '</td>';
                            results += '<td class="6">' + msg[i].AgentCode + '</td>';
                            results += '<td class="2 ">' + msg[i].AgentName + '</td>';
                            results += '<td class="3"><div style="float: right;">' + msg[i].Crates + '</div></td>';
                            results += '<td class="4"><div style="float: right;">' + parseFloat(OppeningBalance).toFixed(2) + '</div></td>';
                            results += '<td class="7"><div style="float: right;">' + parseFloat(SaleValue).toFixed(2) + '</div></td>';
                            results += '<td class="5"><div style="float: right;">' + parseFloat(AmountToBePaid).toFixed(2) + '</div></td>';
                            results += '<td class="5"><div style="float: right;">' + parseFloat(PaidAmount).toFixed(2) + '</div></td>';
                            results += '<td class="5"><div style="float: right;">' + parseFloat(ToDayDueAmount).toFixed(2) + '</div></td>';
                            results += '<td class="5"><div style="float: right;">' + parseFloat(DueAmount).toFixed(2) + '</div></td></tr>';
                        }
                    }
                }
                results += '</table></div>';
                $("#div_grdReports2").html(results);
            }
        }
        function filldespinv(msg) {
            var results = '<div class="divcontainer" style="overflow:auto;"><table  id="tbl_grdReports3" class="table table-bordered table-hover dataTable no-footer" border="2" style="width:100%;">';
            results += '<thead><tr ><th scope="col" style="text-align:center;">Inventory</th><th scope="col" style="text-align:center;">Opening</th><th scope="col" style="text-align:center;">Issued</th><th scope="col" style="text-align:center;">Received</th><th scope="col" style="text-align:center;">Difference</th><th scope="col" style="text-align:center;" >Closing</th></tr></thead></tbody>';
            if (msg.length > 0) {
            var msg = msg[0].delaveryinven;
//            if (msg.length > 0) {
                for (var i = 0; i < msg.length; i++) {
                    var Opp = msg[i].Opp;
                    var Issued = msg[i].Issued;
                    var Received = msg[i].Received;
                    var Difference = msg[i].Difference;
                    var Closing = msg[i].Closing;
                    if (Opp == "0" && Issued == "0" && Received == "0" && Difference == "0" && Closing == "0") {
                    }
                    else {
                        results += '<tr>';
                        results += '<td class="1" >' + msg[i].Inventory + '</td>';
                        results += '<td class="6"><div style="float: right;">' + msg[i].Opp + '</div></td>';
                        results += '<td class="2 "><div style="float: right;">' + msg[i].Issued + '</div></td>';
                        results += '<td class="3"><div style="float: right;">' + msg[i].Received + '</div></td>';
                        results += '<td class="4"><div style="float: right;">' + msg[i].Difference + '</div></td>';
                        results += '<td class="5"><div style="float: right;">' + msg[i].Closing + '</div></td></tr>';
                    }
                }
                results += '</table></div>';
                $("#div_grdReports3").html(results);
            }
        }
        function filldespsaletype(msg) {
            var results = '<div class="divcontainer" style="overflow:auto;"><table  id="tbl_grdReports4" class="table table-bordered table-hover dataTable no-footer" border="2" style="width:100%;">';
            results += '<thead><tr ><th scope="col"  style="text-align:center;">Sales Type</th><th scope="col"  style="text-align:center;">Sale Value</th><th scope="col"  style="text-align:center;" >Paid Amount</th><th scope="col"  style="text-align:center;">Due Value</th></tr></thead></tbody>';
           if (msg.length > 0) {
            var msg = msg[0].delaverysalety;
//            if (msg.length > 0) {
                for (var i = 0; i < msg.length; i++) {
                    var SalesType = msg[i].SalesType;
                    if (SalesType == "Total") {
                        results += '<tr>';
                        results += '<td class="1"  style="font-size:14px;font-weight:bold;color:#006400;" >' + msg[i].SalesType + '</td>';
                        results += '<td class="6"  style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + parseFloat(msg[i].SaleValue).toFixed(2) + '</div></td>';
                        results += '<td class="2"  style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + parseFloat(msg[i].PaidAmount).toFixed(2) + '</div></td>';
                        results += '<td class="5"  style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + parseFloat(msg[i].DueValue).toFixed(2) + '</div></td></tr>';
                    }
                    else {
                        results += '<tr>';
                        results += '<td class="1" >' + msg[i].SalesType + '</td>';
                        results += '<td class="6"><div style="float: right;">' + parseFloat(msg[i].SaleValue).toFixed(2) + '</div></td>';
                        results += '<td class="2 "><div style="float: right;">' + parseFloat(msg[i].PaidAmount).toFixed(2) + '</div></td>';
                        results += '<td class="5"><div style="float: right;">' + parseFloat(msg[i].DueValue).toFixed(2) + '</div></td></tr>';
                    }
                }
                results += '</table></div>';
                $("#div_grdReports4").html(results);
            }
        }
        function filldespamt(msg) {
            var results = '<div class="divcontainer" style="overflow:auto;"><table  id="tbl_grdReports5" class="table table-bordered table-hover dataTable no-footer" border="2" style="width:100%;">';
            results += '<thead><tr ><th scope="col"  style="text-align:center;">Sales Type</th><th scope="col"  style="text-align:center;">Amount</th></tr></thead></tbody>';
            if (msg.length > 0) {
            var msg = msg[0].delaverysaletyamt;
//            if (msg.length > 0) {
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr>';
                    results += '<td class="1" >' + msg[i].SaleType + '</td>';
                    results += '<td class="6"><div style="float: right;">' + parseFloat(msg[i].Amount).toFixed(2) + '</div></td></tr>';
                }
                results += '</table></div>';
                $("#div_grdReports5").html(results);
            }
        }
        function filldespdenom(msg) {
            var results = '<div class="divcontainer" style="overflow:auto;"><table  id="tbl_grdReports6" class="table table-bordered table-hover dataTable no-footer" border="2" style="width:100%;">';
            results += '<thead><tr ><th scope="col"  style="text-align:center;">Cash</th><th scope="col"  style="text-align:center;">Count</th><th scope="col"  style="text-align:center;">Amount</th></tr></thead></tbody>';
            if (msg.length > 0) {
                var msg = msg[0].delaverydenomi;
                for (var i = 0; i < msg.length; i++) {
                    var Cash = msg[i].Cash;
                    document.getElementById("lblEmpName").innerHTML = msg[i].EmpName;
                    if (Cash == "Total Amount") {
                        results += '<tr>';
                        results += '<td class="1"  style="font-size:14px;font-weight:bold;color:#006400;" >' + msg[i].Cash + '</td>';
                        results += '<td class="6"  style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + msg[i].Count + '</div></td>';
                        results += '<td class="6"  style="font-size:14px;font-weight:bold;color:#006400;"><div style="float: right;">' + msg[i].Amount + '</div></td></tr>';
                    }
                    else {
                        results += '<tr>';
                        results += '<td class="1" >' + msg[i].Cash + '</td>';
                        results += '<td class="6"><div style="float: right;">' + msg[i].Count + '</div></td>';
                        results += '<td class="6"><div style="float: right;">' + msg[i].Amount + '</div></td></tr>';
                    }
                }
                results += '</table></div>';
                $("#div_grdReports6").html(results);
            }
        }
        function CallPrint(strid) {
            document.getElementById("tbl_grdReports1").style.borderCollapse = "collapse";
            document.getElementById("tbl_grdReports2").style.borderCollapse = "collapse";
            document.getElementById("tbl_grdReports3").style.borderCollapse = "collapse";
            document.getElementById("tbl_grdReports4").style.borderCollapse = "collapse";
            document.getElementById("tbl_grdReports5").style.borderCollapse = "collapse";
            document.getElementById("tbl_grdReports6").style.borderCollapse = "collapse";
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Delivery Report<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Delivery Report</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Delivery Report Details
                </h3>
            </div>
            <div class="box-body">
                <div id="div_PBranch">
                    <table>
                        <tr>
                            <td>
                               <label>Sales Office:</label> 
                            </td>
                            <td>
                                <select id="ddlSalesOffice"  class="form-control" onchange="ddlSalesOffice_SelectedIndexChanged();">
                                </select>
                            </td>
                            <%--<td style="width: 5px;">
                            </td>
                            <td>
                                <input id="chkDispatch" type="checkbox" onchange="chkDispatch_CheckedChanged" ></input>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <select id="ddlPlantDispName"  class="form-control">
                                </select>
                            </td>--%>
                        </tr>
                    </table>
                </div>
                <br />
                <div  id="d">
                    <table>
                        <tr>
                            <td>
                                <label id="lblrouten" >Route Name:</label>
                            </td>
                            <td>
                                <select id="ddlRouteName" class="form-control">
                                </select>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <label id="lbltxtdt" >Date:</label>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <input id="txtdate" type="date" class="form-control"></input> 
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <input id="btnGenerate" type="button" value="Generate"
                                    class="btn btn-primary" onclick="get_delavery_report_details();" />
                            </td>
                        </tr>
                    </table>
                        <div id="divPrint">
                            <div style="width: 100%;">
                                <div style="width: 11%; float: left;">
                                    <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="95px" height="90px" />
                                </div>
                                <div style="left: 0%; text-align: center;">
                                    <label id="lblTitle"  style="color:#0252AA;font-size:20px;font-weight:bold;"></label>
                                    <br />
                                </div>
                            </div>
                            <div align="center">
                                <span style="font-size: 18px; text-decoration: underline; color: #0252aa;">DELIVERY REPORT</span>
                            </div>
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 15%;">
                                       <label> Route Name : </label>
                                    </td>
                                    <td style="width: 15%;">
                                        <label id="lblRoute"  Text="" ForeColor="Red"></label>
                                    </td>
                                    <td style="width: 15%;">
                                        <label> Date : </label>
                                    </td>
                                    <td style="width: 15%;">
                                        <label id="lblDate"  Text="" ForeColor="Red"></label>
                                    </td>
                                    <td style="width: 15%;">
                                       <label> Employee Name : </label>
                                    </td>
                                    <td style="width: 15%;">
                                        <label id="lblEmpName"  Text="" ForeColor="Red"></label>
                                    </td>
                                </tr>
                            </table>
                                <div id="div_grdReports1"></div>
                                <br />
                                <div id="div_grdReports2"></div>
                                <br />
                                <div id="div_grdReports3"></div>
                                <br />
                                <div style="width:100%;">
                                    <div style="width:40%;float:left;">
                                        <div id="div_grdReports4"></div>
                                    </div>
                                    <div style="width:2%;float:left;">
                                    <br />
                                    </div>
                                    <div style="width:30%;float:left;">
                                        <div id="div_grdReports5"></div>
                                    </div>
                                    <div style="width:2%;float:left;">
                                    <br />
                                    </div>
                                    <div style="width:26%;float:left;">
                                    <div id="div_grdReports6"></div>
                             </div>
                           </div>
                           <br />
                           <br />
                           <br />
                            <div style="padding-top:3%;"> 
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 25%;">
                                            <span style="font-weight: bold">SALESMAN SIGNATURE</span>
                                        </td>
                                        <td style="width: 25%;">
                                            <span style="font-weight: bold">DESPATCHER SIGNATURE</span>
                                        </td>
                                        <td style="width: 25%;">
                                            <span style="font-weight: bold">SALES EXECUTIVE SIGNATURE</span>
                                        </td>
                                        <td style="width: 25%;">
                                            <span style="font-weight: bold">CASHIER SIGNATURE</span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div>
                        <input id="btnPrint" class="btn btn-primary" type="button" value="Print" onclick="javascript:CallPrint('divPrint');"/>
                </div>
            </div>
        </div>
      </div>
  </section>
</asp:Content>


