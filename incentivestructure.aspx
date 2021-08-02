<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="incentivestructure.aspx.cs" Inherits="incentivestructure" %>

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
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
   
    <script type="text/javascript">
        $(function () {
            Fillclubbings();
            // fillroutenames();
            //routename_onchange();
            bindmanagestructure();
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

        });
        function fillroutenames() {
            var data = { 'operation': 'get_all_Routes' };
            // var data = { 'operation': 'Initilize_branchmapping' };
            var s = function (msg) {
                if (msg) {
                    fill_routes(msg);
                    //counts = 0;
                    //BindCategoriesss(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {

            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

            callHandler(data, s, e);
        }
        function fill_routes(msg) {
            var routenames = document.getElementById('cmbroutename');
            var length = routenames.options.length;
            document.getElementById("cmbroutename").options.length = null;
            document.getElementById("cmbroutename").value = "select";
            var opt = document.createElement('option');
            opt.innerHTML = "Select";
            routenames.appendChild(opt);

            for (var i = 0; i < msg.length; i++) {
                if (msg[i].routename != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].routename;
                    opt.value = msg[i].routesno;
                    routenames.appendChild(opt);
                }
            }
        }
        function routename_onchange() {
            var cmbroutenamename = document.getElementById("cmbroutename").value;
            var data = { 'operation': 'get_routebranches', 'cmbroutenamename': cmbroutenamename };
            var s = function (msg) {
                if (msg) {
                    fillbranchnames(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {

            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

            callHandler(data, s, e);
        }
        function fillbranchnames(msg) {
            document.getElementById('divselected').innerHTML = "";
            for (var i = 0; i <= msg.length; i++) {
                if (typeof msg[i] === "undefined" || msg[i].Name == "" || msg[i].Name == null) {
                }
                else {
                    var label = document.createElement("span");
                    var hidden = document.createElement("input");
                    hidden.type = "hidden";
                    hidden.name = "hidden";
                    hidden.id = "hidden";
                    hidden.value = msg[i].id;
                    var checkbox = document.createElement("input");
                    checkbox.type = "checkbox";
                    checkbox.name = "checkbox";
                    checkbox.value = "checkbox";
                    checkbox.id = msg[i].id;
                    //checkbox.className = 'checkinput';
                    checkbox.className = 'chkclass';
                    document.getElementById('divselected').appendChild(checkbox);
                    label.innerHTML = msg[i].Name;
                    document.getElementById('divselected').appendChild(label);
                    document.getElementById('divselected').appendChild(hidden);
                    document.getElementById('divselected').appendChild(document.createElement("br"));
                }

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
        function Fillclubbings() {
            var data = { 'operation': 'get_branch_clubbings' };
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
                    // hidden.id = "hidden";
                    hidden.value = msg[i].id;
                    var checkbox = document.createElement("input");
                    checkbox.type = "checkbox";
                    checkbox.name = "checkbox";
                    checkbox.value = "checkbox";
                    checkbox.id = "checkbox";
                    //checkbox.id = msg[i].id;
                    //checkbox.className = 'checkinput';
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
                    //                   var Selected = $(this).next(".livalue");
                    //                   var Selectedid = $(this).val();
                    var Selected = $(this).next().text();
                    var Selectedid = $(this).next().next().val();
                    var label = document.createElement("div");
                    var Crosslabel = document.createElement("img");
                    Crosslabel.style.float = "right";
                    Crosslabel.src = "Images/Cross.png";
                    Crosslabel.onclick = function () { RemoveClick(Selectedid); };
                    label.id = Selectedid;
                    //label.innerHTML = Selected.text();
                    label.innerHTML = Selected;
                    label.className = 'divselectedclass';
                    label.onclick = function () { divonclick(label); }
                    document.getElementById('divselected').appendChild(label);
                    label.appendChild(Crosslabel);
                }
                else {
                    //var Selected = $(this).val();
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
            //           $('.checkinput').each(function () {
            //               if ($(this).val() == Selected) {
            //                   $(this).attr("checked", false);
            //               }
            //           });
            $('.chkclass').each(function () {
                if ($(this).next().next().val() == Selected) {
                    $(this).attr("checked", false);
                }
            });
            $('.chkclass').each(function () {
                $(Selected).attr("checked", false);
            });
        }
        function gridselctionchanged(msg) {
            $('.chkclass').each(function () {
                $(this).attr("checked", false);
            });
            for (var i = 0; i < msg.length; i++) {
                //               $('.checkinput').each(function () {
                $('.chkclass').each(function () {
                    //                   if ($(this).val() == msg[i].BranchID) {
                    if ($(this).next().next().val() == msg[i].clubsno) {
                        $(this).attr("checked", true);
                        var Selected = msg[i].clubName;
                        var Selectedid = msg[i].clubsno;
                        var label = document.createElement("div");
                        var Crosslabel = document.createElement("img");
                        Crosslabel.style.float = "right";
                        Crosslabel.src = "Images/Cross.png";
                        Crosslabel.id = Selectedid;
                        Crosslabel.onclick = function () { RemoveClick(Selectedid); };
                        label.id = Selectedid;
                        label.innerHTML = Selected;
                        label.className = 'divselectedclass';
                        label.onclick = function () { divonclick(label); }
                        document.getElementById('divselected').appendChild(label);
                        label.appendChild(Crosslabel);
                    }
                });
            }

        }
        function RefreshClick() {
            //           $('.checkinput').each(function () {
            $('.chkclass').each(function () {
                $(this).attr("checked", false);
            });
            document.getElementById('divselected').innerHTML = "";
            document.getElementById('txtstructureName').value = "";
            document.getElementById('btnstructureSave').innerHTML = "Save";
        }
        function btnSalesofficerouteDeleteclick() {
            //           if (!confirm("Do you really want to delete")) {
            //               return false;
            //           }
            //           var Data = { 'operation': 'btnstructureDeleteClick', 'refno': refno };
            //           var s = function (msg) {
            //               if (msg) {
            //                   alert(msg);
            //                   bindmanagestructure();
            //                   RefreshClick();
            //               }
            //               else {
            //               }
            //           };
            //           var e = function (x, h, e) {
            //               
            //           };
            //           $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

            //           callHandler(Data, s, e);
        }
        function btnStructureSaveclick() {
            var div = document.getElementById('divselected');
            var divs = div.getElementsByTagName('div');
            var divArray = [];
            for (var i = 0; i < divs.length; i += 1) {
                if ($.inArray(divs[i].id, divArray) > -1) {
                    alert("Please Select Different Clubbing");
                    return false;

                }
                else {
                    divArray.push(divs[i].id);
                }
            }
            //           var clubarray = [];
            //           var clubcheckinputs = $('#divchblroutes').find('.chkclass');
            //           clubcheckinputs.each(function (list) {
            //               var checkbox = clubcheckinputs[list];
            //               if (checkbox.checked) {
            //                   clubarray.push(checkbox.id);
            //               }
            //           });
            //           var brancharray =[];
            //           var branchcheckinputs = $('#divselected').find('.chkclass');
            //           branchcheckinputs.each(function (list) {
            //               var checkbox = branchcheckinputs[list];
            //               if (checkbox.checked) {
            //                   brancharray.push(checkbox.id);
            //               }
            //           });


            if (divArray.length == 0) {
                alert("Please Select Clubbing");
                return false;
            }
            var structname = document.getElementById('txtstructureName').value;
            if (structname == "") {
                alert("Please Enter StructureName");
                return false;
            }
            var btnstructureSave = document.getElementById('btnstructureSave').innerHTML;
            var Data = { 'operation': 'btnStructureSaveClick', 'structname': structname, 'dataarr': divArray, 'btnstructureSave': btnstructureSave, 'refno': refno };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    bindmanagestructure();
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
        function bindmanagestructure() {
            var data = { 'operation': 'updatestructuretogrid' };
            var s = function (msg) {
                if (msg) {
                    bindingmanagestruct(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
                //               if (x.status && x.status == 400) {
                //                   alert(x.responseText);
                //                   window.location.assign("Login.aspx");
                //               }
                //               else {
                //                   alert("something went wrong");
                //               }
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

            callHandler(data, s, e);
        }
        var refno = "";
        function bindingmanagestruct(databind) {
            $("#grd_structmangelist").jqGrid("clearGridData");
            var newarray = [];
            var Headarray = [];
            var headdatacol = databind[1];
            var datacol = databind;
            for (var Booking in databind) {

                newarray.push({ 'Sno': newarray.length + 1, 'Structure Name': databind[Booking].RouteName, 'Structure Description': databind[Booking].DistributorName, 'RefNo': databind[Booking].RefNo });
            }
            $("#grd_structmangelist").jqGrid({
                datatype: "local",
                height: 150,
                width: 'auto',
                //overflow-x:'auto',
                overflow: 'auto',
                colNames: Headarray,
                colModel: [{ name: 'Sno', index: 'invdate', width: 90, sortable: false, align: 'center' },
        { name: 'Structure Name', index: 'invdate', width: 170, sortable: false, align: 'center' },
        { name: 'Structure Description', index: 'invdate', width: 600, sortable: false, align: 'center' },
        { name: 'RefNo', index: 'invdate', width: 170, sortable: false, align: 'center', hidden: true}],
                rowNum: 10,
                rowList: [5, 10, 30],
                // rownumbers: true,
                gridview: true,
                loadonce: true,
                pager: "#page_structure",
                caption: "Structure Manage"
            }).jqGrid('navGrid', '#page_structure', { edit: false, add: false, del: false, search: false, refresh: false });
            var mydata = newarray;
            for (var i = 0; i <= mydata.length; i++) {

                jQuery("#grd_structmangelist").jqGrid('addRowData', i + 1, mydata[i]);
            }
            $("#grd_structmangelist").jqGrid('setGridParam', { onSelectRow: function (rowid, iRow, iCol, e) {

                var routename = $('#grd_structmangelist').getCell(rowid, 'Structure Name');
                var routedescription = $('#grd_structmangelist').getCell(rowid, 'Structure Description');
                document.getElementById('divselected').innerHTML = "";
                document.getElementById('txtstructureName').value = routename;
                document.getElementById('btnstructureSave').innerHTML = "MODIFY";

                refno = $('#grd_structmangelist').getCell(rowid, 'RefNo');
                var data = { 'operation': 'updatediv_structure', 'refno': refno };
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 100%; <%--height: 550px; --%> background-color: #fff">
        <table style="padding-left: 400px">
            <tr>
                <td>
                    <label class="headers">
                        STRUCTURE MANAGEMENT</label>
                </td>
            </tr>
        </table>
        <div style="width: 100%;">
            <table style="width: 100%;">
                <tr>
                    <td>
                        <div class="box box-info" style="float: left; width: 350px; height: 330px; overflow: auto;">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Product Clubbing Details
                                </h3>
                            </div>
                            <div class="box-body">
                                <div id="divchblroutes">
                                </div>
                            </div>
                        </div>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <div class="box box-info" style="float: left; width: 350px; height: 330px; overflow: auto;">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Selected Clubbing Details
                                </h3>
                            </div>
                            <div class="box-body">
                                <div id="divselected" >
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
                                                Structure Name</label>
                                        </td>
                                        <td>
                                            <input type="text" id="txtstructureName"    class="form-control" placeholder="Enter Structure Name" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                           <td>
                                                                <table>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <div class="input-group">
                                                                                <div class="input-group-addon">
                                                                                    <span class="glyphicon glyphicon-ok" id="btnstructureSave1" onclick="btnStructureSaveclick()">
                                                                                    </span><span id="btnstructureSave" onclick="btnStructureSaveclick()">Save</span>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                        <td style="padding-left: 7px;">
                                                                         <div class="input-group">
                                                    <div class="input-group-close">
                                                        <span class="glyphicon glyphicon-trash" id="btnstructureDelete1" onclick="btnSalesofficerouteDeleteclick()">
                                                        </span><span id="btnstructureDelete" onclick="btnSalesofficerouteDeleteclick()">Delete</span>
                                                    </div>
                                                </div>
                                                                        
                                                                        </td>    
                                                                        <td style="padding-left: 7px;">
                                                                            <div class="input-group">
                                                                                <div class="input-group-close">
                                                                                    <span class="glyphicon glyphicon-refresh" id='btnstructureRefresh1' onclick="btnRouteAssignRefreshclick()">
                                                                                    </span><span id='btnstructureRefresh' onclick="btnRouteAssignRefreshclick()">Refresh</span>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                       <%-- <td>
                                            <input type="button" id="btnstructureSave" value="Save" class="btn btn-primary" onclick="btnStructureSaveclick();" />
                                            <input type="button" id="btnstructureDelete" value="Delete" class="btn btn-warning" onclick="btnSalesofficerouteDeleteclick();" />
                                            <input type="button" id="btnstructureRefresh" value="Refresh" class="btn btn-danger"
                                                onclick="btnRouteAssignRefreshclick();" />
                                        </td>--%>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <table id="structuremangt_table">
                <tr>
                    <td>
                        <div id="div_structuremgnt" style="padding-left: 5%; width: 100%; cursor: pointer;">
                            <table id="grd_structmangelist">
                            </table>
                            <div id="page_structure">
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
