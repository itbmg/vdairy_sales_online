<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RatesManage.aspx.cs" Inherits="RatesManage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
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
            Rate Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Rate Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Rate Master Details
                </h3>
            </div>
            <div class="box-body">
                <table style="width: 100%;">
                    <tr>
                        <td style="float: left; width: 20%;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnGenerate" Text="Generate" runat="server" CssClass="btn btn-primary"
                                        OnClick="btnGenerate_Click" /></ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="float: right;">
                            <asp:Button ID="Button3" Text="Export To Excel" runat="server" CssClass="btn btn-primary"
                                OnClick="btn_Export_Click" />
                        </td>
                         <td style="width:5px;">
                        </td>
                        <td style="width: 40%;">
                            <asp:FileUpload ID="FileUploadToServer" runat="server" Style="height: 25px; font-size: 16px;" />&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Button ID="Button1" Text="Import" runat="server" CssClass="btn btn-primary"
                                OnClick="btn_Import_Click" />
                        </td>
                    </tr>
                </table>
                <div>
                    <div id="divPrint">
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
                </div>
                <asp:UpdatePanel ID="upd" runat="server">
                    <ContentTemplate>
                        <table align="center">
                            <tr>
                                <td align="center">
                                    <asp:Button ID="BtnSave" Text="Save"  runat="server" CssClass="btn btn-primary"
                                        OnClick="btn_WIDB_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print" OnClientClick="javascript:CallPrint('divPrint');"
                            runat="Server" />
                        <asp:Label ID="lblmsg" runat="server" ForeColor="Red" Text="" Font-Size="20px"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </section>
</asp:Content>
