using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class Test : System.Web.UI.Page
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
                FillRouteName();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
            }
        }
    }
    string buttontext = "";
    protected void btnGenerate_Click(object sender, EventArgs e)
    {

        GetReport();
        Context.Session["buttontext"] = "Products";
    }
    protected void btninventory_Click(object sender, EventArgs e)
    {

        inventoryClosingReport();
        Context.Session["buttontext"] = "Inventory";

    }
    protected void btncollection_Click(object sender, EventArgs e)
    {

        inventoryClosingReport();
        Context.Session["buttontext"] = "Collection";

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
                cmd = new MySqlCommand("SELECT BranchName, sno FROM  branchdata WHERE (sno = @BranchID) and branchdata.flag<>0");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtPlant.Rows)
                {
                    DataRow newrow = dtBranch.NewRow();
                    newrow["BranchName"] = dr["BranchName"].ToString();
                    newrow["sno"] = dr["sno"].ToString();
                    dtBranch.Rows.Add(newrow);
                }
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType and branchdata.flag<>0)");
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
            else if (Session["salestype"].ToString() == "Parlour")
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.sno = @BranchID) AND (branchdata.SalesType IS NOT NULL)");
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
            }
            else
            {
                vdm = new VehicleDBMgr();
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) and (branchdata.flag<>0) OR (branchdata.sno = @BranchID) and (branchdata.flag<>0)");
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
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            vdm = new VehicleDBMgr();
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
            string BranchID = ddlSalesOffice.SelectedValue;
            if (BranchID == "572")
            {
                BranchID = "7";
            }
            cmd = new MySqlCommand("SELECT productsdata.ProductName, products_category.Categoryname, productsdata.Units, productsdata.Qty FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) ORDER BY branchproducts.Rank");
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            DataTable dttable = vdm.SelectQuery(cmd).Tables[0];
            if (dttable.Rows.Count > 0)
            {
                DataView view = new DataView(dttable);
                DataTable produtstbl = view.ToTable(true, "ProductName", "Categoryname", "Units", "Qty");
                Session["Report"] = produtstbl;
                Report = new DataTable();
                Report.Columns.Add("Title");
                int count = 0;
                foreach (DataRow dr in produtstbl.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                    count++;
                }
                Report.Columns.Add("Total Qty").DataType = typeof(Double);
                cmd = new MySqlCommand("SELECT productsdata.ProductName, closubtranprodcts.StockQty, productsdata.sno FROM clotrans INNER JOIN  closubtranprodcts ON clotrans.Sno = closubtranprodcts.RefNo INNER JOIN productsdata ON closubtranprodcts.ProductID = productsdata.sno WHERE (clotrans.IndDate BETWEEN @d1 AND @d2) AND (clotrans.BranchId = @BranchID) GROUP BY productsdata.ProductName");
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtStock = vdm.SelectQuery(cmd).Tables[0];
                DataRow StockRow = Report.NewRow();
                StockRow["Title"] = "Opp Bal";
                foreach (DataRow dr in dtStock.Rows)
                {
                    double StockQty = 0;
                    double.TryParse(dr["StockQty"].ToString(), out StockQty);
                    StockQty = Math.Round(StockQty, 2);
                    if (StockQty == 0)
                    {
                    }
                    else
                    {
                        StockRow[dr["ProductName"].ToString()] = StockQty;
                    }
                }
                Report.Rows.Add(StockRow);
                cmd = new MySqlCommand("SELECT TripInfo.BranchID, TripInfo.DCNo, ProductInfo.ProductName, SUM(ProductInfo.Qty) AS Qty, TripInfo.DispName, TripInfo.I_Date FROM (SELECT tripdata.Sno, tripdata.DCNo, tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, dispatch.BranchID, dispatch.DispMode FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.BranchID = @branch) and (dispatch.disptype='SO') AND (tripdata.I_Date BETWEEN @d1 AND @d2)) TripInfo INNER JOIN (SELECT  ProductName, Sno, Qty FROM (SELECT productsdata.ProductName, tripdata_1.Sno, tripsubdata.Qty FROM tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE (tripdata_1.I_Date BETWEEN @d1 AND @d2)) TripSubInfo) ProductInfo ON TripInfo.Sno = ProductInfo.Sno GROUP BY ProductInfo.ProductName");
                //cmd = new MySqlCommand("SELECT dispatch.BranchID,productsdata.ProductName,Round(sum(tripsubdata.Qty),2) as Qty, tripdata.I_Date, triproutes.RouteID,  dispatch.BranchID FROM productsdata INNER JOIN tripsubdata ON productsdata.sno = tripsubdata.ProductId INNER JOIN tripdata ON tripsubdata.Tripdata_sno = tripdata.Sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2) GROUP BY productsdata.ProductName");
                cmd.Parameters.AddWithValue("@branch", BranchID);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtPlantTripData = vdm.SelectQuery(cmd).Tables[0];
                DataView view1 = new DataView(dtPlantTripData);
                DataTable distincttable = view1.ToTable(true, "BranchID");
                foreach (DataRow drp in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    //newrow["Sno"] = i++;
                    newrow["Title"] = "Inward";
                    double total = 0;
                    foreach (DataRow dr in dtPlantTripData.Rows)
                    {
                        if (drp["BranchID"].ToString() == dr["BranchID"].ToString())
                        {
                            double qtyvalue = 0;
                            double.TryParse(dr["Qty"].ToString(), out qtyvalue);
                            qtyvalue = Math.Round(qtyvalue, 2);
                            if (qtyvalue == 0)
                            {
                            }
                            else
                            {
                                newrow[dr["ProductName"].ToString()] = qtyvalue;
                                total += qtyvalue;
                            }
                        }
                    }
                    newrow["Total Qty"] = total;
                    Report.Rows.Add(newrow);
                }
                DataRow TotRow = Report.NewRow();
                TotRow["Title"] = "Qty";
                double val = 0.0;
                foreach (DataColumn dc in Report.Columns)
                {
                    if (dc.DataType == typeof(Double))
                    {
                        val = 0.0;
                        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                        if (val == 0.0)
                        {
                        }
                        else
                        {
                            TotRow[dc.ToString()] = val;
                        }
                    }
                }
                Report.Rows.Add(TotRow);
                //ravi
                cmd = new MySqlCommand("SELECT branchmappingtable.SuperBranch , indents.I_date, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, productsdata.ProductName, productsdata.sno FROM indents_subtable INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN branchmappingtable ON indents.Branch_id = branchmappingtable.SubBranch INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @BranchID) GROUP BY productsdata.ProductName, productsdata.sno");
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtSOSale = vdm.SelectQuery(cmd).Tables[0];
                DataView viewSO = new DataView(dtSOSale);
                DataTable distinctSo = viewSO.ToTable(true, "SuperBranch");
                foreach (DataRow drp in distinctSo.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    //newrow["Sno"] = i++;
                    newrow["Title"] = "Sale";
                    double total = 0;
                    foreach (DataRow dr in dtSOSale.Rows)
                    {
                        if (drp["SuperBranch"].ToString() == dr["SuperBranch"].ToString())
                        {
                            double DeliveryQty = 0;
                            double.TryParse(dr["DeliveryQty"].ToString(), out DeliveryQty);
                            DeliveryQty = Math.Round(DeliveryQty, 2);
                            if (DeliveryQty == 0)
                            {
                            }
                            else
                            {
                                newrow[dr["ProductName"].ToString()] = DeliveryQty;
                                total += DeliveryQty;
                            }
                        }
                    }
                    newrow["Total Qty"] = total;
                    Report.Rows.Add(newrow);
                }
                //ravi
                cmd = new MySqlCommand("SELECT SUM(Leaks.LeakQty) AS LeakQty,ff.BranchID, Leaks.ProductName, ff.DispName, ff.DespSno AS Sno, SUM(Leaks.FreeMilk) AS FreeMilk, SUM(Leaks.ShortQty) AS ShortQty FROM (SELECT leakages.FreeMilk, leakages.LeakQty, leakages.ShortQty, productsdata.ProductName, tripdata_1.Sno FROM tripdata tripdata_1 INNER JOIN leakages ON tripdata_1.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata_1.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno,BranchID FROM (SELECT dispatch.DispName, tripdata.Sno, dispatch.sno AS DespSno,dispatch.BranchID FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID)) TripInfo) ff ON ff.Sno = Leaks.Sno GROUP BY Leaks.ProductName");
                //cmd = new MySqlCommand("SELECT dispatch.BranchID ,productsdata.ProductName,ROUND(SUM(leakages.FreeMilk),2) AS FreeMilk,ROUND(SUM(leakages.LeakQty),2) AS LeakQty, ROUND(SUM(leakages.ShortQty),2) AS ShortQty, productsdata.sno FROM triproutes INNER JOIN  dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id  = @BranchID) Group By productsdata.ProductName");
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtSOFreeMilk = vdm.SelectQuery(cmd).Tables[0];
                DataView viewFreeMilk = new DataView(dtSOFreeMilk);
                DataTable distinctFree = viewFreeMilk.ToTable(true, "BranchID");
                foreach (DataRow drp in distinctFree.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    //newrow["Sno"] = i++;
                    newrow["Title"] = "Free Milk";
                    double total = 0;
                    foreach (DataRow dr in dtSOFreeMilk.Rows)
                    {
                        if (drp["BranchID"].ToString() == dr["BranchID"].ToString())
                        {
                            double FreeMilk = 0;
                            double.TryParse(dr["FreeMilk"].ToString(), out FreeMilk);
                            FreeMilk = Math.Round(FreeMilk, 2);
                            if (FreeMilk == 0)
                            {
                            }
                            else
                            {
                                newrow[dr["ProductName"].ToString()] = FreeMilk;
                                total += FreeMilk;
                            }
                        }
                    }
                    newrow["Total Qty"] = total;
                    Report.Rows.Add(newrow);
                }
                foreach (DataRow drp in distinctFree.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    //newrow["Sno"] = i++;
                    newrow["Title"] = "Route Shorts";
                    double total = 0;
                    foreach (DataRow dr in dtSOFreeMilk.Rows)
                    {
                        if (drp["BranchID"].ToString() == dr["BranchID"].ToString())
                        {
                            double ShortQty = 0;
                            double.TryParse(dr["ShortQty"].ToString(), out ShortQty);
                            ShortQty = Math.Round(ShortQty, 2);
                            if (ShortQty == 0)
                            {
                            }
                            else
                            {
                                newrow[dr["ProductName"].ToString()] = ShortQty;
                                total += ShortQty;
                            }
                        }
                    }
                    newrow["Total Qty"] = total;
                    Report.Rows.Add(newrow);
                }

                ////  Time Taken So comment...................... Changes...
                //cmd = new MySqlCommand("SELECT dispatch.BranchID,productsdata.ProductName, SUM(branchleaktrans.LeakQty) AS LeakQty, SUM(branchleaktrans.FreeQty) AS FreeQty, SUM(branchleaktrans.ShortQty) AS ShortQty, productsdata.sno FROM  dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN branchleaktrans ON tripdata.Sno = branchleaktrans.TripId INNER JOIN productsdata ON branchleaktrans.ProdId = productsdata.sno WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2) GROUP BY productsdata.ProductName, productsdata.sno");
                //cmd.Parameters.AddWithValue("@BranchID", BranchID);
                //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                //cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                //DataTable dtSOLFS = vdm.SelectQuery(cmd).Tables[0];
                DataTable dtSOLFS = new DataTable();
                DataView viewLFS = new DataView(dtSOLFS);
                DataRow newrow1 = Report.NewRow();
                //newrow1["Sno"] = i++;
                double totalShorts = 0;
                newrow1["Title"] = "SO Shorts";
                foreach (DataRow dr in dtSOLFS.Rows)
                {
                    double ShortQty = 0;
                    double.TryParse(dr["ShortQty"].ToString(), out ShortQty);
                    ShortQty = Math.Round(ShortQty, 2);
                    if (ShortQty == 0)
                    {
                    }
                    else
                    {
                        newrow1[dr["ProductName"].ToString()] = ShortQty;
                        totalShorts += ShortQty;
                    }
                }
                newrow1["Total Qty"] = totalShorts;
                Report.Rows.Add(newrow1);
                cmd = new MySqlCommand("SELECT branchmappingtable.SuperBranch,productsdata.ProductName,ROUND(SUM(indents_subtable.LeakQty),2) AS LeakQty FROM  indents_subtable INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN                         branchmappingtable ON indents.Branch_id = branchmappingtable.SubBranch INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY productsdata.ProductName");
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtAgentLeaks = vdm.SelectQuery(cmd).Tables[0];
                DataView viewAgentLeaks = new DataView(dtAgentLeaks);
                DataTable distinctAgentLeaks = viewAgentLeaks.ToTable(true, "SuperBranch");
                foreach (DataRow drp in distinctAgentLeaks.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    //newrow["Sno"] = i++;
                    newrow["Title"] = "Agent Leaks";
                    double total = 0;
                    foreach (DataRow dr in dtAgentLeaks.Rows)
                    {
                        if (drp["SuperBranch"].ToString() == dr["SuperBranch"].ToString())
                        {
                            double LeakQty = 0;
                            double.TryParse(dr["LeakQty"].ToString(), out LeakQty);
                            LeakQty = Math.Round(LeakQty, 2);
                            if (LeakQty == 0)
                            {
                            }
                            else
                            {
                                newrow[dr["ProductName"].ToString()] = LeakQty;
                                total += LeakQty;
                            }
                        }
                    }
                    newrow["Total Qty"] = total;
                    Report.Rows.Add(newrow);
                }
                //ravi

                cmd = new MySqlCommand("SELECT SUM(Leaks.LeakQty) AS TotalLeaks, SUM(Leaks.VLeaks) AS VLeaks, SUM(Leaks.ReturnQty) AS ReturnQty, SUM(Leaks.LeakQty) AS VReturns, Leaks.ProductName, ff.DispName, ff.DespSno AS Sno, ff.BranchID FROM (SELECT leakages.ReturnQty, leakages.LeakQty, leakages.VLeaks, leakages.VLeaks AS Expr1, productsdata.ProductName, tripdata_1.Sno FROM tripdata tripdata_1 INNER JOIN leakages ON tripdata_1.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata_1.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno, BranchID FROM (SELECT dispatch.DispName, tripdata.Sno, dispatch.sno AS DespSno, dispatch.BranchID FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID)) TripInfo) ff ON ff.Sno = Leaks.Sno GROUP BY Leaks.ProductName");
                //cmd = new MySqlCommand("SELECT productsdata.ProductName, productsdata.sno, dispatch.BranchID, triproutes.Tripdata_sno, ROUND(SUM(leakages.LeakQty), 2) AS TotalLeaks, ROUND(SUM(leakages.ReturnQty), 2) AS ReturnQty, ROUND(SUM(leakages.VLeaks), 2) AS VLeaks, ROUND(SUM(leakages.VReturns), 2) AS VReturns FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE  (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) GROUP BY productsdata.ProductName");
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtRouteLeaks = vdm.SelectQuery(cmd).Tables[0];
                DataView viewRouteLeaks = new DataView(dtRouteLeaks);
                DataTable distinctRouteLeaks = viewRouteLeaks.ToTable(true, "BranchID");
                cmd = new MySqlCommand("SELECT SUM(Leaks.LeakQty) AS TotalLeaks, SUM(Leaks.VLeaks) AS VLeaks, SUM(Leaks.ReturnQty) AS ReturnQty, SUM(Leaks.LeakQty) AS VReturns, Leaks.ProductName, ff.DispName, ff.DespSno AS Sno, ff.BranchID FROM (SELECT leakages.ReturnQty, leakages.LeakQty, leakages.VLeaks, leakages.VLeaks AS Expr1, productsdata.ProductName, tripdata_1.Sno FROM tripdata tripdata_1 INNER JOIN leakages ON tripdata_1.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata_1.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno, BranchID FROM (SELECT dispatch.DispName, tripdata.Sno, dispatch.sno AS DespSno, dispatch.BranchID FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID)) TripInfo) ff ON ff.Sno = Leaks.Sno GROUP BY Leaks.ProductName");
                //cmd = new MySqlCommand("SELECT productsdata.ProductName, productsdata.sno, dispatch.BranchID, triproutes.Tripdata_sno, ROUND(SUM(leakages.TotalLeaks), 2) AS TotalLeaks, ROUND(SUM(leakages.ReturnQty), 2) AS ReturnQty, ROUND(SUM(leakages.VLeaks), 2) AS VLeaks, ROUND(SUM(leakages.VReturns), 2) AS VReturns FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE  (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) GROUP BY productsdata.ProductName");
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtRouteLeaksReturns = vdm.SelectQuery(cmd).Tables[0];
                DataView viewLeaksReturns = new DataView(dtRouteLeaksReturns);
                DataTable distinctLeaksReturns = viewLeaksReturns.ToTable(true, "BranchID");
                foreach (DataRow drp in distinctRouteLeaks.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    //newrow["Sno"] = i++;
                    newrow["Title"] = "Submit Leaks";
                    double total = 0;
                    foreach (DataRow dr in dtRouteLeaksReturns.Rows)
                    {
                        if (drp["BranchID"].ToString() == dr["BranchID"].ToString())
                        {
                            double TotalLeaks = 0;
                            double.TryParse(dr["TotalLeaks"].ToString(), out TotalLeaks);
                            TotalLeaks = Math.Round(TotalLeaks, 2);
                            if (TotalLeaks == 0)
                            {
                            }
                            else
                            {
                                newrow[dr["ProductName"].ToString()] = TotalLeaks;
                                total += TotalLeaks;
                            }
                        }
                    }
                    newrow["Total Qty"] = total;
                    Report.Rows.Add(newrow);
                }
                foreach (DataRow drp in distinctLeaksReturns.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    //newrow["Sno"] = i++;
                    newrow["Title"] = "Varified  Leaks";
                    double total = 0;
                    foreach (DataRow dr in dtRouteLeaksReturns.Rows)
                    {
                        if (drp["BranchID"].ToString() == dr["BranchID"].ToString())
                        {
                            double VLeaks = 0;
                            double.TryParse(dr["TotalLeaks"].ToString(), out VLeaks);
                            VLeaks = Math.Round(VLeaks, 2);

                            if (VLeaks == 0)
                            {
                            }
                            else
                            {
                                newrow[dr["ProductName"].ToString()] = VLeaks;
                                total += VLeaks;
                            }
                        }
                    }
                    newrow["Total Qty"] = total;
                    Report.Rows.Add(newrow);
                }
                DataRow newrowi = Report.NewRow();
                //newrowi["Sno"] = i++;
                newrowi["Title"] = "SO Leaks";
                double totalLeaks = 0;
                foreach (DataRow dr in dtSOLFS.Rows)
                {
                    double LeakQty = 0;
                    double.TryParse(dr["LeakQty"].ToString(), out LeakQty);
                    LeakQty = Math.Round(LeakQty, 2);
                    if (LeakQty == 0)
                    {
                    }
                    else
                    {
                        newrowi[dr["ProductName"].ToString()] = LeakQty;
                        totalLeaks += LeakQty;
                    }
                }
                newrowi["Total Qty"] = totalLeaks;
                Report.Rows.Add(newrowi);
                //ravi
                cmd=new MySqlCommand ("SELECT SUM(Leaks.LeakQty) AS TotalLeaks, SUM(Leaks.VLeaks) AS VLeaks, SUM(Leaks.ReturnQty) AS ReturnQty, SUM(Leaks.LeakQty) AS VReturns, Leaks.ProductName,  ff.DispName, ff.DespSno AS Sno, ff.BranchID FROM (SELECT  leakages.ReturnQty, leakages.LeakQty, leakages.VLeaks, leakages.VLeaks AS Expr1, productsdata.ProductName, tripdata_1.Sno FROM tripdata tripdata_1 INNER JOIN leakages ON tripdata_1.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata_1.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno, BranchID FROM (SELECT dispatch.DispName, tripdata.Sno, dispatch.sno AS DespSno, dispatch.BranchID FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @BranchID)) TripInfo) ff ON ff.Sno = Leaks.Sno  GROUP BY Leaks.ProductName");
                //cmd = new MySqlCommand("SELECT productsdata.ProductName, productsdata.sno, dispatch.BranchID, triproutes.Tripdata_sno, ROUND(SUM(leakages.TotalLeaks), 2) AS TotalLeaks, ROUND(SUM(leakages.ReturnQty), 2) AS ReturnQty FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @BranchID) GROUP BY productsdata.ProductName");
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtSOLeaks = vdm.SelectQuery(cmd).Tables[0];
                DataView viewLeaks = new DataView(dtSOLeaks);
                DataTable distinctLeaks = viewLeaks.ToTable(true, "BranchID");
                foreach (DataRow drp in distinctLeaks.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    //newrow["Sno"] = i++;
                    newrow["Title"] = "Return DC Leaks";
                    double total = 0;
                    foreach (DataRow dr in dtSOLeaks.Rows)
                    {
                        if (drp["BranchID"].ToString() == dr["BranchID"].ToString())
                        {
                            double TotalLeaks = 0;
                            double.TryParse(dr["TotalLeaks"].ToString(), out TotalLeaks);
                            TotalLeaks = Math.Round(TotalLeaks, 2);
                            if (TotalLeaks == 0)
                            {
                            }
                            else
                            {
                                newrow[dr["ProductName"].ToString()] = TotalLeaks;
                                total += TotalLeaks;
                            }
                        }
                    }
                    newrow["Total Qty"] = total;
                    Report.Rows.Add(newrow);
                }
                foreach (DataRow drp in distinctLeaksReturns.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    //newrow["Sno"] = i++;
                    newrow["Title"] = "Route Returns";
                    double total = 0;
                    foreach (DataRow dr in dtRouteLeaksReturns.Rows)
                    {
                        if (drp["BranchID"].ToString() == dr["BranchID"].ToString())
                        {
                            double ReturnQty = 0;
                            double.TryParse(dr["ReturnQty"].ToString(), out ReturnQty);
                            ReturnQty = Math.Round(ReturnQty, 2);
                            if (ReturnQty == 0)
                            {
                            }
                            else
                            {
                                newrow[dr["ProductName"].ToString()] = ReturnQty;
                                total += ReturnQty;
                            }
                        }
                    }
                    newrow["Total Qty"] = total;
                    Report.Rows.Add(newrow);
                }
                foreach (DataRow drp in distinctLeaksReturns.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    //newrow["Sno"] = i++;
                    newrow["Title"] = "Varified  Returns";
                    double total = 0;
                    foreach (DataRow dr in dtRouteLeaksReturns.Rows)
                    {
                        if (drp["BranchID"].ToString() == dr["BranchID"].ToString())
                        {
                            double VReturns = 0;
                            double.TryParse(dr["ReturnQty"].ToString(), out VReturns);
                            VReturns = Math.Round(VReturns, 2);
                            if (VReturns == 0)
                            {
                            }
                            else
                            {
                                newrow[dr["ProductName"].ToString()] = VReturns;
                                total += VReturns;
                            }
                        }
                    }
                    newrow["Total Qty"] = total;
                    Report.Rows.Add(newrow);
                }
                foreach (DataRow drp in distinctLeaks.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    //newrow["Sno"] = i++;
                    newrow["Title"] = "DC Returns";
                    double total = 0;
                    foreach (DataRow dr in dtSOLeaks.Rows)
                    {
                        if (drp["BranchID"].ToString() == dr["BranchID"].ToString())
                        {
                            double ReturnQty = 0;
                            double.TryParse(dr["ReturnQty"].ToString(), out ReturnQty);
                            ReturnQty = Math.Round(ReturnQty, 2);
                            if (ReturnQty == 0)
                            {
                            }
                            else
                            {
                                newrow[dr["ProductName"].ToString()] = ReturnQty;
                                total += ReturnQty;
                            }
                        }
                    }
                    newrow["Total Qty"] = total;
                    Report.Rows.Add(newrow);
                }
                cmd = new MySqlCommand("SELECT branchproducts.branch_sno,productsdata.ProductName, branchproducts.BranchQty,branchproducts.LeakQty FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno WHERE (branchproducts.branch_sno = @BranchID) GROUP BY productsdata.ProductName");
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                DataTable dtClos = vdm.SelectQuery(cmd).Tables[0];
                DataRow DiffRow4 = Report.NewRow();
                DiffRow4["Title"] = "Clo Bal";
                foreach (DataRow drp in dttable.Rows)
                {
                    double total = 0;
                    foreach (DataRow dr in dtClos.Rows)
                    {
                        if (drp["ProductName"].ToString() == dr["ProductName"].ToString())
                        {
                            double BranchQty = 0;
                            double.TryParse(dr["BranchQty"].ToString(), out BranchQty);
                            BranchQty = Math.Round(BranchQty, 2);
                            if (BranchQty == 0)
                            {
                            }
                            else
                            {
                                DiffRow4[dr["ProductName"].ToString()] = BranchQty;
                                total += BranchQty;
                            }
                        }
                    }
                    DiffRow4["Total Qty"] = total;
                }
                Report.Rows.Add(DiffRow4);
                DataRow DiffRow10 = Report.NewRow();
                DiffRow10["Title"] = "Difference";
                foreach (DataRow drp in dttable.Rows)
                {
                    double total = 0;
                    foreach (DataRow dr in dtClos.Rows)
                    {
                        if (drp["ProductName"].ToString() == dr["ProductName"].ToString())
                        {
                            double LeakQty = 0;
                            double.TryParse(dr["LeakQty"].ToString(), out LeakQty);
                            LeakQty = Math.Round(LeakQty, 2);
                            if (LeakQty == 0)
                            {
                            }
                            else
                            {
                                DiffRow10[dr["ProductName"].ToString()] = LeakQty;
                                total += LeakQty;
                            }
                        }
                    }
                    DiffRow10["Total Qty"] = total;
                }
                Report.Rows.Add(DiffRow10);
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
                Session["Report"] = Report;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            grdReports.DataSource = Report;
            grdReports.DataBind();
        }
    }
    void inventoryClosingReport()
    {
        try
        {
            lblmsg.Text = "";
            vdm = new VehicleDBMgr();
            pnlHide.Visible = true;
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
            string BranchID = ddlSalesOffice.SelectedValue;
            if (BranchID == "572")
            {
                BranchID = "7";
            }
            cmd = new MySqlCommand("SELECT InvName, sno, Userdata_sno, flag, Qty FROM invmaster ORDER BY sno");
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            DataTable dttable = vdm.SelectQuery(cmd).Tables[0];
            if (dttable.Rows.Count > 0)
            {
                DataView view = new DataView(dttable);
                DataTable produtstbl = view.ToTable(true, "InvName", "sno");
                Session["Report"] = produtstbl;
                Report = new DataTable();
                //Report.Columns.Add("Sno");
                Report.Columns.Add("Title");
                int count = 0;
                foreach (DataRow dr in produtstbl.Rows)
                {
                    //if (dr["sno"].ToString() == "5" || dr["sno"].ToString() == "7" || dr["sno"].ToString() == "8")
                    //{

                    //}
                    //else
                    //{
                        Report.Columns.Add(dr["InvName"].ToString()).DataType = typeof(Double);
                    //}
                }

            }
            cmd = new MySqlCommand("SELECT invmaster.InvName,invmaster.sno, clotrans.BranchId, closubtraninventory.StockQty FROM clotrans INNER JOIN closubtraninventory ON clotrans.Sno = closubtraninventory.RefNo INNER JOIN invmaster ON closubtraninventory.RefNo = invmaster.sno WHERE (clotrans.BranchId = 174) AND (clotrans.IndDate BETWEEN @d1 AND @d2) GROUP BY invmaster.sno");
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtStock = vdm.SelectQuery(cmd).Tables[0];
            DataRow StockRow = Report.NewRow();
            StockRow["Title"] = "Opp Bal";
            foreach (DataRow dr in dtStock.Rows)
            {
                //if (dr["sno"].ToString() == "5" || dr["sno"].ToString() == "6" || dr["sno"].ToString() == "7" || dr["sno"].ToString() == "8")
                //{

                //}
                //else
                //{
                StockRow[dr["InvName"].ToString()] = dr["StockQty"].ToString();
                //}
            }
            Report.Rows.Add(StockRow);

            //cmd = new MySqlCommand("SELECT dispatch.BranchID, tripdata.I_Date, triproutes.RouteID, SUM(tripinvdata.Qty) AS invqty, invmaster.sno, invmaster.InvName FROM triproutes INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripinvdata ON tripdata.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2) GROUP BY invmaster.sno, invmaster.InvName");
            //ravi
            cmd = new MySqlCommand("SELECT ss.Tripdata_sno, ss.RouteID, ss.BranchID, SUM(ff.Qty) AS invqty, ff.sno, ff.InvName, ff.I_Date FROM (SELECT triproutes.Tripdata_sno, triproutes.RouteID, dispatch.BranchID FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno WHERE (dispatch.BranchID = @BranchID)) ss INNER JOIN (SELECT InvName, Qty, sno, I_Date, Tripdata_sno FROM (SELECT invmaster.InvName, tripinvdata.Qty, invmaster.sno, tripdata.I_Date, tripinvdata.Tripdata_sno FROM  tripdata INNER JOIN tripinvdata ON tripdata.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2)) TripInfo) ff ON ff.Tripdata_sno = ss.Tripdata_sno GROUP BY ff.InvName");
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtPlantTripData = vdm.SelectQuery(cmd).Tables[0];
            DataView view1 = new DataView(dtPlantTripData);
            DataTable distincttable = view1.ToTable(true, "BranchID");
            foreach (DataRow drp in distincttable.Rows)
            {
                DataRow newrow = Report.NewRow();
                //newrow["Sno"] = i++;
                newrow["Title"] = "Inward";
                double total = 0;
                foreach (DataRow dr in dtPlantTripData.Rows)
                {
                    //if (dr["sno"].ToString() == "5" || dr["sno"].ToString() == "6" || dr["sno"].ToString() == "7" || dr["sno"].ToString() == "8")
                    //{

                    //}
                    //else
                    //{
                        if (drp["BranchID"].ToString() == dr["BranchID"].ToString())
                        {
                            double qtyvalue = 0;
                            double.TryParse(dr["invqty"].ToString(), out qtyvalue);
                            if (qtyvalue == 0)
                            {
                            }
                            else
                            {
                                newrow[dr["InvName"].ToString()] = qtyvalue;
                                total += qtyvalue;
                            }
                        }
                    //}
                }
                Report.Rows.Add(newrow);
            }
            DataRow TotRow = Report.NewRow();
            TotRow["Title"] = "Qty";
            double val = 0.0;
            foreach (DataColumn dc in Report.Columns)
            {
                if (dc.DataType == typeof(Double))
                {
                    val = 0.0;
                    double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                    TotRow[dc.ToString()] = val;
                }
            }
            Report.Rows.Add(TotRow);

            //cmd = new MySqlCommand("SELECT tripdata.I_Date, triproutes.RouteID, SUM(tripinvdata.Qty) AS invqty, invmaster.sno, invmaster.InvName, dispatch.Branch_Id FROM triproutes INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripinvdata ON tripdata.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) GROUP BY invmaster.sno, invmaster.InvName");
            //ravi
            cmd = new MySqlCommand("SELECT ss.Tripdata_sno, ss.RouteID, ss.Branch_Id, SUM(ff.Qty) AS invqty, ff.sno, ff.InvName, ff.I_Date FROM (SELECT triproutes.Tripdata_sno, triproutes.RouteID, dispatch.Branch_Id FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno WHERE (dispatch.Branch_Id = @BranchID)) ss INNER JOIN (SELECT InvName, Qty, sno, I_Date, Tripdata_sno FROM  (SELECT invmaster.InvName, tripinvdata.Qty, invmaster.sno, tripdata.I_Date, tripinvdata.Tripdata_sno FROM  tripdata INNER JOIN tripinvdata ON tripdata.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2)) TripInfo) ff ON ff.Tripdata_sno = ss.Tripdata_sno GROUP BY ff.InvName");
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtSOSale = vdm.SelectQuery(cmd).Tables[0];
            DataView viewSO = new DataView(dtSOSale);
            DataTable distinctSo = viewSO.ToTable(true, "Branch_Id");
            foreach (DataRow drp in distinctSo.Rows)
            {
                DataRow newrow = Report.NewRow();
                //newrow["Sno"] = i++;
                newrow["Title"] = "Dispatch";
                double total = 0;
                foreach (DataRow dr in dtSOSale.Rows)
                {
                    //if (dr["sno"].ToString() == "5" || dr["sno"].ToString() == "6" || dr["sno"].ToString() == "7" || dr["sno"].ToString() == "8")
                    //{

                    //}
                    //else
                    //{
                        if (drp["Branch_Id"].ToString() == dr["Branch_Id"].ToString())
                        {
                            double DeliveryQty = 0;
                            double.TryParse(dr["invqty"].ToString(), out DeliveryQty);
                            if (DeliveryQty == 0)
                            {
                            }
                            else
                            {
                                newrow[dr["InvName"].ToString()] = DeliveryQty;
                                total += DeliveryQty;
                            }
                        //}
                    }
                }
                Report.Rows.Add(newrow);
            }
            //cmd = new MySqlCommand("SELECT tripdata.I_Date, triproutes.RouteID, SUM(tripinvdata.Remaining) AS invqty, invmaster.sno, invmaster.InvName, dispatch.Branch_Id FROM triproutes INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripinvdata ON tripdata.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) GROUP BY invmaster.sno, invmaster.InvName");
            //cmd.Parameters.AddWithValue("@BranchID", BranchID);
            //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            //cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            //DataTable dtRouteLeaks = vdm.SelectQuery(cmd).Tables[0];
            //DataView viewRouteLeaks = new DataView(dtRouteLeaks);
            //DataTable distinctRouteLeaks = viewRouteLeaks.ToTable(true, "BranchID");

            /////////////....................................TIME TAKEN CHANGES....................

            //cmd = new MySqlCommand("SELECT tripdata.I_Date, triproutes.RouteID, dispatch.Branch_Id,invmaster.Sno, invmaster.InvName, SUM(invtran.Qty) AS submittedqty, SUM(invtran.VQty) AS verifiedqty FROM triproutes INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty, CBFromTran, CBToTran, DeliveryTime,CollectionTime FROM invtransactions12 WHERE (ToTran = @BranchID)) invtran ON tripdata.Sno = invtran.FromTran INNER JOIN invmaster ON invtran.B_inv_sno = invmaster.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) GROUP BY invmaster.sno");
            // CRR
            cmd = new MySqlCommand("SELECT tripdata.I_Date, invmaster.InvName,invmaster.sno, SUM(tripinvdata.Qty) AS submittedqty, SUM(tripinvdata.Remaining) AS verifiedqty, tripdata.BranchID FROM  invmaster INNER JOIN tripinvdata ON invmaster.sno = tripinvdata.invid INNER JOIN tripdata ON tripinvdata.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (tripdata.BranchID = @BranchID) GROUP BY invmaster.InvName");
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtRouteLeaksReturns = vdm.SelectQuery(cmd).Tables[0];
            //DataTable dtRouteLeaksReturns = new DataTable();
            if (dtRouteLeaksReturns.Rows.Count > 0)
            {
                DataView viewLeaksReturns = new DataView(dtRouteLeaksReturns);
                DataTable distinctLeaksReturns = viewLeaksReturns.ToTable(true, "BranchID");
                foreach (DataRow drp in distinctLeaksReturns.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    //newrow["Sno"] = i++;
                    newrow["Title"] = "Submitt Inventory";
                    double total = 0;
                    foreach (DataRow dr in dtRouteLeaksReturns.Rows)
                    {
                        //if (dr["sno"].ToString() == "5" || dr["sno"].ToString() == "6" || dr["sno"].ToString() == "7" || dr["sno"].ToString() == "8")
                        //{

                        //}
                        //else
                        //{
                            if (drp["BranchID"].ToString() == dr["BranchID"].ToString())
                            {
                                double submittedqty = 0;
                                double.TryParse(dr["submittedqty"].ToString(), out submittedqty);
                                if (submittedqty == 0)
                                {
                                }
                                else
                                {
                                    newrow[dr["InvName"].ToString()] = submittedqty;
                                    total += submittedqty;
                                }
                            //}
                        }
                    }
                    Report.Rows.Add(newrow);
                }

                foreach (DataRow drp in distinctLeaksReturns.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    //newrow["Sno"] = i++;
                    newrow["Title"] = "Varified  Inventory";
                    double total = 0;
                    foreach (DataRow dr in dtRouteLeaksReturns.Rows)
                    {
                        //if (dr["sno"].ToString() == "5" || dr["sno"].ToString() == "6" || dr["sno"].ToString() == "7" || dr["sno"].ToString() == "8")
                        //{

                        //}
                        //else
                        //{
                            if (drp["BranchID"].ToString() == dr["BranchID"].ToString())
                            {
                                double verifiedqty = 0;
                                double.TryParse(dr["verifiedqty"].ToString(), out verifiedqty);
                                if (verifiedqty == 0)
                                {
                                }
                                else
                                {
                                    newrow[dr["InvName"].ToString()] = verifiedqty;
                                    total += verifiedqty;
                                }
                            }
                        //}
                    }
                    Report.Rows.Add(newrow);
                }
            }
            DataRow newrowi = Report.NewRow();
            //newrowi["Sno"] = i++;
            newrowi["Title"] = "SO Delivery";
            double totalLeaks = 0;
            //foreach (DataRow dr in dtSOLFS.Rows)
            //{
            //    double LeakQty = 0;
            //    double.TryParse(dr["LeakQty"].ToString(), out LeakQty);
            //    newrowi[dr["ProductName"].ToString()] = LeakQty;
            //    totalLeaks += LeakQty;
            //}
            Report.Rows.Add(newrowi);
            cmd = new MySqlCommand("SELECT tripdata.I_Date, triproutes.RouteID, dispatch.BranchID, invmaster.InvName, SUM(invtrans.VQty) AS verifiedqty, SUM(invtrans.Qty) AS submittedqty,invmaster.sno FROM triproutes INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty, CBFromTran, CBToTran, DeliveryTime,CollectionTime FROM invtransactions12 WHERE (FromTran = @BranchID)) invtrans ON tripdata.Sno = invtrans.ToTran INNER JOIN invmaster ON invtrans.B_inv_sno = invmaster.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @BranchID) GROUP BY invmaster.sno");
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtSOLeaks = vdm.SelectQuery(cmd).Tables[0];
            DataView viewLeaks = new DataView(dtSOLeaks);
            DataTable distinctLeaks = viewLeaks.ToTable(true, "BranchID");
            foreach (DataRow drp in distinctLeaks.Rows)
            {
                DataRow newrow = Report.NewRow();
                //newrow["Sno"] = i++;
                newrow["Title"] = "Return DC Inventory";
                double total = 0;
                foreach (DataRow dr in dtSOLeaks.Rows)
                {
                    //if (dr["sno"].ToString() == "5" || dr["sno"].ToString() == "6" || dr["sno"].ToString() == "7" || dr["sno"].ToString() == "8")
                    //{

                    //}
                    //else
                    //{
                        if (drp["BranchID"].ToString() == dr["BranchID"].ToString())
                        {
                            double submittedqty = 0;
                            double.TryParse(dr["submittedqty"].ToString(), out submittedqty);
                            if (submittedqty == 0)
                            {
                            }
                            else
                            {
                                newrow[dr["InvName"].ToString()] = submittedqty;
                                total += submittedqty;
                            }
                        //}
                    }
                }
                Report.Rows.Add(newrow);
            }
            foreach (DataRow drp in distinctLeaks.Rows)
            {
                DataRow newrow = Report.NewRow();
                //newrow["Sno"] = i++;
                newrow["Title"] = "Return DC VInventory";
                double total = 0;
                foreach (DataRow dr in dtSOLeaks.Rows)
                {
                    if (drp["BranchID"].ToString() == dr["BranchID"].ToString())
                    {
                        double verifiedqty = 0;
                        double.TryParse(dr["verifiedqty"].ToString(), out verifiedqty);
                        if (verifiedqty == 0)
                        {
                        }
                        else
                        {
                            newrow[dr["InvName"].ToString()] = verifiedqty;
                            total += verifiedqty;
                        }
                    }
                }
                Report.Rows.Add(newrow);
            }
             cmd = new MySqlCommand("SELECT  inventory_monitor.BranchId, inventory_monitor.Inv_Sno, inventory_monitor.Qty, inventory_monitor.Sno, inventory_monitor.EmpId, inventory_monitor.lostQty,invmaster.InvName FROM inventory_monitor INNER JOIN invmaster ON inventory_monitor.Inv_Sno = invmaster.sno WHERE (inventory_monitor.BranchId = @BranchID)");
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            DataTable dtClos = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT clotrans.Sno, clotrans.BranchId, clotrans.EmpId, clotrans.IndDate, clotrans.SalesType, closubtraninventory.InvSno, closubtraninventory.StockQty,invmaster.InvName FROM clotrans INNER JOIN closubtraninventory ON clotrans.Sno = closubtraninventory.RefNo INNER JOIN invmaster ON closubtraninventory.InvSno = invmaster.sno WHERE (clotrans.BranchId = @BranchID) AND (clotrans.IndDate BETWEEN @d1 AND @d2)"); 
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtClostrans = vdm.SelectQuery(cmd).Tables[0];

            DataRow DiffRow4 = Report.NewRow();
            DiffRow4["Title"] = "Clo Bal";
            if (dtClostrans.Rows.Count > 0)
            {
                foreach (DataRow dr in dtClostrans.Rows)
                {
                    //if (dr["InvSno"].ToString() == "5" || dr["InvSno"].ToString() == "6" || dr["InvSno"].ToString() == "7" || dr["InvSno"].ToString() == "8")
                    //{

                    //}
                    //else
                    //{
                        double BranchQty = 0;
                        double.TryParse(dr["StockQty"].ToString(), out BranchQty);
                        if (BranchQty == 0)
                        {
                        }
                        else
                        {
                            DiffRow4[dr["InvName"].ToString()] = BranchQty;
                        }
                    //}
                }
            }
            if (dtClostrans.Rows.Count <= 0)
            {
                foreach (DataRow dr in dtClos.Rows)
                {
                    //if (dr["Inv_Sno"].ToString() == "5" || dr["Inv_Sno"].ToString() == "6" || dr["Inv_Sno"].ToString() == "7" || dr["Inv_Sno"].ToString() == "8")
                    //{

                    //}
                    //else
                    //{
                        double BranchQty = 0;
                        double.TryParse(dr["Qty"].ToString(), out BranchQty);
                        if (BranchQty == 0)
                        {
                        }
                        else
                        {
                            DiffRow4[dr["InvName"].ToString()] = BranchQty;
                        }
                    //}
                }
            }
            

            Report.Rows.Add(DiffRow4);
            DataRow difference5 = Report.NewRow();
            //newrow["Sno"] = i++;
            difference5["Title"] = "Difference";
            Report.Rows.Add(difference5);

            foreach (DataColumn col in Report.Columns)
            {
                string Pname = col.ToString();
                string ProductName = col.ToString();
                ProductName = GetSpace(ProductName);
                Report.Columns[Pname].ColumnName = ProductName;
            }

            grdReports.DataSource = Report;
            grdReports.DataBind();
            Session["Report"] = Report;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            grdReports.DataSource = Report;
            grdReports.DataBind();
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
    protected void Show_Hide_OrdersGrid(object sender, EventArgs e)
    {
        ImageButton imgShowHide = (sender as ImageButton);
        GridViewRow row = (imgShowHide.NamingContainer as GridViewRow);
        if (imgShowHide.CommandArgument == "Show")
        {
            vdm = new VehicleDBMgr();
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
            string BranchID = ddlSalesOffice.SelectedValue;
            if (BranchID == "572")
            {
                BranchID = "7";
            }
            imgShowHide.CommandArgument = "Hide";
            imgShowHide.ImageUrl = "~/images/minus.png";
            string Type = grdReports.Rows[row.RowIndex].Cells[1].Text;
            if (Context.Session["buttontext"] == "Products")
            {
                cmd = new MySqlCommand("SELECT products_category.Categoryname, productsdata.ProductName, productsdata.Units, productsdata.Qty, branchproducts.Rank FROM productsdata INNER JOIN tripsubdata ON productsdata.sno = tripsubdata.ProductId INNER JOIN tripdata ON tripsubdata.Tripdata_sno = tripdata.Sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN                         branchproducts ON dispatch.BranchID = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dttable = vdm.SelectQuery(cmd).Tables[0];
                if (dttable.Rows.Count > 0)
                {
                    DataView view = new DataView(dttable);
                    DataTable produtstbl = view.ToTable(true, "ProductName", "Categoryname", "Units", "Qty");
                    Session["Report"] = produtstbl;
                    Report = new DataTable();
                    Report.Columns.Add("Title");
                    foreach (DataRow dr in produtstbl.Rows)
                    {
                        Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                    }
                    #region "Total Sale"
                    if (Type == "Sale")
                    {

                        cmd = new MySqlCommand("SELECT productsdata.ProductName,Round(Sum(indents_subtable.DeliveryQty),2) as DeliveryQty , indents.I_date, dispatch.DispName FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutesubtable ON dispatch_sub.Route_id = branchroutesubtable.RefNo INNER JOIN indents ON branchroutesubtable.BranchID = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) GROUP BY productsdata.ProductName, dispatch.DispName");
                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtSOTripData = vdm.SelectQuery(cmd).Tables[0];
                        DataView viewSo = new DataView(dtSOTripData);
                        DataTable distincttableSo = viewSo.ToTable(true, "DispName");
                        foreach (DataRow drp in distincttableSo.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Title"] = drp["DispName"].ToString();
                            foreach (DataRow dr in dtSOTripData.Rows)
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {
                                    double DeliveryQty = 0;
                                    double.TryParse(dr["DeliveryQty"].ToString(), out DeliveryQty);
                                    newrow[dr["ProductName"].ToString()] = DeliveryQty;
                                }
                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Title"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
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
                        GrdProducts.DataSource = Report;
                        GrdProducts.DataBind();

                    }
                    #endregion "Total Sale"

                    #region "Qty"
                    if (Type == "Qty")
                    {

                        cmd = new MySqlCommand("SELECT dispatch.DispName,productsdata.ProductName, tripsubdata.Qty, tripdata.I_Date, triproutes.RouteID,  dispatch.BranchID FROM productsdata INNER JOIN tripsubdata ON productsdata.sno = tripsubdata.ProductId INNER JOIN tripdata ON tripsubdata.Tripdata_sno = tripdata.Sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno WHERE (dispatch.Branch_Id = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2) GROUP BY productsdata.ProductName,dispatch.DispName");
                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtSOTripData = vdm.SelectQuery(cmd).Tables[0];
                        DataView viewSo = new DataView(dtSOTripData);
                        DataTable distincttableSo = viewSo.ToTable(true, "DispName");
                        foreach (DataRow drp in distincttableSo.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Title"] = drp["DispName"].ToString();
                            foreach (DataRow dr in dtSOTripData.Rows)
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {
                                    newrow[dr["ProductName"].ToString()] = dr["Qty"].ToString();
                                }
                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Title"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
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
                        GrdProducts.DataSource = Report;
                        GrdProducts.DataBind();
                    }
                    # endregion "Qty"

                    #region "Inward"
                    if (Type == "Inward")
                    {


                        cmd = new MySqlCommand("SELECT dispatch.DispName,productsdata.ProductName, tripsubdata.Qty, tripdata.I_Date, triproutes.RouteID,  dispatch.BranchID FROM productsdata INNER JOIN tripsubdata ON productsdata.sno = tripsubdata.ProductId INNER JOIN tripdata ON tripsubdata.Tripdata_sno = tripdata.Sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2) GROUP BY productsdata.ProductName,dispatch.DispName");
                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtSOTripData = vdm.SelectQuery(cmd).Tables[0];
                        DataView viewSo = new DataView(dtSOTripData);
                        DataTable distincttableSo = viewSo.ToTable(true, "DispName");
                        foreach (DataRow drp in distincttableSo.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Title"] = drp["DispName"].ToString();
                            foreach (DataRow dr in dtSOTripData.Rows)
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {
                                    newrow[dr["ProductName"].ToString()] = dr["Qty"].ToString();
                                }
                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Title"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
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
                        GrdProducts.DataSource = Report;
                        GrdProducts.DataBind();
                    }
                    #endregion "Inward"

                    #region "Free Milk"
                    if (Type == "Free Milk")
                    {
                        cmd = new MySqlCommand("SELECT dispatch.BranchID, productsdata.ProductName, ROUND(SUM(leakages.FreeMilk), 2) AS FreeMilk, ROUND(SUM(leakages.LeakQty), 2) AS LeakQty, ROUND(SUM(leakages.ShortQty), 2) AS ShortQty, productsdata.sno, dispatch.DispName FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) GROUP BY productsdata.ProductName, dispatch.DispName");
                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtSOTripData = vdm.SelectQuery(cmd).Tables[0];
                        DataView viewSo = new DataView(dtSOTripData);
                        DataTable distincttableSo = viewSo.ToTable(true, "DispName");
                        foreach (DataRow drp in distincttableSo.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Title"] = drp["DispName"].ToString();
                            foreach (DataRow dr in dtSOTripData.Rows)
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {
                                    double FreeMilk = 0;
                                    double.TryParse(dr["FreeMilk"].ToString(), out FreeMilk);
                                    newrow[dr["ProductName"].ToString()] = FreeMilk;
                                }
                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Title"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
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
                        GrdProducts.DataSource = Report;
                        GrdProducts.DataBind();
                    }
                    #endregion "Free Milk"

                    #region "Route Shorts"
                    if (Type == "Route Shorts")
                    {
                        cmd = new MySqlCommand("SELECT dispatch.BranchID, productsdata.ProductName, ROUND(SUM(leakages.FreeMilk), 2) AS FreeMilk, ROUND(SUM(leakages.LeakQty), 2) AS LeakQty, ROUND(SUM(leakages.ShortQty), 2) AS ShortQty, productsdata.sno, dispatch.DispName FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) GROUP BY productsdata.ProductName, dispatch.DispName");
                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtSOTripData = vdm.SelectQuery(cmd).Tables[0];
                        DataView viewSo = new DataView(dtSOTripData);
                        DataTable distincttableSo = viewSo.ToTable(true, "DispName");
                        foreach (DataRow drp in distincttableSo.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Title"] = drp["DispName"].ToString();
                            foreach (DataRow dr in dtSOTripData.Rows)
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {
                                    double ShortQty = 0;
                                    double.TryParse(dr["ShortQty"].ToString(), out ShortQty);
                                    newrow[dr["ProductName"].ToString()] = ShortQty;
                                }
                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Title"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
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
                        GrdProducts.DataSource = Report;
                        GrdProducts.DataBind();
                    }
                    #endregion "Route Shorts"

                    #region "SO Shorts"
                    if (Type == "SO Shorts")
                    {
                        cmd = new MySqlCommand("SELECT dispatch.BranchID,dispatch.DispName,productsdata.ProductName, SUM(branchleaktrans.LeakQty) AS LeakQty, SUM(branchleaktrans.FreeQty) AS FreeQty, SUM(branchleaktrans.ShortQty) AS ShortQty, productsdata.sno FROM  dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN branchleaktrans ON tripdata.Sno = branchleaktrans.TripId INNER JOIN productsdata ON branchleaktrans.ProdId = productsdata.sno WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2) GROUP BY productsdata.ProductName, dispatch.DispName");
                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtSOLFS = vdm.SelectQuery(cmd).Tables[0];
                        DataView viewLFS = new DataView(dtSOLFS);
                        DataTable distincttableSo = viewLFS.ToTable(true, "DispName");
                        foreach (DataRow drp in distincttableSo.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Title"] = drp["DispName"].ToString();
                            foreach (DataRow dr in dtSOLFS.Rows)
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {
                                    double ShortQty = 0;
                                    double.TryParse(dr["ShortQty"].ToString(), out ShortQty);
                                    newrow[dr["ProductName"].ToString()] = ShortQty;
                                }
                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Title"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
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
                        GrdProducts.DataSource = Report;
                        GrdProducts.DataBind();
                    }
                    #endregion "SO Shorts"

                    #region "Agent Leaks"
                    if (Type == "Agent Leaks")
                    {
                        cmd = new MySqlCommand("SELECT branchmappingtable.SuperBranch, productsdata.ProductName, ROUND(SUM(indents_subtable.LeakQty), 2) AS LeakQty, branchdata.BranchName FROM indents_subtable INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN branchmappingtable ON indents.Branch_id = branchmappingtable.SubBranch INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY productsdata.ProductName, branchdata.BranchName");
                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtSOTripData = vdm.SelectQuery(cmd).Tables[0];
                        DataView viewSo = new DataView(dtSOTripData);
                        DataTable distincttableSo = viewSo.ToTable(true, "BranchName");
                        foreach (DataRow drp in distincttableSo.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Title"] = drp["BranchName"].ToString();
                            foreach (DataRow dr in dtSOTripData.Rows)
                            {
                                if (drp["BranchName"].ToString() == dr["BranchName"].ToString())
                                {
                                    double LeakQty = 0;
                                    double.TryParse(dr["LeakQty"].ToString(), out LeakQty);
                                    newrow[dr["ProductName"].ToString()] = LeakQty;
                                }
                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Title"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
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
                        GrdProducts.DataSource = Report;
                        GrdProducts.DataBind();
                    }
                    #endregion "Agent Leaks"

                    #region "Submit Leaks"
                    if (Type == "Submit Leaks")
                    {
                        cmd = new MySqlCommand("SELECT productsdata.ProductName, productsdata.sno,dispatch.DispName, dispatch.BranchID, triproutes.Tripdata_sno, ROUND(SUM(leakages.TotalLeaks), 2) AS TotalLeaks, ROUND(SUM(leakages.ReturnQty), 2) AS ReturnQty, ROUND(SUM(leakages.VLeaks), 2) AS VLeaks, ROUND(SUM(leakages.VReturns), 2) AS VReturns FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE  (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) GROUP BY productsdata.ProductName,dispatch.DispName");
                        //cmd = new MySqlCommand("SELECT dispatch.BranchID, productsdata.ProductName, ROUND(SUM(leakages.FreeMilk), 2) AS FreeMilk, ROUND(SUM(leakages.LeakQty), 2) AS LeakQty, ROUND(SUM(leakages.ShortQty), 2) AS ShortQty, productsdata.sno, dispatch.DispName FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) GROUP BY productsdata.ProductName, dispatch.DispName");
                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtSOTripData = vdm.SelectQuery(cmd).Tables[0];
                        DataView viewSo = new DataView(dtSOTripData);
                        DataTable distincttableSo = viewSo.ToTable(true, "DispName");
                        foreach (DataRow drp in distincttableSo.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Title"] = drp["DispName"].ToString();
                            foreach (DataRow dr in dtSOTripData.Rows)
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {
                                    double TotalLeaks = 0;
                                    double.TryParse(dr["TotalLeaks"].ToString(), out TotalLeaks);
                                    newrow[dr["ProductName"].ToString()] = TotalLeaks;
                                }
                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Title"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
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
                        GrdProducts.DataSource = Report;
                        GrdProducts.DataBind();
                    }
                    #endregion "Submit Leaks"

                    #region "Verified Leaks"
                    if (Type == "Verified  Leaks")
                    {
                        cmd = new MySqlCommand("SELECT productsdata.ProductName, productsdata.sno,dispatch.DispName, dispatch.BranchID, triproutes.Tripdata_sno, ROUND(SUM(leakages.TotalLeaks), 2) AS TotalLeaks, ROUND(SUM(leakages.ReturnQty), 2) AS ReturnQty, ROUND(SUM(leakages.VLeaks), 2) AS VLeaks, ROUND(SUM(leakages.VReturns), 2) AS VReturns FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE  (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) GROUP BY productsdata.ProductName,dispatch.DispName");
                        //cmd = new MySqlCommand("SELECT dispatch.BranchID, productsdata.ProductName, ROUND(SUM(leakages.FreeMilk), 2) AS FreeMilk, ROUND(SUM(leakages.LeakQty), 2) AS LeakQty, ROUND(SUM(leakages.ShortQty), 2) AS ShortQty, productsdata.sno, dispatch.DispName FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) GROUP BY productsdata.ProductName, dispatch.DispName");
                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtSOTripData = vdm.SelectQuery(cmd).Tables[0];
                        DataView viewSo = new DataView(dtSOTripData);
                        DataTable distincttableSo = viewSo.ToTable(true, "DispName");
                        foreach (DataRow drp in distincttableSo.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Title"] = drp["DispName"].ToString();
                            foreach (DataRow dr in dtSOTripData.Rows)
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {
                                    double TotalLeaks = 0;
                                    double.TryParse(dr["TotalLeaks"].ToString(), out TotalLeaks);
                                    newrow[dr["ProductName"].ToString()] = TotalLeaks;
                                }
                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Title"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
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
                        GrdProducts.DataSource = Report;
                        GrdProducts.DataBind();
                    }
                    #endregion "Verified Leaks"

                    #region "SO Leaks"
                    if (Type == "SO Leaks")
                    {
                        cmd = new MySqlCommand("SELECT dispatch.BranchID,dispatch.DispName,productsdata.ProductName, SUM(branchleaktrans.LeakQty) AS LeakQty, SUM(branchleaktrans.FreeQty) AS FreeQty, SUM(branchleaktrans.ShortQty) AS ShortQty, productsdata.sno FROM  dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN branchleaktrans ON tripdata.Sno = branchleaktrans.TripId INNER JOIN productsdata ON branchleaktrans.ProdId = productsdata.sno WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2) GROUP BY productsdata.ProductName, dispatch.DispName");
                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtSOLFS = vdm.SelectQuery(cmd).Tables[0];
                        DataView viewLFS = new DataView(dtSOLFS);
                        DataTable distincttableSo = viewLFS.ToTable(true, "DispName");
                        foreach (DataRow drp in distincttableSo.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Title"] = drp["DispName"].ToString();
                            foreach (DataRow dr in dtSOLFS.Rows)
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {
                                    double LeakQty = 0;
                                    double.TryParse(dr["LeakQty"].ToString(), out LeakQty);
                                    newrow[dr["ProductName"].ToString()] = LeakQty;
                                }
                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Title"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
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
                        GrdProducts.DataSource = Report;
                        GrdProducts.DataBind();
                    }
                    #endregion "Return DC Leaks"

                    #region "Return DC Leaks"
                    if (Type == "Return DC Leaks")
                    {
                        cmd = new MySqlCommand("SELECT productsdata.ProductName,dispatch.DispName, productsdata.sno, dispatch.BranchID, triproutes.Tripdata_sno, ROUND(SUM(leakages.TotalLeaks), 2) AS TotalLeaks, ROUND(SUM(leakages.ReturnQty), 2) AS ReturnQty FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @BranchID) GROUP BY productsdata.ProductName,dispatch.DispName");
                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtSOLeaks = vdm.SelectQuery(cmd).Tables[0];
                        DataView viewLeaks = new DataView(dtSOLeaks);
                        DataTable distinctLeaks = viewLeaks.ToTable(true, "DispName");
                        foreach (DataRow drp in distinctLeaks.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            //newrow["Sno"] = i++;
                            newrow["Title"] = drp["DispName"].ToString();
                            foreach (DataRow dr in dtSOLeaks.Rows)
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {
                                    double TotalLeaks = 0;
                                    double.TryParse(dr["TotalLeaks"].ToString(), out TotalLeaks);
                                    newrow[dr["ProductName"].ToString()] = TotalLeaks;
                                }
                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Title"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
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
                        GrdProducts.DataSource = Report;
                        GrdProducts.DataBind();
                    }
                    #endregion "Return DC Leaks"

                    #region "Route Returns"
                    if (Type == "Route Returns")
                    {
                        cmd = new MySqlCommand("SELECT productsdata.ProductName,dispatch.DispName, productsdata.sno, dispatch.BranchID, triproutes.Tripdata_sno, ROUND(SUM(leakages.TotalLeaks), 2) AS TotalLeaks, ROUND(SUM(leakages.ReturnQty), 2) AS ReturnQty FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_ID = @BranchID) GROUP BY productsdata.ProductName,dispatch.DispName");
                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtSOLeaks = vdm.SelectQuery(cmd).Tables[0];
                        DataView viewLeaks = new DataView(dtSOLeaks);
                        DataTable distinctLeaks = viewLeaks.ToTable(true, "DispName");
                        foreach (DataRow drp in distinctLeaks.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            //newrow["Sno"] = i++;
                            newrow["Title"] = drp["DispName"].ToString();
                            foreach (DataRow dr in dtSOLeaks.Rows)
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {
                                    double ReturnQty = 0;
                                    double.TryParse(dr["ReturnQty"].ToString(), out ReturnQty);
                                    newrow[dr["ProductName"].ToString()] = ReturnQty;
                                }
                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Title"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
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
                        GrdProducts.DataSource = Report;
                        GrdProducts.DataBind();
                    }
                    #endregion "Route Returns"

                    #region "Varified Returns"
                    if (Type == "Varified  Returns")
                    {
                        cmd = new MySqlCommand("SELECT productsdata.ProductName,dispatch.DispName, productsdata.sno, dispatch.BranchID, triproutes.Tripdata_sno, ROUND(SUM(leakages.TotalLeaks), 2) AS TotalLeaks, ROUND(SUM(leakages.ReturnQty), 2) AS ReturnQty FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_ID = @BranchID) GROUP BY productsdata.ProductName,dispatch.DispName");
                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtSOLeaks = vdm.SelectQuery(cmd).Tables[0];
                        DataView viewLeaks = new DataView(dtSOLeaks);
                        DataTable distinctLeaks = viewLeaks.ToTable(true, "DispName");
                        foreach (DataRow drp in distinctLeaks.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            //newrow["Sno"] = i++;
                            newrow["Title"] = drp["DispName"].ToString();
                            foreach (DataRow dr in dtSOLeaks.Rows)
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {
                                    double ReturnQty = 0;
                                    double.TryParse(dr["ReturnQty"].ToString(), out ReturnQty);
                                    newrow[dr["ProductName"].ToString()] = ReturnQty;
                                }
                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Title"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
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
                        GrdProducts.DataSource = Report;
                        GrdProducts.DataBind();
                    }
                    #endregion "Varified Returns"

                    #region "DC Returns"
                    if (Type == "DC Returns")
                    {
                        cmd = new MySqlCommand("SELECT productsdata.ProductName,dispatch.DispName, productsdata.sno, dispatch.BranchID, triproutes.Tripdata_sno, ROUND(SUM(leakages.TotalLeaks), 2) AS TotalLeaks, ROUND(SUM(leakages.ReturnQty), 2) AS ReturnQty FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @BranchID) GROUP BY productsdata.ProductName,dispatch.DispName");
                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtSOLeaks = vdm.SelectQuery(cmd).Tables[0];
                        DataView viewLeaks = new DataView(dtSOLeaks);
                        DataTable distinctLeaks = viewLeaks.ToTable(true, "DispName");
                        foreach (DataRow drp in distinctLeaks.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            //newrow["Sno"] = i++;
                            newrow["Title"] = drp["DispName"].ToString();
                            foreach (DataRow dr in dtSOLeaks.Rows)
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {
                                    double ReturnQty = 0;
                                    double.TryParse(dr["ReturnQty"].ToString(), out ReturnQty);
                                    newrow[dr["ProductName"].ToString()] = ReturnQty;
                                }
                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Title"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
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
                        GrdProducts.DataSource = Report;
                        GrdProducts.DataBind();
                    }
                    #endregion "DC Returns"

                    #region "Difference"
                    if (Type == "Difference")
                    {
                        cmd = new MySqlCommand("SELECT productsdata.ProductName,dispatch.DispName, productsdata.sno, dispatch.BranchID, triproutes.Tripdata_sno, ROUND(SUM(leakages.TotalLeaks), 2) AS TotalLeaks, ROUND(SUM(leakages.ReturnQty), 2) AS ReturnQty FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @BranchID) GROUP BY productsdata.ProductName,dispatch.DispName order by productsdata.sno");
                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtReturnDcLeaks = vdm.SelectQuery(cmd).Tables[0];
                        cmd = new MySqlCommand("SELECT productsdata.ProductName, productsdata.sno,dispatch.DispName, dispatch.BranchID, triproutes.Tripdata_sno, ROUND(SUM(leakages.TotalLeaks), 2) AS TotalLeaks, ROUND(SUM(leakages.ReturnQty), 2) AS ReturnQty, ROUND(SUM(leakages.VLeaks), 2) AS VLeaks, ROUND(SUM(leakages.VReturns), 2) AS VReturns FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE  (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) GROUP BY productsdata.ProductName order by productsdata.sno");
                        //cmd = new MySqlCommand("SELECT dispatch.BranchID, productsdata.ProductName, ROUND(SUM(leakages.FreeMilk), 2) AS FreeMilk, ROUND(SUM(leakages.LeakQty), 2) AS LeakQty, ROUND(SUM(leakages.ShortQty), 2) AS ShortQty, productsdata.sno, dispatch.DispName FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) GROUP BY productsdata.ProductName, dispatch.DispName");
                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtVerifiedLeks = vdm.SelectQuery(cmd).Tables[0];
                        foreach (DataRow drSub in dtReturnDcLeaks.Rows)
                        {
                            foreach (DataRow drVerified in dtVerifiedLeks.Rows)
                            {

                                if (drSub["ProductName"].ToString() == drVerified["ProductName"].ToString())
                                {
                                    double Leaks = 0;
                                    double.TryParse(drSub["TotalLeaks"].ToString(), out Leaks);
                                    double VLeaks = 0;
                                    double.TryParse(drVerified["TotalLeaks"].ToString(), out VLeaks);
                                    if (Leaks == VLeaks)
                                    {
                                    }
                                    else
                                    {
                                        DataRow newrow = Report.NewRow();
                                        newrow["Title"] = "Products";
                                        double DiffLeaks = VLeaks - Leaks;
                                        newrow[drVerified["ProductName"].ToString()] = DiffLeaks;
                                        Report.Rows.Add(newrow);
                                    }
                                }
                            }
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Title"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
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
                        GrdProducts.DataSource = Report;
                        GrdProducts.DataBind();
                    }
                    #endregion "Difference"
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "PopupOpen();", true);
                }
            }
            if (Context.Session["buttontext"] == "Inventory")
            {
                cmd = new MySqlCommand("SELECT InvName, sno, Userdata_sno, flag, Qty FROM invmaster ORDER BY sno");
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                DataTable dttable = vdm.SelectQuery(cmd).Tables[0];
                if (dttable.Rows.Count > 0)
                {
                    DataView view = new DataView(dttable);
                    DataTable produtstbl = view.ToTable(true, "InvName", "sno");
                    Session["Report"] = produtstbl;
                    Report = new DataTable();
                    //Report.Columns.Add("Sno");
                    Report.Columns.Add("Title");
                    int count = 0;
                    foreach (DataRow dr in produtstbl.Rows)
                    {
                        if (dr["sno"].ToString() == "5" || dr["sno"].ToString() == "6" || dr["sno"].ToString() == "7" || dr["sno"].ToString() == "8")
                        {

                        }
                        else
                        {
                            Report.Columns.Add(dr["InvName"].ToString()).DataType = typeof(Double);
                        }
                    }
                    #region "Total Dispatch"
                    if (Type == "Dispatch")
                    {

                        cmd = new MySqlCommand("SELECT tripdata.I_Date, triproutes.RouteID, SUM(tripinvdata.Qty) AS invqty, invmaster.sno, invmaster.InvName, dispatch.Branch_Id, dispatch.DispName FROM triproutes INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripinvdata ON tripdata.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) GROUP BY invmaster.sno, dispatch.sno ORDER BY dispatch.sno");
                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtSOTripData = vdm.SelectQuery(cmd).Tables[0];
                        DataView viewSo = new DataView(dtSOTripData);
                        DataTable distincttableSo = viewSo.ToTable(true, "DispName");
                        foreach (DataRow drp in distincttableSo.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Title"] = drp["DispName"].ToString();
                            foreach (DataRow dr in dtSOTripData.Rows)
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {
                                    double DeliveryQty = 0;
                                    double.TryParse(dr["invqty"].ToString(), out DeliveryQty);
                                    newrow[dr["InvName"].ToString()] = DeliveryQty;
                                }
                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Title"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
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
                        GrdProducts.DataSource = Report;
                        GrdProducts.DataBind();

                    }
                    #endregion "Total Sale"

                    #region "Submitt Inventory"
                    if (Type == "Submitt Inventory")
                    {
                        cmd = new MySqlCommand("SELECT tripdata.I_Date, triproutes.RouteID, dispatch.Branch_Id, invmaster.InvName, SUM(invtran.Qty) AS submittedqty, SUM(invtran.VQty) AS verifiedqty, dispatch.DispName FROM triproutes INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty, CBFromTran, CBToTran, DeliveryTime, CollectionTime FROM invtransactions12 WHERE (ToTran = @BranchID)) invtran ON tripdata.Sno = invtran.FromTran INNER JOIN invmaster ON invtran.B_inv_sno = invmaster.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) GROUP BY invmaster.sno, dispatch.sno ORDER BY dispatch.sno");
                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtRouteLeaksReturns = vdm.SelectQuery(cmd).Tables[0];
                        DataView viewLeaksReturns = new DataView(dtRouteLeaksReturns);
                        DataTable distinctLeaksReturns = viewLeaksReturns.ToTable(true, "DispName");
                        foreach (DataRow drp in distinctLeaksReturns.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            //newrow["Sno"] = i++;
                            newrow["Title"] = drp["DispName"].ToString();
                            double total = 0;
                            foreach (DataRow dr in dtRouteLeaksReturns.Rows)
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {
                                    double submittedqty = 0;
                                    double.TryParse(dr["submittedqty"].ToString(), out submittedqty);
                                    newrow[dr["InvName"].ToString()] = submittedqty;
                                    total += submittedqty;
                                }
                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Title"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
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
                        GrdProducts.DataSource = Report;
                        GrdProducts.DataBind();

                    }
                    #endregion "Submitt Inventory"

                    #region "Varified Inventory"

                    if (Type == "Varified  Inventory")
                    {
                        cmd = new MySqlCommand("SELECT tripdata.I_Date, triproutes.RouteID, dispatch.Branch_Id, invmaster.InvName, SUM(invtran.Qty) AS submittedqty, SUM(invtran.VQty) AS verifiedqty, dispatch.DispName FROM triproutes INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty, CBFromTran, CBToTran, DeliveryTime, CollectionTime FROM invtransactions12 WHERE (ToTran = @BranchID)) invtran ON tripdata.Sno = invtran.FromTran INNER JOIN invmaster ON invtran.B_inv_sno = invmaster.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) GROUP BY invmaster.sno, dispatch.sno ORDER BY dispatch.sno");

                        cmd.Parameters.AddWithValue("@BranchID", BranchID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtRouteLeaksReturns = vdm.SelectQuery(cmd).Tables[0];
                        DataView viewLeaksReturns = new DataView(dtRouteLeaksReturns);
                        DataTable distinctLeaksReturns = viewLeaksReturns.ToTable(true, "DispName");
                        foreach (DataRow drp in distinctLeaksReturns.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            //newrow["Sno"] = i++;
                            newrow["Title"] = drp["DispName"].ToString();
                            double total = 0;
                            foreach (DataRow dr in dtRouteLeaksReturns.Rows)
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {
                                    double verifiedqty = 0;
                                    double.TryParse(dr["verifiedqty"].ToString(), out verifiedqty);
                                    newrow[dr["InvName"].ToString()] = verifiedqty;
                                    total += verifiedqty;
                                }
                            }
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["Title"] = "Total";
                        double val = 0.0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                                newvartical[dc.ToString()] = val;
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
                        GrdProducts.DataSource = Report;
                        GrdProducts.DataBind();
                    }


                    #endregion "Varified Inventory"
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "PopupOpen();", true);
                }
            }
            if (Context.Session["buttontext"] == "Collection")
            {

            }

        }
        else
        {
            //row.FindControl("pnlOrders").Visible = false;
            imgShowHide.CommandArgument = "Show";
            //imgShowHide.ImageUrl = "~/images/plus.png";
        }
    }
    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Check your condition here, Cells[1] for ex. is DONE/Not Done column
            if (e.Row.Cells[1].Text == "Opp Bal")
            {
                e.Row.BackColor = System.Drawing.Color.DarkSeaGreen;
            }
            if (e.Row.Cells[1].Text == "Inward")
            {
                e.Row.BackColor = System.Drawing.Color.DarkSeaGreen;
            }
            if (e.Row.Cells[1].Text == "Qty")
            {
                e.Row.BackColor = System.Drawing.Color.DarkSeaGreen;
            }
            if (e.Row.Cells[1].Text == "Sale")
            {
                e.Row.BackColor = System.Drawing.Color.Aqua;
            }
            if (e.Row.Cells[1].Text == "Free Milk")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
            }
            if (e.Row.Cells[1].Text == "Route Shorts")
            {
                e.Row.BackColor = System.Drawing.Color.SkyBlue;
            }
            if (e.Row.Cells[1].Text == "SO Shorts")
            {
                e.Row.BackColor = System.Drawing.Color.SkyBlue;
            }
            if (e.Row.Cells[1].Text == "Agent Leaks")
            {
                e.Row.BackColor = System.Drawing.Color.Gold;
            }
            if (e.Row.Cells[1].Text == "Submit Leaks")
            {
                e.Row.BackColor = System.Drawing.Color.Gold;
            }
            if (e.Row.Cells[1].Text == "Varified  Leaks")
            {
                e.Row.BackColor = System.Drawing.Color.Gold;
            }
            if (e.Row.Cells[1].Text == "SO Leaks")
            {
                e.Row.BackColor = System.Drawing.Color.Gold;
            }
            if (e.Row.Cells[1].Text == "Return DC Leaks")
            {
                e.Row.BackColor = System.Drawing.Color.Gold;
            }
            if (e.Row.Cells[1].Text == "Route Returns")
            {
                e.Row.BackColor = System.Drawing.Color.Lavender;
            }
            if (e.Row.Cells[1].Text == "Varified  Returns")
            {
                e.Row.BackColor = System.Drawing.Color.Lavender;
            }
            if (e.Row.Cells[1].Text == "DC Returns")
            {
                e.Row.BackColor = System.Drawing.Color.Lavender;
            }
            if (e.Row.Cells[1].Text == "Clo Bal")
            {
                e.Row.BackColor = System.Drawing.Color.DarkSalmon;
            }
            if (e.Row.Cells[1].Text == "Difference")
            {
                e.Row.BackColor = System.Drawing.Color.DarkSalmon;
            }
        }

    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string SalesOfficeId = ddlSalesOffice.SelectedValue;
            if (SalesOfficeId == "572")
            {
                SalesOfficeId = "7";
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
            cmd = new MySqlCommand("SELECT Sno, BranchId, Amount, ColAmount, EmpId, IndDate, SalesType, BranchType, BranchRouteID FROM  clotrans WHERE (BranchId = @BranchId) AND (IndDate BETWEEN @d1 AND @d2)");
            cmd.Parameters.AddWithValue("@BranchID", SalesOfficeId);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            DataTable dtCloTrans = vdm.SelectQuery(cmd).Tables[0];
            if (dtCloTrans.Rows.Count > 0)
            {
                lblmsg.Text = "Stock Closed";
            }
            else
            {
                cmd = new MySqlCommand("SELECT productsdata.ProductName, branchdata.SalesType, branchdata.sno,branchproducts.Product_sno, branchproducts.BranchQty, branchproducts.LeakQty FROM branchdata INNER JOIN branchproducts ON branchdata.sno = branchproducts.branch_sno INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno WHERE (branchdata.sno = @BranchID)  GROUP BY productsdata.ProductName order by branchproducts.Rank");
                cmd.Parameters.AddWithValue("@BranchID", SalesOfficeId);
                DataTable dtProReport = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT invmaster.sno, invmaster.InvName, inventory_monitor.Qty FROM inventory_monitor INNER JOIN invmaster ON inventory_monitor.Inv_Sno = invmaster.sno WHERE (inventory_monitor.BranchId = @BranchID)");
                cmd.Parameters.AddWithValue("@BranchID", SalesOfficeId);
                DataTable dtStockInvProduct = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("Insert into clotrans(BranchID,IndDate,SalesType,EmpID) Values(@BranchID,@IndDate,@SalesType,@EmpID)");
                cmd.Parameters.AddWithValue("@BranchID", SalesOfficeId);
                cmd.Parameters.AddWithValue("@IndDate", fromdate);
                cmd.Parameters.AddWithValue("@SalesType", 21);
                cmd.Parameters.AddWithValue("@EmpID", Session["UserSno"].ToString());
                long RefSno = vdm.insertScalar(cmd);
                foreach (DataRow drv in dtProReport.Rows)
                {
                    cmd = new MySqlCommand("Insert into closubtranprodcts (RefNo,ProductID,StockQty,LeakQty) values(@RefNo,@ProductID,@StockQty,@LeakQty)");
                    cmd.Parameters.AddWithValue("@RefNo", RefSno);
                    cmd.Parameters.AddWithValue("@ProductID", drv["Product_sno"].ToString());
                    float BranchQty = 0;
                    float LeakQty = 0;
                    float.TryParse(drv["BranchQty"].ToString(), out BranchQty);
                    float.TryParse(drv["LeakQty"].ToString(), out LeakQty);
                    cmd.Parameters.AddWithValue("@StockQty", BranchQty);
                    cmd.Parameters.AddWithValue("@LeakQty", LeakQty);
                    if (BranchQty != 0 || LeakQty != 0)
                    {
                        vdm.insert(cmd);
                    }
                }
                foreach (DataRow drv in dtStockInvProduct.Rows)
                {
                    cmd = new MySqlCommand("Insert into closubtraninventory (RefNo,InvSno,StockQty) values(@RefNo,@InvSno,@StockQty)");
                    cmd.Parameters.AddWithValue("@RefNo", RefSno);
                    int Qty = 0;
                    int.TryParse(drv["Qty"].ToString(), out Qty);
                    cmd.Parameters.AddWithValue("@InvSno", drv["sno"].ToString());
                    cmd.Parameters.AddWithValue("@StockQty", Qty);
                    if (Qty != 0)
                    {
                        vdm.insert(cmd);
                    }
                }
                lblmsg.Text = "Saved successfully";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
        }
    }
}
