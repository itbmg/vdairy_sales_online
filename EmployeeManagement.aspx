<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EmployeeManagement.aspx.cs" Inherits="EmployeeManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="JIC/JIC.js?v=101" type="text/javascript"></script>
    <script src="JSF/imagezoom.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            filldepartments();
            getBranchEmployeedata();
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
        function AscCallHandler(d, s, e) {
            $.ajax({
                url: 'DairyFleet.axd',
                data: d,
                type: 'GET',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: false,
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

        var fillBranchDataArr = [];
//        function fillBranch(msg) {
//            fillBranchDataArr = msg;
//            var compiledList = [];
//            for (var i = 0; i < msg.length; i++) {
//                var brncName = msg[i].brncName;
//                compiledList.push(brncName);
//            }
//        }
        function btn_lctngradd_Click() {
            var checkedbranch = document.getElementById("ddlBranchName").value;
            if (checkedbranch == "" || checkedbranch == "select") {
                alert("Please Select Branch Name");
                return false;
            }
            var flag = document.getElementById("cmb_emp_flag").value;
            if (flag == "") {
                alert("Please Select Flag");
                $("#cmb_emp_flag").focus();
                return false;
            }

            var otpstatus = document.getElementById("ddlotpstatus").value;
            if (otpstatus == "") {
                alert("Please Select OTP Status");
                $("#ddlotpstatus").focus();
                return false;
            }

            

            var loginsleveltype = $("#cmb_emp_level option:selected").text();
            if (loginsleveltype == "" || loginsleveltype == "Select") {
                alert("Please Select LevelType");
                $("#cmb_emp_level").focus();
                return false;
            }
            var loginsusername = document.getElementById('txt_emp_username').value;
            if (loginsusername == "") {
                alert("Please Enter Login UserName");
                $("#loginsusername").focus();
                return false;
            }
            var loginspassword = document.getElementById('txt_emp_password').value;
            if (loginspassword == "") {
                alert("Please Enter Login Password");
                $("#txt_emp_password").focus();
                return false;
            }
            var employeename = document.getElementById('txt_emp_name').value;
            if (employeename == "") {
                alert("Please Enter Employee Name");
                $("#txt_emp_name").focus();
                return false;
            }
            var mobileno = document.getElementById('txt_emp_phno').value;
            if (mobileno == "") {
                alert("Please Enter Mobile Number");
                $("#txt_emp_phno").focus();
                return false;
            }
            var email = document.getElementById('txt_emp_email').value;
            var Previouscompany = document.getElementById('txt_emp_prevcompany').value;
            var refference = document.getElementById('txt_emp_refference').value;
            var department = document.getElementById('cmb_emp_department').value;
            var daytarget = document.getElementById('txt_emp_dtarget').value;
            var weektarget = document.getElementById('txt_emp_wtarget').value;
            var monthtarget = document.getElementById('txt_emp_mtarget').value;
            var loginsflag = document.getElementById('cmb_emp_flag').value;
            var operationtype = document.getElementById('btn_emp_save').innerHTML;
            var street = document.getElementById('txtStreet').value;
            var city = document.getElementById('txtCity').value;
            //            if (city == "") {
            //                alert("Please Enter City");
            //                $("#txtCity").focus();
            //                return false;
            //            }

            var mandal = document.getElementById('txtMandal').value;
            var district = document.getElementById('txtdistrict').value;
            //            if (district == "") {
            //                alert("Please Enter District");
            //                $("#txtdistrict").focus();
            //                return false;
            //            }

            var state = document.getElementById('txt_state').value;
            //            if (state == "") {
            //                alert("Please Enter State");
            //                $("#txt_state").focus();
            //                return false;
            //            }
            var pincode = document.getElementById('txtPincode').value;
            var panno = document.getElementById('txtpanno').value;
            var sno = serial23;
            var data = { 'operation': 'Emplogins_management', 'sno': sno, 'checkedbranch': checkedbranch, 'loginsleveltype': loginsleveltype, 'loginsusername': loginsusername, 'loginspassword': loginspassword, 'employeename': employeename, 'mobileno': mobileno, 'email': email, 'Previouscompany': Previouscompany, 'refference': refference, 'department': department, 'daytarget': daytarget, 'weektarget': weektarget, 'monthtarget': monthtarget, 'loginsflag': loginsflag, 'operationtype': operationtype, 'street': street, 'city': city, 'mandal': mandal, 'district': district, 'state': state, 'pincode': pincode, 'panno': panno, 'otpstatus': otpstatus };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    employee_management_clear();
                    getBranchEmployeedata();
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
                    alert(x.responseText);
                }
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function employee_management_clear() {

            $('#add_Employee').css('display', 'block');
            $('#emp_showlogs').css('display', 'block');
            $('#div_Customer').css('display', 'none');
            $('#div_Empdata').css('display', 'block');
           // ddlBranchProductNameChange();
            BranchSno = 0;
            document.getElementById('txt_emp_username').value = "";
            document.getElementById('txt_emp_password').value = "";
            document.getElementById('cmb_emp_flag').selectedIndex = 0;
            document.getElementById('ddlotpstatus').selectedIndex = 0;

            
            document.getElementById('cmb_emp_level').selectedIndex = 0;
            document.getElementById('txt_emp_name').value = "";
            document.getElementById('txt_emp_phno').value = "";
            document.getElementById('txt_emp_email').value = "";
            document.getElementById('txt_emp_prevcompany').value = "";
            document.getElementById('txt_emp_refference').value = "";
            document.getElementById('cmb_emp_department').selectedIndex = 0;
            document.getElementById('txt_emp_dtarget').value = "";
            document.getElementById('txt_emp_wtarget').value = "";
            document.getElementById('txt_emp_mtarget').value = "";
            document.getElementById('txtStreet').value = "";
            document.getElementById('txtCity').value = "";
            document.getElementById('txtMandal').value = "";
            document.getElementById('txtdistrict').value = "";
            document.getElementById('txt_state').value = "";
            document.getElementById('txtPincode').value = "";
            document.getElementById('txtpanno').value = "";
            document.getElementById('lblEmpName').innerHTML = "";
            document.getElementById('lblEmployeeEmailId').innerHTML = "";
            document.getElementById('lblEmployeeMobileNumber').innerHTML = "";
            document.getElementById('btn_emp_save').innerHTML = "SAVE";
        }

        function getBranchEmployeedata() {
            var data = { 'operation': 'update_employees_management' };
            var s = function (msg) {
                if (msg.length > 0) {
                    bindingemployee_management(msg);
                    BranchdataArr = msg;
                    BindSalesOffice();
                    searchempEmployeName();
                }

            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillBranchNamechange() {
            bindingemployee_management(BranchdataArr)
        }
        function changeEmployeeName() {
            bindingemployee_management(BranchdataArr)
        }
        var BranchdataArr = [];
        function bindingemployee_management(msg) {
            BranchdataArr = msg;
            var BranchName = document.getElementById('ddlBranchName').value;
            var EmpName = document.getElementById('txtEmployeeSearch').value;

            if (BranchName == "" && EmpName == "") {
                var l = 0;
                var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
                var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col">BranchName</th><th scope="col">Emp Name</th><th scope="col">Mobile</th><th scope="col">Email</th><th scope="col">User Name</th><th scope="col">Level Type</th><th scope="col">Image</th><th scope="col">Flag</th><th scope="col"></th></tr></thead></tbody>';
                for (var i = 0; i < BranchdataArr.length; i++) {
                    var status = 'InActive';
                    if (BranchdataArr[i].Loginsflag == '1') {
                        status = 'Active';
                    }
                    results += '<tr style="background-color:' + COLOR[l] + '">';
                    results += '<td data-title="Capacity"  class="2">' + BranchdataArr[i].Branchname + '</td>';
                    results += '<td scope="row"  class="4"><i class="glyphicon glyphicon-user" aria-hidden="true"></i>&nbsp;<span style="font-weight:600;" id="4">' + BranchdataArr[i].EmpName + '</span></td>';
                    results += '<td scope="row" class="5"><i class="glyphicon glyphicon-phone" aria-hidden="true"></i>&nbsp;<span style="font-weight:600;" id="5">' + BranchdataArr[i].Mobno + '</span></td>';
                    results += '<td scope="row" class="7"><i class="glyphicon glyphicon-envelope" aria-hidden="true"></i>&nbsp;<span id="7">' + BranchdataArr[i].email + '</span></td>';
                    results += '<td data-title="Capacity" style="display:none" class="8">' + BranchdataArr[i].previouscompny + '</td>';
                    results += '<td data-title="Capacity" style="display:none" class="9">' + BranchdataArr[i].refference + '</td>';
                    results += '<td data-title="Capacity" style="display:none" class="10">' + BranchdataArr[i].department + '</td>';
                    results += '<td data-title="Capacity"  class="3">' + BranchdataArr[i].UserName + '</td>';
                    results += '<td data-title="Capacity" style="display:none" class="11">' + BranchdataArr[i].dtrgt + '</td>';
                    results += '<td data-title="Capacity" style="display:none" class="12">' + BranchdataArr[i].wtrgt + '</td>';
                    results += '<td data-title="Capacity" style="display:none" class="13">' + BranchdataArr[i].mtrgt + '</td>';
                    results += '<td data-title="Capacity" style="display:none" class="14">' + BranchdataArr[i].BranchSno + '</td>';
                    results += '<td data-title="Capacity" style="display:none" class="15">' + BranchdataArr[i].sno + '</td>';
                    results += '<td data-title="Capacity" style="display:none" class="17">' + BranchdataArr[i].Password + '</td>';
                    results += '<td data-title="Capacity" style="display:none" class="18">' + BranchdataArr[i].street + '</td>';
                    results += '<td data-title="Capacity" style="display:none" class="19">' + BranchdataArr[i].city + '</td>';
                    results += '<td data-title="Capacity" style="display:none" class="20">' + BranchdataArr[i].mandal + '</td>';
                    results += '<td data-title="Capacity" style="display:none" class="21">' + BranchdataArr[i].district + '</td>';
                    results += '<td data-title="Capacity" style="display:none" class="22">' + BranchdataArr[i].state + '</td>';
                    results += '<td data-title="Capacity" style="display:none" class="23">' + BranchdataArr[i].pincode + '</td>';
                    results += '<td data-title="Capacity" style="display:none" class="26">' + BranchdataArr[i].panno + '</td>';
                    results += '<td data-title="Capacity" style="display:none" class="27">' + BranchdataArr[i].Emp_PIC + '</td>';
                    results += '<td data-title="Capacity" style="display:none" class="28">' + BranchdataArr[i].ftplocation + '</td>';
                    results += '<td scope="row" class="1" style="text-align:center;">' + BranchdataArr[i].LevelType + '</td>';
                    results += '<td scope="row" class="1" style="display:none" class="29">' + BranchdataArr[i].otpstatus + '</td>';

                    var rndmnum = Math.floor((Math.random() * 10) + 1);
                    var img_url = BranchdataArr[i].ftplocation + BranchdataArr[i].Emp_PIC + '?v=' + rndmnum;
                    if (BranchdataArr[i].Emp_PIC != "") {
                        results += '<td><img data-imagezoom="true" class="img-circle img-responsive" id="main_img_item" alt="Item Image" src="' + img_url + '" style="width: 35px; height: 35px;border: 2px solid gray;" /></td>';
                    }
                    else {
                        results += '<td><img data-imagezoom="true" class="img-circle img-responsive" id="main_img_item" alt="Item Image" src="Images/dummy_image.jpg" style="width: 35px; height: 35px;border: 2px solid gray;" /></td>';
                    }
                    results += '<td data-title="Capacity"  class="16">' + status + '</td>';
                    results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';

                    l = l + 1;
                    if (l == 4) {
                        l = 0;
                    }
                }
                results += '</table></div>';
            }
            else if (BranchName != "") {
            if (EmpName != "") {
                var l = 0;
                var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
                var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col">BranchName</th><th scope="col">Emp Name</th><th scope="col">Mobile</th><th scope="col">Email</th><th scope="col">User Name</th><th scope="col">Level Type</th><th scope="col">Image</th><th scope="col">Flag</th><th scope="col"></th></tr></thead></tbody>';
                for (var i = 0; i < BranchdataArr.length; i++) {
                    if (BranchName == BranchdataArr[i].BranchSno && EmpName == BranchdataArr[i].EmpName) {
                        var status = 'InActive';
                        if (BranchdataArr[i].Loginsflag == '1') {
                            status = 'Active';
                        }
                        results += '<tr style="background-color:' + COLOR[l] + '">';
                        results += '<td data-title="Capacity"  class="2">' + BranchdataArr[i].Branchname + '</td>';
                        results += '<td scope="row"  class="4"><i class="glyphicon glyphicon-user" aria-hidden="true"></i>&nbsp;<span style="font-weight:600;" id="4">' + BranchdataArr[i].EmpName + '</span></td>';
                        results += '<td scope="row" class="5"><i class="glyphicon glyphicon-phone" aria-hidden="true"></i>&nbsp;<span style="font-weight:600;" id="5">' + BranchdataArr[i].Mobno + '</span></td>';
                        results += '<td scope="row" class="7"><i class="glyphicon glyphicon-envelope" aria-hidden="true"></i>&nbsp;<span id="7">' + BranchdataArr[i].email + '</span></td>';
                        results += '<td data-title="Capacity" style="display:none" class="8">' + BranchdataArr[i].previouscompny + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="9">' + BranchdataArr[i].refference + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="10">' + BranchdataArr[i].department + '</td>';
                        results += '<td data-title="Capacity"  class="3">' + BranchdataArr[i].UserName + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="11">' + BranchdataArr[i].dtrgt + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="12">' + BranchdataArr[i].wtrgt + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="13">' + BranchdataArr[i].mtrgt + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="14">' + BranchdataArr[i].BranchSno + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="15">' + BranchdataArr[i].sno + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="17">' + BranchdataArr[i].Password + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="18">' + BranchdataArr[i].street + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="19">' + BranchdataArr[i].city + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="20">' + BranchdataArr[i].mandal + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="21">' + BranchdataArr[i].district + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="22">' + BranchdataArr[i].state + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="23">' + BranchdataArr[i].pincode + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="26">' + BranchdataArr[i].panno + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="27">' + BranchdataArr[i].Emp_PIC + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="28">' + BranchdataArr[i].ftplocation + '</td>';
                        results += '<td scope="row" class="1" style="text-align:center;">' + BranchdataArr[i].LevelType + '</td>';
                        results += '<td scope="row" class="1" style="display:none" class="29">' + BranchdataArr[i].otpstatus + '</td>';
                        var rndmnum = Math.floor((Math.random() * 10) + 1);
                        var img_url = BranchdataArr[i].ftplocation + BranchdataArr[i].Emp_PIC + '?v=' + rndmnum;
                        if (BranchdataArr[i].Emp_PIC != "") {
                            results += '<td><img data-imagezoom="true" class="img-circle img-responsive" id="main_img_item" alt="Item Image" src="' + img_url + '" style="width: 35px; height: 35px;border: 2px solid gray;" /></td>';
                        }
                        else {
                            results += '<td><img data-imagezoom="true" class="img-circle img-responsive" id="main_img_item" alt="Item Image" src="Images/dummy_image.jpg" style="width: 35px; height: 35px;border: 2px solid gray;" /></td>';
                        }
                        results += '<td data-title="Capacity"  class="16">' + status + '</td>';
                        results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                        l = l + 1;
                        if (l == 4) {
                            l = 0;
                        }
                    }
                }
                results += '</table></div>';
            }
            else {
                var l = 0;
                var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
                var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col">BranchName</th><th scope="col">Emp Name</th><th scope="col">Mobile</th><th scope="col">Email</th><th scope="col">User Name</th><th scope="col">Level Type</th><th scope="col">Image</th><th scope="col">Flag</th><th scope="col"></th></tr></thead></tbody>';
                for (var i = 0; i < BranchdataArr.length; i++) {
                    if (BranchName == BranchdataArr[i].BranchSno) {
                        var status = 'InActive';
                        if (BranchdataArr[i].Loginsflag == '1') {
                            status = 'Active';
                        }
                        results += '<tr style="background-color:' + COLOR[l] + '">';
                        results += '<td data-title="Capacity"  class="2">' + BranchdataArr[i].Branchname + '</td>';
                        results += '<td scope="row"  class="4"><i class="glyphicon glyphicon-user" aria-hidden="true"></i>&nbsp;<span style="font-weight:600;" id="4">' + BranchdataArr[i].EmpName + '</span></td>';
                        results += '<td scope="row" class="5"><i class="glyphicon glyphicon-phone" aria-hidden="true"></i>&nbsp;<span style="font-weight:600;" id="5">' + BranchdataArr[i].Mobno + '</span></td>';
                        results += '<td scope="row" class="7"><i class="glyphicon glyphicon-envelope" aria-hidden="true"></i>&nbsp;<span id="7">' + BranchdataArr[i].email + '</span></td>';
                        results += '<td data-title="Capacity" style="display:none" class="8">' + BranchdataArr[i].previouscompny + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="9">' + BranchdataArr[i].refference + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="10">' + BranchdataArr[i].department + '</td>';
                        results += '<td data-title="Capacity"  class="3">' + BranchdataArr[i].UserName + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="11">' + BranchdataArr[i].dtrgt + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="12">' + BranchdataArr[i].wtrgt + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="13">' + BranchdataArr[i].mtrgt + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="14">' + BranchdataArr[i].BranchSno + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="15">' + BranchdataArr[i].sno + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="17">' + BranchdataArr[i].Password + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="18">' + BranchdataArr[i].street + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="19">' + BranchdataArr[i].city + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="20">' + BranchdataArr[i].mandal + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="21">' + BranchdataArr[i].district + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="22">' + BranchdataArr[i].state + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="23">' + BranchdataArr[i].pincode + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="26">' + BranchdataArr[i].panno + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="27">' + BranchdataArr[i].Emp_PIC + '</td>';
                        results += '<td data-title="Capacity" style="display:none" class="28">' + BranchdataArr[i].ftplocation + '</td>';
                        results += '<td scope="row" class="1" style="text-align:center;">' + BranchdataArr[i].LevelType + '</td>';
                        results += '<td scope="row" class="1" style="display:none" class="29">' + BranchdataArr[i].otpstatus + '</td>';
                        var rndmnum = Math.floor((Math.random() * 10) + 1);
                        var img_url = BranchdataArr[i].ftplocation + BranchdataArr[i].Emp_PIC + '?v=' + rndmnum;
                        if (BranchdataArr[i].Emp_PIC != "") {
                            results += '<td><img class="img-circle img-responsive" id="main_img_item" alt="Item Image" src="Images/dummy_image.jpg" style="width: 35px; height: 35px;border: 2px solid gray;" /></td>';
                        }
                        else {
                            results += '<td><img class="img-circle img-responsive" id="main_img_item" alt="Item Image" src="Images/dummy_image.jpg" style="width: 35px; height: 35px;border: 2px solid gray;" /></td>';
                        }
                        results += '<td data-title="Capacity"  class="16">' + status + '</td>';
                        results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                        l = l + 1;
                        if (l == 4) {
                            l = 0;
                        }
                    }
                }
                results += '</table></div>';
            }
            }
            $("#div_Empdata").html(results);
        }
        var BranchDataTable = [];
        function BindSalesOffice() {
            BranchDataTable = [];
            var ddlBranchName = document.getElementById('ddlBranchName');
            var length = ddlBranchName.options.length;
            ddlBranchName.options.length = null;
//            var opt = document.createElement('option');
//            opt.innerHTML = "select";
//            ddlBranchName.appendChild(opt);
            for (var i = 0; i < BranchdataArr.length; i++) {
                if (BranchdataArr[i].Branchname != null) {
                    if (BranchDataTable.indexOf(BranchdataArr[i].BranchSno) == -1) {
                        var opt = document.createElement('option');
                        opt.innerHTML = BranchdataArr[i].Branchname;
                        opt.value = BranchdataArr[i].BranchSno;
                        ddlBranchName.appendChild(opt);
                        BranchDataTable.push(BranchdataArr[i].BranchSno);
                    }
                }
            }
        }
        function searchempEmployeName() {
            var compiledList1 = [];
            for (var i = 0; i < BranchdataArr.length; i++) {
                var EmpName = BranchdataArr[i].EmpName;
                compiledList1.push(EmpName);
            }
            $('#txtEmployeeSearch').autocomplete({
                source: compiledList1,
                change: changeEmployeeName,
                autoFocus: true
            });
        }
        
        var serial23;
        function getme(thisid) {

            $('#add_Employee').css('display', 'none');
            $('#emp_showlogs').css('display', 'none');
            $('#div_Customer').css('display', 'block');
            $('#div_Empdata').css('display', 'none');
            var LevelType = $(thisid).parent().parent().children('.1').html();
            var Branchname = $(thisid).parent().parent().children('.2').html();
            var EmpName = $(thisid).parent().parent().find('#4').html();

            var Mobno = $(thisid).parent().parent().find('#5').html();
            var email = $(thisid).parent().parent().find('#7').html();
            var previouscompny = $(thisid).parent().parent().children('.8').html();
            var refference = $(thisid).parent().parent().children('.9').html();
            var department = $(thisid).parent().parent().children('.10').html();
            var UserName = $(thisid).parent().parent().children('.3').html();
            var dtrgt = $(thisid).parent().parent().children('.11').html();
            var wtrgt = $(thisid).parent().parent().children('.12').html();
            var mtrgt = $(thisid).parent().parent().children('.13').html();
            var BranchSno = $(thisid).parent().parent().children('.14').html();
            var sno = $(thisid).parent().parent().children('.15').html();
            var status = $(thisid).parent().parent().children('.16').html();
            var Password = $(thisid).parent().parent().children('.17').html();
            var street = $(thisid).parent().parent().children('.18').html();
            var city = $(thisid).parent().parent().children('.19').html();
            var mandal = $(thisid).parent().parent().children('.20').html();
            var district = $(thisid).parent().parent().children('.21').html();
            var state = $(thisid).parent().parent().children('.22').html();
            var pincode = $(thisid).parent().parent().children('.23').html();
            var panno = $(thisid).parent().parent().children('.26').html();

            var Emp_PIC = $(thisid).parent().parent().children('.27').html();
            var ftplocation = $(thisid).parent().parent().children('.28').html();
            var otpstatus = $(thisid).parent().parent().children('.29').html();

            serial23 = sno;
//            BranchSno = BranchSno;
//            BranchName = Branchname;
            $("#cmb_emp_level").find("option:contains('" + LevelType + "')").each(function () {
                if ($(this).text() == LevelType) {
                    $(this).attr("selected", "selected");
                }
            });
            $("#cmb_emp_department").find("option:contains('" + department + "')").each(function () {
                if ($(this).text() == department) {
                    $(this).attr("selected", "selected");
                }
            });
            document.getElementById('ddlBranchName').value = BranchSno;
            document.getElementById('txt_emp_name').value = EmpName;
            document.getElementById('txt_emp_phno').value = Mobno;
            document.getElementById('txt_emp_email').value = email;
            document.getElementById('txt_emp_prevcompany').value = previouscompny;
            document.getElementById('txt_emp_refference').value = refference;
            document.getElementById('txt_emp_dtarget').value = dtrgt;
            document.getElementById('txt_emp_wtarget').value = wtrgt;
            document.getElementById('txt_emp_mtarget').value = mtrgt;
            document.getElementById('lblEmpName').innerHTML = EmpName;
            document.getElementById('lblEmployeeEmailId').innerHTML = email;
            document.getElementById('lblEmployeeMobileNumber').innerHTML = Mobno;
            document.getElementById('txtStreet').value = street;
            document.getElementById('txtCity').value = city;
            document.getElementById('txtMandal').value = mandal;
            document.getElementById('txtdistrict').value = district;
            document.getElementById('txt_state').value = state;
            document.getElementById('txtPincode').value = pincode;
            document.getElementById('txtpanno').value = panno;
            document.getElementById('txt_emp_username').value = UserName;
            document.getElementById('txt_emp_password').value = Password;
            document.getElementById('btn_emp_save').innerHTML = "MODIFY";
            var Flag = 1;
            if (status == "InActive") {
                Flag = 0;
            }
            document.getElementById('cmb_emp_flag').value = status;
            document.getElementById('ddlotpstatus').value = otpstatus;
            
            var rndmnum = Math.floor((Math.random() * 10) + 1);
            img_url = ftplocation + Emp_PIC + '?v=' + rndmnum;
            if (Emp_PIC != "") {
                $('#main_img').attr('src', img_url).width(200).height(200);
            }
            else {
                $('#main_img').attr('src', 'Images/Employeeimg.jpg').width(200).height(200);
            }
        }
    </script>
    <script type="text/javascript">
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
        function filldepartments() {
            var data = { 'operation': 'get_employees_department' };
            var s = function (msg) {
                if (msg) {
                    fill_employee_departments(msg);
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
        function fill_employee_departments(msg) {
            var cmbempdepartment = document.getElementById('cmb_emp_department');
            var length = cmbempdepartment.options.length;
            cmbempdepartment.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            cmbempdepartment.appendChild(opt);

            for (var i = 0; i < msg.length; i++) {
                if (msg[i].Departmentname != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].Departmentname;
                    opt.value = msg[i].sno;
                    cmbempdepartment.appendChild(opt);
                }
            }
        }
        function emptrgts_categorynames() {
            emptrgts_branchesfill();
            bindemployee_prdttrgt_management();
            var data = { 'operation': 'intialize_branchesproducts_branchname' };
            var s = function (msg) {
                if (msg) {
                    fillemptrgts_categorynames(msg);
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
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillemptrgts_categorynames(msg) {
            var productcategory = document.getElementById('cmb_empprdt_category');
            var length = productcategory.options.length;
            document.getElementById("cmb_empprdt_category").options.length = null;
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
        function empprdt_cateegoryname_onchange() {
            var cmbcatgryname = document.getElementById("cmb_empprdt_category").value;
            var data = { 'operation': 'get_product_subcategory_data', 'cmbcatgryname': cmbcatgryname };
            var s = function (msg) {
                if (msg) {
                    fillempprdt_subcatgry(msg);
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
            //callHandler(data, s, e);
            CallHandler(data, s, e);
        }
        function fillempprdt_subcatgry(msg) {
            var prdtsubcategory = document.getElementById('cmb_empprdt_subcatgry');
            var length = prdtsubcategory.options.length;
            document.getElementById("cmb_empprdt_subcatgry").options.length = null;

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
        function productsdata_subcategory_onchange() {
            var cmbsubcatgryname = document.getElementById("cmb_empprdt_subcatgry").value;
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
                if (x.status && x.status == 400) {
                    alert(x.responseText);
                    window.location.assign("Login.aspx");
                }
                else {
                    alert("something went wrong");
                }
            };
            //callHandler(data, s, e);
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandler(data, s, e);
        };
        function fillproductsdata_products(msg) {

            var cmbprdtname = document.getElementById('cmb_empprdt_prdtsname');
            var length = cmbprdtname.options.length;
            document.getElementById("cmb_empprdt_prdtsname").options.length = null;
            document.getElementById("cmb_empprdt_prdtsname").value = "select";
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
        function Employeedetailsclick() {
            var checkedbranch = document.getElementById("ddlBranchName").value;
            if (checkedbranch == "" || checkedbranch == "select") {
                alert("Please Select Branch Name");
                return false;
            }
            //ddlBranchProductNameChange()
            $('#add_Employee').css('display', 'none');
            $('#emp_showlogs').css('display', 'none');
            $('#div_Customer').css('display', 'block');
            $('#div_Empdata').css('display', 'none');
        }

        function hasExtension(fileName, exts) {
            return (new RegExp('(' + exts.join('|').replace(/\./g, '\\.') + ')$')).test(fileName);
        }
        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#main_img,#img_1').attr('src', e.target.result).width(200).height(200);
                    //                    $('#main_img1,#img_1').attr('src', e.target.result).width(200).height(200);
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

            //            var refno = document.getElementById('lbl_branchsno').innerHTML;
            //            if (refno == "" || refno == "select") {
            //                alert("Please Select Agent Name");
            //                $("#ddlAgentName").focus();
            //                return false;
            //            }
            var Data = new FormData();
            Data.append("operation", "Employee_profile_pic_files_upload");
            Data.append("sno", serial23);
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Employee Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Masters</a></li>
            <li><a href="#">Employee Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Employee Master Details
                </h3>
            </div>
            <div class="box-body">
                <input id="sessionInput" type="hidden" value='<%= Session["LevelType"] %>' />
                <div id="tabs" style="width: 100%; height: 100%;">
                    <%--     <ul style="font-size: 12px;">
                        <li><a href="#branchesManage" onclick="return branchmgntmapping();">Branches Management</a></li>
                        <li><a href="#branchProducts" onclick="return branchprdtcategories();">Branch Products</a></li>
                        <li><a href="#AgentDocuments" onclick="return branchDocuments();" style="display: none;">
                            Agent Documents</a></li>
                    </ul>--%>
                    <div class="box-body" id="div_branchdata">
                        <div id="emp_showlogs" style="text-align: center;">
                            <table align="center">
                                <tr>
                                    <td>
                                        <label>
                                            Branch Name:</label>
                                    </td>
                                    <td style="padding: 14px;">
                                        <select id="ddlBranchName" class="form-control" onchange="fillBranchNamechange();">
                                        </select>
                                    </td>
                                    <td>
                                        <input type="text" id="txtEmployeeSearch" class="form-control" style="height: 28px;
                                            opacity: 1.0; width: 150px;" placeholder="Search Employee Name" />
                                    </td>
                                    <td>
                                        <button type="button" class="btn btn-info btn-flat" style="height: 30px;">
                                            <i class="fa fa-search" aria-hidden="true"></i>
                                        </button>
                                    </td>
                                    <td style="width: 45%;">
                                    </td>
                                    <td>
                                        <%--<input id="add_Employee" type="button" class="btn btn-primary" name="submit" value="Add Employee" onclick="Employeedetailsclick();">--%>
                                        <div id="add_Employee" align="right" class="input-group" style="display: block;">
                                            <div class="input-group-addon" style="width: 100px;">
                                                <span class="glyphicon glyphicon-plus-sign" onclick="Employeedetailsclick();"></span>
                                                <span onclick="Employeedetailsclick();">Add Employee</span>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="div2" style="width: 350px; height: 300px; overflow: auto; font-size: 14px;
                                            display: none;">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="div_Customer" style="display: none;">
                            <div class="row">
                                <div class="col-sm-12 col-xs-12">
                                    <div class="well panel panel-default" style="padding: 0px;">
                                        <div class="panel-body">
                                            <div class="row">
                                                <div class="col-sm-4" style="width: 100%;">
                                                    <div class="row">
                                                        <div class="col-xs-12 col-sm-3 text-center">
                                                            <div class="pictureArea1">
                                                                <h2 style="font-size: 16px; font-weight: 600">
                                                                    EMPLOYEE PHOTO</h2>
                                                                <img class="center-block img-circle img-thumbnail img-responsive profile-img" id="main_img"
                                                                    alt="Agent Image" src="Images/Employeeimg.jpg" style="border-radius: 5px; width: 200px;
                                                                    height: 200px; border-radius: 50%;" />
                                                                <div class="photo-edit-admin">
                                                                    <a onclick="getFile();" class="photo-edit-icon-admin" title="Change Profile Picture"
                                                                        data-toggle="modal" data-target="#photoup"><i class="fa fa-pencil" style="padding-top: 10px;
                                                                            padding-left: 25px;">CHOOSE EMPLOYEE PHOTO</i></a>
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
                                                        <div class="col-xs-12 col-sm-9">
                                                            <h2 class="text-primary">
                                                                <b><span class="glyphicon glyphicon-user"></span>
                                                                    <label style="padding-left: 20px; font-weight: 700;" id="lblEmpName">
                                                                    </label>
                                                                </b>
                                                            </h2>
                                                            <p>
                                                                <strong>Email ID : <span style="color: Red;">*</span></strong>
                                                                <label id="lblEmployeeEmailId">
                                                                </label>
                                                            </p>
                                                            <p>
                                                                <strong>Mobile No :<span style="color: Red;">*</span> </strong>
                                                                <label id="lblEmployeeMobileNumber">
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
                            <div class="box box-info">
                                <div id="AgentDetails" style="display: block;">
                                    <div class="row">
                                        <div class="col-sm-6" style="width: 100%;" id="att_emp">
                                            <div class="box box-solid box-success">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <i style="padding-right: 5px;" class="fa fa-fw fa-user"></i>Employee Details</h3>
                                                    <div class="box-tools pull-right">
                                                        <button class="btn btn-box-tool" data-widget="collapse">
                                                            <i class="fa fa-minus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <div class="box-body no-padding">
                                                        <div id="divCustomerData">
                                                            <table align="center">
                                                                <%-- <tr align="left">
                                <td>
                                            <label id="lblBranch"  style="font-weight:bold">
                                                Select BranchName:</label>
                                        </td>
                                <td>
                                    <input id="txtBranchName" type="text" class="form-control" name="submit" placeholder="Select BranchName"/>
                                </td>
                                <td>
                                    <input type="hidden" id="txtHiddenBranchid"  class="form-control"/>
                                    </td>
                                </tr>--%>
                                                                <tr>
                                                                    <td>
                                                                        <label id="lblleveltype">
                                                                            Level Type</label><span style="color: red; font-weight: bold">*</span>
                                                                        <select id="cmb_emp_level" class="form-control">
                                                                            <option value="0">Select</option>
                                                                            <option value="2">Manager</option>
                                                                            <option value="3">Admin</option>
                                                                            <option value="15">SalesRepresentative</option>
                                                                            <option value="16">SalesManager</option>
                                                                            <option value="6">Opperations</option>
                                                                            <option value="7">Users</option>
                                                                            <option value="8">Accountant</option>
                                                                            <option value="9">SODispatcher</option>
                                                                            <option value="10">PlantDispatcher</option>
                                                                            <option value="11">AccountsOfficer</option>
                                                                            <option value="13">Security</option>
                                                                            <option value="14">Agent</option>
                                                                        </select>
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label id="lblemp_name">
                                                                            Employee Name</label><span style="color: red; font-weight: bold">*</span>
                                                                        <input type="text" id="txt_emp_name" class="form-control"    placeholder="Enter Employee Name" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label id="lblemp_username">
                                                                            User Name</label><span style="color: red; font-weight: bold">*</span>
                                                                        <input type="text" id="txt_emp_username" class="form-control"    placeholder="Enter User Name" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label id="lblemp_password">
                                                                            Password</label><span style="color: red; font-weight: bold">*</span>
                                                                        <input type="password" id="txt_emp_password" class="form-control" placeholder="Enter Password" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label id="lblemp_previouscompany">
                                                                            Previous Company</label>
                                                                        <input type="text" id="txt_emp_prevcompany" class="form-control" placeholder="Enter Previous Company" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label id="lblemp_reffrence">
                                                                            reference</label>
                                                                        <input type="text" id="txt_emp_refference" class="form-control" placeholder="Enter reference" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label id="lblemp_department">
                                                                            Department</label>
                                                                        <select id="cmb_emp_department" class="form-control" placeholder="Enter Department">
                                                                        </select>
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label id="lblemp_daytarget">
                                                                            Day Target</label>
                                                                        <input type="text" id="txt_emp_dtarget" class="form-control" placeholder="Enter Day Target"
                                                                            onkeypress="return isNumber(event);" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label id="lblemp_weektarget">
                                                                            Week Target</label>
                                                                        <input type="text" id="txt_emp_wtarget" class="form-control" placeholder="Enter Week Target"
                                                                            onkeypress="return isNumber(event);" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label id="lblemp_monthtarget">
                                                                            Month Target</label>
                                                                        <input type="text" id="txt_emp_mtarget" class="form-control" placeholder="Enter Month Target"
                                                                            onkeypress="return isNumber(event);" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label id="lblempflag">
                                                                            Flag</label><span style="color: red; font-weight: bold">*</span>
                                                                        <select id="cmb_emp_flag" class="form-control">
                                                                            <option>Active</option>
                                                                            <option>InActive</option>
                                                                        </select>
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label id="Label1">
                                                                            OTP Status</label><span style="color: red; font-weight: bold">*</span>
                                                                        <select id="ddlotpstatus" class="form-control">
                                                                            <option value="1">Active</option>
                                                                            <option value="0">InActive</option>
                                                                        </select>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <br />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6" style="width: 100%;" id="Div1">
                                            <div class="box box-solid box-success">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <i style="padding-right: 5px;" class="fa fa-fw fa-user"></i>Employee Address Details</h3>
                                                    <div class="box-tools pull-right">
                                                        <button class="btn btn-box-tool" data-widget="collapse">
                                                            <i class="fa fa-minus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <div class="box-body no-padding">
                                                        <div id="div_CustomerAddres">
                                                            <table align="center">
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            Street</label>
                                                                        <input id="txtStreet" type="text" name="Street"    class="form-control" placeholder="Enter Street" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            City</label><span style="color: red; font-weight: bold">*</span>
                                                                        <input id="txtCity" type="text" name="City"     class="form-control" placeholder="Enter City Name" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            Mandal</label>
                                                                        <input id="txtMandal" type="text" name="Mandal"    class="form-control" placeholder="Enter Mandal" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            District</label><span style="color: red; font-weight: bold">*</span>
                                                                        <input id="txtdistrict" type="text" name="district"    class="form-control" placeholder="Enter district" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            State Name:</label><span style="color: red; font-weight: bold">*</span>
                                                                        <input type="text" id="txt_state" class="form-control"    placeholder="Enter State Name" />
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
                                                                        <label id="lblemp_phno">
                                                                            Mobile Number</label><span style="color: red; font-weight: bold">*</span>
                                                                        <input type="number" id="txt_emp_phno" class="form-control" placeholder="Enter Mobile Number"
                                                                            onkeypress="return isNumber(event);" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label id="lblemp_email">
                                                                            Email ID</label>
                                                                        <input type="text" id="txt_emp_email" class="form-control" placeholder="Enter Email" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            PAN No</label>
                                                                        <input id="txtpanno" type="text" name="PINCode" class="form-control" placeholder="Enter PAN Number" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <table align="center">
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div class="input-group">
                                                                <div class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-ok" id="btn_emp_save1" onclick="btn_lctngradd_Click()">
                                                                    </span><span id="btn_emp_save" onclick="btn_lctngradd_Click()">SAVE</span>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td style="padding-left: 7px;">
                                                            <div class="input-group">
                                                                <div class="input-group-close">
                                                                    <span class="glyphicon glyphicon-remove" id='btn_emp_clear1' onclick="employee_management_clear()">
                                                                    </span><span id='btn_emp_clear' onclick="employee_management_clear()">CLEAR</span>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <%--<input type="button" value="SAVE" id="btn_emp_save" class="btn btn-primary" onclick="btn_lctngradd_Click();" />
                                            <input type="button" value="CLEAR" id="btn_emp_clear" class="btn btn-danger" onclick="employee_management_clear();" />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div id="div_empmaster_table" style="padding: 0px 10px 100px 10px">
                            <div id="div_Empdata" style="padding: 0px 10px 100px; display: block;">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    </section>
</asp:Content>
