<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ProductMaster.aspx.cs" Inherits="ProductMaster" %>

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
    <script type="text/javascript">

        $(function () {
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
          //  get_Address_details();
            $("#div_Address").css("display", "block");
            $("#div_Department").css("display", "none");
            $("#div_ProductRanking").css("display", "none");
            $("#div_Bankdetails").css("display", "none");
            $("#div_PoEntry").css("display", "none");
            $("#div_CashCollection").css("display", "none");
        }
        function showDepartment() {
            updatecategorytype()
            $("#div_Address").css("display", "none");
            $("#div_Department").css("display", "block");
            $("#div_ProductRanking").css("display", "none");
            $("#div_Bankdetails").css("display", "none");
            $("#div_PoEntry").css("display", "none");
            $("#div_CashCollection").css("display", "none");
        }
        function showProductRanking() {
            product_manages_subcatgry();
            updatesubcategorytype();
            $("#div_Address").css("display", "none");
            $("#div_Department").css("display", "none");
            $("#div_ProductRanking").css("display", "block");
            $("#div_Bankdetails").css("display", "none");
            $("#div_PoEntry").css("display", "none");
            $("#div_CashCollection").css("display", "none");
        }
        function showbankmaster() {

            $("#div_Address").css("display", "none");
            $("#div_Department").css("display", "none");
            $("#div_ProductRanking").css("display", "none");
            $("#div_Bankdetails").css("display", "block");
            $("#div_PoEntry").css("display", "none");
            $("#div_CashCollection").css("display", "none");
        }
        function showPOEntryDetails() {
            updatecategorytype();
//            product_manages_subcatgry();
            updatesubcategorytype();
            Bindproductunits();
            product_manages_products();

            $("#div_Address").css("display", "none");
            $("#div_Department").css("display", "none");
            $("#div_ProductRanking").css("display", "none");
            $("#div_Bankdetails").css("display", "none");
            $("#div_PoEntry").css("display", "block");
            $("#div_CashCollection").css("display", "none");
        }
        function showCashCollectionMaster() {
            $("#div_Address").css("display", "none");
            $("#div_Department").css("display", "none");
            $("#div_ProductRanking").css("display", "none");
            $("#div_Bankdetails").css("display", "none");
            $("#div_PoEntry").css("display", "none");
            $("#div_CashCollection").css("display", "block");
        }

        function updatecategorytype() {
            var target_tab_selector = $('#divsubcategory').attr('li');
            var target_tab_selector1 = $('#divcategory_products').attr('li');
            var target_tab_selector2 = $('#divproductsManage').attr('li');
            $(target_tab_selector).addClass('hide');
            $(target_tab_selector1).addClass('hide');
            $(target_tab_selector2).addClass('ui-tabs-active');

            var FormType = "NewProductMaster";
            var data = { 'operation': 'Updateproducttypemanage', 'FormType': FormType };
            var s = function (msg) {
                if (msg) {
                    updateproducttypemanagement(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function updateproducttypemanagement(msg) {
            var l = 0;
            var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col"></th><th scope="col">Department Name</th><th scope="col">Flag</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                var status = 'InActive';
                if (msg[i].flag == '1') {
                    status = 'Active';
                }
                results += '<tr style="background-color:' + COLOR[l] + '"><td><input id="btn_poplate" type="button"  onclick="getcatgeory(this)" name="submit" class="btn btn-primary" value="Edit" /></td>';
                results += '<td scope="row" class="1" >' + msg[i].Categoryname + '</td>';
                results += '<td scope="row" class="2" >' + status + '</td>';
                results += '<td scope="row" style="display:none" class="4" >' + msg[i].tcategory + '</td>';
                results += '<td style="display:none" class="3">' + msg[i].sno + '</td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_categorydata").html(results);
        }
        function getcatgeory(thisid) {
            var Categoryname = $(thisid).parent().parent().children('.1').html();
            var status = $(thisid).parent().parent().children('.2').html();
            var sno = $(thisid).parent().parent().children('.3').html();
            var tcategory = $(thisid).parent().parent().children('.4').html();
            document.getElementById('txt_productname').value = Categoryname;
            document.getElementById('cmb_prdmngflag').value = status;
            document.getElementById('txttcategoryName').value = tcategory;
            document.getElementById('btn_productmanagesave').value = "MODIFY";
            serial1 = sno;
        }
        function product_manages_subcatgry() {
            updatesubcategorytype();
            var FormType = "NewProductMaster";
            var data = { 'operation': 'intialize_productsmanagement_subcatgry', 'FormType': FormType };
            var s = function (msg) {
                if (msg) {
                    fillproduct_manage_subcatgry(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        };

        function fillproduct_manage_subcatgry(msg) {
            var categryname = document.getElementById('cmb_catgry_catgryname');
            var length = categryname.options.length;
            document.getElementById("cmb_catgry_catgryname").options.length = null;
            //    for (i = 0; i < length; i++) {
            //        categryname.options[i] = null;
            //    }
            var opt = document.createElement('option');
            opt.innerHTML = "Select";
            categryname.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].categoryname != null && msg[i].subproduct == null) {
                    var opt = document.createElement('option');

                    opt.innerHTML = msg[i].categoryname;
                    opt.value = msg[i].sno;
                    categryname.appendChild(opt);
                }
            }
        }


        function updatesubcategorytype() {
            var FormType = "NewProductMaster";
            var data = { 'operation': 'Updatesubcategorytypemanage', 'FormType': FormType };
            var s = function (msg) {
                if (msg) {
                    updatesubcategorytypemanagement(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function updatesubcategorytypemanagement(msg) {
            var l = 0;
            var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col"></th><th scope="col">Subcategory Name</th><th>Category Name</th><th>Description</th><th>Status</th></tr></thead></tbody>';

            for (var i = 0; i < msg.length; i++) {
                var status = 'InActive';
                if (msg[i].flag == '1') {
                    status = 'Active';
                }
                results += '<tr style="background-color:' + COLOR[l] + '"><td><input id="btn_poplate" type="button"  onclick="getsubcatgeory(this)" name="submit" class="btn btn-primary" value="Edit" /></td>';
                results += '<td scope="row" class="1" >' + msg[i].subcatname + '</td>';
                results += '<td scope="row" class="2" >' + msg[i].Categoryname + '</td>';
                results += '<td scope="row" class="5" >' + msg[i].description + '</td>';
                results += '<td scope="row" class="3" >' + status + '</td>';
                results += '<td style="display:none" class="4">' + msg[i].sno + '</td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }

            }

            results += '</table></div>';
            $("#div_sub_categorydata").html(results);
        }
        function getsubcatgeory(thisid) {
            var subcatname = $(thisid).parent().parent().children('.1').html();
            var Categoryname = $(thisid).parent().parent().children('.2').html();
            var status = $(thisid).parent().parent().children('.3').html();
            var sno = $(thisid).parent().parent().children('.4').html();
            var description = $(thisid).parent().parent().children('.5').html();
            document.getElementById('txt_catgry_subname').value = subcatname;
            document.getElementById('cmb_catgry_flag').value = status;
            document.getElementById('txtSubCategoryDescription').value = description;
            document.getElementById('btn_subcatgry_save').value = "MODIFY";
            $("#cmb_catgry_catgryname").find("option:contains('" + Categoryname + "')").each(function () {
                if ($(this).text() == Categoryname) {
                    $(this).attr("selected", "selected");
                }
            });
            serial2 = sno;
        }



        function prdtmngt_catgryValidation() {
            var x = document.getElementById("txt_productname").value;
            if (x == "") {
                alert("Please Provide CategoryName");
                $("#txt_productname").focus();
                return false;
            }
            var y = document.getElementById("cmb_prdmngflag").value;
            if (y == "") {
                alert("Please Select Flag");
                $("#cmb_prdmngflag").focus();
                return false;
            }
            else {
                producttypemanagement();
            }
        }
        function prdtmngt_subcatgryvalidation() {
            var x = document.getElementById("txt_catgry_subname").value;
            if (x == "") {
                alert("Please Provide SubCategoryName");
                $("#txt_catgry_subname").focus();
                return false;
            }
            var y = document.getElementById("cmb_catgry_catgryname").value;
            if (y == "" || y == "Select") {
                alert("Please Select CategoryName");
                $("#cmb_catgry_catgryname").focus();
                return false;
            }
            var z = document.getElementById("cmb_catgry_flag").value;
            if (z == "") {
                alert("Please Select Flag");
                $("#cmb_catgry_flag").focus();
                return false;
            }
            else {
                subcategorymanagement();
            }
        }
        var serial2 = 0;
        function subcategorymanagement() {
            var subcategoryname = document.getElementById('txt_catgry_subname').value;
            var categoryname = document.getElementById('cmb_catgry_catgryname').value;
            var subproductflag = document.getElementById('cmb_catgry_flag').value;
            var description = document.getElementById('txtSubCategoryDescription').value;
            var operationtype = document.getElementById('btn_subcatgry_save').value;
            var sno = serial2;
            var data = { 'operation': 'subcategorytypemanagement', 'sno': sno, 'subcategoryname': subcategoryname, 'categoryname': categoryname, 'subproductflag': subproductflag, 'description': description, 'operationtype': operationtype };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    subcategoryclear();
                    updatesubcategorytype();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function subcategoryclear() {
            document.getElementById('txt_catgry_subname').value = "";
            document.getElementById('cmb_catgry_catgryname').value = "";
            document.getElementById('txtSubCategoryDescription').value = "";
            document.getElementById('cmb_catgry_flag').value = "";
            document.getElementById('btn_subcatgry_save').value = "SAVE";
        }
        var serial1 = 0;
        function producttypemanagement() {
            var productname = document.getElementById('txt_productname').value;
            var producttypeflag = document.getElementById('cmb_prdmngflag').value;
            var tcategoryname = document.getElementById('txttcategoryName').value;
            var operationtype = document.getElementById('btn_productmanagesave').value; s
//            tcategoryname
            var sno = serial1;
            var data = { 'operation': 'producttypemanagement', 'sno': sno, 'productname': productname, 'tcategoryname': tcategoryname, 'producttypeflag': producttypeflag, 'operationtype': operationtype };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    producttypeclear();
                    updatecategorytype();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function producttypeclear() {
            document.getElementById('txt_productname').value = "";
            document.getElementById('cmb_prdmngflag').value = "";
            document.getElementById('btn_productmanagesave').value = "SAVE";
        }






        function getproductinv() {
            var data = { 'operation': 'intialize_Prdt_inventory' };
            var s = function (msg) {
                if (msg) {
                    fill_prdt_inv(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {

            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fill_prdt_inv(msg) {
            var productinv = document.getElementById('cmb_product_inv');
            var length = productinv.options.length;
            document.getElementById("cmb_product_inv").options.length = null;
            //    for (i = 0; i < length; i++) {
            //        productcategory.options[i] = null;
            //    }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Inventory";
            productinv.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].categoryname != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].categoryname;
                    opt.value = msg[i].sno;
                    productinv.appendChild(opt);
                }
            }
        }

        function product_manages_products() {
            Bindproductunits();
            getproductinv();
            var FormType = "NewProductMaster";
            var data = { 'operation': 'intialize_productsmanagement_products', 'FormType': FormType };
            var s = function (msg) {
                if (msg) {
                    fillproduct_manage_products(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        };

        function fillproduct_manage_products(msg) {
            var productcategory = document.getElementById('cmb_products_category');
            var length = productcategory.options.length;
            document.getElementById("cmb_products_category").options.length = null;
            //    for (i = 0; i < length; i++) {
            //        productcategory.options[i] = null;
            //    }
            var opt = document.createElement('option');
            opt.innerHTML = "Select";
            productcategory.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].categoryname != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].categoryname;
                    opt.value = msg[i].sno;
                    productcategory.appendChild(opt);
                }
            }
        }

        function Bindproductunits() {
            var FormType = "NewProductMaster";
            var data = { 'operation': 'Updateproductunitsmanage', 'FormType': FormType };
            var s = function (msg) {
                if (msg) {
                    UpdateProductUnitmanagement(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function UpdateProductUnitmanagement(msg) {
            var l = 0;
            var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col"></th><th>Category Name</th><th scope="col">Subcategory Name</th><th scope="col">Product Name</th><th scope="col">ProductCode</th><th scope="col">Qty</th><th scope="col">Product Unit</th><th scope="col">Unit Price</th><th scope="col">Status</th><th scope="col">Inventory Name</th><th scope="col">Tally Product</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                var status = 'InActive';
                var ifdflag = 'InActive';
                if (msg[i].flag == '1') {
                    status = 'Active';
                }
                if (msg[i].ifdflag == '1') {
                    ifdflag = 'Active';
                }
                results += '<tr style="background-color:' + COLOR[l] + '"><td><input id="btn_poplate" type="button"  onclick="getproductsdata(this)" name="submit" class="btn btn-primary" value="Edit" /></td>';
                results += '<td scope="row" class="1" >' + msg[i].Categoryname + '</td>';
                results += '<td scope="row" class="2" >' + msg[i].SubCatName + '</td>';
                results += '<td scope="row" class="3" >' + msg[i].ProductName + '</td>';
                results += '<td scope="row" class="12" >' + msg[i].ProductCode + '</td>';
                results += '<td scope="row" class="4" >' + msg[i].Qty + '</td>';
                results += '<td scope="row" class="5" >' + msg[i].ProductUnit + '</td>';
                results += '<td scope="row" class="6" >' + msg[i].UnitPrice + '</td>';
                results += '<td scope="row" style="display:none" class="7" >' + msg[i].VatPercent + '</td>';
                results += '<td scope="row" class="8" >' + status + '</td>';
                results += '<td scope="row" class="9" >' + msg[i].InvName + '</td>';

                results += '<td style="display:none" class="26" >' + msg[i].invqty + '</td>';

                results += '<td  class="11">' + msg[i].TProductName + '</td>';
                results += '<td style="display:none" class="10">' + msg[i].sno + '</td>';
                results += '<td style="display:none" scope="row" class="13" >' + msg[i].images + '</td>';
                results += '<td style="display:none" scope="row" class="14" >' + msg[i].ftplocation + '</td>';
                results += '<td style="display:none" class="15" >' + msg[i].specification + '</td>';
                results += '<td style="display:none" class="16" >' + msg[i].materialtype + '</td>';

                results += '<td style="display:none" class="18" >' + msg[i].categorysno + '</td>';
                results += '<td style="display:none" class="19" >' + msg[i].SubCatsno + '</td>';
                results += '<td style="display:none" class="17" >' + msg[i].perunitprice + '</td>';

                results += '<td style="display:none" class="20" >' + msg[i].hsncode + '</td>';
                results += '<td style="display:none" class="21" >' + msg[i].igst + '</td>';
                results += '<td style="display:none" class="22" >' + msg[i].cgst + '</td>';
                results += '<td style="display:none" class="23" >' + msg[i].sgst + '</td>';
                results += '<td style="display:none" class="24" >' + msg[i].gsttaxcategory + '</td>';
                results += '<td style="display:none" class="25" >' + msg[i].pieces + '</td>';
                results += '<td style="display:none" class="27" >' + msg[i].description + '</td>';
                results += '<td style="display:none" class="28" >' + ifdflag + '</td>';
                results += '<td><input id="btn_poplate" style="display:none" type="button"  onclick="getproductslineChart(this);"  name="submit" class="btn btn-primary" value="View" /></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_ProductsData").html(results);
        }
        function getproductsdata(thisid) {
            var Categoryname = $(thisid).parent().parent().children('.1').html();
            var SubCatName = $(thisid).parent().parent().children('.2').html();
            var ProductName = $(thisid).parent().parent().children('.3').html();
            var Qty = $(thisid).parent().parent().children('.4').html();
            var ProductUnit = $(thisid).parent().parent().children('.5').html();
            var UnitPrice = $(thisid).parent().parent().children('.6').html();
            //    var VatPercent = $(thisid).parent().parent().children('.7').html();
            var status = $(thisid).parent().parent().children('.8').html();
            var InvName = $(thisid).parent().parent().children('.9').html();
            var sno = $(thisid).parent().parent().children('.10').html();
            var TProductName = $(thisid).parent().parent().children('.11').html();
            var ProductCode = $(thisid).parent().parent().children('.12').html();
            var image = $(thisid).parent().parent().children('.13').html();
            var ftplocation = $(thisid).parent().parent().children('.14').html();

            var specification = $(thisid).parent().parent().children('.15').html();
            var materialtype = $(thisid).parent().parent().children('.16').html();
            var perunitprice = $(thisid).parent().parent().children('.17').html();

            var categorysno = $(thisid).parent().parent().children('.18').html();
            var subcatsno = $(thisid).parent().parent().children('.19').html();


            var hsncode = $(thisid).parent().parent().children('.20').html();
            var igst = $(thisid).parent().parent().children('.21').html();
            var cgst = $(thisid).parent().parent().children('.22').html();
            var sgst = $(thisid).parent().parent().children('.23').html();
            var gsttaxcategory = $(thisid).parent().parent().children('.24').html();
            var pieces = $(thisid).parent().parent().children('.25').html();

            var invqty = $(thisid).parent().parent().children('.26').html();
            var description = $(thisid).parent().parent().children('.27').html();
            var ifdflag = $(thisid).parent().parent().children('.28').html();

            //    document.getElementById('cmb_products_category').value = Categoryname;
            //    document.getElementById('txt_catgry_subname').value = SubCatName;
            document.getElementById('cmb_catgry_flag').value = status;
            document.getElementById('btn_subcatgry_save').value = "MODIFY";


            document.getElementById('cmb_products_category').value = categorysno;
            products_cateegoryname_onchange();
            document.getElementById('cmb_products_subcatgry').value = subcatsno;
            //    $("#cmb_products_category").find("option:contains('" + Categoryname + "')").each(function () {
            //        if ($(this).text() == Categoryname) {
            //            $(this).attr("selected", "selected");
            //        }
            //    });
            //    
            //    $("#cmb_products_subcatgry").find("option:contains('" + SubCatName + "')").each(function () {
            //        if ($(this).text() == SubCatName) {
            //            $(this).attr("selected", "selected");
            //        }
            //    });
            $("#cmb_product_inv").find("option:contains('" + InvName + "')").each(function () {
                if ($(this).text() == InvName) {
                    $(this).attr("selected", "selected");
                }
            });
            var rndmnum = Math.floor((Math.random() * 10) + 1);
            img_url = ftplocation + image + '?v=' + rndmnum;
            if (image != "") {
                $('#main_img').attr('src', img_url).width(200).height(200);
            }
            else {
                $('#main_img').attr('src', 'Images/Employeeimg.jpg').width(200).height(200);
            }
            document.getElementById('txt_products_prdtsname').value = ProductName;
            document.getElementById('txtProductCode').value = ProductCode;
            document.getElementById('txt_tally_prdtsname').value = TProductName;
            document.getElementById('txt_products_qty').value = Qty;
            document.getElementById('txtpieces').value = pieces;
            document.getElementById('cmb_products_qtymeasurement').value = ProductUnit;
            document.getElementById('txt_products_unitprice').value = UnitPrice;
            //document.getElementById('txt_vat_percent').value = VatPercent;
            document.getElementById('cmb_products_flag').value = status;
            document.getElementById('ddlifdflag').value = ifdflag;

            serial3 = sno;
            document.getElementById('btn_products_save').value = "MODIFY";
            document.getElementById('txtSpecifications').value = specification;
            document.getElementById('ddlmaterialtype').value = materialtype;
            document.getElementById('txtPerUnitPrice').value = perunitprice;
            document.getElementById('lblspecifiactions').innerHTML = specification;
            document.getElementById('lbl_topempname').innerHTML = ProductName;

            document.getElementById('txtHSNCode').value = hsncode;
            document.getElementById('txt_igst').value = igst;
            document.getElementById('txt_cgst').value = cgst;
            document.getElementById('txt_sgst').value = sgst;
            document.getElementById('slct_gsttaxcategory').value = gsttaxcategory;
            document.getElementById('txt_invqty').value = invqty;
            document.getElementById('txtDescription').value = description;
        }

        function prdtmgnt_productsvalidation() {
            var x = document.getElementById("cmb_products_category").value;
            if (x == "" || x == "Select") {
                alert("Please Select CategoryName");
                $("#cmb_products_category").focus();
                return false;
            }
            var y = document.getElementById("cmb_products_subcatgry").value;
            if (y == "" || y == "Select") {
                alert("Please Select SubCategoryName");
                $("#cmb_products_subcatgry").focus();
                return false;
            }
            var z = document.getElementById("txt_products_prdtsname").value;
            if (z == "") {
                alert("Please Provide ProductName");
                $("#txt_products_prdtsname").focus();
                return false;
            }

            var s = document.getElementById("txt_products_qty").value;
            if (s == "") {
                alert("Please Provide Quantity");
                $("#txt_products_qty").focus();
                return false;
            }
            var a = document.getElementById("cmb_products_qtymeasurement").value;
            if (a == "") {
                alert("Please Select Measurement");
                $("#cmb_products_qtymeasurement").focus();
                return false;
            }
            var a = document.getElementById("txt_products_unitprice").value;
            if (a == "") {
                alert("Please Provide UnitPrice");
                $("#txt_products_unitprice").focus();
                return false;
            }
            var n = document.getElementById("cmb_products_flag").value;
            if (n == "") {
                alert("Please Select Flag");
                $("#cmb_products_flag").focus();
                return false;
            }
            var cmb_product_inv = document.getElementById("cmb_product_inv").value;
            if (cmb_product_inv == "" || cmb_product_inv == "Select Inventory") {
                alert("Please Select Inventory");
                return false;
            }
            else {
                productunitsmanagement();
            }
        }
        var serial3 = 0;
        function productunitsmanagement() {
            var cmbproductcategory = document.getElementById('cmb_products_category').value;
            var productsubcategory = document.getElementById('cmb_products_subcatgry').value;
            var productname = document.getElementById('txt_products_prdtsname').value;
            var tally_productname = document.getElementById('txt_tally_prdtsname').value;
            var ProductCode = document.getElementById('txtProductCode').value;
            var productsqty = document.getElementById('txt_products_qty').value;

            var pieces = document.getElementById('txtpieces').value;

            var productsunits = document.getElementById('cmb_products_qtymeasurement').value;
            var productsunitprice = document.getElementById('txt_products_unitprice').value;
            var operationtype = document.getElementById('btn_products_save').value;
            var branchesproductsflag = document.getElementById('cmb_products_flag').value;
            var prdtinvsno = document.getElementById('cmb_product_inv').value;
            var invqty = document.getElementById('txt_invqty').value;


            var ifdflag = document.getElementById('ddlifdflag').value;

            var specification = document.getElementById('txtSpecifications').value;
            var materialtype = document.getElementById('ddlmaterialtype').value;
            var perunitprice = document.getElementById('txtPerUnitPrice').value;

            var HSNCode = document.getElementById('txtHSNCode').value;
            var igst = document.getElementById('txt_igst').value;
            var cgst = document.getElementById('txt_cgst').value;
            var sgst = document.getElementById('txt_sgst').value;
            var gsttaxcategory = document.getElementById('slct_gsttaxcategory').value;
            var description = document.getElementById('txtDescription').value;
            var FormType = "NewProductMaster";
            var sno = serial3;
            var data = { 'operation': 'productunitsmanagement', 'sno': sno, 'ProductCode': ProductCode, 'cmbproductcategory': cmbproductcategory, 'productsubcategory': productsubcategory, 'tproductname': tally_productname, 'productname': productname, 'productsqty': productsqty, 'productsunits': productsunits, 'productsunitprice': productsunitprice, 'operationtype': operationtype, 'branchesproductsflag': branchesproductsflag, 'prdtinvsno': prdtinvsno, 'specification': specification, 'materialtype': materialtype, 'perunitprice': perunitprice, 'HSNCode': HSNCode, 'igst': igst, 'cgst': cgst, 'sgst': sgst, 'gsttaxcategory': gsttaxcategory, 'pieces': pieces, 'invqty': invqty, 'description': description, 'ifdflag': ifdflag, 'FormType': FormType };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    productunitsclear();
                    Bindproductunits();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function productunitsclear() {
            document.getElementById('cmb_products_category').value = "";
            document.getElementById('cmb_products_subcatgry').value = "";
            document.getElementById('txt_products_prdtsname').value = "";
            document.getElementById('txt_tally_prdtsname').value = "";
            document.getElementById('txt_products_qty').value = "";
            document.getElementById('txtpieces').value = "";
            document.getElementById('cmb_products_qtymeasurement').value = "";
            document.getElementById('txt_products_unitprice').value = "";
            document.getElementById('txtProductCode').value = "";
            document.getElementById('cmb_products_flag').value = "";
            document.getElementById('ddlifdflag').value = "";
            document.getElementById('txtSpecifications').value = "";
            document.getElementById('ddlmaterialtype').value = "";
            document.getElementById('txtPerUnitPrice').value = "";
            document.getElementById('lblspecifiactions').innerHTML = "";
            document.getElementById('btn_products_save').value = "SAVE";
            document.getElementById('cmb_product_inv').value = "";
            document.getElementById('txt_invqty').value = "";
            document.getElementById('txtHSNCode').value = "";
            document.getElementById('txtDescription').value = "";
            document.getElementById('txt_igst').value = "";
            document.getElementById('txt_cgst').value = "";
            document.getElementById('txt_sgst').value = "";
            document.getElementById('slct_gsttaxcategory').value = "";
        }
        function products_cateegoryname_onchange() {
            var cmbcatgryname = document.getElementById("cmb_products_category").value;
            var buttonval = document.getElementById("btn_products_save").value;
            var FormType = "NewProductMaster";
            var data = { 'operation': 'get_subcategory_data', 'cmbcatgryname': cmbcatgryname, 'FormType': FormType };
            var s = function (msg) {
                if (msg) {
                    fillproducts_subcatgry(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        };
        function fillproducts_subcatgry(msg) {
            var prdtsubcategory = document.getElementById('cmb_products_subcatgry');
            var length = prdtsubcategory.options.length;
            document.getElementById("cmb_products_subcatgry").options.length = null;
            //    for (i = 0; i < length; i++) {
            //        prdtsubcategory.options[i] = null;
            //    }
            var opt = document.createElement('option');
            opt.innerHTML = "Select";
            prdtsubcategory.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].subcategorynames != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].subcategorynames;
                    opt.value = msg[i].sno;
                    prdtsubcategory.appendChild(opt);
                }
            }
        }
        //Address Details
//        function isNumber(evt) {
//            evt = (evt) ? evt : window.event;
//            var charCode = (evt.which) ? evt.which : evt.keyCode;
//            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
//                return false;
//            }
//            return true;
//        }

//        function ValidateAlpha(evt) {
//            var keyCode = (evt.which) ? evt.which : evt.keyCode
//            if ((keyCode < 65 || keyCode > 90) && (keyCode < 97 || keyCode > 123) && keyCode != 32)

//                return false;
//            return true;
//        }

//        function callHandler(d, s, e) {
//            $.ajax({
//                url: 'DairyFleet.axd',
//                data: d,
//                type: 'GET',
//                dataType: "json",
//                contentType: "application/json; charset=utf-8",
//                async: true,
//                cache: true,
//                success: s,
//                error: e
//            });
//        }
//        function cancelAddressdetails() {
//            $("#div_AddressData").show();
//            $("#Address_fillform").hide();
//            $('#Addresshowlogs').show();
//            Addressforclearall();
//        }
//        function saveAddressDetails() {
//            var companyname = document.getElementById('txtCompanyName').value;
//            if (companyname == "") {
//                alert("Enter  companyname");
//                $("#txtCompanyName").focus();
//                return false;
//            }

//            var buildingaddress = document.getElementById('txtBillingaddress').value;
//            var street = document.getElementById('txtStreet').value;
//            if (street == "") {
//                alert("Enter  Street Name");
//                $("#txtStreet").focus();
//                return false;
//            }
//            var mandal = document.getElementById('txtMandal').value;
//            var district = document.getElementById('txtdistrict').value;
//            if (district == "") {
//                alert("Enter  District Name");
//                $("#txtdistrict").focus();
//                return false;
//            }
//            var state = document.getElementById('txtState').value;
//            if (state == "") {
//                alert("Enter  state");
//                $("#txtState").focus();
//                return false;
//            }
//            var pin = document.getElementById('txtPincode').value;
//            var tin = document.getElementById('txtTinNumber').value;

//            var cst = document.getElementById('txtcst').value;
//            var email = document.getElementById('txtemail').value;
//            if (email == "") {
//                alert("Enter  email");
//                $("#txtemail").focus();
//                return false;
//            }
//            var panno = document.getElementById('txtpanno').value;
//            var customercode = document.getElementById('txtCustomerCode').value;
//            var sno = document.getElementById('lblAddress_sno').value;
//            var btnval = document.getElementById('btn_AddressSave').innerHTML;
//            var data = { 'operation': 'saveAddressDetails', 'customercode': customercode, 'panno': panno, 'cst': cst, 'email': email, 'companyname': companyname, 'buildingaddress': buildingaddress, 'street': street, 'mandal': mandal, 'district': district, 'state': state, 'pin': pin, 'tin': tin, 'sno': sno, 'btnVal': btnval };
//            var s = function (msg) {
//                if (msg) {
//                    if (msg.length > 0) {
//                        alert(msg);
//                        Addressforclearall();
//                        get_Address_details();
//                        $('#div_AddressData').show();
//                        $('#Address_fillform').css('display', 'none');
//                        $('#Addresshowlogs').css('display', 'block');
//                    }
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
//        }
//        function Addressforclearall() {
//            document.getElementById('txtCompanyName').value = "";
//            document.getElementById('txtBillingaddress').value = "";
//            document.getElementById('txtStreet').value = "";
//            document.getElementById('txtMandal').value = "";
//            document.getElementById('txtdistrict').value = "";
//            document.getElementById('txtState').value = "";
//            document.getElementById('txtPincode').value = "";
//            document.getElementById('txtTinNumber').value = "";
//            document.getElementById('txtcst').value = "";
//            document.getElementById('txtpanno').value = "";
//            document.getElementById('txtemail').value = "";
//            document.getElementById('txtCustomerCode').value = "";
//            document.getElementById('btn_AddressSave').innerHTML = "save";
//        }
//        function showAddressdesign() {
//            $("#div_AddressData").hide();
//            $("#Address_fillform").show();
//            $('#Addresshowlogs').hide();
//        }
//        function get_Address_details() {
//            var data = { 'operation': 'get_Address_details' };
//            var s = function (msg) {
//                if (msg) {
//                    if (msg.length > 0) {
//                        fillAddress(msg);
//                    }
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
//        }
//        function fillAddress(msg) {
//            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
//            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">CompanyName</th><th scope="col" class="thcls">BuildingAddress</th><th scope="col" class="thcls">TIN Number</th><th scope="col"></th></tr></thead></tbody>';
//            var k = 1;
//            var l = 0;
//            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
//            for (var i = 0; i < msg.length; i++) {
//                results += '<tr style="background-color:' + COLOR[l] + '">';
//                results += '<td scope="row" class="1 tdmaincls" >' + msg[i].companyname + '</td>';
//                results += '<td data-title="brandstatus"  class="2">' + msg[i].buildingaddress + '</td>';
//                results += '<td data-title="brandstatus" style="display:none" class="3">' + msg[i].street + '</td>';
//                results += '<td data-title="brandstatus"style="display:none" class="4">' + msg[i].mandal + '</td>';
//                results += '<td data-title="brandstatus" style="display:none" class="5">' + msg[i].district + '</td>';
//                results += '<td data-title="brandstatus" style="display:none"class="6">' + msg[i].state + '</td>';
//                results += '<td data-title="brandstatus" style="display:none" class="7">' + msg[i].pin + '</td>';
//                results += '<td data-title="brandstatus" class="8">' + msg[i].tin + '</td>';
//                results += '<td style="display:none" class="10">' + msg[i].cst + '</td>';
//                results += '<td style="display:none" class="11">' + msg[i].email + '</td>';
//                results += '<td style="display:none" class="12">' + msg[i].panno + '</td>';
//                results += '<td style="display:none" class="13">' + msg[i].customercode + '</td>';
//                results += '<td style="display:none" class="9">' + msg[i].sno + '</td>';
//                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmeaddress(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
//                l = l + 1;
//                if (l == 4) {
//                    l = 0;
//                }
//            }
//            results += '</table></div>';
//            $("#div_AddressData").html(results);
//        }
//        function getmeaddress(thisid) {
//            var companyname = $(thisid).parent().parent().children('.1').html();
//            var buildingaddress = $(thisid).parent().parent().children('.2').html();
//            var street = $(thisid).parent().parent().children('.3').html();
//            var mandal = $(thisid).parent().parent().children('.4').html();
//            var district = $(thisid).parent().parent().children('.5').html();
//            var state = $(thisid).parent().parent().children('.6').html();
//            var pin = $(thisid).parent().parent().children('.7').html();
//            var tin = $(thisid).parent().parent().children('.8').html();
//            var sno = $(thisid).parent().parent().children('.9').html();

//            var cst = $(thisid).parent().parent().children('.10').html();
//            var email = $(thisid).parent().parent().children('.11').html();
//            var panno = $(thisid).parent().parent().children('.12').html();
//            var customercode = $(thisid).parent().parent().children('.13').html();

//            document.getElementById('txtCompanyName').value = companyname;
//            document.getElementById('txtBillingaddress').value = buildingaddress;
//            document.getElementById('txtStreet').value = street;
//            document.getElementById('txtMandal').value = mandal;
//            document.getElementById('txtdistrict').value = district;
//            document.getElementById('txtState').value = state;
//            document.getElementById('txtPincode').value = pin;
//            document.getElementById('txtTinNumber').value = tin;
//            document.getElementById('txtcst').value = cst;
//            document.getElementById('txtemail').value = email;
//            document.getElementById('lblAddress_sno').value = sno;
//            document.getElementById('txtpanno').value = panno;
//            document.getElementById('txtCustomerCode').value = customercode;
//            document.getElementById('btn_AddressSave').innerHTML = "Modify";
//            $("#div_AddressData").hide();
//            $("#Address_fillform").show();
//            $('#Addresshowlogs').hide();
//        }
//        function departmentsave_click() {
//            var username = document.getElementById("txt_department_name").value;
//            if (username == "") {
//                alert("Please Provide Department Name");
//                $("#txt_department_name").focus();
//                return false;
//            }
//            else {
//                departmentsave();
//            }
//        }
//        function departmentsave() {
//            var txtdepartmentname = document.getElementById('txt_department_name').value;
//            var operationtype = document.getElementById('btn_dept_save').innerHTML;
//            var sno = serial;
//            var data = { 'operation': 'departmentsave', 'sno': sno, 'txtdepartmentname': txtdepartmentname, 'operationtype': operationtype };
//            var s = function (msg) {
//                if (msg) {
//                    alert(msg);
//                    department_clear();
//                    updatedepartment_manage();
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//                if (x.status && x.status == 400) {
//                    alert(x.responseText);
//                    window.location.assign("Login.aspx");
//                }
//                else {
//                    alert("something went wrong");
//                }
//            };
//            callHandler(data, s, e);
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//        }
//        function department_clear() {
//            document.getElementById('txt_department_name').value = "";
//            document.getElementById('btn_dept_save').innerHTML = "SAVE";
//            serial = 0;
//        }
//        function updatedepartment_manage() {
//            var data = { 'operation': 'updatedepartment_manage' };
//            var s = function (msg) {
//                if (msg) {
//                    BindGrid_department_manage(msg);
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//                if (x.status && x.status == 400) {
//                    alert(x.responseText);
//                    window.location.assign("Login.aspx");
//                }
//                else {
//                    alert("something went wrong");
//                }
//            };
//            callHandler(data, s, e);
//        }
//        var serial = 0;
//        function BindGrid_department_manage(msg) {
//            var l = 0;
//            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
//            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
//            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">Department Name</th><th scope="col"></th></tr></thead></tbody>';
//            for (var i = 0; i < msg.length; i++) {
//                results += '<tr style="background-color:' + COLOR[l] + '">';
//                results += '<td scope="row"  class="1 tdmaincls" >' + msg[i].DeptName + '</td>';
//                results += '<td style="display:none" class="2">' + msg[i].sno + '</td>';
//                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmeDepartment(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';

//                l = l + 1;
//                if (l == 4) {
//                    l = 0;
//                }
//            }
//            results += '</table></div>';
//            $("#div_Deptdata").html(results);
//        }
//        function getmeDepartment(thisid) {
//            var DeptName = $(thisid).parent().parent().children('.1').html();
//            var sno = $(thisid).parent().parent().children('.2').html();
//            document.getElementById('txt_department_name').value = DeptName;
//            document.getElementById('btn_dept_save').innerHTML = "MODIFY";
//            serial = sno;
//        }

//        //Product Ranking Details
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

//        function BindProducts(msg) {
//            document.getElementById('divselected').innerHTML = "";
//            for (var i = 0; i < msg.length; i++) {
//                var Selected = msg[i].ProdName;
//                var Selectedid = msg[i].ProdID;
//                var label = document.createElement("div");
//                var Crosslabel = document.createElement("img");
//                //                Crosslabel.style.float = "right";
//                //                Crosslabel.src = "Images/Cross.png";
//                //                Crosslabel.onclick = function () { RemoveClick(Selectedid); };
//                label.id = Selectedid;
//                label.innerHTML = Selected;
//                label.className = 'divselectedclass';
//                label.onclick = function () { divonclick(this); }
//                document.getElementById('divselected').appendChild(label);
//                //                label.appendChild(Crosslabel);
//            }
//        }
//        function divonclick(selected) {
//            selectedindex = selected;
//            if ($(selected).css('background-color') == 'rgb(255, 255, 255)' || $(selected).css('background-color') == 'rgba(0, 0, 0, 0)') {
//                $('.divselectedclass').each(function () {
//                    $(this).css('background-color', '#ffffff');
//                });
//                $(selected).css('background-color', '#ffffcc');
//            }
//            else {
//                $('.divselectedclass').each(function () {
//                    $(this).css('background-color', '#ffffff');
//                });
//            }
//        }
//        function btnUpClick() {
//            $(selectedindex).insertBefore($(selectedindex).prev());
//        }
//        function btnDownClick() {
//            $(selectedindex).insertAfter($(selectedindex).next());
//        }
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
//        function btnBranchProductsRankingclick() {
//            var div = document.getElementById('divselected');
//            var divs = div.getElementsByTagName('div');
//            var divArray = [];
//            for (var i = 0; i < divs.length; i += 1) {
//                divArray.push(divs[i].id);
//            }
//            var ddlsalesOffice = document.getElementById('ddlProductRankSalesOffice').value;
//            var Data = { 'operation': 'btnBranchProductsRankingclick', 'dataarr': divArray, 'BranchID': ddlsalesOffice };
//            var s = function (msg) {
//                if (msg) {
//                    alert(msg);
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            CallHandlerUsingJson(Data, s, e);
//        }
//        function FillSalesOffice() {
//            var data = { 'operation': 'GetPlantSalesOffice' };
//            var s = function (msg) {
//                if (msg) {
//                    if (msg == "Session Expired") {
//                        alert(msg);
//                        window.location = "Login.aspx";
//                    }
//                    BindSalesOffice(msg);
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
//        }
//        function BindSalesOffice(msg) {
//            var ddlProductRankSalesOffice = document.getElementById('ddlProductRankSalesOffice');
//            var length = ddlProductRankSalesOffice.options.length;
//            ddlProductRankSalesOffice.options.length = null;
//            var opt = document.createElement('option');
//            opt.innerHTML = "select";
//            ddlProductRankSalesOffice.appendChild(opt);
//            for (var i = 0; i < msg.length; i++) {
//                if (msg[i].BranchName != null) {
//                    var opt = document.createElement('option');
//                    opt.innerHTML = msg[i].BranchName;
//                    opt.value = msg[i].Sno;
//                    ddlProductRankSalesOffice.appendChild(opt);
//                }
//            }
//        }
//        //Bank Details
//        function canceldetailsbank() {
//            $("#div_BankData").show();
//            $("#Bank_fillform").hide();
//            $('#Bankshowlogs').show();
//            Bankforclearall();
//        }
//        function saveBankDetails() {
//            var name = document.getElementById('txtBName').value;
//            if (name == "") {
//                $("#txtBName").focus();
//                alert("Enter Bank Name ");
//                return false;

//            }
//            var code = document.getElementById('txtBcode').value;
//            if (code == "") {
//                $("#txtBcode").focus();
//                alert("Enter  BranchCode ");
//                return false;
//            }

//            var status = document.getElementById('ddlbankstatus').value;
//            if (status == "") {
//                $("#ddlbankstatus").focus();
//                alert("Select Status");
//            }

//            //var status = document.getElementById('ddlstatus').value;
//            var btnval = document.getElementById('btn_savebank').innerHTML;
//            var sno = document.getElementById('lbl_banksno').value;
//            var data = { 'operation': 'saveBankDetails', 'Name': name, 'Code': code, 'status': status, 'btnVal': btnval, 'sno': sno };
//            var s = function (msg) {
//                if (msg) {
//                    if (msg.length > 0) {
//                        alert(msg);
//                        get_bank_details();
//                        $('#div_BankData').show();
//                        $('#Bank_fillform').css('display', 'none');
//                        $('#Bankshowlogs').css('display', 'block');
//                        Bankforclearall();
//                    }
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };

//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
//        }
//        function Bankforclearall() {
//            document.getElementById('txtBcode').value = "";
//            document.getElementById('txtBName').value = "";
//            document.getElementById('lbl_banksno').value = "";
//            document.getElementById('ddlbankstatus').selectedIndex = 0;
//            document.getElementById('btn_savebank').innerHTML = "save";
//            $("#lbl_code_error_msg").hide();
//            $("#lbl_name_error_msg").hide();
//            get_bank_details();
//        }
//        function Bankshowdesign() {
//            // get_bank_details();
//            $("#div_BankData").hide();
//            $("#Bank_fillform").show();
//            $('#Bankshowlogs').hide();
//            Bankforclearall();
//        }
//        function get_bank_details() {
//            var data = { 'operation': 'get_bank_details' };
//            var s = function (msg) {
//                if (msg) {
//                    if (msg.length > 0) {
//                        fillbankdetails(msg);
//                    }
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
//        }
//        function fillbankdetails(msg) {
//            var k = 1;
//            var l = 0;
//            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
//            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer">';
//            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">Sno</th></th><th scope="col" class="thcls">Bank Name</th><th scope="col" class="thcls">Branch Code</th><th scope="col" class="thcls">Status</th><th scope="col"></th></tr></thead></tbody>';
//            for (var i = 0; i < msg.length; i++) {
//                var status = msg[i].status;
//                if (status == "0") {
//                    status = "Inactive";

//                }
//                else {
//                    status = "Active";
//                }
//                results += '<tr style="background-color:' + COLOR[l] + '">';
//                results += '<td>' + k++ + '</td>';
//                results += '<td scope="row"  class="1 tdmaincls">' + msg[i].name + '</td>';
//                results += '<td data-title="code" class="2">' + msg[i].code + '</td>';
//                results += '<td data-title="status" class="3">' + status + '</td>';
//                results += '<td style="display:none" class="4">' + msg[i].sno + '</td>';
//                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmeBank(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';

//                l = l + 1;
//                if (l == 4) {
//                    l = 0;
//                }
//            }
//            results += '</table></div>';
//            $("#div_BankData").html(results);
//        }
//        function getmeBank(thisid) {
//            var name = $(thisid).parent().parent().children('.1').html();
//            var Code = $(thisid).parent().parent().children('.2').html();
//            var sno = $(thisid).parent().parent().children('.4').html();
//            var statuscode = $(thisid).parent().parent().children('.3').html();
//            if (statuscode == "Active") {

//                status = "1";
//            }
//            else {
//                status = "0";
//            }
//            document.getElementById('txtBName').value = name;
//            document.getElementById('txtBcode').value = Code;

//            document.getElementById('lbl_banksno').value = sno;
//            document.getElementById('ddlbankstatus').value = status;
//            document.getElementById('btn_savebank').innerHTML = "Modify";
//            $("#div_BankData").hide();
//            $("#Bank_fillform").show();
//            $('#Bankshowlogs').hide();
//        }

//        //PO Entry Details
//        function PoFillSalesOffice() {
//            var data = { 'operation': 'GetPlantSalesOffice' };
//            var s = function (msg) {
//                if (msg) {
//                    if (msg == "Session Expired") {
//                        alert(msg);
//                        window.location = "Login.aspx";
//                    }
//                    POBindSalesOffice(msg);
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
//        }
//        function POBindSalesOffice(msg) {
//            var ddlposalesoffice = document.getElementById('ddlposalesoffice');
//            var length = ddlposalesoffice.options.length;
//            ddlposalesoffice.options.length = null;
//            var opt = document.createElement('option');
//            opt.innerHTML = "select";
//            ddlposalesoffice.appendChild(opt);
//            for (var i = 0; i < msg.length; i++) {
//                if (msg[i].BranchName != null) {
//                    var opt = document.createElement('option');
//                    opt.innerHTML = msg[i].BranchName;
//                    opt.value = msg[i].Sno;
//                    ddlposalesoffice.appendChild(opt);
//                }
//            }
//        }
//        function ddlSalesOfficeChange() {
//            var BranchID = document.getElementById('ddlposalesoffice').value;
//            var data = { 'operation': 'GetSalesRoutes', 'BranchID': BranchID };
//            var s = function (msg) {
//                if (msg) {
//                    if (msg == "Session Expired") {
//                        alert(msg);
//                        window.location = "Login.aspx";
//                    }
//                    BindRouteName(msg);
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
//        }
//        function BindRouteName(msg) {
//            document.getElementById('ddlRouteName').options.length = "";
//            var data = document.getElementById('ddlRouteName');
//            var length = data.options.length;
//            document.getElementById('ddlRouteName').options.length = null;
//            var opt = document.createElement('option');
//            opt.innerHTML = "Select Route Name";
//            opt.value = "";
//            data.appendChild(opt);
//            for (var i = 0; i < msg.length; i++) {
//                if (msg[i] != null) {
//                    var opt = document.createElement('option');
//                    opt.innerHTML = msg[i].RouteName;
//                    opt.value = msg[i].rid;
//                    data.appendChild(opt);
//                }
//            }
//        }
//        function ddlRouteNameChange() {
//            var RouteID = document.getElementById('ddlRouteName').value;
//            var data = { 'operation': 'GetAgents', 'RouteID': RouteID };
//            var s = function (msg) {
//                if (msg) {
//                    BindAgentName(msg);
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
//        }
//        function BindAgentName(msg) {
//            document.getElementById('ddlAgentName').options.length = "";
//            var data = document.getElementById('ddlAgentName');
//            var length = data.options.length;
//            document.getElementById('ddlAgentName').options.length = null;
//            var opt = document.createElement('option');
//            opt.innerHTML = "Select Agent Name";
//            opt.value = "";
//            data.appendChild(opt);
//            for (var i = 0; i < msg.length; i++) {
//                if (msg[i] != null) {
//                    var opt = document.createElement('option');
//                    opt.innerHTML = msg[i].BranchName;
//                    opt.value = msg[i].Sno;
//                    data.appendChild(opt);
//                }
//            }
//        }

//        function btnUpdateQuotationPoNumbers() {
//            var soid = document.getElementById('ddlposalesoffice').value;
//            if (soid == "" || soid == "select") {
//                alert("Please Select SalesOffice Name");
//                $("#ddlposalesoffice").focus();
//                return false;
//            }
//            var ddlRouteName = document.getElementById('ddlRouteName').value;
//            if (ddlRouteName == "" || ddlRouteName == "Select Route Name") {
//                alert("Please Select RouteName");
//                $("#datepicker").focus();
//                return false;
//            }
//            var ddlAgentName = document.getElementById('ddlAgentName').value;
//            if (ddlAgentName == "" || ddlRouteName == "Select Agent Name") {
//                alert("Please Select AgentName");
//                $("#datepicker").focus();
//                return false;
//            }
//            var indentdate = document.getElementById('datepicker').value;
//            if (indentdate == "") {
//                alert("Please Select IndentDate");
//                $("#datepicker").focus();
//                return false;
//            }
//            var pono = document.getElementById('txtPoNo').value;
//            if (pono == "") {
//                alert("Please Enter PONumber");
//                $("#txtPoNo").focus();
//                return false;
//            }
//            var quotano = document.getElementById('txtQuotationNo').value;
//            var grnno = document.getElementById('txtGrnNo').value;
//            var btn_save = document.getElementById("btnPOSave").innerHTML;
//            var data = { 'operation': 'btnUpdateQuotationPoNumbers', 'BranchID': ddlAgentName, 'quotano': quotano, 'indentdate': indentdate, 'pono': pono, 'grnno': grnno, 'soid': soid, 'btn_save': btn_save };
//            var s = function (msg) {
//                if (msg) {
//                    alert(msg);
//                    POforclearall();
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
//        }
//        function POforclearall() {
//            document.getElementById('ddlAgentName').selectedIndex = "";
//            document.getElementById('datepicker').value = "";
//            document.getElementById('txtQuotationNo').value = "";
//            document.getElementById('txtPoNo').value = "";
//            document.getElementById('txtGrnNo').value = "";
//            document.getElementById('ddlposalesoffice').selectedIndex = "";
//            document.getElementById('ddlRouteName').selectedIndex = "";
//            document.getElementById('btnPOSave').innerHTML = "save";
//        }

//        //CASH COLLECTIONS
//        function Cancel_Others_Details() {
//            $("#div_Others").show();
//            $("#Others_fillform").hide();
//            $('#CashCollectionsowlogs').show();
//            CashOthersforclearall();
//        }
//        function Svae_Others_Details() {
//            var ledgename = document.getElementById('txtLedgerName').value;
//            if (ledgename == "") {
//                alert("Enter LedgeName");
//                $("#txtLedgerName").focus();
//                return false;
//            }
//            var ledgercode = document.getElementById('txtLedgerCode').value;
//            if (ledgercode == "") {
//                alert("Enter LedgerCode");
//                $("#txtLedgerCode").focus();
//                return false;
//            }

//            var sno = document.getElementById('lbl_OtherCashSno').value;
//            var btnval = document.getElementById('btnCashOthersSave').innerHTML;

//            var data = { 'operation': 'Svae_Others_Details', 'ledgercode': ledgercode, 'ledgename': ledgename, 'sno': sno, 'btnVal': btnval };
//            var s = function (msg) {
//                if (msg) {
//                    if (msg.length > 0) {
//                        alert(msg);
//                        CashOthersforclearall();
//                        get_Others_Details();
//                        $('#div_Others').show();
//                        $('#Others_fillform').css('display', 'none');
//                        $('#CashCollectionsowlogs').css('display', 'block');
//                    }
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
//        }
//        function CashOthersforclearall() {
//            document.getElementById('txtLedgerCode').value = "";
//            document.getElementById('txtLedgerName').value = "";
//            document.getElementById('btnCashOthersSave').innerHTML = "save";
//        }
//        function showCashCollectiondesign() {
//            $("#div_Others").hide();
//            $("#Others_fillform").show();
//            $('#CashCollectionsowlogs').hide();
//        }
//        function get_Others_Details() {
//            var data = { 'operation': 'get_Others_Details' };
//            var s = function (msg) {
//                if (msg) {
//                    if (msg.length > 0) {
//                        FillOthers(msg);
//                    }
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
//        }
//        function FillOthers(msg) {
//            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
//            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">LedgerName</th><th scope="col" class="thcls">LedgerCode</th><th scope="col"></th></tr></thead></tbody>';
//            var k = 1;
//            var l = 0;
//            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
//            for (var i = 0; i < msg.length; i++) {
//                results += '<tr style="background-color:' + COLOR[l] + '">';
//                results += '<td scope="row"  class="1 tdmaincls" >' + msg[i].name + '</td>';
//                results += '<td data-title="brandstatus"  class="2">' + msg[i].ledgercode + '</td>';
//                results += '<td style="display:none" class="3">' + msg[i].sno + '</td>';
//                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmeCashOthers(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';

//                l = l + 1;
//                if (l == 4) {
//                    l = 0;
//                }
//            }
//            results += '</table></div>';
//            $("#div_Others").html(results);
//        }
//        function getmeCashOthers(thisid) {
//            var ledgername = $(thisid).parent().parent().children('.1').html();
//            var ledgercode = $(thisid).parent().parent().children('.2').html();
//            var sno = $(thisid).parent().parent().children('.3').html();
//            document.getElementById('txtLedgerCode').value = ledgercode;
//            document.getElementById('lbl_OtherCashSno').value = sno;
//            document.getElementById('txtLedgerName').value = ledgername;
//            document.getElementById('btnCashOthersSave').innerHTML = "Modify";
//            $("#div_Others").hide();
//            $("#Others_fillform").show();
//            $('#CashCollectionsowlogs').hide();
//        }

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
    <section class="content">
        <!-- Small boxes (Stat box) -->
        <div class="row">
            <section class="content-header">
                <h1>
                    Mini Masters
                </h1>
                <ol class="breadcrumb">
                    <li><a href="#"><i class="fa fa-dashboard"></i>Operation</a></li>
                    <li><a href="#">Masters</a></li>
                </ol>
            </section>
            <section class="content">
                <div class="box box-info">
                    <div class="box-header with-border">
                    </div>
                    <div class="box-body">
                        <div>
                            <ul class="nav nav-tabs">
                               <%-- <li id="id_tab_Address" class="active"><a data-toggle="tab" href="#" onclick="showAddress()">
                                    <i class="fa fa-user" aria-hidden="true"></i>&nbsp;&nbsp;Address</a></li>--%>
                                <li id="id_tab_Department" class=""><a data-toggle="tab" href="#" onclick="showDepartment()">
                                    <i class="fa fa-building-o" aria-hidden="true"></i>&nbsp;&nbsp;Category</a></li>
                                <li id="id_tab_ProductRanking" class=""><a data-toggle="tab" href="#" onclick="showProductRanking()">
                                    <i class=""></i>&nbsp;&nbsp;SubCategory</a></li>
                                <%--<li id="id_tab_BankDetails" class=""><a data-toggle="tab" href="#" onclick="showbankmaster()">
                                    <i class="fa fa-university"></i>&nbsp;&nbsp;BankDetails</a></li>--%>
                                <li id="id_tab_PoDetails" class=""><a data-toggle="tab" href="#" onclick="showPOEntryDetails()">
                                    <i class="fa fa-file-text-o"></i>&nbsp;&nbsp;ProductMaster</a></li>
                               <%-- <li id="id_tab_CashCollections" class=""><a data-toggle="tab" href="#" onclick="showCashCollectionMaster()">
                                    <i class="icon-money"></i>&nbsp;&nbsp;CashCollections</a></li>--%>
                            </ul>
                        </div>
                        <div id="div_Address" style="display: none;">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Address Details
                                </h3>
                            </div>
                            <div class="box-body">
                                <%--<div id="Addresshowlogs" align="center">
                                        <input id="btn_addAddress" type="button" name="submit" value='AddAddress' class="btn btn-primary"
                                            onclick="showAddressdesign()" />
                                    </div>--%>
                                <div id="Addresshowlogs" align="right" class="input-group" style="display: block;padding-bottom: 20px;">
                                    <div class="input-group-addon" style="width: 100px;">
                                        <span class="glyphicon glyphicon-plus-sign" onclick="showAddressdesign();"></span>
                                        <span onclick="showAddressdesign();">AddAddress</span>
                                    </div>
                                </div>
                                <div id="div_AddressData">
                                </div>
                                <div id='Address_fillform' style="display: none;">
                                    <table align="center">
                                        <tr>
                                            <td>
                                                <label>
                                                    CompanyName</label><span style="color: red; font-weight: bold">*</span>
                                                <input id="txtCompanyName" type="text" name="CompanyName" style="text-transform: capitalize;" class="form-control" placeholder="Enter CompanyName" />
                                            </td>
                                            <td style="width: 4px;">
                                            </td>
                                            <td>
                                                <label>
                                                    Customer Code</label>
                                                <input id="txtCustomerCode" type="text" name="CustomerCode" class="form-control"
                                                    placeholder="Enter Customer Code" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Building Address</label><span style="color: red; font-weight: bold">*</span>
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
                                                                    <span class="glyphicon glyphicon-ok" id="btn_AddressSave1" onclick="saveAddressDetails()">
                                                                    </span><span id="btn_AddressSave" onclick="saveAddressDetails()">save</span>
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
                        <div id="div_Department" style="display: none;">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Category Details
                                </h3>
                            </div>
                            <div class="box-body">
                                <table align="center">
                                <tr>
                                    <td>
                                        <label for="lblProductName" style="font-weight:bold;">
                                            Category Name:</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="text" id="txt_productname" class="form-control" placeholder="Enter Category Name"/>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <label for="lblProductName" style="font-weight:bold;">
                                            Tally Category Name:</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="text" id="txttcategoryName" class="form-control" placeholder="Enter Tally Category Name" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <label for="lblProductName"  style="font-weight:bold;">
                                            Category Code:</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="text" id="txtCategoryCode" class="form-control"  placeholder="Enter Category Code"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="lblFlag" style="font-weight:bold;">
                                            Flag:</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="cmb_prdmngflag" class="form-control">
                                            <option>Active</option>
                                            <option>InActive</option>
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <input type="button" id="btn_productmanagesave" value="SAVE" onclick="prdtmngt_catgryValidation();"
                                            class="btn btn-primary" />
                                        <input type="button" id="btn_productmanageclear" value="CLEAR" class="btn btn-danger"
                                            onclick="producttypeclear();" />
                                    </td>
                                </tr>
                            </table>
                                <br />
                            </div>
                            <div class="box box-info">
                                <div class="box-header with-border">
                                    <h3 class="box-title">
                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Category list
                                    </h3>
                                </div>
                                <div id="div_categorydata">
                                </div>
                            </div>
                        </div>
                        <div id="div_ProductRanking" style="display: none;">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>SubCategory Details
                                </h3>
                            </div>
                            <div class="box-body">
                               <table align="center" class="">
                                <tr>
                                    <td>
                                        <label for="lbl_catgry_subname"  style="font-weight:bold">
                                            Sub Category Name:</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="text" id="txt_catgry_subname" class="form-control" placeholder="Sub Category Name"></input>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="lbl_catgry_name"  style="font-weight:bold">
                                            Category Name:</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="cmb_catgry_catgryname" class="form-control">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                <td>
                                        <label for="lbl_products_qty"  style="font-weight:bold">
                                            Description:</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="text" id="txtSubCategoryDescription" class="form-control" placeholder="Enter Description" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="lbl_catgry_Flag"  style="font-weight:bold">
                                            Flag:</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="cmb_catgry_flag" class="form-control">
                                            <option>Active</option>
                                            <option>InActive</option>
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <input type="button" id="btn_subcatgry_save" value="SAVE" onclick="prdtmngt_subcatgryvalidation();"
                                            class="btn btn-primary" />
                                        <input type="button" id="btn_subcatgry_clear" value="CLEAR" class="btn btn-danger"
                                            onclick="subcategoryclear();" />
                                    </td>
                                </tr>
                            </table>
                                <br />
                            </div>

                                 <div class="box box-info">
                                <div class="box-header with-border">
                                    <h3 class="box-title">
                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>SubCategory list
                                    </h3>
                                </div>
                                <div id="div_sub_categorydata">
                                </div>
                            </div>
                        </div>
                        <div id="div_Bankdetails" style="display: none;">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>BankDetails
                                </h3>
                            </div>
                            <div class="box-body">
                                <%--<div id="Bankshowlogs" align="center">
                                    <input id="btn_addbank" type="button" name="submit" value='AddBank' class="btn btn-primary"
                                        onclick="Bankshowdesign()" />
                                </div>--%>
                                <div id="Bankshowlogs" align="right" class="input-group" style="display: block;padding-bottom: 20px;">
                                    <div class="input-group-addon" style="width: 100px;">
                                        <span class="glyphicon glyphicon-plus-sign" onclick="Bankshowdesign();"></span><span
                                            onclick="Bankshowdesign();">Add Bank</span>
                                    </div>
                                </div>
                                <div id='Bank_fillform' style="display: none;">
                                    <table align="center" style="width: 60%;">
                                        <tr>
                                            <th>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td style="height: 40px;">
                                                <label class="control-label" style="color: #555;">
                                                    Bank Name</label><span style="color: red; font-weight: bold">*</span>
                                            </td>
                                            <td>
                                                <input id="txtBName" type="text" maxlength="45" class="form-control" style="text-transform: capitalize;" name="vendorcode"
                                                    placeholder="Enter Bank Name" onkeypress="return ValidateAlpha(event);" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="control-label" style="color: #555;">
                                                    Branch Code</label>
                                            </td>
                                            <td>
                                                <input id="txtBcode" type="text" maxlength="45" class="form-control" name="vendorcode"
                                                    placeholder="Enter Branch Code" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 40px;">
                                                <label class="control-label" style="color: #555;">
                                                    Status</label><span style="color: red; font-weight: bold">*</span>
                                            </td>
                                            <td>
                                                <select id="ddlbankstatus" class="form-control">
                                                    <option value="1">Active</option>
                                                    <option value="0">InActive</option>
                                                </select>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <label id="lbl_banksno">
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
                                                                    <span class="glyphicon glyphicon-ok" id="btn_savebank1" onclick="saveBankDetails()">
                                                                    </span><span id="btn_savebank" onclick="saveBankDetails()">save</span>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td style="padding-left: 7px;">
                                                            <div class="input-group">
                                                                <div class="input-group-close">
                                                                    <span class="glyphicon glyphicon-remove" id="btn_closebank1" onclick="canceldetailsbank()">
                                                                    </span><span id="btn_closebank" onclick="canceldetailsbank()">Close</span>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="div_BankData">
                                </div>
                            </div>
                        </div>
                        <div id="div_PoEntry" style="display: none;">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Products Details
                                </h3>
                            </div>
                            <div class="box-body">
                                 <table  align="center">
                                <tr>
                                    <td>
                                        <label for="lbl_products_category"  style="font-weight:bold">
                                            Category:</label><span style="color: red; font-weight: bold">*</span>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="cmb_products_category" class="form-control" onchange="return products_cateegoryname_onchange();">
                                        </select>
                                    </td>
                                    <td>
                                        <label for="lbl_products_subcategory"  style="font-weight:bold">
                                            Sub Category:</label><span style="color: red; font-weight: bold">*</span>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="cmb_products_subcatgry" class="form-control">
                                       <option disabled selected>Select</option>
                                        </select>
                                    </td>
                                </tr>
                                   <tr>
                                    <td>
                                        <label for="lbl_products_productname"  style="font-weight:bold">
                                            Product Code:</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="text" id="txtProductCode" class="form-control" placeholder="Enter Product Code"></input>
                                    </td>
                                    <td>
                                        <label for="lbl_products_productname"  style="font-weight:bold">
                                            Product Name:</label><span style="color: red; font-weight: bold">*</span>
                                    </td>
                                    <td style="height: 40px;" >
                                        <input type="text" id="txt_products_prdtsname" class="form-control" placeholder="Enter Product Name"></input>
                                    </td>
                                </tr>
                                  <tr>
                                    <td>
                                        <label for="lbl_products_productname"  style="font-weight:bold">
                                           Tally Name:</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="text" id="txt_tally_prdtsname" class="form-control" placeholder="Enter Tally Product Name"></input>
                                    </td>
                                    <td>
                                        <label for="lbl_products_qty"  style="font-weight:bold">
                                            Description:</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="text" id="txtDescription" class="form-control" placeholder="Enter Description" />
                                    </td>
                                    
                                    </tr>   
                                    <tr>
                                    <td>
                                        <label for="lbl_products_qty"  style="font-weight:bold">
                                            Qty:</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="number" id="txt_products_qty" class="form-control" placeholder="Enter Quantity" onkeypress="return isNumber(event);" />
                                    </td>
                                    <td >
                                    <label for="lbl_products_qty"  style="font-weight:bold">
                                            Pieces:</label>
                                    </td>
                                    <td >
                                        <input type="number" id="txtpieces" class="form-control" placeholder="Enter Pieces"/>
                                    </td>
                                </tr>
                                <tr>
                                <td>
                                        <label for="lbl_products_unitprice"  style="font-weight:bold">
                                            Unit Price:</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="number" id="txt_products_unitprice" class="form-control" placeholder="Enter Price"/>
                                    </td>

                                    <td>
                                        <label for="lbl_products_Flag"  style="font-weight:bold">
                                            PerUnitPrice:</label>
                                    </td>
                                    <td style="height: 40px;">
                                    <input type="text" id="txtPerUnitPrice" class="form-control" placeholder="Enter PerUnitPrice" />
                                        <%--<select id="ddlmasterpack" class="form-control">
                                            <option value="Yes">Yes</option>
                                            <option value="No">No</option>
                                        </select>--%>
                                    </td>



                                
                                    
                                    <td style="display:none;">
                                        <label for="lbl_products_vat"  style="font-weight:bold">
                                            Vat Percent:</label>
                                    </td>
                                    <td style="height: 40px;display:none;">
                                        <input type="text" id="txt_vat_percent" class="form-control" placeholder="Enter Vat Percent" />
                                    </td>
                                    
                                </tr>
                                <tr>
                                <td>
                                    <label for="lbl_products_qty"  style="font-weight:bold">
                                            UOM:</label>
                                    </td>
                                    <td >
                                        <select id="cmb_products_qtymeasurement" class="form-control">
                                          <option>ml</option>
                                            <option>kgs</option>
                                            <option>ltr</option>
                                            <option>gms</option>
                                            <option>Nos</option>
                                            <option>Packets</option>
                                            <option>Pouches</option>
                                            <option>Set</option>
                                            <option>Box</option>
                                            <option>Bags</option>
                                            <option>Kits</option>
                                        </select>
                                    </td>
                                <td>
                                        <label for="lbl_products_Flag"  style="font-weight:bold">
                                            Flag:</label><span style="color: red; font-weight: bold">*</span>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="cmb_products_flag" class="form-control">
                                            <option>Active</option>
                                            <option>InActive</option>
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                 <td>
                                        <label id="lbl_prdt_inv"  style="font-weight:bold">
                                            Inventory:</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="cmb_product_inv" class="form-control">
                                        </select>
                                    </td>
                                <td>
                                        <label id="lbl_inv_qty"  style="font-weight:bold">
                                            Inventory Qty:</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="text" id="txt_invqty" class="form-control" placeholder="Enter InventoryQty" />
                                    </td>
                                    </tr>
                                    <tr>
                                    <td>
                                        <label for="lbl_products_Flag"  style="font-weight:bold">
                                            Material Type:</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="ddlmaterialtype" class="form-control">
                                            <option value="R">Returnable</option>
                                            <option value="D">Disposable</option>
                                        </select>
                                    </td>
                                   <td>
                                        <label for="lbl_hsncode"  style="font-weight:bold">
                                            HSN Code:</label>
                                    <td style="height: 40px;">
                                        <input type="text" id="txtHSNCode" class="form-control" placeholder="Enter HSNCode" />
                                    </td>
                                    </tr>
                                    <tr>
                                     <td>
                                        <label for="lbl_igst"  style="font-weight:bold">
                                            IGST(%):</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="text" id="txt_igst" class="form-control" placeholder="Enter IGST" onchange="igstchange();" />
                                    </td>
                                    <td>
                                        <label for="lbl_cgst"  style="font-weight:bold">
                                            CGST(%):</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="text" id="txt_cgst" readonly class="form-control" placeholder="Enter CGST" />
                                    </td>
                                    </tr>
                                    <tr>
                                    <td>
                                        <label for="lbl_sgst"  style="font-weight:bold">
                                            SGST(%):</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="text" id="txt_sgst" readonly class="form-control" placeholder="Enter SGST" />
                                    </td>
                                    <td>
                                        <label for="lbl_cgst"  style="font-weight:bold">
                                            GST Tax Category:</label>
                                    </td>
                                    <td style="height: 40px;">
                                         <select id="slct_gsttaxcategory" class="form-control">
                                            <option value="Regular">Regular</option>
                                            <option value="Nil Rated">Nil Rated</option>
                                            <option value="Exempt">Exempt</option>
                                        </select>
                                    </td>
                                    </tr>
                                    <tr>
                                    <td style="height: 40px;">
                                    <label class="control-label" for="txt_empname"  style="font-weight:bold">
                                                    Specifications</label>
                                                   </td>
                                 <td style="height: 40px;">
                                <textarea class="form-control" id="txtSpecifications" rows="3" placeholder="Enter Product Specifications" cols="40"></textarea>
                                    </td>
                                    <td>
                                        <label for="lbl_ifdproducts_Flag"  style="font-weight:bold">
                                            IfdFlag:</label><span style="color: red; font-weight: bold">*</span>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="ddlifdflag" class="form-control">
                                            <option>InActive</option> 
                                            <option>Active</option>
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <input type="button" id="btn_products_save" value="SAVE" onclick="prdtmgnt_productsvalidation();"
                                            class="btn btn-primary" />
                                        <input type="button" id="btn_products_clear" value="CLEAR" class="btn btn-danger"
                                            onclick="productunitsclear();" />
                                    </td>
                                </tr>
                            </table>
                            </div>
                            <div class="box box-info">
                                <div class="box-header with-border">
                                    <h3 class="box-title">
                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>ProductList
                                    </h3>
                                </div>
                                <div id="div_ProductsData">
                                </div>
                            </div>
                        </div>
                        <div id="div_CashCollection" style="display: none">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Others Details
                                </h3>
                            </div>
                            <div class="box-body">
                                <div id="CashCollectionsowlogs" align="right" class="input-group" style="display: block;padding-bottom: 20px;">
                                    <div class="input-group-addon" style="width: 100px;">
                                        <span class="glyphicon glyphicon-plus-sign" onclick="showCashCollectiondesign();">
                                        </span><span onclick="showCashCollectiondesign();">Add Others</span>
                                    </div>
                                </div>
                                <div id="div_Others">
                                </div>
                                <div id='Others_fillform' style="display: none;">
                                    <table align="center">
                                        <tr>
                                            <td>
                                                <label>
                                                    Ledger Name</label><span style="color: red; font-weight: bold">*</span>
                                            </td>
                                            <td>
                                                <input id="txtLedgerName" type="text" name="LedgerName" class="form-control" placeholder="Enter LedgerName" />
                                            </td>
                                        </tr>
                                        <tr>
                                        <td style="height:5px;">
                                        </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Ledger Code</label><span style="color: red; font-weight: bold">*</span>
                                            </td>
                                            <td>
                                                <input id="txtLedgerCode" type="text" name="LedgerCode" class="form-control" placeholder="Enter LedgerCode" />
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <label id="lbl_OtherCashSno">
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
                                                                    <span class="glyphicon glyphicon-ok" id="btnCashOthersSave1" onclick="Svae_Others_Details()">
                                                                    </span><span id="btnCashOthersSave" onclick="Svae_Others_Details()">save</span>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td style="padding-left: 7px;">
                                                            <div class="input-group">
                                                                <div class="input-group-close">
                                                                    <span class="glyphicon glyphicon-remove" id='btnCashOthersCancel1' onclick="Cancel_Others_Details()">
                                                                    </span><span id='btnCashOthersCancel' onclick="Cancel_Others_Details()">Close</span>
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
                    </div>
                </div>
        </div>
    </section>
</asp:Content>
