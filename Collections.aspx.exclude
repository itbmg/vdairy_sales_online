<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Collections.aspx.cs" Inherits="Collections" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <%-- <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>--%>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
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
    <style type="text/css">
        .datepicker
        {
            border: 1px solid gray;
            background: url("Images/CalBig.png") no-repeat scroll 99%;
            width: 100%;
            height: 34px;
            padding: 6px 12px;
            font-size: 14px;
            line-height: 1.42857143;
            color: #555;
            cursor: pointer;
            margin: .5em 0;
            padding: .6em 20px;
            filter: Alpha(Opacity=0);
            box-shadow: 3px 3px 3px #ccc;
        }
        .ui-autocomplete
        {
            max-height: 100px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */
            z-index: 1000 !important;
        }
        .custom-combobox
        {
            position: relative;
            display: inline-block;
        }
        .custom-combobox-toggle
        {
            position: absolute;
            top: 0;
            bottom: 0;
            margin-left: -1px;
            padding: 0;
        }
        .custom-combobox-input
        {
            margin: 0;
            height: 35px;
            width: 250px;
        }
    </style>
    <script type="text/javascript">
        (function ($) {
            $.widget("custom.combobox", {
                _create: function () {
                    this.wrapper = $("<span>")
          .addClass("custom-combobox")
          .insertAfter(this.element);

                    this.element.hide();
                    this._createAutocomplete();
                    this._createShowAllButton();
                },

                _createAutocomplete: function () {
                    var selected = this.element.children(":selected"),
          value = selected.val() ? selected.text() : "";

                    this.input = $("<input>")
          .appendTo(this.wrapper)
          .val(value)
          .attr("title", "")
          .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
          .autocomplete({
              delay: 0,
              minLength: 0,
              source: $.proxy(this, "_source")
          })
          .tooltip({
              tooltipClass: "ui-state-highlight"
          });

                    this._on(this.input, {
                        autocompleteselect: function (event, ui) {
                            ui.item.option.selected = true;
                            this._trigger("select", event, {
                                item: ui.item.option
                            });
                        },

                        autocompletechange: "_removeIfInvalid"
                    });
                },

                _createShowAllButton: function () {
                    var input = this.input,
          wasOpen = false;

                    $("<a>")
          .attr("tabIndex", -1)
          .attr("title", "Show All Items")
          .tooltip()
          .appendTo(this.wrapper)
          .button({
              icons: {
                  primary: "ui-icon-triangle-1-s"
              },
              text: false
          })
          .removeClass("ui-corner-all")
          .addClass("custom-combobox-toggle ui-corner-right")
          .mousedown(function () {
              wasOpen = input.autocomplete("widget").is(":visible");
          })
          .click(function () {
              input.focus();

              // Close if already visible
              if (wasOpen) {
                  return;
              }

              // Pass empty string as value to search for, displaying all results
              input.autocomplete("search", "");
          });
                },

                _source: function (request, response) {
                    var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
                    response(this.element.children("option").map(function () {
                        var text = $(this).text();
                        if (this.value && (!request.term || matcher.test(text)))
                            return {
                                label: text,
                                value: text,
                                option: this
                            };
                    }));
                },

                _removeIfInvalid: function (event, ui) {

                    // Selected an item, nothing to do
                    if (ui.item) {
                        return;
                    }

                    // Search for a match (case-insensitive)
                    var value = this.input.val(),
          valueLowerCase = value.toLowerCase(),
          valid = false;
                    this.element.children("option").each(function () {
                        if ($(this).text().toLowerCase() === valueLowerCase) {
                            this.selected = valid = true;
                            return false;
                        }
                    });

                    // Found a match, nothing to do
                    if (valid) {
                        return;
                    }

                    // Remove invalid value
                    this.input
          .val("")
          .attr("title", value + " didn't match any item")
          .tooltip("open");
                    this.element.val("");
                    this._delay(function () {
                        this.input.tooltip("close").attr("title", "");
                    }, 2500);
                    this.input.autocomplete("instance").term = "";
                },

                _destroy: function () {
                    this.wrapper.remove();
                    this.element.show();
                }
            });
        })(jQuery);

        $(function () {
            $("#combobox").combobox();
            $("#toggle").click(function () {
                $("#combobox").toggle();
            });
        });
    </script>
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
            $('#dtchequedate').val(today);
            FillSalesOffice();
            FillHeads();
            get_faAccountNumber();
            // get_finance_Branches();
            var Level = '<%=Session["LevelType"] %>';
            if (Level == "AccountsOfficer") {
                $('.divTransactionclass').css('display', 'table-row');
            }
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
        function ddlAgentNameChange(id) {
            BtnGetAmountDeatailsClick();
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
        function numberOnlyExample() {
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
                    //                   document.getElementById('txtTodayAmount').innerHTML = msg[0].TodayAmount;
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
        function BtnCollectionAmountClick() {

            var AmountReceived = document.getElementById('txtAmountReceiving').value;
            if (AmountReceived == "") {
                alert("Enter Amount Received");
                return false;
            }
            var ddlAgentName = document.getElementById('ddlAgentName').value;
            var Remarks = document.getElementById('txtRemarks').value;
            if (Remarks == "") {
                alert("Enter Remarks");
                return false;
            }
            var soid = document.getElementById('ddlSalesOffice').value;

            var ddlPaymentType = document.getElementById('ddlPaymentType').value;
            var ddltransactiontype = document.getElementById('ddltransactiontype').value;
            var PaidDate = document.getElementById('datepicker').value;
            var chequeDate = document.getElementById('datepicker').value;
            var txtChequeNo = "";
            var txtBankName = "";
            var faaccuntno = "";
            if (ddlPaymentType == "Cheque") {
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
            if (ddlPaymentType == "Bank Transfer") {
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
            if (ddlPaymentType == "DD") {
                txtChequeNo = document.getElementById('txtChequeNo').value;
                if (txtChequeNo == "") {
                    alert("Enter DD No");
                    return false;
                }
            }
            if (ddlPaymentType == "Journal Voucher" || PaymentType == "Incentive") {
                txtChequeNo = document.getElementById('txtChequeNo').value;
                if (txtChequeNo == "") {
                    alert("Enter Journal Voucher");
                    return false;
                }
                var Head = document.getElementById("combobox");
                HeadSno = Head.options[Head.selectedIndex].value;
                var HeadOfAccount = Head.options[Head.selectedIndex].text;
            }
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
            if (AmountReceived == txt_Total) {
            }
            else {
                alert("Please fill denomination amount");
                return false;
            }
            var data = { 'operation': 'btnCollectionAmountClick', 'HeadSno': HeadSno, 'BranchID': ddlAgentName, 'Amount': AmountReceived, 'Remarks': Remarks, 'PaymentType': ddlPaymentType, 'ChequeNo': txtChequeNo, 'PaidDate': PaidDate, 'chequeDate': chequeDate, 'BankName': txtBankName, 'ddltransactiontype': ddltransactiontype, 'soid': soid, 'DenominationString': DenominationString, 'ReturnDenominationString': ReturnDenominationString, 'faaccuntno': faaccuntno };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById('txtAmountReceiving').value = "";
                    document.getElementById('txtRemarks').value = "";
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        var PaymentType = "";
        function ddlPaymentTypeChange(Payment) {
            PaymentType = Payment.options[Payment.selectedIndex].text;
            if (PaymentType == "Cash") {
                $('.divChequeclass').css('display', 'none');
                $('.divChequeDateclass').css('display', 'none');
                $('.divBankclass').css('display', 'none');
                $('.divDebitclass').css('display', 'none');
                $('.divfinanceaccno').css('display', 'none');
            }
            if (PaymentType == "Bank Transfer") {
                $('.divChequeclass').css('display', 'none');
                $('.divChequeDateclass').css('display', 'none');
                $('.divBankclass').css('display', 'table-row');
                $('.divDebitclass').css('display', 'none');
                $('.divfinanceaccno').css('display', 'table-row');

            }
            if (PaymentType == "Cheque") {
                $('.divChequeclass').css('display', 'table-row');
                $('.divChequeDateclass').css('display', 'table-row');
                $('.divBankclass').css('display', 'table-row');
                $('.divDebitclass').css('display', 'none');
                document.getElementById("spnchequeno").innerHTML = "Cheque No";
                var input = document.getElementById("txtChequeNo");
                input.placeholder = "Enter Cheque No";
            }
            if (PaymentType == "DD") {
                $('.divChequeclass').css('display', 'table-row');
                $('.divChequeDateclass').css('display', 'table-row');
                $('.divBankclass').css('display', 'table-row');
                $('.divfinanceaccno').css('display', 'none');
                document.getElementById("spnchequeno").innerHTML = "DD No";
                $('.divDebitclass').css('display', 'none');
                var input = document.getElementById("txtChequeNo");
                input.placeholder = "Enter DD No";
            }
            if (PaymentType == "Journal Voucher" || PaymentType == "Incentive") {
                $('.divChequeclass').css('display', 'table-row');
                $('.divDebitclass').css('display', 'table-row');
                document.getElementById("spnchequeno").innerHTML = "Voucher No";
                var input = document.getElementById("txtChequeNo");
                input.placeholder = "Enter Journal Voucher";
            }
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
        //        function get_finance_Branches() {
        //            var data = { 'operation': 'get_finance_Branches' };
        //            var s = function (msg) {
        //                if (msg) {
        //                    if (msg == "Session Expired") {
        //                        alert(msg);
        //                        window.location = "Login.aspx";
        //                    }
        //                    fillfabranches(msg);

        //                }
        //                else {
        //                }
        //            };
        //            var e = function (x, h, e) {
        //            };
        //            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        //            callHandler(data, s, e);
        //        }
        //        function fillfabranches(msg) {
        //            var ddlHeads = document.getElementById('ddlfabranch');
        //            var length = ddlHeads.options.length;
        //            ddlHeads.options.length = null;
        //            var opt = document.createElement('option');
        //            opt.innerHTML = "Select BranchName";
        //            ddlHeads.appendChild(opt);
        //            for (var i = 0; i < msg.length; i++) {
        //                if (msg[i].branchname != null) {
        //                    var opt = document.createElement('option');
        //                    opt.innerHTML = msg[i].branchname;
        //                    opt.value = msg[i].branchid;
        //                    ddlHeads.appendChild(opt);
        //                }
        //            }
        //        }



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
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 50%;">
                            <table align="center">
                                <tr>
                                    <td>
                                        <label>Sales Office</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="ddlSalesOffice" class="form-control" onchange="ddlSalesOfficeChange(this);">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>Route Name</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="ddlRouteName" class="form-control" onchange="ddlRouteNameChange(this);">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>Agent Name</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="ddlAgentName" class="form-control" onchange="ddlAgentNameChange(this);">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Date:</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <input type="date" id="datepicker" placeholder="DD-MM-YYYY" class="form-control" />
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <input type="button" id="Button1" value="Get Details" class="btn btn-primary" onclick="BtnGetAmountDeatailsClick();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>Opening Balance</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <span id="txtOppBal" style="font-size: 18px; color: Red; font-weight: bold;"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>Amount To Be Paid</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <span id="txtTotAmount" style="font-size: 18px; color: Red; font-weight: bold;">
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>Payment Type</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="ddlPaymentType" class="form-control" onchange="ddlPaymentTypeChange(this);">
                                            <option>Cash</option>
                                            <option>Cheque</option>
                                            <option>DD</option>
                                            <option>Bank Transfer</option>
                                            <option>Incentive</option>
                                            <option>Journal Voucher</option>
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
                                        <span id="spnchequeno">Cheque No</span>
                                    </td>
                                    <td id="divCheque" style="height: 40px;">
                                        <input type="text" id="txtChequeNo" class="form-control" placeholder="Enter Cheque No" />
                                    </td>
                                </tr>
                              
                                <tr class="divfinanceaccno" style="display: none;">
                                    <td>
                                        <label>Finance AccountNo</label>
                                    </td>
                                    <td style="height: 40px;">
                                      <select id="ddlfaaccno" class="form-control">
                                        </select>
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
                                        <input type="text" id="txtAmountReceiving" class="form-control" onkeyup="AmountReceivingChange(this);"
                                            placeholder="Enter Amount" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>Closing Balance</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <span id="txtCloBal" style="font-size: 18px; color: Red; font-weight: bold;"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>Remarks</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <textarea rows="4" cols="45" id="txtRemarks" class="ddlsize" maxlength="2000" placeholder="Enter Remarks"></textarea>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 50%;">
                            <table align="center">
                                <tr>
                                    <td>
                                    </td>
                                    <td align="center">
                                        <label style="font-size: 18px; color: Black;">Denomination</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
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
                                    <td align="center">
                                        <input type="button" id="btnSave" value="Save" class="btn btn-primary" onclick="BtnCollectionAmountClick();" />
                                          <input type="button" class="btn btn-danger" id="close_collection" value="Close" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </section>
</asp:Content>
