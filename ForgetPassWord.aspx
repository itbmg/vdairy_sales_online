<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForgetPassWord.aspx.cs" Inherits="ForgetPassWord" %>

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
    <asp:Label runat="server" Text="Forgot PassWord" ID="lblVyshnavi" ForeColor="White" Font-Size="Large"></asp:Label>
  </td>
  </tr>
     
  </table>
    <div  align="center">
<%--  <fieldset style="width:350px;height:150px;">
    <legend>Forgot Password</legend> --%>
    <table align="center">
      <tr>
           <td>
            <asp:Label ID="lblEmail" runat="server" Text="Email Address: "/>
           </td>
           <td>
            <asp:TextBox ID="txtEmail" runat="server" placeholder="Enter Email Address" />
   <asp:RequiredFieldValidator ID="RV1" runat="server"  ControlToValidate="txtEmail"  ErrorMessage="Enter Email" SetFocusOnError="True">
   </asp:RequiredFieldValidator>
               <asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
                   ForeColor="Red" ControlToValidate="txtEmail" runat="server" 
                   ErrorMessage="InValid EmailID" 
                   ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
           </td>
      </tr>
      <tr></tr>
     <tr>
     <td>
         <a href="Login.aspx">Back to Login Page</a>
     </td>
     <td>
      <asp:Button ID="btnSubmit" runat="server" Text="Submit" 
          onclick="btnSubmit_Click" />
    
     </td>
     </tr>
    <tr>
    <td>
  <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Font-Size="16px" Text=""/>
    
    </td>
    </tr>
    </table>
    
    
  
   
  <%-- </fieldset>--%>
    </div>
   </div>

 </section>
    </form>
</body>
</html>
