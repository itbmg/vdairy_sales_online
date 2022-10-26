<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CashForm.aspx.cs" Inherits="CashForm" %>

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
   <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
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
        .removeclass
        {
            background: #fd2053 !important;
            border-radius: 100% !important;
            padding: 0px !important;
            height: 30px !important;
            width: 30px !important;
            color: #ffffff !important;
            border-color: #fd2053 !important;
        }
        .prntcls
        {
            background: #00c0ef !important;
            border-radius: 100% !important;
            padding: 0px !important;
            height: 30px !important;
            width: 30px !important;
            color: #ffffff !important;
            border-color: #00c0ef !important;
        }
    </style>
    <script type="text/javascript">
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=300,height=300,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.write('<link rel="stylesheet" type="text/css" href="Css/print.css" />');
            newWin.document.close();
        }
//        (function ($) {
//            $.widget("custom.combobox", {
//                _create: function () {
//                    this.wrapper = $("<span>")
//          .addClass("custom-combobox")
//          .insertAfter(this.element);

//                    this.element.hide();
//                    this._createAutocomplete();
//                    this._createShowAllButton();
//                },

//                _createAutocomplete: function () {
//                    var selected = this.element.children(":selected"),
//          value = selected.val() ? selected.text() : "";

//                    this.input = $("<input>")
//          .appendTo(this.wrapper)
//          .val(value)
//          .attr("title", "")
//          .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
//          .autocomplete({
//              delay: 0,
//              minLength: 0,
//              source: $.proxy(this, "_source")
//          })
//          .tooltip({
//              tooltipClass: "ui-state-highlight"
//          });

//                    this._on(this.input, {
//                        autocompleteselect: function (event, ui) {
//                            ui.item.option.selected = true;
//                            this._trigger("select", event, {
//                                item: ui.item.option
//                            });
//                        },

//                        autocompletechange: "_removeIfInvalid"
//                    });
//                },

//                _createShowAllButton: function () {
//                    var input = this.input,
//          wasOpen = false;

//                    $("<a>")
//          .attr("tabIndex", -1)
//          .attr("title", "Show All Items")
//          .tooltip()
//          .appendTo(this.wrapper)
//          .button({
//              icons: {
//                  primary: "ui-icon-triangle-1-s"
//              },
//              text: false
//          })
//          .removeClass("ui-corner-all")
//          .addClass("custom-combobox-toggle ui-corner-right")
//          .mousedown(function () {
//              wasOpen = input.autocomplete("widget").is(":visible");
//          })
//          .click(function () {
//              input.focus();

//              // Close if already visible
//              if (wasOpen) {
//                  return;
//              }

//              // Pass empty string as value to search for, displaying all results
//              input.autocomplete("search", "");
//          });
//                },

//                _source: function (request, response) {
//                    var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
//                    response(this.element.children("option").map(function () {
//                        var text = $(this).text();
//                        if (this.value && (!request.term || matcher.test(text)))
//                            return {
//                                label: text,
//                                value: text,
//                                option: this
//                            };
//                    }));
//                },

//                _removeIfInvalid: function (event, ui) {

//                    // Selected an item, nothing to do
//                    if (ui.item) {
//                        return;
//                    }

//                    // Search for a match (case-insensitive)
//                    var value = this.input.val(),
//          valueLowerCase = value.toLowerCase(),
//          valid = false;
//                    this.element.children("option").each(function () {
//                        if ($(this).text().toLowerCase() === valueLowerCase) {
//                            this.selected = valid = true;
//                            return false;
//                        }
//                    });

//                    // Found a match, nothing to do
//                    if (valid) {
//                        return;
//                    }

//                    // Remove invalid value
//                    this.input
//          .val("")
//          .attr("title", value + " didn't match any item")
//          .tooltip("open");
//                    this.element.val("");
//                    this._delay(function () {
//                        this.input.tooltip("close").attr("title", "");
//                    }, 2500);
//                    this.input.autocomplete("instance").term = "";
//                },

//                _destroy: function () {
//                    this.wrapper.remove();
//                    this.element.show();
//                }
//            });
//        })(jQuery);

