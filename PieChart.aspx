<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PieChart.aspx.cs" Inherits="PieChart" %>

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
            Pie Chart<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-pie-chart"></i>Chart Reports</a></li>
            <li><a href="#">Pie Chart</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Pie Chart
                </h3>
            </div>
            <div class="box-body">
                <div>
                    <table>
                        <tr>
                            <td>
                                <select id="ddlType" class="form-control">
                                    <option>Plant Wise Products</option>
                                    <option>Sales Office Wise Products</option>
                                    <option>Amount Wise</option>
                                </select>
                            </td>
                             <td style="width: 5px;">
                            </td>
                            <td>
                                <div id="divSo" style="display: none;">
                                    <select id="ddlSalesOffice" class="form-control">
                                    </select>
                                </div>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                From date
                            </td>
                            <td>
                                <input type="date" id="txtFromdate" class="form-control" />
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <input type="button" id="submit" value="Submit" class="btn btn-primary" onclick="DetailsValidating()" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="example" class="k-content">
                    <div class="chart-wrapper">
                        <%-- style="background: center no-repeat url('../../content/shared/styles/world-map.png');"--%>
                        <div id="chart">
                        </div>
                    </div>
                    <script type="text/javascript">
                        $(function () {
                            FillSalesOffice();
                        });
                        function FillSalesOffice() {
                            var data = { 'operation': 'GetSalesOffice' };
                            var s = function (msg) {
                                if (msg) {
                                    if (msg == "Session Expired") {
                                        alert(msg);
                                        window.location.assign("Login.aspx");
                                    }
                                    else {
                                        if (msg.length > 0) {
                                            $('#divSo').css('display', 'block');
                                            BindSalesOffice(msg);
                                        }
                                        else {
                                            $('#divSo').css('display', 'none');

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
                        function BindSalesOffice(msg) {
                            document.getElementById('ddlSalesOffice').options.length = "";
                            var veh = document.getElementById('ddlSalesOffice');
                            var length = veh.options.length;
                            for (i = length - 1; i >= 0; i--) {
                                veh.options[i] = null;
                            }
                            var opt = document.createElement('option');
                            opt.innerHTML = "Select Sales Office";
                            opt.value = "";
                            veh.appendChild(opt);
                            for (var i = 0; i < msg.length; i++) {
                                if (msg[i] != null) {
                                    var opt = document.createElement('option');
                                    opt.innerHTML = msg[i].BranchName;
                                    opt.value = msg[i].Sno;
                                    veh.appendChild(opt);
                                }
                            }
                        }
                        function DetailsValidating() {
                            var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
                            var txtFromdate = document.getElementById('txtFromdate').value;
                            if (txtFromdate == "") {
                                alert("Select start date");
                                return false;
                            }
                            var ddlType = document.getElementById('ddlType').value;
                            var data = { 'operation': 'GetPieChartValues', 'startDate': txtFromdate, 'ddlSalesOffice': ddlSalesOffice, 'ddlType': ddlType };
                            var s = function (msg) {
                                if (msg) {
                                    if (msg == "Session Expired") {
                                        alert(msg);
                                        window.location.assign("Login.aspx");
                                    }
                                    else {
                                        createChart(msg);
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
                        var newXarray = [];
                        function createChart(databind) {
                            $("#chart").empty();
                            newXarray = [];
                            var textname = "";
                            var ddlType = document.getElementById('ddlType').value;
                            if (ddlType == "Amount Wise") {
                                var agent = document.getElementById("ddlSalesOffice");
                                var agentname = agent.options[agent.selectedIndex].text;
                                textname = agentname + " Amount Collections ";
                            }
                            if (ddlType == "Plant Wise Products") {
                                textname = " Plant Wise Products";
                            }
                            if (ddlType == "Sales Office Wise Products") {
                                var agent = document.getElementById("ddlSalesOffice");
                                var agentname = agent.options[agent.selectedIndex].text;
                                textname = agentname + " Products ";
                            }
                            var Amount = databind[0].Amount;
                            var RouteName = databind[0].RouteName;
                            for (var i = 0; i < RouteName.length; i++) {
                                newXarray.push({ "category": RouteName[i], "value": parseFloat(Amount[i]) });
                            }
                            $("#chart").kendoChart({
                                title: {
                                    position: "bottom",
                                    text: textname,
                                    color: "#006600",
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
                                seriesColors: ["#3275a8", "#267ed4", "#068c35", "#808080", "#FFA500", "#A52A2A", "#FF7F50", "#00FF00", "#808000", "#0041C2", "#800517", "#1C1715"],
                                tooltip: {
                                    visible: true,
                                    format: "{0}%"
                                }
                            });
                        }
                    </script>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
