<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="agentoverallreport.aspx.cs" Inherits="agentoverallreport" %>

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
        $(function () {
            FillSalesOffices();
        });

        function Fillmarketexicutives() {
            var soid = document.getElementById('ddlSalesOffice').value;
            var data = { 'operation': 'Getmarketexicutives', 'soid': soid };
            var s = function (msg) {
                if (msg) {
                    BindmanagersNames(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        //sno
        function BindmanagersNames(msg) {
            document.getElementById('slctmarketing').options.length = "";
            var ddlSalesRepresentative = document.getElementById('slctmarketing');
            var length = ddlSalesRepresentative.options.length;
            for (i = length - 1; i >= 0; i--) {
                ddlSalesRepresentative.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select SalesRepresentative";
            opt.value = "";
            ddlSalesRepresentative.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].sno;
                    opt.value = msg[i].sno;
                    ddlSalesRepresentative.appendChild(opt);
                }
            }
        }

        function FillSalesOffices() {
            var data = { 'operation': 'GetSalesOffice' };
            var s = function (msg) {
                if (msg) {
                    BindSalesOfficeNames(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindSalesOfficeNames(msg) {
            document.getElementById('ddlSalesOffice').options.length = "";
            var ddlSalesOffice = document.getElementById('ddlSalesOffice');
            var length = ddlSalesOffice.options.length;
            for (i = length - 1; i >= 0; i--) {
                ddlSalesOffice.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Sales Office";
            opt.value = "";
            ddlSalesOffice.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlSalesOffice.appendChild(opt);
                }
            }
        }
        function ddlddlSalesOfficeChange(Id) {
            var soid = document.getElementById('ddlSalesOffice').value;
            if (soid == "4609") {
                $('#trddltype').css("display", "none");
                var type = document.getElementById('ddltype').value;
            }
            else {
                $('#trddltype').css("display", "none");
            }
            var data = { 'operation': 'GetSalesOfficeChange', 'BranchID': Id.value };
            var s = function (msg) {
                if (msg) {
                    BindSoRouteName(msg);
                    Fillmarketexicutives();
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
        function ddlRouteNameChange(Id) {
            var data = { 'operation': 'GetRouteNameChange', 'RouteID': Id.value };
            var s = function (msg) {
                if (msg) {
                    BindBranchName(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindBranchName(msg) {
            document.getElementById('ddlBranchName').options.length = "";
            var ddlBranchName = document.getElementById('ddlBranchName');
            var length = ddlBranchName.options.length;
            for (i = length - 1; i >= 0; i--) {
                ddlBranchName.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Agent Name";
            opt.value = "";
            ddlBranchName.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null || msg[i].BranchName != "" || msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].b_id;
                    ddlBranchName.appendChild(opt);
                }
            }
        }
        function GetAgentSalesClick() {
            var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
            if (ddlSalesOffice == "Select Sales Office" || ddlSalesOffice == "") {
                alert("Please Select Sales Office");
                return false;
            }
            var ddlRouteName = document.getElementById('ddlRouteName').value;
            var SR = document.getElementById('slctmarketing').value;
            var data = { 'operation': 'GetAgetntinfosrwise', 'soid': ddlSalesOffice, 'routeid': ddlRouteName, 'SR': SR };
            var s = function (msg) {
                if (msg) {
                    Bindagentinformation(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function Bindagentinformation(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">AgentName</th><th scope="col" class="thcls">Routename</th><th scope="col" class="thcls">mobileno</th><th scope="col">salestype</th><th scope="col">collectiontype</th><th scope="col"></th><th scope="col"></th><th scope="col"></th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row" class="1" >' + msg[i].Agentname + '</td>';
                results += '<td data-title="brandstatus"  class="2">' + msg[i].Routename + '</td>';
                results += '<td data-title="brandstatus" style="class="3">' + msg[i].mobileno + '</td>';
                results += '<td data-title="brandstatus"style=" class="4">' + msg[i].salestype + '</td>';
                results += '<td data-title="brandstatus" style="class="5">' + msg[i].collectiontype + '</td>';
                results += '<td style="display:none" class="10">' + msg[i].agentid + '</td>';
                results += '<td><button type="button"  title="Click Here To view Info!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 prntcls"  onclick="btn_view_information_grid(this)"><span class="glyphicon glyphicon-th-list" style="top: 0px !important;"></span></button></td>';
                results += '<td ><button type="button" title="Click Here To View Indent!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="btn_view_indent_grid(this)"><span class="glyphicon glyphicon-ok-circle" style="top: 0px !important;"></span></button></td>';
                results += '<td ><button type="button" title="Click Here Add Marketing Materials!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 removeclass"   onclick="btn_add_material_grid(this)"><span class="glyphicon glyphicon-plus" style="top: 0px !important;"></span></button></td>';
                results += '</tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#divagentdata").html(results);
        }

        function btn_view_indent_grid(thisid) {
            var soid = document.getElementById('ddlSalesOffice').value;
            var agentid = $(thisid).parent().parent().children('.10').html();
            window.open("agentindentstatement.aspx?soidval=" + soid + ",&agentidval=" + agentid + "", "_blank");
        }

        function btn_add_material_grid(thisid) {
            var soid = document.getElementById('ddlSalesOffice').value;
            var agentid = $(thisid).parent().parent().children('.10').html();
            $('#divMainAddNewRows').css('display', 'block');
        }

        //SELECT BranchName, SalesType, phonenumber, emailid, Address, DateOfEntry, duelimit, TinNumber, Due_Limit_Type, Agent_PIC, panno, street, city, mandal, district, pincode, cst, email, gstno, bankid, ifsccode, accountno, regtype, SplBonus, sno, Due_Limit_Days, statename, doorno, LeakagePercent  FROM            branchdata
        function btn_view_information_grid(thisid) {
            var agentid = $(thisid).parent().parent().children('.10').html();
            var data = { 'operation': 'GetAgetntinformationagentwise', 'agentid': agentid };
            var s = function (msg) {
                if (msg) {
                    Bindagentoverallinformation(msg);
                    $('#divMainAddNewRows').css('display', 'none');
                    $('#divaginfo').css('display', 'block');
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function Bindagentoverallinformation(msg) {
            for (var i = 0; i < msg.length; i++) {
                var agentname = msg[i].BranchName;
                var phoneno = msg[i].phone;
                var emailid = msg[i].email;
                var gstno = msg[i].gstin;
                var panno = msg[i].panno;
                var adharno = "";
                var taxregtype = msg[i].regtype;
                var address = msg[i].address;
                var duelimittype = msg[i].Due_Limit_Type;
                var duelimit = msg[i].duelimit;
                var duelimitdays = msg[i].LimitDays;
                var doe = msg[i].CreateDate;
                var bankname = msg[i].Bankid;
                var acno = msg[i].customeraccno;
                var ifsccode = msg[i].ifsccode;
                //                    var agentdepositamount = msg[i].agentname;
                //                    var materialtype = msg[i].agentname;
                //                    var materialissudedate = msg[i].agentname;
                document.getElementById('spnagentname').innerHTML = agentname;
                document.getElementById('spnphoneno').innerHTML = phoneno;
                document.getElementById('spnmail').innerHTML = emailid;
                document.getElementById('spngst').innerHTML = gstno;
                document.getElementById('spngpan').innerHTML = panno;
                document.getElementById('spngadr').innerHTML = adharno;
                document.getElementById('Span6').innerHTML = taxregtype;
                document.getElementById('Span7').innerHTML = duelimittype;
                document.getElementById('Span8').innerHTML = duelimit;
                document.getElementById('Span9').innerHTML = doe;
                document.getElementById('Span10').innerHTML = bankname;
                document.getElementById('Span11').innerHTML = acno;
                document.getElementById('Span12').innerHTML = ifsccode;
                //                    document.getElementById('Span1').innerHTML = agentname;
                //                    document.getElementById('Span1').innerHTML = agentname;
                //                    document.getElementById('Span1').innerHTML = agentname;  


            }
        }
        function OrdersCloseClicks() {
            $('#divMainAddNewRows').css('display', 'none');
            $('#divaginfo').css('display', 'none');
        }
        function numberOnlyExample() {
            if ((event.keyCode < 48) || (event.keyCode > 57))
                return false;
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
            Agent Sale<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Agent Sale</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Agent Sale Details
                </h3>
            </div>
            <div class="box-body">
                <div style="width: 100%;">
                    <div style="width: 100%;">
                        <div style="width: 100%;">
                            <div>
                                <table align="center">
                                    <tr>
                                        <td>
                                            <label>
                                                Sales Office Name</label>
                                        </td>
                                        <td style="height: 40px;">
                                            <select id="ddlSalesOffice" class="form-control" onchange="ddlddlSalesOfficeChange(this);">
                                            </select>
                                        </td>
                                        <td>
                                        </td>
                                        <td style="display:none;">
                                            <label>
                                                Route Name</label>
                                        </td>
                                        <td style="height: 40px;display:none;">
                                            <select id="ddlRouteName" class="form-control">
                                            </select>
                                        </td>
                                        <td>
                                            <label>
                                                Marketing Person</label>
                                        </td>
                                        <td style="height: 40px;">
                                            <select id="slctmarketing" class="form-control">
                                            </select>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <input type="button" id="Button1" value="Agent Sale" class="btn btn-primary" onclick="GetAgentSalesClick();" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div id="divagentdata">
                    </div>
                </div>
            </div>
        </div>

        <div id="divMainAddNewRows" class="pickupclass" style="text-align: center; height: 100%;
                    width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
                    background: rgba(192, 192, 192, 0.7);">
                    <div id="div2" style="border: 2px solid #A0A0A0; position: absolute; top: 0%;
                    background-color: White; right: 25%; width: 50%;  -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                    -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                    border-radius: 10px 10px 10px 10px; padding:2%">
                        <table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;"
                            id="tableCollectionDetailss" class="mainText2" border="1">
                            <tr>
                                <td>
                                   <label>Material Type</label>
                                </td>
                                <td style="height:40px;">
                                    <select id="Select1" class="form-control">
                                    <option>Select</option>
                                    <option>Refrigerator</option>
                                    <option>Board</option>
                                      </select>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <label>Deposit Amount</label>
                                </td>
                                <td style="height:40px;">
                                    <input type="text" id="txtamount" class="form-control" value="" onkeypress="return numberOnlyExample();"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label> Remarks</label>
                                </td>
                                <td style="height:40px;">
                                    <input type="text" id="txtRemarkss" class="form-control" value="" 
                                        placeholder="Enter Remarks" />
                                </td>
                            </tr>
                        </table>
                        <table align="center" style="height: 40px;">
                            <tr>
                               
                                <td style="height:40px;">
                                    <input type="button" id="Button5" value="Save" onclick="btnApproveVoucherclicks();"
                                         class="btn btn-success" />
                                </td>
                               
                            </tr>
                        </table>
                    </div>
                    <div id="div4" style="width: 35px; top: 0%; right: 24%; position: absolute;
                    z-index: 99999; cursor: pointer;">
                        <img src="Images/close1.png" alt="close" onclick="OrdersCloseClicks();" style="width: 30px;height: 25px;"/>
                    </div>
                </div>


                <div id="divaginfo" class="pickupclass" style="text-align: center; height: 100%;
                    width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
                    background: rgba(192, 192, 192, 0.7);">
                    <div id="div3" style="position: absolute; top: 0%;
                    background-color: White; right: 25%;   -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                    -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                    border-radius: 10px 10px 10px 10px; padding:2%">
                        <table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;"
                            id="table1" class="mainText2" border="1">
                            <tr>
                                <td>
                                   <label>Agent Name</label>
                                </td>
                                <td>
                                  <span id="spnagentname"></span>
                                </td>
                                <td></td>
                                <td>
                                   <label>Phone No</label>
                                </td>
                                <td>
                                  <span id="spnphoneno"></span>
                                </td>
                                <td></td>
                                <td>
                                   <label>Email</label>
                                </td>
                                <td>
                                  <span id="spnmail"></span>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                   <label>Gst No</label>
                                </td>
                                <td>
                                  <span id="spngst"></span>
                                </td>
                                <td></td>
                                <td>
                                   <label>Pan No</label>
                                </td>
                                <td>
                                  <span id="spngpan"></span> 
                                </td>
                                <td></td>
                                <td>
                                   <label>Adhar No</label>
                                </td>
                                <td>
                                  <span id="spngadr"></span>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                   <label>Tax RegType</label>
                                </td>
                                <td>
                                  <span id="Span6"></span>
                                </td>
                                <td></td>
                                <td>
                                   <label>DueLimit Type</label>
                                </td>
                                <td>
                                  <span id="Span7"></span>
                                </td>
                                <td></td>
                                <td>
                                   <label>DueLimit</label>
                                </td>
                                <td>
                                  <span id="Span8"></span>
                                </td>
                                <td></td>
                                 <td>
                                   <label>Date Of Join</label>
                                </td>
                                <td>
                                  <span id="Span9"></span>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                   <label>Bank Name</label>
                                </td>
                                <td>
                                  <span id="Span10"></span>
                                </td>
                                <td></td>
                                <td>
                                   <label>Bank AccountNo</label>
                                </td>
                                <td>
                                  <span id="Span11"></span>
                                </td>
                                <td></td>
                                 <td>
                                   <label>Ifsc Code</label>
                                </td>
                                <td>
                                  <span id="Span12"></span>
                                </td>
                            </tr>
                           
                        </table>
                        <table align="center" style="height: 40px;">
                            <tr>
                               
                                <td style="height:40px;">
                                    <input type="button" id="Button2" value="Save" onclick="btnApproveVoucherclicks();"
                                         class="btn btn-success" />
                                </td>
                               
                            </tr>
                        </table>
                    </div>
                    <div id="div5" style="width: 35px; top: 0%; right: 24%; position: absolute;
                    z-index: 99999; cursor: pointer;">
                        <img src="Images/close1.png" alt="close" onclick="OrdersCloseClicks();" style="width: 30px;height: 25px;"/>
                    </div>
                </div>
    </section>
    <br />
</asp:Content>
