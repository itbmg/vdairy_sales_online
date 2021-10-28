<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="AgentLedger.aspx.cs" Inherits="AgentLedger" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="Js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
     <link href="jquery.jqGrid-4.5.2/js/i18n/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <script src="jquery.jqGrid-4.5.2/src/i18n/grid.locale-en.js" type="text/javascript"></script>
    <script src="jquery.jqGrid-4.5.2/js/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script src="jquery-ui-1.10.3.custom/js/jquery-ui.js" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js?v=3004" type="text/javascript"></script>
    <script src="js/newjs/jquery-ui.js?v=3004" type="text/javascript"></script>
     <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css"/>
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script language="javascript" type="text/javascript">
        function CallPrint(strid) {
            var divToPrint = document.getElementById(strid);
            var newWin = window.open('', 'Print-Window', 'width=400,height=400,top=100,left=100');
            newWin.document.open();
            newWin.document.write('<html><body   onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
        }
        function OrderValidate() {
            var fromDate = document.getElementById('<%=txtFromdate.ClientID %>').value;
            var txtTodate = document.getElementById('<%=txtTodate.ClientID %>').value;
            if (fromDate == "") {
                alert("Select From Date");
                return false;
            }
            if (txtTodate == "") {
                alert("Select To Date");
                return false;
            }
        }
        function rowno(rowindex) {
            var i, IndnentNo, Row; var Type = "";
            i = parseInt(rowindex) + 1;
            var table = document.getElementById('<%=grdReports.ClientID %>');
            Row = table.rows[i];
            IndnentNo = Row.cells[2].innerHTML;
            Type = Row.cells[3].innerHTML;
            if (Type == "Indent") {
                var data = { 'operation': 'LedgerDetailsClick', 'IndnentNo': IndnentNo, 'Type': Type };
                var s = function (msg) {
                    if (msg) {
                        $('#divLedgerScreen').removeTemplate();
                        $('#divLedgerScreen').setTemplateURL('LedgerIndent.htm');
                        $('#divLedgerScreen').processTemplate(msg);
                        $('#divLedgerDetails').css('display', 'block');
                        IndentCal();
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                callHandler(data, s, e);
            }
            else {
                alert("Collection not Ready");
                return false;
            }
        }
        function IndentCal() {
            var TotDelivery = 0.0;
            $('.DeliverQtyClass').each(function (i, obj) {
                if ($(this).text() == "") {
                }
                else {
                    TotDelivery += parseFloat($(this).text());
                }
            });
            document.getElementById('txtTotQty').innerHTML = parseFloat(TotDelivery).toFixed(2);
            var TotAmount = 0.0;
            $('.AmountClass').each(function (i, obj) {
                if ($(this).text() == "") {
                }
                else {
                    TotAmount += parseFloat($(this).text());
                }
            });
            document.getElementById('txtTotAmount').innerHTML = parseFloat(TotAmount).toFixed(2);
        }
        function LedgerCloseClick() {
            $('#divLedgerDetails').css('display', 'none');
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
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);
        });
        function showAgentLedger() {
            $("#div_AgentLedger").css("display", "block");
            $("#div_AgentDocumentUpload").css("display", "none");
        }
        function showdiv_AgentDocumentUpload() {
            FillSalesOffice()
            $("#div_AgentLedger").css("display", "none");
            $("#div_AgentDocumentUpload").css("display", "block");
        }
        function callHandler_nojson_post(d, s, e) {
            $.ajax({
                url: 'DairyFleet.axd',
                type: "POST",
                // dataType: "json",
                contentType: false,
                processData: false,
                data: d,
                success: s,
                error: e
            });
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
        }
        function ddlSalesOfficeChange(ID) {
            var BranchID = ID.value;
            var data = { 'operation': 'GetSalesRoutes', 'BranchID': BranchID };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Session Expired") {
                        alert(msg);
                        window.location = "Login.aspx";
                    }
                    BindRouteName(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindRouteName(msg) {
            document.getElementById('ddlRouteName_photo').options.length = "";
            var veh = document.getElementById('ddlRouteName_photo');
            var length = veh.options.length;
            for (i = length - 1; i >= 0; i--) {
                veh.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Route Name";
            opt.value = "";
            veh.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].RouteName;
                    opt.value = msg[i].rid;
                    veh.appendChild(opt);
                }
            }
        }
        function ddlRouteNameChange(id) {
            FillAgentName(id.value);
        }
        function FillAgentName(RouteID) {
            var data = { 'operation': 'GetAgents', 'RouteID': RouteID };
            var s = function (msg) {
                if (msg) {
                    BindAgentName(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function BindAgentName(msg) {
            document.getElementById('ddlAgentName_photo').options.length = "";
            var ddlAgentName = document.getElementById('ddlAgentName_photo');
            var length = ddlAgentName.options.length;
            for (i = length - 1; i >= 0; i--) {
                ddlAgentName.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Agent Name";
            opt.value = "";
            ddlAgentName.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].BranchName;
                    opt.value = msg[i].Sno;
                    ddlAgentName.appendChild(opt);
                }
            }
        }

        function hasExtension(fileName, exts) {
            return (new RegExp('(' + exts.join('|').replace(/\./g, '\\.') + ')$')).test(fileName);
        }

        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#main_img,#img_1').attr('src', e.target.result).width(200).height(200);
                    //                    $('#main_img1,#img_1').attr('src', e.target.result).width(200).height(200);
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
        function getFile() {
            document.getElementById("file").click();
        }
        //----------------> convert base 64 to file
        function dataURItoBlob(dataURI) {
            // convert base64/URLEncoded data component to raw binary data held in a string
            var byteString;
            if (dataURI.split(',')[0].indexOf('base64') >= 0)
                byteString = atob(dataURI.split(',')[1]);
            else
                byteString = unescape(dataURI.split(',')[1]);
            // separate out the mime component
            var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];
            // write the bytes of the string to a typed array
            var ia = new Uint8Array(byteString.length);
            for (var i = 0; i < byteString.length; i++) {
                ia[i] = byteString.charCodeAt(i);
            }
            return new Blob([ia], { type: 'image/jpeg' });
        }
        function upload_profile_pic() {
            var dataURL = document.getElementById('main_img').src;
            var div_text = $('#yourBtn').text().trim();
            var blob = dataURItoBlob(dataURL);

            var ddlAgentName = document.getElementById('ddlAgentName_photo').value;
            if (ddlAgentName == "" || ddlAgentName == "select") {
                alert("Please Select Agent Name");
                $("#ddlAgentName").focus();
                return false;
            }
            var Data = new FormData();
            Data.append("operation", "Agent_profile_pic_files_upload");
            Data.append("sno", ddlAgentName);
            Data.append("blob", blob);
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    //                    $('#btn_upload_profilepic').css('display', 'none');
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler_nojson_post(Data, s, e);
        }

        function hasExtension(fileName, exts) {
            return (new RegExp('(' + exts.join('|').replace(/\./g, '\\.') + ')$')).test(fileName);
        }

        function readURL1(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    //                    $('#main_img,#img_1').attr('src', e.target.result).width(200).height(200);
                    $('#main_img1,#img_1').attr('src', e.target.result).width(200).height(200);
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
        function getshopFile() {
            document.getElementById("file1").click();
        }
        //----------------> convert base 64 to file
        function dataURItoBlob(dataURI) {
            // convert base64/URLEncoded data component to raw binary data held in a string
            var byteString;
            if (dataURI.split(',')[0].indexOf('base64') >= 0)
                byteString = atob(dataURI.split(',')[1]);
            else
                byteString = unescape(dataURI.split(',')[1]);
            // separate out the mime component
            var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];
            // write the bytes of the string to a typed array
            var ia = new Uint8Array(byteString.length);
            for (var i = 0; i < byteString.length; i++) {
                ia[i] = byteString.charCodeAt(i);
            }
            return new Blob([ia], { type: 'image/jpeg' });
        }
        function upload_shopprofile_pic() {
            var dataURL = document.getElementById('main_img1').src;
            var div_text = $('#yourBtn1').text().trim();
            var blob = dataURItoBlob(dataURL);

            var ddlAgentName = document.getElementById('ddlAgentName_photo').value;
            if (ddlAgentName == "" || ddlAgentName == "select") {
                alert("Please Select Agent Name");
                $("#ddlAgentName").focus();
                return false;
            }
            var Data = new FormData();
            Data.append("operation", "Agent_Shopprofile_pic_files_upload");
            Data.append("sno", ddlAgentName);
            Data.append("blob", blob);
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    //                    $('#btn_upload_profilepic').css('display', 'none');
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler_nojson_post(Data, s, e);
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
    <section class="content-header">
        <h1>
            Agent Ledger<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Operations</a></li>
            <li><a href="#">Agent Ledger</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
         <div>
                    <ul class="nav nav-tabs">
                        <li id="liDepartment" class="active"><a data-toggle="tab" href="#" onclick="showAgentLedger()">
                            <i class="fa fa-street-view"></i>&nbsp;&nbsp;AgentLedger</a></li>
                       <%-- <li id="liAddress" class=""><a data-toggle="tab" href="#" onclick="showdiv_AgentDocumentUpload()">
                            <i class="fa fa-file-text"></i>&nbsp;&nbsp;AgentDocumentUpload</a></li>--%>
                    </ul>
                </div>
                 <div id="div_AgentLedger">
                <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Agent Ledger Details
                </h3>
            </div>
            <div class="box-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table align="center">
                            <tr>
                                <td>
                                    <asp:Label ID="lblroutename" runat="server" style="font-weight:bold;">Route Name:</asp:Label>
                                    <asp:DropDownList ID="ddlRouteName" runat="server" CssClass="form-control" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlRouteName_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblagentname" runat="server" style="font-weight:bold;">Agent Name:</asp:Label>
                                    <asp:DropDownList ID="ddlAgentName" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 40px;">
                                 <span id="lblfrom_date" style="font-weight:bold;">From Date</span>&nbsp;
                                    <asp:TextBox ID="txtFromdate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtFromdate" Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="width: 1px;">
                                </td>
                                <td style="height: 40px;">
                                 <span id="lbltodate" style="font-weight:bold;">To Date</span>&nbsp;
                                    <asp:TextBox ID="txtTodate" runat="server" Width="205px" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtTodate"
                                        Format="dd-MM-yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnGenerate" Text="Generate" runat="server" OnClientClick="OrderValidate();"
                                        CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlhide" Visible="false" runat="server">
                        <div id="divPrint">
                            <div style="width: 100%;">
                                <div style="width: 11%; float: left;">
                                    <img src="Images/Vyshnavilogo.png" alt="Vyshnavi Dairy" width="95px" height="90px" />
                                </div>
                                <div style="left: 0%; text-align: center;">
                                    <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="26px" ForeColor="#0252aa"
                                        Text=""></asp:Label>
                                    <br />
                                </div>
                                <div style="width: 100%;">
                                    <span style="font-size: 15px; font-weight: bold; padding-left: 27%; text-decoration: underline;
                                        color: #0252aa;">Agent Ledger Statement</span><br />
                                    <div>
                                        <div style="width: 40%; float: left; padding-left: 7%;">
                                            <span style="font-weight: bold;">Agent Name: </span>
                                            <asp:Label ID="lblAgent" runat="server" Style="font-size: 12px;" ForeColor="Red"
                                                Text=""></asp:Label>
                                        </div>
                                        <span style="font-weight: bold;">Date: </span>
                                        <asp:Label ID="lbl_fromDate" runat="server" Style="font-size: 12px;" ForeColor="Red"
                                            Text=""></asp:Label>
                                        <span style="font-size: 18px;">TO</span>
                                        <asp:Label ID="lbl_selttodate" runat="server" Style="font-size: 12px;" Text="" ForeColor="Red"></asp:Label>
                                    </div>
                                    <div align="center">
                                        Opp Bal
                                        <asp:Label ID="lblOppbal" runat="server" Style="font-size: 16px; font-weight: bold;"
                                            Text="" ForeColor="Red"></asp:Label><span style="font-size: 16px; font-weight: bold;
                                                color: Red;">.Rs</span>
                                    </div>
                                </div>
                            </div>
                            <asp:GridView ID="grdReports" runat="server" ForeColor="White" Width="100%" GridLines="Both"
                                CssClass="EU_DataTable" Font-Size="Smaller" OnRowDataBound="grdReports_RowDataBound">
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                    Font-Names="Raavi" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#ffffff" ForeColor="#333333" HorizontalAlign="Center" />
                                <AlternatingRowStyle HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnIndent" runat="server" Text="Details" CssClass="btn btn-primary" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <div align="center">
                                Closing Bal
                                <asp:Label ID="lblCloBal" runat="server" Text="" ForeColor="Red" Style="font-size: 16px;
                                    font-weight: bold;"></asp:Label><span style="font-size: 16px; font-weight: bold;
                                        color: Red;">.Rs</span>
                            </div>
                        </div>
                         <asp:Button ID="btnPrint" CssClass="btn btn-primary" Text="Print" OnClientClick="javascript:CallPrint('divPrint');"
                            runat="Server" />
                            </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                       
                        <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" Font-Size="20px"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:Button ID="Button3" Text="Export To Excel" runat="server" CssClass="btn btn-primary"
                    OnClick="btn_Export_Click"  />
                <div id="divLedgerDetails" class="pickupclass" style="text-align: center; height: 100%;
                    width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
                    background: rgba(192, 192, 192, 0.7);">
                    <div id="divAddNewRow" style="border: 5px solid #A0A0A0; position: absolute; top: 30%;
                        background-color: White; left: 10%; right: 10%; width: 80%; height: 50%; -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                        -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
                        border-radius: 10px 10px 10px 10px;">
                        <div id="divLedgerScreen">
                        </div>
                    </div>
                    <div id="divclose" style="width: 35px; top: 24.5%; right: 9.5%; position: absolute;
                        z-index: 99999; cursor: pointer;">
                        <img src="Images/Odclose.png" alt="close" onclick="LedgerCloseClick();" />
                    </div>
                </div>
            </div>
        </div>
        </div>
        </div>
        <div id="div_AgentDocumentUpload" style="display:none;">
         <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Agent & Shop Photos Details
                </h3>
            </div>
            <div class="box-body">
    <div style="display: block;">
        <table align="center">
            <tr>
                <td>
                    <label>
                        SalesOffice Name:</label><span style="color: red; font-weight: bold">*</span>
               
                    <select id="ddlSalesOffice" class="form-control" onchange="ddlSalesOfficeChange(this);">
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        Route Name:</label><span style="color: red; font-weight: bold">*</span>
               
                    <select id="ddlRouteName_photo" class="form-control" onchange="ddlRouteNameChange(this);">
                        <option>select</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        Agent Name:</label><span style="color: red; font-weight: bold">*</span>
                    <select id="ddlAgentName_photo" class="form-control">
                    </select>
                </td>
            </tr>
        </table>
    </div>
      <div class="row" align="center">
        <div class="col-xs-12 col-sm-3 text-center">
            <div class="pictureArea1">
            Agent Photo
                <img class="center-block img-circle img-thumbnail img-responsive profile-img" id="main_img"
                    alt="Agent Image" src="Images/Employeeimg.jpg" style="border-radius: 5px; width: 200px; height: 200px; border-radius: 50%;" />
                <%--<img id="prw_img" class="center-block img-circle img-thumbnail img-responsive profile-img" src="Images/Employeeimg.jpg" alt="your image" style="width: 150px; height: 150px;">--%>
                <div class="photo-edit-admin">
                    <a onclick="getFile();" class="photo-edit-icon-admin" title="Change Profile Picture"
                        data-toggle="modal"  data-target="#photoup"><i class="fa fa-pencil"></i></a>
                </div>
                <div id="yourBtn" class="img_btn" onclick="getFile();" style="margin-top: 5px; display: none;">
                    Click to Choose Image
                </div>
                <div style="height: 0px; width: 0px; overflow: hidden;">
                    <input id="file" type="file" name="files[]" onchange="readURL(this);">
                </div>
                <div>
                    <input type="button" id="btn_upload_profilepic" class="btn btn-primary" onclick="upload_profile_pic();"
                        style="margin-top: 5px;" value="Upload Profile Pic">
                </div>
            </div>
        </div>
        <!--/col-->
        <div class="col-xs-12 col-sm-3 text-left">
            <div class="pictureArea1">
            Shop Photo
                <img class="center-block img-circle img-thumbnail img-responsive profile-img" id="main_img1"
                    alt="Shop Image" src="Images/Employeeimg.jpg" style="border-radius: 5px; width: 200px; height: 200px; border-radius: 50%;" />
                <%--<img id="prw_img" class="center-block img-circle img-thumbnail img-responsive profile-img" src="Images/Employeeimg.jpg" alt="your image" style="width: 150px; height: 150px;">--%>
                <div class="photo-edit-admin">
                    <a onclick="getshopFile();" class="photo-edit-icon-admin" title="Change Profile Picture"
                        data-toggle="modal" data-target="#photoup"><i class="fa fa-pencil"></i></a>
                </div>
                <div id="yourBtn1" class="img_btn" onclick="getshopFile();" style="margin-top: 5px; display: none;">
                    Click to Choose Image
                </div>
                <div style="height: 0px; width: 0px; overflow: hidden;">
                    <input id="file1" type="file" name="files[]" onchange="readURL1(this);">
                </div>
                <div>
                    <input type="button" id="Button2" class="btn btn-primary" onclick="upload_shopprofile_pic();"
                        style="margin-top: 5px;" value="Upload Shop Profile Pic">
                </div>
            </div>
        </div>
        <!--/col-->
    </div>
    </div>
    </div>
        </div>
    </section>
</asp:Content>
