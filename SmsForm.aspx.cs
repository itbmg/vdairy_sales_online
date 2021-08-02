using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Text;
using ClosedXML.Excel;
using System.Configuration;
using System.Net;

public partial class SmsForm : System.Web.UI.Page
{
    MySqlCommand cmd;
    string BranchID = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["branch"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            BranchID = Session["branch"].ToString();
        }
        //vdm = new VehicleDBMgr();
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                FillAgentName();

            }
        }
    }
    void FillAgentName()
    {
        try
        {
            vdm = new VehicleDBMgr();
            cmd = new MySqlCommand(" SELECT branchroutes.RouteName, branchroutes.Sno, branchroutes.BranchID FROM branchroutes INNER JOIN branchdata ON branchroutes.BranchID = branchdata.sno WHERE (branchroutes.BranchID = @brnchid) OR (branchdata.SalesOfficeID = @SOID)");
            //cmd = new MySqlCommand("SELECT RouteName, Sno, BranchID FROM branchroutes WHERE (BranchID = @brnchid)");
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            cmd.Parameters.AddWithValue("@brnchid", BranchID);
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            ddlRouteName.DataSource = dtRoutedata;
            ddlRouteName.DataTextField = "RouteName";
            ddlRouteName.DataValueField = "Sno";
            ddlRouteName.DataBind();
            ddlRouteName.Items.Insert(0, new ListItem("Select Route Name", "0"));

        }
        catch (Exception ex)
        {
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {

        GetReport();
    }
    void GetReport()
    {
        try
        {
            
            
            if (ddlRouteName.SelectedValue == "0")
            {
                lblmsg.Text = "Please Select Route Name";

            }
            if (ddlAgentName.SelectedValue == "0")
            {
                lblmsg.Text = "Please Select Agent Name";

            }
            if (MySMSBox.Text.Length > 0)
            {
                lblmsg.Text = "Enter Msg To Send";
            }
            if (MySMSBox.Text != "")
            {
                lblmsg.Text = "";
                vdm = new VehicleDBMgr();
                cmd = new MySqlCommand("SELECT sno, BranchName, phonenumber, emailid FROM branchdata WHERE (sno = @branchid)");
                cmd.Parameters.AddWithValue("@branchid", ddlAgentName.SelectedValue);
                DataTable dtbranchdetails = vdm.SelectQuery(cmd).Tables[0];
                string MobNo = dtbranchdetails.Rows[0]["phonenumber"].ToString();
                MobNo = "9959693439";
                if (MobNo.Length == 10)
                {
                    string msg = MySMSBox.Text;
                    WebClient client = new WebClient();
                    //string strUrl = " http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + no + "&msg=" + message1 + "&type=1 ";
                    //string baseurl = "http://103.225.76.43/blank/sms/user/urlsmstemp.php?username=vyshnavidairy&pass=vyshnavi@123&senderid=VYSHRM&dest_mobileno=" + MobNo + "&message=%20" + msg + "&response=Y";

                    //string baseurl = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + MobNo + "&message=" + msg + "&sender=VYSNVI&type=1&route=2";
                    //string baseurl = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VSALES&to=" + MobNo + "&msg=%20" + ProductName + "TotalQty =" + TotalQty + "(" + diffproduct + ")" + "&type=1";

                    string baseurl = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + MobNo + "&msg=" + msg + "&type=1";
                    Stream data = client.OpenRead(baseurl);
                    StreamReader reader = new StreamReader(data);
                    string ResponseID = reader.ReadToEnd();
                    data.Close();
                    reader.Close();
                    lblmsg.Text = "Message Sent Successfully";
                }
                else
                {
                    lblmsg.Text = "Please Update Mobile Number To This Agent";
                }
            }
        }
        catch(Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void ddlRouteName_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand(" SELECT branchdata.sno, branchdata.BranchName, branchroutes.RouteName FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (branchroutes.Sno = @routesno) ORDER BY branchdata.BranchName");
        //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName, branchroutes.RouteName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchroutesubtable ON branchmappingtable.SubBranch = branchroutesubtable.BranchID INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno WHERE (branchmappingtable.SuperBranch = @SuperBranch) AND (branchroutes.Sno = @routesno) ORDER BY branchdata.BranchName");
        cmd.Parameters.AddWithValue("@SuperBranch", BranchID);
        cmd.Parameters.AddWithValue("@routesno", ddlRouteName.SelectedValue);
        DataTable dtbranchdata = vdm.SelectQuery(cmd).Tables[0];
        ddlAgentName.DataSource = dtbranchdata;
        ddlAgentName.DataTextField = "BranchName";
        ddlAgentName.DataValueField = "sno";
        ddlAgentName.DataBind();
        ddlAgentName.Items.Insert(0, new ListItem("Select Agent Name", "0"));
    }
}