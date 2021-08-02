using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using ClosedXML.Excel;
using System.IO;

public partial class Branchwise_due_summary : System.Web.UI.Page
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
                FillSalesOffice();
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.sno = @BranchID)");
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
    DateTime Prevdate = DateTime.Now;

    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            lblmessage.Text = "";
            Session["RouteName"] = ddlSalesOffice.SelectedItem.Text;
            lblSalesOffice.Text = ddlSalesOffice.SelectedItem.Text;
            vdm = new VehicleDBMgr();
            DataTable GrandtotReport = new DataTable();

            DateTime start_date = DateTime.Now;
            string[] fromdatestrig = txtFromdate.Text.Split(' ');
            if (fromdatestrig.Length > 1)
            {
                if (fromdatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = fromdatestrig[0].Split('-');
                    string[] times = fromdatestrig[1].Split(':');
                    start_date = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
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
            Session["filename"] = "SalesOffice Due Report ->" + ddlSalesOffice.SelectedItem.Text;
            lbl_fromDate.Text = txtFromdate.Text;
            lbl_selttodate.Text = txtTodate.Text;
            DataTable Report = new DataTable();
          //  lblDate.Text = start_date.ToString("dd/MMM/yyyy");
            Session["filename"] = "BRANCHWISE DUE SUMMARY";
            string BranchID = ddlSalesOffice.SelectedValue;
            TimeSpan dateSpan = todate.Subtract(start_date);
            int NoOfdays = dateSpan.Days;
            NoOfdays = NoOfdays + 1;
            GrandtotReport = new DataTable();
            GrandtotReport.Columns.Add("Sno");
            GrandtotReport.Columns.Add("Date");
            GrandtotReport.Columns.Add("Oppening Balance");
            GrandtotReport.Columns.Add("Sale Value").DataType = typeof(Double);
            GrandtotReport.Columns.Add("Paid Amount").DataType = typeof(Double);
            GrandtotReport.Columns.Add("Bank Transfer").DataType = typeof(Double);
            GrandtotReport.Columns.Add("JV/Incentive").DataType = typeof(Double);
            GrandtotReport.Columns.Add("Closing Balance").DataType = typeof(Double);

            double final_totalsalesvalue = 0;
            double final_totalpaidamount = 0;
            double final_totalbankTransfer = 0;
            double final_totaljv = 0;
            for (int j = 0; j < NoOfdays; j++)
            {
                DateTime fromdate = start_date.AddDays(j);
                cmd = new MySqlCommand("SELECT  modifiedroutes.RouteName, modifiedroutes.sno as routeid,  modifiedroutesubtable.BranchID,       branchdata.BranchName  FROM    modifiedroutes        INNER JOIN    modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo        INNER JOIN    branchdata ON modifiedroutesubtable.BranchID = branchdata.sno  WHERE      (modifiedroutes.BranchID = @BranchID)         AND(modifiedroutesubtable.EDate IS NULL)         AND(modifiedroutesubtable.CDate <= @starttime) AND (branchdata.flag=@flag) OR (modifiedroutes.BranchID = @BranchID)AND(modifiedroutesubtable.EDate > @starttime)         AND(modifiedroutesubtable.CDate <= @starttime) AND (branchdata.flag=@flag) GROUP BY branchdata.BranchName  ORDER BY modifiedroutes.RouteName");
                cmd.Parameters.AddWithValue("@branchid", BranchID);
                cmd.Parameters.AddWithValue("@flag", "1");
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtble = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT * FROM agent_bal_trans WHERE inddate between @d1 and @d2");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtagenttrans = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT sum(AmountPaid) as AmountPaid,Branchid, PaymentType FROM collections WHERE PaidDate between @d1 and @d2 AND PaymentType <> 'Cash' group by Branchid, PaymentType");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                DataTable dtcollections = vdm.SelectQuery(cmd).Tables[0];

                DataTable dtrouteamount = new DataTable();
                DataTable dtsalescollection = new DataTable();
                Report = new DataTable();
                Report.Columns.Add("Sno");
                Report.Columns.Add("Date");
                Report.Columns.Add("Oppening Balance");
                Report.Columns.Add("Sale Value").DataType = typeof(Double);
                Report.Columns.Add("Paid Amount").DataType = typeof(Double);
                Report.Columns.Add("Bank Transfer").DataType = typeof(Double);
                Report.Columns.Add("JV/Incentive").DataType = typeof(Double);
                Report.Columns.Add("Closing Balance").DataType = typeof(Double);

               
                int Totalcount = 1;
                string RouteName = "";
                int i = 1;
                DataView view = new DataView(dtble);
                string routeid = "";
                string finalrouteid = "";
                DataTable distincttable = view.ToTable(true, "BranchName", "BranchID", "RouteName", "routeid");
                double ftotaloppbal = 0;
                double ftotalClosingbal = 0;
                double ftotalsalesvalue = 0;
                double ftotalpaidamount = 0;
                double ftotalbankTransfer = 0;
                double ftotaljv = 0;

                double grand_totaloppbal = 0;
                double grand_totalClosingbal = 0;
                double grand_totalsalesvalue = 0;
                double grand_totalpaidamount = 0;
                double grand_totalbankTransfer = 0;
                double grand_totaljv = 0;

                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    finalrouteid = branch["routeid"].ToString();
                    if (RouteName != branch["RouteName"].ToString())
                    {
                        if (Totalcount == 1)
                        {
                            //  newrow["Route Name"] = branch["RouteName"].ToString();
                            Totalcount++;
                        }
                        else
                        {
                            DataRow newvar = Report.NewRow();
                            // newvar["Agent Name"] = "Total";
                            int route = 0;
                            int.TryParse(routeid, out route);
                            foreach (DataRow dr in dtble.Select("RouteID='" + route + "'"))
                            {
                                if (branch["BranchName"].ToString() == dr["BranchName"].ToString())
                                {
                                    foreach (DataRow drcollections in dtcollections.Select("Branchid='" + dr["BranchID"].ToString() + "'"))
                                    {
                                        string PaymentType = drcollections["PaymentType"].ToString();
                                        if (PaymentType == "Bank Transfer")
                                        {
                                            double banktransfervalue = 0;
                                            double.TryParse(drcollections["AmountPaid"].ToString(), out banktransfervalue);
                                            newrow["Bank Transfer"] = banktransfervalue;
                                            ftotalbankTransfer += banktransfervalue;
                                            grand_totalbankTransfer += banktransfervalue;
                                        }
                                        if (PaymentType == "Journal Voucher" || PaymentType == "Incentive")
                                        {
                                            double jvvalue = 0;
                                            double.TryParse(drcollections["AmountPaid"].ToString(), out jvvalue);
                                            newrow["JV/Incentive"] = jvvalue;
                                            ftotaljv += jvvalue;
                                            grand_totaljv += jvvalue;
                                        }

                                    }
                                    foreach (DataRow drtrans in dtagenttrans.Select("agentid='" + dr["BranchID"].ToString() + "'"))
                                    {
                                        double salesvalue = 0;
                                        double.TryParse(drtrans["salesvalue"].ToString(), out salesvalue);
                                        ftotalsalesvalue += salesvalue;
                                        grand_totalsalesvalue += salesvalue;
                                        double paidamount = 0;
                                        double.TryParse(drtrans["paidamount"].ToString(), out paidamount);
                                        ftotalpaidamount += paidamount;
                                        grand_totalpaidamount += paidamount;
                                        double oppvalue = 0;
                                        double.TryParse(drtrans["opp_balance"].ToString(), out oppvalue);
                                        ftotaloppbal += oppvalue;
                                        grand_totaloppbal += oppvalue;
                                        double closvalue = 0;
                                        double.TryParse(drtrans["clo_balance"].ToString(), out closvalue);
                                        ftotalClosingbal += closvalue;
                                        grand_totalClosingbal += closvalue;
                                    }
                                }
                            }
                            newvar["Oppening Balance"] = Math.Round(ftotaloppbal, 2);
                            newvar["Sale Value"] = Math.Round(ftotalsalesvalue, 2);
                            newvar["Paid Amount"] = Math.Round(ftotalpaidamount, 2);
                            newvar["Bank Transfer"] = Math.Round(ftotalbankTransfer, 2);
                            newvar["JV/Incentive"] = Math.Round(ftotaljv, 2);
                            newvar["Closing Balance"] = Math.Round(ftotalClosingbal, 2);
                            double totCurdavg = 0;
                            totCurdavg = Math.Round(totCurdavg, 2);
                            Report.Rows.Add(newvar);
                            ftotaloppbal = 0;
                            ftotalsalesvalue = 0;
                            ftotalpaidamount = 0;
                            ftotalbankTransfer = 0;
                            ftotalClosingbal = 0;
                            ftotaljv = 0;
                            // newrow["Route Name"] = branch["RouteName"].ToString();
                            Totalcount++;
                            DataRow space = Report.NewRow();
                            // space["Agent Name"] = "";
                            Report.Rows.Add(space);
                            routeid = branch["routeid"].ToString();
                        }
                    }
                    else
                    {
                        // newrow["Route Name"] = "";
                        routeid = branch["routeid"].ToString();
                    }
                    RouteName = branch["RouteName"].ToString();
                    //newrow["Agent Code"] = branch["BranchID"].ToString();
                    //newrow["Agent Name"] = branch["BranchName"].ToString();
                    foreach (DataRow dr in dtble.Rows)
                    {
                        if (branch["BranchName"].ToString() == dr["BranchName"].ToString())
                        {
                            DataRow[] dragenttrans = dtagenttrans.Select("agentid='" + dr["BranchID"].ToString() + "'");
                            if (dragenttrans.Length <= 0)
                            {
                                cmd = new MySqlCommand("SELECT MAX(sno) as sno FROM agent_bal_trans WHERE agentid=@Branchid AND (inddate < @d1)");
                                cmd.Parameters.AddWithValue("@Branchid", dr["BranchID"].ToString());
                                cmd.Parameters.AddWithValue("@d1", fromdate.AddDays(-1));
                                DataTable dtPrev_trans = vdm.SelectQuery(cmd).Tables[0];
                                if (dtPrev_trans.Rows.Count > 0)
                                {
                                    string sno = dtPrev_trans.Rows[0]["sno"].ToString();
                                    if (sno == "")
                                    {
                                        double closingbalance = 0;
                                        newrow["Oppening Balance"] = closingbalance;
                                        newrow["Closing Balance"] = closingbalance;
                                        newrow["Sale Value"] = closingbalance;
                                        newrow["Paid Amount"] = closingbalance;
                                    }
                                    else
                                    {
                                        cmd = new MySqlCommand("SELECT agentid, opp_balance, inddate, salesvalue, clo_balance FROM agent_bal_trans WHERE sno=@sno");
                                        cmd.Parameters.AddWithValue("@sno", dtPrev_trans.Rows[0]["sno"].ToString());
                                        DataTable dtagent_value = vdm.SelectQuery(cmd).Tables[0];
                                        if (dtagent_value.Rows.Count > 0)
                                        {
                                            double closingbalance = 0;
                                            double.TryParse(dtagent_value.Rows[0]["clo_balance"].ToString(), out closingbalance);
                                            string inddate = dtagent_value.Rows[0]["inddate"].ToString();
                                            DateTime dtinddate = Convert.ToDateTime(inddate);
                                            if (dtinddate < fromdate)
                                            {
                                                closingbalance = Math.Round(closingbalance, 2);
                                                newrow["Oppening Balance"] = closingbalance;
                                                newrow["Closing Balance"] = closingbalance;
                                                ftotaloppbal += closingbalance;
                                                ftotalClosingbal += closingbalance;
                                                grand_totaloppbal += closingbalance;
                                                grand_totalClosingbal += closingbalance;
                                            }
                                            else
                                            {
                                                newrow["Oppening Balance"] = 0;
                                                newrow["Closing Balance"] = 0;
                                            }
                                            newrow["Sale Value"] = 0;
                                            newrow["Paid Amount"] = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    double closingbalance = 0;
                                    newrow["Oppening Balance"] = closingbalance;
                                    newrow["Closing Balance"] = closingbalance;
                                    newrow["Sale Value"] = closingbalance;
                                    newrow["Paid Amount"] = closingbalance;
                                }
                            }
                            double banktransfervalue = 0;
                            double jvvalue = 0;
                            foreach (DataRow drcollections in dtcollections.Select("Branchid='" + dr["BranchID"].ToString() + "'"))
                            {
                                //JV/Incentive
                                string PaymentType = drcollections["PaymentType"].ToString();
                                if (PaymentType == "Bank Transfer")
                                {
                                    double.TryParse(drcollections["AmountPaid"].ToString(), out banktransfervalue);
                                    newrow["Bank Transfer"] = banktransfervalue;
                                    ftotalbankTransfer += banktransfervalue;
                                    grand_totalbankTransfer += banktransfervalue;
                                }
                                if (PaymentType == "Journal Voucher" || PaymentType == "Incentive")
                                {

                                    double.TryParse(drcollections["AmountPaid"].ToString(), out jvvalue);
                                    newrow["JV/Incentive"] = jvvalue;
                                    ftotaljv += jvvalue;
                                    grand_totaljv += jvvalue;
                                }
                            }
                            foreach (DataRow drtrans in dtagenttrans.Select("agentid='" + dr["BranchID"].ToString() + "'"))
                            {
                                double oppvalue = 0;
                                double.TryParse(drtrans["opp_balance"].ToString(), out oppvalue);
                                ftotaloppbal += oppvalue;
                                grand_totaloppbal += oppvalue;
                                newrow["Oppening Balance"] = Math.Round(oppvalue, 2);
                                double salesvalue = 0;
                                double.TryParse(drtrans["salesvalue"].ToString(), out salesvalue);
                                newrow["Sale Value"] = salesvalue;
                                ftotalsalesvalue += salesvalue;
                                grand_totalsalesvalue += salesvalue;
                                double paidamount = 0;
                                double.TryParse(drtrans["paidamount"].ToString(), out paidamount);
                                if (paidamount > 0)
                                {
                                    paidamount = paidamount - banktransfervalue;
                                    paidamount = paidamount - jvvalue;
                                }
                                if (banktransfervalue == 0)
                                {
                                    newrow["Bank Transfer"] = banktransfervalue;
                                }
                                if (jvvalue == 0)
                                {
                                    newrow["JV/Incentive"] = jvvalue;
                                }
                                newrow["Paid Amount"] = paidamount;
                                ftotalpaidamount += paidamount;
                                grand_totalpaidamount += paidamount;
                                double closvalue = 0;
                                double.TryParse(drtrans["clo_balance"].ToString(), out closvalue);
                                newrow["Closing Balance"] = Math.Round(closvalue, 2);
                                ftotalClosingbal += closvalue;
                                grand_totalClosingbal += closvalue;
                            }
                        }
                    }
                    Report.Rows.Add(newrow);
                    routeid = branch["routeid"].ToString();
                    i++;
                }
                DataRow TotRow = Report.NewRow();
                // TotRow["Agent Name"] = "Total";
                TotRow["Oppening Balance"] = Math.Round(ftotaloppbal, 2);
                TotRow["Sale Value"] = Math.Round(ftotalsalesvalue, 2);
                TotRow["Paid Amount"] = Math.Round(ftotalpaidamount, 2);
                TotRow["Bank Transfer"] = Math.Round(ftotalbankTransfer, 2);
                TotRow["JV/Incentive"] = Math.Round(ftotaljv, 2);
                TotRow["Closing Balance"] = Math.Round(ftotalClosingbal, 2);
                Report.Rows.Add(TotRow);
                DataRow newbreak1 = Report.NewRow();
               // newbreak1["Agent Name"] = "";
                Report.Rows.Add(newbreak1);

                DataRow grandtotal = GrandtotReport.NewRow();
                grandtotal["Sno"] = j + 1;
                grandtotal["Date"] = start_date.AddDays(j).ToString("dd/MMM/yyyy");
                grandtotal["Oppening Balance"] = Math.Round(grand_totaloppbal, 2);
                grandtotal["Sale Value"] = Math.Round(grand_totalsalesvalue, 2);
                grandtotal["Paid Amount"] = Math.Round(grand_totalpaidamount, 2);
                grandtotal["Bank Transfer"] = Math.Round(grand_totalbankTransfer, 2);
                grandtotal["JV/Incentive"] = Math.Round(grand_totaljv, 2);
                grandtotal["Closing Balance"] = Math.Round(grand_totalClosingbal, 2);
                GrandtotReport.Rows.Add(grandtotal);
               // final_totaloppbal += grand_totaloppbal;
                final_totalsalesvalue += grand_totalsalesvalue;
                final_totalpaidamount += grand_totalpaidamount;
                final_totalbankTransfer += grand_totalbankTransfer;
                final_totaljv += grand_totaljv;
               // final_totalClosingbal += grand_totalClosingbal;
            }
            DataRow finalRow = GrandtotReport.NewRow();
            finalRow["Date"] = "Total";
           // finalRow["Oppening Balance"] = Math.Round(final_totaloppbal, 2);
            finalRow["Sale Value"] = Math.Round(final_totalsalesvalue, 2);
            finalRow["Paid Amount"] = Math.Round(final_totalpaidamount, 2);
            finalRow["Bank Transfer"] = Math.Round(final_totalbankTransfer, 2);
            finalRow["JV/Incentive"] = Math.Round(final_totaljv, 2);
            //finalRow["Closing Balance"] = Math.Round(final_totalClosingbal, 2);
            GrandtotReport.Rows.Add(finalRow);

            grdReports.DataSource = GrandtotReport;
            grdReports.DataBind();
            Session["xportdata"] = GrandtotReport;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            lblmessage.Text = ex.Message;
        }
    }
}