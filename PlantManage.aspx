<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PlantManage.aspx.cs" Inherits="Manage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="JIC/JIC.js?v=101" type="text/javascript"></script>
    <script src="JSF/imagezoom.js" type="text/javascript"></script>
    <script src="Plant/Script/fleetscript.js?v=3006" type="text/javascript"></script>
    <style type="text/css">
        label
        {
            display: inline-block;
            max-width: 100%;
            margin-bottom: 5px;
            font-weight: 700;
        }
    </style>
    <script type="text/javascript">
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
        $(function () {
            Bindbranchmanagement();
            branches_manages_salestype();
            get_bank_details();
            get_state_details();
        });
        function get_state_details() {
            var data = { 'operation': 'get_state_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillstates(msg);
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
        function fillstates(msg) {
            var ddlstate = document.getElementById('slct_state');
            var length = ddlstate.options.length;
            ddlstate.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            ddlstate.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].statename != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].statename;
                    opt.value = msg[i].sno;
                    ddlstate.appendChild(opt);
                }
            }
        }
        function branches_manages_salestype() {
            var data = { 'operation': 'intialize_branchesmanages_salestype' };
            var s = function (msg) {
                if (msg) {
                    fillbranches_manage_salestype(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillbranches_manage_salestype(msg) {
            var branchtype = document.getElementById('cmb_salestype');
            var length = branchtype.options.length;
            branchtype.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            branchtype.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].salestype != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].salestype;
                    opt.value = msg[i].sno;
                    branchtype.appendChild(opt);
                }
            }
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
            var ddlCustomerBankName = document.getElementById('ddlCustomerBankName');
            var length = ddlCustomerBankName.options.length;
            ddlCustomerBankName.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            ddlCustomerBankName.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].name != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].name;
                    opt.value = msg[i].sno;
                    ddlCustomerBankName.appendChild(opt);
                }
            }
        }
        var BranchDataTable = [];
        function BindSalesOffice() {
            BranchDataTable = [];
            var ddlBranchName = document.getElementById('ddlBranchName');
            var length = ddlBranchName.options.length;
            ddlBranchName.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            ddlBranchName.appendChild(opt);
            for (var i = 0; i < BranchdataArr.length; i++) {
                if (BranchdataArr[i].SuperBranchName != null) {
                    if (BranchDataTable.indexOf(BranchdataArr[i].SuperSno) == -1) {
                        var opt = document.createElement('option');
                        opt.innerHTML = BranchdataArr[i].SuperBranchName;
                        opt.value = BranchdataArr[i].SuperSno;
                        ddlBranchName.appendChild(opt);
                        BranchDataTable.push(BranchdataArr[i].SuperSno);
                    }
                }
            }
        }
        function searchagentdetais() {
            var compiledList = [];
            var Branchid = document.getElementById('ddlBranchName').value;
            for (var i = 0; i < BranchdataArr.length; i++) {
                if (Branchid == BranchdataArr[i].SuperSno) {
                    var brncName = BranchdataArr[i].BranchName;
                    compiledList.push(brncName);
                }
            }
            $('#txtAgentName').autocomplete({
                source: compiledList,
                change: changeagentname,
                autoFocus: true
            });
        }

        function changeagentname() {
            BindingBranchManagement();
        }
        function BindingBranchManagement() {
            var BranchName = document.getElementById('txtAgentName').value;
            if (BranchName == "") {
                var l = 0;
                var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
                var k = 1;
                var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">Sno</th><th scope="col" class="thcls">Customer Code</th><th scope="col" class="thcls">Customer Name</th><th scope="col" class="thcls">Cust Code</th><th scope="col" class="thcls">Mobile</th><th scope="col" class="thcls">E-Mail</th><th scope="col" class="thcls">Sales Type</th><th scope="col" class="thcls">Flag</th><th scope="col" class="thcls" style="display:none;">Image</th><th scope="col"></th></tr></thead></tbody>';
                for (var i = 0; i < BranchdataArr.length; i++) {

                    if (BranchdataArr[i].Flag == '1') {
                        var status = 'Active';
                    }
                    else {
                        var status = 'InActive';
                    }
                    results += '<tr style="background-color:' + COLOR[l] + '">';
                    results += '<td scope="row" class="1" style="text-align:center;">' + k + '</td>';
                    results += '<td data-title="Capacity" class="2">' + BranchdataArr[i].refsno + '</td>';
                    results += '<td scope="row" class="3"><i class="glyphicon glyphicon-user" style="color: cadetblue;" aria-hidden="true"></i>&nbsp;<span class="tdmaincls" id="3">' + BranchdataArr[i].BranchName + '</span></td>';
                    results += '<td  class="29">' + BranchdataArr[i].customercode + '</td>';
                    results += '<td scope="row" class="6"><i class="glyphicon glyphicon-phone-alt" style="color: cadetblue;" aria-hidden="true"></i>&nbsp;<span class="tdmaincls" id="6">' + BranchdataArr[i].phone + '</span></td>';
                    results += '<td scope="row" class="42"><i class="fa fa-envelope" style="color: cadetblue;" aria-hidden="true"></i>&nbsp;<span class="tdmaincls" id="42">' + BranchdataArr[i].email + '</span></td>';
                    //                  results += '<td style="display:none" class="42">' + BranchdataArr[i].email + '</td>';
                    results += '<td  class="4">' + BranchdataArr[i].Salestype + '</td>';
                    results += '<td  class="11">' + status + '</td>';
                    results += '<td style="display:none" class="5">' + BranchdataArr[i].collectiontyp + '</td>';
                    results += '<td style="display:none"  class="7">' + BranchdataArr[i].address + '</td>';
                    results += '<td style="display:none" class="8">' + BranchdataArr[i].DTarget + '</td>';
                    results += '<td style="display:none" class="9">' + BranchdataArr[i].WTarget + '</td>';
                    results += '<td style="display:none" class="10">' + BranchdataArr[i].MTarget + '</td>';
                    results += '<td style="display:none" class="21">' + BranchdataArr[i].TBranchName + '</td>';
                    results += '<td style="display:none" class="13">' + BranchdataArr[i].lat + '</td>';
                    results += '<td style="display:none"  class="14">' + BranchdataArr[i].lng + '</td>';
                    results += '<td style="display:none" class="15">' + BranchdataArr[i].Otherbrands + '</td>';
                    results += '<td style="display:none" class="16">' + BranchdataArr[i].duelimit + '</td>';
                    results += '<td style="display:none" class="17">' + BranchdataArr[i].Salesrep + '</td>';
                    results += '<td  style="display:none" class="18">' + BranchdataArr[i].Due_Limit_Type + '</td>';
                    results += '<td style="display:none" class="19">' + BranchdataArr[i].LimitDays + '</td>';
                    results += '<td style="display:none"  class="22">' + BranchdataArr[i].branchcode + '</td>';
                    results += '<td style="display:none" class="23">' + BranchdataArr[i].tinno + '</td>';
                    results += '<td style="display:none" class="24">' + BranchdataArr[i].ledgerdr + '</td>';
                    results += '<td style="display:none" class="25">' + BranchdataArr[i].incentive + '</td>';
                    results += '<td  style="display:none" class="26">' + BranchdataArr[i].panno + '</td>';
                    results += '<td style="display:none" class="27">' + BranchdataArr[i].state + '</td>';
                    results += '<td  style="display:none" class="28">' + BranchdataArr[i].whcode + '</td>';
                    results += '<td style="display:none" class="30">' + BranchdataArr[i].ladger_dr_code + '</td>';
                    results += '<td style="display:none" class="31">' + BranchdataArr[i].Branchid + '</td>';
                    results += '<td style="display:none" class="32">' + BranchdataArr[i].Agent_PIC + '</td>';
                    results += '<td style="display:none" class="33">' + BranchdataArr[i].ftplocation + '</td>';
                    results += '<td style="display:none" class="34">' + BranchdataArr[i].street + '</td>';
                    results += '<td style="display:none" class="35">' + BranchdataArr[i].city + '</td>';
                    results += '<td style="display:none" class="36">' + BranchdataArr[i].mandal + '</td>';
                    results += '<td style="display:none" class="37">' + BranchdataArr[i].district + '</td>';
                    results += '<td style="display:none" class="38">' + BranchdataArr[i].state + '</td>';
                    results += '<td style="display:none" class="39">' + BranchdataArr[i].pincode + '</td>';
                    results += '<td style="display:none" class="40">' + BranchdataArr[i].tinno + '</td>';
                    results += '<td style="display:none" class="41">' + BranchdataArr[i].cst + '</td>';
                    results += '<td style="display:none" class="43">' + BranchdataArr[i].doorno + '</td>';
                    results += '<td style="display:none" class="44">' + BranchdataArr[i].area + '</td>';
                    results += '<td style="display:none" class="45">' + BranchdataArr[i].gstin + '</td>';
                    results += '<td style="display:none" class="50">' + BranchdataArr[i].regtype + '</td>';
                    results += '<td style="display:none" class="46">' + BranchdataArr[i].Bankid + '</td>';
                    results += '<td style="display:none" class="47">' + BranchdataArr[i].ifsccode + '</td>';
                    results += '<td style="display:none" class="48">' + BranchdataArr[i].customeraccno + '</td>';
                    var rndmnum = Math.floor((Math.random() * 10) + 1);
                    var img_url = BranchdataArr[i].ftplocation + BranchdataArr[i].Agent_PIC + '?v=' + rndmnum;
                    if (BranchdataArr[i].Agent_PIC != "") {
                        results += '<td style="display:none;"><img data-imagezoom="true" class="img-circle img-responsive" id="main_img_item" alt="Item Image" src="' + img_url + '" style="width: 35px; height: 35px;border: 2px solid gray;" /></td>';
                    }
                    else {
                        results += '<td style="display:none;"><img data-imagezoom="true" class="img-circle img-responsive" id="main_img_item" alt="Item Image" src="Images/dummy_image.jpg" style="width: 35px; height: 35px;border: 2px solid gray;" /></td>';
                    }

                    results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getmeCustomerData(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>';

                    results += '<td style="display:none" class="20">' + BranchdataArr[i].sno + '</td></tr>';
                    l = l + 1;
                    if (l == 4) {
                        l = 0;
                    }
                    k++;
                }
                results += '</table></div>';
            }
            else {
                var J = 0;
                var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];

                var k = 1;
                var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
                results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">Sno</th><th scope="col" class="thcls">Branch Code</th><th scope="col" class="thcls">Customer Name</th><th scope="col" class="thcls">Customer Code</th><th scope="col" class="thcls">Mobile</th><th scope="col" class="thcls">E-Mail</th><th scope="col" class="thcls">Sales Type</th><th scope="col" class="thcls">Flag</th><th scope="col" class="thcls">Image</th><th scope="col" ></th></tr></thead></tbody>';
                for (var i = 0; i < BranchdataArr.length; i++) {
                    if (BranchName == BranchdataArr[i].BranchName) {
                        if (BranchdataArr[i].Flag == '1') {
                            var status = 'Active';
                        }
                        else {
                            var status = 'InActive';
                        }
                        results += '<tr style="background-color:' + COLOR[J] + '">';
                        results += '<td scope="row" class="1" style="text-align:center;">' + k + '</td>';
                        results += '<td data-title="Capacity" class="2">' + BranchdataArr[i].refsno + '</td>';
                        results += '<td scope="row"  class="3"><i class="glyphicon glyphicon-user" style="color: cadetblue;" aria-hidden="true"></i>&nbsp;<span class="tdmaincls" id="3">' + BranchdataArr[i].BranchName + '</span></td>';
                        results += '<td  class="29">' + BranchdataArr[i].customercode + '</td>';
                        results += '<td scope="row" class="6"><i class="glyphicon glyphicon-phone-alt" style="color: cadetblue;" aria-hidden="true"></i>&nbsp;<span class="tdmaincls" id="6">' + BranchdataArr[i].phone + '</span></td>';
                        results += '<td scope="row" class="42"><i class="fa fa-envelope" style="color: cadetblue;" aria-hidden="true"></i>&nbsp;<span class="tdmaincls" id="42">' + BranchdataArr[i].email + '</span></td>';
                        results += '<td  class="4">' + BranchdataArr[i].Salestype + '</td>';
                        results += '<td  class="11">' + status + '</td>';
                        results += '<td style="display:none" class="5">' + BranchdataArr[i].collectiontyp + '</td>';
                        results += '<td style="display:none"  class="7">' + BranchdataArr[i].address + '</td>';
                        results += '<td style="display:none" class="8">' + BranchdataArr[i].DTarget + '</td>';
                        results += '<td style="display:none" class="9">' + BranchdataArr[i].WTarget + '</td>';
                        results += '<td style="display:none" class="10">' + BranchdataArr[i].MTarget + '</td>';
                        results += '<td style="display:none" class="21">' + BranchdataArr[i].TBranchName + '</td>';
                        results += '<td style="display:none" class="13">' + BranchdataArr[i].lat + '</td>';
                        results += '<td style="display:none"  class="14">' + BranchdataArr[i].lng + '</td>';
                        results += '<td style="display:none" class="15">' + BranchdataArr[i].Otherbrands + '</td>';
                        results += '<td style="display:none" class="16">' + BranchdataArr[i].duelimit + '</td>';
                        results += '<td style="display:none" class="17">' + BranchdataArr[i].Salesrep + '</td>';
                        results += '<td  style="display:none" class="18">' + BranchdataArr[i].Due_Limit_Type + '</td>';
                        results += '<td style="display:none" class="19">' + BranchdataArr[i].LimitDays + '</td>';
                        results += '<td style="display:none"  class="22">' + BranchdataArr[i].branchcode + '</td>';
                        results += '<td style="display:none" class="23">' + BranchdataArr[i].tinno + '</td>';
                        results += '<td style="display:none" class="24">' + BranchdataArr[i].ledgerdr + '</td>';
                        results += '<td style="display:none" class="25">' + BranchdataArr[i].incentive + '</td>';
                        results += '<td  style="display:none" class="26">' + BranchdataArr[i].panno + '</td>';
                        results += '<td style="display:none" class="27">' + BranchdataArr[i].state + '</td>';
                        results += '<td  style="display:none" class="28">' + BranchdataArr[i].whcode + '</td>';
                        results += '<td style="display:none" class="29">' + BranchdataArr[i].customercode + '</td>';
                        results += '<td style="display:none" class="30">' + BranchdataArr[i].ladger_dr_code + '</td>';
                        results += '<td style="display:none" class="31">' + BranchdataArr[i].Branchid + '</td>';
                        results += '<td style="display:none" class="32">' + BranchdataArr[i].Agent_PIC + '</td>';
                        results += '<td style="display:none" class="33">' + BranchdataArr[i].ftplocation + '</td>';
                        results += '<td style="display:none" class="34">' + BranchdataArr[i].street + '</td>';
                        results += '<td style="display:none" class="35">' + BranchdataArr[i].city + '</td>';
                        results += '<td style="display:none" class="36">' + BranchdataArr[i].mandal + '</td>';
                        results += '<td style="display:none" class="37">' + BranchdataArr[i].district + '</td>';
                        results += '<td style="display:none" class="38">' + BranchdataArr[i].state + '</td>';
                        results += '<td style="display:none" class="39">' + BranchdataArr[i].pincode + '</td>';
                        results += '<td style="display:none" class="40">' + BranchdataArr[i].tinno + '</td>';
                        results += '<td style="display:none" class="41">' + BranchdataArr[i].cst + '</td>';
                        results += '<td style="display:none" class="43">' + BranchdataArr[i].doorno + '</td>';
                        results += '<td style="display:none" class="44">' + BranchdataArr[i].area + '</td>';
                        results += '<td style="display:none" class="45">' + BranchdataArr[i].gstin + '</td>';
                        results += '<td style="display:none" class="50">' + BranchdataArr[i].regtype + '</td>';
                        results += '<td style="display:none" class="46">' + BranchdataArr[i].Bankid + '</td>';
                        results += '<td style="display:none" class="47">' + BranchdataArr[i].ifsccode + '</td>';
                        results += '<td style="display:none" class="48">' + BranchdataArr[i].customeraccno + '</td>';
                        var rndmnum = Math.floor((Math.random() * 10) + 1);
                        var img_url = BranchdataArr[i].ftplocation + BranchdataArr[i].Agent_PIC + '?v=' + rndmnum;
                        if (BranchdataArr[i].Agent_PIC != "") {
                            results += '<td><img data-imagezoom="true" class="img-circle img-responsive" id="main_img_item" alt="Item Image" src="' + img_url + '" style="width: 35px; height: 35px;border: 2px solid gray;" /></td>';
                        }
                        else {
                            results += '<td><img data-imagezoom="true" class="img-circle img-responsive" id="main_img_item" alt="Item Image" src="Images/dummy_image.jpg" style="width: 35px; height: 35px;border: 2px solid gray;" /></td>';
                        }
                        results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getmeCustomerData(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>';
                        results += '<td style="display:none" class="20">' + BranchdataArr[i].sno + '</td></tr>';
                        J = J + 1;
                        if (J == 4) {
                            J = 0;
                        }
                        k++;
                    }
                }
                results += '</table></div>';
            }
            $("#div_BranchData").html(results);
        }

        function branchesmanagement() {
            var checkedbranch = document.getElementById('ddlBranchName').value;
            if (checkedbranch == "") {
                alert("BranchName Can Not Be Empty");
                return false;
            }
            var branchcode = document.getElementById('txt_branchcode').value;
            var whccode = document.getElementById('txtWhccode').value;
            var customercode = document.getElementById('txtCustomerCode').value;
            var brnchsname = document.getElementById('txt_branchname').value;
            if (brnchsname == "") {
                alert("Please Enter Customer Name");
                $("#txt_branchname").focus();
                return false;
            }
            var ledgerdr = document.getElementById('txt_ledgerdr').value;
            var LedgerCode = document.getElementById('txtLedgerCode').value;
            var tally_branchname = document.getElementById('txt_tally_branchname').value;
            var txtsr = document.getElementById('txtsr').value;
            var cmbsalestype = document.getElementById('cmb_salestype').value;
            if (cmbsalestype == "" || cmbsalestype == "select") {
                alert("Please Select SalesType");
                $("#cmb_salestype").focus();
                return false;
            }
            var cmbcollectiontype = document.getElementById('cmb_collectiontype').value;
            if (cmbcollectiontype == "" || cmbcollectiontype == "select") {
                alert("Please Select CollectionType");
                $("#cmb_branchesManageflag").focus();
                return false;
            }
            var cmblimittype = document.getElementById('cmblimittype').value;
            var txtduelimit = document.getElementById('txtduelimit').value;
            var branchesManageflag = document.getElementById('cmb_branchesManageflag').value;
            if (branchesManageflag == "") {
                alert("Please Select Flag");
                $("#cmb_branchesManageflag").focus();
                return false;
            }
            var branchesManage_Phone = document.getElementById('txt_branch_mobile').value;
            if (branchesManage_Phone == "") {
                alert("Please enter Phone");
                $("#txt_branch_mobile").focus();
                return false;
            }
            var operationtype = document.getElementById('btn_brnchs_mng_save').innerHTML;
            var brnchlat = document.getElementById('txtbrnchlat').value;
            var brnchlong = document.getElementById('txtbrnchlong').value;
            var doorno = document.getElementById('txt_doorno').value;
            var area = document.getElementById('txt_area').value;
            if (area == "") {
                alert("Please enter area");
                $("#txt_area").focus();
                return false;
            }
            var street = document.getElementById('txtStreet').value;
            var city = document.getElementById('txtCity').value;
            if (city == "") {
                alert("Please enter city");
                $("#txtCity").focus();
                return false;
            }
            var mandal = document.getElementById('txtMandal').value;
            var district = document.getElementById('txtdistrict').value;
            if (district == "") {
                alert("Please enter district");
                $("#txtdistrict").focus();
                return false;
            }
            var state = document.getElementById('slct_state').value;
            if (state == "" || state == "select") {
                alert("Please select state");
                $("#slct_state").focus();
                return false;
            }
            var pincode = document.getElementById('txtPincode').value;
            if (pincode == "") {
                alert("Please enter pincode");
                $("#txtPincode").focus();
                return false;
            }
            var tinno = document.getElementById('txt_tinno').value;
            var panno = document.getElementById('txtpanno').value;
            var cst = document.getElementById('txtcst').value;
            var email = document.getElementById('txtemail').value;
            var refno = document.getElementById('lblSno').innerHTML;
            var GSTIN = document.getElementById('txt_GST').value;
            var regtype = document.getElementById('slct_regtype').value;
            var Bankid = document.getElementById('ddlCustomerBankName').value;
            var ifsccode = document.getElementById('txtCustomerIfsc').value;
            var customeraccno = document.getElementById('txtCutomerAccNo').value;


            var mmname = document.getElementById('txt_mm').value;
            var mename = document.getElementById('txt_mexecutive').value;
            var amount = document.getElementById('txt_amount').value;
            var amountsince = document.getElementById('txt_amountsince').value;


            var data = { 'operation': 'Save_branchsmanagement_click', 'checkedbranch': checkedbranch, 'GSTIN': GSTIN, 'regtype': regtype, 'doorno': doorno, 'area': area, 'branchcode': branchcode, 'whccode': whccode, 'customercode': customercode, 'brnchsname': brnchsname, 'ledgerdr': ledgerdr, 'LedgerCode': LedgerCode, 'tally_branchname': tally_branchname, 'txtsr': txtsr, 'cmbsalestype': cmbsalestype, 'cmbcollectiontype': cmbcollectiontype, 'cmblimittype': cmblimittype, 'txtduelimit': txtduelimit, 'branchesManageflag': branchesManageflag, 'branchesManage_Phone': branchesManage_Phone, 'operationtype': operationtype, 'brnchlat': brnchlat, 'brnchlong': brnchlong, 'street': street, 'city': city, 'mandal': mandal, 'district': district, 'state': state, 'pincode': pincode, 'tinno': tinno, 'panno': panno, 'cst': cst, 'email': email, 'Bankid': Bankid, 'ifsccode': ifsccode, 'customeraccno': customeraccno, 'brncsno': refno, 'mmname': mmname, 'mename': mename, 'amount': amount, 'amountsince': amountsince };
            //var data = { 'operation': 'branchsmanagement', 'checkedbranch': checkedbranch, 'GSTIN': GSTIN, 'doorno': doorno, 'area': area, 'branchcode': branchcode, 'whccode': whccode, 'customercode': customercode, 'brnchsname': brnchsname, 'ledgerdr': ledgerdr, 'LedgerCode': LedgerCode, 'tally_branchname': tally_branchname, 'txtsr': txtsr, 'cmbsalestype': cmbsalestype, 'cmbcollectiontype': cmbcollectiontype, 'cmblimittype': cmblimittype, 'txtduelimit': txtduelimit, 'branchesManageflag': branchesManageflag, 'branchesManage_Phone': branchesManage_Phone, 'operationtype': operationtype, 'brnchlat': brnchlat, 'brnchlong': brnchlong, 'street': street, 'city': city, 'mandal': mandal, 'district': district, 'state': state, 'pincode': pincode, 'tinno': tinno, 'panno': panno, 'cst': cst, 'email': email, 'brncsno': refno };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    branchesmanagementclear();
                    Bindbranchmanagement();
                    BindSalesOffice();
                    Bindbranchmanagement();
                    branches_manages_salestype();
                    get_bank_details();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function branchesmanagementclear() {
            $('#add_customer').css('display', 'block');
            $('#emp_showlogs').css('display', 'block');
            $('#div_Customer').css('display', 'none');
            $('#div_BranchData').css('display', 'block');
            document.getElementById('txtsr').value = "";
            document.getElementById('txt_branchname').value = "";
            document.getElementById('txt_tally_branchname').value = "";
            document.getElementById('cmb_salestype').selectedIndex = 0;
            document.getElementById('cmb_collectiontype').selectedIndex = 0;
            document.getElementById('txt_branch_mobile').value = "";
            document.getElementById('txtLedgerCode').value = "";
            document.getElementById('txtduelimit').value = "";
            document.getElementById('btn_brnchs_mng_save').innerHTML = "SAVE";
            document.getElementById('txtbrnchlat').value = "0";
            document.getElementById('txtbrnchlong').value = "0";
            //akbar
            document.getElementById('txtWhccode').value = "";
            document.getElementById('txtCustomerCode').value = "";
            document.getElementById('txt_branchcode').value = "";
            document.getElementById('txt_tinno').value = "";
            document.getElementById('txt_ledgerdr').value = "";
            // document.getElementById('slct_state').value = "";
            document.getElementById('txtpanno').value = "";
            document.getElementById('lblCustomerName').innerHTML = "";
            document.getElementById('lbladdressName').innerHTML = "";
            document.getElementById('lblCustomerEmailId').innerHTML = "";
            document.getElementById('lblCustomerMobileNumber').innerHTML = "";
            document.getElementById('main_img').value = "Images/Employeeimg.jpg";
            document.getElementById('txtCity').value = "";
            document.getElementById('txtStreet').value = "";
            document.getElementById('txtMandal').value = "";
            document.getElementById('txtdistrict').value = "";
            document.getElementById('slct_state').value = "0";
            document.getElementById('txtPincode').value = "";
            document.getElementById('txt_tinno').value = "";
            document.getElementById('txtcst').value = "";
            document.getElementById('txtemail').value = "";
            document.getElementById('txt_GST').value = "";
            document.getElementById('txt_doorno').value = "";
            document.getElementById('txt_area').value = "";
            document.getElementById('ddlCustomerBankName').value = "";
            document.getElementById('txtCustomerIfsc').value = "";
            document.getElementById('txtCutomerAccNo').value = "";
        }
        var BranchdataArr = [];
        function Bindbranchmanagement() {
            var data = { 'operation': 'Updatebranchmanagement' };
            var s = function (msg) {
                if (msg) {
                    BranchdataArr = msg;
                    BindingBranchManagement();
                    BindSalesOffice();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var CustomerSno = 0;
        var Customer_Code = 0;
        var BranchID = 0;
        function getmeCustomerData(thisid) {
            BindSalesOffice();
            $('#add_customer').css('display', 'none');
            $('#emp_showlogs').css('display', 'none');
            $('#div_Customer').css('display', 'block');
            $('#div_BranchData').css('display', 'none');
            var refsno = $(thisid).parent().parent().children('.2').html();
            CustomerSno = refsno;
            var BranchName = $(thisid).parent().parent().find('#3').html();
            var Salestype = $(thisid).parent().parent().children('.4').html();
            var collectiontyp = $(thisid).parent().parent().children('.5').html();
            var phone = $(thisid).parent().parent().find('#6').html();
            var address = $(thisid).parent().parent().children('.7').html();
            var DTarget = $(thisid).parent().parent().children('.8').html();
            var WTarget = $(thisid).parent().parent().children('.9').html();
            var MTarget = $(thisid).parent().parent().children('.10').html();
            var status = $(thisid).parent().parent().children('.11').html();
            var lat = $(thisid).parent().parent().children('.13').html();
            var lng = $(thisid).parent().parent().children('.14').html();
            var Otherbrands = $(thisid).parent().parent().children('.15').html();
            var duelimit = $(thisid).parent().parent().children('.16').html();
            var Salesrep = $(thisid).parent().parent().children('.17').html();
            var Due_Limit_Type = $(thisid).parent().parent().children('.18').html();
            var LimitDays = $(thisid).parent().parent().children('.19').html();
            var TBranchName = $(thisid).parent().parent().children('.21').html();
            var branchcode = $(thisid).parent().parent().children('.22').html();
            var tinno = $(thisid).parent().parent().children('.23').html();
            var ledgerdr = $(thisid).parent().parent().children('.24').html();
            var incentive = $(thisid).parent().parent().children('.25').html();
            var panno = $(thisid).parent().parent().children('.26').html();
            var state = $(thisid).parent().parent().children('.27').html();
            var whccode = $(thisid).parent().parent().children('.28').html();
            var customercode = $(thisid).parent().parent().children('.29').html();
            Customer_Code = customercode;
            var ledgercode = $(thisid).parent().parent().children('.30').html();
            var Branchid = $(thisid).parent().parent().children('.31').html();
            BranchID = Branchid;
            var Agent_PIC = $(thisid).parent().parent().children('.32').html();
            var ftplocation = $(thisid).parent().parent().children('.33').html();
            var street = $(thisid).parent().parent().children('.34').html();
            var city = $(thisid).parent().parent().children('.35').html();
            var mandal = $(thisid).parent().parent().children('.36').html();
            var district = $(thisid).parent().parent().children('.37').html();
            var state = $(thisid).parent().parent().children('.38').html();
            var pincode = $(thisid).parent().parent().children('.39').html();
            var tinno = $(thisid).parent().parent().children('.40').html();
            var cst = $(thisid).parent().parent().children('.41').html();
            var email = $(thisid).parent().parent().find('#42').html();
            var doorno = $(thisid).parent().parent().children('.43').html();
            var area = $(thisid).parent().parent().children('.44').html();
            var gst = $(thisid).parent().parent().children('.45').html();
            var Bankid = $(thisid).parent().parent().children('.46').html();
            var ifsccode = $(thisid).parent().parent().children('.47').html();
            var customeraccno = $(thisid).parent().parent().children('.48').html();
            var regtype = $(thisid).parent().parent().children('.50').html();
            document.getElementById('ddlCustomerBankName').value = Bankid;
            document.getElementById('txtCustomerIfsc').value = ifsccode;
            document.getElementById('txtCutomerAccNo').value = customeraccno;
            document.getElementById('cmb_salestype').value = Salestype;
            document.getElementById('txtStreet').value = street;
            document.getElementById('txtCity').value = city;
            document.getElementById('txtMandal').value = mandal;
            document.getElementById('txtdistrict').value = district;
            document.getElementById('slct_state').value = state;
            document.getElementById('txtPincode').value = pincode;
            document.getElementById('txt_tinno').value = tinno;
            document.getElementById('txtcst').value = cst;
            document.getElementById('txtemail').value = email;
            document.getElementById('ddlBranchName').value = Branchid;
            document.getElementById('txtWhccode').value = whccode;
            document.getElementById('txtCustomerCode').value = customercode;
            document.getElementById('txt_branchcode').value = branchcode;
            document.getElementById('txt_tinno').value = tinno;
            document.getElementById('txt_ledgerdr').value = ledgerdr;
            document.getElementById('slct_state').value = state;
            document.getElementById('txtpanno').value = panno;
            document.getElementById('txt_branchname').value = BranchName;
            document.getElementById('txt_tally_branchname').value = TBranchName;
            document.getElementById('txtsr').value = Salesrep;
            document.getElementById('txt_branch_mobile').value = phone;
            document.getElementById('txtLedgerCode').value = ledgercode;
            document.getElementById('lblCustomerName').innerHTML = BranchName;
            document.getElementById('lbladdressName').innerHTML = street + "," + mandal + "," + district + "," + city + "," + state;
            document.getElementById('lblCustomerEmailId').innerHTML = email;
            document.getElementById('lblCustomerMobileNumber').innerHTML = phone;
            document.getElementById('cmb_collectiontype').value = collectiontyp;
            document.getElementById('cmblimittype').selectedIndex = 0;
            document.getElementById('txt_GST').value = gst;
            document.getElementById('slct_regtype').value = regtype;
            document.getElementById('txt_doorno').value = doorno;
            document.getElementById('txt_area').value = area;
            document.getElementById('lblSno').innerHTML = refsno;
            document.getElementById('lbl_branchsno').innerHTML = BranchID;
            if (Due_Limit_Type == "Amount") {
                document.getElementById('txtduelimit').value = duelimit;
            }
            if (Due_Limit_Type == "Days") {
                document.getElementById('txtduelimit').value = LimitDays;
            }
            $("#cmb_salestype").find("option:contains('" + Salestype + "')").each(function () {
                if ($(this).text() == Salestype) {
                    $(this).attr("selected", "selected");
                }
            });
            document.getElementById('cmb_branchesManageflag').value = status;
            document.getElementById('txtStreet').value = street;
            document.getElementById('txtCity').value = city;
            document.getElementById('txtMandal').value = mandal;
            document.getElementById('txtdistrict').value = district;
            document.getElementById('slct_state').value = state;
            document.getElementById('txtPincode').value = pincode;
            document.getElementById('txt_tinno').value = tinno;
            document.getElementById('txtcst').value = cst;
            document.getElementById('txtemail').value = email;
            document.getElementById('btn_brnchs_mng_save').innerHTML = "MODIFY";
            document.getElementById('txtbrnchlat').value = lat;
            document.getElementById('txtbrnchlong').value = lng;
            var rndmnum = Math.floor((Math.random() * 10) + 1);
            img_url = ftplocation + Agent_PIC + '?v=' + rndmnum;
            if (Agent_PIC != "") {
                $('#main_img').attr('src', img_url).width(200).height(200);
            }
            else {
                $('#main_img').attr('src', 'Images/Employeeimg.jpg').width(200).height(200);
            }
        }
        function AgentDetailsClick() {
            $('#Customer_Product').css('display', 'none');
            $('#AgentDetails').css('display', 'block');
            $('#div_Branch_Products').css('display', 'none');
            $("#div_Documents").css("display", "none");
            $("#CustomerProductFillform").css("display", "none");
            $('#div_CustomerAmountDetails').css('display', 'none');
            $('#div_CustomerBalance').css('display', 'none');
        }
        function customerdetailsclick() {
            var checkedbranch = document.getElementById('ddlBranchName').value; ;
            if (checkedbranch == "" || checkedbranch == "select") {
                alert("Please Select Branch Name");
                return false;
            }
            $('#add_customer').css('display', 'none');
            $('#emp_showlogs').css('display', 'none');
            $('#div_Customer').css('display', 'block');
            $('#div_BranchData').css('display', 'none');
            $("#div_Documents").css("display", "none");
            $("#div_CustomerBalance").css("display", "none");
        }
        function AgentProductsClick() {
            BindBranchProducts();
            branches_products_branchname();
            $('#Customer_Product').css('display', 'block');
            $('#AgentDetails').css('display', 'none');
            $('#div_Branch_Products').css('display', 'block');
            $('#CustomerProductFillform').css('display', 'none');
            $("#div_Documents").css("display", "none");
            $('#div_CustomerAmountDetails').css('display', 'none');
            $('#div_CustomerBalance').css('display', 'none');
        }
        function change_Documents() {
            getcustomer_Uploaded_Documents(CustomerSno)
            $("li").removeClass("active");
            $("li").addClass("");
            $('#AgentDetails').css('display', 'none');
            $('#Customer_Product').css('display', 'none');
            $('#CustomerProductFillform').css('display', 'none');
            $("#id_tab_documents").removeClass("");
            $("#id_tab_documents").addClass("active");
            $("#div_basic_details").css("display", "none");
            $("#btn_modify").css("display", "none");
            $("#div_Documents").css("display", "block");
            $('#div_Branch_Products').css('display', 'none');
            $('#Customer_Product').css('display', 'none');
            $('#div_CustomerAmountDetails').css('display', 'none');
            $('#div_CustomerBalance').css('display', 'none');

        }
        function change_Amount() {
            getcustomer_Amount_Details(CustomerSno)
            $("li").removeClass("active");
            $("li").addClass("");
            $('#AgentDetails').css('display', 'none');
            $('#Customer_Product').css('display', 'none');
            $('#CustomerProductFillform').css('display', 'none');
            $("#id_tab_documents").removeClass("");
            $("#id_tab_documents").addClass("active");
            $("#div_basic_details").css("display", "none");
            $("#btn_modify").css("display", "none");
            $("#div_Documents").css("display", "none");
            $('#div_Branch_Products').css('display', 'none');
            $('#Customer_Product').css('display', 'none');
            $('#div_CustomerAmountDetails').css('display', 'block');
            $('#div_CustomerBalance').css('display', 'block');



        }

        function showCustomerProductDetaislclick() {
            $('#CustomerProductFillform').css('display', 'block');
            $('#Customer_Product').css('display', 'none');
            $('#div_Branch_Products').css('display', 'none');
        }
        //        function changeagentproductname() {
        //            var ProductName = document.getElementById('txt_Product_search').value;
        //            for (var i = 0; i < AgentProductArr.length; i++) {
        //                if (ProductName == AgentProductArr[i].ProductName) {
        //                    document.getElementById('txt_Product_search').value = AgentProductArr[i].ProductName;
        //                    document.getElementById('txtHiddenName').value = AgentProductArr[i].pdtsno;
        //                }
        //            }
        //        }


        function getBranchProducts() {
            var AgentId = document.getElementById('txtHiddenName').value;
            var data = { 'operation': 'updatebrnchprdt_check_togrid', 'checkedbranch': AgentId };
            var s = function (msg) {
                if (msg.length > 0) {
                    BindingBranchProducts(msg);
                }
                else {
                    BindBranchProducts();
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function branches_products_branchname() {
            var data = { 'operation': 'intialize_branchesproducts_branchname' };
            var s = function (msg) {
                if (msg) {
                    fillbranches_products_branchname(msg);
                }
                else {

                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        };

        function fillbranches_products_branchname(msg) {
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
            var subcatgry = document.getElementById('cmb__brnch_subcatgry');
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
            var cmbprdtname = document.getElementById('cmb_productname');
            var length = cmbprdtname.options.length;
            cmbprdtname.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            cmbprdtname.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].ProductName != null && msg[i].cmb_productname == null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].ProductName;
                    opt.value = msg[i].sno;
                    cmbprdtname.appendChild(opt);
                }
            }
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
            Data.append("operation", "Agent_profile_pic_files_upload");
            Data.append("sno", CustomerSno);
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
        function getFile_doc() {
            document.getElementById("FileUpload1").click();
        }
        function upload_Customer_Document_Info() {
            var documentid = document.getElementById('ddl_documenttype').value;
            var documentname = document.getElementById('ddl_documenttype').selectedOptions[0].innerText;
            if (documentid == null || documentid == "" || documentid == "Select Document Type") {
                document.getElementById("ddl_documenttype").focus();
                alert("Please select Document Type");
                return false;
            }
            var documentExists = 0;
            $('#tbl_documents tr').each(function () {
                var selectedrow = $(this);
                var document_manager_id = selectedrow[0].cells[0].innerHTML;
                if (document_manager_id == documentid) {
                    alert(documentname + "  Already Exist For This Employee");
                    documentExists = 1;
                    return false;
                }

            });
            if (documentExists == 1) {
                return false;
            }
            var Data = new FormData();
            Data.append("operation", "save_customerdocument");
            Data.append("CustomerSno", CustomerSno);
            Data.append("Customer_Code", Customer_Code);
            Data.append("documentname", documentname);
            Data.append("documentid", documentid);
            var fileUpload = $("#FileUpload1").get(0);
            var files = fileUpload.files;
            for (var i = 0; i < files.length; i++) {
                Data.append(files[i].name, files[i]);
            }

            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    getcustomer_Uploaded_Documents(CustomerSno);
                }
            };
            var e = function (x, h, e) {
                alert(e.toString());
            };
            callHandler_nojson_post(Data, s, e);
        }
        function readURL_doc(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.readAsDataURL(input.files[0]);
                document.getElementById("FileUpload_div").innerHTML = input.files[0].name;
            }
        }




        function getcustomer_Uploaded_Documents(CustomerSno) {
            var data = { 'operation': 'getcustomer_Uploaded_Documents', 'CustomerSno': CustomerSno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillcustomer_Uploaded_Documents(msg);
                    }
                    else {
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillcustomer_Uploaded_Documents(msg) {

            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col" style="text-align:center">Document Name</th><th scope="col">Photo</th><th scope="col"></th></tr></thead></tbody>';
            var k = 1;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr><td>' + k++ + '</td>';
                var path = img_url = msg[i].ftplocation + msg[i].photo;
                var documentid = msg[i].documentid;
                var documentName = "";
                if (documentid == "1") {
                    documentName = "DrivingLicence";
                }
                if (documentid == "2") {
                    documentName = "Aadarcard";
                }
                if (documentid == "3") {
                    documentName = "Voterid";
                }
                if (documentid == "4") {
                    documentName = "PanCard";
                }
                if (documentid == "5") {
                    documentName = "Passport";
                }
                if (documentid == "6") {
                    documentName = "Passbook";
                }
                results += '<th scope="row" class="1" style="text-align:center;">' + documentName + '</th>';
                results += '<td data-title="brandstatus" class="2"><img src=' + path + '  style="cursor:pointer;height:200px;width:200px;border-radius: 5px;"/></td>';
                results += '</tr>';
            }
            results += '</table></div>';
            $("#div_documents_table").html(results);
        }

        function getcustomer_Amount_Details(CustomerSno) {
            var temp = "PlantManage";
            var data = { 'operation': 'GetAgentClosingAmount', 'CustomerSno': CustomerSno, 'temp': temp };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillAgentAmountDetails(msg);
                    }
                    else {
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function fillAgentAmountDetails(msg) {
            var l = 0;
            var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col"></th><th scope="col">Customer Name</th><th scope="col">Amount</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '"><td><input id="btn_poplate" type="button"  onclick="getamountDetails(this)" name="submit" class="btn btn-primary" value="View" /></td>';
                results += '<td scope="row" class="1" >' + msg[i].Branchname + '</td>';
                results += '<td scope="row" class="4" >' + msg[i].PaidAmount + '</td>';
                results += '<td style="display:none" class="3">' + msg[i].Branchid + '</td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_CustomerAmountDetails").html(results);
        }
        function getamountDetails(thisid) {
            $('#div_routewisemainCompare').css('display', 'block');
            var data = { 'operation': 'GetAgent_Transaction', 'CustomerSno': CustomerSno};
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillAmountDetails(msg);
                    }
                    else {
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillAmountDetails(msg) {
            var l = 0;
            var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Date</th><th scope="col">SaleValue</th><th scope="col">DebitAmount</th><th scope="col">OpeningAmount</th><th scope="col">TotalAmount</th><th scope="col">PaidAmount</th><th scope="col">Jv/IncentivAmount</th><th scope="col">ClosingAmount</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row" class="1" >' + msg[i].DelivaryDate + '</td>';
                results += '<td scope="row" class="4" >' + msg[i].SaleValue + '</td>';
                results += '<td scope="row" class="4" >' + msg[i].DebitAmount + '</td>';
                results += '<td scope="row" class="4" >' + msg[i].OpeningAmount + '</td>';
                results += '<td scope="row" class="4" >' + msg[i].TotalAmount + '</td>';
                results += '<td scope="row" class="4" >' + msg[i].PaidAmount + '</td>';
                results += '<td scope="row" class="4" >' + msg[i].JvAmount + '</td>';
                results += '<td scope="row" class="4" >' + msg[i].ClosingAmount + '</td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_routewiseCompare").html(results);
        }

        function close_routewiseCompare() {
            $('#div_routewisemainCompare').css('display', 'none');
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Customer Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Masters</a></li>
            <li><a href="#">Customer Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Customer Master Details
                </h3>
            </div>
            <div class="box-body">
                <div id="tabs" style="width: 100%; height: 100%;">
                    <div class="box-body" id="div_branchdata">
                        <div id="emp_showlogs" style="text-align: center;">
                            <table align="center">
                                <tr>
                                    <td>
                                        <label>
                                            Branch Name:</label>
                                    </td>
                                    <td style="padding: 14px;">
                                        <select id="ddlBranchName" class="form-control" onchange="searchagentdetais();">
                                        </select>
                                    </td>
                                    <td>
                                        <input type="text" id="txtAgentName" class="form-control" style="height: 28px; opacity: 1.0;
                                            width: 165px;" placeholder="Search Customer Name" />
                                    </td>
                                    <td>
                                        <span class="input-group-btn">
                                            <button type="button" class="btn btn-info btn-flat" style="height: 30px;">
                                                <i class="fa fa-search" aria-hidden="true"></i>
                                            </button>
                                        </span>
                                    </td>
                                    <td style="width: 43%">
                                    </td>
                                    <td>
                                        <div id="Customersowlogs" align="center" class="input-group" style="display: block;">
                                            <div class="input-group-addon">
                                                <span class="glyphicon glyphicon-plus-sign" onclick="customerdetailsclick();"></span>
                                                <span onclick="customerdetailsclick();">Add Customer</span>
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
                                                                    CUSTOMER PHOTO</h2>
                                                                <img class="center-block img-circle img-thumbnail img-responsive profile-img" id="main_img"
                                                                    alt="Agent Image" src="Images/Employeeimg.jpg" style="border-radius: 5px; width: 200px;
                                                                    height: 200px; border-radius: 50%;" />
                                                                <div class="photo-edit-admin">
                                                                    <a onclick="getFile();" class="photo-edit-icon-admin" title="Change Profile Picture"
                                                                        data-toggle="modal" data-target="#photoup"><i class="fa fa-pencil" style="padding-top: 10px;
                                                                            padding-left: 25px;">CHOOSE CUSTOMER PHOTO</i></a>
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
                                                                    <label id="lblCustomerName">
                                                                    </label>
                                                                </b>
                                                            </h2>
                                                            <p>
                                                                <strong>Address : <span style="color: Red;">*</span></strong>
                                                                <label style="padding-left: 20px; font-weight: 700;" id="lbladdressName">
                                                                </label>
                                                            </p>
                                                            <p>
                                                                <strong>Email ID : <span style="color: Red;">*</span></strong>
                                                                <label id="lblCustomerEmailId">
                                                                </label>
                                                            </p>
                                                            <p>
                                                                <strong>Mobile No :<span style="color: Red;">*</span> </strong>
                                                                <label id="lblCustomerMobileNumber">
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
                            <div>
                                <ul class="nav nav-tabs">
                                    <li id="id_tab_Personal" class="active"><a data-toggle="tab" href="#" onclick="AgentDetailsClick()">
                                        <i class="fa fa-street-view"></i>&nbsp;&nbsp;Customer Details</a></li>
                                    <li id="id_tab_Products" class=""><a data-toggle="tab" href="#" onclick="AgentProductsClick()">
                                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Customer Product Data</a></li>
                                    <li id="id_tab_documents" class=""><a data-toggle="tab" href="#" onclick="change_Documents()">
                                        <i class="fa fa-file-text"></i>Customer Documents</a></li>
                                    <li id="Li1" class=""><a data-toggle="tab" href="#" onclick="change_Amount()"><i
                                        class="fa fa-file-text"></i>Customer Amount</a></li>
                                </ul>
                            </div>
                            <div class="box box-info">
                                <div id="AgentDetails" style="display: block;">
                                    <div class="row">
                                        <div class="col-sm-6" style="width: 100%;" id="att_emp">
                                            <div class="box box-solid box-success">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <i style="padding-right: 5px;" class="fa fa-fw fa-user"></i>Customer Details</h3>
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
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            Customer Name</label><span style="color: red; font-weight: bold">*</span>
                                                                        <input type="text" id="txt_branchname" class="form-control" placeholder="Enter Customer Name" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            Customer Code</label>
                                                                        <input type="text" id="txtCustomerCode" class="form-control" placeholder="Enter CustomerCode" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            WH CODE</label>
                                                                        <input type="text" id="txtWhccode" class="form-control" placeholder="Enter WH Code" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            Branch Code</label>
                                                                        <input type="text" id="txt_branchcode" class="form-control" placeholder="Enter Branch Code" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            Tally Name</label>
                                                                        <input type="text" id="txt_tally_branchname" class="form-control" placeholder="Enter Tally Branch Name" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            SR Name</label>
                                                                        <input type="text" id="txtsr" class="form-control" placeholder="Enter SR" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            Ledger(Dr)</label>
                                                                        <input type="text" id="txt_ledgerdr" class="form-control" placeholder="Enter Ledger(Dr)" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            Ledger Code</label>
                                                                        <input type="text" id="txtLedgerCode" class="form-control" placeholder="Enter Ledger Code" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            Sales Type</label><span style="color: red; font-weight: bold">*</span>
                                                                        <select id="cmb_salestype" class="form-control">
                                                                        </select>
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            Collection Type</label><span style="color: red; font-weight: bold">*</span>
                                                                        <select id="cmb_collectiontype" class="form-control">
                                                                            <option>select</option>
                                                                            <option>DUE</option>
                                                                            <option>CASH</option>
                                                                        </select>
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            Due Limit Type</label>
                                                                        <select id="cmblimittype" class="form-control">
                                                                            <option>Amount</option>
                                                                            <option>Days</option>
                                                                        </select>
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            Due Limit</label>
                                                                        <input type="text" id="txtduelimit" class="form-control" onkeypress="return isNumber(event);"
                                                                            placeholder="Enter Due Limit" value="0" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            Latitude</label>
                                                                        <input type="text" id="txtbrnchlat" class="form-control" placeholder="Enter Latitude"
                                                                            value="0" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            Longitude</label>
                                                                        <input type="text" id="txtbrnchlong" class="form-control" value="0" placeholder="Enter Longitude" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            Flag</label><span style="color: red; font-weight: bold">*</span>
                                                                        <select id="cmb_branchesManageflag" class="form-control">
                                                                            <option>Active</option>
                                                                            <option>InActive</option>
                                                                        </select>
                                                                    </td>
                                                                </tr>
                                                                <tr></tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            Marketing Manager</label><span style="color: red; font-weight: bold">*</span>
                                                                        <input type="text" id="txt_mm" class="form-control" placeholder="Enter  Marketing Manager Name" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            Marketing Executive </label>
                                                                        <input type="text" id="txt_mexecutive" class="form-control" placeholder="Enter Executive Name" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                     <td>
                                                                        <label>
                                                                            Type</label><span style="color: red; font-weight: bold">*</span>
                                                                        <select id="slctmmtype" class="form-control">
                                                                            <option>Cr</option>
                                                                            <option>Dr</option>
                                                                        </select>
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            Amount</label>
                                                                        <input type="text" id="txt_amount" class="form-control" placeholder="Enter Amount" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            Amount Since</label>
                                                                        <input type="date" id="txt_amountsince" class="form-control"/>
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
                                                        <i style="padding-right: 5px;" class="fa fa-fw fa-user"></i>Customer Address Details</h3>
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
                                                                            Door No</label>
                                                                        <input id="txt_doorno" type="text" name="Street" class="form-control" placeholder="Enter Door No" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            Area</label>
                                                                        <input id="txt_area" type="text" name="City" class="form-control" placeholder="Enter Area" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            Street</label>
                                                                        <input id="txtStreet" type="text" name="Street" class="form-control" placeholder="Enter Street" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            City</label><span style="color: red; font-weight: bold">*</span>
                                                                        <input id="txtCity" type="text" name="City" class="form-control" placeholder="Enter City Name" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            Mandal</label>
                                                                        <input id="txtMandal" type="text" name="Mandal" class="form-control" placeholder="Enter Mandal" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            District</label><span style="color: red; font-weight: bold">*</span>
                                                                        <input id="txtdistrict" type="text" name="district" class="form-control" placeholder="Enter District" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            State Name:</label><span style="color: red; font-weight: bold">*</span>
                                                                        <%--<input type="text" id="txt_state" class="form-control" placeholder="Enter State Name" />--%>
                                                                        <select id="slct_state" class="form-control">
                                                                        </select>
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
                                                                            Tin Number:</label>
                                                                        <input type="text" id="txt_tinno" class="form-control" placeholder="Enter Tin Number" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            CST Number
                                                                        </label>
                                                                        <input id="txtcst" type="text" name="PINCode" class="form-control" placeholder="Enter CST Number" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            Mobile Number:</label><span style="color: red; font-weight: bold">*</span>
                                                                        <input type="text" id="txt_branch_mobile" class="form-control" onkeypress="return isNumber(event);"
                                                                            placeholder="Enter Mobile Number" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            PAN No</label>
                                                                        <input id="txtpanno" type="text" name="PINCode" class="form-control" placeholder="Enter PAN Number" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            Email Address</label>
                                                                        <input id="txtemail" type="text" name="TinNumber" class="form-control" placeholder="Enter Email Address" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            GSTIN</label><span style="color: red; font-weight: bold">*</span>
                                                                        <input id="txt_GST" type="text" name="PINCode" class="form-control" placeholder="Enter GSTIN" />
                                                                    </td>
                                                                    <td style="width: 4px;">
                                                                    </td>
                                                                    <td>
                                                                        <label>
                                                                            Reg Type</label><span style="color: red; font-weight: bold">*</span>
                                                                        <select id="slct_regtype" class="form-control">
                                                                            <option value="0">Select Reg Type</option>
                                                                            <option value="Casual Taxable Person">Casual Taxable Person</option>
                                                                            <option value="Composition Levy">Composition Levy</option>
                                                                            <option value="Government Department or PSU">Government Department or PSU</option>
                                                                            <option value="Non Resident Taxable Person">Non Resident Taxable Person</option>
                                                                            <option value="Regular/TDS/ISD">Regular/TDS/ISD</option>
                                                                            <option value="UN Agency or Embassy">UN Agency or Embassy</option>
                                                                            <option value="Special Economic Zone">Special Economic Zone</option>
                                                                        </select>
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none;">
                                                                    <td>
                                                                        <label id="lbl_branchsno">
                                                                        </label>
                                                                    </td>
                                                                </tr>
                                                                <tr style="display: none;">
                                                                    <td>
                                                                        <label id="lblSno">
                                                                        </label>
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
                                        <div class="col-sm-6" style="width: 100%;" id="Div4">
                                            <div class="box box-solid box-success">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <i style="padding-right: 5px;" class="fa fa-fw fa-user"></i>Bank Details</h3>
                                                    <div class="box-tools pull-right">
                                                        <button class="btn btn-box-tool" data-widget="collapse">
                                                            <i class="fa fa-minus"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <div class="box-body">
                                                    <div class="box-body no-padding">
                                                        <div id="divCustomerBank">
                                                            <table align="center">
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            BankName</label><span style="color: red; font-weight: bold">*</span>
                                                                        <select id="ddlCustomerBankName" class="form-control">
                                                                            <option>select</option>
                                                                        </select>
                                                                        <td style="width: 4px;">
                                                                        </td>
                                                                        <td>
                                                                            <label>
                                                                                IFSC Code</label><span style="color: red; font-weight: bold">*</span>
                                                                            <input id="txtCustomerIfsc" type="text" name="IFSC" class="form-control" placeholder="Enter IFSC Code" />
                                                                        </td>
                                                                        <td style="width: 4px;">
                                                                        </td>
                                                                        <td>
                                                                            <label>
                                                                                AccountNumber</label><span style="color: red; font-weight: bold">*</span>
                                                                            <input id="txtCutomerAccNo" type="text" name="AccountNumber" class="form-control"
                                                                                placeholder="Enter AccountNumber" />
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
                                                                    <span class="glyphicon glyphicon-ok" id="btn_brnchs_mng_save1" onclick="branchesmanagement()">
                                                                    </span><span id="btn_brnchs_mng_save" onclick="branchesmanagement()">SAVE</span>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td style="padding-left: 7px;">
                                                            <div class="input-group">
                                                                <div class="input-group-close">
                                                                    <span class="glyphicon glyphicon-remove" id='btn_brnchs_mng_clear1' onclick="branchesmanagementclear()">
                                                                    </span><span id='btn_brnchs_mng_clear' onclick="branchesmanagementclear()">CLEAR</span>
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
                        <div id="div_empmaster_table" style="padding: 0px 10px 100px 10px">
                            <div id="div_BranchData" style="padding: 0px 10px 100px; display: block;">
                            </div>
                        </div>
                        <div id="Customer_Product" style="display: none; margin-top: -85px;">
                            <table align="center">
                                <tr>
                                    <td>
                                        <input type="text" id="txt_Product_search" style="height: 28px; opacity: 1.0; width: 165px;"
                                            class="form-control" placeholder="Search Product Name" />
                                    </td>
                                    <td>
                                        <input type="hidden" id="txtHiddenName" class="form-control" />
                                    </td>
                                    <td style="width: 65%;">
                                        <span class="input-group-btn">
                                            <button type="button" class="btn btn-info btn-flat" style="height: 30px;">
                                                <i class="fa fa-search" aria-hidden="true"></i>
                                            </button>
                                        </span>
                                    </td>
                                    <%--<td style="width: 200px;padding-right:20px;">
                                        <a href="RatesManage.aspx" target="_blank">Get Rate Sheet</a> 
                                    </td>--%>
                                    <td>
                                        <div id="divCustomerProduct" align="center" class="input-group" style="display: block;">
                                            <div class="input-group-addon" style="width: 100px;">
                                                <span class="glyphicon glyphicon-plus-sign" onclick="showCustomerProductDetaislclick();">
                                                </span><span onclick="showCustomerProductDetaislclick();">Add Produtct</span>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="CustomerProductFillform" style="display: none;">
                            <div class="row">
                                <div class="col-sm-6" style="width: 100%;" id="Div3">
                                    <div class="box box-solid box-success" style="margin-top: -85px;">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">
                                                <i style="padding-right: 5px;" class="fa fa-fw fa-user"></i>Customer Products</h3>
                                            <div class="box-tools pull-right">
                                                <button class="btn btn-box-tool" data-widget="collapse">
                                                </button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div class="box-body no-padding">
                                                <div style="display: block;">
                                                    <table align="center">
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Category Name</label>
                                                                <select id="cmb_brchprdt_Catgry_name" class="form-control" onchange="return productsdata_categoryname_onchange();">
                                                                </select>
                                                            </td>
                                                            <td style="width: 4px;">
                                                            </td>
                                                            <td>
                                                                <label>
                                                                    SubCategory Name</label>
                                                                <select id="cmb__brnch_subcatgry" class="form-control" onchange="return productsdata_subcategory_onchange();">
                                                                    <option>select</option>
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Product Name</label>
                                                                <select id="cmb_productname" class="form-control">
                                                                </select>
                                                            </td>
                                                            <td style="width: 4px;">
                                                            </td>
                                                            <td>
                                                                <label>
                                                                    Unit Price</label>
                                                                <input type="text" name="price" id="txt_productunitprice" class="form-control" placeholder="Enter Unit Price" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    MRP</label>
                                                                <input type="text" name="price" id="txt_mrp" class="form-control" placeholder="Enter MRP" />
                                                            </td>
                                                            <td style="width: 4px;">
                                                            </td>
                                                            <td>
                                                                <label>
                                                                    Flag</label>
                                                                <select id="cmb_branchesproductsflag" class="form-control">
                                                                    <option>Active</option>
                                                                    <option>InActive</option>
                                                                </select>
                                                            </td>
                                                            <td style="width: 4px;">
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
                                <div class="col-sm-6" style="width: 100%;" id="Div5">
                                    <div class="box box-solid box-success">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">
                                                <i style="padding-right: 5px;" class="fa fa-fw fa-user"></i>Tax Details</h3>
                                            <div class="box-tools pull-right">
                                                <button class="btn btn-box-tool" data-widget="collapse">
                                                    <i class="fa fa-minus"></i>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div class="box-body no-padding">
                                                <div style="display: block;">
                                                    <table align="center">
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Vat Percent</label>
                                                                <input type="text" name="price" id="txt_vatPercent" class="form-control" placeholder="Enter Vat Percent" />
                                                            </td>
                                                            <td style="width: 4px;">
                                                            </td>
                                                            <td>
                                                                <label>
                                                                    CGST</label>
                                                                <input id="txtCustomerCGST" type="text" name="CGST" class="form-control" placeholder="Enter CGST" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    SGST</label>
                                                                <input id="txtCustomerSGST" type="text" name="SGST" class="form-control" placeholder="Enter SGST" />
                                                            </td>
                                                            <td style="width: 4px;">
                                                            </td>
                                                            <td>
                                                                <label>
                                                                    IGST</label>
                                                                <input id="txtCustomerIGST" type="text" name="IGST" class="form-control" placeholder="Enter IGST" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <br />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <div class="input-group">
                                                                                <div class="input-group-addon">
                                                                                    <span class="glyphicon glyphicon-ok" id="btn_brnch_prodtuct_save1" onclick="branchproducts()">
                                                                                    </span><span id="btn_brnch_prodtuct_save" onclick="branchproducts()">SAVE</span>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                        <td style="padding-left: 7px;">
                                                                            <div class="input-group">
                                                                                <div class="input-group-close">
                                                                                    <span class="glyphicon glyphicon-remove" id='btn_brnch_prodtuct_clear1' onclick="branchproductsclear()">
                                                                                    </span><span id='btn_brnch_prodtuct_clear' onclick="branchproductsclear()">CLEAR</span>
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
                        </div>
                        <div id="branchProducts">
                            <table id="table4">
                                <tr>
                                    <td>
                                        <div id="div_Branch_Products" style="padding: 0px 10px 100px; display: block;">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="div_Documents" style="display: none; margin-top: -92px;">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Documents Upload</h3>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div>
                                        <br />
                                        <div class="box-body">
                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <label class="control-label">
                                                        Document Type</label>
                                                    <select id="ddl_documenttype" class="form-control">
                                                        <option>Select Document Type</option>
                                                        <option value="1">DrivingLicence</option>
                                                        <option value="2">Aadarcard</option>
                                                        <option value="3">Voterid</option>
                                                        <option value="4">PanCard</option>
                                                        <option value="5">Passport</option>
                                                        <option value="6">Passbook</option>
                                                    </select>
                                                </div>
                                                <div class="col-sm-4">
                                                    <table class="table table-bordered table-striped">
                                                        <tbody>
                                                            <tr>
                                                                <td>
                                                                    <div id="FileUpload_div" class="img_btn" onclick="getFile_doc()" style="height: 50px;
                                                                        width: 100%">
                                                                        Choose Document To Upload
                                                                    </div>
                                                                    <div style="height: 0px; width: 0px; overflow: hidden;">
                                                                        <input id="FileUpload1" type="file" name="files[]" onchange="readURL_doc(this);" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                                <div class="col-sm-4">
                                                    <input id="btn_upload_document" type="button" class="btn btn-primary" name="submit"
                                                        value="UPLOAD" onclick="upload_Customer_Document_Info();" style="width: 120px;
                                                        margin-top: 25px;" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="box-body">
                                            <div id="div_documents_table">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="div_CustomerBalance" style="display: none; margin-top: -92px;">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Documents Upload</h3>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div>
                                        <br />
                                        <div class="box-body">
                                            <div id="div_CustomerAmountDetails">
                                            </div>
                                            <div class="modal fade in" id="div_routewisemainCompare" style="display: none; padding-right: 17px;
                                                width: 110%;">
                                                <div class="modal-dialog">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                                <span aria-hidden="true" onclick="close_routewiseCompare();">×</span></button>
                                                            <h4 class="modal-title">
                                                                Agent Amount Details</h4>
                                                        </div>
                                                        <div class="modal-body" id="div_routewiseCompare" style="height: 400px; overflow-y: scroll;">
                                                        </div>
                                                        <div class="modal-footer">
                                                            <button type="button" class="btn btn-danger" onclick="close_routewiseCompare();">
                                                                Close</button>
                                                        </div>
                                                    </div>
                                                    <!-- /.modal-content -->
                                                </div>
                                                <!-- /.modal-dialog -->
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    </section>
</asp:Content>
