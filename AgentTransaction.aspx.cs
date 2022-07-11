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

public partial class AgentTransaction : System.Web.UI.Page
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
        Button1.Visible = false;
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) and (branchdata.flag<>0) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) and (branchdata.flag<>0)");
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
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                PBranch.Visible = false;
                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (dispatch.flag = 1) AND (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
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
        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE   (dispatch.flag = 1) AND (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
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
        cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutes ON dispatch_sub.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (dispatch.sno = @dispsno) AND  (dispatch.flag = 1)");
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
            cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost),2) AS Totalsalevalue,ROUND(SUM(indents_subtable.DeliveryQty),2) AS DeliveryQty,products_category.Categoryname, productsdata.ProductName, DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate FROM productsdata INNER JOIN indents_subtable ON productsdata.sno = indents_subtable.Product_sno INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchdata.sno = @BranchID) and (indents_subtable.DeliveryQty<>'0') GROUP BY productsdata.sno, IndentDate ORDER BY products_category.sno,indents.I_date");
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtAgent = vdm.SelectQuery(cmd).Tables[0];
            if (dtAgent.Rows.Count <= 0)
            {
                cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost), 2) AS Totalsalevalue, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, MAX(indents.I_date) AS indentdate FROM productsdata INNER JOIN indents_subtable ON productsdata.sno = indents_subtable.Product_sno INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchdata.sno = @BranchID) AND (indents_subtable.DeliveryQty > 0)");
                cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
                DataTable dtAgent_lastDelivery = vdm.SelectQuery(cmd).Tables[0];
                if (dtAgent_lastDelivery.Rows.Count > 0)
                {
                    string dtlastdel = dtAgent_lastDelivery.Rows[0]["indentdate"].ToString();
                    if (dtlastdel != "")
                    {
                        fromdate = Convert.ToDateTime(dtlastdel).AddDays(1);
                    }
                    cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost),2) AS Totalsalevalue,ROUND(SUM(indents_subtable.DeliveryQty),2) AS DeliveryQty,products_category.Categoryname, productsdata.ProductName, DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate FROM productsdata INNER JOIN indents_subtable ON productsdata.sno = indents_subtable.Product_sno INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchdata.sno = @BranchID) and (indents_subtable.DeliveryQty<>'0') GROUP BY productsdata.sno, IndentDate ORDER BY products_category.sno,indents.I_date");
                    cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                    dtAgent = vdm.SelectQuery(cmd).Tables[0];
                }
            }
            cmd = new MySqlCommand("SELECT Sum(AmountPaid) as AmountPaid , Remarks,  PayTime, EmpID, ReceiptNo, VarifyDate, TransactionType, AmountDebited, DiffAmount, SalesOfficeID, Status FROM collections WHERE (Branchid = @BranchID) AND (TransactionType = @type) AND (Status = @status) AND (PaidDate BETWEEN @d1 AND @d2)");
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            cmd.Parameters.AddWithValue("@type", "Debit");
            cmd.Parameters.AddWithValue("@status", "1");
            DataTable dtdebitamount = vdm.SelectQuery(cmd).Tables[0];
            double debitprice = 0;
            foreach (DataRow dr in dtdebitamount.Rows)
            {
                double amountDebited = 0;
                double.TryParse(dr["AmountDebited"].ToString(), out amountDebited);
                debitprice += amountDebited;
            }
            cmd = new MySqlCommand("SELECT Branchid, AmountPaid, Remarks, DATE_FORMAT(PaidDate, '%d/%b/%y') AS PDate, PayTime, EmpID, ReceiptNo, VarifyDate, TransactionType, AmountDebited, DiffAmount, SalesOfficeID, Status FROM collections WHERE (Branchid = @BranchID) AND (TransactionType = @type) AND (Status = @status) AND (PaidDate BETWEEN @d1 AND @d2)");
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            cmd.Parameters.AddWithValue("@type", "Debit");
            cmd.Parameters.AddWithValue("@status", "1");
            DataTable dtAgent_Debits = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT Amount FROM branchaccounts WHERE (BranchId = @BranchID)");
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            DataTable dtAgent_presentopp = vdm.SelectQuery(cmd).Tables[0];
            double agentpresentopp = 0;
            double.TryParse(dtAgent_presentopp.Rows[0]["Amount"].ToString(), out agentpresentopp);
            agentpresentopp = agentpresentopp - debitprice;
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

            if (dtAgent.Rows.Count > 0)
            {
                DataView view = new DataView(dtAgent);
                DataTable produtstbl = view.ToTable(true, "ProductName", "Categoryname");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("DeliverDate");
                int count = 0;
                foreach (DataRow dr in produtstbl.Rows)
                {

                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                    count++;
                }
                Report.Columns.Add("Total", typeof(Double)).SetOrdinal(count + 2);
                Report.Columns.Add("Sale Value", typeof(Double)).SetOrdinal(count + 3);
                Report.Columns.Add("Amount Debited", typeof(Double)).SetOrdinal(count + 4);
                Report.Columns.Add("Opp Bal").SetOrdinal(count + 5);
                Report.Columns.Add("Total Amount").SetOrdinal(count + 6);
                Report.Columns.Add("Paid Amount", typeof(Double)).SetOrdinal(count + 7);
                Report.Columns.Add("Incentive/JV", typeof(Double)).SetOrdinal(count + 8);
                Report.Columns.Add("Bal Amount").SetOrdinal(count + 9);
                DataTable distincttable = view.ToTable(true, "IndentDate");
                int i = 1;
                double oppcarry = 0;
                TimeSpan dateSpan = todate.Subtract(fromdate);
                int NoOfdays = dateSpan.Days;
                NoOfdays = NoOfdays + 1;
                double totdebitedamount = 0;
                DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
                cmd = new MySqlCommand("SELECT totalsaleamount.totalsale, totalsaleamount.Branch_id, SUM(collections.AmountPaid) AS amountpaid FROM (SELECT SUM(indentssub.DeliveryQty * indentssub.UnitCost) AS totalsale, indents.Branch_id FROM indents INNER JOIN (SELECT IndentNo, Product_sno, Qty, Cost, Remark, DeliveryQty, Status, D_date, unitQty, UnitCost, Sno, PaymentStatus, LeakQty, OTripId, DTripId,DelTime FROM indents_subtable WHERE (D_date BETWEEN @starttime AND @endtime)) indentssub ON indents.IndentNo = indentssub.IndentNo WHERE (indents.Branch_id = @BranchID) GROUP BY indents.Branch_id) totalsaleamount INNER JOIN (SELECT Branchid, UserData_sno, AmountPaid, Denominations, Remarks, Sno, PaidDate, PaymentType, tripId, CheckStatus, ReturnDenomin, PayTime, VEmpID, ChequeNo, EmpID, ReceiptNo FROM collections collections_1 WHERE (Branchid = @BranchID) AND (PaidDate BETWEEN @starttime AND @endtime) AND (CheckStatus IS NULL) OR (Branchid = @BranchID) AND (CheckStatus = 'V') AND (VarifyDate BETWEEN @starttime AND @endtime)) collections ON totalsaleamount.Branch_id = collections.Branchid");
                ////  cmd = new MySqlCommand("SELECT totalsaleamount.totalsale, totalsaleamount.Branch_id, SUM(collections.AmountPaid) AS amountpaid FROM (SELECT SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS totalsale, indents.Branch_id FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo WHERE (indents.I_date BETWEEN @starttime AND @endtime) AND (indents.Branch_id = @BranchID) GROUP BY indents.Branch_id) totalsaleamount INNER JOIN (SELECT Branchid, UserData_sno, AmountPaid, Denominations, Remarks, Sno, PaidDate, PaymentType, tripId, CheckStatus, ReturnDenomin, PayTime, VEmpID, ChequeNo, EmpID, ReceiptNo FROM collections collections_1 WHERE (Branchid = @BranchID) AND (PaidDate BETWEEN @starttime AND @endtime) AND (CheckStatus IS NULL) OR (Branchid = @BranchID) AND (CheckStatus = 'V') AND (VarifyDate BETWEEN @starttime AND @endtime)) collections ON totalsaleamount.Branch_id = collections.Branchid"); 
                cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(ServerDateCurrentdate));
                DataTable dtSaleCollection = vdm.SelectQuery(cmd).Tables[0];
                double totsale = 0;
                double totamt = 0;
                for (int j = 0; j < NoOfdays; j++)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    string dtcount = fromdate.AddDays(j).ToString();
                    DateTime dtDOE = Convert.ToDateTime(dtcount);
                    //string dtdate1 = branch["IndentDate"].ToString();
                    string dtdate1 = dtDOE.AddDays(-1).ToString();
                    DateTime dtDOE1 = Convert.ToDateTime(dtdate1).AddDays(1);
                    string ChangedTime1 = dtDOE1.ToString("dd/MMM/yy");
                    string ChangedTime2 = dtDOE.AddDays(-1).ToString("dd MMM yy");
                    newrow["DeliverDate"] = ChangedTime1;
                    double amtpaid = 0;
                    double incentiveamtpaid = 0;
                    double totamtpaid = 0;
                    double totincentiveamtpaid = 0;
                    double totchequeamtpaid = 0;
                    double debitedamount = 0;
                    foreach (DataRow drdtclubtotal in dtAgentDayWiseCollection.Select("PDate='" + ChangedTime1 + "'"))
                    {
                        double.TryParse(drdtclubtotal["AmountPaid"].ToString(), out totamtpaid);
                        amtpaid += totamtpaid;
                    }
                    foreach (DataRow drdtincentive in dtAgentIncentive.Select("PDate='" + ChangedTime1 + "'"))
                    {
                        double.TryParse(drdtincentive["AmountPaid"].ToString(), out totincentiveamtpaid);

                        incentiveamtpaid += totincentiveamtpaid;
                    }
                    foreach (DataRow drdtchequeotal in dtAgentchequeCollection.Select("VarifyDate='" + ChangedTime1 + "'"))
                    {
                        double.TryParse(drdtchequeotal["AmountPaid"].ToString(), out totchequeamtpaid);

                        amtpaid += totchequeamtpaid;
                    }
                    if (dtSaleCollection.Rows.Count > 0)
                    {
                        double.TryParse(dtSaleCollection.Rows[0]["totalsale"].ToString(), out totsale);
                        double.TryParse(dtSaleCollection.Rows[0]["amountpaid"].ToString(), out totamt);
                    }
                    else
                    {
                        totsale = 0;
                        totamt = 0;
                    }
                    double total = 0;
                    double Amount = 0;
                    foreach (DataRow dr in dtAgent.Rows)
                    {
                        if (ChangedTime2 == dr["IndentDate"].ToString())
                        {
                            double qtyvalue = 0;
                            double DQty = 0;
                            double.TryParse(dr["DeliveryQty"].ToString(), out DQty);
                            newrow[dr["ProductName"].ToString()] = DQty;
                            double.TryParse(dr["Totalsalevalue"].ToString(), out qtyvalue);
                            Amount += qtyvalue;
                            total += DQty;
                        }
                    }
                    foreach (DataRow dr in dtAgent_Debits.Rows)
                    {
                        if (ChangedTime1 == dr["PDate"].ToString())
                        {
                            double amountDebited = 0;

                            double.TryParse(dr["AmountDebited"].ToString(), out amountDebited);
                            debitedamount += amountDebited;
                            totdebitedamount += amountDebited;
                        }
                    }
                    double aopp = agentpresentopp + totamt - totsale;
                    double actbal = 0;
                    actbal = aopp;
                    if (totdebitedamount == 0.0)
                    {
                        if (oppcarry == 0.0)
                        {
                            aopp = aopp;
                        }
                        else
                        {
                            aopp = oppcarry;
                        }
                    }
                    else
                    {
                        if (debitedamount != 0.0)
                        {
                            if (oppcarry == 0.0)
                            {
                                aopp = aopp;
                            }
                            else
                            {
                                aopp = oppcarry;
                            }
                        }
                        else
                        {
                            aopp = Math.Abs(aopp);
                            aopp = totdebitedamount - aopp;
                            aopp = oppcarry;
                        }
                    }
                    if (totsale == 0)
                    {
                        aopp = oppcarry;
                    }
                    newrow["Total"] = total;
                    newrow["Sale Value"] = Math.Round(Amount);

                    newrow["Opp Bal"] = Math.Round(aopp);
                    double totalamt = aopp + Amount + debitedamount;
                    newrow["Total Amount"] = Math.Round(totalamt);
                    //newrow["Paid Amount"] = amtpaid - incentiveamtpaid;
                    newrow["Paid Amount"] = amtpaid - incentiveamtpaid;
                    newrow["Incentive/JV"] = incentiveamtpaid;
                    newrow["Amount Debited"] = debitedamount;
                    double tot_amount = amtpaid;
                    double totalbalance = totalamt - tot_amount;
                    newrow["Bal Amount"] = Math.Round(totalbalance);
                    oppcarry = totalbalance;
                    if (Amount + amtpaid + debitedamount != 0)
                    {
                        Report.Rows.Add(newrow);
                        i++;
                    }
                    totsale = totsale - Amount;
                }
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
                GridView grd = grdReports;
                grd.DataSource = Report;
                grd.DataBind();
                Session["xportdata"] = Report;
            }
            else
            {
                pnlHide.Visible = false;
                lblmsg.Text = "No data were found";
                grdReports.DataSource = Report;
                grdReports.DataBind();
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
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            vdm = new VehicleDBMgr();
            DataTable dtbalance = (DataTable)Session["xportdata"];
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            foreach (DataRow dr in dtbalance.Rows)
            {
                double openingbalance = 0;
                double.TryParse(dr["Opp Bal "].ToString(), out openingbalance);
                double paidamout = 0;
                double.TryParse(dr["Paid Amount "].ToString(), out paidamout);
                double ince_Jv_amount = 0;
                double.TryParse(dr["Incentive/JV "].ToString(), out ince_Jv_amount);
                double closing = 0;
                double.TryParse(dr["Bal Amount "].ToString(), out closing);
                double totalpaidamount = paidamout + ince_Jv_amount;
                //double closingamount = openingbalance - totalpaidamount;
                if (dr["DeliverDate "].ToString() != "Total")
                {
                    DateTime Date = Convert.ToDateTime(dr["DeliverDate "].ToString()).AddDays(-1);
                    string inddate = Date.ToString("yyyy-MM-dd");
                    cmd = new MySqlCommand("update agent_bal_trans  set opp_balance=@opp_balance,salesvalue=@salesvalue,paidamount=@paidamount,clo_balance=@clo_balance  where agentid=@agentid and inddate=@inddate");
                    cmd.Parameters.AddWithValue("@opp_balance", openingbalance);
                    cmd.Parameters.AddWithValue("@salesvalue", dr["Sale Value "].ToString());
                    cmd.Parameters.AddWithValue("@paidamount", totalpaidamount);
                    cmd.Parameters.AddWithValue("@clo_balance", closing);
                    cmd.Parameters.AddWithValue("@agentid", ddlAgentName.SelectedValue);
                    cmd.Parameters.AddWithValue("@inddate", inddate);
                    if (vdm.Update(cmd) == 0)
                    {
                        cmd = new MySqlCommand("insert into agent_bal_trans(agentid,opp_balance,inddate,salesvalue,paidamount,clo_balance,createdate,entryby)values(@agentid,@opp_balance,@inddate,@salesvalue,@paidamount,@clo_balance,@createdate,@entryby)");
                        cmd.Parameters.AddWithValue("@agentid", ddlAgentName.SelectedValue);
                        cmd.Parameters.AddWithValue("@opp_balance", openingbalance);
                        cmd.Parameters.AddWithValue("@inddate", inddate);
                        cmd.Parameters.AddWithValue("@salesvalue", dr["Sale Value "].ToString());
                        cmd.Parameters.AddWithValue("@paidamount", totalpaidamount);
                        cmd.Parameters.AddWithValue("@clo_balance", closing);
                        cmd.Parameters.AddWithValue("@createdate", ServerDateCurrentdate);
                        cmd.Parameters.AddWithValue("@entryby", "1000");
                        vdm.insert(cmd);
                    }
                }
                else
                {
                    string empty = "";
                }
            }
        }
        catch (Exception ex)
        {

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
}