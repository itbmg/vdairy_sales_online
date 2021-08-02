<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="statemastar.aspx.cs" Inherits="statemastar" %>

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
    <style type="text/css">
         <style>
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
        
.icon-btn {
    padding: 1px 13px 3px 2px;
    border-radius:50px;
}

label {
    display: inline-block;
    max-width: 100%;
    margin-bottom: 5px;
    font-weight: 700;
}

.glyphicon {
    position: relative;
    top: 1px;
    display: inline-block;
    font-family: 'Glyphicons Halflings';
    font-style: normal;
    font-weight: 400;
    line-height: 1;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
}
    </style>
    <script type="text/javascript">

        $(function () {
            get_state_details();
        });
        //Address Details
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        function ValidateAlpha(evt) {
            var keyCode = (evt.which) ? evt.which : evt.keyCode
            if ((keyCode < 65 || keyCode > 90) && (keyCode < 97 || keyCode > 123) && keyCode != 32)

                return false;
            return true;
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
        function savestateDetails() {
            var statename = document.getElementById('txt_state').value;
            if (statename == "") {
                alert("Enter  statename");
                $("#txt_state").focus();
                return false;
            }

            var statecode = document.getElementById('txt_statecode').value;
            if (statecode == "") {
                alert("Enter statecode");
                $("#txt_statecode").focus();
                return false;
            }
            var ecode = document.getElementById('txt_ecode').value;
            if (ecode == "") {
                alert("Enter  ecode");
                $("#txt_ecode").focus();
                return false;
            }
            var gststatecode = document.getElementById('txt_gststatecode').value;
            if (gststatecode == "") {
                alert("Enter  gststatecode");
                $("#txt_gststatecode").focus();
                return false;
            }
            var btnval = document.getElementById('btn_stateSave').innerHTML;
            var sno = document.getElementById('lbl_sno').value;
            var data = { 'operation': 'savestateDetails', 'statename': statename, 'statecode': statecode, 'ecode': ecode, 'gststatecode': gststatecode, 'sno': sno, 'btnVal': btnval };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        alert(msg);
                        forclearall();
                        get_state_details();
                        $("#div_AddressData").show();
                        $("#Address_fillform").hide();
                        $('#Addresshowlogs').show();
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
        function forclearall() {
            document.getElementById('txt_state').value = "";
            document.getElementById('txt_statecode').value = "";
            document.getElementById('txt_ecode').value = "";
            document.getElementById('txt_gststatecode').value = "";
            document.getElementById('btn_stateSave').innerHTML = "Save";
        }
        function get_state_details() {
            var data = { 'operation': 'get_state_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillstates(msg);
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
        function fillstates(msg) {
            var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr style="background:#5aa4d0; color: white; font-weight: bold;"><th scope="col"></th><th scope="col">State Name</th><th scope="col">State Code</th><th scope="col">E Code</th><th scope="col">GST State Code</th></tr></thead></tbody>';
            var k = 1;
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '"><td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmeaddress(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>';
                results += '<th scope="row" class="1" >' + msg[i].statename + '</th>';
                results += '<td data-title="brandstatus"  class="2">' + msg[i].statecode + '</td>';
                results += '<td data-title="brandstatus"  class="3">' + msg[i].ecode + '</td>';
                results += '<td data-title="brandstatus" class="4">' + msg[i].gststatecode + '</td>';
                results += '<td style="display:none" class="9">' + msg[i].sno + '</td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_AddressData").html(results);
        }
        function getmeaddress(thisid) {
            var statename = $(thisid).parent().parent().children('.1').html();
            var statecode = $(thisid).parent().parent().children('.2').html();
            var ecode = $(thisid).parent().parent().children('.3').html();
            var gststatecode = $(thisid).parent().parent().children('.4').html();
            var sno = $(thisid).parent().parent().children('.9').html();
            document.getElementById('txt_state').value = statename;
            document.getElementById('txt_statecode').value = statecode;
            document.getElementById('txt_ecode').value = ecode;
            document.getElementById('txt_gststatecode').value = gststatecode;
            document.getElementById('btn_stateSave').innerHTML = "Modify";
            document.getElementById('lbl_sno').value = sno;
            $("#div_AddressData").hide();
            $("#Address_fillform").show();
            $('#Addresshowlogs').hide();
        }
        function showstatedesign() {
            $("#div_AddressData").hide();
            $("#Address_fillform").show();
            $('#Addresshowlogs').hide();
        }
        function cancelstatedetails() {
            $("#div_AddressData").show();
            $("#Address_fillform").hide();
            $('#Addresshowlogs').show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content">
        <!-- Small boxes (Stat box) -->
        <div class="row">
            <section class="content-header">
                <h1>
                    Mini Masters
                </h1>
                <ol class="breadcrumb">
                    <li><a href="#"><i class="fa fa-dashboard"></i>Operation</a></li>
                    <li><a href="#">Masters</a></li>
                </ol>
            </section>
            <section class="content">
                <div class="box box-info">
                    <div class="box-header with-border">
                    </div>
                    <div class="box-body">
                        <div id="div_Address">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>State Details
                                </h3>
                            </div>
                            <div class="box-body">
                                <%--<div id="Addresshowlogs" align="center">
                                        <input id="btn_addAddress" type="button" name="submit" value='AddAddress' class="btn btn-primary"
                                            onclick="showAddressdesign()" />
                                    </div>--%>
                                <div id="Addresshowlogs" align="right" class="input-group" style="display: block;">
                                    <div class="input-group-addon" style="width: 100px;">
                                        <span class="glyphicon glyphicon-plus-sign" onclick="showstatedesign();"></span>
                                        <span onclick="showstatedesign();">Add State</span>
                                    </div>
                                </div>
                                <div id="div_AddressData">
                                </div>
                                <div id='Address_fillform' style="display: none;">
                                    <table align="center">
                                        <tr>
                                            <td>
                                                <label>
                                                    State Name</label><span style="color: red; font-weight: bold">*</span>
                                                <input id="txt_state" type="text" name="CompanyName" class="form-control" placeholder="Enter State Name" />
                                            </td>
                                            <td style="width: 4px;">
                                            </td>
                                            <td>
                                                <label>
                                                    State Code</label><span style="color: red; font-weight: bold">*</span>
                                                <input id="txt_statecode" type="text" name="CustomerCode" class="form-control"
                                                    placeholder="Enter State Code" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    E Code</label><span style="color: red; font-weight: bold">*</span>
                                                <input id="txt_ecode" type="text" name="CompanyName" class="form-control" placeholder="Enter  E Code" />
                                            </td>
                                            <td style="width: 4px;">
                                            </td>
                                            <td>
                                                <label>
                                                   Gst State Code</label><span style="color: red; font-weight: bold">*</span>
                                                <input id="txt_gststatecode" type="text" name="CustomerCode" class="form-control"
                                                    placeholder="Enter Gst State Code" />
                                            </td>
                                        </tr>
                                        
                                        <tr style="display: none;">
                                            <td>
                                                <label id="lbl_sno">
                                                </label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center" style="height: 40px;">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div class="input-group">
                                                                <div class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-ok" id="btn_AddressSave1" onclick="savestateDetails()">
                                                                    </span><span id="btn_stateSave" onclick="savestateDetails()">Save</span>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td style="padding-left: 7px;">
                                                            <div class="input-group">
                                                                <div class="input-group-close">
                                                                    <span class="glyphicon glyphicon-remove" id="btnAddressCancel1" onclick="cancelstatedetails()">
                                                                    </span><span id="btnAddressCancel" onclick="cancelstatedetails()">Close</span>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
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

