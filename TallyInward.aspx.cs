using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;
using System.IO;
public partial class TallyInward : System.Web.UI.Page
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
            ddlSalesOffice.Items.Insert(0, new ListItem("Cash Sale", "0"));
            ddlSalesOffice.Items.Insert(0, new ListItem("Staff Sale", "1"));
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
            ddlSalesOffice.Items.Insert(0, new ListItem("Cash Sale", "0"));
            ddlSalesOffice.Items.Insert(0, new ListItem("Staff Sale", "1"));
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
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            Session["RouteName"] = ddlSalesOffice.SelectedItem.Text;
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
            lblRoutName.Text = ddlSalesOffice.SelectedItem.Text;
            Session["xporttype"] = "TallyInward";
            Session["filename"] = ddlSalesOffice.SelectedItem.Text + " Tally Inward" + fromdate.ToString("dd/MM/yyyy");

            cmd = new MySqlCommand("SELECT branchdata.branchcode, branchdata.sno, salestypemanagement.salestype FROM branchdata INNER JOIN salestypemanagement ON branchdata.SalesType = salestypemanagement.sno WHERE (branchdata.sno = @Branchid)");
            cmd.Parameters.AddWithValue("@Branchid", ddlSalesOffice.SelectedValue);
            DataTable dtbranch = vdm.SelectQuery(cmd).Tables[0];
            string branchtype = "";
            string branchcode = "";
            if (dtbranch.Rows.Count > 0)
            {
                branchtype = dtbranch.Rows[0]["salestype"].ToString();
                branchcode = dtbranch.Rows[0]["branchcode"].ToString();
            }
            if (branchtype == "Plant")
            {
                cmd = new MySqlCommand("SELECT TripInfo.Sno, TripInfo.DCNo,ProductInfo.ProductName,ProductInfo.categorycode,ProductInfo.Categoryname,ProductInfo.UnitPrice,ProductInfo.tproduct, ProductInfo.Qty,ProductInfo.Productid,ProductInfo.Itemcode, TripInfo.I_Date, TripInfo.VehicleNo, TripInfo.Status,TripInfo.whcode,TripInfo.DispName, TripInfo.DispType, TripInfo.DispMode FROM (SELECT tripdata.Sno, tripdata.DCNo, tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, dispatch.DispType, dispatch.DispMode,branchdata.whcode FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE        (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (tripdata.status <>'C')) TripInfo INNER JOIN (SELECT Categoryname,categorycode, ProductName,tproduct, Sno, Qty,Productid,Itemcode,UnitPrice FROM (SELECT products_category.Categoryname,products_category.categorycode, productsdata.ProductName,productsdata.tproduct,productsdata.sno as Productid, productsdata.Itemcode,productsdata.UnitPrice,tripdata_1.Sno, tripsubdata.Qty FROM            tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata_1.AssignDate BETWEEN @d1 AND @d2)) TripSubInfo) ProductInfo ON TripInfo.Sno = ProductInfo.Sno");
                cmd.Parameters.AddWithValue("@branch", Session["branch"]);
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT productsdata.sno,productsdata.Itemcode,products_category.Categoryname, products_subcategory.SubCatName, branchproducts.Rank, productsdata.ProductName,productsdata.tproduct, branchproducts.branch_sno FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (branchproducts.branch_sno = @Branch) AND (tripdata.BranchID = @BranchID) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@Branch", Session["branch"].ToString());
                DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
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
                if (produtstbl.Rows.Count > 0)
                {
                    DataView view = new DataView(dtble);
                    DataTable distinctproducts = view.ToTable(true, "Productid", "Categoryname");
                    Report = new DataTable();
                    Report.Columns.Add("Voucher Date");
                    Report.Columns.Add("Voucher No");
                    Report.Columns.Add("Item Name");
                    Report.Columns.Add("Qty");
                    Report.Columns.Add("Rate");
                    Report.Columns.Add("Amount");
                    Report.Columns.Add("Narration");
                    //DataTable distincttable = view.ToTable(true, "Batter", "Chapathi", "Chikki", "Icecream", "PAROTA", "Rasagulla", "Malailaddu", "KALAKANDA");
                    int i = 1;
                    string ProductName = "";
                    string Itemcode = "";
                    string UnitPrice = "";
                    string WhsCode = "";
                    string categorycode = "";
                    foreach (DataRow dr in produtstbl.Rows)
                    {
                        //if (dr["Categoryname"].ToString() == "Batter" || dr["Categoryname"].ToString() == "Chapathi" || dr["Categoryname"].ToString() == "Chikki" || dr["Categoryname"].ToString() == "Icecream" || dr["Categoryname"].ToString() == "PAROTA" || dr["Categoryname"].ToString() == "Rasagulla" || dr["Categoryname"].ToString() == "KALAKANDA" || dr["Categoryname"].ToString() == "Malailaddu")
                        //{
                        double totalqty = 0;
                        DataRow newrow = Report.NewRow();
                        foreach (DataRow drproduct in dtble.Select("Productid='" + dr["sno"].ToString() + "'"))
                        {
                            double AssignQty = 0;
                            double.TryParse(drproduct["Qty"].ToString(), out AssignQty); ;
                            ProductName = drproduct["tproduct"].ToString();
                            Itemcode = drproduct["Itemcode"].ToString();
                            UnitPrice = drproduct["UnitPrice"].ToString();
                            WhsCode = drproduct["whcode"].ToString();
                            categorycode = drproduct["categorycode"].ToString();
                            totalqty += AssignQty;
                        }
                        newrow["Voucher Date"] = fromdate.ToString("dd-MMM-yyyy");
                        newrow["Voucher No"] = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "/" + fromdate.ToString("dd/MM"); ;
                        // newrow["Voucher No"] = "PBK/" + fromdate.ToString("dd/MM/yyyy") + "";
                        newrow["Item Name"] = ProductName;
                        newrow["Qty"] = totalqty;
                        newrow["Rate"] = "0";
                        newrow["Amount"] = "0";
                        newrow["Narration"] = "Being the stock transfer to  " + ddlSalesOffice.SelectedItem.Text + "," + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                        Report.Rows.Add(newrow);
                        //}
                    }
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;
                }
                else
                {
                    pnlHide.Visible = false;
                    lblmsg.Text = "No DC Found";
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                }
            }
            else if (branchtype == "SALES OFFICE" || branchtype == "C & F")
            {
                cmd = new MySqlCommand("SELECT productsdata.tproduct, ROUND(SUM(tripsubdata.Qty), 2) AS qty,tripdata.sno, tripdata.I_Date, empmanage.UserName  FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN empmanage ON tripdata.EmpId = empmanage.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (empmanage.Branch = @BranchID)  AND (tripdata.BranchID = @PlantID) GROUP BY productsdata.tproduct");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@PlantID", Session["branch"]);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate).AddDays(-1));
                cmd.Parameters.AddWithValue("@Type", "Agent");
                DataTable dtAgent = vdm.SelectQuery(cmd).Tables[0];
                Report.Columns.Add("Voucher Date");
                Report.Columns.Add("Voucher No");
                Report.Columns.Add("Item Name");
                Report.Columns.Add("Qty");
                Report.Columns.Add("Rate");
                Report.Columns.Add("Amount");
                Report.Columns.Add("Narration");
                int i = 1;
                if (dtAgent.Rows.Count > 0)
                {
                    //DataTable distincttable = view.ToTable(true, "BranchName", "BSno");
                    foreach (DataRow branch in dtAgent.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        if (ddlSalesOffice.SelectedValue == "306")
                        {
                            newrow["Voucher Date"] = fromdate.AddDays(1).ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            newrow["Voucher Date"] = fromdate.ToString("dd-MMM-yyyy");
                        }
                        newrow["Voucher No"] = branch["sno"].ToString();
                        newrow["Item Name"] = branch["tproduct"].ToString();
                        newrow["Qty"] = branch["qty"].ToString();
                        newrow["Rate"] = "0";
                        newrow["Amount"] = "0";
                        newrow["Narration"] = "Being the stock transfer to  " + ddlSalesOffice.SelectedItem.Text + " vide dc No " + branch["sno"].ToString() + ",DC Date " + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                        Report.Rows.Add(newrow);
                        i++;
                    }
                }
                cmd = new MySqlCommand("SELECT  dispatch.DispName,dispatch.Branch_ID, dispatch.sno, dispatch.BranchID, tripdata.I_Date, tripdata.Sno AS TripSno, dispatch.DispMode, branchmappingtable.SuperBranch, triproutes.Tripdata_sno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN branchmappingtable ON dispatch.BranchID = branchmappingtable.SubBranch WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2)and (dispatch.DispType ='SO') and (tripdata.Status<>'C') OR (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @SuperBranch)  and (dispatch.DispType = 'SO')and (tripdata.Status<>'C')  ORDER BY dispatch.sno");
                cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtDispnames = vdm.SelectQuery(cmd).Tables[0];
                if (dtDispnames.Rows.Count > 0)
                {
                    foreach (DataRow drSub in dtDispnames.Rows)
                    {
                        if (drSub["DispMode"].ToString() == "" || drSub["DispMode"].ToString() == "SPL")
                        {
                        }
                        else
                        {
                            if (drSub["Branch_ID"].ToString() == ddlSalesOffice.SelectedValue)
                            {
                            }
                            else
                            {
                                cmd = new MySqlCommand("SELECT ff.TripID, Triproutes.RouteID, ff.Qty, ff.ProductId, Triproutes.Tripdata_sno,  ff.I_Date, ff.tproduct FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (Tripdata_sno = @TripSno)) Triproutes INNER JOIN (SELECT TripID, Qty, ProductId,I_Date, tproduct FROM (SELECT        tripdata.Sno AS TripID, tripsubdata.Qty, tripsubdata.ProductId, tripdata.I_Date, productsdata.tproduct FROM  tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE (tripdata.I_Date BETWEEN @starttime AND @endtime)) tripinfo) ff ON ff.TripID = Triproutes.Tripdata_sno");
                                cmd.Parameters.AddWithValue("@TripSno", drSub["TripSno"].ToString());
                                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                                cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
                                DataTable DtTripSubData = vdm.SelectQuery(cmd).Tables[0];
                                foreach (DataRow branch in DtTripSubData.Rows)
                                {
                                    DataRow newrow = Report.NewRow();
                                    newrow["Voucher Date"] = fromdate.ToString("dd-MMM-yyyy");
                                    newrow["Voucher No"] = branch["TripID"].ToString();
                                    newrow["Item Name"] = branch["tproduct"].ToString();
                                    newrow["Qty"] = branch["Qty"].ToString();
                                    newrow["Rate"] = "0";
                                    newrow["Amount"] = "0";
                                    newrow["Narration"] = "Being the stock transfer to " + drSub["DispName"].ToString() + " from " + ddlSalesOffice.SelectedItem.Text + " vide dc No " + branch["TripID"].ToString() + ",DC Date " + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                                    Report.Rows.Add(newrow);
                                    i++;
                                }
                            }
                        }
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
            else if (ddlSalesOffice.SelectedItem.Text == "Cash Sale" || ddlSalesOffice.SelectedItem.Text == "Staff Sale")
            {
                Report.Columns.Add("Voucher Date");
                Report.Columns.Add("Voucher No");
                Report.Columns.Add("Item Name");
                Report.Columns.Add("Qty");
                Report.Columns.Add("Rate");
                Report.Columns.Add("Amount");
                Report.Columns.Add("Narration");
                if (ddlSalesOffice.SelectedItem.Text == "Cash Sale")
                {
                    cmd = new MySqlCommand("SELECT TripInfo.Sno, TripInfo.DCNo, ProductInfo.tproduct, ProductInfo.Categoryname, ProductInfo.Qty, TripInfo.I_Date, TripInfo.VehicleNo, TripInfo.Status, TripInfo.DispName, TripInfo.DispType, TripInfo.DispMode FROM (SELECT tripdata.Sno, tripdata.DCNo, tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, dispatch.DispType, dispatch.DispMode FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.DispMode = 'LOCAL') AND (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2)) TripInfo INNER JOIN (SELECT Categoryname, ProductName, Sno, Qty, tproduct FROM  (SELECT products_category.Categoryname, productsdata.ProductName, tripdata_1.Sno, tripsubdata.Qty, productsdata.tproduct FROM tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata_1.AssignDate BETWEEN @d1 AND @d2)) TripSubInfo) ProductInfo ON TripInfo.Sno = ProductInfo.Sno");
                }
                else
                {
                    cmd = new MySqlCommand("SELECT TripInfo.Sno, TripInfo.DCNo, ProductInfo.tproduct, ProductInfo.Categoryname, ProductInfo.Qty, TripInfo.I_Date, TripInfo.VehicleNo, TripInfo.Status, TripInfo.DispName, TripInfo.DispType, TripInfo.DispMode FROM (SELECT tripdata.Sno, tripdata.DCNo, tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, dispatch.DispType, dispatch.DispMode FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.DispMode = 'Staff') AND (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2)) TripInfo INNER JOIN (SELECT Categoryname, ProductName, Sno, Qty, tproduct FROM  (SELECT products_category.Categoryname, productsdata.ProductName, tripdata_1.Sno, tripsubdata.Qty, productsdata.tproduct FROM tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata_1.AssignDate BETWEEN @d1 AND @d2)) TripSubInfo) ProductInfo ON TripInfo.Sno = ProductInfo.Sno");
                }
                cmd.Parameters.AddWithValue("@branch", Session["branch"]);
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate).AddDays(-1));
                DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
                int i = 1;
                foreach (DataRow branch in dtble.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Voucher Date"] = fromdate.AddDays(-1).ToString("dd-MMM-yyyy");
                    newrow["Voucher No"] = branch["Sno"].ToString();
                    newrow["Item Name"] = branch["tproduct"].ToString();
                    newrow["Qty"] = branch["qty"].ToString();
                    newrow["Rate"] = "0";
                    newrow["Amount"] = "0";
                    if (ddlSalesOffice.SelectedItem.Text == "Cash Sale")
                    {
                        newrow["Narration"] = "Being the stock transfer to  " + ddlSalesOffice.SelectedItem.Text + " vide dc No " + branch["sno"].ToString() + ",DC Date " + fromdate.AddDays(-1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                    }
                    else
                    {
                        newrow["Narration"] = "Being the stock transfer to  " + branch["DispName"].ToString() + " from " + ddlSalesOffice.SelectedItem.Text + " vide dc No " + branch["sno"].ToString() + ",DC Date " + fromdate.AddDays(-1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();

                    }
                    Report.Rows.Add(newrow);
                    i++;
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
            }
            else
            {
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