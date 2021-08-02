<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Productwiseleakpiechart.aspx.cs" Inherits="Productwiseleakpiechart" %>

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
<div id="divSo" style="display:none;">
<select id="ddlSalesOffice" class="txtsize" ></select>
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
                        font-size: 16px;" class="SaveButton" onclick="DetailsValidating()" />
</td>
</tr>
</table>
</div>
    <div id="example" class="k-content">
    <div class="chart-wrapper">
   <%-- style="background: center no-repeat url('../../content/shared/styles/world-map.png');"--%>
        <div id="chart" ></div>
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
            var data = { 'operation': 'GetProductWiseleaks', 'startDate': txtFromdate, 'ddlSalesOffice': ddlSalesOffice };
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
            var RouteName = databind[0].RouteName;
            var leakqty = databind[0].totalleak;
            for (var i = 0; i < RouteName.length; i++) {
                newXarray.push({ "category": RouteName[i], "value": parseFloat(leakqty[i]) });
            }
            $("#chart").kendoChart({
                title: {
                    position: "bottom",
                    text: "Product Wise Leaks",
                    color: "#006600"
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
                seriesColors: ["#C0C0C0", "#FF00FF", "#00FFFF", "#FFFF00", "#0000FF", "#00FF00", "#FF0000", "#B43104", "#8A084B", "#0041C2", "#800517", "#1C1715"],
                tooltip: {
                    visible: true,
                    format: "{0}%"
                }
            });
        }
    </script>
</div>
</asp:Content>

