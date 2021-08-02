<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RouteWiseCosting.aspx.cs" Inherits="RouteWiseCosting" %>

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
    <script type="text/javascript">
        $(function () {
            get_Costing_details();
            FillSalesOffice();
        });
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
        $(function () {
            $('#btn_addCosting').click(function () {
                $('#div_Costing').hide();
                $('#div_Routewisecosting').show();
                $('#Costing_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
            });
            $('#btn_close').click(function () {
                $('#Costing_fillform').css('display', 'none');
                 $('#div_Costing').show();
                $('#showlogs').css('display', 'block');
                forclearall();
            });
             var today = new Date();
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!
                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd
                }
                if (mm < 10) {
                    mm = '0' + mm
                }
                var hrs = today.getHours();
                var mnts = today.getMinutes();
                $('#txtDate').val(yyyy + '-' + mm + '-' + dd);
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
            var ddlBranchName = document.getElementById('ddlBranchName');
            var length = ddlBranchName.options.length;
            ddlBranchName.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Branch Name";
            ddlBranchName.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlBranchName.appendChild(opt);
                }
            }
        }
        var routearray = [];
        function ddlSalesOfficeChange() {
            var Branchid = document.getElementById('ddlBranchName').value;
            var data = { 'operation': 'GetSalesRoutes', 'BranchID': Branchid };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    routearray = msg;
                    routenamefill();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var DataTable;
        var ProductTable = [];
        function routenamefill() {
            routearray;
//            DummyTable1;
            DataTable = [];
            var rows = $("#tabledetails tr:gt(0)");
            var rowsno = 1;
//            $(rows).each(function (i, obj) {
//                if ($(this).find('#ddlRouteName').val() != "") {
//                    txtsno = rowsno;
//                    routename = $(this).find('#ddlRouteName').val();
//                    saleincentive = $(this).find('#txtSaleIncentive').val();
//                    primarytransport = $(this).find('#txtPrimaryTransport').val();
//                    secondarytransport = $(this).find('#txtSecondaryTransport').val();
//                    crateloss = $(this).find('#txtCrateLoss').val();
//                    salary = $(this).find('#txtSalary').val();
//                    travelexpense = $(this).find('#txtTravelExpense').val();
//                    rent = $(this).find('#txtRent').val();
//                    baddebit = $(this).find('#txtBaddebit').val();
//                    officeexpense = $(this).find('#txtofficeexpensis').val();
//                    totalsellingcost = $(this).find('#TxtTSellingCost').val();
//                    Salevolume = $(this).find('#txtSVolume').val();
//                    timapact = $(this).find('#txtTImpact').text();
//                    hdnroutesno = $(this).find('#hdnroutesno').val();
//                    sisno = $(this).find('#subsno').val();
//                    DataTable.push({ Sno: txtsno, routename: routename, saleincentive: saleincentive, primarytransport: primarytransport, secondarytransport: secondarytransport, crateloss: crateloss, salary: salary, travelexpense: travelexpense, rent: rent, baddebit: baddebit, officeexpense: officeexpense, totalsellingcost: totalsellingcost, Salevolume: Salevolume, timapact: timapact, hdnroutesno: hdnroutesno, sisno: sisno });
//                    rowsno++;
//                }
//            });
            var txtsno = 0;
            var saleincentive = 0;
            var primarytransport = 0;
            var secondarytransport = 0;
            var crateloss = 0;
            var salary = 0;
            var travelexpense = 0;
            var rent = 0;
            var baddebit = 0;
            var officeexpense = 0;
            var totalsellingcost = 0;
            var Salevolume = 0;
            var timapact = 0;
            var sisno=0
            var Sno = parseInt(txtsno) + 1;
            for (var i = 0; i < routearray.length; i++) {
                routename = routearray[i].RouteName;
                hdnroutesno = routearray[i].rid;
                DataTable.push({ Sno: Sno, routename: routename, saleincentive: saleincentive, primarytransport: primarytransport, secondarytransport: secondarytransport, crateloss: crateloss, salary: salary, travelexpense: travelexpense, rent: rent, baddebit: baddebit, officeexpense: officeexpense, totalsellingcost: totalsellingcost, Salevolume: Salevolume, timapact: timapact, hdnroutesno: hdnroutesno, sisno: sisno });
                var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" ID="tabledetails">';
                results += '<thead><tr><th scope="col">Sno</th><th scope="col">RouteName</th><th scope="col">SaleIncentive</th><th scope="col">PrimaryTransport</th><th scope="col">SecondaryTransport</th><th scope="col">CrateLoss</th><th scope="col">Salary</th><th scope="col">TravelExpense</th><th scope="col">Rent</th><th scope="col">BadDebit</th><th scope="col">OfficeExpense</th><th scope="col">TotalSellingcost</th><th scope="col">SaleVolume</th><th scope="col">TotalImapact</th></tr></thead></tbody>';
                var j = 1;
                for (var i = 0; i < DataTable.length; i++) {
                    results += '<tr><td scope="row" class="1" style="text-align:center;" id="txtsno">' + j + '</td>';
                    results += '<td ><input id="ddlRouteName" readonly  class="Routecls" style="width:90px;" value="' + DataTable[i].routename + '"/></td>';
                    results += '<td style="display:none;" id="ddlRouteName" class="2">' + DataTable[i].routename + '</td>';
                    results += '<td ><input id="txtSaleIncentive" type="text" class="clsSaleIncentive" placeholder= "Enter SaleIncentive" onkeypress="return isFloat(event)" style="width:90px;" value="' + DataTable[i].saleincentive + '"/></td>';
                    results += '<td ><input id="txtPrimaryTransport" type="text" class="clsPrimaryTransport" placeholder= "Enter PrimaryTransport" onkeypress="return isFloat(event)" style="width:90px;" value="' + DataTable[i].primarytransport + '"/></td>';
                    results += '<td ><input id="txtSecondaryTransport" type="text" class="clsSecondaryTransport" placeholder= "Enter SecondaryTransport" onkeypress="return isFloat(event)" style="width:90px;" value="' + DataTable[i].secondarytransport + '"/></td>';
                    results += '<td ><input id="txtCrateLoss" type="text" class="clsCrateLoss" style="width:50px;" placeholder= "Enter CrateLoss" onkeypress="return isFloat(event)" value="' + DataTable[i].crateloss + '"/></td>';
                    results += '<td ><input id="txtSalary" type="text" class="clsSalary" style="width:50px;" placeholder= "Enter Salary" onkeypress="return isFloat(event)" value="' + DataTable[i].salary + '"/></td>';
                    results += '<td><input id="txtTravelExpense"  class="clsTravelExpense" style="width:90px; placeholder= "Enter TravelExpense" value="' + DataTable[i].travelexpense + '"/></td>';
                    results += '<td ><input id="txtRent" type="text" class="clsRent"  style="width:50px;" placeholder= "Enter Rent" onkeypress="return isFloat(event)" value="' + DataTable[i].rent + '"/></td>';
                    results += '<td ><input id="txtBaddebit" type="text" class="clsBaddebit" placeholder= "Enter Baddebit" style="width:90px;" value="' + DataTable[i].baddebit + '"/></td>';
                    results += '<td ><input id="txtofficeexpensis" type="text" class="clsofficeexpensis" placeholder= "Enter OfficeExpensis" style="width:50px;" onkeypress="return isFloat(event)" value="' + DataTable[i].officeexpense + '"/></td>';
                    results += '<td ><input id="TxtTSellingCost" type="text"  class="clsTSellingCost" placeholder= "Enter Total SellingCost" onkeypress="return isFloat(event)" style="width:90px;" value="' + DataTable[i].totalsellingcost + '"/></td>';
                    results += '<td ><input id="txtSVolume" type="text"  class="clsSVolume" placeholder= "Enter SaleVolume" onkeypress="return isFloat(event)" style="width:90px;" value="' + DataTable[i].Salevolume + '"/></td>';
                    results += '<td ><input id="txtTImpact" type="text"  class="clsTImpact" placeholder= "Enter Total Impact" onkeypress="return isFloat(event)" style="width:90px;" value="' + DataTable[i].timapact + '"/></td>';
                    results += '<td ><input id="hdnroutesno" type="hidden" value="' + DataTable[i].hdnroutesno + '"/>';
                    results += '<th data-title="From"><input  id="subsno" type="hidden" name="subsno" value="' + DataTable[i].sisno + '" style="width:90px;" font-size:12px;padding: 0px 5px;height:30px;"></input>';
                    results += '<td style="display:none" class="4">' + i + '</td></tr>';
                    j++;
                    //}
                }
                results += '</table></div>';
                $("#div_Routewisecosting").html(results);
            }
        }
        var Costing_array = [];
        function saveCostingDetails() {
            var Branchid = document.getElementById('ddlBranchName').value;
            if (Branchid == "") {
                alert("Enter BranchName");
                return false;
            }
            var date = document.getElementById('txtDate').value;
            var btnval = document.getElementById('btn_save').value;
            $('#tabledetails> tbody > tr').each(function () {
                var txtsno = $(this).find('#txtSno').text();
                var routename = $(this).find('#ddlRouteName').val();
                var saleincentive = $(this).find('#txtSaleIncentive').val();
                var primarytransport = $(this).find('#txtPrimaryTransport').val();
                var secondarytransport = $(this).find('#txtSecondaryTransport').val();
                var crateloss = $(this).find('#txtCrateLoss').val();
                var salary = $(this).find('#txtSalary').val();
                var travelexpense = $(this).find('#txtTravelExpense').val();
                var rent = $(this).find('#txtRent').val();
                var baddebit = $(this).find('#txtBaddebit').val();
                var officeexpense = $(this).find('#txtofficeexpensis').val();
                var totalsellingcost = $(this).find('#TxtTSellingCost').val();
                var Salevolume = $(this).find('#txtSVolume').val();
                var timapact = $(this).find('#txtTImpact').text();
                var Routeid = $(this).find('#hdnroutesno').val();
                var sno = $(this).find('#subsno').val();
                if (saleincentive == "" || saleincentive == "0") {
                }
                else {
                    Costing_array.push({ 'txtsno': txtsno, 'Routeid': Routeid, 'saleincentive': saleincentive, 'primarytransport': primarytransport, 'secondarytransport': secondarytransport, 'crateloss': crateloss, 'salary': salary, 'travelexpense': travelexpense, 'rent': rent, 'baddebit': baddebit, 'officeexpense': officeexpense, 'totalsellingcost': totalsellingcost, 'Salevolume': Salevolume, 'timapact': timapact,'sno': sno, });
                }
            });
            var Data = { 'operation': 'saveCostingDetails', 'date': date, 'Branchid': Branchid,  'btnVal': btnval, 'Costing_array': Costing_array };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                $('#div_Costing').show();
                $('#Costing_fillform').css('display', 'none');
                $('#showlogs').css('display', 'block');
                get_Costing_details();
               forclearall();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(Data, s, e);
        }
        function get_Costing_details() {
            var data = { 'operation': 'get_Costing_details'};
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillCosting(msg);
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
          var subcost = [];
        function fillCosting(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col"></th><th scope="col">BranchName</th><th scope="col">CostingDate</th></tr></thead></tbody>';
            var costdetails =msg[0].CostingDetails;
            subcost =msg[0].CostingSubDetails;
            var k = 1;
            for (var i = 0; i < costdetails.length; i++) {
                results += '<tr><td><input id="btn_poplate" type="button"  onclick="getcosting(this)" name="submit" class="btn btn-primary" value="Edit" /></td>';
                results += '<td data-title="podate"  class="1">' + costdetails[i].BranchName + '</td>';
                results += '<td data-title="name" class="2">' + costdetails[i].Branchid + '</td>';
                results += '<td data-title="quotationdate" class="3" style="display:none;">' + costdetails[i].date + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_Costing").html(results);
        }


        function getcosting(thisid) {
                  $('#div_Costing').hide();
                $('#Costing_fillform').css('display', 'block');
                $('#showlogs').css('display', 'none');
            var BranchName = $(thisid).parent().parent().children('.1').html();
            var Branchid = $(thisid).parent().parent().children('.2').html();
            var date = $(thisid).parent().parent().children('.3').html();
            document.getElementById('ddlBranchName').value=Branchid;
            document.getElementById('txtDate').value=date;
            document.getElementById('btn_save').value = "Modify";
            var table = document.getElementById("tabledetails");
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" ID="tabledetails">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">RouteName</th><th scope="col">SaleIncentive</th><th scope="col">PrimaryTransport</th><th scope="col">SecondaryTransport</th><th scope="col">CrateLoss</th><th scope="col">Salary</th><th scope="col">TravelExpense</th><th scope="col">Rent</th><th scope="col">BadDebit</th><th scope="col">OfficeExpense</th><th scope="col">TotalSellingcost</th><th scope="col">SaleVolume</th><th scope="col">TotalImapact</th></tr></thead></tbody>';
             var k = 1;
            for (var i = 0; i < subcost.length; i++) {
                 if (Branchid == subcost[i].branchid) {
                    results += '<tr><td data-title="Sno" class="1">' + k + '</td>';
                    results += '<td ><input id="ddlRouteName" disabled="disabled" readonly  class="Routecls" style="width:90px;" value="' + subcost[i].RouteName + '"/></td>';
                    results += '<td style="display:none;" disabled="disabled" id="ddlRouteName" class="2">' + subcost[i].RouteName + '</td>';
                    results += '<td ><input id="txtSaleIncentive" type="text" class="clsSaleIncentive"  onkeypress="return isFloat(event)" style="width:90px;" value="' + subcost[i].saleincentive + '"/></td>';
                    results += '<td ><input id="txtPrimaryTransport" type="text" class="clsPrimaryTransport"  onkeypress="return isFloat(event)" style="width:90px;" value="' + subcost[i].primarytransport + '"/></td>';
                    results += '<td ><input id="txtSecondaryTransport" type="text" class="clsSecondaryTransport"  onkeypress="return isFloat(event)" style="width:90px;" value="' + subcost[i].secondarytransport + '"/></td>';
                    results += '<td ><input id="txtCrateLoss" type="text" class="clsCrateLoss" style="width:50px;" onkeypress="return isFloat(event)" value="' + subcost[i].crateloss + '"/></td>';
                    results += '<td ><input id="txtSalary" type="text" class="clsSalary" style="width:50px;" onkeypress="return isFloat(event)" value="' + subcost[i].salary + '"/></td>';
                    results += '<td><input id="txtTravelExpense"  class="clsTravelExpense" style="width:90px; value="' + subcost[i].travelexpense + '"/></td>';
                    results += '<td ><input id="txtRent" type="text" class="clsRent"  style="width:50px;" onkeypress="return isFloat(event)" value="' + subcost[i].rent + '"/></td>';
                    results += '<td ><input id="txtBaddebit" type="text" class="clsBaddebit" style="width:90px;" value="' + subcost[i].baddebit + '"/></td>';
                    results += '<td ><input id="txtofficeexpensis" type="text" class="clsofficeexpensis"  style="width:50px;" onkeypress="return isFloat(event)" value="' + subcost[i].officeexpense + '"/></td>';
                    results += '<td ><input id="TxtTSellingCost" type="text"  class="clsTSellingCost" onkeypress="return isFloat(event)" style="width:90px;" value="' + subcost[i].totalsellingcost + '"/></td>';
                    results += '<td ><input id="txtSVolume" type="text"  class="clsTSellingCost" onkeypress="return isFloat(event)" style="width:90px;" value="' + subcost[i].Salevolume + '"/></td>';
                    results += '<td ><input id="txtTImpact" type="text"  class="clsTSellingCost" onkeypress="return isFloat(event)" style="width:90px;" value="' + subcost[i].timapact + '"/></td>';
                    results += '<td ><input id="hdnroutesno" type="hidden" value="' + subcost[i].Routeid + '"/>';
                    results += '<th data-title="From"><input  id="subsno" type="hidden" name="subsno" value="' + subcost[i].sno + '" style="width:90px;" font-size:12px;padding: 0px 5px;height:30px;"></input></td></tr>';
                    k++;
            }
            }
            results += '</table></div>';
            $("#div_Routewisecosting").html(results);
        }
        function forclearall() {
            document.getElementById('ddlBranchName').selectedIndex = "";
            var empty = [];
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" ID="tabledetails">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">RouteName</th><th scope="col">SaleIncentive</th><th scope="col">PrimaryTransport</th><th scope="col">SecondaryTransport</th><th scope="col">CrateLoss</th><th scope="col">Salary</th><th scope="col">TravelExpense</th><th scope="col">Rent</th><th scope="col">BadDebit</th><th scope="col">OfficeExpense</th><th scope="col">TotalSellingcost</th><th scope="col">SaleVolume</th><th scope="col">TotalImapact</th></tr></thead></tbody>';
            for (var i = 0; i < empty.length; i++) {
            }
            results += '</table></div>';
            $("#div_Routewisecosting").html(results);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Route Wise Costing
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operation</a></li>
            <li><a href="#">Route Wise Costing</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Route Wise Costing Details
                </h3>
            </div>
            <div class="box-body">
                <div id="showlogs" align="center">
                    <input id="btn_addCosting" type="button" name="submit" value='AddCosting' class="btn btn-primary"
                         />
                </div>
                <div id="div_Costing">
                </div>
                    <div id='Costing_fillform' style="display: none;">
                    <table align="center">
                       <tr>
                            <td>
                                <label>
                                    Costing Date</label>
                            </td>
                            <td style="height: 40px;">
                            <input id="txtDate" class="form-control" type="date">
                                        </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Branch Name</label>
                            </td>
                            <td style="height: 40px;">
                            <select id="ddlBranchName" class="form-control" onchange="ddlSalesOfficeChange();">
                                        </select>
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td>
                                <label id="lbl_sno"  >
                                </label>
                            </td>
                        </tr>
                         </table>
                         <table align="center">
                         <div id="div_Routewisecosting">
                </div>
                        <tr>
                            <td colspan="2" align="center" style="height: 40px;">
                                <input id="btn_save" type="button" class="btn btn-primary" name="submit" value='save'
                                    onclick="saveCostingDetails()" />
                                <input id='btn_close' type="button" class="btn btn-danger" name="Close" value='Close'/>
                                   
                            </td>
                        </tr>
                    </table>
                </div>
            <%--    <div id='Costing_fillform' style="display: none;">
                    <table align="center">
                        <tr>
                            <td>
                                <label>
                                    Branch Name</label>
                            </td>
                            <td style="height: 40px;">
                            <select id="ddlBranchName" class="form-control" onchange="ddlSalesOfficeChange();">
                                        </select>
                            </td>
                            <td>
                               <label>
                                    RouteName</label>
                            </td>
                            <td>
                             <select id="ddlRouteName" class="form-control">
                                        </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Sale Incentive</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtSaleIncentive" type="text" name="Street" class="form-control"  placeholder="Enter Street"  />
                            </td>
                            <td>
                                <label>
                                    PrimaryTransport</label>
                            </td>
                            <td>
                                <input id="txtPrimaryTransport" type="text" name="Mandal" class="form-control" placeholder="Enter Mandal"/>
                            </td>
                            </tr>
                            <tr>
                            <td >
                                <label>
                                    SecondaryTransport</label>
                            </td>
                            <td>
                                <input id="txtSecondaryTransport" type="text" name="district" class="form-control"  placeholder="Enter district"/>
                            </td>
                            <td>
                                <label>
                                    Crates Loss</label>
                            </td>
                            <td>
                                <input id="txtCrateLoss" type="text" name="Loss" class="form-control" placeholder="Enter Crates Loss"/>
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <label>
                                    Salary</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtSalary" type="text" name="Salary" class="form-control" placeholder="Enter PIN Code" onkeypress="return isNumber(event)"/>
                            </td>
                             <td>
                                <label>
                                    Travel Expensis</label>
                            </td>
                            <td>
                                <input id="txtTravelExpense" type="text" name="TravelExpensis" class="form-control" placeholder="Travel Expensis"/>
                            </td>
                        </tr>
                         <tr>
                            <td >
                                <label>
                                    Rent</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtRent" type="text" name="Rent" class="form-control" placeholder="Enter Rent" onkeypress="return isNumber(event)"/>
                            </td>
                             <td>
                                <label>
                                    Office  Expensis</label>
                            </td>
                            <td>
                                <input id="txtofficeexpensis" type="text" name="OfficeExpensis" class="form-control" placeholder="Office Expensis"/>
                            </td>
                        </tr>
                         <tr>
                            <td >
                                <label>
                                    Provison for bad debit</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtBaddebit" type="text" name="Salary" class="form-control" placeholder="Enter PIN Code" onkeypress="return isNumber(event)"/>
                            </td>
                             <td>
                                <label>
                                    Total SellingCost</label>
                            </td>
                            <td>
                                <input id="TxtTSellingCost" type="text" name="TotalSellingCost" class="form-control" placeholder="Enter Total SellingCost"/>
                            </td>
                        </tr>
                            <tr>
                            <td >
                                <label>
                                    Sales Volume</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtSVolume" type="text" name="SalesVolume" class="form-control" placeholder="Enter Sales Volume"/>
                            </td>
                             <td>
                                <label>
                                    Total Impact</label>
                            </td>
                            <td>
                                <input id="txtTImpact" type="text" name="TotalImpact" class="form-control" placeholder="Enter Total Impact"/>
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td>
                                <label id="lbl_sno"  >
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center" style="height: 40px;">
                                <input id="btn_save" type="button" class="btn btn-primary" name="submit" value='save'
                                    onclick="saveCostingDetails()" />
                                <input id='btn_close' type="button" class="btn btn-danger" name="Close" value='Close'
                                    onclick="cancelCostingdetails()" />
                            </td>
                        </tr>
                    </table>
                </div>--%>
            </div>
        </div>
    </section>
</asp:Content>
