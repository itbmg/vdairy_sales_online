<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="smstransactionentry.aspx.cs" Inherits="smstransactionentry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <style>
        input[type=checkbox]
        {
            transform: scale(1.5);
        }
        input[type=checkbox]
        {
            width: 30px;
            height: 23px !important;
            margin-right: 8px;
            cursor: pointer;
            font-size: 10px;
            visibility: hidden;
        }
        input[type=checkbox]:after
        {
            content: " ";
            background-color: #fff;
            display: inline-block;
            margin-left: 10px;
            padding-bottom: 0px;
            color: #24b6dc;
            width: 16px;
            height: 18px;
            visibility: visible;
            border: 1px solid rgba(18, 18, 19, 0.12);
            padding-left: 3px;
            border-radius: 0px;
        }
        input[type="checkbox"]:not(:disabled):hover:after
        {
            border: 1px solid #24b6dc;
        }
        input[type=checkbox]:checked:after
        {
            content: "\2714";
            padding: -5px;
            font-weight: bold;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);
        });
    </script>
    <script type="text/javascript">
        $(function () {
            fillemployes();
            fillmsgtypes();
            FillSalesOffices();
        });
        function FillSalesOffices() {
            var data = { 'operation': 'GetSalesOffice' };
            var s = function (msg) {
                if (msg) {
                    BindSalesOfficeNames(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindSalesOfficeNames(msg) {
            document.getElementById('divlocation').innerHTML = "";
            var divstudents = document.getElementById('divlocation');
            var tablestrctr = document.createElement('table');
            tablestrctr.id = "tablelocdetails";
            tablestrctr.setAttribute("class", "location-table");
            $(tablestrctr).append('<thead></thead><tbody></tbody>');
            for (var i = 0; i < msg.length; i++) {
                $(tablestrctr).append('<tr><td><input type="checkbox" class="checkedlocid" id="ckbpresent" onclick="selectall_checks(this)";  value="' + msg[i].Sno + '"></td><td class="1">' + msg[i].BranchName + '</td></tr>');
            }
            $(tablestrctr).append('<br/>');
            $(tablestrctr).append('<br/>');
            $(tablestrctr).append('<br/>');
            divstudents.appendChild(tablestrctr);
        }

        function selectall_checks(thisid) {
            if ($(thisid).is(':checked')) {
                $(this).find(':checkbox').prop('checked', true);
            }
            else {
                $(this).find(':checkbox').prop('checked', true);
            }
        }

        //naveen
        function fillemployes() {
            var data = { 'operation': 'get_smsemp_details' };
            var s = function (msg) {
                if (msg) {
                    bindemployedetails(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
                if (x.status && x.status == 400) {
                    alert(x.responseText);
                    window.location.assign("Login.aspx");
                }
                else {
                    alert("something went wrong");
                }
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function bindemployedetails(msg) {
            var divchblroutes = document.getElementById('divchblroutes');
            var tablestrctr = document.createElement('table');
            tablestrctr.id = "tableempdetails";
            tablestrctr.setAttribute("class", "emp-table");
            $(tablestrctr).append('<thead></thead><tbody></tbody>');
            for (var i = 0; i < msg.length; i++) {
                $(tablestrctr).append('<tr><td><input type="checkbox" class="checkedid" id="ckbpresent" onclick="selectall_checks(this)";  value="' + msg[i].sno + '"></td><td class="1">' + msg[i].Empname + '</td></tr>');
            }
            $(tablestrctr).append('<br/>');
            $(tablestrctr).append('<br/>');
            $(tablestrctr).append('<br/>');
            divchblroutes.appendChild(tablestrctr);
        }


        function fillmsgtypes() {
            var data = { 'operation': 'get_smstype_details' };
            var s = function (msg) {
                if (msg) {
                    bindmsgtype(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
                if (x.status && x.status == 400) {
                    alert(x.responseText);
                    window.location.assign("Login.aspx");
                }
                else {
                    alert("something went wrong");
                }
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function bindmsgtype(msg) {
            var divmsgtype = document.getElementById('divmsgtype');
            var tablestrctr = document.createElement('table');
            tablestrctr.id = "tablemsgdetails";
            tablestrctr.setAttribute("class", "emp-table");
            $(tablestrctr).append('<thead></thead><tbody></tbody>');
            for (var i = 0; i < msg.length; i++) {
                $(tablestrctr).append('<tr><td><input type="checkbox" class="checkedmsgid" id="ckbpresent" onclick="selectall_checks(this)";  value="' + msg[i].msgtype + '"></td><td class="1">' + msg[i].msgtype + '</td></tr>');
            }
            $(tablestrctr).append('<br/>');
            $(tablestrctr).append('<br/>');
            $(tablestrctr).append('<br/>');
            divmsgtype.appendChild(tablestrctr);
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


        function save() {
            var employelist = [];
            var msgtypelist = [];
            var locationlist = [];
            var count = 0;
            var rows = $("#tableempdetails tr:gt(0)");
            var rowsno = 1;
            var remarks = "";
            $("input:checkbox[class=checkedid]:checked").each(function () {
                var empid = $(this).val();
                var abc = { employee: empid};
                employelist.push(abc);
            });

            var rowmsgs = $("#tablemsgdetails tr:gt(0)");
            var rowmsgsno = 1;
            $("input:checkbox[class=checkedmsgid]:checked").each(function () {
                var msg = $(this).val();
                var abc = { msgtype: msg};
                msgtypelist.push(abc);
            });

            var rows = $("#tablelocdetails tr:gt(0)");
            var rowsno = 1;
            var remarks = "";
            $("input:checkbox[class=checkedlocid]:checked").each(function () {
                var location = $(this).val();
                var soidname = $(this).next().next().text();
                if (soidname == null || soidname == "") {
                    return;
                }
                var abc = { soid: location, soidname: soidname };
                locationlist.push(abc);

            });
            var btnsave = document.getElementById('btnSave').innerHTML;
            var Data = { 'operation': 'btnSavesmsClick', 'employelist': employelist, 'msgtypelist': msgtypelist, 'locationlist': locationlist, 'btnsave': btnsave };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    Bindsmstransmsgtype();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(Data, s, e);
        }


        function Bindsmstransmsgtype() {
            var data = { 'operation': 'get_smstransmsgtype_details' };
            var s = function (msg) {
                if (msg) {
                   
                }
                else {
                }
            };
            var e = function (x, h, e) {
                if (x.status && x.status == 400) {
                    alert(x.responseText);
                    window.location.assign("Login.aspx");
                }
                else {
                    alert("something went wrong");
                }
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Route Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Masters</a></li>
            <li><a href="#">Route Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Route Master Details
                </h3>
            </div>
            <div class="box-body">
                <div style="width: 100%;">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div class="box box-info" style="float: left; width: 240px; height: 330px; overflow: auto;">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Employe Details
                                        </h3>
                                    </div>
                                    <div class="box-body">
                                        <div id="divchblroutes">
                                        </div>
                                    </div>
                                </div>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <div class="box box-info" style="float: left; width: 250px; height: 330px; overflow: auto;">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <i style="padding-right: 5px;" class="fa fa-cog"></i>MsgType Details
                                        </h3>
                                    </div>
                                    <div class="box-body">
                                        <div id="divmsgtype">
                                        </div>
                                    </div>
                                </div>
                            </td>
                            <td style="width: 5px;">
                            </td>
                            <td>
                                <div class="box box-info" style="float: left; width: 250px; height: 330px; overflow: auto;">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Location Details
                                        </h3>
                                    </div>
                                    <div class="box-body">
                                        <div id="divlocation">
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
                                        <div id="divmsgselected">
                                        </div>
                                        <div id="divlocselected">
                                        </div>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <input type="button" class="btnUp" onclick="btnUpClick();" /><br />
                                <input type="button" class="btnDown" onclick="btnDownClick();" />
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
                <div>
                    <div class="input-group">
                        <div class="input-group-addon">
                            <span class="glyphicon glyphicon-ok" id="btn_brnch_prodtuct_save1" onclick="save()">
                            </span><span id="btnSave" onclick="save()">Save</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-list"></i>Route list
                </h3>
            </div>
            <div id="div_Routedata" style="width: 100%; cursor: pointer; height: 400px; overflow: auto;">
            </div>
        </div>
    </section>
</asp:Content>
