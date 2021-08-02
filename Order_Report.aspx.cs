using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class Order_Report : System.Web.UI.Page
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
            }
            else
            {
                PBranch.Visible = false;

                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID) and (dispatch.flag=@flag)) OR ((branchdata_1.SalesOfficeID = @SOID) and (dispatch.flag=@flag))");
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
        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID) and (dispatch.flag=@flag)) OR ((branchdata_1.SalesOfficeID = @SOID) and (dispatch.flag=@flag))");
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
    DateTime fromdate = DateTime.Now;

    string routeid = "";
    string routeitype = "";
    DataTable Report = new DataTable();
    DataTable productsReport = new DataTable();
    DataTable FinalReport = new DataTable();

    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            lblmsg.Text = "";
            pnlHide.Visible = true;
            Session["RouteName"] = ddlRouteName.SelectedItem.Text;
            lblRouteName.Text = ddlRouteName.SelectedItem.Text;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            Report = new DataTable();
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
            string Branch = "";
            //cmd = new MySqlCommand("SELECT branchroutes.RouteName,branchproducts.Rank,branchroutesubtable.Rank as RouteRank,  indents_subtable.unitQty, indents.IndentType, productsdata.ProductName, branchdata.BranchName,productsdata.Units, productsdata.sno,products_category.Categoryname, invmaster.Qty FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN indents ON branchdata.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON products_subcategory.sno = productsdata.SubCat_sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno WHERE (branchroutes.Sno = @TripID) AND (indents.I_date BETWEEN @starttime AND @endtime) AND (indents.Status <> 'D') AND (indents.IndentType = @itype) and (branchproducts.branch_sno=@BranchID) GROUP BY productsdata.ProductName, branchdata.BranchName, productsdata.sno, products_category.Categoryname ORDER BY branchproducts.Rank,RouteRank");
            cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, brnchprdt.Rank, modifiedroutesubtable.Rank AS RouteRank, indents_subtable.unitQty, indent.IndentType, productsdata.ProductName,branchdata.BranchName,branchdata.SalesType, productsdata.Units, productsdata.qty as uomqty,productsdata.invqty, productsdata.sno, products_category.Categoryname, invmaster.Qty, modifiedroutesubtable.BranchID FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, I_date, IndentType, Status FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN (SELECT        branch_sno, product_sno, Rank FROM  branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno WHERE (modifiedroutes.Sno = @TripID) AND (indent.Status <> 'D') AND (indent.IndentType = @itype) AND (brnchprdt.branch_sno = @BranchID) AND  (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (modifiedroutes.Sno = @TripID) AND (indent.Status <> 'D') AND (indent.IndentType = @itype) AND (brnchprdt.branch_sno = @BranchID) AND  (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY productsdata.ProductName, branchdata.BranchName, productsdata.sno, products_category.Categoryname ORDER BY brnchprdt.Rank, RouteRank");
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
                Branch = ddlSalesOffice.SelectedValue;
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                Branch = Session["branch"].ToString();
            }
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@itype", routeitype);
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT modifiedroutesubtable.BranchID, branchaccounts.Amount, inventory_monitor.Qty,  inventory_monitor.Inv_Sno FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN inventory_monitor ON branchdata.sno = inventory_monitor.BranchId LEFT OUTER JOIN branchaccounts ON branchdata.sno = branchaccounts.BranchId WHERE (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY branchdata.BranchName, branchaccounts.Amount, inventory_monitor.Qty, inventory_monitor.Inv_Sno ");
            cmd.Parameters.AddWithValue("@RouteID", routeid);
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
                Branch = ddlSalesOffice.SelectedValue;
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                Branch = Session["branch"].ToString();
            }
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@itype", routeitype);
            DataTable dtAgentBal = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, brnchprdt.Rank, modifiedroutesubtable.Rank AS RouteRank, offer_indents_sub.offer_indent_qty AS unitQty, offerindent.IndentType, productsdata.ProductName, branchdata.BranchName, productsdata.Units,productsdata.Qty as uomqty,productsdata.invqty, productsdata.sno, products_category.Categoryname, invmaster.Qty, modifiedroutesubtable.BranchID FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT idoffer_indents, agent_id, indent_date, indents_id, IndentType, I_modified_by FROM offer_indents WHERE (indent_date BETWEEN @starttime AND @endtime)) offerindent ON branchdata.sno = offerindent.agent_id INNER JOIN offer_indents_sub ON offerindent.idoffer_indents = offer_indents_sub.idoffer_indents INNER JOIN productsdata ON offer_indents_sub.product_id = productsdata.sno INNER JOIN (SELECT branch_sno, product_sno, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno WHERE (modifiedroutes.Sno = @TripID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) AND (offerindent.IndentType = @itype) OR (modifiedroutes.Sno = @TripID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (offerindent.IndentType = @itype) GROUP BY branchdata.BranchName, modifiedroutesubtable.Rank, productsdata.sno, productsdata.ProductName, products_category.Categoryname ORDER BY RouteRank, brnchprdt.Rank");
            cmd.Parameters.AddWithValue("@TripID", routeid);
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            }
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@itype", routeitype);
            DataTable dtoffer = vdm.SelectQuery(cmd).Tables[0];

            DataView viewprdts = new DataView(dtble);
            DataView view_offer_prdts = new DataView(dtoffer);
            DataTable Finalprodutstbl = viewprdts.ToTable(true, "ProductName", "Categoryname", "Units", "Qty", "Rank", "uomqty", "invqty");
            DataTable offerprodutstbl = view_offer_prdts.ToTable(true, "ProductName", "Categoryname", "Units", "Qty", "Rank", "uomqty", "invqty");
            productsReport.Columns.Add("ProductName");
            productsReport.Columns.Add("Categoryname");
            productsReport.Columns.Add("Units");
            productsReport.Columns.Add("Qty");
            productsReport.Columns.Add("Rank");
            productsReport.Columns.Add("uomqty");
            productsReport.Columns.Add("invqty");
            foreach (DataRow drprdt in Finalprodutstbl.Rows)
            {
                DataRow newrow = productsReport.NewRow();
                newrow["ProductName"] = drprdt["ProductName"].ToString();
                newrow["Categoryname"] = drprdt["Categoryname"].ToString();
                newrow["Units"] = drprdt["Units"].ToString();
                newrow["Qty"] = drprdt["Qty"].ToString();
                newrow["Rank"] = drprdt["Rank"].ToString();
                newrow["uomqty"] = drprdt["uomqty"].ToString();
                newrow["invqty"] = drprdt["invqty"].ToString();
                productsReport.Rows.Add(newrow);
            }
            foreach (DataRow drprdt in offerprodutstbl.Rows)
            {
                DataRow[] data_exist = productsReport.Select("ProductName='" + drprdt["ProductName"].ToString() + "'");
                if (data_exist.Length > 0)
                {

                }
                else
                {
                    DataRow newrow = productsReport.NewRow();
                    newrow["ProductName"] = drprdt["ProductName"].ToString();
                    newrow["Categoryname"] = drprdt["Categoryname"].ToString();
                    newrow["Units"] = drprdt["Units"].ToString();
                    newrow["Qty"] = drprdt["Qty"].ToString();
                    newrow["Rank"] = drprdt["Rank"].ToString();
                    productsReport.Rows.Add(newrow);
                }

            }
            if (dtble.Rows.Count > 0)
            {
                DataView view = new DataView(dtble);
                DataTable produtstbl = view.ToTable(true, "ProductName", "Categoryname", "Units", "Qty");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("Agent Name");
                Report.Columns.Add("Crates Bal");
                Report.Columns.Add("Cans Bal");
                Report.Columns.Add("Bal Amount");
                int count = 0;
                productsReport.DefaultView.Sort = "Rank ASC";
                foreach (DataRow dr in productsReport.Rows)
                {

                    if (dr["Categoryname"].ToString() == "MILK")
                    {
                        Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                        count++;
                    }
                    else
                    {
                        Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                    }
                }
                Report.Columns.Add("Total Indent", typeof(Double)).SetOrdinal(count + 5);
                DataTable distincttable = view.ToTable(true, "BranchID", "BranchName", "RouteRank", "SalesType");
                distincttable.DefaultView.Sort = "RouteRank ASC";
                distincttable = distincttable.DefaultView.ToTable(true);
                int i = 1;
                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    newrow["Agent Name"] = branch["BranchName"].ToString();

                    string SalesType = branch["SalesType"].ToString();
                    DataRow[] dragentamount = dtAgentBal.Select("BranchID=" + branch["BranchID"].ToString());
                    int ivvbal = 0;

                    foreach (DataRow drc in dragentamount)
                    {
                        double amount = 0;
                        double.TryParse(drc.ItemArray[1].ToString(), out amount);
                        if (SalesType == "20")
                        {
                            newrow["Bal Amount"] = amount.ToString();
                        }
                        string invsno = drc.ItemArray[3].ToString();
                        if (invsno == "1")
                        {
                            newrow["Crates Bal"] = drc.ItemArray[2].ToString();
                        }
                        if (invsno == "2" || invsno == "3" || invsno == "4")
                        {
                            int invqty = 0;
                            int.TryParse(drc.ItemArray[2].ToString(), out invqty);
                            ivvbal += invqty;
                            newrow["Cans Bal"] = ivvbal.ToString();
                        }
                    }
                    double total = 0;
                    foreach (DataRow dr in productsReport.Rows)
                    {
                        double offerqty = 0;
                        double indqty = 0;
                        double totindqty = 0;

                        foreach (DataRow drindent in dtble.Select("ProductName='" + dr["ProductName"].ToString() + "' AND BranchID='" + branch["BranchID"].ToString() + "'"))
                        {
                            ////if (drindent["Units"].ToString() == "Pkts")
                            ////{
                            ////    double unitQty = 0;
                            ////    double uomqty = 0;
                            ////    double.TryParse(drindent["uomqty"].ToString(), out uomqty);
                            ////    double.TryParse(drindent["unitQty"].ToString(), out unitQty);
                            ////    indqty = unitQty * uomqty / 1000;
                            ////}
                            ////else
                            ////{
                                double.TryParse(drindent["unitQty"].ToString(), out indqty);
                            ////}
                        }
                        foreach (DataRow droffer in dtoffer.Select("ProductName='" + dr["ProductName"].ToString() + "' AND BranchID='" + branch["BranchID"].ToString() + "'"))
                        {
                            ////if (droffer["Units"].ToString() == "Pkts")
                            ////{
                            ////    double unitQty = 0;
                            ////    double uomqty = 0;
                            ////    double.TryParse(droffer["uomqty"].ToString(), out uomqty);
                            ////    double.TryParse(droffer["unitQty"].ToString(), out unitQty);
                            ////    offerqty = unitQty * uomqty / 1000;
                            ////}
                            ////else
                            ////{
                                double.TryParse(droffer["unitQty"].ToString(), out offerqty);
                            ////}
                        }
                        totindqty = indqty + offerqty;
                        if (totindqty == 0)
                        {
                        }
                        else
                        {
                            newrow[dr["ProductName"].ToString()] = totindqty;

                        }
                        if (dr["Categoryname"].ToString() == "MILK")
                        {
                            total += totindqty;
                        }
                    }
                    newrow["Total Indent"] = total;
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
                DataRow Break = Report.NewRow();
                Break["Agent Name"] = "";
                Report.Rows.Add(Break);
                DataRow newInventory = Report.NewRow();
                newInventory["Agent Name"] = "CRATES";
                double TotCreatsQty = 0;
                double TotCansQty = 0;

                foreach (DataRow dr in productsReport.Rows)
                {
                    var lastRow = Report.Rows[Report.Rows.Count - 2][dr["ProductName"].ToString()];
                    double First = 0;
                    double.TryParse(lastRow.ToString(), out First);
                    double test = 0;
                    double.TryParse(lastRow.ToString(), out test);
                    double InvUomQty =0;
                    double.TryParse(dr["InvQty"].ToString(), out InvUomQty);
                    double UomQty = 0;
                    double.TryParse(dr["UomQty"].ToString(), out UomQty);
                    double finalqty = 0;
                    finalqty = (InvUomQty * UomQty) / 1000;
                    double InvQty = 0;
                    InvQty = First / finalqty;
                    int result = 0;
                    double quotient = 0;
                    if (First != 0)
                    {
                       quotient = Math.Floor(InvQty); ;// Math.DivRem(First, Qty, out result);

                    }
                    if (First == 0)
                    {
                        //int ffqty = 0;
                        //int.TryParse(InvQty.ToString(), out ffqty);
                        //quotient = Math.DivRem((int)test, ffqty, out result);

                    }
                    if (dr["Categoryname"].ToString() == "MILK")
                    {
                        if (dr["Qty"].ToString() != "12")
                        {
                        }
                        else
                        {
                            TotCreatsQty += quotient;
                            newInventory[dr["ProductName"].ToString()] = quotient;
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
                            }
                            else
                            {
                                newInventory[dr["ProductName"].ToString()] = quotient;
                            }
                        }
                    }
                }
                Report.Rows.Add(newInventory);
                DataRow newExtraLtrs = Report.NewRow();
                newExtraLtrs["Agent Name"] = "Extra Ltrs";
                double totextraltrs = 0;
                foreach (DataRow dr in produtstbl.Rows)
                {
                    var lastRow = Report.Rows[Report.Rows.Count - 3][dr["ProductName"].ToString()];
                    int First = 0;
                    int.TryParse(lastRow.ToString(), out First);
                    int Qty = 0;
                    int.TryParse(dr["Qty"].ToString(), out Qty);
                    int InvQty = 0;
                    InvQty = First / Qty;
                    int result = 0;
                    float quotient = Math.DivRem(First, Qty, out result);
                    if (dr["Categoryname"].ToString() == "MILK")
                    {
                        if (dr["Qty"].ToString() != "12")
                        {
                        }
                        else
                        {
                            totextraltrs += result;
                            newExtraLtrs[dr["ProductName"].ToString()] = result;
                            newExtraLtrs["TOTAL INDENT"] = totextraltrs;
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
                            }
                            else
                            {
                                newExtraLtrs[dr["ProductName"].ToString()] = result;
                            }
                        }
                    }
                }
                Report.Rows.Add(newExtraLtrs);
                DataRow newInventoryCans = Report.NewRow();
                newInventoryCans["Agent Name"] = "CANS";
                foreach (DataRow dr in dtble.Rows)
                {
                    var lastRow = Report.Rows[Report.Rows.Count - 4][dr["ProductName"].ToString()];
                    float First = 0;
                    float.TryParse(lastRow.ToString(), out First);
                    float Qty = 0;
                    float.TryParse(dr["Qty"].ToString(), out Qty);
                    double InvQty = 0;
                    InvQty = Math.Round(First / Qty, 2);
                    if (dr["Categoryname"].ToString() == "MILK")
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