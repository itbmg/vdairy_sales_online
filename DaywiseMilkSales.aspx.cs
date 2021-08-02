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

public partial class Day_wise_Milk_Sales : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    int count = 0; double previousvalue = 0; int j = 0;
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
                lblTitle.Text = Session["TitleName"].ToString();
                txtfromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txttodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                LoadCategoryName();
            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
            GetReport();
    }
    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCategoryName();
        //ddlSubCategoryName.Items.Insert(0, new ListItem("ALL", "ALL"));
    }
    protected void LoadCategoryName()
    {
        vdm = new VehicleDBMgr();
        string type = ddlReportType.SelectedValue;
        cmd = new MySqlCommand("SELECT    sno, category_sno, SubCatName, Flag, userdata_sno, fat, description, rank, tempcatsno, tempsub_catsno FROM  products_subcategory WHERE (Flag = @flag) AND (tempsub_catsno IS NOT NULL) AND (tempsub_catsno <> '0') AND (tempcatsno = @CatSno) ORDER BY tempsub_catsno");
        cmd.Parameters.AddWithValue("@CatSno", ddlReportType.SelectedValue);
        cmd.Parameters.AddWithValue("@flag", "1");
        DataTable dtCategory = vdm.SelectQuery(cmd).Tables[0];
        ddlSubCategoryName.DataSource = dtCategory;
        ddlSubCategoryName.DataTextField = "description";
        ddlSubCategoryName.DataValueField = "tempsub_catsno";
        ddlSubCategoryName.DataBind();
        ddlSubCategoryName.Items.Insert(0, new ListItem("ALL", "ALL"));

    }
    protected void btnSMS_Click(object sender, EventArgs e)
    {
        string MobNo = txtMobNo.Text;
        if (MobNo.Length == 10)
        {
            vdm = new VehicleDBMgr();
            DataTable Report = new DataTable();
            DateTime fromdate = DateTime.Now;
            string[] dateFromstrig = txtfromdate.Text.Split(' ');
            if (dateFromstrig.Length > 1)
            {
                if (dateFromstrig[0].Split('-').Length > 0)
                {
                    string[] dates = dateFromstrig[0].Split('-');
                    string[] times = dateFromstrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            DateTime Todate = DateTime.Now;
            string[] dateTostrig = txttodate.Text.Split(' ');
            if (dateTostrig.Length > 1)
            {
                if (dateTostrig[0].Split('-').Length > 0)
                {
                    string[] dates = dateTostrig[0].Split('-');
                    string[] times = dateTostrig[1].Split(':');
                    Todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            lbl_selfromdate.Text = fromdate.ToString("dd/MM/yyyy");
            lbl_selttodate.Text = Todate.ToString("dd/MM/yyyy");
            Session["filename"] = "TOTAL DC REPORT";



            cmd = new MySqlCommand("SELECT ROUND(SUM(tripsubdata.Qty), 2) AS Qty, productsdata.ProductName FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, Status FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN (SELECT branch_sno, product_sno, unitprice, flag, userdata_sno, DTarget, WTarget, MTarget, BranchQty, LeakQty, Rank FROM branchproducts WHERE (branch_sno = @branch)) brnchprdt ON tripsubdata.ProductId = brnchprdt.product_sno INNER JOIN productsdata ON brnchprdt.product_sno = productsdata.sno WHERE (dispatch.Branch_Id = @branch) GROUP BY productsdata.sno ORDER BY brnchprdt.Rank");
            cmd.Parameters.AddWithValue("@branch", Session["branch"]);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            DataTable dtTotalDespatch = vdm.SelectQuery(cmd).Tables[0];
            double TotalQty = 0;
            string ProductName = "";
            if (dtTotalDespatch.Rows.Count > 0)
            {
                foreach (DataRow dr in dtTotalDespatch.Rows)
                {
                    double unitQty = 0;
                    double.TryParse(dr["Qty"].ToString(), out unitQty);
                    ProductName += dr["ProductName"].ToString() + "->" + Math.Round(unitQty, 2) + ";" + "\r\n";
                    TotalQty += Math.Round(unitQty, 2);
                }
            }

            string Date = DateTime.Now.ToString("dd/MM/yyyy");
            WebClient client = new WebClient();
            string DispatchName = "SRIKALAHASTHI";
            //Stream data = client.OpenRead(baseurl);
            //StreamReader reader = new StreamReader(data);
            //string ResponseID = reader.ReadToEnd();
            //data.Close();
            //reader.Close();

            cmd = new MySqlCommand("SELECT ROUND(SUM(tripsubdata.Qty), 2) AS Qty, products_subcategory.SubCatName, products_category.Categoryname, products_category.sno AS categorysno, products_subcategory.sno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, Status FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.Branch_Id = @branch) GROUP BY categorysno, products_subcategory.sno ORDER BY categorysno");
            cmd.Parameters.AddWithValue("@branch", Session["branch"]);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            DataTable dtTotalDespatch_subcategorywise = vdm.SelectQuery(cmd).Tables[0];
            double SubCategoryTotalQty = 0;
            string subcategoryName = "";
            if (dtTotalDespatch_subcategorywise.Rows.Count > 0)
            {
                foreach (DataRow dr in dtTotalDespatch_subcategorywise.Rows)
                {
                    double unitQty = 0;
                    double.TryParse(dr["Qty"].ToString(), out unitQty);
                    if (dr["categorysno"].ToString() == "10")
                    {
                        subcategoryName += dr["SubCatName"].ToString() + "CURD" + "->" + Math.Round(unitQty, 2) + ";" + "\r\n";
                    }
                    else
                    {
                        subcategoryName += dr["SubCatName"].ToString() + "->" + Math.Round(unitQty, 2) + ";" + "\r\n";
                    }
                    SubCategoryTotalQty += Math.Round(unitQty, 2);
                }
            }
            WebClient client1 = new WebClient();
            //Stream data1 = client.OpenRead(baseurl1);
            //StreamReader reader1 = new StreamReader(data1);
            //string ResponseID1 = reader1.ReadToEnd();
            //data1.Close();
            //reader1.Close();

            lblmsg.Text = "Message Sent Successfully";
            txtMobNo.Text = "";
        }
        else
        {
            lblmsg.Text = "Please Enter 10 digit Number";
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
    DataTable Report = new DataTable();
    DataTable dtble1 = new DataTable();
    DataTable dtdirect = new DataTable();
    DataTable dtcurdBM = new DataTable();
    DataTable dtdirectcurdBM = new DataTable();
    DataTable distincttable = new DataTable();
    DataTable sortedProductDT1 = new DataTable();
    DataTable distinctproducts = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            Report = new DataTable();
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            string[] dateFromstrig = txtfromdate.Text.Split(' ');
            if (dateFromstrig.Length > 1)
            {
                if (dateFromstrig[0].Split('-').Length > 0)
                {
                    string[] dates = dateFromstrig[0].Split('-');
                    string[] times = dateFromstrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            DateTime Todate = DateTime.Now;
            string[] dateTostrig = txttodate.Text.Split(' ');
            if (dateTostrig.Length > 1)
            {
                if (dateTostrig[0].Split('-').Length > 0)
                {
                    string[] dates = dateTostrig[0].Split('-');
                    string[] times = dateTostrig[1].Split(':');
                    Todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            lbl_selfromdate.Text = fromdate.ToString("dd/MM/yyyy");
            lbl_selttodate.Text = Todate.ToString("dd/MM/yyyy");
            Session["filename"] = "DAY WISE MILK SALE REPORT";
            TimeSpan dateSpan = Todate.Subtract(fromdate);
            int NoOfdays = dateSpan.Days;
            NoOfdays = NoOfdays + 1;
            DataTable dtsubcategory = new DataTable();
            //if (ddlReportType.SelectedValue == "1")
            //{
            if (ddlSubCategoryName.SelectedValue == "ALL")
            {
                cmd = new MySqlCommand("SELECT  products_subcategory.rank,DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, products_subcategory.tempsub_catsno,products_subcategory.description AS SubCatName,  productsdata.tempsubcatsno, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS SaleValue FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.tempsubcatsno = products_subcategory.tempsub_catsno INNER JOIN products_category ON products_subcategory.tempcatsno = products_category.tempcatsno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty > 0) AND (products_category.tempcatsno = @CatSno) GROUP BY IndentDate,products_subcategory.tempsub_catsno ORDER BY products_subcategory.tempsub_catsno");
                //  cmd = new MySqlCommand("SELECT  ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.Product_sno, products_subcategory.sno, products_subcategory.SubCatName, DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate FROM  branchmappingtable branchmappingtable_1 INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchmappingtable ON branchmappingtable_1.SuperBranch = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (products_category.sno = '9') AND  (branchmappingtable_1.SuperBranch <> 538) GROUP BY IndentDate, products_subcategory.sno");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                cmd.Parameters.AddWithValue("@CatSno", ddlReportType.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
                dtble1 = vdm.SelectQuery(cmd).Tables[0];
                //cmd = new MySqlCommand("SELECT SUM(tripsubdata.Qty) AS dispqty, products_category.Categoryname, products_subcategory.SubCatName, result.IndentDate FROM (SELECT dispatch.sno, dispatch.DispName, tripdat.Sno AS tripid, DATE_FORMAT(tripdat.I_Date, '%d %b %y') AS IndentDate FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, Status, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno WHERE (dispatch.Branch_Id = @branch) AND (dispatch.DispMode IS NOT NULL) AND (dispatch.DispMode <> 'SPL')) result INNER JOIN tripsubdata ON result.tripid = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (products_category.sno = 9) GROUP BY products_subcategory.sno, result.IndentDate");
                //cmd.Parameters.AddWithValue("@branch", Session["branch"]);
                //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                //cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
                //dtdirect = vdm.SelectQuery(cmd).Tables[0];
                DataView dv = dtble1.DefaultView;
                dv.Sort = "IndentDate ASC";
                DataTable sortedProductDT = dv.ToTable();
                if (sortedProductDT.Rows.Count > 0)
                {
                    DataView view = new DataView(sortedProductDT);
                    DataTable distinctproducts = view.ToTable(true, "SubCatName", "tempsub_catsno");
                    DataView dv1 = distinctproducts.DefaultView;
                    dv1.Sort = "tempsub_catsno ASC";
                    sortedProductDT1 = dv1.ToTable();

                    Report = new DataTable();
                    Report.Columns.Add("SNo");
                    Report.Columns.Add("Date");
                    int count = 0;
                    foreach (DataRow dr in sortedProductDT1.Rows)
                    {
                        Report.Columns.Add(dr["SubCatName"].ToString()).DataType = typeof(Double);
                    }
                    Report.Columns.Add("Total").DataType = typeof(Double);
                    DataTable distincttable = view.ToTable(true, "IndentDate");
                    int i = 1;
                    foreach (DataRow branch in distincttable.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["SNo"] = i;
                        string AssignDate = branch["IndentDate"].ToString();
                        DateTime dtAssignDate = Convert.ToDateTime(AssignDate).AddDays(1);
                        string AssigDate = dtAssignDate.ToString("dd MMM yyyy");
                        newrow["Date"] = AssigDate;
                        double totmilk = 0;
                        foreach (DataRow dr in sortedProductDT.Rows)
                        {
                            double directdel = 0;
                            if (dr["IndentDate"].ToString() == AssignDate)
                            {
                                //foreach (DataRow drdtdirect in dtdirect.Select("IndentDate='" + AssignDate + "'"))
                                //{
                                //    if (drdtdirect["SubCatName"].ToString() == dr["SubCatName"].ToString())
                                //    {
                                //        double.TryParse(drdtdirect["dispqty"].ToString(), out directdel);
                                //    }
                                //}
                                double delqty = 0;
                                double.TryParse(dr["DeliveryQty"].ToString(), out delqty);
                                newrow[dr["SubCatName"].ToString()] = Math.Round(delqty + directdel, 2);
                                totmilk += delqty + directdel;

                            }
                        }
                        newrow["Total"] = Math.Round(totmilk, 2);
                        Report.Rows.Add(newrow);
                        i++;
                    }
                }
            }
            else
            {
               // AND (branchmappingtable.SubBranch NOT IN (538, 2749, 3928, 1801, 3625))
                if (Session["branch"].ToString() == "172")
                {
                    cmd = new MySqlCommand("SELECT  products_subcategory.rank,DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, products_subcategory.tempcatsno, productsdata.tempsubcatsno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS SaleValue, products_category.description AS Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.tempsubcatsno = products_subcategory.tempsub_catsno INNER JOIN products_category ON products_subcategory.tempcatsno = products_category.tempcatsno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (branchmappingtable.SubBranch NOT IN (538, 2749, 3928, 1801, 3625)) AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty > 0) AND (products_subcategory.tempsub_catsno = @SubCatsno) GROUP BY IndentDate,productsdata.ProductName ORDER BY products_subcategory.tempsub_catsno,productsdata.rank");
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@SubCatsno", ddlSubCategoryName.SelectedValue);
                }
                else
                {
                    cmd = new MySqlCommand("SELECT  products_subcategory.rank,DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, products_subcategory.tempcatsno, productsdata.tempsubcatsno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS SaleValue, products_category.description AS Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.tempsubcatsno = products_subcategory.tempsub_catsno INNER JOIN products_category ON products_subcategory.tempcatsno = products_category.tempcatsno WHERE (branchmappingtable.SuperBranch = @BranchID)  AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty > 0) AND (products_subcategory.tempsub_catsno = @SubCatsno) GROUP BY IndentDate,productsdata.ProductName ORDER BY products_subcategory.tempsub_catsno,productsdata.rank");
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@SubCatsno", ddlSubCategoryName.SelectedValue);
                }
                dtsubcategory = vdm.SelectQuery(cmd).Tables[0];
                //cmd = new MySqlCommand("SELECT SUM(tripsubdata.Qty) AS dispqty, products_category.Categoryname, products_subcategory.SubCatName, result.IndentDate FROM (SELECT dispatch.sno, dispatch.DispName, tripdat.Sno AS tripid, DATE_FORMAT(tripdat.I_Date, '%d %b %y') AS IndentDate FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, Status, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno WHERE (dispatch.Branch_Id = @branch) AND (dispatch.DispMode IS NOT NULL) AND (dispatch.DispMode <> 'SPL')) result INNER JOIN tripsubdata ON result.tripid = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (products_category.sno = 9) GROUP BY products_subcategory.sno, result.IndentDate");
                //cmd.Parameters.AddWithValue("@branch", Session["branch"]);
                //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                //cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
                //dtdirect = vdm.SelectQuery(cmd).Tables[0];
                DataView dv = dtsubcategory.DefaultView;
                dv.Sort = "IndentDate ASC";
                DataTable sortedProductDT = dv.ToTable();
                if (sortedProductDT.Rows.Count > 0)
                {
                    DataView view = new DataView(sortedProductDT);
                    distinctproducts = view.ToTable(true, "ProductName");
                    Report = new DataTable();
                    Report.Columns.Add("SNo");
                    Report.Columns.Add("Date");
                    int count = 0;
                    foreach (DataRow dr in distinctproducts.Rows)
                    {
                        Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                    }
                    Report.Columns.Add("Total");
                    DataTable distincttable = view.ToTable(true, "IndentDate");
                    int i = 1;
                    foreach (DataRow branch in distincttable.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["SNo"] = i;
                        string AssignDate = branch["IndentDate"].ToString();
                        DateTime dtAssignDate = Convert.ToDateTime(AssignDate).AddDays(1);
                        string AssigDate = dtAssignDate.ToString("dd MMM yyyy");
                        newrow["Date"] = AssigDate;
                        double totmilk = 0;
                        foreach (DataRow dr in sortedProductDT.Rows)
                        {
                            double directdel = 0;
                            if (dr["IndentDate"].ToString() == AssignDate)
                            {
                                //foreach (DataRow drdtdirect in dtdirect.Select("IndentDate='" + AssignDate + "'"))
                                //{
                                //    if (drdtdirect["SubCatName"].ToString() == dr["SubCatName"].ToString())
                                //    {
                                //        double.TryParse(drdtdirect["dispqty"].ToString(), out directdel);
                                //    }
                                //}
                                double delqty = 0;
                                double.TryParse(dr["DeliveryQty"].ToString(), out delqty);
                                newrow[dr["ProductName"].ToString()] = Math.Round(delqty + directdel, 2);
                                totmilk += delqty + directdel;
                            }
                        }
                        newrow["Total"] = Math.Round(totmilk, 2);
                        Report.Rows.Add(newrow);
                        i++;
                    }
                }
            }
            DataRow newvartical = Report.NewRow();
            newvartical["Date"] = "Total";
            DataRow newAvg = Report.NewRow();
            newAvg["Date"] = "Avg Per Day";
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
            //}
            //if (ddlReportType.SelectedValue == "2")
            //{
            //    //cmd = new MySqlCommand("SELECT ind.IndentNo, DATE_FORMAT(ind.I_date, '%d %b %y') AS IndentDate, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.Product_sno,products_subcategory.sno, products_subcategory.SubCatName, products_category.sno AS categorysno FROM branchmappingtable INNER JOIN branchmappingtable branchmappingtable_1 ON branchmappingtable.SubBranch = branchmappingtable_1.SuperBranch INNER JOIN (SELECT IndentNo, Branch_id, TotalQty, TotalPrice, I_date, D_date, Status, UserData_sno, PaymentStatus, I_createdby, I_modifiedby, IndentType FROM indents WHERE (I_date BETWEEN @d1 AND @d2)) ind ON branchmappingtable_1.SubBranch = ind.Branch_id INNER JOIN indents_subtable ON ind.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchmappingtable.SuperBranch = @branch) AND (products_category.sno = 10) OR (branchmappingtable.SuperBranch = @branch) AND (products_category.sno = 12) GROUP BY IndentDate, products_category.sno");
            //    //cmd = new MySqlCommand("SELECT ind.IndentNo, DATE_FORMAT(ind.I_date, '%d %b %y') AS IndentDate, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.Product_sno,products_subcategory.sno, products_subcategory.SubCatName, products_category.sno AS categorysno FROM branchmappingtable INNER JOIN branchmappingtable branchmappingtable_1 ON branchmappingtable.SubBranch = branchmappingtable_1.SuperBranch INNER JOIN (SELECT IndentNo, Branch_id, TotalQty, TotalPrice, I_date, D_date, Status, UserData_sno, PaymentStatus, I_createdby, I_modifiedby, IndentType FROM indents WHERE (I_date BETWEEN @d1 AND @d2)) ind ON branchmappingtable_1.SubBranch = ind.Branch_id INNER JOIN indents_subtable ON ind.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchmappingtable.SuperBranch = @branch) AND (products_category.sno = 10) AND (branchmappingtable_1.SuperBranch <> 538) OR (branchmappingtable.SuperBranch = @branch) AND (products_category.sno = 12) AND (branchmappingtable_1.SuperBranch <> 538) GROUP BY IndentDate, products_category.sno");
            //    cmd = new MySqlCommand("SELECT ind.IndentNo, DATE_FORMAT(ind.I_date, '%d %b %y') AS IndentDate, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.Product_sno,products_subcategory.sno, products_subcategory.SubCatName, products_category.sno AS categorysno FROM branchmappingtable INNER JOIN branchmappingtable branchmappingtable_1 ON branchmappingtable.SubBranch = branchmappingtable_1.SuperBranch INNER JOIN (SELECT IndentNo, Branch_id, TotalQty, TotalPrice, I_date, D_date, Status, UserData_sno, PaymentStatus, I_createdby, I_modifiedby, IndentType FROM indents WHERE (I_date BETWEEN @d1 AND @d2)) ind ON branchmappingtable_1.SubBranch = ind.Branch_id INNER JOIN indents_subtable ON ind.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchmappingtable.SuperBranch = @branch) AND (products_category.sno = 10) AND (branchmappingtable_1.SuperBranch <> 538) AND (productsdata.Units <> @units) OR (branchmappingtable.SuperBranch = @branch) AND (products_category.sno = 12) AND (branchmappingtable_1.SuperBranch <> 538) AND  (productsdata.Units <> @units) GROUP BY IndentDate, products_category.sno");
            //    cmd.Parameters.AddWithValue("@branch", Session["branch"]);
            //    cmd.Parameters.AddWithValue("@units", "ltr");
            //    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            //    cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
            //    dtcurdBM = vdm.SelectQuery(cmd).Tables[0];
            //    //cmd = new MySqlCommand("SELECT SUM(tripsubdata.Qty) AS dispqty, products_category.Categoryname,products_category.sno AS categorysno,result.IndentDate FROM (SELECT dispatch.sno, dispatch.DispName, tripdat.Sno AS tripid, DATE_FORMAT(tripdat.I_Date, '%d %b %y') AS IndentDate FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, Status, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno WHERE (dispatch.Branch_Id = @branch) AND (dispatch.DispMode IS NOT NULL) AND (dispatch.DispMode <> 'SPL')) result INNER JOIN tripsubdata ON result.tripid = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (products_category.sno = 10) OR (products_category.sno = 12) GROUP BY result.IndentDate, products_category.sno");
            //    //cmd.Parameters.AddWithValue("@branch", Session["branch"]);
            //    //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            //    //cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
            //    //dtdirectcurdBM = vdm.SelectQuery(cmd).Tables[0];
            //    if (dtcurdBM.Rows.Count > 0)
            //    {
            //        DataView view = new DataView(dtcurdBM);
            //        Report = new DataTable();
            //        Report.Columns.Add("SNo");
            //        Report.Columns.Add("Date");
            //        Report.Columns.Add("Curd", typeof(Double));
            //        Report.Columns.Add("Butter Milk", typeof(Double));
            //        Report.Columns.Add("Total Curd & BM", typeof(Double));
            //        DataTable distincttable = view.ToTable(true, "IndentDate");
            //        int i = 1;
            //        foreach (DataRow branch in distincttable.Rows)
            //        {
            //            DataRow newrow = Report.NewRow();
            //            newrow["SNo"] = i;
            //            string AssignDate = branch["IndentDate"].ToString();
            //            DateTime dtAssignDate = Convert.ToDateTime(AssignDate).AddDays(1);
            //            string AssigDate = dtAssignDate.ToString("dd MMM yyyy");

            //            newrow["Date"] = AssigDate;
            //            double totmilk = 0;

            //            foreach (DataRow dr in dtcurdBM.Rows)
            //            {
            //                double directdel = 0;
            //                if (dr["IndentDate"].ToString() == AssignDate)
            //                {
            //                    //foreach (DataRow drdtdirect in dtdirectcurdBM.Select("IndentDate='" + AssignDate + "'"))
            //                    //{
            //                    //    if (drdtdirect["categorysno"].ToString() == dr["categorysno"].ToString())
            //                    //    {
            //                    //        double.TryParse(drdtdirect["dispqty"].ToString(), out directdel);
            //                    //    }
            //                    //}
            //                    double delqty = 0;
            //                    double.TryParse(dr["DeliveryQty"].ToString(), out delqty);
            //                    if (dr["categorysno"].ToString() == "10")
            //                    {
            //                        newrow["Curd"] = Math.Round(delqty + directdel, 2);
            //                        totmilk += delqty + directdel;

            //                    }
            //                    if (dr["categorysno"].ToString() == "12")
            //                    {
            //                        double div = 0;
            //                        div = delqty + directdel;
            //                        newrow["Butter Milk"] = Math.Round(div / 3);
            //                        totmilk += div + directdel;

            //                    }

            //                }
            //            }
            //            newrow["Total Curd & BM"] = Math.Round(totmilk, 2);
            //            Report.Rows.Add(newrow);
            //            i++;
            //        }
            //    }
            //    DataRow newvartical = Report.NewRow();
            //    newvartical["Date"] = "Total";
            //    DataRow newAvg = Report.NewRow();
            //    newAvg["Date"] = "Avg Per Day";
            //    double Avgval = 0.0;
            //    double val = 0.0;
            //    foreach (DataColumn dc in Report.Columns)
            //    {
            //        if (dc.DataType == typeof(Double))
            //        {
            //            Avgval = 0.0;
            //            val = 0.0;
            //            double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
            //            newvartical[dc.ToString()] = val;
            //            Avgval = val / NoOfdays;
            //            newAvg[dc.ToString()] = Math.Round(Avgval, 2);
            //        }
            //    }
            //    Report.Rows.Add(newvartical);
            //    Report.Rows.Add(newAvg);

            //}



            grdtotal_dcReports.DataSource = Report;
            grdtotal_dcReports.DataBind();

            

            //for (int jj = 0; jj < grdtotal_dcReports.Rows.Count; jj++)
            //{
            //    DataGridViewRow  dgvr = 

            //    DataGridViewCell dr = 
            //    double previousvalue = 0;
            //    int i = 0;
            //    int count = 0;
            //    for (int rowcnt = 2; rowcnt < dgvr.Cells.Count; rowcnt++)
            //    {
            //        //int diff = 0;
            //        //int.TryParse(dgvr.Cells[rowcnt].Text, out diff);
            //        if (count == 0 || count == 1)
            //        {
            //            double presentvalue = 0;
            //            double.TryParse(dgvr.Cells[rowcnt].Text, out presentvalue);
            //            if (presentvalue < previousvalue)
            //            {
            //                dgvr.Cells[rowcnt].BackColor = Color.SandyBrown;
            //                // double.TryParse(dgvr.Cells[rowcnt].Text, out previousvalue);
            //            }
            //            else
            //            {
            //                dgvr.Cells[rowcnt].BackColor = Color.Green;
            //                double.TryParse(dgvr.Cells[rowcnt].Text, out previousvalue);
            //            }
            //            if (dgvr.Cells[1].Text.Contains("Avg Per Day"))
            //            {
            //                previousvalue = 0;
            //            }
            //            count++;
            //        }
            //        else
            //        {
            //            count = 0;
            //            previousvalue = 0;
            //            double presentvalue = 0;
            //            double.TryParse(dgvr.Cells[rowcnt].Text, out presentvalue);
            //            if (presentvalue < previousvalue)
            //            {
            //                dgvr.Cells[rowcnt].BackColor = Color.SandyBrown;
            //                // double.TryParse(dgvr.Cells[rowcnt].Text, out previousvalue);
            //            }
            //            else
            //            {
            //                dgvr.Cells[rowcnt].BackColor = Color.Green;
            //                double.TryParse(dgvr.Cells[rowcnt].Text, out previousvalue);
            //            }
            //            if (dgvr.Cells[1].Text.Contains("Avg Per Day"))
            //            {
            //                previousvalue = 0;
            //            }
            //            //double presentvalue = 0;
            //            //double.TryParse(dgvr.Cells[rowcnt].Text, out presentvalue);
            //            //if (presentvalue < previousvalue)
            //            //{
            //            //    dgvr.Cells[rowcnt].BackColor = Color.SandyBrown;
            //            //    // double.TryParse(dgvr.Cells[rowcnt].Text, out previousvalue);
            //            //}
            //            //else
            //            //{
            //            //    dgvr.Cells[rowcnt].BackColor = Color.Green;
            //            //    double.TryParse(dgvr.Cells[rowcnt].Text, out previousvalue);
            //            //}
            //            //if (dgvr.Cells[1].Text.Contains("Avg Per Day"))
            //            //{
            //            //    previousvalue = 0;
            //            //}
            //        }
            //}
            //    }
        }
        catch (Exception ex)
        {

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
            int count = 1;
            foreach (TableCell cell in grdtotal_dcReports.HeaderRow.Cells)
            {
                if (count == 2 || count == 4 || count == 5)
                {
                    dt.Columns.Add(cell.Text);

                }
                else
                {
                    dt.Columns.Add(cell.Text).DataType = typeof(double);
                }
                count++;
            }
            foreach (GridViewRow row in grdtotal_dcReports.Rows)
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
                wb.ColumnWidth = 5;
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
                    // HttpContext.Current.ApplicationInstance.CompleteRequest();
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void grdtotal_dcReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //e.Row.Cells[1].Visible = false;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            double presentvalue = 0;
            if (count == 0)
            {
                previousvalue = 0;
            }
            int last = e.Row.Cells.Count;

            double.TryParse(e.Row.Cells[last-1].Text, out presentvalue);
            if (presentvalue > previousvalue)
            {
                if (previousvalue == 0)
                {
                    previousvalue = presentvalue;
                    //e.Row.Cells[2].BackColor = System.Drawing.Color.Green;
                    //e.Row.Cells[2].Font.Size = FontUnit.Large;
                    //e.Row.Cells[2].ForeColor = Color.White;
                    //e.Row.Cells[2].Font.Bold = true;
                    if (j == 1)
                    {
                        e.Row.Cells[last - 1].BackColor = System.Drawing.Color.Green;
                        e.Row.Cells[last - 1].Font.Size = FontUnit.Large;
                        e.Row.Cells[last - 1].ForeColor = Color.White;
                        e.Row.Cells[last - 1].Font.Bold = true;
                        //++
                    }
                    count++;
                }
                else
                {
                    e.Row.Cells[last - 1].BackColor = System.Drawing.Color.Green;
                    e.Row.Cells[last - 1].Font.Size = FontUnit.Large;
                    e.Row.Cells[last - 1].ForeColor = Color.White;
                    e.Row.Cells[last - 1].Font.Bold = true;
                    count = 0;
                }
            }
            else if (presentvalue >= previousvalue)
            {
                e.Row.Cells[last - 1].BackColor = System.Drawing.Color.SkyBlue;
                e.Row.Cells[last - 1].Font.Size = FontUnit.Large;
                e.Row.Cells[last - 1].ForeColor = Color.White;
                e.Row.Cells[last - 1].Font.Bold = true;
            }
            else
            {
                previousvalue = presentvalue;
                e.Row.Cells[last - 1].BackColor = System.Drawing.Color.Red;
                e.Row.Cells[last - 1].Font.Size = FontUnit.Large;
                e.Row.Cells[last - 1].ForeColor = Color.White;
                e.Row.Cells[last - 1].Font.Bold = true;
                j = 1;
                count++;
            }
            if (e.Row.Cells[1].Text == "Avg Per Day")
            {
                e.Row.Cells[last - 1].BackColor = System.Drawing.Color.White;
                e.Row.Cells[last - 1].Font.Size = FontUnit.Large;
                e.Row.Cells[last - 1].ForeColor = Color.White;
                e.Row.Cells[last - 1].Font.Bold = true;
                // previousvalue = 0;
            }
            if (e.Row.Cells[1].Text == "Total")
            {
                e.Row.Cells[last - 1].BackColor = System.Drawing.Color.White;
                e.Row.Cells[last - 1].Font.Size = FontUnit.Large;
                e.Row.Cells[last - 1].ForeColor = Color.White;
                e.Row.Cells[last - 1].Font.Bold = true;
            }
            //if (e.Row.Cells[2].Text == "Total")
            //{
            //    e.Row.BackColor = System.Drawing.Color.CadetBlue;
            //    e.Row.Font.Size = FontUnit.Large;
            //    e.Row.ForeColor = Color.White;
            //    e.Row.Font.Bold = true;
            //}

        }

    }
}