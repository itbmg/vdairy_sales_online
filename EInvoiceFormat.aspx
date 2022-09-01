<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EInvoiceFormat.aspx.cs" Inherits="EInvoiceFormat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="Js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #content {
            position: absolute;
            z-index: 1;
        }

        #bg-text {
            position: absolute;
            opacity: 0.1;
            color: lightgrey;
            font-size: 120px; /*transform:rotate(300deg);
        -webkit-transform:rotate(300deg);*/
        }
    </style>
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
            //document.getElementById("bg-text").style.opacity = "0.1";
            //document.getElementById("bg-text").style.width = "85%";
            //document.getElementById("bg-text").style.position = "absolute";
            //document.getElementById("bg-text").style.padding = "12% 8% 8% 8%";
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
            var ddlsalesOffice = document.getElementById('ddlsalesOffice');
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

        function btnEInvoice_click() {
            var fromdate = document.getElementById('txtFrom_date').value;
            var ddlsalesOffice = document.getElementById('ddlsalesOffice').value;
            if (fromdate == "") {
                alert("Please select from date");
                return false;
            }
            var data = { 'operation': 'btnEInvoice_click', 'fromdate': fromdate, 'SOID': ddlsalesOffice };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Data not found") {
                        alert(msg);
                        return false;
                    }
                    /*if (msg.length > 0) {*/
                        spnJsonData.innerHTML = JSON.stringify(msg);

                    //}
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
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
        <h1>Agent Invoice<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Masters</a></li>
            <li><a href="#">Agent Invoice</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Agent Invoice Details
                </h3>
            </div>
            <div class="box-body">
                <table>
                    <tr>
                        <td>
                            <select id="ddlsalesOffice" class="form-control">
                            </select>
                        </td>
                        <td style="width: 5px;"></td>
                        <td>
                            <input type="date" id="txtFrom_date" class="form-control" />
                        </td>
                        <td style="width: 5px;"></td>
                        <td>
                            <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="btnEInvoice_click()">
                                <i class="fa fa-refresh"></i>Get Details
                            </button>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <div id="div_itemdetails">
                    <span id="spnJsonData"></span>
                </div>
            </div>
            <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="javascript:CallPrint('divPrint');">
                <i class="fa fa-print"></i>Print
            </button>
        </div>
    </section>
</asp:Content>


