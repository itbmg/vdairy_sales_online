<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="LocalSale.aspx.cs" Inherits="LocalSale" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <%--<link href="Css/style.css" rel="stylesheet" type="text/css" />--%>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
     <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            FillLocalDispatches();
        });
        function GetLocalSaleClick() {
            var SalesType = ddlSalesType.options[ddlSalesType.selectedIndex].text;
            if (SalesType == "Staff Sale") {
                var LocalSaleName = document.getElementById('txtLocalSaleName').value;
                if (LocalSaleName == "") {
                    alert("Employee Number Does Not Matched");
                    return false;
                }
                 $('#divFillScreen').removeTemplate();
            }
            else {
                var LocalSaleName = document.getElementById('txtLocalSaleName').value;
                if (LocalSaleName == "") {
                    alert("Enter Sale Name");
                    return false;
                }
            }
            GetBranchProducts();
            GetBranchInventory();
        }
        function GetBranchProducts() {
            var data = { 'operation': 'GetLocalSaleProducts' };
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
            var LocalSaleName = document.getElementById('txtLocalSaleName').value;
            var ReceiptNo = document.getElementById('txtReceiptNo').value;
            var SalesType = document.getElementById('ddlSalesType').value;
            if (LocalSaleName == "") {
                alert("Enter Sale Name");
                return false;
            }
             var SalesType = ddlSalesType.options[ddlSalesType.selectedIndex].text;
             if (SalesType == "Staff Sale") {
                 var SalesTypeId = document.getElementById('txtHiddenName').value;
                 if (SalesTypeId == "") {
                     alert("Employee Name Does Not Matched");
                     return false;
                 }
             }
             if (SalesType == "Local Sale") {
                 var ReceiptNo = document.getElementById('txtReceiptNo').value;
                 if (ReceiptNo == "") {
                     alert("Enter ReceiptNo");
                     return false;
                 }
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
            var Data = { 'operation': 'btnLocalSaleSaveclick', 'data': Orderdetails, 'invdata': inventorydetails, 'routename': LocalSaleName, 'SalesType': SalesType, 'SalesTypeId': SalesTypeId, 'ReceiptNo': ReceiptNo };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    document.getElementById('txtLocalSaleName').value = "";
                    document.getElementById('txtReceiptNo').value = "";
                    document.getElementById('txtEmployeeSearch').value = "";
                    document.getElementById('txtSearch').value = "";
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
        function FillLocalDispatches() {
            var data = { 'operation': 'GetLocalDispatches' };
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

        var localsalenames = [];
        function FillLocalDispatches() {
            var data = { 'operation': 'GetLocalDispatches', 'SalesType': SalesType };
            var s = function (msg) {
                if (msg) {
                    localsalenames = msg;
                    var availableTags = [];
                    for (i = 0; i < msg.length; i++) {
                        availableTags.push(msg[i].routename);
                    }
                    $('#txtSearch').autocomplete({
                        source: availableTags,
                        change: localsalechange,
                        autoFocus: true
                    });

                }
            }
            var e = function (x, h, e) {
                alert(e.toString());
            };
            callHandler(data, s, e);
        }

        function localsalechange() {
            var desc = document.getElementById('txtSearch').value;
            SalesType = ddlSalesType.options[ddlSalesType.selectedIndex].text;
            if (SalesType == "Staff Sale") {
                for (var i = 0; i < localsalenames.length; i++) {
                    if (desc == localsalenames[i].routename) {
                        document.getElementById('txtLocalSaleName').value = localsalenames[i].routename;
                        document.getElementById('txtHiddenName').value = localsalenames[i].routesno;
                    }
                    else {
//                        alert("Employee Name Does Not Matched");
//                        return false;
                    }
                }
            }
        }
        function GetClearClick() {
            document.getElementById('txtLocalSaleName').disabled = false;
            document.getElementById('txtLocalSaleName').value = "";
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
        //        function checked(thisid) {
        //            $('.chkclass').each(function (i, obj) {
        //                if ($(this).val() == thisid.value) {
        //                    $(this).attr('checked', 'checked');
        //                    document.getElementById('txtLocalSaleName').disabled = true;
        //                    document.getElementById('txtLocalSaleName').value = $(this).next().text();
        //                }
        //                else {
        //                    $(this).removeAttr('checked')
        //                }
        //            });
        //        }
        //    $('.chkclass').each(function () {
        //        if ($(this).is(":checked")) {
        //            if (values == "") {
        //                values += $(this).next().next().val();

        //            }
        //            else {
        //                values += ';' + $(this).next().next().val();

        //            }
        //            //values += $(this).next().next().val() + '@';
        //        }
        //    });
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
        var SalesType = "";
        function ddlPaymentTypeChange(Payment) {
            document.getElementById('txtSearch').value = "";
            FillLocalDispatches();
            SalesType = Payment.options[Payment.selectedIndex].text;
            if (SalesType == "Local Sale") {
                $('#tdrerecipt').css('display', 'block');
                //                $('#txtEmployeeSearch').css('display', 'none');
                $('#txtSearchicon').css('display', 'none');
                $('#txtLocalsale').css('display', 'block');
                document.getElementById('txtLocalSaleName').value = "";
                document.getElementById('txtReceiptNo').value = "";
            }
            if (SalesType == "Staff Sale") {
                $('#tdrerecipt').css('display', 'none');
                //                $('#txtEmployeeSearch').css('display', 'block');
                $('#txtSearchicon').css('display', 'block');
                $('#txtLocalsale').css('display', 'none');
                FillLocalDispatches();
                document.getElementById('txtLocalSaleName').value = "";
                document.getElementById('txtReceiptNo').value = "";
            }
            if (SalesType == "Free Sale") {
                $('#tdrerecipt').css('display', 'none');
                //                $('#txtEmployeeSearch').css('display', 'none');
                $('#txtSearchicon').css('display', 'none');
                $('#txtLocalsale').css('display', 'block');
                document.getElementById('txtLocalSaleName').value = "";
                document.getElementById('txtReceiptNo').value = "";
                //                document.getElementById('txtSearch').value = "";
            }
        }
    </script>
    <style>
     .btn
        {
            padding:6px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Local Sale<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Local Sale</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Local Sale Details
                </h3>
            </div>
            <div class="box-body">
                <div style="width: 100%;">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 49%; float: left;">
                                <div>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <select id="ddlSalesType" class="form-control" onchange="ddlPaymentTypeChange(this);">
                                                    <option>Free Sale</option>
                                                    <option>Staff Sale</option>
                                                    <option>Local Sale</option>
                                                </select>
                                            </td>
                                              <td style="width:5px;"></td>
                                            <td id="tdrerecipt" style="display:none;" >
                                                <input type="text" id="txtReceiptNo" class="form-control" placeholder="Enter ReceiptNo" />
                                            </td>
                                            <td style="width:5px;"></td>
                                            <td id="txtLocalsale">
                                                <input type="text" id="txtLocalSaleName" class="form-control" placeholder="Enter Sale Name" />
                                            </td>
                                             <td style="height: 40px;">
                            <input id="txtHiddenName" type="hidden" class="form-control" name="HiddenName" />
                        </td>
                                            <td style="width:5px;"></td>
                                            <td>
                                                <input type="button" id="Button1" value="Get Products" class="btn btn-primary" onclick="GetLocalSaleClick();" />
                                            </td>
                                            <td style="width:5px;"></td>
                                            <td>
                                                <input type="button" id="btnClear" value="Clear" class="btn btn-warning" onclick="GetClearClick();" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td style="float: right;padding-right: 31px;" id="txtEmployeeSearch">
                             <input type="text" id="txtSearch" class="form-control" placeholder="Search Local Sale Name" />
                             
                            </td>
                            <td>
                                  <span class="input-group-btn">
                                            <button type="button" class="btn btn-info btn-flat" style="height: 30px;">
                                                <i class="fa fa-search" aria-hidden="true"></i>
                                            </button>
                                        </span>       
                                    </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                </div>
                <div style="width: 60%;">
                    <div id="divFillScreen">
                    </div>
                    <div id="divInventory">
                    </div>
                    <br />
                    <div>
                       <%-- <a target="_blank" id="BtnPrintDetails" class="btn btn-primary" style="text-decoration: none;" onclick="GetPrintDetails();">Get Print
                            Details </a>--%>
                             <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="GetPrintDetails();"><i class="fa fa-print"></i> Print</button>
                    </div>
                    <br />
                </div>
            </div>
        </div>
    </section>
</asp:Content>
