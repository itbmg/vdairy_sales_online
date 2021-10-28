<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ReceiptBook.aspx.cs" Inherits="ReceiptBook" %>

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
    <script language="javascript">
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=300,height=300,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.write('<link rel="stylesheet" type="text/css" href="Css/print.css" />');
            newWin.document.close();
        }
    </script>
    <style type="text/css">
        .mylbl
        {
            font-size: 12px;
        }
        .mylbl1
        {
            font-size: 12px;
        }
        .mylbl12
        {
            font-size: 14px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);

        });

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
            Receipt<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#"><i></i>Cash details</a></li>
            <li><a href="#">Receipt</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Receipt Details
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
                                                        <asp:Label ID="lbl_tripid" runat="server" CssClass="lblClass">Receipt No:</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtReceiptNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:HiddenField ID="txthiddentype" runat="server" />
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
                                            <span class="mylbl1">RECEIPT</span><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="font-size: 14px; font-weight: bold; float: right;">No:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblreceiptno" runat="server" CssClass="mylbl1" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <span style="font-size: 14px; font-weight: bold; float: right;">Date:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDate" runat="server" CssClass="mylbl1" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <table>
                                        <tr>
                                            <td>
                                                <span class="mylbl1">Received with thanks from M/s./Mr </span>
                                            </td>
                                            <td style="float: left;">
                                                <asp:Label ID="lblPayCash" runat="server" CssClass="mylbl1" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="mylbl1">the sum of Rupees</span>
                                            </td>
                                            <td style="float: left;">
                                                <asp:Label ID="lblRupess" runat="server" CssClass="mylbl1" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="mylbl1">towards</span>
                                            </td>
                                            <td style="float: left;">
                                                <asp:Label ID="lbltowards" runat="server" CssClass="mylbl1" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="mylbl1">vide cash/cheque No</span>
                                            </td>
                                            <td style="float: left;">
                                                <asp:Label ID="lblCheque" runat="server" CssClass="mylbl1" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="mylbl1">Date</span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblChequeDate" runat="server" CssClass="mylbl1" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="mylbl1">Remarks</span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRemarks" runat="server" CssClass="mylbl1" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <div>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 25%;">
                                                <span class="mylbl12">Rs.</span>
                                                <asp:Label ID="lblAmount" runat="server" class="mylbl12" Text=""></asp:Label>
                                            </td>
                                            <td style="width: 20%;">
                                            </td>
                                            <td style="width: 55%;">
                                                For
                                                <asp:Label ID="lblSignTitle" runat="server" class="mylbl12" Text=""></asp:Label>
                                                <br />
                                                <br />
                                                Signature
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 25%;">
                                                Depositor Sign
                                            </td>
                                            <td style="width: 20%;">
                                            </td>
                                            <td style="width: 55%;">
                                            </td>
                                        </tr>
                                    </table>
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
