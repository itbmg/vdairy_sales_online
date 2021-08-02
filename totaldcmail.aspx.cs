using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

using System.Data.OleDb;
using System.IO;
using System.Text;
using ClosedXML.Excel;
using System.Configuration;
using System.Net;
using System.Net.Mail;





public partial class totaldcmail : System.Web.UI.Page
{
    MySqlCommand cmd;
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

            MobNo = "9092691491";

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
    DataTable dttempproducts = new DataTable();
    DataTable dtSortedSubCategory = new DataTable();
    DataTable dtSubCatgory = new DataTable();
    DataTable produtstbl1 = new DataTable();

    DataTable tempdtSortedSubCategory = new DataTable();
    DataTable tempprodutstbl1 = new DataTable();
    DataTable tempdttempproducts = new DataTable();


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
            cmd = new MySqlCommand("SELECT result.DispName, result.VehicleNo, result.Sno, tripsubdata.Qty, productsdata.sno, productsdata.tempsubcatsno, productsdata.ProductName, products_category.Categoryname, result.AssignDate, result.Status,result.I_Date, result.DCNo FROM (SELECT dispatch.DispName, tripdat.Sno, tripdat.VehicleNo, tripdat.AssignDate, tripdat.Status, tripdat.I_Date, tripdat.DCNo FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, Status, VehicleNo, I_Date, DCNo FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno WHERE (dispatch.Branch_Id = @branch) AND (dispatch.DispMode = 'SPL' OR dispatch.DispMode IS NULL) OR (dispatch.DispMode = 'SPL' OR dispatch.DispMode IS NULL) AND (branchdata.SalesOfficeID = @SOID)) result INNER JOIN tripsubdata ON result.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno ORDER BY result.DispName");
            cmd.Parameters.AddWithValue("@branch", Session["branch"]);
            cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            
            
            
            
            //cmd = new MySqlCommand("SELECT result.DispName, result.VehicleNo, result.Sno, tripsubdata.Qty, productsdata.sno, productsdata.tempsubcatsno, productsdata.ProductName, products_category.Categoryname, result.AssignDate, result.Status,result.I_Date, result.DCNo FROM (SELECT dispatch.DispName, tripdat.Sno, tripdat.VehicleNo, tripdat.AssignDate, tripdat.Status, tripdat.I_Date, tripdat.DCNo FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, Status, VehicleNo, I_Date, DCNo FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno WHERE (dispatch.Branch_Id = @branch) AND (dispatch.DispMode <> 'SPL' OR dispatch.DispMode IS NOT NULL) OR (dispatch.DispMode <> 'SPL' OR dispatch.DispMode IS NOT NULL) AND (branchdata.SalesOfficeID = @SOID)) result INNER JOIN tripsubdata ON result.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno ORDER BY result.DispName");
            //cmd.Parameters.AddWithValue("@branch", Session["branch"]);
            //cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
            //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            //cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            //DataTable dtble = vdm.SelectQuery(cmd).Tables[0];

            string bids = Session["branch"].ToString();
            //cmd = new MySqlCommand(" SELECT products_category.Categoryname, products_subcategory.SubCatName,branchproducts.Rank, productsdata.ProductName FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) and (branchproducts.flag=@Flag) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
            cmd = new MySqlCommand("SELECT products_category.Categoryname, products_subcategory.description AS SubCategoryName, products_subcategory.SubCatName, products_subcategory.tempsub_catsno AS SubCatSno, branchproducts.Rank, branchproducts.product_sno AS sno, productsdata.ProductName, productsdata.tempsubcatsno, branchproducts.branch_sno FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno INNER JOIN empmanage ON tripdata.DEmpId = empmanage.Sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (empmanage.Branch = @BranchID) AND (branchproducts.branch_sno = @Branch) GROUP BY productsdata.ProductName ORDER BY  productsdata.tempsubcatsno");
            //cmd.Parameters.AddWithValue("@Flag", "1");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@Branch", Session["branch"].ToString());
            DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
            produtstbl1 = produtstbl;
           

