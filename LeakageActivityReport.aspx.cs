using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
public partial class LeakageActivityReport : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    int j = 1;
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
                lblTitle.Text = Session["TitleName"].ToString();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txttodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            }
        }
    }
    void FillSalesOffice()
    {
        try
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
            FillEmployees();
        }
        catch
        {
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    void FillEmployees()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (ddlreporttype.SelectedValue == "Sales Man")
            {
                ddlrouteoremployee.Visible = true;
                cmd = new MySqlCommand("SELECT Sno, EmpName FROM empmanage WHERE (Branch = @SuperBranch) AND (EmpType = 1)");
                cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlrouteoremployee.DataSource = dtRoutedata;
                ddlrouteoremployee.DataTextField = "EmpName";
                ddlrouteoremployee.DataValueField = "Sno";
                ddlrouteoremployee.DataBind();
            }
            if (ddlreporttype.SelectedValue == "Routes")
            {
                ddlrouteoremployee.Visible = true;
                cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
                cmd.Parameters.AddWithValue("@BranchD", ddlSalesOffice.SelectedValue);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlrouteoremployee.DataSource = dtRoutedata;
                ddlrouteoremployee.DataTextField = "DispName";
                ddlrouteoremployee.DataValueField = "sno";
                ddlrouteoremployee.DataBind();
            }
            if (ddlreporttype.SelectedValue == "Consolidated")
            {
                ddlrouteoremployee.Visible = false;
               // lblroute.Visible = false;
            }
        }
        catch
        {

        }
    }
    protected void ddlreporttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEmployees();
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
            DataTable Report = new DataTable();
            Session["RouteName"] = "Leaks Shorts And Returns";
            Session["IDate"] = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime dtDate = DateTime.Now;
            string Time = dtDate.ToString("dd/MMM/yyyy");
            lblselected.Text = ddlSalesOffice.SelectedItem.Text;

            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            string[] fromdatestrig = txtdate.Text.Split(' ');
            string[] todatestrig = txttodate.Text.Split(' ');
            if (fromdatestrig.Length > 1)
            {
                if (fromdatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = fromdatestrig[0].Split('-');
                    string[] times = fromdatestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            if (todatestrig.Length > 1)
            {
                if (todatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = todatestrig[0].Split('-');
                    string[] times = todatestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            lbl_selfromdate.Text = txtdate.Text;
            lbl_selttodate.Text = txttodate.Text;
            #region------------>Leaks<-------------
            if (ddlType.SelectedValue == "Leaks")
            {
                DataTable dtLeaks = new DataTable();
                cmd = new MySqlCommand("SELECT SUM(Leaks.TotalLeaks) AS LeakQty, Leaks.ProductName, ff.DespSno AS sno, ff.DispName, DATE_FORMAT(Leaks.I_Date, '%m/%d/%Y') AS IndentDATE, Leaks.EmpId FROM (SELECT leakages.TotalLeaks, leakages.VReturns, leakages.TotalLeaks AS Expr1, productsdata.ProductName, tripdata_1.Sno, leakages.ReturnQty, tripdata_1.I_Date, tripdata_1.ReturnDCTime, tripdata_1.EmpId FROM tripdata tripdata_1 INNER JOIN leakages ON tripdata_1.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata_1.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno FROM (SELECT dispatch.DispName, tripdata.Sno, dispatch.sno AS DespSno FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID)) TripInfo) ff ON ff.Sno = Leaks.Sno GROUP BY Leaks.I_Date, Leaks.ProductName,Leaks.EmpId ORDER BY Leaks.I_Date");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                dtLeaks = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT tripdat.Sno, tripdat.Status, tripsubdata.ProductId, productsdata.ProductName, products_category.Categoryname FROM triproutes INNER JOIN (SELECT Sno, Status, I_Date, BranchID FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchdata ON tripdat.BranchID = branchdata.sno WHERE (tripdat.Status <> 'C') AND (tripdat.BranchID = @BranchID) OR (tripdat.Status <> 'C') AND (branchdata.SalesOfficeID = @BranchID) GROUP BY tripsubdata.ProductId, products_category.Categoryname");
                // cmd.Parameters.AddWithValue("@flag", "1");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT Sno, EmpName FROM empmanage WHERE (Branch = @SuperBranch) AND (EmpType = 1)");
                cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
                DataTable dtsalesmans = vdm.SelectQuery(cmd).Tables[0];
                if (ddlreporttype.SelectedValue == "Sales Man")
                {
                        lblreporttitle.Text = "<u>Sales Man Leak Activity</u>";
                        if (produtstbl.Rows.Count > 0)
                        {
                            DataView view = new DataView(dtLeaks);
                            Report = new DataTable();
                            Report.Columns.Add("SNo");
                            Report.Columns.Add("DATE");
                            Report.Columns.Add("ROUTE");
                            int count = 0;
                            DataTable distinctProduct = view.ToTable(true, "ProductName");
                            // distinctProduct.DefaultView.Sort = "Rank asc";
                            DataView dv = distinctProduct.DefaultView;
                            dv.Sort = "ProductName asc";
                            DataTable sortedDT = dv.ToTable();
                            foreach (DataRow dr in produtstbl.Rows)
                            {
                                Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                            }
                            Report.Columns.Add("Total Despatch Qty").DataType = typeof(Double);
                            Report.Columns.Add("Total Leak ltrs").DataType = typeof(Double);
                            Report.Columns.Add("Average For Total Dispatch").DataType = typeof(Double);
                            DataView viewdate = new DataView(dtLeaks);
                            DataTable distinctdates = viewdate.ToTable(true, "IndentDATE");
                            int i = 1;
                            TimeSpan dateSpan = todate.Subtract(fromdate);
                            int NoOfdays = dateSpan.Days;
                            NoOfdays = NoOfdays + 1;
                            for (int j = 0; j < NoOfdays; j++)
                            {
                                string dtcount = fromdate.AddDays(j).ToString();
                                DateTime dtIndentDate = Convert.ToDateTime(dtcount);
                                DataRow newrow = Report.NewRow();
                                newrow["SNo"] = i++;
                                string IndentDate = dtcount;
                                //DateTime dtIndentDate = Convert.ToDateTime(IndentDate);
                                DateTime dtchange = dtIndentDate.AddDays(-1);
                                string ChangedTime1 = dtchange.ToString("MM/dd/yyyy");
                                string ChangedTime = dtIndentDate.ToString("dd/MMM/yyyy");
                                newrow["DATE"] = ChangedTime;
                                cmd = new MySqlCommand("SELECT tripdat.Sno, tripdat.Status, tripsubdata.ProductId, SUM(tripsubdata.Qty) AS dispatchqty, productsdata.ProductName, triproutes.Tripdata_sno, dispatch.DispName FROM triproutes INNER JOIN (SELECT Sno, Status, I_Date, BranchID, EmpId FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno WHERE (tripdat.EmpId = @dispid) AND (tripdat.Status <> 'C') GROUP BY tripsubdata.ProductId, dispatch.DispName");
                                cmd.Parameters.AddWithValue("@dispid", ddlrouteoremployee.SelectedValue);
                                cmd.Parameters.AddWithValue("@d1", GetLowDate(dtIndentDate).AddDays(-1));
                                cmd.Parameters.AddWithValue("@d2", GetHighDate(dtIndentDate).AddDays(-1));
                                DataTable dtdispqty = vdm.SelectQuery(cmd).Tables[0];
                                double Totleaks = 0;
                                double Total = 0;
                                string routename = "";
                                foreach (DataRow drleaks in dtLeaks.Select("EmpId='" + ddlrouteoremployee.SelectedValue + "' AND IndentDATE='" + ChangedTime1 + "' "))
                                {

                                    double TotalLeaks = 0;
                                    double.TryParse(drleaks["LeakQty"].ToString(), out TotalLeaks);
                                    //double UnitCost = 0;
                                    //double.TryParse(drleaks["UnitPrice"].ToString(), out UnitCost);
                                    TotalLeaks = Math.Round(TotalLeaks, 2);
                                    newrow[drleaks["ProductName"].ToString()] = TotalLeaks;
                                    // Total += TotalLeaks * UnitCost;
                                    Totleaks += TotalLeaks;
                                }
                                double totdispatch = 0;
                                double avg = 0;
                                foreach (DataRow dr in dtdispqty.Rows)
                                {
                                    double dispatchqty = 0;
                                    newrow["ROUTE"] = dr["DispName"].ToString();
                                    routename = dr["DispName"].ToString();
                                    double.TryParse(dr["dispatchqty"].ToString(), out dispatchqty);
                                    totdispatch += dispatchqty;
                                }
                                newrow["Total Despatch Qty"] = Math.Round(totdispatch, 2);
                                newrow["Total Leak ltrs"] = Math.Round(Totleaks, 2);
                                if (dtdispqty.Rows.Count > 0)
                                {
                                    avg = (Totleaks / totdispatch) * 100;
                                    newrow["Average For Total Dispatch"] = Math.Round(avg, 2);
                                }
                                else
                                {
                                    newrow["Average For Total Dispatch"] = Math.Round(avg, 2);

                                }
                                //if (routename == "")
                                //{
                                //}
                                //else
                                //{
                                Report.Rows.Add(newrow);
                                //}
                            }
                        }
                    DataRow newvartical = Report.NewRow();
                    newvartical["ROUTE"] = "Total";
                    //float val = 0;
                    //float.TryParse(Report.Compute("sum([Total  Amount])", "[Total  Amount]<>'0'").ToString(), out val);
                    //newvartical["Total  Amount"] = val;
                    double val1 = 0;
                    double totavg = 0;
                    double totleakltrs = 0;
                    double totdispltrs = 0;
                    foreach (DataColumn dc in Report.Columns)
                    {
                        if (dc.DataType == typeof(Double))
                        {
                            val1 = 0.0;
                            double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val1);
                            if (val1 == 0.0)
                            {
                            }
                            else
                            {
                                newvartical[dc.ToString()] = val1;
                            }
                        }
                        if (dc.ColumnName == "Total Leak ltrs")
                        {
                            totleakltrs = val1;
                        }
                        if (dc.ColumnName == "Total Despatch Qty")
                        {
                            totdispltrs = val1;
                        }
                        if (dc.ColumnName == "Average For Total Dispatch")
                        {
                            if (totdispltrs > 0)
                            {
                                totavg = (totleakltrs / totdispltrs) * 100;
                            }
                            newvartical["Average For Total Dispatch"] = Math.Round(totavg, 2);
                        }
                    }
                    Report.Rows.Add(newvartical);
                    foreach (var column in Report.Columns.Cast<DataColumn>().ToArray())
                    {
                        if (Report.AsEnumerable().All(dr => dr.IsNull(column)))
                            Report.Columns.Remove(column);
                    }
                    foreach (DataColumn col in Report.Columns)
                    {
                        string Pname = col.ToString();
                        string ProductName = col.ToString();
                        ProductName = GetSpace(ProductName);
                        Report.Columns[Pname].ColumnName = ProductName;
                    }
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                }
                if (ddlreporttype.SelectedValue == "Consolidated")
                {

                    DataTable Report1 = new DataTable();
                    Report1.Columns.Add("SNo");
                    Report1.Columns.Add("SalesMan");
                    Report1.Columns.Add("NO.Of Days");
                    Report1.Columns.Add("Despatch Qty").DataType = typeof(Double);
                    Report1.Columns.Add("Leak Qty").DataType = typeof(Double);
                    Report1.Columns.Add("LeakPerceent").DataType = typeof(Double);
                    //lblreporttitle.Text = "<u>Sales Man Leak Activity</u>";
                    cmd = new MySqlCommand("SELECT  SUM(Leaks.TotalLeaks) AS LeakQty, Leaks.EmpId FROM (SELECT  leakages.TotalLeaks, leakages.VReturns, leakages.TotalLeaks AS Expr1, productsdata.ProductName, tripdata_1.Sno, leakages.ReturnQty,tripdata_1.I_Date, tripdata_1.ReturnDCTime, tripdata_1.EmpId FROM tripdata tripdata_1 INNER JOIN leakages ON tripdata_1.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE  (tripdata_1.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno FROM (SELECT dispatch.DispName, tripdata.Sno, dispatch.sno AS DespSno FROM  branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID)) TripInfo) ff ON ff.Sno = Leaks.Sno GROUP BY Leaks.EmpId ORDER BY Leaks.I_Date");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                    DataTable dtLeaks1 = vdm.SelectQuery(cmd).Tables[0];
                    double Totleaks = 0;
                    double Total = 0;
                    string routename = "";
                    double totdispatch = 0;
                    double avg = 0;
                    double totaldays = 0;
                    int i = 1;
                    foreach (DataRow drsalesman in dtsalesmans.Rows)
                    {
                        if (produtstbl.Rows.Count > 0)
                        {
                            DataRow newrow = Report1.NewRow();
                            newrow["SNo"] = i++;
                            cmd = new MySqlCommand("SELECT  QTY, VALUE FROM  (SELECT SUM(dispatchqty) AS QTY, I_Date, COUNT(I_Date) AS VALUE FROM  (SELECT SUM(tripsubdata.Qty) AS dispatchqty, tripdat.I_Date FROM triproutes INNER JOIN (SELECT Sno, Status, I_Date, BranchID, EmpId FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno WHERE (tripdat.EmpId = @dispid) AND (tripdat.Status <> 'C')  GROUP BY tripdat.I_Date) derivedtbl_1) derivedtbl_2");
                            cmd.Parameters.AddWithValue("@dispid", drsalesman["Sno"].ToString());
                            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                            DataTable dtdispqty = vdm.SelectQuery(cmd).Tables[0];
                            //foreach (DataRow dr in dtdispqty.Rows)
                            //{
                            double dispatchqty = 0;
                            newrow["SalesMan"] = drsalesman["EmpName"].ToString();
                            //routename = dr["DispName"].ToString();
                            string dispqty = dtdispqty.Rows[0]["QTY"].ToString();
                            double.TryParse(dispqty, out dispatchqty);
                            dispatchqty = Math.Round(dispatchqty, 2);
                            newrow["Despatch Qty"] = dispatchqty;
                            totdispatch += dispatchqty;
                            double totdays = 0;
                            string days = dtdispqty.Rows[0]["VALUE"].ToString();
                            double.TryParse(days, out totdays);
                            totaldays += totdays;
                            newrow["NO.Of Days"] = totdays;
                            foreach (DataRow drleaks in dtLeaks1.Select("EmpId='" + drsalesman["sno"].ToString() + "' "))
                            {
                                double TotalLeaks = 0;
                                double.TryParse(drleaks["LeakQty"].ToString(), out TotalLeaks);
                                TotalLeaks = Math.Round(TotalLeaks, 2);
                                newrow["Leak Qty"] = TotalLeaks;
                                // newrow[drleaks["ProductName"].ToString()] = TotalLeaks;
                                // Total += TotalLeaks * UnitCost; 
                                double leakper = TotalLeaks / dispatchqty * 100;
                                newrow["LeakPerceent"] = Math.Round(leakper,2);
                                Totleaks += TotalLeaks;
                            }
                            //}
                            Report1.Rows.Add(newrow);
                        }
                    }
                    DataRow salesreport1 = Report1.NewRow();
                    salesreport1["SalesMan"] = "Total";
                    salesreport1["DESPATCH QTY"] = Math.Round(totdispatch, 2);
                    salesreport1["LEAK QTY"] = Math.Round(Totleaks, 2);
                    salesreport1["NO.Of Days"] = totaldays;
                    double totd = Math.Round(Totleaks, 2);
                    double totl = Math.Round(totdispatch, 2);
                    double totp = totd / totl * 100;
                    salesreport1["LeakPerceent"] = Math.Round(totp, 2);
                    Report1.Rows.Add(salesreport1);
                    grdReports.DataSource = Report1;
                    grdReports.DataBind();
                }
                #region routes
                if (ddlreporttype.SelectedValue == "Routes")
                {
                    lblreporttitle.Text = "<u>Route Leak Activity</u>";
                    if (produtstbl.Rows.Count > 0)
                    {
                        DataView view = new DataView(dtLeaks);
                        Report = new DataTable();
                        Report.Columns.Add("SNo");
                        Report.Columns.Add("DATE");
                        Report.Columns.Add("SALES MAN");
                        int count = 0;
                        DataTable distinctProduct = view.ToTable(true, "ProductName");
                        // distinctProduct.DefaultView.Sort = "Rank asc";
                        DataView dv = distinctProduct.DefaultView;
                        dv.Sort = "ProductName asc";
                        DataTable sortedDT = dv.ToTable();
                        foreach (DataRow dr in sortedDT.Rows)
                        {
                            Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                        }
                        Report.Columns.Add("Total Despatch Qty").DataType = typeof(Double);
                        Report.Columns.Add("Total Leak ltrs").DataType = typeof(Double);
                        Report.Columns.Add("Average For Total Dispatch").DataType = typeof(Double);
                        DataView viewdate = new DataView(dtLeaks);
                        DataTable distinctdates = viewdate.ToTable(true, "IndentDATE");
                        int i = 1;
                        foreach (DataRow drdates in distinctdates.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["SNo"] = i++;
                            string IndentDate = drdates["IndentDATE"].ToString();
                            DateTime dtIndentDate = Convert.ToDateTime(IndentDate);
                            string ChangedTime = dtIndentDate.AddDays(1).ToString("dd/MMM/yyyy");
                            newrow["DATE"] = ChangedTime;
                            cmd = new MySqlCommand("SELECT tripdat.Sno, tripdat.Status, tripsubdata.ProductId, SUM(tripsubdata.Qty) AS dispatchqty, productsdata.ProductName FROM triproutes INNER JOIN (SELECT Sno, Status, I_Date, BranchID, EmpId FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE (triproutes.RouteID = @dispid) AND (tripdat.Status <> 'C') GROUP BY tripsubdata.ProductId");
                            cmd.Parameters.AddWithValue("@dispid", ddlrouteoremployee.SelectedValue);
                            cmd.Parameters.AddWithValue("@d1", GetLowDate(dtIndentDate));
                            cmd.Parameters.AddWithValue("@d2", GetHighDate(dtIndentDate));
                            DataTable dtdispqty = vdm.SelectQuery(cmd).Tables[0];
                            double Totleaks = 0;
                            double Total = 0;
                            foreach (DataRow drleaks in dtLeaks.Select("sno='" + ddlrouteoremployee.SelectedValue + "' AND IndentDATE='" + IndentDate + "' "))
                            {
                                newrow["SALES MAN"] = "0";// drleaks["EmpName"].ToString();
                                double TotalLeaks = 0;
                                double.TryParse(drleaks["LeakQty"].ToString(), out TotalLeaks);
                                TotalLeaks = Math.Round(TotalLeaks, 2);
                                //double UnitCost = 0;
                                //double.TryParse(drleaks["UnitPrice"].ToString(), out UnitCost);
                                newrow[drleaks["ProductName"].ToString()] = TotalLeaks;
                                // Total += TotalLeaks * UnitCost;
                                Totleaks += TotalLeaks;
                            }
                            double totdispatch = 0;
                            double avg = 0;
                            foreach (DataRow dr in dtdispqty.Rows)
                            {
                                double dispatchqty = 0;
                                double.TryParse(dr["dispatchqty"].ToString(), out dispatchqty);
                                totdispatch += dispatchqty;
                            }
                            newrow["Total Despatch Qty"] = Math.Round(totdispatch, 2);
                            newrow["Total Leak ltrs"] = Math.Round(Totleaks, 2);
                            if (dtdispqty.Rows.Count > 0)
                            {
                                avg = (Totleaks / totdispatch) * 100;
                                newrow["Average For Total Dispatch"] = Math.Round(avg, 2);
                            }
                            else
                            {
                                newrow["Average For Total Dispatch"] = Math.Round(avg, 2);

                            }
                            Report.Rows.Add(newrow);
                        }
                    }
                    DataRow newvartical = Report.NewRow();
                    newvartical["SALES MAN"] = "Total";
                    double val1 = 0;
                    double totavg = 0;
                    double totleakltrs = 0;
                    double totdispltrs = 0;
                    foreach (DataColumn dc in Report.Columns)
                    {
                        if (dc.DataType == typeof(Double))
                        {
                            val1 = 0.0;
                            double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val1);
                            newvartical[dc.ToString()] = val1;
                        }
                        if (dc.ColumnName == "Total Leak ltrs")
                        {
                            totleakltrs = val1;
                        }
                        if (dc.ColumnName == "Total Despatch Qty")
                        {
                            totdispltrs = val1;
                        }
                        if (dc.ColumnName == "Average For Total Dispatch")
                        {
                            if (totdispltrs > 0)
                            {
                                totavg = (totleakltrs / totdispltrs) * 100;
                            }
                            newvartical["Average For Total Dispatch"] = Math.Round(totavg, 2);
                        }
                    }
                    Report.Rows.Add(newvartical);
                    foreach (DataColumn col in Report.Columns)
                    {
                        string Pname = col.ToString();
                        string ProductName = col.ToString();
                        ProductName = GetSpace(ProductName);
                        Report.Columns[Pname].ColumnName = ProductName;
                    }
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;
                }
                #endregion
            }
            #endregion
        }
        catch
        {
        }
    }
    
    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[3].Text == "0")
            {
                e.Row.Visible = false;
            }
            else
            {
                e.Row.Cells[0].Text = j.ToString();
                j++;
            }
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
}