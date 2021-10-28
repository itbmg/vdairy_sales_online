<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VoucherPrint.aspx.cs" Inherits="VoucherPrint" %>


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
    <link href="autocomplete/jquery-ui.css?v=3002" rel="stylesheet" type="text/css" />
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
        var LevelType = "";
        $(function () {
           // FillApprovalEmploye();
            //FillHeads();
            $("#datepicker").datepicker({ dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
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

            $('#txtFromDate').val(today);
            $('#txtToDate').val(today);
           
                divViewVoucherClick();
                BtnGenerateClick();
           
        });
//        
//        function FillSalesOffice() {
//            var data = { 'operation': 'GetSalesOffice' };
//            var s = function (msg) {
//                if (msg) {
//                    if (msg == "Session Expired") {
//                        alert(msg);
//                        window.location = "Login.aspx";
//                    }
//                    BindSalesOffice(msg);
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//            callHandler(data, s, e);
//        }
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
//        function BindSalesOffice(msg) {
//            var ddlsalesOffice = document.getElementById('ddlsalesOffice');
//            var length = ddlsalesOffice.options.length;
//            ddlsalesOffice.options.length = null;
//            var opt = document.createElement('option');
//            opt.innerHTML = "select";
//            ddlsalesOffice.appendChild(opt);
//            for (var i = 0; i < msg.length; i++) {
//                if (msg[i].BranchName != null) {
//                    var opt = document.createElement('option');
//                    opt.innerHTML = msg[i].BranchName;
//                    opt.value = msg[i].Sno;
//                    ddlsalesOffice.appendChild(opt);
//                }
//            }
//        }
//        
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
            var dlCash = document.getElementById("combobox").value;
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
       
        function BtnclearClick() {
            $('#divRaiseVoucher').css('display', 'none');
            $('#divViewVoucher').css('display', 'block');
            $('#divVocherPayable').css('display', 'none');
//            $('#div_raisevoucher').css('display', 'block');
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
//                $('#div_vivegeneratedet').css('display', 'none');
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
            $("#divHeadAcount").html("");
            document.getElementById("combobox").selectedIndex = ""
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
       
      

        function BtnGenerateClick() {
            var fromdate = document.getElementById("txtFromDate").value;
            var ToDate = document.getElementById("txtToDate").value;
           // var ddlType = document.getElementById("ddlType").value;
            if (fromdate == "") {
                alert("Select From Date");
                return false;
            }
            if (ToDate == "") {
                alert("Select To Date");
                return false;
            }
           // var branchID = "0";

            branchID = document.getElementById("ddlSo").value;
         
            var FormType = "VoucherPrint";
            var data = { 'operation': 'btnViewVoucherGeneretaeClick', 'fromdate': fromdate, 'ToDate': ToDate, 'BranchID': branchID, 'FormType': FormType };
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
//            $('#div_raisevoucher').css('display', 'block');
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
//                $('#div_vivegeneratedet').css('display', 'none');
                divViewVoucherClick();
                
                $('#divViewVoucher').css('display', 'block');
            }
            BtnGenerateClick();
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
        
        var serial = 0;
        function BindViewVouchers(msg) {
            var results = '<div  style="overflow:auto;"><table style="width:100%;" class="table table-bordered table-hover dataTable no-footer">';
            results += '<thead><tr style="background-color: #7ac8f5;color: black;font-size: 13px;"><th scope="col" style="text-align:center;font-weight: 600;">Voucher ID</th><th scope="col" style="text-align:center;font-weight: 600;">Ref No</th><th scope="col" style="text-align:center;font-weight: 600;">Voucher Type</th><th scope="col" style="text-align:center;font-weight: 600;">Cash To</th><th scope="col" style="text-align:center;font-weight: 600;">Amount</th><th scope="col" style="text-align:center;font-weight: 600;">Approve By</th><th scope="col" style="text-align:center;font-weight: 600;">Status</th><th scope="col" style="text-align:center;font-weight: 600;">Approval Remarks</th><th></th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                results += '<td scope="row" class="50"  style="text-align:center;" >' + msg[i].branchvoucherid + '</td>';
                results += '<td scope="row" class="1"  style="text-align:center;" >' + msg[i].VoucherID + '</td>';
                results += '<td scope="row" class="2"  style="text-align:center;">' + msg[i].VoucherType + '</td>';
                results += '<td scope="row" class="8"  style="display:none;">' + msg[i].BranchID + '</td>';
                results += '<td data-title="brandstatus"  style="text-align:center;" class="9" >' + msg[i].CashTo + '</td>';
                results += '<td data-title="brandstatus"  style="text-align:center;" class="3" >' + msg[i].Amount + '</td>';
                results += '<td data-title="brandstatus"  style="text-align:center;" class="4">' + msg[i].ApproveEmpName + '</td>';
                results += '<td data-title="brandstatus"  style="display:none;"  style="text-align:center;" class="5" >' + msg[i].ApprovalAmount + '</td>';
                results += '<td data-title="brandstatus"  style="text-align:center;" class="6"  >' + msg[i].Status + '</td>';
                results += '<td data-title="brandstatus"  style="display:none;" style="text-align:center;" class="10">' + msg[i].ApprovedBy + '</td>';
                results += '<td data-title="brandstatus"  style="display:none;" style="text-align:center;" class="11">' + msg[i].Empid + '</td>';
                results += '<td data-title="brandstatus"  style="text-align:center;" class="12">' + msg[i].Remarks + '</td>';
                results += '<td data-title="brandstatus"  style="display:none;" style="text-align:center;" class="13">' + msg[i].Description + '</td>';
                results += '<td data-title="brandstatus"  style="display:none;" style="text-align:center;" class="7">' + msg[i].ApprovalRemarks + '</td>';
                //                    results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="geteditdetails(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Print!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 prntcls"  onclick="printvoucher(this)"><span class="glyphicon glyphicon-print" style="top: 0px !important;"></span></button></td>';
                //                    results += '<td data-title="brandstatus"><button type="button" title="Click Here To Pay!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 prntcls" style="background-color: #f39c12!important;border-color: #f39c12!important;"  onclick="payvoucher(this)"><span class="glyphicon glyphicon-usd" style="top: 0px !important;"></span></button></td>';
                //                    results += '<td data-title="brandstatus"><button type="button" title="Click Here To Cancel!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 removeclass"   onclick="cancelvoucher(this)"><span class="glyphicon glyphicon-remove-circle" style="top: 0px !important;"></span></button></td></tr>';
            }
            results += '</table></div>';
            $("#divVoucherGrid").html(results);
        }
    
        
        //sai
        
        function OrdersCloseClicks() {
            $('#divMainAddNewRows').css('display', 'none');
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
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Cash Voucher 
                </h3>
            </div>
            <div class="box-body">
                <div style="width: 100%; background-color: #fff">
                  
                  <br />
                    
                    <div id="divViewVoucher">
                       
                        <div id="div_vivegeneratedet">
                            <table>
                                <tr>
                                    <td id="divSOffice" style="display: none;">
                                        <select id="ddlSo" class="form-control">
                                        </select>
                                    </td>
                                    <td>
                                        From Date
                                    </td>
                                    <td>
                                        <input type="date" id="txtFromDate" class="formDate" />
                                    </td>
                                    <td>
                                        To Date
                                    </td>
                                    <td>
                                        <input type="date" id="txtToDate" class="formDate" />
                                    </td>
                                    <%--<td>
                                        Type
                                    </td>
                                    <td>
                                        <select id="ddlType" class="form-control">
                                            
                                            <option value="R">Raised</option>
                                            <option value="A">Approved</option>
                                            <option value="C">Rejected</option>
                                            <option value="P">Paid</option>
                                        </select>
                                    </td>--%>
                                    <td  style="width:6px;"></td>
                                    <td> 
                                        <input type="button" id="btnGeneretae" value="Generate" onclick="BtnGenerateClick();"
                                            class="btn btn-primary" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                         <div id="divVoucherGrid" style="height: 450px; overflow: auto; font-size: 12px;">
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
                </div>
            </div>
        </div>
    </section>
</asp:Content>

