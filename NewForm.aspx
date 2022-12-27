<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="NewForm.aspx.cs" Inherits="NewForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="Js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css"/>
    <style type="text/css">
        .ddlsize
        {
            width: 196px;
            height: 30px;
            font-size: 16px;
            border: 1px solid gray;
            border-radius: 7px 7px 7px 7px;
        }
        .datepicker
        {
            border: 1px solid gray;
            background: url("Images/CalBig.png") no-repeat scroll 99%;
            width: 70%;
            top: 0;
            left: 0;
            height: 20px;
            font-weight: 700;
            font-size: 12px;
            cursor: pointer;
            border: 1px solid gray;
            margin: .5em 0;
            padding: .6em 20px;
            border-radius: 10px 10px 10px 10px;
            filter: Alpha(Opacity=0);
            box-shadow: 3px 3px 3px #ccc;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#datepicker").datepicker({ dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
                onSelect: function (selectedDate) {
                }
            });
            $("#dtchequedate").datepicker({ dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
                onSelect: function (selectedDate) {
                }
            });
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#datepicker').val(today);
            FillSalesOffice();
            get_Others_Details();
            get_faAccountNumber();
            FillHeads();
        });
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
        function ddlSalesOfficeChange(ID) {
            var BranchID = ID.value;
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
            var veh = document.getElementById('ddlRouteName');
            var length = veh.options.length;
            for (i = length - 1; i >= 0; i--) {
                veh.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Route Name";
            opt.value = "";
            veh.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].RouteName;
                    opt.value = msg[i].rid;
                    veh.appendChild(opt);
                }
            }
        }
        function ddlRouteNameChange(id) {
            FillAgentName(id.value);
        }
        function FillAgentName(RouteID) {
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
            var ddlAgentName = document.getElementById('ddlAgentName');
            var length = ddlAgentName.options.length;
            for (i = length - 1; i >= 0; i--) {
                ddlAgentName.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Agent Name";
            opt.value = "";
            ddlAgentName.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlAgentName.appendChild(opt);
                }
            }
        }
        function BtnCashAmountClick() {
            var collectiontype = document.getElementById('ddlcollectiontype').value;
            if (collectiontype == "Select") {
                alert("Select collection Type");
                return false;
            }
            var ddlfreezertype = document.getElementById('ddlfreezertype').value;
            var ddlfreezeramounttype = document.getElementById('ddlfreezeramounttype').value;
            var ddlTransType = document.getElementById('ddlTransType').value;
            if (collectiontype == "SD Deposit") {

                var ddlAmountType = document.getElementById('ddlAmountType').value;
                if (ddlAmountType == "Select") {
                    alert("Select Collection Type");
                    return false;
                }
                var paymenttype = document.getElementById('ddlPaymentType').value;
                if (paymenttype == "Select") {
                    alert("Select Payment Type");
                    return false;
                }
                var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
                if (ddlSalesOffice == "select") {
                    alert("Select salesOffice");
                    return false;
                }
                var ddlRouteName = document.getElementById('ddlRouteName').value;
                if (ddlRouteName == "Select Route Name") {
                    alert("Select Route Name");
                    return false;
                }
                var Name = $("#ddlAgentName option:selected").text();
                var AgentID = document.getElementById('ddlAgentName').value;
                if (AgentID == "Select Agent Name") {
                    alert("Select Agent Name");
                    return false;
                }
                var txtAmount = document.getElementById('txtAmount').value;
                if (txtAmount == "") {
                    alert("Enter Amount");
                    return false;
                }
                var txtCashierRemarks = document.getElementById('txtCashierRemarks').value;
                if (txtCashierRemarks == "") {
                    alert("Enter Remarks");
                    return false;
                }
                var chequeDate = "";
                var txtChequeNo = "";
                var txtBankName = "";
                if (paymenttype == "Cheque") {
                    txtChequeNo = document.getElementById('txtChequeNo').value;
                    chequeDate = document.getElementById('dtchequedate').value;
                    txtBankName = document.getElementById('txtBankName').value;
                    if (txtChequeNo == "") {
                        alert("Enter Cheque No");
                        return false;
                    }
                    if (chequeDate == "") {
                        alert("Enter chequeDate");
                        return false;
                    }
                    if (txtBankName == "") {
                        alert("Enter Bank Name");
                        return false;
                    }
                }
                if (paymenttype == "DD") {
                    txtChequeNo = document.getElementById('txtChequeNo').value;
                    if (txtChequeNo == "") {
                        alert("Enter DD No");
                        return false;
                    }
                }
                if (paymenttype == "Journal Voucher") {
                    txtChequeNo = document.getElementById('txtChequeNo').value;
                    if (txtChequeNo == "") {
                        alert("Enter Journal Voucher");
                        return false;
                    }
                }
                if (!confirm("Do you want to save this transaction")) {
                    return false;
                }
                if (paymenttype == "Cash" || paymenttype == "PhonePay") {
                    var txt_Total = document.getElementById("txtCTotAmount").innerHTML;
                    var rowsdenominations = $("#tableCollectionDetails tr:gt(0)");
                    var DenominationString = "";
                    var ReturnDenominationString = "";
                    $(rowsdenominations).each(function (i, obj) {
                        if ($(this).closest("tr").find(".CashClass").text() == "") {
                        }
                        else {
                            var str = $(this).closest("tr").find(".CashClass").text();
                            DenominationString += str.trim() + 'x' + $(this).closest("tr").find(".AmountClass").val() + "+";
                            ReturnDenominationString += str.trim() + 'x' + $(this).closest("tr").find(".RAmountClass").val() + "+";
                        }
                    });
                    if (txtAmount == txt_Total) {
                    }
                    else {
                        alert("Please fill denomination amount");
                        return false;
                    }
                }
//                var data = { 'operation': 'BtnCashAmountClick', 'ddlTransType': ddlTransType, 'DenominationString': DenominationString, 'ReturnDenominationString': ReturnDenominationString,  'AgentID': AgentID, 'Name': Name, 'paymenttype': paymenttype, 'collectiontype': collectiontype,  'ChequeNo': txtChequeNo, 'chequeDate': chequeDate, 'BankName': txtBankName, 'Amount': txtAmount, 'Remarks': txtCashierRemarks };
                var data = { 'operation': 'BtnCashAmountClick', 'ddlTransType': ddlTransType, 'DenominationString': DenominationString, 'ReturnDenominationString': ReturnDenominationString, 'ddlfreezertype': ddlfreezertype, 'ddlfreezeramounttype': ddlfreezeramounttype, 'AgentID': AgentID, 'Name': Name, 'paymenttype': paymenttype, 'collectiontype': collectiontype, 'ddlAmountType': ddlAmountType, 'ChequeNo': txtChequeNo, 'chequeDate': chequeDate, 'BankName': txtBankName, 'Amount': txtAmount, 'Remarks': txtCashierRemarks };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        document.getElementById('txtDesc').value = "";
                        document.getElementById('txtAmount').value = "";
                        document.getElementById('txtCashierRemarks').value = "";
                        Denominationzero();
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                callHandler(data, s, e);
            }
            if (collectiontype == "Other") {

                var Name = document.getElementById('txtDesc').value;
                var MasterName = document.getElementById('txtHiddenName').value;
                if (MasterName == Name) {
                }
                else {
                }
                var ledger_code = document.getElementById('txtLedgerCode').value;
                if (ledger_code == "") {
                }
                var paymenttype = document.getElementById('ddlPaymentType').value;
                if (paymenttype == "Select") {
                    alert("Select Payment Type");
                    return false;
                }
                var txtAmount = document.getElementById('txtAmount').value;
                if (txtAmount == "") {
                    alert("Enter Amount");
                    return false;
                }
                var ddlAmountType = document.getElementById('ddlAmountType').value;
                if (ddlAmountType == "Select") {
                    alert("Select Collection Type");
                    return false;
                }
                var chequeDate = "";
                var txtChequeNo = "";
                var txtBankName = "";
                if (paymenttype == "Cheque") {
                    txtChequeNo = document.getElementById('txtChequeNo').value;
                    chequeDate = document.getElementById('dtchequedate').value;
                    txtBankName = document.getElementById('txtBankName').value;
                    if (txtChequeNo == "") {
                        alert("Enter Cheque No");
                        return false;
                    }
                    if (chequeDate == "") {
                        alert("Enter chequeDate");
                        return false;
                    }
                    if (txtBankName == "") {
                        alert("Enter Bank Name");
                        return false;
                    }
                }
                if (paymenttype == "DD") {
                    txtChequeNo = document.getElementById('txtChequeNo').value;
                    txtBankName = document.getElementById('txtBankName').value;
                    if (txtChequeNo == "") {
                        alert("Enter DD No");
                        return false;
                    }
                    if (txtBankName == "") {
                        alert("Enter Bank Name");
                        return false;
                    }
                }
                if (paymenttype == "Journal Voucher") {
                    txtChequeNo = document.getElementById('txtChequeNo').value;
                    if (txtChequeNo == "") {
                        alert("Enter Journal Voucher");
                        return false;
                    }
                }
                var txtCashierRemarks = document.getElementById('txtCashierRemarks').value;
                if (txtCashierRemarks == "") {
                    alert("Enter Remarks");
                    return false;
                }
                if (!confirm("Do you want to save this transaction")) {
                    return false;
                }
                var txt_Total = document.getElementById("txtCTotAmount").innerHTML;
                var rowsdenominations = $("#tableCollectionDetails tr:gt(0)");
                var DenominationString = "";
                var ReturnDenominationString = "";
                $(rowsdenominations).each(function (i, obj) {
                    if ($(this).closest("tr").find(".CashClass").text() == "") {
                    }
                    else {
                        var str = $(this).closest("tr").find(".CashClass").text();
                        DenominationString += str.trim() + 'x' + $(this).closest("tr").find(".AmountClass").val() + "+";
                        ReturnDenominationString += str.trim() + 'x' + $(this).closest("tr").find(".RAmountClass").val() + "+";
                    }
                });
                if (txtAmount == txt_Total) {
                }
                else {
                    alert("Please fill denomination amount");
                    return false;
                }
//                var data = { 'operation': 'BtnCashAmountClick', 'ledger_code': ledger_code, 'ddlTransType': ddlTransType, 'DenominationString': DenominationString, 'ReturnDenominationString': ReturnDenominationString, 'Name': Name, 'paymenttype': paymenttype, 'collectiontype': collectiontype,  'ChequeNo': txtChequeNo, 'chequeDate': chequeDate, 'BankName': txtBankName, 'Amount': txtAmount, 'Remarks': txtCashierRemarks };

                var data = { 'operation': 'BtnCashAmountClick', 'ledger_code': ledger_code, 'ddlTransType': ddlTransType, 'DenominationString': DenominationString, 'ReturnDenominationString': ReturnDenominationString, 'ddlfreezertype': ddlfreezertype, 'ddlfreezeramounttype': ddlfreezeramounttype, 'Name': Name, 'paymenttype': paymenttype, 'collectiontype': collectiontype, 'ddlAmountType': ddlAmountType, 'ChequeNo': txtChequeNo, 'chequeDate': chequeDate, 'BankName': txtBankName, 'Amount': txtAmount, 'Remarks': txtCashierRemarks };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        document.getElementById('txtDesc').value = "";
                        document.getElementById('txtAmount').value = "";
                        document.getElementById('txtCashierRemarks').value = "";
                        Denominationzero();
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                callHandler(data, s, e);
            }
            else if (collectiontype == "SalesOfficeCollection") {

                var txtAmount = document.getElementById('txtAmount').value;
                if (txtAmount == "") {
                    alert("Enter Amount Received");
                    return false;
                }
                var ddlAgentName = document.getElementById('ddlAgentName').value;
                var txtCashierRemarks = document.getElementById('txtCashierRemarks').value;
                if (txtCashierRemarks == "") {
                    alert("Enter Remarks");
                    return false;
                }
                var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
                var paymenttype = document.getElementById('ddlPaymentType').value;
                if (paymenttype == "Select") {
                    alert("Select Payment Type");
                    return false;
                }
                var ddltransactiontype = document.getElementById('ddltransactiontype').value;
                var PaidDate = document.getElementById('datepicker').value;
                var chequeDate = document.getElementById('dtchequedate').value;
                var txtChequeNo = "";
                var txtBankName = "";
                var faaccuntno = "";

                if (paymenttype == "Cheque") {
                    txtChequeNo = document.getElementById('txtChequeNo').value;
                    chequeDate = document.getElementById('dtchequedate').value;
                    txtBankName = document.getElementById('txtBankName').value;
                    if (txtChequeNo == "") {
                        alert("Enter Cheque No");
                        return false;
                    }
                    if (chequeDate == "") {
                        alert("Enter chequeDate");
                        return false;
                    }
                    if (txtBankName == "") {
                        alert("Enter Bank Name");
                        return false;
                    }
                }
                var HeadSno = 0;
                if (paymenttype == "Bank Transfer") {
                    txtBankName = document.getElementById('txtBankName').value;
                    if (txtBankName == "") {
                        alert("Enter Bank Name");
                        return false;
                    }
                    faaccuntno = document.getElementById('ddlfaaccno').value;
                    if (faaccuntno == "" || faaccuntno == "select account number") {
                        alert("Select account number");
                        return false;
                    }
                }
                if (paymenttype == "DD") {
                    txtChequeNo = document.getElementById('txtChequeNo').value;
                    if (txtChequeNo == "") {
                        alert("Enter DD No");
                        return false;
                    }
                }
                if (paymenttype == "Journal Voucher" || paymenttype == "Incentive") {
                    txtChequeNo = document.getElementById('txtChequeNo').value;
                    if (txtChequeNo == "") {
                        alert("Enter Journal Voucher");
                        return false;
                    }
                    var Head = document.getElementById("combobox");
                    HeadSno = Head.options[Head.selectedIndex].value;
                    var HeadOfAccount = Head.options[Head.selectedIndex].text;
                }
                if (paymenttype == "Cash" || paymenttype == "PhonePay") {
                    var rowsdenominations = $("#tableCollectionDetails tr:gt(0)");
                    var DenominationString = "";
                    var ReturnDenominationString = "";
                    $(rowsdenominations).each(function (i, obj) {
                        if ($(this).closest("tr").find(".CashClass").text() == "") {
                        }
                        else {
                            var str = $(this).closest("tr").find(".CashClass").text();
                            DenominationString += str.trim() + 'x' + $(this).closest("tr").find(".AmountClass").val() + "+";
                            ReturnDenominationString += str.trim() + 'x' + $(this).closest("tr").find(".RAmountClass").val() + "+";
                        }
                    });
                    if (!confirm("Do you want to save this transaction")) {
                        return false;
                    }
                    var txt_Total = document.getElementById("txtCTotAmount").innerHTML;
                    if (txtAmount == txt_Total) {
                    }
                    else {
                        alert("Please fill denomination amount");
                        return false;
                    }
                }
                var data = { 'operation': 'BtnCashAmountClick', 'collectiontype': collectiontype, 'HeadSno': HeadSno, 'BranchID': ddlAgentName, 'Amount': txtAmount, 'Remarks': txtCashierRemarks, 'paymenttype': paymenttype, 'ChequeNo': txtChequeNo, 'PaidDate': PaidDate, 'chequeDate': chequeDate, 'BankName': txtBankName, 'ddltransactiontype': ddltransactiontype, 'soid': ddlSalesOffice, 'DenominationString': DenominationString, 'ReturnDenominationString': ReturnDenominationString, 'faaccuntno': faaccuntno };
                var s = function (msg) {
                    if (msg) {
                        alert(msg);
                        document.getElementById('txtAmount').value = "";
                        document.getElementById('txtCashierRemarks').value = "";
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





        function Denominationzero() {
            var Total = 0;
            $('.AmountClass').val(Total);
            $('.RAmountClass').text(Total);
            document.getElementById('txtAmont').innerHTML = Total;
            document.getElementById('txtreturnAmount').innerHTML = Total;
            document.getElementById('txtCTotAmount').innerHTML = Total;
        }
        function CountChange(count) {
            var TotalCash = 0;
            var Total = 0;
            if (count.value == "") {
                $(count).closest("tr").find(".TotalClass").text(Total);
                return false;
            }
            var Cash = $(count).closest("tr").find(".CashClass").text();
            Total = parseInt(count.value) * parseInt(Cash);
            $(count).closest("tr").find(".TotalClass").text(Total);
            $('.TotalClass').each(function (i, obj) {
                TotalCash += parseInt($(this).text());
            });
            document.getElementById('txt_Total').innerHTML = TotalCash;
        }
        function ColCountChange(count) {
            var TotalCash = 0;
            var Total = 0;
            if (count.value == "") {
                $(count).closest("tr").find(".TotalClass").text(Total);
                $('.TotalClass').each(function (i, obj) {
                    var Amount = $(this).text();
                    if (Amount == "") {
                        Amount = 0.0;
                    }
                    TotalCash += parseFloat(Amount);
                });
                document.getElementById('txtAmont').innerHTML = parseFloat(TotalCash).toFixed(2);
                var Amount = document.getElementById('txtAmont').innerHTML;
                var ReturnAmount = document.getElementById('txtreturnAmount').innerHTML;
                var Total = parseFloat(Amount) - parseFloat(ReturnAmount);
                document.getElementById('txtCTotAmount').innerHTML = Total;
                return false;
            }
            else {
                var Cash = $(count).closest("tr").find(".CashClass").text();
                Total = parseFloat(count.value) * parseFloat(Cash);
                $(count).closest("tr").find(".TotalClass").text(Total);
                $('.TotalClass').each(function (i, obj) {
                    var Amount = $(this).text();
                    if (Amount == "") {
                        Amount = 0.0;
                    }
                    TotalCash += parseFloat(Amount);
                });
                document.getElementById('txtAmont').innerHTML = parseFloat(TotalCash).toFixed(2);
                var Amount = document.getElementById('txtAmont').innerHTML;
                var ReturnAmount = document.getElementById('txtreturnAmount').innerHTML;
                var Total = parseFloat(Amount) - parseFloat(ReturnAmount);
                document.getElementById('txtCTotAmount').innerHTML = Total;
            }
        }

        function ReturnCountChange(count) {
            var TotalCash = 0;
            var Total = 0;
            if (count.value == "") {
                $(count).closest("tr").find(".ReturnAmountClass").text(Total);
                $('.ReturnAmountClass').each(function (i, obj) {
                    var Amount = $(this).text();
                    if (Amount == "") {
                        Amount = 0.0;
                    }
                    TotalCash += parseFloat(Amount);
                });
                document.getElementById('txtreturnAmount').innerHTML = parseFloat(TotalCash).toFixed(2);
                var Amount = document.getElementById('txtAmont').innerHTML;
                var ReturnAmount = document.getElementById('txtreturnAmount').innerHTML;
                var Total = parseFloat(Amount) - parseFloat(ReturnAmount);
                document.getElementById('txtCTotAmount').innerHTML = Total;
                return false;
            }
            else {
                var Cash = $(count).closest("tr").find(".CashClass").text();
                Total = parseFloat(count.value) * parseFloat(Cash);
                $(count).closest("tr").find(".ReturnAmountClass").text(Total);
                $('.ReturnAmountClass').each(function (i, obj) {
                    var Amount = $(this).text();
                    if (Amount == "") {
                        Amount = 0.0;
                    }
                    TotalCash += parseFloat(Amount);
                });
                document.getElementById('txtreturnAmount').innerHTML = parseFloat(TotalCash).toFixed(2);
                var Amount = document.getElementById('txtAmont').innerHTML;
                var ReturnAmount = document.getElementById('txtreturnAmount').innerHTML;
                var Total = parseFloat(Amount) - parseFloat(ReturnAmount);
                document.getElementById('txtCTotAmount').innerHTML = Total;
            }
        }
        function ddlCollectionTypeChange() {
            var collectiontype = document.getElementById('ddlcollectiontype').value;
            if (collectiontype == "Select") {
                alert("Select collection Type");
                return false;
            }
            if (collectiontype == "SD Deposit") {

                $('.divsalesoffclass').css('display', 'table-row ');
                $('.divRouteclass').css('display', 'table-row ');
                $('.divAgentclass').css('display', 'table-row ');
                $('.divnameclass').css('display', 'none');
                $('.divcodeclass').css('display', 'none');

                $('.clsclosingbalance').css('display', 'none ');
                $('.lblopening').css('display', 'none ');
                $('.lblpaidamount').css('display', 'none ');

            }
            if (collectiontype == "SalesOfficeCollection") {

                $('.divsalesoffclass').css('display', 'table-row ');
                $('.divRouteclass').css('display', 'table-row ');
                $('.divAgentclass').css('display', 'table-row ');
                $('.divnameclass').css('display', 'none');
                $('.divcodeclass').css('display', 'none');
                $('.clsclosingbalance').css('display', 'table-row ');
                $('.lblopening').css('display', 'table-row ');
                $('.lblpaidamount').css('display', 'table-row ');
                $('.clspurpose').css('display', 'none');
            }
            if (collectiontype == "Other") {

                $('.divsalesoffclass').css('display', 'none ');
                $('.divRouteclass').css('display', 'none ');
                $('.divAgentclass').css('display', 'none ');
                $('.divnameclass').css('display', 'table-row');
                $('.divcodeclass').css('display', 'table-row');

                $('.clsclosingbalance').css('display', 'none ');
                $('.lblopening').css('display', 'none ');
                $('.lblpaidamount').css('display', 'none ');
                $('.clspurpose').css('display', 'table-row ');
                
            }

        }
        var PaymentType = "";
        function ddlPaymentTypeChange(Payment) {
            PaymentType = Payment.options[Payment.selectedIndex].text;
            if (PaymentType == "Cash" || paymenttype == "PhonePay" ) {
                $('.divChequeclass').css('display', 'none');
                $('.divChequeDateclass').css('display', 'none');
                $('.divBankclass').css('display', 'none');
                $('.divfinanceaccno').css('display', 'none');
                $('#tdDenomination').css('display', 'block');
                document.getElementById("txtAmount").innerHTML = "";

            }
            if (PaymentType == "Bank Transfer") {
                $('.divChequeclass').css('display', 'none');
                $('.divChequeDateclass').css('display', 'none');
                $('.divBankclass').css('display', 'table-row');
                $('.divfinanceaccno').css('display', 'table-row');
                $('#tdDenomination').css('display', 'none');
                $('.clsclosingbalance').css('display', 'block ');
                $('.divDebitclass').css('display', 'none');
                var ddlcollectiontype = document.getElementById('ddlcollectiontype').value;
                if (ddlcollectiontype == "SalesOfficeCollection") {
                    $('.divBankclass').css('display', 'table-row');
                }
                var ddlcollectiontype = document.getElementById('ddlAmountType').value;
                if (ddlcollectiontype == "Agent") {
                    $('.divfinanceaccno').css('display', 'none');
                }
                if (ddlcollectiontype == "Other") {
                    $('.divfinanceaccno').css('display', 'none');
                }
                document.getElementById("txtAmount").innerHTML = "";
            }
            if (PaymentType == "Cheque") {
                $('.divChequeclass').css('display', 'table-row');
                $('.divChequeDateclass').css('display', 'table-row');
                $('.divBankclass').css('display', 'table-row');
                $('.divfinanceaccno').css('display', 'none');
                document.getElementById("spnchequeno").innerHTML = "Cheque No";
                $('#tdDenomination').css('display', 'none');
                var input = document.getElementById("txtChequeNo");
                input.placeholder = "Enter Cheque No";
                document.getElementById("txtAmount").innerHTML = "";
            }
            if (PaymentType == "DD") {
                $('.divChequeclass').css('display', 'table-row');
                $('.divChequeDateclass').css('display', 'table-row');
                $('.divBankclass').css('display', 'table-row');
                $('#tdDenomination').css('display', 'none');
                document.getElementById("spnchequeno").innerHTML = "DD No";
                var input = document.getElementById("txtChequeNo");
                input.placeholder = "Enter DD No";
                document.getElementById("txtAmount").value = "";
            }
            if (PaymentType == "Journal Voucher" || PaymentType == "Incentive") {
                $('.divChequeclass').css('display', 'table-row');
                $('.clsclosingbalance').css('display', 'block ');
                $('#tdDenomination').css('display', 'none');
                document.getElementById("spnchequeno").innerHTML = "Voucher No";
                var input = document.getElementById("txtChequeNo");
                input.placeholder = "Enter Journal Voucher";
                if (collectiontype == "Agent") {
                    $('.divDebitclass').css('display', 'table-row');
                }
                else {
                    $('.divDebitclass').css('display', 'none');
                }

                       
                if (PaymentType == "Incentive") {
                    $('.divfinanceaccno').css('display', 'none');
                    $('.divBankclass').css('display', 'none');
                    $('.divDebitclass').css('display', 'table-row');
                    $('#tdDenomination').css('display', 'none');
                }
                //                 }
                document.getElementById("txtAmount").innerHTML = "";
            }
        }
        var Freezertype;
        function ddlFreezerPurposeChange(FreezerPurpose) {
            Freezertype = FreezerPurpose.options[FreezerPurpose.selectedIndex].text;
            if (Freezertype == "freezer deposit") {
                $('.divfreezerclass').css('display', 'table-row');
                $('.divfreezerAmounttypeclass').css('display', 'table-row');
//                $('.divtranstypeclass').css('display', 'table-row');
            }
            else {
                $('.divfreezerclass').css('display', 'none');
                $('.divfreezerAmounttypeclass').css('display', 'none');
//                $('.divtranstypeclass').css('display', 'none');
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

        var othersnames = [];
        function get_Others_Details() {
            var data = { 'operation': 'get_Others_Details' };
            var s = function (msg) {
                if (msg) {
                    othersnames = msg;
                    var availableTags = [];
                    for (i = 0; i < msg.length; i++) {
                        availableTags.push(msg[i].name);
                    }
                    $("#txtDesc").autocomplete({
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
            var desc = document.getElementById('txtDesc').value;
            for (var i = 0; i < othersnames.length; i++) {
                if (desc == othersnames[i].name) {
                    document.getElementById('txtLedgerCode').value = othersnames[i].ledgercode;
                    document.getElementById('txtHiddenName').value = othersnames[i].name;
                }
            }
        }
        function ddlAgentNameChange(id) {
            BtnGetAmountDeatailsClick();
        }
        function BtnGetAmountDeatailsClick() {
            var ddlRouteName = document.getElementById('ddlRouteName').value;
            if (ddlRouteName == "Select Route Name" || ddlRouteName == "") {
                alert("Select Route Name");
                return false;
            }
            var ddlAgentName = document.getElementById('ddlAgentName').value;
            if (ddlAgentName == "Select Agent Name" || ddlAgentName == "") {
                alert("Select Agent Name");
                return false;
            }
            var DOE = document.getElementById('datepicker').value;
            if (DOE == "") {
                alert("Select Date");
                return false;
            }
            var data = { 'operation': 'GetAgentAmountDeatails', 'BranchID': ddlAgentName, 'DOE': DOE };
            var s = function (msg) {
                if (msg) {
                    document.getElementById('txtOppBal').innerHTML = msg[0].TotalAmount;
                    var TotAmount = parseFloat(msg[0].TotalAmount);
                    document.getElementById('txtTotAmount').innerHTML = parseFloat(TotAmount).toFixed(2);
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
            var ddlHeads = document.getElementById('ddlfaaccno');
            var length = ddlHeads.options.length;
            ddlHeads.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select account number";
            ddlHeads.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].accountno != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].accountno;
                    opt.value = msg[i].sno;
                    ddlHeads.appendChild(opt);
                }
            }
        }
        function AmountReceivingChange(Amount) {
            var ddltransactiontype = document.getElementById('ddltransactiontype').value;
            if (ddltransactiontype == "Credit") {
                if (Amount.value == "") {
                    var TotAmount = document.getElementById('txtTotAmount').innerHTML;
                    var paidAmount = 0;
                    var CloBal = parseFloat(TotAmount) - parseFloat(paidAmount);
                    document.getElementById('txtCloBal').innerHTML = CloBal;
                }
                else {
                    var TotAmount = document.getElementById('txtTotAmount').innerHTML;
                    var paidAmount = Amount.value;
                    var CloBal = parseFloat(TotAmount) - parseFloat(paidAmount);
                    document.getElementById('txtCloBal').innerHTML = CloBal;
                }
            }
            if (ddltransactiontype == "Debit") {
                if (Amount.value == "") {
                    var TotAmount = document.getElementById('txtTotAmount').innerHTML;
                    var paidAmount = 0;
                    var CloBal = parseFloat(TotAmount) + parseFloat(paidAmount);
                    document.getElementById('txtCloBal').innerHTML = CloBal;
                }
                else {
                    var TotAmount = document.getElementById('txtTotAmount').innerHTML;
                    var paidAmount = Amount.value;
                    var CloBal = parseFloat(TotAmount) + parseFloat(paidAmount);
                    document.getElementById('txtCloBal').innerHTML = CloBal;
                }
            }

        }

        function BindHeads(msg) {
            var ddlHeads = document.getElementById('combobox');
            var length = ddlHeads.options.length;
            ddlHeads.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            ddlHeads.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].HeadName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].HeadName;
                    opt.value = msg[i].Sno;
                    ddlHeads.appendChild(opt);
                }
            }
        }
        function FillHeads() {
            var data = { 'operation': 'GetHeadNames' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindHeads(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Collections<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Collections</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Collections Details
                </h3>
            </div>
            <div class="box-body">
                <table style="width:100%;">
                    <tr>
                        <td style="width:42%;float:left;">
                            <table >
                                <tr>
                                    <td>
                                        <label>Collection Type</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="ddlcollectiontype" class="form-control" onchange="ddlCollectionTypeChange(this);">
                                            <option>Select</option>
                                            <option>SD Deposit</option>
                                            <option>Other</option>
                                            <option>SalesOfficeCollection</option>
                                        </select>
                                    </td>
                                </tr>
                                <tr class="clspurpose">
                                    <td>
                                        <label>Purpose</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="ddlAmountType" class="form-control" onchange="ddlFreezerPurposeChange(this);">
                                            <option>Select</option>
                                            <option>freezer deposit</option>
                                            <option>Others</option>
                                        </select>
                                    </td>
                                </tr>
                                <tr class="divsalesoffclass" style="display: none;">
                                    <td>
                                        <label>Sales Office</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="ddlSalesOffice" class="form-control" onchange="ddlSalesOfficeChange(this);">
                                        </select>
                                    </td>
                                </tr>
                                <tr class="divRouteclass" style="display: none;">
                                    <td>
                                        <label>Route Name</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="ddlRouteName" class="form-control" onchange="ddlRouteNameChange(this);">
                                        </select>
                                    </td>
                                </tr>
                                <tr class="divAgentclass" style="display: none;">
                                    <td>
                                        <label>Agent Name</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="ddlAgentName" class="form-control" onchange="ddlAgentNameChange(this);">
                                        </select>
                                    </td>
                                </tr>
                                 <tr class="divnameclass" style="display: none;">
                                    <td id="txtname">
                                      <label>  Name </label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="text" id="txtDesc" class="form-control" placeholder="Enter Name" />
                                    </td>
                                    <td style="height: 40px;">
                            <input id="txtHiddenName" type="hidden" class="form-control" name="HiddenName" />
                        </td>
                                </tr>
                                <tr class="divcodeclass" style="display: none;">
                                <td id="txtCode">
                                       <label> LedgerCode </label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="text" id="txtLedgerCode" class="form-control" placeholder="Enter LedgerCode" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Date:</label>
                                    </td>
                                    <td  style="height:20px;">
                                        <input type="date" id="datepicker" class="form-control"/>
                                    </td>
                                    
                                </tr>
                                <tr class="lblopening" style="display:none;">
                                    <td>
                                        <label>Opening Balance</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <span id="txtOppBal" style="font-size: 18px; color: Red; font-weight: bold;"></span>
                                    </td>
                                </tr>
                                <tr class="lblpaidamount" style="display:none;">
                                    <td>
                                        <label>Amount To Be Paid</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <span id="txtTotAmount" style="font-size: 18px; color: Red; font-weight: bold;">
                                        </span>
                                    </td>
                                </tr>

                               
                                
                                <tr class="divfreezerclass" style="display: none;">
                                    <td>
                                        <label>Freezer Type</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="ddlfreezertype" class="form-control">
                                            <option>100 ltrs</option>
                                            <option>200 ltrs</option>
                                            <option>300 ltrs</option>
                                            <option>500 ltrs</option>
                                        </select>
                                    </td>
                                </tr>
                                <tr class="divtranstypeclass" style="display: none;">
                                    <td>
                                        <label>Trans Type</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="ddlTransType" class="form-control">
                                            <option>Debit</option>
                                            <option>Credit</option>
                                        </select>
                                    </td>
                                </tr>
                                <tr class="divfreezerAmounttypeclass" style="display: none;">
                                    <td>
                                        <label>Freezer Amount Type</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="ddlfreezeramounttype" class="form-control">
                                            <option>Deposit</option>
                                            <option>Installment</option>
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>Payment Type</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="ddlPaymentType" class="form-control" onchange="ddlPaymentTypeChange(this);">
                                            <option>Select</option>
                                            <option>Cash</option>
                                            <option>PhonePay</option>
                                            <option>Cheque</option>
                                            <option>DD</option>
                                            <option>Bank Transfer</option>
                                            <option>Incentive</option>
                                           <%-- <option>Journal Voucher</option>--%>
                                        </select>
                                    </td>
                                </tr>
                                <tr class="divTransactionclass" style="display: none;">
                                    <td>
                                        <label>Transaction Type</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="ddltransactiontype" class="form-control">
                                            <option selected="selected">Credit</option>
                                            <option>Debit</option>
                                        </select>
                                    </td>
                                </tr>
                                <tr class="divChequeclass" style="display: none;">
                                    <td>
                                        <label id="spnchequeno">Cheque No</label>
                                    </td>
                                    <td id="divCheque" style="height: 40px;">
                                        <input type="text" id="txtChequeNo" class="form-control" placeholder="Enter Cheque No" />
                                    </td>
                                </tr>
                                <tr class="divfinanceaccno" style="display: none;">
                                    <td>
                                        <label>Finance AccountNo</label>
                                    </td>
                                    <td  style="height: 40px;">
                                      <select id="ddlfaaccno" class="form-control"></select>
                                    </td>
                                </tr>
                                <tr class="divBankclass" style="display: none;">
                                    <td>
                                        <label>Bank Name</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="text" id="txtBankName" class="form-control" placeholder="Enter Bank Name" />
                                    </td>
                                </tr>
                                <tr class="divDebitclass" style="display: none;">
                                    <td>
                                        <label>Debit A/C</label>
                                    </td>
                                    <td style="height: 40px;">
                                      <select id="combobox" class="form-control">
                                    </select>
                                        
                                    </td>
                                </tr>
                                <tr class="divChequeDateclass" style="display: none;">
                                    <td>
                                        <label>Cheque Date</label>
                                    </td>
                                    <td id="divchequedate" style="height: 40px;">
                                        <input type="date" id="dtchequedate" placeholder="DD-MM-YYYY" class="form-control" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>Amount Receiving</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="text" id="txtAmount" class="form-control" placeholder="Enter Amount" onkeyup="AmountReceivingChange(this);"/>
                                    </td>
                                </tr>

                                <tr class="clsclosingbalance" style="display:none;">
                                    <td>
                                        <label>Closing Balance</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <span id="txtCloBal" style="font-size: 18px; color: Red; font-weight: bold;"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    <label>
                                    Remarks
                                    </label>
                                        
                                    </td>
                                    <td style="height: 40px;">
                                        <textarea rows="5" cols="45" id="txtCashierRemarks" class="form-control" maxlength="2000"
                                            placeholder="Enter Remarks"></textarea>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="button" id="btnSave" value="Save" class="btn btn-primary" onclick="BtnCashAmountClick();" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width:52%;float:left;" id="tdDenomination">
                        <div style="padding-left: 31%;">
                        <table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;"
                                            id="tableCollectionDetails" class="mainText2" border="1">
                                            <thead>
                                                <tr>
                                                    <td style="width: 25%; height: 20px; color: #2f3293; font-size: 18px; font-weight: bold;
                                                        text-align: center;">
                                                        Cash
                                                        <br />
                                                    </td>
                                                    <td style="width: 25%; text-align: center; height: 20px; font-size: 18px; font-weight: bold;
                                                        color: #2f3293;">
                                                        Received Amount
                                                        <br />
                                                    </td>
                                                    <td style="width: 25%; text-align: center; height: 20px; font-size: 18px; font-weight: bold;
                                                        color: #2f3293;">
                                                        Return Amount
                                                        <br />
                                                    </td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr class="tblrowcolor">
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <span id="Span2" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;">2000</span>
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                                        <input type="number" id="Number18" onkeyup="ColCountChange(this);" class="AmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Count" />
                                                        <input type="hidden" class="TotalClass" id="Hidden18" value="0" />
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <input type="number" id="Number19" onkeyup="ReturnCountChange(this);" class="RAmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Return" />
                                                        <input type="hidden" class="ReturnAmountClass" id="Hidden19" value="0" />
                                                    </td>
                                                </tr>
                                                <tr class="tblrowcolor">
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <span id="txtsno" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>500</b></span>
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                                        <input type="number" id="txtCount" onkeyup="ColCountChange(this);" class="AmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Count" />
                                                        <input type="hidden" class="TotalClass" id="Hidden1" value="0" />
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <input type="number" id="Number10" onkeyup="ReturnCountChange(this);" class="RAmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Return" />
                                                        <input type="hidden" class="ReturnAmountClass" id="Hidden10" value="0" />
                                                    </td>
                                                </tr>
                                                 <tr class="tblrowcolor">
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <span id="Span3" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>200</b></span>
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                                        <input type="number" id="Number8" onkeyup="ColCountChange(this);" class="AmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Count" />
                                                        <input type="hidden" class="TotalClass" id="Hidden9" value="0" />
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <input type="number" id="Number9" onkeyup="ReturnCountChange(this);" class="RAmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Return" />
                                                        <input type="hidden" class="ReturnAmountClass" id="Hidden20" value="0" />
                                                    </td>
                                                </tr>

                                                <tr class="tblrowcolor">
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <span id="Span1" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>100</b></span>
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                                        <input type="number" id="Number1" onkeyup="ColCountChange(this);" class="AmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Count" />
                                                        <input type="hidden" class="TotalClass" id="Hidden2" value="0" />
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <input type="number" id="Number11" onkeyup="ReturnCountChange(this);" class="RAmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Return" />
                                                        <input type="hidden" class="ReturnAmountClass" id="Hidden11" value="0" />
                                                    </td>
                                                </tr>
                                                <tr class="tblrowcolor">
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <span id="Span4" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>50</b></span>
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                                        <input type="number" id="Number2" onkeyup="ColCountChange(this);" class="AmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Count" />
                                                        <input type="hidden" class="TotalClass" id="Hidden3" value="0" />
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <input type="number" id="Number12" onkeyup="ReturnCountChange(this);" class="RAmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Return" />
                                                        <input type="hidden" class="ReturnAmountClass" id="Hidden12" value="0" />
                                                    </td>
                                                </tr>
                                                <tr class="tblrowcolor">
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <span id="Span6" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>20</b></span>
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                                        <input type="number" id="Number3" onkeyup="ColCountChange(this);" class="AmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Count" />
                                                        <input type="hidden" class="TotalClass" id="Hidden4" value="0" />
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <input type="number" id="Number13" onkeyup="ReturnCountChange(this);" class="RAmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Return" />
                                                        <input type="hidden" class="ReturnAmountClass" id="Hidden13" value="0" />
                                                    </td>
                                                </tr>
                                                <tr class="tblrowcolor">
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <span id="Span8" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>10</b></span>
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                                        <input type="number" id="Number4" onkeyup="ColCountChange(this);" class="AmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Count" />
                                                        <input type="hidden" class="TotalClass" id="Hidden5" value="0" />
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <input type="number" id="Number14" onkeyup="ReturnCountChange(this);" class="RAmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Return" />
                                                        <input type="hidden" class="ReturnAmountClass" id="Hidden14" value="0" />
                                                    </td>
                                                </tr>
                                                <tr class="tblrowcolor">
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <span id="Span10" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>5</b></span>
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                                        <input type="number" id="Number5" onkeyup="ColCountChange(this);" class="AmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Count" />
                                                        <input type="hidden" class="TotalClass" id="Hidden6" value="0" />
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <input type="number" id="Number15" onkeyup="ReturnCountChange(this);" class="RAmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Return" />
                                                        <input type="hidden" class="ReturnAmountClass" id="Hidden15" value="0" />
                                                    </td>
                                                </tr>
                                                <tr class="tblrowcolor">
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <span id="Span12" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>2</b></span>
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                                        <input type="number" id="Number6" onkeyup="ColCountChange(this);" class="AmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Count" />
                                                        <input type="hidden" class="TotalClass" id="Hidden7" value="0" />
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <input type="number" id="Number16" onkeyup="ReturnCountChange(this);" class="RAmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Return" />
                                                        <input type="hidden" class="ReturnAmountClass" id="Hidden16" value="0" />
                                                    </td>
                                                </tr>
                                                <tr class="tblrowcolor">
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <span id="Span14" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>1</b></span>
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                                        <input type="number" id="Number7" onkeyup="ColCountChange(this);" class="AmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Count" />
                                                        <input type="hidden" class="TotalClass" id="Hidden8" value="0" />
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <input type="number" id="Number17" onkeyup="ReturnCountChange(this);" class="RAmountClass"
                                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Return" />
                                                        <input type="hidden" class="ReturnAmountClass" id="Hidden17" value="0" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; text-align: center;
                                                        color: Gray;">
                                                        <span style="font-size: 18px; color: Blue;">Total:</span>
                                                    </td>
                                                    <td style="width: 20%; height: 30px; vertical-align: top; font-size: 12px; font-weight: 500;
                                                        text-align: center; padding: 0px 0px 0px 3px">
                                                        <span style="font-size: 16px; color: Red; font-weight: bold;" id="txtAmont">0</span>
                                                    </td>
                                                    <td style="width: 20%; height: 30px; font-size: 11px; vertical-align: middle; text-align: center;
                                                        color: Gray;" align="center">
                                                        <span style="font-size: 16px; color: Red; font-weight: bold;" id="txtreturnAmount">0</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%; height: 30px; vertical-align: middle; text-align: center;
                                                        color: Gray;">
                                                        <span style="font-size: 18px; color: Blue;">Total Amount:</span>
                                                    </td>
                                                    <td style="width: 20%; height: 30px; font-size: 11px; vertical-align: middle; text-align: center;
                                                        color: Gray;" align="center" colspan="2">
                                                        <span id="txtCTotAmount" style="font-size: 16px; color: Red; font-weight: bold;">0
                                                        </span>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </section>
</asp:Content>

