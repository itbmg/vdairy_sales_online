<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="IncentiveStructureApproval.aspx.cs" Inherits="IncentiveStructureApproval" %>

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
    </script>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600">
    </asp:ToolkitScriptManager>
    <div>
     <section class="content-header">
        <h1>
            Incentive Structure Approval<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Cash Details</a></li>
            <li><a href="#">Incentive Structure Approval</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Incentive Structure Approval Details
                </h3>
            </div>
            <div class="box-body">
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
            <table>
                <tr>
                <td>
                    <asp:Panel ID="Panel2" runat="server">
                            <asp:DropDownList ID="ddlrptType" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlrptType_SelectedIndexChanged">
                            <asp:ListItem>Raised</asp:ListItem>
                            <asp:ListItem>Approved</asp:ListItem>
                            <asp:ListItem>Pending</asp:ListItem>
                            <asp:ListItem>Rejected</asp:ListItem>
                            </asp:DropDownList>
                        </asp:Panel>
                    </td>
                    <td style="width:5px;"></td>
                    <td>
                        <asp:Panel ID="Panel1" runat="server">
                            <asp:DropDownList ID="ddlSalesOffice" runat="server" CssClass="form-control" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlSalesOffice_SelectedIndexChanged">
                            </asp:DropDownList>
                        </asp:Panel>
                    </td>
                    <td style="width:5px;"></td>
                    
                    <td>
                        <asp:Panel ID="PBranch" runat="server">
                            <asp:DropDownList ID="ddlIncentiveStructure" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </asp:Panel>
                    </td>
                    <td style="width:5px;"></td>
                    <td>
                        <asp:Button ID="btnGenerate" Text="Generate" runat="server" OnClientClick="OrderValidate();"
                            CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                    </td>
                </tr>
            </table>
            <div id="divPrint">
                <div style="width: 100%;">
                    <div style="width: 11%; float: left;">
                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="95px" height="90px" />
                    </div>
                    <div style="left: 0%; text-align: center;">
                        <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="26px" ForeColor="#0252aa"
                            Text=""></asp:Label>
                    </div>
                    <div align="center">
                        <span style="font-size: 18px; text-decoration: underline; color: #0252aa;">Incentive Structure Report </span>
                    </div>
                    <div style="width: 100%;">
                        <br />
                        <div>
                            <div style="width: 40%; float: left; padding-left: 4%;">
                                <span style="color: #0252aa; margin-right: 4%; font-size: 14px;">Approved Date: </span>
                                <asp:Label ID="lblDate" runat="server" ForeColor="Red" Font-Size="14px" Text=""></asp:Label>
                            </div>
                            <span style="color: #0252aa; font-size: 14px; margin-right: 4%;">Structure Name:
                            </span>
                            <asp:Label ID="lblStructureName" runat="server" ForeColor="Red" Font-Size="14px"
                                Text=""></asp:Label>
                        </div>
                    </div>
                    <br />
                    <br />
                </div>
                <asp:GridView ID="grdReports" runat="server" ForeColor="White" Width="100%" CssClass="EU_DataTable"
                    GridLines="Both" Font-Bold="true" OnRowDataBound="grdReports_RowDataBound">
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
             <br />
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 25%;">
                                <span style="font-weight: bold">Verified By</span>
                            </td>
                            <td style="width: 25%;">
                                <span style="font-weight: bold">Approved By:</span>
                                 <asp:Label ID="lblApprovedBy" runat="server" Font-Bold="true" Font-Size="26px" ForeColor="#0252aa"
                            Text=""></asp:Label>
                            </td>
                            <td style="width: 25%;">
                                
                            </td>
                            
                        </tr>
                    </table>
            <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print"  OnClientClick="javascript:CallPrint('divPrint');"
                runat="Server" />
            <asp:Button ID="btnApprove" CssClass="btn btn-primary" Text="Approval"  runat="Server" OnClick="btnApprove_Click" />
                <asp:Button ID="btnReject" CssClass="btn btn-primary" Text="Reject"  runat="Server" OnClick="btnReject_Click" />
            <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    </section>
    </div>
    
</asp:Content>
