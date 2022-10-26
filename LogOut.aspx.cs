using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Web.Services;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.IO;
public partial class LogOut : System.Web.UI.Page
{
    MySqlCommand cmd;
    VehicleDBMgr vdm = new VehicleDBMgr();
    SqlCommand a_cmd;
    string ipaddress;
    
    protected void Page_Load(object sender, EventArgs e)
    {

        DateTime Currentdate = VehicleDBMgr.GetTime(vdm.conn);
        Session["UserName"] = null;
        Session["userdata_sno"] = null;
        Session["Owner"] = null;
        Session["LevelType"] = null;
        Session["Plant"] = null;
        Session["SalesOffice"] = null;
        Session["Distributors"] = null;
        Session["getcategorynames"] = null;
        Session["getsalesofficenames"] = null;
        Session["getbranchcategorynames"] = null;
        Session["getproductsnames"] = null;
        Session["getsubregionsnames"] = null;
        Session["getsalesofficenames"] = null;
        Session["getbranchnames"] = null;
        Session["getrates_categorynames"] = null;
        Session["getrates_productsnames"]=null;
        Session["getrates_subregion_subregionsnames"]=null;
        Session["getrates_subregions_categorynames"]=null;
        Session["getrates_subregions_productsnames"]=null; 
        Session["getrates_routes_subregionsnames"]=null;
        Session["getrates_routes_categorynames"]=null;
        Session["getrates_routes_productsnames"] =null;
        Session["getrates_routes_routename"] = null;
        Session["Branches"] = null;
        Session["PlantSno"] = null;
        Session["SalesoffSno"] = null;
        Session["branchSno"] = null;

        if (Session["EmpSno"] != null)
        {
            string sno = Session["EmpSno"].ToString();
            cmd = new MySqlCommand("Select max(sno) as transno from logininfo where UserId=@userid");
            cmd.Parameters.AddWithValue("@userid", sno);
            DataTable dttime = vdm.SelectQuery(cmd).Tables[0];
            if (dttime.Rows.Count > 0)
            {
                string transno = dttime.Rows[0]["transno"].ToString();
                cmd = new MySqlCommand("UPDATE logininfo set logouttime=@logouttime,status=@status where sno=@sno");
                cmd.Parameters.AddWithValue("@logouttime", Currentdate);
                cmd.Parameters.AddWithValue("@status", "0");
                cmd.Parameters.AddWithValue("@sno", transno);
                vdm.Update(cmd);
            }
        }
        Session.Abandon();
        Response.Redirect("login.aspx");
    }
}