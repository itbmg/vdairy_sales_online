﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DaywiseMilkSales.aspx.cs" Inherits="Day_wise_Milk_Sales" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <style>
        .HeaderStyle
        {
            border: solid 1px White;
            background-color: #81BEF7;
            font-weight: bold;
            text-align: center;
        }
    </style>
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
    <style type="text/css">
        th {
            text-align: center !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600">
    </asp:ToolkitScriptManager>
    <div>
        <asp:UpdateProgress ID="updateProgress1" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999; background-color: #FFFFFF; opacity: 0.7;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="thumbnails/loading.gif"
                        Style="padding: 10px; position: absolute; top: 40%; left: 40%; z-index: 99999;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <section class="content-header">
        <h1>Daywise Milk sales<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Daywise Milk sales</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Daywise Milk sales Details
                </h3>
            </div>
            <div class="box-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <%-- <td>
                                    <span>Category</span>
                                    <asp:CheckBox ID="chkCategory" runat="server" CssClass="form-control" AutoPostBack="True" OnCheckedChanged="chkChangedCtegory"></asp:CheckBox>
                                </td>
                                <td style="width: 5px;"></td>
                                <td>
                                    <span>SubCategory</span>
                                    <asp:CheckBox ID="chkSubCategory" runat="server" CssClass="form-control" AutoPostBack="True" OnCheckedChanged="chkChangedCtegory"></asp:CheckBox>
                                </td>--%>
                                <td>
                                    <asp:DropDownList ID="ddlType" runat="server"   CssClass="form-control" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                                        AutoPostBack="True">
                                        <asp:ListItem Value="AgentWise" Text="AgentWise">AgentWise</asp:ListItem>
                                        <asp:ListItem Value="SalesOffice" Text="SalesOffice">SalesOffice</asp:ListItem>
                                        <%--<asp:ListItem Value="RouteWise" Text="RouteWise">RouteWise</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5px;"></td>
                                <td>
                                    <asp:DropDownList ID="ddlcatType" runat="server"   CssClass="form-control" OnSelectedIndexChanged="ddlcatType_SelectedIndexChanged"
                                        AutoPostBack="True">
                                        <asp:ListItem Value="Category" Text="Category">Category</asp:ListItem>
                                        <asp:ListItem Value="SubCategory" Text="SubCategory">SubCategory</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5px;"></td>
                                <td>
                                    <span>PktWise</span>
                                    <asp:CheckBox ID="chkPktOrLtr" runat="server"   CssClass="form-control" AutoPostBack="True"  ></asp:CheckBox>
                                </td>
                            </tr>
                            <tr>
                                <asp:Panel ID="PBranch" runat="server">
                                    <td>
                                        <span>SalesOffice</span>
                                        <asp:DropDownList ID="ddlSalesOffice" runat="server"    CssClass="form-control" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlSalesOffice_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5px;"></td>
                                </asp:Panel>

                                <asp:Panel ID="PRoute" runat="server">
                                    <td>
                                        <span>Route</span>
                                        <asp:DropDownList ID="ddlDispName" runat="server"   CssClass="form-control" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlDispName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5px;"></td>
                                </asp:Panel>
                                <asp:Panel ID="PAgent" runat="server">
                                    <td>
                                        <span>Agent</span>
                                        <asp:DropDownList ID="ddlAgentName" runat="server"  CssClass="form-control">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5px;"></td>
                                </asp:Panel>
                                <asp:Panel ID="PCategory" runat="server">
                                    <td>
                                        <span>Report Type :</span>
                                        <asp:DropDownList ID="ddlReportType" runat="server"   CssClass="form-control" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged"
                                            AutoPostBack="True">
                                            <asp:ListItem Value="1" Text="Milk">Milk</asp:ListItem>
                                            <asp:ListItem Value="2" Text="Curd">Curd</asp:ListItem>
                                            <asp:ListItem Value="7" Text="Ghee">Ghee</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5px;"></td>
                                </asp:Panel>
                                <asp:Panel ID="PSubCategory" runat="server">
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <span>SubCategory:</span>
                                        <asp:DropDownList ID="ddlSubCategoryName" runat="server"    CssClass="form-control">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5px;"></td>
                                </asp:Panel>
                                <td>
                                    <asp:Label ID="lblfromdate" runat="server">From Date:</asp:Label>
                                    <asp:TextBox ID="txtfromdate" runat="server" Width="100px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="fromdate_CalendarExtender1" runat="server" Enabled="True"
                                        TargetControlID="txtfromdate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="width: 5px;"></td>
                                <td>
                                    <asp:Label ID="lbltodate" runat="server">To Date:</asp:Label>
                                    <asp:TextBox ID="txttodate" runat="server" Width="100px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txttodate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="width: 5px;"></td>
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
                                        <div style="width: 100%;">
                                            <span style="font-size: 18px; font-weight: bold; text-decoration: underline; color: #0252aa;">Day Wise Milk Sales Report</span><br />
                                            <div>
                                            </div>
                                        </div>
                                        <div align="center">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_selfromdate" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                    </td>
                                                    <td></td>
                                                    <td>
                                                        <span style="font-size: 18px;">TO</span>
                                                    </td>
                                                    <td></td>
                                                    <td>
                                                        <asp:Label ID="lbl_selttodate" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:GridView ID="grdtotal_dcReports" runat="server" ForeColor="White" Width="100%"
                                            GridLines="Both" Font-Bold="true" Font-Size="Smaller">
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                            <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                                Font-Names="Raavi" Font-Size="Small" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#ffffff" ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print" OnClientClick="javascript:CallPrint('divPrint');"
                                runat="Server" />
                            <asp:TextBox ID="txtMobNo" runat="server" Visible="false" CssClass="txtsize"></asp:TextBox>
                            <asp:Button ID="btnSMS" Text="Send SMS" runat="server" Visible="false" CssClass="btn btn-primary"
                                OnClick="btnSMS_Click" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                        <asp:Panel ID="exportPanel" runat="server" Visible="false">
                <asp:Button ID="Button3" Text="Export To Excel" runat="server"   CssClass="btn btn-primary"
                    OnClick="btn_Export_Click" />
                        </asp:Panel>
            </div>
        </div>
    </section>
</asp:Content>
