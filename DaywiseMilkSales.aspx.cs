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
                FillAgentName();
                PCategory.Visible = true;
                PSubCategory.Visible = false;
                PRoute.Visible = true;
                PAgent.Visible = true;

                //if (chkCategory.Checked || chkSubCategory.Checked)
                //{
                //    PCategory.Visible = true;
                //    PSubCategory.Visible = false;
                //}
                //else
                //{
                //    PCategory.Visible = false;
                //    PSubCategory.Visible = false;
                //}

            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
            GetReport();
    }
    //protected void chkChangedCtegory(Object sender,EventArgs e)
    //{
    //    if (chkCategory.Checked)
    //    {

    //    }
    //    else if(chkSubCategory.Checked)
    //    {
    //        PCategory.Visible = true;
    //        PSubCategory.Visible = true;
    //    }
    //    else
    //    {
    //        PCategory.Visible = false;
    //        PSubCategory.Visible = false;
    //    }
    //}


    void FillAgentName()
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) and (branchdata.flag<>0) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) and (branchdata.flag<>0) ");
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
                cmd = new MySqlCommand("SELECT BranchName, sno FROM  branchdata WHERE (sno = @BranchID) and (flag<>0)");
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
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));

            }
            else
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, branchdata.SalesType FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) and (branchdata.flag<>0) OR (branchdata.sno = @BranchID) AND (branchdata.SalesType IS NOT NULL) and (branchdata.flag<>0)");
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlDispName.DataSource = dtRoutedata;
                ddlDispName.DataTextField = "BranchName";
                ddlDispName.DataValueField = "sno";
                ddlDispName.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch)");
        cmd = new MySqlCommand("SELECT RouteName, Sno,BranchID FROM branchroutes WHERE (BranchID = @BranchID) and  (flag=@flag)");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@flag", "1");
        DataTable dtBranch = vdm.SelectQuery(cmd).Tables[0];
        
        ddlDispName.DataSource = dtBranch;
        ddlDispName.DataTextField = "RouteName";
        ddlDispName.DataValueField = "Sno";
        ddlDispName.DataBind();
        ddlDispName.Items.Insert(0, new ListItem("Select", "0"));
    }
    protected void ddlDispName_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch)");
        cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName,branchroutes.Sno FROM branchroutesubtable INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (branchroutes.Sno = @RouteID) AND branchdata.flag=@flag");
        cmd.Parameters.AddWithValue("@RouteID", ddlDispName.SelectedValue);
        cmd.Parameters.AddWithValue("@flag", "1");
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlAgentName.DataSource = dtRoutedata;
        ddlAgentName.DataTextField = "BranchName";
        ddlAgentName.DataValueField = "sno";
        ddlAgentName.DataBind();
    }

    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCategoryName();
        //ddlSubCategoryName.Items.Insert(0, new ListItem("ALL", "ALL"));
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlcatType.SelectedValue == "Category")
        //{
        //    PCategory.Visible = true;
        //    PSubCategory.Visible = false;
        //    PBranch.Visible = true;
        //    PRoute.Visible = true;
        //    PAgent.Visible = true;
        //}
        //else if (ddlcatType.SelectedValue == "SubCategory")
        //{
        //    PCategory.Visible = true;
        //    PSubCategory.Visible = true;
        //    PBranch.Visible = true;
        //    PRoute.Visible = true;
        //    PAgent.Visible = true;
        //}
        //else if (ddlType.SelectedValue == "SalesOffice")
        //{

        //    PCategory.Visible = true;
        //    PSubCategory.Visible = true;
        //    PBranch.Visible = true;
        //    PRoute.Visible = false;
        //    PAgent.Visible = false;
        //}
        //else if (ddlType.SelectedValue == "RouteWise")
        //{
        //    PAgent.Visible = false;

        //    PCategory.Visible = true;
        //    PSubCategory.Visible = true;
        //    PBranch.Visible = true;
        //    PRoute.Visible = true;

        //}
        //else if (ddlType.SelectedValue == "AgentWise")
        //{
        //    PCategory.Visible = true;
        //    PSubCategory.Visible = true;
        //    PBranch.Visible = true;
        //    PRoute.Visible = true;
        //    PAgent.Visible = true;
        //}
        if (ddlType.SelectedValue == "SalesOffice" && ddlcatType.SelectedValue == "Category")
        {
            PCategory.Visible = true;
            PSubCategory.Visible = false;
            PBranch.Visible = true;
            PRoute.Visible = false;
            PAgent.Visible = false;
        }
        else if (ddlType.SelectedValue == "RouteWise" && ddlcatType.SelectedValue == "Category")
        {
            PCategory.Visible = true;
            PSubCategory.Visible = false;
            PBranch.Visible = true;
            PRoute.Visible = true;
            PAgent.Visible = false;
        }
        else if (ddlType.SelectedValue == "AgentWise" && ddlcatType.SelectedValue == "Category")
        {
            PCategory.Visible = true;
            PSubCategory.Visible = false;
            PBranch.Visible = true;
            PRoute.Visible = true;
            PAgent.Visible = true;
        }
        else if (ddlType.SelectedValue == "SalesOffice" && ddlcatType.SelectedValue == "SubCategory")
        {
            PCategory.Visible = true;
            PSubCategory.Visible = true;
            PBranch.Visible = true;
            PRoute.Visible = false;
            PAgent.Visible = false;
        }
        else if (ddlType.SelectedValue == "RouteWise" && ddlcatType.SelectedValue == "SubCategory")
        {
            PCategory.Visible = true;
            PSubCategory.Visible = true;
            PBranch.Visible = true;
            PRoute.Visible = true;
            PAgent.Visible = false;
        }
        else if (ddlType.SelectedValue == "AgentWise" && ddlcatType.SelectedValue == "SubCategory")
        {
            PCategory.Visible = true;
            PSubCategory.Visible = true;
            PBranch.Visible = true;
            PRoute.Visible = true;
            PAgent.Visible = true;
        }
    }
    protected void ddlcatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlType.SelectedValue == "SalesOffice" && ddlcatType.SelectedValue == "Category")
        {
            PCategory.Visible = true;
            PSubCategory.Visible = false;
            PBranch.Visible = true;
            PRoute.Visible = false;
            PAgent.Visible = false;
        }
        else if (ddlType.SelectedValue == "RouteWise" && ddlcatType.SelectedValue == "Category")
        {
            PCategory.Visible = true;
            PSubCategory.Visible = false;
            PBranch.Visible = true;
            PRoute.Visible = true;
            PAgent.Visible = false;
        }
        else if (ddlType.SelectedValue == "AgentWise" && ddlcatType.SelectedValue == "Category")
        {
            PCategory.Visible = true;
            PSubCategory.Visible = false;
            PBranch.Visible = true;
            PRoute.Visible = true;
            PAgent.Visible = true;
        }
        else if (ddlType.SelectedValue == "SalesOffice" && ddlcatType.SelectedValue == "SubCategory")
        {
            PCategory.Visible = true;
            PSubCategory.Visible = true;
            PBranch.Visible = true;
            PRoute.Visible = false;
            PAgent.Visible = false;
        }
        else if (ddlType.SelectedValue == "RouteWise" && ddlcatType.SelectedValue == "SubCategory")
        {
            PCategory.Visible = true;
            PSubCategory.Visible = true;
            PBranch.Visible = true;
            PRoute.Visible = true;
            PAgent.Visible = false;
        }
        else if (ddlType.SelectedValue == "AgentWise" && ddlcatType.SelectedValue == "SubCategory")
        {
            PCategory.Visible = true;
            PSubCategory.Visible = true;
            PBranch.Visible = true;
            PRoute.Visible = true;
            PAgent.Visible = true;
        }

    }
    protected void LoadCategoryName()
    {
        vdm = new VehicleDBMgr();
        string type = ddlReportType.SelectedValue;
        cmd = new MySqlCommand("SELECT    sno, category_sno, SubCatName, Flag, userdata_sno, fat, description, rank, tempcatsno, sno FROM  products_subcategory WHERE (Flag = @flag) AND (category_sno = @CatSno) ORDER BY tempsub_catsno");
        cmd.Parameters.AddWithValue("@CatSno", ddlReportType.SelectedValue);
        cmd.Parameters.AddWithValue("@flag", "1");
        DataTable dtCategory = vdm.SelectQuery(cmd).Tables[0];
        ddlSubCategoryName.DataSource = dtCategory;
        ddlSubCategoryName.DataTextField = "SubCatName";
        ddlSubCategoryName.DataValueField = "sno";
        ddlSubCategoryName.DataBind();

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
            Button3.Visible = true;
            lblmsg.Text = "";
            pnlHide.Visible = true;
            exportPanel.Visible = true;
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

            if (ddlType.SelectedValue == "SalesOffice" && ddlcatType.SelectedValue == "Category")
            {
                cmd = new MySqlCommand("SELECT  products_subcategory.rank,DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, products_subcategory.category_sno, productsdata.SubCat_sno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,ROUND(SUM(indents_subtable.pkt_qty), 2) AS PktQty, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS SaleValue, ROUND(SUM(indents_subtable.pkt_rate * indents_subtable.pkt_qty), 2) AS PktValue,products_category.Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchmappingtable_1.SuperBranch = @BranchID)  AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty > 0) AND (products_category.sno = @catsno) GROUP BY IndentDate,productsdata.ProductName ORDER BY indents.I_date,products_category.sno");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@catsno", ddlReportType.SelectedValue);
                dtsubcategory = vdm.SelectQuery(cmd).Tables[0];
            }
            else if (ddlType.SelectedValue == "SalesOffice" && ddlcatType.SelectedValue == "SubCategory")
            {
                cmd = new MySqlCommand("SELECT  products_subcategory.rank,DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, products_subcategory.category_sno, productsdata.SubCat_sno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,ROUND(SUM(indents_subtable.pkt_qty), 2) AS PktQty, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS SaleValue,ROUND(SUM(indents_subtable.pkt_rate * indents_subtable.pkt_qty), 2) AS PktValue, products_category.Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchmappingtable_1.SuperBranch = @BranchID)  AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty > 0) AND (products_subcategory.sno = @Subcatsno) GROUP BY IndentDate,productsdata.ProductName ORDER BY indents.I_date,products_category.sno");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@Subcatsno", ddlSubCategoryName.SelectedValue);
                dtsubcategory = vdm.SelectQuery(cmd).Tables[0];
            }
            //else if (ddlType.SelectedValue == "RouteWise" && ddlcatType.SelectedValue == "Category")
            //{
            //    cmd = new MySqlCommand("SELECT dispath.DispName, ROUND(SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost), 2) AS salevalue, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.UnitCost, DATE_FORMAT(tripdat.I_Date, '%d %b %y') AS IndentDate, productsdata.ProductName,products_category.Categoryname FROM (SELECT sno, DispName FROM dispatch WHERE (sno = @dispatchsno)) dispath INNER JOIN triproutes ON dispath.sno = triproutes.RouteID INNER JOIN (SELECT Sno, I_Date, Status FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN indents_subtable ON tripdat.Sno = indents_subtable.DTripId INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno where products_category.sno=@catsno GROUP BY tripdat.Sno, productsdata.ProductName, products_category.Categoryname ORDER BY tripdat.I_Date");
            //    cmd.Parameters.AddWithValue("@dispatchsno", ddlDispName.SelectedValue);
            //    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            //    cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
            //    cmd.Parameters.AddWithValue("@catsno", ddlReportType.SelectedValue);
            //    dtsubcategory = vdm.SelectQuery(cmd).Tables[0];

            //}
            //else if (ddlType.SelectedValue == "RouteWise" && ddlcatType.SelectedValue == "SubCategory")
            //{
            //    cmd = new MySqlCommand("SELECT dispath.DispName, ROUND(SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost), 2) AS salevalue, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.UnitCost, DATE_FORMAT(tripdat.I_Date, '%d %b %y') AS IndentDate, productsdata.ProductName,products_category.Categoryname FROM (SELECT sno, DispName FROM dispatch WHERE (sno = @dispatchsno)) dispath INNER JOIN triproutes ON dispath.sno = triproutes.RouteID INNER JOIN (SELECT Sno, I_Date, Status FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN indents_subtable ON tripdat.Sno = indents_subtable.DTripId INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno where products_subcategory.sno=@Subcatsno GROUP BY tripdat.Sno, productsdata.ProductName, products_category.Categoryname ORDER BY tripdat.I_Date");
            //    cmd.Parameters.AddWithValue("@dispatchsno", ddlDispName.SelectedValue);
            //    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            //    cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
            //    cmd.Parameters.AddWithValue("@Subcatsno", ddlSubCategoryName);
            //    dtsubcategory = vdm.SelectQuery(cmd).Tables[0];
            //}
            else if (ddlType.SelectedValue == "AgentWise" && ddlcatType.SelectedValue == "Category")
            {
                cmd = new MySqlCommand("SELECT  products_subcategory.rank,DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, products_subcategory.category_sno, productsdata.SubCat_sno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,ROUND(SUM(indents_subtable.pkt_qty), 2) AS PktQty, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS SaleValue,ROUND(SUM(indents_subtable.pkt_rate * indents_subtable.pkt_qty), 2) AS PktValue, products_category.Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchmappingtable_1.SubBranch = @BranchID)  AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty > 0) AND (products_category.sno = @catsno) GROUP BY IndentDate,productsdata.ProductName ORDER BY indents.I_date,products_category.sno");
                cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@catsno", ddlReportType.SelectedValue);
                dtsubcategory = vdm.SelectQuery(cmd).Tables[0];

            }
            else if (ddlType.SelectedValue == "AgentWise" && ddlcatType.SelectedValue == "SubCategory")
            {
                cmd = new MySqlCommand("SELECT  products_subcategory.rank,DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, products_subcategory.category_sno, productsdata.SubCat_sno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, ROUND(SUM(indents_subtable.pkt_qty), 2) AS PktQty,ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS SaleValue,ROUND(SUM(indents_subtable.pkt_rate * indents_subtable.pkt_qty), 2) AS PktValue, products_category.Categoryname FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchmappingtable_1.SubBranch = @BranchID)  AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty > 0)  AND (products_subcategory.sno = @Subcatsno)  GROUP BY IndentDate,productsdata.ProductName ORDER BY indents.I_date,products_category.sno");
                cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@SubCatsno", ddlSubCategoryName.SelectedValue);
                dtsubcategory = vdm.SelectQuery(cmd).Tables[0];
            }






            //cmd = new MySqlCommand("SELECT SUM(tripsubdata.Qty) AS dispqty, products_category.Categoryname, products_subcategory.SubCatName, result.IndentDate FROM (SELECT dispatch.sno, dispatch.DispName, tripdat.Sno AS tripid, DATE_FORMAT(tripdat.I_Date, '%d %b %y') AS IndentDate FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, Status, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno WHERE (dispatch.Branch_Id = @branch) AND (dispatch.DispMode IS NOT NULL) AND (dispatch.DispMode <> 'SPL')) result INNER JOIN tripsubdata ON result.tripid = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (products_category.sno = 9) GROUP BY products_subcategory.sno, result.IndentDate");
            //cmd.Parameters.AddWithValue("@branch", Session["branch"]);
            //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            //cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
            //dtdirect = vdm.SelectQuery(cmd).Tables[0];
            DataView dv = dtsubcategory.DefaultView;
            //dv.Sort = "IndentDate";
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
                    //string pLtr = dr["ProductName"].ToString() + "ltr";
                    //string PPakt = dr["ProductName"].ToString() + "pkt";
                    //Report.Columns.Add(pLtr).DataType = typeof(Double);
                    //Report.Columns.Add(PPakt).DataType = typeof(Double);
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                }
                Report.Columns.Add("Total");
                //Report.Columns.Add("Total(packets)");
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
                    double totmilk = 0; double totpktqty = 0;
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
                            if (chkPktOrLtr.Checked)
                            {
                                double delpqty = 0;
                                double.TryParse(dr["PktQty"].ToString(), out delpqty);
                                newrow[dr["ProductName"].ToString()] = Math.Round(delpqty, 2);
                                totpktqty += delpqty;
                            }
                            else
                            {
                                double delqty = 0;
                                double.TryParse(dr["DeliveryQty"].ToString(), out delqty);
                                newrow[dr["ProductName"].ToString()] = Math.Round(delqty + directdel, 2);
                                totmilk += delqty + directdel;
                            }
                        }
                    }
                    if (chkPktOrLtr.Checked)
                    {
                        newrow["Total"] = Math.Round(totpktqty, 2);
                    }
                    else
                    {
                        newrow["Total"] = Math.Round(totmilk, 2);
                    }
                    Report.Rows.Add(newrow);
                    i++;
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




            grdtotal_dcReports.DataSource = Report;
            grdtotal_dcReports.DataBind();




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
    //protected void grdtotal_dcReports_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    //e.Row.Cells[1].Visible = false;
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        double presentvalue = 0;
    //        if (count == 0)
    //        {
    //            previousvalue = 0;
    //        }
    //        int last = e.Row.Cells.Count;

    //        double.TryParse(e.Row.Cells[last-1].Text, out presentvalue);
    //        if (presentvalue > previousvalue)
    //        {
    //            if (previousvalue == 0)
    //            {
    //                previousvalue = presentvalue;
    //                //e.Row.Cells[2].BackColor = System.Drawing.Color.Green;
    //                //e.Row.Cells[2].Font.Size = FontUnit.Large;
    //                //e.Row.Cells[2].ForeColor = Color.White;
    //                //e.Row.Cells[2].Font.Bold = true;
    //                if (j == 1)
    //                {
    //                    e.Row.Cells[last - 1].BackColor = System.Drawing.Color.Green;
    //                    e.Row.Cells[last - 1].Font.Size = FontUnit.Large;
    //                    e.Row.Cells[last - 1].ForeColor = Color.White;
    //                    e.Row.Cells[last - 1].Font.Bold = true;
    //                    //++
    //                }
    //                count++;
    //            }
    //            else
    //            {
    //                e.Row.Cells[last - 1].BackColor = System.Drawing.Color.Green;
    //                e.Row.Cells[last - 1].Font.Size = FontUnit.Large;
    //                e.Row.Cells[last - 1].ForeColor = Color.White;
    //                e.Row.Cells[last - 1].Font.Bold = true;
    //                count = 0;
    //            }
    //        }
    //        else if (presentvalue >= previousvalue)
    //        {
    //            e.Row.Cells[last - 1].BackColor = System.Drawing.Color.SkyBlue;
    //            e.Row.Cells[last - 1].Font.Size = FontUnit.Large;
    //            e.Row.Cells[last - 1].ForeColor = Color.White;
    //            e.Row.Cells[last - 1].Font.Bold = true;
    //        }
    //        else
    //        {
    //            previousvalue = presentvalue;
    //            e.Row.Cells[last - 1].BackColor = System.Drawing.Color.Red;
    //            e.Row.Cells[last - 1].Font.Size = FontUnit.Large;
    //            e.Row.Cells[last - 1].ForeColor = Color.White;
    //            e.Row.Cells[last - 1].Font.Bold = true;
    //            j = 1;
    //            count++;
    //        }
    //        if (e.Row.Cells[1].Text == "Avg Per Day")
    //        {
    //            e.Row.Cells[last - 1].BackColor = System.Drawing.Color.White;
    //            e.Row.Cells[last - 1].Font.Size = FontUnit.Large;
    //            e.Row.Cells[last - 1].ForeColor = Color.White;
    //            e.Row.Cells[last - 1].Font.Bold = true;
    //            // previousvalue = 0;
    //        }
    //        if (e.Row.Cells[1].Text == "Total")
    //        {
    //            e.Row.Cells[last - 1].BackColor = System.Drawing.Color.White;
    //            e.Row.Cells[last - 1].Font.Size = FontUnit.Large;
    //            e.Row.Cells[last - 1].ForeColor = Color.White;
    //            e.Row.Cells[last - 1].Font.Bold = true;
    //        }
    //        //if (e.Row.Cells[2].Text == "Total")
    //        //{
    //        //    e.Row.BackColor = System.Drawing.Color.CadetBlue;
    //        //    e.Row.Font.Size = FontUnit.Large;
    //        //    e.Row.ForeColor = Color.White;
    //        //    e.Row.Font.Bold = true;
    //        //}

    //    }

    //}
}