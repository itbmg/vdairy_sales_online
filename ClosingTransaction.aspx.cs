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

public partial class ClosingTransaction : System.Web.UI.Page
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
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                //lblTitle.Text = Session["TitleName"].ToString();
            }
        }
    }
    void FillSalesOffice()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Group")
            {
                PPlant.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) ");
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
    protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        PBranch.Visible = true;
        cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
        cmd.Parameters.AddWithValue("@SuperBranch", ddlPlant.SelectedValue);
        cmd.Parameters.AddWithValue("@SalesType", "21");
        cmd.Parameters.AddWithValue("@SalesType1", "26");
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlSalesOffice.DataSource = dtRoutedata;
        ddlSalesOffice.DataTextField = "BranchName";
        ddlSalesOffice.DataValueField = "sno";
        ddlSalesOffice.DataBind();

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
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        DateTime fromdate = DateTime.Now;
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
        try
        {
            string BranchID = "";
            //  string BranchID = dr["sno"].ToString();
            if (BranchID == "572")
            {
                BranchID = "7";
            }
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, branchroutes.Sno AS routesno, branchroutes.RouteName FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN branchroutes ON branchdata.sno = branchroutes.BranchID WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) AND (branchroutes.flag <> 0) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) AND (branchroutes.flag <> 0) ORDER BY branchdata.sno");
            cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            DataTable dtroutes = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, SUM(indents_subtable.DeliveryQty) AS saleQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue,modifiedroutes.Sno AS routesno, modifidroutssubtab.BranchID, branchdata_2.BranchName, branchdata_2.flag FROM branchdata branchdata_2 RIGHT OUTER JOIN branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo ON branchdata_2.sno = modifidroutssubtab.BranchID LEFT OUTER JOIN indents_subtable INNER JOIN (SELECT IndentNo, I_date, Branch_id FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indt ON indents_subtable.IndentNo = indt.IndentNo ON modifidroutssubtab.BranchID = indt.Branch_id WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY branchdata.sno, routesno");
            cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routesno, SUM(colltion.AmountPaid) AS amtpaid FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT Branchid, AmountPaid, PaidDate FROM collections WHERE (PaymentType <> 'Cheque') AND (PaymentType <> 'Bank Transfer') AND (PaidDate BETWEEN @d1 AND @d2)) colltion ON modifidroutssubtab.BranchID = colltion.Branchid WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY branchdata.sno");
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                dtrouteamount = vdm.SelectQuery(cmd).Tables[0];
            }
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routesno, SUM(colltion.AmountPaid) AS amtpaid FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT Branchid, AmountPaid, PaidDate FROM collections WHERE (PaymentType = 'Cheque') AND (VarifyDate BETWEEN @d1 AND @d2) AND (CheckStatus = 'V')) colltion ON modifidroutssubtab.BranchID = colltion.Branchid WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY modifidroutssubtab.BranchID");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            DataTable dtrouteChequeamount = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routesno, SUM(colltion.AmountPaid) AS amtpaid FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT Branchid, AmountPaid, PaidDate FROM collections WHERE (PaymentType = 'Bank Transfer') AND (VarifyDate BETWEEN @d1 AND @d2) AND (banktransferstatus = 'V')) colltion ON modifidroutssubtab.BranchID = colltion.Branchid WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY modifidroutssubtab.BranchID");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            DataTable dtrouteBankTransferamount = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT Branchid, AmountPaid, Remarks, DATE_FORMAT(PaidDate, '%d/%b/%y') AS PDate, PayTime, EmpID, ReceiptNo, VarifyDate, TransactionType, AmountDebited, DiffAmount, SalesOfficeID, Status FROM collections WHERE (SalesOfficeID = @BranchID) AND (TransactionType = @type) AND (Status = @status) AND (PayTime BETWEEN @d1 AND @d2)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@type", "Debit");
            cmd.Parameters.AddWithValue("@status", "1");
            DataTable dtAgent_Debits = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT Branchid, AmountPaid, Remarks, DATE_FORMAT(PaidDate, '%d/%b/%y') AS PDate, PayTime, EmpID, ReceiptNo, VarifyDate, TransactionType, AmountDebited, DiffAmount, SalesOfficeID, Status FROM collections WHERE (SalesOfficeID = @BranchID) AND (TransactionType = @type) AND (Status = @status) AND (PayTime BETWEEN @d1 AND @d2)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@type", "Credit");
            cmd.Parameters.AddWithValue("@status", "1");
            DataTable dtAgent_Credits = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT   sno, OppBalance, SaleValue, paidamount, ClosingBalance, IndentDate, EntryDate, agentid, salesofficeid, SaleQty, ReceivedAmount, DiffAmount, RouteId FROM tempduetrasactions where IndentDate between @d1 AND @d2 AND salesofficeid=@SOID");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-2)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-2)));
            cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
            DataTable dtOpp = vdm.SelectQuery(cmd).Tables[0];
            DataTable Report = new DataTable();
            Report.Columns.Add("Sno");
            Report.Columns.Add("SalesOfficeId");
            Report.Columns.Add("Route Code");
            Report.Columns.Add("Agent Code");
            Report.Columns.Add("Agent Name");
            Report.Columns.Add("Oppening Balance");
            Report.Columns.Add("Sale Qty").DataType = typeof(Double);
            Report.Columns.Add("Sale Value").DataType = typeof(Double);
            Report.Columns.Add("Received Amount").DataType = typeof(Double);
            Report.Columns.Add("Debit Amount").DataType = typeof(Double);
            Report.Columns.Add("Closing Amount").DataType = typeof(Double);
            foreach (DataRow drroutes in dtroutes.Rows)
            {
                double totalsaleqty = 0;
                double totalopp = 0;
                double totalsalevalue = 0;
                double totalamountpaid = 0;
                double totalclosingbalance = 0;
                int i = 1;
                foreach (DataRow drsale in dtroutecollection.Select("routesno='" + drroutes["routesno"].ToString() + "'"))
                {
                    DataRow newrow1 = Report.NewRow();
                    newrow1["Sno"] = i++;
                    newrow1["Route Code"] = drsale["routesno"].ToString();
                    newrow1["Agent Code"] = drsale["BranchID"].ToString();
                    newrow1["Agent Name"] = drsale["BranchName"].ToString();
                    newrow1["SalesOfficeId"] = ddlSalesOffice.SelectedValue;
                    double saleqty = 0;
                    double salevalue = 0;
                    double amountpaid = 0;
                    double chequeamountpaid = 0;
                    double banktrasferamountpaid = 0;
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
                        newrow1["Debit Amount"] = amountDebited;
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
                    if (dtsalescollection.Rows.Count > 0)
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

                    foreach (DataRow drBankTransfer in dtrouteBankTransferamount.Select("BranchID='" + drsale["BranchID"].ToString() + "'"))
                    {
                        double.TryParse(drBankTransfer["amtpaid"].ToString(), out banktrasferamountpaid);
                        totalamountpaid += Math.Round(banktrasferamountpaid, 2);
                    }
                    double totamt = 0;
                    totamt = amountpaid + chequeamountpaid + banktrasferamountpaid;
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
            }
            foreach (DataRow drroutes in Report.Rows)
            {
                if (drroutes["Route Code"].ToString() == "")
                {

                }
                else
                {
                    double oppeningbal1 = 0;
                    double saleqty1 = 0;
                    double salevalue1 = 0;
                    double receivedamt1 = 0;
                    double closingamt1 = 0;
                    double differenceamount1 = 0;
                    double debitamount = 0;
                    double.TryParse(drroutes["Oppening Balance"].ToString(), out oppeningbal1);
                    double.TryParse(drroutes["Sale Qty"].ToString(), out saleqty1);
                    double.TryParse(drroutes["Sale Value"].ToString(), out salevalue1);
                    double.TryParse(drroutes["Received Amount"].ToString(), out receivedamt1);
                    double.TryParse(drroutes["Closing Amount"].ToString(), out closingamt1);
                    double.TryParse(drroutes["Debit Amount"].ToString(), out debitamount);
                    cmd = new MySqlCommand("UPDATE  tempbranchaccounts SET Amount=@Amount where  Agentid=@BranchId");
                    cmd.Parameters.AddWithValue("@Amount", closingamt1);
                    cmd.Parameters.AddWithValue("@BranchId", drroutes["Agent Code"].ToString());
                    //vdm.Update(cmd);
                    cmd = new MySqlCommand("UPDATE tempduetrasactions SET salesofficeid = @SalesOfficeId, RouteId = @RouteId, agentid = @AgentId, IndentDate = @IndentDate, EntryDate = @EntryDate, OppBalance = @OppBalance,SaleQty = @SaleQty, SaleValue = @SaleValue, ReceivedAmount = @ReceivedAmount, ClosingBalance = @ClosingBalance, DiffAmount = @DiffAmount WHERE (IndentDate = @IndentDate) AND (agentid = @AgentId)");
                    cmd.Parameters.AddWithValue("@SalesOfficeId", drroutes["SalesOfficeId"].ToString());
                    cmd.Parameters.AddWithValue("@RouteId", drroutes["Route Code"].ToString());
                    cmd.Parameters.AddWithValue("@AgentId", drroutes["Agent Code"].ToString());
                    cmd.Parameters.AddWithValue("@IndentDate", GetLowDate(fromdate).AddDays(-1));
                    cmd.Parameters.AddWithValue("@EntryDate", ServerDateCurrentdate);

                    cmd.Parameters.AddWithValue("@OppBalance", Math.Round(oppeningbal1, 2));
                    cmd.Parameters.AddWithValue("@SaleQty", Math.Round(saleqty1, 2));
                    cmd.Parameters.AddWithValue("@SaleValue", Math.Round(salevalue1, 2));
                    cmd.Parameters.AddWithValue("@ReceivedAmount", Math.Round(receivedamt1, 2));
                    cmd.Parameters.AddWithValue("@ClosingBalance", Math.Round(closingamt1, 2));
                    cmd.Parameters.AddWithValue("@DiffAmount", Math.Round(debitamount, 2));
                    if (vdm.Update(cmd) == 0)
                    {
                        cmd = new MySqlCommand("insert into tempduetrasactions (salesofficeid,RouteId,agentid,IndentDate,EntryDate,OppBalance,SaleQty,SaleValue,ReceivedAmount,ClosingBalance,DiffAmount) values (@SalesOfficeId,@RouteId,@AgentId,@IndentDate,@EntryDate,@OppBalance,@SaleQty,@SaleValue,@ReceivedAmount,@ClosingBalance,@DiffAmount)");
                        cmd.Parameters.AddWithValue("@SalesOfficeId", drroutes["SalesOfficeId"].ToString());
                        cmd.Parameters.AddWithValue("@RouteId", drroutes["Route Code"].ToString());
                        cmd.Parameters.AddWithValue("@AgentId", drroutes["Agent Code"].ToString());
                        cmd.Parameters.AddWithValue("@IndentDate", GetLowDate(fromdate).AddDays(-1));
                        cmd.Parameters.AddWithValue("@EntryDate", ServerDateCurrentdate);
                        double oppeningbal = 0;
                        double saleqty = 0;
                        double salevalue = 0;
                        double receivedamt = 0;
                        double closingamt = 0;
                        double differenceamount = 0;
                        double debitamount1 = 0;
                        double.TryParse(drroutes["Oppening Balance"].ToString(), out oppeningbal);
                        double.TryParse(drroutes["Sale Qty"].ToString(), out saleqty);
                        double.TryParse(drroutes["Sale Value"].ToString(), out salevalue);
                        double.TryParse(drroutes["Received Amount"].ToString(), out receivedamt);
                        double.TryParse(drroutes["Closing Amount"].ToString(), out closingamt);
                        double.TryParse(drroutes["Debit Amount"].ToString(), out debitamount1);
                        cmd.Parameters.AddWithValue("@OppBalance", Math.Round(oppeningbal, 2));
                        cmd.Parameters.AddWithValue("@SaleQty", Math.Round(saleqty, 2));
                        cmd.Parameters.AddWithValue("@SaleValue", Math.Round(salevalue, 2));
                        cmd.Parameters.AddWithValue("@ReceivedAmount", Math.Round(receivedamt, 2));
                        cmd.Parameters.AddWithValue("@ClosingBalance", Math.Round(closingamt, 2));
                        cmd.Parameters.AddWithValue("@DiffAmount", Math.Round(debitamount1, 2));
                        vdm.insert(cmd);
                    }
                }
            }

            //BranchID = "172";
            cmd = new MySqlCommand("SELECT   sno, invsno, openinginv, isuue_invqty, receive_invqty, closing_invqty, doe, closing_date, branchid FROM inventarytransactions where branchid=@branchid AND closing_date between @d1 and @d2");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
            DataTable dtinvopp = vdm.SelectQuery(cmd).Tables[0];


            cmd = new MySqlCommand("SELECT  triproutes.Tripdata_sno, SUM(tripinvdata.Qty) AS DispatchQty, SUM(tripinvdata.Remaining) AS ReceivedQty, invmaster.tempinvname AS InvName, invmaster.sno AS InvSno, dispatch.Branch_Id FROM  tripdata INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripinvdata ON tripdata.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @branchid) GROUP BY invmaster.sno");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
            DataTable dtinventransaction = vdm.SelectQuery(cmd).Tables[0];

            double invissue = 0;
            double invreceive = 0;
            double openinvqty = 0;
            foreach (DataRow drdaycoll in dtinventransaction.Rows)
            {
                double issuedqty = 0; double ReceivedQty = 0;
                double.TryParse(drdaycoll["DispatchQty"].ToString(), out invissue);
                double.TryParse(drdaycoll["ReceivedQty"].ToString(), out invreceive);
                double presentopening = 0; double closinginvqty = 0;
                foreach (DataRow drtripinv in dtinvopp.Select("invsno='" + drdaycoll["InvSno"].ToString() + "'AND branchid='" + ddlSalesOffice.SelectedValue + "'"))
                {
                    double.TryParse(drtripinv["closing_invqty"].ToString(), out presentopening);
                    double oppening = presentopening + invreceive;
                    closinginvqty = oppening - invissue;
                }
                cmd = new MySqlCommand("UPDATE inventarytransactions SET  openinginv = @openinginv, isuue_invqty = @isuue_invqty, receive_invqty = @receive_invqty, closing_invqty = @closing_invqty, doe = @doe WHERE (closing_date = @closing_date) AND (branchid = @branchid) AND (invsno=@invsno)");
                cmd.Parameters.AddWithValue("@invsno", drdaycoll["InvSno"].ToString());
                cmd.Parameters.AddWithValue("@openinginv", presentopening);
                cmd.Parameters.AddWithValue("@isuue_invqty", invissue);
                cmd.Parameters.AddWithValue("@receive_invqty", invreceive);
                cmd.Parameters.AddWithValue("@closing_invqty", closinginvqty);
                cmd.Parameters.AddWithValue("@doe", ServerDateCurrentdate);
                cmd.Parameters.AddWithValue("@closing_date", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
                if (vdm.Update(cmd) == 0)
                {
                    cmd = new MySqlCommand("insert into inventarytransactions (invsno,openinginv,isuue_invqty,receive_invqty,closing_invqty,doe,closing_date,branchid) values (@invsno,@openinginv,@isuue_invqty,@receive_invqty,@closing_invqty,@doe,@closing_date,@branchid)");
                    cmd.Parameters.AddWithValue("@invsno", drdaycoll["InvSno"].ToString());
                    cmd.Parameters.AddWithValue("@openinginv", presentopening);
                    cmd.Parameters.AddWithValue("@isuue_invqty", invissue);
                    cmd.Parameters.AddWithValue("@receive_invqty", invreceive);
                    cmd.Parameters.AddWithValue("@closing_invqty", closinginvqty);
                    cmd.Parameters.AddWithValue("@doe", ServerDateCurrentdate);
                    cmd.Parameters.AddWithValue("@closing_date", GetLowDate(fromdate));
                    cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
                    vdm.insert(cmd);
                }
            }
        }
        catch (Exception ex)
        {

        }
        
    }
}