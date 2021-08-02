using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Text;
using ClosedXML.Excel;
using System.Configuration;
using System.Net;

public partial class agentwiseincentiverptnew : System.Web.UI.Page
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
                FillRouteName();
                lblTitle.Text = Session["TitleName"].ToString();
            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        lblmsg.Text = " ";
        GetReport();
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
                //cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
                //cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@BranchD", Session["branch"].ToString());
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlRouteName.DataSource = dtRoutedata;
                ddlRouteName.DataTextField = "DispName";
                ddlRouteName.DataValueField = "sno";
                ddlRouteName.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
        cmd.Parameters.AddWithValue("@BranchD", ddlSalesOffice.SelectedValue);
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
    DateTime fromdate = DateTime.Now;
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            lblmsg.Text = "";
            DateTime fromdate = DateTime.Now;
            Report = new DataTable();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
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
            DateTime todate = DateTime.Now;
            string[] todatestrig = txttodate.Text.Split(' ');
            if (todatestrig.Length > 1)
            {
                if (todatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = todatestrig[0].Split('-');
                    string[] times = todatestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            lblRouteName.Text = ddlRouteName.SelectedItem.Text;
            lblDate.Text = fromdate.ToString("dd/MMM/yyyy");
            lbl_selttodate.Text = todate.ToString("dd/MMM/yyyy");
            Session["filename"] = "Agent Wise Incentive";

            TimeSpan dateSpan = todate.Subtract(fromdate);
            int NoOfdays = dateSpan.Days;
            NoOfdays = NoOfdays + 1;
            cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, modifiedroutes.Sno AS routeid, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue, indent.IndentType, productsdata.ProductName, branchdata.sno AS BSno, branchdata.BranchName, productsdata.Units, productsdata.sno, products_category.Categoryname FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, Status, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE  (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @endtime) AND (modifiedroutes.BranchID = @TripID) OR (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (modifiedroutes.BranchID = @TripID) GROUP BY branchdata.BranchName, productsdata.sno, products_category.Categoryname ORDER BY modifiedroutes.RouteName");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@TripID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(todate.AddDays(-1)));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];



            cmd = new MySqlCommand("SELECT products_category.Categoryname, productsdata.sno, productsdata.ProductName, branchproducts.Rank FROM indents_subtable INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN branchmappingtable ON indents.Branch_id = branchmappingtable.SubBranch INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN                         branchproducts ON branchmappingtable.SuperBranch = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
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
                Report.Columns.Add("Agentid");
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
                            double totavgmilkSale = 0;
                            double ftotalcurdSale = 0;
                            double totavgcurdSale = 0;
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
                    newrow["Agentid"] = branch["BSno"].ToString();

                    
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
          

                // new incentive query naveen
                cmd = new MySqlCommand("SELECT incentive_maindetails.sno, incentive_maindetails.incentivetype, incentive_maindetails.branchid, incentive_maindetails.routeid, incentive_maindetails.agentid, branchdata.BranchName,   incentive_maindetails.structuretype, incentive_maindetails.leakage, incentive_maindetails.fromdate, incentive_maindetails.todate, incentive_maindetails.remarks, incentive_maindetails.createdby,   incentive_maindetails.approvedby, incentive_maindetails.createddate, incentive_maindetails.status, incentive_subdetails.refno, incentive_subdetails.productid, incentive_subdetails.amount FROM            incentive_maindetails INNER JOIN incentive_subdetails ON incentive_maindetails.sno = incentive_subdetails.refno INNER JOIN branchdata ON branchdata.sno = incentive_maindetails.agentid  WHERE        (incentive_maindetails.branchid = @branchid) AND (incentive_maindetails.routeid = @routeid)");
                cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@routeid", ddlRouteName.SelectedValue);
                DataTable dtincentive = vdm.SelectQuery(cmd).Tables[0];
                DataTable dtagent = view.ToTable(true, "branchid", "routeid", "agentid", "BranchName");
                if (dtagent.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtagent.Rows)
                    {
                        string agentid = dr[""].ToString();

                    }
                }
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
    protected void btnMessage_Click(object sender, EventArgs e)
    {
        try
        {
            lblmsg.Text = "";
            if (Session["Report"] != null)
            {
                vdm = new VehicleDBMgr();
                DataTable dtIncentive = (DataTable)Session["Report"];
                foreach (DataRow dr in dtIncentive.Rows)
                {
                    if (dr["sno"].ToString() == "")
                    {

                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT sno,BranchName, phonenumber FROM branchdata where sno=@BranchName");
                        cmd.Parameters.AddWithValue("@BranchName", dr["sno"].ToString());
                        DataTable dtBranchdata = vdm.SelectQuery(cmd).Tables[0];
                        if (dtBranchdata.Rows.Count > 0)
                        {
                            string BranchName = dtBranchdata.Rows[0]["BranchName"].ToString();
                            string phonenumber = dtBranchdata.Rows[0]["phonenumber"].ToString();
                            string IncentiveAmount = dr["totdiscount"].ToString();


                            DateTime fromdate = DateTime.Now;

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
                            DateTime todate = DateTime.Now;
                            string[] todatestrig = txttodate.Text.Split(' ');
                            if (todatestrig.Length > 1)
                            {
                                if (todatestrig[0].Split('-').Length > 0)
                                {
                                    string[] dates = todatestrig[0].Split('-');
                                    string[] times = todatestrig[1].Split(':');
                                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                                }
                            }
                            string fDate = fromdate.ToString("dd/MM/yyyy");
                            string tDate = todate.ToString("dd/MM/yyyy");
                            if (phonenumber.Length == 10)
                            {
                                WebClient client = new WebClient();
                                //string strUrl = " http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + no + "&msg=" + message1 + "&type=1 ";

                                string baseurl = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + phonenumber + "&message=Dear%20" + BranchName + "%20,%20 Your Incentive Amount For Date%20%20" + fDate + "%20To%20" + tDate + "%20Amount:%20" + IncentiveAmount + "&sender=VYSNVI&type=1&route=2";

                                // string baseurl = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + phonenumber + "&msg=Dear%20" + BranchName + "%20,%20 Your Incentive Amount For Date%20%20" + fDate + "%20To%20" + tDate + "%20Amount:%20" + IncentiveAmount + "&type=1";
                                Stream data = client.OpenRead(baseurl);
                                StreamReader reader = new StreamReader(data);
                                string ResponseID = reader.ReadToEnd();
                                data.Close();
                                reader.Close();
                            }
                        }
                    }
                }
                lblmsg.Text = "SMS Sent Successfully";
            }
            else
            {
                lblmsg.Text = "No data were found";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable("GridView_Data");
            foreach (TableCell cell in grdReports.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in grdReports.Rows)
            {
                dt.Rows.Add();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (row.Cells[i].Text == "&nbsp;")
                    {
                        row.Cells[i].Text = "0";
                    }
                    dt.Rows[dt.Rows.Count - 1][i] = row.Cells[i].Text;
                }
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string FileName = Session["filename"].ToString();
                Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}