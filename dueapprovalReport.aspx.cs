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

public partial class dueapprovalReport : System.Web.UI.Page
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
                FillRouteName();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
                lblDispatchName.Text = ddlSalesOffice.SelectedItem.Text;
            }
        }
    }
    void FillRouteName()
    {
        vdm = new VehicleDBMgr();

        //cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) OR (branchdata.sno = @BranchID)");
        cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, branchdata.SalesType, branchdata.flag FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType = 21 OR branchdata.SalesType = 26) OR (branchdata.sno = @BranchID)");
        cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
        cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlSalesOffice.DataSource = dtRoutedata;
        ddlSalesOffice.DataTextField = "BranchName";
        ddlSalesOffice.DataValueField = "sno";
        ddlSalesOffice.DataBind();
        ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));

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
    DataTable dtble = new DataTable();

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            vdm = new VehicleDBMgr();
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
            //cmd = new MySqlCommand("SELECT  branchroutes.RouteName,branchsales.BranchID as AgentCode, branchdata.BranchName, branchsales.DOE, branchsales.SaleValue, branchsales.Collection, branchsales.dueamt AS DUEAMOUNT,branchsales.remarks FROM branchsales INNER JOIN branchroutes ON branchsales.Routeid = branchroutes.Sno INNER JOIN branchdata ON branchsales.BranchID = branchdata.sno WHERE (branchsales.salesofficeid = @branch) AND (branchsales.indentdate BETWEEN @d1 AND @d2)");
            cmd = new MySqlCommand("SELECT branchroutes.RouteName, branchsales.BranchID AS AgentCode, branchdata.BranchName, branchdata.CollectionType, branchsales.DOE, branchsales.SaleValue, branchsales.Collection,branchsales.dueamt AS DUEAMOUNT, branchsales.remarks, collect.AmountPaid FROM branchsales INNER JOIN branchroutes ON branchsales.Routeid = branchroutes.Sno INNER JOIN branchdata ON branchsales.BranchID = branchdata.sno LEFT OUTER JOIN (SELECT Branchid, UserData_sno, SUM(AmountPaid) AS AmountPaid, Denominations, Remarks, Sno, PaidDate, PaymentType, tripId, CheckStatus,ReturnDenomin, PayTime, VEmpID, ChequeNo, EmpID, ReceiptNo, VarifyDate, ChequeDate, BankName FROM collections WHERE (PaidDate BETWEEN @d3 AND @d4) AND (CheckStatus IS NULL) AND (tripId IS NULL) GROUP BY Branchid) collect ON branchdata.sno = collect.Branchid WHERE (branchsales.salesofficeid = @branch) AND (branchsales.indentdate BETWEEN @d1 AND @d2)"); 
            cmd.Parameters.AddWithValue("branch", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@d3", GetLowDate(fromdate.AddDays(1)));
            cmd.Parameters.AddWithValue("@d4", GetHighDate(fromdate.AddDays(1)));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            lblDate.Text = GetLowDate(fromdate).AddDays(1).ToString();
            grdReports.DataSource = dtble;
            grdReports.DataBind();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdReports.DataSource = dtble;
            grdReports.DataBind();
        }
        
    }
}