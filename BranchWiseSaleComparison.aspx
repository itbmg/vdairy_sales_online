<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BranchWiseSaleComparison.aspx.cs" Inherits="BranchWiseSaleComparison" %>

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
            $('#txtFromDate').val(yyyy + '-' + mm + '-' + dd);
            FillSalesOffice();
//            $('#div_PBranch').css('display', 'block');
            var type = '<%=Session["salestype"] %>';
//            if (type == "Plant") {
//                $('#div_PBranch').css('display', 'block');
//            }
//            else {
//                //$('#div_PBranch').css('display', 'none');
//            }
//            document.getElementById('lblTitle').innerHTML = '<%=Session["TitleName"] %>';
        });
        function FillSalesOffice() {
            var test1 = "Report";
            var data = { 'operation': 'GetSalesOffices', 'SelecteType': test1 };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindSalesOffice(msg);
                    //                    ddlSalesOffice_SelectedIndexChanged();
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
//        function ddlSalesOffice_SelectedIndexChanged() {
//            var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
//            var data = { 'operation': 'get_fillroute_details_despatch', 'ddlSalesOffice': ddlSalesOffice };
//            var s = function (msg) {
//                if (msg) {
//                    if (msg == "Session Expired") {
//                        alert(msg);
//                        window.location = "Login.aspx";
//                    }
//                    Bindroutedetails(msg);
//                    ddlRouteNameChange()
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
//        }
//        function Bindroutedetails(msg) {
//            var ddlsalesOffice = document.getElementById('ddlRouteName');
//            var length = ddlsalesOffice.options.length;
//            ddlsalesOffice.options.length = null;
//            for (var i = 0; i < msg.length; i++) {
//                if (msg[i].BranchName != null) {
//                    var opt = document.createElement('option');
//                    opt.innerHTML = msg[i].BranchName;
//                    opt.value = msg[i].Sno;
//                    ddlsalesOffice.appendChild(opt);
//                }
//            }
//        }

//        function ddlRouteNameChange() {
//            
//          var  Routeid = document.getElementById('ddlRouteName').value;
//            if (Routeid == "Select Route" || Routeid == "") {
//                alert("Please Select Route Name");
//                return false;
//            }
//            var data = { 'operation': 'getAgent_Name', 'Routeid': Routeid };
//            var s = function (msg) {
//                if (msg) {
//                    BindAgentName(msg);
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
//        }
//        function BindAgentName(msg) {
//            document.getElementById('ddlAgentName').options.length = "";
//            var veh = document.getElementById('ddlAgentName');
//            var length = veh.options.length;
//            for (i = length - 1; i >= 0; i--) {
//                veh.options[i] = null;
//            }
//            var opt = document.createElement('option');
//            opt.innerHTML = "Select AgentName";
//            opt.value = "";
////            opt.innerHTML = "ALL";
////            opt.value = "ALL";
//            veh.appendChild(opt);
//            for (var i = 0; i < msg.length; i++) {
//                if (msg[i] != null) {
//                    var opt = document.createElement('option');
//                    opt.innerHTML = msg[i].AgentName;
//                    opt.value = msg[i].sno;
//                    veh.appendChild(opt);
//                }
//            }
//        }
        function BranchIndentDayWiseComparison() {
            var FromDate = document.getElementById('txtFromDate').value;
            var Todate = document.getElementById('txtFromDate').value;
            var BranchId = document.getElementById('ddlSalesOffice').value;
            // $('#divHide').css('display', 'block');
            var test = "Report";
            var data = { 'operation': 'Branch_Wise_DayComparism', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate, 'test': test };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    fill_Branch_Wise_DayIndent(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fill_Branch_Wise_DayIndent(msg) {
            scrollTo(0, 0);
            $('#divMainAddNewRow1').css('display', 'block');
            j = 1;
            var results = '<div><table class="table table-bordered table-hover dataTable no-footer" border="2" style="width:100%;">';
            results += '<thead><tr  style="background:#5aa4d0; color: white; font-weight: bold;"><th style="text-align: center;font-weight: bold;">Branch Name</th><th style="text-align: center;font-weight: bold;">YesterDay</th><th style="text-align: center;font-weight: bold;">Last Week</th><th style="text-align: center;font-weight: bold;">Last Month</th><th style="text-align: center;font-weight: bold;">Last Year</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr onclick="btnRouteWiseDayComparison(\'' + msg[i].BranchID + '\')">';
                results += '<th  class="2"><div id="txtamount" class="subCategeoryClass">' + msg[i].BranchName + '</th>';
                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterindent + '</div></td>';
                results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekindent + '</div></td>';
                results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthindent + '</div></td>';
                results += '<td  class="2"><div id="Span1" style="" class="QtyClass"><div style="float: right;">' + msg[i].lastyearindent + '</div></td></tr>';
                l = l + 1;
                if (l == 30) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#divComaprison").html(results);
        }
        function btnRouteWiseDayComparison(BranchId) {
            var BranchId;
            var FromDate = document.getElementById('txtFromDate').value;
            var Todate = document.getElementById('txtFromDate').value;
            var PlantName = document.getElementById('ddlSalesOffice').value;
            //            $('#divHide').css('display', 'block');
            var test = "Report";
            var data = { 'operation': 'get_RouteWiseDay_Comparison', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate, 'test': test };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    fill_RouteWise_DayComparison(msg);

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        var routewisearry = []; var salestypearr = [];
        function fill_RouteWise_DayComparison(msg) {
            $('#divMainAddNewRow1').css('display', 'block');
            $('#div_routewisemain').css('display', 'block');
            $('#myModal').css('display', 'block');
            $('#routeprint').css('display', 'block');
            var BranchTable = []; var totyesterindent = 0; var totlastweekindent = 0; var grand_yesterindent = 0; var grand_lastweekindent = 0;
            var totlastmonthindent = 0; var grand_lastmonthindent = 0; var totlastyearindent = 0; var grand_lastyearindent = 0;
            var results = '<div class="divcontainer" style="overflow:auto;"><table border="1" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr  style="background:#5aa4d0; color: white; font-weight: bold;"><th style="text-align: center;font-weight: bold;">Route Name</th><th style="text-align: center;font-weight: bold;">SalesType</th><th style="text-align: center;font-weight: bold;">YesterDay</th><th style="text-align: center;font-weight: bold;">Last Week</th><th style="text-align: center;font-weight: bold;">Last Month</th><th style="text-align: center;font-weight: bold;">Last Year</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].yesterindent == "" && msg[i].lastweekindent == "" && msg[i].lastweekindent == "") {
                }
                else {
                    if (BranchTable.indexOf(msg[i].BranchName) == -1) {
                        if (i == 0) {
                        }
                        else {
                            if (totyesterindent > 0 && totlastweekindent > 0 && totlastmonthindent > 0 && totlastyearindent > 0) {
                                results += '<tr>';
                                results += '<td scope="row" class="1" ></td>';
                                results += '<td scope="row" class="1"  style="font-size:18px;font-weight:bold;color:#006400;">Total</td>';
                                results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totyesterindent).toFixed(2) + '</td>';
                                results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totlastweekindent).toFixed(2) + '</td>';
                                results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totlastmonthindent).toFixed(2) + '</td>';
                                results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totlastyearindent).toFixed(2) + '</td></tr>';
                            }
                        }
                        totyesterindent = 0;
                        totlastweekindent = 0;
                        totlastmonthindent = 0;
                        totlastyearindent = 0;
                        results += '<tr>';
                        results += '<td scope="row" class="1">' + msg[i].BranchName + '</td>';
                        results += '<td onclick="btnAgentWiseDayComparison(\'' + msg[i].salestypeid + '\');"><span style="text-decoration: underline;color:blue;">' + msg[i].SalesType + '</span></td>';
                        results += '<td class="2">' + parseFloat(msg[i].yesterindent).toFixed(2) || 0 + '</td>';
                        results += '<td class="2">' + parseFloat(msg[i].lastweekindent).toFixed(2) || 0 + '</td>';
                        results += '<td class="2">' + parseFloat(msg[i].lastmonthindent).toFixed(2) || 0 + '</td>';
                        results += '<td class="2">' + parseFloat(msg[i].lastyearindent).toFixed(2) || 0 + '</td></tr>';
                        totyesterindent += parseFloat(msg[i].yesterindent) || 0;
                        grand_yesterindent += parseFloat(msg[i].yesterindent) || 0;
                        totlastweekindent += parseFloat(msg[i].lastweekindent) || 0;
                        grand_lastweekindent += parseFloat(msg[i].lastweekindent) || 0;
                        totlastmonthindent += parseFloat(msg[i].lastmonthindent) || 0;
                        grand_lastmonthindent += parseFloat(msg[i].lastmonthindent) || 0;
                        totlastyearindent += parseFloat(msg[i].lastyearindent) || 0;
                        grand_lastyearindent += parseFloat(msg[i].lastyearindent) || 0;
                        BranchTable.push(msg[i].BranchName);
                    }
                    else {
                        results += '<tr>';
                        results += '<td scope="row" class="1" ></td>';
                        results += '<td  onclick="btnAgentWiseDayComparison(\'' + msg[i].salestypeid + '\');"><span style="text-decoration: underline;color:blue;">' + msg[i].SalesType + '</span></td>';
                        results += '<td  class="2">' + parseFloat(msg[i].yesterindent).toFixed(2) || 0 + '</td>';
                        results += '<td  class="2">' + parseFloat(msg[i].lastweekindent).toFixed(2) || 0 + '</td>';
                        results += '<td  class="2">' + parseFloat(msg[i].lastmonthindent).toFixed(2) || 0 + '</td>';
                        results += '<td class="2">' + parseFloat(msg[i].lastyearindent).toFixed(2) || 0 + '</td></tr>'
                        totyesterindent += parseFloat(msg[i].yesterindent) || 0;
                        grand_yesterindent += parseFloat(msg[i].yesterindent) || 0;
                        totlastweekindent += parseFloat(msg[i].lastweekindent) || 0;
                        grand_lastweekindent += parseFloat(msg[i].lastweekindent) || 0;
                        totlastmonthindent += parseFloat(msg[i].lastmonthindent) || 0;
                        grand_lastmonthindent += parseFloat(msg[i].lastmonthindent) || 0;
                        totlastyearindent += parseFloat(msg[i].lastyearindent) || 0;
                        grand_lastyearindent += parseFloat(msg[i].lastyearindent) || 0;
                    }
                }
            }
            results += '<tr>';
            results += '<td scope="row" class="1" ></td>';
            results += '<td scope="row" class="1" style="font-size:18px;font-weight:bold;color:#006400;" >Total</td>';
            results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totyesterindent).toFixed(2) + '</td>';
            results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totlastweekindent).toFixed(2) + '</td>'
            results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totlastmonthindent).toFixed(2) + '</td>';
            results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totlastyearindent).toFixed(2) + '</td></tr>';
            results += '<tr>';
            results += '<td scope="row" class="1" ></td>';
            results += '<td scope="row" class="1" style="font-size:22px;font-weight:bold;color:#B22222;" >Grand Total</td>';
            results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;">' + parseFloat(grand_yesterindent).toFixed(2) + '</td>';
            results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;">' + parseFloat(grand_lastweekindent).toFixed(2) + '</td>';
            results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;">' + parseFloat(grand_lastmonthindent).toFixed(2) + '</td>';
            results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#B22222;">' + parseFloat(grand_lastyearindent).toFixed(2) + '</td></tr>';
            results += '</table></div>';
            $("#div_routewise").html(results);
        }
        function closepopup(msg) {
            $("#myModal").css("display", "none");
           $("#div_routewisemain").css("display", "none");
        }
        function btnAgentWiseDayComparison(BranchId) {
            var BranchId;
            var FromDate = document.getElementById('txtFromDate').value;
            var Todate = document.getElementById('txtFromDate').value;
            var PlantName = document.getElementById('ddlSalesOffice').value;
            //            $('#divHide').css('display', 'block');
            var test = "Report";
            var data = { 'operation': 'Get_AgentWiseDay_Comparison', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate, 'test': test };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    fill_AgentWise_DayComparison(msg);

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function fill_AgentWise_DayComparison(msg) {
            $('#divMainAddNewRow1').css('display', 'block');
            $('#div_routewisemain').css('display', 'block');
            $('#div_agentwisemain').css('display', 'block');
            $('#myModal_agent').css('display', 'block');
            $('#agentprint').css('display', 'block');
            j = 1;
            var tyesterindent = 0; var tlastweekindent = 0; var tlastmonthindent = 0; var tlastyearindent = 0;
            var results = '<div  style="overflow:auto;"><table border="1" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr  style="background:#5aa4d0; color: white; font-weight: bold;"><th style="text-align: center;font-weight: bold;">Agent Name</th><th style="text-align: center;font-weight: bold;">YesterDay</th><th style="text-align: center;font-weight: bold;">Last Week</th><th style="text-align: center;font-weight: bold;">Last Month</th><th style="text-align: center;font-weight: bold;">Last Year</th></tr></thead></tbody>';

            var k = 1;
            var l = 0;
            //var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                //                results += '<td scope="row">' + msg[i].RouteName + '</td>';
                results += '<td scope="row" onclick="btnAgentWiseDayProductComparison(\'' + msg[i].AgentId + '\');">' + msg[i].AgentName + '</td>';
                //                results += '<td scope="row">' + msg[i].SRname + '</td>';
                //                results += '<td scope="row">' + msg[i].salesman + '</td>';
                results += '<td scope="row" class="2" ><div style="float:right;padding-right: 10%;">' + msg[i].yesterindent + '</div></td>';
                results += '<td data-title="brandstatus"  class="3"><div style="float:right;padding-right: 10%;">' + msg[i].lastweekindent + '</div></td>';
                results += '<td data-title="brandstatus"  class="3"><div style="float:right;padding-right: 10%;">' + msg[i].lastmonthindent + '</div></td>';
                results += '<td data-title="brandstatus"  class="3"><div style="float:right;padding-right: 10%;">' + msg[i].lastyearindent + '</div></td></tr>';
                //                l = l + 1;
                //                if (l == 4) {
                //                    l = 0;
                //                }
                tyesterindent += parseFloat(msg[i].yesterindent) || 0;
                tlastweekindent += parseFloat(msg[i].lastweekindent) || 0;
                tlastmonthindent += parseFloat(msg[i].lastmonthindent) || 0;
                tlastyearindent += parseFloat(msg[i].lastyearindent) || 0;
            }
            results += '<tr>';
            results += '<td scope="row" class="1">Total</td>';
            results += '<td scope="row" class="1" style="font-size:18px;font-weight:bold;color:#006400;" ><div style="float:right;padding-right: 7%;">' + parseFloat(tyesterindent).toFixed(2) || 0 + '</div></td>';
            results += '<td   style="font-size:18px;font-weight:bold;color:#006400;"><div style="float:right;padding-right: 7%;">' + parseFloat(tlastweekindent).toFixed(2) || 0 + '</div></td>';
            results += '<td   style="font-size:18px;font-weight:bold;color:#006400;"><div style="float:right;padding-right: 7%;">' + parseFloat(tlastmonthindent).toFixed(2) || 0 + '</div></td>';
            results += '<td   style="font-size:18px;font-weight:bold;color:#006400;"><div style="float:right;padding-right: 7%;">' + parseFloat(tlastyearindent).toFixed(2) || 0 + '</div></td></tr>';
            results += '</table></div>';
            $("#div_agentwise").html(results);
        }
        function close_agentwise(msg) {
            $("#myModal_agent").css("display", "none");
            $('#div_agentwisemain').css('display', 'none');
        }
        function btnAgentWiseDayProductComparison(BranchId) {

            var FromDate = document.getElementById('txtFromDate').value;
            var Todate = document.getElementById('txtFromDate').value;
            //            var BranchId = document.getElementById('ddlAgentName').value;
            var test = "Report";
            var data = { 'operation': 'Get_AgentWiseDayProduct_Comparison', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate, 'test': test };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    fill_AgentWise_DayProductComparison(msg);

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }


        function fill_AgentWise_DayProductComparison(msg) {
            //            $('#divMainAddNewRow1').css('display', 'block');
            //            $('#div_routewisemain').css('display', 'block');
            //            $('#div_agentwisemain').css('display', 'block');
            $('#div_routewisemainCompare').css('display', 'block');
            $('#myModal_product').css('display', 'block');
            $('#agentproduct').css('display', 'block');

            j = 1;
            var tyesterindent = 0; var tlastweekindent = 0; var tlastmonthindent = 0; var tlastyearindent = 0;
            var results = '<div  style="overflow:auto;"><table border="1" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr  style="background:#5aa4d0; color: white; font-weight: bold;"><th style="text-align: center;font-weight: bold;">Product Name</th><th style="text-align: center;font-weight: bold;">YesterDay</th><th style="text-align: center;font-weight: bold;">Last Week</th><th style="text-align: center;font-weight: bold;">Last Month</th><th style="text-align: center;font-weight: bold;">Last Year</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                results += '<td scope="row">' + msg[i].ProductName + '</td>';
                results += '<td scope="row" class="2" onclick="btnAgentWiseBetweenDayProductComparison(\'' + msg[i].ProductId + '_DayWise' + '\');" ><div style="float:right;padding-right: 10%;">' + msg[i].yesterindent + '</div></td>';
                results += '<td data-title="brandstatus"  class="3" onclick="btnAgentWiseBetweenDayProductComparison(\'' + msg[i].ProductId + "_WeakWise" + '\');"><div style="float:right;padding-right: 10%;">' + msg[i].lastweekindent + '</div></td>';
                results += '<td data-title="brandstatus"  class="3" onclick="btnAgentWiseBetweenDayProductComparison(\'' + msg[i].ProductId + "_MonthWise" + '\');"><div style="float:right;padding-right: 10%;">' + msg[i].lastmonthindent + '</div></td>';
                results += '<td data-title="brandstatus"  class="3" onclick="btnAgentWiseBetweenDayProductComparison(\'' + msg[i].ProductId + "_YearWise" + '\');"><div style="float:right;padding-right: 10%;">' + msg[i].lastyearindent + '</div></td></tr>';
                tyesterindent += parseFloat(msg[i].yesterindent) || 0;
                tlastweekindent += parseFloat(msg[i].lastweekindent) || 0;
                tlastmonthindent += parseFloat(msg[i].lastmonthindent) || 0;
                tlastyearindent += parseFloat(msg[i].lastyearindent) || 0;
            }
            results += '<tr>';
            results += '<td scope="row" class="1">Total</td>';
            results += '<td scope="row" class="1" style="font-size:18px;font-weight:bold;color:#006400;" ><div style="float:right;padding-right: 7%;">' + parseFloat(tyesterindent).toFixed(2) || 0 + '</div></td>';
            results += '<td   style="font-size:18px;font-weight:bold;color:#006400;"><div style="float:right;padding-right: 7%;">' + parseFloat(tlastweekindent).toFixed(2) || 0 + '</div></td>';
            results += '<td   style="font-size:18px;font-weight:bold;color:#006400;"><div style="float:right;padding-right: 7%;">' + parseFloat(tlastmonthindent).toFixed(2) || 0 + '</div></td>';
            results += '<td   style="font-size:18px;font-weight:bold;color:#006400;"><div style="float:right;padding-right: 7%;">' + parseFloat(tlastyearindent).toFixed(2) || 0 + '</div></td></tr>';
            results += '</table></div>';
            $("#div_routewiseCompare").html(results);
        }
        function closepopup_product(msg) {
            $("#myModal_product").css("display", "none");
            $('#div_routewisemainCompare').css('display', 'none');
        }
        function btnAgentWiseBetweenDayProductComparison(BranchId) {
            var BranchId;
            var FromDate = document.getElementById('txtFromDate').value;
            var Todate = document.getElementById('txtFromDate').value;
//            var PlantName = document.getElementById('ddlPlant').value;
            var test = "Report";
            var data = { 'operation': 'Get_AgentWiseBetweenDayProduct_Comparison', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate, 'test': test };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    FillAgentProductDayComparisonDetails(msg);

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }


        var TotalDate = []; var attendancearry = []; var totattendance = []; var emptytable4 = [];
        function FillAgentProductDayComparisonDetails(msg) {
            $('#example').css('display', 'block');
            $('#mymodel_daywise').css('display', 'block');
            $('#AgentDayProduct').css('display', 'block');
            var result = [];
            emptytable4 = [];

            TotalDate = msg[0].daywisedatescls;
            totattendance = msg[0].daywiseproductcls;
            var results = '<div class="divcontainer" style="overflow:auto;"><table border="1" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr>';
            results += '<th scope="col" style="text-align:center;"><i class="fa fa-user" aria-hidden="true"></i> ProductName</th>';
            for (var i = 0; i < TotalDate.length; i++) {
                results += '<th scope="col" id="txtDate"><i class="fa fa-calendar" aria-hidden="true"></i> ' + TotalDate[i].DeliveryDates + '</th>';
            }
            results += '</tr></thead></tbody>';
            for (var i = 0; i < totattendance.length; i++) {
                results += '<tr>';
                var Employeename = totattendance[i].ProductName
                if (emptytable4.indexOf(Employeename) == -1) {
                    results += '<td data-title="brandstatus" class="4">' + totattendance[i].ProductName + '</td>';
                    emptytable4.push(Employeename);
                    for (var j = 0; j < TotalDate.length; j++) {
                        for (var k = 0; k < totattendance.length; k++) {
                            if (TotalDate[j].DeliveryDates == totattendance[k].Date && Employeename == totattendance[k].ProductName) {
                                //                                        var st = totattendance[k].Empid + '-' + totattendance[k].LogDate;
                                results += '<td  data-title="brandstatus" class="2">' + totattendance[k].DeliveryQty + '</td>';
                            }
                        }
                    }
                    results += '</tr>';
                }
            }
            results += '</table></div>';
            $("#ProductWiseChart").html(results);
        }
        function closepopup_daywise() {
            $('#example').css('display', 'none');
            $("#mymodel_daywise").css("display", "none");
        }
