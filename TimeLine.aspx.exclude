<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TimeLine.aspx.cs" Inherits="TimeLine" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="Kendo/jquery.min.js" type="text/javascript"></script>
    <script src="Kendo/kendo.all.min.js" type="text/javascript"></script>
    <link href="Kendo/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="Kendo/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <link href="Css/Timeline.css" rel="stylesheet" type="text/css" />
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <link href="Css/RouteWiseTimeLine.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {

            Details();
        });
        function templateonclick(id)
        {
         //alert(id.id);
            //alert(id.className);
            //var dispatchid = this.id;
            var txtFromdate = document.getElementById('txtFromdate').value;
            var rowtype = id.className;
            var data = { 'operation': 'Getdetails',  'rowtype': rowtype,'startDate': txtFromdate };
            var s = function (msg) {
                if (msg) {
                    bindapprovals(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function DashBoardDetails() {
            $("#window").empty();
            $("#chart").empty();
            $("#example").empty();
            document.getElementById('Cgraphs').className = "";
            //document.getElementById('Rgraphs').className = "";
            document.getElementById('timeline').className = "";
            document.getElementById('dashboard').className += "active";
            var data = { 'operation': 'GetDashBoardDetails' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    else {
                        BindDashBoard(msg);
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
        function BindDashBoard(msg) {
            $('#example').removeTemplate();
            $('#example').setTemplateURL('TimeLineDetails.htm');
            $('#example').processTemplate(msg);
        }
        function routewiseDetails() {
            var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
            var txtFromdate = document.getElementById('txtFromdate').value;
//            if (txtFromdate == "") {
//                alert("Select start date");
//                return false;
//            }
            var data = { 'operation': 'Getrouteleaksreturns', 'startDate': txtFromdate, 'ddlSalesOffice': ddlSalesOffice };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    else {
                        createChart(msg);
                         routewiseleakDetails();
           
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

        function createChart(databind) {
            var myTableDiv = document.getElementById("example");
            var d = document.createElement("div");
            myTableDiv.appendChild(d);

            //td1.appendChild(d);
            var RouteName = databind[0].RouteName;
            var dispatch = databind[0].totaldispatch;
            var saleqty = databind[0].totalsale;
            var leakqty = databind[0].totalleak;
            var returnqty = databind[0].totalreturn;
            var shortqty = databind[0].totalshort;
            var freeqty = databind[0].totalfree;
            //                for (var i = 0; i < RouteName.length; i++) {
            //                    newXarray.push({ "category": RouteName[i], "value": parseFloat(Amount[i]) });
            //         
            //   }
            $(d).kendoChart({
                title: {
                    text: "Route Wise Details",
                     color: "#FE2E2E",
                    align: "left",
                    font: "20px Verdana"

                },
                legend: {
                    visible: true
                },
                chartArea: {
                    width: 1100,
                    padding: 30
                },
                seriesDefaults: {
                    type: "column",
                },
                series: [{
                    name: "Total Dispatch",
                    data: dispatch
                }, {
                    name: "Total Sale",
                    data: saleqty
                }, {
                    name: "Total Leaks",
                    data: leakqty
                }, {
                    name: "Total Short",
                    data: shortqty
                }, {
                    name: "Total Free",
                    data: freeqty
                }, {
                    name: "Total Return",
                    data: returnqty
                }],
                valueAxis: {
                    line: {
                        visible: false
                    },
                    minorGridLines: {
                        visible: true
                    }
                },
                categoryAxis: {
                    categories: RouteName,
                    majorGridLines: {
                        visible: false
                    }
                },
                tooltip: {
                    visible: true,
                    template: "#= series.name #: #= value #"

                }
            });
            $(d).bind("kendo:skinChange", createChart);
            //$('#chart').append(divleak);


        }

        function routewiseleakDetails() {
            var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
            var txtFromdate = document.getElementById('txtFromdate').value;
//            if (txtFromdate == "") {
//                alert("Select start date");
//                return false;
//            }
            var data = { 'operation': 'Getroutewiseleaks', 'startDate': txtFromdate, 'ddlSalesOffice': ddlSalesOffice };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    else {
                        createleakbarChart(msg);
                         productwiseleakpie();
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
         var newleakXarray = [];
        function createleakbarChart(databind) {
            var myTableDiv = document.getElementById("example");
            var divleakbar = document.createElement("div");
            myTableDiv.appendChild(divleakbar);

            var RouteName = databind[0].RouteName;
            var routeid = databind[0].routeid;
            var leakqty = databind[0].totalleak;
                            for (var i = 0; i < RouteName.length; i++) {
                                newleakXarray.push({ "category": RouteName[i], "categoryid": routeid[i] });
                            }
            $(divleakbar).kendoChart({
                title: {
                    text: "Route Wise % leaks",
                    color:"#FE2E2E",
                    align: "left",
                    font: "20px Verdana"

                },
                legend: {
                    visible: false
                },
                seriesDefaults: {
                    type: "column"
                },
                series: [{
                    name: "Leak",
                    data: leakqty
                }],
                valueAxis: {
                    line: {
                        visible: false
                    },
                    minorGridLines: {
                        visible: true
                    }
                },
                categoryAxis: {
                    categories: RouteName,
                    majorGridLines: {
                        visible: false
                    }
                },
                tooltip: {
                    visible: true,
                    template: "#= series.name #: #= value #%"

                },
                plotAreaClick: function (e) {
                   // alert(e.category);
                                var categoryname=e.category;

                   for (var i = 0; i < newleakXarray.length; i++) {
                                //newXarray.push({ "category": RouteName[i], "categoryid": routeid[i] });
                  if(newleakXarray[i].category==categoryname)
                  {
                  //alert(newleakXarray[i].categoryid);
                  getleakdetails(newleakXarray[i].categoryid);
                  }
                  }
//                  var index=  newXarray.indexOf(e.category);
//                  if(index>=0)
//                  {
//                  alert(newXarray[index].categoryid);
//                  }
                }
            });
            $(divleakbar).bind("kendo:skinChange", createleakbarChart);

        }
        function getleakdetails(routesno)
        {
         var routesno = routesno;
            var txtFromdate = document.getElementById('txtFromdate').value;
//            if (txtFromdate == "") {
//                alert("Select start date");
//                return false;
//            }
            var data = { 'operation': 'Getrouteleaks', 'startDate': txtFromdate, 'routesno': routesno };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    if (msg == "No Leaks Found") {
                        alert(msg);
                    }
                    else {
                    btnpopupclick(msg);
                         
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
        function btnpopupclick(msg) {
           $('.pickupclass').css('display', 'block');
           $('#divSlots').removeTemplate();
                    $('#divSlots').setTemplateURL('routeleaks.htm');
                    $('#divSlots').processTemplate(msg);
       }
       function PopupCloseClick() {
           $('.pickupclass').css('display', 'none');
       }
        function productwiseleakpie() {
            var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
            var txtFromdate = document.getElementById('txtFromdate').value;
//            if (txtFromdate == "") {
//                alert("Select start date");
//                return false;
//            }
            var data = { 'operation': 'GetProductWiseleaks', 'startDate': txtFromdate, 'ddlSalesOffice': ddlSalesOffice };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    else {
                        productleakpieChart(msg);
            threedaysindentdespatch();

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
        var newXarray = [];
        function productleakpieChart(databind) {
            newXarray = [];
            var myTableDiv = document.getElementById("example");
            var divproductleakpie = document.createElement("div");
            myTableDiv.appendChild(divproductleakpie);
            var RouteName = databind[0].RouteName;
            var leakqty = databind[0].totalleak;
            for (var i = 0; i < RouteName.length; i++) {
                newXarray.push({ "category": RouteName[i], "value": parseFloat(leakqty[i]) });
            }
            $(divproductleakpie).kendoChart({
                title: {
                    //position: "bottom",
                    text: "Product Wise % Leaks",
                    color: "#FE2E2E",
                    align: "left",
                    font: "20px Verdana"

                },
                legend: {
                    visible: true
                },
                chartArea: {
                    background: ""
                },
                seriesDefaults: {
                    labels: {
                        visible: true,
                        background: "transparent",
                        template: "#= category #: #= value#"
                    }
                },
                dataSource: {
                    data: newXarray
                },
                series: [{
                    type: "pie",
                    field: "value",
                    categoryField: "category"
                }],
                seriesColors: ["#C0C0C0", "#FF00FF", "#00FFFF", "#FFFF00", "#0000FF", "#00FF00", "#FF0000", "#B43104", "#8A084B", "#0041C2", "#800517", "#1C1715"],
                tooltip: {
                    visible: true,
                    format: "{0}%"
                }
            });
        }

        function threedaysindentdespatch() {
            var ddlSalesOffice = document.getElementById('ddlSalesOffice').value;
            var txtFromdate = document.getElementById('txtFromdate').value;
            
            var data = { 'operation': 'Getthreedaysindentdespatch', 'startDate': txtFromdate, 'ddlSalesOffice': ddlSalesOffice };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    else {
                        createindentChart(msg);
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

        function createindentChart(databind) {
            var myTableDiv = document.getElementById("example");
            var d = document.createElement("div");
            myTableDiv.appendChild(d);
            var dispatch = databind[0].totaldispatch;
            var saleqty = databind[0].totalsale;
            var indent = databind[0].totalleak;
            var indentdate = databind[0].totalreturn;
            $(d).kendoChart({
                title: {
                    text: "Seven Days Indent Vs Despatch",
                    color: "#FE2E2E",
                    align: "left",
                    font: "20px Verdana"
                },
                legend: {
                    visible: false
                },
                seriesDefaults: {
                    type: "column",
                    width:20
                },
                series: [{
                    name: "Total Indent",
                    data: indent
                }, {
                    name: "Total Dispatch",
                    data: dispatch
                }, {
                    name: "Total Sale",
                    data: saleqty
                }],
                valueAxis: {
                    //max: 15000,
                    line: {
                        visible: false
                    },
                    minorGridLines: {
                        visible: true
                    }
                },
                categoryAxis: {
                    categories: indentdate,
                    majorGridLines: {
                        visible: false
                    }
                },
                tooltip: {
                    visible: true,
                    template: "#= series.name #: #= value #"

                }
            });
            $(d).bind("kendo:skinChange", createindentChart);

        }

        
        function Details() {

          var date = document.getElementById('txtFromdate').value;
//            if (date == "" || date == "mm/dd/yyyy") {
//                alert("Please Select Date");
//                return false;
            //            }
            var status = "salesoffice";
            var data = { 'operation': 'GetApprovalDetails', 'IndentDate': date, 'status': status };
            var s = function (msg) {
                if (msg) {
                    bindsalesofficedetails(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function Cgraphs() {
            $("#window").empty();
            $("#chart").empty();
            $("#example").empty();
            document.getElementById('Cgraphs').className += "active";
           // document.getElementById('Rgraphs').className = "";
            document.getElementById('timeline').className = "";
            document.getElementById('dashboard').className = "";

            document.getElementById('window').innerHTML = "";
            routewiseDetails();
           
        }
        function Rgraphs() {
            $("#window").empty();
            $("#chart").empty();
            $("#example").empty();
            document.getElementById('Cgraphs').className = "";
            //document.getElementById('Rgraphs').className += "active";
            document.getElementById('timeline').className = "";
            document.getElementById('dashboard').className = "";
        }
        function openwindow() {
            document.getElementById('Cgraphs').className = "";
            //document.getElementById('Rgraphs').className = "";
            document.getElementById('timeline').className += "active";
            document.getElementById('dashboard').className = "";


            $('#window').css('display', 'block');
            $("#window").empty(); 
            $("#chart").empty();
            $("#example").empty();

            var date = document.getElementById('txtFromdate').value;
//            if (date == "" || date == "mm/dd/yyyy") {
//                alert("Please Select Date");
//                return false;
//            }
            var data = { 'operation': 'GetApprovalDetails', 'IndentDate': date };
            var s = function (msg) {
                if (msg) {
                    $('#window').removeTemplate();
                    $('#window').setTemplateURL('RouteWiseTimeLine.htm');
                    $('#window').processTemplate(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
            
        }
        function bindsalesofficedetails(msg) {
            $('#divtemplate').removeTemplate();
            $('#divtemplate').setTemplateURL('Timeline.htm');
            $('#divtemplate').processTemplate(msg);
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
    <style type="text/css">
        .active
        {
            color: #555555;
            text-decoration: none;
            background-color: #e5e5e5;
        }
        .cp
        {
            /* these styles will let the divs line up next to each other
       while accepting dimensions */
            display: inline-block;
            width: 100%;
            height: 500px; /*background: black;*/ /* a small margin to separate the blocks */
            margin-left: 10px;
            margin-right: 10px;
            margin-top: 10px;
            margin-bottom: 10px;
        }
        window</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Timeline Report<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Timeline Report</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Timeline Details
                </h3>
            </div>
            <div class="box-body">
        <table>
            <tr>
                <td>
                    <div id="divSo" style="display: none;">
                        <select id="ddlSalesOffice" class="form-control">
                        </select>
                    </div>
                </td>
                  <td style="width: 5px;">
                                </td>
                <td>
                    Select date
                </td>
                <td>
                    <input type="date" id="txtFromdate" class="form-control" />
                </td>
                  <td style="width: 5px;">
                                </td>
                <td>
                    <input type="button" id="submit" value="Submit"  class="btn btn-primary" onclick="Details();" />
                </td>
            </tr>
        </table>
    </div>
    <div id="divtemplate">
    </div>
    <div class="sub-nav">
        <div class="navbar">
            <div class="navbar-inner">
                <ul class="nav analytics-tabs">
                    <li class="" id="timeline" onclick="openwindow();"><a>Route Info</a></li>
                    <li class="" id="Cgraphs" onclick="Cgraphs();"><a>Graphs</a></li>
                    <%--<li class="" id="Rgraphs" onclick="Rgraphs();"><a>Graphs</a></li>--%>
                    <li class="" id="dashboard" onclick="DashBoardDetails();"><a>TimeLine</a></li>
                </ul>
            </div>
        </div>
    </div>
    <div id="window">
    </div>
    <div id="example" class="k-content">
        <div class="chart-wrapper">
            <%-- style="background: center no-repeat url('../../content/shared/styles/world-map.png');"--%>
            <div id="chart">
            </div>
        </div>
    </div>
    <div id="divSlots" style="display: none;">
    </div>
    </div>
    </section>
</asp:Content>
