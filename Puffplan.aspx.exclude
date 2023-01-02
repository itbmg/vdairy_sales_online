<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Puffplan.aspx.cs" Inherits="Puffplan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
     <link href="js/DateStyles.css?v=3004" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
    <style type="text/css">
        .ddlsize
        {
            width: 200px;
            height: 30px;
            font-size: 16px;
            border: 1px solid gray;
            border-radius: 7px 7px 7px 7px;
        }
        .datepicker
        {
            border: 1px solid gray;
            background: url("Images/CalBig.png") no-repeat scroll 99%;
              width: 70%;
            top: 0;
            left: 0;
            height: 20px;
            font-weight: 700;
            font-size: 12px;
            cursor: pointer;
            border: 1px solid gray;
            margin: .5em 0;
            padding: .6em 20px;
            border-radius: 10px 10px 10px 10px;
            filter: Alpha(Opacity=0);
            box-shadow: 3px 3px 3px #ccc;
        }
        .chkclass
{
    width:200px;
    height:34px;
    font-size:18px;
background: #2a81d7;
background: -moz-linear-gradient(0% 100% 90deg,#206bcb,#3e9ee5);
/*background: -webkit-gradient(linear,0% 0,0% 100%,from(#0066CC),to(#0066CC));*/
border-top: 1px solid #2a73a6;
border-right: 1px solid #165899;
border-bottom: 1px solid #07428f;
border-left: 1px solid #165899;
-moz-box-shadow: inset 0 1px 0 0 #62b1e9;
-webkit-box-shadow: inset 0 1px 0 0 #62b1e9;
/*box-shadow: inset 0 1px 0 0 #0066CC;*/
color: #FFF;
cursor: pointer;
text-decoration: none;
text-shadow: 0 -1px 1px #1d62ab;
border-radius: 4px 4px 4px 4px;
border-style:none;
}
/*.chkclass:hover
{
    width:200px;
    height:34px;
    font-size:18px;
background: #0066CC;
background: -moz-linear-gradient(0% 100% 90deg,#206bcb,#3e9ee5);
background: -webkit-gradient(linear,0% 0,0% 100%,from(#0066CC),to(#0066CC));
border-top: 1px solid #2a73a6;
border-right: 1px solid #165899;
border-bottom: 1px solid #07428f;
border-left: 1px solid #165899;
-moz-box-shadow: inset 0 1px 0 0 #62b1e9;
-webkit-box-shadow: inset 0 1px 0 0 #62b1e9;
box-shadow: inset 0 1px 0 0 #0066CC;
color: #FFF;
cursor: pointer;
text-decoration: none;
text-shadow: 0 -1px 1px #1d62ab;
border-radius: 4px 4px 4px 4px;
}*/
.label1
{
    left: 120%;
    position:absolute;
}
.label2
{
    left: 200%;
      position:absolute;
}
.label3
{
    left: 300%;
      position:absolute;
}
        input[type=number]::-webkit-inner-spin-button,
         input[type=number]::-webkit-outer-spin-button
        {
            -webkit-appearance: none;
            margin: 0;
        }
    </style>
     <script type="text/javascript">
         $(function () {
             FillPlantDispatches();
             $("#datepicker").datepicker({ dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
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
         });
         function FillPlantDispatches() {
             var data = { 'operation': 'Get_So_Plant_Despatches' };
             var s = function (msg) {
                 if (msg) {
                     bindPlantDispatches(msg);
                 }
                 else {
                 }
             };
             var e = function (x, h, e) {
             };
             $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
             callHandler(data, s, e);
         }
         function bindPlantDispatches(msg) {
             $('#divFillScreen').removeTemplate();
             $('#divFillScreen').setTemplateURL('PuffPlan.htm');
             $('#divFillScreen').processTemplate(msg);
             var rowPlan = $("#tablePlandetails tr:gt(0)");
             $(rowPlan).each(function (i, obj) {
                 if ($(this).find('#btnPlan').val() == "Planned") {
                     $(this).find('#btnPlan').css('backgroundColor', '#FA7E75');
                 }
                 else {
                     $(this).find('#btnPlan').css('backgroundColor', '#59FA87');
                 }
                 if ($(this).find('#btnDispatch').val() == "Yes") {
                     $(this).find('#btnDispatch').css('backgroundColor', '#FA7E75');
                 }
                 else {
                     $(this).find('#btnDispatch').css('backgroundColor', '#59FA87');
                 }
             });
         }
         //        function checked(thisid) {
         //            window.location = "TripAssignment.aspx";
         //        } document.getElementById("btnSynctoDB").style.backgroundColor = "#FA7E75";
         var DispatchStatus = "";
         function btnGetPlanClick(id) {
             DispatchStatus = "Plan";
             var DispID = $(id).closest("tr").find("#hdnDispSno").val();
             var btnPlan = $(id).closest("tr").find("#btnPlan").val();
             if (btnPlan == "Planned") {
                 alert("Plan Completed");
                 return false;
             }
             var btnDispatch = $(id).closest("tr").find("#btnDispatch").val();
             if (btnDispatch == "Yes") {
                 alert("Despatch Completed");
                 return false;
             }
             getDispatchValues(DispID);
         }
         function btnGetDispatchClick(id) {
             DispatchStatus = "Despatch";
             var DispID = $(id).closest("tr").find("#hdnDispSno").val();
             var btnPlan = $(id).closest("tr").find("#btnPlan").val();
             if (btnPlan == "Plan") {
                 alert("Despatch not yet Planned");
                 return false;
             }
             alert("sorry,you dont have permission");
             return false;
         }
         function getDispatchValues(DispID) {
             var date = document.getElementById('datepicker').value;
             var data = { 'operation': 'GetDispatchValues', 'DispSno': DispID, 'DispatchStatus': DispatchStatus, 'IndentDate': date };
             var s = function (msg) {
                 if (msg) {
                     if (msg == "Success") {
                         window.location = "IndentAssignMent.aspx";
                     }
                     else {
                         return false;
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
         function btnGeneratePlanClick() {
             var date = document.getElementById('datepicker').value;
             if (date == "" || date == "mm/dd/yyyy") {
                 alert("Please Select Date");
                 return false;
             }
             var data = { 'operation': 'GetDispatchPlanStatus', 'IndentDate': date };
             var s = function (msg) {
                 if (msg) {
                     bindPlantDispatches(msg);
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
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table  style="display:none;">
        <tr>
            <td>
                <label>
                     Date:</label>
            </td>
            <td >
                <input type="text" name="journey_date" class="datepicker" tabindex="3" readonly="readonly"
                    id="datepicker" placeholder="DD-MM-YYYY" />
            </td>
            <td>
                <input type="button" id="btnGenerate" value="Generate" class="SaveButton" onclick="btnGeneratePlanClick();"
                    style="width: 120px; height: 30px; font-size: 16px;" />
            </td>
        </tr>
    </table>
    <div id="divFillScreen">
        </div>
</asp:Content>


