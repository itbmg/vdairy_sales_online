<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RouteAgents.aspx.cs" Inherits="RouteAgents" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <link rel="icon" href="Images/Vyshnavilogo.png" type="image/x-icon" title=BMG />
    <title>Vyshnavi Dairy </title>
    <style type="text/css">
        div#mapcontent
        {
            right: 0;
            bottom: 0;
            left: 0px;
            top: 48px;
            overflow: hidden;
            position: absolute;
        }
        .inner
        {
            width: 320px;
            position: absolute;
            left: 0px;
            bottom: 0px;
            color: Black;
            z-index: 99900;
            height: 100%;
            top: -2px;
            opacity: 0.9;
        }
        .togglediv
        {
            position: absolute;
            width: 300px;
            color: Black; /*z-index: 99900;*/
            border: 2px solid #d5d5d5;
            background-color: #ffffff;
            cursor: pointer;
            top: 57px;
            bottom: 0px;
            left: 0px;
            opacity: 0.9;
            z-index: 99999;
        }
        .datebuttonsleft
        {
            -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff;
            box-shadow: inset 0px 1px 0px 0px #ffffff;
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #ededed), color-stop(1, #dfdfdf) );
            cursor: pointer;
            margin: 0px;
            border-radius: 3px 0px 0px 0px;
            width: 35px;
            height: 26px;
            border: 0px solid #ffffff;
            background: #d5d5d5 url(Images/Left.png) no-repeat center;
        }
        .datebuttonsright
        {
            -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff;
            box-shadow: inset 0px 1px 0px 0px #ffffff;
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #ededed), color-stop(1, #dfdfdf) );
            cursor: pointer;
            margin: 0px;
            border-radius: 0px 3px 0px 0px;
            width: 35px;
            height: 26px;
            border: 0px solid #ffffff;
            background: #d5d5d5 url(Images/Right.png) no-repeat center;
        }
        .datebuttonsleft:hover
        {
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #dfdfdf), color-stop(1, #ededed) );
            background: -moz-linear-gradient( center top, #dfdfdf 5%, #ededed 100% );
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#dfdfdf', endColorstr='#ededed');
            background: #f5f5f5 url(Images/Left.png) no-repeat center;
            background-color: #dfdfdf;
        }
        .datebuttonsright:hover
        {
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #dfdfdf), color-stop(1, #ededed) );
            background: -moz-linear-gradient( center top, #dfdfdf 5%, #ededed 100% );
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#dfdfdf', endColorstr='#ededed');
            background: #f5f5f5 url(Images/Right.png) no-repeat center;
            background-color: #dfdfdf;
        }
    </style>
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
   <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&key=AIzaSyDGYPgYpwZ4ZQCLCAujetDwArlVBC_S9TI&sensor=false"></script>
    <script src="JIC/infobox.js" type="text/javascript"></script>
    <script src="JIC/markerclusterer.js" type="text/javascript"></script>
    <script src="JIC/markerclusterer_GZ.js" type="text/javascript"></script>
    <style type="text/css">
        html, body
        {
            margin: 0;
            padding: 0;
            height: 100%;
        }
        .bpmouseover
        {
            height: 430px;
            width: 250px;
            display: none;
            position: absolute;
            z-index: 99999;
            padding: 10px 5px 5px 15px;
            background-color: #fffffc;
            border: 1px solid #c0c0c0;
            border-radius: 3px 3px 3px 3px;
            box-shadow: 3px 3px 3px rgba(0,0,0,0.3);
            font-family: Verdana;
            font-size: 12px;
            opacity: 1.0;
        }
        
        .googleMapcls
        {
            width: 100%;
            height: 650px;
            position: relative;
            overflow: hidden;
        }
        
        .inner
        {
            width: 350px;
            position: absolute;
            left: 0px;
            bottom: 0px;
            color: Black;
            z-index: 99900;
            height: 100%;
            top: -2px;
            opacity: 0.9;
        }
        .togglediv
        {
            position: absolute;
            width: 330px;
            color: Black; /*z-index: 99900;*/
            border: 2px solid #d5d5d5;
            background-color: #ffffff;
            opacity: 1.9;
            cursor: pointer;
            top: 57px;
            bottom: 0px;
            left: 0px;
            opacity: 0.9;
        }
        
        .btnShowcls
        {
            display: inline-block;
            min-width: 54px;
            border: 1px solid #dcdcdc;
            border: 1px solid rgba(0,0,0,0.1);
            text-align: center;
            color: #444;
            font-size: 12px;
            font-weight: bold;
            height: 27px;
            padding: 0 8px;
            line-height: 27px;
            -webkit-border-radius: 2px;
            -moz-border-radius: 2px;
            border-radius: 2px;
            -webkit-transition: all 0.218s;
            -moz-transition: all 0.218s;
            -o-transition: all 0.218s;
            transition: all 0.218s;
            background-color: #f5f5f5;
            background-image: -webkit-gradient(linear,left top,left bottom,from(#f5f5f5),to(#f1f1f1));
            background-image: -webkit-linear-gradient(top,#f5f5f5,#f1f1f1);
        }
        .btnShowcls:hover
        {
            cursor: pointer;
            -moz-box-shadow: 0px 0px 10px 0px #e0e0e0;
            -webkit-box-shadow: 0px 0px 10px 0px #e0e0e0;
            box-shadow: 0px 0px 10px 0px #e0e0e0;
        }
        div#mapcontent
        {
            right: 0;
            bottom: 0;
            left: 0px;
            top: 48px;
            overflow: hidden;
            position: absolute;
        }
        .tabclass
        {
            display: inline;
            background: #f4f4f4;
            color: rgb(243, 12, 12);
            border: 1px solid gray;
            font-size: 12px;
            margin-right: 4px;
            text-align: center;
            cursor: pointer;
            padding-left: 4px;
            padding-right: 4px;
            font-weight: bold;
        }
        .elemstyle
        {
            direction: ltr;
            overflow: hidden;
            text-align: center;
            position: relative;
            color: rgb(0, 0, 0);
            font-family: Arial, sans-serif;
            -webkit-user-select: none;
            font-size: 13px;
            background-color: rgb(255, 255, 255);
            padding: 1px 6px;
            border: 1px solid rgb(113, 123, 135);
            -webkit-box-shadow: rgba(0, 0, 0, 0.4) 0px 2px 4px;
            box-shadow: rgba(0, 0, 0, 0.4) 0px 2px 4px;
            font-weight: bold;
            min-width: 29px;
            background-position: initial initial;
            background-repeat: initial initial;
        }
        #progressbar1 .ui-progressbar-value
        {
            background-color: #6CEB7B;
        }
        #progressbar2 .ui-progressbar-value
        {
            background-color: #6CEB7B;
        }
        #progressbar3 .ui-progressbar-value
        {
            background-color: #6CEB7B;
        }
        #progressbar4 .ui-progressbar-value
        {
            background-color: #6CEB7B;
        }
        
        .progress-label
        {
            position: absolute;
            left: 50%;
            top: 4px;
            font-weight: bold;
            text-shadow: 1px 1px 0 #fff;
        }
        .pickupclass
        {
            height: 100%;
            width: 100%;
            display: none;
            position: absolute;
            z-index: 99999;
            padding: 10px 5px 5px 15px;
            background-color: #FFFFFF;
            border: 1px solid Gray;
            top: 230px;
            left: 100px;
            position: absolute;
            border-radius: 3px 3px 3px 3px;
            box-shadow: 3px 3px 3px rgba(0,0,0,0.3);
            font-family: Verdana;
            font-size: 12px;
            opacity: 1.0;
        }
    </style>
    <script type="text/javascript">
        // Add a Home control that returns the user to London
        function HomeControl(controlDiv, map) {
        }

        function DrawControl(controlDiv, map) {
        }
        function StopControl(controlDiv, map) {
        }
        function ClearControl(controlDiv, map) {
        }
        $(function () {
            var data = { 'operation': 'GetSalesOffice' };
            var s = function (msg) {
                if (msg) {
                    Bindplants(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        });
        function Bindplants(data) {
            var select = document.getElementById('ddl_plants');
            var opt = document.createElement('option');
            opt.value = "Select Sales Office";
            opt.innerHTML = "Select Sales Office";
            select.appendChild(opt);
            for (var i = 0; i < data.length; i++) {
                if (data[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = data[i].BranchName;
                    opt.value = data[i].Sno;
                    select.appendChild(opt);
                }
            }
        }

        function plantsselectchange() {
            var selected = document.getElementById('ddl_plants');
            var Branch = selected.options[selected.selectedIndex].text;
            var BranchID = selected.options[selected.selectedIndex].value;
            if (BranchID == "Select Sales Office") {
                alert("Please Select Sales Office");
                return;
            }
            var data = { 'operation': 'GetSalesOfficeRouteDespatches', 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    Bindroutes(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function Bindroutes(data) {
            document.getElementById('divtrips').innerHTML = "";
            var divtrips = document.getElementById('divtrips');
            $("#divtrips").append("<table><tr><td><input autocomplete='off' class='checkinput' type='checkbox' title='SelectAll' onclick='checkclick(this);'/><span class='livalue'>Select All</span></td></tr></table>");
            for (var i = 0; i < data.length; i++) {
                $("#divtrips").append("<table><tr><td><input autocomplete='off' class='checkinput' type='checkbox' onclick='checkclick(this);' title=" + data[i].DespName + "/><span class='livalue'>" + data[i].DespName + "</span></td></tr></table>");
            }
        }
        function checkclick(checkedvalue) {
            var checkinputs = $('#divtrips').find('.checkinput');
            if (checkedvalue.title == "SelectAll") {
                if (checkedvalue.checked == true) {
                    checkinputs.each(function (list) {
                        var checkbox = checkinputs[list];
                        checkbox.checked = true;
                    });
                }
                else {
                    checkinputs.each(function (list) {
                        var checkbox = checkinputs[list];
                        checkbox.checked = false;
                    });
                }
            }
            else {
                if (checkedvalue.checked == false) {
                    checkinputs.each(function (list) {
                        var checkbox = checkinputs[list];
                        if (checkbox.title == "SelectAll") {
                            checkbox.checked = false;
                        }
                    });
                }
            }
        }
        $(function () {
            var hidden = false;
            $("#btnClose").click(function () {
                if (hidden) {
                    $(".togglediv").stop().animate({ left: 0 }, 500);
                    hidden = false;
                    $("#btnClose").attr('title', "Hide");
                    $("#btnClose").attr('src', "Images/bigleft.png");
                }
                else {
                    $(".togglediv").css('margin-left', 0);
                    $(".togglediv").css('margin-right', 0);
                    $(".togglediv").animate({ left: '-305px' }, 500);
                    $("#btnClose").attr('title', "Show");
                    $("#btnClose").attr('src', "Images/bigright.png");
                    hidden = true;
                }
            });
        });
    </script>
    <script type="text/javascript">
        var map;
        function initialize() {
            var mapOptions = {
                zoom: 12,
                center: new google.maps.LatLng(13.1045991, 80.2007615),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map = new google.maps.Map(document.getElementById('googleMap'), mapOptions);
            var homeControlDiv = document.createElement('div');
            var homeControl = new HomeControl(homeControlDiv, map);
            map.controls[google.maps.ControlPosition.TOP_RIGHT].push(homeControlDiv);

            var ClearControlDiv = document.createElement('div');
            var clearcontrol = new ClearControl(ClearControlDiv, map);
            map.controls[google.maps.ControlPosition.TOP_RIGHT].push(ClearControlDiv);

            var StopControlDiv = document.createElement('div');
            var stopcontrol = new StopControl(StopControlDiv, map);
            map.controls[google.maps.ControlPosition.TOP_RIGHT].push(StopControlDiv);

            var DrawControlDiv = document.createElement('div');
            var drawcontrol = new DrawControl(DrawControlDiv, map);
            map.controls[google.maps.ControlPosition.TOP_RIGHT].push(DrawControlDiv);
        }
        google.maps.event.addDomListener(window, 'load', initialize);
    </script>
    <script type="text/javascript">
        function btn_generate_Click() {
            isfirstlocation = false;
            initialize();
            clearInterval(interval);
            var Username = '<%= Session["field1"] %>';
            var selected = document.getElementById('ddl_plants');
            var plantname = selected.value;
            var startdt = ""; //  document.getElementById('txtFromDate').value;
            var checkinputs = $('#divtrips').find('.checkinput');
            var checkedroutes = "";
            checkinputs.each(function (list) {
                var checkbox = checkinputs[list];
                if (checkbox.checked == true) {
                    var spn = $(this).next('.livalue');
                    var spanvalue = spn[0].innerHTML;
                    if (spanvalue != "SelectAll") {
                        checkedroutes += spanvalue + "@";
                    }
                    else {
                        checkedroutes = "";
                        return false;
                    }
                }
            });
            if (checkedroutes.length > 0) {
                checkedroutes = checkedroutes.slice(0, checkedroutes.length - 1);
            }
            var Branch = selected.options[selected.selectedIndex].text;
            var BranchID = selected.options[selected.selectedIndex].value;
            if (BranchID == "Select Sales Office") {
                alert("Please Select Sales Office");
                return;
            }
            var data = { 'operation': 'GetAgentLocations', 'startdt': startdt, 'routes': checkedroutes, 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    closeinfowindow();
                    polyroute(msg);
                    GetOtheragents();
                    GetStoppedAgents();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        var interval;
        var colors = new Array();
        var markersArray = [];
        var polilinepath = [];
        var firstlog = false;
        var stoppedmarkers = [];
        var flightPath = null;
        var maxcount = 0;
        var vehiclesarray = [];
        var allflightPlanCoordinates = [];
        var globellooper;
        var vehcolors = [];
        var isfirstlocation = false;

        function polyroute(msg) {
            logcount = 0;
            remcount = 0;
            $("#speedval").val("speed");
            polilinepath = [];
            allflightPlanCoordinates = [];
            if (flightPath) {
                flightPath.setMap(null);
            }
            colors = new Array("red", "blue", "black", "gray", "maroon", "Alabama Crimson", "Amber", "Bangladesh green", "Heart Gold", "Camouflage green", "Cadmium red", "Burgundy", "Bright green");
            var triproutedata = msg;
            var selected = document.getElementById('ddl_plants');
            var plantname = selected.options[selected.selectedIndex].text;
            for (var cnt = 0; cnt < triproutedata.length; cnt++) {
                var Latitude = triproutedata[cnt].latitude;
                var Longitude = triproutedata[cnt].longitude;
                var point = new google.maps.LatLng(
              parseFloat(Latitude),
              parseFloat(Longitude));
                var pltname = triproutedata[cnt].BranchName;
                var dispname = triproutedata[cnt].dispname;
                var routecolor = triproutedata[cnt].routecolor;
                var dispname = triproutedata[cnt].BranchName + "--" + triproutedata[cnt].dispname;
                var lctnicon = "";
                if (routecolor == "") {
                    lctnicon = "Images/Green.png";
                }
                if (routecolor == "meroon") {
                    lctnicon = "Images/meroon1.png";
                }
                if (routecolor == "red") {
                    lctnicon = "Images/red1.png";
                }
                if (routecolor == "yellow") {
                    lctnicon = "Images/yellow.png";
                }
                if (routecolor == "blue") {
                    lctnicon = "Images/blue1.png";
                }
                if (routecolor == "green") {
                    lctnicon = "Images/Green.png";
                }
                if (routecolor == "black") {
                    lctnicon = "Images/black1.png";
                }
                if (routecolor == "sky") {
                    lctnicon = "Images/sky1.png";
                }

                var marker = new google.maps.Marker({
                    position: point,
                    map: map,
                    center: location,
                    zoom: 12,
                    icon: lctnicon,
                    title: dispname
                });
                markersArray.push(marker);
                var content = "Agent Name : " + pltname + "Route Name : " + dispname;
                var infowindow = new google.maps.InfoWindow({
                    content: content
                });
                google.maps.event.addListener(marker, 'click', function () {
                    infowindow.open(map, marker);
                });
                attachInfowindow(marker, location, "Agent Name : " + pltname);
            }
        }
        function GetOtheragents() {
            var selected = document.getElementById('ddl_plants');
            var plantname = selected.value;
            var startdt = ""; //  document.getElementById('txtFromDate').value;
            var checkinputs = $('#divtrips').find('.checkinput');
            var checkedroutes = "";
            checkinputs.each(function (list) {
                var checkbox = checkinputs[list];
                if (checkbox.checked == true) {
                    var spn = $(this).next('.livalue');
                    var spanvalue = spn[0].innerHTML;
                    if (spanvalue != "SelectAll") {
                        checkedroutes += spanvalue + "@";
                    }
                    else {
                        checkedroutes = "";
                        return false;
                    }
                }
            });
            if (checkedroutes.length > 0) {
                checkedroutes = checkedroutes.slice(0, checkedroutes.length - 1);
            }
            var Branch = selected.options[selected.selectedIndex].text;
            var BranchID = selected.options[selected.selectedIndex].value;
            if (BranchID == "Select Sales Office") {
                alert("Please Select Sales Office");
                return;
            }
            var data = { 'operation': 'GetOtherAgentLocations','routes': checkedroutes, 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    Otherpolyroute(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        var infoWindow = new google.maps.InfoWindow();
        function Otherpolyroute(msg) {
            logcount = 0;
            remcount = 0;
            $("#speedval").val("speed");
            polilinepath = [];
            allflightPlanCoordinates = [];
            if (flightPath) {
                flightPath.setMap(null);
            }
            colors = new Array("red", "blue", "black", "gray", "maroon", "Alabama Crimson", "Amber", "Bangladesh green", "Heart Gold", "Camouflage green", "Cadmium red", "Burgundy", "Bright green");
            var triproutedata = msg;
            var selected = document.getElementById('ddl_plants');
            var plantname = selected.options[selected.selectedIndex].text;
            for (var cnt = 0; cnt < triproutedata.length; cnt++) {
                var Latitude = triproutedata[cnt].latitude;
                var Longitude = triproutedata[cnt].longitude;
                var point = new google.maps.LatLng(
              parseFloat(Latitude),
              parseFloat(Longitude));
                var pltname = triproutedata[cnt].BranchName;
                var lctnicon = "Images/red1.png";
                var marker = new google.maps.Marker({
                    position: point,
                    map: map,
                    center: location,
                    zoom: 12,
                    icon: lctnicon,
                    title: pltname
                });
                markersArray.push(marker);
                var content = "Agent Name : " + pltname;
                var infowindow = new google.maps.InfoWindow({
                    content: content
                });
                google.maps.event.addListener(marker, 'click', function () {
                    infowindow.open(map, marker);
                });
                attachInfowindow(marker, location, "Agent Name : " + pltname);
            }
        }

        function GetStoppedAgents() {
            var selected = document.getElementById('ddl_plants');
            var plantname = selected.value;
            var startdt = ""; //  document.getElementById('txtFromDate').value;
            var checkinputs = $('#divtrips').find('.checkinput');
            var checkedroutes = "";
            checkinputs.each(function (list) {
                var checkbox = checkinputs[list];
                if (checkbox.checked == true) {
                    var spn = $(this).next('.livalue');
                    var spanvalue = spn[0].innerHTML;
                    if (spanvalue != "SelectAll") {
                        checkedroutes += spanvalue + "@";
                    }
                    else {
                        checkedroutes = "";
                        return false;
                    }
                }
            });
            if (checkedroutes.length > 0) {
                checkedroutes = checkedroutes.slice(0, checkedroutes.length - 1);
            }
            var Branch = selected.options[selected.selectedIndex].text;
            var BranchID = selected.options[selected.selectedIndex].value;
            if (BranchID == "Select Sales Office") {
                alert("Please Select Sales Office");
                return;
            }
            var data = { 'operation': 'GetStoppedAgentLocations', 'routes': checkedroutes, 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    stoppedpolyroute(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function stoppedpolyroute(msg) {
            logcount = 0;
            remcount = 0;
            $("#speedval").val("speed");
            polilinepath = [];
            allflightPlanCoordinates = [];
            if (flightPath) {
                flightPath.setMap(null);
            }
            colors = new Array("red", "blue", "black", "gray", "maroon", "Alabama Crimson", "Amber", "Bangladesh green", "Heart Gold", "Camouflage green", "Cadmium red", "Burgundy", "Bright green");
            var triproutedata = msg;
            var selected = document.getElementById('ddl_plants');
            var plantname = selected.options[selected.selectedIndex].text;
            for (var cnt = 0; cnt < triproutedata.length; cnt++) {
                var Latitude = triproutedata[cnt].latitude;
                var Longitude = triproutedata[cnt].longitude;
                var point = new google.maps.LatLng(
              parseFloat(Latitude),
              parseFloat(Longitude));
                var pltname = triproutedata[cnt].BranchName;
                var lctnicon = "Images/Yellow.png";
                var marker = new google.maps.Marker({
                    position: point,
                    map: map,
                    center: location,
                    zoom: 12,
                    icon: lctnicon,
                    title: pltname
                });
                markersArray.push(marker);
                var content = "Agent Name : " + pltname;
                var infowindow = new google.maps.InfoWindow({
                    content: content
                });
                google.maps.event.addListener(marker, 'click', function () {
                    infowindow.open(map, marker);
                });
                attachInfowindow(marker, location, "Agent Name : " + pltname);
            }
        }
        function attachInfowindow(marker, latlng, country) {
            var location = latlng;
            var boxText = document.createElement("div");
            boxText.style.cssText = "border: 1px solid black; margin-top: 8px; background: white; padding: 5px;";
            boxText.innerHTML = '<b>' + country + '</b><br />';

            var myOptions = {
                content: boxText
				, disableAutoPan: false
				, maxWidth: 0
				, pixelOffset: new google.maps.Size(-140, 0)
				, zIndex: null
				, boxStyle: {
				    background: "url('Images/tipbox.gif') no-repeat"
				  , opacity: 0.9
				  , width: "350px"
				}
				, closeBoxMargin: "10px 5px 0px 2px"
                , closeBoxURL: ""
				, infoBoxClearance: new google.maps.Size(1, 1)
				, isHidden: false
				, pane: "floatPane"
				, enableEventPropagation: false
            };


            var ib = new InfoBox(myOptions);
            //var infowindow = new google.maps.InfoWindow({ content: '<b>' + description + '</b><br />' + location });
            google.maps.event.addListener(marker, 'mouseover', function () {
                //infowindow.open(map,marker);
                ib.open(map, marker);
            });
            google.maps.event.addListener(marker, 'mouseout', function () {
                //infowindow.close();
                ib.close();
            });
        }
        function deleteOverlays() {
            clearOverlays();
            markersArray = [];
        }

        // Sets the map on all markers in the array.
        function setAllMap(map) {
            for (var i = 0; i < markersArray.length; i++) {
                markersArray[i].setMap(map);
            }
        }

        // Removes the overlays from the map, but keeps them in the array.
        function clearOverlays() {
            setAllMap(null);
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

        var vehlogscount = [];
        function btnclearclick() {
            deleteallOverlays();
            function deleteallOverlays() {
                clearallOverlays();
                polilinepath = [];
                for (var cnt = 0; cnt < triproutedata.vehicleslist.length; cnt++) {
                    var flightPlanCoordinates = [];
                    flightPlanCoordinates = vehiclesarray[triproutedata.vehicleslist[cnt].vehicleno];
                    var prevcnt = vehlogscount[triproutedata.vehicleslist[cnt].vehicleno];
                    remcount = flightPlanCoordinates.length;
                    var tot = remcount + prevcnt;
                    vehlogscount[triproutedata.vehicleslist[cnt].vehicleno] = tot;
                    flightPlanCoordinates.length = 0;
                    vehiclesarray[triproutedata.vehicleslist[cnt].vehicleno] = flightPlanCoordinates;
                }
            }
            // Sets the map on all markers in the array.
            function allsetAllMap(map) {
                for (i = 0; i < polilinepath.length; i++) {
                    polilinepath[i].setMap(map); //or line[i].setVisible(false);
                }
            }

            // Removes the overlays from the map, but keeps them in the array.
            function clearallOverlays() {
                allsetAllMap(null);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div  style="width:100%;height:85px;">
        <div style="width: 86%; float: right; text-align: center;">
            <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="30px" Font-Italic="true"
                Font-Names="oblique" ForeColor="#0252aa" Text=""></asp:Label>
        </div>
        <div style="float: right;">
         <a href="Delivery_Collection_Report.aspx"><span style="font-size: 18px;color: #0252aa;">Back        </span></a> 
                <a href="LogOut.aspx">    LogOut</a> 
        </div>
    </div>
    <div style="width: 100%; height: 100%;">
        <div class="togglediv" id="divtoggle">
            <div class="inner">
                <img id="btnClose" alt="" src="Images/bigleft.png" title="Hide" style="float: right;
                    border: 1px solid #d5d5d5; width: 17px; height: 20px; background-color: #ffffff;" />
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Sales Office Name</label>
                                    </td>
                                    <td>
                                        <%--  <asp:DropDownList ID="ddl_plant" CssClass="txtClass" runat="server" Width="150px">
                                    <asp:ListItem Value="0" Selected="True">Select Plant</asp:ListItem>
                                </asp:DropDownList>--%>
                                        <select id="ddl_plants" style="width: 200px;" onchange="plantsselectchange();">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div id="divtrips" style="height: 200px; width: 200px; border: 1px solid #d5d5d5;
                                            border-radius: 3px 3px 3px 3px; overflow: auto;">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <%-- <tr>
                                    <td>
                                        <label>
                                            Date</label>
                                    </td>
                                    <td>
                                        <input id="txtFromDate" type="date" style="width: 200px; height: 22px; font-size: 13px;
                                            padding: .2em .4em; border: 1px solid gray; border-radius: 4px 4px 4px 4px;" />
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <input type="button" id="btn_generate" class="ContinueButton" value="Get Locations"
                                            style="height: 25px; width: 100px;" onclick="btn_generate_Click();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_nofifier" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <%-- <table cellpadding="0" cellspacing="0" style="border: 1px solid #d5d5d5; width: 55px;
                                            height: 22px; border-radius: 3px 3px 3px 3px;">
                                            <tr>
                                                <td>
                                                    <button id="precday" class="datebuttonsleft" onclick="return PrevValidating();">
                                                    </button>
                                                </td>
                                                <td>
                                                    <input type="text" style="width: 40px; padding: .2em .0em; height: 21px; border-top: 0px solid #ffffff;
                                                        text-align: center; border-bottom: 0px solid #ffffff; border-left: 1px solid #d5d5d5;
                                                        border-right: 1px solid #d5d5d5; font-size: 13px;" readonly="readonly" id="speedval"
                                                        value="speed" />
                                                </td>
                                                <td>
                                                    <button id="nextday" class="datebuttonsright" onclick="return NextValidating();">
                                                    </button>
                                                </td>
                                            </tr>
                                        </table>--%>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="mapcontent">
            <div id="googleMap" style="width: 100%; height: 100%; position: relative; background-color: rgb(229, 227, 223);">
            </div>
        </div>
    </div>
    </form>
</body>
</html>
