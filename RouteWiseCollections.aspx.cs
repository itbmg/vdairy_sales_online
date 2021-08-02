using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class RouteWiseCollections : System.Web.UI.Page
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
            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    void FillRouteName()
    {
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
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));

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
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));

            }
        }
        catch
        {
        }
    }
    protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID)  AND (dispatch.flag=@flag))  OR ((branchdata_1.SalesOfficeID = @SOID) AND (dispatch.flag=@flag))");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@flag", "1");
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlRouteName.DataSource = dtRoutedata;
        ddlRouteName.DataTextField = "DispName";
        ddlRouteName.DataValueField = "sno";
        ddlRouteName.DataBind();
        ddlRouteName.Items.Insert(0, new ListItem("Select", "0"));
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
    DateTime fromdate = DateTime.Now;

    string routeid = "";
    string routeitype = "";
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            Session["RouteName"] = ddlRouteName.SelectedItem.Text;
            lblRouteName.Text = ddlRouteName.SelectedItem.Text;
            vdm = new VehicleDBMgr();
            DataTable Report = new DataTable();

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
            lblDate.Text = fromdate.ToString("dd/MMM/yyyy");
            // cmd.Parameters.AddWithValue("SELECT branchdata.sno, branchdata.BranchName, branchaccounts.Amount FROM dispatch INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN branchaccounts ON branchdata.sno = branchaccounts.BranchId WHERE (dispatch.sno = 5)");
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, collections.AmountPaid, collections.PaidDate, collections.PaymentType, collections.CheckStatus, collections.PayTime,collections.ChequeNo,collections.tripId, collections.ReceiptNo FROM dispatch INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN collections ON branchdata.sno = collections.Branchid WHERE (dispatch.sno = @dispatchsno) AND (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.AmountPaid <> 0)");
            cmd.Parameters.AddWithValue("@dispatchsno", ddlRouteName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            DataTable dtroutecollection = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT Sno, BranchId, ReceivedFrom, AgentID, Empid, Amountpayable, AmountPaid, DOE, Create_by, Modified_by, Remarks, OppBal, dispatchid, Receipt, PaymentStatus,ChequeNo, GroupRecieptNo, GroupRef, Tripid FROM cashreceipts WHERE (BranchId = @branchid) AND (DOE BETWEEN @d1 AND @d2) AND (AmountPaid <> 0)");
            if (Session["salestype"].ToString() == "Plant")
            {
            cmd.Parameters.AddWithValue("@branchid",ddlSalesOffice.SelectedValue);
            }
            if (Session["salestype"].ToString() != "Plant")
            {
                cmd.Parameters.AddWithValue("@branchid", Session["branch"]);
            }
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            DataTable dtreciept = vdm.SelectQuery(cmd).Tables[0];
            Report = new DataTable();
            Report.Columns.Add("Reciept No");
            Report.Columns.Add("Agent Code");
            Report.Columns.Add("Agent Name");
            Report.Columns.Add("Collected Time");
            Report.Columns.Add("Collected Amount");
            Report.Columns.Add("Payment Type");
            Report.Columns.Add("Collection Type");
            double amountcollected = 0;
            foreach (DataRow branch in dtroutecollection.Rows)
            {
                string reciptno = "";
                DataRow newrow = Report.NewRow();
                newrow["Agent Code"] = branch["sno"].ToString();
                newrow["Agent Name"] = branch["BranchName"].ToString();
                newrow["Collected Amount"] = branch["AmountPaid"].ToString();
                reciptno = branch["ReceiptNo"].ToString();
                if (reciptno == "")
                {
                    foreach (DataRow drdt in dtreciept.Select("Tripid='" + branch["tripId"].ToString() + "'"))
                    {
                        if (branch["sno"].ToString() == drdt["AgentID"].ToString())
                        {
                            newrow["Reciept No"] = drdt["Receipt"].ToString();
                        }
                    }
                }
                if (reciptno != "")
                {
                    newrow["Reciept No"] = reciptno;
                }
                if (branch["PaymentType"].ToString() == "Cheque")
                {

                }
                else
                {
                    double totamt = 0;
                    double.TryParse(branch["AmountPaid"].ToString(),out totamt);
                    amountcollected += totamt;

                }
                newrow["Collected Time"] = branch["PaidDate"].ToString();
                if (branch["PaymentType"].ToString() == "Cheque")
                {
                    newrow["Payment Type"] = "Cheque No:" + branch["ChequeNo"].ToString() + "" + "Status:" + branch["CheckStatus"].ToString() + "";
                }
                else
                {
                    newrow["Payment Type"] = branch["PaymentType"].ToString();
                }
                if (branch["tripId"].ToString() == "")
                {
                    newrow["Collection Type"] = "Sales Office";
                }
                else
                {
                    newrow["Collection Type"] = "Trip";
                }
                Report.Rows.Add(newrow);
            }
            DataRow newrowtotal = Report.NewRow();
            newrowtotal["Collected Time"]="Total";
            newrowtotal["Collected Amount"] = Math.Round(amountcollected,2);
            Report.Rows.Add(newrowtotal);
            grdReports.DataSource = Report;
            grdReports.DataBind();

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}