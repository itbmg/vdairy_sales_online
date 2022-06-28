using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Drawing;
using System.IO;

public partial class RouteWiseAmountInv : System.Web.UI.Page
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
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
    string routeid = "";
    string routeitype = "";
    void GetReport()
    {
        try
        {
            pnlHide.Visible = true;
            lblmsg.Text = "";
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
            lblDate.Text = fromdate.ToString("dd/MMM/yyyy");
            string BranchID = ddlSalesOffice.SelectedValue;
            if (BranchID == "572")
            {
                BranchID = "7";
            }

            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno,branchdata.CollectionType, branchroutes.Sno AS routesno, branchroutes.RouteName FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN branchroutes ON branchdata.sno = branchroutes.BranchID WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) AND (branchroutes.flag <> 0) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) AND (branchroutes.flag <> 0) ORDER BY branchdata.sno");
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            cmd.Parameters.AddWithValue("@BranchID", BranchID);

            DataTable dtroutes = vdm.SelectQuery(cmd).Tables[0];
            if (ddlreporttype.SelectedItem.Text == "Amount Balance")
            {
                //cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, SUM(indents_subtable.DeliveryQty) AS saleQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue,modifiedroutes.Sno AS routesno, modifidroutssubtab.BranchID, branchdata_2.BranchName FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT IndentNo, I_date, Branch_id FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indt ON modifidroutssubtab.BranchID = indt.Branch_id INNER JOIN indents_subtable ON indt.IndentNo = indents_subtable.IndentNo INNER JOIN branchdata branchdata_2 ON modifidroutssubtab.BranchID = branchdata_2.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY branchdata.sno, routesno");
                cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, SUM(indents_subtable.DeliveryQty) AS saleQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue,modifiedroutes.Sno AS routesno, modifidroutssubtab.BranchID, branchdata_2.BranchName, branchdata_2.flag,SUM(branchdata_2.DueLimit) AS Duelimit FROM branchdata branchdata_2 RIGHT OUTER JOIN branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo ON branchdata_2.sno = modifidroutssubtab.BranchID LEFT OUTER JOIN indents_subtable INNER JOIN (SELECT IndentNo, I_date, Branch_id FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indt ON indents_subtable.IndentNo = indt.IndentNo ON modifidroutssubtab.BranchID = indt.Branch_id WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifiedroutes.Sno ORDER BY routesno");
                cmd.Parameters.AddWithValue("@SOID", BranchID);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(Todate.AddDays(-1)));
                DataTable dtroutecollection = vdm.SelectQuery(cmd).Tables[0];
                //cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno,branchdata_1.CollectionType,modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routesno, SUM(colltion.AmountPaid) AS amtpaid FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT Branchid, AmountPaid, PaidDate FROM collections WHERE (PaymentType <> 'Cheque') AND (PaidDate BETWEEN @d1 AND @d2)) colltion ON modifidroutssubtab.BranchID = colltion.Branchid WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY modifidroutssubtab.BranchID");
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routesno, SUM(colltion.AmountPaid) AS amtpaid, branchdata_1.SalesType, branchdata_2.CollectionType FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT  RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT Branchid, AmountPaid, PaidDate FROM collections WHERE (PaymentType <> 'Cheque') AND (PaidDate BETWEEN @d1 AND @d2)) colltion ON modifidroutssubtab.BranchID = colltion.Branchid INNER JOIN branchdata branchdata_2 ON modifidroutssubtab.BranchID = branchdata_2.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifiedroutes.Sno  ORDER BY modifiedroutes.Sno ");
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                cmd.Parameters.AddWithValue("@SOID", BranchID);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                DataTable dtrouteamount = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno,modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routesno, SUM(colltion.AmountPaid) AS amtpaid FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT Branchid, AmountPaid, PaidDate FROM collections WHERE (PaymentType = 'Cheque') AND (VarifyDate BETWEEN @d1 AND @d2) AND (CheckStatus = 'V')) colltion ON modifidroutssubtab.BranchID = colltion.Branchid WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifiedroutes.Sno ORDER BY modifiedroutes.Sno");
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                cmd.Parameters.AddWithValue("@SOID", BranchID);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                DataTable dtrouteChequeamount = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT Sno, SalesOfficeId, RouteId, AgentId, IndentDate, EntryDate, OppBalance, SaleQty, SaleValue, ReceivedAmount, SUM(ClosingBalance) AS ClosingBalance,DiffAmount FROM duetransactions WHERE (IndentDate BETWEEN @d1 AND @d2) AND (SalesOfficeId = @SOID) GROUP BY RouteId ORDER BY RouteId");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-2)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-2)));
                cmd.Parameters.AddWithValue("@SOID", BranchID);
                DataTable dtOpp = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.Sno AS routesno, modifiedroutes.RouteName, modifidroutssubtab.BranchID,branchdata_2.BranchName AS Agentname, inventory_monitor.Inv_Sno, inventory_monitor.Qty FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN branchdata branchdata_2 ON modifidroutssubtab.BranchID = branchdata_2.sno LEFT OUTER JOIN inventory_monitor ON modifidroutssubtab.BranchID = inventory_monitor.BranchId WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID, inventory_monitor.Inv_Sno ORDER BY branchdata.sno, routesno");
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@SOID", BranchID);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                DataTable dtinvopp = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.Sno AS routesno, modifiedroutes.RouteName, invtran.B_inv_sno, SUM(invtran.Qty) AS deliveryqty,modifidroutssubtab.BranchID, branchdata_2.BranchName AS Agentname FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN branchdata branchdata_2 ON modifidroutssubtab.BranchID = branchdata_2.sno LEFT OUTER JOIN (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty, CBFromTran, CBToTran, DeliveryTime,CollectionTime, Remarks FROM invtransactions12 WHERE (TransType = 1) AND (DOE BETWEEN @d1 AND @d2) OR (TransType = 2) AND (DOE BETWEEN @d1 AND @d2)) invtran ON modifidroutssubtab.BranchID = invtran.ToTran WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID, invtran.B_inv_sno, branchdata_2.BranchName ORDER BY branchdata.sno, routesno");
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                DateTime dt1 = GetLowDate(fromdate.AddDays(-1));
                DateTime dt2 = GetLowDate(fromdate);
                cmd.Parameters.AddWithValue("@d1", dt1.AddHours(15));
                cmd.Parameters.AddWithValue("@d2", dt2.AddHours(15));
                cmd.Parameters.AddWithValue("@SOID", BranchID);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                DataTable dtInvdelivery = vdm.SelectQuery(cmd).Tables[0];

                Report = new DataTable();
                Report.Columns.Add("RouteCode");
                Report.Columns.Add("Route Name");
                Report.Columns.Add("Opening Balance");
                Report.Columns.Add("Sale Qty").DataType = typeof(Double);
                Report.Columns.Add("Sale Value").DataType = typeof(Double);
                Report.Columns.Add("Received Amount").DataType = typeof(Double);
                Report.Columns.Add("Closing Amount").DataType = typeof(Double);
                Report.Columns.Add("Eligible Due").DataType = typeof(Double);
                Report.Columns.Add("Over Due").DataType = typeof(Double);
                Report.Columns.Add("Crates Balance").DataType = typeof(Double);
                Report.Columns.Add("Excess Crates").DataType = typeof(Double);
                Report.Columns.Add("Cans Balance").DataType = typeof(Double);
                Report.Columns.Add("Excess Cans").DataType = typeof(Double);

                double totalsaleqty = 0;
                double totalopp = 0;
                double totalsalevalue = 0;
                double totalamountpaid = 0;
                double totalclosingbalance = 0;
                double totaloverdue = 0;
                double totalcratesbal = 0;
                double totalcansbal = 0;
                foreach (DataRow drroutes in dtroutes.Rows)
                {
                    
                    string collectiontype = drroutes["CollectionType"].ToString();
                   
                    foreach (DataRow drsale in dtroutecollection.Select("routesno='" + drroutes["routesno"].ToString() + "'"))
                    {
                        DataRow newrow1 = Report.NewRow();
                        //collectiontype = drsale["CollectionType"].ToString();
                        //newrow1["Route Name"] = drroutes["RouteName"].ToString();

                        newrow1["RouteCode"] = drsale["routesno"].ToString();
                        //newrow1["Route Code"] = drsale["routesno"].ToString();
                        newrow1["Route Name"] = drsale["RouteName"].ToString();
                        double saleqty = 0;
                        double salevalue = 0;
                        double amountpaid = 0;
                        double chequeamountpaid = 0;
                        double oppbalance = 0;
                        int cratesopp = 0;
                        int cratesdelivered = 0;
                        int cratesexcess = 0;
                        int cansexcess = 0;
                        int cansdelivered = 0;
                        int cansopp = 0;
                        foreach (DataRow dropp in dtOpp.Select("RouteId='" + drsale["routesno"].ToString() + "'"))
                        {
                            double.TryParse(dropp["ClosingBalance"].ToString(), out oppbalance);
                            newrow1["Opening Balance"] = oppbalance;

                        }
                        double.TryParse(drsale["saleQty"].ToString(), out saleqty);
                        double.TryParse(drsale["salevalue"].ToString(), out salevalue);
                        newrow1["Sale Qty"] = Math.Round(saleqty, 2);
                        newrow1["Sale Value"] = Math.Round(salevalue, 2);
                        totalopp += Math.Round(oppbalance, 2);
                        totalsaleqty += Math.Round(saleqty, 2);
                        totalsalevalue += Math.Round(salevalue, 2);
                        foreach (DataRow drcoll in dtrouteamount.Select("routesno='" + drsale["routesno"].ToString() + "'"))
                        {
                            double.TryParse(drcoll["amtpaid"].ToString(), out amountpaid);
                            totalamountpaid += Math.Round(amountpaid, 2);
                            collectiontype = drcoll["CollectionType"].ToString();


                        }
                        foreach (DataRow drChequecoll in dtrouteChequeamount.Select("routesno='" + drsale["routesno"].ToString() + "'"))
                        {
                            double.TryParse(drChequecoll["amtpaid"].ToString(), out chequeamountpaid);
                            totalamountpaid += Math.Round(chequeamountpaid, 2);
                            //collectiontype = drChequecoll["CollectionType"].ToString();


                        }
                        foreach (DataRow drinvopp in dtinvopp.Select("routesno='" + drsale["routesno"].ToString() + "'"))
                        {
                            int bal=0;
                            if (drinvopp["Inv_Sno"].ToString() == "1")
                            {
                                int.TryParse(drinvopp["Qty"].ToString(), out bal);
                                cratesopp += bal;
                            }
                            if (drinvopp["Inv_Sno"].ToString() == "2")
                            {
                                int.TryParse(drinvopp["Qty"].ToString(), out bal);
                                cansopp += bal;
                            }
                            if (drinvopp["Inv_Sno"].ToString() == "3")
                            {
                                int.TryParse(drinvopp["Qty"].ToString(), out bal);
                                cansopp += bal;
                            }
                            if (drinvopp["Inv_Sno"].ToString() == "4")
                            {
                                int.TryParse(drinvopp["Qty"].ToString(), out bal);
                                cansopp += bal;
                            }
                        }
                        foreach (DataRow drinvdel in dtInvdelivery.Select("routesno='" + drsale["routesno"].ToString() + "'"))
                        {
                            int del=0;
                            if (drinvdel["B_inv_sno"].ToString() == "1")
                            {
                                int.TryParse(drinvdel["deliveryqty"].ToString(), out del);
                                cratesdelivered += del;
                            }
                            if (drinvdel["B_inv_sno"].ToString() == "2")
                            {
                                int.TryParse(drinvdel["deliveryqty"].ToString(), out del);
                                cansdelivered += del;
                            }
                            if (drinvdel["B_inv_sno"].ToString() == "3")
                            {
                                int.TryParse(drinvdel["deliveryqty"].ToString(), out del);
                                cansdelivered += del;
                            }
                            if (drinvdel["B_inv_sno"].ToString() == "4")
                            {
                                int.TryParse(drinvdel["deliveryqty"].ToString(), out del);
                                cansdelivered += del;
                            }
                        }
                        double totamt = 0;
                        totamt = amountpaid + chequeamountpaid;
                        newrow1["Received Amount"] = Math.Round(totamt, 2);
                        double closing = 0;
                        closing = (oppbalance + salevalue) - (totamt);
                        newrow1["Closing Amount"] = Math.Round(closing, 2);
                        totalclosingbalance += Math.Round(closing, 2);
                        double eligibledue = 0;
                        double overdue = 0;
                        int status = 0;
                        double.TryParse(drsale["Duelimit"].ToString(), out eligibledue);
                        //if (collectiontype == "CASH")
                        //{
                        //    newrow1["Eligible Due"] = "0";
                        //    newrow1["Over Due"] = Math.Round(closing, 2);
                        //    totaloverdue += Math.Round(closing, 2);
                        //}
                        //if (collectiontype != "CASH")
                        //{
                        //    eligibledue = 5000;
                        //    newrow1["Eligible Due"] = "5000";
                        //    if (closing > 5000)
                        //    {
                        //        overdue = Math.Round(closing - eligibledue, 2);
                        //    }
                        //    newrow1["Over Due"] = Math.Round(overdue, 2);
                        //    totaloverdue += Math.Round(overdue, 2);

                        //}
                        newrow1["Eligible Due"] = eligibledue;
                        if (closing > eligibledue)
                        {
                            overdue = Math.Round(closing - eligibledue, 2);
                        }
                        newrow1["Over Due"] = Math.Round(overdue, 2);
                        newrow1["Crates Balance"] = cratesopp;
                        totalcratesbal += cratesopp;
                        newrow1["Excess Crates"] = cratesopp - cratesdelivered;
                        newrow1["Cans Balance"] = cansopp;
                        newrow1["Excess Cans"] = cansopp - cansdelivered;
                        totalcansbal += cansopp;
                        
                        
                        totaloverdue += Math.Round(overdue, 2);
                        Report.Rows.Add(newrow1);

                    }
                    
                    
                }
                DataRow TotRow = Report.NewRow();
                TotRow["Route Name"] = "Total";
                TotRow["Opening Balance"] = totalopp;
                TotRow["Sale Qty"] = totalsaleqty;
                TotRow["Sale Value"] = totalsalevalue;
                TotRow["Received Amount"] = totalamountpaid;
                TotRow["Closing Amount"] = totalclosingbalance;
                TotRow["Over Due"] = totaloverdue;
                TotRow["Crates Balance"] = totalcratesbal;
                TotRow["Cans Balance"] = totalcansbal;
                Report.Rows.Add(TotRow);
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["AmountClosing"] = Report;

              
            }
            if (ddlreporttype.SelectedItem.Text == "Inventory Balance")
            {
                Report = new DataTable();
                Report.Columns.Add("Sno");
                Report.Columns.Add("Agent Code");
                Report.Columns.Add("Agent Name");
                Report.Columns.Add("Crates Oppening");
                Report.Columns.Add("Issued Crates").DataType = typeof(Double);
                Report.Columns.Add("Return Crates").DataType = typeof(Double);
                Report.Columns.Add("Crates Balance").DataType = typeof(Double);
                Report.Columns.Add("Excess").DataType = typeof(Double);
                DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);

                cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, modifiedroutes.Sno AS routesno, modifidroutssubtab.BranchID, branchdata_2.BranchName, branchdata_2.flag,inventory_monitor.Qty FROM inventory_monitor RIGHT OUTER JOIN branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo ON inventory_monitor.BranchId = modifidroutssubtab.BranchID LEFT OUTER JOIN branchdata branchdata_2 ON modifidroutssubtab.BranchID = branchdata_2.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) AND (inventory_monitor.Inv_Sno = 1) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY branchdata.sno, routesno");
                cmd.Parameters.AddWithValue("@SOID", BranchID);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(Todate.AddDays(-1)));
                DataTable dtroutecollection = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow drroutes in dtroutes.Rows)
                {
                    double totalopp = 0;
                    double totalissued = 0;
                    double totalreceived = 0;
                    double totalbalance = 0;
                    double totalexcess = 0;
                    //string collectiontype = drroutes["CollectionType"].ToString();
                    DataRow newrow = Report.NewRow();
                    newrow["Agent Name"] = drroutes["RouteName"].ToString();
                    Report.Rows.Add(newrow);
                    DataRow newbreak = Report.NewRow();
                    newbreak["Agent Name"] = "";
                    Report.Rows.Add(newbreak);
                    int i = 1;
                    foreach (DataRow drsale in dtroutecollection.Select("routesno='" + drroutes["routesno"].ToString() + "'"))
                    {
                        cmd = new MySqlCommand("SELECT invtras.TransType, invtras.FromTran, invtras.ToTran, invtras.Qty, invtras.DOE, invmaster.sno AS invsno, invmaster.InvName FROM (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty FROM invtransactions12 WHERE (ToTran = @branchid) AND (DOE BETWEEN @d1 AND @d2) OR (DOE BETWEEN @d1 AND @d2) AND (FromTran = @branchid)) invtras INNER JOIN invmaster ON invtras.B_inv_sno = invmaster.sno WHERE (invmaster.sno = 1) ORDER BY invtras.DOE");
                        cmd.Parameters.AddWithValue("@branchid", drsale["BranchID"].ToString());
                        DateTime dt1 = GetLowDate(fromdate.AddDays(-1));
                        DateTime dt2 = GetLowDate(Todate);
                        cmd.Parameters.AddWithValue("@d1", dt1.AddHours(15));
                        cmd.Parameters.AddWithValue("@d2", dt2.AddHours(15));
                        DataTable dtinventoryDC = vdm.SelectQuery(cmd).Tables[0];
                        // cmd = new MySqlCommand("SELECT invtransactions12.TransType, invtransactions12.FromTran, invtransactions12.ToTran, invtransactions12.Qty, invtransactions12.DOE, invmaster.sno AS invsno,invmaster.InvName FROM invtransactions12 INNER JOIN invmaster ON invtransactions12.B_inv_sno = invmaster.sno WHERE (invtransactions12.ToTran = @branchid) AND (invtransactions12.DOE BETWEEN @d1 AND @d2) OR (invtransactions12.DOE BETWEEN @d1 AND @d2) AND (invtransactions12.FromTran = @branchid) ORDER BY invtransactions12.DOE");
                        cmd = new MySqlCommand("SELECT invtran.TransType, invtran.FromTran, invtran.ToTran, SUM(invtran.Qty) AS qty, invtran.DOE, invmaster.sno AS invsno, invmaster.InvName FROM (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty FROM invtransactions12 WHERE (ToTran = @branchid) AND (DOE BETWEEN @d1 AND @d2) OR (DOE BETWEEN @d1 AND @d2) AND (FromTran = @branchid)) invtran INNER JOIN invmaster ON invtran.B_inv_sno = invmaster.sno WHERE (invmaster.sno = 1) GROUP BY invtran.TransType");
                        cmd.Parameters.AddWithValue("@branchid", drsale["BranchID"].ToString());
                        DateTime dtmin = GetLowDate(fromdate.AddDays(-1));
                        DateTime dtmax = GetLowDate(ServerDateCurrentdate);
                        cmd.Parameters.AddWithValue("@d1", dtmin.AddHours(15));
                        cmd.Parameters.AddWithValue("@d2", dtmax.AddHours(15));
                        DataTable dtprevinventoryDC = vdm.SelectQuery(cmd).Tables[0];
                        DataRow newrow1 = Report.NewRow();
                        int oppcrates = 0;


                        // int Ctotcan40ltr = 0;
                        int Ctotcrates = 0;
                        //int Ctotcan20ltr = 0;
                        int Dtotcrates = 0;
                        //int Dtotcan20ltr = 0;
                        //int Dtotcan40ltr = 0;
                        //int prevCtotcan40ltr = 0;
                        int prevCtotcrates = 0;
                        //int prevCtotcan20ltr = 0;
                        int prevDtotcrates = 0;
                        //int prevDtotcan20ltr = 0;
                        //int prevDtotcan40ltr = 0;
                        //int prevDtotcan10ltr = 0;
                        //int prevCtotcan10ltr = 0;
                        //int Dtotcan10ltr = 0;
                        //int Ctotcan10ltr = 0;
                        newrow1["Sno"] = i++;
                        newrow1["Agent Code"] = drsale["BranchID"].ToString();
                        newrow1["Agent Name"] = drsale["BranchName"].ToString();
                        int.TryParse(drsale["Qty"].ToString(), out oppcrates);
                        foreach (DataRow drprev in dtprevinventoryDC.Rows)
                        {
                            if (drprev["TransType"].ToString() == "2")
                            {
                                if (drprev["invsno"].ToString() == "1")
                                {
                                    int prevDcrates = 0;
                                    int.TryParse(drprev["Qty"].ToString(), out prevDcrates);
                                    prevDtotcrates += prevDcrates;
                                }

                            }
                            if (drprev["TransType"].ToString() == "1" || drprev["TransType"].ToString() == "3")
                            {
                                if (drprev["invsno"].ToString() == "1")
                                {
                                    int prevCcrates = 0;
                                    int.TryParse(drprev["Qty"].ToString(), out prevCcrates);
                                    prevCtotcrates += prevCcrates;
                                }

                            }
                        }
                        foreach (DataRow dr in dtinventoryDC.Rows)
                        {
                            if (dr["TransType"].ToString() == "2")
                            {
                                if (dr["invsno"].ToString() == "1")
                                {
                                    int Dcrates = 0;
                                    int.TryParse(dr["Qty"].ToString(), out Dcrates);
                                    Dtotcrates += Dcrates;
                                }


                            }
                            if (dr["TransType"].ToString() == "1" || dr["TransType"].ToString() == "3")
                            {
                                if (dr["invsno"].ToString() == "1")
                                {
                                    int Ccrates = 0;
                                    int.TryParse(dr["Qty"].ToString(), out Ccrates);
                                    Ctotcrates += Ccrates;
                                }

                            }
                        }
                        oppcrates = oppcrates + prevCtotcrates - prevDtotcrates;
                        int CratesClo = oppcrates + Dtotcrates - Ctotcrates;
                        //int Can10ltrClo = oppcan10ltr + Dtotcan10ltr - Ctotcan10ltr;
                        //int Can20ltrClo = oppcan20ltr + Dtotcan20ltr - Ctotcan20ltr;
                        //int Can40ltrClo = oppcan40ltr + Dtotcan40ltr - Ctotcan40ltr;

                        newrow1["Crates Oppening"] = oppcrates;
                        newrow1["Issued Crates"] = Dtotcrates;
                        newrow1["Return Crates"] = Ctotcrates;
                        newrow1["Crates Balance"] = CratesClo;
                        int excess = 0;
                        totalopp += oppcrates;
                        totalissued += Dtotcrates;
                        totalreceived += Ctotcrates;
                        totalbalance += CratesClo;
                        if (CratesClo > Dtotcrates)
                        {
                            excess = CratesClo - Dtotcrates;
                            newrow1["Excess"] = excess;
                            totalexcess += excess;
                        }
                        else
                        {
                            newrow1["Excess"] = excess;
                        }

                        Report.Rows.Add(newrow1);
                        i++;
                    }
                    DataRow TotRow = Report.NewRow();
                    TotRow["Agent Name"] = "Total";
                    TotRow["Crates Oppening"] = totalopp;
                    TotRow["Issued Crates"] = totalissued;
                    TotRow["Return Crates"] = totalreceived;
                    TotRow["Crates Balance"] = totalbalance;
                    TotRow["Excess"] = totalexcess;
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
        }
    }
    protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grdReports, "Select$" + e.Row.RowIndex);
            e.Row.Attributes["style"] = "cursor:pointer";
        }
    }
    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int index = grdReports.SelectedRow.RowIndex;
        string headsno = grdReports.SelectedRow.Cells[0].Text;
        string name = grdReports.SelectedRow.Cells[1].Text;
        string country = grdReports.SelectedRow.Cells[2].Text;
        DateTime fromdate = new DateTime();
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
        //string message = "Row Index: " + index + "\\nName: " + name + "\\nCountry: " + country;
        try
        {
            vdm = new VehicleDBMgr();
            DataTable Report = new DataTable();
            string BranchID = ddlSalesOffice.SelectedValue;
            if (BranchID == "572")
            {
                BranchID = "7";
            }

            cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, SUM(indents_subtable.DeliveryQty) AS saleQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue,modifiedroutes.Sno AS routesno, modifidroutssubtab.BranchID, branchdata_2.BranchName, branchdata_2.flag FROM branchdata branchdata_2 RIGHT OUTER JOIN branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo ON branchdata_2.sno = modifidroutssubtab.BranchID LEFT OUTER JOIN indents_subtable INNER JOIN (SELECT IndentNo, I_date, Branch_id FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indt ON indents_subtable.IndentNo = indt.IndentNo ON modifidroutssubtab.BranchID = indt.Branch_id WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY branchdata.sno, routesno");
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(Todate.AddDays(-1)));
            DataTable dtroutecollection = vdm.SelectQuery(cmd).Tables[0];
            //cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno,branchdata_1.CollectionType,modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routesno, SUM(colltion.AmountPaid) AS amtpaid FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT Branchid, AmountPaid, PaidDate FROM collections WHERE (PaymentType <> 'Cheque') AND (PaidDate BETWEEN @d1 AND @d2)) colltion ON modifidroutssubtab.BranchID = colltion.Branchid WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY modifidroutssubtab.BranchID");
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routesno, SUM(colltion.AmountPaid) AS amtpaid, branchdata_1.SalesType, branchdata_2.CollectionType FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT  RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT Branchid, AmountPaid, PaidDate FROM collections WHERE (PaymentType <> 'Cheque') AND (PaidDate BETWEEN @d1 AND @d2)) colltion ON modifidroutssubtab.BranchID = colltion.Branchid INNER JOIN branchdata branchdata_2 ON modifidroutssubtab.BranchID = branchdata_2.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY modifidroutssubtab.BranchID");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            DataTable dtrouteamount = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno,modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routesno, SUM(colltion.AmountPaid) AS amtpaid FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT Branchid, AmountPaid, PaidDate FROM collections WHERE (PaymentType = 'Cheque') AND (VarifyDate BETWEEN @d1 AND @d2) AND (CheckStatus = 'V')) colltion ON modifidroutssubtab.BranchID = colltion.Branchid WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY modifidroutssubtab.BranchID");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            DataTable dtrouteChequeamount = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT Sno, SalesOfficeId, RouteId, AgentId, IndentDate, EntryDate, OppBalance, SaleQty, SaleValue, ReceivedAmount, ClosingBalance, DiffAmount FROM duetransactions WHERE (IndentDate BETWEEN @d1 AND @d2) AND (SalesOfficeId = @SOID)");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-2)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-2)));
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            DataTable dtOpp = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT AgentId, IndentDate, SaleQty, SaleValue, ReceivedAmount, ClosingBalance, DiffAmount, SaleValue - ReceivedAmount AS due FROM duetransactions WHERE        (IndentDate BETWEEN @d1 AND @d2) AND (SalesOfficeId = @SOID) AND (ClosingBalance <> 0) Order by AgentId");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-30)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            DataTable dtduefrom = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.Sno AS routesno, modifiedroutes.RouteName, modifidroutssubtab.BranchID,branchdata_2.BranchName AS Agentname, inventory_monitor.Inv_Sno, inventory_monitor.Qty FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN branchdata branchdata_2 ON modifidroutssubtab.BranchID = branchdata_2.sno LEFT OUTER JOIN inventory_monitor ON modifidroutssubtab.BranchID = inventory_monitor.BranchId WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID, inventory_monitor.Inv_Sno ORDER BY branchdata.sno, routesno");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            DataTable dtinvopp = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.Sno AS routesno, modifiedroutes.RouteName, invtran.B_inv_sno, SUM(invtran.Qty) AS deliveryqty,modifidroutssubtab.BranchID, branchdata_2.BranchName AS Agentname FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN branchdata branchdata_2 ON modifidroutssubtab.BranchID = branchdata_2.sno LEFT OUTER JOIN (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty, CBFromTran, CBToTran, DeliveryTime,CollectionTime, Remarks FROM invtransactions12 WHERE (TransType = 1) AND (DOE BETWEEN @d1 AND @d2) OR (TransType = 2) AND (DOE BETWEEN @d1 AND @d2)) invtran ON modifidroutssubtab.BranchID = invtran.ToTran WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID, invtran.B_inv_sno, branchdata_2.BranchName ORDER BY branchdata.sno, routesno");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            DateTime dt3 = GetLowDate(fromdate.AddDays(-1));
            DateTime dt4 = GetLowDate(fromdate);
            cmd.Parameters.AddWithValue("@d1", dt3.AddHours(15));
            cmd.Parameters.AddWithValue("@d2", dt4.AddHours(15));
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            DataTable dtInvdelivery = vdm.SelectQuery(cmd).Tables[0];
            Report = new DataTable();
            Report.Columns.Add("Sno");
            //Report.Columns.Add("Route Code");
            Report.Columns.Add("Agent Code");
            Report.Columns.Add("Agent Name");
            Report.Columns.Add("Opening Balance");
            Report.Columns.Add("Sale Qty").DataType = typeof(Double);
            Report.Columns.Add("Sale Value").DataType = typeof(Double);
            Report.Columns.Add("Received Amount").DataType = typeof(Double);
            Report.Columns.Add("Closing Amount").DataType = typeof(Double);
            Report.Columns.Add("Eligible Due").DataType = typeof(Double);
            Report.Columns.Add("Over Due").DataType = typeof(Double);
            Report.Columns.Add("Since From");
            Report.Columns.Add("Crates Balance").DataType = typeof(Double);
            Report.Columns.Add("Excess Crates").DataType = typeof(Double);
            Report.Columns.Add("Cans Balance").DataType = typeof(Double);
            Report.Columns.Add("Excess Cans").DataType = typeof(Double);

           
                double totalsaleqty = 0;
                double totalopp = 0;
                double totalsalevalue = 0;
                double totalamountpaid = 0;
                double totalclosingbalance = 0;
                double totaloverdue = 0;
                string collectiontype = "";
                DataRow newrow = Report.NewRow();
                newrow["Agent Name"] = name;
                Report.Rows.Add(newrow);
                DataRow newbreak = Report.NewRow();
                newbreak["Agent Name"] = "";
                Report.Rows.Add(newbreak);
                int i = 1;
                foreach (DataRow drsale in dtroutecollection.Select("routesno='" + headsno + "'"))
                {
                    DataRow newrow1 = Report.NewRow();
                    //collectiontype = drsale["CollectionType"].ToString();
                    newrow1["Sno"] = i++;
                    //newrow1["Route Code"] = drsale["routesno"].ToString();
                    newrow1["Agent Code"] = drsale["BranchID"].ToString();
                    newrow1["Agent Name"] = drsale["BranchName"].ToString();
                    double saleqty = 0;
                    double salevalue = 0;
                    double amountpaid = 0;
                    double chequeamountpaid = 0;
                    double oppbalance = 0;
                    int cratesopp = 0;
                    int cratesdelivered = 0;
                    int cansdelivered = 0;
                    int cansopp = 0;
                    foreach (DataRow dropp in dtOpp.Select("AgentId='" + drsale["BranchID"].ToString() + "'"))
                    {
                        double.TryParse(dropp["ClosingBalance"].ToString(), out oppbalance);
                        newrow1["Opening Balance"] = oppbalance;

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
                        collectiontype = drcoll["CollectionType"].ToString();


                    }
                    foreach (DataRow drChequecoll in dtrouteChequeamount.Select("BranchID='" + drsale["BranchID"].ToString() + "'"))
                    {
                        double.TryParse(drChequecoll["amtpaid"].ToString(), out chequeamountpaid);
                        totalamountpaid += Math.Round(chequeamountpaid, 2);
                        //collectiontype = drChequecoll["CollectionType"].ToString();


                    }
                    foreach (DataRow drinvopp in dtinvopp.Select("BranchID='" + drsale["BranchID"].ToString() + "'"))
                    {
                        int bal = 0;
                        if (drinvopp["Inv_Sno"].ToString() == "1")
                        {
                            int.TryParse(drinvopp["Qty"].ToString(), out bal);
                            cratesopp += bal;
                        }
                        if (drinvopp["Inv_Sno"].ToString() == "2")
                        {
                            int.TryParse(drinvopp["Qty"].ToString(), out bal);
                            cansopp += bal;
                        }
                        if (drinvopp["Inv_Sno"].ToString() == "3")
                        {
                            int.TryParse(drinvopp["Qty"].ToString(), out bal);
                            cansopp += bal;
                        }
                        if (drinvopp["Inv_Sno"].ToString() == "4")
                        {
                            int.TryParse(drinvopp["Qty"].ToString(), out bal);
                            cansopp += bal;
                        }
                    }
                    foreach (DataRow drinvdel in dtInvdelivery.Select("BranchID='" + drsale["BranchID"].ToString() + "'"))
                    {
                        int del = 0;
                        if (drinvdel["B_inv_sno"].ToString() == "1")
                        {
                            int.TryParse(drinvdel["deliveryqty"].ToString(), out del);
                            cratesdelivered += del;
                        }
                        if (drinvdel["B_inv_sno"].ToString() == "2")
                        {
                            int.TryParse(drinvdel["deliveryqty"].ToString(), out del);
                            cansdelivered += del;
                        }
                        if (drinvdel["B_inv_sno"].ToString() == "3")
                        {
                            int.TryParse(drinvdel["deliveryqty"].ToString(), out del);
                            cansdelivered += del;
                        }
                        if (drinvdel["B_inv_sno"].ToString() == "4")
                        {
                            int.TryParse(drinvdel["deliveryqty"].ToString(), out del);
                            cansdelivered += del;
                        }
                    }
                    double totamt = 0;
                    totamt = amountpaid + chequeamountpaid;
                    newrow1["Received Amount"] = Math.Round(totamt, 2);
                    double closing = 0;
                    closing = (oppbalance + salevalue) - (totamt);
                    newrow1["Closing Amount"] = Math.Round(closing, 2);
                    totalclosingbalance += Math.Round(closing, 2);
                    double eligibledue = 0;
                    double overdue = 0;
                    int status = 0;
                    if (closing > 0)
                    {
                        foreach (DataRow drduefrom in dtduefrom.Select("AgentId='" + drsale["BranchID"].ToString() + "'"))
                        {
                            double due = 0;
                            double.TryParse(drduefrom["ClosingBalance"].ToString(), out due);
                            if (due == closing)
                            {
                                if (status == 0)
                                {
                                    string ChangedTime1 = drduefrom["IndentDate"].ToString();
                                    DateTime dtDOE = Convert.ToDateTime(ChangedTime1);
                                    string dtdate1 = dtDOE.AddDays(1).ToString("dd MMM yy");
                                    newrow1["Since From"] = dtdate1;
                                    status = 1;
                                }
                            }


                        }
                    }
                    if (collectiontype == "CASH")
                    {
                        newrow1["Eligible Due"] = "0";
                        newrow1["Over Due"] = Math.Round(closing, 2);
                        totaloverdue += Math.Round(closing, 2);
                    }
                    if (collectiontype != "CASH")
                    {
                        eligibledue = 0;
                        newrow1["Eligible Due"] = "5000";
                        //if (closing > 5000)
                        //{
                            overdue = Math.Round(closing - eligibledue, 2);
                        //}
                        newrow1["Over Due"] = Math.Round(overdue, 2);
                        totaloverdue += Math.Round(overdue, 2);

                    }

                    newrow1["Crates Balance"] = cratesopp;
                    newrow1["Excess Crates"] = cratesopp - cratesdelivered;
                    newrow1["Cans Balance"] = cansopp;
                    newrow1["Excess Cans"] = cansopp - cansdelivered;
                    Report.Rows.Add(newrow1);

                }
                //DataRow TotRow = Report.NewRow();
                //TotRow["Agent Name"] = "Total";
                //TotRow["Oppening Balance"] = totalopp;
                //TotRow["Sale Qty"] = totalsaleqty;
                //TotRow["Sale Value"] = totalsalevalue;
                //TotRow["Received Amount"] = totalamountpaid;
                //TotRow["Closing Amount"] = totalclosingbalance;
                //TotRow["Over Due"] = totaloverdue;
                //Report.Rows.Add(TotRow);

                DataRow totalinventory = Report.NewRow();
                totalinventory["Agent Name"] = "TOTAL";
                double val = 0.0;
                foreach (DataColumn dc in Report.Columns)
                {
                    if (dc.DataType == typeof(Double))
                    {
                        val = 0.0;
                        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                        totalinventory[dc.ToString()] = val;
                    }
                }
                Report.Rows.Add(totalinventory);
                DataRow newbreak1 = Report.NewRow();
                newbreak1["Agent Name"] = "";
                Report.Rows.Add(newbreak1);
                GrdProducts.DataSource = Report;
            GrdProducts.DataBind();
            for (int ii = 0; ii < GrdProducts.Rows.Count; ii++)
            {
                GridViewRow dgvr = GrdProducts.Rows[ii];
                if (dgvr.Cells[1].Text != dgvr.Cells[2].Text)
                {
                    //GridViewRow compare = grdReports.Rows[ii + 1];
                    DateTime dtmydatetime2 = DateTime.Now;
                    string dt1 = dgvr.Cells[1].Text;
                    string dt2 = dgvr.Cells[2].Text;
                    if (dt1 == "&nbsp;")
                    {
                        if (dt2 != "&nbsp;")
                        {
                            if (dt2 == "Total")
                            {
                                dgvr.Cells[2].BackColor = Color.SandyBrown;
                            }
                            else
                            {
                                dgvr.Cells[2].BackColor = Color.LawnGreen;
                            }

                        }
                    }

                }
                string exces = dgvr.Cells[9].Text;
                if (exces == "&nbsp;")
                {
                }
                if (exces != "&nbsp;")
                {
                    if (exces != "0")
                    {
                        dgvr.Cells[9].BackColor = Color.Red;
                    }
                }


            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "PopupOpen();", true);


        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}