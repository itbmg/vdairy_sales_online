<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="generalincentivestructure.aspx.cs" Inherits="generalincentivestructure" %>

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
        var LevelType = "";
        $(function () {
            FillSalesOffice();
            Getinsentivrdetails();
        });
        function Getinsentivrdetails() {
            var data = { 'operation': 'Getgeneralinsentivedetails' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    Bindincentivedetails(msg);

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function Bindincentivedetails(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            var LevelType = '<%=Session["LevelType"] %>';
            results += '<thead><tr style="background-color: #7ac8f5;color: black;font-size: 13px;text-align:center;"><th scope="col" style="text-align:center;font-weight: 600;">Branch Name</th><th scope="col" style="text-align:center;font-weight: 600;">Structure Name</th><th scope="col" style="text-align:center;font-weight: 600;">Product Name</th><th scope="col" style="text-align:center;font-weight: 600;">Price</th><th scope="col" style="text-align:center;font-weight: 600;">Status</th><th scope="col" style="text-align:center;font-weight: 600;"></th><th></th><th></th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                results += '<td scope="row" class="1" style="text-align:center;font-size: 13px;"  >' + msg[i].SalesOfficeName + '</td>';
                results += '<td scope="row" class="50" style="text-align:center; display:none;"  >' + msg[i].SalesOfficeid + '</td>';
                results += '<td scope="row" class="2"  style="text-align:center;font-size: 13px;">' + msg[i].structurename + '</td>';
                results += '<td scope="row" class="8"  style="display:none;">' + msg[i].productid + '</td>';
                results += '<td data-title="brandstatus"  style="text-align:center;font-size: 13px;" class="9" >' + msg[i].productname + '</td>';
                results += '<td data-title="brandstatus"  style="text-align:center;font-size: 13px;" class="4">' + msg[i].price + '</td>';
                results += '<td data-title="brandstatus"  style="text-align:center;font-size: 13px;" class="6"  >' + msg[i].status + '</td>';
                results += '<td data-title="brandstatus"  style="display:none;" style="text-align:center;" class="10">' + msg[i].sno + '</td>';
//                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="geteditdetails(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>';
//                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Print!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 prntcls"  onclick="printvoucher(this)"><span class="glyphicon glyphicon-print" style="top: 0px !important;"></span></button></td>';
//                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Cancel!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 removeclass"   onclick="cancelvoucher(this)"><span class="glyphicon glyphicon-remove-circle" style="top: 0px !important;"></span></button></td></tr>';
                results += '</tr>';
            }
            results += '</table></div>';
            $("#divincentivedata").html(results);
        }

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
        function GetBranchProducts() {
            var branchid = document.getElementById("ddlSalesOffice").value;
            var ddlproducttype = "branch";
            var data = { 'operation': 'GetBranchProducts', 'BranchID': branchid, 'ddlproducttype': ddlproducttype };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    fillproductdetails(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };

            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        var Temptable1 = [];
        function fillproductdetails(msg) {
            var results = '<div  style="overflow:auto;"><table id="tblproducts" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr class="trbgclrcls"><th>Sno</th><th scope="col" class="thcls">ProductName</th><th scope="col" class="thcls">Incentive Price</th><th scope="col"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row" class="1 tdmaincls" >' + k + '</td>';
                results += '<td scope="row" class="1 tdmaincls" >' + msg[i].ProdName + '</td>';
                results += '<td style="display:none" class="11"><input id="hdnempsno" class="clsProductsno" type="hidden" value="' + msg[i].ProdID + '" /></td>';
                results += '<td style="display:none" class="13"><input id="txtpname" class="clsProductname" type="text" value="' + msg[i].ProdName + '" /></td>';
                results += '<td class="13"><input type="text" class="clsRate" id="txtRate"/></td>';
                results += '</tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
                k++;
            }
            results += '</table></div>';
            $("#divAgentproducts").html(results);
            $("#tdAgentProductSave").css("display", "block");
        }


        function BtnRaiseVoucherClick() {
            var structurename = document.getElementById("txtstruname").value;
            var SalesOfficeid = document.getElementById("ddlSalesOffice").value;
            var btnSave = document.getElementById("btnAgentSave").innerHTML;
            var rows = $("#tblproducts tr:gt(0)");
            var Cashdetails = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtRate').val() != "") {
                    Cashdetails.push({ SNo: $(this).find('#hdnempsno').val(), Account: $(this).find('#txtpname').val(), amount: $(this).find('#txtRate').val() });
                }
            });
            if (!confirm("Do you want to save this transaction")) {
                return false;
            }
            var data = { 'operation': 'btnsave_genaralincentivedetails', 'Cashdetails': Cashdetails, 'btnSave': btnSave, 'inserntivetype': structurename, 'SalesOfficeid': SalesOfficeid
            };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById("ddlinserntivetype").selectedIndex = 0;
                    document.getElementById("ddlSalesOffice").selectedIndex = 0;
                    document.getElementById("ddlRouteName").selectedIndex = 0;
                    document.getElementById("ddlAgentName").selectedIndex = 0;
                    document.getElementById("ddlEmpApprove").selectedIndex = 0;
                    Cashform = [];
                    Getinsentivrdetails();
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
        function cancelAddressdetails() {
            $("#BranchProducts_FillForm").hide();
            $('#BranchProductsShowLogs').show();
            BranchProductsForClearAll();
        }
        function divonclick(selected) {
            selectedindex = selected;
            if ($(selected).css('background-color') == 'rgb(255, 255, 255)' || $(selected).css('background-color') == 'rgba(0, 0, 0, 0)') {
                $('.divselectedclass').each(function () {
                    $(this).css('background-color', '#ffffff');
                });
                $(selected).css('background-color', '#ffffcc');
            }
            else {
                $('.divselectedclass').each(function () {
                    $(this).css('background-color', '#ffffff');
                });
            }
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
            $('#div_AgentProductFillform').css('display', 'block');
            $('#div_raisevoucher').css('display', 'none');
            $('#divclose').css('display', 'block');
        }

        function BtnclearClick() {
            $('#div_AgentProductFillform').css('display', 'none');
            $('#div_raisevoucher').css('display', 'block');
            $('#divclose').css('display', 'none');
            Cashform = [];
        }
        //CheckBox Function

        function selectall_checks(thisid) {
            if ($(thisid).is(':checked')) {
                $(this).find(':checkbox').prop('checked', true);
            }
            else {
                $(this).find(':checkbox').prop('checked', true);
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content">
        <!-- Small boxes (Stat box) -->
        <div class="row">
            <section class="content">
                <div class="box box-info">
                    <div class="box-header with-border">
                    </div>
                    <div class="box-body">
                        <div id="div_raisevoucher" style="text-align: -webkit-right;">
                            <table>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <div class="input-group" style="cursor: pointer;">
                                            <div class="input-group-addon">
                                                <span class="glyphicon glyphicon-plus-sign" onclick="divRaiseVoucherClick()"></span>
                                                <span onclick="divRaiseVoucherClick()">Add Insentive</span>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div id="divincentivedata">
                            </div>
                        </div>
                        <div id="div_AgentProductFillform" style="display: none;">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Incentive Details
                                </h3>
                            </div>
                            <div class="box-body">
                                <table>
                                    <tr>
                                        <td>
                                            <label>
                                                Sales Office</label>
                                            <select id="ddlSalesOffice" class="form-control" onchange="GetBranchProducts();">
                                            </select>
                                        </td>
                                        <td style="width:5px;"></td>
                                         <td>
                                            <label>
                                                Structure Name</label>
                                            <input type="text" id="txtstruname" class="form-control"/>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table align="center">
                                    <tr>
                                        <td>
                                            <div class="box box-info" style="float: left;">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Product Details
                                                    </h3>
                                                </div>
                                                <div class="box-body">
                                                    <div id="divAgentproducts">
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="tdAgentProductSave" style="display: none">
                                        <td style="padding-left: 55%; !important;">
                                            <div class="input-group">
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-ok" id="btnAgentSave1" onclick="BtnRaiseVoucherClick()">
                                                    </span><span id="btnAgentSave" onclick="BtnRaiseVoucherClick()">Save</span>
                                                </div>
                                                
                                            </div>
                                           
                                        </td>
                                        
                                    </tr>
                                </table>
                                <<div class="input-group">
                                            <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-ok" id="Span1" onclick="BtnclearClick()">
                                                    </span><span id="Span2" onclick="BtnclearClick()">Close</span>
                                                </div>
                                            </div>
                            </div>
                        </div>
                    </div>
                </div>
        </div>
    </section>
</asp:Content>

