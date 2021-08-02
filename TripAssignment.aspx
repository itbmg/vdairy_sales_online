<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TripAssignment.aspx.cs" Inherits="Plant_TripAssignment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);
        });
    </script>
    <style type="text/css">
        
        input[type=number]::-webkit-inner-spin-button, input[type=number]::-webkit-outer-spin-button
        {
            -webkit-appearance: none;
            margin: 0;
        }
         .btn
        {
            padding:6px;
        }
    </style>
    <script type="text/javascript">
        var values = "";
        var today = "";
        $(function () {
            GetPlanDetails();
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
                        $('#txtVehicle').css('display', 'block');
                        $('#txtVehicleNo').css('display', 'none');
                        $('#BtnPrintDetails').css('display', 'none');
                        $('#LoadPrint').css('display', 'block');
                        $('#btnclick').css('display', 'none');
                        btnclickhere();
                        btnclickhere();
                    }
                    else {
                        $('#LoadPrint').css('display', 'none');
                        $('#BtnPrintDetails').css('display', 'block');
                        $('#txtVehicleNo').css('display', 'block');
                        $('#txtVehicle').css('display', 'none');
                        $('#divEmp').css('display', 'block ');
                        $('#lblEmp').css('display', 'block');
                        ddlFromDispatchChange(DispSno, DispType);
                        $('#btnclick').css('display', 'block');
                    }
                    FillVehicleNo();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function FillVehicleNo() {
            var data = { 'operation': 'GetVehicleNos' };
            var s = function (msg) {
                if (msg) {
                    fillVehiclelist(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillVehiclelist(msg) {
            document.getElementById('txtVehicleNo').options.length = "";
            var txtVehicleNo = document.getElementById('txtVehicleNo');
            var length = txtVehicleNo.options.length;
            for (i = length - 1; i >= 0; i--) {
                txtVehicleNo.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Vehicle";
            opt.value = "";
            txtVehicleNo.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].VehicleNo;
                    opt.value = msg[i].VehicleNo;
                    txtVehicleNo.appendChild(opt);
                }
            }
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
            var EmpID = document.getElementById("ddlEmployee").value;
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
                    GetInventory(msg);
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
        function FillEmployee() {
            var data = { 'operation': 'Get_Employee' };
            var s = function (msg) {
                if (msg) {
                    fillEmployeelist(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillEmployeelist(msg) {
            document.getElementById('ddlEmployee').options.length = "";
            var veh = document.getElementById('ddlEmployee');
            var length = veh.options.length;
            for (i = length - 1; i >= 0; i--) {
                veh.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Employee";
            opt.value = "";
            veh.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].EmployeeName;
                    opt.value = msg[i].Employee_id;
                    veh.appendChild(opt);
                }
            }
        }
        function GetProducts(msg) {
            $('#divFillScreen').removeTemplate();
            $('#divFillScreen').setTemplateURL('TripRoutes5.htm');
            $('#divFillScreen').processTemplate(msg);
            getTripValues();
        }
        function gettubscans() {
            var tottub = 0;
            var totcan = 0;
            var totcDecimal = 0;
            $('.tottubsclass').each(function (i, obj) {
                var tottubsclass = $(this).text();
                if (tottubsclass == "" || tottubsclass == "0") {
                }
                else {
                    var tubqtys = $(this).closest('tr').find('#txttubs').text();
                    tubqtys = parseFloat(tubqtys).toFixed(2);
                    tottub += tubqtys;
                }
            });
            document.getElementById('txt_tubs').innerHTML = tottub;
            $('.totExtraltrs').each(function (i, obj) {
                var totExtraltrs = $(this).text();
                if (totExtraltrs == "" || totExtraltrs == "0") {
                }
                else {
                    var ltrs = $(this).closest('tr').find('#txtExtraltrs').text();
                    ltrs = parseFloat(ltrs).toFixed(2);
                    totextraltr += ltrs;
                }
            });
            document.getElementById('txt_Extraltr').innerHTML = totextraltr;
            $('.totcansclass').each(function (i, obj) {
                var totcansclass = $(this).text();
                if (totcansclass == "" || totcansclass == "0") {
                }
                else {
                    var cansqtys = $(this).closest('tr').find('#txtcans').text();
                    cansqtys = parseFloat(cansqtys).toFixed(2);
                    totcan += cansqtys;
                }
            });
            document.getElementById('txt_cans').innerHTML = totcan;
        }
        function getTripValues() {
            var tot = 0;
            var totc = 0;
            var totcDecimal = 0;
            $('.totqtyclass').each(function (i, obj) {
                var totqtyclass = $(this).val();
                var orderunits = $(this).closest('tr').find('#prdtunits').val();
                var orderqty = $(this).closest('tr').find('#prdtinvqty').val();
                var prdtname = $(this).closest('tr').find('#txtproduct').text();
                if (orderunits == "ltr") {
                    totc = parseFloat(totqtyclass / orderqty);
                    $(this).closest('tr').find('#txtcans').text(totc);
                }
                if (orderunits == "kgs") {
                    totc = parseFloat(totqtyclass / orderqty);
                    totc = parseFloat(totc).toFixed(2);
                    $(this).closest('tr').find('#txtcans').text(totc);
                }
                if (orderunits == "ml") {
                    if (prdtname == "CURD175") {
                        totc = Math.floor(totqtyclass / 10.5);
                        totcDecimal = (totqtyclass % 10.5);
                        totcDecimal = parseFloat(totcDecimal).toFixed(2);
                        totc = parseFloat(totc).toFixed(2);
                        $(this).closest('tr').find('#txttubs').text(totc);
                        $(this).closest('tr').find('#txtExtraltrs').text(totcDecimal);
                    }
                    else {
                        totc = Math.floor(totqtyclass / orderqty);
                        totc = parseFloat(totc).toFixed(2);
                        totcDecimal = (totqtyclass % orderqty);
                        totcDecimal = parseFloat(totcDecimal).toFixed(2);
                        $(this).closest('tr').find('#txttubs').text(totc);
                        $(this).closest('tr').find('#txtExtraltrs').text(totcDecimal);
                    }
                }
                if (orderunits == "gms") {

                }
                if (totqtyclass == "" || totqtyclass == "0") {
                }
                else {
                    var orderqtys = $(this).closest('tr').find('#txtProductQty').val();
                    tot += parseInt(orderqtys);
                }
            });
            document.getElementById('txt_RetunQty').innerHTML = tot;
            var tottub = 0;
            var totcan = 0;
            var totextraltr = 0;
            $('.tottubsclass').each(function (i, obj) {
                var tottubsclass = $(this).text();
                if (tottubsclass == "" || tottubsclass == "0") {
                }
                else {
                    var tubqtys = $(this).closest('tr').find('#txttubs').text();
                    tottub += parseFloat(tubqtys);
                }
            });
            document.getElementById('txt_tubs').innerHTML = tottub;
            $('.totExtraltrs').each(function (i, obj) {
                var totExtraltrs = $(this).text();
                if (totExtraltrs == "" || totExtraltrs == "0") {
                }
                else {
                    var ltrs = $(this).closest('tr').find('#txtExtraltrs').text();
                    ltrs = parseFloat(ltrs).toFixed(2);
                    totextraltr += parseFloat(ltrs);
                }
            });
            document.getElementById('txt_Extraltr').innerHTML = totextraltr;
            $('.totcansclass').each(function (i, obj) {
                var totcansclass = $(this).text();
                if (totcansclass == "" || totcansclass == "0") {
                }
                else {
                    var cansqtys = $(this).closest('tr').find('#txtcans').text();
                    cansqtys = parseFloat(cansqtys).toFixed(2);
                    totcan += parseFloat(cansqtys);
                }
            });
            document.getElementById('txt_cans').innerHTML = totcan;
        }
        function onchangeindentqty() {
            getTripValues();
        }
        function GetInventory(msg) {
            $('#divInventory').removeTemplate();
            $('#divInventory').setTemplateURL('TripInventory.htm');
            $('#divInventory').processTemplate(msg);
        }
        //        divInventory
        function btnPlantTripsaveclick() {
            document.getElementById("BtnSave").disabled = true;
            var datatab = [];
            var Employee = document.getElementById('ddlEmployee').value;
            var Date = today;
            var txtVehicleN = document.getElementById('txtVehicleNo').value;
            if (Employee == "" || Employee == "Select Employee") {
                alert("Please Select Employee");
                return false;
            }
            if (txtVehicleN == "" || txtVehicleN == "Select Vehicle") {
                alert("Please Select Vehicle");
                return false;
            }
            var rows = $("#tabledetails tr:gt(0)");
            var Orderdetails = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtProductQty').val() != "") {
                    var hdn_editqty = $(this).find('#hdn_editqty').val();
                    var txtProductQty = $(this).find('#txtProductQty').val();

                    var tot_qty = 0;
                    tot_qty = parseFloat(hdn_editqty) + parseFloat(txtProductQty);
                    if (tot_qty > 0) {
                        var Product = 0;
                        Orderdetails.push({ ProductSno: $(this).find('#hdnProductSno').val(), Product: Product, Qty: $(this).find('#txtProductQty').val(), RemainingQty: $(this).find('#txtremainingqty').text() });
                    }
                }
            });
            if (Orderdetails != "") {
                var invrows = $("#table_inventory_details tr:gt(0)");
                var inventorydetails = new Array();
                $(invrows).each(function (i, obj) {
                    if ($(this).find('#txtInvQty').val() != "0") {
                        inventorydetails.push({ Sno: $(this).find('#txtsno').val(), InventorySno: $(this).find('#hdnProductSno').val(), Qty: $(this).find('#txtInvQty').val() });
                    }
                });
                if (CheckStatus == "false") {
                    if (inventorydetails.length == 0) {
                        alert("Please Enter Inventory");
                        return false;
                    }
                }
            }
            //table_inventory_details
            Operationvalues = "D";
            var Data = { 'operation': 'btnPlantTripSaveClick', 'data': Orderdetails, 'invdata': inventorydetails, 'EmpID': Employee, 'routename': DispSno, 'Permissions': Operationvalues, 'VehicleNo': txtVehicleN };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");

                    }
                    if (msg == "Data Successfully Saved") {
                        $('#divFillScreen').removeTemplate();
                        $('#divFillScreen').setTemplateURL('TripRoutes5.htm');
                        $('#divFillScreen').processTemplate(datatab);
                        $('#divInventory').removeTemplate();
                        $('#divInventory').setTemplateURL('TripInventory.htm');
                        $('#divInventory').processTemplate(datatab);
                        alert(msg);
                        btnPlantTripRefreshClick();
                    }
                    if (msg == "Data Successfully Updated") {
                        $('#divFillScreen').removeTemplate();
                        $('#divFillScreen').setTemplateURL('TripRoutes5.htm');
                        $('#divFillScreen').processTemplate(datatab);
                        $('#divInventory').removeTemplate();
                        $('#divInventory').setTemplateURL('TripInventory.htm');
                        $('#divInventory').processTemplate(datatab);
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
//        function GetPrintDetails() {
////            window.location = "DeliveryChallanReport.aspx";
//        }
        function btnPlantTripRefreshClick() {
            document.getElementById('ddlEmployee').value = "";
            document.getElementById('txtVehicleNo').value = "";
            Operationvalues = "";
            CheckStatus = "";

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
        function ddlFromDispatchChange(DispSno, DispType) {
            var data = { 'operation': 'GetSalesOfficeEmployee', 'DispSno': DispSno, 'DispType': DispType };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillEmployeelist(msg);
                    }
                    else {
                        FillEmployee();
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
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        function btngetlayoutclick() {
            var x = document.getElementById('txt_xaxis').value;
            var y = document.getElementById('txt_yaxis').value;
            var z = document.getElementById('txt_zaxis').value;
            var totalqty = document.getElementById('txt_inputcapacity').value;
            x = parseInt(x);
            y = parseInt(y);
            z = parseInt(z);
            totalqty = parseInt(totalqty);
            var grndcpcty = x * y;
            var totalcpcty = x * y * z;
            if (totalqty > totalcpcty) {
                alert("Quantity exceeded.");
                return;
            }
            createTable(x, y);
            var filledrowsqty = totalqty / grndcpcty;
            filledrowsqty = parseInt(filledrowsqty);
            var remaining = totalqty % grndcpcty;
            var remaincount = 1;
            for (var i = 0; i < x; i++) {
                for (var j = 0; j < y; j++) {
                    var elemnt = $('.x' + i + 'y' + j);
                    if (remaincount <= remaining) {
                        elemnt.html(filledrowsqty + 1);
                        elemnt.val(filledrowsqty + 1);
                        remaincount++;
                    }
                    else {
                        elemnt.html(filledrowsqty);
                        elemnt.val(filledrowsqty);
                    }
                }
            }
        }

        function cellclick(id) {
            var subtbl = $('#tblsubdiv');
            for (var k = 0, subrow; subrow = subtbl[0].rows[k]; k++) {
                subtbl[0].rows[k].cells[1].childNodes[0].value = "";
            }
            var celltext = $(id).html();
            var cellval = $(id).val();
            cellval = parseInt(cellval);
            var n = celltext.indexOf("-");
            if (n > -1) {
                var checkcontainer = celltext.indexOf("\n");
                if (checkcontainer > -1) {
                    var spltdval = celltext.split("\n");
                    for (var a = 0; a < spltdval.length; a++) {
                        var splittedval = spltdval[a].split("-");
                        if (typeof splittedval[1] === "undefined") {
                        }
                        else {
                            var subtbl = $('#tblsubdiv');
                            for (var k = 0, subrow; subrow = subtbl[0].rows[k]; k++) {
                                var product = subtbl[0].rows[k].cells[0].innerText;
                                var prdct = ' ' + product + ' ';
                                var spltdprdct = splittedval[1];
                                var spltdprdctvalue = splittedval[0];
                                spltdprdctvalue = parseInt(spltdprdctvalue);
                                if (prdct == spltdprdct) {
                                    subtbl[0].rows[k].cells[1].childNodes[0].value = spltdprdctvalue;
                                }
                            }
                        }
                    }

                    var background = $(id).css('background-color');
                    if (background == "rgb(221, 213, 255)") {
                        $(id).css('background-color', 'rgb(255, 255, 204)');
                    }
                    else {
                        var subtbl = $('#tblsubdiv');
                        for (var k = 0, subrow; subrow = subtbl[0].rows[k]; k++) {
                            subtbl[0].rows[k].cells[1].childNodes[0].value = "";
                        }
                        $(id).css('background-color', 'rgb(221, 213, 255)');
                    }
                }
                else {
                    var splittedval = celltext.split("-");
                    if (typeof splittedval[1] === "undefined") {
                        var background = $(id).css('background-color');
                        if (background == "rgb(255, 255, 204)") {
                            $(id).css('background-color', '#FFFFFC');
                        }
                        else {
                            $(id).css('background-color', 'rgb(255, 255, 204)');
                        }
                    }
                    else {
                        var subtbl = $('#tblsubdiv');
                        for (var k = 0, subrow; subrow = subtbl[0].rows[k]; k++) {
                            var prdct = ' ' + subtbl[0].rows[k].cells[0].innerText;
                            var spltdprdct = splittedval[1];
                            var spltdprdctvalue = splittedval[0];
                            spltdprdctvalue = parseInt(spltdprdctvalue);
                            if (prdct == spltdprdct) {
                                subtbl[0].rows[k].cells[1].childNodes[0].value = spltdprdctvalue;
                            }
                        }
                        var background = $(id).css('background-color');
                        if (background == "rgb(255, 255, 204)") {
                            if (cellval == spltdprdctvalue) {
                                $(id).css('background-color', 'rgb(180, 245, 234)');
                            }
                            else {
                                $(id).css('background-color', 'rgb(243, 207, 200)');
                            }
                        }
                        else {
                            $(id).css('background-color', 'rgb(255, 255, 204)');
                        }
                    }
                }
            }
            else {
                var background = $(id).css('background-color');
                if (background == "rgb(255, 255, 204)") {
                    $(id).css('background-color', '#FFFFFC');
                }
                else {
                    $(id).css('background-color', 'rgb(255, 255, 204)');
                }
            }
        }

        function onproductclick(thisid) {
            var slctdproductqty = 0;
            var slctdcellsqty = 0;
            var clickedprdctname = $(thisid).html();
            if (clickedprdctname != "Remove") {
                var clickedprdctcnt = $(thisid).parent('td').next('td').next('td');
                var slctdproductqty = $(clickedprdctcnt).find('span').html();
                var tbllayout = $('#tbllayout');
                slctdproductqty = parseInt(slctdproductqty);
                if (slctdproductqty > 0) {
                    if (typeof tbllayout[0] === "undefined") {
                    }
                    else {
                        for (var i = 0, row; row = tbllayout[0].rows[i]; i++) {
                            for (var j = 0, cell; cell = tbllayout[0].rows[i].cells[j]; j++) {
                                if (tbllayout[0].rows[i].cells[j].style.backgroundColor == "rgb(255, 255, 204)") {
                                    var cellvalue = tbllayout[0].rows[i].cells[j].value;
                                    var celltext = tbllayout[0].rows[i].cells[j].innerHTML;
                                    cellvalue = parseInt(cellvalue);
                                    var checkcontainer = celltext.indexOf("\n");
                                    if (checkcontainer > -1) {
                                        var spltdval = celltext.split("\n");
                                        for (var a = 0; a < spltdval.length; a++) {
                                            var splittedval = spltdval[a].split("-");
                                            if (typeof splittedval[1] === "undefined") {
                                            }
                                            else {
                                                var subtbl = $('#tblsubdiv');
                                                for (var k = 0, subrow; subrow = subtbl[0].rows[k]; k++) {
                                                    var product = subtbl[0].rows[k].cells[0].innerText;
                                                    var productqty = subtbl[0].rows[k].cells[2].innerText;
                                                    productqty = parseInt(productqty);
                                                    var prdct = ' ' + product + ' ';
                                                    var spltdprdct = splittedval[1];
                                                    var spltdprdctvalue = splittedval[0];
                                                    spltdprdctvalue = parseInt(spltdprdctvalue);
                                                    if (prdct == spltdprdct) {
                                                        var addprdctvls = productqty + spltdprdctvalue;
                                                        subtbl[0].rows[k].cells[2].childNodes[0].innerHTML = addprdctvls;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else {
                                        var n = celltext.indexOf("-");
                                        if (n > -1) {
                                            var splittedval = celltext.split("-");
                                            if (typeof splittedval[1] === "undefined") {
                                            }
                                            else {
                                                var subtbl = $('#tblsubdiv');
                                                for (var k = 0, subrow; subrow = subtbl[0].rows[k]; k++) {
                                                    var prdct = ' ' + subtbl[0].rows[k].cells[0].innerText;
                                                    var productqty = subtbl[0].rows[k].cells[2].innerText;
                                                    productqty = parseInt(productqty);
                                                    var spltdprdct = splittedval[1];
                                                    var spltdprdctvalue = splittedval[0];
                                                    spltdprdctvalue = parseInt(spltdprdctvalue);
                                                    if (prdct == spltdprdct) {
                                                        var addprdctvls = productqty + spltdprdctvalue;
                                                        subtbl[0].rows[k].cells[2].childNodes[0].innerHTML = addprdctvls;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    slctdcellsqty += cellvalue;
                                    if (slctdproductqty >= slctdcellsqty) {
                                        tbllayout[0].rows[i].cells[j].innerHTML = cellvalue + " - " + clickedprdctname;
                                        tbllayout[0].rows[i].cells[j].style.backgroundColor = "rgb(180, 245, 234)";
                                    }
                                    else {
                                        var tempremain = slctdproductqty + cellvalue - slctdcellsqty;
                                        if (tempremain > 0) {
                                            tbllayout[0].rows[i].cells[j].innerHTML = tempremain + " - " + clickedprdctname;
                                            tbllayout[0].rows[i].cells[j].style.backgroundColor = "rgb(243, 207, 200)";
                                        }
                                    }
                                }
                            }
                        }
                        var remain = 0;
                        if (slctdproductqty >= slctdcellsqty) {
                            remain = slctdproductqty - slctdcellsqty;
                        }
                        $(clickedprdctcnt).find('span').html(remain);
                    }
                }
            }
            else {
                var tbllayout = $('#tbllayout');
                for (var i = 0, row; row = tbllayout[0].rows[i]; i++) {
                    for (var j = 0, cell; cell = tbllayout[0].rows[i].cells[j]; j++) {
                        if (tbllayout[0].rows[i].cells[j].style.backgroundColor == "rgb(255, 255, 204)") {
                            var cellvalue = tbllayout[0].rows[i].cells[j].value;
                            var celltext = tbllayout[0].rows[i].cells[j].innerHTML;
                            cellvalue = parseInt(cellvalue);
                            var checkcontainer = celltext.indexOf("\n");
                            if (checkcontainer > -1) {
                                var spltdval = celltext.split("\n");
                                for (var m = 0; m < spltdval.length; m++) {
                                    var splittedval = spltdval[m].split("-");
                                    if (typeof splittedval[1] === "undefined") {
                                    }
                                    else {
                                        var subtbl = $('#tblsubdiv');
                                        for (var k = 0, subrow; subrow = subtbl[0].rows[k]; k++) {
                                            var product = subtbl[0].rows[k].cells[0].innerText;
                                            var productqty = subtbl[0].rows[k].cells[2].innerText;
                                            productqty = parseInt(productqty);
                                            var prdct = ' ' + product + ' ';
                                            var spltdprdct = splittedval[1];
                                            var spltdprdctvalue = splittedval[0];
                                            spltdprdctvalue = parseInt(spltdprdctvalue);
                                            if (prdct == spltdprdct) {
                                                var addprdctvls = productqty + spltdprdctvalue;
                                                subtbl[0].rows[k].cells[2].childNodes[0].innerHTML = addprdctvls;
                                                tbllayout[0].rows[i].cells[j].innerHTML = tbllayout[0].rows[i].cells[j].value;
                                                tbllayout[0].rows[i].cells[j].style.backgroundColor = "#FFFFFC";
                                            }
                                        }
                                    }
                                }
                            }
                            else {
                                var n = celltext.indexOf("-");
                                if (n > -1) {
                                    var splittedval = celltext.split("-");
                                    if (typeof splittedval[1] === "undefined") {
                                    }
                                    else {
                                        var subtbl = $('#tblsubdiv');
                                        for (var k = 0, subrow; subrow = subtbl[0].rows[k]; k++) {
                                            var prdct = ' ' + subtbl[0].rows[k].cells[0].innerText;
                                            var productqty = subtbl[0].rows[k].cells[2].innerText;
                                            productqty = parseInt(productqty);
                                            var spltdprdct = splittedval[1];
                                            var spltdprdctvalue = splittedval[0];
                                            spltdprdctvalue = parseInt(spltdprdctvalue);
                                            if (prdct == spltdprdct) {
                                                var addprdctvls = productqty + spltdprdctvalue;
                                                subtbl[0].rows[k].cells[2].childNodes[0].innerHTML = addprdctvls;
                                                tbllayout[0].rows[i].cells[j].innerHTML = tbllayout[0].rows[i].cells[j].value;
                                                tbllayout[0].rows[i].cells[j].style.backgroundColor = "#FFFFFC";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            var subtbl = $('#tblsubdiv');
            for (var k = 0, subrow; subrow = subtbl[0].rows[k]; k++) {
                subtbl[0].rows[k].cells[1].childNodes[0].value = "";
            }
        }

        function layoutproducts() {
            var subdiv = $('#vehsublayput');
            var productstbl = $('#tabledetails');
            if (typeof productstbl[0] === "undefined") {
            }
            else {
                var subdivhtml = '<table id="tblsubdiv">';
                for (var i = 1, row; row = productstbl[0].rows[i]; i++) {
                    var product = $(row).find('#txtproduct').html();
                    var productqty = $(row).find('#txttubs').html();
                    productqty = parseInt(productqty);
                    if (productqty > 0) {
                        var pdctval = product.substring(3, product.length);
                        product = pdctval.substring(0, pdctval.length - 4);
                        subdivhtml += '<tr><td><span style="cursor:pointer;" onclick="onproductclick(this)">' + product + '</span></td><td><input onkeyup="runScript(this)"/></td><td><span>' + productqty + '</span></td></tr>';
                    }
                }
                subdivhtml += '<tr><td><span style="cursor:pointer;font-weight:bold;" onclick="onproductclick(this)">Remove</span></td><td><input style="display:none;"/></td><td><span style="display:none;">0</span></td></tr>';
                subdivhtml += '</table>';
                subdivhtml += '<input type="button" value="Split" style="width:100px;height:30px;font-weight:bold;font-size:15px;background-color: #2a73a6;color:white; border-radius:3px 3px 3px 3px;" onclick="btnsplitclick()"/>';
                subdivhtml += '<input type="button" value="Done" style="width:100px;height:30px;font-weight:bold;font-size:15px;background-color: #2a73a6;color:white; border-radius:3px 3px 3px 3px;" onclick="btnsaveclick()"/>';
                subdivhtml += '<input type="button" value="Print" style="width:100px;height:30px;font-weight:bold;font-size:15px;background-color: #2a73a6;color:white; border-radius:3px 3px 3px 3px;" onclick="btnprintclick()"/>';
                subdiv.html(subdivhtml);
            }
        }

        function btnsaveclick() {
            var tbllayout = $('#tbllayout');
            if (typeof tbllayout[0] === "undefined") {
            }
            else {
                var finalstring = new Array();
                for (var i = 0, row; row = tbllayout[0].rows[i]; i++) {
                    for (var j = 0, cell; cell = tbllayout[0].rows[i].cells[j]; j++) {
                        var cellclass = tbllayout[0].rows[i].cells[j].className;
                        var cellvalue = tbllayout[0].rows[i].cells[j].value;
                        var celltext = tbllayout[0].rows[i].cells[j].innerHTML;
                        cellvalue = parseInt(cellvalue);
                        var checkcontainer = celltext.indexOf("-");
                        if (checkcontainer > -1) {
                            finalstring.push({ cellcss: cellclass, cellvalue: celltext });
                        }
                    }
                }
                var data = { 'operation': 'vehlayoutsave', 'tripid': '1', 'layoutstring': finalstring };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                        }
                        else {
                            alert("Error occurred,please try again.");
                        }
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                CallHandlerUsingJson(data, s, e);
            }
        }

        function btnprintclick() {

            //var tble = document.getElementById("vehlayout").innerHTML;
            var tble = document.getElementById("tbllayout");
            document.getElementById("printdata").innerHTML = tble.outerHTML;
            //div.innerHTML = tble;


            var divToPrint = document.getElementById('areaToPrint');
            var newWin = window.open('about:blank');
            //        setTimeout(function () {
            //            if (!newWin || newWin.outerHeight === 0) {
            //                //First Checking Condition Works For IE & Firefox
            //                //Second Checking Condition Works For Chrome
            //                alert("Popup Blocker is enabled! Please add this site to your exception list.");

            //            } else {

            newWin.document.write(divToPrint.innerHTML);
            newWin.print();
            newWin.close();

            //            }
            //        }, 1000);
        }




        function runScript(id) {
            var txtval = $(id).val();
            var clickedprdctcnt = $(id).parent('td').next('td');
            var lblval = $(clickedprdctcnt).find('span').html();
            txtval = parseInt(txtval);
            lblval = parseInt(lblval);
            if (id.value < 0) id.value = 0;
            if (id.value > lblval) {
                //                alert("Entered input exceeds product quantity.");
                var sss = id.value.length - 1;
                id.value = id.value.substring(0, sss);
            }
        }

        function createTable(num_rows, num_cols) {
            // var num_rows = document.getElementById('rows').value;
            // var num_cols = document.getElementById('cols').value;
            var headtbl = '<table id="headtbl">\n';
            headtbl += '<tr>';
            for (var j = 0; j < num_cols; j++) {
                headtbl += '<td style="text-align:center;width:62px;"><input type="checkbox" onclick="checkclick(this)" id=h0' + j + ' />';
                headtbl += '</td>'
            }
            headtbl += '</tr>\n</table>';

            var theader = '<table id="tbllayout" border="1">\n';
            var tbody = "";
            for (var i = 0; i < num_rows; i++) {
                tbody += '<tr>';
                for (var j = 0; j < num_cols; j++) {
                    tbody += '<td style="background-color: #FFFFFC; width:60px;height:60px;text-align:center;color:Green; font-size:11px;" onclick="cellclick(this)" class=x' + i + 'y' + j + '>';
                    tbody += '</td>'
                }
                tbody += '</tr>\n';
            }
            var tfooter = '</table>';
            document.getElementById('vehlayout').innerHTML = headtbl + theader + tbody + tfooter;
        }

        function checkclick(thisid) {
            var chkbox = $(thisid);
            var checkedid = chkbox[0].id;
            checkedid = checkedid.substring(2, 3);
            checkedid = parseInt(checkedid);
            var tbllayout = $('#tbllayout');
            if (typeof tbllayout[0] === "undefined") {
            }
            else {
                for (var i = 0, row; row = tbllayout[0].rows[i]; i++) {
                    for (var j = 0, cell; cell = tbllayout[0].rows[i].cells[j]; j++) {
                        if (checkedid == j) {
                            if (thisid.checked) {
                                if (tbllayout[0].rows[i].cells[j].style.backgroundColor == "rgb(255, 255, 252)" || tbllayout[0].rows[i].cells[j].style.backgroundColor == "#FFFFFC") {
                                    tbllayout[0].rows[i].cells[j].style.backgroundColor = "rgb(255, 255, 204)";
                                }
                            }
                            else {
                                if (tbllayout[0].rows[i].cells[j].style.backgroundColor == "rgb(255, 255, 204)") {
                                    tbllayout[0].rows[i].cells[j].style.backgroundColor = "#FFFFFC";
                                }
                            }
                        }
                    }
                }
            }
        }

        function btnsplitclick() {
            var tbllayout = $('#tbllayout');
            var selectedcellscnt = 0;
            if (typeof tbllayout[0] === "undefined") {
            }
            else {
                for (var i = 0, row; row = tbllayout[0].rows[i]; i++) {
                    for (var j = 0, cell; cell = tbllayout[0].rows[i].cells[j]; j++) {
                        if (tbllayout[0].rows[i].cells[j].style.backgroundColor == "rgb(255, 255, 204)") {
                            selectedcellscnt++;
                        }
                    }
                }
                if (selectedcellscnt > 1) {
                    alert("Please select only one cell to split.");
                    return;
                }
                else {
                    var prdctscnt = 0;
                    var prdctstrng = "";
                    for (var i = 0, row; row = tbllayout[0].rows[i]; i++) {
                        for (var j = 0, cell; cell = tbllayout[0].rows[i].cells[j]; j++) {
                            if (tbllayout[0].rows[i].cells[j].style.backgroundColor == "rgb(255, 255, 204)") {
                                var cellvalue = tbllayout[0].rows[i].cells[j].value;
                                cellvalue = parseInt(cellvalue);
                                var subtbl = $('#tblsubdiv');
                                for (var k = 0, subrow; subrow = subtbl[0].rows[k]; k++) {
                                    var product = subtbl[0].rows[k].cells[0].innerText;
                                    var productqty = subtbl[0].rows[k].cells[1].childNodes[0].value;
                                    var totproductqty = subtbl[0].rows[k].cells[2].innerText;
                                    productqty = parseInt(productqty);
                                    if (productqty > 0) {
                                        prdctscnt += productqty;
                                        prdctstrng += productqty + ' - ' + product + ' \n';
                                    }
                                }
                                if (prdctscnt <= cellvalue) {
                                    var celltext = tbllayout[0].rows[i].cells[j].innerHTML;
                                    cellvalue = parseInt(cellvalue);
                                    var checkcontainer = celltext.indexOf("\n");
                                    if (checkcontainer > -1) {
                                        var spltdval = celltext.split("\n");
                                        for (var m = 0; m < spltdval.length; m++) {
                                            var splittedval = spltdval[m].split("-");
                                            if (typeof splittedval[1] === "undefined") {
                                            }
                                            else {
                                                var subtbl = $('#tblsubdiv');
                                                for (var k = 0, subrow; subrow = subtbl[0].rows[k]; k++) {
                                                    var product = subtbl[0].rows[k].cells[0].innerText;
                                                    var productqty = subtbl[0].rows[k].cells[2].innerText;
                                                    productqty = parseInt(productqty);
                                                    var prdct = ' ' + product + ' ';
                                                    var spltdprdct = splittedval[1];
                                                    var spltdprdctvalue = splittedval[0];
                                                    spltdprdctvalue = parseInt(spltdprdctvalue);
                                                    if (prdct == spltdprdct) {
                                                        var addprdctvls = productqty + spltdprdctvalue;
                                                        subtbl[0].rows[k].cells[2].childNodes[0].innerHTML = addprdctvls;
                                                        tbllayout[0].rows[i].cells[j].innerHTML = tbllayout[0].rows[i].cells[j].value;
                                                        tbllayout[0].rows[i].cells[j].style.backgroundColor = "#FFFFFC";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else {
                                        var n = celltext.indexOf("-");
                                        if (n > -1) {
                                            var splittedval = celltext.split("-");
                                            if (typeof splittedval[1] === "undefined") {
                                            }
                                            else {
                                                var subtbl = $('#tblsubdiv');
                                                for (var k = 0, subrow; subrow = subtbl[0].rows[k]; k++) {
                                                    var prdct = ' ' + subtbl[0].rows[k].cells[0].innerText;
                                                    var productqty = subtbl[0].rows[k].cells[2].innerText;
                                                    productqty = parseInt(productqty);
                                                    var spltdprdct = splittedval[1];
                                                    var spltdprdctvalue = splittedval[0];
                                                    spltdprdctvalue = parseInt(spltdprdctvalue);
                                                    if (prdct == spltdprdct) {
                                                        var addprdctvls = productqty + spltdprdctvalue;
                                                        subtbl[0].rows[k].cells[2].childNodes[0].innerHTML = addprdctvls;
                                                        tbllayout[0].rows[i].cells[j].innerHTML = tbllayout[0].rows[i].cells[j].value;
                                                        tbllayout[0].rows[i].cells[j].style.backgroundColor = "#FFFFFC";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    for (var k = 0, subrow; subrow = subtbl[0].rows[k]; k++) {
                                        var product = subtbl[0].rows[k].cells[0].innerText;
                                        var productqty = subtbl[0].rows[k].cells[1].childNodes[0].value;
                                        var totproductqty = subtbl[0].rows[k].cells[2].innerText;
                                        productqty = parseInt(productqty);
                                        if (productqty > 0) {
                                            var remain = 0;
                                            remain = totproductqty - productqty;
                                            subtbl[0].rows[k].cells[2].childNodes[0].innerHTML = remain;
                                            subtbl[0].rows[k].cells[1].childNodes[0].value = "";
                                        }
                                    }
                                    tbllayout[0].rows[i].cells[j].innerHTML = prdctstrng;
                                    tbllayout[0].rows[i].cells[j].style.backgroundColor = "rgba(221, 213, 255, 1)";
                                }
                                else {
                                    alert("Selected quantity exceeds the limit of the cell.\nPlease give proper input.");
                                }
                            }
                        }
                    }
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Trip Assignment<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Trip Assignment</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Trip Assignment Details
                </h3>
            </div>
            <div class="box-body">
                <div style="background-color: #FFFFFF;">
                    <div id="divPrint">
                        <div>
                            <table>
                                <tr>
                                    <td id="divTime" style="display: none;">
                                        Date <span id="txtTime" style="color: Blue; font-size: 18px; font-weight: bold;">
                                        </span>
                                    </td>
                                    <td>
                                       <label>    Route Name</label> <span id="txtDispName" style="color: Red; font-size: 18px; font-weight: bold;">
                                        </span>
                                    </td>
                                    <td style="width:6px;"></td>
                                    <td>
                                        <label>
                                            Vehicle No</label>
                                    </td>
                                    <td>
                                        <select id="txtVehicleNo" class="form-control">
                                        </select>
                                        <span id="txtVehicle" style="color: Red; font-size: 18px; font-weight: bold; display: none;">
                                        </span>
                                    </td>
                                    <td style="width:6px;"></td>
                                    <td id="lblEmp">
                                    </td>
                                    <td id="divEmp">
                                        <select id="ddlEmployee" class="form-control">
                                        </select>
                                    </td>
                                    <td style="width:6px;"></td>
                                    <td>
                                        <input type="button" id="btnclick" value="Click Here" class="btn btn-primary" onclick="btnclickhere();"/>
                                    </td>
                                    <td>
                                        <input type="button" id="btn_getlayout" value="Get Layout" onclick="btngetlayoutclick();"
                                            style="width: 100px; height: 24px; font-size: 14px; display: none;" />
                                    </td>
                                </tr>
                            </table>
                            <table id="divtblexaxis" style="display: none;">
                                <tr>
                                    <td>
                                        <input id="txt_xaxis" value="8" />
                                    </td>
                                    <td>
                                        <input id="txt_yaxis" value="6" />
                                    </td>
                                    <td>
                                        <input id="txt_zaxis" value="10" />
                                    </td>
                                    <td>
                                        <input id="txt_inputcapacity" value="400" />
                                    </td>
                                </tr>
                            </table>
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
                <%--<button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="GetPrintDetails();"><i class="fa fa-print"></i> Print</button>--%>
              <%--  <a target="_blank" id="BtnPrintDetails" class="btn btn-primary" style="text-decoration: none;
                    width: 156px; height: 30px; font-size: 14px;" onclick="GetPrintDetails();">Get Print
                    Details </a>--%>
                <br />
                <div id="LoadPrint" style="display: none;">
                    <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print" OnClientClick="javascript:CallPrint('divPrint');"
                        runat="Server"  />
                </div>
                <div style="width: 100%;">
                    <div id="vehlayout" style="float: left; width: 60%;">
                    </div>
                    <div id="vehsublayput" style="float: right; width: 40%;">
                    </div>
                </div>
                <div id="areaToPrint" align="center" style="width: 21.0cm; height: 29.7cm; background-color: White;"
                    hidden="hidden">
                    <style type="text/css">
                        @page
                        {
                            margin: 0;
                        }
                    </style>
                    <div align="center" style="color: #0252aa; font-weight: bold; font-size: x-large;
                        top: 50px; position: relative;">
                        Veer Dairy TRIP ASSIGNMENT LAYOUT
                    </div>
                    <div style="height: 120px; width: 100%;">
                    </div>
                    <div align="center" style="width: 100%;" id="printdata">
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
