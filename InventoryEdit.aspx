<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="InventoryEdit.aspx.cs" Inherits="InventoryEdit" %>

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
                    GetEditIndentValues();
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
        function ddlSalesOfficeChange(ID) {
            var BranchID = ID.value;
            var data = { 'operation': 'GetDespatches', 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    bindRoutes(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        //        function FillRoutes() {
        //            var data = { 'operation': 'GetDespatches' };
        //            var s = function (msg) {
        //                if (msg) {
        //                    bindRoutes(msg);
        //                }
        //                else {
        //                }
        //            };
        //            var e = function (x, h, e) {
        //            };
        //            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        //            callHandler(data, s, e);
        //        }
        function bindRoutes(msg) {
            var ddlRouteName = document.getElementById('ddlRouteName');
            var length = ddlRouteName.options.length;
            ddlRouteName.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Route Name";
            ddlRouteName.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].routename != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].routename;
                    opt.value = msg[i].routesno;
                    ddlRouteName.appendChild(opt);
                }
            }
        }
        var RouteSno = 0;
        function ddlRouteNameChange(Id) {
            var data = { 'operation': 'GetRouteNameChange', 'RouteID': Id.value };
            var s = function (msg) {
                if (msg) {
                    BindBranchName(msg);
                    $('#divFillScreen').removeTemplate();
                    $('#divFillScreen').setTemplateURL('InventoryEdit.htm');
                    $('#divFillScreen').processTemplate();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function ddlAgentNameChange() {
            GetEditIndentValues();
        }
        function BindBranchName(msg) {
            document.getElementById('ddlBranchName').options.length = "";
            var veh = document.getElementById('ddlBranchName');
            var length = veh.options.length;
            for (i = length - 1; i >= 0; i--) {
                veh.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Agent Name";
            opt.value = "";
            veh.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null || msg[i].BranchName != "" || msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].b_id;
                    veh.appendChild(opt);
                }
            }
        }

        var TotalInvqty = 0;
        var TotalClosingInvqty = 0;
        function DeliveryChange(Givenqty) {
            var Qty = $(Givenqty).closest("tr").find("#hdninvdel").val();
            var invcloQty = $(Givenqty).closest("tr").find("#hdnclosing").val();
            if (Givenqty.value == "") {
                TotalInvqty = parseInt(Qty) + parseInt(0);
                $(Givenqty).closest("tr").find("#txtinvclosing").text(TotalInvqty);
                return false;
            }
            if (Qty > Givenqty.value) {
                TotalInvqty = parseInt(Qty) - parseInt(Givenqty.value);
                TotalClosingInvqty = parseInt(invcloQty) - parseInt(TotalInvqty);
                $(Givenqty).closest("tr").find("#txtinvclosing").text(TotalClosingInvqty);
            }
            if (Qty < Givenqty.value) {
                TotalInvqty = parseInt(Qty) - parseInt(Givenqty.value);
                TotalClosingInvqty = parseInt(invcloQty) + parseInt(TotalInvqty);

                $(Givenqty).closest("tr").find("#txtinvclosing").text(TotalClosingInvqty);
            }
            if (Qty == Givenqty.value) {
                TotalInvqty = parseInt(Qty) - parseInt(Givenqty.value);

                var invQty = $(Givenqty).closest("tr").find("#txtinvclosing").text();

                TotalClosingInvqty = parseInt(invQty) + parseInt(Givenqty.value);
                $(Givenqty).closest("tr").find("#txtinvclosing").text(TotalClosingInvqty);
            }
        }
        function ReceivingChange() {

        }

        function GetEditIndentValues() {
            var ddlRouteName = document.getElementById('ddlRouteName').value;
            if (ddlRouteName == "Select Route Name" || ddlRouteName == "") {
                alert("Please Select Route Name");
                return false;
            }
            var BranchName = document.getElementById('ddlBranchName').value;
            if (BranchName == "Select Agent Name" || BranchName == "") {
                alert("Please Select Agent Name");
                return false;
            }
            var soid = document.getElementById('ddlSalesOffice').value;
            if (soid == "select") {
                alert("Please Select SalesOffice");
                return false;
            }
            var transactiontype = document.getElementById('ddltransactiontype').value;
            var txtDate = document.getElementById('datepicker').value;
            if (txtDate == "") {
                alert("Please Select Date");
                return false;
            }
            var data = { 'operation': 'GetEditInventoryValues', 'RouteID': ddlRouteName, 'BranchID': BranchName, 'IndDate': txtDate, 'transactiontype': transactiontype, 'soid': soid };
            var s = function (msg) {
                if (msg) {
                    $('#divFillScreen').removeTemplate();
                    $('#divFillScreen').setTemplateURL('InventoryEdit.htm');
                    $('#divFillScreen').processTemplate(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function btnEditInventorySaveClick(id) {
            var txtDate = document.getElementById('datepicker').value;
            if (txtDate == "") {
                alert("Please Select Date");
                return false;
            }
            var rows = $("#table_Indent_details tr:gt(0)");
            var Indentdetails = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtProductName').text() != "") {
                    Indentdetails.push({ Productsno: $(this).find('#hdnProductSno').val(), Product: $(this).find('#txtProductName').text(), DelQty: $(this).find('#txtinvdel').val(), ReturnQty: $(this).find('#txtUnitCost').val(), IndentNo: $(this).find('#txtIndentNo').text() });
                }
            });
            var transactiontype = document.getElementById('ddltransactiontype').value;
            var BranchName = document.getElementById('ddlBranchName').value;
            var ddlRouteName = document.getElementById('ddlRouteName').value;
            var data = { 'operation': 'btnEditInventorySaveClick', 'data': Indentdetails, 'BranchID': BranchName, 'refno': ddlRouteName, 'indentdate': txtDate, 'transactiontype': transactiontype };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(data, s, e);
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Edit Inventory<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Edit Inventory</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Edit Inventory Details
                </h3>
            </div>
            <div class="box-body">
                <table align="center">
                    <tr>
                        <td>
                        <label for="lblBranch">
                            Sales Office</label>
                        </td>
                        <td style="height:40px;">
                            <select id="ddlSalesOffice" class="form-control" onchange="ddlSalesOfficeChange(this);">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap>
                            <label for="lblBranch">
                                Route Name</label>
                        </td>
                        <td style="height:40px;">
                            <select id="ddlRouteName" class="form-control" onchange="ddlRouteNameChange(this);">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap>
                            <label for="lblBranch">
                                Agent Name</label>
                        </td>
                        <td style="height:40px;">
                            <select id="ddlBranchName" class="form-control" onchange="ddlAgentNameChange(this);">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap>
                            <label for="lblBranch">
                                Indent Date</label>
                        </td>
                        <td style="height:40px;">
                            <input type="date" id="datepicker" placeholder="DD-MM-YYYY" class="form-control" />
                        </td>
                    </tr>
                    <tr class="divTransactionclass">
                        <td>
                        <label for="lblBranch">
                            Transaction Type
                            </label>
                        </td>
                        <td style="height:40px;">
                            <select id="ddltransactiontype" class="form-control">
                                <option selected="selected">Route</option>
                                <option>Branch</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <input type="button" id="Button1" value="Get Inventory" class="btn btn-primary" onclick="GetEditIndentValues();" />
                        </td>
                    </tr>
                </table>
                <div id="divFillScreen">
                </div>
            </div>
        </div>
    </section>
</asp:Content>
