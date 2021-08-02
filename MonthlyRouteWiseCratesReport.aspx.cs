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
public partial class MonthlyRouteWiseCratesReport : System.Web.UI.Page
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
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                lblTitle.Text = Session["TitleName"].ToString();
                //lblDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) OR (branchdata.sno = @BranchID)");
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
            Session["filename"] = "Monthly Route Wise Crates Report ->" + ddlSalesOffice.SelectedItem.Text;
            lblAgent.Text = ddlSalesOffice.SelectedItem.Text;
            lbl_fromDate.Text = txtFromdate.Text;
            lbl_selttodate.Text = txtTodate.Text;
            string BranchID = ddlSalesOffice.SelectedValue;
            if (BranchID == "572")
            {
                BranchID = "158";
            }
            cmd = new MySqlCommand("SELECT SUM(tripinvdata.Qty) AS issued, SUM(tripinvdata.Remaining) AS returnqty, invmaster.InvName, invmaster.sno, dispatch.sno AS dispatchsno, dispatch.DispName FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripinvdata ON tripdat.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (dispatch.Branch_Id = @brnchid) GROUP BY dispatch.sno, invmaster.sno");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@brnchid", BranchID);
            DataTable dtInventory = vdm.SelectQuery(cmd).Tables[0];
            if (dtInventory.Rows.Count > 0)
            {
                DataView view = new DataView(dtInventory);
                Report.Columns.Add("Route Name");
                Report.Columns.Add("Issued crates").DataType = typeof(Int32);
                Report.Columns.Add("Return crates").DataType = typeof(Int32);
                Report.Columns.Add("Difference crates").DataType = typeof(Int32);
                Report.Columns.Add("Issued can 20ltr").DataType = typeof(Int32);
                Report.Columns.Add("Return can 20ltr").DataType = typeof(Int32);
                Report.Columns.Add("Difference can 20ltr").DataType = typeof(Int32);
                Report.Columns.Add("Issued can 40ltr").DataType = typeof(Int32);
                Report.Columns.Add("Return can 40ltr").DataType = typeof(Int32);
                Report.Columns.Add("Difference can 40ltr").DataType = typeof(Int32);
                int i = 1;
                int k = 0;
                DataTable distincttable = view.ToTable(true, "DispName", "dispatchsno");
                foreach (DataRow drinv in distincttable.Rows)
                {
                    DataRow drnew = Report.NewRow();
                    string dtdate1 = drinv["dispatchsno"].ToString();
                    string dispatchname = drinv["DispName"].ToString();
                    drnew["Route Name"] = dispatchname;
                    foreach (DataRow drinvc in dtInventory.Rows)
                    {
                        string dtdate2 = drinvc["dispatchsno"].ToString();
                        if (dtdate1 == dtdate2)
                        {
                            if (drinvc["sno"].ToString() == "1")
                            {
                                int issuedcrates = 0;
                                int receivedcrates = 0;
                                int.TryParse(drinvc["issued"].ToString(), out issuedcrates);
                                int.TryParse(drinvc["returnqty"].ToString(), out receivedcrates);
                                drnew["Issued crates"] = drinvc["issued"].ToString();
                                drnew["Return crates"] = drinvc["returnqty"].ToString();
                                drnew["Difference crates"] = issuedcrates - receivedcrates;
                            }
                            if (drinvc["sno"].ToString() == "3")
                            {
                                int issuedcan20ltr = 0;
                                int receivedcan20ltr = 0;
                                int.TryParse(drinvc["issued"].ToString(), out issuedcan20ltr);
                                int.TryParse(drinvc["returnqty"].ToString(), out receivedcan20ltr);
                                drnew["Issued can 20ltr"] = drinvc["issued"].ToString();
                                drnew["Return can 20ltr"] = drinvc["returnqty"].ToString();
                                drnew["Difference can 20ltr"] = issuedcan20ltr - receivedcan20ltr;
                            }
                            if (drinvc["sno"].ToString() == "4")
                            {
                                int issuedcan40ltr = 0;
                                int receivedcan40ltr = 0;
                                int.TryParse(drinvc["issued"].ToString(), out issuedcan40ltr);
                                int.TryParse(drinvc["returnqty"].ToString(), out receivedcan40ltr);
                                drnew["Issued can 40ltr"] = drinvc["issued"].ToString();
                                drnew["Return can 40ltr"] = drinvc["returnqty"].ToString();
                                drnew["Difference can 40ltr"] = issuedcan40ltr - receivedcan40ltr;
                            }

                        }
                    }
                    Report.Rows.Add(drnew);
                    k++;
                }
                DataRow newvartical = Report.NewRow();
                newvartical["Route Name"] = "Total";
                int val = 0;
                foreach (DataColumn dc in Report.Columns)
                {
                    if (dc.DataType == typeof(Int32))
                    {
                        val = 0;
                        Int32.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
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
                lblmsg.Text = "No Data Found";
                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
        }
        catch (Exception ex)
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