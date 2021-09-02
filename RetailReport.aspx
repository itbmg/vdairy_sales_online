<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RetailReport.aspx.cs" Inherits="RetailReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
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
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
    </script>
    <script type="text/javascript">
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

        function get_Retail_Report_click() {
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
            var data = { 'operation': 'get_Retail_Report_click', 'fromdate': fromdate, 'todate': todate };
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
            results += '<thead><tr><th scope="col"></th><th scope="col">Ref No</th><th scope="col">OutletName</th><th scope="col">Address</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                //if (status == msg[i].status) {
                results += '<tr><th><input id="btn_Print" type="button"   onclick="printclick(this);"  name="Edit" class="btn btn-primary" value="Print" /></th>'
                results += '<td scope="row" class="1"  style="text-align:center;">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].outletname + '</td>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].address + '</td></tr>';
                // results += '<td data-title="brandstatus" class="2">' + msg[i].expiredate + '</td></tr>';
                //}
            }
            results += '</table></div>';
            $("#divRetailForm").html(results);
        }
        function printclick(thisid) {
            var sno = $(thisid).parent().parent().children('.1').html();
            var data = { 'operation': 'get_Retail_Sub_Report_click', 'sno': sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        $('#divPrint').css('display', 'block');
                        $('#Button2').css('display', 'block');
                        var Retail_Details = msg[0].RetailDetals;
                        var Retail_Sub_Details = msg[0].SubRetailDetals;
                        document.getElementById('spnvendorname').innerHTML = Retail_Details[0].outletname;
//                        document.getElementById('spnAddress').innerHTML = Retail_Details[0].Add_ress;
                        document.getElementById('spnaddress').innerHTML = Retail_Details[0].address;
                        document.getElementById('spnlatitude').innerHTML = Retail_Details[0].lat;
                        document.getElementById('spantin').innerHTML = Retail_Details[0].tinno;
                        document.getElementById('spnlongtitude').innerHTML = Retail_Details[0].lot;
                        fill_Retail_Sub_Details(Retail_Sub_Details);
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
        var ed = 0; var tax = 0; var pf = 0; var disamt = 0;
        function fill_Retail_Sub_Details(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" border="2" style="width:100%;">';
            results += '<thead><tr><th scope="col">Sno</th><th scope="col">ProductName</th><th scope="col">Qty</th></tr></thead></tbody>';
            var j = 1;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr><th scope="row" class="1" style="text-align:center;">' + j + '</th>';
                results += '<td data-title="brandstatus" class="2">' + msg[i].productname + '</td>';
                results += '<td data-title="brandstatus" class="tammountcls">' + msg[i].qty + '</td></tr>'
                j++;
            }
            var t2 = "Total";
            results += '<tr><th scope="row" class="1" style="text-align:center;"></th>';
            results += '<td data-title="brandstatus"  class="6">' + t2 + '</td>';
            results += '<td data-title="brandstatus" class="7"><span id="totalcls"></span></td></tr>';
            results += '</table></div>';
            $("#div_itemdetails").html(results);
            GetTotalCal();
        }
        function GetTotalCal() {
            var totamount = 0;
            $('.tammountcls').each(function (i, obj) {
                var qtyclass = $(this).text();
                if (qtyclass == "" || qtyclass == "0") {
                }
                else {
                    totamount += parseFloat(qtyclass);
                }
            });
            document.getElementById('totalcls').innerHTML = parseFloat(totamount).toFixed(2);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Retail Report<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Reports</a></li>
            <li><a href="#">Retail Report</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Retail Report
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
                                <input id="btn_save" type="button" class="btn btn-primary" name="submit" value='Get Details'
                                    onclick="get_Retail_Report_click()" />
                            </td>
                        </tr>
                    </table>
                    <div id="divRetailForm" style="height: 300px; overflow-y: scroll;">
                    </div>
                </div>
                <div>
                </div>
              <div id="divPrint" style="display: none;">
                <div style="height:1040px;">
                    <div style="width: 13%; float: right;">
                        <img src="Images/Vyshnavilogo.png" alt="VT Milk" width="100px" height="72px" />
                        <br />
                    </div>
                    <div>
                        <div style="font-family: Arial; font-size: 14pt; font-weight: bold; color: Black;">
                            <span>VITA MILK DAIRY PRODUCTS </span>
                            <br />
                        </div>
                        <%--<div style="width:26%;">
                        <span id="spnAddress" style="font-size: 14px;"></span>
                        </div>--%>
                        VYSHNAVI HOUSE<br />
                       no.25,2nd Street,Periyar Nagar,
                        <br />
                       Korattur,Chennai-600080.<br />
                    </div>
                    <div align="center" style="border-bottom: 1px solid gray; border-top: 1px solid gray;">
                        <span style="font-size: 18px; font-weight: bold;">RETAIL & MODERN TRADE OUTLET PROFILE FORM</span>
                    </div>
                    <div style="width: 100%;">
                        <table style="width: 100%;">
                            <tr>
                             
                                <td style="width: 49%; float: left;">
                                <label style="font-size: 12px;" float: left;">
                                        OutlateName :</label>
                                    <span id="spnvendorname"></span>
                                    <br />
                                    <label style="font-size: 12px;">
                                        Address :</label>
                                    <span id="spnaddress" style="font-size: 14px;"></span>
                                    <br />
                                    <label style="font-size: 12px;">
                                        Tin no :</label>
                                    <span id="spantin" style="font-size: 14px;"></span>
                                    <br />
                                </td>
                                <td style="width: 49%; float: right;">
                                    <label style="font-size: 12px;">
                                        Latitude :</label>
                                    <span id="spnlatitude" style="font-size: 14px;"></span>
                                    <br />
                                    <label style="font-size: 12px;">
                                        Longtitude :</label>
                                    <span id="spnlongtitude" style="font-size: 14px;"></span>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div_itemdetails">
                    </div>
                    </div>
                </div>
                <input id="Button2" type="button" class="btn btn-primary" name="submit" style="display:none;" value='Print'
                    onclick="javascript:CallPrint('divPrint');" />
                <asp:Label ID="lblmsg" runat="server" Font-Size="20px" ForeColor="Red" Text=""></asp:Label>
            </div>
            </div>
    </section>
</asp:Content>
