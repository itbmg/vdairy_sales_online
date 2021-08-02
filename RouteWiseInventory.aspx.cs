using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class RouteWiseInventory : System.Web.UI.Page
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
                FillRouteName();
            }
        }


    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    void FillRouteName()
    {
        try
        {
            vdm = new VehicleDBMgr();
            cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
            //cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            ddlRouteName.DataSource = dtRoutedata;
            ddlRouteName.DataTextField = "DispName";
            ddlRouteName.DataValueField = "sno";
            ddlRouteName.DataBind();
        }
        catch
        {
        }
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
             vdm = new VehicleDBMgr();
             DataTable Report = new DataTable();
             DateTime fromdate = DateTime.Now;
             string[] fromdatestrig = txtFromdate.Text.Split(' ');
             lblRoute.Text = ddlRouteName.SelectedItem.Text;
             if (fromdatestrig.Length > 1)
             {
                 if (fromdatestrig[0].Split('-').Length > 0)
                 {
                     string[] dates = fromdatestrig[0].Split('-');
                     string[] times = fromdatestrig[1].Split(':');
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
             lblDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
             cmd = new MySqlCommand("SELECT invmaster.InvName,sum(tripinvdata.Qty) as Qty,sum(tripinvdata.Remaining) as rr FROM  dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN tripinvdata ON tripdata.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (dispatch.sno = @dispatchsno) AND (tripdata.I_Date > @starttime) AND (tripdata.I_Date < @endtime) Group by invmaster.InvName order by invmaster.sno");
             cmd.Parameters.AddWithValue("@dispatchsno", ddlRouteName.SelectedValue);
             cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-2)));
             cmd.Parameters.AddWithValue("@endtime", GetHighDate(todate));
             DataTable dtInventory = vdm.SelectQuery(cmd).Tables[0];
             cmd = new MySqlCommand("SELECT sno, InvName FROM invmaster order by sno");
             DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
             Report = new DataTable();
             Report.Columns.Add("Type");
             foreach (DataRow dr in produtstbl.Rows)
             {
                 Report.Columns.Add(dr["InvName"].ToString()).DataType = typeof(Double);
             }
             DataRow newrowISS = Report.NewRow();
             foreach (DataRow dr in dtInventory.Rows)
             {
                 newrowISS["Type"] = "Issued";
                 foreach (DataRow drr in produtstbl.Rows)
                 {
                     if (dr["InvName"].ToString() == drr["InvName"].ToString())
                     {
                         newrowISS[drr["InvName"].ToString()] = dr["Qty"].ToString();
                     }
                 }
             }
             Report.Rows.Add(newrowISS);
             DataRow newrowRE = Report.NewRow();
             foreach (DataRow dr in dtInventory.Rows)
             {
                 newrowRE["Type"] = "Return";
                 foreach (DataRow drr in produtstbl.Rows)
                 {
                     if (dr["InvName"].ToString() == drr["InvName"].ToString())
                     {
                         newrowRE[drr["InvName"].ToString()] = dr["rr"].ToString();
                     }
                 }
             }
             Report.Rows.Add(newrowRE);
             grdReports.DataSource = Report;
             grdReports.DataBind();
         }
         catch
         {
         }
     }
}