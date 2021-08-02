<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="InventaryMoniter.aspx.cs" Inherits="InventaryMoniter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script src="js/jquery-1.4.4.js?v=3004" type="text/javascript"></script>
    <script src="js/newjs/jquery.js?v=3004" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3004" type="text/javascript"></script>
    <link href="jquery.jqGrid-4.5.2/js/i18n/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <script src="jquery.jqGrid-4.5.2/src/i18n/grid.locale-en.js" type="text/javascript"></script>
    <script src="jquery.jqGrid-4.5.2/js/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script src="jquery-ui-1.10.3.custom/js/jquery-ui.js" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js?v=3004" type="text/javascript"></script>
    <script src="js/newjs/jquery-ui.js?v=3004" type="text/javascript"></script>
    <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .inputstable td
        {
            padding: 5px 5px 5px 5px;
        }
        .col
        {
            border: 1px solid #d5d5d5;
            text-align: center;
        }
        
        .BranchNames-table
        {
            width: 100%;
            border-collapse: collapse;
        }
        .BranchNames-table th
        {
            font-weight: bold;
            vertical-align: middle;
        }
        .BranchNames-table td, .BranchNames-table th
        {
            padding: 5px;
            text-align: left;
            border: 1px solid #ddd;
        }
        
        .BranchNames-table tr:nth-child(odd)
        {
            background: #f9f9f9;
        }
        .BranchNames-table tr:nth-child(even)
        {
            background: #ffffff;
        }
    </style>
    <style type="text/css">
        .inputstable td
        {
            padding: 5px 5px 5px 5px;
        }
        .col
        {
            border: 1px solid #d5d5d5;
            text-align: center;
        }
        
        .students-table
        {
            width: 100%;
            border-collapse: collapse;
        }
        .students-table th
        {
            font-weight: bold;
            vertical-align: middle;
        }
        .students-table td, .students-table th
        {
            padding: 5px;
            text-align: left;
            border: 1px solid #ddd;
        }
        
        .students-table tr:nth-child(odd)
        {
            background: #f9f9f9;
        }
        .students-table tr:nth-child(even)
        {
            background: #ffffff;
        }
    </style>
    <style>
        input[type=checkbox]
        {
            transform: scale(1.5);
        }
        input[type=checkbox]
        {
            width: 30px;
            height: 18px;
            margin-right: 8px;
            cursor: pointer;
            font-size: 10px;
            visibility: hidden;
        }
        input[type=checkbox]:after
        {
            content: " ";
            background-color: #fff;
            display: inline-block;
            margin-left: 10px;
            padding-bottom: 0px;
            color: #24b6dc;
            width: 16px;
            height: 16px;
            visibility: visible;
            border: 1px solid rgba(18, 18, 19, 0.12);
            padding-left: 3px;
            border-radius: 0px;
        }
        input[type="checkbox"]:not(:disabled):hover:after
        {
            border: 1px solid #24b6dc;
        }
        input[type=checkbox]:checked:after
        {
            content: "\2714";
            padding: -5px;
            font-weight: bold;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            //   branches_products_branchname();
            //  PoFillSalesOffice();
            $("#div_Distributor").css("display", "block");
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#datepicker').val(today);
            FillSalesOffice();

        });
