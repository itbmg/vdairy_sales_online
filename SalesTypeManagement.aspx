<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SalesTypeManagement.aspx.cs" Inherits="SalesTypeManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css">
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css">
    <%-- <script src="plugins/morris/morris.js" type="text/javascript"></script>--%>
    <link rel="stylesheet" type="text/css" href="dist/css/AdminLTE.css" />
    <link rel="stylesheet" href="dist/css/AdminLTE.min.css">
    <link rel="stylesheet" href="dist/css/skins/_all-skins.min.css">
    <script src="js/jquery.js"></script>
    <link href="css/font-awesome.min.css" rel="stylesheet">
    <script src="JSF/jquery.blockUI.js" type="text/javascript"></script>
    <link href="css/custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);
        });
    </script>
    <style type="text/css">
        th
        {
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            updatesalestype();
        });
        function salestypevalidation() {
            var category = document.getElementById('ddlCategory').value;
            if (category == "") {
                alert("Select Category");
                return false;
            }
            var salestype = document.getElementById('txt_salestype').value;
            if (salestype == "") {
                alert("Enter Sales Type");
                return false;
            }
            var salestypeflag = document.getElementById('cmb_flag').value;
            if (salestypeflag == "") {
                alert("Select Flag");
                return false;
            }
            var operationtype = document.getElementById('btn_save').value;
            var sno = serial;
            var data = { 'operation': 'salestypemanage', 'category': category, 'sno': sno, 'salestype': salestype, 'salestypeflag': salestypeflag, 'operationtype': operationtype };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    else {
                        alert(msg);
                        salestypeclear();
                        updatesalestype();
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
        function salestypeclear() {
            document.getElementById('txt_salestype').value = "";
            document.getElementById('cmb_flag').selectedIndex = 0;
            document.getElementById('btn_save').value = "SAVE";
        }
        function updatesalestype() {
            var data = { 'operation': 'updatesalestypemanage' };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    else {
                        if (msg.length > 0) {
                            BindGridSales(msg);

                        }
                        else {
                            alert(msg);
                        }
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
        var serial = 0;
        function BindGridSales(msg) {
            var l = 0;
            var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col"></th><th scope="col">Category</th><th scope="col">Sales Type</th><th scope="col">Status</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                var status = 'InActive';
                if (msg[i].flag == '1') {
                    status = 'Active';
                }
                results += '<tr style="background-color:' + COLOR[l] + '"><td><input id="btn_poplate" type="button"  onclick="getme(this)" name="submit" class="btn btn-primary" value="Choose" /></td>';
                results += '<td scope="row" class="1" style="text-align:center;">' + msg[i].category + '</td>';
                results += '<td scope="row" class="2" style="text-align:center;">' + msg[i].salestype + '</td>';
                results += '<td data-title="Capacity" class="3">' + status + '</td>';
                results += '<td style="display:none" class="4">' + msg[i].sno + '</td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }
        function getme(thisid) {
            var category = $(thisid).parent().parent().children('.1').html();
            var salestype = $(thisid).parent().parent().children('.2').html();
            var status = $(thisid).parent().parent().children('.3').html();
            var sno = $(thisid).parent().parent().children('.4').html();
            document.getElementById('ddlCategory').value = category;
            document.getElementById('txt_salestype').value = salestype;
            document.getElementById('cmb_flag').value = status;
            document.getElementById('btn_save').value = "MODIFY";
            serial = sno;
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
        <h1>
            Sales Type Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Masters</a></li>
            <li><a href="#">Sales Type Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Sales Type Details
                </h3>
            </div>
            <div class="box-body">
                <table align="center">
                <tr>
                        <td>
                            <label for="lblSalesType">
                                Category:</label>
                        </td>
                        <td style="height: 40px;">
                               <select id="ddlCategory" class="form-control">
                                <option>CashAgents</option>
                                <option>DueAgents</option>
                                <option>Institutional</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="lblSalesType">
                                Sales Type:</label>
                        </td>
                        <td style="height: 40px;">
                            <input type="text" id="txt_salestype" class="form-control" placeholder="Enter Sales Type" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="lblFlag">
                                Flag:</label>
                        </td>
                        <td style="height: 40px;">
                            <select id="cmb_flag" class="form-control">
                                <option>Active</option>
                                <option>InActive</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="height: 40px;">
                            <input type="button" id="btn_save" value="SAVE" onclick="salestypevalidation();"
                                class="btn btn-primary" />
                            <input type="button" id="btn_clear" value="CLEAR" class="btn btn-danger" onclick="salestypeclear();" />
                        </td>
                    </tr>
                </table>
                <div id="div_Deptdata" style="width: 100%; text-align: center; height: 400px; overflow: auto;">
                </div>
            </div>
        </div>
    </section>
</asp:Content>
