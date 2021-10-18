﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="dueapprovalReport.aspx.cs" Inherits="dueapprovalReport" %>

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
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600">
    </asp:ToolkitScriptManager>
    <div>
        <asp:UpdateProgress ID="updateProgress1" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0;
                    right: 0; left: 0; z-index: 9999; background-color: #FFFFFF; opacity: 0.7;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="thumbnails/301.gif" Style="padding: 10px;
                        position: absolute; top: 40%; left: 40%; z-index: 99999;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <div>
        <table>
            <tr>
                <td nowrap>
                    <label for="lblBranch">
                        SalesOffice</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSalesOffice" runat="server" CssClass="txtsizee">
                    </asp:DropDownList>
                </td>
                <td nowrap>
                    <label for="lblBranch">
                        Indent Date</label>
                </td>
                <td>
                    <asp:TextBox ID="txtdate" runat="server" Width="205px"></asp:TextBox>
                    <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                        TargetControlID="txtdate" Format="dd-MM-yyyy HH:mm">
                    </asp:CalendarExtender>
                </td>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnGenerate" Text="Generate" runat="server" OnClientClick="OrderValidate();"
                        CssClass="SaveButton" OnClick="btnGenerate_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div id="divPrint">
    <div>
     <div align="center">
                    <div style="width: 100%;">
                        <div style="width: 11%; float: left;">
                            <img src="Images/Vyshnavilogo.png" alt="VT Milk" width="95px" height="90px" />
                        </div>
                        <div style="left: 0%; text-align: center;">
                            <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="26px" ForeColor="#0252aa"
                                Text=""></asp:Label>
                            <br />
                        </div>
                        <div align="center">
                            <span style="font-size: 18px; text-decoration: underline; color: #0252aa;">DUE APPROVAL REPORT</span>
                        </div>
                        <div style="width: 100%;">
                            <br />
                            <div>
                                <div style="width: 40%; float: left; padding-left: 4%;">
                                    <span style="color: #0252aa; margin-right: 4%; font-size: 14px;">Date: </span>
                                    <asp:Label ID="lblDate" runat="server" ForeColor="Red" Font-Size="14px" Text=""></asp:Label>
                                </div>
                                <span style="color: #0252aa; font-size: 14px; margin-right: 4%;">SalesOffice Name:
                                </span>
                                <asp:Label ID="lblDispatchName" runat="server" ForeColor="Red" Font-Size="14px" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
    <asp:GridView ID="grdReports" runat="server" ForeColor="White" Width="100%" CssClass="gridcls"
                    GridLines="Both" Font-Bold="true" Font-Size="Smaller" >
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                    <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                        Font-Names="Raavi" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                    <AlternatingRowStyle HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                </asp:GridView>
                <br />
            <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>

    </div>
    </div>
    <div>
                <asp:Button ID="btnPrint" CssClass="SaveButton" Text="Print" Style="height: 25px;
                    width: 120px;font-size:20px;" OnClientClick="javascript:CallPrint('divPrint');" runat="Server" />
                <br />
                <br />
            </div>
</asp:Content>
