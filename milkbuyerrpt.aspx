<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="milkbuyerrpt.aspx.cs" Inherits="milkbuyerrpt" %>

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
            Milk Buyer <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#"><i></i>Due Details</a></li>
            <li><a href="#">Milk Buyer Transaction</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Milk Buyer Transaction Details
                </h3>
            </div>
            <div class="box-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <asp:Panel ID="PBranch" runat="server">
                                        <asp:DropDownList ID="ddlSalesOffice" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </asp:Panel>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <asp:TextBox ID="txtdate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtdate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                <%--<td style="width: 5px;">
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTodate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtTodate"
                                        Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>--%>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <asp:Label ID="lblreporttype" runat="server">Report Type:</asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlreporttype" runat="server" CssClass="form-control">
                                        <asp:ListItem>WithOut incentive</asp:ListItem>
                                         <asp:ListItem>With incentive</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <asp:Button ID="btnGenerate" Text="Generate" runat="server" OnClientClick="OrderValidate();"
                                        CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                                </td>
                            </tr>
                        </table>
                        <div id="divPrint">
                            <div style="width: 100%;">
                                <div style="width: 11%; float: left;">
                                    <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="95px" height="90px" />
                                </div>
                                <div style="left: 0%; text-align: center;">
                                    <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="26px" ForeColor="#0252aa"
                                        Text=""></asp:Label>
                                    <%--<span style="font-size: 26px; font-weight: bold; color: #0252aa;">VITA MILK DAIRY PRODUCTS
                              </span>--%>
                                    <br />
                                </div>
                                <div align="center">
                                    <span style="font-size: 18px; text-decoration: underline; color: #0252aa;">Milk Buyer
                                         REPORT </span>
                                </div>
                                <div style="width: 100%;">
                                    <br />
                                    <div>
                                        <div style="width: 40%; float: left; padding-left: 4%;">
                                            <span style="color: #0252aa; margin-right: 4%; font-size: 14px;">Date: </span>
                                            <asp:Label ID="lblDate" runat="server" ForeColor="Red" Font-Size="14px" Text=""></asp:Label>
                                        </div>
                                        <span style="color: #0252aa; font-size: 14px; margin-right: 4%;">SalesOffice Name:
                                        </span>
                                        <asp:Label ID="lblRouteName" runat="server" ForeColor="Red" Font-Size="14px" Text=""></asp:Label>
                                    </div>
                                </div>
                                <br />
                                <br />
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
                        <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print" OnClientClick="javascript:CallPrint('divPrint');"
                            runat="Server" />
                        <asp:Button ID="BtnSave" Text="Save" runat="server" CssClass="btn btn-primary" OnClick="BtnSave_Click" />
                        <br />
                        <asp:Label ID="lblmessage" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:Button ID="Button3" Text="Export To Excel" runat="server" CssClass="btn btn-primary"
                    OnClick="btn_Export_Click" />
                <br />
                <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
            </div>
        </div>
    </section>
</asp:Content>


