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

public partial class AgentLedger : System.Web.UI.Page
{
    MySqlCommand cmd;
    string BranchID = "";
    string Branchname = "";
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
                FillAgentName();
                lblTitle.Text = Session["TitleName"].ToString();
            }
        }
    }
    void FillAgentName()
    {
        try
        {
            vdm = new VehicleDBMgr();
            cmd = new MySqlCommand(" SELECT branchroutes.RouteName, branchroutes.Sno, branchroutes.BranchID FROM branchroutes INNER JOIN branchdata ON branchroutes.BranchID = branchdata.sno WHERE (branchroutes.BranchID = @brnchid) OR (branchdata.SalesOfficeID = @SOID)");
            //cmd = new MySqlCommand("SELECT RouteName, Sno, BranchID FROM branchroutes WHERE (BranchID = @brnchid)");
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            cmd.Parameters.AddWithValue("@brnchid", BranchID);
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            ddlRouteName.DataSource = dtRoutedata;
            ddlRouteName.DataTextField = "RouteName";
            ddlRouteName.DataValueField = "Sno";
            ddlRouteName.DataBind();
            ddlRouteName.Items.Insert(0, new ListItem("Select Route", "0"));

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
    protected void ddlRouteName_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand(" SELECT branchdata.sno, branchdata.BranchName, branchroutes.RouteName FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (branchroutes.Sno = @routesno) ORDER BY branchdata.BranchName");
        //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName, branchroutes.RouteName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchroutesubtable ON branchmappingtable.SubBranch = branchroutesubtable.BranchID INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno WHERE (branchmappingtable.SuperBranch = @SuperBranch) AND (branchroutes.Sno = @routesno) ORDER BY branchdata.BranchName");
        cmd.Parameters.AddWithValue("@SuperBranch", BranchID);
        cmd.Parameters.AddWithValue("@routesno", ddlRouteName.SelectedValue);
        DataTable dtbranchdata = vdm.SelectQuery(cmd).Tables[0];
        ddlAgentName.DataSource = dtbranchdata;
        ddlAgentName.DataTextField = "BranchName";
        ddlAgentName.DataValueField = "sno";
        ddlAgentName.DataBind();
        ddlAgentName.Items.Insert(0, new ListItem("Select Agent", "0"));

    }
    DataTable NewReport = new DataTable();
    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            pnlhide.Visible = true;
            DataTable Report = new DataTable();
             NewReport = new DataTable();
            DateTime fromdate = DateTime.Now;
            lblmsg.Text = "";
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
            lblAgent.Text = ddlAgentName.SelectedItem.Text;
            lbl_fromDate.Text = fromdate.ToString();
            lbl_selttodate.Text = todate.ToString();
            fromdate = fromdate.AddDays(-2);
            cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost), 2) AS Amount, indents.IndentNo, indents.I_date, indents_subtable.D_date FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (indents.Branch_id = @BranchID) GROUP BY indents.IndentNo");
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtIndent = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT Branchid, UserData_sno, AmountPaid, Denominations, Remarks, Sno, PaidDate, PaymentType, tripId, CheckStatus, ReturnDenomin, PayTime FROM collections WHERE (Branchid = @BranchID) AND (PaidDate BETWEEN @d1 AND @d2) and (AmountPaid<>'0')");
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable dtCollections = vdm.SelectQuery(cmd).Tables[0];
            Report.Columns.Add("Indent");
            Report.Columns.Add("IndentDate");
            Report.Columns.Add("Debit");
            Report.Columns.Add("Credit");
            Report.Columns.Add("Closing Balance");
            Report.Columns.Add("ReceiptNo");
            Report.Columns.Add("CDate");
            double CreditAmount = 0;
            double DebitAmount = 0;
            foreach (DataRow drI in dtIndent.Rows)
            {
                string Status = "true";
                foreach (DataRow drC in dtCollections.Rows)
                {
                    string IndentDate = drI["D_date"].ToString();
                    string ChangedTime = "";
                    if (IndentDate == "")
                    {
                    }
                    else
                    {
                        DateTime dtIndentDate = Convert.ToDateTime(IndentDate);
                        ChangedTime = dtIndentDate.ToString("dd/MMM/yyyy");
                    }
                    string ColDate = drC["PaidDate"].ToString();
                    DateTime dtColDate = Convert.ToDateTime(ColDate);
                    string ColTime = dtColDate.ToString("dd/MMM/yyyy");
                    if (Status == "true")
                    {
                        //if (ChangedTime == ColTime)
                        //{
                        DataRow drnew = Report.NewRow();
                        drnew["Debit"] = drI["Amount"].ToString();
                        drnew["Indent"] = drI["IndentNo"].ToString();
                        drnew["IndentDate"] = drI["D_date"].ToString();
                        double Camount = 0;
                        double.TryParse(drI["Amount"].ToString(), out Camount);
                        CreditAmount += Camount;
                        Report.Rows.Add(drnew);
                        Status = "false";
                        drnew["Closing Balance"] = drI["Amount"].ToString();
                        //}
                    }
                    else if (ChangedTime == ColTime)
                    {
                        DataRow drnew1 = Report.NewRow();
                        drnew1["Credit"] = drC["AmountPaid"].ToString();
                        drnew1["ReceiptNo"] = drC["Sno"].ToString();
                        drnew1["CDate"] = drC["PaidDate"].ToString();
                        //drnew1["Closing Balance"] = drC["AmountPaid"].ToString();
                        double Damount = 0;
                        double.TryParse(drC["AmountPaid"].ToString(), out Damount);
                        DebitAmount += Damount;
                        Report.Rows.Add(drnew1);
                    }
                }
            }
            cmd = new MySqlCommand("SELECT Amount, BranchId FROM  branchaccounts WHERE (BranchId = @BranchID)");
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            DataTable dtAmount = vdm.SelectQuery(cmd).Tables[0];
            string BrAmount = dtAmount.Rows[0]["Amount"].ToString();
            double BranchAmount = 0;
            double.TryParse(BrAmount, out BranchAmount);
            double oppAmount = BranchAmount - CreditAmount + DebitAmount;
            oppAmount = Math.Round(oppAmount, 2);
            lblOppbal.Text = oppAmount.ToString();
            NewReport.Columns.Add("Sno");
            NewReport.Columns.Add("IndentNo");
            NewReport.Columns.Add("Type");
            NewReport.Columns.Add("Description");
            NewReport.Columns.Add("Debit");
            NewReport.Columns.Add("Credit");
            NewReport.Columns.Add("Closing Balance");
            int i = 1;
            double CreditColAmount = 0;
            foreach (DataRow drI in Report.Rows)
            {
                DataRow drnew = NewReport.NewRow();
                drnew["Debit"] = drI["Debit"].ToString();
                string Debit = drI["Debit"].ToString();
                if (i == 1)
                {
                    double Cb = 0;
                    double.TryParse(drI["Closing Balance"].ToString(), out Cb);
                    double CloBal = oppAmount + Cb;
                    CloBal = Math.Round(CloBal, 2);
                    drnew["Closing Balance"] = CloBal;
                    drnew["IndentNo"] = drI["Indent"].ToString();
                    drnew["Type"] = "Indent";
                    CreditColAmount = CloBal;
                    drnew["Description"] = "Delivery For Indent No " + drI["Indent"].ToString() + " Date " + drI["IndentDate"].ToString() + "";
                }
                else
                {
                    if (Debit == "")
                    {
                        double Cb = 0;
                        double.TryParse(drI["Credit"].ToString(), out Cb);
                        drnew["Credit"] = drI["Credit"].ToString();
                        double CloBal = CreditColAmount - Cb;
                        CloBal = Math.Round(CloBal, 2);
                        drnew["Closing Balance"] = CloBal;
                        drnew["IndentNo"] = drI["ReceiptNo"].ToString();
                        drnew["Type"] = "Collection";
                        CreditColAmount = CloBal;
                        drnew["Description"] = "Receipt No For Payment " + drI["ReceiptNo"].ToString() + " Date " + drI["CDate"].ToString() + "";
                    }
                    else
                    {
                        double Cb = 0;
                        double.TryParse(drI["Debit"].ToString(), out Cb);
                        double CloBal = CreditColAmount + Cb;
                        CloBal = Math.Round(CloBal, 2);
                        drnew["Closing Balance"] = CloBal;
                        drnew["IndentNo"] = drI["Indent"].ToString();
                        drnew["Type"] = "Indent";
                        CreditColAmount = CloBal;
                        drnew["Description"] = "Delivery For Indent No " + drI["Indent"].ToString() + " Date " + drI["IndentDate"].ToString() + "";
                    }
                }
                drnew["Sno"] = i++.ToString();
                NewReport.Rows.Add(drnew);
            }
            lblCloBal.Text = CreditColAmount.ToString();
            grdReports.DataSource = NewReport;
            grdReports.DataBind();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdReports.DataSource = NewReport;
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
    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Button btn = (Button)e.Row.FindControl("btnIndent");
            btn.Attributes["onclick"] = "javascript:return rowno('" + e.Row.RowIndex + "')";
        }
        //e.Row.Cells[2].Visible = false;
        //e.Row.Cells[3].Visible = false;
    }
}