//        $(function () {
//            $("#combobox").combobox();
//            $("#toggle").click(function () {
//                $("#combobox").toggle();
//            });
//        });
    </script>
    <script type="text/javascript">
        var LevelType = "";
        $(function () {
            FillApprovalEmploye();
            FillHeads();
            get_Others_Details();
            $("#datepicker").datepicker({ dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
                onSelect: function (selectedDate) {
                }
            });
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                $('.divsalesOffice').css('display', 'table-row');
                $('.divAprovalEmp').css('display', 'none');
                FillSalesOffice();
            }
            else {
                $('.divsalesOffice').css('display', 'none');
                $('.divAprovalEmp').css('display', 'table-row');
            }
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#datepicker').val(today);

            $('#txtFromDate').val(today);
            $('#txtToDate').val(today);
            $('#divMainAddNewRows').css('display', 'none');
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                $('#divSOffice').css('display', 'block');
                $('#div_vivegeneratedet').css('display', 'block');
                $('#divViewVoucher').css('display', 'block');
                Fillso();
                BtnGenerateClick();
            }
            else {
                $('#divSOffice').css('display', 'none');
                $('#div_vivegeneratedet').css('display', 'none');
                $('#divViewVoucher').css('display', 'block');
                divViewVoucherClick();
                BtnGenerateClick();
            }
        });
        function FillHeads() {
            var data = { 'operation': 'GetHeadNames' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindHeads(msg);
//                    BindBillHeads(msg);

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
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
//        function ddlSalesOfficeChange(ID) {
//            var BranchID = ID.value;
//            var data = { 'operation': 'GetHeadNames', 'BranchID': BranchID };
//            var s = function (msg) {
//                if (msg) {
//                    if (msg == "Session Expired") {
//                        alert(msg);
//                        window.location = "Login.aspx";
//                    }
//                    BindHeads(msg);
//                    BindBillHeads(msg);

//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
//        }

//        function BindHeads(msg) {
//            var ddlHeads = document.getElementById('combobox');
//            var length = ddlHeads.options.length;
//            ddlHeads.options.length = null;
//            var opt = document.createElement('option');
//            opt.innerHTML = "select";
//            ddlHeads.appendChild(opt);
//            for (var i = 0; i < msg.length; i++) {
//                if (msg[i].HeadName != null) {
//                    var opt = document.createElement('option');
//                    opt.innerHTML = msg[i].HeadName;
//                    opt.value = msg[i].Sno;
//                    ddlHeads.appendChild(opt);
//                }
//            }
//        }


//        var HeadNames = [];
//        function FillHeads() {
////            var BranchID = ID.value;
//            var data = { 'operation': 'GetHeadNames'};
//            var s = function (msg) {
//                if (msg) {
//                    HeadNames = msg;
//                    var availableTags = [];
//                    for (i = 0; i < msg.length; i++) {
//                        availableTags.push(msg[i].HeadName);
//                    }
//                    $("#combobox").autocomplete({
//                        source: function (req, responseFn) {
//                            var re = $.ui.autocomplete.escapeRegex(req.term);
//                            var matcher = new RegExp("^" + re, "i");
//                            var a = $.grep(availableTags, function (item, index) {
//                                return matcher.test(item);
//                            });
//                            responseFn(a);
//                        },
//                        change: GetHeadsId,
//                        autoFocus: true
//                    });
//                }
//            }
//            var e = function (x, h, e) {
//                alert(e.toString());
//            };
//            callHandler(data, s, e);
//        }

        var HeadNames = [];var hedadlist = [];
        function BindHeads(msg) {
            hedadlist = msg;
            for (var i = 0; i < msg.length; i++) {
                var HeadName = msg[i].HeadName;
                HeadNames.push(HeadName);
            }
            $('#combobox').autocomplete({
                source: HeadNames,
                change: GetHeadsId,
                autoFocus: true
            });
        }
         
        function GetHeadsId() {
            var HeadName = document.getElementById('combobox').value;
            for (var i = 0; i < hedadlist.length; i++) {
                if (HeadName == hedadlist[i].HeadName) {
                    document.getElementById('hidden_headid').value = hedadlist[i].Sno;
                }
            }
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
        
        function BindBillHeads(msg) {
            var ddlHeads = document.getElementById('ddlBillHead');
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
        function FillApprovalEmploye() {
            var data = { 'operation': 'GetApproveEmployeeNames' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindApprovalEmploye(msg);

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindApprovalEmploye(msg) {
            var ddlEmpApprove = document.getElementById('ddlEmpApprove');
            var length = ddlEmpApprove.options.length;
            ddlEmpApprove.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            ddlEmpApprove.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].UserName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].UserName;
                    opt.value = msg[i].Sno;
                    ddlEmpApprove.appendChild(opt);
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
        var HeadLimit = 0;
        function ddlCashToChange(ID) {
            //var dlCash = ID.value;
            var dlCash = document.getElementById("hidden_headid").value;

            var data = { 'operation': 'GetHeadLimit', 'HeadSno': dlCash };
            var s = function (msg) {
                if (msg) {
                    HeadLimit = msg;
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function BtnRaiseVoucherClick() {
            
            var Description = "";
            var ddlVoucherType = document.getElementById("ddlVoucherType").value;
            var ddlEmployee = "0";
            Description = document.getElementById("txtdesc").value;
            var CashType = "0";
            if (ddlVoucherType == "Credit") {
                CashType = document.getElementById("ddlCashType").value;
            }
            var txtAMount = document.getElementById("txtAMount").innerHTML;
            var txtRemarks = document.getElementById("txtRemarks").value;
            var ddlEmpApprove = document.getElementById("ddlEmpApprove").value;
            var LevelType = '<%=Session["LevelType"] %>';
            if (Description == "") {
                alert("Enter Pay To");
                return false;
            }
            if (txtAMount == "") {
                alert("Enter Amount");
                return false;
            }
            if (txtRemarks == "") {
                alert("Enter Remarks");
                return false;
            }
            var ddlBillHead = "";
            if (ddlVoucherType == "Credit") {
                ddlBillHead = document.getElementById("ddlBillHead").value;
                CashType = document.getElementById("ddlCashType").value;
                if (CashType == "" || CashType == "Select") {
                    alert("Select Cash Type");
                    return false;
                }
                if (CashType == "Cash") {
                }
                else {
                    if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                    }
                    else {
                        if (ddlEmpApprove == "" || ddlEmpApprove == "select") {
                            alert("Select Approval Name");
                            return false;
                        }
                    }
                }
            }
            if (ddlVoucherType == "Debit") {
                if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                }
                else {
                    if (ddlEmpApprove == "" || ddlEmpApprove == "select") {
                        alert("Select Approval Name");
                        return false;
                    }
                }
            }
            var CashType = "";
            if (ddlVoucherType == "Due") {
                CashType = document.getElementById("ddlCashType").value;
                if (CashType == "" || CashType == "Select") {
                    alert("Select Cash Type");
                    return false;
                }
                if (CashType == "Cash") {
                }
                else {
                    if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                    }
                    else {
                        if (ddlEmpApprove == "" || ddlEmpApprove == "select") {
                            alert("Select Approval Name");
                            return false;
                        }
                    }
                }
            }
            if (ddlVoucherType == "") {
                alert("Select Voucher Type");
                return false;
            }
            //            var Head = document.getElementById("combobox");
            var HeadLimit = document.getElementById("hidden_headid").value;
            var ddlCashTo = document.getElementById("combobox").value; ;
            //var HeadLimit = Head.options[Head.selectedIndex].value;
            //            var ddlCashTo = Head.options[Head.selectedIndex].text;
           
            var count = 0;
            var IName = "";
            $('.AccountClass').each(function (i, obj) {
                if (count == 0) {
                    IName = $(this).text();
                    count++;

                }
            });
            ddlCashTo = IName;
            var salaryamount = document.getElementById("spnsalary").innerHTML;
            var TotRate = document.getElementById("txtAMount").innerHTML;
            var transactiontype = document.getElementById("ddltransactiontype").value;
            if (transactiontype == "Employee") {
                var employeename = document.getElementById("txtHiddenEName").value;
                var payto = document.getElementById("txtdesc").value;
                if (payto != employeename) {
                    alert("'" + payto + "'" + "        Employee Not Avilable");
                    return false;
                }
            }
            else {
                var othersname = document.getElementById("txtHiddenOName").value;
                var payto = document.getElementById("txtdesc").value;
                //                if (payto != othersname) {
                //                    // alert(payto  +"        Pay Not Avilable");
                //                    alert("'" + payto + "'" + "     Pay To Not Avilable");
                //                    return false;
                //                }
            }
            var salesOffice = document.getElementById("ddlsalesOffice").value;
            var btnSave = document.getElementById("btnSave").value;
            var spnVoucherID = document.getElementById("spnVoucherID").innerHTML;
            var rows = $("#tableCashFormdetails tr:gt(0)");
            var Cashdetails = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtProductQty').val() != "") {
                    //                    TotRate += $(this).find('#txtamount').text();
                    Cashdetails.push({ SNo: $(this).find('#hdnHeadSno').val(), Account: $(this).find('#txtAccount').text(), amount: $(this).find('#txtamount').text() });
                }
            });
            if (!confirm("Do you want to save this transaction")) {
                return false;
            }
            var data = { 'operation': 'BtnRaiseVoucherClick', 'Cashdetails': Cashdetails, 'Description': Description, 'Amount': txtAMount, 'Remarks': txtRemarks, 'EmpApprove': ddlEmpApprove, 'VoucherType': ddlVoucherType, 'CashTo': ddlCashTo, 'Employee': ddlEmployee, 'btnSave': btnSave, 'spnVoucherID': spnVoucherID, 'CashType': CashType, 'ddlBillHead': ddlBillHead, 'BranchID': salesOffice, 'transactiontype': transactiontype };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById("ddlVoucherType").selectedIndex = 0;
                    document.getElementById("btnSave").value = "Raise";
                    document.getElementById("spnVoucherID").innerHTML = "";
                    document.getElementById("txtdesc").value = "";
                    document.getElementById("txtRemarks").value = "";
                    document.getElementById("ddlEmpApprove").selectedIndex = 0;
                    document.getElementById("txtAMount").innerHTML = "";
                    Cashform = [];
                    //                    $('#divHeadAcount').setTemplateURL('CashForm.htm');
                    //                    $('#divHeadAcount').processTemplate(Cashform);
                    $("#divHeadAcount").html("");
                    $("#divHeadAcount").css("display", "none");
                    divRaiseVoucherClick();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(data, s, e);
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
        function divRaiseVoucherClick() {
            $('#divRaiseVoucher').css('display', 'block');
            $('#divViewVoucher').css('display', 'none');
            $('#divVocherPayable').css('display', 'none');
            $('#div_raisevoucher').css('display', 'none');
        }
        function BtnclearClick() {
            $('#divRaiseVoucher').css('display', 'none');
            $('#divViewVoucher').css('display', 'block');
            $('#divVocherPayable').css('display', 'none');
            $('#div_raisevoucher').css('display', 'block');
            $('#divMainAddNewRows').css('display', 'none');
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                $('#divSOffice').css('display', 'block');
                Fillso();
                $('#div_vivegeneratedet').css('display', 'block');
                $('#divViewVoucher').css('display', 'block');
            }
            else {
                $('#divSOffice').css('display', 'none');
                $('#div_vivegeneratedet').css('display', 'none');
                divViewVoucherClick();
                BtnGenerateClick();
                $('#divViewVoucher').css('display', 'block');
            }
            document.getElementById("ddlVoucherType").selectedIndex = 0;
            document.getElementById("btnSave").value = "Raise";
            document.getElementById("spnVoucherID").innerHTML = "";
            document.getElementById("txtdesc").value = "";
            document.getElementById("txtRemarks").value = "";
            document.getElementById("ddlEmpApprove").selectedIndex = 0;
            document.getElementById("txtAMount").innerHTML = "";
            document.getElementById("txtCashAmount").value = "";

            Cashform = [];
           

            $("#divHeadAcount").html("");
            document.getElementById("combobox").value = "";
//            $('#combobox').focus().val('');
            //$('#combobox').autocomplete('');
            
        }
        function BtnPayVoucher_close() {
            $('#divRaiseVoucher').css('display', 'none');
            $('#divViewVoucher').css('display', 'block');
            $('#divVocherPayable').css('display', 'none');
            $('#div_raisevoucher').css('display', 'block');
            $('#divMainAddNewRows').css('display', 'none');
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                $('#divSOffice').css('display', 'block');
                Fillso();
                $('#div_vivegeneratedet').css('display', 'block');
                $('#divViewVoucher').css('display', 'block');
            }
            else {
                $('#divSOffice').css('display', 'none');
                $('#div_vivegeneratedet').css('display', 'none');
                divViewVoucherClick();
                BtnGenerateClick();
                $('#divViewVoucher').css('display', 'block');
            }
        }
        function divViewVoucherClick() {
            $('#divRaiseVoucher').css('display', 'none');
            $('#divViewVoucher').css('display', 'block');
            $('#divVoucherGrid').css('display', 'block');
            $('#divVocherPayable').css('display', 'none');
            $('#divVarifyVoucher').css('display', 'none');
            $('#printvoucher').css('display', 'none');
            $('#divMainAddNewRows').css('display', 'none');
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                $('#divSOffice').css('display', 'block');
                Fillso();
            }
            else {
                $('#divSOffice').css('display', 'none');
            }
        }
        function Fillso() {
            var data = { 'operation': 'GetSalesOffice' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindddlSo(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindddlSo(msg) {
            var ddlsalesOffice = document.getElementById('ddlSo');
            var length = ddlsalesOffice.options.length;
            ddlsalesOffice.options.length = null;
            var opt = document.createElement('option');
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlsalesOffice.appendChild(opt);
                }
            }
        }
        function payvoucher(thisid) {
            //            Denominationzero();
            $('#divRaiseVoucher').css('display', 'none');
            $('#divVarifyVoucher').css('display', 'none');
            $('#divCashierRemarks').css('display', 'block');
            var VoucherID = $(thisid).parent().parent().children('.1').html();
            var VoucherType = $(thisid).parent().parent().children('.2').html();
            var Amount = $(thisid).parent().parent().children('.3').html();
            var ApproveEmpName = $(thisid).parent().parent().children('.4').html();
            var ApprovalAmount = $(thisid).parent().parent().children('.5').html();
            var Status = $(thisid).parent().parent().children('.6').html();
            var Remarks = $(thisid).parent().parent().children('.7').html();
            var BranchID = $(thisid).parent().parent().children('.8').html();
            var CashTo = $(thisid).parent().parent().children('.9').html();
            var ApprovalRemarks = $(thisid).parent().parent().children('.12').html();
            var Empid = $(thisid).parent().parent().children('.11').html();
            var ApprovedBy = $(thisid).parent().parent().children('.10').html();
            var Description = $(thisid).parent().parent().children('.13').html();
            if (Status == "Raised") {
                Status = "Raised";
                $('.divpay').css('display', 'table-row');
                $('.divCashierRemarks').css('display', 'table-row');
                alert("Voucher Not Approved");
                return false;
                $('#divVocherPayable').css('display', 'none');
                $('#divViewVoucher').css('display', 'block');
            }
            if (Status == "Approved") {
                Status = "Approved";
                $('.divpay').css('display', 'table-row');
                $('#divVocherPayable').css('display', 'block');
                $('.divCashierRemarks').css('display', 'table-row');
                $('#divViewVoucher').css('display', 'none');
            }
            if (Status == "Rejected") {
                Status = "Rejected";
                $('.divpay').css('display', 'table-row');
                $('.divCashierRemarks').css('display', 'table-row');
                alert("Voucher is cancelled");
                return false;
            }
            if (Status == "Paid") {
                Status = "Paid";
                $('.divCashierRemarks').css('display', 'none');
                $('.divpay').css('display', 'none');

                alert("Voucher already paid");
                return false;
            }
            document.getElementById("txtVoucherID").innerHTML = VoucherID;
            document.getElementById("spnVoucherType").innerHTML = VoucherType;
            document.getElementById("spnCashTo").innerHTML = CashTo;
            document.getElementById("spnDescription").innerHTML = Description;
            document.getElementById("spnAmount").innerHTML = Amount;
            document.getElementById("spnApprovalAmount").innerHTML = ApprovalAmount;
            document.getElementById("spnApprovalEmp").innerHTML = ApproveEmpName;
            document.getElementById("spnStatus").innerHTML = Status;
            document.getElementById("spnApprovalEmp").innerHTML = ApproveEmpName;
            if (Status == "Raised") {
                $('.divpay').css('display', 'none');
                $('.divforce').css('display', 'table-row');
            }
            else {
                $('.divforce').css('display', 'none');
            }
            document.getElementById("spnApprovalRemarks").innerHTML = ApprovalRemarks;
            document.getElementById("spnRemarks").innerHTML = ApprovalRemarks;
            document.getElementById("hdnAprovalEmpid").value = ApprovedBy;
            document.getElementById("hdnEmpID").value = Empid;
        }
        function divVarifyVoucherClick() {
            $('#divRaiseVoucher').css('display', 'none');
            $('#divViewVoucher').css('display', 'none');
            $('#divVocherPayable').css('display', 'none');
            $('#divVarifyVoucher').css('display', 'none');
        }
        function BtnGetVoucherClick() {
            var VoucherID = document.getElementById("txtVoucherID").innerHTML;
            if (VoucherID == "") {
                alert("Enter Voucher ID");
                return false;
            }
            var data = { 'operation': 'BtnGetVoucherClick', 'VoucherID': VoucherID };
            var s = function (msg) {
                if (msg) {
                    if (msg == "No voucher found") {
                        alert(msg);
                        return false;
                    }
                    else {
                        var Status = msg[0].Status;
                        if (Status == "R") {
                            Status = "Raised";
                            $('.divpay').css('display', 'table-row');
                            $('.divCashierRemarks').css('display', 'table-row');
                            alert("Voucher Not Approved");
                            return false;
                        }
                        if (Status == "A") {
                            Status = "Approved";
                            $('.divpay').css('display', 'table-row');
                            $('.divCashierRemarks').css('display', 'table-row');
                        }
                        if (Status == "C") {
                            Status = "Rejected";
                            $('.divpay').css('display', 'table-row');
                            $('.divCashierRemarks').css('display', 'table-row');
                            alert("Voucher is cancelled");
                            return false;
                        }
                        if (Status == "P") {
                            Status = "Paid";
                            $('.divCashierRemarks').css('display', 'none');
                            $('.divpay').css('display', 'none');
                            alert("Voucher already paid");
                            return false;
                        }
                        document.getElementById("spnEmpName").innerHTML = msg[0].EmpName;
                        var VoucherType = msg[0].VoucherType;
                        document.getElementById("spnVoucherType").innerHTML = VoucherType;
                        document.getElementById("spnCashTo").innerHTML = msg[0].CashTo;
                        document.getElementById("spnDescription").innerHTML = msg[0].Description;
                        document.getElementById("spnAmount").innerHTML = msg[0].Amount;
                        document.getElementById("spnApprovalAmount").innerHTML = msg[0].ApprovalAmount;
                        document.getElementById("spnApprovalEmp").innerHTML = msg[0].ApproveEmpName;

                        document.getElementById("spnStatus").innerHTML = Status;
                        if (Status == "Raised") {
                            $('.divpay').css('display', 'none');
                            $('.divforce').css('display', 'table-row');
                        }
                        else {
                            $('.divforce').css('display', 'none');

                        }
                        document.getElementById("spnApprovalRemarks").innerHTML = msg[0].ApprovalRemarks;
                        document.getElementById("spnRemarks").innerHTML = msg[0].Remarks;
                        document.getElementById("hdnAprovalEmpid").value = msg[0].ApprovedBy;
                        document.getElementById("hdnEmpID").value = msg[0].Empid;
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
        function BtnGetVarifyVoucherClick() {
            var VoucherID = document.getElementById("txtVarifyVoucherID").value;
            if (VoucherID == "") {
                alert("Enter Voucher ID");
                return false;
            }
            var data = { 'operation': 'BtnGetVoucherClick', 'VoucherID': VoucherID };
            var s = function (msg) {
                if (msg) {
                    if (msg == "No voucher found") {
                        alert(msg);
                        return false;
                    }
                    else {

                        var VoucherType = msg[0].VoucherType;
                        var Status = msg[0].Status;
                        if (Status == "P") {
                        }
                        else {
                            alert("This voucher is not Paid");
                            return false;
                        }
                        if (VoucherType == "Due") {
                            $('#divClearViucher').css('display', 'table-row');
                        }
                        else {
                            $('#divClearViucher').css('display', 'none');
                            alert("This voucher is not varified");
                            return false;
                        }
                        document.getElementById("spnVarifyAmount").innerHTML = msg[0].ApprovalAmount;
                        document.getElementById("sonVarifyRemarks").innerHTML = msg[0].Remarks;
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
        function BtnPayVoucherClick() {
            var VoucherID = document.getElementById("txtVoucherID").innerHTML;
            var ApprovalAmount = document.getElementById("spnApprovalAmount").innerHTML;
            if (ApprovalAmount == "") {
                alert("Approval Amount can not be null");
                return false;
            }
            var DOE = document.getElementById('datepicker').value;
            var Remarks = document.getElementById("txtCashierRemarks").innerHTML;
            var VoucherType = document.getElementById("spnVoucherType").innerHTML;
            var Force = 0;
            var chkforce = document.getElementById("chkforce").checked;
            if (chkforce == true) {
                var Force = 1;
            }
            else {
                var Force = 0;
            }
            var rowsdenominations = $("#tableReportingDetails tr:gt(0)");
            var DenominationString = "";
            $(rowsdenominations).each(function (i, obj) {
                if ($(this).closest("tr").find(".CashClass").text() == "") {
                }
                else {
                    var str = $(this).closest("tr").find(".CashClass").text();

                    DenominationString += str.trim() + 'x' + $(this).closest("tr").find(".qtyclass").val() + "+";
                }
            });
            if (!confirm("Do you want to save this transaction")) {
                return false;
            }
            var data = { 'operation': 'BtnPayVoucherClick', 'VoucherID': VoucherID, 'ApprovalAmount': ApprovalAmount, 'Remarks': Remarks, 'Force': Force, 'VoucherType': VoucherType, 'DOE': DOE, 'DenominationString': DenominationString };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById("txtVoucherID").innerHTML = "";
                    document.getElementById("spnApprovalAmount").innerHTML = "";
                    document.getElementById("txtCashierRemarks").value = "";
                    document.getElementById("spnVoucherType").innerHTML = "";
                    document.getElementById("spnCashTo").innerHTML = "";
                    document.getElementById("spnDescription").innerHTML = "";
                    document.getElementById("spnEmpName").innerHTML = "";
                    document.getElementById("spnAmount").innerHTML = "";
                    document.getElementById("spnRemarks").innerHTML = "";
                    document.getElementById("spnApprovalRemarks").innerHTML = "";
                    document.getElementById("spnApprovalEmp").innerHTML = "";
                    document.getElementById("spnStatus").innerHTML = "";
                    $('#divClearViucher').css('display', 'none');
                    $('.qtyclass input').val("0");
                    $('#tableReportingDetails tr').each(function () {
                        $(this).find('.qtyclass').val("0");
                    });
                    $('#divRaiseVoucher').css('display', 'none');
                    $('#divViewVoucher').css('display', 'block');
                    $('#divVocherPayable').css('display', 'none');
                    $('#div_raisevoucher').css('display', 'block');
                    $('#divMainAddNewRows').css('display', 'none');
                    var LevelType = '<%=Session["LevelType"] %>';
                    if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                        $('#divSOffice').css('display', 'block');
                        Fillso();
                        $('#div_vivegeneratedet').css('display', 'block');
                        $('#divViewVoucher').css('display', 'block');
                    }
                    else {
                        $('#divSOffice').css('display', 'none');
                        $('#div_vivegeneratedet').css('display', 'none');
                        divViewVoucherClick();
                        BtnGenerateClick();
                        $('#divViewVoucher').css('display', 'block');
                    }
                }
                else {
                    $('#divRaiseVoucher').css('display', 'none');
                    $('#divViewVoucher').css('display', 'block');
                    $('#divVocherPayable').css('display', 'none');
                    $('#div_raisevoucher').css('display', 'block');
                    $('#divMainAddNewRows').css('display', 'none');
                    var LevelType = '<%=Session["LevelType"] %>';
                    if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                        $('#divSOffice').css('display', 'block');
                        Fillso();
                        $('#div_vivegeneratedet').css('display', 'block');
                        $('#divViewVoucher').css('display', 'block');
                    }
                    else {
                        $('#divSOffice').css('display', 'none');
                        $('#div_vivegeneratedet').css('display', 'none');
                        divViewVoucherClick();
                        BtnGenerateClick();
                        $('#divViewVoucher').css('display', 'block');
                    }
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function BtnGenerateClick() {
            var fromdate = document.getElementById("txtFromDate").value;
            var ToDate = document.getElementById("txtToDate").value;
            var ddlType = document.getElementById("ddlType").value;
            if (fromdate == "") {
                alert("Select From Date");
                return false;
            }
            if (ToDate == "") {
                alert("Select To Date");
                return false;
            }
            var branchID = "0";
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                branchID = document.getElementById("ddlSo").value;
            }
            else {
            }
            var data = { 'operation': 'btnViewVoucherGeneretaeClick', 'fromdate': fromdate, 'ToDate': ToDate, 'Type': ddlType, 'BranchID': branchID };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        BindViewVouchers(msg);
                    }
                    else {
                        msg = 0;
                        BindViewVouchers(msg);
                    }
                }
                else {
                    msg = 0;
                    BindViewVouchers(msg);
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        //        function displayButtons(cellvalue, options, rowObject) {
        //            var edit = "<input  type='button' value='Edit ' onclick=\"calledit('" + options.rowId + "');\"  />",
        //             save = "<input  type='button' value='  Cancel' onclick=\"callCancelVoucher('" + options.rowId + "');\"  />",
        //             Print = "<input  type='button' value='  Print' onclick=\"callPrintVoucher('" + options.rowId + "');\"  />";
        //            return edit + save + Print;
        //        }
        function geteditdetails(thisid) {
            
            var VoucherID = $(thisid).parent().parent().children('.1').html();
            var VoucherType = $(thisid).parent().parent().children('.2').html();
            var Amount = $(thisid).parent().parent().children('.3').html();
            var ApproveEmpName = $(thisid).parent().parent().children('.4').html();
            var ApprovalAmount = $(thisid).parent().parent().children('.5').html();
            var Status = $(thisid).parent().parent().children('.6').html();
            var Remarks = $(thisid).parent().parent().children('.12').html();
            var ApprovalRemarks = $(thisid).parent().parent().children('.7').html();
            var Approvalby = $(thisid).parent().parent().children('.10').html();
            var BranchID = $(thisid).parent().parent().children('.8').html();
            var CashTo = $(thisid).parent().parent().children('.9').html();
            if (Status == "Raised") {
                if (CashTo == "Employee Expenses") {
                    $('.divEmp').css('display', 'table-row');
                }
                else {
                    $('.divEmp').css('display', 'none');
                }
                document.getElementById('spnVoucherID').innerHTML = VoucherID;
                document.getElementById('ddlVoucherType').value = VoucherType;
                if (VoucherType == "Debit") {
                    $('.divAprovalEmp').css('display', 'table-row');
                }
                if (VoucherType == "Credit") {
                    $('.divAprovalEmp').css('display', 'none');
                }
                if (VoucherType == "Due") {
                    $('.divAprovalEmp').css('display', 'table-row');
                }
                document.getElementById('txtdesc').value = CashTo;
                document.getElementById('txtAMount').innerHTML = Amount;
                document.getElementById('txtRemarks').value = Remarks;
                document.getElementById('ddlEmpApprove').value = Approvalby;

                document.getElementById('btnSave').value = "Edit Voucher";
                ddlVoucherTypeChange();
                $('#divRaiseVoucher').css('display', 'block');
                $('#divViewVoucher').css('display', 'none');
                $('#divVocherPayable').css('display', 'none');
                $('#div_raisevoucher').css('display', 'none');
                var data = { 'operation': 'GetSubPaybleValues', 'VoucherID': VoucherID, 'BranchID': BranchID };
                var s = function (msg) {
                    if (msg) {
                        filleditheadaccount(msg);
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
                var altmsg = "Voucher is " + Status;
                alert(altmsg);
                return false;
            }
        }
        function filleditheadaccount(msg) {

            Cashform = [];
            for (var i = 0; i < msg.length; i++) {
                var HeadSno = msg[i].HeadSno;
                var HeadOfAccount = msg[i].HeadOfAccount;
                var Amount = msg[i].Amount;
                Cashform.push({ HeadSno: HeadSno, HeadOfAccount: HeadOfAccount, Amount: Amount });
            }

            $("#divHeadAcount").css("display", "block");
            var results = '<div  style="overflow:auto;"><table id="tableCashFormdetails" class="table table-bordered" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col" style="text-align:center;">Head Of Account</th><th scope="col" style="text-align:center;">Amount</th></tr></thead></tbody>';
            for (var i = 0; i < Cashform.length; i++) {
                results += '<tr>';
                results += '<td scope="row" class="1" style="text-align:center;"><span id="txtAccount" class= "AccountClass" ><b style="font-weight: 400; ">' + Cashform[i].HeadOfAccount + '</b></span></td>';
                results += '<td class="2" style="text-align:center;"><span id="txtamount" class= "AmountClass" ><b style="font-weight: 400; ">' + Cashform[i].Amount + '</b></span></td>';
                results += '<td style="display:none" class="7"><input type="hidden" id="hdnHeadSno" value="' + Cashform[i].HeadSno + '" /></td>';
                results += '<td  class="6"> <img src="Images/minus_PNG64.png" onclick="RowDeletingClick(this);" style="cursor:pointer;" width="30px" height="30px" alt="Edit" title="Click here to remove Head Of Account."/> </td></tr>';
            }
            results += '</table></div>';
            $("#divHeadAcount").html(results);
        }
        function printvoucher(thisid) {
            var VoucherID = $(thisid).parent().parent().children('.1').html();
            var BranchID = $(thisid).parent().parent().children('.8').html();
            var data = { 'operation': 'Get_Voucher_Print_Details', 'VoucherID': VoucherID, 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    fillprintdetails(msg)
                }
                else {
                    alert(msg);
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var amount; var VoucherId; var branchid;
        function fillprintdetails(msg) {
            $('#printvoucher').css('display', 'block');
            $('#divVoucherGrid').css('display', 'none');
            document.getElementById('lblVoucherType').innerHTML = msg[0].vouchertype;
            document.getElementById('lblVoucherno').innerHTML = msg[0].voucherid;
            document.getElementById('lblDate').innerHTML = msg[0].date;
            document.getElementById('lblPayCash').innerHTML = msg[0].nameof;
            document.getElementById('lblRemarks').innerHTML = msg[0].Remarks;
            document.getElementById('lblTitle').innerHTML = msg[0].Title;
            document.getElementById('lblApprove').innerHTML = msg[0].approveemp;
            document.getElementById('lblReceived').innerHTML = msg[0].lblReceived;

            amount = msg[0].amount;
            branchid = msg[0].branchid;
            VoucherId = msg[0].VoucherID;
            PopupOpen(VoucherId);
        }
        function PopupOpen(VoucherId) {
            VoucherId;
            branchid;
            var data = { 'operation': 'GetSubPaybleValues', 'VoucherID': VoucherId, 'BranchID': branchid };
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
                    alert(msg);
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BtnclearClick_pr() {
            $('#divRaiseVoucher').css('display', 'none');
            $('#divViewVoucher').css('display', 'block');
            $('#divVocherPayable').css('display', 'none');
            $('#div_raisevoucher').css('display', 'block');
            $('#divMainAddNewRows').css('display', 'none');
            $('#printvoucher').css('display', 'none');
            $("#divHead").html("");
            BtnGenerateClick();
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                $('#divSOffice').css('display', 'block');
                Fillso();
                $('#div_vivegeneratedet').css('display', 'block');
                $('#divViewVoucher').css('display', 'block');
            }
            else {
                $('#divSOffice').css('display', 'none');
                $('#div_vivegeneratedet').css('display', 'none');
                divViewVoucherClick();
                BtnGenerateClick();
                $('#divViewVoucher').css('display', 'block');
            }
        }
        function cancelvoucher(thisid) {
            var VoucherID = $(thisid).parent().parent().children('.1').html();
            var Status = $(thisid).parent().parent().children('.6').html();

            if (Status == "Raised") {
                if (!confirm("Do you really want Save")) {
                    return false;
                }
                var data = { 'operation': 'btnVoucherCancelClick', 'VoucherID': VoucherID };
                var s = function (msg) {
                    if (msg) {
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                callHandler(data, s, e);
            } else {
                var altmsg = "Voucher is " + Status;
                alert(altmsg);
                return false;
            }
        }
        //        function callCancelVoucher(rowid) {
        //            var VoucherID = $('#grd_brchtypemangement').getCell(rowid, 'VoucherID');
        //            var Status = $('#grd_brchtypemangement').getCell(rowid, 'Status');
        //            if (Status == "Raised") {
        //                if (!confirm("Do you really want Save")) {
        //                    return false;
        //                }
        //                var data = { 'operation': 'btnVoucherCancelClick', 'VoucherID': VoucherID };
        //                var s = function (msg) {
        //                    if (msg) {
        //                    }
        //                    else {
        //                    }
        //                };
        //                var e = function (x, h, e) {
        //                };
        //                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        //                callHandler(data, s, e);
        //            } else {
        //                var altmsg = "Voucher is " + Status;
        //                alert(altmsg);
        //                return false;
        //            }
        //        }
        var serial = 0;
        function BindViewVouchers(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                //results += '<thead><tr style="background-color: #7ac8f5;color: black;font-size: 13px;text-align:center;"><th scope="col" style="text-align:center;font-weight: 600;">Voucher ID</th><th scope="col" style="text-align:center;font-weight: 600;">Voucher Type</th><th scope="col" style="text-align:center;font-weight: 600;">Cash To</th><th scope="col" style="text-align:center;font-weight: 600;">Amount</th><th scope="col" style="text-align:center;font-weight: 600;">Approve By</th><th scope="col" style="text-align:center;font-weight: 600;">Status</th><th scope="col" style="text-align:center;font-weight: 600;">Approval Remarks</th><th scope="col"></th><th scope="col"></th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr>';
                    results += '<td scope="row" class="50" style="text-align:center; display:none;"  onclick="btnViewVouchers(\'' + msg[i].VoucherID + '\')" >' + msg[i].branchvoucherid + '</td>';
                    results += '<td scope="row" class="1"  style="text-align:center;font-size: 13px;width: 51px;" >' + msg[i].VoucherID + '</td>';
                    results += '<td scope="row" class="2"  style="text-align:center;font-size: 13px;width: 60px;" onclick="btnViewVouchers(\'' + msg[i].VoucherID + '\')" >' + msg[i].VoucherType + '</td>';
                    results += '<td scope="row" class="8"  style="display:none;">' + msg[i].BranchID + '</td>';
                    results += '<td style="text-align:center;font-size: 13px;width: 150px;" class="9" onclick="btnViewVouchers(\'' + msg[i].VoucherID + '\')" >' + msg[i].CashTo + '</td>';
                    results += '<td style="text-align:center;font-size: 13px;width: 50px;" class="3" onclick="btnViewVouchers(\'' + msg[i].VoucherID + '\')" >' + msg[i].Amount + '</td>';
                    results += '<td style="text-align:center;font-size: 13px;width: 69px;" class="4" onclick="btnViewVouchers(\'' + msg[i].VoucherID + '\')" >' + msg[i].ApproveEmpName + '</td>';
                    results += '<td style="display:none;"  style="text-align:center;" class="5" >' + msg[i].ApprovalAmount + '</td>';
                    results += '<td style="text-align:center;font-size: 13px;width: 54px;" class="6" onclick="btnViewVouchers(\'' + msg[i].VoucherID + '\')" >' + msg[i].Status + '</td>';
                    results += '<td style="display:none;" style="text-align:center;" class="10">' + msg[i].ApprovedBy + '</td>';
                    results += '<td style="display:none;" style="text-align:center;" class="11">' + msg[i].Empid + '</td>';
                    results += '<td style="text-align:center;font-size: 13px;" class="12" onclick="btnViewVouchers(\'' + msg[i].VoucherID + '\')">' + msg[i].Remarks + '</td>';
                    results += '<td style="display:none;" style="text-align:center;" class="13">' + msg[i].Description + '</td>';
                    results += '<td style="display:none;" style="text-align:center;" class="7" >' + msg[i].ApprovalRemarks + '</td>';
                    //results += '<td ><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="geteditdetails(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>';
                    //results += '<td ><button type="button" title="Click Here To Print!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 prntcls"  onclick="printvoucher(this)"><span class="glyphicon glyphicon-print" style="top: 0px !important;"></span></button></td>';
                    //results += '<td ><button type="button" title="Click Here To Pay!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 prntcls"  onclick="payvoucher(this)"><span class="glyphicon glyphicon-usd" style="top: 0px !important;"></span></button></td>';
                    results += '<td ><button type="button" title="Click Here To Approve!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="btn_approve_voucher_grid(this)"><span class="glyphicon glyphicon-ok-circle" style="top: 0px !important;"></span></button></td>';
                    results += '<td ><button type="button" title="Click Here To Cancel!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 removeclass"   onclick="cancelvoucher(this)"><span class="glyphicon glyphicon-remove-circle" style="top: 0px !important;"></span></button></td>';
                    results += '</tr>';
                }
            }
            else {
                //results += '<thead><tr style="background-color: #7ac8f5;color: black;font-size: 13px;"><th scope="col" style="text-align:center;font-weight: 600;">Voucher ID</th><th scope="col" style="text-align:center;font-weight: 600;">Voucher Type</th><th scope="col" style="text-align:center;font-weight: 600;">Cash To</th><th scope="col" style="text-align:center;font-weight: 600;">Amount</th><th scope="col" style="text-align:center;font-weight: 600;">Approve By</th><th scope="col" style="text-align:center;font-weight: 600;">Status</th><th scope="col" style="text-align:center;font-weight: 600;">Approval Remarks</th><th scope="col"></th><th scope="col"></th><th scope="col"></th><th scope="col"></th></tr></thead></tbody>';
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr>';
                    results += '<td scope="row" class="50" style="text-align:center; display:none;"  >' + msg[i].branchvoucherid + '</td>';
                    results += '<td scope="row" class="1" style="text-align:center;font-size: 13px;width: 51px;"  >' + msg[i].VoucherID + '</td>';
                    results += '<td scope="row" class="2"  style="text-align:center;font-size: 13px;width: 60px;">' + msg[i].VoucherType + '</td>';
                    results += '<td scope="row" class="8"  style="display:none;">' + msg[i].BranchID + '</td>';
                    results += '<td data-title="brandstatus"  style="text-align:center;font-size: 13px;width: 150px;" class="9" >' + msg[i].CashTo + '</td>';
                    results += '<td data-title="brandstatus"  style="text-align:center;font-size: 13px;width: 50px;" class="3" >' + msg[i].Amount + '</td>';
                    results += '<td data-title="brandstatus"  style="text-align:center;font-size: 13px;width: 69px;" class="4">' + msg[i].ApproveEmpName + '</td>';
                    results += '<td data-title="brandstatus"  style="display:none;"  style="text-align:center;" class="5" >' + msg[i].ApprovalAmount + '</td>';
                    results += '<td data-title="brandstatus"  style="text-align:center;font-size: 13px;width: 54px;" class="6"  >' + msg[i].Status + '</td>';
                    results += '<td data-title="brandstatus"  style="display:none;" style="text-align:center;" class="10">' + msg[i].ApprovedBy + '</td>';
                    results += '<td data-title="brandstatus"  style="display:none;" style="text-align:center;" class="11">' + msg[i].Empid + '</td>';
                    results += '<td data-title="brandstatus"  style="text-align:center;font-size: 13px;" class="12">' + msg[i].Remarks + '</td>';
                    results += '<td data-title="brandstatus"  style="display:none;" style="text-align:center;" class="13">' + msg[i].Description + '</td>';
                    results += '<td data-title="brandstatus"  style="display:none;" style="text-align:center;" class="7">' + msg[i].ApprovalRemarks + '</td>';
                    results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="geteditdetails(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>';
                    results += '<td data-title="brandstatus"><button type="button" title="Click Here To Print!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 prntcls"  onclick="printvoucher(this)"><span class="glyphicon glyphicon-print" style="top: 0px !important;"></span></button></td>';
                    results += '<td data-title="brandstatus"><button type="button" title="Click Here To Pay!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 prntcls" style="background-color: #f39c12!important;border-color: #f39c12!important;"  onclick="payvoucher(this)"><span class="glyphicon glyphicon-usd" style="top: 0px !important;"></span></button></td>';
                    results += '<td data-title="brandstatus"><button type="button" title="Click Here To Cancel!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 removeclass"   onclick="cancelvoucher(this)"><span class="glyphicon glyphicon-remove-circle" style="top: 0px !important;"></span></button></td></tr>';
                }
                results += '</table></div>';
            }
            $("#divVoucherGrid_sub").html(results);
        }

        function txtExpAmountChange(txtExpAmount) {
            var ApprovalAmount = document.getElementById("spnVarifyAmount").innerHTML;
            var ExpAmount = txtExpAmount.value;
            var BalAmount = ApprovalAmount - ExpAmount;
            document.getElementById("spnBalAmount").innerHTML = BalAmount;

        }
        function txtReceivedAmountChange(txtReceivedAmount) {
            var ApprovalAmount = document.getElementById("spnVarifyAmount").innerHTML;
            var ExpAmount = document.getElementById("txtExpAmount").value;
            var ReceivedAmount = document.getElementById("txtReceivedAmount").value;
            ExpAmount = parseFloat(ExpAmount).toFixed(2);
            ReceivedAmount = parseFloat(ReceivedAmount).toFixed(2);
            var BalAmount = ExpAmount + ReceivedAmount;
            BalAmount = parseFloat(BalAmount).toFixed(2);
            ApprovalAmount = parseFloat(ApprovalAmount).toFixed(2);
            if (ApprovalAmount == BalAmount) {
                $('.divClearRaiseVoucher').css('display', 'none');
                $('.divDue').css('display', 'none');
            }
            else {

                $('.divClearRaiseVoucher').css('display', 'table-row');
                $('.divDue').css('display', 'table-row');
            }
        }

        function cashamountchange() {
            var VoucherType = document.getElementById("ddlVoucherType").value;
            if (VoucherType == "Debit") {
                var Amount = document.getElementById("txtCashAmount").value;
                var debitvochertype = document.getElementById("slctdebittype").value;
                if (debitvochertype == "Cash") {
                    ////if (Amount > 10000) {
                    ////    alert("Enter the Amount Below Ten Thousend Rupes (OR) Either If You Are Send the money to bank Or branch Please Select Branch OR Bank");
                    ////    document.getElementById("txtCashAmount").value = "";
                    ////    document.getElementById("slctdebittype").focus();
                    ////    return false;

                    ////}
                }
            }
        }



        var Cashform = [];
        function BtnCashToAddClick() {
          
            $("#divHeadAcount").css('display', 'block');
            var VoucherType = document.getElementById("ddlVoucherType").value;
            if (VoucherType == "Select" || VoucherType == "") {
                alert("Select Voucher Type");
                return false;
            }
            if (VoucherType == "SalaryAdvance" || VoucherType == "SalaryPayble") {
                var tstype = document.getElementById("ddltransactiontype").value;
                if (tstype == "Select" || tstype == "") {
                    alert("Please Select Transaction Type");
                    return false;
                }
            }
            var transactiontype = document.getElementById("ddltransactiontype").value;
            if (transactiontype == "Employee") {
                var employeename = document.getElementById("txtHiddenEName").value;
                var payto = document.getElementById("txtdesc").value;
                if (payto != employeename) {
                    alert("'" + payto + "'" + "        Employee Not Avilable");
                    return false;
                }
            }
            else {
                var othersname = document.getElementById("txtHiddenOName").value;
                var payto = document.getElementById("txtdesc").value;
                //                if (payto != othersname) {
                //                    // alert(payto  +"        Pay Not Avilable");
                //                    alert("'" + payto + "'" + "     Pay To Not Avilable");
                //                    return false;
                //                }
                //                else
                if (payto == "") {
                    alert("Please Fill The PayTo");
                    return false;
                }
            }
            var salaryamount = document.getElementById("spnsalary").innerHTML;
            if (VoucherType == "SalaryAdvance") {
                if (transactiontype == "Employee") {
                    if (salaryamount != "" || salaryamount != "0") {
                        alert("You Dont Have Permissions For Raising Salary Advance. Please Select VoucherType Salary Paybles");
                        return false;
                    }
                }
                else {
                    alert("You Dont Have Permissions For Raising Salary Advance. Please Select VoucherType  Debit OR Credit OR Due");
                    return false;
                }
            }
            var HeadSno = document.getElementById("hidden_headid").value;
            var HeadOfAccount = document.getElementById("combobox").value; ;
//            var Head = document.getElementById("combobox");
//            var HeadSno = Head.options[Head.selectedIndex].value;
//            var HeadOfAccount = Head.options[Head.selectedIndex].text;
            if (HeadOfAccount == "select" || HeadOfAccount == "") {
                alert("Select Account Name");
                return false;
            }
            var Checkexist = false;
            $('.AccountClass').each(function (i, obj) {
                var IName = $(this).text();
                if (IName == HeadOfAccount) {
                    alert("Account Already Added");
                    Checkexist = true;
                }
            });
            if (Checkexist == true) {
                return;
            }
            var Amount = document.getElementById("txtCashAmount").value;
            if (Amount == "") {
                alert("Enter Amount");
                return false;
            }
            else {
                if (VoucherType == "Credit") {
                    Cashform.push({ HeadSno: HeadSno, HeadOfAccount: HeadOfAccount, Amount: Amount });
                    //                }
                    var results = '<div  style="overflow:auto;"><table id="tableCashFormdetails" class="table table-bordered" role="grid" aria-describedby="example2_info">';
                    results += '<thead><tr><th scope="col" style="text-align: center;">Head Of Account</th><th scope="col" style="text-align: center;">Amount</th></tr></thead></tbody>';
                    for (var i = 0; i < Cashform.length; i++) {
                        results += '<tr>';
                        results += '<td scope="row" class="1" style="text-align:center;"><span id="txtAccount" class="AccountClass" ><b style="font-weight: 400;">' + Cashform[i].HeadOfAccount + '</b></span></td>';
                        results += '<td class="2" style="text-align:center;"><span id="txtamount" class="AmountClass" ><b style="font-weight: 400; ">' + Cashform[i].Amount + '</b></span><img src="Images/minus_PNG64.png" onclick="RowDeletingClick(this);" style="cursor:pointer;" width="30px" height="30px" alt="Edit" title="Click here to remove Head Of Account."/></td>';
                        results += '<td style="display:none" class="7"><input type="hidden" id="hdnHeadSno" value="' + Cashform[i].HeadSno + '" /></td></tr>';
                    }
                    results += '</table></div>';
                    $("#divHeadAcount").html(results);
                    var TotRate = 0.0;
                    $('.AmountClass').each(function (i, obj) {
                        if ($(this).text() == "") {
                        }
                        else {
                            TotRate += parseFloat($(this).text());
                        }
                    });
                    TotRate = parseFloat(TotRate).toFixed(2);
                    document.getElementById("txtAMount").innerHTML = TotRate;
                    document.getElementById("txtCashAmount").value = "";

                    var date = new Date();
                    var day = date.getDate();
                    var month = date.getMonth() + 1;
                    var year = date.getFullYear();
                    if (month < 10) month = "0" + month;
                    if (day < 10) day = "0" + day;
                    today = day + "-" + month + "-" + year;
                    var ddlVoucherType = document.getElementById('ddlVoucherType').value;
                    var txtdesc = document.getElementById('txtdesc').value;
                    if (ddlVoucherType == "Debit") {
                        var text = "Being the cash paid to " + txtdesc + ".  Purpose of      Bill No -    Date : " + today + "  Total Amount " + TotRate + "";
                        document.getElementById('txtRemarks').value = text;
                    }
                    else if (ddlVoucherType == "Credit") {
                        var text = "IOU recovery " + txtdesc + " Amount  " + TotRate + "";
                        document.getElementById('txtRemarks').value = text;
                    }
                    else if (ddlVoucherType == "Due") {
                        var text = "Being the IOU  given from " + txtdesc + " purpose of " + today + "";
                        document.getElementById('txtRemarks').value = text;
                    }
                    else {
                        var text = "";
                        document.getElementById('txtRemarks').value = text;
                    }
                }
                else {
                    Amount = parseFloat(Amount);

                    

                    var dlCash = document.getElementById("hidden_headid").value;
                    var data = { 'operation': 'GetHeadLimit', 'HeadSno': dlCash };
                    var s = function (msg) {
                        if (msg) {
                            HeadLimit = msg;
                            HeadLimit = parseFloat(HeadLimit);
                            if (Amount >= HeadLimit) {
                                alert("Please Enter Amount Less Than limit");
                                return false;
                            }
                            Cashform.push({ HeadSno: HeadSno, HeadOfAccount: HeadOfAccount, Amount: Amount });
                            //                }
                            var results = '<div  style="overflow:auto;"><table id="tableCashFormdetails" class="table table-bordered" role="grid" aria-describedby="example2_info">';
                            results += '<thead><tr><th scope="col" style="text-align:center;">Head Of Account</th><th scope="col" style="text-align:center;">Amount</th></tr></thead></tbody>';
                            for (var i = 0; i < Cashform.length; i++) {
                                results += '<tr>';
                                results += '<td scope="row" class="1" style="text-align:center;"><span id="txtAccount"  class="AccountClass"><b style="font-weight: 400; ">' + Cashform[i].HeadOfAccount + '</b></span></td>';
                                results += '<td class="2" style="text-align:center;"><span id="txtamount"  class="AmountClass"><b style="font-weight: 400; ">' + Cashform[i].Amount + '</b></span><img src="Images/minus_PNG64.png" onclick="RowDeletingClick(this);" style="cursor:pointer;" width="30px" height="30px" alt="Edit" title="Click here to remove Head Of Account."/></td>';
                                results += '<td style="display:none" class="7"><input type="hidden" id="hdnHeadSno" value="' + Cashform[i].HeadSno + '" /></td>';
                                results += '<td style="display:none" class="6"><input type="hidden" id="hdnsubSno" value="' + Cashform[i].subsno + '"/></td></tr>';
                            }
                            results += '</table></div>';
                            $("#divHeadAcount").html(results);
                            var TotRate = 0.0;
                            $('.AmountClass').each(function (i, obj) {
                                if ($(this).text() == "") {
                                }
                                else {
                                    TotRate += parseFloat($(this).text());
                                }
                            });
                            TotRate = parseFloat(TotRate).toFixed(2);
                            document.getElementById("txtAMount").innerHTML = TotRate;
                            document.getElementById("txtCashAmount").value = "";
                            if (VoucherType == "SalaryAdvance" || VoucherType == "SalaryPayble") {
                                if (TotRate > parseFloat(salaryamount)) {
                                    alert("Please Enter Amount Less Than Salary limit");
                                    $("#divHeadAcount").html("");
                                    Cashform = [];
                                    document.getElementById("txtAMount").innerHTML = "";
                                    return false;
                                }
                                else if (transactiontype == "Others") {
                                    alert("You Dont Have Permissions For Raising Salary Paybles. Please Select Voucher Type Debit OR Credit OR Due");
                                    $("#divHeadAcount").html("");
                                    Cashform = [];
                                    document.getElementById("txtAMount").innerHTML = "";
                                    return false;
                                }
                            }

                            var date = new Date();
                            var day = date.getDate();
                            var month = date.getMonth() + 1;
                            var year = date.getFullYear();
                            if (month < 10) month = "0" + month;
                            if (day < 10) day = "0" + day;
                            today = day + "-" + month + "-" + year;
                            var ddlVoucherType = document.getElementById('ddlVoucherType').value;
                            var txtdesc = document.getElementById('txtdesc').value;
                            if (ddlVoucherType == "Debit") {
                                var text = "Being the cash paid to " + txtdesc + ".  Purpose of      Bill No -    Date : " + today + "  Total Amount " + TotRate + "";
                                document.getElementById('txtRemarks').value = text;
                            }
                            else if (ddlVoucherType == "Credit") {
                                var text = "IOU recovery " + txtdesc + " Amount  " + TotRate + "";
                                document.getElementById('txtRemarks').value = text;
                            }
                            else if (ddlVoucherType == "Due") {
                                var text = "Being the IOU  given from " + txtdesc + "  purpose of " + today + "";
                                document.getElementById('txtRemarks').value = text;
                            }
                            else {
                                var text = "";
                                document.getElementById('txtRemarks').value = text;
                            }
                        }
                        else {
                        }
                    };
                    var e = function (x, h, e) {
                    };
                    callHandler(data, s, e);
                }
            }
        }
        function BtnClearRaiseVoucherClick() {
            var EmpName = document.getElementById("hdnEmpID").value;
            var ApprovalAmount = document.getElementById("spnApprovalAmount").innerHTML;
            var ExpAmount = document.getElementById("txtExpAmount").value;
            var ReceivedAmount = document.getElementById("txtReceivedAmount").innerHTML;
            var BalAmount = ExpAmount + ReceivedAmount;
            var Amount = ApprovalAmount - BalAmount;

            var VoucherType = document.getElementById("spnVoucherType").innerHTML;
            var CashTo = document.getElementById("spnCashTo").innerHTML;
            var Description = document.getElementById("spnDescription").innerHTML;
            var AprovedBy = document.getElementById("hdnAprovalEmpid").value;

            var data = { 'operation': 'BtnClearRaiseVoucherClick', 'Description': Description, 'Amount': Amount, 'VoucherType': VoucherType, 'CashTo': CashTo, 'Employee': EmpName, 'AprovedBy': AprovedBy };
            var s = function (msg) {
                if (msg) {
                    document.getElementById("spnVoucher").innerHTML = msg;
                    document.getElementById("SpnDue").innerHTML = Amount;
                    $('.ClearRaiseVoucher').css('display', 'none');
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function RowDeletingClick(Account) {
            Cashform = [];
            var HeadOfAccount = "";
            var HeadSno = "";
            var Account = $(Account).closest("tr").find("#txtAccount").text();
            var Amount = "";
            var rows = $("#tableCashFormdetails tr:gt(0)");
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtamount').text() != "") {
                    HeadOfAccount = $(this).find('#txtAccount').text();
                    HeadSno = $(this).find('#HeadSno').val();
                    Amount = $(this).find('#txtamount').text();
                    if (Account == HeadOfAccount) {
                    }
                    else {
                        Cashform.push({ HeadSno: HeadSno, HeadOfAccount: HeadOfAccount, Amount: Amount });
                    }
                }
            });
            var results = '<div style="overflow:auto;"><table id="tableCashFormdetails" class="table table-bordered" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col" style="text-align:center;">Head Of Account</th><th scope="col" style="text-align:center;">Amount</th><th scope="col"></th></tr></thead></tbody>';
            for (var i = 0; i < Cashform.length; i++) {
                results += '<tr>';
                results += '<td scope="row" class="1" style="text-align:center;"><span id="txtAccount"  class="AccountClass"><b style="font-weight: 400; ">' + Cashform[i].HeadOfAccount + '</b></span></td>';
                results += '<td class="2" style="text-align:center;"><span id="txtamount" class="AmountClass"><b style="font-weight: 400; ">' + Cashform[i].Amount + '</b></span></td>';
                results += '<td style="display:none" class="7"><input type="hidden" id="hdnHeadSno" value="' + Cashform[i].HeadSno + '" /></td>';
                results += '<td  class="6"> <img src="Images/minus_PNG64.png" onclick="RowDeletingClick(this);" style="cursor:pointer;" width="30px" height="30px" alt="Edit" title="Click here to remove Head Of Account."/> </td></tr>';
            }
            results += '</table></div>';
            $("#divHeadAcount").html(results);
            var TotRate = 0.0;
            $('.AmountClass').each(function (i, obj) {
                if ($(this).text() == "") {
                }
                else {
                    TotRate += parseFloat($(this).text());
                }
            });
            TotRate = parseFloat(TotRate).toFixed(2);
            document.getElementById("txtAMount").innerHTML = TotRate;

            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = day + "-" + month + "-" + year;
            var ddlVoucherType = document.getElementById('ddlVoucherType').value;
            var txtdesc = document.getElementById('txtdesc').value;
            if (ddlVoucherType == "Debit") {
                var text = "Being the cash paid to " + txtdesc + ".  Purpose of      Bill No -    Date : " + today + "  Total Amount " + TotRate + "";
                document.getElementById('txtRemarks').value = text;
            }
            else if (ddlVoucherType == "Credit") {
                var text = "IOU recovery " + txtdesc + " Amount  " + TotRate + "";
                document.getElementById('txtRemarks').value = text;
            }
            else if (ddlVoucherType == "Due") {
                var text = "Being the IOU  given from " + txtdesc + " purpose of " + today + "";
                document.getElementById('txtRemarks').value = text;
            }
            else {
                var text = "";
                document.getElementById('txtRemarks').value = text;
            }
        }
        function BtnVarifyVoucherSaveClick() {
            var VoucherID = document.getElementById("txtVarifyVoucherID").value;
            if (VoucherID == "") {
                alert("Enter Voucher ID");
                return false;
            }
            var ReceivedAmount = document.getElementById("txtReceivedAmount").value;
            if (ReceivedAmount == "") {
                alert("Enter Received Amount");
                return false;
            }
            var Due = document.getElementById("SpnDue").innerHTML;
            var data = { 'operation': 'BtnVarifyVoucherSaveClick', 'VoucherID': VoucherID, 'ReceivedAmount': ReceivedAmount, 'Due': Due };
            var s = function (msg) {
                if (msg) {
                    divVarifyVoucherClick();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function ddlVoucherTypeChange() {
            var VoucherType =  document.getElementById("ddlVoucherType").value;
            var LevelType = '<%=Session["LevelType"] %>';
            if (VoucherType == "Debit") {
                if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                }
                else {
                    $('.divAprovalEmp').css('display', 'table-row');
                    $('.transactionclass').css('display', 'table-row');
                }

                $('.divType').css('display', 'none');
                $('#spnsalary').css('display', 'block');
                $('.divdType').css('display', 'block');
            }
            if (VoucherType == "Credit") {
                $('.divAprovalEmp').css('display', 'none');
                $('.divType').css('display', 'table-row');
                $('#spnsalary').css('display', 'none');
                $('.divdType').css('display', 'none');
            }
            if (VoucherType == "Due") {
                if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                }
                else {
                    $('.divAprovalEmp').css('display', 'table-row');
                }
                $('.divType').css('display', 'table-row');
                $('#spnsalary').css('display', 'none');
                $('.divdType').css('display', 'none');
            }
            if (VoucherType == "SalaryAdvance" || VoucherType == "SalaryPayble") {
                $('.transactionclass').css('display', 'table-row');
                $('.divdType').css('display', 'none');
            }
            else {
                if (VoucherType == "Debit") {
                   
                }
                else {
                    $('.transactionclass').css('display', 'none');
                }
            }
            $('#spnsalary').css('display', 'block');
        }

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
        function ddlCashTypeChange(Cash) {
            var CashType = Cash.value;
            var VoucherType = document.getElementById("ddlVoucherType").value;
            if (VoucherType == "Credit") {
                if (CashType == "Cash") {
                    $('.divAprovalEmp').css('display', 'none');
                    $('#divMainAddNewRow').css('display', 'none');
                }
                else {
                    $('.divAprovalEmp').css('display', 'table-row');
                    $('#divMainAddNewRow').css('display', 'block');
                }
            }
            else {
                if (VoucherType == "Due") {
                    if (CashType == "Bills") {
                        alert("Select Cash Or Others");
                        return false;
                    }
                }
                $('.divAprovalEmp').css('display', 'table-row');
            }
        }
        function btnVoucherAddClick() {
            var ddlBillHead = document.getElementById("ddlBillHead").value;
            if (ddlBillHead == "select" || ddlBillHead == "") {
                alert("Select Head Of Account");
                return false;
            }
            $('#divMainAddNewRow').css('display', 'none');
        }
        function OrdersCloseClick() {
            $('#divMainAddNewRow').css('display', 'none');
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
                    $("#txtdesc").autocomplete({
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
            document.getElementById('spnsalary').innerHTML = "";
            $('#spnsalary').css('display', 'none');
            var desc = document.getElementById('txtdesc').value;
            for (var i = 0; i < othersnames.length; i++) {
                if (desc == othersnames[i].name) {
                    // document.getElementById('txtLedgerCode').value = othersnames[i].ledgercode;
                    document.getElementById('txtHiddenOName').value = othersnames[i].name;
                }
            }
        }
        function changeemployeeid() {
            var desc = document.getElementById('txtdesc').value;
            for (var i = 0; i < employeesname.length; i++) {
                if (desc == employeesname[i].ledgername) {
                    document.getElementById('txthiddenempid').value = employeesname[i].empid;
                    document.getElementById('txtHiddenEName').value = employeesname[i].ledgername;
                    get_Employee_Salary_Details();
                }
            }
        }
        var employeesname = [];
     
        function ddltransactiontypechanged() {
            var employeetype = document.getElementById("ddltransactiontype").value;
            if (employeetype == "Employee") {

            }
            else {
                get_Others_Details();
            }
        }
        function get_Employee_Salary_Details() {
            $('#spnsalary').css('display', 'block');
            document.getElementById('spnsalary').innerHTML = "";
            var employeetype = document.getElementById("ddltransactiontype").value;
            if (employeetype == "Employee") {
                var empid = document.getElementById('txthiddenempid').value;
            }
            else {
                var empid = "";
            }

            var data = { 'operation': 'get_Employee_Salary_Details', 'empid': empid };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    if (msg == "Employee Not Avilable") {
                        alert(msg);
                    }
                    else {
                        var msglength = msg.length;
                        document.getElementById('spnsalary').innerHTML = msg[msglength - 1].netpay;
                        //                    document.getElementById('spnsalary').innerHTML = msg[0].netpay;
                        $('#tdhidesalary').css('display', 'block');
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
        //sai
        var VoucherID = "0";
        var branchID = "0";
        var branchvoucherid = "0";
        function btnViewVouchers(VoucherIDs) {
            VoucherID = VoucherIDs;
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                $('.ddlSo').css('display', 'table-row');
                branchID = document.getElementById("ddlSo").value;
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
                    BindViewVoucherss(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindViewVoucherss(msg) {
            $('#divMainAddNewRows').css('display', 'block');
            var emp = [];
            $('#divHeads').setTemplateURL('SubPayable.htm');
            $('#divHeads').processTemplate(emp);
            document.getElementById("spnNames").innerHTML = msg[0].Description;
            document.getElementById("spnVoucherTypes").innerHTML = msg[0].VoucherType;
            document.getElementById("spnAmounts").innerHTML = msg[0].Amount;
            document.getElementById("spnApprovalEmps").innerHTML = msg[0].ApproveEmpName;
            document.getElementById("txtCashierRemarkss").value = msg[0].Remarks;
            document.getElementById("spnVoucherIDs").innerHTML = msg[0].branchVocherID;
            document.getElementById("txtApprovalamts").value = msg[0].ApprovalAmount;
            document.getElementById("txtRemarkss").value = msg[0].ApprovalRemarks;
            document.getElementById("txtApprovalamts").value = msg[0].Amount;
            PopupOpens(VoucherID);
        }
        function PopupOpens(VocherID) {
            var data = { 'operation': 'GetAppriveSubPaybleValues', 'VoucherID': VocherID, 'BranchID': branchID };
            var s = function (msg) {
                if (msg) {
                    $('#divHeads').setTemplateURL('SubPayable.htm');
                    $('#divHeads').processTemplate(msg);
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
        function OrdersCloseClicks() {
            $('#divMainAddNewRows').css('display', 'none');
        }
        function btnApproveVoucherclicks() {
            var Remarks = document.getElementById("txtCashierRemarkss").value;
            var Approvalamt = document.getElementById("txtApprovalamts").value;
            if (Approvalamt == "") {
                alert("Enter Amount");
                return false;
            }
            var branchID = "0";
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                $('.ddlSo').css('display', 'table-row');
                branchID = document.getElementById("ddlSo").value;
            }
            else {
            }
            var AppRemarks = document.getElementById("txtRemarkss").value;
            var Status = "A";
            var data = { 'operation': 'btnApproveVoucherclick', 'VoucherID': VoucherID, 'BranchID': branchID, 'Approvalamt': Approvalamt, 'AppRemarks': Remarks, 'Status': Status, 'Remarks': Remarks };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    $('#divMainAddNewRows').css('display', 'none');
                    document.getElementById("txtCashierRemarkss").value = "";
                    document.getElementById("txtApprovalamts").value = "";
                    document.getElementById("txtRemarkss").value = "";
                    var LevelType = '<%=Session["LevelType"] %>';
                    if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                        BtnGenerateClick();
                    }
                    else {
                        //GetRisedVouchers();
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function btnRejectVoucherclicks() {
            var Remarks = document.getElementById("txtCashierRemarkss").value;
            var Approvalamt = document.getElementById("txtApprovalamts").value;
            if (Approvalamt == "") {
                alert("Enter Amount");
                return false;
            }
            var branchID = "0";
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                $('.ddlSo').css('display', 'table-row');
                branchID = document.getElementById("ddlSo").value;
            }
            else {
            }
            var AppRemarks = document.getElementById("txtRemarks").value;
            var Status = "C";
            var data = { 'operation': 'btnRejectVoucherclick', 'VoucherID': VoucherID, 'BranchID': branchID, 'Approvalamt': Approvalamt, 'Remarks': Remarks, 'Status': Status };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    $('#divMainAddNewRows').css('display', 'none');
                    document.getElementById("txtCashierRemarkss").value = "";
                    document.getElementById("txtApprovalamts").value = "";
                    document.getElementById("txtRemarkss").value = "";
                    var LevelType = '<%=Session["LevelType"] %>';
                    if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                        BtnGenerateClick();
                    }
                    else {
                        //GetRisedVouchers();
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function btnVoucherUpdateClicks() {
            var Remarks = document.getElementById("txtCashierRemarkss").value;
            var branchID = "0";
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                $('.ddlSo').css('display', 'table-row');
                branchID = document.getElementById("ddlSo").value;
            }
            else {
            }
            var data = { 'operation': 'btnVoucherUpdateClick', 'VoucherID': VoucherID, 'branchID': branchID, 'Remarks': Remarks };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    $('#divMainAddNewRows').css('display', 'none');
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function btn_approve_voucher_grid(thisid) {
            var branchID = "0";
            var VoucherID = $(thisid).parent().parent().children('.1').html();
            var Amount = $(thisid).parent().parent().children('.3').html();
            var remarks = $(thisid).parent().parent().children('.12').html();
            var Status = $(thisid).parent().parent().children('.6').html();
            var LevelType = '<%=Session["LevelType"] %>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                $('.ddlSo').css('display', 'table-row');
                branchID = document.getElementById("ddlSo").value;
            }
            else {
            }
            var data = { 'operation': 'btn_approve_voucher_grid', 'VoucherID': VoucherID, 'Amount': Amount, 'remarks': remarks, 'branchID': branchID };
            var s = function (msg) {
                if (msg) {
                    BtnGenerateClick();
                }
                else {
                    BtnGenerateClick();
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
            Voucher Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Voucher Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-body">
                <div style="width: 100%; background-color: #fff">
                    <%--<div style="width: 100%; float: left;">
                        <a id="ancRaise" onclick="divRaiseVoucherClick();" style="width: 20%; font-size: 24px;
                            text-decoration: underline;">Raise Voucher</a>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp <a id="ancView" onclick="divViewVoucherClick();"
                                style="padding-left: 25%; font-size: 24px; text-decoration: underline;">View Vouchers</a>
                    </div>--%>
                  <div id="div_raisevoucher" style="text-align: -webkit-right;" >
                   <table>
                        <tr>
                            <td>
                            </td>
                            <td>
                            <div class="input-group"  style="cursor: pointer;">
                                <div class="input-group-addon">
                                <span  class="glyphicon glyphicon-plus-sign" onclick="divRaiseVoucherClick()"></span> <span onclick="divRaiseVoucherClick()">Raise Voucher</span>
                          </div>
                          </div>
                            </td>
                     </tr>
                     </table>
                </div>
                  <br />
                    <div id="divRaiseVoucher" style="display: none;"> 
                        <%--<table style="padding-left: 450px">
                            <tr>
                                <td>
                                    <h2>
                                        Raise Voucher</h2>
                                </td>
                            </tr>
                        </table>
                        <br />--%>
                        <table align="center">
                            <tr class="divsalesOffice" style="display: none">
                                <td>
                                    Sales Office
                                </td>
                                <td style="height: 40px;">
                                    <select id="ddlsalesOffice" class="form-control" onchange="ddlSalesOfficeChange(this);">
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Voucher Type
                                </td>
                                <td style="height: 40px;">
                                    <select id="ddlVoucherType" class="form-control" onchange="ddlVoucherTypeChange();">
                                        <option>Select</option>
                                        <option>Debit</option>
                                        <option>Credit</option>
                                        <option>Due</option>
                                        <option>SalaryAdvance</option>
                                        <option>SalaryPayble</option>
                                    </select>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td class="divType" style="display: none; height: 40px;">
                                    <select id="ddlCashType" class="form-control" onchange="ddlCashTypeChange(this);">
                                        <option>Select</option>
                                        <option>Cash</option>
                                        <option>Bills</option>
                                        <option>Others</option>
                                    </select>
                                </td>

                                <td class="divdType" style="display: none; height: 40px;">
                                    <select id="slctdebittype" class="form-control">
                                        <option>Cash</option>
                                        <option>Branch</option>
                                        <option>Bank</option>
                                    </select>
                                </td>
                            </tr>
                            <tr class="transactionclass" style="display: none;">
                                <td>
                                    Type Of Transaction
                                </td>
                                <td style="height: 40px;">
                                    <select id="ddltransactiontype" class="form-control" onchange="ddltransactiontypechanged();">
                                        <option>Select</option>
                                        <option>Employee</option>
                                        <option>Others</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Pay To
                                </td>
                                <td style="height: 40px;">
                                    <input type="text" id="txtdesc" class="form-control"    maxlength="45" placeholder="Enter PayTo" />
                                </td>
                                <td style="height: 40px;">
                              <input id="txtHiddenEName" type="hidden" class="form-control" name="HiddenName" />
                               <input id="txtHiddenOName" type="hidden" class="form-control" name="HiddenName" />
                              <input id="txthiddenempid" type="hidden" class="form-control" name="Hiddenempid" />
                               </td>
                               <td></td>
                               <td id="tdhidesalary" style="height: 40px;display:none;">
                                    <span id="spnsalary" class="form-control">
                                    </span>
                                </td>

                            </tr>
                           
                            <tr>
                                <td>
                                    Head Of Account
                                </td>
                               
                                   
                                    <td style="height: 40px;">
                                    <input type="text" id="combobox" class="form-control"    maxlength="45" placeholder="Select Head Of Account" />
                                </td>
                                    <%--<select id="combobox" class="form-control">
                                    </select>--%>
                                    <td>
                                 <input id="hidden_headid" type="hidden" class="form-control" name="hiddenheadid" />
                               </td>
                                <td style="width:6px;">
                                </td>
                                <td>
                                    <input type="number" id="txtCashAmount" class="form-control" placeholder="Enter Amount" onkeyup="cashamountchange(this);"/>
                                </td>
                                <td>
                                    <%--<input type="button" id="Button3" value="Add" onclick="BtnCashToAddClick();" class="btn btn-primary" />--%>
                                    <button type="button" title="Click Here To Add!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 prntcls"   onclick="BtnCashToAddClick();">
                                    <span class="glyphicon glyphicon-plus" style="top: 0px !important;"></span></button>
                                </td>
                            </tr>
                            <tr>
                                <td rowspan="1" colspan="2">
                                    <div id="divHeadAcount">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Amount
                                </td>
                                <td style="height: 40px;">
                                    <span id="txtAMount" class="form-control"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Remarks
                                </td>
                                <td style="height: 40px;">
                                    <textarea rows="5" cols="45" id="txtRemarks" class="form-control" maxlength="2000"
                                        placeholder="Enter Remarks"></textarea>
                                </td>
                            </tr>
                            <tr class="divAprovalEmp" style="display: none">
                                <td>
                                    Requested For Aproval
                                </td>
                                <td style="height: 40px;">
                                    <select id="ddlEmpApprove" class="form-control">
                                    </select>
                                </td>
                            </tr>
                            
                            <tr style="display: none">
                                <td>
                                    Voucher ID
                                </td>
                                <td>
                                    <span id="spnVoucherID" class="Spancontrol"></span>
                                </td>
                            </tr>
                        </table>
                        <table align="center">
                            <tr>
                                <td style="display: block;">
                                </td>
                                <td style="height: 40px;">
                                    <input type="button" id="btnSave" value="Raise" onclick="BtnRaiseVoucherClick();"
                                        class="btn btn-primary" />
                                </td>
                                <td style="width:3%;">
                                </td>
                                <td style="height: 40px;">
                                    <input type="button" id="btnClear" value="Close" onclick="BtnclearClick();"
                                        class="btn btn-danger" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divViewVoucher" style="display:none;">
                        <%--<table style="padding-left: 450px">
                            <tr>
                                <td>
                                    <h2>
                                        View Vocher</h2>
                                </td>
                            </tr>
                        </table>
                        <br />--%>
                        <div id="div_vivegeneratedet" style="display:none;">
                            <table>
                                <tr>
                                    <td id="divSOffice" style="display: none;">
                                        <select id="ddlSo" class="form-control">
                                        </select>
                                    </td>
                                    <td hidden>
                                        From Date
                                    </td>
                                    <td hidden>
                                        <input type="date" id="txtFromDate" class="formDate" />
                                    </td>
                                    <td hidden>
                                        To Date
                                    </td>
                                    <td hidden>
                                        <input type="date" id="txtToDate" class="formDate" />
                                    </td>
                                    <td>
                                        Type
                                    </td>
                                    <td>
                                        <select id="ddlType" class="form-control">
                                            <%--<option>All</option>--%>
                                            <option value="R">Raised</option>
                                            <option value="A">Approved</option>
                                            <option value="C">Rejected</option>
                                            <option value="P">Paid</option>
                                        </select>
                                    </td>
                                    <td  style="width:6px;"></td>
                                    <td> 
                                        <input type="button" id="btnGeneretae" value="Generate" onclick="BtnGenerateClick();"
                                            class="btn btn-primary" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                         <div id="divVoucherGrid">
                            <table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;border: 1px solid #f4f4f4;"
                                id="table1" class="mainText2" border="1" >
                               <tr style="height:42px;background-color:#7ac8f5;font-size: 14px;" class="trbgclrcls">
                                     <th scope="col" style="text-align:center;font-weight: 600;width: 51px;">
                                        Voucher ID
                                    </th>
                                    <th scope="col" style="text-align:center;font-weight: 600;width: 60px;">
                                        Voucher Type
                                    </th>
                                    <th scope="col" style="text-align:center;font-weight: 600;width: 150px;">
                                        Cash To
                                    </th>
                                    <th scope="col" style="text-align:center;font-weight: 600;width: 50px;">
                                         Amount
                                    </th>
                                    <th scope="col" style="text-align:center;font-weight: 600;width: 69px;">
                                        Approve By
                                    </th>
                                    <th scope="col" style="text-align:center;font-weight: 600;width: 54px;">
                                        Status
                                    </th>
                                    <th scope="col" style="text-align:center;font-weight: 600;width: 673px;">
                                        Approval Remarks
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <div style="height:420px;overflow-y:auto;" id="divVoucherGrid_sub">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table>
                            <tr>
                                <td>
                                    <div id="printvoucher" style="display: none;">
                                        <div id="divPrint">
                                            <div>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td rowspan="2">
                                                            <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="55px" height="25px" />
                                                        </td>
                                                        <td colspan="4">
                                                            <h4>
                                                                <span id="lblTitle"></span>
                                                            </h4>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="font-size: 10px; padding-left: 24%; text-decoration: underline;">
                                                            <span id="lblVoucherType"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span style="font-size: 12px; float: right;">Voucher No:</span>
                                                        </td>
                                                        <td>
                                                            <b><span id="lblVoucherno"></span></b>
                                                        </td>
                                                        <td>
                                                            <span style="font-size: 12px; float: right;">Date:</span>
                                                        </td>
                                                        <td>
                                                            <b><span id="lblDate"></span></b>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <b>Pay To </b>
                                                            </td>
                                                            <td style="float: left;">
                                                                <b><span id="lblPayCash"></span></b>
                                                            </td>
                                                        </tr>
                                                        <tr style="background-color: #f4f4f4;">
                                                            <td colspan="2" rowspan="1" style="width: 100%;">
                                                                <div id="divHead">
                                                                </div>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                           <tr>
                                                            <td>
                                                                <b><span class="mylbl">Remarks:</span> </b>
                                                            </td>
                                                            <td style="float: left;">
                                                                <b><span id="lblRemarks"></span></b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <b><span class="mylbl">Received Rs:</span> </b>
                                                            </td>
                                                            <td style="float: left;">
                                                                <b><span id="lblReceived"></span></b>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <br />
                                                <div>
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 20%;">
                                                                <span style="font-size: 12px;">Cashier</span>
                                                            </td>
                                                            <td style="width: 20%;">
                                                                <span style="font-size: 12px;">Verified By</span>
                                                            </td>
                                                            <td style="width: 20%;">
                                                                <span style="font-size: 12px;">Accounts officer</span>
                                                            </td>
                                                            <td style="width: 20%;">
                                                              <span style="font-size: 12px;" ID="lblApprove"></span>
                                                            </td>
                                                            <td style="width: 20%;">
                                                                <span style="font-size: 12px;">Receiver's Signature</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <br />
                                        <div>
                                        <table>
                                        <tr>
                                        <td>
                                        <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="javascript:CallPrint('divPrint');">
                                            <i class="fa fa-print"></i>Print
                                        </button>
                                        </td>
                                        <td style="width:3%;">
                                        </td>
                                        <td>
                                         <input type="button" id="Button7" value="Close" onclick="BtnclearClick_pr();"
                                             class="btn btn-danger" />
                                             </td>
                                        </tr></table></div>
                                        <br />
                                        <asp:Label ID="lblmsg" runat="server" Font-Size="20px" ForeColor="Red" Text=""></asp:Label>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divVocherPayable" style="display: none;">
                        <div align="center">
                            <span style="font-size: 18px; font-weight: bold;"><b>Vocher Payable</b></span>
                        </div>
                        <br />
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 50%;">
                                    <table align="center" >
                                        <tr>
                                            <td>
                                                <label>
                                                    Date:</label>
                                            </td>
                                            <td style="height:40px;">
                                                <input type="date" id="datepicker" placeholder="DD-MM-YYYY" class="form-control" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Voucher ID
                                            </td>
                                            <td style="height:40px;">
                                            <span id="txtVoucherID" class="form-control" />
                                                <%--<input type="text" id="txtVoucherID" class="form-control" placeholder="Enter Voucher ID" />--%>
                                            </td>
                                            <td>
                                                <input type="button" id="btnGet" value="Get" onclick="BtnGetVoucherClick();" class="btn btn-primary"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Voucher Type
                                            </td>
                                            <td style="height:40px;">
                                                <span id="spnVoucherType" class="form-control"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Cash To
                                            </td>
                                            <td style="height:40px;">
                                                <span id="spnCashTo" class="form-control"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Description
                                            </td>
                                            <td style="height:40px;">
                                                <span id="spnDescription" class="form-control"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Employee Name
                                            </td>
                                            <td style="height:40px;">
                                                <span id="spnEmpName" class="form-control"></span>
                                                <input type="hidden" id="hdnEmpID" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Amount
                                            </td>
                                            <td style="height:40px;">
                                                <span id="spnAmount" class="form-control"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Remarks
                                            </td>
                                            <td style="height:40px;">
                                                <span id="spnRemarks" class="SpanRemarks" style="height: 80px; width: 100%;"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Approval Amount
                                            </td>
                                            <td style="height:40px;">
                                                <span id="spnApprovalAmount" class="form-control"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Approval Remarks
                                            </td>
                                            <td style="height:40px;">
                                                <span id="spnApprovalRemarks" class="form-control" style="height: 80px; width: 100%;"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                             
                                                Requested For Aproval
                                            </td>
                                            <td style="height:40px;">
                                                <span id="spnApprovalEmp" class="form-control"></span>
                                                <input type="hidden" id="hdnAprovalEmpid" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 50%;">
                                    <table align="center" >
                                        <tr>
                                            <td>
                                                Status
                                            </td>
                                            <td style="height:40px;">
                                                <span id="spnStatus" class="form-control"></span>
                                            </td>
                                        </tr>
                                        <tr class="divCashierRemarks" style="display: none">
                                            <td>
                                                Remarks
                                            </td>
                                            <td style="height:40px;">
                                                <textarea rows="5" cols="45" id="txtCashierRemarks" class="form-control" maxlength="2000"
                                                    placeholder="Enter Remarks"></textarea>
                                            </td>
                                        </tr>
                                        <tr class="divforce" style="display: none">
                                            <td>
                                                <input type="checkbox" id="chkforce" onchange="chkRequestedchange();" />
                                                Force Approval
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <table cellpadding="0" cellspacing="0" style="width: 100%;" id="tableReportingDetails"
                                                    class="mainText2" border="1">
                                                    <thead>
                                                        <tr>
                                                            <td style="width: 25%; height: 20px; color: #2f3293; font-size: 14px; font-weight: bold;
                                                                text-align: center;">
                                                                Cash
                                                                <br />
                                                            </td>
                                                            <td style="width: 25%; text-align: center; height: 20px; font-size: 14px; font-weight: bold;
                                                                color: #2f3293;">
                                                                Count
                                                                <br />
                                                            </td>
                                                            <td style="width: 10%; text-align: center; height: 20px; font-size: 14px; font-weight: bold;
                                                                color: #2f3293; padding: 0px 0px 0px 2%;">
                                                                Total
                                                                <br />
                                                            </td>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr class="tblrowcolor">
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <span id="Span16" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                                                    <b>2000</b></span>
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <b style="font-size: 11px; font-weight: bold;">X</b>
                                                                <input type="number" id="Number8" onkeyup="CountChange(this);" class="qtyclass" onkeypress="return numberOnlyExample();"
                                                                    style="width: 80%; height: 24px; border: 1px solid gray; border-radius: 6px 6px 6px 6px;"
                                                                    placeholder="Enter Count" />
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <span id="Span17" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                                                    <b>0</b></span>
                                                            </td>
                                                        </tr>
                                                        <tr class="tblrowcolor">
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <span id="txtsno" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                                                    <b>500</b></span>
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <b style="font-size: 11px; font-weight: bold;">X</b>
                                                                <input type="number" id="txtCount" onkeyup="CountChange(this);" class="qtyclass"
                                                                    onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                                                    border-radius: 6px 6px 6px 6px;" placeholder="Enter Count" />
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <span id="Span2" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                                                    <b>0</b></span>
                                                            </td>
                                                        </tr>
                                                         <tr class="tblrowcolor">
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span18" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>200</b></span>
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                        <input type="number" id="Number9" onkeyup="CountChange(this);" class="qtyclass" onkeypress="return numberOnlyExample();"
                                            style="width: 80%; height: 24px; border: 1px solid gray; border-radius: 6px 6px 6px 6px;"
                                            placeholder="Enter Count" />
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span19" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                            <b>0</b></span>
                                    </td>
                                </tr>
                                                        <tr class="tblrowcolor">
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <span id="Span1" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                                                    <b>100</b></span>
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <b style="font-size: 11px; font-weight: bold;">X</b>
                                                                <input type="number" id="Number1" onkeyup="CountChange(this);" class="qtyclass" onkeypress="return numberOnlyExample();"
                                                                    style="width: 80%; height: 24px; border: 1px solid gray; border-radius: 6px 6px 6px 6px;"
                                                                    placeholder="Enter Count" />
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <span id="Span3" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                                                    <b>0</b></span>
                                                            </td>
                                                        </tr>
                                                        <tr class="tblrowcolor">
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <span id="Span4" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                                                    <b>50</b></span>
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <b style="font-size: 11px; font-weight: bold;">X</b>
                                                                <input type="number" id="Number2" onkeyup="CountChange(this);" class="qtyclass" onkeypress="return numberOnlyExample();"
                                                                    style="width: 80%; height: 24px; border: 1px solid gray; border-radius: 6px 6px 6px 6px;"
                                                                    placeholder="Enter Count" />
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <span id="Span5" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                                                    <b>0</b></span>
                                                            </td>
                                                        </tr>
                                                        <tr class="tblrowcolor">
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <span id="Span6" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                                                    <b>20</b></span>
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <b style="font-size: 11px; font-weight: bold;">X</b>
                                                                <input type="number" id="Number3" onkeyup="CountChange(this);" class="qtyclass" onkeypress="return numberOnlyExample();"
                                                                    style="width: 80%; height: 24px; border: 1px solid gray; border-radius: 6px 6px 6px 6px;"
                                                                    placeholder="Enter Count" />
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <span id="Span7" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                                                    <b>0</b></span>
                                                            </td>
                                                        </tr>
                                                        <tr class="tblrowcolor">
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <span id="Span8" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                                                    <b>10</b></span>
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <b style="font-size: 11px; font-weight: bold;">X</b>
                                                                <input type="number" id="Number4" onkeyup="CountChange(this);" class="qtyclass" onkeypress="return numberOnlyExample();"
                                                                    style="width: 80%; height: 24px; border: 1px solid gray; border-radius: 6px 6px 6px 6px;"
                                                                    placeholder="Enter Count" />
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <span id="Span9" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                                                    <b>0</b></span>
                                                            </td>
                                                        </tr>
                                                        <tr class="tblrowcolor">
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <span id="Span10" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                                                    <b>5</b></span>
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <b style="font-size: 11px; font-weight: bold;">X</b>
                                                                <input type="number" id="Number5" onkeyup="CountChange(this);" class="qtyclass" onkeypress="return numberOnlyExample();"
                                                                    style="width: 80%; height: 24px; border: 1px solid gray; border-radius: 6px 6px 6px 6px;"
                                                                    placeholder="Enter Count" />
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <span id="Span11" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                                                    <b>0</b></span>
                                                            </td>
                                                        </tr>
                                                        <tr class="tblrowcolor">
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <span id="Span12" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                                                    <b>2</b></span>
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <b style="font-size: 11px; font-weight: bold;">X</b>
                                                                <input type="number" id="Number6" onkeyup="CountChange(this);" class="qtyclass" onkeypress="return numberOnlyExample();"
                                                                    style="width: 80%; height: 24px; border: 1px solid gray; border-radius: 6px 6px 6px 6px;"
                                                                    placeholder="Enter Count" />
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <span id="Span13" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                                                    <b>0</b></span>
                                                            </td>
                                                        </tr>
                                                        <tr class="tblrowcolor">
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <span id="Span14" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                                                    <b>1</b></span>
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <b style="font-size: 11px; font-weight: bold;">X</b>
                                                                <input type="number" id="Number7" onkeyup="CountChange(this);" class="qtyclass" onkeypress="return numberOnlyExample();"
                                                                    style="width: 80%; height: 24px; border: 1px solid gray; border-radius: 6px 6px 6px 6px;"
                                                                    placeholder="Enter Count" />
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                                <span id="Span15" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                                                    <b>0</b></span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 20%; height: 30px; vertical-align: top; font-size: 12px; font-weight: 500;
                                                                text-align: center; padding: 0px 0px 0px 3px">
                                                            </td>
                                                            <td style="width: 20%; height: 30px; vertical-align: middle; text-align: center;
                                                                color: Gray;">
                                                                <span style="font-size: 16px; color: Blue;">Total:</span>
                                                            </td>
                                                            <td style="width: 20%; height: 30px; font-size: 11px; vertical-align: middle; text-align: center;
                                                                color: Gray;" align="center">
                                                                <span style="font-size: 16px; color: Red; font-weight: bold;" id="txt_Total"></span>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                            </td>
                                            <td class="divpay" style="display: none">
                                                <input type="button" id="btnPay" value="Pay" onclick="BtnPayVoucherClick();" class="btn btn-primary" />
                                            </td>
                                            <td style="width:3%;">
                                            </td>
                                            <td>
                                                <input type="button" id="btn_close" value="Close" onclick="BtnPayVoucher_close();" class="btn btn-danger" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divVarifyVoucher" style="display: none;">
                        <table style="padding-left: 450px">
                            <tr>
                                <td>
                                    <h2>
                                        Varify Vocher</h2>
                                </td>
                            </tr>
                        </table>
                        <table align="center">
                            <tr>
                                <td>
                                    Voucher ID
                                </td>
                                <td style="height:40px;">
                                    <input type="text" id="txtVarifyVoucherID" class="Spancontrol" placeholder="Enter Voucher ID" />
                                </td>
                                <td >
                                    <input type="button" id="Button4" value="Get" onclick="BtnGetVarifyVoucherClick();"
                                        class="btn btn-primary" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Amount
                                </td>
                                <td style="height:40px;">
                                    <span id="spnVarifyAmount" class="Spancontrol"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Remarks
                                </td>
                                <td style="height:40px;">
                                    <span id="sonVarifyRemarks" class="SpanRemarks"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td rowspan="15" colspan="15">
                                    <div class="divclass" id="divClearViucher" style="display: none;">
                                        <table>
                                            <tr>
                                                <td>
                                                    Expenses Amount
                                                </td>
                                                <td style="height:40px;">
                                                    <input type="text" id="txtExpAmount" class="Spancontrol" placeholder="Enter Expenses Amount"
                                                        onchange="txtExpAmountChange(this);" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Balance Amount
                                                </td>
                                                <td style="height:40px;">
                                                    <span id="spnBalAmount" class="Spancontrol"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Received Amount
                                                </td>
                                                <td style="height:40px;">
                                                    <input type="text" id="txtReceivedAmount" class="Spancontrol" placeholder="Enter Received Amount"
                                                        onchange="txtReceivedAmountChange(this);" />
                                                </td>
                                            </tr>
                                            <tr class="divClearRaiseVoucher" style="display: none;">
                                                <td>
                                                    Voucher ID
                                                </td>
                                                <td>
                                                    <span id="spnVoucher" class="Spancontrol"></span>
                                                </td>
                                                <td class="ClearRaiseVoucher" style="height:40px;">
                                                    <input type="button" id="ClearRaiseVoucher" value="Raise Voucher" onclick="BtnClearRaiseVoucherClick();"
                                                        class="btn btn-primary" />
                                                </td>
                                            </tr>
                                            <tr class="divDue" style="display: none;">
                                                <td>
                                                    if Due
                                                </td>
                                                <td style="height:40px;">
                                                    <span id="SpnDue" class="Spancontrol"></span>
                                                    <%--    <input type="text" id="txtDue" class="Spancontrol" placeholder="Enter Received Amount" />--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td style="height:40px;">
                                                    <input type="button" id="Button1" value="Save" onclick="BtnVarifyVoucherSaveClick();"
                                                        class="btn btn-primary" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="divMainAddNewRow" class="pickupclass" style="text-align: center; height: 100%;
                    width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
                    background: rgba(192, 192, 192, 0.7);">
                    <div id="divAddNewRow" style="border: 5px solid #A0A0A0; position: absolute; top: 30%;
                        background-color: White; left: 10%; right: 10%; width: 80%; height: 50%; -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                        -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                        border-radius: 10px 10px 10px 10px;">
                        <table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;"
                            id="tableCollectionDetails" class="mainText2" border="1">
                            <tr>
                                <td>
                                    <label>
                                        Head Of Account</label>
                                </td>
                                <td>
                                    <select id="ddlBillHead" class="form-control">
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <input type="button" value="ADD Voucher" id="btnAdd" onclick="btnVoucherAddClick();"
                                        class="btn btn-primary" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divclose" style="width: 35px; top: 24.5%; right: 9.5%; position: absolute;
                        z-index: 99999; cursor: pointer;">
                        <img src="Images/Odclose.png" alt="close" onclick="OrdersCloseClick();" />
                    </div>
                </div>
            </div>
            <div id="divMainAddNewRows" class="pickupclass" style="text-align: center; height: 100%;
                    width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
                    background: rgba(192, 192, 192, 0.7);">
                    <div id="div2" style="border: 2px solid #A0A0A0; position: absolute; top: 8%;
                    background-color: White; right: 25%; width: 50%;  -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                    -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                    border-radius: 10px 10px 10px 10px; padding:2%">
                        <table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;"
                            id="tableCollectionDetailss" class="mainText2" border="1">
                            <tr>
                                <td>
                                  <label>  Name</label>
                                </td>
                                <td style="height:40px;">
                                    <span id="spnNames" style="font-size:20px;font-weight:900;color:#00a65a;"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>  Voucher ID</label>
                                </td>
                                <td style="height:40px;">
                                    <span id="spnVoucherIDs" class="form-control"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   <label>    VoucherType</label>
                                </td>
                                <td style="height:40px;">
                                    <span id="spnVoucherTypes" style="font-size:20px;font-weight:900;color:#00a65a;"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="divHeads">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>   Amount</label>
                                </td>
                                <td style="height:40px;">
                                    <span id="spnAmounts" style="font-size:20px;font-weight:900;color:#3c8dbc;"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   <label>    Approval By</label>
                                </td>
                                <td style="height:40px;">
                                    <span id="spnApprovalEmps" style="font-size:20px;font-weight:900;color:#00a65a;"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   <label>    Remarks</label>
                                </td>
                                <td style="height:40px;">
                                    <textarea rows="3" cols="45" id="txtCashierRemarkss" class="form-control" placeholder="Enter Remarks"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>   Approval Amount </label>
                                </td>
                                <td style="height:40px;">
                                    <input type="number" id="txtApprovalamts" class="form-control" value="" onkeypress="return numberOnlyExample();"
                                        placeholder="Enter Approval Amount" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>   Approval Remarks</label>
                                </td>
                                <td style="height:40px;">
                                    <input type="text" id="txtRemarkss" class="form-control" value="" onkeypress="return numberOnlyExample();"
                                        placeholder="Enter Remarks" />
                                </td>
                            </tr>
                        </table>
                        <table align="center" style="height: 40px;">
                            <tr>
                                <td style="height:40px;">
                                    <input type="button" value="Update" id="Button2" onclick="btnVoucherUpdateClicks();"
                                         class="btn btn-primary" />
                                </td>
                                <td style="width:3%;">
                                </td>
                                <td style="height:40px;">
                                    <input type="button" id="Button5" value="Approve" onclick="btnApproveVoucherclicks();"
                                         class="btn btn-success" />
                                </td>
                                <td style="width:3%;">
                                </td>
                                <td style="height:40px;">
                                    <input type="button" id="Button6" value="Reject" onclick="btnRejectVoucherclicks();"
                                         class="btn btn-danger" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div4" style="width: 35px; top: 7.5%; right: 24%; position: absolute;
                    z-index: 99999; cursor: pointer;">
                        <img src="Images/close1.png" alt="close" onclick="OrdersCloseClicks();" style="width: 30px;height: 25px;"/>
                    </div>
                </div>
        </div>
    </section>
</asp:Content>
