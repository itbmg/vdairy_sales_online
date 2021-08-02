<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SpecialDispatch.aspx.cs" Inherits="SpecialDispatch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <link href="Css/style.css" rel="stylesheet" type="text/css" />
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <style type="text/css">
        .ddlsize
        {
            width: 166px;
            height: 30px;
            font-size: 16px;
            border: 1px solid gray;
            border-radius: 7px 7px 7px 7px;
        }
        .divselectedclass
        {
            font-size: 16px;
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
            FilSpecialDispatches();
            FillEmployee();
        });
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
        function GetLocalSaleClick() {
            var SplDcName = document.getElementById('txtSplDcName').value;
            if (SplDcName == "") {
                alert("Enter Spl DC Name");
                return false;
            }
            GetBranchProducts();
            GetBranchInventory();
        }
        function GetBranchProducts() {
            var SplDcName = document.getElementById('txtSplDcName').value;
            var BranchID = document.getElementById('ddlSalesOffice').value;
            var data = { 'operation': 'GetSplDcProducts', 'SplDcName': SplDcName, 'BranchID': BranchID };
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
            callHandler(data, s, e);
        }
        function GetProducts(msg) {
            $('#divFillScreen').removeTemplate();
            $('#divFillScreen').setTemplateURL('LocalSaleProducts.htm');
            $('#divFillScreen').processTemplate(msg);
        }
        function GetBranchInventory() {
            var data = { 'operation': 'GetAgetntsaleInventory' };
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
            $('#divInventory').setTemplateURL('LocalInventory.htm');
            $('#divInventory').processTemplate(msg);
        }
        function GetPrintDetails() {
            window.location = "DeliveryChallanReport.aspx";
        }
        function numberOnlyExample() {
            if ((event.keyCode < 48) || (event.keyCode > 57))
                return false;
        }
        function btnLocalSaleSaveclick() {
            var SplDcName = document.getElementById('txtSplDcName').value;
            if (SplDcName == "") {
                alert("Enter Spl Dc Name");
                return false;
            }
            var rows = $("#tabledetails tr:gt(0)");
            var Orderdetails = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtProductQty').val() != "" || $(this).find('#txtProductQty').val() != "0") {
                    var txtProductQty = $(this).find('#txtProductQty').val();
                    var tot_qty = 0;
                    tot_qty = parseFloat(txtProductQty);
                    if (tot_qty > 0) {
                        Orderdetails.push({ ProductSno: $(this).find('#hdnProductSno').val(), Product: $(this).find('#txtproduct').text(), Qty: $(this).find('#txtProductQty').val() });
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
                if (inventorydetails.length == 0) {
                    alert("Please Enter Inventory");
                    return false;
                }
            }
            var BranchID = document.getElementById('ddlSalesOffice').value;
            var Employee = document.getElementById('ddlEmployee').value;
            if (Employee == "Select Employee" || Employee == "") {
                alert("Select Employee Name");
                return false;
            }
            var Data = { 'operation': 'btnSpecialSaleSaveclick', 'data': Orderdetails, 'invdata': inventorydetails, 'routename': SplDcName, 'BranchID': BranchID, 'EmpID': Employee };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    $('#divFillScreen').removeTemplate();
                    $('#divFillScreen').setTemplateURL('LocalSaleProducts.htm');
                    $('#divFillScreen').processTemplate();
                    $('#divInventory').removeTemplate();
                    $('#divInventory').setTemplateURL('LocalInventory.htm');
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
        function FilSpecialDispatches() {
            var data = { 'operation': 'GetSpecialDispatches' };
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
        function fillTriproutes(msg) {
            document.getElementById('divchblroutes').innerHTML = "";
            for (var i = 0; i <= msg.length; i++) {
                if (typeof msg[i] === "undefined" || msg[i].routename == "" || msg[i].routename == null) {
                }
                else {
                    var label = document.createElement("span");
                    var hidden = document.createElement("input");
                    hidden.type = "hidden";
                    hidden.name = "hidden";
                    hidden.value = msg[i].routesno;
                    var checkbox = document.createElement("input");
                    checkbox.type = "checkbox";
                    checkbox.name = "checkbox";
                    checkbox.value = msg[i].routesno;
                    checkbox.id = "checkbox";
                    checkbox.onclick = function () { checked(this); };
                    //checkbox.className = 'checkinput';
                    checkbox.className = 'chkclass';
                    document.getElementById('divchblroutes').appendChild(checkbox);
                    label.innerHTML = msg[i].routename;
                    label.className = 'divselectedclass';
                    document.getElementById('divchblroutes').appendChild(label);
                    document.getElementById('divchblroutes').appendChild(hidden);
                    document.getElementById('divchblroutes').appendChild(document.createElement("br"));
                }
            }
        }
        function FillEmployee() {
            var data = { 'operation': 'Get_SpL_Employe' };
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
        function GetClearClick() {
            document.getElementById('txtSplDcName').disabled = false;
            document.getElementById('txtSplDcName').value = "";
            $('#divSO').css('display', 'block');
            $('.chkclass').each(function (i, obj) {
                $(this).removeAttr('checked')
            });
            $('#divFillScreen').removeTemplate();
            $('#divFillScreen').setTemplateURL('LocalSaleProducts.htm');
            $('#divFillScreen').processTemplate();
            $('#divInventory').removeTemplate();
            $('#divInventory').setTemplateURL('LocalInventory.htm');
            $('#divInventory').processTemplate();
        }
        function checked(thisid) {
            $('.chkclass').each(function (i, obj) {
                if ($(this).val() == thisid.value) {
                    $(this).attr('checked', 'checked');
                    document.getElementById('txtSplDcName').value = $(this).next().text();
                    document.getElementById('txtSplDcName').disabled = true;
                    $('#divSO').css('display', 'none');
                }
                else {
                    $(this).removeAttr('checked')
                }
            });
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
            Special Despatch<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Special Despatch</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Special Despatch Details
                </h3>
            </div>
            <div class="box-body">
                <div style="width: 100%;">
                    <div style="width: 69%; float: left; height: 120px;">
                        <table>
                            <tr>
                                <td id="divSO">
                                    <select id="ddlSalesOffice" class="form-control">
                                    </select>
                                </td>
                                <td>
                                    <input type="text" id="txtSplDcName" class="form-control" placeholder="Enter Special Dispatch Name" />
                                </td>
                                <td style="width:5px;"></td>
                                <td>
                                    <select id="ddlEmployee" class="form-control">
                                    </select>
                                </td>
                                <td style="width:5px;"></td>
                                <td>
                                    <input type="button" id="btnGetProducts" value="Get Products" class="btn btn-primary"
                                        onclick="GetLocalSaleClick();"  />
                                </td>
                                <td style="width:5px;"></td>
                                <td>
                                    <input type="button" id="btnClear" value="Clear" class="btn btn-warning" onclick="GetClearClick();"/>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 30%; float: right; height: 120px;">
                        <div id="divchblroutes" style="float: left; width: 215px; height: 120px; border: 1px solid gray;
                            overflow: auto; border-radius: 3px 0px 0px 3px;">
                        </div>
                    </div>
                    <br />
                    <br />
                </div>
                <div style="width: 100%;">
                    <div id="divFillScreen">
                    </div>
                    <div id="divInventory">
                    </div>
                    <br />
                    <div>
                     <%--   <a target="_blank" id="BtnPrintDetails" class="btn btn-primary" style="text-decoration: none;" onclick="GetPrintDetails();">Get Print
                            Details </a>--%>
                            <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="GetPrintDetails();"><i class="fa fa-print"></i> Print</button>
                    </div>
                    <br />
                </div>
            </div>
        </div>
    </section>
</asp:Content>
