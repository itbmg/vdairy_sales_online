<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EditCollections.aspx.cs" Inherits="EditCollections" %>

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
            //FillRoutes();
            FillSalesOffice();
            $("#datepicker").datepicker({ dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
                onSelect: function (selectedDate) {
                    GetEditIndentValues();
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
        function ddlSalesOfficeChange(ID) {
            var BranchID = ID.value;
            var data = { 'operation': 'GetDespatches', 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    bindRoutes(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function FillRoutes() {
            var BranchID = document.getElementById('ddlSalesOffice').value;
            var data = { 'operation': 'GetDespatches', 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    bindRoutes(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function bindRoutes(msg) {
            var ddlRouteName = document.getElementById('ddlRouteName');
            var length = ddlRouteName.options.length;
            ddlRouteName.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Route Name";
            ddlRouteName.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].routename != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].routename;
                    opt.value = msg[i].routesno;
                    ddlRouteName.appendChild(opt);
                }
            }
        }
        var RouteSno = 0;
        function ddlRouteNameChange(Id) {
            var data = { 'operation': 'GetRouteNameChange', 'RouteID': Id.value };
            var s = function (msg) {
                if (msg) {
                    BindBranchName(msg);
                    $('#divFillScreen').removeTemplate();
                    $('#divFillScreen').setTemplateURL('CollectionsEdit1.htm');
                    $('#divFillScreen').processTemplate();

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function ddlAgentNameChange() {
            GetEditIndentValues();
        }
        function BindBranchName(msg) {
            document.getElementById('ddlBranchName').options.length = "";
            var veh = document.getElementById('ddlBranchName');
            var length = veh.options.length;
            for (i = length - 1; i >= 0; i--) {
                veh.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Agent Name";
            opt.value = "";
            veh.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null || msg[i].BranchName != "" || msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].b_id;
                    veh.appendChild(opt);
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
        function GetEditIndentValues() {
            var Returnqty = 0;

            var ddlRouteName = document.getElementById('ddlRouteName').value;
            if (ddlRouteName == "Select Route Name" || ddlRouteName == "") {
                alert("Please Select Route Name");
                return false;
            }
            //            var BranchName = document.getElementById('ddlBranchName').value;
            //            if (BranchName == "Select Agent Name" || BranchName == "") {
            //                alert("Please Select Agent Name");
            //                return false;
            //            }
            var txtDate = document.getElementById('datepicker').value;
            if (txtDate == "") {
                alert("Please Select Date");
                return false;
            }
            var data = { 'operation': 'GetEditCollectionValuesClick', 'RouteID': ddlRouteName, 'IndDate': txtDate };
            var s = function (msg) {
                if (msg) {
                    $('#divFillScreen').removeTemplate();
                    $('#divFillScreen').setTemplateURL('CollectionsEdit1.htm');
                    $('#divFillScreen').processTemplate(msg);
                    $('.TotClass').each(function (i, obj) {
                        Returnqty += parseFloat($(this).val());
                    });
                    document.getElementById('txtAmount').innerHTML = Returnqty;
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function btnEditCollectionSaveClick(id) {
            var txtDate = document.getElementById('datepicker').value;
            if (txtDate == "") {
                alert("Please Select Date");
                return false;
            }
            var rows = $("#table_Indent_details tr:gt(0)");
            var Indentdetails = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtDeliveryQty').val() != "" || $(this).find('#txtDeliveryQty').val() != "0") {
                    var txtProductQty = $(this).find('#txtDeliveryQty').val();
                    var tot_qty = 0;
                    tot_qty = parseFloat(txtProductQty);
                    if (tot_qty > 0) {
                        Indentdetails.push({ BranchID: $(this).find('#txtIndentNo').text(), TripId: $(this).find('#txtunitQty').text(), PaidDate: $(this).find('#txtPaiddate').text(), PaidAmount: $(this).find('#txtDeliveryQty').val() });
                    }
                }
            });
            var rowsdenominations = $("#tableReportingDetails tr:gt(0)");
            var DenominationString = "";
            $(rowsdenominations).each(function (i, obj) {
                if ($(this).closest("tr").find(".CashClass").text() == "") {
                }
                else {
                    DenominationString += $(this).closest("tr").find(".CashClass").text() + 'x' + $(this).closest("tr").find(".qtyclass").val() + "+";
                }
            });
            var ColAmount = document.getElementById('txtAmount').innerHTML;
            var SubAmount = document.getElementById('txtSubmittedAmount').innerHTML;
            var ddlRouteName = document.getElementById('ddlRouteName').value;
            var soid = document.getElementById('ddlSalesOffice').value;
            var data = { 'operation': 'btnEditCollectionSaveClick', 'data': Indentdetails, 'refno': ddlRouteName, 'indentdate': txtDate, 'Denominations': DenominationString, 'ColAmount': ColAmount, 'SubAmount': SubAmount, 'SalesOfficeID': soid };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    $('#divFillScreen').removeTemplate();
                    $('#divFillScreen').setTemplateURL('CollectionsEdit1.htm');
                    $('#divFillScreen').processTemplate();
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
            Edit Collections<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Edit Collections</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Edit Collections Details
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
                            <select id="ddlSalesOffice" class="form-control" onchange="ddlSalesOfficeChange(this);">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap>
                            <label for="lblBranch">
                                Route Name</label>
                        </td>
                        <td style="height:40px;">
                            <select id="ddlRouteName" class="form-control" onchange="ddlAgentNameChange(this);">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap>
                            <label for="lblBranch">
                                Indent Date</label>
                        </td>
                        <td style="height:40px;">
                            <input type="date" id="datepicker" placeholder="DD-MM-YYYY" class="form-control" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <input type="button" id="Button1" value="Get Collections" class="btn btn-primary"
                                onclick="GetEditIndentValues();" />
                        </td>
                    </tr>
                </table>
                <div id="divFillScreen">
                </div>
                <div id="divdenomination">
                </div>
            </div>
        </div>
    </section>
</asp:Content>
