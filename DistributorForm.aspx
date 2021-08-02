<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DistributorForm.aspx.cs" Inherits="DistributorForm" %>

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
    <script type="text/javascript">
        $(function () {
            get_Distributor_details();
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
        });
        function showAddress() {
          // get_Distributor_details();
            $("#div_Distributor").css("display", "block");
            $("#div_ProductRanking").css("display", "none");
        }
        function showProductRanking() {
            //get_Distributor_details();

            Get_Products();
            $("#div_Distributor").css("display", "none");
            $("#div_ProductRanking").css("display", "block");
        }
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
            $("#div_DistributorData").show();
            $("#Distributor_FillForm").hide();
            $('#DistributorShowlogs').show();
            DistributorForClearAll();
        }
        function saveDistributorDetails() {
            var distributorname = document.getElementById('txtDistributorName').value;
            if (distributorname == "") {
                alert("Enter  distributorname");
                $("#txtDistributorName").focus();
                return false;
            }
            var buildingaddress = document.getElementById('txtBillingaddress').value;
            var street = document.getElementById('txtStreet').value;
            if (street == "") {
                alert("Enter  Street Name");
                $("#txtStreet").focus();
                return false;
            }
            var mandal = document.getElementById('txtMandal').value;
            var district = document.getElementById('txtdistrict').value;
            if (district == "") {
                alert("Enter  District Name");
                $("#txtdistrict").focus();
                return false;
            }
            var state = document.getElementById('txtState').value;
            if (state == "") {
                alert("Enter  state");
                $("#txtState").focus();
                return false;
            }
            var pin = document.getElementById('txtPincode').value;
            var tin = document.getElementById('txtTinNumber').value;

            var cst = document.getElementById('txtcst').value;
            var email = document.getElementById('txtemail').value;
            if (email == "") {
                alert("Enter  email");
                $("#txtemail").focus();
                return false;
            }
            var panno = document.getElementById('txtpanno').value;
            var sno = document.getElementById('lblAddress_sno').value;
            var btnval = document.getElementById('btn_DistributorDetailsSave').innerHTML;
            var data = { 'operation': 'saveDistributorDetails', 'panno': panno, 'cst': cst, 'email': email, 'distributorname': distributorname, 'buildingaddress': buildingaddress, 'street': street, 'mandal': mandal, 'district': district, 'state': state, 'pin': pin, 'tin': tin, 'sno': sno, 'btnVal': btnval };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        DistributorForClearAll();
                        get_Distributor_details();
                        $('#div_DistributorData').show();
                        $('#Distributor_FillForm').css('display', 'none');
                        $('#DistributorShowlogs').css('display', 'block');
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function DistributorForClearAll() {
            document.getElementById('txtDistributorName').value = "";
            document.getElementById('txtBillingaddress').value = "";
            document.getElementById('txtStreet').value = "";
            document.getElementById('txtMandal').value = "";
            document.getElementById('txtdistrict').value = "";
            document.getElementById('txtState').value = "";
            document.getElementById('txtPincode').value = "";
            document.getElementById('txtTinNumber').value = "";
            document.getElementById('txtcst').value = "";
            document.getElementById('txtpanno').value = "";
            document.getElementById('txtemail').value = "";
           // document.getElementById('txtCustomerCode').value = "";
            document.getElementById('btn_DistributorDetailsSave').innerHTML = "save";
        }
        function showDistributordesign() {
            $("#div_DistributorData").hide();
            $("#Distributor_FillForm").show();
            $('#DistributorShowlogs').hide();
        }
        function get_Distributor_details() {
            var data = { 'operation': 'get_Distributor_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        FillDistributorDetails(msg);
                        FillDistributorName(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function FillDistributorDetails(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">CompanyName</th><th scope="col" class="thcls">BuildingAddress</th><th scope="col" class="thcls">TIN Number</th><th scope="col"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row" class="1 tdmaincls" >' + msg[i].distributorname + '</td>';
                results += '<td data-title="brandstatus"  class="2">' + msg[i].buildingaddress + '</td>';
                results += '<td data-title="brandstatus" style="display:none" class="3">' + msg[i].street + '</td>';
                results += '<td data-title="brandstatus"style="display:none" class="4">' + msg[i].mandal + '</td>';
                results += '<td data-title="brandstatus" style="display:none" class="5">' + msg[i].district + '</td>';
                results += '<td data-title="brandstatus" style="display:none"class="6">' + msg[i].state + '</td>';
                results += '<td data-title="brandstatus" style="display:none" class="7">' + msg[i].pin + '</td>';
                results += '<td data-title="brandstatus" class="8">' + msg[i].tin + '</td>';
                results += '<td style="display:none" class="10">' + msg[i].cst + '</td>';
                results += '<td style="display:none" class="11">' + msg[i].email + '</td>';
                results += '<td style="display:none" class="12">' + msg[i].panno + '</td>';
                //results += '<td style="display:none" class="13">' + msg[i].customercode + '</td>';
                results += '<td style="display:none" class="9">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmeaddress(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_DistributorData").html(results);
        }
        function getmeaddress(thisid) {
            var distributorname = $(thisid).parent().parent().children('.1').html();
            var buildingaddress = $(thisid).parent().parent().children('.2').html();
            var street = $(thisid).parent().parent().children('.3').html();
            var mandal = $(thisid).parent().parent().children('.4').html();
            var district = $(thisid).parent().parent().children('.5').html();
            var state = $(thisid).parent().parent().children('.6').html();
            var pin = $(thisid).parent().parent().children('.7').html();
            var tin = $(thisid).parent().parent().children('.8').html();
            var sno = $(thisid).parent().parent().children('.9').html();
            var cst = $(thisid).parent().parent().children('.10').html();
            var email = $(thisid).parent().parent().children('.11').html();
            var panno = $(thisid).parent().parent().children('.12').html();
            //var customercode = $(thisid).parent().parent().children('.13').html();

            document.getElementById('txtDistributorName').value = distributorname;
            document.getElementById('txtBillingaddress').value = buildingaddress;
            document.getElementById('txtStreet').value = street;
            document.getElementById('txtMandal').value = mandal;
            document.getElementById('txtdistrict').value = district;
            document.getElementById('txtState').value = state;
            document.getElementById('txtPincode').value = pin;
            document.getElementById('txtTinNumber').value = tin;
            document.getElementById('txtcst').value = cst;
            document.getElementById('txtemail').value = email;
            document.getElementById('lblAddress_sno').value = sno;
            document.getElementById('txtpanno').value = panno;
           //document.getElementById('txtCustomerCode').value = customercode;
            document.getElementById('btn_DistributorDetailsSave').innerHTML = "Modify";
            $("#div_DistributorData").hide();
            $("#Distributor_FillForm").show();
            $('#DistributorShowlogs').hide();
        }
       

        //Product Ranking Details
//        function ddlproductRankSalesOfficeChange(ID) {
//            //FillProducts();
//            var BranchID = ID.value;
//            var data = { 'operation': 'GetBranchProducts', 'BranchID': BranchID };
//            var s = function (msg) {
//                if (msg) {
//                    BindProducts(msg);
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
//        }
        var othersnames = [];
        function Get_Products() {
            var data = { 'operation': 'Get_Products'};
            var s = function (msg) {
                if (msg) {
                    othersnames = msg;
                    var availableTags = [];
                    for (i = 0; i < msg.length; i++) {
                        availableTags.push(msg[i].ProdName);
                    }
                    $("#txtProductName").autocomplete({
                        source: function (req, responseFn) {
                            var re = $.ui.autocomplete.escapeRegex(req.term);
                            var matcher = new RegExp("^" + re, "i");
                            var a = $.grep(availableTags, function (item, index) {
                                return matcher.test(item);
                            });
                            responseFn(a);
                        },
                        change: GetOthersid,
                        autoFocus: true
                    });
                }
            }
            var e = function (x, h, e) {
                alert(e.toString());
            };
            callHandler(data, s, e);
        }
        function GetOthersid() {
            var desc = document.getElementById('txtProductName').value;
            for (var i = 0; i < othersnames.length; i++) {
                if (desc == othersnames[i].ProdName) {
                    document.getElementById('spnHiddenProductSno').innerHTML = othersnames[i].ProdID;
                }
            }
        }
        var othersnames1 = [];
        function FillDistributorName(msg) {
            othersnames1 = msg;
            var availableTags = [];
            for (i = 0; i < msg.length; i++) {
                availableTags.push(msg[i].distributorname);
            }
            $("#txtDistributor_Name").autocomplete({
                source: function (req, responseFn) {
                    var re = $.ui.autocomplete.escapeRegex(req.term);
                    var matcher = new RegExp("^" + re, "i");
                    var a = $.grep(availableTags, function (item, index) {
                        return matcher.test(item);
                    });
                    responseFn(a);
                },
                change: GetOthersid1,
                autoFocus: true
            });
        }
        function GetOthersid1() {
            var distri = document.getElementById('txtDistributor_Name').value;
            for (var i = 0; i < othersnames1.length; i++) {
                if (distri == othersnames1[i].distributorname) {
                    document.getElementById('spnHiddenDistributorSno').innerHTML = othersnames1[i].sno;
                }
            }
        }
        function Addproducts() {
            DataTable = [];
            var txtsno = 0;
            var txtProductName = "";
            var txtProductSno = "";
            var txtOrderQty = "";
            var txtOrderRate = "";
            var txtOrderTotal = "";
            var txtUnitqty = "";
            var txtUnits = "";
            var orderunitqty = "";
            var Qty = 0;
            var Rate = 0;
            var Total = 0;
            var txtPrvQty = 0;
            var Description = 0;
            var UnitPrice = 0;
            var txtDescription;
            //var ProductSno = document.getElementById('cmb_productname').value;
            var rows = $("#tabledetails tr:gt(0)");
            var ProductName = document.getElementById('txtProductName').value;
            if (ProductName == "") {
                //                alert("Please Select Product Name");
                return false;
            }
            // var ProductName = Product.options[Product.selectedIndex].text;
            var ProductSno = document.getElementById('spnHiddenProductSno').innerHTML;
            var Checkexist = false;
            $('.ProductClass').each(function (i, obj) {
                var PName = $(this).text();
                if (PName == ProductName) {
                    alert("Product Already Added");
                    Checkexist = true;
                    document.getElementById('txtProductName').value = "";
                }
            });
            if (Checkexist == true) {
                return;
            }
            // var IndentNo = $('#hdnIndentNo').val();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtsno').text() != "") {
                    txtsno = $(this).find('#txtsno').text();
                    txtProductName = $(this).find('#txtproduct').text();
                    txtProductSno = $(this).find('#hdnProductSno').val();
                    txtOrderQty = $(this).find('#txtOrderQty').val();
                    txtorderunitRate = $(this).find('#txtOrderRate').val();
                    txtOrderTotal = $(this).find('#txtOrderTotal').text();
                    orderunitqty = $(this).find('#txtUnitQty').val();
                    txtUnitqty = $(this).find('#hdnUnitQty').val();
                    txtOrderRate = $(this).find('#hdnRate').val();
                    txtDescription = $(this).find('#txtDescription').text();
                    //txtPrvQty = $(this).find('#txtPrvQty').text();
                    DataTable.push({ sno: txtsno, ProductCode: txtProductName, Productsno: txtProductSno, Total: txtOrderTotal, orderunitqty: orderunitqty, Rate: txtorderunitRate, Desciption: txtDescription });
                }
            });
            var Sno = parseInt(txtsno) + 1;
            var Prevqty = 0;
            DataTable.push({ sno: Sno, ProductCode: ProductName, Productsno: ProductSno, Total: Total, orderunitqty: 0, Rate: UnitPrice, Desciption: Description });
            var j = 1;
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" id="tabledetails">';
            results += '<thead><tr><th scope="col"></th><th scope="col">Product Name</th><th scope="col">Qty</th><th scope="col">Rate</th></tr></thead></tbody>';
            for (var i = 0; i < DataTable.length; i++) {
                results += '<tr ><td id="txtsno">' + j + '</td>';
                results += '<td scope="row" class="ProductClass" style="text-align:center;" id="txtproduct" >' + DataTable[i].ProductCode + '</td>';
                results += '<td style="display:none" ><input type="text"  id="hdnProductSno"  value="' + DataTable[i].Productsno + '"/></td>';

                results += '<td data-title="Capacity" class="3"><input type="text"  id="txtUnitQty" onkeyup="OrderUnitChange(this);" onkeypress = "return isFloat(event)"  class="Unitqtyclass" value="' + DataTable[i].orderunitqty + '"/></td></td>';
                //results += '<td data-title="Capacity" class="4"><span  id="txtDescription">' + DataTable[i].Desciption + '</span></td>';
                results += '<td data-title="Capacity" class="5"><input type="text" id="txtOrderRate" class="rateclass" onkeypress = "return isFloat(event)" value="' + DataTable[i].Rate + '"/></td></td></tr>';
//                results += '<td data-title="Capacity" class="6"><span id="txtOrderTotal"  class="totalclass">' + parseFloat(DataTable[i].Rate) * parseFloat(DataTable[i].orderunitqty) + '</span></td></tr>';
                j++;
            }
            results += '<tr><td></td><td></td><td data-title="Qtytotal"><span id="txt_totqty"></span></td><td  type="hidden"></td><td dat-tile="Total"><span id="txt_totRate" hidden></span></td><td data-title="Total"><span id="txt_total"></span></td><td ></td></tr>';
            results += '</table></div>';
            $("#divproducts").html(results);
            document.getElementById('txtProductName').value = "";
            //gettubscans();
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
            $('.Unitqtyclass').each(function (i, obj) {
                // var qtyclass = $(this).val();
                var qtyclass = $(this).closest('tr').find('#txtUnitQty').val();
                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totalltr += parseFloat(qtyclass);
                    cnt++;
                }
            });
            //                var FloatQty = qty.toFixed(2)
            //                alert(cnt);
            document.getElementById('txt_totqty').innerHTML = parseFloat(totalltr).toFixed(2);
        }
        $(document).click(function () {
            $('#txtProductName').keypress(function (e) {
                var key = e.which;
                if (key == 13)  // the enter key code
                {
                    $('input[name = butAssignProd]').click();
                    return false;
                }
            });
            $('input[name="butAssignProd"]').click(function () {
                Addproducts();
            });
        });

