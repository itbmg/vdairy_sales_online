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
using System.Globalization;
using System.Drawing;

public partial class PuffWiseInventory : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                FillSalesOffice();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchdata.flag = 1) AND (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
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
                PBranch.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata.flag = 1) AND (branchdata_1.SalesOfficeID = @SOID) OR (branchdata.sno = @BranchID)");
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
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Plant")
            {
                lblDispatchName.Text = ddlSalesOffice.SelectedItem.Text;
                lblDate.Text = txtdate.Text;
            }
            else
            {
                lblDispatchName.Text = "SALES OFFICE";
                lblDate.Text = txtdate.Text;
            }
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
            Session["filename"] = ddlSalesOffice.SelectedItem.Text + " REPORT " + fromdate.AddDays(1).ToString("dd/MM/yyyy");
            pnlHide.Visible = true;
            cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno, dispatch.BranchID, tripdata.I_Date FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2) GROUP BY dispatch.DispName");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtDispnames = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT clotrans.BranchId, invmaster.InvName, invmaster.sno, closubtraninventory.StockQty FROM clotrans INNER JOIN closubtraninventory ON clotrans.Sno = closubtraninventory.RefNo INNER JOIN invmaster ON closubtraninventory.InvSno = invmaster.sno WHERE (clotrans.BranchId = @BranchID) AND (clotrans.IndDate BETWEEN @d1 AND @d2) GROUP BY invmaster.sno");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtClo = vdm.SelectQuery(cmd).Tables[0];
            Report = new DataTable();
            Report.Columns.Add("Sno");
            Report.Columns.Add("Dispatch Name");
            Report.Columns.Add("Received Crates", typeof(Double));
            Report.Columns.Add("Return Crates", typeof(Double));
            Report.Columns.Add("Difference Crates", typeof(Double));
            Report.Columns.Add("Received Can40ltr", typeof(Double));
            Report.Columns.Add("Return Can40ltr", typeof(Double));
            Report.Columns.Add("Difference Can40ltr", typeof(Double));
            Report.Columns.Add("Received Can20ltr", typeof(Double));
            Report.Columns.Add("Return Can20ltr", typeof(Double));
            Report.Columns.Add("Difference Can20ltr", typeof(Double));
            int i = 1;
            int cratesreceived = 0;
            int cratesreturned = 0;
            int can20received = 0;
            int can20returned = 0;
            int can40received = 0;
            int can40returned = 0;
            foreach (DataRow drSub in dtDispnames.Rows)
            {
                cmd = new MySqlCommand("SELECT triproutes.Tripdata_sno, tripinvdata.Qty, tripinvdata.Remaining, invmaster.InvName, invmaster.sno FROM tripdata INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripinvdata ON tripdata.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.sno = @dispatchSno)"); 
                cmd.Parameters.AddWithValue("@dispatchSno", drSub["sno"].ToString());
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable DtTripSubData = vdm.SelectQuery(cmd).Tables[0];
                string Disp = drSub["DispName"].ToString();
                string[] strName = Disp.Split('_');
                DataRow newSo = Report.NewRow();

                
                DataRow drinventry = Report.NewRow();
                drinventry["Sno"] = i;
                drinventry["Dispatch Name"] = Disp;
                int Ctotcan40ltr = 0;
                int Ctotcrates = 0;
                int Ctotcan20ltr = 0;
                int Dtotcrates = 0;
                int Dtotcan20ltr = 0;
                int Dtotcan40ltr = 0;

                foreach (DataRow drtripinv in DtTripSubData.Rows)
                {
                    if (drtripinv["sno"].ToString() == "1")
                    {
                        int Dcrates = 0;
                        int.TryParse(drtripinv["Qty"].ToString(), out Dcrates);
                        Dtotcrates += Dcrates;

                        int Ccrates = 0;
                        int.TryParse(drtripinv["Remaining"].ToString(), out Ccrates);
                        Ctotcrates += Ccrates;
                    }
                    if (drtripinv["sno"].ToString() == "3")
                    {
                        int Dcan20ltr = 0;
                        int.TryParse(drtripinv["Qty"].ToString(), out Dcan20ltr);
                        Dtotcan20ltr += Dcan20ltr;

                        int Ccan20ltr = 0;
                        int.TryParse(drtripinv["Remaining"].ToString(), out Ccan20ltr);
                        Ctotcan20ltr += Ccan20ltr;

                    }
                    if (drtripinv["sno"].ToString() == "4")
                    {
                        int Dcan40ltr = 0;
                        int.TryParse(drtripinv["Qty"].ToString(), out Dcan40ltr);
                        Dtotcan40ltr += Dcan40ltr;

                        int Ccan40ltr = 0;
                        int.TryParse(drtripinv["Remaining"].ToString(), out Ccan40ltr);
                        Ctotcan40ltr += Ccan40ltr;

                    }
                }

               
                drinventry["Received Crates"] = Dtotcrates;
                drinventry["Return Crates"] = Ctotcrates;
                drinventry["Difference Crates"] = Dtotcrates - Ctotcrates;
                drinventry["Received Can40ltr"] = Dtotcan40ltr;
                drinventry["Return Can40ltr"] = Ctotcan40ltr;
                drinventry["Difference Can40ltr"] = Dtotcan40ltr - Ctotcan40ltr;
                drinventry["Received Can20ltr"] = Dtotcan20ltr;
                drinventry["Return Can20ltr"] = Ctotcan20ltr;
                drinventry["Difference Can20ltr"] = Dtotcan20ltr - Ctotcan20ltr;
                Report.Rows.Add(drinventry);
                i++;
                cratesreceived += Dtotcrates;
                cratesreturned += Ctotcrates;
                can20received += Dtotcan20ltr;
                can20returned += Ctotcan20ltr;
                can40received += Dtotcan40ltr;
                can40returned += Ctotcan40ltr;
            }

            if (ddlSalesOffice.SelectedValue == "285")
            {
                cmd = new MySqlCommand("SELECT invtras.TransType, invtras.FromTran, invtras.ToTran, SUM(invtras.Qty) AS delivered, invtras.DOE, invmaster.sno AS invsno, invmaster.InvName FROM (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty FROM invtransactions12 WHERE (DOE BETWEEN @d1 AND @d2) OR (DOE BETWEEN @d1 AND @d2)) invtras INNER JOIN invmaster ON invtras.B_inv_sno = invmaster.sno INNER JOIN branchmappingtable ON invtras.ToTran = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @branchid) GROUP BY invsno ORDER BY invtras.DOE");
                cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
                DateTime dt1 = GetLowDate(fromdate.AddDays(-1));
                DateTime dt2 = GetLowDate(fromdate);
                cmd.Parameters.AddWithValue("@d1", dt1.AddHours(15));
                cmd.Parameters.AddWithValue("@d2", dt2.AddHours(15));
                DataTable dtinventoryD = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT invtras.TransType, invtras.FromTran, invtras.ToTran, SUM(invtras.Qty) AS Collected, invtras.DOE, invmaster.sno AS invsno, invmaster.InvName FROM (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty FROM invtransactions12 WHERE (DOE BETWEEN @d1 AND @d2) OR (DOE BETWEEN @d1 AND @d2)) invtras INNER JOIN invmaster ON invtras.B_inv_sno = invmaster.sno INNER JOIN branchmappingtable ON invtras.FromTran = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @branchid) GROUP BY invsno ORDER BY invtras.DOE");
                cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", dt1.AddHours(15));
                cmd.Parameters.AddWithValue("@d2", dt2.AddHours(15));
                DataTable dtinventoryC = vdm.SelectQuery(cmd).Tables[0];
                DataRow drinventry1 = Report.NewRow();
                drinventry1["Sno"] = i;
                drinventry1["Dispatch Name"] = "Office Stock";
                int Stockcan40ltr = 0;
                int Stockcrates = 0;
                int Stockcan20ltr = 0;
                int cratesdel = 0;
                int cratescol = 0;
                int can20del = 0;
                int can20col = 0;
                int can40del = 0;
                int can40col = 0;
                double cratesopp = 0;
                double can20opp = 0;
                double can40opp = 0;
                string crates = "1";
                string can40 = "4";
                string can20 = "3";
                foreach (DataRow dtinvd in dtinventoryD.Rows)
                {
                    int inv = 0;

                    if (dtinvd["invsno"].ToString() == "1")
                    {
                        int.TryParse(dtinvd["delivered"].ToString(), out inv);
                        cratesdel += inv;
                    }
                    if (dtinvd["invsno"].ToString() == "3")
                    {
                        int.TryParse(dtinvd["delivered"].ToString(), out inv);
                        can20del += inv;
                    } 
                    if (dtinvd["invsno"].ToString() == "4")
                    {
                        int.TryParse(dtinvd["delivered"].ToString(), out inv);
                        can40del += inv;
                    }
                   
                }
                foreach (DataRow dtinvd in dtinventoryC.Rows)
                {
                    int inv = 0;

                    if (dtinvd["invsno"].ToString() == "1")
                    {
                        int.TryParse(dtinvd["Collected"].ToString(), out inv);
                        cratescol += inv;
                    }
                    if (dtinvd["invsno"].ToString() == "3")
                    {
                        int.TryParse(dtinvd["Collected"].ToString(), out inv);
                        can20col += inv;
                    }
                    if (dtinvd["invsno"].ToString() == "4")
                    {
                        int.TryParse(dtinvd["Collected"].ToString(), out inv);
                        can40col += inv;
                    }

                }
                foreach (DataRow drdtclubtotal in dtClo.Select("sno='" + crates + "'"))
                {


                    int Ccrates = 0;
                    int.TryParse(drdtclubtotal["StockQty"].ToString(), out Ccrates);
                    Stockcrates += Ccrates;
                }
                foreach (DataRow drdtclubtotal in dtClo.Select("sno='" + can40 + "'"))
                {


                    int Ccan40ltr = 0;
                    int.TryParse(drdtclubtotal["StockQty"].ToString(), out Ccan40ltr);
                    Stockcan40ltr += Ccan40ltr;

                }
                foreach (DataRow drdtclubtotal in dtClo.Select("sno='" + can20 + "'"))
                {

                    int Ccan20ltr = 0;
                    int.TryParse(drdtclubtotal["StockQty"].ToString(), out Ccan20ltr);
                    Stockcan20ltr += Ccan20ltr;
                }
                double diff = 0;
                cratesopp = cratesreturned - cratescol;//total crates returnd to plant - totalcrates collected from agents
                diff = cratesreceived - cratesdel;//total crates received fron plant- total crates delivered to agents
                cratesopp = cratesopp - diff;//
                can20opp = can20returned - can20col;
                diff = can20received - can20del;
                can20opp = can20opp - diff;
                can40opp = can40returned - can40col;
                diff = can40received - can40del;
                can40opp = can40opp - diff;
                int num = 0;
                if (cratesopp > 0)
                {
                   drinventry1["Received Crates"] = cratesopp;

                }
                if (cratesopp <= 0)
                {
                   drinventry1["Received Crates"] = num;
                    cratesopp = num;
                }
                if (can40opp > 0)
                {
                   drinventry1["Received Can40ltr"] = can40opp;

                }
                if (can40opp <= 0)
                {
                   drinventry1["Received Can40ltr"] = num;
                    can40opp = num;

                }
                if (can20opp > 0)
                {
                   drinventry1["Received Can20ltr"] = can20opp;

                }
                if (can20opp <= 0)
                {
                   drinventry1["Received Can20ltr"] = num;
                    can20opp = num;
                }
                drinventry1["Return Crates"] = Stockcrates;
                cratesopp = 0;
                drinventry1["Difference Crates"] = cratesopp - Stockcrates;
                drinventry1["Received Can40ltr"] = can40opp;
                drinventry1["Return Can40ltr"] = Stockcan40ltr;
                drinventry1["Difference Can40ltr"] = can40opp - Stockcan40ltr;
                drinventry1["Received Can20ltr"] = can20opp;
                drinventry1["Return Can20ltr"] = Stockcan20ltr;
                drinventry1["Difference Can20ltr"] = can20opp - Stockcan20ltr;
               Report.Rows.Add(drinventry1);
                i++;
            }
                
            
            DataRow brk = Report.NewRow();
            brk["Dispatch Name"] = "";
            Report.Rows.Add(brk);

            DataRow totalinventory = Report.NewRow();
            totalinventory["Dispatch Name"] = "TOTAL";
            double val = 0.0;
            foreach (DataColumn dc in Report.Columns)
            {
                if (dc.DataType == typeof(Double))
                {
                    val = 0.0;
                    double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                    totalinventory[dc.ToString()] = val;
                }
            }
            Report.Rows.Add(totalinventory);
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