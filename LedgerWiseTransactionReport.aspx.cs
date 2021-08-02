using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class LedgerWiseTransactionReport : System.Web.UI.Page
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
                lblTitle.Text = Session["TitleName"].ToString();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txttodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
            FillEmployees();

        }
        catch
        {
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    void FillEmployees()
    {
        try
        {
            vdm = new VehicleDBMgr();
            
                cmd = new MySqlCommand("SELECT Sno, BranchId, HeadName, LimitAmount, AccountType, AgentID, EmpID FROM accountheads WHERE (BranchId = @SuperBranch) OR (BranchId IS NULL) ORDER BY BranchId");
                cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];


                ddlAccountHead.DataSource = dtRoutedata;
                ddlAccountHead.DataTextField = "HeadName";
                ddlAccountHead.DataValueField = "Sno";
                ddlAccountHead.DataBind();
            
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
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            DataTable Report = new DataTable();
            DateTime dtDate = DateTime.Now;
            string Time = dtDate.ToString("dd/MMM/yyyy");
            lblselected.Text = ddlSalesOffice.SelectedItem.Text;

            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            string[] fromdatestrig = txtdate.Text.Split(' ');
            string[] todatestrig = txttodate.Text.Split(' ');
            if (fromdatestrig.Length > 1)
            {
                if (fromdatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = fromdatestrig[0].Split('-');
                    string[] times = fromdatestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            if (todatestrig.Length > 1)
            {
                if (todatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = todatestrig[0].Split('-');
                    string[] times = todatestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            lbl_selfromdate.Text = txtdate.Text;
            lbl_selttodate.Text = txttodate.Text;

            Report = new DataTable();
            Report.Columns.Add("DATE");
            Report.Columns.Add("ACCOUNT HEAD");
            Report.Columns.Add("AMOUNT DEBITED");
            Report.Columns.Add("AMOUNT CREDITED");
            Report.Columns.Add("REF VOURCHER ID");
            Report.Columns.Add("VOURCHER ID");
            Report.Columns.Add("ON NAME OF");

            cmd = new MySqlCommand("SELECT subpayable.RefNo, subpayable.HeadDesc, subpayable.Amount, subpayable.HeadSno, cashpayables.CashTo, DATE_FORMAT(cashpayables.DOE, '%d %b %y') AS Entrydate, cashpayables.VocherID, cashpayables.onNameof, cashpayables.ApprovalRemarks, cashpayables.VoucherType FROM subpayable INNER JOIN cashpayables ON subpayable.RefNo = cashpayables.Sno WHERE (subpayable.HeadSno = @HeadSno) AND (cashpayables.BranchID = @BranchID) AND (cashpayables.DOE BETWEEN @d1 AND @d2) ORDER BY cashpayables.DOE");
            cmd.Parameters.AddWithValue("@HeadSno",ddlAccountHead.SelectedValue);
            cmd.Parameters.AddWithValue("@BranchID",ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
            DataTable dtaccountdetails = vdm.SelectQuery(cmd).Tables[0];
            double totamount_debited = 0;
            double totamount_credited = 0;
            foreach (DataRow dr in dtaccountdetails.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["DATE"] = dr["Entrydate"].ToString();
                newrow["ACCOUNT HEAD"] = ddlAccountHead.SelectedItem.Text;
                if (dr["VoucherType"].ToString() == "Credit")
                {
                    double credit = 0;
                    double.TryParse(dr["Amount"].ToString(), out credit);
                    newrow["AMOUNT CREDITED"] = dr["Amount"].ToString();
                    totamount_credited += credit;
                }
                else
                {
                    double debit = 0;
                    double.TryParse(dr["Amount"].ToString(), out debit);

                    newrow["AMOUNT DEBITED"] = dr["Amount"].ToString();
                    totamount_debited += debit;

                }
                newrow["REF VOURCHER ID"] = dr["RefNo"].ToString();
                newrow["VOURCHER ID"] = dr["VocherID"].ToString();
                newrow["ON NAME OF"] = dr["onNameof"].ToString();
                Report.Rows.Add(newrow);

            }
            DataRow drtotal = Report.NewRow();
            drtotal["ACCOUNT HEAD"] = "TOTAL";
            drtotal["AMOUNT DEBITED"] = totamount_debited;
            drtotal["AMOUNT CREDITED"] = totamount_credited;
            Report.Rows.Add(drtotal);

            grdReports.DataSource = Report;
            grdReports.DataBind();
        }
        catch
        {

        }
    }
    private string GetSpace(string p)
    {
        int i = 0;
        for (; i < p.Length; i++)
        {
            if (char.IsNumber(p[i]))
            {
                break;
            }
        }
        return p.Substring(0, i) + " " + p.Substring(i, p.Length - i);
    }
}