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

public partial class DayWisePuffInventory : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!Page.IsCallback)
        //{
        //    FillSalesOffice();
        //    //txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
        //}
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                FillSalesOffice();
                //txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                //lblTitle.Text = Session["TitleName"].ToString();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txttodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
            Session["filename"] = ddlSalesOffice.SelectedItem.Text + " REPORT " + fromdate.AddDays(1).ToString("dd/MM/yyyy");
            pnlHide.Visible = true;
            cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno, dispatch.BranchID,tripdata.sno as tripid,DATE_FORMAT(tripdata.AssignDate, '%d %b %y') AS AssignDate FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
            DataTable dtDispnames = vdm.SelectQuery(cmd).Tables[0];

            Report = new DataTable();
            Report.Columns.Add("Sno");
            Report.Columns.Add("DC Date");
            Report.Columns.Add("Dispatch Name");
            Report.Columns.Add("Received Crates", typeof(Double));
            Report.Columns.Add("Return Crates", typeof(Double));
            Report.Columns.Add("Difference Crates", typeof(Double));
            Report.Columns.Add("Verified Crates", typeof(Double));

            Report.Columns.Add("Received Other Crates", typeof(Double));
            Report.Columns.Add("Return Other Crates", typeof(Double));
            Report.Columns.Add("Difference Other Crates", typeof(Double));
            Report.Columns.Add("Verified Other Crates", typeof(Double));


            Report.Columns.Add("Received Can40ltr", typeof(Double));
            Report.Columns.Add("Return Can40ltr", typeof(Double));
            Report.Columns.Add("Difference Can40ltr", typeof(Double));
            Report.Columns.Add("Verified Can40ltr", typeof(Double));
            Report.Columns.Add("Received Can20ltr", typeof(Double));
            Report.Columns.Add("Return Can20ltr", typeof(Double));
            Report.Columns.Add("Difference Can20ltr", typeof(Double));
            Report.Columns.Add("Verified Can20ltr", typeof(Double));


            int i = 1;
            foreach (DataRow drSub in dtDispnames.Rows)
            {
                cmd = new MySqlCommand("SELECT triproutes.Tripdata_sno, tripinvdata.Qty, tripinvdata.Remaining, invmaster.InvName, invmaster.sno FROM tripdata INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripinvdata ON tripdata.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (tripdata.sno = @tripsno)");
                cmd.Parameters.AddWithValue("@tripsno", drSub["tripid"].ToString());
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
                DataTable DtTripSubData = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT TransType, FromTran, ToTran, Qty, VQty,B_inv_sno FROM invtransactions12 WHERE (ToTran = @tripsno) AND (FromTran = @soid) AND (TransType= 3)");
                cmd.Parameters.AddWithValue("@tripsno", drSub["tripid"].ToString());
                cmd.Parameters.AddWithValue("@soid", ddlSalesOffice.SelectedValue);
                DataTable DtverifiedData = vdm.SelectQuery(cmd).Tables[0];

                string Disp = drSub["DispName"].ToString();
                string[] strName = Disp.Split('_');
                DataRow newSo = Report.NewRow();
                string AssignDate = drSub["AssignDate"].ToString();
                DateTime dtAssignDate = Convert.ToDateTime(AssignDate);
                string ChangedTime = dtAssignDate.ToString("dd/MMM/yyyy");

                DataRow drinventry = Report.NewRow();
                drinventry["Sno"] = i;
                drinventry["DC Date"] = ChangedTime;
                drinventry["Dispatch Name"] = Disp;
                int Ctotcan40ltr = 0;
                int Vtotcan40ltr = 0;
                int Ctotcrates = 0;
                int Vtotcrates = 0;
                int Ctotcan20ltr = 0;
                int Dtotcrates = 0;
                int Dtotcan20ltr = 0;
                int Vtotcan20ltr = 0;
                int Dtotcan40ltr = 0;

                int Dtotothcrts = 0;
                int Vtotothcrts = 0;
                int Ctotothcrts = 0;
               

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

                        foreach (DataRow drdtverifiedtotal in DtverifiedData.Select("B_inv_sno='" + drtripinv["sno"].ToString() + "'"))
                        {
                            int Vcrates = 0;
                            int.TryParse(drdtverifiedtotal["VQty"].ToString(), out Vcrates);
                            Vtotcrates += Vcrates;

                        }
                    }
                    if (drtripinv["sno"].ToString() == "3")
                    {
                        int Dcan20ltr = 0;
                        int.TryParse(drtripinv["Qty"].ToString(), out Dcan20ltr);
                        Dtotcan20ltr += Dcan20ltr;

                        int Ccan20ltr = 0;
                        int.TryParse(drtripinv["Remaining"].ToString(), out Ccan20ltr);
                        Ctotcan20ltr += Ccan20ltr;
                        foreach (DataRow drdtverifiedtotal in DtverifiedData.Select("B_inv_sno='" + drtripinv["sno"].ToString() + "'"))
                        {
                            int Vcan20ltr = 0;
                            int.TryParse(drdtverifiedtotal["VQty"].ToString(), out Vcan20ltr);
                            Vtotcan20ltr += Vcan20ltr;

                        }

                    }

                    if (drtripinv["sno"].ToString() == "19")
                    {
                        int dothcreats = 0;
                        int.TryParse(drtripinv["Qty"].ToString(), out dothcreats);
                        Dtotothcrts += dothcreats;

                        int cothcrts = 0;
                        int.TryParse(drtripinv["Remaining"].ToString(), out cothcrts);
                        Ctotothcrts += cothcrts;
                        foreach (DataRow drdtverifiedtotal in DtverifiedData.Select("B_inv_sno='" + drtripinv["sno"].ToString() + "'"))
                        {
                            int Vothcrts = 0;
                            int.TryParse(drdtverifiedtotal["VQty"].ToString(), out Vothcrts);
                            Vtotothcrts += Vothcrts;
                        }

                    }

                    if (drtripinv["sno"].ToString() == "4")
                    {
                        int Dcan40ltr = 0;
                        int.TryParse(drtripinv["Qty"].ToString(), out Dcan40ltr);
                        Dtotcan40ltr += Dcan40ltr;

                        int Ccan40ltr = 0;
                        int.TryParse(drtripinv["Remaining"].ToString(), out Ccan40ltr);
                        Ctotcan40ltr += Ccan40ltr;
                        foreach (DataRow drdtverifiedtotal in DtverifiedData.Select("B_inv_sno='" + drtripinv["sno"].ToString() + "'"))
                        {
                            int Vcan40ltr = 0;
                            int.TryParse(drdtverifiedtotal["VQty"].ToString(), out Vcan40ltr);
                            Vtotcan40ltr += Vcan40ltr;
                        }

                    }
                }


                drinventry["Received Crates"] = Dtotcrates;
                drinventry["Return Crates"] = Ctotcrates;
                drinventry["Difference Crates"] = Dtotcrates - Ctotcrates;
                drinventry["Verified Crates"] = Vtotcrates;

                drinventry["Received Other Crates"] = Dtotothcrts;
                drinventry["Return Other Crates"] = Ctotothcrts;
                drinventry["Difference Other Crates"] = Dtotothcrts - Ctotothcrts;
                drinventry["Verified Other Crates"] = Vtotothcrts;


                drinventry["Received Can40ltr"] = Dtotcan40ltr;
                drinventry["Return Can40ltr"] = Ctotcan40ltr;
                drinventry["Difference Can40ltr"] = Dtotcan40ltr - Ctotcan40ltr;
                drinventry["Verified Can40ltr"] = Vtotcan40ltr;
                drinventry["Received Can20ltr"] = Dtotcan20ltr; 
                drinventry["Return Can20ltr"] = Ctotcan20ltr;
                drinventry["Difference Can20ltr"] = Dtotcan20ltr - Ctotcan20ltr;
                drinventry["Verified Can20ltr"] = Vtotcan20ltr;
                Report.Rows.Add(drinventry);
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
                if (count == 2 || count == 3)
                {
                    dt.Columns.Add(cell.Text);
                }
                //if (count == 3)
                //{
                //    dt.Columns.Add(cell.Text);
                //}
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