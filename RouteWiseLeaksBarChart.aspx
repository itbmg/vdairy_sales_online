<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RouteWiseLeaksBarChart.aspx.cs" Inherits="RouteWiseLeaksBarChart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="Kendo/jquery.min.js" type="text/javascript"></script>
    <script src="Kendo/kendo.all.min.js" type="text/javascript"></script>
    <link href="Kendo/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="Kendo/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <table>
            <tr>
                <td>
                    <div id="divSo" style="display: none;">
                        <select id="ddlSalesOffice" class="txtsize">
                        </select>
                    </div>
                </td>
                <td>
                    From date
                </td>
                <td>
                    <input type="date" id="txtFromdate" class="txtsize" />
                </td>
                <td>
                    <input type="button" id="submit" value="Submit" style="width: 120px; height: 24px;
                        font-size: 16px;" class="SaveButton" onclick="Details();" />
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
            function Details() {
                var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
                var txtFromdate = document.getElementById('txtFromdate').value;
                if (txtFromdate == "") {
                    alert("Select start date");
                    return false;
                }
                var data = { 'operation': 'Getroutewiseleaks', 'startDate': txtFromdate, 'ddlSalesOffice': ddlSalesOffice };
                var s = function (msg) {
                    if (msg) {
                        if (msg == "Session Expired") {
                            alert(msg);
                            window.location.assign("Login.aspx");
                        }
                        else {
                            createpieChart(msg);
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
            var newXarray = [];

            function createpieChart(databind) {
                $("#chart").empty();
                newXarray = [];
                var RouteName = databind[0].RouteName;
                var leakqty = databind[0].totalleak;
                //                for (var i = 0; i < RouteName.length; i++) {
                //                    newXarray.push({ "category": RouteName[i], "value": parseFloat(Amount[i]) });
                //                }
                $("#chart").kendoChart({
                    title: {
                        text: "Route Wise leaks"
                    },
                    legend: {
                        visible: false
                    },
                    seriesDefaults: {
                        type: "column"
                    },
                    series: [{
                        name: "Leak",
                        data: leakqty
                    }],
                    valueAxis: {
                        max: 5,
                        line: {
                            visible: false
                        },
                        minorGridLines: {
                            visible: true
                        }
                    },
                    categoryAxis: {
                        categories: RouteName,
                        majorGridLines: {
                            visible: false
                        }
                    },
                    tooltip: {
                        visible: true,
                        template: "#= series.name #: #= value #%"

                    }
                });
            }

            // $(document).ready(Details);
            //        $(document).ready(function () {
            //            setTimeout(function () {
            //                $("#example").bind("kendo:skinChange", function (e) {
            //                    //  createChartmethod();
            //                });
            //            }, 400);
            //        });
            $(document).bind("kendo:skinChange", createpieChart);
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
    </div>
</asp:Content>

