using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class DayWiseDispatch : System.Web.UI.Page
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
            lblmsg.Text = "";
            pnlHide.Visible = true;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
            Report = new DataTable();
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
            //cmd = new MySqlCommand("SELECT SUM(tripsubdata.Qty) AS assignedqty,productsdata.ProductName, products_category.Categoryname,DATE_FORMAT(tripdata.AssignDate, '%d %b %y') as AssignDate FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2) GROUP BY productsdata.ProductName, tripdata.AssignDate ORDER BY tripdata.AssignDate"); 
            cmd = new MySqlCommand("SELECT result.AssignDate, SUM(tripsubdata.Qty) AS assignedqty, tripsubdata.ProductId, products_category.Categoryname,productsdata.ProductName,productsdata.Qty as uomqty FROM (SELECT dispatch.sno AS dissno, tripdat.Sno, tripdat.AssignDate FROM dispatch INNER JOIN  triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, EmpId, DATE_FORMAT(AssignDate, '%d %b %y') AS AssignDate,Status FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno WHERE (dispatch.Branch_Id = @BranchID)) result INNER JOIN tripsubdata ON result.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno GROUP BY result.AssignDate, tripsubdata.ProductId");
            //cmd = new MySqlCommand("SELECT SUM(tripsubdata.Qty) AS assignedqty, productsdata.ProductName, products_category.Categoryname, DATE_FORMAT(tripdata.AssignDate, '%d %b %y') AS AssignDate FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno WHERE (((dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2)) OR ((tripdata.AssignDate BETWEEN @d1 AND @d2) AND (branchdata.SalesOfficeID = @SOID))) GROUP BY productsdata.ProductName, products_category.Categoryname,AssignDate ORDER BY AssignDate");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
           // cmd = new MySqlCommand("SELECT SUM(tripinvdata.Qty) AS invassigned, invmaster.InvName, tripdata.AssignDate, invmaster.sno FROM tripdata INNER JOIN tripinvdata ON tripdata.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno INNER JOIN triproutes ON tripinvdata.Tripdata_sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno WHERE (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @branch) AND (tripdata.Status <> 'C') GROUP BY invmaster.sno, tripdata.I_Date ORDER BY tripdata.AssignDate");
            cmd = new MySqlCommand("SELECT SUM(tripinvdata.Qty) AS invassigned, invmaster.InvName, tripdat.AssignDate, invmaster.sno FROM (SELECT Sno, EmpId, DATE_FORMAT(AssignDate, '%d %b %y') AS AssignDate, Status, Userdata_sno, Remarks, VehicleNo, RecieptNo, I_Date, DEmpId, ATripid, InvStatus, GPStatus, PlanStatus, DespatchStatus FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2) AND (Status <> 'C') AND (BranchID = @branch)) tripdat INNER JOIN tripinvdata ON tripdat.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno GROUP BY tripinvdata.invid, tripdat.AssignDate ORDER BY tripdat.AssignDate");   
            cmd.Parameters.AddWithValue("@branch", Session["branch"]);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            DataTable dtinventry = vdm.SelectQuery(cmd).Tables[0];
          cmd = new MySqlCommand(" SELECT products_category.Categoryname, products_subcategory.SubCatName,branchproducts.Rank, productsdata.ProductName FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) and (branchproducts.flag=@Flag) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
            //  cmd = new MySqlCommand("SELECT products_category.Categoryname, products_subcategory.SubCatName, branchproducts.Rank, productsdata.ProductName, tripdata.AssignDate FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN tripsubdata ON branchproducts.product_sno = tripsubdata.ProductId INNER JOIN tripdata ON tripsubdata.Tripdata_sno = tripdata.Sno WHERE (tripdata.BranchID = @BranchID) AND (branchproducts.flag = @Flag) AND (tripdata.AssignDate BETWEEN @d1 AND @d2) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
            cmd.Parameters.AddWithValue("@Flag", "1");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@Branch", Session["branch"].ToString());
            DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
            if (produtstbl.Rows.Count > 0)
            {
                DataView view = new DataView(dtble);
                Report = new DataTable();
                Report.Columns.Add("SNo");
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
                DataTable distincttable = view.ToTable(true, "AssignDate");
                int i = 1;
                
                foreach (DataRow branch in distincttable.Rows)
                {
                    string AssignDate = branch["AssignDate"].ToString();
                    DateTime dtAssignDate = Convert.ToDateTime(AssignDate);
                    string ChangedTime = dtAssignDate.ToString("dd/MMM/yyyy");
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    newrow["DC Date"] = ChangedTime;
                    double total = 0;
                    double totalcurdandBM = 0;
                    double totalltrs = 0;
                    foreach (DataRow drdisp in dtble.Rows)
                    {

                        string AssignDate2 = drdisp["AssignDate"].ToString();
                        DateTime dtAssignDate2 = Convert.ToDateTime(AssignDate2);
                        string ChangedTime2 = dtAssignDate2.ToString("dd/MMM/yyyy");
                        if (ChangedTime == ChangedTime2)
                        {
                            double assqty = 0;
                            double curdBm = 0;
                            double Buttermilk = 0;
                            double AssignQty = 0;
                            double qtyvalue = 0;
                            double uom = 0;
                            double.TryParse(drdisp["assignedqty"].ToString(), out AssignQty);
                            newrow[drdisp["ProductName"].ToString()] =Math.Round(AssignQty,2);
                            if (drdisp["Categoryname"].ToString() == "MILK")
                            {
                                double.TryParse(drdisp["assignedqty"].ToString(), out assqty);
                                double.TryParse(drdisp["uomqty"].ToString(), out uom);
                                double milkqty = assqty * uom / 1000;
                                total += milkqty;
                            }
                            if (drdisp["Categoryname"].ToString() == "CURD" || drdisp["Categoryname"].ToString() == "Curd Buckets" || drdisp["Categoryname"].ToString() == "Curd Cups")
                            {
                                double.TryParse(drdisp["assignedqty"].ToString(), out curdBm);
                                double.TryParse(drdisp["uomqty"].ToString(), out uom);
                                double curdqty = curdBm * uom / 1000;
                                totalcurdandBM += curdqty;
                            }
                            if (drdisp["Categoryname"].ToString() == "ButterMilk")
                            {
                                double.TryParse(drdisp["assignedqty"].ToString(), out Buttermilk);
                                double.TryParse(drdisp["uomqty"].ToString(), out uom);
                                double buttermilkqty = Buttermilk * uom / 1000;
                                totalcurdandBM += buttermilkqty;
                            }
                        }
                    }
                    double totcans = 0;

                    foreach (DataRow drinv in dtinventry.Rows)
                    {
                         string invAssignDate = drinv["AssignDate"].ToString();
                        DateTime dtinvAssignDate = Convert.ToDateTime(invAssignDate);
                        string invChangedTime2 = dtinvAssignDate.ToString("dd/MMM/yyyy");
                        if (ChangedTime == invChangedTime2)
                        {
                            if (drinv["sno"].ToString() == "1")
                            {

                                double issuedcrates = 0;
                                double.TryParse(drinv["invassigned"].ToString(), out issuedcrates);

                                newrow["Issued Crates"] = issuedcrates;
                            }
                            if (drinv["sno"].ToString() == "4")
                            {
                                double issuedcans = 0;
                                double.TryParse(drinv["invassigned"].ToString(), out issuedcans);
                               // newrow["Issued Cans"] = issuedcans;
                                totcans += issuedcans;
                            }
                            if (drinv["sno"].ToString() == "3")
                            {
                                double issuedcans = 0;
                                double.TryParse(drinv["invassigned"].ToString(), out issuedcans);
                                totcans += issuedcans;
                            }
                            if (drinv["sno"].ToString() == "5")
                            {
                                double issuedcans = 0;
                                double.TryParse(drinv["invassigned"].ToString(), out issuedcans);
                                totcans += issuedcans;
                            }
                            if (drinv["sno"].ToString() == "2")
                            {
                                double issuedcans = 0;
                                double.TryParse(drinv["invassigned"].ToString(), out issuedcans);
                                totcans += issuedcans;
                            }

                        }
                    }
                    newrow["Issued Cans"] = totcans;

                    newrow["Total Milk"] =Math.Round(total,2);
                    newrow["Total Curd&BM"] = Math.Round(totalcurdandBM,2);
                    newrow["Total Lts"] = Math.Round(total + totalcurdandBM,2);
                    Report.Rows.Add(newrow);
                    i++;
                }
                foreach (var column in Report.Columns.Cast<DataColumn>().ToArray())
                {
                    if (Report.AsEnumerable().All(dr => dr.IsNull(column)))
                        Report.Columns.Remove(column);
                }
                DataRow newvartical = Report.NewRow();
               // newvartical["Route Name"] = "Total";
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