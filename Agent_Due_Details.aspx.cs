﻿using MySql.Data.MySqlClient;
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
public partial class Agent_Due_Details : System.Web.UI.Page
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
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) and (branchdata.flag<>0) ");
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
                PBranch.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) and (branchdata.flag<>0) OR (branchdata.sno = @BranchID) and (branchdata.flag<>0)");
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
            lblDate.Text = fromdate.ToString("dd/MMM/yyyy");
            Session["filename"] = "AGENT WISE DUE REPORT";
            string BranchID = ddlSalesOffice.SelectedValue;
            if (BranchID == "7")
            {
                cmd = new MySqlCommand("SELECT  modifiedroutes.RouteName, modifiedroutes.sno as routeid,  modifiedroutesubtable.BranchID,       branchdata.BranchName  FROM    modifiedroutes        INNER JOIN    modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo        INNER JOIN    branchdata ON modifiedroutesubtable.BranchID = branchdata.sno  WHERE      (modifiedroutes.BranchID = @BranchID)AND(modifiedroutesubtable.EDate IS NULL)AND(modifiedroutesubtable.CDate <= @starttime) AND (branchdata.flag=@flag) and modifiedroutes.sno not in('25')  OR (modifiedroutes.BranchID = @BranchID)AND(modifiedroutesubtable.EDate > @starttime)AND(modifiedroutesubtable.CDate <= @starttime) AND (branchdata.flag=@flag) and modifiedroutes.sno not in('42','28','41','33','32','31','34','25','36','5') GROUP BY branchdata.BranchName  ORDER BY modifiedroutes.RouteName");
                cmd.Parameters.AddWithValue("@branchid", BranchID);
                cmd.Parameters.AddWithValue("@flag", "1");
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            }
            else
            {
                cmd = new MySqlCommand("SELECT  modifiedroutes.RouteName, modifiedroutes.sno as routeid,  modifiedroutesubtable.BranchID,       branchdata.BranchName  FROM    modifiedroutes        INNER JOIN    modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo        INNER JOIN    branchdata ON modifiedroutesubtable.BranchID = branchdata.sno  WHERE      (modifiedroutes.BranchID = @BranchID)AND(modifiedroutesubtable.EDate IS NULL)AND(modifiedroutesubtable.CDate <= @starttime) AND (branchdata.flag=@flag)  OR (modifiedroutes.BranchID = @BranchID)AND(modifiedroutesubtable.EDate > @starttime)AND(modifiedroutesubtable.CDate <= @starttime) AND (branchdata.flag=@flag) GROUP BY branchdata.BranchName  ORDER BY modifiedroutes.RouteName");
                cmd.Parameters.AddWithValue("@branchid", BranchID);
                cmd.Parameters.AddWithValue("@flag", "1");
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            }
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT * FROM agent_bal_trans WHERE inddate between @d1 and @d2");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtagenttrans = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT sum(AmountPaid) as AmountPaid,Branchid, PaymentType FROM collections WHERE PaidDate between @d1 and @d2  group by Branchid, PaymentType");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            DataTable dtcollections = vdm.SelectQuery(cmd).Tables[0];

            DataTable dtrouteamount = new DataTable();
            DataTable dtsalescollection = new DataTable();
            Report = new DataTable();
            Report.Columns.Add("Sno");
            Report.Columns.Add("Route Name");
            Report.Columns.Add("Agent Code");
            Report.Columns.Add("Agent Name");
            Report.Columns.Add("Oppening Balance");
            Report.Columns.Add("Sale Value").DataType = typeof(Double);
            Report.Columns.Add("Paid Amount").DataType = typeof(Double);
            Report.Columns.Add("PhonePay").DataType = typeof(Double);
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
            double ftotalPhonePay = 0;

            double grand_totaloppbal = 0;
            double grand_totalClosingbal = 0;
            double grand_totalsalesvalue = 0;
            double grand_totalpaidamount = 0;
            double grand_totalbankTransfer = 0;
            double grand_totaljv = 0;
            double grand_totalPhonepay = 0;
            
            foreach (DataRow branch in distincttable.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["SNo"] = i;
                finalrouteid = branch["routeid"].ToString();
                if (RouteName != branch["RouteName"].ToString())
                {
                    if (Totalcount == 1)
                    {
                        newrow["Route Name"] = branch["RouteName"].ToString();
                        Totalcount++;
                    }
                    else
                    {
                        DataRow newvar = Report.NewRow();
                        newvar["Agent Name"] = "Total";
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
                                    if (PaymentType == "PhonePay")
                                    {
                                        double Phonepayvalue = 0;
                                        double.TryParse(drcollections["AmountPaid"].ToString(), out Phonepayvalue);
                                        newrow["PhonePay"] = Phonepayvalue;
                                        ftotalPhonePay += Phonepayvalue;
                                        grand_totalPhonepay += Phonepayvalue;
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
                        newvar["PhonePay"] = Math.Round(ftotalPhonePay, 2);
                        newvar["JV/Incentive"] = Math.Round(ftotaljv, 2);
                        newvar["Closing Balance"] = Math.Round(ftotalClosingbal, 2);
                        double totCurdavg = 0;
                        totCurdavg = Math.Round(totCurdavg, 2);
                        Report.Rows.Add(newvar);
                        ftotaloppbal = 0;
                        ftotalsalesvalue = 0;
                        ftotalpaidamount = 0;
                        ftotalbankTransfer = 0;
                        ftotalPhonePay = 0;

                        ftotalClosingbal = 0;
                        ftotaljv = 0;
                        newrow["Route Name"] = branch["RouteName"].ToString();
                        Totalcount++;
                        DataRow space = Report.NewRow();
                        space["Agent Name"] = "";
                        Report.Rows.Add(space);
                        routeid = branch["routeid"].ToString();
                    }
                }
                else
                {
                    newrow["Route Name"] = "";
                    routeid = branch["routeid"].ToString();
                }
                RouteName = branch["RouteName"].ToString();
                newrow["Agent Code"] = branch["BranchID"].ToString();
                newrow["Agent Name"] = branch["BranchName"].ToString();
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
                        double PhonePayValue = 0;
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
                            if (PaymentType == "PhonePay")
                            {
                                double.TryParse(drcollections["AmountPaid"].ToString(), out PhonePayValue);
                                newrow["PhonePay"] = PhonePayValue;
                                ftotalPhonePay += PhonePayValue;
                                grand_totalPhonepay += PhonePayValue;
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
                                double otheramount = banktransfervalue + jvvalue + PhonePayValue;
                                paidamount = paidamount - otheramount;
                                foreach (DataRow drcollections1 in dtcollections.Select("Branchid='" + dr["BranchID"].ToString() + "'"))
                                {
                                    string PaymentType = drcollections1["PaymentType"].ToString();
                                    if(PaymentType == "Cash") 
                                    {
                                        double cashamt = 0;
                                        double.TryParse(drcollections1["AmountPaid"].ToString(), out cashamt);
                                       double tota = cashamt + paidamount;
                                        paidamount = tota - paidamount;

                                    }
                                }
                            }
                            if (banktransfervalue == 0)
                            {
                                newrow["Bank Transfer"] = banktransfervalue;
                            }
                            if (PhonePayValue == 0)
                            {
                                newrow["PhonePay"] = PhonePayValue;
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
            TotRow["Agent Name"] = "Total";
            TotRow["Oppening Balance"] = Math.Round(ftotaloppbal, 2);
            TotRow["Sale Value"] = Math.Round(ftotalsalesvalue, 2);
            TotRow["Paid Amount"] = Math.Round(ftotalpaidamount, 2);
            TotRow["Bank Transfer"] = Math.Round(ftotalbankTransfer, 2);
            TotRow["PhonePay"] = Math.Round(ftotalPhonePay, 2);
            TotRow["JV/Incentive"] = Math.Round(ftotaljv, 2);
            TotRow["Closing Balance"] = Math.Round(ftotalClosingbal, 2);
            Report.Rows.Add(TotRow);
            DataRow newbreak1 = Report.NewRow();
            newbreak1["Agent Name"] = "";
            Report.Rows.Add(newbreak1);

            DataRow grandtotal = Report.NewRow();
            grandtotal["Agent Name"] = "Grand Total";
            grandtotal["Oppening Balance"] = Math.Round(grand_totaloppbal, 2);
            grandtotal["Sale Value"] = Math.Round(grand_totalsalesvalue, 2);
            grandtotal["Paid Amount"] = Math.Round(grand_totalpaidamount, 2);
            grandtotal["Bank Transfer"] = Math.Round(grand_totalbankTransfer, 2);
            grandtotal["PhonePay"] = Math.Round(grand_totalPhonepay, 2);
            grandtotal["JV/Incentive"] = Math.Round(grand_totaljv, 2);
            grandtotal["Closing Balance"] = Math.Round(grand_totalClosingbal,2);
            Report.Rows.Add(grandtotal);
            grdReports.DataSource = Report;
            grdReports.DataBind();
            Session["xportdata"] = Report;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            lblmessage.Text = ex.Message;
        }
    }
}