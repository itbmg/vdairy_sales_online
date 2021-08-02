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

public partial class NewAgentReport : System.Web.UI.Page
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
                //PBranch.Visible = true;
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
                //PBranch.Visible = true;
                //cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) OR (branchdata.sno = @BranchID)");
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, branchdata_1.SalesType FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata_1.SalesType IS NOT NULL) OR (branchdata.sno = @BranchID) AND (branchdata_1.SalesType IS NOT NULL)");
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
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable("GridView_Data");
            int count = 0;
            foreach (TableCell cell in grdReports.HeaderRow.Cells)
            {
                //if (count == 2)
                //{
                    dt.Columns.Add(cell.Text);
                //}
                //else
                //{
                //    dt.Columns.Add(cell.Text).DataType = typeof(double);
                //}
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
    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            lblmsg.Text = "";
            PanelHide.Visible = true;
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
            lblFromdate.Text = fromdate.ToString("dd/MMM/yyyy");
            lblToDate.Text = todate.ToString("dd/MMM/yyyy");
            if (ddlreporttype.SelectedValue == "New Agents")
            {
                Session["filename"] = "New Agent Report";
                //cmd = new MySqlCommand("SELECT branchdata.sno AS AgentCode, branchdata.BranchName AS AgentName,branchdata.DateOfEntry AS CreatedDate, branchdata.Address, branchdata.phonenumber FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @BranchID) AND (branchdata.DateOfEntry BETWEEN @d1 AND @d2) and ( branchdata.flag=@flag)");
                cmd = new MySqlCommand("SELECT branchroutes.RouteName, branchdata.sno AS AgentCode, branchdata.BranchName AS AgentName, branchdata.DateOfEntry AS CreatedDate, branchdata.Address,branchdata.phonenumber FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchroutesubtable ON branchdata.sno = branchroutesubtable.BranchID INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (branchdata.DateOfEntry BETWEEN @d1 AND @d2) ORDER BY branchroutes.Sno");
                cmd.Parameters.AddWithValue("@flag", 1);
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                DataTable dtNewAgent = vdm.SelectQuery(cmd).Tables[0];
                if (dtNewAgent.Rows.Count > 0)
                {
                    grdReports.DataSource = dtNewAgent;
                    grdReports.DataBind();
                }
                else
                {
                    lblmsg.Text = "No Agent Added between  Dates";
                }
            }
            if (ddlreporttype.SelectedValue == "Stoped Agents")
            {
                Session["filename"] = "Stopped Agent Report";
                cmd = new MySqlCommand("SELECT totaldata.sno, totaldata.BranchName, totaldata.flag, totaldata.phonenumber, totaldata.DateOfEntry, totaldata.Address, totaldata.CollectionType, totaldata.Due_Limit_Days, totaldata.Due_Limit_Type, totaldata.SalesRepresentative, totaldata.delqty, branchroutes.RouteName, branchaccounts.Amount FROM branchaccounts RIGHT OUTER JOIN (SELECT branchdata.sno, branchdata.BranchName, branchdata.flag, branchdata.phonenumber, branchdata.DateOfEntry, branchdata.Address,branchdata.CollectionType, branchdata.Due_Limit_Days, branchdata.Due_Limit_Type, branchdata.SalesRepresentative,SUM(indents_subtable.DeliveryQty) AS delqty FROM indents_subtable INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM indents WHERE (I_date BETWEEN @d1 AND @d2)) indnts ON indents_subtable.IndentNo = indnts.IndentNo RIGHT OUTER JOIN branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno ON indnts.Branch_id = branchdata.sno WHERE (branchmappingtable.SuperBranch = @BranchID) GROUP BY branchdata.sno) totaldata ON branchaccounts.BranchId = totaldata.sno LEFT OUTER JOIN branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo ON totaldata.sno = branchroutesubtable.BranchID WHERE (totaldata.delqty IS NULL) OR (totaldata.delqty < 0)");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-30)));
                DataTable dtAgent = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName, indnts.maxdate FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN (SELECT IndentNo, Branch_id, MAX(I_date) AS maxdate FROM indents GROUP BY Branch_id) indnts ON branchdata.sno = indnts.Branch_id WHERE (branchmappingtable.SuperBranch = @branchid) GROUP BY branchdata.sno");
                cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
                DataTable dtenddate = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName, inventory_monitor.Inv_Sno, inventory_monitor.Qty FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN inventory_monitor ON branchdata.sno = inventory_monitor.BranchId WHERE (branchmappingtable.SuperBranch = @branchid) GROUP BY branchdata.sno, inventory_monitor.Inv_Sno");
                cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
                DataTable dtinventory = vdm.SelectQuery(cmd).Tables[0];
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("Agent Code");
                Report.Columns.Add("Agent Name");
                Report.Columns.Add("Address");
                Report.Columns.Add("Mobile");
               // Report.Columns.Add("Status");
                //Report.Columns.Add("Route_Name");
                //Report.Columns.Add("Sales_Rep");
                Report.Columns.Add("Created_On");
                Report.Columns.Add("Stoped_From");
                Report.Columns.Add("Amount_Balance");
                Report.Columns.Add("Crates_Balance");
                Report.Columns.Add("Cans_Balance");
                int i = 1;
                foreach (DataRow dr in dtAgent.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    newrow["Agent Code"] = dr["sno"].ToString();
                    newrow["Agent Name"] = dr["BranchName"].ToString();
                    
                    newrow["Address"] = dr["Address"].ToString();
                    newrow["Mobile"] = dr["phonenumber"].ToString();
                    //if (dr["flag"].ToString() == "1")
                    //{
                    //    newrow["Status"] = "Active";

                    //}
                    //if (dr["flag"].ToString() == "0")
                    //{
                    //    newrow["Status"] = "Inactive";

                    //}
                    //newrow["Route_Name"] = dr["RouteName"].ToString();
                    newrow["Created_On"] = dr["DateOfEntry"].ToString();
                    foreach (DataRow drdt in dtenddate.Select("sno='" + dr["sno"].ToString() + "'"))
                    {
                        newrow["Stoped_From"] = drdt["maxdate"].ToString();
                    }
                    newrow["Amount_Balance"] = dr["Amount"].ToString();
                    int crates = 0;
                    int cans = 0;
                    foreach (DataRow drdt in dtinventory.Select("sno='" + dr["sno"].ToString() + "'"))
                    {
                        int crts = 0;
                        int cns = 0;
                        if (drdt["Inv_Sno"].ToString() == "1")
                        {

                            int.TryParse(drdt["Qty"].ToString(), out crts);
                            crates += crts;

                        }
                        if (drdt["Inv_Sno"].ToString() == "2")
                        {

                            int.TryParse(drdt["Qty"].ToString(), out cns);
                            cans += cns;

                        }
                        if (drdt["Inv_Sno"].ToString() == "3")
                        {

                            int.TryParse(drdt["Qty"].ToString(), out cns);
                            cans += cns;

                        }
                        if (drdt["Inv_Sno"].ToString() == "4")
                        {

                            int.TryParse(drdt["Qty"].ToString(), out cns);
                            cans += cns;

                        }
                        if (drdt["Inv_Sno"].ToString() == "5")
                        {

                            int.TryParse(drdt["Qty"].ToString(), out cns);
                            cans += cns;

                        }
                    }
                    newrow["Crates_Balance"] = crates;
                    newrow["Cans_Balance"] = cans;
                    Report.Rows.Add(newrow);
                    i++;
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
                
            }
        }
        catch(Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}