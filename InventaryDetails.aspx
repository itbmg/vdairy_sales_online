<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="InventaryDetails.aspx.cs" Inherits="InventaryDetails" %>
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
        .inputstable td
        {
            padding: 5px 5px 5px 5px;
        }
        .col
        {
            border: 1px solid #d5d5d5;
            text-align: center;
        }
        .BranchNames-table
        {
            width: 100%;
            border-collapse: collapse;
        }
        .BranchNames-table th
        {
            font-weight: bold;
            vertical-align: middle;
        }
        .BranchNames-table td, .BranchNames-table th
        {
            padding: 5px;
            text-align: left;
            border: 1px solid #ddd;
        }
        .BranchNames-table tr:nth-child(odd)
        {
            background: #f9f9f9;
        }
        .BranchNames-table tr:nth-child(even)
        {
            background: #ffffff;
        }
    </style>
    <style type="text/css">
        .inputstable td
        {
            padding: 5px 5px 5px 5px;
        }
        .col
        {
            border: 1px solid #d5d5d5;
            text-align: center;
        }
        .students-table
        {
            width: 100%;
            border-collapse: collapse;
        }
        .students-table th
        {
            font-weight: bold;
            vertical-align: middle;
        }
        .students-table td, .students-table th
        {
            padding: 5px;
            text-align: left;
            border: 1px solid #ddd;
        }
        .students-table tr:nth-child(odd)
        {
            background: #f9f9f9;
        }
        .students-table tr:nth-child(even)
        {
            background: #ffffff;
        }
    </style>
    <style>
        input[type=checkbox]
        {
            transform: scale(1.5);
        }
        input[type=checkbox]
        {
            width: 30px;
            height: 18px;
            margin-right: 8px;
            cursor: pointer;
            font-size: 10px;
            visibility: hidden;
        }
        input[type=checkbox]:after
        {
            content: " ";
            background-color: #fff;
            display: inline-block;
            margin-left: 10px;
            padding-bottom: 0px;
            color: #24b6dc;
            width: 16px;
            height: 16px;
            visibility: visible;
            border: 1px solid rgba(18, 18, 19, 0.12);
            padding-left: 3px;
            border-radius: 0px;
        }
        input[type="checkbox"]:not(:disabled):hover:after
        {
            border: 1px solid #24b6dc;
        }
        input[type=checkbox]:checked:after
        {
            content: "\2714";
            padding: -5px;
            font-weight: bold;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            FillSalesOffice();
            //  PoFillSalesOffice();
            $("#div_Distributor").css("display", "block");
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#datepicker').val(today);
            $('#txtTodate').val(today);
            
        });
        //        function showBranchProducts() {
        //            $("#BranchProductsShowLogs").css("display", "block");
        //            $("#div_Distributor").css("display", "block");
        //            $("#div_AgentProductFillform").css("display", "none");
        //            $("#BranchProducts_FillForm").hide();
        //        }
        //        function showDistributordesign() {
        //            FillSalesOffice();
        //            //            branches_products_branchname();
        //            $("#BranchProducts_FillForm").show();
        //            $('#BranchProductsShowLogs').hide();
        //            $("#div_Distributor").css("display", "block");
        //            $("#div_AgentProductFillform").css("display", "none");
        //        }
        //Address Details
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        function ValidateAlpha(evt) {
            var keyCode = (evt.which) ? evt.which : evt.keyCode
            if ((keyCode < 65 || keyCode > 90) && (keyCode < 97 || keyCode > 123) && keyCode != 32)

                return false;
            return true;
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
        //        function cancelAddressdetails() {
        //            $("#BranchProducts_FillForm").hide();
        //            $('#BranchProductsShowLogs').show();
        //            BranchProductsForClearAll();
        //        }
        function FillSalesOffice() {
            var getbrnachname = [];
            var data = { 'operation': 'GetPlantSalesOffice' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindSalesOffice(msg)
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
        function btnGetInventaryButtonClick() {
            var BranchID = document.getElementById('ddlSalesOffice').value;
            var fromdate = document.getElementById('datepicker').value;
            var todate = document.getElementById('txtTodate').value;
            var data = { 'operation': 'get_Plant_Wise_InventaryDetails', 'BranchID': BranchID, 'fromdate': fromdate, 'todate': todate };
            var s = function (msg) {
                if (msg) {
                    fill_AgentSale_Plant_Inventary_Details(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        };
        var Temptable = [];
        function fill_AgentSale_Plant_Inventary_Details(msg) {
            var status = "0";
            var present = '';
            var results = '<div  style="overflow:auto;"><table align="left" class="table table-bordered table-hover dataTable no-footer" id="tableaProductdetails">';
            results += '<thead><tr><th>Date</th><th>InventaryName</th><th>Openning</th><th>Issued</th><th>Received</th><th>Closing</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#794b4b", "#FF6600", "#fdd400", "#84b761", "#cc4748", "#cd82ad", "#2f4074", "#448e4d", "#b7b83f", "#b9783f", "#b93e3d", "#913167", "#18d79c", "#a2907f", "#41ee80", "#8cfa40", "#dbdf9d", "#3f4fb2", "#01909b", "#124c65", "#33879b", "#869ae2"];
            for (var i = 0; i < msg.length; i++) {
                //                if (msg[i].IssuedQty != "0" && msg[i].ReceivedQty != "0") {
                results += '<tr>';
                results += '<td   class="2"><div id="txtamount" style="font-size: 14px;font-weight: bold;cursor:pointer;" title="view Product in Line Chart"   class="subCategeoryClass"><b>' + msg[i].closingdate + '<b></td>';
                results += '<td style="width: 33%; heights: 20px; vertical-align: middle; font-size: 16px; text-align: center;"  class="2"><div id="txtAccount" style="font-size: 12px; font-weight: bold;" onclick="btnGetDaywise_Inventary_Click(\'' + msg[i].BranchID + '\');" class="AccountClass"><b>' + msg[i].InvName + '</b></td>';
                results += '<td   class="2"><div id="txtamount" style="font-size: 14px;font-weight: bold;cursor:pointer;" title="view Product in Line Chart"   class="subCategeoryClass"><b>' + msg[i].opening + '<b></td>';
                results += '<td   class="2"><div id="txtamount" style="font-size: 14px;font-weight: bold;cursor:pointer;" title="view Product in Line Chart"   class="subCategeoryClass"><b>' + msg[i].IssuedQty + '<b></td>';
                results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;" class="2" ><div style="font-size: 14px;  font-weight: bold;"><b>' + msg[i].ReceivedQty + '</b></div></td>';
                results += '<td   class="2"><div id="txtamount" style="font-size: 14px;font-weight: bold;cursor:pointer;" title="view Product in Line Chart"   class="subCategeoryClass"><b>' + msg[i].closing + '<b></td></tr>';
                l = l + 1;
                if (l == 30) {
                    l = 0;
                }
                //                }
            }
            results += '</table></div>';
            $("#div_students").html(results);
        }
        var BranchID = "";
        function btnGetDaywise_Inventary_Click(BranchId) {
            BranchID = BranchId;
            var fromdate = document.getElementById('datepicker').value;
            var todate = document.getElementById('txtTodate').value;
            var data = { 'operation': 'get_Plant_DayWise_InventaryDetails', 'BranchID': BranchID, 'fromdate': fromdate, 'todate': todate };
            var s = function (msg) {
                if (msg) {
                    fill_Plant_DayWise_Inventary_Details(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        };
        function PopupCloseClick() {
            $('#div_agentwisemain1').css('display', 'none');
        }
        var Temptable = [];
        function fill_Plant_DayWise_Inventary_Details(msg) {
            $('#div_agentwisemain1').css('display', 'block');
            var status = "0";
            var present = '';
            var results = '<div  style="overflow:auto;"><table align="left" class="table table-bordered table-hover dataTable no-footer" id="tableaProductdetails">';
            results += '<thead><tr><th>Date</th><th>InventaryName</th><th>Openning</th><th>Issued</th><th>Received</th><th>Closing</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#794b4b", "#FF6600", "#fdd400", "#84b761", "#cc4748", "#cd82ad", "#2f4074", "#448e4d", "#b7b83f", "#b9783f", "#b93e3d", "#913167", "#18d79c", "#a2907f", "#41ee80", "#8cfa40", "#dbdf9d", "#3f4fb2", "#01909b", "#124c65", "#33879b", "#869ae2"];
            for (var i = 0; i < msg.length; i++) {
                //                if (msg[i].IssuedQty != "0" && msg[i].ReceivedQty != "0") {
                results += '<tr>';
                results += '<td   class="2"><div id="txtamount" style="font-size: 14px;font-weight: bold;cursor:pointer;" title="view Product in Line Chart"   class="subCategeoryClass"><b>' + msg[i].closingdate + '<b></td>';
                results += '<td style="width: 33%; heights: 20px; vertical-align: middle; font-size: 16px; text-align: center;"  class="2"><div id="txtAccount" style="font-size: 12px; font-weight: bold;" onclick="btnGetBranch_Inventary_Click(\'' + msg[i].BranchID + '\');" class="AccountClass"><b>' + msg[i].InvName + '</b></td>';
                results += '<td   class="2"><div id="txtamount" style="font-size: 14px;font-weight: bold;cursor:pointer;" title="view Product in Line Chart"   class="subCategeoryClass"><b>' + msg[i].opening + '<b></td>';
                results += '<td   class="2"><div id="txtamount" style="font-size: 14px;font-weight: bold;cursor:pointer;" title="view Product in Line Chart"   class="subCategeoryClass"><b>' + msg[i].IssuedQty + '<b></td>';
                results += '<td class="2" ><div style="font-size: 14px;  font-weight: bold;"><b>' + msg[i].ReceivedQty + '</b></div></td>';
                results += '<td   class="2"><div id="txtamount" style="font-size: 14px;font-weight: bold;cursor:pointer;" title="view Product in Line Chart"   class="subCategeoryClass"><b>' + msg[i].closing + '<b></td></tr>';
                l = l + 1;
                if (l == 30) {
                    l = 0;
                }
                //                }
            }
            results += '</table></div>';
            $("#div_Branch_Inventary").html(results);
        }

        //
        var BranchID = "";
        function btnGetBranch_Inventary_Click(BranchId) {
            BranchID = BranchId;
            var fromdate = document.getElementById('datepicker').value;
            var data = { 'operation': 'get_Branch_Wise_InventaryDetails', 'BranchID': BranchID, 'fromdate': fromdate };
            var s = function (msg) {
                if (msg) {
                    fill_Branch_Wise_Inventary_Details(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        };
        function PopupCloseClick() {
            $('#div_agentwisemain1').css('display', 'none');
        }
        var Temptable = [];
        function fill_Branch_Wise_Inventary_Details(msg) {
            $('#div_agentwisemain1').css('display', 'block');
            var status = "0";
            var present = '';
            var results = '<div  style="overflow:auto;"><table align="left" class="table table-bordered table-hover dataTable no-footer" id="tableaProductdetails">';
            //results += '<thead><tr><th>DispatchName</th><th>ReceivedCrates</th><th>ReturnCrates</th><th>DifferenceCrates</th><th>ReceivedCan40ltr</th><th>DifferenceCan40ltr</th><th>ReturnCan40ltr</th><th>ReceivedCan20ltr</th><th>ReturnCan20ltr</th><th>DifferenceCan20ltr</th></tr></thead></tbody>';
            results += '<thead><tr><th>DispatchName</th><th>Received ' + msg[0].InvName + '</th><th>Return ' + msg[0].InvName + '</th><th>Difference ' + msg[0].InvName + '</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#794b4b", "#FF6600", "#fdd400", "#84b761", "#cc4748", "#cd82ad", "#2f4074", "#448e4d", "#b7b83f", "#b9783f", "#b93e3d", "#913167", "#18d79c", "#a2907f", "#41ee80", "#8cfa40", "#dbdf9d", "#3f4fb2", "#01909b", "#124c65", "#33879b", "#869ae2"];
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].ReceivedCrates != "0" && msg[i].ReturnCrates != "0") {
                    results += '<tr>';
                    results += '<td style="width: 33%; heights: 20px; vertical-align: middle; font-size: 16px; text-align: center;"  class="2"><div id="txtAccount" style="font-size: 12px; font-weight: bold;"  class="AccountClass"><b>' + msg[i].BranchName + '</b></td>';
                    results += '<td   class="2"><div id="txtamount" style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;"  title="view Product in Line Chart"   class="subCategeoryClass"><b>' + msg[i].ReceivedCrates + '<b></td>';
                    results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;"  class="2"><div id="Span1" style="font-size: 14px;  font-weight: bold;" class="QtyClass"><b>' + msg[i].ReturnCrates + '</b></td>';
                    results += '<td style="width: 33%; height: 20px; vertical-align: middle; font-size: 16px; text-align: center;"  class="2"><div id="Span1" style="font-size: 14px;  font-weight: bold;" class="QtyClass"><b>' + msg[i].DifferenceCrates + '</b></td></tr>';
                    l = l + 1;
                    if (l == 30) {
                        l = 0;
                    }
                }
            }
            results += '</table></div>';
            $("#div_Branch_Inventary").html(results);
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content">
        <!-- Small boxes (Stat box) -->
        <div class="row">
            <section class="content-header">
                <h1>
                    Inventary Report
                </h1>
                <ol class="breadcrumb">
                    <li><a href="#"><i class="fa fa-dashboard"></i>Reports</a></li>
                    <li><a href="#">Inventary Report</a></li>
                </ol>
            </section>
            <section class="content">
                <div class="box box-info">
                    <div class="box-header with-border">
                    </div>
                    <div class="box-body">
                        <div id="div_Distributor">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Branch Inventary Details
                                </h3>
                            </div>
                            <div class="box-body">
                                <%-- <div id="BranchProductsShowLogs" align="center">
                                        <input id="btn_addAddress" type="button" name="submit" value='AddAddress' class="btn btn-primary"
                                            onclick="showDistributordesign()" />
                                    </div>--%>
                                <%-- <div id="BranchProductsShowLogs" align="right" class="input-group" style="display: block;
                                    padding-bottom: 20px;">
                                    <div class="input-group-addon" style="width: 100px;">
                                        <span class="glyphicon glyphicon-plus-sign" onclick="showDistributordesign();"></span>
                                        <span onclick="showDistributordesign();">Add Distributor</span>
                                    </div>
                                </div>--%>
                                <%-- <div id="div_BranchProductsData">
                                </div>--%>
                                <div id='BranchProducts_FillForm'>
                                    <table align="center">
                                        <tr>
                                            <td>
                                                <label>
                                                    BranchName</label>
                                                    </td>
                                                    <td>
                                                <select id="ddlSalesOffice" class="form-control" onchange="ddlSalesOfficeChanged();">
                                                </select>
                                            </td>
                                            <td style="width: 25px;">
                                            </td>
                                            <td>
                                                <label>
                                                   From Date
                                                </label>
                                                </td>
                                                <td>
                                                <input type="date" id="datepicker" placeholder="DD-MM-YYYY" class="form-control" />
                                            </td>
                                            <td style="width: 25px;">
                                            </td>
                                            <td>
                                                <label>
                                                   To Date
                                                </label>
                                                </td>
                                                <td>
                                                <input type="date" id="txtTodate" placeholder="DD-MM-YYYY" class="form-control" />
                                            </td>
                                            <td style="width: 25px;">
                                            </td>
                                            <td>
                                                <div class="input-group">
                                                    <div class="input-group-addon">
                                                        <span class="glyphicon glyphicon-ok" id="spnInventaryBtn1" onclick="btnGetInventaryButtonClick()">
                                                        </span><span id="spnInventaryBtn" onclick="btnGetInventaryButtonClick()">Get</span>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <%-- <tr>
                                            <td>
                                                <div id="div_students" style="font-family: 'Open Sans';
                                                    font-size: 13px;">
                                                </div>
                                            </td>
                                        </tr>--%>
                                    </table>
                                    <div>
                                    </div>
                                </div>
                                <div class="modal fade in" id="div_agentwisemain1" style="display: none; padding-right: 17px;">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header" style="text-align: right">
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                    <span aria-hidden="true" onclick="PopupCloseClick();">×</span></button>
                                                <h4 class="modal-title">
                                                    Branch Wise Inventary Details</h4>
                                            </div>
                                            <div class="modal-body" id="div_agentwise1" style="height: 400px;overflow-y: scroll;">
                                                <div class="modal-body" id="div_Branch_Inventary" style="height: 400px;">
                                                </div>
                                            </div>
                                        </div>
                                        <!-- /.modal-content -->
                                    </div>
                                    <!-- /.modal-dialog -->
                                </div>
                                <div id="div_students" style="font-family: 'Open Sans'; font-size: 13px;">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
        </div>
    </section>
</asp:Content>
