<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EditIndent.aspx.cs" Inherits="EditIndent" %>
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
            //FillRoutes();
            FillSalesOffice();
            $("#datepicker").datepicker({
                dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
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
        function FillRoutes() {
            var BranchID = document.getElementById('ddlSalesOffice').value;
            var data = { 'operation': 'GetDespatches', 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
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
                    $('#divFillScreen').setTemplateURL('IndentEdit.htm');
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
        function AddNewRowDelivers() {
            FillCategeoryname();
            $('#divDeliveryProducts').css('display', 'block');
        }
        function FillCategeoryname() {
            var data = { 'operation': 'FillCategeoryname' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindCategeoryname(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        };
        function BindCategeoryname(msg) {
            var brnchprdtcatgryname = document.getElementById('cmb_brchprdt_Catgry_name');
            var length = brnchprdtcatgryname.options.length;
            brnchprdtcatgryname.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            brnchprdtcatgryname.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].categoryname != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].categoryname;
                    opt.value = msg[i].sno;
                    brnchprdtcatgryname.appendChild(opt);
                }
            }
        }
        function productsdata_categoryname_onchange() {
            var cmbcatgryname = document.getElementById("cmb_brchprdt_Catgry_name").value;
            var data = { 'operation': 'get_product_subcategory_data', 'cmbcatgryname': cmbcatgryname };
            var s = function (msg) {
                if (msg) {
                    fillproductsdata_subcatgry(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
            //AscCallHandler(data, s, e);
        };
        function fillproductsdata_subcatgry(msg) {
            var brnchsubcategory = document.getElementById('cmb__brnch_subcatgry');
            var length = brnchsubcategory.options.length;
            document.getElementById("cmb__brnch_subcatgry").options.length = null;
            document.getElementById("cmb__brnch_subcatgry").value = "select";
            //        for (i = 0; i < length; i++) {
            //            prdtsubcategory.options[i] = null;
            //        } 
            var opt = document.createElement('option');
            opt.innerHTML = "Select";
            brnchsubcategory.appendChild(opt);

            for (var i = 0; i < msg.length; i++) {
                if (msg[i].subcategorynames != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].subcategorynames;
                    opt.value = msg[i].sno;
                    brnchsubcategory.appendChild(opt);
                }
            }
        }
        function productsdata_subcategory_onchange() {
            var cmbsubcatgryname = document.getElementById("cmb__brnch_subcatgry").value;
            //    var e = document.getElementById("cmb__brnch_subcatgry");
            //    var cmbsubcatgryname = e.options[e.selectedIndex].value;
            var data = { 'operation': 'get_products_data', 'cmbsubcatgryname': cmbsubcatgryname };
            var s = function (msg) {
                if (msg) {
                    fillproductsdata_products(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
            // AscCallHandler(data, s, e);
        };
        function fillproductsdata_products(msg) {
            var cmbprdtname = document.getElementById('cmb_productname');
            var length = cmbprdtname.options.length;
            document.getElementById("cmb_productname").options.length = null;
            document.getElementById("cmb_productname").value = "select";
            //    for (i = 0; i < length; i++) {
            //        cmbprdtname.options[i] = null;
            //    }
            var opt = document.createElement('option');
            opt.innerHTML = "Select";
            cmbprdtname.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].ProductName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].ProductName;
                    opt.value = msg[i].sno;
                    cmbprdtname.appendChild(opt);
                }
            }
        }
        function DeliversCloseClick() {
            $('#divDeliveryProducts').css('display', 'none');
        }
        function btnDeliversAddClick() {
            var CategoryName = document.getElementById('cmb_brchprdt_Catgry_name').value;
            if (CategoryName == "select" || CategoryName == "") {
                alert("Select Category Name");
                return false;
            }
            var SubCategoryName = document.getElementById('cmb__brnch_subcatgry').value;
            if (SubCategoryName == "Select" || SubCategoryName == "") {
                alert("Select Sub Category Name");
                return false;
            }
            var productname = document.getElementById('cmb_productname').value;
            if (productname == "Select" || productname == "") {
                alert("Select product Name");
                return false;
            }
            AddRowDelivers();
        }
        var UnitPrice;
        var Units;
        var UnitQty;
        var QtyUnit;
        var orderunitRate;
        var Description;
        var ProductSno = 0;
        function ProductNameChane(sno) {
            ProductSno = sno.value;
            var BranchID = document.getElementById('ddlBranchName').value;
            var data = { 'operation': 'GetProductNamechange', 'ProductSno': ProductSno, 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    UnitPrice = msg[0].orderunitRate;

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var DeliverTable;
        function AddRowDelivers() {
            DeliverTable = [];
            var txtProductName = "";
            var txtProductSno = "";
            var txtqty = "";
            var txtIndentNo = 0;
            var ddlDelivered = "";
            var DIndentNo = 0;
            var txtCost = 0;
            var HdnIndentSno = 0;
            var indentcount = 0;
            var rows = $("#table_Indent_details tr:gt(0)");
            var Product = document.getElementById('cmb_productname');
            var ProductSno = Product.options[Product.selectedIndex].value;
            var ProductName = Product.options[Product.selectedIndex].text;
            var Checkexist = false;
            $('.ProductClass').each(function (i, obj) {
                var PName = $(this).text();
                if (PName == ProductName) {
                    alert("Product Already Added");
                    Checkexist = true;
                }
            });
            if (Checkexist == true) {
                return;
            }
            //            $('#hdnIndent').val(txtIndentNo);
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtProductName').text() != "") {
                    txtProductName = $(this).find('#txtProductName').text();
                    txtProductSno = $(this).find('#hdnProductSno').val();
                    txtIndentNo = $(this).find('#txtIndentNo').text();
                    if (indentcount == 0) {
                        DIndentNo = txtIndentNo;
                        indentcount++;
                    }
                    txtqty = $(this).find('#txtunitQty').text();
                    ddlDelivered = $(this).find('#txtDeliveryQty').val();
                    txtCost = $(this).find('#txtUnitCost').val();
                    DeliverTable.push({ ProductName: txtProductName, Product_sno: txtProductSno, IndentNo: txtIndentNo, unitQty: txtqty, DeliveryQty: ddlDelivered, UnitCost: txtCost });
                }
            });
            var hdnISno = 0;
            var Delivered = "";
            var Qty = 0;
            var hdnISno = 0;
            var Cost = 0;

            var leak = 0;
            var DQty = 0;
            var RQty = 0;
            DeliverTable.push({ ProductName: ProductName, Product_sno: ProductSno, IndentNo: DIndentNo, unitQty: Qty, DeliveryQty: DQty, UnitCost: UnitPrice });
            $('#divFillScreen').setTemplateURL('IndentEdit.htm');
            $('#divFillScreen').processTemplate(DeliverTable);


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
            if (BranchName == "select" || BranchName == "") {
                alert("Please Select Sales Office");
                return false;
            }
            
            var txtDate = document.getElementById('datepicker').value;
            if (txtDate == "") {
                alert("Please Select Date");
                return false;
            }
            var data = {
                'operation': 'GetEditIndentValuesClick', 'RouteID': ddlRouteName, 'BranchID': BranchName, 'IndDate': txtDate, 'soid': soid};
            var s = function (msg) {
                if (msg) {
                    $('#divFillScreen').removeTemplate();
                    $('#divFillScreen').setTemplateURL('IndentEdit.htm');
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
        function btnEditIndentSaveClick(id) {
            var txtDate = document.getElementById('datepicker').value;
            if (txtDate == "") {
                alert("Please Select Date");
                return false;
            }
            var rows = $("#table_Indent_details tr:gt(0)");
            var Indentdetails = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtProductName').text() != "") {
                    Indentdetails.push({ Productsno: $(this).find('#hdnProductSno').val(), Product: $(this).find('#txtProductName').text(), DelQty: $(this).find('#txtDeliveryQty').val(), Rate: $(this).find('#txtUnitCost').val(), IndentNo: $(this).find('#txtIndentNo').text() });
                }
            });
            //            var IndentNo = $(id).closest("tr").find('#txtIndentNo').text();
            //            var ProductSno = $(id).closest("tr").find('#hdnProductSno').val();
            //            var UnitCost = $(id).closest("tr").find('#txtUnitCost').val();
            //            var DelQty = $(id).closest("tr").find('#txtDeliveryQty').val();
            //            if (UnitCost == "") {
            //                alert("Please Enter Rate");
            //                return false;
            //            }
            //            if (DelQty == "") {
            //                alert("Please Enter Del Qty");
            //                return false;
            //            }
            var BranchName = document.getElementById('ddlBranchName').value;
            var ddlRouteName = document.getElementById('ddlRouteName').value;
            var data = { 'operation': 'btnEditIndentSaveClick', 'data': Indentdetails, 'BranchID': BranchName, 'refno': ddlRouteName, 'indentdate': txtDate };
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
            Edit Indent<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Edit Indent</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Edit Indent Details
                </h3>
            </div>
            <div class="box-body">
                <table align="center">
                    <tr>
                        <td>
                         <label for="lblBranch">
                            Sales Office
                            </label>
                        </td>
                        <td style="height: 40px;">
                            <select id="ddlSalesOffice" class="form-control" onchange="ddlSalesOfficeChange(this);">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="lblBranch">
                                Route Name</label>
                        </td>
                        <td style="height: 40px;">
                            <select id="ddlRouteName" class="form-control" onchange="ddlRouteNameChange(this);">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="lblBranch">
                                Agent Name</label>
                        </td>
                        <td style="height: 40px;">
                            <select id="ddlBranchName" class="form-control" onchange="ddlAgentNameChange(this);">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="lblBranch">
                                Indent Date</label>
                        </td>
                        <td style="height: 40px;">
                            <input type="date" id="datepicker" placeholder="DD-MM-YYYY" class="form-control" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <input type="button" id="Button1" value="GET Indent" class="btn btn-primary" onclick="GetEditIndentValues();" />
                        </td>
                    </tr>
                </table>
                <div id="divFillScreen">
                </div>
            </div>
        </div>
    </section>
</asp:Content>
