<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EditIndent.aspx.cs" Inherits="EditIndent" %>

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
        .ddlsize {
            width: 230px;
            height: 30px;
            font-size: 16px;
            border: 1px solid gray;
            border-radius: 7px 7px 7px 7px;
        }

        .datepicker {
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
                    BindDeliverInventory();
                    BindCollectionInventory();
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
        var DairyStatus = "";
        function ddlAgentNameChange() {
            DairyStatus = "Delivers";
            GetEditIndentValues();
            BindDeliverInventory();
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
        var pktrate;
        var ltr_rate;
        var Units;
        var UnitQty;
        var invqty;
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
                    pktrate = msg[0].orderunitRate;
                    ltr_rate = msg[0].UnitPrice;
                    Unitqty = msg[0].Unitqty;
                    invqty = msg[0].invqty;
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
            var pkt_dqty = 0;
            var pkt_qty = 0;
            var pkt_rate = 0;
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
                    txtqty = $(this).find('#hdnltr_Unitqty').val();
                    ddlDelivered = $(this).find('#txtLtrQty').text();
                    txtCost = $(this).find('#txtLtr_rate').text();
                    pkt_dqty = $(this).find('#txtPkts_Dqty').val();
                    pkt_qty = $(this).find('#hdnPkt_UnitQty').val();
                    pkt_rate = $(this).find('#txtPkt_rate').text();
                    tubQty = $(this).find('#txtTubQty').val();
                    Total = $(this).find('#txtTotal_Value').text();
                    DeliverTable.push({ ProductName: txtProductName, Product_sno: txtProductSno, IndentNo: txtIndentNo, ltr_UnitQty: txtqty, ltr_Qty: ddlDelivered, Ltr_rate: txtCost, pkt_dqty: pkt_dqty, pkt_qty: pkt_qty, pkt_rate: pkt_rate, tubQty: tubQty, Total: Total });
                }
            });
            var hdnISno = 0;
            var Delivered = "";
            var ltr_UnitQty = 0;
            var hdnISno = 0;
            var Cost = 0;
            var pkt_dqty = 0;
            var pkt_qty = 0;
            var leak = 0;
            var ltr_Qty = 0;
            var RQty = 0;
            var tubQty = 0;
            
            DeliverTable.push({ ProductName: ProductName, Product_sno: ProductSno, IndentNo: DIndentNo, ltr_UnitQty: ltr_UnitQty, ltr_Qty: ltr_Qty, Ltr_rate: ltr_rate, pkt_dqty: pkt_dqty, pkt_qty: pkt_qty, pkt_rate: pktrate, tubQty: tubQty, Unitqty: Unitqty, invqty: invqty });
            $('#divFillScreen').setTemplateURL('IndentEdit.htm');
            $('#divFillScreen').processTemplate(DeliverTable);
            calcTot();
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
            var txtDate = document.getElementById('datepicker').value;
            if (txtDate == "") {
                alert("Please Select Date");
                return false;
            }
            var data = { 'operation': 'GetEditIndentValuesClick', 'RouteID': ddlRouteName, 'BranchID': BranchName, 'IndDate': txtDate };
            var s = function (msg) {
                if (msg) {
                    //BindDeliverInventory();
                    //BindCollectionInventory();
                    BindDeliverInventory();
                    $('#divFillScreen').removeTemplate();
                    $('#divFillScreen').setTemplateURL('IndentEdit.htm');
                    $('#divFillScreen').processTemplate(msg);
                    calcTot();
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
                    Indentdetails.push({ Productsno: $(this).find('#hdnProductSno').val(), Product: $(this).find('#txtProductName').text(), DelQty: $(this).find('#txtLtrQty').text(), Rate: $(this).find('#txtLtr_rate').text(), pkt_dqty: $(this).find('#txtPkts_Dqty').val(), pkt_qty: $(this).find('#hdnPkt_UnitQty').val(), pkt_rate: $(this).find('#txtPkt_rate').text(), tub_qty: $(this).find('#txtTubQty').val(), IndentNo: $(this).find('#txtIndentNo').text() });
                }
            });
            //added by akbar 20-May-2022
            var rowInventory = $("#tableInventory tr:gt(0)");
            var Inventorydetails = new Array();
            $(rowInventory).each(function (i, obj) {
                if ($(this).find('#txtSno').text() == "" || $(this).find('#txtGivenQty').val() == "") {
                }
                else {
                    Inventorydetails.push({ SNo: $(this).find('#txtSno').text(), InvSno: $(this).find('#hdnInvSno').val(), GivenQty: $(this).find('#txtGivenQty').val(), BalanceQty: $(this).find('#txtbalanceQty').text() });
                }
            });
            if (Inventorydetails.length == 0) {
                alert("Please Fill Given Qty");
                return false;
            }
            //end added by akbar 20-May-2022
            var BranchName = document.getElementById('ddlBranchName').value;
            var ddlRouteName = document.getElementById('ddlRouteName').value;
            var data = { 'operation': 'btnNewEditIndentSaveClick', 'data': Indentdetails, 'Inventorydetails': Inventorydetails, 'BranchID': BranchName, 'refno': ddlRouteName, 'indentdate': txtDate};
            var s = function (msg) {
                if (msg) {
                    //alert(msg);
                    CollectionInventrySaveClick();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(data, s, e);
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

                //$(TubQty).closest("tr").find("#txtUnitQty").val(parseFloat(totltrvalue).toFixed(2));
               // $(TubQty).closest("tr").find("#txtQtypkts").val(parseFloat(totltrvalue).toFixed(2));

                $(TubQty).closest("tr").find("#txtLtrQty").text(parseFloat(totltrvalue).toFixed(2))
                $(TubQty).closest("tr").find("#txtPkts_Dqty").val(parseFloat(totalpkts).toFixed(2));

                var val = parseFloat(totltrvalue).toFixed(2);
                OrderUnitChange(TubQty);
                calcTot();
            }
        }
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

                //$(PktQty).closest("tr").find("#txtUnitQty").val(parseFloat(totltrvalue).toFixed(2));
                //$(PktQty).closest("tr").find("#txtQtypkts").val(parseFloat(totltrvalue).toFixed(2));

                $(PktQty).closest("tr").find("#txtLtrQty").text(parseFloat(totltrvalue).toFixed(2))
                $(PktQty).closest("tr").find("#txtTubQty").val(parseFloat(totaltub).toFixed(2));
                var val = parseFloat(totltrvalue).toFixed(2);
                OrderUnitChange(PktQty);
                calcTot();
            }
        }
        function OrderUnitChange(UnitQty) {
            var totalqty;
            var qty = 0.0;
            var Rate = 0;
            var rate = 0;
            var total = 0;
            var totalltr = 0;
            var totallpkts = 0;
            var TotalRate = 0;
            var cnt = 0;
            if (UnitQty.value == "") {
                $(UnitQty).closest("tr").find("#txtOrderTotal").text(parseFloat(total).toFixed(2));
                $('.Unitqtyclass').each(function (i, obj) {
                    // var qtyclass = $(this).val();
                    //var qtyclass = $(this).closest('tr').find('#txtUnitQty').val();
                    var qtyclass = $(this).closest('tr').find('#txtPkts_Dqty').val();

                    if (qtyclass == "" || qtyclass == "0") {
                    }
                    else {
                        totallpkts += parseFloat(qtyclass);
                        cnt++;
                    }
                });
                //                var FloatQty = qty.toFixed(2)
                //                alert(cnt);
                document.getElementById('txt_totqty').innerHTML = parseFloat(totallpkts).toFixed(2);
                $('.rateclass').each(function (i, obj) {
                    rate += parseFloat($(this).text());
                });
                var Floatrate = rate.toFixed(2)
                document.getElementById('txt_totRate').innerHTML = parseFloat(Floatrate).toFixed(2);
                $('.totalclass').each(function (i, obj) {
                    total += parseFloat($(this).text());
                });
                document.getElementById('txt_total').innerHTML = parseFloat(total).toFixed(2);
                return false;
            }
            var Qty = $(UnitQty).closest("tr").find("#hdnUnitQty").val();
            var Units = $(UnitQty).closest("tr").find("#hdnUnits").val();
            Rate = $(UnitQty).closest("tr").find("#txtPkt_rate").text();
            var Units = $(UnitQty).closest("tr").find("#hdnUnits").val();
            //var unitqty = $(UnitQty).closest("tr").find("#txtUnitQty").val();
            var unitqty = $(UnitQty).closest("tr").find("#txtPkts_Dqty").val();
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
            var FinalRate = 0;
            FinalRate = unitqty * Rate;
            $(UnitQty).closest("tr").find("#txtTotal_Value").text(parseFloat(FinalRate).toFixed(2));
            cnt = 0;
            $('.Unitqtyclass').each(function (i, obj) {
                // var qtyclass = $(this).val();
                //var qtyclass = $(this).closest('tr').find('#txtUnitQty').val();
                var qtyclass = $(this).closest('tr').find('#txtPkts_Dqty').val();

                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totallpkts += parseInt(qtyclass);

                    cnt++;
                }
            });
            //            alert(cnt);
            //            var FloatQty = qty.toFixed(2)
            document.getElementById('txt_TotalPkts').innerHTML = parseFloat(totallpkts).toFixed(2);
            rate = 0;
            //$('.rateclass').each(function (i, obj) {
            //    rate += parseFloat($(this).text());
            //});
            //document.getElementById('txt_totRate').innerHTML = parseFloat(rate).toFixed(2);
            //total = 0;
            //$('.totalclass').each(function (i, obj) {
            //    total += parseFloat($(this).text());
            //});
            //document.getElementById('txt_total').innerHTML = parseFloat(total).toFixed(2);
        }
        var FinalAmount;
        function calcTot() {
            var qty = 0.0;
            var rate = 0;
            var total = 0;
            var totallpkts = 0;
            var totallAmount = 0;
            var totalltr = 0;
            var cnt = 0;
            $('.Unitqtyclass').each(function (i, obj) {
                //var qtyclass = $(this).next.next.next.text();
                var qtyclass = $(this).closest('tr').find('#txtPkts_Dqty').val();
                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totallpkts += parseFloat(qtyclass);
                    cnt++;
                }
            });


            $('.clsTotal').each(function (i, obj) {
                //var qtyclass = $(this).next.next.next.text();
                var totalclass = $(this).closest('tr').find('#txtTotal_Value').text();
                if (totalclass == "" || totalclass == "0") {
                }
                else {
                    totallAmount += parseFloat(totalclass);
                    cnt++;
                }
            });

            document.getElementById('txt_TotalAmount').innerHTML = parseFloat(totallAmount).toFixed(2);

            
            document.getElementById('txt_TotalPkts').innerHTML = parseFloat(totallpkts).toFixed(2);
            //$('.rateclass').each(function (i, obj) {
            //    rate += parseFloat($(this).text());
            //});
            //document.getElementById('txt_totRate').innerHTML = parseFloat(rate).toFixed(2);
            //$('.totalclass').each(function (i, obj) {
            //    total += parseFloat($(this).text());
            //});
            //document.getElementById('txt_total').innerHTML = parseFloat(total).toFixed(2);
            FinalAmount = total;

        }
        function CollectionInventrySaveClick() {

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
            var txtDate = document.getElementById('datepicker').value;
            if (txtDate == "") {
                alert("Please Select Date");
                return false;
            }

            
            var rowInventory = $("#tableInventory tr:gt(0)");
            var Inventorydetails = new Array();
            $(rowInventory).each(function (i, obj) {
                if ($(this).find('#txtSno').text() == "" || $(this).find('#txtReceivedQty').val() == "") {
                }
                else {
                    Inventorydetails.push({ SNo: $(this).find('#txtSno').text(), InvSno: $(this).find('#hdnInvSno').val(), ReceivedQty: $(this).find('#txtReceivedQty').val(), BalanceQty: $(this).find('#txtbalanceQty').text() });
                }
            });
            if (Inventorydetails.length == 0) {
                alert("Please Fill Received Qty");
                return false;
            }
            var data = { 'operation': 'CollectioninventrySaveClick', 'inddate': txtDate, 'RouteSno': ddlRouteName, 'BranchID': BranchName, 'Inventorydetails': Inventorydetails };
            var s = function (msg) {
                if (msg) {
                    receiveInventorydetailssave();
                    alert(msg);
                    document.getElementById('BtnSave').value = "Edit";
                    if (msg == "Session Expired") {
                        window.location = "Login.aspx";
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


        
        function numberOnlyExample() {
            if ((event.keyCode < 48) || (event.keyCode > 57))
                return false;
        }
        



        var InventoryTable;
        function AddInventoryRows() {
            InventoryTable = [];
            var txtsno = 0;
            var txtInvName = "";
            var txtInvSno = "";
            var txtInvqty = "";
            var txtbalanceQty = "";
            var rows = $("#tableInventory tr:gt(0)");
            var Inventory = document.getElementById('cmb_Inventory');
            var InventorySno = Inventory.options[Inventory.selectedIndex].value;
            var InventoryName = Inventory.options[Inventory.selectedIndex].text;
            var Checkexist = false;
            $('.InventoryClass').each(function (i, obj) {
                var IName = $(this).text();
                if (IName == InventoryName) {
                    alert("Inventory Already Added");
                    Checkexist = true;
                }
            });
            if (Checkexist == true) {
                return;
            }
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtSno').text() != "") {
                    txtsno = $(this).find('#txtSno').text();
                    txtInvName = $(this).find('#txtInvName').text();
                    txtInvSno = $(this).find('#hdnInvSno').val();
                    txtInvqty = $(this).find('#txtInvqty').text();
                    txtbalanceQty = $(this).find('#txtbalanceQty').text();
                    InventoryTable.push({ Sno: txtsno, InventoryName: txtInvName, InventorySno: txtInvSno, Qty: txtInvqty });
                }
            });
            var Sno = parseInt(txtsno) + 1;
            var txtInvqty = 0;
            InventoryTable.push({ Sno: Sno, InventoryName: InventoryName, InventorySno: InventorySno, Qty: txtInvqty });
            $('#divInventory').setTemplateURL('DeliverInventory6.htm');
            $('#divInventory').processTemplate(InventoryTable);
            DeliverCal();
        }

        var ColInventoryTable;
        function AddColInventoryRows() {
            ColInventoryTable = [];
            var txtsno = 0;
            var txtInvName = "";
            var txtInvSno = "";
            var txtInvqty = "";
            var txtbalanceQty = "";
            var rows = $("#tableColInventory tr:gt(0)");
            var Inventory = document.getElementById('cmb_colInventory');
            var InventorySno = Inventory.options[Inventory.selectedIndex].value;
            var InventoryName = Inventory.options[Inventory.selectedIndex].text;
            var Checkexist = false;
            $('.InventoryClass').each(function (i, obj) {
                var IName = $(this).text();
                if (IName == InventoryName) {
                    alert("Inventory Already Added");
                    Checkexist = true;
                }
            });
            if (Checkexist == true) {
                return;
            }
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtSno').text() != "") {
                    txtsno = $(this).find('#txtSno').text();
                    txtInvName = $(this).find('#txtInvName').text();
                    txtInvSno = $(this).find('#hdnInvSno').val();
                    txtInvqty = $(this).find('#txtInvqty').text();
                    txtbalanceQty = $(this).find('#txtbalanceQty').text();
                    ColInventoryTable.push({ Sno: txtsno, InventoryName: txtInvName, InventorySno: txtInvSno, Qty: txtInvqty });
                }
            });
            var Sno = parseInt(txtsno) + 1;
            var txtInvqty = 0;
            ColInventoryTable.push({ Sno: Sno, InventoryName: InventoryName, InventorySno: InventorySno, Qty: txtInvqty });
            $('#divcolInventory').setTemplateURL('CollectionInventory9.htm');
            $('#divcolInventory').processTemplate(ColInventoryTable);
            DeliverCal();
        }

        function BindDeliverInventory() {
            DairyStatus = "Delivers";
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
            var txtDate = document.getElementById('datepicker').value;
            if (txtDate == "") {
                alert("Please Select Date");
                return false;
            }
            var data = { 'operation': 'GetDeliverInventory', 'bid': BranchName, 'RouteSno': ddlRouteName, 'inddate': txtDate, 'DairyStatus': DairyStatus };
            var s = function (msg) {
                if (msg) {

                    //                    CollectionCal();
                    //if (DairyStatus == "Delivers") {
                    $('#divInventory').removeTemplate();
                    $('#divInventory').setTemplateURL('DeliverInventory6.htm');
                    $('#divInventory').processTemplate(msg);
                    DeliverCal();
                    BindCollectionInventory();
                    //}


                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function BindCollectionInventory() {
            DairyStatus = "Collections";
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
            var txtDate = document.getElementById('datepicker').value;
            if (txtDate == "") {
                alert("Please Select Date");
                return false;
            }
            var data = { 'operation': 'GetDeliverInventory', 'bid': BranchName, 'DairyStatus': DairyStatus, 'RouteSno': ddlRouteName, 'inddate': txtDate };
            var s = function (msg) {
                if (msg) {

                    //                    CollectionCal();
                    //if (DairyStatus == "Collections") {
                    $('#divcolInventory').setTemplateURL('CollectionInventory9.htm');
                    $('#divcolInventory').processTemplate(msg);
                    InvenCal();
                    //}

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function AddInventory() {
            FillInventory();
            $('#divMainAddNewRow').css('display', 'block');
        }
        function AddColInventory() {
            FillInventory();
            $('#divColMainAddNewRow').css('display', 'block');
        }
        function FillInventory() {
            var Branchid = document.getElementById("ddlSalesOffice").value;
            var data = { 'operation': 'GetInventoryDeatails', 'BranchID': Branchid };
            var s = function (msg) {
                if (msg) {
                    BindInventoryName(msg);
                    BindColInventoryName(msg);

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindColInventoryName(msg) {
            document.getElementById('cmb_colInventory').options.length = "";
            var veh = document.getElementById('cmb_colInventory');
            var length = veh.options.length;
            for (i = length - 1; i >= 0; i--) {
                veh.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Inventory";
            opt.value = "";
            veh.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].RouteName;
                    opt.value = msg[i].rid;
                    veh.appendChild(opt);
                }
            }
        }
        function BindInventoryName(msg) {
            document.getElementById('cmb_Inventory').options.length = "";
            var veh = document.getElementById('cmb_Inventory');
            var length = veh.options.length;
            for (i = length - 1; i >= 0; i--) {
                veh.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Inventory";
            opt.value = "";
            veh.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].RouteName;
                    opt.value = msg[i].rid;
                    veh.appendChild(opt);
                }
            }
        }
        function btnInventoryAddClick() {
            var InventoryName = document.getElementById('cmb_Inventory').value;
            if (InventoryName == "select Inventory" || InventoryName == "") {
                alert("Select Inventory Name");
                return false;
            }
            AddInventoryRows();
        }
        function btnColInventoryAddClick() {
            var InventoryName = document.getElementById('cmb_colInventory').value;
            if (InventoryName == "select Inventory" || InventoryName == "") {
                alert("Select Inventory Name");
                return false;
            }
            AddColInventoryRows();
        }
        function OrdersCloseClick() {
            $('#divMainAddNewRow').css('display', 'none');
        }
        function ColInvCloseClick() {
            $('#divColMainAddNewRow').css('display', 'none');
        }
        
        function DeliverCal() {
            $('.GivenQtyclass').each(function (i, obj) {
                var Qty = $(this).closest("tr").find('.InvQtyClass').text();
                if (Qty == "" || Qty == "0") {
                    var GivenQty = $(this).val();
                    if (GivenQty == "") {
                        GivenQty = "0";
                    }
                    var balanceQty = parseInt(Qty) + parseInt(GivenQty);
                    $(this).closest("tr").find('.ClassbalanceQty').text(balanceQty);
                }
                else {
                    var GivenQty = $(this).val();
                    if (GivenQty == "0" || GivenQty == "") {
                        GivenQty = 0;
                        var balanceQty = parseInt(Qty) + parseInt(GivenQty);
                        $(this).closest("tr").find('.ClassbalanceQty').text(balanceQty);
                    }
                    else {
                        var balanceQty = parseInt(Qty) + parseInt(GivenQty);
                        $(this).closest("tr").find('.ClassbalanceQty').text(balanceQty);
                    }
                }
            });
        }
        function InvenCal() {
            $('.ReceivedQtyclass').each(function (i, obj) {
                var InvQtyClass = $(this).closest("tr").find('.InvQtyClass').text();
                if (InvQtyClass == "" || InvQtyClass == "0") {
                }
                else {
                    var Qty = $(this).closest("tr").find('.InvQtyClass').text();
                    var ReceivedQty = $(this).val();
                    if (ReceivedQty == "0" || ReceivedQty == "") {
                        ReceivedQty = 0;
                        var balanceQty = parseInt(Qty) - parseInt(ReceivedQty);
                        $(this).closest("tr").find('.ClassbalanceQty').text(balanceQty);
                    }
                    else {
                        var balanceQty = parseInt(Qty) - parseInt(ReceivedQty);
                        $(this).closest("tr").find('.ClassbalanceQty').text(balanceQty);
                    }
                }
            });
        }
        var TotalInvqty = 0;
        function GivenQtyChange(Givenqty) {
            var Qty = $(Givenqty).closest("tr").find("#txtInvqty").text();
            if (Givenqty.value == "") {
                TotalInvqty = parseInt(Qty) + parseInt(0);
                $(Givenqty).closest("tr").find("#txtbalanceQty").text(TotalInvqty);
                return false;
            }
            TotalInvqty = parseInt(Qty) + parseInt(Givenqty.value);
            $(Givenqty).closest("tr").find("#txtbalanceQty").text(TotalInvqty);
        }
        function ReceivedQtyChange(ReceivedQty) {
            var Qty = $(ReceivedQty).closest("tr").find("#txtInvqty").text()
            if (ReceivedQty.value == "") {
                $(ReceivedQty).closest("tr").find("#txtbalanceQty").text(Qty);
                return false;
            }
            TotalInvqty = parseInt(Qty) - parseInt(ReceivedQty.value);
            $(ReceivedQty).closest("tr").find("#txtbalanceQty").text(TotalInvqty);
        }


       
        //end added by akbar 20-May-2022
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
        <h1>Edit Indent<small>Preview</small>
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

                <div id="tbldropdowns">
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
                                <input type="date" id="datepicker" class="form-control" placeholder="DD-MM-YYYY" class="form-control" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td style="height: 40px;">
                                <input type="button" id="Button1" value="GET Indent" class="btn btn-primary" onclick="GetEditIndentValues();" />
                            </td>
                        </tr>
                    </table>
                    <div id="divFillScreen">
                    </div>
                    <%--<div align="center">
                        <span style="color: Red; font-size: 18px; font-weight: bold;"><b>Inventory</b></span>
                    </div>--%>
                    <div id="divInventory">
                    </div>
                    <%--<div align="center">
                        <span style="color: Red; font-size: 18px; font-weight: bold;"><b>Inventory Collection</b></span>
                    </div>--%>
                    <div id="divcolInventory">
                    </div>
                    <div align="center">
                    <input type="button" id="btnSave" value="Save" onclick="btnEditIndentSaveClick();" class="btn btn-primary" style="text-align: center" />
                    </div>
                    <%--Added by akbar 20-May-2022--%>
                    <%--<div id="divDelivers">
                <input type="button" value="Deliveries" id="btndeliveries" class="btn btn-primary" onclick=" return divDeliveryclick();" />
            </div>--%>
                    <%--<div id="divCollections">
                <input type="button" value="Collections" id="btncollections" class="inputButton"
                    onclick=" return divCollectionsclick();" />
            </div>--%>
                    <%--end Added by akbar 20-May-2022--%>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