//        function close_routewise() {
//            $('#div_routewisemain').css('display', 'none');
//        }
//        function close_routewiseCompare() {
//            $('#div_routewisemainCompare').css('display', 'none');
//        }
//        function close_AgentProductLineChart() {
//            $('#example').css('display', 'none');
//        }
//        function close_agentwise() {
//            $('#div_agentwisemain').css('display', 'none');
//        }
//        function ddltypeChange() {
//            var Type = document.getElementById('ddltype').value;
//            if (Type == "BranchWise") {
//                $('#lblroutetid').css('display', 'none');
//                $('#trrouteid').css('display', 'none');
//                $('#lblagentid').css('display', 'none');
//                $('#tragentid').css('display', 'none');
//                $('#div_PBranch').css('display', 'block');

//            }
//            if (Type == "RouteWise") {
//                $('#lblroutetid').css('display', 'block');
//                $('#trrouteid').css('display', 'block');
//                $('#lblagentid').css('display', 'none');
//                $('#tragentid').css('display', 'none');
//                $('#div_PBranch').css('display', 'block');

//            }
//            if (Type == "AgentWise") {
//                $('#lblroutetid').css('display', 'block');
//                $('#trrouteid').css('display', 'block');
//                $('#lblagentid').css('display', 'block');
//                $('#tragentid').css('display', 'block');
//                $('#div_PBranch').css('display', 'block');
//            }
//        }
        function CallPrint(strid) {
            document.getElementById("divComaprison").style.borderCollapse = "collapse";
//            document.getElementById("tbl_grdReports2").style.borderCollapse = "collapse";
//            document.getElementById("tbl_grdReports3").style.borderCollapse = "collapse";
//            document.getElementById("tbl_grdReports4").style.borderCollapse = "collapse";
//            document.getElementById("tbl_grdReports5").style.borderCollapse = "collapse";
//            document.getElementById("tbl_grdReports6").style.borderCollapse = "collapse";
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        function CallRoutePrint(strid) {
            document.getElementById("div_routewise").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports2").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports3").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports4").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports5").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports6").style.borderCollapse = "collapse";
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        function CallAgentPrint(strid) {
            document.getElementById("div_agentwise").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports2").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports3").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports4").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports5").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports6").style.borderCollapse = "collapse";
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        function CallAgentProductPrint(strid) {
            document.getElementById("div_routewiseCompare").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports2").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports3").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports4").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports5").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports6").style.borderCollapse = "collapse";
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        function CallAgentDayProductPrint(strid) {
            document.getElementById("ProductWiseChart").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports2").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports3").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports4").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports5").style.borderCollapse = "collapse";
            //            document.getElementById("tbl_grdReports6").style.borderCollapse = "collapse";
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
            Branch Wise Sale Comparison<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Branch Wise Sale Comparison</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Branch Wise Sale Comparison Details
                </h3>
            </div>
            <div class="box-body">
                <%--<div id="div_PBranch">
                    <table>
                      <tr>
                            <td>
                               <label>Select Type</label> 
                            </td>
                            <td>
                                <select id="ddltype" class="form-control" onchange="ddltypeChange();">
                                    <option value="">Select Type</option>
                                    <option value="BranchWise">BranchWise</option>
                                    <option value="RouteWise">RouteWise</option>
                                    <option value="AgentWise">AgentWise</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                        <td style="width: 5px;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                               <label>Sales Office:</label> 
                            </td>
                            <td>
                                <select id="ddlSalesOffice"  class="form-control" onchange="ddlSalesOffice_SelectedIndexChanged();">
                                </select>
                            </td>
                        </tr>
                    </table>
                </div>--%>
                <br />
                 <br />
                  <br />
                <div  id="d">
                    <table>
                        <%--<tr >
                            <td id="lblroutetid" style="display:none">
                                <label id="lblrouten" >Route Name:</label>
                            </td>
                            <td id="trrouteid" style="display:none">
                                <select id="ddlRouteName" class="form-control" onchange="ddlRouteNameChange();">
                                </select>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td id="lblagentid" style="display:none">
                                <label id="lblAgent" >AgentName:</label>
                            </td>
                            <td id="tragentid" style="display:none">
                                <select id="ddlAgentName" class="form-control">
                                </select>
                            </td>
                            <td style="width: 5px;">
                            </td>--%>
                            <tr>
                           
                            <td>
                               <label>Sales Office:</label> 
                            </td>
                            <td>
                                <select id="ddlSalesOffice"  class="form-control" onchange="ddlSalesOffice_SelectedIndexChanged();">
                                </select>
                            </td>
                       
                            <td>
                                <label id="lbltxtdt" >Date:</label>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <input id="txtFromDate" type="date" class="form-control"></input> 
                            </td>
                            <td style="width: 5px;">
                            </td>
                             <%--<td>
                                <input id="txtToDate" type="date" class="form-control"></input> 
                            </td>
                            <td style="width: 5px;">
                            </td>--%>
                            <td>
                                <input id="btnGenerate" type="button" value="Generate"
                                    class="btn btn-primary" onclick="BranchIndentDayWiseComparison();" />
                            </td>
                        </tr>
                    </table>
                     <br />
                  <br />
                       <div  id="firstdiv">
           <div id="divPrint">
            <div class="col-sm-12" style="width:100%;">
                    <div class="box box-solid box-info">
                        <div class="box-header with-border">
                            <h3 class="ion ion-clipboard">
                                <i style="padding-right: 5px;"></i>Day Wise Sale Comparison
                            </h3>
                            <div class="box-tools pull-left">
                                <button class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="box-body no-padding">
                                <div>
                                    <div id="divComaprison">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
             </div>
             </div>
        <div id="divHide" style="width: 120%; display: none;">
         <div class="modal fade in" id="divMainAddNewRow1" style="display: none; padding-right: 17px;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true" onclick="CloseClick1();">×</span></button>
                        <h4 class="modal-title">
                            Branch Wise Details</h4>
                    </div>
                    <div class="modal-body" id="divChart" style="height: 400px; overflow-y: scroll;">
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" onclick="CloseClick1();">
                            Close</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        </div>
        <div class="modal fade in" id="div_routewisemain" style="display: none; padding-right: 17px;width: 110%;">
            <div class="modal" id="myModal" role="dialog" style="overflow:auto;">
                <div class="modal-dialog" style="width: 71% !important;">
                  <!-- Modal content-->
                  <div class="modal-content">
                    <div class="modal-header" >
                    </div>
                    <div class="modal-body">
                       <div id="routeprint" style="display:none;">
                                <div>
                                    <div align="center"  style="font-family: Arial; font-size: 18pt; font-weight: bold; color: Black;">
                                        <span>Sri Vyshnavi Dairy Specialities (P) Ltd </span>
                                        <br />
                                    </div>
                                </div>
                                <div align="center" style="border-bottom: 1px solid gray; border-top: 1px solid gray;">
                                    <span style="font-size: 26px; font-weight: bold;"><u>Route Wise Details </u></span>
                                </div>
                             <div id="div_routewise"></div> 
                    </div>
                    <div class="modal-footer">
                     <input id="Button3" type="button" class="btn btn-primary" name="submit" value='Print'
                                onclick="javascript:CallPrint('routeprint');" />
                      <button type="button" class="btn btn-default" id="close" onclick="closepopup();">Close</button>
                    </div>
                  </div>
                </div>
              </div>
              </div>
            </div>
             <div class="modal fade in" id="div_agentwisemain"  style="display: none; padding-right: 17px;width: 110%;">
            <div class="modal" id="myModal_agent" role="dialog" style="overflow:auto;">
                <div class="modal-dialog" style="width: 71% !important;">
                  <!-- Modal content-->
                  <div class="modal-content">
                    <div class="modal-header" >
                    </div>
                    <div class="modal-body">
                       <div id="agentprint" style="display:none;">
                                <div>
                                    <div align="center"  style="font-family: Arial; font-size: 18pt; font-weight: bold; color: Black;">
                                        <span>Sri Vyshnavi Dairy Specialities (P) Ltd </span>
                                        <br />
                                    </div>
                                </div>
                                <div align="center" style="border-bottom: 1px solid gray; border-top: 1px solid gray;">
                                    <span style="font-size: 26px; font-weight: bold;"><u>Agent Wise Details </u></span>
                                </div>
                             <div id="div_agentwise"></div> 
                    </div>
                    <div class="modal-footer">
                     <input id="Button1" type="button" class="btn btn-primary" name="submit" value='Print'
                                onclick="javascript:CallPrint('agentprint');" />
                      <button type="button" class="btn btn-default" id="btnAgentPrint" onclick="close_agentwise();">Close</button>
                    </div>
                  </div>
                </div>
              </div>
              </div>
            </div>
               

               <div class="modal fade in" id="div_routewisemainCompare" style="display: none; padding-right: 17px;width: 110%;">
            <div class="modal" id="myModal_product" role="dialog" style="overflow:auto;">
                <div class="modal-dialog" style="width: 71% !important;">
                  <!-- Modal content-->
                  <div class="modal-content">
                    <div class="modal-header" >
                    </div>
                    <div class="modal-body">
                       <div id="agentproduct" style="display:none;">
                                <div>
                                    <div align="center"  style="font-family: Arial; font-size: 18pt; font-weight: bold; color: Black;">
                                        <span>Sri Vyshnavi Dairy Specialities (P) Ltd </span>
                                        <br />
                                    </div>
                                </div>
                                <div align="center" style="border-bottom: 1px solid gray; border-top: 1px solid gray;">
                                    <span style="font-size: 26px; font-weight: bold;"><u>Product Wise Details </u></span>
                                </div>
                             <div id="div_routewiseCompare"></div> 
                    </div>
                    <div class="modal-footer">
                     <input id="btnAgentProduct" type="button" class="btn btn-primary" name="submit" value='Print'
                                onclick="javascript:CallPrint('agentproduct');" />
                      <button type="button" class="btn btn-default" id="Button4" onclick="closepopup_product();">Close</button>
                    </div>
                  </div>
                </div>
              </div>
              </div>
            </div>

            <div class="modal fade in" id="example" style="display: none; padding-right: 17px;width: 110%;">
            <div class="modal" id="mymodel_daywise" role="dialog" style="overflow:auto;">
                <div class="modal-dialog" style="width: 71% !important;">
                  <!-- Modal content-->
                  <div class="modal-content">
                    <div class="modal-header" >
                    </div>
                    <div class="modal-body">
                       <div id="AgentDayProduct" style="display:none;">
                                <div>
                                    <div align="center"  style="font-family: Arial; font-size: 18pt; font-weight: bold; color: Black;">
                                        <span>Sri Vyshnavi Dairy Specialities (P) Ltd </span>
                                        <br />
                                    </div>
                                </div>
                                <div align="center" style="border-bottom: 1px solid gray; border-top: 1px solid gray;">
                                    <span style="font-size: 26px; font-weight: bold;"><u>Product Day Wise Details </u></span>
                                </div>
                             <div id="ProductWiseChart"></div> 
                    </div>
                    <div class="modal-footer">
                     <input id="btnAgentDayProduct" type="button" class="btn btn-primary" name="submit" value='Print'
                                onclick="javascript:CallPrint('AgentDayProduct');" />
                      <button type="button" class="btn btn-default" id="Button5" onclick="closepopup_daywise();">Close</button>
                    </div>
                  </div>
                </div>
              </div>
              </div>
            </div>
                <%--<div class="modal fade in" id="" style="display: none; padding-right: 17px;width: 110%;">
                <div id="AgentDayProduct">
                    <div class="modal-dialog1">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true" onclick="close_AgentProductLineChart();">×</span></button>
                                <h4 class="modal-title">
                                    Day Wise Product Details</h4>
                            </div>
                            <div class="modal-body" id="" style="height: 400px; overflow-y: scroll;">
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-danger" onclick="close_AgentProductLineChart();">
                                    Close</button>
                            </div>
                            
                        </div>
                        <!-- /.modal-content -->
                    </div>
                    <!-- /.modal-dialog -->
                     <input id="" class="btn btn-primary" type="button" value="Print" onclick="javascript:CallAgentDayProductPrint('');"/>
                </div>
                </div>--%>
                        <div>
                        <input id="btnPrint" class="btn btn-primary" type="button" value="Print" onclick="javascript:CallPrint('divPrint');"/>
                </div>
        </div>
      </div>
      </div>
      </div>
  </section>
</asp:Content>
