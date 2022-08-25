using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Agent_Balance_Tracking : System.Web.UI.Page
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
                FillSalesOffice();
                //txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                // txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    void FillSalesOffice()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Plant")
            {
                PBranch.Visible = true;
                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("BranchName");
                dtBranch.Columns.Add("sno");
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchdata.flag=1) AND (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType)  ");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                cmd.Parameters.AddWithValue("@SalesType", "23");
                DataTable dtNewPlant = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtNewPlant.Rows)
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
    DateTime Prevdate = DateTime.Now;

    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            lblmessage.Text = "";
            Session["RouteName"] = ddlSalesOffice.SelectedItem.Text;
            lblRouteName.Text = ddlSalesOffice.SelectedItem.Text;
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            lblDate.Text = fromdate.ToString("dd/MMM/yyyy");
            Session["filename"] = "AGENT WISE DUE REPORT";
            string BranchID = ddlSalesOffice.SelectedValue;
            cmd = new MySqlCommand("SELECT  modifiedroutes.RouteName, modifiedroutes.sno as routeid,  modifiedroutesubtable.BranchID,       branchdata.BranchName,branchaccounts.amount  FROM    modifiedroutes        INNER JOIN    modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo        INNER JOIN    branchdata ON modifiedroutesubtable.BranchID = branchdata.sno  inner join  branchaccounts on branchdata.sno=branchaccounts.branchid  WHERE      (modifiedroutes.BranchID = @BranchID)         AND(modifiedroutesubtable.EDate IS NULL)         AND(modifiedroutesubtable.CDate <= @starttime) AND (branchdata.flag=@flag) OR (modifiedroutes.BranchID = @BranchID)AND(modifiedroutesubtable.EDate > @starttime)         AND(modifiedroutesubtable.CDate <= @starttime) AND (branchdata.flag=@flag) GROUP BY branchdata.BranchName  ORDER BY modifiedroutes.RouteName");
            cmd.Parameters.AddWithValue("@branchid", BranchID);
            cmd.Parameters.AddWithValue("@flag", "1");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate).AddDays(-1));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];

            //cmd = new MySqlCommand("SELECT * FROM agent_bal_trans WHERE inddate between @d1 and @d2");
            //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
            //cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate).AddDays(-1));
            //DataTable dtagenttrans = vdm.SelectQuery(cmd).Tables[0];

            //cmd = new MySqlCommand("SELECT sum(AmountPaid) as AmountPaid,Branchid FROM collections WHERE PaidDate between @d1 and @d2 AND PaymentType <> 'Cash' group by Branchid");
            //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            //cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            //DataTable dtcollections = vdm.SelectQuery(cmd).Tables[0];

            DataTable dtrouteamount = new DataTable();
            DataTable dtsalescollection = new DataTable();
           DataTable Report = new DataTable();
            Report.Columns.Add("Sno");
            Report.Columns.Add("Route Name");
            Report.Columns.Add("Agent Code");
            Report.Columns.Add("Agent Name");
            Report.Columns.Add("BranchClosing");
            Report.Columns.Add("Transaction Closing").DataType = typeof(Double);
            Report.Columns.Add("Difference").DataType = typeof(Double);
            int Totalcount = 1;
            string RouteName = "";
            int i = 1;
            //DataView view = new DataView(dtble);
            //DataTable distincttable = view.ToTable(true, "BranchName", "BranchID", "RouteName", "routeid");
            
            foreach (DataRow branch in dtble.Rows)
            {
                double branchamount = 0;
                double transactionamout = 0;
                DataRow newrow = Report.NewRow();
                newrow["Route Name"] = branch["RouteName"].ToString();
                newrow["Agent Code"] = branch["BranchID"].ToString();
                newrow["Agent Name"] = branch["BranchName"].ToString();
                newrow["BranchClosing"] = branch["Amount"].ToString();
                double.TryParse(branch["Amount"].ToString(), out branchamount);

                cmd = new MySqlCommand("select max(sno)  as agentsno from  agent_bal_trans WHERE agentid=@agentid");
                cmd.Parameters.AddWithValue("@agentid", branch["BranchID"].ToString());
                DataTable dtmaxsno = vdm.SelectQuery(cmd).Tables[0];
                if (dtmaxsno.Rows.Count > 0)
                {
                    cmd = new MySqlCommand("select * from  agent_bal_trans WHERE sno=@sno");
                    cmd.Parameters.AddWithValue("@sno", dtmaxsno.Rows[0]["agentsno"].ToString());
                }
                DataTable dtagentbalance = vdm.SelectQuery(cmd).Tables[0];

                if (dtagentbalance.Rows.Count > 0)
                {
                    double.TryParse(dtagentbalance.Rows[0]["clo_balance"].ToString(), out transactionamout);
                }
                double diff = branchamount - transactionamout;
                newrow["BranchClosing"] = branchamount;
                newrow["Transaction Closing"] = transactionamout;
                newrow["Difference"] = Math.Round(diff);
                Report.Rows.Add(newrow);
            }
            grdReports.DataSource = Report;
            grdReports.DataBind();

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            lblmessage.Text = ex.Message;
        }
    }
}