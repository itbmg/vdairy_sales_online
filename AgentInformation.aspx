<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="AgentInformation.aspx.cs" Inherits="AgentInformation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Kendo/jquery.min.js" type="text/javascript"></script>
    <script src="Kendo/kendo.all.min.js" type="text/javascript"></script>
    <link href="Kendo/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="Kendo/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <script src="Js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <link href="Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        function agentimage(agentphoto) {
            var rndmnum = Math.floor((Math.random() * 10) + 1);
            var image = agentphoto[0].Agent_PIC;
            var ftplocation = agentphoto[0].ftplocation;
            img_url = ftplocation + image + '?v=' + rndmnum;
            if (image != "") {
                $('#main_img').attr('src', img_url).width(200).height(200);
            }
            else {
                $('#main_img').attr('src', 'Images/Employeeimg.jpg').width(200).height(200);
            }
        }

        function shopimage(shopphoto) {
            var rndmnum = Math.floor((Math.random() * 10) + 1);
            var image = shopphoto[0].Shop_Photo;
            var ftplocation = shopphoto[0].ftplocation;
            img_url1 = ftplocation + image + '?v=' + rndmnum;
            if (image != "") {
                $('#main_img1').attr('src', img_url1).width(200).height(200);
            }
            else {
                $('#main_img1').attr('src', 'Images/Employeeimg.jpg').width(200).height(200);
            }
        }
        $(function () {
            FillSalesOffice();
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#txtdate').val(today);
            $('#todate').val(today);
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
        function ddlBrancchange() {
            var BranchID = document.getElementById('ddlSalesOffice').value;
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
        function ddlroutchange() {
            var RouteID = document.getElementById('ddlRouteName').value;
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
        function btnGenerate_Click() {
            $('#divPrint').css('display', 'block');
            var agentid = document.getElementById('ddlAgentName').value;
            var data = { 'operation': 'get_Agent_Information_Details', 'agentid': agentid };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Time Out Expired") {
                        alert(msg);
                        return false;
                    }
                    else {
                        fillAgentDetails(msg);
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
        var agentarr = [];
        var productarr = [];
        var incentivearr = [];
        function fillAgentDetails(msg) {
            agentarr = msg[0].BranchDetails;
            productarr = msg[0].AgentSale;
            incentivearr = msg[0].AgentInentive;

            fillAgentincentivedetails();
            document.getElementById('lblagent').innerHTML = agentarr[0].BranchName;
            //            document.getElementById('lblagentcode').innerHTML = agentarr[0].BranchName;
            document.getElementById('lblcreateddate').innerHTML = agentarr[0].CreateDate;
            document.getElementById('lbl_lat').innerHTML = agentarr[0].lat;
            document.getElementById('lbl_long').innerHTML = agentarr[0].lng;

            document.getElementById('lbladdress').innerHTML = agentarr[0].address;
            document.getElementById('lblmob').innerHTML = agentarr[0].phone;
            document.getElementById('lblroutname').innerHTML = agentarr[0].RouteName;
            document.getElementById('lblsr').innerHTML = agentarr[0].Salesrep;
            document.getElementById('lblduelimit').innerHTML = agentarr[0].DueLimit;
            document.getElementById('lblbalamt').innerHTML = agentarr[0].BalAmount;
            document.getElementById('lblamtduedate').innerHTML = agentarr[0].CreateDate;
            document.getElementById('lblcratesdue').innerHTML = agentarr[0].CratesDue;
            document.getElementById('lblcratduedate').innerHTML = agentarr[0].CreateDate;
            document.getElementById('lblcansdue').innerHTML = agentarr[0].CansDue;
            document.getElementById('lblcansduedate').innerHTML = agentarr[0].CreateDate;
            document.getElementById('lblcompanyname').innerHTML = agentarr[0].CompanyName;
            document.getElementById('lblfreezertype').innerHTML = agentarr[0].freezer_type; ;
            document.getElementById('lblfreezerTamount').innerHTML = agentarr[0].TotalAmount; ;
            document.getElementById('lblinstallamount').innerHTML = agentarr[0].InstallAmount; ;

            var results = '<div  style="overflow:auto;"><table boarder="2" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">ProductName</th><th scope="col">Rate</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
            for (var i = 0; i < productarr.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '"><th scope="row" class="1">' + productarr[i].ProductName + '</th>';
                results += '<td data-title="brandstatus"  class="2">' + productarr[i].rate + '</td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#GridProductWiseRate").html(results);
        }

        function fillAgentincentivedetails() {
            var results = '<div  style="overflow:auto;"><table boarder="2" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">FromDate</th><th scope="col">ToDate</th><th scope="col">StructureName</th><th scope="col">LeakPercent</th><th scope="col">TotalDiscount</th><th scope="col">Remarks</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
            for (var i = 0; i < incentivearr.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '"><th scope="row" class="1">' + incentivearr[i].FromDate + '</th>';
                results += '<td data-title="brandstatus"  class="2">' + incentivearr[i].Todate + '</td>';
                results += '<td data-title="brandstatus"  class="2">' + incentivearr[i].StructureName + '</td>';
                results += '<td data-title="brandstatus"  class="2">' + incentivearr[i].leakagepercent + '</td>';
                results += '<td data-title="brandstatus"  class="2">' + incentivearr[i].TotalDiscount + '</td>';
                results += '<td data-title="brandstatus"  class="2">' + incentivearr[i].Remarks + '</td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#GridIncentive").html(results);
        }

        function ProductInformationpieChart(databind) {
            var newarray = [];
            $("#chart").empty();
            var myTableDiv = document.getElementById("chart");
            var divproductleakpie = document.createElement("div");
            myTableDiv.appendChild(divproductleakpie);
            //            var RouteName = databind[0].subCategeoryName;
            //            var leakqty = databind[0].Qty;
            for (var i = 0; i < databind.length; i++) {
                newarray.push({ "category": databind[i].subCategeoryName, "value": parseFloat(databind[i].Qty) });
            }
            $(divproductleakpie).kendoChart({
                title: {
                    //position: "bottom",
                    text: "Product Wise Qty",
                    color: "#FE2E2E",
                    align: "left",
                    font: "20px Verdana"

                },
                legend: {
                    visible: true
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
                    data: newarray
                },
                series: [{
                    type: "pie",
                    field: "value",
                    categoryField: "category"
                }],
                seriesColors: ["#C0C0C0", "#FF00FF", "#00FFFF", "#322B2B", "#0000FF", "#00FF00", "#FF0000", "#B43104", "#8A084B", "#0041C2", "#800517", "#1C1715"],
                tooltip: {
                    visible: true,
                    format: "{0}%"
                }
            });
        }
        var productsdata = []; var agentphoto = []; var shopphoto = [];
        function fillProdcutData() {
            var agentid = document.getElementById('ddlAgentName').value;
            var ddlFruits = document.getElementById('ddlAgentName');
            var AgentName = ddlFruits.options[ddlFruits.selectedIndex].innerHTML;
            //            var selectedValue = ddlFruits.value;
            var fromdate = document.getElementById('txtdate').value;
            var todate = document.getElementById('todate').value;
            getagentmnthlysale();
            var data = { 'operation': 'GetAgentPrdtInformation', 'agentid': agentid, 'fromdate': fromdate, 'todate': todate, 'AgentName': AgentName };
            var s = function (msg) {
                if (msg) {
                    agentphoto = msg[0].AgentImages;
                    shopphoto = msg[0].ShopImages;
                    productsdata = msg[0].TotalProductclass;
                    $('#tableProductData').removeTemplate();
                    $('#tableProductData').setTemplateURL('Product1.htm');
                    $('#tableProductData').processTemplate(productsdata);
                    ProductInformationpieChart(productsdata);
                    agentimage(agentphoto);
                    shopimage(shopphoto);
                    $('#divbutton').css('display', 'block');
                    $('#divPrint').css('display', 'none');
                    var i = 0;
                    $('.subCategeoryClass').each(function (i, obj) {
                        if ($(this).text() == "") {
                        }
                        else {
                            if (i == 0) {
                                $(this).css('color', '#C0C0C0');
                            }
                            if (i == 1) {
                                $(this).css('color', '#FF00FF');
                            }
                            if (i == 2) {
                                $(this).css('color', '#00FFFF');
                            }
                            if (i == 3) {
                                $(this).css('color', '#322B2B');
                            }
                            if (i == 4) {
                                $(this).css('color', '#0000FF');
                            }
                            if (i == 5) {
                                $(this).css('color', '#00FF00');
                            }
                            if (i == 6) {
                                $(this).css('color', '#FF0000');
                            }
                            if (i == 7) {
                                $(this).css('color', '#B43104');
                            }
                            if (i == 8) {
                                $(this).css('color', '#8A084B');
                            }
                            if (i == 9) {
                                $(this).css('color', '#0041C2');
                            }
                            if (i == 10) {
                                $(this).css('color', '#800517');
                            }
                            if (i == 11) {
                                $(this).css('color', '#1C1715');
                            }
                            if (i == 12) {
                                $(this).css('color', '#8A084B');
                            }
                            i++;
                        }
                    });
                    var TotQty = 0.0;
                    var j = 0;
                    $('.QtyClass').each(function (i, obj) {
                        if ($(this).text() == "") {
                        }
                        else {
                            if (j == 0) {
                                $(this).css('color', '#C0C0C0');
                            }
                            if (j == 1) {
                                $(this).css('color', '#FF00FF');
                            }
                            if (j == 2) {
                                $(this).css('color', '#00FFFF');
                            }
                            if (j == 3) {
                                $(this).css('color', '#322B2B');
                            }
                            if (j == 4) {
                                $(this).css('color', '#0000FF');
                            }
                            if (j == 5) {
                                $(this).css('color', '#00FF00');
                            }
                            if (j == 6) {
                                $(this).css('color', '#FF0000');
                            }
                            if (i == 7) {
                                $(this).css('color', '#B43104');
                            }
                            if (j == 8) {
                                $(this).css('color', '#8A084B');
                            }
                            if (j == 9) {
                                $(this).css('color', '#0041C2');
                            }
                            if (j == 10) {
                                $(this).css('color', '#800517');
                            }
                            if (j == 11) {
                                $(this).css('color', '#1C1715');
                            }
                            if (j == 12) {
                                $(this).css('color', '#8A084B');
                            }
                            j++;
                            TotQty += parseFloat($(this).text());
                        }
                    });
                    TotQty = parseFloat(TotQty).toFixed(2);
                    document.getElementById("txt_total").innerHTML = TotQty;
                    $('.QtyClass').each(function (i, obj) {
                        if ($(this).text() == "") {
                        }
                        else {
                            var Qty = parseFloat($(this).text());
                            var Percen = 0;
                            Percen = (Qty / TotQty) * 100;
                            Percen = parseFloat(Percen).toFixed(2);
                            $(this).closest("tr").find("#txtPercentage").text(Percen);
                        }
                    });
                    var k = 0;
                    $('.PercentageClass').each(function (i, obj) {
                        if ($(this).text() == "") {
                        }
                        else {
                            if (k == 0) {
                                $(this).css('color', '#C0C0C0');
                            }
                            if (k == 1) {
                                $(this).css('color', '#FF00FF');
                            }
                            if (k == 2) {
                                $(this).css('color', '#00FFFF');
                            }
                            if (k == 3) {
                                $(this).css('color', '#322B2B');
                            }
                            if (k == 4) {
                                $(this).css('color', '#0000FF');
                            }
                            if (k == 5) {
                                $(this).css('color', '#00FF00');
                            }
                            if (k == 6) {
                                $(this).css('color', '#FF0000');
                            }
                            if (k == 7) {
                                $(this).css('color', '#B43104');
                            }
                            if (k == 8) {
                                $(this).css('color', '#8A084B');
                            }
                            if (k == 9) {
                                $(this).css('color', '#0041C2');
                            }
                            if (k == 10) {
                                $(this).css('color', '#800517');
                            }
                            if (k == 11) {
                                $(this).css('color', '#1C1715');
                            }
                            if (k == 12) {
                                $(this).css('color', '#8A084B');
                            }
                            k++;
                        }
                    });


                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var type = "SingleProduct";
        function btnVariationProductlineChart(ID) {
            type = "SingleProduct";
            var SubcatSno = $(ID).closest("tr").find('#hdnsubCatsno').val();
            var agentid = document.getElementById('ddlAgentName').value;
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'getLineForProduct', 'SubcatSno': SubcatSno, 'agentid': agentid, 'type': type };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Time Out Expired") {
                        alert(msg);
                        return false;
                    }
                    else {
                        LineChartforsubcategeory(msg);
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
        function LineChartforsubcategeory(databind) {
            var datainXSeries = 0;
            var datainYSeries = 0;
            var newXarray = [];
            var newYarray = [];

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
            $("#divprdtline").kendoChart({
                title: {
                    text: "Product Wise Line Chart"
                },
                legend: {
                    visible: false
                },

                seriesDefaults: {
                    type: "line",
                    style: "smooth",
                    width: 90
                },
                series: newYarray,
                valueAxis: {
                    line: {
                        visible: false
                    },
                    minorGridLines: {
                        visible: true
                    }
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
                    template: "#= series.name #: #= value #"
                }
            });
        }
        function btnProductlineChart() {
            type = "All";

            var agentid = document.getElementById('ddlAgentName').value;
            var fromdate = document.getElementById('txtdate').value;
            var todate = document.getElementById('todate').value;
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'getLineForProduct', 'agentid': agentid, 'type': type, 'fromdate': fromdate, 'todate': todate };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Time Out Expired") {
                        alert(msg);
                        return false;
                    }
                    else {
                        LineChartforsubcategeory(msg);
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
        function test() {
            var lat = document.getElementById('lbl_lat').innerHTML;
            var long = document.getElementById('lbl_long').innerHTML;
            if (lat == "" && long == "") {
                alert("Location not found");
                return false;
            }
            window.open('Locations.html?lat=' + lat + "&long=" + long);
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
        function LocateAgentremarks() {
            //            window.open("AgentRemarks.aspx", "_self");
            window.open("AgentRemarks.aspx")
        }
        function getagentmnthlysale() {
            var agentid = document.getElementById('ddlAgentName').value;
            var fromdate = document.getElementById('txtdate').value;
            var todate = document.getElementById('todate').value;
            var data = { 'operation': 'get_Agent_Monthlsale_Line', 'agentid': agentid, 'fromdate': fromdate, 'todate': todate };
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
        var datainXSeries = 0;
        var datainYSeries = 0;
        var newXarray = [];
        var newYarray = [];
        var linechatrtdata = [];
        var griddata = [];
        function lineChart(databind) {
            var newYarray = [];
            var newXarray = [];
            linechatrtdata = databind[0].AgentSale;
            griddata = databind[0].AgentMonthlySale;
            createtablegridview(griddata);
            var branch = linechatrtdata[0].branchid;
            for (var k = 0; k < linechatrtdata.length; k++) {
                var BranchName = [];
                var IndentDate = linechatrtdata[k].Month;
                var Status = linechatrtdata[k].salevalue;
                newXarray = IndentDate.split(',');
                for (var i = 0; i < linechatrtdata.length; i++) {
                    newYarray.push({ 'data': linechatrtdata[i].salevalue.split(','), 'name': linechatrtdata[i].str.split(',') });
                }
            }
            $("#BranchLinechart").kendoChart({
                title: {
                    text: " Month Wise Comparison",
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
            var results = '<div  style="overflow:auto;"><table boarder="2" class="table table-bordered table-hover dataTable no-footer">';
            results += '<thead><tr><th scope="col">Month</th><th scope="col">SaleValue</th></tr></thead></tbody>';
            //var k = 1;
            for (var i = 0; i < griddata.length; i++) {
                //results += '<tr><td scope="row" class="1"  style="text-align:center;">' + (i + 1) + '</td>';
                results += '<td data-title="Product Name" class="2">' + griddata[i].Month + '</td>';
                results += '<td data-title="Product Value" class="3">' + griddata[i].salevalue + '</td>';
                //results += '<td style="display:none;"><label id="lbl_sno"></label></td>';
                //k++;
                results += '</tr>';
            }
            results += '</table></div>';
            $("#divAgentData").html(results);
        }
        function ShowRemarks() {
            getagentname();
            $('#divMainAddNewRow').css('display', 'block');
        }
        function CloseClick() {
            $('#divMainAddNewRow').css('display', 'none');
            $('#hiddeninward').css('display', 'block');
            $('#hiddenoutward').css('display', 'block');
        }
        function getagentname() {
            var agentname = ddlAgentName.options[ddlAgentName.selectedIndex].innerHTML;
            var agentid =document.getElementById('ddlAgentName').value;
            document.getElementById('spnaAgentName').innerHTML = agentname;
            document.getElementById('spnagentid').innerHTML = agentid;
        }
        function forclearall() {
            document.getElementById('txtRemarks').value = "";
            document.getElementById('spnaAgentName').innerHTML = "";
            document.getElementById('btn_save').value = "SAVE";
        }
        function saveAgentRemarks() {
//            var empname = document.getElementById('hiddenemployee').value;
            var AgentName = document.getElementById('spnagentid').innerHTML;
            var Remarks = document.getElementById('txtRemarks').value;
            var btnval = document.getElementById('btn_save').value;
            var data = { 'operation': 'saveAgentRemarks',  'AgentName': AgentName, 'Remarks': Remarks, 'btnVal': btnval };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        forclearall();
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

        function showAgentMaster() {
            
            $("#divAgentRemarksReport").css("display", "none");
            $("#div_agentMaster").css("display", "block");
        }
        function showAgentRemarksreport() {
            FillSales_Office();
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#txtFromDate').val(today);
            $('#txtToDate').val(today);
            $("#div_agentMaster").css("display", "none");
            $("#divAgentRemarksReport").css("display", "block");
        }
        function FillSales_Office() {
            var data = { 'operation': 'GetPlantSalesOffice' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    Bind_SalesOffice(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function Bind_SalesOffice(msg) {
            var ddlBranchName = document.getElementById('ddlBranchName');
            var length = ddlBranchName.options.length;
            ddlBranchName.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            ddlBranchName.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlBranchName.appendChild(opt);
                }
            }
        }
        function ddlBranc_change() {
            var BranchID = document.getElementById('ddlBranchName').value;
            var data = { 'operation': 'GetSalesRoutes', 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindRoute_Name(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindRoute_Name(msg) {
            document.getElementById('ddlRoute_Name').options.length = "";
            var veh = document.getElementById('ddlRoute_Name');
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

        function ddlrout_change() {
            var RouteID = document.getElementById('ddlRoute_Name').value;
            var data = { 'operation': 'GetAgents', 'RouteID': RouteID };
            var s = function (msg) {
                if (msg) {
                    BindAgent_Name(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindAgent_Name(msg) {
            document.getElementById('ddlAgent_Name').options.length = "";
            var ddlAgent_Name = document.getElementById('ddlAgent_Name');
            var length = ddlAgent_Name.options.length;
            for (i = length - 1; i >= 0; i--) {
                ddlAgent_Name.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Agent Name";
            opt.value = "";
            ddlAgent_Name.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlAgent_Name.appendChild(opt);
                }
            }
        }
        function getRemarks_click() {
            var agentid = document.getElementById('ddlAgent_Name').value;
            var ddlFruits = document.getElementById('ddlAgent_Name');
            var AgentName = ddlFruits.options[ddlFruits.selectedIndex].innerHTML;
            //            var selectedValue = ddlFruits.value;
            var fromdate = document.getElementById('txtFromDate').value;
            var todate = document.getElementById('txtToDate').value;
            var data = { 'operation': 'getRemarks_click', 'agentid': agentid, 'fromdate': fromdate, 'todate': todate };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    bind_AgentRemarks(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function bind_AgentRemarks(msg) {
            var branch = ddlAgent_Name.options[ddlAgent_Name.selectedIndex].innerHTML;
            $('#spnAgent').css('display', 'block');
            document.getElementById('spnAgent').innerHTML = "" + branch;
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">Date</th><th scope="col">Remarks</th></tr></thead></tbody>';
            //var k = 1;
            var k = 1;
            var l = 0;
            j = 1;
            var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '"><td scope="row" class="1">' + j + '</td>';
                results += '<td data-title="Product Name" class="2">' + msg[i].Date + '</td>';
                results += '<td data-title="Product Value" class="3">' + msg[i].Remarks + '</td>';
                results += '</tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
                j++;
            }
            results += '</table></div>';
            $("#Div_Remarks").html(results);
        }
    </script>
    <style type="text/css">
        .lblclass
        {
            color: #0252AA;
            font-size: 20px;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600">
    </asp:ToolkitScriptManager>
    <div>
    </div>
    <section class="content-header">
        <h1>
            Agent Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Masters</a></li>
            <li><a href="#">Agent Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
        <div>
                    <ul class="nav nav-tabs">
                        <li id="id_tab_Personal" class="active"><a data-toggle="tab" href="#" onclick="showAgentMaster()">
                            <i class="fa fa-street-view"></i>&nbsp;&nbsp;Agent Master</a></li>
                        <li id="id_tab_documents" class=""><a data-toggle="tab" href="#" onclick="showAgentRemarksreport()">
                            <i class="fa fa-file-text"></i>&nbsp;&nbsp;Agent Remarks Report</a></li>
                    </ul>
                </div>
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Agent Details
                </h3>
            </div>
            <div id="div_agentMaster">  
            <div class="box-body">
                        <div id="div1">
                            <table>
                                <tr>
                                    <td>
                                        <label id="lblsalesoffice">Sales Office:</label>
                                        </td>
                                        <td>
                                        <select id="ddlSalesOffice" class="form-control" onchange="ddlBrancchange();">
                                        </select>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                    <label id="lblroutename">Route Name:</label>
                                    </td>
                                    <td>
                                    <select id="ddlRouteName" class="form-control" onchange="ddlroutchange();">
                                        </select>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                    <label id="lblagentname">Agent Name:</label>
                                    </td>
                                    <td>
                                      <select id="ddlAgentName" class="form-control">
                                        </select>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server">From Date:</asp:Label>
                                        <input type="date" class="form-control" id="txtdate" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server">To Date:</asp:Label>
                                        <input type="date" class="form-control" id="todate" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server"></asp:Label>
                                           <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="fillProdcutData()"><i class="fa fa-refresh"></i> Generate </button>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divchart">
                            <table>
                                <tr>
                                    <td>
                                        <div id="tableProductData">
                                        </div>
                                    </td>
                                    <td>
                                        <div id="Div2" class="k-content">
                                            <div class="chart-wrapper">
                                                <div id="chart">
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divHide">
                            <div id="Div3" class="k-content">
                                <div class="chart-wrapper">
                                    <div id="divprdtline">
                                    </div>
                                </div>
                            </div>
                        </div>
                    <div id="example" class="k-content">
                    <div class="chart-wrapper">
                     <div>
                     <div id="divAgentData"  style="display:none;">
                      </div>
                      </div>
                        <div id="BranchLinechart">
                        </div>
                        </div>
                    </div>
                 
                    <div>
                    </BR>
                    </BR>
                        <div id="divbutton" style="float: right;">
                                           <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="btnGenerate_Click()"><i class="fa fa-plus-square"></i> More Detail </button>
                        </div>
                        </div>
                        <div id="divPrint" style="display:none;">
                            <div style="width: 100%;">
                                <div style="width: 100%;">
                                    <div style="width: 11%; float: left;">
                                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="120px"
                                            height="135px" />
                                    </div>
                                    <div style="left: 0%; text-align: center;">

                                        <label ID="lblTitle" style="font-size:26px;" ></label>
                                        <br />
                                    </div>
                                    <div style="width: 100%;">
                                        <span style="font-size: 16px; font-weight: bold; padding-left: 25%; text-decoration: underline;">
                                            AGENT MASTER</span><br />
                                        <br />
                                    </div>
                                </div>
                                   <div style="float:left;">
            <div class="pictureArea1">
                 <div id="Agentimage" style="float: right; margin: 37px 19px 0px 0px;">
                  Agent Photo   
                      <img class="center-block img-circle img-thumbnail img-responsive profile-img" id="main_img"
                    alt="Agent Image" src="Images/Employeeimg.jpg" style="border-radius: 5px; width: 200px; height: 200px; border-radius: 50%;" />
                    </div>
                      <div id="AgentShopImage" style="float: right; margin: 37px 19px 0px 0px;">
                      Shop Photo
                      <img class="center-block img-circle img-thumbnail img-responsive profile-img" id="main_img1"
                    alt="Shop Image" src="Images/Employeeimg.jpg" style="border-radius: 5px; width: 200px; height: 200px; border-radius: 50%;" />
                    </div>
                    </div>
                    </div>
                    
                  
                                <br />
                                <br />
                                <table style="width: 80%;">
                                    <tr>
                                        <td>
                                            <span style="font-size: 20px; font-weight: bold; padding-left: 12%; color: #0252aa;
                                                text-decoration: underline;">DETAILS</span><br />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="font-size: 12px; font-weight: bold; padding-left: 15%;">AGENT NAME:</span><br />
                                        </td>
                                        <td style="height:40px;">
                                        <label id="lblagent" class="lblclass">
                                                </Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="font-size: 12px; font-weight: bold; padding-left: 15%;">AGENT CODE:</span><br />
                                        </td>
                                        <td style="height:40px;">
                                         <label id="lblagentcode" class="lblclass">
                                                </Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="font-size: 12px; font-weight: bold; padding-left: 15%;">AGENT CREATED ON:</span><br />
                                        </td>
                                        <td style="height:40px;">
                                        <label id="lblcreateddate" class="lblclass">
                                                </Label>
                                        </td>
                                    </tr>
                                      <tr>
                                        <td>
                                            <span style="font-size: 12px; font-weight: bold; padding-left: 15%;">Coordinates:</span><br />
                                        </td>
                                        <td style="height:40px;">
                                        <label id="lbl_lat" class="lblclass">
                                                </Label>
                                                 <label id="lbl_long" class="lblclass">
                                                </Label>
                                        </td>
                                        <td>
                                     
                                           <button type="button"  id="Hyper_latlong"  class="btn btn-primary" style="margin-right: 5px;" onclick="test()"><i class="fa fa-location-arrow"></i> Get For location </button>
                    
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="font-size: 12px; font-weight: bold; padding-left: 15%;">ADDRESS:</span><br />
                                        </td>
                                        <td style="height:40px;">
                                            <label id="lbladdress" class="lblclass">
                                                </Label>
                                        </td>
                                        <td>
                              <%--  <a target="_blank" href="AgentRemarks.aspx"><span style="font-size: 18px;color: #0252aa;">Remarks</span></a>--%>
								 <input id="btn_remarks"  type="button"   class="btn btn-primary" value="Remarks" onclick="ShowRemarks();"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="font-size: 12px; font-weight: bold; padding-left: 15%;">MOBILE NUMBER:</span><br />
                                        </td>
                                        <td style="height:40px;">
                                        <label id="lblmob" class="lblclass">
                                                </Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="font-size: 12px; font-weight: bold; padding-left: 15%;">ROUTE NAME:</span><br />
                                        </td>
                                        <td style="height:40px;">
                                           <label id="lblroutname" class="lblclass">
                                                </Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="font-size: 12px; font-weight: bold; padding-left: 15%;">SALES REPRESENTATIVE:</span><br />
                                        </td>
                                        <td style="height:40px;">
                                        <label id="lblsr" class="lblclass">
                                                </Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="font-size: 12px; font-weight: bold; padding-left: 15%;">DUE LIMIT:</span><br />
                                        </td>
                                        <td style="height:40px;">
                                        <label id="lblduelimit" class="lblclass">
                                                </Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="font-size: 20px; font-weight: bold; padding-left: 12%; color: #0252aa;
                                                text-decoration: underline;">ASSETS</span><br />
                                            <br />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="font-size: 12px; font-weight: bold; padding-left: 15%;">BALANCE RECEIVABLE
                                                RS:</span><br />
                                        </td>
                                        <td style="height:40px;">
                                        <label id="lblbalamt" class="lblclass">
                                                </Label>
                                            <span style="font-size: 12px; font-weight: bold;">as on date:</span>
                                             <label id="lblamtduedate" class="lblclass">
                                                </Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="font-size: 12px; font-weight: bold; padding-left: 15%;">BALANCE CRATES
                                                RECEIVABLE:</span><br />
                                        </td>
                                        <td style="height:40px;">
                                        <label id="lblcratesdue" class="lblclass">
                                                </Label>
                                            <span style="font-size: 12px; font-weight: bold;">as on date:</span>
                                             <label id="lblcratduedate" class="lblclass">
                                                </Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="font-size: 12px; font-weight: bold; padding-left: 15%;">BALANCE CANS RECEIVABLE:</span><br />
                                        </td>
                                        <td style="height:40px;">
                                        <label id="lblcansdue" class="lblclass">
                                                </Label>

                                            <span style="font-size: 12px; font-weight: bold;">as on date:</span>

                                             <label id="lblcansduedate" class="lblclass">
                                                </Label>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>
                                            <span style="font-size: 20px; font-weight: bold; padding-left: 12%; color: #0252aa;
                                                text-decoration: underline;">FreeZerDetails</span><br />
                                            <br />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="font-size: 12px; font-weight: bold; padding-left: 15%;">Company Name
                                               </span><br />
                                        </td>
                                        <td style="height:40px;">
                                         <label id="lblcompanyname" class="lblclass">
                                                </Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="font-size: 12px; font-weight: bold; padding-left: 15%;">FreezerType 
                                                </span><br />
                                        </td>
                                        <td style="height:40px;">
                                        <label id="lblfreezertype" class="lblclass">
                                                </Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="font-size: 12px; font-weight: bold; padding-left: 15%;">TotalAmount:</span><br />
                                        </td>
                                        <td style="height:40px;">
                                         <label id="lblfreezerTamount" class="lblclass">
                                                </Label>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>
                                            <span style="font-size: 12px; font-weight: bold; padding-left: 15%;">Installamount:</span><br />
                                        </td>
                                        <td style="height:40px;">
                                         <label id="lblinstallamount" class="lblclass">
                                                </Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                                
                            </div>
                             <span style="font-size: 20px; font-weight: bold; padding-left: 12%; color: #0252aa;
                                                text-decoration: underline;">Incentive</span><br />
                               <div id="GridIncentive">  
                               </div>
                             <span style="font-size: 20px; font-weight: bold; padding-left: 12%; color: #0252aa;
                                                text-decoration: underline;">Product Wise rates</span><br />
                          <div id="GridProductWiseRate">
                          </div>
                        </div>
                        <label id="lblmsg">
                                                </Label>
                <br />
                <br />
                  <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="javascript:CallPrint('divPrint');"><i class="fa fa-print"></i> Print </button>
          <div id="divMainAddNewRow" class="pickupclass" style="text-align: center; height: 100%;
                width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
                background: rgba(192, 192, 192, 0.7);">
                <div id="divAddNewRow" style="border: 5px solid #A0A0A0; position: absolute; top: 8%;
                    background-color: White; left: 10%; right: 10%; width: 80%; height: 10%; -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                    -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                    border-radius: 10px 10px 10px 10px;">
                    <table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;"
                        id="tableCollectionDetails" class="mainText2" border="1">
                      <tr>
                                <td>
                                   <label>Agent Name</label>
                                   </td>
                                   <td style="height: 40px;">
                                   <span id="spnaAgentName" type="text" style="color: Red; font-weight: bold;
                                    font-size: 25px;" class="clsAgentName" name="AgentName"></span> 
                                   </td>   
                                   </tr>
                               <tr>
                                   <td style="height: 40px;">
                                      <label id="lblremarks">
                                                Remarks:</label>
                                        </td>
                                        <td colspan="3" style="height: 40px;">
                                            <textarea rows="3" cols="45" id="txtRemarks" placeholder="Enter Remarks"></textarea>
                                        </td>
                                   </tr>
                                    <tr>
                                    <td>
                                    </td>
                                        <td>
                                            <input type="button" id="btn_save" value="Save" onclick="saveAgentRemarks();" class="btn btn-primary" />
                                            <input type="button" id="btnclear" value="Clear" class="btn btn-warning"
                                                onclick="forclearall();" />
                                                 <input type="button" class="btn btn-danger" id="close_vehmaster" value="Close" onclick="CloseClick();" />
                                        </td>
                                    </tr>
                                       <span id="spnagentid" type="text" style="width: 500px; color: Red; font-weight: bold;
                                    font-size: 25px;display:none" class="clsAgentName" name="AgentName"></span>
                                   <span id="hiddenemployee" class="form-control" style="display:none;"> </span>
                    </table>
             </div>
           <div id="divclose" style="width: 35px; top: 7.5%; right: 8%; position: absolute;
                    z-index: 99999; cursor: pointer;">
                    <img src="Images/Close.png" alt="close" onclick="CloseClick();" />
                </div>
          </div>
            </div>
            </div>
            <div id="divAgentRemarksReport" style="display:none;">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Agent Remarks Report
                </h3>
            </div>
            <div class="box-body">
                  <table>
                                <tr>
                                    <td>
                                        <label id="Label4">Sales Office:</label>
                                        </td>
                                        <td>
                                        <select id="ddlBranchName" class="form-control" onchange="ddlBranc_change();">
                                        </select>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                    <label id="Label5">Route Name:</label>
                                    </td>
                                    <td>
                                    <select id="ddlRoute_Name" class="form-control" onchange="ddlrout_change();">
                                        </select>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                    <label id="Label6">Agent Name:</label>
                                    </td>
                                    <td>
                                      <select id="ddlAgent_Name" class="form-control">
                                        </select>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <asp:Label ID="Label7" runat="server">From Date:</asp:Label>
                                        <input type="date" class="form-control" id="txtFromDate" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <asp:Label ID="Label8" runat="server">To Date:</asp:Label>
                                        <input type="date" class="form-control" id="txtToDate" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server"></asp:Label>
                                        <input type="button" id="Button3" value="Generate" class="btn btn-primary" onclick="getRemarks_click();" />
                                    </td>
                                </tr>
                            </table>
                           <span id="spnAgent" style="width: 235px; color: Red; font-weight: bold; font-size: 18px;display:none;"></span> 
            </div>
            <div id="Div_Remarks">
                </div>
            </div>
        </div>
    </section>
</asp:Content>
