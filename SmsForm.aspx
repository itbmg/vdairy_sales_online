<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SmsForm.aspx.cs" Inherits="SmsForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
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
            SMS Form<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">SMS Form</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>SMS Details
                </h3>
            </div>
            <div class="box-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table align="center">
                            <tr>
                                <td>
                                    <label>Route Name:</label>
                                </td>
                                <td style="height:40px;">
                                    <asp:DropDownList ID="ddlRouteName" runat="server" CssClass="form-control" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlRouteName_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Agent Name:</label>
                                </td>
                                <td style="height:40px;">
                                    <asp:DropDownList ID="ddlAgentName" runat="server" CssClass="form-control" AutoPostBack="true">
                                    <asp:ListItem disabled Selected>Select Agent Name</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td style="height:40px;">
                                    <asp:TextBox runat="server" ID="MySMSBox" TextMode="MultiLine" Rows="10" Columns="50"  placeholder="Enter Message"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td style="height:40px;">
                                    <asp:Button ID="btnGenerate"  Text="Send Sms" runat="server" OnClientClick="OrderValidate();" CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td >
                                </td>
                                <td>
                                    <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </section>
</asp:Content>
