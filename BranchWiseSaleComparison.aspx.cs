using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;


public partial class BranchWiseSaleComparison : System.Web.UI.Page
{
    MySqlCommand cmd;
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        //UserName = Session["field1"].ToString();
        //vdm = new VehicleDBMgr();
        if (Session["salestype"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                //FillRouteName();
                //txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                //lblTitle.Text = Session["TitleName"].ToString();

            }
        }
    }
    //void FillRouteName()
    //{
    //    try
    //    {
    //        vdm = new VehicleDBMgr();
    //        if (Session["salestype"].ToString() == "Plant")
    //        {
    //            PBranch.Visible = true;
    //            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType)");
    //            cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
    //            cmd.Parameters.AddWithValue("@SalesType", "21");
    //            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
    //            //if (ddlSalesOffice.SelectedIndex == -1)
    //            //{
    //            //    ddlSalesOffice.SelectedItem.Text = "Select";
    //            //}
    //            ddlSalesOffice.DataSource = dtRoutedata;
    //            ddlSalesOffice.DataTextField = "BranchName";
    //            ddlSalesOffice.DataValueField = "sno";
    //            ddlSalesOffice.DataBind();
    //            ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));
    //        }
    //        else
    //        {
    //            PBranch.Visible = false;
    //            //cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
    //            ////cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
    //            //cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
    //            //cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
    //            //DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
    //            //ddlRouteName.DataSource = dtRoutedata;
    //            //ddlRouteName.DataTextField = "DispName";
    //            //ddlRouteName.DataValueField = "sno";
    //            //ddlRouteName.DataBind();

    //            cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID)  and (DispType is NULL) AND (dispatch.flag=@flag)) OR ((branchdata_1.SalesOfficeID = @SOID)  and (DispType is NULL) AND (dispatch.flag=@flag))");
    //            //cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
    //            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
    //            cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
    //            cmd.Parameters.AddWithValue("@flag", "1");
    //            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
    //            ddlRouteName.DataSource = dtRoutedata;
    //            ddlRouteName.DataTextField = "DispName";
    //            ddlRouteName.DataValueField = "sno";
    //            ddlRouteName.DataBind();
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    //protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    vdm = new VehicleDBMgr();
    //    cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID) AND (dispatch.flag=@flag)) OR ( (dispatch.flag=@flag) AND (branchdata_1.SalesOfficeID = @SOID))");
    //    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
    //    cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
    //    cmd.Parameters.AddWithValue("@flag", "1");
    //    DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
    //    ddlRouteName.DataSource = dtRoutedata;
    //    ddlRouteName.DataTextField = "DispName";
    //    ddlRouteName.DataValueField = "sno";
    //    ddlRouteName.DataBind();
    //}
    //private DateTime GetLowDate(DateTime dt)
    //{
    //    double Hour, Min, Sec;
    //    DateTime DT = DateTime.Now;
    //    DT = dt;
    //    Hour = -dt.Hour;
    //    Min = -dt.Minute;
    //    Sec = -dt.Second;
    //    DT = DT.AddHours(Hour);
    //    DT = DT.AddMinutes(Min);
    //    DT = DT.AddSeconds(Sec);
    //    return DT;

    //}

    //private DateTime GetHighDate(DateTime dt)
    //{
    //    double Hour, Min, Sec;
    //    DateTime DT = DateTime.Now;
    //    Hour = 23 - dt.Hour;
    //    Min = 59 - dt.Minute;
    //    Sec = 59 - dt.Second;
    //    DT = dt;
    //    DT = DT.AddHours(Hour);
    //    DT = DT.AddMinutes(Min);
    //    DT = DT.AddSeconds(Sec);
    //    return DT;
    //}
    //protected void btnGenerate_Click(object sender, EventArgs e)
    //{
    //    string TitleName = Session["TitleName"].ToString();
    //    GetReport();
    //}
    //DataTable Report = new DataTable();
    //void GetReport()
    //{
    //    try
    //    {
    //        lblmsg.Text = "";
    //        pnlHide.Visible = true;
    //        float denominationtotal = 0;
    //        Session["filename"] = ddlRouteName.SelectedItem.Text + "Delivery" + DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
    //        lblRoute.Text = ddlRouteName.SelectedItem.Text;
    //        lblDate.Text = txtdate.Text;
    //        vdm = new VehicleDBMgr();
    //        Report = new DataTable();
    //        DateTime fromdate = DateTime.Now;
    //        string[] datestrig = txtdate.Text.Split(' ');
    //        if (datestrig.Length > 1)
    //        {
    //            if (datestrig[0].Split('-').Length > 0)
    //            {
    //                string[] dates = datestrig[0].Split('-');
    //                string[] times = datestrig[1].Split(':');
    //                fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
    //            }
    //        }
    //        DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
    //        lblDate.Text = fromdate.ToString("dd/MMM/yyyy");
    //        cmd = new MySqlCommand("SELECT  ff.TripID, Triproutes.RouteID, ff.DispQty, ff.sno FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (RouteID = @dispatchSno)) Triproutes INNER JOIN (SELECT TripID, DispQty, sno FROM  (SELECT tripdata.Sno AS TripID, tripsubdata.Qty AS DispQty, tripsubdata.ProductId AS sno FROM  tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno WHERE (tripdata.I_Date BETWEEN @starttime AND @endtime)) tripinfo) ff ON ff.TripID = Triproutes.Tripdata_sno");
    //        cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
    //        cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
    //        cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
    //        DataTable dtSub_yesterdayData = vdm.SelectQuery(cmd).Tables[0];

    //        cmd = new MySqlCommand("SELECT  ff.TripID, Triproutes.RouteID, ff.DispQty, ff.sno FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (RouteID = @dispatchSno)) Triproutes INNER JOIN (SELECT TripID, DispQty, sno FROM  (SELECT tripdata.Sno AS TripID, tripsubdata.Qty AS DispQty, tripsubdata.ProductId AS sno FROM  tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno WHERE (tripdata.I_Date BETWEEN @starttime AND @endtime)) tripinfo) ff ON ff.TripID = Triproutes.Tripdata_sno");
    //        cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-8)));
    //        cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-8)));
    //        cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
    //        DataTable dtSub_LastWeekData = vdm.SelectQuery(cmd).Tables[0];

    //        cmd = new MySqlCommand("SELECT  ff.TripID, Triproutes.RouteID, ff.DispQty, ff.sno FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (RouteID = @dispatchSno)) Triproutes INNER JOIN (SELECT TripID, DispQty, sno FROM  (SELECT tripdata.Sno AS TripID, tripsubdata.Qty AS DispQty, tripsubdata.ProductId AS sno FROM  tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno WHERE (tripdata.I_Date BETWEEN @starttime AND @endtime)) tripinfo) ff ON ff.TripID = Triproutes.Tripdata_sno");
    //        cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-31)));
    //        cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-31)));
    //        cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
    //        DataTable dtSub_lastMonthData = vdm.SelectQuery(cmd).Tables[0];



    //        cmd = new MySqlCommand("SELECT  ff.TripID, Triproutes.RouteID, ff.DispQty, ff.sno FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (RouteID = @dispatchSno)) Triproutes INNER JOIN (SELECT TripID, DispQty, sno FROM  (SELECT tripdata.Sno AS TripID, tripsubdata.Qty AS DispQty, tripsubdata.ProductId AS sno FROM  tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno WHERE (tripdata.I_Date BETWEEN @starttime AND @endtime)) tripinfo) ff ON ff.TripID = Triproutes.Tripdata_sno");
    //        cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-365)));
    //        cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-365)));
    //        cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
    //        DataTable dtSub_lastYearData = vdm.SelectQuery(cmd).Tables[0];

    //        cmd = new MySqlCommand("SELECT products_category.Categoryname,productsdata.Sno, productsdata.ProductName, products_subcategory.SubCatName  FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) Group by productsdata.ProductName ORDER BY productsdata.Rank");
    //        if (Session["salestype"].ToString() == "Plant")
    //        {
    //            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
    //        }
    //        else
    //        {
    //            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
    //        }
    //        DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
    //        DataTable dtallprdts = new DataTable();
    //        dtallprdts.Columns.Add("Product_sno");
    //        dtallprdts.Columns.Add("ProductName");
    //        dtallprdts.Columns.Add("unitQty");
    //        dtallprdts.Columns.Add("DeliveryQty");
    //        dtallprdts.Columns.Add("LeakQty");
    //        dtallprdts.Columns.Add("Total");
    //        foreach (DataRow dr in produtstbl.Rows)
    //        {
    //            float unitqty = 0;
    //            float deliveryqty = 0;
    //            float leakqty = 0;
    //            float Total = 0;
    //            DataRow newRow = dtallprdts.NewRow();
    //            newRow["Product_sno"] = dr["Sno"].ToString();
    //            newRow["ProductName"] = dr["ProductName"].ToString();
    //            newRow["unitQty"] = unitqty;
    //            newRow["DeliveryQty"] = deliveryqty;
    //            newRow["LeakQty"] = leakqty;
    //            newRow["Total"] = Total;
    //            dtallprdts.Rows.Add(newRow);
    //        }
    //        Report = new DataTable();
    //        Report.Columns.Add("Product Name");
    //        Report.Columns.Add("Yester Day").DataType = typeof(Double);
    //        Report.Columns.Add("Last Week").DataType = typeof(Double);
    //        Report.Columns.Add("Last Month").DataType = typeof(Double);
    //        Report.Columns.Add("Last Year").DataType = typeof(Double);
    //        DataRow newrow = Report.NewRow();
    //        foreach (DataRow branch in dtallprdts.Rows)
    //        {
    //            DataRow newrow1 = Report.NewRow();
    //            newrow1["Product Name"] = branch["ProductName"].ToString();
    //            float DispQty = 0;
    //            foreach (DataRow drSubData in dtSub_yesterdayData.Rows)
    //            {
    //                if (branch["Product_sno"].ToString() == drSubData["sno"].ToString())
    //                {
    //                    float.TryParse(drSubData["DispQty"].ToString(), out DispQty);
    //                    if (DispQty > 0)
    //                    {
    //                        newrow1["Yester Day"] = drSubData["DispQty"].ToString();
    //                    }
    //                }
    //            }
    //            foreach (DataRow drSubData in dtSub_LastWeekData.Rows)
    //            {
    //                if (branch["Product_sno"].ToString() == drSubData["sno"].ToString())
    //                {
    //                    float.TryParse(drSubData["DispQty"].ToString(), out DispQty);
    //                    if (DispQty > 0)
    //                    {
    //                        newrow1["Last Week"] = drSubData["DispQty"].ToString();
    //                    }
    //                }
    //            }
    //            foreach (DataRow drSubData in dtSub_lastMonthData.Rows)
    //            {
    //                if (branch["Product_sno"].ToString() == drSubData["sno"].ToString())
    //                {
    //                    float.TryParse(drSubData["DispQty"].ToString(), out DispQty);
    //                    if (DispQty > 0)
    //                    {
    //                        newrow1["Last Month"] = drSubData["DispQty"].ToString();
    //                    }
    //                }
    //            }
    //            foreach (DataRow drSubData in dtSub_lastYearData.Rows)
    //            {
    //                if (branch["Product_sno"].ToString() == drSubData["sno"].ToString())
    //                {
    //                    float.TryParse(drSubData["DispQty"].ToString(), out DispQty);
    //                    if (DispQty > 0)
    //                    {
    //                        newrow1["Last Year"] = drSubData["DispQty"].ToString();
    //                    }
    //                }
    //            }
    //            if (DispQty > 0)
    //            {
    //                Report.Rows.Add(newrow1);
    //            }
    //        }
    //        DataRow newvartical = Report.NewRow();
    //        newvartical["Product Name"] = "Total";
    //        double val = 0.0;
    //        foreach (DataColumn dc in Report.Columns)
    //        {
    //            if (dc.DataType == typeof(Double))
    //            {
    //                val = 0.0;
    //                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
    //                newvartical[dc.ToString()] = val;
    //            }
    //        }
    //        Report.Rows.Add(newvartical);

    //        grdReports.DataSource = Report;
    //        grdReports.DataBind();

    //        Session["xportdata"] = Report;
    //    }
    //    catch
    //    {
    //    }
    //}
}