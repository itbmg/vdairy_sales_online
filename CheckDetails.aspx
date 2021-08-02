<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CheckDetails.aspx.cs" Inherits="CheckDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="Js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <style type="text/css">
        .ddlDropStatus
        {
            width: 100%;
            height: 34px;
            border: 1px solid gray;
            border-radius: 6px 6px 6px 6px;
            font-size: 16px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director" || LevelType == "Admin") {
                $('.divsalesOffice').css('display', 'table-row');
                $('.tdType').css('display', 'table-row');
                FillSalesOffice();
            }
            else {
                $('.divsalesOffice').css('display', 'none');
                $('.tdType').css('display', 'none');
            }
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#txtFromDate').val(today);
            $('#txtTodate').val(today);
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
    </script>
    <script type="text/javascript">
        function GenerateClick() {
            var ddlType = document.getElementById('ddlType').value;
            var FromDate = document.getElementById('txtFromDate').value;
            if (FromDate == "") {
                alert("Please From Date");
                return false;
            }
            var ToDate = document.getElementById('txtTodate').value;
            if (ToDate == "") {
                alert("Please To Date");
                return false;
            }
            var branchID = "0";
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director" || LevelType == "Admin") {
                branchID = document.getElementById("ddlsalesOffice").value;
            }
            else {
            }
            var data = { 'operation': 'GetCheckDetails', 'FromDate': FromDate, 'ToDate': ToDate, 'BranchID': branchID, 'ddlType': ddlType };
            var s = function (msg) {
                if (msg) {

                    if (ddlType == "Cheque") {
                        $('#divCheckDetails').removeTemplate();
                        $('#divCheckDetails').setTemplateURL('ChequeVarify5.htm');
                        $('#divCheckDetails').processTemplate(msg);
                        get_faAccountNumber();
                    }
                    else {
                        $('#divCheckDetails').removeTemplate();
                        $('#divCheckDetails').setTemplateURL('BankTransferVerify.htm');
                        $('#divCheckDetails').processTemplate(msg);
                        get_faAccountNumber1();
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
        function get_faAccountNumber() {
            var data = { 'operation': 'get_finance_AccNos' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    fillfaaccounts(msg);

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillfaaccounts(msg) {
            $('.accountnocls').each(function () {
                var accounttype = $(this);
                accounttype[0].options.length = null;
                for (var i = 0; i < msg.length; i++) {
                    if (msg[i].accountno != null) {
                        var option = document.createElement('option');
                        option.innerHTML = msg[i].accountno;
                        option.value = msg[i].sno;
                        accounttype[0].appendChild(option);
                    }
                }
            });
        }

        function get_faAccountNumber1() {
            var data = { 'operation': 'get_finance_AccNos' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    fillfaaccounts1(msg);

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillfaaccounts1(msg) {
            $('.accountnocls1').each(function () {
                var accounttype = $(this);
                accounttype[0].options.length = null;
                for (var i = 0; i < msg.length; i++) {
                    if (msg[i].accountno != null) {
                        var option = document.createElement('option');
                        option.innerHTML = msg[i].accountno;
                        option.value = msg[i].sno;
                        accounttype[0].appendChild(option);
                    }
                }
            });
        }


        function btnChequeVerifySaveClick(id) {

            var ddlType = document.getElementById('ddlType').value;
            if (ddlType == "" || ddlType == "Select") {
                alert("Enter Amount");
                return false;
            }
            if (ddlType == "Cheque") {
                var Amount = $(id).closest("tr").find('#txtAmount').val();
                if (Amount == "") {
                    alert("Enter Amount");
                    return false;
                }
                var Status = $(id).closest("tr").find('#ddlStatus').val();
                if (Status == "Select") {
                    alert("Select Cheque Status");
                    return false;
                }
                var ddlfaaccno = $(id).closest("tr").find('#ddlfaaccno').val();
                var txt_date = $(id).closest("tr").find('#txt_date').val();
                if (txt_date == "") {
                    alert("Select Date");
                    return false;
                }
                if (!confirm("Do you really want Save")) {
                    return false;
                }
                var BranchSno = $(id).closest("tr").find('#hdnBranchSno').val();
                var ChequeNo = $(id).closest("tr").find('#txtChequeNo').text();
                var type = $(id).closest("tr").find('#txtSno').text();
                var AgentName = $(id).closest("tr").find('#txtAgentName').text();

                var data = { 'operation': 'btnChecksVerifySaveClick', 'BranchSno': BranchSno, 'cleardate': txt_date, 'ChequeNo': ChequeNo, 'Amount': Amount, 'Status': Status, 'type': type, 'ddlfaaccno': ddlfaaccno, 'ddlType': ddlType, 'AgentName': AgentName };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        if (msg == "Session Expired") {
                            window.location = "Login.aspx";
                        }

                        GenerateClick();
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                callHandler(data, s, e);
            }
            else {
                var Amount = $(id).closest("tr").find('#txtAmount').val();
                if (Amount == "") {
                    alert("Enter Amount");
                    return false;
                }
                var Status = $(id).closest("tr").find('#ddlStatus').val();
                if (Status == "Select") {
                    alert("Select Cheque Status");
                    return false;
                }
                var txt_date = $(id).closest("tr").find('#txt_date').val();
                if (txt_date == "") {
                    alert("Select Date");
                    return false;
                }
                if (!confirm("Do you really want Save")) {
                    return false;
                }
                var ddlfaaccno = $(id).closest("tr").find('#ddlfaaccno1').val();
                var BranchSno = $(id).closest("tr").find('#hdnBranchSno').val();
                var type = $(id).closest("tr").find('#txtSno').text();
                var CollSno = $(id).closest("tr").find('#hiddencollsno').val();
                var AgentName = $(id).closest("tr").find('#spnagentname').text();

                var data = { 'operation': 'btnChecksVerifySaveClick', 'BranchSno': BranchSno, 'cleardate': txt_date, 'Amount': Amount, 'Status': Status, 'type': type, 'ddlfaaccno': ddlfaaccno, 'ddlType': ddlType, 'CollSno': CollSno, 'AgentName': AgentName };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        if (msg == "Session Expired") {
                            window.location = "Login.aspx";
                        }
                        GenerateClick();
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                callHandler(data, s, e);
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
            Cheque Approval<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Cheque Approval</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Cheque Approval Details
                </h3>
            </div>
            <div class="box-body">
                <div style="width: 100%; background-color: #fff">
                    <div>
                        <table>
                            <tr>
                                <td class="tdType" style="display: none;">
                                    <select id="ddlType" class="form-control">
                                        <option>Select</option>
                                        <option>Cheque</option>
                                        <option>Bank Transfer</option>
                                    </select>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td class="divsalesOffice" style="display: none;">
                                    <select id="ddlsalesOffice" class="form-control">
                                    </select>
                                </td>
                                <td>
                                    Fromdate
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <input type="date" id="txtFromDate" class="form-control" />
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    Todate
                                </td>
                                <td>
                                    <input type="date" id="txtTodate" class="form-control" />
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <input type="button" id="btnGenerate" value="Generate" onclick="GenerateClick();"
                                        class="btn btn-primary" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divCheckDetails">
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
