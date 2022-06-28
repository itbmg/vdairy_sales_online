<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostBack="true" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="DashboardNetSale.aspx.cs" Inherits="DashboardNetSale" %>

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
    </style>
    <script type="text/javascript">
    $(function () {
        $("#leftdiv").css('margin-left', 0);
        $("#leftdiv").css('margin-right', 0);
        $("#leftdiv").animate({ left: '-640px' }, 0); 
        $("#rightdiv").css('width', '100%');
        hiddenvalue = true;
            $('#divPlant').css('display', 'block');
            var IndDate = "";
        var todate = "";
//             var PlantName = "";
        var date = new Date();
        var day = date.getDate();
        var month = date.getMonth() + 1;
        var year = date.getFullYear();
        if (month < 10) month = "0" + month;
        day=day-1;
        if (day < 10) day = "0" + day;
        today = year + "-" + month + "-" + day;
        $('#txtDate').val(today);
            $('#txtTodate').val(today);
            FillPlant();
            ddlTypeChange(day);
            ddlPlantNameChange(day);
             

var start = moment().subtract(1, 'days');
var end = moment().subtract(1, 'days');

function cb(start, end) {
    $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
}

$('#reportrange').daterangepicker({
    startDate: start,
    endDate: end,
    ranges: {
        'Today': [moment(), moment()],
        'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
//           'Last 7 Days': [moment().subtract(6, 'days'), moment()],
//           'Last 30 Days': [moment().subtract(29, 'days'), moment()],
//           'This Month': [moment().startOf('month'), moment().endOf('month')],
//           'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
    }
}, cb);

cb(start, end);
      
   

});

function branchtypeChange(){
var branchtype = document.getElementById('ddlbarnchCategory').value;
if(branchtype =="BranchWiseCollections")
{
  $('#tddatatypeid').css('display', 'none');
}
else 
{
 $('#tddatatypeid').css('display', 'block');
}
}



    var MaxSale = 0;
    var Type = "";
    function ddlTypeChange(ID) {
        Type = document.getElementById('ddlType').value;
        if (Type == "Plant Wise") {
            FillPlant();
//                $('#divPlant').css('display', 'block');
        }
        else {
            MaxSale = 200;
                FillPlant();
            // Getgauge();
//                $('#divPlant').css('display', 'none');
        }
    }