//        function OrderUnitChange(UnitQty) {
//            var totalqty;
//            var qty = 0.0;
//            var Rate = 0;
//            var rate = 0;
//            var total = 0;
//            var totalltr = 0;
//            var TotalRate = 0;
//            var cnt = 0;
//            $('.Unitqtyclass').each(function (i, obj) {
//                // var qtyclass = $(this).val();
//                var qtyclass = $(this).closest('tr').find('#txtUnitQty').val();
//                if (qtyclass == "" || qtyclass == "0") {
//                }
//                else {
//                    totalltr += parseFloat(qtyclass);
//                    cnt++;
//                }
//            });
//            //                var FloatQty = qty.toFixed(2)
//            //                alert(cnt);
//            document.getElementById('txt_totqty').innerHTML = parseFloat(totalltr).toFixed(2);
//        }
       

        function isFloat(evt) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                //if dot sign entered more than once then don't allow to enter dot sign again. 46 is the code for dot sign
                var parts = evt.srcElement.value.split('.');
                if (parts.length > 1 && charCode == 46)
                    return false;
                return true;
            }
        }
//        function calTotal_gst() {
//            var $row = $(this).closest('tr');
//            var price = 0;
//            price = parseFloat($row.find('.txtOrderRate').val());
//            var quantity = 0;
//            quantity = parseFloat($row.find('.txtUnitQty').val());
//            var cost = 0;
//            cost = quantity * price;
//            $row.find('#totalclass').text(parseFloat(cost).toFixed(2));
//            // clstotalval_gst();
//        }

