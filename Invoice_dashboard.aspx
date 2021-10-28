<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Invoice_dashboard.aspx.cs" Inherits="Invoice_dashboard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <%--<link href="Css/style.css" rel="stylesheet" type="text/css" />--%>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />
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
            var fromDate = document.getElementById('<%=txtFromdate.ClientID %>').value;
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
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="thumbnails/loading.gif"
                        Style="padding: 10px; position: absolute; top: 40%; left: 40%; z-index: 99999;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <section class="content-header">
        <h1>
            Invoice DashBoard<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Reports</a></li>
            <li><a href="#">Reports</a></li>
            <li><a href="#">Invoice DashBoard</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Invoice DashBoard Details
                </h3>
            </div>
            <div class="box-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table>
                        <tr>
                        <td>
                                        <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Label">InvoiceNo</asp:Label>&nbsp;
                                       </td>
                                       <td>
                                        <asp:TextBox ID="TextBox1" runat="server" placeholder="Search Invoice Number" CssClass="form-control"></asp:TextBox>
                                    </td>
                        
                        </tr>
                            <tr>
                                <td>
                                <asp:Label ID="Label2" Font-Bold="true" runat="server" Text="Label">State Name:</asp:Label>&nbsp;
                                   
                                </td>
                                    <td>
                                        <asp:Panel ID="PBranch" runat="server">
                                            <asp:DropDownList ID="ddlstateName" runat="server" CssClass="form-control" >
                                            </asp:DropDownList>
                                        </asp:Panel>
                                    </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                <asp:Label ID="Label3" Font-Bold="true" runat="server" Text="Label">Company Name:</asp:Label>&nbsp;
                                   
                                </td>
                                    <td>
                                        <asp:Panel ID="Panel1" runat="server">
                                            <asp:DropDownList ID="ddlcompny" runat="server" CssClass="form-control" >
                                            <asp:ListItem Value="ALL">ALL</asp:ListItem>
                                            <asp:ListItem Value="1">SVDS</asp:ListItem>
                                            <asp:ListItem Value="2">SVD</asp:ListItem>
                                            <asp:ListItem Value="3">SVF</asp:ListItem>
                                            </asp:DropDownList>
                                        </asp:Panel>
                                    </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                        <asp:Panel ID="Panel2" runat="server">
                                            <asp:DropDownList ID="ddltype" runat="server" CssClass="form-control"  AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlSalesOffice_SelectedIndexChanged">
                                            <asp:ListItem >Date Based</asp:ListItem>
                                            <asp:ListItem >Invoice Based</asp:ListItem>
                                            <asp:ListItem >Stock Transfer</asp:ListItem>
                                            </asp:DropDownList>
                                        </asp:Panel>
                                    </td>
                                    <td>
                                        <asp:Panel ID="Panel3" runat="server">
                                            <asp:DropDownList ID="ddlinvoicetype" runat="server" CssClass="form-control"  AutoPostBack="True">
                                            <asp:ListItem >NonTax</asp:ListItem>
                                            <asp:ListItem >Tax</asp:ListItem>
                                            </asp:DropDownList>
                                        </asp:Panel>
                                    </td>
                               <asp:Panel ID="panel_invoice" runat="server" Visible="false">
                                <td >
                                                <asp:TextBox ID="txt_fromno" runat="server" Width="205px" placeholder="Enter From InvoiceNo:" CssClass="form-control"></asp:TextBox>
                                            </td>
                                            <td >
                                                <asp:TextBox ID="txt_tono" runat="server" Width="205px" placeholder="Enter To InvoiceNo:" CssClass="form-control"></asp:TextBox>
                                            </td>
                                            </asp:Panel>
                                            <asp:Panel ID="panel_date" runat="server" Visible="true">
                                <td>
                                    <asp:TextBox ID="txtFromdate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtFromdate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_todate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                        TargetControlID="txt_todate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                </asp:Panel>
                                <td style="width: 5px;">
                                </td>
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
                                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="100px"
                                            height="72px" />
                                    </div>
                                    <div style="left: 0%; text-align: center;">
                                        <br />
                                        <div style="width: 100%;">
                                            <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="26px" ForeColor="#0252aa"
                                                Text=""></asp:Label><br />
                                            <div>
                                            </div>
                                        </div>
                                        <div align="center">
                                            <span style="font-size: 18px; text-decoration: underline; color: #0252aa;">Invoice DashBoard </span>
                                        </div>
                                        <div align="center">
                                            <table style="width: 50%;">
                                                <tr>
                                                    <td>
                                                        State Name:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblstateName" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                    </td>
                                                    <td>
                                                        Date:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_selfromdate" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:GridView ID="grdReports" runat="server" ForeColor="White" Width="100%" CssClass="gridcls"
                                            GridLines="Both" Font-Bold="true">
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
                            <br />
                            <br />
                            <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print" OnClientClick="javascript:CallPrint('divPrint');"
                                runat="Server" />
                          <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl_utility.ashx">Export to XL</asp:HyperLink>
                        </asp:Panel>
                        <br />
                        <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
            </div>
        </div>
    </section>
</asp:Content>
