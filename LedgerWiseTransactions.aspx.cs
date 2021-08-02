using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class LedgerWiseTransactions : System.Web.UI.Page
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
                FillSalesOffice();
                lblTitle.Text = Session["TitleName"].ToString();
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            }
        }
    }
    void FillSalesOffice()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Plant")
            {
                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("BranchName");
                dtBranch.Columns.Add("sno");
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType)  ");
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
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                cmd = new MySqlCommand("SELECT BranchName, sno FROM branchdata WHERE (sno = @BranchID)");
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch
        {
        }
    }
    protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand("SELECT accountheads.HeadName, accountheads.Sno FROM subpayable INNER JOIN accountheads ON subpayable.HeadSno = accountheads.Sno INNER JOIN cashpayables ON subpayable.RefNo = cashpayables.Sno WHERE (cashpayables.BranchID = @BranchID) AND (cashpayables.VoucherType = 'Debit') GROUP BY accountheads.HeadName ORDER BY accountheads.HeadName");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        DataTable dtHead = vdm.SelectQuery(cmd).Tables[0];
        ddlHeadOfaccounts.DataSource = dtHead;
        ddlHeadOfaccounts.DataTextField = "HeadName";
        ddlHeadOfaccounts.DataValueField = "Sno";
        ddlHeadOfaccounts.DataBind();
        ddlHeadOfaccounts.Items.Insert(0, new ListItem("Select", "0"));
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
            DateTime fromdate = DateTime.Now;
            DataTable Report = new DataTable();
            lblmsg.Text = "";
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
            Session["filename"] = "Statement of Account";
            lblLedger.Text = ddlHeadOfaccounts.SelectedItem.Text;
            lbl_fromDate.Text = txtFromdate.Text;
            lbl_selttodate.Text = txtTodate.Text;
            pnlHide.Visible = true;
            cmd = new MySqlCommand("SELECT DATE_FORMAT(cashpayables.DOE, '%d %b %y') AS EntryDate, cashpayables.VocherID,  cashpayables.Amount FROM accountheads INNER JOIN subpayable ON accountheads.Sno = subpayable.HeadSno INNER JOIN cashpayables ON subpayable.RefNo = cashpayables.Sno WHERE (cashpayables.BranchID = @BranchID)  AND (subpayable.HeadSno = @HeadSno) and (cashpayables.Status=@Status) AND (cashpayables.DOE between @d1 and @d2)   ORDER BY cashpayables.DOE");
            cmd.Parameters.AddWithValue("@Status", 'P');
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@HeadSno", ddlHeadOfaccounts.SelectedValue);
            DataTable dtCredit = vdm.SelectQuery(cmd).Tables[0];
            Report.Columns.Add("Sno");
            Report.Columns.Add("Date");
            Report.Columns.Add("VoucherID");
            Report.Columns.Add("Amount").DataType = typeof(Double);
            if (dtCredit.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtCredit.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["Date"] = dr["EntryDate"].ToString();
                    newrow["VoucherID"] = dr["VocherID"].ToString();
                    double amount = 0;
                    double.TryParse(dr["Amount"].ToString(),out amount);

                    newrow["Amount"] = amount;
                    Report.Rows.Add(newrow);
                }
                DataRow newvartical = Report.NewRow();
                newvartical["VoucherID"] = "Total";
                double val = 0.0;
                foreach (DataColumn dc in Report.Columns)
                {
                    if (dc.DataType == typeof(Double))
                    {
                        val = 0.0;
                        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                        newvartical[dc.ToString()] = val;
                    }
                }
                Report.Rows.Add(newvartical);
                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
            else
            {
                lblmsg.Text = "No data were found";

            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}