<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="VehicleMaster.aspx.cs" Inherits="VehicleMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            UpdateVehicles();
        });

        function Vehiclevalidation() {
            var VehicleNumber = document.getElementById('txtVehicleNumber').value;
            var txtCapacity = document.getElementById('txtCapacity').value;
            if (VehicleNumber == "") {
                alert("Enter Vehicle Number");
                $("#txtVehicleNumber").focus();
                return false;
            }
            if (txtCapacity == "") {
                alert("Enter Capacity");
                $("#txtCapacity").focus();
                return false;
            }
            var sno = serial;
            var btnval = document.getElementById('btnSave').innerHTML;
            var data = { 'operation': 'SaveVehicleMasterClick', 'sno': sno, 'btnval': btnval, 'VehicleNo': VehicleNumber, 'Capacity': txtCapacity };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    UpdateVehicles();
                    vehcleforclearall();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function UpdateVehicles() {
            var data = { 'operation': 'GetBranchVehicles' };
            var s = function (msg) {
                if (msg) {
                    BindVehicles(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var serial = 0;
        function BindVehicles(msg) {
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">VehicleNo</th><th scope="col" class="thcls">Capacity</th><th scope="col"></th></tr></thead></tbody>';
            var j = 1;
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row" class="1"><i class="fa fa-truck" style="color: cadetblue;" aria-hidden="true"></i>&nbsp;<span class="tdmaincls" id="1">' + msg[i].VehicleNo + '</span></td>';
//                results += '<td scope="row" class="1" >' + msg[i].VehicleNo + '</td>';
                results += '<td data-title="Capacity" class="2">' + msg[i].Capacity + '</td>';
                results += '<td data-title="Capacity" style="display:none;"  class="3">' + msg[i].sno + '</td>';
                results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getvehcledata(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';

                j++;
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_Vehdata").html(results);
        }
        function getvehcledata(thisid) {
            var VehicleNo = $(thisid).parent().parent().find('#1').html();
//            var VehicleNo = $(thisid).parent().parent().children('.1').html();
            var Capacity = $(thisid).parent().parent().children('.2').html();
            var sno = $(thisid).parent().parent().children('.3').html();

            document.getElementById('txtCapacity').value = Capacity;

            document.getElementById('txtVehicleNumber').value = VehicleNo;
            document.getElementById('btnSave').innerHTML = "MODIFY";
            serial = sno;
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
        function vehcleforclearall() {
            document.getElementById('txtVehicleNumber').value = "";
            document.getElementById('txtCapacity').value = "";
            document.getElementById('btnSave').innerHTML = "SAVE";

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Vehicle Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Vehicle Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Vehicle Master Details
                </h3>
            </div>
            <div class="box-body">
                <table align="center">
                    <tr>
                        <td>
                         <label>
                            Vehicle Number<span style="color: red; font-weight: bold">*</span>
                            </label>
                        </td>
                        <td style="height: 40px;">
                            <input type="text" id="txtVehicleNumber" class="form-control" placeholder="Enter Vehicle Number" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <label>
                            Capacity<span style="color: red; font-weight: bold">*</span>
                            </label>
                        </td>
                        <td style="height: 40px;">
                            <input type="text" id="txtCapacity" class="form-control" placeholder="Enter Capacity" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                         <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div class="input-group">
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-ok" id="btnSave1" onclick="Vehiclevalidation()">
                                                    </span><span id="btnSave" onclick="Vehiclevalidation()">SAVE</span>
                                                </div>
                                            </div>
                                                        </td>
                                                        <td style="padding-left: 7px;">
                                                            <div class="input-group">
                                                                <div class="input-group-close">
                                                                    <span class="glyphicon glyphicon-remove" id='btvehcleclear1' onclick="vehcleforclearall()">
                                                                    </span><span id='btvehcleclear' onclick="vehcleforclearall()">CLEAR</span>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                        <%--<td>
                            <div class="input-group">
                                                <div class="input-group-addon">
                                                    <span class="glyphicon glyphicon-ok" id="btnSave1" onclick="Vehiclevalidation()">
                                                    </span><span id="btnSave" onclick="Vehiclevalidation()">SAVE</span>
                                                </div>
                                            </div>
                        </td>--%>
                    </tr>
                </table>
                <br />
                <div id="div_Vehdata" style="width: 100%; text-align: center; height: 400px; overflow: auto;">
                </div>
            </div>
        </div>
    </section>
</asp:Content>
