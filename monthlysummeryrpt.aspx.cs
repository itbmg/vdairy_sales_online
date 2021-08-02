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

public partial class monthlysummeryrpt : System.Web.UI.Page
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

    protected void btnsubGenerate_Click(object sender, EventArgs e)
    {
        categorywise();
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
    DataTable dtSortedSubCategory = new DataTable();
    DataTable dtSortedCategory = new DataTable();
    DataTable dttempproducts = new DataTable();
    DataTable produtstbl1 = new DataTable();
    DataTable dtSubCatgory = new DataTable();
    DataTable dtCatgory = new DataTable();
    DataTable dtSortedCategoryAndSubCat = new DataTable();

    DataTable dtCatgoryAndSub = new DataTable();

    void categorywise()
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

        int mnth;
        var diffMonths = (todate.Month + todate.Year * 12) - (fromdate.Month + fromdate.Year * 12);

        string value = ddlmonth.SelectedItem.Value;
        mnth = Convert.ToInt32(value);
        DateTime now = DateTime.Now;
        var startDate = new DateTime(now.Year, mnth, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        fromdate = Convert.ToDateTime(startDate);
        todate = Convert.ToDateTime(endDate);

        cmd = new MySqlCommand("SELECT   products_subcategory.rank,branchdata.BranchName,products_subcategory.tempcatsno, productsdata.tempsubcatsno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS SaleValue, products_category.description AS Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.tempsubcatsno = products_subcategory.tempsub_catsno INNER JOIN products_category ON products_subcategory.tempcatsno = products_category.tempcatsno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty>0) AND  (products_subcategory.tempcatsno NOT IN ('3,7'))  GROUP BY branchdata.BranchName,products_category.tempcatsno ORDER BY branchdata.sno");
        cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
        cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
        DataTable dtdue = vdm.SelectQuery(cmd).Tables[0];

        cmd = new MySqlCommand("SELECT   products_subcategory.rank,branchdata.BranchName,products_subcategory.tempcatsno,products_subcategory.description, productsdata.tempsubcatsno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS SaleValue, products_category.description AS Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.tempsubcatsno = products_subcategory.tempsub_catsno INNER JOIN products_category ON products_subcategory.tempcatsno = products_category.tempcatsno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty>0) AND (products_subcategory.tempcatsno IN ('3')) GROUP BY branchdata.BranchName, productsdata.tempsubcatsno");
        cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
        cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
        DataTable dtBiproduct = vdm.SelectQuery(cmd).Tables[0];


        cmd = new MySqlCommand("SELECT   products_subcategory.rank, branchdata.BranchName, products_subcategory.tempcatsno, productsdata.tempsubcatsno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS salevalue, products_category.description AS Categoryname,products_subcategory.description FROM  (SELECT IndentNo, Branch_id, I_date, Status, IndentType FROM  indents WHERE (I_date BETWEEN @d1 AND @d2) AND (Status <> 'D')) indent INNER JOIN branchdata ON indent.Branch_id = branchdata.sno INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.tempsubcatsno = products_subcategory.tempsub_catsno INNER JOIN products_category ON products_subcategory.tempcatsno = products_category.tempcatsno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents_subtable.DeliveryQty <> 0) GROUP BY branchdata.sno, branchdata.BranchName, products_category.tempcatsno ORDER BY branchdata.BranchName");
        cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
        cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
        cmd.Parameters.AddWithValue("@Catsno", "All");
        DataTable DtLocalTemp = vdm.SelectQuery(cmd).Tables[0];


        cmd = new MySqlCommand("SELECT ff.Sno, Leaks.tempcatsno,Leaks.rank,Leaks.ProductName,Leaks.tempsubcatsno, Leaks.Categoryname, Leaks.ProductId, ROUND(SUM(Leaks.DeliveryQty),2) AS DeliveryQty FROM (SELECT tripdata.Sno, productsdata.ProductName,productsdata.tempsubcatsno, products_category.Categoryname,products_subcategory.rank, products_category.tempcatsno,tripsubdata.ProductId, tripsubdata.Qty AS DeliveryQty FROM tripsubdata INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN tripdata ON tripsubdata.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (tripsubdata.Qty>0)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno, I_Date FROM (SELECT dispatch.DispName, tripdata_1.Sno, dispatch.sno AS DespSno, tripdata_1.I_Date FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata tripdata_1 ON triproutes.Tripdata_sno = tripdata_1.Sno WHERE (dispatch.Branch_Id = @BranchID) AND (dispatch.Route_id IS NULL) AND (dispatch.DispType <> 'spl') AND (dispatch.DispType <> 'agent') AND (dispatch.DispType <> 'so')) TripInfo) ff ON ff.Sno = Leaks.Sno GROUP BY Leaks.tempcatsno");
        cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
        cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
        DataTable dtlocal = vdm.SelectQuery(cmd).Tables[0];
        dtlocal.Merge(DtLocalTemp);


        //cmd = new MySqlCommand("SELECT ff.Sno, Leaks.tempcatsno,Leaks.rank,Leaks.tempsubcatsno, Leaks.Categoryname, ROUND(SUM(Leaks.DeliveryQty),2) AS DeliveryQty FROM (SELECT tripdata.Sno, productsdata.ProductName,productsdata.tempsubcatsno, products_category.Categoryname,products_subcategory.rank, products_category.tempcatsno,tripsubdata.ProductId, tripsubdata.Qty AS DeliveryQty FROM tripsubdata INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN tripdata ON tripsubdata.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (tripsubdata.Qty>0)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno, I_Date FROM (SELECT dispatch.DispName, tripdata_1.Sno, dispatch.sno AS DespSno, tripdata_1.I_Date FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata tripdata_1 ON triproutes.Tripdata_sno = tripdata_1.Sno WHERE (dispatch.Branch_Id = @BranchID) AND (dispatch.Route_id IS NULL) AND (dispatch.DispMode <> 'SPL')) TripInfo) ff ON ff.Sno = Leaks.Sno GROUP BY Leaks.tempcatsno");
        //cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
        //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
        //cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
        //cmd.Parameters.AddWithValue("@Catsno", ddlCategoryName.SelectedValue);
        //DataTable dtlocal = vdm.SelectQuery(cmd).Tables[0];

        cmd = new MySqlCommand("SELECT branchproducts.branch_sno, branchproducts.product_sno, branchproducts.Rank, productsdata.ProductName, products_category.Categoryname FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) ORDER BY branchproducts.Rank");
        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
        cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
        cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
        cmd.Parameters.AddWithValue("@Branch", Session["branch"].ToString());
        DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];

        cmd = new MySqlCommand("SELECT    products_category.tempcatsno AS CatSno,productsdata.tempsubcatsno,branchproducts.unitprice,branchproducts.branch_sno,products_subcategory.tempsub_catsno AS SubCatSno, products_category.description AS Categoryname, branchproducts.product_sno AS sno, productsdata.ProductName, branchproducts.Rank,products_subcategory.description AS SubCategoryName FROM  products_category INNER JOIN products_subcategory ON products_category.tempcatsno = products_subcategory.tempcatsno INNER JOIN productsdata ON products_subcategory.tempsub_catsno = productsdata.tempsubcatsno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno WHERE (branchproducts.branch_sno = @BranchId) ORDER BY products_category.tempcatsno");
        cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
        produtstbl1 = vdm.SelectQuery(cmd).Tables[0];

        if (produtstbl.Rows.Count > 0)
        {
            DataView view1 = new DataView(produtstbl1);
            DataView view = new DataView(dtdue);
            DataTable distinctproducts = view1.ToTable(true, "Categoryname");
            DataTable tempBiCategory = new DataTable();
            tempBiCategory.Columns.Add("description");
            foreach (DataRow drbip in dtBiproduct.Rows)
            {
                DataRow binewrow = tempBiCategory.NewRow();
                if (drbip["description"].ToString() == "BuffalowGhee" || drbip["description"].ToString() == "CowGhee")
                {
                    binewrow["description"] = "Ghee";
                }
                else
                {
                    binewrow["description"] = drbip["description"].ToString();

                }
                tempBiCategory.Rows.Add(binewrow);
            }
            DataView viewbiproduct = new DataView(tempBiCategory);
            DataTable distinctbiproduct = viewbiproduct.ToTable(true, "description");

            Report = new DataTable();
            Report.Columns.Add("SNo");
            Report.Columns.Add("AgentName");
            string tempcat = "";
            int count = 0;
            foreach (DataRow dr in distinctproducts.Rows)
            {
                if (dr["Categoryname"].ToString() == "BiProducts")
                {
                    foreach (DataRow drbip in distinctbiproduct.Rows)
                    {
                        Report.Columns.Add(drbip["description"].ToString()).DataType = typeof(Double);
                    }
                }
                else
                {
                    Report.Columns.Add(dr["Categoryname"].ToString()).DataType = typeof(Double);
                }
                count++;
            }
           // Report.Columns.Add("Total").DataType = typeof(Double); ;

            dttempproducts = new DataTable();
            dttempproducts.Columns.Add("ProductName");
            dttempproducts.Columns.Add("SubCatSno").DataType = typeof(int); ;
            dttempproducts.Columns.Add("CatSno").DataType = typeof(int); ;
            dttempproducts.Columns.Add("rank").DataType = typeof(int);

            int i = 1;
            double totalsalevalue = 0;
            DataTable distincttable = view.ToTable(true, "BranchName");
            foreach (DataRow branch in distincttable.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["SNo"] = i;
                newrow["AgentName"] = branch["BranchName"].ToString();
                double total = 0;
                double totalcurdandBM = 0;
                double totalbBiproducts = 0;
                double totalsweets = 0;
                double totalFoodProducts = 0;
                double totalBeverages = 0;
                double totalOthers = 0;
                double totalIceCream = 0;
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
                            double qty = 0;
                            double salevalue = 0;
                            newrow[dr["Categoryname"].ToString()] = dr["DeliveryQty"].ToString();
                            int rank = 0;
                            double.TryParse(dr["DeliveryQty"].ToString(), out qty);
                            total += qty;
                        }
                    }
                }

                double gheetotal = 0;
                foreach (DataRow dr in dtBiproduct.Rows)
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
                            double qty = 0;
                            double salevalue = 0;
                            if (dr["description"].ToString() == "BuffalowGhee" || dr["description"].ToString() == "CowGhee")
                            {

                                double.TryParse(dr["DeliveryQty"].ToString(), out qty);
                                gheetotal += qty;
                                newrow["Ghee"] = gheetotal.ToString();
                            }
                            else
                            {
                                newrow[dr["description"].ToString()] = dr["DeliveryQty"].ToString();
                            }
                            int rank = 0;
                            double.TryParse(dr["DeliveryQty"].ToString(), out qty);
                            total += qty;

                        }
                    }
                }

                //newrow["Total"] = total;
                Report.Rows.Add(newrow);
                i++;
            }
            DataRow newrow1 = Report.NewRow();
            newrow1["SNo"] = i;
            newrow1["AgentName"] = "LOCAL SALE";
            double total1 = 0;
            double totalcurdandBM1 = 0;

            DataTable temptable = new DataTable();
            temptable.Columns.Add("Sno");
            temptable.Columns.Add("tempcatsno");
            temptable.Columns.Add("ProductName");
            temptable.Columns.Add("tempsubcatsno");
            temptable.Columns.Add("Categoryname");
            temptable.Columns.Add("DeliveryQty");
            temptable.Columns.Add("rank");
            cmd = new MySqlCommand("SELECT  products_subcategory.sno, products_subcategory.category_sno, products_subcategory.SubCatName, products_subcategory.Flag, products_subcategory.userdata_sno, products_subcategory.fat,products_subcategory.rank, products_subcategory.tempcatsno, products_subcategory.tempsub_catsno, products_category.description,products_category.Categoryname FROM products_subcategory INNER JOIN products_category ON products_subcategory.tempcatsno = products_category.tempcatsno WHERE (products_subcategory.tempsub_catsno IS NOT NULL) AND (products_subcategory.tempsub_catsno <> '0')");
            DataTable dtcategory = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in dtlocal.Rows)
            {
                DataRow tempnewrow = temptable.NewRow();
                tempnewrow["Sno"] = dr["Sno"].ToString();
                if (dr["description"].ToString() != "")
                {
                    foreach (DataRow drr in dtcategory.Select("tempsub_catsno='" + dr["tempsubcatsno"].ToString() + "'"))
                    {
                        tempnewrow["tempcatsno"] = drr["tempcatsno"].ToString();
                        if (dr["description"].ToString() == "BuffalowGhee" || dr["description"].ToString() == "CowGhee")
                        {
                            tempnewrow["Categoryname"] = "Ghee";
                        }
                        else if (dr["description"].ToString() == "Paneer" || dr["description"].ToString() == "Butter" || dr["description"].ToString() == "Cream" || dr["description"].ToString() == "Lassi")
                        {
                            tempnewrow["Categoryname"] = dr["description"].ToString();
                        }
                        else
                        {
                            tempnewrow["Categoryname"] = dr["Categoryname"].ToString();
                        }
                        tempnewrow["rank"] = drr["rank"].ToString();
                    }
                    tempnewrow["tempsubcatsno"] = dr["tempsubcatsno"].ToString();
                    tempnewrow["DeliveryQty"] = dr["DeliveryQty"].ToString();
                    temptable.Rows.Add(tempnewrow);
                }
                else
                {
                    foreach (DataRow drr in dtcategory.Select("tempsub_catsno='" + dr["tempsubcatsno"].ToString() + "'"))
                    {
                        tempnewrow["tempcatsno"] = drr["tempcatsno"].ToString();
                        tempnewrow["Categoryname"] = drr["description"].ToString();
                        tempnewrow["rank"] = drr["rank"].ToString();
                    }
                    tempnewrow["tempsubcatsno"] = dr["tempsubcatsno"].ToString();
                    tempnewrow["DeliveryQty"] = dr["DeliveryQty"].ToString();
                    temptable.Rows.Add(tempnewrow);
                }
            }


            //foreach (DataRow dr in distinctproducts.Rows)
            //{
            //    if (dr["Categoryname"].ToString() == "BiProducts")
            //    {
            //        foreach (DataRow drbip in distinctbiproduct.Rows)
            //        {
            //            Report.Columns.Add(drbip["description"].ToString()).DataType = typeof(Double);
            //        }
            //    }
            //    else
            //    {
            //        Report.Columns.Add(dr["Categoryname"].ToString()).DataType = typeof(Double);
            //    }
            //    count++;
            //}

            //double totalcurdandBM1 = 0;
            double totalbBiproducts1 = 0;
            double totalsweets1 = 0;
            double totalFoodProducts1 = 0;
            double totalBeverages1 = 0;
            double totalOthers1 = 0;
            double totalIceCream1 = 0;
            double ltotal = 0;
            DataView Temp_View = new DataView(temptable);
            DataTable tempdata = Temp_View.ToTable(true, "Categoryname");

            foreach (DataRow dr in tempdata.Rows)
            {
                double temptot = 0;
                foreach (DataRow drr1 in temptable.Select("Categoryname='" + dr["Categoryname"].ToString() + "'"))
                {

                    double qtyvalue = 0;
                    string DeliveryQty = drr1["DeliveryQty"].ToString();
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
                        // newrow1[drr1["Categoryname"].ToString()] = drr1["DeliveryQty"].ToString();
                        double.TryParse(drr1["DeliveryQty"].ToString(), out dqty);
                        temptot += dqty;
                        ltotal += dqty;
                        int rank = 0;
                    }
                }
                newrow1[dr["Categoryname"].ToString()] = temptot;
            }
           // newrow1["Total"] = ltotal;
            Report.Rows.Add(newrow1);
            i++;
            DataRow newvartical = Report.NewRow();
            newvartical["AgentName"] = "Total";
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
            DataRow dtnewrow = Report.NewRow();
            newvartical["AgentName"] = "Total";

            DataTable dtsummary = new DataTable();
            dtsummary.Columns.Add("CategoryName");
            dtsummary.Columns.Add("Qty");
            string s = "Total";
            double summilk = 0, sumcurd = 0, sumpanner = 0, sumghee = 0, sumbutter = 0, sumlassi = 0, sumsweets = 0, sumfood = 0, sumbeverages = 0, sumothers = 0, sumicecream = 0, sumcream = 0; ;
            double gtotal = 0, gMilkcurdtotal = 0, gotherstotal = 0;
            foreach (DataRow drreport in Report.Select("AgentName='" + s + "'"))
            {
                DataRow summrow = dtsummary.NewRow();
                summrow["CategoryName"] = "Milk";
                summrow["Qty"] = drreport["Milk"].ToString();
                double dqty = 0;
                double.TryParse(drreport["Milk"].ToString(), out dqty);
                gtotal += dqty;
                gMilkcurdtotal += dqty;
                dtsummary.Rows.Add(summrow);

                DataRow summrow1 = dtsummary.NewRow();
                summrow1["CategoryName"] = "Curd";
                summrow1["Qty"] = drreport["Curd"].ToString();
                dqty = 0;
                double.TryParse(drreport["Curd"].ToString(), out dqty);
                gtotal += dqty;
                gMilkcurdtotal += dqty;
                dtsummary.Rows.Add(summrow1);

                DataRow summrow13 = dtsummary.NewRow();
                summrow13["CategoryName"] = "CurdMilkTotal";
                summrow13["Qty"] = gMilkcurdtotal;
                dtsummary.Rows.Add(summrow13);

                DataRow summrow2 = dtsummary.NewRow();
                summrow2["CategoryName"] = "Paneer";
                summrow2["Qty"] = drreport["Paneer"].ToString();
                dqty = 0;
                double.TryParse(drreport["Paneer"].ToString(), out dqty);
                gtotal += dqty;
                gotherstotal += dqty;
                dtsummary.Rows.Add(summrow2);


                DataRow summrow3 = dtsummary.NewRow();
                summrow3["CategoryName"] = "Ghee";
                summrow3["Qty"] = drreport["Ghee"].ToString();
                dqty = 0;
                double.TryParse(drreport["Ghee"].ToString(), out dqty);
                gotherstotal += dqty;
                gtotal += dqty;
                dtsummary.Rows.Add(summrow3);


                DataRow summrow4 = dtsummary.NewRow();
                summrow4["CategoryName"] = "Butter";
                summrow4["Qty"] = drreport["Butter"].ToString();
                dqty = 0;
                double.TryParse(drreport["Butter"].ToString(), out dqty);
                gotherstotal += dqty;
                gtotal += dqty;
                dtsummary.Rows.Add(summrow4);


                DataRow summrow5 = dtsummary.NewRow();
                summrow5["CategoryName"] = "Cream";
                summrow5["Qty"] = drreport["Cream"].ToString();
                dqty = 0;
                double.TryParse(drreport["Cream"].ToString(), out dqty);
                gotherstotal += dqty;
                gtotal += dqty;
                dtsummary.Rows.Add(summrow5);


                DataRow summrow6 = dtsummary.NewRow();
                summrow6["CategoryName"] = "Lassi";
                summrow6["Qty"] = drreport["Lassi"].ToString();
                dqty = 0;
                double.TryParse(drreport["Lassi"].ToString(), out dqty);
                gotherstotal += dqty;
                gtotal += dqty;
                dtsummary.Rows.Add(summrow6);


                DataRow summrow7 = dtsummary.NewRow();
                summrow7["CategoryName"] = "Sweets";
                summrow7["Qty"] = drreport["Sweets"].ToString();
                dqty = 0;
                double.TryParse(drreport["Sweets"].ToString(), out dqty);
                gotherstotal += dqty;
                gtotal += dqty;
                dtsummary.Rows.Add(summrow7);


                DataRow summrow8 = dtsummary.NewRow();
                summrow8["CategoryName"] = "FoodProduc";
                summrow8["Qty"] = drreport["FoodProduc"].ToString();
                dqty = 0;
                double.TryParse(drreport["FoodProduc"].ToString(), out dqty);
                gotherstotal += dqty;
                gtotal += dqty;
                dtsummary.Rows.Add(summrow8);


                DataRow summrow9 = dtsummary.NewRow();
                summrow9["CategoryName"] = "Beverages";
                summrow9["Qty"] = drreport["Beverages"].ToString();
                dqty = 0;
                double.TryParse(drreport["Beverages"].ToString(), out dqty);
                gotherstotal += dqty;
                gtotal += dqty;
                dtsummary.Rows.Add(summrow9);


                //DataRow summrow10 = dtsummary.NewRow();
                //summrow10["CategoryName"] = "Others";
                //summrow10["Qty"] = drreport["Others"].ToString();
                //dqty = 0;
                //double.TryParse(drreport["Others"].ToString(), out dqty);
                //gotherstotal += dqty;
                //gtotal += dqty;
                //dtsummary.Rows.Add(summrow10);

                DataRow summrow11 = dtsummary.NewRow();
                summrow11["CategoryName"] = "IceCream";
                summrow11["Qty"] = drreport["IceCream"].ToString();
                dqty = 0;
                double.TryParse(drreport["IceCream"].ToString(), out dqty);
                gotherstotal += dqty;
                gtotal += dqty;
                dtsummary.Rows.Add(summrow11);

                DataRow summrow12 = dtsummary.NewRow();
                summrow12["CategoryName"] = "BiProductsTotal";
                summrow12["Qty"] = gotherstotal;
                dtsummary.Rows.Add(summrow12);

                DataRow summrow14 = dtsummary.NewRow();
                summrow14["CategoryName"] = "GrandTotal";
                summrow14["Qty"] = gtotal;
                dtsummary.Rows.Add(summrow14);
            }

            foreach (var column in dtsummary.Columns.Cast<DataColumn>().ToArray())
            {
                if (dtsummary.AsEnumerable().All(dr => dr.IsNull(column)))
                    dtsummary.Columns.Remove(column);
            }
            foreach (DataColumn col in Report.Columns)
            {
                string Pname = col.ToString();
                string ProductName = col.ToString();
                ProductName = GetSpace(ProductName);
                Report.Columns[Pname].ColumnName = ProductName;
            }

            GridView1.DataSource = Report;
            GridView1.DataBind();
        }
    }

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
            cmd = new MySqlCommand("SELECT MONTHNAME(indents.I_date) AS MONTH, YEAR(indents.I_date) AS Expr1, branchdata.BranchName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,  ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS SaleValue FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo WHERE        (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty > 0) GROUP BY branchdata.BranchName, YEAR(indents.I_date), MONTH(indents.I_date) ORDER BY branchdata.sno, MONTH(indents.I_date)");
            //cmd = new MySqlCommand("SELECT branchdata.BranchName,productsdata.tempsubcatsno,productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty) ) AS SaleValue, products_category.Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN  branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (products_category.tempcatsno=@Catsno) GROUP BY branchdata.BranchName, productsdata.ProductName");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable temptable = vdm.SelectQuery(cmd).Tables[0];
            DataView Temp_View = new DataView(temptable);
            DataTable tempdata = Temp_View.ToTable(true, "MONTH");
            Report = new DataTable();
            Report.Columns.Add("SNo");
            Report.Columns.Add("AgentName");
            foreach (DataRow dr in tempdata.Rows)
            {
                Report.Columns.Add(dr["MONTH"].ToString()).DataType = typeof(Double);
            }
            int i = 1;
            double totalsalevalue = 0;
            DataTable distincttable = Temp_View.ToTable(true, "BranchName");
            foreach (DataRow branch in distincttable.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["SNo"] = i;
                newrow["AgentName"] = branch["BranchName"].ToString();
                double total = 0;
                double totalcurdandBM = 0;
                double totalbBiproducts = 0;
                double totalsweets = 0;
                double totalFoodProducts = 0;
                double totalBeverages = 0;
                double totalOthers = 0;
                double totalIceCream = 0;
                foreach (DataRow dr in temptable.Rows)
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
                            double qty = 0;
                            double salevalue = 0;
                            newrow[dr["MONTH"].ToString()] = dr["DeliveryQty"].ToString();
                            int rank = 0;
                            double.TryParse(dr["DeliveryQty"].ToString(), out qty);
                            total += qty;
                        }
                    }
                }
                //newrow["Total"] = total;
                Report.Rows.Add(newrow);
                i++;
            }
            grdReports.DataSource = Report;
            grdReports.DataBind();
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
                    dt.Columns.Add(cell.Text);
                }
                count++;
            }
            foreach (GridViewRow row in grdReports.Rows)
            {
                dt.Rows.Add();
                for (int i = 1; i < row.Cells.Count; i++)
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

    protected void grdReports_RowCreated(object sender, GridViewRowEventArgs e)
    {
        // Adding a column manually once the header created
        if (e.Row.RowType == DataControlRowType.Header) // If header created
        {
           
        }
        // }
        //}
    }
}