<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ProductClubbing.aspx.cs" Inherits="ProductClubbing" %>

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
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            FillBranchproducts();
            bindproductclub();
            $('#divSlots').removeTemplate();
            $('#divSlots').setTemplateURL('slots.htm');
            $('#divSlots').processTemplate();
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
        function btnpopupclick() {
            $('.pickupclass').css('display', 'block');
        }
        function PopupCloseClick() {
            $('.pickupclass').css('display', 'none');
        }
        var slottable = [];

        function btnpopupAddClick() {
            var range = document.getElementById('popuprange').value;
            var amount = document.getElementById('popupamount').value;
            if (range == "" || amount == "") {
                alert("Please Add Range And Amount");
                return false;
            }
            else {

                for (var i = 0; i < slottable.length; i++) {

                    if (slottable[i].range == range) {
                        alert("This Range Already Added");
                        return false;
                    }
                }
            }
            slottable.push({ range: range, amount: amount });
            $('#divSlots').removeTemplate();
            $('#divSlots').setTemplateURL('slots.htm');
            $('#divSlots').processTemplate(slottable);
        }
        function RemoveRowClick(remove) {
            var range = $(remove).closest('tr').find('#txtrange').val();
            var slotdata = [];
            for (var i = 0; i < slottable.length; i++) {

                if (slottable[i].range != range) {
                    slotdata.push({ range: slottable[i].range, amount: slottable[i].amount });
                }
                $('#divSlots').removeTemplate();
                $('#divSlots').setTemplateURL('slots.htm');
                $('#divSlots').processTemplate(slotdata);

            }
            slottable = slotdata;
        }
        function FillBranchproducts() {
            var data = { 'operation': 'get_branch_products' };
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
            $("#divchblproducts").html("");
            var prnt = $("#divchblproducts");
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
            document.getElementById('divchblproducts').innerHTML = "";
            document.getElementById('divselectedprdt').innerHTML = "";
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
                    //checkbox.className = 'checkinput';
                    checkbox.className = 'chkclass';
                    document.getElementById('divchblproducts').appendChild(checkbox);
                    label.innerHTML = msg[i].Name;
                    document.getElementById('divchblproducts').appendChild(label);
                    document.getElementById('divchblproducts').appendChild(hidden);
                    document.getElementById('divchblproducts').appendChild(document.createElement("br"));
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
                    document.getElementById('divselectedprdt').appendChild(label);
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
        }
        function gridselctionchanged(msg) {
            $('.chkclass').each(function () {
                $(this).attr("checked", false);
            });
            for (var i = 0; i < msg.length; i++) {
                //               $('.checkinput').each(function () {
                $('.chkclass').each(function () {
                    //                   if ($(this).val() == msg[i].BranchID) {
                    if ($(this).next().next().val() == msg[i].productsno) {
                        $(this).attr("checked", true);
                        var Selected = msg[i].ProductName;
                        var Selectedid = msg[i].productsno;
                        var label = document.createElement("div");
                        var Crosslabel = document.createElement("img");
                        Crosslabel.style.float = "right";
                        Crosslabel.src = "Images/Cross.png";
                        Crosslabel.onclick = function () { RemoveClick(Selectedid); };
                        label.id = Selectedid;
                        label.innerHTML = Selected;
                        label.className = 'divselectedclass';
                        label.onclick = function () { divonclick(label); }
                        document.getElementById('divselectedprdt').appendChild(label);
                        label.appendChild(Crosslabel);
                    }
                });
            }

        }
        function fillslots(msg) {
            slottable.length = 0;
            for (var Booking in msg) {
                slottable.push({ range: msg[Booking].range, amount: msg[Booking].amount });
            }
            //slottable.push({ range: msg[range, amount: amount });
            $('#divSlots').removeTemplate();
            $('#divSlots').setTemplateURL('slots.htm');
            $('#divSlots').processTemplate(slottable);
            //           $('#divSlots').removeTemplate();
            //           $('#divSlots').setTemplateURL('slots.htm');
            //           $('#divSlots').processTemplate(msg);
        }
        function RefreshClick() {
            //           $('.checkinput').each(function () {
            $('.chkclass').each(function () {
                $(this).attr("checked", false);
            });
            document.getElementById('divselectedprdt').innerHTML = "";
            document.getElementById('txtClubbingName').value = "";
            document.getElementById('btnprdtclubSave').innerHTML = "Save";
            slottable = [];
            $('#divSlots').removeTemplate();
            $('#divSlots').setTemplateURL('slots.htm');
            $('#divSlots').processTemplate();
        }
        function btnproductsclubbingDeleteclick() {
            if (!confirm("Do you really want to delete")) {
                return false;
            }
            //var Data = { 'operation': 'btnRoutesDeleteClick', 'refno': refno };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    bindproductclub();
                    RefreshClick();
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

            callHandler(Data, s, e);
        }
        function numberOnlyExample() {
            if ((event.keyCode < 48) || (event.keyCode > 57))
                return false;
        }
        function btnproductsclubbingSaveclick() {
            var div = document.getElementById('divselectedprdt');
            var divs = div.getElementsByTagName('div');
            var divArray = [];
            for (var i = 0; i < divs.length; i += 1) {
                divArray.push(divs[i].id);
            }
            if (divArray.length == 0) {
                alert("Please Select Branch");
                return false;
            }
            var clubbingname = document.getElementById('txtClubbingName').value;
            if (clubbingname == "") {
                alert("Please Enter ClubbingName");
                return false;
            }
            var btnsave = document.getElementById('btnprdtclubSave').innerHTML;
            var rows = $("#table_SlotDetails tr:gt(0)");
            var slotdetails = new Array();
            $(rows).each(function (i, obj) {
                if (typeof $(this).find('#txtrange').val() != "undefined") {
                    slotdetails.push({ Range: $(this).find('#txtrange').val(), Amount: $(this).find('#txtamt').val() });
                }

            });
            if (slotdetails.length == 0) {
                alert("Please Add Slabs For This Clubbing");
                return false;
            }
            var Data = { 'operation': 'btnprdtclubbingSaveClick', 'clubbingname': clubbingname, 'dataarr': divArray, 'btnsave': btnsave, 'slotdetails': slotdetails, 'refno': refno };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    bindproductclub();
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
        function btnproductclubbingRefreshclick() {
            RefreshClick();
        }
        function bindproductclub() {
            var data = { 'operation': 'updateprdt_clubbingtogrid' };
            var s = function (msg) {
                if (msg) {
                    bindingproductclubbing(msg);
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
        function bindingproductclubbing(databind) {
            $("#grd_productclubbinglist").jqGrid("clearGridData");
            var newarray = [];
            var Headarray = [];
            var headdatacol = databind[1];
            var datacol = databind;
            for (var Booking in databind) {

                newarray.push({ 'Sno': newarray.length + 1, 'Club Name': databind[Booking].RouteName, 'Club Description': databind[Booking].DistributorName, 'RefNo': databind[Booking].RefNo });
            }
            $("#grd_productclubbinglist").jqGrid({
                datatype: "local",
                height: 150,
                width: 'auto',
                //overflow-x:'auto',
                overflow: 'auto',
                colNames: Headarray,
                colModel: [{ name: 'Sno', index: 'invdate', width: 90, sortable: false, align: 'center' },
        { name: 'Club Name', index: 'invdate', width: 170, sortable: false, align: 'center' },
        { name: 'Club Description', index: 'invdate', width: 600, sortable: false, align: 'center' },
        { name: 'RefNo', index: 'invdate', width: 170, sortable: false, align: 'center', hidden: true}],
                rowNum: 10,
                rowList: [5, 10, 30],
                // rownumbers: true,
                gridview: true,
                loadonce: true,
                pager: "#page_prdtclubbing",
                caption: "Products Clubbing"
            }).jqGrid('navGrid', '#page_prdtclubbing', { edit: false, add: false, del: false, search: false, refresh: false });
            var mydata = newarray;
            for (var i = 0; i <= mydata.length; i++) {

                jQuery("#grd_productclubbinglist").jqGrid('addRowData', i + 1, mydata[i]);
            }
            $("#grd_productclubbinglist").jqGrid('setGridParam', { onSelectRow: function (rowid, iRow, iCol, e) {

                var clubname = $('#grd_productclubbinglist').getCell(rowid, 'Club Name');
                var clubdescription = $('#grd_productclubbinglist').getCell(rowid, 'Club Description');
                document.getElementById('divselectedprdt').innerHTML = "";
                document.getElementById('txtClubbingName').value = clubname;
                document.getElementById('btnprdtclubSave').innerHTML = "MODIFY";

                refno = $('#grd_productclubbinglist').getCell(rowid, 'RefNo');
                var data = { 'operation': 'updatedivclubbingselected', 'refno': refno };
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
                var data = { 'operation': 'updatedivslots', 'refno': refno };
                var s = function (msg) {
                    if (msg) {
                        fillslots(msg);
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 100%; <%--height: 550px; --%> background-color: #fff">
        <table style="padding-left: 400px">
            <tr>
                <td>
                    <label class="headers">
                        PRODUCTS CLUBBING</label>
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
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Agent Details
                                </h3>
                            </div>
                            <div class="box-body">
                                <div id="divchblproducts">
                                </div>
                            </div>
                        </div>
                    </td>
                    <%-- <td>
                        <input type="button" class="btnRight" onclick="btnRightClick();"/><br />
                        <input type="button" class="btnleft" onclick="btnleftClick();"/>
                    </td>--%>
                    <td>
                        <div class="box box-info" style="float: left; width: 240px; height: 330px; overflow: auto;">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Selected Agent Details
                                </h3>
                            </div>
                            <div class="box-body">
                                <div id="divselectedprdt">
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
                                                Clubbing Name</label>
                                        </td>
                                        <td>
                                            <input type="text" id="txtClubbingName" class="form-control" placeholder="Enter Clubbing Name" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <div id="divSlots">
                                            </div>
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
                                                                                    <span class="glyphicon glyphicon-ok" id="btnprdtclubSave1" onclick="btnproductsclubbingSaveclick()">
                                                                                    </span><span id="btnprdtclubSave" onclick="btnproductsclubbingSaveclick()">Save</span>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                        <td style="padding-left: 7px;">
                                                                         <div class="input-group">
                                                    <div class="input-group-close">
                                                        <span class="glyphicon glyphicon-trash" id="btnDelete1" onclick="btnproductsclubbingDeleteclick()">
                                                        </span><span id="btnDelete" onclick="btnproductsclubbingDeleteclick()">Delete</span>
                                                    </div>
                                                </div>
                                                                        
                                                                        </td>    
                                                                        <td style="padding-left: 7px;">
                                                                            <div class="input-group">
                                                                                <div class="input-group-close">
                                                                                    <span class="glyphicon glyphicon-refresh" id='btnRefresh1' onclick="btnproductclubbingRefreshclick1()">
                                                                                    </span><span id='btnRefresh' onclick="btnproductclubbingRefreshclick()">Refresh</span>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>

                                        <%--<td>
                                            <input type="button" id="btnprdtclubSave" value="Save" class="btn btn-primary" onclick="btnproductsclubbingSaveclick();" />
                                            <input type="button" id="btnDelete" value="Delete" class="btn btn-danger" onclick="btnproductsclubbingDeleteclick();" />
                                            <input type="button" id="btnRefresh" value="Refresh" class="btn btn-warning" onclick="btnproductclubbingRefreshclick();" />
                                        </td>--%>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <table id="prdtclubbing_table">
                <tr>
                    <td>
                        <div id="div_productclubbing" style="padding-left: 5%; width: 100%; cursor: pointer;">
                            <table id="grd_productclubbinglist">
                            </table>
                            <div id="page_prdtclubbing">
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
