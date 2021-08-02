using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class LeaksAndReturns : System.Web.UI.Page
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
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
            }
        }
    }
    void FillSalesOffice()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (ddlreporttype.SelectedValue == "Dispatch Wise")
            {
                if (Session["salestype"].ToString() == "Plant")
                {
                    cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE ((Branch_Id = @BranchD) AND  (DispType=@disptype) AND  (flag=@flag)) OR ((Branch_Id = @BranchD) AND  (DispType=@disptype1) AND  (flag=@flag))");
                    cmd.Parameters.AddWithValue("@BranchD", Session["branch"].ToString());
                    cmd.Parameters.AddWithValue("@disptype", "SO");
                    cmd.Parameters.AddWithValue("@disptype1", "SM");
                    cmd.Parameters.AddWithValue("@flag", "1");
                    DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                    ddlRouteName.DataSource = dtRoutedata;
                    ddlRouteName.DataTextField = "DispName";
                    ddlRouteName.DataValueField = "sno";
                    ddlRouteName.DataBind();
                }
                else
                {
                    cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)  AND  (flag=@flag)");
                    cmd.Parameters.AddWithValue("@BranchD", Session["branch"].ToString());
                    cmd.Parameters.AddWithValue("@flag", "1");
                    DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                    ddlRouteName.DataSource = dtRoutedata;
                    ddlRouteName.DataTextField = "DispName";
                    ddlRouteName.DataValueField = "sno";
                    ddlRouteName.DataBind();
                }
            }
            else
            {
                if (Session["salestype"].ToString() == "Plant")
                {
                    cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) and (branchdata.flag=@flag) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) and (branchdata.flag=@flag) ");
                    cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                    cmd.Parameters.AddWithValue("@SalesType", "21");
                    cmd.Parameters.AddWithValue("@SalesType1", "26");
                    cmd.Parameters.AddWithValue("@flag", "1");
                    DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                    ddlRouteName.DataSource = dtRoutedata;
                    ddlRouteName.DataTextField = "BranchName";
                    ddlRouteName.DataValueField = "sno";
                    ddlRouteName.DataBind();
                }
                else
                {
                    cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND branchdata_1.flag=@flag  OR (branchdata.sno = @BranchID) AND branchdata.flag=@flag");
                    cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                    cmd.Parameters.AddWithValue("@flag", "1");
                    DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                    ddlRouteName.DataSource = dtRoutedata;
                    ddlRouteName.DataTextField = "BranchName";
                    ddlRouteName.DataValueField = "sno";
                    ddlRouteName.DataBind();
                }
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
    protected void ddlreporttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSalesOffice();
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
            lblDate.Text = Time;
            lblDispatchName.Text = ddlRouteName.SelectedItem.Text;
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            string[] fromdatestrig = txtdate.Text.Split(' ');
            if (fromdatestrig.Length > 1)
            {
                if (fromdatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = fromdatestrig[0].Split('-');
                    string[] times = fromdatestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            string[] todatestrig = txtTodate.Text.Split(' ');
            if (todatestrig.Length > 1)
            {
                if (todatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = todatestrig[0].Split('-');
                    string[] times = todatestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            #region leaks
            if (ddlType.SelectedValue == "Leaks")
            {
                lblLeak.Text = "Leaks For  " + ddlRouteName.SelectedItem.Text;
                if (ddlreporttype.SelectedValue == "Dispatch Wise")
                {
                    //cmd = new MySqlCommand("SELECT tripdata.Sno, DATE_FORMAT(leakages.EntryDate, '%d %b %y') AS Date, leakages.TotalLeaks, dispatch.DispName, leakages.ProductID, leakages.LeakQty,leakages.TripID, leakages.VarifyStatus, branchproducts.unitprice, productsdata.ProductName FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN branchproducts ON leakages.ProductID = branchproducts.product_sno AND dispatch.Branch_Id = branchproducts.branch_sno INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (dispatch.sno = @dispatchsno) AND (leakages.EntryDate BETWEEN @d1 AND @d2) AND (leakages.VarifyStatus <> 'NULL')");
                    cmd = new MySqlCommand("SELECT tripdat.Sno, DATE_FORMAT(tripdat.I_Date, '%d %b %y') AS Date, leakages.TotalLeaks, dispatch.DispName, leakages.ProductID, leakages.LeakQty, leakages.TripID,leakages.VarifyStatus, branchproducts.unitprice, productsdata.ProductName FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN leakages ON tripdat.Sno = leakages.TripID INNER JOIN branchproducts ON leakages.ProductID = branchproducts.product_sno AND dispatch.Branch_Id = branchproducts.branch_sno INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (dispatch.sno = @dispatchsno) AND (leakages.VarifyStatus <> 'NULL')");
                    cmd.Parameters.AddWithValue("@dispatchsno", ddlRouteName.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                }
                else
                {
                    ////cmd = new MySqlCommand("SELECT tripdat.Sno, DATE_FORMAT(tripdat.I_Date, '%d %b %y') AS DATE, leakages.TotalLeaks, productsdata.UnitPrice, productsdata.ProductName, leakages.ProductID,leakages.LeakQty, leakages.TripID, leakages.VarifyStatus FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN leakages ON tripdat.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (leakages.VarifyStatus <> 'NULL') AND (dispatch.BranchID = @sobranchid)"); 
                    //Ravindra 30/01/2017
                    cmd = new MySqlCommand("SELECT Leaks.TotalLeaks,leaks.productsno, Leaks.UnitPrice, Leaks.ProductName,DATE_FORMAT(ff.I_Date, '%d %b %y') AS DATE,  ff.DispName, Leaks.Sno AS Sno FROM (SELECT leakages.TotalLeaks,productsdata.UnitPrice, productsdata.ProductName,productsdata.sno AS productsno, tripdata_1.Sno FROM            tripdata tripdata_1 INNER JOIN leakages ON tripdata_1.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE        (leakages.VarifyStatus <> 'NULL') AND (tripdata_1.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT  DispName, Sno, DespSno, I_Date FROM  (SELECT  dispatch.DispName, tripdata.Sno, dispatch.sno AS DespSno, tripdata.I_Date FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN  triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @BranchID)) TripInfo) ff ON ff.Sno = Leaks.Sno");
                    cmd.Parameters.AddWithValue("@BranchID", ddlRouteName.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                }
                // cmd = new MySqlCommand("SELECT tripdata.Sno,DATE_FORMAT(leakages.EntryDate, '%d %b %y') as Date, leakages.TotalLeaks,productsdata.UnitPrice, productsdata.ProductName, dispatch.DispName, leakages.ProductID  FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE  (dispatch.sno = @dispatchsno) AND (leakages.EntryDate between @d1 and @d2) Group by leakages.EntryDate, productsdata.ProductName");

                DataTable dtLeaks = vdm.SelectQuery(cmd).Tables[0];

                Report = new DataTable();
                Report.Columns.Add("Date");
                DataView view = new DataView(dtLeaks);
                DataTable distinctProduct = view.ToTable(true, "ProductName", "productsno");
                foreach (DataRow dr in distinctProduct.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                }
                Report.Columns.Add("Total  Leaks").DataType = typeof(Double);
                Report.Columns.Add("Despatch Qty").DataType = typeof(Double);
                Report.Columns.Add("Leaks %");
                //Report.Columns.Add("Total  Amount").DataType = typeof(float);
                DataTable distincttable = view.ToTable(true, "Date");
                int i = 1;
                double Tot_leaks = 0;
                double TotldespQty = 0;
                DataTable dtproductdispatch = new DataTable();
                DataTable dtproductdispatch1 = new DataTable();
               
                double totalqty = 0;
                cmd = new MySqlCommand("SELECT  t2.ProductId,ROUND(SUM(t2.Qty),2) AS QTY,t2.ProductName,  t1.Tripdata_sno, t1.Tripdata_sno AS Expr1, t1.AssignDate, t1.BranchID FROM (SELECT triproutes.Tripdata_sno, tripdata.AssignDate, dispatch.BranchID, tripdata.Sno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @branchid)) t1 INNER JOIN (SELECT tripsubdata.ProductId, tripsubdata.Qty, productsdata.ProductName, tripdata_1.Sno FROM tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE (tripdata_1.AssignDate BETWEEN @d1 AND @d2) AND (tripdata_1.Status <> 'C') GROUP BY tripsubdata.ProductId, tripdata_1.Sno) t2 ON t1.Tripdata_sno = t2.Sno GROUP BY t2.ProductId");
                cmd.Parameters.AddWithValue("@BranchID", ddlRouteName.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                dtproductdispatch = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow branch in distincttable.Rows)
                {
                    double Totleaks = 0;
                    DataRow newrow = Report.NewRow();
                    string IndentDate = branch["Date"].ToString();
                    DateTime dtIndentDate = Convert.ToDateTime(IndentDate);
                    cmd = new MySqlCommand("SELECT t2.ProductId, ROUND(SUM(t2.Qty), 2) AS disqty, t1.BranchID FROM (SELECT triproutes.Tripdata_sno, tripdata.AssignDate, dispatch.BranchID, tripdata.Sno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE        (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @branchid)) t1 INNER JOIN (SELECT        tripsubdata.ProductId, tripsubdata.Qty, productsdata.ProductName, tripdata_1.Sno FROM tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE      (tripdata_1.AssignDate BETWEEN @d1 AND @d2) AND (tripdata_1.Status <> 'C') GROUP BY tripsubdata.ProductId, tripdata_1.Sno) t2 ON t1.Tripdata_sno = t2.Sno");
                    //cmd = new MySqlCommand("SELm\ECT  SUM(tripsubdata.Qty) AS disqty FROM  dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripsubdata ON triproutes.Tripdata_sno = tripsubdata.Tripdata_sno INNER JOIN tripdata ON tripsubdata.Tripdata_sno = tripdata.Sno INNER JOIN (SELECT  product_sno FROM branchproducts  WHERE (branch_sno = @Branchid)) brnchprdts ON tripsubdata.ProductId = brnchprdts.product_sno WHERE (dispatch.BranchID = @Branchid) AND (tripdata.AssignDate BETWEEN @d1 AND @d2)");
                    cmd.Parameters.AddWithValue("@Branchid", ddlRouteName.SelectedValue);
                   // cmd.Parameters.AddWithValue("@TripID", branch["Sno"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(dtIndentDate));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(dtIndentDate));
                    DataTable dtdispatch = vdm.SelectQuery(cmd).Tables[0];
                   
                    string ChangedTime = dtIndentDate.AddDays(1).ToString("dd/MMM/yyyy");
                    newrow["Date"] = ChangedTime;
                    //newrow["Branch Name"] = branch["BranchName"].ToString();
                    double Total = 0;
                    foreach (DataRow dr in dtLeaks.Select("Date='" + branch["Date"].ToString() + "'"))
                    {
                        if (branch["Date"].ToString() == dr["Date"].ToString())
                        {
                            //if (dr["DeliveryQty"].ToString() != "")
                            //{
                            double TotalLeaks = 0;
                            double.TryParse(dr["TotalLeaks"].ToString(), out TotalLeaks);
                            double UnitCost = 0;
                            double.TryParse(dr["UnitPrice"].ToString(), out UnitCost);
                            if (TotalLeaks == 0)
                            {
                            }
                            else
                            {
                                newrow[dr["ProductName"].ToString()] = TotalLeaks;
                                Total += TotalLeaks * UnitCost;
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
                lblMessage.Text = "Total Leak % = " + Tot_per.ToString("F2");
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

            #region Returns
            if (ddlType.SelectedValue == "Returns")
            {
                lblLeak.Text = "Returns For  " + ddlRouteName.SelectedItem.Text;
                if (ddlreporttype.SelectedValue == "Dispatch Wise")
                {
                    cmd = new MySqlCommand("SELECT tripdata.Sno,DATE_FORMAT(leakages.EntryDate, '%d %b %y') as Date, leakages.ReturnQty,productsdata.UnitPrice, productsdata.ProductName, dispatch.DispName, leakages.ProductID  FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE  (dispatch.sno = @dispatchsno) AND (leakages.EntryDate between @d1 and @d2) AND (leakages.VarifyStatus <> 'NULL')");
                    cmd.Parameters.AddWithValue("@dispatchsno", ddlRouteName.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                }
                else
                {
                    cmd = new MySqlCommand("SELECT tripdata.Sno, DATE_FORMAT(leakages.EntryDate,'%d %b %y') as Date, leakages.ReturnQty, productsdata.UnitPrice, productsdata.ProductName, leakages.ProductID FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (leakages.EntryDate BETWEEN @d1 AND @d2) AND (leakages.VarifyStatus <> 'NULL') AND (dispatch.BranchID = @sobranchid) AND (leakages.ReturnQty <> 0)");
                    cmd.Parameters.AddWithValue("@sobranchid", ddlRouteName.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));

                }

                DataTable dtLeaks = vdm.SelectQuery(cmd).Tables[0];
                Report = new DataTable();
                Report.Columns.Add("Date");
                DataView view = new DataView(dtLeaks);
                DataTable distinctProduct = view.ToTable(true, "ProductName");
                foreach (DataRow dr in distinctProduct.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                }
                Report.Columns.Add("Total  Returns").DataType = typeof(Double);
                Report.Columns.Add("Total  Average Returns For Total Dispatch");
                Report.Columns.Add("Total  Amount").DataType = typeof(float);
                DataTable distincttable = view.ToTable(true, "Date", "Sno");
                int i = 1;
                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    cmd = new MySqlCommand("SELECT Tripdata_sno, ProductId, SUM(Qty) AS disqty, DeliverQty, Status, Price FROM tripsubdata WHERE (Tripdata_sno = @tripdatasno)");
                    cmd.Parameters.AddWithValue("@tripdatasno", branch["Sno"].ToString());
                    DataTable dtdispatch = vdm.SelectQuery(cmd).Tables[0];
                    string IndentDate = branch["Date"].ToString();
                    DateTime dtIndentDate = Convert.ToDateTime(IndentDate);
                    string ChangedTime = dtIndentDate.ToString("dd/MMM/yyyy");
                    newrow["Date"] = ChangedTime;
                    //newrow["Branch Name"] = branch["BranchName"].ToString();
                    double Totreturn = 0;
                    double Total = 0;
                    foreach (DataRow dr in dtLeaks.Rows)
                    {
                        if (branch["Date"].ToString() == dr["Date"].ToString())
                        {
                            //if (dr["DeliveryQty"].ToString() != "")
                            //{
                            double ReturnQty = 0;
                            double.TryParse(dr["ReturnQty"].ToString(), out ReturnQty);
                            double UnitCost = 0;
                            double.TryParse(dr["UnitPrice"].ToString(), out UnitCost);
                            newrow[dr["ProductName"].ToString()] = ReturnQty;
                            Total += ReturnQty * UnitCost;
                            Totreturn += ReturnQty;
                            //}
                        }
                    }
                    newrow["Total  Returns"] = Totreturn;
                    double avg = 0;

                    if (dtdispatch.Rows.Count > 0)
                    {
                        double dispqty = 0;
                        double.TryParse(dtdispatch.Rows[0]["disqty"].ToString(), out dispqty);
                        avg = Totreturn / dispqty;
                        newrow["Total  Average Returns For Total Dispatch"] = Math.Round(avg, 2);


                    }
                    else
                    {
                        //avg = 0;
                        newrow["Total  Average Returns For Total Dispatch"] = avg;

                    }
                    newrow["Total  Amount"] = Total;
                    Report.Rows.Add(newrow);
                }
                DataRow newvartical = Report.NewRow();
                newvartical["Date"] = "Total";
                float val = 0;
                float.TryParse(Report.Compute("sum([Total  Amount])", "[Total  Amount]<>'0'").ToString(), out val);
                newvartical["Total  Amount"] = val;
                double val1 = 0;
                foreach (DataColumn dc in Report.Columns)
                {
                    if (dc.DataType == typeof(Double))
                    {
                        val1 = 0.0;
                        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val1);
                        newvartical[dc.ToString()] = val1;
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

            #region Puff Leaks

            if (ddlType.SelectedValue == "Puff Leaks")
            {
                lblLeak.Text = "Puff Leaks For  " + ddlRouteName.SelectedItem.Text;
                if (ddlreporttype.SelectedValue == "Dispatch Wise")
                {
                    
                }
                else
                {
                    //cmd = new MySqlCommand("SELECT tripdata.Sno, DATE_FORMAT(leakages.EntryDate, '%d %b %y') AS DATE, leakages.TotalLeaks, productsdata.UnitPrice, productsdata.ProductName, leakages.ProductID, leakages.LeakQty, leakages.TripID,leakages.VarifyStatus FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (leakages.EntryDate BETWEEN @d1 AND @d2) AND (leakages.VarifyStatus <> 'NULL') AND (dispatch.BranchID = @sobranchid)");
                    cmd = new MySqlCommand("SELECT tripdat.Sno, DATE_FORMAT(tripdat.I_Date, '%d %b %y') AS DATE, productsdata.UnitPrice, productsdata.ProductName, SUM(branchleaktrans.LeakQty) AS TotalLeaks FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN branchleaktrans ON tripdat.Sno = branchleaktrans.TripId INNER JOIN productsdata ON branchleaktrans.ProdId = productsdata.sno WHERE (dispatch.BranchID = @sobranchid) GROUP BY tripdat.I_Date, branchleaktrans.ProdId");
                    cmd.Parameters.AddWithValue("@sobranchid", ddlRouteName.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                }
                // cmd = new MySqlCommand("SELECT tripdata.Sno,DATE_FORMAT(leakages.EntryDate, '%d %b %y') as Date, leakages.TotalLeaks,productsdata.UnitPrice, productsdata.ProductName, dispatch.DispName, leakages.ProductID  FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE  (dispatch.sno = @dispatchsno) AND (leakages.EntryDate between @d1 and @d2) Group by leakages.EntryDate, productsdata.ProductName");

                DataTable dtLeaks = vdm.SelectQuery(cmd).Tables[0];
                Report = new DataTable();
                Report.Columns.Add("Date");
                DataView view = new DataView(dtLeaks);
                DataTable distinctProduct = view.ToTable(true, "ProductName");
                foreach (DataRow dr in distinctProduct.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                }
                Report.Columns.Add("Total  Leaks").DataType = typeof(Double);
                Report.Columns.Add("Total  Average Leaks For Total Dispatch");
                Report.Columns.Add("Total  Amount").DataType = typeof(float);
                DataTable distincttable = view.ToTable(true, "Date");
                int i = 1;
                foreach (DataRow branch in distincttable.Rows)
                {
                    string IndentDate = branch["Date"].ToString();
                    DateTime dtIndentDate = Convert.ToDateTime(IndentDate);
                    DataRow newrow = Report.NewRow();
                    cmd = new MySqlCommand("SELECT tripdat.Sno, DATE_FORMAT(tripdat.I_Date, '%d %b %y') AS DATE, tripsubdata.ProductId, SUM(tripsubdata.Qty) AS disqty FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno WHERE (dispatch.BranchID = @sobranchid)");
                    cmd.Parameters.AddWithValue("@sobranchid", ddlRouteName.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(dtIndentDate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(dtIndentDate.AddDays(-1)));
                    DataTable dtdispatch = vdm.SelectQuery(cmd).Tables[0];
                    string ChangedTime = dtIndentDate.AddDays(1).ToString("dd/MMM/yyyy");
                    newrow["Date"] = ChangedTime;
                    //newrow["Branch Name"] = branch["BranchName"].ToString();
                    double Totleaks = 0;
                    double Total = 0;
                    foreach (DataRow dr in dtLeaks.Rows)
                    {
                        if (branch["Date"].ToString() == dr["Date"].ToString())
                        {
                            //if (dr["DeliveryQty"].ToString() != "")
                            //{
                            double TotalLeaks = 0;
                            double.TryParse(dr["TotalLeaks"].ToString(), out TotalLeaks);
                            double UnitCost = 0;
                            double.TryParse(dr["UnitPrice"].ToString(), out UnitCost);
                            newrow[dr["ProductName"].ToString()] = TotalLeaks;
                            Total += TotalLeaks * UnitCost;
                            Totleaks += TotalLeaks;

                            //}
                        }
                    }
                    newrow["Total  Leaks"] = Totleaks;
                    double avg = 0;

                    if (dtdispatch.Rows.Count > 0)
                    {
                        double dispqty = 0;
                        double.TryParse(dtdispatch.Rows[0]["disqty"].ToString(), out dispqty);
                        avg = (Totleaks / dispqty) * 100;
                        newrow["Total  Average Leaks For Total Dispatch"] = Math.Round(avg, 2);
                    }
                    else
                    {
                        //avg = 0;
                        newrow["Total  Average Leaks For Total Dispatch"] = avg;
                    }
                    newrow["Total  Amount"] = Total;
                    Report.Rows.Add(newrow);
                }
                DataRow newvartical = Report.NewRow();
                newvartical["Date"] = "Total";
                float val = 0;
                float.TryParse(Report.Compute("sum([Total  Amount])", "[Total  Amount]<>'0'").ToString(), out val);
                newvartical["Total  Amount"] = val;
                double val1 = 0;
                foreach (DataColumn dc in Report.Columns)
                {
                    if (dc.DataType == typeof(Double))
                    {
                        val1 = 0.0;
                        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val1);
                        newvartical[dc.ToString()] = val1;
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

            #region Free
            if (ddlType.SelectedValue == "Free")
            {
                lblLeak.Text = "Free For  " + ddlRouteName.SelectedItem.Text;
                if (ddlreporttype.SelectedValue == "Dispatch Wise")
                {
                    ////cmd = new MySqlCommand("SELECT tripdata.Sno,DATE_FORMAT(leakages.EntryDate, '%d %b %y') as Date, leakages.ReturnQty,productsdata.UnitPrice, productsdata.ProductName, dispatch.DispName, leakages.ProductID  FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE  (dispatch.sno = @dispatchsno) AND (leakages.EntryDate between @d1 and @d2) AND (leakages.VarifyStatus <> 'NULL')");
                    ////cmd.Parameters.AddWithValue("@dispatchsno", ddlRouteName.SelectedValue);
                    ////cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                    ////cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                }
                else
                {
                    cmd = new MySqlCommand("SELECT  Leaks.productsno, Leaks.UnitPrice, Leaks.ProductName,DATE_FORMAT(ff.I_Date,'%d %b %y') as Date , ff.DispName, Leaks.TripID, Leaks.FreeMilk FROM  (SELECT  leakages.TotalLeaks, productsdata.UnitPrice, productsdata.ProductName, productsdata.sno AS productsno, leakages.TripID, leakages.FreeMilk FROM   tripdata tripdata_1 INNER JOIN  leakages ON tripdata_1.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (leakages.FreeMilk <> 0) AND (tripdata_1.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno, I_Date FROM (SELECT dispatch.DispName, tripdata.Sno, dispatch.sno AS DespSno, tripdata.I_Date FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @sobranchid)) TripInfo) ff ON ff.Sno = Leaks.TripID");
                    //cmd = new MySqlCommand("SELECT tripdata.Sno, DATE_FORMAT(tripdata.I_date,'%d %b %y') as Date, ROUND(SUM(leakages.FreeMilk),2) as FreeMilk, productsdata.UnitPrice, productsdata.ProductName, leakages.ProductID FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_date BETWEEN @d1 AND @d2) AND  (dispatch.Branch_ID = @sobranchid) AND (leakages.FreeMilk <> 0) group by productsdata.ProductName,Date");
                    cmd.Parameters.AddWithValue("@sobranchid", ddlRouteName.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                }
                DataTable dtLeaks = vdm.SelectQuery(cmd).Tables[0];
                Report = new DataTable();
                Report.Columns.Add("Date");
                DataView view = new DataView(dtLeaks);
                DataTable distinctProduct = view.ToTable(true, "ProductName");
                foreach (DataRow dr in distinctProduct.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                }
                Report.Columns.Add("Total  FreeMilk").DataType = typeof(Double);
                //Report.Columns.Add("Total  Average Returns For Total Dispatch");
                Report.Columns.Add("Total  Amount").DataType = typeof(float);
                DataTable distincttable = view.ToTable(true, "Date");
                int i = 1;
                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    string IndentDate = branch["Date"].ToString();
                    DateTime dtIndentDate = Convert.ToDateTime(IndentDate).AddDays(1);
                    string ChangedTime = dtIndentDate.ToString("dd/MMM/yyyy");
                    newrow["Date"] = ChangedTime;
                    //newrow["Branch Name"] = branch["BranchName"].ToString();
                    double Totreturn = 0;
                    double Total = 0;
                    foreach (DataRow dr in dtLeaks.Rows)
                    {
                        if (branch["Date"].ToString() == dr["Date"].ToString())
                        {
                            double FreeMilk = 0;
                            double.TryParse(dr["FreeMilk"].ToString(), out FreeMilk);
                            double UnitCost = 0;
                            double.TryParse(dr["UnitPrice"].ToString(), out UnitCost);
                            newrow[dr["ProductName"].ToString()] = FreeMilk;
                            Total += FreeMilk * UnitCost;
                            Totreturn += FreeMilk;
                        }
                    }
                    newrow["Total  FreeMilk"] = Totreturn;
                    double avg = 0;
                    newrow["Total  Amount"] = Total;
                    Report.Rows.Add(newrow);
                }
                DataRow newvartical = Report.NewRow();
                newvartical["Date"] = "Total";
                float val = 0;
                float.TryParse(Report.Compute("sum([Total  Amount])", "[Total  Amount]<>'0'").ToString(), out val);
                newvartical["Total  Amount"] = val;
                double val1 = 0;
                foreach (DataColumn dc in Report.Columns)
                {
                    if (dc.DataType == typeof(Double))
                    {
                        val1 = 0.0;
                        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val1);
                        newvartical[dc.ToString()] = val1;
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

            #region Shorts
            if (ddlType.SelectedValue == "Shorts")
            {
                lblLeak.Text = "Shorts For  " + ddlRouteName.SelectedItem.Text;
                if (ddlreporttype.SelectedValue == "Dispatch Wise")
                {
                    ////cmd = new MySqlCommand("SELECT tripdata.Sno,DATE_FORMAT(leakages.EntryDate, '%d %b %y') as Date, leakages.ReturnQty,productsdata.UnitPrice, productsdata.ProductName, dispatch.DispName, leakages.ProductID  FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE  (dispatch.sno = @dispatchsno) AND (leakages.EntryDate between @d1 and @d2) AND (leakages.VarifyStatus <> 'NULL')");
                    ////cmd.Parameters.AddWithValue("@dispatchsno", ddlRouteName.SelectedValue);
                    ////cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                    ////cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                }
                else
                {
                    cmd = new MySqlCommand("SELECT  Leaks.productsno, Leaks.UnitPrice, Leaks.ProductName,DATE_FORMAT(ff.I_Date,'%d %b %y') as Date , ff.DispName, Leaks.TripID , Leaks.ShortQty FROM  (SELECT   productsdata.UnitPrice, productsdata.ProductName, productsdata.sno AS productsno, leakages.TripID, leakages.ShortQty FROM   tripdata tripdata_1 INNER JOIN  leakages ON tripdata_1.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (leakages.ShortQty <> 0) AND (tripdata_1.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno, I_Date FROM (SELECT dispatch.DispName, tripdata.Sno, dispatch.sno AS DespSno, tripdata.I_Date FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @sobranchid)) TripInfo) ff ON ff.Sno = Leaks.TripID");
                    //cmd = new MySqlCommand("SELECT tripdata.Sno, DATE_FORMAT(tripdata.I_date,'%d %b %y') as Date, ROUND(SUM(leakages.shortqty),2) as shortqty, productsdata.UnitPrice, productsdata.ProductName, leakages.ProductID FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_date BETWEEN @d1 AND @d2) AND  (dispatch.Branch_ID = @sobranchid) AND (leakages.shortqty <> 0) group by Date,productsdata.ProductName");
                    cmd.Parameters.AddWithValue("@sobranchid", ddlRouteName.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                }
                DataTable dtAll = new DataTable();
                DataTable dtLeaks = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT branchleaktrans.EmpId,DATE_FORMAT(tripdata.I_date,'%d %b %y') as Date, branchleaktrans.TripId AS TripID, branchleaktrans.ProdId,  branchleaktrans.DOE, branchleaktrans.BranchID,branchleaktrans.Status,  branchleaktrans.ShortQty, tripdata.I_Date, branchproducts.Rank, productsdata.ProductName,productsdata.UnitPrice FROM branchleaktrans INNER JOIN tripdata ON branchleaktrans.TripId = tripdata.Sno INNER JOIN branchproducts ON branchleaktrans.BranchID = branchproducts.branch_sno AND branchleaktrans.ProdId = branchproducts.product_sno INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchleaktrans.BranchID = @BranchID) AND branchleaktrans.ShortQty<>0 GROUP BY tripdata.I_Date, branchleaktrans.ProdId ORDER BY TripID ");
                cmd.Parameters.AddWithValue("@BranchID", ddlRouteName.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                DataTable dtsalesofficeLeaks = vdm.SelectQuery(cmd).Tables[0];

                dtAll.Merge(dtLeaks);
                dtAll.Merge(dtsalesofficeLeaks);


                Report = new DataTable();
                Report.Columns.Add("Date");
                DataView view = new DataView(dtAll);
                DataTable distinctProduct = view.ToTable(true, "ProductName");
                foreach (DataRow dr in distinctProduct.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                }
                Report.Columns.Add("Total  Shortqty").DataType = typeof(Double);
               // Report.Columns.Add("Total  Average Returns For Total Dispatch");
                Report.Columns.Add("Total  Amount").DataType = typeof(float);
                DataTable distincttable = view.ToTable(true, "Date");
                DataView dv = distincttable.DefaultView;
                dv.Sort = "Date ASC";
                DataTable sortedProductDT = dv.ToTable();
                int i = 1;
                foreach (DataRow branch in sortedProductDT.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    string IndentDate = branch["Date"].ToString();
                    DateTime dtIndentDate = Convert.ToDateTime(IndentDate).AddDays(1);
                    string ChangedTime = dtIndentDate.ToString("dd/MMM/yyyy");
                    newrow["Date"] = ChangedTime;
                    double Totreturn = 0;
                    double Total = 0;
                    double totqty = 0;
                    foreach (DataRow dr in dtAll.Rows)
                    {
                        if (branch["Date"].ToString() == dr["Date"].ToString())
                        {
                            double Shortqty = 0;
                            double.TryParse(dr["ShortQty"].ToString(), out Shortqty);
                            double UnitCost = 0;
                            double.TryParse(dr["UnitPrice"].ToString(), out UnitCost);
                            totqty += Shortqty;
                            newrow[dr["ProductName"].ToString()] = totqty;
                            Total += totqty * UnitCost;
                        }
                    }
                    newrow["Total  Shortqty"] = totqty;
                    double avg = 0;
                    ////if (dtdispatch.Rows.Count > 0)
                    ////{
                    ////    double dispqty = 0;
                    ////    double.TryParse(dtdispatch.Rows[0]["disqty"].ToString(), out dispqty);
                    ////    avg = Totreturn / dispqty;
                    ////    newrow["Total  Average Returns For Total Dispatch"] = Math.Round(avg, 2);
                    ////}
                    ////else
                    ////{
                    ////    //avg = 0;
                    ////    newrow["Total  Average Returns For Total Dispatch"] = avg;

                    ////}
                    newrow["Total  Amount"] = Total;
                    Report.Rows.Add(newrow);
                }
                DataRow newvartical = Report.NewRow();
                newvartical["Date"] = "Total";
                float val = 0;
                float.TryParse(Report.Compute("sum([Total  Amount])", "[Total  Amount]<>'0'").ToString(), out val);
                newvartical["Total  Amount"] = val;
                double val1 = 0;
                foreach (DataColumn dc in Report.Columns)
                {
                    if (dc.DataType == typeof(Double))
                    {
                        val1 = 0.0;
                        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val1);
                        newvartical[dc.ToString()] = val1;
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
        catch
        {
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
