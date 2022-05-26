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
//using ExtensionMethods;

public partial class Delivery_Collection_Report : System.Web.UI.Page
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
    protected void btnSMS_Click(object sender, EventArgs e)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string MobNo = txtMobNo.Text;
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            lblmsg.Text = "";
            if (MobNo.Length == 10)
            {

                DataTable Report = new DataTable();
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
                Session["filename"] = "NET SALE REPORT " + ddlSalesOffice.SelectedItem.Value;
                cmd = new MySqlCommand("SELECT SUM(indents_subtable.DeliveryQty) AS DeliverQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS totalsaleamt, productsdata.ProductName, productsdata.sno FROM branchmappingtable INNER JOIN indents ON branchmappingtable.SubBranch = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @branch) GROUP BY productsdata.sno");
                cmd.Parameters.AddWithValue("@branch", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtTotalsale = vdm.SelectQuery(cmd).Tables[0];
                double TotalQty = 0;
                double Totalsalevalue = 0;
                string ProductName = "";
                if (dtTotalsale.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTotalsale.Rows)
                    {
                        double DeliverQty = 0;
                        double salevalue = 0;
                        double.TryParse(dr["DeliverQty"].ToString(), out DeliverQty);
                        double.TryParse(dr["totalsaleamt"].ToString(), out salevalue);
                        ProductName += dr["ProductName"].ToString() + "=" + Math.Round(DeliverQty, 2) + ";";
                        TotalQty += Math.Round(DeliverQty, 2);
                        Totalsalevalue += Math.Round(salevalue, 2);
                    }
                }
                string Date = DateTime.Now.ToString("dd/MM/yyyy");
                WebClient client = new WebClient();
                string SalesOfficeName = ddlSalesOffice.SelectedItem.Text;
                if (Session["TitleName"].ToString() == "Sri Vyshnavi Dairy Specialities (P) Ltd")
                {
                    //string strUrl = " http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + no + "&msg=" + message1 + "&type=1 ";
                    string baseurl = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + MobNo + "&message=%20" + SalesOfficeName + "%20,%20 + NET SALES FOR TODAY" + "%20:%20" + fromdate + "%20%20" + ProductName + "TotalQty =" + TotalQty + "%20,%20" + "Sale Value =" + Totalsalevalue + "&sender=VYSNVI&type=1&route=2"; 
                    //string baseurl = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VSALES&to=" + MobNo + "&msg=%20" + SalesOfficeName + "%20,%20 + NET SALES FOR TODAY" + "%20:%20" + fromdate + "%20%20" + ProductName + "TotalQty =" + TotalQty + "%20,%20" + "Sale Value =" + Totalsalevalue + "&type=1";
                    Stream data = client.OpenRead(baseurl);
                    StreamReader reader = new StreamReader(data);
                    string ResponseID = reader.ReadToEnd();
                    data.Close();
                    reader.Close();
                }
                else
                {
                    string baseurl = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + MobNo + "&message=%20" + SalesOfficeName + "%20,%20 + NET SALES FOR TODAY" + "%20:%20" + fromdate + "%20%20" + ProductName + "TotalQty =" + TotalQty + "%20,%20" + "Sale Value =" + Totalsalevalue + "&sender=VYSNVI&type=1&route=2"; 

                    //string baseurl = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VFWYRA&to=" + MobNo + "&msg=%20" + SalesOfficeName + "%20,%20 + NET SALES FOR TODAY" + "%20:%20" + fromdate + "%20%20" + ProductName + "TotalQty =" + TotalQty + "%20,%20" + "Sale Value =" + Totalsalevalue + "&type=1";
                    Stream data = client.OpenRead(baseurl);
                    StreamReader reader = new StreamReader(data);
                    string ResponseID = reader.ReadToEnd();
                    data.Close();
                    reader.Close();
                }
                string message = "  " + SalesOfficeName + "   NET SALES FOR TODAY" + " " + fromdate + " " + ProductName + "TotalQty =" + TotalQty + ", " + " Sale Value =" + Totalsalevalue + " ";
                // string text = message.Replace("\n", "\n" + System.Environment.NewLine);
                cmd = new MySqlCommand("insert into smsinfo (agentid,branchid,mainbranch,msg,mobileno,msgtype,branchname,doe) values (@agentid,@branchid,@mainbranch,@msg,@mobileno,@msgtype,@branchname,@doe)");
                cmd.Parameters.AddWithValue("@agentid", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@branchid", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@mainbranch", Session["SuperBranch"].ToString());
                cmd.Parameters.AddWithValue("@msg", message);
                cmd.Parameters.AddWithValue("@mobileno", MobNo);
                cmd.Parameters.AddWithValue("@msgtype", "TripEdnd");
                cmd.Parameters.AddWithValue("@branchname", SalesOfficeName);
                cmd.Parameters.AddWithValue("@doe", ServerDateCurrentdate);
                vdm.insert(cmd);

                lblmsg.Text = "Message Sent Successfully";
                txtMobNo.Text = "";
            }
            else
            {
                lblmsg.Text = "Please Enter 10 digit Number";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;

        }
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        pnlHide.Visible = true;
        pnlExcel.Visible = true;
        pnlSms.Visible = true;
        DateTime fromdate = DateTime.Now;
        string[] datestrig = txtdate.Text.Split(' ');
        lblpreparedby.Text = "";
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
            if (salesoff == "8097" ||   salesoff == "174" || salesoff == "1801" || salesoff == "527" || salesoff == "306" || salesoff == "538" || salesoff == "2909" || salesoff == "2749" || salesoff == "3781" || salesoff == "3928" || salesoff == "4607")
            {
                GetReport();
            }
            else
            {
                if (salesoff == "159")
                {
                    //GetReport();
                    getHYDReport();
                }
                if (salesoff == "2" || salesoff == "4" || salesoff == "271" || salesoff == "285" || salesoff == "2948" || salesoff == "4607" || salesoff == "457" || salesoff == "570" || salesoff == "572" || salesoff == "3559" || salesoff == "282" || salesoff == "458" || salesoff == "4609")
                {
                    status = "Nellore";

                    getnelloreReport();
                }
            }
            cmd = new MySqlCommand("SELECT clotrans.Sno, clotrans.BranchId, clotrans.EmpId, clotrans.IndDate, empmanage.EmpName FROM clotrans INNER JOIN empmanage ON clotrans.EmpId = empmanage.Sno WHERE (clotrans.BranchId = @branch) AND (clotrans.IndDate BETWEEN @d1 AND @d2)");
            cmd.Parameters.Add("@branch", salesoff);
            cmd.Parameters.Add("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.Add("@d2", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtemp = vdm.SelectQuery(cmd).Tables[0];
            if (dtemp.Rows.Count > 0)
            {
                lblpreparedby.Text = dtemp.Rows[0]["EmpName"].ToString();
            }
        }
        else
        {
            var salesoff = Session["branch"].ToString();
            if (salesoff == "8097" ||  salesoff == "174" || salesoff == "527" || salesoff == "306" || salesoff == "4607" || salesoff == "538" || salesoff == "2909" || salesoff == "2749" || salesoff == "3781" || salesoff == "3928")
            {
                GetReport();
            }
            else
            {
                if (salesoff == "159")
                {
                    getHYDReport();
                }
                if (salesoff == "2" || salesoff == "4" || salesoff == "271" || salesoff == "285" || salesoff == "2948" || salesoff == "457" || salesoff == "570" || salesoff == "572" || salesoff == "3559" || salesoff == "282" || salesoff == "458" || salesoff == "4609")
                {
                    status = "Nellore";
                    getnelloreReport();
                }
            }//new
            cmd = new MySqlCommand("SELECT clotrans.Sno, clotrans.BranchId, clotrans.EmpId, clotrans.IndDate, empmanage.EmpName FROM clotrans INNER JOIN empmanage ON clotrans.EmpId = empmanage.Sno WHERE (clotrans.BranchId = @branch) AND (clotrans.IndDate BETWEEN @d1 AND @d2)");
            cmd.Parameters.Add("@branch", salesoff);
            cmd.Parameters.Add("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.Add("@d2", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtemp = vdm.SelectQuery(cmd).Tables[0];
            if (dtemp.Rows.Count > 0)
            {
                lblpreparedby.Text = dtemp.Rows[0]["EmpName"].ToString();
            }
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
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            Report = new DataTable();
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Plant")
            {
                lblDispatchName.Text = ddlSalesOffice.SelectedItem.Text;
                lblDate.Text = txtdate.Text;
            }
            else
            {
                lblDispatchName.Text = ddlSalesOffice.SelectedItem.Text;
                lblDate.Text = txtdate.Text;
            }
            Session["RouteName"] = "NET SALES FOR " + ddlSalesOffice.SelectedItem.Text;
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
            Session["filename"] = ddlSalesOffice.SelectedItem.Text + " REPORT " + fromdate.AddDays(1).ToString("dd/MM/yyyy");
            cmd = new MySqlCommand("SELECT modifiedroutesubtable.BranchID,modifiedroutes.Sno, modifiedroutes.RouteName, indents_subtable.Product_sno, productsdata.ProductName,productsdata.SubCat_sno, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS Total, indents_subtable.UnitCost, ROUND(SUM(indents_subtable.LeakQty), 2) AS ILeakQty, products_category.Categoryname, ROUND(SUM(indents_subtable.LeakQty), 2) AS LeakQty,indents_subtable.DTripId, tripdata.RecieptNo FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN  products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN tripdata ON indents_subtable.DTripId = tripdata.Sno WHERE (modifiedroutes.BranchID = @BranchID) AND (modifiedroutesubtable.EDate IS NULL) AND (branchdata.CollectionType <> 'DUE') AND (modifiedroutesubtable.CDate <= @starttime) OR (modifiedroutes.BranchID = @BranchID) AND (branchdata.CollectionType <> 'DUE') AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime)  GROUP BY modifiedroutes.Sno, productsdata.sno");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@startime", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT branchroutes.Sno, branchroutes.RouteName, tripdata.Sno AS Expr1, tripdata.ReceivedAmount, tripdata.CollectedAmount, tripdata.ReceivedAmount as SubmittedAmount FROM branchroutes INNER JOIN dispatch ON branchroutes.Sno = dispatch.Route_id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.Branch_Id = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2) GROUP BY dispatch.sno, tripdata.Sno");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtreceiptamount = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT branchproducts.Rank,productsdata.ProductName,productsdata.sno, products_category.Categoryname, productsdata.Units, productsdata.Qty,branchproducts.unitprice FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) ORDER BY branchproducts.Rank");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            DataTable dttable = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT    branchproducts.unitprice,branchproducts.branch_sno,products_subcategory.tempsub_catsno AS SubCatSno, products_category.description AS Categoryname, branchproducts.product_sno AS sno, productsdata.ProductName,productsdata.SubCat_sno, branchproducts.Rank,products_subcategory.description AS SubCategoryName FROM  products_category INNER JOIN products_subcategory ON products_category.sno = products_subcategory.category_sno INNER JOIN productsdata ON products_subcategory.tempsub_catsno = productsdata.SubCat_sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno WHERE (branchproducts.branch_sno = @BranchID) ORDER BY products_subcategory.tempsub_catsno, branchproducts.Rank");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            produtstbl1 = vdm.SelectQuery(cmd).Tables[0];


            cmd = new MySqlCommand("SELECT SUM(tripinvdata.Qty) AS issued, SUM(tripinvdata.Remaining) AS returnqty, invmaster.InvName, invmaster.sno, dispatch.sno AS dispatchsno,dispatch.DispName,dispatch.Route_id FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripinvdata ON tripdat.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno WHERE (dispatch.Branch_Id = @brnchid) AND (dispatch.DispType IS NULL) OR (branchdata.SalesOfficeID = @brnchid) AND (dispatch.DispType IS NULL) GROUP BY dispatch.sno, invmaster.sno");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@brnchid", ddlSalesOffice.SelectedValue);
            DataTable dtInventory = vdm.SelectQuery(cmd).Tables[0];
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
            produtstbl.Columns.Add("Rank").DataType = typeof(int);
            produtstbl.Columns.Add("SubCat_sno").DataType = typeof(int);




            foreach (DataRow dr in produtstbl1.Rows)
            {
                DataRow newRow = produtstbl.NewRow();
                newRow["Categoryname"] = dr["Categoryname"].ToString();
                newRow["sno"] = dr["sno"].ToString();
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

            DataView dv = produtstbl1.DefaultView;
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
            dtalldelivery.Columns.Add("scheme");
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
            cmd = new MySqlCommand("SELECT productsdata.SubCat_sno,productsdata.sno AS prodsno, productsdata.ProductName, indents_subtable.DeliveryQty, indents_subtable.UnitCost, branchdata.BranchName, indents_subtable.DTripId, indent.Branch_id FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON branchdata.sno = modifiedroutesubtable.BranchID INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (modifiedroutes.BranchID = @BranchID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) AND (branchdata.CollectionType = @CollectionType) OR (modifiedroutes.BranchID = @BranchID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (branchdata.CollectionType = @CollectionType) GROUP BY branchdata.sno, productsdata.sno");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@CollectionType", "DUE");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@startime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtDue = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName, ROUND(SUM(offer_indents_sub.offer_indent_qty), 2) AS unitQty,offer_indents_sub.unit_price, productsdata.ProductName, productsdata.Units, productsdata.sno AS productid, ROUND(SUM(offer_indents_sub.offer_delivered_qty), 2) AS Delqty, products_category.Categoryname FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutes ON dispatch_sub.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT idoffer_indents, idoffers_assignment, salesoffice_id, route_id, agent_id, indent_date, indents_id, IndentType, I_modified_by FROM offer_indents WHERE (indent_date BETWEEN @starttime AND @endtime)) offerindents ON modifiedroutesubtable.BranchID = offerindents.agent_id INNER JOIN offer_indents_sub ON offerindents.idoffer_indents = offer_indents_sub.idoffer_indents INNER JOIN productsdata ON offer_indents_sub.product_id = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.Branch_Id = @BranchID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (dispatch.Branch_Id = @BranchID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY productsdata.sno");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            DataTable dt_offertble = vdm.SelectQuery(cmd).Tables[0];

            if (sortedProductDT.Rows.Count > 0)
            {
                DataView view = new DataView(dtble);
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("Route Name");
                foreach (DataRow dr in sortedProductDT.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                }
                Report.Columns.Add("Total Qty").DataType = typeof(Double);
                Report.Columns.Add("Total Amount").DataType = typeof(Double);
                Report.Columns.Add("Dues Amount").DataType = typeof(Double);
                Report.Columns.Add("Cash Amount").DataType = typeof(Double);
                Report.Columns.Add("Receipt No").DataType = typeof(Double);
                Report.Columns.Add("Crates Iss.").DataType = typeof(Double);
                Report.Columns.Add("Crates Rec.").DataType = typeof(Double);
                Report.Columns.Add("Cans Iss.").DataType = typeof(Double);
                Report.Columns.Add("Cans Rec.").DataType = typeof(Double);
                DataTable distincttable = view.ToTable(true, "Sno", "RouteName");

                dttempproducts = new DataTable();
                dttempproducts.Columns.Add("ProductName");
                dttempproducts.Columns.Add("SubCatSno").DataType = typeof(int); ;

                double grnd_tot_qty = 0;
                double grnd_tot_cashamount = 0;
                double grnd_tot_dueamount = 0;
                double grnd_tot_amount = 0;
                int i = 1;
                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    newrow["Route Name"] = branch["RouteName"].ToString();
                    double total = 0;
                    double Amount = 0;
                    float submittedAmount = 0;
                    float receivedAmount = 0;
                    double recieptno = 0;
                    foreach (DataRow dr in dtble.Rows)
                    {
                        if (branch["RouteName"].ToString() == dr["RouteName"].ToString())
                        {
                            double qtyvalue = 0;
                            double delqty = 0;
                            double.TryParse(dr["DeliveryQty"].ToString(), out delqty);

                            if (delqty == 0.0)
                            {
                            }
                            else
                            {
                                newrow[dr["ProductName"].ToString()] = Math.Round(delqty, 2);
                                int m = 0;
                                DataRow tempnewrow = dttempproducts.NewRow();
                                tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                tempnewrow["SubCatSno"] = dr["SubCat_sno"].ToString();
                                dttempproducts.Rows.Add(tempnewrow);
                                m++;
                            }
                            double.TryParse(dr["DeliveryQty"].ToString(), out qtyvalue);
                            double UnitCost = 0;
                            double.TryParse(dr["UnitCost"].ToString(), out UnitCost);
                            double DAmount = 0;
                            double.TryParse(dr["Total"].ToString(), out DAmount);
                            Amount += DAmount;
                            total += qtyvalue;
                            if (dr["RecieptNo"].ToString() == "")
                            {
                            }
                            else
                            {
                                double.TryParse(dr["RecieptNo"].ToString(), out recieptno);
                            }
                        }
                    }
                    foreach (DataRow drdtclubtotal in dtreceiptamount.Select("RouteName='" + branch["RouteName"].ToString() + "'"))
                    {
                        float.TryParse(drdtclubtotal["SubmittedAmount"].ToString(), out submittedAmount);
                        float.TryParse(drdtclubtotal["ReceivedAmount"].ToString(), out receivedAmount);
                    }
                    newrow["Total Qty"] = total;
                    grnd_tot_qty += total;
                    newrow["Total Amount"] = Amount;
                    grnd_tot_amount += Amount;
                    newrow["Cash Amount"] = submittedAmount;
                    grnd_tot_cashamount += submittedAmount;
                    newrow["Receipt No"] = recieptno;
                    int cratesissued = 0;
                    int cratesreceived = 0;
                    int cansissued = 0;
                    int cansreceived = 0;
                    foreach (DataRow drdtinv in dtInventory.Select("Route_id='" + branch["Sno"].ToString() + "'"))
                    {
                        int cansi = 0;
                        int cansr = 0;
                        if (drdtinv["sno"].ToString() == "1")
                        {
                            int.TryParse(drdtinv["issued"].ToString(), out cratesissued);
                            int.TryParse(drdtinv["returnqty"].ToString(), out cratesreceived);
                        }
                        if (drdtinv["sno"].ToString() == "2")
                        {
                            int.TryParse(drdtinv["issued"].ToString(), out cansi);
                            int.TryParse(drdtinv["returnqty"].ToString(), out cansr);
                            cansissued += cansi;
                            cansreceived += cansr;
                        }
                        if (drdtinv["sno"].ToString() == "3")
                        {
                            int.TryParse(drdtinv["issued"].ToString(), out cansi);
                            int.TryParse(drdtinv["returnqty"].ToString(), out cansr);
                            cansissued += cansi;
                            cansreceived += cansr;
                        }
                        if (drdtinv["sno"].ToString() == "4")
                        {
                            int.TryParse(drdtinv["issued"].ToString(), out cansi);
                            int.TryParse(drdtinv["returnqty"].ToString(), out cansr);
                            cansissued += cansi;
                            cansreceived += cansr;
                        }
                        if (drdtinv["sno"].ToString() == "5")
                        {
                            int.TryParse(drdtinv["issued"].ToString(), out cansi);
                            int.TryParse(drdtinv["returnqty"].ToString(), out cansr);
                            cansissued += cansi;
                            cansreceived += cansr;
                        }
                    }
                    if (cratesissued == 0.0)
                    {
                    }
                    else
                    {
                        newrow["Crates Iss."] = cratesissued;
                    }
                    if (cratesreceived == 0.0)
                    {
                    }
                    else
                    {
                        newrow["Crates Rec."] = cratesreceived;
                    }
                    if (cansissued == 0.0)
                    {
                    }
                    else
                    {
                        newrow["Cans Iss."] = cansissued;
                    }
                    if (cansreceived == 0.0)
                    {
                    }
                    else
                    {
                        newrow["Cans Rec."] = cansreceived;
                    }
                    Report.Rows.Add(newrow);
                    i++;
                }
                foreach (DataRow branchroute in dtble.Rows)
                {
                    foreach (DataRow drtotdelivery in dtalldelivery.Rows)
                    {
                        if (branchroute["Product_sno"].ToString() == drtotdelivery["sno"].ToString())
                        {
                            float qty = 0;
                            float.TryParse(branchroute["DeliveryQty"].ToString(), out qty);
                            float qtycpy = 0;
                            float.TryParse(drtotdelivery["deliverQty"].ToString(), out qtycpy);
                            float totalqty = qty + qtycpy;
                            drtotdelivery["deliverQty"] = Math.Round(totalqty, 2);
                        }
                        else
                        {
                        }
                    }
                }
                DataView viewdue = new DataView(dtDue);
                DataTable distincttabledue = viewdue.ToTable(true, "BranchName");
                foreach (DataRow dr in distincttabledue.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    newrow["Route Name"] = dr["BranchName"].ToString();
                    double Amount = 0;
                    double total = 0;
                    double dueamt = 0;
                    double paidamt = 0;
                    foreach (DataRow branch in dtDue.Rows)
                    {
                        if (dr["BranchName"].ToString() == branch["BranchName"].ToString())
                        {
                            double DeliveryQty = 0;
                            double.TryParse(branch["DeliveryQty"].ToString(), out DeliveryQty);
                            if (DeliveryQty == 0.0)
                            {
                            }
                            else
                            {
                                newrow[branch["ProductName"].ToString()] = Math.Round(DeliveryQty, 2);
                                double qtyvalue = 0;
                                double.TryParse(branch["DeliveryQty"].ToString(), out qtyvalue);
                                double UnitCost = 0;
                                double.TryParse(branch["UnitCost"].ToString(), out UnitCost);
                                Amount += qtyvalue * UnitCost;
                                total += qtyvalue;

                                DataRow tempnewrow = dttempproducts.NewRow();
                                tempnewrow["ProductName"] = branch["ProductName"].ToString();
                                tempnewrow["SubCatSno"] = branch["SubCat_sno"].ToString();
                                dttempproducts.Rows.Add(tempnewrow);
                            }
                        }
                    }
                    newrow["Total Qty"] = total;
                    grnd_tot_qty += total;
                    newrow["Total Amount"] = Amount;
                    grnd_tot_amount += Amount;
                    dueamt = Amount - paidamt;
                    if (dueamt > 0)
                    {
                        newrow["Dues Amount"] = Amount - paidamt;
                        grnd_tot_dueamount += dueamt;
                    }
                    else
                    {
                        newrow["Dues Amount"] = 0;
                    }
                    Report.Rows.Add(newrow);
                    i++;
                }
                foreach (DataRow drdue in dtDue.Rows)
                {
                    foreach (DataRow drtotdeliverydue in dtalldelivery.Rows)
                    {
                        if (drdue["prodsno"].ToString() == drtotdeliverydue["sno"].ToString())
                        {
                            float qty = 0;
                            float.TryParse(drdue["DeliveryQty"].ToString(), out qty);
                            float qtycpy = 0;
                            float.TryParse(drtotdeliverydue["deliverQty"].ToString(), out qtycpy);
                            float totalqty = qty + qtycpy;
                            drtotdeliverydue["deliverQty"] = totalqty;
                        }
                        else
                        {
                        }
                    }
                }


                //dtSubCatgory.DefaultView.Sort = "SubCatSno desc";
                //dtSortedSubCategory = dtSubCatgory.DefaultView.ToTable();




                DataTable dtlOCAL = new DataTable();
                DataTable distinctLeaks = new DataTable();
                /// By Ravi New
                /// 
                if (ddlSalesOffice.SelectedValue == "1801")
                {
                }
                else
                {
                    cmd = new MySqlCommand("SELECT cc.ProductName, cc.ProdSno, cc.Qty, ff.DispName, ff.DispMode FROM (SELECT productsdata.sno AS ProdSno, productsdata.ProductName, tripdata.Sno, tripsubdata.Qty FROM            tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE        (tripdata.I_Date BETWEEN @d1 AND @d2)) cc INNER JOIN (SELECT I_Date, DispName, DispMode, Tripdata_sno FROM (SELECT tripdata_1.I_Date, dispatch.DispName, dispatch.DispMode, triproutes.Tripdata_sno FROM triproutes INNER JOIN tripdata tripdata_1 ON triproutes.Tripdata_sno = tripdata_1.Sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno WHERE        (tripdata_1.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) AND (dispatch.DispMode = 'FREE') OR (tripdata_1.I_Date BETWEEN @D1 AND @D2) AND (dispatch.Branch_Id = @BranchID) AND (dispatch.DispMode = 'LOCAL')) ff_1) ff ON  cc.Sno = ff.Tripdata_sno");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    dtlOCAL = vdm.SelectQuery(cmd).Tables[0];
                    DataView viewLeaks = new DataView(dtlOCAL);
                    distinctLeaks = viewLeaks.ToTable(true, "DispName", "DispMode");
                }
                foreach (DataRow drp in distinctLeaks.Rows)
                {
                    if (drp["DispMode"].ToString() == "LOCAL")
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["SNo"] = i++;
                        newrow["Route Name"] = drp["DispName"].ToString();
                        double total = 0;
                        foreach (DataRow dr in dtlOCAL.Rows)
                        {
                            if (drp["DispMode"].ToString() == "LOCAL")
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {

                                    double Qty = 0;
                                    double.TryParse(dr["Qty"].ToString(), out Qty);
                                    if (Qty == 0.0)
                                    {
                                    }
                                    else
                                    {
                                        DataRow tempnewrow = dttempproducts.NewRow();
                                        tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                        tempnewrow["SubCatSno"] = dr["SubCat_sno"].ToString();
                                        dttempproducts.Rows.Add(tempnewrow);
                                        newrow[dr["ProductName"].ToString()] = Qty;
                                        total += Qty;
                                    }
                                }
                            }
                        }
                        newrow["Total Qty"] = total;
                        Report.Rows.Add(newrow);
                    }

                }
                foreach (DataRow branchroute in dtlOCAL.Rows)
                {
                    foreach (DataRow drtotdelivery in dtalldelivery.Rows)
                    {
                        if (branchroute["ProdSno"].ToString() == drtotdelivery["sno"].ToString())
                        {
                            float qty = 0;
                            float.TryParse(branchroute["Qty"].ToString(), out qty);
                            float qtycpy = 0;
                            float.TryParse(drtotdelivery["deliverQty"].ToString(), out qtycpy);
                            float totalqty = qty + qtycpy;
                            drtotdelivery["deliverQty"] = Math.Round(totalqty, 2);
                        }
                        else
                        {
                        }
                    }
                }
                cmd = new MySqlCommand("SELECT clotrans.BranchId,productsdata.SubCat_sno,productsdata.ProductName, closubtranprodcts.StockQty, productsdata.sno FROM clotrans INNER JOIN closubtranprodcts ON clotrans.Sno = closubtranprodcts.RefNo INNER JOIN productsdata ON closubtranprodcts.ProductID = productsdata.sno WHERE (clotrans.BranchId = @BranchID) AND (clotrans.IndDate BETWEEN @d1 AND @d2) AND (clotrans.Transaction_Type = 0) GROUP BY productsdata.ProductName");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-2)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-2)));
                DataTable dtOpp = vdm.SelectQuery(cmd).Tables[0];
                DataView viewOpp = new DataView(dtOpp);
                DataTable distinctOpp = viewOpp.ToTable(true, "BranchId");
                double totalopp = 0;
                foreach (DataRow drp in distinctOpp.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i++;
                    newrow["Route Name"] = "Oppening Stock";
                    foreach (DataRow dr in dtOpp.Rows)
                    {
                        if (drp["BranchId"].ToString() == dr["BranchId"].ToString())
                        {

                            double StockQty = 0;
                            double.TryParse(dr["StockQty"].ToString(), out StockQty);
                            StockQty = Math.Round(StockQty, 2);
                            newrow[dr["ProductName"].ToString()] = StockQty;
                            totalopp += StockQty;
                            if (StockQty == 0.0)
                            {
                            }
                            else
                            {

                                DataRow tempnewrow = dttempproducts.NewRow();
                                tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                tempnewrow["SubCatSno"] = dr["SubCat_sno"].ToString();
                                dttempproducts.Rows.Add(tempnewrow);
                            }
                        }
                    }
                    newrow["Total Qty"] = totalopp;
                    Report.Rows.Add(newrow);
                }

                cmd = new MySqlCommand("SELECT clotrans.BranchId,productsdata.ProductName,productsdata.SubCat_sno, closubtranprodcts.StockQty, productsdata.sno FROM clotrans INNER JOIN closubtranprodcts ON clotrans.Sno = closubtranprodcts.RefNo INNER JOIN productsdata ON closubtranprodcts.ProductID = productsdata.sno WHERE (clotrans.BranchId = @BranchID) AND (clotrans.IndDate BETWEEN @d1 AND @d2) AND (clotrans.Transaction_Type = 0) GROUP BY productsdata.ProductName");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtClo = vdm.SelectQuery(cmd).Tables[0];
                DataView viewClo = new DataView(dtClo);
                DataTable distinctClo = viewClo.ToTable(true, "BranchId");
                double totalclos = 0;
                foreach (DataRow drp in distinctClo.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i++;
                    newrow["Route Name"] = "Closing Stock";
                    foreach (DataRow dr in dtClo.Rows)
                    {
                        if (drp["BranchId"].ToString() == dr["BranchId"].ToString())
                        {

                            double StockQty = 0;
                            double.TryParse(dr["StockQty"].ToString(), out StockQty);
                            StockQty = Math.Round(StockQty, 2);
                            newrow[dr["ProductName"].ToString()] = StockQty;
                            totalclos += StockQty;

                            if (StockQty == 0.0)
                            {
                            }
                            else
                            {

                                DataRow tempnewrow = dttempproducts.NewRow();
                                tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                tempnewrow["SubCatSno"] = dr["SubCat_sno"].ToString();
                                dttempproducts.Rows.Add(tempnewrow);
                            }
                        }
                    }
                    newrow["Total Qty"] = totalclos;
                    Report.Rows.Add(newrow);
                }




                DataTable dtproductsdata = new DataTable();
                DataTable dtAllLeaks = new DataTable();
                DataTable dtDispnames = new DataTable();
                DataTable dtoldnames = new DataTable();
                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno, dispatch.BranchID, tripdata.I_Date,tripdata.sno as TripSno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN branchmappingtable ON dispatch.BranchID = branchmappingtable.SubBranch WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (tripdata.Status <> 'C') AND (dispatch.DispMode = @Agent) AND (branchmappingtable.SuperBranch = @BranchID)  AND (dispatch.Branch_Id <> @Branch_ID) GROUP BY  tripdata.Sno");
                cmd.Parameters.AddWithValue("@Agent", "AGENT");
                cmd.Parameters.AddWithValue("@SM", "SM");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@Branch_ID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtAgentDesp = vdm.SelectQuery(cmd).Tables[0];
                if (ddlSalesOffice.SelectedValue == "1801")
                {
                }
                else
                {
                    cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno, dispatch.BranchID, tripdata.I_Date,tripdata.sno as TripSno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2)  and (dispatch.DispType='SO') and (tripdata.Status<>'C')  group by tripdata.Sno ORDER BY dispatch.sno");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    dtoldnames = vdm.SelectQuery(cmd).Tables[0];
                }

                DataTable dtNEWDESP = new DataTable();
                dtNEWDESP = dtAgentDesp.Copy();
                dtNEWDESP.Merge(dtoldnames);
                DataTable dtAgentDispnames = new DataTable();
                if (ddlSalesOffice.SelectedValue == "159")
                {
                    cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno, dispatch.BranchID, tripdata.I_Date,tripdata.sno as TripSno, dispatch.DispMode FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.DispType = @Agent OR dispatch.DispType = @SM) AND (dispatch.Branch_Id = @BranchID) and (tripdata.Status<>'C')  GROUP BY tripdata.sno");
                    cmd.Parameters.AddWithValue("@Agent", "AGENT");
                    cmd.Parameters.AddWithValue("@SM", "SM");
                    cmd.Parameters.AddWithValue("@BranchID", 4626);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    dtAgentDispnames = vdm.SelectQuery(cmd).Tables[0];
                }
                if (ddlSalesOffice.SelectedValue == "1801")
                {
                    cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno, dispatch.BranchID, tripdata.I_Date,tripdata.sno as TripSno, dispatch.DispMode FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.DispType = @Agent OR dispatch.DispType = @SM) AND (dispatch.Branch_Id = @BranchID) and (tripdata.Status<>'C')  GROUP BY tripdata.sno");
                    cmd.Parameters.AddWithValue("@Agent", "AGENT");
                    cmd.Parameters.AddWithValue("@SM", "SM");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    dtAgentDispnames = vdm.SelectQuery(cmd).Tables[0];
                }
                dtDispnames = dtNEWDESP.Copy();
                dtDispnames.Merge(dtAgentDispnames);
                dtAllLeaks = new DataTable();
                dtAllLeaks.Columns.Add("ShortQty");
                dtAllLeaks.Columns.Add("ProductID");
                dtAllLeaks.Columns.Add("LeakQty");
                dtAllLeaks.Columns.Add("FreeMilk");
                dtAllLeaks.Columns.Add("ReturnQty");
                dtAllLeaks.Columns.Add("VReturnQty");
                dtAllLeaks.Columns.Add("TripId");
                dtAllLeaks.Columns.Add("UnitPrice");
                foreach (DataRow drpdt in produtstbl.Rows)
                {
                    DataRow newRow = dtAllLeaks.NewRow();
                    newRow["ShortQty"] = "0";
                    newRow["ProductID"] = drpdt["Sno"].ToString();
                    newRow["FreeMilk"] = "0";
                    newRow["LeakQty"] = "0";
                    newRow["ReturnQty"] = "0";
                    newRow["VReturnQty"] = "0";
                    newRow["TripId"] = "0";
                    newRow["UnitPrice"] = drpdt["unitprice"].ToString();
                    dtAllLeaks.Rows.Add(newRow);
                }
                cmd = new MySqlCommand("SELECT branchleaktrans.EmpId, branchleaktrans.TripId, branchleaktrans.ProdId, branchleaktrans.LeakQty, branchleaktrans.DOE, branchleaktrans.BranchID,branchleaktrans.Status, branchleaktrans.FreeQty, branchleaktrans.ShortQty, tripdata.I_Date, branchproducts.Rank, productsdata.ProductName FROM branchleaktrans INNER JOIN tripdata ON branchleaktrans.TripId = tripdata.Sno INNER JOIN branchproducts ON branchleaktrans.BranchID = branchproducts.branch_sno AND branchleaktrans.ProdId = branchproducts.product_sno INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchleaktrans.BranchID = @BranchID) GROUP BY tripdata.I_Date, branchleaktrans.ProdId ORDER BY tripdata.I_Date, branchproducts.Rank");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtsalesofficeLeaks = vdm.SelectQuery(cmd).Tables[0];
                if (dtsalesofficeLeaks.Rows.Count > 0)
                {
                    DataRow newLeakages = Report.NewRow();
                    //newLeakages["Route Name"] = "PuffLeakages" + " " + Disp;
                    newLeakages["Route Name"] = "Puff_Leakages";
                    float totLeakQty = 0;
                    //float totLeakAmount = 0;
                    foreach (DataRow drNewRowLeaks in dtsalesofficeLeaks.Rows)
                    {
                        foreach (DataRow drdt in produtstbl.Rows)
                        {
                            if (drNewRowLeaks["ProdId"].ToString() == drdt["sno"].ToString())
                            {


                                float LeakQty = 0;
                                float.TryParse(drNewRowLeaks["LeakQty"].ToString(), out LeakQty);

                                if (LeakQty == 0.0)
                                {
                                }
                                else
                                {



                                    newLeakages[drdt["ProductName"].ToString()] = Math.Round(LeakQty, 2);
                                    float UnitCost = 0;
                                    float.TryParse(drdt["unitprice"].ToString(), out UnitCost);
                                    // float Total = LeakQty * UnitCost;
                                    totLeakQty += LeakQty;
                                }
                                //totLeakAmount += Total;
                            }

                        }
                    }


                    foreach (DataRow drpuffLeaks in dtsalesofficeLeaks.Rows)
                    {
                        foreach (DataRow drNew in dtAllLeaks.Rows)
                        {
                            if (drpuffLeaks["ProdId"].ToString() == drNew["ProductID"].ToString())
                            {
                                float LeakQty = 0;
                                float.TryParse(drpuffLeaks["LeakQty"].ToString(), out LeakQty);
                                float AllLeaks = 0;
                                float.TryParse(drNew["LeakQty"].ToString(), out AllLeaks);
                                float TotalShortQty = LeakQty + AllLeaks;
                                drNew["LeakQty"] = TotalShortQty;
                            }
                        }
                    }
                    foreach (DataRow drdelivery in dtalldelivery.Rows)
                    {
                        foreach (DataRow drpuff in dtAllLeaks.Rows)
                        {
                            if (drdelivery["sno"].ToString() == drpuff["ProductID"].ToString())
                            {
                                float shortQty = 0;
                                float.TryParse(drpuff["LeakQty"].ToString(), out shortQty);
                                drdelivery["LeakQty"] = Math.Round(shortQty, 2);
                            }
                        }
                    }
                    newLeakages["Total Qty"] = Math.Round(totLeakQty, 2);
                    grnd_tot_qty += totLeakQty;
                    //newLeakages["Total Amount"] = Math.Round(totLeakAmount, 2);
                    if (totLeakQty > 0)
                    {
                        Report.Rows.Add(newLeakages);
                    }
                }
                if (dtDispnames.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDispnames.Rows)
                    {
                        ////......01/09/2016  CRREDDY..........
                        cmd = new MySqlCommand("SELECT Triproutes.Tripdata_sno, Triproutes.RouteID, ff.Sno, ff.TotalLeaks, ff.VLeaks, ff.VReturns, ff.ReturnQty, ff.ProductName, ff.ProductID, ff.FreeMilk, ff.ShortQty FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (RouteID = @dispatchSno)) Triproutes INNER JOIN (SELECT Sno, TotalLeaks, VLeaks, VReturns, ReturnQty, ProductName, ProductID, FreeMilk, ShortQty FROM (SELECT tripdata.Sno, leakages.TotalLeaks, leakages.VLeaks, leakages.VReturns, leakages.ReturnQty, productsdata.ProductName, leakages.ProductID, leakages.FreeMilk,  leakages.ShortQty FROM  leakages INNER JOIN tripdata ON leakages.TripID = tripdata.Sno INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2)) tripinfo) ff ON ff.Sno = Triproutes.Tripdata_sno");
                        cmd.Parameters.AddWithValue("@dispatchSno", dr["sno"].ToString());
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtLeakble = vdm.SelectQuery(cmd).Tables[0];
                        string TripDataSno = "0";
                        if (dtLeakble.Rows.Count > 0)
                        {
                            TripDataSno = dtLeakble.Rows[0]["Sno"].ToString();
                        }
                        else
                        {
                            TripDataSno = "0";
                        }
                        cmd = new MySqlCommand("SELECT EmpId, TripId, ProdId, LeakQty, DOE, BranchID, Status, FreeQty, ShortQty FROM branchleaktrans WHERE (TripId = @tripid)");
                        cmd.Parameters.AddWithValue("@tripid", TripDataSno);
                        DataTable dtsalesofficeshortfree = vdm.SelectQuery(cmd).Tables[0];


                        foreach (DataRow drr in dtLeakble.Rows)
                        {
                            foreach (DataRow drallleak in dtAllLeaks.Rows)
                            {
                                if (drr["ProductID"].ToString() == drallleak["ProductID"].ToString())
                                {
                                    float leakqty = 0;
                                    float leakqtycpy = 0;
                                    float shortQty = 0;
                                    float shortQtycpy = 0;
                                    float freeqty = 0;
                                    float freeqtycpy = 0;
                                    float.TryParse(drr["TotalLeaks"].ToString(), out leakqty);
                                    float.TryParse(drallleak["LeakQty"].ToString(), out leakqtycpy);
                                    float.TryParse(drr["ShortQty"].ToString(), out shortQty);
                                    float.TryParse(drallleak["ShortQty"].ToString(), out shortQtycpy);
                                    float.TryParse(drr["FreeMilk"].ToString(), out freeqty);
                                    float.TryParse(drallleak["FreeMilk"].ToString(), out freeqtycpy);

                                    drallleak["ShortQty"] = shortQty + shortQtycpy;
                                    drallleak["LeakQty"] = leakqty + leakqtycpy;
                                    drallleak["FreeMilk"] = freeqtycpy + freeqty;
                                    float retunQty = 0;
                                    float VretunQty = 0;
                                    float.TryParse(drallleak["ReturnQty"].ToString(), out retunQty);
                                    float.TryParse(drallleak["VReturnQty"].ToString(), out VretunQty);
                                    float alreturnqty = 0;
                                    float alVreturnqty = 0;
                                    float.TryParse(drr["ReturnQty"].ToString(), out alreturnqty);
                                    float.TryParse(drr["VReturns"].ToString(), out alVreturnqty);
                                    float totalreturn = 0;
                                    float totalVreturn = 0;
                                    totalreturn = retunQty + alreturnqty;
                                    totalVreturn = VretunQty + alVreturnqty;
                                    drallleak["ReturnQty"] = Math.Round(totalreturn, 2);
                                    drallleak["VReturnQty"] = Math.Round(totalVreturn, 2);
                                    drallleak["TripId"] = drr["Sno"].ToString();
                                }
                            }
                        }
                        foreach (DataRow drshortfree in dtsalesofficeshortfree.Rows)
                        {
                            foreach (DataRow drallleak in dtAllLeaks.Rows)
                            {
                                if (drshortfree["ProdId"].ToString() == drallleak["ProductID"].ToString())
                                {

                                    float shortQty = 0;
                                    float freeQty = 0;
                                    float soshortQty = 0;
                                    float sofreeQty = 0;
                                    float.TryParse(drallleak["ShortQty"].ToString(), out shortQty);
                                    float.TryParse(drallleak["FreeMilk"].ToString(), out freeQty);
                                    float.TryParse(drshortfree["ShortQty"].ToString(), out soshortQty);
                                    float.TryParse(drshortfree["FreeQty"].ToString(), out sofreeQty);
                                    float totalshort = shortQty + soshortQty;
                                    float totalfree = freeQty + sofreeQty;
                                    drallleak["ShortQty"] = Math.Round(totalshort, 2);
                                    drallleak["FreeMilk"] = Math.Round(totalfree, 2);
                                }
                            }
                        }

                        string Disp = dr["DispName"].ToString();
                        string[] strName = Disp.Split('_');
                        DataRow newLeakages = Report.NewRow();
                        newLeakages["Route Name"] = "Leakages" + " " + Disp;
                        float totLeakQty = 0;
                        float totLeakAmount = 0;
                        foreach (DataRow drNewRowLeaks in dtLeakble.Rows)
                        {
                            foreach (DataRow drdt in produtstbl.Rows)
                            {
                                if (drNewRowLeaks["ProductID"].ToString() == drdt["sno"].ToString())
                                {
                                    float LeakQty = 0;
                                    float.TryParse(drNewRowLeaks["TotalLeaks"].ToString(), out LeakQty);
                                    if (LeakQty == 0.0)
                                    {
                                    }
                                    else
                                    {

                                        newLeakages[drdt["ProductName"].ToString()] = Math.Round(LeakQty, 2);
                                        float UnitCost = 0;
                                        float.TryParse(drdt["unitprice"].ToString(), out UnitCost);
                                        float Total = LeakQty * UnitCost;
                                        totLeakQty += LeakQty;
                                        totLeakAmount += Total;
                                    }
                                }

                            }
                        }
                        newLeakages["Total Qty"] = Math.Round(totLeakQty, 2);
                        grnd_tot_qty += totLeakQty;
                        newLeakages["Total Amount"] = Math.Round(totLeakAmount, 2);
                        grnd_tot_amount += totLeakAmount;
                        if (totLeakQty > 0)
                        {
                            Report.Rows.Add(newLeakages);
                        }
                        DataRow newVLeakages = Report.NewRow();
                        newVLeakages["Route Name"] = "VLeakages" + " " + Disp;
                        float totVLeakQty = 0;
                        float totVLeakAmount = 0;
                        foreach (DataRow drNewRowLeaks in dtLeakble.Rows)
                        {
                            foreach (DataRow drdt in produtstbl.Rows)
                            {
                                if (drNewRowLeaks["ProductID"].ToString() == drdt["sno"].ToString())
                                {

                                    float LeakQty = 0;
                                    float.TryParse(drNewRowLeaks["VLeaks"].ToString(), out LeakQty);
                                    if (LeakQty == 0.0)
                                    {
                                    }
                                    else
                                    {
                                        newVLeakages[drdt["ProductName"].ToString()] = Math.Round(LeakQty, 2);
                                        float UnitCost = 0;
                                        float.TryParse(drdt["unitprice"].ToString(), out UnitCost);
                                        float Total = LeakQty * UnitCost;
                                        totVLeakQty += LeakQty;
                                        totVLeakAmount += Total;
                                    }
                                }

                            }
                        }
                        if (totVLeakQty > 0)
                        {
                            Report.Rows.Add(newVLeakages);
                        }
                    }
                    cmd = new MySqlCommand("SELECT tripdata.EmpId,DispatcherID FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.sno =@dispatchSno) AND (tripdata.I_Date BETWEEN @d1 AND @d2)");
                    cmd.Parameters.AddWithValue("@dispatchSno", dtDispnames.Rows[0]["sno"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    DataTable DtEmpId = vdm.SelectQuery(cmd).Tables[0];
                    cmd = new MySqlCommand("SELECT Sno FROM  tripdata WHERE (DEmpId = @DEmpId) AND (I_Date BETWEEN @d1 AND @d2)");
                    cmd.Parameters.AddWithValue("@DEmpId", DtEmpId.Rows[0]["EmpId"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    DataTable DtTripId = vdm.SelectQuery(cmd).Tables[0];
                    if (DtTripId.Rows.Count == 0)
                    {
                        cmd = new MySqlCommand("SELECT Sno FROM  tripdata WHERE (DEmpId = @DEmpId) AND (I_Date BETWEEN @d1 AND @d2)");
                        cmd.Parameters.AddWithValue("@DEmpId", DtEmpId.Rows[0]["DispatcherID"].ToString());
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DtTripId = vdm.SelectQuery(cmd).Tables[0];
                    }
                    DataTable DtLeaks = new DataTable();
                    foreach (DataRow drTrip in DtTripId.Rows)
                    {
                        cmd = new MySqlCommand("SELECT ShortQty, ProductID, LeakQty,ReturnQty,FreeMilk FROM leakages WHERE (TripID = @TripID) and VarifyStatus IS NULL");
                        cmd.Parameters.AddWithValue("@TripID", drTrip["Sno"].ToString());
                        DtLeaks = vdm.SelectQuery(cmd).Tables[0];
                        foreach (DataRow drLeaks in DtLeaks.Rows)
                        {
                            foreach (DataRow drNew in dtAllLeaks.Rows)
                            {
                                if (drLeaks["ProductID"].ToString() == drNew["ProductID"].ToString())
                                {
                                    float ShortQty = 0;
                                    float.TryParse(drLeaks["ShortQty"].ToString(), out ShortQty);
                                    float AllShort = 0;
                                    float.TryParse(drNew["ShortQty"].ToString(), out AllShort);
                                    float TotalShortQty = ShortQty + AllShort;
                                    drNew["ShortQty"] = TotalShortQty;
                                    float FreeMilk = 0;
                                    float.TryParse(drLeaks["FreeMilk"].ToString(), out FreeMilk);
                                    float AllFree = 0;
                                    float.TryParse(drNew["FreeMilk"].ToString(), out AllFree);
                                    float TotalFreeQty = FreeMilk + AllFree;
                                    drNew["FreeMilk"] = TotalFreeQty;
                                    float ReturnMilk = 0;
                                    float.TryParse(drLeaks["ReturnQty"].ToString(), out ReturnMilk);
                                    float AllReturn = 0;
                                    float.TryParse(drNew["ReturnQty"].ToString(), out AllReturn);
                                    float TotalReturnQty = ReturnMilk + AllReturn;
                                    drNew["ReturnQty"] = TotalReturnQty;
                                }
                            }
                        }
                    }
                }
                DataRow totalschememilk = Report.NewRow();
                totalschememilk["Route Name"] = "SCHEME MILK";
                float totOfferFreeQty = 0;
                float totOfferFreeAmount = 0;
                foreach (DataRow drOfferfree in dt_offertble.Rows)
                {
                    foreach (DataRow drdt in produtstbl.Rows)
                    {
                        if (drOfferfree["ProductID"].ToString() == drdt["sno"].ToString())
                        {
                            float freeQty = 0;
                            float.TryParse(drOfferfree["Delqty"].ToString(), out freeQty);
                            totalschememilk[drdt["ProductName"].ToString()] = Math.Round(freeQty, 2);
                            float UnitCost = 0;
                            float.TryParse(drOfferfree["unit_price"].ToString(), out UnitCost);
                            float Total = freeQty * UnitCost;
                            totOfferFreeQty += freeQty;
                            totOfferFreeAmount += Total;
                        }
                    }
                }
                totalschememilk["Total Qty"] = Math.Round(totOfferFreeQty, 2);
                grnd_tot_qty += totOfferFreeQty;
                totalschememilk["Total Amount"] = Math.Round(totOfferFreeAmount, 2);
                grnd_tot_amount += totOfferFreeAmount;
                Report.Rows.Add(totalschememilk);

                DataRow totalfreemilk = Report.NewRow();
                totalfreemilk["Route Name"] = "FREE MILK";
                float totFreeQty = 0;
                float totFreeAmount = 0;
                foreach (DataRow drNewRowfree in dtAllLeaks.Rows)
                {
                    foreach (DataRow drdt in produtstbl.Rows)
                    {
                        if (drNewRowfree["ProductID"].ToString() == drdt["sno"].ToString())
                        {
                            float freeQty = 0;
                            float.TryParse(drNewRowfree["FreeMilk"].ToString(), out freeQty);
                            if (freeQty == 0.0)
                            {
                            }
                            else
                            {
                                totalfreemilk[drdt["ProductName"].ToString()] = Math.Round(freeQty, 2);
                                float UnitCost = 0;
                                float.TryParse(drNewRowfree["UnitPrice"].ToString(), out UnitCost);
                                float Total = freeQty * UnitCost;
                                totFreeQty += freeQty;
                                totFreeAmount += Total;
                            }
                        }
                    }
                }
                totalfreemilk["Total Qty"] = Math.Round(totFreeQty, 2);
                grnd_tot_qty += totFreeQty;
                totalfreemilk["Total Amount"] = Math.Round(totFreeAmount, 2);
                grnd_tot_amount += totFreeAmount;
                Report.Rows.Add(totalfreemilk);
                foreach (DataRow drdelivery in dtalldelivery.Rows)
                {
                    foreach (DataRow drfree in dtAllLeaks.Rows)
                    {
                        if (drdelivery["sno"].ToString() == drfree["ProductID"].ToString())
                        {
                            float freeQty = 0;
                            float.TryParse(drfree["FreeMilk"].ToString(), out freeQty);
                            drdelivery["freeQty"] = Math.Round(freeQty, 2);
                        }
                    }
                }

                foreach (DataRow drdelivery in dtalldelivery.Rows)
                {
                    foreach (DataRow drfree in dt_offertble.Rows)
                    {
                        if (drdelivery["sno"].ToString() == drfree["ProductID"].ToString())
                        {
                            float freeQty = 0;
                            float PrevfreeQty = 0;
                            float totfreeQty = 0;
                            float.TryParse(drfree["Delqty"].ToString(), out freeQty);
                            totfreeQty = freeQty;
                            drdelivery["scheme"] = Math.Round(totfreeQty, 2);
                        }
                    }
                }

                foreach (DataRow drp in distinctLeaks.Rows)
                {
                    if (drp["DispMode"].ToString() == "Free")
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["SNo"] = i++;
                        newrow["Route Name"] = drp["DispName"].ToString();
                        double total = 0;
                        foreach (DataRow dr in dtlOCAL.Rows)
                        {
                            if (dr["DispMode"].ToString() == "Free")
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {

                                    double Qty = 0;
                                    double.TryParse(dr["Qty"].ToString(), out Qty);
                                    if (Qty == 0.0)
                                    {
                                    }
                                    else
                                    {
                                        newrow[dr["ProductName"].ToString()] = Qty;
                                        total += Qty;
                                    }
                                }
                            }
                        }
                        newrow["Total Qty"] = total;
                        grnd_tot_qty += total;
                        Report.Rows.Add(newrow);
                    }
                }
                DataRow totalshortmilk = Report.NewRow();
                totalshortmilk["Route Name"] = "SHORT MILK";
                float totshortQty = 0;
                float totshortAmount = 0;
                foreach (DataRow drNewRowshort in dtAllLeaks.Rows)
                {
                    foreach (DataRow drdt in produtstbl.Rows)
                    {
                        if (drNewRowshort["ProductID"].ToString() == drdt["sno"].ToString())
                        {
                            float shortQty = 0;
                            float.TryParse(drNewRowshort["ShortQty"].ToString(), out shortQty);
                            if (shortQty == 0.0)
                            {
                            }
                            else
                            {
                                totalshortmilk[drdt["ProductName"].ToString()] = Math.Round(shortQty, 2);
                                float UnitCost = 0;
                                float.TryParse(drNewRowshort["UnitPrice"].ToString(), out UnitCost);
                                float Total = shortQty * UnitCost;
                                totshortQty += shortQty;
                                totshortAmount += Total;
                            }
                        }
                    }
                }
                totalshortmilk["Total Qty"] = Math.Round(totshortQty, 2);
                grnd_tot_qty += totshortQty;
                totalshortmilk["Total Amount"] = Math.Round(totshortAmount, 2);
                grnd_tot_amount += totshortAmount;
                Report.Rows.Add(totalshortmilk);
                foreach (DataRow drdelivery in dtalldelivery.Rows)
                {

                    foreach (DataRow drshort in dtAllLeaks.Rows)
                    {
                        if (drdelivery["sno"].ToString() == drshort["ProductID"].ToString())
                        {
                            float shortQty = 0;
                            float.TryParse(drshort["ShortQty"].ToString(), out shortQty);
                            drdelivery["shortQty"] = Math.Round(shortQty, 2);
                        }
                    }
                }
                DataRow totalreturnmilk = Report.NewRow();
                totalreturnmilk["Route Name"] = "RETURN MILK";

                float totreturnQty = 0;
                float totreturnAmount = 0;
                foreach (DataRow drNewRowreturn in dtAllLeaks.Rows)
                {
                    foreach (DataRow drdt in produtstbl.Rows)
                    {
                        if (drNewRowreturn["ProductID"].ToString() == drdt["sno"].ToString())
                        {
                            float returnQty = 0;
                            float.TryParse(drNewRowreturn["ReturnQty"].ToString(), out returnQty);
                            if (returnQty == 0.0)
                            {
                            }
                            else
                            {
                                totalreturnmilk[drdt["ProductName"].ToString()] = Math.Round(returnQty, 2);
                                float UnitCost = 0;
                                float.TryParse(drNewRowreturn["UnitPrice"].ToString(), out UnitCost);
                                float Total = returnQty * UnitCost;
                                totreturnQty += returnQty;
                                totreturnAmount += Total;
                            }
                        }
                    }
                }
                totalreturnmilk["Total Qty"] = Math.Round(totreturnQty, 2);
                grnd_tot_qty += totreturnQty;
                totalreturnmilk["Total Amount"] = Math.Round(totreturnAmount, 2);
                grnd_tot_amount += totreturnAmount;
                Report.Rows.Add(totalreturnmilk);
                DataRow newVReturns = Report.NewRow();
                newVReturns["Route Name"] = "VReturns";
                float totVReturnQty = 0;
                float totVReturnAmount = 0;
                foreach (DataRow drNewRowLeaks in dtAllLeaks.Rows)
                {
                    foreach (DataRow drdt in produtstbl.Rows)
                    {
                        if (drNewRowLeaks["ProductID"].ToString() == drdt["sno"].ToString())
                        {
                            float LeakQty = 0;
                            float.TryParse(drNewRowLeaks["VReturnQty"].ToString(), out LeakQty);
                            if (LeakQty == 0.0)
                            {
                            }
                            else
                            {
                                newVReturns[drdt["ProductName"].ToString()] = Math.Round(LeakQty, 2);
                                float UnitCost = 0;
                                float.TryParse(drdt["unitprice"].ToString(), out UnitCost);
                                float Total = LeakQty * UnitCost;
                                totVReturnQty += LeakQty;
                                totVReturnAmount += Total;
                            }
                        }

                    }
                }
                newVReturns["Total Qty"] = Math.Round(totVReturnQty, 2);
                Report.Rows.Add(newVReturns);
                foreach (DataRow drdelivery in dtalldelivery.Rows)
                {
                    foreach (DataRow drreturn in dtAllLeaks.Rows)
                    {
                        if (drdelivery["sno"].ToString() == drreturn["ProductID"].ToString())
                        {
                            float returnQty = 0;
                            float.TryParse(drreturn["ReturnQty"].ToString(), out returnQty);
                            drdelivery["returnQty"] = Math.Round(returnQty, 2);
                        }
                    }
                }
                //new total
                DataRow newvartical1 = Report.NewRow();
                newvartical1["Route Name"] = "TOTAL";
                foreach (DataRow drtotaldel in dtAllLeaks.Rows)
                {
                    foreach (DataRow drdelivery in dtalldelivery.Rows)
                    {
                        if (drtotaldel["ProductID"].ToString() == drdelivery["sno"].ToString())
                        {
                            float qtycpy = 0;
                            float shortqty = 0;
                            float returnqty = 0;
                            float freeqty = 0;
                            float qtydelivercpy = 0;
                            float scheme = 0;
                            float.TryParse(drtotaldel["LeakQty"].ToString(), out qtycpy);
                            float.TryParse(drtotaldel["ShortQty"].ToString(), out shortqty);
                            float.TryParse(drtotaldel["ReturnQty"].ToString(), out returnqty);
                            float.TryParse(drtotaldel["FreeMilk"].ToString(), out freeqty);
                            float.TryParse(drdelivery["deliverQty"].ToString(), out qtydelivercpy);
                            float.TryParse(drdelivery["scheme"].ToString(), out scheme);
                            float totdelivery = 0;
                            totdelivery = qtycpy + qtydelivercpy + shortqty + returnqty + freeqty + scheme;
                            if (totdelivery == 0.0)
                            {
                            }
                            else
                            {
                                newvartical1[drdelivery["ProductName"].ToString()] = Math.Round(totdelivery, 2);
                                newvartical1["Total Qty"] = Math.Round(grnd_tot_qty, 2);
                                newvartical1["Total Amount"] = Math.Round(grnd_tot_amount, 2);
                                newvartical1["Cash Amount"] = Math.Round(grnd_tot_cashamount, 2);
                                newvartical1["Dues Amount"] = Math.Round(grnd_tot_dueamount, 2);

                            }
                        }
                        else
                        {
                        }
                    }
                }
                Report.Rows.Add(newvartical1);
                DataRow Break2 = Report.NewRow();
                Break2["Route Name"] = "RECEIVED MILK";
                Report.Rows.Add(Break2);
                foreach (DataRow drSub in dtDispnames.Rows)
                {
                    /////01/09/2016 CRREDDY.....................
                    cmd = new MySqlCommand("SELECT ff.TripID, Triproutes.RouteID, ff.Qty, ff.ProductId, Triproutes.Tripdata_sno FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (Tripdata_sno = @Tripdata_sno)) Triproutes INNER JOIN (SELECT TripID, Qty, ProductId FROM (SELECT tripdata.Sno AS TripID, tripsubdata.Qty, tripsubdata.ProductId FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno WHERE (tripdata.I_Date BETWEEN @starttime AND @endtime)) tripinfo) ff ON ff.TripID = Triproutes.Tripdata_sno");
                    cmd.Parameters.AddWithValue("@Tripdata_sno", drSub["TripSno"].ToString());
                    cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
                    DataTable DtTripSubData = vdm.SelectQuery(cmd).Tables[0];
                    string Disp = drSub["DispName"].ToString();
                    string[] strName = Disp.Split('_');
                    DataRow newSo = Report.NewRow();
                    newSo["Route Name"] = strName[0];

                    foreach (DataRow drTripSub in DtTripSubData.Rows)
                    {
                        foreach (DataRow drdt in produtstbl.Rows)
                        {
                            if (drTripSub["ProductId"].ToString() == drdt["Sno"].ToString())
                            {
                                float Qty = 0;
                                float.TryParse(drTripSub["Qty"].ToString(), out Qty);
                                if (Qty == 0.0)
                                {
                                }
                                else
                                {
                                    newSo[drdt["ProductName"].ToString()] = drTripSub["Qty"].ToString();
                                    DataRow tempnewrow = dttempproducts.NewRow();
                                    tempnewrow["ProductName"] = drdt["ProductName"].ToString();
                                    tempnewrow["SubCatSno"] = drdt["SubCat_sno"].ToString();
                                    dttempproducts.Rows.Add(tempnewrow);
                                }
                            }
                        }
                        foreach (DataRow dralldispqty in dtalldispatch.Rows)
                        {
                            if (drTripSub["ProductId"].ToString() == dralldispqty["sno"].ToString())
                            {
                                float qty = 0;
                                float.TryParse(drTripSub["Qty"].ToString(), out qty);
                                float qtycpy = 0;
                                float.TryParse(dralldispqty["TotalQty"].ToString(), out qtycpy);
                                float totalqty = qty + qtycpy;
                                dralldispqty["TotalQty"] = totalqty;
                            }
                            else
                            {
                            }
                        }
                    }
                    Report.Rows.Add(newSo);
                }




                DataRow totaldispatch = Report.NewRow();
                totaldispatch["Route Name"] = "TOTAL";
                DataRow TotalRow = Report.NewRow();
                TotalRow["Route Name"] = "DIFFERENCE";
                foreach (DataRow dr in produtstbl.Rows)
                {
                    foreach (DataRow dralldispqty in dtalldispatch.Rows)
                    {
                        if (dr["Sno"].ToString() == dralldispqty["sno"].ToString())
                        {
                            float qtycpy = 0;
                            float.TryParse(dralldispqty["TotalQty"].ToString(), out qtycpy);
                            float totalqty = 0;
                            totalqty = qtycpy;
                            if (totalqty == 0.0)
                            {
                            }
                            else
                            {
                                totaldispatch[dr["ProductName"].ToString()] = Math.Round(totalqty, 2);
                                DataRow tempnewrow = dttempproducts.NewRow();
                                tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                tempnewrow["SubCatSno"] = dr["SubCat_sno"].ToString();
                                dttempproducts.Rows.Add(tempnewrow);
                            }
                        }
                        else
                        {
                        }
                    }
                }
                Report.Rows.Add(totaldispatch);
                DataView SubCatview = new DataView(dttempproducts);
                dtSubCatgory = SubCatview.ToTable(true, "SubCatSno");
                DataView dv1 = dtSubCatgory.DefaultView;
                dv1.Sort = "SubCatSno ASC";
                dtSortedSubCategory = dv1.ToTable();


                /// Ravi
                cmd = new MySqlCommand("SELECT Leaks.short, Leaks.ProductID, Leaks.free, Leaks.I_Date, Leaks.totleak, Leaks.TripID, Leaks.BranchID, Leaks.Sno FROM (SELECT leakages.ShortQty AS short, leakages.ProductID, leakages.FreeMilk AS free, tripdata.I_Date, leakages.TotalLeaks AS totleak, leakages.TripID, tripdata.Sno, tripdata.BranchID FROM leakages INNER JOIN tripdata ON leakages.TripID = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno FROM  (SELECT dispatch.DispName, tripdata_1.Sno FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata tripdata_1 ON triproutes.Tripdata_sno = tripdata_1.Sno WHERE (dispatch.BranchID = @branchid) AND (tripdata_1.I_Date BETWEEN @d1 AND @d2)) dd) ff ON ff.Sno = Leaks.Sno");
                cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dttotsaleleak = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow drsaleleak in dttotsaleleak.Rows)
                {
                    foreach (DataRow drdelivery in dtalldelivery.Rows)
                    {

                        if (drsaleleak["ProductID"].ToString() == drdelivery["sno"].ToString())
                        {
                            float leakqty = 0;
                            float.TryParse(drdelivery["leakQty"].ToString(), out leakqty);
                            float leakcpy = 0;
                            float.TryParse(drsaleleak["totleak"].ToString(), out leakcpy);
                            float totalleakqty = leakqty + leakcpy;
                            drdelivery["leakQty"] = totalleakqty;
                        }
                        else
                        {
                        }
                    }
                }
                foreach (var column in Report.Columns.Cast<DataColumn>().ToArray())
                {
                    if (Report.AsEnumerable().All(dr => dr.IsNull(column)))
                        Report.Columns.Remove(column);
                }
                foreach (DataRow dralldispqty in dtalldispatch.Rows)
                {
                    foreach (DataRow drdelivery in dtalldelivery.Rows)
                    {
                        if (dralldispqty["Sno"].ToString() == drdelivery["sno"].ToString())
                        {
                            try
                            {
                                float totimwardqtycpy = 0;
                                float qtydelivercpy = 0;
                                float freeQty = 0;
                                float leakQty = 0;
                                float shortQty = 0;
                                float returnQty = 0;
                                float scheme = 0;
                                float.TryParse(dralldispqty["TotalQty"].ToString(), out totimwardqtycpy);
                                foreach (DataRow branchroute in dtOpp.Rows)
                                {
                                    if (branchroute["sno"].ToString() == drdelivery["sno"].ToString())
                                    {
                                        float StockQty = 0;
                                        float.TryParse(branchroute["StockQty"].ToString(), out StockQty);
                                        totimwardqtycpy = totimwardqtycpy + StockQty;
                                    }
                                    else
                                    {
                                    }
                                }
                                float.TryParse(drdelivery["deliverQty"].ToString(), out qtydelivercpy);
                                float.TryParse(drdelivery["freeQty"].ToString(), out freeQty);
                                float.TryParse(drdelivery["leakQty"].ToString(), out leakQty);
                                float.TryParse(drdelivery["shortQty"].ToString(), out shortQty);
                                float.TryParse(drdelivery["returnQty"].ToString(), out returnQty);
                                float.TryParse(drdelivery["scheme"].ToString(), out scheme);
                                float totdelivery = 0;
                                float totdifference = 0;
                                totdelivery = freeQty + leakQty + shortQty + returnQty + qtydelivercpy + scheme;
                                foreach (DataRow branchroute in dtClo.Rows)
                                {
                                    if (branchroute["sno"].ToString() == drdelivery["sno"].ToString())
                                    {
                                        float closStockQty = 0;
                                        float.TryParse(branchroute["StockQty"].ToString(), out closStockQty);
                                        totdelivery = totdelivery + closStockQty;
                                    }
                                    else
                                    {
                                    }
                                }
                                totdifference = totimwardqtycpy - totdelivery;
                                if (totimwardqtycpy == 0.0)
                                {
                                }
                                else
                                {
                                    TotalRow[dralldispqty["ProductName"].ToString()] = Math.Round(totdifference, 2);
                                }
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                        }
                    }
                }
                Report.Rows.Add(TotalRow);
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
                    if (dgvr.Cells[1].Text.Contains("Leakages") && !dgvr.Cells[1].Text.Contains("VLeakages"))
                    {
                        GridViewRow compare = grdReports.Rows[ii + 1];
                        for (int rowcnt = 2; rowcnt < dgvr.Cells.Count; rowcnt++)
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
                    if (dgvr.Cells[1].Text.Contains("DIFFERENCE"))
                    {
                        for (int rowcnt = 2; rowcnt < dgvr.Cells.Count; rowcnt++)
                        {
                            int diff = 0;
                            int.TryParse(dgvr.Cells[rowcnt].Text, out diff);
                            if (diff > 0 || diff < 0)
                            {
                                dgvr.Cells[rowcnt].BackColor = Color.SandyBrown;
                            }
                        }
                    }
                }
                Session["xportdata"] = Report;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdReports.DataSource = Report;
            grdReports.DataBind();
        }
    }
    void getHYDReport()
    {
        try
        {
            lblmsg.Text = "";
            Report = new DataTable();
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Plant")
            {
                lblDispatchName.Text = ddlSalesOffice.SelectedItem.Text;
                lblDate.Text = txtdate.Text;
            }
            else
            {
                lblDispatchName.Text = ddlSalesOffice.SelectedItem.Text;
                lblDate.Text = txtdate.Text;
            }
            Session["RouteName"] = "NET SALES FOR " + ddlSalesOffice.SelectedItem.Text;
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
            Session["filename"] = ddlSalesOffice.SelectedItem.Text + " REPORT " + fromdate.AddDays(1).ToString("dd/MM/yyyy");
            //cmd = new MySqlCommand("SELECT branchroutes.RouteName, indents_subtable.Product_sno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS Total, indents_subtable.UnitCost, ROUND(SUM(indents_subtable.LeakQty), 2) AS ILeakQty, products_category.Categoryname, ROUND(SUM(indents_subtable.LeakQty), 2) AS LeakQty, indents_subtable.DTripId, tripdata.RecieptNo FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN indents ON branchdata.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN tripdata ON indents_subtable.DTripId = tripdata.Sno WHERE (branchroutes.BranchID = @BranchID) AND (branchdata.CollectionType <> 'DUE') AND (indents.I_date BETWEEN @starttime AND @endtime) GROUP BY branchroutes.RouteName, productsdata.ProductName");
            cmd = new MySqlCommand("SELECT modifiedroutes.Sno, productsdata.SubCat_sno,modifiedroutes.RouteName, indents_subtable.Product_sno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty), 2) AS Total, indents_subtable.UnitCost, ROUND(SUM(indents_subtable.LeakQty), 2) AS ILeakQty, products_category.Categoryname, ROUND(SUM(indents_subtable.LeakQty), 2) AS LeakQty,indents_subtable.DTripId, tripdata.RecieptNo FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN  products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN tripdata ON indents_subtable.DTripId = tripdata.Sno WHERE (modifiedroutes.BranchID = @BranchID) AND (modifiedroutesubtable.EDate IS NULL) AND (branchdata.CollectionType <> 'DUE') AND (modifiedroutesubtable.CDate <= @starttime) AND (indents_subtable.DeliveryQty <>'0') OR (modifiedroutes.BranchID = @BranchID) AND (branchdata.CollectionType <> 'DUE') AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (indents_subtable.DeliveryQty <>'0')  GROUP BY modifiedroutes.Sno,productsdata.sno");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@startime", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT branchroutes.Sno, branchroutes.RouteName, tripdata.Sno AS Expr1, tripdata.ReceivedAmount, tripdata.CollectedAmount, tripdata.ReceivedAmount as SubmittedAmount FROM branchroutes INNER JOIN dispatch ON branchroutes.Sno = dispatch.Route_id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.Branch_Id = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2) GROUP BY dispatch.sno, tripdata.Sno");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtreceiptamount = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT branchproducts.Rank,productsdata.ProductName,productsdata.sno, products_category.Categoryname, productsdata.Units, productsdata.Qty,branchproducts.unitprice FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) ORDER BY branchproducts.Rank");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            DataTable dttable = vdm.SelectQuery(cmd).Tables[0];



            cmd = new MySqlCommand("SELECT    branchproducts.unitprice,branchproducts.branch_sno,products_subcategory.tempsub_catsno AS SubCatSno, products_category.description AS Categoryname, branchproducts.product_sno AS sno, productsdata.ProductName,productsdata.SubCat_sno, branchproducts.Rank,products_subcategory.description AS SubCategoryName FROM  products_category INNER JOIN products_subcategory ON products_category.sno = products_subcategory.category_sno INNER JOIN productsdata ON products_subcategory.sno = productsdata.SubCat_sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno WHERE (branchproducts.branch_sno = @BranchId) ORDER BY products_subcategory.tempsub_catsno, branchproducts.Rank");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            produtstbl1 = vdm.SelectQuery(cmd).Tables[0];


            cmd = new MySqlCommand("SELECT SUM(tripinvdata.Qty) AS issued, SUM(tripinvdata.Remaining) AS returnqty, invmaster.InvName, invmaster.sno, dispatch.sno AS dispatchsno,dispatch.DispName,dispatch.Route_id FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripinvdata ON tripdat.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno WHERE (dispatch.Branch_Id = @brnchid) AND (dispatch.DispType IS NULL) OR (branchdata.SalesOfficeID = @brnchid) AND (dispatch.DispType IS NULL) GROUP BY dispatch.sno, invmaster.sno");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@brnchid", ddlSalesOffice.SelectedValue);
            DataTable dtInventory = vdm.SelectQuery(cmd).Tables[0];
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
            produtstbl.Columns.Add("Rank").DataType = typeof(int);
            produtstbl.Columns.Add("SubCat_sno").DataType = typeof(int);

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

            DataView dv = produtstbl1.DefaultView;
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
            dtalldelivery.Columns.Add("scheme");
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
            MySqlCommand cmd1 = new MySqlCommand();
            ////cmd = new MySqlCommand("SELECT productsdata.sno AS prodsno, productsdata.ProductName, indents_subtable.DeliveryQty, indents_subtable.UnitCost, branchdata.BranchName,collections.AmountPaid, indents_subtable.DTripId, indent.Branch_id FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno LEFT OUTER JOIN collections ON indent.Branch_id = collections.Branchid AND indents_subtable.DTripId = collections.tripId WHERE (modifiedroutes.BranchID = @BranchID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) AND (branchdata.CollectionType = @CollectionType) OR (modifiedroutes.BranchID = @BranchID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (branchdata.CollectionType = @CollectionType) GROUP BY branchdata.sno, productsdata.sno");
            cmd = new MySqlCommand("SELECT  productsdata.sno AS prodsno,productsdata.SubCat_sno, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty),2) As DeliveryQty, indents_subtable.UnitCost, branchdata.BranchName,indents_subtable.DTripId, indent.Branch_id, branchdata.SalesType FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON branchdata.sno = modifiedroutesubtable.BranchID INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM indents WHERE  (I_date BETWEEN @starttime AND @endtime)) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (modifiedroutes.BranchID = @BranchID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) AND (branchdata.CollectionType = @CollectionType) AND (branchdata.SalesType NOT IN (35, 36, 37,41)) OR (modifiedroutes.BranchID = @BranchID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND  (branchdata.CollectionType = @CollectionType) AND (branchdata.SalesType NOT IN (35, 36, 37,41)) GROUP BY branchdata.sno, productsdata.sno, branchdata.SalesType");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@CollectionType", "DUE");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@startime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtDue = vdm.SelectQuery(cmd).Tables[0];

            /////Ravindra---03/11/2017
            cmd = new MySqlCommand("SELECT  productsdata.sno AS prodsno, productsdata.SubCat_sno,productsdata.ProductName, SUM(indents_subtable.DeliveryQty) AS DeliveryQty, indents_subtable.UnitCost, indents_subtable.DTripId, branchdata.SalesType,  branchdata.CollectionType,salestypemanagement.salestype AS Expr1  FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON branchdata.sno = modifiedroutesubtable.BranchID INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN salestypemanagement ON branchdata.SalesType = salestypemanagement.sno WHERE (modifiedroutes.BranchID = @BranchID) AND (branchdata.CollectionType = @CollectionType) AND (branchdata.SalesType = '35' OR branchdata.SalesType = '36' OR branchdata.SalesType = '37' OR branchdata.SalesType = '41' ) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (modifiedroutes.BranchID = @BranchID) AND (branchdata.CollectionType = @CollectionType) AND (branchdata.SalesType = '35' OR branchdata.SalesType = '36' OR branchdata.SalesType = '37' OR branchdata.SalesType = '41') AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY productsdata.sno, branchdata.SalesType, productsdata.ProductName, salestypemanagement.salestype");
            //////cmd1 = new MySqlCommand("SELECT  productsdata.sno AS prodsno, productsdata.ProductName, SUM(indents_subtable.DeliveryQty) AS DeliveryQty, indents_subtable.UnitCost, indents_subtable.DTripId,branchdata.SalesType, branchdata.CollectionType, salestypemanagement.salestype AS Expr1 FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON branchdata.sno = modifiedroutesubtable.BranchID INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM  indents WHERE  (I_date BETWEEN @starttime AND @endtime)) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN salestypemanagement ON branchdata.SalesType = salestypemanagement.sno WHERE (modifiedroutes.BranchID = @BranchID) AND (branchdata.CollectionType = @CollectionType) AND ((branchdata.SalesType = '35') OR (branchdata.SalesType = '36') OR (branchdata.SalesType = '37') OR (branchdata.SalesType = '41') ) GROUP BY productsdata.sno, branchdata.SalesType, productsdata.ProductName, salestypemanagement.salestype");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@CollectionType", "DUE");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@startime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtHYDDue1 = vdm.SelectQuery(cmd).Tables[0];
            //DataTable dtHYDDue1 = new DataTable();
            cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName, ROUND(SUM(offer_indents_sub.offer_indent_qty), 2) AS unitQty,offer_indents_sub.unit_price, productsdata.ProductName, productsdata.Units, productsdata.sno AS productid, ROUND(SUM(offer_indents_sub.offer_delivered_qty), 2) AS Delqty, products_category.Categoryname FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutes ON dispatch_sub.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT idoffer_indents, idoffers_assignment, salesoffice_id, route_id, agent_id, indent_date, indents_id, IndentType, I_modified_by FROM offer_indents WHERE (indent_date BETWEEN @starttime AND @endtime)) offerindents ON modifiedroutesubtable.BranchID = offerindents.agent_id INNER JOIN offer_indents_sub ON offerindents.idoffer_indents = offer_indents_sub.idoffer_indents INNER JOIN productsdata ON offer_indents_sub.product_id = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.Branch_Id = @BranchID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (dispatch.Branch_Id = @BranchID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY productsdata.sno");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            DataTable dt_offertble = vdm.SelectQuery(cmd).Tables[0];
            if (sortedProductDT.Rows.Count > 0)
            {
                DataView view = new DataView(dtble);
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("Route Name");
                foreach (DataRow dr in sortedProductDT.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                }
                Report.Columns.Add("Total Qty").DataType = typeof(Double);
                Report.Columns.Add("Total Amount").DataType = typeof(Double);
                Report.Columns.Add("Dues Amount").DataType = typeof(Double);
                Report.Columns.Add("Cash Amount").DataType = typeof(Double);
                Report.Columns.Add("Receipt No").DataType = typeof(Double);
                Report.Columns.Add("Crates Iss.").DataType = typeof(Double);
                Report.Columns.Add("Crates Rec.").DataType = typeof(Double);
                Report.Columns.Add("Cans Iss.").DataType = typeof(Double);
                Report.Columns.Add("Cans Rec.").DataType = typeof(Double);
                DataTable distincttable = view.ToTable(true, "Sno", "RouteName");

                dttempproducts = new DataTable();
                dttempproducts.Columns.Add("ProductName");
                dttempproducts.Columns.Add("SubCatSno").DataType = typeof(int); ;

                int i = 1;
                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    newrow["Route Name"] = branch["RouteName"].ToString();
                    double total = 0;
                    double Amount = 0;
                    float submittedAmount = 0;
                    float receivedAmount = 0;
                    foreach (DataRow dr in dtble.Rows)
                    {
                        if (branch["RouteName"].ToString() == dr["RouteName"].ToString())
                        {
                            double qtyvalue = 0;
                            double delqty = 0;
                            double.TryParse(dr["DeliveryQty"].ToString(), out delqty);
                            if (delqty == 0.0)
                            {
                            }
                            else
                            {
                                newrow[dr["ProductName"].ToString()] = Math.Round(delqty, 2);
                                int m = 0;

                                DataRow tempnewrow = dttempproducts.NewRow();
                                tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                tempnewrow["SubCatSno"] = dr["SubCat_sno"].ToString();
                                dttempproducts.Rows.Add(tempnewrow);
                                m++;
                            }


                            double.TryParse(dr["DeliveryQty"].ToString(), out qtyvalue);
                            double UnitCost = 0;
                            double.TryParse(dr["UnitCost"].ToString(), out UnitCost);
                            double DAmount = 0;
                            double.TryParse(dr["Total"].ToString(), out DAmount);
                            Amount += DAmount;
                            total += qtyvalue;
                        }
                    }
                    //double.TryParse(branch["RecieptNo"].ToString(), out recieptno);
                    foreach (DataRow drdtclubtotal in dtreceiptamount.Select("RouteName='" + branch["RouteName"].ToString() + "'"))
                    {
                        float.TryParse(drdtclubtotal["SubmittedAmount"].ToString(), out submittedAmount);
                        float.TryParse(drdtclubtotal["ReceivedAmount"].ToString(), out receivedAmount);
                    }
                    newrow["Total Qty"] = total;
                    newrow["Total Amount"] = Amount;
                    newrow["Cash Amount"] = submittedAmount;
                    //newrow["Receipt No"] = recieptno;
                    int cratesissued = 0;
                    int cratesreceived = 0;
                    int cansissued = 0;
                    int cansreceived = 0;
                    foreach (DataRow drdtinv in dtInventory.Select("Route_id='" + branch["Sno"].ToString() + "'"))
                    {
                        int cansi = 0;
                        int cansr = 0;
                        if (drdtinv["sno"].ToString() == "1")
                        {
                            int.TryParse(drdtinv["issued"].ToString(), out cratesissued);
                            int.TryParse(drdtinv["returnqty"].ToString(), out cratesreceived);
                        }
                        if (drdtinv["sno"].ToString() == "2")
                        {
                            int.TryParse(drdtinv["issued"].ToString(), out cansi);
                            int.TryParse(drdtinv["returnqty"].ToString(), out cansr);
                            cansissued += cansi;
                            cansreceived += cansr;
                        }
                        if (drdtinv["sno"].ToString() == "3")
                        {
                            int.TryParse(drdtinv["issued"].ToString(), out cansi);
                            int.TryParse(drdtinv["returnqty"].ToString(), out cansr);
                            cansissued += cansi;
                            cansreceived += cansr;
                        }
                        if (drdtinv["sno"].ToString() == "4")
                        {
                            int.TryParse(drdtinv["issued"].ToString(), out cansi);
                            int.TryParse(drdtinv["returnqty"].ToString(), out cansr);
                            cansissued += cansi;
                            cansreceived += cansr;
                        }
                        if (drdtinv["sno"].ToString() == "5")
                        {
                            int.TryParse(drdtinv["issued"].ToString(), out cansi);
                            int.TryParse(drdtinv["returnqty"].ToString(), out cansr);
                            cansissued += cansi;
                            cansreceived += cansr;
                        }
                    }
                    if (cratesissued == 0.0)
                    {
                    }
                    else
                    {
                        newrow["Crates Iss."] = cratesissued;
                    }
                    if (cratesreceived == 0.0)
                    {
                    }
                    else
                    {
                        newrow["Crates Rec."] = cratesreceived;
                    }
                    if (cansissued == 0.0)
                    {
                    }
                    else
                    {
                        newrow["Cans Iss."] = cansissued;
                    }
                    if (cansreceived == 0.0)
                    {
                    }
                    else
                    {
                        newrow["Cans Rec."] = cansreceived;
                    }
                    Report.Rows.Add(newrow);
                    i++;
                }
                foreach (DataRow branchroute in dtble.Rows)
                {
                    foreach (DataRow drtotdelivery in dtalldelivery.Rows)
                    {
                        if (branchroute["Product_sno"].ToString() == drtotdelivery["sno"].ToString())
                        {
                            float qty = 0;
                            float.TryParse(branchroute["DeliveryQty"].ToString(), out qty);
                            float qtycpy = 0;
                            float.TryParse(drtotdelivery["deliverQty"].ToString(), out qtycpy);
                            float totalqty = qty + qtycpy;
                            drtotdelivery["deliverQty"] = Math.Round(totalqty, 2);
                        }
                        else
                        {
                        }
                    }
                }
                DataView viewdue = new DataView(dtDue);
                DataTable distincttabledue = viewdue.ToTable(true, "BranchName");
                foreach (DataRow dr in distincttabledue.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    newrow["Route Name"] = dr["BranchName"].ToString();
                    double Amount = 0;
                    double total = 0;
                    double dueamt = 0;
                    double paidamt = 0;
                    foreach (DataRow branch in dtDue.Rows)
                    {
                        if (dr["BranchName"].ToString() == branch["BranchName"].ToString())
                        {
                            double DeliveryQty = 0;
                            double.TryParse(branch["DeliveryQty"].ToString(), out DeliveryQty);
                            if (DeliveryQty == 0.0)
                            {
                            }
                            else
                            {
                                newrow[branch["ProductName"].ToString()] = Math.Round(DeliveryQty, 2);
                                double qtyvalue = 0;
                                double.TryParse(branch["DeliveryQty"].ToString(), out qtyvalue);
                                double UnitCost = 0;
                                double.TryParse(branch["UnitCost"].ToString(), out UnitCost);
                                //double.TryParse(branch["AmountPaid"].ToString(), out paidamt);
                                Amount += qtyvalue * UnitCost;
                                total += qtyvalue;

                                int m = 0;
                                DataRow tempnewrow = dttempproducts.NewRow();
                                tempnewrow["ProductName"] = branch["ProductName"].ToString();
                                tempnewrow["SubCatSno"] = branch["SubCat_sno"].ToString();
                                dttempproducts.Rows.Add(tempnewrow);
                                m++;


                            }
                        }
                    }
                    newrow["Total Qty"] = total;
                    newrow["Total Amount"] = Amount;
                    //newrow["Cash Amount"] = paidamt;
                    dueamt = Amount - paidamt;
                    if (dueamt > 0)
                    {
                        newrow["Dues Amount"] = Amount - paidamt;
                    }
                    else
                    {
                        newrow["Dues Amount"] = 0;
                    }
                    Report.Rows.Add(newrow);
                    i++;
                }
                foreach (DataRow drdue in dtDue.Rows)
                {
                    foreach (DataRow drtotdeliverydue in dtalldelivery.Rows)
                    {
                        if (drdue["prodsno"].ToString() == drtotdeliverydue["sno"].ToString())
                        {
                            float qty = 0;
                            float.TryParse(drdue["DeliveryQty"].ToString(), out qty);
                            float qtycpy = 0;
                            float.TryParse(drtotdeliverydue["deliverQty"].ToString(), out qtycpy);
                            float totalqty = qty + qtycpy;
                            drtotdeliverydue["deliverQty"] = totalqty;
                        }
                        else
                        {
                        }
                    }
                }
                DataView viewhyddue = new DataView(dtHYDDue1);
                DataTable distincttableduehyd = viewhyddue.ToTable(true, "Expr1");
                foreach (DataRow dr in distincttableduehyd.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    newrow["Route Name"] = dr["Expr1"].ToString();
                    double Amount = 0;
                    double total = 0;
                    double dueamt = 0;
                    double paidamt = 0;
                    foreach (DataRow branchhyd in dtHYDDue1.Rows)
                    {
                        if (dr["Expr1"].ToString() == branchhyd["Expr1"].ToString())
                        {
                            double DeliveryQty = 0;
                            double.TryParse(branchhyd["DeliveryQty"].ToString(), out DeliveryQty);
                            if (DeliveryQty == 0.0)
                            {
                            }
                            else
                            {
                                newrow[branchhyd["ProductName"].ToString()] = Math.Round(DeliveryQty, 2);
                                double qtyvalue = 0;
                                double.TryParse(branchhyd["DeliveryQty"].ToString(), out qtyvalue);
                                double UnitCost = 0;
                                double.TryParse(branchhyd["UnitCost"].ToString(), out UnitCost);
                                //double.TryParse(branch["AmountPaid"].ToString(), out paidamt);
                                Amount += qtyvalue * UnitCost;
                                total += qtyvalue;

                                DataRow tempnewrow = dttempproducts.NewRow();
                                tempnewrow["ProductName"] = branchhyd["ProductName"].ToString();
                                tempnewrow["SubCatSno"] = branchhyd["SubCat_sno"].ToString();
                                dttempproducts.Rows.Add(tempnewrow);
                            }
                        }
                    }
                    newrow["Total Qty"] = total;
                    newrow["Total Amount"] = Amount;
                    //newrow["Cash Amount"] = paidamt;
                    dueamt = Amount - paidamt;
                    if (dueamt > 0)
                    {
                        newrow["Dues Amount"] = Amount - paidamt;
                    }
                    else
                    {
                        newrow["Dues Amount"] = 0;
                    }
                    Report.Rows.Add(newrow);
                    i++;
                }
                foreach (DataRow drhyddue in dtHYDDue1.Rows)
                {
                    foreach (DataRow drtotdeliverydue in dtalldelivery.Rows)
                    {
                        if (drhyddue["prodsno"].ToString() == drtotdeliverydue["sno"].ToString())
                        {
                            float qty = 0;
                            float.TryParse(drhyddue["DeliveryQty"].ToString(), out qty);
                            float qtycpy = 0;
                            float.TryParse(drtotdeliverydue["deliverQty"].ToString(), out qtycpy);
                            float totalqty = qty + qtycpy;
                            drtotdeliverydue["deliverQty"] = totalqty;
                        }
                        else
                        {
                        }
                    }
                }

                //cmd = new MySqlCommand("SELECT tripsubdata.Qty, productsdata.ProductName, productsdata.sno,dispatch.DispName FROM dispatch INNER JOIN  triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE (dispatch.Branch_Id = @BranchID) AND (dispatch.DispMode = 'LOCAL') AND (tripdata.I_Date BETWEEN @D1 AND @D2) GROUP BY productsdata.ProductName");
                /// By Ravi New
                cmd = new MySqlCommand("SELECT cc.ProductName, cc.ProdSno, cc.Qty, ff.DispName, ff.DispMode FROM (SELECT productsdata.sno AS ProdSno, productsdata.ProductName, tripdata.Sno, tripsubdata.Qty FROM            tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE        (tripdata.I_Date BETWEEN @d1 AND @d2)) cc INNER JOIN (SELECT I_Date, DispName, DispMode, Tripdata_sno FROM (SELECT tripdata_1.I_Date, dispatch.DispName, dispatch.DispMode, triproutes.Tripdata_sno FROM triproutes INNER JOIN tripdata tripdata_1 ON triproutes.Tripdata_sno = tripdata_1.Sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno WHERE        (tripdata_1.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) AND (dispatch.DispMode = 'FREE') OR (tripdata_1.I_Date BETWEEN @D1 AND @D2) AND (dispatch.Branch_Id = @BranchID) AND (dispatch.DispMode = 'LOCAL')) ff_1) ff ON  cc.Sno = ff.Tripdata_sno");
                //cmd = new MySqlCommand("SELECT tripsubdata.Qty, productsdata.ProductName, productsdata.sno, dispatch.DispName, dispatch.DispMode FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE (dispatch.Branch_Id = @BranchID) AND (dispatch.DispMode = 'LOCAL') AND (tripdata.I_Date BETWEEN @D1 AND @D2) OR (dispatch.Branch_Id = @BranchID) AND (dispatch.DispMode = 'FREE') AND (tripdata.I_Date BETWEEN @D1 AND @D2) GROUP BY productsdata.ProductName, dispatch.DispName");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtlOCAL = vdm.SelectQuery(cmd).Tables[0];
                DataView viewLeaks = new DataView(dtlOCAL);
                DataTable distinctLeaks = viewLeaks.ToTable(true, "DispName", "DispMode");
                foreach (DataRow drp in distinctLeaks.Rows)
                {
                    if (drp["DispMode"].ToString() == "LOCAL")
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["SNo"] = i++;
                        newrow["Route Name"] = drp["DispName"].ToString();
                        double total = 0;
                        foreach (DataRow dr in dtlOCAL.Rows)
                        {
                            if (drp["DispMode"].ToString() == "LOCAL")
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {

                                    double Qty = 0;
                                    double.TryParse(dr["Qty"].ToString(), out Qty);
                                    total += Qty;

                                    if (Qty == 0.0)
                                    {
                                    }
                                    else
                                    {
                                        newrow[dr["ProductName"].ToString()] = Qty;
                                        //DataRow tempnewrow = dttempproducts.NewRow();
                                        //tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                        //tempnewrow["SubCatSno"] = dr["tempsubcatsno"].ToString();
                                        //dttempproducts.Rows.Add(tempnewrow);
                                    }
                                }
                            }
                        }
                        newrow["Total Qty"] = total;
                        Report.Rows.Add(newrow);
                    }

                }
                foreach (DataRow branchroute in dtlOCAL.Rows)
                {
                    foreach (DataRow drtotdelivery in dtalldelivery.Rows)
                    {
                        if (branchroute["ProdSno"].ToString() == drtotdelivery["sno"].ToString())
                        {
                            float qty = 0;
                            float.TryParse(branchroute["Qty"].ToString(), out qty);
                            float qtycpy = 0;
                            float.TryParse(drtotdelivery["deliverQty"].ToString(), out qtycpy);
                            float totalqty = qty + qtycpy;
                            drtotdelivery["deliverQty"] = Math.Round(totalqty, 2);
                        }
                        else
                        {
                        }
                    }
                }
                cmd = new MySqlCommand("SELECT clotrans.BranchId,productsdata.ProductName,productsdata.SubCat_sno, closubtranprodcts.StockQty, productsdata.sno FROM clotrans INNER JOIN closubtranprodcts ON clotrans.Sno = closubtranprodcts.RefNo INNER JOIN productsdata ON closubtranprodcts.ProductID = productsdata.sno WHERE (clotrans.BranchId = @BranchID) AND (clotrans.IndDate BETWEEN @d1 AND @d2) AND (clotrans.Transaction_Type = 0) GROUP BY productsdata.ProductName");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-2)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-2)));
                DataTable dtOpp = vdm.SelectQuery(cmd).Tables[0];
                DataView viewOpp = new DataView(dtOpp);
                DataTable distinctOpp = viewOpp.ToTable(true, "BranchId");
                foreach (DataRow drp in distinctOpp.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i++;
                    newrow["Route Name"] = "Oppening Stock";
                    double total = 0;
                    foreach (DataRow dr in dtOpp.Rows)
                    {
                        if (drp["BranchId"].ToString() == dr["BranchId"].ToString())
                        {

                            double StockQty = 0;
                            double.TryParse(dr["StockQty"].ToString(), out StockQty);
                            if (StockQty == 0.0)
                            {

                            }
                            else
                            {
                                DataRow tempnewrow = dttempproducts.NewRow();
                                tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                tempnewrow["SubCatSno"] = dr["SubCat_sno"].ToString();
                                dttempproducts.Rows.Add(tempnewrow);
                                newrow[dr["ProductName"].ToString()] = StockQty;
                            }


                            total += StockQty;

                        }
                    }
                    newrow["Total Qty"] = total;
                    Report.Rows.Add(newrow);
                }
                foreach (DataRow branchroute in dtOpp.Rows)
                {
                    foreach (DataRow drtotdelivery in dtalldelivery.Rows)
                    {
                        if (branchroute["sno"].ToString() == drtotdelivery["sno"].ToString())
                        {
                            float StockQty = 0;
                            float.TryParse(branchroute["StockQty"].ToString(), out StockQty);
                            float qtycpy = 0;
                            float.TryParse(drtotdelivery["deliverQty"].ToString(), out qtycpy);
                            float totalqty = qtycpy - StockQty;
                            drtotdelivery["deliverQty"] = Math.Round(totalqty, 2);
                        }
                        else
                        {
                        }
                    }
                }
                cmd = new MySqlCommand("SELECT clotrans.BranchId,productsdata.ProductName,productsdata.SubCat_sno, closubtranprodcts.StockQty, productsdata.sno FROM clotrans INNER JOIN closubtranprodcts ON clotrans.Sno = closubtranprodcts.RefNo INNER JOIN productsdata ON closubtranprodcts.ProductID = productsdata.sno WHERE (clotrans.BranchId = @BranchID) AND (clotrans.IndDate BETWEEN @d1 AND @d2) AND (clotrans.Transaction_Type = 0) GROUP BY productsdata.ProductName");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtClo = vdm.SelectQuery(cmd).Tables[0];
                DataView viewClo = new DataView(dtClo);
                DataTable distinctClo = viewClo.ToTable(true, "BranchId");
                foreach (DataRow drp in distinctClo.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i++;
                    newrow["Route Name"] = "Closing Stock";
                    double total = 0;
                    foreach (DataRow dr in dtClo.Rows)
                    {
                        if (drp["BranchId"].ToString() == dr["BranchId"].ToString())
                        {

                            double StockQty = 0;
                            double.TryParse(dr["StockQty"].ToString(), out StockQty);
                            if (StockQty == 0.0)
                            {

                            }
                            else
                            {
                                DataRow tempnewrow = dttempproducts.NewRow();
                                tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                tempnewrow["SubCatSno"] = dr["SubCat_sno"].ToString();
                                dttempproducts.Rows.Add(tempnewrow);
                                newrow[dr["ProductName"].ToString()] = StockQty;
                            }
                            total += StockQty;
                        }
                    }
                    newrow["Total Qty"] = total;
                    Report.Rows.Add(newrow);
                }
                foreach (DataRow branchroute in dtClo.Rows)
                {
                    foreach (DataRow drtotdelivery in dtalldelivery.Rows)
                    {
                        if (branchroute["sno"].ToString() == drtotdelivery["sno"].ToString())
                        {
                            float StockQty = 0;
                            float.TryParse(branchroute["StockQty"].ToString(), out StockQty);
                            float qtycpy = 0;
                            float.TryParse(drtotdelivery["deliverQty"].ToString(), out qtycpy);
                            float totalqty = StockQty + qtycpy;
                            drtotdelivery["deliverQty"] = Math.Round(totalqty, 2);
                        }
                        else
                        {
                        }
                    }
                }




                DataTable dtproductsdata = new DataTable();
                DataTable dtAllLeaks = new DataTable();
                //cmd = new MySqlCommand("SELECT sno, DispName FROM dispatch WHERE (BranchID = @BranchID)");
                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno, dispatch.BranchID, tripdata.sno as TripSno,tripdata.I_Date FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2)  and (dispatch.DispType='SO') and (tripdata.Status<>'C')  group by tripdata.Sno ORDER BY dispatch.sno");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtoldnames = vdm.SelectQuery(cmd).Tables[0];
                DataTable dtDispnames = new DataTable();
                DataTable dtAgentDispnames = new DataTable();
                if (ddlSalesOffice.SelectedValue == "159")
                {
                    cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno, dispatch.BranchID, tripdata.I_Date,tripdata.sno as TripSno, dispatch.DispMode FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.DispType = @Agent OR dispatch.DispType = @SM) AND (dispatch.Branch_Id = @BranchID) and (tripdata.Status<>'C')  GROUP BY tripdata.sno");
                    cmd.Parameters.AddWithValue("@Agent", "AGENT");
                    cmd.Parameters.AddWithValue("@SM", "SM");
                    cmd.Parameters.AddWithValue("@BranchID", 4626);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    dtAgentDispnames = vdm.SelectQuery(cmd).Tables[0];
                }
                dtDispnames = dtoldnames.Copy();
                dtDispnames.Merge(dtAgentDispnames);
                dtAllLeaks = new DataTable();
                dtAllLeaks.Columns.Add("ShortQty");
                dtAllLeaks.Columns.Add("ProductID");
                dtAllLeaks.Columns.Add("LeakQty");
                dtAllLeaks.Columns.Add("FreeMilk");
                dtAllLeaks.Columns.Add("ReturnQty");
                dtAllLeaks.Columns.Add("VReturnQty");
                dtAllLeaks.Columns.Add("TripId");
                dtAllLeaks.Columns.Add("UnitPrice");
                foreach (DataRow drpdt in produtstbl.Rows)
                {
                    DataRow newRow = dtAllLeaks.NewRow();
                    newRow["ShortQty"] = "0";
                    newRow["ProductID"] = drpdt["Sno"].ToString();
                    newRow["FreeMilk"] = "0";
                    newRow["LeakQty"] = "0";
                    newRow["ReturnQty"] = "0";
                    newRow["VReturnQty"] = "0";
                    newRow["TripId"] = "0";
                    newRow["UnitPrice"] = drpdt["unitprice"].ToString();
                    dtAllLeaks.Rows.Add(newRow);
                }
                cmd = new MySqlCommand("SELECT branchleaktrans.EmpId, branchleaktrans.TripId, branchleaktrans.ProdId, branchleaktrans.LeakQty, branchleaktrans.DOE, branchleaktrans.BranchID,branchleaktrans.Status, branchleaktrans.FreeQty, branchleaktrans.ShortQty, tripdata.I_Date, branchproducts.Rank, productsdata.ProductName FROM branchleaktrans INNER JOIN tripdata ON branchleaktrans.TripId = tripdata.Sno INNER JOIN branchproducts ON branchleaktrans.BranchID = branchproducts.branch_sno AND branchleaktrans.ProdId = branchproducts.product_sno INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchleaktrans.BranchID = @BranchID) GROUP BY tripdata.I_Date, branchleaktrans.ProdId ORDER BY tripdata.I_Date, branchproducts.Rank");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtsalesofficeLeaks = vdm.SelectQuery(cmd).Tables[0];
                if (dtsalesofficeLeaks.Rows.Count > 0)
                {
                    DataRow newLeakages = Report.NewRow();
                    //newLeakages["Route Name"] = "PuffLeakages" + " " + Disp;
                    newLeakages["Route Name"] = "Puff_Leakages";
                    float totLeakQty = 0;
                    //float totLeakAmount = 0;
                    foreach (DataRow drNewRowLeaks in dtsalesofficeLeaks.Rows)
                    {
                        foreach (DataRow drdt in produtstbl.Rows)
                        {
                            if (drNewRowLeaks["ProdId"].ToString() == drdt["sno"].ToString())
                            {
                                float LeakQty = 0;
                                float.TryParse(drNewRowLeaks["LeakQty"].ToString(), out LeakQty);
                                newLeakages[drdt["ProductName"].ToString()] = Math.Round(LeakQty, 2);
                                float UnitCost = 0;
                                float.TryParse(drdt["unitprice"].ToString(), out UnitCost);
                                // float Total = LeakQty * UnitCost;
                                totLeakQty += LeakQty;
                                //totLeakAmount += Total;
                            }

                        }
                    }
                    newLeakages["Total Qty"] = Math.Round(totLeakQty, 2);
                    //newLeakages["Total Amount"] = Math.Round(totLeakAmount, 2);
                    if (totLeakQty > 0)
                    {
                        Report.Rows.Add(newLeakages);
                    }
                }
                if (dtDispnames.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDispnames.Rows)
                    {
                        //cmd = new MySqlCommand("SELECT tripdata.Sno, leakages.TotalLeaks,leakages.VLeaks,leakages.VReturns,leakages.ReturnQty, productsdata.ProductName, dispatch.DispName, leakages.ProductID, leakages.FreeMilk, leakages.ShortQty FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE  (dispatch.sno = @dispatchsno) AND (tripdata.I_Date BETWEEN @d1 AND @d2)");
                        ////......01/09/2016  CRREDDY..........
                        cmd = new MySqlCommand("SELECT Triproutes.Tripdata_sno, Triproutes.RouteID, ff.Sno, ff.TotalLeaks, ff.VLeaks, ff.VReturns, ff.ReturnQty, ff.ProductName, ff.ProductID, ff.FreeMilk, ff.ShortQty FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (RouteID = @dispatchSno)) Triproutes INNER JOIN (SELECT Sno, TotalLeaks, VLeaks, VReturns, ReturnQty, ProductName, ProductID, FreeMilk, ShortQty FROM (SELECT tripdata.Sno, leakages.TotalLeaks, leakages.VLeaks, leakages.VReturns, leakages.ReturnQty, productsdata.ProductName, leakages.ProductID, leakages.FreeMilk,  leakages.ShortQty FROM  leakages INNER JOIN tripdata ON leakages.TripID = tripdata.Sno INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2)) tripinfo) ff ON ff.Sno = Triproutes.Tripdata_sno");
                        cmd.Parameters.AddWithValue("@dispatchSno", dr["sno"].ToString());
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtLeakble = vdm.SelectQuery(cmd).Tables[0];
                        string TripDataSno = "0";
                        if (dtLeakble.Rows.Count > 0)
                        {
                            TripDataSno = dtLeakble.Rows[0]["Sno"].ToString();
                        }
                        else
                        {
                            TripDataSno = "0";
                        }
                        cmd = new MySqlCommand("SELECT EmpId, TripId, ProdId, LeakQty, DOE, BranchID, Status, FreeQty, ShortQty FROM branchleaktrans WHERE (TripId = @tripid)");
                        cmd.Parameters.AddWithValue("@tripid", TripDataSno);
                        DataTable dtsalesofficeshortfree = vdm.SelectQuery(cmd).Tables[0];


                        foreach (DataRow drr in dtLeakble.Rows)
                        {
                            foreach (DataRow drallleak in dtAllLeaks.Rows)
                            {
                                if (drr["ProductID"].ToString() == drallleak["ProductID"].ToString())
                                {
                                    float leakqty = 0;
                                    float leakqtycpy = 0;
                                    float shortQty = 0;
                                    float shortQtycpy = 0;
                                    float freeqty = 0;
                                    float freeqtycpy = 0;
                                    float.TryParse(drr["TotalLeaks"].ToString(), out leakqty);
                                    float.TryParse(drallleak["LeakQty"].ToString(), out leakqtycpy);
                                    float.TryParse(drr["ShortQty"].ToString(), out shortQty);
                                    float.TryParse(drallleak["ShortQty"].ToString(), out shortQtycpy);
                                    float.TryParse(drr["FreeMilk"].ToString(), out freeqty);
                                    float.TryParse(drallleak["FreeMilk"].ToString(), out freeqtycpy);

                                    drallleak["ShortQty"] = shortQty + shortQtycpy;
                                    drallleak["LeakQty"] = leakqty + leakqtycpy;
                                    drallleak["FreeMilk"] = freeqtycpy + freeqty;
                                    float retunQty = 0;
                                    float VretunQty = 0;
                                    float.TryParse(drallleak["ReturnQty"].ToString(), out retunQty);
                                    float.TryParse(drallleak["VReturnQty"].ToString(), out VretunQty);
                                    float alreturnqty = 0;
                                    float alVreturnqty = 0;
                                    float.TryParse(drr["ReturnQty"].ToString(), out alreturnqty);
                                    float.TryParse(drr["VReturns"].ToString(), out alVreturnqty);
                                    float totalreturn = 0;
                                    float totalVreturn = 0;
                                    totalreturn = retunQty + alreturnqty;
                                    totalVreturn = VretunQty + alVreturnqty;
                                    drallleak["ReturnQty"] = Math.Round(totalreturn, 2);
                                    drallleak["VReturnQty"] = Math.Round(totalVreturn, 2);
                                    drallleak["TripId"] = drr["Sno"].ToString();
                                }
                            }
                        }
                        foreach (DataRow drshortfree in dtsalesofficeshortfree.Rows)
                        {
                            foreach (DataRow drallleak in dtAllLeaks.Rows)
                            {
                                if (drshortfree["ProdId"].ToString() == drallleak["ProductID"].ToString())
                                {

                                    float shortQty = 0;
                                    float freeQty = 0;
                                    float soshortQty = 0;
                                    float sofreeQty = 0;
                                    float.TryParse(drallleak["ShortQty"].ToString(), out shortQty);
                                    float.TryParse(drallleak["FreeMilk"].ToString(), out freeQty);
                                    float.TryParse(drshortfree["ShortQty"].ToString(), out soshortQty);
                                    float.TryParse(drshortfree["FreeQty"].ToString(), out sofreeQty);
                                    float totalshort = shortQty + soshortQty;
                                    float totalfree = freeQty + sofreeQty;
                                    drallleak["ShortQty"] = Math.Round(totalshort, 2);
                                    drallleak["FreeMilk"] = Math.Round(totalfree, 2);
                                }
                            }
                        }

                        string Disp = dr["DispName"].ToString();
                        string[] strName = Disp.Split('_');
                        DataRow newLeakages = Report.NewRow();
                        newLeakages["Route Name"] = "Leakages" + " " + Disp;
                        float totLeakQty = 0;
                        float totLeakAmount = 0;
                        foreach (DataRow drNewRowLeaks in dtLeakble.Rows)
                        {
                            foreach (DataRow drdt in produtstbl.Rows)
                            {
                                if (drNewRowLeaks["ProductID"].ToString() == drdt["sno"].ToString())
                                {
                                    float LeakQty = 0;
                                    float.TryParse(drNewRowLeaks["TotalLeaks"].ToString(), out LeakQty);
                                    newLeakages[drdt["ProductName"].ToString()] = Math.Round(LeakQty, 2);
                                    float UnitCost = 0;
                                    float.TryParse(drdt["unitprice"].ToString(), out UnitCost);
                                    float Total = LeakQty * UnitCost;
                                    totLeakQty += LeakQty;
                                    totLeakAmount += Total;
                                }

                            }
                        }
                        newLeakages["Total Qty"] = Math.Round(totLeakQty, 2);
                        newLeakages["Total Amount"] = Math.Round(totLeakAmount, 2);
                        if (totLeakQty > 0)
                        {
                            Report.Rows.Add(newLeakages);
                        }
                        DataRow newVLeakages = Report.NewRow();
                        newVLeakages["Route Name"] = "VLeakages" + " " + Disp;
                        float totVLeakQty = 0;
                        float totVLeakAmount = 0;
                        foreach (DataRow drNewRowLeaks in dtLeakble.Rows)
                        {
                            foreach (DataRow drdt in produtstbl.Rows)
                            {
                                if (drNewRowLeaks["ProductID"].ToString() == drdt["sno"].ToString())
                                {
                                    float LeakQty = 0;
                                    float.TryParse(drNewRowLeaks["VLeaks"].ToString(), out LeakQty);
                                    newVLeakages[drdt["ProductName"].ToString()] = Math.Round(LeakQty, 2);
                                    float UnitCost = 0;
                                    float.TryParse(drdt["unitprice"].ToString(), out UnitCost);
                                    float Total = LeakQty * UnitCost;
                                    totVLeakQty += LeakQty;
                                    totVLeakAmount += Total;
                                }

                            }
                        }
                        // newVLeakages["Total Qty"] = Math.Round(totVLeakQty, 2);
                        // newVLeakages["Total Amount"] = Math.Round(totVLeakAmount, 2);
                        if (totVLeakQty > 0)
                        {
                            Report.Rows.Add(newVLeakages);
                        }
                    }


                    cmd = new MySqlCommand("SELECT tripdata.EmpId,DispatcherID FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.sno =@dispatchSno) AND (tripdata.I_Date BETWEEN @d1 AND @d2)");
                    cmd.Parameters.AddWithValue("@dispatchSno", dtDispnames.Rows[0]["sno"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    DataTable DtEmpId = vdm.SelectQuery(cmd).Tables[0];
                    cmd = new MySqlCommand("SELECT Sno FROM  tripdata WHERE (DEmpId = @DEmpId) AND (I_Date BETWEEN @d1 AND @d2)");
                    cmd.Parameters.AddWithValue("@DEmpId", DtEmpId.Rows[0]["EmpId"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    DataTable DtTripId = vdm.SelectQuery(cmd).Tables[0];
                    if (DtTripId.Rows.Count == 0)
                    {
                        cmd = new MySqlCommand("SELECT Sno FROM  tripdata WHERE (DEmpId = @DEmpId) AND (I_Date BETWEEN @d1 AND @d2)");
                        cmd.Parameters.AddWithValue("@DEmpId", DtEmpId.Rows[0]["DispatcherID"].ToString());
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DtTripId = vdm.SelectQuery(cmd).Tables[0];
                    }
                    DataTable DtLeaks = new DataTable();
                    foreach (DataRow drTrip in DtTripId.Rows)
                    {
                        cmd = new MySqlCommand("SELECT ShortQty, ProductID, LeakQty,ReturnQty,FreeMilk FROM leakages WHERE (TripID = @TripID) and VarifyStatus IS NULL");
                        cmd.Parameters.AddWithValue("@TripID", drTrip["Sno"].ToString());
                        DtLeaks = vdm.SelectQuery(cmd).Tables[0];
                        foreach (DataRow drLeaks in DtLeaks.Rows)
                        {
                            foreach (DataRow drNew in dtAllLeaks.Rows)
                            {
                                if (drLeaks["ProductID"].ToString() == drNew["ProductID"].ToString())
                                {
                                    float ShortQty = 0;
                                    float.TryParse(drLeaks["ShortQty"].ToString(), out ShortQty);
                                    float AllShort = 0;
                                    float.TryParse(drNew["ShortQty"].ToString(), out AllShort);
                                    float TotalShortQty = ShortQty + AllShort;
                                    drNew["ShortQty"] = TotalShortQty;
                                    float FreeMilk = 0;
                                    float.TryParse(drLeaks["FreeMilk"].ToString(), out FreeMilk);
                                    float AllFree = 0;
                                    float.TryParse(drNew["FreeMilk"].ToString(), out AllFree);
                                    float TotalFreeQty = FreeMilk + AllFree;
                                    drNew["FreeMilk"] = TotalFreeQty;
                                    float ReturnMilk = 0;
                                    float.TryParse(drLeaks["ReturnQty"].ToString(), out ReturnMilk);
                                    float AllReturn = 0;
                                    float.TryParse(drNew["ReturnQty"].ToString(), out AllReturn);
                                    float TotalReturnQty = ReturnMilk + AllReturn;
                                    drNew["ReturnQty"] = TotalReturnQty;
                                }
                            }
                        }
                    }
                }
                DataRow totalschememilk = Report.NewRow();
                totalschememilk["Route Name"] = "SCHEME MILK";
                float totOfferFreeQty = 0;
                float totOfferFreeAmount = 0;
                foreach (DataRow drOfferfree in dt_offertble.Rows)
                {
                    foreach (DataRow drdt in produtstbl.Rows)
                    {
                        if (drOfferfree["ProductID"].ToString() == drdt["sno"].ToString())
                        {
                            float freeQty = 0;
                            float.TryParse(drOfferfree["Delqty"].ToString(), out freeQty);
                            totalschememilk[drdt["ProductName"].ToString()] = Math.Round(freeQty, 2);
                            float UnitCost = 0;
                            float.TryParse(drOfferfree["unit_price"].ToString(), out UnitCost);
                            float Total = freeQty * UnitCost;
                            totOfferFreeQty += freeQty;
                            totOfferFreeAmount += Total;
                        }
                    }
                }
                totalschememilk["Total Qty"] = Math.Round(totOfferFreeQty, 2);
                totalschememilk["Total Amount"] = Math.Round(totOfferFreeAmount, 2);
                if (totOfferFreeQty > 0)
                {
                    Report.Rows.Add(totalschememilk);
                }

                DataRow totalfreemilk = Report.NewRow();
                totalfreemilk["Route Name"] = "FREE MILK";
                float totFreeQty = 0;
                float totFreeAmount = 0;
                foreach (DataRow drNewRowfree in dtAllLeaks.Rows)
                {
                    foreach (DataRow drdt in produtstbl.Rows)
                    {
                        if (drNewRowfree["ProductID"].ToString() == drdt["sno"].ToString())
                        {
                            float freeQty = 0;
                            float.TryParse(drNewRowfree["FreeMilk"].ToString(), out freeQty);
                            if (freeQty == 0.0)
                            {
                            }
                            else
                            {
                                totalfreemilk[drdt["ProductName"].ToString()] = Math.Round(freeQty, 2);
                                float UnitCost = 0;
                                float.TryParse(drNewRowfree["UnitPrice"].ToString(), out UnitCost);
                                float Total = freeQty * UnitCost;
                                totFreeQty += freeQty;
                                totFreeAmount += Total;
                            }
                        }
                    }
                }
                totalfreemilk["Total Qty"] = Math.Round(totFreeQty, 2);
                totalfreemilk["Total Amount"] = Math.Round(totFreeAmount, 2);
                if (totFreeQty > 0)
                {
                    Report.Rows.Add(totalfreemilk);
                }
                foreach (DataRow drdelivery in dtalldelivery.Rows)
                {
                    foreach (DataRow drfree in dtAllLeaks.Rows)
                    {
                        if (drdelivery["sno"].ToString() == drfree["ProductID"].ToString())
                        {
                            float freeQty = 0;
                            float.TryParse(drfree["FreeMilk"].ToString(), out freeQty);
                            drdelivery["freeQty"] = Math.Round(freeQty, 2);
                        }
                    }
                }

                foreach (DataRow drdelivery in dtalldelivery.Rows)
                {
                    foreach (DataRow drfree in dt_offertble.Rows)
                    {
                        if (drdelivery["sno"].ToString() == drfree["ProductID"].ToString())
                        {
                            float freeQty = 0;
                            float PrevfreeQty = 0;
                            float totfreeQty = 0;
                            float.TryParse(drfree["Delqty"].ToString(), out freeQty);
                            //float.TryParse(drdelivery["freeQty"].ToString(), out PrevfreeQty);
                            //totfreeQty = freeQty + PrevfreeQty;
                            totfreeQty = freeQty;
                            drdelivery["scheme"] = Math.Round(totfreeQty, 2);
                        }
                    }
                }

                foreach (DataRow drp in distinctLeaks.Rows)
                {
                    if (drp["DispMode"].ToString() == "Free")
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["SNo"] = i++;
                        newrow["Route Name"] = drp["DispName"].ToString();
                        double total = 0;
                        foreach (DataRow dr in dtlOCAL.Rows)
                        {
                            if (dr["DispMode"].ToString() == "Free")
                            {
                                if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                {

                                    double Qty = 0;
                                    double.TryParse(dr["Qty"].ToString(), out Qty);
                                    newrow[dr["ProductName"].ToString()] = Qty;
                                    total += Qty;
                                }
                            }


                        }
                        newrow["Total Qty"] = total;
                        Report.Rows.Add(newrow);
                    }

                }
                DataRow totalshortmilk = Report.NewRow();
                totalshortmilk["Route Name"] = "SHORT MILK";
                float totshortQty = 0;
                float totshortAmount = 0;
                foreach (DataRow drNewRowshort in dtAllLeaks.Rows)
                {
                    foreach (DataRow drdt in produtstbl.Rows)
                    {
                        if (drNewRowshort["ProductID"].ToString() == drdt["sno"].ToString())
                        {
                            float shortQty = 0;
                            float.TryParse(drNewRowshort["ShortQty"].ToString(), out shortQty);
                            if (shortQty == 0.0)
                            {
                            }
                            else
                            {
                                totalshortmilk[drdt["ProductName"].ToString()] = Math.Round(shortQty, 2);
                                float UnitCost = 0;
                                float.TryParse(drNewRowshort["UnitPrice"].ToString(), out UnitCost);
                                float Total = shortQty * UnitCost;
                                totshortQty += shortQty;
                                totshortAmount += Total;
                            }
                        }
                    }
                }
                totalshortmilk["Total Qty"] = Math.Round(totshortQty, 2);
                totalshortmilk["Total Amount"] = Math.Round(totshortAmount, 2);
                if (totshortQty > 0)
                {
                    Report.Rows.Add(totalshortmilk);
                }
                foreach (DataRow drdelivery in dtalldelivery.Rows)
                {

                    foreach (DataRow drshort in dtAllLeaks.Rows)
                    {
                        if (drdelivery["sno"].ToString() == drshort["ProductID"].ToString())
                        {
                            float shortQty = 0;
                            float.TryParse(drshort["ShortQty"].ToString(), out shortQty);
                            drdelivery["shortQty"] = Math.Round(shortQty, 2);
                        }
                    }
                }
                DataRow totalreturnmilk = Report.NewRow();
                totalreturnmilk["Route Name"] = "RETURN MILK";

                float totreturnQty = 0;
                float totreturnAmount = 0;
                foreach (DataRow drNewRowreturn in dtAllLeaks.Rows)
                {
                    foreach (DataRow drdt in produtstbl.Rows)
                    {
                        if (drNewRowreturn["ProductID"].ToString() == drdt["sno"].ToString())
                        {
                            float returnQty = 0;
                            float.TryParse(drNewRowreturn["ReturnQty"].ToString(), out returnQty);
                            if (returnQty == 0.0)
                            {
                            }
                            else
                            {
                                totalreturnmilk[drdt["ProductName"].ToString()] = Math.Round(returnQty, 2);
                                float UnitCost = 0;
                                float.TryParse(drNewRowreturn["UnitPrice"].ToString(), out UnitCost);
                                float Total = returnQty * UnitCost;
                                totreturnQty += returnQty;
                                totreturnAmount += Total;
                            }
                        }
                    }
                }
                totalreturnmilk["Total Qty"] = Math.Round(totreturnQty, 2);
                totalreturnmilk["Total Amount"] = Math.Round(totreturnAmount, 2);
                if (totreturnQty > 0)
                {
                    Report.Rows.Add(totalreturnmilk);
                }
                DataRow newVReturns = Report.NewRow();
                newVReturns["Route Name"] = "VReturns";
                float totVReturnQty = 0;
                float totVReturnAmount = 0;
                foreach (DataRow drNewRowLeaks in dtAllLeaks.Rows)
                {
                    foreach (DataRow drdt in produtstbl.Rows)
                    {
                        if (drNewRowLeaks["ProductID"].ToString() == drdt["sno"].ToString())
                        {
                            float LeakQty = 0;
                            float.TryParse(drNewRowLeaks["VReturnQty"].ToString(), out LeakQty);
                            if (LeakQty == 0.0)
                            {
                            }
                            else
                            {
                                newVReturns[drdt["ProductName"].ToString()] = Math.Round(LeakQty, 2);
                                float UnitCost = 0;
                                float.TryParse(drdt["unitprice"].ToString(), out UnitCost);
                                float Total = LeakQty * UnitCost;
                                totVReturnQty += LeakQty;
                                totVReturnAmount += Total;
                            }
                        }

                    }
                }
                newVReturns["Total Qty"] = Math.Round(totVReturnQty, 2);
                //newVReturns["Total Amount"] = Math.Round(totVReturnAmount, 2);
                if (totVReturnQty > 0)
                {
                    Report.Rows.Add(newVReturns);
                }
                foreach (DataRow drdelivery in dtalldelivery.Rows)
                {
                    foreach (DataRow drreturn in dtAllLeaks.Rows)
                    {
                        if (drdelivery["sno"].ToString() == drreturn["ProductID"].ToString())
                        {
                            float returnQty = 0;
                            float.TryParse(drreturn["ReturnQty"].ToString(), out returnQty);
                            drdelivery["returnQty"] = Math.Round(returnQty, 2);
                        }
                    }
                }

                //new total
                DataRow newvartical1 = Report.NewRow();
                newvartical1["Route Name"] = "TOTAL";
                foreach (DataRow drtotaldel in dtAllLeaks.Rows)
                {
                    foreach (DataRow drdelivery in dtalldelivery.Rows)
                    {
                        if (drtotaldel["ProductID"].ToString() == drdelivery["sno"].ToString())
                        {
                            float qtycpy = 0;
                            float shortqty = 0;
                            float returnqty = 0;
                            float freeqty = 0;
                            float qtydelivercpy = 0;
                            float scheme = 0;
                            float.TryParse(drtotaldel["LeakQty"].ToString(), out qtycpy);
                            float.TryParse(drtotaldel["ShortQty"].ToString(), out shortqty);
                            float.TryParse(drtotaldel["ReturnQty"].ToString(), out returnqty);
                            float.TryParse(drtotaldel["FreeMilk"].ToString(), out freeqty);
                            float.TryParse(drdelivery["deliverQty"].ToString(), out qtydelivercpy);
                            float.TryParse(drdelivery["scheme"].ToString(), out scheme);

                            float totdelivery = 0;
                            totdelivery = qtycpy + qtydelivercpy + shortqty + returnqty + freeqty + scheme;
                            if (totdelivery == 0.0)
                            {
                            }
                            else
                            {
                                newvartical1[drdelivery["ProductName"].ToString()] = Math.Round(totdelivery, 2);
                            }
                        }
                        else
                        {
                        }
                    }
                }
                double val = 0.0;
                foreach (DataColumn dc in Report.Columns)
                {
                    if (dc.DataType == typeof(Double))
                    {
                        if (dc.ColumnName != "Receipt No")
                        {
                            val = 0.0;
                            double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                            if (val == 0.0)
                            {
                            }
                            else
                            {
                                newvartical1[dc.ToString()] = val;
                            }
                        }
                    }
                }
                Report.Rows.Add(newvartical1);
                DataRow Break2 = Report.NewRow();
                Break2["Route Name"] = "RECEIVED MILK";
                Report.Rows.Add(Break2);
                foreach (DataRow drSub in dtDispnames.Rows)
                {
                    //cmd = new MySqlCommand("SELECT triproutes.Tripdata_sno, tripsubdata.ProductId, tripsubdata.Qty FROM tripdata INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (triproutes.RouteID = @dispatchSno)");
                    /////01/09/2016 CRREDDY.....................

                    cmd = new MySqlCommand("SELECT ff.TripID, Triproutes.RouteID, ff.Qty, ff.ProductId, Triproutes.Tripdata_sno FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (Tripdata_sno = @Tripdata_sno)) Triproutes INNER JOIN (SELECT TripID, Qty, ProductId FROM (SELECT tripdata.Sno AS TripID, tripsubdata.Qty, tripsubdata.ProductId FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno WHERE (tripdata.I_Date BETWEEN @starttime AND @endtime)) tripinfo) ff ON ff.TripID = Triproutes.Tripdata_sno");
                    //cmd = new MySqlCommand("SELECT ff.TripID, Triproutes.RouteID, ff.Qty, ff.ProductId, Triproutes.Tripdata_sno FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (RouteID = @dispatchSno)) Triproutes INNER JOIN (SELECT TripID, Qty, ProductId FROM (SELECT tripdata.Sno AS TripID, tripsubdata.Qty, tripsubdata.ProductId FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno WHERE (tripdata.I_Date BETWEEN @starttime AND @endtime)) tripinfo) ff ON ff.TripID = Triproutes.Tripdata_sno");
                    cmd.Parameters.AddWithValue("@Tripdata_sno", drSub["TripSno"].ToString());
                    cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
                    DataTable DtTripSubData = vdm.SelectQuery(cmd).Tables[0];
                    string Disp = drSub["DispName"].ToString();
                    string[] strName = Disp.Split('_');
                    DataRow newSo = Report.NewRow();
                    newSo["Route Name"] = strName[0];
                    foreach (DataRow drTripSub in DtTripSubData.Rows)
                    {
                        foreach (DataRow drdt in produtstbl.Rows)
                        {
                            if (drTripSub["ProductId"].ToString() == drdt["Sno"].ToString())
                            {
                                float Qty = 0;
                                float.TryParse(drTripSub["Qty"].ToString(), out Qty);
                                if (Qty == 0.0)
                                {
                                }
                                else
                                {
                                    newSo[drdt["ProductName"].ToString()] = drTripSub["Qty"].ToString();
                                    DataRow tempnewrow = dttempproducts.NewRow();
                                    tempnewrow["ProductName"] = drdt["ProductName"].ToString();
                                    tempnewrow["SubCatSno"] = drdt["SubCat_sno"].ToString();
                                    dttempproducts.Rows.Add(tempnewrow);
                                }
                            }
                        }
                        foreach (DataRow dralldispqty in dtalldispatch.Rows)
                        {
                            if (drTripSub["ProductId"].ToString() == dralldispqty["sno"].ToString())
                            {
                                float qty = 0;
                                float.TryParse(drTripSub["Qty"].ToString(), out qty);
                                float qtycpy = 0;
                                float.TryParse(dralldispqty["TotalQty"].ToString(), out qtycpy);
                                float totalqty = qty + qtycpy;
                                dralldispqty["TotalQty"] = totalqty;
                            }
                            else
                            {
                            }
                        }
                    }
                    Report.Rows.Add(newSo);
                }

                DataRow totaldispatch = Report.NewRow();
                totaldispatch["Route Name"] = "TOTAL";
                DataRow TotalRow = Report.NewRow();
                TotalRow["Route Name"] = "DIFFERENCE";
                foreach (DataRow dr in produtstbl.Rows)
                {
                    foreach (DataRow dralldispqty in dtalldispatch.Rows)
                    {
                        if (dr["Sno"].ToString() == dralldispqty["sno"].ToString())
                        {
                            float qtycpy = 0;
                            float.TryParse(dralldispqty["TotalQty"].ToString(), out qtycpy);
                            float totalqty = 0;
                            totalqty = qtycpy;
                            if (totalqty == 0.0)
                            {
                            }
                            else
                            {
                                totaldispatch[dr["ProductName"].ToString()] = Math.Round(totalqty, 2);

                                DataRow tempnewrow = dttempproducts.NewRow();
                                tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                tempnewrow["SubCatSno"] = dr["SubCat_sno"].ToString();
                                dttempproducts.Rows.Add(tempnewrow);
                            }
                        }
                        else
                        {
                        }
                    }
                }
                Report.Rows.Add(totaldispatch);




                DataView SubCatview = new DataView(dttempproducts);
                dtSubCatgory = SubCatview.ToTable(true, "SubCatSno");
                DataView dv1 = dtSubCatgory.DefaultView;
                dv1.Sort = "SubCatSno ASC";
                dtSortedSubCategory = dv1.ToTable();


                //cmd = new MySqlCommand("SELECT dispatch.BranchID, leakages.ShortQty as short, leakages.ProductID, leakages.FreeMilk AS free, tripdata.I_Date, leakages.TotalLeaks AS totleak, leakages.TripID FROM leakages INNER JOIN tripdata ON leakages.TripID = tripdata.Sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno WHERE (dispatch.BranchID = @branchid) AND (tripdata.I_Date BETWEEN @d1 AND @d2) ");
                /// Ravi
                cmd = new MySqlCommand("SELECT Leaks.short, Leaks.ProductID, Leaks.free, Leaks.I_Date, Leaks.totleak, Leaks.TripID, Leaks.BranchID, Leaks.Sno FROM (SELECT leakages.ShortQty AS short, leakages.ProductID, leakages.FreeMilk AS free, tripdata.I_Date, leakages.TotalLeaks AS totleak, leakages.TripID, tripdata.Sno, tripdata.BranchID FROM leakages INNER JOIN tripdata ON leakages.TripID = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno FROM  (SELECT dispatch.DispName, tripdata_1.Sno FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata tripdata_1 ON triproutes.Tripdata_sno = tripdata_1.Sno WHERE (dispatch.BranchID = @branchid) AND (tripdata_1.I_Date BETWEEN @d1 AND @d2)) dd) ff ON ff.Sno = Leaks.Sno");
                cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dttotsaleleak = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow drsaleleak in dttotsaleleak.Rows)
                {
                    foreach (DataRow drdelivery in dtalldelivery.Rows)
                    {

                        if (drsaleleak["ProductID"].ToString() == drdelivery["sno"].ToString())
                        {
                            float leakqty = 0;
                            float.TryParse(drdelivery["leakQty"].ToString(), out leakqty);
                            float leakcpy = 0;
                            float.TryParse(drsaleleak["totleak"].ToString(), out leakcpy);
                            float totalleakqty = leakqty + leakcpy;
                            drdelivery["leakQty"] = totalleakqty;
                        }
                        else
                        {
                        }
                    }
                }
                foreach (var column in Report.Columns.Cast<DataColumn>().ToArray())
                {
                    if (Report.AsEnumerable().All(dr => dr.IsNull(column)))
                        Report.Columns.Remove(column);
                }
                foreach (DataRow dralldispqty in dtalldispatch.Rows)
                {
                    foreach (DataRow drdelivery in dtalldelivery.Rows)
                    {
                        if (dralldispqty["Sno"].ToString() == drdelivery["sno"].ToString())
                        {
                            float qtycpy = 0;
                            float qtydelivercpy = 0;
                            float freeQty = 0;
                            float leakQty = 0;
                            float shortQty = 0;
                            float returnQty = 0;
                            float scheme = 0;
                            float.TryParse(dralldispqty["TotalQty"].ToString(), out qtycpy);
                            float.TryParse(drdelivery["deliverQty"].ToString(), out qtydelivercpy);
                            float.TryParse(drdelivery["freeQty"].ToString(), out freeQty);
                            float.TryParse(drdelivery["leakQty"].ToString(), out leakQty);
                            float.TryParse(drdelivery["shortQty"].ToString(), out shortQty);
                            float.TryParse(drdelivery["returnQty"].ToString(), out returnQty);
                            float.TryParse(drdelivery["scheme"].ToString(), out scheme);
                            float totdelivery = 0;
                            float totdifference = 0;
                            totdelivery = freeQty + leakQty + shortQty + returnQty + qtydelivercpy + scheme;
                            totdifference = qtycpy - totdelivery;
                            if (qtycpy == 0.0)
                            {
                            }
                            else
                            {
                                TotalRow[dralldispqty["ProductName"].ToString()] = Math.Round(totdifference, 2);
                            }
                        }
                        else
                        {
                        }
                    }
                }
                Report.Rows.Add(TotalRow);
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
                    if (dgvr.Cells[1].Text.Contains("Leakages") && !dgvr.Cells[1].Text.Contains("VLeakages"))
                    {
                        GridViewRow compare = grdReports.Rows[ii + 1];

                        for (int rowcnt = 2; rowcnt < dgvr.Cells.Count; rowcnt++)
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
                    if (dgvr.Cells[1].Text.Contains("DIFFERENCE"))
                    {

                        for (int rowcnt = 2; rowcnt < dgvr.Cells.Count; rowcnt++)
                        {
                            int diff = 0;
                            int.TryParse(dgvr.Cells[rowcnt].Text, out diff);
                            if (diff > 0 || diff < 0)
                            {
                                dgvr.Cells[rowcnt].BackColor = Color.SandyBrown;
                            }

                        }


                    }
                }
                Session["xportdata"] = Report;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdReports.DataSource = Report;
            grdReports.DataBind();
        }
    }
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
                SalesOfficeID = "158";
            }
            //cmd = new MySqlCommand("SELECT SUM(indents_subtable.DeliveryQty) AS DeliveryQty, indents_subtable.UnitCost, branchdata.BranchName,branchdata.sno, productsdata.sno AS prodsno, productsdata.ProductName FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN indents ON branchdata.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (branchroutes.BranchID = @BranchID) AND (branchdata.CollectionType <> 'DUE') AND (indents.I_date BETWEEN @starttime AND @endtime) GROUP BY prodsno, branchdata.sno ORDER BY branchdata.sno, prodsno");
            cmd = new MySqlCommand("SELECT  SUM(indents_subtable.DeliveryQty) AS DeliveryQty,productsdata.SubCat_sno, indents_subtable.UnitCost, branchdata.BranchName, branchdata.sno, productsdata.sno AS prodsno, productsdata.ProductName FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (modifiedroutes.BranchID = @BranchID) AND (branchdata.CollectionType <> 'DUE') AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (modifiedroutes.BranchID = @BranchID) AND (branchdata.CollectionType <> 'DUE') AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY prodsno, branchdata.sno ORDER BY branchdata.sno, prodsno");
            cmd.Parameters.AddWithValue("@BranchID", SalesOfficeID);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            if (dtble.Rows.Count > 0)
            {

                cmd = new MySqlCommand("SELECT branchproducts.Rank,productsdata.ProductName,productsdata.sno, products_category.Categoryname, productsdata.Units, productsdata.Qty,branchproducts.unitprice FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) ORDER BY branchproducts.Rank");
                cmd.Parameters.AddWithValue("@BranchID", SalesOfficeID);
                DataTable dttable = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT    productsdata.SubCat_sno,branchproducts.unitprice,branchproducts.branch_sno,products_subcategory.tempsub_catsno AS SubCatSno, products_category.description AS Categoryname, branchproducts.product_sno AS sno, productsdata.ProductName, branchproducts.Rank,products_subcategory.description AS SubCategoryName FROM  products_category INNER JOIN products_subcategory ON products_category.sno = products_subcategory.category_sno INNER JOIN productsdata ON products_subcategory.sno = productsdata.SubCat_sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno WHERE (branchproducts.branch_sno = @BranchId) AND (branchproducts.flag = @flag) ORDER BY products_subcategory.tempsub_catsno, branchproducts.Rank");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@flag", "1");
                produtstbl1 = vdm.SelectQuery(cmd).Tables[0];

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
                if (sortedProductDT.Rows.Count > 0)
                {
                    DataView view = new DataView(dtble);
                    Report = new DataTable();
                    Report.Columns.Add("SNo");
                    Report.Columns.Add("DC NO");
                    Report.Columns.Add("Route Name");
                    foreach (DataRow dr in sortedProductDT.Rows)
                    {
                        Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                    }
                    Report.Columns.Add("Total Qty").DataType = typeof(Double);
                    Report.Columns.Add("Total Amount").DataType = typeof(Double);
                    Report.Columns.Add("Dues Amount").DataType = typeof(Double);
                    Report.Columns.Add("Cash Amount").DataType = typeof(Double);
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
                    foreach (DataRow branch in distincttable.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["SNo"] = i;
                        string bn = branch["BranchName"].ToString();
                        if (bn == "Gunnala Praveen -Garepalli Distb")
                        {

                        }
                        newrow["Route Name"] = branch["BranchName"].ToString();
                        double total = 0;
                        double Amount = 0;
                        foreach (DataRow dr in dtble.Rows)
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
                        cmd = new MySqlCommand("SELECT DcNo, BranchID, IndDate FROM agentdc WHERE (IndDate BETWEEN @d1 AND @d2) and branchid=@brncid");
                        cmd.Parameters.AddWithValue("@brncid", branch["sno"].ToString());
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtagentdcno = vdm.SelectQuery(cmd).Tables[0];
                        string dcno = "";
                        if (dtagentdcno.Rows.Count > 0)
                        {
                            dcno = dtagentdcno.Rows[0]["DcNo"].ToString();
                            newrow["DC NO"] = dcno;
                        }
                        else
                        {
                            dcno = "";
                            newrow["DC NO"] = dcno;
                        }
                        newrow["Total Qty"] = Math.Round(total, 2);
                        grnd_tot_qty += total;
                        newrow["Total Amount"] = Math.Round(Amount, 2);
                        grnd_tot_amount += Amount;
                        newrow["Cash Amount"] = Math.Round(Amount, 2);
                        grnd_tot_cashamount += Amount;
                        Report.Rows.Add(newrow);
                        i++;
                    }
                    foreach (DataRow branchroute in dtble.Rows)
                    {
                        foreach (DataRow drtotdelivery in dtalldelivery.Rows)
                        {
                            if (branchroute["prodsno"].ToString() == drtotdelivery["sno"].ToString())
                            {

                                float qty = 0;
                                float.TryParse(branchroute["DeliveryQty"].ToString(), out qty);
                                float qtycpy = 0;
                                float.TryParse(drtotdelivery["deliverQty"].ToString(), out qtycpy);
                                float totalqty = qty + qtycpy;
                                drtotdelivery["deliverQty"] = totalqty;
                            }
                            else
                            {
                            }
                        }
                    }
                    DataView viewdue = new DataView(dtDue);
                    DataTable distincttabledue = viewdue.ToTable(true, "BranchName");
                    foreach (DataRow dr in distincttabledue.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["SNo"] = i;
                        newrow["Route Name"] = dr["BranchName"].ToString();
                        double Amount = 0;
                        double total = 0;
                        foreach (DataRow branch in dtDue.Rows)
                        {
                            if (dr["BranchName"].ToString() == branch["BranchName"].ToString())
                            {
                                double DeliveryQty = 0;
                                double.TryParse(branch["DeliveryQty"].ToString(), out DeliveryQty);
                                if (DeliveryQty == 0.0)
                                {
                                }
                                else
                                {
                                    DataRow tempnewrow = dttempproducts.NewRow();
                                    tempnewrow["ProductName"] = branch["ProductName"].ToString();
                                    tempnewrow["SubCatSno"] = branch["SubCat_sno"].ToString();
                                    dttempproducts.Rows.Add(tempnewrow);
                                    newrow[branch["ProductName"].ToString()] = DeliveryQty;
                                }


                                double qtyvalue = 0;
                                double.TryParse(branch["DeliveryQty"].ToString(), out qtyvalue);
                                double UnitCost = 0;
                                double.TryParse(branch["UnitCost"].ToString(), out UnitCost);
                                Amount += qtyvalue * UnitCost;
                                total += qtyvalue;
                            }
                        }
                        newrow["Total Qty"] = Math.Round(total, 2);
                        grnd_tot_qty += total;
                        newrow["Total Amount"] = Math.Round(Amount, 2);
                        grnd_tot_amount += Amount;
                        newrow["Dues Amount"] = Math.Round(Amount, 2);
                        grnd_tot_dueamount += Amount;
                        Report.Rows.Add(newrow);
                        i++;
                    }
                    foreach (DataRow drdue in dtDue.Rows)
                    {
                        foreach (DataRow drtotdeliverydue in dtalldelivery.Rows)
                        {
                            if (drdue["prodsno"].ToString() == drtotdeliverydue["sno"].ToString())
                            {
                                float qty = 0;
                                float.TryParse(drdue["DeliveryQty"].ToString(), out qty);
                                float qtycpy = 0;
                                float.TryParse(drtotdeliverydue["deliverQty"].ToString(), out qtycpy);
                                float totalqty = qty + qtycpy;
                                drtotdeliverydue["deliverQty"] = totalqty;
                            }
                            else
                            {
                            }
                        }
                    }
                    /// By Ravi New
                    cmd = new MySqlCommand("SELECT cc.ProductName, cc.ProdSno, cc.Qty, ff.DispName, ff.DispMode FROM (SELECT productsdata.sno AS ProdSno, productsdata.ProductName, tripdata.Sno, tripsubdata.Qty FROM            tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE        (tripdata.I_Date BETWEEN @d1 AND @d2)) cc INNER JOIN (SELECT I_Date, DispName, DispMode, Tripdata_sno FROM (SELECT tripdata_1.I_Date, dispatch.DispName, dispatch.DispMode, triproutes.Tripdata_sno FROM triproutes INNER JOIN tripdata tripdata_1 ON triproutes.Tripdata_sno = tripdata_1.Sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno WHERE        (tripdata_1.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) AND (dispatch.DispMode = 'FREE') OR (tripdata_1.I_Date BETWEEN @D1 AND @D2) AND (dispatch.Branch_Id = @BranchID) AND (dispatch.DispMode = 'LOCAL')) ff_1) ff ON  cc.Sno = ff.Tripdata_sno");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    DataTable dtlOCAL = vdm.SelectQuery(cmd).Tables[0];
                    DataView viewLeaks = new DataView(dtlOCAL);
                    DataTable distinctLeaks = viewLeaks.ToTable(true, "DispName", "DispMode");
                    foreach (DataRow drp in distinctLeaks.Rows)
                    {
                        if (drp["DispMode"].ToString() == "LOCAL")
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["SNo"] = i++;
                            newrow["Route Name"] = drp["DispName"].ToString();
                            double total = 0;
                            foreach (DataRow dr in dtlOCAL.Rows)
                            {
                                if (drp["DispMode"].ToString() == "LOCAL")
                                {
                                    if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                    {

                                        double Qty = 0;
                                        double.TryParse(dr["Qty"].ToString(), out Qty);
                                        newrow[dr["ProductName"].ToString()] = Qty;
                                        total += Qty;
                                    }
                                }
                            }
                            newrow["Total Qty"] = total;
                            grnd_tot_qty += total;
                            Report.Rows.Add(newrow);
                        }
                    }
                    foreach (DataRow branchroute in dtlOCAL.Rows)
                    {
                        foreach (DataRow drtotdelivery in dtalldelivery.Rows)
                        {
                            if (branchroute["ProdSno"].ToString() == drtotdelivery["sno"].ToString())
                            {
                                float qty = 0;
                                float.TryParse(branchroute["Qty"].ToString(), out qty);
                                float qtycpy = 0;
                                float.TryParse(drtotdelivery["deliverQty"].ToString(), out qtycpy);
                                float totalqty = qty + qtycpy;
                                drtotdelivery["deliverQty"] = Math.Round(totalqty, 2);
                            }
                            else
                            {
                            }
                        }
                    }
                    cmd = new MySqlCommand("SELECT clotrans.BranchId,productsdata.ProductName,productsdata.SubCat_sno, closubtranprodcts.StockQty, productsdata.sno FROM clotrans INNER JOIN closubtranprodcts ON clotrans.Sno = closubtranprodcts.RefNo INNER JOIN productsdata ON closubtranprodcts.ProductID = productsdata.sno WHERE (clotrans.BranchId = @BranchID) AND (clotrans.IndDate BETWEEN @d1 AND @d2) AND (clotrans.Transaction_Type = 0) GROUP BY productsdata.ProductName");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-2)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-2)));
                    DataTable dtOpp = vdm.SelectQuery(cmd).Tables[0];
                    DataView viewOpp = new DataView(dtOpp);
                    DataTable distinctOpp = viewOpp.ToTable(true, "BranchId");
                    foreach (DataRow drp in distinctOpp.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["SNo"] = i++;
                        newrow["Route Name"] = "Oppening Stock";
                        double total = 0;
                        foreach (DataRow dr in dtOpp.Rows)
                        {
                            if (drp["BranchId"].ToString() == dr["BranchId"].ToString())
                            {

                                double StockQty = 0;
                                double.TryParse(dr["StockQty"].ToString(), out StockQty);

                                if (StockQty == 0.0)
                                {
                                }
                                else
                                {
                                    DataRow tempnewrow = dttempproducts.NewRow();
                                    tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                    tempnewrow["SubCatSno"] = dr["SubCat_sno"].ToString();
                                    dttempproducts.Rows.Add(tempnewrow);
                                    newrow[dr["ProductName"].ToString()] = StockQty;
                                }

                                total += StockQty;
                            }
                        }
                        newrow["Total Qty"] = total;
                        Report.Rows.Add(newrow);
                    }

                    cmd = new MySqlCommand("SELECT clotrans.BranchId,productsdata.ProductName, productsdata.SubCat_sno,closubtranprodcts.StockQty, productsdata.sno FROM clotrans INNER JOIN closubtranprodcts ON clotrans.Sno = closubtranprodcts.RefNo INNER JOIN productsdata ON closubtranprodcts.ProductID = productsdata.sno WHERE (clotrans.BranchId = @BranchID) AND (clotrans.IndDate BETWEEN @d1 AND @d2) AND (clotrans.Transaction_Type = 0) GROUP BY productsdata.ProductName");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    DataTable dtClo = vdm.SelectQuery(cmd).Tables[0];
                    DataView viewClo = new DataView(dtClo);
                    DataTable distinctClo = viewClo.ToTable(true, "BranchId");
                    foreach (DataRow drp in distinctClo.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["SNo"] = i++;
                        newrow["Route Name"] = "Closing Stock";
                        double total = 0;
                        foreach (DataRow dr in dtClo.Rows)
                        {
                            if (drp["BranchId"].ToString() == dr["BranchId"].ToString())
                            {

                                double StockQty = 0;
                                double.TryParse(dr["StockQty"].ToString(), out StockQty);

                                if (StockQty == 0.0)
                                {
                                }
                                else
                                {
                                    DataRow tempnewrow = dttempproducts.NewRow();
                                    tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                    tempnewrow["SubCatSno"] = dr["SubCat_sno"].ToString();
                                    dttempproducts.Rows.Add(tempnewrow);
                                    newrow[dr["ProductName"].ToString()] = StockQty;
                                }
                                total += StockQty;
                            }
                        }
                        newrow["Total Qty"] = total;
                        Report.Rows.Add(newrow);
                    }

                    DataTable dtproductsdata = new DataTable();
                    DataTable dtAllLeaks = new DataTable();
                    dtAllLeaks = new DataTable();
                    dtAllLeaks.Columns.Add("ShortQty");
                    dtAllLeaks.Columns.Add("ProductID");
                    dtAllLeaks.Columns.Add("LeakQty");
                    dtAllLeaks.Columns.Add("FreeMilk");
                    dtAllLeaks.Columns.Add("ReturnQty");
                    dtAllLeaks.Columns.Add("VReturnQty");

                    dtAllLeaks.Columns.Add("TripId");
                    dtAllLeaks.Columns.Add("UnitPrice");
                    foreach (DataRow drpdt in produtstbl.Rows)
                    {
                        DataRow newRow = dtAllLeaks.NewRow();
                        newRow["ShortQty"] = "0";
                        newRow["ProductID"] = drpdt["Sno"].ToString();
                        newRow["FreeMilk"] = "0";
                        newRow["LeakQty"] = "0";
                        newRow["ReturnQty"] = "0";
                        newRow["VReturnQty"] = "0";
                        newRow["TripId"] = "0";
                        newRow["UnitPrice"] = drpdt["unitprice"].ToString();
                        dtAllLeaks.Rows.Add(newRow);
                    }




                    //Akbar
                    cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno, dispatch.BranchID, tripdata.I_Date,tripdata.sno as TripSno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN branchmappingtable ON dispatch.BranchID = branchmappingtable.SubBranch WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (tripdata.Status <> 'C') AND (dispatch.DispMode = @Agent) AND (branchmappingtable.SuperBranch = @BranchID)  AND (dispatch.Branch_Id <> @Branch_ID) GROUP BY  tripdata.Sno");
                    cmd.Parameters.AddWithValue("@Agent", "AGENT");
                    cmd.Parameters.AddWithValue("@SM", "SM");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@Branch_ID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    DataTable dtAgentDesp = vdm.SelectQuery(cmd).Tables[0];
                    if (SalesOfficeID == "158" || SalesOfficeID == "1801")
                    {
                        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno, dispatch.BranchID, tripdata.I_Date,tripdata.sno as TripSno, dispatch.DispMode FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.DispType = @Agent OR dispatch.DispType = @SM) AND (dispatch.Branch_Id = @BranchID) and (tripdata.Status<>'C')  GROUP BY tripdata.sno");
                        cmd.Parameters.AddWithValue("@Agent", "AGENT");
                        cmd.Parameters.AddWithValue("@SM", "SM");
                        cmd.Parameters.AddWithValue("@BranchID", SalesOfficeID);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno, dispatch.DispMode,dispatch.BranchID, tripdata.I_Date,tripdata.sno as TripSno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2)  and (dispatch.DispType='SO') and (tripdata.Status<>'C')  group by tripdata.Sno ORDER BY dispatch.sno");
                        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    }
                    DataTable dtDispnames = vdm.SelectQuery(cmd).Tables[0];
                    DataTable dtNEWDESP = new DataTable();
                    dtNEWDESP = dtAgentDesp.Copy();
                    dtNEWDESP.Merge(dtDispnames);
                    dtDispnames = dtNEWDESP.Copy();
                    //old
                    //if (SalesOfficeID == "158" || SalesOfficeID == "1801")
                    //{
                    //    cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno, dispatch.BranchID, tripdata.I_Date,tripdata.sno as TripSno, dispatch.DispMode FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.DispType = @Agent OR dispatch.DispType = @SM) AND (dispatch.Branch_Id = @BranchID) and (tripdata.Status<>'C')  GROUP BY tripdata.sno");
                    //    cmd.Parameters.AddWithValue("@Agent", "AGENT");
                    //    cmd.Parameters.AddWithValue("@SM", "SM");
                    //    cmd.Parameters.AddWithValue("@BranchID", SalesOfficeID);
                    //    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    //    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    //}
                    //else if (SalesOfficeID == "4609")
                    //{
                    //    cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno, dispatch.DispMode,dispatch.BranchID, tripdata.I_Date,tripdata.sno as TripSno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2)  and (dispatch.DispType='SO') and (tripdata.Status<>'C')  group by tripdata.Sno ORDER BY dispatch.sno");
                    //    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    //    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    //    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    //}
                    //else
                    //{
                    //    cmd = new MySqlCommand("SELECT  dispatch.DispName, dispatch.sno, dispatch.BranchID, tripdata.I_Date, tripdata.Sno AS TripSno, dispatch.DispMode, branchmappingtable.SuperBranch, triproutes.Tripdata_sno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN branchmappingtable ON dispatch.BranchID = branchmappingtable.SubBranch WHERE (dispatch.BranchID = @BranchID)  AND (tripdata.I_Date BETWEEN @d1 AND @d2)and (dispatch.DispType='SO') and (tripdata.Status<>'C') OR (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @SuperBranch)  and (dispatch.DispType='SO')and (tripdata.Status<>'C') group by tripdata.Sno  ORDER BY dispatch.sno");
                    //    cmd.Parameters.AddWithValue("@SuperBranch", SalesOfficeID);
                    //    cmd.Parameters.AddWithValue("@BranchID", SalesOfficeID);
                    //    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    //    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    //}
                    //DataTable dtDispnames = vdm.SelectQuery(cmd).Tables[0];
                    //old
                    foreach (DataRow dr in dtDispnames.Rows)
                    {
                        //////     cmd = new MySqlCommand("SELECT tripdata.Sno, leakages.TotalLeaks, leakages.VLeaks, leakages.VReturns, leakages.ReturnQty, leakages.ProductID, leakages.FreeMilk, leakages.ShortQty FROM tripdata INNER JOIN leakages ON tripdata.Sno = leakages.TripID WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (tripdata.Sno = @Tripid)");
                        ////......01/09/2016  CRREDDY..........

                        cmd = new MySqlCommand("SELECT Triproutes.Tripdata_sno, Triproutes.RouteID, ff.Sno, ff.TotalLeaks, ff.VLeaks, ff.VReturns, ff.ReturnQty, ff.ProductName, ff.ProductID, ff.FreeMilk, ff.ShortQty FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (RouteID = @dispatchSno)) Triproutes INNER JOIN (SELECT Sno, TotalLeaks, VLeaks, VReturns, ReturnQty, ProductName, ProductID, FreeMilk, ShortQty FROM (SELECT tripdata.Sno, leakages.TotalLeaks, leakages.VLeaks, leakages.VReturns, leakages.ReturnQty, productsdata.ProductName, leakages.ProductID, leakages.FreeMilk,  leakages.ShortQty FROM  leakages INNER JOIN tripdata ON leakages.TripID = tripdata.Sno INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2)) tripinfo) ff ON ff.Sno = Triproutes.Tripdata_sno");
                        cmd.Parameters.AddWithValue("@dispatchSno", dr["sno"].ToString());
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtLeakble = vdm.SelectQuery(cmd).Tables[0];
                        string TripDataSno = "0";
                        if (dtLeakble.Rows.Count > 0)
                        {
                            TripDataSno = dtLeakble.Rows[0]["Sno"].ToString();
                        }
                        else
                        {
                            TripDataSno = "0";
                        }

                        cmd = new MySqlCommand("SELECT EmpId, TripId, ProdId, LeakQty, DOE, BranchID, Status, FreeQty, ShortQty FROM branchleaktrans WHERE (TripID = @tripid)");
                        cmd.Parameters.AddWithValue("@tripid", TripDataSno);
                        DataTable dtsalesofficeshortfree = vdm.SelectQuery(cmd).Tables[0];


                        foreach (DataRow drr in dtLeakble.Rows)
                        {
                            foreach (DataRow drallleak in dtAllLeaks.Rows)
                            {
                                if (drr["ProductID"].ToString() == drallleak["ProductID"].ToString())
                                {
                                    float leakQty = 0;
                                    float leakQtycpy = 0;
                                    float shortQty = 0;
                                    float shortQtycpy = 0;
                                    float freeQty = 0;
                                    float freeQtycpy = 0;
                                    float.TryParse(drr["TotalLeaks"].ToString(), out leakQty);
                                    float.TryParse(drallleak["LeakQty"].ToString(), out leakQtycpy);
                                    float.TryParse(drr["ShortQty"].ToString(), out shortQty);
                                    float.TryParse(drallleak["ShortQty"].ToString(), out shortQtycpy);
                                    float.TryParse(drr["FreeMilk"].ToString(), out freeQty);
                                    float.TryParse(drallleak["FreeMilk"].ToString(), out freeQtycpy);

                                    drallleak["ShortQty"] = shortQty + shortQtycpy;
                                    drallleak["LeakQty"] = leakQty + leakQtycpy;
                                    drallleak["FreeMilk"] = freeQty + freeQtycpy;
                                    //drallleak["ReturnQty"] = drr["ReturnQty"].ToString();
                                    float retunQty = 0;
                                    float VretunQty = 0;
                                    float.TryParse(drallleak["ReturnQty"].ToString(), out retunQty);
                                    float.TryParse(drallleak["VReturnQty"].ToString(), out VretunQty);
                                    float alreturnqty = 0;
                                    float alVreturnqty = 0;
                                    float.TryParse(drr["ReturnQty"].ToString(), out alreturnqty);
                                    float.TryParse(drr["VReturns"].ToString(), out alVreturnqty);
                                    float totalreturn = 0;
                                    float totalVreturn = 0;
                                    totalreturn = retunQty + alreturnqty;
                                    totalVreturn = VretunQty + alVreturnqty;
                                    drallleak["ReturnQty"] = Math.Round(totalreturn, 2);
                                    drallleak["VReturnQty"] = Math.Round(totalVreturn, 2);
                                    drallleak["TripId"] = drr["Sno"].ToString();
                                }
                            }
                        }
                        foreach (DataRow drshortfree in dtsalesofficeshortfree.Rows)
                        {
                            foreach (DataRow drallleak in dtAllLeaks.Rows)
                            {
                                if (drshortfree["ProdId"].ToString() == drallleak["ProductID"].ToString())
                                {

                                    float shortQty = 0;
                                    float freeQty = 0;
                                    float soshortQty = 0;
                                    float sofreeQty = 0;
                                    float.TryParse(drallleak["ShortQty"].ToString(), out shortQty);
                                    float.TryParse(drallleak["FreeMilk"].ToString(), out freeQty);
                                    float.TryParse(drshortfree["ShortQty"].ToString(), out soshortQty);
                                    float.TryParse(drshortfree["FreeQty"].ToString(), out sofreeQty);
                                    float totalshort = shortQty + soshortQty;
                                    float totalfree = freeQty + sofreeQty;
                                    drallleak["ShortQty"] = Math.Round(totalshort, 2);
                                    drallleak["FreeMilk"] = Math.Round(totalfree, 2);
                                }
                            }
                        }
                        string DispMode = dr["DispMode"].ToString();
                        if (DispMode != "AGENT")
                        {
                            string Disp = dr["DispName"].ToString();
                            string[] strName = Disp.Split('_');
                            DataRow newLeakages = Report.NewRow();
                            newLeakages["Route Name"] = "Leakages" + " " + strName[0];
                            float totLeakQty = 0;
                            float totLeakAmount = 0;
                            foreach (DataRow drNewRowLeaks in dtLeakble.Rows)
                            {
                                foreach (DataRow drdt in produtstbl.Rows)
                                {

                                    if (drNewRowLeaks["ProductID"].ToString() == drdt["sno"].ToString())
                                    {
                                        float LeakQty = 0;
                                        float.TryParse(drNewRowLeaks["TotalLeaks"].ToString(), out LeakQty);
                                        if (LeakQty == 0.0)
                                        {
                                        }
                                        else
                                        {
                                            newLeakages[drdt["ProductName"].ToString()] = Math.Round(LeakQty, 2);
                                        }
                                        float UnitCost = 0;
                                        float.TryParse(drdt["unitprice"].ToString(), out UnitCost);
                                        float Total = LeakQty * UnitCost;
                                        totLeakQty += LeakQty;
                                        totLeakAmount += Total;
                                    }
                                }
                            }
                            if (totLeakQty != 0.0)
                            {
                                newLeakages["Total Qty"] = Math.Round(totLeakQty, 2);
                                grnd_tot_qty += totLeakQty;
                                newLeakages["Total Amount"] = Math.Round(totLeakAmount, 2);
                                grnd_tot_amount += totLeakAmount;
                                Report.Rows.Add(newLeakages);
                            }
                            DataRow newVLeakages = Report.NewRow();
                            newVLeakages["Route Name"] = "VLeakages" + " " + Disp;
                            float totVLeakQty = 0;
                            float totVLeakAmount = 0;
                            foreach (DataRow drNewRowLeaks in dtLeakble.Rows)
                            {
                                foreach (DataRow drdt in produtstbl.Rows)
                                {
                                    if (drNewRowLeaks["ProductID"].ToString() == drdt["sno"].ToString())
                                    {
                                        float LeakQty = 0;
                                        float.TryParse(drNewRowLeaks["VLeaks"].ToString(), out LeakQty);
                                        newVLeakages[drdt["ProductName"].ToString()] = Math.Round(LeakQty, 2);
                                        float UnitCost = 0;
                                        float.TryParse(drdt["unitprice"].ToString(), out UnitCost);
                                        float Total = LeakQty * UnitCost;
                                        totVLeakQty += LeakQty;
                                        totVLeakAmount += Total;
                                    }

                                }
                            }
                            if (totVLeakQty != 0.0)
                            {
                                newVLeakages["Total Qty"] = Math.Round(totVLeakQty, 2);
                                //newVLeakages["Total Amount"] = Math.Round(totVLeakAmount, 2);
                                Report.Rows.Add(newVLeakages);
                            }
                        }
                    }
                    cmd = new MySqlCommand("SELECT tripdata.EmpId FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.sno =@dispatchSno) AND (tripdata.I_Date BETWEEN @d1 AND @d2)");
                    cmd.Parameters.AddWithValue("@dispatchSno", dtDispnames.Rows[0]["TripSno"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    DataTable DtEmpId = vdm.SelectQuery(cmd).Tables[0];
                    cmd = new MySqlCommand("SELECT Sno FROM  tripdata WHERE (DEmpId = @DEmpId) AND (I_Date BETWEEN @d1 AND @d2)");
                    cmd.Parameters.AddWithValue("@DEmpId", DtEmpId.Rows[0]["EmpId"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    DataTable DtTripId = vdm.SelectQuery(cmd).Tables[0];
                    DataTable DtLeaks = new DataTable();
                    foreach (DataRow drTrip in DtTripId.Rows)
                    {
                        cmd = new MySqlCommand("SELECT ShortQty, ProductID, LeakQty,ReturnQty, FreeMilk FROM leakages WHERE (TripID = @TripID) and VarifyStatus is NULL group by productid");
                        cmd.Parameters.AddWithValue("@TripID", drTrip["Sno"].ToString());
                        DtLeaks = vdm.SelectQuery(cmd).Tables[0];
                        foreach (DataRow drLeaks in DtLeaks.Rows)
                        {
                            foreach (DataRow drNew in dtAllLeaks.Rows)
                            {
                                if (drLeaks["ProductID"].ToString() == drNew["ProductID"].ToString())
                                {
                                    float ShortQty = 0;
                                    float.TryParse(drLeaks["ShortQty"].ToString(), out ShortQty);
                                    float AllShort = 0;
                                    float.TryParse(drNew["ShortQty"].ToString(), out AllShort);
                                    float TotalShortQty = ShortQty + AllShort;
                                    drNew["ShortQty"] = TotalShortQty;
                                    float FreeMilk = 0;
                                    float.TryParse(drLeaks["FreeMilk"].ToString(), out FreeMilk);
                                    float AllFree = 0;
                                    float.TryParse(drNew["FreeMilk"].ToString(), out AllFree);
                                    float TotalFreeQty = FreeMilk + AllFree;
                                    drNew["FreeMilk"] = TotalFreeQty;

                                    //float ReturnMilk = 0;
                                    //float.TryParse(drLeaks["ReturnQty"].ToString(), out ReturnMilk);
                                    float AllReturn = 0;
                                    float.TryParse(drNew["ReturnQty"].ToString(), out AllReturn);
                                    // float TotalReturnQty = ReturnMilk + AllReturn;
                                    float TotalReturnQty = AllReturn;
                                    drNew["ReturnQty"] = TotalReturnQty;

                                }
                            }
                        }
                    }

                    //offer
                    DataTable offerdt = new DataTable();
                    offerdt.Columns.Add("Tripid");
                    offerdt.Columns.Add("DispatchName");
                    offerdt.Columns.Add("ProductSno");
                    offerdt.Columns.Add("ProductName");
                    offerdt.Columns.Add("qty");
                    offerdt.Columns.Add("OfferQty");
                    foreach (DataRow drSub in dtDispnames.Rows)
                    {
                        cmd = new MySqlCommand("SELECT ff.TripID, Triproutes.RouteID, ff.Qty,ff.offerqty, ff.ProductId, Triproutes.Tripdata_sno FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (Tripdata_sno = @TripSno)) Triproutes INNER JOIN (SELECT TripID, Qty, ProductId,offerqty FROM (SELECT tripdata.Sno AS TripID, tripsubdata.Qty,tripsubdata.offerqty, tripsubdata.ProductId FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno WHERE (tripdata.I_Date BETWEEN @starttime AND @endtime)) tripinfo) ff ON ff.TripID = Triproutes.Tripdata_sno");
                        cmd.Parameters.AddWithValue("@TripSno", drSub["TripSno"].ToString());
                        cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
                        DataTable DtTripSubData = vdm.SelectQuery(cmd).Tables[0];
                        string Disp = drSub["DispName"].ToString();
                        string[] strName = Disp.Split('_');
                        foreach (DataRow drTripSub in DtTripSubData.Rows)
                        {
                            float oferwithqty = 0;
                            foreach (DataRow drdt in produtstbl.Rows)
                            {
                                if (drTripSub["ProductId"].ToString() == drdt["Sno"].ToString())
                                {
                                    float offerqty = 0;
                                    float.TryParse(drTripSub["offerqty"].ToString(), out offerqty);
                                    float qty = 0;
                                    float.TryParse(drTripSub["Qty"].ToString(), out qty);
                                    if (offerqty == 0.0 && qty == 0.0)
                                    {
                                    }
                                    else
                                    {
                                        DataRow offerrow = offerdt.NewRow();
                                        offerrow["ProductSno"] = drTripSub["ProductId"].ToString();
                                        offerrow["OfferQty"] = drTripSub["offerqty"].ToString();
                                        offerrow["Tripid"] = drTripSub["TripID"].ToString();
                                        offerrow["DispatchName"] = strName[0];
                                        offerrow["qty"] = drTripSub["Qty"].ToString();
                                        offerdt.Rows.Add(offerrow);
                                    }
                                }
                            }
                        }
                    }
                    DataRow Break3 = Report.NewRow();
                    Break3["Route Name"] = "Offer Qty";
                    float totaloffqty = 0;
                    foreach (DataRow drdt in produtstbl.Rows)
                    {
                        float totalofferqty = 0;
                        foreach (DataRow droffer in offerdt.Select("ProductSno='" + drdt["Sno"].ToString() + "'"))
                        {
                            float offerqty = 0;
                            float.TryParse(droffer["OfferQty"].ToString(), out offerqty);
                            if (offerqty == 0.0)
                            {
                            }
                            else
                            {
                                totalofferqty += offerqty;
                            }
                            //}
                        }
                        if (totalofferqty != 0.0)
                        {
                            Break3[drdt["ProductName"].ToString()] = totalofferqty.ToString();
                            totaloffqty += totalofferqty;
                        }
                        else
                        {
                        }
                    }
                    if (totaloffqty != 0.0)
                    {
                        Break3["Total Qty"] = Math.Round(totaloffqty, 2);
                        grnd_tot_amount += totaloffqty;
                        Report.Rows.Add(Break3);
                    }
                    //offer
                    DataRow totalfreemilk = Report.NewRow();
                    totalfreemilk["Route Name"] = "FREE MILK";
                    float totFreeQty = 0;
                    float totFreeAmount = 0;
                    foreach (DataRow drNewRowfree in dtAllLeaks.Rows)
                    {
                        foreach (DataRow drdt in produtstbl.Rows)
                        {
                            if (drNewRowfree["ProductID"].ToString() == drdt["sno"].ToString())
                            {
                                float freeQty = 0;
                                float.TryParse(drNewRowfree["FreeMilk"].ToString(), out freeQty);
                                if (freeQty == 0.0)
                                {
                                }
                                else
                                {
                                    totalfreemilk[drdt["ProductName"].ToString()] = Math.Round(freeQty, 2);
                                    float UnitCost = 0;
                                    float.TryParse(drNewRowfree["UnitPrice"].ToString(), out UnitCost);
                                    float Total = freeQty * UnitCost;
                                    totFreeQty += freeQty;
                                    totFreeAmount += Total;
                                }
                            }
                        }
                    }
                    if (totFreeQty != 0.0)
                    {
                        totalfreemilk["Total Qty"] = Math.Round(totFreeQty, 2);
                        grnd_tot_qty += totFreeQty;
                        totalfreemilk["Total Amount"] = Math.Round(totFreeAmount, 2);
                        grnd_tot_amount += totFreeAmount;
                        Report.Rows.Add(totalfreemilk);
                    }
                    foreach (DataRow drdelivery in dtalldelivery.Rows)
                    {
                        foreach (DataRow drfree in dtAllLeaks.Rows)
                        {
                            if (drdelivery["sno"].ToString() == drfree["ProductID"].ToString())
                            {
                                float freeQty = 0;
                                float.TryParse(drfree["FreeMilk"].ToString(), out freeQty);
                                drdelivery["freeQty"] = Math.Round(freeQty, 2);
                            }
                        }
                    }
                    foreach (DataRow drp in distinctLeaks.Rows)
                    {
                        if (drp["DispMode"].ToString() == "Free")
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["SNo"] = i++;
                            newrow["Route Name"] = drp["DispName"].ToString();
                            double total = 0;
                            foreach (DataRow dr in dtlOCAL.Rows)
                            {
                                if (dr["DispMode"].ToString() == "Free")
                                {
                                    if (drp["DispName"].ToString() == dr["DispName"].ToString())
                                    {

                                        double Qty = 0;
                                        double.TryParse(dr["Qty"].ToString(), out Qty);
                                        if (Qty == 0.0)
                                        {
                                        }
                                        else
                                        {
                                            newrow[dr["ProductName"].ToString()] = Qty;
                                            total += Qty;
                                        }
                                    }
                                }


                            }
                            if (total != 0.0)//Note
                            {
                                newrow["Total Qty"] = total;
                                grnd_tot_qty += total;
                                Report.Rows.Add(newrow);
                            }
                        }

                    }

                    DataRow totalshortmilk = Report.NewRow();
                    totalshortmilk["Route Name"] = "SHORT MILK";
                    float totshortQty = 0;
                    float totshortAmount = 0;
                    foreach (DataRow drNewRowshort in dtAllLeaks.Rows)
                    {
                        foreach (DataRow drdt in produtstbl.Rows)
                        {
                            if (drNewRowshort["ProductID"].ToString() == drdt["sno"].ToString())
                            {
                                float shortQty = 0;
                                float.TryParse(drNewRowshort["ShortQty"].ToString(), out shortQty);
                                if (shortQty == 0.0)
                                {
                                }
                                else
                                {
                                    totalshortmilk[drdt["ProductName"].ToString()] = Math.Round(shortQty, 2);
                                    float UnitCost = 0;
                                    float.TryParse(drNewRowshort["UnitPrice"].ToString(), out UnitCost);
                                    float Total = shortQty * UnitCost;
                                    totshortQty += shortQty;
                                    totshortAmount += Total;
                                }
                            }
                        }
                    }
                    if (totshortQty != 0.0)
                    {
                        totalshortmilk["Total Qty"] = Math.Round(totshortQty, 2);
                        grnd_tot_qty += totshortQty;
                        totalshortmilk["Total Amount"] = Math.Round(totshortAmount, 2);
                        grnd_tot_amount += totshortAmount;
                        Report.Rows.Add(totalshortmilk);
                    }
                    foreach (DataRow drdelivery in dtalldelivery.Rows)
                    {
                        foreach (DataRow drshort in dtAllLeaks.Rows)
                        {
                            if (drdelivery["sno"].ToString() == drshort["ProductID"].ToString())
                            {
                                float shortQty = 0;
                                float.TryParse(drshort["ShortQty"].ToString(), out shortQty);
                                drdelivery["shortQty"] = Math.Round(shortQty, 2);
                            }
                        }
                    }


                    DataRow totalreturnmilk = Report.NewRow();
                    totalreturnmilk["Route Name"] = "RETURN MILK";

                    float totreturnQty = 0;
                    float totreturnAmount = 0;
                    foreach (DataRow drNewRowreturn in dtAllLeaks.Rows)
                    {
                        foreach (DataRow drdt in produtstbl.Rows)
                        {
                            if (drNewRowreturn["ProductID"].ToString() == drdt["sno"].ToString())
                            {
                                float returnQty = 0;
                                float.TryParse(drNewRowreturn["ReturnQty"].ToString(), out returnQty);
                                if (returnQty == 0.0)
                                {
                                }
                                else
                                {
                                    totalreturnmilk[drdt["ProductName"].ToString()] = Math.Round(returnQty, 2);
                                    float UnitCost = 0;
                                    float.TryParse(drNewRowreturn["UnitPrice"].ToString(), out UnitCost);
                                    float Total = returnQty * UnitCost;
                                    totreturnQty += returnQty;
                                    totreturnAmount += Total;
                                }
                            }
                        }
                    }
                    if (totreturnQty != 0.0)
                    {
                        totalreturnmilk["Total Qty"] = Math.Round(totreturnQty, 2);
                        grnd_tot_qty += totreturnQty;
                        //totalreturnmilk["Total Amount"] = Math.Round(totreturnAmount, 2);
                        Report.Rows.Add(totalreturnmilk);
                    }
                    DataRow newVReturns = Report.NewRow();
                    newVReturns["Route Name"] = "VReturns";
                    float totVReturnQty = 0;
                    float totVReturnAmount = 0;
                    foreach (DataRow drNewRowLeaks in dtAllLeaks.Rows)
                    {
                        foreach (DataRow drdt in produtstbl.Rows)
                        {
                            if (drNewRowLeaks["ProductID"].ToString() == drdt["sno"].ToString())
                            {
                                float LeakQty = 0;
                                float.TryParse(drNewRowLeaks["VReturnQty"].ToString(), out LeakQty);
                                if (LeakQty == 0.0)
                                {
                                }
                                else
                                {
                                    newVReturns[drdt["ProductName"].ToString()] = Math.Round(LeakQty, 2);
                                    float UnitCost = 0;
                                    float.TryParse(drdt["unitprice"].ToString(), out UnitCost);
                                    float Total = LeakQty * UnitCost;
                                    totVReturnQty += LeakQty;
                                    totVReturnAmount += Total;
                                }
                            }

                        }
                    }
                    if (totVReturnQty != 0.0)
                    {
                        newVReturns["Total Qty"] = Math.Round(totVReturnQty, 2);
                        newVReturns["Total Amount"] = Math.Round(totVReturnAmount, 2);
                        Report.Rows.Add(newVReturns);
                    }
                    foreach (DataRow drdelivery in dtalldelivery.Rows)
                    {
                        foreach (DataRow drreturn in dtAllLeaks.Rows)
                        {
                            if (drdelivery["sno"].ToString() == drreturn["ProductID"].ToString())
                            {
                                float returnQty = 0;
                                float.TryParse(drreturn["ReturnQty"].ToString(), out returnQty);
                                drdelivery["returnQty"] = Math.Round(returnQty, 2);
                            }
                        }
                    }
                    //new total 
                    DataRow newvartical1 = Report.NewRow();
                    newvartical1["Route Name"] = "TOTAL";
                    foreach (DataRow drtotaldel in dtAllLeaks.Rows)
                    {
                        foreach (DataRow drdelivery in dtalldelivery.Rows)
                        {
                            if (drtotaldel["ProductID"].ToString() == drdelivery["sno"].ToString())
                            {
                                float qtycpy = 0;
                                float qtydelivercpy = 0;
                                float shortqty = 0;
                                float returnqty = 0;
                                float freeqty = 0;
                                float.TryParse(drtotaldel["LeakQty"].ToString(), out qtycpy);
                                float.TryParse(drdelivery["deliverQty"].ToString(), out qtydelivercpy);
                                float totdelivery = 0;
                                float.TryParse(drtotaldel["ShortQty"].ToString(), out shortqty);
                                float.TryParse(drtotaldel["ReturnQty"].ToString(), out returnqty);
                                float.TryParse(drtotaldel["FreeMilk"].ToString(), out freeqty);
                                float.TryParse(drdelivery["deliverQty"].ToString(), out qtydelivercpy);
                                totdelivery = qtycpy + qtydelivercpy;
                                totdelivery = qtycpy + qtydelivercpy + shortqty + returnqty + freeqty;
                                if (totdelivery == 0.0)
                                {
                                }
                                else
                                {
                                    newvartical1[drdelivery["ProductName"].ToString()] = Math.Round(totdelivery, 2);
                                    newvartical1["Total Qty"] = Math.Round(grnd_tot_qty, 2);
                                    newvartical1["Total Amount"] = Math.Round(grnd_tot_amount, 2);
                                    newvartical1["Cash Amount"] = Math.Round(grnd_tot_cashamount, 2);
                                    newvartical1["Dues Amount"] = Math.Round(grnd_tot_dueamount, 2);
                                }
                            }
                            else
                            {
                            }
                        }
                    }
                    Report.Rows.Add(newvartical1);
                    //double val = 0.0;
                    //foreach (DataColumn dc in Report.Columns)
                    //{
                    //    if (dc.DataType == typeof(Double))
                    //    {
                    //        val = 0.0;
                    //        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                    //        if (val == 0.0)
                    //        {
                    //        }
                    //        else
                    //        {
                    //            newvartical1[dc.ToString()] = val;
                    //        }
                    //    }
                    //}
                    //Report.Rows.Add(newvartical1);
                    DataRow Break2 = Report.NewRow();
                    //Actual

                    DataView viewdispatch = new DataView(offerdt);
                    DataTable tempoffer = viewdispatch.ToTable(true, "DispatchName", "Tripid");
                    Break2["Route Name"] = "RECEIVED MILK";
                    Report.Rows.Add(Break2);
                    foreach (DataRow drSub in tempoffer.Rows)
                    {
                        DataRow newSo = Report.NewRow();
                        newSo["Route Name"] = drSub["DispatchName"].ToString();
                        foreach (DataRow drTripSub in offerdt.Select("Tripid='" + drSub["Tripid"].ToString() + "'"))
                        {
                            float oferwithqty = 0;
                            foreach (DataRow drdt in produtstbl.Rows)
                            {
                                if (drTripSub["ProductSno"].ToString() == drdt["Sno"].ToString())
                                {
                                    float Qty = 0;
                                    float.TryParse(drTripSub["Qty"].ToString(), out Qty);
                                    float offerqty = 0;
                                    float.TryParse(drTripSub["offerqty"].ToString(), out offerqty);
                                    if (Qty == 0.0)
                                    {
                                    }
                                    else
                                    {
                                        oferwithqty = Qty + offerqty;
                                        newSo[drdt["ProductName"].ToString()] = oferwithqty.ToString();
                                        DataRow tempnewrow = dttempproducts.NewRow();
                                        tempnewrow["ProductName"] = drdt["ProductName"].ToString();
                                        tempnewrow["SubCatSno"] = drdt["SubCat_sno"].ToString();
                                        dttempproducts.Rows.Add(tempnewrow);
                                    }
                                }
                            }
                            foreach (DataRow dralldispqty in dtalldispatch.Rows)
                            {
                                if (drTripSub["ProductSno"].ToString() == dralldispqty["sno"].ToString())
                                {
                                    float qty = 0;
                                    float.TryParse(drTripSub["Qty"].ToString(), out qty);
                                    float qtycpy = 0;
                                    float.TryParse(dralldispqty["TotalQty"].ToString(), out qtycpy);
                                    float totalqty = qty + qtycpy;
                                    dralldispqty["TotalQty"] = totalqty;
                                }
                                else
                                {
                                }
                            }
                        }
                        Report.Rows.Add(newSo);
                    }

                    DataRow totaldispatch = Report.NewRow();
                    totaldispatch["Route Name"] = "TOTAL";
                    DataRow TotalRow = Report.NewRow();
                    TotalRow["Route Name"] = "DIFFERENCE";
                    foreach (DataRow dr in produtstbl.Rows)
                    {
                        foreach (DataRow dralldispqty in dtalldispatch.Rows)
                        {
                            if (dr["Sno"].ToString() == dralldispqty["sno"].ToString())
                            {
                                float qtycpy = 0;
                                float.TryParse(dralldispqty["TotalQty"].ToString(), out qtycpy);
                                float totalqty = 0;
                                totalqty = qtycpy;
                                if (totalqty == 0.0)
                                {
                                }
                                else
                                {
                                    totaldispatch[dr["ProductName"].ToString()] = Math.Round(totalqty, 2);
                                    DataRow tempnewrow = dttempproducts.NewRow();
                                    tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                    tempnewrow["SubCatSno"] = dr["SubCat_sno"].ToString();
                                    dttempproducts.Rows.Add(tempnewrow);
                                }
                            }
                            else
                            {
                            }
                        }
                    }
                    Report.Rows.Add(totaldispatch);
                    DataView SubCatview = new DataView(dttempproducts);
                    dtSubCatgory = SubCatview.ToTable(true, "SubCatSno");
                    DataView dv1 = dtSubCatgory.DefaultView;
                    dv1.Sort = "SubCatSno ASC";
                    dtSortedSubCategory = dv1.ToTable();

                    //cmd = new MySqlCommand("SELECT dispatch.BranchID, leakages.ShortQty as short, leakages.ProductID, leakages.FreeMilk AS free, tripdata.I_Date, leakages.TotalLeaks AS totleak, leakages.TripID FROM leakages INNER JOIN tripdata ON leakages.TripID = tripdata.Sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno WHERE (dispatch.BranchID = @branchid) AND (tripdata.I_Date BETWEEN @d1 AND @d2) ");
                    /// Ravi
                    cmd = new MySqlCommand("SELECT Leaks.short, Leaks.ProductID, Leaks.free, Leaks.I_Date, Leaks.totleak, Leaks.TripID, Leaks.BranchID, Leaks.Sno FROM (SELECT leakages.ShortQty AS short, leakages.ProductID, leakages.FreeMilk AS free, tripdata.I_Date, leakages.TotalLeaks AS totleak, leakages.TripID, tripdata.Sno, tripdata.BranchID FROM leakages INNER JOIN tripdata ON leakages.TripID = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno FROM  (SELECT dispatch.DispName, tripdata_1.Sno FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata tripdata_1 ON triproutes.Tripdata_sno = tripdata_1.Sno WHERE (dispatch.BranchID = @branchid) AND (tripdata_1.I_Date BETWEEN @d1 AND @d2)) dd) ff ON ff.Sno = Leaks.Sno");
                    cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    DataTable dttotsaleleak = vdm.SelectQuery(cmd).Tables[0];
                    foreach (DataRow drsaleleak in dttotsaleleak.Rows)
                    {
                        foreach (DataRow drdelivery in dtalldelivery.Rows)
                        {

                            if (drsaleleak["ProductID"].ToString() == drdelivery["sno"].ToString())
                            {
                                float leakqty = 0;
                                float.TryParse(drdelivery["leakQty"].ToString(), out leakqty);
                                float leakcpy = 0;
                                float.TryParse(drsaleleak["totleak"].ToString(), out leakcpy);
                                float totalleakqty = leakqty + leakcpy;
                                drdelivery["leakQty"] = totalleakqty;
                            }
                            else
                            {
                            }
                        }
                    }

                    foreach (var column in Report.Columns.Cast<DataColumn>().ToArray())
                    {
                        if (Report.AsEnumerable().All(dr => dr.IsNull(column)))
                            Report.Columns.Remove(column);
                    }
                    foreach (DataRow dralldispqty in dtalldispatch.Rows)
                    {
                        foreach (DataRow drdelivery in dtalldelivery.Rows)
                        {
                            if (dralldispqty["Sno"].ToString() == drdelivery["sno"].ToString())
                            {
                                try
                                {
                                    float totimwardqtycpy = 0;
                                    float qtydelivercpy = 0;
                                    float freeQty = 0;
                                    float leakQty = 0;
                                    float shortQty = 0;
                                    float returnQty = 0;
                                    float scheme = 0;
                                    float.TryParse(dralldispqty["TotalQty"].ToString(), out totimwardqtycpy);
                                    foreach (DataRow branchroute in dtOpp.Rows)
                                    {
                                        if (branchroute["sno"].ToString() == drdelivery["sno"].ToString())
                                        {
                                            float StockQty = 0;
                                            float.TryParse(branchroute["StockQty"].ToString(), out StockQty);
                                            totimwardqtycpy = totimwardqtycpy + StockQty;
                                        }
                                        else
                                        {
                                        }
                                    }
                                    float.TryParse(drdelivery["deliverQty"].ToString(), out qtydelivercpy);
                                    float.TryParse(drdelivery["freeQty"].ToString(), out freeQty);
                                    float.TryParse(drdelivery["leakQty"].ToString(), out leakQty);
                                    float.TryParse(drdelivery["shortQty"].ToString(), out shortQty);
                                    float.TryParse(drdelivery["returnQty"].ToString(), out returnQty);
                                    //float.TryParse(drdelivery["scheme"].ToString(), out scheme);
                                    float totdelivery = 0;
                                    float totdifference = 0;
                                    totdelivery = freeQty + leakQty + shortQty + returnQty + qtydelivercpy + scheme;
                                    foreach (DataRow branchroute in dtClo.Rows)
                                    {

                                        if (branchroute["sno"].ToString() == drdelivery["sno"].ToString())
                                        {
                                            float closStockQty = 0;
                                            float.TryParse(branchroute["StockQty"].ToString(), out closStockQty);
                                            totdelivery = totdelivery + closStockQty;
                                        }
                                        else
                                        {
                                        }
                                    }
                                    totdifference = totimwardqtycpy - totdelivery;
                                    //if (totimwardqtycpy == 0.0)
                                    //{
                                    //}
                                    //else
                                    //{
                                    TotalRow[dralldispqty["ProductName"].ToString()] = Math.Round(totdifference, 2);
                                    //}
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                            }
                        }
                    }
                    Report.Rows.Add(TotalRow);
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
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable("GridView_Data");
            int count = 0;
            foreach (TableCell cell in grdReports.HeaderRow.Cells)
            {
                if (count == 2)
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
    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Check your condition here, Cells[1] for ex. is DONE/Not Done column
            var salesoffice = ddlSalesOffice.SelectedItem.Text;
            var salesoff = ddlSalesOffice.SelectedValue;
            if (salesoff == "ROUTE WISE")
            {
                if (e.Row.Cells[1].Text == "Oppening Stock")
                {
                    e.Row.BackColor = System.Drawing.Color.Aquamarine;
                }
                if (e.Row.Cells[1].Text == "Closing Stock")
                {
                    e.Row.BackColor = System.Drawing.Color.DarkSalmon;
                }
            }
            else
            {
                if (e.Row.Cells[2].Text == "Oppening Stock")
                {
                    e.Row.BackColor = System.Drawing.Color.Aquamarine;
                }
                if (e.Row.Cells[2].Text == "Closing Stock")
                {
                    e.Row.BackColor = System.Drawing.Color.DarkSalmon;
                }
            }



        }
    }

    int j = 0;
    //protected void grdReports_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    // Adding a column manually once the header created
    //    if (e.Row.RowType == DataControlRowType.Header) // If header created
    //    {
    //        //j++;
    //        //e.Row.Cells[1].Visible = false;
    //        GridView ProductGrid = (GridView)sender;
    //        GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Separator, DataControlRowState.Insert);
    //        //   grdReports.HeaderRow.Cells[0].Text = "Header 1";

    //        //if (e.Row.RowType == DataControlRowType.DataRow)
    //        //{
    //        //cell.VerticalAlign = VerticalAlign.Middle;
    //        //string ItemName = cell.Text;
    //        TableCell HeaderCell = new TableCell();
    //        //if (j == 0)
    //        //{
    //        //if (ddlSalesOffice.SelectedValue == "ALL")
    //        //{
    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "Net Sales Details";
    //        HeaderCell.VerticalAlign = VerticalAlign.Middle;

    //        if (status == "Nellore")
    //        {
    //            HeaderCell.ColumnSpan = 3;
    //        }
    //        else
    //        {
    //            HeaderCell.ColumnSpan = 2;
    //        }// For merging three columns (Direct, Referral, Total)
    //        HeaderCell.CssClass = "HeaderStyle";
    //        HeaderRow.Cells.Add(HeaderCell);
    //        foreach (DataRow drsubcategory in dtSortedSubCategory.Rows)
    //        {
    //            DataTable distinctTable = dttempproducts.DefaultView.ToTable(true, "ProductName", "SubCatSno");
    //            int k = 0;
    //            foreach (DataRow dramount in distinctTable.Select("SubCatSno='" + drsubcategory["SubCatSno"].ToString() + "'"))
    //            {
    //                //foreach (DataRow dramount in dtBranch.Select("ProductName='" + ItemName + "' AND SubCatSno='" + drsubcategory["SubCatSno"].ToString() + "'"))
    //                //{
    //                //    string Temp = dramount["ProductName"].ToString();
    //                //}
    //                //Adding Year Column
    //                k++;
    //            }
    //            HeaderCell = new TableCell();
    //            if (k != 0)
    //            {
    //                foreach (DataRow dramount in produtstbl1.Select("SubCatSno='" + drsubcategory["SubCatSno"].ToString() + "'"))
    //                {
    //                    HeaderCell.Text = dramount["SubCategoryName"].ToString();
    //                }
    //            }
    //            HeaderCell.VerticalAlign = VerticalAlign.Middle;
    //            HeaderCell.ColumnSpan = k; // For merging three columns (Direct, Referral, Total)
    //            HeaderCell.CssClass = "HeaderStyle";
    //            //HeaderCell.ForeColor = col
    //            HeaderRow.Cells.Add(HeaderCell);
    //        }
    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "Net Totals";
    //        HeaderCell.VerticalAlign = VerticalAlign.Middle;
    //        HeaderCell.ColumnSpan = 8;
    //        HeaderCell.CssClass = "HeaderStyle";
    //        HeaderRow.Cells.Add(HeaderCell);
    //        // HeaderRow.Controls.Add(HeaderCell);
    //        //(GridView2.HeaderRow.Cells[3].
    //        ProductGrid.Controls[0].Controls.AddAt(0, HeaderRow);
    //    }
    //    // }
    //    //}
    //}

}
