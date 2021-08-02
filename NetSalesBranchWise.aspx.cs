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

public partial class NetSalesBranchWise : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["branch"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                lblTitle.Text = Session["TitleName"].ToString();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            }
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
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            pnlHide.Visible = true;
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
            DataTable dtdue = new DataTable();
            if (Session["branch"].ToString() == "172")
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty) ) AS SaleValue, products_category.Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN  branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (branchmappingtable.SubBranch NOT IN (1801)) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY branchdata.BranchName, productsdata.ProductName ORDER BY branchmappingtable.SuperBranch");
                //cmd = new MySqlCommand("SELECT branchdata.BranchName, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty) ) AS SaleValue, products_category.Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN  branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (branchmappingtable.SubBranch NOT IN (538,2749, 3928, 1801, 3625)) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY branchdata.BranchName, productsdata.ProductName");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                 dtdue = vdm.SelectQuery(cmd).Tables[0];
            }
            else if (Session["branch"].ToString() == "1801")
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty) ) AS SaleValue, products_category.Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN  branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY branchdata.BranchName, productsdata.ProductName");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                dtdue = vdm.SelectQuery(cmd).Tables[0];

            }
            else if (Session["branch"].ToString() == "3625")
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty) ) AS SaleValue, products_category.Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN  branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY branchdata.BranchName, productsdata.ProductName");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                dtdue = vdm.SelectQuery(cmd).Tables[0];

            }
            else
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty) ) AS SaleValue, products_category.Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN  branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY branchdata.BranchName, productsdata.ProductName");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                dtdue = vdm.SelectQuery(cmd).Tables[0];
            }



            cmd = new MySqlCommand("SELECT   products_subcategory.rank, branchdata.BranchName, products_subcategory.tempcatsno, productsdata.tempsubcatsno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS salevalue, products_category.description AS Categoryname FROM  (SELECT IndentNo, Branch_id, I_date, Status, IndentType FROM  indents WHERE (I_date BETWEEN @d1 AND @d2) AND (Status <> 'D')) indent INNER JOIN branchdata ON indent.Branch_id = branchdata.sno INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.tempsubcatsno = products_subcategory.tempsub_catsno INNER JOIN products_category ON products_subcategory.tempcatsno = products_category.tempcatsno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents_subtable.DeliveryQty <> 0) GROUP BY branchdata.sno, branchdata.BranchName, productsdata.ProductName ORDER BY branchdata.BranchName");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
            DataTable DtLocalTemp = vdm.SelectQuery(cmd).Tables[0];


            cmd = new MySqlCommand("SELECT ff.Sno, Leaks.tempcatsno,Leaks.rank,Leaks.ProductName,Leaks.tempsubcatsno, Leaks.Categoryname, Leaks.ProductId, ROUND(SUM(Leaks.DeliveryQty),2) AS DeliveryQty FROM (SELECT tripdata.Sno, productsdata.ProductName,productsdata.tempsubcatsno, products_category.Categoryname,products_subcategory.rank, products_category.tempcatsno,tripsubdata.ProductId, tripsubdata.Qty AS DeliveryQty FROM tripsubdata INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN tripdata ON tripsubdata.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (tripsubdata.Qty>0)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno, I_Date FROM (SELECT dispatch.DispName, tripdata_1.Sno, dispatch.sno AS DespSno, tripdata_1.I_Date FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata tripdata_1 ON triproutes.Tripdata_sno = tripdata_1.Sno WHERE (dispatch.Branch_Id = @BranchID) AND (dispatch.Route_id IS NULL) AND (dispatch.DispType <> 'spl') AND (dispatch.DispType <> 'agent') AND (dispatch.DispType <> 'so')) TripInfo) ff ON ff.Sno = Leaks.Sno GROUP BY Leaks.ProductName");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable dtlocal = vdm.SelectQuery(cmd).Tables[0];
            dtlocal.Merge(DtLocalTemp);

            //cmd = new MySqlCommand("SELECT ff.Sno, Leaks.ProductName, Leaks.Categoryname, Leaks.ProductId, SUM(Leaks.DeliveryQty) AS DeliveryQty FROM (SELECT        tripdata.Sno, productsdata.ProductName, products_category.Categoryname, tripsubdata.ProductId, tripsubdata.Qty AS DeliveryQty FROM tripsubdata INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN tripdata ON tripsubdata.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno, I_Date FROM (SELECT dispatch.DispName, tripdata_1.Sno, dispatch.sno AS DespSno, tripdata_1.I_Date FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata tripdata_1 ON triproutes.Tripdata_sno = tripdata_1.Sno WHERE (dispatch.Branch_Id = @BranchID) AND (dispatch.Route_id IS NULL) AND (dispatch.DispMode <> 'SPL')) TripInfo) ff ON ff.Sno = Leaks.Sno GROUP BY Leaks.ProductName");
            //cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            //cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            //DataTable dtlocal = vdm.SelectQuery(cmd).Tables[0];


            cmd = new MySqlCommand("SELECT branchproducts.branch_sno, branchproducts.product_sno, branchproducts.Rank, productsdata.ProductName, products_category.Categoryname FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) ORDER BY branchproducts.Rank");
            //cmd.Parameters.AddWithValue("@Flag", "1");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@Branch", Session["branch"].ToString());
            DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
            if (produtstbl.Rows.Count > 0)
            {
                DataView view1 = new DataView(produtstbl);
                DataView view = new DataView(dtdue);
                DataTable distinctproducts = view1.ToTable(true, "ProductName", "Categoryname");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("Agent Name");
                int count = 0;
                foreach (DataRow dr in distinctproducts.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                    count++;
                }
                Report.Columns.Add("Total Milk", typeof(Double)).SetOrdinal(count + 2);
                Report.Columns.Add("Total Curd&BM", typeof(Double)).SetOrdinal(count + 3);
                Report.Columns.Add("Total Lts", typeof(Double)).SetOrdinal(count + 4);
                int i = 1;
                double totalsalevalue = 0;
                DataTable distincttable = view.ToTable(true, "BranchName");
                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    newrow["Agent Name"] = branch["BranchName"].ToString();
                    double total = 0;
                    double totalcurdandBM = 0;
                    foreach (DataRow dr in dtdue.Rows)
                    {
                        if (branch["BranchName"].ToString() == dr["BranchName"].ToString())
                        {
                            double qtyvalue = 0;
                            string DeliveryQty = dr["DeliveryQty"].ToString();
                            if (DeliveryQty == "")
                            {
                            }
                            else
                            {
                                double assqty = 0;
                                double curdBm = 0;
                                double Buttermilk = 0;
                                double salevalue = 0;
                                newrow[dr["ProductName"].ToString()] = DeliveryQty;
                                //if (dr["Categoryname"].ToString() == "MILK")
                                //{
                                //double.TryParse(dr["DeliveryQty"].ToString(), out qtyvalue);
                                //total += qtyvalue;
                                if (dr["Categoryname"].ToString() == "MILK")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out assqty);
                                    total += assqty;
                                }
                                if (dr["Categoryname"].ToString() == "CURD")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out curdBm);
                                    totalcurdandBM += curdBm;
                                    //double.TryParse(dr["SaleValue"].ToString(), out salevalue);
                                    //totalsalevalue += salevalue;
                                }
                                if (dr["Categoryname"].ToString() == "ButterMilk")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                    totalcurdandBM += Buttermilk;
                                    //double.TryParse(dr["SaleValue"].ToString(), out salevalue);
                                    //totalsalevalue += salevalue;
                                }
                            }
                            //}
                        }
                    }
                    newrow["Total Milk"] = total;
                    newrow["Total Curd&BM"] = totalcurdandBM;
                    newrow["Total Lts"] = total + totalcurdandBM;
                    Report.Rows.Add(newrow);
                    i++;
                }
                DataRow newrow1 = Report.NewRow();
                newrow1["SNo"] = i;
                newrow1["Agent Name"] = "LOCAL SALE";
                double total1 = 0;
                double totalcurdandBM1 = 0;
                foreach (DataRow dr in dtlocal.Rows)
                {

                    double qtyvalue = 0;
                    string DeliveryQty = dr["DeliveryQty"].ToString();
                    if (DeliveryQty == "")
                    {
                    }
                    else
                    {
                        double assqty = 0;
                        double curdBm = 0;
                        double Buttermilk = 0;
                        double dqty = 0;
                        double.TryParse(DeliveryQty, out dqty);
                        dqty = Math.Round(dqty, 2);
                        double salevalue = 0;
                        newrow1[dr["ProductName"].ToString()] = dqty;
                        //if (dr["Categoryname"].ToString() == "MILK")
                        //{
                        //double.TryParse(dr["DeliveryQty"].ToString(), out qtyvalue);
                        //total += qtyvalue;
                        string catname = dr["Categoryname"].ToString();
                        if (dr["Categoryname"].ToString() == "MILK" || dr["Categoryname"].ToString() == "Milk")
                        {
                            double.TryParse(dr["DeliveryQty"].ToString(), out assqty);
                            assqty = Math.Round(assqty, 2);
                            total1 += assqty;
                        }
                        if (dr["Categoryname"].ToString() == "CURD" || dr["Categoryname"].ToString() == "Curd")
                        {
                            double.TryParse(dr["DeliveryQty"].ToString(), out curdBm);
                            curdBm = Math.Round(curdBm, 2);
                            totalcurdandBM1 += curdBm;
                            //double.TryParse(dr["SaleValue"].ToString(), out salevalue);
                            //totalsalevalue += salevalue;
                        }
                        if (dr["Categoryname"].ToString() == "ButterMilk")
                        {
                            double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                            Buttermilk = Math.Round(Buttermilk, 2);
                            totalcurdandBM1 += Buttermilk;
                            //double.TryParse(dr["SaleValue"].ToString(), out salevalue);
                            //totalsalevalue += salevalue;
                        }
                    }
                    //}
                }
                newrow1["Total Milk"] = total1;
                newrow1["Total Curd&BM"] = totalcurdandBM1;
                newrow1["Total Lts"] = total1 + totalcurdandBM1;
                Report.Rows.Add(newrow1);
                i++;

                foreach (var column in Report.Columns.Cast<DataColumn>().ToArray())
                {
                    if (Report.AsEnumerable().All(dr => dr.IsNull(column)))
                        Report.Columns.Remove(column);
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
                //DataRow newvartical1 = Report.NewRow();
                //newvartical1["Agent Name"] = "AvgRate";
                //double avg = 0.0;
                //foreach (DataColumn dc in Report.Columns)
                //{
                //    if (dc.DataType == typeof(Double))
                //    {
                //        val = 0.0;
                //        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                //        newvartical1[dc.ToString()] = val / totalsalevalue;
                //    }
                //}
                //Report.Rows.Add(newvartical1);
                foreach (DataColumn col in Report.Columns)
                {
                    string Pname = col.ToString();
                    string ProductName = col.ToString();
                    ProductName = GetSpace(ProductName);
                    Report.Columns[Pname].ColumnName = ProductName;
                }
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
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable("GridView_Data");
            int count = 0;
            foreach (TableCell cell in grdReports.HeaderRow.Cells)
            {
                if (count == 1)
                {
                    dt.Columns.Add(cell.Text);
                }
                else
                {
                    dt.Columns.Add(cell.Text).DataType = typeof(double);
                }
                count++;
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
                //string FileName = Session["filename"].ToString();
                string FileName = "Branch Wse Net Sale";
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