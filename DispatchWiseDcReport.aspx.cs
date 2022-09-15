using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.IO;
using MySql.Data.MySqlClient;

public partial class DispatchWiseDcReport : System.Web.UI.Page
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
                fillso();
                FillPlantDisp();
            }
        }
    }
    void fillso()
    {
        vdm = new VehicleDBMgr();
        if (Session["salestype"].ToString() == "Plant")
        {
            PBranch.Visible = false;
        }
        else
        {
            PBranch.Visible = true;
            cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata.flag = 1) AND (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            ddlSalesOffice.DataSource = dtRoutedata;
            ddlSalesOffice.DataTextField = "BranchName";
            ddlSalesOffice.DataValueField = "sno";
            ddlSalesOffice.DataBind();
        }
    }
    void FillPlantDisp()
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno WHERE  (dispatch.Branch_Id = @BranchID) AND (dispatch.Dispdate IS NOT NULL) OR (branchdata.SalesOfficeID = @SOID) AND (dispatch.Dispdate IS NOT NULL) ORDER BY dispatch.BranchID, dispatch.sno");
        cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
        cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlDispName.DataSource = dtRoutedata;
        ddlDispName.DataTextField = "DispName";
        ddlDispName.DataValueField = "sno";
        ddlDispName.DataBind();

    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
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
            lbl_fromDate.Text = fromdate.ToString("dd/MM/yyyy");
            lbl_selttodate.Text = Todate.ToString("dd/MM/yyyy");
            Session["filename"] = "TOTAL DC REPORT";
            //cmd = new MySqlCommand("SELECT tripdata.Sno, tripsubdata.Qty, productsdata.ProductName, tripdata.VehicleNo, dispatch.DispName FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE (branchroutes.BranchID = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2)");
            cmd = new MySqlCommand("SELECT ROUND(SUM(tripsubdata.Qty), 2) AS Qty, productsdata.ProductName FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno  WHERE (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (triproutes.RouteID = @DispNo) GROUP BY productsdata.ProductName");
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd.Parameters.AddWithValue("@branch", Session["branch"]);
            }
            else
            {
                cmd.Parameters.AddWithValue("@branch", ddlSalesOffice.SelectedValue);
            }
            cmd.Parameters.AddWithValue("@DispNo", ddlDispName.SelectedValue);
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
                    ProductName += dr["ProductName"].ToString() + "->" + Math.Round(unitQty, 2) + ";";
                    TotalQty += Math.Round(unitQty, 2);
                }
            }
            string Date = DateTime.Now.ToString("dd/MM/yyyy");
            WebClient client = new WebClient();
            string DispatchName = "";
            if (Session["BranchName"] != null)
            {
                DispatchName = Session["BranchName"].ToString();
            }
            else
            {
                 DispatchName = "SRIKALAHASTHI";
            }
            string baseurl = "http://103.16.101.52:8080/sendsms/bulksms?username=kapd-vyshnavi&password=vysavi&type=0&dlr=1&destination=" + MobNo + "&source=VYSNAVI&message=%20" + DispatchName + "%20,%20 + Despatch%20For%20" + ProductName + "TotalQty ->" + TotalQty + "";
            Stream data = client.OpenRead(baseurl);
            StreamReader reader = new StreamReader(data);
            string ResponseID = reader.ReadToEnd();
            data.Close();
            reader.Close();
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
            lblDispName.Text = ddlDispName.SelectedItem.Text;
            lbl_fromDate.Text = fromdate.ToString("dd/MM/yyyy");
            lbl_selttodate.Text = Todate.ToString("dd/MM/yyyy");
            Session["filename"] = "TOTAL DC REPORT";
            //cmd = new MySqlCommand("SELECT tripdata.Sno, tripsubdata.Qty, productsdata.ProductName, tripdata.VehicleNo, dispatch.DispName FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE (branchroutes.BranchID = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2)");
            cmd = new MySqlCommand("SELECT tripdata.Sno, tripsubdata.Qty, productsdata.Qty as uomqty, productsdata.ProductName,tripdata.I_Date, tripdata.VehicleNo,tripdata.Status, dispatch.DispName, products_category.Categoryname FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2) and (dispatch.sno=@DispNo)");
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd.Parameters.AddWithValue("@branch", Session["branch"]);
            }
            else
            {
                cmd.Parameters.AddWithValue("@branch", ddlSalesOffice.SelectedValue);
            }
            cmd.Parameters.AddWithValue("@DispNo", ddlDispName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            //cmd = new MySqlCommand(" SELECT products_category.Categoryname, products_subcategory.SubCatName,branchproducts.Rank, productsdata.ProductName FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) and (branchproducts.flag=@Flag) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
            cmd = new MySqlCommand("SELECT products_category.Categoryname, products_subcategory.SubCatName, branchproducts.Rank, productsdata.ProductName, branchproducts.branch_sno FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno INNER JOIN empmanage ON tripdata.DEmpId = empmanage.Sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (empmanage.Branch = @BranchID) AND (branchproducts.branch_sno = @Branch) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
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
                        //newrow["Route Name"] = branch["DispName"].ToString();
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
                                newrow[dr["ProductName"].ToString()] = AssignQty;
                                if (dr["Categoryname"].ToString() == "MILK")
                                {
                                    double.TryParse(dr["Qty"].ToString(), out assqty);
                                    double.TryParse(dr["uomqty"].ToString(), out uom);
                                    double milkqty = assqty * uom / 1000;
                                    total += milkqty;
                                }
                                if (dr["Categoryname"].ToString() == "CURD" || dr["Categoryname"].ToString() == "Curd Buckets" || dr["Categoryname"].ToString() == "Curd Cups")
                                {
                                    double.TryParse(dr["Qty"].ToString(), out curdBm);
                                    double.TryParse(dr["uomqty"].ToString(), out uom);
                                    double curdqty = curdBm * uom / 1000;
                                    totalcurdandBM += curdqty;
                                }
                                if (dr["Categoryname"].ToString() == "ButterMilk")
                                {
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
                        foreach (DataRow drinv in dtissuedinv.Rows)
                        {
                            if (drinv["invid"].ToString() == "1")
                            {

                                double issuedcrates = 0;
                                double.TryParse(drinv["Qty"].ToString(), out issuedcrates);

                                newrow["Issued Crates"] = issuedcrates;
                            }
                            if (drinv["invid"].ToString() == "4")
                            {
                                double issuedcans = 0;
                                double.TryParse(drinv["Qty"].ToString(), out issuedcans);
                                newrow["Issued Cans"] = issuedcans;
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
                newvartical["VehicleNo"] = "Total";
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
                Session["xportdata"] = Report;
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