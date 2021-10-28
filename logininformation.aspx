<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="logininformation.aspx.cs" Inherits="logininformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="Js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <link href="Css/style.css" rel="stylesheet" type="text/css" />
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3004" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
 <link href="autocomplete/jquery-ui.css?v=3002" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $('#div_loginreport').css('display', 'block');
//            $('#div_livelogin').css('display', 'none');
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
            $('#txt_date').val(yyyy + '-' + mm + '-' + dd);
            $('#txt_fdate').val(yyyy + '-' + mm + '-' + dd);
            $('#txt_tdate').val(yyyy + '-' + mm + '-' + dd);
        });
        function showloginreport() {
            $('#div_loginreport').css('display', 'block');
//            $('#div_livelogin').css('display', 'none');
        }
//        function showlivelogin() {
//            $('#div_loginreport').css('display', 'none');
////            $('#div_livelogin').css('display', 'block');
////            get_livelogin_details();
//        }
        function showemplyeewise() {
            $('#showlogs_rptdetails').css('display', 'block');
            $('#showlogs_date').css('display', 'none');
            get_employee_details();
        }
        function showdatewise() {
            $('#showlogs_date').css('display', 'block');
            $('#showlogs_rptdetails').css('display', 'none');
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
        function CallHandlerUsingJson(d, s, e) {
            $.ajax({
                type: "GET",
                url: "DairyFleet.axd?json=",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(d),
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }
        var employeenames = [];
        function get_employee_details() {
            var data = { 'operation': 'get_employee_details' };
            var s = function (msg) {
                if (msg) {
                    employeenames = msg;
                    var availableTags = [];
                    for (var i = 0; i < msg.length; i++) {
                        var employeename = msg[i].employeename;
                        availableTags.push(employeename);
                    }
                    $('#txt_employeename').autocomplete({
                        source: availableTags,
                        change: empnamechange,
                        autoFocus: true
                    });
                }
            }
            var e = function (x, h, e) {
                alert(e.toString());
            };
            callHandler(data, s, e);
        }
        function empnamechange() {
            var name = document.getElementById('txt_employeename').value;
            for (var i = 0; i < employeenames.length; i++) {
                if (name == employeenames[i].employeename) {
                    document.getElementById('txt_employeeid').value = employeenames[i].sno;
                }
            }
        }
        function btn_getlogininfoemployee_details() {
            var employeeid = document.getElementById('txt_employeeid').value;
            var fromdate = document.getElementById('txt_fdate').value;
            var todate = document.getElementById('txt_tdate').value;
            if (employeeid == "") {
                alert("Please select Employee Name");
                return false;
            }
            if (fromdate == "") {
                alert("Please select from date");
                return false;
            }
            if (todate == "") {
                alert("Please select to date");
                return false;
            }
            var data = { 'operation': 'btn_getlogininfoemployee_details', 'employeeid': employeeid, 'fromdate': fromdate, 'todate': todate };
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
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function btn_getlogininfodatewise_details() {
            var date = document.getElementById('txt_date').value;
            if (date == "") {
                alert("Please select to date");
                return false;
            }
            var data = { 'operation': 'btn_getlogininfoemployee_details', 'date': date };
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
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        
        function filldetails(msg) {
            $('#get_logreport').css("display", "block");
            $('#div_print').css("display", "none");
            var emptable = [];
            var results = '<div    style="overflow:auto;"><table class="table table-bordered table-hover dataTable" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr role="row" style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col" style="font-weight: bold;">Employee Name</th><th scope="col" style="font-weight: bold;">IpAddress</th><th scope="col" style="font-weight: bold;">DeviceType</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = [""];
            for (var i = 0; i < msg.length; i++) {
                var userid = msg[i].sno
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td data-title="Capacity" class="1"  onclick="logsclick(\'' + userid + '\');" title="Click here to all login details"><i class="fa fa-arrow-circle-right" title="Click Here" aria-hidden="true">&nbsp;&nbsp;&nbsp;&nbsp;' + msg[i].employeename + '</td>';
                results += '<td  class="4" >' + msg[i].ipaddress + '</td>';
                results += '<td  class="5" >' + msg[i].devicetype + '</td></tr>';
            }
            results += '</table></div>';
            $("#get_logreport").html(results);
        }
        function get_radio_value() {
            var inputs = document.getElementsByName("selected");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].checked) {
                    return inputs[i].value;
                }
            }
        }
        function logsclick(userid) {
            var fromdate = document.getElementById('txt_fdate').value;
            var todate = document.getElementById('txt_tdate').value;
            var date = document.getElementById('txt_date').value;
            var session = get_radio_value();
            var userid = userid;
            var data = { 'operation': 'get_logindetails_eachemployee', 'userid': userid, 'fromdate': fromdate, 'todate': todate, 'date': date, 'session': session };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillloginemployeedetails(msg);
                    }
                    else {
                        msg = 0;
                        fillloginemployeedetails(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillloginemployeedetails(msg) {
            //$("#myModal").css("display", "block");
            $('#get_logreport').css("display", "none");
            $('#div_print').css("display", "block");
            var myString = document.getElementById('txt_fdate').value; //xml nodeValue from time element
            var array = new Array();
            array = myString.split('-');
            document.getElementById('lblFromDate').innerHTML = (array[2] + "-" + array[1] + "-" + array[0]);

            var myString1 = document.getElementById('txt_tdate').value; //xml nodeValue from time element
            var array1 = new Array();
            array1 = myString1.split('-');
            document.getElementById('lbltodate').innerHTML = (array1[2] + "-" + array1[1] + "-" + array1[0]);

            var empname = msg[0].employeename;
            $('#lblName').text(empname);

            var COLOR = [""];
            var k = 1;
            var l = 0;
            var results = '<div><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col">Sno</th><th scope="col" style="text-align:center">Login Date</th><th scope="col">Login Time</th><th scope="col">Logout Time</th><th scope="col" style="text-align:center">Timeinterval</th><th scope="col">IpAddress</th><th scope="col">Device</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '"><td >' + k++ + '</td>';
                results += '<td  class="4" style="text-align:center;">' + msg[i].indate + '</td>';
                results += '<td  class="4" style="text-align:center;">' + msg[i].intime + '</td>';
                if (msg[i].outtime != "" || msg[i].outtime != null || msg[i].outtime != "null") {
                    results += '<td  class="4" style="text-align:center;">' + msg[i].outtime + '</td>';
                }
                else {
                    results += '<td  class="4" style="text-align:center;"></td>';
                }
                if (msg[i].outtime != "" || msg[i].outtime != null || msg[i].outtime != "null") {
                    results += '<td  class="4" style="text-align:center;">' + msg[i].timeinterval + '</td>';
                }
                else {
                    results += '<td  class="4" style="text-align:center;"></td>';
                }
                results += '<td  class="4" style="text-align:center;">' + msg[i].ipaddress + '</td>';
                results += '<td  class="5" style="text-align:center;">' + msg[i].devicetype + '</td>';
                results += '</tr>';
            }
            results += '</table></div>';
            $("#div_loginsget").html(results);
        }
        function btn_getlogininfodatewise_close() {
            $('#get_logreport').css("display", "block");
            $('#div_print').css("display", "none");
        }
        function closepopup(msg) {
            $("#myModal").css("display", "none");
        }
        
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            <i class="fa fa-files-o" aria-hidden="true"></i>Login Information<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Login Information Details</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
           
            <div id="div_loginreport">
            <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Login Report Details
                </h3>
            </div>
             <div class="box-body">
                <div id="fill_loginrpt" style="text-align: -webkit-center;">
                <table>
                    <tr>
                        <td>
                             <input id="rdolst_0" type="radio" name="selected" value="empwise"  name="selected" onclick="showemplyeewise();" />
                                                Employee Wise
                        </td>
                        <td   style="padding-left: 10%;">
                        </td>
                        <td>
                            <input id="rdolst_1" type="radio" name="selected" value="datewise"  name="selected" onclick="showdatewise();" />
                                                Date Wise
                        </td>
                    </tr>
                 </table>
                </div>
                  <div id="showlogs_rptdetails" style="display: none;text-align: -webkit-center;padding-top: 2%;padding-bottom: 2%;">
                    <table>
                        <tr>
                          <td>
                                <label>
                                 Employee Name : </label>
                            </td>
                            <td>
                              <input id="txt_employeename" type="text" class="form-control" name="branch_search" onchange="empnamechange();" placeholder="Enter Employee  Name">
                              <input id="txt_employeeid" type="text" style="display:none" class="form-control" name="employeeid" />
                            </td>
                            <td>
                                <label>
                                 From Date : </label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txt_fdate" class="form-control" type="date" name="fromdate"/>
                            </td>
                            <td>
                                <label>
                                 To Date : </label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txt_tdate" class="form-control" type="date" name="todate"/>
                            </td>
                            
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <div class="input-group">
                                    <div class="input-group-addon">
                                        <span class="glyphicon glyphicon-flash"></span> <span id="btn_save" onclick="btn_getlogininfoemployee_details();">Get Details</span>
                                    </div>
                                </div>
                            </td>
                     </tr>
                    </table>
                </div>
                <div id="showlogs_date"  style="display:none;text-align: -webkit-center;padding-top: 2%;padding-bottom: 2%;"> 
                  <table>
                    <tr>
                         <td>
                                <label>
                                 Date : </label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txt_date" class="form-control" type="date" name="date"/>
                            </td>
                             <td style="width: 5px;">
                            </td>
                            <td>
                                <div class="input-group">
                                    <div class="input-group-addon">
                                        <span class="glyphicon glyphicon-flash"></span> <span id="Span1" onclick="btn_getlogininfodatewise_details();">Get Details</span>
                                    </div>
                                </div>
                            </td>
                    </tr>
                  </table>
                </div>
                <div id="get_logreport">
                </div>
                <div style="width: 100%;display:none" id="div_print" >
                    <div style="width: 13%; float: left;">
                        <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="100px" height="72px" />
                    </div>
                    <div align="center">
                        <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="20px" ForeColor="#0252aa"
                            Text="">Sri Vyshnavi Dairy Specialities (P) Ltd </asp:Label>
                        <br />
                        <asp:Label ID="lblAddress" runat="server" Font-Bold="true" Font-Size="12px" ForeColor="#0252aa"
                            Text="">R.S.No:381/2, Punabaka village Post, Pellakuru Mandal, Nellore District -524129., Andhra Pradesh(State). Phone: 9440622077, Fax: 044 – 26177799. </asp:Label>
                        <br />
                        <span style="font-size: 18px; font-weight: bold; color: #0252aa;">Login Information Report : </span>
                         <br />
                    </div>
                     <table style="width: 80%">
                        <tr>
                            <td>
                               <label id="lblName"  style="color: red;"></label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                From Date : 
                            </td>
                            <td>
                                <span id="lblFromDate"  style="color: red;"></span>
                            </td>
                            <td>
                                To date : 
                            </td>
                            <td>
                                <span id="lbltodate" style="color: red;"></span>
                            </td>
                        </tr>
                    </table>
                 <div id="div_loginsget">
                </div>
                <br />
                <div id="Div1"  style="text-align: -webkit-right;padding-top: 2%;padding-bottom: 2%;"> 
                  <table>
                    <tr>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                    <span class="glyphicon glyphicon-print"></span> <span id="btnPrint" onclick="javascript:CallPrint('div_print');">Print</span>
                                </div>
                            </div>
                        </td>
                        <td style="width: 5px;">
                        </td>
                        <td>
                            <div class="input-group">
                                <div class="input-group-addon">
                                    <span class="glyphicon glyphicon-remove"></span> <span id="Span2" onclick="btn_getlogininfodatewise_close();">Close</span>
                                </div>
                            </div>
                        </td>
                    </tr>
                  </table>
                </div>
             </div>
            </div>
            </div>
            </div>
           
        </div>
       
    </section>
</asp:Content>
