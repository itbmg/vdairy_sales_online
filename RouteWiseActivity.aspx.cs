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

public partial class RouteWiseActivity : System.Web.UI.Page
{
     MySqlCommand cmd;
    string BranchID = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["branch"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            BranchID = Session["branch"].ToString();
        }
        //vdm = new VehicleDBMgr();
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                lblTitle.Text = Session["TitleName"].ToString();
                //lblDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                FillAgentName();
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            }
        }
    }
    void FillAgentName()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Plant")
            {
                PBranch.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                cmd.Parameters.AddWithValue("@SalesType", "21");
                cmd.Parameters.AddWithValue("@SalesType1", "26");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));

            }
            else
            {
                PBranch.Visible = false;
                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID) AND (dispatch.flag=@flag)) OR ((branchdata_1.SalesOfficeID = @SOID) AND (dispatch.flag=@flag))");
                //cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@flag", "1");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlDispName.DataSource = dtRoutedata;
                ddlDispName.DataTextField = "DispName";
                ddlDispName.DataValueField = "sno";
                ddlDispName.DataBind();
                ddlDispName.Items.Insert(0, new ListItem("Select", "0"));
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
        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID) AND (dispatch.flag=@flag)) OR ((branchdata_1.SalesOfficeID = @SOID) AND (dispatch.flag=@flag))");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@flag", "1");

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
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            vdm = new VehicleDBMgr();
            DataTable Report = new DataTable();
            DateTime fromdate = DateTime.Now;
            string[] fromdatestrig = txtFromdate.Text.Split(' ');
            if (fromdatestrig.Length > 1)
            {
                if (fromdatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = fromdatestrig[0].Split('-');
                    string[] times = fromdatestrig[1].Split(':');
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
            Session["filename"] = "Route wise Activity Report ->" + ddlDispName.SelectedItem.Text;
            lblAgent.Text = ddlDispName.SelectedItem.Text;
            lbl_fromDate.Text = txtFromdate.Text;
            lbl_selttodate.Text = txtTodate.Text;
            string BranchID = ddlSalesOffice.SelectedValue;
            if (BranchID == "572")
            {
                BranchID = "158";
            }
            //cmd = new MySqlCommand("SELECT Round(sum(indents_subtable.DeliveryQty),2) as DeliveryQty, indents_subtable.UnitCost, products_category.Categoryname, productsdata.ProductName, DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN tripdata ON indents_subtable.DTripId = tripdata.Sno INNER JOIN triproutes ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (dispatch.sno = @dispatchsno) GROUP BY tripdata.Sno, productsdata.ProductName, products_category.Categoryname");
            cmd = new MySqlCommand("SELECT dispath.DispName, ROUND(SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost), 2) AS salevalue, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.UnitCost, DATE_FORMAT(tripdat.I_Date, '%d %b %y') AS IndentDate, productsdata.ProductName,products_category.Categoryname FROM (SELECT sno, DispName FROM dispatch WHERE (sno = @dispatchsno)) dispath INNER JOIN triproutes ON dispath.sno = triproutes.RouteID INNER JOIN (SELECT Sno, I_Date, Status FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN indents_subtable ON tripdat.Sno = indents_subtable.DTripId INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno GROUP BY tripdat.Sno, productsdata.ProductName, products_category.Categoryname ORDER BY tripdat.I_Date");
            cmd.Parameters.AddWithValue("@dispatchsno", ddlDispName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtAgent = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT products_category.Categoryname, products_subcategory.SubCatName, productsdata.ProductName, productsdata.Units, invmaster.Qty FROM branchproducts INNER JOIN dispatch ON branchproducts.branch_sno = dispatch.Branch_Id INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno WHERE (dispatch.sno = @dispatchSno) GROUP BY productsdata.ProductName ORDER BY productsdata.Rank");
            cmd.Parameters.AddWithValue("@dispatchSno", ddlDispName.SelectedValue);
            DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
            if (produtstbl.Rows.Count > 0)
            {
                DataView view = new DataView(dtAgent);
                DataTable distinctproducts = view.ToTable(true, "ProductName", "Categoryname");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("IndentDate");
                foreach (DataRow dr in distinctproducts.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                }
                Report.Columns.Add("Total").DataType = typeof(Double);
                Report.Columns.Add("Sale Value").DataType = typeof(Double);
                DataTable distincttable = view.ToTable(true, "IndentDate");
                int i = 1;
                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    string dtdate1 = branch["IndentDate"].ToString();
                    DateTime dtDOE1 = Convert.ToDateTime(dtdate1).AddDays(1);
                    string ChangedTime1 = dtDOE1.ToString("dd/MMM/yyyy");
                    newrow["IndentDate"] = ChangedTime1;

                    double total = 0;
                    double Amount = 0;
                    foreach (DataRow dr in dtAgent.Rows)
                    {
                        if (branch["IndentDate"].ToString() == dr["IndentDate"].ToString())
                        {
                            double qtyvalue = 0;
                            double salevalue = 0;
                            double DQty = 0;
                            double.TryParse(dr["DeliveryQty"].ToString(), out DQty);
                            newrow[dr["ProductName"].ToString()] = DQty;
                            //if (dr["Categoryname"].ToString() == "MILK")
                            //{
                                double.TryParse(dr["DeliveryQty"].ToString(), out qtyvalue);
                                double.TryParse(dr["salevalue"].ToString(), out salevalue);
                                Amount += salevalue;
                                total += qtyvalue;
                            //}
                        }
                    }
                    newrow["Total"] = total;
                    newrow["Sale Value"] = Amount;
                    Report.Rows.Add(newrow);
                    i++;
                }
                DataRow newvartical = Report.NewRow();
                newvartical["IndentDate"] = "Total";
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
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
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
            foreach (TableCell cell in grdReports.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
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