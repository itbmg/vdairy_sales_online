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

public partial class TallyPayments : System.Web.UI.Page
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
    DataTable dtReport = new DataTable();
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
            DataTable Report = new DataTable();
            Report.Columns.Add("DOE");
            Report.Columns.Add("Ref Receipt");
            Report.Columns.Add("Receipt");
            Report.Columns.Add("Type");
            Report.Columns.Add("Name");
            Report.Columns.Add("Amount").DataType = typeof(Double);
            lbl_selfromdate.Text = fromdate.ToString("dd/MM/yyyy");
            lblRoutName.Text = ddlSalesOffice.SelectedItem.Text;
            DateTime ReportDate = VehicleDBMgr.GetTime(vdm.conn);
            DateTime dtapril = new DateTime();
            DateTime dtmarch = new DateTime();
            int currentyear = ReportDate.Year;
            int nextyear = ReportDate.Year + 1;
            if (ReportDate.Month > 3)
            {
                string apr = "4/1/" + currentyear;
                dtapril = DateTime.Parse(apr);
                string march = "3/31/" + nextyear;
                dtmarch = DateTime.Parse(march);
            }
            if (ReportDate.Month <= 3)
            {
                string apr = "4/1/" + (currentyear - 1);
                dtapril = DateTime.Parse(apr);
                string march = "3/31/" + (nextyear - 1);
                dtmarch = DateTime.Parse(march);
            }
            Session["xporttype"] = "TallyPayments";
            string ledger = "";
            cmd = new MySqlCommand("SELECT tbranchname, ladger_dr FROM branchdata WHERE (sno = @BranchID)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            DataTable dtledger = vdm.SelectQuery(cmd).Tables[0];
            if (dtledger.Rows.Count > 0)
            {
                ledger = dtledger.Rows[0]["ladger_dr"].ToString();
            }
            Session["filename"] = ddlSalesOffice.SelectedItem.Text + " Tally Payments" + fromdate.ToString("dd/MM/yyyy");
            cmd = new MySqlCommand("SELECT cashpayables.BranchID,cashpayables.doe,cashpayables.VocherID,subpayable.vouchercode,cashpayables.sno,subpayable.HeadSno,cashpayables.Remarks, cashpayables.Remarks,subpayable.sno, subpayable.Amount, accountheads.HeadName FROM cashpayables INNER JOIN subpayable ON cashpayables.Sno = subpayable.RefNo INNER JOIN accountheads ON subpayable.HeadSno = accountheads.Sno WHERE (cashpayables.BranchID = @BranchID) AND (cashpayables.DOE BETWEEN @d1 AND @d2) AND ((cashpayables.VoucherType = 'Debit') OR (cashpayables.VoucherType = 'SalaryAdvance') OR (cashpayables.VoucherType = 'SalaryPayble'))  AND (cashpayables.Status = 'P')");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            DataTable dtAgent = vdm.SelectQuery(cmd).Tables[0];
            if (dtAgent.Rows.Count > 0)
            {
                DataView view = new DataView(dtAgent);
                dtReport = new DataTable();
                dtReport.Columns.Add("Voucher Date");
                dtReport.Columns.Add("Voucher No");
                dtReport.Columns.Add("Voucher Type");
                dtReport.Columns.Add("Ledger (Cr)");
                dtReport.Columns.Add("Ledger (Dr)");
                dtReport.Columns.Add("Amount");
                dtReport.Columns.Add("Narration");
                int i = 1;
                foreach (DataRow branch in dtAgent.Rows)
                {
                    string VoucherNo = "";
                    cmd = new MySqlCommand("SELECT  vouchercode,RefNo, HeadDesc, Amount, HeadSno, sno, branchid, paiddate FROM subpayable  WHERE (branchid = @BranchID) AND (paiddate BETWEEN @d1 AND @d2)");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                    cmd.Parameters.AddWithValue("@d2", GetLowDate(fromdate));
                    DataTable dtVoucher = vdm.SelectQuery(cmd).Tables[0];
                    if (dtVoucher.Rows.Count > 0)
                    {
                        DataRow[] drvoucher = dtVoucher.Select("branchid='" + ddlSalesOffice.SelectedValue + "' and RefNo='" + branch["sno"].ToString() + "' and HeadSno='" + branch["HeadSno"].ToString() + "'");
                        if (drvoucher.Length > 0)
                        {
                            foreach (DataRow drv in drvoucher)
                            {
                                VoucherNo = drv.ItemArray[0].ToString();
                            }
                        }
                        else
                        {
                            cmd = new MySqlCommand("SELECT IFNULL(MAX(vouchercode), 0) + 1 AS Sno FROM subpayable WHERE (branchid = @branchid)  AND (paiddate BETWEEN @d1 AND @d2)");
                            cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
                            cmd.Parameters.AddWithValue("@HeadSno", branch["HeadSno"].ToString());
                            cmd.Parameters.AddWithValue("@d1", GetLowDate(dtapril));
                            cmd.Parameters.AddWithValue("@d2", GetHighDate(dtmarch));
                            DataTable dtvoucherno = vdm.SelectQuery(cmd).Tables[0];
                            VoucherNo = dtvoucherno.Rows[0]["Sno"].ToString();
                            cmd = new MySqlCommand("update  subpayable set vouchercode=@vouchercode, paiddate=@paiddate,branchid=@branchid  where (RefNo=@RefNo) AND (HeadSno = @HeadSno)"); ;
                            cmd.Parameters.AddWithValue("@vouchercode", VoucherNo);
                            cmd.Parameters.AddWithValue("@paiddate", fromdate);
                            cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
                            cmd.Parameters.AddWithValue("@RefNo", branch["sno"].ToString());
                            cmd.Parameters.AddWithValue("@HeadSno", branch["HeadSno"].ToString());
                            vdm.Update(cmd);
                        }
                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT IFNULL(MAX(vouchercode), 0) + 1 AS Sno FROM subpayable WHERE (branchid = @branchid)  AND (paiddate BETWEEN @d1 AND @d2)");
                        cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
                        cmd.Parameters.AddWithValue("@HeadSno", branch["HeadSno"].ToString());
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(dtapril));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(dtmarch));
                        DataTable dtvoucherno = vdm.SelectQuery(cmd).Tables[0];
                        VoucherNo = dtvoucherno.Rows[0]["Sno"].ToString();
                        cmd = new MySqlCommand("update  subpayable set vouchercode=@vouchercode, paiddate=@paiddate,branchid=@branchid  where  (RefNo=@RefNo) AND (HeadSno = @HeadSno)"); ;
                        cmd.Parameters.AddWithValue("@vouchercode", VoucherNo);
                        cmd.Parameters.AddWithValue("@paiddate", fromdate);
                        cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
                        cmd.Parameters.AddWithValue("@RefNo", branch["sno"].ToString());
                        cmd.Parameters.AddWithValue("@HeadSno", branch["HeadSno"].ToString());
                        vdm.Update(cmd);
                    }
                    if (VoucherNo == "0")
                    {
                        cmd = new MySqlCommand("SELECT IFNULL(MAX(vouchercode), 0) + 1 AS Sno FROM subpayable WHERE (branchid = @branchid)  AND (paiddate BETWEEN @d1 AND @d2)");
                        cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
                        cmd.Parameters.AddWithValue("@HeadSno", branch["HeadSno"].ToString());
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(dtapril));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(dtmarch));
                        DataTable dtvoucherno = vdm.SelectQuery(cmd).Tables[0];
                        VoucherNo = dtvoucherno.Rows[0]["Sno"].ToString();
                        cmd = new MySqlCommand("update  subpayable set vouchercode=@vouchercode, paiddate=@paiddate,branchid=@branchid  where  (RefNo=@RefNo) AND (HeadSno = @HeadSno)"); ;
                        cmd.Parameters.AddWithValue("@vouchercode", VoucherNo);
                        cmd.Parameters.AddWithValue("@paiddate", fromdate);
                        cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
                        cmd.Parameters.AddWithValue("@RefNo", branch["sno"].ToString());
                        cmd.Parameters.AddWithValue("@HeadSno", branch["HeadSno"].ToString());
                        vdm.Update(cmd);
                    }
                    string NewVoucherNo = "0";
                    int countdc = 0;
                    int.TryParse(VoucherNo, out countdc);
                    if (countdc <= 10)
                    {
                        NewVoucherNo = "0000" + countdc;
                    }
                    if (countdc >= 10 && countdc <= 99)
                    {
                        NewVoucherNo = "000" + countdc;
                    }
                    if (countdc >= 99 && countdc <= 999)
                    {
                        NewVoucherNo = "00" + countdc;
                    }
                    if (countdc >= 999)
                    {
                        NewVoucherNo = "0" + countdc;
                    }
                    if (ddlSalesOffice.SelectedValue == "172")
                    {
                        NewVoucherNo = "0" + NewVoucherNo;
                    }
                    DataRow newrow = dtReport.NewRow();
                    newrow["Voucher Date"] = fromdate.ToString("dd-MMM-yyyy");
                    if (fromdate.Month > 3)
                    {
                        newrow["Voucher No"] = dtapril.ToString("yy") + dtmarch.ToString("yy") + NewVoucherNo;
                    }
                    else
                    {
                        if (fromdate.Month < 3)
                        {
                            newrow["Voucher No"] = dtapril.ToString("yy") + dtmarch.ToString("yy") + NewVoucherNo;
                        }
                        else
                        {
                            newrow["Voucher No"] = dtapril.AddYears(-1).ToString("yy") + dtmarch.AddYears(-1).ToString("yy") + NewVoucherNo;
                        }
                    }
                    //if (fromdate.Month > 3)
                    //{
                    //    newrow["Voucher No"] = dtapril.ToString("yy") + dtmarch.ToString("yy") + NewVoucherNo;
                    //}
                    //else
                    //{
                    //    newrow["Voucher No"] = dtapril.AddYears(-1).ToString("yy") + dtmarch.AddYears(-1).ToString("yy") + NewVoucherNo;
                    //}
                    newrow["Voucher Type"] = "Cash Payment Import";
                    newrow["Ledger (Cr)"] = ledger;
                    newrow["Ledger (Dr)"] = branch["HeadName"].ToString();
                    newrow["Amount"] = branch["Amount"].ToString();
                    double invval = 0;
                    newrow["Narration"] = branch["Remarks"].ToString() + ",VoucherID  " + VoucherNo + ",Emp Name  " + Session["EmpName"].ToString();
                    dtReport.Rows.Add(newrow);
                    i++;
                }
                grdReports.DataSource = dtReport;
                grdReports.DataBind();
                Session["xportdata"] = dtReport;
            }
            else
            {
                pnlHide.Visible = false;
                lblmsg.Text = "No Indent Found";
                grdReports.DataSource = dtReport;
                grdReports.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdReports.DataSource = dtReport;
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