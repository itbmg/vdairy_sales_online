<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="GatePassReport.aspx.cs" Inherits="GatePassReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script language="javascript">
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
            GatePass Report<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">GatePass Report</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>GatePass Details
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
                                    <td style="width: 5px; '">
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltodate" runat="server">To Date:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txttodate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                        <asp:CalendarExtender ID="todate_calendarextender" runat="server" Enabled="true"
                                            TargetControlID="txttodate" Format="dd-MM-yyyy HH:mm">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td style="width: 5px; '">
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_getdetails" Text="Get Gate Pass Details" runat="server" CssClass="btn btn-primary"
                                            OnClick="btn_getdetails_Click" />
                                    </td>
                                </tr>
                            </table>
                            <div id="divtripdata" align="center" style="height: 180px; width: 100%; text-align: center;
                                overflow: auto;">
                                <asp:GridView ID="Gridtripdata" runat="server" ForeColor="White" Width="100%" CssClass="gridcls"
                                    GridLines="Both" Font-Bold="true">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                            </div>
                            <asp:Label ID="lbldateValidation" runat="server" Font-Size="20px" ForeColor="Red"
                                Text=""></asp:Label>
                        </div>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <div>
                                            <table id="tbltrip">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_tripid" runat="server">Gate Pass Ref No:</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_gatePassid" runat="server" CssClass="inputText"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 5px; '">
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
                                    <div style="width: 13%; float: left;">
                                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="95px" height="90px" />
                                    </div>
                                    <div align="center">
                                        <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="20px" ForeColor="#0252aa"
                                            Text=""></asp:Label>
                                        <br />
                                        <asp:Label ID="lblAddress" runat="server" Font-Bold="true" Font-Size="12px" ForeColor="#0252aa"
                                            Text=""></asp:Label>
                                        <br />
                                    </div>
                                    <div align="center">
                                        <span style="font-size: 16px; font-weight: bold; text-decoration: underline; color: #0252aa;">
                                            GATE PASS DETAILS</span><br />
                                        Tin No.
                                        <asp:Label ID="lbltinNo" runat="server" Font-Bold="true" Font-Size="14px" ForeColor="#0252aa"
                                            Text=""></asp:Label>
                                              <br />
                                       Ref No
                                        <asp:Label ID="lblrefno" runat="server" Font-Bold="true" Font-Size="14px" ForeColor="#0252aa"
                                            Text=""></asp:Label>
                                        <br />
                                        Gate Pass No
                                        <asp:Label ID="lblGatePassNo" runat="server" Font-Bold="true" Font-Size="14px" ForeColor="#0252aa"
                                            Text=""></asp:Label>
                                    </div>
                                    <div>
                                        <div style="width: 60%; float: left;">
                                            <span>To,</span>
                                            <br />
                                            <span>The Security</span>
                                            <br />
                                            <span>Name Of the Person : </span>
                                            <asp:Label ID="lblName" runat="server" Font-Bold="true" Font-Size="14px" ForeColor="#0252aa"
                                                Text=""></asp:Label>
                                        </div>
                                        <div style="left: 61%; position: absolute;">
                                            <span style="width: 100px; margin-top: 2px;">Date</span>
                                            <br />
                                            <span style="width: 100px; margin-top: 2px; line-height: 14px;">Time</span>
                                            <br />
                                            <span style="width: 100px; margin-top: 2px;">Vehicle No</span>
                                        </div>
                                        <div style="left: 71%; position: absolute;">
                                            <asp:Label ID="lblDate" runat="server" Font-Bold="true" Font-Size="14px" ForeColor="#0252aa"
                                                Text=""></asp:Label>
                                            <br />
                                            <asp:Label ID="lblTime" runat="server" Font-Bold="true" Font-Size="14px" ForeColor="#0252aa"
                                                Text=""></asp:Label>
                                            <br />
                                            <asp:Label ID="lblVehicleNo" runat="server" Font-Bold="true" Font-Size="14px" ForeColor="#0252aa"
                                                Text=""></asp:Label>
                                        </div>
                                    </div>
                                    <asp:GridView ID="grdReports" runat="server" ForeColor="White" Width="100%" GridLines="Both"
                                        Font-Size="Smaller">
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                        <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                            Font-Names="Raavi" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                                        <AlternatingRowStyle HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                                    </asp:GridView>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width:33%; float: left; font-weight: bold; font-size: 12px;">
                                                Receiver's Signature
                                            </td>
                                            <td style="width: 33%; float: right; font-weight: bold; font-size: 12px;">
                                                Issuing Authority
                                            </td>
                                              <td style="width: 33%; float: right; font-weight: bold; font-size: 12px;">
                                                Security Sign
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <br />
                            </asp:Panel>
                            <asp:Button ID="btnPrint" runat="Server" CssClass="btn btn-primary" OnClientClick="javascript:CallPrint('divPrint');"
                                Text="Print" />
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl_utility.ashx">Export to XL</asp:HyperLink>
                            <br />
                            <asp:Label ID="lblmsg" runat="server" Font-Size="20px" ForeColor="Red" Text=""></asp:Label>
                            </br> </br> </br> </br>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </section>
</asp:Content>
