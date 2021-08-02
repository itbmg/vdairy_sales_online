<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DispatchManagement.aspx.cs" Inherits="DispatchManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <link href="css/custom.css" rel="stylesheet" type="text/css" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <script src="Js/JTemplate.js?v=3004" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);

        });
    </script>
    <script type="text/javascript">
        $(function fillroutes() {
            var data = { 'operation': 'get_all_Routes' };
            var s = function (msg) {
                if (msg) {
                    fill_allroutes(msg);
                    fiidispatchroutes(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);

        });
        function fill_allroutes(msg) {
            var routenames = document.getElementById('cmb_routename');
            var length = routenames.options.length;
            document.getElementById("cmb_routename").options.length = null;
            document.getElementById("cmb_routename").value = "select";
            var opt = document.createElement('option');
            opt.innerHTML = "Select";
            routenames.appendChild(opt);
            var sales = '<%=Session["salestype"] %>';
            var brnch = '<%=Session["branch"] %>';
            if (sales == "Plant") {
                for (var i = 0; i < msg.length; i++) {
                    if (msg[i].routename != null) {
                        if (msg[i].mainbrnch == brnch) {
                            var opt = document.createElement('option');
                            opt.innerHTML = msg[i].routename;
                            opt.value = msg[i].routesno;
                            routenames.appendChild(opt);
                        }

                    }
                }
            }
            else {
                for (var i = 0; i < msg.length; i++) {
                    if (msg[i].routename != null) {
                        var opt = document.createElement('option');
                        opt.innerHTML = msg[i].routename;
                        opt.value = msg[i].routesno;
                        routenames.appendChild(opt);
                    }
                }
            }

            binddispatches();
        }
        function fiidispatchroutes(msg) {
            document.getElementById('divdischblroutes').innerHTML = "";
            document.getElementById('divselected').innerHTML = "";
            for (var i = 0; i <= msg.length; i++) {
                if (typeof msg[i] === "undefined" || msg[i].routename == "" || msg[i].routename == null) {
                }
                else {
                    var label = document.createElement("span");
                    var hidden = document.createElement("input");
                    hidden.type = "hidden";
                    hidden.name = "hidden";
                    hidden.value = msg[i].routesno;
                    var checkbox = document.createElement("input");
                    checkbox.type = "checkbox";
                    checkbox.name = "checkbox";
                    checkbox.value = "checkbox";
                    checkbox.id = "checkbox";
                    //checkbox.className = 'checkinput';
                    checkbox.className = 'chkclass';
                    document.getElementById('divdischblroutes').appendChild(checkbox);
                    label.innerHTML = msg[i].routename;
                    document.getElementById('divdischblroutes').appendChild(label);
                    document.getElementById('divdischblroutes').appendChild(hidden);
                    document.getElementById('divdischblroutes').appendChild(document.createElement("br"));
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
                    var anchor = document.createElement("a");
                    anchor.tagName = "Click Here";

                    var Crosslabel = document.createElement("img");
                    Crosslabel.style.float = "right";
                    Crosslabel.src = "Images/Cross.png";
                    Crosslabel.onclick = function () { RemoveClick(Selectedid); };
                    label.id = Selectedid;
                    label.innerHTML = Selected + " ";

                    label.className = 'divselectedclass';
                    label.onclick = function () { divonclick(label); }
                    document.getElementById('divselected').appendChild(label);
                    var data = { 'operation': 'get_Routes_indents', 'Selectedid': Selectedid };
                    var s = function (msg) {
                        if (msg) {
                            var i = 1;
                            for (var booking in msg) {
                                var checkbox = document.createElement("input");
                                var labelindent = document.createElement("span");
                                checkbox.type = "checkbox";
                                checkbox.name = "checkbox";
                                checkbox.value = "checkbox";
                                checkbox.id = "checkbox" + i;
                                checkbox.onclick = function () { checked(this); }
                                checkbox.className = 'chkclassindent';
                                labelindent.innerHTML = msg[booking].indenttype;
                                label.appendChild(checkbox);
                                label.appendChild(labelindent);
                                i++;

                            }
                            label.appendChild(Crosslabel);
                        }
                        else {
                        }
                    };
                    var e = function (x, h, e) {
                    };
                    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                    callHandler(data, s, e);
                }
                else {
                    var Selected = $(this).next().next().val();
                    var elem = document.getElementById(Selected);
                    var p = elem.parentNode;
                    p.removeChild(elem);
                }
            });
        }
        function RemoveClick(Selected) {
            var elem = document.getElementById(Selected);
            var p = elem.parentNode;
            p.removeChild(elem);
            $('.chkclass').each(function () {
                if ($(this).next().next().val() == Selected) {
                    $(this).attr("checked", false);
                }
            });
        }
        function checked(thisid) {
        }
        var dispatchsno = "";
        function btndispatchadd_click() {
            var div = document.getElementById('divselected');
            var divs = div.getElementsByTagName('div');
            var divArray = [];
            for (var i = 0; i < divs.length; i += 1) {
                var chkid = divs[i].getElementsByTagName('input');
                for (var ind = 0; ind < chkid.length; ind += 1) {
                    if ($(chkid[ind]).is(':checked')) {
                        divArray.push(divs[i].id + '-' + $(chkid[ind]).next('span').text());
                    }
                }
            }
            var DispatchRoutename = document.getElementById("txt_dispatch_name").value;
            if (DispatchRoutename == "") {
                alert("Please Provide Dispatch Name");
                $("#txt_dispatch_name").focus();
                return false;
            }
            var Dispatchflag = document.getElementById("cmb_dispatchflag").value;
            if (Dispatchflag == "") {
                alert("Please Select Flag");
                $("#cmb_dispatchflag").focus();
                return false;
            }
            var Org_RouteNames = document.getElementById("cmb_routename").value;
            if (Org_RouteNames == "") {
                alert("Please Select Route Name");
                $("#cmb_routename").focus();
                return false;
            }
            var cmd_desptype = document.getElementById("cmd_desptype").value;
            if (cmd_desptype == "") {
                alert("Please Select DespType");
                $("#cmd_desptype").focus();
                return false;
            }
            var txtdate = document.getElementById("txtdate").value;
            if (txtdate == "") {
                alert("Please Enter No Of Days");
                $("#txtdate").focus();
                return false;
            }
            var txtdesptime = document.getElementById("txtdesptime").value;
            if (txtdesptime == "") {
                alert("Please Select Desp Time");
                $("#txtdesptime").focus();
                return false;
            }
            var txtindtime = document.getElementById("txtindtime").value;
            if (txtindtime == "") {
                alert("Please Select Ind Time");
                $("#txtindtime").focus();
                return false;
            }
            var btnaddmodify = document.getElementById('btn_dispatch_add').innerHTML;
            var Data = { 'operation': 'btn_dispatchRoutesClick', 'DispatchRoutename': DispatchRoutename, 'Org_RouteNames': Org_RouteNames, 'dataarr': divArray, 'btnaddmodify': btnaddmodify, 'Dispatchflag': Dispatchflag, 'dispatchsno': dispatchsno, 'desptype': cmd_desptype, 'NoOfdays': txtdate, 'desptime': txtdesptime, 'indtime': txtindtime };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    binddispatches();
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

            document.getElementById("txt_dispatch_name").value = "";
            document.getElementById("cmb_routename").selectedIndex = 0;
            document.getElementById('btn_dispatch_add').innerHTML = "Save"
            document.getElementById('divselected').innerHTML = "";
            document.getElementById("cmb_dispatchflag").selectedIndex = 0;
            document.getElementById("cmd_desptype").selectedIndex = 0;
            document.getElementById("txtdate").value = "";
            $('.chkclass').each(function () {
                $(this).attr("checked", false);
            });
        }
        function binddispatches() {
            var data = { 'operation': 'updatedispatchestogrid' };
            var s = function (msg) {
                if (msg) {
                    bindingdispatches(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {

            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

            callHandler(data, s, e);
        }
        var dispatchsno = "";
        function bindingdispatches(databind) {
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">Dispatch Name</th><th scope="col" class="thcls">MainRouteName</th><th scope="col" class="thcls">Flag</th><th scope="col" class="thcls">Despatch Date</th><th scope="col" class="thcls">Despatch Type</th><th scope="col" class="thcls">Despatch Time</th><th scope="col" class="thcls">Indent Time</th><th scope="col" class="thcls">Route Description</th><th scope="col"></th></tr></thead></tbody>';
            for (var Booking in databind) {
                var status = 'InActive';
                if (databind[Booking].flag == '1') {
                    status = 'Active';
                }
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row"  class="1 tdmaincls" >' + databind[Booking].RouteName + '</td>';
                results += '<td data-title="Capacity"  class="2 tdmaincls">' + databind[Booking].Mainroute + '</td>';
                results += '<td data-title="Capacity" class="3">' + status + '</td>';
                results += '<td data-title="Capacity" class="4">' + databind[Booking].DespDate + '</td>';
                results += '<td data-title="Capacity" class="5">' + databind[Booking].DespType + '</td>';
                results += '<td data-title="Capacity" class="6">' + databind[Booking].DespTime + '</td>';
                results += '<td data-title="Capacity" class="7">' + databind[Booking].IndTime + '</td>';
                results += '<td data-title="Capacity" class="8">' + databind[Booking].DistributorName + '</td>';
                results += '<td style="display:none" class="9">' + databind[Booking].RefNo + '</td>';
                results += '<td><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';

                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_Deapatchdata").html(results);
        }
        function getme(thisid) {
            document.getElementById('divselected').innerHTML = "";
            var RouteName = $(thisid).parent().parent().children('.1').html();
            var Mainroute = $(thisid).parent().parent().children('.2').html();
            var status = $(thisid).parent().parent().children('.3').html();
            var DespDate = $(thisid).parent().parent().children('.4').html();
            var DespType = $(thisid).parent().parent().children('.5').html();
            var DespTime = $(thisid).parent().parent().children('.6').html();
            var IndTime = $(thisid).parent().parent().children('.7').html();
            var DistributorName = $(thisid).parent().parent().children('.8').html();
            var refno = $(thisid).parent().parent().children('.9').html();
            dispatchsno = refno;
            $("#cmb_routename").find("option:contains('" + Mainroute + "')").each(function () {
                if ($(this).text() == Mainroute) {
                    $(this).attr("selected", "selected");
                }
            });
            document.getElementById('txt_dispatch_name').value = RouteName;
            document.getElementById('btn_dispatch_add').innerHTML = "MODIFY";
            document.getElementById('cmb_dispatchflag').value = status;
            var strDispType = DespType;
            if (strDispType == "Sales Office") {
                strDispType = "SO";
            }
            if (strDispType == "Direct Routes") {
                strDispType = "SM";
            }
            document.getElementById('cmd_desptype').value = strDispType;
            document.getElementById('txtdate').value = DespDate;
            document.getElementById('txtdesptime').value = DespTime;
            document.getElementById('txtindtime').value = IndTime;
            var data = { 'operation': 'updatedispatchselected', 'dispatchsno': dispatchsno };
            var s = function (msg) {
                if (msg) {
                    gridselctionchanged(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {

            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function gridselctionchanged(msg) {
            var Selected = "";
            var Selectedid = "";
            var subroutesno = "";
            var indenttype = "";
            $('.chkclass').each(function () {
                $(this).attr("checked", false);
            });
            for (var vi = 0; vi < msg.length; vi++) {
                $('.chkclass').each(function () {
                    if (msg.length == 0) {
                        if (vi == 0) {
                            subroutesno = msg[vi].subroutesno;
                            indenttype = msg[vi].IndentType;
                        }
                    }
                    else {
                        subroutesno = msg[vi].subroutesno;
                        indenttype = msg[vi].IndentType;
                    }
                    if ($(this).next().next().val() == msg[vi].subroutesno) {
                        $(this).attr("checked", true);
                        Selected = msg[vi].subroutename + " ";
                        Selectedid = msg[vi].subroutesno;
                        var label = document.createElement("div");
                        var Crosslabel = document.createElement("img");
                        Crosslabel.style.float = "right";
                        Crosslabel.src = "Images/Cross.png";
                        Crosslabel.onclick = function () { RemoveClick(Selectedid); };
                        label.id = Selectedid;
                        label.innerHTML = Selected;
                        label.className = 'divselectedclass';
                        //label.onclick = function () { divonclick(label); }
                        document.getElementById('divselected').appendChild(label);
                        var data = { 'operation': 'get_Routes_indents', 'Selectedid': Selectedid };
                        var s = function (msg1) {
                            if (msg1) {
                                var i = 1;
                                for (var booking in msg1) {
                                    var checkbox = document.createElement("input");
                                    var labelindent = document.createElement("span");
                                    checkbox.type = "checkbox";
                                    checkbox.name = "checkbox";
                                    checkbox.value = "checkbox";
                                    checkbox.id = "checkbox" + i;
                                    checkbox.onclick = function () { checked(this); }
                                    checkbox.className = 'chkclassindent';
                                    labelindent.innerHTML = msg1[booking].indenttype;
                                    label.appendChild(checkbox);
                                    label.appendChild(labelindent);
                                    if (msg1[booking].routesno == subroutesno) {
                                        $('.chkclassindent').each(function () {
                                            if ($(this).next().text() == indenttype) {
                                                $(this).attr("checked", true);
                                            }
                                        });
                                    }
                                    i++;

                                }
                                label.appendChild(Crosslabel);
                            }
                            else {
                            }
                        };
                        var e = function (x, h, e) {

                        };
                        $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                        callHandler(data, s, e);
                    }
                });
            }

        }
        function divonclick(selected) {
            selectedindex = selected;
            if ($(selected).css('background-color') == 'rgb(255, 255, 255)' || $(selected).css('background-color') == 'rgba(0, 0, 0, 0)') {
                $('.divselectedclass').each(function () {
                    $(this).css('background-color', '#ffffff');
                });
                $(selected).css('background-color', '#ffffcc');
            }
            else {
                $('.divselectedclass').each(function () {
                    $(this).css('background-color', '#ffffff');
                });
            }
        }
        function btnUpClick() {
            $(selectedindex).insertBefore($(selectedindex).prev());
        }
        function btnDownClick() {
            $(selectedindex).insertAfter($(selectedindex).next());
        }
        function btndispatchesDeleteclick() {
            if (!confirm("Do you really want to delete")) {
                return false;
            }
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

        function showdispatch() {
            //            forclearall();
            $("#div_dispatchMobile").css("display", "none");
            $("#div_Dispatch").css("display", "block");
        }
        function showDispatchMobile() {
            //            forclearall1();
            get_Plant_Despatches();
            get_DisppatchMobienumbers();
            $("#div_Dispatch").css("display", "none");
            $("#div_dispatchMobile").css("display", "block");
        }
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
                alert("Please Select DispatchName");
                $("#ddldispname").focus();
                return false;
            }
            var Name = document.getElementById("txtName").value;
            if (Name == "") {
                alert("Please Enter Name");
                $("#txtName").focus();
                return false;
            }
            var MobileNumber = document.getElementById("txtMobileNumber").value;
            if (MobileNumber == "") {
                alert("Please Enter MobileNumber");
                $("#txtMobileNumber").focus();
                return false;
            }
            //            var msgtype = document.getElementById("ddlmsgtype").value;
            var msgtype = ddlmsgtype.options[ddlmsgtype.selectedIndex].innerHTML;
            if (msgtype == "") {
                alert("Please Select Message Type");
                $("#ddlmsgtype").focus();
                return false;
            }
            var Checkexist = false;
            var emailid = document.getElementById("txtEmailid").value;
            $('.spnMobilenumbers').each(function (i, obj) {
                var Number=$(this).find('#spnMobilenumbers').text()
                var Number = $(this).text();
                if (Number == MobileNumber) {
//                    alert("MobileNumber Already Added");
//                    Checkexist = true;
                }
            });
//            $('.clsmsgtype').each(function (i, obj) {
//                var type = $(this).text();
//                if (type == msgtype) {
//                    alert("MobileNumber Already Added");
//                    Checkexist = true;
//                }
//            });
            if (Checkexist == true) {
                return;
            }
            else {
                DispatchMobileArr.push({ DispatchName: DispatchName, Name: Name, MobileNumber: MobileNumber, emailid: emailid, msgtype: msgtype, dispatchid: dispatchid });
                var results = '<div  ><table id="tableInvoiceFormdetails" class="responsive-table">';
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
            opt.innerHTML = "Select DispatchName";
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
            var btnval = document.getElementById("btn_save").innerHTML;
            var ddldispname = document.getElementById("ddldispname").value;
            if (ddldispname == "" || ddldispname == "Select DispatchName") {
                alert("Please Select Dispatch Name");
                Checkexist = true;
            }
            var Mobilnumarr = new Array();
            $(rows).each(function (i, obj) {
                Mobilnumarr.push({ mobilenumber: $(this).find('#spnMobilenumbers').text(), dispatchname: $(this).find('#hdndispatchid').val(), name: $(this).find('#spnName').text(), emailid: $(this).find('#spnEmailid').text(), msgtype: $(this).find('#spnmsgtype').text(), sno: $(this).find('#hdnsno').val() });
            });
            if (Mobilnumarr.length == "0") {
                alert("Please enter Dispatch Mobile Numbers");
                return false;
            }
            var Data = { 'operation': 'saveDispatchMobileNumbers', 'Mobilnumarr': Mobilnumarr, 'btnval': btnval, 'dispno': ddldispname, };
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
            var J = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var results = '<div  style="overflow:auto;"><table class="responsive-table">';
            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">DispatchName</th><th scope="col"></th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[J] + '">';
                results += '<td scope="row" class="1 tdmaincls" >' + msg[i].dispatchname + '</td>';
                results += '<td style="display:none" class="2">' + msg[i].dispatchid + '</td>';
                results += '<td style="display:none" class="3">' + msg[i].Name + '</td>';
//                results += '<td scope="row" class="4"><i class="glyphicon glyphicon-phone" aria-hidden="true"></i>&nbsp;<span style="font-weight:600;" id="4">' + msg[i].MobileNumber + '</span></td>';
//                results += '<td scope="row" class="42"><i class="glyphicon glyphicon-envelope" aria-hidden="true"></i>&nbsp;<span id="42">' + msg[i].emailid + '</span></td>';
//                results += '<td  class="6">' + msg[i].msgtype + '</td>';
                results += '<td style="display:none" class="7">' + msg[i].sno + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getcoln(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';
                J = J + 1;
                if (J == 4) {
                    J = 0;
                }
            }
            results += '</table></div>';
            $("#div_data").html(results);
        }
        function getcoln(thisid) {
//            get_Plant_Despatches();
            var DispatchName = $(thisid).parent().parent().children('.1').html();
            var dispatchid = $(thisid).parent().parent().children('.2').html();
            var Name = $(thisid).parent().parent().children('.3').html();
//            var MobileNumber = $(thisid).parent().parent().find('#4').html();
//            var emailid = $(thisid).parent().parent().find('#5').html();
          //  var msgtype = $(thisid).parent().parent().children('.6').html();
            var sno = $(thisid).parent().parent().children('.7').html();
            document.getElementById('ddldispname').value = dispatchid;
            document.getElementById('btn_save').innerHTML = "Modify";
            get_DisppatchMobie_no(dispatchid);
        }
        function get_DisppatchMobie_no(sno) {
            var data = { 'operation': 'get_DisppatchMobie_no', 'DispNo': sno};
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
                var Name = msg[i].name;
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
            document.getElementById('btn_save').innerHTML = "Save";
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
            Despatch Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Masters</a></li>
            <li><a href="#">Despatch Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div>
                <ul class="nav nav-tabs">
                    <li id="id_tab_Personal" class="active"><a data-toggle="tab" href="#" onclick="showdispatch()">
                        <i class="fa fa-street-view"></i>&nbsp;&nbsp;Dispatch Details</a></li>
                    <li id="id_tab_documents" class=""><a data-toggle="tab" href="#" onclick="showDispatchMobile()">
                        <i class="fa fa-file-text"></i>&nbsp;&nbsp;Dispatch Mobile</a></li>
                </ul>
            </div>
        </div>
        <div id="div_Dispatch">
            <div class="box box-info">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Despatch Master Details
                    </h3>
                </div>
                <div class="box-body">
                    <table align="center">
                        <tr>
                            <td>
                                <label id="lbl_dispatchname">
                                    Dispatch Name:</label><span style="color: red; font-weight: bold">*</span>
                            </td>
                            <td style="height: 40px;">
                                <input type="text" id="txt_dispatch_name" class="form-control"    placeholder="Enter Dispatch Name" />
                            </td>
                            <td>
                                <label id="lbl_routename">
                                    Route Name:</label><span style="color: red; font-weight: bold">*</span>
                            </td>
                            <td style="height: 40px;">
                                <select id="cmb_routename" class="form-control">
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="lblFlag">
                                    Dispatch Type:</label><span style="color: red; font-weight: bold">*</span>
                            </td>
                            <td style="height: 40px;">
                                <select id="cmd_desptype" class="form-control">
                                    <option value="SO">Sales Office</option>
                                    <option value="SM">Direct Routes</option>
                                </select>
                            </td>
                            <td>
                                <label>
                                    Dispatch Date:</label><span style="color: red; font-weight: bold">*</span>
                            </td>
                            <td style="height: 40px;">
                                <input type="text" id="txtdate" class="form-control" placeholder="Enter No Of Dates" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Dispatch Time:</label><span style="color: red; font-weight: bold">*</span>
                            </td>
                            <td style="height: 40px;">
                                <input type="text" id="txtdesptime" value="12:00:00" class="form-control" placeholder="Enter No Of Dates"
                                    min="11:00" />
                            </td>
                      
                            <td>
                                <label>
                                    Indent Time:</label><span style="color: red; font-weight: bold">*</span>
                            </td>
                            <td style="height: 40px;">
                                <input type="text" id="txtindtime" value="12:00:00" class="form-control" placeholder="Enter No Of Dates"
                                    min="11:00" max="21:00" step="900" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="lblFlag">
                                    Flag:</label><span style="color: red; font-weight: bold">*</span>
                            </td>
                            <td style="height: 40px;">
                                <select id="cmb_dispatchflag" class="form-control">
                                    <option>Active</option>
                                    <option>InActive</option>
                                </select>
                            </td>
                        </tr>
                    </table>
                    <div style="width: 100%;">
                        <table align="center">
                            <tr>
                                <td>
                                    <div class="box box-info" style="float: left; width: 350px; height: 330px; overflow: auto;">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">
                                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Route Details
                                            </h3>
                                        </div>
                                        <div class="box-body">
                                            <div id="divdischblroutes">
                                            </div>
                                        </div>
                                    </div>
                                </td>
                                <td style="width: 10px;">
                                </td>
                                <td>
                                    <div class="box box-info" style="float: left; width: 240px; height: 330px; overflow: auto;">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">
                                                <i style="padding-right: 5px;" class="fa fa-cog"></i>Selected Route Details
                                            </h3>
                                        </div>
                                        <div class="box-body">
                                            <div id="divselected">
                                            </div>
                                        </div>
                                    </div>
                                </td>
                                <td style="width: 5px;">
                                </td>
                                <td>
                                    <input type="button" class="btnUp" onclick="btnUpClick();" /><br />
                                    <input type="button" class="btnDown" onclick="btnDownClick();" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center" style="height: 40px;">
                                    <table>
                                        <tr>
                                            <td>
                                            <div class="input-group">
                                                                <div class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-ok" id="btn_dispatch_add1" onclick="btndispatchadd_click()">
                                                                    </span><span id="btn_dispatch_add" onclick="btndispatchadd_click()">Save</span>
                                                                </div>
                                                            </div>
                                                <%--<input type="button" id="btn_dispatch_add" style=" padding: 0px 0;
                                                     text-align: center; font-size: 21px; font-weight: bold;
                                                    line-height: 1.428571429;" value="Save" class="btn btn-primary" onclick="btndispatchadd_click();" />--%>
                                            </td>
                                            <td style="padding-left: 7px;">
                                                <div class="input-group">
                                                    <div class="input-group-close">
                                                        <span class="glyphicon glyphicon-trash" id="btnDelete1" onclick="btndispatchesDeleteclick()">
                                                        </span><span id="btnDelete" onclick="btndispatchesDeleteclick()">Delete</span>
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
            <div class="box box-primary">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <i style="padding-right: 5px;" class="fa fa-list"></i>Despatch Details
                    </h3>
                </div>
                <div id="div_Deapatchdata" style="width: 100%; cursor: pointer; height: 400px; overflow: auto;">
                </div>
            </div>
        </div>
        <div id="div_dispatchMobile" style="display: none;">
            <div class="box box-info">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <i style="padding-right: 5px;" class="fa fa-cog"></i>Despatcher Mobile Details
                    </h3>
                </div>
                <div id="div_DispatchMobilenumber">
                </div>
                <table align="center">
                    <tr>
                        <td>
                            <label>
                                DispatchName</label><span style="color: red; font-weight: bold">*</span>
                            <select id="ddldispname" class="form-control" onchange="ddldispatchStats();">
                            </select>
                        </td>
                        <td style="width:2px;">
                        </td>
                        <td>
                            <label>
                                Name</label><span style="color: red; font-weight: bold">*</span>
                       
                            <input id="txtName" type="text" name="Name" class="form-control" placeholder="Enter Name" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Mobile Number</label><span style="color: red; font-weight: bold">*</span>
                       
                            <input id="txtMobileNumber" type="text" name="Street" class="form-control" placeholder="Enter Mobile Number" />
                        </td>
                        <td style="width:2px;">
                        </td>
                        <td>
                            <label>
                                E-Mailid</label>
                       
                            <input id="txtEmailid" type="text" name="Mandal" class="form-control" placeholder="Enter Mail Id" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                MSG Type</label><span style="color: red; font-weight: bold">*</span>
                       
                            <select id="ddlmsgtype" class="form-control">
                                <option value="1">Indent</option>
                                <option value="2">Dispatch</option>
                            </select>
                        </td>
                        <td style="width:2px;">
                        </td>
                        <td>
                                <input type="button" id="btn_add" style="width: 30px; height: 30px; padding: 0px 0;
                                    border-radius: 15px; text-align: center; font-size: 21px; font-weight: bold;
                                    line-height: 1.428571429;margin-left: 28px;" value="+" class="btn btn-primary" onclick="BtnAddClick();" />
                            </td>
                       
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div id="div_totalinvoiceDetails">
                            </div>
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
                                                <span class="glyphicon glyphicon-ok" id="btn_save1" onclick="saveDispatchMobileNumbers()">
                                                </span><span id="btn_save" onclick="saveDispatchMobileNumbers()">Save</span>
                                            </div>
                                        </div>
                                    </td>
                                    <td style="padding-left: 7px;">
                                        <div class="input-group">
                                            <div class="input-group-close">
                                                <span class="glyphicon glyphicon-remove" id="btn_closebank1" onclick="forclearall()">
                                                </span><span id="btn_closebank" onclick="forclearall()">Close</span>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%-- <input id="btn_save" type="button" class="btn btn-primary" name="submit" value='save'
                                onclick="saveDispatchMobileNumbers()" />
                            <input id='btn_close' type="button" class="btn btn-danger" name="Close" value='Close'
                                onclick="forclearall()" />--%>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="box box-primary">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        <i style="padding-right: 5px;" class="fa fa-list"></i>Despatcher Mobile Numbers
                        list
                    </h3>
                </div>
                <div id="div_data">
                </div>
            </div>
        </div>
    </section>
</asp:Content>
