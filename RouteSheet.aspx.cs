using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class RouteSheet : System.Web.UI.Page
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
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
            }
        }


    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
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
                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID) AND (dispatch.flag=@flag)) OR ((branchdata_1.SalesOfficeID = @SOID) AND (dispatch.flag=@flag))");
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
        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID and dispatch.flag <>0) OR (branchdata_1.SalesOfficeID = @SOID  and dispatch.flag <>0)");
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
    DateTime fromdate = DateTime.Now;

    string routeid = "";
    string routeitype = "";
    void GetReport()
    {
        try
        {

            lblmsg.Text = "";
            pnlHide.Visible = true;
            Session["RouteName"] = ddlRouteName.SelectedItem.Text;
            lblRouteName.Text = ddlRouteName.SelectedItem.Text;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
            DataTable Report = new DataTable();

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
            lblDate.Text = fromdate.AddDays(1).ToString("dd/MMM/yyyy");
            Session["filename"] = ddlRouteName.SelectedItem.Text + fromdate.AddDays(1).ToString("dd/MM/yyyy");
            cmd = new MySqlCommand("select Route_id,IndentType from dispatch_sub where dispatch_sno=@dispsno");

            cmd.Parameters.AddWithValue("@dispsno", ddlRouteName.SelectedValue);
            DataTable dtrouteindenttype = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow drrouteitype in dtrouteindenttype.Rows)
            {
                routeid = drrouteitype["Route_id"].ToString();
                routeitype = drrouteitype["IndentType"].ToString();
            }
            // cmd = new MySqlCommand("SELECT branchroutes.RouteName, branchproducts.Rank, indents_subtable.UnitCost, indents_subtable.unitQty, indents.IndentType, productsdata.ProductName, branchdata.BranchName, productsdata.Units, productsdata.sno, products_category.Categoryname, invmaster.Qty, inventory_monitor.Qty AS invopening FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN indents ON branchdata.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON products_subcategory.sno = productsdata.SubCat_sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno INNER JOIN inventory_monitor ON branchdata.sno = inventory_monitor.BranchId  WHERE (branchroutes.Sno = @TripID) AND (indents.I_date BETWEEN @starttime AND @endtime) AND (indents.Status <> 'D') AND (indents.IndentType = @itype) AND (branchproducts.branch_sno = @BranchID) GROUP BY productsdata.ProductName, branchdata.BranchName, productsdata.sno, products_category.Categoryname ORDER BY branchproducts.Rank");
            cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, brnchprdt.Rank, modifiedroutesubtable.Rank AS RouteRank, indents_subtable.pkt_rate as UnitCost, indents_subtable.pkt_qty as unitQty, indent.IndentType, productsdata.ProductName, productsdata.Qty as uomqty,branchdata.BranchName, productsdata.Units, productsdata.sno, products_category.Categoryname,products_category.sno as catid, invmaster.Qty, inventory_monitor.Qty AS invopening FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, I_date, IndentType, Status FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN  (SELECT branch_sno, product_sno, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno INNER JOIN inventory_monitor ON branchdata.sno = inventory_monitor.BranchId WHERE (modifiedroutes.Sno = @TripID) AND (indent.Status <> 'D') AND (indent.IndentType = @itype) AND (brnchprdt.branch_sno = @BranchID) AND  (modifiedroutesubtable.EDate IS NULL) OR (modifiedroutes.Sno = @TripID) AND (indent.Status <> 'D') AND (indent.IndentType = @itype) AND (brnchprdt.branch_sno = @BranchID) AND  (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate < @starttime) GROUP BY productsdata.ProductName, branchdata.BranchName, productsdata.sno, products_category.Categoryname ORDER BY brnchprdt.Rank, RouteRank");
            cmd.Parameters.AddWithValue("@TripID", routeid);
            if (Session["salestype"].ToString() == "Plant")
            {
                if (ddlSalesOffice.SelectedValue == "572")
                {
                    cmd.Parameters.AddWithValue("@BranchID", 158);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            }
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@itype", routeitype);
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            
            cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, brnchprdt.Rank, modifiedroutesubtable.Rank AS RouteRank, offer_indents_sub.unit_price as UnitCost , offer_indents_sub.offer_indent_qty AS unitQty, offerindents.IndentType, productsdata.ProductName, productsdata.Qty as uomqty,branchdata.BranchName, productsdata.Units, productsdata.sno, products_category.Categoryname,products_category.sno as catid, invmaster.Qty, inventory_monitor.Qty AS invopening FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN(SELECT idoffer_indents, agent_id, indent_date, IndentType FROM offer_indents WHERE (indent_date BETWEEN @starttime AND @endtime)) offerindents ON branchdata.sno = offerindents.agent_id INNER JOIN offer_indents_sub ON offerindents.idoffer_indents = offer_indents_sub.idoffer_indents INNER JOIN productsdata ON offer_indents_sub.Product_id = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN(SELECT branch_sno, product_sno, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno INNER JOIN inventory_monitor ON branchdata.sno = inventory_monitor.BranchId WHERE(modifiedroutes.Sno = @TripID) AND(offerindents.IndentType = @itype) AND(brnchprdt.branch_sno = @BranchID) AND(modifiedroutesubtable.EDate IS NULL) OR(modifiedroutes.Sno = @TripID)  AND(offerindents.IndentType = @itype) AND(brnchprdt.branch_sno = @BranchID) AND(modifiedroutesubtable.EDate > @starttime) AND(modifiedroutesubtable.CDate < @starttime) GROUP BY productsdata.ProductName, branchdata.BranchName, productsdata.sno, products_category.Categoryname ORDER BY brnchprdt.Rank, RouteRank");
            // cmd = new MySqlCommand("SELECT modifiedroutes.RouteName,offer_indents_sub.unit_price as UnitCost, ROUND(SUM(offer_indents_sub.offer_indent_qty), 2) AS unitQty, productsdata.ProductName, productsdata.Units, products_category.Categoryname, brnchprdt.Rank, invmaster.Qty,productsdata.sno AS productid,productsdata.Qty as UOM FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutes ON dispatch_sub.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT idoffer_indents, idoffers_assignment, salesoffice_id, route_id, agent_id, indent_date, indents_id, IndentType, I_modified_by FROM offer_indents WHERE (indent_date BETWEEN @starttime AND @endtime) AND (IndentType = @itype)) offerindents ON modifiedroutesubtable.BranchID = offerindents.agent_id INNER JOIN offer_indents_sub ON offerindents.idoffer_indents = offer_indents_sub.idoffer_indents INNER JOIN productsdata ON offer_indents_sub.product_id = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN (SELECT branch_sno, product_sno, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno WHERE (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY modifiedroutes.RouteName, productsdata.sno ORDER BY brnchprdt.Rank");
            cmd.Parameters.AddWithValue("@TripID", routeid);
            if (Session["salestype"].ToString() == "Plant")
            {
                if (ddlSalesOffice.SelectedValue == "572")
                {
                    cmd.Parameters.AddWithValue("@BranchID", 158);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            }
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@itype", routeitype);
            DataTable dt_offertble = vdm.SelectQuery(cmd).Tables[0];
            if (dtble.Rows.Count > 0)
            {
                DataView view = new DataView(dtble);
                DataTable produtstbl = view.ToTable(true, "ProductName", "Categoryname", "Units", "Qty", "catid");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("Agent Name");
                int count = 0;
                int ColCount = 0;
                foreach (DataRow dr in produtstbl.Rows)
                {

                    //string[] catarr = { "1", "2", "7", "11", "12", "14", "15", "16", "22", "26", "34", "35", "39", "40", "47", "48" };
                    //if (catarr.Contains(dr["catid"].ToString()))
                    //{
                        //}
                        //    if (dr["Categoryname"].ToString() == "MILK" || dr["Categoryname"].ToString() == "CURD" || dr["Categoryname"].ToString() == "ButterMilk" || dr["Categoryname"].ToString() == "Curd Buckets" || dr["Categoryname"].ToString() == "Curd Cups" || dr["Categoryname"].ToString() == "Curd Cups" || dr["Categoryname"].ToString() == "Curd Cups")
                        //{
                        //Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                        //count++;
                        //ColCount++;
                    //}
                    //else
                    //{
                        Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                        //ColCount++;
                    //}
                }
                Report.Columns.Add("TOTAL Milk INDENT(ltr/pkts)", typeof(Double));//SetOrdinal(count + 2);
                Report.Columns.Add("TOTAL Curd INDENT(ltr/pkts)", typeof(Double));//SetOrdinal(count + 2);
                Report.Columns.Add("Total Sales Amount", typeof(Double));//.SetOrdinal(ColCount + 3);
                Report.Columns.Add("Issued Crates", typeof(Double));//.SetOrdinal(ColCount + 4);
                Report.Columns.Add("Recieved Crates", typeof(Double));//SetOrdinal(ColCount + 5);
                Report.Columns.Add("Issued 40 ltr Cans", typeof(Double));//SetOrdinal(ColCount + 6);
                Report.Columns.Add("Issued 20 ltr Cans", typeof(Double));//SetOrdinal(ColCount + 7);
                Report.Columns.Add("Issued 10 ltr Cans", typeof(Double));//SetOrdinal(ColCount + 8);
                Report.Columns.Add("Recieved 40 ltr Cans", typeof(Double));//SetOrdinal(ColCount + 9);
                Report.Columns.Add("Recieved 20 ltr Cans", typeof(Double));//SetOrdinal(ColCount + 10);
                Report.Columns.Add("Recieved 10 ltr Cans", typeof(Double));//SetOrdinal(ColCount + 11);
                DataTable distincttable = view.ToTable(true, "BranchName");
                int i = 1;
                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    newrow["Agent Name"] = branch["BranchName"].ToString();

                    double milktotal = 0;
                    double curdtotal = 0;
                    string totalcr = "";
                    string branchopening = "";
                    double TotalAmpunt = 0;
                    double TotalOfferAmpunt = 0;
                    double offermilkqty = 0;
                    double offercurdqty = 0;

                    foreach (DataRow dr in dtble.Rows)
                    {
                        double indandofferQty = 0;
                        double offerqty = 0;
                        double UnitQty = 0;
                        double UnitCost = 0;
                        double offerUnitCost = 0;
                        double TotOffercost = 0;
                        double Totcost = 0;

                        if (branch["BranchName"].ToString() == dr["BranchName"].ToString())
                        {
                            foreach (DataRow droffer in dt_offertble.Select("ProductName='" + dr["ProductName"].ToString() + "'AND BranchName='" + dr["BranchName"].ToString() + "'"))
                            {
                                double.TryParse(droffer["unitQty"].ToString(), out offerqty);
                                double.TryParse(droffer["UnitCost"].ToString(), out offerUnitCost);
                                TotOffercost = offerqty * offerUnitCost;
                                if (dr["Categoryname"].ToString() == "MILK")
                                {
                                    double.TryParse(droffer["unitQty"].ToString(), out offermilkqty);
                                    milktotal += offermilkqty;
                                }
                                if (dr["Categoryname"].ToString() == "CURD" || dr["Categoryname"].ToString() == "Curd Buckets" || dr["Categoryname"].ToString() == "Curd Cups" || dr["Categoryname"].ToString() == "ButterMilk")
                                {
                                    double.TryParse(droffer["unitQty"].ToString(), out offercurdqty);
                                    curdtotal += offercurdqty;
                                }
                            }
                            TotalOfferAmpunt += TotOffercost;

                            double qtyvalue = 0;
                            double invqtys = 0;
                            //newrow[dr["ProductName"].ToString()] = dr["unitQty"].ToString();
                            double.TryParse(dr["unitQty"].ToString(), out UnitQty);
                            double.TryParse(dr["UnitCost"].ToString(), out UnitCost);
                            Totcost = UnitQty * UnitCost;
                            double uom = 0;

                            if (dr["Categoryname"].ToString() == "MILK")
                            {
                                double.TryParse(dr["unitQty"].ToString(), out qtyvalue);
                                double.TryParse(dr["uomqty"].ToString(), out uom);
                                //double milkqty = qtyvalue * uom / 1000;
                                milktotal += qtyvalue;
                            }
                            if (dr["Categoryname"].ToString() == "CURD" || dr["Categoryname"].ToString() == "Curd Buckets" || dr["Categoryname"].ToString() == "Curd Cups" || dr["Categoryname"].ToString() == "ButterMilk")
                            {
                                double.TryParse(dr["unitQty"].ToString(), out qtyvalue);
                                double.TryParse(dr["uomqty"].ToString(), out uom);
                                //double curdqty = qtyvalue * uom / 1000;
                                curdtotal += qtyvalue;
                            }
                        }
                        indandofferQty = UnitQty + offerqty;
                        newrow[dr["ProductName"].ToString()] = indandofferQty;
                        TotalAmpunt += Totcost;
                    }
                    totalcr = milktotal.ToString();
                    newrow["TOTAL Milk INDENT(ltr/pkts)"] = milktotal;
                    newrow["TOTAL Curd INDENT(ltr/pkts)"] = curdtotal;
                    newrow["Total Sales Amount"] = TotalOfferAmpunt + TotalAmpunt;
                    //newrow["Issued Crates/Cans"] = indenttubs + '/' + indentcans;
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
                        if (val == 0)
                        {
                            // Report.Columns.;
                        }
                    }
                }
                Report.Rows.Add(newvartical);
                DataRow Break = Report.NewRow();
                Break["Agent Name"] = "";
                Report.Rows.Add(Break);
                DataRow newInventory = Report.NewRow();
                newInventory["Agent Name"] = "CRATES";
                double TotCreatsQty = 0;
                double TotCansQty = 0;

                foreach (DataRow dr in produtstbl.Rows)
                {
                    var lastRow = Report.Rows[Report.Rows.Count - 2][dr["ProductName"].ToString()];
                    float First = 0;
                    float.TryParse(lastRow.ToString(), out First);
                    float Qty = 0;
                    float.TryParse(dr["Qty"].ToString(), out Qty);
                    double InvQty = 0;
                    InvQty = Math.Round(First / Qty);
                    if (dr[0].ToString() == "MILK")
                    {
                        if (dr["Qty"].ToString() != "12")
                        {
                        }
                        else
                        {
                            TotCreatsQty += InvQty;
                            newInventory[dr["ProductName"].ToString()] = InvQty;
                            newInventory["TOTAL INDENT"] = TotCreatsQty;
                        }
                    }
                    else
                    {
                        if (dr["Qty"].ToString() != "12")
                        {
                        }
                        else
                        {
                            if (dr["Units"].ToString() == "gms")
                            {
                                string branchid = Session["branch"].ToString();
                                if (branchid == "7")
                                {
                                    if (dr["ProductName"].ToString() == "C-CURD100")
                                    {
                                        //float.TryParse(drprdt["Qty"].ToString(), out invqty);
                                        newInventory[dr["ProductName"].ToString()] = Math.Round(First / 3.2);
                                    }
                                    if (dr["ProductName"].ToString() == "C-CURD200")
                                    {
                                        //float.TryParse(drprdt["Qty"].ToString(), out invqty);
                                        newInventory[dr["ProductName"].ToString()] = Math.Round(First / 4.8);
                                    }

                                }
                            }
                            if (dr["Units"].ToString() == "ml")
                            {
                                string branchid = Session["branch"].ToString();
                                if (branchid == "7")
                                {
                                    if (dr["ProductName"].ToString() == "CURD200")
                                    {
                                        newInventory[dr["ProductName"].ToString()] = Math.Round(First / 10);
                                    }
                                    else
                                    {
                                        newInventory[dr["ProductName"].ToString()] = Math.Round(First / Qty);
                                    }
                                }
                                else
                                {
                                    newInventory[dr["ProductName"].ToString()] = Math.Round(First / Qty);
                                }
                            }
                            else
                            {
                                newInventory[dr["ProductName"].ToString()] = InvQty;
                            }
                        }
                    }
                }
                Report.Rows.Add(newInventory);
                DataRow newInventoryCans = Report.NewRow();
                newInventoryCans["Agent Name"] = "CANS";
                foreach (DataRow dr in produtstbl.Rows)
                {
                    var lastRow = Report.Rows[Report.Rows.Count - 3][dr["ProductName"].ToString()];
                    float First = 0;
                    float.TryParse(lastRow.ToString(), out First);
                    float Qty = 0;
                    float.TryParse(dr["Qty"].ToString(), out Qty);
                    double InvQty = 0;
                    InvQty = Math.Round(First / Qty, 2);
                    //InvQty=  Math.Round(InvQty,2);
                    //InvQty = Math.Ceiling(InvQty);
                    if (dr[0].ToString() == "MILK")
                    {
                        if (dr["Qty"].ToString() != "12")
                        {
                            TotCansQty += InvQty;
                            newInventoryCans[dr["ProductName"].ToString()] = InvQty;
                            newInventoryCans["TOTAL INDENT"] = TotCansQty;
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        if (dr["Qty"].ToString() != "12")
                        {
                            TotCansQty += InvQty;
                            newInventoryCans[dr["ProductName"].ToString()] = InvQty;
                        }
                        else
                        {
                        }
                    }
                }
                Report.Rows.Add(newInventoryCans);

                DataRow newInventoryscheem = Report.NewRow();
                newInventoryscheem["Agent Name"] = "Offer";
                foreach (DataRow dr in produtstbl.Rows)
                {
                    var lastRow = Report.Rows[Report.Rows.Count - 4][dr["ProductName"].ToString()];
                    double itemwisetotal = 0;
                    string itemname = dr["ProductName"].ToString();
                    foreach (DataRow droffer in dt_offertble.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
                    {
                        double qty = 0;
                        double.TryParse(droffer["unitQty"].ToString(), out qty);
                        itemwisetotal += qty;
                    }
                    newInventoryscheem[dr["ProductName"].ToString()] = itemwisetotal;
                }
                Report.Rows.Add(newInventoryscheem);
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