<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Derivery_sheet.aspx.cs" Inherits="Derivery_sheet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script language="javascript">
        function CallPrint(strid) {
            //            var prtContent = document.getElementById(strid);
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
            Delivery History Report<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Delivery History Report</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Delivery History Report Details
                </h3>
            </div>
            <div class="box-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div>
                            <table>
                                <tr>
                                    <asp:Panel ID="PBranch" runat="server">
                                          <td>
                                            Sales Office:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSalesOffice" runat="server" CssClass="form-control" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlSalesOffice_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 5px;">
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlPlantDispName" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </td>
                                    </asp:Panel>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <div runat="server" id="d">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblrouten" runat="server">Route Name:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRouteName" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltxtdt" runat="server">Date:</asp:Label>
                                    </td>
                                    <td style="width: 5px;">
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
                                        <asp:Button ID="btnGenerate" Text="Generate" runat="server" OnClientClick="OrderValidate();"
                                            CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlHide" runat="server" Visible="false">
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
                                    </div>
                                    <div align="center">
                                        <span style="font-size: 18px; text-decoration: underline; color: #0252aa;">DELIVERY HISTORY REPORT</span>
                                    </div>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 15%;">
                                                Route Name
                                            </td>
                                            <td style="width: 15%;">
                                                <asp:Label ID="lblRoute" runat="server" Text="" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td style="width: 15%;">
                                                Date
                                            </td>
                                            <td style="width: 15%;">
                                                <asp:Label ID="lblDate" runat="server" Text="" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="grdReports" runat="server" CssClass="gridcls" GridLines="Both"
                                        Font-Bold="true" >
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                        <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                            Font-Names="Raavi" Font-Size="18px" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#ffffff" ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                    <div style="height:10px;"></div>
                                     <asp:GridView ID="grdbalance" runat="server" CssClass="gridcls"
                                    GridLines="Both" Font-Bold="true" >
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                        <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                            Font-Names="Raavi" Font-Size="18px" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#ffffff" ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                    <br />
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 25%;">
                                                <span style="font-weight: bold">SALESMAN SIGNATURE</span>
                                            </td>
                                            <td style="width: 25%;">
                                                <span style="font-weight: bold">DESPATCHER SIGNATURE</span>
                                            </td>
                                            <td style="width: 25%;">
                                                <span style="font-weight: bold">SALES EXECUTIVE SIGNATURE</span>
                                            </td>
                                            <td style="width: 25%;">
                                                <span style="font-weight: bold">CASHIER SIGNATURE</span>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print" OnClientClick="javascript:CallPrint('divPrint');"
                                    runat="Server" />
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl_utility.ashx">Export to XL</asp:HyperLink>
                            </asp:Panel>
                            <asp:Label ID="lblmsg" runat="server" ForeColor="Red" Text="" Font-Size="20px"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </section>
</asp:Content>
