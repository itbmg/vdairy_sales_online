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
using System.Xml;

public partial class Puff_Leakes : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    DataTable Report = new DataTable();
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
                txttodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
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
    //void GetReport()
    //{
    //    vdm = new VehicleDBMgr();
    //    Report.Columns.Add("Date");
    //    Report.Columns.Add("DESPATCH QTY");
    //    Report.Columns.Add("LEAK QTY");
    //    Report.Columns.Add("LEAK%");
    //    DateTime fromdate = DateTime.Now;
    //    string[] datestrig = txtdate.Text.Split(' ');
    //    lblmsg.Text = "";
    //    lblDispatchName.Text = "";
    //    if (datestrig.Length > 1)
    //    {
    //        if (datestrig[0].Split('-').Length > 0)
    //        {
    //            string[] dates = datestrig[0].Split('-');
    //            string[] times = datestrig[1].Split(':');
    //            fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
    //        }
    //    }
    //    DateTime todate = DateTime.Now;
    //    string[] datestrig1 = txttodate.Text.Split(' ');
    //    lblDispatchName.Text = "";
    //    if (datestrig1.Length > 1)
    //    {
    //        if (datestrig1[0].Split('-').Length > 0)
    //        {
    //            string[] dates = datestrig1[0].Split('-');
    //            string[] times = datestrig1[1].Split(':');
    //            todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
    //        }
    //    }
    //    lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
    //    lblTodate.Text = todate.ToString("dd/MMM/yyyy");
    //    try
    //    {
    //        lblDispatchName.Text = ddlSalesOffice.SelectedItem.Text;
    //        cmd = new MySqlCommand("SELECT   ROUND(SUM(T3.Expr1), 2) AS QTY, ROUND(SUM(T4.LeakQty), 2) AS leakqty, T3.AssignDate, T3.Tripdata_sno FROM (SELECT t1.ATripid, T2.Expr1, T2.Tripdata_sno, t1.AssignDate FROM (SELECT tripdata.Sno AS ATripid, dispatch.BranchID, tripdata.AssignDate FROM tripdata INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno WHERE (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @BranchID) GROUP BY tripdata.Sno) t1 LEFT OUTER JOIN (SELECT SUM(Qty) AS Expr1, Tripdata_sno FROM tripsubdata GROUP BY Tripdata_sno) T2 ON t1.ATripid = T2.Tripdata_sno) T3 LEFT OUTER JOIN (SELECT SUM(LeakQty) AS LeakQty, SUM(LeakQty) / 100 AS LEAKperc, TripId, DOE FROM branchleaktrans WHERE (BranchID = @BranchID) GROUP BY TripId) T4 ON T3.Tripdata_sno = T4.TripId GROUP BY T3.AssignDate");
    //        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
    //        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
    //        cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
    //        DataTable dtpuffleaks = vdm.SelectQuery(cmd).Tables[0];
    //        if (dtpuffleaks.Rows.Count > 0)
    //        {
    //            double totaldispatchqty = 0;
    //            double totalleak = 0;
    //            double totalleakpercen = 0;
    //            foreach (DataRow dr in dtpuffleaks.Rows)
    //            {
    //                DateTime Invoice = Convert.ToDateTime(dr["AssignDate"].ToString());
    //                string Invoicdate = Invoice.ToString("dd/MM/yyyy");
    //                DataRow newrow = Report.NewRow();
    //                newrow["Date"] = Invoicdate;
    //                double disp = 0;
    //                double.TryParse(dr["QTY"].ToString(), out disp);
    //                totaldispatchqty += disp;
    //                newrow["DESPATCH QTY"] = Math.Round(disp, 2); ;
    //                double leak = 0;
    //                double.TryParse(dr["LeakQty"].ToString(), out leak);
    //                totalleak += leak;
    //                newrow["LEAK QTY"] = Math.Round(leak, 2); ;
    //                double leakper = 0;
    //                leakper = leak / 100;
    //                totalleakpercen += leakper;
    //                newrow["LEAK%"] = Math.Round(leakper, 2);
    //                Report.Rows.Add(newrow);
    //            }
    //            DataRow salesreport1 = Report.NewRow();
    //            salesreport1["Date"] = "Total";
    //            salesreport1["DESPATCH QTY"] = Math.Round(totaldispatchqty, 2);
    //            salesreport1["LEAK QTY"] = Math.Round(totalleak, 2);
    //            double totd = Math.Round(totalleak, 2);
    //            double totl = Math.Round(totaldispatchqty, 2);
    //            double totp = totd / totl * 100;
    //            salesreport1["LEAK%"] = Math.Round(totp, 2);
    //            Report.Rows.Add(salesreport1);
    //            grdReports.DataSource = Report;
    //            grdReports.DataBind();
    //            pnlHide.Visible = true;
    //        }
    //    }
    //    catch(Exception ex)
    //    {
    //        lblmsg.Text=ex.Message;
    //    }
    //}
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            DataTable Report = new DataTable();
            Session["RouteName"] = "Puff Leaks Report";
            //Session["IDate"] = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime dtDate = DateTime.Now;
            string Time = dtDate.ToString("dd/MMM/yyyy");
            //lblDate.Text = Time;
            lblDispatchName.Text = ddlSalesOffice.SelectedItem.Text;
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            string[] datestrig = txtdate.Text.Split(' ');
            lblmsg.Text = "";
            lblDispatchName.Text = "";
            string[] fromdatestrig = txtdate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            DateTime todate = DateTime.Now;
            string[] datestrig1 = txttodate.Text.Split(' ');
            lblDispatchName.Text = "";
            if (datestrig1.Length > 1)
            {
                if (datestrig1[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig1[0].Split('-');
                    string[] times = datestrig1[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            lblFromDate.Text = fromdate.ToString("dd/MMM/yyyy");
            lblTodate.Text = todate.ToString("dd/MMM/yyyy");
            #region leaks
            lblpuffLeak.Text = "PuffLeaks For  " + ddlSalesOffice.SelectedItem.Text;
            lblDispatchName.Text = "" + ddlSalesOffice.SelectedItem.Text;
            ////cmd = new MySqlCommand("SELECT tripdat.Sno, DATE_FORMAT(tripdat.I_Date, '%d %b %y') AS DATE, leakages.TotalLeaks, productsdata.UnitPrice, productsdata.ProductName, leakages.ProductID,leakages.LeakQty, leakages.TripID, leakages.VarifyStatus FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN leakages ON tripdat.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (leakages.VarifyStatus <> 'NULL') AND (dispatch.BranchID = @sobranchid)"); 
            //Ravindra 30/01/2017
            cmd = new MySqlCommand("SELECT   Leaks.LeakQty, Leaks.UnitPrice, Leaks.ProdId, Leaks.ProductName,DATE_FORMAT(ff.I_Date, '%d %b %y') AS DATE, ff.DispName, Leaks.Sno FROM  (SELECT branchleaktrans.LeakQty,branchleaktrans.ProdId, productsdata.UnitPrice, productsdata.ProductName, productsdata.sno AS productsno, tripdata_1.Sno FROM tripdata tripdata_1 INNER JOIN branchleaktrans ON tripdata_1.Sno = branchleaktrans.TripId INNER JOIN productsdata ON branchleaktrans.ProdId = productsdata.sno WHERE (tripdata_1.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno, I_Date FROM  (SELECT dispatch.DispName, tripdata.Sno, dispatch.sno AS DespSno, tripdata.I_Date FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @BranchID)) TripInfo) ff ON ff.Sno = Leaks.Sno ORDER BY ff.I_Date");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtpuffleaks = vdm.SelectQuery(cmd).Tables[0];
            Report = new DataTable();
            Report.Columns.Add("Date");
            DataView view = new DataView(dtpuffleaks);
            DataTable distinctProduct = view.ToTable(true, "ProductName", "ProdId");
            foreach (DataRow dr in distinctProduct.Rows)
            {
                Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
            }
            Report.Columns.Add("Total  Leaks").DataType = typeof(Double);
            Report.Columns.Add("Despatch Qty").DataType = typeof(Double);
            Report.Columns.Add("Leaks %");
            //Report.Columns.Add("Total  Amount").DataType = typeof(float);
            DataTable distincttable = view.ToTable(true, "Date", "Sno");
            int i = 1;
            double Tot_leaks = 0;
            double TotldespQty = 0;
            DataTable dtproductdispatch = new DataTable();
            DataTable dtproductdispatch1 = new DataTable();
            double totalqty = 0;
            cmd = new MySqlCommand("SELECT  t2.ProductId,ROUND(SUM(t2.Qty),2) AS QTY,t2.ProductName,  t1.Tripdata_sno, t1.Tripdata_sno AS Expr1, t1.AssignDate, t1.BranchID FROM (SELECT triproutes.Tripdata_sno, tripdata.AssignDate, dispatch.BranchID, tripdata.Sno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @branchid)) t1 INNER JOIN (SELECT tripsubdata.ProductId, tripsubdata.Qty, productsdata.ProductName, tripdata_1.Sno FROM tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE (tripdata_1.AssignDate BETWEEN @d1 AND @d2) AND (tripdata_1.Status <> 'C') GROUP BY tripsubdata.ProductId, tripdata_1.Sno) t2 ON t1.Tripdata_sno = t2.Sno GROUP BY t2.ProductId");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            dtproductdispatch = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow branch in distincttable.Rows)
            {
                double Totleaks = 0;
                DataRow newrow = Report.NewRow();
                string IndentDate = branch["Date"].ToString();
                DateTime dtIndentDate = Convert.ToDateTime(IndentDate);
                cmd = new MySqlCommand("SELECT t2.ProductId, ROUND(SUM(t2.Qty), 2) AS disqty, t1.BranchID FROM (SELECT triproutes.Tripdata_sno, tripdata.AssignDate, dispatch.BranchID, tripdata.Sno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE        (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @branchid)) t1 INNER JOIN (SELECT        tripsubdata.ProductId, tripsubdata.Qty, productsdata.ProductName, tripdata_1.Sno FROM tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE        (tripdata_1.AssignDate BETWEEN @d1 AND @d2) AND (tripdata_1.Status <> 'C') GROUP BY tripsubdata.ProductId, tripdata_1.Sno) t2 ON t1.Tripdata_sno = t2.Sno");
                cmd.Parameters.AddWithValue("@Branchid", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(dtIndentDate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(dtIndentDate));
                DataTable dtdispatch = vdm.SelectQuery(cmd).Tables[0];
                string ChangedTime = dtIndentDate.AddDays(1).ToString("dd/MMM/yyyy");
                newrow["Date"] = ChangedTime;
                double Total = 0;
                foreach (DataRow dr in dtpuffleaks.Select("Sno='" + branch["Sno"].ToString() + "'"))
                {
                    if (branch["Date"].ToString() == dr["Date"].ToString())
                    {
                        double TotalLeaks = 0;
                        double.TryParse(dr["LeakQty"].ToString(), out TotalLeaks);
                        if (TotalLeaks == 0)
                        {
                        }
                        else
                        {
                            newrow[dr["ProductName"].ToString()] = TotalLeaks;
                            //Total += TotalLeaks * UnitCost;
                            Totleaks += TotalLeaks;
                            Tot_leaks += TotalLeaks;
                        }
                        //}
                    }
                }
                newrow["Total  Leaks"] = Totleaks;
                double avg = 0;
                if (dtdispatch.Rows.Count > 0)
                {
                    double dispqty = 0;
                    double.TryParse(dtdispatch.Rows[0]["disqty"].ToString(), out dispqty);
                    if (dispqty == 0)
                    {
                    }
                    else
                    {
                        avg = (Totleaks / dispqty) * 100;
                    }
                    newrow["Leaks %"] = Math.Round(avg, 2);
                    newrow["Despatch Qty"] = Math.Round(dispqty, 2);
                    TotldespQty += dispqty;

                }
                else
                {
                    //avg = 0;
                    newrow["Leaks %"] = avg;
                    newrow["Despatch Qty"] = "";

                }
                //newrow["Total  Amount"] = Total;
                Report.Rows.Add(newrow);
            }

            double Tot_per = 0;
            Tot_per = Tot_leaks / TotldespQty;
            Tot_per = Tot_per * 100;
            //lblMessage.Text = "Total Leak % = " + Tot_per.ToString("F2");
            foreach (var column in Report.Columns.Cast<DataColumn>().ToArray())
            {
                if (Report.AsEnumerable().All(dr => dr.IsNull(column)))
                    Report.Columns.Remove(column);
            }
            DataRow newvartical = Report.NewRow();
            DataRow newvarticalks = Report.NewRow();
            newvartical["Date"] = "Total";

            double val1 = 0;
            foreach (DataColumn dc in Report.Columns)
            {
                if (dc.DataType == typeof(Double))
                {
                    val1 = 0.0;
                    double empty = 0;
                    double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val1);
                    newvartical[dc.ToString()] = val1;
                    if (dc.ToString() == "Total  Leaks" || dc.ToString() == "Despatch Qty")
                    {
                        newvarticalks[dc.ToString()] = empty;
                    }
                    else
                    {
                        newvarticalks[dc.ToString()] = val1;
                    }

                }
            }
            Report.Rows.Add(newvartical);
            //
            DataRow newvartical1 = Report.NewRow();
            newvartical1["Date"] = "Dispatchqty";
            DataRow newvartical2 = Report.NewRow();
            double val2 = 0;
            string va = string.Empty;
            double Gval = 0;
            int inc = 1;
            foreach (DataRow drr2 in dtproductdispatch.Rows)
            {
                string col1 = drr2[2].ToString();
                va = drr2[1].ToString();
                try
                {
                    newvartical1[col1] = va;
                    Gval = Gval + Convert.ToDouble(va);

                    //
                    double d = Convert.ToDouble(newvarticalks[col1]);
                    double LeakPercent = (d / Convert.ToDouble(va)) * 100;
                    //if (col1 == "Total  Leaks" || col1 == "Despatch Qty")
                    //{
                    //    newvarticalks[col1] = " ";
                    //}
                    //else
                    //{
                    newvarticalks[col1] = LeakPercent.ToString("F2");
                    //}
                    //

                    inc++;
                }
                catch (Exception ex)
                {

                }
            }
            newvartical1["Despatch Qty"] = val1.ToString();
            Report.Rows.Add(newvartical1);
            newvarticalks["Date"] = "Leak%";
            Report.Rows.Add(newvarticalks);
            grdReports.DataSource = Report;
            grdReports.DataBind();
            Session["xportdata"] = Report;
            #endregion
        }
        catch
        {
        }
    }

}