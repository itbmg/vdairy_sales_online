using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using MySql.Data.MySqlClient;

public partial class ForgetPassWord : System.Web.UI.Page
{
    MySqlCommand command;
    MailMessage mail = new MailMessage();
    static VehicleDBMgr vdm;
    //static DataDownloader ddwnldr;
    protected void Page_Load(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        //ddwnldr = new DataDownloader();
        //vdm.InitializeDB();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = "";
            command = new MySqlCommand("SELECT UserName,Password,Email FROM empmanage WHERE Email = @Email");
            command.CommandType = CommandType.Text;
            MySqlParameter email = new MySqlParameter("@Email", MySqlDbType.VarChar, 50);
            email.Value = txtEmail.Text.Trim().ToString();
            command.Parameters.Add(email);
            DataTable dt = vdm.SelectQuery(command).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string emailid = "";
                emailid = txtEmail.Text.ToString();
                string Email = dt.Rows[0]["Email"].ToString();
                if (Email == emailid)
                {
                    MailMessage Msg = new MailMessage();
                    MailAddress fromMail = new MailAddress("vyshnavidairyinfo@gmail.com");
                    // Sender e-mail address.
                    Msg.From = fromMail;
                    const string password = "vyshnavi123";
                    // Recipient e-mail address.
                    if (emailid == "")
                    {
                        emailid = "vyshnavidairyinfo@gmail.com";
                    }
                    Msg.To.Add(new MailAddress(emailid));
                    // Subject of e-mail
                    Msg.Subject = "Forgot Password Information";
                    Msg.Body += "Welcome to Vyshnavi Dairy<br/><br/>";
                    Msg.Body += "Username: " + dt.Rows[0]["UserName"] + "<br><br>Password: " + dt.Rows[0]["Password"] + "<br><br>";
                    Msg.IsBodyHtml = true;
                    string sSmtpServer = "";
                    sSmtpServer = "smtp.gmail.com";
                    int portNumber = 587;
                    SmtpClient a = new SmtpClient(sSmtpServer, portNumber);
                    a.Host = sSmtpServer;
                    a.Credentials = new NetworkCredential("vyshnavidairyinfo@gmail.com", password);
                    a.EnableSsl = true;
                    a.Send(Msg);
                    lblMessage.Text = "Password is sent to you email id";
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    lblMessage.Text = "Please check your EmailID";
                }
            }
            else
            {
                lblMessage.Text = "Email Address Not Registered";
            }
        }
        catch
        {
            Response.Redirect("Login.aspx");
        }
    }
}