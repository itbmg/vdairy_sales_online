<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Permissions.aspx.cs" Inherits="Permissions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <style type="text/css">
        .divselectedclass
        {
            border: 1px solid gray;
            padding-top: 2px;
            padding-bottom: 2px;
        }
        .back-red
        {
            background-color: #ffffcc;
        }
        .back-white
        {
            background-color: #ffffff;
        }
        .datepicker
        {
            border: 1px solid gray;
            background: url("Images/CalBig.png") no-repeat scroll 99%;
            width: 70%;
            top: 0;
            left: 0;
            height: 20px;
            font-weight: 700;
            font-size: 12px;
            cursor: pointer;
            border: 1px solid gray;
            margin: .5em 0;
            padding: .6em 20px;
            border-radius: 10px 10px 10px 10px;
            filter: Alpha(Opacity=0);
            box-shadow: 3px 3px 3px #ccc;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#datepicker").datepicker({ dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
                onSelect: function (selectedDate) {
                }
            });
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#datepicker').val(today);
            FillSalesOffice();
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

        });

        function FillSalesOffice() {
            var data = { 'operation': 'GetPlantSalesOffice' };
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
            var ddlsalesOffice = document.getElementById('ddlSalesOffice');
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
        function ddlSalesOfficeChange(ID) {
            var BranchID = ID.value;
            var data = { 'operation': 'GetSalesRoutes', 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindRouteName(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindRouteName(msg) {
            document.getElementById('ddlRouteName').options.length = "";
            var veh = document.getElementById('ddlRouteName');
            var length = veh.options.length;
            for (i = length - 1; i >= 0; i--) {
                veh.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Route Name";
            opt.value = "";
            veh.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].RouteName;
                    opt.value = msg[i].rid;
                    veh.appendChild(opt);
                }
            }
        }
        function ddlRouteNameChange(id) {
            FillAgentName(id.value);
        }

        var div_uncheck_Array = [];
        var div_check_Array = [];
        var div_prevlist_Array = [];
        function fillroutes_divchklist(msg) {
            document.getElementById('divchblroutes').innerHTML = "";
            document.getElementById('divselected').innerHTML = "";
            for (var i = 0; i <= msg.length; i++) {
                if (typeof msg[i] === "undefined" || msg[i].BranchName == "" || msg[i].BranchName == null) {
                }
                else {
                    var label = document.createElement("span");
                    var hidden = document.createElement("input");
                    hidden.type = "hidden";
                    hidden.name = "hidden";
                    hidden.value = msg[i].Sno;
                    var checkbox = document.createElement("input");
                    checkbox.type = "checkbox";
                    checkbox.name = "checkbox";
                    checkbox.value = "checkbox";
                    checkbox.id = "checkbox";
                    checkbox.className = 'chkclass';
                    document.getElementById('divchblroutes').appendChild(checkbox);
                    label.innerHTML = msg[i].BranchName;
                    document.getElementById('divchblroutes').appendChild(label);
                    document.getElementById('divchblroutes').appendChild(hidden);
                    document.getElementById('divchblroutes').appendChild(document.createElement("br"));
                }
            }
            TabclassClick();
        }
        function TabclassClick() {
            $("input[type='checkbox']").click(function () {
                if ($(this).is(":checked")) {
                    var Selected = $(this).next().text();
                    var Selectedid = $(this).next().next().val();
                    var label = document.createElement("div");
                    var Crosslabel = document.createElement("img");
                    Crosslabel.style.float = "right";
                    Crosslabel.src = "Images/Cross.png";
                    Crosslabel.onclick = function () { RemoveClick(Selectedid); };
                    label.id = Selectedid;
                    label.innerHTML = Selected;
                    label.className = 'divselectedclass';
                    label.onclick = function () { divonclick(label); }
                    document.getElementById('divselected').appendChild(label);
                    label.appendChild(Crosslabel);
                }
                else {
                    var Selected = $(this).next().next().val();
                    var elem = document.getElementById(Selected);
                    var p = elem.parentNode;
                    p.removeChild(elem);
                }
            });
        }
        function FillAgentName(RouteID) {
            var data = { 'operation': 'GetAgents', 'RouteID': RouteID };
            var s = function (msg) {
                if (msg) {
                    fillroutes_divchklist(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function RemoveClick(Selected) {
            var elem = document.getElementById(Selected);
            var p = elem.parentNode;
            p.removeChild(elem);
            $('.chkclass').each(function () {
                if ($(this).next().next().val() == Selected) {
                    $(this).attr("checked", false);
                    $(this).attr("disabled", false);
                }
            });

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
        function btn_PermissionsSaveclick() {
            var div = document.getElementById('divselected');
            var divs = div.getElementsByTagName('div');
            var divArray = [];
            for (var i = 0; i < divs.length; i += 1) {
                divArray.push(divs[i].id);
            }
            if (divArray.length == 0) {
                alert("Please Select Agents");
                return false;
            }
            var txtRemarks = document.getElementById("txtRemarks").value;
            var PaidDate = document.getElementById('datepicker').value;
            var ddlpermissionfor = document.getElementById('ddlpermissionfor').value;
            var ddlsalesofficeid = document.getElementById('ddlSalesOffice').value;
            var btnsave = document.getElementById('btnSave').value;

            var Data = { 'operation': 'save_Permissions', 'dataarr': divArray, 'Remarks': txtRemarks, 'Permissiondate': PaidDate, 'permissionfor': ddlpermissionfor, 'SalesOfficeID': ddlsalesofficeid };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(Data, s, e);
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Permossions<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Permossions</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Permossions Details
                </h3>
            </div>
            <div class="box-body">
                <table>
                    <tr>
                        <td>
                            <label class="headers">
                                SalesOffice:</label>
                        </td>
                        <td>
                            <select id="ddlSalesOffice" class="form-control" style="width: 180px;" onchange="ddlSalesOfficeChange(this);">
                            </select>
                        </td>
                        <td>
                            <label class="headers">
                                Route Name:</label>
                        </td>
                        <td>
                            <select id="ddlRouteName" class="form-control" style="width: 180px;" onchange="ddlRouteNameChange(this);">
                            </select>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <div style="width: 100%;">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div class="box box-info" style="float: left; width: 350px; height: 330px; overflow: auto;">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Agent Details
                                        </h3>
                                    </div>
                                    <div class="box-body">
                                        <div id="divchblroutes">
                                        </div>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="box box-info" style="float: left; width: 240px; height: 330px; overflow: auto;">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Selected Agent Details
                                        </h3>
                                    </div>
                                    <div class="box-body">
                                        <div id="divselected">
                                        </div>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="box box-info">
                                    <div class="box-header with-border">
                                    </div>
                                    <div class="box-body">
                                        <table>
                                            <tr>
                                                <td>
                                                    <label>
                                                        Permission Upto</label>
                                                </td>
                                                <td style="height: 40px;">
                                                    <input type="date" id="datepicker" placeholder="DD-MM-YYYY" class="form-control" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>
                                                        Remarks:</label>
                                                </td>
                                                <td style="height: 40px;">
                                                    <input type="text" id="txtRemarks" class="form-control" placeholder="Enter Remarks" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>
                                                        Permission For</label>
                                                </td>
                                                <td style="height: 40px;">
                                                    <select id="ddlpermissionfor" class="form-control" style="width: 180px;">
                                                        <option selected="selected">Incentive</option>
                                                        <option>Indent</option>
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <input type="button" id="btnSave" value="Save" class="btn btn-primary" onclick="btn_PermissionsSaveclick();" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
