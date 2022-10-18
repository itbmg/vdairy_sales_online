<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="New_agent_invoice.aspx.cs" Inherits="New_agent_invoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="Js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
     #content
        {
            position: absolute;
            z-index: 1;
        }
       
    </style>
    <script type="text/javascript">
        function CallPrint(strid) {
            document.getElementById("tbl_po_print").style.borderCollapse = "collapse";
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
    </script>
    <script type="text/javascript">
        $(function () {
            FillSalesOffice()
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#txtFrom_date').val(today);
            $('#txtFrom_date1').val(today);
        });

        function FillSalesOffice() {
            var data = { 'operation': 'GetPlantSalesOffice' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindSalesOffice(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindSalesOffice(msg) {
            var ddlsalesOffice = document.getElementById('ddlSalesOffice');
            var length = ddlsalesOffice.options.length;
            ddlsalesOffice.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select";
            ddlsalesOffice.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlsalesOffice.appendChild(opt);
                }
            }
        }
        function ddlSalesOfficeChanged(ID) {
            var BranchID = ID.value;
            var data = { 'operation': 'GetSalesRoutes', 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindRouteName(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindRouteName(msg) {
            document.getElementById('ddlDispName').options.length = "";
            var veh = document.getElementById('ddlDispName');
            var length = veh.options.length;
            for (i = length - 1; i >= 0; i--) {
                veh.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Route Name";
            opt.value = "";
            veh.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].RouteName;
                    opt.value = msg[i].rid;
                    veh.appendChild(opt);
                }
            }
        }
        function ddlDispNameChanged(id) {
            FillAgentName(id.value);
        }
        function FillAgentName(RouteID) {
            var data = { 'operation': 'GetAgents', 'RouteID': RouteID };
            var s = function (msg) {
                if (msg) {
                    BindAgentName(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindAgentName(msg) {
            document.getElementById('ddlAgentName').options.length = "";
            var ddlAgentName = document.getElementById('ddlAgentName');
            var length = ddlAgentName.options.length;
            for (i = length - 1; i >= 0; i--) {
                ddlAgentName.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Agent Name";
            opt.value = "";
            ddlAgentName.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlAgentName.appendChild(opt);
                }
            }
        }
        function btnAgentInvoice_click() {
            var fromdate = document.getElementById('txtFrom_date').value;
            var AgentId = document.getElementById('ddlAgentName').value;
            var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
            var ddltype = document.getElementById('ddltype').value;
            if (fromdate == "") {
                alert("Please select from date");
                return false;
            }
            var data = { 'operation': 'btnAgentInvoice_click', 'fromdate': fromdate, 'AgentId': AgentId, 'SOID': ddlSalesOffice, 'ddltype': ddltype };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Data not found") {
                        alert(msg);
                        return false;
                    }
                    if (msg.length > 0) {
                        TotVat = 0.0;
                        var Aagent_Invoice = msg[0].Aagent_Invoice;
                        var Aagent_Invoice_item_det = msg[0].Aagent_Invoice_item_det;
                        var Aagent_Inventary = msg[0].Aagent_Inventary;
                        fillheaderdetails(Aagent_Invoice);
                        filldetails(Aagent_Invoice_item_det, Aagent_Inventary);
                        $("#divPrint").css("display", "block");
                        $("#btn_Print").css("display", "block");
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
        function clearall() {
            document.getElementById('span_toGSTIN').innerHTML = "";
            document.getElementById('lbltile').innerHTML = "";
            document.getElementById('spnAddress').innerHTML = "";
            document.getElementById('spngstnno').innerHTML = "";
            document.getElementById('spninvoiceno').innerHTML = "";
            document.getElementById('spninvoicedate').innerHTML = "";
            //            document.getElementById('spnfrmstatename').innerHTML = msg[0].frmstatename;
            //            document.getElementById('spnstatecode').innerHTML = msg[0].frmstatecode;
            document.getElementById('spndateofsupply').innerHTML = "";
            document.getElementById('spnplaceofsupply').innerHTML = "";

            document.getElementById('spnfromname').innerHTML = "";
            document.getElementById('spnfromaddress').innerHTML = "";
            document.getElementById('spnfromgstn').innerHTML = "";
            document.getElementById('spnfromstate').innerHTML = "";
            document.getElementById('spnfromstatecode').innerHTML = "";
            document.getElementById('lblpartyname').innerHTML = "";
            //            document.getElementById('lblroutename').innerHTML = msg[0].AgentName;lblsignname
            document.getElementById('spn_toaddress').innerHTML = "";
            document.getElementById('lbl_tostate').innerHTML = "";
            document.getElementById('lbl_tostatecode').innerHTML = "";
            document.getElementById('lblvendorphoneno').innerHTML = "";
            document.getElementById('lblvendoremail').innerHTML = "";
            document.getElementById('lblsignname').innerHTML = "";
            document.getElementById('lbl_companymobno').innerHTML = "";
            document.getElementById('lbl_companyemail').innerHTML = "";
        }
        function fillheaderdetails(msg) {
            clearall();
            if (msg.length > 0) {
                document.getElementById('span_toGSTIN').innerHTML = msg[0].togstin;
                document.getElementById('lbltile').innerHTML = msg[0].titlename;
                document.getElementById('spnAddress').innerHTML = msg[0].BranchAddress;
                document.getElementById('spngstnno').innerHTML = msg[0].fromgstn;
                document.getElementById('spninvoiceno').innerHTML = msg[0].invoiceno;
                document.getElementById('spninvoicedate').innerHTML = msg[0].invoicedate;
                //            document.getElementById('spnfrmstatename').innerHTML = msg[0].frmstatename;
                //            document.getElementById('spnstatecode').innerHTML = msg[0].frmstatecode;
                document.getElementById('spndateofsupply').innerHTML = msg[0].invoicedate;
                document.getElementById('spnplaceofsupply').innerHTML = msg[0].city;

                document.getElementById('spnfromname').innerHTML = msg[0].titlename;
                document.getElementById('spnfromaddress').innerHTML = msg[0].BranchAddress;
                document.getElementById('spnfromgstn').innerHTML = msg[0].fromgstn;
                document.getElementById('spnfromstate').innerHTML = msg[0].frmstatename;
                document.getElementById('spnfromstatecode').innerHTML = msg[0].frmstatecode;
                document.getElementById('lblpartyname').innerHTML = msg[0].AgentName;
                //            document.getElementById('lblroutename').innerHTML = msg[0].AgentName;lblsignname
                document.getElementById('spn_toaddress').innerHTML = msg[0].AgentAddress;
                document.getElementById('lbl_tostate').innerHTML = msg[0].tostatename;
                document.getElementById('lbl_tostatecode').innerHTML = msg[0].tostatecode;
                document.getElementById('lblvendorphoneno').innerHTML = msg[0].phoneno;
                document.getElementById('lblvendoremail').innerHTML = msg[0].email;
                document.getElementById('lblsignname').innerHTML = msg[0].titlename;
                document.getElementById('lbl_companymobno').innerHTML = msg[0].companyphone;
                document.getElementById('lbl_companyemail').innerHTML = msg[0].companyemail;
            }
        }
        var TotalAmount = 0; var totamount = 0;
        var TotVat = 0.0;
        function filldetails(msg, Aagent_Inventary) {
            var results = '<div  ><table border="1" id="tbl_po_print" style="width: 100%;" class="table table-bordered table-hover dataTable no-footer">';
            results += '<thead><tr style="background:antiquewhite;"><th value="#" colspan="1" style = "font-size: 12px;" rowspan="2">Sno</th><th value="Item Code" style = "font-size: 12px;" colspan="1" rowspan="2">Item Code</th><th style = "font-size: 12px;" value="Item Name" colspan="1" rowspan="2">Item Description</th><th style = "font-size: 12px;" value="HSN CODE" colspan="1" rowspan="2">HSN CODE</th><th value="UOM" style = "font-size: 12px;" colspan="1" rowspan="2">UOM</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="2">Qty</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="2">Qty(Kgs)</th><th value="Rate/Item (Rs.)" style = "font-size: 12px;" colspan="1" rowspan="2">Rate/Item (Rs.)</th><th value="Discount (Rs.)" style = "font-size: 12px;" colspan="1" rowspan="2">Discount (Rs.)</th><th value="Taxable Value" style = "font-size: 12px;" colspan="1" rowspan="2">Taxable Value</th><th value="CGST" style = "font-size: 12px;" colspan="2" rowspan="1">SGST</th><th value="SGST" colspan="2" style = "font-size: 12px;" rowspan="1">CGST</th><th value="IGST" style = "font-size: 12px;" colspan="2" rowspan="1">IGST</th><th value="Taxable Value" style = "font-size: 12px;" colspan="1" rowspan="2">Total Amount</th></tr><tr style="background:antiquewhite;"><th value="%" style = "font-size: 12px;" colspan="1" rowspan="1">%</th><th style = "font-size: 12px;" value="Amt (Rs.)" colspan="1" rowspan="1">Amt (Rs.)</th><th value="%" style = "font-size: 12px;" colspan="1" rowspan="1">%</th><th style = "font-size: 12px;" value="Amt (Rs.)" colspan="1" rowspan="1">Amt (Rs.)</th><th value="%" style = "font-size: 12px;" colspan="1" rowspan="1">%</th><th value="Amt (Rs.)" colspan="1" rowspan="1" style = "font-size: 12px;">Amt (Rs.)</th></tr></thead>';
            var tot_taxablevalue = 0;
            var tot_sgstamount = 0;
            var tot_cgstamount = 0;
            var tot_igstamount = 0;
            var tot_totalamount = 0;
            var tot_qty = 0;
            var msglength = msg.length;
            document.getElementById('spninvoicetype').innerHTML = msg[0].dctype;
            document.getElementById('spninvoiceno').innerHTML = msg[msglength - 1].invoiceno;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="font-size: 12px;">'
                results += '<td scope="row" class="1"  style="text-align:center;">' + msg[i].sno + '</td>';
                results += '<td scope="row" class="1"  style="text-align:center;">' + msg[i].itemcode + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].ProductName + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].hsncode + '</td>';
                results += '<td data-title="brandstatus" class="2">Nos</td>';
//                
                var qty = 0;
                qty = msg[i].qty;
                var uomqty = 0;
                uomqty = msg[i].uomqty;
                var changeqty = 0;
                var cal = 1000 / uomqty;
                changeqty = qty * cal;
                results += '<td data-title="brandstatus" style="font-weight:700;" class="2">' + parseFloat(changeqty).toFixed(2) + '</td>';
                results += '<td data-title="brandstatus" class="2">' + parseFloat(msg[i].qty).toFixed(2) + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].rate + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].discount + '</td>';
                results += '<td data-title="brandstatus" class="2">' + parseFloat(msg[i].taxablevalue).toFixed(2) + '</td>';
                tot_qty += parseFloat(msg[i].qty);
                tot_taxablevalue += parseFloat(msg[i].taxablevalue);
                tot_sgstamount += parseFloat(msg[i].sgstamount);
                tot_cgstamount += parseFloat(msg[i].cgstamount);
                tot_igstamount += parseFloat(msg[i].igstamount);
                tot_totalamount += parseFloat(msg[i].totalamount);
                results += '<td data-title="brandstatus" class="2">' + msg[i].sgst + '</td>';
                results += '<td data-title="brandstatus" class="2">' + parseFloat(msg[i].sgstamount).toFixed(2) + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].cgst + '</td>';
                results += '<td data-title="brandstatus" class="2">' + parseFloat(msg[i].cgstamount).toFixed(2) + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].igst + '</td>';
                results += '<td data-title="brandstatus" class="2">' + parseFloat(msg[i].igstamount).toFixed(2) + '</td>';
                var total = 0;
                results += '<td data-title="brandstatus" class="2">' + parseFloat(msg[i].totalamount).toFixed(2) + '</td></tr>';
            }
            var Total = "Total";
            results += '<tr>';
            results += '<td style = "font-size: 12px;text-align:center;background:antiquewhite;" colspan="5"><label>' + Total + '</label></td>';
            results += '<td style = "font-size: 12px;text-align:center;"><label></label></td>';
            results += '<td style = "font-size: 12px;text-align:center;"><label>' + parseFloat(tot_qty).toFixed(2) + '</label></td>';
            results += '<td style = "font-size: 12px;text-align:center;background:antiquewhite;" colspan="2"><label></label></td>';
            results += '<td style = "font-size: 12px;text-align:center;"><label>' + parseFloat(tot_taxablevalue).toFixed(2) + '</label></td>';
            results += '<td colspan="2" style="text-align:center;font-size: 12px;"><label>' + parseFloat(tot_sgstamount).toFixed(2) + '</label></td>';
            results += '<td colspan="2" style="text-align:center;font-size: 12px;"><label>' + parseFloat(tot_cgstamount).toFixed(2) + '</label></td>';
            results += '<td colspan="2" style="text-align:center;font-size: 12px;"><label>' + parseFloat(tot_igstamount).toFixed(2) + '</label></td>';
            results += '<td style="font-size: 12px;"><label>' + parseFloat(tot_totalamount).toFixed(2) + '</label></td>';
            var invname = "Inventory Details";
            results += '<tr >'
            results += '<td data-title="brandstatus" class="2"></td>';
            results += '<td scope="row" class="1" colspan="14" style="text-align:left;font-size: 14px;"><label>' + invname + '</label></td></tr>';

            for (var i = 0; i < Aagent_Inventary.length; i++) {
                results += '<tr style="font-size: 12px;">'
                results += '<td data-title="brandstatus" class="2"></td>';
                results += '<td data-title="brandstatus" class="2"></td>';
                results += '<td data-title="brandstatus" class="2">' + Aagent_Inventary[i].InvName + '</td>';
                results += '<td data-title="brandstatus" class="2">' + Aagent_Inventary[i].Opqty + '</td>';
                results += '<td data-title="brandstatus" class="2">' + Aagent_Inventary[i].Issueqty + '</td>';
                results += '<td data-title="brandstatus" class="2">' + Aagent_Inventary[i].Receivedqty + '</td>';
                results += '<td data-title="brandstatus" class="2">' + Aagent_Inventary[i].cloqty + '</td>';
                results += '<td data-title="brandstatus" colspan="12" class="2"></td></tr>';
            }
            results += '</tr></table></div>';
            results += '</table></div>';
            $("#div_itemdetails").html(results);
            var roundoff = Math.round(tot_totalamount);
            document.getElementById('recevied').innerHTML = inWords(parseInt(roundoff)) + "/-";
        }
        var a = ['', 'one ', 'two ', 'three ', 'four ', 'five ', 'six ', 'seven ', 'eight ', 'nine ', 'ten ', 'eleven ', 'twelve ', 'thirteen ', 'fourteen ', 'fifteen ', 'sixteen ', 'seventeen ', 'eighteen ', 'nineteen '];
        var b = ['', '', 'twenty', 'thirty', 'forty', 'fifty', 'sixty', 'seventy', 'eighty', 'ninety'];

        function inWords(num) {
            if ((num = num.toString()).length > 9) return 'overflow';
            n = ('000000000' + num).substr(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
            if (!n) return; var str = '';
            str += (n[1] != 0) ? (a[Number(n[1])] || b[n[1][0]] + ' ' + a[n[1][1]]) + 'crore ' : '';
            str += (n[2] != 0) ? (a[Number(n[2])] || b[n[2][0]] + ' ' + a[n[2][1]]) + 'lakh ' : '';
            str += (n[3] != 0) ? (a[Number(n[3])] || b[n[3][0]] + ' ' + a[n[3][1]]) + 'thousand ' : '';
            str += (n[4] != 0) ? (a[Number(n[4])] || b[n[4][0]] + ' ' + a[n[4][1]]) + 'hundred ' : '';
            str += (n[5] != 0) ? ((str != '') ? 'and ' : '') + (a[Number(n[5])] || b[n[5][0]] + ' ' + a[n[5][1]]) + 'only ' : '';
            return str;
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
             Agent Invoice<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Masters</a></li>
            <li><a href="#"> Agent Invoice</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
        <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Agent Invoice Details
                </h3>
            </div>
            <div class="box-body">
  <table>
                            <tr>
                                <td >
                                 <select id="ddlSalesOffice" class="form-control" onchange="ddlSalesOfficeChanged(this);">
                                        </select>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                 <select id="ddlDispName" class="form-control" onchange="ddlDispNameChanged(this);">
                                        </select>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                <select id="ddlAgentName" class="form-control">
                                        </select>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td style="display:none;">
                                            <select id="ddltype" class="form-control">
                                                <option value="01">Book1</option>
                                               <option value="02">Book2</option>
                                                 <option value="03">Book3</option>
                                             </select>
                                            </td>
                                            <td>
                                            <select id="ddltype" class="form-control">
                                                <option value="NonTax">NonTax</option>
                                               <option value="Tax">Tax</option>
                                             </select>
                                            </td>
                                             <td style="width: 5px;">
                            </td>
                                <td>
                                  <input type="date" id="txtFrom_date" class="form-control"/>
                                </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                            <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="btnAgentInvoice_click()"><i class="fa fa-refresh"></i> Get Details </button>
                            </td>
                        </tr>
                    </table>
                    <br />
                     <br />
                    <div id="divPrint" style="display: none;height:50%;">
                   <div class="content">
                    <div style="border:2px solid gray;">
                    <div style="width: 17%; float: right; padding-top:5px;">
                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="100px" height="72px" />
                        <br />
                    </div>
                    <div style="border:1px solid gray;">
                        <div style="font-family: Arial; font-size: 20px; font-weight: bold; color: Black;text-align: center;">
                            <span id="lbltile"> </span>
                            <br />
                        </div>
                        <div style="width:73%; padding-left:12%;text-align: center;">    
                        <span id="spnAddress" style="font-size: 14px;"></span>
                        <br />
                           <span id="Span1" style="font-size: 11px;font-weight: bold;">Website: www.vyshnavi.in</span>
                        <br />
                        <br />
                        </div>
                        <div style="width:73%; padding-left:12%;text-align: center;display:none;">
                        <span id="spngstnno" style="font-size: 14px;"></span>
                        <br />
                    <br />
                        </div>
                    </div>
                    <div align="center" style="border-bottom: 1px solid gray; border-top: 1px solid gray; background:antiquewhite;">
                        <span style="font-size: 18px; font-weight: bold;" id="spninvoicetype"></span>
                    </div>
                    <div style="width: 100%;">
                       <%-- <table style="width: 100%;border: 3px solid #dddddd;"  class="table table-bordered table-hover dataTable no-footer">--%>
                        <table style="width: 100%;">
                            <tr>
                            <td style="width: 60%;border: 2px solid gray;padding-left: 2%;">
                              <label style="font-size: 14px;font-weight: bold;">
                                        Bill From </label>
                                        <br />
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Name :</label>
                                    <span id="spnfromname" style="font-size: 11px;"></span>
                                    <br>
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Address :</label>
                                    <span id="spnfromaddress" style="font-size: 11px;"></span>
                                    <br>
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                         GSTIN :</label>
                                    <span id="spnfromgstn" style="font-size: 11px;font-weight: bold !important;"></span>
                                    <br>
                                     <label style="font-size: 12px;font-weight: bold !important;">
                                        Telephone no :</label>
                                    <span id="lbl_companymobno" style="font-size: 11px;"></span>
                                     &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Email Id :</label>
                                    <span id="lbl_companyemail" style="font-size: 11px;"></span>
                                    <br />
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        State Name :</label>
                                    <span id="spnfromstate" style="font-size: 11px;"></span>
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                       State Code :</label>
                                    <span id="spnfromstatecode" style="font-size: 11px;"></span>
                                    <br>
                                </td>
                            
                               
                                <td style="width: 39%; border:2px solid gray;padding-left: 2%;">
                                    <label style="font-size: 12px;">
                                       Invoice No :</label>
                                    <span id="spninvoiceno" style="font-size: 14px;"></span>
                                    <br />
                                    <label style="font-size: 12px;">
                                       Invoice Date :</label>
                                    <span id="spninvoicedate" style="font-size: 14px;"></span>
                                    <br />

                                   <%-- <label style="font-size: 12px;">
                                        Reverse Charge (Y/N):  :</label>
                                    <span id="spnreversecharge" style="font-size: 14px;">N</span>
                                    <br />--%>
                                   
                                   <%--<label style="font-size: 12px;">
                                        State Name  :</label>
                                    <span id="spnfrmstatename" style="font-size: 14px;"></span>
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <label style="font-size: 12px;">
                                        State Code  :</label>
                                    <span id="spnstatecode" style="font-size: 14px;"></span>
                                    <br />--%>

                                    <label style="font-size: 12px;">
                                        Buyer's OrderNo :</label>
                                    <span id="spnorderno" style="font-size: 14px;"></span>
                                    <br />
                                    <label style="font-size: 12px;">
                                        Mode/Terms of Payments :</label>
                                    <span id="lblmodeofpayments" style="font-size: 14px;"></span>
                                    <br />
                                </td>
                            </tr>
                        </table>

                        <table style="width: 100%;">
                        <tbody>
                            <tr>
                                
                                <td style="width: 60%;border: 2px solid gray;padding-left: 2%;">
                                  <label style="font-size: 14px;font-weight: bold;">
                                        Bill To </label>
                                        <br />
                                <label style="font-size: 12px;font-weight: bold !important;">
                                        Name :</label>
                                    <span id="lblpartyname" style="font-size: 11px;"></span>
                                    <br>
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Address :</label>
                                    <span id="spn_toaddress" style="font-size: 11px;"></span>
                                    <br>
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        GSTIN :</label>
                                    <span id="span_toGSTIN" style="font-size: 11px;"></span>
                                    <br>
                                     <label style="font-size: 12px;font-weight: bold !important;">
                                        State Name :</label>
                                    <span id="lbl_tostate" style="font-size: 11px;"></span>
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                     <label style="font-size: 12px;font-weight: bold !important;">
                                        State Code :</label>
                                    <span id="lbl_tostatecode" style="font-size: 11px;"></span>
                                    <br>
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Telephone no :</label>
                                    <span id="lblvendorphoneno" style="font-size: 11px;"></span>
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Email Id :</label>
                                    <span id="lblvendoremail" style="font-size: 11px;"></span>
                                    <br>
                                </td>
                                 <td style="width: 39%; border:2px solid gray;padding-left: 2%;">
                                <label style="font-size: 12px;">
                                        Transport Mode:</label>
                                    <span id="spntransport" style="font-size: 14px;">By Road</span>
                                    <br />
                                    <label style="font-size: 12px;">
                                        Vehicle No. :</label>
                                    <span id="spnvehicleno" style="font-size: 14px;"></span>
                                    <br />
                                     <label style="font-size: 12px;">
                                       Date of Supply :</label>
                                    <span id="spndateofsupply" style="font-size: 14px;"></span>
                                     <br />
                                     <label style="font-size: 12px;">
                                       Place of Supply :</label>
                                    <span id="spnplaceofsupply" style="font-size: 14px;"></span>

                                </td>
                            </tr>
                        </tbody></table>
                    </div>
                    <div style="font-family: Arial; font-weight: bold; color: Black; text-align:center; border:2px solid gray;">
                      <br />
                      </div>
                   <div id="div_itemdetails">
                    </div>
                <table>
                  <label style="font-size: 16px;font-weight: bold;">
                                        Towards:
                                             </label><label>Rs.</label>
                                    <span id="recevied" onclick="test.rnum.value = toWords(test.inum.value);" value="To Words"></span>

                </table>
                <br />
                <br />
                <br />
                <table style="width: 100%;">
                 <tr> 
                                     <td style="width: 25%;" colspan="3"></td>
                                       <td style="width: 50%;" >
                                            For  <span id="lblsignname" style="font-weight: bold; font-size: 12px;"></span>
                                       <br />
                                       <br />
                                       <br />
                                       </td>
                                    </tr>
                                        <tr>
                                            <td style="width: 20%;">
                                                <span style="font-weight: bold; font-size: 12px;">Prepared by</span>
                                            </td>
                                            <td style="width: 15%;">
                                                <span style="font-weight: bold; font-size: 12px;">Checked by</span>
                                            </td>
                                            <td style="width: 25%;">
                                                <span style="font-weight: bold; font-size: 12px;">Accountant</span>
                                            </td>
                                            <td style="width: 50%;">
                                                <span style="font-weight: bold; font-size: 12px;">Authorised Signatory</span>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                      <div>
                                        <span style="font-weight: bold; font-size: 13px;">Decleration:</span>
                        <br />
                                    <span style="font-size: 11px;">We declare that this Invoice shows the actual price of the goods described and that all particulars are true and correct</span>
                                    <br />
                                    </div>
                </div>
                </div>
                </div>
                  
                             <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="javascript:CallPrint('divPrint');"><i class="fa fa-print"></i> Print </button>

                </div>
                </div>
                </section>
</asp:Content>
