<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DashBoardAgentLineChart.aspx.cs" Inherits="DashBoardAgentLineChart" %>
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

      var LineChartType= '<%=Session["LineChartType"] %>';
      if(LineChartType == "SalesOffice")
        {
          $('#divSalesOffice').css('display','block');
            $('#divBranchSalesType').css('display','none');
            $('#divRoute').css('display','none');
            $('#firstdiv').css('display','none');
            btnSalesOfficeBetweenDayProductComparison();
        }
        else if(LineChartType =="BranchSalesType")
        {
          $('#divSalesOffice').css('display','none');
            $('#divBranchSalesType').css('display','block');
            $('#divRoute').css('display','none');
            $('#firstdiv').css('display','none');
            btnSalesTypeBetweenDayProductComparison();
        }
         else if(LineChartType =="RouteWise")
        {
          $('#divSalesOffice').css('display','none');
            $('#divBranchSalesType').css('display','none');
            $('#divRoute').css('display','block');
            $('#firstdiv').css('display','none');
            btnRouteWiseBetweenDayProductComparison();
        }
         else if(LineChartType =="AgentWise")
        {
           btnAgentWiseBetweenDayProductComparison();
           $('#divSalesOffice').css('display','none');
            $('#divBranchSalesType').css('display','none');
            $('#divRoute').css('display','none');
            $('#firstdiv').css('display','block');
        }
       });
        var TotalProductclass=[];var DaywiseAgenttSale=[];
       function btnAgentWiseBetweenDayProductComparison() {
       document.getElementById("spnAgentName").innerHTML='<%=Session["AgentName"] %>';
       document.getElementById("spnToDate4").innerHTML='<%=Session["To_Date"] %>';
       document.getElementById("spnFromDate4").innerHTML='<%=Session["FromDate"] %>';
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
    //Sales Office Wise

       var TotalSalesOfficeProductclass=[];var DaywiseSalesOfficeSale=[];
       function btnSalesOfficeBetweenDayProductComparison() {
       document.getElementById("spnSalesOffice").innerHTML='<%=Session["SalesOfficeName"] %>';


       document.getElementById("spnToDate1").innerHTML='<%=Session["LDTD"] %>';
       document.getElementById("spnFromDate1").innerHTML='<%=Session["LDFD"] %>';


        var data = { 'operation': 'Get_SalesOfficeBetweenDayProduct_Comparison' };
        var s = function (msg) {
            if (msg) {
                if (msg == "Session Expired") {
                    alert(msg);
                    window.location = "Login.aspx";
                }
                 TotalSalesOfficeProductclass = msg[0].TotalProductclass;
                DaywiseSalesOfficeSale= msg[0].linechartvaluesclass;
                FillSalesOfficeProductDayComparisonDetails(DaywiseSalesOfficeSale);
                FillSalesOfficeProductDayProductPiechart(TotalSalesOfficeProductclass);
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
       function FillSalesOfficeProductDayProductPiechart(databind) {
           newXarray = [];
           if (databind.length > 0) {
                   var RouteName = databind[0].CategeoryName;
                   var leakqty = databind[0].Qty;
                   var color = ["#794b4b", "#FF6600", "#fdd400", "#84b761", "#cc4748", "#cd82ad", "#2f4074", "#448e4d", "#b7b83f", "#b9783f", "#b93e3d", "#913167", "#18d79c", "#a2907f", "#41ee80", "#8cfa40", "#dbdf9d", "#3f4fb2", "#01909b", "#124c65", "#33879b", "#869ae2", "#000000"];
                   for (var i = 0; i < databind.length; i++) {
                       newXarray.push({ "category": databind[i].CategeoryName, "value": parseFloat(databind[i].Qty), "color": color[i] });
                   }
               }
               var chart = AmCharts.makeChart("divSalesOfficePieChart", {
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
        function FillSalesOfficeProductDayComparisonDetails(databind) {
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
        var chart = AmCharts.makeChart("divSalesOfficeLineChart", {
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

    //SalesOffice SalesType Wise
       var TotalSalesTypeProductclass=[];var DaywiseSalesTypeSale=[];
       function btnSalesTypeBetweenDayProductComparison() {
       document.getElementById("spnSalesType").innerHTML='<%=Session["SalesTypeName"] %>';
       document.getElementById("spnToDate2").innerHTML='<%=Session["To_Date"] %>';
       document.getElementById("spnFromDate2").innerHTML='<%=Session["FromDate"] %>';
        var data = { 'operation': 'Get_SalesTypeBetweenDayProduct_Comparison' };
        var s = function (msg) {
            if (msg) {
                if (msg == "Session Expired") {
                    alert(msg);
                    window.location = "Login.aspx";
                }
                TotalSalesTypeProductclass = msg[0].TotalProductclass;
                DaywiseSalesTypeSale= msg[0].linechartvaluesclass;
                FillSalesTypeProductDayComparisonDetails(DaywiseSalesTypeSale);
                FillSalesTypeProductDayProductPiechart(TotalSalesTypeProductclass);
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
       function FillSalesTypeProductDayProductPiechart(databind) {
           newXarray = [];
           if (databind.length > 0) {
                   var RouteName = databind[0].CategeoryName;
                   var leakqty = databind[0].Qty;
                   var color = ["#794b4b", "#FF6600", "#fdd400", "#84b761", "#cc4748", "#cd82ad", "#2f4074", "#448e4d", "#b7b83f", "#b9783f", "#b93e3d", "#913167", "#18d79c", "#a2907f", "#41ee80", "#8cfa40", "#dbdf9d", "#3f4fb2", "#01909b", "#124c65", "#33879b", "#869ae2", "#000000"];
                   for (var i = 0; i < databind.length; i++) {
                       newXarray.push({ "category": databind[i].CategeoryName, "value": parseFloat(databind[i].Qty), "color": color[i] });
                   }
               }
               var chart = AmCharts.makeChart("divSalesTypePieChart", {
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
        function FillSalesTypeProductDayComparisonDetails(databind) {
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
        var chart = AmCharts.makeChart("divSalesTypeLineChart", {
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
    //Route Wise
       var TotalRouteWiseTypeProductclass=[];var DaywiseRouteWiseSale=[];
       function btnRouteWiseBetweenDayProductComparison() {
       document.getElementById("spnRouteName").innerHTML='<%=Session["RouteName"] %>';
       document.getElementById("spnToDate3").innerHTML='<%=Session["To_Date"] %>';
       document.getElementById("spnFromDate3").innerHTML='<%=Session["FromDate"] %>';
        var data = { 'operation': 'Get_RouteWiseBetweenDayProduct_Comparison' };
        var s = function (msg) {
            if (msg) {
                if (msg == "Session Expired") {
                    alert(msg);
                    window.location = "Login.aspx";
                }
                  TotalRouteWiseTypeProductclass = msg[0].TotalProductclass;
                DaywiseRouteWiseSale= msg[0].linechartvaluesclass;
                FillRouteWiseProductDayComparisonDetails(DaywiseRouteWiseSale);
                FillRouteWiseProductDayProductPiechart(TotalRouteWiseTypeProductclass);
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
       function FillRouteWiseProductDayProductPiechart(databind) {
           newXarray = [];
           if (databind.length > 0) {
                   var RouteName = databind[0].CategeoryName;
                   var leakqty = databind[0].Qty;
                   var color = ["#794b4b", "#FF6600", "#fdd400", "#84b761", "#cc4748", "#cd82ad", "#2f4074", "#448e4d", "#b7b83f", "#b9783f", "#b93e3d", "#913167", "#18d79c", "#a2907f", "#41ee80", "#8cfa40", "#dbdf9d", "#3f4fb2", "#01909b", "#124c65", "#33879b", "#869ae2", "#000000"];
                   for (var i = 0; i < databind.length; i++) {
                       newXarray.push({ "category": databind[i].CategeoryName, "value": parseFloat(databind[i].Qty), "color": color[i] });
                   }
               }
               var chart = AmCharts.makeChart("divRouteWisePieChart", {
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
        function FillRouteWiseProductDayComparisonDetails(databind) {
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
        var chart = AmCharts.makeChart("divRouteWiseLineChart", {
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
                    Day wise Agent sale Qty<small>Preview</small>
                </h1>
                <ol class="breadcrumb">
                    <li><a href="#"><i class="fa fa-dashboard"></i>Chart Report</a></li>
                    <li><a href="#">Day wise Agent sale Qty</a></li>
                </ol>
            </section>
            <section class="content">
                <div class="box box-info">
                    <div class="box-body">
                        <div style="width: 100%; padding-left: 15%;">
                            
                            <div id="divSalesOffice" style="display:none;">
                                <h4 class="modal-title" style="text-align: left">
                                    <label>
                                        Sales Office Name:-</label><span style="color: red; font-weight: bold" id="spnSalesOffice"></span>
                                    ( From Date : <span style="color: red; font-weight: bold" id="spnFromDate1"></span>
                                    - To Date : <span style="color: red; font-weight: bold" id="spnToDate1">*</span>
                                    )</h4>
                                <div class="modal-body" id="divSalesOfficeLineChart" style="height: 400px;">
                                </div>
                                <div class="modal-body" id="divSalesOfficePieChart" style="height: 400px;">
                                </div>
                            </div>
                            <div id="divBranchSalesType" style="display:none;">
                                <h4 class="modal-title" style="text-align: left">
                                    <label>
                                        SalesType:-</label><span style="color: red; font-weight: bold" id="spnSalesType"></span>
                                    ( From Date : <span style="color: red; font-weight: bold" id="spnFromDate2"></span>
                                    - To Date : <span style="color: red; font-weight: bold" id="spnToDate2">*</span>
                                    )</h4>
                                <div class="modal-body" id="divSalesTypeLineChart" style="height: 400px;">
                                </div>
                                <div class="modal-body" id="divSalesTypePieChart" style="height: 400px;">
                                </div>
                            </div>
                            <div id="divRoute" style="display:none;">
                                <h4 class="modal-title" style="text-align: left">
                                    <label>
                                        Route Name:-</label><span style="color: red; font-weight: bold" id="spnRouteName"></span>
                                    ( From Date : <span style="color: red; font-weight: bold" id="spnFromDate3"></span>
                                    - To Date : <span style="color: red; font-weight: bold" id="spnToDate3">*</span>
                                    )</h4>
                                <div class="modal-body" id="divRouteWiseLineChart" style="height: 400px;">
                                </div>
                                <div class="modal-body" id="divRouteWisePieChart" style="height: 400px;">
                                </div>
                            </div>
                            <div id="firstdiv" style="display:none;">
                                <h4 class="modal-title" style="text-align: left">
                                    <label>
                                        Agent Name:-</label><span style="color: red; font-weight: bold" id="spnAgentName"></span>
                                    ( From Date : <span style="color: red; font-weight: bold" id="spnFromDate4"></span>
                                    - To Date : <span style="color: red; font-weight: bold" id="spnToDate4">*</span>
                                    )</h4>
                                <div class="modal-body" id="ProductWiseChart" style="height: 400px;">
                                </div>
                                <div class="modal-body" id="AgentDayProductPieChart" style="height: 400px;">
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
    </asp:UpdatePanel>
</asp:Content>
