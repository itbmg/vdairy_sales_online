<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ClosingStockEdit.aspx.cs" Inherits="ClosingStockEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="Js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
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
        .ddlsize
        {
            width: 230px;
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
    </style>
    <script type="text/javascript">
        $(function () {
            FillSalesOffice();
            $("#datepicker").datepicker({ dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
                onSelect: function (selectedDate) {
                    ddlSalesOfficeChange();
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
        function btnBranchStock() {
            var soid = document.getElementById('ddlSalesOffice').value;
            ddlSalesOfficeChange(soid);

        }
        function ddlSalesOfficeChange(ID) {
            //FillProducts();
            //var BranchID = ID;
            var BranchID = document.getElementById('ddlSalesOffice').value;
            if (BranchID == "select" || BranchID == "") {
                alert("Select SalesOffice");
                return false;
            }
            var PaidDate = document.getElementById('datepicker').value;
            var data = { 'operation': 'GetSOClosingStock', 'BranchID': BranchID, 'PaidDate': PaidDate };
            var s = function (msg) {
                if (msg) {
                    //  BindProducts(msg);
                    $('#divFillScreen').removeTemplate();
                    $('#divFillScreen').setTemplateURL('StockEdit.htm');
                    $('#divFillScreen').processTemplate(msg);
                    getbranchinventory(BranchID);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function getbranchinventory(ID) {
            var BranchID = ID;
            var PaidDate = document.getElementById('datepicker').value;
            var data = { 'operation': 'GetInvClosingStock', 'BranchID': BranchID, 'PaidDate': PaidDate };
            var s = function (msg) {
                if (msg) {
                    //  BindProducts(msg);
                    $('#divInventory').removeTemplate();
                    $('#divInventory').setTemplateURL('TripInventory.htm');
                    $('#divInventory').processTemplate(msg);
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
        function btnPlantTripsaveclick() {
            var ddlsalesOffice = document.getElementById('ddlSalesOffice').value;
            var rows = $("#table_Indent_details tr:gt(0)");
            var Indentdetails = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtStockQty').val() != "" || $(this).find('#txtStockQty').val() != "0") {
                    var hdn_editqty = $(this).find('#hdnstckqty').val();
                    var txtProductQty = $(this).find('#txtStockQty').val();
                    var tot_qty = 0;
                    tot_qty = parseFloat(hdn_editqty) + parseFloat(txtProductQty);
                    if (Math.abs(tot_qty) > 0) {
                        Indentdetails.push({ Productsno: $(this).find('#hdnProductSno').val(), LeakQty: $(this).find('#txtleak').val(), StockQty: $(this).find('#txtStockQty').val(), RemainingQty: $(this).find('#hdnstckqty').val(), FreeQty: $(this).find('#hdnlekqty').val() });
                    }
                }
            });
            var invrows = $("#table_inventory_details tr:gt(0)");
            var inventorydetails = new Array();
            $(invrows).each(function (i, obj) {
                if ($(this).find('#txtInvQty').val() != "") {
                    inventorydetails.push({ Sno: $(this).find('#txtsno').val(), InventorySno: $(this).find('#hdnProductSno').val(), Qty: $(this).find('#txtInvQty').val() });
                }
            });
            var PaidDate = document.getElementById('datepicker').value;
            var data = { 'operation': 'btnclosingstockedit', 'data': Indentdetails, 'invdata': inventorydetails, 'refno': ddlsalesOffice, 'indentdate': PaidDate };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    $('#divFillScreen').removeTemplate();
                    $('#divFillScreen').setTemplateURL('StockEdit.htm');
                    $('#divFillScreen').processTemplate();
                    $('#divInventory').removeTemplate();
                    $('#divInventory').setTemplateURL('TripInventory.htm');
                    $('#divInventory').processTemplate();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(data, s, e);
        }

        function FillSalesOffice() {
            var data = { 'operation': 'GetPlantSalesOffice' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindSalesOffice(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindSalesOffice(msg) {
            var ddlsalesOffice = document.getElementById('ddlSalesOffice');
            var length = ddlsalesOffice.options.length;
            ddlsalesOffice.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            ddlsalesOffice.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlsalesOffice.appendChild(opt);
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            ClosingStock Edit<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">ClosingStock Edit</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>ClosingStock Details
                </h3>
            </div>
            <div class="box-body">
                <table>
                    <tr>
                        <td>
                            <span>Sales Office</span>
                        </td>
                        <td>
                            <select id="ddlSalesOffice" class="form-control" onchange="btnBranchStock();">
                            </select>
                        </td>
                        <td style="width:5px;">
                        </td>
                        <td>
                            <span>Indent Date</span>
                        </td>
                        <td>
                            <input type="date" id="datepicker" placeholder="DD-MM-YYYY" class="form-control" />
                        </td>
                        <td style="width:5px;">
                        </td>
                        <td>
                            <input type="button" id="btnGetDetails" value="Get Details" class="btn btn-primary" onclick="btnBranchStock();"/>
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
