<%@ Page Language="C#" AutoEventWireup="true" CodeFile="otp.aspx.cs" Inherits="otp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title></title>
    <!-- Theme style -->
    <link href="autocomplete/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="dist/css/AdminLTE.min.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/normalize/5.0.0/normalize.min.css" type="text/css"/>
    <link rel='stylesheet prefetch' href='http://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css' type="text/css"/>
    <link rel="stylesheet" href="css/OTP/style.css" type="text/css"/>
    <%--<script src="js/jquery.autocomplete.min.js" type="text/javascript"></script>--%>
    <style>
        .bcolor
        {
            background: #e0e0e0;
        }
        .fbcolor
        {
            background: "#0073b7";
        }
        .dheight
        {
            height: 590px;
        }
        .frcolor
        {
            color: Red;
            font-size: large;
            font-weight: bold;
        }
        .fgcolor
        {
            color: Green;
            font-size: large;
            font-weight: bold;
        }
        .fsize
        {
            font-size: large;
            font-weight: bold;
        }
        .toppad
        {
            padding-top: 25px;
        }
        .toppad1
        {
            padding-top: 15px;
        }
    </style>
    <script type="text/jscript">
        function save() {
            var retval = 0;
            var otpval = document.getElementById("txt_otppassword").value;
            PageMethods.save(otpval, message1);
        }

        function message1(msg) {
            var LevelType = '<%=Session["leveltype"] %>';
            if (msg == 'Transaction Completed Sccessfully...') {
                alert(msg);
                document.getElementById("txt_otppassword").value = "";

                if (LevelType == "Opperations" || LevelType == "SODispatcher" || LevelType == "Users" || LevelType == "Dispatcher") {
                    window.open("Order_Report.aspx", '_self');
                }
                if (LevelType == "PlantDispatcher") {
                    window.open("PlanDespatch.aspx", '_self');
                }
                if (LevelType == "Accountant") {
                    window.open("TripEnd.aspx", false);
                }
                if (LevelType == "Admin" || LevelType == "Manager" || LevelType == "MAdmin" || LevelType == "SalesManager" || LevelType == "Director") {
                    window.open("Delivery_Collection_Report.aspx", '_self');
                }
                if (LevelType == "AccountsOfficer") {
                    window.open("CheckDetails.aspx", '_self');
                }
                if (LevelType == "Casheir") {
                    window.open("cashform.aspx", '_self');
                }
                if (LevelType == "Security") {
                    window.open("Gatepass.aspx", '_self');
                }

            }
            else if (msg == 'Please,Check the OTP Data...') {
                alert(msg);
                document.getElementById("txt_otppassword").value = "";
                window.open('otp.aspx', '_self');
            }
            else if (msg == 'Timeout Error...') {
                window.open('login.aspx', '_self');
            }
            else if (msg == 'Please try again...') {
                alert(msg);
                window.open('login.aspx', '_self');

            }
        }

        function Resents() {
            var mno = 0;
            PageMethods.Resent(mno, message2);
        }
        function message2(msg) {
            alert(msg);
        }

        function setFocusToTextBox() {
            document.getElementById("txt_otppassword").focus();
        }


        var timeleft = 180;
        var downloadTimer = setInterval(function () {
            timeleft--;
            document.getElementById("countdowntimer").textContent = timeleft;
            if (timeleft <= 0) {
                clearInterval(downloadTimer)
            }
            if (timeleft == 0) {
                window.open('Login.aspx', '_self');
            }
        }, 1000);

    </script>
    <script type="text/javascript">
        window.onload = window.history.forward(0);  
    </script>
</head>

<body onload='setFocusToTextBox()'>
 <form id="form1" runat="server">
    <div class="dheight">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" EnablePageMethods="true" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdateProgress ID="UpdateProgress" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 1px; width: 1px; top: 0;
                    right: 0; left: 0; z-index: 9999999; background-color: Gray; opacity: 0.7;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="images/Loading.gif" AlternateText="Loading ..."
                        ToolTip="Loading ..." Style="padding: 1px; position: fixed; top: 45%; left: 45%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
   <div class="container" style="border:6px solid yellow;border-radius: 4px;padding: 6px 10px;">
   <div style=" padding-top:  10%;">
  <div class="row">
      <div class="col-half_left" >
        <div class="input-group" style="float: right;">
          <img src="images/Vyshnavilogo.png" style="width: 78%;"/>
        </div>
      </div>
      <div class="col-half_rig">
        <h3>SRI VYSHNAVI DAIRY SPECIALITIES (P) LTD</h3>
         <div >
         <h4>OTP authentication</h4>
        </div>
      </div>
    </div>
  <div class="row">
      <h4>Please enter One Time Password(OTP) Which is sent to your registered Mobile Number</h4>
      <div class="row">
      <div class="col-half">
        <div class="input-group input-group-icon">
          <p class="act">Approval Type :</p>
        </div>
      </div>
      <div class="col-half" style="padding-top:2%;">
        <div class="input-group">
         <asp:Label ID="Lbl_Approvaltype" runat="server" Font-Bold="true" Text=""></asp:Label>
        </div>
      </div>
    </div>
    <div class="row">
      <div class="col-half">
        <div class="input-group input-group-icon">
          <p class="act">Date  :</p>
        </div>
      </div>
      <div class="col-half" style="padding-top:2%;">
        <div class="input-group">
          <asp:Label ID="Lbl_Date" runat="server" Font-Bold="true" Text=""></asp:Label>
        </div>
      </div>
    </div>
      </div>
  <div class="row">
      <h5>Sucessfully sent The One Time Password(OTP) to your registered Mobile Number.</h5>
      
      <div class="col-half">
        <div class="input-group input-group-icon">
          <p class="act">One Time Password :</p>
        </div>
      </div>
      <div class="col-half">
        <div class="input-group">
          <input type="password" id="txt_otppassword" placeholder="Enter your One Time Password" class="form-control"/>
         <input type="button" id="btn_Resendotp" value="Resend OTP" onclick="Resents();"   class="btn btn-primary"/>
        </div>
      </div>
    </div>
    <br />  
     <div class="row">
      <div class="col-half-r">
        <div class="input-group input-group-icon">
        <input type="button" id="btn_Save" value="Verify" onclick="save();" class="btn btn-primary_s" />
        <input type="button" class="btn btn-primary" id="btn_close" value="Cancel" onclick="cancledeatilse();"  class="btn btn-primary_c" />
        </div>
      </div>
    </div>
     <p style="color: red;text-align: center;">This Page will Automatically Timeout After <span id="countdowntimer" style="color: Green;">180</span> seconds.</p>
     </div>
    </div>
  <script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/jquery/2.1.3/jquery.min.js'></script>
  <script type="text/javascript" src="css/OTP/index.js"></script>
            </ContentTemplate>
            <%-- <Triggers> 
            </Triggers>--%>
        </asp:UpdatePanel>
    </div>
    </form>

