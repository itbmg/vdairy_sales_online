using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class SalesOfficeCollection : System.Web.UI.Page
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
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
            Session["RouteName"] = ddlSalesOffice.SelectedItem.Text;
            lblRouteName.Text = ddlSalesOffice.SelectedItem.Text;
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
            DateTime Todate = DateTime.Now;
            string[] Todatestrig = txtTodate.Text.Split(' ');
            if (Todatestrig.Length > 1)
            {
                if (Todatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = Todatestrig[0].Split('-');
                    string[] times = Todatestrig[1].Split(':');
                    Todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            lblDate.Text = fromdate.ToString("dd/MMM/yyyy");
            string BranchID = ddlSalesOffice.SelectedValue;
            if (BranchID == "572")
            {
                BranchID = "158";
            }
            string Status = ddlStatus.SelectedValue;
            if (Status == "ALL")
            {
                // cmd.Parameters.AddWithValue("SELECT branchdata.sno, branchdata.BranchName, branchaccounts.Amount FROM dispatch INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN branchaccounts ON branchdata.sno = branchaccounts.BranchId WHERE (dispatch.sno = 5)");
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, collections.AmountPaid, collections.PaidDate, collections.PaymentType, collections.CheckStatus, collections.PayTime,collections.ChequeNo,collections.Remarks, collections.tripId, collections.ReceiptNo, dispatch.DispName FROM dispatch INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN collections ON branchdata.sno = collections.Branchid WHERE (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) AND (dispatch.Branch_Id = @brnchid) AND (collections.TransactionType IS NULL) Group by collections.PaidDate");
                cmd.Parameters.AddWithValue("@brnchid", BranchID);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            }
            if (Status == "Cash")
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, collections.AmountPaid, collections.PaidDate, collections.PaymentType, collections.CheckStatus, collections.PayTime,collections.ChequeNo,collections.Remarks, collections.tripId, collections.ReceiptNo, dispatch.DispName FROM dispatch INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN collections ON branchdata.sno = collections.Branchid WHERE (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) AND (dispatch.Branch_Id = @brnchid) AND (collections.TransactionType IS NULL) AND (collections.PaymentType = @pt) GROUP BY collections.PaidDate");
                cmd.Parameters.AddWithValue("@brnchid", BranchID);
                cmd.Parameters.AddWithValue("@pt", "Cash");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));

            }
            if (Status == "Cheque")
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, collections.AmountPaid, collections.PaidDate, collections.PaymentType, collections.CheckStatus, collections.PayTime,collections.ChequeNo,collections.Remarks, collections.tripId, collections.ReceiptNo, dispatch.DispName FROM dispatch INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN collections ON branchdata.sno = collections.Branchid WHERE (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) AND (dispatch.Branch_Id = @brnchid) AND (collections.TransactionType IS NULL) AND (collections.PaymentType = @pt) GROUP BY collections.PaidDate");
                cmd.Parameters.AddWithValue("@brnchid", BranchID);
                cmd.Parameters.AddWithValue("@pt", "Cheque");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            }
            if (Status == "DD")
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, collections.AmountPaid, collections.PaidDate, collections.PaymentType, collections.CheckStatus, collections.PayTime,collections.ChequeNo,collections.Remarks, collections.tripId, collections.ReceiptNo, dispatch.DispName FROM dispatch INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN collections ON branchdata.sno = collections.Branchid WHERE (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) AND (dispatch.Branch_Id = @brnchid) AND (collections.TransactionType IS NULL) AND (collections.PaymentType = @pt) GROUP BY collections.PaidDate");
                cmd.Parameters.AddWithValue("@brnchid", BranchID);
                cmd.Parameters.AddWithValue("@pt", "DD");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            }
            if (Status == "Bank Transfer")
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, collections.AmountPaid, collections.PaidDate, collections.PaymentType, collections.CheckStatus, collections.PayTime,collections.ChequeNo,collections.Remarks, collections.tripId, collections.ReceiptNo, dispatch.DispName FROM dispatch INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN collections ON branchdata.sno = collections.Branchid WHERE (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) AND (dispatch.Branch_Id = @brnchid) AND (collections.TransactionType IS NULL) AND (collections.PaymentType = @pt) GROUP BY collections.PaidDate");
                cmd.Parameters.AddWithValue("@brnchid", BranchID);
                cmd.Parameters.AddWithValue("@pt", "Bank Transfer");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            }
            if (Status == "Incentive")
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, collections.AmountPaid, collections.PaidDate, collections.PaymentType, collections.CheckStatus, collections.PayTime,collections.ChequeNo,collections.Remarks, collections.tripId, collections.ReceiptNo, dispatch.DispName FROM dispatch INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN collections ON branchdata.sno = collections.Branchid WHERE (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) AND (dispatch.Branch_Id = @brnchid) AND (collections.TransactionType IS NULL) AND (collections.PaymentType = @pt) GROUP BY collections.PaidDate, branchdata.sno");
                cmd.Parameters.AddWithValue("@brnchid", BranchID);
                cmd.Parameters.AddWithValue("@pt", "Incentive");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            }
            if (Status == "Journal Voucher")
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, collections.AmountPaid, collections.PaidDate, collections.PaymentType, collections.CheckStatus, collections.PayTime,collections.ChequeNo,collections.Remarks, collections.tripId, collections.ReceiptNo, dispatch.DispName FROM dispatch INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN collections ON branchdata.sno = collections.Branchid WHERE (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) AND (dispatch.Branch_Id = @brnchid) AND (collections.TransactionType IS NULL) AND (collections.PaymentType = @pt) GROUP BY collections.PaidDate");
                cmd.Parameters.AddWithValue("@brnchid", BranchID);
                cmd.Parameters.AddWithValue("@pt", "Journal Voucher");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            }
            if (Status == "Route Collections")
            {
                cmd = new MySqlCommand("SELECT  collections.PaymentType,collections.Remarks,collections.PaidDate,collections.AmountPaid, branchroutes.RouteName as DispName, branchroutes.sno as routeid,  branchroutesubtable.BranchID, branchdata.BranchName,branchdata.sno  FROM    branchroutes   INNER JOIN    branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo   INNER JOIN    branchdata ON branchroutesubtable.BranchID = branchdata.sno  INNER JOIN collections ON branchdata.sno=collections.Branchid    WHERE (branchroutes.BranchID = @BranchID) and (branchdata.flag=@flag) and (collections.PaidDate between @d1 and @d2) and (collections.tripId <> 'NULL') GROUP BY branchdata.BranchName  ORDER BY DispName ");
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@flag", "1");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            }
            DataTable dtroutecollection = vdm.SelectQuery(cmd).Tables[0];

            Report = new DataTable();
            Report.Columns.Add("Route Name");
            Report.Columns.Add("Agent Code");
            Report.Columns.Add("Agent Name");
            //Report.Columns.Add("Refno");
            Report.Columns.Add("ReceiptNo");
            Report.Columns.Add("Collected Time");
            Report.Columns.Add("Collected Amount").DataType = typeof(Double);
            Report.Columns.Add("Payment Type");
            Report.Columns.Add("Collection Type");
            Report.Columns.Add("Narration");

            foreach (DataRow branch in dtroutecollection.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["Route Name"] = branch["DispName"].ToString();
                newrow["Agent Code"] = branch["sno"].ToString();
                newrow["Agent Name"] = branch["BranchName"].ToString();
                //newrow["Refno"] = branch["ReceiptNo"].ToString();
                newrow["Collected Amount"] = branch["AmountPaid"].ToString();
               
                if (Status == "Route Collections")
                {
                    newrow["Payment Type"] = branch["PaymentType"].ToString();
                    newrow["Collection Type"] = "Trip";
                    newrow["Narration"] = branch["Remarks"].ToString();
                    newrow["Collected Time"] = branch["PaidDate"].ToString();
                }
                else
                {
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
                    newrow["Narration"] = branch["Remarks"].ToString();
                    newrow["ReceiptNo"] = branch["ReceiptNo"].ToString();
                    newrow["Collected Time"] = branch["PaidDate"].ToString();
                }
                Report.Rows.Add(newrow);
            }
            DataRow TotRow = Report.NewRow();
            TotRow["Agent Name"] = "Total";
            double val = 0.0;
            foreach (DataColumn dc in Report.Columns)
            {
                if (dc.DataType == typeof(Double))
                {
                    val = 0.0;
                    double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                    TotRow[dc.ToString()] = val;
                }
            }
            Report.Rows.Add(TotRow);





            grdReports.DataSource = Report;
            grdReports.DataBind();

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}