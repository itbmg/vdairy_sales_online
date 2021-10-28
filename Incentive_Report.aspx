<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Incentive_Report.aspx.cs" Inherits="Incentive_Report" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script src="js/jquery-1.4.4.js" type="text/javascript"></script>--%>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script src="js/jquery-1.4.4.js?v=3004" type="text/javascript"></script>
    <script src="js/newjs/jquery.js?v=3004" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3004" type="text/javascript"></script>
    <link href="jquery.jqGrid-4.5.2/js/i18n/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <script src="jquery.jqGrid-4.5.2/src/i18n/grid.locale-en.js" type="text/javascript"></script>
    <script src="jquery.jqGrid-4.5.2/js/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script src="jquery-ui-1.10.3.custom/js/jquery-ui.js" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js?v=3004" type="text/javascript"></script>
    <script src="js/newjs/jquery-ui.js?v=3004" type="text/javascript"></script>
    <script type="text/javascript">

        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        $(function () {
            FillSalesOffice()
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
            var ddlsaleoffice = document.getElementById('ddlsaleoffice');
            var length = ddlsaleoffice.options.length;
            ddlsaleoffice.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            ddlsaleoffice.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlsaleoffice.appendChild(opt);
                }
            }
        }
        function ddlsaleofficechange() {
          var BranchID= document.getElementById('ddlsaleoffice').value;
          var data = { 'operation': 'GetSalesOfficeChange', 'BranchID': BranchID }; 
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
                    opt.innerHTML = msg[i].routename;
                    opt.value = msg[i].routesno;
                    veh.appendChild(opt);
                }
            }
        }
        function ddlroutenamechange() {
            var RouteID = document.getElementById('ddlRouteName').value;
            var data = { 'operation': 'GetRouteNameChange', 'RouteID': RouteID };
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
                    opt.value = msg[i].b_id;
                    ddlAgentName.appendChild(opt);
                }
            }
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
        function btngenarateIncentive() {
            var BranchID = document.getElementById('ddlsaleoffice').value;
            var Route_id = document.getElementById('ddlRouteName').value;
            var Agent_id = document.getElementById('ddlAgentName').value;
            var fromdate = document.getElementById('txtFromdate').value;
            var todate = document.getElementById('txtTodate').value;
            var data = { 'operation': 'get_Agentwise_incentive_statements', 'BranchID': BranchID, 'Route_id': Route_id, 'Agent_id': Agent_id, 'fromdate': fromdate, 'todate': todate };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fill_agentwiseincentivedetails(msg)
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
        var subincentive = []; var incentive = [];
        function fill_agentwiseincentivedetails(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" border="2" style="width:100%;">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">IndentDate</th><th scope="col">TotalLtrs</th></tr></thead></tbody>';
            var j = 1;
            var totltrs = 0;
            var totlamount = 0;

            var route = document.getElementById("ddlRouteName");
            var routename = route.options[route.selectedIndex].text;

            var agent = document.getElementById("ddlAgentName");
            var AgentName = agent.options[agent.selectedIndex].text;

            var From_date = document.getElementById('txtFromdate').value;
            var To_Date = document.getElementById('txtTodate').value;

            document.getElementById('spnroutename').innerHTML = routename;
            document.getElementById('spnagentname').innerHTML = AgentName;
//            document.getElementById('spndate').innerHTML = "" + From_date + "TO" + To_Date + "";

            incentive = msg[0].incentive;
            subincentive = msg[0].SubIsncentive;
            for (var i = 0; i < incentive.length; i++) {
                results += '<tr><th scope="row" class="1" style="text-align:center;">' + j + '</th>';
                results += '<td data-title="brandstatus" class="2">' + incentive[i].indentdate + '</td>';
                results += '<td data-title="brandstatus" class="tammountcls">' + incentive[i].TotalLtrs + '</td></tr>'
                totltrs += parseFloat(incentive[i].TotalLtrs);
                j++;
            }
            results += '<tr>';
            results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;"></td>';
            results += '<td scope="row" class="1" style="font-size:18px;font-weight:bold;color:#006400;" >Total</td>';
            results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + totltrs + '</td></tr>';
            var t1 = "ClubbingName"; var t2 = "TotalSale"; var t3 = "AverageSale"; var t4 = "DiscountSlot"; var t5 = "TotalAmount";
            results += '<tr><th scope="row" class="1" style="text-align:center;"></th>';
            results += '<td data-title="brandstatus"  class="6">' + t1 + '</td>';
            results += '<td data-title="brandstatus"  class="6">' + t2 + '</td>';
            results += '<td data-title="brandstatus"  class="6">' + t3 + '</td>';
            results += '<td data-title="brandstatus"  class="6">' + t4 + '</td>';
            results += '<td data-title="brandstatus"  class="6">' + t5 + '</td>';
            results += '<td data-title="brandstatus" class="7"><span id="totalcls"></span></td></tr>';
            for (var i = 0; i < subincentive.length; i++) {
                results += '<tr><th scope="row" class="1" style="text-align:center;">' + j + '</th>';
                results += '<td data-title="brandstatus" class="2">' + subincentive[i].ClubbingName + '</td>';
                results += '<td data-title="brandstatus" class="2">' + subincentive[i].TotalSale + '</td>';
                results += '<td data-title="brandstatus" class="2">' + subincentive[i].AverageSale + '</td>';
                results += '<td data-title="brandstatus" class="2">' + subincentive[i].DiscountSlot + '</td>';
                results += '<td data-title="brandstatus" class="2">' + subincentive[i].TotalAmount + '</td></tr>'
                totlamount += parseFloat(subincentive[i].TotalAmount);
                document.getElementById('spnincentive').innerHTML = subincentive[0].IncentiveGiven;
                document.getElementById('spndiscount').innerHTML = totlamount;
                document.getElementById('txtRemarks').innerHTML = subincentive[0].Remarks;
                document.getElementById('spndate').innerHTML = subincentive[0].Date;
                j++;
            }
            results += '<tr>';
            results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;"></td>';
            results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;"></td>';
            results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;"></td>';
            results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;"></td>';
            results += '<td scope="row" class="1" style="font-size:18px;font-weight:bold;color:#006400;" >TotalDiscount</td>';
            results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + totlamount + '</td></tr>';
            results += '</table></div>';
            $("#div_products").html(results);
            //GetTotalCal();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Retail Form <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Retail Form</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Retail Form Details
                </h3>
            </div>
            <div class="box-body">
                <div id='Retail_FillForm'>
                    <table align="center">
                        <tr>
                            <td>
                                <label>
                                    Select Branch
                                </label>
                            </td>
                            <td style="height: 40px;">
                               <select id="ddlsaleoffice" class="form-control" onchange="ddlsaleofficechange()";>
                                </select>
                            </td>
                             </tr>
                             <tr>
                            <td>
                                <label>
                                   Select RouteName</label>
                            </td>
                            <td style="height: 40px;">
                                <select id="ddlRouteName" class="form-control" onchange="ddlroutenamechange()";>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                  Select  AgentName</label>
                            </td>
                            <td style="height: 40px;">
                                <select id="ddlAgentName" class="form-control">
                                </select>
                            </td>
                       </tr>
                             <tr>
                            <td>
                                <label>
                                    Fromdate</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtFromdate" type="date" class="form-control" name="Fromdate" placeholder="Enter  Fromdate" />
                            </td>
                            </tr>
                             <tr>
                            <td>
                                <label>
                                    Todate</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtTodate" type="date" class="form-control" name="Todate" />
                            </td>
                        </tr>
                    </table>
                    <div id="">
                    </div>
                    <table align="center">
                        <tr>
                            <td>
                                <input type="button" class="btn btn-primary" id="btn_generate" value="Genarate" onclick="btngenarateIncentive();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div id="divPrint">
                <div style="width: 100%;">
                    <div style="width: 11%; float: left;">
                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="95px" height="90px" />
                    </div>
                    <div style="left: 0%; text-align: center;">
                        <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="26px" ForeColor="#0252aa"
                            Text=""></asp:Label>
                        <br />
                    </div>
                </div>
                 <table style="width: 100%;">
                        <tr>
                            <td>
                            <label style="font-size: 12px;" float: left;">
                                        AgentName :</label>
                               <span id="spnagentname"></span>
                            </td>
                            <td>
                              <label style="font-size: 12px;" float: left;">
                                        RouteName :</label>
                                <span id="spnroutename"></span>
                            </td>
                            <td>
                              <label style="font-size: 12px;" float: left;">
                                        Date :</label>
                                    <span id="spndate"></span>
                            </td>
                        </tr>
                    </table>
                 <div id="div_products" >
                    </div>
                <div>
                    <table style="width: 100%;">
                        <tr>
                            <td>
                            <label style="font-size: 12px;" float: left;">
                                        ActualDiscount :</label>
                               <span id="spndiscount"></span>
                            </td>
                            <td>
                              <label style="font-size: 12px;" float: left;">
                                        IncentiveGiven :</label>
                                <span id="spnincentive"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                              <label style="font-size: 12px;" float: left;">
                                        Remarks :</label>
                                    <span id="spnremarks"></span>
                            </td>
                            <td>
                                 <textArea id="txtRemarks" type="text" class="form-control" name="Remarks" placeholder="Remarks" > </textArea>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 25%;">
                                <span style="font-weight: bold; font-size: 14px;">Prepared By</span>
                            </td>
                            <td style="width: 25%;">
                                <span style="font-weight: bold; font-size: 14px;">Accounts</span>
                            </td>
                            <td style="width: 25%;">
                                <span style="font-weight: bold; font-size: 14px;">Marketing Manager</span>
                            </td>
                            <td style="width: 25%;">
                                <span style="font-weight: bold; font-size: 14px;">Agent Sign</span>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div>
                <table>
                        <tr>
                        <input id="btnPrint" type="button" class="btn btn-primary" name="submit"  value='Print'
                    onclick="javascript:CallPrint('divPrint');" />
                        </td>
                     </tr>
                </table>
            </div>
</asp:Content>

