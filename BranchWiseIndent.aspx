<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="BranchWiseIndent.aspx.cs" Inherits="BranchWiseIndent" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Kendo/jquery.min.js" type="text/javascript"></script>
    <script src="Kendo/kendo.all.min.js" type="text/javascript"></script>
    <link href="Kendo/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="Kendo/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function callHandler(d, s, e) {
            $.ajax({
                url: 'DairyFleet.axd',
                data: d,
                type: 'GET',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }
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
        function indentincreasedecresereport2015() {
            var BranchID = document.getElementById('<%=ddlSalesOffice.ClientID %>').value;
            var ddlType = document.getElementById('<%=ddlType.ClientID %>').value;
            if (ddlType == "Branch Wise") {
                alert("Please Select Agent Type");
                return false;
            }
            var data = { 'operation': 'Getindentincreasedecresereport2015', 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    else {
                        createleakbarChart2015(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var newleakXarray = [];
        function createleakbarChart2015(databind) {
            var newYarray = [];
            var newXarray = [];
            var myTableDiv = document.getElementById("example");
            var divleakbar = document.createElement("div");
            myTableDiv.appendChild(divleakbar);
            for (var k = 0; k < databind.length; k++) {
                var BranchName = [];
                var IndentDate = databind[k].IndentDate;
                var UnitQty = databind[k].UnitQty;
                var DeliveryQty = databind[k].DeliveryQty;
                var Status = databind[k].Status;
                newXarray = IndentDate.split(',');
                for (var i = 0; i < DeliveryQty.length; i++) {
                    newYarray.push({ 'data': DeliveryQty[i].split(','), 'name': Status[i] });
                }
            }
            $('#chart2').kendoChart({
                title: {
                    text: "Indent Increase Decrease Report For Previous Month",
                    color: "#006600"
                },
                legend: {
                    position: "bottom"
                },
                chartArea: {
                    background: ""
                },
                seriesDefaults: {
                    type: "line",
                    style: "smooth"
                },
                series: newYarray,
                valueAxis: {
                    labels: {
                        format: "{0}"
                    },
                    line: {
                        visible: false
                    },
                    axisCrossingValue: -10
                },
                categoryAxis: {
                    categories: newXarray,
                    //                        categories: [2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011],
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: 65
                    }
                },
                tooltip: {
                    visible: true,
                    format: "{0}",
                    template: "#= series.name #: #= value #"
                }
            });
        }
        function indentincreasedecresereport() {
            var BranchID = document.getElementById('<%=ddlSalesOffice.ClientID %>').value;
            var ddlType = document.getElementById('<%=ddlType.ClientID %>').value;
            if (ddlType == "Branch Wise") {
                alert("Please Select Agent Type");
                return false;
            }
            var data = { 'operation': 'Getindentincreasedecresereport', 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    else {
                        createleakbarChart(msg);
                        indentincreasedecresereport2015();
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var newleakXarray = [];
        function createleakbarChart(databind) {
            var newYarray = [];
            var newXarray = [];
            var myTableDiv = document.getElementById("example");
            var divleakbar = document.createElement("div");
            myTableDiv.appendChild(divleakbar);
            for (var k = 0; k < databind.length; k++) {
                var BranchName = [];
                var IndentDate = databind[k].IndentDate;
                var UnitQty = databind[k].UnitQty;
                var DeliveryQty = databind[k].DeliveryQty;
                var Status = databind[k].Status;
                newXarray = IndentDate.split(',');
                for (var i = 0; i < DeliveryQty.length; i++) {
                    newYarray.push({ 'data': DeliveryQty[i].split(','), 'name': Status[i] });
                }
            }
            $('#chart1').kendoChart({
                title: {
                    text: "Indent Increase Decrease Report for 2016",
                    color: "#006600"
                },
                legend: {
                    position: "bottom"
                },
                chartArea: {
                    background: ""
                },
                seriesDefaults: {
                    type: "line",
                    style: "smooth"
                },
                series: newYarray,
                valueAxis: {
                    labels: {
                        format: "{0}"
                    },
                    line: {
                        visible: false
                    },
                    axisCrossingValue: -10
                },
                categoryAxis: {
                    categories: newXarray,
                    //                        categories: [2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011],
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: 65
                    }
                },
                tooltip: {
                    visible: true,
                    format: "{0}",
                    template: "#= series.name #: #= value #"
                }
            });
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
            Branch Wise Indent<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Branch Wise Indent</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Branch Wise Indent Details
                </h3>
            </div>
            <div class="box-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div>
                            <table>
                                <tr>
                                 <td>
                                        <asp:DropDownList ID="ddlremarkstype" runat="server" CssClass="form-control">
                                            <asp:ListItem>Without Remarks</asp:ListItem>
                                            <asp:ListItem>With Remarks</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged">
                                            <asp:ListItem>Branch Wise</asp:ListItem>
                                            <asp:ListItem>Agent Wise</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <asp:Panel ID="PBranch" runat="server" Visible="false">
                                            <asp:DropDownList ID="ddlSalesOffice" runat="server" CssClass="form-control">
                                            </asp:DropDownList>
                                        </asp:Panel>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlindenttype" runat="server" CssClass="form-control">
                                            <asp:ListItem>All</asp:ListItem>
                                            <asp:ListItem>Increase</asp:ListItem>
                                            <asp:ListItem>Decrese</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromdate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                        <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="txtFromdate" Format="dd-MM-yyyy HH:mm">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTodate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtTodate"
                                            Format="dd-MM-yyyy HH:mm">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td style="width: 5px;">
                                    </td>
                                    <td>
                                        <asp:Button ID="btnGenerate" Text="Generate" runat="server" OnClientClick="OrderValidate();"
                                            CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <asp:Panel ID="pnlHide" runat="server" Visible="false">
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
                                        <span style="font-size: 14px; font-weight: bold; padding-left: 35%; text-decoration: underline;
                                            color: #0252aa;">Branch Wise Indent Comparison Report</span><br />
                                        <div align="center">
                                            <span style="font-weight: bold;">From Date: </span>
                                            <asp:Label ID="lbl_fromDate" runat="server" Style="font-size: 11px;" ForeColor="Red"
                                                Text=""></asp:Label>
                                            <span style="font-size: bold;">To Date</span>
                                            <asp:Label ID="lbl_selttodate" runat="server" Style="font-size: 11px;" Text="" ForeColor="Red"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <asp:GridView ID="grdReports" runat="server" ForeColor="White" Width="100%" CssClass="gridcls"
                                    GridLines="Both" Font-Bold="true" Font-Size="Smaller" OnRowDataBound="grdReports_RowDataBound">
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                    <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                        Font-Names="Raavi" Font-Size="Small" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                                    <AlternatingRowStyle HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                                <br />
                                <div id="example" class="k-content">
                                    <div class="chart-wrapper">
                                      <div id="chart2">
                                        </div>
                                        <br />
                                        <div id="chart1">
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 30%;">
                                            <span style="font-weight: bold; font-size: 14px;">PREPARED BY</span>
                                        </td>
                                        <td style="width: 30%;">
                                            <span style="font-weight: bold; font-size: 14px;">MANAGER</span>
                                        </td>
                                        <td style="width: 30%;">
                                            <span style="font-weight: bold; font-size: 14px;">MARKETING EXECUTIVE</span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                           <i class="fa fa-print"></i> <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print" OnClientClick="javascript:CallPrint('divPrint');"
                                runat="Server" />
                            <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                        </asp:Panel>
                        <br />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl_utility.ashx"><i class="fa fa-file-excel-o"></i>  Export to XL</asp:HyperLink>
                <br />
            </div>
        </div>
    </section>
</asp:Content>
