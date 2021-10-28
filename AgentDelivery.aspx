<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="AgentDelivery.aspx.cs" Inherits="AgentDelivery" %>

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
            Agent Delivery<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Agent Delivery</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Agent Delivery Details
                </h3>
            </div>
            <div class="box-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div runat="server" id="d">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblroute" runat="server">Route Name:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRouteName" runat="server" CssClass="form-control" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlroutename_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <asp:Label ID="lblagentname" runat="server">Agent Name:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlagentname" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltxtdate" runat="server">Date:</asp:Label>
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
                                <div style="border: 2px solid gray;padding-top: 4%;" class="col-md-12">
                                <div>
                                    <table style="width:100%;">
                                        <tr>
                                            <td>
                                                <div style="text-align: right;">
                                                    <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="100px" height="72px" />
                                                </div>
                                            </td>
                                            <td>
                                                <div style="text-align: center;" >
                                                    <asp:Label ID="lblTitle" runat="server" style="font-size: 25px; font-weight: bold;" Text=""></asp:Label>
                                                    <br />
                                                    <asp:Label ID="lbladdress" runat="server" style="font-size: 14px;" Text=""></asp:Label>
                                                    
                                                    <br />
                                                    
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="width: 100%;">
                                      <div style="text-align: center;text-decoration: underline;"><span style="font-size: 18px; font-weight: bold;">Delivery Challan</span></div>
                                       <%-- <table style="width: 100%;border: 3px solid #dddddd;"  class="table table-bordered table-hover dataTable no-footer">--%>
                                        <table style="width: 100%;"  class="table table-bordered table-hover dataTable no-footer">
                                            <tr>
                                                <td style="width: 73%;">
                                                To
                                                <br />
                                                <asp:Label ID="lbl_AgentName" runat="server" ForeColor="Red" Font-Size="14px" Text=""></asp:Label>
                                                 <br />
                                                  <asp:Label ID="lblToaddress" runat="server" ForeColor="Red" Font-Size="14px" Text=""></asp:Label>
                                                  <br />
                                                  </td>
                                                  <td>
                                                <asp:Label ID="lbldcno" runat="server"  Font-Size="14px" Text="">DC Number:</asp:Label>
                                                <asp:Label ID="lbl_dcno" runat="server"  Font-Size="14px" ForeColor="Red" Text=""></asp:Label>
                                                <br />
                                                <asp:Label ID="lbldcdate" runat="server"  Font-Size="14px" Text="">DC Date:</asp:Label>
                                                <asp:Label ID="lbl_dcdate" runat="server"  Font-Size="14px" ForeColor="Red" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
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
                                   <%--<div style="width:15%;">
                                       <%-- <table style="width: 100%;border: 3px solid #dddddd;"  class="table table-bordered table-hover dataTable no-footer">--%>
                                       <%-- <table style="width: 100%;" class="table table-bordered table-hover dataTable no-footer">
                                            <tr>
                                            <td style="width: 30%;">
                                            From
                                            </br>
                                            <asp:Label ID="lbl_Title" runat="server"  Font-Size="14px" Text=""></asp:Label>
                                            </br>
                                            <asp:Label ID="lblFromAdress" runat="server" ForeColor="Red" Font-Size="14px" Text=""></asp:Label>
                                            </td>
                                            </tr>
                                        </table>--%>
                                    <%--</div>--%>
                                    <br /><br /><br /><br />
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 25%;">
                                                <span style="font-weight: bold; font-size: 12px;">RECEIVER'S SIGNATURE</span>
                                            </td>
                                            <td style="width: 25%;">
                                                <span style="font-weight: bold; font-size: 12px;">SECURITY</span>
                                            </td>
                                            <%--<td style="width: 25%;">
                                                <asp:Label ID="lbldispat" runat="server" Text="" Font-Bold="true"></asp:Label>
                                            </td>--%>
                                            <td style="width: 25%;">
                                                <span style="font-weight: bold; font-size: 12px;">AUTHORISED SIGNATURE</span>
                                            </td>
                                    </tr>
                                </table>
                                </div>
                                </div>
                                <asp:Button ID="btnPrint" Text="Print" CssClass="btn btn-primary" OnClientClick="javascript:CallPrint('divPrint');"
                                    runat="Server" />
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl_utility.ashx">Export to XL</asp:HyperLink>
                            </asp:Panel>
                            <br />
                            <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </section>
</asp:Content>
