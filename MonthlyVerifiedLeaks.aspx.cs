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


public partial class MonthlyVerifiedLeaks : System.Web.UI.Page
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

                FillAgentName();

            }
        }
    }
    void FillAgentName()
    {

        vdm = new VehicleDBMgr();
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Plant")
            {
                PBranch.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchdata.flag = 1) AND (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                cmd.Parameters.AddWithValue("@SalesType", "21");
                cmd.Parameters.AddWithValue("@SalesType1", "26");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
            }
            else
            {
                PBranch.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE  (branchdata.flag = 1) AND  (branchdata_1.SalesOfficeID = @SOID) OR (branchdata.sno = @BranchID)");
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
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
            Session["filename"] = "Monthly Verified Leaks Report ->" + ddlSalesOffice.SelectedItem.Text;
            lblAgent.Text = ddlSalesOffice.SelectedItem.Text;
            lbl_fromDate.Text = txtFromdate.Text;
            lbl_selttodate.Text = txtTodate.Text;
            string BranchID = ddlSalesOffice.SelectedValue;
            if (BranchID == "572")
            {
                BranchID = "7";
            }

            //cmd = new MySqlCommand("SELECT SUM(leakages.VLeaks) AS vleaks, SUM(leakages.VReturns) AS vreturns, SUM(leakages.TotalLeaks) AS totleaks, SUM(leakages.ReturnQty) AS returnqty,tripdata.Sno AS tripsno, tripdata.AssignDate AS AssignDate, tripdata.ReturnDCTime FROM triproutes INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN branchdata ON dispatch.BranchID = branchdata.sno WHERE (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (branchdata.sno = @brnchid) GROUP BY tripdata.AssignDate ORDER BY AssignDate");
            //RAVINDRA
            cmd = new MySqlCommand("SELECT SUM(Leaks.VLeaks) AS vleaks, SUM(Leaks.VReturns) AS vreturns, SUM(Leaks.TotalLeaks) AS totleaks, SUM(Leaks.ReturnQty) AS returnqty, Leaks.AssignDate,Leaks.ReturnDCTime FROM (SELECT leakages.VLeaks, leakages.VReturns, leakages.TotalLeaks, productsdata.ProductName, tripdata_1.Sno, leakages.ReturnQty, tripdata_1.AssignDate,tripdata_1.ReturnDCTime FROM tripdata tripdata_1 INNER JOIN leakages ON tripdata_1.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata_1.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno FROM (SELECT dispatch.DispName, tripdata.Sno, dispatch.sno AS DespSno FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @BranchID)) TripInfo) ff ON ff.Sno = Leaks.Sno GROUP BY Leaks.AssignDate ORDER BY Leaks.AssignDate");
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtAgent = vdm.SelectQuery(cmd).Tables[0];
            
                DataView view = new DataView(dtAgent);
                //DataTable distinctproducts = view.ToTable(true, "ProductName");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("DC Date");
                Report.Columns.Add("ReturnDC Date");
                Report.Columns.Add("Submit Leaks", typeof(Double));
                Report.Columns.Add("Verified Leaks", typeof(Double));
                Report.Columns.Add("Difference Leaks", typeof(Double));
                Report.Columns.Add("Submit Returns", typeof(Double));
                Report.Columns.Add("Verified Returns", typeof(Double));
                Report.Columns.Add("Difference Returns", typeof(Double));
                DataTable distincttable = view.ToTable(true, "AssignDate", "ReturnDCTime");
                int i = 1;
                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    string dtdate1 = branch["AssignDate"].ToString();
                    string dtdate2 = branch["ReturnDCTime"].ToString();
                    DateTime dtDOE1 = Convert.ToDateTime(dtdate1);
                    DateTime dtDOE2 = Convert.ToDateTime(dtdate2);
                    string ChangedTime1 = dtDOE1.ToString("dd/MMM/yyyy");
                    string ChangedTime2 = dtDOE2.ToString("dd/MMM/yyyy");
                    newrow["DC Date"] = ChangedTime1;
                    newrow["ReturnDC Date"] = ChangedTime2;

                    double total = 0;
                    double Amount = 0;
                    foreach (DataRow dr in dtAgent.Rows)
                    {
                        if (branch["AssignDate"].ToString() == dr["AssignDate"].ToString())
                        {
                            double submitleaks = 0;
                            double verifiedleaks = 0;
                            double submitreturns = 0;
                            double verifiedreturns = 0;
                            double.TryParse(dr["totleaks"].ToString(), out submitleaks);
                            double.TryParse(dr["vleaks"].ToString(), out verifiedleaks);
                            double.TryParse(dr["returnqty"].ToString(), out submitreturns);
                            double.TryParse(dr["vreturns"].ToString(), out verifiedreturns);
                            newrow["Submit Leaks"] = Math.Round(submitleaks,2);
                            newrow["Verified Leaks"] = Math.Round(verifiedleaks, 2);
                            newrow["Difference Leaks"] = Math.Round(submitleaks - verifiedleaks, 2);
                            newrow["Submit Returns"] = Math.Round(submitreturns, 2);
                            newrow["Verified Returns"] = Math.Round(verifiedreturns, 2);
                            newrow["Difference Returns"] = Math.Round(submitreturns - verifiedreturns, 2);

                        }
                    }
                    Report.Rows.Add(newrow);
                    i++;

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
        catch(Exception ex)
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