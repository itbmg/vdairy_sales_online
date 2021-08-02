using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class DayEndReport : System.Web.UI.Page
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
            if (Session["salestype"].ToString() == "Plant")
            {
                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("BranchName");
                dtBranch.Columns.Add("sno");
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType)  ");
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
                cmd = new MySqlCommand("SELECT BranchName, sno FROM branchdata WHERE (sno = @BranchID)");
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
    DataTable RouteReport = new DataTable();
    DataTable CashPayReport = new DataTable();
    DataTable IOUReport = new DataTable();
    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            RouteReport = new DataTable();
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
            lbl_fromDate.Text = txtFromdate.Text;
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
            cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
            cmd.Parameters.AddWithValue("@SalesType", "21");
            cmd.Parameters.AddWithValue("@SalesType1", "26");
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];

            RouteReport = new DataTable();
            RouteReport.Columns.Add("SNo");
            RouteReport.Columns.Add("SalesOffice");
            RouteReport.Columns.Add("Sale Qty(Ltrs)").DataType = typeof(Double);
            RouteReport.Columns.Add("Sale Value").DataType = typeof(Double);
            RouteReport.Columns.Add("Collection").DataType = typeof(Double);
            RouteReport.Columns.Add("Cash Sale").DataType = typeof(Double);
            RouteReport.Columns.Add("Cash Sale Due").DataType = typeof(Double);
            RouteReport.Columns.Add("Credit Sale").DataType = typeof(Double);
            RouteReport.Columns.Add("Issued Crates").DataType = typeof(Double);
            RouteReport.Columns.Add("Received Crates").DataType = typeof(Double);
            RouteReport.Columns.Add("Issued Cans").DataType = typeof(Double);
            RouteReport.Columns.Add("Received Cans").DataType = typeof(Double);
            //RouteReport.Columns.Add("Payments");
            //RouteReport.Columns.Add("Bank Deposit");
            //RouteReport.Columns.Add("Due Amount");
            int i = 0;
            foreach (DataRow dr in dtRoutedata.Rows)
            {
                double totsale = 0;
                double totamtreceived = 0;
                double totdue = 0;
                double salevalue = 0;
                if (dr["sno"].ToString() != "925")
                {
                    DataRow newrow = RouteReport.NewRow();
                    newrow["SNo"] = i;
                    newrow["SalesOffice"] = dr["BranchName"].ToString();
                    cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, dispatch.DispName, tripdat.Sno AS tripid, SUM(indents_subtable.DeliveryQty) AS DeliveryQty,SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN indents_subtable ON tripdat.Sno = indents_subtable.DTripId WHERE (dispatch.DispMode IS NULL) AND (branchdata.sno = @BranchID) ORDER BY branchdata.sno");
                    cmd.Parameters.AddWithValue("@BranchID", dr["sno"].ToString());
                    if (dr["sno"].ToString() == "306" || dr["sno"].ToString() == "2749" )
                    {
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-2)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-2)));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    }

                    DataTable dtbranchdata = vdm.SelectQuery(cmd).Tables[0];
                    if (dtbranchdata.Rows.Count > 0)
                    {
                        string saleva = dtbranchdata.Rows[0]["salevalue"].ToString();
                        double DeliveryQty = 0;
                        double.TryParse(dtbranchdata.Rows[0]["DeliveryQty"].ToString(), out DeliveryQty);
                        double.TryParse(saleva, out salevalue);

                        newrow["Sale Qty(Ltrs)"] = Math.Round(DeliveryQty, 2);
                        newrow["Sale Value"] = Math.Round(salevalue, 2);
                        totsale = Math.Round(salevalue, 2);
                    }
                    if (dr["sno"].ToString() == "306" || dr["sno"].ToString() == "2749" )
                    {
                        cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, SUM(collectin.AmountPaid) AS collectionamount, collectin.PaymentType, collectin.Branchid, collectin.CheckStatus FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SuperBranch INNER JOIN (SELECT Branchid, AmountPaid, PaidDate, PaymentType, CheckStatus, tripId FROM collections WHERE (PaidDate BETWEEN @d1 AND @d2)) collectin ON branchmappingtable.SubBranch = collectin.Branchid WHERE (collectin.CheckStatus IS NULL) AND (collectin.tripId IS NULL) AND (branchdata.sno = @BranchID) ORDER BY branchdata.sno");
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                        cmd.Parameters.AddWithValue("@SOID", dr["sno"].ToString());
                        cmd.Parameters.AddWithValue("@BranchID", dr["sno"].ToString());
                        DataTable dtbranchcollection = vdm.SelectQuery(cmd).Tables[0];
                        double branchcol = 0;
                        if (dtbranchcollection.Rows.Count > 0)
                        {
                            string amtcollection = dtbranchcollection.Rows[0]["collectionamount"].ToString();
                            double.TryParse(amtcollection, out branchcol);
                        }
                        cmd = new MySqlCommand("SELECT  Sno, EmpId, Cdate, SUM(ReceivedAmount) AS receamt FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2) AND (BranchID = @BranchID)");
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-2)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-2)));
                        cmd.Parameters.AddWithValue("@BranchID", dr["sno"].ToString());
                        DataTable dttripcollection = vdm.SelectQuery(cmd).Tables[0];
                        double trpcollection = 0;

                        if (dttripcollection.Rows.Count > 0)
                        {
                            string amtcollection = dttripcollection.Rows[0]["receamt"].ToString();
                            double.TryParse(amtcollection, out trpcollection);


                        }
                        newrow["Collection"] = Math.Round(branchcol + trpcollection, 2);
                        totamtreceived = Math.Round(branchcol + trpcollection, 2);
                    }
                    if (dr["sno"].ToString() != "306" || dr["sno"].ToString() != "2749" )
                    {
                        cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, SUM(collectin.AmountPaid) AS collectionamount, collectin.PaymentType, collectin.Branchid, collectin.CheckStatus FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SuperBranch INNER JOIN (SELECT Branchid, AmountPaid, PaidDate, PaymentType, CheckStatus FROM            collections WHERE (PaidDate BETWEEN @d1 AND @d2)) collectin ON branchmappingtable.SubBranch = collectin.Branchid WHERE (branchdata.sno = @BranchID) AND (collectin.CheckStatus IS NULL) ORDER BY branchdata.sno");
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                        cmd.Parameters.AddWithValue("@SOID", dr["sno"].ToString());
                        cmd.Parameters.AddWithValue("@BranchID", dr["sno"].ToString());
                        DataTable dtbranchcollection = vdm.SelectQuery(cmd).Tables[0];
                        if (dtbranchcollection.Rows.Count > 0)
                        {
                            string amtcollection = dtbranchcollection.Rows[0]["collectionamount"].ToString();
                            double totcollection = 0;
                            double.TryParse(amtcollection, out totcollection);

                            newrow["Collection"] = Math.Round(totcollection, 2);
                            totamtreceived = Math.Round(totcollection, 2);
                        }
                    }
                   
                        cmd = new MySqlCommand("SELECT SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS due FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM  indents WHERE        (I_date BETWEEN @starttime AND @endtime)) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (modifiedroutes.BranchID = @BranchID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) AND (branchdata.CollectionType = 'DUE') OR (modifiedroutes.BranchID = @BranchID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND  (branchdata.CollectionType = 'DUE') GROUP BY modifiedroutes.BranchID");

                    cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate).AddDays(-1));
                    cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate).AddDays(-1));
                    cmd.Parameters.AddWithValue("@BranchID", dr["sno"].ToString());
                    DataTable dtdue = vdm.SelectQuery(cmd).Tables[0];

                    double creditdue = 0;
                    if (dtdue.Rows.Count > 0)
                    {
                        double.TryParse(dtdue.Rows[0]["due"].ToString(), out creditdue);
                        creditdue = Math.Round(creditdue, 2);
                    }
                    double cashsale = 0;
                    cashsale = salevalue - creditdue;
                    cashsale = Math.Round(cashsale, 2);
                    newrow["Credit Sale"] = creditdue;
                    newrow["Cash Sale"] = cashsale;

                    cmd = new MySqlCommand("SELECT SUM(tripinvdata.Qty) AS Issued, SUM(tripinvdata.Remaining) AS Received, invmaster.InvName, invmaster.sno FROM tripdata INNER JOIN tripinvdata ON tripdata.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (tripdata.BranchID = @BranchID) GROUP BY invmaster.InvName, invmaster.sno");
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate).AddDays(-1));
                    cmd.Parameters.AddWithValue("@BranchID", dr["sno"].ToString());
                    DataTable dtInventoryIssue = vdm.SelectQuery(cmd).Tables[0];
                    if (dtInventoryIssue.Rows.Count > 0)
                    {
                        int issuedcrates = 0;
                        int issuedcans = 0;
                        int receiveddcrates = 0;
                        int receivedcans = 0;
                        foreach (DataRow dropp in dtInventoryIssue.Rows)
                        {
                            if (dropp["sno"].ToString() == "1")
                            {
                                int isscrates = 0;
                                int.TryParse(dropp["Issued"].ToString(), out isscrates);
                                issuedcrates += isscrates;
                                int reccrates = 0;
                                int.TryParse(dropp["Received"].ToString(), out reccrates);
                                receiveddcrates += reccrates;
                            }
                            if (dropp["sno"].ToString() == "2" || dropp["sno"].ToString() == "3" || dropp["sno"].ToString() == "4")
                            {
                                int isscans = 0;
                                int.TryParse(dropp["Issued"].ToString(), out isscans);
                                issuedcans += isscans;
                                int reccans = 0;
                                int.TryParse(dropp["Received"].ToString(), out reccans);
                                receivedcans += reccans;
                            }
                        }
                        newrow["Issued Crates"] = issuedcrates;
                        newrow["Issued Cans"] = issuedcans;
                        newrow["Received Crates"] = receiveddcrates;
                        newrow["Received Cans"] = receivedcans;
                    }
                    cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, brnchprdt.Rank, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue, indent.IndentType,productsdata.ProductName, branchdata.sno AS BSno, branchdata.BranchName, productsdata.Units, productsdata.sno, products_category.Categoryname,invmaster.Qty FROM indents_subtable INNER JOIN (SELECT IndentNo, Branch_id, Status, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON indents_subtable.IndentNo = indent.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN (SELECT branch_sno, product_sno, unitprice, flag, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno RIGHT OUTER JOIN modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno ON indent.Branch_id = branchdata.sno WHERE (modifiedroutes.BranchID = @BranchID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @endtime) OR (modifiedroutes.BranchID = @BranchID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) and (branchdata.Due_Limit_Days = 0) GROUP BY branchdata.sno ORDER BY branchdata.sno");
                    cmd.Parameters.AddWithValue("@BranchID", dr["sno"].ToString());
                    cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
                    DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
                    cmd = new MySqlCommand("SELECT SUM(collction.AmountPaid) AS amountpaid, branchdata.sno, branchdata.BranchName FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT Branchid, UserData_sno, AmountPaid, PaidDate, PaymentType, tripId, CheckStatus, VarifyDate, ChequeDate FROM collections WHERE (VarifyDate BETWEEN @d1 AND @d2) AND (CheckStatus = 'V')) collction ON modifiedroutesubtable.BranchID = collction.Branchid INNER JOIN branchdata ON collction.Branchid = branchdata.sno WHERE (modifiedroutes.BranchID = @BranchID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @d2) OR (modifiedroutes.BranchID = @BranchID) AND (modifiedroutesubtable.EDate > @d1) AND (modifiedroutesubtable.CDate <= @d2) GROUP BY branchdata.sno, branchdata.BranchName order by branchdata.sno");
                    //cmd = new MySqlCommand("SELECT SUM(collction.AmountPaid) AS amountpaid, branchdata.sno, branchdata.BranchName FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT Branchid, UserData_sno, AmountPaid, PaidDate, PaymentType, tripId, CheckStatus FROM collections WHERE (PaidDate BETWEEN @d1 AND @d2) AND (CheckStatus IS NULL) OR (PaidDate BETWEEN @d1 AND @d2) AND (CheckStatus = 'V')) collction ON modifiedroutesubtable.BranchID = collction.Branchid INNER JOIN branchdata ON collction.Branchid = branchdata.sno WHERE (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @d1) OR (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate > @d1) AND (modifiedroutesubtable.CDate <= @d1) GROUP BY branchdata.sno, branchdata.BranchName");
                    //cmd = new MySqlCommand("SELECT SUM(collections.AmountPaid) AS amountpaid, branchdata_1.sno, branchdata_1.BranchName FROM modifiedroutes INNER JOIN branchdata ON modifiedroutes.BranchID = branchdata.sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata branchdata_1 ON modifiedroutesubtable.BranchID = branchdata_1.sno INNER JOIN collections ON branchdata_1.sno = collections.Branchid WHERE (modifiedroutes.BranchID = @BranchID) AND (modifiedroutes.Sno = @RouteID) AND (collections.PaidDate BETWEEN @d1 AND @d2) OR (branchdata.SalesOfficeID = @BranchID) AND (modifiedroutes.Sno = @RouteID) AND (collections.PaidDate BETWEEN @d1 AND @d2) GROUP BY branchdata.sno, branchdata_1.BranchName"); 
                    cmd.Parameters.AddWithValue("@BranchID", dr["sno"].ToString());
                    //cmd.Parameters.AddWithValue("@RouteID", routeid);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                    DataTable dtcollection = vdm.SelectQuery(cmd).Tables[0];

                    cmd = new MySqlCommand("SELECT SUM(collction.AmountPaid) AS amountpaid, branchdata.sno, branchdata.BranchName FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT Branchid, UserData_sno, AmountPaid, PaidDate, PaymentType, tripId, CheckStatus, VarifyDate, ChequeDate FROM collections WHERE (PaidDate BETWEEN @d1 AND @d2) AND (CheckStatus IS NULL)) collction ON modifiedroutesubtable.BranchID = collction.Branchid INNER JOIN branchdata ON collction.Branchid = branchdata.sno WHERE (modifiedroutes.BranchID = @BranchID)  AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @d2) OR (modifiedroutes.BranchID = @BranchID) AND (modifiedroutesubtable.EDate > @d1) AND (modifiedroutesubtable.CDate <= @d2) GROUP BY branchdata.sno, branchdata.BranchName order by branchdata.sno");
                    cmd.Parameters.AddWithValue("@BranchID", dr["sno"].ToString());
                    //cmd.Parameters.AddWithValue("@RouteID", routeid);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                    DataTable dtCashcollection = vdm.SelectQuery(cmd).Tables[0];
                    DataView view = new DataView(dtble);
                    DataTable distincttable = view.ToTable(true, "BranchName", "BSno");
                    double cashsaledue = 0;

                    foreach (DataRow branch in distincttable.Rows)
                    {
                        float amountpaid = 0;
                        float totamountpaid = 0;
                        foreach (DataRow drdtclubtotal in dtcollection.Select("sno='" + branch["BSno"].ToString() + "'"))
                        {
                            float.TryParse(drdtclubtotal["amountpaid"].ToString(), out amountpaid);
                        }
                        float cashamountpaid = 0;
                        foreach (DataRow drdtclubtotal in dtCashcollection.Select("sno='" + branch["BSno"].ToString() + "'"))
                        {
                            float.TryParse(drdtclubtotal["amountpaid"].ToString(), out cashamountpaid);
                        }
                        totamountpaid = amountpaid + cashamountpaid;

                        foreach (DataRow drb in dtble.Rows)
                        {
                            if (branch["BranchName"].ToString() == drb["BranchName"].ToString())
                            {
                                double svalue = 0;
                                double.TryParse(drb["salevalue"].ToString(), out svalue);
                                if (svalue < totamountpaid)
                                {

                                }
                                else
                                {
                                    double due = 0;
                                    due = svalue - totamountpaid;
                                    due = Math.Round(due, 2);
                                    cashsaledue += due;
                                }
                            }
                        }
                    }
                    
                        cashsaledue = Math.Round(cashsaledue, 2);
                        newrow["Cash Sale Due"] = cashsaledue;

                    // cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, dispatch.DispName, SUM(collectin.AmountPaid) AS collectionamount, collectin.PaymentType, collectin.Branchid, collectin.CheckStatus FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutesubtable ON dispatch_sub.Route_id = branchroutesubtable.RefNo INNER JOIN (SELECT Branchid, AmountPaid, PaidDate, PaymentType, CheckStatus FROM collections WHERE (PaidDate BETWEEN @d1 AND @d2)) collectin ON branchroutesubtable.BranchID = collectin.Branchid WHERE (branchdata_1.SalesOfficeID = @SOID) AND (dispatch.DispMode IS NULL) AND (collectin.CheckStatus IS NULL) OR (branchdata.sno = @BranchID) AND (dispatch.DispMode IS NULL) AND (collectin.CheckStatus IS NULL) ORDER BY branchdata.sno");
                    //cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, SUM(collectin.AmountPaid) AS collectionamount, collectin.PaymentType, collectin.Branchid, collectin.CheckStatus FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SuperBranch INNER JOIN (SELECT Branchid, AmountPaid, PaidDate, PaymentType, CheckStatus FROM collections WHERE (PaidDate BETWEEN @d1 AND @d2)) collectin ON branchmappingtable.SubBranch = collectin.Branchid WHERE (branchdata_1.SalesOfficeID = @SOID) AND (collectin.CheckStatus IS NULL) AND (collectin.PaymentType <> @pt) OR (collectin.CheckStatus IS NULL) AND (branchdata.sno = @BranchID) AND (collectin.PaymentType <> @pt) ORDER BY branchdata.sno");

                    ////cmd = new MySqlCommand("SELECT Sno, BranchID, CashTo, DOE, VocherID, Remarks, Amount, Approvedby, SUM(ApprovedAmount) AS paidamount, ApprovalRemarks, Status, Created_by, Modify_by,Empid, onNameof, CloBal, VoucherType, CashierRemarks, ForceApproval, ReceivedAmount, DueAmount, AccountType, AgentID FROM cashpayables WHERE (BranchID = @BranchID) AND (DOE BETWEEN @d1 AND @d2) AND (VoucherType <> @vtype) AND (CashTo <> @cto)");
                    ////cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                    ////cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                    ////cmd.Parameters.AddWithValue("@BranchID", dr["sno"].ToString());
                    ////cmd.Parameters.AddWithValue("@vtype", "Credit");
                    ////cmd.Parameters.AddWithValue("@cto", "Bank Deposit");
                    ////DataTable dtbranchpayments = vdm.SelectQuery(cmd).Tables[0];
                    ////if (dtbranchpayments.Rows.Count > 0)
                    ////{
                    ////    string amtcollection = dtbranchpayments.Rows[0]["paidamount"].ToString();
                    ////    double totcollection = 0;
                    ////    double.TryParse(amtcollection, out totcollection);

                    ////    newrow["Payments"] = Math.Round(totcollection, 2);
                    ////}
                    ////cmd = new MySqlCommand("SELECT Sno, BranchID, CashTo, DOE, VocherID, Remarks, Amount, Approvedby, SUM(ApprovedAmount) AS bankdeposit, ApprovalRemarks, Status, Created_by, Modify_by, Empid, onNameof, CloBal, VoucherType, CashierRemarks, ForceApproval, ReceivedAmount, DueAmount, AccountType, AgentID FROM cashpayables WHERE (BranchID = @BranchID) AND (DOE BETWEEN @d1 AND @d2) AND (CashTo = @cto)");
                    ////cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                    ////cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                    ////cmd.Parameters.AddWithValue("@BranchID", dr["sno"].ToString());
                    ////cmd.Parameters.AddWithValue("@vtype", "Credit");
                    ////cmd.Parameters.AddWithValue("@cto", "Bank Deposit");
                    ////DataTable dtbankdeposit = vdm.SelectQuery(cmd).Tables[0];
                    ////if (dtbankdeposit.Rows.Count > 0)
                    ////{
                    ////    string amtcollection = dtbankdeposit.Rows[0]["bankdeposit"].ToString();
                    ////    double totcollection = 0;
                    ////    double.TryParse(amtcollection, out totcollection);

                    ////    newrow["Bank Deposit"] = Math.Round(totcollection, 2);
                    ////}
                    ////newrow["Due Amount"] = Math.Round(totsale - totamtreceived, 2);


                    RouteReport.Rows.Add(newrow);
                    i++;
                }

            }
            DataRow totalinventory = RouteReport.NewRow();
            totalinventory["SalesOffice"] = "TOTAL";
            double val = 0.0;
            foreach (DataColumn dc in RouteReport.Columns)
            {
                if (dc.DataType == typeof(Double))
                {
                    val = 0.0;
                    double.TryParse(RouteReport.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                    totalinventory[dc.ToString()] = val;
                }
            }
            RouteReport.Rows.Add(totalinventory);
            grdDenomination.DataSource = RouteReport;
            grdDenomination.DataBind();

        }
        catch
        {

        }
    }
}