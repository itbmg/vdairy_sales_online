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
public partial class AgentwiseDueReport : System.Web.UI.Page
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

                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                FillSalesOffice();
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
                cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno WHERE  (branchdata.CollectionType = @CollectionType) and ((branchmappingtable.SuperBranch = @SuperBranch)  OR (branchdata_1.SalesOfficeID = @SOID))");
                cmd.Parameters.AddWithValue("@SuperBranch", BranchID);
                cmd.Parameters.AddWithValue("@SOID", BranchID);
                cmd.Parameters.AddWithValue("@CollectionType", "Due");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlAgentName.DataSource = dtRoutedata;
                ddlAgentName.DataTextField = "BranchName";
                ddlAgentName.DataValueField = "sno";
                ddlAgentName.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
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
            vdm = new VehicleDBMgr();
             Report = new DataTable();
             pnlHide.Visible = true;
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
            lblDate.Text = txtFromdate.Text;
            ldltdat.Text = txtTodate.Text;

            Session["filename"] = ddlAgentName.SelectedItem.Text + "-> DUE REPORT";
            lblAgent.Text = ddlAgentName.SelectedItem.Text;
            //cmd = new MySqlCommand("SELECT branchroutes.RouteName, productsdata.ProductName, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,indents_subtable.UnitCost, products_category.Categoryname, ROUND(SUM(indents_subtable.LeakQty), 2) AS LeakQty FROM indents INNER JOIN branchroutesubtable ON indents.Branch_id = branchroutesubtable.BranchID INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN  productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE  (indents.I_date >= @starttime) AND (indents.I_date <= @endtime) AND (branchroutes.Sno BETWEEN 29 AND 33)GROUP BY branchroutes.RouteName, productsdata.ProductName, products_category.Categoryname, indents_subtable.LeakQty");
            //cmd = new MySqlCommand("SELECT DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, indents_subtable.DeliveryQty * indents_subtable.UnitCost AS Amount FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno WHERE (branchdata.CollectionType = 'Due') AND (indents.I_date BETWEEN @d1 AND @d2) AND (branchdata.sno = @BranchID) GROUP BY indents.I_date, branchdata.BranchName");
            cmd = new MySqlCommand("SELECT DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, indents_subtable.DeliveryQty * indents_subtable.UnitCost AS Amount, productsdata.ProductName, indents_subtable.DeliveryQty,indents_subtable.UnitCost FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (branchdata.CollectionType = 'Due') AND (indents.I_date BETWEEN @d1 AND @d2) AND (branchdata.sno = @BranchID) and (indents_subtable.Status=@Status) GROUP BY indents.I_date, branchdata.BranchName, productsdata.ProductName, indents_subtable.DeliveryQty");
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@Status", "Delivered");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtdue = vdm.SelectQuery(cmd).Tables[0];
            Report = new DataTable();
            Report.Columns.Add("IndentDate");
            //Report.Columns.Add("Branch Name");
            DataView view = new DataView(dtdue);
            DataTable distinctProduct = view.ToTable(true, "ProductName");
            foreach (DataRow dr in distinctProduct.Rows)
            {
                Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
            }
            Report.Columns.Add("Total  Amount").DataType = typeof(float);
            DataTable distincttable = view.ToTable(true, "BranchName", "IndentDate");
            int i = 1;
            foreach (DataRow branch in distincttable.Rows)
            {
                DataRow newrow = Report.NewRow();
                string IndentDate = branch["IndentDate"].ToString();
                DateTime dtIndentDate = Convert.ToDateTime(IndentDate).AddDays(1);
                string ChangedTime = dtIndentDate.ToString("dd/MMM/yyyy");
                newrow["IndentDate"] = ChangedTime;
                //newrow["Branch Name"] = branch["BranchName"].ToString();
                double Total = 0;
                foreach (DataRow dr in dtdue.Rows)
                {
                    if (branch["IndentDate"].ToString() == dr["IndentDate"].ToString())
                    {
                        if (dr["DeliveryQty"].ToString() != "")
                        {
                            double DeliveryQty = 0;
                            string Dqty = dr["DeliveryQty"].ToString();
                            if (Dqty == "0")
                            {
                            }
                            else
                            {
                                double.TryParse(Dqty, out DeliveryQty);
                                double UnitCost = 0;
                                double.TryParse(dr["UnitCost"].ToString(), out UnitCost);
                                newrow[dr["ProductName"].ToString()] = DeliveryQty;
                                Total += DeliveryQty * UnitCost;
                            }
                        }
                    }
                }
                newrow["Total  Amount"] = Total;
                Report.Rows.Add(newrow);
            }
            foreach (var column in Report.Columns.Cast<DataColumn>().ToArray())
            {
                if (Report.AsEnumerable().All(dr => dr.IsNull(column)))
                    Report.Columns.Remove(column);
            }
            DataRow newvartical = Report.NewRow();
            newvartical["IndentDate"] = "Total";
            float val = 0;
            float.TryParse(Report.Compute("sum([Total  Amount])", "[Total  Amount]<>'0'").ToString(), out val);
            newvartical["Total  Amount"] = val;
            double val1 = 0;
            foreach (DataColumn dc in Report.Columns)
            {
                if (dc.DataType == typeof(Double))
                {
                    val1 = 0.0;
                    double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val1);
                    newvartical[dc.ToString()] = val1;
                }
            }
            Report.Rows.Add(newvartical);
            grdReports.DataSource = Report;
            grdReports.DataBind();
            Session["xportdata"] = Report;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdReports.DataSource = Report;
            grdReports.DataBind();
        }
    }
    protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno WHERE  (branchdata.CollectionType = @CollectionType) and ((branchmappingtable.SuperBranch = @SuperBranch)  OR (branchdata_1.SalesOfficeID = @SOID))");
        cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@CollectionType", "Due");
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlAgentName.DataSource = dtRoutedata;
        ddlAgentName.DataTextField = "BranchName";
        ddlAgentName.DataValueField = "sno";
        ddlAgentName.DataBind();
        
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