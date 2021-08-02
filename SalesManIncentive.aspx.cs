using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class SalesManIncentive : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["salestype"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                lblTitle.Text = Session["TitleName"].ToString();
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                FillDispName();
            }
        }
    }
    void FillDispName()
    {
        try
        {
            string salestype = Session["salestype"].ToString();
            vdm = new VehicleDBMgr();
            cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID) and (dispatch.flag=@flag)) OR ((branchdata_1.SalesOfficeID = @SOID) and (dispatch.flag=@flag))");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@flag", "1");
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            ddlRouteName.DataSource = dtRoutedata;
            ddlRouteName.DataTextField = "DispName";
            ddlRouteName.DataValueField = "sno";
            ddlRouteName.DataBind();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    private DateTime GetLowDate(DateTime dt)
    {
        double Hour, Min, Sec;
        DateTime DT = DateTime.Now;
        DT = dt;
        Hour = -dt.Hour;
        Min = -dt.Minute;
        Sec = -dt.Second;
        DT = DT.AddHours(Hour);
        DT = DT.AddMinutes(Min);
        DT = DT.AddSeconds(Sec);
        return DT;
    }
    private DateTime GetHighDate(DateTime dt)
    {
        double Hour, Min, Sec;
        DateTime DT = DateTime.Now;
        Hour = 23 - dt.Hour;
        Min = 59 - dt.Minute;
        Sec = 59 - dt.Second;
        DT = dt;
        DT = DT.AddHours(Hour);
        DT = DT.AddMinutes(Min);
        DT = DT.AddSeconds(Sec);
        return DT;
    }
    DateTime fromdate = DateTime.Now;
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            PanelHide.Visible = true;
            Session["RouteName"] = "Sales Man Incentive";
            Session["filename"] = ddlRouteName.SelectedItem.Text + "Sales Man Incentive";
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            lblRoutName.Text = ddlRouteName.SelectedItem.Text;
        
            vdm = new VehicleDBMgr();
            DataTable Report = new DataTable();
            string[] datestrig = txtFromdate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            DateTime todate = DateTime.Now;
            string[] todatestrig = txtTodate.Text.Split(' ');
            if (todatestrig.Length > 1)
            {
                if (todatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = todatestrig[0].Split('-');
                    string[] times = todatestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            lbl_selfromdate.Text = fromdate.ToString("dd MMM yy");
            lbl_selttodate.Text = todate.ToString("dd MMM yy");
            Report = new DataTable();
            Report.Columns.Add("SNo");
            Report.Columns.Add("Date");
            Report.Columns.Add("Incentive Amount");
            Report.Columns.Add("Fine Amount");
            TimeSpan dateSpan = todate.Subtract(fromdate);
            int NoOfdays = dateSpan.Days;
            NoOfdays = NoOfdays + 1;
            int i = 1;
            for (int j = 0; j < NoOfdays; j++)
            {
                DataRow newrow = Report.NewRow();
                newrow["SNo"] = i++.ToString();
                string dtcount = fromdate.AddDays(j).ToString();
                DateTime dtDOE = Convert.ToDateTime(dtcount);
                string dtdate1 = dtDOE.AddDays(-1).ToString();
                DateTime dtDOE1 = Convert.ToDateTime(dtdate1).AddDays(1);
                string ChangedTime1 = dtDOE1.ToString("dd/MMM/yy");
                string ChangedTime2 = dtDOE.AddDays(-1).ToString("dd MMM yy");
                newrow["Date"] = ChangedTime1;
                Report.Rows.Add(newrow);
            }
            grdReports.DataSource = Report;
            grdReports.DataBind();
            Session["xportdata"] = Report;
        }
        catch(Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}