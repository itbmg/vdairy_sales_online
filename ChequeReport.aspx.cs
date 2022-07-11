using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using ClosedXML.Excel;

public partial class ChequeReport : System.Web.UI.Page
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
                lblTitle.Text = Session["TitleName"].ToString();
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                FillSalesOffice();
                FillAgentName();

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
    void FillSalesOffice()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Plant")
            {
                
                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("BranchName");
                dtBranch.Columns.Add("sno");
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) and (branchdata.flag<>0) ");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                cmd.Parameters.AddWithValue("@SalesType", "21");
                cmd.Parameters.AddWithValue("@SalesType1", "26");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtRoutedata.Rows)
                {
                    DataRow newrow = dtBranch.NewRow();
                    newrow["BranchName"] = dr["BranchName"].ToString();
                    newrow["sno"] = dr["sno"].ToString();
                    dtBranch.Rows.Add(newrow);
                }
                cmd = new MySqlCommand("SELECT BranchName, sno FROM  branchdata WHERE (sno = @BranchID) and (flag<>0)");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtPlant.Rows)
                {
                    DataRow newrow = dtBranch.NewRow();
                    newrow["BranchName"] = dr["BranchName"].ToString();
                    newrow["sno"] = dr["sno"].ToString();
                    dtBranch.Rows.Add(newrow);
                }
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) and (branchdata.flag<>0)");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                cmd.Parameters.AddWithValue("@SalesType", "23");
                DataTable dtNewPlant = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtNewPlant.Rows)
                {
                    DataRow newrow = dtBranch.NewRow();
                    newrow["BranchName"] = dr["BranchName"].ToString();
                    newrow["sno"] = dr["sno"].ToString();
                    dtBranch.Rows.Add(newrow);
                }
                ddlSalesOffice.DataSource = dtBranch;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
            }
            else
            {
                cmd = new MySqlCommand("SELECT BranchName, sno FROM branchdata WHERE (sno = @BranchID) and (flag<>0)");
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
    DataTable dtCheque = new DataTable();
    void GetReport()
    {
        try
        {
            pnlHide.Visible = true;
            vdm = new VehicleDBMgr();
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
            DataTable dtothercheques = new DataTable();

            Session["filename"] = "CHEQUE REPORT";
            string Status = ddlStatus.SelectedValue;
            if (Status == "ALL")
            {
               // cmd = new MySqlCommand("SELECT DATE_FORMAT(collections.PaidDate, '%d %b %y') AS PaidDate,DATE_FORMAT(collections.VarifyDate, '%d %b %y') AS VarifyDate,branchdata.BranchName,collections.CheckStatus,collections.BankName,collections.Remarks,collections.ChequeNo,collections.AmountPaid,DATE_FORMAT(collections.ChequeDate, '%d %b %y') AS chequeDate FROM branchdata INNER JOIN collections ON branchdata.sno = collections.Branchid INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (collections.PaymentType = 'Cheque') AND (branchmappingtable.SuperBranch = @SuperBranch)  and (collections.PaidDate between @d1 and @d2) and (collections.tripid is null) GROUP BY branchdata.BranchName, collections.Denominations, collections.PaidDate order by collections.PaidDate");
                cmd = new MySqlCommand("SELECT DATE_FORMAT(collections.PaidDate, '%d %b %y') AS PaidDate, DATE_FORMAT(collections.VarifyDate, '%d %b %y') AS VarifyDate, branchdata_2.BranchName,collections.CheckStatus, collections.BankName, collections.Remarks, collections.ChequeNo, collections.AmountPaid, DATE_FORMAT(collections.ChequeDate, '%d %b %y') AS chequeDate FROM  branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno INNER JOIN branchdata branchdata_2 ON branchmappingtable.SubBranch = branchdata_2.sno INNER JOIN collections ON branchdata_2.sno = collections.Branchid WHERE (branchdata.sno = @SuperBranch) AND (collections.PaymentType = 'Cheque') AND (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) OR (branchdata_1.SalesOfficeID = @SuperBranch) AND (collections.PaymentType = 'Cheque') AND (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) GROUP BY branchdata_2.BranchName, collections.Denominations, collections.PaidDate ORDER BY PaidDate");
                cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                dtCheque = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT Sno,Branchid, Name, Amount, Remarks,DATE_FORMAT(DOE, '%d %b %y') AS PaidDate, Receiptno, Agentid, PaymentType, CollectionType, CollectionFrom, CheckStatus, ChequeNo, VarifyDate, DATE_FORMAT(ChequeDate, '%d %b %y') AS chequeDate,BankName FROM cashcollections WHERE (CollectionType = 'Cheque') AND (Branchid = @SuperBranch) AND (DOE between @d1 and @d2)");
                cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                dtothercheques = vdm.SelectQuery(cmd).Tables[0];
            }
            if (Status == "Pending")
            {
               // cmd = new MySqlCommand("SELECT DATE_FORMAT(collections.PaidDate, '%d %b %y') AS PaidDate,DATE_FORMAT(collections.VarifyDate, '%d %b %y') AS VarifyDate,branchdata.BranchName,collections.CheckStatus,collections.BankName,collections.Remarks, collections.ChequeNo,collections.AmountPaid,DATE_FORMAT(collections.ChequeDate, '%d %b %y') AS chequeDate FROM branchdata INNER JOIN collections ON branchdata.sno = collections.Branchid INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (collections.PaymentType = 'Cheque') AND (branchmappingtable.SuperBranch = @SuperBranch) and(collections.CheckStatus=@CheckStatus) and (collections.tripid is null) and (collections.PaidDate between @d1 and @d2) GROUP BY branchdata.BranchName, collections.Denominations, collections.PaidDate order by collections.PaidDate");
                cmd = new MySqlCommand("SELECT DATE_FORMAT(collections.PaidDate, '%d %b %y') AS PaidDate, DATE_FORMAT(collections.VarifyDate, '%d %b %y') AS VarifyDate, branchdata_2.BranchName,collections.CheckStatus, collections.BankName, collections.Remarks, collections.ChequeNo, collections.AmountPaid, DATE_FORMAT(collections.ChequeDate, '%d %b %y') AS chequeDate FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno INNER JOIN branchdata branchdata_2 ON branchmappingtable.SubBranch = branchdata_2.sno INNER JOIN collections ON branchdata_2.sno = collections.Branchid WHERE (branchdata.sno = @SuperBranch) AND (collections.PaymentType = 'Cheque') AND (collections.CheckStatus = @CheckStatus) AND (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) OR (branchdata_1.SalesOfficeID = @SuperBranch) AND (collections.PaymentType = 'Cheque') AND (collections.CheckStatus = @CheckStatus) AND (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) GROUP BY branchdata_2.BranchName, collections.Denominations, collections.PaidDate ORDER BY PaidDate");
                cmd.Parameters.AddWithValue("@CheckStatus", 'P');
                cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                dtCheque = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT Sno,Branchid, Name, Amount, Remarks,DATE_FORMAT(DOE, '%d %b %y') AS PaidDate, Receiptno, Agentid, PaymentType, CollectionType, CollectionFrom, CheckStatus, ChequeNo, VarifyDate, DATE_FORMAT(ChequeDate, '%d %b %y') AS chequeDate,BankName FROM cashcollections WHERE (CollectionType = 'Cheque') AND (Branchid = @SuperBranch) AND (CheckStatus = 'P') AND (DOE between @d1 and @d2)");
                cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                dtothercheques = vdm.SelectQuery(cmd).Tables[0];
            }
            if (Status == "Verified")
            {
                //cmd = new MySqlCommand("SELECT DATE_FORMAT(collections.PaidDate, '%d %b %y') AS PaidDate,DATE_FORMAT(collections.VarifyDate, '%d %b %y') AS VarifyDate,branchdata.BranchName,collections.CheckStatus,collections.BankName,collections.Remarks, collections.ChequeNo,collections.AmountPaid,DATE_FORMAT(collections.ChequeDate, '%d %b %y') AS chequeDate FROM branchdata INNER JOIN collections ON branchdata.sno = collections.Branchid INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (collections.PaymentType = 'Cheque') AND (branchmappingtable.SuperBranch = @SuperBranch) and(collections.CheckStatus=@CheckStatus)  and (collections.tripid is null) and (collections.VarifyDate between @d1 and @d2) GROUP BY branchdata.BranchName, collections.Denominations, collections.PaidDate order by collections.PaidDate");
                cmd = new MySqlCommand("SELECT DATE_FORMAT(collections.PaidDate, '%d %b %y') AS PaidDate, DATE_FORMAT(collections.VarifyDate, '%d %b %y') AS VarifyDate, branchdata_2.BranchName,collections.CheckStatus, collections.BankName, collections.Remarks, collections.ChequeNo, collections.AmountPaid, DATE_FORMAT(collections.ChequeDate, '%d %b %y') AS chequeDate FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno INNER JOIN branchdata branchdata_2 ON branchmappingtable.SubBranch = branchdata_2.sno INNER JOIN collections ON branchdata_2.sno = collections.Branchid WHERE (branchdata.sno = @SuperBranch) AND (collections.PaymentType = 'Cheque') AND (collections.CheckStatus = @CheckStatus) AND (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) OR (branchdata_1.SalesOfficeID = @SuperBranch) AND (collections.PaymentType = 'Cheque') AND (collections.CheckStatus = @CheckStatus) AND (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) GROUP BY branchdata_2.BranchName, collections.Denominations, collections.PaidDate ORDER BY PaidDate");
                cmd.Parameters.AddWithValue("@CheckStatus", 'V');
                cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                dtCheque = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT Sno,Branchid, Name, Amount, Remarks,DATE_FORMAT(DOE, '%d %b %y') AS PaidDate, Receiptno, Agentid, PaymentType, CollectionType, CollectionFrom, CheckStatus, ChequeNo, VarifyDate, DATE_FORMAT(ChequeDate, '%d %b %y') AS chequeDate,BankName FROM cashcollections WHERE (CollectionType = 'Cheque') AND (Branchid = @SuperBranch) AND (CheckStatus = 'V') AND (VarifyDate between @d1 and @d2)");
                cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                dtothercheques = vdm.SelectQuery(cmd).Tables[0];
            }
            if (Status == "Bounce")
            {
                //cmd = new MySqlCommand("SELECT DATE_FORMAT(collections.PaidDate, '%d %b %y') AS PaidDate,DATE_FORMAT(collections.VarifyDate, '%d %b %y') AS VarifyDate,branchdata.BranchName,collections.CheckStatus,collections.BankName,collections.Remarks, collections.ChequeNo,collections.AmountPaid,DATE_FORMAT(collections.ChequeDate, '%d %b %y') AS chequeDate FROM branchdata INNER JOIN collections ON branchdata.sno = collections.Branchid INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (collections.PaymentType = 'Cheque') AND (branchmappingtable.SuperBranch = @SuperBranch) and(collections.CheckStatus=@CheckStatus) and (collections.tripid is null) and (collections.PaidDate between @d1 and @d2) GROUP BY branchdata.BranchName, collections.Denominations, collections.PaidDate order by collections.PaidDate");
                cmd = new MySqlCommand("SELECT DATE_FORMAT(collections.PaidDate, '%d %b %y') AS PaidDate, DATE_FORMAT(collections.VarifyDate, '%d %b %y') AS VarifyDate, branchdata_2.BranchName,collections.CheckStatus, collections.BankName, collections.Remarks, collections.ChequeNo, collections.AmountPaid, DATE_FORMAT(collections.ChequeDate, '%d %b %y') AS chequeDate FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SuperBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno INNER JOIN branchdata branchdata_2 ON branchmappingtable.SubBranch = branchdata_2.sno INNER JOIN collections ON branchdata_2.sno = collections.Branchid WHERE (branchdata.sno = @SuperBranch) AND (collections.PaymentType = 'Cheque') AND (collections.CheckStatus = @CheckStatus) AND (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) OR (branchdata_1.SalesOfficeID = @SuperBranch) AND (collections.PaymentType = 'Cheque') AND (collections.CheckStatus = @CheckStatus) AND (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) GROUP BY branchdata_2.BranchName, collections.Denominations, collections.PaidDate ORDER BY PaidDate");
                cmd.Parameters.AddWithValue("@CheckStatus", 'B');
                cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                 dtCheque = vdm.SelectQuery(cmd).Tables[0];
                 cmd = new MySqlCommand("SELECT Sno,Branchid, Name, Amount, Remarks,DATE_FORMAT(DOE, '%d %b %y') AS PaidDate, Receiptno, Agentid, PaymentType, CollectionType, CollectionFrom, CheckStatus, ChequeNo, VarifyDate, DATE_FORMAT(ChequeDate, '%d %b %y') AS chequeDate,BankName FROM cashcollections WHERE (CollectionType = 'Cheque') AND (Branchid = @SuperBranch) AND (CheckStatus = 'B') AND (DOE between @d1 and @d2) ");
                 cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
                 cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                 cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                 dtothercheques = vdm.SelectQuery(cmd).Tables[0];
            }
            if (Status == "Agent Wise")
            {
                cmd = new MySqlCommand("SELECT DATE_FORMAT(collections.PaidDate, '%d %b %y') AS PaidDate,collections.PaidDate AS pd, DATE_FORMAT(collections.VarifyDate, '%d %b %y') AS VarifyDate, branchdata.BranchName,collections.CheckStatus, collections.BankName,collections.Remarks, collections.ChequeNo, collections.AmountPaid, DATE_FORMAT(collections.ChequeDate, '%d %b %y') AS chequeDate FROM branchdata INNER JOIN collections ON branchdata.sno = collections.Branchid WHERE (collections.PaymentType = 'Cheque') AND (collections.PaidDate BETWEEN @d1 AND @d2) AND (collections.tripId IS NULL) AND (collections.Branchid = @agentid) GROUP BY branchdata.BranchName, collections.Denominations, collections.PaidDate ORDER BY pd");
                cmd.Parameters.AddWithValue("@agentid", ddlAgentName.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                dtCheque = vdm.SelectQuery(cmd).Tables[0];
            }
            if (Status == "Others")
            {
                cmd = new MySqlCommand("SELECT Sno,Branchid, Name, Amount, Remarks,DATE_FORMAT(DOE, '%d %b %y') AS PaidDate, Receiptno, Agentid, PaymentType, CollectionType, CollectionFrom, CheckStatus, ChequeNo, VarifyDate, DATE_FORMAT(ChequeDate, '%d %b %y') AS chequeDate,BankName FROM cashcollections WHERE (CollectionType = 'Cheque') AND (Branchid = @SuperBranch) AND (DOE between @d1 and @d2)");
                cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                dtothercheques = vdm.SelectQuery(cmd).Tables[0];
            }
            Report = new DataTable();
            Report.Columns.Add("ReceivedDate");
            Report.Columns.Add("chequeDate");
            Report.Columns.Add("VarifyDate");
            Report.Columns.Add("ReceivedFrom");
            Report.Columns.Add("BankName");
            Report.Columns.Add("BranchName");
            Report.Columns.Add("Status");
            Report.Columns.Add("ChequeNo");
            Report.Columns.Add("Amount").DataType = typeof(Double);
            Report.Columns.Add("Remarks");
            if (dtCheque.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtCheque.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["ReceivedDate"] = dr["PaidDate"].ToString();
                    newrow["chequeDate"] = dr["chequeDate"].ToString();
                    newrow["VarifyDate"] = dr["VarifyDate"].ToString();
                    newrow["BankName"] = dr["BankName"].ToString();
                    newrow["ReceivedFrom"] = "Agent";
                    newrow["BranchName"] = dr["BranchName"].ToString();
                    string ColStatus = dr["CheckStatus"].ToString();
                    string ChequeStatus = "";
                    if (ColStatus == "P")
                    {
                        ChequeStatus = "Pending";
                    }
                    if (ColStatus == "V")
                    {
                        ChequeStatus = "Verified";
                    }
                    if (ColStatus == "B")
                    {
                        ChequeStatus = "Bounce";
                    }
                    if (ColStatus == "R")
                    {
                        ChequeStatus = "Rejected";
                    }
                    newrow["Status"] = ChequeStatus;
                    newrow["ChequeNo"] = dr["ChequeNo"].ToString();
                    newrow["Amount"] = dr["AmountPaid"].ToString();
                    newrow["Remarks"] = dr["Remarks"].ToString();
                    Report.Rows.Add(newrow);
                }
                foreach (DataRow dr in dtothercheques.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["ReceivedDate"] = dr["PaidDate"].ToString();
                    newrow["chequeDate"] = dr["chequeDate"].ToString();
                    newrow["VarifyDate"] = dr["VarifyDate"].ToString();
                    newrow["BankName"] = dr["BankName"].ToString();
                    newrow["ReceivedFrom"] = "Others";
                    newrow["BranchName"] = dr["Name"].ToString();
                    string ColStatus = dr["CheckStatus"].ToString();
                    string ChequeStatus = "";
                    if (ColStatus == "P")
                    {
                        ChequeStatus = "Pending";
                    }
                    if (ColStatus == "V")
                    {
                        ChequeStatus = "Verified";
                    }
                    if (ColStatus == "B")
                    {
                        ChequeStatus = "Bounce";
                    }
                    if (ColStatus == "R")
                    {
                        ChequeStatus = "Rejected";
                    }
                    newrow["Status"] = ChequeStatus;
                    newrow["ChequeNo"] = dr["ChequeNo"].ToString();
                    newrow["Amount"] = dr["Amount"].ToString();
                    newrow["Remarks"] = dr["Remarks"].ToString();

                    Report.Rows.Add(newrow);
                }
                DataRow newTotal = Report.NewRow();
                newTotal["BranchName"] = "Total Amount";
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
            else
            {
                if (Status == "Others")
                {
                    foreach (DataRow dr in dtothercheques.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["ReceivedDate"] = dr["PaidDate"].ToString();
                        newrow["chequeDate"] = dr["chequeDate"].ToString();
                        newrow["VarifyDate"] = dr["VarifyDate"].ToString();
                        newrow["BankName"] = dr["BankName"].ToString();
                        newrow["ReceivedFrom"] = "Others";
                        newrow["BranchName"] = dr["Name"].ToString();
                        string ColStatus = dr["CheckStatus"].ToString();
                        string ChequeStatus = "";
                        if (ColStatus == "P")
                        {
                            ChequeStatus = "Pending";
                        }
                        if (ColStatus == "V")
                        {
                            ChequeStatus = "Verified";
                        }
                        if (ColStatus == "B")
                        {
                            ChequeStatus = "Bounce";
                        }
                        if (ColStatus == "R")
                        {
                            ChequeStatus = "Rejected";
                        }
                        newrow["Status"] = ChequeStatus;
                        newrow["ChequeNo"] = dr["ChequeNo"].ToString();
                        newrow["Amount"] = dr["Amount"].ToString();
                        newrow["Remarks"] = dr["Remarks"].ToString();

                        Report.Rows.Add(newrow);
                    }
                    DataRow newTotal = Report.NewRow();
                    newTotal["BranchName"] = "Total Amount";
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
                else
                {
                    pnlHide.Visible = false;
                    lblmsg.Text = "No Cheques were Found";
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
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
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        string Status = ddlStatus.SelectedValue;
        if (Status == "Agent Wise")
        {
            Panelinventory.Visible = true;
        }
        else
        {
            Panelinventory.Visible = false;

        }

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
}