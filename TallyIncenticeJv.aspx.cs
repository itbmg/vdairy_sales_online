using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;
using System.IO;

public partial class TallyIncenticeJv : System.Web.UI.Page
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
        //UserName = Session["field1"].ToString();
        //vdm = new VehicleDBMgr();
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            Session["RouteName"] = ddlSalesOffice.SelectedItem.Text;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            string[] dateFromstrig = txtFromdate.Text.Split(' ');
            if (dateFromstrig.Length > 1)
            {
                if (dateFromstrig[0].Split('-').Length > 0)
                {
                    string[] dates = dateFromstrig[0].Split('-');
                    string[] times = dateFromstrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            string[] datetostrig = txtTodate.Text.Split(' ');
            if (datetostrig.Length > 1)
            {
                if (datetostrig[0].Split('-').Length > 0)
                {
                    string[] dates = datetostrig[0].Split('-');
                    string[] times = datetostrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            lbl_selfromdate.Text = fromdate.ToString("dd/MM/yyyy");
            lblRoutName.Text = ddlSalesOffice.SelectedItem.Text;
            string DCNO = "";
            Report.Columns.Add("JV No.");
            Report.Columns.Add("JV Date");
            Report.Columns.Add("Ledger Name");
            Report.Columns.Add("Amount");
            Report.Columns.Add("Narration");
            cmd = new MySqlCommand("SELECT sno, BranchName, incentivename,Branchcode FROM branchdata WHERE (sno = @BranchID)");
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            }
            DataTable dtincetivename = vdm.SelectQuery(cmd).Tables[0];
            string Jvname = "";
            if (dtincetivename.Rows.Count > 0)
            {
                Jvname = dtincetivename.Rows[0]["Branchcode"].ToString() + "JV";
            }
            DCNO = Jvname + DCNO;
            DataTable dtble = new DataTable();
            if (ddltype.SelectedValue == "Incentives")
            {
                Session["xporttype"] = "TallyIncentive";
                Session["filename"] = ddlSalesOffice.SelectedItem.Text + " Tally Incentive" + fromdate.ToString("dd/MM/yyyy");
                cmd = new MySqlCommand("SELECT branchdata.tbranchname, branchmappingtable.SuperBranch, incentivetransactions.TotalDiscount, incentivetransactions.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN incentivetransactions ON branchdata.sno = incentivetransactions.BranchId INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (incentivetransactions.EntryDate BETWEEN @d1 AND @d2) OR (branchdata_1.SalesOfficeID = @SOID) AND (incentivetransactions.EntryDate BETWEEN @d1 AND @d2)");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                dtble = vdm.SelectQuery(cmd).Tables[0];
                double totamount = 0;
                fromdate = fromdate.AddDays(-1);
                string frmdate = fromdate.ToString("dd-MM-yyyy");
                string[] strjv = frmdate.Split('-');
                foreach (DataRow branch in dtble.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["JV No."] = DCNO + strjv[1];
                    newrow["JV Date"] = fromdate.ToString("dd-MMM-yyyy");
                    newrow["Ledger Name"] = branch["tbranchname"].ToString();
                    double amount = 0;
                    double.TryParse(branch["TotalDiscount"].ToString(), out amount);
                    totamount += amount;
                    newrow["Amount"] = "-" + amount;
                    newrow["Narration"] = "Being the incentive for the month of " + fromdate.ToString("MMM-yyyy") + " Total Amount " + branch["TotalDiscount"].ToString() + ",Emp Name  " + Session["EmpName"].ToString();
                    Report.Rows.Add(newrow);
                }
                DataRow newrow1 = Report.NewRow();
                newrow1["JV No."] = DCNO + strjv[1];
                newrow1["JV Date"] = fromdate.ToString("dd-MMM-yyyy");
                newrow1["Ledger Name"] = "Sales Discount-" + dtincetivename.Rows[0]["incentivename"].ToString();
                newrow1["Amount"] = totamount;
                newrow1["Narration"] = "Being the amount credited to  " + ddlSalesOffice.SelectedItem.Text + " for the month of " + fromdate.ToString("MMM-yyyy") + " Total Amount " + totamount + ",Emp Name  " + Session["EmpName"].ToString();
                Report.Rows.Add(newrow1);
            }
            if (ddltype.SelectedValue == "Journel Voucher")
            {
                Session["xporttype"] = "TallyJournelvoucher";
                Session["filename"] = ddlSalesOffice.SelectedItem.Text + " Tally Journel Voucher" + fromdate.ToString("dd/MM/yyyy");
                 //cmd = new MySqlCommand("SELECT branchdata.tbranchname,collections.sno, collections.remarks, collections.AmountPaid as TotalDiscount FROM dispatch INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN collections ON branchdata.sno = collections.Branchid WHERE (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) AND (dispatch.Branch_Id = @BranchID) AND (collections.TransactionType IS NULL) AND (collections.PaymentType = @pt) GROUP BY collections.PaidDate");

               cmd = new MySqlCommand("SELECT branchdata.tbranchname, collections.Sno, collections.Remarks, collections.AmountPaid AS TotalDiscount, accountheads.HeadName FROM dispatch INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN collections ON branchdata.sno = collections.Branchid INNER JOIN accountheads ON collections.HeadSno = accountheads.Sno WHERE (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) AND (dispatch.Branch_Id = @BranchID) AND (collections.TransactionType IS NULL) AND (collections.PaymentType = @pt) GROUP BY collections.PaidDate, accountheads.HeadName");
                cmd.Parameters.AddWithValue("@pt", "Journal Voucher");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                dtble = vdm.SelectQuery(cmd).Tables[0];

                fromdate = fromdate.AddDays(-1);
                string frmdate = fromdate.ToString("dd-MM-yyyy");
                string[] strjv = frmdate.Split('-');
                foreach (DataRow branch in dtble.Rows)
                {

                    DataRow newrow = Report.NewRow();
                    newrow["JV No."] = branch["sno"].ToString();
                    newrow["JV Date"] = fromdate.ToString("dd-MMM-yyyy");
                    newrow["Ledger Name"] = branch["tbranchname"].ToString();
                    double amount = 0;
                    double.TryParse(branch["TotalDiscount"].ToString(), out amount);
                    newrow["Amount"] = "-" + amount;
                    newrow["Narration"] = branch["remarks"].ToString() + ",Emp Name  " + Session["EmpName"].ToString();
                    Report.Rows.Add(newrow);
                    DataRow newrow1 = Report.NewRow();
                    newrow1["JV No."] = branch["sno"].ToString();
                    newrow1["JV Date"] = fromdate.ToString("dd-MMM-yyyy");
                    newrow1["Ledger Name"] = branch["HeadName"].ToString();
                    newrow1["Amount"] = amount;
                    newrow1["Narration"] = branch["remarks"].ToString() + ",Emp Name  " + Session["EmpName"].ToString();
                    Report.Rows.Add(newrow1);
                }
            }
            if (ddltype.SelectedValue == "Incentive Voucher")
            {
                Session["xporttype"] = "TallyIncentivevoucher";
                Session["filename"] = ddlSalesOffice.SelectedItem.Text + " Tally Incentive Voucher" + fromdate.ToString("dd/MM/yyyy");
                cmd = new MySqlCommand("SELECT branchdata.tbranchname, collections.Sno, collections.Remarks, collections.AmountPaid AS TotalDiscount, accountheads.HeadName FROM dispatch INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN collections ON branchdata.sno = collections.Branchid INNER JOIN accountheads ON collections.HeadSno = accountheads.Sno WHERE (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) AND (dispatch.Branch_Id = @BranchID) AND (collections.TransactionType IS NULL) AND (collections.PaymentType = @pt) GROUP BY collections.PaidDate, accountheads.HeadName");
                cmd.Parameters.AddWithValue("@pt", "Incentive");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                dtble = vdm.SelectQuery(cmd).Tables[0];
                fromdate = fromdate.AddDays(-1);
                string frmdate = fromdate.ToString("dd-MM-yyyy");
                string[] strjv = frmdate.Split('-');
                foreach (DataRow branch in dtble.Rows)
                {

                    DataRow newrow = Report.NewRow();
                    newrow["JV No."] = branch["sno"].ToString();
                    newrow["JV Date"] = fromdate.ToString("dd-MMM-yyyy");
                    newrow["Ledger Name"] = branch["tbranchname"].ToString();
                    double amount = 0;
                    double.TryParse(branch["TotalDiscount"].ToString(), out amount);
                    //totamount += amount;
                    newrow["Amount"] = "-" + amount;
                    newrow["Narration"] = branch["remarks"].ToString() + ",Emp Name  " + Session["EmpName"].ToString();
                    Report.Rows.Add(newrow);
                    DataRow newrow1 = Report.NewRow();
                    newrow1["JV No."] = branch["sno"].ToString();
                    newrow1["JV Date"] = fromdate.ToString("dd-MMM-yyyy");
                    newrow1["Ledger Name"] = branch["HeadName"].ToString();
                    newrow1["Amount"] = amount;
                    newrow1["Narration"] = branch["remarks"].ToString() + ",Emp Name  " + Session["EmpName"].ToString();
                    Report.Rows.Add(newrow1);
                }
            }
            grdReports.DataSource = Report;
            grdReports.DataBind();
            Session["xportdata"] = Report;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdReports.DataSource = Report;
            grdReports.DataBind();
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