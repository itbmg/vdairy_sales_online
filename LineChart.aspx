<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="LineChart.aspx.cs" Inherits="Default2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="Kendo/jquery.min.js" type="text/javascript"></script>
    <script src="Kendo/kendo.all.min.js" type="text/javascript"></script>
    <link href="Kendo/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="Kendo/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Agent,Route,Sales Office Sales Vs Avg Sales<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Chart Reports</a></li>
            <li><a href="#">Line Chart</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Line Chart Details
                </h3>
            </div>
            <div class="box-body">
                <div style="width: 100%;" align="center">
                    <table>
                        <tr>
                            <td>
                                <label>
                                    Chart Type</label>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td style="height: 40px;">
                                <select id="ddlType" class="form-control" onchange="ddlTypeChange();">
                                    <option>Sales Office Wise</option>
                                    <option>Route Wise</option>
                                    <option>Agent Wise</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Type</label>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td style="height: 40px;">
                                <select id="ddldatatype" class="form-control">
                                    <option>Day Wise</option>
                                    <option>Monthly Wise</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Sales Office</label>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td style="height: 40px;">
                                <select id="ddlSalesOffice" class="form-control" onchange="ddlSalesOfficeChange(this);">
                                </select>
                            </td>
                        </tr>
                        <tr id="divroute">
                            <td>
                                <span>Route Name</span>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td style="height: 40px;">
                                <select id="ddlRouteName" class="form-control" onchange="ddlRouteNameChange(this);">
                                </select>
                            </td>
                        </tr>
                        <tr id="divagent">
                            <td>
                                <span>Agent Name</span>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td style="height: 40px;">
                                <select id="ddlAgentName" class="form-control">
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 90px; height: 50px;">
                                <span>From Date</span>
                                <input type="date" id="txtFromdate" class="form-control" />
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td style="width: 90px; height: 50px;">
                                <span>To Date</span>
                                <input type="date" id="txtTodate" class="form-control" />
                            </td>
                            <td style="width: 30%;">
                            
                            </td>
                              <td>
                              <a target='_blank' href="Classification_linechart.aspx">get Classification Type</a>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 90px; height: 50px;">
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <input type="button" id="submit" value="Submit" class="btn btn-success" onclick="DetailsValidating()" />
                            </td>
                        </tr>
                    </table>
                    <span id="lblmsg" class="lblmsg"></span>
                </div>
                <br />
                <%--style="opacity: .9;filter: alpha(opacity=20);background: center no-repeat url('Images/VLogo - Copy.png');"--%>
                <div id="divPrint">
                    <div id="example" class="k-content absConf">
                        <div class="chart-wrapper" style="margin: auto;">
                            <div id="chart">
                            </div>
                        </div>
                        <script type="text/javascript">
                            $(function () {
                                FillSalesOffice();
                                ddlTypeChange();
                                GetSalesTypes();
                            });
                        </script>
                        <script type="text/javascript">
                            function CallPrint(strid) {
                                //            var prtContent = document.getElementById(strid);
                                var divToPrint = document.getElementById(strid);
                                var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
                                newWin.document.open();
                                newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
                                newWin.document.close();
                            }
                            var salesTypes = [];
                            function GetSalesTypes() {
                                var data = { 'operation': 'updatesalestypemanage' };
                                var s = function (msg) {
                                    if (msg) {
                                        salesTypes = msg;
                                    }
                                    else {
                                    }
                                };
                                var e = function (x, h, e) {
                                };
                                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                                callHandler(data, s, e);
                            }
                            function GetClassificationtype() {
                                window.open = "Classification_linechart.aspx";
                            }
                            function ddlTypeChange() {
                                var ddlType = document.getElementById('ddlType').value;
                                if (ddlType == "Agent Wise") {
                                    $('#divroute').css('display', 'table-row');
                                    $('#divagent').css('display', 'table-row');
                                }
                                if (ddlType == "Route Wise") {
                                    $('#divroute').css('display', 'table-row');
                                    $('#divagent').css('display', 'none');
                                }
                                if (ddlType == "Sales Office Wise") {
                                    $('#divroute').css('display', 'none');
                                    $('#divagent').css('display', 'none');
                                }
                            }
                            function DetailsValidating() {
                                $("#example").empty();
                                $("#chart").empty();
                                var txtFromdate = document.getElementById('txtFromdate').value;
                                var txtTodate = document.getElementById('txtTodate').value;
                                if (txtFromdate == "") {
                                    alert("Select start date");
                                    return false;
                                }
                                if (txtTodate == "") {
                                    alert("Select end date");
                                    return false;
                                }
                                var ddlAgentName = document.getElementById('ddlAgentName').value;
                                var ddlRouteName = document.getElementById('ddlRouteName').value;
                                var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
                                var ddlType = document.getElementById('ddlType').value;
                                if (ddlType == "Agent Wise") {
                                    if (ddlSalesOffice == "" || ddlSalesOffice == "Select Sales Office") {
                                        alert("Select Sales Offic name");
                                        return false;
                                    }
                                    if (ddlRouteName == "" || ddlRouteName == "Select Route Name") {
                                        alert("Select route name");
                                        return false;
                                    }
                                    if (ddlAgentName == "" || ddlAgentName == "Select Agent Name") {
                                        alert("Select agent name");
                                        return false;
                                    }
                                }
                                if (ddlType == "Route Wise") {
                                    if (ddlSalesOffice == "" || ddlSalesOffice == "Select Sales Office") {
                                        alert("Select Sales Offic name");
                                        return false;
                                    }
                                    if (ddlRouteName == "" || ddlRouteName == "Select Route Name") {
                                        alert("Select route name");
                                        return false;
                                    }
                                }
                                if (ddlType == "Sales Office Wise") {
                                    if (ddlSalesOffice == "" || ddlSalesOffice == "Select Sales Office") {
                                        alert("Select Sales Offic name");
                                        return false;
                                    }
                                }
                                var ddldatatype = document.getElementById('ddldatatype').value;
                                var data = { 'operation': 'GetLineChartValues', 'startDate': txtFromdate, 'endDate': txtTodate, 'AgentName': ddlAgentName, 'RouteName': ddlRouteName, 'SalesOffice': ddlSalesOffice, 'Type': ddlType, 'ddldatatype': ddldatatype };
                                var s = function (msg) {
                                    if (msg) {
                                        if (msg == "Session Expired") {
                                            alert(msg);
                                            window.location.assign("Login.aspx");
                                        }
                                        if (msg == "Please Select Monthly Wise") {
                                            alert(msg);
                                            return false;
                                        }
                                        else {
                                            createChart(msg);
                                            getPiechart();
                                            getduechartreport();
                                            getinventorychartreport();
                                            $('#divremarks').css('display', 'block');
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
                            function getPiechart() {
                                var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
                                var txtFromdate = document.getElementById('txtFromdate').value;
                                var txtTodate = document.getElementById('txtTodate').value;
                                if (txtFromdate == "") {
                                    alert("Select start date");
                                    return false;
                                }
                                var ddlType = document.getElementById('ddlType').value;
                                var data = { 'operation': 'GetPieChart_ClassificationType', 'startDate': txtFromdate, 'enddate': txtTodate, 'ddlSalesOffice': ddlSalesOffice, 'ddlType': ddlType };
                                var s = function (msg) {
                                    if (msg) {
                                        if (msg == "Session Expired") {
                                            alert(msg);
                                            window.location.assign("Login.aspx");
                                        }
                                        else {
                                            if (msg.length > 0) {
                                                createpieChart(msg);
                                            }   
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
                            
                            function GetTotalCal() {
                                document.getElementById('Kgsclass').innerHTML = "";
                                document.getElementById('ltrclass').innerHTML = "";
                                var totalltr = 0;
                                $('.clsqtyltr').each(function (i, obj) {
                                    var qtyclass = $(this).text();
                                    if (qtyclass == "" || qtyclass == "0") {
                                    }
                                    else {
                                        totalltr += parseFloat(qtyclass);
                                    }

                                });
                                var totalkgs = 0;
                                $('.clsqtykgs').each(function (i, obj) {
                                    var qtyclass = $(this).text();
                                    if (qtyclass == "" || qtyclass == "0") {
                                    }
                                    else {
                                        totalkgs += parseFloat(qtyclass);
                                    }

                                });
                                document.getElementById('Kgsclass').innerHTML = parseFloat(totalkgs).toFixed(2);
                                document.getElementById('ltrclass').innerHTML = parseFloat(totalltr).toFixed(2);
                            }
                            var newXarray = [];
                            function createpieChart(databind) {
                                $("#chart").empty();
                                newXarray = [];
                                var textname = "";
                                var ddlType = document.getElementById('ddlType').value;
                                var totalqty = databind[0].totalqty;
                                if (ddlType == "Sales Office Wise") {
                                    var agent = document.getElementById("ddlSalesOffice");
                                    var agentname = agent.options[agent.selectedIndex].text;
                                    textname = agentname + " - " + " Classification Agents " + " - " + "Pie Chart" ;
                                }
                                //                                if (ddlType == "Plant Wise Products") {
                                //                                    textname = " Plant Wise Products";
                                //                                }
                                //                                if (ddlType == "Sales Office Wise Products") {
                                //                                    var agent = document.getElementById("ddlSalesOffice");
                                //                                    var agentname = agent.options[agent.selectedIndex].text;
                                //                                    textname = agentname + " Products ";
                                //                                }
                                var myTableDiv_1 = document.getElementById("example");
                                var divleakbar_1 = document.createElement("div");
                                myTableDiv_1.appendChild(divleakbar_1);
                                var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                                results += '<thead><tr><th scope="col">Type</th><th scope="col">Qty</th><th scope="col">Avg Qty</th><th scope="col">Percentage</th></tr></thead></tbody>';
                                var Amount = databind[0].Amount;
                                var DeliveryQty = databind[0].DeliveryQty;
                                var AverageyQty = databind[0].AverageyQty;
                                var RouteName = databind[0].RouteName;
                                for (var i = 0; i < RouteName.length; i++) {
                                    results += '<tr>';
                                    results += '<td scope="row" class="1" >' + RouteName[i] + '</td>';
                                    results += '<td data-title="Capacity" class="2">' + DeliveryQty[i] + '</td>';
                                    results += '<td data-title="Capacity" class="clsqtyltr">' + AverageyQty[i] + '</td>';
                                    results += '<td data-title="Capacity" class="clsqtykgs">' + Amount[i] + '</td></tr>';
                                }
                                results += '<tr>';
                                results += '<td scope="row" class="1" ></td>';
                                results += '<td scope="row" class="1" >Total</td>';
                                results += '<td scope="row" class="1" ><span id="ltrclass"></span></span></td>';
                                results += '<td data-title="Capacity" class="4"><span id="Kgsclass"></span></span></th><td data-title="IsTransport" ></td></tr>';
                                results += '</table></div>';
                                $(divleakbar_1).html(results);
                                GetTotalCal();
                                var myTableDiv = document.getElementById("example");
                                var divleakbar = document.createElement("div");
                                divleakbar.style.height = "500px";
                                myTableDiv.appendChild(divleakbar);
                                for (var i = 0; i < RouteName.length; i++) {
                                    newXarray.push({ "category": RouteName[i], "value": parseFloat(Amount[i]) });
                                }
                                $(divleakbar).kendoChart({
                                    title: {
                                        text: textname,
                                        color: "#A52A2A",
                                        font: "bold italic 18px Arial,Helvetica,sans-serif"
                                    },
                                    legend: {
                                        visible: false
                                    },
                                    chartArea: {
                                        background: ""
                                    },
                                    seriesDefaults: {
                                        labels: {
                                            visible: true,
                                            background: "transparent",
                                            template: "#= category #: #= value#"
                                        }
                                    },
                                    dataSource: {
                                        data: newXarray
                                    },
                                    series: [{
                                        type: "pie",
                                        field: "value",
                                        categoryField: "category"
                                    }],
                                    seriesColors: ["#A52A2A", "#00FF00", "#1C1715", "#0041C2", "#FFA500", "#A52A2A", "#FF7F50", "#00FF00", "#808000", "#0041C2", "#800517", "#1C1715"],
                                    tooltip: {
                                        visible: true,
                                        format: "{0}%"
                                    }
                                });
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
                            //                            function ddlAgentNameChange(id) {
                            //                                BtnGetAmountDeatailsClick();
                            //                            }
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
                            var datainXSeries = 0;
                            var datainYSeries = 0;
                            var newXarray = [];
                            var newYarray = [];
                            function createChart(databind) {
                                var myTableDiv = document.getElementById("example");
                                var divleakbar = document.createElement("div");
                                var ddldatatype = document.getElementById('ddldatatype').value;
                                if (ddldatatype == "Monthly Wise") {
                                    divleakbar.style.height = "500px";
                                }
                                else {
                                    divleakbar.style.height = "900px";
                                }
                                myTableDiv.appendChild(divleakbar);
                                var ddlType = document.getElementById('ddlType').value;
                                var textname = "";
                                var aqty = databind[0].DeliveryQty;
                                var splitqty = aqty[1].split(',');
                                var avgqty = splitqty[0];
                                if (ddlType == "Agent Wise") {
                                    var agent = document.getElementById("ddlAgentName");
                                    var agentname = agent.options[agent.selectedIndex].text;
                                    textname = agentname + " Agent wise sales vs avg sales --> " + avgqty;
                                }
                                if (ddlType == "Route Wise") {
                                    var agent = document.getElementById("ddlRouteName");
                                    var agentname = agent.options[agent.selectedIndex].text;
                                    textname = agentname + " Route wise sales vs avg sales --> " + avgqty;
                                }
                                if (ddlType == "Sales Office Wise") {
                                    var agent = document.getElementById("ddlSalesOffice");
                                    var agentname = agent.options[agent.selectedIndex].text;
                                    textname = agentname + " Sales office wise sales vs avg sales --> " + avgqty;
                                }
                                datainXSeries = 0;
                                datainYSeries = 0;
                                newXarray = [];
                                newYarray = [];
                                //                                $("#chart").empty();
                                //                                document.getElementById('chart').innerHTML = "";
                                for (var k = 0; k < databind.length; k++) {
                                    var BranchName = [];
                                    var IndentDate = databind[k].IndentDate;
                                    var UnitQty = databind[k].UnitQty;
                                    var DeliveryQty = databind[k].DeliveryQty;
                                    var Status = databind[k].Status;
                                    newXarray = IndentDate.split(',');
                                    for (var i = 0; i < DeliveryQty.length; i++) {
                                        newYarray.push({ 'data': DeliveryQty[i].split(','), 'name': Status[i] });
                                    }
                                }
                                $(divleakbar).kendoChart({
                                    title: {
                                        text: textname,
                                        color: "#006600",
                                        font: "bold italic 18px Arial,Helvetica,sans-serif"
                                    },
                                    legend: {
                                        position: "bottom",
                                        visible: false
                                    },
                                    chartArea: {
                                        background: ""
                                    },
                                    seriesDefaults: {
                                        type: "line",
                                        style: "smooth"
                                    },
                                    series: newYarray,
                                    valueAxis: {
                                        labels: {
                                            format: "{0}"
                                        },
                                        line: {
                                            visible: false
                                        },
                                        axisCrossingValue: -10
                                    },
                                    categoryAxis: {
                                        categories: newXarray,
                                        majorGridLines: {
                                            visible: false
                                        },
                                        labels: {
                                            rotation: 65
                                        }
                                    },
                                    seriesColors: ["#a0a700", "#A52A2A", "#FF7F50", "#00FF00", "#808000", "#0041C2", "#800517", "#1C1715"],
                                    tooltip: {
                                        visible: true,
                                        format: "{0}%",
                                        template: "#= series.name #: #= value #"
                                    }
                                });
                            }
                            function getinventorychartreport() {
                                var txtFromdate = document.getElementById('txtFromdate').value;
                                var txtTodate = document.getElementById('txtTodate').value;
                                if (txtFromdate == "") {
                                    alert("Select start date");
                                    return false;
                                }
                                if (txtTodate == "") {
                                    alert("Select end date");
                                    return false;
                                }
                                var ddlAgentName = document.getElementById('ddlAgentName').value;
                                var ddlRouteName = document.getElementById('ddlRouteName').value;
                                var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
                                var ddlType = document.getElementById('ddlType').value;
                                if (ddlType == "Agent Wise") {
                                    if (ddlSalesOffice == "" || ddlSalesOffice == "Select Sales Office") {
                                        alert("Select Sales Offic name");
                                        return false;
                                    }
                                    if (ddlRouteName == "" || ddlRouteName == "Select Route Name") {
                                        alert("Select route name");
                                        return false;
                                    }
                                    if (ddlAgentName == "" || ddlAgentName == "Select Agent Name") {
                                        alert("Select agent name");
                                        return false;
                                    }
                                }
                                if (ddlType == "Route Wise") {
                                    if (ddlSalesOffice == "" || ddlSalesOffice == "Select Sales Office") {
                                        alert("Select Sales Offic name");
                                        return false;
                                    }
                                    if (ddlRouteName == "" || ddlRouteName == "Select Route Name") {
                                        alert("Select route name");
                                        return false;
                                    }
                                }
                                if (ddlType == "Sales Office Wise") {
                                    if (ddlSalesOffice == "" || ddlSalesOffice == "Select Sales Office") {
                                        alert("Select Sales Offic name");
                                        return false;
                                    }
                                }
                                var ddldatatype = document.getElementById('ddldatatype').value;

                                var data = { 'operation': 'GetLineChart_agentinventorytransactions', 'startDate': txtFromdate, 'endDate': txtTodate, 'AgentName': ddlAgentName, 'RouteName': ddlRouteName, 'SalesOffice': ddlSalesOffice, 'Type': ddlType, 'ddldatatype': ddldatatype };
                                var s = function (msg) {
                                    if (msg) {
                                        if (msg == "Session Expired") {
                                            alert(msg);
                                            window.location.assign("Login.aspx");
                                        }
                                        else {
                                            if (msg.length > 0) {
                                                inventorychartreport(msg);

                                            }
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
                            function inventorychartreport(databind) {
                                var myTableDiv = document.getElementById("example");
                                var divleakbar = document.createElement("div");
                                myTableDiv.appendChild(divleakbar);
                                var ddlType = document.getElementById('ddlType').value;
                                var textname = "";
                                var aqty = databind[0].DeliveryQty;
                                var splitqty = aqty[1].split(',');
                                var avgqty = splitqty[0];
                                if (ddlType == "Agent Wise") {
                                    var agent = document.getElementById("ddlAgentName");
                                    var agentname = agent.options[agent.selectedIndex].text;
                                    textname = agentname + " Agent wise Inventory  ";
                                }
                                if (ddlType == "Route Wise") {
                                    var agent = document.getElementById("ddlRouteName");
                                    var agentname = agent.options[agent.selectedIndex].text;
                                    textname = agentname + " Route wise Inventory ";
                                }
                                if (ddlType == "Sales Office Wise") {
                                    var agent = document.getElementById("ddlSalesOffice");
                                    var agentname = agent.options[agent.selectedIndex].text;
                                    textname = agentname + " Sales office wise Inventory  ";
                                }
                                datainXSeries = 0;
                                datainYSeries = 0;
                                newXarray = [];
                                newYarray = [];
                                //                                $("#chart").empty();
                                //                                document.getElementById('chart').innerHTML = "";
                                for (var k = 0; k < databind.length; k++) {
                                    var BranchName = [];
                                    var IndentDate = databind[k].IndentDate;
                                    var UnitQty = databind[k].UnitQty;
                                    var DeliveryQty = databind[k].DeliveryQty;
                                    var Status = databind[k].Status;
                                    newXarray = IndentDate.split(',');
                                    for (var i = 0; i < DeliveryQty.length; i++) {
                                        newYarray.push({ 'data': DeliveryQty[i].split(','), 'name': Status[i] });
                                    }
                                }
                                $(divleakbar).kendoChart({
                                    title: {
                                        text: textname,
                                        color: "#FF0000",
                                        font: "bold italic 18px Arial,Helvetica,sans-serif"
                                    },
                                    legend: {
                                        position: "bottom"
                                    },
                                    chartArea: {
                                        background: ""
                                    },
                                    seriesDefaults: {
                                        type: "line",
                                        style: "smooth"
                                    },
                                    series: newYarray,
                                    valueAxis: {
                                        labels: {
                                            format: "{0}"
                                        },
                                        line: {
                                            visible: false
                                        },
                                        axisCrossingValue: -10
                                    },
                                    categoryAxis: {
                                        categories: newXarray,
                                        majorGridLines: {
                                            visible: false
                                        },
                                        labels: {
                                            rotation: 65
                                        }
                                    },
                                    seriesColors: ["#00FF00", "#FFA500", "#800080", "#FF0000"],
                                    tooltip: {
                                        visible: true,
                                        format: "{0}%",
                                        template: "#= series.name #: #= value #"
                                    }
                                });
                            }
                            var salestype = "";
                            function getduechartreport() {
                                var txtFromdate = document.getElementById('txtFromdate').value;
                                var txtTodate = document.getElementById('txtTodate').value;
                                if (txtFromdate == "") {
                                    alert("Select start date");
                                    return false;
                                }
                                if (txtTodate == "") {
                                    alert("Select end date");
                                    return false;
                                }
                                var ddlAgentName = document.getElementById('ddlAgentName').value;
                                var ddlRouteName = document.getElementById('ddlRouteName').value;
                                var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
                                var ddlType = document.getElementById('ddlType').value;
                                if (ddlType == "Agent Wise") {
                                    if (ddlSalesOffice == "" || ddlSalesOffice == "Select Sales Office") {
                                        alert("Select Sales Offic name");
                                        return false;
                                    }
                                    if (ddlRouteName == "" || ddlRouteName == "Select Route Name") {
                                        alert("Select route name");
                                        return false;
                                    }
                                    if (ddlAgentName == "" || ddlAgentName == "Select Agent Name") {
                                        alert("Select agent name");
                                        return false;
                                    }
                                }
                                if (ddlType == "Route Wise") {
                                    if (ddlSalesOffice == "" || ddlSalesOffice == "Select Sales Office") {
                                        alert("Select Sales Offic name");
                                        return false;
                                    }
                                    if (ddlRouteName == "" || ddlRouteName == "Select Route Name") {
                                        alert("Select route name");
                                        return false;
                                    }
                                }
                                if (ddlType == "Sales Office Wise") {
                                    if (ddlSalesOffice == "" || ddlSalesOffice == "Select Sales Office") {
                                        alert("Select Sales Offic name");
                                        return false;
                                    }
                                }
                                var ddldatatype = document.getElementById('ddldatatype').value;
                                if (ddlType == "Agent Wise") {
                                    var data = { 'operation': 'GetLineChart_agentduetransactions', 'startDate': txtFromdate, 'endDate': txtTodate, 'AgentName': ddlAgentName, 'RouteName': ddlRouteName, 'SalesOffice': ddlSalesOffice, 'Type': ddlType, 'ddldatatype': ddldatatype };
                                    var s = function (msg) {
                                        if (msg) {
                                            if (msg == "Session Expired") {
                                                alert(msg);
                                                window.location.assign("Login.aspx");
                                            }
                                            else {
                                                if (msg.length > 0) {
                                                    createdueChart(msg);
                                                }
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
                                else {
                                    for (var k = 0; k < salesTypes.length; k++) {
                                        if (salesTypes[k].status == "1") {
                                            salestype = salesTypes[k].salestype;
                                            var data = { 'operation': 'GetLineChart_agentduetransactions','salestype':salestype, 'classificationtype': salesTypes[k].sno, 'startDate': txtFromdate, 'endDate': txtTodate, 'AgentName': ddlAgentName, 'RouteName': ddlRouteName, 'SalesOffice': ddlSalesOffice, 'Type': ddlType, 'ddldatatype': ddldatatype };
                                            var s = function (msg) {
                                                if (msg) {
                                                    if (msg == "Session Expired") {
                                                        alert(msg);
                                                        window.location.assign("Login.aspx");
                                                    }
                                                    else {
                                                        if (msg.length > 0) {
                                                            createdueChart(msg);
                                                        }
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
                                    }
                                }
                            }
                            function remarks_save_click() {
                                var txtRemarks = document.getElementById('txtRemarks').value;
                                if (txtRemarks == "") {
                                    alert("Enter Remarks");
                                    return false;
                                }
                            }
                            function createdueChart(databind) {
                                var myTableDiv = document.getElementById("example");
                                var divleakbar = document.createElement("div");
                                myTableDiv.appendChild(divleakbar);
                                var ddlType = document.getElementById('ddlType').value;
                                var textname = "";
                                var aqty = databind[0].DeliveryQty;
                                var splitqty = aqty[1].split(',');
                                var avgqty = splitqty[0];
                                var sta = databind[0].Status;
                                var splitsta = sta[2];
                                if (ddlType == "Agent Wise") {
                                    var agent = document.getElementById("ddlAgentName");
                                    var agentname = agent.options[agent.selectedIndex].text;
                                    textname = agentname + " - " + " Agent wise Sale Value vs Due ";
                                }
                                if (ddlType == "Route Wise") {
                                    var agent = document.getElementById("ddlRouteName");
                                    var agentname = agent.options[agent.selectedIndex].text;
                                    textname = agentname + " -" + " Route wise " + splitsta + " - " + " Sale Value vs Due ";
                                }
                                if (ddlType == "Sales Office Wise") {
                                    var agent = document.getElementById("ddlSalesOffice");
                                    var agentname = agent.options[agent.selectedIndex].text;
                                    textname = agentname + " - " + splitsta + " - " + " Wise Sale Value vs Due ";
                                }
                                datainXSeries = 0;
                                datainYSeries = 0;
                                newXarray = [];
                                newYarray = [];
                                //                                $("#chart").empty();
                                //                                document.getElementById('chart').innerHTML = "";
                                for (var k = 0; k < databind.length; k++) {
                                    var BranchName = [];
                                    var IndentDate = databind[k].IndentDate;
                                    var UnitQty = databind[k].UnitQty;
                                    var DeliveryQty = databind[k].DeliveryQty;
                                    var Status = databind[k].Status;
                                    newXarray = IndentDate.split(',');
                                    for (var i = 0; i < DeliveryQty.length; i++) {
                                        newYarray.push({ 'data': DeliveryQty[i].split(','), 'name': Status[i] });
                                    }
                                }
                                $(divleakbar).kendoChart({
                                    title: {
                                        text: textname,
                                        color: "#0000FF",
                                        font: "bold italic 18px Arial,Helvetica,sans-serif"
                                    },
                                    legend: {
                                        position: "bottom"
                                    },
                                    chartArea: {
                                        background: ""
                                    },
                                    seriesDefaults: {
                                        type: "line",
                                        style: "smooth"
                                    },
                                    series: newYarray,
                                    valueAxis: {
                                        labels: {
                                            format: "{0}"
                                        },
                                        line: {
                                            visible: false
                                        },
                                        axisCrossingValue: -10
                                    },
                                    categoryAxis: {
                                        categories: newXarray,
                                        majorGridLines: {
                                            visible: false
                                        },
                                        labels: {
                                            rotation: 65
                                        }
                                    },
                                    seriesColors: ["#A52A2A", "#008000", "#00FFFF"],
                                    tooltip: {
                                        visible: true,
                                        format: "{0}%",
                                        template: "#= series.name #: #= value #"
                                    }
                                });
                            }
                        </script>
                    </div>
                </div>
                <div id="divremarks" style="padding-left:17%;display:none;">
                    <table >
                        <tr>
                            <td>
                                Remarks
                            </td>
                            <td style="height:40px;">
                                <textarea rows="3" cols="55" id="txtRemarks" class="form-control" maxlength="2000"
                                    placeholder="Enter Remarks"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td style="height:40px;">
                                <button type="button" class="btn btn-success" style="margin-right: 5px;"  value="Save" onclick="remarks_save_click();">
                                Save
                                </button>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 100%;" align="center">
                    <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="javascript:CallPrint('divPrint');">
                        <i class="fa fa-print"></i>Print
                    </button>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
