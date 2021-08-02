<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="IventoryManagement.aspx.cs" Inherits="IventoryManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script src="jquery-ui-1.10.3.custom/js/jquery-ui.js" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);

        });

        $(function () {
            updateinventory();
            $("#tabs").tabs();
            $("#regiontabs").tabs().addClass("ui-tabs-vertical ui-helper-clearfix");
            $("#regiontabs  li").removeClass("ui-corner-top").addClass("ui-corner-left");

        });
        function inventory_validation() {
            var x = document.getElementById("txt_inventory_name").value;
            if (x == "") {
                alert("Please Provide InventoryName");
                $("#txt_inventory_name").focus();
                return false;
            }
            var y = document.getElementById("cmb_flag").value;
            if (y == "") {
                alert("Please Select Flag");
                $("#cmb_flag").focus();
                return false;
            }
            else {
                Inventorymanage();
            }
        }
        function inventory_purchase_validation() {

            var x = document.getElementById("cmb_invname").value;
            if (x == "") {
                alert("Please Select InventoryName");
                $("#cmb_invname").focus();
                return false;
            }
            var purchasedt = document.getElementById("txt_purchase_date").value;
            if (purchasedt == "") {
                alert("Please Select Date");
                $("#txt_purchase_date").focus();
                return false;
            }
            var purchaseqty = document.getElementById("txt_qty").value;
            if (purchaseqty == "") {
                alert("Please Provide Qty");
                $("#txt_qty").focus();
                return false;
            }
            var purchasecost = document.getElementById("txt_cost").value;
            if (purchasecost == "") {
                alert("Please Provide Cost");
                $("#txt_cost").focus();
                return false;
            }
            var y = document.getElementById("cmb_flag").value;
            if (y == "") {
                alert("Please Select Flag");
                $("#cmb_flag").focus();
                return false;
            }
            else {
                Inventory_Purchase_manage();
            }
        }
        function cost_onchange() {
            var qty = document.getElementById("txt_qty").value;
            var cost = document.getElementById("txt_cost").value;
            var total = qty * cost;
            document.getElementById("txt_total").value = total;
        }
        function Inventorymanage() {
            var inventoryname = document.getElementById('txt_inventory_name').value;
            var invflag = document.getElementById('cmb_flag').value;
            var invqty = document.getElementById('txt_inv_unitsqty').value;
            var operationtype = document.getElementById('btn_save').innerHTML;
            var sno = serial;
            if (invqty == "") {
                alert("Please Provide Qty");
                $("#cmb_flag").focus();
                return false;
            }

            var data = { 'operation': 'Inventorymanage', 'sno': sno, 'inventoryname': inventoryname, 'invflag': invflag, 'operationtype': operationtype, 'invqty': invqty };

            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    inventory_clear();
                    updateinventory();
                }
                else {
                }
            };
            var e = function (x, h, e) {
                if (x.status && x.status == 400) {
                    alert(x.responseText);
                    window.location.assign("Login.aspx");
                }
                else {
                    alert("something went wrong");
                }
            };
            callHandler(data, s, e);
        }
        function inventory_clear() {
            document.getElementById('txt_inventory_name').value = "";
            document.getElementById('cmb_flag').value = "";
            document.getElementById('txt_inv_unitsqty').value = "";
            document.getElementById('btn_save').innerHTML = "SAVE";
        }
        function updateinventory() {

            var data = { 'operation': 'update_inventory_manage' };
            var s = function (msg) {
                if (msg) {
                    BindGrid_inventory(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        var serial = 0;
        function BindGrid_inventory(msg) {
            var l = 0;

            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">Inventory Name</th><th scope="col" class="thcls">Status</th><th scope="col" class="thcls">Qty</th><th scope="col"></th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                var status = 'InActive';
                if (msg[i].flag == '1') {
                    status = 'Active';
                }
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row"  class="1 tdmaincls" >' + msg[i].inventoryname + '</td>';
                results += '<td scope="row" class="2" >' + status + '</td>';
                results += '<td scope="row" class="3" >' + msg[i].Qty + '</td>';
                results += '<td style="display:none" class="4">' + msg[i].sno + '</td>';
                results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';

                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_invdata").html(results);
        }

        function getme(thisid) {
            var inventoryname = $(thisid).parent().parent().children('.1').html();
            var status = $(thisid).parent().parent().children('.2').html();
            var Qty = $(thisid).parent().parent().children('.3').html();
            var sno = $(thisid).parent().parent().children('.4').html();

            document.getElementById('txt_inventory_name').value = inventoryname;
            document.getElementById('txt_inv_unitsqty').value = Qty;
            document.getElementById('cmb_flag').value = status;
            document.getElementById('btn_save').innerHTML = "MODIFY";
            serial = sno;
        }



        function Inventory_Purchase_manage() {
            var inventoryname = document.getElementById('cmb_invname').value;
            var purchasedate = document.getElementById('txt_purchase_date').value;
            var purchaseqty = document.getElementById('txt_qty').value;
            var purchasecost = document.getElementById('txt_cost').value;
            var purchasetotal = document.getElementById('txt_total').value;
            //var invflag = document.getElementById('cmb_flag').value;
            var operationtype = document.getElementById('purchase_save').value;
            var sno = serial;


            var data = { 'operation': 'Inventory_Purchase_manage', 'sno': sno, 'inventoryname': inventoryname, 'purchasedate': purchasedate, 'purchaseqty': purchaseqty, 'purchasecost': purchasecost, 'purchasetotal': purchasetotal, 'operationtype': operationtype };

            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    inventory_purchase_clear();
                    updateinventory_Purchase_manage();
                }
                else {
                }
            };
            var e = function (x, h, e) {
                if (x.status && x.status == 400) {
                    alert(x.responseText);
                    window.location.assign("Login.aspx");
                }
                else {
                    alert("something went wrong");
                }
            };
            callHandler(data, s, e);
        }
        function inventory_purchase_clear() {
            document.getElementById('cmb_invname').value = "";
            document.getElementById('txt_purchase_date').value = "";
            document.getElementById('txt_qty').value = "";
            document.getElementById('txt_cost').value = "";
            document.getElementById('txt_total').value = "";
            document.getElementById('purchase_save').value = "SAVE";
        }
        function updateinventory_Purchase_manage() {
            var data = { 'operation': 'update_inventory_Purchase_manage' };
            var s = function (msg) {
                if (msg) {
                    BindGrid_Purchase_inventory(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        var serial1 = 0;
        function BindGrid_Purchase_inventory(msg) {

            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">Inventory Name</th><th scope="col" class="thcls">Purchase Date</th><th scope="col" class="thcls">Qty</th><th scope="col" class="thcls">Cost</th><th scope="col" class="thcls">Total</th><th scope="col" class="thcls">Status</th><th scope="col"></th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                var status = 'InActive';
                if (msg[i].flag == '1') {
                    status = 'Active';
                }
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row"   class="1 tdmaincls" >' + msg[i].inventoryname + '</td>';
                results += '<td scope="row" class="1" >' + msg[i].purchasedate + '</td>';
                results += '<td scope="row" class="1" >' + msg[i].qty + '</td>';
                results += '<td scope="row" class="1" >' + msg[i].cost + '</td>';
                results += '<td scope="row" class="1" >' + msg[i].total + '</td>';
                results += '<td scope="row" class="1" >' + status + '</td>';
                results += '<td style="display:none" class="2">' + msg[i].sno + '</td>';
                results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';

                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_purchasedata").html(results);
            //           
        }
        function cmb_invname_onchange() {
            updateinventory_Purchase_manage();
            var data = { 'operation': 'intialize_purchase_invname' };
            var s = function (msg) {
                if (msg) {
                    fillpurchase_inventoryname(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        };

        function fillpurchase_inventoryname(msg) {
            var cmbinvname = document.getElementById('cmb_invname');
            var length = cmbinvname.options.length;
            cmbinvname.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            cmbinvname.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].InvName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].InvName;
                    opt.value = msg[i].sno;
                    cmbinvname.appendChild(opt);
                }
            }
        }
        function cmb_scrap_invname_onchange() {

            lvltype = '<%=Session["LevelType"]%>';
            if (lvltype == "Plant") {
                $('#cmb_scrap_level_type option[value="1"]').attr("selected", true);
                document.getElementById('cmb_scrap_salesoffice').disabled = true;
                document.getElementById('cmb_scrap_distributor').disabled = true;
                document.getElementById('cmb_scrap_branch').disabled = true;
                //document.getElementById("trscrap_plant").disabled = true;
                $('#trscrap_plant').css('display', 'none');
            }
            if (lvltype == "Sales Office") {
                $('#cmb_scrap_level_type option[value="1"]').attr("disabled", true);
                $('#cmb_scrap_level_type option[value="2"]').attr("selected", true);
                //document.getElementById('cmb_scrap_salesoffice').disabled = true;
                document.getElementById('cmb_scrap_distributor').disabled = true;
                document.getElementById('cmb_scrap_branch').disabled = true;
                document.getElementById("trscrap_plant").disabled = true;
                $('#trscrap_so').css('display', 'none');
                $('#trscrap_plant').css('display', 'none');
            }
            if (lvltype == "Distributors") {
                $('#cmb_scrap_level_type option[value="1"]').attr("disabled", true);
                $('#cmb_scrap_level_type option[value="2"]').attr("disabled", true);
                $('#cmb_scrap_level_type option[value="3"]').attr("selected", true);
                // document.getElementById('cmb_scrap_salesoffice').disabled = true;
                //document.getElementById('cmb_scrap_distributor').disabled = true;
                document.getElementById('cmb_scrap_branch').disabled = true;
                document.getElementById("trscrap_plant").disabled = true;
                $('#trscrap_so').css('display', 'none');
                $('#trscrap_dist').css('display', 'none');
                $('#trscrap_plant').css('display', 'none');
            }
            var data = { 'operation': 'intialize_purchase_invname' };
            var s = function (msg) {
                if (msg) {
                    fillScrap_inventoryname(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        };
        function fillScrap_inventoryname(msg) {
            var cmbinvname = document.getElementById('cmb_scrap_invname');
            var length = cmbinvname.options.length;
            cmbinvname.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            cmbinvname.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].InvName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].InvName;
                    opt.value = msg[i].sno;
                    cmbinvname.appendChild(opt);
                }
            }
        }
        function scrapleveltype_onchange(level) {
            var cmb_scrap_level = level.value;
            if (cmb_scrap_level = "undefined") {
                cmb_scrap_level = document.getElementById("cmb_scrap_level_type").value;
            }
            if (cmb_scrap_level == '0') {
                document.getElementById('cmb_scrap_plantname').value = "";
                document.getElementById('cmb_scrap_salesoffice').value = "";
                document.getElementById('cmb_scrap_distributor').value = "";
                document.getElementById('cmb_scrap_branch').value = "";
                document.getElementById('cmb_scrap_plantname').disabled = true;
                document.getElementById('cmb_scrap_salesoffice').disabled = true;
                document.getElementById('cmb_scrap_distributor').disabled = true;
                document.getElementById('cmb_scrap_branch').disabled = true;
            }
            if (cmb_scrap_level == '1') {
                document.getElementById('cmb_scrap_plantname').value = "";
                document.getElementById('cmb_scrap_salesoffice').value = "";
                document.getElementById('cmb_scrap_distributor').value = "";
                document.getElementById('cmb_scrap_branch').value = "";
                document.getElementById('cmb_scrap_plantname').disabled = false;
                document.getElementById('cmb_scrap_salesoffice').disabled = true;
                document.getElementById('cmb_scrap_distributor').disabled = true;
                document.getElementById('cmb_scrap_branch').disabled = true;
            }
            if (cmb_scrap_level == '2') {
                document.getElementById('cmb_scrap_plantname').value = "";
                document.getElementById('cmb_scrap_salesoffice').value = "";
                document.getElementById('cmb_scrap_distributor').value = "";
                document.getElementById('cmb_scrap_branch').value = "";
                document.getElementById('cmb_scrap_plantname').disabled = false;
                document.getElementById('cmb_scrap_salesoffice').disabled = false;
                document.getElementById('cmb_scrap_distributor').disabled = true;
                document.getElementById('cmb_scrap_branch').disabled = true;
            }
            if (cmb_scrap_level == '3') {
                document.getElementById('cmb_scrap_plantname').value = "";
                document.getElementById('cmb_scrap_salesoffice').value = "";
                document.getElementById('cmb_scrap_distributor').value = "";
                document.getElementById('cmb_scrap_branch').value = "";
                document.getElementById('cmb_scrap_plantname').disabled = false;
                document.getElementById('cmb_scrap_salesoffice').disabled = false;
                document.getElementById('cmb_scrap_distributor').disabled = false;
                document.getElementById('cmb_scrap_branch').disabled = true;
            }
            if (cmb_scrap_level == '4') {
                document.getElementById('cmb_scrap_plantname').value = "";
                document.getElementById('cmb_scrap_salesoffice').value = "";
                document.getElementById('cmb_scrap_distributor').value = "";
                document.getElementById('cmb_scrap_branch').value = "";
                document.getElementById('cmb_scrap_plantname').disabled = false;
                document.getElementById('cmb_scrap_salesoffice').disabled = false;
                document.getElementById('cmb_scrap_distributor').disabled = false;
                document.getElementById('cmb_scrap_branch').disabled = false;
            }
        }
        function received_inventory() {
            var data = { 'operation': 'intialize_received_invname' };
            var s = function (msg) {
                if (msg) {
                    //fillScrap_inventoryname(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
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
        input[type=number]::-webkit-inner-spin-button, input[type=number]::-webkit-outer-spin-button
        {
            -webkit-appearance: none;
            margin: 0;
        }
        
        body
        {
            min-width: 100px;
        }
        .ui-widget
        {
            -webkit-font-smoothing: antialiased;
            -moz-osx-font-smoothing: grayscale;
            font-family: 'Source Sans Pro' , 'Helvetica Neue' ,Helvetica,Arial,sans-serif;
            font-weight: 400;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Inventory Management<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Masters</a></li>
            <li><a href="#">Inventory Management</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Inventory Details
                </h3>
            </div>
            <div class="box-body">
                <input id="sessionInput" type="hidden" value='<%= Session["LevelType"] %>' />
                <div id="tabs" style="width: 100%">
                    <ul style="font-size: 12px">
                        <li><a href="#InvMaster" onclick="return updateinventory();">InventoryMaster</a></li>
                        <%--<li><a href="#InvPurchase" onclick="return cmb_invname_onchange();">Purchases</a></li>--%>
                        <%--<li><a href="#Inv_scrap" onclick="return cmb_scrap_invname_onchange();">Scraps</a></li>
                        <li><a href="#Inv_Recieved" onclick="return received_inventory();">Recieved</a></li>--%>
                    </ul>
                    <div id="InvMaster">
                        <table align="center">
                            <tr>
                                <td>
                                    <label id="lblinventoryname">
                                        Inventory Name:</label><span style="color: red; font-weight: bold">*</span>
                                </td>
                                <td style="height: 40px;">
                                    <input type="text" id="txt_inventory_name"    class="form-control" placeholder="Enter Inventory Name" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label id="lblinvqty">
                                        Qty:</label>
                                </td>
                                <td style="height: 40px;">
                                    <input type="text" id="txt_inv_unitsqty" class="form-control" placeholder="Enter Qty" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="lblFlag">
                                        Flag:</label>
                                </td>
                                <td style="height: 40px;">
                                    <select id="cmb_flag" class="form-control">
                                        <option>Active</option>
                                        <option>InActive</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <div class="input-group">
                                                    <div class="input-group-addon">
                                                        <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="inventory_validation()">
                                                        </span><span id="btn_save" onclick="inventory_validation()">SAVE</span>
                                                    </div>
                                                </div>
                                            </td>
                                            <td style="padding-left: 7px;">
                                                <div class="input-group">
                                                    <div class="input-group-close">
                                                        <span class="glyphicon glyphicon-remove" id='btn_clear1' onclick="inventory_clear()">
                                                        </span><span id='btn_clear' onclick="inventory_clear()">CLEAR</span>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="div_invdata">
                        </div>
                    </div>
                    <div id="InvPurchase" style="display:none;">
                        <table align="center">
                            <tr>
                                <td>
                                    <label id="lbl_invname">
                                        Inventory Name:</label>
                                    <select id="cmb_invname" class="form-control">
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label id="lbl_dop">
                                        Purchase Date:</label>
                                    <input type="date" id="txt_purchase_date" class="form-control" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label id="lbl_qty">
                                        Qty:</label>
                                    <input type="number" id="txt_qty" class="form-control" placeholder="Enter Qty" onkeyup="cost_onchange()" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label id="lbl_cost">
                                        Cost:</label>
                                    <input type="number" id="txt_cost" class="form-control" placeholder="Enter Cost"
                                        onkeyup="cost_onchange()" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label id="lbl_total">
                                        Total:</label>
                                    <input type="number" id="txt_total" class="form-control" placeholder="Enter Total" />
                                </td>
                            </tr>
                            <tr>
                                
                                <td>
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <div class="input-group">
                                                    <div class="input-group-addon">
                                                        <span class="glyphicon glyphicon-ok" id="purchase_save1" onclick="inventory_purchase_validation()">
                                                        </span><span id="purchase_save" onclick="inventory_purchase_validation()">SAVE</span>
                                                    </div>
                                                </div>
                                            </td>
                                            <td style="padding-left: 7px;">
                                                <div class="input-group">
                                                    <div class="input-group-close">
                                                        <span class="glyphicon glyphicon-remove" id='purchase_clear' onclick="inventory_purchase_clear()">
                                                        </span><span id='inventory_purchase_clear' onclick="inventory_clear()">CLEAR</span>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br/>
                        <div id="div_purchasedata">
                        </div>
                    </div>
                 <%--   <div id="Inv_scrap">
                        <table align="center">
                            <tr>
                                <td>
                                    <label id="lbl_scrap_invname">
                                        Inventory Name:</label>
                                    <select id="cmb_scrap_invname" class="form-control">
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label id="lbl_scrapdate">
                                        Scrap Date:</label>
                                    <input type="date" id="txt_scrap_date" class="form-control" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label id="lblscraptype">
                                        Scrap Type:</label>
                                    <select id="cmb_scraptype" class="form-control">
                                        <option>select</option>
                                        <option>LOST</option>
                                        <option>DAMAGE</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label id="lbl_scrap_qty">
                                        QTY:</label>
                                    <input type="number" id="txt_scrap_qty" class="form-control" placeholder="Enter Qty" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label id="lblleveltype">
                                        Level Type:</label>
                                    <select id="cmb_scrap_level_type" class="form-control" onchange="scrapleveltype_onchange(this);">
                                        <option value="0">select</option>
                                        <option value="1" id="optscrapplant">Plant</option>
                                        <option value="2">Sales Office</option>
                                        <option value="3">Distributors</option>
                                        <option value="4">Branches</option>
                                    </select>
                                </td>
                            </tr>
                            <tr id="trscrap_plant">
                                <td>
                                    <label id="lblscrap_plant">
                                        Plant Name:</label>
                                    <select id="cmb_scrap_plantname" class="form-control">
                                    </select>
                                </td>
                            </tr>
                            <tr id="trscrap_so">
                                <td>
                                    <label id="lblscrap_so">
                                        Sales Office:</label>
                                    <select id="cmb_scrap_salesoffice" class="form-control">
                                    </select>
                                </td>
                            </tr>
                            <tr id="trscrap_dist">
                                <td>
                                    <label id="lblscrap_distributor">
                                        Distributor:</label>
                                    <select id="cmb_scrap_distributor" class="form-control">
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label id="lblscrap_branch">
                                        Branch Name:</label>
                                    <select id="cmb_scrap_branch" class="form-control">
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <div class="input-group">
                                                    <div class="input-group-addon">
                                                        <span class="glyphicon glyphicon-ok" id="btn_scrap_save1"></span><span id="btn_scrap_save">
                                                            SAVE</span>
                                                    </div>
                                                </div>
                                            </td>
                                            <td style="padding-left: 7px;">
                                                <div class="input-group">
                                                    <div class="input-group-close">
                                                        <span class="glyphicon glyphicon-remove" id='btn_scrap_clear1'></span><span id='btn_scrap_clear'>
                                                            CLEAR</span>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="Inv_Recieved">
                        <table>
                        </table>
                    </div>--%>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
