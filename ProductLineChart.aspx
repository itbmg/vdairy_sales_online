<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ProductLineChart.aspx.cs" Inherits="AgentRemarks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    <script src="Kendo/jquery.min.js" type="text/javascript"></script>
    <script src="Kendo/kendo.all.min.js" type="text/javascript"></script>
    <link href="Kendo/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="Kendo/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            getProductNames();
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#txtFromDate').val(today);
            $('#txtTodate').val(today);
        });
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        function ValidateAlpha(evt) {
            var keyCode = (evt.which) ? evt.which : evt.keyCode
            if ((keyCode < 65 || keyCode > 90) && (keyCode < 97 || keyCode > 123) && keyCode != 32)
                return false;
            return true;
        }
//        function getagentname() {
//            document.getElementById('txtProductName').value = '<%= Session["ProductName"] %>';
//            productid.innerHTML = '<%= Session["productid"] %>';
////            document.getElementById('txtProductName').value = productname;
//        }
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
        var ProductArr = [];
        function getProductNames() {
            var data = { 'operation': 'GetProductnames' };
            var s = function (msg) {
                if (msg.length > 0) {
                    ProductArr = msg;
                    searchempProductName();
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function searchempProductName() {
            var compiledList1 = [];
            for (var i = 0; i < ProductArr.length; i++) {
                var ProductName = ProductArr[i].ProductName;
                compiledList1.push(ProductName);
            }
            $('#txtProductName').autocomplete({
                source: compiledList1,
                //change: changeEmployeeName,
                autoFocus: true
            });
        }

        function ravi() {
            var ProductName = document.getElementById('txtProductName').value;
            for (var i = 0; i < ProductArr.length; i++) {
                if (name == ProductArr[i].ProductName) {
                    document.getElementById('txtproductid').value = ProductArr[i].sno;
                }
            }
        }
        function cancelAddressdetails() {
            forclearall();
        }
        function get_Product_Monthly_Sale() {
            var fromdate = document.getElementById('txtFromDate').value;
            var todate = document.getElementById('txtTodate').value;
            var productid = document.getElementById('txtProductName').value;
            get_Product_Monthly_Sale_Pie();
            var data = { 'operation': 'get_Product_Monthly_Sale', 'productid': productid, 'fromdate': fromdate, 'todate': todate };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Time Out Expired") {
                        alert(msg);
                        return false;
                    }
                    else {
                        lineChart(msg);
                        //                        createtablegridview(msg);
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
        function get_Product_Monthly_Sale_Pie() {
            var fromdate = document.getElementById('txtFromDate').value;
            var todate = document.getElementById('txtTodate').value;
            var productid = document.getElementById('txtProductName').value;
            var data = { 'operation': 'get_Product_Monthly_Sale_Pie', 'productid': productid, 'fromdate': fromdate, 'todate': todate };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Time Out Expired") {
                        alert(msg);
                        return false;
                    }
                    else {
                        createpieChart(msg);
                        pichartdata(msg);
                        //                        createtablegridview(msg);
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
        var datainXSeries = 0;
        var datainYSeries = 0;
        var newXarray = [];
        var newYarray = [];
        var linechatrtdata = [];
        var griddata = [];
        var productname = "";
        function lineChart(databind) {
            var newYarray = [];
            var newXarray = [];
            linechatrtdata = databind[0].ProductSale;
            griddata = databind[0].ProductMonthlySale;
            createtablegridview(griddata);
            var branch = linechatrtdata[0].branchid;
            for (var k = 0; k < linechatrtdata.length; k++) {
                var BranchName = [];
                var IndentDate = linechatrtdata[k].Month;
                var Status = linechatrtdata[k].salevalue;
                productname = document.getElementById('txtProductName').value;
                newXarray = IndentDate.split(',');
                for (var i = 0; i < linechatrtdata.length; i++) {
                    newYarray.push({ 'data': linechatrtdata[i].salevalue.split(','), 'name': linechatrtdata[i].str.split(',') });
                }
            }
            $("#ProductsLinechart").kendoChart({
                title: {
                    text:""+productname+ "",
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
                tooltip: {
                    visible: true,
                    format: "{0}%",
                    template: "#= series.name #: #= value #"
                }
            });
        }
        function createtablegridview(griddata) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer">';
            results += '<thead><tr><th scope="col">Month</th><th scope="col">DelivaryQty</th><th scope="col">SaleValue</th></tr></thead></tbody>';
            //var k = 1;
            for (var i = 0; i < griddata.length; i++) {
                //results += '<tr><td scope="row" class="1"  style="text-align:center;">' + (i + 1) + '</td>';
                results += '<td data-title="Product Name" class="2">' + griddata[i].Month + '</td>';
                results += '<td data-title="Product Name" class="2">' + griddata[i].DelivaryQty + '</td>';
                results += '<td data-title="Product Value" class="3">' + griddata[i].salevalue + '</td>';
                //results += '<td style="display:none;"><label id="lbl_sno"></label></td>';
                //k++;
                results += '</tr>';
            }
            results += '</table></div>';
            $("#divProductData").html(results);
        }

        var newXarray = [];
        function createpieChart(databind) {
            $("#ProductPie_Chart").empty();
            var textname = "";
            newXarray = [];
            for (var i = 0; i < databind.length; i++) {
                newXarray.push({ "category": databind[i].BranchName, "value": parseFloat(databind[i].salevalue) });
            }
            $("#ProductPie_Chart").kendoChart({
                title: {
                    position: "top",
                    text: ""+productname+"PIE Chart",
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
                        template: "#= category #:#= value#"
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
                seriesColors: ["#C0C0C0", "#FF00FF", "#00FFFF", "#322B2B", "#0000FF", "#00FF00", "#FF0000", "#B43104", "#8A084B", "#0041C2", "#800517", "#1C1715", "#008B00", "#8B8386", "#FFDEAD", "#8B2500", "#8B475D"],
                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
        }
        function pichartdata(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col"></th><th scope="col">BranchName</th><th scope="col">DelivaryQty</th><th scope="col">SaleValue</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '"><td><input id="btn_poplate" type="button"  onclick="getme(this)" name="submit" class="btn btn-primary" value="View" /></td>';
                results += '<th scope="row" style="display:none;" class="1" >' + msg[i].Branchid + '</th>';
                results += '<td scope="row" class="2" >' + msg[i].BranchName + '</th>';
                results += '<td scope="row" class="2" >' + msg[i].DelivaryQty + '</th>';
                results += '<td data-title="brandstatus"  class="3">' + msg[i].salevalue + '</td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#divPieChartdata").html(results);
        }
        function getme(thisid) {
            var Branchid = $(thisid).parent().parent().children('.1').html();
            var fromdate = document.getElementById('txtFromDate').value;
            var todate = document.getElementById('txtTodate').value;
            var productid = document.getElementById('txtProductName').value;
            var data = { 'operation': 'get_Product_Routewise_Data', 'productid': productid, 'Branchid': Branchid, 'fromdate': fromdate, 'todate': todate };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Time Out Expired") {
                        alert(msg);
                        return false;
                    }
                    else {
                        pichartdataroutewise(msg);
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
        function pichartdataroutewise(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">BranchName</th><th scope="col">DelivaryQty</th><th scope="col">SaleValue</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<th scope="row" style="display:none;" class="1" >' + msg[i].Branchid + '</th>';
                results += '<td scope="row" class="2" >' + msg[i].BranchName + '</th>';
                results += '<td data-title="brandstatus"  class="3">' + msg[i].DelivaryQty + '</td>'
                results += '<td data-title="brandstatus"  class="3">' + msg[i].salevalue + '</td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#divPieChartdata").html(results);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Product LineChart 
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operation</a></li>
            <li><a href="#">Product LineChart</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Product LineChart
                </h3>
            </div>
            <div class="box-body">
            <div>
             <a href="Product_Master.aspx"><span style="font-size: 18px;color: #0252aa;">Back</span></a> 
            </div>
             <div id="ProductLineChartTab">
                         <table>
                                <tr>
                                    <td>
                                        <label>
                                            ProductName:</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="text" id="txtProductName" class="form-control" />
                                    </td>
                                    <td>
                                        <label>
                                            FromDate:</label>
                                    </td>
                                    <td style="height: 40px;">
                                    <input type="date" id="txtFromDate" class="form-control" />
                                    </td>
                                    <td>
                                        <label>
                                            ToDate:</label>
                                    </td>
                                    <td style="height: 40px;">
                                     <input type="date" id="txtTodate" class="form-control" />
                                    </td>
                                     <td style="height: 40px;">
                                        <input type="button" id="btnGenarate" value="Generate" onclick="get_Product_Monthly_Sale();"
                                            class="btn btn-primary" />
                                    </td>
                                </tr>
                            </table>
                            <span id="txtproductid" style="display:none;" class="form-control"></span>
                        <div>
                    <div id="example" class="k-content">
                    <div class="chart-wrapper">
                        <div id="ProductsLinechart">
                        </div>
                         <div id="divProductData">
                      </div>
                    </div>
                    </div>
                    <div id="example1" class="k-content">
                    <div class="chart-wrapper">
                        <div id="ProductPie_Chart">
                        </div>
                        <div id="divPieChartdata">
                      </div>
                    </div>
                        </div>
                        </div>
            </div>
        </div>
    </section>
</asp:Content>