//        var PlantName = "";
    function ddlPlantNameChange(ID) {
        var PlantName = document.getElementById('ddlPlant').value;
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
    function FillPlant() {
        var SelecteType=document.getElementById('ddlType').value;
        var data = { 'operation': 'GetSalesOffices','SelecteType':SelecteType };
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
        //GraphicalNetSaleClick();
    }
//        function Get_Dispatch_Sale_CategoryWise_Click() {
//            var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
//            var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
//            var PlantName = document.getElementById('ddlPlant').value;
//            var data = { 'operation': 'Get_Dispatch_Sale_CategoryWise', 'BranchId': PlantName, 'FromDate': FromDate, 'Todate': Todate };
//            var s = function (msg) {
//                if (msg) {
//                    if (msg == "Session Expired") {
//                        alert(msg);
//                        window.location = "Login.aspx";
//                    }
//                    fillcategorywisedispatchsale(msg);
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
//        }

    function fillcategorywisedispatchsale(msg) {
          
        var ddlDataType=document.getElementById("ddlDataType").value;
        var ddlbarnchCategory=document.getElementById("ddlbarnchCategory").value;
        if(ddlbarnchCategory=="CategoryWiseDespatch"){
        if(ddlDataType == "Quantity")
        {
        var MilkQty = msg[0].MilkQty;
        var CurdQty = msg[0].CurdQty;
        var OtherQty = msg[0].OtherQty;
           
        document.getElementById('hmilkqty').innerHTML = MilkQty + ' Ltrs';
        document.getElementById('hcurdqty').innerHTML = CurdQty + ' Ltrs';
        document.getElementById('hothersqty').innerHTML = OtherQty;
        document.getElementById('spnmilkid').innerHTML = "MilkQty";
        document.getElementById('spncurdid').innerHTML = "Curd Qty";
        document.getElementById('spnothersid').innerHTML = "Other Qty";
        //id=""
        }
        else 
        {
         var MilkValue = msg[0].MilkValue;
        var CurdValue = msg[0].CurdValue;
        var OtherValue = msg[0].OthersValue;
           
        document.getElementById('hmilkqty').innerHTML = MilkValue;
        document.getElementById('hcurdqty').innerHTML = CurdValue;
        document.getElementById('hothersqty').innerHTML = OtherValue;
        document.getElementById('spnmilkid').innerHTML = "Milk Value";
        document.getElementById('spncurdid').innerHTML = "Curd Value";
        document.getElementById('spnothersid').innerHTML = "Other Value";
        }
        }
      else if(ddlbarnchCategory == "BranchWiseDespatch")
        {
           if(ddlDataType == "Quantity")
        {
         var DispatchQty = msg[0].GroupTotDispQty;
        document.getElementById('hdispatchqty').innerHTML = DispatchQty + ' Ltrs';
        }
        else 
        {
        }
        }
       else if(ddlbarnchCategory == "BranchWiseCollections")
        {
           var SaleValue = msg[0].GroupTotSaleValue;
           var collamount = msg[0].GroupTotCollectionValue;
           var dueamount = msg[0].GroupDueAmount;
           document.getElementById('hsalevalue').innerHTML = '&#8377; ' + SaleValue;
           document.getElementById('spnamount').innerHTML = '&#8377; ' + collamount;
           document.getElementById('hduevalue').innerHTML = '&#8377; ' + dueamount;
        }
        else 
        {

        }
    }
    function PopupOpen(){
     
     $('#divMainAddNewRow1').css('display', 'block');
     return false; 
    }
    function ExecuteCodeBehindClickEvent() {
            //Get the Button reference and trigger the click event
           // document.getElementById("btnsess").click();
            

        }
    function branchwisedispqty() {
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
        $('#divHide').css('display', 'block');
        var data = { 'operation': 'Branch_Wise_DayComparism', 'BranchId': PlantName, 'FromDate': FromDate, 'Todate': Todate };
        var s = function (msg) {
            if (msg) {
                if (msg == "Session Expired") {
                    alert(msg);
                    window.location = "Login.aspx";

                }
              document.getElementById('<%= btnsess.ClientID %>').click();
              $('#div_MainPlantDetails').css('display', 'block');
            }
            else {
             $('#div_MainPlantDetails').css('display', 'block');
//            fillPlant_Dispatch_Qty();
            }
        };
        var e = function (x, h, e) {
        };
        $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }

    function fillbranchwisedispqty(msg) {
        scrollTo(0, 0);
        $('#divMainAddNewRow1').focus();
        $('#divMainAddNewRow1').css('display', 'block');
        j = 1;
        var DataType=msg[0].DataType;
        if(DataType =="Daily")
        {
        var results = '<div  style="overflow:auto;"><table border="1" id="tbl_po_print" style="width: 100%;" class="table table-bordered table-hover dataTable no-footer">';
        results += '<thead><tr style="background:antiquewhite;"><th value="Branch Name" style = "font-size: 12px;" colspan="1" rowspan="2">Branch Name</th><th value="YesterDay" style = "font-size: 12px;" colspan="3" rowspan="1">YesterDay</th><th value="Last Week" colspan="3" style = "font-size: 12px;" rowspan="1">Last Week</th><th value="Last Month" style = "font-size: 12px;" colspan="3" rowspan="1">Last Month</th><th value="Last Year" style = "font-size: 12px;" colspan="3" rowspan="1">Last Year</th></tr><tr style="background:antiquewhite;"><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th style = "font-size: 12px;" value="%" colspan="1" rowspan="1">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th style = "font-size: 12px;" value="%" colspan="1" rowspan="1">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th></tr></thead>';
        var k = 1;
        var l = 0;
        for (var i = 0; i < msg.length; i++) {
            results += '<tr onclick="btnRouteWiseDayComparison(\'' + msg[i].BranchID + '\')">';
            results += '<th  class="2"><div id="txtamount" class="subCategeoryClass">' + msg[i].BranchName + '</th>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterdayaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterdaypercentage + '</div></td>';
            results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekpercentage + '</div></td>';
            results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthpercentage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthpercentage + '</div></td>';
            results += '<td  class="2"><div id="Span1" style="" class="QtyClass"><div style="float: right;">' + msg[i].lastyearpercentage + '</div></td></tr>';
            l = l + 1;
            if (l == 30) {
                l = 0;
            }
        }
        }
        else if(DataType =="Weekly")
        {
            var results = '<div  style="overflow:auto;"><table border="1" id="tbl_po_print" style="width: 100%;" class="table table-bordered table-hover dataTable no-footer">';
        results += '<thead><tr style="background:antiquewhite;"><th value="Branch Name" style = "font-size: 12px;" colspan="1" rowspan="2">Branch Name</th><th value="PresentWeek" style = "font-size: 12px;" colspan="3" rowspan="1">PresentWeek</th><th value="Last Week" colspan="3" style = "font-size: 12px;" rowspan="1">Last Week</th><th value="Last Month" style = "font-size: 12px;" colspan="3" rowspan="1">Last Month</th><th value="Last Year" style = "font-size: 12px;" colspan="3" rowspan="1">Last Year</th></tr><tr style="background:antiquewhite;"><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th style = "font-size: 12px;" value="%" colspan="1" rowspan="1">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th style = "font-size: 12px;" value="%" colspan="1" rowspan="1">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th></tr></thead>';
        var k = 1;
        var l = 0;
        for (var i = 0; i < msg.length; i++) {
            results += '<tr onclick="btnRouteWiseDayComparison(\'' + msg[i].BranchID + '\')">';
            results += '<th  class="2"><div id="txtamount" class="subCategeoryClass">' + msg[i].BranchName + '</th>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].presentweakindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].presentweakavg + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].presentweakpercentage + '</div></td>';
                
            results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekpercentage + '</div></td>';
               
            results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthweekindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthweekaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthweekpercentage + '</div></td>';
                
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearweekindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearweekaverage + '</div></td>';
            results += '<td  class="2"><div id="Span1" style="" class="QtyClass"><div style="float: right;">' + msg[i].lastyearweekpercentage + '</div></td></tr>';
            l = l + 1;
            if (l == 30) {
                l = 0;
            }
        }
        }
        else if(DataType =="Monthly")
        {
        var results = '<div  style="overflow:auto;"><table border="1" id="tbl_po_print" style="width: 100%;" class="table table-bordered table-hover dataTable no-footer">';
        results += '<thead><tr style="background:antiquewhite;"><th value="Branch Name" style = "font-size: 12px;" colspan="1" rowspan="2">Branch Name</th><th value="Present Month" style = "font-size: 12px;" colspan="3" rowspan="1">Present Month</th><th value="Last Month" colspan="3" style = "font-size: 12px;" rowspan="1">Last Month</th><th value="Last Sixth Month" style = "font-size: 12px;" colspan="3" rowspan="1">Last Sixth Month</th><th value="Last Year" style = "font-size: 12px;" colspan="3" rowspan="1">Last Year</th></tr><tr style="background:antiquewhite;"><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th style = "font-size: 12px;" value="%" colspan="1" rowspan="1">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th style = "font-size: 12px;" value="%" colspan="1" rowspan="1">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th></tr></thead>';
        var k = 1;
        var l = 0;
        for (var i = 0; i < msg.length; i++) {
            results += '<tr onclick="btnRouteWiseDayComparison(\'' + msg[i].BranchID + '\')">';
            results += '<th  class="2"><div id="txtamount" class="subCategeoryClass">' + msg[i].BranchName + '</th>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].presentmonthindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].presentmonthavg + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].presentmonthpercentage + '</div></td>';
               
            results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthpercentage + '</div></td>';
                
            results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastsixthmonthindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastsixthmonthaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastsixthmonthpercentage + '</div></td>';
               
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearmonthindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearmonthaverage + '</div></td>';
            results += '<td  class="2"><div id="Span1" style="" class="QtyClass"><div style="float: right;">' + msg[i].lastyearmonthpercentage + '</div></td></tr>';
            l = l + 1;
            if (l == 30) {
                l = 0;
            }
        }
        }
        else if(DataType =="Yearly")
        {
        var results = '<div  style="overflow:auto;"><table border="1" id="tbl_po_print" style="width: 100%;" class="table table-bordered table-hover dataTable no-footer">';
        results += '<thead><tr style="background:antiquewhite;"><th value="Branch Name" style = "font-size: 12px;" colspan="1" rowspan="2">Branch Name</th><th value="Present Year" style = "font-size: 12px;" colspan="3" rowspan="1">Present Year</th><th value="Last Year" colspan="3" style = "font-size: 12px;" rowspan="1">Last Year</th></tr><tr style="background:antiquewhite;"><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th></tr></thead>';
        var k = 1;
        var l = 0;
        for (var i = 0; i < msg.length; i++) {
            results += '<tr onclick="btnRouteWiseDayComparison(\'' + msg[i].BranchID + '\')">';
            results += '<th  class="2"><div id="txtamount" class="subCategeoryClass">' + msg[i].BranchName + '</th>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].presentyearindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].presentyearhavg + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].presentyearpercentage + '</div></td>';
                
                
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearaverage + '</div></td>';
            results += '<td  class="2"><div id="Span1" style="" class="QtyClass"><div style="float: right;">' + msg[i].lastyearpercentage + '</div></td></tr>';
            l = l + 1;
            if (l == 30) {
                l = 0;
            }
        }
        }
        results += '</table></div>';
        $("#divChart").html(results);
    }
        
    function btnGroupUnder_PlantClick_Branch_Wise_SaleQty(BranchId) {
        var BranchId;
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
        $('#divHide').css('display', 'block');
        var data = { 'operation': 'Branch_Wise_DayComparism', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
        var s = function (msg) {
            if (msg) {
                if (msg == "Session Expired") {
                    alert(msg);
                    window.location = "Login.aspx";
                }
                fillbranchwisedispqty(msg);
            }
            else {
            }
        };
        var e = function (x, h, e) {
        };
        $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }
        

//        function fillPlant_Dispatch_Qty(msg) {
//            scrollTo(0, 0);
////            $('#divMainAddNewRow1').focus();
////            $('#divMainAddNewRow1').css('display', 'block');
//             $('#div_MainPlantDetails').css('display', 'block');
//            j = 1;
//            var DataType=msg[0].DataType;
//            if(DataType =="Daily")
//            {
//            var results = '<div  style="overflow:auto;"><table border="1" id="tbl_po_print" style="width: 100%;" class="table table-bordered table-hover dataTable no-footer">';
//            results += '<thead><tr style="background:antiquewhite;"><th value="Branch Name" style = "font-size: 12px;" colspan="1" rowspan="2">Branch Name</th><th value="YesterDay" style = "font-size: 12px;" colspan="3" rowspan="1">YesterDay</th><th value="Last Week" colspan="3" style = "font-size: 12px;" rowspan="1">Last Week</th><th value="Last Month" style = "font-size: 12px;" colspan="3" rowspan="1">Last Month</th><th value="Last Year" style = "font-size: 12px;" colspan="3" rowspan="1">Last Year</th></tr><tr style="background:antiquewhite;"><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th style = "font-size: 12px;" value="%" colspan="1" rowspan="1">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th style = "font-size: 12px;" value="%" colspan="1" rowspan="1">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th></tr></thead>';
//            var k = 1;
//            var l = 0;
//         for (var i = 0; i < msg.length; i++) {
//                results += '<tr onclick="btnGroupUnder_PlantClick_Branch_Wise_SaleQty(\'' + msg[i].BranchID + '\')">';
//                results += '<th  class="2"><div id="txtamount" class="subCategeoryClass">' + msg[i].BranchName + '</th>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterindent + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterdayaverage + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterdaypercentage + '</div></td>';
//                results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekindent + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekaverage + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekpercentage + '</div></td>';
//                results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthindent + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthaverage + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthpercentage + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearindent + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthpercentage + '</div></td>';
//                results += '<td  class="2"><div id="Span1" style="" class="QtyClass"><div style="float: right;">' + msg[i].lastyearpercentage + '</div></td></tr>';
//                l = l + 1;
//                if (l == 30) {
//                    l = 0;
//                }
//            }
//            }
//            else if(DataType =="Weekly")
//            {
//             var results = '<div  style="overflow:auto;"><table border="1" id="tbl_po_print" style="width: 100%;" class="table table-bordered table-hover dataTable no-footer">';
//            results += '<thead><tr style="background:antiquewhite;"><th value="Branch Name" style = "font-size: 12px;" colspan="1" rowspan="2">Branch Name</th><th value="PresentWeek" style = "font-size: 12px;" colspan="3" rowspan="1">PresentWeek</th><th value="Last Week" colspan="3" style = "font-size: 12px;" rowspan="1">Last Week</th><th value="Last Month" style = "font-size: 12px;" colspan="3" rowspan="1">Last Month</th><th value="Last Year" style = "font-size: 12px;" colspan="3" rowspan="1">Last Year</th></tr><tr style="background:antiquewhite;"><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th style = "font-size: 12px;" value="%" colspan="1" rowspan="1">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th style = "font-size: 12px;" value="%" colspan="1" rowspan="1">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th></tr></thead>';
//            var k = 1;
//            var l = 0;
//            for (var i = 0; i < msg.length; i++) {
//                results += '<tr onclick="btnGroupUnder_PlantClick_Branch_Wise_SaleQty(\'' + msg[i].BranchID + '\')">';
//                results += '<th  class="2"><div id="txtamount" class="subCategeoryClass">' + msg[i].BranchName + '</th>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].presentweakindent + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].presentweakavg + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].presentweakpercentage + '</div></td>';
//                
//                results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekindent + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekaverage + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekpercentage + '</div></td>';
//               
//                results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthweekindent + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthweekaverage + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthweekpercentage + '</div></td>';
//                
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearweekindent + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearweekaverage + '</div></td>';
//                results += '<td  class="2"><div id="Span1" style="" class="QtyClass"><div style="float: right;">' + msg[i].lastyearweekpercentage + '</div></td></tr>';
//                l = l + 1;
//                if (l == 30) {
//                    l = 0;
//                }
//            }
//            }
//            else if(DataType =="Monthly")
//            {
//            var results = '<div  style="overflow:auto;"><table border="1" id="tbl_po_print" style="width: 100%;" class="table table-bordered table-hover dataTable no-footer">';
//            results += '<thead><tr style="background:antiquewhite;"><th value="Branch Name" style = "font-size: 12px;" colspan="1" rowspan="2">Branch Name</th><th value="Present Month" style = "font-size: 12px;" colspan="3" rowspan="1">Present Month</th><th value="Last Month" colspan="3" style = "font-size: 12px;" rowspan="1">Last Month</th><th value="Last Sixth Month" style = "font-size: 12px;" colspan="3" rowspan="1">Last Sixth Month</th><th value="Last Year" style = "font-size: 12px;" colspan="3" rowspan="1">Last Year</th></tr><tr style="background:antiquewhite;"><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th style = "font-size: 12px;" value="%" colspan="1" rowspan="1">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th style = "font-size: 12px;" value="%" colspan="1" rowspan="1">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th></tr></thead>';
//            var k = 1;
//            var l = 0;
//            for (var i = 0; i < msg.length; i++) {
//                results += '<tr onclick="btnGroupUnder_PlantClick_Branch_Wise_SaleQty(\'' + msg[i].BranchID + '\')">';
//                results += '<th  class="2"><div id="txtamount" class="subCategeoryClass">' + msg[i].BranchName + '</th>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].presentmonthindent + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].presentmonthavg + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].presentmonthpercentage + '</div></td>';
//               
//                results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthindent + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthaverage + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthpercentage + '</div></td>';
//                
//                results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastsixthmonthindent + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastsixthmonthaverage + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastsixthmonthpercentage + '</div></td>';
//               
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearmonthindent + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearmonthaverage + '</div></td>';
//                results += '<td  class="2"><div id="Span1" style="" class="QtyClass"><div style="float: right;">' + msg[i].lastyearmonthpercentage + '</div></td></tr>';
//                l = l + 1;
//                if (l == 30) {
//                    l = 0;
//                }
//            }
//            }
//            else if(DataType =="Yearly")
//            {
//            var results = '<div  style="overflow:auto;"><table border="1" id="tbl_po_print" style="width: 100%;" class="table table-bordered table-hover dataTable no-footer">';
//            results += '<thead><tr style="background:antiquewhite;"><th value="Branch Name" style = "font-size: 12px;" colspan="1" rowspan="2">Branch Name</th><th value="Present Year" style = "font-size: 12px;" colspan="3" rowspan="1">Present Year</th><th value="Last Year" colspan="3" style = "font-size: 12px;" rowspan="1">Last Year</th></tr><tr style="background:antiquewhite;"><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th></tr></thead>';
//            var k = 1;
//            var l = 0;
//            for (var i = 0; i < msg.length; i++) {
//                results += '<tr onclick="btnGroupUnder_PlantClick_Branch_Wise_SaleQty(\'' + msg[i].BranchID + '\')">';
//                results += '<th  class="2"><div id="txtamount" class="subCategeoryClass">' + msg[i].BranchName + '</th>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].presentyearindent + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].presentyearhavg + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].presentyearpercentage + '</div></td>';
//                
//                
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearindent + '</div></td>';
//                results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearaverage + '</div></td>';
//                results += '<td  class="2"><div id="Span1" style="" class="QtyClass"><div style="float: right;">' + msg[i].lastyearpercentage + '</div></td></tr>';
//                l = l + 1;
//                if (l == 30) {
//                    l = 0;
//                }
//            }
//            }
//            results += '</table></div>';
//            $("#div_PlantDetails").html(results);
//        }


//var TotalDate = []; var attendancearry = []; var totattendance = []; var emptytable4 = [];
//        function fillPlant_Dispatch_Qty(msg) {
////           scrollTo(0, 0);
//            $('#divMainAddNewRow1').focus();
//            $('#divMainAddNewRow1').css('display', 'block');
//             $('#div_MainPlantDetails').css('display', 'block');
//            var result = [];
//            emptytable4 = [];
//            emptytable5=[];
//            TotalDate = msg[0].daywisedatescls;
//            totattendance = msg[0].linechartvaluesclass;
//            var results = '<div class="divcontainer" style="overflow:auto;"><table border="1" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
//            results += '<thead><tr>';
//            results += '<th scope="col" style="text-align:center;"><i class="fa fa-user" aria-hidden="true"></i>DataType</th><th scope="col" style="text-align:center;"><i class="fa fa-user" aria-hidden="true"></i>BranchName</th>';
//            for (var i = 0; i < TotalDate.length; i++) {
//                results += '<th scope="col" id="txtDate"><i class="fa fa-calendar" aria-hidden="true"></i> ' + TotalDate[i].ThisMonthDate + '</th>';
//            }
//            results += '</tr></thead></tbody>';
//            for (var i = 0; i < totattendance.length; i++) {
//                results += '<tr>';
//                var thismonthtype = totattendance[i].thismonthtype
//                var BranchName = totattendance[i].BranchName
//                if (emptytable4.indexOf(thismonthtype) == -1) {
//                    results += '<td data-title="brandstatus" class="4">' + totattendance[i].thismonthtype + '</td>';
//                    emptytable4.push(thismonthtype);
//                    for (var j = 0; j < TotalDate.length; j++) {
//                        for (var k = 0; k < totattendance.length; k++) {
////                        if(totattendance[k].thismonthtype ==" 
//                            if (TotalDate[j].ThisMonthDate == totattendance[k].IndentDate && thismonthtype == totattendance[k].thismonthtype && BranchName == totattendance[k].BranchName) {
//                                if (emptytable5.indexOf(BranchName) == -1) {
//                               results += '<td data-title="brandstatus" class="4">' + totattendance[i].BranchName + '</td>';
//                                 emptytable5.push(BranchName);
//                                 }
//                                results += '<td  data-title="brandstatus" class="2">' + totattendance[k].dqty + '</td>';
//                            }
//                        }
//                    }
//                    results += '</tr>';
////                    emptytable5=[];
//                }
//                else 
//                {
////                
//                }
//            }
//            results += '</table></div>';
//            $("#div_PlantDetails").html(results);
//        }

TotalDate = []; var attendancearry = []; var totattendance = []; var emptytable4 = [];
//var pageUrl = '<%=ResolveUrl("DashboardNetSale.aspx") %>';
//    function fillPlant_Dispatch_Qty() {
//   $.ajax({
//            type: "POST",
//            url: pageUrl + "/report",
//            data: "", //ur data to be sent to server
//            contentType: "application/json; charset=utf-8", 
//                        
//        });
//    
//    }

   

    function btnRoute_Wise_SaleQty(BranchId) {
        var BranchId;
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        $('#div_routewisemain').css('display', 'block');
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
        $('#div_routewisemain').css('display', 'block');
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
        $("#div_routewise").html(results);
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
        $('#divMainAddNewRow1').css('display', 'block');
        $('#div_routewisemain').css('display', 'block');
        $('#div_agentwisemain').css('display', 'block');
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
        $("#div_agentwise").html(results);
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
                    if (msg[0].type == "GroupWise" || msg[0].type == "CompanyWise") {
                fillPlant_Dispatch_SaleValue(msg);
                }
                else
                {
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
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
        $('#divHide').css('display', 'block');
        var data = { 'operation': 'branchwise_SaleValue', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
        var s = function (msg) {
            if (msg) {
                if (msg == "Session Expired") {
                    alert(msg);
                    window.location = "Login.aspx";
                }
                
                fillbranchwise_Sale_Value(msg);
                  
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
    function branchwisemilkdetails() {
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
          var ddlDataType = document.getElementById('ddlDataType').value;
        $('#divHide').css('display', 'block');
        var data = { 'operation': 'branchwise_Dispatch_milk_qty', 'BranchId': PlantName, 'FromDate': FromDate, 'Todate': Todate,'ddlDataType':ddlDataType };
        var s = function (msg) {
            if (msg) {
                if (msg == "Session Expired") {
                    alert(msg);
                    window.location = "Login.aspx";
                }
                if (msg[0].type == "GroupWise" || msg[0].type == "CompanyWise")
                {
                fillPlant_Dispatch_MilkQty(msg);
                }
                else
                {
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
        var ddlDataType = document.getElementById('ddlDataType').value;
         if(ddlDataType =="Quantity")
         {
        var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th >Milk Qty</th><th>Milk AvgQty</th></tr></thead></tbody>';
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
        else 
        {
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
         var ddlDataType = document.getElementById('ddlDataType').value;
         if(ddlDataType =="Quantity")
         {
        var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th >Dispatch Milk Qty</th><th >Dispatch Milk AvgQty</th></tr></thead></tbody>';
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
        else 
        {
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
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
        var ddlDataType=document.getElementById("ddlDataType").value;
        $('#divHide').css('display', 'block');
        var data = { 'operation': 'branchwise_Dispatch_milk_qty', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate,'ddlDataType':ddlDataType };
        var s = function (msg) {
            if (msg) {
                if (msg == "Session Expired") {
                    alert(msg);
                    window.location = "Login.aspx";
                }
                
                fillbranchwise_Milk_dispqty(msg);
                  
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
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var ddlDataType=document.getElementById("ddlDataType").value;
        $('#divHide').css('display', 'block');
        var data = { 'operation': 'Route_Wise_Milk_SaleQty', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate,'ddlDataType':ddlDataType };
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
        var ddlDataType=document.getElementById("ddlDataType").value;
        if(ddlDataType =="Quantity")
        {
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
        else 
        {
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
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var ddlDataType=document.getElementById("ddlDataType").value;
        $('#divHide').css('display', 'block');
        var data = { 'operation': 'Agent_Wise_Milk_SaleQty', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate,'ddlDataType':ddlDataType };
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
         var ddlDataType=document.getElementById("ddlDataType").value;
        if(ddlDataType =="Quantity")
        {
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
        else 
        {
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
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
        var ddlDataType=document.getElementById("ddlDataType").value;
        $('#divHide').css('display', 'block');
        var data = { 'operation': 'branchwise_Curd_Dispatch_qty', 'BranchId': PlantName, 'FromDate': FromDate, 'Todate': Todate,'ddlDataType':ddlDataType };
        var s = function (msg) {
            if (msg) {
                if (msg == "Session Expired") {
                    alert(msg);
                    window.location = "Login.aspx";
                }
                if (msg[0].type == "GroupWise" || msg[0].type == "CompanyWise")
                {
                fillPlant_Dispatch_CurdQty(msg);
                }
                else
                {
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
        var ddlDataType=document.getElementById("ddlDataType").value;
        if(ddlDataType =="Quantity")
        {
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
        else 
        {
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
         var ddlDataType=document.getElementById("ddlDataType").value;
        if(ddlDataType =="Quantity")
        {
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
        else 
        {
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
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
        var ddlDataType=document.getElementById("ddlDataType").value;
        $('#divHide').css('display', 'block');
        var data = { 'operation': 'branchwise_Curd_Dispatch_qty', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate,'ddlDataType':ddlDataType };
        var s = function (msg) {
            if (msg) {
                if (msg == "Session Expired") {
                    alert(msg);
                    window.location = "Login.aspx";
                }
                
                fillbranchwise_Curd_dispqty(msg);
                  
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
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var ddlDataType=document.getElementById("ddlDataType").value;
        $('#divHide').css('display', 'block');
        var data = { 'operation': 'Route_Wise_Curd_SaleQty', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate,'ddlDataType':ddlDataType };
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
         var ddlDataType=document.getElementById("ddlDataType").value;
        if(ddlDataType =="Quantity")
        {
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
        else 
        {
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
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var ddlDataType=document.getElementById("ddlDataType").value;
        $('#divHide').css('display', 'block');
        var data = { 'operation': 'Agent_Wise_Curd_SaleQty', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate,'ddlDataType':ddlDataType };
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
         var ddlDataType=document.getElementById("ddlDataType").value;
        if(ddlDataType =="Quantity")
        {
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
        else  
        {
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
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
        var ddlDataType=document.getElementById("ddlDataType").value;
        $('#divHide').css('display', 'block');
        var data = { 'operation': 'branchwise_Others_Dispatch_qty', 'BranchId': PlantName, 'FromDate': FromDate, 'Todate': Todate,'ddlDataType':ddlDataType };
        var s = function (msg) {
            if (msg) {
                if (msg == "Session Expired") {
                    alert(msg);
                    window.location = "Login.aspx";
                }
                if (msg[0].type == "GroupWise" || msg[0].type == "CompanyWise")
                {
                fillPlant_Dispatch_OthersQty(msg);
                }
                else
                {
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
        $('#div_MainPlantDetails').css('display', 'block');
        j = 1;
         var ddlDataType=document.getElementById("ddlDataType").value;
        if(ddlDataType =="Quantity")
        {
        var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th>Sale Qty</th><th>Sale AvgQty</th></tr></thead></tbody>';
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
        else 
        {
        var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th >Sale Value</th><th>Sale AvgValue</th></tr></thead></tbody>';
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
        $("#div_PlantDetails").html(results);
    }
    function fillbranchwise_Others_dispqty(msg) {
        scrollTo(0, 0);
        $('#divMainAddNewRow1').css('display', 'block');
        j = 1;
          var ddlDataType=document.getElementById("ddlDataType").value;
        if(ddlDataType =="Quantity")
        {
        var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th>Other Dispatch Qty</th><th>Other Dispatch AvgQty</th></tr></thead></tbody>';
        var k = 1;
        var l = 0;
        for (var i = 0; i < msg.length; i++) {
            results += '<tr style="background-color: antiquewhite;">';
            results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnRoute_Wise_Others_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
            results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].dispatchqty + '</b></td>';
            results += '<td  class="2"><div id="Span1" style="font-size: 12px;font-weight: bold;" class="QtyClass"><b>' + msg[i].AvgQty + '</b></td></tr>';
            l = l + 1;
            if (l == 30) {
                l = 0;
            }
        }
        }
        else 
        {
        var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr style="background-color: #abbed2;"><th >Branch Name</th><th>Other Dispatch Value</th><th>Other Dispatch AvgValue</th></tr></thead></tbody>';
        var k = 1;
        var l = 0;
        for (var i = 0; i < msg.length; i++) {
            results += '<tr style="background-color: antiquewhite;">';
            results += '<td  class="2"><div id="txtamount" style="font-size: 12px; font-weight: bold;cursor:pointer;" title="view Product in Line Chart"  onclick="btnRoute_Wise_Others_SaleQty(\'' + msg[i].BranchID + '\');"  class="subCategeoryClass"><b>' + msg[i].BranchName + '<b></td>';
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
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
        var ddlDataType=document.getElementById("ddlDataType").value;
        $('#divHide').css('display', 'block');
        var data = { 'operation': 'branchwise_Others_Dispatch_qty', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate,'ddlDataType':ddlDataType };
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

    function btnRoute_Wise_Others_SaleQty(BranchId) {
        var BranchId;
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
         var ddlDataType=document.getElementById("ddlDataType").value;
        $('#divHide').css('display', 'block');
        var data = { 'operation': 'Route_Wise_Other_SaleQty', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate,'ddlDataType':ddlDataType };
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
         var ddlDataType=document.getElementById("ddlDataType").value;
        if(ddlDataType =="Quantity")
        {
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
        else 
        {
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
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        $('#divHide').css('display', 'block');
        var ddlDataType=document.getElementById("ddlDataType").value;
        var data = { 'operation': 'Agent_Wise_Other_SaleQty', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate,'ddlDataType':ddlDataType };
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
        var ddlDataType=document.getElementById("ddlDataType").value;
        if(ddlDataType =="Quantity")
        {
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
        else 
        {
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
                if (msg[0].type == "GroupWise" || msg[0].type == "CompanyWise")
                {
                fillPlantUnder_Branch_Wise_Collections(msg);
                }
                else{
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
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
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
                if (msg[0].type == "GroupWise" || msg[0].type == "CompanyWise")
                {
                fillPlantUnder_Branch_Wise_DueAmount(msg);
                }
                else
                {
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


var routewisearry=[];var salestypearr=[];
        function fill_Branch_Wise_DueAmount(msg) {
        scrollTo(0, 0);
        $('#divMainAddNewRow1').css('display', 'block');
        j = 1;
        var BranchTable = [];var totsale = 0;var totpaidamount = 0; var grand_totsale = 0;var grand_totpaidamount = 0;
        var totdiff = 0;var grand_totdiff = 0;
        if(msg[0].type1 =="BranchWise")
        {
        routewisearry=msg;
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
                    totpaidamount=0;
                    totdiff=0;
                    results += '<tr style="background-color: antiquewhite;">';
                    results += '<td scope="row" class="1">' + routewisearry[i].BranchName + '</td>';
                    results += '<td  onclick="btnRoute_Wise_DueAmount_Click(\'' + routewisearry[i].salestypeid +'\');"><span style="text-decoration: underline;color:blue;">' + routewisearry[i].SalesType + '</span></td>';
//                      results += '<td  class="2">' + routewisearry[i].SaleValue + '</td>';
//                      results += '<td  class="2">' + routewisearry[i].PaidAmount + '</td>';
                    results += '<td  class="2">' + routewisearry[i].dispatchqty + '</td></tr>';
                    totsale += parseFloat(routewisearry[i].SaleValue);
                    grand_totsale += parseFloat(routewisearry[i].SaleValue);
                    totpaidamount += parseFloat(routewisearry[i].PaidAmount);
                    grand_totpaidamount += parseFloat(routewisearry[i].PaidAmount);
                    totdiff += parseFloat(routewisearry[i].dispatchqty);
                    grand_totdiff +=parseFloat(routewisearry[i].dispatchqty);;
                    BranchTable.push(routewisearry[i].BranchName);
                }
                else {
                    results += '<tr style="background-color: antiquewhite;">';
                    results += '<td scope="row" class="1" ></td>';
                    results += '<td  onclick="btnRoute_Wise_DueAmount_Click(\'' + routewisearry[i].salestypeid +'\');"><span style="text-decoration: underline;color:blue;">' + routewisearry[i].SalesType + '</span></td>';
//                      results += '<td  class="2">' + routewisearry[i].SaleValue + '</td>';
//                      results += '<td  class="2">' + routewisearry[i].PaidAmount + '</td>';
                    results += '<td  class="2">' + routewisearry[i].dispatchqty + '</td></tr>';
                    totsale += parseFloat(routewisearry[i].SaleValue);
                    grand_totsale +=parseFloat(routewisearry[i].SaleValue);;
                    totpaidamount += parseFloat(routewisearry[i].PaidAmount);
                    grand_totpaidamount +=  parseFloat(routewisearry[i].PaidAmount);
                    totdiff += parseFloat(routewisearry[i].dispatchqty);
                    grand_totdiff +=parseFloat(routewisearry[i].dispatchqty);;
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
            else
            {
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
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
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
        $('#divMainAddNewRow1').css('display', 'block');
        $('#div_routewisemain').css('display', 'block');
        j = 1;
        var BranchTable = [];var totsale = 0;var totpaidamount = 0; var grand_totsale = 0;var grand_totpaidamount = 0;
        var totdiff = 0;var grand_totdiff = 0;
        if(msg[0].type1 !="RouteClassification")
        {
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
        else
        {
        routewisearry=msg;
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
                    totpaidamount=0;
                    totdiff=0;
                    results += '<tr style="background-color: antiquewhite;">';
                    results += '<td scope="row" class="1">' + routewisearry[i].BranchName + '</td>';
                    results += '<td  onclick="btnAgent_Wise_DueAmount_Click(\'' + routewisearry[i].salestypeid +'\');"><span style="text-decoration: underline;color:blue;">' + routewisearry[i].SalesType + '</span></td>';
//                      results += '<td  class="2">' + routewisearry[i].SaleValue + '</td>';
//                      results += '<td  class="2">' + routewisearry[i].PaidAmount + '</td>';
                    results += '<td  class="2">' + routewisearry[i].dispatchqty + '</td></tr>';
                    totsale += parseFloat(routewisearry[i].SaleValue);
                    grand_totsale += parseFloat(routewisearry[i].SaleValue);
                    totpaidamount += parseFloat(routewisearry[i].PaidAmount);
                    grand_totpaidamount += parseFloat(routewisearry[i].PaidAmount);
                    totdiff += parseFloat(routewisearry[i].dispatchqty);
                    grand_totdiff +=parseFloat(routewisearry[i].dispatchqty);
                    BranchTable.push(routewisearry[i].BranchName);
                }
                else {
                    results += '<tr style="background-color: antiquewhite;">';
                    results += '<td scope="row" class="1" ></td>';
                    results += '<td  onclick="btnAgent_Wise_DueAmount_Click(\'' + routewisearry[i].salestypeid +'\');"><span style="text-decoration: underline;color:blue;">' + routewisearry[i].SalesType + '</span></td>';
//                      results += '<td  class="2">' + routewisearry[i].SaleValue + '</td>';
//                      results += '<td  class="2">' + routewisearry[i].PaidAmount + '</td>';
                    results += '<td  class="2">' + routewisearry[i].dispatchqty + '</td></tr>';
                    totsale += parseFloat(routewisearry[i].SaleValue);
                    grand_totsale +=parseFloat(routewisearry[i].SaleValue);;
                    totpaidamount += parseFloat(routewisearry[i].PaidAmount);
                    grand_totpaidamount +=  parseFloat(routewisearry[i].PaidAmount);
                    totdiff += parseFloat(routewisearry[i].dispatchqty);
                    grand_totdiff +=parseFloat(routewisearry[i].dispatchqty);;
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
                if (msg[0].type == "GroupWise" || msg[0].type == "CompanyWise")
                {
                fill_Plant_Under_Branch_Wise_DayIndent(msg);
                }
                else
                {
                fill_Branch_Wise_DayIndent(msg);
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
    function fill_Branch_Wise_DayIndent(msg) {
//            scrollTo(0, 0);

//            $('#divMainAddNewRow1').css('display', 'block');
        $('#div_MainBranchComparison').css('display', 'block');
            $('#div_MainPlantComparison').css('display', 'none');
        j = 1;
        var results = '<div  style="height: 400px;overflow-y: scroll;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr  style="background:#5aa4d0; color: white; font-weight: bold;"><th style="text-align: center;font-weight: bold;">Branch Name</th><th style="text-align: center;font-weight: bold;">YesterDay</th><th style="text-align: center;font-weight: bold;">Last Week</th><th style="text-align: center;font-weight: bold;">Last Month</th><th style="text-align: center;font-weight: bold;">Last Year</th></tr></thead></tbody>';
        var k = 1;
        var l = 0;
        for (var i = 0; i < msg.length; i++) {
            results += '<tr onclick="btnRouteWiseDayComparison(\'' + msg[i].BranchID + '\')">';
            results += '<th  class="2"><div id="txtamount" class="subCategeoryClass">' + msg[i].BranchName + '</th>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterdayaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterdaypercentage + '</div></td>';

            results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekpercentage + '</div></td>';

            results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthpercentage + '</div></td>';

            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthpercentage + '</div></td>';
            results += '<td  class="2"><div id="Span1" style="" class="QtyClass"><div style="float: right;">' + msg[i].lastyearpercentage + '</div></td></tr>';
            l = l + 1;
            if (l == 30) {
                l = 0;
            }
        }
        results += '</table></div>';
        $("#divChart").html(results);
    }
    function fill_Plant_Under_Branch_Wise_DayIndent(msg) {
        scrollTo(0, 0);
            $('#div_MainPlantComparison').css('display', 'block');
            $('#div_MainBranchComparison').css('display', 'none');
        j = 1;
        var results = '<div  style="height: 400px;overflow-y: scroll;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr  style="background:#5aa4d0; color: white; font-weight: bold;"><th style="text-align: center;font-weight: bold;">Plant Name</th><th style="text-align: center;font-weight: bold;">YesterDay</th><th style="text-align: center;font-weight: bold;">Last Week</th><th style="text-align: center;font-weight: bold;">Last Month</th><th style="text-align: center;font-weight: bold;">Last Year</th></tr></thead></tbody>';
        var k = 1;
        var l = 0;
        for (var i = 0; i < msg.length; i++) {
            results += '<tr onclick="btnGroupUnder_PlantClick_Branch_Wise_DayComparison(\'' + msg[i].BranchID + '\')">';
            results += '<th  class="2"><div id="txtamount" class="subCategeoryClass">' + msg[i].BranchName + '</th>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterdayaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterdaypercentage + '</div></td>';

            results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekpercentage + '</div></td>';

            results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthpercentage + '</div></td>';

            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthpercentage + '</div></td>';
            results += '<td  class="2"><div id="Span1" style="" class="QtyClass"><div style="float: right;">' + msg[i].lastyearpercentage + '</div></td></tr>';
            l = l + 1;
            if (l == 30) {
                l = 0;
            }
        }
        results += '</table></div>';
        $("#div_PlantComparison").html(results);
    }

    function btnGroupUnder_PlantClick_Branch_Wise_DayComparison(BranchId) {
        var BranchId;
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
//            $('#divHide').css('display', 'block');
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



    function btnRouteWiseDayComparison(BranchId) {
        var BranchId;
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
        //            $('#divHide').css('display', 'block');
        var data = { 'operation': 'get_RouteWiseDay_Comparison', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
        var s = function (msg) {
            if (msg) {
                if (msg == "Session Expired") {
                    alert(msg);
                    window.location = "Login.aspx";
                }
                fill_RouteWise_DayComparison(msg);
                   
            }
            else {
            }
        };
        var e = function (x, h, e) {
        };
        $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }



    var routewisearry = []; var salestypearr = [];
    function fill_RouteWise_DayComparison(msg) {
//            $('#divMainAddNewRow1').css('display', 'block');
        $('#div_routewisemain').css('display', 'block');
        var results = '<div  style="overflow:auto;"><table border="1" id="tbl_po_print" style="width: 100%;" class="table table-bordered table-hover dataTable no-footer">';
        results += '<thead><tr style="background:antiquewhite;"><th value="Branch Name" style = "font-size: 12px;" colspan="1" rowspan="2">Branch Name</th><th value="YesterDay" style = "font-size: 12px;" colspan="3" rowspan="1">YesterDay</th><th value="Last Week" colspan="3" style = "font-size: 12px;" rowspan="1">Last Week</th><th value="Last Month" style = "font-size: 12px;" colspan="3" rowspan="1">Last Month</th><th value="Last Year" style = "font-size: 12px;" colspan="3" rowspan="1">Last Year</th></tr><tr style="background:antiquewhite;"><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th style = "font-size: 12px;" value="%" colspan="1" rowspan="1">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th style = "font-size: 12px;" value="%" colspan="1" rowspan="1">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th></tr></thead>';


    for (var i = 0; i < msg.length; i++) {
            results += '<tr>';
            results += '<td onclick="btnAgentWiseDayComparison(\'' + msg[i].salestypeid +'\');"><span style="text-decoration: underline;color:blue;">' + msg[i].SalesType + '</span></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterdayaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterdaypercentage + '</div></td>';

            results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekpercentage + '</div></td>';

            results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthpercentage + '</div></td>';

            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthpercentage + '</div></td>';
            results += '<td  class="2"><div id="Span1" style="" class="QtyClass"><div style="float: right;">' + msg[i].lastyearpercentage + '</div></td></tr>';
            }
        results += '</table></div>';
        $("#div_routewise").html(results);
    }
    function btnAgentWiseDayComparison(BranchId) {
        var BranchId;
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
        //            $('#divHide').css('display', 'block');
        var data = { 'operation': 'Get_AgentWiseDay_Comparison', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
        var s = function (msg) {
            if (msg) {
                if (msg == "Session Expired") {
                    alert(msg);
                    window.location = "Login.aspx";
                }
                fill_AgentWise_DayComparison(msg);

            }
            else {
            }
        };
        var e = function (x, h, e) {
        };
        $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }

    function fill_AgentWise_DayComparison(msg) {
        $('#div_routewisemain').css('display', 'block');
        $('#div_agentwisemain').css('display', 'block');

        var BranchTable = []; var totyesterindent = 0; var totlastweekindent = 0; var grand_yesterindent = 0; var grand_lastweekindent = 0;
        var totlastmonthindent = 0; var grand_lastmonthindent = 0; var totlastyearindent = 0; var grand_lastyearindent = 0;
            var results = '<div  style="overflow:auto;"><table border="1" id="tbl_po_print" style="width: 100%;" class="table table-bordered table-hover dataTable no-footer">';
        results += '<thead><tr style="background:antiquewhite;"><th value="Branch Name" style = "font-size: 12px;" colspan="1" rowspan="2">Route Name</th><th value="Branch Name" style = "font-size: 12px;" colspan="1" rowspan="2">Branch Name</th><th value="YesterDay" style = "font-size: 12px;" colspan="3" rowspan="1">YesterDay</th><th value="Last Week" colspan="3" style = "font-size: 12px;" rowspan="1">Last Week</th><th value="Last Month" style = "font-size: 12px;" colspan="3" rowspan="1">Last Month</th><th value="Last Year" style = "font-size: 12px;" colspan="3" rowspan="1">Last Year</th></tr><tr style="background:antiquewhite;"><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th style = "font-size: 12px;" value="%" colspan="1" rowspan="1">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th style = "font-size: 12px;" value="%" colspan="1" rowspan="1">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="1">Qty</th><th style = "font-size: 12px;" value="Avg" colspan="1" rowspan="1">Avg</th><th value="%" colspan="1" rowspan="1" style = "font-size: 12px;">%</th></tr></thead>';
        for (var i = 0; i < msg.length; i++) {
            if (msg[i].yesterindent == "0" && msg[i].lastweekindent == "0" && msg[i].lastweekindent == "0") {
            }
            else {
                if (BranchTable.indexOf(msg[i].routename) == -1) {
                    if (i == 0) {
                    }
                    else {
                        if (totyesterindent > 0 && totlastweekindent > 0 && totlastmonthindent > 0 && totlastyearindent > 0) {
                            results += '<tr>';
                            results += '<td scope="row" class="1" ></td>';
                            results += '<td scope="row" class="1"  style="font-size:18px;font-weight:bold;color:#006400;">Total</td>';
                            results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totyesterindent).toFixed(2) + '</td>';
                                results += '<td scope="row" class="1" ></td>';
                                results += '<td scope="row" class="1" ></td>';
                            results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totlastweekindent).toFixed(2) + '</td>';
                                results += '<td scope="row" class="1" ></td>';
                                results += '<td scope="row" class="1" ></td>';
                            results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totlastmonthindent).toFixed(2) + '</td>';
                                results += '<td scope="row" class="1" ></td>';
                                results += '<td scope="row" class="1" ></td>';
                            results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totlastyearindent).toFixed(2) + '</td></tr>';
                        }
                    }
                    totyesterindent = 0;
                    totlastweekindent = 0;
                    totlastmonthindent = 0;
                    totlastyearindent = 0;
                    results += '<tr>';
                    results += '<td scope="row" class="1">' + msg[i].routename + '</td>';
                    results += '<td onclick="btnAgentWiseDayProductComparison(\'' + msg[i].AgentId + '\');"><span style="text-decoration: underline;color:blue;">' + msg[i].AgentName + '</span></td>';
                    results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterdayaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterdaypercentage + '</div></td>';

            results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekpercentage + '</div></td>';

            results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthpercentage + '</div></td>';

            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthpercentage + '</div></td>';
            results += '<td  class="2"><div id="Span1" style="" class="QtyClass"><div style="float: right;">' + msg[i].lastyearpercentage + '</div></td></tr>';
                    totyesterindent += parseFloat(msg[i].yesterindent) || 0;
                    grand_yesterindent += parseFloat(msg[i].yesterindent) || 0;
                    totlastweekindent += parseFloat(msg[i].lastweekindent) || 0;
                    grand_lastweekindent += parseFloat(msg[i].lastweekindent) || 0;
                    totlastmonthindent += parseFloat(msg[i].lastmonthindent) || 0;
                    grand_lastmonthindent += parseFloat(msg[i].lastmonthindent) || 0;
                    totlastyearindent += parseFloat(msg[i].lastyearindent) || 0;
                    grand_lastyearindent += parseFloat(msg[i].lastyearindent) || 0;
                    BranchTable.push(msg[i].routename);
                }
                else {
                    results += '<tr>';
                    results += '<td scope="row" class="1" ></td>';
                    results += '<td  onclick="btnAgentWiseDayProductComparison(\'' + msg[i].AgentId + '\');"><span style="text-decoration: underline;color:blue;">' + msg[i].AgentName + '</span></td>';
                    results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterdayaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].yesterdaypercentage + '</div></td>';

            results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastweekpercentage + '</div></td>';

            results += '<td  class="2"><div id="txtamount" class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthaverage + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthpercentage + '</div></td>';

            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastyearindent + '</div></td>';
            results += '<td  class="2"><div id="txtamount"  class="subCategeoryClass"><div style="float: right;">' + msg[i].lastmonthpercentage + '</div></td>';
            results += '<td  class="2"><div id="Span1" style="" class="QtyClass"><div style="float: right;">' + msg[i].lastyearpercentage + '</div></td></tr>';
                    totyesterindent += parseFloat(msg[i].yesterindent) || 0;
                    grand_yesterindent += parseFloat(msg[i].yesterindent) || 0;
                    totlastweekindent += parseFloat(msg[i].lastweekindent) || 0;
                    grand_lastweekindent += parseFloat(msg[i].lastweekindent) || 0;
                    totlastmonthindent += parseFloat(msg[i].lastmonthindent) || 0;
                    grand_lastmonthindent += parseFloat(msg[i].lastmonthindent) || 0;
                    totlastyearindent += parseFloat(msg[i].lastyearindent) || 0;
                    grand_lastyearindent += parseFloat(msg[i].lastyearindent) || 0;
                }
            }
        }
        results += '<tr>';
        results += '<td scope="row" class="1" ></td>';
        results += '<td scope="row" class="1" style="font-size:18px;font-weight:bold;color:#006400;" >Total</td>';
        results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totyesterindent).toFixed(2) + '</td>';
            results += '<td scope="row" class="1" ></td>';
            results += '<td scope="row" class="1" ></td>';
        results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totlastweekindent).toFixed(2) + '</td>'
            results += '<td scope="row" class="1" ></td>';
            results += '<td scope="row" class="1" ></td>';
        results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totlastmonthindent).toFixed(2) + '</td>';
            results += '<td scope="row" class="1" ></td>';
            results += '<td scope="row" class="1" ></td>';
        results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totlastyearindent).toFixed(2) + '</td></tr>';
        results += '<tr>';
        results += '<td scope="row" class="1" ></td>';
        results += '<td scope="row" class="1" style="font-size:22px;font-weight:bold;color:#B22222;" >Grand Total</td>';
        results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;">' + parseFloat(grand_yesterindent).toFixed(2) + '</td>';
            results += '<td scope="row" class="1" ></td>';
            results += '<td scope="row" class="1" ></td>';
        results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;">' + parseFloat(grand_lastweekindent).toFixed(2) + '</td>';
            results += '<td scope="row" class="1" ></td>';
            results += '<td scope="row" class="1" ></td>';
        results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;">' + parseFloat(grand_lastmonthindent).toFixed(2) + '</td>';
            results += '<td scope="row" class="1" ></td>';
            results += '<td scope="row" class="1" ></td>';
        results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#B22222;">' + parseFloat(grand_lastyearindent).toFixed(2) + '</td></tr>';
        results += '</table></div>';
        $("#div_agentwise").html(results);
    }


    function btnAgentWiseDayProductComparison(BranchId) {
        var BranchId;
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
        //            $('#divHide').css('display', 'block');
        var data = { 'operation': 'Get_AgentWiseDayProduct_Comparison', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
        var s = function (msg) {
            if (msg) {
                if (msg == "Session Expired") {
                    alert(msg);
                    window.location = "Login.aspx";
                }
                fill_AgentWise_DayProductComparison(msg);

            }
            else {
            }
        };
        var e = function (x, h, e) {
        };
        $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }


    function fill_AgentWise_DayProductComparison(msg) {
//            $('#divMainAddNewRow1').css('display', 'block');
        $('#div_routewisemain').css('display', 'block');
        $('#div_agentwisemain').css('display', 'block');
        $('#div_routewisemainCompare').css('display', 'block');

            
            
        j = 1;
        var tyesterindent = 0; var tlastweekindent = 0; var tlastmonthindent = 0; var tlastyearindent = 0;
        var results = '<div  style="overflow:auto;"><table border="1" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
        results += '<thead><tr  style="background:#5aa4d0; color: white; font-weight: bold;"><th style="text-align: center;font-weight: bold;">Product Name</th><th style="text-align: center;font-weight: bold;">YesterDay</th><th style="text-align: center;font-weight: bold;">Last Week</th><th style="text-align: center;font-weight: bold;">Last Month</th><th style="text-align: center;font-weight: bold;">Last Year</th></tr></thead></tbody>';
        var k = 1;
        var l = 0;
        for (var i = 0; i < msg.length; i++) {
            results += '<tr>';
            results += '<td scope="row">' + msg[i].ProductName + '</td>';
            results += '<td scope="row" class="2" onclick="btnAgentWiseBetweenDayProductComparison(\'' + msg[i].ProductId+'_DayWise' + '\');" ><div style="float:right;padding-right: 10%;">' + msg[i].yesterindent + '</div></td>';
            results += '<td data-title="brandstatus"  class="3" onclick="btnAgentWiseBetweenDayProductComparison(\'' + msg[i].ProductId +"_WeakWise"+ '\');"><div style="float:right;padding-right: 10%;">' + msg[i].lastweekindent + '</div></td>';
            results += '<td data-title="brandstatus"  class="3" onclick="btnAgentWiseBetweenDayProductComparison(\'' + msg[i].ProductId +"_MonthWise"+ '\');"><div style="float:right;padding-right: 10%;">' + msg[i].lastmonthindent + '</div></td>';
            results += '<td data-title="brandstatus"  class="3" onclick="btnAgentWiseBetweenDayProductComparison(\'' + msg[i].ProductId +"_YearWise"+ '\');"><div style="float:right;padding-right: 10%;">' + msg[i].lastyearindent + '</div></td></tr>';
            tyesterindent += parseFloat(msg[i].yesterindent) || 0;
            tlastweekindent += parseFloat(msg[i].lastweekindent) || 0;
            tlastmonthindent += parseFloat(msg[i].lastmonthindent) || 0;
            tlastyearindent += parseFloat(msg[i].lastyearindent) || 0;
        }
        results += '<tr>';
        results += '<td scope="row" class="1">Total</td>';
        results += '<td scope="row" class="1" style="font-size:18px;font-weight:bold;color:#006400;" ><div style="float:right;padding-right: 7%;">' + tyesterindent + '</div></td>';
        results += '<td   style="font-size:18px;font-weight:bold;color:#006400;"><div style="float:right;padding-right: 7%;">' + tlastweekindent + '</div></td>';
        results += '<td   style="font-size:18px;font-weight:bold;color:#006400;"><div style="float:right;padding-right: 7%;">' + tlastmonthindent+ '</div></td>';
        results += '<td   style="font-size:18px;font-weight:bold;color:#006400;"><div style="float:right;padding-right: 7%;">' + tlastyearindent+ '</div></td></tr>';
        results += '</table></div>';
        $("#div_routewiseCompare").html(results);
    }



    function btnAgentWiseBetweenDayProductComparison(BranchId) {
        var BranchId;
        var FromDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
        //            $('#divHide').css('display', 'block');
        var data = { 'operation': 'Get_AgentWiseBetweenDayProduct_Comparison', 'BranchId': BranchId, 'FromDate': FromDate, 'Todate': Todate };
        var s = function (msg) {
            if (msg) {
                if (msg == "Session Expired") {
                    alert(msg);
                    window.location = "Login.aspx";
                }
                FillAgentProductDayComparisonDetails(msg);

            }
            else {
            }
        };
        var e = function (x, h, e) {
        };
        $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }

        function FillAgentProductDayComparisonDetails(databind) {
            
//             $('#divMainAddNewRow1').css('display', 'block');
        $('#div_routewisemain').css('display', 'block');
        $('#div_agentwisemain').css('display', 'block');
        $('#div_routewisemainCompare').css('display', 'block');
        $('#example').css('display', 'block');

            
//            var myTableDiv = document.getElementById("example");
//            var divleakbar = document.createElement("div");
////            divleakbar.style.height = "300px";
//            divleakbar
////            divleakbar.style.cssText = 'height:300;.backgroundColor:#f00';
////                        did++;
//                        var idvalue = "ProductWise";
//                        divleakbar.setAttribute("id", "idvalue");
//            myTableDiv.appendChild(divleakbar);
        var textname = "";
        var sta = databind[0].ProductName;
        datainXSeries = 0;
        datainYSeries = 0;
        newYarray = [];
        newXarray = [];
        newSalearray = [];
        newDuearray = [];
    textname =" Qty       "+sta;
        for (var k = 0; k < databind.length; k++) {
            var BranchName = [];
            var IndentDate = databind[0].IndentDate;
            //                var UnitQty = databind[k].UnitQty;
            var SaleValue = databind[0].dqty;
//             
            newXarray = IndentDate.split(',');
//              
            newSalearray = SaleValue.split(',');

            var titlename   =""+textname+"-"+newSalearray[0]+"";

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


//        function fill_AgentWise_BetweenDayProductComparison(msg) {
//            $('#divMainAddNewRow1').css('display', 'block');
//            $('#div_routewisemain').css('display', 'block');
//            $('#div_agentwisemain').css('display', 'block');
//            $('#div_routewisemainCompare').css('display', 'block');
//            $('#div_AgentDaywiseProductMain').css('display', 'block');

//            
//            
//            j = 1;
//            var tyesterindent = 0; var tlastweekindent = 0; var tlastmonthindent = 0; var tlastyearindent = 0;
//            var results = '<div  style="overflow:auto;"><table border="1" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
//            results += '<thead><tr  style="background:#5aa4d0; color: white; font-weight: bold;"><th style="text-align: center;font-weight: bold;">Product Name</th><th style="text-align: center;font-weight: bold;">AgentId</th><th style="text-align: center;font-weight: bold;">YesterDay</th><th style="text-align: center;font-weight: bold;">Last Week</th><th style="text-align: center;font-weight: bold;">Last Month</th><th style="text-align: center;font-weight: bold;">Last Year</th></tr></thead></tbody>';
//            var k = 1;
//            var l = 0;
//            for (var i = 0; i < msg.length; i++) {
//                results += '<tr>';
//                results += '<td scope="row" onclick="btnAgentWiseDayProductComparison(\'' + msg[i].ProductId + '\');">' + msg[i].ProductName + '</td>';
//                results += '<td scope="row" class="2" ><div style="float:right;padding-right: 10%;">' + msg[i].yesterindent + '</div></td>';
//                results += '<td data-title="brandstatus"  class="3"><div style="float:right;padding-right: 10%;">' + msg[i].lastweekindent + '</div></td>';
//                results += '<td data-title="brandstatus"  class="3"><div style="float:right;padding-right: 10%;">' + msg[i].lastmonthindent + '</div></td>';
//                results += '<td data-title="brandstatus"  class="3"><div style="float:right;padding-right: 10%;">' + msg[i].lastyearindent + '</div></td></tr>';
//                tyesterindent += parseFloat(msg[i].yesterindent) || 0;
//                tlastweekindent += parseFloat(msg[i].lastweekindent) || 0;
//                tlastmonthindent += parseFloat(msg[i].lastmonthindent) || 0;
//                tlastyearindent += parseFloat(msg[i].lastyearindent) || 0;
//            }
//            results += '<tr>';
//            results += '<td scope="row" class="1">Total</td>';
//            results += '<td scope="row" class="1" style="font-size:18px;font-weight:bold;color:#006400;" ><div style="float:right;padding-right: 7%;">' + parseFloat(tyesterindent).toFixed(2) || 0 + '</div></td>';
//            results += '<td   style="font-size:18px;font-weight:bold;color:#006400;"><div style="float:right;padding-right: 7%;">' + parseFloat(tlastweekindent).toFixed(2) || 0 + '</div></td>';
//            results += '<td   style="font-size:18px;font-weight:bold;color:#006400;"><div style="float:right;padding-right: 7%;">' + parseFloat(tlastmonthindent).toFixed(2) || 0 + '</div></td>';
//            results += '<td   style="font-size:18px;font-weight:bold;color:#006400;"><div style="float:right;padding-right: 7%;">' + parseFloat(tlastyearindent).toFixed(2) || 0 + '</div></td></tr>';
//            results += '</table></div>';
//            $("#div_AgentDaywiseProductDetails").html(results);
//        }

        
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
        $('#example').css('display', 'none');
    }
    function PlantCloseClick() {
        $('#div_MainPlantDetails').css('display', 'none');
    }
    
    function GraphicalNetSaleClick() {
        $('#divMainAddNewRow1').css('display','none');
        $('#divHide').css('display','none');
        var IndDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
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
            var ddlbranchcategory = document.getElementById('ddlbarnchCategory').value;
            if(ddlbranchcategory =="CategoryWiseDespatch")
            {
                fillProdcutData();
                $('#spnDueValue').css('display', 'none ');
                $('#spnCollection').css('display', 'none ');
                $('#spnOthersDispatchQty').css('display', 'block ');
                $('#spnCurdDispatchQty').css('display', 'block ');
                $('#spnMilkDispatchQty').css('display', 'block ');
                $('#spnSaleValue').css('display', 'none ');
                $('#spnDispatchQty').css('display', 'none ');
                $('#Branch_wise_Dispatch').css('display', 'none ');
                $('#example1').css('display', 'block');
            }
            else if(ddlbranchcategory=="BranchWiseCollections")
            {
             GetBranchwiseDetails();
//              $('#example1').css('display', 'block');
              $('#firstdiv').css('display', 'block ');
//            BranchIndentDayWiseComparison();
//            $('#SpanDetails').css('display', 'none ');
//            $('#Branch_wise_Dispatch').css('display', 'none ');
             $('#spnSaleValue').css('display', 'block ');
            $('#spnDueValue').css('display', 'block ');
            $('#spnCollection').css('display', 'block ');
             $('#spnOthersDispatchQty').css('display', 'none ');
            $('#spnCurdDispatchQty').css('display', 'none ');
            $('#spnMilkDispatchQty').css('display', 'none ');
              $('#spnDispatchQty').css('display', 'none ');

               $('#div_MainPlantComparison').css('display', 'none');
            $('#example1').css('display', 'none');
            }
            else if(ddlbranchcategory=="BranchWiseDespatch"){
           
           GetBranchwiseDetails();
             $('#spnSaleValue').css('display', 'none ');
            $('#spnDueValue').css('display', 'none ');
            $('#spnCollection').css('display', 'none ');
              $('#spnOthersDispatchQty').css('display', 'none ');
            $('#spnCurdDispatchQty').css('display', 'none ');
            $('#spnMilkDispatchQty').css('display', 'none ');
            $('#firstdiv').css('display', 'block ');
            $('#spnDispatchQty').css('display', 'block ');
            $('#div_MainPlantComparison').css('display', 'none');
            $('#example1').css('display', 'none');
            }
    }
    var TotalProductclass=[];var SubTotalProductclass=[];
    function fillProdcutData() {
        var IndDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
        var branchCategory = document.getElementById('ddlbarnchCategory').value;
            
        var data = { 'operation': 'GetProductInformation', 'IndDate': IndDate, 'BranchID': PlantName, 'Todate':Todate,'Type': Type };
        var s = function (msg) {
            if (msg) {
                $('#firstdiv').css('display', 'block ');
                TotalProductclass = msg[0].TotalProductclass;
                SubTotalProductclass= msg[0].SubTotalProductclass;
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
        
    function fillsubcategorywiseproductsdata(msg) {
    var BranchTable = [];
        var temp=document.getElementById("ddlDataType").value;
        j = 1;
        var pdelivaryqty = 0; var psalevalue = 0;
        if(temp =="Quantity")
        {
        var results = '<div  style="overflow:auto;"><table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;" id="tableaProductdetails" class="mainText2" border="1">';
        results += '<thead><tr><th style="width: 33%; text-align: center; height: 20px; font-size: 16px; font-weight: bold;color: #2f3293;">Category Name</th><th style="width: 33%; height: 20px; text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">SubCategory Name</th><th style="width: 33%; height: 20px; text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">DelivaryQty</th><th style="width: 33%; height: 20px; text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">Qty(%)</th></tr></thead></tbody>';
        var k = 1;
        var l = 0;
        var COLOR = [ "#794b4b","#FF6600","#fdd400","#84b761","#cc4748","#cd82ad","#2f4074","#448e4d","#b7b83f","#b9783f","#b93e3d","#913167","#18d79c","#a2907f","#41ee80","#8cfa40","#dbdf9d","#3f4fb2","#01909b","#124c65","#33879b","#869ae2"];
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
        else
        {
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
        else 
        {
         var results = '<div  style="overflow:auto;"><table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;" id="tableaProductdetails" class="mainText2" border="1">';
        results += '<thead><tr><th style="width: 33%; text-align: center; height: 20px; font-size: 16px; font-weight: bold;color: #2f3293;">Category Name</th><th style="width: 33%; height: 20px; text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">SubCategory Name</th><th style="width: 33%; height: 20px; text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">SaleValue</th><th style="width: 33%; height: 20px; text-align: center; font-size: 16px; font-weight: bold;color: #2f3293;">Value(%)</th></tr></thead></tbody>';
        var k = 1;
        var l = 0;
        var COLOR = [ "#794b4b","#FF6600","#fdd400","#84b761","#cc4748","#cd82ad","#2f4074","#448e4d","#b7b83f","#b9783f","#b93e3d","#913167","#18d79c","#a2907f","#41ee80","#8cfa40","#dbdf9d","#3f4fb2","#01909b","#124c65","#33879b","#869ae2","#000000"];
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
        else
        {
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
    function CloseClick1() {
        $('html, body').animate({
            scrollTop: $("#BranchWiseDespatch").offset().down
        }, 2000);
        $('#BranchWiseDespatch').focus();
        $('#divMainAddNewRow1').css('display', 'none');
    }


    var GroupWiseArr=[];var Branchwise=[];
    function GetBranchwiseDetails() {

        var IndDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var Todate = $('#reportrange').data('daterangepicker').endDate.toString();
        var PlantName = document.getElementById('ddlPlant').value;
        var ddlDataType = document.getElementById('ddlDataType').value;
        var ddlbarnchCategory = document.getElementById('ddlbarnchCategory').value;
        
        var data = { 'operation': 'GetBranchwiseDetailsInformation', 'IndDate': IndDate,'Todate':Todate, 'BranchID': PlantName, 'Type': Type,'ddlDataType':ddlDataType,'ddlbarnchCategory':ddlbarnchCategory };
        var s = function (msg) {
            if (msg) {

                GroupWiseArr = msg[0].GroupWiseDetils;
                Branchwise= msg[0].ApprovalPlanDetails;
                fillcategorywisedispatchsale(GroupWiseArr);
               var ddlDataType=document.getElementById("ddlDataType").value;
               var ddlbarnchCategory = document.getElementById('ddlbarnchCategory').value;
              if(ddlbarnchCategory =="BranchWiseCollections")
              {
              ddlDataType="Value";
              }
//                ddlDataType
            if(ddlDataType =="Quantity")
               {
                $('#BranchWiseDespatch').removeTemplate();
                $('#BranchWiseDespatch').setTemplateURL('BranchDespatches6.htm');
                $('#BranchWiseDespatch').processTemplate(Branchwise);
                }
                else 
                {
                 $('#BranchWiseDespatch').removeTemplate();
                $('#BranchWiseDespatch').setTemplateURL('BranchDespatchesValue.htm');
                $('#BranchWiseDespatch').processTemplate(Branchwise);
                }
                  
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






    function btnClickCategoryId(branchname) {
        var branchname;
        var startDate = $('#reportrange').data('daterangepicker').startDate.toString();
        var endDate = $('#reportrange').data('daterangepicker').endDate.toString();
            var ddlDataType=document.getElementById("ddlDataType").value;
        var data = { 'operation': 'GetCategoryWiseChart', 'startDate': startDate, 'endDate': endDate,  'branchname': branchname,'ddlDataType':ddlDataType };
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
    var tbldata=[];
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

//            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
//            results += '<thead><tr><th scope="col">BranchName</th><th scope="col">DelivaryQty</th><th scope="col">AvgQty</th></tr></thead></tbody>';
//            var k = 1;
//            var l = 0;
//            var Color = ["#FF0F00", "#FF6600", "#FF9E01", "#FCD202", "#F8FF01", "#F8FF01", "#B0DE09", "#04D215", "#0D8ECF", "#40ff00", "#00ff40", " #00ff80", " #00ffff", " #0080ff", "#0D52D1", "#2A0CD0", "#8A0CCF", "#CD0D74", "#754DEB", "#DDDDDD", "#999999", "#333333", "#000000"];
//            for (var i = 0; i < tbldata.length; i++) {
//                results += '<tr style="background-color:' + Color[l] + '">';
//                results += '<td scope="row" onclick="btnClickCategoryId(\'' + tbldata[i].Routeid + '\');"  class="2" >' + tbldata[i].RouteName + '</th>';
//                results += '<td scope="row" class="2" ><div style="float:right;padding-right: 10%;">' + tbldata[i].DeliveryQty + '</div></th>';
//                results += '<td data-title="brandstatus"   class="3"><div style="float:right;padding-right: 10%;">' + tbldata[i].AverageyQty + '</div></td></tr>';
//                l = l + 1;
//                //                if (l == 4) {
//                //                    l = 0;
//                //                }
//                DeliveryQty += parseFloat(tbldata[i].DeliveryQty) || 0;
//                AverageyQty += parseFloat(tbldata[i].AverageyQty) || 0;
//            }
//            results += '<tr>';
//            results += '<td scope="row" class="1">Total</td>';
//            results += '<td scope="row" class="1" style="font-size:18px;font-weight:bold;color:#006400;" ><div style="float:right;padding-right: 7%;">' + parseFloat(DeliveryQty).toFixed(2) + '</div></td>';
//            results += '<td   style="display:none;font-size:18px;font-weight:bold;color:#006400;"><div style="float:right;padding-right: 7%;">' + parseFloat(AverageyQty).toFixed(2) + '</div></td></tr>';
//            results += '</table></div>';
//            $("#id_table_data").html(results);
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
        var type =document.getElementById("ddlDataType").value;
        if(databind.length >0)
        {
        if(type =="Quantity")
        {
        var RouteName = databind[0].subCategeoryName;
        var leakqty = databind[0].Qty;

        var color = [ "#794b4b", "#FF6600", "#fdd400", "#84b761", "#cc4748", "#cd82ad", "#2f4074", "#448e4d", "#b7b83f", "#b9783f", "#b93e3d", "#913167", "#18d79c", "#a2907f", "#41ee80", "#8cfa40", "#dbdf9d", "#3f4fb2", "#01909b", "#124c65", "#33879b", "#869ae2", "#000000"];

        for (var i = 0; i < databind.length; i++) {
            newXarray.push({ "category": databind[i].subCategeoryName, "value": parseFloat(databind[i].Qty), "color": color[i] });
        }
        }
        else 
        {
         var RouteName = databind[0].subCategeoryName;
        var leakqty = databind[0].SaleValue;
        var color = [ "#794b4b", "#FF6600", "#fdd400", "#84b761", "#cc4748", "#cd82ad", "#2f4074", "#448e4d", "#b7b83f", "#b9783f", "#b93e3d", "#913167", "#18d79c", "#a2907f", "#41ee80", "#8cfa40", "#dbdf9d", "#3f4fb2", "#01909b", "#124c65", "#33879b", "#869ae2", "#000000"];
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
  
  
    




   
  


    
    function btnShowRouteDetails(ID) {
        var DespSno = ID.value;
        GetAgentDetails(DespSno);
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
    function CallFunction() {
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600">
                                    </asp:ToolkitScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>

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
                <div style="width: 100%; padding-left: 15%;">
                    <table>
                        <tr>
                            <td style="width: 130px;">
                                <select id="ddlType" class="form-control" onchange="ddlTypeChange(this);">
                                    <option>Vyshnavi Group</option>
                                    <option>SVDS</option>
                                    <option>SVF</option>
                                </select>
                            </td>
                            <td style="width: 2%;">
                            </td>
                            <td style="width: 130px; display: none;" id="divPlant">
                                <select id="ddlPlant" class="form-control" onchange="ddlPlantNameChange(this);">
                                </select>
                            </td>
                            <td style="width: 2%;">
                            </td>
                            <td style="width: 130px;" id="tdddlbarnchCategory">
                                <select id="ddlbarnchCategory" class="form-control" onchange="branchtypeChange();">
                                    <option>BranchWiseDespatch</option>
                                    <option>BranchWiseCollections</option>
                                    <option>CategoryWiseDespatch</option>
                                </select>
                            </td>
                            <td style="width: 2%;">
                            </td>
                            <td style="width: 2%;">
                            </td>
                            <td style="width: 130px;" id="tddatatypeid">
                                <select id="ddlDataType" class="form-control">
                                    <option>Quantity</option>
                                    <option>Value</option>
                                </select>
                            </td>
                            <td style="width: 2%;">
                            </td>
                            <td>
                                <div id="reportrange" style="background: #fff; cursor: pointer; padding: 5px 10px;
                                    border: 1px solid #ccc; width: 100%">
                                    <i class="fa fa-calendar"></i>&nbsp; <span></span><i class="fa fa-caret-down"></i>
                                </div>
                            </td>
                            <td>
                                &nbsp; &nbsp;
                            </td>
                            <td>
                                <input type="button" id="submit" value="Submit" class="btn btn-primary" onclick="GraphicalNetSaleClick()" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div style="display: none;" id="firstdiv">
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
                                    <div class="col-lg-3 col-xs-6" id="spnDispatchQty" style="width: 82%; padding-left: 250px !important">
                                        <!-- small box -->
                                        <div class="small-box bg-aqua">
                                            <div class="inner">
                                                <h3 id="hdispatchqty">
                                                    0
                                                </h3>
                                                <p id="spndespatchid">
                                                </p>
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
                                    <div class="col-lg-3 col-xs-6" id="spnSaleValue">
                                        <!-- small box -->
                                        <div class="small-box bg-purple">
                                            <div class="inner">
                                                <h3 id="hsalevalue">
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
                                        </div>
                                    </div>
                                    <!-- ./col -->
                                    <div class="col-lg-3 col-xs-6" id="spnMilkDispatchQty" style="height: 20px;">
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
                                    <div class="col-lg-3 col-xs-6" id="spnCurdDispatchQty">
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
                                    <div class="col-lg-3 col-xs-6" id="spnOthersDispatchQty">
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
                                            <a href="#datagrid" class="small-box-footer" onclick="branchwiseotherdetails();"
                                                data-toggle="modal" data-target="#myModal">BranchWise Others Qty <i class="fa fa-arrow-circle-right">
                                                </i></a>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-xs-6" id="spnCollection">
                                        <!-- small box -->
                                        <div class="small-box bg-teal">
                                            <div class="inner">
                                                <h3 id="spnamount">
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
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-xs-6" id="spnDueValue">
                                        <!-- small box -->
                                        <div class="small-box bg-blue">
                                            <div class="inner">
                                                <h3 id="hduevalue">
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
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12" id="Branch_wise_Dispatch">
                        <div id="BranchWiseDespatch">
                        </div>
                    </div>
                    <div class="col-sm-12" style="width: 100%; display: none;" id="div_MainPlantComparison">
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
                    <div id="example1" class="k-content" class="col-sm-12">
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
                    <div id="divHide" style="width: 120%; display: none;">
                        <div class="modal fade in" id="div_MainPlantDetails" style="display: none; padding-right: 17px;">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true" onclick="CloseClick1();">×</span></button>
                                        <h4 class="modal-title">
                                            Plant Wise Details</h4>
                                    </div>
                                   
                                    <div style="display: none;">
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
                                    <div class="modal-body" id="div_PlantDetails" style="height: 400px; overflow-y: scroll;">
                                        
                                            <asp:Button ID="btnsess" runat="server" Text="Submit"  OnClick="btnsessionvalue_Click" Style="display: none" />
                                                <asp:GridView ID="grdrpt" runat="server">
                                                </asp:GridView>
                                                <asp:GridView ID="grddata" runat="server" Style="width: 100%;" Font-Bold="true" OnRowCommand="grdReports_RowCommand">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Button ID="Button1" runat="server" Text="View" CssClass="btn btn-success" CommandArgument='<%#Container.DataItemIndex%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                          
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
                        <div class="modal fade in" id="divMainAddNewRow1" style="display: none; padding-right: 17px;">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true" onclick="CloseClick1();">×</span></button>
                                        <h4 class="modal-title">
                                            Branch Wise Details</h4>
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
                    </div>
                    <div class="modal fade in" id="div_routewisemain" style="display: none; padding-right: 17px;
                        width: 110%;">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true" onclick="close_routewise();">×</span></button>
                                    <h4 class="modal-title">
                                        Route Wise Details</h4>
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
                    <div class="modal fade in" id="div_agentwisemain" style="display: none; padding-right: 17px;">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true" onclick="close_agentwise();">×</span></button>
                                    <h4 class="modal-title">
                                        Agent Wise Details</h4>
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
                    <div class="modal fade in" id="div_routewisemainCompare" style="display: none; padding-right: 17px;
                        width: 110%;">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true" onclick="close_routewiseCompare();">×</span></button>
                                    <h4 class="modal-title">
                                        Product Wise Details</h4>
                                </div>
                                <div class="modal-body" id="div_routewiseCompare" style="height: 400px; overflow-y: scroll;">
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-danger" onclick="close_routewiseCompare();">
                                        Close</button>
                                </div>
                            </div>
                            <!-- /.modal-content -->
                        </div>
                        <!-- /.modal-dialog -->
                    </div>
                    <div class="modal fade in" id="example" style="display: none; padding-right: 17px;
                        width: 110%;">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true" onclick="close_AgentProductLineChart();">×</span></button>
                                    <h4 class="modal-title">
                                        Day Wise Product Details</h4>
                                </div>
                                <div class="modal-body" id="ProductWiseChart" style="height: 400px; overflow-y: scroll;">
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-danger" onclick="close_AgentProductLineChart();">
                                        Close</button>
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
                                        Route Wise Details</h4>
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
                </div>
            </div>
        </div>
    </section>
      </ContentTemplate>
                                        </asp:UpdatePanel>
</asp:Content>
