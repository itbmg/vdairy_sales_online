
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class TotalLeaks : System.Web.UI.Page
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
            DataTable AllProducts = new DataTable();
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
            #region---------->Sales Office Wise<-------------
            
                #region------------>Leaks<-------------
                if (ddlType.SelectedValue == "Leaks")
                {
                    DataTable dtLeaks = new DataTable();
                    DataTable dtallroutes = new DataTable();
                    if (dtsalestype.Rows[0]["SalesType"].ToString() == "23")
                    {
                        //if (Session["salestype"].ToString() == "Plant")
                        //{
                        //if (ddlReportType.SelectedValue == "Day Wise")
                        //{
                        //cmd = new MySqlCommand("SELECT SUM(leakages.TotalLeaks) AS LeakQty, leakages.VarifyStatus, leakages.TotalLeaks, productsdata.ProductName,productsdata.sno AS productid, tripdat.AssignDate, dispatch.DispName,dispatch.sno, tripdat.Sno AS tripdatasno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN leakages ON tripdat.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (dispatch.Branch_Id = @BranchID) AND (leakages.VarifyStatus = 'V') GROUP BY dispatch.sno, productsdata.sno");
                       // cmd = new MySqlCommand("SELECT SUM(leakages.TotalLeaks) AS LeakQty, leakages.VarifyStatus, leakages.TotalLeaks, productsdata.ProductName, productsdata.sno AS productid, tripdat.AssignDate, dispatch.DispName, dispatch.sno, tripdat.Sno AS tripdatasno, products_category.Categoryname FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN leakages ON tripdat.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.Branch_Id = @BranchID) AND (leakages.VarifyStatus = 'V') GROUP BY dispatch.sno, productsdata.sno");
                        cmd = new MySqlCommand("SELECT result.LeakQty, result.VarifyStatus, result.TotalLeaks, result.ProductName, result.productid, result.AssignDate, result.DispName, result.sno, result.Branch_Id, result.tripdatasno, result.Categoryname, brnchproductsRank.Rank FROM (SELECT SUM(leakages.TotalLeaks) AS LeakQty, leakages.VarifyStatus, leakages.TotalLeaks, productsdata.ProductName, productsdata.sno AS productid, tripdat.AssignDate, dispatch.DispName, dispatch.sno, dispatch.Branch_Id, tripdat.Sno AS tripdatasno, products_category.Categoryname FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN leakages ON tripdat.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.Branch_Id = @BranchID) AND (leakages.VarifyStatus = 'V') GROUP BY dispatch.sno, productsdata.sno) result INNER JOIN (SELECT branch_sno, product_sno, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchproductsRank ON result.productid = brnchproductsRank.product_sno");
                        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                        dtLeaks = vdm.SelectQuery(cmd).Tables[0];

                        cmd = new MySqlCommand("SELECT DispName, sno, DispType, DispMode, flag FROM dispatch WHERE (Branch_Id = @BranchID) AND (flag <> 0) GROUP BY sno");
                        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                        dtallroutes = vdm.SelectQuery(cmd).Tables[0];

                    }
                    else
                    {

                       /////Ravi
                        if (ddlSalesOffice.SelectedValue == "285")
                        {
                            /////Ravi  29 July 2017  This Plant wise multiple dispatches soo putting this query
                            cmd = new MySqlCommand("SELECT LeakQty, ProductName, DispName, Categoryname, Prodsno AS productid, Sno FROM (SELECT        SUM(Leaks.TotalLeaks) AS LeakQty, Leaks.ProductName, Leaks.Categoryname, ff.DispName, Leaks.Prodsno, ff.Sno FROM (SELECT leakages.TotalLeaks, productsdata.ProductName, tripdata_1.Sno, products_category.Categoryname, productsdata.sno AS Prodsno FROM tripdata tripdata_1 INNER JOIN leakages ON tripdata_1.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE  (tripdata_1.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno FROM (SELECT        dispatch.DispName, tripdata.Sno, dispatch.sno AS DespSno FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE        (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @BranchID)) TripInfo) ff ON ff.Sno = Leaks.Sno GROUP BY Leaks.ProductName, ff.DespSno, ff.Sno) result GROUP BY ProductName, DispName, Sno");
                        }
                        else
                        {
                            cmd = new MySqlCommand("SELECT result.LeakQty, result.ProductName, result.DispName, result.Categoryname, result.Prodsno as productid  FROM (SELECT        SUM(Leaks.TotalLeaks) AS LeakQty, Leaks.ProductName, Leaks.Categoryname, ff.DispName, Leaks.Prodsno FROM (SELECT leakages.TotalLeaks, productsdata.ProductName, tripdata_1.Sno, products_category.Categoryname, productsdata.sno AS Prodsno FROM tripdata tripdata_1 INNER JOIN leakages ON tripdata_1.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE        (leakages.VarifyStatus = 'V') AND (tripdata_1.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT        DispName, Sno, DespSno FROM            (SELECT        dispatch.DispName, tripdata.Sno, dispatch.sno AS DespSno FROM            branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE        (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID)) TripInfo) ff ON ff.Sno = Leaks.Sno GROUP BY Leaks.ProductName, ff.DespSno) result INNER JOIN (SELECT branch_sno, product_sno, Rank FROM            branchproducts WHERE        (branch_sno = @BranchID)) brnchproductsRank ON result.Prodsno = brnchproductsRank.product_sno");
                        }
                        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                        dtLeaks = vdm.SelectQuery(cmd).Tables[0];
                        if (ddlSalesOffice.SelectedValue == "285")
                        {
                            cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName, dispatch.flag, dispatch.DispType, dispatch.DispMode FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id WHERE (dispatch.BranchId = @BranchID) AND (dispatch.flag <> 0)  GROUP BY dispatch.sno ORDER BY dispatch.Branch_Id");
                        }
                        else
                        {
                            cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName, dispatch.flag, dispatch.DispType, dispatch.DispMode FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id WHERE (dispatch.Branch_Id = @BranchID) AND (dispatch.flag <> 0)  GROUP BY dispatch.sno ORDER BY dispatch.Branch_Id");
                        }
                        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                        dtallroutes = vdm.SelectQuery(cmd).Tables[0];

                    }

                    cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno, dispatch.DispType, dispatch.DispMode, dispatch.flag, SUM(tripsubdata.Qty) AS dispqty, productsdata.ProductName, dispatch.BranchID, branchdata.BranchName FROM dispatch INNER JOIN branchdata ON dispatch.BranchID = branchdata.sno INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE (dispatch.flag <> 0) AND (branchdata.sno = @BranchID) GROUP BY dispatch.BranchID");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                    DataTable dtpuffleaksDispatch = vdm.SelectQuery(cmd).Tables[0];
                    //22/12/2016
                    ////cmd = new MySqlCommand("SELECT result.Sno, result.UnitPrice, result.ProductName, result.TotalLeaks, result.BranchID, result.Categoryname, result.productid, brnchprdtrank.Rank FROM (SELECT tripdat.Sno, productsdata.UnitPrice, productsdata.ProductName, SUM(branchleaktrans.LeakQty) AS TotalLeaks, dispatch.BranchID, products_category.Categoryname, productsdata.sno AS productid FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN branchleaktrans ON tripdat.Sno = branchleaktrans.TripId INNER JOIN productsdata ON branchleaktrans.ProdId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.BranchID = @BranchID) AND (branchleaktrans.LeakQty > 0) GROUP BY branchleaktrans.ProdId, dispatch.BranchID, productsdata.sno) result INNER JOIN (SELECT branch_sno, product_sno, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdtrank ON result.productid = brnchprdtrank.product_sno");
                    cmd = new MySqlCommand("SELECT        result.Sno, result.UnitPrice, result.ProductName, result.TotalLeaks, result.BranchID, result.Categoryname, result.productid, brnchprdtrank.Rank FROM (SELECT tripdat.Sno, productsdata.UnitPrice, productsdata.ProductName, SUM(branchleaktrans.LeakQty) AS TotalLeaks, dispatch.BranchID, products_category.Categoryname, productsdata.sno AS productid,  branchdata.SalesOfficeID FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM  tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN branchleaktrans ON tripdat.Sno = branchleaktrans.TripId INNER JOIN productsdata ON branchleaktrans.ProdId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchdata ON dispatch.BranchID = branchdata.sno WHERE (dispatch.BranchID = @BranchID)  GROUP BY branchleaktrans.ProdId, dispatch.BranchID, productsdata.sno, branchdata.SalesOfficeID) result INNER JOIN   (SELECT branch_sno, product_sno, Rank FROM branchproducts WHERE  (branch_sno = @BranchID)) brnchprdtrank ON result.productid = brnchprdtrank.product_sno"); 
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                    DataTable dtpuffleaks = vdm.SelectQuery(cmd).Tables[0];

                    DataView view_PuffLeak_Product = new DataView(dtpuffleaks);
                    DataTable distinct_PuffLeak_Product = view_PuffLeak_Product.ToTable(true, "productid", "ProductName", "Categoryname","Rank");
                    AllProducts = new DataTable();
                    AllProducts.Columns.Add("Category Name");
                    AllProducts.Columns.Add("Productid");
                    AllProducts.Columns.Add("ProductName");
                    AllProducts.Columns.Add("Rank").DataType = typeof(Double); 

                    DataView view = new DataView(dtLeaks);
                    DataTable distinctproducts = view.ToTable(true, "productid", "ProductName", "Categoryname");

                    foreach (DataRow dr in distinctproducts.Rows)
                    {
                        DataRow newrow = AllProducts.NewRow();
                        newrow["Category Name"] = dr["Categoryname"].ToString();
                        newrow["Productid"] = dr["productid"].ToString();
                        newrow["ProductName"] = dr["ProductName"].ToString();
                        //newrow["Rank"] = dr["Rank"].ToString();
                        AllProducts.Rows.Add(newrow);

                    }
                    foreach (DataRow drr in distinct_PuffLeak_Product.Rows)
                    {
                        DataRow[] data_exist = AllProducts.Select("productid='" + drr["productid"].ToString() + "'");
                        if (data_exist.Length > 0)
                        {

                        }
                        else
                        {
                            DataRow newrow = AllProducts.NewRow();
                            newrow["Category Name"] = drr["Categoryname"].ToString();
                            newrow["Productid"] = drr["productid"].ToString();
                            newrow["ProductName"] = drr["ProductName"].ToString();
                            //newrow["Rank"] = drr["Rank"].ToString();
                            AllProducts.Rows.Add(newrow);
                        }
                    }
                   // DataView dv = AllProducts.DefaultView;
                    //dv.Sort = "Rank ASC";
                    DataView dv = AllProducts.DefaultView;
                    dv.Sort = "Rank ASC";
                    DataTable sortedDT = dv.ToTable();
                    //AllProducts.DefaultView.Sort = "Rank";


                    //cmd = new MySqlCommand(" SELECT products_category.Categoryname, products_subcategory.SubCatName, productsdata.ProductName FROM productsdata INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno ORDER BY productsdata.Rank");
                    cmd = new MySqlCommand("SELECT products_category.Categoryname,productsdata.ProductName,productsdata.sno FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) and (branchproducts.flag=@flag) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
                    cmd.Parameters.AddWithValue("@flag", "1");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
                    if (produtstbl.Rows.Count > 0)
                    {
                        DataView view1 = new DataView(dtallroutes);
                        Report = new DataTable();
                        Report.Columns.Add("SNo");
                        Report.Columns.Add("DC Date");
                        Report.Columns.Add("Route Name");
                        foreach (DataRow dr in produtstbl.Rows)
                        {
                            Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                        }
                        Report.Columns.Add("Total ltrs").DataType = typeof(Double);
                        Report.Columns.Add("Total Dispatch").DataType = typeof(Double);
                        Report.Columns.Add("Leaks %");
                        DataTable distincttable = view1.ToTable(true, "sno", "DispName");
                        int i = 1;
                        double totleakltrs = 0;
                        double totdispltrs = 0;
                        foreach (DataRow branch in distincttable.Rows)
                        {
                            double Leakltrs = 0;
                            DataRow newrow = Report.NewRow();
                            newrow["SNo"] = i;
                            newrow["Route Name"] = branch["DispName"].ToString();

                            cmd = new MySqlCommand("SELECT tripdat.Sno, tripdat.Status, tripsubdata.ProductId, SUM(tripsubdata.Qty) AS dispatchqty FROM triproutes INNER JOIN (SELECT Sno, Status, I_Date, BranchID FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno WHERE (triproutes.RouteID = @dispid) AND (tripdat.Status <> 'C')");
                            cmd.Parameters.AddWithValue("@dispid", branch["sno"].ToString());
                            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                            DataTable dtdispqty = vdm.SelectQuery(cmd).Tables[0];

                            foreach (DataRow dr in dtLeaks.Rows)
                            {
                                if (branch["DispName"].ToString() == dr["DispName"].ToString())
                                {
                                    double TotalLeaks = 0;
                                    double.TryParse(dr["LeakQty"].ToString(), out TotalLeaks);
                                    //double UnitCost = 0;
                                    //double.TryParse(dr["unitprice"].ToString(), out UnitCost);
                                    double tot = TotalLeaks / 2;
                                    //if (TotalLeaks == 0)
                                    //{
                                    //}
                                    //else
                                    //{
                                        
                                    newrow[dr["ProductName"].ToString()] = Math.Round(TotalLeaks, 2);
                                        //Total += tot * UnitCost;
                                        Leakltrs += TotalLeaks;
                                    //}
                                }
                            }
                            newrow["Total ltrs"] = Math.Round(Leakltrs, 2);
                            // newrow["Total Amount"] = Total;
                            totleakltrs += Math.Round(Leakltrs, 2);
                            double avg = 0;

                            if (dtdispqty.Rows.Count > 0)
                            {
                                double dispqty = 0;
                                double.TryParse(dtdispqty.Rows[0]["dispatchqty"].ToString(), out dispqty);
                                if (dispqty > 0)
                                {
                                    avg = (Leakltrs / dispqty) * 100;
                                }
                                else
                                {
                                    avg = 0;
                                }
                                newrow["Leaks %"] = Math.Round(avg, 2);
                                totdispltrs += dispqty;
                                newrow["Total Dispatch"] = Math.Round(dispqty, 2);
                            }
                            else
                            {
                                //avg = 0;
                                newrow["Leaks %"] = avg;

                            }
                            Report.Rows.Add(newrow);
                            i++;
                        }
                       
                        DataRow newvartical = Report.NewRow();
                        newvartical["Route Name"] = "Total";

                        float val2 = 0;
                        float.TryParse(Report.Compute("sum([Total ltrs])", "[Total ltrs]<>'0'").ToString(), out val2);
                        newvartical["Total ltrs"] = val2;
                        double val1 = 0;
                        double totavg = 0;
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
                            if (dc.ColumnName == "Leaks %")
                            {
                                totavg = (totleakltrs / totdispltrs) * 100;
                                newvartical["Leaks %"] = Math.Round(totavg, 2);
                            }
                        }
                        
                        Report.Rows.Add(newvartical);
                        DataRow newvarticalbrk = Report.NewRow();
                        newvarticalbrk["Route Name"] = "";
                        Report.Rows.Add(newvarticalbrk);

                        DataRow newvarticalbrk1 = Report.NewRow();
                        newvarticalbrk1["Route Name"] = "";
                        Report.Rows.Add(newvarticalbrk1);

                        foreach (DataRow dr in dtpuffleaksDispatch.Rows)
                        {
                            DataRow newpuffleaks = Report.NewRow();
                            newpuffleaks["Route Name"] = dr["BranchName"].ToString() + "   PUFF LEAKAGE";
                            double totpuffleaks = 0;
                            double avg = 0;
                            foreach (DataRow drleaks in dtpuffleaks.Select("BranchID='" + dr["BranchID"].ToString() + "'"))
                            {
                                double TotalLeaks = 0;
                                double.TryParse(drleaks["TotalLeaks"].ToString(), out TotalLeaks);
                                newpuffleaks[drleaks["ProductName"].ToString()] = Math.Round(TotalLeaks, 2);
                                totpuffleaks += TotalLeaks;
                            }
                            newpuffleaks["Total ltrs"] = Math.Round(totpuffleaks, 2);

                            double dispqty = 0;
                            double.TryParse(dr["dispqty"].ToString(), out dispqty);
                            if (dispqty > 0)
                            {
                                avg = (totpuffleaks / dispqty) * 100;
                            }
                            else
                            {
                                avg = 0;
                            }
                            newpuffleaks["Leaks %"] = Math.Round(avg, 2);
                            // totdispltrs += dispqty;
                            newpuffleaks["Total Dispatch"] = Math.Round(dispqty, 2);
                            Report.Rows.Add(newpuffleaks);

                        }
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
#endregion

                #region------------->Returns<---------------
                if (ddlType.SelectedValue == "Returns")
                {
                    DataTable dtLeaks = new DataTable();

                    if (dtsalestype.Rows[0]["SalesType"].ToString() == "23")
                    {
                        //cmd = new MySqlCommand("SELECT dispatch.DispName,tripdata.I_Date,tripdata.AssignDate,tripdata.Sno AS tripdatasno, branchproducts.unitprice, leakages.VReturns as ReturnQty, productsdata.ProductName FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN branchproducts ON dispatch.Branch_Id = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno WHERE (dispatch.Branch_Id = @BranchID) AND (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (leakages.VarifyReturnStatus = 'V') GROUP BY dispatch.DispName, productsdata.ProductName");
                        cmd = new MySqlCommand("SELECT dispatch.DispName, tripdat.I_Date, tripdat.AssignDate, tripdat.Sno AS tripdatasno, branchproducts.unitprice, SUM(leakages.VReturns) AS ReturnQty, productsdata.ProductName, dispatch.sno AS dispsno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN leakages ON tripdat.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN branchproducts ON dispatch.Branch_Id = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno WHERE (dispatch.Branch_Id = @BranchID) AND (leakages.VarifyReturnStatus = 'V') GROUP BY dispatch.DispName, productsdata.ProductName, dispatch.sno");
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                        dtLeaks = vdm.SelectQuery(cmd).Tables[0];
                    }
                    else
                    {
                        //cmd = new MySqlCommand("SELECT dispatch.DispName,tripdata.I_Date,tripdata.AssignDate,tripdata.Sno AS tripdatasno, branchproducts.unitprice, leakages.ReturnQty, productsdata.ProductName FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN branchproducts ON dispatch.Branch_Id = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno WHERE (dispatch.Branch_Id = @BranchID) AND (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (leakages.VarifyStatus = 'V') GROUP BY dispatch.DispName, productsdata.ProductName");
                        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno AS dispsno, tripdat.AssignDate, tripdat.Sno AS tripdatasno, branchproducts.unitprice, SUM(leakages.ReturnQty) AS returnqty, productsdata.ProductName FROM  dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN leakages ON tripdat.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN branchproducts ON dispatch.Branch_Id = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno WHERE (leakages.VarifyStatus = 'V') AND (dispatch.Branch_Id = @BranchID) GROUP BY dispatch.DispName, branchproducts.product_sno");
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                        dtLeaks = vdm.SelectQuery(cmd).Tables[0];
                    }
                    //}
                    cmd = new MySqlCommand("SELECT products_category.Categoryname,productsdata.ProductName FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) GROUP BY productsdata.ProductName ORDER BY productsdata.Rank");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
                    if (produtstbl.Rows.Count > 0)
                    {
                        DataView view = new DataView(dtLeaks);
                        DataTable distinctproducts = view.ToTable(true, "ProductName");
                        Report = new DataTable();
                        Report.Columns.Add("SNo");
                        Report.Columns.Add("DC Date");
                        Report.Columns.Add("Route Name");
                        foreach (DataRow dr in produtstbl.Rows)
                        {
                            foreach (DataRow branch in distinctproducts.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
                            {
                                Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                            }
                        }
                        Report.Columns.Add("Total ltrs").DataType = typeof(Double);
                        Report.Columns.Add("Total Dispatch").DataType = typeof(Double);
                        Report.Columns.Add("Average Returns For Total Dispatch");
                        DataTable distincttable = view.ToTable(true, "DispName", "dispsno");
                        int i = 1;
                        double totrtnltrs = 0;
                        double totdispatchltrs = 0;
                        foreach (DataRow branch in distincttable.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            double Returnltrs = 0;
                            newrow["SNo"] = i++.ToString();
                            cmd = new MySqlCommand("SELECT tripdat.Sno, tripdat.Status, tripsubdata.ProductId, SUM(tripsubdata.Qty) AS dispatchqty FROM triproutes INNER JOIN (SELECT Sno, Status, I_Date, BranchID FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno WHERE (triproutes.RouteID = @dispid) AND (tripdat.Status <> 'C')");
                            cmd.Parameters.AddWithValue("@dispid", branch["dispsno"].ToString());
                            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                            DataTable dtdispqty = vdm.SelectQuery(cmd).Tables[0];
                            newrow["Route Name"] = branch["DispName"].ToString();
                            foreach (DataRow dr in dtLeaks.Rows)
                            {
                                if (branch["DispName"].ToString() == dr["DispName"].ToString())
                                {
                                    double TotalReturns = 0;
                                    double.TryParse(dr["ReturnQty"].ToString(), out TotalReturns);
                                    if (TotalReturns >= 0)
                                    {
                                        newrow[dr["ProductName"].ToString()] = Math.Round(TotalReturns, 2);
                                        Returnltrs += TotalReturns;
                                        totrtnltrs += TotalReturns;
                                    }
                                }
                            }
                            newrow["Total ltrs"] = Math.Round(Returnltrs, 2);
                            double avg = 0;

                            if (dtdispqty.Rows.Count > 0)
                            {
                                double dispqty = 0;
                                double.TryParse(dtdispqty.Rows[0]["dispatchqty"].ToString(), out dispqty);
                                avg = (Returnltrs / dispqty) * 100;
                                newrow["Total Dispatch"] = Math.Round(dispqty, 2); ;
                                //newrow["Average Returns For Total Dispatch"] = Math.Round(avg, 2);
                                totdispatchltrs += dispqty;

                            }
                            else
                            {
                                //avg = 0;
                                newrow["Average Returns For Total Dispatch"] = avg;

                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Route Name"] = "Total";
                        
                        float val2 = 0;
                        float.TryParse(Report.Compute("sum([Total ltrs])", "[Total ltrs]<>'0'").ToString(), out val2);
                        newvartical["Total ltrs"] = val2;
                        double val1 = 0;
                        double totavg = 0;

                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val1 = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val1);
                                newvartical[dc.ToString()] = val1;
                            }
                            if (dc.ColumnName == "Leaks %")
                            {
                                totavg = (totrtnltrs / totdispatchltrs) * 100;
                                newvartical["Average Returns For Total Dispatch"] = Math.Round(totavg, 2);
                            }
                        }
                        foreach (DataColumn col in Report.Columns)
                        {
                            string Pname = col.ToString();
                            string ProductName = col.ToString();
                            ProductName = GetSpace(ProductName);
                            Report.Columns[Pname].ColumnName = ProductName;
                        }
                        Report.Rows.Add(newvartical);
                        grdReports.DataSource = Report;
                        grdReports.DataBind();
                        Session["xportdata"] = Report;
                    }
                }
                #endregion
            #endregion
           

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
    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {

        FillSalesOffice();

    }
}