<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="IndentAssignMent.aspx.cs" Inherits="IndentAssignMent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <%--<script src="src/jquery-1.6.1.min.js" type="text/javascript"></script>--%>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
     <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <%-- <script src="src/ui.dropdownchecklist.js" type="text/javascript"></script>
    <link href="src/ui.dropdownchecklist.standalone.css" rel="stylesheet" type="text/css" />
    <link href="src/ui.dropdownchecklist.themeroller.css" rel="stylesheet" type="text/css" />
    --%>
     <script type="text/javascript">
         $(function () {
             window.history.forward(1);
         });
        
    </script>
    <style type="text/css">
         .ddlsize
        {
            width: 200px;
            height: 30px;
            font-size: 16px;
            border: 1px solid gray;
            border-radius: 7px 7px 7px 7px;
        }
        .datepicker
        {
            border: 1px solid gray;
            background: url("Images/CalBig.png") no-repeat scroll 99%;
              width: 70%;
            top: 0;
            left: 0;
            height: 20px;
            font-weight: 700;
            font-size: 12px;
            cursor: pointer;
            border: 1px solid gray;
            margin: .5em 0;
            padding: .6em 20px;
            border-radius: 10px 10px 10px 10px;
            filter: Alpha(Opacity=0);
            box-shadow: 3px 3px 3px #ccc;
        }
        input[type=number]::-webkit-inner-spin-button,
         input[type=number]::-webkit-outer-spin-button
        {
            -webkit-appearance: none;
            margin: 0;
        }
    </style>
    <script type="text/javascript">
        var values = "";
        var today = "";
        $(function () {
            GetPlanDetails();
            //            $("#datepicker").datepicker({ dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
            //                onSelect: function (selectedDate) {
            //                }
            //            });
            //            var date = new Date();
            //            var day = date.getDate();
            //            var month = date.getMonth() + 1;
            //            var year = date.getFullYear();
            //            if (month < 10) month = "0" + month;
            //            if (day < 10) day = "0" + day;
            //            today = year + "-" + month + "-" + day;
            //            $('#txtDate').val(today);
            //            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        });
        var DispName = "";
        var DispType = "";
        var DispSno = "";
        var DispatchStatus = "";
        var SOBranchId = "";
        var DispatchDate = "";
        var VehicleNo = "";
        function GetPlanDetails() {
            var data = { 'operation': 'GetTripDispPlanDetails' };
            var s = function (msg) {
                if (msg) {
                    DispName = msg[0].DispName;
                    DispType = msg[0].DispType;
                    DispSno = msg[0].DispSno;
                    DispatchStatus = msg[0].DispatchStatus;
                    SOBranchId = msg[0].SOBranchId;
                    DispatchDate = msg[0].DispatchDate;
                    VehicleNo = msg[0].VehicleNo;
                    document.getElementById('txtDispName').innerHTML = msg[0].DispName;
                    if (DispatchStatus == "Load") {
                        document.getElementById('txtVehicle').innerHTML = VehicleNo;
                        $('#divEmp').css('display', 'none');
                        $('#lblEmp').css('display', 'none');
//                        $('#txtVehicle').css('display', 'block');
//                        $('#txtVehicleNo').css('display', 'none');
                        $('#BtnPrintDetails').css('display', 'none');
                        $('#LoadPrint').css('display', 'block');
                        $('#btnclick').css('display', 'none');
                        btnclickhere();
                        btnclickhere();
                    }
                    else {
                        $('#LoadPrint').css('display', 'none');
                        $('#BtnPrintDetails').css('display', 'block');
//                        $('#txtVehicleNo').css('display', 'block');
//                        $('#txtVehicle').css('display', 'none');
                        $('#divEmp').css('display', 'block ');
                        $('#lblEmp').css('display', 'block');
//                        ddlFromDispatchChange(DispSno, DispType);
                        $('#btnclick').css('display', 'block');
                    }
                    //                   btnclickhere();
                    btnclickhere();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var Operationvalues = "";
        var CheckStatus = "";
        function btnclickhere() {
            GetBranchProducts();
            GetBranchInventory();
        }
        function FillRouteName() {
            var data = { 'operation': 'get_Plant_TripRoutes' };
            var s = function (msg) {
                if (msg) {
                    fillTriproutes(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function CallHandlerUsingJson(d, s, e) {
            $.ajax({
                type: "GET",
                url: "DairyFleet.axd?json=",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(d),
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }
        function GetBranchProducts() {
            var EmpID = "" ;
            var data = { 'operation': 'get_Plant_Trip_RouteNameChange', 'EmpID': EmpID, 'routename': DispSno, 'DispatchStatus': DispatchStatus };
            var s = function (msg) {
                if (msg) {
                    GetProducts(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(data, s, e);
        }
        function GetBranchInventory() {
            var data = { 'operation': 'Gettripinventory_manage' };
            var s = function (msg) {
                if (msg) {
//                    GetInventory(msg);
                    if (DispatchStatus == "Load") {
                        var date = new Date();
                        var day = date.getDate();
                        var month = date.getMonth() + 1;
                        var year = date.getFullYear();
                        if (month < 10) month = "0" + month;
                        if (day < 10) day = "0" + day;
                        var today = year + "-" + month + "-" + day;
                        document.getElementById('txtTime').innerHTML = today;
                        $('#divTime').css('display', 'block');
                        $('#DivSave').css('display', 'none');
                        $('#LoadPrint').css('display', 'block');
                        $('#btn_getlayout').css('display', 'block');
                        $('#divtblexaxis').css('display', 'block');
                        layoutproducts();
                    }
                    else {
                        $('#divTime').css('display', 'none');
                        $('#DivSave').css('display', 'block');
                        $('#LoadPrint').css('display', 'none');
                        $('#btn_getlayout').css('display', 'none');
                        $('#divtblexaxis').css('display', 'none');
                        //                        layoutproducts();
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
        function GetProducts(msg) {
            $('#divFillScreen').removeTemplate();
            $('#divFillScreen').setTemplateURL('PuffPlanTemplate1.htm');
            $('#divFillScreen').processTemplate(msg);
        }
//        function GetInventory(msg) {
//            $('#divInventory').removeTemplate();
//            $('#divInventory').setTemplateURL('TripInventory.htm');
//            $('#divInventory').processTemplate(msg);
//        }
        //        divInventory
        function btnPuffPlanningSaveClick() {
            document.getElementById("BtnSave").disabled = true;

            var datatab = [];
            var Employee = "" ;
            var Date = today;
            var txtVehicleN = "";
            var rows = $("#tabledetails tr:gt(0)");
            var Orderdetails = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtProductQty').val() != "") {
                    Orderdetails.push({ ProductSno: $(this).find('#hdnProductSno').val(), Product: $(this).find('#txtproduct').text(), Qty: $(this).find('#txtProductQty').val(), RemainingQty: $(this).find('#txtremainingqty').text() });
                }
            });
            //table_inventory_details
            Operationvalues = "D";
            var Data = { 'operation': 'btnPuffPlanningSaveClick', 'data': Orderdetails, 'EmpID': Employee, 'routename': DispSno, 'Permissions': Operationvalues, 'VehicleNo': txtVehicleN };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    if (msg == "Data Successfully Saved") {
                        $('#divFillScreen').removeTemplate();
                        $('#divFillScreen').setTemplateURL('PuffPlanTemplate1.htm');
                        $('#divFillScreen').processTemplate(datatab);
                        alert(msg);
                        btnPlantTripRefreshClick();
                    }
                    if (msg == "Data Successfully Updated") {
                        $('#divFillScreen').removeTemplate();
                        $('#divFillScreen').setTemplateURL('PuffPlanTemplate1.htm');
                        $('#divFillScreen').processTemplate(datatab);
                        alert(msg);
                        btnPlantTripRefreshClick();
                    }
                    else {
                        alert(msg);
                        btnPlantTripRefreshClick();
                    }
                }
                else {
                    alert(msg);
                }
                document.getElementById("BtnSave").disabled = true;

            };
            var e = function (x, h, e) {
                document.getElementById("BtnSave").disabled = false;

            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(Data, s, e);
        }
        function CallHandlerUsingJson(d, s, e) {
            $.ajax({
                type: "GET",
                url: "DairyFleet.axd?json=",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(d),
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }
        function GetPrintDetails() {
            window.location = "DeliveryChallanReport.aspx";
        }
        function btnPlantTripRefreshClick() {
            Operationvalues = "";
            CheckStatus = "";
            window.location = "Puffplan.aspx";
        }
        function numberOnlyExample() {
            if ((event.keyCode < 48) || (event.keyCode > 57))
                return false;
        }
        function OrderUnitChange(qty) {
            document.getElementById('txtProductQty').innerHTML = qty;
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
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div  style="background-color: #FFFFFF;">
        <table style="padding-left: 400px">
            <tr>
                <td>
                    <label class="headers">
                        TRIP ASSIGNMENT</label>
                </td>
            </tr>
        </table>
        <div id="divPrint">
            <div>
                <table>
                    <tr>
                        <td id="divTime" style="display: none;">
                            Date <span id="txtTime" style="color: Blue; font-size: 18px; font-weight: bold;">
                            </span>
                        </td>
                        <td>
                            Route Name <span id="txtDispName" style="color: Red; font-size: 18px; font-weight: bold;">
                            </span>
                        </td>
                       <%-- <td>
                            <label>
                                Vehicle No</label>
                        </td>
                        <td>
                            <select id="txtVehicleNo" class="dtimesize">
                            </select>
                            <span id="txtVehicle" style="color: Red; font-size: 18px; font-weight: bold; display: none;">
                            </span>
                        </td>
                        <td id="lblEmp">
                            <label>
                                Employee:</label>
                        </td>
                        <td id="divEmp">
                            <select id="ddlEmployee" class="dtimesize">
                            </select>
                        </td>
                        <td>
                            <input type="button" id="btnclick" value="Click Here" class="SaveButton" onclick="btnclickhere();"
                                style="width: 156px; height: 30px; font-size: 16px;" />
                        </td>
                        <td>
                            <input type="button" id="btn_getlayout" value="Get Layout" onclick="btngetlayoutclick();"
                                style="width: 100px; height: 24px; font-size: 14px;display:none;" />
                        </td>--%>
                    </tr>
                   
                </table>
                <%--<table id="divtblexaxis" style="display:none;">
                 <tr>
                        <td >
                            <input id="txt_xaxis" value="8" />
                        </td>
                        <td> 
                            <input id="txt_yaxis" value="6" />
                        </td>
                        <td >
                            <input id="txt_zaxis" value="10" />
                        </td>
                        <td >
                            <input id="txt_inputcapacity" value="400" />
                        </td>
                    </tr>
                </table>--%>
            </div>
            <div id="divFillScreen">
            </div>
            <div id="divInventory">
            </div>
        </div>
        <div>
            <table id="table_trip_gridview" style="padding-left: 38%">
                <tr>
                    <td>
                        <div id="divtripass" style="padding-left: 5%; width: 90%; cursor: pointer;">
                            <table id="grd_tripassignment">
                            </table>
                            <div id="trippage">
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <a target="_blank" id="BtnPrintDetails" class="SaveButton" style="text-decoration: none;
        width: 156px; height: 30px; font-size: 14px;" onclick="GetPrintDetails();">Get Print
        Details </a>
    <br />
    <div id="LoadPrint" style="display: none;">
        <asp:Button ID="btnPrint" CssClass="SaveButton" Text="Print" OnClientClick="javascript:CallPrint('divPrint');"
            runat="Server" Style="width: 100px; height: 30px; font-size: 14px;" />
    </div>
   <%-- <div style="width: 100%;">
        <div id="vehlayout" style="float: left; width: 60%;">
        </div>
        <div id="vehsublayput" style="float: right; width: 40%;">
        </div>
    </div>
    <div id="areaToPrint" align="center" style="width:21.0cm;height:29.7cm;background-color:White;" hidden="hidden">
    <style type="text/css">
   @page {
   margin: 0;
}
</style><div align="center" style="color: #0252aa; font-weight: bold;font-size:x-large;top:50px;position:relative;">
       Sri Vyshnavi Dairy Specialities (P) Ltd TRIP ASSIGNMENT LAYOUT
    </div>
    <div style="height:120px;width:100%;">
    
    </div>
    <div align="center" style="width:100%;" id="printdata">
    </div>
    </div>--%>
</asp:Content>
