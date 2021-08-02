<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="insentivemaster.aspx.cs" Inherits="insentivemaster" %>

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
    </script>
    <script type="text/javascript">
        var LevelType = "";
        $(function () {
            FillApprovalEmploye();
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
                    GetBranchProducts();
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
                    BindAgentName(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindAgentName(msg) {
            document.getElementById('ddlAgentName').options.length = "";
            var ddlAgentName = document.getElementById('ddlAgentName');
            var length = ddlAgentName.options.length;
            for (i = length - 1; i >= 0; i--) {
                ddlAgentName.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Agent Name";
            opt.value = "";
            ddlAgentName.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlAgentName.appendChild(opt);
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
                    BindHeads(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }



        var HeadNames = []; var hedadlist = [];
        function BindHeads(msg) {
            hedadlist = msg;
            for (var i = 0; i < msg.length; i++) {
                var HeadName = msg[i].ProdName;
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
                if (HeadName == hedadlist[i].ProdName) {
                    document.getElementById('hidden_headid').value = hedadlist[i].ProdID;
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
           
        }
        
        
        function BtnRaiseVoucherClick() {
            var inserntivetype = document.getElementById("ddlinserntivetype").value;
            var SalesOfficeid = document.getElementById("ddlSalesOffice").value;
            var Routeid = document.getElementById("ddlRouteName").value;
            var agentid = document.getElementById("ddlAgentName").value;
            var structureexistornot = document.getElementById("ddlstructureexist").value;
            var frmdate = document.getElementById("txtfrmdate").value;
            var todate = document.getElementById("txttodate").value;
            var Remarks = document.getElementById("txtRemarks").value;
            var ddlEmpApprove = document.getElementById("ddlEmpApprove").value;
            var btnSave = document.getElementById("btnSave").value;
//            var spnVoucherID = document.getElementById("spnVoucherID").innerHTML;
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
            var data = { 'operation': 'btnsave_incentivemasterdetails', 'Cashdetails': Cashdetails, 'inserntivetype': inserntivetype, 'SalesOfficeid': SalesOfficeid,
                'Routeid': Routeid, 'agentid': agentid, 'structureexistornot': structureexistornot, 'frmdate': frmdate, 'todate': todate, 'btnSave': btnSave, 'Remarks': Remarks,
                'EmpApprove': ddlEmpApprove
            };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    document.getElementById("ddlinserntivetype").selectedIndex = 0;
                    document.getElementById("ddlSalesOffice").selectedIndex = 0;
                    document.getElementById("ddlRouteName").selectedIndex = 0;
                    document.getElementById("ddlAgentName").selectedIndex = 0;
                    document.getElementById("ddlstructureexist").selectedIndex = 0;
                    document.getElementById("txtfrmdate").value = "";
                    document.getElementById("txttodate").value = "";
                    document.getElementById("txtRemarks").value = "";
                    document.getElementById("ddlEmpApprove").selectedIndex = 0;
                    Cashform = [];
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

        function Getinsentivrdetails() {
            var data = { 'operation': 'Getinsentivrdetails' };
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

            results += '<thead><tr style="background-color: #7ac8f5;color: black;font-size: 13px;text-align:center;"><th scope="col" style="text-align:center;font-weight: 600;">Incentive Type</th><th scope="col" style="text-align:center;font-weight: 600;">Branch Name</th><th scope="col" style="text-align:center;font-weight: 600;">Route Name</th><th scope="col" style="text-align:center;font-weight: 600;">Agent Name</th><th scope="col" style="text-align:center;font-weight: 600;">From Date</th><th scope="col" style="text-align:center;font-weight: 600;">To Date</th><th scope="col" style="text-align:center;font-weight: 600;">Status</th><th scope="col" style="text-align:center;font-weight: 600;">Remarks</th><th scope="col"></th><th scope="col"></th></tr></thead></tbody>';
            if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                for (var i = 0; i < msg.length; i++) {
                    results += '<tr>';
                    results += '<td scope="row" class="1" style="text-align:center;font-size: 13px;width: 51px;"  >' + msg[i].inserntivetype + '</td>';
                    results += '<td scope="row" class="50" style="text-align:center; display:none;"  >' + msg[i].SalesOfficeid + '</td>';
                    results += '<td scope="row" class="2"  style="text-align:center;font-size: 13px;width: 60px;">' + msg[i].SalesOfficeName + '</td>';
                    results += '<td scope="row" class="8"  style="display:none;">' + msg[i].Routeid + '</td>';
                    results += '<td data-title="brandstatus"  style="text-align:center;font-size: 13px;width: 150px;" class="9" >' + msg[i].Routename + '</td>';
                    results += '<td scope="row" class="3"  style="display:none;">' + msg[i].agentid + '</td>';
                    results += '<td data-title="brandstatus"  style="text-align:center;font-size: 13px;width: 69px;" class="4">' + msg[i].Agentname + '</td>';
                    results += '<td data-title="brandstatus"   style="text-align:center;" class="5" >' + msg[i].frmdate + '</td>';
                    results += '<td data-title="brandstatus"   style="text-align:center;" class="5" >' + msg[i].todate + '</td>';
                    results += '<td data-title="brandstatus"  style="text-align:center;font-size: 13px;width: 54px;" class="6"  >' + msg[i].status + '</td>';
                    results += '<td data-title="brandstatus"  style="display:none;" style="text-align:center;" class="10">' + msg[i].incentivesno + '</td>';
                    results += '<td data-title="brandstatus"  style="text-align:center;font-size: 13px;" class="12">' + msg[i].Remarks + '</td>';
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
                    results += '<td scope="row" class="1" style="text-align:center;font-size: 13px;width: 51px;"  >' + msg[i].inserntivetype + '</td>';
                    results += '<td scope="row" class="50" style="text-align:center; display:none;"  >' + msg[i].SalesOfficeid + '</td>';
                    results += '<td scope="row" class="2"  style="text-align:center;font-size: 13px;width: 60px;">' + msg[i].SalesOfficeName + '</td>';
                    results += '<td scope="row" class="8"  style="display:none;">' + msg[i].Routeid + '</td>';
                    results += '<td data-title="brandstatus"  style="text-align:center;font-size: 13px;width: 150px;" class="9" >' + msg[i].Routename + '</td>';
                    results += '<td scope="row" class="3"  style="display:none;">' + msg[i].agentid + '</td>';
                    results += '<td data-title="brandstatus"  style="text-align:center;font-size: 13px;width: 69px;" class="4">' + msg[i].Agentname + '</td>';
                    results += '<td data-title="brandstatus"   style="text-align:center;" class="5" >' + msg[i].frmdate + '</td>';
                    results += '<td data-title="brandstatus"   style="text-align:center;" class="5" >' + msg[i].todate + '</td>';
                    results += '<td data-title="brandstatus"  style="text-align:center;font-size: 13px;width: 54px;" class="6"  >' + msg[i].status + '</td>';
                    results += '<td data-title="brandstatus"  style="display:none;" style="text-align:center;" class="10">' + msg[i].incentivesno + '</td>';
                    results += '<td data-title="brandstatus"  style="text-align:center;font-size: 13px;" class="12">' + msg[i].Remarks + '</td>';
                    results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="geteditdetails(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>';
                    results += '<td data-title="brandstatus"><button type="button" title="Click Here To Print!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 prntcls"  onclick="printvoucher(this)"><span class="glyphicon glyphicon-print" style="top: 0px !important;"></span></button></td>';
                    results += '<td data-title="brandstatus"><button type="button" title="Click Here To Cancel!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 removeclass"   onclick="cancelvoucher(this)"><span class="glyphicon glyphicon-remove-circle" style="top: 0px !important;"></span></button></td></tr>';
                }
                results += '</table></div>';
            }
            $("#divincentivedata").html(results);
        }

        function geteditdetails(thisid) {
            alert("edit operation only autharised persons");
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
                results += '<td  class="6"> <img src="Images/Odclose.png" onclick="RowDeletingClick(this);" style="cursor:pointer;" width="30px" height="30px" alt="Edit" title="Click here to remove Head Of Account."/> </td></tr>';
            }
            results += '</table></div>';
            $("#divHeadAcount").html(results);
        }
        
       
        var serial = 0;
        function Binddata(msg) {
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
                    if (Amount > 10000) {
                        alert("Enter the Amount Below Ten Thousend Rupes (OR) Either If You Are Send the money to bank Or branch Please Select Branch OR Bank");
                        document.getElementById("txtCashAmount").value = "";
                        document.getElementById("slctdebittype").focus();
                        return false;

                    }
                }
            }
        }



        var Cashform = [];
        function BtnCashToAddClick() {
            $("#divHeadAcount").css('display', 'block');
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
                    alert("Item Already Added");
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

            Cashform.push({ HeadSno: HeadSno, HeadOfAccount: HeadOfAccount, Amount: Amount });
            
            var results = '<div  style="overflow:auto;"><table id="tableCashFormdetails" class="table table-bordered" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col" style="text-align: center;">Head Of Account</th><th scope="col" style="text-align: center;">Amount</th></tr></thead></tbody>';
            for (var i = 0; i < Cashform.length; i++) {
                results += '<tr>';
                results += '<td scope="row" class="1" style="text-align:center;"><span id="txtAccount" class="AccountClass" ><b style="font-weight: 400;">' + Cashform[i].HeadOfAccount + '</b></span></td>';
                results += '<td class="2" style="text-align:center;"><span id="txtamount" class="AmountClass" ><b style="font-weight: 400; ">' + Cashform[i].Amount + '</b></span><img src="Images/Odclose.png" onclick="RowDeletingClick(this);" style="cursor:pointer;" width="30px" height="30px" alt="Edit" title="Click here to remove Head Of Account."/></td>';
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
                results += '<td  class="6"> <img src="Images/Odclose.png" onclick="RowDeletingClick(this);" style="cursor:pointer;" width="30px" height="30px" alt="Edit" title="Click here to remove Head Of Account."/> </td></tr>';
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
            var VoucherType = document.getElementById("ddlVoucherType").value;
            var LevelType = '<%=Session["LevelType"] %>';
            if (VoucherType == "Debit") {
                if (LevelType == "AccountsOfficer" || LevelType == "Director") {
                }
                else {
                    $('.divAprovalEmp').css('display', 'table-row');
                }
                $('.divType').css('display', 'none');
                $('#spnsalary').css('display', 'none');
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
                $('.transactionclass').css('display', 'none');

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
                if (desc == employeesname[i].employee_num) {
                    document.getElementById('txthiddenempid').value = employeesname[i].empid;
                    document.getElementById('txtHiddenEName').value = employeesname[i].employee_num;
                    get_Employee_Salary_Details();
                }
            }
        }
        var employeesname = [];
        function get_Hrms_Employee_Details() {
            var data = { 'operation': 'get_Hrms_Employee_Details' };
            var s = function (msg) {
                if (msg) {
                    employeesname = msg;
                    var availableTags = [];
                    for (i = 0; i < msg.length; i++) {
                        availableTags.push(msg[i].employee_num);
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
                        change: changeemployeeid,
                        autoFocus: true
                    });
                }
            }
            var e = function (x, h, e) {
                alert(e.toString());
            };
            callHandler(data, s, e);
        }

        function ddltransactiontypechanged() {
            var employeetype = document.getElementById("ddltransactiontype").value;
            if (employeetype == "Employee") {
                get_Hrms_Employee_Details();


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

        function ddlstructureexistchanged() {
            var structureexist = document.getElementById("ddlstructureexist").value;
            if (structureexist == "New") {
                $('#trstructre').css('display', 'none');
                $('#tritem').css('display', 'table-row');
            //trstructre tritem
            }
            else {
                $('#tritem').css('display', 'none');
                $('#trstructre').css('display', 'table-row');
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Insentive Details<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Insentive Details</a></li>
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
                            <tr>
                                <td>
                                    Insentive Type
                                </td>
                                <td style="height: 40px;">
                                    <select id="ddlinserntivetype" class="form-control" onchange="ddlVoucherTypeChange();">
                                        <option value="Normal Incentive">Normal Incentive</option>
                                        <option value="Leakage Incentive">Leakage Incentive</option>
                                    </select>
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    Sales Office
                                </td>
                                <td style="height: 40px;">
                                    <select id="ddlSalesOffice" class="form-control" onchange="ddlSalesOfficeChange(this);">
                                        </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Route Name
                                </td>
                                <td style="height: 40px;">
                                     <select id="ddlRouteName" class="form-control" onchange="ddlRouteNameChange(this);">
                                        </select>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    Agent Name
                                </td>
                                <td style="height: 40px;">
                                    <select id="ddlAgentName" class="form-control" onchange="ddlAgentNameChange(this);">
                                    </select>
                                </td>
                            </tr>


                            
                            <tr>
                                <td>
                                    Structure Exit (or) Not
                                </td>
                                <td style="height: 40px;">
                                    <select id="ddlstructureexist" class="form-control" onchange="ddlstructureexistchanged();">
                                        <option>Select</option>
                                        <option>New</option>
                                        <option>Exist</option>
                                    </select>
                                </td>
                            </tr>
                             <tr id="trstructre" style="display:none;">
                                <td>
                                    Structure Type
                                </td>
                                <td style="height: 40px;">
                                    <select id="ddlstructuretype" class="form-control" onchange="ddlstructuretypechanged();">
                                       
                                    </select>
                                </td>
                            </tr>
                            <tr id="tritem" style="display:none;">
                                <td>
                                    Item Name
                                </td>
                                <td style="height: 40px;">
                                    <input type="text" id="combobox" class="form-control" maxlength="45" placeholder="Select Item Name" />
                                </td>
                                <%--<select id="combobox" class="form-control">
                                    </select>--%>
                                <td>
                                    <input id="hidden_headid" type="hidden" class="form-control" name="hiddenheadid" />
                                </td>
                                <td style="width: 6px;">
                                </td>
                                <td>
                                    <input type="number" id="txtCashAmount" class="form-control" placeholder="Enter Insentive Amount"
                                        onkeyup="cashamountchange(this);" />
                                </td>
                                <td>
                                    <%--<input type="button" id="Button3" value="Add" onclick="BtnCashToAddClick();" class="btn btn-primary" />--%>
                                    <button type="button" title="Click Here To Add!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 prntcls"
                                        onclick="BtnCashToAddClick();">
                                        <span class="glyphicon glyphicon-plus" style="top: 0px !important;"></span>
                                    </button>
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
                                    Leakages
                                </td>
                                <td style="height: 40px;">
                                    <span id="txtAMount" class="form-control"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    From Date
                                </td>
                                <td style="height: 40px;">
                                     <input type="date" id="txtfrmdate" placeholder="DD-MM-YYYY" class="form-control" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    To Date
                                </td>
                                <td style="height: 40px;">
                                     <input type="date" id="txttodate" placeholder="DD-MM-YYYY" class="form-control" />
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
                            <tr class="divAprovalEmp">
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
                                    <input type="button" id="btnSave" value="Save" onclick="BtnRaiseVoucherClick();"
                                        class="btn btn-primary" />
                                </td>
                                <td style="width: 3%;">
                                </td>
                                <td style="height: 40px;">
                                    <input type="button" id="btnClear" value="Close" onclick="BtnclearClick();" class="btn btn-danger" />
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
                <div id="div2" style="border: 2px solid #A0A0A0; position: absolute; top: 8%; background-color: White;
                    right: 25%; width: 50%; -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                    -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                    border-radius: 10px 10px 10px 10px; padding: 2%">
                    <table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;"
                        id="tableCollectionDetailss" class="mainText2" border="1">
                        <tr>
                            <td>
                                <label>
                                    Name</label>
                            </td>
                            <td style="height: 40px;">
                                <span id="spnNames" style="font-size: 20px; font-weight: 900; color: #00a65a;"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Voucher ID</label>
                            </td>
                            <td style="height: 40px;">
                                <span id="spnVoucherIDs" class="form-control"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    VoucherType</label>
                            </td>
                            <td style="height: 40px;">
                                <span id="spnVoucherTypes" style="font-size: 20px; font-weight: 900; color: #00a65a;">
                                </span>
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
                                <label>
                                    Amount</label>
                            </td>
                            <td style="height: 40px;">
                                <span id="spnAmounts" style="font-size: 20px; font-weight: 900; color: #3c8dbc;">
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Approval By</label>
                            </td>
                            <td style="height: 40px;">
                                <span id="spnApprovalEmps" style="font-size: 20px; font-weight: 900; color: #00a65a;">
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Remarks</label>
                            </td>
                            <td style="height: 40px;">
                                <textarea rows="3" cols="45" id="txtCashierRemarkss" class="form-control" placeholder="Enter Remarks"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Approval Amount
                                </label>
                            </td>
                            <td style="height: 40px;">
                                <input type="number" id="txtApprovalamts" class="form-control" value="" onkeypress="return numberOnlyExample();"
                                    placeholder="Enter Approval Amount" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Approval Remarks</label>
                            </td>
                            <td style="height: 40px;">
                                <input type="text" id="txtRemarkss" class="form-control" value="" onkeypress="return numberOnlyExample();"
                                    placeholder="Enter Remarks" />
                            </td>
                        </tr>
                    </table>
                    <table align="center" style="height: 40px;">
                        <tr>
                            <td style="height: 40px;">
                                <input type="button" value="Update" id="Button2" onclick="btnVoucherUpdateClicks();"
                                    class="btn btn-primary" />
                            </td>
                            <td style="width: 3%;">
                            </td>
                            <td style="height: 40px;">
                                <input type="button" id="Button5" value="Approve" onclick="btnApproveVoucherclicks();"
                                    class="btn btn-success" />
                            </td>
                            <td style="width: 3%;">
                            </td>
                            <td style="height: 40px;">
                                <input type="button" id="Button6" value="Reject" onclick="btnRejectVoucherclicks();"
                                    class="btn btn-danger" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="div4" style="width: 35px; top: 7.5%; right: 24%; position: absolute; z-index: 99999;
                    cursor: pointer;">
                    <img src="Images/close1.png" alt="close" onclick="OrdersCloseClicks();" style="width: 30px;
                        height: 25px;" />
                </div>
            </div>
        </div>
    </section>
</asp:Content>
