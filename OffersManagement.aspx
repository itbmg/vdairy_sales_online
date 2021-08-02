<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="OffersManagement.aspx.cs" Inherits="OffersManagement" %>

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
            //            $("#datepicker").datepicker({ dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
            //                onSelect: function (selectedDate) {
            //                }
            //            });
            //            var date = new Date();
            //            var day = date.getDate();
            //            var month = date.getMonth() + 1;
            //            var year = date.getFullYear();
            //            if (month < 10) month = "0" + month;
            //            if (day < 10) day = "0" + day;
            //            today = year + "-" + month + "-" + day;
            //            $('#datepicker').val(today);
            //            $('#offerto').val(today);
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
            document.getElementById('ddlSalesOffice').selectedIndex = 1;

            FillBranchproducts();
        }
        function FillBranchproducts() {
            var data = { 'operation': 'get_branch_products' };
            var s = function (msg) {
                if (msg) {
                    fillroutes_divchklist(msg);
                    fillofferproducts(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function fillroutes_divchklist(msg) {
            document.getElementById('divchblproducts').innerHTML = "";
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
                    checkbox.id = msg[i].id;
                    //checkbox.className = 'checkinput';
                    checkbox.className = 'chkclass';
                    document.getElementById('divchblproducts').appendChild(checkbox);
                    label.innerHTML = msg[i].Name;
                    document.getElementById('divchblproducts').appendChild(label);
                    document.getElementById('divchblproducts').appendChild(hidden);
                    document.getElementById('divchblproducts').appendChild(document.createElement("br"));
                }
            }
            //TabclassClick();
        }
        function fillofferproducts(msg) {
            document.getElementById('divofferproducts').innerHTML = "";
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
                    checkbox.id = msg[i].id;
                    //checkbox.className = 'checkinput';
                    checkbox.className = 'chkOfferclass';
                    document.getElementById('divofferproducts').appendChild(checkbox);
                    label.innerHTML = msg[i].Name;
                    document.getElementById('divofferproducts').appendChild(label);
                    document.getElementById('divofferproducts').appendChild(hidden);
                    document.getElementById('divofferproducts').appendChild(document.createElement("br"));
                }
            }

        }
        function btn_offersSaveclick() {
            var Productlist = new Array();
            var Offer_Productlist = new Array();

            $("#divchblproducts .chkclass").each(function () {
                if (this.checked == true) {
                    var product_sno = this.id;
                    Productlist.push(product_sno);
                }
            });
            $("#divofferproducts .chkOfferclass").each(function () {
                if (this.checked == true) {
                    var Offer_product_sno = this.id;
                    Offer_Productlist.push(Offer_product_sno);
                }
            });


            var txtOfferName = document.getElementById("txtOfferName").value;
            var txtprdtqtyifabove = document.getElementById('txtprdtqtyifabove').value;
            var txofferprdtqty = document.getElementById('txofferprdtqty').value;
            var ddlOfferfor = document.getElementById('ddlOfferfor').value;
            var ddlsalesofficeid = document.getElementById('ddlSalesOffice').value;
            var btnsave = document.getElementById('btnSave').innerHTML;
            //alert("Productlist:" + Productlist + "offerProductlist" + Offer_Productlist);
            var Data = { 'operation': 'save_Offers', 'Productlist': Productlist, 'Offer_Productlist': Offer_Productlist, 'OfferName': txtOfferName, 'prdtqtyifabove': txtprdtqtyifabove, 'offerprdtqty': txofferprdtqty, 'Offerfor': ddlOfferfor, 'SalesOfficeID': ddlsalesofficeid };
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
            Offer Management<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Masters</a></li>
            <li><a href="#"><i class="fa fa-dashboard"></i>Offers</a></li>
            <li><a href="#">Offer Management</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Offer Details
                </h3>
            </div>
            <div class="box-body">
                <table>
                    <tr>
                        <td>
                            <label class="headers">
                                SalesOffice:</label>
                        </td>
                        <td>
                            <select id="ddlSalesOffice" class="form-control" style="width: 180px;">
                            </select>
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
                                            <i style="padding-right: 5px;" class="fa fa-cog"></i>Offer Product Details
                                        </h3>
                                    </div>
                                    <div class="box-body">
                                <div id="divchblproducts" class="divchblproducts" >
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
                                <div id="divofferproducts" class="divofferproducts" >
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
                                                    Offer Name:</label>
                                            </td>
                                            <td style="height:40px;">
                                                <input type="text" id="txtOfferName" class="form-control" placeholder="Offer Name" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Product Qty If Above</label>
                                            </td>
                                            <td style="height:40px;">
                                                <input type="text" id="txtprdtqtyifabove" class="form-control" placeholder="Product Qty" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Offer Product Qty</label>
                                            </td>
                                            <td style="height:40px;">
                                                <input type="text" id="txofferprdtqty" class="form-control" placeholder="Offer Product Qty" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Offer For</label>
                                            </td>
                                            <td style="height:40px;">
                                                <select id="ddlOfferfor" class="form-control" >
                                                    <option selected="selected">Sales</option>
                                                    <option>Incentive</option>
                                                </select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <%--<input type="button" id="btnSave" value="Save" class="btn btn-primary"  onclick="btn_offersSaveclick();" />--%>
                                                <div class="input-group">
                                                                <div class="input-group-addon">
                                                                    <span class="glyphicon glyphicon-ok" id="btnSave1" onclick="btn_offersSaveclick()">
                                                                    </span><span id="btnSave" onclick="btn_offersSaveclick()">Save</span>
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
