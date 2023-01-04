<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="AgentDueTransactions.aspx.cs" Inherits="AgentDueTransactions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="Js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #content
        {
            position: absolute;
            z-index: 1;
        }
        
    </style>
    <script type="text/javascript">
//        function CallPrint(strid) {
//            document.getElementById("tbl_po_print").style.borderCollapse = "collapse";
//            var divToPrint = document.getElementById(strid);
//            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
//            newWin.document.open();
//            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
//            newWin.document.close();
//        }
    </script>
    <script type="text/javascript">
        $(function () {
            FillSalesOffice()
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#txtFrom_date').val(today);
//            $('#txtTodate').val(today);
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
            opt.innerHTML = "select";
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
        
        function Get_AgentWiseDue_Transaction_Report() {
            var fromdate = document.getElementById('txtFrom_date').value;
//            var todate = document.getElementById('txtTodate').value;

           
            var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
            if (fromdate == "") {
                alert("Please select from date");
                return false;
            }
            
            var data = { 'operation': 'Get_AgentWiseDue_Transaction_Report', 'fromdate': fromdate,  'BranchId': ddlSalesOffice };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Data not found") {
                        alert(msg);
                        return false;
                    }
                    if (msg.length > 0) {
                        filldetails(msg);
                        $("#divPrint").css("display", "block");
                        $("#btn_Print").css("display", "block");
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


        function filldetails(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" ID="tabledetails">';
            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">RouteCode</th><th scope="col" class="thcls">AgentCode</th><th scope="col" class="thcls">AgentName</th><th scope="col">OppeningBalance</th><th scope="col">SaleQty</th><th scope="col">SaleValue</th><th scope="col">ReceivedAmount</th><th scope="col">ClosingAmount</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row" class="1 tdmaincls" id="txtRouteCode"  name="RouteCode" class="clsdesc" value="' + msg[i].RouteCode + '">' + msg[i].RouteCode + '</td>';
                results += '<td data-title="brandstatus"  class="2" id="txtAgentCode"  name="AgentCode" class="clsAgentCode" value="' + msg[i].AgentCode + '" >' + msg[i].AgentCode + '</td>';
                results += '<td data-title="brandstatus"  class="3" id="txtAgentName"  name="AgentName" class="clsAgentName" value="' + msg[i].AgentName + '">' + msg[i].AgentName + '</td>';
                results += '<td data-title="brandstatus" class="4" id="txtOppeningBalance"  name="OppeningBalance" class="clsOppeningBalance" value="' + msg[i].OppeningBalance + '">' + msg[i].OppeningBalance + '</td>';
                results += '<td data-title="brandstatus"  class="5" id="txtSaleQty"  name="SaleQty" class="clsSaleQty" value="' + msg[i].SaleQty + '">' + msg[i].SaleQty + '</td>';
                results += '<td data-title="brandstatus" class="6" id="txtSaleValue"  name="SaleValue" class="clsSaleValue" value="' + msg[i].SaleValue + '">' + msg[i].SaleValue + '</td>';
                results += '<td data-title="brandstatus"  class="7" id="txtReceivedAmount"  name="ReceivedAmount" class="clsReceivedAmount" value="' + msg[i].ReceivedAmount + '">' + msg[i].ReceivedAmount + '</td>';
                results += '<td data-title="brandstatus" class="8" id="txtClosingAmount"  name="ClosingAmount" class="clsClosingAmount" value="' + msg[i].ClosingAmount + '">' + msg[i].ClosingAmount + '</td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_itemdetails").html(results);
        }


        var fillduetrasaction = [];
        function Save_DueTransaction() {
            var fromdate = document.getElementById('txtFrom_date').value;
            var btnval = document.getElementById('btn_save').value;
            var Branchid = document.getElementById('ddlSalesOffice').value;
//            Get_AgentWiseDue();
            $('#tabledetails> tbody > tr').each(function () {
                var RouteCode = $(this).find('#txtRouteCode').text();
                var AgentCode = $(this).find('#txtAgentCode').text();
                var AgentName = $(this).find('#txtAgentName').text();
                var OppeningBalance = $(this).find('#txtOppeningBalance').text();
                var SaleQty = $(this).find('#txtSaleQty').text();
                var SaleValue = $(this).find('#txtSaleValue').text();
                var ReceivedAmount = $(this).find('#txtReceivedAmount').text();
                var ClosingAmount = $(this).find('#txtClosingAmount').text();
                if (RouteCode == "") {
                }
                else {
                    fillduetrasaction.push({ 'RouteCode': RouteCode, 'AgentCode': AgentCode, 'AgentName': AgentName, 'OppeningBalance': OppeningBalance, 'SaleQty': SaleQty, 'SaleValue': SaleValue, 'ReceivedAmount': ReceivedAmount, 'ClosingAmount': ClosingAmount });
                }
            });
            var Data = { 'op': 'Save_Agent_Due_Transaction_Details', 'btnval': btnval, 'fromdate': fromdate, 'Branchid': Branchid, 'fillduetrasaction': fillduetrasaction };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
//                    get_Stock_Closing_Details();
                    scrollTo(0, 0);
                }
            }
            var e = function (x, h, e) {
            };
            CallHandlerUsingJson_POST(Data, s, e);
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
        function CallHandlerUsingJson_POST(d, s, e) {
            d = JSON.stringify(d);
            d = encodeURIComponent(d);
            $.ajax({
                type: "POST",
                url: "DairyFleet.axd",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: d,
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
             Agent Invoice<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Masters</a></li>
            <li><a href="#"> Agent Invoice</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
        <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Agent Invoice Details
                </h3>
            </div>
            <div class="box-body">
  <table>
                            <tr>
                                <td >
                                 <select id="ddlSalesOffice" class="form-control" onchange="ddlSalesOfficeChanged(this);">
                                        </select>
                                </td>
                               
                                             <td style="width: 5px;">
                            </td>
                                <td>
                                  <input type="date" id="txtFrom_date" class="form-control"/>
                                </td>
                            <td style="width: 5px;">
                            </td>
                            
                            <td>
                            <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="Get_AgentWiseDue_Transaction_Report()"><i class="fa fa-refresh"></i> Get Details </button>
                            </td>
                        </tr>
                    </table>
                    <br />
                     <br />
                    <div id="divPrint" style="display: none;height:50%;">
                   <div class="content">
                
                   <div id="div_itemdetails">
                    </div>
                    <input type="button" value="save" class="form-control" id="btn_save" onclick="Save_DueTransaction();" />
                </div>
                </div>
                </div>
                </div>
                </div>
                </div>
                </section>
</asp:Content>
