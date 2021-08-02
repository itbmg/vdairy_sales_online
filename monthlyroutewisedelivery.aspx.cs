using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;


public partial class monthlyroutewisedelivery : System.Web.UI.Page
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
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
                FillRouteName();
            }
        }

    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    void FillRouteName()
    {
        vdm = new VehicleDBMgr();
        if (Session["salestype"].ToString() == "Plant")
        {
            PBranch.Visible = true;
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType)");
            cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
            cmd.Parameters.AddWithValue("@SalesType", "21");
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            ddlSalesOffice.DataSource = dtRoutedata;
            ddlSalesOffice.DataTextField = "BranchName";
            ddlSalesOffice.DataValueField = "sno";
            ddlSalesOffice.DataBind();
            ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            PBranch.Visible = false;
            cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            ddlRouteName.DataSource = dtRoutedata;
            ddlRouteName.DataTextField = "DispName";
            ddlRouteName.DataValueField = "sno";
            ddlRouteName.DataBind();
        }
    }
    protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
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
    string routeid = "";
    string routeitype = "";
    DataTable Report = new DataTable();
    DataTable dtble = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            PanelHide.Visible = true;
            Report = new DataTable();
            Session["RouteName"] = ddlRouteName.SelectedItem.Text;
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
            lblRoutName.Text = ddlRouteName.SelectedItem.Text;
            Session["filename"] = "AGENT WISE DELIVERY REPORT";
            if (ddlType.SelectedValue == "Consolidated")
            {
                cmd = new MySqlCommand("select Route_id,IndentType from dispatch_sub where dispatch_sno=@dispsno");
                cmd.Parameters.AddWithValue("@dispsno", ddlRouteName.SelectedValue);
                DataTable dtrouteindenttype = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow drrouteitype in dtrouteindenttype.Rows)
                {
                    routeid = drrouteitype["Route_id"].ToString();
                    routeitype = drrouteitype["IndentType"].ToString();
                }
                cmd = new MySqlCommand("SELECT branchaccounts.Amount, branchaccounts.BranchId FROM branchroutesubtable INNER JOIN branchaccounts ON branchroutesubtable.BranchID = branchaccounts.BranchId WHERE (branchroutesubtable.RefNo = @RouteID)");
                cmd.Parameters.AddWithValue("@RouteID", routeid);
                DataTable dtbalance = vdm.SelectQuery(cmd).Tables[0];

                //cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, brnchprdt.Rank, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.UnitCost, indent.IndentType,productsdata.ProductName, branchdata.sno AS BSno, branchdata.BranchName, productsdata.Units, productsdata.sno, products_category.Categoryname,invmaster.Qty FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, Status, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN (SELECT branch_sno, product_sno, unitprice, flag, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno WHERE (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @endtime) OR (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY indents_subtable.UnitCost, branchdata.BranchName, productsdata.sno, products_category.Categoryname ORDER BY brnchprdt.Rank");
                ////cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, brnchprdt.Rank, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.UnitCost, indent.IndentType,productsdata.ProductName, branchdata.sno AS BSno, branchdata.BranchName, productsdata.Units, productsdata.sno, products_category.Categoryname,invmaster.Qty FROM  indents_subtable INNER JOIN (SELECT IndentNo, Branch_id, Status, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON indents_subtable.IndentNo = indent.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN (SELECT branch_sno, product_sno, unitprice, flag, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno RIGHT OUTER JOIN modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno ON indent.Branch_id = branchdata.sno WHERE (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @endtime) OR (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime)) GROUP BY indents_subtable.UnitCost, branchdata.BranchName, productsdata.sno, products_category.Categoryname ORDER BY brnchprdt.Rank");
                cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, brnchprdt.Rank, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.UnitCost, indent.IndentType,productsdata.ProductName, branchdata.sno AS BSno, branchdata.BranchName, productsdata.Units, productsdata.sno, products_category.Categoryname,invmaster.Qty FROM indents_subtable INNER JOIN (SELECT IndentNo, Branch_id, Status, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON indents_subtable.IndentNo = indent.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN (SELECT branch_sno, product_sno, unitprice, flag, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno RIGHT OUTER JOIN modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno ON indent.Branch_id = branchdata.sno WHERE (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @endtime) OR (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY branchdata.sno, productsdata.sno ORDER BY brnchprdt.Rank");
                if (Session["salestype"].ToString() == "Plant")
                {
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                }
                cmd.Parameters.AddWithValue("@RouteID", routeid);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(todate.AddDays(-1)));
                DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT SUM(collction.AmountPaid) AS amountpaid, branchdata.sno, branchdata.BranchName FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT Branchid, UserData_sno, AmountPaid, PaidDate, PaymentType, tripId, CheckStatus, VarifyDate, ChequeDate FROM collections WHERE (VarifyDate BETWEEN @d1 AND @d2) AND (CheckStatus = 'V')) collction ON modifiedroutesubtable.BranchID = collction.Branchid INNER JOIN branchdata ON collction.Branchid = branchdata.sno WHERE (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @d2) OR (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate > @d1) AND (modifiedroutesubtable.CDate <= @d2) GROUP BY branchdata.sno, branchdata.BranchName");
                //cmd = new MySqlCommand("SELECT SUM(collction.AmountPaid) AS amountpaid, branchdata.sno, branchdata.BranchName FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT Branchid, UserData_sno, AmountPaid, PaidDate, PaymentType, tripId, CheckStatus FROM collections WHERE (PaidDate BETWEEN @d1 AND @d2) AND (CheckStatus IS NULL) OR (PaidDate BETWEEN @d1 AND @d2) AND (CheckStatus = 'V')) collction ON modifiedroutesubtable.BranchID = collction.Branchid INNER JOIN branchdata ON collction.Branchid = branchdata.sno WHERE (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @d1) OR (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate > @d1) AND (modifiedroutesubtable.CDate <= @d1) GROUP BY branchdata.sno, branchdata.BranchName");
                //cmd = new MySqlCommand("SELECT SUM(collections.AmountPaid) AS amountpaid, branchdata_1.sno, branchdata_1.BranchName FROM modifiedroutes INNER JOIN branchdata ON modifiedroutes.BranchID = branchdata.sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata branchdata_1 ON modifiedroutesubtable.BranchID = branchdata_1.sno INNER JOIN collections ON branchdata_1.sno = collections.Branchid WHERE (modifiedroutes.BranchID = @BranchID) AND (modifiedroutes.Sno = @RouteID) AND (collections.PaidDate BETWEEN @d1 AND @d2) OR (branchdata.SalesOfficeID = @BranchID) AND (modifiedroutes.Sno = @RouteID) AND (collections.PaidDate BETWEEN @d1 AND @d2) GROUP BY branchdata.sno, branchdata_1.BranchName"); 
                if (Session["salestype"].ToString() == "Plant")
                {
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                }
                cmd.Parameters.AddWithValue("@RouteID", routeid);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                DataTable dtcollection = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT SUM(collction.AmountPaid) AS amountpaid, branchdata.sno, branchdata.BranchName FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT Branchid, UserData_sno, AmountPaid, PaidDate, PaymentType, tripId, CheckStatus, VarifyDate, ChequeDate FROM collections WHERE (PaidDate BETWEEN @d1 AND @d2) AND (CheckStatus IS NULL)) collction ON modifiedroutesubtable.BranchID = collction.Branchid INNER JOIN branchdata ON collction.Branchid = branchdata.sno WHERE (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @d2) OR (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate > @d1) AND (modifiedroutesubtable.CDate <= @d2) GROUP BY branchdata.sno, branchdata.BranchName");
                if (Session["salestype"].ToString() == "Plant")
                {
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                }
                cmd.Parameters.AddWithValue("@RouteID", routeid);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                DataTable dtCashcollection = vdm.SelectQuery(cmd).Tables[0];

                if (dtble.Rows.Count > 0)
                {
                    DataView view = new DataView(dtble);
                    DataTable produtstbl = view.ToTable(true, "ProductName", "Categoryname");
                    Report = new DataTable();
                    Report.Columns.Add("SNo");
                    Report.Columns.Add("Agent Name");
                    foreach (DataRow dr in produtstbl.Rows)
                    {
                        Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                    }
                    Report.Columns.Add("Total Sale").DataType = typeof(Double);
                    Report.Columns.Add("Sale Value").DataType = typeof(Double);
                    Report.Columns.Add("Paid Amount").DataType = typeof(Double);
                    Report.Columns.Add("Due Amount").DataType = typeof(Double);
                    Report.Columns.Add("Total Due Amount").DataType = typeof(Double);
                    DataTable distincttable = view.ToTable(true, "BranchName", "BSno");
                    int i = 1;
                    foreach (DataRow branch in distincttable.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["SNo"] = i;
                        string DCNO = "0";
                        float amountpaid = 0;
                        float totamountpaid = 0;
                        foreach (DataRow drdtclubtotal in dtcollection.Select("sno='" + branch["BSno"].ToString() + "'"))
                        {
                            float.TryParse(drdtclubtotal["amountpaid"].ToString(), out amountpaid);
                        }
                        float cashamountpaid = 0;
                        foreach (DataRow drdtclubtotal in dtCashcollection.Select("sno='" + branch["BSno"].ToString() + "'"))
                        {
                            float.TryParse(drdtclubtotal["amountpaid"].ToString(), out cashamountpaid);
                        }
                        float Finaldue = 0;
                        foreach (DataRow drdtclubtotal in dtbalance.Select("BranchId='" + branch["BSno"].ToString() + "'"))
                        {
                            float.TryParse(drdtclubtotal["Amount"].ToString(), out Finaldue);
                        }
                        newrow["Agent Name"] = branch["BranchName"].ToString();
                        totamountpaid = amountpaid + cashamountpaid;

                        double total = 0;
                        double totalSale = 0;
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
                                if (DeliveryQty == 0)
                                {
                                }
                                else
                                {
                                    newrow[dr["ProductName"].ToString()] = DeliveryQty;
                                }

                                if (dr["Categoryname"].ToString() == " ")
                                {
                                }
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
                        newrow["Sale Value"] = totalSale;
                        newrow["Paid Amount"] = Math.Round(totamountpaid, 2);
                        newrow["Due Amount"] = Math.Round((totalSale - totamountpaid), 2);
                        if (Finaldue == 0)
                        {
                        }
                        else
                        {
                            newrow["Total Due Amount"] = Math.Round(Finaldue, 2);
                            Report.Rows.Add(newrow);
                            i++;
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
                            if (val == 0.0)
                            {
                            }
                            else
                            {
                                newvartical[dc.ToString()] = val;
                            }
                        }
                    }
                    Report.Rows.Add(newvartical);
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
                    PanelHide.Visible = false;
                    lblmsg.Text = "No Indent Found";
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                }
            }
            else
            {
                TimeSpan dateSpan = todate.Subtract(fromdate);
                int NoOfdays = dateSpan.Days;
                NoOfdays = NoOfdays + 1;
                cmd = new MySqlCommand("select Route_id,IndentType from dispatch_sub where dispatch_sno=@dispsno");
                cmd.Parameters.AddWithValue("@dispsno", ddlRouteName.SelectedValue);
                DataTable dtrouteindenttype = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow drrouteitype in dtrouteindenttype.Rows)
                {
                    routeid = drrouteitype["Route_id"].ToString();
                    routeitype = drrouteitype["IndentType"].ToString();
                }
                cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, products_category.Categoryname, products_category.sno, branchdata.BranchName, indent.I_date FROM indents_subtable INNER JOIN (SELECT IndentNo, Branch_id, Status, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON indents_subtable.IndentNo = indent.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno RIGHT OUTER JOIN modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno ON indent.Branch_id = branchdata.sno WHERE (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @endtime) AND (branchdata.flag=1)  OR (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (branchdata.flag=1) GROUP BY products_category.Categoryname, products_category.sno, branchdata.BranchName, indent.I_date ORDER BY indent.I_date");
                if (Session["salestype"].ToString() == "Plant")
                {
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                }
                cmd.Parameters.AddWithValue("@RouteID", routeid);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(todate.AddDays(-1)));
                dtble = vdm.SelectQuery(cmd).Tables[0];
                Report = new DataTable();
                Report.Columns.Add("Sno");
                Report.Columns.Add("Agent Name");
                for (int j = 0; j < NoOfdays; j++)
                {
                    DateTime adddays = fromdate.AddDays(j);
                    string newdate = adddays.ToString("dd/MMM");
                    string newmilk = newdate + "Milk";
                    string newcurd = newdate + "Curd";
                    Report.Columns.Add(newmilk).DataType = typeof(Double);
                    Report.Columns.Add(newcurd).DataType = typeof(Double);
                }
                int i = 1;
                DataView view = new DataView(dtble);
                DataTable distincttable = view.ToTable(true, "BranchName");
                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["Agent Name"] = branch["BranchName"].ToString();
                    foreach (DataRow dr in dtble.Rows)
                    {
                        if (branch["BranchName"].ToString() == dr["BranchName"].ToString())
                        {
                            string DOE = dr["I_date"].ToString();
                            if (DOE == "")
                            {
                            }
                            else
                            {
                                if (dr["Categoryname"].ToString() == "MILK")
                                {
                                    DateTime dtDOE = Convert.ToDateTime(DOE).AddDays(1);
                                    string ChangedTime = dtDOE.ToString("dd/MMM");
                                    string newmilk = ChangedTime + "Milk";
                                    double DeliveryQty = 0;
                                    double.TryParse(dr["DeliveryQty"].ToString(), out DeliveryQty);
                                    newrow[newmilk] = DeliveryQty;
                                }
                                if (dr["Categoryname"].ToString() == "CURD")
                                {
                                    DateTime dtDOE = Convert.ToDateTime(DOE).AddDays(1);
                                    string ChangedTime = dtDOE.ToString("dd/MMM");
                                    string newcurd = ChangedTime + "Curd";
                                    double DeliveryQty = 0;
                                    double.TryParse(dr["DeliveryQty"].ToString(), out DeliveryQty);
                                    newrow[newcurd] = DeliveryQty;
                                }
                            }
                        }
                    }
                    Report.Rows.Add(newrow);
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
                grdnewreports.DataSource = Report;
                grdnewreports.DataBind();
                Session["xportdata"] = Report;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdReports.DataSource = Report;
            grdReports.DataBind();
        }
    }
    protected void gvEmployee_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
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
            TimeSpan dateSpan = todate.Subtract(fromdate);
            int NoOfdays = dateSpan.Days;
            NoOfdays = NoOfdays + 1;
            TableCell HeaderCell3 = new TableCell();
            HeaderCell3.Text = "Sno";
            HeaderCell3.ColumnSpan = 1;
            HeaderCell3.BackColor = System.Drawing.Color.LightBlue;
            HeaderCell3.ForeColor = System.Drawing.Color.Black;
            HeaderRow.Cells.Add(HeaderCell3);
            TableCell HeaderCell1 = new TableCell();
            HeaderCell1.Text = "Agent Name";
            HeaderCell1.ColumnSpan = 1;
            HeaderCell1.BackColor = System.Drawing.Color.LightBlue;
            HeaderCell1.ForeColor = System.Drawing.Color.Black;
            HeaderRow.Cells.Add(HeaderCell1);
            for (int j = 0; j < NoOfdays; j++)
            {
                DateTime adddays = fromdate.AddDays(j);
                string newdate = adddays.ToString("dd/MMM");
                TableCell HeaderCell2 = new TableCell();
                HeaderCell2.Text = newdate;
                HeaderCell2.ColumnSpan = 2;
                HeaderCell2.BorderColor = System.Drawing.Color.Gray;
                HeaderCell2.BackColor = System.Drawing.Color.LightBlue;
                HeaderCell2.ForeColor = System.Drawing.Color.Black;
                HeaderRow.Cells.Add(HeaderCell2);
            }
            grdnewreports.Controls[0].Controls.AddAt(0, HeaderRow);

            GridViewRow HeaderRow1 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell12 = new TableCell();
            HeaderCell12.Text = "";
            HeaderRow1.Cells.Add(HeaderCell12);

            HeaderCell12 = new TableCell();
            HeaderCell12.Text = "";
            HeaderRow1.Cells.Add(HeaderCell12);
            for (int j = 0; j < NoOfdays; j++)
            {
                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "Milk";
                HeaderCell.BorderColor = System.Drawing.Color.Gray;
                HeaderCell.BackColor = System.Drawing.Color.LightBlue;
                HeaderCell.ForeColor = System.Drawing.Color.Black;
                HeaderRow1.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Curd";
                HeaderCell.BorderColor = System.Drawing.Color.Gray;
                HeaderCell.BackColor = System.Drawing.Color.LightBlue;
                HeaderCell.ForeColor = System.Drawing.Color.Black;
                HeaderRow1.Cells.Add(HeaderCell);
            }
            HeaderRow.Attributes.Add("class", "header");
            HeaderRow1.Attributes.Add("class", "header");
            grdnewreports.Controls[0].Controls.AddAt(1, HeaderRow1);
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