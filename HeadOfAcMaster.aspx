<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="HeadOfAcMaster.aspx.cs" Inherits="HeadOfAcMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            FillSalesOffice();
            UpdateHeads();
        });
        function ddlSalesOfficeChange(ID) {
            UpdateHeads();
        }
        function UpdateHeads() {
            var BranchID = document.getElementById('ddlSalesOffice').value;
            if (BranchID == "select") {
                BranchID = "";
            }
            var data = { 'operation': 'GetHeadOfAccpunts', 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    BindVehicles(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var serial = 0;
        function BindVehicles(msg) {
            var k = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">Code</th><th scope="col" class="thcls">Ledger Code</th><th scope="col" class="thcls">Ledger Name</th><th scope="col" class="thcls">Account Type</th><th scope="col" class="thcls">Limit</th><th scope="col" class="thcls">Status</th><th scope="col"></th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[k] + '">';
                results += '<td scope="row" class="1" >' + msg[i].Code + '</td>';
                results += '<td data-title="Capacity" class="6">' + msg[i].accountcode + '</td>';
                results += '<td data-title="Capacity"  class="2 tdmaincls">' + msg[i].HeadName + '</td>';
                results += '<td data-title="Capacity" class="3">' + msg[i].AccountType + '</td>';
                results += '<td data-title="Capacity" class="4">' + msg[i].Limit + '</td>';
                results += '<td data-title="Capacity" class="7">' + msg[i].flag + '</td>';
                results += '<td style="display:none" class="5">' + msg[i].Sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                k = k + 1;
                if (k == 4) {
                    k = 0;
                }
            }
            results += '</table></div>';
            $("#div_Headdata").html(results);
        }
        function getme(thisid) {
            var Code = $(thisid).parent().parent().children('.1').html();
            var HeadName = $(thisid).parent().parent().children('.2').html();
            var Ledgercode = $(thisid).parent().parent().children('.6').html();
            var AccountType = $(thisid).parent().parent().children('.3').html();
            var Limit = $(thisid).parent().parent().children('.4').html();
            var cmb_flag = $(thisid).parent().parent().children('.7').html();
            var Sno = $(thisid).parent().parent().children('.5').html();
            document.getElementById('txtDecription').value = HeadName;
            document.getElementById('txtLedgercode').value = Ledgercode;

            document.getElementById('ddlAccountType').value = AccountType;
            //            if (AccountType == "Agent") {
            //                FillAgents();
            //                $('.divPayTo').css('display', 'table-row');
            //                $('.divPayTodesc').css('display', 'none');
            //                document.getElementById('ddlCashTo').value = Code;
            //            }
            //            if (AccountType == "Employee") {
            //                FillEmploye();
            //                $('.divPayTo').css('display', 'table-row');
            //                $('.divPayTodesc').css('display', 'none');
            //                document.getElementById('ddlCashTo').value = Code;
            //            }
            //            if (AccountType == "Others") {
            //                $('.divPayTo').css('display', 'none');
            //                $('.divPayTodesc').css('display', 'table-row');
            //            }
            document.getElementById('txtLimit').value = Limit;
            document.getElementById('cmb_flag').value = cmb_flag;

            serial = Sno;
            document.getElementById('btnSave').innerHTML = "MODIFY";
        }
        function Headvalidation() {
            var Decription = "";
            var Ledgercode = "";
            var BranchID = document.getElementById('ddlSalesOffice').value;
            if (BranchID == "" || BranchID == "select") {
                alert("Please Select SalesOffice Name");
                $("#ddlSalesOffice").focus();
                return false;
            }
            var AccountType = document.getElementById("ddlAccountType").value;
            if (AccountType == "") {
                alert("Please Select Account Type");
                $("#ddlAccountType").focus();
                return false;
            }

            var EMPID = 0;
            //            if (AccountType == "Others") {
            Decription = document.getElementById("txtDecription").value;
            Ledgercode = document.getElementById("txtLedgercode").value;

            //            }
            //            else {
            //                var Head = document.getElementById("ddlCashTo");
            //                EMPID = Head.options[Head.selectedIndex].value;
            //                Decription = Head.options[Head.selectedIndex].text;
            //            }
            if (Decription == "") {
                alert("Enter Head Description");
                return false;
            }
            var Limit = document.getElementById('txtLimit').value;
            var flag = document.getElementById('cmb_flag').value;
            var btnSave = document.getElementById('btnSave').innerHTML;
            var data = { 'operation': 'SaveHeadMasterClick', 'Ledgercode': Ledgercode, 'flag': flag, 'Decription': Decription, 'serial': serial, 'btnSave': btnSave, 'Limit': Limit, 'AccountType': AccountType, 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    UpdateHeads();
                    document.getElementById('txtDecription').value = "";
                    document.getElementById('txtLedgercode').value = "";
                    document.getElementById('btnSave').innerHTML = "Save";
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        //        function ddlAccountTypeChange(Account) {
        //            var AccountType = Account.value;
        ////            if (AccountType == "Agent") {
        ////                FillAgents();
        ////                $('.divPayTo').css('display', 'table-row');
        ////                $('.divPayTodesc').css('display', 'none');
        ////            }
        ////            if (AccountType == "Employee") {
        ////                FillEmploye();
        ////                $('.divPayTo').css('display', 'table-row');
        ////                $('.divPayTodesc').css('display', 'none');
        ////            }
        ////            if (AccountType == "Others") {
        ////                $('.divPayTo').css('display', 'none');
        ////                $('.divPayTodesc').css('display', 'table-row');

        ////            }
        //        }
        function FillAgents() {
            var data = { 'operation': 'GetAgentNames' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindAgentNames(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindAgentNames(msg) {
            var ddlAgent = document.getElementById('ddlCashTo');
            var length = ddlAgent.options.length;
            ddlAgent.options.length = null;
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlAgent.appendChild(opt);
                }
            }
        }
        function FillEmploye() {
            var data = { 'operation': 'GetEmployeeNames' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindEmployeeName(msg);

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindEmployeeName(msg) {
            var ddlCashTo = document.getElementById('ddlCashTo');
            var length = ddlCashTo.options.length;
            ddlCashTo.options.length = null;
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].UserName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].UserName;
                    opt.value = msg[i].Sno;
                    ddlCashTo.appendChild(opt);
                }
            }
        }
        function FillSalesOffice() {
            var data = { 'operation': 'GetAllSalesOffice' };
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
            Head Of Accounts<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Masters</a></li>
            <li><a href="#">Head Of Accounts</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Head Of Accounts Details
                </h3>
            </div>
            <div class="box-body">
                <table align="center">
                    <tr>
                        <td>
                            <label>Sales Office</label><span style="color: red; font-weight: bold">*</span>
                       
                            <select id="ddlSalesOffice" class="form-control" onchange="ddlSalesOfficeChange(this);">
                            </select>
                        </td>
                        <td style="width:5px;">
                   </td>
                        <td>
                            <label>Account Type</label><span style="color: red; font-weight: bold">*</span>
                            <select id="ddlAccountType" class="form-control" >
                                <option>Select</option>
                               <%-- <option>Agent</option>
                                <option>Employee</option>--%>
                                <option>Others</option>
                            </select>
                        </td>
                    </tr>
                    <tr class="divPayTodesc">
                        <td>
                           <label> Ledger Code</label>
                        
                            <input type="text" id="txtLedgercode" class="form-control" placeholder="Enter Ledger Code" />
                        </td>
                   <td style="width:5px;">
                   </td>
                        <td>
                            <label>Head Description</label>
                            <input type="text" id="txtDecription" class="form-control"    placeholder="Enter Head Description" />
                        </td>
                    </tr>
                    <%--<tr class="divPayTo" style="display: none">
                        <td>
                            
                            <label>Head Decription</label>
                            <select id="ddlCashTo" class="form-control">
                            </select>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                             <label>Limit</label>
                            <input type="text" id="txtLimit" class="form-control" placeholder="Enter Limit" />
                        </td>
                        <td style="width:5px;">
                   </td>
                        <td>
                            <label id="lbldeptflag">
                                Flag:</label>
                            <select id="cmb_flag" class="form-control">
                                <option value="1">Active</option>
                                <option value="0">InActive</option>
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
                                                    <span class="glyphicon glyphicon-ok" id="btnSave1" onclick="Headvalidation()">
                                                    </span><span id="btnSave" onclick="Headvalidation()">SAVE</span>
                                                </div>
                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                        <%--<td>
                            <div class="input-group">
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-ok" id="btnSave1" onclick="Vehiclevalidation()">
                                                    </span><span id="btnSave" onclick="Vehiclevalidation()">SAVE</span>
                                                </div>
                                            </div>
                        </td>--%>
                    </tr>
                     </table>
                </div>
                </div>
                  <div class="box box-info">
                   <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-list"></i>Head Of Accounts list
                </h3>
            </div>
                <div id="div_Headdata" style="width: 100%; cursor: pointer; height: 400px; overflow: auto;">
                </div>
            </div>
        
    </section>
</asp:Content>
