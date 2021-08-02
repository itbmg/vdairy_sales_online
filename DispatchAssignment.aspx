﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DispatchAssignment.aspx.cs" Inherits="DispatchAssignment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <link href="jquery.jqGrid-4.5.2/ui.Jquery.css" rel="stylesheet" type="text/css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="jquery-ui-1.10.3.custom/js/jquery-1.9.1.js" type="text/javascript"></script>
    <link href="jquery.jqGrid-4.5.2/js/i18n/jquery-ui-1.9.2.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="jquery-ui-1.10.3.custom/css/ui-lightness/jquery-ui-1.10.3.custom.css"
        rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <link href="jquery.jqGrid-4.5.2/js/i18n/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <script src="jquery.jqGrid-4.5.2/src/i18n/grid.locale-en.js" type="text/javascript"></script>
    <script src="jquery.jqGrid-4.5.2/js/jquery.jqGrid.min.js" type="text/javascript"></script>
    <link href="jquery.jqGrid-4.5.2/js/Jquery.ui.css.css" rel="stylesheet" type="text/css" />
    <script src="jquery-ui-1.10.3.custom/js/jquery-ui.js" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);

        });
    </script>
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
    </style>
    <script type="text/javascript">
        $(function () {
            FillBranches();
            FillEmployeeName();
            bindmanageroutes();
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

        });

        function FillBranches() {
            var data = { 'operation': 'get_Pending_dispatches' };
            // var data = { 'operation': 'Initilize_branchmapping' };
            var s = function (msg) {
                if (msg) {
                    fillroutes_divchklist(msg);
                    //counts = 0;
                    //BindCategoriesss(msg);
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
        function BindCategoriesss(data) {
            //$("#divcategiriesdata").unload();

            $("#divchblroutes").html("");
            var prnt = $("#divchblroutes");
            for (var i = 0; i < data.length; i++) {
                if (data[i].brncName != null) {

                    filltrees(data[i].sno, data[i].brncName, data[i].lstbrnch, prnt);
                }
            }
            TabclassClick();
        }
        var counts = 0;
        function filltrees(sno, branchname, lstbrnch, liparent) {
            var ZZZ = branchname;
            var branchnm = ZZZ.split(" ").join("_");
            liparent.append("<div id='div" + branchnm + "' class='divcategory'>");
            if (counts == 0) {
                $("#div" + branchnm + "").append("<div class='titledivcls'><table style='width:100%;'><tr style='width:100%;'><td style='width:1%;'></td><td><h2 class='unitline'>" + branchnm + "</h2></td><td style='padding-right: 15px;vertical-align: left;'><span class='iconminus' title='Hide' onclick='minusclick(this);'></span></td></tr></table></div>");
            }
            counts++;
            $("#div" + branchnm + "").append("<ul id='ul" + branchnm + "' class='ulclass'>");
            for (var j = 0; j < lstbrnch.length; j++) {
                if (lstbrnch[j].lstbrnch.length > 0) {
                    //                       $("#div" + branchnm + "").append("<table style='width:100%;'><tr><td></td><td style='padding-right: 20px;vertical-align: middle;'><span class='iconminus' title='Hide' onclick='minusclick(this);'></span></td></tr></table></div>");
                    $("#ul" + branchnm + "").append("<div class='uldiv'><table style='width:100%;'><tr><td><li><a class='activeanchor'><input autocomplete='off' class='checkinput' type='checkbox' value=" + lstbrnch[j].sno + "  /><span class='livalue'>" + lstbrnch[j].brncName + "</span></a></li></td><td style='padding-right: 20px;vertical-align: left;'><span class='iconminusli' title='Hide' onclick='liminusclick(this);'></span></td></tr></table></div>");
                }
                else {
                    $("#ul" + branchnm + "").append("<li><a class='activeanchor'><input autocomplete='off' class='checkinput' type='checkbox' value=" + lstbrnch[j].sno + " /><span class='livalue'>" + lstbrnch[j].brncName + "</span></a></li>");
                }
                var prnts = $("#ul" + branchnm + "");
                filltrees(lstbrnch[j].sno, lstbrnch[j].brncName, lstbrnch[j].lstbrnch, prnts);
            }
        }
        function Ravi(thisid) {
            $('.checkinput').each(function (i, obj) {
                if ($(this).val() == thisid.value) {
                    $(this).attr('checked', 'checked');
                }
                else {
                    $(this).removeAttr('checked')
                }
            });
        }
        var div_uncheck_Array = [];
        var div_check_Array = [];
        var div_prevlist_Array = [];
        function fillroutes_divchklist(msg) {
            document.getElementById('divchblroutes').innerHTML = "";
            for (var i = 0; i <= msg.length; i++) {
                if (typeof msg[i] === "undefined" || msg[i].Name == "" || msg[i].Name == null) {
                }
                else {
                    var label = document.createElement("span");
                    var hidden = document.createElement("input");
                    hidden.type = "hidden";
                    hidden.name = "hidden";
                    hidden.value = msg[i].id;
                    var checkbox = document.createElement("input");
                    checkbox.type = "checkbox";
                    checkbox.name = "checkbox";
                    checkbox.value = msg[i].id;
                    checkbox.id = "checkbox";

                    //checkbox.className = 'checkinput';
                    checkbox.className = 'chkclass';
                    document.getElementById('divchblroutes').appendChild(checkbox);
                    label.innerHTML = msg[i].Name;
                    document.getElementById('divchblroutes').appendChild(label);
                    document.getElementById('divchblroutes').appendChild(hidden);
                    document.getElementById('divchblroutes').appendChild(document.createElement("br"));
                }
            }
        }


        function gridselctionchanged(msg) {
            div_prevlist_Array = [];
            $('.chkclass').each(function () {
                $(this).attr("checked", false);
            });
            for (var i = 0; i < msg.length; i++) {
                $('.chkclass').each(function () {
                    if ($(this).next().next().val() == msg[i].BranchID) {
                        $(this).attr("checked", true);
                        var Selected = msg[i].DistributorName;
                        var Selectedid = msg[i].BranchID;
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
                        div_prevlist_Array.push(Selectedid);
                    }
                });
            }

        }





        function btndispatchassignment() {
            var div = document.getElementById('divchblroutes');
            var divs = div.getElementsByTagName('div');
            var divArray = [];
            for (var i = 0; i < divs.length; i += 1) {
                divArray.push(divs[i].id);
            }

            var subcategorystrng = [];
            var checkinputs = $('#divchblroutes').find('.chkclass');
            checkinputs.each(function (list) {
                var checkbox = checkinputs[list];
                if (checkbox.checked) {
                    divArray.push(checkbox.value);
                }
            });



            if (divArray.length == 0) {
                alert("Please Select Dispatches To Assign");
                return false;
            }
            var routename = document.getElementById('ddlemployeeName').value;
            if (routename == "") {
                alert("Please select Employee Name");
                return false;
            }

            var elt = document.getElementById('ddlemployeeName');

            if (elt.selectedIndex == -1)
                return false;


            var Employeename = elt.options[elt.selectedIndex].text;
            if (Employeename == "") {
                alert("Please select Employee Name");
                return false;
            }
            var mobile = document.getElementById('txtMobileno').innerHTML;
            if (mobile == "") {
                alert("Please Update Mobile Number");
                return false;
            }
            var btnsave = document.getElementById('btnSave').value;
            //var Data = { 'operation': 'btnRoutesSaveClick', 'routename': routename, 'dataarr': divArray, 'divindentArray': divindentArray, 'div_uncheck_Array': div_uncheck_Array, 'div_check_Array': div_check_Array, 'btnsave': btnsave, 'refno': refno };
            var Data = { 'operation': 'btndispatchassign', 'routename': routename, 'dataarr': divArray, 'Mobile': mobile, 'clubbingname': Employeename, 'btnsave': btnsave, 'refno': refno };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    bindmanageroutes();

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

            CallHandlerUsingJson(Data, s, e);

        }

        function bindmanageroutes() {
            // var data = { 'operation': 'updateroutestogrid' };
            var data = { 'operation': 'updateAssignedEmployee' };
            var s = function (msg) {
                if (msg) {
                    bindingmanageroutes(msg);
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
        var refno = "";
        function bindingmanageroutes(databind) {
            $("#grd_routesmangelist").jqGrid("clearGridData");
            var newarray = [];
            var Headarray = [];
            var headdatacol = databind[1];
            var datacol = databind;
            for (var Booking in databind) {

                newarray.push({ 'Sno': newarray.length + 1, 'Route Name': databind[Booking].RouteName, 'RouteIndents': databind[Booking].Indenttype, 'Route Description': databind[Booking].DistributorName, 'RefNo': databind[Booking].RefNo });
            }
            $("#grd_routesmangelist").jqGrid({
                datatype: "local",
                height: 150,
                width: 'auto',
                //overflow-x:'auto',
                overflow: 'auto',
                colNames: Headarray,
                colModel: [{ name: 'Sno', index: 'invdate', width: 90, sortable: false, align: 'center' },
        { name: 'Route Name', index: 'invdate', width: 170, sortable: false, align: 'center' },
        { name: 'RouteIndents', index: 'invdate', width: 170, sortable: false, align: 'center' },
        { name: 'Route Description', index: 'invdate', width: 600, sortable: false },
        { name: 'RefNo', index: 'invdate', width: 170, sortable: false, align: 'center', hidden: true}],
                rowNum: 10,
                rowList: [5, 10, 30],
                // rownumbers: true,
                gridview: true,
                loadonce: true,
                pager: "#page4",
                caption: "Routes Manage"
            }).jqGrid('navGrid', '#page4', { edit: false, add: false, del: false, search: false, refresh: false });
            var mydata = newarray;
            for (var i = 0; i <= mydata.length; i++) {

                jQuery("#grd_routesmangelist").jqGrid('addRowData', i + 1, mydata[i]);
            }
            $("#grd_routesmangelist").jqGrid('setGridParam', { onSelectRow: function (rowid, iRow, iCol, e) {

                var routename = $('#grd_routesmangelist').getCell(rowid, 'Route Name');
                var routedescription = $('#grd_routesmangelist').getCell(rowid, 'Route Description');
                document.getElementById('divselected').innerHTML = "";
                document.getElementById('txtRouteName').value = routename;
                document.getElementById('btnSave').value = "MODIFY";

                refno = $('#grd_routesmangelist').getCell(rowid, 'RefNo');
                var data = { 'operation': 'updatedivselected', 'refno': refno };
                var s = function (msg) {
                    if (msg) {
                        gridselctionchanged(msg);
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
                var data = { 'operation': 'updatedivindents', 'refno': refno };
                var s = function (msg) {
                    if (msg) {
                        gridselctionchangedindent(msg);
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
        function FillEmployeeName() {
            var data = { 'operation': 'GetEmployeenames' };
            var s = function (msg) {
                if (msg) {
                    BindAgentName(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindAgentName(msg) {
            document.getElementById('ddlemployeeName').options.length = "";
            var ddlAgentName = document.getElementById('ddlemployeeName');
            var length = ddlAgentName.options.length;
            for (i = length - 1; i >= 0; i--) {
                ddlAgentName.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Employee Name";
            opt.value = "";
            ddlAgentName.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    opt.id = msg[i].mobile;
                    ddlAgentName.appendChild(opt);
                }
            }
        }
        var phone = "";
        function ddlemployeechange(bname) {
            phone = bname.options[bname.selectedIndex].id;
            document.getElementById('txtMobileno').innerHTML = phone;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Despatch Assign<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#"><i ></i>Indent Details</a></li>
            <li><a href="#">Despatch Assign</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Despatch Assign Details
                </h3>
            </div>
            <div class="box-body">
                <div style="width: 100%;">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="divchblroutes" style="float: left; width: 350px; height: 330px; overflow: auto;">
                                </div>
                            </td>
                            <td>
                                <div id="div1" style="height: 230px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <label>
                                                    Employee Name:</label>
                                            </td>
                                            <td>
                                                <select id="ddlemployeeName" class="form-control" onchange="ddlemployeechange(this);">
                                                </select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Registered Number:</label>
                                            </td>
                                            <td>
                                                <span id="txtMobileno" style="font-size: 18px; color: Red; font-weight: bold;"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <input type="button" id="btnSave" value="Save" class="btn btn-primary" onclick="btndispatchassignment();" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table id="routemangt_table">
                        <tr>
                            <td>
                                <div id="div_routesmgnt" style="padding-left: 5%; width: 100%; cursor: pointer;">
                                    <table id="grd_routesmangelist">
                                    </table>
                                    <div id="page4">
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
