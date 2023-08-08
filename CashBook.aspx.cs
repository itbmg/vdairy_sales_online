using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net;
using System.IO;

public partial class CashBook : System.Web.UI.Page
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
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
                FillRouteName();

            }
        }
    }
    void FillRouteName()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Group")
            {
                PPlant.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.flag<>0) ");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlPlant.DataSource = dtRoutedata;
                ddlPlant.DataTextField = "BranchName";
                ddlPlant.DataValueField = "sno";
                ddlPlant.DataBind();
                ddlPlant.Items.Insert(0, new ListItem("Select Plant", "0"));
            }
            else if (Session["salestype"].ToString() == "Plant")
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
                cmd=new MySqlCommand ("SELECT BranchName, sno FROM  branchdata WHERE (sno = @BranchID) and (flag<>0)");
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
    protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        DataTable dtBranch = new DataTable();
        dtBranch.Columns.Add("BranchName");
        dtBranch.Columns.Add("sno");
        cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) and (branchdata.flag<>0) ");
        cmd.Parameters.AddWithValue("@SuperBranch", ddlPlant.SelectedValue);
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
        cmd.Parameters.AddWithValue("@BranchID", ddlPlant.SelectedValue);
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
    DataTable RouteReport = new DataTable();
    DataTable CashPayReport = new DataTable();
    DataTable IOUReport = new DataTable();
    void GetReport()
    {
        try
        {
            pnlfoter.Visible = true;
            pnlHide.Visible = true;
            hidePanel.Visible = false;
            lblSalesOffice.Text = "";
            lblOppBal.Text = "";
            lblpreparedby.Text = "";
            lblmsg.Text = "";
            lblCash.Text = "";
            lblTotalAmout.Text = "";
            lblDiffernce.Text = "";
            vdm = new VehicleDBMgr();
            RouteReport = new DataTable();
            CashPayReport = new DataTable();
            IOUReport = new DataTable();
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
            Session["filename"] = "Cash Book ->" + Session["branchname"].ToString();
            lblSalesOffice.Text = ddlSalesOffice.SelectedItem.Text;
            //string DOE = txtFromdate.Text;
            //DateTime dtDOE = Convert.ToDateTime(DOE);
            //string ChangedTime = dtDOE.ToString("dd/MMM/yyyy");
            lbl_fromDate.Text = fromdate.ToString("dd/MMM/yyyy");
            string BranchID = ddlSalesOffice.SelectedValue;
            DataTable dtCashBook = new DataTable();
            RouteReport.Columns.Add("DispName");
            RouteReport.Columns.Add("Reciept No");
            RouteReport.Columns.Add("Received Amount").DataType = typeof(Double);
            if (BranchID == "172" || BranchID == "7")
            {
            }
            else
            {
                cmd = new MySqlCommand("SELECT dispatch.DispName, tripdata.RecieptNo, tripdata.ReceivedAmount FROM tripdata INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN branchdata ON tripdata.BranchID = branchdata.sno WHERE (tripdata.BranchID = @BranchID) AND (tripdata.Cdate BETWEEN @d1 AND @d2) OR (tripdata.Cdate BETWEEN @d1 AND @d2) AND (branchdata.SalesOfficeID = @BranchID)");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                dtCashBook = vdm.SelectQuery(cmd).Tables[0];
            }
            cmd = new MySqlCommand("SELECT Branchid, AmountPaid FROM collections WHERE (Branchid = @BranchID) AND (PaidDate BETWEEN @d1 AND @d2)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate).AddDays(-1));
            DataTable dtOpp = vdm.SelectQuery(cmd).Tables[0];
            if (dtOpp.Rows.Count > 0)
            {
                //DataRow newClo = RouteReport.NewRow();
                //newClo["DispName"] = "Oppind Balance";
                lblOppBal.Text = dtOpp.Rows[0]["AmountPaid"].ToString();
                //RouteReport.Rows.Add(newClo);
            }
            cmd = new MySqlCommand("SELECT Branchid, AmountPaid,VarifyDate FROM collections WHERE (Branchid = @BranchID) AND (PaidDate BETWEEN @d1 AND @d2)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            DataTable dtclosedtime = vdm.SelectQuery(cmd).Tables[0];
            if (dtclosedtime.Rows.Count > 0)
            {
                //DataRow newClo = RouteReport.NewRow();
                //newClo["DispName"] = "Oppind Balance";
                string VarifyDate = dtclosedtime.Rows[0]["VarifyDate"].ToString();
                DateTime dtVarifyDate = Convert.ToDateTime(VarifyDate);
                string ChangedTime = dtVarifyDate.ToString("dd/MMM/yyyy HH:MM");
                lbl_ClosingDate.Text = ChangedTime;
                //RouteReport.Rows.Add(newClo);
            }
            foreach (DataRow dr in dtCashBook.Rows)
            {
                DataRow newrow = RouteReport.NewRow();
                newrow["DispName"] = dr["DispName"].ToString();
                newrow["Reciept No"] = dr["RecieptNo"].ToString();
                double ReceivedAmount = 0;
                double.TryParse(dr["ReceivedAmount"].ToString(), out ReceivedAmount);
                string Amount = dr["ReceivedAmount"].ToString();
                if (Amount == "0")
                {
                }
                else
                {
                    newrow["Received Amount"] = ReceivedAmount;//.ToString("#,##0.00");
                    RouteReport.Rows.Add(newrow);
                }
            }
            cmd = new MySqlCommand("SELECT Branchid, Amount, Remarks, DOE, Receiptno, Name,PaymentType FROM cashcollections WHERE (Branchid = @Branchid) AND (DOE BETWEEN @d1 AND @d2)  AND (CollectionType = 'Cash')");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            DataTable dtCashCollection = vdm.SelectQuery(cmd).Tables[0];
            if (dtCashCollection.Rows.Count > 0)
            {
                foreach (DataRow dr in dtCashCollection.Rows)
                {
                    DataRow newrow = RouteReport.NewRow();
                    string stat = "";
                    if (dr["PaymentType"].ToString() == "Others")
                    {
                        stat = "Oth";
                    }
                    if (dr["PaymentType"].ToString() == "freezer deposit")
                    {
                        stat = "F.D";

                    }
                    newrow["DispName"] = dr["Name"].ToString() + "(" + stat + ")";
                    newrow["Reciept No"] = dr["Receiptno"].ToString();
                    string Amount = dr["Amount"].ToString();
                     if (Amount == "0")
                     {
                     }
                     else
                     {
                         newrow["Received Amount"] =  dr["Amount"].ToString();
                         RouteReport.Rows.Add(newrow);
                     }
                }
            }
            //cmd = new MySqlCommand("SELECT branchdata.BranchName, collections.AmountPaid, collections.ReceiptNo FROM collections INNER JOIN branchdata ON collections.Branchid = branchdata.sno INNER JOIN branchmappingtable ON collections.Branchid = branchmappingtable.SubBranch WHERE  (branchmappingtable.SuperBranch = @BranchID) AND (collections.tripId IS NULL) AND (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.PaymentType = 'Cash')");
            if (BranchID == "172")
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, collections.AmountPaid, collections.ReceiptNo FROM collections INNER JOIN branchdata ON collections.Branchid = branchdata.sno INNER JOIN empmanage ON collections.EmpID = empmanage.Sno WHERE (collections.tripId IS NULL) AND (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.PaymentType = 'Cash') AND (branchdata.SalesType <> 21) AND (branchdata.SalesType <> 21) AND  (empmanage.Branch = @BranchID)");
            }
            else if (BranchID == "7")
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, cashreceipts.AmountPaid,cashreceipts.PaymentStatus, cashreceipts.Receipt AS ReceiptNo FROM branchdata INNER JOIN cashreceipts ON branchdata.sno = cashreceipts.AgentID WHERE (branchdata.SalesType <> 21) AND (branchdata.SalesType <> 21) AND (cashreceipts.DOE BETWEEN @d1 AND @d2) AND (cashreceipts.BranchId = @BranchID) and (cashreceipts.PaymentStatus = 'Cash')  order by ReceiptNo");
            }
            else
            {
                //cmd = new MySqlCommand("SELECT branchdata.BranchName, collections.AmountPaid, collections.ReceiptNo FROM collections INNER JOIN branchdata ON collections.Branchid = branchdata.sno INNER JOIN branchmappingtable ON collections.Branchid = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @BranchID) AND (collections.tripId IS NULL) AND (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.PaymentType = 'Cash') AND (branchdata.SalesType <> 21) AND (branchdata.SalesType <> 21) ");
                cmd = new MySqlCommand("SELECT branchdata_2.BranchName, collections.AmountPaid, collections.ReceiptNo FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SuperBranch INNER JOIN branchdata branchdata_2 ON branchmappingtable.SubBranch = branchdata_2.sno INNER JOIN collections ON branchdata_2.sno = collections.Branchid WHERE (branchdata_1.SalesOfficeID = @BranchID) AND (collections.tripId IS NULL) AND (collections.PaidDate BETWEEN @d1 AND @d2) AND  (collections.PaymentType = 'Cash') OR (branchdata.sno = @BranchID) AND (collections.tripId IS NULL) AND (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.PaymentType = 'Cash')");
            }
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            DataTable dtAgents = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in dtAgents.Rows)
            {
                DataRow newrow = RouteReport.NewRow();
                string ReceiptNo = dr["ReceiptNo"].ToString();
                if (ReceiptNo == "")
                {
                    ReceiptNo = "0";
                }
                if (ReceiptNo != "0" )
                {
                    newrow["DispName"] = dr["BranchName"].ToString();
                    newrow["Reciept No"] = dr["ReceiptNo"].ToString();
                    string Amount = dr["AmountPaid"].ToString();
                    if (BranchID == "7")
                    {
                        string PaymentStatus = dr["PaymentStatus"].ToString();

                        if (PaymentStatus == "Incentive" || PaymentStatus == "Cheque" || PaymentStatus == "Bank Trans" || PaymentStatus == "Journal Vo") 
                        {
                        }
                        else
                        {
                            if (Amount == "0")
                            {
                            }
                            else
                            {
                                newrow["Received Amount"] =  dr["AmountPaid"].ToString();
                                RouteReport.Rows.Add(newrow);
                            }
                        }
                    }
                    else
                    {
                        if (Amount == "0")
                        {
                        }
                        else
                        {
                            newrow["Received Amount"] =dr["AmountPaid"].ToString();
                            RouteReport.Rows.Add(newrow);
                        }
                    }
                }
            }

            //RouteReport.DefaultView.Sort = "Reciept No ASC";
            //RouteReport.DefaultView.ToTable(true);
            DataView dv = RouteReport.DefaultView;
            dv.Sort = "Reciept No ASC";
            DataTable sortedDT = dv.ToTable();
            cmd = new MySqlCommand("SELECT CashTo as Payments,ApprovedAmount as Amount,VocherID FROM cashpayables WHERE  (BranchID = @BranchID) AND (DOE BETWEEN @d1 AND @d2) AND (Status = 'P') AND ((VoucherType = 'Debit') OR  (VoucherType = 'SalaryPayble') OR  (VoucherType = 'SalaryAdvance')) and (Status <>'C')");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            DataTable dtCashPayble = vdm.SelectQuery(cmd).Tables[0];


            DataRow newvartical = sortedDT.NewRow();
            newvartical["DispName"] = "Total";
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
            DataRow newrowBal = sortedDT.NewRow();
            newrowBal["DispName"] = "Clo Balance";
            double OppBal = 0;
            if (dtOpp.Rows.Count > 0)
            {
                double.TryParse(dtOpp.Rows[0]["AmountPaid"].ToString(), out OppBal);
            }
            double TotalAmount = val + OppBal;
            newrowBal["Received Amount"] = val + OppBal;
            sortedDT.Rows.Add(newrowBal);


            grdRouteCash.DataSource = sortedDT;
            grdRouteCash.DataBind();
            CashPayReport.Columns.Add("Vocher ID");
            CashPayReport.Columns.Add("Payments");
            CashPayReport.Columns.Add("Amount").DataType = typeof(Double);
            //cmd = new MySqlCommand("SELECT Sno, BranchID, CashTo, DOE, VocherID, Remarks, SUM(ApprovedAmount) AS Amount FROM  cashpayables WHERE (BranchID = @BranchID)  AND (VoucherType = 'Credit')GROUP BY CashTo ORDER BY CashTo");
            cmd = new MySqlCommand("SELECT cashpayables.Sno, cashpayables.BranchID, cashpayables.CashTo,subpayable.HeadSno, cashpayables.DOE, cashpayables.VocherID, cashpayables.Remarks, SUM(cashpayables.ApprovedAmount) AS Amount, accountheads.HeadName FROM cashpayables INNER JOIN subpayable ON cashpayables.Sno = subpayable.RefNo INNER JOIN accountheads ON subpayable.HeadSno = accountheads.Sno WHERE (cashpayables.BranchID = @BranchID) AND (cashpayables.Status='P') AND (cashpayables.VoucherType = 'Credit') GROUP BY accountheads.HeadName ORDER BY accountheads.HeadName");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            DataTable dtCredit = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in dtCashPayble.Rows)
            {
                DataRow newrow = CashPayReport.NewRow();
                newrow["Vocher ID"] = dr["VocherID"].ToString();
                newrow["Payments"] = dr["Payments"].ToString();
                string Amount = dr["Amount"].ToString();
                if (Amount == "0")
                {
                }
                else
                {
                    newrow["Amount"] = dr["Amount"].ToString();
                    CashPayReport.Rows.Add(newrow);
                }
            }
            DataRow newCash = CashPayReport.NewRow();
            newCash["Payments"] = "Total";
            double valnewCash = 0.0;
            foreach (DataColumn dc in CashPayReport.Columns)
            {
                if (dc.DataType == typeof(Double))
                {
                    valnewCash = 0.0;
                    double.TryParse(CashPayReport.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out valnewCash);
                    newCash[dc.ToString()] = valnewCash;
                }
            }
            CashPayReport.Rows.Add(newCash);
            DataTable dtIoutoday = new DataTable();
            dtIoutoday.Columns.Add("Vocher ID");
            dtIoutoday.Columns.Add("Today IOU");
            dtIoutoday.Columns.Add("Amount").DataType = typeof(Double);
            cmd = new MySqlCommand("SELECT CashTo as Payments,ApprovedAmount as Amount,VocherID FROM cashpayables WHERE  (BranchID = @BranchID) AND (DOE BETWEEN @d1 AND @d2) AND (Status = 'P') AND (VoucherType = 'Due') and (Status <>'C')");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            DataTable dtTodayIOU = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in dtTodayIOU.Rows)
            {
                DataRow newrow = dtIoutoday.NewRow();
                newrow["Vocher ID"] = dr["VocherID"].ToString();
                newrow["Today IOU"] = dr["Payments"].ToString();
                string Amount = dr["Amount"].ToString();
                if (Amount == "0")
                {
                }
                else
                {
                    newrow["Amount"] = dr["Amount"].ToString();
                    dtIoutoday.Rows.Add(newrow);
                }
            }
            grdTodayIOU.DataSource = dtIoutoday;
            grdTodayIOU.DataBind();
            double DebitCash = 0;
            DataRow newDebitBal = CashPayReport.NewRow();
            newDebitBal["Payments"] = "Closing Balance";
            newDebitBal["Amount"] = TotalAmount - valnewCash;
            DebitCash = TotalAmount - valnewCash;
            CashPayReport.Rows.Add(newDebitBal);
            lblhidden.Text = DebitCash.ToString();
            grdCashPayable.DataSource = CashPayReport;
            grdCashPayable.DataBind();
            DataTable dtIOU = new DataTable();
            IOUReport.Columns.Add("Sno");
            IOUReport.Columns.Add("IOU");
            IOUReport.Columns.Add("Amount").DataType = typeof(Double);
            cmd = new MySqlCommand("SELECT IOU as onNameof, Amount  FROM ioutable WHERE (BranchID = @BranchID) AND (DOE BETWEEN @d1 AND @d2) ");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            dtIOU = vdm.SelectQuery(cmd).Tables[0];
            if (dtIOU.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtIOU.Rows)
                {
                    DataRow newrow = IOUReport.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["IOU"] = dr["onNameof"].ToString();
                    string Amount = dr["Amount"].ToString();
                    if (Amount == "0")
                    {
                    }
                    else
                    {
                        newrow["Amount"] = dr["Amount"].ToString();
                        IOUReport.Rows.Add(newrow);
                    }
                }
            }
            else
            {
                cmd = new MySqlCommand("SELECT BranchID FROM  Collections WHERE (BranchId = @BranchId) AND (PaidDate BETWEEN @d1 AND @d2)");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                DataTable dtCol = vdm.SelectQuery(cmd).Tables[0];
                if (dtCol.Rows.Count > 0)
                {

                }
                else
                {
                    cmd = new MySqlCommand("SELECT cashpayables.onNameof,cashpayables.CashTo, SUM(cashpayables.ApprovedAmount) AS Amount, subpayable.HeadSno, accountheads.HeadName FROM cashpayables INNER JOIN subpayable ON cashpayables.Sno = subpayable.RefNo INNER JOIN accountheads ON subpayable.HeadSno = accountheads.Sno WHERE (cashpayables.BranchID = @BranchID) AND (cashpayables.VoucherType = 'Due') AND (cashpayables.Status<> 'C') AND (cashpayables.Status='P') GROUP BY  accountheads.HeadName ORDER BY accountheads.HeadName");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    dtIOU = vdm.SelectQuery(cmd).Tables[0];
                    foreach (DataRow dr in dtIOU.Rows)
                    {
                        DataRow newrow = IOUReport.NewRow();
                        newrow["Sno"] = dr["HeadSno"].ToString();
                        newrow["IOU"] = dr["CashTo"].ToString();
                        newrow["Amount"] = dr["Amount"].ToString();
                        IOUReport.Rows.Add(newrow);
                    }
                }
                foreach (DataRow dr in IOUReport.Rows)
                {
                    foreach (DataRow drcredit in dtCredit.Rows)
                    {
                        string sno = dr["Sno"].ToString();
                        string creditsno = drcredit["HeadSno"].ToString();
                        if (dr["Sno"].ToString() == drcredit["HeadSno"].ToString())
                        {
                            if (sno == "1766")
                            {

                            }
                            double CAmount = 0;
                            double.TryParse(drcredit["Amount"].ToString(), out CAmount);
                            double Amount = 0;
                            double.TryParse(dr["Amount"].ToString(), out Amount);
                            double TAmount = Amount - CAmount;
                            dr["Amount"] = TAmount;
                        }
                    }
                }
                DataTable Report = new DataTable();
                Report.Columns.Add("Sno");
                Report.Columns.Add("IOU");
                Report.Columns.Add("Amount").DataType = typeof(Double);
                foreach (DataRow dr in IOUReport.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = dr["Sno"].ToString();
                    newrow["IOU"] = dr["IOU"].ToString();
                    string Amount = dr["Amount"].ToString();
                    if (Amount == "0")
                    {
                    }
                    else
                    {
                        newrow["Amount"] = dr["Amount"].ToString();
                        Report.Rows.Add(newrow);
                    }
                }
                IOUReport = Report;
            }

            DataRow newIOUCash = IOUReport.NewRow();
            newIOUCash["IOU"] = "IOU'S Total";
            double valIOUCash = 0.0;
            foreach (DataColumn dc in IOUReport.Columns)
            {
                if (dc.DataType == typeof(Double))
                {
                    valIOUCash = 0.0;
                    double.TryParse(IOUReport.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out valIOUCash);
                    newIOUCash[dc.ToString()] = valIOUCash;
                }
            }
            lblIou.Text = valIOUCash.ToString();
            IOUReport.Rows.Add(newIOUCash);
            grdDue.DataSource = IOUReport;
            grdDue.DataBind();
            Session["IOUReport"] = IOUReport;

            double TotNetAmount = 0;
            if (BranchID == "7")
            {
                //DiffPanel.Visible = false;
                //hidePanel.Visible = false;

                //cmd = new MySqlCommand("  SELECT  sno,DATE_FORMAT(doe, '%d %b %y') AS EntryDate, paymenttype, receiptno, voucherid, branchid, amount,  remarks FROM  zerocashpaybles WHERE (branchid = @BranchID)  AND (doe BETWEEN @d1 AND @d2) AND (paymenttype is NULL)  ORDER BY DOE");
                //cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                //cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                //DataTable dtZeroOpp = vdm.SelectQuery(cmd).Tables[0];
                //double TotalZeroOpp = 0;
                //double ZeroOpp = 0;
                //if (dtZeroOpp.Rows.Count > 0)
                //{
                //    string OppAmount = dtZeroOpp.Rows[0]["amount"].ToString();
                //    double.TryParse(OppAmount, out ZeroOpp);
                //}
                //TotalZeroOpp = OppBal + ZeroOpp;
                //TotalZeroOpp = Math.Round(TotalZeroOpp, 0);
                //lblZeroOppBal.Text = TotalZeroOpp.ToString();
                //cmd = new MySqlCommand("  SELECT  sno,DATE_FORMAT(doe, '%d %b %y') AS EntryDate, paymenttype, receiptno, voucherid, branchid, Sum(amount) as amount,  remarks,Name FROM  zerocashpaybles WHERE (branchid = @BranchID) AND (paymenttype = @Type) AND (doe BETWEEN @d1 AND @d2) Group By DATE(doe) ORDER BY DOE");
                //cmd.Parameters.AddWithValue("@Type", "Receipt");
                //cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                //cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                //DataTable dtReceipt = vdm.SelectQuery(cmd).Tables[0];
                //double TotalReceipt = 0;
                //double ZeroReceipt = 0;
                //if (dtReceipt.Rows.Count > 0)
                //{
                //    string ReceiptAmount = dtReceipt.Rows[0]["amount"].ToString();
                //    double.TryParse(ReceiptAmount, out ZeroReceipt);
                //}
                //TotalReceipt = val + ZeroReceipt;
                //TotalReceipt = Math.Round(TotalReceipt, 0);
                //lblZeroReceipts.Text = TotalReceipt.ToString();

                //cmd = new MySqlCommand("  SELECT  sno,DATE_FORMAT(doe, '%d %b %y') AS EntryDate, paymenttype, receiptno, voucherid, branchid, Sum(amount) as amount,  remarks,Name FROM  zerocashpaybles WHERE (branchid = @BranchID) AND (paymenttype = @Type) AND (doe BETWEEN @d1 AND @d2) Group By DATE(doe) ORDER BY DOE");
                //cmd.Parameters.AddWithValue("@Type", "Payment");
                //cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                //cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                //DataTable dtPayment = vdm.SelectQuery(cmd).Tables[0];
                //double TotalPayMent = 0;
                //double ZeroPayMent = 0;
                //if (dtPayment.Rows.Count > 0)
                //{
                //    string PayMentAmount = dtPayment.Rows[0]["amount"].ToString();
                //    double.TryParse(PayMentAmount, out ZeroPayMent);
                //}
                //TotalPayMent = valnewCash + ZeroPayMent;
                //TotalPayMent = Math.Round(TotalPayMent, 0);
                //lblZeroPayments.Text = TotalPayMent.ToString();
                //double TotAmount = 0;
                //TotAmount = TotalZeroOpp + TotalReceipt;
                //double TotCashAmount = 0;
                //TotCashAmount = TotAmount - TotalPayMent;

                //TotNetAmount = TotCashAmount - valIOUCash;
            }
            //cmd = new MySqlCommand("SELECT Branchid, AmountPaid,Denominations,EmpID FROM collections WHERE (Branchid = @BranchID) AND (PaidDate BETWEEN @d1 AND @d2)");
            cmd = new MySqlCommand("SELECT collections.Branchid, collections.AmountPaid, collections.Denominations, collections.VEmpID, collections.EmpID, empmanage.EmpName FROM collections INNER JOIN empmanage ON collections.EmpID = empmanage.Sno WHERE (collections.Branchid = @BranchID) AND (collections.PaidDate BETWEEN @d1 AND @d2)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            DataTable dtClo = vdm.SelectQuery(cmd).Tables[0];
            if (dtClo.Rows.Count > 0)
            {

                hidePanel.Visible = false;
                DiffPanel.Visible = true;
                //string llCash = denominationtotal.ToString();

                panelGrid.Visible = true;
                PanelDen.Visible = false;
                DataTable dtDenom = new DataTable();
                dtDenom.Columns.Add("Cash");
                dtDenom.Columns.Add("Count");
                dtDenom.Columns.Add("Amount");
                string strDenomin = dtClo.Rows[0]["Denominations"].ToString();
                double denominationtotal = 0;
                foreach (string str in strDenomin.Split('+'))
                {
                    if (str != "")
                    {
                        DataRow newDeno = dtDenom.NewRow();
                        string[] price = str.Split('x');
                        if (price.Length > 1)
                        {
                            newDeno["Cash"] = price[0];
                            newDeno["Count"] = price[1];
                            float denamount = 0;
                            float.TryParse(price[0], out denamount);
                            float DencAmount = 0;
                            float.TryParse(price[1], out DencAmount);
                            newDeno["Amount"] = Convert.ToDecimal(denamount * DencAmount).ToString("#,##0.00");
                            denominationtotal += denamount * DencAmount;
                            dtDenom.Rows.Add(newDeno);
                        }
                    }
                }
                DataRow newDenoTotal = dtDenom.NewRow();
                newDenoTotal["Cash"] = "Total";
                newDenoTotal["Amount"] = denominationtotal;
                dtDenom.Rows.Add(newDenoTotal);
                string IOU = lblIou.Text;
                double DIOU = 0;
                double.TryParse(IOU, out DIOU);
                double TotalCash = 0;
                TotalCash = denominationtotal + DIOU;
                lblTotalAmout.Text = TotalCash.ToString();
                double Differnce = 0;
                //Differnce = DebitCash - TotalCash;
                Differnce = TotalCash - DebitCash;
                lblDiffernce.Text = Differnce.ToString();
                lblpreparedby.Text = dtClo.Rows[0]["EmpName"].ToString();
                //OppBal


                double Zerodiff = 0;
                Zerodiff = TotNetAmount - denominationtotal;
                Zerodiff = Math.Round(Zerodiff, 0);
                lblZeroDiffence.Text = Zerodiff.ToString();
                grdDenomination.DataSource = dtDenom;
                grdDenomination.DataBind();
                lblCash.Text = denominationtotal.ToString();
            }
            else
            {
                PanelDen.Visible = true;
                panelGrid.Visible = false;
                DiffPanel.Visible = true;
                string IOU = lblIou.Text;
                double DIOU = 0;
                double.TryParse(IOU, out DIOU);
                double dif = 0;
                double totdif = 0;
                totdif = DIOU + dif;
                lblDiffernce.Text = totdif.ToString();

            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string totIOU = lblIou.Text;
            //string DenCash = Session["Cash"].ToString();
            double IOU = 0;
            double.TryParse(totIOU, out IOU);
            //double Cash = 0;
            //double.TryParse(DenCash, out Cash);
            // double TotalAmount = 0;
            double Totalclosing = 0;
            //TotalAmount = Cash + IOU;
            //double diffamount = 0;
            //double.TryParse(lblDiffernce.Text, out diffamount);
            double.TryParse(lblhidden.Text, out Totalclosing);

            DataTable dt = (DataTable)Session["IOUReport"];
            lblmsg.Text = "";
            DateTime fromdate = new DateTime();
            string[] datestrig = txtFromdate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            fromdate = fromdate;
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            string DenominationString = Session["DenominationString"].ToString();
            DenominationString = DenominationString.Trim();
            cmd = new MySqlCommand("SELECT BranchID FROM  Collections WHERE (BranchId = @BranchId) AND (PaidDate BETWEEN @d1 AND @d2)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            DataTable dtCol = vdm.SelectQuery(cmd).Tables[0];
            if (dtCol.Rows.Count > 0)
            {
                lblmsg.Text = "Cash Book already closed";

            }
            else
            {
               
                cmd = new MySqlCommand("SELECT cashpayables.Sno, cashpayables.BranchID, cashpayables.CashTo,subpayable.HeadSno, cashpayables.DOE, cashpayables.VocherID, cashpayables.Remarks, SUM(cashpayables.ApprovedAmount) AS Amount, accountheads.HeadName FROM cashpayables INNER JOIN subpayable ON cashpayables.Sno = subpayable.RefNo INNER JOIN accountheads ON subpayable.HeadSno = accountheads.Sno WHERE (cashpayables.BranchID = @BranchID) AND (cashpayables.Status='P') AND (cashpayables.VoucherType = 'Credit') GROUP BY accountheads.Sno ORDER BY accountheads.HeadName");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                DataTable dtCredit = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT cashpayables.onNameof,cashpayables.CashTo, SUM(cashpayables.ApprovedAmount) AS Amount, subpayable.HeadSno, accountheads.HeadName FROM cashpayables INNER JOIN subpayable ON cashpayables.Sno = subpayable.RefNo INNER JOIN accountheads ON subpayable.HeadSno = accountheads.Sno WHERE (cashpayables.BranchID = @BranchID) AND (cashpayables.VoucherType = 'Due') AND (cashpayables.Status<> 'C') AND (cashpayables.Status='P') GROUP BY  accountheads.HeadName ORDER BY accountheads.HeadName");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                DataTable dtDebit = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtDebit.Rows)
                {
                    string IouName = dr["HeadName"].ToString();
                    double iouamtdebit = 0;
                    double iouamtcredit = 0;
                    double TotIouBal = 0;
                    double.TryParse(dr["Amount"].ToString(), out iouamtdebit);
                    foreach (DataRow drcredit in dtCredit.Select("HeadSno='" + dr["HeadSno"].ToString() + "'"))
                    {
                        double.TryParse(drcredit["Amount"].ToString(), out iouamtcredit);
                    }
                    TotIouBal = iouamtdebit - iouamtcredit;
                    if (TotIouBal == 0)
                    {
                    }
                    else
                    {
                        cmd = new MySqlCommand("Insert into ioutable (BranchID,IOU,Amount,DOE) values(@BranchID,@IOU,@Amount,@DOE)");
                        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                        cmd.Parameters.AddWithValue("@Amount", TotIouBal);
                        cmd.Parameters.AddWithValue("@IOU", IouName);
                        cmd.Parameters.AddWithValue("@DOE", fromdate);
                        vdm.insert(cmd);
                    }
                }
                cmd = new MySqlCommand("Insert into Collections (BranchID,AmountPaid,UserData_sno,PaidDate,PaymentType,Denominations,EmpID,VarifyDate) values(@BranchID,@AmountPaid,@UserData_sno,@PaidDate,@PaymentType,@Denominations,@EmpID,@VarifyDate)");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@AmountPaid", Math.Round(Totalclosing, 2));
                cmd.Parameters.AddWithValue("@Denominations", DenominationString);
                cmd.Parameters.AddWithValue("@UserData_sno", "1");
                cmd.Parameters.AddWithValue("@PaidDate", fromdate);
                cmd.Parameters.AddWithValue("@VarifyDate", ServerDateCurrentdate);
                cmd.Parameters.AddWithValue("@PaymentType", "Cash");
                cmd.Parameters.AddWithValue("@EmpID", Session["UserSno"].ToString());
                vdm.insert(cmd);
                if (ddlSalesOffice.SelectedValue == "172")
                {

                    string strDenomin = DenominationString;
                    double denominationtotal = 0;
                    foreach (string str in strDenomin.Split('+'))
                    {
                        if (str != "")
                        {
                            string[] price = str.Split('x');
                            if (price.Length > 1)
                            {
                                float denamount = 0;
                                float.TryParse(price[0], out denamount);
                                float DencAmount = 0;
                                float.TryParse(price[1], out DencAmount);
                                denominationtotal += denamount * DencAmount;
                            }
                        }
                    }
                    cmd = new MySqlCommand("SELECT  DispNo, PhoneNumber, Sno, EmpID, EmailID, MsgType, name FROM mobilenotable where MsgType=@MsgType");
                    cmd.Parameters.AddWithValue("@MsgType", "3");
                    DataTable dtmobileno = vdm.SelectQuery(cmd).Tables[0];
                    if (dtmobileno.Rows.Count > 0)
                    {
                        foreach (DataRow drmobile in dtmobileno.Rows)
                        {
                            // string Date = fromdate;
                            phonenumber = drmobile["PhoneNumber"].ToString();
                            WebClient client = new WebClient();
                            string strdate = fromdate.ToString("dd/MMM");
                            string message = "";
                            if (Session["TitleName"].ToString() == "Sri Vyshnavi Dairy Specialities (P) Ltd")
                            {
                                string baseurl = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + phonenumber + "&message=%20" + ddlSalesOffice.SelectedItem.Text + "%20CashBook%20Cash In Hand%20Amount%20for%20The%20Date%20Of%20%20" + strdate + "%20Amount%20is =" + denominationtotal + "&sender=VYSNVI&type=1&route=2"; 
                               // string baseurl = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VSALES&to=" + phonenumber + "&msg=%20" + ddlSalesOffice.SelectedItem.Text + "%20CashBook%20Cash In Hand%20Amount%20for%20The%20Date%20Of%20%20" + strdate + "%20Amount%20is =" + denominationtotal + "&type=1";
                                message = "" + ddlSalesOffice.SelectedItem.Text + " Closing Amount for The Date Of" + strdate + "ClosingAmoount is =" + denominationtotal + "";
                                Stream data = client.OpenRead(baseurl);
                                StreamReader reader = new StreamReader(data);
                                string ResponseID = reader.ReadToEnd();
                                data.Close();
                                reader.Close();
                            }
                            else
                            {
                                string baseurl = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + phonenumber + "&message=Dear%20" + ddlSalesOffice.SelectedItem.Text + "%20CashBook%20Closing%20Amount%20for%20The%20Date%20Of%20%20" + strdate + "%20Amount%20is =" + Math.Round(Totalclosing, 2) + "&sender=VYSNVI&type=1&route=2"; 
                               // string baseurl = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VFWYRA&to=" + phonenumber + "&msg=Dear%20" + ddlSalesOffice.SelectedItem.Text + "%20CashBook%20Closing%20Amount%20for%20The%20Date%20Of%20%20" + strdate + "%20Amount%20is =" + Math.Round(Totalclosing, 2) + "&type=1";
                                message = "" + ddlSalesOffice.SelectedItem.Text + "Your Incentive Amount Credeted for The Month Of" + strdate + "Amount is =" + Math.Round(Totalclosing, 2) + "";
                                Stream data = client.OpenRead(baseurl);
                                StreamReader reader = new StreamReader(data);
                                string ResponseID = reader.ReadToEnd();
                                data.Close();
                                reader.Close();
                            }
                            //cmd = new MySqlCommand("insert into smsinfo (agentid,branchid,mainbranch,msg,mobileno,msgtype,agentname,doe) values (@agentid,@branchid,@mainbranch,@msg,@mobileno,@msgtype,@agentname,@doe)");
                            //cmd.Parameters.AddWithValue("@agentid", BranchID);
                            //cmd.Parameters.AddWithValue("@branchid", soid);
                            //cmd.Parameters.AddWithValue("@mainbranch", Session["SuperBranch"].ToString());
                            //cmd.Parameters.AddWithValue("@msg", message);
                            //cmd.Parameters.AddWithValue("@mobileno", phonenumber);
                            //cmd.Parameters.AddWithValue("@msgtype", "CashBook");
                            //cmd.Parameters.AddWithValue("@agentname", BranchName);
                            //cmd.Parameters.AddWithValue("@doe", ServerDateCurrentdate);
                            //vdm.insert(cmd);

                        }
                    }

                }

                lblmsg.Text = "Cash Book saved successfully";
                GetReport();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grdDue, "Select$" + e.Row.RowIndex);
            e.Row.Attributes["style"] = "cursor:pointer";
        }
    }
    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int index = grdDue.SelectedRow.RowIndex;
        string headsno = grdDue.SelectedRow.Cells[0].Text;
        string name = grdDue.SelectedRow.Cells[1].Text;
        string country = grdDue.SelectedRow.Cells[2].Text;
        DateTime fromdate = new DateTime();
        string[] datestrig = txtFromdate.Text.Split(' ');
        if (datestrig.Length > 1)
        {
            if (datestrig[0].Split('-').Length > 0)
            {
                string[] dates = datestrig[0].Split('-');
                string[] times = datestrig[1].Split(':');
                fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
            }
        }
        //string message = "Row Index: " + index + "\\nName: " + name + "\\nCountry: " + country;
        try
        {
            vdm = new VehicleDBMgr();
            DataTable Report = new DataTable();

            cmd = new MySqlCommand("SELECT BranchID FROM  Collections WHERE (BranchId = @BranchId) AND (PaidDate BETWEEN @d1 AND @d2)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            DataTable dtCol = vdm.SelectQuery(cmd).Tables[0];
            if (dtCol.Rows.Count > 0)
            {
                lblmsg.Text = "Please Select Details On Current Day Cash Book";

            }
            else
            {
                //cmd = new MySqlCommand("SELECT DATE_FORMAT(cashpayables.DOE, '%d %b %y') AS EntryDate, cashpayables.VocherID, accountheads.HeadName,cashpayables.VoucherType, cashpayables.Amount, cashpayables.ApprovedAmount as ApprovedAmount FROM accountheads INNER JOIN subpayable ON accountheads.Sno = subpayable.HeadSno INNER JOIN cashpayables ON subpayable.RefNo = cashpayables.Sno WHERE (cashpayables.BranchID = @BranchID)  AND (subpayable.HeadSno = @HeadSno) and (cashpayables.Status=@Status)  ORDER BY cashpayables.DOE");
                cmd = new MySqlCommand("SELECT DATE_FORMAT(cashpayables.DOE, '%d %b %y') AS EntryDate, cashpayables.VocherID, accountheads.HeadName, cashpayables.VoucherType,cashpayables.Amount, cashpayables.ApprovedAmount FROM accountheads INNER JOIN subpayable ON accountheads.Sno = subpayable.HeadSno INNER JOIN cashpayables ON subpayable.RefNo = cashpayables.Sno WHERE (subpayable.HeadSno = @HeadSno) AND (cashpayables.Status = @Status) AND ((cashpayables.VoucherType<>'Debit') OR (cashpayables.VoucherType<>'SalaryAdvance') OR (cashpayables.VoucherType<>'SalaryPayble'))  ORDER BY cashpayables.DOE");
                cmd.Parameters.AddWithValue("@Status", 'P');
                //cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@HeadSno", headsno);
                DataTable dtCredit = vdm.SelectQuery(cmd).Tables[0];
                Report.Columns.Add("Date");
                Report.Columns.Add("VoucherID");
                Report.Columns.Add("AccountName");
                Report.Columns.Add("IOUAmount").DataType = typeof(Double);
                Report.Columns.Add("CreditAmount").DataType = typeof(Double);
                double DueAmount = 0;
                double CreditAmount = 0;
                if (dtCredit.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCredit.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Date"] = dr["EntryDate"].ToString();
                        newrow["VoucherID"] = dr["VocherID"].ToString();
                        newrow["AccountName"] = dr["HeadName"].ToString();
                        string VoucherType = dr["VoucherType"].ToString();
                        if (VoucherType == "Due")
                        {
                            double ReceivedAmount = 0;
                            double.TryParse(dr["Amount"].ToString(), out ReceivedAmount);
                            newrow["IOUAmount"] = ReceivedAmount;
                            DueAmount += ReceivedAmount;
                        }
                        if (VoucherType == "Credit")
                        {
                            double ApprovedAmount = 0;
                            double.TryParse(dr["ApprovedAmount"].ToString(), out ApprovedAmount);
                            newrow["CreditAmount"] = ApprovedAmount;
                            CreditAmount += ApprovedAmount;
                        }
                        Report.Rows.Add(newrow);
                    }
                }
                double Amount = 0;
                Amount = DueAmount - CreditAmount;
                //lblmsg.Text = "Due Amount: " + Amount.ToString();
                DataRow newrowbalance = Report.NewRow();
                newrowbalance["AccountName"] = "Due Amount: ";
                newrowbalance["IOUAmount"] = Amount;
                Report.Rows.Add(newrowbalance);
                GrdProducts.DataSource = Report;
                GrdProducts.DataBind();
                ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "PopupOpen();", true);
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }

    public string phonenumber { get; set; }
}