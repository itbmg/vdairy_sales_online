<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Product_Master.aspx.cs" Inherits="ProductMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Plant/Script/fleetscript.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script src="jquery-ui-1.10.3.custom/js/jquery-ui.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script src="jquery-ui-1.10.3.custom/js/jquery-ui.js" type="text/javascript"></script>
    <script src="js/newjs/jquery-ui.js?v=3004" type="text/javascript"></script>
    <script src="Kendo/kendo.all.min.js" type="text/javascript"></script>
    <link href="Kendo/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="Kendo/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .ui-widget
        {
            font-family: arial;
            font-size: 12px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#categorytabs").tabs();
            updatecategorytype();
        });
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
        function callHandler_nojson_post(d, s, e) {
            $.ajax({
                url: 'DairyFleet.axd',
                type: "POST",
                // dataType: "json",
                contentType: false,
                processData: false,
                data: d,
                success: s,
                error: e
            });
        }

        function hasExtension(fileName, exts) {
            return (new RegExp('(' + exts.join('|').replace(/\./g, '\\.') + ')$')).test(fileName);
        }

        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#main_img,#img_1').attr('src', e.target.result).width(200).height(200);
                    //                    $('#img_1').css('display', 'inline');
                };
                reader.readAsDataURL(input.files[0]);
            }
        }

        function getFile() {
            document.getElementById("file").click();
        }
        //----------------> convert base 64 to file
        function dataURItoBlob(dataURI) {
            // convert base64/URLEncoded data component to raw binary data held in a string
            var byteString;
            if (dataURI.split(',')[0].indexOf('base64') >= 0)
                byteString = atob(dataURI.split(',')[1]);
            else
                byteString = unescape(dataURI.split(',')[1]);
            // separate out the mime component
            var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];
            // write the bytes of the string to a typed array
            var ia = new Uint8Array(byteString.length);
            for (var i = 0; i < byteString.length; i++) {
                ia[i] = byteString.charCodeAt(i);
            }
            return new Blob([ia], { type: 'image/jpeg' });
        }
        function upload_profile_pics() {
            var dataURL = document.getElementById('main_img').src;
            var div_text = $('#yourBtn').text().trim();
            var blob = dataURItoBlob(dataURL);
            var sno = serial3;
            var Data = new FormData();
            Data.append("operation", "Product_Image_files_upload");
            Data.append("sno", sno);
            Data.append("blob", blob);
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler_nojson_post(Data, s, e);
        }
        function igstchange() {
            var igst = document.getElementById('txt_igst').value;
            var percent = igst / 2;
            document.getElementById('txt_cgst').value = percent;
            document.getElementById('txt_sgst').value = percent;
        }
    </script>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Product Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Masters</a></li>
            <li><a href="#">Product Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Product Master Details
                </h3>
            </div>
            <div class="box-body">
                <div id="categorymanagement">
                    <div id="categorytabs">
                        <ul style="font-size: 12px">
                            <li><a href="#divproductsManage">Category</a></li>
                            <li><a href="#divsubcategory" onclick=" return product_manages_subcatgry();">Sub Category</a></li>
                            <li><a href="#divcategory_products" onclick=" return product_manages_products();">Products</a></li>
                        </ul>
                        <div id="divproductsManage">
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
                              </br>
                            </br>
                            <div class="box box-primary">
                             <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-list"></i>Category list
                </h3>
            </div>
                            <div id="div_categorydata" style="width:100%;height:350px;overflow:auto;"></div>
                            </div>
                        </div>
                        <div id="divsubcategory">
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
                             
                                </br>
                            </br>
                            <div class="box box-primary">
                             <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-list"></i>SubCategory list
                </h3>
            </div>
                           <div id="div_sub_categorydata" style="width:100%;height:350px;overflow:auto;"></div>
                            </div>
                             
                        </div>
                        <div id="divcategory_products">
                        <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="well panel panel-default" style="padding: 0px;">
                                <div class="panel-body" style="margin-rihght:-15px;margin-rihght:-15px;background-color:gainsboro;">
                                    <div class="row">
                                        <div class="col-sm-4" style="width: 100%;">
                                            <div class="row" >
                                                <div class="col-xs-12 col-sm-3 text-center">
                                                    <div class="pictureArea1">
                                                        <img class="center-block img-circle img-thumbnail img-responsive profile-img" id="main_img"
                                                            alt="Product image" src="Images/Employeeimg.jpg"  style="border-radius: 5px; width: 200px; height: 200px; border-radius: 50%;" />
                                                        <%--<img id="prw_img" class="center-block img-circle img-thumbnail img-responsive profile-img" src="Images/Employeeimg.jpg" alt="your image" style="width: 150px; height: 150px;">--%>
                                                        <div class="photo-edit-admin">
                                                            <a onclick="getFile();" class="photo-edit-icon-admin" title="Change Profile Picture"
                                                                data-toggle="modal" data-target="#photoup"><i class="fa fa-pencil"></i></a>
                                                        </div>
                                                        <div id="yourBtn" class="img_btn" onclick="getFile();" style="margin-top: 5px; display: none;">
                                                            Click to Choose Image
                                                        </div>
                                                        <div style="height: 0px; width: 0px; overflow: hidden;">
                                                            <input id="file" type="file" name="files[]" onchange="readURL(this);">
                                                        </div>
                                                        <div>
                                                            <input type="button" id="btn_upload_profilepic" class="btn btn-primary" onclick="upload_profile_pics();"
                                                                style="margin-top: 5px;" value="Upload Profile Pic">
                                                        </div>
                                                    </div>
                                                </div>
                                                 <div class="col-xs-12 col-sm-9">
                                                  <h2 class="text-primary">
                                                                <b><span class="fa fa-product-hunt"></span>
                                                                    <label id="lbl_topempname">
                                                                    </label>
                                                                </b>
                                                            </h2>
                                                            <p>
                                                                <strong  style="font-weight:bold">Specifications: <span style="color: Red;">*</span></strong>
                                                                <label style="padding-left: 20px; font-weight: 700;" id="lblspecifiactions">
                                                                </label>
                                                            </p>
                                                        </div>
                                                <!--/col-->
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
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
                                            <option>Pkts</option>
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
                            </br>
                            </br>
                            <div class="box box-primary">
                             <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-list"></i>Product list
                </h3>
            </div>
                           <div id="div_ProductsData" style="width:100%;height:350px;overflow:auto;"></div>
                            <table id="table_products" style="padding-left: 5%;">
                                <tr>
                                    <td>
                                        <div id="div_products1" style="padding-left: 15%; width: 90%; cursor: pointer;">
                                            <table id="grd_productslist">
                                            </table>
                                            <div id="page3">
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
        </div>
    </section>
</asp:Content>
