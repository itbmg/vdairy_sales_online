<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ApprovalForm.aspx.cs" Inherits="ApprovalForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="Js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
   <%-- <link href="Css/style.css" rel="stylesheet" type="text/css" />--%>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <script src="js/date.format.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                $('.divsalesOffice').css('display', 'table-row');
                FillSalesOffice();
            }
            else {
                $('.divsalesOffice').css('display', 'none');
                GetRisedVouchers();
            }
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#txtFromDate').val(today);
            $('#txtToDate').val(today);
        });
        function FillSalesOffice() {
            var data = { 'operation': 'GetSalesOffice' };
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
            var ddlsalesOffice = document.getElementById('ddlsalesOffice');
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
        function BtnGenerateClick() {
            var salesOffice = document.getElementById("ddlsalesOffice").value;
            if (salesOffice == "select") {
                alert("Select Sales Office");
                return false;
            }
            GetSalesOfficeRisedVouchers(salesOffice);
        }
        function GetSalesOfficeRisedVouchers(salesOffice) {
            var ddlType = document.getElementById("ddlType").value;
            var fromdate = document.getElementById("txtFromDate").value;
            var ToDate = document.getElementById("txtToDate").value;
            var data = { 'operation': 'GetRaisedVouchers', 'BranchID': salesOffice, 'Type': ddlType, 'fromdate': fromdate, 'ToDate': ToDate };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    $('#divRaisedVouchers').removeTemplate();
                    $('#divRaisedVouchers').setTemplateURL('RaisedVouchers3.htm');
                    $('#divRaisedVouchers').processTemplate(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function GetRisedVouchers() {
            var BranchID = "0";
            var data = { 'operation': 'GetRaisedVouchers', 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    $('#divRaisedVouchers').removeTemplate();
                    $('#divRaisedVouchers').setTemplateURL('RaisedVouchers3.htm');
                    $('#divRaisedVouchers').processTemplate(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var VoucherID = "0";
        var branchID = "0";
        var branchvoucherid = "0";
        function btnViewVoucher(ID) {
            branchvoucherid = $(ID).closest("tr").find('#spnbranchvoucherid').text();
            VoucherID = $(ID).closest("tr").find('#txtVoucherID').text();
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                $('.divsalesOffice').css('display', 'table-row');
                branchID = document.getElementById("ddlsalesOffice").value;
            }
            else {
            }
            var data = { 'operation': 'GetBtnViewVoucherclick', 'BranchID': branchID, 'VoucherID': VoucherID };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindViewVouchers(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindViewVouchers(msg) {
            $('#divMainAddNewRow').css('display', 'block');
            var emp = [];
            $('#divHead').setTemplateURL('SubPayable.htm');
            $('#divHead').processTemplate(emp);
            document.getElementById("spnName").innerHTML = msg[0].Description;
            document.getElementById("spnVoucherType").innerHTML = msg[0].VoucherType;
            document.getElementById("spnAmount").innerHTML = msg[0].Amount;
            document.getElementById("spnApprovalEmp").innerHTML = msg[0].ApproveEmpName;
            document.getElementById("txtCashierRemarks").value = msg[0].Remarks;
            document.getElementById("spnVoucherID").innerHTML = branchvoucherid;
            document.getElementById("txtApprovalamt").value = msg[0].ApprovalAmount;
            document.getElementById("txtRemarks").value = msg[0].ApprovalRemarks;
            document.getElementById("txtApprovalamt").value = msg[0].Amount;
            PopupOpen(VoucherID);
        }
        function PopupOpen(VocherID) {
            var data = { 'operation': 'GetAppriveSubPaybleValues', 'VoucherID': VocherID, 'BranchID': branchID };
            var s = function (msg) {
                if (msg) {
                    $('#divHead').setTemplateURL('SubPayable.htm');
                    $('#divHead').processTemplate(msg);
                    var TotRate = 0.0;
                    $('.AmountClass').each(function (i, obj) {
                        if ($(this).text() == "") {
                        }
                        else {
                            TotRate += parseFloat($(this).text());
                        }
                    });
                    TotRate = parseFloat(TotRate).toFixed(2);
                    document.getElementById("txt_total").innerHTML = TotRate;
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function OrdersCloseClick() {
            $('#divMainAddNewRow').css('display', 'none');
        }
        function btnApproveVoucherclick() {
            var Remarks = document.getElementById("txtCashierRemarks").value;
            var Approvalamt = document.getElementById("txtApprovalamt").value;
            if (Approvalamt == "") {
                alert("Enter Amount");
                return false;
            }
            var branchID = "0";
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                $('.divsalesOffice').css('display', 'table-row');
                branchID = document.getElementById("ddlsalesOffice").value;
            }
            else {
            }
            var AppRemarks = document.getElementById("txtRemarks").value;
            var Status = "A";
            var data = { 'operation': 'btnApproveVoucherclick', 'VoucherID': VoucherID, 'BranchID': branchID, 'Approvalamt': Approvalamt, 'AppRemarks': Remarks, 'Status': Status, 'Remarks': Remarks };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    $('#divMainAddNewRow').css('display', 'none');
                    document.getElementById("txtCashierRemarks").value = "";
                    document.getElementById("txtApprovalamt").value = "";
                    document.getElementById("txtRemarks").value = "";
                    var LevelType = '<%=Session["LevelType"] %>';
                    if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                        BtnGenerateClick();
                    }
                    else {
                        GetRisedVouchers();
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function btnRejectVoucherclick() {
            var Remarks = document.getElementById("txtCashierRemarks").value;
            var Approvalamt = document.getElementById("txtApprovalamt").value;
            if (Approvalamt == "") {
                alert("Enter Amount");
                return false;
            }
            var branchID = "0";
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                $('.divsalesOffice').css('display', 'table-row');
                branchID = document.getElementById("ddlsalesOffice").value;
            }
            else {
            }
            var AppRemarks = document.getElementById("txtRemarks").value;
            var Status = "C";
            var data = { 'operation': 'btnRejectVoucherclick', 'VoucherID': VoucherID, 'BranchID': branchID, 'Approvalamt': Approvalamt, 'Remarks': Remarks, 'Status': Status };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    $('#divMainAddNewRow').css('display', 'none');
                    document.getElementById("txtCashierRemarks").value = "";
                    document.getElementById("txtApprovalamt").value = "";
                    document.getElementById("txtRemarks").value = "";
                    var LevelType = '<%=Session["LevelType"] %>';
                    if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                        BtnGenerateClick();
                    }
                    else {
                        GetRisedVouchers();
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function btnVoucherUpdateClick() {
            var Remarks = document.getElementById("txtCashierRemarks").value;
            var branchID = "0";
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                $('.divsalesOffice').css('display', 'table-row');
                branchID = document.getElementById("ddlsalesOffice").value;
            }
            else {
            }
            var data = { 'operation': 'btnVoucherUpdateClick', 'VoucherID': VoucherID, 'branchID': branchID, 'Remarks': Remarks };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    $('#divMainAddNewRow').css('display', 'none');
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
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
            Approval Voucher<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Approval Voucher</a></li>
        </ol>
    </section>
    <section class="content" style="overflow-x: inherit !important;">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Approval Voucher Details
                </h3>
            </div>
            <div class="box-body">
                <table>
                    <tr class="divsalesOffice" style="display: none">
                        <td>
                           <label>    Sales Office</label>
                        </td>
                        <td>
                            <select id="ddlsalesOffice" class="form-control">
                            </select>
                        </td>
                        <td style="width:5px;"></td>
                        <td>
                            <label>   Status</label>
                        </td>
                        <td>
                            <select id="ddlType" class="form-control">
                                <option value="R">Raised</option>
                                <option value="A">Approved</option>
                                <option value="C">Rejected</option>
                                <option value="P">Paid</option>
                            </select>
                        </td>
                        <td style="width:5px;"></td>
                        <td>
                            <label>   From Date</label>
                        </td>
                        <td>
                            <input type="date" id="txtFromDate" class="form-control" />
                        </td>
                        <td style="width:5px;"></td>
                        <td>
                           <label>    To Date</label>
                        </td>
                        <td>
                            <input type="date" id="txtToDate" class="form-control" />
                        </td>
                        <td style="width:5px;">
                        </td>
                        <td>
                            <input type="button" id="btnPay" value="Genarete" onclick="BtnGenerateClick();" class="btn btn-primary" />
                        </td>
                    </tr>
                </table>
                <div id="divRaisedVouchers">
                </div>
                 <div id="divMainAddNewRow" class="pickupclass" style="text-align: center; height: 100%;
                    width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
                    background: rgba(192, 192, 192, 0.7);">
                    <div id="divAddNewRow" style="border: 5px solid #A0A0A0; position: absolute; top: 2%;
                        background-color: White; left: 7%; right: 7%; width: 85%; -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                        -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                        border-radius: 10px 10px 10px 10px;">
                        <table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;"
                            id="tableCollectionDetails" class="mainText2" border="1">
                            <tr>
                                <td>
                                  <label>  Name</label>
                                </td>
                                <td style="height:40px;">
                                    <span id="spnName" style="font-size:20px;font-weight:900;color:#00a65a;"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>  Voucher ID</label>
                                </td>
                                <td style="height:40px;">
                                    <span id="spnVoucherID" class="form-control"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   <label>    VoucherType</label>
                                </td>
                                <td style="height:40px;">
                                    <span id="spnVoucherType" style="font-size:20px;font-weight:900;color:#00a65a;"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="divHead">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>   Amount</label>
                                </td>
                                <td style="height:40px;">
                                    <span id="spnAmount" style="font-size:20px;font-weight:900;color:#3c8dbc;"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   <label>    Approval Employee</label>
                                </td>
                                <td style="height:40px;">
                                    <span id="spnApprovalEmp" style="font-size:20px;font-weight:900;color:#00a65a;"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   <label>    Remarks</label>
                                </td>
                                <td style="height:40px;">
                                    <textarea rows="3" cols="45" id="txtCashierRemarks" class="form-control" placeholder="Enter Remarks"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>   Approval Amount </label>
                                </td>
                                <td style="height:40px;">
                                    <input type="number" id="txtApprovalamt" class="form-control" value="" onkeypress="return numberOnlyExample();"
                                        placeholder="Enter Approval Amount" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>   Approval Remarks</label>
                                </td>
                                <td style="height:40px;">
                                    <input type="text" id="txtRemarks" class="form-control" value="" onkeypress="return numberOnlyExample();"
                                        placeholder="Enter Remarks" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height:40px;">
                                    <input type="button" value="Update" id="btnAdd" onclick="btnVoucherUpdateClick();"
                                         class="btn btn-primary" />
                                </td>
                                <td style="height:40px;">
                                    <input type="button" id="Button1" value="Approve" onclick="btnApproveVoucherclick();"
                                         class="btn btn-success" />
                                </td>
                                <td style="height:40px;">
                                    <input type="button" id="Button2" value="Reject" onclick="btnRejectVoucherclick();"
                                         class="btn btn-danger" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divclose" style="width: 35px; top:7%; right: 9.5%; position: absolute;
                        z-index: 99999; cursor: pointer;">
                        <img src="Images/Odclose.png" alt="close" onclick="OrdersCloseClick();" />
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
