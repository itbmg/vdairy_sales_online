<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EditDC.aspx.cs" Inherits="EditDC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <style type="text/css">
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
        $(function () {
            FillVehicleNo();
           
        });
        function btnEmployeeDetails() {
            var Dcno = document.getElementById("txtDcno").value;
            if (Dcno == "") {
                alert("Enter Dc No");
                return false;
            }
            var data = { 'operation': 'Get_Employee_editDC', 'Dcno': Dcno };
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
    </script>
  
    <script type="text/javascript">
        function btnGetDcDetails() {
            var Dcno = document.getElementById("txtDcno").value;
            if (Dcno == "") {
                alert("Enter Dc No");
                return false;
            }
            GetBranchProducts();
            GetBranchInventory();
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

        function CallHandlerUsingJson_POST(d, s, e) {
            d = JSON.stringify(d);
            d = encodeURIComponent(d);
            $.ajax({
                type: "POST",
                url: "DairyFleet.axd",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: d,
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }

        function GetBranchProducts() {
            var Dcno = document.getElementById("txtDcno").value;
            var data = { 'operation': 'GetEditDcProducts', 'DcNo': Dcno };
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
            callHandler(data, s, e);
        }
        function GetProducts(msg) {
            var vehicleno = msg[0].vehcleno;
            var status = msg[0].status;
            if (vehicleno == "") {

            } else {
                $("#txtVehicleNo").find("option:contains('" + vehicleno + "')").each(function () {
                    if ($(this).text() == vehicleno) {
                        $(this).attr("selected", "selected");
                    }
                });
            }
            if (status == "") {

            }
            else {
                if (status == "A") {
                    status = "Assigned";
                }
                if (status == "C") {
                    status = "Canceled";
                }
                if (status == "P") {
                    status = "Pending";
                }
                if (status == "V") {
                    status = "Verified";
                }

                $("#cmbdcstatus").find("option:contains('" + status + "')").each(function () {
                    if ($(this).text() == status) {
                        $(this).attr("selected", "selected");
                    }
                });
            }

            $('#divFillScreen').removeTemplate();
            $('#divFillScreen').setTemplateURL('TripRoutes6.htm');
            $('#divFillScreen').processTemplate(msg);
        }
        function GetBranchInventory() {
            var Dcno = document.getElementById("txtDcno").value;
            var data = { 'operation': 'EditDcInventory', 'DcNo': Dcno };
            var s = function (msg) {
                if (msg) {
                    GetInventory(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function onchangeindentqty() {
        }
        function btnEditDCSaveclick() {
            var Dcno = document.getElementById("txtDcno").value;
            if (Dcno == "") {
                alert("Enter Dc No");
                return false;
            }
            var vehicleno = document.getElementById("txtVehicleNo").value;
            var status = document.getElementById("cmbdcstatus").value;
            var Employee = document.getElementById('ddlEmployee').value;
            if (Employee == "" || Employee == "Select Employee") {
                alert("Select Employee Name");
                return false;
            }
            var rows = $("#tabledetails tr:gt(0)");
            var Orderdetails = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtProductQty').val() != "" || $(this).find('#txtProductQty').val() != "0") {
                    var hdn_editqty = $(this).find('#hdn_editqty').val();
                    var txtProductQty = $(this).find('#txtProductQty').val();

                    var tot_qty = 0;
                    tot_qty = parseFloat(hdn_editqty) + parseFloat(txtProductQty);
                    if (tot_qty > 0) {
                        var Product = 0;
                        Orderdetails.push({ ProductSno: $(this).find('#hdnProductSno').val(), Product: Product, Qty: $(this).find('#txtProductQty').val(),  tub_qty: $(this).find('#txtTubQty').val(), pkt_qty: $(this).find('#txtQtypkts').val() });
                    }
                }
            });
            if (Orderdetails != "") {
                var invrows = $("#table_inventory_details tr:gt(0)");
                var inventorydetails = new Array();
                $(invrows).each(function (i, obj) {
                    var txtInvQty = $(this).find('#txtInvQty').val();
                    var hdn_inv_editqty = $(this).find('#hdn_inv_editqty').val();
                    var tot_qty = 0;
                    tot_qty = parseInt(txtInvQty) + parseInt(hdn_inv_editqty);
                    if (tot_qty > 0) {
                        inventorydetails.push({ Sno: $(this).find('#txtsno').val(), InventorySno: $(this).find('#hdnProductSno').val(), Qty: $(this).find('#txtInvQty').val() });
                    }
                });
            }
            //table_inventory_details
            Operationvalues = "D";
            var Data = { 'op': 'btnEditDCSaveclick', 'invdata': inventorydetails, 'tripid': Dcno, 'VehicleNo': vehicleno, 'EmpID': Employee, 'status': status, 'data': Orderdetails };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    if (msg == "Data Successfully Updated") {
                        $('#divFillScreen').removeTemplate();
                        $('#divFillScreen').setTemplateURL('TripRoutes6.htm');
                        $('#divFillScreen').processTemplate();
                        $('#divInventory').removeTemplate();
                        $('#divInventory').setTemplateURL('EditDcInventory1.htm');
                        $('#divInventory').processTemplate();
                        alert(msg);
                    }
                }
            };
            var e = function (x, h, e) {
            };
            CallHandlerUsingJson_POST(Data, s, e);
        }

        function GetInventory(msg) {
            $('#divInventory').removeTemplate();
            $('#divInventory').setTemplateURL('EditDcInventory1.htm');
            $('#divInventory').processTemplate(msg);
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
                $(TubQty).closest("tr").find("#txtDupUnitQty").text(parseFloat(totltrvalue).toFixed(2))
                $(TubQty).closest("tr").find("#txtProductQty").val(parseFloat(totltrvalue).toFixed(2))
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
            $('.Unitqtyclass').each(function (i, obj) {
                var qtyclass = $(this).closest('tr').find('#txtQtypkts').val();
                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totalltr += parseInt(qtyclass);
                    cnt++;
                }
            });
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Edit DC<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Edit DC</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Edit DC Details
                </h3>
            </div>
            <div class="box-body">
                <table align="center">
                    <tr>
                        <td>
                            <label for="lblSalesType">
                                DC No:</label>
                        </td>
                        <td style="height:40px;">
                            <input type="text" id="txtDcno" class="form-control" placeholder="Enter Dc No" />
                        </td>
                       <td>
                           <input type="button" id="btndcclick" value="Get Employees" class="btn btn-primary" onclick="btnEmployeeDetails();"/>

                       </td>
                      
                    </tr>
                    <tr>
                        <td>
                            <label for="lblSalesType">
                                Vehicle No:</label>
                        </td>
                        <td style="height:40px;">
                            <select id="txtVehicleNo" class="form-control">
                            </select>
                        </td>
                         </tr>
                    <tr>
                        <td>
                            <label for="lblSalesType">
                                Employee Name:</label>
                        </td>
                        <td style="height:40px;">
                            <select id="ddlEmployee" class="form-control">
                            </select>
                        </td>
                         </tr>
                    <tr>
                        <td>
                            <label for="lbldcstatus">
                                DC Status:</label>
                        </td>
                        <td style="height:40px;">
                            <select id="cmbdcstatus" class="form-control">
                                <option value="0">Select</option>
                                <option value="1">Assigned</option>
                                <option value="2">Canceled</option>
                                <option value="3">Pending</option>
                                <option value="4">Verified</option>
                            </select>
                        </td>
                     </tr>
                    <tr>
                    <td></td>
                      <td style="height:40px;">
                            <input type="button" id="btnclick" value="Get Dc Details" class="btn btn-primary" onclick="btnGetDcDetails();"/>
                        </td>
                    </tr>
                </table>
                <div id="divFillScreen">
                </div>
                <div id="divInventory">
                </div>
            </div>
        </div>
    </section>
</asp:Content>