//        $(document).click(function () {
//            $('#tabledetails1').on('change', '.txtOrderRate', calTotal_gst)
//        });

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
        function btnUpClick() {
            $(selectedindex).insertBefore($(selectedindex).prev());
        }
        function btnDownClick() {
            $(selectedindex).insertAfter($(selectedindex).next());
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
        function btnDistributorProductsSaveClick() {
            if (!confirm("Do you really want Save")) {
                return false;
            }
            var BranchName = document.getElementById('spnHiddenDistributorSno').innerHTML;
            //var totrate = document.getElementById('txt_totRate').value;
            var rows = $("#tabledetails tr:gt(0)");
            var Orderdetails = new Array();
            $(rows).each(function (i, obj) {
                var txtsno = $(this).find('#txtsno').text();
                var txtUnitQty = $(this).find('#txtUnitQty').val();
                if (txtsno == "" || txtUnitQty == "") {
                }
                else {
                    Orderdetails.push({ SNo: $(this).find('#txtsno').text(), ProductSno: $(this).find('#hdnProductSno').val(), Product: $(this).find('#txtproduct').text(), Rate: $(this).find('#txtOrderRate').val(), UnitCost: $(this).find('#txtUnitQty').val() });
                }
            });
            //var TotalPrice = parseFloat(totTotal) - parseFloat(FinalAmount);
            var Data = { 'operation': 'btnDistributorProductsSaveClick', 'distributorid': BranchName, 'distributerproduct': Orderdetails };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    $("#divproducts").empty();
                    //                    $(".Unitqtyclass").attr("disabled", true);
                    //                    $(".OfferUnitqtyclass").attr("disabled", true);
                    /// document.getElementById('BtnSave').value = "Edit";
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content">
        <!-- Small boxes (Stat box) -->
        <div class="row">
            <section class="content-header">
                <h1>
                    Distributor Details
                </h1>
                <ol class="breadcrumb">
                    <li><a href="#"><i class="fa fa-dashboard"></i>Operation</a></li>
                    <li><a href="#">Distributor Details</a></li>
                </ol>
            </section>
            <section class="content">
                <div class="box box-info">
                    <div class="box-header with-border">
                    </div>
                    <div class="box-body">
                        <div>
                            <ul class="nav nav-tabs">
                                <li id="id_tab_Address" class="active"><a data-toggle="tab" href="#" onclick="showAddress()">
                                    <i class="fa fa-user" aria-hidden="true"></i>&nbsp;&nbsp;Distributor Master</a></li>
                                <li id="id_tab_ProductRanking" class=""><a data-toggle="tab" href="#" onclick="showProductRanking()">
                                    <i class=""></i>&nbsp;&nbsp;DistributorDelivary</a></li>
                                
                            </ul>
                        </div>
                        <div id="div_Distributor" style="display: none;">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Distributor Details
                                </h3>
                            </div>
                            <div class="box-body">
                                <%--<div id="DistributorShowlogs" align="center">
                                        <input id="btn_addAddress" type="button" name="submit" value='AddAddress' class="btn btn-primary"
                                            onclick="showDistributordesign()" />
                                    </div>--%>
                                <div id="DistributorShowlogs" align="right" class="input-group" style="display: block;padding-bottom: 20px;">
                                    <div class="input-group-addon" style="width: 100px;">
                                        <span class="glyphicon glyphicon-plus-sign" onclick="showDistributordesign();"></span>
                                        <span onclick="showDistributordesign();">Add Distributor</span>
                                    </div>
                                </div>
                                <div id="div_DistributorData">
                                </div>
                                <div id='Distributor_FillForm' style="display: none;">
                                    <table align="center">
                                        <tr>
                                            <td>
                                                <label>
                                                    Distributor Name</label><span style="color: red; font-weight: bold">*</span>
                                                <input id="txtDistributorName" type="text" name="CompanyName" style="text-transform: capitalize;" class="form-control" placeholder="Enter CompanyName" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Address</label><span style="color: red; font-weight: bold">*</span>
                                                <input id="txtBillingaddress" type="text" name="BillingAddress" style="text-transform: capitalize;" class="form-control"
                                                    placeholder="Enter Building Address" />
                                            </td>
                                            <td style="width: 4px;">
                                            </td>
                                            <td>
                                                <label>
                                                    Street</label>
                                                <input id="txtStreet" type="text" name="Street" class="form-control" style="text-transform: capitalize;" placeholder="Enter Street" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Mandal</label>
                                                <input id="txtMandal" type="text" name="Mandal" class="form-control" style="text-transform: capitalize;" placeholder="Enter Mandal" />
                                            </td>
                                            <td style="width: 4px;">
                                            </td>
                                            <td>
                                                <label>
                                                    District</label>
                                                <input id="txtdistrict" type="text" name="district" class="form-control" style="text-transform: capitalize;" placeholder="Enter District" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    State</label>
                                                <input id="txtState" type="text" name="State" class="form-control" style="text-transform: capitalize;" placeholder="Enter State" />
                                            </td>
                                            <td style="width: 4px;">
                                            </td>
                                            <td>
                                                <label>
                                                    PIN Code</label>
                                                <input id="txtPincode" type="text" name="PINCode" class="form-control" placeholder="Enter PIN Code"
                                                    onkeypress="return isNumber(event)" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    TIN Number</label>
                                                <input id="txtTinNumber" type="text" name="TinNumber" class="form-control" placeholder="Enter TIN Number"
                                                    onkeypress="return isNumber(event)" />
                                            </td>
                                            <td style="width: 4px;">
                                            </td>
                                            <td>
                                                <label>
                                                    CST Number
                                                </label>
                                                <input id="txtcst" type="text" name="PINCode" class="form-control" placeholder="Enter CST Number" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Email Address</label><span style="color: red; font-weight: bold">*</span>
                                                <input id="txtemail" type="text" name="TinNumber" class="form-control" placeholder="Enter Email Address" />
                                            </td>
                                            <td style="width: 4px;">
                                            </td>
                                            <td>
                                                <label>
                                                    PAN Number</label>
                                                <input id="txtpanno" type="text" name="PINCode" class="form-control" placeholder="Enter PAN Number" />
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <label id="lblAddress_sno">
                                                </label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center" style="height: 40px;">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div class="input-group">
                                                                <div class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-ok" id="btn_DistributorDetailsSave1" onclick="saveDistributorDetails()">
                                                                    </span><span id="btn_DistributorDetailsSave" onclick="saveDistributorDetails()">save</span>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td style="padding-left: 7px;">
                                                            <div class="input-group">
                                                                <div class="input-group-close">
                                                                    <span class="glyphicon glyphicon-remove" id="btnAddressCancel1" onclick="cancelAddressdetails()">
                                                                    </span><span id="btnAddressCancel" onclick="cancelAddressdetails()">Close</span>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div id="div_ProductRanking" style="display: none;">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Product Ranking Details
                                </h3>
                            </div>
                            <div class="box-body">
                                <table align="center">
                                    <tr>
                                         <td>
                                              <label>
                                                    Distributor Name</label><span style="color: red; font-weight: bold">*</span>
                                                <input id="txtDistributor_Name" type="text" style="text-transform: capitalize;" class="form-control" placeholder="Select DistributorName" />
                                            <span id="spnHiddenDistributorSno" hidden></span>
                                            </td>
                                            <td style="width: 4px;">
                                            </td>
                                            <td>
                                              <label>
                                                    ProductName</label><span style="color: red; font-weight: bold">*</span>
                                                <input id="txtProductName" type="text" style="text-transform: capitalize;" class="form-control" placeholder="Select ProductName" />
                                                <input type="text" name="butAssignProd" placeholder="click here" style="display:none;"/>
                                                <span id="spnHiddenProductSno" hidden></span>
                                            </td>
                                            <td style="width: 4px;">
                                            </td>
                                             <td style="padding-top: 27px;">
                                                            <div class="input-group">
                                                                <div class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-ok" id="btnProduct" onclick="Addproducts()">
                                                                    </span><span id="btnProduct1" onclick="Addproducts()">Add</span>
                                                                </div>
                                                            </div>
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
                                                    <div id="divproducts">
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 233px;">
                                            <div class="input-group">
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-ok" id="btnSave1" onclick="btnDistributorProductsSaveClick()">
                                                    </span><span id="btnSave" onclick="btnDistributorProductsSaveClick()">Save</span>
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

