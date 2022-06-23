<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="totaldcmail.aspx.cs" Inherits="totaldcmail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script language="javascript">
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body style="font-size:20px;"  onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        function OrderValidate() {
            var fromDate = document.getElementById('<%=txtfromdate.ClientID %>').value;
            if (fromDate == "") {
                alert("Select Date");
                return false;
            }
            var todate = document.getElementById('<%=txttodate.ClientID %>').value;
            if (todate == "") {
                alert("Select To Date");
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
            ProductWise TotalDC <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Reports</a></li>
            <li><a href="#"><i></i>Despatch</a></li>
            <li><a href="#">ProductWise TotalDC</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>ProductWise TotalDC Details
                </h3>
            </div>
            <div class="box-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <span>Report Type :</span>
                                    <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control">
                                        <asp:ListItem>Milk Curd & BM</asp:ListItem>
                                        <asp:ListItem>Other Products</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 6px">
                                </td>
                                <td>
                                    <asp:Label ID="lblfromdate" runat="server">From Date:</asp:Label>
                                    <asp:TextBox ID="txtfromdate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="fromdate_CalendarExtender1" runat="server" Enabled="True"
                                        TargetControlID="txtfromdate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="width: 6px">
                                </td>
                                <td>
                                    <asp:Label ID="lbltodate" runat="server">To Date:</asp:Label>
                                    <asp:TextBox ID="txttodate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txttodate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="width: 6px">
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
                                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="120px"
                                            height="135px" />
                                    </div>
                                    <div style="left: 0%; text-align: center;">
                                        <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="26px" ForeColor="#0252aa"
                                            Text=""></asp:Label>
                                        <br />
                                        <div style="width: 100%;">
                                            <span style="font-size: 18px; font-weight: bold; text-decoration: underline; color: #0252aa;">
                                                Total DC Report</span><br />
                                            <div>
                                            </div>
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
                                        </div>
                                        <asp:GridView ID="grdtotal_dcReports" runat="server" ForeColor="White" Width="100%"
                                            CssClass="table table-bordered table-hover dataTable no-footer" GridLines="Both"
                                            Font-Bold="true" Font-Size="Smaller" OnRowCreated="grdtotal_dcReports_RowCreated">
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                            <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                                Font-Names="Raavi" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                                            <AlternatingRowStyle HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print" OnClientClick="javascript:CallPrint('divPrint');"
                                runat="Server" />
                            <asp:Button ID="Button3" Text="Export To Excel" runat="server" CssClass="btn btn-primary"
                                OnClick="btn_Export_Click" />
                            <br />
                            Mobile No
                            <asp:TextBox ID="txtMobNo" runat="server" Width="120px" CssClass="form-control" placeholder="Enter Mob No"></asp:TextBox>
                            <asp:Button ID="btnSMS" Text="Send SMS" runat="server" CssClass="btn btn-primary"
                                OnClick="btnSMS_Click" />
                            <br />
                            <asp:Button ID="Button1" CssClass="btn btn-primary" Text="Send Mail" OnClick="SendEmail"
                                runat="Server" />
                            <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>--%>
                <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
            </div>
        </div>
    </section>
</asp:Content>
