<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DashBoard.aspx.cs" Inherits="DashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script src="Kendo/jquery.min.js" type="text/javascript"></script>
    <script src="Kendo/kendo.all.min.js" type="text/javascript"></script>
    <link href="Kendo/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="Kendo/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link href="Css/Timeline.css" rel="stylesheet" type="text/css" />
     <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <link href="Css/RouteWiseTimeLine.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            Details();
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
        function Details() {
            
            var data = { 'operation': 'GetDashBoardDetails'};
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location.assign("Login.aspx");
                    }
                    else {
                      BindDashBoard(msg);
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
        function BindDashBoard(msg) {
            $('#divtemplate').removeTemplate();
            $('#divtemplate').setTemplateURL('TimeLineDetails.htm');
            $('#divtemplate').processTemplate(msg);
        }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div style="width: 100%; height: 550px; background-color: #fff">
<div id="divtemplate">

    </div>
    </div>
</asp:Content>

