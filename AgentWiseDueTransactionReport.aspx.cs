using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Text;
using ClosedXML.Excel;
using System.Configuration;
using System.Net;
using System.Drawing;

public partial class AgentWiseDueTransactionReport : System.Web.UI.Page
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
                FillRouteName();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
               // txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    void FillRouteName()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Plant")
            {
                PBranch.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                cmd.Parameters.AddWithValue("@SalesType", "21");
                cmd.Parameters.AddWithValue("@SalesType1", "26");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
            }
            else
            {
                PBranch.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.sno = @BranchID)");
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
    string routeid = "";
    string routeitype = "";
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            lblmessage.Text = "";
            Session["RouteName"] = ddlSalesOffice.SelectedItem.Text;
            lblRouteName.Text = ddlSalesOffice.SelectedItem.Text;
            vdm = new VehicleDBMgr();
            DataTable Report = new DataTable();
            string[] datestrig = txtdate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            //DateTime Todate = DateTime.Now;
            //string[] Todatestrig = txtTodate.Text.Split(' ');
            //if (Todatestrig.Length > 1)
            //{
            //    if (Todatestrig[0].Split('-').Length > 0)
            //    {
            //        string[] dates = Todatestrig[0].Split('-');
            //        string[] times = Todatestrig[1].Split(':');
            //        Todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
            //    }
            //}
            lblDate.Text = fromdate.ToString("dd/MMM/yyyy");
            string BranchID = ddlSalesOffice.SelectedValue;
            if (BranchID == "572")
            {
                BranchID = "158";
            }
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, branchroutes.Sno AS routesno, branchroutes.RouteName FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN branchroutes ON branchdata.sno = branchroutes.BranchID WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) AND (branchroutes.flag <> 0) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) AND (branchroutes.flag <> 0) ORDER BY branchdata.sno");
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            DataTable dtroutes = vdm.SelectQuery(cmd).Tables[0];
            if (ddlreporttype.SelectedItem.Text == "Amount Balance")
            {
                //cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, SUM(indents_subtable.DeliveryQty) AS saleQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue,modifiedroutes.Sno AS routesno, modifidroutssubtab.BranchID, branchdata_2.BranchName FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT IndentNo, I_date, Branch_id FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indt ON modifidroutssubtab.BranchID = indt.Branch_id INNER JOIN indents_subtable ON indt.IndentNo = indents_subtable.IndentNo INNER JOIN branchdata branchdata_2 ON modifidroutssubtab.BranchID = branchdata_2.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY branchdata.sno, routesno");
                cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, SUM(indents_subtable.DeliveryQty) AS saleQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue,modifiedroutes.Sno AS routesno, modifidroutssubtab.BranchID, branchdata_2.BranchName, branchdata_2.flag FROM branchdata branchdata_2 RIGHT OUTER JOIN branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo ON branchdata_2.sno = modifidroutssubtab.BranchID LEFT OUTER JOIN indents_subtable INNER JOIN (SELECT IndentNo, I_date, Branch_id FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indt ON indents_subtable.IndentNo = indt.IndentNo ON modifidroutssubtab.BranchID = indt.Branch_id WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY branchdata.sno, routesno");
                cmd.Parameters.AddWithValue("@SOID", BranchID);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtroutecollection = vdm.SelectQuery(cmd).Tables[0];
                DataTable dtrouteamount = new DataTable();
                DataTable dtsalescollection = new DataTable();
                //cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routesno, SUM(colltion.AmountPaid) AS amtpaid FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT Branchid, AmountPaid, PaidDate FROM collections WHERE (PaymentType = 'CASH') AND (PaidDate BETWEEN @d1 AND @d2)) colltion ON modifidroutssubtab.BranchID = colltion.Branchid WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifiedroutes.Sno ORDER BY branchdata.sno, routesno");
                if (BranchID == "306")
                {
                    cmd = new MySqlCommand("SELECT branchdata.tbranchname AS BranchName,branchdata.sno AS BranchID,cashreceipts.Remarks, cashreceipts.Receipt, DATE_FORMAT(tripdata.I_Date, '%d %b %y') AS DOE, SUM(cashreceipts.AmountPaid) AS amtpaid, cashreceipts.PaymentStatus, tripdata.Sno FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN cashreceipts ON branchdata.sno = cashreceipts.AgentID INNER JOIN tripdata ON cashreceipts.Tripid = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @BranchID) AND (cashreceipts.PaymentStatus = 'Cash') AND (cashreceipts.AmountPaid > 0) Group by branchdata.tbranchname ORDER BY branchdata.sno");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate).AddDays(-1));
                    cmd.Parameters.AddWithValue("@Type", "Agent");
                    dtrouteamount = vdm.SelectQuery(cmd).Tables[0];

                    cmd = new MySqlCommand("SELECT branchdata.tBranchName,branchdata.sno AS BranchID,collections.receiptno as rec,collections.Remarks, collections.Sno as ReceiptNo,DATE_FORMAT(collections.PaidDate, '%d %b %y') AS DOE , collections.AmountPaid, collections.PaymentType FROM collections INNER JOIN branchdata ON collections.Branchid = branchdata.sno INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno WHERE (collections.PaidDate BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @BranchID) AND (collections.PaymentType = 'Cash') AND (collections.AmountPaid > 0) AND (collections.tripid is NULL) OR (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.PaymentType = 'Cash') AND (branchdata_1.SalesOfficeID = @SOID) AND (collections.tripid is NULL) AND (collections.AmountPaid > 0)");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(1));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate).AddDays(1));
                    cmd.Parameters.AddWithValue("@Type", "Agent");
                    dtsalescollection = vdm.SelectQuery(cmd).Tables[0];
                    //dtrouteamount = vdm.SelectQuery(cmd).Tables[0];
                }
                else
                {
                    cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routesno, SUM(colltion.AmountPaid) AS amtpaid FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT Branchid, AmountPaid, PaidDate FROM collections WHERE (PaymentType <> 'Cheque') AND (PaidDate BETWEEN @d1 AND @d2)) colltion ON modifidroutssubtab.BranchID = colltion.Branchid WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY branchdata.sno");
                    cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                    cmd.Parameters.AddWithValue("@SOID", BranchID);
                    cmd.Parameters.AddWithValue("@BranchID", BranchID);
                    dtrouteamount = vdm.SelectQuery(cmd).Tables[0];
                }
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routesno, SUM(colltion.AmountPaid) AS amtpaid FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT Branchid, AmountPaid, PaidDate FROM collections WHERE (PaymentType = 'Cheque') AND (VarifyDate BETWEEN @d1 AND @d2) AND (CheckStatus = 'V')) colltion ON modifidroutssubtab.BranchID = colltion.Branchid WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY modifidroutssubtab.BranchID");
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                cmd.Parameters.AddWithValue("@SOID", BranchID);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                DataTable dtrouteChequeamount = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT Branchid, AmountPaid, Remarks, DATE_FORMAT(PaidDate, '%d/%b/%y') AS PDate, PayTime, EmpID, ReceiptNo, VarifyDate, TransactionType, AmountDebited, DiffAmount, SalesOfficeID, Status FROM collections WHERE (SalesOfficeID = @BranchID) AND (TransactionType = @type) AND (Status = @status) AND (PayTime BETWEEN @d1 AND @d2)");
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                cmd.Parameters.AddWithValue("@type", "Debit");
                cmd.Parameters.AddWithValue("@status", "1");
                DataTable dtAgent_Debits = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT Branchid, AmountPaid, Remarks, DATE_FORMAT(PaidDate, '%d/%b/%y') AS PDate, PayTime, EmpID, ReceiptNo, VarifyDate, TransactionType, AmountDebited, DiffAmount, SalesOfficeID, Status FROM collections WHERE (SalesOfficeID = @BranchID) AND (TransactionType = @type) AND (Status = @status) AND (PayTime BETWEEN @d1 AND @d2)");
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                cmd.Parameters.AddWithValue("@type", "Credit");
                cmd.Parameters.AddWithValue("@status", "1");
                DataTable dtAgent_Credits = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT  sno, OppBalance, SaleValue, paidamount, ClosingBalance, IndentDate, EntryDate, agentid, salesofficeid, SaleQty, ReceivedAmount, DiffAmount, RouteId, status FROM tempduetrasactions WHERE (IndentDate BETWEEN @d1 AND @d2) AND (salesofficeid = @SOID)");
                //cmd = new MySqlCommand("SELECT Sno, SalesOfficeId, RouteId, AgentId, IndentDate, EntryDate, OppBalance, SaleQty, SaleValue, ReceivedAmount, ClosingBalance, DiffAmount FROM duetransactions WHERE (IndentDate BETWEEN @d1 AND @d2) AND (SalesOfficeId = @SOID)");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-2)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-2)));
                cmd.Parameters.AddWithValue("@SOID", BranchID);
                DataTable dtOpp = vdm.SelectQuery(cmd).Tables[0];
                Report = new DataTable();
                Report.Columns.Add("Sno");
                Report.Columns.Add("Route Code");
                Report.Columns.Add("Agent Code");
                Report.Columns.Add("Agent Name");
                Report.Columns.Add("Oppening Balance");
                Report.Columns.Add("Sale Qty").DataType = typeof(Double);
                Report.Columns.Add("Sale Value").DataType = typeof(Double);
                Report.Columns.Add("Received Amount").DataType = typeof(Double);
                Report.Columns.Add("Closing Amount").DataType = typeof(Double);
                foreach (DataRow drroutes in dtroutes.Rows)
                {
                    double totalsaleqty = 0;
                    double totalopp = 0;
                    double totalsalevalue = 0;
                    double totalamountpaid = 0;
                    double totalclosingbalance = 0;
                    DataRow newrow = Report.NewRow();
                    newrow["Agent Name"] = drroutes["RouteName"].ToString();
                    Report.Rows.Add(newrow);
                    DataRow newbreak = Report.NewRow();
                    newbreak["Agent Name"] = "";
                    Report.Rows.Add(newbreak);
                    int i = 1;
                    foreach (DataRow drsale in dtroutecollection.Select("routesno='" + drroutes["routesno"].ToString() + "'"))
                    {
                        DataRow newrow1 = Report.NewRow();
                        newrow1["Sno"] = i++;
                        newrow1["Route Code"] = drsale["routesno"].ToString();
                        newrow1["Agent Code"] = drsale["BranchID"].ToString();
                        newrow1["Agent Name"] = drsale["BranchName"].ToString();
                        double saleqty = 0;
                        double salevalue = 0;
                        double amountpaid = 0;
                        double chequeamountpaid = 0;
                        double oppbalance = 0;
                        double debitamount = 0;
                        double creditamount = 0;
                        foreach (DataRow dropp in dtOpp.Select("AgentId='" + drsale["BranchID"].ToString() + "'"))
                        {
                            double.TryParse(dropp["ClosingBalance"].ToString(), out oppbalance);
                            newrow1["Oppening Balance"] = oppbalance;
                        }
                        foreach (DataRow drdeb in dtAgent_Debits.Select("Branchid='" + drsale["BranchID"].ToString() + "'"))
                        {
                            double amountDebited = 0;
                            double.TryParse(drdeb["AmountDebited"].ToString(), out amountDebited);
                            debitamount += amountDebited;
                        }
                        foreach (DataRow drcre in dtAgent_Credits.Select("Branchid='" + drsale["BranchID"].ToString() + "'"))
                        {
                            double amountcredited = 0;
                            double.TryParse(drcre["AmountPaid"].ToString(), out amountcredited);
                            creditamount += amountcredited;
                        }
                        double.TryParse(drsale["saleQty"].ToString(), out saleqty);
                        double.TryParse(drsale["salevalue"].ToString(), out salevalue);
                        newrow1["Sale Qty"] = Math.Round(saleqty, 2);
                        newrow1["Sale Value"] = Math.Round(salevalue, 2);
                        totalopp += Math.Round(oppbalance, 2);
                        totalsaleqty += Math.Round(saleqty, 2);
                        totalsalevalue += Math.Round(salevalue, 2);
                        foreach (DataRow drcoll in dtrouteamount.Select("BranchID='" + drsale["BranchID"].ToString() + "'"))
                        {
                            double.TryParse(drcoll["amtpaid"].ToString(), out amountpaid);
                            totalamountpaid += Math.Round(amountpaid, 2);
                        }
                        if(dtsalescollection.Rows.Count>0)
                        {
                            foreach (DataRow drcoll in dtsalescollection.Select("BranchID='" + drsale["BranchID"].ToString() + "'"))
                            {
                                double.TryParse(drcoll["AmountPaid"].ToString(), out amountpaid);
                                totalamountpaid += Math.Round(amountpaid, 2);
                            }
                        }
                        foreach (DataRow drChequecoll in dtrouteChequeamount.Select("BranchID='" + drsale["BranchID"].ToString() + "'"))
                        {
                            double.TryParse(drChequecoll["amtpaid"].ToString(), out chequeamountpaid);
                            totalamountpaid += Math.Round(chequeamountpaid, 2);
                        }
                        double totamt = 0;
                        totamt = amountpaid + chequeamountpaid;
                        newrow1["Received Amount"] = Math.Round(totamt, 2);
                        double closing = 0;
                        closing = (oppbalance + salevalue) - (totamt);
                        closing = closing + debitamount;
                        newrow1["Closing Amount"] = Math.Round(closing, 2);
                        totalclosingbalance += Math.Round(closing, 2);
                        if (saleqty == 0 && salevalue == 0 && totamt == 0 && closing == 0)
                        {
                        }
                        else
                        {
                            Report.Rows.Add(newrow1);
                        }
                    }
                    DataRow TotRow = Report.NewRow();
                    TotRow["Agent Name"] = "Total";
                    TotRow["Oppening Balance"] = totalopp;
                    TotRow["Sale Qty"] = totalsaleqty;
                    TotRow["Sale Value"] = totalsalevalue;
                    TotRow["Received Amount"] = totalamountpaid;
                    TotRow["Closing Amount"] = totalclosingbalance;
                    Report.Rows.Add(TotRow);
                    DataRow newbreak1 = Report.NewRow();
                    newbreak1["Agent Name"] = "";
                    Report.Rows.Add(newbreak1);
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["AmountClosing"] = Report;
                for (int ii = 0; ii < grdReports.Rows.Count; ii++)
                {
                    GridViewRow dgvr = grdReports.Rows[ii];
                    if (dgvr.Cells[2].Text != dgvr.Cells[3].Text)
                    {
                        //GridViewRow compare = grdReports.Rows[ii + 1];
                        DateTime dtmydatetime2 = DateTime.Now;
                        string dt1 = dgvr.Cells[2].Text;
                        string dt2 = dgvr.Cells[3].Text;
                        if (dt1 == "&nbsp;")
                        {
                            if (dt2 != "&nbsp;")
                            {
                                if (dt2 == "Total")
                                {
                                    dgvr.Cells[3].BackColor = Color.SandyBrown;
                                }
                                else
                                {
                                    dgvr.Cells[3].BackColor = Color.LawnGreen;
                                }

                            }
                        }

                    }
                }
            }
            if (ddlreporttype.SelectedItem.Text == "Inventory Balance")
            {
                Report = new DataTable();
                Report.Columns.Add("Sno");
                Report.Columns.Add("Route Code");
                Report.Columns.Add("Agent Code");
                Report.Columns.Add("Agent Name");
                //Report.Columns.Add("Crates Oppening");
                Report.Columns.Add("Issued Crates").DataType = typeof(Double);
                Report.Columns.Add("Return Crates").DataType = typeof(Double);
                //Report.Columns.Add("Crates Balance").DataType = typeof(Double);
               // Report.Columns.Add("Cans Oppening").DataType = typeof(Double);
                Report.Columns.Add("Issued Can40ltr").DataType = typeof(Double);
                Report.Columns.Add("Return Can40ltr").DataType = typeof(Double);
                Report.Columns.Add("Issued Can20ltr").DataType = typeof(Double);
                Report.Columns.Add("Return Can20ltr").DataType = typeof(Double);
                Report.Columns.Add("Issued Can10ltr").DataType = typeof(Double);
                Report.Columns.Add("Return Can10ltr").DataType = typeof(Double);
                //Report.Columns.Add("Cans Balance").DataType = typeof(Double);
                //cmd = new MySqlCommand("");
                //DataTable dtInvOpp = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.Sno AS routesno, modifiedroutes.RouteName, invtran.B_inv_sno, SUM(invtran.Qty) AS deliveryqty,modifidroutssubtab.BranchID, branchdata_2.BranchName AS Agentname FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN branchdata branchdata_2 ON modifidroutssubtab.BranchID = branchdata_2.sno LEFT OUTER JOIN (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty, CBFromTran, CBToTran, DeliveryTime,CollectionTime, Remarks FROM invtransactions12 WHERE (TransType = 1) AND (DOE BETWEEN @d1 AND @d2) OR (TransType = 2) AND (DOE BETWEEN @d1 AND @d2)) invtran ON modifidroutssubtab.BranchID = invtran.ToTran WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID, invtran.B_inv_sno, branchdata_2.BranchName ORDER BY branchdata.sno, routesno");
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                DateTime dt1 = GetLowDate(fromdate.AddDays(-1));
                DateTime dt2 = GetLowDate(fromdate);
                cmd.Parameters.AddWithValue("@d1", dt1.AddHours(15));
                cmd.Parameters.AddWithValue("@d2", dt2.AddHours(15));
                cmd.Parameters.AddWithValue("@SOID", BranchID);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                DataTable dtInvdelivery = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.Sno AS routesno, modifiedroutes.RouteName, invtran.B_inv_sno, SUM(invtran.Qty) AS deliveryqty,modifidroutssubtab.BranchID, branchdata_2.BranchName AS Agentname FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN branchdata branchdata_2 ON modifidroutssubtab.BranchID = branchdata_2.sno LEFT OUTER JOIN (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty, CBFromTran, CBToTran, DeliveryTime,CollectionTime, Remarks FROM invtransactions12 WHERE (TransType = 1) AND (DOE BETWEEN @d1 AND @d2) OR (TransType = 3) AND (DOE BETWEEN @d1 AND @d2)) invtran ON modifidroutssubtab.BranchID = invtran.FromTran WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID, invtran.B_inv_sno, branchdata_2.BranchName ORDER BY branchdata.sno, routesno");
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d1", dt1.AddHours(15));
                cmd.Parameters.AddWithValue("@d2", dt2.AddHours(15));
                cmd.Parameters.AddWithValue("@SOID", BranchID);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                DataTable dtinvcollection = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.Sno AS routesno, modifiedroutes.RouteName, modifidroutssubtab.BranchID,branchdata_2.BranchName AS Agentname FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN branchdata branchdata_2 ON modifidroutssubtab.BranchID = branchdata_2.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY branchdata.sno, routesno");
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                cmd.Parameters.AddWithValue("@SOID", BranchID);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                DataTable dtbranches = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow drroutes in dtroutes.Rows)
                {
                     DataRow newrow = Report.NewRow();
                    newrow["Agent Name"] = drroutes["RouteName"].ToString();
                    Report.Rows.Add(newrow);
                    DataRow newbreak = Report.NewRow();
                    newbreak["Agent Name"] = "";
                    Report.Rows.Add(newbreak);
                    int i = 1;
                    int cratesissued = 0;
                    int cratesreturn = 0;
                    int can40issued = 0;
                    int can40return = 0;
                    int can20issued = 0;
                    int can20return = 0;
                    int can10issued = 0;
                    int can10return = 0;
                    foreach (DataRow drsale in dtbranches.Select("routesno='" + drroutes["routesno"].ToString() + "'"))
                    {
                        DataRow newrow1 = Report.NewRow();
                        newrow1["Sno"] = i++;
                        newrow1["Route Code"] = drsale["routesno"].ToString();
                        newrow1["Agent Code"] = drsale["BranchID"].ToString();
                        newrow1["Agent Name"] = drsale["Agentname"].ToString();
                       
                       
                        foreach (DataRow drDelivery in dtInvdelivery.Select("BranchID='" + drsale["BranchID"].ToString() + "'"))
                        {
                            if (drDelivery["B_inv_sno"].ToString() == "1")
                            {
                                int cisued = 0;
                                int.TryParse(drDelivery["deliveryqty"].ToString(),out cisued);
                                newrow1["Issued Crates"] = drDelivery["deliveryqty"].ToString();
                                cratesissued += cisued;
                            }
                            if (drDelivery["B_inv_sno"].ToString() == "2")
                            {
                                int cisued = 0;
                                int.TryParse(drDelivery["deliveryqty"].ToString(), out cisued);
                                newrow1["Issued Can10ltr"] = drDelivery["deliveryqty"].ToString();
                                can10issued += cisued;
                            }
                            if (drDelivery["B_inv_sno"].ToString() == "3")
                            {
                                int cisued = 0;
                                int.TryParse(drDelivery["deliveryqty"].ToString(), out cisued);
                                newrow1["Issued Can20ltr"] = drDelivery["deliveryqty"].ToString();
                                can20issued += cisued;
                            }
                            if (drDelivery["B_inv_sno"].ToString() == "4")
                            {
                                int cisued = 0;
                                int.TryParse(drDelivery["deliveryqty"].ToString(), out cisued);
                                newrow1["Issued Can40ltr"] = drDelivery["deliveryqty"].ToString();
                                can40issued += cisued;
                            }
                        }
                        foreach (DataRow drColl in dtinvcollection.Select("BranchID='" + drsale["BranchID"].ToString() + "'"))
                        {
                            if (drColl["B_inv_sno"].ToString() == "1")
                            {
                                int creturn = 0;
                                int.TryParse(drColl["deliveryqty"].ToString(), out creturn);
                                newrow1["Return Crates"] = drColl["deliveryqty"].ToString();
                                cratesreturn += creturn;
                            }
                            if (drColl["B_inv_sno"].ToString() == "2")
                            {
                                int creturn = 0;
                                int.TryParse(drColl["deliveryqty"].ToString(), out creturn);
                                newrow1["Return Can10ltr"] = drColl["deliveryqty"].ToString();
                                can10return += creturn;
                            }
                            if (drColl["B_inv_sno"].ToString() == "3")
                            {
                                int creturn = 0;
                                int.TryParse(drColl["deliveryqty"].ToString(), out creturn);
                                newrow1["Return Can20ltr"] = drColl["deliveryqty"].ToString();
                                can20return += creturn;
                            }
                            if (drColl["B_inv_sno"].ToString() == "4")
                            {
                                int creturn = 0;
                                int.TryParse(drColl["deliveryqty"].ToString(), out creturn);
                                newrow1["Return Can40ltr"] = drColl["deliveryqty"].ToString();
                                can40return += creturn;
                            }
                        }
                        Report.Rows.Add(newrow1);

                    }
                    DataRow TotRow = Report.NewRow();
                    TotRow["Agent Name"] = "Total";
                    TotRow["Issued Crates"] = cratesissued;
                    TotRow["Return Crates"] = cratesreturn;
                    TotRow["Issued Can10ltr"] = can10issued;
                    TotRow["Return Can10ltr"] = can10return;
                    TotRow["Issued Can20ltr"] = can20issued;
                    TotRow["Return Can20ltr"] = can20return;
                    TotRow["Issued Can40ltr"] = can40issued;
                    TotRow["Return Can40ltr"] = can40return;
                    Report.Rows.Add(TotRow);
                    DataRow newbreak1 = Report.NewRow();
                    newbreak1["Agent Name"] = "";
                    Report.Rows.Add(newbreak1);
                }

                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            lblmessage.Text = ex.Message;
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlreporttype.SelectedItem.Text == "Amount Balance")
            {
                vdm = new VehicleDBMgr();
                string SalesOfficeId = ddlSalesOffice.SelectedValue;
                if (SalesOfficeId == "572")
                {
                    SalesOfficeId = "158";
                }
                string[] datestrig = txtdate.Text.Split(' ');
                if (datestrig.Length > 1)
                {
                    if (datestrig[0].Split('-').Length > 0)
                    {
                        string[] dates = datestrig[0].Split('-');
                        string[] times = datestrig[1].Split(':');
                        fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                    }
                }
                fromdate = fromdate.AddDays(-1);
                Prevdate = fromdate.AddDays(-1);
                DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
                cmd = new MySqlCommand("SELECT Sno, SalesOfficeId, RouteId, AgentId, IndentDate, EntryDate, OppBalance, SaleQty, SaleValue, ReceivedAmount, ClosingBalance, DiffAmount FROM duetransactions WHERE (SalesOfficeId = @SalesOfficeId) AND (IndentDate BETWEEN @d1 AND @d2) ORDER BY IndentDate");
                cmd.Parameters.AddWithValue("@SalesOfficeId", SalesOfficeId);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(Prevdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Prevdate));
                DataTable dtprevdate = vdm.SelectQuery(cmd).Tables[0];
                DataTable dt = (DataTable)Session["AmountClosing"];
                if (dtprevdate.Rows.Count > 0)
                {
                    foreach (DataRow drroutes in dt.Rows)
                    {

                        if (drroutes["Route Code"].ToString() == "")
                        {

                        }
                        else
                        {
                            cmd = new MySqlCommand("UPDATE duetransactions SET SalesOfficeId = @SalesOfficeId, RouteId = @RouteId, AgentId = @AgentId, IndentDate = @IndentDate, EntryDate = @EntryDate, OppBalance = @OppBalance,SaleQty = @SaleQty, SaleValue = @SaleValue, ReceivedAmount = @ReceivedAmount, ClosingBalance = @ClosingBalance, DiffAmount = @DiffAmount WHERE (IndentDate = @IndentDate) AND (AgentId = @AgentId)");
                            cmd.Parameters.AddWithValue("@SalesOfficeId", SalesOfficeId);
                            cmd.Parameters.AddWithValue("@RouteId", drroutes["Route Code"].ToString());
                            cmd.Parameters.AddWithValue("@AgentId", drroutes["Agent Code"].ToString());
                            cmd.Parameters.AddWithValue("@IndentDate", GetLowDate(fromdate));
                            cmd.Parameters.AddWithValue("@EntryDate", ServerDateCurrentdate);
                            double oppeningbal1 = 0;
                            double saleqty1 = 0;
                            double salevalue1 = 0;
                            double receivedamt1 = 0;
                            double closingamt1 = 0;
                            double differenceamount1 = 0;
                            double.TryParse(drroutes["Oppening Balance"].ToString(), out oppeningbal1);
                            double.TryParse(drroutes["Sale Qty"].ToString(), out saleqty1);
                            double.TryParse(drroutes["Sale Value"].ToString(), out salevalue1);
                            double.TryParse(drroutes["Received Amount"].ToString(), out receivedamt1);
                            double.TryParse(drroutes["Closing Amount"].ToString(), out closingamt1);
                            //double.TryParse(drroutes["Route Code"].ToString(), out differenceamount);
                            cmd.Parameters.AddWithValue("@OppBalance", Math.Round(oppeningbal1, 2));
                            cmd.Parameters.AddWithValue("@SaleQty", Math.Round(saleqty1, 2));
                            cmd.Parameters.AddWithValue("@SaleValue", Math.Round(salevalue1, 2));
                            cmd.Parameters.AddWithValue("@ReceivedAmount", Math.Round(receivedamt1, 2));
                            cmd.Parameters.AddWithValue("@ClosingBalance", Math.Round(closingamt1, 2));
                            cmd.Parameters.AddWithValue("@DiffAmount", Math.Round(differenceamount1, 2));
                            if (vdm.Update(cmd) == 0)
                            {
                                cmd = new MySqlCommand("insert into duetransactions (SalesOfficeId,RouteId,AgentId,IndentDate,EntryDate,OppBalance,SaleQty,SaleValue,ReceivedAmount,ClosingBalance,DiffAmount) values (@SalesOfficeId,@RouteId,@AgentId,@IndentDate,@EntryDate,@OppBalance,@SaleQty,@SaleValue,@ReceivedAmount,@ClosingBalance,@DiffAmount)");
                                cmd.Parameters.AddWithValue("@SalesOfficeId", SalesOfficeId);
                                cmd.Parameters.AddWithValue("@RouteId", drroutes["Route Code"].ToString());
                                cmd.Parameters.AddWithValue("@AgentId", drroutes["Agent Code"].ToString());
                                cmd.Parameters.AddWithValue("@IndentDate", GetLowDate(fromdate));
                                cmd.Parameters.AddWithValue("@EntryDate", ServerDateCurrentdate);
                                double oppeningbal = 0;
                                double saleqty = 0;
                                double salevalue = 0;
                                double receivedamt = 0;
                                double closingamt = 0;
                                double differenceamount = 0;
                                double.TryParse(drroutes["Oppening Balance"].ToString(), out oppeningbal);
                                double.TryParse(drroutes["Sale Qty"].ToString(), out saleqty);
                                double.TryParse(drroutes["Sale Value"].ToString(), out salevalue);
                                double.TryParse(drroutes["Received Amount"].ToString(), out receivedamt);
                                double.TryParse(drroutes["Closing Amount"].ToString(), out closingamt);
                                //double.TryParse(drroutes["Route Code"].ToString(), out differenceamount);
                                cmd.Parameters.AddWithValue("@OppBalance", Math.Round(oppeningbal, 2));
                                cmd.Parameters.AddWithValue("@SaleQty", Math.Round(saleqty, 2));
                                cmd.Parameters.AddWithValue("@SaleValue", Math.Round(salevalue, 2));
                                cmd.Parameters.AddWithValue("@ReceivedAmount", Math.Round(receivedamt, 2));
                                cmd.Parameters.AddWithValue("@ClosingBalance", Math.Round(closingamt, 2));
                                cmd.Parameters.AddWithValue("@DiffAmount", Math.Round(differenceamount, 2));
                                vdm.insert(cmd);
                            }
                        }
                    }
                    lblmsg.Text = "Agent Due Transactions Saved Successfully";
                    lblmessage.Text = "Agent Due Transactions Saved Successfully";
                    MessageBox.Show("Agent Due Transactions Saved Successfully", Page);
                }
                if (dtprevdate.Rows.Count <= 0)
                {
                    string msg = "Previous Day Transaction Not Yet Closed";
                    MessageBox.Show("Previous Day Transaction Not Yet Closed", Page);
                }
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            lblmessage.Text = ex.Message;
        }

    }
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable("GridView_Data");
            int count = 0;
            foreach (TableCell cell in grdReports.HeaderRow.Cells)
            {
                if (count == 3)
                {
                    dt.Columns.Add(cell.Text);
                }
                else
                {
                    dt.Columns.Add(cell.Text).DataType = typeof(double);
                }
                count++;
            }
            foreach (GridViewRow row in grdReports.Rows)
            {
                dt.Rows.Add();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (row.Cells[i].Text == "&nbsp;")
                    {
                        row.Cells[i].Text = "0";
                    }
                    dt.Rows[dt.Rows.Count - 1][i] = row.Cells[i].Text;
                }
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string FileName = "DueTransactionReport";
                Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            lblmessage.Text = ex.Message;
        }
    }
}