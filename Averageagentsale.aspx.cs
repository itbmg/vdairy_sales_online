using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class Averageagentsale : System.Web.UI.Page
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
        //UserName = Session["field1"].ToString();
        //vdm = new VehicleDBMgr();
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                lblTitle.Text = Session["TitleName"].ToString();
                FillSalesOffice();
            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    void FillSalesOffice()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Plant")
            {
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
    string routeid = "";
    string routeitype = "";
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            Report = new DataTable();
            Session["RouteName"] = ddlSalesOffice.SelectedItem.Text;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            string[] dateFromstrig = txtFromdate.Text.Split(' ');
            if (dateFromstrig.Length > 1)
            {
                if (dateFromstrig[0].Split('-').Length > 0)
                {
                    string[] dates = dateFromstrig[0].Split('-');
                    string[] times = dateFromstrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            string[] datetostrig = txtTodate.Text.Split(' ');
            if (datetostrig.Length > 1)
            {
                if (datetostrig[0].Split('-').Length > 0)
                {
                    string[] dates = datetostrig[0].Split('-');
                    string[] times = datetostrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            lbl_selfromdate.Text = fromdate.ToString("dd/MM/yyyy");
            lbl_selttodate.Text = todate.ToString("dd/MM/yyyy");
            lblRoutName.Text = ddlSalesOffice.SelectedItem.Text;
            Session["filename"] = "AGENT WISE DELIVERY REPORT";
            TimeSpan dateSpan = todate.Subtract(fromdate);
            int NoOfdays = dateSpan.Days;
            NoOfdays = NoOfdays + 1;
            //cmd = new MySqlCommand("select Route_id,IndentType from dispatch_sub where dispatch_sno=@dispsno");
            //cmd.Parameters.AddWithValue("@dispsno", ddlRouteName.SelectedValue);
            //DataTable dtrouteindenttype = vdm.SelectQuery(cmd).Tables[0];
            //foreach (DataRow drrouteitype in dtrouteindenttype.Rows)
            //{
            //    routeid = drrouteitype["Route_id"].ToString();
            //    routeitype = drrouteitype["IndentType"].ToString();
            //}
            // cmd = new MySqlCommand("SELECT branchroutes.RouteName,branchproducts.Rank, ROUND(SUM(indents_subtable.DeliveryQty),2) AS DeliveryQty,indents_subtable.UnitCost, indents.IndentType, productsdata.ProductName,branchdata.sno as BSno, branchdata.BranchName,productsdata.Units, productsdata.sno,products_category.Categoryname, invmaster.Qty FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN indents ON branchdata.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON products_subcategory.sno = productsdata.SubCat_sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno WHERE (branchroutes.Sno = @TripID) AND (indents.I_date BETWEEN @starttime AND @endtime) AND (indents.Status <> 'D')  and (branchproducts.branch_sno=@BranchID) GROUP BY indents_subtable.UnitCost, branchdata.BranchName, productsdata.sno, products_category.Categoryname ORDER BY branchproducts.Rank");
            ////cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, brnchprdt.Rank, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.UnitCost, indent.IndentType,productsdata.ProductName, branchdata.sno AS BSno, branchdata.BranchName, productsdata.Units, productsdata.sno, products_category.Categoryname,invmaster.Qty FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, Status, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN  products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN (SELECT branch_sno, product_sno, unitprice, flag, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno WHERE (modifiedroutes.Sno = @TripID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (modifiedroutes.Sno = @TripID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY indents_subtable.UnitCost, branchdata.BranchName, productsdata.sno, products_category.Categoryname ORDER BY brnchprdt.Rank");
            cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, brnchprdt.Rank, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.UnitCost, indent.IndentType, productsdata.ProductName, branchdata.sno AS BSno, branchdata.BranchName, productsdata.Units, productsdata.sno, products_category.Categoryname, invmaster.Qty FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, Status, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN (SELECT        branch_sno, product_sno, unitprice, flag, Rank FROM  branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN  invmaster ON productsdata.Inventorysno = invmaster.sno WHERE (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) AND (modifiedroutes.BranchID = @TripID) OR (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (modifiedroutes.BranchID = @TripID) GROUP BY branchdata.BranchName, productsdata.sno, products_category.Categoryname ORDER BY modifiedroutes.RouteName");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@TripID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(todate.AddDays(-1)));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            //cmd = new MySqlCommand("SELECT SUM(collction.AmountPaid) AS amountpaid, branchdata.sno, branchdata.BranchName FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT Branchid, UserData_sno, AmountPaid, Denominations, Remarks, Sno, PaidDate, PaymentType, tripId, CheckStatus, ReturnDenomin, PayTime, VEmpID, ChequeNo, EmpID, ReceiptNo FROM collections WHERE (PaidDate BETWEEN @d1 AND @d2)) collction ON modifiedroutesubtable.BranchID = collction.Branchid INNER JOIN branchdata ON collction.Branchid = branchdata.sno WHERE (modifiedroutes.Sno = @TripID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @d1) OR (modifiedroutes.Sno = @TripID) AND (modifiedroutesubtable.EDate > @d1) AND (modifiedroutesubtable.CDate <= @d1) GROUP BY branchdata.sno, branchdata.BranchName");
            //cmd = new MySqlCommand("SELECT SUM(collction.AmountPaid) AS amountpaid, branchdata.sno, branchdata.BranchName FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT Branchid, UserData_sno, AmountPaid, PaidDate, PaymentType, tripId, CheckStatus FROM collections WHERE (PaidDate BETWEEN @d1 AND @d2) AND (CheckStatus IS NULL) OR (PaidDate BETWEEN @d1 AND @d2) AND (CheckStatus = 'V')) collction ON modifiedroutesubtable.BranchID = collction.Branchid INNER JOIN branchdata ON collction.Branchid = branchdata.sno WHERE (modifiedroutes.BranchID = @TripID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @d1) OR (modifiedroutes.BranchID = @TripID) AND (modifiedroutesubtable.EDate > @d1) AND (modifiedroutesubtable.CDate <= @d1) GROUP BY branchdata.sno, branchdata.BranchName"); 
            //if (Session["salestype"].ToString() == "Plant")
            //{
            //    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            //}
            //else
            //{
            //    cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            //}
            //cmd.Parameters.AddWithValue("@TripID", Session["branch"]);
            //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            //cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            //DataTable dtcollection = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand(" SELECT products_category.Categoryname, productsdata.ProductName, branchproducts.Rank FROM indents_subtable INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN branchmappingtable ON indents.Branch_id = branchmappingtable.SubBranch INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchproducts ON branchmappingtable.SuperBranch = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            //cmd = new MySqlCommand("SELECT products_category.Categoryname, products_subcategory.SubCatName, productsdata.ProductName FROM branchproducts INNER JOIN dispatch ON branchproducts.branch_sno = dispatch.Branch_Id INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.sno = @dispatchSno) Group by productsdata.ProductName  ORDER BY productsdata.Rank");
            //cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
            if (dtble.Rows.Count > 0)
            {
                DataView view = new DataView(dtble);
                //DataTable produtstbl = view.ToTable(true, "ProductName", "Categoryname");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("Route Name");
                Report.Columns.Add("Agent Name");
                foreach (DataRow dr in produtstbl.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                }
                Report.Columns.Add("Total Sale", typeof(Double));
                Report.Columns.Add("Sale Value", typeof(Double));
                Report.Columns.Add("Avg Sale", typeof(Double));
                DataTable distincttable = view.ToTable(true, "BranchName", "BSno", "RouteName");
                int i = 1;
                string RouteName = "";
                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    string DCNO = "0";
                    float amountpaid = 0;
                    //foreach (DataRow drdtclubtotal in dtcollection.Select("sno='" + branch["BSno"].ToString() + "'"))
                    //{
                    //    float.TryParse(drdtclubtotal["amountpaid"].ToString(), out amountpaid);
                    //}
                    
                        newrow["Route Name"] = branch["RouteName"].ToString();
                    
                    newrow["Agent Name"] = branch["BranchName"].ToString();

                    double total = 0;
                    double totalSale = 0;
                    double AvgSale = 0;
                    foreach (DataRow dr in dtble.Rows)
                    {
                        if (branch["BranchName"].ToString() == dr["BranchName"].ToString())
                        {
                            double Amount = 0;
                            double qtyvalue = 0;
                            double DeliveryQty = 0;
                            double.TryParse(dr["DeliveryQty"].ToString(), out DeliveryQty);
                            double UnitCost = 0;
                            double.TryParse(dr["UnitCost"].ToString(), out UnitCost);
                            newrow[dr["ProductName"].ToString()] = DeliveryQty;
                            if (dr["Categoryname"].ToString() == "MILK")
                            {
                                double.TryParse(dr["DeliveryQty"].ToString(), out qtyvalue);
                            }
                            Amount = DeliveryQty * UnitCost;
                            total += DeliveryQty;
                            totalSale += Amount;
                        }
                    }
                    newrow["Total Sale"] = total;
                    double avg = 0;
                    avg = total / NoOfdays;
                    avg = Math.Round(avg, 2);
                    newrow["Avg Sale"] = avg;
                    newrow["Sale Value"] = totalSale;
                    //newrow["Paid Amount"] = Math.Round(amountpaid, 2);
                    //newrow["Due Amount"] = Math.Round((totalSale - amountpaid), 2);
                    double avg1 = 0;
                    double avg2 = 0;
                    double.TryParse(txtavg1.Text, out avg1);
                    double.TryParse(txtavg2.Text, out avg2);
                    if (avg >= avg1 )
                    {
                        if (avg <= avg2)
                        {
                            Report.Rows.Add(newrow);
                            i++;
                        }
                    }
                    
                }
                DataRow newvartical = Report.NewRow();
                newvartical["Agent Name"] = "Total";
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
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
            }
            else
            {
                lblmsg.Text = "No Indent Found";
                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
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
}