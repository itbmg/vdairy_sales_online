<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RouteWiseClassification.aspx.cs" Inherits="RouteWiseClassification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="JIC/JIC.js?v=101" type="text/javascript"></script>
    <script src="JSF/imagezoom.js" type="text/javascript"></script>
    <script src="Plant/Script/fleetscript.js?v=3005" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            get_branch_Names();
            $('#mydiv').css('display', 'none');
            $('#divclose').css('display', 'none');
            $('#divcontroles').css('display', 'block');
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd
            }
            if (mm < 10) {
                mm = '0' + mm
            }
            var hrs = today.getHours();
            var mnts = today.getMinutes();
            $('#txtfromdate').val(yyyy + '-' + mm + '-' + dd);
        });
        function get_branch_Names() {
            var data = { 'operation': 'GetPlantSalesOffice' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "login.html";
                    }
                    fillbranchnames(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillbranchnames(msg) {
            document.getElementById('ddlbranches').options.length = "";
            var veh = document.getElementById('ddlbranches');
            var length = veh.options.length;
            for (i = length - 1; i >= 0; i--) {
                veh.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select BranchName";
            opt.value = "";
            veh.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    veh.appendChild(opt);
                }
            }
        }
        function get_branch_Names() {
            var data = { 'operation': 'GetPlantSalesOffice' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "login.html";
                    }
                    fillbranchnames(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillbranchnames(msg) {
            document.getElementById('ddlbranches').options.length = "";
            var veh = document.getElementById('ddlbranches');
            var length = veh.options.length;
            for (i = length - 1; i >= 0; i--) {
                veh.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select BranchName";
            opt.value = "";
            veh.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    veh.appendChild(opt);
                }
            }
        }
        function get_Employees() {
        var branchid=document.getElementById('ddlbranches').value;
            var data = { 'operation': 'get_Employees_leveltypes','branchid':branchid };
            var s = function (msg) {
                if (msg) {
                    fillemployees(msg);
                    //                    fillemployees1(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
       function fillemployees(msg) {
            document.getElementById('Slect_EmpName').options.length = "";
            var veh = document.getElementById('Slect_EmpName');
            var length = veh.options.length;
            for (i = length - 1; i >= 0; i--) {
                veh.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Employee";
            opt.value = "";
            veh.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].LevelType;
                    opt.value = msg[i].LevelType;
                    veh.appendChild(opt);
                }
            }
        }
        function getRouteClassification_click() {
            $('#divcontroles').css('display', 'none');
            $('#mydiv').css('display', 'block');
            var fromdate = document.getElementById('txtfromdate').value;
            var branchid = document.getElementById('ddlbranches').value;
            var data = { 'operation': 'getRouteClassification', 'branchid': branchid,'fromdate':fromdate};
            var s = function (msg) {
                if (msg) {
                    fillitems(msg);
                    get_Employees();
                     $('#tbl_message').css('display', 'block');
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
       var routewisearry=[];var salestypearr=[];
          function fillitems(msg) {
           var BranchTable = [];var totsale = 0;var totpaidamount = 0; var grand_totsale = 0;var grand_totpaidamount = 0;
           var totdiff = 0;var grand_totdiff = 0;
           routewisearry=msg[0].RouteClassification;
           salestypearr=msg[0].CategoryClassifications;
              var results = '<div class="divcontainer" style="overflow:auto;"><table border="1" class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
              results += '<thead><tr><th scope="col">RouteName</th><th scope="col">SalesType</th><th scope="col">SaleValue</th><th scope="col">PaidAmount</th><th scope="col">Difference</th></tr></thead></tbody>';
              for (var i = 0; i < routewisearry.length; i++) {
                  if (BranchTable.indexOf(routewisearry[i].RouteName) == -1) {
                      if (i == 0) {
                      }
                      else {
                          results += '<tr>';
                          results += '<td scope="row" class="1" ></td>';
                          results += '<td scope="row" class="1"  style="font-size:18px;font-weight:bold;color:#006400;">Total</td>';
                          results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totsale).toFixed(2) + '</td>';
                          results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totpaidamount).toFixed(2) + '</td>'
                          results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totdiff).toFixed(2) + '</td></tr>';
                      }
                     totsale = 0;
                     totpaidamount=0;
                     totdiff=0;
                      results += '<tr>';
                      results += '<td scope="row" class="1">' + routewisearry[i].RouteName + '</td>';
                       results += '<td  onclick="btnClicksaletypes(\'' + routewisearry[i].salestypeid +'\');"><span style="text-decoration: underline;color:blue;">' + routewisearry[i].SalesType + '</span></td>';
                      results += '<td  class="2">' + routewisearry[i].SaleValue + '</td>';
                      results += '<td  class="2">' + routewisearry[i].PaidAmount + '</td>';
                      results += '<td  class="2">' + routewisearry[i].difference + '</td></tr>';
                      totsale += parseFloat(routewisearry[i].SaleValue);
                      grand_totsale += parseFloat(routewisearry[i].SaleValue);
                      totpaidamount += parseFloat(routewisearry[i].PaidAmount);
                      grand_totpaidamount += parseFloat(routewisearry[i].PaidAmount);
                      totdiff += parseFloat(routewisearry[i].difference);
                      grand_totdiff +=parseFloat(routewisearry[i].difference);;
                      BranchTable.push(routewisearry[i].RouteName);
                  }
                  else {
                      results += '<tr>';
                      results += '<td scope="row" class="1" ></td>';
                      results += '<td  onclick="btnClicksaletypes(\'' + routewisearry[i].salestypeid +'\');"><span style="text-decoration: underline;color:blue;">' + routewisearry[i].SalesType + '</span></td>';
                      results += '<td  class="2">' + routewisearry[i].SaleValue + '</td>';
                      results += '<td  class="2">' + routewisearry[i].PaidAmount + '</td>';
                      results += '<td  class="2">' + routewisearry[i].difference + '</td></tr>';
                      totsale += parseFloat(routewisearry[i].SaleValue);
                      grand_totsale +=parseFloat(routewisearry[i].SaleValue);;
                      totpaidamount += parseFloat(routewisearry[i].PaidAmount);
                      grand_totpaidamount +=  parseFloat(routewisearry[i].PaidAmount);
                      totdiff += parseFloat(routewisearry[i].difference);
                      grand_totdiff +=parseFloat(routewisearry[i].difference);;
                  }
              }
              results += '<tr>';
              results += '<td scope="row" class="1" ></td>';
//              avgfat = totfatqty / totqty;
//              avgfat = parseFloat(avgfat).toFixed(2);
              results += '<td scope="row" class="1" style="font-size:18px;font-weight:bold;color:#006400;" >Total</td>';
              results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totsale).toFixed(2) + '</td>';
              results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totpaidamount).toFixed(2) + '</td>'
              results += '<td  class="2" style="font-size:18px;font-weight:bold;color:#006400;">' + parseFloat(totdiff).toFixed(2) + '</td></tr>';
              results += '<tr>';
              results += '<td scope="row" class="1" ></td>';
              results += '<td scope="row" class="1" style="font-size:22px;font-weight:bold;color:#B22222;" >Grand Total</td>';
              results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;">' + parseFloat(grand_totsale).toFixed(2) + '</td>';
              results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;">' + parseFloat(grand_totpaidamount).toFixed(2) + '</td>';
              results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;">' + parseFloat(grand_totdiff).toFixed(2) + '</td></tr>';
              results += '<tr>';
              results += '<td scope="row" class="1" ></td>';
              results += '<td scope="row" class="1" style="font-size:22px;font-weight:bold;color:#B22222;" ></td>';
              results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;"></td>';
              results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;"></td>';
              results += '<td  class="2" style="font-size:22px;font-weight:bold;color:#B22222;"></tr>';
               for (var i = 0; i < salestypearr.length; i++) {
                      results += '<tr>';
                      results += '<td onclick="btnClicksaletypes(\'' + salestypearr[i].salesbranchid +'\');"><span style="text-decoration: underline;color:blue;">' + salestypearr[i].SalesType + '</span></td>';
                      results += '<td  class="2">' + salestypearr[i].SaleValue + '</td>';
                      results += '<td  class="2">' + salestypearr[i].PaidAmount + '</td>';
                      results += '<td  class="2">' + salestypearr[i].difference + '</td></tr>';
               }
              results += '</table></div>';
              $("#div_BindRoueclassificationdata").html(results);
          }
        function btnClicksaletypes(routesalestype) {
            var routesalestype;
             var fromdate = document.getElementById('txtfromdate').value;
            var data = { 'operation': 'get_categorywiseagentsalepaidamt', 'routesalestype': routesalestype, 'fromdate': fromdate};
            var s = function (msg) {
                if (msg) {
                    if (msg) {
                        $('#divMainAddNewRow').css('display', 'none');
                        fillbranchwiseproductsdata(msg);
                    }
                    else {
                    }
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillbranchwiseproductsdata(msg) {
            $('#divMainAddNewRow').css('display', 'block');
            $('#Show_agentwisesalepaidamt').css('display', 'block');
             $('#tbl_message').css('display', 'block');
            
            j = 1;
            var tSaleValue = 0; var tPaidAmount = 0;var tdifference=0;
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col">RouteName</th><th scope="col">AgentName</th><th scope="col">SRName</th><th scope="col">SalesMan</th><th scope="col">SaleValue</th><th scope="col">PaidAmount</th><th scope="col">Difference</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row">' + msg[i].RouteName + '</td>';
                results += '<td scope="row">' + msg[i].AgentName + '</td>';
                results += '<td scope="row">' + msg[i].SRname + '</td>';
                results += '<td scope="row">' + msg[i].salesman + '</td>';
                results += '<td scope="row" class="2" ><div style="float:right;padding-right: 10%;">' + parseFloat(msg[i].SaleValue)|| 0 + '</div></td>';
                results += '<td data-title="brandstatus"  class="3"><div style="float:right;padding-right: 10%;">' + parseFloat(msg[i].PaidAmount)|| 0 + '</div></td>';
                results += '<td data-title="brandstatus"  class="3"><div style="float:right;padding-right: 10%;">' + parseFloat(msg[i].difference)|| 0 + '</div></td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
                tSaleValue += parseFloat(msg[i].SaleValue) || 0;
                tPaidAmount += parseFloat(msg[i].PaidAmount) || 0;
                tdifference += parseFloat(msg[i].difference) || 0;
            }
            results += '<tr>';
            results += '<td scope="row"></td>';
            results += '<td scope="row"></td>';
            results += '<td scope="row"></td>';
            results += '<td scope="row" class="1">Total</td>';
            results += '<td scope="row" class="1" style="font-size:18px;font-weight:bold;color:#006400;" ><div style="float:right;padding-right: 7%;">' + parseFloat(tSaleValue).toFixed(2)|| 0 + '</div></td>';
            results += '<td   style="font-size:18px;font-weight:bold;color:#006400;"><div style="float:right;padding-right: 7%;">' + parseFloat(tPaidAmount).toFixed(2)|| 0 + '</div></td>';
            results += '<td   style="font-size:18px;font-weight:bold;color:#006400;"><div style="float:right;padding-right: 7%;">' + parseFloat(tdifference).toFixed(2)|| 0 + '</div></td></tr>';
            results += '</table></div>';
            $("#Show_agentwisesalepaidamt").html(results);
        }
        function CloseClick() {
            $('#div_BindRoueclassificationdata').css('display', 'block');
            $('#Show_agentwisesalepaidamt').css('display', 'none');
             $('#divMainAddNewRow').css('display', 'none');
        }
        function btnSendSms_click() {
           var fromdate = document.getElementById('txtfromdate').value;
            var branchid = document.getElementById('ddlbranches').value;
            var leveltype = document.getElementById('Slect_EmpName').value;
            if (!confirm("Do you want to save this transaction")) {
                return false;
            }
            var data = { 'operation': 'SendSms_ToSalesRepresentive','fromdate':fromdate,'branchid':branchid,'leveltype':leveltype};
            var s = function (msg) {
                if (msg) {
                    if (msg) {
                    alert(msg);
                    }
                    else {
                    }
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
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
               Route Classification<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Reports</a></li>
            <li><a href="#"> Route Classification</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i> Route Classification
                </h3>
            </div>
            <div class="box-body">
                <div runat="server" id="d">
                    <table>
                        <tr>
                          <td>
                                        <label>Sales Office</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="ddlbranches" class="form-control">
                                        </select>
                                    </td>
                            <td>
                                <label>
                                    From Date:</label>
                            </td>
                            <td>
                                <input type="date" id="txtfromdate" class="form-control" />
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                            <input type="button" id="txtGenerate" class="btn btn-primary"  value="Generate" onclick="getRouteClassification_click();"/>
                            </td>
                        </tr>
                    </table>
                    <div id="div_BindRoueclassificationdata">
                    </div>
                    <div>
                    <table id="tbl_message" style="display:none;">
                   <tr>
                    <td>
                                        <label>Employee Type</label>
                                    </td>
                                    <td style="height: 40px;">
                                        <select id="Slect_EmpName" class="form-control">
                                        </select>
                                    </td>
                                    <td style="width: 40px;"></td>
                                    <td>
                    <input type="button" id="btnSms" class="btn btn-primary"  value="SendSms" onclick="btnSendSms_click();"/>
                    </td>
                    </tr>
                    </table>
                    </div>
   <div id="divMainAddNewRow" class="pickupclass" style="text-align: center; height: 100%;
            width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
            background: rgba(192, 192, 192, 0.7);">
            <div id="divAddNewRow" style="border: 5px solid #A0A0A0; position: absolute; top: 8%;
                background-color: White; left: 10%; right: 10%; -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                border-radius: 10px 10px 10px 10px;">
                <table align="left" cellpadding="0" cellspacing="0" style="float: left; width: 100%;"
                    id="tableCollectionDetails" class="mainText2" border="1">
                    <tr>
                        <td colspan="2">
                            <div id="Show_agentwisesalepaidamt">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="button" class="btn btn-danger" id="close_vehmaster" value="Close" onclick="CloseClick();" />
                        </td>
                    </tr>
                </table>
            </div>
          <div id="div7" style="width: 35px; top: 7.5%; right: 10%; position: absolute; z-index: 99999;
                cursor: pointer;">
                <img src="Images/Close.png" alt="close" width="100%" height="100%" onclick="CloseClick();" />
            </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
