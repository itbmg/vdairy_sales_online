<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EWaybillDetails.aspx.cs" Inherits="EWaybillDetails" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="JIC/JIC.js?v=101" type="text/javascript"></script>
    <script src="JSF/imagezoom.js" type="text/javascript"></script>
    <script src="Plant/Script/fleetscript.js?v=3006" type="text/javascript"></script>
    <%-- <link href="http://www.jqueryscript.net/css/jquerysctipttop.css" rel="stylesheet" type="text/css" />
    <script src="http://code.jquery.com/jquery-latest.min.js" type="text/javascript"></script>--%>
    <script src="Barcode/jquery-barcode.js" type="text/javascript"></script>
    <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />

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


            var branchID = document.getElementById("ddlsalesOffice").value;
            var ddltype = document.getElementById("ddltype").value;

            var data = { 'operation': 'GetEWayDetails', 'FromDate': FromDate, 'BranchID': branchID, 'ddltype': ddltype };
            var s = function (msg) {
                if (msg) {
                    fillewaydetails(msg);
                    UpdateVehicles();

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function fillewaydetails(msg) {
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" ID="tabledetails">';
            results += '<thead><tr><th scope="col">AgentName</th><th scope="col">GstNo</th><th scope="col">InvoiceNo</th><th scope="col">TotalValue</th><th scope="col">Distance</th><th scope="col">VehcleNo</th><th scope="col" style="display:none;">Type</th><th scope="col">Status</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                //if (msg[i].Totalvalue >= "50000") {
                    results += '<tr>';
                    results += '<td id="spnagentname" type="text" class="clsAgentName">' + msg[i].AgentName + '</td>';
                    results += '<td id="spnagentname" type="text" class="clsgst">' + msg[i].gstno + '</td>';
                    results += '<td id="spnagentname" style="display:none;"  class="clsAgentId">' + msg[i].Agentid + '</td>';
                    results += '<td id="spnagentname"  class="clsInvoiceNo">' + msg[i].InvoiceNo + '</td>';
                    results += '<td id="spnagentname"  class="clsTotvalue">' + msg[i].Totalvalue + '</td>';
                    if (msg[i].Distance == "0") {
                        results += '<td class="cls_Distance"><input id="txtDistance" type="text" class="form-control" placeholder="Enter Distance" value="' + msg[i].Distance + '"/></td>';
                    }
                    else {
                        results += '<td data-title="brandstatus"  class="cls_Distance">' + msg[i].Distance + '</td>';
                    }
                    results += '<td data-title="brandstatus" style = "display:none;" class="cls_ewb_no">' + msg[i].ewb_no + '</td>';
                    results += '<td data-title="brandstatus" style = "display:none;" class="cls_ewb_date">' + msg[i].ewb_date + '</td>';
                    results += '<td data-title="brandstatus" style = "display:none;" class="cls_ewb_expiredate">' + msg[i].ewb_expiredate + '</td>';
                    results += '<td data-title="brandstatus" style = "display:none;" class="cls_GeneratedBy">' + msg[i].GeneratedBy + '</td>';
                    results += '<td data-title="brandstatus" style = "display:none;" class="cls_InvoiceDate">' + msg[i].ewb_expiredate + '</td>';
                    if (msg[i].VehNo == "") {
                        results += '<td><input id="ddlvehcle" class="cls_VehNo form-control" placeholder="Select Vehcle Number" type="text"  class="form-control"  style=""  value="' + msg[i].VehNo + '"/></td>';
                    }
                    else {
                        results += '<td data-title="brandstatus" id="ddlvehcle" class="cls_VehNo">' + msg[i].VehNo + '</td>';
                    }
                results += '<td data-title="brandstatus" style = "display:none;" class="12">' + msg[i].UserType + '</td>';
                if (msg[i].InvoiceStatus == "R") {
                    results += '<td data-title="brandstatus" id="spnIRN" style = "display:none;" class="clsIRN" >' + msg[i].Irn + '</td>';
                }
                    var status = msg[i].status;
                    if (status == "R") {
                        status = "Raised"
                    }
                    if (msg[i].status == "R") {
                        results += '<td data-title="brandstatus"  class="11">' + status + '</td>';
                        results += '<td data-title="brandstatus"><button type="button"  disabled="true"  title="Click Here To Generate Einvoice!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="GenerateEinvoice(this)"><i class="fa fa-file-text"></i></button></td>';
                        results += '<td data-title="brandstatus"><button type="button" title="Click Here To View E-WayBill!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 prntcls"  onclick="View_eWayBill_Click(this)"><span class="glyphicon glyphicon-list-alt" style="top: 0px !important;"></span></button></td>';

                    }
                    else {
                        results += '<td data-title="brandstatus"  class="11">' + status + '</td>';
                        results += '<td data-title="brandstatus"><button  type="button" title="Click Here To Generate E-WayBill!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="Generate_eWayBill(this)"><i class="fa fa-file-text"></i></span></button></td>';
                        results += '<td data-title="brandstatus"><button type="button" disabled="true" title="Click Here To View E-WayBill!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 prntcls"  onclick="View_eWayBill_Click(this)"><span class="glyphicon glyphicon-list-alt" style="top: 0px !important;"></span></button></td>';
                    }
                results += '<td data-title="brandstatus"><button type="button" disabled="true" title="Click Here To Cancel E-WayBill!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 removeclass"   onclick="CanceleWayBill(this)"><span class="glyphicon glyphicon-remove-circle" style="top: 0px !important;"></span></button></td></tr>';
                    results += '<td style="display:none" class="4">' + i + '</td></tr>';
                //}
            }
            results += '</table></div>';
            $("#divEWayBilldata").html(results);
        }
        var BindVehicles = [];
        function UpdateVehicles() {
            var data = { 'operation': 'GetBranchVehicles' };
            var s = function (msg) {
                if (msg) {
                    BindVehicles = msg;

                    var VehiclesList = [];
                    for (var i = 0; i < msg.length; i++) {
                        var VehicleNo = msg[i].VehicleNo;
                        VehiclesList.push(VehicleNo);
                    }
                    $('.cls_VehNo').autocomplete({
                        source: VehiclesList,
                        //change: vehclenamechange,
                        autoFocus: true
                    });
                }
            }
            var e = function (x, h, e) {
                alert(e.toString());
            };
            callHandler(data, s, e);
        }

        //function vehclenamechange() {
        //    var vehcleid = document.getElementById("ddlvehcle").value;;
        //    for (var i = 0; i < BindVehicles.length; i++) {
        //        var VehicleNo = BindVehicles[i].VehicleNo;
        //        if (vehcleid == VehicleNo) {
        //            document.getElementById("spnvehicleid").value = BindVehicles[i].sno;
        //        }

        //    }
        //}

        //Generate eWay Bill
        function Generate_eWayBill(id) {

            var FromDate = document.getElementById('txtFromDate').value;
            var soid = document.getElementById('ddlsalesOffice').value;


            //var UserType = $(id).parent().parent().children('.12').html();
            var UserType = document.getElementById("ddltype").value;

            var Distance = $(id).closest("tr").find('#txtDistance').val();
            var invoiceno = $(id).parent().parent().children('.clsInvoiceNo').html();
            if (Distance == "" || Distance =="0") {
                alert("Enter Distance");
                return false;
            }
            if (UserType == "R") {
                var irn = $(id).parent().parent().children('.clsIRN').html();
                if (irn == "" || irn == null) {
                    alert("IRN Number not available Please Raise eInvoice");
                    return false;
                }
            }
            var vehcleno = $(id).closest("tr").find('#ddlvehcle').val();
            var agentid = $(id).parent().parent().children('.clsAgentId').html();
            var Totvalue = $(id).parent().parent().children('.clsTotvalue').html();
            if (!confirm("Do you really want Save")) {
                return false;
            }
            if (UserType == "R") {

                if (Totvalue >= "50000") {
                    var data = { 'operation': 'generate_ewaybill_using_IRN', 'soid': soid, 'agentid': agentid, 'invoiceno': invoiceno, 'Distance': Distance, 'vehcleno': vehcleno, 'FromDate': FromDate, 'irn': irn };
                }
                else {
                    alert("InvoiceAmount must be graterthan fifty thousand");
                    return false;
                }
            }
            else {
                if (Totvalue >= "50000") {
                    var data = { 'operation': 'generate_ewaybill_Non_Registerd', 'soid': soid, 'agentid': agentid, 'invoiceno': invoiceno, 'Distance': Distance, 'vehcleno': vehcleno, 'FromDate': FromDate };
                }
                else {
                    alert("InvoiceAmount must be graterthan fifty thousand");
                    return false;
                }
            }
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    //spnJsonData.innerHTML = JSON.stringify(msg);
                    GenerateClick();
                    if (msg == "Session Expired") {
                        window.location = "Login.aspx";
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

        //eWay Bill Print
        function View_eWayBill_Click(thisid) {
            var FromDate = document.getElementById('txtFromDate').value;
            var soid = document.getElementById('ddlsalesOffice').value;
            var status = $(thisid).parent().parent().children('.11').html();
            var invoiceno = $(thisid).parent().parent().children('.clsInvoiceNo').html();
            var Distance = $(thisid).parent().parent().children('.cls_Distance').html();
            var vehcleno = $(thisid).parent().parent().children('.cls_VehNo').html();
            //var vehicleid = $(thisid).parent().parent().children('.spnvehicleid').html();
            var agentid = $(thisid).parent().parent().children('.clsAgentId').html();
            var ewb_no = $(thisid).parent().parent().children('.cls_ewb_no').html();
            var ewb_date = $(thisid).parent().parent().children('.cls_ewb_date').html();
            var ewb_expiredate = $(thisid).parent().parent().children('.cls_ewb_expiredate').html();
            var InvoiceDate = $(thisid).parent().parent().children('.cls_InvoiceDate').html();
            var GeneratedBy = $(thisid).parent().parent().children('.cls_GeneratedBy').html();



            var branchID = document.getElementById("ddlsalesOffice").value;
            var ddltype = document.getElementById("ddltype").value;

            var data = { 'operation': 'btnAgentInvoice_click', 'fromdate': FromDate, 'AgentId': agentid, 'SOID': branchID, 'ddltype': ddltype };
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


                        GenerateQRCode(ewb_no);
                        generateBarcode(ewb_no);
                        document.getElementById('spn_eWayBillNo').innerHTML = ewb_no;
                        document.getElementById('spn_eWayDate').innerHTML = ewb_date;
                        document.getElementById('spn_Gene_by').innerHTML = GeneratedBy;
                        document.getElementById('spnExpireDate').innerHTML = ewb_expiredate;
                        document.getElementById('spnMode').innerHTML = "Road";
                        document.getElementById('spnDistance').innerHTML = Distance;
                        document.getElementById('spnType').innerHTML = "Outward - Others-stock for tranpor";
                        document.getElementById('spnDocDetail').innerHTML = invoiceno;
                        document.getElementById('spnTransType').innerHTML = "Regular";

                        document.getElementById('spn_VehcleMode').innerHTML = "Road";
                        document.getElementById('spn_VehcleNo').innerHTML = vehcleno;
                        document.getElementById('spn_From').innerHTML = "Wyra";
                        document.getElementById('spnEnterdDate').innerHTML = ewb_date;
                        document.getElementById('spnEnterBy').innerHTML = GeneratedBy;

                        document.getElementById('spnTransPortName').innerHTML = "Vyshnavi Foods";
                        document.getElementById('spnTransPortDate').innerHTML = ewb_date;

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
            //document.getElementById('spnAddress').innerHTML = "";
            //document.getElementById('spngstnno').innerHTML = "";
            //document.getElementById('spninvoiceno').innerHTML = "";
            //document.getElementById('spninvoicedate').innerHTML = "";
            //            document.getElementById('spnfrmstatename').innerHTML = msg[0].frmstatename;
            //            document.getElementById('spnstatecode').innerHTML = msg[0].frmstatecode;
            //document.getElementById('spndateofsupply').innerHTML = "";
            document.getElementById('spn_From').innerHTML = "";

            document.getElementById('spnfromname').innerHTML = "";
            document.getElementById('spnfromaddress').innerHTML = "";
            document.getElementById('spnfromgstn').innerHTML = "";
            //document.getElementById('spngstnum').innerHTML = "";

            document.getElementById('spnfromstate').innerHTML = "";
            document.getElementById('spnfromstatecode').innerHTML = "";
            document.getElementById('lblpartyname').innerHTML = "";
            //            document.getElementById('lblroutename').innerHTML = msg[0].AgentName;lblsignname
            document.getElementById('spn_toaddress').innerHTML = "";
            document.getElementById('lbl_tostate').innerHTML = "";
            document.getElementById('lbl_tostatecode').innerHTML = "";
            document.getElementById('lblvendorphoneno').innerHTML = "";
            document.getElementById('lblvendoremail').innerHTML = "";
            //document.getElementById('lblsignname').innerHTML = "";
            document.getElementById('lbl_companymobno').innerHTML = "";
            document.getElementById('lbl_companyemail').innerHTML = "";
            //document.getElementById('spninvoicetype').innerHTML = "";
        }
        function fillheaderdetails(msg) {
            clearall();
            if (msg.length > 0) {
                document.getElementById('span_toGSTIN').innerHTML = msg[0].togstin;
                document.getElementById('lbltile').innerHTML = "e-Way Bill";
                //document.getElementById('spnAddress').innerHTML = msg[0].BranchAddress;
                //document.getElementById('spngstnno').innerHTML = msg[0].fromgstn;
                //document.getElementById('spninvoiceno').innerHTML = msg[0].invoiceno;
                //document.getElementById('spninvoicedate').innerHTML = msg[0].invoicedate;
                document.getElementById('spn_From').innerHTML = msg[0].city;
                document.getElementById('spnfromname').innerHTML = msg[0].titlename;
                document.getElementById('spnfromaddress').innerHTML = msg[0].BranchAddress;
                document.getElementById('spnfromgstn').innerHTML = msg[0].fromgstn;
                //document.getElementById('spngstnum').innerHTML = msg[0].fromgstn;
                document.getElementById('spnfromstate').innerHTML = msg[0].frmstatename;
                document.getElementById('spnfromstatecode').innerHTML = msg[0].frmstatecode;
                document.getElementById('lblpartyname').innerHTML = msg[0].AgentName;
                document.getElementById('spn_toaddress').innerHTML = msg[0].AgentAddress;
                document.getElementById('lbl_tostate').innerHTML = msg[0].tostatename;
                document.getElementById('lbl_tostatecode').innerHTML = msg[0].tostatecode;
                document.getElementById('lblvendorphoneno').innerHTML = msg[0].phoneno;
                document.getElementById('lblvendoremail').innerHTML = msg[0].email;
                //document.getElementById('lblsignname').innerHTML = msg[0].titlename;
                document.getElementById('lbl_companymobno').innerHTML = msg[0].companyphone;
                document.getElementById('lbl_companyemail').innerHTML = msg[0].companyemail;
                //document.getElementById('spninvoicetype').innerHTML = msg[0].dctype;
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
                //document.getElementById('spninvoiceno').innerHTML = msg[msglength - 1].invoiceno;
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
            results += '</table></div>';
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

        function generateBarcode(ewb_no) {

            var value = ewb_no;
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
        function GenerateQRCode(ewb_no) {
            var data = ewb_no;
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
        <h1>eWay Bill<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">eWay Bill</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>eWay Bill Details
                </h3>
            </div>
            <div class="box-body">
                <div style="width: 100%; background-color: #fff">
                    <div>
                        <table>
                            <tr>
                                <td class="divsalesOffice">
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
                            <td>Type
                            </td>
                            <td>
                                <select id="ddltype" class="form-control">
                                    <option value="R">Registerd</option>
                                    <option value="N">Non-Registerd</option>
                                </select>
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
                                        <%--<div style="border: 1px solid gray;">
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
                                                
                                                <br />
                                                <br />
                                            </div>
                                            <div style="width: 73%; padding-left: 12%; text-align: center; display: none;">
                                                <span id="spngstnno" style="font-size: 14px;"></span>
                                                <br />
                                            </div>
                                            <br />
                                        </div>--%>
                                         <div>
                                            <br />
                                            <div style="font-family: Arial; font-size: 40px; font-weight: bold; color: Black; text-align: center;">
                                                <span id="lbltile"></span>
                                                <br />
                                            </div>
                                            <br />
                                        </div>
                                        <%--<div align="center" style="border-bottom: 1px solid gray; border-top: 1px solid gray; background: antiquewhite;">
                                            <span style="font-size: 18px; font-weight: bold;" id="spninvoicetype">TaxInvoice</span>
                                        </div>--%>
                                        <div style="width: 100%;">
                                            <%-- <table style="width: 100%;border: 3px solid #dddddd;"  class="table table-bordered table-hover dataTable no-footer">--%>
                                            <div style="width: 100%;float: left; border-bottom: 1px solid gray; border-top: 1px solid gray; background: antiquewhite;">
                                                <span style="font-size: 18px; font-weight: bold;" id="spnInvoiceHeader">1.E-WAY BILL Details</span>
                                            </div>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 25%; border-top: 2px solid gray; padding-left: 2%;">
                                                        <label style="font-size: 12px; font-weight: bold !important;">
                                                            eWay Bill No :</label>
                                                        <span id="spn_eWayBillNo" style="font-size: 11px;"></span>
                                                    </td>
                                                    <td style="width: 25%; border-top: 2px solid gray; padding-left: 2%;">
                                                        <label style="font-size: 12px; font-weight: bold !important;">
                                                            Generated Date :</label>
                                                        <span id="spn_eWayDate" style="font-size: 11px;"></span>
                                                    </td>
                                                    <td style="width: 25%; border-top: 2px solid gray; padding-left: 2%;">
                                                        <label style="font-size: 12px; font-weight: bold !important;">
                                                            Generated By :</label>
                                                        <span id="spn_Gene_by" style="font-size: 11px;"></span>
                                                    </td>
                                                    <td style="width: 25%; border-top: 2px solid gray; padding-left: 2%;">
                                                        <label style="font-size: 12px; font-weight: bold !important;">
                                                            Valid Upto:</label>
                                                        <span id="spnExpireDate" style="font-size: 11px;"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%; padding-left: 2%;" colspan="2">
                                                        <label style="font-size: 12px; font-weight: bold !important;">
                                                            Mode :</label>
                                                        <span id="spnMode" style="font-size: 11px;"></span>
                                                    </td>
                                                    <td style="width: 25%; padding-left: 2%;" colspan="2">
                                                        <label style="font-size: 12px; font-weight: bold !important;">
                                                            Approx Distance: :</label>
                                                        <span id="spnDistance" style="font-size: 11px;"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%; border-bottom: 2px solid gray; padding-left: 2%;">
                                                        <label style="font-size: 12px; font-weight: bold !important;">
                                                            Type :</label>
                                                        <span id="spnType" style="font-size: 11px;"></span>
                                                    </td>
                                                    <td style="width: 25%; border-bottom: 2px solid gray; padding-left: 2%;">
                                                        <label style="font-size: 12px; font-weight: bold !important;">
                                                            Document Details:</label>
                                                        <span id="spnDocDetail" style="font-size: 11px;"></span>
                                                    </td>
                                                    <td style="width: 25%; border-bottom: 2px solid gray; padding-left: 2%;" colspan="2">
                                                        <label style="font-size: 12px; font-weight: bold !important;">
                                                            Transaction type</label>
                                                        <span id="spnTransType" style="font-size: 11px;"></span>
                                                    </td>
                                                </tr>
                                            </table>

                                            <%-- <div style="width: 100%;">--%>
                                            <%-- <table style="width: 100%;border: 3px solid #dddddd;"  class="table table-bordered table-hover dataTable no-footer">--%>
                                          

                                            <div style="width: 100%;float: left; border-bottom: 1px solid gray; border-top: 1px solid gray; background: antiquewhite;">
                                                <span style="font-size: 18px; font-weight: bold;" id="spnPartyDetils">2. Address Details</span>
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
                                        </div>
                                        <div style="width: 100%;float: left; border-bottom: 1px solid gray; border-top: 1px solid gray; background: antiquewhite;">
                                                <span style="font-size: 18px; font-weight: bold;" id="spnGoods">3. Goods Details</span>
                                            </div>
                                        <div id="div_itemdetails1">
                                        </div>
                                        <table>
                                            <label style="font-size: 16px; font-weight: bold;">
                                                Towards:
                                            </label>
                                            <label>
                                                Rs.</label>
                                            <span id="recevied" onclick="test.rnum.value = toWords(test.inum.value);" value="To Words"></span>
                                        </table>
                                        <br />

                                          <div style="width: 100%;float: left; border-bottom: 1px solid gray; border-top: 1px solid gray; background: antiquewhite;">
                                                <span style="font-size: 18px; font-weight: bold;" id="spnTransHead">4.Transportation Details</span>
                                            </div>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 50%; border-bottom: 2px solid gray; padding-left: 2%;">
                                                        <label style="font-size: 12px;">
                                                            Transporter ID & Name :</label>
                                                        <span id="spnTransPortName" style="font-size: 14px;">B2B</span>
                                                    </td>
                                                    <td style="width: 50%; border-bottom: 2px solid gray; padding-left: 2%;">
                                                        <label style="font-size: 12px;">
                                                            Transporter Doc. No & Date : </label>
                                                        <span id="spnTransPortDate" style="font-size: 14px;"></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        <br />
                                         <div style="width: 100%;float: left; border-bottom: 1px solid gray; border-top: 1px solid gray; background: antiquewhite;">
                                                <span style="font-size: 18px; font-weight: bold;" id="spnVehcleDetils">5. Vehicle Details</span>
                                            </div>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 15%; border-top: 2px solid gray; padding-left: 2%;" rowspan="2";>
                                                    <label style="font-size: 12px; font-weight: bold !important;">
                                                        Mode</label>
                                                </td>
                                                
                                                <td style="width: 15%; border-top: 2px solid gray; padding-left: 2%;" rowspan="2";>
                                                    <label style="font-size: 12px; font-weight: bold !important;">
                                                        Vehicle / Trans Doc No & Dt</label>
                                                </td>
                                                <td style="width: 15%; border-top: 2px solid gray; padding-left: 2%;" rowspan="2";>
                                                    <label style="font-size: 12px; font-weight: bold !important;">
                                                        From</label>
                                                </td>
                                                
                                                <td style="width: 15%; border-top: 2px solid gray; padding-left: 2%;" rowspan="2";>
                                                    <label style="font-size: 12px; font-weight: bold !important;">
                                                        Entered Date</label>
                                                </td>
                                                <td style="width: 15%; border-top: 2px solid gray; padding-left: 2%;" rowspan="2";>
                                                    <label style="font-size: 12px; font-weight: bold !important;">
                                                        Entered By</label>
                                                </td>
                                                <%--<td style="width: 15%; border-top: 2px solid gray; padding-left: 2%;">
                                                    <label style="font-size: 12px; font-weight: bold !important;">
                                                        CEWB No.(If any)</label>
                                                </td>
                                                <td style="width: 10%; border-top: 2px solid gray; padding-left: 2%;">
                                                    <label style="font-size: 12px; font-weight: bold !important;">
                                                        Multi Veh.Info(If any)</label>
                                                </td>--%>
                                            </tr>
                                            </table>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 15%; border-top: 2px solid gray;border-bottom: 2px solid gray; padding-left: 2%;" rowspan="2";>
                                                    <span id="spn_VehcleMode" style="font-size: 11px;"></span>
                                                </td>
                                                <td style="width: 15%; border-top: 2px solid gray;border-bottom: 2px solid gray; padding-left: 2%;" rowspan="2";>
                                                    <span id="spn_VehcleNo" style="font-size: 11px;"></span>
                                                </td>
                                                <td style="width: 15%; border-top: 2px solid gray;border-bottom: 2px solid gray; padding-left: 2%;" rowspan="2";>
                                                    <span id="spn_From" style="font-size: 11px;"></span>
                                                </td>
                                                
                                                <td style="width: 15%; border-top: 2px solid gray;border-bottom: 2px solid gray; padding-left: 2%;" rowspan="2";>
                                                    <span id="spnEnterdDate" style="font-size: 11px;"></span>
                                                </td>
                                                <td style="width: 15%; border-top: 2px solid gray;border-bottom: 2px solid gray; padding-left: 2%;" rowspan="2";>
                                                    <span id="spnEnterBy" style="font-size: 11px;"></span>
                                                </td>
                                               <%-- <td style="width: 15%; border-top: 2px solid gray; padding-left: 2%;">
                                                </td>
                                                <td style="width: 10%; border-top: 2px solid gray; padding-left: 2%;">
                                                </td>--%>
                                            </tr>
                                        </table>
                                        <br />
                                        <%--add BarCode--%>
                                        <div align="center">
                                            <div id="barcodeTarget" class="barcodeTarget">
                                            </div>
                                            <canvas id="canvasTarget" width="80" height="150"></canvas>
                                        </div>
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

