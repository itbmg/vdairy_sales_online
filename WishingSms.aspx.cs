using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net;
using System.IO;

public partial class WishingSms : System.Web.UI.Page
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
            }
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

            if (MySMSBox.Text.Length > 0)
            {
                lblmsg.Text = "Enter Msg To Send";
            }
            if (MySMSBox.Text != "")
            {
                lblmsg.Text = "";
                vdm = new VehicleDBMgr();
                if (ddlType.SelectedValue == "Agent")
                {
                    //cmd = new MySqlCommand("SELECT sno, BranchName, phonenumber, emailid FROM branchdata ");
                    cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.phonenumber, branchdata.flag FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno WHERE (branchmappingtable.SuperBranch = 158) AND (branchdata.flag <> 0)");
                    DataTable dtbranchdetails = vdm.SelectQuery(cmd).Tables[0];
                    foreach (DataRow dr in dtbranchdetails.Rows)
                    {
                        string MobNo = dr["phonenumber"].ToString();
                        //string MobNo = "9959693439";
                        if (MobNo.Length == 10)
                        {
                            WebClient client = new WebClient();
                            string msg = MySMSBox.Text;
                            string baseurl2 = "http://103.16.101.52:8080/sendsms/bulksms?username=kapd-vyshnavi&password=vysavi&type=0&dlr=1&destination=" + MobNo + "&source=VYSNAVI&message=%20" + msg + "";
                            Stream data = client.OpenRead(baseurl2);
                            StreamReader reader = new StreamReader(data);
                            string ResponseID = reader.ReadToEnd();
                            data.Close();
                            reader.Close();
                        }
                    }
                    lblmsg.Text = "Message Sent Successfully";
                }
                if (ddlType.SelectedValue == "Employee")
                {
                    cmd = new MySqlCommand("SELECT Sno, EmpName, Address, Mobno FROM  empmanage  ");
                    DataTable dtbranchdetails = vdm.SelectQuery(cmd).Tables[0];
                    foreach (DataRow dr in dtbranchdetails.Rows)
                    {
                        string MobNo = dr["Mobno"].ToString();
                        //string MobNo = "9959693439";
                        if (MobNo.Length == 10)
                        {
                            WebClient client = new WebClient();
                            string msg = MySMSBox.Text;
                            string baseurl = "http://103.16.101.52:8080/sendsms/bulksms?username=kapd-vyshnavi&password=vysavi&type=0&dlr=1&destination=" + MobNo + "&source=VYSNAVI&message=%20" + msg + "";
                            Stream data = client.OpenRead(baseurl);
                            StreamReader reader = new StreamReader(data);
                            string ResponseID = reader.ReadToEnd();
                            data.Close();
                            reader.Close();
                        }
                    }
                    lblmsg.Text = "Message Sent Successfully";
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}