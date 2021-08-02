<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StoppedAgentReport.aspx.cs" Inherits="StoppedAgentReport" %>

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
            Stopped Agent Report<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Reports</a></li>
            <li><a href="#">Sales</a></li>
            <li><a href="#">Stopped Agent Report</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Stopped Agent Details
                </h3>
            </div>
            <div class="box-body">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                            <tr>
                                <td>
                                    <td>
                                        <asp:Panel ID="PBranch" runat="server">
                                            <asp:DropDownList ID="ddlSalesOffice" runat="server" CssClass="form-control" >
                                            </asp:DropDownList>
                                        </asp:Panel>
                                    </td>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromdate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtFromdate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTodate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                        TargetControlID="txtTodate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <asp:Button ID="btnGenerate" Text="Generate" runat="server" OnClientClick="OrderValidate();"
                                        CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                                </td>
                            </tr>
                        </table>
             <asp:Panel ID="pnlHide" runat="server" Visible="false">
                <div id="divPrint">
                    <div style="width: 100%;">
                        <div style="width: 11%; float: left;">
                          <img src="Images/Vyshnavilogo.png" alt="VT Milk" width="100px"
                                            height="72px" />
                        </div>
                        <div style="left: 0%; text-align: center;">
                           <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="26px" ForeColor="#0252aa" Text=""></asp:Label>
                            <br />
                        </div>
                        <div style="width: 100%;">
                            <span style="font-size: 18px; font-weight: bold; padding-left: 27%; text-decoration: underline;
                                color: #0252aa;">Stopped Agent Report</span><br />
                            <div>
                                <div style="width: 60%; float: left; padding-left: 7%;">
                                    <span style="font-weight: bold; color: #0252aa;">From Date: </span>
                                    <asp:Label ID="lblFromDate" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </div>
                                <span style="font-weight: bold; color: #0252aa;">To Date: </span>
                                <asp:Label ID="lblToDate" runat="server" ForeColor="Red" Text=""></asp:Label>
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
                </div>
                </asp:Panel>
                             <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="javascript:CallPrint('divPrint');"><i class="fa fa-print"></i> Print </button>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl_utility.ashx">Export to XL</asp:HyperLink>
                <br />
                <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    </div>
    </section>
</asp:Content>
