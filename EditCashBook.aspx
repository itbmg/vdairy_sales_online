<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EditCashBook.aspx.cs" Inherits="EditCashBook" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <style type="text/css">
        .ddlsize
        {
            width: 230px;
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
            FillSalesOffice();
            $("#datepicker").datepicker({ dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
                onSelect: function (selectedDate) {
                    GetEditCashBookValues();
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
       
        
        function AmountChange(Amount) {
            var TotalCash = 0;
            var Total = 0;
            var Returnqty = 0;

            $('.TotClass').each(function (i, obj) {
                Returnqty += parseFloat($(this).val());
            });
            document.getElementById('txtAmount').innerHTML = Returnqty;
        }

        function CountChange(count) {
            var TotalCash = 0;
            var Total = 0;
            if (count.value == "") {
                $(count).closest("tr").find(".TotalClass").text(Total);
                document.getElementById('txtSubmittedAmount').value = Total;
                return false;
            }
            var Cash = $(count).closest("tr").find(".CashClass").text();
            if (Cash == "Vouchers") {
                Cash = 1;
            }
            Total = parseInt(count.value) * parseInt(Cash);
            $(count).closest("tr").find(".TotalClass").text(Total);
            $('.TotalClass').each(function (i, obj) {
                TotalCash += parseInt($(this).text());
            });
            document.getElementById('txt_Total').innerHTML = TotalCash;
            document.getElementById('txtSubmittedAmount').innerHTML = TotalCash;
        }
        function GetEditCashBookValues() {
            var Returnqty = 0;

            var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
            if (ddlSalesOffice == "Select Route Name" || ddlSalesOffice == "") {
                alert("Please Select Route Name");
                return false;
            }
            var txtDate = document.getElementById('datepicker').value;
            if (txtDate == "") {
                alert("Please Select Date");
                return false;
            }
            var data = { 'operation': 'GetEditCashBookValues', 'SalesOffice': ddlSalesOffice, 'IndDate': txtDate };
            var s = function (msg) {
                if (msg) {
                    $('#divFillScreen').removeTemplate();
                    $('#divFillScreen').setTemplateURL('Reporting8.htm');
                    $('#divFillScreen').processTemplate(msg);
                    if (msg.length > 0) {
                        document.getElementById('hdncashsno').value = msg[0].sno
                        var strdenmn = msg[0].Denominations.split("+");
                        for (i = 0; i < strdenmn.length; i++) {
                            var rowsdenominations = $("#tableReportingDetails tr:gt(0)");
                            var DenominationString = "";
                            $(rowsdenominations).each(function (i, obj) {
                                if (i <= 10) {
                                    var cash = strdenmn[i].split("x");
                                    if ($(this).closest("tr").find(".CashClass").text() == cash[0]) {
                                        Cash = cash[0];
                                        if (Cash != "") {
                                            if (cash[0] == "Vouchers") {
                                                cash[0] = "1";
                                            }
                                            var denamount = 0;
                                            denamount = cash[1];
                                            $(this).closest("tr").find(".qtyclass").val(denamount);
                                            var amount = 0;
                                            amount = parseFloat(cash[0]) * denamount;
                                            $(this).closest("tr").find(".TotalClass").text(amount);
                                        }
                                    }
                                }
                            });
                        }
                    }
                    //$('.TotClass').each(function (i, obj) {
                    //    Returnqty += parseFloat($(this).val());
                    //});


                    CountChange(0);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function btnDenominationSaveClick(id) {
            var txtDate = document.getElementById('datepicker').value;
            if (txtDate == "") {
                alert("Please Select Date");
                return false;
            }
            
            var rowsdenominations = $("#tableReportingDetails tr:gt(0)");
            var DenominationString = "";
            $(rowsdenominations).each(function (i, obj) {
                if ($(this).closest("tr").find(".CashClass").text() == "") {
                }
                else {
                    DenominationString += $(this).closest("tr").find(".CashClass").text() + 'x' + $(this).closest("tr").find(".qtyclass").val() + "+";
                }
            });
            var txtAmount = document.getElementById('txtAmount').value;
            if (txtAmount == "") {
                alert("Please Enter Amount");
                return false;
            }
            var soid = document.getElementById('ddlSalesOffice').value;
            var hdncashsno = document.getElementById('hdncashsno').value;
            var data = { 'operation': 'btnEditCashbookSaveClick', 'indentdate': txtDate, 'Denominations': DenominationString, 'hdncashsno': hdncashsno, 'SubAmount': txtAmount, 'SalesOfficeID': soid };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    $('#divFillScreen').removeTemplate();
                    $('#divFillScreen').setTemplateURL('Reporting8.htm');
                    $('#divFillScreen').processTemplate();
                    document.getElementById('hdncashsno').value = 0;
                    document.getElementById('txtSubmittedAmount').innerHTML = 0;
                    document.getElementById('ddlSalesOffice').value = 0;
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
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
            Edit CashBook<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Edit CashBook</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Edit CashBook Details
                </h3>
            </div>
            <div class="box-body">
                <table align="center">
                    <tr>
                        <td>
                        <label for="lblBranch">
                            Sales Office</label>
                        </td>
                        <td style="height:40px;">
                            <select id="ddlSalesOffice" class="form-control">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap>
                            <label for="lblBranch">
                                CashBook Date</label>
                        </td>
                        <td style="height:40px;wi">
                            <input type="date" id="datepicker" placeholder="DD-MM-YYYY" class="form-control" />
                            <input type="hidden" id="hdncashsno" />
                        </td>
                    </tr>
                    <tr style="height:10px;">
                        <td>
                        </td>
                        <td>
                            <input type="button" id="Button1" value="Get Details" class="btn btn-primary"
                                onclick="GetEditCashBookValues();" />
                        </td>
                    </tr>
                </table>
                <div id="divFillScreen">
                </div>
               

            </div>
        </div>
    </section>
</asp:Content>
