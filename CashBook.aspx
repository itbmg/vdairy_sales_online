<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CashBook.aspx.cs" Inherits="CashBook" EnableEventValidation = "false" %>

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
        function Call_div_pop_print(strid) {
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
        function CountChange(count) {
            var TotalCash = 0;
            var Total = 0;
            if (count.value == "") {
                $(count).closest("tr").find(".TotalClass").text(Total);
                document.getElementById('txtSubmittedAmount').value = Total;
                return false;
            }
            var Cash = $(count).closest("tr").find(".CashClass").text();
            Total = parseInt(count.value) * parseInt(Cash);
            $(count).closest("tr").find(".TotalClass").text(Total);
            $('.TotalClass').each(function (i, obj) {
                TotalCash += parseInt($(this).text());
            });
            document.getElementById('txt_Total').innerHTML = TotalCash;
            document.getElementById('<%=lblCash.ClientID %>').innerHTML = TotalCash;
            var lbliou = document.getElementById('<%=lblIou.ClientID %>').innerHTML;
            var totdif = parseFloat(lbliou) + parseFloat(TotalCash); 
            var labelObj = document.getElementById("<%= lblDiffernce.ClientID %>");
            var label = document.getElementById("<%= lblTotalAmout.ClientID %>");
            var labelhidden = document.getElementById("<%= lblhidden.ClientID %>").innerHTML;
            labelObj.value = "";
            label.value = "";
            document.getElementById('<%=lblTotalAmout.ClientID %>').innerHTML = totdif;
            document.getElementById('<%=lblDiffernce.ClientID %>').innerHTML = totdif - parseFloat(labelhidden);

//            var totCash = document.getElementById('txtAmount').innerHTML;
//            var BalCash = totCash - TotalCash;
//            document.getElementById('txtBalanceAmount').innerHTML = parseFloat(BalCash).toFixed(2);
        }
        function Validate() {
            var Status = "";
            var rowsdenominations = $("#tableReportingDetails tr:gt(0)");
            var DenominationString = "";
            $(rowsdenominations).each(function (i, obj) {
                if ($(this).closest("tr").find(".CashClass").text() == "") {
                }
                else {
                    DenominationString += $(this).closest("tr").find(".CashClass").text() + 'x' + $(this).closest("tr").find(".qtyclass").val() + "+";
                }
            });

            var lblDiffernce = document.getElementById('<%=lblDiffernce.ClientID %>').innerHTML;
            lblDiffernce = parseInt(lblDiffernce);
            lblDiffernce = Math.abs(lblDiffernce);
            if (lblDiffernce > 0) {
                alert("Please check denominations,differnce amount should be zero");
                return false;
            }
            var data = { 'operation': 'BtnGetCashBookClosing', 'DenominationString': DenominationString };
            var s = function (msg) {
                if (msg) {
                    Status = "Success";
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
            if (Status == "") {
                alert("Do you want to save");
                return false;
            }
        }
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
        function ExportToExcel() {
            window.location = "CashDenominationBook.aspx";
        }
        function PopupOpen() {
            $('#divMainAddNewRow').css('display', 'block');
        }
        function popupCloseClick() {
            $('#divMainAddNewRow').css('display', 'none');
        }
    </script>
    <script type="text/javascript">
        $(function () {


            setInterval(function () {
                $('#<%=btnGenerate.ClientID %>').click();

            }, 300000);


        });
       
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
            Cash Book<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Cash Book</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Cash Book Details
                </h3>
            </div>
            <div class="box-body">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="PPlant" runat="server" Visible="false">
                            <asp:DropDownList ID="ddlPlant" runat="server" CssClass="form-control" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlPlant_SelectedIndexChanged">
                            </asp:DropDownList>
                        </asp:Panel>
                    </td>
                    <td style="width:5px;"></td>
                    <td>
                        <asp:DropDownList ID="ddlSalesOffice" runat="server" CssClass="form-control">
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
                        <asp:Button ID="btnGenerate" Text="Generate" runat="server" OnClientClick="OrderValidate();"
                            CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
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
                        <span style="font-size: 18px; font-weight: bold; padding-left: 37%; text-decoration: underline;
                            color: #0252aa;">Cash Book</span><br />
                        <div>
                            <div style="width: 40%; float: left; padding-left: 7%;">
                                <asp:Label ID="lblSalesOffice" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </div>
                            <span style="font-weight: bold;">Date: </span>
                            <asp:Label ID="lbl_fromDate" runat="server" ForeColor="Red" Text=""></asp:Label>
                            <span style="font-weight: bold;">Closed Date: </span>
                            <asp:Label ID="lbl_ClosingDate" runat="server" ForeColor="Red" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
                <br />
                <div>
                    <span style="font-weight: bold; font-size: 16px;">Opening Balance: </span>
                    <asp:Label ID="lblOppBal" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="18px"
                        Text=""></asp:Label>
                </div>
                <div style="width: 100%;">
                    <div style="width: 33%; float: left;">
                        <asp:GridView ID="grdRouteCash" runat="server" ForeColor="White" Width="100%" GridLines="Both"
                            Font-Size="12px">
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
                    <div style="width: 23%; float: left; padding-left: 50px;">
                        <asp:GridView ID="grdCashPayable" runat="server" ForeColor="White" Width="100%" GridLines="Both"
                            Font-Size="12px">
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
                    <div style="width: 33%; float: right;">
                        <asp:GridView ID="grdDue" runat="server" ForeColor="White" Width="100%" GridLines="Both"
                            Font-Size="12px" OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="OnSelectedIndexChanged">
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
                </div>
                    <div style="width: 100%;">
                <div style="width: 33%;">
                    <asp:Panel ID="PanelDen" runat="server" Visible="false">
                        <span style="color: Red; font-size: 18px;">Physical Cash Balance</span>
                        <table cellpadding="0" cellspacing="0" style="width: 100%;" id="tableReportingDetails"
                            class="mainText2" border="1">
                            <thead>
                                <tr>
                                    <td style="width: 25%; height: 20px; color: #2f3293; font-size: 14px; font-weight: bold;
                                        text-align: center;">
                                        Cash
                                        <br />
                                    </td>
                                    <td style="width: 25%; text-align: center; height: 20px; font-size: 14px; font-weight: bold;
                                        color: #2f3293;">
                                        Count
                                        <br />
                                    </td>
                                    <td style="width: 10%; text-align: center; height: 20px; font-size: 14px; font-weight: bold;
                                        color: #2f3293; padding: 0px 0px 0px 2%;">
                                        Total
                                        <br />
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                            <tr class="tblrowcolor">
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span18" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>2000</b></span>
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                        <input type="number" id="Number9" onkeyup="CountChange(this);" class="qtyclass" onkeypress="return numberOnlyExample();"
                                            style="width: 80%; height: 24px; border: 1px solid gray; border-radius: 6px 6px 6px 6px;"
                                            placeholder="Enter Count" />
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span19" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                            <b>0</b></span>
                                    </td>
                                </tr>
                                <tr class="tblrowcolor">
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="txtsno" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>500</b></span>
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                        <input type="number" id="txtCount" onkeyup="CountChange(this);" class="qtyclass"
                                            onkeypress="return numberOnlyExample();" style="width: 80%; height: 24px; border: 1px solid gray;
                                            border-radius: 6px 6px 6px 6px;" placeholder="Enter Count" />
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span2" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                            <b>0</b></span>
                                    </td>
                                </tr>
                                 <tr class="tblrowcolor">
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span16" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>200</b></span>
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                        <input type="number" id="Number8" onkeyup="CountChange(this);" class="qtyclass" onkeypress="return numberOnlyExample();"
                                            style="width: 80%; height: 24px; border: 1px solid gray; border-radius: 6px 6px 6px 6px;"
                                            placeholder="Enter Count" />
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span17" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                            <b>0</b></span>
                                    </td>
                                </tr>
                                <tr class="tblrowcolor">
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span1" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>100</b></span>
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                        <input type="number" id="Number1" onkeyup="CountChange(this);" class="qtyclass" onkeypress="return numberOnlyExample();"
                                            style="width: 80%; height: 24px; border: 1px solid gray; border-radius: 6px 6px 6px 6px;"
                                            placeholder="Enter Count" />
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span3" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                            <b>0</b></span>
                                    </td>
                                </tr>
                                <tr class="tblrowcolor">
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span4" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>50</b></span>
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                        <input type="number" id="Number2" onkeyup="CountChange(this);" class="qtyclass" onkeypress="return numberOnlyExample();"
                                            style="width: 80%; height: 24px; border: 1px solid gray; border-radius: 6px 6px 6px 6px;"
                                            placeholder="Enter Count" />
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span5" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                            <b>0</b></span>
                                    </td>
                                </tr>
                                <tr class="tblrowcolor">
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span6" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>20</b></span>
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                        <input type="number" id="Number3" onkeyup="CountChange(this);" class="qtyclass" onkeypress="return numberOnlyExample();"
                                            style="width: 80%; height: 24px; border: 1px solid gray; border-radius: 6px 6px 6px 6px;"
                                            placeholder="Enter Count" />
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span7" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                            <b>0</b></span>
                                    </td>
                                </tr>
                                <tr class="tblrowcolor">
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span8" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>10</b></span>
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                        <input type="number" id="Number4" onkeyup="CountChange(this);" class="qtyclass" onkeypress="return numberOnlyExample();"
                                            style="width: 80%; height: 24px; border: 1px solid gray; border-radius: 6px 6px 6px 6px;"
                                            placeholder="Enter Count" />
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span9" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                            <b>0</b></span>
                                    </td>
                                </tr>
                                <tr class="tblrowcolor">
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span10" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>5</b></span>
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                        <input type="number" id="Number5" onkeyup="CountChange(this);" class="qtyclass" onkeypress="return numberOnlyExample();"
                                            style="width: 80%; height: 24px; border: 1px solid gray; border-radius: 6px 6px 6px 6px;"
                                            placeholder="Enter Count" />
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span11" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                            <b>0</b></span>
                                    </td>
                                </tr>
                                <tr class="tblrowcolor">
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span12" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>2</b></span>
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                        <input type="number" id="Number6" onkeyup="CountChange(this);" class="qtyclass" onkeypress="return numberOnlyExample();"
                                            style="width: 80%; height: 24px; border: 1px solid gray; border-radius: 6px 6px 6px 6px;"
                                            placeholder="Enter Count" />
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span13" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                            <b>0</b></span>
                                    </td>
                                </tr>
                                <tr class="tblrowcolor">
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span14" class="CashClass" style="font-size: 14px; color: Red; font-weight: bold;"><b>1</b></span>
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <b style="font-size: 11px; font-weight: bold;">X</b>
                                        <input type="number" id="Number7" onkeyup="CountChange(this);" class="qtyclass" onkeypress="return numberOnlyExample();"
                                            style="width: 80%; height: 24px; border: 1px solid gray; border-radius: 6px 6px 6px 6px;"
                                            placeholder="Enter Count" />
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                        <span id="Span15" class="TotalClass" style="font-size: 14px; color: Red; font-weight: bold;">
                                            <b>0</b></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%; height: 30px; vertical-align: top; font-size: 12px; font-weight: 500;
                                        text-align: center; padding: 0px 0px 0px 3px">
                                    </td>
                                    <td style="width: 20%; height: 30px; vertical-align: middle; text-align: center;
                                        color: Gray;">
                                        <span style="font-size: 16px; color: Blue;">Total:</span>
                                    </td>
                                    <td style="width: 20%; height: 30px; font-size: 11px; vertical-align: middle; text-align: center;
                                        color: Gray;" align="center">
                                        <span style="font-size: 16px; color: Red; font-weight: bold;" id="txt_Total"></span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="panelGrid" runat="server" Visible="false">
                        <span style="color: Red; font-size: 18px;">Physical Cash Balance</span>
                        <asp:GridView ID="grdDenomination" runat="server" ForeColor="White" Width="100%"
                            GridLines="Both" Font-Size="Smaller">
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                            <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                Font-Names="Raavi" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                            <AlternatingRowStyle HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                        </asp:GridView>
                    </asp:Panel>
                </div>
                   <div style="width: 33%;float:right;">
                     <asp:GridView ID="grdTodayIOU" runat="server" ForeColor="White" Width="100%" GridLines="Both"
                            Font-Size="12px">
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
                </div>
                <asp:Panel ID="DiffPanel" runat="server" Visible="true">
                    <div style="width: 30%;">
                        <table>
                            <tr>
                                <td style="width: 25%; display: none;">
                                    <span style="font-size: 14px;">Total IOU</span>
                                </td>
                                <td style="width: 25%; display: none;">
                                    <asp:Label ID="lblhidden" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%;">
                                    <span style="font-size: 14px;">Total IOU</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblIou" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%;">
                                    <span style="font-size: 14px;">Cash</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblCash" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%;">
                                    <span style="font-size: 14px;">Total</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblTotalAmout" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%;">
                                    <span style="font-size: 14px;">Differnce</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblDiffernce" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <asp:Panel ID="hidePanel" runat="server" Visible="true">
                    <div>
                        <table>
                            <tr>
                                <td style="width: 25%;">
                                    <span style="font-size: 14px;">Opp Bal</span>
                                </td>
                                <td style="width: 25%;">
                                    <asp:Label ID="lblZeroOppBal" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%;">
                                    <span style="font-size: 14px;">Receipts</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblZeroReceipts" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%;">
                                    <span style="font-size: 14px;">Payments</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblZeroPayments" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%;">
                                    <span style="font-size: 14px;">Differnce</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblZeroDiffence" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>

                <table style="width: 100%;">
                    <tr>
                        <td style="width: 20%;">
                            <span style="font-weight: bold; font-size: 14px;">Manager Signature</span>
                        </td>
                        <td style="width: 20%;">
                            <span style="font-weight: bold; font-size: 14px;">Cashier</span>
                        </td>
                        <td style="width: 30%;">
                            <span style="font-weight: bold; font-size: 14px;">Prepared By:</span>
                            <asp:Label ID="lblpreparedby" runat="server" Font-Size="Large" ForeColor="Red" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divMainAddNewRow" class="pickupclass" style="text-align: center; height: 100%;
                width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
                background: rgba(192, 192, 192, 0.7);">
                <div id="divAddNewRow" style="border: 5px solid #A0A0A0; position: absolute; top: 30%;
                    background-color: White; width: 100%; height: 50%; -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                    -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                    border-radius: 10px 10px 10px 10px;">
                      <div id="div_pop_print">
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GrdProducts" runat="server" ForeColor="White" Width="100%" CssClass="gridcls"
                                GridLines="Both" Font-Bold="true">
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                    Font-Names="Raavi" Font-Size="Small" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                                <AlternatingRowStyle HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </div>
                </div>
                <div id="divclose" style="width: 35px; top: 24.5%; right: .5%; position: absolute;
                    z-index: 99999; cursor: pointer;">
                    <img src="Images/Odclose.png" alt="close" onclick="popupCloseClick();" />
                 <input type="button" class="btn btn-primary" value="Print"  onclick="javascript:Call_div_pop_print('div_pop_print');" /> 
                </div>
            </div>

            <br />
             <input type="button" class="btn btn-primary" value="Print"  onclick="javascript:CallPrint('divPrint');" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
   
    <br />
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlfoter" runat="server" Visible="false">
            <asp:Button ID="BtnSave" Text="Save" runat="server" CssClass="btn btn-primary" OnClick="BtnSave_Click"
                OnClientClick="Validate();"  />
            <input type="button" class="btn btn-primary" value="Export To Excel"  onclick="ExportToExcel();" />
            <%--<asp:Button ID="btnPrint" CssClass="SaveButton" Text="Print" Style="height: 25px;width: 120px;font-size:15px;" OnClientClick="javascript:CallPrint('divPrint');" runat="Server" />--%>
            <%--   <asp:Button ID="Button3" Text="Export To Excel" runat="server" CssClass="SaveButton"
        OnClick="btn_Export_Click" Style="height: 25px; width: 156px;" />--%>
            <br />
            <br />
            <br />
            </asp:Panel>
            <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    </div>
    </section>
</asp:Content>


