<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="OfferAssignment.aspx.cs" Inherits="OfferAssignment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <link href="js/DateStyles.css?v=3003" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#datepicker").datepicker({ dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
                onSelect: function (selectedDate) {
                }
            });
            $("#offerto").datepicker({ dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
                onSelect: function (selectedDate) {
                }
            });
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#datepicker').val(today);
            $('#offerto').val(today);
            FillSalesOffice();
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

        });

        function FillSalesOffice() {
            var data = { 'operation': 'GetPlantSalesOffice' };
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
            var ddlsalesOffice = document.getElementById('ddlSalesOffice');
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
            //document.getElementById('ddlSalesOffice').selectedIndex = 1;
            //fill_offer_structures();
        }
        function btngenerate() {
            var ddlsalesofficeid = document.getElementById('ddlSalesOffice').value;

            var data = { 'operation': 'GetSalesOfficeOffers', 'ddlsalesofficeid': ddlsalesofficeid };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindSalesOfficeOffers(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindSalesOfficeOffers(msg) {
            document.getElementById('divchblroutes').innerHTML = "";
            document.getElementById('divselected').innerHTML = "";
            for (var i = 0; i <= msg.length; i++) {
                if (typeof msg[i] === "undefined" || msg[i].BranchName == "" || msg[i].BranchName == null) {
                }
                else {
                    var label = document.createElement("span");
                    var hidden = document.createElement("input");
                    hidden.type = "hidden";
                    hidden.name = "hidden";
                    hidden.value = msg[i].Sno;
                    var checkbox = document.createElement("input");
                    checkbox.type = "checkbox";
                    checkbox.name = "checkbox";
                    checkbox.value = "checkbox";
                    checkbox.id = "checkbox";
                    checkbox.className = 'chkclass';
                    document.getElementById('divchblroutes').appendChild(checkbox);
                    label.innerHTML = msg[i].BranchName;
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
            $('.chkclass').each(function () {
                if ($(this).next().next().val() == Selected) {
                    $(this).attr("checked", false);
                }
            });

        }
        function btn_offers_assignment_Saveclick() {
            var div = document.getElementById('divselected');
            var divs = div.getElementsByTagName('div');
            var divArray = [];
            for (var i = 0; i < divs.length; i += 1) {
                divArray.push(divs[i].id);
            }
            if (divArray.length == 0) {
                alert("Please Select Offers To Combine");
                return false;
            }

            var txtOfferName = document.getElementById("txtOfferName").value;
            var Offer_from = document.getElementById('datepicker').value;
            var Offer_to = document.getElementById('offerto').value;
            var ddlOfferfor = document.getElementById('ddlOfferfor').value;
            var ddlsalesofficeid = document.getElementById('ddlSalesOffice').value;
            var btnsave = document.getElementById('btnSave').innerHTML;
            //alert("Productlist:" + Productlist + "offerProductlist" + Offer_Productlist);
            var Data = { 'operation': 'save_Offers_assignment', 'dataarr': divArray, 'OfferName': txtOfferName, 'Offerfrom': Offer_from, 'Offerto': Offer_to, 'Offerfor': ddlOfferfor, 'SalesOfficeID': ddlsalesofficeid };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Offers Assignment<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Masters</a></li>
            <li><a href="#"><i class="fa fa-dashboard"></i>Offers</a></li>
            <li><a href="#">Offers Assignment</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Offers Assignment Details
                </h3>
            </div>
            <div class="box-body">
                <table>
                    <tr>
                        <td>
                            <label class="headers">
                                SalesOffice:</label>
                        </td>
                        <td style="height: 40px;">
                            <select id="ddlSalesOffice" class="form-control">
                            </select>
                        </td>
                        <td>
                            <label>
                                Offer For</label>
                        </td>
                        <td style="height: 40px;">
                            <select id="ddlOfferfor" class="form-control">
                                <option selected="selected">Sales Office</option>
                                <option>Routes</option>
                                <option>Agents</option>
                            </select>
                        </td>
                        <td style="width: 5px;">
                            </td>
                            <td>
                             <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="btngenerate()"><i class="fa fa-refresh"></i>Generate</button>
                            </td>
                    </tr>
                </table>
                <br />
                <div style="width: 100%;">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div class="box box-info" style="float: left; width: 350px; height: 330px; overflow: auto;">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Offers Product Details
                                        </h3>
                                    </div>
                                    <div class="box-body">
                                        <div id="divchblroutes" class="divchblproducts">
                                        </div>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="box box-info" style="float: left; width: 240px; height: 330px; overflow: auto;">
                                    <div class="box-header with-border">
                                        <h3 class="box-title">
                                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Selected Offer Details
                                        </h3>
                                    </div>
                                    <div class="box-body">
                                        <div id="divselected" class="divofferproducts">
                                        </div>
                                    </div>
                                </div>
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
                                                        Offer From</label>
                                                </td>
                                                <td style="height: 40px;">
                                                    <input type="date" id="datepicker" placeholder="DD-MM-YYYY" class="form-control" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>
                                                        Offer To</label>
                                                </td>
                                                <td style="height: 40px;">
                                                    <input type="date" id="offerto" placeholder="DD-MM-YYYY" class="form-control" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>
                                                        Name:</label>
                                                </td>
                                                <td style="height: 40px;">
                                                    <input type="text" id="txtOfferName" class="form-control" placeholder="Offer Name" />
                                                </td>
                                            </tr>
                                            <tr>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                   <%-- <input type="button" id="btnSave" value="Save" class="btn btn-primary" onclick="btn_offers_assignment_Saveclick();" />--%>
                                                    <div class="input-group">
                                                                <div class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-ok" id="btnSave1" onclick="btn_offers_assignment_Saveclick()">
                                                                    </span><span id="btnSave" onclick="btn_offers_assignment_Saveclick()">Save</span>
                                                                </div>
                                                            </div>
                                                </td>
                                            </tr>
                                        </table>
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
