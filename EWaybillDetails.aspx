<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EWaybillDetails.aspx.cs" Inherits="EWaybillDetails" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="Js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .ddlDropStatus {
            width: 100%;
            height: 34px;
            border: 1px solid gray;
            border-radius: 6px 6px 6px 6px;
            font-size: 16px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);

            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#txt_Docdate').val(today);
            $('.divsalesOffice').css('display', 'table-row');
            FillSalesOffice();

        });
        function FillSalesOffice() {
            var data = { 'operation': 'GetSalesOffice' };
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
            var ddlsalesOffice = document.getElementById('ddlsalesOffice');
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



        function GenerateClick() {
            var FromDate = document.getElementById('txtFromDate').value;
            if (FromDate == "") {
                alert("Please From Date");
                return false;
            }


            var branchID = document.getElementById("ddlsalesOffice").value;

            var data = { 'operation': 'GetEWayDetails', 'FromDate': FromDate, 'BranchID': branchID };
            var s = function (msg) {
                if (msg) {

                    //$('#divEWayBilldata').removeTemplate();
                    //$('#divEWayBilldata').setTemplateURL('EWayBill.htm');
                    //$('#divEWayBilldata').processTemplate(msg);
                    fillewaydetails(msg);
                    UpdateVehicles();

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function fillewaydetails(msg) {
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info" ID="tabledetails">';
            results += '<thead><tr><th scope="col">AgentName</th><th scope="col">InvoiceNo</th><th scope="col">Distance</th><th scope="col">DocumentNo</th><th scope="col">DocumentDate</th><th scope="col">VehcleNo</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr>';
                results += '<td ><input id="spnagentname" type="text" class="form-control"  placeholder= "Name"  style="text-transform:capitalize;"  value="' + msg[i].AgentName + '"/></td>';
                results += '<td style="display:none;" ><input id="spnagentid"  type="hidden"  class="form-control"  value="' + msg[i].Agentid + '"/></td>';
                results += '<td ><input id="spninvoice"  type="text" class="form-control"    value="' + msg[i].InvoiceNo + '"/></td>';
                results += '<td style="display:none;"><input id="spnhdninvoiceno" type="hidden" class="form-control"  style="display:none;"  value="' + msg[i].hdnInvoiceno + '"/></td>';
                results += '<td ><input id="txtDistance" type="text" class="form-control" placeholder="Enter Distance" style="" value="' + msg[i].Distance + '"/></td>';
                results += '<td ><input id="txtDocNumber" type="text" class="form-control"  placeholder="Enter Ewd bill No" style=""  value="' + msg[i].DocNumber + '"/></td>';
                results += '<td ><input id="txt_Docdate" type="date" class="form-control"  style=""  value="' + msg[i].DocDate + '"/></td>';
                results += '<td ><input id="ddlvehcle" class="accountnocls form-control" placeholder="Select Vehcle Number" type="text"  class="form-control"  style=""  value="' + msg[i].VehcleNo + '"/></td>';
                results += '<td style="display:none;" ><input id="spnvehicleid"  type="hidden"  class="form-control"  value="' + msg[i].vehcleid + '"/></td>';
                //results += '<td data-title="Phosps"><select id="ddlvehcle" class="accountnocls" style="width:100% !important;" ><option  selected value="' + msg[i].VehcleNo + '"/option></select></td>';
                results += '<th data-title="From"><input class="form-control" type="hidden"  id="txt_sno"  name="nationalty" value="' + msg[i].sno + '" ></input>';
                if (msg[i].DocNumber == "") {
                    results += '<td><input type="button" id="btnValue" value="Save" onclick="btnEwayBillSaveClick(this);" class="btn btn-primary" /></td>';
                }
                else {
                    results += '<td><input type="button" id="btnValue" value="Update" onclick="btnEwayBillSaveClick(this);" class="btn btn-primary" /></td>';

                }
                results += '<td style="display:none" class="4">' + i + '</td></tr>';
            }
            results += '</table></div>';
            $("#divEWayBilldata").html(results);
        }

        
       
        //function UpdateVehicles() {
        //    var data = { 'operation': 'GetBranchVehicles' };
        //    var s = function (msg) {
        //        if (msg) {
        //            BindVehicles=msg;
        //        }
        //        else {
        //        }
        //    };
        //    var e = function (x, h, e) {
        //    };
        //    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        //    callHandler(data, s, e);
        //}
        var BindVehicles = [];
        function UpdateVehicles() {
            var data = { 'operation': 'GetBranchVehicles' };
            var s = function (msg) {
                if (msg) {
                    BindVehicles = msg;

                    var VehiclesList = [];
                    for (var i = 0; i < msg.length; i++) {
                        var VehicleNo = msg[i].VehicleNo;
                        VehiclesList.push(VehicleNo);
                    }
                    $('.accountnocls').autocomplete({
                        source: VehiclesList,
                        change: vehclenamechange,
                        autoFocus: true
                    });
                }
            }
            var e = function (x, h, e) {
                alert(e.toString());
            };
            callHandler(data, s, e);
        }

        function vehclenamechange() {
            var vehcleid = document.getElementById("ddlvehcle").value;
            for (var i = 0; i < BindVehicles.length; i++) {
                var VehicleNo = BindVehicles[i].VehicleNo;
                if (vehcleid == VehicleNo) {
                    document.getElementById("spnvehicleid").value = BindVehicles[i].sno;
                }

            }
        }
        





        function btnEwayBillSaveClick(id) {

            var FromDate = document.getElementById('txtFromDate').value;
            var soid = document.getElementById('ddlsalesOffice').value;

            var invoiceno = $(id).closest("tr").find('#spnhdninvoiceno').val();

            var Distance = $(id).closest("tr").find('#txtDistance').val();
            if (Distance == "") {
                alert("Enter Distance");
                return false;
            }
            var DocNumber = $(id).closest("tr").find('#txtDocNumber').val();
            if (DocNumber == "") {
                alert("Enter E-way Bill No");
                return false;
            }
            var EwaybillDate = $(id).closest("tr").find('#txt_Docdate').val();
            if (EwaybillDate == "") {
                alert("Select E - waybillDate");
                return false;
            }
            var vehcleno = $(id).closest("tr").find('#ddlvehcle').val();
            var vehicleid = $(id).closest("tr").find('#spnvehicleid').val();
            var agentid = $(id).closest("tr").find('#spnagentid').val();
            var sno = $(id).closest("tr").find('#txt_sno').val();
            var btnValue = $(id).closest("tr").find('#btnValue').val();
            
            if (!confirm("Do you really want Save")) {
                return false;
            }

            var data = { 'operation': 'btnEwayBillSaveClick', 'soid': soid, 'agentid': agentid, 'invoiceno': invoiceno, 'Distance': Distance, 'DocNumber': DocNumber, 'EwaybillDate': EwaybillDate, 'vehcleno': vehcleno, 'IndDate': FromDate, 'vehicleid': vehicleid, 'sno': sno, 'btnValue': btnValue };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    if (msg == "Session Expired") {
                        window.location = "Login.aspx";
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
        <h1>Cheque Approval<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Cheque Approval</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Cheque Approval Details
                </h3>
            </div>
            <div class="box-body">
                <div style="width: 100%; background-color: #fff">
                    <div>
                        <table>
                            <tr>
                                <td class="divsalesOffice" style="display: none;">
                                    <select id="ddlsalesOffice" class="form-control">
                                    </select>
                                </td>
                                <td>Fromdate
                                </td>
                                <td style="width: 5px;"></td>
                                <td>
                                    <input type="date" id="txtFromDate" class="form-control" />
                                </td>
                                <td style="width: 5px;"></td>
                                <td>
                                    <input type="button" id="btnGenerate" value="Generate" onclick="GenerateClick();"
                                        class="btn btn-primary" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divEWayBilldata">
                    </div>

                </div>
            </div>
        </div>
    </section>
</asp:Content>

