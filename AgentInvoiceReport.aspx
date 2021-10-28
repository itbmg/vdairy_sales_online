<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="AgentInvoiceReport.aspx.cs" Inherits="AgentInvoiceReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
    <script type="text/javascript">
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }

        function CallPrint1(strid) {
            var divToPrint1 = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint1.innerHTML + '</body></html>');
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
            $("#ks").css("display", "block");
        });
        function show_NewInvoice_format() {
            $("#div_dispatchMobile1").css("display", "block");
            $("#ks").css("display", "none");
        }
        function show_Agent_invoicesummary() {
            $("#ks").css("display", "block");
            $("#div_dispatchMobile1").css("display", "none");
        }
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
        function get_Agent_Invoice_Data() {
            var fromdate = document.getElementById('txtFrom_date').value;
            var AgentId = document.getElementById('ddlAgentName').value;
            var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
            var ddltaxtype = "PoNumbers";
            if (fromdate == "") {
                alert("Please select from date");
                return false;
            }
            var ddltype = document.getElementById('ddltaxtype').value;

            var data = { 'operation': 'btnAgentInvoice_click', 'fromdate': fromdate, 'AgentId': AgentId, 'SOID': ddlSalesOffice, 'ddltype': ddltype, 'ddltaxtype': ddltaxtype };
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
                        $("#div_agetdet").css("display", "block");
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
            // document.getElementById('spnfrmstatename').innerHTML = msg[0].frmstatename;
            // document.getElementById('spnstatecode').innerHTML = msg[0].frmstatecode;
            document.getElementById('spndateofsupply').innerHTML = "";
            document.getElementById('spnplaceofsupply').innerHTML = "";

            document.getElementById('spnfromname').innerHTML = "";
            document.getElementById('spnfromaddress').innerHTML = "";
            document.getElementById('spnfromgstn').innerHTML = "";
            document.getElementById('spnfromstate').innerHTML = "";
            document.getElementById('spnfromstatecode').innerHTML = "";
            document.getElementById('lblpartyname').innerHTML = "";
            // document.getElementById('lblroutename').innerHTML = msg[0].AgentName;lblsignname
            document.getElementById('spn_toaddress').innerHTML = "";
            document.getElementById('lbl_tostate').innerHTML = "";
            document.getElementById('lbl_tostatecode').innerHTML = "";
            document.getElementById('lblvendorphoneno').innerHTML = "";
            document.getElementById('lblvendoremail').innerHTML = "";
            document.getElementById('lblsignname').innerHTML = ""; 
            document.getElementById('lbl_companymobno').innerHTML = "";
            document.getElementById('lbl_companyemail').innerHTML = "";
            document.getElementById('lblbuyercompanyname').innerHTML = "";

            document.getElementById('Span2').innerHTML = "";
            document.getElementById('Span3').innerHTML = "";
            document.getElementById('Span4').innerHTML = "";
            document.getElementById('Span5').innerHTML = "";
            document.getElementById('Span6').innerHTML = "";
            document.getElementById('Span7').innerHTML = "";
            document.getElementById('Span8').innerHTML = "";
            document.getElementById('Span9').innerHTML = "";
            document.getElementById('Span10').innerHTML = "";
            document.getElementById('Span11').innerHTML = "";

            $("#Span2").css("display", "none");
            $("#Span3").css("display", "none");
            $("#Span4").css("display", "none");
            $("#Span5").css("display", "none");
            $("#Span6").css("display", "none");
            $("#H1").css("display", "none");
            $("#H2").css("display", "none");
            $("#H3").css("display", "none");
            $("#H4").css("display", "none");
            $("#H5").css("display", "none");
            $("#Span7").css("display", "none");
            $("#Span8").css("display", "none");
            $("#Span9").css("display", "none");
            $("#Span10").css("display", "none");
            $("#Span11").css("display", "none");
            $("#H6").css("display", "none");
            $("#H7").css("display", "none");
            $("#H8").css("display", "none");
            $("#H9").css("display", "none");
            $("#H10").css("display", "none");
        }
        function fillheaderdetails(msg) {
            clearall();
            if (msg.length > 0) {
                var spl = [];
                document.getElementById('span_toGSTIN').innerHTML = msg[0].togstin;
                document.getElementById('lbltile').innerHTML = msg[0].titlename;
                spl = msg[0].ponumber.split(",");
                if (spl.length == 1) {
                    document.getElementById('spnorderno').innerHTML = spl[0];
                }
                if (spl.length == 2) {
                    document.getElementById('spnorderno').innerHTML = spl[0];
                    $("#Span2").css("display", "block");
                    $("#H1").css("display", "block");
                    document.getElementById('Span2').innerHTML = spl[1];
                }
                if (spl.length == 3) {
                    document.getElementById('spnorderno').innerHTML = spl[0];
                    $("#Span2").css("display", "block");
                    $("#Span3").css("display", "block");
                    $("#H1").css("display", "block");
                    $("#H2").css("display", "block");
                    document.getElementById('Span2').innerHTML = spl[1];
                    document.getElementById('Span3').innerHTML = spl[2];
                }
                if (spl.length == 4) {
                    $("#Span2").css("display", "block");
                    $("#Span3").css("display", "block");
                    $("#Span4").css("display", "block");
                    $("#H1").css("display", "block");
                    $("#H2").css("display", "block");
                    $("#H3").css("display", "block");
                    document.getElementById('spnorderno').innerHTML = spl[0];
                    document.getElementById('Span2').innerHTML = spl[1];
                    document.getElementById('Span3').innerHTML = spl[2];
                    document.getElementById('Span4').innerHTML = spl[3];
                }
                if (spl.length == 5) {
                    $("#Span2").css("display", "block");
                    $("#Span3").css("display", "block");
                    $("#Span4").css("display", "block");
                    $("#Span5").css("display", "block");
                    $("#H1").css("display", "block");
                    $("#H2").css("display", "block");
                    $("#H3").css("display", "block");
                    $("#H4").css("display", "block");
                    document.getElementById('spnorderno').innerHTML = spl[0];
                    document.getElementById('Span2').innerHTML = spl[1];
                    document.getElementById('Span3').innerHTML = spl[2];
                    document.getElementById('Span4').innerHTML = spl[3];
                    document.getElementById('Span5').innerHTML = spl[4];
                }
                if (spl.length == 6) {
                    $("#Span2").css("display", "block");
                    $("#Span3").css("display", "block");
                    $("#Span4").css("display", "block");
                    $("#Span5").css("display", "block");
                    $("#Span6").css("display", "block");
                    $("#H1").css("display", "block");
                    $("#H2").css("display", "block");
                    $("#H3").css("display", "block");
                    $("#H4").css("display", "block");
                    $("#H5").css("display", "block");
                    document.getElementById('spnorderno').innerHTML = spl[0];
                    document.getElementById('Span2').innerHTML = spl[1];
                    document.getElementById('Span3').innerHTML = spl[2];
                    document.getElementById('Span4').innerHTML = spl[3];
                    document.getElementById('Span5').innerHTML = spl[4];
                    document.getElementById('Span6').innerHTML = spl[5];
                }




                var spl1 = [];
                document.getElementById('span_toGSTIN').innerHTML = msg[0].togstin;
                document.getElementById('lbltile').innerHTML = msg[0].titlename;
                spl1 = msg[0].grnno.split(",");
                if (spl1.length == 1) {
                    document.getElementById('spngrnno').innerHTML = spl1[0];
                }
                if (spl1.length == 2) {
                    document.getElementById('spngrnno').innerHTML = spl1[0];
                    $("#Span7").css("display", "block");
                    $("#H6").css("display", "block");
                    document.getElementById('Span7').innerHTML = spl1[1];
                }
                if (spl1.length == 3) {
                    document.getElementById('spngrnno').innerHTML = spl1[0];
                    $("#Span7").css("display", "block");
                    $("#Span8").css("display", "block");
                    $("#H6").css("display", "block");
                    $("#H7").css("display", "block");
                    document.getElementById('Span7').innerHTML = spl1[1];
                    document.getElementById('Span8').innerHTML = spl1[2];
                }
                if (spl1.length == 4) {
                    $("#Span7").css("display", "block");
                    $("#Span8").css("display", "block");
                    $("#Span9").css("display", "block");
                    $("#H6").css("display", "block");
                    $("#H7").css("display", "block");
                    $("#H8").css("display", "block");
                    document.getElementById('spngrnno').innerHTML = spl1[0];
                    document.getElementById('Span7').innerHTML = spl1[1];
                    document.getElementById('Span8').innerHTML = spl1[2];
                    document.getElementById('Span9').innerHTML = spl1[3];
                }
                if (spl1.length == 5) {
                    $("#Span7").css("display", "block");
                    $("#Span8").css("display", "block");
                    $("#Span9").css("display", "block");
                    $("#Span10").css("display", "block");
                    $("#H6").css("display", "block");
                    $("#H7").css("display", "block");
                    $("#H8").css("display", "block");
                    $("#H9").css("display", "block");
                    document.getElementById('spngrnno').innerHTML = spl1[0];
                    document.getElementById('Span7').innerHTML = spl1[1];
                    document.getElementById('Span8').innerHTML = spl1[2];
                    document.getElementById('Span9').innerHTML = spl1[3];
                    document.getElementById('Span10').innerHTML = spl1[4];
                }
                if (spl1.length == 6) {
                    $("#Span7").css("display", "block");
                    $("#Span8").css("display", "block");
                    $("#Span9").css("display", "block");
                    $("#Span10").css("display", "block");
                    $("#Span11").css("display", "block");
                    $("#H6").css("display", "block");
                    $("#H7").css("display", "block");
                    $("#H8").css("display", "block");
                    $("#H9").css("display", "block");
                    $("#H10").css("display", "block");
                    document.getElementById('spngrnno').innerHTML = spl1[0];
                    document.getElementById('Span7').innerHTML = spl1[1];
                    document.getElementById('Span8').innerHTML = spl1[2];
                    document.getElementById('Span9').innerHTML = spl1[3];
                    document.getElementById('Span10').innerHTML = spl1[4];
                    document.getElementById('Span11').innerHTML = spl1[5];
                }

//                document.getElementById('spngrnno').innerHTML = msg[0].grnno;
                document.getElementById('spnAddress').innerHTML = msg[0].BranchAddress;
                document.getElementById('spngstnno').innerHTML = msg[0].fromgstn;
                document.getElementById('spndcno').innerHTML = msg[0].quatationno;
                document.getElementById('spninvoicedate').innerHTML = msg[0].invoicedate;

                document.getElementById('spndateofsupply').innerHTML = msg[0].invoicedate;
                document.getElementById('spnplaceofsupply').innerHTML = msg[0].city;
                document.getElementById('spnfromname').innerHTML = msg[0].titlename;
                document.getElementById('spnfromaddress').innerHTML = msg[0].BranchAddress;
                document.getElementById('spnfromgstn').innerHTML = msg[0].fromgstn;
                document.getElementById('spnfromstate').innerHTML = msg[0].frmstatename;
                document.getElementById('spnfromstatecode').innerHTML = msg[0].frmstatecode;
                document.getElementById('lblpartyname').innerHTML = msg[0].AgentName;
                // document.getElementById('lblroutename').innerHTML = msg[0].AgentName;lblsignname
                document.getElementById('spn_toaddress').innerHTML = msg[0].AgentAddress;
                document.getElementById('lbl_tostate').innerHTML = msg[0].tostatename;
                document.getElementById('lbl_tostatecode').innerHTML = msg[0].tostatecode;
                document.getElementById('lblvendorphoneno').innerHTML = msg[0].phoneno;
                document.getElementById('lblvendoremail').innerHTML = msg[0].email;
                document.getElementById('lblsignname').innerHTML = msg[0].titlename;
                document.getElementById('lbl_companymobno').innerHTML = msg[0].companyphone;
                document.getElementById('lbl_companyemail').innerHTML = msg[0].companyemail;
                document.getElementById('spncompanytinno').innerHTML = msg[0].companytinno;
                document.getElementById('spnbuyertinno').innerHTML = msg[0].buyerTinNumber;
                document.getElementById('spnpanNO').innerHTML = msg[0].companypanno;
                document.getElementById('lblbuyercompanyname').innerHTML = msg[0].buyercompanyname;
                document.getElementById('spnhead').innerHTML = msg[0].dctype;

            }
        }
        var TotalAmount = 0; var totamount = 0;
        var TotVat = 0.0;
        function filldetails(msg, Aagent_Inventary) {
            var results = '<div><table border="1" id="tbl_po_print" style="width: 100%;" class="table table-bordered table-hover dataTable no-footer">';
            results += '<thead><tr style="background:antiquewhite;"><th value="#" colspan="1" style = "font-size: 12px;" rowspan="2">Sno</th><th style = "font-size: 12px;" value="Item Name" colspan="1" rowspan="2">Item Description</th><th value="UOM" style = "font-size: 12px;" colspan="1" rowspan="2">UOM</th><th style = "font-size: 12px;" value="HSN CODE" colspan="1" rowspan="2">HSN CODE</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="2">Qty</th><th value="Rate/Item (Rs.)" style = "font-size: 12px;" colspan="1" rowspan="2">Rate/Item (Rs.)</th><th value="Taxable Value" style = "font-size: 12px;" colspan="1" rowspan="2">Taxable Value</th><th value="CGST" style = "font-size: 12px;" colspan="2" rowspan="1">SGST</th><th value="SGST" colspan="2" style = "font-size: 12px;" rowspan="1">CGST</th><th value="IGST" style = "font-size: 12px;" colspan="2" rowspan="1">IGST</th><th value="Taxable Value" style = "font-size: 12px;" colspan="1" rowspan="2">Total Amount</th></tr><tr style="background:antiquewhite;"><th value="%" style = "font-size: 12px;" colspan="1" rowspan="1">%</th><th style = "font-size: 12px;" value="Amt (Rs.)" colspan="1" rowspan="1">Amt (Rs.)</th><th value="%" style = "font-size: 12px;" colspan="1" rowspan="1">%</th><th style = "font-size: 12px;" value="Amt (Rs.)" colspan="1" rowspan="1">Amt (Rs.)</th><th value="%" style = "font-size: 12px;" colspan="1" rowspan="1">%</th><th value="Amt (Rs.)" colspan="1" rowspan="1" style = "font-size: 12px;">Amt (Rs.)</th></tr></thead>';
            var tot_taxablevalue = 0;
            var tot_sgstamount = 0;
            var tot_cgstamount = 0;
            var tot_igstamount = 0;
            var tot_totalamount = 0;
            var tot_qty = 0;
            var msglength = msg.length;
            document.getElementById('spnhead').innerHTML = msg[msglength - 1].dctype;
            document.getElementById('spninvoiceno').innerHTML = msg[msglength - 1].invoiceno;

            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="font-size: 12px;">'
                results += '<td scope="row" class="1"  style="text-align:center;">' + msg[i].sno + '</td>';
                //                results += '<td scope="row" class="1"  style="text-align:center;">' + msg[i].itemcode + '</td>';
                var tproductname = msg[i].Description;
                if (tproductname == "") {
                    results += '<td data-title="brandstatus" class="2">' + msg[i].ProductName + '</td>';
                }
                else {
                    results += '<td data-title="brandstatus" class="2">' + msg[i].ProductName + '(' + msg[i].Description + ')</td>';
                }
                results += '<td data-title="brandstatus" class="2">' + msg[i].uom + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].hsncode + '</td>';
                results += '<td data-title="brandstatus" style = "text-align:center;" class="2">' + parseFloat(msg[i].qty).toFixed(2) + '</td>';
                results += '<td data-title="brandstatus" style = "text-align:center;" class="2">' + msg[i].rate + '</td>';
                //                results += '<td data-title="brandstatus" class="2">' + msg[i].discount + '</td>';
                results += '<td data-title="brandstatus" style = "text-align:center;" class="2">' + parseFloat(msg[i].taxablevalue).toFixed(2) + '</td>';
                tot_qty += parseFloat(msg[i].qty);
                tot_taxablevalue += parseFloat(msg[i].taxablevalue);
                tot_sgstamount += parseFloat(msg[i].sgstamount);
                tot_cgstamount += parseFloat(msg[i].cgstamount);
                tot_igstamount += parseFloat(msg[i].igstamount);
                tot_totalamount += parseFloat(msg[i].totalamount);
                results += '<td data-title="brandstatus" style = "text-align:center;" class="2">' + msg[i].sgst + '</td>';
                results += '<td data-title="brandstatus" style = "text-align:center;" class="2">' + parseFloat(msg[i].sgstamount).toFixed(2) + '</td>';
                results += '<td data-title="brandstatus" style = "text-align:center;" class="2">' + msg[i].cgst + '</td>';
                results += '<td data-title="brandstatus" style = "text-align:center;" class="2">' + parseFloat(msg[i].cgstamount).toFixed(2) + '</td>';
                results += '<td data-title="brandstatus" style = "text-align:center;" class="2">' + msg[i].igst + '</td>';
                results += '<td data-title="brandstatus" style = "text-align:center;" class="2">' + parseFloat(msg[i].igstamount).toFixed(2) + '</td>';
                var total = 0;
                results += '<td data-title="brandstatus" style = "text-align:center;" class="2">' + parseFloat(msg[i].totalamount).toFixed(2) + '</td></tr>';
            }
            var Total = "Total";
            results += '<tr>';
            results += '<td style = "font-size: 12px;text-align:center;background:antiquewhite;" colspan="4"><label>' + Total + '</label></td>';
            results += '<td style = "font-size: 12px;text-align:center;"><label>' + parseFloat(tot_qty).toFixed(2) + '</label></td>';
            results += '<td style = "font-size: 12px;text-align:center;background:antiquewhite;" colspan="1"><label></label></td>';
            results += '<td style = "font-size: 12px;text-align:center;"><label>' + parseFloat(tot_taxablevalue).toFixed(2) + '</label></td>';
            results += '<td colspan="2" style="text-align:center;font-size: 12px;"><label>' + parseFloat(tot_sgstamount).toFixed(2) + '</label></td>';
            results += '<td colspan="2" style="text-align:center;font-size: 12px;"><label>' + parseFloat(tot_cgstamount).toFixed(2) + '</label></td>';
            results += '<td colspan="2" style="text-align:center;font-size: 12px;"><label>' + parseFloat(tot_igstamount).toFixed(2) + '</label></td>';
            results += '<td style="font-size: 12px;"><label>' + parseFloat(tot_totalamount).toFixed(2) + '</label></td></tr>';
            results += '</table></div>';
            $("#div_itemdetails").html(results);
            var roundoff = Math.round(tot_totalamount);
            document.getElementById('recevied').innerHTML = inWords(parseInt(roundoff)) + "/-";
        }
        var a = ['', 'One ', 'Two ', 'Three ', 'Four ', 'Five ', 'Six ', 'Seven ', 'Eight ', 'Nine ', 'Ten ', 'Eleven ', 'Twelve ', 'Thirteen ', 'Fourteen ', 'Fifteen ', 'Sixteen ', 'Seventeen ', 'Eighteen ', 'Nineteen '];
        var b = ['', '', 'Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];

        function inWords(num) {
            if ((num = num.toString()).length > 9) return 'overflow';
            n = ('000000000' + num).substr(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
            if (!n) return; var str = '';
            str += (n[1] != 0) ? (a[Number(n[1])] || b[n[1][0]] + ' ' + a[n[1][1]]) + 'Crore ' : '';
            str += (n[2] != 0) ? (a[Number(n[2])] || b[n[2][0]] + ' ' + a[n[2][1]]) + 'Lakh ' : '';
            str += (n[3] != 0) ? (a[Number(n[3])] || b[n[3][0]] + ' ' + a[n[3][1]]) + 'Thousand ' : '';
            str += (n[4] != 0) ? (a[Number(n[4])] || b[n[4][0]] + ' ' + a[n[4][1]]) + 'Hundred ' : '';
            str += (n[5] != 0) ? ((str != '') ? 'and ' : '') + (a[Number(n[5])] || b[n[5][0]] + ' ' + a[n[5][1]]) + 'Only ' : '';
            return str;
        }
       
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
             Agent Invoice<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Masters</a></li>
            <li><a href="#"> Agent Invoice</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div>
                    <ul class="nav nav-tabs">
                        <li id="id_tab_documents" class=""><a data-toggle="tab" href="#" onclick="show_NewInvoice_format();">
                            <i class="fa fa-file-text"></i>&nbsp;&nbsp;AgentInvoice Summary</a></li>
                            <li id="Li1" class=""><a data-toggle="tab" href="#" onclick="show_Agent_invoicesummary();">
                            <i class="fa fa-file-text"></i>&nbsp;&nbsp;Agent Invoice</a></li>
                    </ul>
                </div>
                </div>
             <div id="div_dispatchMobile1" style="display:none;">
         <div class="box box-info">
               <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Agent Summary Details
                </h3>
            </div>
           <div class="box-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="true">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <asp:Panel ID="PBranch" runat="server">
                                        <asp:DropDownList ID="ddlSalesOfficesumary" runat="server" CssClass="form-control" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlSalesOfficesumary_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </asp:Panel>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDispNamesumary" runat="server" CssClass="form-control" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlDispNamesumary_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAgentNamesumary" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </td>
                                 <td style="width: 5px;">
                                    </td>
                                    <td>
                                    <asp:Panel ID="Categorypannel" runat="server">
                                    <asp:DropDownList ID="ddlCategoryName" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                     </asp:Panel>
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
                                    <asp:Button ID="Button2" Text="Generate" runat="server" OnClientClick="OrderValidate();"
                                        CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                 <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="PanelHide" runat="server" Visible="false">
                            <div id="divPrint1">
                                <div style="width: 100%;">
                                    <div style="width: 11%; float: left;">
                                        <%--<img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="120px"
                                            height="135px" />--%>
                                    </div>
                                    <div style="left: 0%; text-align: center;">
                                       <%-- <asp:Label ID="lblTitle2" runat="server" Font-Bold="true" Font-Size="26px" ForeColor="#0252aa"
                                            Text=""></asp:Label>--%>
                                        <br />
                                        <asp:Label ID="lblAddress" runat="server" Font-Bold="true" Font-Size="12px" ForeColor="#0252aa"
                                            Text=""></asp:Label>
                                        <br /><br /><br /><br /><br /><br /></br><br /></br>
                                    </div>
                                    <div>
                                    </div>
                                    <br />
                                    <div style="width: 100%;text-align: center;">
                                      <asp:Label ID="lbladdress1" runat="server" style="font-size: 18px; font-weight: bold;color: #0252aa;" Text=""></asp:Label><asp:Label ID="lblfromdate" runat="server" Font-Bold="true" Text="" ForeColor="Red"></asp:Label> <asp:Label ID="lblTo" runat="server" Font-Bold="true" Text="To"></asp:Label>  <asp:Label ID="lbltodate" runat="server" Font-Bold="true" Text="" ForeColor="Red"></asp:Label><br />
                                           <asp:Label ID="lblAgent" runat="server" style="font-size: 18px; font-weight: bold;color: #0252aa;" Text=""></asp:Label>
                                            <asp:Label ID="lblAgent1" runat="server" style="font-size: 18px; font-weight: bold;color: #0252aa;" Text=""></asp:Label>
                                         <br /> <br />
                                        <div>
                                            <div style="width: 40%; float: left; padding-left: 7%;">
                                                <asp:Label ID="Label1" runat="server" style="font-weight: bold;" Text="Agent Name"></asp:Label>
                                               <asp:Label ID="lblAgentDescription" runat="server" style="font-weight: bold;color: #0252aa;" Text=""></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--<br /><br />--%>
                                <asp:GridView ID="grdReports" runat="server" ForeColor="White" Width="100%" CssClass="EU_DataTable"
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
                                <br /><br />
                                <asp:GridView ID="grdReports1" runat="server" ForeColor="White" Width="100%" CssClass="EU_DataTable"
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
                                <br /><br />
                                <table align="right">
                                 <tr>
                                                <td style="float: left;">
                                                    <b>
                                                        <asp:Label ID="lblReceived" runat="server" CssClass="mylbl" Text=""></asp:Label>
                                                    </b>
                                                </td>
                                </tr>
                                <tr>
                                <td style="width: 50%;">
                                </td>
                                </tr>
                                </table>
                                <br />
                                <br />

                                 <table style="width: 100%;" id="Description">
                        <tr>
                            <td style="width: 25%;">
                               <asp:Label ID="lblprepared" runat="server" style="font-weight: bold;color: #0252aa;" Text=""></asp:Label><%-- <span style="font-weight: bold; font-size: 12px;">PREPARED BY </span>--%>
                            </td>
                            <td style="width: 25%;">
                                <asp:Label ID="lblaudit" runat="server" style="font-weight: bold;color: #0252aa;" Text=""></asp:Label><%--<span style="font-weight: bold; font-size: 12px;">Audit  BY</span>--%>
                                <br />
                            </td>
                            <td style="width: 25%;">
                                <asp:Label ID="lblauthorised" runat="server" style="font-weight: bold;color: #0252aa;" Text=""></asp:Label><%--<span style="font-weight: bold; font-size: 12px;">Authorised By</span>--%>
                            </td>
                             <td style="width: 25%;">
                            <asp:Label ID="lblapproved" runat="server" style="font-weight: bold;color: #0252aa;" Text=""></asp:Label>    <%--<span style="font-weight: bold; font-size: 12px;">APPROVED BY</span>--%>
                            </td>
                        </tr>
                    </table>



                                <table align="right" id="tblcass">
                                <tr>
                                <td style="width: 50%;">
                                </td>
                                <td style="width: 10%;">
                                </td>
                                    <caption>
                                        <br />
                                        <br />
                                        <tr>
                                        <td style="width: 50%;">
                                        </td>
                                            <td style="width: 50%;">
                                                <asp:Label ID="lbl1" runat="server" style="font-weight: bold;color: #0252aa;" Text="HEAD MARETING"></asp:Label>
                                                <br />
                                                <asp:Label ID="lbl2" runat="server" style="font-weight: bold;color: #0252aa;" Text="SRI VYSHNAVI DAIRY SPECIALITIES (P) LTD"></asp:Label>
                                                <br/>
                                                <asp:Label ID="lblbranch" runat="server" style="font-weight: bold;color: #0252aa;" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </caption>
                            </tr>
                        </table>
                            </div>
                        </asp:Panel>
                         <asp:Panel ID="Panel_button" runat="server" Visible="false">
                                    <div align="left">
                            <input type="button" class="btn btn-primary" value="Print" onclick="javascript:CallPrint1('divPrint1');" />
                            </div>
                            </asp:Panel>
                        </ContentTemplate>
                </asp:UpdatePanel>
               
              <%-- <asp:Panel ID="Panel_button" runat="server" Visible="false">
                        <div align="left">
                <input type="button" class="btn btn-primary" value="Print" onclick="javascript:CallPrint1('divPrint1');" />
                </div>
                </asp:Panel>--%>
                <asp:Label ID="Label5" runat="server" Font-Size="20px" ForeColor="Red" Text=""></asp:Label>
                </div>
                    </div>
                    </div>
       
                    <div id="ks" style="display:none;" >
                    <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Agent Invoice Details
                </h3>
            </div>
            <div class="box-body">
                <table>
                            <tr>
                                <td>
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
                                <td>
                                  <input type="date" id="txtFrom_date" class="form-control" />
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                            <select id="ddltaxtype" class="form-control">
                                                <option value="NonTax">NonTax</option>
                                               <option value="Tax">Tax</option>
                                             </select>
                                            </td>
                                            <td style="width: 5px;">
                                </td>
                                <td>
                                  <input type="button" class="btn btn-primary" id="btnGenerate" value="Generate" onclick="get_Agent_Invoice_Data();" />
                                </td>
                            </tr>
                        </table>
                    <div id="divPrint" style="display: none;height:50%;">
                    <div style="border:2px solid gray;">
                    <div style="width: 17%; float: left; padding-top:5px;">
                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="100px" height="72px" />
                        <br />
                    </div>
                    <div style="border:1px solid gray;text-align:  center;">
                        <div style="font-family: Arial;font-size: 20px;font-weight: bold;color: Black;">
                            <span id="lbltile"> </span>
                            <br />
                        </div>
                        <div>    
                        <span id="spnAddress" style="font-size: 14px;"></span>
                        <br />
                          <%-- <span id="Span1" style="font-size: 14px;font-weight: bold;">Website: www.vyshnavi.in</span>--%>
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
                        <span id="spnhead" style="font-size: 18px; font-weight: bold;"></span>
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
                                    <span id="spnfromname" style="font-size: 13px;"></span>
                                    <br>
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Address :</label>
                                    <span id="spnfromaddress" style="font-size: 13px;"></span>
                                    <br>
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                         GSTIN :</label>
                                    <span id="spnfromgstn" style="font-size: 14px;font-weight: bold !important;"></span>
                                    <br>
                                     <label style="font-size: 12px;font-weight: bold !important;">
                                        Telephone no :</label>
                                    <span id="lbl_companymobno" style="font-size: 13px;"></span>
                                     &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Email Id :</label>
                                    <span id="lbl_companyemail" style="font-size: 13px;"></span>
                                    <br />
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        State Name :</label>
                                    <span id="spnfromstate" style="font-size: 13px;"></span>
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                       State Code :</label>
                                    <span id="spnfromstatecode" style="font-size: 13px;"></span>
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
                                    <label style="font-size: 12px;">
                                        Buyer's OrderNo :</label>
                                    <span id="spnorderno" style="font-size: 14px;"></span> <br />
                                    <a id="H1" style="display:none; color: black !important;"><span id="Span2" style="font-size: 14px;padding-left: 35%; display:none;"></span></a>
                                   <a id="H2" style="display:none; color: black !important;"> <span id="Span3" style="font-size: 14px;padding-left: 35%; display:none;"></span></a>
                                   <a id="H3" style="display:none; color: black !important;"> <span id="Span4" style="font-size: 14px;padding-left: 35%; display:none;"></span></a>
                                   <a id="H4" style="display:none; color: black !important;"> <span id="Span5" style="font-size: 14px;padding-left: 35%; display:none;"></span></a>
                                   <a id="H5" style="display:none; color: black !important;"> <span id="Span6" style="font-size: 14px;padding-left: 35%; display:none;"></span></a>
                                    <label style="font-size: 12px;">
                                        Grn No :</label>
                                    <span id="spngrnno" style="font-size: 14px;"></span>
                                    <a id="H6" style="display:none; color: black !important;"><span id="Span7" style="font-size: 14px;padding-left: 17%; display:none;"></span></a>
                                   <a id="H7" style="display:none; color: black !important;"> <span id="Span8" style="font-size: 14px;padding-left: 17%; display:none;"></span></a>
                                   <a id="H8" style="display:none; color: black !important;"> <span id="Span9" style="font-size: 14px;padding-left: 17%; display:none;"></span></a>
                                   <a id="H9" style="display:none; color: black !important;"> <span id="Span10" style="font-size: 14px;padding-left: 17%; display:none;"></span></a>
                                   <a id="H10" style="display:none; color: black !important;"> <span id="Span11" style="font-size: 14px;padding-left: 17%; display:none;"></span></a>
                                    <br />
                                    <label style="font-size: 12px;">
                                       DcNo :</label>
                                    <span id="spndcno" style="font-size: 14px;"></span>
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
                                        
                                     <span id="lblbuyercompanyname" style="font-size: 13px;font-weight: bold !important;"></span>
                                     <br />
                                    <span id="lblpartyname" style="font-size: 13px;font-weight: bold !important;"></span>
                                    <br>
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Address :</label>
                                    <span id="spn_toaddress" style="font-size: 13px;"></span>
                                    <br>
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        GSTIN :</label>
                                    <span id="span_toGSTIN" style="font-size: 13px;"></span>
                                    <br>
                                     <label style="font-size: 12px;font-weight: bold !important;">
                                        State Name :</label>
                                    <span id="lbl_tostate" style="font-size: 13px;"></span>
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                     <label style="font-size: 12px;font-weight: bold !important;">
                                        State Code :</label>
                                    <span id="lbl_tostatecode" style="font-size: 13px;"></span>
                                    <br>
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Telephone no :</label>
                                    <span id="lblvendorphoneno" style="font-size: 13px;"></span>
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Email Id :</label>
                                    <span id="lblvendoremail" style="font-size: 13px;"></span>
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
                <table class="table table-bordered table-hover dataTable no-footer"  style="width: 100%;">
                            <tr>
                            <td style="width: 50%;">
                             <label>
                            1.Company TIN :</label>
                        <span id="spncompanytinno"></span>
                        <br />
                         <br />
                        <label>
                            2. Buyer TIN :</label>
                        <span id="spnbuyertinno"></span>
                        <br />   <br />
                        <label>
                            3. Company PAN:</label>
                        <span id="spnpanNO"></span>
                        <br />   <br />
                        </td>
                        </tr>
                        </table>
                <br />
                <table style="width: 100%;">
                 <tr> 
                                     <td style="width: 25%;" colspan="3"></td>
                                       <td style="float: right;padding-right: 4%;" >
                                            For  <br /><span id="lblsignname" style="font-weight: bold; font-size: 12px;"></span>
                                       <br />
                                       <br />
                                       <br />
                                       </td>
                                    </tr>
                                        <tr>
                                            <td style="width: 20%;">
                                                
                                            </td>
                                            <td style="width: 15%;">
                                            </td>
                                            <td style="width: 25%;">
                                               
                                            </td>
                                            <%--<td style="width: 25%;">
                                               
                                            </td>--%>
                                            <td style="width: 50%;">
                                                <span style="font-weight: bold; font-size: 12px;">Authorised Signatory</span>
                                                
                                            </td>
                                        </tr>
                                    </table>
                                    <br/>
                                      <div>
                                        <span style="font-weight: bold; font-size: 13px;">Decleration:</span>
                        <br />
                                    <span style="font-size: 14px;">We declare that this Invoice shows the actual price of the goods described and that all particulars are true and correct</span>
                                    <br />
                                    </div>
                </div>
                </div>
                    <div id="div_agetdet" style="display:none;">
                      <input type="button" class="btn btn-primary" value="Print" onclick="javascript:CallPrint('divPrint');" /></div>
                </div>
                </div>
              <%--  <input type="button" class="btn btn-primary" value="Print" onclick="javascript:CallPrint('divPrint');" />--%>
                    </div>
                     <asp:Label ID="lblmsg" runat="server" Font-Size="20px" ForeColor="Red" Text=""></asp:Label>
    </section>
</asp:Content>
