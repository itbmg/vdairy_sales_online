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


public partial class BranchCategoryWiseSale : System.Web.UI.Page
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
                FillAgentName();
                
            }
        }
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string type = ddlType.SelectedValue;
        if (ddlType.SelectedValue == "CategoryWise")
        {
            Categorypannel.Visible = false;
        }
        else
        {
            Categorypannel.Visible = true;
            //FillAgentName();
        }
    }
    void FillAgentName()
    {
        try
        {
            vdm = new VehicleDBMgr();
            //cmd = new MySqlCommand(" SELECT  sno, Categoryname, flag, userdata_sno, tcategory, categorycode, rank, description, tempcatsno FROM products_category");
            cmd = new MySqlCommand("select tempcatsno,description AS Categoryname from products_category where flag=@flag and userdata_sno=@username AND tempcatsno IS NOT NULL order by tempcatsno");
            cmd.Parameters.AddWithValue("@username", Session["userdata_sno"].ToString());
            cmd.Parameters.AddWithValue("@flag", "1");
            DataTable dtCategory = vdm.SelectQuery(cmd).Tables[0];
            ddlCategoryName.DataSource = dtCategory;
            ddlCategoryName.DataTextField = "Categoryname";
            ddlCategoryName.DataValueField = "tempcatsno";
            ddlCategoryName.DataBind();
            ddlCategoryName.Items.Insert(0, new ListItem("ALL", "ALL"));
            //Categorypannel.Visible = false;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
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
    DataTable dtSortedSubCategory = new DataTable();
    DataTable dtSortedCategory = new DataTable();
    DataTable dttempproducts = new DataTable();
    DataTable produtstbl1 = new DataTable();
    DataTable dtSubCatgory = new DataTable();
    DataTable dtCatgory = new DataTable();
    DataTable dtSortedCategoryAndSubCat = new DataTable();

    DataTable dtCatgoryAndSub = new DataTable();


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
            if (ddlType.SelectedValue == "Productwise")
            {
                if (ddlCategoryName.SelectedValue == "ALL")
                {
                    cmd = new MySqlCommand("SELECT   products_subcategory.rank,branchdata.BranchName,products_subcategory.tempcatsno, productsdata.tempsubcatsno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS SaleValue, products_category.description AS Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.tempsubcatsno = products_subcategory.tempsub_catsno INNER JOIN products_category ON products_subcategory.tempcatsno = products_category.tempcatsno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty>0)  GROUP BY branchdata.BranchName, productsdata.ProductName");
                    //cmd = new MySqlCommand("SELECT branchdata.BranchName,productsdata.tempsubcatsno,productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty) ) AS SaleValue, products_category.Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN  branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (products_category.tempcatsno=@Catsno) GROUP BY branchdata.BranchName, productsdata.ProductName");
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@Catsno", ddlCategoryName.SelectedValue);

                }
                else
                {
                    cmd = new MySqlCommand("SELECT   products_subcategory.rank,branchdata.BranchName,products_subcategory.tempcatsno, productsdata.tempsubcatsno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS SaleValue, products_category.description AS Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.tempsubcatsno = products_subcategory.tempsub_catsno INNER JOIN products_category ON products_subcategory.tempcatsno = products_category.tempcatsno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (products_category.tempcatsno = @Catsno) AND (indents_subtable.DeliveryQty>0) GROUP BY branchdata.BranchName, productsdata.ProductName");
                    //cmd = new MySqlCommand("SELECT branchdata.BranchName,productsdata.tempsubcatsno,productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty) ) AS SaleValue, products_category.Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN  branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (products_category.tempcatsno=@Catsno) GROUP BY branchdata.BranchName, productsdata.ProductName");
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@Catsno", ddlCategoryName.SelectedValue);
                }

                DataTable dtdue = vdm.SelectQuery(cmd).Tables[0];
                ////cmd = new MySqlCommand("SELECT dispatch.sno, productsdata.ProductName, products_category.Categoryname, tripsubdata.ProductId, SUM(tripsubdata.Qty) AS DeliveryQty FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.Branch_Id = @BranchID) AND (dispatch.Route_id IS NULL) AND (dispatch.DispMode <> 'AGENT') GROUP BY productsdata.ProductName");
                // By Ravindra 02/02/2017


                cmd = new MySqlCommand("SELECT   products_subcategory.rank, branchdata.BranchName, products_subcategory.tempcatsno, productsdata.tempsubcatsno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS salevalue, products_category.description AS Categoryname FROM  (SELECT IndentNo, Branch_id, I_date, Status, IndentType FROM  indents WHERE (I_date BETWEEN @d1 AND @d2) AND (Status <> 'D')) indent INNER JOIN branchdata ON indent.Branch_id = branchdata.sno INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.tempsubcatsno = products_subcategory.tempsub_catsno INNER JOIN products_category ON products_subcategory.tempcatsno = products_category.tempcatsno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents_subtable.DeliveryQty <> 0) GROUP BY branchdata.sno, branchdata.BranchName, productsdata.ProductName ORDER BY branchdata.BranchName");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                cmd.Parameters.AddWithValue("@Catsno", ddlCategoryName.SelectedValue);
                DataTable DtLocalTemp = vdm.SelectQuery(cmd).Tables[0];


                cmd = new MySqlCommand("SELECT ff.Sno, Leaks.tempcatsno,Leaks.rank,Leaks.ProductName,Leaks.tempsubcatsno, Leaks.Categoryname, Leaks.ProductId, ROUND(SUM(Leaks.DeliveryQty),2) AS DeliveryQty FROM (SELECT tripdata.Sno, productsdata.ProductName,productsdata.tempsubcatsno, products_category.Categoryname,products_subcategory.rank, products_category.tempcatsno,tripsubdata.ProductId, tripsubdata.Qty AS DeliveryQty FROM tripsubdata INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN tripdata ON tripsubdata.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (tripsubdata.Qty>0)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno, I_Date FROM (SELECT dispatch.DispName, tripdata_1.Sno, dispatch.sno AS DespSno, tripdata_1.I_Date FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata tripdata_1 ON triproutes.Tripdata_sno = tripdata_1.Sno WHERE (dispatch.Branch_Id = @BranchID) AND (dispatch.Route_id IS NULL) AND (dispatch.DispType <> 'spl') AND (dispatch.DispType <> 'agent') AND (dispatch.DispType <> 'so')) TripInfo) ff ON ff.Sno = Leaks.Sno GROUP BY Leaks.ProductName");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                DataTable dtlocal = vdm.SelectQuery(cmd).Tables[0];
                dtlocal.Merge(DtLocalTemp);

                // cmd = new MySqlCommand("SELECT branchdata.BranchName, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, products_category.Categoryname FROM branchmappingtable INNER JOIN  branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (branchproducts.branch_sno = @Branch) GROUP BY branchdata.BranchName, productsdata.ProductName ORDER BY branchproducts.Rank");
                cmd = new MySqlCommand("SELECT branchproducts.branch_sno, branchproducts.product_sno, branchproducts.Rank, productsdata.ProductName, products_category.Categoryname FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) ORDER BY branchproducts.Rank");
                //cmd.Parameters.AddWithValue("@Flag", "1");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@Branch", Session["branch"].ToString());
                DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT    products_category.tempcatsno AS CatSno,productsdata.tempsubcatsno,branchproducts.unitprice,branchproducts.branch_sno,products_subcategory.tempsub_catsno AS SubCatSno, products_category.description AS Categoryname, branchproducts.product_sno AS sno, productsdata.ProductName, branchproducts.Rank,products_subcategory.description AS SubCategoryName FROM  products_category INNER JOIN products_subcategory ON products_category.tempcatsno = products_subcategory.tempcatsno INNER JOIN productsdata ON products_subcategory.tempsub_catsno = productsdata.tempsubcatsno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno WHERE (branchproducts.branch_sno = @BranchId) ORDER BY products_subcategory.rank,products_subcategory.tempsub_catsno");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                produtstbl1 = vdm.SelectQuery(cmd).Tables[0];

                if (produtstbl.Rows.Count > 0)
                {
                    DataView view1 = new DataView(produtstbl1);
                    DataView view = new DataView(dtdue);
                    DataTable distinctproducts = view1.ToTable(true, "ProductName", "Categoryname");
                    Report = new DataTable();
                    Report.Columns.Add("SNo");
                    Report.Columns.Add("AgentName");
                    int count = 0;
                    foreach (DataRow dr in distinctproducts.Rows)
                    {
                        Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                        count++;
                    }
                    Report.Columns.Add("Total Milk").DataType = typeof(Double);
                    Report.Columns.Add("Total Curd&BM").DataType = typeof(Double); ;
                    Report.Columns.Add("Total BiProducts").DataType = typeof(Double); ;
                    Report.Columns.Add("Total Beverages").DataType = typeof(Double); ;
                    Report.Columns.Add("Total Sweets").DataType = typeof(Double); ;
                    Report.Columns.Add("Total IceCreams").DataType = typeof(Double); ;
                    Report.Columns.Add("Total FoodProducts").DataType = typeof(Double); ;

                    Report.Columns.Add("Total Others").DataType = typeof(Double); ;
                    Report.Columns.Add("Total Lts").DataType = typeof(Double); ;

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
                                    double Buttermilk = 0;
                                    double salevalue = 0;
                                    newrow[dr["ProductName"].ToString()] = dr["DeliveryQty"].ToString();
                                    //if (dr["Categoryname"].ToString() == "MILK")
                                    //{
                                    //double.TryParse(dr["DeliveryQty"].ToString(), out qtyvalue);
                                    //total += qtyvalue;
                                    if (dr["Categoryname"].ToString() == "Milk")
                                    {
                                        double.TryParse(dr["DeliveryQty"].ToString(), out assqty);
                                        total += assqty;
                                    }
                                    if (dr["Categoryname"].ToString() == "Curd")
                                    {
                                        double.TryParse(dr["DeliveryQty"].ToString(), out curdBm);
                                        totalcurdandBM += curdBm;
                                        //double.TryParse(dr["SaleValue"].ToString(), out salevalue);
                                        //totalsalevalue += salevalue;
                                    }
                                    if (dr["Categoryname"].ToString() == "BiProducts")
                                    {
                                        double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                        totalbBiproducts += Buttermilk;
                                        //double.TryParse(dr["SaleValue"].ToString(), out salevalue);
                                        //totalsalevalue += salevalue;
                                    }
                                    if (dr["Categoryname"].ToString() == "Sweets")
                                    {
                                        double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                        totalsweets += Buttermilk;
                                        //double.TryParse(dr["SaleValue"].ToString(), out salevalue);
                                        //totalsalevalue += salevalue;
                                    }
                                    if (dr["Categoryname"].ToString() == "FoodProduc")
                                    {
                                        double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                        totalFoodProducts += Buttermilk;
                                        //double.TryParse(dr["SaleValue"].ToString(), out salevalue);
                                        //totalsalevalue += salevalue;
                                    }
                                    if (dr["Categoryname"].ToString() == "Beverages")
                                    {
                                        double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                        totalBeverages += Buttermilk;
                                        //double.TryParse(dr["SaleValue"].ToString(), out salevalue);
                                        //totalsalevalue += salevalue;
                                    }
                                    if (dr["Categoryname"].ToString() == "Others")
                                    {
                                        double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                        totalOthers += Buttermilk;
                                        //double.TryParse(dr["SaleValue"].ToString(), out salevalue);
                                        //totalsalevalue += salevalue;
                                    }
                                    if (dr["Categoryname"].ToString() == "IceCream")
                                    {
                                        double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                        totalIceCream += Buttermilk;
                                        //double.TryParse(dr["SaleValue"].ToString(), out salevalue);
                                        //totalsalevalue += salevalue;
                                    }
                                    int rank = 0;
                                    DataRow tempnewrow = dttempproducts.NewRow();
                                    tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                    tempnewrow["SubCatSno"] = dr["tempsubcatsno"].ToString();
                                    tempnewrow["CatSno"] = dr["tempcatsno"].ToString();
                                    int.TryParse(dr["rank"].ToString(), out rank); ;
                                    tempnewrow["rank"] = rank;
                                    dttempproducts.Rows.Add(tempnewrow);
                                }
                                //}
                            }
                        }
                        if (total != 0)
                        {
                            newrow["Total Milk"] = total;
                        }
                        if (totalcurdandBM != 0)
                        {

                            newrow["Total Curd&BM"] = totalcurdandBM;
                        }
                        if (totalbBiproducts != 0)
                        {
                            newrow["Total BiProducts"] = totalbBiproducts;

                        }
                        if (totalBeverages != 0)
                        {
                            newrow["Total Beverages"] = totalBeverages;

                        }
                        if (totalFoodProducts != 0)
                        {
                            newrow["Total FoodProducts"] = totalFoodProducts;

                        }
                        if (totalIceCream != 0)
                        {
                            newrow["Total IceCreams"] = totalIceCream;

                        }
                        if (totalOthers != 0)
                        {
                            newrow["Total Others"] = totalOthers;

                        }
                        if (totalsweets != 0)
                        {
                            newrow["Total Sweets"] = totalsweets;

                        }
                        newrow["Total Lts"] = total + totalcurdandBM + totalsweets + totalOthers + totalIceCream + totalFoodProducts + totalBeverages + totalbBiproducts;
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

                    cmd = new MySqlCommand("SELECT  products_subcategory.sno, products_subcategory.category_sno, products_subcategory.SubCatName, products_subcategory.Flag, products_subcategory.userdata_sno, products_subcategory.fat,products_subcategory.rank, products_subcategory.tempcatsno, products_subcategory.tempsub_catsno, products_category.description FROM products_subcategory INNER JOIN products_category ON products_subcategory.tempcatsno = products_category.tempcatsno WHERE (products_subcategory.tempsub_catsno IS NOT NULL) AND (products_subcategory.tempsub_catsno <> '0')");
                    DataTable dtcategory = vdm.SelectQuery(cmd).Tables[0];
                    foreach (DataRow dr in dtlocal.Rows)
                    {
                        DataRow tempnewrow = temptable.NewRow();
                        tempnewrow["Sno"] = dr["Sno"].ToString();
                        foreach (DataRow drr in dtcategory.Select("tempsub_catsno='" + dr["tempsubcatsno"].ToString() + "'"))
                        {
                            tempnewrow["tempcatsno"] = drr["tempcatsno"].ToString();
                            tempnewrow["Categoryname"] = drr["description"].ToString();
                            tempnewrow["rank"] = drr["rank"].ToString();
                        }
                        tempnewrow["ProductName"] = dr["ProductName"].ToString();
                        tempnewrow["tempsubcatsno"] = dr["tempsubcatsno"].ToString();
                        tempnewrow["DeliveryQty"] = dr["DeliveryQty"].ToString();
                        temptable.Rows.Add(tempnewrow);
                    }
                    double totalbBiproducts1 = 0;
                    double totalsweets1 = 0;
                    double totalFoodProducts1 = 0;
                    double totalBeverages1 = 0;
                    double totalOthers1 = 0;
                    double totalIceCream1 = 0;
                    if (ddlCategoryName.SelectedValue == "ALL")
                    {
                        foreach (DataRow dr in temptable.Rows)
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
                                newrow1[dr["ProductName"].ToString()] = dr["DeliveryQty"].ToString();
                                if (dr["Categoryname"].ToString() == "Milk")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out assqty);
                                    total1 += assqty;
                                }
                                if (dr["Categoryname"].ToString() == "Curd")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out curdBm);
                                    totalcurdandBM1 += curdBm;
                                }
                                if (dr["Categoryname"].ToString() == "BiProducts")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                    totalbBiproducts1 += Buttermilk;
                                }
                                if (dr["Categoryname"].ToString() == "Sweets")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                    totalsweets1 += Buttermilk;
                                }
                                if (dr["Categoryname"].ToString() == "FoodProduc")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                    totalFoodProducts1 += Buttermilk;
                                }
                                if (dr["Categoryname"].ToString() == "Beverages")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                    totalBeverages1 += Buttermilk;
                                }
                                if (dr["Categoryname"].ToString() == "Others")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                    totalOthers1 += Buttermilk;
                                }
                                if (dr["Categoryname"].ToString() == "IceCream")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                    totalIceCream1 += Buttermilk;
                                }
                                int rank = 0;
                                DataRow tempnewrow = dttempproducts.NewRow();
                                tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                tempnewrow["SubCatSno"] = dr["tempsubcatsno"].ToString();
                                tempnewrow["CatSno"] = dr["tempcatsno"].ToString();
                                int.TryParse(dr["rank"].ToString(), out rank); ;
                                tempnewrow["rank"] = rank;
                                dttempproducts.Rows.Add(tempnewrow);
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in temptable.Select("tempcatsno='" + ddlCategoryName.SelectedValue + "'"))
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
                                if (dr["Categoryname"].ToString() == "Milk")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out assqty);
                                    total1 += assqty;
                                }
                                if (dr["Categoryname"].ToString() == "Curd")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out curdBm);
                                    totalcurdandBM1 += curdBm;

                                }
                                if (dr["Categoryname"].ToString() == "BiProducts")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                    totalbBiproducts1 += Buttermilk;
                                }
                                if (dr["tempcatsno"].ToString() == "4")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                    totalsweets1 += Buttermilk;
                                }
                                if (dr["Categoryname"].ToString() == "FoodProduc")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                    totalFoodProducts1 += Buttermilk;
                                }
                                if (dr["Categoryname"].ToString() == "Beverages")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                    totalBeverages1 += Buttermilk;
                                }
                                if (dr["Categoryname"].ToString() == "Others")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                    totalOthers1 += Buttermilk;
                                }
                                if (dr["Categoryname"].ToString() == "IceCream")
                                {
                                    double.TryParse(dr["DeliveryQty"].ToString(), out Buttermilk);
                                    totalIceCream1 += Buttermilk;
                                }
                                int rank = 0;
                                DataRow tempnewrow = dttempproducts.NewRow();
                                tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                tempnewrow["SubCatSno"] = dr["tempsubcatsno"].ToString();
                                tempnewrow["CatSno"] = dr["tempcatsno"].ToString();
                                tempnewrow["rank"] = int.TryParse(dr["rank"].ToString(), out rank); ;
                                dttempproducts.Rows.Add(tempnewrow);
                            }
                        }
                    }
                    if (totalbBiproducts1 != 0)
                    {
                        newrow1["Total BiProducts"] = totalbBiproducts1;
                    }
                    if (totalBeverages1 != 0)
                    {
                        newrow1["Total Beverages"] = totalBeverages1;
                    }
                    if (totalFoodProducts1 != 0)
                    {
                        newrow1["Total FoodProducts"] = totalFoodProducts1;
                    }
                    if (totalIceCream1 != 0)
                    {
                        newrow1["Total IceCreams"] = totalIceCream1;
                    }
                    if (totalOthers1 != 0)
                    {
                        newrow1["Total Others"] = totalOthers1;
                    }
                    if (totalsweets1 != 0)
                    {
                        newrow1["Total Sweets"] = totalsweets1;
                    }
                    if (total1 != 0)
                    {
                        newrow1["Total Milk"] = total1;
                    }
                    if (totalcurdandBM1 != 0)
                    {
                        newrow1["Total Curd&BM"] = totalcurdandBM1;
                    }
                    newrow1["Total Lts"] = total1 + totalcurdandBM1 + totalsweets1 + totalOthers1 + totalIceCream1 + totalFoodProducts1 + totalBeverages1 + totalbBiproducts1; ;
                    Report.Rows.Add(newrow1);
                    i++;

                    DataView SubCatview = new DataView(dttempproducts);
                    dtSubCatgory = SubCatview.ToTable(true, "SubCatSno");
                    DataView dv1 = dtSubCatgory.DefaultView;
                    dv1.Sort = "SubCatSno ASC";
                    dtSortedSubCategory = dv1.ToTable();


                    DataView SubCatview1 = new DataView(dttempproducts);
                    dtCatgory = SubCatview1.ToTable(true, "CatSno");
                    DataView dv2 = dtCatgory.DefaultView;
                    dv2.Sort = "CatSno ASC";
                    dtSortedCategory = dv2.ToTable();
                    foreach (var column in Report.Columns.Cast<DataColumn>().ToArray())
                    {
                        if (Report.AsEnumerable().All(dr => dr.IsNull(column)))
                            Report.Columns.Remove(column);
                    }

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
            else
            {


                cmd = new MySqlCommand("SELECT   products_subcategory.rank,branchdata.BranchName,products_subcategory.tempcatsno, productsdata.tempsubcatsno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS SaleValue, products_category.description AS Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.tempsubcatsno = products_subcategory.tempsub_catsno INNER JOIN products_category ON products_subcategory.tempcatsno = products_category.tempcatsno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty>0) AND  (products_subcategory.tempcatsno NOT IN ('3,7'))  GROUP BY branchdata.BranchName,products_category.tempcatsno");
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
                cmd.Parameters.AddWithValue("@Catsno", ddlCategoryName.SelectedValue);
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
                    string tempcat="";
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
                    Report.Columns.Add("Total").DataType = typeof(Double); ;

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

                        newrow["Total"] = total;
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
                    newrow1["Total"] = ltotal;
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
                    
                    grdReports.DataSource = Report;
                    grdReports.DataBind();

                    GridView1.DataSource = dtsummary;
                    GridView1.DataBind();
                    
                }
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
            if (ddlType.SelectedValue == "Productwise")
            {
                //j++;
                //e.Row.Cells[1].Visible = false;
                GridView ProductGrid = (GridView)sender;
                GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Separator, DataControlRowState.Insert);
                GridViewRow HeaderRow1 = new GridViewRow(0, 0, DataControlRowType.Separator, DataControlRowState.Insert);
                TableCell HeaderCell1 = new TableCell();
                HeaderCell1 = new TableCell();
                HeaderCell1.Text = "Net Sales Details";
                HeaderCell1.VerticalAlign = VerticalAlign.Middle;
                HeaderCell1.ColumnSpan = 2;
                //}// For merging three columns (Direct, Referral, Total)
                HeaderCell1.CssClass = "HeaderStyle";
                HeaderRow1.Cells.Add(HeaderCell1);
                if (ddlCategoryName.SelectedValue != "ALL")
                {

                    TableCell HeaderCell = new TableCell();
                    int i = 3;
                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Net Sales Details";
                    HeaderCell.VerticalAlign = VerticalAlign.Middle;
                    HeaderCell.ColumnSpan = 2;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderRow.Cells.Add(HeaderCell);
                    foreach (DataRow drsubcategory in dtSortedSubCategory.Rows)
                    {
                        DataTable distinctTable = dttempproducts.DefaultView.ToTable(true, "SubCatSno", "ProductName", "CatSno");
                        //DataView SubAndCatview = new DataView(dttempproducts);
                        //dtCatgoryAndSub = SubAndCatview.ToTable(true, "SubCatSno", "ProductName", "CatSno", "rank");
                        //DataView dv3 = dtCatgoryAndSub.DefaultView;
                        //dv3.Sort = "rank ASC";
                        //dtSortedCategoryAndSubCat = dv3.ToTable();

                        int k = 0;
                        foreach (DataRow dramount in distinctTable.Select("SubCatSno='" + drsubcategory["SubCatSno"].ToString() + "'"))
                        {
                            //foreach (DataRow dramount in dtBranch.Select("ProductName='" + ItemName + "' AND SubCatSno='" + drsubcategory["SubCatSno"].ToString() + "'"))
                            //{
                            //    string Temp = dramount["ProductName"].ToString();
                            //}
                            //Adding Year Column
                            k++;
                        }
                        HeaderCell = new TableCell();
                        if (k != 0)
                        {
                            foreach (DataRow dramount in produtstbl1.Select("SubCatSno='" + drsubcategory["SubCatSno"].ToString() + "'"))
                            {
                                HeaderCell.Text = dramount["SubCategoryName"].ToString();
                            }
                        }
                        HeaderCell.VerticalAlign = VerticalAlign.Middle;
                        HeaderCell.ColumnSpan = k; // For merging three columns (Direct, Referral, Total)
                        HeaderCell.CssClass = "HeaderStyle";
                        //HeaderCell.ForeColor = col
                        HeaderRow.Cells.Add(HeaderCell);
                    }
                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Net Totals";
                    HeaderCell.VerticalAlign = VerticalAlign.Middle;
                    HeaderCell.ColumnSpan = 8;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderRow.Cells.Add(HeaderCell);
                    ProductGrid.Controls[0].Controls.AddAt(0, HeaderRow);
                }
                foreach (DataRow drcategory in dtSortedCategory.Rows)
                {

                    //DataTable distinctCatAndSubCatTable = dttempproducts.DefaultView.ToTable(true, "SubCatSno", "ProductName", "CatSno", "rank");
                    DataView SubAndCatview = new DataView(dttempproducts);
                    dtCatgoryAndSub = SubAndCatview.ToTable(true, "SubCatSno", "ProductName", "CatSno", "rank");
                    DataView dv3 = dtCatgoryAndSub.DefaultView;
                    dv3.Sort = "rank ASC";
                    dtSortedCategoryAndSubCat = dv3.ToTable();
                    int L = 0;
                    foreach (DataRow dramount in dtSortedCategoryAndSubCat.Select("CatSno='" + drcategory["CatSno"].ToString() + "'"))
                    {
                        L++;
                    }
                    HeaderCell1 = new TableCell();
                    if (L != 0)
                    {
                        foreach (DataRow dramount in produtstbl1.Select("CatSno='" + drcategory["CatSno"].ToString() + "'"))
                        {
                            HeaderCell1.Text = dramount["Categoryname"].ToString();
                        }
                        HeaderCell1.VerticalAlign = VerticalAlign.Middle;
                        HeaderCell1.ColumnSpan = L; // For merging three columns (Direct, Referral, Total)
                        HeaderCell1.CssClass = "HeaderStyle";
                        HeaderRow1.Cells.Add(HeaderCell1);
                    }
                }
                //HeaderCell1 = new TableCell();
                //HeaderCell1.Text = "Net Totals";
                //HeaderCell1.VerticalAlign = VerticalAlign.Middle;
                //HeaderCell1.ColumnSpan = 8;
                //HeaderCell1.CssClass = "HeaderStyle";
                //HeaderRow1.Cells.Add(HeaderCell1);
                ProductGrid.Controls[0].Controls.AddAt(0, HeaderRow1);
            }
        }
        // }
        //}
    }
}