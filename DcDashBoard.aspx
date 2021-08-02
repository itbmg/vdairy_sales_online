<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DcDashBoard.aspx.cs" Inherits="DcDashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script src="js/jquery-1.4.4.js?v=3004" type="text/javascript"></script>
    <script src="js/newjs/jquery.js?v=3004" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3004" type="text/javascript"></script>
    <link href="jquery.jqGrid-4.5.2/js/i18n/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <script src="jquery.jqGrid-4.5.2/src/i18n/grid.locale-en.js" type="text/javascript"></script>
    <script src="jquery.jqGrid-4.5.2/js/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script src="jquery-ui-1.10.3.custom/js/jquery-ui.js" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js?v=3004" type="text/javascript"></script>
    <script src="js/newjs/jquery-ui.js?v=3004" type="text/javascript"></script>
    <script src="js/JTemplate.js" type="text/javascript"></script>
    <link href="bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap/RouteWiseTimeLine.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            Get_Dc_Details();
        });
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
        function Get_Dc_Details() {
            var data = { 'operation': 'Get_Dc_Details' };
            var s = function (msg) {
                if (msg) {
                    var j = 1;
                    var branchname = "";
                    var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
                    $('#tbl_Dcdashboard_value').append('<tr ><th>Sno</th><th scope="Category Name">BranchName</th><th scope="Category Name">IndentDate</th><th>DcNumber</th><th>StockTransfer Status</th><th>Status</th></td></tr>');

                    for (var i = 0; i < msg.length; i++) {
                        var tablerowcnt = document.getElementById("tbl_Dcdashboard_value").rows.length;
                        $('#tbl_Dcdashboard_value').append('<tr style="align:left;"><td data-title="sno">' + j + '</td><th scope="Category Name">' + msg[i].BranchName + '<i   aria-hidden="true"></th><th scope="Category Name">' + msg[i].Dcdate + '<i   aria-hidden="true"></th><th scope="Category Name">' + msg[i].DCNumber + '</th><th scope="Category Name">' + msg[i].Stocktransfer + '</th><th><span id="spnmoniterqty" class="badge bg-green"><span class="clsmoniterqty">' + msg[i].Status + '</span></span></th></td></tr>');
                        j++;
                        $('#tbl_Dcdashboard_value').append('</br>')
                        $('#tbl_Dcdashboard_value').append('</br>')
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            DashBoard <small>Preview</small>
        </h1>
        <ol class="breadcrumb">
        </ol>
    </section>
    <!-- Main content -->
    <section class="content">
        <div class="row">
            <div class="col-md-6" style="width: 100%;">
                <!-- AREA CHART -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            <i style="padding-right: 5px;" class="fa fa-cog"></i>DC Dash Board</h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data- widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>

                      <div class="box-body">
                        <div class="box-body no-padding">
                            <table  class="table table-bordered table-hover dataTable no-footer" role="grid" aria-
                                describedby="example2_info" id="tbl_Dcdashboard_value">
                            </table>
                        </div>
                    </div>
                        <%--   <div class="box-body">
                        <div class="box-body no-padding">
                            <div id="divFillScreen" style="height: 100%;">
                </div>
                        </div>
                    </div>--%>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
