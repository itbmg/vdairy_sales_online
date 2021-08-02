<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Helpermaster.aspx.cs" Inherits="Helpermaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <link href="Css/style.css" rel="stylesheet" type="text/css" />
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <script src="js/date.format.js" type="text/javascript"></script>
     <script type="text/javascript">
         $(function () {
             FillDespNames();
         });
             function FillDespNames() {
                 var data = { 'operation': 'GetsoandPlantDespNames' };
                 var s = function (msg) {
                     if (msg) {
                         $('#divHelperMaster').removeTemplate();
                         $('#divHelperMaster').setTemplateURL('HelperMaster.htm');
                         $('#divHelperMaster').processTemplate(msg);
                     }
                     else {
                     }
                 };
                 var e = function (x, h, e) {
                 };
                 $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                 callHandler(data, s, e);
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
             function btnHelperMasterSaveclick() {
                 var rows = $("#tableHelperMasterDetails tr:gt(0)");
                 var HelperMasterdetails = new Array();
                 $(rows).each(function (i, obj) {
                     if ($(this).find('#txtAmount').val() != "") {
                         HelperMasterdetails.push({ Despsno: $(this).find('#hdnDespsno').val(), Amount: $(this).find('#txtAmount').val(), First: $(this).find('#txt1helper').val(), Second: $(this).find('#txt2helper').val(), Third: $(this).find('#txt3helper').val(), Fourth: $(this).find('#txt4helper').val() });
                     }
                 });
                 if (HelperMasterdetails.length == 0) {
                     alert("Please Enter Helper Master details");
                     return false;
                 }
                 var Data = { 'operation': 'btnHelperMasterSaveclick', 'HelperMasterdetails': HelperMasterdetails };
                 var s = function (msg) {
                     if (msg) {
                         alert(msg);
                         $('#divHelperMaster').removeTemplate();
                         $('#divHelperMaster').setTemplateURL('HelperMaster.htm');
                         $('#divHelperMaster').processTemplate();
                     }
                     else {
                     }
                 };
                 var e = function (x, h, e) {
                 };
                 $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                 CallHandlerUsingJson(Data, s, e);
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <div id="divHelperMaster">
 </div>
</asp:Content>

