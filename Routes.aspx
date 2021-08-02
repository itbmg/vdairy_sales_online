<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Routes.aspx.cs" Inherits="Routes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);

        });
    </script>
    <script type="text/javascript">
        $(function () {
            FillBranches();
            bindmanageroutes();
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

        });
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
        function FillBranches() {
            var data = { 'operation': 'get_Branches_Salesoffice' };
            var s = function (msg) {
                if (msg) {
                    fillroutes_divchklist(msg);
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
            document.getElementById('divselected').innerHTML = "";
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
                    checkbox.value = "checkbox";
                    checkbox.id = "checkbox";
                    if (msg[i].status == "") {
                        checkbox.disabled = false;
                    }
                    else {
                        checkbox.disabled = true;
                    }
                    checkbox.className = 'chkclass';
                    document.getElementById('divchblroutes').appendChild(checkbox);
                    label.innerHTML = msg[i].Name;
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
                    var uncheckarrylen = div_uncheck_Array.length;
                    var a = div_uncheck_Array.indexOf(Selectedid);
                    if (a == -1) {
                    }
                    else {
                        div_uncheck_Array.splice(a, 1);
                    }
                    var uncheckarraylength = div_uncheck_Array.length;
                    if (uncheckarrylen == uncheckarraylength) {
                        div_check_Array.push(Selectedid);

                    }
                }
                else {
                    var Selected = $(this).next().next().val();
                    var elem = document.getElementById(Selected);
                    var p = elem.parentNode;
                    p.removeChild(elem);
                    div_uncheck_Array.push(Selected);
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
                    $(this).attr("disabled", false);
                }
            });
            div_uncheck_Array.push(Selected);

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
        function RefreshClick() {
            $('.chkclass').each(function () {
                $(this).attr("checked", false);
            });
            document.getElementById('divselected').innerHTML = "";
            document.getElementById('txtRouteName').value = "";
            document.getElementById('txtindenttype').value = "";
            document.getElementById('btnSave').innerHTML = "Save";
            document.getElementById('divindent').innerHTML = "";
            div_uncheck_Array = [];
            div_check_Array = [];
        }

        function btnrightClick() {
            var div = document.getElementById('divindent');
            var divs = div.getElementsByTagName('div');
            var i = divs.length;

            var Selected = document.getElementById('txtindenttype').value;
            for (var i = 0; i < divs.length; i += 1) {
                if (divs[i].innerText == Selected) {
                    alert("Please Enter Different Indent Type");
                    return false;
                }
            }
            var Selectedid = i + 1;
            var label = document.createElement("div");
            var Crosslabel = document.createElement("img");
            Crosslabel.style.float = "right";
            Crosslabel.src = "Images/Cross.png";
            Crosslabel.onclick = function () { RemoveindentClick(Selectedid); };
            label.id = i + 1;
            label.innerHTML = Selected;
            label.className = 'divselectedclass';
            document.getElementById('divindent').appendChild(label);
            label.appendChild(Crosslabel);
            document.getElementById('txtindenttype').value = "";
        }
        function RemoveindentClick(Selected) {
            var elem = document.getElementById(Selected);
            var p = elem.parentNode;
            p.removeChild(elem);
        }
        function gridselctionchangedindent(msg) {
            document.getElementById('divindent').innerHTML = "";
            for (var j = 0; j < msg.length; j++) {
                var div = document.getElementById('divindent');
                var divs = div.getElementsByTagName('div');
                var i = divs.length;
                var Selected = msg[j].Indenttype;
                var Selectedid = i + 1;
                var label = document.createElement("div");
                var Crosslabel = document.createElement("img");
                Crosslabel.style.float = "right";
                Crosslabel.src = "Images/Cross.png";
                Crosslabel.id = Selectedid;
                Crosslabel.onclick = function () { RemoveindentClick(this.id); };
                label.id = Selectedid;
                label.innerHTML = Selected;
                label.className = 'divselectedclass';
                document.getElementById('divindent').appendChild(label);
                label.appendChild(Crosslabel);
            }
        }
        function btnSalesofficerouteDeleteclick() {
            if (!confirm("Do you really want to delete")) {
                return false;
            }
        }
        function btnSalesofficerouteSaveclick() {
            var div = document.getElementById('divselected');
            var divs = div.getElementsByTagName('div');
            var divArray = [];
            for (var i = 0; i < divs.length; i += 1) {
                divArray.push(divs[i].id);
            }
            if (divArray.length == 0) {
                alert("Please Select Branch");
                return false;
            }
            var indenttype = document.getElementById('txtindenttype').value;
            if (indenttype == "Indent1") {
                alert("Please Click the Arrow In IndentType");
                return false;
            }
            var divind = document.getElementById('divindent');
            var divsindent = divind.getElementsByTagName('div');
            var divindentArray = [];
            for (var i = 0; i < divsindent.length; i += 1) {
                divindentArray.push(divsindent[i].innerText);
            }
            if (divindentArray.length == 0) {
                alert("Please Enter Indent Type");
                return false;
            }
            var routename = document.getElementById('txtRouteName').value;
            if (routename == "") {
                alert("Please Enter RouteName");
                return false;
            }
            var srname = document.getElementById('txtsrname').value;
            if (srname == "") {
                alert("Please Enter SR Name");
                return false;
            }

            var btnsave = document.getElementById('btnSave').innerHTML;
            var Data = { 'operation': 'btnRoutesSaveClick', 'routename': routename, 'Name': srname, 'dataarr': divArray, 'divindentArray': divindentArray, 'div_uncheck_Array': div_uncheck_Array, 'div_check_Array': div_check_Array, 'btnsave': btnsave, 'refno': refno };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    bindmanageroutes();
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
        function btnRouteAssignRefreshclick() {
            RefreshClick();
        }
        function bindmanageroutes() {
            var data = { 'operation': 'updateroutestogrid' };
            var s = function (msg) {
                if (msg) {
                    bindingmanageroutes(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var refno = "";
        function bindingmanageroutes(databind) {
            var l = 0;
            var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">Route Name</th><th scope="col" class="thcls">RouteIndents</th><th scope="col" class="thcls">Route Description</th><th scope="col" class="thcls">SR Name</th><th scope="col"></th></tr></thead></tbody>';
            for (var Booking in databind) {
                results += '<tr style="background-color:' + COLOR[l] + '">';
                results += '<td scope="row"  class="1 tdmaincls" >' + databind[Booking].RouteName + '</td>';
                results += '<td data-title="Capacity" class="2">' + databind[Booking].Indenttype + '</td>';
                results += '<td data-title="Capacity" class="3">' + databind[Booking].DistributorName + '</td>';
                results += '<td data-title="Capacity" class="5">' + databind[Booking].srname + '</td>';
                results += '<td style="display:none" class="4">' + databind[Booking].RefNo + '</td>';
                results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"   onclick="getme(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';

                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_Routedata").html(results);
        }
        function getme(thisid) {
            var routename = $(thisid).parent().parent().children('.1').html();
            var srname = $(thisid).parent().parent().children('.5').html();
            refno = $(thisid).parent().parent().children('.4').html();
            document.getElementById('divselected').innerHTML = "";
            document.getElementById('txtRouteName').value = routename;
            document.getElementById('txtsrname').value = srname;
            document.getElementById('btnSave').innerHTML = "MODIFY";
            var data = { 'operation': 'updatedivselected', 'refno': refno };
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
            var data = { 'operation': 'updatedivindents', 'refno': refno };
            var s = function (msg) {
                if (msg) {
                    gridselctionchangedindent(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
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
                                <input type="button" class="btnUp" onclick="btnUpClick();" /><br />
                                <input type="button" class="btnDown" onclick="btnDownClick();" />
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
                                                        Route Name:</label>
                                                </td>
                                                <td style="height: 40px;">
                                                    <input type="text" id="txtRouteName" class="form-control" placeholder="Enter Route Name" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>
                                                        SR Name:</label>
                                                </td>
                                                <td style="height: 40px;">
                                                    <input type="text" id="txtsrname" class="form-control" placeholder="Enter SR Name" />
                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                            <tr>
                                                <td>
                                                    <label>
                                                        Indent Type:</label>
                                                </td>
                                                <td>
                                                    <input type="text" id="txtindenttype" class="form-control" style="width: 80px; height: 30px;"
                                                        placeholder="Enter Indent Type" />
                                                </td>
                                                <td>
                                                    <input type="button" class="btnRight" onclick="btnrightClick();" />
                                                </td>
                                                <td>
                                                    <div class="box box-info" style="height: 130px; width: 120px;">
                                                        <div class="box-header with-border">
                                                        </div>
                                                        <div class="box-body">
                                                            <div id="divindent">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td colspan="2">
                                                                <div class="input-group">
                                                                    <div class="input-group-addon">
                                                                        <span class="glyphicon glyphicon-ok" id="btn_brnch_prodtuct_save1" onclick="btnSalesofficerouteSaveclick()">
                                                                        </span><span id="btnSave" onclick="btnSalesofficerouteSaveclick()">Save</span>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td style="padding-left: 7px;">
                                                                <div class="input-group">
                                                                    <div class="input-group-close">
                                                                        <span class="glyphicon glyphicon-trash" id="btnDelete1" onclick="btnSalesofficerouteDeleteclick()">
                                                                        </span><span id="btnDelete" onclick="btnSalesofficerouteDeleteclick()">Delete</span>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td style="padding-left: 7px;">
                                                                <div class="input-group">
                                                                    <div class="input-group-refresh">
                                                                        <span class="glyphicon glyphicon-refresh" id='btnRefresh1' onclick="btnRouteAssignRefreshclick()">
                                                                        </span><span id='btnRefresh' onclick="btnRouteAssignRefreshclick()">Refresh</span>
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
                            </td>
                        </tr>
                    </table>
                    <br />
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
