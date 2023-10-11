<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EditBalance.aspx.cs" Inherits="EditBalance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   <script src="js/jquery.js"></script>
    <script src="JSF/jquery.min.js"></script>
    <script src="JSF/jquery-ui.js" type="text/javascript"></script>
    <script src="JSF/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">


        $(function () {
            FillSalesOffice()
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#txtFrom_date').val(today);
            $('#txtTo_date').val(today);
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
        function ddlSalesOfficeChanged(ID) {
            var BranchID = ID.value;
            var data = { 'operation': 'GetSalesRoutes', 'BranchID': BranchID };
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
            document.getElementById('ddlDispName').options.length = "";
            var veh = document.getElementById('ddlDispName');
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
        function ddlDispNameChanged(id) {
            FillAgentName(id.value);
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


        function btn_Get_AgentBal_Details() {
            var fromdate = document.getElementById('txtFrom_date').value;
            var AgentId = document.getElementById('ddlAgentName').value;
            var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
            var todate = document.getElementById('txtTo_date').value;
            if (fromdate == "") {
                alert("Please select from date");
                return false;
            }
            if (todate == "") {
                alert("Please select from date");
                return false;
            }
            var data = { 'operation': 'get_Agent_Bal_Trans', 'AgentId': AgentId, 'ddlSalesOffice': ddlSalesOffice, 'fromdate': fromdate, 'todate': todate };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fill_details(msg);
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
        function fill_details(msg) {
            var results = '<div  style="overflow:auto;"><table id="myTable" class="table table-bordered table-hover dataTable no-footer">';
            results += '<thead><tr><th scope="col" >Sno</th><th scope="col" >Agentid</th><th scope="col">Date</th><th scope="col">AgentName</th><th scope="col">Op_Balance</th><th scope="col">SaleValue</th><th scope="col">PaidAmount</th><th scope="col">ClosingValue</th><th></th></tr></thead></tbody>';
            var k = 1;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                //k++;
                results += '<td scope="row"  style="text-align:center;">' + k + '</td>';
                results += '<th scope="row" id="spnAgentId" class="clsAgentid" style="text-align:center;">' + msg[i].AgentId + '</th>';
                results += '<td id="spnDate" class="clsDate">' + msg[i].inddate + '</td>';
                results += '<td id="spnAgentName" class="clsAgentName">' + msg[i].AgentName + '</td>';
                results += '<td class="4"><span id="spn_OpBal" class="clsOp">' + msg[i].opp_balance + '</span></td>';
                results += '<td id="spn_PrevOpBal" class="clsPrevOp" style="width:65px;display:none;">' + msg[i].opp_balance + '</td>';
                //results += '<td><input id="txt_OpBal" data-title="Code" style="width:65px;" onkeyup="CLChange(this);" class="4"  value="' + msg[i].opp_balance + '"/></td>';
                results += '<td><input  id="txt_SaleValue" class="clsSaleValue" style="width:65px;" value="' + msg[i].salesvalue + '"/></td>';
                results += '<td><input id="txt_PaidAmount" class="clsPaidAmount" style="width:65px;" value="' + msg[i].paidamount + '"/></td>';
                results += '<td id="txt_PrevCloBal" class="clsPrevCloBal" style="width:65px;display:none;">' + msg[i].clo_balance + '</td>';
                results += '<td><input  id="txt_CloBal" class="clsCloBal" style="width:65px;" value="' + msg[i].clo_balance + '"/></td>';
                results += '<td><input  id="txt_Sno" class="8" style="width:65px;display:none;"  value="' + msg[i].sno + '"/></td></tr >';
                k++;
            }
            results += '</table></div>';
            $("#div_BrandData").html(results);
        }

        

        var salevalue = 0; var paidamount = 0; var closingamt = 0;
        $(document).click(function () {
            increment = 0;
            $('#myTable').on('change', '.clsSaleValue', calTotal_gst)
                .on('change', '.clsPaidAmount', calTotal_gst);
                .on('change', '.clsCloBal', calTotal_gst);

        });




        var DataTable;
       var presentClosing = 0;
        function insertrow() {
            DataTable = [];
            var DataTable1 = [];
            var AgentName = 0;
            var AgentId = 0;
            var Op_Bal = 0;
            var PrevOp_Bal = 0;
            var SaleValue = 0;
            var PaidAmount = 0;
            var Clo_Bal = 0;
            var PrevClo_Bal = 0;
            var sno = 0;
            var IndDate = 0;
            var rows = $("#myTable tr:gt(0)");
            var rowsno = 1;
            var count = 0;
            $(rows).each(function (i, obj) {
                AgentId = $(this).find('#spnAgentId').text();
                AgentName = $(this).find('#spnAgentName').text();
                IndDate = $(this).find('#spnDate').text();

                PrevOp_Bal = parseFloat($(this).find('#spn_PrevOpBal').text());
                SaleValue = parseFloat($(this).find('#txt_SaleValue').val());
                PaidAmount = parseFloat($(this).find('#txt_PaidAmount').val());
                if (count == 0) {
                    Clo_Bal = parseFloat($(this).find('#txt_CloBal').val());
                    Op_Bal = parseFloat($(this).find('#spn_OpBal').text());
                }
                else {
                    Clo_Bal = presentClosing;
                    Op_Bal = PresentOpening;
                }
                PrevClo_Bal = parseFloat($(this).find('#txt_PrevCloBal').text());
                sno = parseFloat($(this).find('#txt_Sno').val());
                if (PrevClo_Bal != Clo_Bal) {
                    if (count == 0) {
                        Op_Bal = PrevOp_Bal;
                        Clo_Bal = PrevOp_Bal + SaleValue - PaidAmount;

                    }
                    else {
                        Clo_Bal = Op_Bal + SaleValue - PaidAmount;
                    }

                    presentClosing = Clo_Bal;
                    PresentOpening = Clo_Bal;
                    count++;
                }
                else {
                    Clo_Bal = Op_Bal + SaleValue - PaidAmount;
                    //Op_Bal = Clo_Bal;
                }
                sno = $(this).find('#txt_Sno').val();
                DataTable1.push({ 'Op_Bal': Op_Bal, 'AgentName': AgentName, 'AgentId': AgentId, 'SaleValue': SaleValue, 'PaidAmount': PaidAmount, 'Clo_Bal': Clo_Bal, 'sno': sno, 'IndDate': IndDate });//, freigtamt: freigtamt
                rowsno++;
            });
            var results = '<div  style="overflow:auto;"><table id="myTable" class="table table-bordered table-hover dataTable no-footer">';
            results += '<thead><tr><th scope="col" >Sno</th><th scope="col" >Agentid</th><th scope="col">Date</th><th scope="col">AgentName</th><th scope="col">Op_Balance</th><th scope="col">SaleValue</th><th scope="col">PaidAmount</th><th scope="col">ClosingValue</th><th scope="col"></th></tr></thead></tbody>';
            for (var i = 0; i < DataTable1.length; i++) {
                results += '<tr><td scope="row" class="1" st    yle="text-align:center;" id="txtsno">' + i + '</td>';
                results += '<th scope="row" id="spnAgentId" class="clsAgentid" style="text-align:center;">' + DataTable1[i].AgentId + '</th>';
                results += '<td id="spnDate" class="clsDate">' + DataTable1[i].IndDate + '</td>';
                results += '<td id="spnAgentName" class="clsAgentName">' + DataTable1[i].AgentName + '</td>';
                results += '<td class="4"><span id="spn_OpBal" class="clsOp">' + parseFloat(DataTable1[i].Op_Bal).toFixed(2) + '</span></td>';
                results += '<td id="spn_PrevOpBal" class="clsPrevOp" style="width:65px;display:none;">' + parseFloat(DataTable1[i].Op_Bal).toFixed(2) + '</td>';
                results += '<td><input  id="txt_SaleValue" class="clsSaleValue" style="width:65px;" value="' + DataTable1[i].SaleValue + '"/></td>';
                results += '<td><input id="txt_PaidAmount" class="clsPaidAmount" style="width:65px;" value="' + DataTable1[i].PaidAmount + '"/></td>';
                results += '<td><input  id="txt_CloBal" class="clsCloBal" style="width:65px;" value="' + parseFloat(DataTable1[i].Clo_Bal).toFixed(2) + '"/></td>';
                results += '<td id="txt_PrevCloBal" class="clsPrevCloBal" style="width:65px;display:none;">' + parseFloat(DataTable1[i].Clo_Bal).toFixed(2) + '</td>';
                results += '<td><input  id="txt_Sno" class="8" style="width:65px;display:none;"  value="' + DataTable1[i].sno + '"/></td>';
                results += '<td style="display:none" class="4">' + i + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_BrandData").html(results);
        }

        var salevalue = 0; var paidamount = 0; var closingamt = 0;
        let increment = 0;
        function calTotal_gst() {

            var $row = $(this).closest('tr'),
                salevalue = parseFloat($row.find('.clsSaleValue').val(),) || 0
            op = parseFloat($row.find('.clsOp').text(),) || 0
            //closingamt = $row.find('.clsPaidAmount').val(),
            paidamount = parseFloat($row.find('.clsPaidAmount').val(),) || 0

            closingamt = op + salevalue - paidamount;
            $row.find('.clsCloBal').val(parseFloat(closingamt).toFixed(2));
                insertrow();
        }

        var filldetails = [];
        function btnUpdate_Click() {
            $('#myTable> tbody > tr').each(function () {
                var Op_Bal = $(this).find('#spn_OpBal').text();
                var SaleValue = $(this).find('#txt_SaleValue').val();
                var PaidAmount = $(this).find('#txt_PaidAmount').val();
                var Clo_Bal = $(this).find('#txt_CloBal').val();
                var sno = $(this).find('#txt_Sno').val();
                var Date = $(this).find('#spnDate').text();
                var Agentid = $(this).find('#spnAgentId').text();
                filldetails.push({ 'opp_balance': Op_Bal, 'salesvalue': SaleValue, 'PaidAmount': PaidAmount, 'clo_balance': Clo_Bal, 'sno': sno, 'inddate': Date,'AgentId': Agentid });//, 'freigtamt': freigtamt
            });
            var data = { 'operation': 'Edit_Agent_Bal_Trans', 'filldetails': filldetails  };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    btn_Get_AgentBal_Details();
                    if (msg == "Session Expired") {
                        window.location = "Login.aspx";
                    }
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>AgentEditBalance<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">AgentEditBalance</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>AgentEditBalance
                </h3>
            </div>
            <div class="box-body">
                <table>
                    <tr>
                        <td>
                            <select id="ddlSalesOffice" class="form-control" onchange="ddlSalesOfficeChanged(this);">
                            </select>
                        </td>
                        <td style="width: 5px;"></td>
                        <td>
                            <select id="ddlDispName" class="form-control" onchange="ddlDispNameChanged(this);">
                            </select>
                        </td>
                        <td style="width: 5px;"></td>
                        <td>
                            <select id="ddlAgentName" class="form-control">
                            </select>
                        </td>
                        <td style="width: 5px;"></td>
                        <td>
                            <input type="date" id="txtFrom_date" class="form-control" />
                        </td>
                        <td style="width: 5px;"></td>
                        <td>
                            <input type="date" id="txtTo_date" class="form-control" />
                        </td>
                        <td style="width: 5px;"></td>
                        <td>
                            <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="btn_Get_AgentBal_Details()">
                                <i class="fa fa-refresh"></i>Get Details
                            </button>
                        </td>
                    </tr>
                </table>

                <br />

                <br />
                <br />
                <div id="div_BrandData">
                </div>
                <div style="text-align:center">
                <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="btnUpdate_Click()">
                    <i class="fa fa-refresh"></i>Save
                </button>
                    </div>
            </div>
        </div>
    </section>
</asp:Content>

