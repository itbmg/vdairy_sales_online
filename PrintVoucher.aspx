<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PrintVoucher.aspx.cs" Inherits="PrintVoucher" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3004" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <%--<script language="javascript">
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
    </script>--%>
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
            window.history.forward(1);

        });
        function PopupOpen(VocherID) {
            var data = { 'operation': 'GetSubPaybleValues', 'VoucherID': VocherID };
            var s = function (msg) {
                if (msg) {
                    $('#divHead').setTemplateURL('SubPayable.htm');
                    $('#divHead').processTemplate(msg);
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
                    alert(msg);
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
    <style>
        .divPrintcss /*{
background:url(http://www.vyshnavi.net/Images/Vlogo.png) no-repeat scroll 0 5px transparent;
height: 40px;
width: 40px;
display: block;
color: #0252AA;
font-weight: bold;
    }*/</style>
    <style type="text/css">
        .mylbl
        {
            font-size: 12px;
        }
        .mylbl1
        {
            font-size: 12px; /*font-weight: bold;
            
            color:#0252aa;
            font-family: ravvi;*/
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
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
            Voucher<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#"><i></i>Cash details</a></li>
            <li><a href="#">Voucher</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Voucher Details
                </h3>
            </div>
            <div class="box-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <div>
                                            <table id="tbltrip">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_tripid" runat="server">Voucher No:</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtVoucherNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </td>
                                                    <td style="width:6px;">
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnGenerate" runat="server" CssClass="btn btn-primary" OnClick="btnGenerate_Click"
                                                            Text="Generate" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div id="divPrint">
                                <div>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td rowspan="2">
                                                <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="55px" height="25px" />
                                            </td>
                                            <td colspan="4">
                                                <h4>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="mylbl1" Text=""></asp:Label>
                                                </h4>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: 10px; padding-left: 24%; text-decoration: underline;">
                                                <asp:Label ID="lblVoucherType" runat="server" CssClass="mylbl" Text=""></asp:Label>
                                                <%-- <span style="font-size: 16px; font-weight: bold;padding-left: 24%;text-decoration:underline; ">DEBIT VOUCHER</span>--%><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span style="font-size: 12px; float: right;">Voucher No:</span>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblVoucherno" runat="server" CssClass="mylbl" Text=""></asp:Label>
                                                </b>
                                            </td>
                                            <td>
                                                <span style="font-size: 12px; float: right;">Date:</span>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblDate" runat="server" CssClass="mylbl" Text=""></asp:Label>
                                                </b>
                                            </td>
                                        </tr>
                                    </table>
                                    <div>
                                        <table>
                                            <tr>
                                                <td>
                                                    <b>Pay To </b>
                                                </td>
                                                <td style="float: left;">
                                                    <b>
                                                        <asp:Label ID="lblPayCash" runat="server" Text="" CssClass="mylbl"></asp:Label>
                                                    </b>
                                                </td>
                                            </tr>
                                            <tr style="background-color: #f4f4f4;">
                                                <td colspan="2" rowspan="1" style="width: 100%;">
                                                    <div id="divHead">
                                                    </div>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <b><span class="mylbl">Towards</span> </b>
                                                </td>
                                                <td style="float: left;">
                                                    <b>
                                                        <asp:Label ID="lblTowards" runat="server" CssClass="mylbl" Text=""></asp:Label></b>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <b><span class="mylbl">Received Rs.</span> </b>
                                                </td>
                                                <td style="float: left;">
                                                    <b>
                                                        <asp:Label ID="lblReceived" runat="server" CssClass="mylbl" Text=""></asp:Label>
                                                    </b>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                    <div>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 20%;">
                                                    <span style="font-size: 12px;">Cashier</span>
                                                </td>
                                                <td style="width: 20%;">
                                                    <span style="font-size: 12px;">Verified By</span>
                                                </td>
                                                <td style="width: 20%;">
                                                    <span style="font-size: 12px;">Accounts officer</span>
                                                </td>
                                                <td style="width: 20%;">
                                                    <asp:Label ID="lblApprove" runat="server" Font-Size="10px" Text=""></asp:Label>
                                                </td>
                                                <td style="width: 20%;">
                                                    <span style="font-size: 12px;">Receiver's Signature</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <br />
                          <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="javascript:CallPrint('divPrint');"><i class="fa fa-print"></i> Print </button>
                            <br />
                            <asp:Label ID="lblmsg" runat="server" Font-Size="20px" ForeColor="Red" Text=""></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </section>
</asp:Content>
