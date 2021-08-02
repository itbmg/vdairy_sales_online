<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Home" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vyshnavi Dairy</title>
     <link rel="icon" href="Images/Vyshnavilogo.PNG" type="image/x-icon" title=BMG />
      <meta name="google-site-verification" content="XYRQcdP4pSdqcbJ-xFwpcIE96OkKQOPwGOynOPhFwfQ" />
    <style type="text/css">
        .imageclass
        {
            height: 10%;
            width: 70%;
            border: 1px solid #AAA6A6;
            border-radius: 5px 5px 5px 5px;
        }
        .imageclass:hover
        {
            height: 10%;
            width: 70%;
            border: 1px solid #3A3636;
            cursor:pointer;
            border-radius: 5px 5px 5px 5px;
        }
    </style>
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
     <script type="text/javascript">
         $(function () {
             window.history.forward(1);

         });
    </script>
    <script type="text/javascript">
        function accessControlclick() {
            window.location.href = "Distributors/Login.aspx";
        }
    </script>
    <script>
        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
  m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', 'https://www.google-analytics.com/analytics.js', 'ga');

        ga('create', 'UA-90586232-1', 'auto');
        ga('send', 'pageview');

</script>
</head>
<body>
<!-- Google Tag Manager (noscript) -->
<noscript><iframe src="https://www.googletagmanager.com/ns.html?id=GTM-WJP55XK"
height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>
<!-- End Google Tag Manager (noscript) -->

    <form>
    <br />
    <br />
    <table style="width: 100%; padding-left: 10%; padding-right: 10%;">
        <tr>
            <td style="width: 25%;">
                <img src="DefaultImages/ACT.png" alt="Distributors" title="Distributors" onclick="accessControlclick();"
                    class="imageclass" />
            </td>
            <td style="width: 25%;">
                <img src="DefaultImages/APP.png" class="imageclass" />
            </td>
            <td style="width: 25%;">
                <img src="DefaultImages/FA.png" class="imageclass" />
            </td>
            <td style="width: 25%;">
                <img src="DefaultImages/HR.png" class="imageclass" />  <br />
            <br />
            </td>
          
        </tr>
        <tr>
            <td>
                <img src="DefaultImages/MB.png" class="imageclass" />
            </td>
            <td>
                <img src="DefaultImages/BGD.png" class="imageclass" />
            </td>
            <td>
                <img src="DefaultImages/SOC.png" class="imageclass" />
            </td>
            <td>
                <img src="DefaultImages/PS.png" class="imageclass" />  <br />
            <br />
            </td>
            
        </tr>
        <tr>
            <td>
                <img src="DefaultImages/MKT.png" class="imageclass" />
            </td>
            <td>
                <img src="DefaultImages/PQ.png" class="imageclass" />
            </td>
            <td>
                <img src="DefaultImages/VPT.png" class="imageclass" />
            </td>
            <td>
                <img src="DefaultImages/12.png" class="imageclass" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
