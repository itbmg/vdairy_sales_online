<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ReturnDC.aspx.cs" Inherits="ReturnDC" %>

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
            Return DC<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Reports</a></li>
            <li><a href="#"><i></i>Despatch</a></li>
            <li><a href="#">Return DC</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Return DC Details
                </h3>
            </div>
            <div class="box-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div runat="server" id="d">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblfromdate" runat="server">From Date:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                        <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="txtdate" Format="dd-MM-yyyy HH:mm">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td style="width:6px;"></td>
                                    <td>
                                        <asp:Label ID="lbltodate" runat="server">To Date:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txttodate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                        <asp:CalendarExtender ID="todate_calendarextender" runat="server" Enabled="true"
                                            TargetControlID="txttodate" Format="dd-MM-yyyy HH:mm">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td style="width:6px;">
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_getdetails" Text="Get Trip Details" runat="server" CssClass="btn btn-primary"
                                            OnClick="btn_getdetails_Click" />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <div id="divtripdata" align="center" style="height: 180px; width: 100%; text-align: center;
                                            overflow: auto;">
                                            <asp:GridView ID="Gridtripdata" runat="server" CellPadding="4" ForeColor="#333333"
                                                GridLines="None">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <EditRowStyle BackColor="#999999" />
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbldateValidation" runat="server" Font-Size="20px" ForeColor="Red"
                                            Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <div>
                                            <table id="tbltrip">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_tripid" runat="server">DC No:</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_tripid" runat="server" Width="205px" CssClass="form-control" placeholder="Enter DC No"></asp:TextBox>
                                                    </td>
                                                    <td style="width:6px;">
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnGenerate" runat="server" CssClass="btn btn-primary" OnClick="btnGenerate_Click"
                                                            OnClientClick="OrderValidate();" Text="Generate" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <br></br>
                            <asp:Panel ID="pnlHide" runat="server" Visible="false">
                                <div id="divPrint">
                                    <div style="width: 100%;">
                                        <div style="width: 13%; float: left;">
                                            <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="95px" height="90px" />
                                        </div>
                                        <div style="width: 86%; float: right; text-align: center;">
                                            <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="26px" ForeColor="#0252aa"
                                                Text=""></asp:Label>
                                            <br />
                                        </div>
                                        <div style="width: 100%;">
                                            <span style="font-size: 18px; font-weight: bold; padding-left: 25%; text-decoration: underline;">
                                                RETURN DELIVERY CHALLAN / INVOICE</span><br />
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 30%; padding-left: 12%;">
                                                        <span style="font-weight: bold;">DC No: </span>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbldcno" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                    </td>
                                                    <td style="left: 30%;">
                                                        <span style="font-weight: bold;">Date:</span>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblassigndate" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                    </td>
                                                    <td style="left: 70%;">
                                                        <span style="font-weight: bold;">Time:</span>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbldisptime" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 30%; padding-left: 12%;">
                                                    <span style="font-weight: bold;">Party Name:</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblpartyname" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td style="left: 30%;">
                                                    <span style="font-weight: bold;">Route Name:</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblroutename" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td style="left: 70%;">
                                                    <span style="font-weight: bold;">Vehicle No:</span>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblvehicleno" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <asp:GridView ID="grdReports" runat="server" CellPadding="5" CellSpacing="5" CssClass="gridcls"
                                        ForeColor="White" GridLines="Both" Height="400px" Width="100%" Font-Size="Small">
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                        <HeaderStyle BackColor="#f4f4f4" Font-Bold="true" Font-Italic="False" Font-Names="Raavi"
                                            Font-Size="13px" ForeColor="Black" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                    <br />
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 25%;">
                                                <span style="font-weight: bold; font-size: 12px;">RECEIVER'S SIGNATURE</span>
                                            </td>
                                            <td style="width: 25%;">
                                                <span style="font-weight: bold; font-size: 12px;">SECURITY</span>
                                            </td>
                                            <td style="width: 25%;">
                                                <span style="font-weight: bold; font-size: 12px;">DISPATCHER</span>
                                            </td>
                                            <td style="width: 25%;">
                                                <span style="font-weight: bold; font-size: 12px;">MANAGER</span>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <br />
                                <asp:Button ID="btnPrint" runat="Server" CssClass="btn btn-primary" OnClientClick="javascript:CallPrint('divPrint');"
                                    Text="Print" />
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl_utility.ashx">Export to XL</asp:HyperLink>
                            </asp:Panel>
                            <br />
                            <asp:Label ID="lblmsg" runat="server" Font-Size="20px" ForeColor="Red" Text=""></asp:Label>
                        </br> </br> </br> </br>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </section>
</asp:Content>
