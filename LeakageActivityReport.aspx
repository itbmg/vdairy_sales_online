<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="LeakageActivityReport.aspx.cs" Inherits="LeakageActivityReport" %>

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
            var fromDate = document.getElementById('<%=txtdate.ClientID %>').value;
            if (fromDate == "") {
                alert("Select Date");
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
            Leakage Activity<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Leakage Activity</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Leakage Activity Details
                </h3>
            </div>
            <div class="box-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <span>Sales Office</span>
                                </td>
                                <td style="height:40px;">
                                    <asp:Panel ID="PBranch" runat="server">
                                        <asp:DropDownList ID="ddlSalesOffice" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </asp:Panel>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <span>Report Type</span>
                                </td>
                                <td style="height:40px;">
                                    <asp:DropDownList ID="ddlreporttype" runat="server" CssClass="form-control" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlreporttype_SelectedIndexChanged">
                                        <asp:ListItem>Sales Man</asp:ListItem>
                                        <asp:ListItem>Routes</asp:ListItem>
                                         <asp:ListItem>Consolidated</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td id="lblroute">
                                    <span >Route/Employee</span>
                                </td>
                                <td style="height:40px;">
                                    <asp:DropDownList ID="ddlrouteoremployee" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>Type</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control">
                                        <asp:ListItem>Leaks</asp:ListItem>
                                        <asp:ListItem>Returns</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <asp:Label ID="lblctxtdat" runat="server">From Date:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtdate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtdate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <asp:Label ID="lbltodate" runat="server">To Date:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txttodate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txttodate"
                                        Format="dd-MM-yyyy HH:mm">
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
                        <br />
                        <br />
                        <asp:Panel ID="pnlHide" runat="server" Visible="false">
                            <div id="divPrint">
                                <div align="center">
                                    <div style="width: 100%;">
                                        <div style="width: 11%; float: left;">
                                            <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="95px" height="90px" />
                                            <%--  <img src="Images/VLogo.png" width="100" height="80" />--%>
                                        </div>
                                        <div style="left: 0%; text-align: center;">
                                            <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="26px" ForeColor="#0252aa"
                                                Text=""></asp:Label>
                                            <br />
                                        </div>
                                        <div align="center">
                                            <%--<span style="font-size: 18px; text-decoration: underline; color: #0252aa;">Total Leaks and
                                Returns Report</span>--%>
                                            <asp:Label ID="lblreporttitle" runat="server" Font-Bold="true" Font-Size="16px" ForeColor="#0252aa"
                                                Text=""></asp:Label>
                                        </div>
                                        <div align="center">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_selfromdate" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span style="font-size: 18px;">TO</span>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_selttodate" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblselected" runat="server" Text="" ForeColor="Red" Style="font-size: 18px;"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <table align="center">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblLeak" runat="server" Text="" ForeColor="Red" Font-Size="18px" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="grdReports" runat="server" ForeColor="White" Width="100%" CssClass="gridcls"
                                    GridLines="Both" Font-Bold="true" OnRowDataBound="grdReports_RowDataBound">
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
                            <br />
                            <br />
                            <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print" OnClientClick="javascript:CallPrint('divPrint');"
                                runat="Server" />
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl_utility.ashx">Export to XL</asp:HyperLink>
                        </asp:Panel>
                        <br />
                        <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </section>
</asp:Content>
