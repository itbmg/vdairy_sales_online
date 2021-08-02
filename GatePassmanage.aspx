<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="GatePassmanage.aspx.cs" Inherits="GatePassmanage" %>

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
    <link href="bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
    <script src="bootstrap/bootstrap.min.js" type="text/javascript"></script>
    <link href="bootstrap/fleetStyles.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap/formcss.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap/custom.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="bootstrap/font-awesome.min.css" />
    <link href="bootstrap/formstable.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .chkclass
        {
            color: #080A89;
            font-size: 12px;
            float: left;
        }
        .lblclass
        {
            font-size: 13px;
            float: left;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            fillVehicles();
        });
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
        function btnGatePassDeatailsSaveclick() {
            var ddlVehicleNo = document.getElementById('ddlVehicleNo').value;
            var txtRouteName = document.getElementById('txtRouteName').value;
            var txtpartyName = document.getElementById('txtpartyName').value;
            var btnsave = document.getElementById('btnSave').value;
            var Data = { 'operation': 'btnGatePassDeatailsSaveclick', 'gridBinding': gridBinding, 'edittype': btnsave, 'VehicleNo': ddlVehicleNo, 'Name': txtpartyName, 'routename': txtRouteName };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    RefreshClick();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(Data, s, e);
        }
        function RefreshClick() {
            document.getElementById('ddlVehicleNo').selectedIndex = 0;
            document.getElementById('txtRouteName').value = "";
            document.getElementById('txtpartyName').value = "";
            gridBinding = [];
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="responsive-table"><caption></casption>';
            results += '<thead><tr><th scope="col">Ref DC No</th></tr></thead></tbody>';
            for (var i = 0; i < gridBinding.length; i++) {
                results += '<td scope="row"  class="RefDcNo">' + gridBinding[i].refno + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_vendordata").html(results);
        }
        function fillVehicles() {
            var data = { 'operation': 'GetDispatchVehicleNos' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindVehicleNo(msg);

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindVehicleNo(msg) {
            var ddlVehicleNo = document.getElementById('ddlVehicleNo');
            var length = ddlVehicleNo.options.length;
            ddlVehicleNo.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "select Vehicle No";
            ddlVehicleNo.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].VehicleNo != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].VehicleNo;
                    opt.value = msg[i].DCno;
                    ddlVehicleNo.appendChild(opt);
                }
            }
        }
        var gridBinding = [];
        function AddTogridClick() {
            var txtRefDcNo = document.getElementById('txtRefDcNo').value;
            if (txtRefDcNo == "") {
                alert("Please Enter Ref Dc No");
                return false;
            }
            var Checkexist = false;
            $('.RefDcNo').each(function (i, obj) {
                var IName = $(this).text();
                if (IName == txtRefDcNo) {
                    alert("Ref Dc No Already Added");
                    Checkexist = true;
                }
            });
            if (Checkexist == true) {
                return;
            }
            gridBinding.push({ 'refno': txtRefDcNo });
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="responsive-table">';
            results += '<thead><tr><th scope="col">Ref DC No</th></tr></thead></tbody>';
            for (var i = 0; i < gridBinding.length; i++) {
                results += '<td scope="row"  class="RefDcNo">' + gridBinding[i].refno + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_vendordata").html(results);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            GatePass Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">GatePass Master</a></li>
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
                <table align="center">
                    <tr>
                        <td>
                            Vehicle No
                        </td>
                        <td style="height: 40px;">
                            <select id="ddlVehicleNo" class="form-control">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Route Name
                        </td>
                        <td style="height: 40px;">
                            <input type="text" id="txtRouteName" class="form-control" placeholder="Enter Route Name" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Party Name
                        </td>
                        <td style="height: 40px;">
                            <input type="text" id="txtpartyName" class="form-control" placeholder="Enter  Party Name" />
                        </td>
                    </tr>
                </table>
                <table align="center">
                    <tr>
                        <td>
                            Ref Dc No
                        </td>
                        <td style="height: 40px;">
                            <input type="text" id="txtRefDcNo" class="form-control" placeholder="Enter Ref Dc No" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="div_vendordata" style="background: #ffffff">
                            </div>
                        </td>
                    </tr>
                </table>
                <table align="center">
                    <tr>
                        <td>
                            <input type="button" class="btn btn-success" name="submit" class="btn btn-primary"
                                id="btn_save" value='Add' onclick="AddTogridClick()" />
                        </td>
                   <td style="width:5px;">
                   
                   </td>
                        <td>
                            <input type="button" id="btnSave" value="Save" onclick="btnGatePassDeatailsSaveclick();"
                                class="btn btn-primary" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </section>
</asp:Content>
