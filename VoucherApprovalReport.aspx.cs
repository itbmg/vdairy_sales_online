using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class VoucherApprovalReport : System.Web.UI.Page
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
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                FillRouteName();

            }
        }
    }
    void FillRouteName()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Plant")
            {
                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("BranchName");
                dtBranch.Columns.Add("sno");
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) and (branchdata.flag<>0) ");
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
                cmd = new MySqlCommand("SELECT BranchName, sno FROM  branchdata WHERE (sno = @BranchID) and (flag<>0)");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtPlant.Rows)
                {
                    DataRow newrow = dtBranch.NewRow();
                    newrow["BranchName"] = dr["BranchName"].ToString();
                    newrow["sno"] = dr["sno"].ToString();
                    dtBranch.Rows.Add(newrow);
                }
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) and (branchdata.flag<>0) ");
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
                cmd = new MySqlCommand("SELECT BranchName, sno FROM branchdata WHERE (sno = @BranchID) and (flag<>0)");
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
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            pnlHide.Visible = true;
            vdm = new VehicleDBMgr();
            lblmsg.Text = "";
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
            DateTime Todate = DateTime.Now;
            string[] Todatestrig = txtTodate.Text.Split(' ');
            if (Todatestrig.Length > 1)
            {
                if (Todatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = Todatestrig[0].Split('-');
                    string[] times = Todatestrig[1].Split(':');
                    Todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            Session["filename"] = "Voucher Approval Report";
            string Status = ddlStatus.SelectedValue;
            if (Status == "ALL")
            {
                cmd = new MySqlCommand("SELECT cashpayables.VocherID, cashpayables.onNameof, empmanage.EmpName, DATE_FORMAT(cashpayables.DOE, '%d/%m/%Y %h:%i:%s %p') AS DOE , cashpayables.Amount, cashpayables.ApprovedAmount, cashpayables.Remarks, cashpayables.ApprovalRemarks, cashpayables.BranchID, cashpayables.Status,cashpayables.vouchertype, cashpayables.Sno AS refno FROM cashpayables INNER JOIN empmanage ON cashpayables.Approvedby = empmanage.Sno WHERE (cashpayables.BranchID = @BranchID) AND (cashpayables.DOE BETWEEN @d1 AND @d2)  ORDER BY cashpayables.DOE");

            }
            if (Status == "Raised")
            {
                cmd = new MySqlCommand("SELECT cashpayables.VocherID, cashpayables.onNameof, empmanage.EmpName, DATE_FORMAT(cashpayables.DOE, '%d/%m/%Y %h:%i:%s %p') AS DOE , cashpayables.Amount, cashpayables.ApprovedAmount, cashpayables.Remarks, cashpayables.ApprovalRemarks, cashpayables.BranchID, cashpayables.Status,cashpayables.vouchertype, cashpayables.Sno AS refno FROM cashpayables INNER JOIN empmanage ON cashpayables.Approvedby = empmanage.Sno WHERE (cashpayables.BranchID = @BranchID) AND (cashpayables.DOE BETWEEN @d1 AND @d2) AND (cashpayables.Status = @Status) ORDER BY cashpayables.DOE");
                cmd.Parameters.AddWithValue("@Status", 'R');
            }
            if (Status == "Approved")
            {
                cmd = new MySqlCommand("SELECT cashpayables.VocherID, cashpayables.onNameof, empmanage.EmpName, DATE_FORMAT(cashpayables.DOE, '%d/%m/%Y %h:%i:%s %p') AS DOE , cashpayables.Amount, cashpayables.ApprovedAmount, cashpayables.Remarks, cashpayables.ApprovalRemarks, cashpayables.BranchID, cashpayables.Status,cashpayables.vouchertype, cashpayables.Sno AS refno FROM cashpayables INNER JOIN empmanage ON cashpayables.Approvedby = empmanage.Sno WHERE (cashpayables.BranchID = @BranchID) AND (cashpayables.DOE BETWEEN @d1 AND @d2) AND (cashpayables.Status = @Status) ORDER BY cashpayables.DOE");
                cmd.Parameters.AddWithValue("@Status", 'A');
            }
            if (Status == "Rejected")
            {
                cmd = new MySqlCommand("SELECT cashpayables.VocherID, cashpayables.onNameof, empmanage.EmpName, DATE_FORMAT(cashpayables.DOE, '%d/%m/%Y %h:%i:%s %p') AS DOE , cashpayables.Amount, cashpayables.ApprovedAmount, cashpayables.Remarks, cashpayables.ApprovalRemarks, cashpayables.BranchID, cashpayables.Status,cashpayables.vouchertype, cashpayables.Sno AS refno FROM cashpayables INNER JOIN empmanage ON cashpayables.Approvedby = empmanage.Sno WHERE (cashpayables.BranchID = @BranchID) AND (cashpayables.DOE BETWEEN @d1 AND @d2) AND (cashpayables.Status = @Status) ORDER BY cashpayables.DOE");
                cmd.Parameters.AddWithValue("@Status", 'C');
            }
            if (Status == "Paid")
            {
                cmd = new MySqlCommand("SELECT cashpayables.VocherID, cashpayables.onNameof, empmanage.EmpName, DATE_FORMAT(cashpayables.DOE, '%d/%m/%Y %h:%i:%s %p') AS DOE , cashpayables.Amount, cashpayables.ApprovedAmount, cashpayables.Remarks, cashpayables.ApprovalRemarks, cashpayables.BranchID, cashpayables.Status,cashpayables.vouchertype, cashpayables.Sno AS refno FROM cashpayables INNER JOIN empmanage ON cashpayables.Approvedby = empmanage.Sno WHERE (cashpayables.BranchID = @BranchID) AND (cashpayables.DOE BETWEEN @d1 AND @d2) AND (cashpayables.Status = @Status) ORDER BY cashpayables.DOE");
                cmd.Parameters.AddWithValue("@Status", 'P');
            }
            //cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            DataTable dtCheque = vdm.SelectQuery(cmd).Tables[0];
            Report = new DataTable();
            Report.Columns.Add("VoucherDate");
            Report.Columns.Add("VoucherID");
            Report.Columns.Add("Voucher Type");
            Report.Columns.Add("Name");
            Report.Columns.Add("Status");
            //Report.Columns.Add("Approval");
            Report.Columns.Add("Amount").DataType = typeof(Double);
            //Report.Columns.Add("Remarks");
            Report.Columns.Add("ApprovedAmount").DataType = typeof(Double);
            Report.Columns.Add("ApprovalRemarks");
            Report.Columns.Add("Head Of Account");
            if (dtCheque.Rows.Count > 0)
            {
                foreach (DataRow dr in dtCheque.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["VoucherDate"] = dr["DOE"].ToString();
                    newrow["VoucherID"] = dr["VocherID"].ToString();
                    newrow["Voucher Type"] = dr["vouchertype"].ToString();
                    newrow["Name"] = dr["onNameof"].ToString();
                    //newrow["Approval"] = dr["EmpName"].ToString();
                   // newrow["Remarks"] = dr["Remarks"].ToString();
                    string ColStatus = dr["Status"].ToString();
                    string ChequeStatus = "";
                    if (ColStatus == "R")
                    {
                        ChequeStatus = "Raised";
                    }
                    if (ColStatus == "A")
                    {
                        ChequeStatus = "Approved";
                    }
                    if (ColStatus == "C")
                    {
                        ChequeStatus = "Rejected";
                    }
                    if (ColStatus == "P")
                    {
                        ChequeStatus = "Paid";
                    }
                    newrow["Status"] = ChequeStatus;
                    double ApprovedAmount = 0;
                    double.TryParse(dr["ApprovedAmount"].ToString(), out ApprovedAmount);
                    double Amount = 0;
                    double.TryParse(dr["Amount"].ToString(), out Amount);
                    newrow["Amount"] = Amount;
                    newrow["ApprovedAmount"] = ApprovedAmount;
                    newrow["ApprovalRemarks"] = dr["ApprovalRemarks"].ToString();
                    cmd = new MySqlCommand("SELECT subpayable.RefNo, subpayable.HeadDesc, subpayable.Amount, subpayable.HeadSno, accountheads.HeadName FROM subpayable INNER JOIN accountheads ON subpayable.HeadSno = accountheads.Sno WHERE (subpayable.RefNo = @refno)");
                    cmd.Parameters.AddWithValue("@refno", dr["refno"].ToString());
                    DataTable dtheadacc = vdm.SelectQuery(cmd).Tables[0];
                    string head = "";
                    foreach (DataRow drhead in dtheadacc.Rows)
                    {
                        head += drhead["HeadName"].ToString() + "-->" + drhead["Amount"].ToString() + "\r\n";
                    }
                    newrow["Head Of Account"] = head;
                    Report.Rows.Add(newrow);
                }
                DataRow newTotal = Report.NewRow();
                newTotal["Name"] = "Total Amount";
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
            }
            else
            {
                lblmsg.Text = "No Vouchers were Found";
                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdReports.DataSource = Report;
            grdReports.DataBind();
        }
    }
}