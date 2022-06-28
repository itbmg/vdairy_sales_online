using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class StockClosing : System.Web.UI.Page
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
                FillSalesOffice();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) OR (branchdata.sno = @BranchID)");
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
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            Report = new DataTable();
            pnlHide.Visible = true;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
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
            string SalesOfficeId = ddlSalesOffice.SelectedValue;
            if (SalesOfficeId == "472")
            {
                SalesOfficeId = "7";
            }
            cmd = new MySqlCommand("SELECT  branchroutes.Sno as RouteID,branchroutes.RouteName,branchdata.Sno as AgentCode,  branchdata.BranchName, branchaccounts.Amount, branchdata.SalesType, branchdata.CollectionType, inventory_monitor.Inv_Sno,invmaster.InvName, inventory_monitor.Qty FROM  branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN branchaccounts ON branchdata.sno = branchaccounts.BranchId INNER JOIN inventory_monitor ON branchaccounts.BranchId = inventory_monitor.BranchId INNER JOIN invmaster ON inventory_monitor.Inv_Sno = invmaster.sno WHERE (branchroutes.BranchID = @BranchID) GROUP BY branchroutes.RouteName, branchdata.BranchName,inventory_monitor.Inv_Sno");
            cmd.Parameters.AddWithValue("@BranchID", SalesOfficeId);
            DataTable DtStockInv = vdm.SelectQuery(cmd).Tables[0];
            //cmd = new MySqlCommand("SELECT productsdata.ProductName, branchdata.SalesType, branchdata.sno,branchproducts.Product_sno, branchproducts.BranchQty, branchproducts.LeakQty FROM branchdata INNER JOIN branchproducts ON branchdata.sno = branchproducts.branch_sno INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno WHERE (branchdata.sno = @BranchID) GROUP BY productsdata.ProductName order by branchproducts.Rank");
            //cmd.Parameters.AddWithValue("@BranchID", SalesOfficeId);
            //DataTable dtStockProduct = vdm.SelectQuery(cmd).Tables[0];
            //cmd = new MySqlCommand("SELECT invmaster.sno, invmaster.InvName, inventory_monitor.Qty FROM inventory_monitor INNER JOIN invmaster ON inventory_monitor.Inv_Sno = invmaster.sno WHERE (inventory_monitor.BranchId = @BranchID)");
            //cmd.Parameters.AddWithValue("@BranchID", SalesOfficeId);
            //DataTable dtStockInvProduct = vdm.SelectQuery(cmd).Tables[0];
            Report.Columns.Add("RouteName");
            Report.Columns.Add("BranchName");
            Report.Columns.Add("CollectionType");
            Report.Columns.Add("Amount").DataType = typeof(Double);
            Report.Columns.Add("CRATES").DataType = typeof(Double);
            Report.Columns.Add("CAN-10 (ltr/kgs)").DataType = typeof(Double);
            Report.Columns.Add("CAN-20 (ltr/kgs)").DataType = typeof(Double);
            Report.Columns.Add("CAN-40 (ltr/kgs)").DataType = typeof(Double);
            DataView view = new DataView(DtStockInv);
            DataTable distincttable = view.ToTable(true, "RouteID", "RouteName", "AgentCode", "BranchName", "Amount", "CollectionType");
            foreach (DataRow branch in distincttable.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["RouteName"] = branch["RouteName"].ToString();
                newrow["BranchName"] = branch["BranchName"].ToString();
                newrow["Amount"] = branch["Amount"].ToString();
                newrow["CollectionType"] = branch["CollectionType"].ToString();
                foreach (DataRow dr in DtStockInv.Rows)
                {
                    string InvName = dr["InvName"].ToString();
                    if (branch["BranchName"].ToString() == dr["BranchName"].ToString())
                    {
                        if (InvName == "CRATES")
                        {
                            newrow["CRATES"] = dr["Qty"].ToString();
                        }
                        if (InvName == "CAN-10 (ltr/kgs)")
                        {
                            newrow["CAN-10 (ltr/kgs)"] = dr["Qty"].ToString();
                        }
                        if (InvName == "CAN-20 (ltr/kgs)")
                        {
                            newrow["CAN-20 (ltr/kgs)"] = dr["Qty"].ToString();
                        }
                        if (InvName == "CAN-40 (ltr/kgs)")
                        {
                            newrow["CAN-40 (ltr/kgs)"] = dr["Qty"].ToString();
                        }
                    }
                }
                Report.Rows.Add(newrow);
            }
            DataRow newvartical = Report.NewRow();
            newvartical["BranchName"] = "Total";
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
            grdReports.DataSource = Report;
            grdReports.DataBind();
            //DataTable dtProductReport = new DataTable();
            //dtProductReport.Columns.Add("ProductName");
            //dtProductReport.Columns.Add("StockQty");
            //foreach (DataRow dr in dtStockProduct.Rows)
            //{
            //    DataRow drnew = dtProductReport.NewRow();
            //    drnew["ProductName"] = dr["ProductName"].ToString();
            //    string BranchQty = dr["BranchQty"].ToString();
            //    if (BranchQty == "" || BranchQty == "0")
            //    {
            //    }
            //    else
            //    {
            //        drnew["StockQty"] = BranchQty;
            //        dtProductReport.Rows.Add(drnew);
            //    }
            //}
            //GrdProducts.DataSource = dtProductReport;
            //GrdProducts.DataBind();
            //DataTable dtInventoryReport = new DataTable();
            //dtInventoryReport.Columns.Add("InvName");
            //dtInventoryReport.Columns.Add("StockQty");
            //foreach (DataRow dr in dtStockInvProduct.Rows)
            //{
            //    DataRow drnew = dtInventoryReport.NewRow();
            //    drnew["InvName"] = dr["InvName"].ToString();
            //    string BranchQty = dr["Qty"].ToString();
            //    if (BranchQty == "" || BranchQty == "0")
            //    {
            //    }
            //    else
            //    {
            //        drnew["StockQty"] = BranchQty;
            //        dtInventoryReport.Rows.Add(drnew);
            //    }
            //}
            //grdInventory.DataSource = dtInventoryReport;
            //grdInventory.DataBind();

            Session["xportdata"] = Report;
            Session["DtStockInv"] = DtStockInv;
            //Session["DtStockPro"] = dtStockProduct;
            //Session["dtStockInvProduct"] = dtStockInvProduct;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["DtStockInv"] == null)
            {
                lblmsg.Text = "Session Expired  ";

            }
            else
            {
                string SalesOfficeId = ddlSalesOffice.SelectedValue;
                if (SalesOfficeId == "472")
                {
                    SalesOfficeId = "7";
                }
                DataTable dtInvReport = (DataTable)Session["DtStockInv"];
                //DataTable dtProReport = (DataTable)Session["DtStockPro"];
                //DataTable dtStockInvProduct = (DataTable)Session["dtStockInvProduct"];
                foreach (DataRow dr in dtInvReport.Rows)
                {
                    try
                    {
                        cmd = new MySqlCommand("Insert into clotrans(BranchID,Amount,IndDate,SalesType,BranchType,BranchRouteID,EmpID) Values(@BranchID,@Amount,@IndDate,@SalesType,@BranchType,@BranchRouteID,@EmpID)");
                        cmd.Parameters.AddWithValue("@BranchID", dr["AgentCode"].ToString());
                        cmd.Parameters.AddWithValue("@Amount", dr["Amount"].ToString());
                        cmd.Parameters.AddWithValue("@IndDate", DateTime.Now.AddDays(-1));
                        cmd.Parameters.AddWithValue("@SalesType", 20);
                        cmd.Parameters.AddWithValue("@EmpID", Session["UserSno"].ToString());
                        cmd.Parameters.AddWithValue("@BranchType", dr["CollectionType"].ToString());
                        cmd.Parameters.AddWithValue("@BranchRouteID", dr["RouteID"].ToString());
                        long Sno = vdm.insertScalar(cmd);
                        //long Sno = 0;
                        DataRow[] drInvData = dtInvReport.Select("AgentCode=" + dr["AgentCode"].ToString());
                        if (drInvData.Count() > 0)
                        {
                            DataTable dtInv = drInvData.CopyToDataTable();
                            foreach (DataRow drv in dtInv.Rows)
                            {
                                cmd = new MySqlCommand("Insert into closubtraninventory (RefNo,InvSno,StockQty) values(@RefNo,@InvSno,@StockQty)");
                                cmd.Parameters.AddWithValue("@RefNo", Sno);
                                cmd.Parameters.AddWithValue("@InvSno", drv["Inv_Sno"].ToString());
                                cmd.Parameters.AddWithValue("@StockQty", drv["Qty"].ToString());
                                vdm.insert(cmd);
                            }
                        }

                    }
                    catch
                    {
                    }
                }
                //cmd = new MySqlCommand("Insert into clotrans(BranchID,IndDate,SalesType,EmpID) Values(@BranchID,@IndDate,@SalesType,@EmpID)");
                //cmd.Parameters.AddWithValue("@BranchID", SalesOfficeId);
                //cmd.Parameters.AddWithValue("@IndDate", DateTime.Now);
                //cmd.Parameters.AddWithValue("@SalesType", 21);
                //cmd.Parameters.AddWithValue("@EmpID", Session["UserSno"].ToString());
                //long RefSno = vdm.insertScalar(cmd);
                //foreach (DataRow drv in dtProReport.Rows)
                //{
                //    cmd = new MySqlCommand("Insert into closubtranprodcts (RefNo,ProductID,StockQty,LeakQty) values(@RefNo,@ProductID,@StockQty,@LeakQty)");
                //    cmd.Parameters.AddWithValue("@RefNo", RefSno);
                //    cmd.Parameters.AddWithValue("@ProductID", drv["Product_sno"].ToString());
                //    float BranchQty = 0;
                //    float LeakQty = 0;
                //    float.TryParse(drv["BranchQty"].ToString(), out BranchQty);
                //    float.TryParse(drv["LeakQty"].ToString(), out LeakQty);
                //    cmd.Parameters.AddWithValue("@StockQty", BranchQty);
                //    cmd.Parameters.AddWithValue("@LeakQty", LeakQty);
                //    if (BranchQty != 0 || LeakQty != 0)
                //    {
                //        vdm.insert(cmd);
                //    }
                //}
                //foreach (DataRow drv in dtStockInvProduct.Rows)
                //{
                //    cmd = new MySqlCommand("Insert into closubtraninventory (RefNo,InvSno,StockQty) values(@RefNo,@InvSno,@StockQty)");
                //    cmd.Parameters.AddWithValue("@RefNo", RefSno);
                //    int Qty = 0;
                //    int.TryParse(drv["Qty"].ToString(), out Qty);
                //    cmd.Parameters.AddWithValue("@InvSno", drv["sno"].ToString());
                //    cmd.Parameters.AddWithValue("@StockQty", Qty);
                //    if (Qty != 0)
                //    {
                //        vdm.insert(cmd);
                //    }
                //}
                lblmsg.Text = "Saved successfully";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
        }
    }
}