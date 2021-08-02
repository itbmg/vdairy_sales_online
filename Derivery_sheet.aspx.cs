using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class Derivery_sheet : System.Web.UI.Page
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
                FillRouteName();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();

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
                PBranch.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType)");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                cmd.Parameters.AddWithValue("@SalesType", "21");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                //if (ddlSalesOffice.SelectedIndex == -1)
                //{
                //    ddlSalesOffice.SelectedItem.Text = "Select";
                //}
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                PBranch.Visible = false;
                //cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
                ////cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
                //cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                //cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
                //DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                //ddlRouteName.DataSource = dtRoutedata;
                //ddlRouteName.DataTextField = "DispName";
                //ddlRouteName.DataValueField = "sno";
                //ddlRouteName.DataBind();

                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID)  and (DispType is NULL) AND (dispatch.flag=@flag)) OR ((branchdata_1.SalesOfficeID = @SOID)  and (DispType is NULL) AND (dispatch.flag=@flag))");
                //cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@flag", "1");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlRouteName.DataSource = dtRoutedata;
                ddlRouteName.DataTextField = "DispName";
                ddlRouteName.DataValueField = "sno";
                ddlRouteName.DataBind();
            }
        }
        catch
        {
        }
    }
    protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID) AND (dispatch.flag=@flag)) OR ( (dispatch.flag=@flag) AND (branchdata_1.SalesOfficeID = @SOID))");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@flag", "1");
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlRouteName.DataSource = dtRoutedata;
        ddlRouteName.DataTextField = "DispName";
        ddlRouteName.DataValueField = "sno";
        ddlRouteName.DataBind();
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
        string TitleName = Session["TitleName"].ToString();
        GetReport();
    }
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            float denominationtotal = 0;
            Session["filename"] = ddlRouteName.SelectedItem.Text + "Delivery" + DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            lblRoute.Text = ddlRouteName.SelectedItem.Text;
            lblDate.Text = txtdate.Text;
            vdm = new VehicleDBMgr();
            Report = new DataTable();
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
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            lblDate.Text = fromdate.ToString("dd/MMM/yyyy");
            cmd = new MySqlCommand("SELECT  ff.TripID, Triproutes.RouteID, ff.DispQty, ff.sno FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (RouteID = @dispatchSno)) Triproutes INNER JOIN (SELECT TripID, DispQty, sno FROM  (SELECT tripdata.Sno AS TripID, tripsubdata.Qty AS DispQty, tripsubdata.ProductId AS sno FROM  tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno WHERE (tripdata.I_Date BETWEEN @starttime AND @endtime)) tripinfo) ff ON ff.TripID = Triproutes.Tripdata_sno");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            DataTable dtSub_yesterdayData = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT  ff.TripID, Triproutes.RouteID, ff.DispQty, ff.sno FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (RouteID = @dispatchSno)) Triproutes INNER JOIN (SELECT TripID, DispQty, sno FROM  (SELECT tripdata.Sno AS TripID, tripsubdata.Qty AS DispQty, tripsubdata.ProductId AS sno FROM  tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno WHERE (tripdata.I_Date BETWEEN @starttime AND @endtime)) tripinfo) ff ON ff.TripID = Triproutes.Tripdata_sno");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-8)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-8)));
            cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            DataTable dtSub_LastWeekData = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT  ff.TripID, Triproutes.RouteID, ff.DispQty, ff.sno FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (RouteID = @dispatchSno)) Triproutes INNER JOIN (SELECT TripID, DispQty, sno FROM  (SELECT tripdata.Sno AS TripID, tripsubdata.Qty AS DispQty, tripsubdata.ProductId AS sno FROM  tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno WHERE (tripdata.I_Date BETWEEN @starttime AND @endtime)) tripinfo) ff ON ff.TripID = Triproutes.Tripdata_sno");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-31)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-31)));
            cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            DataTable dtSub_lastMonthData = vdm.SelectQuery(cmd).Tables[0];

           

            cmd = new MySqlCommand("SELECT  ff.TripID, Triproutes.RouteID, ff.DispQty, ff.sno FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (RouteID = @dispatchSno)) Triproutes INNER JOIN (SELECT TripID, DispQty, sno FROM  (SELECT tripdata.Sno AS TripID, tripsubdata.Qty AS DispQty, tripsubdata.ProductId AS sno FROM  tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno WHERE (tripdata.I_Date BETWEEN @starttime AND @endtime)) tripinfo) ff ON ff.TripID = Triproutes.Tripdata_sno");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-365)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-365)));
            cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            DataTable dtSub_lastYearData = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT products_category.Categoryname,productsdata.Sno, productsdata.ProductName, products_subcategory.SubCatName  FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) Group by productsdata.ProductName ORDER BY productsdata.Rank");
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            }
            DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
            DataTable dtallprdts = new DataTable();
            dtallprdts.Columns.Add("Product_sno");
            dtallprdts.Columns.Add("ProductName");
            dtallprdts.Columns.Add("unitQty");
            dtallprdts.Columns.Add("DeliveryQty");
            dtallprdts.Columns.Add("LeakQty");
            dtallprdts.Columns.Add("Total");
            foreach (DataRow dr in produtstbl.Rows)
            {
                float unitqty = 0;
                float deliveryqty = 0;
                float leakqty = 0;
                float Total = 0;
                DataRow newRow = dtallprdts.NewRow();
                newRow["Product_sno"] = dr["Sno"].ToString();
                newRow["ProductName"] = dr["ProductName"].ToString();
                newRow["unitQty"] = unitqty;
                newRow["DeliveryQty"] = deliveryqty;
                newRow["LeakQty"] = leakqty;
                newRow["Total"] = Total;
                dtallprdts.Rows.Add(newRow);
            }
            Report = new DataTable();
            Report.Columns.Add("Product Name");
            Report.Columns.Add("Yester Day").DataType = typeof(Double);
            Report.Columns.Add("Last Week").DataType = typeof(Double);
            Report.Columns.Add("Last Month").DataType = typeof(Double);
            //Report.Columns.Add("Last Three Month").DataType = typeof(Double);
            //Report.Columns.Add("Last Six Month").DataType = typeof(Double);
            Report.Columns.Add("Last Year").DataType = typeof(Double);
            DataRow newrow = Report.NewRow();
            foreach (DataRow branch in dtallprdts.Rows)
            {
                DataRow newrow1 = Report.NewRow();
                newrow1["Product Name"] = branch["ProductName"].ToString();
                float DispQty = 0;
                foreach (DataRow drSubData in dtSub_yesterdayData.Rows)
                {
                    if (branch["Product_sno"].ToString() == drSubData["sno"].ToString())
                    {
                        float.TryParse(drSubData["DispQty"].ToString(), out DispQty);
                        if (DispQty > 0)
                        {
                            newrow1["Yester Day"] = drSubData["DispQty"].ToString();
                        }
                    }
                }
                foreach (DataRow drSubData in dtSub_LastWeekData.Rows)
                {
                    if (branch["Product_sno"].ToString() == drSubData["sno"].ToString())
                    {
                        float.TryParse(drSubData["DispQty"].ToString(), out DispQty);
                        if (DispQty > 0)
                        {
                            newrow1["Last Week"] = drSubData["DispQty"].ToString();
                        }
                    }
                }
                foreach (DataRow drSubData in dtSub_lastMonthData.Rows)
                {
                    if (branch["Product_sno"].ToString() == drSubData["sno"].ToString())
                    {
                        float.TryParse(drSubData["DispQty"].ToString(), out DispQty);
                        if (DispQty > 0)
                        {
                            newrow1["Last Month"] = drSubData["DispQty"].ToString();
                        }
                    }
                }
                foreach (DataRow drSubData in dtSub_lastYearData.Rows)
                {
                    if (branch["Product_sno"].ToString() == drSubData["sno"].ToString())
                    {
                        float.TryParse(drSubData["DispQty"].ToString(), out DispQty);
                        if (DispQty > 0)
                        {
                            newrow1["Last Year"] = drSubData["DispQty"].ToString();
                        }
                    }
                }
                if (DispQty > 0)
                {
                    Report.Rows.Add(newrow1);
                }
            }
            DataRow newvartical = Report.NewRow();
            newvartical["Product Name"] = "Total";
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
            cmd = new MySqlCommand("SELECT branchdata.BranchName,branchdata.SalesType, branchaccounts.BranchId, branchaccounts.Amount FROM dispatch INNER JOIN modifiedroutes ON dispatch.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN branchaccounts ON branchdata.sno = branchaccounts.BranchId WHERE (dispatch.sno = @dispsno) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @dt) AND (branchdata.flag=1) OR (dispatch.sno = @dispsno) AND (modifiedroutesubtable.EDate > @dt) AND (modifiedroutesubtable.CDate <= @dt) and (branchdata.flag=1) order by branchdata.BranchName");
            cmd.Parameters.AddWithValue("@dispsno", ddlRouteName.SelectedValue);
            cmd.Parameters.AddWithValue("@dt", GetLowDate(fromdate.AddDays(-1)));
            DataTable dtbranchammount = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT  inventory_monitor.Inv_Sno, inventory_monitor.Qty,modifiedroutesubtable.BranchID FROM dispatch INNER JOIN modifiedroutes ON dispatch.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN inventory_monitor ON modifiedroutesubtable.BranchID = inventory_monitor.BranchId WHERE (dispatch.sno = @dispsno) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @dt) OR (dispatch.sno = @dispsno) AND (modifiedroutesubtable.EDate > @dt) AND (modifiedroutesubtable.CDate <= @dt) ");
            cmd.Parameters.AddWithValue("@dispsno", ddlRouteName.SelectedValue);
            cmd.Parameters.AddWithValue("@dt", GetLowDate(fromdate.AddDays(-1)));
            DataTable dtAgentInventory = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT MIN(Ind.D_date) AS midate, MAX(collect.PaidDate) AS madate FROM dispatch INNER JOIN modifiedroutesubtable ON dispatch.Route_id = modifiedroutesubtable.RefNo INNER JOIN (SELECT indents.Branch_id, MIN(indents_subtable.D_date) AS D_date, MAX(indents_subtable.DTripId) AS DTripId FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo WHERE (indents.I_date BETWEEN @starttime AND @endtime) AND (indents_subtable.D_date IS NOT NULL) GROUP BY indents.Branch_id) Ind ON modifiedroutesubtable.BranchID = Ind.Branch_id INNER JOIN (SELECT PaidDate, tripId FROM collections) collect ON Ind.DTripId = collect.tripId WHERE (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime)");
            cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(ServerDateCurrentdate));
            DataTable dtminmax = vdm.SelectQuery(cmd).Tables[0];
            string mindate = dtminmax.Rows[0]["midate"].ToString();
            string maxdate = dtminmax.Rows[0]["madate"].ToString();
            DateTime midate = DateTime.Parse(mindate);
            DateTime madate = DateTime.Parse(maxdate);
            cmd = new MySqlCommand("SELECT branchdata.BranchName, indents_subtable.DTripId, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS totalamount, indents_subtable.D_date, indent.Branch_id, modifiedroutes.Sno FROM dispatch INNER JOIN modifiedroutes ON dispatch.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutesubtable.RefNo = modifiedroutes.Sno INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, I_date, Branch_id FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo WHERE (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) AND (dispatch.sno = @dispatchSno) OR (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (dispatch.sno = @dispatchSno) GROUP BY branchdata.BranchName, modifiedroutes.Sno");
            cmd.Parameters.AddWithValue("@dispatchSno", ddlPlantDispName.SelectedValue);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@Paidstime", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@Paidetime", GetHighDate(fromdate));
            DataTable dttodaycollection = vdm.SelectQuery(cmd).Tables[0];
            if (dttodaycollection.Rows.Count > 0)
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, collections.Branchid, branchdata.sno, collections.AmountPaid, collections.PaidDate, collections.PaymentType, collections.CheckStatus, collections.PayTime, collections.ChequeNo, collections.tripId, collections.ReceiptNo FROM dispatch INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN  branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN collections ON branchdata.sno = collections.Branchid WHERE (dispatch.sno = @dispatchsno) AND (collections.AmountPaid <> 0) AND (collections.tripId = @TripID)");
                cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
                if (dttodaycollection.Rows[0]["DTripId"].ToString() == "")
                {
                    cmd.Parameters.AddWithValue("@TripID", dttodaycollection.Rows[1]["DTripId"].ToString());
                }
                else
                {
                    if (dttodaycollection.Rows[0]["D_date"].ToString() == "")
                    {
                        cmd.Parameters.AddWithValue("@TripID", dttodaycollection.Rows[1]["DTripId"].ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@TripID", dttodaycollection.Rows[0]["DTripId"].ToString());

                    }
                }
            }
            DataTable dtroutecollection = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT  result.Branch_id, result.BranchName, result.totalamount, result.D_date, result.DTripId, result.DelTime, SUM(collections.AmountPaid) AS AmountPaid,collections.PayTime FROM (SELECT branchdata.sno AS Branch_id, branchdata.BranchName, SUM(indentssub.DeliveryQty * indentssub.UnitCost) AS totalamount, indentssub.D_date,indentssub.DTripId, indentssub.DelTime FROM (SELECT IndentNo, Product_sno, DeliveryQty, D_date, UnitCost, DTripId, DelTime FROM indents_subtable WHERE (D_date BETWEEN @starttime AND @endtime)) indentssub INNER JOIN (SELECT IndentNo, Branch_id, TotalQty, TotalPrice, I_date, D_date, Status, UserData_sno, PaymentStatus, I_createdby, I_modifiedby,IndentType FROM indents WHERE (I_date BETWEEN @Indd1 AND @indd2)) ind ON indentssub.IndentNo = ind.IndentNo RIGHT OUTER JOIN dispatch INNER JOIN modifiedroutes ON dispatch.Route_id = modifiedroutes.Sno INNER JOIN (SELECT RefNo, BranchID, CDate, EDate FROM  modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifroutessub ON modifiedroutes.Sno = modifroutessub.RefNo INNER JOIN branchdata ON modifroutessub.BranchID = branchdata.sno ON ind.Branch_id = branchdata.sno WHERE (dispatch.sno = @dispatchSno) GROUP BY branchdata.sno) result INNER JOIN collections ON result.Branch_id = collections.Branchid WHERE (collections.PaidDate BETWEEN @starttime AND @endtime) AND (collections.tripId <> 'NULL') GROUP BY result.Branch_id");
            cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            cmd.Parameters.AddWithValue("@starttime", midate);
            cmd.Parameters.AddWithValue("@endtime", madate);
            cmd.Parameters.AddWithValue("@Paidstime", midate);
            cmd.Parameters.AddWithValue("@Paidetime", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@Indd1", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@Indd2", GetHighDate(madate));
            DataTable dtBranchcollection = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT  result.Branch_id, result.BranchName, result.totalamount, result.D_date, result.DTripId, result.DelTime, SUM(collections.AmountPaid) AS AmountPaid,collections.PayTime FROM (SELECT branchdata.sno AS Branch_id, branchdata.BranchName, SUM(indentssub.DeliveryQty * indentssub.UnitCost) AS totalamount, indentssub.D_date,indentssub.DTripId, indentssub.DelTime FROM (SELECT IndentNo, Product_sno, DeliveryQty, D_date, UnitCost, DTripId, DelTime FROM indents_subtable WHERE (D_date BETWEEN @starttime AND @endtime)) indentssub INNER JOIN (SELECT IndentNo, Branch_id, TotalQty, TotalPrice, I_date, D_date, Status, UserData_sno, PaymentStatus, I_createdby, I_modifiedby,IndentType FROM indents WHERE (I_date BETWEEN @Indd1 AND @indd2)) ind ON indentssub.IndentNo = ind.IndentNo RIGHT OUTER JOIN dispatch INNER JOIN modifiedroutes ON dispatch.Route_id = modifiedroutes.Sno INNER JOIN (SELECT RefNo, BranchID, CDate, EDate FROM  modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifroutessub ON modifiedroutes.Sno = modifroutessub.RefNo INNER JOIN branchdata ON modifroutessub.BranchID = branchdata.sno ON ind.Branch_id = branchdata.sno WHERE (dispatch.sno = @dispatchSno) GROUP BY branchdata.sno) result INNER JOIN collections ON result.Branch_id = collections.Branchid WHERE (collections.PaidDate BETWEEN @starttime AND @endtime) AND (collections.tripId IS NULL) GROUP BY result.Branch_id");
            cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            cmd.Parameters.AddWithValue("@starttime", midate);
            cmd.Parameters.AddWithValue("@endtime", madate);
            cmd.Parameters.AddWithValue("@Paidstime", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@Paidetime", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@Indd1", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@Indd2", GetHighDate(madate));
            DataTable dtsalesofficecollection = vdm.SelectQuery(cmd).Tables[0];

            DataTable Rpt = new DataTable();
            Rpt.Columns.Add("SNo");
            Rpt.Columns.Add("Agent Code");
            Rpt.Columns.Add("Agent Name");
            Rpt.Columns.Add("Crates");
            Rpt.Columns.Add("Oppening Balance");
            Rpt.Columns.Add("Sale Value");
            Rpt.Columns.Add("Paid Amount");
            Rpt.Columns.Add("Due Amount");
            DataTable dtreport = new DataTable();
            dtreport.Columns.Add("Agent Code");
            dtreport.Columns.Add("Agent Name");
            dtreport.Columns.Add("Crates");
            dtreport.Columns.Add("Oppening Balance");
            dtreport.Columns.Add("Sale Value");
            dtreport.Columns.Add("Paid Amount");
            dtreport.Columns.Add("Due Amount");
            dtreport.Columns.Add("today sale");
            dtreport.Columns.Add("today collected");
            dtreport.Columns.Add("SalesType");
            foreach (DataRow dr in dtbranchammount.Rows)
            {
                DataRow newRow = dtreport.NewRow();
                newRow["Agent Code"] = dr["BranchId"].ToString();
                newRow["Agent Name"] = dr["BranchName"].ToString();
                newRow["Sale Value"] = "0";
                newRow["Oppening Balance"] = dr["Amount"].ToString();
                newRow["Paid Amount"] = "0";
                newRow["Due Amount"] = "0";
                newRow["today sale"] = "0";
                newRow["today collected"] = "0";
                newRow["SalesType"] = dr["SalesType"].ToString();
                dtreport.Rows.Add(newRow);
            }
            foreach (DataRow drbrncnamtcoll in dtreport.Rows)
            {
                foreach (DataRow dr in dttodaycollection.Rows)
                {
                    if (drbrncnamtcoll["Agent Code"].ToString() == dr["Branch_id"].ToString())
                    {
                        float salevalue = 0;
                        float.TryParse(dr["totalamount"].ToString(), out salevalue);
                        drbrncnamtcoll["today sale"] = Math.Round(salevalue, 2);
                    }
                }
            }
            foreach (DataRow drbrncnamtcoll in dtreport.Rows)
            {
                foreach (DataRow dr in dtroutecollection.Rows)
                {
                    if (drbrncnamtcoll["Agent Code"].ToString() == dr["Branchid"].ToString())
                    {
                        float paidamt = 0;
                        float.TryParse(dr["AmountPaid"].ToString(), out paidamt);
                        drbrncnamtcoll["today collected"] = Math.Round(paidamt, 2);
                    }
                }
            }
            foreach (DataRow drbrnchamt in dtreport.Rows)
            {
                foreach (DataRow drbrnchcollection in dtBranchcollection.Rows)
                {
                    if (drbrnchamt["Agent Code"].ToString() == drbrnchcollection["Branch_id"].ToString())
                    {

                        float salevalue = 0;
                        float.TryParse(drbrnchcollection["totalamount"].ToString(), out salevalue);
                        float paidamt = 0;
                        float.TryParse(drbrnchcollection["AmountPaid"].ToString(), out paidamt);
                        float dueamt = 0;
                        drbrnchamt["Sale Value"] = Math.Round(salevalue, 2);
                        drbrnchamt["Paid Amount"] = Math.Round(paidamt, 2);
                        drbrnchamt["Due Amount"] = Math.Round(dueamt, 2);
                    }
                }
            }
            foreach (DataRow drbrnchamt in dtreport.Rows)
            {
                foreach (DataRow drsocollection in dtsalesofficecollection.Rows)
                {
                    if (drbrnchamt["Agent Code"].ToString() == drsocollection["Branch_id"].ToString())
                    {

                        float sopaidamt = 0;
                        float.TryParse(drsocollection["AmountPaid"].ToString(), out sopaidamt);
                        float trippaid = 0;
                        float.TryParse(drbrnchamt["Paid Amount"].ToString(), out trippaid);

                        float amtpaid = sopaidamt + trippaid;
                        drbrnchamt["Paid Amount"] = Math.Round(amtpaid, 2);
                    }
                }
            }
            int k = 1;
            cmd = new MySqlCommand("SELECT sno,salestype FROM salestypemanagement WHERE (status = 1) ORDER BY salestype DESC");
            DataTable dtsalesType = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in dtsalesType.Rows)
            {
                DataRow[] drsalestype = dtreport.Select("SalesType='" + dr["sno"].ToString() + "'");
                if (drsalestype.Length > 0)
                {
                    DataRow newRow41 = Rpt.NewRow();
                    if (dr["sno"].ToString() == "20")
                    {
                        newRow41["Agent Code"] = dr["salestype"].ToString();
                        Rpt.Rows.Add(newRow41);
                    }
                }
                if (dr["sno"].ToString() == "20")
                {
                    foreach (DataRow drbrnchamt in dtreport.Select("SalesType='" + dr["sno"].ToString() + "'"))
                    {
                        DataRow newRow1 = Rpt.NewRow();
                        newRow1["SNo"] = k;
                        newRow1["Agent Code"] = drbrnchamt["Agent Code"].ToString();
                        DataRow[] dragentinv = dtAgentInventory.Select("BranchID='" + drbrnchamt["Agent Code"].ToString() + "'");
                        string creates = "";
                        foreach (DataRow drc in dragentinv)
                        {
                            string invsno = drc.ItemArray[0].ToString();
                            if (invsno == "1" || invsno == "2" || invsno == "3" || invsno == "4")
                            {
                                if (drc.ItemArray[1].ToString() == "0")
                                {
                                }
                                else
                                {
                                    creates += drc.ItemArray[1].ToString() + "/";
                                    newRow1["Crates"] = creates;
                                }
                            }
                        }
                        newRow1["Agent Name"] = drbrnchamt["Agent Name"].ToString();
                        newRow1["Sale Value"] = drbrnchamt["Sale Value"].ToString();
                        float salevalue = 0;
                        float.TryParse(drbrnchamt["Sale Value"].ToString(), out salevalue);
                        float paidamt = 0;
                        float.TryParse(drbrnchamt["Paid Amount"].ToString(), out paidamt);
                        float todaypaid = 0;
                        float.TryParse(drbrnchamt["today collected"].ToString(), out todaypaid);
                        float oppamt = 0;
                        float.TryParse(drbrnchamt["Oppening Balance"].ToString(), out oppamt);
                        float aopp = oppamt + paidamt - salevalue;
                        float totaldue = aopp + salevalue - paidamt;
                        if (dr["sno"].ToString() == "20")
                        {
                            newRow1["Oppening Balance"] = aopp;
                            newRow1["Paid Amount"] = Math.Round(paidamt, 2);
                            newRow1["Due Amount"] = Math.Round(totaldue, 2);
                            Rpt.Rows.Add(newRow1);
                        }
                        else
                        {
                            newRow1["Paid Amount"] = Math.Round(todaypaid, 2);
                            Rpt.Rows.Add(newRow1);
                        }
                        k++;
                    }
                }
            }
            grdReports.DataSource = Report;
            grdReports.DataBind();
            grdbalance.DataSource = Rpt;
            grdbalance.DataBind();
            Session["xportdata"] = Report;
        }
        catch
        {
        }
    }
}