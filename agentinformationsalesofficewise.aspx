<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="agentinformationsalesofficewise.aspx.cs" Inherits="agentinformationsalesofficewise" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function CallPrint(strid) {
            //            var prtContent = document.getElementById(strid);
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        function OrderValidate() {
         
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
    <section class="content-header">
        <h1>
            Agent Avg Sale<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Agent Avg Sale</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Agent Avg Sale Details
                </h3>
            </div>
            <div class="box-body">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlSalesOffice" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                   
                    <td style="width:5px;"></td>
                    <td>
                        <asp:Button ID="btnGenerate" Text="Generate" runat="server" OnClientClick="OrderValidate();"
                            CssClass="btn btn-primary" OnClick="btnGenerate_Click"  />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="PanelHide" runat="server" Visible="false">
                <div id="divPrint">
                    <div style="width: 100%;">
                        <div style="width: 11%; float: left;">
                            <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="95px" height="90px"/>
                            <%--  <img src="Images/VLogo.png" width="100" height="80" />--%>
                        </div>
                        <div style="left: 0%; text-align: center;">
                            <br />
                            <div style="width: 100%;">
                                <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="26px" ForeColor="#0252aa"
                                    Text=""></asp:Label><br />
                                <div>
                                </div>
                            </div>
                            <div align="center">
                                <span style="font-size: 18px; text-decoration: underline; color: #0252aa;">Route Wise
                                    Delivery</span>
                            </div>
                            <div align="center">
                                <table style="width: 50%;">
                                    <tr>
                                        <td>
                                            Sales Office Name:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRoutName" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td>
                                            Date:
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_selfromdate" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td>
                                            <span style="font-size: 18px;">To</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_selttodate" runat="server" Style="font-size: 11px;" Text="" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:GridView ID="grdReports" runat="server" ForeColor="White" Width="100%" CssClass="gridcls"
                                GridLines="Both" Font-Bold="true">
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" HorizontalAlign="Left" />
                                <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                    Font-Names="Raavi" Font-Size="Small" HorizontalAlign="center" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="left" />
                                <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Left" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" HorizontalAlign="Left" />
                            </asp:GridView>
                        </div>
                        <br />
                        <br />
                    </div>
                </div>
                <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print"  OnClientClick="javascript:CallPrint('divPrint');"
                    runat="Server" />
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl_utility.ashx">Export to XL</asp:HyperLink>
                <br />
            </asp:Panel>
            <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    </div>
    </section>
</asp:Content>
