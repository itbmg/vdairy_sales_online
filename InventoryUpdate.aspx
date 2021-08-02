<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="InventoryUpdate.aspx.cs" Inherits="InventoryUpdate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Inventory Update<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Inventory Update</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Inventory Update Details
                </h3>
            </div>
            <div class="box-body">
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
                <table style="width: 100%;">
                    <tr>
                        <td style="float: left; width: 20%;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnGenerate" Text="Generate" runat="server" CssClass="btn btn-primary"
                                        OnClick="btnGenerate_Click"  /></ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="float: right;">
                            <asp:Button ID="Button3" Text="Export To Excel" runat="server" CssClass="btn btn-success"
                                OnClick="btn_Export_Click"  />
                        </td>
                        <td style="width:5px;"></td>
                        <td style="width: 40%;">
                            <asp:FileUpload ID="FileUploadToServer" runat="server" Style="height: 25px; font-size: 16px;" />&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Button ID="Button1" Text="Import" runat="server" CssClass="btn btn-success" OnClick="btn_Import_Click" />
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="grdReports" runat="server" ForeColor="White" Width="100%" CssClass="gridcls"
                                GridLines="Both" Font-Bold="true" Font-Size="Smaller">
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                    Font-Names="Raavi" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                                <AlternatingRowStyle HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:UpdatePanel ID="upd" runat="server">
                    <ContentTemplate>
                        <table align="center">
                            <tr>
                                <td align="center">
                                    <asp:Button ID="BtnSave" Text="Save" Visible="false" runat="server" CssClass="btn btn-success"
                                        OnClick="btn_WIDB_Click"  />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="lblmsg" runat="server" ForeColor="Red" Text="" Font-Size="20px"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </section>
</asp:Content>
