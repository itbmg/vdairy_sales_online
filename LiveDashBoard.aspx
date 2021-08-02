<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="LiveDashBoard.aspx.cs" Inherits="LiveDashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script src="js/jquery-1.4.4.js?v=3004" type="text/javascript"></script>
    <script src="js/newjs/jquery.js?v=3004" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3004" type="text/javascript"></script>
    <link href="jquery.jqGrid-4.5.2/js/i18n/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <script src="jquery.jqGrid-4.5.2/src/i18n/grid.locale-en.js" type="text/javascript"></script>
    <script src="jquery.jqGrid-4.5.2/js/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script src="jquery-ui-1.10.3.custom/js/jquery-ui.js" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js?v=3004" type="text/javascript"></script>
    <script src="js/newjs/jquery-ui.js?v=3004" type="text/javascript"></script>
    <script src="js/JTemplate.js" type="text/javascript"></script>
    <link href="bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap/RouteWiseTimeLine.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            SalesReportsclick();
            get_lastweekpossalevalue_details();
            get_currentweekpossalevalue_details();
        });
    function SalesReportsclick() {
          
            var RouteName = "";
            var startDate = "";
            var endDate = "";
//            var startDate = $('#reportrange').data('daterangepicker').startDate.toString();
//            var endDate = $('#reportrange').data('daterangepicker').endDate.toString();
//            branchname = document.getElementById('ddlsalesoffice').value;
           
//            if (branchname == "Select branchname" || branchname == "") {
//                alert("Please Select branchname");
//                return false;
//            }
            var data = { 'operation': 'GetSalevlue' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillweeksales(msg);
//                        $('#mydiv').css('display', 'none');
//                        $('#divclose').css('display', 'block');
//                        $('#tbl_sale_value').css('display', 'block');
//                        $('#divtotal').css('display', 'block');
                    }
                    else {
                        alert("No data found");
//                        $('#mydiv').css('display', 'none');
//                        $('#divclose').css('display', 'none');
//                        $('#divcontrols').css('display', 'block');
//                        $('#divtotal').css('display', 'none');
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
        var rweekTotalDate = []; var rweekattendancearry = []; var rweektotattendance = []; var rweekemptytable4 = [];
        function fillweeksales(msg) {
            rweekTotalDate = msg[0].wweekcls;
            rweektotattendance = msg[0].SalelDetails;
            var results = '<div class="box-body"><table id="tblbiologs" style="font-weight: 800 !important;" class="table table-bordered  dataTable no-footer" role="grid" aria-="" describedby="example2_info">';
            results += '<thead><tr role="row" class="trbgclrcls">';
            results += '<th scope="col" style="text-align:left;display:none;"> Sno</th>';
            results += '<th scope="col" style="text-align:left;display:none;"><i class="fa fa-user" aria-hidden="true"></i> Parlor Id</th>';
            results += '<th scope="col" style="text-align:left;"><i class="fa fa-user" aria-hidden="true"></i>BranchName</th>';
            for (var i = 0; i < rweekTotalDate.length; i++) {
                var dta = rweekTotalDate[i].week;
                var mysplit = dta.split("__");
                var dm = mysplit[0];
                var day = mysplit[1]
                var day2 = mysplit[2]
                // W1_01 / Jan_08 / Jan
                results += '<th scope="col" id="txtDate"><i class="fa fa-calendar" aria-hidden="true"></i> ' + dm + ' </br></th>';
            }
            results += '</tr></thead></tbody>';
            for (var i = 0; i < rweektotattendance.length; i++) {
                var k = i + 1;
                var totalval = 0;
                results += '<tr><td scope="row" style="text-align: left; font-weight: bold; display:none;">' + k + '</td>';
                results += '<td scope="row" class="1" style="text-align: left; font-weight: bold; display:none;" >' + rweektotattendance[i].Branchid + '</td>';
                results += '<td scope="row" class="2" style="text-align: left; font-weight: bold;" >' + rweektotattendance[i].BranchName + '</td>';

                var week1 = rweektotattendance[i].week1;
                if (week1 == "") {
                    week1 = "0";
                }
                var week2 = rweektotattendance[i].week2;
                if (week2 == "") {
                    week2 = "0";
                }
                var week3 = rweektotattendance[i].week3;
                if (week3 == "") {
                    week3 = "0";
                }
                var week4 = rweektotattendance[i].week4;
                if (week4 == "") {
                    week4 = "0";
                }
                results += '<td scope="row" onmouseover="ChangeBackgroundColor(this)" onmouseout="RestoreBackgroundColor(this)" class="3" style="text-align: right; font-weight: bold;"><a id="' + rweektotattendance[i].idate1 + '"  onclick="rweeklogsclick1(this);" title="' + rweektotattendance[i].idate1 + '" data-toggle="modal" data-target="" style="cursor: pointer !important;">' + week1 + '</a></td>';
                results += '<td scope="row" onmouseover="ChangeBackgroundColor(this)" onmouseout="RestoreBackgroundColor(this)" class="3" style="text-align: right; font-weight: bold;"><a id="' + rweektotattendance[i].idate2 + '"  onclick="rweeklogsclick2(this);" title="' + rweektotattendance[i].idate2 + '" data-toggle="modal" data-target="" style="cursor: pointer !important;">' + week2 + '</a></td>';
                results += '<td scope="row" onmouseover="ChangeBackgroundColor(this)" onmouseout="RestoreBackgroundColor(this)" class="3" style="text-align: right; font-weight: bold;"><a id="' + rweektotattendance[i].idate3 + '"  onclick="rweeklogsclick3(this);" title="' + rweektotattendance[i].idate3 + '" data-toggle="modal" data-target="" style="cursor: pointer !important;">' + week3 + '</a></td>';
                results += '<td scope="row" onmouseover="ChangeBackgroundColor(this)" onmouseout="RestoreBackgroundColor(this)" class="3" style="text-align: right; font-weight: bold;"><a id="' + rweektotattendance[i].idate4 + '"  onclick="rweeklogsclick4(this);" title="' + rweektotattendance[i].idate4 + '" data-toggle="modal" data-target="" style="cursor: pointer !important;">' + week4 + '</a></td>';
                totalval = parseFloat(rweektotattendance[i].week1) + parseFloat(rweektotattendance[i].week2) + parseFloat(rweektotattendance[i].week3) + parseFloat(rweektotattendance[i].week4)
                //                results += '<td style="" class="10">' + parseFloat(totalval) + '</td>';
                results += '<td style="display:none" class="11">' + "8009" + '</td></tr>';
            }
            results += '</table></div>';
            $("#divrsaledata").html(results);
        }

        

        function get_currentweekpossalevalue_details() {
            var data = { 'operation': 'get_currentweekpossalevalue_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        currentBindsalvalueGrid(msg);
                    }
                    else {
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        var outward = [];
        var currentweekTotalDate = []; var currentweekattendancearry = []; var currentweektotattendance = []; var currentweekemptytable4 = [];
        function currentBindsalvalueGrid(msg) {
//            lastweekTotalDate = msg[0].wweekcls;
//            lastweektotattendance = msg[0].SalelDetails;
            currentweekTotalDate = msg[0].wweekcls;
            currentweektotattendance = msg[0].SalelDetails;
            var results = '<div class="box-body" style="padding:0px !important;"><table id="posTable" style="font-weight: 800 !important; text-align: right;" class="table table-bordered  dataTable no-footer" role="grid" aria-="" describedby="example2_info">';
            results += '<thead><tr role="row" class="trbgclrcls">';
            results += '<th scope="col" style="text-align:left;width: 150px;display:none"><i class="fa fa-user" aria-hidden="true"></i>BranchName</th>';
            for (var i = 0; i < currentweekTotalDate.length; i++) {
                var dta = currentweekTotalDate[i].week;
                var mysplit = dta.split("(");
                var dm = mysplit[0];
                var day = mysplit[1]
                results += '<th scope="col" id="txtDate" style=""><i class="fa fa-calendar" aria-hidden="true"></i> ' + dm + ' </br> (' + day + '</th>';
            }
            results += '<th scope="col" style="text-align:left;"> Total</th>';
            results += '</tr></thead></tbody>';
            var totalpossalevalue = 0;
            for (var i = 0; i < currentweektotattendance.length; i++) {
                results += '<tr>';
                var parlorname = currentweektotattendance[i].BranchName
                if (currentweekemptytable4.indexOf(parlorname) == -1) {
                    results += '<td data-title="brandstatus" class="4" style="text-align:left;display:none;">' + currentweektotattendance[i].BranchName + '</td>';
                    results += '<td style="display:none" data-title="brandstatus" class="3">' + currentweektotattendance[i].Branchid + '</td>';
                    currentweekemptytable4.push(parlorname);
                    var totalbranchvalue = 0;
                    for (var j = 0; j < currentweekTotalDate.length; j++) {
                        var d = 1;
                        var prevparlorname = "";
                        for (var k = 0; k < currentweektotattendance.length; k++) {
                            if (currentweekTotalDate[j].week == currentweektotattendance[k].idate && parlorname == currentweektotattendance[k].BranchName) {
                                if (currentweektotattendance[k].salevalue == "") {
                                    currentweektotattendance[k].salevalue = "0";
                                }
                                if (currentweektotattendance[k].salevalue != "") {
                                    var st = currentweektotattendance[k].Branchid + '-' + currentweektotattendance[k].idate;
                                    //20/Jan(Sunday)
                                    var logdt = currentweektotattendance[k].idate;
                                    var mysplit = logdt.split("(");
                                    var sp = mysplit[0];
                                    var res = logdt.split("/");
                                    var mnth = res[1];
                                    var res2 = mnth.split("(");
                                    var mnth2 = res2[1];
                                   // var cmpid = currentweektotattendance[k].cmpid;
                                    results += '<td class="1" style="display:none"><input class="form-control" type="text" name="empid" id="txtempid"  value="' + st + '"/></td>';
                                    if (mnth2 == "Sunday)" || sp == "26/Jan") {
                                        results += '<td onmouseover="ChangeBackgroundColor(this)" onmouseout="RestoreBackgroundColor(this)"  id="' + st + '" data-title="brandstatus" class="1"><a id="' + st + '"  onclick="logsclick(this);" title="' + st + '" data-toggle="modal" data-target="#myModal" style="color: #126b16 !important; cursor: pointer !important;"><span id="spnsalevalue">' + currentweektotattendance[k].salevalue + '</span></a></td>';
                                    }
                                    else {
                                        results += '<td onmouseover="ChangeBackgroundColor(this)" onmouseout="RestoreBackgroundColor(this)"  id="' + st + '" data-title="brandstatus" class="1"><a id="' + st + '"  onclick="logsclick(this);" title="' + st + '" data-toggle="modal" data-target="#myModal" style="color: #126b16 !important; cursor: pointer !important;"><span id="spnsalevalue">' + currentweektotattendance[k].salevalue + '</span></a></td>';
                                    }
                                    totalbranchvalue += parseFloat(currentweektotattendance[k].salevalue);
                                    results += '<td style="display:none" data-title="brandstatus" class="2">' + currentweektotattendance[k].idate + '</td>';
                                    results += '<td style="display:none" data-title="brandstatus" class="2"><span id="spnparlorid">' + currentweektotattendance[k].Branchid + '</span></td>';
                                    results += '<td style="display:none" data-title="brandstatus" class="clsmoniterqty">' + currentweektotattendance[k].salevalue + '</td>';
                                    d = 0;
                                }
                                else {

                                }
                            }
                            else {

                            }

                        }

                        if (d == 1) {
                            var status = "A"
                            st = 1 + '-' + 1;
                            results += '<td class="1" style="display:none"><input class="form-control" type="text" name="empid" id="txtempid"  value="' + st + '"/></td>';
                            results += '<td id="' + st + '" data-title="brandstatus" class="1"><a id="' + st + '"   title="' + st + '" data-toggle="modal" data-target="#myModals">0</a></td>';
                            results += '<td style="display:none" data-title="brandstatus" class="2"></td>';
                        }
                    }
                    results += '<td style="color: #009270;" data-title="brandstatus" class="clsmoniterqty">' + totalbranchvalue + '</td>';
                    results += '</tr>';
                }
            }
            results += '</table></div>';
            $("#divsaleval").html(results);
            currentweekemptytable4 = [];
            //GetTotalCal3();
        }



        function rweeklogsclick4(thisid) {
            var parlorid = $(thisid).attr('title');
            var data = { 'operation': 'get_weekpossalevalue_details', 'parlorid': parlorid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        rfillweekdatevalue(msg)
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

        function rweeklogsclick3(thisid) {
            var parlorid = $(thisid).attr('title');
            var data = { 'operation': 'get_weekpossalevalue_details', 'parlorid': parlorid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        rfillweekdatevalue(msg)
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

        function rweeklogsclick2(thisid) {
            var parlorid = $(thisid).attr('title');
            var data = { 'operation': 'get_weekpossalevalue_details', 'parlorid': parlorid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        rfillweekdatevalue(msg)
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

        function rweeklogsclick1(thisid) {
            var parlorid = $(thisid).attr('title');
            var data = { 'operation': 'get_weekpossalevalue_details', 'parlorid': parlorid };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        rfillweekdatevalue(msg)
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

        var TotalDate = []; var attendancearry = []; var totattendance = []; var emptytable4 = [];
        var emptytable5 = [];
        function rfillweekdatevalue(msg) {
            TotalDate = msg[0].wweekcls;
            totattendance = msg[0].SalelDetails;
            var results = '<div class="box-body" style="padding:0px !important;"><table id="tblbiologs" style="font-weight: 800 !important;" class="table table-bordered  dataTable no-footer" role="grid" aria-="" describedby="example2_info">';
            results += '<thead><tr role="row" class="trbgclrcls">';
            results += '<th scope="col" style="text-align:left;width: 150px;"><i class="fa fa-user" aria-hidden="true"></i>BranchName</th>';
            for (var i = 0; i < TotalDate.length; i++) {
                var dta = TotalDate[i].week;
                var mysplit = dta.split("(");
                var dm = mysplit[0];
                var day = mysplit[1]
                results += '<th scope="col" id="txtDate" style="width: 59px;"><i class="fa fa-calendar" aria-hidden="true"></i> ' + dm + ' </br> (' + day + '</th>';
            }
            results += '</tr></thead></tbody>';
            for (var i = 0; i < totattendance.length; i++) {
                results += '<tr>';
                var parlorname = totattendance[i].BranchName
                if (emptytable5.indexOf(parlorname) == -1) {
                    results += '<td data-title="brandstatus" class="4">' + totattendance[i].BranchName + '</td>';
                    results += '<td style="display:none" data-title="brandstatus" class="3">' + totattendance[i].Branchid + '</td>';
                    emptytable5.push(parlorname);
                    for (var j = 0; j < TotalDate.length; j++) {
                        var d = 1;
                        for (var k = 0; k < totattendance.length; k++) {
                            if (TotalDate[j].week == totattendance[k].idate && parlorname == totattendance[k].BranchName) {
                                if (totattendance[k].salevalue != "") {
                                    var st = totattendance[k].Branchid + '-' + totattendance[k].idate;
                                    //20/Jan(Sunday)
                                    var logdt = totattendance[k].idate;
                                    var res = logdt.split("/");
                                    var mnth = res[1];
                                    var res2 = mnth.split("(");
                                    var mnth2 = res2[1];
                                    results += '<td class="1" style="display:none"><input class="form-control" type="text" name="empid" id="txtempid"  value="' + st + '"/></td>';
                                    if (mnth2 == "Sunday)") {
                                        results += '<td style="text-align:right;" onmouseover="ChangeBackgroundColor(this)" onmouseout="RestoreBackgroundColor(this)"  id="' + st + '" data-title="brandstatus" class="1"><a id="' + st + '"  onclick="WEEKDATElogsclick(this);" title="' + st + '" data-toggle="modal" data-target="#divws" style="color: brown; cursor: pointer !important;">' + totattendance[k].salevalue + '</a></td>';
                                    }
                                    else {
                                        results += '<td style="text-align:right;" onmouseover="ChangeBackgroundColor(this)" onmouseout="RestoreBackgroundColor(this)"  id="' + st + '" data-title="brandstatus" class="1"><a id="' + st + '"  onclick="WEEKDATElogsclick(this);" title="' + st + '" data-toggle="modal" data-target="#divws" style="cursor: pointer !important;">' + totattendance[k].salevalue + '</a></td>';
                                    }
                                    results += '<td style="display:none" data-title="brandstatus" class="2">' + totattendance[k].idate + '</td>';
                                    d = 0;
                                }
                            }
                        }
                        if (d == 1) {
                            var status = "A"
                            st = 1 + '-' + 1;
                            results += '<td class="1" style="display:none"><input class="form-control" type="text" name="empid" id="txtempid"  value="' + st + '"/></td>';
                            results += '<td id="' + st + '" data-title="brandstatus" class="1"><a id="' + st + '"   title="' + st + '" data-toggle="modal" data-target="#myModals">0</a></td>';
                            results += '<td style="display:none" data-title="brandstatus" class="2"></td>';
                        }
                    }
                    results += '</tr>';
                }
            }
            results += '</table></div>';
            $("#divsaleval").html(results);
            emptytable5 = []
        }



        function get_lastweekpossalevalue_details() {
            var data = { 'operation': 'GetLastweekSalesDetails' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        lastweeksale(msg);
                    }
                    else {
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var outward = [];
        var lastweekTotalDate = []; var lastweekattendancearry = []; var lastweektotattendance = []; var lastweekemptytable4 = [];
        function lastweeksale(msg) {
            lastweekTotalDate = msg[0].wweekcls;
            lastweektotattendance = msg[0].SalelDetails;
            var results = '<div class="box-body" style="padding:0px !important;"><table id="posTable" style="font-weight: 800 !important; text-align: right;" class="table table-bordered  dataTable no-footer" role="grid" aria-="" describedby="example2_info">';
            results += '<thead><tr role="row" class="trbgclrcls">';
            results += '<th scope="col" style="text-align:left;width: 150px;"><i class="fa fa-user" aria-hidden="true"></i>BranchName</th>';
            for (var i = 0; i < lastweekTotalDate.length; i++) {
                var dta = lastweekTotalDate[i].week;
                var mysplit = dta.split("(");
                var dm = mysplit[0];
                var day = mysplit[1]
                results += '<th scope="col" id="txtDate" style="width: 59px;"><i class="fa fa-calendar" aria-hidden="true"></i> ' + dm + ' </br> (' + day + '</th>';
            }

            results += '</tr></thead></tbody>';
            var totalpossalevalue = 0;
            for (var i = 0; i < lastweektotattendance.length; i++) {
                results += '<tr>';
                var parlorname = lastweektotattendance[i].BranchName;
                if (lastweekemptytable4.indexOf(parlorname) == -1) {

                    results += '<td data-title="brandstatus"  onclick="btnPlantClick(\'' + lastweektotattendance[i].Branchid + '\');" class="4" style="text-align:left;">' + lastweektotattendance[i].BranchName + '</td>';
                    results += '<td style="display:none" data-title="brandstatus" class="3">' + lastweektotattendance[i].Branchid + '</td>';
                    lastweekemptytable4.push(parlorname);
                    for (var j = 0; j < lastweekTotalDate.length; j++) {
                        var d = 1;
                        for (var k = 0; k < lastweektotattendance.length; k++) {
                            if (lastweekTotalDate[j].week == lastweektotattendance[k].idate && parlorname == lastweektotattendance[k].BranchName) {
                                if (lastweektotattendance[k].salevalue != "") {
                                    var st = lastweektotattendance[k].Branchid + '-' + lastweektotattendance[k].idate;
                                    //20/Jan(Sunday)
                                    var logdt = lastweektotattendance[k].idate;
                                    var mysplit = logdt.split("(");
                                    var sp = mysplit[0];
                                    var res = logdt.split("/");
                                    var mnth = res[1];
                                    var res2 = mnth.split("(");
                                    var mnth2 = res2[1];
                                    // var cmpid = lastweektotattendance[k].cmpid;
                                    results += '<td class="1" style="display:none"><input class="form-control" type="text" name="empid" id="txtempid"  value="' + st + '"/></td>';
                                    if (mnth2 == "Sunday)" || sp == "26/Jan") {
                                        results += '<td onmouseover="ChangeBackgroundColor(this)" onmouseout="RestoreBackgroundColor(this)"  id="' + st + '" data-title="brandstatus" class="1"><a id="' + st + '"  onclick="logsclick(this);" title="' + st + '" data-toggle="modal" data-target="#myModal" style="color: brown; cursor: pointer !important;"><span id="spnsalevalue">' + lastweektotattendance[k].salevalue + '</span></a></td>';
                                    }
                                    else {
                                        results += '<td onmouseover="ChangeBackgroundColor(this)" onmouseout="RestoreBackgroundColor(this)"  id="' + st + '" data-title="brandstatus" class="1"><a id="' + st + '"  onclick="logsclick(this);" title="' + st + '" data-toggle="modal" data-target="#myModal" style="cursor: pointer !important;"><span id="spnsalevalue">' + lastweektotattendance[k].salevalue + '</span></a></td>';
                                    }
                                    results += '<td style="display:none" data-title="brandstatus" class="2">' + lastweektotattendance[k].idate + '</td>';
                                    results += '<td style="display:none" data-title="brandstatus" class="2"><span id="spnparlorid">' + lastweektotattendance[k].Branchid + '</span></td>';
                                    results += '<td style="display:none" data-title="brandstatus" class="clsmoniterqty">' + lastweektotattendance[k].salevalue + '</td>';
                                    d = 0;
                                }
                            }
                        }
                        if (d == 1) {
                            var status = "A"
                            st = 1 + '-' + 1;
                            results += '<td class="1" style="display:none"><input class="form-control" type="text" name="empid" id="txtempid"  value="' + st + '"/></td>';
                            results += '<td id="' + st + '" data-title="brandstatus" class="1"><a id="' + st + '"   title="' + st + '" data-toggle="modal" data-target="#myModals">0</a></td>';
                            results += '<td style="display:none" data-title="brandstatus" class="2"></td>';
                        }
                    }
                    results += '</tr>';
                }
            }
            results += '</table></div>';
            $("#divsaledata").html(results);
            lastweekemptytable4 = [];
            //GetTotalCal3();
        }

        function btnPlantClick(branchname) {
            var branchname;
            var data = { 'operation': 'GetLastweekBranchwiseSalesDetails', 'branchname': branchname};
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    else {
                        if (msg.length > 0) {
                            filllastweekbranchsale(msg);
                        }
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
        function filllastweekbranchsale(msg) {
            $('#branchwiselastweekdiv').css('display', 'block');
            lastweekTotalDate = msg[0].wweekcls;
            lastweektotattendance = msg[0].SalelDetails;
            var results = '<div class="box-body" style="padding:0px !important;"><table id="posTable" style="font-weight: 800 !important; text-align: right;" class="table table-bordered  dataTable no-footer" role="grid" aria-="" describedby="example2_info">';
            results += '<thead><tr role="row" class="trbgclrcls">';
            results += '<th scope="col" style="text-align:left;width: 150px;"><i class="fa fa-user" aria-hidden="true"></i>BranchName</th>';
            for (var i = 0; i < lastweekTotalDate.length; i++) {
                var dta = lastweekTotalDate[i].week;
                var mysplit = dta.split("(");
                var dm = mysplit[0];
                var day = mysplit[1]
                results += '<th scope="col" id="txtDate" style="width: 59px;"><i class="fa fa-calendar" aria-hidden="true"></i> ' + dm + ' </br> (' + day + '</th>';
            }

            results += '</tr></thead></tbody>';
            var totalpossalevalue = 0;
            for (var i = 0; i < lastweektotattendance.length; i++) {
                results += '<tr>';
                var parlorname = lastweektotattendance[i].BranchName;
                if (lastweekemptytable4.indexOf(parlorname) == -1) {

                    results += '<td data-title="brandstatus"  onclick="btnPlantClick(\'' + lastweektotattendance[i].Branchid + '\');" class="4" style="text-align:left;">' + lastweektotattendance[i].BranchName + '</td>';
                    results += '<td style="display:none" data-title="brandstatus" class="3">' + lastweektotattendance[i].Branchid + '</td>';
                    lastweekemptytable4.push(parlorname);
                    for (var j = 0; j < lastweekTotalDate.length; j++) {
                        var d = 1;
                        for (var k = 0; k < lastweektotattendance.length; k++) {
                            if (lastweekTotalDate[j].week == lastweektotattendance[k].idate && parlorname == lastweektotattendance[k].BranchName) {
                                if (lastweektotattendance[k].salevalue != "") {
                                    var st = lastweektotattendance[k].Branchid + '-' + lastweektotattendance[k].idate;
                                    //20/Jan(Sunday)
                                    var logdt = lastweektotattendance[k].idate;
                                    var mysplit = logdt.split("(");
                                    var sp = mysplit[0];
                                    var res = logdt.split("/");
                                    var mnth = res[1];
                                    var res2 = mnth.split("(");
                                    var mnth2 = res2[1];
                                    // var cmpid = lastweektotattendance[k].cmpid;
                                    results += '<td class="1" style="display:none"><input class="form-control" type="text" name="empid" id="txtempid"  value="' + st + '"/></td>';
                                    if (mnth2 == "Sunday)" || sp == "26/Jan") {
                                        results += '<td onmouseover="ChangeBackgroundColor(this)" onmouseout="RestoreBackgroundColor(this)"  id="' + st + '" data-title="brandstatus" class="1"><a id="' + st + '"  onclick="logsclick(this);" title="' + st + '" data-toggle="modal" data-target="#myModal" style="color: brown; cursor: pointer !important;"><span id="spnsalevalue">' + lastweektotattendance[k].salevalue + '</span></a></td>';
                                    }
                                    else {
                                        results += '<td onmouseover="ChangeBackgroundColor(this)" onmouseout="RestoreBackgroundColor(this)"  id="' + st + '" data-title="brandstatus" class="1"><a id="' + st + '"  onclick="logsclick(this);" title="' + st + '" data-toggle="modal" data-target="#myModal" style="cursor: pointer !important;"><span id="spnsalevalue">' + lastweektotattendance[k].salevalue + '</span></a></td>';
                                    }
                                    results += '<td style="display:none" data-title="brandstatus" class="2">' + lastweektotattendance[k].idate + '</td>';
                                    results += '<td style="display:none" data-title="brandstatus" class="2"><span id="spnparlorid">' + lastweektotattendance[k].Branchid + '</span></td>';
                                    results += '<td style="display:none" data-title="brandstatus" class="clsmoniterqty">' + lastweektotattendance[k].salevalue + '</td>';
                                    d = 0;
                                }
                            }
                        }
                        if (d == 1) {
                            var status = "A"
                            st = 1 + '-' + 1;
                            results += '<td class="1" style="display:none"><input class="form-control" type="text" name="empid" id="txtempid"  value="' + st + '"/></td>';
                            results += '<td id="' + st + '" data-title="brandstatus" class="1"><a id="' + st + '"   title="' + st + '" data-toggle="modal" data-target="#myModals">0</a></td>';
                            results += '<td style="display:none" data-title="brandstatus" class="2"></td>';
                        }
                    }
                    results += '</tr>';
                }
            }
            results += '</table></div>';
            $("#divbranchsaledata").html(results);
            lastweekemptytable4 = [];
            //GetTotalCal3();
        }
        </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            DashBoard <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
        </ol>
    </section>
    <!-- Main content -->
    <section class="content">
        <div class="row">
            <div class="col-md-6" style="width: 100%;">
                <!-- AREA CHART -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Dash Board</h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data- widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="row" id="div3">
            <div class="col-xs-5" style="width: 49% !important;padding-right: 0px !important;padding-left: 15px !important;">
            <div class="box box-primary">
            <div class="box-header" style="text-align: right;">
            
              <h3 class="box-title">Week Wise Sale Quantity</h3>
             </div>
                 <div id="divrsaledata"> </div>
            </div>
        </div>
        <div class="col-xs-7" style="width:51% !important; padding-right: 0px !important;padding-left: 0px !important;">
            <div class="box box-primary">
            <div class="box-header" style="text-align: right;">
              <h3 class="box-title">Day Wise Sale Quantity</h3>
             </div>
                 <div id="divsaleval"> </div>
                 <br />
            </div>
        </div>
    </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-header">
                            <h3 class="box-title">Plant Wise Sale Quantity</h3>
                        </div>
                        <div class="box-body">
                        <div id="divsaledata">
                          </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" style="display:none;" id="branchwiselastweekdiv">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-header">
                            <h3 class="box-title">Branch Wise Sale Quantity</h3>
                        </div>
                        <div class="box-body">
                        <div id="divbranchsaledata">
                          </div>
                        </div>
                    </div>
                </div>
            </div>


                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
