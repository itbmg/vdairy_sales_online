<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="incentiveapproval.aspx.cs" Inherits="incentiveapproval" %>

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
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#txtfromdate').val(today);
            FillSalesOffice();
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
        function ddlSalesOfficeChange(Id) {
            var soid = document.getElementById('ddlSalesOffice').value;
            var data = { 'operation': 'GetSalesOfficeChange', 'BranchID': Id.value };
            var s = function (msg) {
                if (msg) {
                    BindSoRouteName(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindSoRouteName(msg) {
            document.getElementById('ddlRouteName').options.length = "";
            var ddlRouteName = document.getElementById('ddlRouteName');
            var length = ddlRouteName.options.length;
            for (i = length - 1; i >= 0; i--) {
                ddlRouteName.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Route Name";
            opt.value = "";
            ddlRouteName.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].routename;
                    opt.value = msg[i].routesno;
                    ddlRouteName.appendChild(opt);
                }
            }
        }

        function btngenarate() {
            var soid = document.getElementById('ddlSalesOffice').value;
            var routeid = document.getElementById('ddlRouteName').value;
            var incentivetype = "";
            var month = document.getElementById('slctmnth').value;
            var data = { 'operation': 'getincentivependingdetails', 'BranchID': soid, 'routeid': routeid, 'incentivetype': incentivetype, 'month': month };
            var s = function (msg) {
                if (msg) {
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
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" Id="tblincentivedata">';
            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">AgentName</th><th scope="col" class="thcls">FromDate</th><th scope="col" class="thcls">ToDate</th><th scope="col">IncentiveAmout</th><th scope="col">leakpercent</th><th scope="col">Remarks</th><th scope="col"></th><th scope="col"></th><th scope="col"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row" class="1" >' + msg[i].Agentname + '</td>';
                results += '<td data-title="brandstatus"  class="2">' + msg[i].frmdate + '</td>';
                results += '<td data-title="brandstatus" style="class="3">' + msg[i].todate + '</td>';
                results += '<td data-title="brandstatus"style=" class="4">' + msg[i].Amount + '</td>';
                results += '<td data-title="brandstatus" style="class="5">' + msg[i].leakpercent + '</td>';
                results += '<td data-title="brandstatus" style="class="6">' + msg[i].Remarks + '</td>';
                results += '<td style="display:none" class="9">' + msg[i].incentivesno + '</td>';
                results += '<td style="display:none" class="10">' + msg[i].agentid + '</td>';

                results += '<td data-title="From" style="display:none;"><input id="txtAgentname" class="productcls" name="code" readonly value="' + msg[i].Agentname + '" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;"></input></td>';
                results += '<td data-title="From" style="display:none;"><input id="txttodate" class="productcls" name="code" readonly value="' + msg[i].todate + '" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;"></input></td>';
                results += '<td data-title="From" style="display:none;"><input id="txtfrmdate" class="productcls" name="code" readonly value="' + msg[i].frmdate + '" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;"></input></td>';
                results += '<td data-title="From" style="display:none;"><input id="txtAmount" class="productcls" name="code" readonly value="' + msg[i].Amount + '" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;"></input></td>';
                results += '<td data-title="From" style="display:none;"><input id="txtleakpercent" class="productcls" name="code" readonly value="' + msg[i].leakpercent + '" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;"></input></td>';
                results += '<td data-title="From" style="display:none;"><input id="txtRemarks" class="productcls" name="code" readonly value="' + msg[i].Remarks + '" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;"></input></td>';
                results += '<td data-title="From" style="display:none;"><input id="txtincentivesno" class="productcls" name="code" readonly value="' + msg[i].incentivesno + '" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;"></input></td>';
                results += '<td data-title="From" style="display:none;"><input id="txtagentid" class="productcls" name="code" readonly value="' + msg[i].agentid + '" style="width:100%; font-size:12px;padding: 0px 5px;height:30px;"></input></td>';


//                results += '<td><button type="button"  title="Click Here To view!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 prntcls"  onclick="btn_view_incentive_grid(this)"><span class="glyphicon glyphicon-th-list" style="top: 0px !important;"></span></button></td>';
//                results += '<td ><button type="button" title="Click Here To Approve!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="btn_approve_incentive_grid(this)"><span class="glyphicon glyphicon-ok-circle" style="top: 0px !important;"></span></button></td>';
//                results += '<td ><button type="button" title="Click Here To Cancel!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 removeclass"   onclick="cancelincentive(this)"><span class="glyphicon glyphicon-remove-circle" style="top: 0px !important;"></span></button></td>';
                results += '</tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#divincentivedata").html(results);
        }
        var fillstockclosing = [];
        function saveincentivedetails() {
            var soid = document.getElementById('ddlSalesOffice').value;
            var fromdate = document.getElementById('txtfromdate').value;
            var btnval = "Save";
            $('#tblincentivedata> tbody > tr').each(function () {
                var Agentname = $(this).find('#txtAgentname').val();
                var frmdate = $(this).find('#txtfrmdate').val();
                var todate = $(this).find('#txttodate').val();
                var Amount = $(this).find('#txtAmount').val();
                var leakpercent = $(this).find('#txtleakpercent').val();
                var Remarks = $(this).find('#txtRemarks').val();
                var incentivesno = $(this).find('#txtincentivesno').val();
                var agentid = $(this).find('#txtagentid').val();
                if (agentid == "" || agentid == "0" && Amount == "" || Amount == "0") {
                }
                else {
                    fillstockclosing.push({ 'soid': soid, 'Agentname': Agentname, 'frmdate': frmdate, 'todate': todate, 'Amount': Amount, 'leakpercent': leakpercent,  'incentivesno': incentivesno, 'agentid': agentid });
                }
            });
            var data = { 'operation': 'approveincentivependingdetails', 'sofid': soid, 'fromdate': fromdate, 'btnval': btnval, 'fillstockclosing': fillstockclosing };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    clearall();
                }
            }
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(data, s, e);
        }
        function clearall() {
            fillstockclosing = [];
        }
        function btn_approve_incentive_grid(thisid) {
            var sno = $(thisid).parent().parent().children('.9').html();
            var agentid = $(thisid).parent().parent().children('.10').html();
            var agentname = $(thisid).parent().parent().children('.1').html();
            var soid = document.getElementById('ddlSalesOffice').value;
            var routeid = document.getElementById('ddlRouteName').value;
            var month = document.getElementById('slctmnth').value;
            var data = { 'operation': 'approveincentivependingdetails', 'agentid': agentid, 'month': month, 'soid': soid, 'routeid': routeid, 'agentname': agentname };
            var s = function (msg) {
                if (msg) {
                    $('#divMainAddNewRows').css('display', 'block');
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function btn_view_incentive_grid(thisid) {
            var sno = $(thisid).parent().parent().children('.9').html();
            var agentid = $(thisid).parent().parent().children('.10').html();
            var agentname = $(thisid).parent().parent().children('.1').html();
            var soid = document.getElementById('ddlSalesOffice').value;
            var routeid = document.getElementById('ddlRouteName').value;
            var month = document.getElementById('slctmnth').value;
            var data = { 'operation': 'viewincentivependingdetails', 'agentid': agentid, 'month': month, 'soid': soid, 'routeid': routeid, 'agentname': agentname };
            var s = function (msg) {
                if (msg) {
                    $('#divMainAddNewRows').css('display', 'block'); 
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
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
            <section class="content">
                <div class="box box-info">
                    <div class="box-header with-border">
                    </div>
                    <div class="box-body">
                        <div id="div_AgentProductFillform">
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
                                        <td style="width: 5px;">
                                        </td>
                                        <td>
                                            <label>
                                                Route</label>
                                            <select id="ddlRouteName" class="form-control">
                                            </select>
                                        </td>
                                        <td style="width: 5px;">
                                        </td>
                                        <td>
                                            <label>
                                                Month</label>
                                            <select id="slctmnth" class="form-control">
                                                <option value="00">Select Month</option>
                                                <option value="01">January</option>
                                                <option value="02">February</option>
                                                <option value="03">March</option>
                                                <option value="04">April</option>
                                                <option value="05">May</option>
                                                <option value="06">June</option>
                                                <option value="07">July</option>
                                                <option value="08">August</option>
                                                <option value="09">September</option>
                                                <option value="10">October</option>
                                                <option value="11">November</option>
                                                <option value="12">December</option>
                                            </select>
                                        </td>
                                        <td style="width: 5px;">
                                        </td>
                                        <td>
                                <label>
                                    From Date:</label>
                            </td>
                            <td>
                                <input type="date" id="txtfromdate" class="form-control" />
                            </td>
                            <td style="width: 5px;">
                                        </td>
                                        <td>
                                            <label>
                                                &nbsp</label>
                                            <div class="input-group">
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-ok" id="Span1" onclick="btngenarate()"></span><span
                                                        id="Span2" onclick="btngenarate()">Genarate</span>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </div>
                            <div id="divincentivedata"></div>
                            <div class="input-group">
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-ok" id="Span3" onclick="saveincentivedetails()"></span><span
                                                        id="Span4" onclick="saveincentivedetails()">Save</span>
                                                </div>
                                            </div>
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
                      <asp:GridView ID="grdrpt" runat="server"></asp:GridView>
                    </div>
                    <div id="div4" style="width: 35px; top: 7.5%; right: 24%; position: absolute;
                    z-index: 99999; cursor: pointer;">
                        <img src="Images/close1.png" alt="close" onclick="OrdersCloseClicks();" style="width: 30px;height: 25px;"/>
                    </div>
                </div>
        </div>


    </section>
</asp:Content>
