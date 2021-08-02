using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class TotalLeakReturnDispatch : System.Web.UI.Page
{
    MySqlCommand cmd;
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
                //if (ddlReportType.SelectedValue == "Plant Wise")
                //{
                //    cmd = new MySqlCommand("SELECT sno, BranchName, SalesType, Lat, Lng, Radius, phonenumber, emailid, userdata_sno, flag, WTarget, MTarget, DTarget, CollectionType, Address, DateOfEntry,incentiveStructure_sno, OrtherBrands, ShopName, SalesOfficeID, RouteID, BranchCode, phonenumber2, duelimit, TinNumber, Photo, SalesRepresentative,Due_Limit_Days, Due_Limit_Type FROM branchdata WHERE (SalesType = 23) AND (flag <> 0)");
                //}
                //if (ddlReportType.SelectedValue == "SalesOffice Wise")
                //{
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) and (branchdata.flag=@flag) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) and (branchdata.flag=@flag)");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                cmd.Parameters.AddWithValue("@SalesType", "21");
                cmd.Parameters.AddWithValue("@SalesType1", "26");
                cmd.Parameters.AddWithValue("@flag", "1");
                //}
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) and (branchdata.flag=@flag) OR (branchdata.sno = @BranchID) AND (branchdata.SalesType IS NOT NULL) and (branchdata.flag=@flag)");
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                cmd.Parameters.AddWithValue("@flag", "1");
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
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            DataTable Report = new DataTable();
            Session["RouteName"] = "Leaks And Returns";
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
            cmd = new MySqlCommand("SELECT sno, SalesType FROM branchdata WHERE (sno = @BranchID)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            DataTable dtsalestype = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT  dispatch.DispName, dispatch.sno, dispatch.flag FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) AND (dispatch.DispType IS NULL) AND (dispatch.flag <> 0) OR (dispatch.DispType IS NULL) AND (branchdata_1.SalesOfficeID = @SOID) AND (dispatch.flag <> 0) ORDER BY dispatch.Branch_Id, dispatch.sno");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            DataTable dtLeaks = new DataTable();
            DataTable dtReturns = new DataTable();
            DataTable dttotLeaks = new DataTable();
            DataTable dttotReturns = new DataTable();
            if (dtsalestype.Rows[0]["SalesType"].ToString() == "23")
            {
                cmd = new MySqlCommand("SELECT SUM(leakages.TotalLeaks) AS LeakQty, leakages.VarifyStatus, leakages.TotalLeaks, productsdata.ProductName, tripdat.AssignDate, dispatch.DispName,dispatch.sno, tripdat.Sno AS tripdatasno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN leakages ON tripdat.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (dispatch.Branch_Id = @BranchID) AND (leakages.VarifyStatus = 'V') GROUP BY dispatch.sno, productsdata.sno");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                dtLeaks = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT dispatch.DispName, tripdat.I_Date, tripdat.AssignDate, tripdat.Sno AS tripdatasno, branchproducts.unitprice, SUM(leakages.VReturns) AS ReturnQty, productsdata.ProductName, dispatch.sno AS dispsno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN leakages ON tripdat.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN branchproducts ON dispatch.Branch_Id = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno WHERE (dispatch.Branch_Id = @BranchID) AND (leakages.VarifyReturnStatus = 'V') GROUP BY dispatch.DispName, productsdata.ProductName, dispatch.sno");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                dtReturns = vdm.SelectQuery(cmd).Tables[0];
            }
            else
            {
                //cmd = new MySqlCommand("SELECT ROUND(SUM(leakages.TotalLeaks), 2) AS LeakQty, productsdata.ProductName, dispatch.sno, dispatch.DispName FROM tripdata INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno WHERE (leakages.VarifyStatus = 'V') AND (dispatch.Branch_Id = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2) OR (leakages.VarifyStatus = 'V') AND (branchdata.SalesOfficeID = @SOID) AND (tripdata.I_Date BETWEEN @d1 AND @d2) GROUP BY productsdata.sno, dispatch.sno ORDER BY dispatch.Branch_Id");
                cmd = new MySqlCommand("SELECT SUM(Leaks.TotalLeaks) AS LeakQty, Leaks.ProductName, ff.DispName, ff.DespSno AS Sno FROM (SELECT leakages.TotalLeaks, productsdata.ProductName, tripdata_1.Sno FROM tripdata tripdata_1 INNER JOIN leakages ON tripdata_1.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (leakages.VarifyStatus = 'V') AND (tripdata_1.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno FROM (SELECT dispatch.DispName, tripdata.Sno, dispatch.sno AS DespSno FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE        (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) OR (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchdata.SalesOfficeID = @SOID)) TripInfo) ff ON ff.Sno = Leaks.Sno GROUP BY ff.DispName, Leaks.ProductName, ff.DespSno");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                dtLeaks = vdm.SelectQuery(cmd).Tables[0];

                ////cmd = new MySqlCommand("SELECT SUM(Leaks.TotalLeaks) AS LeakQty, Leaks.ProductName FROM (SELECT leakages.TotalLeaks, productsdata.ProductName, tripdata_1.Sno FROM tripdata tripdata_1 INNER JOIN leakages ON tripdata_1.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (leakages.VarifyStatus = 'V') AND (tripdata_1.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno FROM (SELECT dispatch.DispName, tripdata.Sno, dispatch.sno AS DespSno FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE        (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) OR (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchdata.SalesOfficeID = @SOID)) TripInfo) ff ON ff.Sno = Leaks.Sno GROUP BY Leaks.ProductName");
                //////cmd = new MySqlCommand("SELECT ROUND(SUM(leakages.TotalLeaks), 2) AS LeakQty, productsdata.ProductName FROM tripdata INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno WHERE (dispatch.Branch_Id = @BranchID) AND (leakages.VarifyStatus = 'V') AND (tripdata.I_Date BETWEEN @d1 AND @d2) OR (leakages.VarifyStatus = 'V') AND (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchdata.SalesOfficeID = @SOID) GROUP BY productsdata.sno");
                ////cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                ////cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
                ////cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                ////cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                ////dttotLeaks = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno AS dispsno, tripdat.AssignDate, tripdat.Sno AS tripdatasno, branchproducts.unitprice, SUM(leakages.ReturnQty) AS returnqty, productsdata.ProductName FROM  dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN leakages ON tripdat.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN branchproducts ON dispatch.Branch_Id = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno WHERE (leakages.VarifyStatus = 'V') AND (dispatch.Branch_Id = @BranchID) GROUP BY dispatch.DispName, branchproducts.product_sno");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                dtReturns = vdm.SelectQuery(cmd).Tables[0];

                ////cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno AS dispsno, tripdat.AssignDate, tripdat.Sno AS tripdatasno, branchproducts.unitprice, SUM(leakages.ReturnQty) AS returnqty, productsdata.ProductName FROM  dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN leakages ON tripdat.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN branchproducts ON dispatch.Branch_Id = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno WHERE (leakages.VarifyStatus = 'V') AND (dispatch.Branch_Id = @BranchID) GROUP BY branchproducts.product_sno");
                ////cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                ////cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                ////cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                ////dttotReturns = vdm.SelectQuery(cmd).Tables[0];
            }
            cmd = new MySqlCommand("SELECT productsdata.ProductName, products_category.Categoryname, productsdata.Units, productsdata.Qty FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) ORDER BY branchproducts.Rank");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            DataTable dttable = vdm.SelectQuery(cmd).Tables[0];
            if (dttable.Rows.Count > 0)
            {
                DataView view = new DataView(dttable);
                DataTable produtstbl = view.ToTable(true, "ProductName", "Categoryname", "Units", "Qty");
                Session["Report"] = produtstbl;
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("Route Name");
                Report.Columns.Add("Type");
                foreach (DataRow dr in produtstbl.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                }
                Report.Columns.Add("Total ltrs").DataType = typeof(Double);
                Report.Columns.Add("Average For Total Dispatch");
                DataView dtview = new DataView(dtRoutedata);
                DataTable empltbl = dtview.ToTable(true, "DispName", "sno");
                int i = 1;
                foreach (DataRow branch in empltbl.Rows)
                {
                    cmd = new MySqlCommand("SELECT tripdat.Sno, tripdat.Status, tripsubdata.ProductId, SUM(tripsubdata.Qty) AS dispatchqty, productsdata.ProductName FROM triproutes INNER JOIN (SELECT Sno, Status, I_Date, BranchID FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE (triproutes.RouteID = @dispid) AND (tripdat.Status <> 'C') GROUP BY tripsubdata.ProductId");
                    cmd.Parameters.AddWithValue("@dispid", branch["sno"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                    DataTable dtdispqty = vdm.SelectQuery(cmd).Tables[0];
                    double Leakltrs = 0;
                    DataRow routename = Report.NewRow();
                    routename["SNo"] = i;
                    routename["Route Name"] = branch["DispName"].ToString();
                    routename["Type"] = "Dispatch";
                    double totdispatch = 0;
                    foreach (DataRow dr in dtdispqty.Rows)
                    {
                        double dispatchqty = 0;
                        double.TryParse(dr["dispatchqty"].ToString(), out dispatchqty);
                        double tot = dispatchqty / 2;
                        routename[dr["ProductName"].ToString()] = Math.Round(dispatchqty, 2);
                        totdispatch += dispatchqty;
                    }
                    routename["Total ltrs"] = Math.Round(totdispatch, 2);
                    routename["Average For Total Dispatch"] = "0";
                    if (totdispatch != 0)
                    {
                        Report.Rows.Add(routename);
                    }
                    DataRow newleak = Report.NewRow();
                    newleak["Type"] = "Leakage";
                    double totalleak = 0;
                    double leakavg = 0;
                    foreach (DataRow dr in dtLeaks.Rows)
                    {
                        if (branch["DispName"].ToString() == dr["DispName"].ToString())
                        {
                            double TotalLeaks = 0;
                            double.TryParse(dr["LeakQty"].ToString(), out TotalLeaks);
                            double tot = TotalLeaks / 2;
                            newleak[dr["ProductName"].ToString()] = Math.Round(TotalLeaks, 2);
                            Leakltrs += TotalLeaks;
                            totalleak += TotalLeaks;
                        }
                    }
                    if (totdispatch != 0)
                    {
                        leakavg = (totalleak / totdispatch) * 100;
                    }
                    newleak["Total ltrs"] = Math.Round(Leakltrs, 2);
                    newleak["Average For Total Dispatch"] = Math.Round(leakavg, 2);
                    if (totdispatch != 0)
                    {
                        Report.Rows.Add(newleak);
                    }
                    DataRow newreturn = Report.NewRow();
                    newreturn["Type"] = "Returns";
                    double Returnltrs = 0;
                    double returnavg = 0;
                    foreach (DataRow dr in dtReturns.Rows)
                    {
                        if (branch["DispName"].ToString() == dr["DispName"].ToString())
                        {
                            double TotalReturns = 0;
                            double.TryParse(dr["ReturnQty"].ToString(), out TotalReturns);
                            if (TotalReturns >= 0)
                            {
                                newreturn[dr["ProductName"].ToString()] = Math.Round(TotalReturns, 2);
                                Returnltrs += TotalReturns;
                            }
                        }
                    }
                    if (totdispatch != 0)
                    {
                        returnavg = (Returnltrs / totdispatch) * 100;
                    }
                    newreturn["Total ltrs"] = Math.Round(Returnltrs, 2);
                    newreturn["Average For Total Dispatch"] = Math.Round(returnavg, 2);
                    if (totdispatch != 0)
                    {
                        Report.Rows.Add(newreturn);
                    }
                    DataRow newvartical = Report.NewRow();
                    newvartical["Route Name"] = "";
                    Report.Rows.Add(newvartical);
                    i++;
                }
                //cmd = new MySqlCommand("SELECT tripdat.Sno, tripdat.Status, tripsubdata.ProductId, SUM(tripsubdata.Qty) AS dispatchqty, productsdata.ProductName FROM triproutes INNER JOIN (SELECT Sno, Status, I_Date, BranchID FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN branchdata ON tripdat.BranchID = branchdata.sno WHERE (tripdat.BranchID = @branchid) OR (branchdata.SalesOfficeID = @branchid) GROUP BY tripsubdata.ProductId");
                //cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
                //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                //cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                //DataTable dttotdispqty = vdm.SelectQuery(cmd).Tables[0];
                //DataRow Total = Report.NewRow();
                //Total["SNo"] = i;
                //Total["Route Name"] = "Total";
                //Total["Type"] = "Dispatch";
                //double totaldispatch = 0;
                //foreach (DataRow dr in dttotdispqty.Rows)
                //{
                //    double dispatchqty = 0;
                //    double.TryParse(dr["dispatchqty"].ToString(), out dispatchqty);
                //    double tot = dispatchqty / 2;
                //    Total[dr["ProductName"].ToString()] = Math.Round(dispatchqty, 2);
                //    totaldispatch += dispatchqty;
                //}
                //Total["Total ltrs"] = Math.Round(totaldispatch, 2);
                //Total["Average For Total Dispatch"] = "0";
                //if (totaldispatch != 0)
                //{
                //    Report.Rows.Add(Total);
                //}
                //DataRow newleaktotal = Report.NewRow();
                //newleaktotal["Type"] = "Leakage";
                //double totleakavg = 0;
                //double alleakages = 0;
                //foreach (DataRow dr in dttotLeaks.Rows)
                //{

                //    double TotalLeaks = 0;
                //    double.TryParse(dr["LeakQty"].ToString(), out TotalLeaks);
                //    double tot = TotalLeaks / 2;
                //    newleaktotal[dr["ProductName"].ToString()] = Math.Round(TotalLeaks, 2);
                //    alleakages += TotalLeaks;
                //}
                //if (totaldispatch != 0)
                //{
                //    totleakavg = (alleakages / totaldispatch) * 100;
                //}
                //newleaktotal["Total ltrs"] = Math.Round(alleakages, 2);
                //newleaktotal["Average For Total Dispatch"] = Math.Round(totleakavg, 2);
                //if (totaldispatch != 0)
                //{
                //    Report.Rows.Add(newleaktotal);
                //}
                //DataRow newReturntotal = Report.NewRow();
                //newReturntotal["Type"] = "Return";
                //double totReturnavg = 0;
                //double allReturns = 0;
                //foreach (DataRow dr in dttotReturns.Rows)
                //{

                //    double TotalLeaks = 0;
                //    double.TryParse(dr["ReturnQty"].ToString(), out TotalLeaks);
                //    double tot = TotalLeaks / 2;
                //    newReturntotal[dr["ProductName"].ToString()] = Math.Round(TotalLeaks, 2);
                //    allReturns += TotalLeaks;
                //}
                //if (totaldispatch != 0)
                //{
                //    totReturnavg = (allReturns / totaldispatch) * 100;
                //}
                //newReturntotal["Total ltrs"] = Math.Round(allReturns, 2);
                //newReturntotal["Average For Total Dispatch"] = Math.Round(totReturnavg, 2);
                //if (totaldispatch != 0)
                //{
                //    Report.Rows.Add(newReturntotal);
                //}
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
                Session["xportdata"] = Report;

            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
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
    //protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    FillSalesOffice();

    //}
}