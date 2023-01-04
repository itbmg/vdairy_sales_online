<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TripEnd.aspx.cs" Inherits="TripEnd" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="Kendo/jquery.min.js" type="text/javascript"></script>
    <script src="Kendo/kendo.all.min.js" type="text/javascript"></script>
    <link href="Kendo/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="Kendo/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <link href="Css/Timeline.css" rel="stylesheet" type="text/css" />
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <link href="Css/RouteWiseTimeLine.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .ddlsize
        {
            width: 280px;
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
        .plan
        {
            width: 50%;
        }
    </style>
    <style type="text/css">
        .active
        {
            color: #555555;
            text-decoration: none;
            background-color: #e5e5e5;
        }
        .cp
        {
            /* these styles will let the divs line up next to each other
       while accepting dimensions */
            display: inline-block;
            width: 10%;
            height: 800px; /*background: black;*/ /* a small margin to separate the blocks */
            margin-left: 10px;
            margin-right: 10px;
            margin-top: 10px;
            margin-bottom: 10px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);

        });
    </script>
    <script type="text/javascript">
        $(function () {
            filldispatchname();
            get_Indent_Employees();
        });
        var DispSno = "";
        var DispName = "";
        var tripid = "";
        function GetEndDispatches() {
            var data = { 'operation': 'GetTripDispPlanDetails' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        DispName = msg[0].DispName;
                        document.getElementById('txtRoute').innerHTML = msg[0].DispName;
                        DispSno = msg[0].DispSno;
                        tripid = msg[0].tripid;
                        GetReturnsAndLeaks();
                        $('#Button2').css('display', 'none');
                        $('#divAmount').css('display', 'none');
                        $('#DivReturn').css('display', 'block');
                    }
                    else {
                        $('#Button2').css('display', 'block');
                        $('#DivReturn').css('display', 'none');
                        $('#divAmount').css('display', 'block');
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
        function GetReturnsAndLeaks() {
            BindVerifyInventory();
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
        function fillcsoroutes() {
            var data = { 'operation': 'GetCsodispatchRoutes' };
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
            opt.innerHTML = "Select Route";
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
        function BindVerifyInventory() {
            var data = { 'operation': 'GetVerifyInventory', 'RouteSno': DispSno, 'tripsno': tripid };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    $('#divfilltirp_end_details').setTemplateURL('InventoryVerify5.htm');
                    $('#divfilltirp_end_details').processTemplate(msg);
                    BindLeakReporting();
                    BindReturnReporting();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindLeakReporting() {
            var data = { 'operation': 'GetVerifyLeaks', 'RouteSno': DispSno, 'tripsno': tripid };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    $('#LeakReporting').removeTemplate();
                    $('#LeakReporting').setTemplateURL('VerifyLeaks.htm');
                    $('#LeakReporting').processTemplate(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindReturnReporting() {
            var data = { 'operation': 'GetVerifyReturns', 'RouteSno': DispSno, 'tripsno': tripid };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    $('#ReturnReporting').removeTemplate();
                    $('#ReturnReporting').setTemplateURL('VarifyReturns.htm');
                    $('#ReturnReporting').processTemplate(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function btnReturnsVarifySaveClick() {
            if (!confirm("Do you really want Save")) {
                return false;
            }
            var rowsVarifyReturndetails = $("#VarifyReturndetails tr:gt(0)");
            var VarifyReturndetails = new Array();
            $(rowsVarifyReturndetails).each(function (i, obj) {
                if ($(this).find('#txtsno').text() != "") {
                    VarifyReturndetails.push({ ProductID: $(this).find('#hdnProductSno').val(), ReturnsQty: $(this).find('#txtReturnsQty').val(), TripID: $(this).find('#hdnTripID').val(), ReturnRemarks: $(this).find('#txtreturnRemarks').val() });
                }
            });
            var data = { 'operation': 'btnReturnsVarifySaveClick', 'RouteLeakdetails': VarifyReturndetails };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    if (msg == "Session Expired") {
                        window.location = "Login.aspx";
                    }
                    BindReturnReporting();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(data, s, e);
        }
        function btnLeakVarifySaveClick() {
            if (!confirm("Do you really want Save")) {
                return false;
            }
            var rowsVarifyRouteLeakdetails = $("#VarifyLeakdetails tr:gt(0)");
            var VarifyRouteLeakdetails = new Array();
            $(rowsVarifyRouteLeakdetails).each(function (i, obj) {
                if ($(this).find('#txtsno').text() != "") {
                    VarifyRouteLeakdetails.push({ ProductID: $(this).find('#hdnProductSno').val(), LeaksQty: $(this).find('#txtLeaksQty').val(), TripID: $(this).find('#hdnTripID').val(), Remarks: $(this).find('#txtleakremarks').val() });
                }
            });
            var data = { 'operation': 'btnLeakVarifySaveClick', 'RouteLeakdetails': VarifyRouteLeakdetails };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    if (msg == "Session Expired") {
                        window.location = "Login.aspx";
                    }
                    BindLeakReporting();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(data, s, e);
        }
        function btnInventoryVerifySaveClick() {
            if (!confirm("Do you really want Save")) {
                return false;
            }
            var rows = $("#tableInventoryVerify tr:gt(0)");
            var InvDatails = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtSno').text() != "" || $(this).find('#txtSubmittQty').val() != "") {
                    InvDatails.push({ SNo: $(this).find('#hdnInvSno').val(), Qty: $(this).find('#txtSubmittQty').val(), TripID: $(this).find('#hdntripID').val(), Remarks: $(this).find('#txtinventoryremarks').val() });
                }
            });
            var data = { 'operation': 'btnInventoryVerifySaveClick', 'InvDatails': InvDatails };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    if (msg == "Session Expired") {
                        window.location = "Login.aspx";
                    }
                    BindVerifyInventory();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(data, s, e);
        }
        function btn_getdetails_click() {
            var IndentDate = document.getElementById('datepicker').value;
            if (IndentDate == "") {
                alert("Please Select Date");
                return false;
            }
            var data = { 'operation': 'GetTripEnd_Details', 'IndentDate': IndentDate };
            var s = function (msg) {
                if (msg) {
                    fillTripEndDetails(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function numberOnlyExample() {
            if ((event.keyCode < 48) || (event.keyCode > 57))
                return false;
        }
        function fillTripEndDetails(msg) {
            $('#divfilltirp_end_details').removeTemplate();
            $('#divfilltirp_end_details').setTemplateURL('RoutewiseTripEnd4.htm');
            $('#divfilltirp_end_details').processTemplate(msg);
//            var branchid = '<%=Session["branch"] %>';
//            if (branchid == "159" || branchid == "4626") {
//                $('.txtreceivedamount').attr('readonly', true);
//            }
//            else {

//                $('#txtspnreceivedamount1').css('display', 'none');
//                $('.txtreceivedamount').attr('readonly', false);
//            }
        }
        function btnTripendclick(id) {
            var TripId = $(id).closest("tr").find('#txttripid').text();
            var EmpName = $(id).closest("tr").find('#txtempname').text();
            var RouteName = $(id).closest("tr").find('#txtroutename').text();
            var CollectedAmt = $(id).closest("tr").find('#txtcollectedamt').text();
            var SubmittedAmt = $(id).closest("tr").find('#txtsubmittedamt').text();
            var ReceivedAmt = $(id).closest("tr").find('#txtreceivedamount').val();
            var Remarks = $(id).closest("tr").find('#txtremarks').val();
            var txt_Total = document.getElementById("txt_Total").innerHTML;
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
            if (ReceivedAmt == txt_Total) {
            }
            else {
                alert("Please fill denomination amount");
                return false;
            }
            var data = { 'operation': 'btnPlantTrip_EndSaveClick', 'TripId': TripId, 'SubmittedAmt': SubmittedAmt, 'ReceivedAmt': ReceivedAmt, 'Remarks': Remarks, 'DenominationString': DenominationString };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    //btnPlantTripRefreshClick();
                    btn_getdetails_click();
                    Denominationzero();
                }
                else {
                }
            };
            var e = function (x, h, e) {
                //                    alert("something went wrong");
                //                }
            };
            callHandler(data, s, e);
        }
        function Denominationzero() {
            var Total = 0;
            $('.qtyclass').val(Total);
            $('.TotalClass').text(Total);
            document.getElementById('txt_Total').innerHTML = Total;
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
        function GetAmountDetails() {
            $('#DivReturn').css('display', 'none    ');
            $('#divAmount').css('display', 'block');
        }
        function get_Indent_Employees() {
            var data = { 'operation': 'get_Indent_Employees' };
            var s = function (msg) {
                if (msg) {
                    fillemployees(msg);
                    //                    fillemployees1(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function get_Indent_Employees1() {
            var data = { 'operation': 'get_Indent_Employees' };
            var s = function (msg) {
                if (msg) {
                    //                    fillemployees(msg);
                    fillemployees1(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillemployees(msg) {
            document.getElementById('Slect_EmpName').options.length = "";
            var veh = document.getElementById('Slect_EmpName');
            var length = veh.options.length;
            for (i = length - 1; i >= 0; i--) {
                veh.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Employee";
            opt.value = "";
            veh.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].EmpName;
                    opt.value = msg[i].sno;
                    veh.appendChild(opt);
                }
            }
        }
        function filldispatchname() {
            var data = { 'operation': 'get_Plant_TripRoutes' };
            var s = function (msg) {
                if (msg) {
                    BranchNamewisebankname(msg);
                    //                    BranchNamewisebankname1(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function filldispatchname1() {
            var data = { 'operation': 'get_Plant_TripRoutes' };
            var s = function (msg) {
                if (msg) {
                    //                    BranchNamewisebankname(msg);
                    BranchNamewisebankname1(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var dispatchdta;
        function BranchNamewisebankname(msg) {
            document.getElementById('divchkdispatch').innerHTML = "";
            document.getElementById('div_chkRouteName').innerHTML = "";
            dispatchdta = msg;
            var branch = [];
            var selectedbranch = "";
            for (var i = 0; i < dispatchdta.length; i++) {
                if (typeof dispatchdta[i] === "undefined" || dispatchdta[i].Branchname == "" || dispatchdta[i].Branchname == null) {
                }
                else {
                    var tbranchname = dispatchdta[i].Branchname;
                    var tbranchid = dispatchdta[i].Branchname;
                    if (selectedbranch != "Select All") {
                        //                        if (tbranchid == selectedbranch) {
                        tbranchname = tbranchname.replace(/[^a-zA-Z0-9]/g, '');
                        var exists = branch.indexOf(tbranchname);
                        if (exists == -1) {
                            var branchname = dispatchdta[i].Branchname;
                            branchname = branchname.replace(/[^a-zA-Z0-9]/g, '');
                            branch.push(branchname);
                            $("#divchkdispatch").append("<div id='div" + branchname + "' class='divcategory'>");
                        }
                        //                        }
                    }
                    else {
                        tbranchname = tbranchname.replace(/[^a-zA-Z0-9]/g, '');
                        var exists = branch.indexOf(tbranchname);
                        if (exists == -1) {
                            var branchname = dispatchdta[i].Branchname;
                            branchname = branchname.replace(/[^a-zA-Z0-9]/g, '');
                            branch.push(branchname);
                            $("#divchkdispatch").append("<div id='div" + branchname + "' class='divcategory'>");
                        }
                    }
                }
            }
            for (var p = 0; p < branch.length; p++) {
                $("#div" + branch[p] + "").append("<div class='titledivcls'><table id='banktable' style='width:100%;'><tr><td style='width: 120px;'><h2 class='unitline'>" + branch[p] + "</h2></td><td></td><td style='padding-right: 20px;vertical-align: middle;'><span class='iconminus' title='Hide' onclick='minusclick(this);'></span></td></tr></table></div>");
                $("#div" + branch[p] + "").append("<ul style='padding-top: 9% !important;' id='ul" + branch[p] + "' class='ulclass' >");
                for (var i = 0; i < dispatchdta.length; i++) {
                    var tbranchname = dispatchdta[i].Branchname;
                    tbranchname = tbranchname.replace(/[^a-zA-Z0-9]/g, '');
                    if (typeof dispatchdta[i] === "undefined" || dispatchdta[i].RouteName == "" || dispatchdta[i].RouteName == null) {
                    }
                    else {
                        if (branch[p] == tbranchname) {
                            var label = document.createElement("span");
                            var hidden = document.createElement("input");
                            hidden.type = "hidden";
                            hidden.Bankname = "hidden";
                            hidden.value = dispatchdta[i].Route_id;
                            var checkbox = document.createElement("input");
                            checkbox.type = "checkbox";
                            checkbox.Bankname = "checkbox";
                            checkbox.value = dispatchdta[i].Route_id;
                            checkbox.id = "checkbox";
                            checkbox.className = 'chkclass';
                            document.getElementById('ul' + branch[p]).appendChild(checkbox);
                            label.innerHTML = dispatchdta[i].RouteName;
                            document.getElementById('ul' + branch[p]).appendChild(label);
                            document.getElementById('ul' + branch[p]).appendChild(hidden);
                            document.getElementById('ul' + branch[p]).appendChild(document.createElement("br"));
                        }
                    }
                }
                //                checkbox.onclick = TabclassClick();
            }
        }
        function minusclick(thisid) {
            var prntdiv = $(thisid).parents(".titledivcls");
            ul = prntdiv.next("ul");
            if (thisid.title == "Hide") {
                ul.slideUp("slow");
                $(thisid).attr('title', "Show");
                $(thisid).css("background", "url('Images/plus.png') no-repeat");
            }
            else {
                ul.slideDown("slow");
                $(thisid).attr('title', "Hide");
                $(thisid).css("background", "url('Images/minus.png') no-repeat");
            }
        }


        //        var Dispatcharr = [];
        //         function TabclassClick() {
        //             $("input[type='checkbox']").click(function () {
        //                 $('input.chkclass:checkbox:checked').each(function () {
        //                     var sThisVal = $(this).val();
        //                     Dispatcharr.push({ 'dispatchid': sThisVal });
        //                 });
        //             });
        //         }
        function save_DispatchAssignDetails() {
            var Dispatcharr = [];
            $("input:checkbox[class=chkclass]:checked").each(function () {
                var dispatchid = $(this).val();
                var abc = { dispatchid: dispatchid };
                Dispatcharr.push(abc);
            });

            if (Dispatcharr == 0) {
                alert("You must select at least one checkbox!");
                return false;
            }
            var btnVal = document.getElementById('btn_save').value;
            var empid = document.getElementById('Slect_EmpName').value;

            if (empid == "" || empid == "Select Employee") {
                alert("Please Select Employee Name");
                return false;
            }
            var date = document.getElementById('txtFromDate').value;
            var data = { 'operation': 'Save_Dispatch_Assign_Details', 'empid': empid, 'btnVal': btnVal, 'date': date, 'Dispatcharr': Dispatcharr };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    RefreshClick1();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            CallHandlerUsingJson(data, s, e);
        }

        function RefreshClick1() {
            //           $('.checkinput').each(function () {
            $('.chkclass').each(function () {
                $(this).attr("checked", false);
            });
//            document.getElementById('divchkdispatch').innerHTML = "";
            document.getElementById('btn_save').innerHTML = "Save";
            document.getElementById('Slect_EmpName').selectedIndex = ""
        }


        function fillemployees1(msg) {
            document.getElementById('ddlindentEmployee').options.length = "";

            var veh = document.getElementById('ddlindentEmployee');
            var length = veh.options.length;
            for (i = length - 1; i >= 0; i--) {
                veh.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Employee";
            opt.value = "";
            veh.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].EmpName;
                    opt.value = msg[i].sno;
                    veh.appendChild(opt);
                }
            }
        }
        var dispatchdta1;
        function BranchNamewisebankname1(msg) {
            document.getElementById('div_chkRouteName').innerHTML = "";
            document.getElementById('divchkdispatch').innerHTML = "";
            dispatchdta1 = msg;
            var branch = [];
            var selectedbranch = "";
            for (var i = 0; i < dispatchdta1.length; i++) {
                if (typeof dispatchdta1[i] === "undefined" || dispatchdta1[i].Branchname == "" || dispatchdta1[i].Branchname == null) {
                }
                else {
                    var tbranchname = dispatchdta1[i].Branchname;
                    var tbranchid = dispatchdta1[i].Branchname;
                    if (selectedbranch != "Select All") {
                        //                        if (tbranchid == selectedbranch) {
                        tbranchname = tbranchname.replace(/[^a-zA-Z0-9]/g, '');
                        var exists = branch.indexOf(tbranchname);
                        if (exists == -1) {
                            var branchname = dispatchdta1[i].Branchname;
                            branchname = branchname.replace(/[^a-zA-Z0-9]/g, '');
                            branch.push(branchname);
                            $("#div_chkRouteName").append("<div id='div" + branchname + "' class='divcategory1'>");
                        }
                        //                        }
                    }
                    else {
                        tbranchname = tbranchname.replace(/[^a-zA-Z0-9]/g, '');
                        var exists = branch.indexOf(tbranchname);
                        if (exists == -1) {
                            var branchname = dispatchdta1[i].Branchname;
                            branchname = branchname.replace(/[^a-zA-Z0-9]/g, '');
                            branch.push(branchname);
                            $("#div_chkRouteName").append("<div id='div" + branchname + "' class='divcategory1'>");
                        }
                    }
                }
            }
            for (var p = 0; p < branch.length; p++) {
                $("#div" + branch[p] + "").append("<div class='titledivcls1'><table id='banktable1' style='width:100%;'><tr><td style='width: 120px;'><h2 class='unitline1'>" + branch[p] + "</h2></td><td></td><td style='padding-right: 20px;vertical-align: middle;'><span class='iconminus' title='Hide' onclick='minusclick(this);'></span></td></tr></table></div>");
                $("#div" + branch[p] + "").append("<ul style='padding-top: 9% !important;' id='ul" + branch[p] + "' class='ulclass1' >");
                for (var i = 0; i < dispatchdta1.length; i++) {
                    var tbranchname = dispatchdta1[i].Branchname;
                    tbranchname = tbranchname.replace(/[^a-zA-Z0-9]/g, '');
                    if (typeof dispatchdta1[i] === "undefined" || dispatchdta1[i].RouteName == "" || dispatchdta1[i].RouteName == null) {
                    }
                    else {
                        if (branch[p] == tbranchname) {
                            var label = document.createElement("span");
                            var hidden = document.createElement("input");
                            hidden.type = "hidden";
                            hidden.Bankname = "hidden";
                            hidden.value = dispatchdta1[i].Route_id;
                            var checkbox = document.createElement("input");
                            checkbox.type = "checkbox";
                            checkbox.Bankname = "checkbox";
                            checkbox.value = dispatchdta1[i].Route_id;
                            checkbox.id = "checkbox";
                            checkbox.className = 'chkclass';
                            document.getElementById('ul' + branch[p]).appendChild(checkbox);
                            label.innerHTML = dispatchdta1[i].RouteName;
                            document.getElementById('ul' + branch[p]).appendChild(label);
                            document.getElementById('ul' + branch[p]).appendChild(hidden);
                            document.getElementById('ul' + branch[p]).appendChild(document.createElement("br"));
                        }
                    }
                }
                //                checkbox.onclick = TabclassClick();
            }
            TabclassClick();
        }
        function TabclassClick() {
            $("input[type='checkbox']").click(function () {
                if ($(this).is(":checked")) {
                    //                   var Selected = $(this).next(".livalue");
                    //                   var Selectedid = $(this).val();
                    var Selected = $(this).next().text();
                    var Selectedid = $(this).next().next().val();
                    var label = document.createElement("div");
                    var Crosslabel = document.createElement("img");
                    Crosslabel.style.float = "right";
                    Crosslabel.src = "Images/Cross.png";
                    Crosslabel.onclick = function () { RemoveClick(Selectedid); };
                    label.id = Selectedid;
                    //label.innerHTML = Selected.text();
                    label.innerHTML = Selected;
                    label.className = 'divselectedclass';
                    label.onclick = function () { divonclick(label); }
                    document.getElementById('divselectedroutenames').appendChild(label);
                    label.appendChild(Crosslabel);
                    var uncheckarrylen = div_uncheck_Array.length;
                    var a = div_uncheck_Array.indexOf(Selectedid);
                    if (a == -1) {

                    }
                    else {
                        div_uncheck_Array.splice(a, 1);
                    }
                    //div_uncheck_Array.pop(Selectedid);

                    var uncheckarraylength = div_uncheck_Array.length;
                    if (uncheckarrylen == uncheckarraylength) {
                        div_check_Array.push(Selectedid);

                    }
                }
                else {
                    //var Selected = $(this).val();
                    var Selected = $(this).next().next().val();
                    var elem = document.getElementById(Selected);
                    var p = elem.parentNode;
                    p.removeChild(elem);
                    div_uncheck_Array.push(Selected);
                }
            });
        }

        function save_IndentAssignDetails() {
            var div = document.getElementById('divselectedroutenames');
            var divs = div.getElementsByTagName('div');
            var Dispatcharr = [];
            for (var i = 0; i < divs.length; i += 1) {
                Dispatcharr.push(divs[i].id);
            }
            if (Dispatcharr == 0) {
                alert("You must select at least one checkbox!");
                return false;
            }
            var btnVal = document.getElementById('btnSave').innerHTML;
            var empid = document.getElementById('ddlindentEmployee').value;
            if (empid == "" || empid == "Select Employee") {
                alert("Please Select Employee Name");
                return false;
            }
            var data = { 'operation': 'Svae_Indent_Assign_Details', 'empid': empid, 'btnVal': btnVal, 'dataarr': Dispatcharr };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    RefreshClick();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            CallHandlerUsingJson(data, s, e);
        }
        var div_uncheck_Array = [];
        function RemoveClick(Selected) {
            var elem = document.getElementById(Selected);
            var p = elem.parentNode;
            p.removeChild(elem);
            $('.chkclass').each(function () {
                if ($(this).next().next().val() == Selected) {
                    $(this).attr("checked", false);
                    $(this).attr("disabled", false);
                }
            });
            div_uncheck_Array.push(Selected);
        }

        function RefreshClick() {
            //           $('.checkinput').each(function () {
            $('.chkclass').each(function () {
                $(this).attr("checked", false);
            });
            document.getElementById('divselectedroutenames').innerHTML = "";
            document.getElementById('btnSave').innerHTML = "Save";
            document.getElementById('ddlindentEmployee').selectedIndex=""
            div_uncheck_Array = [];
            div_check_Array = [];
        }

        function showTripAssign() {
            
            filldispatchname();
            get_Indent_Employees();
            $("#div_TripAssign").css("display", "block");
            $("#div_TripReAssign").css("display", "none");
            $("#div_TripEnd").css("display", "none");
            $("#div_IndentAssign").css("display", "none");
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#txtFromDate').val(today);
        }
        function showTripReAssign() {
            $("#div_TripAssign").css("display", "none");
            $("#div_TripReAssign").css("display", "block");
            $("#div_TripEnd").css("display", "none");
            $("#div_IndentAssign").css("display", "none");
        }
        function showTripEnd() {
            GetEndDispatches();
            $("#div_TripEnd").css("display", "block");
            $("#div_TripAssign").css("display", "none");
            $("#div_TripReAssign").css("display", "none");
            $("#div_IndentAssign").css("display", "none");
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#datepicker').val(today);
        }
        function showIndentAssign() {
            filldispatchname1();
            get_Indent_Employees1();
            $("#div_IndentAssign").css("display", "block");
            $("#div_TripAssign").css("display", "none");
            $("#div_TripReAssign").css("display", "none");
            $("#div_TripEnd").css("display", "none");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600">
    </asp:ToolkitScriptManager>
    <div>
        <asp:UpdateProgress ID="updateProgress1" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0;
                    right: 0; left: 0; z-index: 9999; background-color: #FFFFFF; opacity: 0.7;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="thumbnails/loading.gif"
                        Style="padding: 10px; position: absolute; top: 40%; left: 40%; z-index: 99999;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <section class="content-header">
        <h1>
            Trip Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Trip Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
        <div>
                    <ul class="nav nav-tabs">
                        <li id="liDepartment" class=""><a data-toggle="tab" href="#" onclick="showTripAssign()">
                            <i class="fa fa-street-view"></i>&nbsp;&nbsp;DispatchAssign</a></li>
                        <li id="liAddress" class=""><a data-toggle="tab" href="#" onclick="showTripReAssign()">
                            <i class="fa fa-file-text"></i>&nbsp;&nbsp;TripReAssign</a></li>
                        <li id="liProductRanking" class=""><a data-toggle="tab" href="#" onclick="showTripEnd()">
                            <i class="fa fa-file-text"></i>&nbsp;&nbsp;TripEnd</a></li>
                            <li id="li1" class=""><a data-toggle="tab" href="#" onclick="showIndentAssign()">
                            <i class="fa fa-file-text"></i>&nbsp;&nbsp;IndentAssign</a></li>
                    </ul>
                </div>
        <div id="div_TripEnd" style="display:none">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Trip End Details
                </h3>
            </div>
            <div class="box-body">
                <div style="width: 100%; background-color: #fff">
                    <div style="width: 70%;">
                        <div id="DivReturn" style="font-size: 18px;">
                            <table>
                                <tr>
                                    <td nowrap>
                                        <span id="txtRoute" style="color: Red; font-size: 18px; font-weight: bold;"></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table>
                            <tr>
                                <td>
                                    <label>
                                        Date:</label>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <input type="date" id="datepicker" class="form-control" />
                                </td>
                                <td style="width:5px;">
                                </td>
                                <td>
                                    <input type="button" id="Button2" value="Get Details" class="btn btn-primary" onclick="btn_getdetails_click();" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <table style="width:100%;">
                <tr>
                <td style="width:75%;">
                  <div id="divfilltirp_end_details" style="height:500px;overflow:auto;">
                </div>
                </td>
                 <td style="float:left;" >
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
                                <span id="Span18" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>2000</b></span>
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
                                <span id="txtsno" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>500</b></span>
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
                                <span id="Span16" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>200</b></span>
                            </td>
                            <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                text-align: center; padding: 0px 0px 0px 3px">
                                <b style="font-size: 11px; font-weight: bold;">X</b>
                                <input type="number" id="Number8" onkeyup="CountChange(this);" class="qtyclass"
                                    onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                    border-radius: 6px 6px 6px 6px;" placeholder="Enter Count" />
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
                                <span id="Span1" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>100</b></span>
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
                                <span id="Span4" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>50</b></span>
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
                                <span id="Span6" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>20</b></span>
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
                                <span id="Span8" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>10</b></span>
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
                                <span id="Span10" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>5</b></span>
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
                                <span id="Span12" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>2</b></span>
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
                                <span id="Span14" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>1</b></span>
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
              </div>
              </div>
              </div>
                <div id="div_TripAssign">
                <div class="box box-info" style="float: left; width: 100%; height: 100%;padding-top:3%;padding:2%">
                 <div style="width: 24%; float: left; height: 100%;">
                            <div  style="float: left; width: 240px; height: 330px; overflow: auto;" >
                                <div class="box-header with-border">
                                    <h3 class="box-title">
                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Despatch Names
                                    </h3>
                                </div>
                                <div class="box-body"  style="height:100%;">
                                    <div id="divchkdispatch">
                                    </div>
                                </div>
                            </div>
                        </div>
                      <table >

                       <tr>
                                        <td>
                                            <label id="lblEmployeeName">
                                                Employee Name:</label><span style="color: red; font-weight: bold">*</span>
                                        </td>
                                        <td style="height: 40px;">
                                            <select id="Slect_EmpName" class="form-control">
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label id="lbldeptflag">
                                                Date:</label><span style="color: red; font-weight: bold"></span>
                                        </td>
                                        <td style="height: 40px;">
                                            <input type="date" id="txtFromDate" class="form-control" />
                                        </td>
                                    </tr>
                            <tr>
                            <td>
                            </td>
                            <td>
                                <input type="button" id="btn_save" value="Save" class="btn btn-primary" onclick="save_DispatchAssignDetails();" />
                            </td>
                        </tr>
                    </table>
                </div>
                </div>
              <div id="div_TripReAssign" style="display:none">
                <div class="box box-info">
                 <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Trip ReAssign Details
                </h3>
            </div>
                  <div class="box-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblctxtdat" runat="server">Date:</asp:Label>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtdate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtdate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="width:5px;"  >
                                </td>
                               <%-- <td>
                                    <span>Select Type</span>
                                </td>
                                <td style="height:40px;">
                                    <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control" AutoPostBack="True">
                                        <asp:ListItem>Assign</asp:ListItem>
                                        <asp:ListItem>Pending</asp:ListItem>
                                        <asp:ListItem>Verified</asp:ListItem>
                                        <asp:ListItem>Cancel</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5px;">
                                </td>--%>
                                <td>
                                    <asp:Button ID="btnGenerate" Text="Generate" runat="server" OnClientClick="OrderValidate();"
                                        CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                                </td>
                            </tr>
                        </table>
                        <div id="divPrint">
                            <div align="center">
                                <div style="width: 100%;">
                                    <div style="width: 11%; float: left;">
                                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="95px" height="90px" />
                                    </div>
                                    <div style="left: 0%; text-align: center;">
                                        <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="26px" ForeColor="#0252aa"
                                            Text=""></asp:Label>
                                        <br />
                                    </div>
                                    <div align="center">
                                    </div>
                                    <div style="width: 100%;">
                                    </div>
                                </div>
                                <asp:GridView ID="grdReports" runat="server" CssClass="Grid" HeaderStyle-BackColor="#61A6F8"
                                    HeaderStyle-Font-Bold="true" HeaderStyle-ForeColor="White" ForeColor="White"
                                    Width="100%" GridLines="Both" Font-Bold="true" AutoGenerateEditButton="True" AutoGenerateColumns="false"
                                    OnRowEditing="gvDetails_RowEditing" OnRowCancelingEdit="gvDetails_RowCancelingEdit"
                                    OnRowCommand="GridViews_RowCommand" OnRowUpdating="gvDetails_RowUpdating">
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                    <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                        Font-Names="Raavi" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                                    <AlternatingRowStyle HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333"/>
                                    <Columns>
                                       <%-- <asp:TemplateField SortExpression="SendDate">
                                            <ItemTemplate>
                                                <asp:Button ID="btnPost" CssClass="btn btn-primary" Text="Update" runat="server" CommandName="Comment"
                                                    CommandArgument='<%#Eval("Sno") + ";" +Eval("Status")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                         <asp:TemplateField HeaderText="UserName" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtUserName" runat="server" Text='<%# Eval("UserName") %>' Width="140"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DispName" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="lblDispName" runat="server" Text='<%# Eval("DispName") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDispName" runat="server" Text='<%# Eval("DispName") %>' Width="140"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Sno" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="lblSno" runat="server" Text='<%# Eval("Sno") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSno" runat="server" Text='<%# Eval("Sno") %>' Width="140"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Status" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtStatus" runat="server" Text='<%# Eval("Status") %>' Width="140"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Canceldate" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="lblCanceldate" runat="server" Text='<%# Eval("Canceldate") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCanceldate" runat="server" Text='<%# Eval("Canceldate") %>' Width="140"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CollectedAmount" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="lblCollectedAmount" runat="server" Text='<%# Eval("CollectedAmount") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCollectedAmount" runat="server" Text='<%# Eval("CollectedAmount") %>' Width="140"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SubmittedAmount" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="lblSubmittedAmount" runat="server" Text='<%# Eval("SubmittedAmount") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSubmittedAmount" runat="server" Text='<%# Eval("SubmittedAmount") %>' Width="140"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ReceivedAmount" ItemStyle-Width="150">
                    <ItemTemplate>
                        <asp:Label ID="lblReceivedAmount" runat="server" Text='<%# Eval("ReceivedAmount") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtReceivedAmount" runat="server" Text='<%# Eval("ReceivedAmount") %>' Width="140"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                                

                            </div>
                            <br />
                            <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
                </div>
                </div>
                </div>
       <div id="div_IndentAssign" style="display:none;">
       
                <div class="box box-info" style="float: left; width: 100%; height: 100%;padding-top:3%;padding:2% !important;">
                <%-- <div style="width: 24%;  height: 100%;">
                            <div  style="float: left; width: 240px; height: 330px; overflow: auto;" >
                                <div class="box-header with-border">
                                    <h3 class="box-title">
                                        <i style="padding-right: 5px;" class="fa fa-cog"></i>RouteName 
                                    </h3>
                                </div>
                                <div class="box-body"  style="height:100%;">
                                    <div id="div_chkRouteName">
                                    </div>
                                </div>
                                 </div>
                             <div class="box box-info" style="float: right; width: 240px; height: 330px; overflow: auto;">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Selected Route Details
                                        </h3>
                                    </div>
                                    <div class="box-body">
                                <div id="divselectedroutenames" >
                                </div>
                                </div>
                                </div>
                        </div>
                     <table>
                        <tr>
                            <td>
                                <span id="Span20" style="padding-left:41px">Employee Name</span>
                            </td>
                            &nbsp&nbsp&nbsp&nbsp&nbsp
                            <td>
                                <select id="ddlindentEmployee" class="form-control">
                                </select>
                            </td>
                            <td>
                            </td>
                            <td>
                                <input type="button" id="Button1" value="Save" class="btn btn-primary" onclick="save_IndentAssignDetails();" />
                            </td>
                        </tr>
                    </table>--%>
                                    <div >
                                        <table>
                        <tr>
                            <td>
                                <span id="Span20" style="padding-left:41px">Employee Name</span>
                            </td>
                            &nbsp&nbsp&nbsp&nbsp&nbsp
                            <td>
                                <select id="ddlindentEmployee" class="form-control">
                                </select>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    </div>
                    </div>
                    <div style="padding:2% !important;">
                     <table style="width: 100%;">
                        <tr>
                            <td>
                              <div class="box box-info" style="float: left; width: 350px; height: 330px; overflow: auto;">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Route Details
                                        </h3>
                                    </div>
                                    <div class="box-body">
                                <div id="div_chkRouteName" >
                                </div>
                                </div>
                                </div>
                            </td>
                            <td>
                              <div class="box box-info" style="float: left; width: 240px; height: 330px; overflow: auto;">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Selected Route Details
                                        </h3>
                                    </div>
                                    <div class="box-body">
                                <div id="divselectedroutenames" >
                                </div>
                                </div>
                                </div>
                            </td>
                                </tr>
                                </table>
                                </div>
                                 <div style="text-align: -webkit-center;padding-bottom: 2%;">
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <div class="input-group">
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-ok" id="btnSave1" onclick="save_IndentAssignDetails()">
                                                    </span><span id="btnSave" onclick="save_IndentAssignDetails()">Save</span>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                </div>
                                </div>
                              
    </section>
</asp:Content>
