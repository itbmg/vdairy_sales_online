<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="OfferDc_Print.aspx.cs" Inherits="OfferDc_Print" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }

        function btnDCDetails_click() {
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
            var data = { 'operation': 'get_DeliveryChallan_click', 'refdcno': refdcno};
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        FillDCMain_details(msg);
                        GetOfferDC_Products();
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
            
            if (msg.length > 0) {
                document.getElementById('spncompanyname').innerHTML = msg[0].Title
                document.getElementById('spnAddress').innerHTML = msg[0].Address
                document.getElementById('spnfromaddress').innerHTML = msg[0].Address
                document.getElementById('span_toGSTIN').innerHTML = msg[0].gstin;
                document.getElementById('lblgstin').innerHTML = msg[0].fromgstin;
                document.getElementById('spnfromgstn').innerHTML = msg[0].fromgstin;
                document.getElementById('lbl_tostate').innerHTML = msg[0].tostatename;
                document.getElementById('lbl_tostatecode').innerHTML = msg[0].tostatecode;
                document.getElementById('lbl_fromstate').innerHTML = msg[0].fromstatename;
                //document.getElementById('spnfstate').innerHTML = msg[0].fromstatename;
                document.getElementById('spnfromstate').innerHTML = msg[0].fromstatename;
                document.getElementById('lbl_fromstate_code').innerHTML = msg[0].fromstatecode;
                // document.getElementById('spnfstcode').innerHTML = msg[0].fromstatecode;
                document.getElementById('spnfromstatecode').innerHTML = msg[0].fromstatecode;
                document.getElementById('lblRefdcno').innerHTML = document.getElementById('txt_Refno').value

                document.getElementById('lbldcno').innerHTML = msg[0].DcNo
                document.getElementById('lblassigndate').innerHTML = msg[0].assigndate
                document.getElementById('spndateofsupply').innerHTML = msg[0].assigndate

                document.getElementById('lbldisptime').innerHTML = msg[0].PlanTime

                document.getElementById('lblpartyname').innerHTML = msg[0].partyname
                document.getElementById('spnplaceofsupply').innerHTML = msg[0].city
                document.getElementById('hdnDespsno').value = msg[0].Dispatchsno

                document.getElementById('lblroutename').innerHTML = msg[0].routename
                document.getElementById('lblvehicleno').innerHTML = msg[0].vehicleno
                document.getElementById('lbldispat').innerHTML = msg[0].Dispatcher
                document.getElementById('lbldcType').innerHTML = msg[0].dctype

                document.getElementById('spnfromname').innerHTML = msg[0].branchname
            }
        }
        function GetOfferDC_Products() {
            var refno = document.getElementById('txt_Refno').value;
            var data = { 'operation': 'get_DeliveryChallan_click', 'refdcno': refno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        FillDCMain_details(msg);
                        GetProducts();
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
        function GetProducts() {
            var refno = document.getElementById('txt_Refno').value;
            var Dispatchsno = document.getElementById('hdnDespsno').value;
            var data = { 'operation': 'GetOfferDC_Products', 'Dispatchsno': Dispatchsno, 'refdcno': refno };
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
            var results = '<div  style="overflow:auto;"><table border="1" style="width: 100%;" class="table table-bordered table-hover dataTable no-footer">';
            results += '<thead><tr><th scope="col">Sl No</th><th scope="col">Product Name</th><th scope="col">Qty</th><th scope="col">OfferQty</th><th scope="col">Crates</th><th scope="col">Cans</th><th scope="col">Rate</th><th scope="col">Amount</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>'
                results += '<td scope="row" class="1"  style="text-align:center;">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].ProductName + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].qty + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].OfferQty + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].crates + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].cans + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].rate + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].amount + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_products").html(results);
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
                             <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="GetOfferDC_Products();"><i class="fa fa-refresh"></i> Generate </button>
                                                    </td>
                                                </tr>
                                            </table>
                </div>
                <div>
                </div>
               <div id="divPrint" >
                <div style="border: 2px solid gray;">
                    <div style="width: 17%; float: right;padding-top: 12px;">
                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="100px" height="72px" />
                        <br />
                    </div>
                    <div style="border: 1px solid gray;">
                        <div style="font-family: Arial; font-size: 20pt; font-weight: bold; color: Black; text-align:center;">
                            <span id="spncompanyname" style="font-size: 20px;"></span>
                            <br />
                        </div>
                        <div style="width:73%; padding-left:12%; text-align:center;">
                        <span id="spnAddress" style="font-size: 11px;"></span>
                        <br />
                        <br />
                        </div>

                          <div style="width:73%; padding-left:31%;display:none;">
                        <label style="font-size: 12px;font-weight: bold !important;">   GSTIN.</label> <span id="lblgstin" style="font-size: 12px;"></span>
                        </div>
                         <div style="width:73%;padding-left:27%;">
                         <br />
                          <div style="display:none;">
                          <label style="font-size: 12px;"> State Name.</label> <span id="lbl_fromstate" style="font-size: 12px;"></span>
                          <label style="font-size: 12px;"> State Code.</label> <span id="lbl_fromstate_code" style="font-size: 12px;"></span>
                          </div>
                        </div>
                    </div>
                    <div align="center" style="border-bottom: 2px solid gray; border-top: 1px solid gray;background: antiquewhite;">
                        <span style="font-size: 18px; font-weight: bold;" id="lbldcType"></span>
                    </div>
                    <div style="width: 100%;">
                      <div style="width: 100%;">
                       
                            <table style="width: 100%;">
                            <tr>
                            <td style="width: 49%;border: 2px solid gray;padding-left: 2%;">
                             <label style="font-size: 12px;">
                                        Bill From </label>
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Name :</label>
                                    <span id="spnfromname" style="font-size: 11px;"></span>
                                    <br />
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Address :</label>
                                    <span id="spnfromaddress" style="font-size: 11px;"></span>
                                    <br />
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                         GSTIN :</label>
                                    <span id="spnfromgstn" style="font-size: 11px;font-weight: bold !important;"></span>
                                    <br />
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        State Name :</label>
                                    <span id="spnfromstate" style="font-size: 11px;"></span>
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                       State Code :</label>
                                    <span id="spnfromstatecode" style="font-size: 11px;"></span>
                                    <br />
                                </td>
                                
                                <td style="width: 49%;border: 2px solid gray;padding-left: 2%;padding-bottom: 5%;">
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Transport Mode :</label>
                                    <span id="spntransportmode" style="font-size: 11px;"></span>
                                    <br />
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Vehicle No. :</label>
                                    <span id="lblvehicleno" style="font-size: 11px;"></span>
                                    <br />
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Date of Supply :</label>
                                    <span id="spndateofsupply" style="font-size: 11px;"></span>
                                    <br />
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Place of Supply :</label>
                                    <span id="spnplaceofsupply" style="font-size: 11px;"></span>
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%;">
                        <tr style="background:antiquewhite;">
                                <td style="text-align:center; font-weight: bold;">
                                 <label style="font-size: 12px;">
                                        Bill To </label>
                                  </td>
                                  <td style="text-align:center;">
                                  <%--<label style="font-size: 12px;">
                                        Ship To </label>--%>
                                  </td>
                            </tr>
                            <tr>
                                <td style="width: 49%;border: 2px solid gray;padding-left: 2%;">
                                <label style="font-size: 12px;font-weight: bold !important;">
                                        Name :</label>
                                    <span id="lblpartyname" style="font-size: 11px;"></span>
                                    <input type="hidden" id="hdnDespsno"/>
                                    <br />
                                     <span id="lblroutename" style="font-size: 11px;"></span>
                                     <br />
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Address :</label>
                                    <span id="spn_toaddress" style="font-size: 11px;"></span>
                                    <br />
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        GSTIN :</label>
                                    <span id="span_toGSTIN" style="font-size: 11px;"></span>
                                    <br />
                                     <label style="font-size: 12px;font-weight: bold !important;">
                                        State Name :</label>
                                    <span id="lbl_tostate" style="font-size: 11px;"></span>
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                     <label style="font-size: 12px;font-weight: bold !important;">
                                        State Code :</label>
                                    <span id="lbl_tostatecode" style="font-size: 11px;"></span>
                                    <br />
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Telephone no :</label>
                                    <span id="lblvendorphoneno" style="font-size: 11px;"></span>
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Email Id :</label>
                                    <span id="lblvendoremail" style="font-size: 11px;"></span>
                                    <br />
                                </td>
                                <td style="width: 49%;border: 2px solid gray;padding-left: 2%;">
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        DC. No. :</label>
                                    <span id="lbldcno" style="font-size: 11px;"></span>
                                    <br />
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                        Ref NO :</label>
                                    <span id="lblRefdcno" style="font-size: 11px;"></span>
                                    <br />
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                         Date :</label>
                                    <span id="lblassigndate" style="font-size: 11px;"></span>
                                    <br />
                                       <label style="font-size: 12px;font-weight: bold !important;">
                                        Time :</label>
                                    <span id="lbldisptime" style="font-size: 11px;"></span>
                                    <br />
                                    <label style="font-size: 12px;font-weight: bold !important;">
                                       Reverse Charge (Y/N):</label>
                                       <span id="spnreversecharge" style="font-size: 11px;">N</span>
                                  
                                 
                                </td>
                            </tr>
                        </table>
                    </div>
                    </div>

                      <div style="font-family: Arial; font-weight: bold; color: Black; text-align:center; border:2px solid gray;">
                      <br />
                      </div>
                           <div id="div_products" >
                    </div>
                    <br />
                     <br />
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 25%;">
                                                <span style="font-weight: bold; font-size: 12px;">RECEIVER'S SIGNATURE</span>
                                            </td>
                                            <td style="width: 25%;">
                                                <span style="font-weight: bold; font-size: 12px;">SECURITY</span>
                                            </td>
                                            <td style="width: 25%;">
                                                <span id="lbldispat" style="font-weight: bold; font-size: 12px;"></span>
                                            </td>
                                            <td style="width: 25%;">
                                                <span style="font-weight: bold; font-size: 12px;">AUTHORISED SIGNATURE</span>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <br />
                        </div>
                        </div>
                        
                        </div>
                             <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="javascript:CallPrint('divPrint');"><i class="fa fa-print"></i> Print </button>
                        </div>
                        </div>
                        
                        </section>
</asp:Content>


