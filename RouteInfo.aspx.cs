using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class RouteInfo : System.Web.UI.Page
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
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
                lblAddress.Text = Session["Address"].ToString();
                lbltinNo.Text = Session["TinNo"].ToString();
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
            PBranch.Visible = false;
            cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID)  and (DispType is NULL) and (dispatch.flag=@flag)) OR ((branchdata_1.SalesOfficeID = @SOID)  and (DispType is NULL)) and (dispatch.flag=@flag)");
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
    protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID)   and (dispatch.flag=@flag)) OR ((branchdata_1.SalesOfficeID = @SOID)   and (dispatch.flag=@flag))");
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
    string routeid = "";
    string routeitype = "";
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            Report = new DataTable();
            Session["RouteName"] = ddlRouteName.SelectedItem.Text;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
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
            lbl_selfromdate.Text = fromdate.ToString("dd/MM/yyyy");
            lblRoutName.Text = ddlRouteName.SelectedItem.Text;
            Session["filename"] = "AGENT WISE DELIVERY REPORT";
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
            cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, brnchprdt.Rank, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.UnitCost, indent.IndentType,productsdata.ProductName, branchdata.sno AS BSno, branchdata.BranchName, productsdata.Units, productsdata.sno, products_category.Categoryname,invmaster.Qty FROM indents_subtable INNER JOIN (SELECT IndentNo, Branch_id, Status, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON indents_subtable.IndentNo = indent.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN (SELECT branch_sno, product_sno, unitprice, flag, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno RIGHT OUTER JOIN modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno ON indent.Branch_id = branchdata.sno WHERE (branchdata.flag=@flag) AND (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @endtime) OR (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY branchdata.sno, productsdata.sno ORDER BY brnchprdt.Rank");
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            }
            cmd.Parameters.AddWithValue("@RouteID", routeid);
            cmd.Parameters.AddWithValue("@flag", "1");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
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
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
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
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            DataTable dtCashcollection = vdm.SelectQuery(cmd).Tables[0];

            if (dtble.Rows.Count > 0)
            {
                DataView view = new DataView(dtble);
                DataTable produtstbl = view.ToTable(true, "ProductName", "Categoryname");
                Report = new DataTable();
                Report.Columns.Add("Sno");
                Report.Columns.Add("DC No");
                Report.Columns.Add("Agent Name");
                int count = 0;
                foreach (DataRow dr in produtstbl.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                    count++;
                }
                Report.Columns.Add("Total Sale", typeof(Double)).SetOrdinal(count + 3);
                Report.Columns.Add("Sale Value", typeof(Double)).SetOrdinal(count + 4);
                Report.Columns.Add("Paid Amount", typeof(Double)).SetOrdinal(count + 5);
                Report.Columns.Add("Due Amount", typeof(Double)).SetOrdinal(count + 6);
                Report.Columns.Add("Total Due Amount", typeof(Double)).SetOrdinal(count + 7);
                Report.Columns.Add("Crates").SetOrdinal(count + 8);
                Report.Columns.Add("Cans").SetOrdinal(count + 9);
                Report.Columns.Add("Distributor/AgentSignature").SetOrdinal(count +10);
                DataTable distincttable = view.ToTable(true, "BranchName", "BSno");
                int i = 1;
                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i;
                    string DCNO = "0";
                    string Crates = "0";
                    string Cans = "0";
                    //cmd = new MySqlCommand("SELECT DcNo FROM  agentdc WHERE (BranchID = @BranchID) AND (IndDate BETWEEN @d1 AND @d2)");
                    cmd = new MySqlCommand("SELECT agentdc.DcNo, inventory_monitor.Inv_Sno, inventory_monitor.Qty FROM agentdc INNER JOIN inventory_monitor ON agentdc.BranchID = inventory_monitor.BranchId WHERE  (agentdc.BranchID = @BranchID) AND (agentdc.IndDate BETWEEN @d1 AND @d2)");
                    cmd.Parameters.AddWithValue("@BranchID", branch["BSno"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    DataTable dtDc = vdm.SelectQuery(cmd).Tables[0];
                    if (dtDc.Rows.Count > 0)
                    {
                    string InvSno = "0";
                        DCNO = dtDc.Rows[0]["DcNo"].ToString();
                         InvSno = dtDc.Rows[0]["Inv_Sno"].ToString();
                        if (InvSno == "1")
                        {
                            Crates = dtDc.Rows[0]["Qty"].ToString();
                        }
                        if (InvSno == "4")
                        {
                            Cans = dtDc.Rows[0]["Qty"].ToString();
                        }
                    }
                    else
                    {
                    }
                    newrow["DC No"] = DCNO;
                    newrow["Crates"] = Crates;
                    newrow["Cans"] = Cans;
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
                            if (DeliveryQty == 0)
                            {
                            }
                            else
                            {
                                double UnitCost = 0;
                                double.TryParse(dr["UnitCost"].ToString(), out UnitCost);
                                if (dr["ProductName"].ToString() == "")
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
                            }
                            totalSale += Amount;
                        }
                    }
                    newrow["Total Sale"] = total;
                    newrow["Sale Value"] = totalSale;
                    newrow["Paid Amount"] = Math.Round(totamountpaid, 2);
                    newrow["Due Amount"] = Math.Round((totalSale - totamountpaid), 2);
                    newrow["Total Due Amount"] = Math.Round(Finaldue, 2);
                    Report.Rows.Add(newrow);
                    i++;
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
                pnlHide.Visible = false;
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