using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string username = Session["UserName"].ToString();

            if (Session["UserName"]==null)
            {
                //MessageBox.Show("Session Expired", this);
                MessageBox.Show("Session Expired", this);

                Response.Redirect("Login.aspx");
            }
        }
        catch (Exception ex)
        {

            if (ex.Message == "Object reference not set to an instance of an object.")
            {
               // Response.Write("Session Expired");
               // ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "var r = confirm('Your Session Has Expired'); if (r == true) var str= '../Login.aspx'; location.href = str ;", true);
                MessageBox.Show("Session Expired", this);
                Response.Redirect("Login.aspx");
            }
        }
    }
}