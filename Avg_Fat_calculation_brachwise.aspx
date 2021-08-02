<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Avg_Fat_calculation_brachwise.aspx.cs" Inherits="Avg_Fat_calculation_brachwise" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="Js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function CallPrint(strid) {
            //            var prtContent = document.getElementById(strid);
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        function BtnGenerateClick() {
            var fromdate = document.getElementById("txtFromDate").value;
            var ToDate = document.getElementById("txtToDate").value;
            if (fromdate == "") {
                alert("Select From Date");
                return false;
            }
            if (ToDate == "") {
                alert("Select To Date");
                return false;
            }
            var data = { 'operation': 'Get_Avg_Fat_calculation_brachwise', 'fromdate': fromdate, 'ToDate': ToDate };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    //                    testarray(msg);
                    Bind_Fat_calculation_brachwise(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var BranchTable = [];

          var totqty = 0;
          var totfatqty = 0;
          var avgfat = 0;

          var grand_totqty = 0;
          var grand_totfatqty = 0;
          var grand_avgfat = 0;

          function Bind_Fat_calculation_brachwise(msg) {
              var results = '<div class="divcontainer" style="overflow:auto;"><table border="1" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
              results += '<thead><tr><th scope="col">Branch Name</th><th scope="col">Product Name</th><th scope="col">Qty</th><th scope="col">FAT</th><th scope="col">FAT Qty</th></tr></thead></tbody>';
              for (var i = 0; i < msg.length; i++) {
                  if (BranchTable.indexOf(msg[i].BranchName) == -1) {
                      if (i == 0) {
                      }
                      else {
                          results += '<tr>';
                          results += '<td scope="row" class="1" ></td>';
                          avgfat = totfatqty / totqty;
                          avgfat = parseFloat(avgfat).toFixed(2);
                          results += '<td scope="row" class="1"  style="font-size:18px;font-weight:bold;color:#006400;">Total</td>';
                          results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totqty).toFixed(2) + '</td>';
                          results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + avgfat + '</td>';
                          results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totfatqty).toFixed(2) + '</td></tr>';
                      }
                      totqty = 0;
                      totfatqty = 0;
                      avgfat = 0;
                      results += '<tr>';
                      results += '<td scope="row" class="1"  style="font-size:18px;font-weight:bold;color:blue;">' + msg[i].BranchName + '</td>';
                      results += '<td scope="row" class="1" >' + msg[i].SubCatName + '</td>';
                      results += '<td  class="2">' + msg[i].Qty + '</td>';
                      results += '<td  class="2">' + msg[i].Fat + '</td>';
                      results += '<td  class="2">' + msg[i].fatQty + '</td></tr>';
                      totqty += parseFloat(msg[i].Qty);
                      totfatqty += parseFloat(msg[i].fatQty);
                      grand_totqty += totqty;
                      grand_totfatqty += totfatqty;
                      BranchTable.push(msg[i].BranchName);
                  }
                  else {
                      results += '<tr>';
                      results += '<td scope="row" class="1" ></td>';
                      results += '<td scope="row" class="1" >' + msg[i].SubCatName + '</td>';
                      results += '<td  class="2">' + msg[i].Qty + '</td>';
                      results += '<td  class="2">' + msg[i].Fat + '</td>';
                      results += '<td  class="2">' + msg[i].fatQty + '</td></tr>';
                      totqty += parseFloat(msg[i].Qty);
                      totfatqty += parseFloat(msg[i].fatQty);
                      grand_totqty += totqty;
                      grand_totfatqty += totfatqty;
                  }
              }
              results += '<tr>';
              results += '<td scope="row" class="1" ></td>';
              avgfat = totfatqty / totqty;
              avgfat = parseFloat(avgfat).toFixed(2);
              results += '<td scope="row" class="1" style="font-size:18px;font-weight:bold;color:#006400;" >Total</td>';
              results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totqty).toFixed(2) + '</td>';
              results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + avgfat + '</td>';
              results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totfatqty).toFixed(2) + '</td></tr>';

              results += '<tr>';
              results += '<td scope="row" class="1" ></td>';
              grand_avgfat = grand_totfatqty / grand_totqty;
              grand_avgfat = parseFloat(grand_avgfat).toFixed(2);
              results += '<td scope="row" class="1" style="font-size:22px;font-weight:bold;color:#B22222;" >Grand Total</td>';
              results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;">' + parseFloat(grand_totqty).toFixed(2) + '</td>';
              results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;">' + grand_avgfat + '</td>';
              results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;">' + parseFloat(grand_totfatqty).toFixed(2) + '</td></tr>';
              results += '</table></div>';
              $("#div_Deptdata").html(results);
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
            Avg FAT Calculation<small>Preview</small>
        </h1>
        <ol class="breadcrumb" >
            <li><a href="#">Avg FAT Calculation</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Avg FAT Calculation Details
                </h3>
            </div>
            <div class="box-body">
                <br />
                <table align="center">
                <tr>
                  <td>
                                    From Date
                                </td>
                                <td>
                                    <input type="date" id="txtFromDate" class="formDate" />
                                </td>
                                <td>
                                    To Date
                                </td>
                                <td>
                                    <input type="date" id="txtToDate" class="formDate" />
                                </td>
                                <td  style="width:6px;"></td>
                                <td> 
                                    <input type="button" id="btnGeneretae" value="Generate" onclick="BtnGenerateClick();"
                                        class="btn btn-primary" />
                                </td>
                </tr>
                </table>
                <br />
                  <div id="divPrint">
                 <div id="div_Deptdata"></div>
                </div>
                <br />
                 <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="javascript:CallPrint('divPrint');"><i class="fa fa-print"></i> Print </button>
                </div>
                
                </div>
                </section>
</asp:Content>
