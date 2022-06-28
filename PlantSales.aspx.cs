using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using ClosedXML.Excel;
using System.IO;

public partial class PlantSales : System.Web.UI.Page
{
    MySqlCommand cmd;
    string BranchID = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["branch"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            BranchID = Session["branch"].ToString();
        }
        //vdm = new VehicleDBMgr();
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                lblTitle.Text = Session["TitleName"].ToString();
                //lblDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                FillAgentName();

            }
        }
    }
    void FillAgentName()
    {

        try
        {
            vdm = new VehicleDBMgr();
            PBranch.Visible = true;
            DataTable dtBranch = new DataTable();
            dtBranch.Columns.Add("BranchName");
            dtBranch.Columns.Add("sno");
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
            ddlSalesOffice.DataSource = dtBranch;
            ddlSalesOffice.DataTextField = "BranchName";
            ddlSalesOffice.DataValueField = "sno";
            ddlSalesOffice.DataBind();
        }
        catch
        {
        }
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
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
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            vdm = new VehicleDBMgr();
            DataTable Report = new DataTable();
            DateTime fromdate = DateTime.Now;
            string[] fromdatestrig = txtFromdate.Text.Split(' ');
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
            string[] todatestrig = txtTodate.Text.Split(' ');
            if (todatestrig.Length > 1)
            {
                if (todatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = todatestrig[0].Split('-');
                    string[] times = todatestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            Session["filename"] = "SalesOffice wise Activity Report ->" + ddlSalesOffice.SelectedItem.Text;
            lblAgent.Text = ddlSalesOffice.SelectedItem.Text;
            lbl_fromDate.Text = txtFromdate.Text;
            lbl_selttodate.Text = txtTodate.Text;
            TimeSpan dateSpan = todate.Subtract(fromdate);
            int NoOfdays = dateSpan.Days;
            NoOfdays = NoOfdays + 1;
            string BranchID = ddlSalesOffice.SelectedValue;
            if (BranchID == "572")
            {
                BranchID = "7";
            }
            if (ddlType.SelectedValue == "All")
            {
                if (BranchID == "172")
                {
                    cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.UnitCost, products_category.Categoryname, productsdata.ProductName, DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchmappingtable_1.SuperBranch FROM indents INNER JOIN  indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchmappingtable ON indents.Branch_id = branchmappingtable.SubBranch INNER JOIN branchmappingtable branchmappingtable_1 ON branchmappingtable.SuperBranch = branchmappingtable_1.SubBranch WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable_1.SuperBranch = @brnchid) AND (branchmappingtable_1.SubBranch NOT IN (538, 1801, 3625)) GROUP BY productsdata.ProductName, products_category.Categoryname, IndentDate ORDER BY indents.I_date");
                }
                else
                {
                    cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.UnitCost, products_category.Categoryname, productsdata.ProductName, DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchmappingtable_1.SuperBranch FROM indents INNER JOIN  indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchmappingtable ON indents.Branch_id = branchmappingtable.SubBranch INNER JOIN branchmappingtable branchmappingtable_1 ON branchmappingtable.SuperBranch = branchmappingtable_1.SubBranch WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable_1.SuperBranch = @brnchid)  GROUP BY productsdata.ProductName, products_category.Categoryname, IndentDate ORDER BY indents.I_date");
                }
            }
            else
            {
                if (ddlType.SelectedItem.Text == "Curd")
                {
                    if (BranchID == "172")
                    {
                        cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.UnitCost, products_category.Categoryname, productsdata.ProductName, DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchmappingtable_1.SuperBranch FROM indents INNER JOIN  indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchmappingtable ON indents.Branch_id = branchmappingtable.SubBranch INNER JOIN branchmappingtable branchmappingtable_1 ON branchmappingtable.SuperBranch = branchmappingtable_1.SubBranch WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable_1.SuperBranch = @brnchid) AND (branchmappingtable_1.SubBranch NOT IN (538, 1801, 3625)) AND (products_category.sno IN (10, 37, 38, 39)) GROUP BY productsdata.ProductName, products_category.Categoryname, IndentDate ORDER BY indents.I_date");
                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.UnitCost, products_category.Categoryname, productsdata.ProductName, DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchmappingtable_1.SuperBranch FROM indents INNER JOIN  indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchmappingtable ON indents.Branch_id = branchmappingtable.SubBranch INNER JOIN branchmappingtable branchmappingtable_1 ON branchmappingtable.SuperBranch = branchmappingtable_1.SubBranch WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable_1.SuperBranch = @brnchid)  AND (products_category.sno IN (10, 37, 38, 39)) GROUP BY productsdata.ProductName, products_category.Categoryname, IndentDate ORDER BY indents.I_date");
                    }
                    
                }
                else
                {
                    if (BranchID == "172")
                    {
                        cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.UnitCost, products_category.Categoryname, productsdata.ProductName, DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchmappingtable_1.SuperBranch FROM indents INNER JOIN  indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchmappingtable ON indents.Branch_id = branchmappingtable.SubBranch INNER JOIN branchmappingtable branchmappingtable_1 ON branchmappingtable.SuperBranch = branchmappingtable_1.SubBranch WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable_1.SuperBranch = @brnchid) AND (branchmappingtable_1.SubBranch NOT IN (538, 1801, 3625)) and (products_category.Categoryname=@Cat) GROUP BY productsdata.ProductName, products_category.Categoryname, IndentDate ORDER BY indents.I_date");
                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.UnitCost, products_category.Categoryname, productsdata.ProductName, DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchmappingtable_1.SuperBranch FROM indents INNER JOIN  indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchmappingtable ON indents.Branch_id = branchmappingtable.SubBranch INNER JOIN branchmappingtable branchmappingtable_1 ON branchmappingtable.SuperBranch = branchmappingtable_1.SubBranch WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable_1.SuperBranch = @brnchid) and (products_category.Categoryname=@Cat) GROUP BY productsdata.ProductName, products_category.Categoryname, IndentDate ORDER BY indents.I_date");
                    }
                    
                    cmd.Parameters.AddWithValue("@Cat", ddlType.SelectedItem.Text);
                }
            }
            cmd.Parameters.AddWithValue("@brnchid", BranchID);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtAgent = vdm.SelectQuery(cmd).Tables[0];
            if (ddlType.SelectedValue == "All")
            {
                if (BranchID == "172")
                {
                    cmd = new MySqlCommand("SELECT productsdata.ProductName, branchproducts.Rank, productsdata.sno, products_category.Categoryname FROM branchmappingtable INNER JOIN indents ON branchmappingtable.SubBranch = indents.Branch_id INNER JOIN branchmappingtable branchmappingtable_1 ON branchmappingtable.SuperBranch = branchmappingtable_1.SubBranch INNER JOIN branchproducts ON branchmappingtable_1.SuperBranch = branchproducts.branch_sno INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo AND branchproducts.product_sno = indents_subtable.Product_sno INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable_1.SuperBranch = @brnchid) AND (branchmappingtable_1.SubBranch NOT IN (538, 1801, 3625)) GROUP BY productsdata.ProductName, productsdata.sno, products_category.Categoryname ORDER BY branchproducts.Rank");
                }
                else
                {
                    cmd = new MySqlCommand("SELECT productsdata.ProductName, branchproducts.Rank, productsdata.sno, products_category.Categoryname FROM branchmappingtable INNER JOIN indents ON branchmappingtable.SubBranch = indents.Branch_id INNER JOIN branchmappingtable branchmappingtable_1 ON branchmappingtable.SuperBranch = branchmappingtable_1.SubBranch INNER JOIN branchproducts ON branchmappingtable_1.SuperBranch = branchproducts.branch_sno INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo AND branchproducts.product_sno = indents_subtable.Product_sno INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable_1.SuperBranch = @brnchid) GROUP BY productsdata.ProductName, productsdata.sno, products_category.Categoryname ORDER BY branchproducts.Rank");
                }
                
            }
            else
            {
                if (ddlType.SelectedItem.Text == "Curd")
                {
                    if (BranchID == "172")
                    {
                        cmd = new MySqlCommand("SELECT productsdata.ProductName, branchproducts.Rank, productsdata.sno, products_category.Categoryname FROM branchmappingtable INNER JOIN indents ON branchmappingtable.SubBranch = indents.Branch_id INNER JOIN branchmappingtable branchmappingtable_1 ON branchmappingtable.SuperBranch = branchmappingtable_1.SubBranch INNER JOIN branchproducts ON branchmappingtable_1.SuperBranch = branchproducts.branch_sno INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo AND branchproducts.product_sno = indents_subtable.Product_sno INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable_1.SuperBranch = @brnchid) AND (branchmappingtable_1.SubBranch NOT IN (538, 1801, 3625)) AND (products_category.sno IN (10, 37, 38, 39)) GROUP BY productsdata.ProductName, productsdata.sno, products_category.Categoryname ORDER BY branchproducts.Rank");
                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT productsdata.ProductName, branchproducts.Rank, productsdata.sno, products_category.Categoryname FROM branchmappingtable INNER JOIN indents ON branchmappingtable.SubBranch = indents.Branch_id INNER JOIN branchmappingtable branchmappingtable_1 ON branchmappingtable.SuperBranch = branchmappingtable_1.SubBranch INNER JOIN branchproducts ON branchmappingtable_1.SuperBranch = branchproducts.branch_sno INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo AND branchproducts.product_sno = indents_subtable.Product_sno INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable_1.SuperBranch = @brnchid)  AND (products_category.sno IN (10, 37, 38, 39)) GROUP BY productsdata.ProductName, productsdata.sno, products_category.Categoryname ORDER BY branchproducts.Rank");
                    }
                }
                else
                {
                    if (BranchID == "172")
                    {
                        cmd = new MySqlCommand("SELECT productsdata.ProductName, branchproducts.Rank, productsdata.sno, products_category.Categoryname FROM branchmappingtable INNER JOIN indents ON branchmappingtable.SubBranch = indents.Branch_id INNER JOIN branchmappingtable branchmappingtable_1 ON branchmappingtable.SuperBranch = branchmappingtable_1.SubBranch INNER JOIN branchproducts ON branchmappingtable_1.SuperBranch = branchproducts.branch_sno INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo AND branchproducts.product_sno = indents_subtable.Product_sno INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable_1.SuperBranch = @brnchid) AND (branchmappingtable_1.SubBranch NOT IN (538,  1801, 3625)) and (products_category.Categoryname=@Cat) GROUP BY productsdata.ProductName, productsdata.sno, products_category.Categoryname ORDER BY branchproducts.Rank");
                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT productsdata.ProductName, branchproducts.Rank, productsdata.sno, products_category.Categoryname FROM branchmappingtable INNER JOIN indents ON branchmappingtable.SubBranch = indents.Branch_id INNER JOIN branchmappingtable branchmappingtable_1 ON branchmappingtable.SuperBranch = branchmappingtable_1.SubBranch INNER JOIN branchproducts ON branchmappingtable_1.SuperBranch = branchproducts.branch_sno INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo AND branchproducts.product_sno = indents_subtable.Product_sno INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable_1.SuperBranch = @brnchid) and (products_category.Categoryname=@Cat) GROUP BY productsdata.ProductName, productsdata.sno, products_category.Categoryname ORDER BY branchproducts.Rank");
                    }
                    cmd.Parameters.AddWithValue("@Cat", ddlType.SelectedItem.Text);
                }
            }
            cmd.Parameters.AddWithValue("@brnchid", BranchID);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
            if (produtstbl.Rows.Count > 0)
            {
                DataView view = new DataView(dtAgent);
                DataTable distinctproducts = view.ToTable(true, "ProductName", "Categoryname");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("IndentDate");
                foreach (DataRow dr in produtstbl.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                }
                Report.Columns.Add("Total").DataType = typeof(Double);
                Report.Columns.Add("Sale Value").DataType = typeof(Double);
                DataTable distincttable = view.ToTable(true, "IndentDate");
                int i = 1;
                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    string dtdate1 = branch["IndentDate"].ToString();
                    DateTime dtDOE1 = Convert.ToDateTime(dtdate1).AddDays(1);
                    string ChangedTime1 = dtDOE1.ToString("dd/MMM/yyyy");
                    newrow["IndentDate"] = ChangedTime1;

                    double total = 0;
                    double Amount = 0;
                    foreach (DataRow dr in dtAgent.Rows)
                    {
                        if (branch["IndentDate"].ToString() == dr["IndentDate"].ToString())
                        {
                            double qtyvalue = 0;
                            double UnitCost = 0;
                            double DQty = 0;
                            double.TryParse(dr["DeliveryQty"].ToString(), out DQty);
                            newrow[dr["ProductName"].ToString()] = DQty;
                            //if (dr["Categoryname"].ToString() == "MILK")
                            //{
                            double.TryParse(dr["DeliveryQty"].ToString(), out qtyvalue);
                            double.TryParse(dr["UnitCost"].ToString(), out UnitCost);
                            Amount += UnitCost * qtyvalue;
                            total += qtyvalue;
                            //}
                        }
                    }
                    newrow["Total"] = total;
                    newrow["Sale Value"] = Amount;
                    Report.Rows.Add(newrow);
                    i++;
                }
                DataRow newvartical = Report.NewRow();
                newvartical["IndentDate"] = "Total";
                DataRow newAvg = Report.NewRow();
                newAvg["IndentDate"] = "Avg Per Day";
                double Avgval = 0.0;
                double val = 0.0;
                foreach (DataColumn dc in Report.Columns)
                {
                    if (dc.DataType == typeof(Double))
                    {
                        Avgval = 0.0;
                        val = 0.0;
                        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                        newvartical[dc.ToString()] = val;
                        Avgval = val / NoOfdays;
                        newAvg[dc.ToString()] = Math.Round(Avgval, 2);
                    }
                }
                Report.Rows.Add(newvartical);
                Report.Rows.Add(newAvg);

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