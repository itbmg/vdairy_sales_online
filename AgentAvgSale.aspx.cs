using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class AgentAvgSale : System.Web.UI.Page
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
    private DateTime GetLowMonthRetrive(DateTime dt)
    {
        double Day, Hour, Min, Sec;
        DateTime DT = dt;
        DT = dt;
        Day = -dt.Day + 1;
        Hour = -dt.Hour;
        Min = -dt.Minute;
        Sec = -dt.Second;
        DT = DT.AddDays(Day);
        DT = DT.AddHours(Hour);
        DT = DT.AddMinutes(Min);
        DT = DT.AddSeconds(Sec);
        return DT;

    }
    private DateTime GetHighMonth(DateTime dt)
    {
        double Day, Hour, Min, Sec;
        DateTime DT = DateTime.Now;
        Day = 31 - dt.Day;
        Hour = 23 - dt.Hour;
        Min = 59 - dt.Minute;
        Sec = 59 - dt.Second;
        DT = dt;
        DT = DT.AddDays(Day);
        DT = DT.AddHours(Hour);
        DT = DT.AddMinutes(Min);
        DT = DT.AddSeconds(Sec);
        if (DT.Day == 3)
        {
            DT = DT.AddDays(-3);
        }
        else if (DT.Day == 2)
        {
            DT = DT.AddDays(-2);
        }
        else if (DT.Day == 1)
        {
            DT = DT.AddDays(-1);
        }
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
            ////cmd = new MySqlCommand("SELECT modifiedroutes.RouteName,modifiedroutes.Sno AS routeid, brnchprdt.Rank, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue, indent.IndentType,productsdata.ProductName, branchdata.sno AS BSno, branchdata.BranchName, productsdata.Units, productsdata.sno, products_category.Categoryname, invmaster.Qty FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, Status, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN (SELECT branch_sno, product_sno, unitprice, flag, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno WHERE (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @endtime) AND (modifiedroutes.BranchID = @TripID) OR (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (modifiedroutes.BranchID = @TripID) GROUP BY branchdata.BranchName, productsdata.sno, products_category.Categoryname ORDER BY modifiedroutes.RouteName");

            //Ravvindra 01/24/2017
            //cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, modifiedroutes.Sno AS routeid, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue, indent.IndentType, productsdata.ProductName, branchdata.sno AS BSno, branchdata.BranchName, productsdata.Units, productsdata.sno, products_category.Categoryname FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, Status, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE  (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @endtime) AND (modifiedroutes.BranchID = @TripID) OR (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (modifiedroutes.BranchID = @TripID) GROUP BY branchdata.BranchName, productsdata.sno, products_category.Categoryname ORDER BY modifiedroutes.RouteName");
            int Years = todate.Year - fromdate.Year;

            DateTime firstmonth = new DateTime();
            DateTime lastmonth = new DateTime();
            //        todate = todate.AddMonths(1);
            //        TimeSpan dateSpan1 = todate.Subtract(fromdate);
            //        int years = (dateSpan1.Days / 365);
            //        int months = ((dateSpan1.Days % 365) / 31) + (years * 12);
                 //   int i = 1;
                    if (Years != 0)
                    {
                        int yearnumber = 0;
                        for (int j = 0; j < Years; j++)
                        {
                            firstmonth = GetLowMonthRetrive(fromdate.AddYears(j));
                            lastmonth=GetHighMonth(firstmonth);
                            DateTime dtF = firstmonth.AddDays(-1);
                            TimeSpan dateSpan2 = lastmonth.Subtract(dtF);
                            yearnumber++;
                        }
                    }
            
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
                Report.Columns.Add("Total Sale", typeof(Double));
                Report.Columns.Add("Sale Value", typeof(Double));
                Report.Columns.Add("Avg Sale", typeof(Double));
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
                            double totsal = 0;
                            double totsaleval = 0;
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
                                double.TryParse(dr["salevalue"].ToString(), out UnitCost);
                                totsal += DeliveryQty;
                                totsaleval += UnitCost;
                            }
                            newvar["Total Sale"] = Math.Round(totsal, 2);
                            newvar["Sale Value"] = Math.Round(totsaleval, 2);
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
                    double total = 0;
                    double totalSale = 0;
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
                            }
                            total += DeliveryQty;
                            totalSale += UnitCost;
                        }
                    }
                    newrow["Total Sale"] = Math.Round(total, 2);
                    double avg = 0;
                    avg = total / NoOfdays;
                    avg = Math.Round(avg, 2);
                    newrow["Avg Sale"] = avg;
                    newrow["Sale Value"] = Math.Round(totalSale, 2);
                    Report.Rows.Add(newrow);
                    routeid = branch["routeid"].ToString();
                    i++;
                }
                DataRow newvar1 = Report.NewRow();
                newvar1["Agent Name"] = "Total";
                cmd = new MySqlCommand("SELECT brnchprdt.Rank, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue, productsdata.ProductName FROM indents_subtable INNER JOIN (SELECT IndentNo, Branch_id, Status, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON indents_subtable.IndentNo = indent.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN (SELECT branch_sno, product_sno, unitprice, flag, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno RIGHT OUTER JOIN modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno ON indent.Branch_id = branchdata.sno WHERE (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @endtime) AND (productsdata.ProductName IS NOT NULL) OR (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (productsdata.ProductName IS NOT NULL) GROUP BY productsdata.sno ORDER BY brnchprdt.Rank");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@RouteID", finalrouteid);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(todate.AddDays(-1)));
                DataTable dtfinalroutetotal = vdm.SelectQuery(cmd).Tables[0];
                double totsal1 = 0;
                double totsaleval1 = 0;
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
                    double UnitCost = 0;
                    double.TryParse(dr["salevalue"].ToString(), out UnitCost);
                    totsal1 += DeliveryQty;
                    totsaleval1 += UnitCost;
                }
                newvar1["Total Sale"] = Math.Round(totsal1, 2);
                newvar1["Sale Value"] = Math.Round(totsaleval1, 2);
                Report.Rows.Add(newvar1);
                DataRow space1 = Report.NewRow();
                space1["Agent Name"] = "";
                Report.Rows.Add(space1);


                ////cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue, productsdata.ProductName, productsdata.sno FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, Status, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN (SELECT branch_sno, product_sno, unitprice, flag, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno WHERE (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @endtime) AND (modifiedroutes.BranchID = @TripID) OR (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (modifiedroutes.BranchID = @TripID) GROUP BY productsdata.sno ORDER BY brnchprdt.Rank");
                //Ravindra 24/01/2017
                cmd = new MySqlCommand("SELECT  ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue, productsdata.ProductName, productsdata.sno FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, Status, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @endtime) AND (modifiedroutes.BranchID = @TripID) OR (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (modifiedroutes.BranchID = @TripID) GROUP BY productsdata.sno");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@TripID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(todate.AddDays(-1)));
                DataTable dttotal = vdm.SelectQuery(cmd).Tables[0];
                DataRow newvar2 = Report.NewRow();
                newvar2["Agent Name"] = "Total";
                double totsal2 = 0;
                double totsaleval2 = 0;
                foreach (DataRow dr in dttotal.Rows)
                {
                    double DeliveryQty = 0;
                    double.TryParse(dr["DeliveryQty"].ToString(), out DeliveryQty);
                    if (DeliveryQty == 0.0)
                    {
                    }
                    else
                    {
                        newvar2[dr["ProductName"].ToString()] = DeliveryQty;
                    }
                    double UnitCost = 0;
                    double.TryParse(dr["salevalue"].ToString(), out UnitCost);
                    totsal2 += DeliveryQty;
                    totsaleval2 += UnitCost;
                }
                newvar2["Total Sale"] = Math.Round(totsal2, 2);
                newvar2["Sale Value"] = Math.Round(totsaleval2, 2);
                Report.Rows.Add(newvar2);
                //newvartical["Agent Name"] = "Total";
                //double val = 0.0;
                //foreach (DataColumn dc in Report.Columns)
                //{
                //    if (dc.DataType == typeof(Double))
                //    {
                //        val = 0.0;
                //        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                //        newvartical[dc.ToString()] = val;
                //    }
                //}
                //Report.Rows.Add(newvartical);
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