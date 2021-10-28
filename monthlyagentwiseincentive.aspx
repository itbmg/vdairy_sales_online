<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="monthlyagentwiseincentive.aspx.cs" Inherits="monthlyagentwiseincentive" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
  <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3004" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=300,height=300,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.write('<link rel="stylesheet" type="text/css" href="Css/print.css" />');
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
    </script>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);

        });
    </script>
     <style type="text/css">
        .mylbl
        {
           font-size: 12px;
        }
        .mylbl1
        {
            Font-Size:12px;
        }
    </style>

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
            <div>
                <table>
                    <tr>
                        <td>
                            <asp:Panel ID="PBranch" runat="server">
                                <asp:DropDownList ID="ddlSalesOffice" runat="server" CssClass="form-control" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlSalesOffice_SelectedIndexChanged">
                                </asp:DropDownList>
                            </asp:Panel>
                        </td>
                        <td style="width:5px;"></td>
                        <td>
                            <asp:DropDownList ID="ddlDispName" runat="server" CssClass="form-control" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlDispName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="width:5px;"></td>
                        <td>
                            <asp:DropDownList ID="ddlAgentName" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </td>
                        <td style="width:5px;"></td>
                        <td>
                            <asp:TextBox ID="txtFromdate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                            <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                TargetControlID="txtFromdate" Format="dd-MM-yyyy HH:mm">
                            </asp:CalendarExtender>
                        </td>
                        <td style="width:5px;"></td>
                        <td>
                            <asp:TextBox ID="txtTodate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtTodate"
                                Format="dd-MM-yyyy HH:mm">
                            </asp:CalendarExtender>
                        </td>
                        <td style="width:5px;"></td>
                        <td>
                            <asp:Button ID="btnGenerate" Text="Generate" runat="server" OnClientClick="OrderValidate();"
                                CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divPrint">
                <div style="width: 100%;">
                    <div style="width: 11%; float: left;">
                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="55px" height="25px" />
                    </div>
                    <div style="left: 0%; text-align: center;">
                        <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="26px" ForeColor="#0252aa"
                                            Text=""></asp:Label>
                    </div>
                    <div style="width: 100%;">
                     <span style="font-size: 14px; font-weight: bold; padding-left: 27%; text-decoration: underline;
                                            color: #0252aa;">Incentive Statement</span><br />
                        <div>
                            <div style="float: left; padding-left: 2%;">
                                <span style="font-weight: bold;">Agent Name: </span>
                                <asp:Label ID="lblAgent" class="mylbl1" runat="server" Text=""></asp:Label>
                            </div>
                            <div style="float: left; padding-left: 5%;">
                                <span style="font-weight: bold;">RouteName: </span>
                                <asp:Label ID="lblRoute" class="mylbl1" runat="server" Text=""></asp:Label>
                            </div>
                             <div style="float: left; padding-left: 25%;">
                            <span style="font-weight: bold;">Date: </span>
                            <asp:Label ID="lbl_fromDate" runat="server" class="mylbl1"
                                Text=""></asp:Label>
                            <span style="font-size: 18px;">TO</span>
                            <asp:Label ID="lbl_selttodate" runat="server" class="mylbl1" Text="" ></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:GridView ID="grdReports" runat="server">
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
                                <asp:Label ID="lbincentivegiven" runat="server">IncentiveGiven:</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblincentivegiven" runat="server" Style="font-size: 20px;" ForeColor="Red"
                                    Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server">Remarks:</asp:Label>
                            </td>
                            <td colspan="3">
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print"  OnClientClick="javascript:CallPrint('divPrint');"
                runat="Server" />
            <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:Button ID="Button3" Text="Export To Excel" runat="server" CssClass="btn btn-primary"
        OnClick="btn_Export_Click" />
        </div>
        </div>
        </section>
</asp:Content>
