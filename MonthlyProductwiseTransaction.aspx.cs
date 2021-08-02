using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using ClosedXML.Excel;
using System.IO;

public partial class MonthlyProductwiseTransaction : System.Web.UI.Page
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
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
    private DateTime GetLowMonthRetrive(DateTime dt)
    {
        double Day, Hour, Min, Sec;
        DateTime DT = dt;
        DT = dt;
        Day = -dt.Day + 1;
        Hour = -dt.Hour;
        Min = -dt.Minute;
        Sec = -dt.Second;
        DT = DT.AddDays(Day);
        DT = DT.AddHours(Hour);
        DT = DT.AddMinutes(Min);
        DT = DT.AddSeconds(Sec);
        return DT;

    }
    private DateTime GetHighMonth(DateTime dt)
    {
        double Day, Hour, Min, Sec;
        DateTime DT = DateTime.Now;
        Day = 31 - dt.Day;
        Hour = 23 - dt.Hour;
        Min = 59 - dt.Minute;
        Sec = 59 - dt.Second;
        DT = dt;
        DT = DT.AddDays(Day);
        DT = DT.AddHours(Hour);
        DT = DT.AddMinutes(Min);
        DT = DT.AddSeconds(Sec);
        if (DT.Day == 3)
        {
            DT = DT.AddDays(-3);
        }
        else if (DT.Day == 2)
        {
            DT = DT.AddDays(-2);
        }
        else if (DT.Day == 1)
        {
            DT = DT.AddDays(-1);
        }
        return DT;
    }
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            pnlHide.Visible = true;
            Report = new DataTable();
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
            //fromdate = fromdate.AddDays(-1);
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
            todate = todate.AddMonths(1);
            Session["filename"] = "Product Wise Monthly Transaction"; DateTime firstmonth = new DateTime();
            DateTime lastmonth = new DateTime();
            lbl_fromDate.Text = txtFromdate.Text;
            lbl_selttodate.Text = txtTodate.Text;
            int frommonth = fromdate.Month;
            int tomonth = todate.Month;
            int NextValue = 0;
            TimeSpan dateSpan = todate.Subtract(fromdate);
            int years = (dateSpan.Days / 365);
            int months = ((dateSpan.Days % 365) / 31) + (years * 12);
            Report.Columns.Add("SNo");
            Report.Columns.Add("Product Name");
            int Count = 2;
            if (months != 0)
            {
                for (int j = 0; j < months; j++)
                {
                    firstmonth = GetLowMonthRetrive(fromdate.AddMonths(j));
                    lastmonth = GetHighMonth(firstmonth);
                    cmd = new MySqlCommand("SELECT productsdata.ProductName, ROUND(SUM(tripsubdata.Qty), 2) AS Qty, tripdata.I_Date FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE (dispatch.Branch_Id = @BranchID) AND (tripdata.AssignDate BETWEEN @d1 AND @d2) GROUP BY productsdata.ProductName");
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                    DateTime dtF = firstmonth;
                    cmd.Parameters.AddWithValue("@d1", dtF);
                    cmd.Parameters.AddWithValue("@d2", lastmonth);
                    DataTable dtAgent = vdm.SelectQuery(cmd).Tables[0];
                    string ChangedTime1 = firstmonth.ToString("MMM/yyyy");
                    if (Report.Columns.Count == 2)
                    {

                        int i = 0;
                        Report.Columns.Add(ChangedTime1).DataType = typeof(Double);
                        foreach (DataRow dr in dtAgent.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow["SNo"] = i++.ToString();
                            newrow["Product Name"] = dr["ProductName"].ToString();
                            string DOE = dr["I_date"].ToString();
                            DateTime dtDOE = Convert.ToDateTime(DOE).AddDays(1);
                            string ChangedTime = dtDOE.ToString("MMM/yyyy");
                            double delQty = 0;
                            double.TryParse(dr["Qty"].ToString(), out delQty);
                            newrow[ChangedTime] = delQty;
                            Report.Rows.Add(newrow);
                        }
                        Count++;
                        NextValue = i;
                    }
                    else
                    {
                        int k = 0;
                        DataColumn Col = Report.Columns.Add(ChangedTime1, System.Type.GetType("System.Double"));
                        Col.SetOrdinal(Count);
                        foreach (DataRow dr in dtAgent.Rows)
                        {
                            
                            try
                            {
                                Report.Rows[k][Count] = dr["Qty"].ToString();
                            }
                            catch
                            {
                                string ProductName= dr["ProductName"].ToString();
                                DataRow newrow = Report.NewRow();
                                newrow["SNo"] = NextValue++.ToString();
                                newrow["Product Name"] = dr["ProductName"].ToString();
                                string DOE = dr["I_date"].ToString();
                                DateTime dtDOE = Convert.ToDateTime(DOE).AddDays(1);
                                string ChangedTime = dtDOE.ToString("MMM/yyyy");
                                double delQty = 0;
                                double.TryParse(dr["Qty"].ToString(), out delQty);
                                newrow[ChangedTime] = delQty;
                                Report.Rows.Add(newrow);
                            }
                            k++;

                        }
                        Count++;
                    }
                }
                DataRow newvartical = Report.NewRow();
                newvartical["Product Name"] = "Total";
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
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
            }
            else
            {
                pnlHide.Visible = false;
                lblmsg.Text = "Please Select atleast two months";
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