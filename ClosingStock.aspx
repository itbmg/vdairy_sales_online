<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ClosingStock.aspx.cs" Inherits="Test" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
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
        function PopupOpen() {
            $('#divMainAddNewRow').css('display', 'block');
        }
        function popupCloseClick() {
            $('#divMainAddNewRow').css('display', 'none');
        }
    </script>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);

        });
    </script>
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        .Grid td
        {
            background-color: #A1DCF2;
            color: black;
            font-size: 10pt;
            line-height: 200%;
        }
        .Grid th
        {
            background-color: #3AC0F2;
            color: White;
            font-size: 10pt;
            line-height: 200%;
        }
        .ChildGrid td
        {
            background-color: #eee !important;
            color: black;
            font-size: 10pt;
            line-height: 200%;
        }
        .ChildGrid th
        {
            background-color: #6C6C6C !important;
            color: White;
            font-size: 10pt;
            line-height: 200%;
        }
        .Nested_ChildGrid td
        {
            background-color: #fff !important;
            color: black;
            font-size: 10pt;
            line-height: 200%;
        }
        .Nested_ChildGrid th
        {
            background-color: #2B579A !important;
            color: White;
            font-size: 10pt;
            line-height: 200%;
        }
    </style>
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
            Closing Stock<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Closing Stock</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Closing Stock Details
                </h3>
            </div>
            <div class="box-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlSalesOffice" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </td>
                                 <td style="width:6px;">
                                </td>
                                <td>
                                    <asp:TextBox ID="txtdate" runat="server" Width="205px" CssClass="form-control"> </asp:TextBox>
                                    <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtdate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                                <br />
                        <table>
                            <tr>
                                <td>
                                    <div style="width: 25%; float: left;">
                                        <asp:Button ID="Button2" runat="server" Text="Products" CssClass="btn btn-primary" OnClick="btnGenerate_Click"
                                            />
                                    </div>
                                </td>
                                <td style="width:6px;">
                                </td>
                                <td>
                                    <div style="width: 44%; float: left;">
                                        <asp:Button ID="Button1" runat="server" Text="Inventory" CssClass="btn btn-primary" OnClick="btninventory_Click"
                                           />
                                    </div>
                                </td>
                                <td style="width:6px;">
                                </td>
                                <%--<td>
                                    <div style="width: 44%; float: left;">
                                        <asp:Button ID="Button3" runat="server" Text="Collections" CssClass="btn btn-primary"
                                             />
                                    </div>
                                </td>--%>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlHide" runat="server" Visible="false">
                            <div id="divPrint">
                                <div style="width: 100%;">
                                    <div style="width: 11%; float: left;">
                                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="95px" height="90px" />
                                    </div>
                                    <br />
                                    <br />
                                    <br />
                                    <div style="left: 0%; text-align: center;">
                                        <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="26px" ForeColor="#0252aa"
                                            Text=""></asp:Label>
                                        <br />
                                    </div>
                                    <div style="width: 100%;">
                                        <br />
                                    </div>
                                    <br />
                                    <br />
                                </div>
                                <asp:GridView ID="grdReports" runat="server" Style="width: 100%;" Font-Bold="true"
                                    OnRowDataBound="grdReports_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="imgOrdersShow" runat="server" OnClick="Show_Hide_OrdersGrid"
                                                                CommandArgument="Show" ImageUrl="~/images/plus.png" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <br />
                            <br />
                            <br />
                            <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print"  OnClientClick="javascript:CallPrint('divPrint');"
                                runat="Server" />
                            <asp:Button ID="BtnSave" Text="Save" runat="server" CssClass="btn btn-primary" OnClick="BtnSave_Click"/>
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl_utility.ashx">Export to XL</asp:HyperLink>
                        </asp:Panel>
                        <br />
                        <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div id="divMainAddNewRow" class="pickupclass" style="text-align: center; height: 100%;
                    width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
                    background: rgba(192, 192, 192, 0.7);">
                    <div id="divAddNewRow" style="border: 5px solid #A0A0A0; position: absolute; top: 30%;
                        background-color: White; width: 100%; height: 50%; -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                        -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                        border-radius: 10px 10px 10px 10px;">
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="GrdProducts" runat="server" ForeColor="White" Width="100%" CssClass="gridcls"
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="divclose" style="width: 35px; top: 24.5%; right: .5%; position: absolute;
                        z-index: 99999; cursor: pointer;">
                        <img src="Images/Odclose.png" alt="close" onclick="popupCloseClick();" />
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
