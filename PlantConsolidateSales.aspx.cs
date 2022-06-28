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


public partial class PlantConsolidateSales : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["branch"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                lblTitle.Text = Session["TitleName"].ToString();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
    DataTable dtSortedSubCategory = new DataTable();
    DataTable dtSortedCategory = new DataTable();
    DataTable dttempproducts = new DataTable();
    DataTable produtstbl1 = new DataTable();
    DataTable dtSubCatgory = new DataTable();
    DataTable dtCatgory = new DataTable();
    DataTable dtSortedCategoryAndSubCat = new DataTable();

    DataTable dtCatgoryAndSub = new DataTable();


    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            pnlHide.Visible = true;
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
            DataTable tempbranchindentsale = new DataTable();
            cmd = new MySqlCommand("SELECT  TripInfo.Sno, ROUND(SUM(ProductInfo.Qty), 2) AS dispatchqty,DATE_FORMAT(TripInfo.I_date, '%d %b %y') AS I_date, DATE_FORMAT(TripInfo.AssignDate, '%d %b %y') AS AssignDate, TripInfo.BranchName, TripInfo.Branch_Id, TripInfo.GroupId, TripInfo.CompanyId FROM (SELECT tripdata.Sno, tripdata.AssignDate,tripdata.I_date, branchdata_1.BranchName, dispatch.BranchID, dispatch.Branch_Id, dispatch.GroupId, dispatch.CompanyId FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (dispatch.Branch_Id = @BranchID) AND (tripdata.AssignDate BETWEEN @d1 AND @d2)) TripInfo INNER JOIN (SELECT Sno, Qty FROM (SELECT  tripdata_1.Sno, tripsubdata.Qty FROM tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno WHERE (tripdata_1.AssignDate BETWEEN @d1 AND @d2)) TripSubInfo) ProductInfo ON TripInfo.Sno = ProductInfo.Sno GROUP BY TripInfo.Branch_Id,DATE(TripInfo.AssignDate) ORDER BY TripInfo.AssignDate");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtDispatchesbranches = vdm.SelectQuery(cmd).Tables[0];
            if (Session["branch"].ToString() == "172")
            {
                cmd = new MySqlCommand("SELECT   DATE_FORMAT(indents.I_date, '%d %b %y') AS I_date,branchdata.BranchName, ROUND(SUM(indents_subtable.DeliveryQty) ) AS DeliveryQty, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty) ) AS salevalue, branchmappingtable.SuperBranch FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo  WHERE  (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @BranchID) AND (branchmappingtable.SubBranch NOT IN (538,2749, 3928, 1801, 3625)) GROUP BY branchmappingtable.SuperBranch,DATE(indents.I_date) ORDER BY I_date");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                tempbranchindentsale = vdm.SelectQuery(cmd).Tables[0];



                cmd = new MySqlCommand("SELECT DATE_FORMAT(indents.I_date, '%d %b %y') AS I_date, branchdata.BranchName, SUM(indents_subtable.DeliveryQty) AS DeliveryQty, branchmappingtable.SuperBranch, branchmappingtable.SubBranch FROM  branchmappingtable INNER JOIN  branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN   branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN  branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN  indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo WHERE        (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @BranchID) AND (branchmappingtable.SubBranch NOT IN (1801, 3625)) GROUP BY DATE(indents.I_date), branchmappingtable.SubBranch ORDER BY I_date");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                DataTable dt = vdm.SelectQuery(cmd).Tables[0];


            }
            else if (Session["branch"].ToString() == "3625")
            {
                cmd = new MySqlCommand("SELECT   DATE_FORMAT(indents.I_date, '%d %b %y') AS I_date,branchdata.BranchName, ROUND(SUM(indents_subtable.DeliveryQty) ) AS DeliveryQty, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty) ) AS salevalue, branchmappingtable.SuperBranch FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo  WHERE  (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @BranchID) GROUP BY branchmappingtable.SuperBranch,DATE(indents.I_date) ORDER BY I_date");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                tempbranchindentsale = vdm.SelectQuery(cmd).Tables[0];
            }
            else if (Session["branch"].ToString() == "1801")
            {
                cmd = new MySqlCommand("SELECT   DATE_FORMAT(indents.I_date, '%d %b %y') AS I_date,branchdata.BranchName, ROUND(SUM(indents_subtable.DeliveryQty) ) AS DeliveryQty, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty) ) AS salevalue, branchmappingtable.SuperBranch FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo  WHERE  (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @BranchID) GROUP BY branchmappingtable.SuperBranch,DATE(indents.I_date) ORDER BY I_date");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                tempbranchindentsale = vdm.SelectQuery(cmd).Tables[0];
            }
            else if (Session["branch"].ToString() == "7")
            {
                cmd = new MySqlCommand("SELECT   DATE_FORMAT(indents.I_date, '%d %b %y') AS I_date,branchdata.BranchName, ROUND(SUM(indents_subtable.DeliveryQty) ) AS DeliveryQty, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty) ) AS salevalue, branchmappingtable.SuperBranch FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo WHERE  (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @BranchID) AND (branchmappingtable.SubBranch NOT IN (159,4626)) GROUP BY branchmappingtable.SuperBranch,DATE(indents.I_date) ORDER BY I_date");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                tempbranchindentsale = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT  DATE_FORMAT(indents.I_date, '%d %b %y') AS I_date,ROUND(SUM(indents_subtable.DeliveryQty) ) AS DeliveryQty,ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty)) AS salevalue, indents_subtable.UnitCost, branchmappingtable.SuperBranch FROM (SELECT  IndentNo, Branch_id, I_date, Status, IndentType FROM  indents WHERE (I_date BETWEEN @d1 AND @d2) AND (Status <> 'D')) indent INNER JOIN branchdata ON indent.Branch_id = branchdata.sno INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (indents_subtable.DeliveryQty <> 0) GROUP BY branchmappingtable.SuperBranch,DATE(indents.I_date) ORDER BY branchdata.BranchNam,I_date");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                DataTable temptable = vdm.SelectQuery(cmd).Tables[0];
                tempbranchindentsale.Merge(temptable);
            }
            else
            {
                cmd = new MySqlCommand("SELECT   DATE_FORMAT(indents.I_date, '%d %b %y') AS I_date,branchdata.BranchName, ROUND(SUM(indents_subtable.DeliveryQty) ) AS DeliveryQty, ROUND(SUM(indents_subtable.UnitCost * indents_subtable.DeliveryQty) ) AS salevalue, branchmappingtable.SuperBranch FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchmappingtable branchmappingtable_1 ON branchdata.sno = branchmappingtable_1.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable_1.SubBranch = branchdata_1.sno INNER JOIN indents ON branchdata_1.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo WHERE  (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @BranchID) GROUP BY branchmappingtable.SuperBranch,DATE(indents.I_date) ORDER BY I_date");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                tempbranchindentsale = vdm.SelectQuery(cmd).Tables[0];
            }
            tempbranchindentsale = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT  DATE_FORMAT(tripdata.AssignDate, '%d %b %y') AS I_date, tripdata.BranchID AS SuperBranch, SUM(leakages.TotalLeaks) AS Leaks, SUM(leakages.ReturnQty) AS `Return`, tripdata.AssignDate AS Expr1 FROM tripdata INNER JOIN leakages ON tripdata.Sno = leakages.TripID WHERE  (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (tripdata.BranchID = @BranchId) GROUP BY tripdata.BranchID,DATE(tripdata.AssignDate) ORDER BY I_date");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
            DataTable DtLeksAndReturns = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT   DATE_FORMAT(tripdata.AssignDate, '%d %b %y') AS I_date,SUM(leakages.ShortQty) AS ShortQty, SUM(leakages.FreeMilk) AS FreeMilk, tripdata.AssignDate, branchmappingtable.SuperBranch, branchmappingtable.SubBranch FROM leakages INNER JOIN  tripdata ON leakages.TripID = tripdata.Sno INNER JOIN branchmappingtable ON tripdata.BranchID = branchmappingtable.SubBranch WHERE  (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @BranchId) GROUP BY branchmappingtable.SuperBranch,DATE(tripdata.AssignDate) ORDER BY tripdata.AssignDate");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
            DataTable dtShortAndFree = vdm.SelectQuery(cmd).Tables[0];
            if (dtDispatchesbranches.Rows.Count > 0)
            {
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("Date");
                int count = 0;
                Report.Columns.Add("TotalDispatch").DataType = typeof(Double);
                Report.Columns.Add("TotalSale").DataType = typeof(Double); ;
                Report.Columns.Add("TotalLeaks").DataType = typeof(Double); ;
                Report.Columns.Add("TotalReturns").DataType = typeof(Double); ;
                Report.Columns.Add("TotalFree").DataType = typeof(Double); ;
                Report.Columns.Add("TotalShort").DataType = typeof(Double); ;
                Report.Columns.Add("TotalClosing").DataType = typeof(Double); ;
                int i = 1;
                double totalsalevalue = 0;
                DataView view = new DataView(dtDispatchesbranches);
                DataTable distincttable = view.ToTable(true, "I_date");
                
                foreach (DataRow branch in distincttable.Rows)
                {
                    double totalleakreturn = 0;
                    double totalshortfree = 0;
                    DataRow newrow = Report.NewRow();
                    double shortqty = 0;
                    double freeqty = 0;
                    double leakqty = 0;
                    double saleqty = 0;
                    double DispQty = 0;
                    double returnqty = 0;
                    DateTime Date = Convert.ToDateTime(branch["I_date"].ToString()).AddDays(1);
                    string Date1 = Date.ToString("dd MMM yyyy");
                    newrow["Date"] = Date1;
                    foreach (DataRow drrdelivery in dtDispatchesbranches.Select("Branch_Id='" + Session["branch"].ToString() + "'AND i_date='" + branch["I_date"].ToString() + "'"))
                    {
                        double.TryParse(drrdelivery["dispatchqty"].ToString(), out DispQty);
                        newrow["TotalDispatch"] = Math.Round(DispQty, 2);
                    }
                    foreach (DataRow drrdelivery in tempbranchindentsale.Select("SuperBranch='" + Session["branch"].ToString() + "'AND i_date='" + branch["I_date"].ToString() + "'"))
                    {
                        double.TryParse(drrdelivery["DeliveryQty"].ToString(), out saleqty);
                        newrow["TotalSale"] = Math.Round(saleqty, 2); //drrdelivery["DeliveryQty"].ToString();
                    }
                    foreach (DataRow drleaks in DtLeksAndReturns.Select("SuperBranch='" + Session["branch"].ToString() + "'AND i_date='" + branch["I_date"].ToString() + "'"))
                    {
                        double.TryParse(drleaks["Leaks"].ToString(), out leakqty);
                        double.TryParse(drleaks["Return"].ToString(), out returnqty);
                        newrow["TotalLeaks"] = Math.Round(leakqty, 2); //drleaks["Leaks"].ToString();
                        newrow["TotalReturns"] = Math.Round(returnqty, 2); //drleaks["Return"].ToString();
                        totalleakreturn += leakqty;
                        totalleakreturn += returnqty;
                    }
                    foreach (DataRow drfree in dtShortAndFree.Select("SuperBranch='" + Session["branch"].ToString() + "'AND i_date='" + branch["I_date"].ToString() + "'"))
                    {
                        double.TryParse(drfree["ShortQty"].ToString(), out shortqty);
                        double.TryParse(drfree["FreeMilk"].ToString(), out freeqty);
                        newrow["TotalFree"] = Math.Round(freeqty, 2); //drfree["ShortQty"].ToString();
                        newrow["TotalShort"] = Math.Round(shortqty, 2);// drfree["FreeMilk"].ToString();
                        totalshortfree += freeqty;
                        totalshortfree += shortqty;

                    }
                    double mixedqy = saleqty + totalshortfree + totalleakreturn;
                    double Total = DispQty - mixedqy;
                    newrow["TotalClosing"] = Math.Round(Total, 2);
                    Report.Rows.Add(newrow);
                }
                i++;
                foreach (var column in Report.Columns.Cast<DataColumn>().ToArray())
                {
                    if (Report.AsEnumerable().All(dr => dr.IsNull(column)))
                        Report.Columns.Remove(column);
                }
                DataRow newvartical = Report.NewRow();
                newvartical["Date"] = "Total";
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
                if (count == 1)
                {
                    dt.Columns.Add(cell.Text);
                }
                else
                {
                    dt.Columns.Add(cell.Text);
                }
                count++;
            }
            foreach (GridViewRow row in grdReports.Rows)
            {
                dt.Rows.Add();
                for (int i = 1; i < row.Cells.Count; i++)
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
                string FileName = "Branch Wse Net Sale";
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