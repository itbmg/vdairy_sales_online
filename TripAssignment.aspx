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
        input[type=number]::-webkit-inner-spin-button, input[type=number]::-webkit-outer-spin-button {
            -webkit-appearance: none;
            margin: 0;
        }

        .btn {
            padding: 6px;
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
                    GetBranchInventory();
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
                    }
                    else {
                        $('#divTime').css('display', 'none');
                        $('#DivSave').css('display', 'block');
                        $('#LoadPrint').css('display', 'none');
                        $('#btn_getlayout').css('display', 'none');
                        $('#divtblexaxis').css('display', 'none');
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
            $('#divFillScreen').setTemplateURL('TripRoutes6.htm');
            $('#divFillScreen').processTemplate(msg);
            
            getTripValuesCalculation();
        }
        function getTripValuesCalculation() {
            var tot_ltr = 0;
            $('.totalQtyclass').each(function (i, obj) {
                var qtyltr = $(this).closest('tr').find('#txtProductQty').val();
                if (qtyltr == "" || qtyltr == "0") {
                }
                else {
                    tot_ltr += parseFloat(qtyltr);
                }
            });
            document.getElementById('txt_QtyLtr').innerHTML = parseFloat(tot_ltr).toFixed(2);
        }
        function GetInventory(msg) {
            $('#divInventory').removeTemplate();
            $('#divInventory').setTemplateURL('TripInventory.htm');
            $('#divInventory').processTemplate(msg);
            GetInventoryCalculation();
        }
        function GetInventoryCalculation() {
            var SumInvQty = 0;
            $('.totinvclass').each(function (i, obj) {
                SumInvQty = 0;
                var hdnProductSno = $(this).closest('tr').find('#hdnProductSno').val();
                $('.Unitqtyclass').each(function (i, obj) {
                    var tubqty = $(this).closest('tr').find('#txtTubQty').val();
                    var invsno = $(this).closest('tr').find('#hdnInvSno').val();
                    if (tubqty == "" || tubqty == "0") {
                    }
                    else {
                        if (hdnProductSno == invsno) {
                            SumInvQty += parseFloat(tubqty);
                        }

                    }
                });
                $(this).closest('tr').find('#txtInvQty').val(parseFloat(SumInvQty).toFixed(0));
            });
        }

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
                        Orderdetails.push({ ProductSno: $(this).find('#hdnProductSno').val(), Product: Product, Qty: $(this).find('#txtProductQty').val(), tub_qty: $(this).find('#txtTubQty').val(), pkt_qty: $(this).find('#txtQtypkts').val() });
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
                        $('#divFillScreen').setTemplateURL('TripRoutes6.htm');
                        $('#divFillScreen').processTemplate(datatab);
                        $('#divInventory').removeTemplate();
                        $('#divInventory').setTemplateURL('TripInventory.htm');
                        $('#divInventory').processTemplate(datatab);
                        alert(msg);
                        btnPlantTripRefreshClick();
                    }
                    if (msg == "Data Successfully Updated") {
                        $('#divFillScreen').removeTemplate();
                        $('#divFillScreen').setTemplateURL('TripRoutes6.htm');
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

        // Calculation for all the Pkts,Tubs,Ltr/kgs
        function OrderPktQtyChange(PktQty) {
            if (PktQty.value == "") {

            }
            else {
                var invQty = $(PktQty).closest("tr").find("#hdninvQty").val();
                var unitQty = $(PktQty).closest("tr").find("#hdnUnitQty").val();
                var pktval = PktQty.value;
                var totltr = parseFloat(pktval * unitQty);
                var totltrvalue = parseFloat(totltr / 1000);
                var totaltub = parseFloat(pktval / invQty);
                $(PktQty).closest("tr").find("#txtDupUnitQty").text(parseFloat(totltrvalue).toFixed(2))
                $(PktQty).closest("tr").find("#txtProductQty").val(parseFloat(totltrvalue).toFixed(2))
                // $(PktQty).closest("tr").find("#txtLtrQty").val(parseFloat(totltrvalue).toFixed(2))
                //  $(PktQty).closest("tr").find("#hdnltrQty").val(parseFloat(totltrvalue).toFixed(2))
                $(PktQty).closest("tr").find("#txtTubQty").val(parseFloat(totaltub).toFixed(2));
                var tot_ltr = 0;
                $('.totalQtyclass').each(function (i, obj) {
                    var qtyltr = $(this).closest('tr').find('#txtProductQty').val();
                    if (qtyltr == "" || qtyltr == "0") {
                    }
                    else {
                        tot_ltr += parseFloat(qtyltr);
                    }
                });
                document.getElementById('txt_QtyLtr').innerHTML = parseFloat(tot_ltr).toFixed(2);
                var val = parseFloat(totltrvalue).toFixed(2);
                OrderUnitChange(PktQty);
                GetInventoryCalculation();
            }
        }
        function OrderTubQtyChange(TubQty) {
            if (TubQty.value == "") {
            }
            else {
                var invQty = $(TubQty).closest("tr").find("#hdninvQty").val();
                var unitQty = $(TubQty).closest("tr").find("#hdnUnitQty").val();
                var tubval = TubQty.value;
                var totalpkts = parseFloat(tubval * invQty);
                var totltr = parseFloat(totalpkts * unitQty);
                var totltrvalue = parseFloat(totltr / 1000);
                // $(TubQty).closest("tr").find("#txtQtypkts").val(parseFloat(totltrvalue).toFixed(2));
                $(TubQty).closest("tr").find("#txtDupUnitQty").text(parseFloat(totltrvalue).toFixed(2))
                $(TubQty).closest("tr").find("#txtProductQty").val(parseFloat(totltrvalue).toFixed(2))
                //$(TubQty).closest("tr").find("#txtLtrQty").val(parseFloat(totltrvalue).toFixed(2))
                //$(TubQty).closest("tr").find("#hdnltrQty").val(parseFloat(totltrvalue).toFixed(2))
                $(TubQty).closest("tr").find("#txtQtypkts").val(parseFloat(totalpkts).toFixed(2));
                var tot_ltr = 0;
                $('.totalQtyclass').each(function (i, obj) {
                    var qtyltr = $(this).closest('tr').find('#txtProductQty').val();
                    if (qtyltr == "" || qtyltr == "0") {
                    }
                    else {
                        tot_ltr += parseFloat(qtyltr);
                    }
                });
                document.getElementById('txt_QtyLtr').innerHTML = parseFloat(tot_ltr).toFixed(2);
                var val = parseFloat(totltrvalue).toFixed(2);
                OrderUnitChange(TubQty);
                GetInventoryCalculation();
            }
        }
        function OrderUnitChange(UnitQty) {
            var totalqty;
            var qty = 0.0;
            var Rate = 0;
            var rate = 0;
            var total = 0;
            var totalltr = 0;
            var TotalRate = 0;
            var cnt = 0;
            if (UnitQty.value == "") {
                //$(UnitQty).closest("tr").find("#txtOrderTotal").text(parseFloat(total).toFixed(2));
                $('.Unitqtyclass').each(function (i, obj) {
                    var qtyclass = $(this).closest('tr').find('#txtQtypkts').val();
                    if (qtyclass == "" || qtyclass == "0") {
                    }
                    else {
                        totalltr += parseFloat(qtyclass);

                        cnt++;
                    }
                });
                //  document.getElementById('txt_totqty').innerHTML = parseFloat(totalltr).toFixed(2);
                //$('.rateclass').each(function (i, obj) {
                //    rate += parseFloat($(this).text());
                //});
                //var Floatrate = rate.toFixed(2)
                // document.getElementById('txt_totRate').innerHTML = parseFloat(Floatrate).toFixed(2);
                $('.totalQtyclass').each(function (i, obj) {
                    total += parseFloat($(this).text());
                });
                document.getElementById('txt_QtyLtr').innerHTML = parseFloat(total).toFixed(2);
                return false;
            }
            var Qty = $(UnitQty).closest("tr").find("#hdnUnitQty").val();
            var Units = $(UnitQty).closest("tr").find("#hdnUnits").val();
            var Units = $(UnitQty).closest("tr").find("#hdnUnits").val();
            var unitqty = $(UnitQty).closest("tr").find("#txtQtypkts").val();
            if (Units == "ml") {
                totalqty = parseFloat(unitqty);
            }
            if (Units == "ltr") {
                totalqty = parseInt(unitqty);
            }
            if (Units == "gms") {
                totalqty = parseFloat(unitqty);
            }
            if (Units == "kgs") {
                totalqty = parseInt(unitqty);
            }
            if (Units == "Pkts") {
                totalqty = parseInt(unitqty);
            }
            $(UnitQty).closest("tr").find("#hdnQty").val(totalqty)
            //cnt = 0;
            $('.Unitqtyclass').each(function (i, obj) {
                var qtyclass = $(this).closest('tr').find('#txtQtypkts').val();
                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totalltr += parseInt(qtyclass);
                    cnt++;
                }
            });
            //document.getElementById('txt_totqty').innerHTML = parseFloat(totalltr).toFixed(2);
            //total = 0;
            //$('.totalclass').each(function (i, obj) {
            //    total += parseFloat($(this).text());
            //});
            //document.getElementById('txt_total').innerHTML = parseFloat(total).toFixed(2);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>Trip Assignment<small>Preview</small>
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
                                    <td id="divTime" style="display: none;">Date <span id="txtTime" style="color: Blue; font-size: 18px; font-weight: bold;"></span>
                                    </td>
                                    <td>
                                        <label>Route Name</label>
                                        <span id="txtDispName" style="color: Red; font-size: 18px; font-weight: bold;"></span>
                                    </td>
                                    <td style="width: 6px;"></td>
                                    <td>
                                        <label>
                                            Vehicle No</label>
                                    </td>
                                    <td>
                                        <select id="txtVehicleNo" class="form-control">
                                        </select>
                                        <span id="txtVehicle" style="color: Red; font-size: 18px; font-weight: bold; display: none;"></span>
                                    </td>
                                    <td style="width: 6px;"></td>
                                    <td id="lblEmp"></td>
                                    <td id="divEmp">
                                        <select id="ddlEmployee" class="form-control">
                                        </select>
                                    </td>
                                    <td style="width: 6px;"></td>
                                    <td>
                                        <input type="button" id="btnclick" value="Click Here" class="btn btn-primary" onclick="btnclickhere();" />
                                    </td>

                                </tr>
                            </table>

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

                    <br />
                    <div id="LoadPrint" style="display: none;">
                        <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print" OnClientClick="javascript:CallPrint('divPrint');"
                            runat="Server" />
                    </div>

                    <div id="areaToPrint" align="center" style="width: 21.0cm; height: 29.7cm; background-color: White;"
                        hidden="hidden">
                        <style type="text/css">
                            @page {
                                margin: 0;
                            }
                        </style>
                        <div align="center" style="color: #0252aa; font-weight: bold; font-size: x-large; top: 50px; position: relative;">
                            SRI VYSHNAVI DAIRY SPECIALITIES (P) LTD TRIP ASSIGNMENT LAYOUT
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
