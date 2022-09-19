<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Invoice.aspx.cs" Inherits="Invoice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script src="js/jquery-1.4.4.js?v=3004" type="text/javascript"></script>
    <style type="text/css">
        .container
        {
            max-width: 100%;
        }
        th
        {
            text-align: center;
        }
        #content
        {
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
            document.getElementById("tbl_invoice").style.borderCollapse = "collapse";
            document.getElementById("tbl_invoice2").style.borderCollapse = "collapse";
            
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        function clearall() {
            document.getElementById('spncompanyname').innerHTML = "";
            document.getElementById('spnAddress').innerHTML = "";
            //document.getElementById('spnfromaddress').innerHTML = "";
            document.getElementById('lblgstin').innerHTML = "";
            //document.getElementById('spnfromgstn').innerHTML = "";
            document.getElementById('lbl_tostate').innerHTML = "";
            //document.getElementById('lbl_tostatecode').innerHTML = "";
            //document.getElementById('lbl_fromstate').innerHTML = "";
            //document.getElementById('spnfstate').innerHTML = msg[0].fromstatename;
            //document.getElementById('spnfromstate').innerHTML = "";
            //document.getElementById('lbl_fromstate_code').innerHTML = "";
            //document.getElementById('spnfromstatecode').innerHTML = "";
            //document.getElementById('lblRefdcno').innerHTML = "";
            //document.getElementById('lbldcno').innerHTML = "";
            document.getElementById('lblassigndate').innerHTML = "";
            //document.getElementById('spndateofsupply').innerHTML = "";
           // document.getElementById('lbldisptime').innerHTML = "";
           // document.getElementById('spnplaceofsupply').innerHTML = "";
            document.getElementById('hdnDespsno').value = "";
            //document.getElementById('lblroutename').innerHTML = "";
            //document.getElementById('lblvehicleno').innerHTML = "";
            document.getElementById('lbldispat').innerHTML = "";
            document.getElementById('lbldcType').innerHTML = "";
            //document.getElementById('spnfromname').innerHTML = "";
            //document.getElementById('lbl_companymobno').innerHTML = "";
            //document.getElementById('lbl_companyemail').innerHTML = "";
           document.getElementById('lblsignname').innerHTML = "";
            document.getElementById('lblpartyname').innerHTML = "";
            document.getElementById('span_toGSTIN').innerHTML = "";
            //document.getElementById('lblvendorphoneno').innerHTML = "";
            document.getElementById('spn_toaddress').innerHTML = "";
           // document.getElementById('lblvendoremail').innerHTML = "";
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
            var data = { 'operation': 'get_DeliveryChallan_click', 'refdcno': refdcno };
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
                //document.getElementById('spnfromaddress').innerHTML = msg[0].Address;
                document.getElementById('lblgstin').innerHTML = msg[0].fromgstin;
                //document.getElementById('spnfromgstn').innerHTML = msg[0].fromgstin;
                document.getElementById('lbl_tostate').innerHTML = msg[0].tostatename;
                //document.getElementById('lbl_tostatecode').innerHTML = msg[0].tostatecode;
                //document.getElementById('lbl_fromstate').innerHTML = msg[0].fromstatename;
                //document.getElementById('spnfstate').innerHTML = msg[0].fromstatename;
                //document.getElementById('spnfromstate').innerHTML = msg[0].fromstatename;
               // document.getElementById('lbl_fromstate_code').innerHTML = msg[0].fromstatecode;
               // document.getElementById('spnfromstatecode').innerHTML = msg[0].fromstatecode;
               // document.getElementById('lblRefdcno').innerHTML = document.getElementById('txt_Refno').value;
                document.getElementById('lbldcno').innerHTML = msg[0].DcNo;
                document.getElementById('lblassigndate').innerHTML = msg[0].assigndate;
                //document.getElementById('spndateofsupply').innerHTML = msg[0].assigndate;
                //document.getElementById('lbldisptime').innerHTML = msg[0].PlanTime;
                //document.getElementById('spnplaceofsupply').innerHTML = msg[0].city;
                document.getElementById('hdnDespsno').value = msg[0].Dispatchsno;
               // document.getElementById('lblroutename').innerHTML = msg[0].routename;
                //document.getElementById('lblvehicleno').innerHTML = msg[0].vehicleno;
                document.getElementById('lbldispat').innerHTML = msg[0].Dispatcher;
                document.getElementById('lbldcType').innerHTML = msg[0].dctype;
               // document.getElementById('spnfromname').innerHTML = msg[0].Title;
               // document.getElementById('lbl_companymobno').innerHTML = msg[0].companyphone;
               // document.getElementById('lbl_companyemail').innerHTML = msg[0].companyemail;
                document.getElementById('lblsignname').innerHTML = msg[0].Title;
                if (msg[0].dispmode == "LOCAL" || msg[0].dispmode == "Staff" || msg[0].dispmode == "Free") {
                    document.getElementById('spn_toaddress').innerHTML = msg[0].partyname;
                }
                else {
                    if (msg[0].dispmode == "AGENT") {
                        document.getElementById('lblpartyname').innerHTML = msg[0].partyname;
                        document.getElementById('span_toGSTIN').innerHTML = msg[0].togstin;
                        //document.getElementById('lblvendorphoneno').innerHTML = msg[0].phoneno;
                        document.getElementById('spn_toaddress').innerHTML = msg[0].AgentAddress;
                        //document.getElementById('lblvendoremail').innerHTML = msg[0].email;
                    }
                    else {
                        document.getElementById('lblpartyname').innerHTML = msg[0].Title + "-" + msg[0].partyname;
                        document.getElementById('span_toGSTIN').innerHTML = msg[0].togstin;
                        //document.getElementById('lblvendorphoneno').innerHTML = msg[0].phoneno;
                        document.getElementById('spn_toaddress').innerHTML = msg[0].AgentAddress;
                        //document.getElementById('lblvendoremail').innerHTML = msg[0].email;
                    }
                }
            }
        }
        function GetDC_Products() {
            var refno = document.getElementById('txt_Refno').value;
            var data = { 'operation': 'get_DeliveryChallan_click', 'refdcno': refno };
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
            var data = { 'operation': 'GetDC_Products', 'Dispatchsno': Dispatchsno, 'refdcno': refno };
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

            $("#divPrint").css("display", "block");
            var results = '<div><table id="tbl_invoice" class="table table-bordered table-hover dataTable no-footer" border="2" style="width:100%;">';
            results += '<thead><tr style="font-size:12px;background: antiquewhite;"><th>Product</th><th>HSN</th><th>%</th><th>Rate</th><th>Qty</th><th>Dis</th><th>Amount</th></tr></thead></tbody>';
            var j = 1;
            var total_sgst = 0, total_cgst = 0, total_igst = 0, total_amount = 0, total_taxable = 0, total_qty = 0;
            var PAmount2 = 0, total_sgst2 = 0, total_cgst2 = 0, total_igst2 = 0, tot_amount2 = 0;
            var PAmount9 = 0, total_sgst9 = 0, total_cgst9 = 0, total_igst9 = 0, tot_amount9 = 0;
            var PAmount0 = 0, total_sgst0 = 0, total_cgst0 = 0, total_igst0 = 0, tot_amount0 = 0;
            var PAmount6 = 0, total_sgst6 = 0, total_cgst6 = 0, total_igst6 = 0, tot_amount6 = 0;
            var PAmount14 = 0, total_sgst14 = 0, total_cgst14 = 0, total_igst14 = 0, tot_amount14 = 0;
            for (var i = 0; i < msg.length; i++) {
                var itemcode = msg[i].itemcode;
                if (itemcode == "Inventory") {
                }
                else {
                    var sgst = 0;
                    var sgstamount = 0;
                    var cgst = 0;
                    var cgstamount = 0;
                    var Igst = 0;
                    var Igstamount = 0;
                    var totRate = 0;
                    var price = 0;
                    var qty = 0;
                    rate = parseFloat(msg[i].rate);
                    qty += parseFloat(msg[i].qty);
                    total_qty += parseFloat(msg[i].qty);
                    total_amount += parseFloat(msg[i].totalamount); ; ;
                    total_sgst += parseFloat(msg[i].sgstamount);
                    total_cgst += parseFloat(msg[i].cgstamount);
                    total_igst += parseFloat(msg[i].igstamount);
                    if (msg[i].igst == "0" && msg[i].cgst == "0") {
                        PAmount0 += parseFloat(msg[i].taxablevalue);
                        total_sgst0 += parseFloat(msg[i].sgstamount);
                        total_cgst0 += parseFloat(msg[i].cgstamount);
                        total_igst0 += parseFloat(msg[i].igstamount);
                        tot_amount0 += parseFloat(msg[i].totalamount);
                    }
                    if (msg[i].igst == "5" || msg[i].cgst == "2.5") {
                        PAmount2 += parseFloat(msg[i].taxablevalue);
                        total_sgst2 += parseFloat(msg[i].sgstamount);
                        total_cgst2 += parseFloat(msg[i].cgstamount);
                        total_igst2 += parseFloat(msg[i].igstamount);
                        tot_amount2 += parseFloat(msg[i].totalamount);
                    }
                    if (msg[i].igst == "12" || msg[i].cgst == "6") {
                        PAmount6 += parseFloat(msg[i].taxablevalue);
                        total_sgst6 += parseFloat(msg[i].sgstamount);
                        total_cgst6 += parseFloat(msg[i].cgstamount); ;
                        total_igst6 += parseFloat(msg[i].igstamount);
                        tot_amount6 += parseFloat(msg[i].totalamount); ;
                    }
                    if (msg[i].igst == "18" || msg[i].cgst == "9") {
                        PAmount9 += parseFloat(msg[i].taxablevalue);
                        total_sgst9 += parseFloat(msg[i].sgstamount); ;
                        total_cgst9 += parseFloat(msg[i].cgstamount); ;
                        total_igst9 += parseFloat(msg[i].igstamount);
                        tot_amount9 += parseFloat(msg[i].totalamount); ;
                    }
                    if (msg[i].igst == "28" || msg[i].cgst == "14") {
                        PAmount14 += parseFloat(msg[i].taxablevalue);
                        total_sgst14 += parseFloat(msg[i].sgstamount); ;
                        total_cgst14 += parseFloat(msg[i].cgstamount); ;
                        total_igst14 += parseFloat(msg[i].igstamount);
                        tot_amount14 += parseFloat(msg[i].totalamount); ;
                    }
                    results += '<tr>';
                    results += '<td data-title="brandstatus" class="8">' + msg[i].ProductName + '</td>';
                    results += '<td data-title="brandstatus" class="9">' + msg[i].hsncode + '</td>';
                    results += '<td data-title="brandstatus" class="9"><div style="float: right;">' + (parseFloat(msg[i].sgst) + parseFloat(msg[i].cgst)).toFixed(2) + '</div></td>';
                    results += '<td data-title="brandstatus" class="4"><div style="float: right;">' + parseFloat(msg[i].rate); +'</div></td>';
                    results += '<td data-title="brandstatus" class="3"><div style="float: right;">' + parseFloat(msg[i].qty); +'</div></td>';
                    results += '<td data-title="brandstatus" class="3"><div style="float: right;">' + parseFloat(msg[i].discount) + '</div></td>';
                    results += '<td data-title="brandstatus" class="4"><div style="float: right;">' + parseFloat(msg[i].taxablevalue).toFixed(2) + '</div></td>';
                    results += '</tr>'
                    j++;
                }
            }
            var tot = "";
            var tqty = "Total"
            results += '<td data-title="brandstatus" colspan="4"  class="2" style="text-align: center; font-weight: bold; font-size: medium;">' + tqty + '</td>';
            results += '<td data-title="brandstatus" colspan="1"  class="2" style="text-align: center; font-weight: bold; font-size: medium;">' + total_qty.toFixed(2) + '</td>';
            results += '<td data-title="brandstatus"></td>';
            results += '<td data-title="brandstatus"  style="font-weight: bold; font-size: medium;"><span id="totammountcls"><div style="float: right;">' + total_amount.toFixed(2) + '</div></span></td></tr>';
            results += '<tr>';
            results += '</tr>';
            results += '<tr>';
            results += '</tr>';
            //            var invname = "Inventory Details";
            //            results += '<tr >'
            //            results += '<td data-title="brandstatus" class="2"></td>';
            //            results += '<td scope="row" class="1" colspan="13" style="text-align:left;font-size: 14px;"><label>' + invname + '</label></td></tr>';
            //            for (var i = 0; i < msg.length; i++) {
            //                var itemcode = msg[i].itemcode;
            //                if (itemcode == "Inventory") {
            //                    results += '<tr style="font-size: 12px;">'
            //                    results += '<td data-title="brandstatus" class="2"></td>';
            //                    results += '<td data-title="brandstatus" class="2"></td>';
            //                    results += '<td data-title="brandstatus" class="2">' + msg[i].ProductName + '</td>';
            //                    results += '<td data-title="brandstatus" class="2">' + msg[i].qty + '</td>';
            //                    results += '<td data-title="brandstatus" colspan="12" class="2"></td></tr>';
            //                }
            //            }
            results += '</table></div>';
            results += '<div  style="padding-left: 17%;padding-top: 3%;"><table id="tbl_invoice2" class="table table-bordered table-hover dataTable no-footer" border="2" style="width:80%;">';
            results += '<thead><tr style="font-size:12px;background: antiquewhite;"><th>#</th><th>CGST</th><th>SGST</th><th>IGST</th><th>Total</th></tr></thead></tbody>';
            if (total_sgst0 != 0 && total_igst0 != 0) {
                results += '<tr><td data-title="brandstatus"  class="6">0%</td>';
                //                results += '<td data-title="brandstatus"  class="6"><span><div style="float: right;">' + PAmount0.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus"  class="5"><span><div style="float: right;">' + total_sgst0.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus"  class="5"><span><div style="float: right;">' + total_cgst0.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus"  class="5"><span><div style="float: right;">' + total_igst0.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus" class="7"><span><div style="float: right;">' + tot_amount0.toFixed(2) + '</div></span></td></tr>';
            }
            if (total_sgst2 != 0 || total_igst2 != 0) {
                results += '<tr><td data-title="brandstatus"  class="6">5%</td>';
                //results += '<td data-title="brandstatus"  class="6"><span><div style="float: right;">' + PAmount2.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus"  class="5"><span><div style="float: right;">' + total_sgst2.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus"  class="5"><span><div style="float: right;">' + total_cgst2.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus"  class="5"><span><div style="float: right;">' + total_igst2.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus" class="7"><span><div style="float: right;">' + tot_amount2.toFixed(2) + '</div></span></td></tr>';
            }
            if (total_sgst6 != 0 || total_igst6 != 0) {
                results += '<tr><td data-title="brandstatus"  class="6">12%</td>';
                //results += '<td data-title="brandstatus"  class="6"><span><div style="float: right;">' + PAmount6.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus"  class="5"><span><div style="float: right;">' + total_sgst6.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus"  class="5"><span><div style="float: right;">' + total_cgst6.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus"  class="5"><span><div style="float: right;">' + total_igst6.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus" class="7"><span><div style="float: right;">' + tot_amount6.toFixed(2) + '</div></span></td></tr>';
            }
            if (total_sgst9 != 0 || total_igst9 != 0) {
                results += '<tr><td data-title="brandstatus"  class="6">18%</td>';
                // results += '<td data-title="brandstatus"  class="6"><span><div style="float: right;">' + PAmount9.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus"  class="5"><span><div style="float: right;">' + total_sgst9.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus"  class="5"><span><div style="float: right;">' + total_cgst9.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus"  class="5"><span><div style="float: right;">' + total_igst9.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus" class="7"><span><div style="float: right;">' + tot_amount9.toFixed(2) + '</div></span></td></tr>';
            }
            if (total_sgst14 != 0 || total_igst14 != 0) {
                results += '<tr><td data-title="brandstatus"  class="6">28%</td>';
                // results += '<td data-title="brandstatus"  class="6"><span><div style="float: right;">' + PAmount14.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus"  class="5"><span><div style="float: right;">' + total_sgst14.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus"  class="5"><span><div style="float: right;">' + total_cgst14.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus"  class="5"><span><div style="float: right;">' + total_igst14.toFixed(2) + '</div></span></td>';
                results += '<td data-title="brandstatus" class="7"><span><div style="float: right;">' + tot_amount14.toFixed(2) + '</div></span></td></tr>';
            }
            results += '<tr><td data-title="brandstatus"  class="6"></td>';
            //results += '<td data-title="brandstatus"  class="6"><span><div style="float: right;">' + total_taxable.toFixed(2) + '</div></span></td>';
            results += '<td data-title="brandstatus"  class="5"><span><div style="float: right;">' + total_sgst.toFixed(2) + '</div></span></td>';
            results += '<td data-title="brandstatus"  class="5"><span><div style="float: right;">' + total_cgst.toFixed(2) + '</div></span></td>';
            results += '<td data-title="brandstatus"  class="5"><span><div style="float: right;">' + total_igst.toFixed(2) + '</div></span></td>';
            results += '<td data-title="brandstatus" class="7"><span><div style="float: right;">' + total_amount.toFixed(2) + '</div></span></td></tr>';
            results += '</table></div>';
            $("#div_products").html(results);
            var amountwords = total_amount.toFixed(2);
            document.getElementById('recevied').innerHTML = inWords(parseInt(total_amount));
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section class="content-header">
        <h1>
            Delivery Challan<small>Preview</small>
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
                            <td style="width: 5px;">
                            </td>
                            <td>
                             <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="btnDCDetails_click()"><i class="fa fa-refresh"></i> Get DC Details </button>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div id="divPOdata" style="height: 300px; overflow-y: scroll;">
                    </div>
                    <br />
<table id="tbltrip">
<tr>
<td>
Ref No
</td>
<td>
<input type="text" id="txt_Refno" class="form-control" placeholder="Enter Ref No" />
</td>
<td style="width: 5px;">
</td>
<td style="width: 5px;">
</td>
<td>
<button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="GetDC_Products();"><i class="fa fa-refresh"></i> Generate </button>
</td>
</tr>
</table>
                </div>
                <div>
                </div>
              
               <div id="divPrint" style="display:none;font-family: initial;">
                <div style="border: 2px solid gray;" class="col-md-12">
                    <div  style="width: 13%; float: left;">
                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="60px" height="60px" />
                        <br />
                    </div>
                   
                    <div style="border: 1px solid gray;">
                        <div align="center" style="font-family: Arial; font-size: 12pt; font-weight: bold;
                            color: Black;">
                            <span id="spncompanyname" style="font-size: 12px;"></span>
                            <br />
                        </div>
                        <div align="center" style="font-family: Arial; font-size: 10px; font-weight: bold;
                            color: Black; padding-left: 10px; font-size: 12px !important;">
                             <span id="spnAddress" style="font-size: 11px;font-weight: bold;"></span>
                             <br />
                              <%--<span id="Span1" style="font-size: 11px;font-weight: bold;"> Website: www.vyshnavi.in</span>--%>
                            <br /> <label style="font-size: 12px;font-weight: bold !important;">GSTIN:</label> <span id="lblgstin" style="font-size: 11px;"></span>
                        <br />
                        </div>
                    </div>
                    <%--<div style="width:73%; padding-left:31%;display:none;">
                        <label style="font-size: 12px;font-weight: bold !important;">   GSTIN.</label> <span id="lblgstin" style="font-size: 12px;"></span>
                        </div>--%>
                    <div align="center" style="border-bottom: 2px solid gray; border-top: 1px solid gray;background: antiquewhite;">
                        <span style="font-size: 18px; font-weight: bold;" id="lbldcType"></span>
                    </div>
                    <div style="width: 100%;">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 49%;border: 2px solid gray;padding-left: 2%;">
                                <label style="font-size: 12px;font-weight: bold !important;">
                                        Name :</label>
                                    <span id="lblpartyname" style="font-size: 11px;font-weight: bold;"></span>
                                      <br />
                                <label style="font-size: 12px;font-weight: bold;">
                                        Bill To </label>
                                  <span id="spn_toaddress" style="font-size: 11px;font-weight: bold;"></span>
                                  <input type="hidden" id="hdnDespsno"/>
                                    <br />
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Invoice No. :</label>
                                    <span id="lbldcno" style="font-size: 11px;font-weight: bold;"></span>
                                </td>
                                <td>
                                </td>
                                <td style="width: 50%;border: 2px solid gray;padding-left: 2%;">
                                 <label style="font-size: 12px;font-weight: bold !important;">
                                       TO GSTIN :</label>
                                    <span id="span_toGSTIN" style="font-size: 11px;font-weight: bold;"></span>
                                    <br />
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                       To State Name :</label>
                                    <span id="lbl_tostate" style="font-size: 11px;font-weight: bold;"></span>
                                    </br>
                                     <label style="font-size: 12px;font-weight: bold !important;">
                                         Date :</label>
                                    <span id="lblassigndate" style="font-size: 11px;font-weight: bold;"></span>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </div>
                     <div id="div_products">
                    </div>
                    <table style="width: 100%;">
                          <tr>
                                <td style="width: 49%;">
                                    <label>
                                        Received Rs :  
                                         </label>
                                    <span id="recevied" onclick="test.inum.value = toWords(test.inum.value);" value="To Words"   ></span>
                                </td>
                            </tr>
                    </table>
                    <br />
                    <br />
                    <div id="div_products">
                    </div>
                     <table style="width: 100%;">
                                    <tr> 
                                     <td  colspan="3"></td>
                                       <td style="width: 50%;" >
                                            For <span id="lblsignname" style="font-weight: bold; font-size: 11px;"></span>
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
                                    <span style="font-size: 11px;">We declare that this invioce shows the actual price of the goods decribe and that all particulars are ture and correct</span>
                                    <br />
                                    </div>
                    <div style="text-align: center;">
                     <label>* Thank you for shopping with VYSHNAVI *</label>
                    </div>
                </div>
 <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="javascript:CallPrint('divPrint');"><i class="fa fa-print"></i> Print </button>
                        </div>
                        </div>
                        </section>
</asp:Content>

