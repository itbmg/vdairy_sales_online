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


public partial class AgentWiseInventory : System.Web.UI.Page
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
                //lblDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                FillAgentName();
                lblTitle.Text = Session["TitleName"].ToString();
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType)");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                cmd.Parameters.AddWithValue("@SalesType", "21");
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
                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
                //cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
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
        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
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
    DataTable NewReport = new DataTable();
    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            pnlHide.Visible = true;
            DataTable Report = new DataTable();
            DateTime fromdate = DateTime.Now;
            NewReport = new DataTable();
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
            Session["filename"] = ddlAgentName.SelectedItem.Text + "-> INVENTORY TRANSACTION";
            lblAgent.Text = ddlAgentName.SelectedItem.Text;
            lbl_fromDate.Text = fromdate.ToString("dd/MMM/yyyy");
            lbl_selttodate.Text = todate.ToString("dd/MMM/yyyy");
            Report.Columns.Add("Sno");
            Report.Columns.Add("Date");
            Report.Columns.Add("Opp Crates");
            Report.Columns.Add("Issued Crates", typeof(Double));
            Report.Columns.Add("Received Crates", typeof(Double));
            Report.Columns.Add("CB Crates");
            Report.Columns.Add("Opp Cans");
            Report.Columns.Add("Issued Cans", typeof(Double));
            Report.Columns.Add("Received Cans", typeof(Double));
            Report.Columns.Add("CB Cans");
            cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName,due_trans_inventory.doe, due_trans_inventory.oppening, due_trans_inventory.isuued, due_trans_inventory.received, due_trans_inventory.closing, due_trans_inventory.opp10, due_trans_inventory.issue10, due_trans_inventory.rec10, due_trans_inventory.clo10, due_trans_inventory.opp20, due_trans_inventory.issu20, due_trans_inventory.rec20, due_trans_inventory.clo20, due_trans_inventory.opp40, due_trans_inventory.issu40, due_trans_inventory.rec40, due_trans_inventory.clo40 FROM due_trans_inventory INNER JOIN branchdata ON due_trans_inventory.agentid = branchdata.sno WHERE (due_trans_inventory.agentid = @BranchID) AND (due_trans_inventory.doe BETWEEN @d1 AND @D2)");
            cmd.Parameters.AddWithValue("@dTransType", "2");
            //cmd.Parameters.AddWithValue("@cTransType", "3");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            DataTable dtallInventoryDc = vdm.SelectQuery(cmd).Tables[0];
            int i = 1;
            foreach (DataRow dr in dtallInventoryDc.Rows)
            {
                DataRow drnew = Report.NewRow();
                drnew["Sno"] = i++.ToString();
                string IndentDate = dr["doe"].ToString();
                DateTime dtIndentDate = Convert.ToDateTime(IndentDate);
                string ChangedTime = dtIndentDate.ToString("dd/MMM/yyyy");
                drnew["Date"] = ChangedTime;

                drnew["Opp Crates"] = dr["oppening"].ToString();
                drnew["Issued Crates"] = dr["isuued"].ToString();
                drnew["Received Crates"] = dr["received"].ToString();
                drnew["CB Crates"] = dr["closing"].ToString(); 

                int opp10 = 0;
                int.TryParse(dr["opp10"].ToString(), out opp10);
                int opp20 = 0;
                int.TryParse(dr["opp20"].ToString(), out opp20);
                int opp40 = 0;
                int.TryParse(dr["opp40"].ToString(), out opp40);
                int canoppbal = 0;
                canoppbal = opp10 + opp20 + opp40;

                int rec10 = 0;
                int.TryParse(dr["rec10"].ToString(), out rec10);
                int rec20 = 0;
                int.TryParse(dr["rec20"].ToString(), out rec20);
                int rec40 = 0;
                int.TryParse(dr["rec40"].ToString(), out rec40);
                int canIssuedbal = 0;
                canIssuedbal = rec10 + rec20 + rec40;

                int issue10 = 0;
                int.TryParse(dr["issue10"].ToString(), out issue10);
                int issue20 = 0;
                int.TryParse(dr["issu20"].ToString(), out issue20);
                int issue40 = 0;
                int.TryParse(dr["issu40"].ToString(), out issue40);
                int canrecbal = 0;
                canrecbal = issue10 + issue20 + issue40;

                int can10 = 0;
                int.TryParse(dr["clo10"].ToString(), out can10);
                int can20 = 0;
                int.TryParse(dr["clo20"].ToString(), out can20);
                int can40 = 0;
                int.TryParse(dr["clo40"].ToString(), out can40);
                int canbal = 0;
                canbal = can10 + can20 + can40;
                drnew["Opp Cans"] = canoppbal.ToString();
                drnew["Issued Cans"] = canIssuedbal.ToString();
                drnew["Received Cans"] = canrecbal.ToString();
                drnew["CB Cans"] = canbal.ToString();
                Report.Rows.Add(drnew);
            }
            DataRow newTotal = Report.NewRow();
            newTotal["Date"] = "Total";
            double val = 0.0;
            foreach (DataColumn dc in Report.Columns)
            {
                if (dc.DataType == typeof(Double))
                {
                    val = 0.0;
                    double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                    newTotal[dc.ToString()] = val;
                }
            }
            Report.Rows.Add(newTotal);
            grdReports.DataSource = Report;
            grdReports.DataBind();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdReports.DataSource = NewReport;
            grdReports.DataBind();
        }
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
    protected void ddlDispName_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch)");
        cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutes ON dispatch_sub.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (dispatch.sno = @dispsno)");
        cmd.Parameters.AddWithValue("@dispsno", ddlDispName.SelectedValue);
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlAgentName.DataSource = dtRoutedata;
        ddlAgentName.DataTextField = "BranchName";
        ddlAgentName.DataValueField = "sno";
        ddlAgentName.DataBind();
    }
}