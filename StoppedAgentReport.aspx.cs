using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class StoppedAgentReport : System.Web.UI.Page
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
                lblTitle.Text = Session["TitleName"].ToString();
                FillRouteName();
            }
        }
    }
    void FillRouteName()
    {
        vdm = new VehicleDBMgr();
        if (Session["salestype"].ToString() == "Plant")
        {
            PBranch.Visible = true;
            DataTable dtBranch = new DataTable();
            dtBranch.Columns.Add("BranchName");
            dtBranch.Columns.Add("sno");
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
            cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
            cmd.Parameters.AddWithValue("@SalesType", "21");
            cmd.Parameters.AddWithValue("@SalesType1", "26");
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in dtRoutedata.Rows)
            {
                DataRow newrow = dtBranch.NewRow();
                newrow["BranchName"] = dr["BranchName"].ToString();
                newrow["sno"] = dr["sno"].ToString();
                dtBranch.Rows.Add(newrow);
            }
            cmd = new MySqlCommand("SELECT BranchName, sno FROM  branchdata WHERE (sno = @BranchID)");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in dtPlant.Rows)
            {
                DataRow newrow = dtBranch.NewRow();
                newrow["BranchName"] = dr["BranchName"].ToString();
                newrow["sno"] = dr["sno"].ToString();
                dtBranch.Rows.Add(newrow);
            }
            ddlSalesOffice.DataSource = dtBranch;
            ddlSalesOffice.DataTextField = "BranchName";
            ddlSalesOffice.DataValueField = "sno";
            ddlSalesOffice.DataBind();
        }
        else
        {
            PBranch.Visible = true;
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.sno = @BranchID) AND (branchdata.SalesType IS NOT NULL)");
            cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            ddlSalesOffice.DataSource = dtRoutedata;
            ddlSalesOffice.DataTextField = "BranchName";
            ddlSalesOffice.DataValueField = "sno";
            ddlSalesOffice.DataBind();
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
    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            lblmsg.Text = "";
            DataTable Report = new DataTable();
            DateTime fromdate = DateTime.Now;
            string[] fromdatestrig = txtFromdate.Text.Split(' ');
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
            lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
            lblToDate.Text = todate.ToString("dd/MMM/yyyy");

            Session["filename"] = "Stopped Agent Report";
            Report.Columns.Add("Sno");
            Report.Columns.Add("Route Name");
            Report.Columns.Add("SR");
            Report.Columns.Add("Agent Name");
            Report.Columns.Add("Starting Date");
            Report.Columns.Add("Closed Date");
            Report.Columns.Add("Amount").DataType = typeof(Double);
            Report.Columns.Add("Inventory").DataType = typeof(Double);
            cmd = new MySqlCommand("SELECT branchdata.BranchName, modifiedroutes.BranchID, modifiedroutesubtable.CDate, modifiedroutesubtable.EDate, modifiedroutes.RouteName, branchdata.sno, branchaccounts.Amount, SUM(inventory_monitor.Qty) as Qty, branchdata.SalesRepresentative FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN branchaccounts ON branchdata.sno = branchaccounts.BranchId INNER JOIN inventory_monitor ON branchdata.sno = inventory_monitor.BranchId WHERE (modifiedroutes.BranchID = @BranchID) AND (branchdata.flag = 0) AND (modifiedroutesubtable.CDate BETWEEN @d1 AND @d2) GROUP BY branchdata.BranchName, inventory_monitor.Qty, branchdata.SalesRepresentative ORDER BY modifiedroutes.RouteName, modifiedroutesubtable.CDate DESC"); 
            //cmd = new MySqlCommand("SELECT branchdata.sno As AgentCode, branchdata.BranchName as AgentName, branchdata.phonenumber as MobileNo, branchdata.emailid as Email FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch ) AND (branchdata.flag = @flag )");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@flag", 0);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable dtstopAgent = vdm.SelectQuery(cmd).Tables[0];
            if (dtstopAgent.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtstopAgent.Rows)
                {
                    cmd = new MySqlCommand("SELECT indents.I_date FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo WHERE (indents.Branch_id = @BranchID) AND (indents_subtable.Status = 'Delivered') order by   indents.I_date  desc limit 1");
                    cmd.Parameters.AddWithValue("@BranchID", dr["sno"].ToString());
                    DataTable dtindent = vdm.SelectQuery(cmd).Tables[0];
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["Route Name"] = dr["RouteName"].ToString();
                    newrow["SR"] = dr["SalesRepresentative"].ToString();
                    newrow["Agent Name"] = dr["BranchName"].ToString();
                    string cdate = dr["CDate"].ToString();
                    DateTime dtPlantime = Convert.ToDateTime(cdate).AddDays(1);
                    string time = dtPlantime.ToString("dd/MMM/yyyy");
                    newrow["Starting Date"] = time;
                    newrow["Amount"] = dr["Amount"].ToString();
                    newrow["Inventory"] = dr["Qty"].ToString(); 
                    if (dtindent.Rows.Count > 0)
                    {
                        string idate = dtindent.Rows[0]["I_date"].ToString();
                        DateTime dtidate = Convert.ToDateTime(idate).AddDays(1);
                        string stridate = dtidate.ToString("dd/MMM/yyyy");
                        newrow["Closed Date"] = stridate;
                    }
                    Report.Rows.Add(newrow);
                }

                DataRow newTotal = Report.NewRow();
                newTotal["Agent Name"] = "Total";
                double val = 0.0;
                foreach (DataColumn dc in Report.Columns)
                {
                    if (dc.DataType == typeof(Double))
                    {
                        val = 0.0;
                        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                        newTotal[dc.ToString()] = val;
                    }
                }
                Report.Rows.Add(newTotal);
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
                pnlHide.Visible = true;
            }
            else
            {
                lblmsg.Text="No Agent Stopped between Two Dates";
                grdReports.DataSource = Report;
                grdReports.DataBind();
                pnlHide.Visible = false;
            }
        }
        catch(Exception ex)
        {
            lblmsg.Text = ex.Message;
            pnlHide.Visible = false;
        }
    }
}