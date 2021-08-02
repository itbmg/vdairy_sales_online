<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Classification_linechart.aspx.cs" Inherits="Classification_linechart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="Kendo/jquery.min.js" type="text/javascript"></script>
    <script src="Kendo/kendo.all.min.js" type="text/javascript"></script>
    <link href="Kendo/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="Kendo/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Classification agents Sales Vs Avg Sales<small>Preview</small>
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
                      <%--  <tr>
                            <td>
                                <label>
                                    Type</label>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td style="height: 40px;">
                                <select id="ddl_classificationtype" class="form-control">
                                    <option value="20">AGENTS</option>
                                    <option value="32">CATERING AGENTS</option>
                                    <option value="34">DISCONTINUED AGENTS</option>
                                    <option value="33">DUE AGENTS</option>
                                    <option value="18">INSTITUTIONAL</option>
                                    <option value="37">Comapass Group</option>
                                    <option value="36">Fresh and Honest cafe</option>
                                    <option value="22">Parlour</option>
                                </select>
                            </td>
                        </tr>--%>
                        <tr>
                            <td>
                                <label>
                                    Chart Type</label>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td style="height: 40px;">
                                <select id="ddlType" class="form-control">
                                    <option>Sales Office Wise</option>
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
                                <select id="ddlSalesOffice" class="form-control">
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
                              <a target='_blank' href="LineChart.aspx">get Route Wise Sales</a>
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
                <div id="divPrint">
                    <div id="example" class="k-content absConf">
                        <div class="chart-wrapper" style="margin: auto;">
                            <div id="chart">
                            </div>
                        </div>
                        <script type="text/javascript">
                            $(function () {
                                FillSalesOffice();
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
                            function BindSalesOffice(msg) {
                                var ddlsalesOffice = document.getElementById('ddlSalesOffice');
                                var length = ddlsalesOffice.options.length;
                                ddlsalesOffice.options.length = null;
//                                var opt = document.createElement('option');
//                                opt.innerHTML = "Select Sales Office";
//                                ddlsalesOffice.appendChild(opt);
                                for (var i = 0; i < msg.length; i++) {
                                    if (msg[i].BranchName != null) {
                                        var opt = document.createElement('option');
                                        opt.innerHTML = msg[i].BranchName;
                                        opt.value = msg[i].Sno;
                                        ddlsalesOffice.appendChild(opt);
                                    }
                                }
                            }
                            var salestype = "";
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
                                var ddldatatype = document.getElementById('ddldatatype').value;
                                var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
                                var ddlType = document.getElementById('ddlType').value;
                                if (ddlType == "Sales Office Wise") {
                                    if (ddlSalesOffice == "" || ddlSalesOffice == "Select Sales Office") {
                                        alert("Select Sales Offic name");
                                        return false;
                                    }
                                }
//                                var ddl_classificationtype = document.getElementById('ddl_classificationtype').value;
                                for (var k = 0; k < salesTypes.length; k++) {
                                    if (salesTypes[k].status == "1") {
                                        salestype = salesTypes[k].salestype;
                                        var data = { 'operation': 'GetLineChart_classificationindentreport','salestype':salestype,'ddldatatype':ddldatatype, 'classificationtype': salesTypes[k].sno, 'startDate': txtFromdate, 'endDate': txtTodate, 'SalesOffice': ddlSalesOffice, 'Type': ddlType };
                                        var s = function (msg) {
                                            if (msg) {
                                                if (msg == "Session Expired") {
                                                    alert(msg);
                                                    window.location.assign("Login.aspx");
                                                }
                                                else {
                                                    if (msg.length > 0) {
                                                        createChart(msg);
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
                            var datainXSeries = 0;
                            var datainYSeries = 0;
                            var newXarray = [];
                            var newYarray = [];
                            function createChart(databind) {
                                var myTableDiv = document.getElementById("example");
                                var divleakbar = document.createElement("div");
                                myTableDiv.appendChild(divleakbar);
                                var ddlType = document.getElementById('ddlType').value;
                                var textname = "";
                                var aqty = databind[0].DeliveryQty;
                                var splitqty = aqty[1].split(',');
                                var avgqty = splitqty[0];
                                var sta = databind[0].Status;
                                var splitsta = sta[1];
                                //                                if (ddlType == "Agent Wise") {
                                //                                    var agent = document.getElementById("ddlAgentName");
                                //                                    var agentname = agent.options[agent.selectedIndex].text;
                                //                                    textname = agentname + " Agent wise sales vs avg sales --> " + avgqty;
                                //                                }
                                //                                if (ddlType == "Route Wise") {
                                //                                    var agent = document.getElementById("ddlRouteName");
                                //                                    var agentname = agent.options[agent.selectedIndex].text;
                                //                                    textname = agentname + " Route wise sales vs avg sales --> " + avgqty;
                                //                                }
                                if (ddlType == "Sales Office Wise") {
                                    var agent = document.getElementById("ddlSalesOffice");
                                    var agentname = agent.options[agent.selectedIndex].text;
//                                    var ddl_classificationtype = document.getElementById('ddl_classificationtype');
//                                    var name = ddl_classificationtype.options[ddl_classificationtype.selectedIndex].text;
                                    textname = agentname + " - " + splitsta + " - " + " sales vs avg sales --> " + avgqty;
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
                                    seriesColors: ["#a0a700", "#0041C2", "#ff8d00"],
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
                <div style="padding-left:30%;">
                    <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="javascript:CallPrint('divPrint');">
                        <i class="fa fa-print"></i>Print
                    </button>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
