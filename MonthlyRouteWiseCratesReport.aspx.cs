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
                BranchID = "7";
            }
            //cmd = new MySqlCommand("SELECT SUM(tripinvdata.Qty) AS issued, SUM(tripinvdata.Remaining) AS returnqty, invmaster.InvName, invmaster.sno, dispatch.sno AS dispatchsno, dispatch.DispName FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripinvdata ON tripdat.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (dispatch.Branch_Id = @brnchid) GROUP BY dispatch.sno, invmaster.sno");
            //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            //cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            //cmd.Parameters.AddWithValue("@brnchid", BranchID);
            //DataTable dtInventory = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName, branchroutes.RouteName,branchroutes.sno as RouteSno FROM branchdata INNER JOIN branchroutesubtable ON branchdata.sno = branchroutesubtable.BranchID INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno WHERE (branchmappingtable.SuperBranch = @bid) AND (branchdata.SalesType <> '21') AND (branchdata.flag = 1)");
            cmd.Parameters.AddWithValue("@bid", BranchID);
            DataTable dtbranch = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT invtras.TransType, invtras.FromTran, invtras.ToTran, SUM(invtras.Qty) as Qty,invmaster.sno AS invsno, invmaster.InvName FROM  invtransactions12 as invtras INNER JOIN invmaster ON invtras.B_inv_sno = invmaster.sno INNER JOIN branchmappingtable ON branchmappingtable.SubBranch=invtras.ToTran  WHERE branchmappingtable.SuperBranch=@FromTran and invtras.DOE between @d1 and @d2 GROUP by branchmappingtable.SubBranch ORDER BY invtras.DOE");
            cmd.Parameters.AddWithValue("@FromTran", BranchID);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable dtinventaryissued = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT invtras.TransType, invtras.FromTran, invtras.ToTran, SUM(invtras.Qty) as Qty,invmaster.sno AS invsno, invmaster.InvName FROM  invtransactions12 as invtras INNER JOIN invmaster ON invtras.B_inv_sno = invmaster.sno INNER JOIN branchmappingtable ON branchmappingtable.SubBranch=invtras.FromTran WHERE branchmappingtable.SuperBranch=@FromTran and invtras.DOE between @d1 and @d2 GROUP by branchmappingtable.SubBranch ORDER BY invtras.DOE");
            cmd.Parameters.AddWithValue("@FromTran", BranchID);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable dtinventaryreceived = vdm.SelectQuery(cmd).Tables[0];
            dtinventaryissued.Merge(dtinventaryreceived);
            if (dtinventaryissued.Rows.Count > 0)
            {
                DataView view = new DataView(dtbranch);
                Report.Columns.Add("Route Name");
                Report.Columns.Add("Issued crates").DataType = typeof(Int32);
                Report.Columns.Add("Return crates").DataType = typeof(Int32);
                Report.Columns.Add("Difference crates").DataType = typeof(Int32);
                //Report.Columns.Add("Issued can 20ltr").DataType = typeof(Int32);
                //Report.Columns.Add("Return can 20ltr").DataType = typeof(Int32);
                //Report.Columns.Add("Difference can 20ltr").DataType = typeof(Int32);
                //Report.Columns.Add("Issued can 40ltr").DataType = typeof(Int32);
                //Report.Columns.Add("Return can 40ltr").DataType = typeof(Int32);
                //Report.Columns.Add("Difference can 40ltr").DataType = typeof(Int32);
                int i = 1;
                int k = 0;
                DataTable distincttable = view.ToTable(true, "BranchName", "sno", "RouteName", "RouteSno");
                DataTable distinct_route = view.ToTable(true, "RouteName", "RouteSno");
                double Ctotreccrates = 0;
                double Dtotissuecrates = 0;
                foreach (DataRow drrouteinv in distinct_route.Rows)
                {
                    string Route_Name = drrouteinv["RouteName"].ToString();
                    DataRow drnew = Report.NewRow();
                    drnew["Route Name"] = Route_Name;
                    foreach (DataRow drinv in distincttable.Select("RouteSno='" + drrouteinv["RouteSno"].ToString() + "'"))
                    {
                        foreach (DataRow dr in dtinventaryissued.Select("ToTran='" + drinv["sno"].ToString() + "'"))
                        {
                            if (dr["TransType"].ToString() == "2")
                            {
                                if (dr["invsno"].ToString() == "1")
                                {
                                    double Dcrates = 0;
                                    double.TryParse(dr["Qty"].ToString(), out Dcrates);
                                    //newrow[dr["InvName"].ToString()] = Dcrates;
                                    Dtotissuecrates += Dcrates;
                                    //totalissueqty += Dcrates;
                                }
                            }
                        }
                        foreach (DataRow drr in dtinventaryissued.Select("FromTran='" + drinv["sno"].ToString() + "'"))
                        {
                            if (drr["TransType"].ToString() == "1" || drr["TransType"].ToString() == "3")
                            {
                                if (drr["invsno"].ToString() == "1")
                                {
                                    int Ccrates = 0;
                                    int.TryParse(drr["Qty"].ToString(), out Ccrates);
                                    //newrow[drr["InvName"].ToString()] = Ccrates;
                                    Ctotreccrates += Ccrates;
                                    //totreceivedqty += Ccrates;
                                }
                            }
                        }
                    }
                    drnew["Issued crates"] = Dtotissuecrates;
                    drnew["Return crates"] = Ctotreccrates;
                    drnew["Difference crates"] = Dtotissuecrates - Ctotreccrates;
                    Report.Rows.Add(drnew);
                    Ctotreccrates = 0;
                    Dtotissuecrates = 0;
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