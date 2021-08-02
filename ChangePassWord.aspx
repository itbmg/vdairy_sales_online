<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassWord.aspx.cs" Inherits="ChangePassWord" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  <link rel="icon" href="Images/tlogo.PNG" type="image/x-icon" title=BMG />
    <title>Vyshnavi Dairy </title>
<script type="text/javascript" src="js/jquery-1.4.4.js"></script>
    <link href="Styles/login.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .labelClass
        {
            font-size:18px;
        }
     .txtClass
        {
           width:280px;
           height:30px; 
           padding-left:10px;
           border:1px solid gray;
           border-radius: 7px 7px 7px 7px;
        }
        .txtClassforDate
        {
           width:165px;
           height:30px; 
           border:1px solid gray;
           border-radius: 7px 7px 7px 7px;
        }
        body
        {
               margin: 0px;
	padding:0;
	height:100%;
	width:100%;
	font: 11px "Lucida Sans Unicode", "Lucida Sans", "Lucida Grande", verdana, arial, helvetica;
	/*color: #282828; background-image:url('Images/4.jpg');
            background-repeat:no-repeat;*/
	text-align: center;
background: url(Images/logo.png) no-repeat center center fixed; 
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
     <form id="form1" class="titlePane" runat="server">
    <section class="container">
    <div  id="content_left" style="top:3%;position:absolute;z-index:99;" align="center">
  <table align="center">
  <tr>
  <td>
    <asp:Label runat="server" Text="Change Password" ID="lblVyshnavi" ForeColor="White" Font-Size="Large"></asp:Label>
  </td>
  </tr>
  </table>
  <br />
    <table align="center"  style="border:1px solid gray;">
    
    <tr>
            <td>
                <asp:Label ID="lblOldPassWord" runat="server" Text="Current Password"></asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtOldPassWord" TextMode="Password" runat="server" placeholder="Enter Current Password" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtOldPassWord" ForeColor="Red" ErrorMessage="*">
                </asp:RequiredFieldValidator>
            </td>
    </tr>
      <tr>
            <td>
                <asp:Label ID="lblNewPassWord" runat="server" Text="New Password"></asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtNewPassWord" TextMode="Password" runat="server" placeholder="Enter New Password" ></asp:TextBox>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNewPassWord" ForeColor="Red" ErrorMessage="*">
                </asp:RequiredFieldValidator>
            </td>
    </tr>
      <tr>
            <td>
                <asp:Label ID="lblConformPassWord" runat="server" Text="Conform Password"></asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtConformPassWord" TextMode="Password" runat="server" placeholder="Enter Confirm Password" ></asp:TextBox>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtConformPassWord" ForeColor="Red" ErrorMessage="*">
                </asp:RequiredFieldValidator>
            </td>
    </tr>
    <tr>
    <td>
     <a href="LogOut.aspx">Back To Login Page</a>
    </td>
    <td>
        <asp:Button ID="btnSubmitt" runat="server"  Text="Submitt" 
            onclick="btnSubmitt_Click" />
    </td>
    </tr>
    <tr>
    <td>
       
    </td>
    <td>
    <asp:Label ID="lblError" ForeColor="Red" runat="server" Text=""></asp:Label>
    <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text=""></asp:Label>
    </td>
        
    </tr>
    </table>
    </div>
    </section>
    
    </form>
</body>
</html>
