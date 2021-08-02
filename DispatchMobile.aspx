<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DispatchMobile.aspx.cs" Inherits="DispatchMobile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <link href="css/custom.css" rel="stylesheet" type="text/css" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
     <script src="Js/JTemplate.js?v=3004" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            get_Plant_Despatches();
            get_DisppatchMobienumbers();
        });
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
        function CallHandlerUsingJson(d, s, e) {
            d = JSON.stringify(d);
            d = encodeURIComponent(d);
            $.ajax({
                type: "GET",
                url: " DairyFleet.axd?json=",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: d,
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }
        var DispatchMobileArr = [];
        function BtnAddClick() {
            $("#div_totalinvoiceDetails").css("display", "block");
            var DispatchName = ddldispname.options[ddldispname.selectedIndex].innerHTML;
            var dispatchid = document.getElementById("ddldispname").value;
            if (DispatchName == "Select DispatchName" || DispatchName == "") {
                alert("Select DispatchName");
                return false;
            }
            var Name = document.getElementById("txtName").value;
            var MobileNumber = document.getElementById("txtMobileNumber").value;
            //            var msgtype = document.getElementById("ddlmsgtype").value;
            var msgtype = ddlmsgtype.options[ddlmsgtype.selectedIndex].innerHTML;
            var Checkexist = false;
            var emailid = document.getElementById("txtEmailid").value;
            $('.MobileNumbercls').each(function (i, obj) {
                var Number = $(this).text();
                if (Number == MobileNumber) {
                    alert("MobileNumber Already Added");
                    Checkexist = true;
                }
            });
            if (Checkexist == true) {
                return;
            }
            else {
                DispatchMobileArr.push({ DispatchName: DispatchName, Name: Name, MobileNumber: MobileNumber, emailid: emailid, msgtype: msgtype, dispatchid: dispatchid });
                var results = '<div  style="overflow:auto;"><table id="tableInvoiceFormdetails" class="responsive-table">';
                results += '<thead><tr><th scope="col"></th><th scope="col">MobileNumber</th><th scope="col">DispatchName</th><th scope="col">Name</th><th scope="col">EmailId</th><th scope="col">MSG Type</th></tr></thead></tbody>';
                for (var i = 0; i < DispatchMobileArr.length; i++) {
                    results += '<tr><td></td>';
                    results += '<td scope="row" class="1" style="text-align:center;"><span id="spnMobilenumbers" style="font-size: 16px; color: Red;  font-weight: bold;" class="MobileNumbercls"><b>' + DispatchMobileArr[i].MobileNumber + '</b></span></td>';
                    results += '<td class="2"><span id="spnDispatchName" style="font-size: 16px; color: green; font-weight: bold;"class="clsdispatchname"><b>' + DispatchMobileArr[i].DispatchName + '</b></span></td>';
                    results += '<td class="2"><span id="spnName" style="font-size: 16px; color: green; font-weight: bold;"class="clsname"><b>' + DispatchMobileArr[i].Name + '</b></span></td>';
                    results += '<td class="2"><span id="spnEmailid" style="font-size: 16px; color: green; font-weight: bold;"class="clsEmailid"><b>' + DispatchMobileArr[i].emailid + '</b></span></td>';
                    results += '<td class="2"><span id="spnmsgtype" style="font-size: 16px; color: green; font-weight: bold;"class="clsmsgtype"><b>' + DispatchMobileArr[i].msgtype + '</b></span></td>';
                    results += '<td style="display:none" class="7"><input type="hidden" id="hdndispatchid" value="' + DispatchMobileArr[i].dispatchid + '" /></td>';
                    results += '<td style="display:none" class="6"><input type="hidden" id="hdnsno" value="' + DispatchMobileArr[i].sno + '"/></td>';
                    results += '<td  class="6"> <img src="Images/Odclose.png" onclick="RowDeletingClick(this);" style="cursor:pointer;" width="30px" height="30px" alt="Edit" title="Edit Qty"/> </td></tr>';
                }
                results += '</table></div>';
                $("#div_totalinvoiceDetails").html(results);
            }
        }
        function get_Plant_Despatches() {
            var data = { 'operation': 'get_Plant_Despatches' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    Bindempname(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function Bindempname(msg) {
            var ddldispname = document.getElementById('ddldispname');
            var length = ddldispname.options.length;
            ddldispname.options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Branch Name";
            ddldispname.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].RouteName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].RouteName;
                    opt.value = msg[i].Route_id;
                    ddldispname.appendChild(opt);
                }
            }
        }
        function RowDeletingClick(Account) {
            DispatchMobileArr = [];
            var MobileNumber = "";
            var Name = "";
            var number = $(Account).closest("tr").find("#spnMobilenumbers").text();
            var DispatchName = "";
            var rows = $("#tableInvoiceFormdetails tr:gt(0)");
            $(rows).each(function (i, obj) {
                if ($(this).find('#spnMobilenumbers').text() != "") {
                    MobileNumber = $(this).find('#spnMobilenumbers').text();
                    DispatchName = $(this).find('#spnDispatchName').text();
                    Name = $(this).find('#spnName').text();
                    emailid = $(this).find('#spnEmailid').text();
                    msgtype = $(this).find('#spnmsgtype').text();
                    dispatchid = $(this).find('#hdndispatchid').text();
                    sno = $(this).find('#hdnsno').text();
                    if (number == MobileNumber) {
                    }
                    else {
                        DispatchMobileArr.push({ DispatchName: DispatchName, Name: Name, MobileNumber: MobileNumber, emailid: emailid, msgtype: msgtype, dispatchid: dispatchid, sno: sno });
                    }
                }
            });
            var results = '<div style="overflow:auto;"><table id="tableInvoiceFormdetails" class="responsive-table">';
            results += '<thead><tr><th scope="col"></th><th scope="col">MobileNumber</th><th scope="col">DispatchName</th><th scope="col">Name</th><th scope="col">EmailId</th><th scope="col">MSG Type</th></tr></thead></tbody>';
            for (var i = 0; i < DispatchMobileArr.length; i++) {
                results += '<tr><td></td>';
                results += '<td scope="row" class="1" style="text-align:center;"><span id="spnMobilenumbers" style="font-size: 16px; color: Red;  font-weight: bold;" class="MobileNumbercls"><b>' + DispatchMobileArr[i].MobileNumber + '</b></span></td>';
                results += '<td class="2"><span id="spnDispatchName" style="font-size: 16px; color: green; font-weight: bold;"class="clsdispatchname"><b>' + DispatchMobileArr[i].DispatchName + '</b></span></td>';
                results += '<td class="2"><span id="spnName" style="font-size: 16px; color: green; font-weight: bold;"class="clsname"><b>' + DispatchMobileArr[i].Name + '</b></span></td>';
                results += '<td class="2"><span id="spnEmailid" style="font-size: 16px; color: green; font-weight: bold;"class="clsEmailid"><b>' + DispatchMobileArr[i].emailid + '</b></span></td>';
                results += '<td class="2"><span id="spnmsgtype" style="font-size: 16px; color: green; font-weight: bold;"class="clsmsgtype"><b>' + DispatchMobileArr[i].msgtype + '</b></span></td>';
                results += '<td style="display:none" class="7"><input type="hidden" id="hdndispatchid" value="' + DispatchMobileArr[i].dispatchid + '" /></td>';
                results += '<td style="display:none" class="6"><input type="hidden" id="hdnsno" value="' + DispatchMobileArr[i].sno + '"/></td>';
                results += '<td  class="6"> <img src="Images/Odclose.png" onclick="RowDeletingClick(this);" style="cursor:pointer;" width="30px" height="30px" alt="Edit" title="Edit Qty"/> </td></tr>';
            }
            results += '</table></div>';
            $("#div_totalinvoiceDetails").html(results);
        }
        function saveDispatchMobileNumbers() {
            var rows = $("#tableInvoiceFormdetails tr:gt(0)");
            var btnval = document.getElementById("btn_save").value;
            var Mobilnumarr = new Array();
            $(rows).each(function (i, obj) {
                Mobilnumarr.push({ mobilenumber: $(this).find('#spnMobilenumbers').text(), dispatchname: $(this).find('#hdndispatchid').val(), name: $(this).find('#spnName').text(), emailid: $(this).find('#spnEmailid').text(), msgtype: $(this).find('#spnmsgtype').text(), sno: $(this).find('#hdnsno').val() });
            });
            if (Mobilnumarr.length == "0") {
                alert("Please enter Dispatch Mobile Numbers");
                return false;
            }
            var Data = { 'operation': 'saveDispatchMobileNumbers', 'Mobilnumarr': Mobilnumarr, 'btnval': btnval };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    forclearall();
                }
            }
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(Data, s, e);
        }
        function get_DisppatchMobienumbers() {
            var data = { 'operation': 'get_DisppatchMobienumbers' };
            var s = function (msg) {
                if (msg) {
                    filldispatchmobilenumbers(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function filldispatchmobilenumbers(msg) {
            var results = '<div  style="overflow:auto;"><table class="responsive-table">';
            results += '<thead><tr><th scope="col"></th><th scope="col">DispatchName</th><th scope="col">Name</th><th scope="col">MobileNumber</th><th scope="col">EmailId</th><th scope="col">MSG Type</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr><td><input id="btn_poplate" type="button"  onclick="getcoln(this)" name="submit" class="btn btn-primary" value="Edit" /></td>';
                results += '<th scope="row" class="1" style="text-align:center;">' + msg[i].dispatchname + '</th>';
                results += '<td style="display:none" class="2">' + msg[i].dispatchid + '</td>';
                results += '<td  class="3">' + msg[i].Name + '</td>';
                results += '<td  class="4">' + msg[i].MobileNumber + '</td>';
                results += '<td   class="5">' + msg[i].emailid + '</td>';
                results += '<td class="6">' + msg[i].msgtype + '</td>';
                results += '<td style="display:none" class="7">' + msg[i].sno + '</td></tr>';
            }
            results += '</table></div>';
            $("#div_data").html(results);
        }
        function getcoln(thisid) {
            var DispatchName = $(thisid).parent().parent().children('.1').html();
            var dispatchid = $(thisid).parent().parent().children('.2').html();
            var Name = $(thisid).parent().parent().children('.3').html();
            var MobileNumber = $(thisid).parent().parent().children('.4').html();
            var emailid = $(thisid).parent().parent().children('.5').html();
            var msgtype = $(thisid).parent().parent().children('.6').html();
            var sno = $(thisid).parent().parent().children('.7').html();
            document.getElementById('btn_save').value = "Modify";
            get_DisppatchMobie_no(sno);
        }
        function get_DisppatchMobie_no(sno) {
            var data = { 'operation': 'get_DisppatchMobie_no', 'sno': sno };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        filldetails(msg);
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
        function filldetails(msg) {
            $("#div_totalinvoiceDetails").css("display", "block");
            DispatchMobileArr = [];
            for (var i = 0; i < msg.length; i++) {
                var sno = msg[i].sno;
                var dispatchid = msg[i].dispatchid;
                var MobileNumber = msg[i].MobileNumber;
                var emailid = msg[i].emailid;
                var msgtype = msg[i].msgtype;
                var Name = msg[i].Name;
                var DispatchName = msg[i].dispatchname;
                DispatchMobileArr.push({ DispatchName: DispatchName, Name: Name, MobileNumber: MobileNumber, emailid: emailid, msgtype: msgtype, dispatchid: dispatchid, sno: sno });
            }
            var results = '<div style="overflow:auto;"><table id="tableInvoiceFormdetails" class="responsive-table">';
            results += '<thead><tr><th scope="col"></th><th scope="col">MobileNumber</th><th scope="col">DispatchName</th><th scope="col">Name</th><th scope="col">EmailId</th><th scope="col">MSG Type</th></tr></thead></tbody>';
            for (var i = 0; i < DispatchMobileArr.length; i++) {
                results += '<tr><td></td>';
                results += '<td scope="row" class="1" style="text-align:center;"><span id="spnMobilenumbers" style="font-size: 16px; color: Red;  font-weight: bold;" class="MobileNumbercls"><b>' + DispatchMobileArr[i].MobileNumber + '</b></span></td>';
                results += '<td class="2"><span id="spnDispatchName" style="font-size: 16px; color: green; font-weight: bold;"class="clsdispatchname"><b>' + DispatchMobileArr[i].DispatchName + '</b></span></td>';
                results += '<td class="2"><span id="spnName" style="font-size: 16px; color: green; font-weight: bold;"class="clsname"><b>' + DispatchMobileArr[i].Name + '</b></span></td>';
                results += '<td class="2"><span id="spnEmailid" style="font-size: 16px; color: green; font-weight: bold;"class="clsEmailid"><b>' + DispatchMobileArr[i].emailid + '</b></span></td>';
                results += '<td class="2"><span id="spnmsgtype" style="font-size: 16px; color: green; font-weight: bold;"class="clsmsgtype"><b>' + DispatchMobileArr[i].msgtype + '</b></span></td>';
                results += '<td style="display:none" class="7"><input type="hidden" id="hdndispatchid" value="' + DispatchMobileArr[i].dispatchid + '" /></td>';
                results += '<td style="display:none" class="6"><input type="hidden" id="hdnsno" value="' + DispatchMobileArr[i].sno + '"/></td>';
                results += '<td  class="6"> <img src="Images/Odclose.png" onclick="RowDeletingClick(this);" style="cursor:pointer;" width="30px" height="30px" alt="Edit" title="Edit Qty"/> </td></tr>';
            }
            results += '</table></div>';
            $("#div_totalinvoiceDetails").html(results);
        }
        function forclearall() {
            document.getElementById('ddldispname').selectedIndex = 0;
            document.getElementById('txtName').value = "";
            document.getElementById('txtMobileNumber').value = "";
            document.getElementById('txtEmailid').value = "";
            document.getElementById('ddlmsgtype').selectedIndex = 0;
            document.getElementById('btn_save').value = "Save";
            DispatchMobileArr = [];
            var results = '<div style="overflow:auto;"><table id="tableInvoiceFormdetails" class="responsive-table">';
            results += '<thead-><tr><th scope="col"></th><th scope="col">MobileNumber</th><th scope="col">DispatchName</th><th scope="col">Name</th><th scope="col">EmailId</th><th scope="col">MSG Type</th></tr></thead></tbody>';
            for (var i = 0; i < DispatchMobileArr.length; i++) {
                results += '<tr><td></td>';
                results += '<td scope="row" class="1" style="text-align:center;"><span id="spnMobilenumbers" style="font-size: 16px; color: Red;  font-weight: bold;" class="MobileNumbercls"><b>' + DispatchMobileArr[i].MobileNumber + '</b></span></td>';
                results += '<td class="2"><span id="spnDispatchName" style="font-size: 16px; color: green; font-weight: bold;"class="clsdispatchname"><b>' + DispatchMobileArr[i].DispatchName + '</b></span></td>';
                results += '<td class="2"><span id="spnName" style="font-size: 16px; color: green; font-weight: bold;"class="clsname"><b>' + DispatchMobileArr[i].Name + '</b></span></td>';
                results += '<td class="2"><span id="spnEmailid" style="font-size: 16px; color: green; font-weight: bold;"class="clsEmailid"><b>' + DispatchMobileArr[i].emailid + '</b></span></td>';
                results += '<td class="2"><span id="spnmsgtype" style="font-size: 16px; color: green; font-weight: bold;"class="clsmsgtype"><b>' + DispatchMobileArr[i].msgtype + '</b></span></td>';
                results += '<td style="display:none" class="7"><input type="hidden" id="hdndispatchid" value="' + DispatchMobileArr[i].dispatchid + '" /></td>';
                results += '<td style="display:none" class="6"><input type="hidden" id="hdnsno" value="' + DispatchMobileArr[i].sno + '"/></td>';
                results += '<td  class="6"> <img src="Images/Odclose.png" onclick="RowDeletingClick(this);" style="cursor:pointer;" width="30px" height="30px" alt="Edit" title="Edit Qty"/> </td></tr>';
            }
            results += '</table></div>';
            $("#div_totalinvoiceDetails").html(results);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Dispatche MobileNumbers
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operation</a></li>
            <li><a href="#">Dispatche MobileNumbers</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Dispatche MobileNumbers
                </h3>
            </div>
            <div class="box-body">
                <div id="div_DispatchMobilenumber">
                </div>
                    <table align="center">
                        <tr>
                            <td>
                                <label>
                                    DispatchName</label>
                            </td>
                             <td>
                            <select id="ddldispname" class="form-control" onchange="ddldispatchStats();">
                                        </select>
                            </td>
                            <td>
                               <label>
                                    Name</label>
                            </td>
                            <td>
                                <input id="txtName" type="text" name="Name" class="form-control" placeholder="Enter Name" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Mobile Number</label>
                            </td>
                            <td style="height: 40px;">
                                <input id="txtMobileNumber" type="text" name="Street" class="form-control"  placeholder="Enter Mobile Number"  />
                            </td>
                            <td>
                                <label>
                                    E-Mailid</label>
                            </td>
                            <td>
                                <input id="txtEmailid" type="text" name="Mandal" class="form-control" placeholder="Enter Mail Id"/>
                            </td>
                            </tr>
                            <tr>
                            
                            <td>
                                <label>
                                    MSG Type</label>
                            </td>
                                    <td>
                                        <select id="ddlmsgtype" class="form-control">
                                            <option value="1">Indent</option>
                                            <option value="2">Dispatch</option>
                                        </select>
                                    </td>
                                    <td></td>
                            <td>
                                <input id="btn_add" type="button" onclick="BtnAddClick();" class="btn btn-success"
                                    name="Add" value='Add' />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div id="div_totalinvoiceDetails">
                                </div>
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td>
                                <label id="lbl_sno"  >
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center" style="height: 40px;">
                                <input id="btn_save" type="button" class="btn btn-primary" name="submit" value='save'
                                    onclick="saveDispatchMobileNumbers()" />
                                <input id='btn_close' type="button" class="btn btn-danger" name="Close" value='Close'
                                    onclick="forclearall()" />
                            </td>
                        </tr>
                    </table>
                <div id="div_data">
                </div>
            </div>
        </div>
    </section>
</asp:Content>
