<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SwitchAccount.aspx.cs" Inherits="SwitchAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="JIC/JIC.js?v=101" type="text/javascript"></script>
    <script src="JSF/imagezoom.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            get_employewise_SalesLogins_details();
        });
        function isFloat(evt) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                //if dot sign entered more than once then don't allow to enter dot sign again. 46 is the code for dot sign
                var parts = evt.srcElement.value.split('.');
                if (parts.length > 1 && charCode == 46)
                    return false;
                return true;
            }
        }
        function ValidateAlpha(evt) {
            var keyCode = (evt.which) ? evt.which : evt.keyCode
            if ((keyCode < 65 || keyCode > 90) && (keyCode < 97 || keyCode > 123) && keyCode != 32)

                return false;
            return true;
        }
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
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
        function get_employewise_SalesLogins_details() {
            var data = { 'operation': 'get_employewise_SalesLogins_details' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillparlordetails(msg);
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
        function fillparlordetails(msg) {
            var results = '<div  class="box-body"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-="" describedby="example2_info" id="tbl_Stores_value">';
            results += '<thead><tr role="row" class="trbgclrcls"><th scope="col" style="width: 10px">Sno</th><th scope="col">BranchName</th><th scope="col" style="font-weight: bold;">Switch To Account</th></tr></thead></tbody>';
            var totalsalevalue = 0;
            var totalpurchasevalue = 0;
            var totalexpencesvalue = 0;
            var totalprofit = 0;
            for (var i = 0; i < msg.length; i++) {
                var k = i + 1;
                results += '<tr><td scope="row" style="text-align: center; font-weight: bold;">' + k + '</td>';
                results += '<td scope="row" class="1" style="text-align: center; font-weight: bold;" >' + msg[i].Branchname + '</td>';
                results += '<td scope="row" class="2" style="display:none" >' + msg[i].BranchSno + '</td>';
                results += '<td style="text-align: center;" data-title="brandstatus"><button type="button" title="Switch To Account" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmeaddress(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>';
                //results += '<td style="text-align: center;"><a href="#" onclick="viewdetails(this);" title="View Details" class="tip btn btn-success btn-xs"><i class="fa fa-2x fa-plus-circle"></i></a></td>';
                results += '<td style="display:none" class="3">' + msg[i].empid + '</td></tr>';
            }
            results += '</table></div>';
            $("#divsaledata").html(results);
        }

        function getmeaddress(thisid) {
            var BranchId = $(thisid).parent().parent().children('.2').html();
            var empid = $(thisid).parent().parent().children('.3').html();
            var data = { 'operation': 'get_EmployeeBranchAssinged_Details', 'empid': empid, 'BranchId': BranchId };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        window.open("Delivery_Collection_Report.aspx", "_self");
                    }
                    else {
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            }; $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Home</h1>
        <ol class="breadcrumb">
            <li><a href=""><i class="fa fa-dashboard"></i>Home</a></li>
            <li class="active">Dashboard</li>
        </ol>
    </section>
    <section class="content">
        <div class="row">
            <div class="col-xs-12">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-header">
                                <h3 class="box-title">
                                    Branch Details</h3>
                            </div>
                            <div class="box-body">
                                <div id="divsaledata">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
