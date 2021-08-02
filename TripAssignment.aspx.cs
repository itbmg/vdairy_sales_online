using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Plant_TripAssignment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string username = Session["UserName"].ToString();
            if (Session["UserName"] == null)
            {
                Response.Redirect("Login.aspx");
            }
        }
        catch (Exception ex)
        {
            if (ex.Message == "Object reference not set to an instance of an object.")
            {
                MessageBox.Show("Session Expired", this);
                Response.Redirect("Login.aspx");
            }
        }
    }
}