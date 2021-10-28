<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="IncentiveReport.aspx.cs" Inherits="IncentiveReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />--%>
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
        //        $(function () {
        //            // window.history.forward(1);
        //           
        //        });
        //        function ondueamtchange() {

        //        }
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
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="thumbnails/loading.gif"
                        Style="padding: 10px; position: absolute; top: 40%; left: 40%; z-index: 99999;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <section class="content-header">
        <h1>
            Incentive Report<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Masters</a></li>
            <li><a href="#">Incentive Report</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Incentive Report Details
                </h3>
            </div>
            <div class="box-body">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table align="center">
                <tr>
                    <td>
                        <asp:Label ID="lblincentiveType" runat="server">Incentive Type:</asp:Label>
                    </td>
                    <td style="height: 40px;">
                        <asp:DropDownList ID="ddlincentivetype" runat="server" CssClass="form-control">
                            <asp:ListItem>Normal Incentive</asp:ListItem>
                            <asp:ListItem>Leakage Incentive</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblroutename" runat="server">Route Name:</asp:Label>
                    </td>
                    <td style="height: 40px;">
                        <asp:DropDownList ID="ddlRouteName" runat="server" CssClass="form-control" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlRouteName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblagentname" runat="server">Agent Name:</asp:Label>
                    </td>
                    <td style="height: 40px;">
                        <asp:DropDownList ID="ddlAgentName" runat="server" CssClass="form-control" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlAgentName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblprevincentive" runat="server">Previous Incentive:</asp:Label>
                    </td>
                    <td style="height: 40px;">
                        <asp:Label ID="txtprevDate" runat="server" Text="" ForeColor="Blue" Font-Size="20px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblstructure" runat="server">Structure Name:</asp:Label>
                    </td>
                    <td style="height: 40px;">
                        <asp:DropDownList ID="ddlstructure" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbllekage" runat="server">Leakage:</asp:Label>
                    </td>
                    <td style="height: 40px;">
                        <asp:TextBox ID="txtleakage" Text="0" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblfromdate" runat="server">From Date:</asp:Label>
                    </td>
                    <td style="height: 40px;">
                        <asp:TextBox ID="txtFromdate" runat="server"  Width="205px" class="form-control"></asp:TextBox>
                        <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                            TargetControlID="txtFromdate" Format="dd-MM-yyyy HH:mm">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbltodate" runat="server">To Date:</asp:Label>
                    </td>
                    <td style="height: 40px;">
                        <asp:TextBox ID="txtTodate" runat="server"  Width="205px" class="form-control"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtTodate"
                            Format="dd-MM-yyyy HH:mm">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnGenerate" Text="Generate" runat="server"  OnClientClick="OrderValidate();" CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
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
                        <br />
                    </div>
                    <div style="width: 100%;">
                        <div style="width: 100%;">
                            <span style="font-size: 13px; font-weight: bold; padding-left: 27%; text-decoration: underline;
                                color: #0252aa;">INCENTIVE REPORT</span><br />
                            <div>
                                <div style="float: left; padding-left: 2%;">
                                    <span style="font-weight: bold;">Agent Name: </span>
                                    <asp:Label ID="lblAgent" Style="font-size: 11px;" runat="server" Text=""></asp:Label>
                                </div>
                                <div style="float: left; padding-left: 5%;">
                                    <span style="font-weight: bold;">RouteName: </span>
                                    <asp:Label ID="lblroute" Style="font-size: 11px;" runat="server" Text=""></asp:Label>
                                </div>
                                <div style="float: left; padding-left: 5%;">
                                    <span style="font-weight: bold;">Date:</span>
                                    <asp:Label ID="lbldate" Style="font-size: 11px;" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:GridView ID="grdReports" runat="server" ForeColor="White" Width="100%" GridLines="Both"
                    Font-Size="18px">
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                    <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                        Font-Names="Raavi" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" Font-Size="18px"/>
                    <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" Font-Size="18px" />
                    <AlternatingRowStyle HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                </asp:GridView>
                <div>
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Label ID="lblactualdiscount" runat="server">ActualDiscount:</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblactualdiscount1" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblincentivegiven" runat="server">IncentiveGiven:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtincentivegiven" runat="server" Width="205px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server">Remarks:</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox TextMode="MultiLine" ID="txtremarks" runat="server" Width="605px" ReadOnly></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 25%;">
                                <span style="font-weight: bold; font-size: 14px;">Prepared By</span>
                            </td>
                            <td style="width: 25%;">
                                <span style="font-weight: bold; font-size: 14px;">Accounts</span>
                            </td>
                            <td style="width: 25%;">
                                <span style="font-weight: bold; font-size: 14px;">Marketing Manager</span>
                            </td>
                            <td style="width: 25%;">
                                <span style="font-weight: bold; font-size: 14px;">Agent Sign</span>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Label ID="lbl_warn" runat="server" Text="" ForeColor="Orange" Font-Bold="true" Font-Size="20px"></asp:Label>
                <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
            </div>
            <div>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnicentivesave" CssClass="btn btn-primary"  OnClientClick="" runat="Server" OnClick="btnicentivesave_Click" />
                            <br />
                            <br />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print"  OnClientClick="javascript:CallPrint('divPrint');" runat="Server" />
                            <br />
                            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="Button3" Text="Export To Excel" runat="server" CssClass="btn btn-primary"
        OnClick="btn_Export_Click"  />
        </div>
        </div>
        </section>
</asp:Content>