            if (produtstbl.Rows.Count > 0)
            {
                DataView view = new DataView(dtble);
                //DataTable distinctproducts = view.ToTable(true, "ProductName");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("Route Name");
                int count = 0;
                foreach (DataRow dr in produtstbl.Rows)
                {
                    string catname = dr["Categoryname"].ToString();
                    if (catname == "MILK" || catname == "Curd Cups" || catname == "Curd Buckets" || catname == "CURD")
                    {
                        Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                        count++;
                    }
                    
                }
                Report.Columns.Add("Total Milk", typeof(Double));
                Report.Columns.Add("Total Curd&BM", typeof(Double));
                Report.Columns.Add("Total Lts", typeof(Double));
                Report.Columns.Add("Issued Crates", typeof(Double));
                Report.Columns.Add("Issued Cans", typeof(Double));
                Report.Columns.Add("Total Amount", typeof(Double));

                dttempproducts = new DataTable();
                dttempproducts.Columns.Add("ProductName");
                dttempproducts.Columns.Add("SubCatSno").DataType = typeof(int); ;



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
                       
                        newrow["Route Name"] = branch["DispName"].ToString();
                        string AssignDate = branch["I_Date"].ToString();
                        DateTime dtAssignDate = Convert.ToDateTime(AssignDate);
                        string ChangedTime = dtAssignDate.ToString("dd/MMM/yyyy");
                       

                        double total = 0;
                        double totalcurdandBM = 0;
                        foreach (DataRow dr in dtble.Rows)
                        {

                            if (branch["Sno"].ToString() == dr["Sno"].ToString())
                            {
                                double assqty = 0;
                                double curdBm = 0;
                                double AssignQty = 0;
                                double.TryParse(dr["Qty"].ToString(), out AssignQty);
                                if (dr["Categoryname"].ToString() == "MILK")
                                {
                                    newrow[dr["ProductName"].ToString()] = AssignQty;

                                    double.TryParse(dr["Qty"].ToString(), out assqty);
                                    total += assqty;
                                    DataRow tempnewrow = dttempproducts.NewRow();
                                    tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                    tempnewrow["SubCatSno"] = dr["tempsubcatsno"].ToString();
                                    dttempproducts.Rows.Add(tempnewrow);
                                }
                                if (dr["Categoryname"].ToString() == "CURD")
                                {
                                    newrow[dr["ProductName"].ToString()] = AssignQty;

                                    double.TryParse(dr["Qty"].ToString(), out curdBm);
                                    totalcurdandBM += curdBm;
                                    DataRow tempnewrow = dttempproducts.NewRow();
                                    tempnewrow["ProductName"] = dr["ProductName"].ToString();
                                    tempnewrow["SubCatSno"] = dr["tempsubcatsno"].ToString();
                                    dttempproducts.Rows.Add(tempnewrow);
                                }
                                //if (dr["Categoryname"].ToString() == "ButterMilk")
                                //{
                                //    newrow[dr["ProductName"].ToString()] = AssignQty;

                                //    double.TryParse(dr["Qty"].ToString(), out Buttermilk);
                                //    totalcurdandBM += Buttermilk;
                                //}

                               
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
                DataRow newvartical = Report.NewRow();
                newvartical["Route Name"] = "Total";
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

                DataView SubCatview = new DataView(dttempproducts);
                dtSubCatgory = SubCatview.ToTable(true, "SubCatSno");
                DataView dv1 = dtSubCatgory.DefaultView;
                dv1.Sort = "SubCatSno ASC";
                dtSortedSubCategory = dv1.ToTable();


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
                        foreach (DataRow dr in dtble.Rows)
                        {

                            if (branch["Sno"].ToString() == dr["Sno"].ToString())
                            {
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
                DataRow newvartical = Report.NewRow();
                newvartical["Route Name"] = "Total";
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

    string status = "";

    protected void grdtotal_dcReports_RowCreated(object sender, GridViewRowEventArgs e)
    {
        // Adding a column manually once the header created
        if (e.Row.RowType == DataControlRowType.Header) // If header created
        {
            e.Row.BackColor = System.Drawing.Color.LightPink;
            //j++;
            //e.Row.Cells[1].Visible = false;
            GridView ProductGrid = (GridView)sender;
            GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Separator, DataControlRowState.Insert);
            HeaderRow.BackColor = System.Drawing.Color.FromArgb(138, 43, 226);
            //   grdReports.HeaderRow.Cells[0].Text = "Header 1";

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //cell.VerticalAlign = VerticalAlign.Middle;
            //string ItemName = cell.Text;
            TableCell HeaderCell = new TableCell();
            //if (j == 0)
            //{
            //if (ddlSalesOffice.SelectedValue == "ALL")
            //{
            HeaderCell = new TableCell();
            HeaderCell.Text = "Total Dispatch Details";
            HeaderCell.VerticalAlign = VerticalAlign.Middle;

            if (status == "Nellore")
            {
                HeaderCell.ColumnSpan = 3;
            }
            else
            {
                HeaderCell.ColumnSpan = 2;
            }// For merging three columns (Direct, Referral, Total)
            HeaderCell.CssClass = "HeaderStyle";
            HeaderRow.Cells.Add(HeaderCell);
            if (tempdtSortedSubCategory.Rows.Count > 0)
            {

            }
            else
            {
                tempdtSortedSubCategory = dtSortedSubCategory;
            }

            if (tempprodutstbl1.Rows.Count > 0)
            {

            }
            else
            {
                tempprodutstbl1 = produtstbl1;
            }
            if (tempdttempproducts.Rows.Count > 0)
            {

            }
            else
            {
                tempdttempproducts = dttempproducts;
            }

            foreach (DataRow drsubcategory in tempdtSortedSubCategory.Rows)
            {
                DataTable distinctTable = tempdttempproducts.DefaultView.ToTable(true, "ProductName", "SubCatSno");
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
                    foreach (DataRow dramount in tempprodutstbl1.Select("tempsubcatsno='" + drsubcategory["SubCatSno"].ToString() + "'"))
                    {
                        HeaderCell.Text = dramount["SubCategoryName"].ToString();
                    }
                }
                HeaderCell.VerticalAlign = VerticalAlign.Middle;
                HeaderCell.ColumnSpan = k; // For merging three columns (Direct, Referral, Total)
                HeaderCell.CssClass = "HeaderStyle";
                //HeaderCell.ForeColor =
                HeaderRow.Cells.Add(HeaderCell);
            }
            HeaderCell = new TableCell();
            HeaderCell.Text = "Net Totals";
            HeaderCell.VerticalAlign = VerticalAlign.Middle;
            HeaderCell.ColumnSpan = 6;
            HeaderCell.CssClass = "HeaderStyle";
            HeaderRow.Cells.Add(HeaderCell);
            // HeaderRow.Controls.Add(HeaderCell);
            //(GridView2.HeaderRow.Cells[3].
            ProductGrid.Controls[0].Controls.AddAt(0, HeaderRow);

        }
        // }
        //}
    }

    protected void SendEmail(object sender, EventArgs e)
    {
        GetReport();
        VehicleDBMgr vdbmngr = new VehicleDBMgr();
        DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdbmngr.conn);
        DateTime dtFromdate = ServerDateCurrentdate.AddDays(-1);
        string DATE = dtFromdate.ToString("dd/MM/yyyy");
        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                grdtotal_dcReports.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString());
                string senderID = "dd@vyshnavi.in";// use sender's email id here..
                const string senderPassword = "Vyshnavi@123"; // sender password here...
                SmtpClient smtp = new SmtpClient
                {
                    Host = "czismtp.logix.in", // smtp server address here...
                    Port = 587,
                    //security type=tsl;
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 60000,
                };
                //MailMessage mm = new MailMessage();
                //mm.From = new MailAddress(senderID);
                //string tomailid = "naveen@vyshnavi.in";
                //string[] multimailid = tomailid.Split(',');
                //foreach (string mailid in multimailid)
                //{
                //    mm.To.Add(new MailAddress(mailid));
                //}

                MailMessage mm = new MailMessage(senderID, "");
                mm.Subject = "Dispatch Details (" + DATE + ")";
                mm.Body = "Dispatch Details:<hr />" + sw.ToString(); ;
                mm.IsBodyHtml = true;
                smtp.Send(mm);
                //SmtpClient smtp = new SmtpClient();
                //smtp.Host = "smtp.gmail.com";
                //smtp.EnableSsl = true;
                //System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                //NetworkCred.UserName = "sender@gmail.com";
                //NetworkCred.Password = "<password>";
                //smtp.UseDefaultCredentials = true;
                //smtp.Credentials = NetworkCred;
                //smtp.Port = 587;
                //smtp.Send(mm);
            }
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
}