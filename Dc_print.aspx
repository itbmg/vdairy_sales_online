<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Dc_print.aspx.cs" Inherits="Dc_print" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js?v=3004" type="text/javascript"></script>
    <style type="text/css">
        .container {
            max-width: 100%;
        }

        th {
            text-align: center;
        }

        #content {
            position: absolute;
            z-index: 1;
        }

        
    </style>
    <script type="text/javascript">
        $(function () {
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#txtfromdate').val(today);
            $('#txttodate').val(today);
        });
        function CallPrint(strid) {
            document.getElementById("tbl_po_print").style.borderCollapse = "collapse";
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        function clearall() {
            document.getElementById('spncompanyname').innerHTML = "";
            document.getElementById('spnAddress').innerHTML = "";
            document.getElementById('spnfromaddress').innerHTML = "";
            document.getElementById('lblgstin').innerHTML = "";
            document.getElementById('spnfromgstn').innerHTML = "";
            document.getElementById('spngstnum').innerHTML = "";
            document.getElementById('lbl_tostate').innerHTML = "";
            document.getElementById('lbl_tostatecode').innerHTML = "";
            document.getElementById('lbl_fromstate').innerHTML = "";
            //document.getElementById('spnfstate').innerHTML = msg[0].fromstatename;
            document.getElementById('spnfromstate').innerHTML = "";
            document.getElementById('lbl_fromstate_code').innerHTML = "";
            document.getElementById('spnfromstatecode').innerHTML = "";
            document.getElementById('lblRefdcno').innerHTML = "";
            document.getElementById('lbldcno').innerHTML = "";
            document.getElementById('lblassigndate').innerHTML = "";
            document.getElementById('spndateofsupply').innerHTML = "";
            document.getElementById('lbldisptime').innerHTML = "";
            document.getElementById('spnplaceofsupply').innerHTML = "";
            document.getElementById('hdnDespsno').value = "";
            document.getElementById('lblroutename').innerHTML = "";
            document.getElementById('lblvehicleno').innerHTML = "";
            document.getElementById('lbldispat').innerHTML = "";
            document.getElementById('lbldcType').innerHTML = "";
            document.getElementById('spnfromname').innerHTML = "";
            document.getElementById('lbl_companymobno').innerHTML = "";
            document.getElementById('lbl_companyemail').innerHTML = "";
            document.getElementById('lblsignname').innerHTML = "";
            document.getElementById('lblpartyname').innerHTML = "";
            document.getElementById('span_toGSTIN').innerHTML = "";
            document.getElementById('lblvendorphoneno').innerHTML = "";
            document.getElementById('spn_toaddress').innerHTML = "";
            document.getElementById('lblvendoremail').innerHTML = "";
        }
        function btnDCDetails_click() {
            clearall();
            var fromdate = document.getElementById('txtfromdate').value;
            var todate = document.getElementById('txttodate').value

            if (fromdate == "") {
                alert("Please select from date");
                return false;
            }
            if (todate == "") {
                alert("Please select to date");
                return false;
            }
            var data = { 'operation': 'get_DC_details_click', 'fromdate': fromdate, 'todate': todate };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldetails(msg);
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
        function filldetails(msg) {
            // var status = "A";
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer">';
            results += '<thead><tr><th scope="col"></th><th scope="col">Ref No</th><th scope="col">Assign Date</th><th scope="col">Permissions</th><th scope="col">VehicleNo</th><th scope="col">DispatchName</th><th scope="col">Employee Name</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                //if (status == msg[i].status) {
                results += '<tr><th><button id="btn_Print" type="button"   onclick="printclick(this);"  name="Edit" class="btn btn-primary" ><i class="fa fa-print"></i> Print</button></th>'
                results += '<td scope="row" class="1"  style="text-align:center;">' + msg[i].TripId + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].AssignDate + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].Permissions + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].VehicleNo + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].DispatchName + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].Employee + '</td></tr>';
                // results += '<td data-title="brandstatus" class="2">' + msg[i].expiredate + '</td></tr>';
                //}
            }
            results += '</table></div>';
            $("#divPOdata").html(results);
        }
        function printclick(thisid) {
            var refdcno = $(thisid).parent().parent().children('.1').html();
            document.getElementById('txt_Refno').value = refdcno;
            var DcType = document.getElementById('ddltype').value;
            var data = { 'operation': 'get_DeliveryChallan_click', 'refdcno': refdcno, 'DcType': DcType };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        FillDCMain_details(msg);
                        GetDC_Products();
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
                document.getElementById('spncompanyname').innerHTML = msg[0].Title;
                document.getElementById('spnAddress').innerHTML = msg[0].Address;
                document.getElementById('spnfromaddress').innerHTML = msg[0].Address;
                document.getElementById('lblgstin').innerHTML = msg[0].fromgstin;
                document.getElementById('spnfromgstn').innerHTML = msg[0].fromgstin;
                document.getElementById('spngstnum').innerHTML = msg[0].fromgstin;
                document.getElementById('lbl_tostate').innerHTML = msg[0].tostatename;
                document.getElementById('lbl_tostatecode').innerHTML = msg[0].tostatecode;
                document.getElementById('lbl_fromstate').innerHTML = msg[0].fromstatename;
                //document.getElementById('spnfstate').innerHTML = msg[0].fromstatename;
                document.getElementById('spnfromstate').innerHTML = msg[0].fromstatename;
                document.getElementById('lbl_fromstate_code').innerHTML = msg[0].fromstatecode;
                document.getElementById('spnfromstatecode').innerHTML = msg[0].fromstatecode;
                document.getElementById('lblRefdcno').innerHTML = document.getElementById('txt_Refno').value;
                document.getElementById('lbldcno').innerHTML = msg[0].DcNo;
                document.getElementById('lblassigndate').innerHTML = msg[0].assigndate;
                document.getElementById('spndateofsupply').innerHTML = msg[0].assigndate;
                document.getElementById('lbldisptime').innerHTML = msg[0].PlanTime;
                document.getElementById('spnplaceofsupply').innerHTML = msg[0].city;
                document.getElementById('hdnDespsno').value = msg[0].Dispatchsno;
                document.getElementById('lblroutename').innerHTML = msg[0].routename;
                document.getElementById('lblvehicleno').innerHTML = msg[0].vehicleno;
                document.getElementById('lbldispat').innerHTML = msg[0].Dispatcher;
                document.getElementById('lbldcType').innerHTML = msg[0].dctype;
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
                        document.getElementById('lblpartyname').innerHTML = msg[0].Title + "-" + msg[0].partyname;
                        document.getElementById('span_toGSTIN').innerHTML = msg[0].togstin;
                        document.getElementById('lblvendorphoneno').innerHTML = msg[0].phoneno;
                        document.getElementById('spn_toaddress').innerHTML = msg[0].AgentAddress;
                        document.getElementById('lblvendoremail').innerHTML = msg[0].email;
                    }
                }
            }
        }
        function GetDC_Products() {
            var refno = document.getElementById('txt_Refno').value;
            var DcType = document.getElementById('ddltype').value;
            var data = { 'operation': 'get_DeliveryChallan_click', 'refdcno': refno, 'DcType': DcType };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        FillDCMain_details(msg);
                        GetHeader();
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
        function GetHeader() {
            var refno = document.getElementById('txt_Refno').value;
            var Dispatchsno = document.getElementById('hdnDespsno').value;
            var DcType = document.getElementById('ddltype').value;
            var data = { 'operation': 'GetDC_Products', 'Dispatchsno': Dispatchsno, 'refdcno': refno, 'DcType': DcType };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Data not found") {
                        alert(msg);
                        return false;
                    }
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
            var lbldcType = document.getElementById('lbldcType').innerHTML;
            var results = '<div  style="overflow:auto;"><table border="1" id="tbl_po_print" style="width: 100%;" class="table table-bordered table-hover dataTable no-footer">';
            results += '<thead><tr style="background:antiquewhite;"><th value="#" colspan="1" style = "font-size: 12px;" rowspan="2">Sno</th><th value="Item Code" style = "font-size: 12px;" colspan="1" rowspan="2">Item Code</th><th style = "font-size: 12px;" value="Item Name" colspan="1" rowspan="2">Item Description</th><th style = "font-size: 12px;" value="HSN CODE" colspan="1" rowspan="2">HSN CODE</th><th value="UOM" style = "font-size: 12px;" colspan="1" rowspan="2">UOM</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="2">Qty(ltrs/kgs)</th><th value="Qty" style = "font-size: 12px;" colspan="1" rowspan="2">Qty(packets)</th><th value="Rate/Item (Rs.)" style = "font-size: 12px;" colspan="1" rowspan="2">Rate/Item (Rs.)</th><th value="Discount (Rs.)" style = "font-size: 12px;" colspan="1" rowspan="2">Discount (Rs.)</th><th value="Taxable Value" style = "font-size: 12px;" colspan="1" rowspan="2">Taxable Value</th><th value="CGST" style = "font-size: 12px;" colspan="2" rowspan="1">SGST</th><th value="SGST" colspan="2" style = "font-size: 12px;" rowspan="1">CGST</th><th value="IGST" style = "font-size: 12px;" colspan="2" rowspan="1">IGST</th><th value="Taxable Value" style = "font-size: 12px;" colspan="1" rowspan="2">Total Amount</th></tr><tr style="background:antiquewhite;"><th value="%" style = "font-size: 12px;" colspan="1" rowspan="1">%</th><th style = "font-size: 12px;" value="Amt (Rs.)" colspan="1" rowspan="1">Amt (Rs.)</th><th value="%" style = "font-size: 12px;" colspan="1" rowspan="1">%</th><th style = "font-size: 12px;" value="Amt (Rs.)" colspan="1" rowspan="1">Amt (Rs.)</th><th value="%" style = "font-size: 12px;" colspan="1" rowspan="1">%</th><th value="Amt (Rs.)" colspan="1" rowspan="1" style = "font-size: 12px;">Amt (Rs.)</th></tr></thead>';
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
                    results += '<td data-title="brandstatus" class="2">' + parseFloat(msg[i].pkt_qty).toFixed(2) + '</td>';
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
            results += '<td style = "font-size: 12px;text-align:center;background:antiquewhite;" colspan="3"><label></label></td>';
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
            $("#div_products").html(results);
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
    <section class="content-header">
        <h1>Delivery Challan<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Reports</a></li>
            <li><a href="#">Delivery Challan Report</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Delivery Challan Report
                </h3>
            </div>
            <div class="box-body">
                <div runat="server" id="d">
                    <table>
                        <tr>
                            <td>
                                <label>
                                    From Date:</label>
                            </td>
                            <td>
                                <input type="date" id="txtfromdate" class="form-control" />
                            </td>
                            <td>
                                <label>
                                    To Date:</label>
                            </td>
                            <td>
                                <input type="date" id="txttodate" class="form-control" />
                            </td>
                            <td style="width: 5px;"></td>
                            <td>
                                <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="btnDCDetails_click()"><i class="fa fa-refresh"></i>Get DC Details </button>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div id="divPOdata" style="height: 300px; overflow-y: scroll;">
                    </div>
                    <br />
                    <table id="tbltrip">
                        <tr>
                            <td>Ref No
                            </td>
                            <td>
                                <input type="text" id="txt_Refno" class="form-control" placeholder="Enter Ref No" />
                            </td>
                            <td style="width: 5px;"></td>
                            <td>DcType
                            </td>
                            <td>
                                <select id="ddltype" class="form-control">
                                    <option value="NonTax">NonTax</option>
                                    <option value="Tax">Tax</option>
                                </select>
                            </td>
                            <td style="width: 5px;"></td>
                            <td style="width: 5px;"></td>
                            <td>
                                <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="GetDC_Products();"><i class="fa fa-refresh"></i>Generate </button>
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                </div>

                <div id="divPrint" class="watermark">
                    <div class="content">
                        <div style="border: 2px solid gray;" class="col-md-12">
                            <div style="width: 17%; float: right; padding-top: 12px;">
                                <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="100px" height="72px" />
                                <br />
                            </div>
                            <div style="border: 1px solid gray;">
                                <div style="font-family: Arial; font-size: 20pt; font-weight: bold; color: Black; text-align: center;">
                                    <span id="spncompanyname" style="font-size: 20px;"></span>
                                    <br />
                                </div>
                                <div style="width: 73%; padding-left: 12%; text-align: center;">
                                    <span id="spnAddress" style="font-size: 11px; font-weight: bold;"></span>
                                    <br />
                                    <label style="font-size: 12px; font-weight: bold !important;">
                                        GSTIN :</label>
                                    <span id="spngstnum" style="font-size: 11px; font-weight: bold !important;"></span>
                                    <br />
                                    <%--   <span id="Span1" style="font-size: 11px; font-weight: bold;">Website: www.vyshnavi.in</span>--%>
                                    <br />
                                </div>

                                <div style="width: 73%; padding-left: 31%; display: none;">
                                    <label style="font-size: 12px; font-weight: bold !important;">GSTIN.</label>
                                    <span id="lblgstin" style="font-size: 12px;"></span>
                                </div>
                                <div style="width: 73%; padding-left: 27%;">
                                    <br />
                                    <div style="display: none;">
                                        <label style="font-size: 12px;">State Name.</label>
                                        <span id="lbl_fromstate" style="font-size: 12px;"></span>
                                        <label style="font-size: 12px;">State Code.</label>
                                        <span id="lbl_fromstate_code" style="font-size: 12px;"></span>
                                    </div>
                                </div>
                            </div>
                            <div align="center" style="border-bottom: 2px solid gray; border-top: 1px solid gray; background: antiquewhite;">
                                <span style="font-size: 18px; font-weight: bold;" id="lbldcType"></span>
                            </div>
                            <div style="width: 100%;">
                                <div style="width: 100%;">

                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 60%; border: 2px solid gray; padding-left: 2%;">
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
                                                <span id="lbl_companymobno" style="font-size: 11px;"></span>
                                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <label style="font-size: 12px; font-weight: bold !important;">
                                        Email Id :</label>
                                                <span id="lbl_companyemail" style="font-size: 11px;"></span>
                                                <br />
                                                <label style="font-size: 12px; font-weight: bold !important;">
                                                    State Name :</label>
                                                <span id="spnfromstate" style="font-size: 11px;"></span>
                                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <label style="font-size: 12px; font-weight: bold !important;">
                                        State Code :</label>
                                                <span id="spnfromstatecode" style="font-size: 11px;"></span>
                                                <br />
                                            </td>
                                            <td style="width: 39%; border: 2px solid gray; padding-left: 2%;">
                                                <label style="font-size: 12px; font-weight: bold !important;">
                                                    Invoice No. :</label>
                                                <span id="lbldcno" style="font-size: 11px;"></span>
                                                <br />
                                                <label style="font-size: 12px; font-weight: bold !important;">
                                                    Ref NO :</label>
                                                <span id="lblRefdcno" style="font-size: 11px;"></span>
                                                <br />
                                                <label style="font-size: 12px; font-weight: bold !important;">
                                                    Date :</label>
                                                <span id="lblassigndate" style="font-size: 11px;"></span>
                                                <br />
                                                <label style="font-size: 12px; font-weight: bold !important;">
                                                    Time :</label>
                                                <span id="lbldisptime" style="font-size: 11px;"></span>
                                                <br />

                                            </td>

                                        </tr>
                                    </table>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 60%; border: 2px solid gray; padding-left: 2%;">
                                                <label style="font-size: 14px; font-weight: bold;">
                                                    Bill To
                                                </label>
                                                <br />
                                                <label style="font-size: 12px; font-weight: bold !important;">
                                                    Name :</label>
                                                <span id="lblpartyname" style="font-size: 11px;"></span>
                                                <input type="hidden" id="hdnDespsno" />
                                                <br />
                                                <label style="font-size: 12px; font-weight: bold !important;">
                                                    Despatch Name :</label>
                                                <span id="lblroutename" style="font-size: 11px;"></span>
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
                                                    Telephone no :</label>
                                                <span id="lblvendorphoneno" style="font-size: 11px;"></span>
                                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <label style="font-size: 12px; font-weight: bold !important;">
                                        Email Id :</label>
                                                <span id="lblvendoremail" style="font-size: 11px;"></span>
                                                <br />
                                                <label style="font-size: 12px; font-weight: bold !important;">
                                                    State Name :</label>
                                                <span id="lbl_tostate" style="font-size: 11px;"></span>
                                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                     <label style="font-size: 12px; font-weight: bold !important;">
                                         State Code :</label>
                                                <span id="lbl_tostatecode" style="font-size: 11px;"></span>
                                                <br />
                                            </td>
                                            <td style="width: 39%; border: 2px solid gray; padding-left: 2%; padding-bottom: 5%;">
                                                <label style="font-size: 12px; font-weight: bold !important;">
                                                    Transport Mode :</label>
                                                <span id="spntransportmode" style="font-size: 11px;">By Road</span>
                                                <br />
                                                <label style="font-size: 12px; font-weight: bold !important;">
                                                    Vehicle No. :</label>
                                                <span id="lblvehicleno" style="font-size: 11px;"></span>
                                                <br />
                                                <label style="font-size: 12px; font-weight: bold !important;">
                                                    Date of Supply :</label>
                                                <span id="spndateofsupply" style="font-size: 11px;"></span>
                                                <br />
                                                <label style="font-size: 12px; font-weight: bold !important;">
                                                    Place of Supply :</label>
                                                <span id="spnplaceofsupply" style="font-size: 11px;"></span>
                                                <br />
                                            </td>

                                        </tr>
                                    </table>
                                </div>
                                <div style="font-family: Arial; font-weight: bold; color: Black; text-align: center; border: 2px solid gray;">
                                    <br />
                                </div>
                                <div id="div_products">
                                </div>
                                <table>
                                    <label style="font-size: 16px; font-weight: bold;">
                                        Towards:
                                    </label>
                                    <label>Rs.</label>
                                    <span id="recevied" onclick="test.rnum.value = toWords(test.inum.value);" value="To Words"></span>

                                </table>
                                <br />
                                <br />
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="3"></td>
                                        <td style="width: 50%;">For <span id="lblsignname" style="font-weight: bold; font-size: 11px;"></span>
                                            <br />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">
                                            <span style="font-weight: bold; font-size: 12px;">RECEIVER'S SIGNATURE</span>
                                        </td>
                                        <td style="width: 15%;">
                                            <span style="font-weight: bold; font-size: 12px;">SECURITY</span>
                                        </td>
                                        <td style="width: 25%;">
                                            <span id="lbldispat" style="font-weight: bold; font-size: 12px;"></span>
                                        </td>
                                        <td style="width: 50%;">

                                            <span style="font-weight: bold; font-size: 12px;">AUTHORISED SIGNATURE</span>
                                        </td>
                                    </tr>
                                </table>
                                <br />

                                <br />
                                <div>
                                    <span style="font-weight: bold; font-size: 13px;">Decleration:</span>
                                    <br />
                                    <span style="font-size: 11px;">We declare that this invoice shows the actual price of the goods decribe and that all particulars are ture and correct</span>
                                    <br />
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="javascript:CallPrint('divPrint');"><i class="fa fa-print"></i>Print </button>
            </div>
        </div>
    </section>
</asp:Content>
