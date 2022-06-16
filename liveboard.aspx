<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="liveboard.aspx.cs" Inherits="liveboard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Kendo/jquery.min.js" type="text/javascript"></script>
    <script src="Kendo/kendo.all.min.js" type="text/javascript"></script>
    <link href="Kendo/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="Kendo/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <script src="https://www.amcharts.com/lib/3/amcharts.js"></script>
    <script src="https://www.amcharts.com/lib/3/pie.js"></script>
    <script src="https://www.amcharts.com/lib/3/plugins/export/export.min.js"></script>
    <link rel="stylesheet" href="https://www.amcharts.com/lib/3/plugins/export/export.css"
        type="text/css" media="all" />
    <script src="https://www.amcharts.com/lib/3/themes/light.js"></script>
    <script src="https://www.amcharts.com/lib/3/serial.js"></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" />
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <link href="Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <link href="Css/RouteWiseTimeLine.css" rel="stylesheet" type="text/css" />
    <link href="Css/AdminLTE.css" rel="stylesheet" type="text/css" />
    <link href="Css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <%-- <script type="text/javascript" src="https://cdn.jsdelivr.net/jquery/latest/jquery.min.js"></script>--%>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/momentjs/latest/moment.min.js"></script>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.css" />
    <style>
        table#tableaProductdetails
        {
            empty-cells: hide;
        }
        .HeaderStyle
        {
            border: solid 1px White;
            background-color: #81BEF7;
            font-weight: bold;
            text-align: center;
        }
    </style>
    <script type="text/javascript">
       window.setInterval(function () {
           $('.blink').toggle();
       }, 250);
       $(function () {
//           $("#leftdiv").css('margin-left', 0);
//           $("#leftdiv").css('margin-right', 0);
//           $("#leftdiv").animate({ left: '-640px' }, 0);
//           $("#rightdiv").css('width', '100%');
           hiddenvalue = true;
           $('#divPlant').css('display', 'block');
           $('#tidcategory').css('display', 'none');
           $('#firstdiv').css('display', 'none');
           daterangepicker();
           datecontrolsession();


           $('#spnDispatchQty').css('display', 'none');
           $('#spnSvdsCompany').css('display', 'none');
           $('#spnSvfCompany').css('display', 'none');
           $('#spnMilkDispatchQty').css('display', 'none');
           $('#spnCurdDispatchQty').css('display', 'none');
           $('#spnOthersDispatchQty').css('display', 'none');
           $('#spnDispatchQty').css('display', 'none');
           $('#spnSvdsCompany').css('display', 'none');
           $('#spnSvfCompany').css('display', 'none');
           $('#spnSaleValue').css('display', 'none');
           $('#spnSaleValue').css('display', 'none');
           $('#spnCollection').css('display', 'none');
           $('#spnDueValue').css('display', 'none');
           $('#example1').css('display', 'none');

       });

       function daterangepicker() {
           var start = moment().subtract(1, 'days');
           var end = moment().subtract(1, 'days');
           function cb(start, end) {
               $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
               datecontrolsession();
           }
           $('#reportrange').daterangepicker({
               startDate: start,
               endDate: end,
               ranges: {
                   'Today': [moment(), moment()],
                   'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')]
                   //           'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                   //           'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                   //           'This Month': [moment().startOf('month'), moment().endOf('month')],
                   //           'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
               }
           }, cb);
           cb(start, end);

       }

       function daterangepicker1() {
           var Day = '<%=Session["Days"] %>';
           var start = moment().subtract(1, 'days');
           var end =  moment().subtract(1, 'days');
           function cb(start, end) {
               $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
               datecontrolsession();
           }
           $('#reportrange').daterangepicker({
               startDate: start,
               endDate: end,
               ranges: {
                   'Today': [moment(), moment()],
                   'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')]
                   //           'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                   //           'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                   //           'This Month': [moment().startOf('month'), moment().endOf('month')],
                   //           'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
               }
           }, cb);
           cb(start, end);
       }
        var TotalProductclass=[];var DaywiseAgenttSale=[];
       function btnAgentWiseBetweenDayProductComparison() {
        //var BranchId;
//        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
//        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
       // var PlantName = document.getElementById('ddlPlant').value;
            $('#divHide').css('display', 'block');
        var data = { 'operation': 'Get_AgentWiseBetweenDayProduct_Comparison' };
        var s = function (msg) {
            if (msg) {
                if (msg == "Session Expired") {
                    alert(msg);
                    window.location = "Login.aspx";
                }
                 TotalProductclass = msg[0].TotalProductclass;
                DaywiseAgenttSale= msg[0].linechartvaluesclass;
                FillAgentProductDayComparisonDetails(DaywiseAgenttSale);
                FillAgentProductDayProductPiechart(TotalProductclass);
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
       function FillAgentProductDayProductPiechart(databind) {
           newXarray = [];
           //$('#div_agentwisemain').css('display', 'block');
       // $('#div_routewisemainCompare').css('display', 'block');
        $('#example').css('display', 'block');
           if (databind.length > 0) {
                   var RouteName = databind[0].CategeoryName;
                   var leakqty = databind[0].Qty;
                   var color = ["#794b4b", "#FF6600", "#fdd400", "#84b761", "#cc4748", "#cd82ad", "#2f4074", "#448e4d", "#b7b83f", "#b9783f", "#b93e3d", "#913167", "#18d79c", "#a2907f", "#41ee80", "#8cfa40", "#dbdf9d", "#3f4fb2", "#01909b", "#124c65", "#33879b", "#869ae2", "#000000"];
                   for (var i = 0; i < databind.length; i++) {
                       newXarray.push({ "category": databind[i].CategeoryName, "value": parseFloat(databind[i].Qty), "color": color[i] });
                   }
               }
               var chart = AmCharts.makeChart("AgentDayProductPieChart", {
                   "type": "pie",
                   "startDuration": 0,
                   "theme": "light",
                   "addClassNames": true,
                   "labelRadius": -35,
                   "labelText": "",
                   "legend": {
                       "align": "center",
                       "position": "bottom",
                       "marginRight": 21,
                       "markerType": "circle",
                       "right": -20,
                       "labelText": "[[title]]",
                       "valueText": "",
                       "valueWidth": 80,
                       "textClickEnabled": true,
                       "labelsEnabled": false,
                       "autoMargins": false,
                       "marginTop": 0,
                       "marginBottom": 0,
                       "marginLeft": 0,
                       "marginRight": 0,
                       "pullOutRadius": 0,
                       fontSize: 9,
                       fontweight: "bold"
                   },
                   "innerRadius": "30%",
                   "defs": {
                       "filter": [{
                           "id": "shadow",
                           "width": "200%",
                           "height": "200%",
                           "feOffset": {
                               "result": "offOut",
                               "in": "SourceAlpha",
                               "dx": 0,
                               "dy": 0
                           },
                           "feGaussianBlur": {
                               "result": "blurOut",
                               "in": "offOut",
                               "stdDeviation": 5
                           },
                           "feBlend": {
                               "in": "SourceGraphic",
                               "in2": "blurOut",
                               "mode": "normal"
                           }
                       }]
                   },
                   "dataProvider": newXarray,
                   "valueField": "value",
                   "colorField": "color",
                   "titleField": "category",
                   "export": {
                       "enabled": true
                   }
               });
           }
        function FillAgentProductDayComparisonDetails(databind) {
            
         //$('#divMainAddNewRow1').css('display', 'block');
        //$('#div_routewisemain').css('display', 'block');
       // $('#div_agentwisemain').css('display', 'block');
       // $('#div_routewisemainCompare').css('display', 'block');
        $('#example').css('display', 'block');
   
        var textname = "";
        var sta = databind[0].ProductName;
        datainXSeries = 0;
        datainYSeries = 0;
        newYarray = [];
        newXarray = [];
        newSalearray = [];
        newDuearray = [];
       textname ="Sale Quantity";
        for (var k = 0; k < databind.length; k++) {
            var BranchName = [];
            var IndentDate = databind[0].IndentDate;
            //                var UnitQty = databind[k].UnitQty;
            var SaleValue = databind[0].dqty;
            newXarray = IndentDate.split(',');
            newSalearray = SaleValue.split(',');
            var titlename   =textname;
            for (var i = 0; i < newXarray.length; i++) {
                newYarray.push({ "data": newSalearray[i], "val": newXarray[i].toString() });
                //                    newYarray.push({ 'sale': DeliveryQty[i].split(','), 'name': Status[i] });
            }
        }
        var chart = AmCharts.makeChart("ProductWiseChart", {
            "type": "serial",
            "theme": "light",
                "titles": [{
            "text": titlename
                }],
            "legend": {
                "useGraphSettings": true
            },
            "dataProvider": newYarray,
            "valueAxes": [{
                "integersOnly": true,
                "reversed": false,
                "axisAlpha": 0,
                "dashLength": 5,
                "gridCount": 10,
            }],
            "startDuration": 0.5,
            "graphs": [
                        {
                            "balloonText": "Actual Sale " +sta+" [[category]]: [[value]]",
                            "bullet": "round",
                            "title": "Sale Value",
                            "valueField": "data",
                            "fillAlphas": 0
                        }], 
            "chartCursor": {
                "cursorAlpha": 0,
                "zoomable": false
            },
            "categoryField": "val",
            "categoryAxis": {
                "gridPosition": "start",
                "axisAlpha": 0,
                "fillAlpha": 0.05,
                "gridAlpha": 0,
                "position": "left",
                "bold": true,
                "labelRotation": 25

            },
            "export": {
                "enabled": true,
//                    "position": "bottom-right"
            }
        });
    }


       function DateClick() {
           datecontrolsession();
       }
       function datecontrolsession() {
           var IndDate = $('#reportrange').data('daterangepicker').startDate.toString();
           var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           var data = { 'operation': 'pickervaluesettosession', 'IndDate': IndDate, 'Todate': Todate };
           var s = function (msg) {
               if (msg) {

               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       // branch wise information
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
       var TotalProductclass = []; var SubTotalProductclass = [];
       function fillProdcutData() {
           //            var IndDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           var PlantName = document.getElementById('ctl00_ContentPlaceHolder1_ddlPlant').value;
           var branchCategory = document.getElementById('ctl00_ContentPlaceHolder1_ddlbarnchCategory').value;
           var data = { 'operation': 'GetProductInformation', 'BranchID': PlantName, 'Type': Type };
           var s = function (msg) {
               if (msg) {
                   document.getElementById('ctl00_ContentPlaceHolder1_firstdiv').style.display = "block";
                   //                    $('#ctl00_ContentPlaceHolder1_firstdiv').css('display', "block;");
                   //                    $('#firstdiv').css('display', 'block ');
                   TotalProductclass = msg[0].TotalProductclass;
                   SubTotalProductclass = msg[0].SubTotalProductclass;
                   fillcategorywisedispatchsale(SubTotalProductclass);

                   fillsubcategorywiseproductsdata(TotalProductclass);
                   ProductInformationpieChart(TotalProductclass);
               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }

       function fillcategorywisedispatchsale(msg) {
           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;
           var ddlbarnchCategory = document.getElementById('ctl00_ContentPlaceHolder1_ddlbarnchCategory').value;
           if (ddlbarnchCategory == "CategoryWiseDespatch") {
               if (ddlDataType == "Quantity") {
                   var MilkQty = msg[0].MilkQty;
                   var CurdQty = msg[0].CurdQty;
                   var OtherQty = msg[0].OtherQty;
                   document.getElementById('hmilkqty').innerHTML = MilkQty + ' Ltrs';
                   document.getElementById('hcurdqty').innerHTML = CurdQty + ' Ltrs';
                   document.getElementById('hothersqty').innerHTML = OtherQty;
                   document.getElementById('spnmilkid').innerHTML = "MilkQty";
                   document.getElementById('spncurdid').innerHTML = "Curd Qty";
                   document.getElementById('spnothersid').innerHTML = "Other Qty";

                   $('#spnDispatchQty').css('display', 'none');
                   $('#spnSvdsCompany').css('display', 'none');
                   $('#spnSvfCompany').css('display', 'none');
                   $('#spnSaleValue').css('display', 'none');
                   $('#spnCollection').css('display', 'none');
                   $('#spnDueValue').css('display', 'none');
                   $('#example1').css('display', 'block');

                   $('#Branch_wise_Dispatch').css('display', 'none');


               }
               else {
                   var MilkValue = msg[0].MilkValue;
                   var CurdValue = msg[0].CurdValue;
                   var OtherValue = msg[0].OthersValue;

                   document.getElementById('hmilkqty').innerHTML = MilkValue;
                   document.getElementById('hcurdqty').innerHTML = CurdValue;
                   document.getElementById('hothersqty').innerHTML = OtherValue;
                   document.getElementById('spnmilkid').innerHTML = "Milk Value";
                   document.getElementById('spncurdid').innerHTML = "Curd Value";
                   document.getElementById('spnothersid').innerHTML = "Other Value";

                   $('#spnDispatchQty').css('display', 'none');
                   $('#spnSvdsCompany').css('display', 'none');
                   $('#spnSvfCompany').css('display', 'none');
                   $('#spnSaleValue').css('display', 'none');
                   $('#spnSaleValue').css('display', 'none');
                   $('#spnCollection').css('display', 'none');
                   $('#spnDueValue').css('display', 'none');
                   $('#example1').css('display', 'block');
                   $('#Branch_wise_Dispatch').css('display', 'none');
               }
           }
           else if (ddlbarnchCategory == "BranchWiseDespatch") {
               if (ddlDataType == "Quantity") {
                   var DispatchQty = msg[0].GroupTotDispQty;
                   document.getElementById('hdispatchqty').innerHTML = DispatchQty + ' Ltrs';
                   $('#spnMilkDispatchQty').css('display', 'none');
                   $('#spnCurdDispatchQty').css('display', 'none');
                   $('#spnOthersDispatchQty').css('display', 'none');

               }
               else {
               }
           }
           else if (ddlbarnchCategory == "BranchWiseCollections") {
               var SaleValue = msg[0].GroupTotSaleValue;
               var collamount = msg[0].GroupTotCollectionValue;
               var dueamount = msg[0].GroupDueAmount;
               document.getElementById('hsalevalue').innerHTML = '&#8377; ' + SaleValue;
               document.getElementById('spnamount').innerHTML = '&#8377; ' + collamount;
               document.getElementById('hduevalue').innerHTML = '&#8377; ' + dueamount;
               $('#spnMilkDispatchQty').css('display', 'none');
               $('#spnCurdDispatchQty').css('display', 'none');
               $('#spnOthersDispatchQty').css('display', 'none');
           }
           else {

           }
       }

       function fillsubcategorywiseproductsdata(msg) {
           var BranchTable = [];
           var temp = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;
           j = 1;
           var pdelivaryqty = 0; var psalevalue = 0;
           if (temp == "Quantity") {
               var results = '<div  style="overflow:auto;"><table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;" id="tableaProductdetails" class="mainText2" border="1">';
               results += '<thead><tr><th style="width: 33%; text-align: center; height: 20px; font-size: 16px; font-weight: bold;color: #2f3293;">Category Name</th><th style="width: 33%; height: 20px; text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">SubCategory Name</th><th style="width: 33%; height: 20px; text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">DelivaryQty</th><th style="width: 33%; height: 20px; text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">Qty(%)</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               var COLOR = ["#794b4b", "#FF6600", "#fdd400", "#84b761", "#cc4748", "#cd82ad", "#2f4074", "#448e4d", "#b7b83f", "#b9783f", "#b93e3d", "#913167", "#18d79c", "#a2907f", "#41ee80", "#8cfa40", "#dbdf9d", "#3f4fb2", "#01909b", "#124c65", "#33879b", "#869ae2"];
               for (var i = 0; i < TotalProductclass.length; i++) {
                   if (BranchTable.indexOf(TotalProductclass[i].CategeoryName) == -1) {
                       results += '<tr >';
                       results += '<td style="width: 33%; heights: 20px; vertical-align: middle; font-size: 16px; text-align: center;"  class="2"><div id="txtAccount" style="font-size: 12px; font-weight: bold;" onclick="btnClickCategoryId(\'' + TotalProductclass[i].CatSno + '\');" class="AccountClass"><b>' + TotalProductclass[i].CategeoryName + '</b></td>';
                       results += '<td style="background-color:' + COLOR[l] + '"  class="2"><div id="txtamount" style="font-size: 14px;color:#ffff;  font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnVariationProductlineChart(\'' + TotalProductclass[i].subCatsno + '\');"  class="subCategeoryClass"><b>' + TotalProductclass[i].subCategeoryName + '<b></td>';
                       results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;"  class="2"><div id="Span1" style="font-size: 14px;  font-weight: bold;" class="QtyClass"><b>' + TotalProductclass[i].Qty + '</b></td>';
                       results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;display:none;""  class="2"><div id="Span1" style="font-size: 14px;  font-weight: bold;" class="QtyClass"><b>' + TotalProductclass[i].SaleValue + '</b></td>';
                       results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;" class="2" ><div style="font-size: 14px;  font-weight: bold;"><b>' + TotalProductclass[i].Nos + '</b></div></td>';
                       results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;display:none;"" class="2" ><div style="font-size: 14px;  font-weight: bold;"><b>' + TotalProductclass[i].Kgs + '</b></div></td></tr>';
                       //                results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;" class="2" ><div style="font-size: 14px;  font-weight: bold;"><b>' + msg[i].Nos + '</b></div></td></tr>';
                       l = l + 1;
                       if (l == 30) {
                           l = 0;
                       }
                       BranchTable.push(TotalProductclass[i].CategeoryName);
                   }
                   else {
                       results += '<tr>';
                       results += '<td style="border:none" class="1" ></td>';
                       results += '<td style="background-color:' + COLOR[l] + '"  class="2"><div id="txtamount" style="font-size: 14px;color:#ffff;  font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnVariationProductlineChart(\'' + TotalProductclass[i].subCatsno + '\');"  class="subCategeoryClass"><b>' + TotalProductclass[i].subCategeoryName + '<b></td>';
                       results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;"  class="2"><div id="Span1" style="font-size: 14px;  font-weight: bold;" class="QtyClass"><b>' + TotalProductclass[i].Qty + '</b></td>';
                       results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;display:none;""  class="2"><div id="Span1" style="font-size: 14px;  font-weight: bold;" class="QtyClass"><b>' + TotalProductclass[i].SaleValue + '</b></td>';
                       results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;" class="2" ><div style="font-size: 14px;  font-weight: bold;"><b>' + TotalProductclass[i].Nos + '</b></div></td>';
                       results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;display:none;"" class="2" ><div style="font-size: 14px;  font-weight: bold;"><b>' + TotalProductclass[i].Kgs + '</b></div></td></tr>';
                       l = l + 1;
                       if (l == 30) {
                           l = 0;
                       }
                   }
               }
           }
           else {
               var results = '<div  style="overflow:auto;"><table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;" id="tableaProductdetails" class="mainText2" border="1">';
               results += '<thead><tr><th style="width: 33%; text-align: center; height: 20px; font-size: 16px; font-weight: bold;color: #2f3293;">Category Name</th><th style="width: 33%; height: 20px; text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">SubCategory Name</th><th style="width: 33%; height: 20px; text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">SaleValue</th><th style="width: 33%; height: 20px; text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">Value(%)</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               var COLOR = ["#794b4b", "#FF6600", "#fdd400", "#84b761", "#cc4748", "#cd82ad", "#2f4074", "#448e4d", "#b7b83f", "#b9783f", "#b93e3d", "#913167", "#18d79c", "#a2907f", "#41ee80", "#8cfa40", "#dbdf9d", "#3f4fb2", "#01909b", "#124c65", "#33879b", "#869ae2"];
               for (var i = 0; i < TotalProductclass.length; i++) {
                   if (BranchTable.indexOf(TotalProductclass[i].CategeoryName) == -1) {
                       results += '<tr >';
                       results += '<td style="width: 33%; heights: 20px; vertical-align: middle; font-size: 16px; text-align: center;"  class="2"><div id="txtAccount" style="font-size: 12px; font-weight: bold;" onclick="btnClickCategoryId(\'' + TotalProductclass[i].CatSno + '\');" class="AccountClass"><b>' + TotalProductclass[i].CategeoryName + '</b></td>';
                       results += '<td style="background-color:' + COLOR[l] + '"  class="2"><div id="txtamount" style="font-size: 14px;color:#ffff;  font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnVariationProductlineChart(\'' + TotalProductclass[i].subCatsno + '\');"  class="subCategeoryClass"><b>' + TotalProductclass[i].subCategeoryName + '<b></td>';
                       results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;display:none;""  class="2"><div id="Span1" style="font-size: 14px;  font-weight: bold;" class="QtyClass"><b>' + TotalProductclass[i].Qty + '</b></td>';
                       results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;"  class="2"><div id="Span1" style="font-size: 14px;  font-weight: bold;" class="QtyClass"><b>' + TotalProductclass[i].SaleValue + '</b></td>';
                       results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;display:none;"" class="2" ><div style="font-size: 14px;  font-weight: bold;"><b>' + TotalProductclass[i].Nos + '</b></div></td>';
                       results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;" class="2" ><div style="font-size: 14px;  font-weight: bold;"><b>' + TotalProductclass[i].Kgs + '</b></div></td></tr>';
                       //                results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;" class="2" ><div style="font-size: 14px;  font-weight: bold;"><b>' + msg[i].Nos + '</b></div></td></tr>';
                       l = l + 1;
                       if (l == 30) {
                           l = 0;
                       }
                       BranchTable.push(TotalProductclass[i].CategeoryName);
                   }
                   else {
                       results += '<tr>';
                       results += '<td style="border:none" class="1" ></td>';
                       results += '<td style="background-color:' + COLOR[l] + '"  class="2"><div id="txtamount" style="font-size: 14px;color:#ffff;  font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnVariationProductlineChart(\'' + TotalProductclass[i].subCatsno + '\');"  class="subCategeoryClass"><b>' + TotalProductclass[i].subCategeoryName + '<b></td>';
                       results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;display:none;""  class="2"><div id="Span1" style="font-size: 14px;  font-weight: bold; class="QtyClass"><b>' + TotalProductclass[i].Qty + '</b></td>';
                       results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;"  class="2"><div id="Span1" style="font-size: 14px;  font-weight: bold;" class="QtyClass"><b>' + TotalProductclass[i].SaleValue + '</b></td>';
                       results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;display:none;"" class="2" ><div style="font-size: 14px;  font-weight: bold;"><b>' + TotalProductclass[i].Nos + '</b></div></td>';
                       results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;" class="2" ><div style="font-size: 14px;  font-weight: bold;"><b>' + TotalProductclass[i].Kgs + '</b></div></td></tr>';
                       l = l + 1;
                       if (l == 30) {
                           l = 0;
                       }
                   }
               }
           }
           //                var  Qty = parseFloat(msg[0].TotQty / 1000).toFixed(2);
           //                
           results += '</table></div>';
           $("#tableProductData").html(results);
       }

       function btnVariationProductlineChart(subCatsno) {
           var subCatsno;
           //            var Fromdate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'getLineChartforsubcategeoryReport', 'SubcatSno': subCatsno };
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
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       function LineChartforsubcategeory(msg) {
           scrollTo(0, 0);
           $('html, body').animate({
               scrollTop: $("#divMainAddNewRow1").offset().top
           }, 2000);
           $('#divMainAddNewRow1').focus();
           $('#divMainAddNewRow1').css('display', 'block');
           j = 1;
           var pdelivaryqty = 0; var psalevalue = 0;
           var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
           results += '<thead><tr style="background-color: #abbed2;"><th scope="col">Branch Name</th><th scope="col">Delivary Qty</th></tr></thead></tbody>';
           var k = 1;
           var l = 0;
           var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
           for (var i = 0; i < msg.length; i++) {
               var Deliverqty = parseFloat(msg[i].dispatchqty).toFixed(2) || 0;
               results += '<tr style="background-color: antiquewhite;">';
               results += '<td scope="row"   class="2" onclick="btnVariationProductlineChart(\'' + msg[i].BranchID + '\');" >' + msg[i].BranchName + '</th>';
               results += '<td scope="row" class="2" ><div style="float:right;padding-right: 10%;">' + Deliverqty + '</div></td></tr>';
               l = l + 1;
               if (l == 4) {
                   l = 0;
               }
               pdelivaryqty += parseFloat(msg[i].dispatchqty) || 0;
           }
           results += '<tr style="background-color: antiquewhite;">';
           results += '<td scope="row" class="1">Total</td>';
           results += '<td scope="row" class="1" style="font-size:18px;font-weight:bold;color:#006400;" ><div style="float:right;padding-right: 7%;">' + parseFloat(pdelivaryqty).toFixed(2) + '</div></td></tr>';
           results += '</table></div>';
           $("#divChart").html(results);
       }

       var newXarray = [];
       function ProductInformationpieChart(databind) {
           newXarray = [];
           $("#Div1").empty();
           $("#Div2").empty();
           var myTableDiv = document.getElementById("Div1");
           var divproductleakpie = document.createElement("div");
           divproductleakpie.style.height = "450px";
           divproductleakpie.setAttribute("id", "id_you_like");
           myTableDiv.appendChild(divproductleakpie);
           var type = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;
           if (databind.length > 0) {
               if (type == "Quantity") {
                   var RouteName = databind[0].subCategeoryName;
                   var leakqty = databind[0].Qty;

                   var color = ["#794b4b", "#FF6600", "#fdd400", "#84b761", "#cc4748", "#cd82ad", "#2f4074", "#448e4d", "#b7b83f", "#b9783f", "#b93e3d", "#913167", "#18d79c", "#a2907f", "#41ee80", "#8cfa40", "#dbdf9d", "#3f4fb2", "#01909b", "#124c65", "#33879b", "#869ae2", "#000000"];

                   for (var i = 0; i < databind.length; i++) {
                       newXarray.push({ "category": databind[i].subCategeoryName, "value": parseFloat(databind[i].Qty), "color": color[i] });
                   }
               }
               else {
                   var RouteName = databind[0].subCategeoryName;
                   var leakqty = databind[0].SaleValue;
                   var color = ["#794b4b", "#FF6600", "#fdd400", "#84b761", "#cc4748", "#cd82ad", "#2f4074", "#448e4d", "#b7b83f", "#b9783f", "#b93e3d", "#913167", "#18d79c", "#a2907f", "#41ee80", "#8cfa40", "#dbdf9d", "#3f4fb2", "#01909b", "#124c65", "#33879b", "#869ae2", "#000000"];
                   for (var i = 0; i < databind.length; i++) {
                       newXarray.push({ "category": databind[i].subCategeoryName, "value": parseFloat(databind[i].SaleValue), "color": color[i] });

                   }
               }
               var chart = AmCharts.makeChart("id_you_like", {
                   "type": "pie",
                   "startDuration": 0,
                   "theme": "light",
                   "addClassNames": true,
                   "labelRadius": -35,
                   "labelText": "",
                   "legend": {
                       "align": "center",
                       "position": "bottom",
                       "marginRight": 21,
                       "markerType": "circle",
                       "right": -20,
                       "labelText": "[[title]]",
                       "valueText": "",
                       "valueWidth": 80,
                       "textClickEnabled": true,
                       "labelsEnabled": false,
                       "autoMargins": false,
                       "marginTop": 0,
                       "marginBottom": 0,
                       "marginLeft": 0,
                       "marginRight": 0,
                       "pullOutRadius": 0,
                       fontSize: 9,
                       fontweight: "bold"
                   },
                   "innerRadius": "30%",
                   "defs": {
                       "filter": [{
                           "id": "shadow",
                           "width": "200%",
                           "height": "200%",
                           "feOffset": {
                               "result": "offOut",
                               "in": "SourceAlpha",
                               "dx": 0,
                               "dy": 0
                           },
                           "feGaussianBlur": {
                               "result": "blurOut",
                               "in": "offOut",
                               "stdDeviation": 5
                           },
                           "feBlend": {
                               "in": "SourceGraphic",
                               "in2": "blurOut",
                               "mode": "normal"
                           }
                       }]
                   },
                   "dataProvider": newXarray,
                   "valueField": "value",
                   "colorField": "color",
                   "titleField": "category",
                   "export": {
                       "enabled": true
                   }
               });
           }
       }



       function btnClickCategoryId(branchname) {
           var branchname;
           //        var startDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //        var endDate = $('#reportrange').data('daterangepicker').endDate.toString();

           var PlantName = document.getElementById('ctl00_ContentPlaceHolder1_ddlPlant').value;
           var branchCategory = document.getElementById('ctl00_ContentPlaceHolder1_ddlbarnchCategory').value;
           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;
           var data = { 'operation': 'GetCategoryWiseChart', 'branchname': branchname, 'ddlDataType': ddlDataType };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location.assign("Login.aspx");
                   }
                   else {
                       if (msg.length > 0) {
                           createCateGorypieChart(msg);
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
       var newXarray = [];
       var tbldata = [];
       function createCateGorypieChart(databind) {
           $('#divMainAddNewRow1').css('display', 'block');
           $('#divHide').css('display', 'block');
           newXarray = [];
           var textname = "";
           //var totalqty = databind[0].totalqty;
           var Amount = databind[0].Amount;
           var DeliveryQty = databind[0].DeliveryQty;
           var AverageyQty = databind[0].AverageyQty;
           var RouteName = databind[0].RouteName;

           tbldata = databind[0].PieValueTableClass

           var catidandBranchid = databind[0].catidandBranchid;
           //            var myTableDiv = document.getElementById("example");
           //            var divleakbar = document.createElement("div");
           //            divleakbar.style.height = "300px";
           //            divleakbar.setAttribute("id", "id_you_like");
           //            myTableDiv.appendChild(divleakbar);

           //            var divleakbar1 = document.createElement("div");
           //            divleakbar1.style.height = "300px";
           //            divleakbar1.setAttribute("id", "id_table_data");
           //            myTableDiv.appendChild(divleakbar1);
           var color = ["#FF0F00", "#FF6600", "#FF9E01", "#FCD202", "#F8FF01", "#F8FF01", "#B0DE09", "#04D215", "#0D8ECF", "#40ff00", "#00ff40", " #00ff80", " #00ffff", " #0080ff", "#0D52D1", "#2A0CD0", "#8A0CCF", "#CD0D74", "#754DEB", "#DDDDDD", "#999999", "#333333", "#000000"];
           for (var i = 0; i < RouteName.length; i++) {
               newXarray.push({ "category": RouteName[i], "value": parseFloat(Amount[i]), "catidandBranchid": catidandBranchid[i], "color": color[i] });
           }
           var chart = AmCharts.makeChart("divChart", {
               "type": "pie",
               "startDuration": 0,
               "theme": "light",
               "addClassNames": true,
               "labelRadius": -35,
               "labelText": "",
               "legend": {
                   "align": "right",
                   "position": "bottom",
                   "marginRight": 21,
                   "markerType": "circle",
                   "right": -20,
                   "labelText": "[[title]]",
                   "valueText": "[[percents]]",
                   //"useGraphSettings": true,
                   "valueWidth": 80,
                   "textClickEnabled": true,
                   "labelsEnabled": true,
                   "autoMargins": false,
                   "marginTop": 0,
                   "marginBottom": 0,
                   "marginLeft": 0,
                   "marginRight": 0,
                   "pullOutRadius": 0,
                   fontSize: 9,
                   fontweight: "bold"
               },
               "innerRadius": "30%",
               "defs": {
                   "filter": [{
                       "id": "shadow",
                       "width": "200%",
                       "height": "200%",
                       "feOffset": {
                           "result": "offOut",
                           "in": "SourceAlpha",
                           "dx": 0,
                           "dy": 0
                       },
                       "feGaussianBlur": {
                           "result": "blurOut",
                           "in": "offOut",
                           "stdDeviation": 5
                       },
                       "feBlend": {
                           "in": "SourceGraphic",
                           "in2": "blurOut",
                           "mode": "normal"
                       }
                   }]
               },
               "dataProvider": newXarray,
               "valueField": "value",
               "colorField": "color",
               "titleField": "category",
               "export": {
                   "enabled": true
               },
               "listeners": [{
                   "event": "clickSlice",
                   "method": function (e) {
                       var dp = e.dataItem.dataContext.catidandBranchid;
                       btnClickCategoryId(dp);
                   }
               }]
           });
       }







       function branchwisemilkdetails() {
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           //            var PlantName = document.getElementById('ddlPlant').value;
           //            var ddlDataType = document.getElementById('ddlDataType').value;
           var PlantName = document.getElementById('ctl00_ContentPlaceHolder1_ddlPlant').value;
           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'branchwise_Dispatch_milk_qty', 'BranchId': PlantName, 'ddlDataType': ddlDataType };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   if (msg[0].type == "GroupWise" || msg[0].type == "CompanyWise") {
                       fillPlant_Dispatch_MilkQty(msg);
                   }
                   else {
                       fillbranchwise_Milk_dispqty(msg);
                   }
                   //                    fillbranchwisesalevalue(msg);
               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       function fillbranchwise_Milk_dispqty(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           j = 1;


           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;


           // var ddlDataType = document.getElementById('ddlDataType').value;
           if (ddlDataType == "Quantity") {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th >Milk Qty (ltrs)</th><th>Milk AvgQty (ltrs)</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr  style="background-color: antiquewhite;">';
                   results += '<td   class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnRoute_Wise_Milk_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgQty + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           else {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th >Milk Value</th><th>Milk AvgValue</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr  style="background-color: antiquewhite;">';
                   results += '<td   class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnRoute_Wise_Milk_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchvalue + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgSaleValue + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           results += '</table></div>';
           $("#divChart").html(results);
       }
       function fillPlant_Dispatch_MilkQty(msg) {
           $('#div_MainPlantDetails').css('display', 'block');
           j = 1;
           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;
           if (ddlDataType == "Quantity") {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th >Dispatch Milk Qty (ltrs)</th><th >Dispatch Milk AvgQty (ltrs)</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnGroupUnder_PlantClick_Branch_Wise_DispatchMilQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgQty + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           else {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th >Dispatch Milk Value</th><th >Dispatch Milk AvgValue</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnGroupUnder_PlantClick_Branch_Wise_DispatchMilQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchvalue + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgSaleValue + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           results += '</table></div>';
           $("#div_PlantDetails").html(results);
       }

       function btnGroupUnder_PlantClick_Branch_Wise_DispatchMilQty(BranchId) {
           var BranchId;
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           //            var PlantName = document.getElementById('ddlPlant').value;
           //            var ddlDataType = document.getElementById("ddlDataType").value;

           var PlantName = document.getElementById('ctl00_ContentPlaceHolder1_ddlPlant').value;
           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;


           $('#divHide').css('display', 'block');

           var data = { 'operation': 'branchwise_Dispatch_milk_qty', 'BranchId': BranchId, 'ddlDataType': ddlDataType };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }

                   fillbranchwise_Milk_dispqty(msg);
                   $('#div_MainPlantDetails').css('display', 'none');
               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }



       function btnRoute_Wise_Milk_SaleQty(BranchId) {
           var BranchId;
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           //            var ddlDataType = document.getElementById("ddlDataType").value;
           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'Route_Wise_Milk_SaleQty', 'BranchId': BranchId, 'ddlDataType': ddlDataType };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   fillRouteWise_MilkSale_Qty(msg);

               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       function fillRouteWise_MilkSale_Qty(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           $('#div_routewisemain').css('display', 'block');
           j = 1;

           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;


           //var ddlDataType = document.getElementById("ddlDataType").value;
           if (ddlDataType == "Quantity") {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th>Route Name</th><th>Milk Sale Qty</th><th>Milk Sale AvgQty</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td   class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnAgent_Wise_Milk_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgQty + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           else {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th>Route Name</th><th>Milk Sale Value</th><th>Milk Sale AvgValue</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td   class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnAgent_Wise_Milk_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchvalue + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgSaleValue + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           results += '</table></div>';
           $("#div_routewise").html(results);
       }
       function btnAgent_Wise_Milk_SaleQty(BranchId) {
           var BranchId;
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           //            var ddlDataType = document.getElementById("ddlDataType").value;
           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'Agent_Wise_Milk_SaleQty', 'BranchId': BranchId, 'ddlDataType': ddlDataType };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   fillAgentWise_MIlkSale_Qty(msg);

               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       function fillAgentWise_MIlkSale_Qty(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           $('#div_routewisemain').css('display', 'block');
           $('#div_agentwisemain').css('display', 'block');
           j = 1;


           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;


           //            var ddlDataType = document.getElementById("ddlDataType").value;
           if (ddlDataType == "Quantity") {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Agent Name</th><th>Milk Sale Qty</th><th>Milk Sale AvgQty</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td   class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"    class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgQty + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           else {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Agent Name</th><th>Milk Sale Value</th><th>Milk Sale AvgValue</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td   class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"    class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchvalue + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgSaleValue + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           results += '</table></div>';
           $("#div_agentwise").html(results);
       }
       function branchwiseCurddetails() {
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           //            var PlantName = document.getElementById('ddlPlant').value;
           //            var ddlDataType = document.getElementById("ddlDataType").value;

           var PlantName = document.getElementById('ctl00_ContentPlaceHolder1_ddlPlant').value;
           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;


           $('#divHide').css('display', 'block');
           var data = { 'operation': 'branchwise_Curd_Dispatch_qty', 'BranchId': PlantName, 'ddlDataType': ddlDataType };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   if (msg[0].type == "GroupWise" || msg[0].type == "CompanyWise") {
                       fillPlant_Dispatch_CurdQty(msg);
                   }
                   else {
                       fillbranchwise_Curd_dispqty(msg);
                   }
                   //                    fillbranchwisesalevalue(msg);
               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       function fillPlant_Dispatch_CurdQty(msg) {
           $('#div_MainPlantDetails').css('display', 'block');
           j = 1;
           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;
           //            var ddlDataType = document.getElementById("ddlDataType").value;
           if (ddlDataType == "Quantity") {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th >Sale Qty</th><th >Sale AvgQty</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnGroupUnder_PlantClick_Branch_Wise_DispatchCurdQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgQty + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           else {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th >Sale Value</th><th >Sale AvgValue</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnGroupUnder_PlantClick_Branch_Wise_DispatchCurdQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchvalue + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgSaleValue + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           results += '</table></div>';
           $("#div_PlantDetails").html(results);
       }

       function fillbranchwise_Curd_dispqty(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           j = 1;


           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;
           //            var ddlDataType = document.getElementById("ddlDataType").value;
           if (ddlDataType == "Quantity") {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th >Curd Dispatch Qty</th><th >Curd Dispatch AvgQty</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td   class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnRoute_Wise_Curd_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgQty + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           else {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th >Curd Dispatch Value</th><th >Curd Dispatch AvgValue</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td   class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnRoute_Wise_Curd_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchvalue + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgSaleValue + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           results += '</table></div>';
           $("#divChart").html(results);
       }

       function btnGroupUnder_PlantClick_Branch_Wise_DispatchCurdQty(BranchId) {
           var BranchId;
           var PlantName = document.getElementById('ctl00_ContentPlaceHolder1_ddlPlant').value;
           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;
           $('#divHide').css('display', 'block');

           var data = { 'operation': 'branchwise_Curd_Dispatch_qty', 'BranchId': BranchId, 'ddlDataType': ddlDataType };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   fillbranchwise_Curd_dispqty(msg);
                   $('#div_MainPlantDetails').css('display', 'none');
               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }



       function btnRoute_Wise_Curd_SaleQty(BranchId) {
           var BranchId;
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           //            var ddlDataType = document.getElementById("ddlDataType").value;


           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;

           $('#divHide').css('display', 'block');
           var data = { 'operation': 'Route_Wise_Curd_SaleQty', 'BranchId': BranchId, 'ddlDataType': ddlDataType };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   fillRouteWise_CurdSale_Qty(msg);
               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       function fillRouteWise_CurdSale_Qty(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           $('#div_routewisemain').css('display', 'block');
           j = 1;

           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;


           //            var ddlDataType = document.getElementById("ddlDataType").value;
           if (ddlDataType == "Quantity") {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Route Name</th><th>Curd Sale Qty</th><th>Curd Sale AvgQty</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnAgent_Wise_Curd_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgQty + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           else {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Route Name</th><th >Curd Sale Value</th><th>Curd Sale AvgValue</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnAgent_Wise_Curd_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchvalue + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgSaleValue + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           results += '</table></div>';
           $("#div_routewise").html(results);
       }
       function btnAgent_Wise_Curd_SaleQty(BranchId) {
           var BranchId;
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           //            var ddlDataType = document.getElementById("ddlDataType").value;
           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;


           $('#divHide').css('display', 'block');
           var data = { 'operation': 'Agent_Wise_Curd_SaleQty', 'BranchId': BranchId, 'ddlDataType': ddlDataType };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   fillAgentWise_CurdSale_Qty(msg);

               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       function fillAgentWise_CurdSale_Qty(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           $('#div_routewisemain').css('display', 'block');
           $('#div_agentwisemain').css('display', 'block');
           j = 1;
           //            var ddlDataType = document.getElementById("ddlDataType").value;
           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;


           if (ddlDataType == "Quantity") {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Agent Name</th><th >Curd Sale Qty</th><th >Curd Sale AvgQty</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td   class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"    class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgQty + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           else {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Agent Name</th><th >Curd Sale Value</th><th >Curd Sale AvgValue</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td   class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"    class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchvalue + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgSaleValue + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           results += '</table></div>';
           $("#div_agentwise").html(results);
       }
       function branchwiseotherdetails() {
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           //            var PlantName = document.getElementById('ddlPlant').value;
           //            var ddlDataType = document.getElementById("ddlDataType").value;

           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;
           var PlantName = document.getElementById('ctl00_ContentPlaceHolder1_ddlPlant').value;


           $('#divHide').css('display', 'block');
           var data = { 'operation': 'branchwise_Others_Dispatch_qty', 'BranchId': PlantName, 'ddlDataType': ddlDataType };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   if (msg[0].type == "GroupWise" || msg[0].type == "CompanyWise") {
                       fillPlant_Dispatch_OthersQty(msg);
                   }
                   else {
                       fillbranchwise_Others_dispqty(msg);
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
       function fillPlant_Dispatch_OthersQty(msg) {
           $('#div_MainGroupOthersProducts').css('display', 'block');
           j = 1;
           //            var ddlDataType = document.getElementById("ddlDataType").value;
           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;


           if (ddlDataType == "Quantity") {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Product Name</th><th>Sale Qty</th><th>Sale AvgQty</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnGroupUnder_PlantClick_Branch_Wise_DispatchOthersQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgQty + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           else {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th>Product Name</th><th >Sale Value</th><th>Sale AvgValue</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnGroupUnder_PlantClick_Branch_Wise_DispatchOthersQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchvalue + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgSaleValue + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           results += '</table></div>';
           $("#div_GroupOthersProducts").html(results);
       }
       function fillbranchwise_Others_dispqty(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           j = 1;
           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;

           //var ddlDataType = document.getElementById("ddlDataType").value;
           if (ddlDataType == "Quantity") {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th>Other Dispatch Qty</th><th>Other Dispatch AvgQty</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnPlantClick(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgQty + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           else {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th>Other Dispatch Value</th><th>Other Dispatch AvgValue</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnPlantClick(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchvalue + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgSaleValue + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           results += '</table></div>';
           $("#divChart").html(results);
       }

       function btnGroupUnder_PlantClick_Branch_Wise_DispatchOthersQty(BranchId) {
           var BranchId;
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           //            var PlantName = document.getElementById('ddlPlant').value;
           //            var ddlDataType = document.getElementById("ddlDataType").value;

           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;
           var PlantName = document.getElementById('ctl00_ContentPlaceHolder1_ddlPlant').value;
           var temp = "Type";
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'branchwise_Others_Dispatch_qty', 'BranchId': BranchId, 'ddlDataType': ddlDataType, 'temp': temp };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   fillbranchwise_Others_dispqty(msg);
               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }


       //Extra Code Begin
       function branchwiseotherdetails1() {
                       var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
                       var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           //            var PlantName = document.getElementById('ddlPlant').value;
           //            var ddlDataType = document.getElementById("ddlDataType").value;

           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;
           var PlantName = document.getElementById('ctl00_ContentPlaceHolder1_ddlPlant').value;

           var type = "ProductWise";
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'branchwise_Others_Dispatch_qty1', 'BranchId': PlantName, 'ddlDataType': ddlDataType, 'type': type,'IndDate':FromDate,'Todate':Todate };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   if (msg[0].type == "GroupWise" || msg[0].type == "CompanyWise") {
                       fillPlant_Dispatch_OthersQty1(msg);
                   }
                   else {
                       fillbranchwise_Others_dispqty1(msg);
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
       function fillPlant_Dispatch_OthersQty1(msg) {
           $('#div_MainOthersCategory').css('display', 'block');
           j = 1;
           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;
           if (ddlDataType == "Quantity") {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Category Name</th><th>Sale Qty</th><th>Sale AvgQty</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnGroupUnder_PlantClick_Branch_Wise_DispatchOthersQty1(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgQty + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           else {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th>Category Name</th><th >Sale Value</th><th>Sale AvgValue</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnGroupUnder_PlantClick_Branch_Wise_DispatchOthersQty1(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchvalue + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgSaleValue + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           results += '</table></div>';
           $("#div_OthersCategory").html(results);
       }
       function fillbranchwise_Others_dispqty1(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           j = 1;
           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;

           //var ddlDataType = document.getElementById("ddlDataType").value;
           if (ddlDataType == "Quantity") {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th>Other Dispatch Qty</th><th>Other Dispatch AvgQty</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnRoute_Wise_Others_SaleQty1(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgQty + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           else {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th>Other Dispatch Value</th><th>Other Dispatch AvgValue</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnRoute_Wise_Others_SaleQty1(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchvalue + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgSaleValue + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           results += '</table></div>';
           $("#divChart").html(results);
       }

       function btnGroupUnder_PlantClick_Branch_Wise_DispatchOthersQty1(BranchId) {
           var BranchId;
                       var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
                       var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           //            var PlantName = document.getElementById('ddlPlant').value;
           //            var ddlDataType = document.getElementById("ddlDataType").value;

           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;
           var PlantName = document.getElementById('ctl00_ContentPlaceHolder1_ddlPlant').value;
           var temp = "ProductWise";
           var temp1 = "catType";
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'branchwise_Others_Dispatch_qty1', 'BranchId': BranchId, 'ddlDataType': ddlDataType, 'temp': temp, 'temp1': temp1, 'IndDate': FromDate, 'Todate': Todate };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   //                    if (msg[0].temp == "ProducWise") {
                   //                        branchwiseotherdetails();
                   //                    }
                   //                    else {
                   if (msg[0].type == "GroupWise" || msg[0].type == "CompanyWise") {
                       fillPlant_Dispatch_OthersQty(msg);
                   }
                   else {
                       fillbranchwise_Others_dispqty(msg);
                   }
                   //                    }
               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }


       //need to Add begin



       function btnPlantClick(Branchid) {
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           //            var PlantName = document.getElementById('ddlPlant').value;
           //            var ddlDataType = document.getElementById("ddlDataType").value;

           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;
           var PlantName = Branchid;


           $('#divHide').css('display', 'block');
           var data = { 'operation': 'branchwise_Others_Dispatch_qty', 'BranchId': PlantName, 'ddlDataType': ddlDataType };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   fillBranchothersqty(msg);

               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       function fillBranchothersqty(msg) {
           $('#div_MainPlantDetails').css('display', 'block');
           j = 1;
           //            var ddlDataType = document.getElementById("ddlDataType").value;
           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;


           if (ddlDataType == "Quantity") {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th>Sale Qty</th><th>Sale AvgQty</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnRoute_Wise_Others_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgQty + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           else {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th >Sale Value</th><th>Sale AvgValue</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnRoute_Wise_Others_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchvalue + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgSaleValue + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           results += '</table></div>';
           $("#div_PlantDetails").html(results);
       }
       //end




       //ENd











       function btnRoute_Wise_Others_SaleQty(BranchId) {
           var BranchId;
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           //var ddlDataType = document.getElementById("ddlDataType").value;

           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;


           $('#divHide').css('display', 'block');
           var data = { 'operation': 'Route_Wise_Other_SaleQty', 'BranchId': BranchId, 'ddlDataType': ddlDataType };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   fillRouteWise_OtherSale_Qty(msg);
               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       function fillRouteWise_OtherSale_Qty(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           $('#div_routewisemain').css('display', 'block');
           j = 1;
           //var ddlDataType = document.getElementById("ddlDataType").value;

           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;


           if (ddlDataType == "Quantity") {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Route Name</th><th>Other Sale Qty</th><th>Other Sale AvgQty</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td   class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnAgent_Wise_Other_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgQty + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           else {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Route Name</th><th >Other Sale Value</th><th>Other Sale AvgValue</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td   class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnAgent_Wise_Other_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchvalue + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgSaleValue + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           results += '</table></div>';
           $("#div_routewise").html(results);
       }
       function btnAgent_Wise_Other_SaleQty(BranchId) {
           var BranchId;
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           $('#divHide').css('display', 'block');
           //var ddlDataType = document.getElementById("ddlDataType").value;

           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;


           var data = { 'operation': 'Agent_Wise_Other_SaleQty', 'BranchId': BranchId, 'ddlDataType': ddlDataType };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   fillAgentWise_OtherSale_Qty(msg);

               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       function fillAgentWise_OtherSale_Qty(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           $('#div_routewisemain').css('display', 'block');
           $('#div_agentwisemain').css('display', 'block');
           j = 1;
           var ddlDataType = document.getElementById('ctl00_ContentPlaceHolder1_ddlDataType').value;

           // var ddlDataType = document.getElementById("ddlDataType").value;
           if (ddlDataType == "Quantity") {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Agent Name</th><th >Other Sale Qty</th><th>Other Sale AvgQty</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"    class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgQty + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           else {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Agent Name</th><th >Other Sale Value</th><th>Other Sale AvgValue</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"    class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchvalue + '</b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgSaleValue + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
           }
           results += '</table></div>';
           $("#div_agentwise").html(results);
       }
       function branchwisesaledetails() {
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           var PlantName = document.getElementById('ctl00_ContentPlaceHolder1_ddlPlant').value;
           //var PlantName = document.getElementById('ddlPlant').value;
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'branchwise_SaleValue', 'BranchId': PlantName };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   if (msg[0].type == "GroupWise" || msg[0].type == "CompanyWise") {
                       fillPlant_Dispatch_SaleValue(msg);
                   }
                   else {
                       fillbranchwise_Sale_Value(msg);
                   }
                   //                    fillbranchwisesalevalue(msg);
               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       function fillPlant_Dispatch_SaleValue(msg) {
           $('#div_MainPlantDetails').css('display', 'block');
           j = 1;
           var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
           results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th >Sale Value</th></tr></thead></tbody>';
           var k = 1;
           var l = 0;
           for (var i = 0; i < msg.length; i++) {
               results += '<tr style="background-color: antiquewhite;">';
               results += '<td class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnGroupUnder_PlantClick_Branch_Wise_DispatchSaleValue(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
               results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
               l = l + 1;
               if (l == 30) {
                   l = 0;
               }
           }
           results += '</table></div>';
           $("#div_PlantDetails").html(results);
       }
       function fillbranchwise_Sale_Value(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           j = 1;
           var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
           results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th >Sale Value</th></tr></thead></tbody>';
           var k = 1;
           var l = 0;
           for (var i = 0; i < msg.length; i++) {
               results += '<tr style="background-color: antiquewhite;">';
               results += '<td class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnRoute_Wise_SaleVaue(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
               results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
               l = l + 1;
               if (l == 30) {
                   l = 0;
               }
           }
           results += '</table></div>';
           $("#divChart").html(results);
       }
       function btnGroupUnder_PlantClick_Branch_Wise_DispatchSaleValue(BranchId) {
           var BranchId;
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           //            var PlantName = document.getElementById('ddlPlant').value;
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'branchwise_SaleValue', 'Branch_Id': BranchId };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }

                   fillbranchwise_Sale_Value(msg);
                   $('#div_MainPlantDetails').css('display', 'none');
               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }

       function btnRoute_Wise_SaleVaue(BranchId) {
           var BranchId;
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'Route_Wise_SaleValue', 'BranchId': BranchId };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   fillRouteWise_Sale_Value(msg);

               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       function fillRouteWise_Sale_Value(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           $('#div_routewisemain').css('display', 'block');
           j = 1;
           var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
           results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th>Sale Value</th></tr></thead></tbody>';
           var k = 1;
           var l = 0;
           for (var i = 0; i < msg.length; i++) {
               results += '<tr style="background-color: antiquewhite;">';
               results += '<td class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnAgent_Wise_SaleVaue(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
               results += '<td   class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
               l = l + 1;
               if (l == 30) {
                   l = 0;
               }
           }
           results += '</table></div>';
           $("#div_routewise").html(results);
       }
       function btnAgent_Wise_SaleVaue(BranchId) {
           var BranchId;
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'Agent_Wise_SaleValue', 'BranchId': BranchId };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   fillAgentWise_Sale_Value(msg);

               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       function fillAgentWise_Sale_Value(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           $('#div_routewisemain').css('display', 'block');
           $('#div_agentwisemain').css('display', 'block');
           j = 1;
           var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
           results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th >Sale Value</th></tr></thead></tbody>';
           var k = 1;
           var l = 0;
           for (var i = 0; i < msg.length; i++) {
               results += '<tr style="background-color: antiquewhite;">';
               results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"    class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
               results += '<td class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
               l = l + 1;
               if (l == 30) {
                   l = 0;
               }
           }
           results += '</table></div>';
           $("#div_agentwise").html(results);
       }

       function BranchWiseCollection() {
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           //            var BranchId = document.getElementById('ddlPlant').value;
           var BranchId = document.getElementById('ctl00_ContentPlaceHolder1_ddlPlant').value;
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'Branch_Wise_Collection', 'BranchId': BranchId };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   if (msg[0].type == "GroupWise" || msg[0].type == "CompanyWise") {
                       fillPlantUnder_Branch_Wise_Collections(msg);
                   }
                   else {
                       fill_Branch_Wise_Collections(msg);
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

       function fillPlantUnder_Branch_Wise_Collections(msg) {
           $('#div_MainPlantDetails').css('display', 'block');
           j = 1;
           var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
           results += '<thead><tr style="background-color: #abbed2;"><th >Plant Name</th><th >Collection Amount</th></tr></thead></tbody>';
           var k = 1;
           var l = 0;
           for (var i = 0; i < msg.length; i++) {
               results += '<tr style="background-color: antiquewhite;">';
               results += '<td class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnGroupUnder_PlantClick_Branch_Wise_Collection(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
               results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
               l = l + 1;
               if (l == 30) {
                   l = 0;
               }
           }
           results += '</table></div>';
           $("#div_PlantDetails").html(results);
       }

       function fill_Branch_Wise_Collections(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           j = 1;
           var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
           results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th ">Collection Amount</th></tr></thead></tbody>';
           var k = 1;
           var l = 0;
           for (var i = 0; i < msg.length; i++) {
               results += '<tr style="background-color: antiquewhite;">';
               results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnRoute_Wise_Collection_Click(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
               results += '<td class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
               l = l + 1;
               if (l == 30) {
                   l = 0;
               }
           }
           results += '</table></div>';
           $("#divChart").html(results);
       }
       function btnGroupUnder_PlantClick_Branch_Wise_Collection(BranchId) {
           var BranchId;
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           //            var PlantName = document.getElementById('ddlPlant').value;
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'Branch_Wise_Collection', 'BranchId': BranchId };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }

                   fill_Branch_Wise_Collections(msg);

               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }

       function btnRoute_Wise_Collection_Click(BranchId) {
           var BranchId;
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'Route_Wise_Collection', 'BranchId': BranchId };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   fill_Route_Wise_Collections(msg);
               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       function fill_Route_Wise_Collections(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           $('#div_routewisemain').css('display', 'block');
           j = 1;
           var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
           results += '<thead><tr style="background-color: #abbed2;"><th >Route Name</th><th >Collction Amount</th></tr></thead></tbody>';
           var k = 1;
           var l = 0;
           for (var i = 0; i < msg.length; i++) {
               results += '<tr style="background-color: antiquewhite;">';
               results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;"  onclick="btnAgent_Wise_Collection_Click(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
               results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
               l = l + 1;
               if (l == 30) {
                   l = 0;
               }
           }
           results += '</table></div>';
           $("#div_routewise").html(results);
       }
       function btnAgent_Wise_Collection_Click(BranchId) {
           var BranchId;
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'Agent_Wise_Collection', 'BranchId': BranchId };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   fill_Agent_Wise_Collections(msg);

               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       function fill_Agent_Wise_Collections(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           $('#div_routewisemain').css('display', 'block');
           $('#div_agentwisemain').css('display', 'block');
           j = 1;
           var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
           results += '<thead><tr style="background-color: #abbed2;"><th >Agent Name</th><th >Collection Amount</th></tr></thead></tbody>';
           var k = 1;
           var l = 0;
           for (var i = 0; i < msg.length; i++) {
               results += '<tr style="background-color: antiquewhite;">';
               results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"    class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
               results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
               l = l + 1;
               if (l == 30) {
                   l = 0;
               }
           }
           results += '</table></div>';
           $("#div_agentwise").html(results);
       }
       function BranchWiseDueDetails() {
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           //            var BranchId = document.getElementById('ddlPlant').value;
           var BranchId = document.getElementById('ctl00_ContentPlaceHolder1_ddlPlant').value;
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'Branch_Wise_DueAmount', 'BranchId': BranchId };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   if (msg[0].type == "GroupWise" || msg[0].type == "CompanyWise") {
                       fillPlantUnder_Branch_Wise_DueAmount(msg);
                   }
                   else {
                       fill_Branch_Wise_DueAmount(msg);
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
       function fillPlantUnder_Branch_Wise_DueAmount(msg) {
           $('#div_MainPlantDetails').css('display', 'block');
           j = 1;
           var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
           results += '<thead><tr style="background-color: #abbed2;"><th >Plant Name</th><th >Due Amount</th></tr></thead></tbody>';
           var k = 1;
           var l = 0;
           for (var i = 0; i < msg.length; i++) {
               results += '<tr style="background-color: antiquewhite;">';
               results += '<td class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnGroupUnder_PlantClick_Branch_Wise_DueAmount(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
               results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
               l = l + 1;
               if (l == 30) {
                   l = 0;
               }
           }
           results += '</table></div>';
           $("#div_PlantDetails").html(results);
       }
       //        function fill_Branch_Wise_DueAmount(msg) {
       //            scrollTo(0, 0);
       //            $('#divMainAddNewRow1').css('display', 'block');
       //            j = 1;
       //            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
       //            results += '<thead><tr style="background-color: #abbed2;"><th>Branch Name</th><th >Due Amount</th></tr></thead></tbody>';
       //            var k = 1;
       //            var l = 0;
       //            for (var i = 0; i < msg.length; i++) {
       //                results += '<tr style="background-color: antiquewhite;">';
       //                results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnRoute_Wise_DueAmount_Click(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
       //                results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
       //                l = l + 1;
       //                if (l == 30) {
       //                    l = 0;
       //                }
       //            }
       //            results += '</table></div>';
       //            $("#divChart").html(results);
       //        }


       var routewisearry = []; var salestypearr = [];
       function fill_Branch_Wise_DueAmount(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           j = 1;
           var BranchTable = []; var totsale = 0; var totpaidamount = 0; var grand_totsale = 0; var grand_totpaidamount = 0;
           var totdiff = 0; var grand_totdiff = 0;
           if (msg[0].type1 == "BranchWise") {
               routewisearry = msg;
               //           salestypearr=msg[0].CategoryClassifications;
               var results = '<div class="divcontainer" style="overflow:auto;"><table border="1" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               //results += '<thead><tr><th scope="col">RouteName</th><th scope="col">SalesType</th><th scope="col">SaleValue</th></tr></thead></tbody>';

               //results += '<thead><tr><th scope="col">RouteName</th><th scope="col">SalesType</th><th scope="col">SaleValue</th><th scope="col">PaidAmount</th><th scope="col">Difference</th></tr></thead></tbody>';
               for (var i = 0; i < routewisearry.length; i++) {
                   if (BranchTable.indexOf(routewisearry[i].BranchName) == -1) {
                       if (i == 0) {
                       }
                       else {
                           results += '<tr style="background-color: antiquewhite;">';
                           results += '<td scope="row" class="1" ></td>';
                           results += '<td scope="row" class="1"  style="font-size:18px;font-weight:bold;color:#006400;">Total</td>';
                           //                          results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totsale).toFixed(2) + '</td>';
                           //                          results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totpaidamount).toFixed(2) + '</td>'
                           results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totdiff).toFixed(2) + '</td></tr>';
                       }
                       totsale = 0;
                       totpaidamount = 0;
                       totdiff = 0;
                       results += '<tr style="background-color: antiquewhite;">';
                       results += '<td scope="row" class="1">' + routewisearry[i].BranchName + '</td>';
                       results += '<td  onclick="btnRoute_Wise_DueAmount_Click(\'' + routewisearry[i].salestypeid + '\');"><span style="text-decoration: underline;color:blue;">' + routewisearry[i].SalesType + '</span></td>';
                       //                      results += '<td  class="2">' + routewisearry[i].SaleValue + '</td>';
                       //                      results += '<td  class="2">' + routewisearry[i].PaidAmount + '</td>';
                       results += '<td  class="2">' + routewisearry[i].dispatchqty + '</td></tr>';
                       totsale += parseFloat(routewisearry[i].SaleValue);
                       grand_totsale += parseFloat(routewisearry[i].SaleValue);
                       totpaidamount += parseFloat(routewisearry[i].PaidAmount);
                       grand_totpaidamount += parseFloat(routewisearry[i].PaidAmount);
                       totdiff += parseFloat(routewisearry[i].dispatchqty);
                       grand_totdiff += parseFloat(routewisearry[i].dispatchqty); ;
                       BranchTable.push(routewisearry[i].BranchName);
                   }
                   else {
                       results += '<tr style="background-color: antiquewhite;">';
                       results += '<td scope="row" class="1" ></td>';
                       results += '<td  onclick="btnRoute_Wise_DueAmount_Click(\'' + routewisearry[i].salestypeid + '\');"><span style="text-decoration: underline;color:blue;">' + routewisearry[i].SalesType + '</span></td>';
                       //                      results += '<td  class="2">' + routewisearry[i].SaleValue + '</td>';
                       //                      results += '<td  class="2">' + routewisearry[i].PaidAmount + '</td>';
                       results += '<td  class="2">' + routewisearry[i].dispatchqty + '</td></tr>';
                       totsale += parseFloat(routewisearry[i].SaleValue);
                       grand_totsale += parseFloat(routewisearry[i].SaleValue); ;
                       totpaidamount += parseFloat(routewisearry[i].PaidAmount);
                       grand_totpaidamount += parseFloat(routewisearry[i].PaidAmount);
                       totdiff += parseFloat(routewisearry[i].dispatchqty);
                       grand_totdiff += parseFloat(routewisearry[i].dispatchqty); ;
                   }
               }
               results += '<tr style="background-color: antiquewhite;">';
               results += '<td scope="row" class="1" ></td>';
               //              avgfat = totfatqty / totqty;
               //              avgfat = parseFloat(avgfat).toFixed(2);
               results += '<td scope="row" class="1" style="font-size:18px;font-weight:bold;color:#006400;" >Total</td>';
               //              results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totsale).toFixed(2) + '</td>';
               //              results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totpaidamount).toFixed(2) + '</td>'
               results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totdiff).toFixed(2) + '</td></tr>';
               results += '<tr style="background-color: antiquewhite;">';
               results += '<td scope="row" class="1" ></td>';
               results += '<td scope="row" class="1" style="font-size:22px;font-weight:bold;color:#B22222;" >Grand Total</td>';
               //              results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;">' + parseFloat(grand_totsale).toFixed(2) + '</td>';
               //              results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;">' + parseFloat(grand_totpaidamount).toFixed(2) + '</td>';
               results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;">' + parseFloat(grand_totdiff).toFixed(2) + '</td></tr>';
               results += '</table></div>';
               $("#divChart").html(results);
           }
           else {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th>Branch Name</th><th >Due Amount</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnRoute_Wise_DueAmount_Click(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
               results += '</table></div>';
               $("#divChart").html(results);
           }
       }

       function btnGroupUnder_PlantClick_Branch_Wise_DueAmount(BranchId) {
           var BranchId;
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           //            var PlantName = document.getElementById('ddlPlant').value;
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'Branch_Wise_DueAmount', 'BranchId': BranchId };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }

                   fill_Branch_Wise_DueAmount(msg);

               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       function btnRoute_Wise_DueAmount_Click(BranchId) {
           var BranchId;
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'Route_Wise_DueAmount', 'BranchId': BranchId };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   fill_Route_Wise_DueAmount(msg);

               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }

       function fill_Route_Wise_DueAmount(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           $('#div_routewisemain').css('display', 'block');
           j = 1;
           var BranchTable = []; var totsale = 0; var totpaidamount = 0; var grand_totsale = 0; var grand_totpaidamount = 0;
           var totdiff = 0; var grand_totdiff = 0;
           if (msg[0].type1 != "RouteClassification") {
               var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               results += '<thead><tr style="background-color: #abbed2;"><th >Route Name</th><th >Due Amount</th></tr></thead></tbody>';
               var k = 1;
               var l = 0;
               for (var i = 0; i < msg.length; i++) {
                   results += '<tr style="background-color: antiquewhite;">';
                   results += '<td    class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" onclick="btnAgent_Wise_DueAmount_Click(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                   results += '<td    class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
                   l = l + 1;
                   if (l == 30) {
                       l = 0;
                   }
               }
               results += '</table></div>';
               $("#div_routewise").html(results);
           }
           else {
               routewisearry = msg;
               //           salestypearr=msg[0].CategoryClassifications;
               var results = '<div class="divcontainer" style="overflow:auto;"><table border="1" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
               //results += '<thead><tr><th scope="col">RouteName</th><th scope="col">SalesType</th><th scope="col">SaleValue</th></tr></thead></tbody>';

               //results += '<thead><tr><th scope="col">RouteName</th><th scope="col">SalesType</th><th scope="col">SaleValue</th><th scope="col">PaidAmount</th><th scope="col">Difference</th></tr></thead></tbody>';
               for (var i = 0; i < routewisearry.length; i++) {
                   if (BranchTable.indexOf(routewisearry[i].BranchName) == -1) {
                       if (i == 0) {
                       }
                       else {
                           results += '<tr style="background-color: antiquewhite;">';
                           results += '<td scope="row" class="1" ></td>';
                           results += '<td scope="row" class="1"  style="font-size:18px;font-weight:bold;color:#006400;">Total</td>';
                           //                          results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totsale).toFixed(2) + '</td>';
                           //                          results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totpaidamount).toFixed(2) + '</td>'
                           results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totdiff).toFixed(2) + '</td></tr>';
                       }
                       totsale = 0;
                       totpaidamount = 0;
                       totdiff = 0;
                       results += '<tr style="background-color: antiquewhite;">';
                       results += '<td scope="row" class="1">' + routewisearry[i].BranchName + '</td>';
                       results += '<td  onclick="btnAgent_Wise_DueAmount_Click(\'' + routewisearry[i].salestypeid + '\');"><span style="text-decoration: underline;color:blue;">' + routewisearry[i].SalesType + '</span></td>';
                       //                      results += '<td  class="2">' + routewisearry[i].SaleValue + '</td>';
                       //                      results += '<td  class="2">' + routewisearry[i].PaidAmount + '</td>';
                       results += '<td  class="2">' + routewisearry[i].dispatchqty + '</td></tr>';
                       totsale += parseFloat(routewisearry[i].SaleValue);
                       grand_totsale += parseFloat(routewisearry[i].SaleValue);
                       totpaidamount += parseFloat(routewisearry[i].PaidAmount);
                       grand_totpaidamount += parseFloat(routewisearry[i].PaidAmount);
                       totdiff += parseFloat(routewisearry[i].dispatchqty);
                       grand_totdiff += parseFloat(routewisearry[i].dispatchqty);
                       BranchTable.push(routewisearry[i].BranchName);
                   }
                   else {
                       results += '<tr style="background-color: antiquewhite;">';
                       results += '<td scope="row" class="1" ></td>';
                       results += '<td  onclick="btnAgent_Wise_DueAmount_Click(\'' + routewisearry[i].salestypeid + '\');"><span style="text-decoration: underline;color:blue;">' + routewisearry[i].SalesType + '</span></td>';
                       //                      results += '<td  class="2">' + routewisearry[i].SaleValue + '</td>';
                       //                      results += '<td  class="2">' + routewisearry[i].PaidAmount + '</td>';
                       results += '<td  class="2">' + routewisearry[i].dispatchqty + '</td></tr>';
                       totsale += parseFloat(routewisearry[i].SaleValue);
                       grand_totsale += parseFloat(routewisearry[i].SaleValue); ;
                       totpaidamount += parseFloat(routewisearry[i].PaidAmount);
                       grand_totpaidamount += parseFloat(routewisearry[i].PaidAmount);
                       totdiff += parseFloat(routewisearry[i].dispatchqty);
                       grand_totdiff += parseFloat(routewisearry[i].dispatchqty); ;
                   }
               }
               results += '<tr style="background-color: antiquewhite;">';
               results += '<td scope="row" class="1" ></td>';
               //              avgfat = totfatqty / totqty;
               //              avgfat = parseFloat(avgfat).toFixed(2);
               results += '<td scope="row" class="1" style="font-size:18px;font-weight:bold;color:#006400;" >Total</td>';
               //              results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totsale).toFixed(2) + '</td>';
               //              results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totpaidamount).toFixed(2) + '</td>'
               results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totdiff).toFixed(2) + '</td></tr>';
               results += '<tr style="background-color: antiquewhite;">';
               results += '<td scope="row" class="1" ></td>';
               results += '<td scope="row" class="1" style="font-size:22px;font-weight:bold;color:#B22222;" >Grand Total</td>';
               //              results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;">' + parseFloat(grand_totsale).toFixed(2) + '</td>';
               //              results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;">' + parseFloat(grand_totpaidamount).toFixed(2) + '</td>';
               results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;">' + parseFloat(grand_totdiff).toFixed(2) + '</td></tr>';
               results += '</table></div>';
               $("#div_routewise").html(results);
           }
       }
       function btnAgent_Wise_DueAmount_Click(BranchId) {
           var BranchId;
           //            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
           //            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
           $('#divHide').css('display', 'block');
           var data = { 'operation': 'Agent_Wise_DueAmount', 'BranchId': BranchId };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   fill_Agent_Wise_DueAmount(msg);

               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       function fill_Agent_Wise_DueAmount(msg) {
           scrollTo(0, 0);
           $('#divMainAddNewRow1').css('display', 'block');
           $('#div_routewisemain').css('display', 'block');
           $('#div_agentwisemain').css('display', 'block');
           j = 1;
           var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
           results += '<thead><tr style="background-color: #abbed2;"><th >Agent Name</th><th>Due Amount</th></tr></thead></tbody>';
           var k = 1;
           var l = 0;
           for (var i = 0; i < msg.length; i++) {
               results += '<tr style="background-color: antiquewhite;">';
               results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"    class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
               results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
               l = l + 1;
               if (l == 30) {
                   l = 0;
               }
           }
           results += '</table></div>';
           $("#div_agentwise").html(results);
       }


       function bindplantdetails() {
           var SelecteType = document.getElementById('ctl00_ContentPlaceHolder1_ddltype').value;
           var data = { 'operation': 'GetSalesOffices', 'SelecteType': SelecteType };
           var s = function (msg) {
               if (msg) {
                   if (msg == "Session Expired") {
                       alert(msg);
                       window.location = "Login.aspx";
                   }
                   BindPlantNames(msg);
               }
               else {
               }
           };
           var e = function (x, h, e) {
           };
           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
           callHandler(data, s, e);
       }
       function BindPlantNames(msg) {
           var ddlPlant = document.getElementById("ctl00_ContentPlaceHolder1_ddlPlant");
           var length = ddlPlant.options.length;
           ddlPlant.options.length = null;
           for (var i = 0; i < msg.length; i++) {
               if (msg[i].BranchName != null) {
                   var opt = document.createElement('option');
                   opt.innerHTML = msg[i].BranchName;
                   opt.value = msg[i].Sno;
                   ddlPlant.appendChild(opt);
               }
           }
       }


       function branchtypeChange() {
           var branchtype = document.getElementById('ctl00_ContentPlaceHolder1_ddlbarnchCategory').value;
           var planttype = document.getElementById('ctl00_ContentPlaceHolder1_ddlPlant').value;
           if (branchtype == "BranchWiseCollections") {
               $('#tddatatypeid').css('display', 'none');
               $('#tdbtnsubmit').css('display', 'block');
               $('#tdDay_type').css('display', 'none');
               $('#tddatatypeid').css('display', 'none');
               $('#tidcategory').css('display', 'none');

               $('#spnDispatchQty').css('display', 'none');
               $('#spnSvdsCompany').css('display', 'none');
               $('#spnSvfCompany').css('display', 'none');
               $('#spnMilkDispatchQty').css('display', 'none');
               $('#spnCurdDispatchQty').css('display', 'none');
               $('#spnOthersDispatchQty').css('display', 'none');
               $('#spnDispatchQty').css('display', 'none');
               $('#spnSvdsCompany').css('display', 'none');
               $('#spnSvfCompany').css('display', 'none');
               $('#spnSaleValue').css('display', 'none');
               $('#spnSaleValue').css('display', 'none');
               $('#spnCollection').css('display', 'none');
               $('#spnDueValue').css('display', 'none');
               $('#example1').css('display', 'none');
           }
           if (branchtype == "CategoryWiseDespatch") {
               $('#tdbtnsubmit').css('display', 'none');
               $('#tidcategory').css('display', 'block');
               $('#tdDay_type').css('display', 'none');
               $('#tddatatypeid').css('display', 'block');
               $('#spnMilkDispatchQty').css('display', 'block');
               $('#spnCurdDispatchQty').css('display', 'block');
               $('#spnOthersDispatchQty').css('display', 'block');

               $('#spnDispatchQty').css('display', 'none');
               $('#spnSvdsCompany').css('display', 'none');
               $('#spnSvfCompany').css('display', 'none');
               $('#spnSaleValue').css('display', 'none');
               $('#spnCollection').css('display', 'none');
               $('#spnDueValue').css('display', 'none');
               $('#example1').css('display', 'block');
               // datecontrolsession();
           }
           if (branchtype == "BranchWiseDespatch") {
               $('#tddatatypeid').css('display', 'none');
               $('#tdbtnsubmit').css('display', 'block');
               $('#tidcategory').css('display', 'none');
               $('#tdDay_type').css('display', 'block');
               $('#spnMilkDispatchQty').css('display', 'none');
               $('#spnCurdDispatchQty').css('display', 'none');
               $('#spnOthersDispatchQty').css('display', 'none');
               $('#example1').css('display', 'none');
               $('#spnSaleValue').css('display', 'none');
               $('#spnCollection').css('display', 'none');
               $('#spnDueValue').css('display', 'none');

               if (planttype == "8009") {
                   $('#spnDispatchQty').css('display', 'none');
                   $('#spnSvdsCompany').css('display', 'none');
                   $('#spnSvfCompany').css('display', 'none');
                   $('#spnSaleValue').css('display', 'none');
               }
               if (planttype == "8012") {
                   $('#spnDispatchQty').css('display', 'none');
                   $('#spnSvdsCompany').css('display', 'none');
                   $('#spnSvfCompany').css('display', 'none');
                   $('#spnSaleValue').css('display', 'none');
               }
               if (planttype == "8013") {
                   $('#spnDispatchQty').css('display', 'none');
                   $('#spnSvdsCompany').css('display', 'none');
                   $('#spnSvfCompany').css('display', 'none');
                   $('#spnSaleValue').css('display', 'none');
               }
           }
           daterangepicker1();
       }
       function CloseClick1() {
           $('html, body').animate({
               scrollTop: $("#BranchWiseDespatch").offset().down
           }, 2000);
           $('#BranchWiseDespatch').focus();
           $('#divMainAddNewRow1').css('display', 'none');
           $('#div_MainPlantDetails').css('display', 'block');
       }
       function close_routewise() {
           $('#div_routewisemain').css('display', 'none');
       }
       function close_agentwise() {
           $('#div_agentwisemain').css('display', 'none');
       }
       function close_routewiseCompare() {
           $('#div_routewisemainCompare').css('display', 'none');
       }
       function close_AgentProductLineChart() {
           branchtypeChange();
           $('#example').css('display', 'none');
       }
       function PlantCloseClick() {
           $('#div_MainPlantDetails').css('display', 'none');
       }
       function OthersGroupProductsClick() {
           $('#div_MainGroupOthersProducts').css('display', 'none');

       }
       function OthersCategoryClick() {
           $('#div_MainOthersCategory').css('display', 'none');
       }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:UpdateProgress ID="updateProgress1" runat="server">
                    <ProgressTemplate>
                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0;
                            right: 0; left: 0; z-index: 9999; background-color: #FFFFFF; opacity: 0.7;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="thumbnails/loading.gif"
                                Style="padding: 10px; position: absolute; top: 40%; left: 40%; z-index: 99999;" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <section class="content-header">
                <h1>
                    Dash Board<small>Preview</small>
                </h1>
                <ol class="breadcrumb">
                    <li><a href="#"><i class="fa fa-dashboard"></i>Dash Board</a></li>
                    <li><a href="#">Dash Board</a></li>
                </ol>
            </section>
            <section class="content">
                <div class="box box-info">
                    <div class="box-body">
                        <div style="width: 100%; padding-left: 6%;">
                            <table>
                                <tr>
                                    <td style="width: 130px;">
                                        <asp:DropDownList ID="ddltype" runat="server" CssClass="form-control" onchange="bindplantdetails()">
                                            <asp:ListItem Value="Vyshnavi Group" Text="Vyshnavi Group">Vyshnavi Group</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 2%;">
                                    </td>
                                    <td style="width: 130px;" id="divPlant">
                                        <asp:DropDownList ID="ddlPlant" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                      
                                    </td>
                                    <td style="width: 2%;">
                                    </td>
                                    <td style="width: 130px;" id="tdddlbarnchCategory">
                                        <asp:DropDownList ID="ddlbarnchCategory" runat="server" CssClass="form-control" onchange="branchtypeChange();">
                                            <asp:ListItem Value="BranchWiseDespatch" Text="BranchWiseDespatch">BranchWiseDespatch</asp:ListItem>
                                            <asp:ListItem Value="BranchWiseCollections" Text="BranchWiseCollections">BranchWiseCollections</asp:ListItem>
                                            <asp:ListItem Value="CategoryWiseDespatch" Text="CategoryWiseDespatch">CategoryWiseDespatch</asp:ListItem>
                                        </asp:DropDownList>
                                        <%--<select id="ddlbarnchCategory" class="form-control" onchange="branchtypeChange();">
                                            <option>BranchWiseDespatch</option>
                                            <option>BranchWiseCollections</option>
                                            <option>CategoryWiseDespatch</option>
                                        </select>--%>
                                    </td>
                                    <td style="width: 130px; display: none" id="tddatatypeid">
                                        <asp:DropDownList ID="ddlDataType" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="Quantity" Text="Quantity">Quantity</asp:ListItem>
                                            <asp:ListItem Value="Value" Text="Value">Value</asp:ListItem>
                                        </asp:DropDownList>
                                        <%-- <select id="ddlDataType" class="form-control">
                                            <option>Quantity</option>
                                            <option>Value</option>
                                        </select>--%>
                                    </td>
                                    <td style="width: 2%;">
                                    </td>
                                    <td style="width: 130px;" id="tdDay_type">
                                        <asp:DropDownList ID="ddlDayType" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="Day" Text="Quantity">Daily</asp:ListItem>
                                            <asp:ListItem Value="Weak" Text="Value">Weakly</asp:ListItem>
                                            <asp:ListItem Value="Month" Text="Value">Monthly</asp:ListItem>
                                        </asp:DropDownList>
                                        <%-- <select id="ddlDataType" class="form-control">
                                            <option>Quantity</option>
                                            <option>Value</option>
                                        </select>--%>
                                    </td>
                                    <td style="width: 2%;">
                                    </td>
                                    <td>
                                        <div id="reportrange" style="background: #fff; cursor: pointer; padding: 5px 10px;
                                            border: 1px solid #ccc; width: 100%" onclick="DateClick();">
                                            <i class="fa fa-calendar"></i>&nbsp; <span></span><i class="fa fa-caret-down"></i>
                                        </div>
                                    </td>
                                    <td>
                                        &nbsp; &nbsp;
                                    </td>
                                    <td id="tdbtnsubmit">
                                        <asp:Button ID="btnsubmit" runat="server" Text="Submit" CssClass="btn btn-primary"
                                            OnClick="btnsubmit_click" />
                                        <%--<input type="button" id="btncategory" value="Submit" class="btn btn-primary" onclick="GraphicalNetSaleClick()" style="display:none;" />--%>
                                    </td>
                                    <td id="tidcategory">
                                        <input type="button" id="btncategory" value="Get" class="btn btn-primary" onclick="fillProdcutData()" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <div id="firstdiv" runat="server">
                            <div class="col-md-12" id="SpanDetails">
                                <div class="box box-solid box-warning">
                                    <div class="box-header with-border">
                                        <h3 class="fa fa-inbox">
                                            <i style="padding-right: 5px;"></i>Sales Details
                                        </h3>
                                        <div class="box-tools pull-left">
                                            <button class="btn btn-box-tool" data-widget="collapse">
                                                <i class="fa fa-minus"></i>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="box-body">
                                        <div class="box-body no- back-red activepadding">
                                            <div class="col-lg-3 col-xs-6" id="spnDispatchQty" style="width: 30%;">
                                                <!-- small box -->
                                                <div class="small-box bg-aqua">
                                                    <div class="inner">
                                                        <asp:Label ID="lbldispatchqty" runat="server" Text="0" CssClass="h3"> </asp:Label>
                                                        <%-- <h3 id="hdispatchqty"> 
                                                            0
                                                        </h3>--%>
                                                        <p id="spndespatchid">
                                                        </p>
                                                    </div>
                                                    <div class="icon">
                                                        <i class="fa fa-cubes"></i>
                                                    </div>
                                                    <%--<a href="#datagrid" class="small-box-footer" onclick="branchwisedispqty();" data-toggle="modal"
                                                        data-target="#myModal">BranchWise Dispatchqty </a>--%>
                                                    <asp:LinkButton ID="LinkButton1" Text="Group Dispatchqty" CssClass="small-box-footer"
                                                        OnClick="LinkButton_Click" runat="server">Group Dispatchqty<i class="fa fa-arrow-circle-right"></i></asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="col-lg-3 col-xs-6" id="spnSvdsCompany" style="width: 30%;">
                                                <!-- small box -->
                                                <div class="small-box bg-aqua">
                                                    <div class="inner">
                                                        <asp:Label ID="lblSvdsdispatch" runat="server" Text="0" CssClass="h3"> </asp:Label>
                                                        <%-- <h3 id="hdispatchqty"> 
                                                            0
                                                        </h3>--%>
                                                        <p id="P1">
                                                        </p>
                                                    </div>
                                                    <div class="icon">
                                                        <i class="fa fa-cubes"></i>
                                                    </div>
                                                    <%--<a href="#datagrid" class="small-box-footer" onclick="branchwisedispqty();" data-toggle="modal"
                                                        data-target="#myModal">BranchWise Dispatchqty </a>--%>
                                                    <asp:LinkButton ID="LinkButton2" Text="SVDS Dispatch Qty" CssClass="small-box-footer"
                                                        OnClick="LinkButton_Click" runat="server">SVDS Dispatch Qty<i class="fa fa-arrow-circle-right"></i></asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="col-lg-3 col-xs-6" id="spnSvfCompany" style="width: 30%;">
                                                <!-- small box -->
                                                <div class="small-box bg-aqua">
                                                    <div class="inner">
                                                        <asp:Label ID="lblSvfdispatch" runat="server" Text="0" CssClass="h3"> </asp:Label>
                                                        <%-- <h3 id="hdispatchqty"> 
                                                            0
                                                        </h3>--%>
                                                        <p id="P2">
                                                        </p>
                                                    </div>
                                                    <div class="icon">
                                                        <i class="fa fa-cubes"></i>
                                                    </div>
                                                    <%--<a href="#datagrid" class="small-box-footer" onclick="branchwisedispqty();" data-toggle="modal"
                                                        data-target="#myModal">BranchWise Dispatchqty </a>--%>
                                                    <asp:LinkButton ID="LinkButton3" Text="Svf Dispatch Qty" CssClass="small-box-footer"
                                                        OnClick="LinkButton_Click" runat="server">Svf Dispatch Qty<i class="fa fa-arrow-circle-right"></i></asp:LinkButton>
                                                </div>
                                            </div>
                                            <!-- ./col -->
                                            <div class="col-lg-3 col-xs-6" id="spnSaleValue">
                                                <!-- small box -->
                                                <div class="small-box bg-purple">
                                                    <div class="inner">
                                                        <%--<h3 id="hsalevalue1" runat="server">
                                                            0
                                                        </h3>--%>
                                                        <h3 id="hsalevalue" runat="server">
                                                            0
                                                        </h3>
                                                        <p id="spnsaleid">
                                                        </p>
                                                    </div>
                                                    <div class="icon">
                                                        <i class="ion ion-pie-graph"></i>
                                                    </div>
                                                    <a href="#datagrid" class="small-box-footer" onclick="branchwisesaledetails();" data-toggle="modal"
                                                        data-target="#myModal">BranchWise Sale Value <i class="fa fa-arrow-circle-right">
                                                        </i></a>
                                                    <%--<asp:LinkButton id="LinkButton4" Text="BranchWise SaleValue" CssClass="small-box-footer" OnClick="LinkButtonSalesValue_Click" runat="server">BranchWise SaleValue<i class="fa fa-arrow-circle-right"></i></asp:LinkButton>--%>
                                                </div>
                                            </div>
                                            <!-- ./col -->
                                            <div class="col-lg-3 col-xs-6" id="spnMilkDispatchQty" style="height: 20px; display: none;">
                                                <!-- small box -->
                                                <div class="small-box bg-olive">
                                                    <div class="inner">
                                                        <h3 id="hmilkqty">
                                                            0
                                                        </h3>
                                                        <p id="spnmilkid">
                                                        </p>
                                                    </div>
                                                    <div class="icon">
                                                        <i class="fa fa-thumbs-o-up"></i>
                                                    </div>
                                                    <a href="#datagrid" class="small-box-footer" onclick="branchwisemilkdetails();" data-toggle="modal"
                                                        data-target="#myModal">BranchWise Milk Qty <i class="fa fa-arrow-circle-right"></i>
                                                    </a>
                                                </div>
                                            </div>
                                            <div class="col-lg-3 col-xs-6" id="spnCurdDispatchQty" style="display: none;">
                                                <!-- small box -->
                                                <div class="small-box bg-teal">
                                                    <div class="inner">
                                                        <h3 id="hcurdqty">
                                                            0
                                                        </h3>
                                                        <p id="spncurdid">
                                                        </p>
                                                    </div>
                                                    <div class="icon">
                                                        <i class="ion ion-stats-bars"></i>
                                                    </div>
                                                    <a href="#datagrid" class="small-box-footer" onclick="branchwiseCurddetails();" data-toggle="modal"
                                                        data-target="#myModal">BranchWise Curd Qty <i class="fa fa-arrow-circle-right"></i>
                                                    </a>
                                                </div>
                                            </div>
                                            <!-- ./col -->
                                            <div class="col-lg-3 col-xs-6" id="spnOthersDispatchQty" style="display: none;">
                                                <!-- small box -->
                                                <div class="small-box bg-yellow">
                                                    <div class="inner">
                                                        <h3 id="hothersqty">
                                                            0
                                                        </h3>
                                                        <p id="spnothersid">
                                                        </p>
                                                    </div>
                                                    <div class="icon">
                                                        <i class="ion ion-bag"></i>
                                                    </div>
                                                    <a href="#datagrid" class="small-box-footer" onclick="branchwiseotherdetails1();"
                                                        data-toggle="modal" data-target="#myModal">BranchWise Others Qty <i class="fa fa-arrow-circle-right">
                                                        </i></a>
                                                </div>
                                            </div>
                                            <div class="col-lg-3 col-xs-6" id="spnCollection">
                                                <!-- small box -->
                                                <div class="small-box bg-teal">
                                                    <div class="inner">
                                                        <h3 id="spnamount" runat="server">
                                                            0
                                                        </h3>
                                                        <p id="spncollid">
                                                        </p>
                                                    </div>
                                                    <div class="icon">
                                                        <i class="ion ion-bag"></i>
                                                    </div>
                                                    <a href="#datagrid" class="small-box-footer" onclick="BranchWiseCollection();" data-toggle="modal"
                                                        data-target="#myModal">Branch Collection <i class="fa fa-arrow-circle-right"></i>
                                                    </a>
                                                    <%--<asp:LinkButton id="LinkButton2" Text="BranchWise Collections" CssClass="small-box-footer" OnClick="LinkButtonCollections_Click" runat="server">BranchWise Collections<i class="fa fa-arrow-circle-right"></i></asp:LinkButton>--%>
                                                </div>
                                            </div>
                                            <div class="col-lg-3 col-xs-6" id="spnDueValue">
                                                <!-- small box -->
                                                <div class="small-box bg-blue">
                                                    <div class="inner">
                                                        <h3 id="hduevalue" runat="server">
                                                            0
                                                        </h3>
                                                        <p id="spndueid">
                                                        </p>
                                                    </div>
                                                    <div class="icon">
                                                        <i class="ion ion-bag"></i>
                                                    </div>
                                                    <a href="#datagrid" class="small-box-footer" onclick="BranchWiseDueDetails();" data-toggle="modal"
                                                        data-target="#myModal">Branch Due details<i class="fa fa-arrow-circle-right"></i></a>
                                                    <%-- <asp:LinkButton id="LinkButton3" Text="BranchWise Due" CssClass="small-box-footer" OnClick="LinkButtonDue_Click" runat="server">BranchWise Due<i class="fa fa-arrow-circle-right"></i></asp:LinkButton>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12" id="Branch_wise_Dispatch">
                                <div class="row">
                                    <div id="datagrid">
                                        <div class="box box-solid box-success">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">
                                                    <i style="padding-right: 5px;" class="fa fa-fw fa-user"></i>Branch Wise Information</h3>
                                                <div class="box-tools pull-right">
                                                    <button class="btn btn-box-tool" data-widget="collapse">
                                                        <i class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="box-body" id="BranchWiseDespatch">
                                                <asp:GridView ID="grdbranchwisedispatch" runat="server" Style="text-align: right"
                                                    CssClass="table table-bordered table-hover dataTable no-footer" OnRowCommand="grdbranchwisedispatch_RowCommand"
                                                    OnRowDataBound="grdbranchwisedispatch_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnnewclick" runat="server" Text="View" CssClass="btn btn-primary"
                                                                    CommandArgument='<%#Container.DataItemIndex%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div style="width: 200px; position: fixed; left: 50%; top: 95%; margin-left: -100px;">
                                    <table class="inputstable">
                                        <tbody>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnsalesofficewise" runat="server" Text="View Sales Office Wise"
                                                        CssClass="btn btn-primary" Width="180px" OnClick="btnsalesoffice_click" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div class="col-sm-12" style="width: 100%; display: none;" id="div_MainPlantComparison"
                                runat="server">
                                <div class="box box-solid box-info">
                                    <div class="box-header with-border">
                                        <h3 class="ion ion-clipboard">
                                            <i style="padding-right: 5px;"></i>Sale Quantity Comparison
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
                                                <div id="div_PlantComparison">
                                                    <asp:GridView ID="grdbranchwisecomparison" runat="server" CssClass="table table-bordered table-hover dataTable no-footer">
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-12" style="width: 100%; display: none;" id="div_MainBranchComparison">
                                <div class="box box-solid box-info">
                                    <div class="box-header with-border">
                                        <h3 class="ion ion-clipboard">
                                            <i style="padding-right: 5px;"></i>Sale Quantity Comparison
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
                            <div id="example1" class="col-sm-12" style="display: none;">
                                <div class="col-sm-6">
                                    <div class="box box-solid box-danger">
                                        <div class="box-header with-border">
                                            <h3 class="ion ion-clipboard">
                                                <i style="padding-right: 5px;"></i>SubCategory Wise Information
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
                                                    <div id="tableProductData">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="box box-solid box-info">
                                        <div class="box-header with-border">
                                            <h3 class="ion ion-pie-graph">
                                                <i style="padding-right: 5px;"></i>SubCategory Wise Qty
                                            </h3>
                                            <div class="box-tools pull-right">
                                                <button class="btn btn-box-tool" data-widget="collapse">
                                                    <i class="fa fa-minus"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div class="box-body no-padding">
                                                <div id="Div1" class="k-content">
                                                    <div class="chart-wrapper">
                                                        <div id="Div2">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divHide1" style="width: 120%; display: none;" runat="server">
                                <div class="modal fade in" id="div_MainPlantDetails1" runat="server" style="display: none;
                                    padding-right: 17px;">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header" style="text-align: right">
                                                <asp:Button ID="ClosButton" Text="X" Font-Names="Verdana" Font-Size="14pt" class="btn btn-danger"
                                                    OnClick="Close_Click" runat="server" /></button>
                                                <h4 class="modal-title" style="text-align: left">
                                                    Plant Wise Avg Qty ( From Date :
                                                    <asp:Label ID="lblfromdate" runat="server" ForeColor="Red"></asp:Label>
                                                    - To Date :
                                                    <asp:Label ID="lbltodate" runat="server" ForeColor="Red"></asp:Label>
                                                    )</h4>
                                            </div>
                                            <div style="display: none;">
                                            </div>
                                            <div class="modal-body" id="div_PlantDetails1" style="height: 400px; overflow-y: scroll;">
                                                <asp:GridView ID="grddata" CssClass="table table-bordered table-hover dataTable no-footer"
                                                    runat="server" Style="width: 100%; text-align: right" Font-Bold="true" OnRowDataBound="grddata_RowDataBound"
                                                    OnRowCommand="grddata_RowCommand" OnRowCreated="grddata_RowCreated">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Button ID="Button1" runat="server" Text="View" CssClass="btn btn-primary" CommandArgument='<%#Container.DataItemIndex%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <!-- /.modal-content -->
                                    </div>
                                    <!-- /.modal-dialog -->
                                </div>
                                <div class="modal fade in" id="divMainAddNewRow2" runat="server" style="display: none;
                                    padding-right: 17px;">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header" style="text-align: right">
                                                <asp:Button ID="Button3" Text="X" Font-Names="Verdana" Font-Size="14pt" class="btn btn-danger"
                                                    OnClick="Close_Click1" runat="server" /></button>
                                                <h4 class="modal-title" style="text-align: left">
                                                    Branch Wise Avg Qty ( From Date :
                                                    <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                                                    - To Date :
                                                    <asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label>
                                                    )</h4>
                                            </div>
                                            <div style="text-align: right; padding-right: 99px; font-size: 21px;">
                                                <asp:LinkButton ID="LinkButton12" Text="View LineChart" CssClass="small-box-footer"
                                                    OnClick="LinkButton12_Click" runat="server">View Cattegory Wise<i class="fa fa-arrow-circle-right"></i></asp:LinkButton>
                                            </div>
                                            <div class="modal-body" id="divChart1" style="height: 400px; overflow-y: scroll;">
                                                <asp:GridView ID="grdBranchReport" CssClass="table table-bordered table-hover dataTable no-footer"
                                                    runat="server" Style="width: 100%; text-align: right" Font-Bold="true" OnRowCommand="grdBranchReport_RowCommand"
                                                    OnRowCreated="grdBranchReport_RowCreated" OnRowDataBound="grdBranchReport_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Button ID="Button2" runat="server" Text="View" CssClass="btn btn-primary" CommandArgument='<%#Container.DataItemIndex%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <!-- /.modal-content -->
                                    </div>
                                    <!-- /.modal-dialog -->
                                </div>
                                <div class="modal fade in" id="div_MainPlantWiseCategory" runat="server" style="display: none;
                                    padding-right: 17px;">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header" style="text-align: right">
                                                <asp:Button ID="Button19" Text="X" Font-Names="Verdana" Font-Size="14pt" class="btn btn-danger"
                                                    OnClick="Close_ClickPlantWiseCategory" runat="server" /></button>
                                                <h4 class="modal-title" style="text-align: left">
                                                    PlantWise Category Sales Details ( From Date :
                                                    <asp:Label ID="Label43" runat="server" ForeColor="Red"></asp:Label>
                                                    - To Date :
                                                    <asp:Label ID="Label44" runat="server" ForeColor="Red"></asp:Label>
                                                    )</h4>
                                            </div>
                                            <div class="modal-body" id="div_PlantWiseCategory" style="height: 400px; overflow-y: scroll;">
                                                <asp:GridView ID="grdPlatWiseCategoryReport" CssClass="table table-bordered table-hover dataTable no-footer"
                                                    runat="server" Style="width: 100%; text-align: right" Font-Bold="true" OnRowCommand="grdPlatWiseCategoryReport_RowCommand"
                                                    OnRowDataBound="grdPlatWiseCategoryReport_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Button ID="Button20" runat="server" Text="View" CssClass="btn btn-primary" CommandArgument='<%#Container.DataItemIndex%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <!-- /.modal-content -->
                                    </div>
                                    <!-- /.modal-dialog -->
                                </div>
                                <div class="modal fade in" id="div_MainPlantWiseSubCategory" runat="server" style="display: none;
                                    padding-right: 17px;">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header" style="text-align: right">
                                                <asp:Button ID="Button21" Text="X" Font-Names="Verdana" Font-Size="14pt" class="btn btn-danger"
                                                    OnClick="Close_ClickPlantWiseSubCategory" runat="server" /></button>
                                                <h4 class="modal-title" style="text-align: left">
                                                    PlantWise SubCategory Sales Details ( From Date :
                                                    <asp:Label ID="Label45" runat="server" ForeColor="Red"></asp:Label>
                                                    - To Date :
                                                    <asp:Label ID="Label46" runat="server" ForeColor="Red"></asp:Label>
                                                    )</h4>
                                            </div>
                                            <div class="modal-body" id="div6" style="height: 400px; overflow-y: scroll;">
                                                <asp:GridView ID="grdPlatWiseSubCategoryReport" CssClass="table table-bordered table-hover dataTable no-footer"
                                                    runat="server" Style="width: 100%; text-align: right" Font-Bold="true" OnRowDataBound="grdPlatWiseSubCategoryReport_RowDataBound">
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <!-- /.modal-content -->
                                    </div>
                                    <!-- /.modal-dialog -->
                                </div>
                            </div>
                            <div id="divHide" style="width: 120%; display: none;">
                                <div class="modal fade in" id="div_MainOthersCategory" style="display: none; padding-right: 17px;">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                    <span aria-hidden="true" onclick="OthersCategoryClick();">×</span></button>
                                                <h4 class="modal-title">
                                                    Category Details ( From Date :
                                                    <asp:Label ID="Label3" runat="server" ForeColor="Red"></asp:Label>
                                                    - To Date :
                                                    <asp:Label ID="Label4" runat="server" ForeColor="Red"></asp:Label>
                                                    )</h4>
                                            </div>
                                            <div style="display: none;">
                                            </div>
                                            <div class="modal-body" id="div_OthersCategory" style="height: 400px; overflow-y: scroll;">
                                            </div>
                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-danger" onclick="OthersCategoryClick();">
                                                    Close</button>
                                            </div>
                                        </div>
                                        <!-- /.modal-content -->
                                    </div>
                                    <!-- /.modal-dialog -->
                                </div>
                                <div class="modal fade in" id="div_MainGroupOthersProducts" style="display: none;
                                    padding-right: 17px;">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                    <span aria-hidden="true" onclick="CloseClick1();">×</span></button>
                                                <h4 class="modal-title">
                                                    Product Details ( From Date :
                                                    <asp:Label ID="Label5" runat="server" ForeColor="Red"></asp:Label>
                                                    - To Date :
                                                    <asp:Label ID="Label6" runat="server" ForeColor="Red"></asp:Label>
                                                    )</h4>
                                            </div>
                                            <div style="display: none;">
                                            </div>
                                            <div class="modal-body" id="div_GroupOthersProducts" style="height: 400px; overflow-y: scroll;">
                                            </div>
                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-danger" onclick="OthersGroupProductsClick();">
                                                    Close</button>
                                            </div>
                                        </div>
                                        <!-- /.modal-content -->
                                    </div>
                                    <!-- /.modal-dialog -->
                                </div>
                                <div class="modal fade in" id="divMainAddNewRow1" style="display: none; padding-right: 17px;">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                    <span aria-hidden="true" onclick="CloseClick1();">×</span></button>
                                                <h4 class="modal-title">
                                                    Branch Wise Details ( From Date :
                                                    <asp:Label ID="Label7" runat="server" ForeColor="Red"></asp:Label>
                                                    - To Date :
                                                    <asp:Label ID="Label8" runat="server" ForeColor="Red"></asp:Label>
                                                    )</h4>
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
                                <div class="modal fade in" id="div_MainPlantDetails" style="display: none; padding-right: 17px;">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                    <span aria-hidden="true" onclick="CloseClick1();">×</span></button>
                                                <h4 class="modal-title">
                                                    Plant Wise Details ( From Date :
                                                    <asp:Label ID="Label9" runat="server" ForeColor="Red"></asp:Label>
                                                    - To Date :
                                                    <asp:Label ID="Label10" runat="server" ForeColor="Red"></asp:Label>
                                                    )</h4>
                                            </div>
                                            <div style="display: none;">
                                            </div>
                                            <div class="modal-body" id="div_PlantDetails" style="height: 400px; overflow-y: scroll;">
                                            </div>
                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-danger" onclick="PlantCloseClick();">
                                                    Close</button>
                                            </div>
                                        </div>
                                        <!-- /.modal-content -->
                                    </div>
                                    <!-- /.modal-dialog -->
                                </div>
                            </div>
                            <div class="modal fade in" id="div_routewisemain" style="display: none; padding-right: 17px;
                                width: 110%;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true" onclick="close_routewise();">×</span></button>
                                            <h4 class="modal-title">
                                                Route Wise Details ( From Date :
                                                <asp:Label ID="Label11" runat="server" ForeColor="Red"></asp:Label>
                                                - To Date :
                                                <asp:Label ID="Label12" runat="server" ForeColor="Red"></asp:Label>
                                                )</h4>
                                        </div>
                                        <div class="modal-body" id="div_routewise" style="height: 400px; overflow-y: scroll;">
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-danger" onclick="close_routewise();">
                                                Close</button>
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>
                            <div class="modal fade in" id="div_routewisemainCompare" runat="server" style="display: none;
                                padding-right: 17px; width: 110%;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header" style="text-align: right">
                                            <asp:Button ID="Button5" Text="X" Font-Names="Verdana" Font-Size="14pt" class="btn btn-danger"
                                                OnClick="Close_Click2" runat="server" /></button>
                                            <h4 class="modal-title" style="text-align: left">
                                                Route Wise Avg Qty ( From Date :
                                                <asp:Label ID="Label13" runat="server" ForeColor="Red"></asp:Label>
                                                - To Date :
                                                <asp:Label ID="Label14" runat="server" ForeColor="Red"></asp:Label>
                                                )</h4>
                                        </div>
                                        <div style="text-align: left; padding-right: 99px; font-size: 21px;">
                                            <table style="width:100%;">
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton8" Text="View" CssClass="small-box-footer" OnClick="LinkButton8_Click"
                                                            runat="server">View RouteWise<i class="fa fa-arrow-circle-right"></i></asp:LinkButton>
                                                            </td>
                                                            <td>
                                                        <asp:LinkButton ID="LinkButton11" Text="View LineChart" CssClass="small-box-footer"
                                                            OnClick="LinkButton11_Click" runat="server">View CategoryWise<i class="fa fa-arrow-circle-right"></i></asp:LinkButton>
                                                            </td>
                                                            <td>
                                                        <asp:LinkButton ID="LinkButton5" Text="View LineChart" CssClass="small-box-footer"
                                                            OnClick="LinkButton5_Click" runat="server">View LineChart<i class="fa fa-arrow-circle-right"></i></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="modal-body" id="div_routewiseCompare" runat="server" style="height: 400px;
                                            overflow-y: scroll;">
                                            <asp:GridView ID="grdRouteReport" runat="server" CssClass="table table-bordered table-hover dataTable no-footer"
                                                Style="width: 100%; text-align: right" Font-Bold="true" OnRowDataBound="grdRouteReport_RowDataBound"
                                                OnRowCreated="grdRouteReport_RowCreated" OnRowCommand="grdRouteReport_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="Button6" runat="server" Text="View" CssClass="btn btn-primary" CommandArgument='<%#Container.DataItemIndex%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>
                            <div class="modal fade in" id="div_MainSalesOfficeCategory" runat="server" style="display: none;
                                padding-right: 17px; width: 110%;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header" style="text-align: right">
                                            <asp:Button ID="Button16" Text="X" Font-Names="Verdana" Font-Size="14pt" class="btn btn-danger"
                                                OnClick="Close_ClickSalesOfficeCategory" runat="server" /></button>
                                            <h4 class="modal-title" style="text-align: left">
                                                SalesOfficeWise Category Sales Details ( From Date :
                                                <asp:Label ID="Label39" runat="server" ForeColor="Red"></asp:Label>
                                                - To Date :
                                                <asp:Label ID="Label40" runat="server" ForeColor="Red"></asp:Label>
                                                )</h4>
                                        </div>
                                        <div class="modal-body" id="div_SalesOfficeCategory" runat="server" style="height: 400px;
                                            overflow-y: scroll;">
                                            <asp:GridView ID="grdSalesOfficeCategoryReport" runat="server" CssClass="table table-bordered table-hover dataTable no-footer"
                                                Style="width: 100%; text-align: right" Font-Bold="true" OnRowDataBound="grdSalesOfficeCategoryReport_RowDataBound"
                                                OnRowCommand="grdSalesOfficeCategoryReport_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="Button17" runat="server" Text="View" CssClass="btn btn-primary" CommandArgument='<%#Container.DataItemIndex%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>
                            <div class="modal fade in" id="div_MainSalesOfficeSubCategory" runat="server" style="display: none;
                                padding-right: 17px; width: 110%;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header" style="text-align: right">
                                            <asp:Button ID="Button18" Text="X" Font-Names="Verdana" Font-Size="14pt" class="btn btn-danger"
                                                OnClick="Close_ClickSalesOfficeSubCategory" runat="server" /></button>
                                            <h4 class="modal-title" style="text-align: left">
                                                SalesOfficeWise SubCategory Sales Details ( From Date :
                                                <asp:Label ID="Label41" runat="server" ForeColor="Red"></asp:Label>
                                                - To Date :
                                                <asp:Label ID="Label42" runat="server" ForeColor="Red"></asp:Label>
                                                )</h4>
                                        </div>
                                        <div class="modal-body" id="div_SalesOfficeSubCategory" runat="server" style="height: 400px;
                                            overflow-y: scroll;">
                                            <asp:GridView ID="grdSalesOfficeSubCategoryReport" runat="server" CssClass="table table-bordered table-hover dataTable no-footer"
                                                Style="width: 100%; text-align: right" Font-Bold="true" OnRowDataBound="grdSalesOfficeSubCategoryReport_RowDataBound">
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>
                            <div class="modal fade in" id="div_mainRouteSalesType" runat="server" style="display: none;
                                padding-right: 17px; width: 110%;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header" style="text-align: right">
                                            <asp:Button ID="Button9" Text="X" Font-Names="Verdana" Font-Size="14pt" class="btn btn-danger"
                                                OnClick="Close_RouteSalesTypeClick2" runat="server" /></button>
                                            <h4 class="modal-title" style="text-align: left">
                                                Route Wise Avg Qty ( From Date :
                                                <asp:Label ID="Label27" runat="server" ForeColor="Red"></asp:Label>
                                                - To Date :
                                                <asp:Label ID="Label28" runat="server" ForeColor="Red"></asp:Label>
                                                )</h4>
                                        </div>
                                        <div class="modal-body" id="div_RouteSalesType" runat="server" style="height: 400px;
                                            overflow-y: scroll;">
                                            <asp:GridView ID="GrdRouteSalesType" runat="server" CssClass="table table-bordered table-hover dataTable no-footer"
                                                Style="width: 100%; text-align: right" Font-Bold="true" OnRowCommand="GrdRouteSalesType_RowCommand"
                                                OnRowDataBound="GrdRouteSalesType_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="Button11" runat="server" Text="View" CssClass="btn btn-primary" CommandArgument='<%#Container.DataItemIndex%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>
                            <div class="modal fade in" id="divnewroute" runat="server" style="display: none;
                                padding-right: 17px; width: 110%;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header" style="text-align: right">
                                            <asp:Button ID="Button4" Text="X" Font-Names="Verdana" Font-Size="14pt" class="btn btn-danger"
                                                OnClick="Close_Click4" runat="server" /></button>
                                            <h4 class="modal-title" style="text-align: left">
                                                Route Wise Avg Qty ( From Date :
                                                <asp:Label ID="Label15" runat="server" ForeColor="Red"></asp:Label>
                                                - To Date :
                                                <asp:Label ID="Label16" runat="server" ForeColor="Red"></asp:Label>
                                                )</h4>
                                        </div>
                                        <div style="text-align: right; padding-right: 99px; font-size: 21px;">
                                            <asp:LinkButton ID="LinkButton6" Text="View LineChart" CssClass="small-box-footer"
                                                OnClick="LinkButton6_Click" runat="server">View LineChart<i class="fa fa-arrow-circle-right"></i></asp:LinkButton>
                                        </div>
                                        <div class="modal-body" id="div4" runat="server" style="height: 400px; overflow-y: scroll;">
                                            <asp:GridView ID="grdnewroute" runat="server" CssClass="table table-bordered table-hover dataTable no-footer"
                                                Style="width: 100%; text-align: right" Font-Bold="true" OnRowDataBound="grdnewroute_RowDataBound"
                                                OnRowCreated="grdnewroute_RowCreated" OnRowCommand="grdnewroute_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="Button7" runat="server" Text="View" CssClass="btn btn-primary" CommandArgument='<%#Container.DataItemIndex%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>
                            <div class="modal fade in" id="divMainRouteUnderSalestype" runat="server" style="display: none;
                                padding-right: 17px; width: 110%;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header" style="text-align: right">
                                            <asp:Button ID="Button10" Text="X" Font-Names="Verdana" Font-Size="14pt" class="btn btn-danger"
                                                OnClick="Close_RouteUnderSalestype" runat="server" /></button>
                                            <h4 class="modal-title" style="text-align: left">
                                                Route Wise SalesType Qty ( From Date :
                                                <asp:Label ID="Label29" runat="server" ForeColor="Red"></asp:Label>
                                                - To Date :
                                                <asp:Label ID="Label30" runat="server" ForeColor="Red"></asp:Label>
                                                )</h4>
                                        </div>
                                        <div style="text-align: right; padding-right: 99px; font-size: 21px;">
                                            <asp:LinkButton ID="LinkButton10" Text="View LineChart" CssClass="small-box-footer"
                                                OnClick="LinkButton9_Click" runat="server">View SubCategoryWise<i class="fa fa-arrow-circle-right"></i></asp:LinkButton>
                                        </div>
                                        <div class="modal-body" id="divRouteUnderSalestype" runat="server" style="height: 400px;
                                            overflow-y: scroll;">
                                            <asp:GridView ID="grdRouteUnderSalesType" runat="server" CssClass="table table-bordered table-hover dataTable no-footer"
                                                Style="width: 100%; text-align: right" Font-Bold="true" OnRowCommand="grdRouteUnderSalesType_RowCommand"
                                                OnRowCreated="grdRouteUnderSalesType_RowCreated" OnRowDataBound="grdRouteUnderSalesType_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="Button12" runat="server" Text="View" CssClass="btn btn-primary" CommandArgument='<%#Container.DataItemIndex%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>
                            <div class="modal fade in" id="divMainRouteUnderCategory" runat="server" style="display: none;
                                padding-right: 17px; width: 110%;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header" style="text-align: right">
                                            <asp:Button ID="Button14" Text="X" Font-Names="Verdana" Font-Size="14pt" class="btn btn-danger"
                                                OnClick="Close_RouteUnderCategory" runat="server" /></button>
                                            <h4 class="modal-title" style="text-align: left">
                                                Route Wise SubCategory ( From Date :
                                                <asp:Label ID="Label35" runat="server" ForeColor="Red"></asp:Label>
                                                - To Date :
                                                <asp:Label ID="Label36" runat="server" ForeColor="Red"></asp:Label>
                                                )</h4>
                                        </div>
                                        <div class="modal-body" id="divRouteUnderCategory" runat="server" style="height: 400px;
                                            overflow-y: scroll;">
                                            <asp:GridView ID="grdRouteUnderCategory" runat="server" CssClass="table table-bordered table-hover dataTable no-footer"
                                                Style="width: 100%; text-align: right" Font-Bold="true" OnRowCommand="grdRouteUnderCategory_RowCommand"
                                                OnRowDataBound="grdRouteUnderCategory_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="Button15" runat="server" Text="View" CssClass="btn btn-primary" CommandArgument='<%#Container.DataItemIndex%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>
                            <div class="modal fade in" id="divMainAgentUnderCategoryProduct" runat="server" style="display: none;
                                padding-right: 17px; width: 110%;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header" style="text-align: right">
                                            <asp:Button ID="Button15" Text="X" Font-Names="Verdana" Font-Size="14pt" class="btn btn-danger"
                                                OnClick="Close_AgentUnderCategoryProduct" runat="server" /></button>
                                            <h4 class="modal-title" style="text-align: left">
                                                Agent Wise Products ( From Date :
                                                <asp:Label ID="Label37" runat="server" ForeColor="Red"></asp:Label>
                                                - To Date :
                                                <asp:Label ID="Label38" runat="server" ForeColor="Red"></asp:Label>
                                                )</h4>
                                        </div>
                                        <div class="modal-body" id="divAgentUnderCategoryProduct" runat="server" style="height: 400px;
                                            overflow-y: scroll;">
                                            <asp:GridView ID="grdAgentUnderCategoryProduct" runat="server" CssClass="table table-bordered table-hover dataTable no-footer"
                                                Style="width: 100%; text-align: right" Font-Bold="true" OnRowDataBound="grdAgentUnderCategoryProduct_RowDataBound">
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>
                            <div class="modal fade in" id="divMainRouteWiseUnderSalestype" runat="server" style="display: none;
                                padding-right: 17px; width: 110%;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header" style="text-align: right">
                                            <asp:Button ID="Button11" Text="X" Font-Names="Verdana" Font-Size="14pt" class="btn btn-danger"
                                                OnClick="Close_RouteWiseUnderSalestype" runat="server" /></button>
                                            <h4 class="modal-title" style="text-align: left">
                                                Agent Wise SalesType Qty ( From Date :
                                                <asp:Label ID="Label31" runat="server" ForeColor="Red"></asp:Label>
                                                - To Date :
                                                <asp:Label ID="Label32" runat="server" ForeColor="Red"></asp:Label>
                                                )</h4>
                                        </div>
                                        <div class="modal-body" id="divRouteWiseUnderSalestype" runat="server" style="height: 400px;
                                            overflow-y: scroll;">
                                            <asp:GridView ID="grdRouteWiseUnderSalesType" runat="server" CssClass="table table-bordered table-hover dataTable no-footer"
                                                Style="width: 100%; text-align: right" Font-Bold="true" OnRowCommand="grdRouteWiseUnderSalesType_RowCommand"
                                                OnRowCreated="grdRouteWiseUnderSalesType_RowCreated" OnRowDataBound="grdRouteWiseUnderSalesType_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="Button13" runat="server" Text="View" CssClass="btn btn-primary" CommandArgument='<%#Container.DataItemIndex%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>
                            <div class="modal fade in" id="div_agentwisemain1" runat="server" style="display: none;
                                padding-right: 17px;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header" style="text-align: right">
                                            <asp:Button ID="Button8" Text="X" Font-Names="Verdana" Font-Size="14pt" class="btn btn-danger"
                                                OnClick="Close_Click3" runat="server" /></button>
                                            <h4 class="modal-title" style="text-align: left">
                                                Agent Wise Avg Qty ( From Date :
                                                <asp:Label ID="Label17" runat="server" ForeColor="Red"></asp:Label>
                                                - To Date :
                                                <asp:Label ID="Label18" runat="server" ForeColor="Red"></asp:Label>
                                                )</h4>
                                        </div>
                                        <div style="text-align: right; padding-right: 99px; font-size: 21px;">
                                            <asp:LinkButton ID="LinkButton7" Text="View LineChart" CssClass="small-box-footer"
                                                OnClick="LinkButton7_Click" runat="server">View LineChart<i class="fa fa-arrow-circle-right"></i></asp:LinkButton>
                                        </div>
                                        <div class="modal-body" id="div_agentwise1" runat="server" style="height: 400px;
                                            overflow-y: scroll;">
                                            <asp:GridView ID="grdAgentReport" CssClass="table table-bordered table-hover dataTable no-footer"
                                                runat="server" Style="width: 100%; text-align: right" Font-Bold="true" OnRowDataBound="grdAgentReport_RowDataBound"
                                                OnRowCreated="grdRouteReport_RowCreated" OnRowCommand="grdAgentReport_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="Button8" runat="server" Text="View" CssClass="btn btn-primary" CommandArgument='<%#Container.DataItemIndex%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>
                            <div class="modal fade in" id="divMainRouteWiseAgentUnderSalestype" runat="server"
                                style="display: none; padding-right: 17px; width: 110%;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header" style="text-align: right">
                                            <asp:Button ID="Button12" Text="X" Font-Names="Verdana" Font-Size="14pt" class="btn btn-danger"
                                                OnClick="Close_RouteWiseAgentUnderSalestype" runat="server" /></button>
                                            <h4 class="modal-title" style="text-align: left">
                                                Agent Product Wise Sales Qty ( From Date :
                                                <asp:Label ID="Label33" runat="server" ForeColor="Red"></asp:Label>
                                                - To Date :
                                                <asp:Label ID="Label34" runat="server" ForeColor="Red"></asp:Label>
                                                )</h4>
                                        </div>
                                        <div class="modal-body" id="divRouteWiseAgentUnderSalestype" runat="server" style="height: 400px;
                                            overflow-y: scroll;">
                                            <asp:GridView ID="grdRouteWiseAgentUnderSalesType" runat="server" CssClass="table table-bordered table-hover dataTable no-footer"
                                                Style="width: 100%; text-align: right" Font-Bold="true" OnRowCommand="grdRouteWiseAgentUnderSalesType_RowCommand">
                                                <%--<Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="Button7" runat="server" Text="View" CssClass="btn btn-primary" CommandArgument='<%#Container.DataItemIndex%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>--%>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>
                            <div class="modal fade in" id="divagentwisesale" runat="server" style="display: none;
                                padding-right: 17px;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header" style="text-align: right">
                                            <asp:Button ID="Button7" Text="X" Font-Names="Verdana" Font-Size="14pt" class="btn btn-danger"
                                                OnClick="Close_Click5" runat="server" /></button>
                                            <h4 class="modal-title" style="text-align: left">
                                                Day wise Agent sale Qty ( From Date :
                                                <asp:Label ID="Label19" runat="server" ForeColor="Red"></asp:Label>
                                                - To Date :
                                                <asp:Label ID="Label20" runat="server" ForeColor="Red"></asp:Label>
                                                )</h4>
                                        </div>
                                        <div style="text-align: right; padding-right: 99px; font-size: 21px;">
                                            <asp:LinkButton ID="LinkButton4" Text="View LineChart" CssClass="small-box-footer"
                                                OnClick="LinkButton4_Click" runat="server">View LineChart<i class="fa fa-arrow-circle-right"></i></asp:LinkButton>
                                        </div>
                                        <div class="modal-body" id="div5" runat="server" style="height: 400px; overflow-y: scroll;">
                                            <asp:GridView ID="grdagentwisesale" CssClass="table table-bordered table-hover dataTable no-footer"
                                                runat="server" Style="width: 100%; text-align: right" Font-Bold="true">
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>
                            <div class="modal fade in" id="div_agentwisemain" style="display: none; padding-right: 17px;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true" onclick="close_agentwise();">×</span></button>
                                            <h4 class="modal-title">
                                                Agent Wise Details ( From Date :
                                                <asp:Label ID="Label21" runat="server" ForeColor="Red"></asp:Label>
                                                - To Date :
                                                <asp:Label ID="Label22" runat="server" ForeColor="Red"></asp:Label>
                                                )</h4>
                                        </div>
                                        <div class="modal-body" id="div_agentwise" style="height: 400px; overflow-y: scroll;">
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-danger" onclick="close_agentwise();">
                                                Close</button>
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>
                            <div class="modal fade in" id="example" style="display: none; padding-right: 17px;
                                width: 100%; height: 800px;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true" onclick="close_AgentProductLineChart();">×</span></button>
                                            <h4 class="modal-title">
                                                Day Wise Product Details ( From Date :
                                                <asp:Label ID="Label23" runat="server" ForeColor="Red"></asp:Label>
                                                - To Date :
                                                <asp:Label ID="Label24" runat="server" ForeColor="Red"></asp:Label>
                                                )</h4>
                                        </div>
                                        <div style="height: 400px; overflow-y: scroll;">
                                            <div class="modal-body" id="ProductWiseChart" style="height: 400px;">
                                            </div>
                                            <div class="modal-body" id="AgentDayProductPieChart" style="height: 400px;">
                                            </div>
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>
                            <div class="modal fade in" id="divMainAddNewRow" style="display: none; padding-right: 17px;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true" onclick="PopupCloseClick();">×</span></button>
                                            <h4 class="modal-title">
                                                Route Wise Details ( From Date :
                                                <asp:Label ID="Label25" runat="server" ForeColor="Red"></asp:Label>
                                                - To Date :
                                                <asp:Label ID="Label26" runat="server" ForeColor="Red"></asp:Label>
                                                )</h4>
                                        </div>
                                        <div class="modal-body" id="divRouteDetails" style="height: 400px; overflow-y: scroll;">
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-danger" onclick="PopupCloseClick();">
                                                Close</button>
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>



                            <div class="modal fade in" id="div3" style="display: none; padding-right: 17px;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true" onclick="PopupCloseClick();">×</span></button>
                                            <h4 class="modal-title">
                                                Route Wise Details ( From Date :
                                                <asp:Label ID="Label47" runat="server" ForeColor="Red"></asp:Label>
                                                - To Date :
                                                <asp:Label ID="Label48" runat="server" ForeColor="Red"></asp:Label>
                                                )</h4>
                                        </div>
                                        <div class="modal-body" id="div7" style="height: 400px; overflow-y: scroll;">
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-danger" onclick="PopupCloseClick();">
                                                Close</button>
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddltype" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
