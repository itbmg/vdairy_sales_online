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

public partial class BranchWiseDispatch : System.Web.UI.Page
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
                FillSalesOffice();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txttodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
            }
        }
    }
    void FillSalesOffice()
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) and (branchdata.flag<>0) ");
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
                cmd = new MySqlCommand("SELECT BranchName, sno FROM  branchdata WHERE (sno = @BranchID) and branchdata.flag<>0");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtPlant.Rows)
                {
                    DataRow newrow = dtBranch.NewRow();
                    newrow["BranchName"] = dr["BranchName"].ToString();
                    newrow["sno"] = dr["sno"].ToString();
                    dtBranch.Rows.Add(newrow);
                }
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType and branchdata.flag<>0)");
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE ((branchdata_1.SalesOfficeID = @SOID) AND (branchdata_1.flag=@flag)) OR ((branchdata.sno = @BranchID) AND (branchdata.flag=@flag))");
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                cmd.Parameters.AddWithValue("@flag", "1");
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
    DataTable dtalldispatch = new DataTable();
    DataTable dtalldelivery = new DataTable();
    DataTable Report = new DataTable();
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
            pnlHide.Visible = true;
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
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
            string[] todatestrig = txttodate.Text.Split(' ');
            if (todatestrig.Length > 1)
            {
                if (todatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = todatestrig[0].Split('-');
                    string[] times = todatestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            Session["RouteName"] = ddlSalesOffice.SelectedItem.Text + " REPORT " + fromdate.AddDays(1).ToString("dd/MM/yyyy");
            Session["filename"] = ddlSalesOffice.SelectedItem.Text + " REPORT " + fromdate.AddDays(1).ToString("dd/MM/yyyy");
            cmd = new MySqlCommand("SELECT dispatch.sno,  SUM(tripsubdata.Qty) AS Qty, DATE_FORMAT(tripdata.AssignDate, '%d %b %y') AS AssignDate, tripsubdata.ProductId, productsdata.ProductName,products_subcategory.SubCatName, products_category.Categoryname FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripsubdata ON triproutes.Tripdata_sno = tripsubdata.Tripdata_sno INNER JOIN tripdata ON tripsubdata.Tripdata_sno = tripdata.Sno INNER JOIN (SELECT branch_sno, product_sno, unitprice, flag, userdata_sno, DTarget, WTarget, MTarget, BranchQty, LeakQty, Rank FROM branchproducts WHERE (branch_sno = @salesoffice)) brnchprdts ON tripsubdata.ProductId = brnchprdts.product_sno INNER JOIN productsdata ON brnchprdts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.BranchID = @salesoffice) AND (tripdata.AssignDate BETWEEN @d1 AND @d2) GROUP BY tripsubdata.ProductId, tripdata.AssignDate ORDER BY AssignDate, brnchprdts.Rank");
            cmd.Parameters.AddWithValue("@salesoffice", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable dtsalesofficeTotalDispatch = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT products_category.Categoryname,branchproducts.product_sno, productsdata.ProductName, products_subcategory.SubCatName,branchproducts.unitprice FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @salesoffice) ORDER BY branchproducts.Rank");
            cmd.Parameters.AddWithValue("@salesoffice", ddlSalesOffice.SelectedValue);
            DataTable dtsalesofficeproducts = vdm.SelectQuery(cmd).Tables[0];

            if (dtsalesofficeTotalDispatch.Rows.Count > 0)
            {
                DataView view = new DataView(dtsalesofficeTotalDispatch);

                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("DC Date");
                foreach (DataRow dr in dtsalesofficeproducts.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                }
                Report.Columns.Add("Total MIlk").DataType = typeof(Double);
                Report.Columns.Add("Total Curd & BM").DataType = typeof(Double);
                Report.Columns.Add("Total Ltrs").DataType = typeof(Double);
                DataTable distincttable = view.ToTable(true, "AssignDate");
                int i = 1;
                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    string dtdate1 = branch["AssignDate"].ToString();
                    DateTime dtDOE1 = Convert.ToDateTime(dtdate1);
                    string ChangedTime1 = dtDOE1.ToString("dd/MMM/yyyy");
                    newrow["DC Date"] = ChangedTime1;

                    double total = 0;
                    double Amount = 0;
                    double totalmilk = 0;
                    double totalcurdandBM = 0;
                    double totalltrs = 0;
                    foreach (DataRow dr in dtsalesofficeTotalDispatch.Rows)
                    {
                        if (branch["AssignDate"].ToString() == dr["AssignDate"].ToString())
                        {
                            double qtyvalue = 0;
                            double UnitCost = 0;
                            double DQty = 0;
                            double assqty = 0;
                            double curdBm = 0;
                            double Buttermilk = 0;
                            double AssignQty = 0;
                            double.TryParse(dr["Qty"].ToString(), out DQty);
                            newrow[dr["ProductName"].ToString()] =Math.Round(DQty,2);
                            if (dr["Categoryname"].ToString() == "MILK")
                            {
                                double.TryParse(dr["Qty"].ToString(), out assqty);
                                totalmilk += assqty;
                            }
                            if (dr["Categoryname"].ToString() == "CURD")
                            {
                                double.TryParse(dr["Qty"].ToString(), out curdBm);
                                totalcurdandBM += curdBm;
                            }
                            if (dr["Categoryname"].ToString() == "ButterMilk")
                            {
                                double.TryParse(dr["Qty"].ToString(), out Buttermilk);
                                totalcurdandBM += Buttermilk;
                            }

                            double.TryParse(dr["Qty"].ToString(), out qtyvalue);
                            total += qtyvalue;
                        }
                    }
                    newrow["Total Ltrs"] = Math.Round(total,2);
                    newrow["Total MIlk"] =Math.Round(totalmilk,2);
                    newrow["Total Curd & BM"] =Math.Round(totalcurdandBM,2);
                    Report.Rows.Add(newrow);
                    i++;
                }
                foreach (var column in Report.Columns.Cast<DataColumn>().ToArray())
                {
                    if (Report.AsEnumerable().All(dr => dr.IsNull(column)))
                        Report.Columns.Remove(column);
                }
                DataRow newvartical = Report.NewRow();
                newvartical["DC Date"] = "Total";
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
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
            }
        }
        catch(Exception ex)
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
}