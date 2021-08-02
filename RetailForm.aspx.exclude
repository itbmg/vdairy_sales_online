<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RetailForm.aspx.cs" Inherits="RetailForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script src="js/jquery-1.4.4.js" type="text/javascript"></script>--%>
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
            $('#btn_addOutlet').click(function () {
                $('#Retail_FillForm').css('display', 'block');
                $('#showlogs').css('display', 'none');
                $('#div_RetailData').hide();
                var today = new Date();
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!
                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd
                }
                if (mm < 10) {
                    mm = '0' + mm
                }
                var hrs = today.getHours();
                var mnts = today.getMinutes();
                GetFixedrows();
            });
            $('#close_Retail_Form').click(function () {
                $('#Retail_FillForm').css('display', 'none');
                $('#showlogs').css('display', 'block');
                $('#div_RetailData').show();
                forclearall();

            });
            get_Retail_sub_details();
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd
            }
            if (mm < 10) {
                mm = '0' + mm
            }
            var hrs = today.getHours();
            var mnts = today.getMinutes();

        });

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
        function upload_profile_pic() {
            var dataURL = document.getElementById('main_img').src;
            var div_text = $('#yourBtn').text().trim();
            var blob = dataURItoBlob(dataURL);
            var sno = document.getElementById('lblsno').value;
            //            var empsno = 1;
            var Data = new FormData();
            Data.append("operation", "Retailer_profile_pic_files_upload");
            Data.append("sno", sno);
            Data.append("blob", blob);
            var s = function (msg) {
                if (msg) {
                    alert(msg);
//                    document.getElementById('btn_upload_profilepic').disabled = true;
                }
                else {
                    //                    document.location = "Default.aspx";
                }
            };
            var e = function (x, h, e) {
            };
            callHandler_nojson_post(Data, s, e);
        }

        function GetFixedrows() {
            var Silonames = "BATTER,CHAPATI,PAROTA,CURD,PANNER,BUTTER,CHEESE,CHIKIES,BUTTER MILK,LASSI,FLAVOURD MILK,YOGHURT,PCH,UHT,ICE CREAM";
            var names = Silonames.split(',');
            var results = '<div  style="overflow:auto;"><table id="tbl_Retail_Product_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">ProductName</th><th scope="col">ProductStatus</th><th scope="col">Top Brands</th><th scope="col">Avg Sales/PerDay</th><th scope="col">Vyshnavi(Y/N)</th><th scope="col">SupportRequired</th></tr></thead></tbody>';
            var j = 1;
            for (var i = 0; i < 15; i++) {
                results += '<td data-title="Sno" class="1">' + j++ + '</td>';
                results += '<th scope="row" ><input class="clsProductName" disabled="disabled" type="text"  id="txt_ProductName" placeholder="ProductName" name="ProductName" value="' + names[i] + '" style="font-size:12px;padding: 0px 5px;height:30px;"/></th>';
                results += '<td data-title="Phosps"><select id="ddlproductstatus" class="clsproductstatus" style="width:90px;" ><option value="Yes">Yes</option><option  value="No">No</option></select></td>';
                results += '<th scope="row" ><input class="clsBrand"  type="text" placeholder="Enter Brands" name="Brands" id="txtBrands" value=" " style="font-size:12px;padding: 0px 5px;height:30px;"/></th>';
                results += '<td data-title="AvgSale" style="text-align:center;" ><input class="clssale" type="text" placeholder="" name="Route"  value="" id="txt_sale"  style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td data-title="Phosps"><select id="ddlvyshnavi" class="clsproductstatus" style="width:90px;" ><option value="Yes">Yes</option><option  value="No">No</option></select></td>';
                results += '<td data-title="SupportRequired" style="text-align:center;" class="5"><input class="clsSupportRequired" type="text" placeholder=""  id="txtSupportrequired" name="SupportRequired" value="" style="font-size:12px;padding: 0px 5px;height:30px;"/></td>';
                results += '<td ><span id="txttotal" style="display:none;"  class="clstotal"style="width:500px;"></td></tr>';
            }
            results += '</table></div>';
            $("#div_RetailProducts").html(results);
        }
        function save_Retail_Form_click() {
            var outletname = document.getElementById('txtOutlet').value;
            var address = document.getElementById('txtAddress').value;
            var name = document.getElementById('txtIncharge').value;
            var phoneno = document.getElementById('txtPhoneNo').value;
            var emailid = document.getElementById('txtEmail-Id').value;
            var tinno = document.getElementById('txtTinNo').value;
            var RouteName = document.getElementById('txtRoute').value;
            var optime = document.getElementById('txtOpeningTime').value;
            var cltime = document.getElementById('txtClosingtime').value;
            var salevalue = document.getElementById('txtValue').innerHTML;
            var paymentterms = document.getElementById('txtPayementTerms').value;
            var tso = document.getElementById('txtTso').value;
            var weakoff = document.getElementById('ddlWeaklyOff').value;
            var rdsservice = document.getElementById('ddlServicing').value;
            var servicefreq = document.getElementById('ddlServicingFrequency').value;
            var category = document.getElementById('txtCategory').value;
            var lat = document.getElementById('txtlat').value;
            var lot = document.getElementById('txtlot').value;
            var btnval = document.getElementById('btn_saveretail').value;
            var total = document.getElementById('txtValue').innerHTML;
            var sno = document.getElementById('lblsno').value;
            var mobileno = document.getElementById('txtofficeNo').value;
            var classification = document.getElementById('txtClasifiaction').value;
            var own = document.getElementById('ddlownfreezer').value;
            var id = document.getElementById('ddlidfreezer').value;
            var milkymist = document.getElementById('ddlmilkyfreezer').value;
            var others = document.getElementById('ddlothersfreezer').value;
            var status = document.getElementById('ddlvyshnavi').value;

            if (outletname == "") {
                alert("Enter outletname");
                return false;
            }
            //            if (tinno == "") {
            //                alert("Enter  Tin No");
            //                return false;
            //            }
            var Retail_array = [];
            $('#tbl_Retail_Product_details> tbody > tr').each(function () {
                var productname = $(this).find('#txt_ProductName').val();
                var brand = $(this).find('#txtBrands').val();
                var qty = $(this).find('#txt_sale').val();
                var description = $(this).find('#txtSupportrequired').val();
                var productstatus = $(this).find('#ddlproductstatus').val();
                var vyshnavi = $(this).find('#ddlvyshnavi').val();
                var refno = $(this).find('#txtrefno').val();
                //                if (qty == "" || qty == "0") {
                //                }
                //                else {
                Retail_array.push({ 'productname': productname, 'productstatus': productstatus, 'brand': brand, 'refno': refno, 'vyshnavistatus': vyshnavi, 'qty': qty, 'description': description
                });
                //                }
            });
            var Data = { 'operation': 'save_Retail_Form_click', 'own': own, 'id': id, 'milkymist': milkymist, 'others': others, 'status': status, 'classification': classification, 'mobileno': mobileno, 'name': name, 'outletname': outletname, 'total': total, 'sno': sno, 'address': address, 'phoneno': phoneno, 'emailid': emailid, 'tinno': tinno, 'RouteName': RouteName, 'optime': optime, 'cltime': cltime, 'salevalue': salevalue, 'paymentterms': paymentterms, 'tso': tso, 'weakoff': weakoff, 'rdsservice': rdsservice, 'servicefreq': servicefreq, 'category': category, 'lat': lat, 'lot': lot, 'btnval': btnval, 'Retail_array': Retail_array };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    get_Retail_sub_details();
                    $('#Retail_FillForm').css('display', 'none');
                    $('#showlogs').css('display', 'block');
                    $('#div_RetailData').show();
                    forclearall();
                }
            }
            var e = function (x, h, e) {
            };
            CallHandlerUsingJson(Data, s, e);
        }
        function get_Retail_sub_details() {
            var data = { 'operation': 'get_Retail_sub_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fill_Retail_details(msg);
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
        var Retail_sub_list = [];
        function fill_Retail_details(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col"></th><th scope="col">OutletName</th><th scope="col">PhoneNo</th><th scope="col">RouteName</th></tr></thead></tbody>';
            var Retail_list = msg[0].RetailDetals;
            Retail_sub_list = msg[0].SubRetailDetals;
            var k = 1;
            for (var i = 0; i < Retail_list.length; i++) {
                results += '<tr><td><input id="btn_poplate" type="button"  onclick="getpurchasevalues(this)" name="submit" class="btn btn-primary" value="Edit" /></td>';
                results += '<td data-title="podate" style="display:none;" class="1">' + Retail_list[i].sno + '</td>';
                results += '<td data-title="name" class="2">' + Retail_list[i].outletname + '</td>';
                results += '<td data-title="delivarydate" style="display:none;" class="3">' + Retail_list[i].address + '</td>';
                results += '<td data-title="expiredate"  class="4">' + Retail_list[i].phoneno + '</td>';
                results += '<td data-title="poamount" style="display:none;" class="5">' + Retail_list[i].emailid + '</td>';
                results += '<td data-title="shortname" class="6" style="display:none;">' + Retail_list[i].tinno + '</td>';
                results += '<td data-title="freigntamt"  class="7" >' + Retail_list[i].RouteName + '</td>';
                results += '<td data-title="mobile" class="8" style="display:none;">' + Retail_list[i].optime + '</td>';
                results += '<td data-title="telphone" class="9" style="display:none;">' + Retail_list[i].cltime + '</td>';
                results += '<td data-title="vattin" class="10" style="display:none;">' + Retail_list[i].paymentterms + '</td>';
                results += '<td data-title="email" class="11" style="display:none;">' + Retail_list[i].tso + '</td>';
                results += '<td data-title="address" class="12" style="display:none;">' + Retail_list[i].weakoff + '</td>';
                results += '<td data-title="quotationno" class="13" style="display:none;">' + Retail_list[i].rdsservice + '</td>';
                results += '<td data-title="quotationdate" class="14" style="display:none;">' + Retail_list[i].servicefreq + '</td>';
                results += '<td data-title="quotationdate" class="15" style="display:none;">' + Retail_list[i].category + '</td>';
                results += '<td data-title="quotationdate" class="16" style="display:none;">' + Retail_list[i].lat + '</td>';
                results += '<td data-title="quotationdate" class="18" style="display:none;">' + Retail_list[i].name + '</td>';
                results += '<td data-title="quotationdate" class="19" style="display:none;">' + Retail_list[i].total + '</td>';
                results += '<td data-title="quotationdate" class="20" style="display:none;">' + Retail_list[i].mobileno + '</td>';
                results += '<td data-title="quotationdate" class="21" style="display:none;">' + Retail_list[i].classification + '</td>';
                results += '<td data-title="quotationdate" class="22" style="display:none;">' + Retail_list[i].own + '</td>';
                results += '<td data-title="quotationdate" class="23" style="display:none;">' + Retail_list[i].id + '</td>';
                results += '<td data-title="quotationdate" class="24" style="display:none;">' + Retail_list[i].milkymist + '</td>';
                results += '<td data-title="quotationdate" class="25" style="display:none;">' + Retail_list[i].others + '</td>';
                results += '<td data-title="quotationdate" class="26" style="display:none;">' + Retail_list[i].status + '</td>';
                results += '<td data-title="quotationdate" class="27" style="display:none;">' + Retail_list[i].images + '</td>';
                results += '<td data-title="quotationdate" class="28" style="display:none;">' + Retail_list[i].ftplocation + '</td>';
                results += '<td data-title="quotationdate" class="17" style="display:none;">' + Retail_list[i].lot + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_RetailData").html(results);
        }



        var sno = 0;
        function getpurchasevalues(thisid) {
            calTotal();
            $('#Retail_FillForm').css('display', 'block');
            $('#showlogs').css('display', 'none');
            $('#div_RetailData').hide();
            $('#newrow').css('display', 'none');
            var sno = $(thisid).parent().parent().children('.1').html();
            var outletname = $(thisid).parent().parent().children('.2').html();
            var address = $(thisid).parent().parent().children('.3').html();
            var phoneno = $(thisid).parent().parent().children('.4').html();
            var emailid = $(thisid).parent().parent().children('.5').html();
            var tinno = $(thisid).parent().parent().children('.6').html();
            var RouteName = $(thisid).parent().parent().children('.7').html();
            var optime = $(thisid).parent().parent().children('.8').html();
            var cltime = $(thisid).parent().parent().children('.9').html();
            var paymentterms = $(thisid).parent().parent().children('.10').html();
            var tso = $(thisid).parent().parent().children('.11').html();
            var weakoff = $(thisid).parent().parent().children('.12').html();
            var rdsservice = $(thisid).parent().parent().children('.13').html();
            var servicefreq = $(thisid).parent().parent().children('.14').html();
            var category = $(thisid).parent().parent().children('.15').html();
            var lat = $(thisid).parent().parent().children('.16').html();
            var lot = $(thisid).parent().parent().children('.17').html();
            var name = $(thisid).parent().parent().children('.18').html();
            var total = $(thisid).parent().parent().children('.19').html();
            var mobileno = $(thisid).parent().parent().children('.20').html();
            var classification = $(thisid).parent().parent().children('.21').html();

            var own = $(thisid).parent().parent().children('.22').html();
            var id = $(thisid).parent().parent().children('.23').html();
            var milkymist = $(thisid).parent().parent().children('.24').html();
            var others = $(thisid).parent().parent().children('.25').html();
            var status = $(thisid).parent().parent().children('.26').html();
            var image = $(thisid).parent().parent().children('.27').html();
            var ftplocation = $(thisid).parent().parent().children('.28').html();



            document.getElementById('txtOutlet').value = outletname;
            document.getElementById('txtAddress').value = address;
            document.getElementById('txtIncharge').value = name;
            document.getElementById('txtPhoneNo').value = phoneno;
            document.getElementById('txtEmail-Id').value = emailid;
            document.getElementById('txtTinNo').value = tinno;
            document.getElementById('txtRoute').value = RouteName;
            document.getElementById('txtOpeningTime').value = optime;
            document.getElementById('txtClosingtime').value = cltime;
            document.getElementById('txtValue').innerHTML = total;
            document.getElementById('txtPayementTerms').value = paymentterms;
            document.getElementById('txtTso').value = tso;
            document.getElementById('ddlWeaklyOff').value = weakoff;
            document.getElementById('ddlServicing').value = rdsservice;
            document.getElementById('ddlServicingFrequency').value = servicefreq;
            document.getElementById('txtCategory').value = category;
            document.getElementById('txtlat').value = lat;
            document.getElementById('txtlot').value = lot;
            document.getElementById('lblsno').value = sno;
            document.getElementById('txtofficeNo').value = mobileno;
            document.getElementById('txtClasifiaction').value = classification;


            document.getElementById('ddlownfreezer').value = own;
            document.getElementById('ddlidfreezer').value = id;
            document.getElementById('ddlmilkyfreezer').value = milkymist;
            document.getElementById('ddlothersfreezer').value = others;
            document.getElementById('ddlvyshnavi').value = status;

            var rndmnum = Math.floor((Math.random() * 10) + 1);
            img_url = ftplocation + image + '?v=' + rndmnum;
            if (image != "") {
                $('#main_img').attr('src', img_url).width(200).height(200);
            }
            else {
                $('#main_img').attr('src', 'Images/Employeeimg.jpg').width(200).height(200);
            }

            document.getElementById('btn_saveretail').value = "Modify";
            var table = document.getElementById("tabledetails");
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" ID="tbl_Retail_Product_details">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">ProductName</th><th scope="col">ProductStatus</th><th scope="col">Top Brands</th><th scope="col">Avg Sales/PerDay</th><th scope="col">Vyshnavi(Y/N)</th><th scope="col">SupportRequired</th></tr></thead></tbody>';
            var k = 1;
            for (var i = 0; i < Retail_sub_list.length; i++) {
                if (sno == Retail_sub_list[i].refno) {
                    results += '<tr><td data-title="Sno" class="1">' + k + '</td>';
                    results += '<td data-title="From"><input class="clsProductName" disabled="disabled" type="text"  id="txt_ProductName" readonly value="' + Retail_sub_list[i].productname + '" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;"></input></td>';
                    results += '<td data-title="From"><input id="ddlproductstatus" class="clsproductstatus"  value="' + Retail_sub_list[i].productstatus + '" style="width:90px; font-size:12px;padding: 0px 5px;height:30px;"></input></td>';
                    results += '<td data-title="From"><input class="clsBrand"  type="text" placeholder="Enter Brands" name="Brands" id="txtBrands"  value="' + Retail_sub_list[i].brand + '" style="width:90px; font-size:12px;padding: 0px 5px;height:30px;"></input></td>';
                    results += '<td data-title="From"><input class="clssale" type="text"  name="Route"   id="txt_sale"  value="' + Retail_sub_list[i].qty + '" onkeypress="return isFloat(event)" style="width:60px; font-size:12px;padding: 0px 5px;height:30px;"></td>';
                    results += '<td data-title="From"><input id="ddlvyshnavi" class="clsproductstatus"  value="' + Retail_sub_list[i].vyshnavistatus + '" style="width:90px; font-size:12px;padding: 0px 5px;height:30px;"></input></td>';
                    results += '<td data-title="From"><input  class="clsSupportRequired" type="text" placeholder=""  id="txtSupportrequired" name="SupportRequired" value="' + Retail_sub_list[i].description + '" onkeypress="return isFloat(event)"  style="width:100%; font-size:12px;padding: 0px 5px;height:30px;"></td>';
                    // results += '<td data-title="From"><input  id="txttotal" style="display:none;"   class="clstotal" value="' + Retail_sub_list[i].sno + '" onkeypress="return isFloat(event)" style="width:60px; font-size:12px;padding: 0px 5px;height:30px;"></td></tr>';
                    results += '<td data-title="From"><input style="display:none;"  id="txtrefno"   value="' + Retail_sub_list[i].sno + '"  style="width:60px; font-size:12px;padding: 0px 5px;height:30px;"></td>';
                    results += '<td ><span id="txttotal" style="display:none;"  class="clstotal"style="width:500px;"></td></tr>'
                    k++;
                }
            }
            results += '</table></div>';
            $("#div_RetailProducts").html(results);
        }

        function clstotalval() {
            var totaamount = 0; var totalpfamount = 0;
            $('.clstotal').each(function (i, obj) {
                var totlclass = $(this).html();

                if (totlclass == "" || totlclass == "0") {
                }
                else {
                    totaamount += parseFloat(totlclass);
                }
            });
            document.getElementById('txtValue').innerHTML = totaamount.toFixed(2);
        }

        var total = "";
        var value = "";
        function calTotal() {
            var $row = $(this).closest('tr'),
            value = $row.find('.clssale').val();
            $row.find('.clstotal').html(value);
            clstotalval();
        }
        $(document).click(function () {
            $('#tbl_Retail_Product_details').on('change', '.clssale', calTotal)
        });
        function forclearall() {
            document.getElementById('txtOutlet').value = "";
            document.getElementById('txtAddress').value = "";
            document.getElementById('txtIncharge').value = "";
            document.getElementById('txtPhoneNo').value = "";
            document.getElementById('txtEmail-Id').value = "";
            document.getElementById('txtTinNo').value = "";
            document.getElementById('txtRoute').value = "";
            document.getElementById('txtOpeningTime').value = "";
            document.getElementById('txtClosingtime').value = "";
            document.getElementById('txtValue').innerHTML = "";
            document.getElementById('txtPayementTerms').value = "";

            document.getElementById('txtofficeNo').value = "";
            document.getElementById('txtClasifiaction').value = "";

            document.getElementById('txtTso').value = "";


            document.getElementById('ddlWeaklyOff').selectedIndex = 0;
            document.getElementById('ddlServicing').selectedIndex = 0;
            document.getElementById('ddlServicingFrequency').value = 0;



            document.getElementById('txtCategory').value = "";
            document.getElementById('txtlat').value = "";
            document.getElementById('txtlot').value = "";

            document.getElementById('btn_saveretail').value = "Save";
            var empty = [];
            var results = '<div  style="overflow:auto;"><table id="tbl_Retail_Product_details" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">ProductName</th>><th scope="col">Top Brands</th><th scope="col">Avg Sales/PerDay</th><th scope="col">Freezer Status</th><th scope="col">Vyshnavi(Y/N)</th><th scope="col">SupportRequired</th></tr></thead></tbody>';
            var j = 1;
            for (var i = 0; i < empty.length; i++) {

            }
            results += '</table></div>';
            $("#div_RetailProducts").html(results);
        }

        function myFunction() {
            if (event.keyCode == 46 || event.keyCode == 110 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 190 ||
            // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
            // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
                // let it happen, don't do anything
                return;
            }
            else {
                // Ensure that it is a number and stop the keypress
                if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                    event.preventDefault();
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Retail Form <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Retail Form</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Retail Form Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" align="center">
                    <input id="btn_addOutlet" type="button" name="submit" value='Add RetailerName' class="btn btn-primary" />
                </div>
                <div id="div_RetailData">
                </div>
                <div id='Retail_FillForm' style="display: none;">
                    <div class="row">
                        <div class="col-sm-12 col-xs-12">
                            <div class="well panel panel-default" style="padding: 0px;">
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-sm-4" style="width: 100%;">
                                            <div class="row">
                                                <div class="col-xs-12 col-sm-3 text-center">
                                                    <div class="pictureArea1">
                                                        <img class="center-block img-circle img-thumbnail img-responsive profile-img" id="main_img"
                                                            alt="your image" style="border-radius: 5px; width: 200px; height: 200px; border-radius: 50%;" />
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
                                                            <input type="button" id="btn_upload_profilepic" class="btn btn-primary" onclick="upload_profile_pic();"
                                                                style="margin-top: 5px;" value="Upload Profile Pic">
                                                        </div>
                                                    </div>
                                                </div>
                                                <!--/col-->
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <table align="center">
                        <tr>
                            <td>
                                <label>
                                    Outlet Name
                                </label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtOutlet" type="text" class="form-control" name="ShortName" placeholder="Enter Outlet Name" />
                            </td>
                            <td>
                                <label>
                                    Address</label>
                            </td>
                            <td style="width: 420px;">
                                <textarea id="txtAddress" rows="4" cols="10" name="Remarks" class="form-control"
                                    placeholder="Enter Address">
                              </textarea>
                            </td>
                            <td>
                                <label>
                                    Incharge</label>
                            </td>
                            <td style="height: 40px;">
                                <input type="text" class="form-control" id="txtIncharge" name="Incharge" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Office No</label>
                            </td>
                            <td style="height: 40px;">
                                <input type="text" class="form-control" id="txtofficeNo" name="Incharge" onkeypress="return isFloat(event)" />
                            </td>
                            <td>
                                <label>
                                    Mobile No</label>
                            </td>
                            <td style="height: 40px;">
                                <input type="text" class="form-control" id="txtPhoneNo" name="Incharge" onkeypress="return isFloat(event)" />
                            </td>
                            <td>
                                <label>
                                    CateGory Name</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtCategory" type="text" class="form-control" name="Category" placeholder="Enter  Category" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Classification</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtClasifiaction" type="text" class="form-control" name="TinNo" placeholder="Enter Classification" />
                            </td>
                            <td>
                                <label>
                                    Email-Id</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtEmail-Id" type="text" class="form-control" name="FreAmount" placeholder="Enter  Email-Id" />
                            </td>
                            <td>
                                <label>
                                    Tin No</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtTinNo" type="text" class="form-control" name="TinNo" placeholder="Enter Tin Number" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Area/Route</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtRoute" type="text" class="form-control" name=" Area/Route" placeholder="Enter  Area/Route" />
                            </td>
                            <td>
                                <label>
                                    OpeningTime
                                </label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtOpeningTime" type="time" class="form-control" name="OpeningTime" />
                            </td>
                            <td>
                                <label>
                                    Closingtime</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtClosingtime" type="time" class="form-control" name="Closingtime" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    WeaklyOff</label>
                            </td>
                            <td style="height: 40px;">
                                <select id="ddlWeaklyOff" class="form-control">
                                    <option value="N">No</option>
                                    <option value="Y">YES</option>
                                </select>
                            </td>
                            <td>
                                <label>
                                    PaymentTerms</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtPayementTerms" type="text" class="form-control" name="PayementTerms" />
                            </td>
                            <td>
                                <label>
                                    RDS Servicing</label>
                            </td>
                            <td style="height: 40px;">
                                <select id="ddlServicing" class="form-control">
                                    <option value="Daily">Daily</option>
                                    <option value="Monthly">Monthly</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Servicing Frequency</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="ddlServicingFrequency" type="text" class="form-control" name="PayementTerms" />
                            </td>
                            <td>
                                <label>
                                    Name Of The TSO</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtTso" type="text" class="form-control" name="PayementTerms" />
                            </td>
                            <td>
                                <label>
                                    Own Freezers</label>
                            </td>
                            <td style="height: 40px;">
                                <select id="ddlownfreezer" class="form-control">
                                    <option value="N">No</option>
                                    <option value="Y">YES</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    ID Freezers</label>
                            </td>
                            <td>
                                <select id="ddlidfreezer" class="form-control">
                                    <option value="N">No</option>
                                    <option value="Y">YES</option>
                                </select>
                            </td>
                            <td>
                                <label>
                                    MilkyMist Freezers</label>
                            </td>
                            <td style="height: 40px;">
                                <select id="ddlmilkyfreezer" class="form-control">
                                    <option value="N">No</option>
                                    <option value="Y">YES</option>
                                </select>
                            </td>
                            <td>
                                <label>
                                    Others Freezers</label>
                            </td>
                            <td style="height: 40px;">
                                <select id="ddlothersfreezer" class="form-control">
                                    <option value="N">No</option>
                                    <option value="Y">YES</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Vyshnavi</label>
                            </td>
                            <td style="height: 40px;">
                                <select id="ddlvyshnavi" class="form-control">
                                    <option value="Y">YES</option>
                                    <option value="N">NO</option>
                                </select>
                            </td>
                            <td>
                                <label>
                                    Latitude</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtlat" type="text" class="form-control" name="PayementTerms" />
                            </td>
                            <td>
                                <label>
                                    Longtitude</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtlot" type="text" class="form-control" name="PayementTerms" />
                            </td>
                            <td>
                                <label id="lblsno" style="display: none;">
                            </td>
                        </tr>
                    </table>
                    <div class="box box-info">
                        <div class="box-header with-border">
                        </div>
                        <div class="box-body">
                            <div id="div_RetailProducts">
                            </div>
                        </div>
                    </div>
                    <table align="center" id="po">
                        <tr>
                            <td>
                                <label>
                                    Perday Value</label>
                            </td>
                            <td>
                                <span id="txtValue" type="text" style="width: 500px; color: Red; font-weight: bold;
                                    font-size: 25px;" class="clspomount" name="PoAmount" onkeypress="return isFloat(event)"
                                    placeholder="Enter PO Amount"></span>
                            </td>
                        </tr>
                    </table>
                    <div id="">
                    </div>
                    <table align="center">
                        <tr>
                            <td>
                                <input type="button" class="btn btn-primary" id="btn_saveretail" value="Save" onclick="save_Retail_Form_click();" />
                                <input type="button" class="btn btn-danger" id="close_Retail_Form" value="Close" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        </div>
    </section>
</asp:Content>
