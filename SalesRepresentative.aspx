<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SalesRepresentative.aspx.cs" Inherits="SalesRepresentative" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="Js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            FillSalesOffice();
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#txtFromDate').val(today);
            $('#txtToDate').val(today);
        });
        function CallPrint(strid) {
            //            var prtContent = document.getElementById(strid);
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
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
        function BtnGenerateClick() {
            var Branchid = document.getElementById("ddlSalesOffice").value;
            var fromdate = document.getElementById("txtFromDate").value;
            var ToDate = document.getElementById("txtToDate").value;
            if (fromdate == "") {
                alert("Select From Date");
                return false;
            }
            if (ToDate == "") {
                alert("Select To Date");
                return false;
            }
            var data = { 'operation': 'get_SaleRepresentive_details', 'Branchid': Branchid, 'fromdate': fromdate, 'ToDate': ToDate };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindSaleRepresentive(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        
        var totqty = 0;
        var grand_totqty = 0;
        function BindSaleRepresentive(msg) {
            var BranchTable = [];
            var results = '<div class="divcontainer" style="overflow:auto;"><table border="1" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sales Representative</th><th scope="col">Agent Name</th><th scope="col">RouteName</th><th scope="col">AvgQty</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                if (BranchTable.indexOf(msg[i].sale_representive) == -1) {
                    if (i == 0) {
                    }
                    else {
                        results += '<tr>';
                        results += '<td scope="row" class="1" ></td>';
                        results += '<td scope="row" class="1"  style="font-size:18px;font-weight:bold;color:#006400;">Total</td>';
                        results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;"></td>'
                        results += '<td  class="tammountcls" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totqty).toFixed(2) + '</td>';
                        results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;"></td></tr>';
                        grand_totqty += parseFloat(totqty);
                        totqty = 0;
                    }
                    totqty = 0;
                    results += '<tr>';
                    results += '<td scope="row" class="1"  style="font-size:18px;font-weight:bold;color:blue;">' + msg[i].sale_representive + '</td>';
                    results += '<td scope="row" class="1" >' + msg[i].AgentName + '</td>';
                    results += '<td  class="2">' + msg[i].RouteName + '</td>';
                    results += '<td  class="tammountcls">' + msg[i].Qty + '</td></tr>';
                    totqty += parseFloat(msg[i].Qty);
                    grand_totqty += parseFloat(totqty);
                    BranchTable.push(msg[i].sale_representive);
                }
                else {
                    results += '<tr>';
                    results += '<td scope="row" class="1" ></td>';
                    results += '<td scope="row" class="1" >' + msg[i].AgentName + '</td>';
                    results += '<td  class="2">' + msg[i].RouteName + '</td>';
                    results += '<td  class="tammountcls">' + msg[i].Qty + '</td></tr>';
                    totqty += parseFloat(msg[i].Qty);
                    // grand_totqty += parseFloat(totqty);
                }
            }
            results += '<tr>';
            results += '<td scope="row" class="1" ></td>';
            results += '<td scope="row" class="1" style="font-size:18px;font-weight:bold;color:#006400;" >Total</td>';
            results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;"></td>';
            results += '<td  class="tammountcls" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totqty).toFixed(2) + '</td></tr>';
            results += '<tr>';
            results += '<td scope="row" class="1" ></td>';
            results += '<td scope="row" class="1" style="font-size:22px;font-weight:bold;color:#B22222;" >Grand Total</td>';
            results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;"></td>';
            results += '<td data-title="brandstatus" class="5"><span id="totalcls" class="badge bg-yellow" style="font-size:18px;font-weight:bold;color:#B22222;"></span></td></tr>';
            results += '</table></div>';
            $("#div_SalesRepresentive").html(results);
            grand_totqty = 0;
            grandtotalinv();
        }
        function grandtotalinv() {
            var totamount = 0;
            $('.tammountcls').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totamount += parseFloat(qtyclass);
                }
            });
            document.getElementById('totalcls').innerHTML = parseFloat(totamount).toFixed(2);
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
            Sales Representative<small>Preview</small>
        </h1>
        <ol class="breadcrumb" >
            <li><a href="#">Sales Representative</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Sales Representative Details
                </h3>
            </div>
            <div class="box-body">
                <br />
                <table align="center">
                <tr>
                <td>
                                       Sales Office:
                                        </td>
                                        <td>
                                        <select id="ddlSalesOffice" class="form-control">
                                        </select>
                                    </td>
                  <td>
                                    From Date
                                </td>
                                <td>
                                    <input type="date" id="txtFromDate" class="formDate" />
                                </td>
                                <td>
                                    To Date
                                </td>
                                <td>
                                    <input type="date" id="txtToDate" class="formDate" />
                                </td>
                                <td  style="width:6px;"></td>
                                <td> 
                                    <input type="button" id="btnGeneretae" value="Generate" onclick="BtnGenerateClick();"
                                        class="btn btn-primary" />
                                </td>
                </tr>
                </table>
                <br />
                  <div id="divPrint">
                 <div id="div_SalesRepresentive"></div>
                </div>
                <br />
                 <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="javascript:CallPrint('divPrint');"><i class="fa fa-print"></i> Print </button>
                </div>
                
                </div>
                </section>
</asp:Content>
