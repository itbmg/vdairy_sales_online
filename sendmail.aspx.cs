using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Net.Mail;


public partial class sendmail : System.Web.UI.Page
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
                lblTitle.Text = Session["TitleName"].ToString();
                rpt();
            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
        //string s = "SELECT SUM(indents_subtable.DeliveryQty) AS saleQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue, branchdata_1.sno AS BranchID, branchdata_1.BranchName FROM branchdata branchdata_2 RIGHT OUTER JOIN branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT        RefNo, Rank, LevelType, BranchID, CDate, EDate FROM            modifiedroutesubtable WHERE        (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo ON branchdata_2.sno = modifidroutssubtab.BranchID LEFT OUTER JOIN  indents_subtable INNER JOIN (SELECT        IndentNo, I_date, Branch_id FROM            indents WHERE        (I_date BETWEEN @starttime AND @endtime)) indt ON indents_subtable.IndentNo = indt.IndentNo ON modifidroutssubtab.BranchID = indt.Branch_id WHERE        (branchdata.SalesType IS NOT NULL) AND (indents_subtable.DeliveryQty <> 0) GROUP BY branchdata_1.sno, branchdata_1.BranchName ORDER BY branchdata.sno";
        //string c = "SELECT branchdata.BranchName, branchdata.sno, SUM(colltion.AmountPaid) AS AmountPaid FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM            modifiedroutesubtable WHERE        (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT        Branchid, AmountPaid, PaidDate  FROM            collections WHERE        (PaymentType <> 'Cheque') AND (PaidDate BETWEEN @d1 AND @d2)) colltion ON modifidroutssubtab.BranchID = colltion.Branchid WHERE        (branchdata.SalesType IS NOT NULL) GROUP BY branchdata.BranchName, branchdata.sno ORDER BY branchdata.sno";
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
    
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            DateTime fromdate = ServerDateCurrentdate;
            DateTime Todate = ServerDateCurrentdate;
            lbl_selfromdate.Text = fromdate.ToString("dd/MM/yyyy");
            lbl_selttodate.Text = Todate.ToString("dd/MM/yyyy");
            Session["filename"] = "TOTAL Salevalue REPORT";

            DataTable dispatch = new DataTable();
            dispatch.Columns.Add("BranchName");
            dispatch.Columns.Add("Dispatchqty");

            cmd = new MySqlCommand("SELECT   dispatch.sno, branchdata.BranchName, dispatch.Branch_Id, dispatch.BranchID, SUM(tripsubdata.Qty) AS dispatchqty, dispatch.CompanyId FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT  Sno, I_Date FROM tripdata WHERE  (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno WHERE (dispatch.CompanyId  = @branchid)  GROUP BY dispatch.Branch_Id");
            cmd.Parameters.AddWithValue("@branchid", "8012");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate).AddDays(-1));
            DataTable dtDispatchesbranches = vdm.SelectQuery(cmd).Tables[0];
            if (dtDispatchesbranches.Rows.Count > 0)
            {
                double dispqtytot = 0;
                foreach (DataRow drdisp in dtDispatchesbranches.Rows)
                {
                    DataRow dnewrow = dispatch.NewRow();
                    string bname = drdisp["BranchName"].ToString();
                    double dispqty = 0;
                    double.TryParse(drdisp["dispatchqty"].ToString(), out dispqty);
                    dnewrow["BranchName"] = bname;
                    dnewrow["Dispatchqty"] = Math.Round(dispqty, 2);
                    dispqtytot += dispqty;
                    dispatch.Rows.Add(dnewrow);
                }
                DataRow dnewrow1 = dispatch.NewRow();
                dnewrow1["BranchName"] = "Total";
                dnewrow1["Dispatchqty"] = Math.Round(dispqtytot, 2);
                dispatch.Rows.Add(dnewrow1);
            }
            //grddispqty.DataSource = dispatch;
            //grddispqty.DataBind();

            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
            cmd.Parameters.AddWithValue("@SuperBranch", "172");
            cmd.Parameters.AddWithValue("@SalesType", "21");
            cmd.Parameters.AddWithValue("@SalesType1", "26");
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];

            
            cmd = new MySqlCommand("SELECT SUM(indents_subtable.DeliveryQty) AS saleQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue, branchdata_1.sno AS BranchID, branchdata_1.BranchName FROM branchdata branchdata_2 RIGHT OUTER JOIN branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE  (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo ON branchdata_2.sno = modifidroutssubtab.BranchID LEFT OUTER JOIN  indents_subtable INNER JOIN (SELECT IndentNo, I_date, Branch_id FROM  indents WHERE  (I_date BETWEEN @starttime AND @endtime)) indt ON indents_subtable.IndentNo = indt.IndentNo ON modifidroutssubtab.BranchID = indt.Branch_id WHERE (branchdata.SalesType IS NOT NULL) AND (indents_subtable.DeliveryQty <> 0) GROUP BY branchdata_1.sno, branchdata_1.BranchName ORDER BY branchdata.sno");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(Todate).AddDays(-1));
           
            DataTable dtsalevalue = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, SUM(colltion.AmountPaid) AS AmountPaid FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM            modifiedroutesubtable WHERE        (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT        Branchid, AmountPaid, PaidDate  FROM  collections WHERE (PaymentType <> 'Cheque') AND (PaidDate BETWEEN @d1 AND @d2)) colltion ON modifidroutssubtab.BranchID = colltion.Branchid WHERE (branchdata.SalesType IS NOT NULL) GROUP BY branchdata.BranchName, branchdata.sno ORDER BY branchdata.sno");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            
            DataTable dtcollection = vdm.SelectQuery(cmd).Tables[0];
            DataTable Report = new DataTable();
            Report.Columns.Add("BranchName");
            Report.Columns.Add("SaleQuantity");
            Report.Columns.Add("SaleValue");
            Report.Columns.Add("Collection Amount");
            Report.Columns.Add("Balance");
            if (dtRoutedata.Rows.Count > 0)
            {
                double totalsaleqty = 0;
                double totalcollamt = 0;
                double totalsaleval = 0;
                double totalbalval = 0;
                foreach (DataRow dr in dtRoutedata.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    string salesofficeid = dr["sno"].ToString();
                    string branchname = dr["BranchName"].ToString();
                    newrow["BranchName"] = branchname;
                    double AmountPaid = 0;
                    double saleQty = 0;
                    double salevalue = 0;
                    foreach (DataRow drs in dtsalevalue.Select("BranchID='" + salesofficeid + "'"))
                    {
                        double.TryParse(drs["saleQty"].ToString(), out saleQty);
                        double.TryParse(drs["salevalue"].ToString(), out salevalue);
                        newrow["SaleQuantity"] = Math.Round(saleQty, 2);
                        totalsaleqty += saleQty;
                        newrow["SaleValue"] = Math.Round(salevalue, 2);
                        totalsaleval += salevalue;
                    }
                    foreach (DataRow drc in dtcollection.Select("sno='" + salesofficeid + "'"))
                    {
                        double.TryParse(drc["AmountPaid"].ToString(), out AmountPaid);
                        newrow["Collection Amount"] = Math.Round(AmountPaid, 2);
                        totalcollamt += AmountPaid;
                    }
                    newrow["Balance"] = Math.Round((salevalue - AmountPaid), 2);
                    totalbalval += (salevalue - AmountPaid);
                    Report.Rows.Add(newrow);
                }
                DataRow newrow1 = Report.NewRow();
                newrow1["BranchName"] = "Total";
                newrow1["SaleQuantity"] = Math.Round(totalsaleqty, 2);
                newrow1["SaleValue"] = Math.Round(totalsaleval, 2);
                newrow1["Collection Amount"] = Math.Round(totalcollamt, 2);
                newrow1["Balance"] = Math.Round(totalbalval, 2);
                Report.Rows.Add(newrow1);
            }
            dispatch.Merge(Report);
            grdtotal_dcReports.DataSource = dispatch;
            grdtotal_dcReports.DataBind();
            Session["xportdata"] = Report;

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }


    void getsaleanddispatch()
    {
        lblmsg.Text = "";
        pnlHide.Visible = true;
        Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
        VehicleDBMgr vdbmngr = new VehicleDBMgr();
        DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdbmngr.conn);
        DateTime FromDate = ServerDateCurrentdate;
        DateTime ToDate = ServerDateCurrentdate;
        lbl_selfromdate.Text = FromDate.ToString("dd/MM/yyyy");
        lbl_selttodate.Text = ToDate.ToString("dd/MM/yyyy");
        Session["filename"] = "TOTAL Salevalue REPORT";


        cmd = new MySqlCommand("SELECT   branchmappingtable.SuperBranch,branchdata.BranchName, ROUND(SUM(indents_subtable.DeliveryQty) ) AS DeliveryQty,ROUND(AVG(indents_subtable.DeliveryQty) ) AS AvgQty, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty) ) AS SaleValue, branchdata.sno FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE  (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch IN (172,1801,3625)) GROUP BY branchdata.sno");
        cmd.Parameters.AddWithValue("@d1", GetLowDate(FromDate).AddDays(-1));
        cmd.Parameters.AddWithValue("@d2", GetHighDate(ToDate).AddDays(-1));
        DataTable dtsalesoffices = vdbmngr.SelectQuery(cmd).Tables[0];
        DataTable MainReport = new DataTable();
        MainReport.Columns.Add("Branchname");
        MainReport.Columns.Add("SalesType");
        MainReport.Columns.Add("SalestypeId");
        MainReport.Columns.Add("AvgRate");
        MainReport.Columns.Add("SaleQty");
        MainReport.Columns.Add("SaleValue");
        if (dtsalesoffices.Rows.Count > 0)
        {
            foreach (DataRow drr in dtsalesoffices.Rows)
            {
                string BranchID = drr["sno"].ToString();
                string salesofficename = drr["BranchName"].ToString();
                DataTable dtyesterdayroutesale = new DataTable();
                DataTable dtyesterdaypaidamount = new DataTable();
                
                    cmd = new MySqlCommand("SELECT   modifiedroutes.Branchid As SuperBranch,modifiedroutes.RouteName, ROUND(SUM(indents_subtable.DeliveryQty),2) AS saleQty,ROUND(AVG(indents_subtable.DeliveryQty) ) AS AvgQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue, modifiedroutes.Sno AS routeid, modifidroutssubtab.BranchID AS bid, branchdata_2.BranchName, branchdata_2.flag, branchdata_1.sno AS BranchID, branchdata_2.SalesType AS SalesTypeId FROM branchdata branchdata_2 RIGHT OUTER JOIN branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT  RefNo, Rank, LevelType, BranchID, CDate, EDate FROM  modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo ON  branchdata_2.sno = modifidroutssubtab.BranchID LEFT OUTER JOIN indents_subtable INNER JOIN (SELECT  IndentNo, I_date, Branch_id FROM   indents WHERE  (I_date BETWEEN @starttime AND @endtime)) indt ON indents_subtable.IndentNo = indt.IndentNo ON  modifidroutssubtab.BranchID = indt.Branch_id WHERE   (branchdata.SalesType IS NOT NULL) AND (indents_subtable.DeliveryQty <> 0) AND (branchdata.sno = @BranchID) GROUP BY SalesTypeId ORDER BY  SalesTypeId");
                    cmd.Parameters.AddWithValue("@SOID", BranchID);
                    cmd.Parameters.AddWithValue("@BranchID", BranchID);
                    cmd.Parameters.AddWithValue("@starttime", GetLowDate(FromDate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@endtime", GetHighDate(ToDate.AddDays(-1)));
                    dtyesterdayroutesale = vdbmngr.SelectQuery(cmd).Tables[0];

                
                    cmd = new MySqlCommand("SELECT   modifiedroutes.Branchid As SuperBranch,branchdata.BranchName, branchdata.sno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routeid, SUM(colltion.AmountPaid) AS AmountPaid FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM   modifiedroutesubtable WHERE  (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT  Branchid, AmountPaid, PaidDate FROM   collections WHERE  (PaymentType <> 'Cheque') AND (PaidDate BETWEEN @d1 AND @d2)) colltion ON modifidroutssubtab.BranchID = colltion.Branchid WHERE (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY branchdata.sno");
                    cmd.Parameters.AddWithValue("@starttime", GetLowDate(FromDate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(FromDate).AddDays(-1));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(ToDate).AddDays(-1));
                    cmd.Parameters.AddWithValue("@SOID", BranchID);
                    cmd.Parameters.AddWithValue("@BranchID", BranchID);
                    dtyesterdaypaidamount = vdbmngr.SelectQuery(cmd).Tables[0];
                DataTable dtsalestype = new DataTable();
                
                    cmd = new MySqlCommand("SELECT  sno, salestype, flag, UserData_sno, status, rank, club_code FROM salestypemanagement where (status = 1) ORDER BY salestype DESC");
                    cmd.Parameters.AddWithValue("@BranchID", BranchID);
                    dtsalestype = vdbmngr.SelectQuery(cmd).Tables[0];
                DataTable dtAll = new DataTable();
                dtAll.Merge(dtyesterdayroutesale);

                DataView view = new DataView(dtAll);
                DataTable distincttable = view.ToTable(true, "SalesTypeId");
                DataTable DayReport = new DataTable();
                DataTable lastweakReport = new DataTable();
                DataTable LastMonthReport = new DataTable();
                DataTable LastYearReport = new DataTable();
                
                if (dtyesterdayroutesale.Rows.Count > 0)
                {
                    //double yesterdaysum = Convert.ToDouble(dtyesterdayroutesale.Compute("SUM(saleQty)", "SuperBranch=" + BranchID + ""));
                    double yesterdaysum = Convert.ToDouble(dtyesterdayroutesale.Compute("Sum(saleQty)", "").ToString());
                    double yesterdayvaluesum = Convert.ToDouble(dtyesterdayroutesale.Compute("Sum(salevalue)", "").ToString());
                }
                foreach (DataRow dr in distincttable.Rows)
                {
                    DataRow newrow = MainReport.NewRow();
                    string SalestypeId = dr["SalesTypeId"].ToString();
                    foreach (DataRow drsalestype in dtsalestype.Select("sno='" + SalestypeId + "'"))
                    {
                        string Salestype = drsalestype["salestype"].ToString();
                        double compare = 0;
                        //newrow["SalesType"] = Salestype;
                        foreach (DataRow dramount in dtyesterdayroutesale.Select("SalestypeId='" + SalestypeId + "'"))
                        {
                            double amount = 0;
                            double.TryParse(dramount["saleQty"].ToString(), out amount);
                            double QtyPercentage = 0; double QtytempPercentage = 0;
                            //QtytempPercentage = (amount / yesterdaysum) * 100;
                            QtyPercentage = Math.Round(QtytempPercentage);
                            // newrow["Yester Day %"] = Percentage;
                            newrow["SaleQty"] = amount + "(" + QtyPercentage + "%)";
                            double salevalue = 0;
                            double.TryParse(dramount["salevalue"].ToString(), out salevalue);
                            double valuepercentage = 0;
                           // valuepercentage = (salevalue / yesterdayvaluesum) * 100;
                            newrow["SaleValue"] = Math.Round(salevalue) + "(" + Math.Round(valuepercentage, 0) + "%)";
                            double AvgRate = salevalue / amount;
                            //newrow["Yester Day Avg"] = Math.Round(Avgqty );
                            newrow["AvgRate"] = Math.Round(AvgRate);
                            newrow["SalestypeId"] = SalestypeId;
                            newrow["SalesType"] = Salestype;
                            newrow["Branchname"] = salesofficename;
                            
                            //yesterdaygrandtotal += amount;
                        }
                        MainReport.Rows.Add(newrow);
                    }
                }
                DataRow newrow2 = MainReport.NewRow();
                newrow2["SalesType"] = "Total";
                //newrow2["SaleQty"] = Math.Round(yesterdaysum);//yesterdaysum;
                //newrow2["SaleValue"] = Math.Round(yesterdayvaluesum);//yesterdayvaluesum;
                //double Avg_Rate = yesterdayvaluesum / yesterdaysum;//yesterdayvaluesum;
                // newrow2["AvgRate"] = Math.Round(Avg_Rate);
                MainReport.Rows.Add(newrow2);
            }
            grdtotal_dcReports.DataSource = MainReport;
            grdtotal_dcReports.DataBind();
            Session["xportdata"] = MainReport;
        }
    }

    void rpt()
    {
        try
        {
            VehicleDBMgr vdbmngr = new VehicleDBMgr();
            DataTable Report = new DataTable();
            DataTable dtcategoryReport = new DataTable();
            dtcategoryReport.Columns.Add("Branchname");
            dtcategoryReport.Columns.Add("SalesType");
            dtcategoryReport.Columns.Add("SaleQty");
            dtcategoryReport.Columns.Add("SaleValue");
            dtcategoryReport.Columns.Add("Colectionamount");
            dtcategoryReport.Columns.Add("Balance");
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
            cmd.Parameters.AddWithValue("@SuperBranch", "172");
            cmd.Parameters.AddWithValue("@SalesType", "21");
            cmd.Parameters.AddWithValue("@SalesType1", "26");
            DataTable dtRoutedata = vdbmngr.SelectQuery(cmd).Tables[0];
            if (dtRoutedata.Rows.Count > 0)
            {
                foreach (DataRow drs in dtRoutedata.Rows)
                {
                    Report = new DataTable();
                    Report.Columns.Add("BranchName");
                    Report.Columns.Add("Route Name");
                    Report.Columns.Add("SalesType");
                    Report.Columns.Add("Saleqty");
                    Report.Columns.Add("Sale Value");
                    Report.Columns.Add("CollectionAmount");
                    Report.Columns.Add("Balance");
                    Report.Columns.Add("SalestypeId");
                    Report.Columns.Add("RouteId");
                    Report.Columns.Add("BranchID");
                    string branchid = drs["sno"].ToString();
                    string BranchName = drs["BranchName"].ToString();
                    DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdbmngr.conn);
                    DateTime dtFromdate = ServerDateCurrentdate.AddDays(-1);



                    cmd = new MySqlCommand("SELECT   modifiedroutes.RouteName, SUM(indents_subtable.DeliveryQty) AS saleQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue, modifiedroutes.Sno AS routeid, modifidroutssubtab.BranchID AS bid, branchdata_2.BranchName, branchdata_2.flag, branchdata_1.sno AS BranchID, branchdata_2.SalesType AS SalesTypeId FROM branchdata branchdata_2 RIGHT OUTER JOIN branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT  RefNo, Rank, LevelType, BranchID, CDate, EDate FROM  modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo ON  branchdata_2.sno = modifidroutssubtab.BranchID LEFT OUTER JOIN indents_subtable INNER JOIN (SELECT  IndentNo, I_date, Branch_id FROM   indents WHERE  (I_date BETWEEN @starttime AND @endtime)) indt ON indents_subtable.IndentNo = indt.IndentNo ON  modifidroutssubtab.BranchID = indt.Branch_id WHERE   (branchdata.SalesType IS NOT NULL) AND (indents_subtable.DeliveryQty <> 0) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY branchdata.sno, branchdata_2.RouteID");
                    cmd.Parameters.AddWithValue("@SOID", branchid);
                    cmd.Parameters.AddWithValue("@BranchID", branchid);
                    cmd.Parameters.AddWithValue("@starttime", GetLowDate(dtFromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@endtime", GetHighDate(dtFromdate.AddDays(-1)));
                    DataTable dtroutesale = vdbmngr.SelectQuery(cmd).Tables[0];
                    cmd = new MySqlCommand("SELECT   branchdata.BranchName, branchdata.sno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routeid, SUM(colltion.AmountPaid) AS AmountPaid FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM   modifiedroutesubtable WHERE  (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT  Branchid, AmountPaid, PaidDate FROM   collections WHERE  (PaymentType <> 'Cheque') AND (PaidDate BETWEEN @d1 AND @d2)) colltion ON modifidroutssubtab.BranchID = colltion.Branchid WHERE (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID ORDER BY branchdata.sno");
                    cmd.Parameters.AddWithValue("@starttime", GetLowDate(dtFromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(dtFromdate));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(dtFromdate));
                    cmd.Parameters.AddWithValue("@SOID", branchid);
                    cmd.Parameters.AddWithValue("@BranchID", branchid);
                    DataTable dtpaidamount = vdbmngr.SelectQuery(cmd).Tables[0];
                    cmd = new MySqlCommand("SELECT  sno, salestype, flag, UserData_sno, status, rank, club_code FROM salestypemanagement where (status = 1) ORDER BY salestype DESC");
                    cmd.Parameters.AddWithValue("@BranchID", branchid);
                    DataTable dtsalestype = vdbmngr.SelectQuery(cmd).Tables[0];
                    
                    
                    
                    
                   
                    int i = 1;
                    double grandavgsale = 0;
                    double Totdueamount = 0;
                    double Totcompassamount = 0;
                    double Totfhamount = 0;
                    double Totcateringamount = 0;
                    double Totinstituteamount = 0;
                    double Totdueagentamount = 0;
                    double TotCRagentamount = 0;
                    double Totcashamount = 0;
                    double Totduevalue = 0;
                    double Totcompassvalue = 0;
                    double Totfhvalue = 0;
                    double Totcateringvalue = 0;
                    double Totinstitutevalue = 0;
                    double Totdueagentvalue = 0;
                    double TotCRagentvalue = 0;
                    double Totcashvalue = 0;
                    if (dtroutesale.Rows.Count > 0)
                    {
                      
                        DataView view = new DataView(dtroutesale);
                        DataTable distinctroutename = view.ToTable(true, "RouteName", "routeid", "BranchID");
                        DataView view1 = new DataView(dtroutesale);
                        DataTable distincttable = view1.ToTable(true, "SalesTypeId");
                        string Salestype = "";
                        string routename = "";
                        string SalestypeId = "";
                        string routeid = "";
                        string BranchID = "";
                        foreach (DataRow drroute in distinctroutename.Rows)
                        {
                            int j = 1; double totalsalevalues = 0; double totalamount = 0; double totalsaleqty = 0;
                            foreach (DataRow drstype in distincttable.Rows)
                            {
                                DataRow[] drsalestype = dtroutesale.Select("SalesTypeId='" + drstype["SalesTypeId"].ToString() + "'AND RouteName='" + drroute["RouteName"].ToString() + "'");
                                if (drsalestype.Length > 0)
                                {
                                    foreach (DataRow drtype in dtsalestype.Select("sno='" + drstype["SalesTypeId"].ToString() + "'"))
                                    {
                                        Salestype = drtype["salestype"].ToString();
                                        SalestypeId = drtype["sno"].ToString();
                                        routename = drroute["RouteName"].ToString();
                                        routeid = drroute["routeid"].ToString();
                                        BranchID = drroute["BranchID"].ToString();
                                        j++;
                                    }
                                    DataRow newRow1 = Report.NewRow();
                                    foreach (DataRow drvalue in dtroutesale.Select("SalesTypeId='" + drstype["SalesTypeId"].ToString() + "' AND RouteName='" + drroute["RouteName"].ToString() + "'"))
                                    {
                                        string creates = "";
                                        float saleQty = 0;
                                        float.TryParse(drvalue["saleQty"].ToString(), out saleQty);
                                        float salevalue = 0;
                                        float.TryParse(drvalue["salevalue"].ToString(), out salevalue);
                                        float amount = 0;
                                        foreach (DataRow dramount in dtpaidamount.Select("BranchID='" + drvalue["bid"].ToString() + "' AND RouteName='" + drroute["RouteName"].ToString() + "'"))
                                        {
                                            amount = 0;
                                            float.TryParse(dramount["AmountPaid"].ToString(), out amount);
                                        }
                                        if (drvalue["SalesTypeId"].ToString() == "20")
                                        {
                                            Totcashvalue += salevalue;
                                            Totcashamount += amount;
                                        }
                                        if (drvalue["SalesTypeId"].ToString() == "36")
                                        {
                                            Totfhvalue += salevalue;
                                            Totfhamount += amount;
                                        }
                                        if (drvalue["SalesTypeId"].ToString() == "32")
                                        {
                                            Totcateringvalue += salevalue;
                                            Totcateringamount += amount;
                                        }
                                        if (drvalue["SalesTypeId"].ToString() == "37")
                                        {
                                            Totcompassvalue += salevalue;
                                            Totcompassamount += amount;
                                        }
                                        if (drvalue["SalesTypeId"].ToString() == "18")
                                        {
                                            Totinstitutevalue += salevalue;
                                            Totinstituteamount += amount;
                                        }
                                        if (drvalue["SalesTypeId"].ToString() == "33")
                                        {
                                            Totdueagentvalue += salevalue;
                                            Totdueagentamount += amount;
                                        }
                                        if (drvalue["SalesTypeId"].ToString() == "42")
                                        {
                                            TotCRagentvalue += salevalue;
                                            TotCRagentamount += amount;
                                        }
                                        totalsalevalues += salevalue;
                                        totalsaleqty += saleQty;
                                        totalamount += amount;
                                    }

                                    newRow1["BranchName"] = BranchName;
                                    newRow1["Sale Value"] = Math.Round(totalsalevalues, 2);
                                    newRow1["Saleqty"] = Math.Round(totalsaleqty, 2);
                                    newRow1["CollectionAmount"] = Math.Round(totalamount, 2);
                                    newRow1["SalesType"] = Salestype;
                                    newRow1["Route Name"] = routename;
                                    newRow1["RouteId"] = routeid;
                                    newRow1["SalestypeId"] = SalestypeId;
                                    newRow1["BranchID"] = BranchID;
                                    double diff = totalsalevalues - totalamount;
                                    newRow1["Balance"] = Math.Round(diff, 2);
                                    totalsalevalues = 0;
                                    totalamount = 0;
                                    totalsaleqty = 0;
                                    //routename = "";
                                    Report.Rows.Add(newRow1);
                                }
                            }
                        }
                    }
                    DataView salestypeview = new DataView(Report);
                    DataTable salestype = salestypeview.ToTable(true, "SalestypeId");
                    foreach (DataRow drms in salestype.Rows)
                    {
                        float Totalsale = 0; float amounttot = 0;
                        float Totalsaleqty = 0; float baltot = 0;
                        string type = "";
                        string Branch_id = "";
                        string SalestypeId = "";
                        string RouteId = "";
                        string Branchname = "";
                        foreach (DataRow dramount in Report.Select("SalestypeId='" + drms["SalestypeId"].ToString() + "'"))
                        {
                            float sale = 0;
                            float.TryParse(dramount["Sale Value"].ToString(), out sale);
                            Totalsale += sale;

                            float Saleqty = 0;
                            float.TryParse(dramount["Saleqty"].ToString(), out Saleqty);
                            Totalsaleqty += Saleqty;


                            float amount = 0;
                            float.TryParse(dramount["CollectionAmount"].ToString(), out amount);
                            amounttot += amount;

                           

                            type = dramount["SalesType"].ToString();
                            Branchname = dramount["BranchName"].ToString();
                        }
                        DataRow newRow = dtcategoryReport.NewRow();
                        newRow["SaleValue"] = Math.Round(Totalsale, 2);
                        newRow["Colectionamount"] = Math.Round(amounttot, 2);
                        newRow["SalesType"] = type;
                        newRow["SaleQty"] = Math.Round(Totalsaleqty, 2);
                        newRow["Balance"] = Math.Round((Totalsale - amounttot), 2);
                        newRow["Branchname"] = Branchname;
                        dtcategoryReport.Rows.Add(newRow);
                    }
                }

                grdtotal_dcReports.DataSource = dtcategoryReport;
                grdtotal_dcReports.DataBind();
                Session["xportdata"] = dtcategoryReport;
            }
        }
        catch
        {
        }
    }

    protected void grdtotal_dcReports_DataBinding(object sender, EventArgs e)
    {
        try
        {
            GridViewGroup First = new GridViewGroup(grdtotal_dcReports, null, "Branchname");
            
        }
        catch (Exception ex)
        {
            throw ex;
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

    protected void SendEmail(object sender, EventArgs e)
    {
        VehicleDBMgr vdbmngr = new VehicleDBMgr();
        DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdbmngr.conn);
        DateTime dtFromdate = ServerDateCurrentdate.AddDays(-1);
        string DATE = dtFromdate.ToString("dd/MM/yyyy");
        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                grdtotal_dcReports.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString());
                string senderID = "ddvyshnavi.in";// use sender's email id here..
                const string senderPassword = "Vyshnavi@123"; // sender password here...
                SmtpClient smtp = new SmtpClient
                {
                    Host = "czismtp.logix.in", // smtp server address here...
                    Port = 587,
                    //security type=tsl;
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 60000,
                };
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(senderID);
                string tomailid = "";
                string[] multimailid = tomailid.Split(',');
                foreach (string mailid in multimailid)
                {
                    mm.To.Add(new MailAddress(mailid));
                }

               // MailMessage mm = new MailMessage(senderID, "naveen15444@gmail.com");
                mm.Subject = "Sales Office Sale Value And Collection Details (" + DATE + ")";
                mm.Body = "Sales Office Sale Value And Collection Details:<hr />" + sw.ToString(); ;
                mm.IsBodyHtml = true;
                smtp.Send(mm);
                //SmtpClient smtp = new SmtpClient();
                //smtp.Host = "smtp.gmail.com";
                //smtp.EnableSsl = true;
                //System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                //NetworkCred.UserName = "sender@gmail.com";
                //NetworkCred.Password = "<password>";
                //smtp.UseDefaultCredentials = true;
                //smtp.Credentials = NetworkCred;
                //smtp.Port = 587;
                //smtp.Send(mm);
            }
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
}