<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="exporttoxml.aspx.cs" Inherits="exporttoxml" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
       
        <asp:Button ID="Button3" Text="Export To Excel" runat="server" CssClass="SaveButton"
            OnClick="btn_Export_Click" Style="height: 25px; width: 156px;" />
        <br />

</asp:Content>

