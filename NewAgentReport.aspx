﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="NewAgentReport.aspx.cs" Inherits="NewAgentReport" %>

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
            var fromDate = document.getElementById('<%=txtFromdate.ClientID %>').value;
            var txtTodate = document.getElementById('<%=txtTodate.ClientID %>').value;
            if (fromDate == "") {
                alert("Select From Date");
                return false;
            }
            if (txtTodate == "") {
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
            New Agent Report<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">New Agent Report</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>New Agent Report Details
                </h3>
            </div>
            <div class="box-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlreporttype" runat="server" CssClass="form-control">
                                        <asp:ListItem>New Agents</asp:ListItem>
                                        <asp:ListItem>Stoped Agents</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width:5px;"></td>
                                <td>
                                    <asp:DropDownList ID="ddlSalesOffice" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </td>
                                <td style="width:5px;"></td>
                                <td>
                                    <asp:TextBox ID="txtFromdate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtFromdate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="width:5px;"></td>
                                <td>
                                    <asp:TextBox ID="txtTodate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtTodate"
                                        Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="width:5px;"></td>
                                <td>
                                    <asp:Button ID="btnGenerate" Text="Generate" runat="server" OnClientClick="OrderValidate();"
                                        CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                                </td>
                            </tr>
                        </table>
            <asp:Panel ID="PanelHide" runat="server" Visible="false">

                        <div id="divPrint">
                            <div style="width: 100%;">
                                <div style="width: 11%; float: left;">
                                    <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="100px" height="88px"/>
                                </div>
                                <div style="left: 0%; text-align: center;">
                                    <span style="font-size: 26px; font-weight: bold; color: #0252aa;">Sri Vyshnavi Foods Pvt Ltd
                                          </span>
                                    <br />Powered by Vyshnavi Dairy
                                </div>
                                <div style="width: 100%;">
                                    <span style="font-size: 18px; font-weight: bold; padding-left: 27%; text-decoration: underline;
                                        color: #0252aa;">New Agent Report</span><br />
                                    <div>
                                        <div style="width: 60%; float: left; padding-left: 7%;">
                                            <span style="font-weight: bold; color: #0252aa;">From Date: </span>
                                            <asp:Label ID="lblFromdate" runat="server" ForeColor="Red" Text=""></asp:Label>
                                        </div>
                                        <span style="font-weight: bold; color: #0252aa;">To Date: </span>
                                        <asp:Label ID="lblToDate" runat="server" ForeColor="Red" Text=""></asp:Label>
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
                          <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print"  OnClientClick="javascript:CallPrint('divPrint');"
                    runat="Server" />
                    </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
              
                <asp:Button ID="Button3" Text="Export To Excel" runat="server" CssClass="btn btn-primary"
                    OnClick="btn_Export_Click"  />
                <br />
                <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
            </div>
        </div>
    </section>
</asp:Content>
