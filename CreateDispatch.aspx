<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CreateDispatch.aspx.cs" Inherits="CreateDispatch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }

    </script>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);

        });
    </script>
    <script type="text/javascript">
        $(function () {
            $('#trsalesoffice').css('display', 'none ');
            $('#trroutename').css('display', 'none ');
            $('#tragentname').css('display', 'none ');

        });
        function dispatchtype_onchange() {
            var dispatchvalue = document.getElementById('cmb_RouteName').value;
            if (dispatchvalue == "0") {
                $('#trsalesoffice').css('display', 'none '); 
                $('#trroutename').css('display', 'none ');
                $('#tragentname').css('display', 'none ');
            }
            if (dispatchvalue == "1") {
                $('#trsalesoffice').css('display', 'block ');
                $('#trroutename').css('display', 'block ');
                $('#tragentname').css('display', 'none ');
                getsalesoffices();
            }
            if (dispatchvalue == "2") {
                $('#trsalesoffice').css('display', 'block ');
                $('#trroutename').css('display', 'block ');
                $('#tragentname').css('display', 'block ');
               // getsalesoffices();
            }
        }
        function getsalesoffices() {
            //var cmbcatgryname = document.getElementById("cmb_brchprdt_Catgry_name").value;
            var data = { 'operation': 'GetSalesOfficeAndPlant' };
            var s = function (msg) {
                if (msg) {
                    fill_salesoffices(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
//                if (x.status && x.status == 400) {
//                    alert(x.responseText);
//                    window.location.assign("Login.aspx");
//                }
//                else {
//                    alert("something went wrong");
//                }
            };
            //callHandler(data, s, e);
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            AscCallHandler(data, s, e);
        }
        function fill_salesoffices(msg) {
            var brnchsubcategory = document.getElementById('Cmbsalesoffice');
            var length = brnchsubcategory.options.length;
            document.getElementById("Cmbsalesoffice").options.length = null;
            document.getElementById("Cmbsalesoffice").value = "select";
            //        for (i = 0; i < length; i++) {
            //            prdtsubcategory.options[i] = null;
            //        } 
            var opt = document.createElement('option');
            opt.innerHTML = "Select";
            brnchsubcategory.appendChild(opt);

            for (var i = 0; i < msg.length; i++) {
                if (msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    brnchsubcategory.appendChild(opt);
                }
            }
        }
        function salesoffice_onchange() {
            var salseofficesno = document.getElementById('Cmbsalesoffice').value;
            var data = { 'operation': 'GetSOAndPlantRoutes', 'salseofficesno': salseofficesno };
            var s = function (msg) {
                if (msg) {
                    fill_routes(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
                
            };
            //callHandler(data, s, e);
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            AscCallHandler(data, s, e);
        }
        function soroute_onchange() {
            var routesno = document.getElementById('cmb_routename').value;
            var data = { 'operation': 'GetRoutesAgents', 'routesno': routesno };
            var s = function (msg) {
                if (msg) {
                    fill_Agents(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {

            };
            //callHandler(data, s, e);
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            AscCallHandler(data, s, e);
        }
        function fill_Agents(msg) {
            var cmb_agentname = document.getElementById('cmb_agentname');
            var length = cmb_agentname.options.length;
            document.getElementById("cmb_agentname").options.length = null;
            document.getElementById("cmb_agentname").value = "select";

            var opt = document.createElement('option');
            opt.innerHTML = "Select";
            cmb_agentname.appendChild(opt);

            for (var i = 0; i < msg.length; i++) {
                if (msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    cmb_agentname.appendChild(opt);
                }
            }
        }
        function fill_routes(msg) {
            var cmb_routename = document.getElementById('cmb_routename');
            var length = cmb_routename.options.length;
            document.getElementById("cmb_routename").options.length = null;
            document.getElementById("cmb_routename").value = "select";
           
            var opt = document.createElement('option');
            opt.innerHTML = "Select";
            cmb_routename.appendChild(opt);

            for (var i = 0; i < msg.length; i++) {
                if (msg[i].BranchName != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    cmb_routename.appendChild(opt);
                }
            }
        }
        function AscCallHandler(d, s, e) {
            $.ajax({
                url: 'DairyFleet.axd',
                data: d,
                type: 'GET',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: false,
                cache: true,
                success: s,
                error: e
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600">
    </asp:ToolkitScriptManager>
    <div>
        <asp:UpdateProgress ID="updateProgress1" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0;
                    right: 0; left: 0; z-index: 9999; background-color: #FFFFFF; opacity: 0.7;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="thumbnails/loading.gif"
                        Style="padding: 10px; position: absolute; top: 40%; left: 40%; z-index: 99999;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <label id="lblroutename">
                            Dispatch Type:</label>
                    </td>
                    <td>
                        <select id="cmb_RouteName" class="txtsize" onchange="return dispatchtype_onchange();">
                        <option value="0">select</option>
                                    <option value="1" >Route Dispatch</option>
                                    <option value="2">Agent Dispatch</option>
                                    <option value="3">Local Sales</option>
                                    <option value="4">Employee Sales</option>
                        </select>
                    </td>
                    <td>
                    </td>
                    <td>
                    <input type="radio" name="vehicletype" value="1">Own Vehicle
                    <input type="radio" name="vehicletype" value="2">Private Vehicle
                    </td>
                </tr>
               <tr id="trsalesoffice">
               <td>
               <label id="lblsalesoffice">
                            Sales Office:</label>
               </td> 
               <td>
               <select id="Cmbsalesoffice" class="txtsize" onchange="return salesoffice_onchange();">
               </select>
               </td>
               </tr>
               <tr id="trroutename">
               <td>
               <label id="lblroute">
                            Route Name:</label>
               </td>
                <td>
               <select id="cmb_routename" class="txtsize" onchange="return soroute_onchange();">
               </select>
               </td>
               </tr>
               <tr id="tragentname">
               <td>
                <label id="lblagentname">
                            Agent Name:</label>
               </td>
               <td>
               <select id="cmb_agentname" class="txtsize">
               </select>
               </td>
               </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
