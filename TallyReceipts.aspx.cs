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

public partial class TallyReceipts : System.Web.UI.Page
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
            cmd.Parameters.AddWithValue("@SalesType", "4");
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
            if (ddlType.SelectedValue == "CashReceipts")
            {
                Report.Columns.Add("DOE");
                Report.Columns.Add("Ref Receipt");
                Report.Columns.Add("Receipt");
                Report.Columns.Add("Type");
                Report.Columns.Add("Name");
                Report.Columns.Add("Amount").DataType = typeof(Double);
                Report.Columns.Add("Remarks");
                lbl_selfromdate.Text = fromdate.ToString("dd/MM/yyyy");
                lblRoutName.Text = ddlSalesOffice.SelectedItem.Text;
                Session["xporttype"] = "TallyReceipts";
                string ledger = "";
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
                cmd = new MySqlCommand("SELECT tbranchname, ladger_dr FROM branchdata WHERE (sno = @BranchID)");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                DataTable dtledger = vdm.SelectQuery(cmd).Tables[0];
                if (dtledger.Rows.Count > 0)
                {
                    ledger = dtledger.Rows[0]["ladger_dr"].ToString();
                }
                Session["filename"] = ddlSalesOffice.SelectedItem.Text + " Tally Receipts" + fromdate.ToString("dd/MM/yyyy");
                DataTable dtAgent = vdm.SelectQuery(cmd).Tables[0];

                if (ddlSalesOffice.SelectedValue == "1")
                {
                    cmd = new MySqlCommand("SELECT branchdata.SalesType, branchdata.tbranchname,cashreceipts.Remarks, cashreceipts.AmountPaid ,cashreceipts.Receipt,DATE_FORMAT(cashreceipts.DOE, '%d %b %y') AS DOE FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno INNER JOIN cashreceipts ON branchdata.sno = cashreceipts.AgentID WHERE (branchmappingtable.SuperBranch = @BranchID) AND (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.AmountPaid > 0) AND (cashreceipts.PaymentStatus = 'Cash')   OR (branchdata_1.SalesOfficeID = @SOID) AND (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.AmountPaid > 0) AND (cashreceipts.PaymentStatus = 'Cash')  ");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                    cmd.Parameters.AddWithValue("@Type", "Agent");
                    DataTable Agent = vdm.SelectQuery(cmd).Tables[0];
                    foreach (DataRow dr in Agent.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        string salestype = dr["salestype"].ToString();
                        newrow["DOE"] = dr["DOE"].ToString();
                        newrow["Receipt"] = dr["Receipt"].ToString();
                        newrow["Name"] = dr["tBranchName"].ToString();
                        double AmountPaid = 0;
                        double.TryParse(dr["AmountPaid"].ToString(), out AmountPaid);
                        newrow["Amount"] = AmountPaid;
                        newrow["Remarks"] = dr["Remarks"].ToString();
                        Report.Rows.Add(newrow);
                    }
                }
                else
                {
                    cmd = new MySqlCommand("SELECT branchdata.salestype,branchdata.tBranchName,collections.ReceiptNo,collections.Sno,DATE_FORMAT(collections.PaidDate, '%d %b %y') AS DOE , collections.AmountPaid, collections.PaymentType,collections.Remarks FROM collections INNER JOIN branchdata ON collections.Branchid = branchdata.sno INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno WHERE (collections.PaidDate BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @BranchID) AND (collections.PaymentType = 'Cash') AND (collections.AmountPaid > 0) OR (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.PaymentType = 'Cash') AND (branchdata_1.SalesOfficeID = @SOID) AND (collections.AmountPaid > 0)");
                    // 01/09/2017
                    //cmd = new MySqlCommand("SELECT branchdata.SalesType, branchdata.tbranchname,cashreceipts.Remarks, cashreceipts.AmountPaid ,cashreceipts.Receipt,DATE_FORMAT(cashreceipts.DOE, '%d %b %y') AS DOE FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno INNER JOIN cashreceipts ON branchdata.sno = cashreceipts.AgentID WHERE (branchmappingtable.SuperBranch = @BranchID) AND (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.AmountPaid > 0) AND (cashreceipts.PaymentStatus = 'Cash')   OR (branchdata_1.SalesOfficeID = @SOID) AND (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.AmountPaid > 0) AND (cashreceipts.PaymentStatus = 'Cash')  ");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                    cmd.Parameters.AddWithValue("@Type", "Agent");
                    DataTable Agent = vdm.SelectQuery(cmd).Tables[0];
                    foreach (DataRow dr in Agent.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        string salestype = dr["salestype"].ToString();
                        newrow["DOE"] = dr["DOE"].ToString();
                        newrow["Receipt"] = dr["ReceiptNo"].ToString();
                        newrow["Name"] = dr["tBranchName"].ToString();
                        double AmountPaid = 0;
                        double.TryParse(dr["AmountPaid"].ToString(), out AmountPaid);
                        newrow["Amount"] = AmountPaid;
                        newrow["Remarks"] = dr["Remarks"].ToString();
                        Report.Rows.Add(newrow);
                    }
                }
                cmd = new MySqlCommand("SELECT Sno,DATE_FORMAT(DOE, '%d %b %y') AS DOE,Receiptno,PaymentType as Type, Name, Amount,Remarks  FROM cashcollections WHERE (Branchid = @BranchID) AND (DOE BETWEEN @d1 AND @d2) AND   (CollectionType = 'Cash') ORDER BY DOE");
                //cmd = new MySqlCommand("SELECT DATE_FORMAT(cashreceipts.DOE, '%d %b %y') AS DOE, cashreceipts.Sno, cashreceipts.Receipt, cashreceipts.AmountPaid, cashcoll.Name, cashcoll.Amount FROM cashreceipts INNER JOIN (SELECT Branchid, Name, Amount, Remarks, DOE, Receiptno, Agentid, PaymentType, CollectionType, CollectionFrom, CheckStatus, ChequeNo, VarifyDate, ChequeDate, BankName, Sno, VEmpID FROM cashcollections WHERE (DOE BETWEEN @d1 AND @d2)) cashcoll ON cashreceipts.Receipt = cashcoll.Receiptno WHERE (cashreceipts.BranchId = @BranchID) AND (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.ReceivedFrom = @Type) ORDER BY DOE");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                cmd.Parameters.AddWithValue("@Type", "Others");
                DataTable dtOthers = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtOthers.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["DOE"] = dr["DOE"].ToString();
                    newrow["Receipt"] = dr["Receiptno"].ToString();
                    newrow["Type"] = dr["Type"].ToString();
                    newrow["Name"] = dr["Name"].ToString();
                    double AmountPaid = 0;
                    double.TryParse(dr["Amount"].ToString(), out AmountPaid);
                    newrow["Amount"] = AmountPaid;
                    newrow["Remarks"] = dr["Remarks"].ToString();
                    Report.Rows.Add(newrow);
                }
                string Receiptno = "";


                if (fromdate.Month > 3)
                {
                    Receiptno = dtapril.ToString("yy") + dtmarch.ToString("yy");
                }
                else
                {
                    if (fromdate.Month < 3)
                    {
                        Receiptno = dtapril.ToString("yy") + dtmarch.ToString("yy");
                    }
                    else
                    {
                        Receiptno = dtapril.AddYears(-1).ToString("yy") + dtmarch.AddYears(-1).ToString("yy");
                    }
                }
                //if (fromdate.Month > 3)
                //{
                //    Receiptno = dtapril.ToString("yy") + dtmarch.ToString("yy");
                //}
                //else
                //{
                //    Receiptno = dtapril.AddYears(-1).ToString("yy") + dtmarch.AddYears(-1).ToString("yy");
                //}
                if (Report.Rows.Count > 0)
                {
                    DataView view = new DataView(Report);
                    dtReport = new DataTable();
                    dtReport.Columns.Add("Voucher Date");
                    dtReport.Columns.Add("Voucher No");
                    dtReport.Columns.Add("Voucher Type");
                    dtReport.Columns.Add("Ledger (Dr)");
                    dtReport.Columns.Add("Ledger (Cr)");
                    dtReport.Columns.Add("Amount");
                    dtReport.Columns.Add("Narration");
                    //DataTable distincttable = view.ToTable(true, "BranchName", "BSno");
                    int i = 1;
                    foreach (DataRow branch in Report.Rows)
                    {
                        DataRow newrow = dtReport.NewRow();
                        //newrow["SNo"] = i;
                        //string DCNO = "0";
                        //cmd = new MySqlCommand("SELECT DcNo FROM  agentdc WHERE (BranchID = @BranchID) AND (IndDate BETWEEN @d1 AND @d2)");
                        //cmd.Parameters.AddWithValue("@BranchID", branch["BSno"].ToString());
                        //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        //cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        //DataTable dtDc = vdm.SelectQuery(cmd).Tables[0];
                        //if (dtDc.Rows.Count > 0)
                        //{
                        //    DCNO = dtDc.Rows[0]["DcNo"].ToString();
                        //}
                        //else
                        //{
                        //}
                        //double Roundingoff = 0;
                        //double taxval = 0;
                        newrow["Voucher Date"] = fromdate.ToString("dd-MMM-yyyy");
                        string newreceipt = "0";
                        int countdc = 0;
                        int.TryParse(branch["Receipt"].ToString(), out countdc);
                        if (countdc < 10)
                        {
                            newreceipt = "0000" + countdc;
                        }
                        if (countdc >= 10 && countdc <= 99)
                        {
                            newreceipt = "000" + countdc;
                        }
                        if (countdc >= 99 && countdc <= 999)
                        {
                            newreceipt = "00" + countdc;
                        }
                        if (countdc >= 999 && countdc <= 9999)
                        {
                            newreceipt = "0" + countdc;
                        }
                        if (countdc > 9999)
                        {
                            newreceipt = "" + countdc;
                        }
                        newrow["Voucher No"] = Receiptno + newreceipt;
                        newrow["Voucher Type"] = "Cash Receipt Import";
                        newrow["Ledger (Dr)"] = ledger;
                        if (branch["Name"].ToString() == "")
                        {
                        }
                        else
                        {
                            newrow["Ledger (Cr)"] = branch["Name"].ToString();
                            newrow["Amount"] = branch["Amount"].ToString();
                            double invval = 0;
                            string Remarks = branch["Remarks"].ToString();
                            if (Remarks.Length < 25)
                            {
                                newrow["Narration"] = "Being the cash receipt to  " + branch["Name"].ToString() + " vide Receipt No " + branch["Receipt"].ToString() + ",Receipt Date " + fromdate.ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                            }
                            else
                            {
                                newrow["Narration"] = Remarks + " vide Receipt No " + branch["Receipt"].ToString() + ",Receipt Date " + fromdate.ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                            }
                            dtReport.Rows.Add(newrow);
                            i++;
                        }
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
            else if (ddlType.SelectedValue == "JournelImport")
            {
                Report.Columns.Add("DOE");
                Report.Columns.Add("Ref Receipt");
                Report.Columns.Add("Receipt");
                Report.Columns.Add("Type");
                Report.Columns.Add("Name");
                Report.Columns.Add("Amount").DataType = typeof(Double);
                Report.Columns.Add("Remarks");
                lbl_selfromdate.Text = fromdate.ToString("dd/MM/yyyy");
                lblRoutName.Text = ddlSalesOffice.SelectedItem.Text;
                Session["xporttype"] = "TallyJournelImport";
                string ledger = "";
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
                ledger = "SVDS.P.LTD PUNABAKA PLANT";

                Session["filename"] = ddlSalesOffice.SelectedItem.Text + " Tally Journel Import" + fromdate.ToString("dd/MM/yyyy");

                //  cmd = new MySqlCommand("SELECT branchdata.salestype,branchdata.tBranchName,collections.ReceiptNo,collections.Sno,DATE_FORMAT(collections.PaidDate, '%d %b %y') AS DOE , collections.AmountPaid, collections.PaymentType FROM collections INNER JOIN branchdata ON collections.Branchid = branchdata.sno INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno WHERE (collections.PaidDate BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @BranchID) AND (collections.PaymentType = 'Cash') AND (collections.AmountPaid > 0) OR (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.PaymentType = 'Cash') AND (branchdata_1.SalesOfficeID = @SOID) AND (collections.AmountPaid > 0)");
                // 01/09/2017
                cmd = new MySqlCommand("SELECT branchdata.SalesType, branchdata.tbranchname,cashreceipts.Remarks, cashreceipts.AmountPaid ,cashreceipts.Receipt,DATE_FORMAT(cashreceipts.DOE, '%d %b %y') AS DOE FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno INNER JOIN cashreceipts ON branchdata.sno = cashreceipts.AgentID WHERE (branchmappingtable.SuperBranch = @BranchID) AND (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.AmountPaid > 0) AND (cashreceipts.PaymentStatus = 'PhonePay')   OR (branchdata_1.SalesOfficeID = @SOID) AND (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.AmountPaid > 0) AND (cashreceipts.PaymentStatus = 'PhonePay')  ");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                cmd.Parameters.AddWithValue("@Type", "Agent");
                DataTable Agent = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in Agent.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    string salestype = dr["salestype"].ToString();
                    newrow["DOE"] = dr["DOE"].ToString();
                    newrow["Receipt"] = dr["Receipt"].ToString();
                    newrow["Name"] = dr["tBranchName"].ToString();
                    double AmountPaid = 0;
                    double.TryParse(dr["AmountPaid"].ToString(), out AmountPaid);
                    newrow["Amount"] = AmountPaid;
                    newrow["Remarks"] = dr["Remarks"].ToString();
                    Report.Rows.Add(newrow);
                }
                cmd = new MySqlCommand("SELECT Sno,DATE_FORMAT(DOE, '%d %b %y') AS DOE,Receiptno,PaymentType as Type, Name, Amount,Remarks  FROM cashcollections WHERE (Branchid = @BranchID) AND (DOE BETWEEN @d1 AND @d2) AND   (CollectionType = 'PhonePay') ORDER BY DOE");
                //cmd = new MySqlCommand("SELECT DATE_FORMAT(cashreceipts.DOE, '%d %b %y') AS DOE, cashreceipts.Sno, cashreceipts.Receipt, cashreceipts.AmountPaid, cashcoll.Name, cashcoll.Amount FROM cashreceipts INNER JOIN (SELECT Branchid, Name, Amount, Remarks, DOE, Receiptno, Agentid, PaymentType, CollectionType, CollectionFrom, CheckStatus, ChequeNo, VarifyDate, ChequeDate, BankName, Sno, VEmpID FROM cashcollections WHERE (DOE BETWEEN @d1 AND @d2)) cashcoll ON cashreceipts.Receipt = cashcoll.Receiptno WHERE (cashreceipts.BranchId = @BranchID) AND (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.ReceivedFrom = @Type) ORDER BY DOE");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                cmd.Parameters.AddWithValue("@Type", "Others");
                DataTable dtOthers = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtOthers.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["DOE"] = dr["DOE"].ToString();
                    newrow["Receipt"] = dr["Receiptno"].ToString();
                    newrow["Type"] = dr["Type"].ToString();
                    newrow["Name"] = dr["Name"].ToString();
                    double AmountPaid = 0;
                    double.TryParse(dr["Amount"].ToString(), out AmountPaid);
                    newrow["Amount"] = AmountPaid;
                    newrow["Remarks"] = dr["Remarks"].ToString();
                    Report.Rows.Add(newrow);
                }
                string Receiptno = "";


                if (fromdate.Month > 3)
                {
                    Receiptno = dtapril.ToString("yy") + dtmarch.ToString("yy");
                }
                else
                {
                    if (fromdate.Month < 3)
                    {
                        Receiptno = dtapril.ToString("yy") + dtmarch.ToString("yy");
                    }
                    else
                    {
                        Receiptno = dtapril.AddYears(-1).ToString("yy") + dtmarch.AddYears(-1).ToString("yy");
                    }
                }
                if (Report.Rows.Count > 0)
                {
                    DataView view = new DataView(Report);
                    dtReport = new DataTable();
                    dtReport.Columns.Add("Voucher Date");
                    dtReport.Columns.Add("Voucher No");
                    dtReport.Columns.Add("Voucher Type");
                    dtReport.Columns.Add("Ledger (Dr)");
                    dtReport.Columns.Add("Ledger (Cr)");
                    dtReport.Columns.Add("Amount");
                    dtReport.Columns.Add("Narration");
                    //DataTable distincttable = view.ToTable(true, "BranchName", "BSno");
                    int i = 1;
                    foreach (DataRow branch in Report.Rows)
                    {
                        DataRow newrow = dtReport.NewRow();
                        newrow["Voucher Date"] = fromdate.ToString("dd-MMM-yyyy");
                        string newreceipt = "0";
                        int countdc = 0;
                        int.TryParse(branch["Receipt"].ToString(), out countdc);
                        if (countdc < 10)
                        {
                            newreceipt = "0000" + countdc;
                        }
                        if (countdc >= 10 && countdc <= 99)
                        {
                            newreceipt = "000" + countdc;
                        }
                        if (countdc >= 99 && countdc <= 999)
                        {
                            newreceipt = "00" + countdc;
                        }
                        if (countdc >= 999 && countdc <= 9999)
                        {
                            newreceipt = "0" + countdc;
                        }
                        if (countdc > 9999)
                        {
                            newreceipt = "" + countdc;
                        }
                        newrow["Voucher No"] = Receiptno + newreceipt;
                        newrow["Voucher Type"] = "Journel Import";
                        newrow["Ledger (Dr)"] = ledger;
                        if (branch["Name"].ToString() == "")
                        {
                        }
                        else
                        {
                            newrow["Ledger (Cr)"] = branch["Name"].ToString();
                            newrow["Amount"] = branch["Amount"].ToString();
                            double invval = 0;
                            string Remarks = branch["Remarks"].ToString();
                            if (Remarks.Length < 25)
                            {
                                newrow["Narration"] = "Being the Bank receipt to  " + branch["Name"].ToString() + " vide Receipt No " + branch["Receipt"].ToString() + ",Receipt Date " + fromdate.ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                            }
                            else
                            {
                                newrow["Narration"] = Remarks + " vide Receipt No " + branch["Receipt"].ToString() + ",Receipt Date " + fromdate.ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                            }
                            dtReport.Rows.Add(newrow);
                            i++;
                        }
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
            else
            {
                Report.Columns.Add("DOE");
                Report.Columns.Add("Ref Receipt");
                Report.Columns.Add("Receipt");
                Report.Columns.Add("Type");
                Report.Columns.Add("Name");
                Report.Columns.Add("Amount").DataType = typeof(Double);
                Report.Columns.Add("Remarks");
                lbl_selfromdate.Text = fromdate.ToString("dd/MM/yyyy");
                lblRoutName.Text = ddlSalesOffice.SelectedItem.Text;
                Session["xporttype"] = "TallyBankReceipts";
                string ledger = "";
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
                cmd = new MySqlCommand("SELECT tbranchname, ladger_dr FROM branchdata WHERE (sno = @BranchID)");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                DataTable dtledger = vdm.SelectQuery(cmd).Tables[0];
                if (dtledger.Rows.Count > 0)
                {
                    ledger = dtledger.Rows[0]["tbranchname"].ToString();
                }

                Session["filename"] = ddlSalesOffice.SelectedItem.Text + " Tally Journel Import" + fromdate.ToString("dd/MM/yyyy");

                //  cmd = new MySqlCommand("SELECT branchdata.salestype,branchdata.tBranchName,collections.ReceiptNo,collections.Sno,DATE_FORMAT(collections.PaidDate, '%d %b %y') AS DOE , collections.AmountPaid, collections.PaymentType FROM collections INNER JOIN branchdata ON collections.Branchid = branchdata.sno INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno WHERE (collections.PaidDate BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @BranchID) AND (collections.PaymentType = 'Cash') AND (collections.AmountPaid > 0) OR (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.PaymentType = 'Cash') AND (branchdata_1.SalesOfficeID = @SOID) AND (collections.AmountPaid > 0)");
                // 01/09/2017
                cmd = new MySqlCommand("SELECT branchdata.SalesType, branchdata.tbranchname,cashreceipts.Remarks, cashreceipts.AmountPaid ,cashreceipts.Receipt,DATE_FORMAT(cashreceipts.DOE, '%d %b %y') AS DOE FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno INNER JOIN cashreceipts ON branchdata.sno = cashreceipts.AgentID WHERE (branchmappingtable.SuperBranch = @BranchID) AND (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.AmountPaid > 0) AND (cashreceipts.PaymentStatus = 'PhonePay')   OR (branchdata_1.SalesOfficeID = @SOID) AND (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.AmountPaid > 0) AND (cashreceipts.PaymentStatus = 'PhonePay')  ");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                cmd.Parameters.AddWithValue("@Type", "Agent");
                DataTable Agent = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in Agent.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    string salestype = dr["salestype"].ToString();
                    newrow["DOE"] = dr["DOE"].ToString();
                    newrow["Receipt"] = dr["Receipt"].ToString();
                    newrow["Name"] = dr["tBranchName"].ToString();
                    double AmountPaid = 0;
                    double.TryParse(dr["AmountPaid"].ToString(), out AmountPaid);
                    newrow["Amount"] = AmountPaid;
                    newrow["Remarks"] = dr["Remarks"].ToString();
                    Report.Rows.Add(newrow);
                }
                cmd = new MySqlCommand("SELECT Sno,DATE_FORMAT(DOE, '%d %b %y') AS DOE,Receiptno,PaymentType as Type, Name, Amount,Remarks  FROM cashcollections WHERE (Branchid = @BranchID) AND (DOE BETWEEN @d1 AND @d2) AND   (CollectionType = 'PhonePay') ORDER BY DOE");
                //cmd = new MySqlCommand("SELECT DATE_FORMAT(cashreceipts.DOE, '%d %b %y') AS DOE, cashreceipts.Sno, cashreceipts.Receipt, cashreceipts.AmountPaid, cashcoll.Name, cashcoll.Amount FROM cashreceipts INNER JOIN (SELECT Branchid, Name, Amount, Remarks, DOE, Receiptno, Agentid, PaymentType, CollectionType, CollectionFrom, CheckStatus, ChequeNo, VarifyDate, ChequeDate, BankName, Sno, VEmpID FROM cashcollections WHERE (DOE BETWEEN @d1 AND @d2)) cashcoll ON cashreceipts.Receipt = cashcoll.Receiptno WHERE (cashreceipts.BranchId = @BranchID) AND (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.ReceivedFrom = @Type) ORDER BY DOE");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                cmd.Parameters.AddWithValue("@Type", "Others");
                DataTable dtOthers = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtOthers.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["DOE"] = dr["DOE"].ToString();
                    newrow["Receipt"] = dr["Receiptno"].ToString();
                    newrow["Type"] = dr["Type"].ToString();
                    newrow["Name"] = dr["Name"].ToString();
                    double AmountPaid = 0;
                    double.TryParse(dr["Amount"].ToString(), out AmountPaid);
                    newrow["Amount"] = AmountPaid;
                    newrow["Remarks"] = dr["Remarks"].ToString();
                    Report.Rows.Add(newrow);
                }
                string Receiptno = "";

                if (fromdate.Month > 3)
                {
                    Receiptno = dtapril.ToString("yy") + dtmarch.ToString("yy");
                }
                else
                {
                    if (fromdate.Month < 3)
                    {
                        Receiptno = dtapril.ToString("yy") + dtmarch.ToString("yy");
                    }
                    else
                    {
                        Receiptno = dtapril.AddYears(-1).ToString("yy") + dtmarch.AddYears(-1).ToString("yy");
                    }
                }
                if (Report.Rows.Count > 0)
                {
                    DataView view = new DataView(Report);
                    dtReport = new DataTable();
                    dtReport.Columns.Add("Voucher Date");
                    dtReport.Columns.Add("Voucher No");
                    dtReport.Columns.Add("Voucher Type");
                    dtReport.Columns.Add("Ledger (Dr)");
                    dtReport.Columns.Add("Ledger (Cr)");
                    dtReport.Columns.Add("Amount");
                    dtReport.Columns.Add("Narration");
                    //DataTable distincttable = view.ToTable(true, "BranchName", "BSno");
                    int i = 1;
                    foreach (DataRow branch in Report.Rows)
                    {
                        DataRow newrow = dtReport.NewRow();
                        newrow["Voucher Date"] = fromdate.ToString("dd-MMM-yyyy");
                        string newreceipt = "0";
                        int countdc = 0;
                        int.TryParse(branch["Receipt"].ToString(), out countdc);
                        if (countdc < 10)
                        {
                            newreceipt = "0000" + countdc;
                        }
                        if (countdc >= 10 && countdc <= 99)
                        {
                            newreceipt = "000" + countdc;
                        }
                        if (countdc >= 99 && countdc <= 999)
                        {
                            newreceipt = "00" + countdc;
                        }
                        if (countdc >= 999 && countdc <= 9999)
                        {
                            newreceipt = "0" + countdc;
                        }
                        if (countdc > 9999)
                        {
                            newreceipt = "" + countdc;
                        }
                        newrow["Voucher No"] = Receiptno + newreceipt;
                        newrow["Voucher Type"] = "Bank Receipts Import";
                        newrow["Ledger (Dr)"] = "Union Bank Of India - 031115010000004 - OD";// ledger;

                        newrow["Ledger (Cr)"] = ledger;// branch["Name"].ToString();
                        newrow["Amount"] = branch["Amount"].ToString();
                        double invval = 0;
                        string Remarks = branch["Remarks"].ToString();
                        if (Remarks.Length < 25)
                        {
                            newrow["Narration"] = "Being the Bank receipt to  " + branch["Name"].ToString() + " vide Receipt No " + branch["Receipt"].ToString() + ",Receipt Date " + fromdate.ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                        }
                        else
                        {
                            newrow["Narration"] = Remarks + " vide Receipt No " + branch["Receipt"].ToString() + ",Receipt Date " + fromdate.ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                        }
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
    //protected void btn_Export_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable("Receipts");
    //        foreach (TableCell cell in grdReports.HeaderRow.Cells)
    //        {
    //            if (cell.Text == "Amount")
    //            {
    //                dt.Columns.Add(cell.Text).DataType = typeof(double);
    //            }
    //            else
    //            {
    //                dt.Columns.Add(cell.Text);
    //            }
    //        }
    //        foreach (GridViewRow row in grdReports.Rows)
    //        {
    //            dt.Rows.Add();
    //            for (int i = 0; i < row.Cells.Count; i++)
    //            {
    //                if (row.Cells[i].Text == "&nbsp;")
    //                {
    //                    row.Cells[i].Text = "0";
    //                }
    //                dt.Rows[dt.Rows.Count - 1][i] = row.Cells[i].Text;
    //            }
    //        }
    //        using (XLWorkbook wb = new XLWorkbook())
    //        {
    //            wb.Worksheets.Add(dt);

    //            Response.Clear();
    //            Response.Buffer = true;
    //            Response.Charset = "";
    //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    //            string FileName = Session["filename"].ToString();
    //            Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");
    //            using (MemoryStream MyMemoryStream = new MemoryStream())
    //            {
    //                wb.SaveAs(MyMemoryStream);
    //                MyMemoryStream.WriteTo(Response.OutputStream);
    //                Response.Flush();
    //                Response.End();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblmsg.Text = ex.Message;
    //    }
    //}
}