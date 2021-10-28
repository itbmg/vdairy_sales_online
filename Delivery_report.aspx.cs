using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Text;

public partial class Dreport : System.Web.UI.Page
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType)  ");
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
                LoadDispatch();
            }
            else
            {
                PBranch.Visible = false;
                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID)  and (DispType is NULL) AND   (dispatch.flag=@flag))  OR ((branchdata_1.SalesOfficeID = @SOID)  and (DispType is NULL) AND  (dispatch.flag=@flag))");
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
        LoadDispatch();
    }
    protected void LoadDispatch()
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) AND   (dispatch.flag=@flag) OR (branchdata_1.SalesOfficeID = @SOID) AND   (dispatch.flag=@flag)");
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
            cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName, dispatch.Branch_Id, dispatch.Route_id, tripdata.SyncStatus, tripdata.Sno AS tripid, tripdata.EmpId, tripdata.AssignDate,tripdata.Status FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.sno = @dispatchSno) AND (tripdata.I_Date BETWEEN @d1 AND @d2) AND (tripdata.SyncStatus <> '0')");
            if (chkDispatch.Checked == true)
            {
                cmd.Parameters.AddWithValue("@dispatchSno", ddlPlantDispName.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            }
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtsyncstatus = vdm.SelectQuery(cmd).Tables[0];
            if (dtsyncstatus.Rows.Count > 0)
            {
                btnPrint.Visible = true;
            }
            else
            {
                btnPrint.Visible = false;
                lblmsg.Text = "Data Not Properly Synced";
            }
            // cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.unitQty), 2) AS unitQty, indents_subtable.Product_sno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, ROUND(SUM(indents_subtable.LeakQty), 2) AS LeakQty, indents_subtable.UnitCost, indents.IndentNo, indents.Branch_id, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS Total FROM  dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutes ON dispatch_sub.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN indents ON branchroutesubtable.BranchID = indents.Branch_id AND dispatch_sub.IndentType = indents.IndentType INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (dispatch.sno = @dispatchSno) AND (indents.I_date BETWEEN @starttime AND @endtime) GROUP BY productsdata.ProductName");
            cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.unitQty), 2) AS unitQty, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,ROUND(SUM(indents_subtable.LeakQty), 2) AS LeakQty, indents_subtable.UnitCost, indent.IndentNo, indent.Branch_id,ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS Total, indents_subtable.Product_sno FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutes ON dispatch_sub.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT IndentNo, Branch_id, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indent ON modifiedroutesubtable.BranchID = indent.Branch_id AND dispatch_sub.IndentType = indent.IndentType INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY indents_subtable.Product_sno");
            if (chkDispatch.Checked == true)
            {
                cmd.Parameters.AddWithValue("@dispatchSno", ddlPlantDispName.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            }
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            // cmd = new MySqlCommand("SELECT branchaccounts.BranchId, branchaccounts.Amount, branchdata.BranchName FROM dispatch INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN branchaccounts ON branchdata.sno = branchaccounts.BranchId WHERE (dispatch.sno = @dispsno)");
            cmd = new MySqlCommand("SELECT branchdata.BranchName,branchdata.SalesType, branchaccounts.BranchId, branchaccounts.Amount FROM dispatch INNER JOIN modifiedroutes ON dispatch.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN branchaccounts ON branchdata.sno = branchaccounts.BranchId WHERE (dispatch.sno = @dispsno) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @dt) AND (branchdata.flag=1) OR (dispatch.sno = @dispsno) AND (modifiedroutesubtable.EDate > @dt) AND (modifiedroutesubtable.CDate <= @dt) and (branchdata.flag=1) order by branchdata.BranchName");
            if (chkDispatch.Checked == true)
            {
                cmd.Parameters.AddWithValue("@dispsno", ddlPlantDispName.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@dispsno", ddlRouteName.SelectedValue);
            }
            cmd.Parameters.AddWithValue("@dt", GetLowDate(fromdate.AddDays(-1)));

            DataTable dtbranchammount = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT  inventory_monitor.Inv_Sno, inventory_monitor.Qty,modifiedroutesubtable.BranchID FROM dispatch INNER JOIN modifiedroutes ON dispatch.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN inventory_monitor ON modifiedroutesubtable.BranchID = inventory_monitor.BranchId WHERE (dispatch.sno = @dispsno) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @dt) OR (dispatch.sno = @dispsno) AND (modifiedroutesubtable.EDate > @dt) AND (modifiedroutesubtable.CDate <= @dt) ");
            if (chkDispatch.Checked == true)
            {
                cmd.Parameters.AddWithValue("@dispsno", ddlPlantDispName.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@dispsno", ddlRouteName.SelectedValue);
            }
            cmd.Parameters.AddWithValue("@dt", GetLowDate(fromdate.AddDays(-1)));

            DataTable dtAgentInventory = vdm.SelectQuery(cmd).Tables[0];
            // cmd = new MySqlCommand("SELECT  MIN(Ind.D_date) AS midate, MAX(collect.PaidDate) AS madate FROM dispatch INNER JOIN branchroutesubtable ON dispatch.Route_id = branchroutesubtable.RefNo INNER JOIN (SELECT indents.Branch_id, MIN(indents_subtable.D_date) AS D_date, MAX(indents_subtable.DTripId) AS DTripId FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo WHERE (indents.I_date BETWEEN @starttime AND @endtime) AND (indents_subtable.D_date IS NOT NULL) GROUP BY indents.Branch_id) Ind ON branchroutesubtable.BranchID = Ind.Branch_id INNER JOIN (SELECT PaidDate, tripId FROM collections) collect ON Ind.DTripId = collect.tripId WHERE (dispatch.sno = @dispatchSno)");
            cmd = new MySqlCommand("SELECT MIN(Ind.D_date) AS midate, MAX(collect.PaidDate) AS madate FROM dispatch INNER JOIN modifiedroutesubtable ON dispatch.Route_id = modifiedroutesubtable.RefNo INNER JOIN (SELECT indents.Branch_id, MIN(indents_subtable.D_date) AS D_date, MAX(indents_subtable.DTripId) AS DTripId FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo WHERE (indents.I_date BETWEEN @starttime AND @endtime) AND (indents_subtable.D_date IS NOT NULL) GROUP BY indents.Branch_id) Ind ON modifiedroutesubtable.BranchID = Ind.Branch_id INNER JOIN (SELECT PaidDate, tripId FROM collections) collect ON Ind.DTripId = collect.tripId WHERE (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime)");
            if (chkDispatch.Checked == true)
            {
                cmd.Parameters.AddWithValue("@dispatchSno", ddlPlantDispName.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            }
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(ServerDateCurrentdate));
            DataTable dtminmax = vdm.SelectQuery(cmd).Tables[0];
            string mindate = dtminmax.Rows[0]["midate"].ToString();
            string maxdate = dtminmax.Rows[0]["madate"].ToString();
            DateTime midate = DateTime.Parse(mindate);
            DateTime madate = DateTime.Parse(maxdate);
            cmd = new MySqlCommand("SELECT branchdata.BranchName, indents_subtable.DTripId, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS totalamount, indents_subtable.D_date, indent.Branch_id, modifiedroutes.Sno FROM dispatch INNER JOIN modifiedroutes ON dispatch.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutesubtable.RefNo = modifiedroutes.Sno INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, I_date, Branch_id FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo WHERE (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) AND (dispatch.sno = @dispatchSno) OR (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (dispatch.sno = @dispatchSno) GROUP BY branchdata.BranchName, modifiedroutes.Sno");
            //  cmd = new MySqlCommand("SELECT branchdata.BranchName, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS totalamount, indents_subtable.D_date, collections.AmountPaid,indent.Branch_id FROM dispatch INNER JOIN modifiedroutes ON dispatch.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo LEFT OUTER JOIN collections ON indents_subtable.DTripId = collections.tripId AND branchdata.sno = collections.Branchid WHERE (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY branchdata.BranchName"); 
            if (chkDispatch.Checked == true)
            {
                cmd.Parameters.AddWithValue("@dispatchSno", ddlPlantDispName.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            }
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@Paidstime", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@Paidetime", GetHighDate(fromdate));
            DataTable dttodaycollection = vdm.SelectQuery(cmd).Tables[0];
            if (dttodaycollection.Rows.Count > 0)
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, collections.Branchid, branchdata.sno, collections.AmountPaid, collections.PaidDate, collections.PaymentType, collections.CheckStatus, collections.PayTime, collections.ChequeNo, collections.tripId, collections.ReceiptNo FROM dispatch INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN  branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN collections ON branchdata.sno = collections.Branchid WHERE (dispatch.sno = @dispatchsno) AND (collections.AmountPaid <> 0) AND (collections.tripId = @TripID)");
                if (chkDispatch.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@dispatchSno", ddlPlantDispName.SelectedValue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
                }
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
            //cmd = new MySqlCommand("SELECT result.BranchName, result.totalamount, result.DelTime, result.PayTime, result.D_date, SUM(collections_1.AmountPaid) AS AmountPaid, result.Branch_id FROM (SELECT branchdata.BranchName, SUM(indentssub.DeliveryQty * indentssub.UnitCost) AS totalamount, indentssub.DelTime, collections.PayTime,indentssub.D_date, SUM(collections.AmountPaid) AS AmountPaid, indents.Branch_id, indentssub.DTripId, branchdata.sno FROM dispatch INNER JOIN modifiedroutes ON dispatch.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN indents ON branchdata.sno = indents.Branch_id INNER JOIN (SELECT IndentNo, Product_sno, DeliveryQty, D_date, UnitCost, DTripId, DelTime FROM indents_subtable WHERE (D_date BETWEEN @starttime AND @endtime)) indentssub ON indents.IndentNo = indentssub.IndentNo LEFT OUTER JOIN collections ON indentssub.DTripId = collections.tripId AND branchdata.sno = collections.Branchid WHERE (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY branchdata.sno, collections.Branchid) result INNER JOIN collections collections_1 ON result.Branch_id = collections_1.Branchid WHERE (collections_1.PaidDate BETWEEN @starttime AND @endtime) AND (collections_1.tripId <> 'NULL') GROUP BY result.BranchName");
            //cmd = new MySqlCommand("SELECT result.Branch_id, result.BranchName, result.totalamount, result.D_date, result.DTripId, result.DelTime, collect.AmountPaid, collect.PayTime FROM (SELECT branchdata.sno AS Branch_id, branchdata.BranchName, SUM(indentssub.DeliveryQty * indentssub.UnitCost) AS totalamount, indentssub.D_date,indentssub.DTripId, indentssub.DelTime FROM (SELECT IndentNo, Product_sno, DeliveryQty, D_date, UnitCost, DTripId, DelTime FROM indents_subtable WHERE (D_date BETWEEN @starttime AND @endtime)) indentssub INNER JOIN (SELECT IndentNo, Branch_id, TotalQty, TotalPrice, I_date, D_date, Status, UserData_sno, PaymentStatus, I_createdby, I_modifiedby,IndentType FROM indents WHERE (I_date BETWEEN @Indd1 AND @indd2)) ind ON indentssub.IndentNo = ind.IndentNo RIGHT OUTER JOIN dispatch INNER JOIN modifiedroutes ON dispatch.Route_id = modifiedroutes.Sno INNER JOIN (SELECT RefNo, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifroutessub ON modifiedroutes.Sno = modifroutessub.RefNo INNER JOIN branchdata ON modifroutessub.BranchID = branchdata.sno ON ind.Branch_id = branchdata.sno WHERE (dispatch.sno = @dispatchSno) GROUP BY branchdata.sno) result INNER JOIN (SELECT Branchid, UserData_sno, SUM(AmountPaid) AS AmountPaid, PaidDate, PaymentType, tripId, PayTime FROM collections WHERE (PaidDate BETWEEN @Paidstime AND @Paidetime) AND (tripId <> 'NULL') GROUP BY Branchid) collect ON result.Branch_id = collect.Branchid");
            cmd = new MySqlCommand("SELECT  result.Branch_id, result.BranchName, result.totalamount, result.D_date, result.DTripId, result.DelTime, SUM(collections.AmountPaid) AS AmountPaid,collections.PayTime FROM (SELECT branchdata.sno AS Branch_id, branchdata.BranchName, SUM(indentssub.DeliveryQty * indentssub.UnitCost) AS totalamount, indentssub.D_date,indentssub.DTripId, indentssub.DelTime FROM (SELECT IndentNo, Product_sno, DeliveryQty, D_date, UnitCost, DTripId, DelTime FROM indents_subtable WHERE (D_date BETWEEN @starttime AND @endtime)) indentssub INNER JOIN (SELECT IndentNo, Branch_id, TotalQty, TotalPrice, I_date, D_date, Status, UserData_sno, PaymentStatus, I_createdby, I_modifiedby,IndentType FROM indents WHERE (I_date BETWEEN @Indd1 AND @indd2)) ind ON indentssub.IndentNo = ind.IndentNo RIGHT OUTER JOIN dispatch INNER JOIN modifiedroutes ON dispatch.Route_id = modifiedroutes.Sno INNER JOIN (SELECT RefNo, BranchID, CDate, EDate FROM  modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifroutessub ON modifiedroutes.Sno = modifroutessub.RefNo INNER JOIN branchdata ON modifroutessub.BranchID = branchdata.sno ON ind.Branch_id = branchdata.sno WHERE (dispatch.sno = @dispatchSno) GROUP BY branchdata.sno) result INNER JOIN collections ON result.Branch_id = collections.Branchid WHERE (collections.PaidDate BETWEEN @starttime AND @endtime) AND (collections.tripId <> 'NULL') GROUP BY result.Branch_id");
            if (chkDispatch.Checked == true)
            {
                cmd.Parameters.AddWithValue("@dispatchSno", ddlPlantDispName.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            }
            cmd.Parameters.AddWithValue("@starttime", midate);
            cmd.Parameters.AddWithValue("@endtime", madate);
            cmd.Parameters.AddWithValue("@Paidstime", midate);
            cmd.Parameters.AddWithValue("@Paidetime", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@Indd1", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@Indd2", GetHighDate(madate));
            DataTable dtBranchcollection = vdm.SelectQuery(cmd).Tables[0];
            //cmd = new MySqlCommand("SELECT result.BranchName, result.totalamount, result.DelTime, result.PayTime, result.D_date, SUM(collections_1.AmountPaid) AS AmountPaid, result.Branch_id FROM (SELECT branchdata.BranchName, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS totalamount, indents_subtable.DelTime,collections.PayTime, indents_subtable.D_date, SUM(collections.AmountPaid) AS AmountPaid, indents.Branch_id, indents_subtable.DTripId, branchdata.sno FROM collections INNER JOIN (SELECT IndentNo, Product_sno, Qty, Cost, Remark, DeliveryQty, Status, D_date, unitQty, UnitCost, Sno, PaymentStatus, LeakQty, OTripId, DTripId, DelTime FROM indents_subtable indents_subtable_1 WHERE (D_date BETWEEN @starttime AND @endtime)) indents_subtable ON collections.tripId = indents_subtable.DTripId LEFT OUTER JOIN indents INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN branchroutesubtable ON branchdata.sno = branchroutesubtable.BranchID INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno INNER JOIN dispatch ON branchroutes.Sno = dispatch.Route_id ON collections.Branchid = indents.Branch_id AND  indents_subtable.IndentNo = indents.IndentNo WHERE        (dispatch.sno = @dispatchSno)  GROUP BY branchdata.BranchName, collections.Branchid) result INNER JOIN collections collections_1 ON result.Branch_id = collections_1.Branchid WHERE (collections_1.PaidDate BETWEEN @starttime AND @endtime) AND (collections_1.tripId IS NULL) GROUP BY result.BranchName");
            //cmd = new MySqlCommand("SELECT result.BranchName, result.totalamount, result.DelTime, result.PayTime, result.D_date, SUM(collections_1.AmountPaid) AS AmountPaid, result.Branch_id FROM (SELECT branchdata.BranchName, SUM(indentssub.DeliveryQty * indentssub.UnitCost) AS totalamount, indentssub.DelTime, collections.PayTime,indentssub.D_date, SUM(collections.AmountPaid) AS AmountPaid, indents.Branch_id, indentssub.DTripId, branchdata.sno FROM dispatch INNER JOIN modifiedroutes ON dispatch.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN indents ON branchdata.sno = indents.Branch_id INNER JOIN (SELECT IndentNo, Product_sno, DeliveryQty, D_date, UnitCost, DTripId, DelTime FROM indents_subtable WHERE (D_date BETWEEN @starttime AND @endtime)) indentssub ON indents.IndentNo = indentssub.IndentNo LEFT OUTER JOIN collections ON indentssub.DTripId = collections.tripId AND branchdata.sno = collections.Branchid WHERE (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY branchdata.sno, collections.Branchid) result INNER JOIN collections collections_1 ON result.Branch_id = collections_1.Branchid WHERE (collections_1.PaidDate BETWEEN @starttime AND @endtime) AND (collections_1.tripId IS NULL) GROUP BY result.BranchName");
            cmd = new MySqlCommand("SELECT  result.Branch_id, result.BranchName, result.totalamount, result.D_date, result.DTripId, result.DelTime, SUM(collections.AmountPaid) AS AmountPaid,collections.PayTime FROM (SELECT branchdata.sno AS Branch_id, branchdata.BranchName, SUM(indentssub.DeliveryQty * indentssub.UnitCost) AS totalamount, indentssub.D_date,indentssub.DTripId, indentssub.DelTime FROM (SELECT IndentNo, Product_sno, DeliveryQty, D_date, UnitCost, DTripId, DelTime FROM indents_subtable WHERE (D_date BETWEEN @starttime AND @endtime)) indentssub INNER JOIN (SELECT IndentNo, Branch_id, TotalQty, TotalPrice, I_date, D_date, Status, UserData_sno, PaymentStatus, I_createdby, I_modifiedby,IndentType FROM indents WHERE (I_date BETWEEN @Indd1 AND @indd2)) ind ON indentssub.IndentNo = ind.IndentNo RIGHT OUTER JOIN dispatch INNER JOIN modifiedroutes ON dispatch.Route_id = modifiedroutes.Sno INNER JOIN (SELECT RefNo, BranchID, CDate, EDate FROM  modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifroutessub ON modifiedroutes.Sno = modifroutessub.RefNo INNER JOIN branchdata ON modifroutessub.BranchID = branchdata.sno ON ind.Branch_id = branchdata.sno WHERE (dispatch.sno = @dispatchSno) GROUP BY branchdata.sno) result INNER JOIN collections ON result.Branch_id = collections.Branchid WHERE (collections.PaidDate BETWEEN @starttime AND @endtime) AND (collections.tripId IS NULL) GROUP BY result.Branch_id");

            cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            cmd.Parameters.AddWithValue("@starttime", midate);
            cmd.Parameters.AddWithValue("@endtime", madate);
            cmd.Parameters.AddWithValue("@Paidstime", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@Paidetime", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@Indd1", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@Indd2", GetHighDate(madate));
            DataTable dtsalesofficecollection = vdm.SelectQuery(cmd).Tables[0];
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
            cmd = new MySqlCommand("SELECT tripdata.Sno, tripdata.Denominations, tripdata.Remarks, tripdata.CollectedAmount, tripdata.SubmittedAmount, tripdata.Cdate, empmanage.EmpName FROM tripdata INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN empmanage ON tripdata.EmpId = empmanage.Sno WHERE (tripdata.I_Date BETWEEN @starttime AND @endtime) AND (dispatch.sno = @dispatchSno) AND (tripdata.Status <> 'C')");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            if (chkDispatch.Checked == true)
            {
                cmd.Parameters.AddWithValue("@dispatchSno", ddlPlantDispName.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            }
            DataTable dtDenomin = vdm.SelectQuery(cmd).Tables[0];
            ///09/01/2016  Ravi
            cmd = new MySqlCommand("SELECT  ff.TripID, Triproutes.RouteID, ff.DispQty, ff.sno FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (RouteID = @dispatchSno)) Triproutes INNER JOIN (SELECT TripID, DispQty, sno FROM  (SELECT tripdata.Sno AS TripID, tripsubdata.Qty AS DispQty, tripsubdata.ProductId AS sno FROM  tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno WHERE (tripdata.I_Date BETWEEN @starttime AND @endtime)) tripinfo) ff ON ff.TripID = Triproutes.Tripdata_sno");
            //cmd = new MySqlCommand("SELECT tripdata.sno as TripID,tripsubdata.Qty as DispQty, productsdata.ProductName,productsdata.sno FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN triproutes ON triproutes.Tripdata_sno = tripsubdata.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE  (tripdata.I_Date BETWEEN @starttime AND @endtime) AND (dispatch.sno = @dispatchsno)");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            if (chkDispatch.Checked == true)
            {
                cmd.Parameters.AddWithValue("@dispatchSno", ddlPlantDispName.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            }
            DataTable dtSubData = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName, ROUND(SUM(offer_indents_sub.offer_indent_qty), 2) AS unitQty, productsdata.ProductName, productsdata.Units, productsdata.sno AS productid, ROUND(SUM(offer_indents_sub.offer_delivered_qty), 2) AS Delqty, products_category.Categoryname FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutes ON dispatch_sub.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT idoffer_indents, idoffers_assignment, salesoffice_id, route_id, agent_id, indent_date, indents_id, IndentType, I_modified_by FROM offer_indents WHERE (indent_date BETWEEN @starttime AND @endtime) ) offerindents ON modifiedroutesubtable.BranchID = offerindents.agent_id INNER JOIN offer_indents_sub ON offerindents.idoffer_indents = offer_indents_sub.idoffer_indents INNER JOIN productsdata ON offer_indents_sub.product_id = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) AND (dispatch.sno = @dispatchSno) OR (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (dispatch.sno = @dispatchSno) GROUP BY productsdata.sno");
            if (chkDispatch.Checked == true)
            {
                cmd.Parameters.AddWithValue("@dispatchSno", ddlPlantDispName.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            }
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            DataTable dt_offertble = vdm.SelectQuery(cmd).Tables[0];


            DataTable dtLeakages = new DataTable();
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
            foreach (DataRow drprdt in dtallprdts.Rows)
            {
                foreach (DataRow drindent in dtble.Rows)
                {
                    if (drprdt["Product_sno"].ToString() == drindent["Product_sno"].ToString())
                    {
                        float qty = 0;
                        float unitqty = 0;
                        float leakqty = 0;
                        float total = 0;
                        float.TryParse(drindent["DeliveryQty"].ToString(), out qty);
                        float.TryParse(drindent["unitQty"].ToString(), out unitqty);
                        float.TryParse(drindent["LeakQty"].ToString(), out leakqty);
                        float.TryParse(drindent["Total"].ToString(), out total);
                        float qtycpy = 0;
                        float unitqtycpy = 0;
                        float leakqtycpy = 0;
                        float totalcpy = 0;
                        float.TryParse(drprdt["DeliveryQty"].ToString(), out qtycpy);
                        float.TryParse(drprdt["unitQty"].ToString(), out unitqtycpy);
                        float.TryParse(drprdt["LeakQty"].ToString(), out leakqtycpy);
                        float.TryParse(drprdt["Total"].ToString(), out totalcpy);

                        float totalqty = qty + qtycpy;
                        float totalunitqty = unitqty + unitqtycpy;
                        float totalleakqty = leakqty + leakqtycpy;
                        float totalsaleqty = total + totalcpy;
                        drprdt["DeliveryQty"] = totalqty;
                        drprdt["unitQty"] = totalunitqty;
                        drprdt["LeakQty"] = totalleakqty;
                        drprdt["Total"] = totalsaleqty;
                    }
                }
            }
            DataTable dtInventory = new DataTable();
            if (dtSubData.Rows.Count > 0)
            {
                string Sno = dtSubData.Rows[0]["TripID"].ToString();
                cmd = new MySqlCommand("select LeakQty,ShortQty,FreeMilk,ProductID from Leakages where TripId=@TripId and VarifyStatus IS NULL Group by ProductID order by ProductID ");
                cmd.Parameters.AddWithValue("@TripId", Sno);
                dtLeakages = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT tripinvdata.Qty, tripinvdata.Remaining,invmaster.sno, invmaster.InvName FROM tripinvdata INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (tripinvdata.Tripdata_sno = @Tripdata_sno) GROUP BY invmaster.InvName ORDER BY invmaster.sno");
                cmd.Parameters.AddWithValue("@Tripdata_sno", Sno);
                dtInventory = vdm.SelectQuery(cmd).Tables[0];
            }
            dtble.DefaultView.Sort = "Product_sno ASC";
            dtble = dtble.DefaultView.ToTable(true);
            if (dtble.Rows.Count > 0)
            {
                Report = new DataTable();
                //Report.
                Report.Columns.Add("Variety");
                Report.Columns.Add("Qty");
                Report.Columns.Add("DispQty");
                Report.Columns.Add("Crates");
                Report.Columns.Add("Returns");
                Report.Columns.Add("Leaks");
                Report.Columns.Add("Short");
                Report.Columns.Add("FreeMilk");
                Report.Columns.Add("Sales");
                Report.Columns.Add("Sales Value");
                //Report.Columns.Add("Denomin");
                double totalqty = 0;
                double Leakqty = 0;
                double tDispqty = 0;
                double tReturnqty = 0;
                double delqty = 0;
                double TotAmount = 0;
                double Totdueamount = 0;
                double Totcompassamount = 0;
                double Totfhamount = 0;
                double Totcateringamount = 0;
                double Totinstituteamount = 0;
                double Totdueagentamount = 0;
                double TotCRagentamount = 0;
                int count = 0;
                if (dtDenomin.Rows.Count > 0)
                {
                    lblEmpName.Text = dtDenomin.Rows[0]["EmpName"].ToString();
                    string stramount = dtDenomin.Rows[0]["Denominations"].ToString();
                    stramount = stramount.Replace("+", " ");
                    foreach (string str in stramount.Split(' '))
                    {
                        if (str == "")
                        {
                        }
                        else
                        {
                            count++;
                        }
                    }
                }
                float totdispqty = 0;
                count = count + 1;
                if (dtble.Rows.Count > count)
                {
                    foreach (DataRow branch in dtallprdts.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Variety"] = branch["ProductName"].ToString();
                        newrow["Qty"] = branch["unitQty"].ToString();
                        float DispQty = 0;
                        float ReturnQty = 0;
                        foreach (DataRow drSubData in dtSubData.Rows)
                        {
                            if (branch["Product_sno"].ToString() == drSubData["sno"].ToString())
                            {
                                float.TryParse(drSubData["DispQty"].ToString(), out DispQty);
                                if (DispQty > 0)
                                {
                                    newrow["DispQty"] = drSubData["DispQty"].ToString();
                                    totdispqty += DispQty;
                                }
                            }
                        }
                        float Leaks = 0;
                        float Totqty = 0;
                        float ShortQty = 0;
                        float FreeMilk = 0;
                        float DeliveryQty = 0;
                        if (dtLeakages.Rows.Count > 0)
                        {
                            string ProductID = branch["Product_sno"].ToString();
                            DataRow[] drleak = dtLeakages.Select("ProductID = '" + ProductID + "'");
                            if (drleak.Length != 0)
                            {
                                for (int i = 0; i < drleak.Length; i++)
                                {
                                    if (branch["Product_sno"].ToString() == drleak[i][3].ToString())
                                    {
                                        string str = drleak[i][0].ToString();
                                        float Ileak = 0;
                                        float.TryParse(branch["LeakQty"].ToString(), out Ileak);
                                        float Rleak = 0;
                                        float.TryParse(str, out Rleak);
                                        Leaks = Ileak + Rleak;
                                        newrow["Leaks"] = Leaks;
                                        //float ShortQty = 0;
                                        float.TryParse(drleak[i][1].ToString(), out ShortQty);
                                        newrow["Short"] = ShortQty;
                                        //float FreeMilk = 0;
                                        float.TryParse(drleak[i][2].ToString(), out FreeMilk);
                                        float schemeqty = 0;
                                        foreach (DataRow drscheme in dt_offertble.Select("productid='" + branch["Product_sno"].ToString() + "'"))
                                        {
                                            float.TryParse(drscheme["Delqty"].ToString(), out schemeqty);
                                        }
                                        newrow["FreeMilk"] = FreeMilk + schemeqty;
                                        //float DeliveryQty = 0;
                                        float.TryParse(branch["DeliveryQty"].ToString(), out DeliveryQty);
                                        Totqty = Leaks + DeliveryQty + FreeMilk + ShortQty + schemeqty;
                                        ReturnQty = DispQty - Totqty;
                                        newrow["Returns"] = Math.Round(ReturnQty, 2);
                                    }
                                    else
                                    {
                                        newrow["Leaks"] = branch["LeakQty"].ToString();
                                        float.TryParse(branch["LeakQty"].ToString(), out Leaks);
                                        //float DeliveryQty = 0;
                                        float schemeqty = 0;
                                        foreach (DataRow drscheme in dt_offertble.Select("productid='" + branch["Product_sno"].ToString() + "'"))
                                        {
                                            float.TryParse(drscheme["Delqty"].ToString(), out schemeqty);
                                        }
                                        float.TryParse(branch["DeliveryQty"].ToString(), out DeliveryQty);
                                        //float ShortQty = 0;
                                        newrow["Short"] = ShortQty;
                                        //float FreeMilk = 0;
                                        newrow["FreeMilk"] = FreeMilk + schemeqty;
                                        Totqty = Leaks + DeliveryQty + FreeMilk + ShortQty + schemeqty;
                                        ReturnQty = DispQty - Totqty;
                                        newrow["Returns"] = Math.Round(ReturnQty, 2);
                                    }
                                    tReturnqty += Math.Round(ReturnQty, 2);
                                    tDispqty += DispQty;
                                    Leakqty += Leaks;
                                }
                            }
                            else
                            {
                                newrow["Leaks"] = branch["LeakQty"].ToString();
                                float.TryParse(branch["LeakQty"].ToString(), out Leaks);
                                //float DeliveryQty = 0;
                                float schemeqty = 0;
                                foreach (DataRow drscheme in dt_offertble.Select("productid='" + branch["Product_sno"].ToString() + "'"))
                                {
                                    float.TryParse(drscheme["Delqty"].ToString(), out schemeqty);
                                }
                                float.TryParse(branch["DeliveryQty"].ToString(), out DeliveryQty);
                                //float ShortQty = 0;
                                newrow["Short"] = ShortQty;
                                //float FreeMilk = 0;
                                newrow["FreeMilk"] = FreeMilk + schemeqty;
                                Totqty = Leaks + DeliveryQty + FreeMilk + ShortQty + schemeqty;
                                ReturnQty = DispQty - Totqty;
                                newrow["Returns"] = Math.Round(ReturnQty, 2);
                                tReturnqty += Math.Round(ReturnQty, 2); ;
                                tDispqty += DispQty;
                                Leakqty += Leaks;
                            }
                        }
                        else
                        {
                            newrow["Leaks"] = branch["LeakQty"].ToString();
                            float.TryParse(branch["LeakQty"].ToString(), out Leaks);
                            float schemeqty = 0;
                            foreach (DataRow drscheme in dt_offertble.Select("productid='" + branch["Product_sno"].ToString() + "'"))
                            {
                                float.TryParse(drscheme["Delqty"].ToString(), out schemeqty);
                            }
                            //float DeliveryQty = 0;
                            float.TryParse(branch["DeliveryQty"].ToString(), out DeliveryQty);
                            //float ShortQty = 0;
                            newrow["Short"] = ShortQty;
                            //float FreeMilk = 0;
                            newrow["FreeMilk"] = FreeMilk + schemeqty;
                            Totqty = Leaks + DeliveryQty + FreeMilk + ShortQty + schemeqty;
                            ReturnQty = DispQty - Totqty;
                            newrow["Returns"] = Math.Round(ReturnQty, 2);
                            tReturnqty += Math.Round(ReturnQty, 2); ;
                            tDispqty += DispQty;
                            Leakqty += Leaks;
                        }
                        newrow["Sales"] = branch["DeliveryQty"].ToString();
                        newrow["Sales Value"] = branch["Total"].ToString();
                        double qtyvalue = 0;
                        double Leakqtyvalue = 0;
                        float delqtyvalue = 0;
                        double TotAmountvalue = 0;
                        double.TryParse(branch["unitQty"].ToString(), out qtyvalue);
                        totalqty += Math.Round(qtyvalue, 2);
                        float.TryParse(branch["DeliveryQty"].ToString(), out delqtyvalue);
                        delqty += Math.Round(delqtyvalue, 2);
                        float Prdtschemeqty1 = 0;
                        foreach (DataRow drscheme in dt_offertble.Select("productid='" + branch["Product_sno"].ToString() + "'"))
                        {
                            float.TryParse(drscheme["Delqty"].ToString(), out Prdtschemeqty1);
                        }
                        Totqty = Leaks + delqtyvalue + FreeMilk + ShortQty + Prdtschemeqty1;
                        ReturnQty = DispQty - Totqty;
                        newrow["Returns"] = Math.Round(ReturnQty, 2);
                        double.TryParse(branch["Total"].ToString(), out TotAmountvalue);
                        TotAmount += Math.Round(TotAmountvalue, 2);
                        if (DispQty > 0)
                        {
                            Report.Rows.Add(newrow);
                        }
                    }
                }
                else
                {
                    foreach (DataRow branch in dtallprdts.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["Variety"] = branch["ProductName"].ToString();
                        newrow["Qty"] = branch["unitQty"].ToString();
                        float DispQty = 0;
                        float ReturnQty = 0;
                        foreach (DataRow drSubData in dtSubData.Rows)
                        {
                            if (branch["Product_sno"].ToString() == drSubData["sno"].ToString())
                            {
                                float.TryParse(drSubData["DispQty"].ToString(), out DispQty);
                                if (DispQty > 0)
                                {
                                    newrow["DispQty"] = drSubData["DispQty"].ToString();
                                    totdispqty += DispQty;
                                }
                            }
                        }
                        float Leaks = 0;
                        float Totqty = 0;
                        float ShortQty = 0;
                        float FreeMilk = 0;
                        float DeliveryQty = 0;

                        if (dtLeakages.Rows.Count > 0)
                        {
                            string ProductID = branch["Product_sno"].ToString();
                            DataRow[] drleak = dtLeakages.Select("ProductID = '" + ProductID + "'");
                            for (int i = 0; i < drleak.Length; i++)
                            {
                                if (branch["Product_sno"].ToString() == drleak[i][3].ToString())
                                {
                                    float Ileak = 0;
                                    float.TryParse(branch["LeakQty"].ToString(), out Ileak);
                                    float Rleak = 0;
                                    float.TryParse(drleak[i][0].ToString(), out Rleak);
                                    Leaks = Ileak + Rleak;
                                    newrow["Leaks"] = Leaks;
                                    // float ShortQty = 0;
                                    float.TryParse(drleak[i][1].ToString(), out ShortQty);
                                    newrow["Short"] = ShortQty;
                                    //float FreeMilk = 0;
                                    float.TryParse(drleak[i][2].ToString(), out FreeMilk);
                                    //float DeliveryQty = 0;
                                    float schemeqty = 0;
                                    foreach (DataRow drscheme in dt_offertble.Select("productid='" + branch["Product_sno"].ToString() + "'"))
                                    {
                                        float.TryParse(drscheme["Delqty"].ToString(), out schemeqty);
                                    }
                                    newrow["FreeMilk"] = FreeMilk + schemeqty;

                                    float.TryParse(branch["DeliveryQty"].ToString(), out DeliveryQty);
                                    Totqty = Leaks + DeliveryQty + FreeMilk + ShortQty + schemeqty;
                                    ReturnQty = DispQty - Totqty;
                                    newrow["Returns"] = Math.Round(ReturnQty, 2);
                                }
                                else
                                {
                                    newrow["Leaks"] = branch["LeakQty"].ToString();
                                    float.TryParse(branch["LeakQty"].ToString(), out Leaks);
                                    //float DeliveryQty = 0;
                                    float.TryParse(branch["DeliveryQty"].ToString(), out DeliveryQty);
                                    //float ShortQty = 0;
                                    newrow["Short"] = ShortQty;
                                    //float FreeMilk = 0;
                                    float schemeqty = 0;
                                    foreach (DataRow drscheme in dt_offertble.Select("productid='" + branch["Product_sno"].ToString() + "'"))
                                    {
                                        float.TryParse(drscheme["Delqty"].ToString(), out schemeqty);
                                    }
                                    newrow["FreeMilk"] = FreeMilk + schemeqty;

                                    Totqty = Leaks + DeliveryQty + FreeMilk + ShortQty + schemeqty;
                                    ReturnQty = DispQty - Totqty;
                                    newrow["Returns"] = Math.Round(ReturnQty, 2);
                                }
                                tReturnqty += Math.Round(ReturnQty, 2);
                                tDispqty += DispQty;
                                Leakqty += Leaks;
                            }
                        }
                        else
                        {
                            newrow["Leaks"] = branch["LeakQty"].ToString();
                            float.TryParse(branch["LeakQty"].ToString(), out Leaks);
                            //float DeliveryQty = 0;
                            float.TryParse(branch["DeliveryQty"].ToString(), out DeliveryQty);
                            //float ShortQty = 0;
                            newrow["Short"] = ShortQty;
                            //float FreeMilk = 0;
                            float schemeqty = 0;
                            foreach (DataRow drscheme in dt_offertble.Select("productid='" + branch["Product_sno"].ToString() + "'"))
                            {
                                float.TryParse(drscheme["Delqty"].ToString(), out schemeqty);
                            }
                            newrow["FreeMilk"] = FreeMilk + schemeqty;

                            Totqty = Leaks + DeliveryQty + FreeMilk + ShortQty + schemeqty;
                            ReturnQty = DispQty - Totqty;
                            newrow["Returns"] = Math.Round(ReturnQty, 2);
                            tReturnqty += Math.Round(ReturnQty, 2); ;
                            tDispqty += DispQty;
                            Leakqty += Leaks;
                        }
                        newrow["Sales"] = branch["DeliveryQty"].ToString();
                        newrow["Sales Value"] = branch["Total"].ToString();
                        double qtyvalue = 0;
                        double Leakqtyvalue = 0;
                        float delqtyvalue = 0;
                        double TotAmountvalue = 0;
                        double.TryParse(branch["unitQty"].ToString(), out qtyvalue);
                        totalqty += Math.Round(qtyvalue, 2);
                        float.TryParse(branch["DeliveryQty"].ToString(), out delqtyvalue);
                        delqty += Math.Round(delqtyvalue, 2);
                        float Prdtschemeqty = 0;
                        foreach (DataRow drscheme in dt_offertble.Select("productid='" + branch["Product_sno"].ToString() + "'"))
                        {
                            float.TryParse(drscheme["Delqty"].ToString(), out Prdtschemeqty);
                        }
                        newrow["FreeMilk"] = FreeMilk + Prdtschemeqty;
                        Totqty = Leaks + delqtyvalue + FreeMilk + ShortQty + Prdtschemeqty;
                        ReturnQty = DispQty - Totqty;
                        newrow["Returns"] = Math.Round(ReturnQty, 2);
                        double.TryParse(branch["Total"].ToString(), out TotAmountvalue);
                        TotAmount += Math.Round(TotAmountvalue, 2);
                        if (DispQty > 0)
                        {
                            Report.Rows.Add(newrow);
                        }
                    }
                    int RowCount = count - dtble.Rows.Count;
                    if (RowCount == 0)
                    {
                        RowCount = RowCount + 1;
                    }
                    else
                    {
                        RowCount = RowCount + 1;
                    }
                    for (int i = 0; i < RowCount; i++)
                    {
                        DataRow newrow1 = Report.NewRow();
                        newrow1["Variety"] = "";
                        newrow1["Qty"] = "";
                        newrow1["DispQty"] = "";
                        newrow1["Returns"] = "";
                        newrow1["Leaks"] = "";
                        newrow1["Sales"] = "";
                        newrow1["Sales Value"] = "";
                        Report.Rows.Add(newrow1);
                    }
                }
                //cmd = new MySqlCommand("SELECT indents.Branch_id,branchdata.salestype, branchdata.BranchName, collections.Branchid, collections.AmountPaid, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS totalamount, collections.PaidDate, indents_subtable.D_date FROM indents INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN branchroutesubtable ON branchdata.sno = branchroutesubtable.BranchID INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno INNER JOIN collections ON indents.Branch_id = collections.Branchid INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN dispatch ON branchroutes.Sno = dispatch.Route_id WHERE (indents.I_date between @starttime AND  @endtime) AND (collections.PaidDate between @Paidstime AND  @Paidetime) AND (dispatch.sno = @dispatchSno)  GROUP BY branchdata.BranchName");
                //cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
                //cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                //cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
                //cmd.Parameters.AddWithValue("@Paidstime", GetLowDate(fromdate));
                //cmd.Parameters.AddWithValue("@Paidetime", GetHighDate(fromdate));
                //DataTable dtBranch = vdm.SelectQuery(cmd).Tables[0];
                //DataView view = new DataView(dtBranch);
                //DataTable distincttable = view.ToTable(true, "BranchName", "AmountPaid", "salestype", "Branchid");
                //foreach (DataRow dd in distincttable.Rows)
                //{
                //    string amount = dd["AmountPaid"].ToString();
                //    int NagitiveAmount = 0;
                //    int.TryParse(amount, out NagitiveAmount);
                //    float AmountPaid = 0;
                //    float.TryParse(dd["AmountPaid"].ToString(), out AmountPaid);
                //    DataRow newrow = Report.NewRow();
                //    float Totamount = 0;
                //    foreach (DataRow branch in dtBranch.Select("Branchid=" + dd["Branchid"].ToString()))
                //    {
                //        if (dd["BranchName"].ToString() == branch["BranchName"].ToString())
                //        {
                //            float totalAmount = 0;
                //            float.TryParse(branch["totalAmount"].ToString(), out totalAmount);
                //            Totamount += totalAmount;
                //        }
                //    }
                //    if (AmountPaid == Totamount)
                //    {
                //    }
                //    else
                //    {
                //        AmountPaid = Math.Abs(AmountPaid);
                //        float dueAmount = Totamount - AmountPaid;
                //        if (dueAmount <= 1)
                //        {
                //            dueAmount = 0;
                //        }
                //        else
                //        {
                           
                //        }
                //    }
                //}
                int tablecounts = Report.Rows.Count;
                if (tablecounts >= 10)
                {
                    DataRow newvartical = Report.NewRow();
                    newvartical["Variety"] = "Total";
                    newvartical["Qty"] = Math.Round(totalqty, 2);
                    newvartical["DispQty"] = Math.Round(totdispqty, 2);
                    newvartical["Returns"] = Math.Round(tReturnqty, 2);
                    newvartical["Leaks"] = Math.Round(Leakqty, 2);
                    newvartical["Sales"] = Math.Round(delqty, 2);
                    newvartical["Sales Value"] = TotAmount;
                    //newvartical["Count"] = Math.Round(Totdueamount, 2);
                    double diffvalu = TotAmount - Math.Round(Totdueamount, 2);
                    Report.Rows.Add(newvartical);
                }
                if (tablecounts < 10)
                {
                    int j = 10 - tablecounts;
                    for (int i = 0; i <= j; i++)
                    {
                        if (tablecounts == 10)
                        {
                            DataRow newvartical = Report.NewRow();
                            newvartical["Variety"] = "Total";
                            newvartical["Qty"] = Math.Round(totalqty, 2);
                            newvartical["DispQty"] = Math.Round(totdispqty, 2);
                            newvartical["Returns"] = Math.Round(tReturnqty, 2);
                            newvartical["Leaks"] = Math.Round(Leakqty, 2);
                            newvartical["Sales"] = Math.Round(delqty, 2);
                            newvartical["Sales Value"] = TotAmount;
                            //newvartical["Count"] = Math.Round(Totdueamount, 2);
                            double diffvalu = TotAmount - Math.Round(Totdueamount, 2);
                            Report.Rows.Add(newvartical);
                        }
                        else
                        {
                            DataRow newvartical1 = Report.NewRow();
                            newvartical1["Variety"] = "";
                            Report.Rows.Add(newvartical1);
                            tablecounts++;
                        }
                    }


                }
                if (dtDenomin.Rows.Count > 0)
                {
                   
                    DataRow empty = Report.NewRow();
                    empty["Variety"] = "";
                    empty["Crates"] = "";
                    empty["Qty"] = "";
                    empty["DispQty"] = "";
                    empty["Returns"] = "";
                    empty["Leaks"] = "";
                    empty["Sales"] = "";
                    empty["Sales Value"] = "";
                    Report.Rows.Add(empty);
                }

                DataRow drnew1 = Report.NewRow();
                drnew1["Variety"] = "Inventory";
                drnew1["Qty"] = "Opp";
                drnew1["DispQty"] = "Issued";
                drnew1["Returns"] = "Received";
                drnew1["Leaks"] = "Difference";
                drnew1["Short"] = "Closing";
                Report.Rows.Add(drnew1);
                //cmd = new MySqlCommand("SELECT modifiedroutes.Sno AS routesno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, inventory_monitor.Inv_Sno, SUM(inventory_monitor.Qty) AS opp FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutes ON dispatch_sub.Route_id = modifiedroutes.Sno INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo LEFT OUTER JOIN inventory_monitor ON modifidroutssubtab.BranchID = inventory_monitor.BranchId WHERE (dispatch.sno = @routesno) GROUP BY routesno, inventory_monitor.Inv_Sno ORDER BY routesno");
                cmd = new MySqlCommand("SELECT modifiedroutes.Sno AS routesno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, inventory_monitor.Inv_Sno, SUM(inventory_monitor.Qty) AS opp, branchdata.BranchName FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutes ON dispatch_sub.Route_id = modifiedroutes.Sno INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN branchdata ON modifidroutssubtab.BranchID = branchdata.sno LEFT OUTER JOIN inventory_monitor ON modifidroutssubtab.BranchID = inventory_monitor.BranchId WHERE (dispatch.sno = @routesno) AND (branchdata.flag <> 0) GROUP BY routesno, inventory_monitor.Inv_Sno ORDER BY routesno");
                cmd.Parameters.AddWithValue("@routesno", ddlRouteName.SelectedValue);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                DataTable dtclosing = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT modifiedroutes.Sno AS routesno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, SUM(invtran.Qty) AS deliverd, invtran.B_inv_sno FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutes ON dispatch_sub.Route_id = modifiedroutes.Sno INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT TransType, FromTran, ToTran, Qty, B_inv_sno, DOE FROM invtransactions12 WHERE (TransType = 1) AND (DOE BETWEEN @d1 AND @d2) OR (TransType = 3) AND (DOE BETWEEN @d1 AND @d2)) invtran ON modifidroutssubtab.BranchID = invtran.FromTran WHERE (dispatch.sno = @routesno) GROUP BY routesno, invtran.B_inv_sno ORDER BY routesno");
                cmd.Parameters.AddWithValue("@routesno", ddlRouteName.SelectedValue);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                DateTime dtmin = GetLowDate(fromdate.AddDays(-1));
                DateTime dtmax = GetLowDate(ServerDateCurrentdate);
                cmd.Parameters.AddWithValue("@d1", dtmin.AddHours(15));
                cmd.Parameters.AddWithValue("@d2", dtmax.AddHours(15));
                DataTable dtinvcollection = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT modifiedroutes.Sno AS routesno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, SUM(invtran.Qty) AS deliverd, invtran.B_inv_sno FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutes ON dispatch_sub.Route_id = modifiedroutes.Sno INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT TransType, FromTran, ToTran, Qty, B_inv_sno, DOE FROM invtransactions12 WHERE (TransType = 1) AND (DOE BETWEEN @d1 AND @d2) OR (TransType = 2) AND (DOE BETWEEN @d1 AND @d2)) invtran ON modifidroutssubtab.BranchID = invtran.ToTran WHERE (dispatch.sno = @routesno) GROUP BY routesno, invtran.B_inv_sno ORDER BY routesno");
                cmd.Parameters.AddWithValue("@routesno", ddlRouteName.SelectedValue);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d1", dtmin.AddHours(15));
                cmd.Parameters.AddWithValue("@d2", dtmax.AddHours(15));
                DataTable dtinvdelivery = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtInventory.Rows)
                {
                    DataRow drnew = Report.NewRow();
                    int closing = 0;
                    int invdel = 0;
                    int invcoll = 0;
                    int oppening = 0;
                    drnew["Variety"] = dr["InvName"].ToString();

                    drnew["DispQty"] = dr["Qty"].ToString();
                    drnew["Returns"] = dr["Remaining"].ToString();
                    int issued = int.Parse(dr["Qty"].ToString());
                    int received = int.Parse(dr["Remaining"].ToString());
                    foreach (DataRow drclosing in dtclosing.Select("Inv_Sno='" + dr["sno"].ToString() + "'"))
                    {
                        int.TryParse(drclosing["opp"].ToString(), out closing);

                    }
                    foreach (DataRow drinvdel in dtinvdelivery.Select("B_inv_sno='" + dr["sno"].ToString() + "'"))
                    {
                        int.TryParse(drinvdel["deliverd"].ToString(), out invdel);
                    }
                    foreach (DataRow drinvcol in dtinvcollection.Select("B_inv_sno='" + dr["sno"].ToString() + "'"))
                    {
                        int.TryParse(drinvcol["deliverd"].ToString(), out invcoll);

                    }
                    oppening = (closing + received) - issued;
                    drnew["Qty"] = oppening;
                    drnew["Short"] = (oppening + issued) - received;
                    drnew["Leaks"] = issued - received;

                    Report.Rows.Add(drnew);
                }



              DataTable  Rpt = new DataTable();
                Rpt.Columns.Add("SNo");
                Rpt.Columns.Add("Agent Code");
                Rpt.Columns.Add("Agent Name");
                Rpt.Columns.Add("Crates");
                Rpt.Columns.Add("Sale Value");
                Rpt.Columns.Add("Oppening Balance");
                Rpt.Columns.Add("Amount To Be Paid");
                Rpt.Columns.Add("Paid Amount");
                Rpt.Columns.Add("Due Amount");
                Rpt.Columns.Add("Del Time");
                Rpt.Columns.Add("Pay Time");
                DataTable dtreport = new DataTable();
                dtreport.Columns.Add("Agent Code");
                dtreport.Columns.Add("Agent Name");
                dtreport.Columns.Add("Crates");
                dtreport.Columns.Add("Oppening Balance");
                dtreport.Columns.Add("Sale Value");
                dtreport.Columns.Add("Amount To Be Paid");
                dtreport.Columns.Add("Paid Amount");
                dtreport.Columns.Add("Due Amount");
                dtreport.Columns.Add("today sale");
                dtreport.Columns.Add("today collected");
                dtreport.Columns.Add("Del Time");
                dtreport.Columns.Add("Pay Time");
                dtreport.Columns.Add("SalesType");
                foreach (DataRow dr in dtbranchammount.Rows)
                {
                    DataRow newRow = dtreport.NewRow();
                    newRow["Agent Code"] = dr["BranchId"].ToString();
                    newRow["Agent Name"] = dr["BranchName"].ToString();
                    newRow["Sale Value"] = "0";
                    newRow["Amount To Be Paid"] = "0";
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
                            drbrnchamt["Del Time"] = drbrnchcollection["DelTime"].ToString();
                            drbrnchamt["Pay Time"] = drbrnchcollection["PayTime"].ToString();
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
                        newRow41["Agent Code"] = dr["salestype"].ToString();
                        Rpt.Rows.Add(newRow41);
                    }
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
                        newRow1["Sale Value"] = drbrnchamt["today sale"].ToString();
                        float salevalue = 0;
                        float.TryParse(drbrnchamt["Sale Value"].ToString(), out salevalue);
                      
                        float todaysalevalue = 0;
                        float.TryParse(drbrnchamt["today sale"].ToString(), out todaysalevalue);
                        if (dr["sno"].ToString() == "20")
                        {
                            Totdueamount += todaysalevalue;
                        }
                        if (dr["sno"].ToString() == "36")
                        {
                            Totfhamount += todaysalevalue;
                        }
                        if (dr["sno"].ToString() == "32")
                        {
                            Totcateringamount += todaysalevalue;
                        }
                        if (dr["sno"].ToString() == "37")
                        {
                            Totcompassamount += todaysalevalue;
                        }
                        if (dr["sno"].ToString() == "18")
                        {
                            Totinstituteamount += todaysalevalue;
                        }
                        if (dr["sno"].ToString() == "33")
                        {
                            Totdueagentamount += todaysalevalue;
                        }
                         if (dr["sno"].ToString() == "42")
                        {
                            TotCRagentamount += todaysalevalue;
                        }
                        
                        float paidamt = 0;
                        float.TryParse(drbrnchamt["Paid Amount"].ToString(), out paidamt);
                        float todaypaid = 0;
                        float.TryParse(drbrnchamt["today collected"].ToString(), out todaypaid);
                        float oppamt = 0;
                        float.TryParse(drbrnchamt["Oppening Balance"].ToString(), out oppamt);

                        float aopp = oppamt + paidamt - salevalue;
                        float totaldue = aopp + todaysalevalue - todaypaid;
                        if (dr["sno"].ToString() == "20")
                        {
                            newRow1["Oppening Balance"] = aopp;
                            newRow1["Amount To Be Paid"] = aopp + todaysalevalue;
                            newRow1["Paid Amount"] = Math.Round(todaypaid, 2);
                            newRow1["Due Amount"] = Math.Round(totaldue, 2);
                            newRow1["Del Time"] = drbrnchamt["Del Time"].ToString();
                            newRow1["Pay Time"] = drbrnchamt["Pay Time"].ToString();
                            Rpt.Rows.Add(newRow1);
                        }
                        else
                        {
                            newRow1["Paid Amount"] = Math.Round(todaypaid, 2);
                            newRow1["Del Time"] = drbrnchamt["Del Time"].ToString();
                            newRow1["Pay Time"] = drbrnchamt["Pay Time"].ToString();
                            Rpt.Rows.Add(newRow1);
                        }
                        k++;
                    }
                }


                //---------------------------------
                // One Grid Completed
                // 
                DataRow empty1 = Report.NewRow();
                empty1["Variety"] = "";
                empty1["Qty"] = "";
                empty1["Crates"] = "";
                empty1["DispQty"] = "";
                empty1["Returns"] = "";
                empty1["Leaks"] = "";
                empty1["Sales"] = "";
                empty1["Sales Value"] = "";
                empty1["Sales Value"] = "";
                Report.Rows.Add(empty1);
                DataRow routedueheading = Report.NewRow();
                routedueheading["Leaks"] = "DUE REPORT";
                Report.Rows.Add(routedueheading);


                //DataTable DueDetails = new DataTable();



                DataRow routedue = Report.NewRow();
                routedue["Variety"] = "SNo";
                routedue["Qty"] = "Agent Code";
                routedue["DispQty"] = "Agent Name";
                routedue["Crates"] = "Crates";
                routedue["Returns"] = "Oppening Balance";
                routedue["Leaks"] = "Sale Value";
                routedue["Short"] = "Amount To Be Paid";
                routedue["FreeMilk"] = "Paid Amount";
                routedue["Sales"] = "ToDay Due";
                routedue["Sales Value"] = "Due Amount";
                Report.Rows.Add(routedue);
                double TDAMOUNT = 0;
                double tReturns = 0;
                double TDSale = 0;
                double TDAmount = 0;
                double TDPaid = 0;
                double TDSV = 0;
                double TDTodayDue = 0;
                double Todayamount = 0;
                foreach (DataRow dr in Rpt.Rows)
                {
                    string Salestype = dr["Agent Name"].ToString();

                    DataRow drnew = Report.NewRow();
                    drnew["Variety"] = dr["SNo"].ToString();
                    drnew["Qty"] = dr["Agent Code"].ToString();
                    drnew["DispQty"] = dr["Agent Name"].ToString();
                    drnew["Crates"] = dr["Crates"].ToString();
                    drnew["Returns"] = dr["Oppening Balance"].ToString();
                    double Returns = 0;
                    double.TryParse(dr["Oppening Balance"].ToString(), out Returns);
                    tReturns += Returns;
                    drnew["Leaks"] = dr["Sale Value"].ToString();
                    double Sale = 0;
                    double.TryParse(dr["Sale Value"].ToString(), out Sale);
                    TDSale += Sale;
                    drnew["Short"] = dr["Amount To Be Paid"].ToString();
                    double Amount = 0;
                    double.TryParse(dr["Amount To Be Paid"].ToString(), out Amount);
                    TDAmount += Amount;
                    drnew["FreeMilk"] = dr["Paid Amount"].ToString();
                    double Paid = 0;
                    double.TryParse(dr["Paid Amount"].ToString(), out Paid);
                    TDPaid += Paid;
                    double todaysale = 0;
                    double todaypaid = 0;
                    double.TryParse(dr["Sale Value"].ToString(), out todaysale);
                    double.TryParse(dr["Paid Amount"].ToString(), out todaypaid);
                    TDAMOUNT += todaypaid;
                    double todaydue = todaysale - todaypaid;
                    //if (todaydue < 0)
                    //{
                    //    todaydue = 0;
                    //}
                    drnew["Sales"] = Math.Round(todaydue, 2);
                    TDTodayDue += todaydue;
                    drnew["Sales Value"] = dr["Due Amount"].ToString();
                    double DueAmount = 0;
                    double.TryParse(dr["Due Amount"].ToString(), out DueAmount);
                    if (Salestype == "AGENTS")
                    {
                        if (DueAmount > 0)
                        {
                            Todayamount += DueAmount;
                        }
                    }
                    TDSV += DueAmount;
                    double amountcount = 0;
                    if (Returns < 0)
                    {
                        Returns = 0;
                    }
                    amountcount = Sale + Amount + Paid + Returns;
                    //if (amountcount > 0)
                    //{
                    Report.Rows.Add(drnew);
                    //}
                    //if (amountcount < 0)
                    //{
                    //    Report.Rows.Add(drnew);
                    //}
                }
                DataRow newTotal = Report.NewRow();
                newTotal["Variety"] = "";
                newTotal["Qty"] = "";
                newTotal["DispQty"] = "Total";
                newTotal["Returns"] = Math.Round(tReturns, 2);
                newTotal["Leaks"] = Math.Round(TDSale, 2);
                newTotal["Short"] = Math.Round(TDAmount, 2);
                newTotal["FreeMilk"] = Math.Round(TDPaid, 2);
                newTotal["Sales"] = Math.Round(TDTodayDue, 2);
                newTotal["Sales Value"] = Math.Round(TDSV, 2);
                Report.Rows.Add(newTotal);

                DataRow space1 = Report.NewRow();
                space1["Variety"] = "";
                Report.Rows.Add(space1);

                DataRow space2 = Report.NewRow();
                space2["Variety"] = "";
                Report.Rows.Add(space2);

                DataRow space4 = Report.NewRow();
                space4["DispQty"] = "Total Sale Vale";
                space4["Returns"] = TotAmount;
                space4["Leaks"] = "Total Collected";
                float totcollectedamt = 0;
                float.TryParse(dtDenomin.Rows[0]["CollectedAmount"].ToString(), out totcollectedamt);
                space4["Short"] = totcollectedamt;
                Report.Rows.Add(space4);



                DataRow space6 = Report.NewRow();
                space6["DispQty"] = "INSTITUTIONAL";
                if (Totinstituteamount > 0)
                {
                    space6["Returns"] = Totinstituteamount;
                    //Report.Rows.Add(space6);
                }

                DataRow space7 = Report.NewRow();
                space7["DispQty"] = "DUE AGENTs";
                space7["Returns"] = Totdueagentamount;
                if (Totdueagentamount > 0)
                {
                    //Report.Rows.Add(space7);
                }

                DataRow space8 = Report.NewRow();
                space8["DispQty"] = "Comapass Group";
                space8["Returns"] = Totcompassamount;
                if (Totcompassamount > 0)
                {
                    //Report.Rows.Add(space8);
                }

                DataRow space9 = Report.NewRow();
                space9["DispQty"] = "Fresh and Honest cafe";
                if (Totfhamount > 0)
                {
                    space9["Returns"] = Totfhamount;
                    //Report.Rows.Add(space9);
                }
                DataRow space10 = Report.NewRow();
                space10["DispQty"] = "Catering Agents";
                if (Totcateringamount > 0)
                {
                    space10["Returns"] = Totcateringamount;
                    //Report.Rows.Add(space10);
                }
                double deduction = 0;
                deduction = Totinstituteamount + Totdueagentamount + Totcompassamount + Totfhamount + Totcateringamount;
                DataRow space15 = Report.NewRow();

                space15["DispQty"] = "Due Agent Sale Value";
                if (Totcateringamount > 0)
                {
                    deduction = Math.Round(deduction, 2);
                    space15["Returns"] = deduction;
                    Report.Rows.Add(space15);
                }

                DataRow space11 = Report.NewRow();
                space11["DispQty"] = "Cash Agents Sale value";
                double cashsales = 0;
                cashsales = TotAmount - deduction;
                cashsales = Math.Round(cashsales, 2);
                space11["Returns"] = cashsales;
                Report.Rows.Add(space11);


                DataRow space13 = Report.NewRow();
                space13["Leaks"] = "Total";
                double total = 0;
                total = totcollectedamt + Totdueamount + Totinstituteamount + Totcompassamount + Totcateringamount + Totfhamount + Totdueagentamount;
                total = Math.Round(total, 0);
                space13["Short"] = total;
                Report.Rows.Add(space13);
                DataRow spaceDenomination = Report.NewRow();
                spaceDenomination["Short"] = "Cash Denomination";
                Report.Rows.Add(spaceDenomination);
                  DataRow spacecash = Report.NewRow();
                spacecash["Leaks"] = "Cash";
                spacecash["Short"] = "Count";
                spacecash["FreeMilk"] = "Amount";
                Report.Rows.Add(spacecash);
                string strDenomin = dtDenomin.Rows[0]["Denominations"].ToString();
                string status = "True";
                strDenomin = strDenomin.Replace("+", " ");

                foreach (string str in strDenomin.Split(' '))
                {
                    if (str != "")
                    {
                        DataRow newden = Report.NewRow();
                        if (status == "True")
                        {
                            status = "False";
                        }
                        string[] price = str.Split('x');
                        newden["Leaks"] = price[0];
                        newden["Short"] = price[1];
                        float denamount = 0;
                        if (price[0] == "Vouchers")
                        {
                            denamount = 1;
                        }
                        else
                        {
                            float.TryParse(price[0], out denamount);
                        }
                        float DencAmount = 0;
                        float.TryParse(price[1], out DencAmount);
                        newden["FreeMilk"] = Convert.ToDecimal(denamount * DencAmount).ToString("#,##0.00");
                        denominationtotal += denamount * DencAmount;
                        Report.Rows.Add(newden);
                    }
                }
                DataRow space12 = Report.NewRow();
                space12["Leaks"] = "Total Amount";
                space12["FreeMilk"] = denominationtotal;
                Report.Rows.Add(space12);

                DataRow newrow10 = Report.NewRow();
                newrow10["DispQty"] = "Differnce";
                double Differnce = 0;
                Differnce = cashsales - denominationtotal;
                Differnce = Math.Round(Differnce, 2);
                newrow10["Returns"] = Differnce;
                Report.Rows.Add(newrow10);

                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
            }
            else
            {
                pnlHide.Visible = false;
                lblmsg.Text = "No Indent Found";
                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
        }
        catch (Exception ex)
        {
            if (ex.Message == "There is no row at position 10.")
            {
                lblmsg.Text = "No Delivery Found For This Route";
                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
            if (ex.Message == "String was not recognized as a valid DateTime.")
            {
                lblmsg.Text = "No Delivery Found For This Route";
                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
            else
            {
                lblmsg.Text = ex.Message;
                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
        }
    }
    protected void chkDispatch_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dtRoutedata = new DataTable();
        if (chkDispatch.Checked == true)
        {
            vdm = new VehicleDBMgr();
            string BranchSno = "";
             BranchSno = Session["branch"].ToString();
            cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch where  (Branch_ID = @BranchID) and (DispType='SM') and (DispMode is NULL)" );
            cmd.Parameters.AddWithValue("@BranchID", BranchSno);
            dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            ddlPlantDispName.DataSource = dtRoutedata;
            ddlPlantDispName.DataTextField = "DispName";
            ddlPlantDispName.DataValueField = "sno";
            ddlPlantDispName.DataBind();
        }
        else
        {
            ddlPlantDispName.DataSource = dtRoutedata;
            ddlPlantDispName.DataBind();
        }
    }
    void GetwyraReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            Session["filename"] = ddlRouteName.SelectedItem.Text + "Delivery" + DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            lblRoute.Text = ddlRouteName.SelectedItem.Text;
            lblDate.Text = txtdate.Text;
            vdm = new VehicleDBMgr();
            DataTable Report = new DataTable();
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
            lblDate.Text = fromdate.ToString("dd/MMM/yyyy");
            cmd = new MySqlCommand("SELECT tripdata.Sno FROM tripdata INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno WHERE (triproutes.RouteID = @DispSno) AND (tripdata.I_Date BETWEEN @d1 AND @d2)");
            if (chkDispatch.Checked == true)
            {
                cmd.Parameters.AddWithValue("@DispSno", ddlPlantDispName.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@DispSno", ddlRouteName.SelectedValue);
            }
             cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));  
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtTripdata = vdm.SelectQuery(cmd).Tables[0];
            if (dtTripdata.Rows.Count > 0)
            {
                //cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.unitQty), 2) AS unitQty, indents_subtable.Product_sno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, ROUND(SUM(indents_subtable.LeakQty), 2) AS LeakQty, indents_subtable.UnitCost, indents.IndentNo, indents.Branch_id, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS Total FROM  dispatch RIGHT OUTER JOIN branchroutesubtable INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno ON dispatch.Route_id = branchroutes.Sno LEFT OUTER JOIN indents_subtable INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo RIGHT OUTER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno ON branchdata.sno = indents.Branch_id WHERE (indents.I_date between @starttime AND  @endtime) AND (indents_subtable.DtripID = @DtripID) GROUP BY productsdata.ProductName");
                 cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.unitQty), 2) AS unitQty, indents_subtable.Product_sno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, ROUND(SUM(indents_subtable.LeakQty), 2) AS LeakQty, indents_subtable.UnitCost, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS Total FROM indents_subtable INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo RIGHT OUTER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (indents_subtable.DTripId = @DtripID) GROUP BY productsdata.ProductName");
                cmd.Parameters.AddWithValue("@DtripID", dtTripdata.Rows[0]["Sno"].ToString());
                DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT indents.Branch_id, branchdata.BranchName, collections.Branchid, collections.AmountPaid, indents_subtable.DeliveryQty * indents_subtable.UnitCost AS totalamount, collections.PaidDate, indents_subtable.D_date FROM indents INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN branchroutesubtable ON branchdata.sno = branchroutesubtable.BranchID INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno INNER JOIN collections ON indents.Branch_id = collections.Branchid INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN dispatch ON branchroutes.Sno = dispatch.Route_id WHERE (indents.I_date between @starttime AND  @endtime) AND (collections.PaidDate between @Paidstime AND  @Paidetime) AND (dispatch.sno = @dispatchSno) and (branchdata.CollectionType =@CollectionType) GROUP BY branchdata.BranchName, indents_subtable.DeliveryQty, indents_subtable.UnitCost, collections.PaidDate, indents_subtable.D_date,indents_subtable.Product_sno");
                cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
                cmd.Parameters.AddWithValue("@CollectionType", "due");
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));  
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));  
                cmd.Parameters.AddWithValue("@Paidstime", GetLowDate(fromdate));  
                cmd.Parameters.AddWithValue("@Paidetime", GetHighDate(fromdate));  
                DataTable dtBranch = vdm.SelectQuery(cmd).Tables[0];
                //cmd = new MySqlCommand("SELECT tripdata.Denominations, tripdata.Remarks, tripdata.CollectedAmount FROM tripdata INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno WHERE (triproutes.RouteID = @TripID) AND (tripdata.Status = 'P')");
                cmd = new MySqlCommand("SELECT tripdata.sno,tripdata.Denominations, tripdata.Remarks, tripdata.SubmittedAmount, tripdata.Cdate, empmanage.EmpName FROM tripdata INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN empmanage ON tripdata.EmpId = empmanage.Sno WHERE (tripdata.I_Date BETWEEN @starttime AND @endtime) AND (dispatch.sno = @dispatchSno)");
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));  
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));  
                if (chkDispatch.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@dispatchSno", ddlPlantDispName.SelectedValue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
                }
                DataTable dtDenomin = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT tripdata.sno as TripID,tripsubdata.Qty as DispQty, productsdata.ProductName,productsdata.sno FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN triproutes ON triproutes.Tripdata_sno = tripsubdata.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE  (tripdata.I_Date BETWEEN @starttime AND @endtime) AND (dispatch.sno = @dispatchsno)");
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));  
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));  
                if (chkDispatch.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@dispatchSno", ddlPlantDispName.SelectedValue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
                }
                DataTable dtSubData = vdm.SelectQuery(cmd).Tables[0];
                DataTable dtLeakages = new DataTable();
                DataTable dtInventory = new DataTable();
                if (dtSubData.Rows.Count > 0)
                {

                    string Sno = dtSubData.Rows[0]["TripID"].ToString();
                    cmd = new MySqlCommand("select LeakQty,ShortQty,FreeMilk,ProductID from Leakages where TripId=@TripId Group by ProductID order by ProductID");
                    cmd.Parameters.AddWithValue("@TripId", Sno);
                    dtLeakages = vdm.SelectQuery(cmd).Tables[0];
                    cmd = new MySqlCommand("SELECT tripinvdata.Qty, tripinvdata.Remaining, invmaster.InvName FROM tripinvdata INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (tripinvdata.Tripdata_sno = @Tripdata_sno) GROUP BY invmaster.InvName ORDER BY invmaster.sno");
                    cmd.Parameters.AddWithValue("@Tripdata_sno", Sno);
                    dtInventory = vdm.SelectQuery(cmd).Tables[0];
                }
                dtble.DefaultView.Sort = "Product_sno ASC";
                dtble = dtble.DefaultView.ToTable(true);
                if (dtble.Rows.Count > 0)
                {
                    Report = new DataTable();
                    //Report.
                    Report.Columns.Add("Variety");
                    Report.Columns.Add("Qty");
                    Report.Columns.Add("DispQty");
                    Report.Columns.Add("Returns");
                    Report.Columns.Add("Leaks");
                    Report.Columns.Add("Short");
                    Report.Columns.Add("FreeMilk");
                    Report.Columns.Add("Sales");
                    Report.Columns.Add("Sales Value");
                    Report.Columns.Add("Agent Name");
                    Report.Columns.Add("Due Amount");
                    Report.Columns.Add("Amount");
                    //Report.Columns.Add("Denomin");
                    double totalqty = 0;
                    double Leakqty = 0;
                    double tDispqty = 0;
                    double tReturnqty = 0;
                    double delqty = 0;
                    double TotAmount = 0;
                    double Totdueamount = 0;
                    int count = 0;
                    if (dtDenomin.Rows.Count > 0)
                    {

                        lblEmpName.Text = dtDenomin.Rows[0]["EmpName"].ToString();
                        string stramount = dtDenomin.Rows[0]["Denominations"].ToString();
                        foreach (string str in stramount.Split(' '))
                        {
                            count++;
                        }
                    }

                    if (dtble.Rows.Count >= count)
                    {

                        foreach (DataRow branch in dtble.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Variety"] = branch["ProductName"].ToString();
                            newrow["Qty"] = branch["unitQty"].ToString();
                            float DispQty = 0;
                            float ReturnQty = 0;
                            foreach (DataRow drSubData in dtSubData.Rows)
                            {
                                if (branch["Product_sno"].ToString() == drSubData["sno"].ToString())
                                {
                                    newrow["DispQty"] = drSubData["DispQty"].ToString();
                                    float.TryParse(drSubData["DispQty"].ToString(), out DispQty);
                                }
                            }
                            float Leaks = 0;
                            float Totqty = 0;
                            if (dtLeakages.Rows.Count > 0)
                            {

                                string ProductID = branch["Product_sno"].ToString();
                                DataRow[] drleak = dtLeakages.Select("ProductID = '" + ProductID + "'");
                                if (drleak.Length != 0)
                                {
                                    for (int i = 0; i < drleak.Length; i++)
                                    {
                                        if (branch["Product_sno"].ToString() == drleak[i][3].ToString())
                                        {
                                            string str = drleak[i][0].ToString();

                                            float Ileak = 0;
                                            float.TryParse(branch["LeakQty"].ToString(), out Ileak);
                                            float Rleak = 0;
                                            float.TryParse(str, out Rleak);
                                            Leaks = Ileak + Rleak;
                                            newrow["Leaks"] = Leaks;
                                            float ShortQty = 0;
                                            float.TryParse(drleak[i][1].ToString(), out ShortQty);
                                            newrow["Short"] = ShortQty;
                                            float FreeMilk = 0;
                                            float.TryParse(drleak[i][2].ToString(), out FreeMilk);
                                            newrow["FreeMilk"] = FreeMilk;
                                            float DeliveryQty = 0;
                                            float.TryParse(branch["DeliveryQty"].ToString(), out DeliveryQty);
                                            Totqty = Leaks + DeliveryQty + FreeMilk + ShortQty;
                                            ReturnQty = DispQty - Totqty;
                                            newrow["Returns"] = Math.Round(ReturnQty, 2);
                                        }
                                        else
                                        {
                                            newrow["Leaks"] = branch["LeakQty"].ToString();
                                            float.TryParse(branch["LeakQty"].ToString(), out Leaks);
                                            float DeliveryQty = 0;
                                            float.TryParse(branch["DeliveryQty"].ToString(), out DeliveryQty);
                                            float ShortQty = 0;
                                            newrow["Short"] = ShortQty;
                                            float FreeMilk = 0;
                                            newrow["FreeMilk"] = FreeMilk;
                                            Totqty = Leaks + DeliveryQty + FreeMilk + ShortQty;
                                            ReturnQty = DispQty - Totqty;
                                            newrow["Returns"] = Math.Round(ReturnQty, 2);
                                        }
                                        tReturnqty += Math.Round(ReturnQty, 2); ;
                                        tDispqty += DispQty;
                                        Leakqty += Leaks;
                                    }
                                }
                                else
                                {
                                    newrow["Leaks"] = branch["LeakQty"].ToString();
                                    float.TryParse(branch["LeakQty"].ToString(), out Leaks);
                                    float DeliveryQty = 0;
                                    float.TryParse(branch["DeliveryQty"].ToString(), out DeliveryQty);
                                    float ShortQty = 0;
                                    newrow["Short"] = ShortQty;
                                    float FreeMilk = 0;
                                    newrow["FreeMilk"] = FreeMilk;
                                    Totqty = Leaks + DeliveryQty + FreeMilk + ShortQty;
                                    ReturnQty = DispQty - Totqty;
                                    newrow["Returns"] = Math.Round(ReturnQty, 2);
                                    tReturnqty += Math.Round(ReturnQty, 2); ;
                                    tDispqty += DispQty;
                                    Leakqty += Leaks;
                                }
                            }
                            else
                            {
                                newrow["Leaks"] = branch["LeakQty"].ToString();
                                float.TryParse(branch["LeakQty"].ToString(), out Leaks);
                                float DeliveryQty = 0;
                                float.TryParse(branch["DeliveryQty"].ToString(), out DeliveryQty);
                                float ShortQty = 0;
                                newrow["Short"] = ShortQty;
                                float FreeMilk = 0;
                                newrow["FreeMilk"] = FreeMilk;
                                Totqty = Leaks + DeliveryQty + FreeMilk + ShortQty;
                                ReturnQty = DispQty - Totqty;
                                newrow["Returns"] = Math.Round(ReturnQty, 2);
                                tReturnqty += Math.Round(ReturnQty, 2); ;
                                tDispqty += DispQty;
                                Leakqty += Leaks;
                            }
                            newrow["Sales"] = branch["DeliveryQty"].ToString();
                            //float LeakQty=0;
                            //float.TryParse(branch["LeakQty"].ToString(), out LeakQty);

                            newrow["Sales Value"] = branch["Total"].ToString();
                            double qtyvalue = 0;
                            double Leakqtyvalue = 0;
                            double delqtyvalue = 0;
                            double TotAmountvalue = 0;
                            double.TryParse(branch["unitQty"].ToString(), out qtyvalue);
                            totalqty += Math.Round(qtyvalue, 2);
                            //double.TryParse(branch["LeakQty"].ToString(), out Leakqtyvalue);
                            //Leakqty += Leakqtyvalue;
                            double.TryParse(branch["DeliveryQty"].ToString(), out delqtyvalue);
                            delqty += Math.Round(delqtyvalue, 2);
                            double.TryParse(branch["Total"].ToString(), out TotAmountvalue);
                            TotAmount += Math.Round(TotAmountvalue, 2);
                            //newrow["Denomin"] = "100";
                            Report.Rows.Add(newrow);
                        }
                    }
                    else
                    {
                        foreach (DataRow branch in dtble.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["Variety"] = branch["ProductName"].ToString();
                            newrow["Qty"] = branch["unitQty"].ToString();
                            float DispQty = 0;
                            float ReturnQty = 0;
                            foreach (DataRow drSubData in dtSubData.Rows)
                            {
                                if (branch["Product_sno"].ToString() == drSubData["sno"].ToString())
                                {
                                    newrow["DispQty"] = drSubData["DispQty"].ToString();
                                    float.TryParse(drSubData["DispQty"].ToString(), out DispQty);
                                }
                            }
                            float Leaks = 0;
                            float Totqty = 0;
                            if (dtLeakages.Rows.Count > 0)
                            {
                                string ProductID = branch["Product_sno"].ToString();
                                DataRow[] drleak = dtLeakages.Select("ProductID = '" + ProductID + "'");
                                for (int i = 0; i < drleak.Length; i++)
                                {
                                    if (branch["Product_sno"].ToString() == drleak[i][3].ToString())
                                    {
                                        float Ileak = 0;
                                        float.TryParse(branch["LeakQty"].ToString(), out Ileak);
                                        float Rleak = 0;
                                        float.TryParse(drleak[i][0].ToString(), out Rleak);
                                        Leaks = Ileak + Rleak;
                                        newrow["Leaks"] = Leaks;
                                        float ShortQty = 0;
                                        float.TryParse(drleak[i][1].ToString(), out ShortQty);
                                        newrow["Short"] = ShortQty;
                                        float FreeMilk = 0;
                                        float.TryParse(drleak[i][2].ToString(), out FreeMilk);
                                        newrow["FreeMilk"] = FreeMilk;
                                        float DeliveryQty = 0;
                                        float.TryParse(branch["DeliveryQty"].ToString(), out DeliveryQty);
                                        Totqty = Leaks + DeliveryQty + FreeMilk + ShortQty;
                                        ReturnQty = DispQty - Totqty;
                                        newrow["Returns"] = Math.Round(ReturnQty, 2);
                                    }
                                    else
                                    {
                                        newrow["Leaks"] = branch["LeakQty"].ToString();
                                        float.TryParse(branch["LeakQty"].ToString(), out Leaks);
                                        float DeliveryQty = 0;
                                        float.TryParse(branch["DeliveryQty"].ToString(), out DeliveryQty);
                                        float ShortQty = 0;
                                        newrow["Short"] = ShortQty;
                                        float FreeMilk = 0;
                                        newrow["FreeMilk"] = FreeMilk;
                                        Totqty = Leaks + DeliveryQty + FreeMilk + ShortQty;
                                        ReturnQty = DispQty - Totqty;
                                        newrow["Returns"] = Math.Round(ReturnQty, 2);
                                    }
                                    tReturnqty += Math.Round(ReturnQty, 2); ;
                                    tDispqty += DispQty;
                                    Leakqty += Leaks;
                                }
                            }
                            else
                            {
                                newrow["Leaks"] = branch["LeakQty"].ToString();
                                float.TryParse(branch["LeakQty"].ToString(), out Leaks);
                                float DeliveryQty = 0;
                                float.TryParse(branch["DeliveryQty"].ToString(), out DeliveryQty);
                                float ShortQty = 0;
                                newrow["Short"] = ShortQty;
                                float FreeMilk = 0;
                                newrow["FreeMilk"] = FreeMilk;
                                Totqty = Leaks + DeliveryQty + FreeMilk + ShortQty;
                                ReturnQty = DispQty - Totqty;
                                newrow["Returns"] = Math.Round(ReturnQty, 2);
                                tReturnqty += Math.Round(ReturnQty, 2); ;
                                tDispqty += DispQty;
                                Leakqty += Leaks;
                            }
                            newrow["Sales"] = branch["DeliveryQty"].ToString();

                            newrow["Sales Value"] = branch["Total"].ToString();
                            double qtyvalue = 0;
                            double Leakqtyvalue = 0;
                            double delqtyvalue = 0;
                            double TotAmountvalue = 0;
                            double.TryParse(branch["unitQty"].ToString(), out qtyvalue);
                            totalqty += Math.Round(qtyvalue, 2);
                            double.TryParse(branch["DeliveryQty"].ToString(), out delqtyvalue);
                            delqty += Math.Round(delqtyvalue, 2);
                            double.TryParse(branch["Total"].ToString(), out TotAmountvalue);
                            TotAmount += Math.Round(TotAmountvalue, 2);
                            Report.Rows.Add(newrow);
                        }
                        int RowCount = count - dtble.Rows.Count;
                        for (int i = 0; i < RowCount + 1; i++)
                        {
                            DataRow newrow1 = Report.NewRow();
                            newrow1["Variety"] = "";
                            newrow1["Qty"] = "";
                            newrow1["DispQty"] = "";
                            newrow1["Returns"] = "";
                            newrow1["Leaks"] = "";
                            newrow1["Sales"] = "";
                            newrow1["Sales Value"] = "";
                            Report.Rows.Add(newrow1);
                        }
                    }


                    DataView view = new DataView(dtBranch);
                    DataTable distincttable = view.ToTable(true, "BranchName", "AmountPaid");
                    foreach (DataRow dd in distincttable.Rows)
                    {
                        string amount = dd["AmountPaid"].ToString();
                        int NagitiveAmount = 0;
                        int.TryParse(amount, out NagitiveAmount);
                        if (NagitiveAmount < 1)
                        {
                            float AmountPaid = 0;
                            float.TryParse(dd["AmountPaid"].ToString(), out AmountPaid);
                            DataRow newrow = Report.NewRow();
                            float Totamount = 0;
                            foreach (DataRow branch in dtBranch.Rows)
                            {
                                if (dd["BranchName"].ToString() == branch["BranchName"].ToString())
                                {
                                    float totalAmount = 0;
                                    float.TryParse(branch["totalAmount"].ToString(), out totalAmount);
                                    Totamount += totalAmount;
                                }
                            }
                            if (AmountPaid == Totamount)
                            {
                            }
                            else
                            {
                                AmountPaid = Math.Abs(AmountPaid);
                                float dueAmount = Totamount - AmountPaid;
                                if (dueAmount <= 1)
                                {
                                    dueAmount = 0;
                                }
                                else
                                {
                                    newrow["Agent Name"] = dd["BranchName"].ToString();
                                    Totdueamount += dueAmount;
                                    newrow["Due Amount"] = Math.Round(dueAmount, 2);
                                    Report.Rows.Add(newrow);
                                }
                            }
                        }
                    }
                    DataRow newvartical = Report.NewRow();
                    newvartical["Variety"] = "Total";
                    newvartical["Qty"] = Math.Round(totalqty, 2);
                    newvartical["DispQty"] = Math.Round(tDispqty, 2);
                    newvartical["Returns"] = Math.Round(tReturnqty, 2);
                    newvartical["Leaks"] = Math.Round(Leakqty, 2);
                    newvartical["Sales"] = Math.Round(delqty, 2);
                    newvartical["Sales Value"] = TotAmount;
                    newvartical["Due Amount"] = Math.Round(Totdueamount, 2);
                    double diffvalu = TotAmount - Math.Round(Totdueamount, 2);
                    Report.Rows.Add(newvartical);
                    if (dtDenomin.Rows.Count > 0)
                    {
                        string strDenomin = dtDenomin.Rows[0]["Denominations"].ToString();

                        string status = "True";
                        int i = 0;
                        foreach (string str in strDenomin.Split(' '))
                        {
                            if (str != "")
                            {
                                DataRow newden = Report.NewRow();
                                if (status == "True")
                                {
                                    newden["Variety"] = "Denomination";
                                    status = "False";
                                }
                                string[] price = str.Split('x');
                                Report.Rows[i][9] = price[0];
                                Report.Rows[i][10] = price[1];
                                float denamount = 0;
                                if (price[0] == "Vouchers")
                                {
                                    denamount = 1;
                                }
                                else
                                {
                                    float.TryParse(price[0], out denamount);
                                }
                                float DencAmount = 0;
                                float.TryParse(price[1], out DencAmount);
                                Report.Rows[i][11] = denamount * DencAmount;
                            }
                            i++;
                        }
                        Report.Rows[10][10] = "Total Amount";
                        Report.Rows[10][11] = dtDenomin.Rows[0]["SubmittedAmount"].ToString();
                        float densubmited = 0;
                        float.TryParse(dtDenomin.Rows[0]["SubmittedAmount"].ToString(), out densubmited);
                        diffvalu -= densubmited;
                        DataRow empty = Report.NewRow();
                        empty["Variety"] = "";
                        empty["Qty"] = "";
                        empty["DispQty"] = "";
                        empty["Returns"] = "";
                        empty["Leaks"] = "";
                        empty["Sales"] = "";
                        empty["Sales Value"] = "";
                        empty["Agent Name"] = "";
                        empty["Due Amount"] = "";
                        empty["Amount"] = "";
                        Report.Rows.Add(empty);
                        DataRow newvarticaldiff = Report.NewRow();
                        newvarticaldiff["Sales Value"] = "Difference";
                        newvarticaldiff["Agent Name"] = Math.Round(diffvalu, 2);
                        Report.Rows.Add(newvarticaldiff);

                    }
                    DataRow drnew1 = Report.NewRow();
                    drnew1["Variety"] = "Inventory";
                    drnew1["Qty"] = "Issued";
                    drnew1["DispQty"] = "Received";
                    drnew1["Returns"] = "Difference";
                    Report.Rows.Add(drnew1);
                    foreach (DataRow dr in dtInventory.Rows)
                    {
                        DataRow drnew = Report.NewRow();
                        drnew["Variety"] = dr["InvName"].ToString();
                        drnew["Qty"] = dr["Qty"].ToString();
                        drnew["DispQty"] = dr["Remaining"].ToString();
                        int issued = int.Parse(dr["Qty"].ToString());
                        int received = int.Parse(dr["Remaining"].ToString());
                        drnew["Returns"] = issued - received;
                        Report.Rows.Add(drnew);
                    }
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;
                }
                else
                {
                    pnlHide.Visible = false;
                    lblmsg.Text = "No Indent Found";
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                }
            }
            else
            {
                pnlHide.Visible = false;
                lblmsg.Text = "No DC Found";
                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
        }
        catch (Exception ex)
        {
            if (ex.Message == "String was not recognized as a valid DateTime.")
            {
                lblmsg.Text = "No Delivery Found For This Route";
            }
            else
            {
                lblmsg.Text = ex.Message;

            }
        }
    }
    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[1].Text == "CASH AGENTS")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Larger;
                e.Row.Font.Bold = true;
            }
        }
        if (e.Row.Cells[1].Text == "CATERING AGENTS")
        {
            e.Row.BackColor = System.Drawing.Color.LightCoral;
            e.Row.Font.Size = FontUnit.Larger;
            e.Row.Font.Bold = true;
        }
        if (e.Row.Cells[1].Text == "DISCONTINUED AGENTS")
        {
            e.Row.BackColor = System.Drawing.Color.Salmon;
            e.Row.Font.Size = FontUnit.Larger;
            e.Row.Font.Bold = true;
        }
        if (e.Row.Cells[1].Text == "DUE AGENTS")
        {
            e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
            e.Row.Font.Size = FontUnit.Larger;
            e.Row.Font.Bold = true;
        }
        if (e.Row.Cells[1].Text == "INSTITUTIONAL")
        {
            e.Row.BackColor = System.Drawing.Color.LightSteelBlue;
            e.Row.Font.Size = FontUnit.Larger;
            e.Row.Font.Bold = true;
        }
        if (e.Row.Cells[1].Text == "Comapass Group")
        {
            e.Row.BackColor = System.Drawing.Color.YellowGreen;
            e.Row.Font.Size = FontUnit.Larger;
            e.Row.Font.Bold = true;
        }
        if (e.Row.Cells[1].Text == "Fresh and Honest cafe")
        {
            e.Row.BackColor = System.Drawing.Color.LightBlue;
            e.Row.Font.Size = FontUnit.Larger;
            e.Row.Font.Bold = true;
        }
        if (e.Row.Cells[1].Text == "C.R Group")
        {
            e.Row.BackColor = System.Drawing.Color.LightSteelBlue;
            e.Row.Font.Size = FontUnit.Larger;
            e.Row.Font.Bold = true;
        }
        if (e.Row.Cells[1].Text == "Parlour")
        {
            e.Row.BackColor = System.Drawing.Color.SlateBlue;
            e.Row.Font.Size = FontUnit.Larger;
            e.Row.Font.Bold = true;
        }
        if (e.Row.Cells[2].Text == "Total")
        {
            e.Row.Font.Size = FontUnit.Medium;
        }
        if (e.Row.Cells[2].Text == "Total Sale Vale" || e.Row.Cells[4].Text == "Total Collected")
        {
            e.Row.Font.Size = FontUnit.Medium;
        }
        if (e.Row.Cells[1].Text == "Grand Total")
        {
            e.Row.BackColor = System.Drawing.Color.GreenYellow;
            e.Row.Font.Size = FontUnit.XLarge;
            e.Row.Font.Bold = true;
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        string TitleName = Session["TitleName"].ToString();
        //if (TitleName == "Sri Vyshnavi Dairy Specialities (P) Ltd")
        //{
        GetReport();
        //}
        //else
        //{   
        //    GetwyraReport();
        //}
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }

    protected void Print(object sender, EventArgs e)
    {
        grdReports.UseAccessibleHeader = true;
        grdReports.HeaderRow.TableSection = TableRowSection.TableHeader;
        grdReports.FooterRow.TableSection = TableRowSection.TableFooter;
        grdReports.Attributes["style"] = "border-collapse:separate";
        foreach (GridViewRow row in grdReports.Rows)
        {
            if (row.RowIndex % 10 == 0 && row.RowIndex != 0)
            {
                row.Attributes["style"] = "page-break-after:always;";
            }
        }
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        grdReports.RenderControl(hw);
        string gridHTML = sw.ToString().Replace("\"", "'").Replace(System.Environment.NewLine, "");
        StringBuilder sb = new StringBuilder();
        sb.Append("<script type = 'text/javascript'>");
        sb.Append("window.onload = new function(){");
        sb.Append("var printWin = window.open('', '', 'left=0");
        sb.Append(",top=0,width=1000,height=600,status=0');");
        sb.Append("printWin.document.write(\"");
        string style = "<style type = 'text/css'>thead {display:table-header-group;} tfoot{display:table-footer-group;}</style>";
        sb.Append(style + gridHTML);
        sb.Append("\");");
        sb.Append("printWin.document.close();");
        sb.Append("printWin.focus();");
        sb.Append("printWin.print();");
        sb.Append("printWin.close();");
        sb.Append("};");
        sb.Append("</script>");
        ClientScript.RegisterStartupScript(this.GetType(), "GridPrint", sb.ToString());
        //grdReports.AllowPaging = true;
        grdReports.DataBind();
    }
}