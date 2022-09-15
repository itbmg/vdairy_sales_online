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


public partial class ProductWiseTotalDCReport : System.Web.UI.Page
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
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                lblTitle.Text = Session["TitleName"].ToString();
                txtfromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txttodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        if (ddlReportType.SelectedValue == "Milk Curd & BM")
        {
            GetReport();
        }
        else
        {
            Biproductsreport();
        }
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
            string baseurl = "http://103.16.101.52:8080/sendsms/bulksms?username=kapd-vyshnavi&password=vysavi&type=0&dlr=1&destination=" + MobNo + "&source=VYSNAVI&message=%20" + DispatchName + "%20,%20 + Despatch%20For%20" + ProductName + "TotalQty ->" + TotalQty + "";
            Stream data = client.OpenRead(baseurl);
            StreamReader reader = new StreamReader(data);
            string ResponseID = reader.ReadToEnd();
            data.Close();
            reader.Close();

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
            string baseurl1 = "http://103.16.101.52:8080/sendsms/bulksms?username=kapd-vyshnavi&password=vysavi&type=0&dlr=1&destination=" + MobNo + "&source=VYSNAVI&message=%20" + DispatchName + "%20,%20 +Total Despatch" + subcategoryName + "TotalQty ->" + SubCategoryTotalQty + "";
            Stream data1 = client.OpenRead(baseurl1);
            StreamReader reader1 = new StreamReader(data1);
            string ResponseID1 = reader1.ReadToEnd();
            data1.Close();
            reader1.Close();

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
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            Report = new DataTable();
            pnlHide.Visible = true;
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
            Session["filename"] = "TOTAL DC REPORT";
            //cmd = new MySqlCommand("SELECT tripdata.Sno, tripsubdata.Qty, productsdata.ProductName,tripdata.I_Date, tripdata.VehicleNo,tripdata.Status, dispatch.DispName, products_category.Categoryname FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.Branch_Id = @branch)  AND (tripdata.AssignDate BETWEEN @d1 AND @d2)");
            cmd = new MySqlCommand("SELECT result.DispName, result.VehicleNo, result.Sno, tripsubdata.Qty, productsdata.ProductName, productsdata.Qty as uomqty, products_category.Categoryname, result.AssignDate, result.Status,result.I_Date, result.DCNo FROM (SELECT dispatch.DispName, tripdat.Sno, tripdat.VehicleNo, tripdat.AssignDate, tripdat.Status, tripdat.I_Date, tripdat.DCNo FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, Status, VehicleNo, I_Date, DCNo FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno WHERE (dispatch.Branch_Id = @branch) OR (branchdata.SalesOfficeID = @SOID)) result INNER JOIN tripsubdata ON result.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno");   
            cmd.Parameters.AddWithValue("@branch", Session["branch"]);
            cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            //cmd = new MySqlCommand(" SELECT products_category.Categoryname, products_subcategory.SubCatName,branchproducts.Rank, productsdata.ProductName FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) and (branchproducts.flag=@Flag) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
            cmd = new MySqlCommand("SELECT products_category.Categoryname, products_subcategory.SubCatName, branchproducts.Rank, productsdata.ProductName, branchproducts.branch_sno FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno INNER JOIN empmanage ON tripdata.DEmpId = empmanage.Sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (empmanage.Branch = @BranchID) AND (branchproducts.branch_sno = @Branch) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
            //cmd.Parameters.AddWithValue("@Flag", "1");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@Branch", Session["branch"].ToString());
            DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
            if (produtstbl.Rows.Count > 0)
            {
                DataView view = new DataView(dtble);
                //DataTable distinctproducts = view.ToTable(true, "ProductName");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("VehicleNo");
                Report.Columns.Add("DC No");
                Report.Columns.Add("DC Date");
                Report.Columns.Add("Route Name");
                int count = 0;
                foreach (DataRow dr in produtstbl.Rows)
                {

                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                    count++;
                }
                Report.Columns.Add("Total Milk", typeof(Double));
                Report.Columns.Add("Total Curd&BM", typeof(Double));
                Report.Columns.Add("Total Lts", typeof(Double));
                Report.Columns.Add("Issued Crates", typeof(Double));
                Report.Columns.Add("Issued Cans", typeof(Double));
                Report.Columns.Add("Total Amount", typeof(Double));
                DataTable distincttable = view.ToTable(true, "DispName", "VehicleNo", "Sno", "Status", "I_Date");
                int i = 1;
                foreach (DataRow branch in distincttable.Rows)
                {
                    if (branch["Status"].ToString() == "C")
                    {

                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT invid, Qty FROM tripinvdata WHERE (Tripdata_sno = @tripid)");
                        cmd.Parameters.AddWithValue("@tripid", branch["Sno"].ToString());
                        DataTable dtissuedinv = vdm.SelectQuery(cmd).Tables[0];
                        DataRow newrow = Report.NewRow();
                        newrow["SNo"] = i;
                        newrow["VehicleNo"] = branch["VehicleNo"].ToString();
                        newrow["DC No"] = branch["Sno"].ToString();
                        newrow["Route Name"] = branch["DispName"].ToString();
                        string AssignDate = branch["I_Date"].ToString();
                        DateTime dtAssignDate = Convert.ToDateTime(AssignDate);
                        string ChangedTime = dtAssignDate.ToString("dd/MMM/yyyy");
                        newrow["DC Date"] = ChangedTime;

                        double total = 0;
                        double totalcurdandBM = 0;
                        double totalltrs = 0;
                        foreach (DataRow dr in dtble.Rows)
                        {

                            if (branch["Sno"].ToString() == dr["Sno"].ToString())
                            {
                                double assqty = 0;
                                double curdBm = 0;
                                double Buttermilk = 0;
                                double AssignQty = 0;
                                double uom = 0;
                                double.TryParse(dr["Qty"].ToString(), out AssignQty);
                                if (dr["Categoryname"].ToString() == "MILK")
                                {
                                    newrow[dr["ProductName"].ToString()] = AssignQty;
                                    double.TryParse(dr["Qty"].ToString(), out assqty);
                                    double.TryParse(dr["uomqty"].ToString(), out uom);
                                    double milkqty = assqty * uom / 1000;
                                    total += milkqty;
                                }
                                if (dr["Categoryname"].ToString() == "CURD" || dr["Categoryname"].ToString() == "Curd Buckets" || dr["Categoryname"].ToString() == "Curd Cups")
                                {
                                    newrow[dr["ProductName"].ToString()] = AssignQty;

                                    double.TryParse(dr["Qty"].ToString(), out curdBm);
                                    double.TryParse(dr["uomqty"].ToString(), out uom);
                                    double curdqty = curdBm * uom / 1000;
                                    totalcurdandBM += curdqty;
                                }
                                if (dr["Categoryname"].ToString() == "ButterMilk")
                                {
                                    newrow[dr["ProductName"].ToString()] = AssignQty;

                                    double.TryParse(dr["Qty"].ToString(), out Buttermilk);
                                    double.TryParse(dr["uomqty"].ToString(), out uom);
                                    double buttermilkqty = Buttermilk * uom / 1000;
                                    totalcurdandBM += buttermilkqty;
                                }
                            }
                        }
                        newrow["Total Milk"] = total;
                        newrow["Total Curd&BM"] = totalcurdandBM;
                        newrow["Total Lts"] = total + totalcurdandBM;
                        double cans = 0;
                        foreach (DataRow drinv in dtissuedinv.Rows)
                        {
                            string invid = drinv["invid"].ToString();
                            if (invid == "2")
                            {
                                invid = "4";
                            }
                            if (invid == "3")
                            {
                                invid = "4";
                            }
                            if (invid == "1")
                            {

                                double issuedcrates = 0;
                                double.TryParse(drinv["Qty"].ToString(), out issuedcrates);

                                newrow["Issued Crates"] = issuedcrates;
                            }
                            if (invid == "4")
                            {
                                double issuedcans = 0;
                                double.TryParse(drinv["Qty"].ToString(), out issuedcans);
                                cans += issuedcans;
                                newrow["Issued Cans"] = cans;
                            }


                        }
                        Report.Rows.Add(newrow);
                        i++;
                    }
                }
                foreach (var column in Report.Columns.Cast<DataColumn>().ToArray())
                {
                    if (Report.AsEnumerable().All(dr => dr.IsNull(column)))
                        Report.Columns.Remove(column);
                }
                DataRow newvartical = Report.NewRow();
                newvartical["Route Name"] = "Total";
                double val = 0.0;
                foreach (DataColumn dc in Report.Columns)
                {
                    if (dc.DataType == typeof(Double))
                    {
                        val = 0.0;
                        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                        if (val > 0)
                        {
                            newvartical[dc.ToString()] = val;
                        }
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
                grdtotal_dcReports.DataSource = Report;
                grdtotal_dcReports.DataBind();
                //Session["xportdata"] = Report;
            }
            else
            {
                pnlHide.Visible = false;
                lblmsg.Text = "No DC Found";
                grdtotal_dcReports.DataSource = Report;
                grdtotal_dcReports.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdtotal_dcReports.DataSource = Report;
            grdtotal_dcReports.DataBind();
        }
    }
    void Biproductsreport()
    {
        try
        {
            lblmsg.Text = "";
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
            Session["filename"] = "TOTAL DC REPORT";
            //cmd = new MySqlCommand("SELECT tripdata.Sno, tripsubdata.Qty, productsdata.ProductName,tripdata.I_Date, tripdata.VehicleNo,tripdata.Status, dispatch.DispName, products_category.Categoryname FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.Branch_Id = @branch)  AND (tripdata.AssignDate BETWEEN @d1 AND @d2)");
            cmd = new MySqlCommand("SELECT tripdata.Sno,tripdata.Dcno, tripsubdata.Qty, productsdata.ProductName, tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, products_category.Categoryname FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno WHERE (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2) OR (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (branchdata.SalesOfficeID = @SOID)");
            cmd.Parameters.AddWithValue("@branch", Session["branch"]);
            cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            //cmd = new MySqlCommand(" SELECT products_category.Categoryname, products_subcategory.SubCatName,branchproducts.Rank, productsdata.ProductName FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) and (branchproducts.flag=@Flag) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
            cmd = new MySqlCommand("SELECT products_category.Categoryname, products_subcategory.SubCatName, branchproducts.Rank, productsdata.ProductName, branchproducts.branch_sno FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno INNER JOIN empmanage ON tripdata.DEmpId = empmanage.Sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (empmanage.Branch = @BranchID) AND (branchproducts.branch_sno = @Branch) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
            //cmd.Parameters.AddWithValue("@Flag", "1");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@Branch", Session["branch"].ToString());
            DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
            if (produtstbl.Rows.Count > 0)
            {
                DataView view = new DataView(dtble);
                //DataTable distinctproducts = view.ToTable(true, "ProductName");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("VehicleNo");
                Report.Columns.Add("DC No");
                Report.Columns.Add("DC Date");
                Report.Columns.Add("Route Name");
                foreach (DataRow dr in produtstbl.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                }
                Report.Columns.Add("Total Milk", typeof(Double));
                Report.Columns.Add("Total Curd&BM", typeof(Double));
                Report.Columns.Add("Total Lts", typeof(Double));
                Report.Columns.Add("Issued Crates", typeof(Double));
                Report.Columns.Add("Issued Cans", typeof(Double));
                Report.Columns.Add("Total Amount", typeof(Double));
                DataTable distincttable = view.ToTable(true, "DispName", "VehicleNo", "Sno", "Status", "I_Date");
                int i = 1;
                foreach (DataRow branch in distincttable.Rows)
                {
                    if (branch["Status"].ToString() == "C")
                    {

                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT invid, Qty FROM tripinvdata WHERE (Tripdata_sno = @tripid)");
                        cmd.Parameters.AddWithValue("@tripid", branch["Sno"].ToString());
                        DataTable dtissuedinv = vdm.SelectQuery(cmd).Tables[0];
                        DataRow newrow = Report.NewRow();
                        newrow["SNo"] = i;
                        newrow["VehicleNo"] = branch["VehicleNo"].ToString();
                        newrow["DC No"] = branch["Sno"].ToString();
                        newrow["Route Name"] = branch["DispName"].ToString();
                        string AssignDate = branch["I_Date"].ToString();
                        DateTime dtAssignDate = Convert.ToDateTime(AssignDate);
                        string ChangedTime = dtAssignDate.ToString("dd/MMM/yyyy");
                        newrow["DC Date"] = ChangedTime;

                        double total = 0;
                        double totalcurdandBM = 0;
                        double totalltrs = 0;
                        foreach (DataRow dr in dtble.Rows)
                        {

                            if (branch["Sno"].ToString() == dr["Sno"].ToString())
                            {
                                double assqty = 0;
                                double curdBm = 0;
                                double Buttermilk = 0;
                                double AssignQty = 0;
                                double.TryParse(dr["Qty"].ToString(), out AssignQty);
                                Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                            }
                        }
                        newrow["Total Milk"] = total;
                        newrow["Total Curd&BM"] = totalcurdandBM;
                        newrow["Total Lts"] = total + totalcurdandBM;
                        double cans = 0;
                        foreach (DataRow drinv in dtissuedinv.Rows)
                        {
                            string invid = drinv["invid"].ToString();
                            if (invid == "2")
                            {
                                invid = "4";
                            }
                            if (invid == "3")
                            {
                                invid = "4";
                            }
                            if (invid == "1")
                            {

                                double issuedcrates = 0;
                                double.TryParse(drinv["Qty"].ToString(), out issuedcrates);

                                newrow["Issued Crates"] = issuedcrates;
                            }
                            if (invid == "4")
                            {
                                double issuedcans = 0;
                                double.TryParse(drinv["Qty"].ToString(), out issuedcans);
                                cans += issuedcans;
                                newrow["Issued Cans"] = cans;
                            }


                        }
                        Report.Rows.Add(newrow);
                        i++;
                    }
                }
                foreach (var column in Report.Columns.Cast<DataColumn>().ToArray())
                {
                    if (Report.AsEnumerable().All(dr => dr.IsNull(column)))
                        Report.Columns.Remove(column);
                }
                DataRow newvartical = Report.NewRow();
                newvartical["Route Name"] = "Total";
                double val = 0.0;
                foreach (DataColumn dc in Report.Columns)
                {
                    if (dc.DataType == typeof(Double))
                    {
                        val = 0.0;
                        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                        if (val > 0)
                        {
                            newvartical[dc.ToString()] = val;
                        }
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
                grdtotal_dcReports.DataSource = Report;
                grdtotal_dcReports.DataBind();
                //Session["xportdata"] = Report;
            }
            else
            {
                lblmsg.Text = "No DC Found";
                grdtotal_dcReports.DataSource = Report;
                grdtotal_dcReports.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdtotal_dcReports.DataSource = Report;
            grdtotal_dcReports.DataBind();
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
}