<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="GatePass.aspx.cs" Inherits="GatePass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <link href="Css/style.css" rel="stylesheet" type="text/css" />
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3004" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <style type="text/css">
        .ddlsize
        {
            width: 280px;
            height: 30px;
            font-size: 16px;
            border: 1px solid gray;
            border-radius: 7px 7px 7px 7px;
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
            window.history.forward(1);

        });
    </script>
    <script language="javascript" type="text/javascript">
        function GetGatePassDetails() {
            var gatepasssno = document.getElementById('txtgatepasssno').value;
            if (gatepasssno == "") {
                alert("Please Gate Pass no");
                return false;
            }
            //            document.getElementById('txtvehicleNo').text(VehicleNo);
            var data = { 'operation': 'GetGatePassDetails', 'gatepasssno': gatepasssno };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    if (msg == "No data found") {
                        alert(msg);
                        return false;
                    }

                    $('#divDetils').removeTemplate();
                    $('#divDetils').setTemplateURL('GatepassDetails.htm');
                    $('#divDetils').processTemplate(msg);
                    //                    document.getElementById('txtvehicleNo').innerHTML = VehicleNo
                    //                    document.getElementById('txtDate').innerHTML = msg[0].Date;
                    //                    document.getElementById('txtTime').innerHTML = msg[0].Time;
                    //                    document.getElementById('txtName').innerHTML = msg[0].UserName;

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BtnVarifyVehicleClick() {
            var gatepasssno = document.getElementById('txtgatepasssno').value;
            if (gatepasssno == "") {
                alert("Please Enter Gate Pass No");
                return false;
            }
            var data = { 'operation': 'BtnVarifyVehicleClick', 'gatepasssno': gatepasssno };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    //                    fillVehicles();
                    $('#divDetils').removeTemplate();
                    $('#divDetils').setTemplateURL('GatepassDetails.htm');
                    $('#divDetils').processTemplate();
                    document.getElementById('txtvehicleNo').innerHTML = "";
                    document.getElementById('txtDate').innerHTML = "";
                    document.getElementById('txtTime').innerHTML = "";
                    document.getElementById('txtName').innerHTML = "";
                }
                else {
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
        //        function CallPrint(strid) {
        //            var divToPrint = document.getElementById(strid);
        //            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
        //            newWin.document.open();
        //            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
        //            newWin.document.close();
        //        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            GatePass Report<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">GatePass Report</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>GatePass Details
                </h3>
            </div>
            <div class="box-body">
                <div>
                    <table>
                        <tr>
                            <td>
                                <label for="lblBranch">
                                    Gate Pass No</label>
                            </td>
                            <td>
                                <input type="text" id="txtgatepasssno" class="form-control" placeholder="Gate Pass No" />
                            </td>
                            <td style="width:5px;"></td>
                            <td>
                                <input type="button" id="Button1" value="Get Details" class="btn btn-primary"
                                    onclick="GetGatePassDetails();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPrint">
                    <div style="width: 100%;">
                        <div style="width: 13%; float: left;">
                            <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="95px" height="90px" />
                        </div>
                        <div align="center">
                            <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="20px" ForeColor="#0252aa"
                                Text=""></asp:Label>
                            <br />
                            <asp:Label ID="lblAddress" runat="server" Font-Bold="true" Font-Size="12px" ForeColor="#0252aa"
                                Text=""></asp:Label>
                            <br />
                        </div>
                        <div align="center">
                            <span style="font-size: 16px; font-weight: bold; text-decoration: underline; color: #0252aa;">
                                GATE PASS DETAILS</span><br />
                            <br />
                            Tin No.
                            <asp:Label ID="lbltinNo" runat="server" Font-Bold="true" Font-Size="14px" ForeColor="#0252aa"
                                Text=""></asp:Label>
                        </div>
                        <div>
                            <div style="width: 90%;">
                                <div style="width: 60%; float: left; line-height: 2;">
                                    <span>To,</span>
                                    <br />
                                    <span>The Security</span>
                                    <br />
                                    <span>Name Of the Person : </span><span id="txtName" style="color: Red; font-weight: bold;
                                        font-size: 14px;"></span>
                                </div>
                                <div style="left: 61%; position: absolute; line-height: 2;">
                                    <span style="width: 100px; margin-top: 2px;">Date</span>
                                    <br />
                                    <span style="width: 100px; margin-top: 2px; line-height: 14px;">Time</span>
                                    <br />
                                    <span style="width: 100px; margin-top: 2px;">Vehicle No</span>
                                </div>
                                <div style="left: 71%; position: absolute; line-height: 2;">
                                    <span id="txtDate" style="width: 100px; margin-top: 2px; color: Red; font-weight: bold;
                                        font-size: 14px;"></span>
                                    <br />
                                    <span id="txtTime" style="width: 100px; margin-top: 2px; color: Red; font-weight: bold;
                                        font-size: 14px;"></span>
                                    <br />
                                    <span id="txtvehicleNo" style="width: 100px; margin-top: 2px; color: Red; font-weight: bold;
                                        font-size: 14px;"></span>
                                </div>
                            </div>
                            <br />
                            <div id="divDetils" style="width: 90%;">
                            </div>
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 50%; float: left; font-weight: bold; font-size: 12px;">
                                        <br />
                                        <br />
                                        Receiver's Signature
                                    </td>
                                    <td style="width: 49%; float: right; font-weight: bold; font-size: 12px;">
                                        <br />
                                        <br />
                                        Issuing Authority
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </div>
                    </div>
                </div>
                <input type="button" id="BtnVarifyVehicle" value="Varify" class="btn btn-primary" onclick="BtnVarifyVehicleClick();"/>
                <br />
                <br />
            </div>
        </div>
    </section>
</asp:Content>
