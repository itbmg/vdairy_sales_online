<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MilkBuyerTransactonremarks.aspx.cs" Inherits="MilkBuyerTransactonremarks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
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
        $(function () {
            FillRoutes();
            var date = new Date();
            var fromday = date.getDate() - 2;
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (fromday < 10) fromday = "0" + fromday;
            var fromdate = year + "-" + month + "-" + fromday;
            $('#txtfromdate').val(fromdate);
        });
        function FillRoutes() {
            var data = { 'operation': 'GetPlantSalesOffice' };
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
            opt.innerHTML = "Select";
            ddlRouteName.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlRouteName.appendChild(opt);
                }
            }
        }
        function fillRouteNames() {
            var salesofficeid = document.getElementById('ddlRouteName').value;
            var data = { 'operation': 'GetSalesRoutes', 'BranchID': salesofficeid };
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
            document.getElementById('ddl_dispatchname').options.length = "";
            var veh = document.getElementById('ddl_dispatchname');
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
        function GetIndentValues() {
            var salesofficeid = document.getElementById('ddlRouteName').value;
            if (salesofficeid == "Select" || salesofficeid == "") {
                alert("Please Select Salesoffice Name");
                return false;
            }
            var ddldispatchname = document.getElementById('ddl_dispatchname').value;
            if (ddldispatchname == "Select" || ddldispatchname == "") {
                alert("Please Select Route Name");
                return false;
            }
            var fromdate = document.getElementById('txtfromdate').value;
            if (fromdate == "") {
                alert("Please Select From Date");
                return false;
            }
//            var todate = document.getElementById('txttodate').value;
//            if (todate == "") {
//                alert("Please Select To Date");
//                return false;
//            }
//            document.getElementById('spnfromdate').innerHTML = fromdate;
//            document.getElementById('spntodate').innerHTML = todate;
            var data = { 'operation': 'GetMilkBuyerTransactonDetails', 'BranchID': salesofficeid, 'ddldispatchname': ddldispatchname, 'fromdate': fromdate };
            var s = function (msg) {
                if (msg) {
                    BindGrid(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);

        }
        function BindGrid(msg) {
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">Agent Name</th><th scope="col">SR Name</th><th scope="col">Due</th><th scope="col">Cheque Pending</th><th scope="col">Net Due</th><th scope="col">Due Date</th><th scope="col">Inv Bal</th><th scope="col">Inv Date</th><th scope="col">Remarks</th><th scope="col"></th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                var due = msg[i].due;
                var invbal = msg[i].invbal;
                var agentname = msg[i].agentname;

                results += '<tr>';
                if (agentname == "AGENTS") {
                    results += '<td scope="row" class="1" ><span style="color:#126146;font-size:22px;font-weight:bold;">' + msg[i].agentname + '</span></td>';
                    results += '<td scope="row" class="2" >' + msg[i].srname + '</td>';
                    results += '<td scope="row" class="3" >' + msg[i].due + '</td>';
                    results += '<td scope="row" class="4" >' + msg[i].chequepending + '</td>';
                    results += '<td scope="row" class="5" >' + msg[i].netdue + '</td>';
                    results += '<td scope="row" class="6" >' + msg[i].duedate + '</td>';
                    results += '<td scope="row" class="7" >' + msg[i].invbal + '</td>';
                    results += '<td scope="row" class="8" >' + msg[i].invdate + '</td>';
                }
                else if (agentname == "CATERING AGENTS") {
                    results += '<td scope="row" class="1" style="color:#DE0505;font-size:22px;font-weight:bold;">' + msg[i].agentname + '</td>';
                    results += '<td scope="row" class="2" >' + msg[i].srname + '</td>';
                    results += '<td scope="row" class="3" >' + msg[i].due + '</td>';
                    results += '<td scope="row" class="4" >' + msg[i].chequepending + '</td>';
                    results += '<td scope="row" class="5" >' + msg[i].netdue + '</td>';
                    results += '<td scope="row" class="6" >' + msg[i].duedate + '</td>';
                    results += '<td scope="row" class="7" >' + msg[i].invbal + '</td>';
                    results += '<td scope="row" class="8" >' + msg[i].invdate + '</td>';
                }
                else if (agentname == "DISCONTINUED AGENTS") {
                    results += '<td scope="row" class="1" style="color:#FF2911;font-size:22px;font-weight:bold;">' + msg[i].agentname + '</td>';
                    results += '<td scope="row" class="2" >' + msg[i].srname + '</td>';
                    results += '<td scope="row" class="3" >' + msg[i].due + '</td>';
                    results += '<td scope="row" class="4" >' + msg[i].chequepending + '</td>';
                    results += '<td scope="row" class="5" >' + msg[i].netdue + '</td>';
                    results += '<td scope="row" class="6" >' + msg[i].duedate + '</td>';
                    results += '<td scope="row" class="7" >' + msg[i].invbal + '</td>';
                    results += '<td scope="row" class="8" >' + msg[i].invdate + '</td>';
                }
                else if (agentname == "DUE AGENTS") {
                    results += '<td scope="row" class="1" style="color:DeepSkyBlue;font-size:22px;font-weight:bold;">' + msg[i].agentname + '</td>';
                    results += '<td scope="row" class="2" >' + msg[i].srname + '</td>';
                    results += '<td scope="row" class="3" >' + msg[i].due + '</td>';
                    results += '<td scope="row" class="4" >' + msg[i].chequepending + '</td>';
                    results += '<td scope="row" class="5" >' + msg[i].netdue + '</td>';
                    results += '<td scope="row" class="6" >' + msg[i].duedate + '</td>';
                    results += '<td scope="row" class="7" >' + msg[i].invbal + '</td>';
                    results += '<td scope="row" class="8" >' + msg[i].invdate + '</td>';
                }
                else if (agentname == "INSTITUTIONAL") {
                    results += '<td scope="row" class="1" style="color:#1170EA;font-size:22px;font-weight:bold;">' + msg[i].agentname + '</td>';
                    results += '<td scope="row" class="2" >' + msg[i].srname + '</td>';
                    results += '<td scope="row" class="3" >' + msg[i].due + '</td>';
                    results += '<td scope="row" class="4" >' + msg[i].chequepending + '</td>';
                    results += '<td scope="row" class="5" >' + msg[i].netdue + '</td>';
                    results += '<td scope="row" class="6" >' + msg[i].duedate + '</td>';
                    results += '<td scope="row" class="7" >' + msg[i].invbal + '</td>';
                    results += '<td scope="row" class="8" >' + msg[i].invdate + '</td>';
                }
                else if (agentname == "Comapass Group") {
                    results += '<td scope="row" class="1" style="color:#669800;font-size:22px;font-weight:bold;">' + msg[i].agentname + '</td>';
                    results += '<td scope="row" class="2" >' + msg[i].srname + '</td>';
                    results += '<td scope="row" class="3" >' + msg[i].due + '</td>';
                    results += '<td scope="row" class="4" >' + msg[i].chequepending + '</td>';
                    results += '<td scope="row" class="5" >' + msg[i].netdue + '</td>';
                    results += '<td scope="row" class="6" >' + msg[i].duedate + '</td>';
                    results += '<td scope="row" class="7" >' + msg[i].invbal + '</td>';
                    results += '<td scope="row" class="8" >' + msg[i].invdate + '</td>';
                }
                else if (agentname == "Fresh and Honest cafe") {
                    results += '<td scope="row" class="1" style="color:#0B6482;font-size:22px;font-weight:bold;">' + msg[i].agentname + '</td>';
                    results += '<td scope="row" class="2" >' + msg[i].srname + '</td>';
                    results += '<td scope="row" class="3" >' + msg[i].due + '</td>';
                    results += '<td scope="row" class="4" >' + msg[i].chequepending + '</td>';
                    results += '<td scope="row" class="5" >' + msg[i].netdue + '</td>';
                    results += '<td scope="row" class="6" >' + msg[i].duedate + '</td>';
                    results += '<td scope="row" class="7" >' + msg[i].invbal + '</td>';
                    results += '<td scope="row" class="8" >' + msg[i].invdate + '</td>';
                }
                else if (agentname == "Parlour") {
                    results += '<td scope="row" class="1" style="color:SlateBlue;font-size:22px;font-weight:bold;">' + msg[i].agentname + '</td>';
                    results += '<td scope="row" class="2" >' + msg[i].srname + '</td>';
                    results += '<td scope="row" class="3" >' + msg[i].due + '</td>';
                    results += '<td scope="row" class="4" >' + msg[i].chequepending + '</td>';
                    results += '<td scope="row" class="5" >' + msg[i].netdue + '</td>';
                    results += '<td scope="row" class="6" >' + msg[i].duedate + '</td>';
                    results += '<td scope="row" class="7" >' + msg[i].invbal + '</td>';
                    results += '<td scope="row" class="8" >' + msg[i].invdate + '</td>';
                }
                else if (agentname == "Total") {
                    results += '<td scope="row" class="1" style="color:SlateBlue;font-size:16px;font-weight:bold;">' + msg[i].agentname + '</td>';
                    results += '<td scope="row" class="2" >' + msg[i].srname + '</td>';
                    results += '<td scope="row" class="3" style="color:SlateBlue;font-size:16px;font-weight:bold;">' + msg[i].due + '</td>';
                    results += '<td scope="row" class="4" style="color:SlateBlue;font-size:16px;font-weight:bold;">' + msg[i].chequepending + '</td>';
                    results += '<td scope="row" class="5" style="color:SlateBlue;font-size:16px;font-weight:bold;">' + msg[i].netdue + '</td>';
                    results += '<td scope="row" class="6" style="color:SlateBlue;font-size:16px;font-weight:bold;">' + msg[i].duedate + '</td>';
                    results += '<td scope="row" class="7" style="color:SlateBlue;font-size:16px;font-weight:bold;">' + msg[i].invbal + '</td>';
                    results += '<td scope="row" class="8" >' + msg[i].invdate + '</td>';
                }
                else if (agentname == "Sub Total") {
                    results += '<td scope="row" class="1" style="color:SlateBlue;font-size:18px;font-weight:bold;">' + msg[i].agentname + '</td>';
                    results += '<td scope="row" class="2" >' + msg[i].srname + '</td>';
                    results += '<td scope="row" class="3" style="color:SlateBlue;font-size:18px;font-weight:bold;">' + msg[i].due + '</td>';
                    results += '<td scope="row" class="4" style="color:SlateBlue;font-size:18px;font-weight:bold;">' + msg[i].chequepending + '</td>';
                    results += '<td scope="row" class="5" style="color:SlateBlue;font-size:18px;font-weight:bold;">' + msg[i].netdue + '</td>';
                    results += '<td scope="row" class="6" >' + msg[i].duedate + '</td>';
                    results += '<td scope="row" class="7" style="color:SlateBlue;font-size:18px;font-weight:bold;">' + msg[i].invbal + '</td>';
                    results += '<td scope="row" class="8" >' + msg[i].invdate + '</td>';
                }
                else if (agentname == "Grand Total") {
                    results += '<td scope="row" class="1" style="color:SlateBlue;font-size:22px;font-weight:bold;">' + msg[i].agentname + '</td>';
                    results += '<td scope="row" class="2" >' + msg[i].srname + '</td>';
                    results += '<td scope="row" class="3" style="color:SlateBlue;font-size:22px;font-weight:bold;">' + msg[i].due + '</td>';
                    results += '<td scope="row" class="4" style="color:SlateBlue;font-size:22px;font-weight:bold;">' + msg[i].chequepending + '</td>';
                    results += '<td scope="row" class="5" style="color:SlateBlue;font-size:22px;font-weight:bold;">' + msg[i].netdue + '</td>';
                    results += '<td scope="row" class="6" >' + msg[i].duedate + '</td>';
                    results += '<td scope="row" class="7" style="color:SlateBlue;font-size:22px;font-weight:bold;">' + msg[i].invbal + '</td>';
                    results += '<td scope="row" class="8" >' + msg[i].invdate + '</td>';
                }
                else {
                    results += '<td scope="row" class="1" >' + msg[i].agentname + '</td>';
                    results += '<td scope="row" class="2" >' + msg[i].srname + '</td>';
                    results += '<td scope="row" class="3" >' + msg[i].due + '</td>';
                    results += '<td scope="row" class="4" >' + msg[i].chequepending + '</td>';
                    results += '<td scope="row" class="5" >' + msg[i].netdue + '</td>';
                    results += '<td scope="row" class="6" >' + msg[i].duedate + '</td>';
                    results += '<td scope="row" class="7" >' + msg[i].invbal + '</td>';
                    results += '<td scope="row" class="8" >' + msg[i].invdate + '</td>';
                }
                results += '<td style="display:none;" scope="row" class="9" >' + msg[i].agentid + '</td>';
              
                if (due == "" || agentname == "Total" || agentname == "Sub Total" || agentname == "Grand Total") {
                    results += '<td  class="9"></td>';
                    results += '<td></td></tr>';
                }
                else {
                    results += '<td  class="10"><input type="text" id="txtremarks" placeholder="Enter Remarks" class="form-control" /></td>';
                    results += '<td><input id="btn_poplate" type="button"  onclick="btnremarkssaveClick(this)" name="submit" class="btn btn-primary" value="Save" /></td></tr>';
                }
            }
            results += '</table></div>';
            $("#div_data").html(results);
        }
        function btnremarkssaveClick(thisid) {
            var salesofficeid = document.getElementById('ddlRouteName').value;
            if (salesofficeid == "Select" || salesofficeid == "") {
                alert("Please Select Salesoffice Name");
                return false;
            }
            var ddldispatchname = document.getElementById('ddl_dispatchname').value;
            if (ddldispatchname == "Select" || ddldispatchname == "") {
                alert("Please Select Route Name");
                return false;
            }
            var fromdate = document.getElementById('txtfromdate').value;
            if (fromdate == "") {
                alert("Please Select From Date");
                return false;
            }
            var due = $(thisid).parent().parent().children('.3').html();
            var chequepending = $(thisid).parent().parent().children('.4').html();
            var netdue = $(thisid).parent().parent().children('.5').html();
            var duedate = $(thisid).parent().parent().children('.6').html();
            var invbal = $(thisid).parent().parent().children('.7').html();
            var invdate = $(thisid).parent().parent().children('.8').html();
            var agentid = $(thisid).parent().parent().children('.9').html();
            var Remarks = $(thisid).closest("tr").find('#txtremarks').val();
            var data = { 'operation': 'btnMilkBuyerTransactonremarkssaveClick', 'due': due, 'chequepending': chequepending, 'netdue': netdue, 'duedate': duedate, 'invbal': invbal, 'invdate': invdate, 'agentid': agentid, 'Remarks': Remarks, 'BranchID': salesofficeid, 'fromdate': fromdate };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
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
            Milk Buyer Remarks<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Milk Buyer Remarks</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Milk Buyer Details
                </h3>
            </div>
            <div class="box-body">
                <table>
                    <tr>
                    </tr>
                    <tr>
                        <td nowrap>
                            <label for="lblBranch">
                                SalesOffice</label>
                        </td>
                        <td>
                            <select id="ddlRouteName" class="form-control" onchange="fillRouteNames();">
                            </select>
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                            <lable id="lblroutename">Route</lable>
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                            <select id="ddl_dispatchname" class="form-control">
                            </select>
                        </td>
                        <td style="width: 5px;">
                        </td>
                         
                        <td>
                            <label>
                                From Date:</label>
                        </td>
                        <td>
                            <input type="date" id="txtfromdate" placeholder="DD-MM-YYYY" class="form-control" />
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                            <input type="button" id="Button1" value="Get Details" class="btn btn-primary" onclick="GetIndentValues();" />
                        </td>
                    </tr>
                </table>
                <div id="divPrint">
                    <div id="div_data">
                    </div>
                </div>
            </div>
            <div align="center">
                <input type="button" id="btnsave" value="Save" onclick="btnApproveSaveClick();" style="display: none;"
                    class="btn btn-primary" />
            </div>
            <br />
            <br />
            <div>
                <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print" OnClientClick="javascript:CallPrint('divPrint');"
                    runat="Server" />
                <br />
                <br />
            </div>
        </div>
    </section>
</asp:Content>
