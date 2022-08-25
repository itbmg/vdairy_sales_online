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
using System.Drawing;
using System.Xml;

public partial class EinoviceTemplate : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    string status = "";
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
                lblTitle.Text = Session["TitleName"].ToString();
            }
        }
    }
    void FillSalesOffice()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Group")
            {
                PPlant.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchdata.flag=1) AND (branchmappingtable.SuperBranch = @SuperBranch) ");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlPlant.DataSource = dtRoutedata;
                ddlPlant.DataTextField = "BranchName";
                ddlPlant.DataValueField = "sno";
                ddlPlant.DataBind();
                ddlPlant.Items.Insert(0, new ListItem("Select Plant", "0"));
            }
            else if (Session["salestype"].ToString() == "Plant")
            {
                PBranch.Visible = true;
                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("BranchName");
                dtBranch.Columns.Add("sno");
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchdata.flag=1) AND (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
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
                PBranch.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.sno = @BranchID) AND (branchdata.SalesType IS NOT NULL)");
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
    protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        PBranch.Visible = true;
        cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchdata.flag=1) AND (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
        cmd.Parameters.AddWithValue("@SuperBranch", ddlPlant.SelectedValue);
        cmd.Parameters.AddWithValue("@SalesType", "21");
        cmd.Parameters.AddWithValue("@SalesType1", "26");
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlSalesOffice.DataSource = dtRoutedata;
        ddlSalesOffice.DataTextField = "BranchName";
        ddlSalesOffice.DataValueField = "sno";
        ddlSalesOffice.DataBind();

    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        pnlHide.Visible = true;
        //pnlExcel.Visible = true;
        //pnlSms.Visible = true;
        DateTime fromdate = DateTime.Now;
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
        if (Session["salestype"].ToString() == "Plant")
        {
            var salesoffice = ddlSalesOffice.SelectedItem.Text;
            var salesoff = ddlSalesOffice.SelectedValue;

            getnelloreReport();
        }
        else
        {
            status = "Nellore";
            getnelloreReport();
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
    DataTable dtalldispatch = new DataTable();
    DataTable produtstbl = new DataTable();
    DataTable dtalldelivery = new DataTable();
    DataTable Report = new DataTable();
    DataTable produtstbl1 = new DataTable();
    DataTable dtSubCatgory = new DataTable();
    DataTable dtSortedSubCategory = new DataTable();
    DataTable dttempproducts = new DataTable();

    void getnelloreReport()
    {
        try
        {
            lblmsg.Text = "";
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Plant")
            {
                lblDispatchName.Text = ddlSalesOffice.SelectedItem.Text;
                lblDate.Text = txtdate.Text;
            }
            else
            {
                lblDispatchName.Text = "SALES OFFICE";
                lblDate.Text = txtdate.Text;
            }
            DateTime fromdate = DateTime.Now;
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
            Session["RouteName"] = ddlSalesOffice.SelectedItem.Text + " REPORT " + fromdate.AddDays(1).ToString("dd/MM/yyyy");
            Session["filename"] = ddlSalesOffice.SelectedItem.Text + " REPORT " + fromdate.AddDays(1).ToString("dd/MM/yyyy");
            string SalesOfficeID = ddlSalesOffice.SelectedValue;
            if (SalesOfficeID == "572")
            {
                SalesOfficeID = "7";
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
            TimeSpan datespan = ReportDate.Subtract(fromdate);
            int NoOfdays = datespan.Days;

            //cmd = new MySqlCommand("SELECT SUM(indents_subtable.DeliveryQty) AS DeliveryQty, indents_subtable.UnitCost, branchdata.BranchName,branchdata.sno, productsdata.sno AS prodsno, productsdata.ProductName FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN indents ON branchdata.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (branchroutes.BranchID = @BranchID) AND (branchdata.CollectionType <> 'DUE') AND (indents.I_date BETWEEN @starttime AND @endtime) GROUP BY prodsno, branchdata.sno ORDER BY branchdata.sno, prodsno");
            cmd = new MySqlCommand("SELECT  SUM(indents_subtable.DeliveryQty) AS DeliveryQty,productsdata.SubCat_sno, indents_subtable.UnitCost, branchdata.BranchName, branchdata.sno, productsdata.sno AS prodsno, productsdata.ProductName FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (modifiedroutes.BranchID = @BranchID)  AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (modifiedroutes.BranchID = @BranchID)  AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY prodsno, branchdata.sno ORDER BY branchdata.sno, prodsno");
            cmd.Parameters.AddWithValue("@BranchID", SalesOfficeID);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT  SUM(indents_subtable.DeliveryQty) AS DeliveryQty, ROUND(SUM(indents_subtable.DeliveryQty*indents_subtable.UnitCost),2) as salevalue, branchdata.sno FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (modifiedroutes.BranchID = @BranchID)  AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (modifiedroutes.BranchID = @BranchID)  AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY branchdata.sno ORDER BY branchdata.sno");
            cmd.Parameters.AddWithValue("@BranchID", SalesOfficeID);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtble1 = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("select products_category.sno as catsno,products_subcategory.sno as subcatsno from products_category inner join products_subcategory on products_category.sno = products_subcategory.category_sno");
            DataTable dtcategory = vdm.SelectQuery(cmd).Tables[0];
            if (dtble.Rows.Count > 0)
            {

                cmd = new MySqlCommand("SELECT branchproducts.Rank,productsdata.ProductName,productsdata.sno, products_category.Categoryname, productsdata.Units, productsdata.Qty,branchproducts.unitprice FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) ORDER BY branchproducts.Rank");
                cmd.Parameters.AddWithValue("@BranchID", SalesOfficeID);
                DataTable dttable = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT    productsdata.SubCat_sno,branchproducts.unitprice,branchproducts.branch_sno,products_subcategory.tempsub_catsno AS SubCatSno, products_category.description AS Categoryname, branchproducts.product_sno AS sno, productsdata.ProductName, branchproducts.Rank,products_subcategory.description AS SubCategoryName FROM  products_category INNER JOIN products_subcategory ON products_category.sno = products_subcategory.category_sno INNER JOIN productsdata ON products_subcategory.sno = productsdata.SubCat_sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno WHERE (branchproducts.branch_sno = @BranchId) AND (products_category.sno <>'1') AND (branchproducts.flag = @flag) ORDER BY products_subcategory.tempsub_catsno, branchproducts.Rank");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@flag", "1");
                produtstbl1 = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT branchdata.gstno,branchdata.sno,branchdata.Branchcode,branchdata.companycode,  branchdata.BranchName,branchdata.stateid, statemastar.statename, statemastar.statecode , statemastar.gststatecode FROM branchdata INNER JOIN statemastar ON branchdata.stateid = statemastar.sno WHERE (branchdata.sno = @BranchID)");
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
                string fromstateid = "";
                string Branchcode = "";
                string gststatecode = "";
                string companycode = "";
                string gstno = "";
                if (dtstatename.Rows.Count > 0)
                {
                    Branchcode = dtstatename.Rows[0]["Branchcode"].ToString();
                    statename = dtstatename.Rows[0]["statename"].ToString();
                    statecode = dtstatename.Rows[0]["statecode"].ToString();
                    gststatecode = dtstatename.Rows[0]["gststatecode"].ToString();
                    companycode = dtstatename.Rows[0]["companycode"].ToString();
                    gstno = dtstatename.Rows[0]["gstno"].ToString();
                }


                dtalldispatch = new DataTable();
                dtalldispatch.Columns.Add("sno");
                dtalldispatch.Columns.Add("ProductName");
                dtalldispatch.Columns.Add("TotalQty");

                produtstbl = new DataTable();
                produtstbl.Columns.Add("Categoryname");
                produtstbl.Columns.Add("sno");
                produtstbl.Columns.Add("SubCatName");
                produtstbl.Columns.Add("ProductName");
                produtstbl.Columns.Add("unitprice");
                produtstbl.Columns.Add("SubCat_sno").DataType = typeof(int);
                produtstbl.Columns.Add("Rank").DataType = typeof(int);

                foreach (DataRow dr in produtstbl1.Rows)
                {
                    DataRow newRow = produtstbl.NewRow();
                    newRow["Categoryname"] = dr["Categoryname"].ToString();
                    newRow["sno"] = dr["Sno"].ToString();
                    newRow["ProductName"] = dr["ProductName"].ToString();
                    newRow["unitprice"] = dr["unitprice"].ToString();
                    newRow["SubCat_sno"] = dr["SubCat_sno"].ToString();
                    if (dr["Rank"].ToString() == "")
                    {
                    }
                    else
                    {
                        newRow["Rank"] = dr["Rank"].ToString();
                        produtstbl.Rows.Add(newRow);
                    }
                }

                DataView dv = produtstbl.DefaultView;
                //dv.Sort = "Rank ASC";
                DataTable sortedProductDT = dv.ToTable();
                foreach (DataRow dr in sortedProductDT.Rows)
                {
                    DataRow newRow = dtalldispatch.NewRow();
                    newRow["sno"] = dr["Sno"].ToString();
                    newRow["ProductName"] = dr["ProductName"].ToString();
                    newRow["TotalQty"] = "0";
                    dtalldispatch.Rows.Add(newRow);
                }
                dtalldelivery = new DataTable();
                dtalldelivery.Columns.Add("sno");
                dtalldelivery.Columns.Add("ProductName");
                dtalldelivery.Columns.Add("deliverQty");
                dtalldelivery.Columns.Add("freeQty");
                dtalldelivery.Columns.Add("leakQty");
                dtalldelivery.Columns.Add("shortQty");
                dtalldelivery.Columns.Add("returnQty");
                foreach (DataRow dr in sortedProductDT.Rows)
                {
                    DataRow newRow = dtalldelivery.NewRow();
                    newRow["sno"] = dr["Sno"].ToString();
                    newRow["ProductName"] = dr["ProductName"].ToString();
                    newRow["deliverQty"] = "0";
                    newRow["freeQty"] = "0";
                    newRow["leakQty"] = "0";
                    newRow["shortQty"] = "0";
                    newRow["returnQty"] = "0";
                    dtalldelivery.Rows.Add(newRow);
                }
                //cmd = new MySqlCommand("SELECT ROUND(Sum(indents_subtable.DeliveryQty),2) as DeliveryQty, indents_subtable.UnitCost, branchdata.BranchName,productsdata.sno as prodsno, productsdata.ProductName FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN indents ON branchdata.sno = indents.Branch_id INNER JOIN  indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (branchroutes.BranchID = @BranchID) AND (branchdata.CollectionType = 'DUE') AND (indents.I_date BETWEEN @starttime AND @endtime) Group by indents.Branch_id,indents_subtable.Product_sno");
                cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty),2) AS DeliveryQty, indents_subtable.UnitCost, branchdata.BranchName, branchdata.sno,productsdata.SubCat_sno, productsdata.sno AS prodsno, productsdata.ProductName FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (modifiedroutes.BranchID = @BranchID) AND (branchdata.CollectionType = 'DUE') AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (modifiedroutes.BranchID = @BranchID) AND (branchdata.CollectionType = 'DUE')  AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY prodsno, branchdata.sno ORDER BY branchdata.sno, prodsno");
                cmd.Parameters.AddWithValue("@BranchID", SalesOfficeID);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtDue = vdm.SelectQuery(cmd).Tables[0];
                Report = new DataTable();
                if (dtble1.Rows.Count > 0)
                {

                    if (sortedProductDT.Rows.Count > 0)
                    {
                        DataView view = new DataView(dtble);
                        Report = new DataTable();
                        Report.Columns.Add("SNo");
                        Report.Columns.Add("InvoiceNo");
                        Report.Columns.Add("AgentName");
                        Report.Columns.Add("StateName");
                        Report.Columns.Add("GstNo");
                        foreach (DataRow dr in sortedProductDT.Rows)
                        {
                            Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                        }
                        Report.Columns.Add("Total Qty").DataType = typeof(Double);
                        Report.Columns.Add("Total Amount").DataType = typeof(Double);
                        //Report.Columns.Add("Dues Amount").DataType = typeof(Double);
                        //Report.Columns.Add("Cash Amount").DataType = typeof(Double);
                        Report.Columns.Add("Receipt No").DataType = typeof(Double);
                        DataTable distincttable = view.ToTable(true, "BranchName", "sno");

                        dttempproducts = new DataTable();
                        dttempproducts.Columns.Add("ProductName");
                        dttempproducts.Columns.Add("SubCatSno").DataType = typeof(int); ;
                        int i = 1;
                        double grnd_tot_qty = 0;
                        double grnd_tot_cashamount = 0;
                        double grnd_tot_dueamount = 0;
                        double grnd_tot_amount = 0;
                        foreach (DataRow dragesalevalue in dtble1.Rows)
                        {
                            if (Convert.ToDouble(dragesalevalue["salevalue"].ToString()) >= 50000)
                            {
                                foreach (DataRow branch in distincttable.Select("sno='" + dragesalevalue["sno"].ToString() + "'"))
                                {
                                    DataRow newrow = Report.NewRow();
                                    newrow["SNo"] = i;
                                    string bn = branch["BranchName"].ToString();
                                    if (bn == "Gunnala Praveen -Garepalli Distb")
                                    {

                                    }
                                    newrow["AgentName"] = branch["BranchName"].ToString();
                                    newrow["StateName"] = statename;
                                    newrow["GstNo"] = gstno;
                                    
                                    double total = 0;
                                    double Amount = 0;




                                    foreach (DataRow dr in dtble.Select("sno='" + dragesalevalue["sno"].ToString() + "'"))
                                    {
                                        string categoryid = "";
                                        foreach (DataRow drcate in dtcategory.Select("subcatsno='" + dr["SubCat_sno"].ToString() + "'"))
                                        {
                                            categoryid = drcate["catsno"].ToString();
                                        }
                                        string[] catarr = { "2","7","11","12","14","15","16","26","34","35","39","40","47","48" };
                                        if (catarr.Contains(categoryid))
                                        {
                                            string brnachname = dr["BranchName"].ToString();
                                            if (branch["BranchName"].ToString() == dr["BranchName"].ToString())
                                            {
                                                double qtyvalue = 0;
                                                double delqty = 0;
                                                string pname = dr["ProductName"].ToString();
                                                double.TryParse(dr["DeliveryQty"].ToString(), out delqty);
                                                if (delqty == 0.0)
                                                {

                                                }
                                                else
                                                {
                                                    newrow[dr["ProductName"].ToString()] = Math.Round(delqty, 2);
                                                    DataRow tempnewrow = dttempproducts.NewRow();
                                                    tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                                    tempnewrow["SubCatSno"] = dr["SubCat_sno"].ToString();
                                                    dttempproducts.Rows.Add(tempnewrow);
                                                }

                                                double.TryParse(dr["DeliveryQty"].ToString(), out qtyvalue);
                                                double UnitCost = 0;
                                                double.TryParse(dr["UnitCost"].ToString(), out UnitCost);
                                                Amount += qtyvalue * UnitCost;
                                                total += qtyvalue;
                                            }
                                        }
                                    }
                                    cmd = new MySqlCommand("SELECT invoiceno, agentid, invoicedate FROM e_invoice WHERE (invoicedate BETWEEN @d1 AND @d2) and agentid=@brncid");
                                    cmd.Parameters.AddWithValue("@brncid", branch["sno"].ToString());
                                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                                    DataTable dtagentdcno = vdm.SelectQuery(cmd).Tables[0];
                                    string dcno = "";
                                    long DcNo = 0;
                                    if (dtagentdcno.Rows.Count > 0)
                                    {
                                        dcno = dtagentdcno.Rows[0]["invoiceno"].ToString();
                                        newrow["InvoiceNo"] = dcno;
                                    }
                                    else
                                    {
                                        cmd = new MySqlCommand("SELECT IFNULL(MAX(invoiceno), 0) + 1 AS Sno FROM e_invoice WHERE (soid = @soid) AND (invoicedate BETWEEN @d1 AND @d2)");
                                        cmd.Parameters.AddWithValue("@soid", ddlSalesOffice.SelectedValue);
                                        cmd.Parameters.AddWithValue("@d1", GetLowDate(dtapril).AddDays(-1));
                                        cmd.Parameters.AddWithValue("@d2", GetHighDate(dtmarch).AddDays(-1));
                                        DataTable dtadcno = vdm.SelectQuery(cmd).Tables[0];
                                        string agentdcNo = dtadcno.Rows[0]["Sno"].ToString();
                                        cmd = new MySqlCommand("Insert Into e_invoice (agentid,invoicedate,soid,invoiceno,stateid,doe) Values(@agentid,@invoicedate,@soid,@einvoiceno,@stateid,@doe)");
                                        cmd.Parameters.AddWithValue("@agentid", branch["sno"].ToString());
                                        cmd.Parameters.AddWithValue("@invoicedate", GetLowDate(fromdate.AddDays(-1)));
                                        cmd.Parameters.AddWithValue("@soid", ddlSalesOffice.SelectedValue);
                                        cmd.Parameters.AddWithValue("@einvoiceno", agentdcNo);
                                        cmd.Parameters.AddWithValue("@stateid", gststatecode);
                                        cmd.Parameters.AddWithValue("@doe", ReportDate);
                                        if (Amount >= 50000)
                                        {
                                            //DcNo = vdm.insertScalar(cmd);
                                        }
                                        cmd = new MySqlCommand("SELECT invoiceno FROM  e_invoice WHERE (agentid = @agentid) AND (invoicedate BETWEEN @d1 AND @d2)");
                                        cmd.Parameters.AddWithValue("@agentid", branch["sno"].ToString());
                                        cmd.Parameters.AddWithValue("@soid", ddlSalesOffice.SelectedValue);
                                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                                        DataTable dtsubDc = vdm.SelectQuery(cmd).Tables[0];
                                        if (dtsubDc.Rows.Count > 0)
                                        {
                                            dcno = dtsubDc.Rows[0]["invoiceno"].ToString();
                                        }
                                        dcno = dcno.ToString();
                                    }
                                    newrow["Total Qty"] = Math.Round(total, 2);
                                    grnd_tot_qty += total;
                                    newrow["Total Amount"] = Math.Round(Amount, 2);
                                    grnd_tot_amount += Amount;
                                    if(Amount >= 50000)
                                    {
                                        Report.Rows.Add(newrow);
                                    }
                                    i++;
                                }
                            }
                        }
                    }

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
                    for (int ii = 0; ii < grdReports.Rows.Count; ii++)
                    {
                        GridViewRow dgvr = grdReports.Rows[ii];
                        if (dgvr.Cells[2].Text.Contains("Leakages") && !dgvr.Cells[2].Text.Contains("VLeakages"))
                        {
                            GridViewRow compare = grdReports.Rows[ii + 1];

                            for (int rowcnt = 3; rowcnt < dgvr.Cells.Count; rowcnt++)
                            {
                                if (dgvr.Cells[rowcnt].Text != compare.Cells[rowcnt].Text)
                                {
                                    compare.Cells[rowcnt].BackColor = Color.SandyBrown;
                                }
                            }
                        }
                    }
                    for (int jj = 0; jj < grdReports.Rows.Count; jj++)
                    {
                        GridViewRow dgvr = grdReports.Rows[jj];
                        if (dgvr.Cells[2].Text.Contains("DIFFERENCE"))
                        {

                            for (int rowcnt = 3; rowcnt < dgvr.Cells.Count; rowcnt++)
                            {
                                //int diff = 0;
                                //int.TryParse(dgvr.Cells[rowcnt].Text, out diff);
                                double diff1 = 0;
                                double.TryParse(dgvr.Cells[rowcnt].Text, out diff1);

                                if (diff1 > 0 || diff1 < 0)
                                {
                                    dgvr.Cells[rowcnt].BackColor = Color.SandyBrown;
                                }

                            }


                        }
                    }
                    Session["xportdata"] = Report;
                }
            }

            else
            {
                lblmsg.Text = "No data found";
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