<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GatepassEdit.aspx.cs" Inherits="GatepassEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <script type="text/javascript">
        $(function () {
            var date = new Date();
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) month = "0" + month;
            if (day < 10) day = "0" + day;
            today = year + "-" + month + "-" + day;
            $('#txtfromdate').val(today);
            $('#txttodate').val(today);
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
         function btnget_Gatepassdetails() {
             var data = { 'operation': 'get_GetpassDetails' };
             var s = function (msg) {
                 if (msg) {
                     if (msg.length > 0) {
                         fillgatepassdetails(msg);
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
         function fillgatepassdetails(msg) {
             var results = '<div  style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
             results += '<thead><tr class="trbgclrcls"><th scope="col" class="thcls">LedgerName</th><th scope="col" class="thcls">LedgerCode</th><th scope="col"></th></tr></thead></tbody>';
             var k = 1;
             var l = 0;
             var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
             for (var i = 0; i < msg.length; i++) {
                 results += '<tr style="background-color:' + COLOR[l] + '">';
                 results += '<td scope="row"  class="1 tdmaincls" >' + msg[i].name + '</td>';
                 results += '<td data-title="brandstatus"  class="2">' + msg[i].ledgercode + '</td>';
                 results += '<td style="display:none" class="3">' + msg[i].sno + '</td>';
                 results += '<td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getmeCashOthers(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td></tr>';

                 l = l + 1;
                 if (l == 4) {
                     l = 0;
                 }
             }
             results += '</table></div>';
             $("#div_Others").html(results);
         }

     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="div_CashCollection" style="display: none">
        <div class="box-header with-border">
            <h3 class="box-title">
                <i style="padding-right: 5px;" class="fa fa-cog"></i>Others Details
            </h3>
        </div>
        <div class="box-body">
            <div id="div_gatepass">
            </div>
            <div id='gatepass_fillform'>
                <table>
                        <tr>
                            <td>
                                <label>
                                    From Date:</label>
                            </td>
                            <td>
                                <input type="date" id="txtfromdate" class="form-control" />
                            </td>
                            <td>
                                <label>
                                    To Date:</label>
                            </td>
                            <td>
                                <input type="date" id="txttodate" class="form-control" />
                            </td>
                            <td style="width: 5px;"></td>
                            <td>
                                <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="btnget_Gatepassdetails()"><i class="fa fa-refresh"></i>GetDetails </button>
                            </td>
                        </tr>
                    </table>
            </div>
        </div>
    </div>
</asp:Content>

