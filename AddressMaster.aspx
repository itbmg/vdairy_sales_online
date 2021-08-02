<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="AddressMaster.aspx.cs" Inherits="AddressMaster" %>

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
            get_Address_details();
            get_bank_details();
            get_Others_Details();
            updatedepartment_manage();
            FillSalesOffice();
            PoFillSalesOffice();
            $("#div_Address").css("display", "block");
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
            get_Address_details();
            $("#div_Address").css("display", "block");
            $("#div_Department").css("display", "none");
            $("#div_ProductRanking").css("display", "none");
            $("#div_Bankdetails").css("display", "none");
            $("#div_PoEntry").css("display", "none");
            $("#div_CashCollection").css("display", "none");
        }
        function showDepartment() {

            $("#div_Address").css("display", "none");
            $("#div_Department").css("display", "block");
            $("#div_ProductRanking").css("display", "none");
            $("#div_Bankdetails").css("display", "none");
            $("#div_PoEntry").css("display", "none");
            $("#div_CashCollection").css("display", "none");
        }
        function showProductRanking() {
            $("#lblsales").css("display", "none");
            $("#txtsales").css("display", "none");
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
            $("#div_AddressData").show();
            $("#Address_fillform").hide();
            $('#Addresshowlogs').show();
            Addressforclearall();
        }
        function saveAddressDetails() {
            var companyname = document.getElementById('txtCompanyName').value;
            if (companyname == "") {
                alert("Enter  companyname");
                $("#txtCompanyName").focus();
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
            var customercode = document.getElementById('txtCustomerCode').value;
            var sno = document.getElementById('lblAddress_sno').value;
            var btnval = document.getElementById('btn_AddressSave').innerHTML;
            var data = { 'operation': 'saveAddressDetails', 'customercode': customercode, 'panno': panno, 'cst': cst, 'email': email, 'companyname': companyname, 'buildingaddress': buildingaddress, 'street': street, 'mandal': mandal, 'district': district, 'state': state, 'pin': pin, 'tin': tin, 'sno': sno, 'btnVal': btnval };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        Addressforclearall();
                        get_Address_details();
                        $('#div_AddressData').show();
                        $('#Address_fillform').css('display', 'none');
                        $('#Addresshowlogs').css('display', 'block');
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
        function Addressforclearall() {
            document.getElementById('txtCompanyName').value = "";
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
            document.getElementById('txtCustomerCode').value = "";
            document.getElementById('btn_AddressSave').innerHTML = "save";
        }
        function showAddressdesign() {
            $("#div_AddressData").hide();
            $("#Address_fillform").show();
            $('#Addresshowlogs').hide();
        }
        function get_Address_details() {
            var data = { 'operation': 'get_Address_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillAddress(msg);
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
        function fillAddress(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">CompanyName</th><th scope="col" class="thcls">BuildingAddress</th><th scope="col" class="thcls">TIN Number</th><th scope="col"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row" class="1 tdmaincls" >' + msg[i].companyname + '</td>';
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
                results += '<td style="display:none" class="13">' + msg[i].customercode + '</td>';
                results += '<td style="display:none" class="9">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmeaddress(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_AddressData").html(results);
        }
        function getmeaddress(thisid) {
            var companyname = $(thisid).parent().parent().children('.1').html();
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
            var customercode = $(thisid).parent().parent().children('.13').html();

            document.getElementById('txtCompanyName').value = companyname;
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
            document.getElementById('txtCustomerCode').value = customercode;
            document.getElementById('btn_AddressSave').innerHTML = "Modify";
            $("#div_AddressData").hide();
            $("#Address_fillform").show();
            $('#Addresshowlogs').hide();
        }
        function departmentsave_click() {
            var username = document.getElementById("txt_department_name").value;
            if (username == "") {
                alert("Please Provide Department Name");
                $("#txt_department_name").focus();
                return false;
            }
            else {
                departmentsave();
            }
        }
        function departmentsave() {
            var txtdepartmentname = document.getElementById('txt_department_name').value;
            var operationtype = document.getElementById('btn_dept_save').innerHTML;
            var sno = serial;
            var data = { 'operation': 'departmentsave', 'sno': sno, 'txtdepartmentname': txtdepartmentname, 'operationtype': operationtype };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    department_clear();
                    updatedepartment_manage();
                }
                else {
                }
            };
            var e = function (x, h, e) {
                if (x.status && x.status == 400) {
                    alert(x.responseText);
                    window.location.assign("Login.aspx");
                }
                else {
                    alert("something went wrong");
                }
            };
            callHandler(data, s, e);
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        }
        function department_clear() {
            document.getElementById('txt_department_name').value = "";
            document.getElementById('btn_dept_save').innerHTML = "SAVE";
            serial = 0;
        }
        function updatedepartment_manage() {
            var data = { 'operation': 'updatedepartment_manage' };
            var s = function (msg) {
                if (msg) {
                    BindGrid_department_manage(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
                if (x.status && x.status == 400) {
                    alert(x.responseText);
                    window.location.assign("Login.aspx");
                }
                else {
                    alert("something went wrong");
                }
            };
            callHandler(data, s, e);
        }
        var serial = 0;
        function BindGrid_department_manage(msg) {
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">Department Name</th><th scope="col"></th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row"  class="1 tdmaincls" >' + msg[i].DeptName + '</td>';
                results += '<td style="display:none" class="2">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmeDepartment(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';

                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }
        function getmeDepartment(thisid) {
            var DeptName = $(thisid).parent().parent().children('.1').html();
            var sno = $(thisid).parent().parent().children('.2').html();
            document.getElementById('txt_department_name').value = DeptName;
            document.getElementById('btn_dept_save').innerHTML = "MODIFY";
            serial = sno;
        }
        //Product Ranking Details

        function ddlproducttypechange() {
            var ddlproducttype = document.getElementById('ddlproducttype').value;
            if (ddlproducttype == "ProductWise") {
                $("#lblsales").css("display", "none");
                $("#txtsales").css("display", "none");
            }
            else {
                $("#lblsales").css("display", "block");
                $("#txtsales").css("display", "block");
            }
        }

        function btngenerateBranchProduct() {
            //FillProducts();
            var BranchID = document.getElementById('ddlProductRankSalesOffice').value; //ddlProductRankSalesOffice ID.value;
            var ddlproducttype = document.getElementById('ddlproducttype').value;
            var data = { 'operation': 'GetBranchProducts', 'BranchID': BranchID, 'ddlproducttype': ddlproducttype };
            var s = function (msg) {
                if (msg) {
                    BindProducts(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var products = [];
        function BindProducts(msg) {
          //  products = msg[0].PlantProducts;
            document.getElementById('divselected').innerHTML = "";
            for (var i = 0; i < msg.length; i++) {
                var Selected = msg[i].ProdName;
                var Selectedid = msg[i].ProdID;
                var label = document.createElement("div");
                var Crosslabel = document.createElement("img");
                //                Crosslabel.style.float = "right";
                //                Crosslabel.src = "Images/Cross.png";
                //                Crosslabel.onclick = function () { RemoveClick(Selectedid); };
                label.id = Selectedid;
                label.innerHTML = Selected;
                label.className = 'divselectedclass';
                label.onclick = function () { divonclick(this); }
                document.getElementById('divselected').appendChild(label);
                //                label.appendChild(Crosslabel);
            }
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
        function btnBranchProductsRankingclick() {
            var div = document.getElementById('divselected');
            var divs = div.getElementsByTagName('div');
            var divArray = [];
            for (var i = 0; i < divs.length; i += 1) {
                divArray.push(divs[i].id);
            }
            var ddlproducttype = document.getElementById('ddlproducttype').value;
            var ddlsalesOffice = document.getElementById('ddlProductRankSalesOffice').value;
            var Data = { 'operation': 'btnBranchProductsRankingclick', 'dataarr': divArray, 'BranchID': ddlsalesOffice, 'DCType': ddlproducttype };
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
            CallHandlerUsingJson(Data, s, e);
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
            var ddlProductRankSalesOffice = document.getElementById('ddlProductRankSalesOffice');
            var length = ddlProductRankSalesOffice.options.length;
            ddlProductRankSalesOffice.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            ddlProductRankSalesOffice.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlProductRankSalesOffice.appendChild(opt);
                }
            }
        }
        //Bank Details
        function canceldetailsbank() {
            $("#div_BankData").show();
            $("#Bank_fillform").hide();
            $('#Bankshowlogs').show();
            Bankforclearall();
        }
        function saveBankDetails() {
            var name = document.getElementById('txtBName').value;
            if (name == "") {
                $("#txtBName").focus();
                alert("Enter Bank Name ");
                return false;

            }
            var code = document.getElementById('txtBcode').value;
            if (code == "") {
                $("#txtBcode").focus();
                alert("Enter  BranchCode ");
                return false;
            }

            var status = document.getElementById('ddlbankstatus').value;
            if (status == "") {
                $("#ddlbankstatus").focus();
                alert("Select Status");
            }

            //var status = document.getElementById('ddlstatus').value;
            var btnval = document.getElementById('btn_savebank').innerHTML;
            var sno = document.getElementById('lbl_banksno').value;
            var data = { 'operation': 'saveBankDetails', 'Name': name, 'Code': code, 'status': status, 'btnVal': btnval, 'sno': sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        get_bank_details();
                        $('#div_BankData').show();
                        $('#Bank_fillform').css('display', 'none');
                        $('#Bankshowlogs').css('display', 'block');
                        Bankforclearall();
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
        function Bankforclearall() {
            document.getElementById('txtBcode').value = "";
            document.getElementById('txtBName').value = "";
            document.getElementById('lbl_banksno').value = "";
            document.getElementById('ddlbankstatus').selectedIndex = 0;
            document.getElementById('btn_savebank').innerHTML = "save";
            $("#lbl_code_error_msg").hide();
            $("#lbl_name_error_msg").hide();
            get_bank_details();
        }
        function Bankshowdesign() {
            // get_bank_details();
            $("#div_BankData").hide();
            $("#Bank_fillform").show();
            $('#Bankshowlogs').hide();
            Bankforclearall();
        }
        function get_bank_details() {
            var data = { 'operation': 'get_bank_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillbankdetails(msg);
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
        function fillbankdetails(msg) {
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer">';
            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">Sno</th></th><th scope="col" class="thcls">Bank Name</th><th scope="col" class="thcls">Branch Code</th><th scope="col" class="thcls">Status</th><th scope="col"></th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                var status = msg[i].status;
                if (status == "0") {
                    status = "Inactive";

                }
                else {
                    status = "Active";
                }
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td>' + k++ + '</td>';
                results += '<td scope="row"  class="1 tdmaincls">' + msg[i].name + '</td>';
                results += '<td data-title="code" class="2">' + msg[i].code + '</td>';
                results += '<td data-title="status" class="3">' + status + '</td>';
                results += '<td style="display:none" class="4">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmeBank(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';

                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_BankData").html(results);
        }
        function getmeBank(thisid) {
            var name = $(thisid).parent().parent().children('.1').html();
            var Code = $(thisid).parent().parent().children('.2').html();
            var sno = $(thisid).parent().parent().children('.4').html();
            var statuscode = $(thisid).parent().parent().children('.3').html();
            if (statuscode == "Active") {

                status = "1";
            }
            else {
                status = "0";
            }
            document.getElementById('txtBName').value = name;
            document.getElementById('txtBcode').value = Code;

            document.getElementById('lbl_banksno').value = sno;
            document.getElementById('ddlbankstatus').value = status;
            document.getElementById('btn_savebank').innerHTML = "Modify";
            $("#div_BankData").hide();
            $("#Bank_fillform").show();
            $('#Bankshowlogs').hide();
        }

        //PO Entry Details
        function PoFillSalesOffice() {
            var data = { 'operation': 'GetPlantSalesOffice' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    POBindSalesOffice(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function POBindSalesOffice(msg) {
            var ddlposalesoffice = document.getElementById('ddlposalesoffice');
            var length = ddlposalesoffice.options.length;
            ddlposalesoffice.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            ddlposalesoffice.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlposalesoffice.appendChild(opt);
                }
            }
        }
        function ddlSalesOfficeChange() {
            var BranchID = document.getElementById('ddlposalesoffice').value;
            var data = { 'operation': 'GetSalesRoutes', 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindRouteName(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindRouteName(msg) {
            document.getElementById('ddlRouteName').options.length = "";
            var data = document.getElementById('ddlRouteName');
            var length = data.options.length;
            document.getElementById('ddlRouteName').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Route Name";
            opt.value = "";
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].RouteName;
                    opt.value = msg[i].rid;
                    data.appendChild(opt);
                }
            }
        }
        function ddlRouteNameChange() {
            var RouteID = document.getElementById('ddlRouteName').value;
            var data = { 'operation': 'GetAgents', 'RouteID': RouteID };
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
            var data = document.getElementById('ddlAgentName');
            var length = data.options.length;
            document.getElementById('ddlAgentName').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Agent Name";
            opt.value = "";
            data.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    data.appendChild(opt);
                }
            }
        }

        function btnUpdateQuotationPoNumbers() {
            var soid = document.getElementById('ddlposalesoffice').value;
            if (soid == "" || soid == "select") {
                alert("Please Select SalesOffice Name");
                $("#ddlposalesoffice").focus();
                return false;
            }
            var ddlRouteName = document.getElementById('ddlRouteName').value;
            if (ddlRouteName == "" || ddlRouteName == "Select Route Name") {
                alert("Please Select RouteName");
                $("#datepicker").focus();
                return false;
            }
            var ddlAgentName = document.getElementById('ddlAgentName').value;
            if (ddlAgentName == "" || ddlRouteName == "Select Agent Name") {
                alert("Please Select AgentName");
                $("#datepicker").focus();
                return false;
            }
            var indentdate = document.getElementById('datepicker').value;
            if (indentdate == "") {
                alert("Please Select IndentDate");
                $("#datepicker").focus();
                return false;
            }
            var pono = document.getElementById('txtPoNo').value;
            if (pono == "") {
                alert("Please Enter PONumber");
                $("#txtPoNo").focus();
                return false;
            }
            var quotano = document.getElementById('txtQuotationNo').value;
            var grnno = document.getElementById('txtGrnNo').value;
            var btn_save = document.getElementById("btnPOSave").innerHTML;
            var data = { 'operation': 'btnUpdateQuotationPoNumbers', 'BranchID': ddlAgentName, 'quotano': quotano, 'indentdate': indentdate, 'pono': pono, 'grnno': grnno, 'soid': soid, 'btn_save': btn_save };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    POforclearall();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function POforclearall() {
//            document.getElementById('ddlAgentName').selectedIndex = "";
            document.getElementById('datepicker').value = "";
            document.getElementById('txtQuotationNo').value = "";
            document.getElementById('txtPoNo').value = "";
            document.getElementById('txtGrnNo').value = "";
//            document.getElementById('ddlposalesoffice').selectedIndex = "";
//            document.getElementById('ddlRouteName').selectedIndex = "";
            document.getElementById('btnPOSave').innerHTML = "save";
        }

        //CASH COLLECTIONS
        function Cancel_Others_Details() {
            $("#div_Others").show();
            $("#Others_fillform").hide();
            $('#CashCollectionsowlogs').show();
            CashOthersforclearall();
        }
        function Svae_Others_Details() {
            var ledgename = document.getElementById('txtLedgerName').value;
            if (ledgename == "") {
                alert("Enter LedgeName");
                $("#txtLedgerName").focus();
                return false;
            }
            var ledgercode = document.getElementById('txtLedgerCode').value;
            if (ledgercode == "") {
                alert("Enter LedgerCode");
                $("#txtLedgerCode").focus();
                return false;
            }

            var sno = document.getElementById('lbl_OtherCashSno').value;
            var btnval = document.getElementById('btnCashOthersSave').innerHTML;

            var data = { 'operation': 'Svae_Others_Details', 'ledgercode': ledgercode, 'ledgename': ledgename, 'sno': sno, 'btnVal': btnval };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        CashOthersforclearall();
                        get_Others_Details();
                        $('#div_Others').show();
                        $('#Others_fillform').css('display', 'none');
                        $('#CashCollectionsowlogs').css('display', 'block');
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
        function CashOthersforclearall() {
            document.getElementById('txtLedgerCode').value = "";
            document.getElementById('txtLedgerName').value = "";
            document.getElementById('btnCashOthersSave').innerHTML = "save";
        }
        function showCashCollectiondesign() {
            $("#div_Others").hide();
            $("#Others_fillform").show();
            $('#CashCollectionsowlogs').hide();
        }
        function get_Others_Details() {
            var data = { 'operation': 'get_Others_Details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        FillOthers(msg);
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
        function FillOthers(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">LedgerName</th><th scope="col" class="thcls">LedgerCode</th><th scope="col"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row"  class="1 tdmaincls" >' + msg[i].name + '</td>';
                results += '<td data-title="brandstatus"  class="2">' + msg[i].ledgercode + '</td>';
                results += '<td style="display:none" class="3">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmeCashOthers(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';

                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_Others").html(results);
        }
        function getmeCashOthers(thisid) {
            var ledgername = $(thisid).parent().parent().children('.1').html();
            var ledgercode = $(thisid).parent().parent().children('.2').html();
            var sno = $(thisid).parent().parent().children('.3').html();
            document.getElementById('txtLedgerCode').value = ledgercode;
            document.getElementById('lbl_OtherCashSno').value = sno;
            document.getElementById('txtLedgerName').value = ledgername;
            document.getElementById('btnCashOthersSave').innerHTML = "Modify";
            $("#div_Others").hide();
            $("#Others_fillform").show();
            $('#CashCollectionsowlogs').hide();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content">
        <!-- Small boxes (Stat box) -->
        <div class="row">
            <section class="content-header">
                <h1>
                    Mini Masters test 
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
                                    <i class="fa fa-building-o" aria-hidden="true"></i>&nbsp;&nbsp;Department</a></li>
                                <li id="id_tab_ProductRanking" class=""><a data-toggle="tab" href="#" onclick="showProductRanking()">
                                    <i class=""></i>&nbsp;&nbsp;Product Ranking</a></li>
                                <%--<li id="id_tab_BankDetails" class=""><a data-toggle="tab" href="#" onclick="showbankmaster()">
                                    <i class="fa fa-university"></i>&nbsp;&nbsp;BankDetails</a></li>--%>
                                <li id="id_tab_PoDetails" class=""><a data-toggle="tab" href="#" onclick="showPOEntryDetails()">
                                    <i class="fa fa-file-text-o"></i>&nbsp;&nbsp;POEntry</a></li>
                                <li id="id_tab_CashCollections" class=""><a data-toggle="tab" href="#" onclick="showCashCollectionMaster()">
                                    <i class="icon-money"></i>&nbsp;&nbsp;CashCollections</a></li>
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
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Department Details
                                </h3>
                            </div>
                            <div class="box-body">
                                <table align="center">
                                    <tr>
                                        <td>
                                            <label id="lbldepartment">
                                                Department Name:</label><span style="color: red; font-weight: bold">*</span>
                                        </td>
                                        <td style="height: 40px;">
                                            <input type="text" id="txt_department_name" class="form-control" style="text-transform: capitalize;" placeholder="Enter Department Name" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label id="lbldeptflag">
                                                Flag:</label><span style="color: red; font-weight: bold">*</span>
                                        </td>
                                        <td style="height: 40px;">
                                            <select id="cmb_dept_flag" class="form-control">
                                                <option>Active</option>
                                                <option>InActive</option>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <div class="input-group">
                                                            <div class="input-group-addon">
                                                                <span class="glyphicon glyphicon-ok" id="btn_dept_save1" onclick="departmentsave_click()">
                                                                </span><span id="btn_dept_save" onclick="departmentsave_click()">SAVE</span>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td style="padding-left: 7px;">
                                                        <div class="input-group">
                                                            <div class="input-group-close">
                                                                <span class="glyphicon glyphicon-remove" id="btn_dept_clear1" onclick="department_clear()">
                                                                </span><span id="btn_dept_clear" onclick="department_clear()">CLEAR</span>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </div>
                            <div class="box box-info">
                                <div class="box-header with-border">
                                    <h3 class="box-title">
                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Department list
                                    </h3>
                                </div>
                                <div id="div_Deptdata">
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
                                                Select Type</label><span style="color: red; font-weight: bold">*</span>
                                        </td>
                                        <td>
                                             <select id="ddlproducttype" class="form-control" onchange="ddlproducttypechange()">
                                                <option value="ProductWise">ProductWise</option>
                                                <option value="BranchWise">BranchWise</option>
                                            </select>
                                        </td>
                                        <td style="width: 4px;">
                                            </td>
                                        <td id="lblsales">
                                            <label>
                                                SalesOffice</label><span style="color: red; font-weight: bold">*</span>
                                        </td>
                                        <td id="txtsales">
                                            <select id="ddlProductRankSalesOffice" class="form-control">
                                            </select>
                                        </td>
                                         <td style="width: 4px;">
                                            </td>
                                        <td>
                                            <div class="input-group">
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-ok" id="Span1" onclick="btngenerateBranchProduct()">
                                                    </span><span id="Span2" onclick="btngenerateBranchProduct()">Generate</span>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table style="padding-left: 20%;" align="center">
                                    <tr>
                                        <td>
                                            <div class="box box-info" style="float: left; width: 350px; overflow: auto;">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Product Details
                                                    </h3>
                                                </div>
                                                <div class="box-body">
                                                    <div id="divselected">
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <input type="button" class="btnUp" onclick="btnUpClick();" /><br />
                                            <input type="button" class="btnDown" onclick="btnDownClick();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="input-group">
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-ok" id="btnSave1" onclick="btnBranchProductsRankingclick()">
                                                    </span><span id="btnSave" onclick="btnBranchProductsRankingclick()">Save</span>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
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
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>PoEntry Details
                                </h3>
                            </div>
                            <div class="box-body">
                                <table align="center">
                                    <tr>
                                        <td>
                                            <label>
                                                Sales Office:</label><span style="color: red; font-weight: bold">*</span>
                                            <select id="ddlposalesoffice" class="form-control" onchange="ddlSalesOfficeChange();">
                                            </select>
                                        </td>
                                        <td style="width:2px;">
                                        </td>
                                        
                                         <td>
                                            <label>
                                                Route Name</label><span style="color: red; font-weight: bold">*</span>
                                            <select id="ddlRouteName" class="form-control" onchange="ddlRouteNameChange();">
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>
                                                Agent Name</label><span style="color: red; font-weight: bold">*</span>
                                            <select id="ddlAgentName" class="form-control">
                                            </select>
                                        </td>
                                        <td style="width:2px;">
                                        </td>
                                        <td>
                                            <label>
                                                IndentDate:</label><span style="color: red; font-weight: bold">*</span>
                                            <input type="date" id="datepicker" placeholder="DD-MM-YYYY" class="form-control" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>
                                                DC Number :</label>
                                            <input type="text" id="txtQuotationNo" placeholder="Enter QuotationNo" class="form-control" />
                                        </td>
                                        <td style="width:2px;">
                                        </td>
                                        <td>
                                            <label>
                                                PO No:</label><span style="color: red; font-weight: bold">*</span>
                                            <input type="text" id="txtPoNo" placeholder="Enter Po Number" class="form-control" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>
                                                GRN No:</label>
                                            <input type="text" id="txtGrnNo" placeholder="Enter Grn Number" class="form-control" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        <table>
                                        <tr>
                                        <td style="padding: 1px;">
                                            <div class="input-group">
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-ok" id="btnPOSave1" onclick="btnUpdateQuotationPoNumbers()">
                                                    </span><span id="btnPOSave" onclick="btnUpdateQuotationPoNumbers()">Save</span>
                                                </div>
                                            </div>
                                            <label id="lbl_collectionsno" style="display: none;">
                                            </label>
                                            </td>
                                            </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
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
