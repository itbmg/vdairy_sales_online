<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GraphicalNetSale.aspx.cs"
    Inherits="RadialGauge" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="icon" href="Images/Vyshnavilogo.png" type="image/x-icon" title=BMG />
    <title>Vyshnavi Dairy </title>
    <link rel="stylesheet" href="css/style.css?v=1113" type="text/css" media="all" />
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
    <link href="Css/bootstrap.min.css" rel="stylesheet" type="text/css"/>
    <%-- <script type="text/javascript" src="https://cdn.jsdelivr.net/jquery/latest/jquery.min.js"></script>--%>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/momentjs/latest/moment.min.js"></script>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.css" />

    <style>
table#tableaProductdetails {
    empty-cells:hide;
}
</style>

    <script type="text/javascript">

   


        //  function createGauge() {
        // $("#gauge").kendoRadialGauge({
        //                pointer: {
        //                    value: $("#gauge-value").val()
        //                },
        //                scale: {
        //                    minorUnit: 5,
        //                    startAngle: -30,
        //                    endAngle: 210,
        //                    max: MaxSale
        //                }
        //            });
        //        }
        var MaxSale = 0;
        var Type = "";
        function ddlTypeChange(ID) {
            Type = document.getElementById('ddlType').value;
            if (Type == "Plant Wise") {
                FillPlant();
                $('#divPlant').css('display', 'block');
            }
            else {
                MaxSale = 200;
                // Getgauge();
                $('#divPlant').css('display', 'none');

            }
        }
        var PlantName = "";
        function ddlPlantNameChange(ID) {
            PlantName = document.getElementById('ddlPlant').value;
            if (PlantName == "1801") {
                MaxSale = 60;
            }
            if (PlantName == "172") {
                MaxSale = 100;
            }
            if (PlantName == "7" || PlantName == "") {
                MaxSale = 80;
            }
            //            Getgauge();
        }
        //        function Getgauge() {
        //            createGauge();
        //            function updateValue() {
        //                $("#gauge").data("kendoRadialGauge").value($("#gauge-value").val());
        //            }
        //            if (kendo.ui.Slider) {
        //                $("#gauge-value").kendoSlider({
        //                    min: 0,
        //                    max: MaxSale,
        //                    showButtons: false,
        //                    change: updateValue
        //                });
        //            } else {
        //                $("#gauge-value").change(updateValue);
        //            }
        //            $(document).bind("kendo:skinChange", function (e) {
        //                createGauge();
        //            });
        //        }
        function FillPlant() {
            var data = { 'operation': 'GetSalesOffices', 'SelecteType': 'SVDS' };
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
            var ddlPlant = document.getElementById('ddlPlant');
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
        function Get_Dispatch_Sale_CategoryWise_Click() {
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            var PlantName = document.getElementById('ddlPlant').value;
            var data = { 'operation': 'Get_Dispatch_Sale_CategoryWise', 'BranchId': PlantName, 'FromDate': FromDate, 'Todate': Todate };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    fillcategorywisedispatchsale(msg);
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
            var DispatchQty = msg[0].DispatchQty;
            var SaleValue = msg[0].SaleValue;
            var MilkQty = msg[0].MilkQty;
            var CurdQty = msg[0].CurdQty;
            var OtherQty = msg[0].OtherQty;
            var collamount = msg[0].amount;
            var dueamount = msg[0].dueamount;
            //            var x = DispatchQty;
            //            x = x.toString();
            //            var lastThree = x.substring(x.length - 3);
            //            var otherNumbers = x.substring(0, x.length - 3);
            //            if (otherNumbers != '')
            //                lastThree = ',' + lastThree;
            //            var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;

            document.getElementById('hdispatchqty').innerHTML = DispatchQty + ' Ltrs';
            document.getElementById('hsalevalue').innerHTML = '₹ ' + SaleValue;
            document.getElementById('hmilkqty').innerHTML = MilkQty + ' Ltrs';
            document.getElementById('hcurdqty').innerHTML = CurdQty + ' Ltrs';
            document.getElementById('hothersqty').innerHTML = OtherQty;
            document.getElementById('spnamount').innerHTML = '₹ ' + collamount;
            document.getElementById('hduevalue').innerHTML = '₹ ' + dueamount;
        }

        //function branchwisedispqty() {
        //    var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        //    var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        //    var PlantName = document.getElementById('ddlPlant').value;
        //    $('#divHide').css('display', 'block');
        //    var data = { 'operation': 'branchwise_Dispatch_qty', 'BranchId': PlantName, 'FromDate': FromDate, 'Todate': Todate };
        //    var s = function (msg) {
        //        if (msg) {
        //            if (msg == "Session Expired") {
        //                alert(msg);
        //                window.location = "Login.aspx";
        //            }
        //            fillbranchwisedispqty(msg);
        //            //                    fillbranchwisesalevalue(msg);
        //        }
        //        else {
        //        }
        //    };
        //    var e = function (x, h, e) {
        //    };
        //    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        //    callHandler(data, s, e);
        //}

        //function fillbranchwisedispqty(msg) {
        //    scrollTo(0, 0);
        //    $('#divMainAddNewRow1').focus();
        //    $('#divMainAddNewRow1').css('display', 'block');
        //    j = 1;
        //    var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" border="1" role="grid" aria-describedby="example2_info">';
        //    results += '<thead><tr style="background-color: #abbed2;"><th style="text-align: center; ">Branch Name</th><th style="text-align: center; ">Dispatch Qty</th></tr></thead></tbody>';
        //    var k = 1;
        //    var l = 0;
        //    for (var i = 0; i < msg.length; i++) {
        //        results += '<tr style="background-color: antiquewhite;">';
        //        results += '<td style="text-align: center"  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnRoute_Wise_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
        //        results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
        //        l = l + 1;
        //        if (l == 30) {
        //            l = 0;
        //        }
        //    }
        //    results += '</table></div>';
        //    $("#divChart").html(results);
        //}
        function btnRoute_Wise_SaleQty(BranchId) {
            
            var BranchId;
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'Route_Wise_SaleValue', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    fillRouteWise_Sale_Qty(msg);

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function fillRouteWise_Sale_Qty(msg) {
            scrollTo(0, 0);
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
            j = 1;
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" border="1" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th >Route Name</th><th >Sale Qty</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color: antiquewhite;">';
                results += '<td class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnAgent_Wise_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                results += '<td class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].SaleQty + '</b></td></tr>';
                l = l + 1;
                if (l == 30) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#divChart").html(results);
        }
        function btnAgent_Wise_SaleQty(BranchId) {
           
            var BranchId;
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'Agent_Wise_SaleValue', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    fillAgentWise_Sale_Qty(msg);

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillAgentWise_Sale_Qty(msg) {
            scrollTo(0, 0);
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
            j = 1;
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" border="1" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th >Agent Name</th><th >Sale Qty</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color: antiquewhite;">';
                results += '<td class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"    class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].SaleQty + '</b></td></tr>';
                l = l + 1;
                if (l == 30) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#divChart").html(results);
        }
        function branchwisemilkdetails() {
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            var PlantName = document.getElementById('ddlPlant').value;
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'branchwise_Dispatch_milk_qty', 'BranchId': PlantName, 'FromDate': FromDate, 'Todate': Todate };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    fillbranchwise_Milk_dispqty(msg);
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
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
            j = 1;
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th >Milk Qty</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                results += '<td   class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnRoute_Wise_Milk_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
                l = l + 1;
                if (l == 30) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#divChart").html(results);
        }
        function btnRoute_Wise_Milk_SaleQty(BranchId) {
            var BranchId;
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'Route_Wise_Milk_SaleQty', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
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
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
            j = 1;
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th>Route Name</th><th>Milk Sale Qty</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color: antiquewhite;">';
                results += '<td   class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnAgent_Wise_Milk_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
                l = l + 1;
                if (l == 30) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#divChart").html(results);
        }
        function btnAgent_Wise_Milk_SaleQty(BranchId) {
            var BranchId;
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'Agent_Wise_Milk_SaleQty', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
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
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
            j = 1;
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th >Agent Name</th><th>Milk Sale Qty</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color: antiquewhite;">';
                results += '<td   class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"    class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
                l = l + 1;
                if (l == 30) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#divChart").html(results);
        }
        function branchwiseCurddetails() {
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            var PlantName = document.getElementById('ddlPlant').value;
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'branchwise_Curd_Dispatch_qty', 'BranchId': PlantName, 'FromDate': FromDate, 'Todate': Todate };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    fillbranchwise_Curd_dispqty(msg);
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

        function fillbranchwise_Curd_dispqty(msg) {
            scrollTo(0, 0);
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
            j = 1;
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th >Curd Dispatch Qty</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color: antiquewhite;">';
                results += '<td   class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnRoute_Wise_Curd_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                results += '<td class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
                l = l + 1;
                if (l == 30) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#divChart").html(results);
        }


        function btnRoute_Wise_Curd_SaleQty(BranchId) {
            var BranchId;
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'Route_Wise_Curd_SaleQty', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
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
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
            j = 1;
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th >Route Name</th><th >Curd Sale Qty</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color: antiquewhite;">';
                results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnAgent_Wise_Curd_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
                l = l + 1;
                if (l == 30) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#divChart").html(results);
        }
        function btnAgent_Wise_Curd_SaleQty(BranchId) {
            var BranchId;
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'Agent_Wise_Curd_SaleQty', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
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
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
            j = 1;
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th >Agent Name</th><th >Curd Sale Qty</th></tr></thead></tbody>';
            var k = 1; 
            var l = 0;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color: antiquewhite;">';
                results += '<td   class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"    class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
                l = l + 1;
                if (l == 30) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#divChart").html(results);
        }


        function branchwiseotherdetails() {
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            var PlantName = document.getElementById('ddlPlant').value;
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'branchwise_Others_Dispatch_qty', 'BranchId': PlantName, 'FromDate': FromDate, 'Todate': Todate };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    fillbranchwise_Others_dispqty(msg);
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

        function fillbranchwise_Others_dispqty(msg) {
            scrollTo(0, 0);
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
            j = 1;
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th >Other Dispatch Qty</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color: antiquewhite;">';
                results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnRoute_Wise_Others_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
                l = l + 1;
                if (l == 30) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#divChart").html(results);
        }





        function btnRoute_Wise_Others_SaleQty(BranchId) {
            var BranchId;
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'Route_Wise_Other_SaleQty', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
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
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
            j = 1;
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th >Route Name</th><th >Other Sale Qty</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color: antiquewhite;">';
                results += '<td   class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnAgent_Wise_Other_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                results += '<td   class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
                l = l + 1;
                if (l == 30) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#divChart").html(results);
        }
        function btnAgent_Wise_Other_SaleQty(BranchId) {
            var BranchId;
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'Agent_Wise_Other_SaleQty', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
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
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
            j = 1;
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th >Agent Name</th><th >Other Sale Qty</th></tr></thead></tbody>';
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
            $("#divChart").html(results);
        }



        function BranchWiseCollection() {
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            var BranchId = document.getElementById('ddlPlant').value;
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'Branch_Wise_Collection', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
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
        function fill_Branch_Wise_Collections(msg) {
            scrollTo(0, 0);
            $('#divMainAddNewRow1').focus();
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

        function btnRoute_Wise_Collection_Click(BranchId) {
            var BranchId;
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'Route_Wise_Collection', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
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
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
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
            $("#divChart").html(results);
        }


        function btnAgent_Wise_Collection_Click(BranchId) {
            var BranchId;
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'Agent_Wise_Collection', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
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
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
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
            $("#divChart").html(results);
        }

        function BranchWiseDueDetails() {
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            var BranchId = document.getElementById('ddlPlant').value;
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'Branch_Wise_DueAmount', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
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
        function fill_Branch_Wise_DueAmount(msg) {
            scrollTo(0, 0);
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
            j = 1;
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


        function btnRoute_Wise_DueAmount_Click(BranchId) {
            var BranchId;
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'Route_Wise_DueAmount', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
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
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
            j = 1;
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
            $("#divChart").html(results);
        }


        function btnAgent_Wise_DueAmount_Click(BranchId) {
            var BranchId;
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'Agent_Wise_DueAmount', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
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
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
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
            $("#divChart").html(results);
        }


        function BranchIndentDayWiseComparison() {
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            var BranchId = document.getElementById('ddlPlant').value;
            // $('#divHide').css('display', 'block');
            var data = { 'operation': 'Branch_Wise_DayComparism', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    fill_Branch_Wise_DayIndent(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fill_Branch_Wise_DayIndent(msg) {
            scrollTo(0, 0);
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
            j = 1;
            var results = '<div  style="height: 400px;overflow-y: scroll;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background-color: #abbed2;"><th style="text-align: center;font-size: 16px; font-weight: bold;color: #2f3293;">Branch Name</th><th style="text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">YesterDay</th><th style="text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">Last Week</th><th style="text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">Last Month</th><th style="text-align: center; height: 20px; font-size: 16px; font-weight: bold;color: #2f3293;">Last Year</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color: antiquewhite;">';
                results += '<td style="text-align: center;color: Gray;"  class="2"><div id="txtamount" style="font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
                results += '<td   class="2"><div id="txtamount" style="cursor:pointer;" title="view Product in Line Chart"   class="subCategeoryClass"><b>' + msg[i].yesterindent + '<b></td>';
                results += '<td   class="2"><div id="txtamount" style="cursor:pointer;" title="view Product in Line Chart"   class="subCategeoryClass"><b>' + msg[i].lastweekindent + '<b></td>';
                results += '<td  class="2"><div id="txtamount" style="cursor:pointer;" title="view Product in Line Chart"    class="subCategeoryClass"><b>' + msg[i].lastmonthindent + '<b></td>';
                results += '<td  class="2"><div id="Span1" style="" class="QtyClass"><b>' + msg[i].lastyearindent + '</b></td></tr>';
                l = l + 1;
                if (l == 30) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#divComaprison").html(results);
        }
        function branchwisesaledetails() {
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            var PlantName = document.getElementById('ddlPlant').value;
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'branchwise_SaleValue', 'BranchId': PlantName, 'FromDate': FromDate, 'Todate': Todate };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    fillbranchwise_Sale_Value(msg);
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

        function fillbranchwise_Sale_Value(msg) {
            scrollTo(0, 0);
            $('#divMainAddNewRow1').focus();
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

        function btnRoute_Wise_SaleVaue(BranchId) {
            var BranchId;
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'Route_Wise_SaleValue', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
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
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
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
            $("#divChart").html(results);
        }
        function btnAgent_Wise_SaleVaue(BranchId) {
            var BranchId;
            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'Agent_Wise_SaleValue', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
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
            $('#divMainAddNewRow1').focus();
            $('#divMainAddNewRow1').css('display', 'block');
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
            $("#divChart").html(results);
        }

        //        function fillbranchwisesalevalue(msg) {
        //            j = 1;
        //            var results = '<div  style="overflow:auto;"><table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;" id="tableaProductdetails" class="mainText2" border="1">';
        //            results += '<thead><tr><th style="width: 33%; text-align: center; height: 20px; font-size: 16px; font-weight: bold;color: #2f3293;">BranchName</th><th style="width: 33%; height: 20px; text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">DispatchQty</th></tr></thead></tbody>';
        //            var k = 1;
        //            var l = 0;
        //            for (var i = 0; i < msg.length; i++) {
        //                results += '<tr>';
        //                results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;color: Gray;"  class="2"><div id="txtamount" style="font-size: 14px;color:#ffff;  font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnVariationProductlineChart(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
        //                results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;color: Gray;"  class="2"><div id="Span1" style="font-size: 14px;color: #ffff;  font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td></tr>';
        //                l = l + 1;
        //                if (l == 30) {
        //                    l = 0;
        //                }
        //            }
        //            results += '</table></div>';
        //            $("#divChart").html(results);
        //        }
    </script>
    <script type="text/javascript">
        var IndDate = "";
        var todate = "";
        var PlantName = "";
        $(function () {
            daterangepicker();
            datecontrolsession();
            //var date = new Date();
            //var day = date.getDate();
            //var month = date.getMonth() + 1;
            //var year = date.getFullYear();
            //if (month < 10) month = "0" + month;
            //day = day - 1;
            //if (day < 10) day = "0" + day;
            //today = year + "-" + month + "-" + day;
            //$('#txtDate').val(today);
            //$('#txtTodate').val(today);
            ddlTypeChange(1);
            ddlPlantNameChange(1);
        });

        function GraphicalNetSaleClick() {
        $('#divMainAddNewRow1').css('display','none');
        $('#divHide').css('display','none');
            Get_Dispatch_Sale_CategoryWise_Click()
          var IndDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            PlantName = document.getElementById('ddlPlant').value;
            if (Type == "Plant Wise") {
                if (PlantName == "" || PlantName == "Select Plant Name") {
                    alert("Please Plant Name");
                    return false;
                }
            }
            if (IndDate == "") {
                alert("Please Select from Date");
                return false;
            }
            if (Todate == "") {
                alert("Please Select to Date");
                return false;
            }
              fillProdcutData();
              GetBranchwiseDetails();
              BranchIndentDayWiseComparison();
        }
        function fillProdcutData() {
           var IndDate = $('#reportrange').data('daterangepicker').startDate.toString();
            var data = { 'operation': 'GetProductInformation', 'IndDate': IndDate, 'BranchID': PlantName, 'Type': Type };
            var s = function (msg) {
                if (msg) {
                
                  $('#firstdiv').css('display', 'block ');
                    fillsubcategorywiseproductsdata(msg);
                    ProductInformationpieChart(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

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

        function fillsubcategorywiseproductsdata(msg) {
        var BranchTable = [];
            j = 1;
            var pdelivaryqty = 0; var psalevalue = 0;
            var results = '<div  style="overflow:auto;"><table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;" id="tableaProductdetails" class="mainText2" border="1">';
            results += '<thead><tr><th style="width: 33%; text-align: center; height: 20px; font-size: 16px; font-weight: bold;color: #2f3293;">Category Name</th><th style="width: 33%; height: 20px; text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">SubCategory Name</th><th style="width: 33%; height: 20px; text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">DelivaryQty</th><th style="width: 33%; height: 20px; text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">Ltrs(%)</th><th style="width: 33%; height: 20px; text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">Kgs(%)</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = [ "#794b4b","#FF6600","#fdd400","#84b761","#cc4748","#cd82ad","#2f4074","#448e4d","#b7b83f","#b9783f","#b93e3d","#913167","#18d79c","#a2907f","#41ee80","#8cfa40","#dbdf9d","#3f4fb2","#01909b","#124c65","#33879b","#869ae2","#000000"];
            for (var i = 0; i < msg.length; i++) {
             if (BranchTable.indexOf(msg[i].CategeoryName) == -1) {
                results += '<tr >';
                results += '<td style="width: 33%; heights: 20px; vertical-align: middle; font-size: 16px; text-align: center;"  class="2"><div id="txtAccount" style="font-size: 12px; font-weight: bold;" class="AccountClass"><b>' + msg[i].CategeoryName + '</b></td>';
                 results += '<td style="background-color:' + COLOR[l] + '"  class="2"><div id="txtamount" style="font-size: 14px;color:#ffff;  font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnVariationProductlineChart(\'' + msg[i].subCatsno + '\');"  class="subCategeoryClass"><b>' + msg[i].subCategeoryName + '<b></td>';
                  results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;"  class="2"><div id="Span1" style="font-size: 14px;  font-weight: bold;" class="QtyClass"><b>' + msg[i].Qty + '</b></td>';
                  results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;" class="2" ><div style="font-size: 14px;  font-weight: bold;"><b>' + msg[i].Ltrs + '</b></div></td>';
                  results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;" class="2" ><div style="font-size: 14px;  font-weight: bold;"><b>' + msg[i].Kgs + '</b></div></td></tr>';
//                results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;" class="2" ><div style="font-size: 14px;  font-weight: bold;"><b>' + msg[i].Nos + '</b></div></td></tr>';
                l = l + 1;
                if (l == 30) {
                    l = 0;
                }
                BranchTable.push(msg[i].CategeoryName);
            }
            else
            {
             results += '<tr>';
             results += '<td style="border:none" class="1" ></td>';
                 results += '<td style="background-color:' + COLOR[l] + '"  class="2"><div id="txtamount" style="font-size: 14px;color:#ffff;  font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnVariationProductlineChart(\'' + msg[i].subCatsno + '\');"  class="subCategeoryClass"><b>' + msg[i].subCategeoryName + '<b></td>';
                  results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;"  class="2"><div id="Span1" style="font-size: 14px;  font-weight: bold;" class="QtyClass"><b>' + msg[i].Qty + '</b></td>';
                  results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;" class="2" ><div style="font-size: 14px;  font-weight: bold;"><b>' + msg[i].Ltrs + '</b></div></td>';
                  results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;" class="2" ><div style="font-size: 14px;  font-weight: bold;"><b>' + msg[i].Kgs + '</b></div></td></tr>';
                l = l + 1;
                if (l == 30) {
                    l = 0;
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
            var Fromdate = $('#reportrange').data('daterangepicker').startDate.toString();
            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            $('#divHide').css('display', 'block');
            var data = { 'operation': 'getLineChartforsubcategeoryReport', 'SubcatSno': subCatsno,'Fromdate':Fromdate,'Todate':Todate };
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
        scrollTo(0,0);
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
                results += '<td scope="row"   class="2" >' + msg[i].BranchName + '</th>';
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
        function CloseClick1() {
         $('html, body').animate({
                scrollTop: $("#BranchWiseDespatch").offset().down
            }, 2000);
            $('#BranchWiseDespatch').focus();
            $('#divMainAddNewRow1').css('display', 'none');
        }

        function GetBranchwiseDetails() {

           var IndDate = $('#reportrange').data('daterangepicker').startDate.toString();
           var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
            var data = { 'operation': 'GetBranchwiseDetailsInformation', 'IndDate': IndDate,'Todate':Todate, 'BranchID': PlantName, 'Type': Type };
            var s = function (msg) {
                if (msg) {
                    $('#BranchWiseDespatch').removeTemplate();
                    $('#BranchWiseDespatch').setTemplateURL('BranchDespatches5.htm');
                    $('#BranchWiseDespatch').processTemplate(msg);
                  
//                    var DespQty = 0.0;
//                    $('.DespClass').each(function (i, obj) {
//                        if ($(this).text() == "") {
//                        }
//                        else {
//                            DespQty += parseFloat($(this).text());
//                        }
//                    });
//                    DespQty = parseFloat(DespQty).toFixed(2);
//                    document.getElementById("txt_totalDesp").innerHTML = DespQty;


                    var SaleQty = 0.0;
                    $('.SaleClass').each(function (i, obj) {
                        if ($(this).text() == "") {
                        }
                        else {
                            SaleQty += parseFloat($(this).text());
                        }
                    });
                    SaleQty = parseFloat(SaleQty).toFixed(2);
//                    document.getElementById("txt_totalSale").innerHTML = SaleQty;


                    $('.SaleClass').each(function (i, obj) {
                        if ($(this).text() == "") {
                        }
                        else {
                            var Qty = parseFloat($(this).text());
                            var SalePercen = 0;
                            SalePercen = (Qty / SaleQty) * 100;
                            SalePercen = parseFloat(SalePercen).toFixed(2);
                            $(this).closest("tr").find("#txtTotalsalePer").text(SalePercen);
                        }
                    });

//                    var SalePercenQty = 0.0;
//                    $('.TotalsalePerClass').each(function (i, obj) {
//                        if ($(this).text() == "") {
//                        }
//                        else {
//                            SalePercenQty += parseFloat($(this).text());
//                        }
//                    });
//                    SalePercenQty = parseFloat(SalePercenQty).toFixed(2);
//                    document.getElementById("txt_totalSalePer").innerHTML = SalePercenQty;
//                    var LeakQty = 0.0;
//                    $('.LeakClass').each(function (i, obj) {
//                        if ($(this).text() == "") {
//                        }
//                        else {
//                            LeakQty += parseFloat($(this).text());
//                        }
//                    });
//                    LeakQty = parseFloat(LeakQty).toFixed(2);
//                    var LeakPercen = 0;
//                    LeakPercen = (LeakQty / DespQty) * 100;
//                    LeakPercen = parseFloat(LeakPercen).toFixed(2);
//                    document.getElementById("txt_totalLeak").innerHTML = LeakQty;
//                    document.getElementById("txt_totalLeak_Percent").innerHTML = LeakPercen;

////                    var FreeQty = 0.0;
////                    $('.FreeClass').each(function (i, obj) {
////                        if ($(this).text() == "") {
////                        }
////                        else {
////                            FreeQty += parseFloat($(this).text());
////                        }
////                    });
////                    FreeQty = parseFloat(FreeQty).toFixed(2);
////                    var FreePercen = 0;
////                    FreePercen = (FreeQty / DespQty) * 100;
////                    FreePercen = parseFloat(FreePercen).toFixed(2);
////                    document.getElementById("txt_totalFree").innerHTML = FreeQty + "  (" + FreePercen + "%)";

////                    var ShortQty = 0.0;
////                    $('.ShortClass').each(function (i, obj) {
////                        if ($(this).text() == "") {
////                        }
////                        else {
////                            ShortQty += parseFloat($(this).text());
////                        }
////                    });
////                    ShortQty = parseFloat(ShortQty).toFixed(2);
////                    var ShortPercen = 0;
////                    ShortPercen = (ShortQty / DespQty) * 100;
////                    ShortPercen = parseFloat(ShortPercen).toFixed(2);
////                    document.getElementById("txt_totalShort").innerHTML = ShortQty + "  (" + ShortPercen + "%)";

////                    var ReturnQty = 0.0;
////                    $('.ReturnsClass').each(function (i, obj) {
////                        if ($(this).text() == "") {
////                        }
////                        else {
////                            ReturnQty += parseFloat($(this).text());
////                        }
////                    });
////                    ReturnQty = parseFloat(ReturnQty).toFixed(2);
////                    var returnPercen = 0;
////                    returnPercen = (ReturnQty / DespQty) * 100;
////                    returnPercen = parseFloat(returnPercen).toFixed(2);
////                    document.getElementById("txt_totalRerurn").innerHTML = ReturnQty + "  (" + returnPercen + "%)";

//                     var FreeQty = 0.0;
//                    $('.FreeClass').each(function (i, obj) {
//                        if ($(this).text() == "") {
//                        }
//                        else {
//                            FreeQty += parseFloat($(this).text());
//                        }
//                    });
//                    FreeQty = parseFloat(FreeQty).toFixed(2);
//                    var FreePercen = 0;
//                    FreePercen = (FreeQty / DespQty) * 100;
//                    FreePercen = parseFloat(FreePercen).toFixed(2);
//                    document.getElementById("txt_totalSaleValue").innerHTML = FreeQty ;

//                    var ShortQty = 0.0;
//                    $('.ShortClass').each(function (i, obj) {
//                        if ($(this).text() == "") {
//                        }
//                        else {
//                            ShortQty += parseFloat($(this).text());
//                        }
//                    });
//                    ShortQty = parseFloat(ShortQty).toFixed(2);
//                    var ShortPercen = 0;
//                    ShortPercen = (ShortQty / DespQty) * 100;
//                    ShortPercen = parseFloat(ShortPercen).toFixed(2);
//                    document.getElementById("txt_totalCollction").innerHTML = ShortQty;
//                    var ReturnQty = 0.0;
//                    $('.ReturnsClass').each(function (i, obj) {
//                        if ($(this).text() == "") {
//                        }
//                        else {
//                            ReturnQty += parseFloat($(this).text());
//                        }
//                    });
//                    ReturnQty = parseFloat(ReturnQty).toFixed(2);
//                    var returnPercen = 0;
//                    returnPercen = (ReturnQty / DespQty) * 100;
//                    returnPercen = parseFloat(returnPercen).toFixed(2);
//                    document.getElementById("txt_totalDueAmount").innerHTML = ReturnQty;
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
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
            var RouteName = databind[0].subCategeoryName;
            var leakqty = databind[0].Qty;
            var color = [ "#794b4b", "#FF6600", "#fdd400", "#84b761", "#cc4748", "#cd82ad", "#2f4074", "#448e4d", "#b7b83f", "#b9783f", "#b93e3d", "#913167", "#18d79c", "#a2907f", "#41ee80", "#8cfa40", "#dbdf9d", "#3f4fb2", "#01909b", "#124c65", "#33879b", "#869ae2", "#000000"];

            for (var i = 0; i < databind.length; i++) {
                newXarray.push({ "category": databind[i].subCategeoryName, "value": parseFloat(databind[i].Qty), "color": color[i] });
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
        var BranchID = "";
        var Status = "GraphicalNetSale";
        function btnViewData(ID) {
            BranchID = $(ID).closest("tr").find('#hdnBranchID').val();
          
            $('#window').css('display', 'block');
            $("#window").empty();
            $("#chart").empty();
            $("#example").empty();
            var date = "";// document.getElementById('txtDate').value;
            date = $('#reportrange').data('daterangepicker').startDate.toString();
            var data = { 'operation': 'GetApprovalDetails', 'IndentDate': date, 'BranchID': BranchID, 'Status': Status };
            var s = function (msg) {
                if (msg) {
                $('#seconddiv').css('display', 'block ');

                    $('#window').removeTemplate();
                    $('#window').setTemplateURL('RouteWiseTimeline2.htm');
                    $('#window').processTemplate(msg);
                    Cgraphs();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function Cgraphs() {
            $("#chart").empty();
            routewiseDetails();
        }
        function routewiseDetails() {
            var  txtFromdate = $('#reportrange').data('daterangepicker').startDate.toString();
            var data = { 'operation': 'Getrouteleaksreturns', 'startDate': txtFromdate, 'ddlSalesOffice': BranchID, 'Status': Status };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    else {
                        createChart(msg);
                        createChart1(msg);
                        routewiseleakDetails();

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
        function createChart(databind) {
            var myTableDiv = document.getElementById("example2");
            var d = document.getElementById("chart");
            d.style.height = "300px";
            d.setAttribute("id", "id_you_like2");
            myTableDiv.appendChild(d);
            var RouteName = databind[0].RouteName;
            var dispatch = databind[0].totaldispatch;
            var saleqty = databind[0].totalsale;
            var leakqty = databind[0].totalleak;
            var returnqty = databind[0].totalreturn;
            var shortqty = databind[0].totalshort;
            var freeqty = databind[0].totalfree;
            var myData = [];
            if (databind.length > 0) {
                for (var k = 0; k < databind.length; k++) {
                    var BranchName = [];
                    RouteName = databind[0].RouteName;
                    dispatch = databind[0].totaldispatch;
                    saleqty = databind[0].totalsale;
                    leakqty = databind[0].totalleak;
                    returnqty = databind[0].totalreturn;
                    shortqty = databind[0].totalshort;
                    freeqty = databind[0].totalfree;
                    color = ["#ff8000", "#CD0D74", "#bfff00", "#40ff00", "#00ffbf", "#00bfff", "#4000ff", "#00ffff", "#0D52D1", "#8A0CCF", "#CD0D74", "#754DEB", "#DDDDDD", "#999999", "#333333", "#000000"];
                    for (var i = 0; i < RouteName.length; i++) {
                        myData.push({ "routename": RouteName[i].toString(), "dispatch": parseInt(dispatch[i]) || 0, "saleqty": parseInt(saleqty[i]) || 0, "color": color[i] });
                    }
                }
            }
            var chart = AmCharts.makeChart("id_you_like2", {
                type: "serial",
                theme: "light",
                marginRight: 70,
                dataProvider: myData,
                categoryField: "routename",
                categoryAxis: {
                    labelRotation: 45,
                    gridPosition: "start"
                },
                valueAxes: [{
                    axisAlpha: 0,
                    position: "left",
//                    title: "Branch wise Netamount"
                }],
//                startDuration: 1,
                "graphs": [
                		{
                		    "balloonText": "DispatchQty:[[value]]",
                		    "fillAlphas": 0.8,
                		    "id": "AmGraph-1",
                		    "lineAlpha": 0.2,
                		    "title": "Income",
                		    "type": "column",
                		    "colorField": "color",
                		    "valueField": "dispatch"
                		},
                		{
                		    "balloonText": "SaleQty:[[value]]",
                		    "fillAlphas": 0.8,
                		    "id": "AmGraph-2",
                		    "lineAlpha": 0.2,
                		    "title": "Expenses",
                		    "type": "column",
                		    "colorField": "color",
                		    "valueField": "saleqty"
                		}
	      ],
                //    
                chartCursor: {
                    cursorAlpha: 0,
                    zoomable: false,
                    categoryBalloonEnabled: false
                },
                "export": {
                    "enabled": true
                }

            });
        }
        function createChart1(databind) {
            var myTableDiv = document.getElementById("example3");
            var d = document.getElementById("chart1");
            d.style.height = "300px";
            d.setAttribute("id", "id_you_like3");
            myTableDiv.appendChild(d);
            var RouteName = databind[0].RouteName;
            var dispatch = databind[0].totaldispatch;
            var saleqty = databind[0].totalsale;
            var leakqty = databind[0].totalleak;
            var returnqty = databind[0].totalreturn;
            var shortqty = databind[0].totalshort;
            var freeqty = databind[0].totalfree;
            var myData = [];
            if (databind.length > 0) {
                for (var k = 0; k < databind.length; k++) {
                    var BranchName = [];
                    RouteName = databind[0].RouteName;
                    leakqty = databind[0].totalleak;
                    returnqty = databind[0].totalreturn;
                    shortqty = databind[0].totalshort;
                    freeqty = databind[0].totalfree;
                    color = ["#ff8000", "#ffff00", "#bfff00", "#40ff00", "#00ffbf", "#00bfff", "#4000ff", "#00ffff", "#0D52D1", "#8A0CCF", "#CD0D74", "#754DEB", "#DDDDDD", "#999999", "#333333", "#000000"];
                    for (var i = 0; i < RouteName.length; i++) {
                        myData.push({ "routename": RouteName[i].toString(), "leakqty": parseInt(leakqty[i]) || 0, "returnqty": parseInt(returnqty[i]) || 0, "shortqty": parseInt(shortqty[i]) || 0, "freeqty": parseInt(freeqty[i]) || 0, "color": color[i] });
                    }
                }
            }
            var chart = AmCharts.makeChart("id_you_like3", {
                type: "serial",
                theme: "light",
                marginRight: 70,
                dataProvider: myData,
                categoryField: "routename",
                categoryAxis: {
                    labelRotation: 45,
                    gridPosition: "start"
                },
                valueAxes: [{
                    axisAlpha: 0,
                    position: "left",
//                    title: "Branch wise Netamount"
                }],
                startDuration: 1,
                "graphs": [
                		{
                		    "balloonText": "LeakQty:[[value]]",
                		    "fillAlphas": 0.8,
                		    "id": "AmGraph-1",
                		    "lineAlpha": 0.2,
                		    "title": "Income",
                		    "type": "column",
                		    "valueField": "leakqty"
                		},
                		{
                		    "balloonText": "Return:[[value]]",
                		    "fillAlphas": 0.8,
                		    "id": "AmGraph-2",
                		    "lineAlpha": 0.2,
                		    "title": "Expenses",
                		    "type": "column",
                		    "valueField": "returnqty"
                		},
        {
            "balloonText": "Short:[[value]]",
            "fillAlphas": 0.8,
            "id": "AmGraph-3",
            "lineAlpha": 0.2,
            "title": "Expenses",
            "type": "column",
            "valueField": "shortqty"
        },
        {
            "balloonText": "Free:[[value]]",
            "fillAlphas": 0.8,
            "id": "AmGraph-3",
            "lineAlpha": 0.2,
            "title": "Expenses",
            "type": "column",
            "valueField": "freeqty"
        }
	],
                //    
                chartCursor: {
                    cursorAlpha: 0,
                    zoomable: false,
                    categoryBalloonEnabled: false
                },
                "export": {
                    "enabled": true
                }

            });
        }




        function btnProductlineChart() {
            window.open("AvgSaleinLineChart.aspx");
            return true;
        }
        function routewiseleakDetails() {
            var txtFromdate = ""; // document.getElementById('txtFromdate').value;
            var data = { 'operation': 'Getroutewiseleaks', 'startDate': IndDate, 'ddlSalesOffice': BranchID, 'Status': Status };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    else {
                        createleakbarChart(msg);
                        productwiseleakpie();
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
        var newleakXarray = [];
        function createleakbarChart(databind) {
            var newYarray = [];
            var newXarray = [];
            newLeaksarray = [];
            newAvgLeaksarray = [];
            var myTableDiv = document.getElementById("example_routeleaks");
            var divleakbar = document.getElementById("divRoteleak");

            divleakbar.style.height = "300px";
            divleakbar.setAttribute("id", "id_Route_Leaks");
            myTableDiv.appendChild(divleakbar);
            for (var k = 0; k < databind.length; k++) {


                var RouteName = databind[0].routename;
                var Leaks = databind[k].Leaks;
                var AvgLeaks = databind[0].AvgLeaks;

                newXarray = RouteName.split(',');
                newLeaksarray = Leaks.split(',');
                newAvgLeaksarray = AvgLeaks.split(',');
                for (var i = 0; i < newXarray.length; i++) {

                    newYarray.push({ "data": newLeaksarray[i], "name": newAvgLeaksarray[i], "val": newXarray[i].toString() });
                    
                }
            }
            
            var chart = AmCharts.makeChart("id_Route_Leaks", {
                "type": "serial",
                "theme": "light",
                "titles": [{
                    "text": "RouteWise Leaks % and Avgleaks %"
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
                    "position": "left",
                    "title": "Route Wise % leaks"
                }],
                "startDuration": 0.5,
                "graphs": [
                          {
                              "balloonText": "Leaks in [[category]]: [[value]]",
                              "bullet": "round",
                              "title": "Leaks",
                              "valueField": "data",
                              "fillAlphas": 0,
                              "fill": "#009933"
                          }, {
                              "balloonText": "Avg Leks in [[category]]: [[value]]",
                              "bullet": "round",
                              "title": "Average Leaks",
                              "valueField": "name",
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
                    "fillColor": "#000000",
                    "gridAlpha": 0,
                    "position": "left",
                    "labelRotation": 25

                },
                "export": {
                    "enabled": true,
                    "position": "bottom-right"
                }
            });
        }


        function productwiseleakpie() {
            var txtFromdate = ""; // document.getElementById('txtFromdate').value;
            var data = { 'operation': 'GetProductWiseleaks', 'startDate': IndDate, 'ddlSalesOffice': BranchID, 'Status': Status };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    else {
                        productleakpieChart(msg);
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
        function productleakpieChart(databind) {
            newXarray = [];
            var myTableDiv = document.getElementById("exampleleak");
            var divproductleakpie = document.getElementById("leakpiechart");
            divproductleakpie.style.height = "300px";
            divproductleakpie.setAttribute("id", "id_Product_Leak_Chart");
            myTableDiv.appendChild(divproductleakpie);
            var RouteName = databind[0].RouteName;
            var leakqty = databind[0].totalleak;
            for (var i = 0; i < RouteName.length; i++) {
                newXarray.push({ "category": RouteName[i], "value": parseFloat(leakqty[i]) });
            }

            var chart = AmCharts.makeChart("id_Product_Leak_Chart", {
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
                "titleField": "category",
                "export": {
                    "enabled": true
                }
              });
        }
        function DateClick() {
            datecontrolsession();
        }
        function btnShowRouteDetails(ID) {
            var DespSno = ID.value;
            GetAgentDetails(DespSno);
        }
        function GetAgentDetails(DespID) {
            var data = { 'operation': 'GetRouteAgentInformation', 'IndDate': IndDate, 'ddlSalesOffice': BranchID, 'DespID': DespID };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    else {
                        $('#divMainAddNewRow').css('display', 'block');
                        $('#divRouteDetails').removeTemplate();
                        $('#divRouteDetails').setTemplateURL('AgentInformation.htm');
                        $('#divRouteDetails').processTemplate(msg);
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
        function PopupCloseClick() {
            $('#divMainAddNewRow').css('display', 'none');
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
    <style type="text/css">
        .col-lg-3
        {
            width: 25%;
        }
        .btn
        {
            padding: 6px 12px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="width: 99%;padding-left: 0.5%;">
    <div class="box-body">
        <div class="box box-info">
            <div style="width: 13%; float: left;">
                <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="95px" height="90px" />
            </div>
            <div style="padding-left: 24%;">
                <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="30px"
                    Font-Names="'Source Sans Pro','Helvetica Neue',Helvetica,Arial,sans-serif" ForeColor="#0252aa" Text=""></asp:Label>
                <div style="float: right; padding-top: 12px;">
                    <a href="Delivery_Collection_Report.aspx" title="Go To Home Page">
                        <img src="Images/home.png" alt="Vyshnavi Dairy" width="70px" height="62px" /></a>
                </div>
                <br />
                <br />
            </div>
            <div style="width: 100%; padding-left: 30%;">
                <table>
                    <tr>
                        <td style="display: none;">
                            <span>Type</span>
                        </td>
                        <td style="width: 200px; display: none;">
                            <select id="ddlType" class="form-control" onchange="ddlTypeChange(this);">
                                <option>Plant Wise</option>
                                <option>Group Wise</option>
                            </select>
                        </td>
                        <td style="width: 200px; display: none;" id="divPlant">
                            <select id="ddlPlant" class="form-control" onchange="ddlPlantNameChange(this);">
                            </select>
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
                        <td>
                            <input type="button" id="submit" value="Submit" class="btn btn-success" onclick="GraphicalNetSaleClick()" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <br />
    </div>
    <div style="display: none;" id="firstdiv">
        <div id="example">
            <div id="divdashboard">
            </div>
        </div>
        <div class="row">
            <%-- <div class="col-sm-7">
                <div class="box box-solid box-warning">
                    <div class="box-header with-border">
                        <h3 class="fa fa-th">
                            <i style="padding-right: 5px;"></i>Sales Graph
                        </h3>
                        <div class="box-tools pull-left">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="box-body no-padding">
                            <div style="width: 25%; float: left;">
                                <div id="gauge-container">
                                    <div id="gauge">
                                    </div>
                                    <input id="gauge-value" style="display: none;" value="0">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>
            <div class="box box-solid box-success">
                <div>
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
                        <div class="box-body no-padding">
                            <div class="col-lg-3 col-xs-6">
                                <!-- small box -->
                                <div class="small-box bg-aqua">
                                    <div class="inner">
                                        <h3 id="hdispatchqty">
                                            0
                                        </h3>
                                        <p>
                                            Despatch Qty</p>
                                    </div>
                                    <div class="icon">
                                        <i class="fa fa-cubes"></i>
                                    </div>
                                    <a href="#datagrid" class="small-box-footer" onclick="branchwisedispqty();" data-toggle="modal"
                                        data-target="#myModal">BranchWise Dispatchqty <i class="fa fa-arrow-circle-right">
                                        </i></a>
                                </div>
                            </div>
                            <!-- ./col -->
                            <div class="col-lg-3 col-xs-6">
                                <!-- small box -->
                                <div class="small-box bg-purple">
                                    <div class="inner">
                                        <h3 id="hsalevalue">
                                            0
                                        </h3>
                                        <p>
                                            Sale Value</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-pie-graph"></i>
                                    </div>
                                    <a href="#datagrid" class="small-box-footer" onclick="branchwisesaledetails();" data-toggle="modal"
                                        data-target="#myModal">BranchWise Sale Value <i class="fa fa-arrow-circle-right">
                                        </i></a>
                                </div>
                            </div>
                            <!-- ./col -->
                            <div class="col-lg-3 col-xs-6" style="height: 20px;">
                                <!-- small box -->
                                <div class="small-box bg-olive">
                                    <div class="inner">
                                        <h3 id="hmilkqty">
                                            0
                                        </h3>
                                        <p>
                                            Milk Qty</p>
                                    </div>
                                    <div class="icon">
                                        <i class="fa fa-thumbs-o-up"></i>
                                    </div>
                                    <a href="#datagrid" class="small-box-footer" onclick="branchwisemilkdetails();" data-toggle="modal"
                                        data-target="#myModal">BranchWise Milk Qty <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                            <div class="col-lg-3 col-xs-6">
                                <!-- small box -->
                                <div class="small-box bg-teal">
                                    <div class="inner">
                                        <h3 id="hcurdqty">
                                            0
                                        </h3>
                                        <p>
                                            Curd Qty</p>
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
                            <div class="col-lg-3 col-xs-6">
                                <!-- small box -->
                                <div class="small-box bg-yellow">
                                    <div class="inner">
                                        <h3 id="hothersqty">
                                            0
                                        </h3>
                                        <p>
                                            Others Qty</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-bag"></i>
                                    </div>
                                    <a href="#datagrid" class="small-box-footer" onclick="branchwiseotherdetails();"
                                        data-toggle="modal" data-target="#myModal">BranchWise Others Qty <i class="fa fa-arrow-circle-right">
                                        </i></a>
                                </div>
                            </div>
                            <div class="col-lg-3 col-xs-6">
                                <!-- small box -->
                                <div class="small-box bg-teal">
                                    <div class="inner">
                                        <h3 id="spnamount">
                                            0
                                        </h3>
                                        <p>
                                            Collection</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-bag"></i>
                                    </div>
                                    <a href="#datagrid" class="small-box-footer" onclick="BranchWiseCollection();" data-toggle="modal"
                                        data-target="#myModal">Branch Collection <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                            <div class="col-lg-3 col-xs-6">
                                <!-- small box -->
                                <div class="small-box bg-blue">
                                    <div class="inner">
                                        <h3 id="hduevalue">
                                            0
                                        </h3>
                                        <p>
                                            Due Value</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-bag"></i>
                                    </div>
                                    <a href="#datagrid" class="small-box-footer" onclick="BranchWiseDueDetails();" data-toggle="modal"
                                        data-target="#myModal">Branch Due details<i class="fa fa-arrow-circle-right"></i></a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="divHide" style="width: 100%; display: none;">
                <%--<div id="divMainAddNewRow1" class="pickupclass" style="height: 100%;width:45%; position: absolute; display: none; left: 25%; top: 16%; z-index: 99999;">
                    <div class="row">
                        <div class="col-sm-6">
                            <div class="box box-solid box-info">
                                <div class="box-header with-border">
                                    <h3 class="fa fa-inbox">
                                        <i style="padding-right: 5px;"></i>Despatch Qty
                                    </h3>
                                </div>
                                <div class="box-body">
                                    <div class="box-body no-padding">
                                        <div id="Div3" class="k-content">
                                            <div class="chart-wrapper">
                                                <div id="divChart" style="height: 400px;overflow-y: scroll;">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="div5" style="width: 35px; top: 0%; right: 2%; position: absolute; z-index: 99999;
                        cursor: pointer;">
                        <img src="Images/Close.png" alt="close" width="100%" height="100%" onclick="CloseClick1();" />
                    </div>
                </div>--%>
                <div id="divMainAddNewRow1" class="pickupclass" style="text-align: center; height: 80%;
                width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
                background: rgba(192, 192, 192, 0.7);">
                <div id="div3" style="border: 5px solid #A0A0A0; position: absolute; top: 8%;
                    background-color: White; left: 10%; right: 10%; width: 80%; height: 100%; -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                    -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                    border-radius: 10px 10px 10px 10px;">
                    <table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;"
                        id="table1" class="mainText3" border="1">
                        <tr>
                            <td colspan="2">
                                <div id="divChart" style="height: 400px;overflow-y: scroll;">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="button" class="btn btn-danger" id="btn_prod_det_close" value="Close" onclick="CloseClick1();" />
                            </td>
                        </tr>
                    </table>
                </div>
                 <div id="div5" style="width: 35px; top: 3%; right: 8%; position: absolute; z-index: 99999;
                        cursor: pointer;">
                        <img src="Images/Close.png" alt="close" width="100%" height="100%" onclick="CloseClick1();" />
                    </div>
            </div>




                <div style="width: 100%;">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div style="width: 100%;">
                <div id="BranchWiseDespatch">
                </div>
            </div>
            <br />
            <div class="row" >
                <div class="col-sm-7" style="width:100%;">
                    <div class="box box-solid box-danger">
                        <div class="box-header with-border">
                            <h3 class="ion ion-clipboard">
                                <i style="padding-right: 5px;"></i>Day Wise Indent Comparison
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
            </div>
            <div id="example1" class="k-content">
                <div style="width: 100%;">
                    <div class="row">
                        <div class="col-sm-7">
                            <div class="box box-solid box-danger">
                                <div class="box-header with-border">
                                    <h3 class="ion ion-clipboard">
                                        <i style="padding-right: 5px;"></i>SubCategory  Wise Information
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
                        <div class="col-sm-7">
                            <div class="box box-solid box-info">
                                <div class="box-header with-border">
                                    <h3 class="ion ion-pie-graph">
                                        <i style="padding-right: 5px;"></i>SubCategory  Wise Qty
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
                </div>
            </div>
        </div>
        <div id="seconddiv" style="display: none;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="box box-solid box-success">
                        <div class="box-header with-border">
                            <h3 class="fa fa-bar-chart-o">
                                <i style="padding-right: 5px;"></i>RouteWise Dispatch And Sale Qty
                            </h3>
                            <div class="box-tools pull-right">
                                <button class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="box-body no-padding">
                                <div id="example2" class="k-content">
                                    <div class="chart-wrapper">
                                        <div id="chart">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="box box-solid box-warning">
                        <div class="box-header with-border">
                            <h3 class="fa fa-bar-chart-o">
                                <i style="padding-right: 5px;"></i>RouteWise Leaks,Returns,Free And Short
                            </h3>
                            <div class="box-tools pull-right">
                                <button class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="box-body no-padding">
                                <div id="example3" class="k-content">
                                    <div class="chart-wrapper">
                                        <div id="chart1">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-9">
                    <div class="box box-solid box-info">
                        <div class="box-header with-border">
                            <h3 class="ion ion-pie-graph">
                                <i style="padding-right: 5px;"></i>Product Wise Leaks %
                            </h3>
                            <div class="box-tools pull-right">
                                <button class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="box-body no-padding">
                                <div id="exampleleak" class="k-content">
                                    <div class="chart-wrapper">
                                        <div id="leakpiechart">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <div class="box box-solid box-primary">
                        <div class="box-header with-border">
                            <h3 class="fa fa-bar-chart-o">
                                <i style="padding-right: 5px;"></i>RouteWise Leaks % and Avgleaks %
                            </h3>
                            <div class="box-tools pull-right">
                                <button class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="box-body no-padding">
                                <div id="example_routeleaks" class="k-content">
                                    <div class="chart-wrapper">
                                        <div id="divRoteleak">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div style="width: 100%;">
                <div style="width: 100%; float: left;">
                    <div id="window">
                    </div>
                </div>
            </div>
            <style scoped>
                #gauge-container
                {
                    background: transparent url("../content/dataviz/gauge/gauge-container-partial.png") no-repeat 50% 50%;
                    width: 296px;
                    height: 296px;
                    text-align: center;
                }
                #gauge
                {
                    width: 270px;
                    height: 210px;
                    margin: 0 auto;
                }
                #gauge-container .k-slider
                {
                    margin-top: -11px;
                    width: 140px;
                }
            </style>
        </div>
        <div id="divMainAddNewRow" class="pickupclass" style="text-align: center; height: 100%;
            width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
            background: rgba(192, 192, 192, 0.7);">
            <div id="divAddNewRow" style="border: 5px solid #A0A0A0; position: absolute; top: 25%;
                background-color: White; left: 2%; right: 2%; width: 96%; height: 100%; -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                border-radius: 10px 10px 10px 10px;">
                <div id="divRouteDetails">
                </div>
            </div>
            <div id="divclose" style="width: 35px; top: 24.5%; right: 2.5%; position: absolute;
                z-index: 99999; cursor: pointer;">
                <img src="Images/Odclose.png" alt="close" onclick="PopupCloseClick();" />
            </div>
        </div>
    </form>
</body>
</html>