</body>
<%--
<body onload='setFocusToTextBox()' class="bcolor">
    <form id="form1" runat="server">
    <div class="dheight">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" EnablePageMethods="true" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdateProgress ID="UpdateProgress" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 1px; width: 1px; top: 0;
                    right: 0; left: 0; z-index: 9999999; background-color: Gray; opacity: 0.7;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="images/Loading.gif" AlternateText="Loading ..."
                        ToolTip="Loading ..." Style="padding: 1px; position: fixed; top: 45%; left: 45%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div align="center" style="padding-top: 6%;">
                    <table width="100%">
                        <tr>
                            <td width="2%" align="right">
                                <img src="Images/Vyshnavilogo.png" style="width: 128px;" />
                            </td>
                            <td width="85%">
                                <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="22px" ForeColor="#ff1493"
                                    Text="VITA MILK DAIRY PRODUCTS SPECIALITIES PVT LIMITED"></asp:Label>
                                <br />
                                <span style="font-size: 20px; font-weight: bold; color: #0252aa; padding-left: 25%;">
                                    OTP Authentication</span>
                            </td>
                            <td width="10%">
                            </td>
                        </tr>
                        <tr>
                            <td width="30%">
                            </td>
                            <td width="60%">
                                <table width="100%">
                                    <tr>
                                        <td class="fsize" colspan="3" style="padding-top: 5%;">
                                            Please enter the One Time Password (OTP) Which is sent to your registered mobile
                                            number.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="toppad" width="25%" style="font-size: large; font-weight: bold;">
                                            Approval Type :
                                        </td>
                                        <td class="toppad" width="25%">
                                            <asp:Label ID="Lbl_Approvaltype" runat="server" Font-Bold="true" Text=""></asp:Label>
                                        </td>
                                        <td width="50%">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="toppad1" width="25%" style="font-size: large; font-weight: bold;">
                                            Date :
                                        </td>
                                        <td class="toppad1" width="25%">
                                            <asp:Label ID="Lbl_Date" runat="server" Font-Bold="true" Text=""></asp:Label>
                                        </td>
                                        <td width="50%">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="50%" colspan="2" align="center" class="fgcolor" style="padding-top: 5%;">
                                            Successfully sent the One Time Password to your Registered Mobile Number.
                                        </td>
                                        <td width="50%">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" width="25%" class="toppad" style="font-size: large; font-weight: bold;">
                                            One Time Password :
                                        </td>
                                        <td width="50%" class="toppad">
                                            <input type="password" id="txt_otppassword" value="" /><input type="button" id="btn_Resendotp"
                                                value="Resend OTP" onclick="Resents();" style="background-color: #0073b7; height: 25px;
                                                width: 100px; color: white; font-weight: bold;" />
                                        </td>
                                        <td width="25%">
                                        </td>
                            </td>
                        </tr>
                        <tr>
                            <td width="25%" align="right" class="toppad1">
                                <input type="button" id="btn_Save" value="Submit" onclick="save();" style="background-color: #0073b7;
                                    height: 30px; width: 70px; color: white; font-weight: bold;" />
                            </td>
                            <td width="25%" class="toppad1">
                                <input type="button" class="btn btn-primary" id="btn_close" value="Cancel" onclick="cancledeatilse();"
                                    style="background-color: #0073b7; height: 30px; width: 70px; color: white; font-weight: bold;" />
                            </td>
                            <td width="50%">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 3%; padding-left: 10%;" colspan="3" class="frcolor">
                                <p>
                                    This page will automatically timeout after <span id="countdowntimer" style="color: Green;">
                                        180</span> seconds.
                                        </p>
                                <br />
                            </td>
                        </tr>
                    </table>
                    </td>
                    <td width="10%">
                    </td>
                    </tr> </table>
                </div>
            </ContentTemplate>
            <%-- <Triggers> 
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>--%>--%>
</html>
