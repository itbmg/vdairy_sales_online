<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DueApproval.aspx.cs" Inherits="DueApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <style type="text/css">
        .ddlsize
        {
            width: 230px;
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
    </style>
    <script type="text/javascript">
        $(function () {
            FillRoutes();
            $("#datepicker").datepicker({ dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
                onSelect: function (selectedDate) {
                    GetEditIndentValues();
                }
            });

            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#datepicker').val(today);
        });
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
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
        var RouteSno = 0;
        function ddlRouteNameChange(Id) {
            var data = { 'operation': 'GetRouteNameChange', 'RouteID': Id.value };
            var s = function (msg) {
                if (msg) {
                    BindBranchName(msg);
                    $('#divFillScreen').removeTemplate();
                    $('#divFillScreen').setTemplateURL('IndentEdit.htm');
                    $('#divFillScreen').processTemplate();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function ddlAgentNameChange() {
            GetEditIndentValues();
        }
        function BindBranchName(msg) {
            document.getElementById('ddlBranchName').options.length = "";
            var veh = document.getElementById('ddlBranchName');
            var length = veh.options.length;
            for (i = length - 1; i >= 0; i--) {
                veh.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Agent Name";
            opt.value = "";
            veh.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null || msg[i].BranchName != "" || msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].b_id;
                    veh.appendChild(opt);
                }
            }
        }
        function btnInventoryDueSaveClick(id) {

        }
        function btnDueSaveClick(id) {
            var dueAmount = $(id).closest("tr").find('#txtdue').text();

            var Remarks = $(id).closest("tr").find('#txtremarks').val();

            if (!confirm("Do you really want Save")) {
                return false;
            }
            var BranchSno = $(id).closest("tr").find('#hdnagentid').val();
            var routesno = $(id).closest("tr").find('#hdnrouteid').val();
            var salevalue = $(id).closest("tr").find('#txtsalevalue').text();
            var collamt = $(id).closest("tr").find('#txtamountcollected').text();
            var salesofficeid = document.getElementById('ddlRouteName').value;
            var txtDate = document.getElementById('datepicker').value;

            var data = { 'operation': 'btnDueApproveClick', 'salesofficeid': salesofficeid, 'txtDate': txtDate, 'BranchSno': BranchSno, 'salevalue': salevalue, 'collamt': collamt, 'routesno': routesno, 'dueAmount': dueAmount, 'Remarks': Remarks };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    if (msg == "Session Expired") {
                        window.location = "Login.aspx";
                    }
                    GetEditIndentValues();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function GetEditIndentValues() {
            var ddlRouteName = document.getElementById('ddlRouteName').value;
            if (ddlRouteName == "Select" || ddlRouteName == "") {
                alert("Please Select Salesoffice Name");
                return false;
            }
            var ddltype = document.getElementById('ddlduetype').value;
            if (ddltype == "Select" || ddltype == "") {
                alert("Please Select Due Type");
                return false;
            }

            var txtDate = document.getElementById('datepicker').value;
            if (txtDate == "") {
                alert("Please Select Date");
                return false;
            }
            var data = { 'operation': 'GetDueDetails', 'RouteID': ddlRouteName, 'IndDate': txtDate, 'ddltype': ddltype };
            var s = function (msg) {
                if (msg) {
                    if (ddltype == "Amount") {
                        $('#divFillScreen').removeTemplate();
                        $('#divFillScreen').setTemplateURL('dueapproval.htm');
                        $('#divFillScreen').processTemplate(msg);
                    }
                    if (ddltype == "Inventory") {
                        $('#divFillScreen').removeTemplate();
                        $('#divFillScreen').setTemplateURL('InventoryDueApproval.htm');
                        $('#divFillScreen').processTemplate(msg);
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
        function btnEditIndentSaveClick(id) {
            var txtDate = document.getElementById('datepicker').value;
            if (txtDate == "") {
                alert("Please Select Date");
                return false;
            }
            var rows = $("#table_Indent_details tr:gt(0)");
            var Indentdetails = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtProductQty').val() != "") {
                    Indentdetails.push({ Productsno: $(this).find('#hdnProductSno').val(), Product: $(this).find('#txtProductName').text(), DelQty: $(this).find('#txtDeliveryQty').val(), Rate: $(this).find('#txtUnitCost').val(), IndentNo: $(this).find('#txtIndentNo').text() });
                }
            });
            //            var IndentNo = $(id).closest("tr").find('#txtIndentNo').text();
            //            var ProductSno = $(id).closest("tr").find('#hdnProductSno').val();
            //            var UnitCost = $(id).closest("tr").find('#txtUnitCost').val();
            //            var DelQty = $(id).closest("tr").find('#txtDeliveryQty').val();
            //            if (UnitCost == "") {
            //                alert("Please Enter Rate");
            //                return false;
            //            }
            //            if (DelQty == "") {
            //                alert("Please Enter Del Qty");
            //                return false;
            //            }
            var BranchName = document.getElementById('ddlBranchName').value;
            var ddlRouteName = document.getElementById('ddlRouteName').value;
            var data = { 'operation': 'btnEditIndentSaveClick', 'data': Indentdetails, 'BranchID': BranchName, 'refno': ddlRouteName, 'indentdate': txtDate };
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
            Due Approval<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Due Approval</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Due Approval Details
                </h3>
            </div>
            <div class="box-body">
                <table>
                    <tr>
                    </tr>
                    <tr>
                        <td nowrap>
                            <label for="lblBranch">
                                Due Type</label>
                        </td>
                        <td>
                            <select id="ddlduetype" class="form-control">
                                <option selected="selected">Amount</option>
                                <option>Inventory</option>
                            </select>
                        </td>
                        <td style="width:5px;"></td>

                        <td >
                      <label for="lblBranch">
                                SalesOffice</label>
                        </td>
                        <td>
                            <select id="ddlRouteName" class="form-control">
                            </select>
                        </td>
                        <td style="width:5px;"></td>

                        <td >
                            <label for="lblBranch">
                                Indent Date</label>
                        </td>
                        <td>
                            <input type="date" id="datepicker" placeholder="DD-MM-YYYY" class="form-control" />
                        </td>
                                              <td style="width:5px;"></td>

                        <td>
                            <input type="button" id="Button1" value="Get Due" class="btn btn-primary" onclick="GetEditIndentValues();" />
                        </td>
                    </tr>
                </table>
                <div id="divPrint">
                    <div id="divFillScreen">
                    </div>
                    <div id="signature">
                    </div>
                </div>
            </div>
            <div>
                <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print" OnClientClick="javascript:CallPrint('divPrint');"
                    runat="Server" />
                <br />
                <br />
            </div>
        </div>
    </section>
</asp:Content>
