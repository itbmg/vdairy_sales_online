<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="WishingSms.aspx.cs" Inherits="WishingSms" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table align="center">
                <tr>
                    <td>
                        <asp:Label ID="lblroutename" runat="server">Type:</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="txtsizee" >
                        <asp:ListItem>Agent</asp:ListItem>
                        <asp:ListItem>Employee</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                <td></td>
                <td>
                <asp:TextBox runat="server" ID="MySMSBox" TextMode="MultiLine" Rows="10" Columns="50" />

                </td>
                </tr>
               
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnGenerate" Width="150px" Height="40px" Font-Size="Large" Text="SEND SMS" runat="server" OnClientClick="OrderValidate();"
                            CssClass="SaveButton" OnClick="btnGenerate_Click" />
                    </td>
                </tr>
                <tr>
                <td>
                </td>
                <td>
                <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
      
        
</asp:Content>

