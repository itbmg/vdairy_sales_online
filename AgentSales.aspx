﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="AgentSales.aspx.cs" Inherits="AgentSales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="Js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <link href="Css/style.css" rel="stylesheet" type="text/css" />
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3004" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <style type="text/css">
        .ddlsize
        {
            width: 280px;
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
        .chkclass
        {
            height: 20px;
            width: 20px;
        }
         .btn
        {
            padding:6px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            FillSalesOffices();
            FillVehicleNo();
            $("#datepicker").datepicker({ dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
                onSelect: function (selectedDate) {
                }
            });
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#datepicker').val(today);
        });
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
            opt.innerHTML = "Select Vehicle No";
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
        function FillSalesOffices() {
            var data = { 'operation': 'GetSalesOffice' };
            var s = function (msg) {
                if (msg) {
                    BindSalesOfficeNames(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindSalesOfficeNames(msg) {
            document.getElementById('ddlSalesOffice').options.length = "";
            var ddlSalesOffice = document.getElementById('ddlSalesOffice');
            var length = ddlSalesOffice.options.length;
            for (i = length - 1; i >= 0; i--) {
                ddlSalesOffice.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Sales Office";
            opt.value = "";
            ddlSalesOffice.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlSalesOffice.appendChild(opt);
                }
            }
        }
        function ddlddlSalesOfficeChange(Id) {
            var soid = document.getElementById('ddlSalesOffice').value;
            if (soid == "4609") {
                $('#trddltype').css("display", "none");
                var type = document.getElementById('ddltype').value;
            }
            else {
                $('#trddltype').css("display", "none");
            }
            var data = { 'operation': 'GetSalesOfficeChange', 'BranchID': Id.value  };
            var s = function (msg) {
                if (msg) {
                    BindSoRouteName(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindSoRouteName(msg) {
            document.getElementById('ddlRouteName').options.length = "";
            var ddlRouteName = document.getElementById('ddlRouteName');
            var length = ddlRouteName.options.length;
            for (i = length - 1; i >= 0; i--) {
                ddlRouteName.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Route Name";
            opt.value = "";
            ddlRouteName.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].routename;
                    opt.value = msg[i].routesno;
                    ddlRouteName.appendChild(opt);
                }
            }
        }
        function ddlRouteNameChange(Id) {
            var data = { 'operation': 'GetRouteNameChange', 'RouteID': Id.value };
            var s = function (msg) {
                if (msg) {
                    BindBranchName(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindBranchName(msg) {
            document.getElementById('ddlBranchName').options.length = "";
            var ddlBranchName = document.getElementById('ddlBranchName');
            var length = ddlBranchName.options.length;
            for (i = length - 1; i >= 0; i--) {
                ddlBranchName.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Agent Name";
            opt.value = "";
            ddlBranchName.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null || msg[i].BranchName != "" || msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].b_id;
                    ddlBranchName.appendChild(opt);
                }
            }
        }
        function GetAgentSalesClick() {
            var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
            if (ddlSalesOffice == "Select Sales Office" || ddlSalesOffice == "") {
                alert("Please Select Sales Office");
                return false;
            }
            var ddlRouteName = document.getElementById('ddlRouteName').value;
            if (ddlRouteName == "Select Route Name" || ddlRouteName == "") {
                alert("Please Select Route Name");
                return false;
            }
            var ddlBranchName = document.getElementById('ddlBranchName').value;
            if (ddlBranchName == "Select Agent Name" || ddlBranchName == "") {
                alert("Please Select Agent Name");
                return false;
            }
            var IndentType = document.getElementById('txtIndentType').value;
            if (IndentType == "indent1" || IndentType == "Indent1") {
                alert("Please Enter Another Type");
                return false;
            }

            GetBranchProducts(ddlSalesOffice);
            GetBranchInventory(ddlSalesOffice);
        }
        function GetBranchProducts(ddlBranchName) {
            var data = { 'operation': 'GetAgetntsaleProducts', 'BranchID': ddlBranchName };
            var s = function (msg) {
                if (msg) {
                    GetProducts(msg);
                    GetBranchInventory(ddlBranchName);
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
            $('#divFillScreen').setTemplateURL('AgentProducts.htm');
            $('#divFillScreen').processTemplate(msg);
            getTripValues();
        }
        function GetBranchInventory(ddlBranchNames) {
            var data = { 'operation': 'GetAgetntsaleInventory', 'BranchID': ddlBranchNames };
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
        function GetInventory(msg) {
            $('#divInventory').removeTemplate();
            $('#divInventory').setTemplateURL('AgentInventory.htm');
            $('#divInventory').processTemplate(msg);
        }
        function btnAgentSaleSaveclick() {
            var SalesOfficeID = document.getElementById('ddlSalesOffice').value;
            var BranchID = document.getElementById('ddlBranchName').value;
            if (BranchID == "Select Agent Name" || BranchID == "") {
                alert("Please Select Agent Name");
                return false;
            }
            var IndentDate = document.getElementById('datepicker').value;
            if (IndentDate == "") {
                alert("Please Select Indent Date");
                return false;
            }
            var txtVehicleNo = document.getElementById('txtVehicleNo').value;
            if (txtVehicleNo == "" || txtVehicleNo == "Select Vehicle No") {
                alert("Please Select Vehicle No");
                return false;
            }

            var type = document.getElementById('ddltype').value;
            

            var BranchName = document.getElementById('ddlBranchName');
            var BranchSno = BranchName.options[BranchName.selectedIndex].value;
            var BName = BranchName.options[BranchName.selectedIndex].text;
            var IndentType = document.getElementById('txtIndentType').value;
            if (IndentType == "") {
                alert("Enter Indent Type");
                return false;
            }
            var rows = $("#tabledetails tr:gt(0)");
            var Orderdetails = new Array();
            var Offerdetails = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtProductQty').val() != "") {
                    var Product = 0;
                    var qty = $(this).find('#txtProductQty').val();
                    qty = parseFloat(qty).toFixed(2);
                    if (qty > 0) {
                        Orderdetails.push({ ProductSno: $(this).find('#hdnProductSno').val(), Product: Product, Qty: $(this).find('#txtProductQty').val(), offerqty: $(this).find('#txtOfferProductqty').val(), RemainingQty: $(this).find('#txtremainingqty').text() });
                    }
                }
            });
            if (Orderdetails != "") {
                var invrows = $("#table_inventory_details tr:gt(0)");
                var inventorydetails = new Array();
                $(invrows).each(function (i, obj) {
                    if ($(this).find('#txtInvQty').val() != "0") {
                        var invqty = $(this).find('#txtInvQty').val();
                        invqty = parseFloat(invqty).toFixed(2);
                        if (invqty > 0) {
                            inventorydetails.push({ Sno: $(this).find('#txtsno').val(), InventorySno: $(this).find('#hdnProductSno').val(), Qty: $(this).find('#txtInvQty').val() });
                        }
                    }
                });
            }
            var Data = { 'operation': 'btnAgentSaleSaveclick', 'data': Orderdetails, 'Offerdetails': Offerdetails, 'invdata': inventorydetails, 'BranchID': BranchID, 'IndentType': IndentType, 'routename': BName, 'indentdate': IndentDate, 'SalesOfficeID': SalesOfficeID, 'VehicleNo': txtVehicleNo, 'type': type };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    $('#divFillScreen').removeTemplate();
                    $('#divFillScreen').setTemplateURL('TripRoutes5.htm');
                    $('#divFillScreen').processTemplate();
                    $('#divInventory').removeTemplate();
                    $('#divInventory').setTemplateURL('TripInventory.htm');
                    $('#divInventory').processTemplate();
                    alert(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(Data, s, e);
        }
        function numberOnlyExample() {
            if ((event.keyCode < 48) || (event.keyCode > 57))
                return false;
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
                    totcan += parseFloat(cansqtys);
                }
            });
            document.getElementById('txt_cans').innerHTML = totcan;
        }
        function getTripValues() {
            var tot = 0;
            var otot = 0;
            var totc = 0;
            var totcDecimal = 0;
            $('.totqtyclass').each(function (i, obj) {
                var totqtyclass = $(this).val();
                var orderunits = $(this).closest('tr').find('#prdtunits').val();
                var orderqty = $(this).closest('tr').find('#prdtinvqty').val();
                if (orderunits == "ltr") {
                    totc = parseFloat(totqtyclass / orderqty);
                    $(this).closest('tr').find('#txtcans').text(totc);
                }
                if (orderunits == "kgs") {
                    totc = parseFloat(totqtyclass / orderqty);
                    $(this).closest('tr').find('#txtcans').text(totc);
                }
                if (orderunits == "ml") {

                    totc = Math.floor(totqtyclass / orderqty);
                    totcDecimal = (totqtyclass % orderqty);
                    $(this).closest('tr').find('#txttubs').text(totc);
                    $(this).closest('tr').find('#txtExtraltrs').text(totcDecimal);
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
            $('.totofferqtyclass').each(function (i, obj) {
                var totofferqtyclass = $(this).val();
                var orderunits = $(this).closest('tr').find('#prdtunits').val();
                var orderqty = $(this).closest('tr').find('#prdtinvqty').val();
                if (totofferqtyclass == "" || totofferqtyclass == "0") {
                }
                else {
                    var orderofferqtys = $(this).closest('tr').find('#txtOfferProductqty').val();
                    otot += parseInt(orderofferqtys);
                }
            });
            document.getElementById('txt_OfferQty').innerHTML = otot;
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
                    totcan += parseFloat(cansqtys);
                }
            });
            document.getElementById('txt_cans').innerHTML = totcan;
        }
        function ddlBranchChange(Id) {
            var IndentDate = document.getElementById('datepicker').value;
            var data = { 'operation': 'GetAllIndentTypes', 'BranchID': Id.value, 'IndentDate': IndentDate };
            var s = function (msg) {
                if (msg) {
                    BindIndentType(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function BindIndentType(msg) {
            document.getElementById('divchblroutes').innerHTML = "";
            for (var i = 0; i <= msg.length; i++) {
                if (typeof msg[i] === "undefined" || msg[i].IndentType == "" || msg[i].IndentType == null) {
                }
                else {
                    var label = document.createElement("span");
                    var hidden = document.createElement("input");
                    hidden.type = "hidden";
                    hidden.name = "hidden";
                    hidden.value = msg[i].IndentType;
                    var checkbox = document.createElement("input");
                    checkbox.type = "checkbox";
                    checkbox.name = "checkbox";
                    checkbox.value = msg[i].IndentType;
                    checkbox.id = "checkbox";
                    checkbox.onclick = function () { checked(this); };
                    //checkbox.className = 'checkinput';
                    checkbox.className = 'chkclass';
                    document.getElementById('divchblroutes').appendChild(checkbox);
                    label.className = "Indentcls"; 
                    label.innerHTML = msg[i].IndentType;
                    document.getElementById('divchblroutes').appendChild(label);
                    document.getElementById('divchblroutes').appendChild(hidden);
                    document.getElementById('divchblroutes').appendChild(document.createElement("br"));
                }
            }
        }
        function checked(thisid) {
            var refdcno = $(thisid).next('.Indentcls').html();
            document.getElementById('txtIndentType').value = refdcno;

        }
//        function GetPrintDetails() {
//            window.location = "DeliveryChallanReport.aspx";
//        }
        function onchangeindentqty() {
            getTripValues();
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Agent Sale<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Agent Sale</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Agent Sale Details
                </h3>
            </div>
            <div class="box-body">
                <div style="width: 100%;">
                    <div style="width: 100%;">
                        <div style="width: 100%; height: 175px;">
                            <div style="width: 50%; float: left;">
                                <table align="center">
                                    <tr>
                                        <td>
                                            <label>Sales Office Name</label>
                                        </td>
                                        <td style="height: 40px;">
                                            <select id="ddlSalesOffice" class="form-control" onchange="ddlddlSalesOfficeChange(this);">
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Route Name</label>
                                        </td>
                                        <td style="height: 40px;">
                                            <select id="ddlRouteName" class="form-control" onchange="ddlRouteNameChange(this);">
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                         <label>   Agent Name</label>
                                        </td>
                                        <td style="height: 40px;">
                                            <select id="ddlBranchName" class="form-control" onchange="ddlBranchChange(this);">
                                            </select>
                                        </td>
                                    </tr>
                                    <tr id="trddltype" style="display:none;">
                                        <td>
                                                <label>Invoice BookType</label>
                                            </td>
                                            <td style="height: 40px;">
                                            <select id="ddltype" class="form-control">
                                                <option value="01">Book1</option>
                                               <option value="02">Book2</option>
                                                 <option value="03">Book3</option>
                                             </select>
                                            </td>
                                    </tr>
                                    <tr>
                                        <td>
                                          <label>  Indent Type</label>
                                        </td>
                                        <td style="height: 40px;">
                                            <input type="text" id="txtIndentType" class="form-control" placeholder="Enter Indent Type" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>
                                                Date</label>
                                        </td>
                                        <td style="height: 40px;">
                                        <input type="date" id="datepicker" placeholder="DD-MM-YYYY" class="form-control" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                           <label> Vehicle No</label>
                                        </td>
                                        <td style="height: 40px;">
                                            <select id="txtVehicleNo" class="form-control">
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <input type="button" id="Button1" value="Agent Sale" class="btn btn-primary" onclick="GetAgentSalesClick();" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="width: 49%; float: right;">
                                <table>
                                    <tr>
                                        <td>
                                            <div id="divchblroutes" style="float: left; width: 215px; height: 100px; border: 1px solid gray;
                                                overflow: auto;">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />
                    <br />
                    <div id="divFillScreen">
                    </div>
                    <br />
                    <br />
                    <br />
                    <br />
                    <div id="divInventory">
                    </div>
                    <br />
                    <div>
                      <%--  <a target="_blank" id="BtnPrintDetails" class="btn btn-primary" style="text-decoration: none;"
                            onclick="GetPrintDetails();">Get Print Details </a>--%>
                            <%--<button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="GetPrintDetails();"><i class="fa fa-print"></i> Print</button>--%>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <br />
</asp:Content>
