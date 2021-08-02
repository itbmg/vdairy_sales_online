using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class RouteWiseDelivery : System.Web.UI.Page
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
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
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
            ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            PBranch.Visible = true;
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE  (branchdata.sno = @BranchID) AND (branchdata.SalesType IS NOT NULL)");
            cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            ddlSalesOffice.DataSource = dtRoutedata;
            ddlSalesOffice.DataTextField = "BranchName";
            ddlSalesOffice.DataValueField = "sno";
            ddlSalesOffice.DataBind();
            ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID)  AND (Dispmode is NULL) AND (dispatch.flag=@flag)) OR ((branchdata_1.SalesOfficeID = @SOID)  AND (Dispmode is NULL) AND (dispatch.flag=@flag))");
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
            cmd = new MySqlCommand("SELECT branchdata.BranchName, modifiedroutes.RouteName, branchdata.sno AS BSno, indent.IndentType, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,indents_subtable.UnitCost, productsdata.ProductName, productsdata.Units, productsdata.sno, products_category.Categoryname, invmaster.Qty,brnchprdt.Rank FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, I_date, Status, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno INNER JOIN (SELECT branch_sno, product_sno, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno WHERE (modifiedroutes.Sno = @TripID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) AND (indents_subtable.DeliveryQty > 0) OR (modifiedroutes.Sno = @TripID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (indents_subtable.DeliveryQty > 0) GROUP BY productsdata.sno, BSno ORDER BY brnchprdt.Rank");
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            }
            cmd.Parameters.AddWithValue("@TripID", routeid);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT branchdata.sno,branchdata.branchcode,branchdata.companycode,  branchdata.BranchName, statemastar.statename ,statemastar.statecode, statemastar.statecode , statemastar.gststatecode FROM branchdata INNER JOIN statemastar ON branchdata.stateid = statemastar.sno WHERE (branchdata.sno = @BranchID)");
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            }
            DataTable dtstatename = vdm.SelectQuery(cmd).Tables[0];
            string statename = "";
            string statecode = "";
            string branchcode = "";
            string gststatecode = "";
            string companycode = "";
            if (dtstatename.Rows.Count > 0)
            {
                statename = dtstatename.Rows[0]["statename"].ToString();
                statecode = dtstatename.Rows[0]["statecode"].ToString();
                branchcode = dtstatename.Rows[0]["branchcode"].ToString();
                gststatecode = dtstatename.Rows[0]["gststatecode"].ToString();
                companycode = dtstatename.Rows[0]["companycode"].ToString();
            }

            cmd = new MySqlCommand("SELECT branchdata.BranchName, modifiedroutes.RouteName, branchdata.sno AS BSno, indent.IndentType, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,indents_subtable.UnitCost, productsdata.ProductName, productsdata.Units, productsdata.sno, products_category.Categoryname, invmaster.Qty,brnchprdt.Rank FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, I_date, Status, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno INNER JOIN (SELECT branch_sno, product_sno, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno WHERE (modifiedroutes.Sno = @TripID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (modifiedroutes.Sno = @TripID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime)");
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            }
            cmd.Parameters.AddWithValue("@TripID", routeid);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtsum = vdm.SelectQuery(cmd).Tables[0];
            double totsum = 0;

            if (dtsum.Rows.Count > 0)
            {
                string sum = dtsum.Rows[0]["DeliveryQty"].ToString();
                double.TryParse(sum, out totsum);
            }
            DateTime ReportDate = VehicleDBMgr.GetTime(vdm.conn);
            DateTime dtapril = new DateTime();
            DateTime dtmarch = new DateTime();
            int currentyear = ReportDate.Year;
            int nextyear = ReportDate.Year + 1;
            if (ReportDate.Month > 3)
            {
                string apr = "4/1/" + currentyear;
                dtapril = DateTime.Parse(apr);
                string march = "3/31/" + nextyear;
                dtmarch = DateTime.Parse(march);
            }
            if (ReportDate.Month <= 3)
            {
                string apr = "4/1/" + (currentyear - 1);
                dtapril = DateTime.Parse(apr);
                string march = "3/31/" + (nextyear - 1);
                dtmarch = DateTime.Parse(march);
            }
            if (dtble.Rows.Count > 0)
            {
                DataView view = new DataView(dtble);
                DataTable produtstbl = view.ToTable(true, "ProductName", "Categoryname");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("DC No");
                Report.Columns.Add("Agent Name");
                int count = 0;
                foreach (DataRow dr in produtstbl.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                    count++;
                }
                Report.Columns.Add("Total Sale").DataType = typeof(Double);
                Report.Columns.Add("Sale Value").DataType = typeof(Double);
                DataTable distincttable = view.ToTable(true, "BranchName", "BSno");
                int i = 1;
                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    string DCNO = "0";
                    cmd = new MySqlCommand("SELECT agentdcno FROM  agentdc WHERE (BranchID = @BranchID) AND (IndDate BETWEEN @d1 AND @d2)");
                    cmd.Parameters.AddWithValue("@BranchID", branch["BSno"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    DataTable dtDc = vdm.SelectQuery(cmd).Tables[0];
                    if (dtDc.Rows.Count > 0)
                    {
                        DCNO = dtDc.Rows[0]["agentdcno"].ToString();
                    }
                    else
                    {
                        DCNO = "0";
                        if (totsum > 0)
                        {
                            long DcNo = 0;
                            string socode = "";
                            if (ReportDate.ToString("dd/MM/yyyy") == fromdate.ToString("dd/MM/yyyy"))
                            {
                                cmd = new MySqlCommand("SELECT IFNULL(MAX(agentdcno), 0) + 1 AS Sno FROM agentdc WHERE (soid = @soid)  AND (IndDate BETWEEN @d1 AND @d2)");
                                cmd.Parameters.AddWithValue("@soid", ddlSalesOffice.SelectedValue);
                                cmd.Parameters.AddWithValue("@d1", GetLowDate(dtapril.AddDays(-1)));
                                cmd.Parameters.AddWithValue("@d2", GetHighDate(dtmarch.AddDays(-1)));
                                DataTable dtadcno = vdm.SelectQuery(cmd).Tables[0];
                                string agentdcNo = dtadcno.Rows[0]["Sno"].ToString();
                                //cmd = new MySqlCommand("Insert Into Agentdc (BranchId,IndDate,soid,agentdcno,stateid,companycode,moduleid,doe,invoicetype) Values(@BranchId,@IndDate,@soid,@agentdcno,@stateid,@companycode,@moduleid,@doe,@invoicetype)");
                                //cmd.Parameters.AddWithValue("@BranchId", branch["BSno"].ToString());
                                //cmd.Parameters.AddWithValue("@IndDate", GetLowDate(fromdate.AddDays(-1)));
                                //cmd.Parameters.AddWithValue("@soid", ddlSalesOffice.SelectedValue);
                                //cmd.Parameters.AddWithValue("@agentdcno", agentdcNo);
                                //cmd.Parameters.AddWithValue("@stateid", gststatecode);
                                //cmd.Parameters.AddWithValue("@companycode", companycode);
                                //cmd.Parameters.AddWithValue("@doe", ReportDate);
                                //cmd.Parameters.AddWithValue("@moduleid", Session["moduleid"].ToString());
                                //cmd.Parameters.AddWithValue("@invoicetype", "RWDelivery");
                                //DcNo = vdm.insertScalar(cmd);
                                cmd = new MySqlCommand("SELECT IndentNo FROM indents WHERE (Branch_id = @BranchId) AND (I_date BETWEEN @d1 AND @d2)");
                                cmd.Parameters.AddWithValue("@BranchId", branch["BSno"].ToString());
                                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                                DataTable dtindentno = vdm.SelectQuery(cmd).Tables[0];
                                if (dtindentno.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtindentno.Rows)
                                    {
                                        cmd = new MySqlCommand("Insert Into dcsubTable (DcNo,IndentNo) Values(@DcNo,@IndentNo)");
                                        cmd.Parameters.AddWithValue("@DcNo", DcNo);
                                        cmd.Parameters.AddWithValue("@IndentNo", dr["IndentNo"].ToString());
                                        vdm.insert(cmd);
                                    }
                                }
                                cmd = new MySqlCommand("SELECT agentdcno FROM  agentdc WHERE (BranchID = @BranchID) AND (IndDate BETWEEN @d1 AND @d2)");
                                cmd.Parameters.AddWithValue("@BranchID", branch["BSno"].ToString());
                                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                                DataTable dtagentdcnoDc = vdm.SelectQuery(cmd).Tables[0];
                                if (dtagentdcnoDc.Rows.Count > 0)
                                {
                                    DCNO = dtagentdcnoDc.Rows[0]["agentdcno"].ToString();
                                }
                            }
                        }
                    }
                    int countdc = 0;
                    int.TryParse(DCNO, out countdc);

                    if (countdc <= 10)
                    {
                        DCNO = "0000" + countdc;
                    }
                    if (countdc >= 10 && countdc <= 99)
                    {
                        DCNO = "000" + countdc;
                    }
                    if (countdc >= 99 && countdc <= 999)
                    {
                        DCNO = "00" + countdc;
                    }
                    if (countdc > 999)
                    {
                        DCNO = "00" + countdc;
                    }
                    if (fromdate.Month > 3)
                    {
                        newrow["DC No"] = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                    }
                    else
                    {
                        newrow["DC No"] = branchcode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                    }
                    newrow["Agent Name"] = branch["BranchName"].ToString();
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
                    newrow["Sale Value"] = totalSale;
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