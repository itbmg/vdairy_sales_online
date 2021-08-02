using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class MilkBuyerReport : System.Web.UI.Page
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
    void FillSalesOffice()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Group")
            {
                PPlant.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) ");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlPlant.DataSource = dtRoutedata;
                ddlPlant.DataTextField = "BranchName";
                ddlPlant.DataValueField = "sno";
                ddlPlant.DataBind();
                ddlPlant.Items.Insert(0, new ListItem("Select Plant", "0"));
            }
            else if (Session["salestype"].ToString() == "Plant")
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.sno = @BranchID) AND (branchdata.SalesType IS NOT NULL)");
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
    protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        PBranch.Visible = true;
        cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
        cmd.Parameters.AddWithValue("@SuperBranch", ddlPlant.SelectedValue);
        cmd.Parameters.AddWithValue("@SalesType", "21");
        cmd.Parameters.AddWithValue("@SalesType1", "26");
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlSalesOffice.DataSource = dtRoutedata;
        ddlSalesOffice.DataTextField = "BranchName";
        ddlSalesOffice.DataValueField = "sno";
        ddlSalesOffice.DataBind();

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

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        pnlHide.Visible = true;
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
        Report.Columns.Add("SNo");
        //Report.Columns.Add("Route Name");
        Report.Columns.Add("SR Name");
        Report.Columns.Add("Agent Name");
        Report.Columns.Add("Due");
        Report.Columns.Add("Cheques Pending");
        if (ddltype.SelectedValue == "With incentive")
        {
            Report.Columns.Add("Incentive");
        }
        Report.Columns.Add("Net Due");
        Report.Columns.Add("Due Since");
        Report.Columns.Add("Crates Bal");
        Report.Columns.Add("Issued");
        Report.Columns.Add("Excess");
        Report.Columns.Add("Cans");
        Report.Columns.Add("Since Date");
        Report.Columns.Add("Remarks");
        lblDispatchName.Text = ddlSalesOffice.SelectedItem.Text;
        DateTime dtfrm = fromdate;
        lblDate.Text = dtfrm.ToString("dd/MM/yyyy");
        fromdate = fromdate.AddDays(-1);

        cmd = new MySqlCommand("SELECT SUM(collections.AmountPaid) AS amount, branchmappingtable.SuperBranch, branchdata.sno, branchdata.BranchName FROM collections INNER JOIN branchdata ON collections.Branchid = branchdata.sno INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (collections.PaymentType = 'Cheque') AND (collections.CheckStatus = @CheckStatus) AND (collections.tripId IS NULL) AND (branchmappingtable.SuperBranch = @SuperBranch) GROUP BY branchdata.sno, branchdata.BranchName");
        cmd.Parameters.AddWithValue("@CheckStatus", 'P');
        cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
        DataTable dtcheque = vdm.SelectQuery(cmd).Tables[0];

        cmd = new MySqlCommand("SELECT branchaccounts.BranchId, branchaccounts.Amount, branchaccounts.FineAmount, branchaccounts.Dtripid, branchaccounts.Ctripid, branchaccounts.SaleValue,branchmappingtable.SuperBranch FROM branchaccounts INNER JOIN branchmappingtable ON branchaccounts.BranchId = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @BranchID)");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        DataTable dtagentbalance = vdm.SelectQuery(cmd).Tables[0];

        cmd = new MySqlCommand("SELECT due_trans_inventory.closing AS crates, due_trans_inventory.clo10 + due_trans_inventory.clo20 + due_trans_inventory.clo40 AS cans,due_trans_inventory.isuued, branchmappingtable.SubBranch FROM due_trans_inventory INNER JOIN branchmappingtable ON due_trans_inventory.agentid = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @BranchID) AND (due_trans_inventory.doe BETWEEN @d1 AND @d2)");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(1));
        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate).AddDays(1));
        DataTable dtinv = vdm.SelectQuery(cmd).Tables[0];

        cmd=new MySqlCommand ("SELECT branchmappingtable.SuperBranch, inventory_monitor.Qty, inventory_monitor.Inv_Sno, inventory_monitor.BranchId FROM inventory_monitor INNER JOIN branchmappingtable ON inventory_monitor.BranchId = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @BranchID)");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        DataTable dtinvebalance = vdm.SelectQuery(cmd).Tables[0];
        int Totalcount = 1;

        cmd=new MySqlCommand ("SELECT sno,salestype FROM salestypemanagement WHERE (status = 1) ORDER BY rank");
        DataTable dtsalesType = vdm.SelectQuery(cmd).Tables[0];
        DataTable dtincentives = new DataTable();
        if (ddltype.SelectedValue == "With incentive")
        {
            cmd=new MySqlCommand ("SELECT incentivetransactions.BranchId, incentivetransactions.TotalDiscount FROM incentivetransactions INNER JOIN branchmappingtable ON incentivetransactions.BranchId = branchmappingtable.SubBranch INNER JOIN branchdata ON branchmappingtable.SuperBranch = branchdata.sno WHERE (incentivetransactions.EntryDate BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @BranchID) OR (incentivetransactions.EntryDate BETWEEN @d1 AND @d2) AND (branchdata.SalesOfficeID = @SOID)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-15));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate).AddDays(10));
            dtincentives = vdm.SelectQuery(cmd).Tables[0];
        
        }
        
        double totdebitamount = 0;
        double totchequeamount = 0;
        double totincentiveamount = 0;
        double totnetdueamount = 0;
        double totcrates = 0;
        double totcans = 0;

        double totroutedebitamount = 0;
        double totroutechequeamount = 0;
        double totrouteincentiveamount = 0;
        double totroutenetdueamount = 0;
        double totroutecrates = 0;
        double totroutecans = 0;

        double totgrandroutedebitamount = 0;
        double totgrandroutechequeamount = 0;
        double totgrandrouteincentiveamount = 0;
        double totgrandroutenetdueamount = 0;
        double totgrandroutecrates = 0;
        double totgrandroutecans = 0;
        DataRow newrow111 = Report.NewRow();
        newrow111["Agent Name"] = "AGENTS";
        Report.Rows.Add(newrow111);
        foreach (DataRow dr in dtsalesType.Rows)
        {
            string salestype = dr["salestype"].ToString();
            if (salestype == "DISCONTINUED AGENTS")
            {
                cmd = new MySqlCommand("SELECT branchroutes.srname, branchroutes.RouteName,branchdata.BranchName,branchdata.sno as Branchid,branchdata.SalesRepresentative, tempduetrasactions.ClosingBalance, salestypemanagement.salestype FROM branchmappingtable INNER JOIN tempduetrasactions ON branchmappingtable.SubBranch = tempduetrasactions.AgentId INNER JOIN branchdata ON tempduetrasactions.AgentId = branchdata.sno INNER JOIN salestypemanagement ON branchdata.SalesType = salestypemanagement.sno INNER JOIN branchroutes ON tempduetrasactions.RouteId = branchroutes.Sno WHERE (branchmappingtable.SuperBranch = @BranchID)  and (salestypemanagement.sno=@SalesType) AND (tempduetrasactions.IndentDate BETWEEN @d1 AND @d2) GROUP BY branchdata.BranchName, salestypemanagement.salestype, branchroutes.RouteName order by salestypemanagement.salestype, branchroutes.RouteName");
            }
            else
            {
                cmd = new MySqlCommand("SELECT  branchroutes.srname,branchroutes.RouteName,branchdata.BranchName,branchdata.sno as Branchid,branchdata.SalesRepresentative, tempduetrasactions.ClosingBalance, salestypemanagement.salestype FROM branchmappingtable INNER JOIN tempduetrasactions ON branchmappingtable.SubBranch = tempduetrasactions.AgentId INNER JOIN branchdata ON tempduetrasactions.AgentId = branchdata.sno INNER JOIN salestypemanagement ON branchdata.SalesType = salestypemanagement.sno INNER JOIN branchroutes ON tempduetrasactions.RouteId = branchroutes.Sno WHERE (branchmappingtable.SuperBranch = @BranchID) AND (branchdata.flag = 1) and (salestypemanagement.sno=@SalesType) AND (tempduetrasactions.IndentDate BETWEEN @d1 AND @d2) GROUP BY branchdata.BranchName, salestypemanagement.salestype, branchroutes.RouteName order by salestypemanagement.salestype, branchroutes.RouteName");
            }
            cmd.Parameters.AddWithValue("@SalesType", dr["sno"].ToString());
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            DataView viewLeaks = new DataView(dtble);
            DataTable distinctroutes = viewLeaks.ToTable(true, "salestype", "RouteName");
            DataTable distinctagents = viewLeaks.ToTable(true, "RouteName", "Branchid");
            if (Totalcount != 1)
            {
                DataRow space1 = Report.NewRow();
                space1["Agent Name"] = "Total";
                space1["Due"] = totdebitamount;
                //space1["Credit"] = totroutecreditamount;
                space1["Cheques Pending"] = totchequeamount;
                if (ddltype.SelectedValue == "With incentive")
                {
                    space1["Incentive"] = totincentiveamount;

                }
                space1["Net Due"] = totnetdueamount;
                space1["Crates Bal"] = totcrates;
                space1["Cans"] = totcans;
                if (totdebitamount == 0.0 && totchequeamount == 0.0 && totnetdueamount == 0.0 && totcrates == 0.0 && totcans == 0.0)
                {
                }
                else
                {
                    Report.Rows.Add(space1);
                }
                totroutedebitamount += totdebitamount;
                totroutechequeamount += totchequeamount;
                totrouteincentiveamount += totincentiveamount;
                totroutenetdueamount += totnetdueamount;
                totroutecrates += totcrates;
                totroutecans += totcans;
                totdebitamount = 0;
                totchequeamount = 0;
                totincentiveamount = 0;
                totnetdueamount = 0;
                totcrates = 0;
                totcans = 0;
                DataRow space2 = Report.NewRow();
                space2["Agent Name"] = "Sub Total";
                space2["Due"] = totroutedebitamount;
                //space1["Credit"] = totroutecreditamount;
                space2["Cheques Pending"] = totroutechequeamount;
                space2["Net Due"] = totroutenetdueamount;
                space2["Crates Bal"] = totroutecrates;
                space2["Cans"] = totroutecans;
                if (totroutedebitamount == 0.0 && totroutechequeamount == 0.0 && totroutenetdueamount == 0.0 && totroutecrates == 0.0 && totroutecans == 0.0)
                {
                }
                else
                {
                    Report.Rows.Add(space2);
                }
                totgrandroutedebitamount += totroutedebitamount;
                totgrandroutechequeamount += totroutechequeamount;
                totgrandrouteincentiveamount += totrouteincentiveamount;
                totgrandroutenetdueamount += totroutenetdueamount;
                totgrandroutecrates += totroutecrates;
                totgrandroutecans += totroutecans;
                totroutedebitamount = 0;
                totroutechequeamount = 0;
                totrouteincentiveamount = 0;
                totroutenetdueamount = 0;
                totroutecrates = 0;
                totroutecans = 0;
                DataRow empty = Report.NewRow();
                empty["Agent Name"] = "";
                DataRow[] drsalestype = dtble.Select("salestype='" + dr["salestype"].ToString() + "'");
                if (drsalestype.Length > 0)
                {
                    Report.Rows.Add(empty);
                }
                DataRow newrow11 = Report.NewRow();
                newrow11["Agent Name"] = dr["salestype"].ToString();
                if (drsalestype.Length > 0)
                {
                    Report.Rows.Add(newrow11);
                }

            }
            //DataRow newrow1 = Report.NewRow();
            //newrow1["Route Name"] = dr["salestype"].ToString();
            //Report.Rows.Add(newrow1);
            string RouteName = "";
            int i = 1;
            //label:
            foreach (DataRow dragents in distinctroutes.Select("salestype='" + dr["salestype"].ToString() + "'"))
            {
              
                foreach (DataRow drbranch in dtble.Select("salestype='" + dr["salestype"].ToString() + "' and RouteName='" + dragents["RouteName"].ToString() + "'"))
                {
                    //DataRow newrow12 = Report.NewRow();
                    //newrow12["Agent Name"] = drbranch["RouteName"].ToString();
                    //Report.Rows.Add(newrow12);
                    DataRow newrow = Report.NewRow();
                    if (RouteName != drbranch["RouteName"].ToString())
                    {
                        if (Totalcount == 1)
                        {
                            newrow["Sno"] = i++.ToString();
                            //newrow["Route Name"] = drbranch["RouteName"].ToString();
                            Totalcount++;
                            DataRow newrow13 = Report.NewRow();
                            newrow13["Agent Name"] = dragents["RouteName"].ToString() + "-" + drbranch["srname"].ToString();
                            Report.Rows.Add(newrow13);
                        }
                        else
                        {
                            if (RouteName != drbranch["RouteName"].ToString())
                            {
                                if (RouteName == "")
                                {
                                }
                                else
                                {
                                    DataRow space1 = Report.NewRow();
                                    space1["Agent Name"] = "Total";
                                    space1["Due"] = totdebitamount;
                                    //space1["Credit"] = totcreditamount;
                                    //Branchid

                                    space1["Cheques Pending"] = totchequeamount;
                                    if (ddltype.SelectedValue == "With incentive")
                                    {
                                        space1["Incentive"] = totincentiveamount;
                                    }
                                    space1["Net Due"] = totnetdueamount;
                                    space1["Crates Bal"] = totcrates;
                                    space1["Cans"] = totcans;
                                    if (totdebitamount == 0.0 && totchequeamount == 0.0 && totnetdueamount == 0.0 && totcrates == 0.0 && totcans == 0.0)
                                    {
                                    }
                                    else
                                    {
                                        Report.Rows.Add(space1);
                                    }
                                    totroutedebitamount += totdebitamount;
                                    totroutechequeamount += totchequeamount;
                                    totrouteincentiveamount += totincentiveamount;
                                    totroutenetdueamount += totnetdueamount;
                                    totroutecrates += totcrates;
                                    totroutecans += totcans;
                                    totdebitamount = 0;
                                    totchequeamount = 0;
                                    totincentiveamount = 0;
                                    totnetdueamount = 0;
                                    totcrates = 0;
                                    totcans = 0;
                                   
                                }
                            }
                            newrow["Sno"] = i++.ToString();
                            //newrow["Route Name"] = drbranch["RouteName"].ToString();
                            Totalcount++;
                            if (RouteName != drbranch["RouteName"].ToString())
                            {
                                if (RouteName == "")
                                {
                                }
                                else
                                {
                                    DataRow space = Report.NewRow();
                                    space["Agent Name"] = "";
                                    Report.Rows.Add(space);
                                    DataRow newrow13 = Report.NewRow();
                                    newrow13["Agent Name"] = dragents["RouteName"].ToString() + "-" + drbranch["srname"].ToString();
                                    Report.Rows.Add(newrow13);
                                }
                            }
                        }
                    }
                    else
                    {
                        //newrow["Route Name"] = "";
                    }
                    RouteName = drbranch["RouteName"].ToString();
                    newrow["Agent Name"] = drbranch["BranchName"].ToString();
                    newrow["SR Name"] = drbranch["SalesRepresentative"].ToString();
                    double chequeamount = 0;
                    if (dtcheque.Rows.Count > 0)
                    {
                        DataRow[] roe = dtcheque.Select("sno=" + drbranch["Branchid"].ToString());
                        foreach (DataRow drc in roe)
                        {
                            double.TryParse(drc.ItemArray[0].ToString(), out chequeamount);
                            newrow["Cheques Pending"] = chequeamount.ToString();
                            totchequeamount += chequeamount;
                        }
                    }
                    double incentiveamount = 0;
                    if (ddltype.SelectedValue == "With incentive")
                    {
                        DataRow[] drincentive = dtincentives.Select("BranchId=" + drbranch["BranchId"].ToString());
                        foreach (DataRow drc in drincentive)
                        {
                            double.TryParse(drc.ItemArray[1].ToString(), out incentiveamount);
                            newrow["Incentive"] = incentiveamount.ToString();
                            totincentiveamount += incentiveamount;
                        }
                    }
                    double amount = 0;
                    double netdue = 0;
                    double.TryParse(drbranch["ClosingBalance"].ToString(), out amount);
                    //if (amount > 0)
                    //{
                    newrow["Due"] = amount;
                    totdebitamount += amount;
                    double removeamount = chequeamount + incentiveamount;
                    netdue = amount - removeamount;
                   
                    newrow["Net Due"] = netdue;
                    totnetdueamount += netdue;
                    DataRow[] drcrates = dtinv.Select("SubBranch=" + drbranch["Branchid"].ToString());
                    int crates = 0;
                    foreach (DataRow drc in drcrates)
                    {
                        int.TryParse(drc.ItemArray[0].ToString(), out crates);
                        totcrates += crates;
                        newrow["Crates Bal"] = crates;
                        int issuedcreates = 0;
                        int.TryParse(drc.ItemArray[2].ToString(), out issuedcreates);
                        newrow["Issued"] = issuedcreates;
                        int excess = 0;
                        excess = crates - issuedcreates;
                        newrow["Excess"] = excess;
                        int cans = 0;
                        int.TryParse(drc.ItemArray[1].ToString(), out cans);
                        newrow["Cans"] = cans;
                        totcans += cans;
                    }
                    DataRow[] drinv = dtinvebalance.Select("BranchId=" + drbranch["Branchid"].ToString());
                    foreach (DataRow drc in drinv)
                    {
                        double dueinventory = 0;
                        double.TryParse(drc.ItemArray[1].ToString(), out dueinventory);
                        if (salestype == "DISCONTINUED AGENTS")
                        {
                            if (drc.ItemArray[2].ToString() == "1")
                            {
                                newrow["Crates Bal"] = dueinventory;
                            }
                            if (drc.ItemArray[2].ToString() == "2")
                            {
                                newrow["Cans"] = dueinventory;
                            }
                        }
                        if (dueinventory > 0)
                        {
                            if (salestype == "DISCONTINUED AGENTS")
                            {
                                cmd = new MySqlCommand("SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty, CBFromTran, CBToTran, DeliveryTime, CollectionTime, Remarks, Modified_EmpId FROM invtransactions12 WHERE (ToTran = @BranchID) ORDER BY DOE DESC");
                            }
                            else
                            {
                                cmd = new MySqlCommand("SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty, CBFromTran, CBToTran, DeliveryTime, CollectionTime, Remarks, Modified_EmpId FROM invtransactions12 WHERE (ToTran = @BranchID) and (DOE between @d1 and @d2) ORDER BY DOE DESC");
                                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddMonths(-4));
                                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                            }
                            cmd.Parameters.AddWithValue("@BranchID", drbranch["Branchid"].ToString());
                            DataTable dtinvamount = vdm.SelectQuery(cmd).Tables[0];
                            if (dtinvamount.Rows.Count > 0)
                            {

                                double diffinv = 0;
                                double getinv = 0;
                                foreach (DataRow drsales in dtinvamount.Rows)
                                {

                                    if (diffinv >= 0)
                                    {
                                        double salesamount = 0;
                                        double.TryParse(drsales["Qty"].ToString(), out salesamount);
                                        getinv += salesamount;
                                        diffinv = dueinventory - getinv;
                                        if (diffinv < 0)
                                        {
                                            string PlanTime = drsales["DOE"].ToString();
                                            DateTime dtPlantime = Convert.ToDateTime(PlanTime);
                                            string time = dtPlantime.ToString("dd/MM/yyyy");
                                            newrow["Since Date"] = time;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    DataRow[] dragentamount = dtagentbalance.Select("BranchId=" + drbranch["Branchid"].ToString());
                    foreach (DataRow drc in dragentamount)
                    {
                        double dueamount = 0;
                        double.TryParse(drc.ItemArray[1].ToString(), out dueamount);
                        if (dueamount > 0)
                        {
                            if (salestype == "DISCONTINUED AGENTS")
                            {
                                cmd = new MySqlCommand("SELECT SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS amount, indents.I_date FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo WHERE (indents.Branch_id = @BranchID) GROUP BY indents.Branch_id, indents.I_date ORDER BY indents.I_date DESC");
                            }
                            else
                            {
                                cmd = new MySqlCommand("SELECT SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS amount, indents.I_date FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo WHERE (indents.Branch_id = @BranchID)  and (indents.I_date between @d1 and @d2) GROUP BY indents.Branch_id, indents.I_date ORDER BY indents.I_date DESC");
                                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddMonths(-4));
                                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                            }
                            cmd.Parameters.AddWithValue("@BranchID", drbranch["Branchid"].ToString());
                            DataTable dtsalesamount = vdm.SelectQuery(cmd).Tables[0];
                            if (dtsalesamount.Rows.Count > 0)
                            {

                                double diffamount = 0;
                                double getamount = 0;
                                foreach (DataRow drsales in dtsalesamount.Rows)
                                {
                                    if (diffamount >= 0)
                                    {
                                        double salesamount = 0;
                                        double.TryParse(drsales["amount"].ToString(), out salesamount);
                                        getamount += salesamount;
                                        diffamount = dueamount - getamount;
                                        if (diffamount < 0)
                                        {
                                            string PlanTime = drsales["I_date"].ToString();
                                            DateTime dtPlantime = Convert.ToDateTime(PlanTime);
                                            string time = dtPlantime.ToString("dd/MM/yyyy");
                                            newrow["Due Since"] = time;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Report.Rows.Add(newrow);
                }
            }
        }
        DataRow space3= Report.NewRow();
        space3["Agent Name"] = "Total";
        space3["Due"] = totdebitamount;
        //space2["Credit"] = totcreditamount;
        space3["Cheques Pending"] = totchequeamount;
        if (ddltype.SelectedValue == "With incentive")
        {
            space3["Incentive"] = totincentiveamount;

        }
        space3["Net Due"] = totnetdueamount;
        space3["Crates Bal"] = totcrates;
        space3["Cans"] = totcans;
        if (totdebitamount == 0.0 && totchequeamount == 0.0 && totnetdueamount == 0.0 && totcrates == 0.0 && totcans == 0.0)
        {
        }
        else
        {
            Report.Rows.Add(space3);
        }
        totroutedebitamount += totdebitamount;
        totroutechequeamount += totchequeamount;
        totrouteincentiveamount += totincentiveamount;
        totroutenetdueamount += totnetdueamount;
        totroutecrates += totcrates;
        totroutecans += totcans;
        DataRow space4 = Report.NewRow();
        space4["Agent Name"] = "Sub Total";
        space4["Due"] = totroutedebitamount;
        //space1["Credit"] = totroutecreditamount;
        space4["Cheques Pending"] = totroutechequeamount;
        if (ddltype.SelectedValue == "With incentive")
        {
            space4["Incentive"] = totrouteincentiveamount;
        }
        space4["Net Due"] = totroutenetdueamount;
        space4["Crates Bal"] = totroutecrates;
        space4["Cans"] = totroutecans;
        if (totroutedebitamount == 0.0 && totroutechequeamount == 0.0 && totroutenetdueamount == 0.0 && totroutecrates == 0.0 && totroutecans == 0.0)
        {
        }
        else
        {
            Report.Rows.Add(space4);
        }
        totgrandroutedebitamount += totroutedebitamount;
        totgrandroutechequeamount += totroutechequeamount;
        totgrandrouteincentiveamount += totrouteincentiveamount;
        totgrandroutenetdueamount += totroutenetdueamount;
        totgrandroutecrates += totroutecrates;
        totgrandroutecans += totroutecans;

        DataRow space5 = Report.NewRow();
        space5["Agent Name"] = "Grand Total";
        space5["Due"] = totgrandroutedebitamount;
        //space1["Credit"] = totroutecreditamount;
        space5["Cheques Pending"] = totgrandroutechequeamount;
        if (ddltype.SelectedValue == "With incentive")
        {
            space5["Incentive"] = totgrandrouteincentiveamount;
        }
        space5["Net Due"] = totgrandroutenetdueamount;
        space5["Crates Bal"] = totgrandroutecrates;
        space5["Cans"] = totgrandroutecans;
        Report.Rows.Add(space5);
        pnlHide.Visible = true;
        grdReports.DataSource = Report;
        grdReports.DataBind();
        Session["xportdata"] = Report;
    }

    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ddltype.SelectedValue == "With incentive")
            {
                e.Row.Cells[12].Width = new Unit("850px");
            }
            else
            {
                e.Row.Cells[11].Width = new Unit("850px");

            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[1].Text == "AGENTS")
            {
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Larger;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[1].Text == "CATERING AGENTS")
            {
                e.Row.BackColor = System.Drawing.Color.LightCoral;
                e.Row.Font.Size = FontUnit.Larger;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[1].Text == "DISCONTINUED AGENTS")
            {
                e.Row.BackColor = System.Drawing.Color.Salmon;
                e.Row.Font.Size = FontUnit.Larger;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[1].Text == "DUE AGENTS")
            {
                e.Row.BackColor = System.Drawing.Color.DeepSkyBlue;
                e.Row.Font.Size = FontUnit.Larger;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[1].Text == "INSTITUTIONAL")
            {
                e.Row.BackColor = System.Drawing.Color.LightSteelBlue;
                e.Row.Font.Size = FontUnit.Larger;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[1].Text == "Comapass Group")
            {
                e.Row.BackColor = System.Drawing.Color.YellowGreen;
                e.Row.Font.Size = FontUnit.Larger;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[1].Text == "Fresh and Honest cafe")
            {
                e.Row.BackColor = System.Drawing.Color.LightBlue;
                e.Row.Font.Size = FontUnit.Larger;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[1].Text == "Parlour")
            {
                e.Row.BackColor = System.Drawing.Color.SlateBlue;
                e.Row.Font.Size = FontUnit.Larger;
                e.Row.Font.Bold = true;
            }
            if (e.Row.Cells[1].Text == "Total")
            {
                e.Row.Font.Size = FontUnit.Medium;
            }
            if (e.Row.Cells[1].Text == "Sub Total")
            {
                e.Row.Font.Size = FontUnit.Large;
            }
            if (e.Row.Cells[1].Text == "Grand Total")
            {
                e.Row.BackColor = System.Drawing.Color.GreenYellow;
                e.Row.Font.Size = FontUnit.XLarge;
                e.Row.Font.Bold = true;
            }
        }
    }
}