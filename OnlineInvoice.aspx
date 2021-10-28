<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="OnlineInvoice.aspx.cs" Inherits="OnlineInvoice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
    </script>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);

        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        Online Dc No
                    </td>
                    <td>
                        <asp:TextBox ID="txtDcNo" runat="server" CssClass="txtsize" placeholder="Enter online dc no"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnGenerate" Text="Generate" runat="server" OnClientClick="OrderValidate();"
                            CssClass="SaveButton" OnClick="btnGenerate_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelHide" runat="server" Visible="false">
                <div id="divPrint">
                    <div style="width: 100%;">
                        <div style="width: 11%; float: left;">
                            <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="95px" height="90px" />
                        </div>
                        <div style="left: 0%; text-align: center;">
                            <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="26px" ForeColor="#0252aa"
                                Text=""></asp:Label>
                            <br />
                        </div>
                        <div style="width: 100%;">
                            <span style="font-size: 18px; font-weight: bold; padding-left: 37%; text-decoration: underline;
                                color: #0252aa;">Invoice</span><br />
                            <span style="font-size: 14px; font-weight: bold; padding-left: 27%; color: #0252aa;">
                                DC No:</span>
                            <asp:Label ID="lblDcNo" runat="server" Font-Bold="true" Text="" ForeColor="Red"></asp:Label>
                            <div>
                            <table style="width: 100%;padding-left:7%;font-size:14px;">
                            <tr>
                            <td>
                             <span style="font-weight: bold;">Customer Name: </span>
                                    <asp:Label ID="lblAgent" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                            <td>
                             <span style="font-weight: bold;">Delivery Date: </span>
                                <asp:Label ID="lbl_fromDate" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                            </tr>
                            <tr>
                            <td>
                              <span style="font-weight: bold;">Mob No: </span>
                                <asp:Label ID="lblMobNo" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                            <td>
                              <span style="font-weight: bold;">Delivery Address: </span>
                              <asp:Label ID="lblAddress" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                            </tr>
                            </table>
                               <%-- <div style="width: 40%; float: left; padding-left: 7%;">--%>
                                   
                               <%-- </div>--%>
                               
                                
                            </div>
                        </div>
                    </div>
                    <asp:GridView ID="grdReports" runat="server" ForeColor="White" Width="100%" CssClass="EU_DataTable"
                        GridLines="Both" Font-Bold="true">
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                        <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                            Font-Names="Raavi" Font-Size="Small" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                        <AlternatingRowStyle HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    </asp:GridView>
                    <table>
                        <tr>
                            <td>
                                <span>Note : </span>
                            </td>
                            <td>
                                1. <span style="font-size: 13px;">Milk will be supplied only as per previous dates requirements</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                2. <span style="font-size: 13px;">Milk once sold shall not be taken back</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                3. <span style="font-size: 13px;">Additional milk be supplied subject to availability</span>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 50%;padding-left:7%;">
                                <span style="font-weight: bold; font-size: 14px;">Receiver Sign</span>
                            </td>
                            <td style="width: 50%;padding-right:7%;float:right;">
                                <span style="font-weight: bold; font-size: 14px;">Authorised Sign</span>
                            </td>
                        </tr>
                    </table>
                    <div>
                        <br />
                        <table style="padding-left: 25%;">
                            <tr>
                                <td>
                                    <span style="color: Black; font-size: 16px;">Thanks for booking,Please visit again</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span style="color: Black; font-size: 16px;">For online purchase visit</span> <span
                                        style="color: Black; font-size: 20px;">   www.vyshnavifoods.com</span>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <input type="button" class="SaveButton" value="Print" style="height: 25px; width: 120px;
        font-size: 15px;" onclick="javascript:CallPrint('divPrint');" />
    <%--<asp:Button ID="btnPrint" CssClass="SaveButton" Text="Print" Style="height: 25px;width: 120px;font-size:15px;" OnClientClick="javascript:CallPrint('divPrint');" runat="Server" />--%>
    <%-- <asp:Button ID="Button3" Text="Export To Excel" runat="server" CssClass="SaveButton"
        OnClick="btn_Export_Click" Style="height: 25px; width: 156px;" />--%>
    <br />
    <br />
    <br />
    <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
</asp:Content>
