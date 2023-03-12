<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EinoviceTemplate.aspx.cs" Inherits="EinoviceTemplate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="Js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="http://www.jqueryscript.net/css/jquerysctipttop.css" rel="stylesheet"
        type="text/css" />
    <script src="http://code.jquery.com/jquery-latest.min.js" type="text/javascript"></script>
    <script src="Barcode/jquery-barcode.js" type="text/javascript"></script>
    <style type="text/css">
        .ddlDropStatus {
            width: 100%;
            height: 34px;
            border: 1px solid gray;
            border-radius: 6px 6px 6px 6px;
            font-size: 16px;
        }

        .removeclass {
            background: #fd2053 !important;
            border-radius: 100% !important;
            padding: 0px !important;
            height: 30px !important;
            width: 30px !important;
            color: #ffffff !important;
            border-color: #fd2053 !important;
        }

        .prntcls {
            background: #00c0ef !important;
            border-radius: 100% !important;
            padding: 0px !important;
            height: 30px !important;
            width: 30px !important;
            color: #ffffff !important;
            border-color: #00c0ef !important;
        }

        .container {
            max-width: 100%;
        }

        th {
            text-align: center;
        }

        #config {
            overflow: auto;
            margin-bottom: 10px;
        }

        .config {
            float: left;
            width: 200px;
            height: 250px;
            border: 1px solid #000;
            margin-left: 10px;
        }

            .config .title {
                font-weight: bold;
                text-align: center;
            }

            .config .barcode2D, #miscCanvas {
                display: none;
            }

        #submit {
            clear: both;
        }

        #barcodeTarget, #canvasTarget {
            margin-top: 20px;
        }

        #qrcode {
            width: 160px;
            height: 160px;
            margin-top: 15px;
        }
    </style>
    <script type="text/javascript">

        $(function () {
            window.history.forward(1);
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#txt_Docdate').val(today);
            $('.divsalesOffice').css('display', 'table-row');
            FillSalesOffice();

        });
        function FillSalesOffice() {
            var data = { 'operation': 'GetSalesOffice' };
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
            var ddlsalesOffice = document.getElementById('ddlsalesOffice');
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

        function GenerateClick() {
            var FromDate = document.getElementById('txtFromDate').value;
            if (FromDate == "") {
                alert("Please From Date");
                return false;
            }
            var type = document.getElementById('ddltype').value;
            if (type == "") {
                alert("Please select type");
                return false;
            }
            var branchID = document.getElementById("ddlsalesOffice").value;
            var data = { 'operation': 'Get_Agent_Einvoice_Details', 'FromDate': FromDate, 'BranchID': branchID, 'type': type };
            var s = function (msg) {
                if (msg == "Data not Found") {
                    alert(msg);
                    $("#divEWayBilldata").html("");
                    return false;
                }
                else {
                    filleAgentdetails(msg);
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function filleAgentdetails(msg) {
            var type = document.getElementById('ddltype').value;
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            if (type == "invoice") {
                results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">AgentName</th><th scope="col" class="thcls">StateName</th><th scope="col" class="thcls">GstNo</th><th scope="col" class="thcls">TotalQty</th><th scope="col" class="thcls">TotalValue</th><th scope="col" class="thcls">Status</th><th scope="col"></th></tr></thead></tbody>';
            }
            else {
                results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">DcNo</th><th scope="col" class="thcls">DispatchName</th><th scope="col" class="thcls">StateName</th><th scope="col" class="thcls">GstNo</th><th scope="col" class="thcls">TotalQty</th><th scope="col" class="thcls">Status</th><th scope="col"></th></tr></thead></tbody>';
            }
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                if (type == "dc") {
                    results += '<td scope="row"  class="12" >' + msg[i].dcno + '</td>';
                }
                results += '<td scope="row"  class="1" tdmaincls" >' + msg[i].AgentName + '</td>';
                results += '<td data-title="brandstatus" style = "display:none;" class="2">' + msg[i].AgentId + '</td>';
                results += '<td data-title="brandstatus" style = "display:none;" class="7">' + msg[i].IRN_NO + '</td>';
                results += '<td data-title="brandstatus" style = "display:none;" class="8">' + msg[i].signed_qr_code + '</td>';
                results += '<td data-title="brandstatus" style = "display:none;" class="9">' + msg[i].ack_no + '</td>';
                results += '<td data-title="brandstatus" style = "display:none;" class="10">' + msg[i].ack_date + '</td>';
                results += '<td data-title="brandstatus" style = "display:none;" class="13">' + msg[i].dispsno + '</td>';
                results += '<td data-title="brandstatus"  class="3">' + msg[i].StateName + '</td>';
                results += '<td data-title="brandstatus"  class="4">' + msg[i].GstNo + '</td>';
                if (type == "dc") {
                    results += '<td data-title="brandstatus"  class="5">' + msg[i].Totalqty + '</td>';
                }
                else {
                    results += '<td data-title="brandstatus"  class="5">' + msg[i].Totalqty + '</td>';
                    results += '<td data-title="brandstatus"  class="6">' + msg[i].Totalvalue + '</td>';
                }
                var status = msg[i].status;
                if (status == "R") {
                    status = "Raised"
                }
                if (type == "dc") {
                    if (msg[i].status == "R") {
                        results += '<td data-title="brandstatus"  class="11">' + status + '</td>';
                        results += '<td data-title="brandstatus"><button type="button" disabled="true" title="Click Here To Generate Einvoice!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="GenerateEinvoice(this)"><i class="fa fa-file-text"></i></button></td>';
                        results += '<td data-title="brandstatus"><button type="button" title="Click Here To View Einvoice!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 prntcls"  onclick="View_DC_Einvoice_Click(this)"><span class="glyphicon glyphicon-list-alt" style="top: 0px !important;"></span></button></td>';
                    }
                    else {
                        results += '<td data-title="brandstatus"  class="11">' + status + '</td>';
                        results += '<td data-title="brandstatus"><button  type="button" title="Click Here To Generate Einvoice!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="GenerateEinvoice(this)"><i class="fa fa-file-text"></i></button></td>';
                        results += '<td data-title="brandstatus"><button type="button" disabled="true" title="Click Here To View Einvoice!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 prntcls"  onclick="View_DC_Einvoice_Click(this)"><span class="glyphicon glyphicon-list-alt" style="top: 0px !important;"></span></button></td>';
                    } 
                }
                else {
                    if (msg[i].status == "R") {
                        results += '<td data-title="brandstatus"  class="11">' + status + '</td>';
                        results += '<td data-title="brandstatus"><button type="button" disabled="true" title="Click Here To Generate Einvoice!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="GenerateEinvoice(this)"><i class="fa fa-file-text"></i></button></td>';
                        results += '<td data-title="brandstatus"><button type="button" title="Click Here To View Einvoice!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 prntcls"  onclick="View_Einvoice_Click(this)"><span class="glyphicon glyphicon-list-alt" style="top: 0px !important;"></span></button></td>';

                    }
                    else {
                        results += '<td data-title="brandstatus"  class="11">' + status + '</td>';
                        results += '<td data-title="brandstatus"><button  type="button" title="Click Here To Generate Einvoice!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="GenerateEinvoice(this)"><i class="fa fa-file-text"></i></button></td>';
                        results += '<td data-title="brandstatus"><button type="button" disabled="true" title="Click Here To View Einvoice!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 prntcls"  onclick="View_Einvoice_Click(this)"><span class="glyphicon glyphicon-list-alt" style="top: 0px !important;"></span></button></td>';
                    }
                }
                results += '<td data-title="brandstatus"><button type="button" disabled="true" title="Click Here To Cancel Einvoice!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 removeclass"   onclick="CancelEinvoice(this)"><span class="glyphicon glyphicon-remove-circle" style="top: 0px !important;"></span></button></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#divEWayBilldata").html(results);
        }
        //Generate EInvoice
        function GenerateEinvoice(thisid) {
            var FromDate = document.getElementById('txtFromDate').value;
            if (FromDate == "") {
                alert("Please From Date");
                return false;
            }
            var type = document.getElementById('ddltype').value;
            if (type == "") {
                alert("Please select type");
                return false;
            }
            var AgentID = $(thisid).parent().parent().children('.2').html();
            var Totvalue = $(thisid).parent().parent().children('.6').html();
            var branchID = document.getElementById("ddlsalesOffice").value;
            var dcno = $(thisid).parent().parent().children('.12').html();
            var data = { 'operation': 'generate_e_invoice_details', 'AgentID': AgentID, 'FromDate': FromDate, 'SOID': branchID, 'dcno': dcno, 'type': type };
            var s = function (msg) {
                if (msg.length > 0) {
                    alert(msg);
                    GenerateClick();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        //EInvoice_Get_Json Response

        function btn_Click_GetInvoice(thisid) {
            var FromDate = document.getElementById('txtFromDate').value;
            if (FromDate == "") {
                alert("Please From Date");
                return false;
            }
            var AgentID = $(thisid).parent().parent().children('.2').html();
            var Irn_No = $(thisid).parent().parent().children('.7').html();

            var branchID = document.getElementById("ddlsalesOffice").value;
            var data = { 'operation': 'btn_Click_GetInvoice', 'AgentID': AgentID, 'FromDate': FromDate, 'SOID': branchID, 'Irn_No': Irn_No };
            var s = function (msg) {
                if (msg) {
                    spnJsonData.innerHTML = JSON.stringify(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        //EInvoice_Print
        function View_Einvoice_Click(thisid) {
            var fromdate = document.getElementById('txtFromDate').value;
            if (fromdate == "") {
                alert("Please From Date");
                return false;
            }
            var AgentID = $(thisid).parent().parent().children('.2').html();
            var Irn_No = $(thisid).parent().parent().children('.7').html();
            var Ack_No = $(thisid).parent().parent().children('.9').html();
            var Ack_Date = $(thisid).parent().parent().children('.10').html();
            var signed_qr_code = $(thisid).parent().parent().children('.8').html();

            var branchID = document.getElementById("ddlsalesOffice").value;
            var ddltype = "Tax";

            var data = { 'operation': 'btnAgent_indent_Invoice_click', 'fromdate': fromdate, 'AgentId': AgentID, 'SOID': branchID, 'ddltype': ddltype };
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


                        GenerateQRCode(signed_qr_code);
                        generateBarcode(Ack_No);
                        document.getElementById('spIrn_No').innerHTML = Irn_No;
                        document.getElementById('spnAck_No').innerHTML = Ack_No;
                        document.getElementById('spnAck_Date').innerHTML = Ack_Date;
                        $("#divPrint").css("display", "block");
                        $("#btn_Print").css("display", "block");
                        $('#divMainAddNewRow1').css('display', 'block');
                        $('#div_itemdetails1').css('display', 'block');
                        $('#myModal').css('display', 'block');

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
            document.getElementById('spnplaceofsupply').innerHTML = "";
            document.getElementById('spnfromname').innerHTML = "";
            document.getElementById('spnfromaddress').innerHTML = "";
            document.getElementById('spnfromgstn').innerHTML = "";
            document.getElementById('spngstnum').innerHTML = "";

            document.getElementById('spnfromstate').innerHTML = "";
            document.getElementById('spnfromstatecode').innerHTML = "";
            document.getElementById('lblpartyname').innerHTML = "";
            document.getElementById('spn_toaddress').innerHTML = "";
            document.getElementById('lbl_tostate').innerHTML = "";
            document.getElementById('lbl_tostatecode').innerHTML = "";
            document.getElementById('lblvendorphoneno').innerHTML = "";
            document.getElementById('lblvendoremail').innerHTML = "";
            document.getElementById('lblsignname').innerHTML = "";
            document.getElementById('lbl_companymobno').innerHTML = "";
            document.getElementById('lbl_companyemail').innerHTML = "";
            document.getElementById('spninvoicetype').innerHTML = "";
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
                document.getElementById('spnplaceofsupply').innerHTML = msg[0].city;
                document.getElementById('spnfromname').innerHTML = msg[0].titlename;
                document.getElementById('spnfromaddress').innerHTML = msg[0].BranchAddress;
                document.getElementById('spnfromgstn').innerHTML = msg[0].fromgstn;
                document.getElementById('spngstnum').innerHTML = msg[0].fromgstn;
                document.getElementById('spnfromstate').innerHTML = msg[0].frmstatename;
                document.getElementById('spnfromstatecode').innerHTML = msg[0].frmstatecode;
                document.getElementById('lblpartyname').innerHTML = msg[0].AgentName;
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
            results += '<thead><tr style="background:antiquewhite;"><th value="#" colspan="1" style = "font-size: 12px;" rowspan="2">Sno</th><th value="Item Code" style = "font-size: 12px;" colspan="1" rowspan="2">Item Code</th><th style = "font-size: 12px;" value="Item Name" colspan="1" rowspan="2">Item Description</th><th value="UOM" style = "font-size: 12px;" colspan="1" rowspan="2">UOM</th><th style = "font-size: 12px;" value="HSN CODE" colspan="1" rowspan="2">HSN CODE</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="2">Qty</th><th value="Rate/Item (Rs.)" style = "font-size: 12px;" colspan="1" rowspan="2">Rate/Item (Rs.)</th><th value="Discount (Rs.)" style = "font-size: 12px;" colspan="1" rowspan="2">Discount (Rs.)</th><th value="Taxable Value" style = "font-size: 12px;" colspan="1" rowspan="2">Taxable Value</th><th value="CGST" style = "font-size: 12px;" colspan="2" rowspan="1">SGST</th><th value="SGST" colspan="2" style = "font-size: 12px;" rowspan="1">CGST</th><th value="IGST" style = "font-size: 12px;" colspan="2" rowspan="1">IGST</th><th value="Taxable Value" style = "font-size: 12px;" colspan="1" rowspan="2">Total Amount</th></tr><tr style="background:antiquewhite;"><th value="%" style = "font-size: 12px;" colspan="1" rowspan="1">%</th><th style = "font-size: 12px;" value="Amt (Rs.)" colspan="1" rowspan="1">Amt (Rs.)</th><th value="%" style = "font-size: 12px;" colspan="1" rowspan="1">%</th><th style = "font-size: 12px;" value="Amt (Rs.)" colspan="1" rowspan="1">Amt (Rs.)</th><th value="%" style = "font-size: 12px;" colspan="1" rowspan="1">%</th><th value="Amt (Rs.)" colspan="1" rowspan="1" style = "font-size: 12px;">Amt (Rs.)</th></tr></thead>';
            var tot_taxablevalue = 0;
            var tot_sgstamount = 0;
            var tot_cgstamount = 0;
            var tot_igstamount = 0;
            var tot_totalamount = 0;
            var tot_qty = 0;
            var msglength = msg.length;
            if (msg.length > 0) {
                //document.getElementById('spninvoicetype').innerHTML = msg[0].dctype;
                document.getElementById('spninvoiceno').innerHTML = msg[msglength - 1].invoiceno;
                //document.getElementById('spntempinvoiceno').innerHTML = msg[msglength - 1].TempInvoice;
            }
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="font-size: 12px;">'
                results += '<td scope="row" class="1"  style="text-align:center;">' + msg[i].sno + '</td>';
                results += '<td scope="row" class="1"  style="text-align:center;">' + msg[i].itemcode + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].ProductName + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].uom + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].hsncode + '</td>';
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
            $("#div_itemdetails1").html(results);
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
        function closepopup() {
            $("#myModal").css("display", "none");
            $("#divMainAddNewRow1").css("display", "none");
            $("#div_itemdetails1").css("display", "none");

        }
        function CloseClick1() {
            $("#myModal").css("display", "none");
            $("#divMainAddNewRow1").css("display", "none");
            $("#div_itemdetails1").css("display", "none");
        }

        //bar code

        function generateBarcode(irn_no) {

            var value = "112315053672459";//irn_no;
            var btype = $("input[name=btype]:checked").val();
            var renderer = $("input[name=renderer]:checked").val();

            var quietZone = false;
            if ($("#quietzone").is(':checked') || $("#quietzone").attr('checked')) {
                quietZone = true;
            }

            var settings = {
                output: renderer,
                bgColor: $("#bgColor").val(),
                color: $("#color").val(),
                barWidth: $("#barWidth").val(),
                barHeight: $("#barHeight").val(),
                moduleSize: $("#moduleSize").val(),
                posX: $("#posX").val(),
                posY: $("#posY").val(),
                addQuietZone: $("#quietZoneSize").val()
            };
            if ($("#rectangular").is(':checked') || $("#rectangular").attr('checked')) {
                value = { code: value, rect: true };
            }
            if (renderer == 'canvas') {
                clearCanvas();
                $("#barcodeTarget").hide();
                $("#canvasTarget").show().barcode(value, btype, settings);
            } else {
                $("#canvasTarget").hide();
                $("#barcodeTarget").html("").show().barcode(value, btype, settings);
            }
        }

        function showConfig1D() {
            $('.config .barcode1D').show();
            $('.config .barcode2D').hide();
        }

        function showConfig2D() {
            $('.config .barcode1D').hide();
            $('.config .barcode2D').show();
        }

        function clearCanvas() {
            var canvas = $('#canvasTarget').get(0);
            var ctx = canvas.getContext('2d');
            ctx.lineWidth = 1;
            ctx.lineCap = 'butt';
            ctx.fillStyle = '#FFFFFF';
            ctx.strokeStyle = '#000000';
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            ctx.strokeRect(0, 0, canvas.width, canvas.height);
        }

        $(function () {
            $('input[name=btype]').click(function () {
                if ($(this).attr('id') == 'datamatrix') showConfig2D(); else showConfig1D();
            });
            $('input[name=renderer]').click(function () {
                if ($(this).attr('id') == 'canvas') $('#miscCanvas').show(); else $('#miscCanvas').hide();
            });

        });

        //bar code end

        //QR code
        function GenerateQRCode(signed_qr_code) {
            var data = "eyJhbGciOiJSUzI1NiIsImtpZCI6IkI4RDYzRUNCNThFQTVFNkY0QUFDM0Q1MjQ1NDNCMjI0NjY2OUIwRjgiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJ1TlkteTFqcVhtOUtyRDFTUlVPeUpHWnBzUGcifQ.eyJkYXRhIjoie1wiU2VsbGVyR3N0aW5cIjpcIjM2QUFHQ1M2MDIyRjFaSlwiLFwiQnV5ZXJHc3RpblwiOlwiMzZCVkRQRzQ2NDlFMVpPXCIsXCJEb2NOb1wiOlwiV1lSLzIyLTIzVC8zMjMwNlwiLFwiRG9jVHlwXCI6XCJJTlZcIixcIkRvY0R0XCI6XCIxMS8wMS8yMDIzXCIsXCJUb3RJbnZWYWxcIjo5MDkuMDMsXCJJdGVtQ250XCI6MyxcIk1haW5Ic25Db2RlXCI6XCIwNDAzOTAxMFwiLFwiSXJuXCI6XCIzM2FiNGFhNzBhODAzNmNiOWQ0Yjg4ZTlhNWI5MzYyNTIyY2I1ZmNjOWU4ZjY3MmViMzU1NDcyZjY2OWRmZWRjXCIsXCJJcm5EdFwiOlwiMjAyMy0wMS0xMSAxMToxODowMFwifSIsImlzcyI6Ik5JQyJ9.nGdvOpCRKCQsIEgoE54SjoGhjZAVMQQZCd_T7dOMXxorKcmXv5Y9Ti6q1BilLzpDrp1GwnidMS-CoEL83L3YZK5ONOk1AACEuTll6dCpfHNZldeUVOxDo0gmWHkOpPpZMICiW4cXvZEpu6G8FNxrhUgpLCLcQS9XMIpWtEvEv8B4ejvQ0DkRDi61ACTvP-_gq_JKsK8urj37SWI8Ow6Yx69EKfw0iVmNaHKo8Yqm-N5gS1cWXrOnNSu5wS_Zolm38wQPZbWGu8JDIG4rLIjBMeqqNkKi9LujRDKD1OUxHrDy0RDFa2cgw8H71x_vbSPt0YcPq0YEmlx-VOoNNG4gOg"//signed_qr_code;
            var size = "155";
            if (data == "") {
                alert('please enter a url or text');
                return false;
            } else {
                if ($("#image").is(':empty')) {
                    $("#image").append("<img src='http://chart.apis.google.com/chart?cht=qr&chl=" + data + "&chs=" + size + "' alt='qr' />");
                    return false;
                } else {
                    $("#image").html("");
                    $("#image").append("<img src='http://chart.apis.google.com/chart?cht=qr&chl=" + data + "&chs=" + size + "' alt='qr' />");
                    return false;
                }
            }
        }

        function CallPrint(strid) {
            document.getElementById("div_itemdetails1").style.borderCollapse = "collapse";
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }




// EInvoice print  for DC

        function View_DC_Einvoice_Click(thisid) {
            var refdcno = $(thisid).parent().parent().children('.12').html();
            var dispsno = $(thisid).parent().parent().children('.13').html();
            //document.getElementById('txt_Refno').value = refdcno;
            var DcType = "Tax";
            var Irn_No = $(thisid).parent().parent().children('.7').html();
            var Ack_No = $(thisid).parent().parent().children('.9').html();
            var Ack_Date = $(thisid).parent().parent().children('.10').html();
            var signed_qr_code = $(thisid).parent().parent().children('.8').html();

            var data = { 'operation': 'get_DeliveryChallan_click', 'refdcno': refdcno, 'DcType': DcType };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        FillDCMain_details(msg);
                        GetDC_Products(refdcno + "_" + dispsno);

                        GenerateQRCode(signed_qr_code);
                        generateBarcode(Ack_No);
                        document.getElementById('spIrn_No').innerHTML = Irn_No;
                        document.getElementById('spnAck_No').innerHTML = Ack_No;
                        document.getElementById('spnAck_Date').innerHTML = Ack_Date;
                        $("#divPrint").css("display", "block");
                        $("#btn_Print").css("display", "block");
                        $('#divMainAddNewRow1').css('display', 'block');
                        $('#div_itemdetails1').css('display', 'block');
                        $('#myModal').css('display', 'block');
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
        function FillDCMain_details(msg) {
            clearall();
            if (msg.length > 0) {
                document.getElementById('lbltile').innerHTML = msg[0].Title;
                document.getElementById('spnAddress').innerHTML = msg[0].Address;
                document.getElementById('spnfromaddress').innerHTML = msg[0].Address;
                document.getElementById('spngstnno').innerHTML = msg[0].fromgstin;
                document.getElementById('spnfromgstn').innerHTML = msg[0].fromgstin;
                document.getElementById('spngstnum').innerHTML = msg[0].fromgstin;
                document.getElementById('lbl_tostate').innerHTML = msg[0].tostatename;
                document.getElementById('lbl_tostatecode').innerHTML = msg[0].tostatecode;
                //document.getElementById('lbl_fromstate').innerHTML = msg[0].fromstatename;
                //document.getElementById('spnfstate').innerHTML = msg[0].fromstatename;
                document.getElementById('spnfromstate').innerHTML = msg[0].fromstatename;
                //document.getElementById('lbl_fromstate_code').innerHTML = msg[0].fromstatecode;
                document.getElementById('spnfromstatecode').innerHTML = msg[0].fromstatecode;
                //document.getElementById('lblRefdcno').innerHTML = document.getElementById('txt_Refno').value;
                document.getElementById('spninvoiceno').innerHTML = msg[0].DcNo;
                document.getElementById('spninvoicedate').innerHTML = msg[0].assigndate;
                //document.getElementById('spndateofsupply').innerHTML = msg[0].assigndate;
                //document.getElementById('lbldisptime').innerHTML = msg[0].PlanTime;
                document.getElementById('spnplaceofsupply').innerHTML = msg[0].city;
                //document.getElementById('hdnDespsno').value = msg[0].Dispatchsno;
                //document.getElementById('lblroutename').innerHTML = msg[0].routename;
                //document.getElementById('lblvehicleno').innerHTML = msg[0].vehicleno;
                //document.getElementById('lbldispat').innerHTML = msg[0].Dispatcher;
                //document.getElementById('lbldcType').innerHTML = msg[0].dctype;
                document.getElementById('spnfromname').innerHTML = msg[0].Title;
                document.getElementById('lbl_companymobno').innerHTML = msg[0].companyphone;
                document.getElementById('lbl_companyemail').innerHTML = msg[0].companyemail;
                document.getElementById('lblsignname').innerHTML = msg[0].Title;
                if (msg[0].dispmode == "LOCAL" || msg[0].dispmode == "Staff" || msg[0].dispmode == "Free") {
                    document.getElementById('lblpartyname').innerHTML = msg[0].partyname;
                }
                else {
                    if (msg[0].dispmode == "AGENT") {
                        document.getElementById('lblpartyname').innerHTML = msg[0].partyname;
                        document.getElementById('span_toGSTIN').innerHTML = msg[0].togstin;
                        document.getElementById('lblvendorphoneno').innerHTML = msg[0].phoneno;
                        document.getElementById('spn_toaddress').innerHTML = msg[0].AgentAddress;
                        document.getElementById('lblvendoremail').innerHTML = msg[0].email;
                    }
                    else {
                        document.getElementById('lblpartyname').innerHTML = msg[0].partyname;
                        document.getElementById('span_toGSTIN').innerHTML = msg[0].togstin;
                        document.getElementById('lblvendorphoneno').innerHTML = msg[0].phoneno;
                        document.getElementById('spn_toaddress').innerHTML = msg[0].AgentAddress;
                        document.getElementById('lblvendoremail').innerHTML = msg[0].email;
                    }
                }
            }
        }
        function GetDC_Products(refdcno) {
            var data = refdcno.split(/_/);
            var refno = data[0];
            var Dispatchsno = data[1];
            //var refno = refdcno;

            //var Dispatchsno = $(thisid).parent().parent().children('.13').html();
            //var Dispatchsno = dispsno;
            var DcType = "Tax";
            var data = { 'operation': 'GetDC_Products', 'refdcno': refno, 'DcType': DcType, 'Dispatchsno': Dispatchsno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillDC_Products(msg);
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
        function fillDC_Products(msg) {
            var TaxType = ""; //  document.getElementById('ddlTaxType').value;lblsignname
            var lbldcType = "Tax";
            var results = '<div  style="overflow:auto;"><table border="1" id="tbl_po_print" style="width: 100%;" class="table table-bordered table-hover dataTable no-footer">';
            results += '<thead><tr style="background:antiquewhite;"><th value="#" colspan="1" style = "font-size: 12px;" rowspan="2">Sno</th><th value="Item Code" style = "font-size: 12px;" colspan="1" rowspan="2">Item Code</th><th style = "font-size: 12px;" value="Item Name" colspan="1" rowspan="2">Item Description</th><th style = "font-size: 12px;" value="HSN CODE" colspan="1" rowspan="2">HSN CODE</th><th value="UOM" style = "font-size: 12px;" colspan="1" rowspan="2">UOM</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="2">Qty</th><th value="Rate/Item (Rs.)" style = "font-size: 12px;" colspan="1" rowspan="2">Rate/Item (Rs.)</th><th value="Discount (Rs.)" style = "font-size: 12px;" colspan="1" rowspan="2">Discount (Rs.)</th><th value="Taxable Value" style = "font-size: 12px;" colspan="1" rowspan="2">Taxable Value</th><th value="CGST" style = "font-size: 12px;" colspan="2" rowspan="1">SGST</th><th value="SGST" colspan="2" style = "font-size: 12px;" rowspan="1">CGST</th><th value="IGST" style = "font-size: 12px;" colspan="2" rowspan="1">IGST</th><th value="Taxable Value" style = "font-size: 12px;" colspan="1" rowspan="2">Total Amount</th></tr><tr style="background:antiquewhite;"><th value="%" style = "font-size: 12px;" colspan="1" rowspan="1">%</th><th style = "font-size: 12px;" value="Amt (Rs.)" colspan="1" rowspan="1">Amt (Rs.)</th><th value="%" style = "font-size: 12px;" colspan="1" rowspan="1">%</th><th style = "font-size: 12px;" value="Amt (Rs.)" colspan="1" rowspan="1">Amt (Rs.)</th><th value="%" style = "font-size: 12px;" colspan="1" rowspan="1">%</th><th value="Amt (Rs.)" colspan="1" rowspan="1" style = "font-size: 12px;">Amt (Rs.)</th></tr></thead>';
            var tot_taxablevalue = 0;
            var tot_sgstamount = 0;
            var tot_cgstamount = 0;
            var tot_igstamount = 0;
            var tot_totalamount = 0;
            var tot_qty = 0;
            for (var i = 0; i < msg.length; i++) {
                var itemcode = msg[i].itemcode;
                if (itemcode == "Inventory") {
                }
                else {
                    results += '<tr style="font-size: 12px;">'
                    results += '<td scope="row" class="1"  style="text-align:center;">' + msg[i].sno + '</td>';
                    results += '<td scope="row" class="1"  style="text-align:center;">' + msg[i].itemcode + '</td>';
                    results += '<td data-title="brandstatus" class="2">' + msg[i].ProductName + '</td>';
                    results += '<td data-title="brandstatus" class="2">' + msg[i].hsncode + '</td>';
                    results += '<td data-title="brandstatus" class="2">' + msg[i].uom + '</td>';
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
            }
            var Total = "Total";
            results += '<tr>';
            results += '<td style = "font-size: 12px;text-align:center;background:antiquewhite;" colspan="5"><label>' + Total + '</label></td>';
            results += '<td style = "font-size: 12px;text-align:center;"><label>' + parseFloat(tot_qty).toFixed(2) + '</label></td>';
            results += '<td style = "font-size: 12px;text-align:center;background:antiquewhite;" colspan="2"><label></label></td>';
            results += '<td style = "font-size: 12px;text-align:center;"><label>' + parseFloat(tot_taxablevalue).toFixed(2) + '</label></td>';
            results += '<td colspan="2" style="text-align:center;font-size: 12px;"><label>' + parseFloat(tot_sgstamount).toFixed(2) + '</label></td>';
            results += '<td colspan="2" style="text-align:center;font-size: 12px;"><label>' + parseFloat(tot_cgstamount).toFixed(2) + '</label></td>';
            results += '<td colspan="2" style="text-align:center;font-size: 12px;"><label>' + parseFloat(tot_igstamount).toFixed(2) + '</label></td>';
            results += '<td style="font-size: 12px;"><label>' + Math.round(tot_totalamount) + '</label></td>';
            var invname = "Inventory Details";
            results += '<tr >'
            results += '<td data-title="brandstatus" class="2"></td>';
            results += '<td scope="row" class="1" colspan="14" style="text-align:left;font-size: 14px;"><label>' + invname + '</label></td></tr>';
            for (var i = 0; i < msg.length; i++) {
                var itemcode = msg[i].itemcode;
                if (itemcode == "Inventory") {
                    results += '<tr style="font-size: 12px;">'
                    results += '<td data-title="brandstatus" class="2"></td>';
                    results += '<td data-title="brandstatus" class="2"></td>';
                    results += '<td data-title="brandstatus" class="2">' + msg[i].ProductName + '</td>';
                    results += '<td data-title="brandstatus" class="2">' + msg[i].qty + '</td>';
                    results += '<td data-title="brandstatus" colspan="12" class="2"></td></tr>';
                }
            }
            results += '</tr></table></div>';
            results += '</table></div>';
            $("#div_itemdetails1").html(results);
            var roundoff = Math.round(tot_totalamount);
            document.getElementById('recevied').innerHTML = toWords(parseInt(roundoff)) + " only/-";
        }
        var th = ['', 'thousand', 'million', 'billion', 'trillion'];

        var dg = ['zero', 'one', 'two', 'three', 'four', 'five', 'six', 'seven', 'eight', 'nine'];

        var tn = ['ten', 'eleven', 'twelve', 'thirteen', 'fourteen', 'fifteen', 'sixteen', 'seventeen', 'eighteen', 'nineteen'];

        var tw = ['Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];
        function toWords(s) {
            s = s.toString();
            s = s.replace(/[\, ]/g, '');
            if (s != parseFloat(s)) return 'not a number';
            var x = s.indexOf('.');
            if (x == -1) x = s.length;
            if (x > 15) return 'too big';
            var n = s.split('');
            var str = '';
            var sk = 0;
            for (var i = 0; i < x; i++) {
                if ((x - i) % 3 == 2) {
                    if (n[i] == '1') {
                        str += tn[Number(n[i + 1])] + ' ';
                        i++;
                        sk = 1;
                    } else if (n[i] != 0) {
                        str += tw[n[i] - 2] + ' ';
                        sk = 1;
                    }
                } else if (n[i] != 0) {
                    str += dg[n[i]] + ' ';
                    if ((x - i) % 3 == 0) str += 'hundred ';
                    sk = 1;
                }
                if ((x - i) % 3 == 1) {
                    if (sk) str += th[(x - i - 1) / 3] + ' ';
                    sk = 0;
                }
            }
            if (x != s.length) {
                var y = s.length;
                str += 'point ';
                for (var i = x + 1; i < y; i++) str += dg[n[i]] + ' ';
            }
            return str.replace(/\s+/g, ' ');

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
    <div id="config" style="display: none;">
        <input type="radio" name="btype" id="code128" checked="checked" value="code128" />
    </div>
    <section class="content-header">
        <h1>EInvoice<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">EInvoice</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>EInvoice Details
                </h3>
            </div>
            <div class="box-body">
                <div style="width: 100%; background-color: #fff">
                    <div>
                        <table>
                            <tr>
                                <td>Type
                            </td>
                            <td>
                                <select id="ddltype" class="form-control">
                                    <option value="invoice">Invoice</option>
                                    <option value="dc">DC</option>
                                </select>
                            </td>
                                <td class="divsalesOffice" style="display: none;">
                                    <select id="ddlsalesOffice" class="form-control">
                                    </select>
                                </td>
                                <td>Fromdate
                                </td>
                                <td style="width: 5px;"></td>
                                <td>
                                    <input type="date" id="txtFromDate" class="form-control" />
                                </td>
                                <td style="width: 5px;"></td>
                                <td>
                                    <input type="button" id="btnGenerate" value="Generate" onclick="GenerateClick();"
                                        class="btn btn-primary" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divEWayBilldata">
                    </div>
                    <div id="div_itemdetails">
                        <span id="spnJsonData"></span>
                    </div>
                </div>
                <div class="modal fade in" id="divMainAddNewRow1" style="display: none; padding-right: 2px;">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true" onclick="CloseClick1();">×</span></button>
                                <%-- <h4 class="modal-title">Branch Wise Details</h4>--%>
                            </div>
                            <div class="modal-body" id="divChart" style="height: 400px; overflow-y: scroll;">
                                <div id="divPrint" style="display: none; height: 50%;">
                                    <div class="content">
                                        <div style="border: 2px solid gray;">
                                            <div style="width: 22%; float: right; padding-top: 5px;">
                                                <%--<img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="100px" height="72px" />
                                                <br />--%>
                                                <div id="image"></div>
                                                <br />
                                            </div>
                                            <div style="border: 1px solid gray;">
                                                <br />
                                                <div style="font-family: Arial; font-size: 20px; font-weight: bold; color: Black; text-align: center;">
                                                    <span id="lbltile"></span>
                                                    <br />
                                                </div>
                                                <div style="width: 73%; padding-left: 12%; text-align: center;">
                                                    <span id="spnAddress" style="font-size: 14px;"></span>
                                                    <br />
                                                    <label style="font-size: 12px; font-weight: bold !important;">
                                                        GSTIN :</label>
                                                    <span id="spngstnum" style="font-size: 11px; font-weight: bold !important;"></span>
                                                    <br />
                                                    <%--<span id="Span1" style="font-size: 11px; font-weight: bold;">Website: www.vyshnavi.in</span>--%>
                                                    <br />
                                                    <br />
                                                </div>
                                                <div style="width: 73%; padding-left: 12%; text-align: center; display: none;">
                                                    <span id="spngstnno" style="font-size: 14px;"></span>
                                                    <br />
                                                </div>
                                                <br />
                                            </div>
                                            <div align="center" style="border-bottom: 1px solid gray; border-top: 1px solid gray; background: antiquewhite;">
                                                <%--<span style="font-size: 18px; font-weight: bold;" id="spninvoicetype"></span>--%>
                                                <span style="font-size: 18px; font-weight: bold;" id="spninvoicetype">TaxInvoice</span>
                                            </div>
                                            <div style="width: 100%;">
                                                <%-- <table style="width: 100%;border: 3px solid #dddddd;"  class="table table-bordered table-hover dataTable no-footer">--%>
                                                <div style="float: left; border-bottom: 1px solid gray; border-top: 1px solid gray; background: antiquewhite;">
                                                    <span style="font-size: 18px; font-weight: bold;" id="spnInvoiceHeader">1.e-Invoice Details</span>
                                                </div>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 60%; border: 2px solid gray; padding-left: 2%;">
                                                            <label style="font-size: 12px; font-weight: bold !important;">
                                                                IRN :</label>
                                                            <span id="spIrn_No" style="font-size: 11px;"></span>
                                                        </td>
                                                        <td style="width: 20%; border: 2px solid gray; padding-left: 2%;">
                                                            <label style="font-size: 12px; font-weight: bold !important;">
                                                                Ack No :</label>
                                                            <span id="spnAck_No" style="font-size: 11px;"></span>
                                                        </td>
                                                        <td style="width: 20%; border: 2px solid gray; padding-left: 2%;">
                                                            <label style="font-size: 12px; font-weight: bold !important;">
                                                                Ack. Date :</label>
                                                            <span id="spnAck_Date" style="font-size: 11px;"></span>
                                                        </td>
                                                    </tr>
                                                </table>

                                                <%-- <div style="width: 100%;">--%>
                                                <%-- <table style="width: 100%;border: 3px solid #dddddd;"  class="table table-bordered table-hover dataTable no-footer">--%>
                                                <div style="float: left; border-bottom: 1px solid gray; border-top: 1px solid gray; background: antiquewhite;">
                                                    <span style="font-size: 18px; font-weight: bold;" id="spnTransHead">2.Transaction Details</span>
                                                </div>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 50%; border: 2px solid gray; padding-left: 2%;">
                                                            <label style="font-size: 12px;">
                                                                Supply Type Code :</label>
                                                            <span id="spnSupplyType" style="font-size: 14px;">B2B</span>
                                                        </td>
                                                        <td style="width: 50%; border: 2px solid gray; padding-left: 2%;">
                                                            <label style="font-size: 12px;">
                                                                Invoice No :</label>
                                                            <span id="spninvoiceno" style="font-size: 14px;"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 50%; border: 2px solid gray; padding-left: 2%;">
                                                            <label style="font-size: 12px;">
                                                                Place of Supply :</label>
                                                            <span id="spnplaceofsupply" style="font-size: 14px;"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 50%; border: 2px solid gray; padding-left: 2%;">
                                                            <label style="font-size: 12px;">
                                                                Document Type :</label>
                                                            <span id="spnDocType" style="font-size: 14px;">Tax Invoice</span>
                                                        </td>
                                                        <td style="width: 50%; border: 2px solid gray; padding-left: 2%;">
                                                            <label style="font-size: 12px;">
                                                                Document Date :</label>
                                                            <span id="spninvoicedate" style="font-size: 14px;"></span>
                                                        </td>
                                                    </tr>
                                                </table>

                                                <div style="float: left; border-bottom: 1px solid gray; border-top: 1px solid gray; background: antiquewhite;">
                                                    <span style="font-size: 18px; font-weight: bold;" id="spnPartyDetils">3.PartyDetails</span>
                                                </div>

                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 50%; border: 2px solid gray; padding-left: 2%;">
                                                            <label style="font-size: 14px; font-weight: bold;">
                                                                Bill From
                                                            </label>
                                                            <br />
                                                            <label style="font-size: 12px; font-weight: bold !important;">
                                                                Name :</label>
                                                            <span id="spnfromname" style="font-size: 11px;"></span>
                                                            <br />
                                                            <label style="font-size: 12px; font-weight: bold !important;">
                                                                Address :</label>
                                                            <span id="spnfromaddress" style="font-size: 11px;"></span>
                                                            <br />
                                                            <label style="font-size: 12px; font-weight: bold !important;">
                                                                GSTIN :</label>
                                                            <span id="spnfromgstn" style="font-size: 11px; font-weight: bold !important;"></span>
                                                            <br />
                                                            <label style="font-size: 12px; font-weight: bold !important;">
                                                                Telephone no :</label>
                                                            <span id="lbl_companymobno" style="font-size: 11px;"></span>&nbsp; &nbsp; &nbsp;
                                                                    &nbsp; &nbsp; &nbsp;
                                                                <label style="font-size: 12px; font-weight: bold !important;">
                                                                    Email Id :</label>
                                                            <span id="lbl_companyemail" style="font-size: 11px;"></span>
                                                            <br />
                                                            <label style="font-size: 12px; font-weight: bold !important;">
                                                                State Name :</label>
                                                            <span id="spnfromstate" style="font-size: 11px;"></span>&nbsp; &nbsp; &nbsp; &nbsp;
                                                                     &nbsp; &nbsp;
                                                                <label style="font-size: 12px; font-weight: bold !important;">
                                                                    State Code :</label>
                                                            <span id="spnfromstatecode" style="font-size: 11px;"></span>
                                                            <br />
                                                        </td>
                                                        <td style="width: 50%; border: 2px solid gray; padding-left: 2%;">
                                                            <label style="font-size: 14px; font-weight: bold;">
                                                                Bill To
                                                            </label>
                                                            <br />
                                                            <label style="font-size: 12px; font-weight: bold !important;">
                                                                Name :</label>
                                                            <span id="lblpartyname" style="font-size: 11px;"></span>
                                                            <br />
                                                            <label style="font-size: 12px; font-weight: bold !important;">
                                                                Address :</label>
                                                            <span id="spn_toaddress" style="font-size: 11px;"></span>
                                                            <br />
                                                            <label style="font-size: 12px; font-weight: bold !important;">
                                                                GSTIN :</label>
                                                            <span id="span_toGSTIN" style="font-size: 11px;"></span>
                                                            <br />
                                                            <label style="font-size: 12px; font-weight: bold !important;">
                                                                State Name :</label>
                                                            <span id="lbl_tostate" style="font-size: 11px;"></span>&nbsp; &nbsp; &nbsp; &nbsp;
                                                                       &nbsp; &nbsp;
                                                                 <label style="font-size: 12px; font-weight: bold !important;">
                                                                     State Code :</label>
                                                            <span id="lbl_tostatecode" style="font-size: 11px;"></span>
                                                            <br />
                                                            <label style="font-size: 12px; font-weight: bold !important;">
                                                                Telephone no :</label>
                                                            <span id="lblvendorphoneno" style="font-size: 11px;"></span>&nbsp; &nbsp; &nbsp;
                                                                    &nbsp; &nbsp; &nbsp;
                                                                  <label style="font-size: 12px; font-weight: bold !important;">
                                                                      Email Id :</label>
                                                            <span id="lblvendoremail" style="font-size: 11px;"></span>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <%-- <table style="width: 100%;">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 50%; border: 2px solid gray; padding-left: 2%;">
                                                                <label style="font-size: 12px;">
                                                                    Invoice No :</label>
                                                                <span id="spninvoiceno" style="font-size: 14px;"></span>
                                                                <span id="spntempinvoiceno" style="display: none;"></span>
                                                                <br />
                                                                <label style="font-size: 12px;">
                                                                    Invoice Date :</label>
                                                                <span id="spninvoicedate" style="font-size: 14px;"></span>
                                                                <br />
                                                                <label style="font-size: 12px;">
                                                                    Buyer's OrderNo :</label>
                                                                <span id="spnorderno" style="font-size: 14px;"></span>
                                                                <br />
                                                                <label style="font-size: 12px;">
                                                                    Mode/Terms of Payments :</label>
                                                                <span id="lblmodeofpayments" style="font-size: 14px;"></span>
                                                                <br />
                                                            </td>

                                                            <td style="width: 50%; border: 2px solid gray; padding-left: 2%;">
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
                                                    </tbody>
                                                </table>--%>
                                            </div>
                                            <div style="font-family: Arial; font-weight: bold; color: Black; text-align: center; border: 2px solid gray;">
                                                <br />
                                            </div>
                                            <div id="div_itemdetails1">
                                            </div>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 35%;">
                                                        <label style="font-size: 16px; font-weight: bold;">
                                                            Towards:
                                                        </label>
                                                        <label>
                                                            Rs.</label>
                                                        <span id="recevied" onclick="test.rnum.value = toWords(test.inum.value);" value="To Words"></span>
                                                    </td>
                                                    <td style="width: 35%;">
                                                        <div align="center">
                                                            <div id="barcodeTarget" class="barcodeTarget">
                                                            </div>
                                                            <canvas id="canvasTarget" width="80" height="150"></canvas>
                                                        </div>
                                                    </td>
                                                    <td align="center" style="width: 30%;">For <span id="lblsignname" style="font-weight: bold; font-size: 12px;"></span>
                                                        <br />
                                                        <br />
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--add BarCode--%>

                                            <br />
                                            <br />
                                            <table style="width: 100%;">
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
                                                <span style="font-size: 11px;">We declare that this Invoice shows the actual price of
                                    the goods described and that all particulars are true and correct</span>
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <input id="Button3" type="button" class="btn btn-primary" name="submit" value='Print'
                                    onclick="javascript:CallPrint('divPrint');" />
                                <button type="button" class="btn btn-default" id="close" onclick="closepopup();">Close</button>
                            </div>
                            <!-- /.modal-content -->
                        </div>
                        <!-- /.modal-dialog -->
                    </div>
                </div>
            </div>
    </section>
</asp:Content>


