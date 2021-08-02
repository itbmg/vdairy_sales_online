using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class agentavgmilk_curdsale : System.Web.UI.Page
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
                lblTitle.Text = Session["TitleName"].ToString();
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            PanelHide.Visible = true;
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
            cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, modifiedroutes.Sno AS routeid, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue, indent.IndentType, productsdata.ProductName, branchdata.sno AS BSno, branchdata.BranchName, productsdata.Units, productsdata.sno, products_category.Categoryname FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, Status, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE  (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @endtime) AND (modifiedroutes.BranchID = @TripID) OR (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (modifiedroutes.BranchID = @TripID) GROUP BY branchdata.BranchName, productsdata.sno, products_category.Categoryname ORDER BY modifiedroutes.RouteName");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@TripID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(todate.AddDays(-1)));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT products_category.Categoryname, productsdata.ProductName, branchproducts.Rank FROM indents_subtable INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN branchmappingtable ON indents.Branch_id = branchmappingtable.SubBranch INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN                         branchproducts ON branchmappingtable.SuperBranch = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
            if (dtble.Rows.Count > 0)
            {
                DataView view = new DataView(dtble);
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("Route Name");
                Report.Columns.Add("Agent Name");
                foreach (DataRow dr in produtstbl.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                }
                Report.Columns.Add("Total Milk Sale", typeof(Double));
                Report.Columns.Add("Milk Avg Sale", typeof(Double));
                Report.Columns.Add("Total Curd Sale", typeof(Double));
                Report.Columns.Add("Curd Avg Sale", typeof(Double));
                DataTable distincttable = view.ToTable(true, "BranchName", "BSno", "RouteName", "routeid");
                int i = 1;
                int Totalcount = 1;
                string RouteName = "";
                string routeid = "";
                string finalrouteid = "";
                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    finalrouteid = branch["routeid"].ToString();
                    if (RouteName != branch["RouteName"].ToString())
                    {
                        if (Totalcount == 1)
                        {
                            newrow["Route Name"] = branch["RouteName"].ToString();
                            Totalcount++;
                        }
                        else
                        {
                            DataRow newvar = Report.NewRow();
                            newvar["Agent Name"] = "Total";
                           
                            double ftotalmilkSale = 0;
                            double ftotalcurdSale = 0;
                            int route = 0;
                            int.TryParse(routeid, out route);

                            foreach (DataRow dr in dtble.Select("RouteID='" + route + "'"))
                            {
                                double d_qty = 0;
                                double DeliveryQty = 0;
                                double UnitCost = 0;
                                DataRow[] drincentive = dtble.Select("RouteID='" + route + "' and ProductName='" + dr["ProductName"].ToString() + "'");
                                foreach (DataRow drc in drincentive)
                                {
                                    double.TryParse(drc.ItemArray[2].ToString(), out DeliveryQty);
                                    double.TryParse(drc.ItemArray[3].ToString(), out UnitCost);
                                    d_qty += DeliveryQty;
                                }
                                if (d_qty == 0.0)
                                {
                                }
                                else
                                {
                                    newvar[dr["ProductName"].ToString()] = d_qty;
                                }
                                if (dr["Categoryname"].ToString() == "MILK")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out DeliveryQty);
                                    ftotalmilkSale += DeliveryQty;
                                }
                                if (dr["Categoryname"].ToString() == "CURD")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out DeliveryQty);
                                    ftotalcurdSale += DeliveryQty;
                                }
                            }
                            newvar["Total Milk Sale"] = Math.Round(ftotalmilkSale, 2);
                            double totMilkavg = 0;
                            totMilkavg = ftotalmilkSale / NoOfdays;
                            totMilkavg = Math.Round(totMilkavg, 2);
                            newvar["Milk Avg Sale"] = totMilkavg;
                            newvar["Total Curd Sale"] = Math.Round(ftotalcurdSale, 2);
                            double totCurdavg = 0;
                            totCurdavg = ftotalcurdSale / NoOfdays;
                            totCurdavg = Math.Round(totCurdavg, 2);
                            newvar["Curd Avg Sale"] = totCurdavg;
                            Report.Rows.Add(newvar);
                            newrow["Route Name"] = branch["RouteName"].ToString();
                            Totalcount++;
                            DataRow space = Report.NewRow();
                            space["Agent Name"] = "";
                            Report.Rows.Add(space);
                            routeid = branch["routeid"].ToString();
                        }

                    }
                    else
                    {
                        newrow["Route Name"] = "";
                        routeid = branch["routeid"].ToString();
                    }
                    RouteName = branch["RouteName"].ToString();
                    newrow["Agent Name"] = branch["BranchName"].ToString();
                    double totalmilkSale = 0;
                    double totalcurdSale = 0;
                    foreach (DataRow dr in dtble.Rows)
                    {
                        if (branch["BranchName"].ToString() == dr["BranchName"].ToString())
                        {
                            double qtyvalue = 0;
                            double DeliveryQty = 0;
                            double.TryParse(dr["DeliveryQty"].ToString(), out DeliveryQty);
                            double UnitCost = 0;
                            double.TryParse(dr["salevalue"].ToString(), out UnitCost);
                            if (DeliveryQty == 0.0)
                            {
                            }
                            else
                            {
                                newrow[dr["ProductName"].ToString()] = DeliveryQty;
                            }
                            if (dr["Categoryname"].ToString() == "MILK")
                            {
                                double.TryParse(dr["DeliveryQty"].ToString(), out qtyvalue);
                                totalmilkSale += qtyvalue;
                            }
                            if (dr["Categoryname"].ToString() == "CURD")
                            {
                                double.TryParse(dr["DeliveryQty"].ToString(), out qtyvalue);
                                totalcurdSale += qtyvalue;
                            }
                        }
                    }
                    newrow["Total Milk Sale"] = Math.Round(totalmilkSale, 2);
                    double Milkavg = 0;
                    Milkavg = totalmilkSale / NoOfdays;
                    Milkavg = Math.Round(Milkavg, 2);
                    newrow["Milk Avg Sale"] = Milkavg;
                    newrow["Total Curd Sale"] = Math.Round(totalcurdSale, 2);
                    double curdavg = 0;
                    curdavg = totalcurdSale / NoOfdays;
                    curdavg = Math.Round(curdavg, 2);
                    newrow["Curd Avg Sale"] = curdavg;
                    Report.Rows.Add(newrow);
                    routeid = branch["routeid"].ToString();
                    i++;
                }
                DataRow newvar1 = Report.NewRow();
                newvar1["Agent Name"] = "Total";
                cmd = new MySqlCommand("SELECT brnchprdt.Rank, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,products_category.Categoryname, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue, productsdata.ProductName FROM indents_subtable INNER JOIN (SELECT IndentNo, Branch_id, Status, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON indents_subtable.IndentNo = indent.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN (SELECT branch_sno, product_sno, unitprice, flag, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno RIGHT OUTER JOIN modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno ON indent.Branch_id = branchdata.sno WHERE (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @endtime) AND (productsdata.ProductName IS NOT NULL) OR (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (productsdata.ProductName IS NOT NULL) GROUP BY productsdata.sno ORDER BY brnchprdt.Rank");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@RouteID", finalrouteid);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(todate.AddDays(-1)));
                DataTable dtfinalroutetotal = vdm.SelectQuery(cmd).Tables[0];
                double dtotalmilkSale = 0;

                double dtotalcurdSale = 0;
                foreach (DataRow dr in dtfinalroutetotal.Rows)
                {
                    double DeliveryQty = 0;

                    double.TryParse(dr["DeliveryQty"].ToString(), out DeliveryQty);
                    if (DeliveryQty == 0.0)
                    {
                    }
                    else
                    {
                        newvar1[dr["ProductName"].ToString()] = DeliveryQty;
                    }
                    if (dr["Categoryname"].ToString() == "MILK")
                    {
                        double.TryParse(dr["DeliveryQty"].ToString(), out DeliveryQty);
                        dtotalmilkSale += DeliveryQty;
                    }
                    if (dr["Categoryname"].ToString() == "CURD")
                    {
                        double.TryParse(dr["DeliveryQty"].ToString(), out DeliveryQty);
                        dtotalcurdSale += DeliveryQty;
                    }
                }
                newvar1["Total Milk Sale"] = Math.Round(dtotalmilkSale, 2);
                double dMilkavg = 0;
                dMilkavg = dtotalmilkSale / NoOfdays;
                dMilkavg = Math.Round(dMilkavg, 2);
                newvar1["Milk Avg Sale"] = dMilkavg;
                newvar1["Total Curd Sale"] = Math.Round(dtotalcurdSale, 2);
                double dcurdavg = 0;
                dcurdavg = dtotalcurdSale / NoOfdays;
                dcurdavg = Math.Round(dcurdavg, 2);
                newvar1["Curd Avg Sale"] = dcurdavg;
                Report.Rows.Add(newvar1);
                DataRow space1 = Report.NewRow();
                space1["Agent Name"] = "";
                Report.Rows.Add(space1);
                cmd = new MySqlCommand("SELECT products_category.Categoryname,  ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue, productsdata.ProductName, productsdata.sno FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, Status, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @endtime) AND (modifiedroutes.BranchID = @TripID) OR (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (modifiedroutes.BranchID = @TripID) GROUP BY productsdata.sno");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@TripID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(todate.AddDays(-1)));
                DataTable dttotal = vdm.SelectQuery(cmd).Tables[0];
                DataRow newvar2 = Report.NewRow();
                newvar2["Agent Name"] = "Total";
                foreach (DataRow dr in dttotal.Rows)
                {
                    double DeliveryQty = 0;
                    double Qty = 0;
                    double.TryParse(dr["DeliveryQty"].ToString(), out Qty);
                    if (Qty == 0.0)
                    {
                    }
                    else
                    {
                        newvar2[dr["ProductName"].ToString()] = Qty;

                    }
                    if (dr["Categoryname"].ToString() == "MILK")
                    {
                        double.TryParse(dr["DeliveryQty"].ToString(), out DeliveryQty);
                        dtotalmilkSale += DeliveryQty;
                    }
                    if (dr["Categoryname"].ToString() == "CURD")
                    {
                        double.TryParse(dr["DeliveryQty"].ToString(), out DeliveryQty);
                        dtotalcurdSale += DeliveryQty;
                    }
                }
                newvar2["Total Milk Sale"] = Math.Round(dtotalmilkSale, 2);
                double eMilkavg = 0;
                eMilkavg = dtotalmilkSale / NoOfdays;
                eMilkavg = Math.Round(eMilkavg, 2);
                newvar2["Milk Avg Sale"] = eMilkavg;
                newvar2["Total Curd Sale"] = Math.Round(dtotalcurdSale, 2);
                double ecurdavg = 0;
                ecurdavg = dtotalcurdSale / NoOfdays;
                ecurdavg = Math.Round(ecurdavg, 2);
                newvar2["Curd Avg Sale"] = ecurdavg;
                Report.Rows.Add(newvar2);

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
            Session["xportdata"] = Report;
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