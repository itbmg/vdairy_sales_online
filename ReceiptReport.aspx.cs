using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class ReceiptReport : System.Web.UI.Page
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
            }
        }
    }
    protected void grdReports_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = Convert.ToInt32(e.CommandArgument);
        GridViewRow row = grdReports.Rows[rowIndex];
        string ReceiptNo = row.Cells[2].Text;
        string Type = row.Cells[4].Text;
        Session["ReceiptNo"] = ReceiptNo;
        Session["Type"] = Type;
        Response.Redirect("ReceiptBook.aspx",false);
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
            Session["filename"] = "Receipt Report";
            string Status = ddlStatus.SelectedValue;
            string ColStatus = "";
            if (Status == "Route")
            {
                ColStatus = "SalesMen";
                cmd = new MySqlCommand("SELECT DATE_FORMAT(cashreceipts.DOE, '%d %b %y') AS DOE,cashreceipts.Sno,cashreceipts.Receipt ,cashreceipts.ReceivedFrom AS Type,dispatch.DispName, empmanage.EmpName, cashreceipts.AmountPaid FROM cashreceipts INNER JOIN dispatch ON cashreceipts.dispatchid = dispatch.sno INNER JOIN empmanage ON cashreceipts.Empid = empmanage.Sno WHERE (cashreceipts.BranchId = @BranchID) AND (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.ReceivedFrom = @Type) ORDER BY cashreceipts.DOE");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                cmd.Parameters.AddWithValue("@Type", ColStatus);
                DataTable dtReceipt = vdm.SelectQuery(cmd).Tables[0];
                grdReports.DataSource = dtReceipt;
                grdReports.DataBind();
            }
            if (Status == "Agent")
            {
                ColStatus = "Agent";
                cmd = new MySqlCommand("SELECT DATE_FORMAT(cashreceipts.DOE, '%d %b %y') AS DOE, cashreceipts.Sno, cashreceipts.Receipt, cashreceipts.ReceivedFrom AS Type, branchdata.BranchName, cashreceipts.AmountPaid FROM cashreceipts INNER JOIN branchdata ON cashreceipts.AgentID = branchdata.sno INNER JOIN branchdata branchdata_1 ON cashreceipts.BranchId = branchdata_1.sno WHERE (cashreceipts.BranchId = @BranchID) AND (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.ReceivedFrom = @Type) OR (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.ReceivedFrom = @Type) AND (branchdata_1.SalesOfficeID = @SalesOfficeID )ORDER BY DOE");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                cmd.Parameters.AddWithValue("@Type", ColStatus);
                cmd.Parameters.AddWithValue("@SalesOfficeID", Session["branch"]);
                DataTable dtReceipt = vdm.SelectQuery(cmd).Tables[0];
                grdReports.DataSource = dtReceipt;
                grdReports.DataBind();
            }
            if (Status == "Others")
            {
                ColStatus = "Others";
                cmd = new MySqlCommand("SELECT DATE_FORMAT(DOE, '%d %b %y') AS DOE,Sno,Receiptno,PaymentType AS Type, Name, Amount  FROM cashcollections WHERE (Branchid = @BranchID) AND (DOE BETWEEN @d1 AND @d2) ORDER BY DOE");
               // cmd = new MySqlCommand("SELECT DATE_FORMAT(cashreceipts.DOE, '%d %b %y') AS DOE, cashreceipts.Sno, cashreceipts.Receipt, cashreceipts.AmountPaid, cashcoll.Name, cashcoll.Amount FROM cashreceipts INNER JOIN (SELECT Branchid, Name, Amount, Remarks, DOE, Receiptno, Agentid, PaymentType, CollectionType, CollectionFrom, CheckStatus, ChequeNo, VarifyDate, ChequeDate, BankName, Sno, VEmpID FROM cashcollections WHERE (DOE BETWEEN @d1 AND @d2)) cashcoll ON cashreceipts.Receipt = cashcoll.Receiptno WHERE (cashreceipts.BranchId = @BranchID) AND (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.ReceivedFrom = @Type) ORDER BY DOE");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                cmd.Parameters.AddWithValue("@Type", ColStatus);
                DataTable dtReceipt = vdm.SelectQuery(cmd).Tables[0];
                grdReports.DataSource = dtReceipt;
                grdReports.DataBind();
            }
            if (Status == "All")
            {
                DataTable Report = new DataTable();
                Report.Columns.Add("DOE");
                Report.Columns.Add("Ref Receipt");
                Report.Columns.Add("Receipt");
                Report.Columns.Add("Type");
                Report.Columns.Add("Name");
                Report.Columns.Add("Amount").DataType = typeof(Double);
                cmd = new MySqlCommand("SELECT branchdata.BranchName,DATE_FORMAT(collections.PaidDate, '%d %b %y') AS DOE , collections.PaymentType, collections.AmountPaid, collections.Sno, collections.ReceiptNo FROM collections INNER JOIN branchmappingtable ON collections.Branchid = branchmappingtable.SubBranch INNER JOIN branchdata ON collections.Branchid = branchdata.sno WHERE (collections.PaymentType <> 'Cash') AND (collections.PaymentType <> 'PhonePay') AND (collections.PaidDate BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @BranchID) ORDER BY branchdata.BranchName");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                DataTable dtothers = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtothers.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["DOE"] = dr["DOE"].ToString();
                    newrow["Ref Receipt"] = dr["Sno"].ToString();
                    newrow["Receipt"] = dr["ReceiptNo"].ToString();
                    newrow["Type"] = dr["PaymentType"].ToString();

                    newrow["Name"] = dr["BranchName"].ToString();
                    double AmountPaid = 0;
                    double.TryParse(dr["AmountPaid"].ToString(), out AmountPaid);
                    newrow["Amount"] = AmountPaid;
                    Report.Rows.Add(newrow);
                }
                cmd = new MySqlCommand("SELECT DATE_FORMAT(cashreceipts.DOE, '%d %b %y') AS DOE,cashreceipts.Sno, cashreceipts.Receipt ,cashreceipts.ReceivedFrom AS Type,dispatch.DispName, empmanage.EmpName, cashreceipts.AmountPaid FROM cashreceipts INNER JOIN dispatch ON cashreceipts.dispatchid = dispatch.sno INNER JOIN empmanage ON cashreceipts.Empid = empmanage.Sno WHERE (cashreceipts.BranchId = @BranchID) AND (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.ReceivedFrom = @Type) ORDER BY cashreceipts.DOE");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                cmd.Parameters.AddWithValue("@Type", "SalesMen");
                DataTable dtRoute = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtRoute.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["DOE"] = dr["DOE"].ToString();
                    newrow["Ref Receipt"] = dr["Sno"].ToString();
                    newrow["Receipt"] = dr["Receipt"].ToString();
                    newrow["Type"] = dr["Type"].ToString();
                    
                    newrow["Name"] = dr["DispName"].ToString();
                    double AmountPaid = 0;
                    double.TryParse(dr["AmountPaid"].ToString(), out AmountPaid);
                    newrow["Amount"] = AmountPaid;
                    Report.Rows.Add(newrow);
                }
                cmd = new MySqlCommand("SELECT DATE_FORMAT(cashreceipts.DOE, '%d %b %y') AS DOE, cashreceipts.Sno, cashreceipts.Receipt, cashreceipts.ReceivedFrom AS Type, branchdata.BranchName, cashreceipts.AmountPaid,cashreceipts.PaymentStatus FROM cashreceipts INNER JOIN branchdata ON cashreceipts.AgentID = branchdata.sno INNER JOIN branchdata branchdata_1 ON cashreceipts.BranchId = branchdata_1.sno WHERE (cashreceipts.BranchId = @BranchID) AND (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.ReceivedFrom = @Type) OR (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.ReceivedFrom = @Type) AND (branchdata_1.SalesOfficeID = @SalesOfficeID )ORDER BY DOE");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                cmd.Parameters.AddWithValue("@Type", "Agent");
                cmd.Parameters.AddWithValue("@SalesOfficeID", Session["branch"]);
                DataTable dtAgent = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtAgent.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["DOE"] = dr["DOE"].ToString();
                    newrow["Ref Receipt"] = dr["Sno"].ToString();

                    newrow["Receipt"] = dr["Receipt"].ToString();
                    newrow["Type"] = dr["PaymentStatus"].ToString();

                    newrow["Name"] = dr["BranchName"].ToString();
                    double AmountPaid = 0;
                    double.TryParse(dr["AmountPaid"].ToString(), out AmountPaid);
                    newrow["Amount"] = AmountPaid;
                    Report.Rows.Add(newrow);
                }

                cmd = new MySqlCommand("SELECT Sno,DATE_FORMAT(DOE, '%d %b %y') AS DOE,Receiptno,PaymentType as Type, Name, Amount  FROM cashcollections WHERE (Branchid = @BranchID) AND (DOE BETWEEN @d1 AND @d2) ORDER BY DOE");
                //cmd = new MySqlCommand("SELECT DATE_FORMAT(cashreceipts.DOE, '%d %b %y') AS DOE, cashreceipts.Sno, cashreceipts.Receipt, cashreceipts.AmountPaid, cashcoll.Name, cashcoll.Amount FROM cashreceipts INNER JOIN (SELECT Branchid, Name, Amount, Remarks, DOE, Receiptno, Agentid, PaymentType, CollectionType, CollectionFrom, CheckStatus, ChequeNo, VarifyDate, ChequeDate, BankName, Sno, VEmpID FROM cashcollections WHERE (DOE BETWEEN @d1 AND @d2)) cashcoll ON cashreceipts.Receipt = cashcoll.Receiptno WHERE (cashreceipts.BranchId = @BranchID) AND (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.ReceivedFrom = @Type) ORDER BY DOE");

                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                cmd.Parameters.AddWithValue("@Type", "Others");
                DataTable dtOthers = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtOthers.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["DOE"] = dr["DOE"].ToString();
                    newrow["Ref Receipt"] = dr["Sno"].ToString();
                    newrow["Receipt"] = dr["Receiptno"].ToString();
                    newrow["Type"] = dr["Type"].ToString();
                    newrow["Name"] = dr["Name"].ToString();
                    double AmountPaid = 0;
                    double.TryParse(dr["Amount"].ToString(), out AmountPaid);
                    newrow["Amount"] = AmountPaid;
                    Report.Rows.Add(newrow);
                }
                DataView dv = Report.DefaultView;
                dv.Sort = "Receipt ASC";
                DataTable sortedDT = dv.ToTable();
                DataRow newvartical = sortedDT.NewRow();
                newvartical["Name"] = "Total";
                double val = 0.0;
                foreach (DataColumn dc in sortedDT.Columns)
                {
                    if (dc.DataType == typeof(Double))
                    {
                        val = 0.0;
                        double.TryParse(sortedDT.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                        newvartical[dc.ToString()] = val;
                    }
                }
                sortedDT.Rows.Add(newvartical);
                grdReports.DataSource = sortedDT;
                grdReports.DataBind();
            }
        }
        catch(Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}