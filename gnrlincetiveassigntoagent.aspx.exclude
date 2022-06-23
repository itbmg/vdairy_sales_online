<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="gnrlincetiveassigntoagent.aspx.cs" Inherits="gnrlincetiveassigntoagent" %>

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
        var LevelType = "";
        $(function () {
            FillSalesOffice();
            $("#datepicker").datepicker({ dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
                onSelect: function (selectedDate) {
                }
            });
            var LevelType = '<%=Session["LevelType"] %>';
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
            $('#divclose').css('display', 'none');
            Getinsentivrdetails();
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
                    Bindstructuredetails();
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

        function Bindstructuredetails() {
            var BranchID = document.getElementById('ddlSalesOffice').value;
            var data = { 'operation': 'getstructuredetails', 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    fillstructuredetails(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillstructuredetails(msg) {
            document.getElementById('ddlstructure').options.length = "";
            var veh = document.getElementById('ddlstructure');
            var length = veh.options.length;
            for (i = length - 1; i >= 0; i--) {
                veh.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Structure";
            opt.value = "";
            veh.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].structurename;
                    opt.value = msg[i].structurename;
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
                    BindAgentNamee(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindAgentNamee(msg) {
            document.getElementById('divchblproducts').innerHTML = "";
            //document.getElementById('divselectedprdt').innerHTML = "";
            for (var i = 0; i <= msg.length; i++) {
                if (typeof msg[i] === "undefined" || msg[i].BranchName == "" || msg[i].BranchName == null) {
                }
                else {
                    var label = document.createElement("span");
                    var hidden = document.createElement("input");
                    hidden.type = "hidden";
                    hidden.name = "hidden";
                    hidden.value = msg[i].Sno;
                    var checkbox = document.createElement("input");
                    checkbox.type = "checkbox";
                    checkbox.name = "checkbox";
                    checkbox.value = "checkbox";
                    checkbox.id = "checkbox";
                    //checkbox.className = 'checkinput';
                    checkbox.className = 'chkclass';
                    document.getElementById('divchblproducts').appendChild(checkbox);
                    label.innerHTML = msg[i].BranchName;
                    document.getElementById('divchblproducts').appendChild(label);
                    document.getElementById('divchblproducts').appendChild(hidden);
                    document.getElementById('divchblproducts').appendChild(document.createElement("br"));
                }
            }
            //TabclassClick();
        }
        var agentarray = [];
        function TabclassClick() {
            $("input[type='checkbox']").click(function () {
                if ($(this).is(":checked")) {
                    var Selected = $(this).next().text();
                    var Selectedid = $(this).next().next().val();
                    agentarray.push
                }
                else {
                    //var Selected = $(this).val();
                    var Selected = $(this).next().next().val();
                    var elem = document.getElementById(Selected);
                    var p = elem.parentNode;
                    p.removeChild(elem);
                }
            });
        }
        function RemoveClick(Selected) {
            var elem = document.getElementById(Selected);
            var p = elem.parentNode;
            p.removeChild(elem);
            $('.chkclass').each(function () {
                if ($(this).next().next().val() == Selected) {
                    $(this).attr("checked", false);
                }
            });
        }


        function BtnRaiseVoucherClick() {
            var structure = document.getElementById("ddlstructure").value;
            var SalesOfficeid = document.getElementById("ddlSalesOffice").value;
            var Routeid = document.getElementById("ddlRouteName").value;
            var leakpercent = document.getElementById("txtleakage").value;
            var btnSave = document.getElementById("btnsave").innerHTML;
            var Cashdetails = new Array();
            $("input:checkbox[class=chkclass]:checked").each(function () {
                var agent = $(this).next().next().val();
                var agentname = $(this).next().text();
                status = "P";
                Cashdetails.push({ SNo: agent, Account: agentname, amount: status });
            });
            if (!confirm("Do you want to save this transaction")) {
                return false;
            }
            var data = { 'operation': 'btnsave_genaralincentiveassigndetails', 'Cashdetails': Cashdetails, 'inserntivetype': structure, 'SalesOfficeid': SalesOfficeid,
                'Routeid': Routeid, 'leakpercent': leakpercent, 'btnSave': btnSave
            };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById("ddlstructure").selectedIndex = 0;
                    document.getElementById("ddlSalesOffice").selectedIndex = 0;
                    document.getElementById("ddlRouteName").selectedIndex = 0;
                    document.getElementById("txtleakage").value = "";
                    agentlist = [];
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


        function Getinsentivrdetails() {
            var data = { 'operation': 'getagentgeeralincentivedetails' };
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
            results += '<thead><tr style="background-color: #7ac8f5;color: black;font-size: 13px;text-align:center;"><th scope="col" style="text-align:center;font-weight: 600;">Branch Name</th><th scope="col" style="text-align:center;font-weight: 600;">Agent Name</th><th scope="col" style="text-align:center;font-weight: 600;">Structure Name</th><th scope="col" style="text-align:center;font-weight: 600;">Leak Percentage</th><th scope="col" style="text-align:center;font-weight: 600;">Status</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                results += '<td scope="row" class="1" style="text-align:center;font-size: 13px;"  >' + msg[i].SalesOfficeName + '</td>';
                results += '<td scope="row" class="1" style="text-align:center;font-size: 13px;"  >' + msg[i].Agentname + '</td>';
                results += '<td scope="row" class="50" style="text-align:center; display:none;"  >' + msg[i].SalesOfficeid + '</td>';
                results += '<td scope="row" class="50" style="text-align:center; display:none;"  >' + msg[i].agentid + '</td>';
                results += '<td scope="row" class="2"  style="text-align:center;font-size: 13px;">' + msg[i].structurename + '</td>';
                results += '<td scope="row" class="8"  style="display:none;">' + msg[i].Routeid + '</td>';
                results += '<td data-title="brandstatus"  style="text-align:center;font-size: 13px;" class="9" >' + msg[i].leakpercent + '</td>';
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
            Cashform = [];
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
                                            <select id="ddlSalesOffice" class="form-control" onchange="ddlSalesOfficeChange(this);">
                                            </select>
                                        </td>
                                         
                                        <td style="width: 4px;">
                                        </td>
                                        <td>
                                            <label>
                                                Route Name</label>
                                            <select id="ddlRouteName" class="form-control" onchange="ddlRouteNameChange(this);">
                                            </select>
                                        </td>
                                        <td style="width: 4px;">
                                        </td>
                                        <td>
                                            <label>
                                                Structure </label>
                                            <select id="ddlstructure" class="form-control">
                                            </select>
                                        </td>
                                        <td style="width: 4px;">
                                        </td>
                                        <td>
                                            <label>
                                                Leakage Percent</label>
                                            <input type="text" id="txtleakage" class="form-control"/>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table align="center">
                                    <tr>
                                        <td>
                                            <div class="box box-info" style="float: left; overflow: auto;">
                                                <div class="box-header with-border">
                                                    <h3 class="box-title">
                                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Agent Details
                                                    </h3>
                                                </div>
                                                <div class="box-body">
                                                    <div id="divchblproducts">
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="tdAgentProductSave">
                                        <td style="padding-left: 95%; !important;">
                                            <div class="input-group">
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-ok" id="btnsave1" onclick="BtnRaiseVoucherClick()">
                                                    </span><span id="btnsave" onclick="BtnRaiseVoucherClick()">Save</span>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="input-group-addon" id="divclose">
                            <input type="button" id="btnClear" value="Close" onclick="BtnclearClick();" class="btn btn-danger" />
                        </div>
                    </div>
                </div>
        </div>
    </section>
</asp:Content>