//        function showBranchProducts() {
//            $("#BranchProductsShowLogs").css("display", "block");
//            $("#div_Distributor").css("display", "block");
//            $("#div_AgentProductFillform").css("display", "none");
//            $("#BranchProducts_FillForm").hide();
//        }
//        function showDistributordesign() {
//            FillSalesOffice();
////            branches_products_branchname();
//            $("#BranchProducts_FillForm").show();
//            $('#BranchProductsShowLogs').hide();
//            $("#div_Distributor").css("display", "block");
//            $("#div_AgentProductFillform").css("display", "none");
//        }
//        function showAgentProducts() {
//            get_SalesOffices_In_AgentProductsTab();
//            branches_products_branchname();
//            // branches_products_branchname();
//            $("#div_Distributor").css("display", "none");
//            $("#div_AgentProductFillform").css("display", "block");
//        }
        //Address Details
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        function ValidateAlpha(evt) {
            var keyCode = (evt.which) ? evt.which : evt.keyCode
            if ((keyCode < 65 || keyCode > 90) && (keyCode < 97 || keyCode > 123) && keyCode != 32)

                return false;
            return true;
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
        function cancelAddressdetails() {
            $("#BranchProducts_FillForm").hide();
            $('#BranchProductsShowLogs').show();
            BranchProductsForClearAll();
        }
        function FillSalesOffice() {
            var getbrnachname = [];
            var data = { 'operation': 'GetPlantSalesOffice' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindSalesOffice(msg)
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
        function btnGetInventaryButtonClick() {
            document.getElementById('div_students').innerHTML = "";
            var BranchID = document.getElementById('ddlSalesOffice').value;
            var fromdate = document.getElementById('datepicker').value;
            var data = { 'operation': 'get_PlantDirectSales_Wise_InventaryDetails', 'BranchID': BranchID, 'fromdate': fromdate };
            var s = function (msg) {
                if (msg) {
                    fill_AgentSale_Plant_Inventary_Details(msg);
                }
                else {

                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        };
        var Temptable = [];
        function fill_AgentSale_Plant_Inventary_Details(msg) {
            var status = "0";
            var present = '';
            var divstudents = document.getElementById('div_students');
            var tablestrctr = document.createElement('table');
            tablestrctr.id = "tabledetails";
            tablestrctr.setAttribute("class", "students-table");
            var k = 1;
            $(tablestrctr).append('<thead><tr><th>Sno</th><th><i style="font-size:20px;padding-right: 5px;"></i>DispName</th><th><i style="font-size:20px;padding-right: 5px;"></i>InventaryName</th><th>Issued</th><th>Received</th><th>Active</th></tr></thead><tbody></tbody>');
            for (var i = 0; i < msg.length; i++) {
                //                if (Temptable.indexOf(msg[i].InvName) == -1) {
                $(tablestrctr).append('<tr><td>' + k + '</td><td class="1">' + msg[i].BranchName + '</td><td class="1">' + msg[i].InvName + '</td><td class="clsIssuedQty" id="txtIssuedQty">' + msg[i].IssuedQty + '</td><td><input type="text" class="clsReceivedQty" id="txtReceivedQty"><input id="hdnInventarySno" class="clsInvetarySno" type="hidden" value="' + msg[i].InvSno + '" /><input id="hdndispsno" class="clsDispSno" type="hidden" value="' + msg[i].BranchID + '" /><input id="hdntripid" class="clsTripId" type="hidden" value="' + msg[i].Tripid + '" /></td><td><input type="checkbox" class="checkedid" id="ckbpresent" onclick="selectall_checks(this)";  value="' + msg[i].BranchID + '"></td></tr>');
                k++;
                //Temptable.push(msg[i].InvName);
                //                }
                //                else {
                //                }
            }
            //                        $(tablestrctr).append('<thead><tr><th>Sno</th><th><i style="font-size:20px;padding-right: 5px;"></i>Product Name</th><th>Active</th></tr></thead><tbody></tbody>');
            //                        for (var i = 0; i < PlantProductsArr.length; i++) {
            //                            var j = 0;
            //                            j = i + 1;
            //                            $(tablestrctr).append('<tr><td>' + j + '</td><td class="1">' + PlantProductsArr[i].ProdName + '</td><td name="idprevsmsstatus" style="display:none"><input id="hdnempsno" type="hidden" value="' + PlantProductsArr[i].ProdID + '" /></td><td><input type="checkbox" class="checkedid" id="ckbpresent" onclick="selectall_checks(this)";  value="' + PlantProductsArr[i].ProdID + '"></td></tr>');
            //                        }
            $(tablestrctr).append('<br/>');
            $(tablestrctr).append('<br/>');
            $(tablestrctr).append('<br/>');
            divstudents.appendChild(tablestrctr);
        }
        function btnPlant_InventarySaveClick() {
            var tempproductlist = [];
            var Branch_Id = document.getElementById('ddlSalesOffice').value;
            var fromdate = document.getElementById('datepicker').value;
            if (Branch_Id == "" || Branch_Id == "select") {
                alert("Please Select BranchName");
                $("#ddlSalesOffice").focus();
                return false;
            }
            $("input:checkbox[class=checkedid]:checked").each(function () {
                var $row = $(this).closest('tr');
                var InventarySno = $row.find('.clsInvetarySno').val();
                var IssuedQty = $row.find('.clsIssuedQty').val();
                var ReceivedQty = $row.find('.clsReceivedQty').val();
                var BranchId = $row.find('.checkedid').val();
                var TripId = $row.find('.clsTripId').val();
                status = "P";
                var abc = { InventarySno: InventarySno, IssuedQty: IssuedQty, ReceivedQty: ReceivedQty, BranchId: BranchId, TripID: TripId };
                tempproductlist.push(abc);
            });
            //            $(rows).each(function (i, obj) {
            //                    var Product_Id = $(this).f
            //                    var Rate = $(this).find('#txtRate').val();
            //                    var ProductName = $(this).text();
            //                    status = "P";
            //                    var abc = { Product_Id: Product_Id, Rate: Rate };
            //                    tempproductlist.push(abc);
            //            });
            var Data = { 'operation': 'btnPlant_InventarySaveClick', 'fromdate': fromdate, 'Branch_Id': Branch_Id, 'BrancProductList': tempproductlist };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById('div_students').innerHTML = "";
                    Temptable = [];
                    $("#divproducts").empty();
                    $('#BranchProducts_FillForm').css('display', 'none');
                    $('#BranchProductsShowLogs').css('display', 'block');
                    if (msg == "Session Expired") {
                        window.location = "Login.html";
                        //  OrderRefresh();
                    }
                }
                else {

                }
            };
            var e = function (x, h, e) {
            };
            CallHandlerUsingJson(Data, s, e);
        }
        function BranchProductsForClearAll() {
            document.getElementById('cmb__brnch_subcatgry').selectedIndex = "0";
            document.getElementById('cmb_brchprdt_Catgry_name').selectedIndex = "0";
            document.getElementById('ddlSalesOffice').value = "";
        }
        function get_SalesOffices_In_AgentProductsTab() {
            var data = { 'operation': 'GetPlantSalesOffice' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindAgentSalesOffice(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindAgentSalesOffice(msg) {
            var ddlsalesOffice = document.getElementById('ddlAgentProductSales');
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


        function ddlAgentProductSalesOfficeChanged() {
            var ddlsalesOffice = document.getElementById('ddlAgentProductSales').value;
            var data = { 'operation': 'Get_Sales_Office_Agents', 'BranchId': ddlsalesOffice };
            var s = function (msg) {
                if (msg) {
                    BindAgentName(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindAgentName(msg) {
            document.getElementById('ddlAgentName').options.length = "";
            var ddlAgentName = document.getElementById('ddlAgentName');
            var length = ddlAgentName.options.length;
            for (i = length - 1; i >= 0; i--) {
                ddlAgentName.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Agent Name";
            opt.value = "";
            ddlAgentName.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].AgentName;
                    opt.value = msg[i].AgentId;
                    ddlAgentName.appendChild(opt);
                }
            }
        }
        function fill_AgentProductTabData_In_Products(msg) {
            var brnchprdtcatgryname = document.getElementById('cmb_brchprdt_AgentCatgry_name');
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
            var subcatgry = document.getElementById('cmb__Agent_Product_subcatgry');
            var length = subcatgry.options.length;
            subcatgry.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            subcatgry.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].subcategorynam != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].subcategorynam;
                    opt.value = msg[i].sno;
                    subcatgry.appendChild(opt);
                }
            }
            //            var cmbprdtname = document.getElementById('cmb_productname');
            //            var length = cmbprdtname.options.length;
            //            cmbprdtname.options.length = null;
            //            var opt = document.createElement('option');
            //            opt.innerHTML = "select";
            //            cmbprdtname.appendChild(opt);
            //            for (var i = 0; i < msg.length; i++) {
            //                if (msg[i].ProductName != null && msg[i].cmb_productname == null) {
            //                    var opt = document.createElement('option');
            //                    opt.innerHTML = msg[i].ProductName;
            //                    opt.value = msg[i].sno;
            //                    cmbprdtname.appendChild(opt);
            //                }
            //            }
        }


        function Agentproductsdata_categoryname_onchange() {
            var cmbcatgryname = document.getElementById("cmb_brchprdt_AgentCatgry_name").value;
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
            //callHandler(data, s, e);
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        };
        function fillproductsdata_subcatgry(msg) {
            var brnchsubcategory = document.getElementById('cmb__Agent_Product_subcatgry');
            var length = brnchsubcategory.options.length;
            document.getElementById("cmb__Agent_Product_subcatgry").options.length = null;
            document.getElementById("cmb__Agent_Product_subcatgry").value = "select";
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
        function Agent_productsdata_subcategory_onchange() {

            var cmbsubcatgryname = document.getElementById("cmb__Agent_Product_subcatgry").value;
            //    var e = document.getElementById("cmb__brnch_subcatgry");
            //    var cmbsubcatgryname = e.options[e.selectedIndex].value;
            var data = { 'operation': 'get_products_data', 'cmbsubcatgryname': cmbsubcatgryname };
            var s = function (msg) {
                if (msg) {
                    fillAgent_ProductTab_productsDetails(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            //callHandler(data, s, e);
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        };
        var Temptable1 = [];
        function fillAgent_ProductTab_productsDetails(msg) {
            var status = "0";
            var present = '';
            var divstudents = document.getElementById('divAgentproducts');
            var tablestrctr = document.createElement('table');
            tablestrctr.id = "tabledetails1";
            tablestrctr.setAttribute("class", "students-table");
            var k = 1;

            $(tablestrctr).append('<thead><tr><th>Sno</th><th><i style="font-size:20px;padding-right: 5px;"></i>Product Name</th><th>Rate</th><th>IGST</th><th>CGST</th><th>SGST</th><th>Active</th></tr></thead><tbody></tbody>');
            for (var i = 0; i < msg.length; i++) {
                if (Temptable1.indexOf(msg[i].ProductName) == -1) {
                    $(tablestrctr).append('<tr><td>' + k + '</td><td class="1">' + msg[i].ProductName + '</td><td name="idprevsmsstatus" style="display:none"><input id="hdnempsno" class="clsProductsno" type="hidden" value="' + msg[i].sno + '" /></td></td><td><input type="text" class="clsRate" id="txtRate" ></td><td><input type="text" class="clsigst" id="txtigst" ></td><td><input type="text" class="clscgst" id="txtcgst"></td><td><input type="text" class="clssgst" id="txt_sgst"></td><td><input type="checkbox" class="checkedid" id="ckbpresent" onclick="selectall_checks(this)";  value="' + msg[i].sno + '"></td></tr>');
                    k++;
                    Temptable1.push(msg[i].ProductName);
                }
                else {
                }

            }
            //                        $(tablestrctr).append('<thead><tr><th>Sno</th><th><i style="font-size:20px;padding-right: 5px;"></i>Product Name</th><th>Active</th></tr></thead><tbody></tbody>');
            //                        for (var i = 0; i < PlantProductsArr.length; i++) {
            //                            var j = 0;
            //                            j = i + 1;
            //                            $(tablestrctr).append('<tr><td>' + j + '</td><td class="1">' + PlantProductsArr[i].ProdName + '</td><td name="idprevsmsstatus" style="display:none"><input id="hdnempsno" type="hidden" value="' + PlantProductsArr[i].ProdID + '" /></td><td><input type="checkbox" class="checkedid" id="ckbpresent" onclick="selectall_checks(this)";  value="' + PlantProductsArr[i].ProdID + '"></td></tr>');
            //                        }
            $(tablestrctr).append('<br/>');
            $(tablestrctr).append('<br/>');
            $(tablestrctr).append('<br/>');
            divstudents.appendChild(tablestrctr);
        }
        function btnAgentProductsSaveClick() {
            var tempproductlist = [];
            var BranchId = document.getElementById('ddlAgentProductSales').value;
            if (BranchId == "" || BranchId == "select") {
                alert("Please Select BranchName");
                $("#ddlSalesOffice").focus();
                return false;
            }
            var AgentId = document.getElementById('ddlAgentName').value;
            if (AgentId == "" || AgentId == "select") {
                alert("Please Select BranchName");
                $("#ddlSalesOffice").focus();
                return false;
            }
            $("input:checkbox[class=checkedid]:checked").each(function () {
                var $row = $(this).closest('tr');
                var Rate = $row.find('.clsRate').val(); //$(this).addClass('.clsRate').html();
                var igst = $row.find('.clsigst').val(); //$(this).addClass('.clsigst').html();
                var cgst = $row.find('.clscgst').val(); //$(this).addClass('.clscgst').html();
                //                var sgst = $(this).addClass('.clssgst').val();
                var sgst = $row.find('.clssgst').val();
                //var sgst = $(this).parent().parent().children('.clssgst').html();
                //                var sgst = $(this).prev('.clssgst').val();
                var Product_Id = $row.find('.clsProductsno').val();
                status = "P";
                var abc = { Product_Id: Product_Id, Rate: Rate, igst: igst, sgst: sgst, cgst: cgst };
                tempproductlist.push(abc);
            });
            //            $(rows).each(function (i, obj) {
            //                    var Product_Id = $(this).f
            //                    var Rate = $(this).find('#txtRate').val();
            //                    var ProductName = $(this).text();
            //                    status = "P";
            //                    var abc = { Product_Id: Product_Id, Rate: Rate };
            //                    tempproductlist.push(abc);
            //            });
            var type = "AgentWiseProduct";
            var Data = { 'operation': 'saveBranch_Products_Details', 'BranchId': AgentId, 'type': type, 'BrancProductList': tempproductlist };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById('divAgentproducts').innerHTML = "";
                    Temptable = [];
                    $("#divproducts").empty();
                    $('#div_AgentProductFillform').css('display', 'none');
                    $('#BranchProductsShowLogs').css('display', 'block');
                    if (msg == "Session Expired") {
                        window.location = "Login.html";
                        //  OrderRefresh();
                    }
                }
                else {

                }
            };
            var e = function (x, h, e) {
            };
            CallHandlerUsingJson(Data, s, e);
        }
        function divonclick(selected) {
            selectedindex = selected;
            if ($(selected).css('background-color') == 'rgb(255, 255, 255)' || $(selected).css('background-color') == 'rgba(0, 0, 0, 0)') {
                $('.divselectedclass').each(function () {
                    $(this).css('background-color', '#ffffff');
                });
                $(selected).css('background-color', '#ffffcc');
            }
            else {
                $('.divselectedclass').each(function () {
                    $(this).css('background-color', '#ffffff');
                });
            }
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
        //CheckBox Function
        function selectall_checks(thisid) {
            if ($(thisid).is(':checked')) {
                $(this).find(':checkbox').prop('checked', true);
            }
            else {
                $(this).find(':checkbox').prop('checked', true);
            }
        }    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content">
        <!-- Small boxes (Stat box) -->
        <div class="row">
            <section class="content-header">
                <h1>
                    Inventary Management
                </h1>
                <ol class="breadcrumb">
                    <li><a href="#"><i class="fa fa-dashboard"></i>Operation</a></li>
                    <li><a href="#">Inventary Management</a></li>
                </ol>
            </section>
            <section class="content">
                <div class="box box-info">
                    <div class="box-header with-border">
                    </div>
                    <div class="box-body">
                       <%-- <div>
                            <ul class="nav nav-tabs">
                                <li id="id_tab_BranchProduct" class="active"><a data-toggle="tab" href="#" onclick="showBranchProducts()">
                                    <i class="fa fa-user" aria-hidden="true"></i>&nbsp;&nbsp;InvetaryDetails</a></li>
                                <li id="id_tab_AgentProduct" style="display:none;" class="" ><a data-toggle="tab" href="#" onclick="showAgentProducts()">
                                    <i class=""></i>&nbsp;&nbsp;AgentProducts</a></li>
                            </ul>
                        </div>--%>
                        <div id="div_Distributor" style="display: none;">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Branch ProductDetails
                                </h3>
                            </div>
                            <div class="box-body">
                               <%-- <div id="BranchProductsShowLogs" align="center">
                                        <input id="btn_addAddress" type="button" name="submit" value='AddAddress' class="btn btn-primary"
                                            onclick="showDistributordesign()" />
                                    </div>--%>
                              <%--  <div id="BranchProductsShowLogs" align="right" class="input-group" style="display: block;
                                    padding-bottom: 20px;">
                                    <div class="input-group-addon" style="width: 100px;">
                                        <span class="glyphicon glyphicon-plus-sign" onclick="showDistributordesign();"></span>
                                        <span onclick="showDistributordesign();">Add Distributor</span>
                                    </div>
                                </div>--%>
                               <%-- <div id="div_BranchProductsData">
                                </div>--%>
                                <div id='BranchProducts_FillForm'>
                                    <table align="left">
                                        <tr>
                                            <td>
                                                <label>
                                                    BranchName</label>
                                                    </td>
                                                    <td>
                                                <select id="ddlSalesOffice" class="form-control" onchange="ddlSalesOfficeChanged();">
                                                </select>
                                            </td>
                                            <td style="width: 25px;">
                                            </td>
                                            <td>
                                                <label>
                                                    Date </label>
                                                    </td>
                                                    <td>
                                                <input type="date" id="datepicker" placeholder="DD-MM-YYYY" class="form-control" />
                                            </td>
                                             <td style="width: 25px;">
                                            </td>
                                            <td>
                                            <div class="input-group">
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-ok" id="spnInventaryBtn1" onclick="btnGetInventaryButtonClick()">
                                                    </span><span id="spnInventaryBtn" onclick="btnGetInventaryButtonClick()">Get</span>
                                                </div>
                                            </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table align="left">
                                        <tr>
                                            <td>
                                                <div id="div_students" style="padding: 0px 0px 5px 5px; font-family: 'Open Sans';
                                                    font-size: 13px; margin-top: 10px; margin-bottom: 10px; display: inline-block;">
                                                </div>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td style="padding-left: 233px;">
                                                <div class="input-group">
                                                    <div class="input-group-addon">
                                                        <span class="glyphicon glyphicon-ok" id="spnBranchClick" onclick="btnPlant_InventarySaveClick()">
                                                        </span><span id="spnBranchClick1" onclick="btnPlant_InventarySaveClick()">Save</span>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="div_AgentProductFillform" style="display: none;">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Agent Product Details
                                </h3>
                            </div>
                            <div class="box-body">
                                <table>
                                    <tr>
                                        <td>
                                         <label>
                                                    BranchName</label>
                                            <select id="ddlAgentProductSales" class="form-control" onchange="ddlAgentProductSalesOfficeChanged();">
                                            </select>
                                        </td>
                                        <td style="width: 5px;">
                                        </td>
                                        <td>
                                         <label>
                                                    AgentName</label>
                                            <select id="ddlAgentName" class="form-control" >
                                            </select>
                                        </td>
                                        <td style="width: 5px;">
                                        </td>
                                        <td>
                                                <label>
                                                    Category Name</label>
                                                <select id="cmb_brchprdt_AgentCatgry_name" class="form-control" onchange="return Agentproductsdata_categoryname_onchange();">
                                                </select>
                                            </td>
                                            <td style="width: 4px;">
                                            </td>
                                            <td>
                                                <label>
                                                    SubCategory Name</label>
                                                <select id="cmb__Agent_Product_subcatgry" class="form-control" onchange="return Agent_productsdata_subcategory_onchange();">
                                                    <option>select</option>
                                                </select>
                                            </td>
                                            <td style="width: 4px;">
                                            </td>
                                            
                                    </tr>
                                </table>
                                <br />
                                <table style="padding-left: 20%;" align="center">
                                    <tr>
                                        <td>
                                            <div class="box box-info" style="float: left;">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Product Details
                                                    </h3>
                                                </div>
                                                <div class="box-body">
                                                    <div id="divAgentproducts">
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 233px;">
                                            <div class="input-group">
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-ok" id="btnAgentSave1" onclick="btnAgentProductsSaveClick()">
                                                    </span><span id="btnAgentSave" onclick="btnAgentProductsSaveClick()">Save</span>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
        </div>
    </section>
</asp:Content>

