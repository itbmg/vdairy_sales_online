<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="icon" href="Images/Vyshnavilogo.PNG" type="image/x-icon" title=Vyshnavi Dairy />
    <title>Vyshnavi Dairy </title>
    <script type="text/javascript" src="js/jquery-1.4.4.js"></script>
    <link href="Styles/login.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .labelClass {
            font-size: 18px;
        }

        .txtClass {
            width: 280px;
            height: 30px;
            padding-left: 10px;
            border: 1px solid gray;
            border-radius: 7px 7px 7px 7px;
        }

        .txtClassforDate {
            width: 165px;
            height: 30px;
            border: 1px solid gray;
            border-radius: 7px 7px 7px 7px;
        }

        body {
            margin: 0px;
            padding: 0;
            height: 100%;
            width: 100%;
            font: 11px "Lucida Sans Unicode", "Lucida Sans", "Lucida Grande", verdana, arial, helvetica; /*color: #282828; background-image:url('Images/4.jpg');
            background-repeat:no-repeat;*/
            text-align: center;
            background: url(Images/login.png) no-repeat center fixed;
            -webkit-background-size: cover;
            -moz-background-size: cover;
            -o-background-size: cover;
            background-size: cover;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <section class="container">
            <div class="login" id="content_left" style="top: 6%; position: absolute; z-index: 99;background-color: aliceblue;"
                align="center">
                <p>
                    <asp:TextBox name="txtUserName" type="text" ID="txtUserName" runat="server" placeholder="Enter UserName" />
                </p>
                <p style="height: 0px;">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUserName"
                        ForeColor="Red" ErrorMessage="Enter UserName">
                    </asp:RequiredFieldValidator>
                </p>
                <p>
                    <asp:TextBox name="txtPassword" TextMode="password" runat="server" placeholder="Enter Password"
                        ID="txtPassword" />
                </p>
                <p style="height: 0px;">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword"
                        ForeColor="Red" ErrorMessage="Enter password">
                    </asp:RequiredFieldValidator>
                </p>
                <p class="submit">
                    <asp:Button ID="btnLogin" Text="Login" runat="server" CssClass="ContinueButton" OnClick="btnLogin_Click" />
                </p>
                <p>
                    <asp:Label ID="lbl_validation" ForeColor="Red" runat="server" Text=""></asp:Label>
                </p>
                <div>
                    <%--<a href="ForgetPassWord.aspx">Forgot PassWord</a>--%>
                </div>
                <span style="font-size: 14px;">Powered by <strong>Vyshnavi Dairy</strong></span>

                <br />
                <br />
               <%-- <div>
                    <a target="_blank" href='https://play.google.com/store/apps/details?id=io.cordova.myappef70e6'>
                        <img alt='Get it on Google Play' style="width: 210px; height: 85px;" src="Images/googleplay.png" /></a>
                </div>--%>

                <div>
                </div>
            </div>
        </section>
    </form>
</body>
</html>
