using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class agentinformationsalesofficewise : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["salestype"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                lblTitle.Text = Session["TitleName"].ToString();
                FillSalesOffice();
            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    void FillSalesOffice()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Plant")
            {
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
            PanelHide.Visible = true;
            Report = new DataTable();
            Session["RouteName"] = ddlSalesOffice.SelectedItem.Text;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
           
            lblRoutName.Text = ddlSalesOffice.SelectedItem.Text;
            Session["filename"] = "AGENT Details REPORT";

            cmd = new MySqlCommand("SELECT branchdata.sno AS Agentid, branchdata.BranchName AS AgentName, branchroutes.RouteName, branchroutes.BranchID AS SalesOfficeID, salestypemanagement.salestype, branchdata.SalesRepresentative FROM            branchroutes INNER JOIN  branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN  branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN  salestypemanagement ON salestypemanagement.sno = branchdata.SalesType WHERE (branchroutes.BranchID = @soid) AND (branchroutes.flag = '1') ORDER BY branchroutes.Sno");
            cmd.Parameters.AddWithValue("@soid", ddlSalesOffice.SelectedValue);
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            if (dtble.Rows.Count > 0)
            {
                grdReports.DataSource = dtble;
                grdReports.DataBind();
                Session["xportdata"] = dtble;
            }
            else
            {
                grdReports.DataSource = dtble;
                grdReports.DataBind();
                Session["xportdata"] = dtble;
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
}