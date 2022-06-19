<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ProductWise_chartReport.aspx.cs" Inherits="ProductWise_chartReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="css/style.css?v=1113" type="text/css" media="all" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Kendo/jquery.min.js" type="text/javascript"></script>
    <script src="Kendo/kendo.all.min.js" type="text/javascript"></script>
    <link href="Kendo/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="Kendo/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);

        });
    </script>
    <style type="text/css">
        .k-chart
        {
            /* height: 150px;
            width: 150px;*/
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Product wise Sales Vs Avg Sales<small>Preview</small>
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
                <div id="example" class="k-content">
                    <script type="text/javascript">
            $(function () {
                FillProductName();
            });
            function FillProductName() {
                var data = { 'operation': 'Updatesubcategorytypemanage' };
                var s = function (msg) {
                    if (msg) {
                        if (msg == "Time Out Expired") {
                            alert(msg);
                            return false;
                        }
                        else {
                            BindProductName(msg);
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
            function BindProductName(msg) {
                var ddlProductName = document.getElementById('ddlProductName');
                var length = ddlProductName.options.length;
                ddlProductName.options.length = null;
                var opt = document.createElement('option');
                opt.innerHTML = "select";
                ddlProductName.appendChild(opt);
                for (var i = 0; i < msg.length; i++) {
                    if (msg[i].subcatname != null) {
                        var opt = document.createElement('option');
                        opt.innerHTML = msg[i].subcatname;
                        opt.value = msg[i].sno;
                        ddlProductName.appendChild(opt);
                    }
                }
            }
            function DetailsValidating() {
                var Fromdate = document.getElementById('txtFromdate').value;
                if (Fromdate == "") {
                    alert("Please select Fromdate");
                    return false;
                }
                var Todate = document.getElementById('txtTodate').value;
                if (Todate == "") {
                    alert("Please select Todate");
                    return false;
                }
                   var ProductName = document.getElementById('ddlProductName').value;
                if (ProductName == "") {
                    alert("Please select Product Name");
                    return false;
                }
                $("#Unauthorized").empty();
                $("#chart").empty();
                $('#chart').css('display', 'block');
                $('#divtable').css('display', 'none');
                var data = { 'operation': 'getLineChartforsubcategeoryReport1', 'Fromdate': Fromdate,'Todate':Todate,'ProductName':ProductName };
                var s = function (msg) {
                    if (msg) {
                        if (msg == "Time Out Expired") {
                            alert(msg);
                            return false;
                        }
                        else {
                            createChartmethod(msg);
                        }
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                    // $('#BookingDetails').html(x);
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                  callHandler(data, s, e);
            }
            function createChartmethod(databind) {
              var datainXSeries = 0;
                          var      datainYSeries = 0;
                         var       newXarray = [];
                         var       newYarray = [];
                          var textname = "";
                                var aqty = databind[0].DeliveryQty;
                                var splitqty = aqty[1].split(',');
                                var avgqty = splitqty[0];
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
                                 var agent = document.getElementById("ddlProductName");
                                    var productname = agent.options[agent.selectedIndex].text;
                textname=productname+" sales Vs Avg Sales --> " + avgqty;

                $("#chart").kendoChart({
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
                    
                },
                categoryAxis: {
                    categories: newXarray,
                    //                        categories: [2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011],
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
                <div style="width: 100%;" align="center">
                    <table>
                        <tr>
                            <td>
                                <span>Product Name</span>
                                <select id="ddlProductName" class="form-control">
                                </select>
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <span>From Date</span>
                                <input type="date" id="txtFromdate" class="form-control" />
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="To Date"></asp:Label>
                                <input type="date" id="txtTodate" class="form-control" />
                            </td>
                            <td style="width: 6px;">
                            </td>
                            <td>
                                <input type="button" id="submit" value="Submit" class="btn btn-success" onclick="DetailsValidating()" />
                            </td>
                        </tr>
                    </table>
                    <span id="lblmsg" class="lblmsg"></span>
                    <br />
                </div>
                <div class="demo-section k-content">
                    <div id="chart">
                    </div>
                    <div style="width: 100%;" align="center">
                        <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="javascript:CallPrint('divPrint');">
                            <i class="fa fa-print"></i>Print
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
