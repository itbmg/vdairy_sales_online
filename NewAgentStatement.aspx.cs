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
public partial class NewAgentStatement : System.Web.UI.Page
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
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
            if (Session["salestype"].ToString() == "Plant")
            {
                PBranch.Visible = true;
                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("BranchName");
                dtBranch.Columns.Add("sno");
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
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
                cmd = new MySqlCommand("SELECT BranchName, sno FROM  branchdata WHERE (sno = @BranchID)");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtPlant.Rows)
                {
                    DataRow newrow = dtBranch.NewRow();
                    newrow["BranchName"] = dr["BranchName"].ToString();
                    newrow["sno"] = dr["sno"].ToString();
                    dtBranch.Rows.Add(newrow);
                }
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType)  ");
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
        //cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlDispName.DataSource = dtRoutedata;
        ddlDispName.DataTextField = "DispName";
        ddlDispName.DataValueField = "sno";
        ddlDispName.DataBind();
        ddlDispName.Items.Insert(0, new ListItem("Select", "0"));
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
            lblmsg.Text = "";
            pnlHide.Visible = true;
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            Report = new DataTable();
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
            Session["filename"] = "Statement of Account ->" + ddlAgentName.SelectedItem.Text;
            lblAgent.Text = ddlAgentName.SelectedItem.Text;
            lbl_fromDate.Text = txtFromdate.Text;
            lbl_selttodate.Text = txtTodate.Text;
            cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost),2) AS Totalsalevalue,ROUND(SUM(indents_subtable.DeliveryQty),2) AS DeliveryQty,products_category.Categoryname, productsdata.ProductName, DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate,branchdata.sno FROM productsdata INNER JOIN indents_subtable ON productsdata.sno = indents_subtable.Product_sno INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchdata.sno = @BranchID) GROUP BY productsdata.sno, IndentDate ORDER BY indents.I_date");
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtAgent = vdm.SelectQuery(cmd).Tables[0];
            if (dtAgent.Rows.Count <= 0)
            {
                cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost), 2) AS Totalsalevalue, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, MAX(indents.I_date) AS indentdate,branchdata.sno FROM productsdata INNER JOIN indents_subtable ON productsdata.sno = indents_subtable.Product_sno INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchdata.sno = @BranchID) AND (indents_subtable.DeliveryQty > 0)");
                cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
                DataTable dtAgent_lastDelivery = vdm.SelectQuery(cmd).Tables[0];
                if (dtAgent_lastDelivery.Rows.Count > 0)
                {
                    string dtlastdel = dtAgent_lastDelivery.Rows[0]["indentdate"].ToString();
                    if (dtlastdel != "")
                    {
                        fromdate = Convert.ToDateTime(dtlastdel).AddDays(1);
                    }
                    cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost),2) AS Totalsalevalue,ROUND(SUM(indents_subtable.DeliveryQty),2) AS DeliveryQty,products_category.Categoryname, productsdata.ProductName, DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate,branchdata.sno FROM productsdata INNER JOIN indents_subtable ON productsdata.sno = indents_subtable.Product_sno INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchdata.sno = @BranchID) GROUP BY productsdata.sno, IndentDate ORDER BY indents.I_date");
                    cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                    dtAgent = vdm.SelectQuery(cmd).Tables[0];
                }
            }
            cmd = new MySqlCommand("SELECT Branchid, AmountPaid, Remarks, DATE_FORMAT(PaidDate, '%d/%b/%y') AS PDate, PayTime, EmpID, ReceiptNo, VarifyDate, TransactionType, AmountDebited, DiffAmount, SalesOfficeID, Status FROM collections WHERE (Branchid = @BranchID) AND (TransactionType = @type) AND (Status = @status) AND (PaidDate BETWEEN @d1 AND @d2)");
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            cmd.Parameters.AddWithValue("@type", "Debit");
            cmd.Parameters.AddWithValue("@status", "1");
            DataTable dtAgent_Debits = vdm.SelectQuery(cmd).Tables[0];
            //cmd = new MySqlCommand("SELECT SUM(AmountPaid) AS AmountPaid, DATE_FORMAT(PaidDate, '%d/%b/%y') AS PaidDate, CheckStatus FROM collections WHERE (Branchid = @BranchID) AND (PaidDate BETWEEN @d1 AND @d2) AND (CheckStatus <> 'P' OR  CheckStatus IS NULL) GROUP BY PaidDate");
            cmd = new MySqlCommand("SELECT SUM(AmountPaid) AS AmountPaid, DATE_FORMAT(PaidDate, '%d/%b/%y') AS PDate, CheckStatus,PaymentType FROM collections WHERE (Branchid = @BranchID) AND (PaidDate BETWEEN @d1 AND @d2) AND (CheckStatus IS NULL)  GROUP BY PDate");
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable dtAgentDayWiseCollection = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT SUM(AmountPaid) AS AmountPaid, DATE_FORMAT(PaidDate, '%d/%b/%y') AS PDate FROM collections WHERE (Branchid = @BranchID) AND (PaidDate BETWEEN @d1 AND @d2) and (tripId is NULL) AND ((PaymentType = 'Incentive') OR (PaymentType = 'Journal Voucher')) GROUP BY PDate");
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable dtAgentIncentive = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT SUM(AmountPaid) AS AmountPaid, DATE_FORMAT(VarifyDate, '%d/%b/%y') AS VarifyDate, CheckStatus FROM collections WHERE (Branchid = @BranchID) AND (CheckStatus = 'V') AND (VarifyDate BETWEEN @d1 AND @d2) GROUP BY VarifyDate");
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable dtAgentchequeCollection = vdm.SelectQuery(cmd).Tables[0];
            double closingvalue = 0;
            double totdebitedamount = 0;
            DataTable dtSaleCollection = new DataTable();
            DataView view = new DataView(dtAgent);
            DataTable produtstbl = view.ToTable(true, "ProductName", "Categoryname");
            Report = new DataTable();
            Report.Columns.Add("SNo");
            Report.Columns.Add("DeliverDate");
            int count = 0;
            //foreach (DataRow dr in produtstbl.Rows)
            //{
            //    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
            //    count++;
            //}
            Report.Columns.Add("Total").DataType = typeof(Double);
            Report.Columns.Add("Sale Value").DataType = typeof(Double);
            Report.Columns.Add("Opp Bal");
            Report.Columns.Add("Total Amount").DataType = typeof(Double);
            Report.Columns.Add("Paid Amount").DataType = typeof(Double);
            Report.Columns.Add("Amount Debited").DataType = typeof(Double);
            Report.Columns.Add("Incentive/JV", typeof(Double)).SetOrdinal(count + 8);
            Report.Columns.Add("Bal Amount");
            int i = 1;
            if (dtAgent.Rows.Count > 0)
            {
                cmd = new MySqlCommand("SELECT Sno, SalesOfficeId, RouteId, AgentId, DATE_FORMAT(IndentDate, '%d %b %y') AS IndentDate, EntryDate, OppBalance, SaleQty, SaleValue, ReceivedAmount, ClosingBalance, DiffAmount FROM tempduetrasactions WHERE (IndentDate BETWEEN @d1 AND @d2) AND (AgentId = @SOID) order by EntryDate");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
                cmd.Parameters.AddWithValue("@SOID", ddlAgentName.SelectedValue);
                DataTable dtOpp = vdm.SelectQuery(cmd).Tables[0];
                DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
                cmd = new MySqlCommand("SELECT totalsaleamount.totalsale, totalsaleamount.Branch_id, SUM(collections.AmountPaid) AS amountpaid FROM (SELECT SUM(indentssub.DeliveryQty * indentssub.UnitCost) AS totalsale, indents.Branch_id FROM indents INNER JOIN (SELECT IndentNo, Product_sno, Qty, Cost, Remark, DeliveryQty, Status, D_date, unitQty, UnitCost, Sno, PaymentStatus, LeakQty, OTripId, DTripId,DelTime FROM indents_subtable WHERE (D_date BETWEEN @starttime AND @endtime)) indentssub ON indents.IndentNo = indentssub.IndentNo WHERE (indents.Branch_id = @BranchID) GROUP BY indents.Branch_id) totalsaleamount INNER JOIN (SELECT Branchid, UserData_sno, AmountPaid, Denominations, Remarks, Sno, PaidDate, PaymentType, tripId, CheckStatus, ReturnDenomin, PayTime, VEmpID, ChequeNo, EmpID, ReceiptNo FROM collections collections_1 WHERE (Branchid = @BranchID) AND (PaidDate BETWEEN @starttime AND @endtime) AND (CheckStatus IS NULL) OR (Branchid = @BranchID) AND (CheckStatus = 'V') AND (VarifyDate BETWEEN @starttime AND @endtime)) collections ON totalsaleamount.Branch_id = collections.Branchid");
                cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(ServerDateCurrentdate));
                dtSaleCollection = vdm.SelectQuery(cmd).Tables[0];
                double aopp = 0; double sale = 0;
                double paidamt = 0; double balance = 0;
                foreach (DataRow dr in dtOpp.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    double totqty = 0;
                    //foreach (DataRow drdtchequeotal in dtAgent.Select("sno='" + dr["AgentId"].ToString() + "'AND IndentDate='" + dr["IndentDate"].ToString() + "'"))
                    //{
                    //    double DQty = 0;
                    //    double.TryParse(drdtchequeotal["DeliveryQty"].ToString(), out DQty);
                    //    newrow[drdtchequeotal["ProductName"].ToString()] = DQty;
                    //    totqty += DQty;
                    //}
                    DateTime date = Convert.ToDateTime(dr["indentDate"].ToString());
                    newrow["SNo"] = i;
                    newrow["Total"] = totqty;
                    newrow["DeliverDate"] = date.AddDays(1).ToString("dd/MM/yyyy");
                    double.TryParse(dr["SaleValue"].ToString(), out sale);
                    newrow["Sale Value"] = sale;
                    double.TryParse(dr["OppBalance"].ToString(), out aopp);
                    double debitamount = 0;
                    double.TryParse(dr["DiffAmount"].ToString(), out debitamount);
                    newrow["Opp Bal"] = aopp - debitamount;
                    double.TryParse(dr["ReceivedAmount"].ToString(), out paidamt);
                    newrow["Paid Amount"] = paidamt;
                    newrow["Amount Debited"] = debitamount;
                    newrow["Total Amount"] = aopp + sale;
                    double.TryParse(dr["ClosingBalance"].ToString(), out balance);
                    newrow["Bal Amount"] = dr["ClosingBalance"].ToString();
                    double closing = 0;
                    double.TryParse(dr["ClosingBalance"].ToString(), out closing);
                    closingvalue = closing;
                    fromdate = date.AddDays(2);
                    if (sale + paidamt > 0)
                    {
                        Report.Rows.Add(newrow);
                    }
                    i++;
                }
            }
            //TimeSpan dateSpan = todate.Subtract(fromdate);
            //int NoOfdays = dateSpan.Days;
            //NoOfdays = NoOfdays + 1;
            //for (int j = 0; j < NoOfdays; j++)
            //{
            //    i++;
            //    DataRow newrow = Report.NewRow();
            //    //newrow["SNo"] = i;
            //    string dtcount = fromdate.AddDays(j).ToString();
            //    DateTime dtDOE = Convert.ToDateTime(dtcount);
            //    //string dtdate1 = branch["IndentDate"].ToString();
            //    string dtdate1 = dtDOE.AddDays(-1).ToString();
            //    DateTime dtDOE1 = Convert.ToDateTime(dtdate1).AddDays(1);
            //    string ChangedTime1 = dtDOE1.ToString("dd/MMM/yy");
            //    string ChangedTime2 = dtDOE.AddDays(-1).ToString("dd MMM yy");
            //    newrow["SNo"] = i;
            //    newrow["DeliverDate"] = ChangedTime1;
            //    double amtpaid = 0;
            //    double incentiveamtpaid = 0;
            //    double totamtpaid = 0;
            //    double totincentiveamtpaid = 0;
            //    double totchequeamtpaid = 0;
            //    double debitedamount = 0;
            //    foreach (DataRow drdtclubtotal in dtAgentDayWiseCollection.Select("PDate='" + ChangedTime1 + "'"))
            //    {
            //        double.TryParse(drdtclubtotal["AmountPaid"].ToString(), out totamtpaid);
            //        amtpaid += totamtpaid;
            //    }
            //    foreach (DataRow drdtincentive in dtAgentIncentive.Select("PDate='" + ChangedTime1 + "'"))
            //    {
            //        double.TryParse(drdtincentive["AmountPaid"].ToString(), out totincentiveamtpaid);

            //        incentiveamtpaid += totincentiveamtpaid;
            //    }
            //    foreach (DataRow drdtchequeotal in dtAgentchequeCollection.Select("VarifyDate='" + ChangedTime1 + "'"))
            //    {
            //        double.TryParse(drdtchequeotal["AmountPaid"].ToString(), out totchequeamtpaid);
            //        amtpaid += totchequeamtpaid;
            //    }
            //    double totsale = 0;
            //    double totamt = 0;
            //    if (dtSaleCollection.Rows.Count > 0)
            //    {
            //        double.TryParse(dtSaleCollection.Rows[0]["totalsale"].ToString(), out totsale);
            //        double.TryParse(dtSaleCollection.Rows[0]["amountpaid"].ToString(), out totamt);
            //    }
            //    else
            //    {
            //        totsale = 0;
            //        totamt = 0;
            //    }
            //    double total = 0;
            //    double Amount = 0;
            //    foreach (DataRow dr in dtAgent.Rows)
            //    {
            //        if (ChangedTime2 == dr["IndentDate"].ToString())
            //        {
            //            double qtyvalue = 0;
            //            double DQty = 0;
            //            double.TryParse(dr["DeliveryQty"].ToString(), out DQty);
            //            newrow[dr["ProductName"].ToString()] = DQty;
            //            double.TryParse(dr["Totalsalevalue"].ToString(), out qtyvalue);
            //            Amount += qtyvalue;
            //            total += DQty;
            //        }
            //    }
            //    foreach (DataRow dr in dtAgent_Debits.Rows)
            //    {
            //        if (ChangedTime1 == dr["PDate"].ToString())
            //        {
            //            double amountDebited = 0;
            //            double.TryParse(dr["AmountDebited"].ToString(), out amountDebited);
            //            debitedamount += amountDebited;
            //            totdebitedamount += amountDebited;
            //        }
            //    }
            //    double aopp = closingvalue + totamt - totsale;
            //    double actbal = 0;
            //    actbal = aopp;
            //    if (totdebitedamount == 0.0)
            //    {
            //        if (closingvalue == 0.0)
            //        {
            //            aopp = aopp;
            //        }
            //        else
            //        {
            //            aopp = closingvalue;
            //        }
            //    }
            //    else
            //    {
            //        if (debitedamount != 0.0)
            //        {

            //            if (closingvalue == 0.0)
            //            {
            //                aopp = aopp;
            //            }
            //            else
            //            {
            //                aopp = closingvalue;
            //            }
            //        }
            //        else
            //        {
            //            aopp = Math.Abs(aopp);
            //            aopp = totdebitedamount - aopp;
            //            aopp = closingvalue;
            //        }
            //    }
            //    if (totsale == 0)
            //    {
            //        aopp = closingvalue;
            //    }
            //    newrow["Total"] = total;
            //    newrow["Sale Value"] = Amount;
            //    newrow["Opp Bal"] = Math.Round(closingvalue, 2);
            //    double totalamt = closingvalue + Amount + debitedamount;
            //    newrow["Total Amount"] = Math.Round(totalamt, 2);
            //    //newrow["Paid Amount"] = amtpaid - incentiveamtpaid;
            //    newrow["Paid Amount"] = amtpaid - incentiveamtpaid;
            //    newrow["Incentive/JV"] = incentiveamtpaid;
            //    newrow["Amount Debited"] = debitedamount;
            //    // double tot_amount = amtpaid + incentiveamtpaid;
            //    double totalbalance = totalamt - amtpaid;
            //    newrow["Bal Amount"] = Math.Round(totalbalance, 2);
            //    closingvalue = totalbalance;
            //    if (Amount + amtpaid + debitedamount > 0)
            //    {
            //        Report.Rows.Add(newrow);
            //    }
            //}
            //for (int j = 0; j < NoOfdays; j
            //}  //{
            //}
            //foreach (DataRow dragent in dtAgent.Rows)
            //{
            //    double total = 0, Amount = 0;
            //    DataRow newrow = Report.NewRow();
            //    DateTime inddate = Convert.ToDateTime(dragent["IndentDate"].ToString());
            //    string indentdate = inddate.ToString("dd/MM/yyyy");
            //    DataRow[] drdates = dtindentdate.Select("DeliverDate='" + indentdate + "'");
            //    if (drdates.Length > 0)
            //    {
            //        //foreach (DataRow drv in drvoucher)
            //        //{
            //        //    VoucherNo = drv.ItemArray[0].ToString();
            //        //}
            //    }
            //    else
            //    {
            //        foreach (DataRow dr in dtAgent.Rows)
            //        {
            //            if (indentdate == dr["IndentDate"].ToString())
            //            {
            //                double qtyvalue1 = 0;
            //                double DQty1 = 0;
            //                double.TryParse(dr["DeliveryQty"].ToString(), out DQty1);
            //                newrow[dr["ProductName"].ToString()] = DQty1;
            //                double.TryParse(dr["Totalsalevalue"].ToString(), out qtyvalue1);
            //                Amount += qtyvalue1;
            //                total += DQty1;
            //            }
            //        }

            //        double qtyvalue = 0;
            //        double DQty = 0;
            //        double.TryParse(dragent["DeliveryQty"].ToString(), out DQty);
            //        newrow[dragent["ProductName"].ToString()] = DQty;
            //        double.TryParse(dragent["Totalsalevalue"].ToString(), out qtyvalue);
            //        Amount += qtyvalue;
            //        total += DQty;
            //        foreach (DataRow drdtclubtotal in dtAgentDayWiseCollection.Select("PDate='" + indentdate + "'"))
            //        {
            //            double.TryParse(drdtclubtotal["AmountPaid"].ToString(), out totamtpaid);
            //            amtpaid += totamtpaid;
            //        }
            //        foreach (DataRow drdtincentive in dtAgentIncentive.Select("PDate='" + indentdate + "'"))
            //        {
            //            double.TryParse(drdtincentive["AmountPaid"].ToString(), out totincentiveamtpaid);
            //            incentiveamtpaid += totincentiveamtpaid;
            //        }
            //        foreach (DataRow drdtchequeotal in dtAgentchequeCollection.Select("VarifyDate='" + indentdate + "'"))
            //        {
            //            double.TryParse(drdtchequeotal["AmountPaid"].ToString(), out totchequeamtpaid);

            //            amtpaid += totchequeamtpaid;
            //        }
            //        double totsale = 0;
            //        double totamt = 0;
            //        if (dtSaleCollection.Rows.Count > 0)
            //        {
            //            double.TryParse(dtSaleCollection.Rows[0]["totalsale"].ToString(), out totsale);
            //            double.TryParse(dtSaleCollection.Rows[0]["amountpaid"].ToString(), out totamt);
            //        }
            //        foreach (DataRow drdtclubtotal in dtAgent_Debits.Select("PDate='" + indentdate + "'"))
            //        {
            //            double amountDebited = 0;
            //            double.TryParse(drdtclubtotal["AmountDebited"].ToString(), out amountDebited);
            //            debitedamount += amountDebited;
            //            totdebitedamount += amountDebited;
            //        }
            //        double aopp = closingvalue + totamt - totsale;
            //        double actbal = 0;
            //        actbal = aopp;
            //        if (totdebitedamount == 0.0)
            //        {
            //            if (closingvalue == 0.0)
            //            {
            //                aopp = aopp;
            //            }
            //            else
            //            {
            //                aopp = closingvalue;
            //            }
            //        }
            //        else
            //        {
            //            if (debitedamount != 0.0)
            //            {

            //                if (closingvalue == 0.0)
            //                {
            //                    aopp = aopp;
            //                }
            //                else
            //                {
            //                    aopp = closingvalue;
            //                }
            //            }
            //            else
            //            {
            //                aopp = Math.Abs(aopp);
            //                aopp = totdebitedamount - aopp;
            //                aopp = closingvalue;
            //            }
            //        }
            //        if (totsale == 0)
            //        {
            //            aopp = closingvalue;
            //        }
            //        // DateTime date = Convert.ToDateTime(indentdate);
            //        newrow["DeliverDate"] = indentdate;
            //        newrow["Total"] = total;
            //        newrow["Sale Value"] = Amount;
            //        newrow["Opp Bal"] = Math.Round(closingvalue, 2);
            //        double totalamt = closingvalue + Amount + debitedamount;
            //        newrow["Total Amount"] = Math.Round(totalamt, 2);
            //        //newrow["Paid Amount"] = amtpaid - incentiveamtpaid;
            //        newrow["Paid Amount"] = amtpaid - incentiveamtpaid;
            //        newrow["Incentive/JV"] = incentiveamtpaid;
            //        newrow["Amount Debited"] = debitedamount;
            //        // double tot_amount = amtpaid + incentiveamtpaid;
            //        double totalbalance = totalamt - amtpaid;
            //        newrow["Bal Amount"] = Math.Round(totalbalance, 2);
            //        closingvalue = totalbalance;
            //        Report.Rows.Add(newrow);
            //    }
            //}
            DataRow newvartical = Report.NewRow();
            newvartical["DeliverDate"] = "Total";
            double val = 0.0;
            foreach (DataColumn dc in Report.Columns)
            {
                if (dc.DataType == typeof(Double))
                {
                    val = 0.0;
                    double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                    newvartical[dc.ToString()] = val;
                }
            }
            Report.Rows.Add(newvartical);
            foreach (DataColumn col in Report.Columns)
            {
                string Pname = col.ToString();
                string ProductName = col.ToString();
                ProductName = GetSpace(ProductName);
                Report.Columns[Pname].ColumnName = ProductName;
            }
            grdReports.DataSource = Report;
            grdReports.DataBind();
            Session["xportdata"] = Report;
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