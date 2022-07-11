using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
public partial class BranchWiseIndent : System.Web.UI.Page
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
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            }
        }
    }
    protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        PBranch.Visible = true;
        if (ddlType.SelectedValue == "Branch Wise")
        {
            PBranch.Visible = false;

        }
        else
        {
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) and (branchdata.flag<>0) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) and (branchdata.flag<>0) ");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                cmd.Parameters.AddWithValue("@SalesType", "21");
                cmd.Parameters.AddWithValue("@SalesType1", "26");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
                PBranch.Visible = true;
            }
            else
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) and (branchdata.flag<>0) OR (branchdata.sno = @BranchID) AND (branchdata.SalesType IS NOT NULL) and (branchdata.flag<>0)");
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
                PBranch.Visible = true;
            }
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
    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ddlType.SelectedValue == "Decrese")
            {
                e.Row.Cells[11].Width = new Unit("600px");
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
                e.Row.BackColor = System.Drawing.Color.Aquamarine;
                e.Row.Font.Size = FontUnit.Large;
                e.Row.Font.Bold = true;
            }
        }
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
    string routeid = "";
    string routeitype = "";
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            Report = new DataTable();
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
            string[] dateFromstrig = txtFromdate.Text.Split(' ');
            if (dateFromstrig.Length > 1)
            {
                if (dateFromstrig[0].Split('-').Length > 0)
                {
                    string[] dates = dateFromstrig[0].Split('-');
                    string[] times = dateFromstrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            string[] datetostrig = txtTodate.Text.Split(' ');
            if (datetostrig.Length > 1)
            {
                if (datetostrig[0].Split('-').Length > 0)
                {
                    string[] dates = datetostrig[0].Split('-');
                    string[] times = datetostrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            string frmdate = fromdate.ToString("dd/MM/yyyy");
            string tdate = todate.ToString("dd/MM/yyyy");
            lbl_fromDate.Text = frmdate;
            lbl_selttodate.Text = tdate;
            if (ddlremarkstype.SelectedValue == "Without Remarks")
            {
                if (ddlType.SelectedValue == "Branch Wise")
                {
                    Report = new DataTable();
                    Report.Columns.Add("SNo");
                    Report.Columns.Add("Sales Office Name");
                    Report.Columns.Add("Route Name");
                    Report.Columns.Add(frmdate);
                    Report.Columns.Add(tdate);
                    Report.Columns.Add("Increase");
                    Report.Columns.Add("Increase %");
                    Report.Columns.Add("Decrease");
                    Report.Columns.Add("Decrease %");
                    cmd = new MySqlCommand("SELECT branchmappingtable.SubBranch, branchmappingtable.SuperBranch, branchdata.BranchName,branchroutes.RouteName, branchroutes.Sno AS Routesno FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN branchroutes ON branchdata.sno = branchroutes.BranchID WHERE (branchmappingtable.SuperBranch = @BranchID) and (branchroutes.flag=1) order by branchdata.sno");
                    // cmd = new MySqlCommand("SELECT branchmappingtable.SubBranch, branchmappingtable.SuperBranch, branchdata.BranchName FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno WHERE (branchmappingtable.SuperBranch = @BranchID)");
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                    DataTable dtbranch = vdm.SelectQuery(cmd).Tables[0];
                    double fromqty = 0;
                    double toqty = 0;
                    double increseqty = 0;
                    double decreseeqty = 0;

                    double totfromqty = 0;
                    double tottoqty = 0;
                    double totincreseqty = 0;
                    double totdecreseeqty = 0;
                    if (dtbranch.Rows.Count > 0)
                    {
                        int i = 1;
                        int Totalcount = 1;
                        string BranchName = "";
                        foreach (DataRow drbranch in dtbranch.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            if (BranchName != drbranch["BranchName"].ToString())
                            {
                                if (Totalcount == 1)
                                {
                                    newrow["Sno"] = i++.ToString();
                                    newrow["Sales Office Name"] = drbranch["BranchName"].ToString();
                                    Totalcount++;
                                }
                                else
                                {

                                    DataRow space1 = Report.NewRow();
                                    space1["Route Name"] = "Total";
                                    space1[frmdate] = fromqty;
                                    space1[tdate] = toqty;
                                    space1["Increase"] = increseqty;
                                    space1["Decrease"] = decreseeqty;
                                    Report.Rows.Add(space1);
                                    newrow["Sno"] = i++.ToString();
                                    newrow["Sales Office Name"] = drbranch["BranchName"].ToString();
                                    Totalcount++;
                                    DataRow space = Report.NewRow();
                                    space["Route Name"] = "";
                                    Report.Rows.Add(space);
                                    totfromqty += fromqty;
                                    tottoqty += toqty;
                                    totincreseqty += increseqty;
                                    totdecreseeqty += decreseeqty;
                                    fromqty = 0;
                                    toqty = 0;
                                    increseqty = 0;
                                    decreseeqty = 0;
                                }
                            }
                            else
                            {
                                newrow["Sales Office Name"] = "";
                            }
                            BranchName = drbranch["BranchName"].ToString();
                            cmd = new MySqlCommand("SELECT branchroutesubtable.BranchID, ROUND(SUM(indents_subtable.unitQty), 2) AS indentqty, branchroutes.Sno AS RouteSno, branchroutes.RouteName FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo LEFT OUTER JOIN indents_subtable INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM indents WHERE (I_date BETWEEN @d1 AND @d2)) indnts ON indents_subtable.IndentNo = indnts.IndentNo ON  branchroutesubtable.BranchID = indnts.Branch_id WHERE (branchroutes.BranchID = @BranchID) AND (branchroutes.flag = 1) GROUP BY branchroutes.Sno, branchroutes.RouteName ORDER BY RouteSno");
                            cmd.Parameters.AddWithValue("@branchID", drbranch["SubBranch"].ToString());
                            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                            DataTable dtindentprev = vdm.SelectQuery(cmd).Tables[0];

                            cmd = new MySqlCommand("SELECT branchroutesubtable.BranchID, ROUND(SUM(indents_subtable.unitQty), 2) AS indentqty, branchroutes.Sno AS RouteSno, branchroutes.RouteName FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo LEFT OUTER JOIN indents_subtable INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM indents WHERE (I_date BETWEEN @d1 AND @d2)) indnts ON indents_subtable.IndentNo = indnts.IndentNo ON  branchroutesubtable.BranchID = indnts.Branch_id WHERE (branchroutes.BranchID = @BranchID) AND (branchroutes.flag = 1) GROUP BY branchroutes.Sno, branchroutes.RouteName ORDER BY RouteSno");
                            cmd.Parameters.AddWithValue("@branchID", drbranch["SubBranch"].ToString());
                            cmd.Parameters.AddWithValue("@d1", GetLowDate(todate));
                            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                            DataTable dtindentpresent = vdm.SelectQuery(cmd).Tables[0];

                            double PrevIndent = 0;
                            double PresentIndent = 0;
                            double DifferenceIndent = 0;
                            if (dtindentprev.Rows.Count > 0)
                            {
                                foreach (DataRow drprev in dtindentprev.Select("RouteSno='" + drbranch["Routesno"].ToString() + "'"))
                                {
                                    double.TryParse(drprev["indentqty"].ToString(), out PrevIndent);
                                    fromqty += PrevIndent;
                                }
                                foreach (DataRow drpresnt in dtindentpresent.Select("RouteSno='" + drbranch["Routesno"].ToString() + "'"))
                                {
                                    double.TryParse(drpresnt["indentqty"].ToString(), out PresentIndent);
                                    toqty += PresentIndent;
                                }

                                DifferenceIndent = PrevIndent - PresentIndent;

                                newrow["Route Name"] = drbranch["RouteName"].ToString();
                                newrow[frmdate] = PrevIndent;
                                newrow[tdate] = PresentIndent;
                                if (DifferenceIndent < 0)
                                {
                                    DifferenceIndent = Math.Abs(DifferenceIndent);
                                    DifferenceIndent = Math.Round(DifferenceIndent, 2);
                                    newrow["Increase"] = DifferenceIndent;
                                    increseqty += DifferenceIndent;
                                    double per = 0;
                                    per = (DifferenceIndent / PrevIndent) * 100;
                                    per = Math.Round(per, 2);
                                    newrow["Increase %"] = per;

                                }
                                else
                                {
                                    DifferenceIndent = Math.Abs(DifferenceIndent);
                                    DifferenceIndent = Math.Round(DifferenceIndent, 2);
                                    newrow["Decrease"] = DifferenceIndent;
                                    decreseeqty += DifferenceIndent;
                                    double per = 0;
                                    per = (DifferenceIndent / PrevIndent) * 100;
                                    per = Math.Round(per, 2);
                                    newrow["Decrease %"] = per;
                                }
                            }
                            //newrow["Route Name"] = drbranch["RouteName"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        DataRow space5 = Report.NewRow();
                        space5["Route Name"] = "Total";
                        space5[frmdate] = fromqty;
                        space5[tdate] = toqty;
                        space5["Increase"] = increseqty;
                        space5["Decrease"] = decreseeqty;
                        Report.Rows.Add(space5);
                        DataRow space7 = Report.NewRow();
                        space7["Route Name"] = "";
                        Report.Rows.Add(space7);
                        DataRow space3 = Report.NewRow();
                        space3["Route Name"] = "Grand Total";
                        space3[frmdate] = totfromqty;
                        space3[tdate] = tottoqty;
                        space3["Increase"] = totincreseqty;
                        space3["Decrease"] = totdecreseeqty;
                        Report.Rows.Add(space3);
                        grdReports.DataSource = Report;
                        grdReports.DataBind();
                        Session["xportdata"] = Report;
                        pnlHide.Visible = true;
                    }
                }
                if (ddlType.SelectedValue == "Agent Wise")
                {
                    Report = new DataTable();
                    Report.Columns.Add("SNo");
                    //Report.Columns.Add("Route Name");
                    Report.Columns.Add("Agent Name");
                    Report.Columns.Add("SR Name");
                    Report.Columns.Add(frmdate);
                    Report.Columns.Add(tdate);
                    Report.Columns.Add("Due");
                    Report.Columns.Add("Crates");
                    if (ddlindenttype.SelectedValue == "Increase" || ddlindenttype.SelectedValue == "All")
                    {
                        Report.Columns.Add("Increase");
                        Report.Columns.Add("Increase %");
                        Report.Columns.Add("Increase Remarks");
                    }
                    if (ddlindenttype.SelectedValue == "Decrese" || ddlindenttype.SelectedValue == "All")
                    {
                        Report.Columns.Add("Decrease");
                        Report.Columns.Add("Decrease %");
                        Report.Columns.Add("Decrease Remarks");
                    }

                    double fromqty = 0;
                    double toqty = 0;
                    double increseqty = 0;
                    double decreseeqty = 0;
                    cmd = new MySqlCommand("SELECT branchaccounts.BranchId, branchaccounts.Amount, branchaccounts.FineAmount, branchaccounts.Dtripid, branchaccounts.Ctripid, branchaccounts.SaleValue,branchmappingtable.SuperBranch FROM branchaccounts INNER JOIN branchmappingtable ON branchaccounts.BranchId = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @BranchID)");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    DataTable dtagentbalance = vdm.SelectQuery(cmd).Tables[0];
                    cmd = new MySqlCommand("SELECT branchmappingtable.SuperBranch, inventory_monitor.Qty, inventory_monitor.Inv_Sno, inventory_monitor.BranchId FROM inventory_monitor INNER JOIN branchmappingtable ON inventory_monitor.BranchId = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @BranchID)");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    DataTable dtinvebalance = vdm.SelectQuery(cmd).Tables[0];
                    cmd = new MySqlCommand("SELECT sno,salestype FROM salestypemanagement WHERE (status = 1) ORDER BY rank");
                    DataTable dtsalesType = vdm.SelectQuery(cmd).Tables[0];
                    double totfromqty = 0;
                    double tottoqty = 0;
                    double totamount = 0;
                    double totcrates = 0;
                    double grandtotamount = 0;
                    double grandtotcrates = 0;
                    double totincreseqty = 0;
                    double totdecreseeqty = 0;

                    double grandtotfromqty = 0;
                    double grandtottoqty = 0;
                    if (dtsalesType.Rows.Count > 0)
                    {
                        int i = 1;
                        int Totalcount = 1;
                        string Routename = "";
                        DataRow newrow111 = Report.NewRow();
                        newrow111["Agent Name"] = "AGENTS";
                        Report.Rows.Add(newrow111);
                        foreach (DataRow dr in dtsalesType.Rows)
                        {
                            string salestype = dr["salestype"].ToString();
                            if (salestype == "DISCONTINUED AGENTS")
                            {
                                cmd = new MySqlCommand("SELECT branchroutes.RouteName,branchdata.SalesType,branchdata.SalesRepresentative, branchroutes.Sno AS Routesno,branchdata.sno as bsno, branchdata.BranchName FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (branchroutes.BranchID = @BranchID) ORDER BY Routesno");
                            }
                            else
                            {
                                cmd = new MySqlCommand("SELECT branchroutes.RouteName,branchdata.SalesType,branchdata.SalesRepresentative, branchroutes.Sno AS Routesno,branchdata.sno as bsno, branchdata.BranchName FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (branchdata.flag = 1) AND (branchroutes.BranchID = @BranchID) ORDER BY Routesno");
                            }
                            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                            DataTable dtbranch = vdm.SelectQuery(cmd).Tables[0];
                            DataView viewLeaks = new DataView(dtbranch);
                            DataTable distinctroutes = viewLeaks.ToTable(true, "salestype", "RouteName");
                            DataTable distinctagents = viewLeaks.ToTable(true, "RouteName", "bsno");
                            if (Totalcount != 1)
                            {
                                DataRow space1 = Report.NewRow();
                                space1["Agent Name"] = "Total";
                                if (ddlindenttype.SelectedValue == "Increase" || ddlindenttype.SelectedValue == "Decrese")
                                {
                                    space1[frmdate] = totfromqty;
                                    space1[tdate] = tottoqty;
                                    grandtotfromqty += totfromqty;
                                    grandtottoqty += tottoqty;
                                    totfromqty = 0;
                                    tottoqty = 0;
                                }
                                else
                                {
                                    space1[frmdate] = totfromqty;
                                    space1[tdate] = tottoqty;
                                    grandtotfromqty += totfromqty;
                                    grandtottoqty += tottoqty;
                                    totfromqty = 0;
                                    tottoqty = 0;
                                }
                                space1["Due"] = totamount;
                                space1["Crates"] = totcrates;
                                grandtotamount += totamount;
                                grandtotcrates += totcrates;
                                totamount = 0;
                                totcrates = 0;
                                if (ddlindenttype.SelectedValue == "All")
                                {
                                    space1["Increase"] = increseqty;
                                    space1["Decrease"] = decreseeqty;
                                }
                                if (ddlindenttype.SelectedValue == "Increase")
                                {
                                    space1["Increase"] = increseqty;
                                }
                                if (ddlindenttype.SelectedValue == "Decrese")
                                {

                                    space1["Decrease"] = decreseeqty;
                                }
                                increseqty = 0;
                                decreseeqty = 0;
                                Report.Rows.Add(space1);
                                DataRow empty = Report.NewRow();
                                empty["Agent Name"] = "";
                                DataRow[] drsalestype = dtbranch.Select("salestype='" + dr["sno"].ToString() + "'");
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
                            foreach (DataRow dragents in distinctroutes.Select("salestype='" + dr["sno"].ToString() + "'"))
                            {

                                foreach (DataRow drbranch in dtbranch.Select("salestype='" + dr["sno"].ToString() + "' and RouteName='" + dragents["RouteName"].ToString() + "'"))
                                {
                                    //foreach (DataRow drbranch in dtbranch.Rows)
                                    //{
                                    DataRow newrow = Report.NewRow();
                                    if (Routename != drbranch["RouteName"].ToString())
                                    {
                                        if (Totalcount == 1)
                                        {
                                            newrow["Sno"] = i++.ToString();
                                            //newrow["Route Name"] = drbranch["RouteName"].ToString();
                                            Totalcount++;
                                            DataRow newrow13 = Report.NewRow();
                                            newrow13["Agent Name"] = dragents["RouteName"].ToString();
                                            Report.Rows.Add(newrow13);
                                        }
                                        else
                                        {
                                            if (Routename != drbranch["RouteName"].ToString())
                                            {
                                                if (Routename == "")
                                                {
                                                }
                                                else
                                                {
                                                    DataRow space1 = Report.NewRow();
                                                    space1["Agent Name"] = "Total";
                                                    if (ddlindenttype.SelectedValue == "Increase" || ddlindenttype.SelectedValue == "Decrese")
                                                    {
                                                        space1[frmdate] = totfromqty;
                                                        space1[tdate] = tottoqty;
                                                        grandtotfromqty += totfromqty;
                                                        grandtottoqty += tottoqty;
                                                        totfromqty = 0;
                                                        tottoqty = 0;
                                                    }
                                                    else
                                                    {
                                                        space1[frmdate] = totfromqty;
                                                        space1[tdate] = tottoqty;
                                                        grandtotfromqty += totfromqty;
                                                        grandtottoqty += tottoqty;
                                                        totfromqty = 0;
                                                        tottoqty = 0;
                                                    }
                                                    space1["Due"] = totamount;
                                                    space1["Crates"] = totcrates;
                                                    grandtotamount += totamount;
                                                    grandtotcrates += totcrates;
                                                    totamount = 0;
                                                    totcrates = 0;
                                                    if (ddlindenttype.SelectedValue == "All")
                                                    {
                                                        space1["Increase"] = increseqty;
                                                        space1["Decrease"] = decreseeqty;
                                                    }
                                                    if (ddlindenttype.SelectedValue == "Increase")
                                                    {
                                                        space1["Increase"] = increseqty;
                                                    }
                                                    if (ddlindenttype.SelectedValue == "Decrese")
                                                    {

                                                        space1["Decrease"] = decreseeqty;
                                                    }
                                                    Report.Rows.Add(space1);
                                                    //newrow["Sno"] = i++.ToString();
                                                    //newrow["Route Name"] = drbranch["RouteName"].ToString();
                                                    //Totalcount++;
                                                    //DataRow space = Report.NewRow();
                                                    //space["Agent Name"] = "";
                                                    //Report.Rows.Add(space);
                                                    //totfromqty += fromqty;
                                                    //tottoqty += toqty;
                                                    //totincreseqty += increseqty;
                                                    //totdecreseeqty += decreseeqty;

                                                    fromqty = 0;
                                                    toqty = 0;
                                                    increseqty = 0;
                                                    decreseeqty = 0;
                                                }
                                            }
                                            newrow["Sno"] = i++.ToString();
                                            //newrow["Route Name"] = drbranch["RouteName"].ToString();
                                            Totalcount++;
                                            if (Routename != drbranch["RouteName"].ToString())
                                            {
                                                if (Routename == "")
                                                {
                                                }
                                                else
                                                {
                                                    DataRow space = Report.NewRow();
                                                    space["Agent Name"] = "";
                                                    Report.Rows.Add(space);
                                                    DataRow newrow13 = Report.NewRow();
                                                    newrow13["Agent Name"] = dragents["RouteName"].ToString();
                                                    Report.Rows.Add(newrow13);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //newrow["Route Name"] = "";
                                    }
                                    Routename = drbranch["RouteName"].ToString();
                                    cmd = new MySqlCommand("SELECT branchroutesubtable.BranchID, ROUND(SUM(indents_subtable.unitQty), 2) AS indentqty, branchroutes.Sno AS RouteSno, branchroutes.RouteName,  branchdata.BranchName, branchdata.sno FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno LEFT OUTER JOIN  indents_subtable INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM indents WHERE (I_date BETWEEN @d1 AND @d2)) indnts ON indents_subtable.IndentNo = indnts.IndentNo ON  branchroutesubtable.BranchID = indnts.Branch_id WHERE  (branchdata.flag = 1) AND (branchroutes.Sno = @RouteID) GROUP BY branchdata.BranchName, branchdata.sno ORDER BY branchdata.sno");
                                    cmd.Parameters.AddWithValue("@RouteID", drbranch["Routesno"].ToString());
                                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                                    DataTable dtindentprev = vdm.SelectQuery(cmd).Tables[0];

                                    cmd = new MySqlCommand("SELECT branchroutesubtable.BranchID, ROUND(SUM(indents_subtable.unitQty), 2) AS indentqty, branchroutes.Sno AS RouteSno, branchroutes.RouteName,  branchdata.BranchName, branchdata.sno FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno LEFT OUTER JOIN  indents_subtable INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM indents WHERE (I_date BETWEEN @d1 AND @d2)) indnts ON indents_subtable.IndentNo = indnts.IndentNo ON  branchroutesubtable.BranchID = indnts.Branch_id WHERE  (branchdata.flag = 1) AND (branchroutes.Sno = @RouteID) GROUP BY branchdata.BranchName, branchdata.sno ORDER BY branchdata.sno");
                                    cmd.Parameters.AddWithValue("@RouteID", drbranch["Routesno"].ToString());
                                    cmd.Parameters.AddWithValue("@d1", GetLowDate(todate));
                                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                                    DataTable dtindentpresent = vdm.SelectQuery(cmd).Tables[0];

                                    double PrevIndent = 0;
                                    double PresentIndent = 0;
                                    double DifferenceIndent = 0;
                                    double DifInd = 0;
                                    if (dtindentprev.Rows.Count > 0)
                                    {
                                        foreach (DataRow drprev in dtindentprev.Select("BranchID='" + drbranch["bsno"].ToString() + "'"))
                                        {
                                            double.TryParse(drprev["indentqty"].ToString(), out PrevIndent);
                                            if (ddlindenttype.SelectedValue == "All")
                                            {
                                                totfromqty += PrevIndent;
                                            }
                                        }
                                        foreach (DataRow drpresnt in dtindentpresent.Select("BranchID='" + drbranch["bsno"].ToString() + "'"))
                                        {
                                            double.TryParse(drpresnt["indentqty"].ToString(), out PresentIndent);
                                            if (ddlindenttype.SelectedValue == "All")
                                            {
                                                tottoqty += PresentIndent;
                                            }
                                        }

                                        DifferenceIndent = PresentIndent - PrevIndent;
                                        DifInd = PresentIndent - PrevIndent;

                                        newrow["Agent Name"] = drbranch["BranchName"].ToString();
                                        newrow["SR Name"] = drbranch["SalesRepresentative"].ToString();
                                        DataRow[] dragentamount = dtagentbalance.Select("BranchId=" + drbranch["bsno"].ToString());
                                        foreach (DataRow drc in dragentamount)
                                        {
                                            double dueamount = 0;
                                            double.TryParse(drc.ItemArray[1].ToString(), out dueamount);
                                            newrow["Due"] = dueamount;
                                            totamount += dueamount;
                                        }
                                        DataRow[] drinv = dtinvebalance.Select("BranchId=" + drbranch["bsno"].ToString());
                                        foreach (DataRow drc in drinv)
                                        {
                                            string crates = drc.ItemArray[2].ToString();
                                            if (crates == "1")
                                            {
                                                double dueinventory = 0;
                                                double.TryParse(drc.ItemArray[1].ToString(), out dueinventory);
                                                newrow["Crates"] = dueinventory;
                                                totcrates += dueinventory;
                                            }
                                        }
                                        if (ddlindenttype.SelectedValue == "All")
                                        {
                                            if (DifferenceIndent >= 0)
                                            {
                                                DifferenceIndent = Math.Abs(DifferenceIndent);
                                                DifferenceIndent = Math.Round(DifferenceIndent, 2);
                                                newrow["Increase"] = DifferenceIndent;
                                                increseqty += DifferenceIndent;
                                                totincreseqty += DifferenceIndent;

                                                double per = 0;
                                                per = (DifferenceIndent / PrevIndent) * 100;
                                                per = Math.Round(per, 2);
                                                newrow["Increase %"] = per;
                                                newrow[frmdate] = PrevIndent;
                                                newrow[tdate] = PresentIndent;
                                            }
                                            else
                                            {
                                                if (PrevIndent == PresentIndent)
                                                {
                                                }
                                                else
                                                {
                                                    DifferenceIndent = Math.Abs(DifferenceIndent);
                                                    DifferenceIndent = Math.Round(DifferenceIndent, 2);
                                                    newrow["Decrease"] = DifferenceIndent;
                                                    decreseeqty += DifferenceIndent;
                                                    totdecreseeqty += DifferenceIndent;
                                                    double per = 0;
                                                    per = (DifferenceIndent / PrevIndent) * 100;
                                                    per = Math.Round(per, 2);
                                                    newrow["Decrease %"] = per;
                                                    newrow[frmdate] = PrevIndent;
                                                    newrow[tdate] = PresentIndent;
                                                }
                                            }
                                        }
                                        if (DifferenceIndent > 0)
                                        {
                                            if (ddlindenttype.SelectedValue == "Increase")
                                            {
                                                DifferenceIndent = Math.Abs(DifferenceIndent);
                                                DifferenceIndent = Math.Round(DifferenceIndent, 2);
                                                newrow["Increase"] = DifferenceIndent;
                                                increseqty += DifferenceIndent;
                                                totincreseqty += DifferenceIndent;

                                                double per = 0;
                                                per = (DifferenceIndent / PrevIndent) * 100;
                                                per = Math.Round(per, 2);
                                                newrow["Increase %"] = per;
                                                newrow[frmdate] = PrevIndent;
                                                newrow[tdate] = PresentIndent;
                                                fromqty += PrevIndent;
                                                if (ddlindenttype.SelectedValue == "Increase")
                                                {
                                                    totfromqty += PrevIndent;
                                                    tottoqty += PresentIndent;
                                                }
                                                else
                                                {

                                                }
                                            }

                                        }
                                        else
                                        {
                                            if (ddlindenttype.SelectedValue == "Decrese")
                                            {
                                                if (PrevIndent == PresentIndent)
                                                {
                                                }
                                                else
                                                {
                                                    DifferenceIndent = Math.Abs(DifferenceIndent);
                                                    DifferenceIndent = Math.Round(DifferenceIndent, 2);
                                                    newrow["Decrease"] = DifferenceIndent;
                                                    decreseeqty += DifferenceIndent;
                                                    totdecreseeqty += DifferenceIndent;
                                                    double per = 0;
                                                    per = (DifferenceIndent / PrevIndent) * 100;
                                                    per = Math.Round(per, 2);
                                                    newrow["Decrease %"] = per;
                                                    newrow[frmdate] = PrevIndent;
                                                    newrow[tdate] = PresentIndent;
                                                    toqty += PrevIndent;
                                                    if (ddlindenttype.SelectedValue == "Decrese")
                                                    {
                                                        totfromqty += PrevIndent;
                                                        tottoqty += PresentIndent;
                                                    }
                                                    else
                                                    {
                                                        tottoqty += PresentIndent;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //newrow["Route Name"] = drbranch["RouteName"].ToString();
                                    Report.Rows.Add(newrow);
                                }
                            }
                        }
                        DataTable dtReport = new DataTable();
                        dtReport.Columns.Add("SNo");
                        //dtReport.Columns.Add("Route Name");
                        dtReport.Columns.Add("Agent Name");
                        dtReport.Columns.Add("SR Name");
                        dtReport.Columns.Add(frmdate);
                        dtReport.Columns.Add(tdate);
                        dtReport.Columns.Add("Due");
                        dtReport.Columns.Add("Crates");
                        if (ddlindenttype.SelectedValue == "All")
                        {
                            dtReport.Columns.Add("Increase");
                            dtReport.Columns.Add("Increase %");
                            dtReport.Columns.Add("Decrease");
                            dtReport.Columns.Add("Decrease %");
                            dtReport.Columns.Add("Decrease Remarks");
                        }
                        if (ddlindenttype.SelectedValue == "Increase")
                        {
                            dtReport.Columns.Add("Increase");
                            dtReport.Columns.Add("Increase %");
                        }
                        if (ddlindenttype.SelectedValue == "Decrese")
                        {
                            dtReport.Columns.Add("Decrease");
                            dtReport.Columns.Add("Decrease %");
                            dtReport.Columns.Add("Decrease Remarks");
                        }
                        foreach (DataRow drbranch in Report.Rows)
                        {
                            DataRow space1 = dtReport.NewRow();
                            //string route = drbranch["Route Name"].ToString();
                            string Agent = drbranch["Agent Name"].ToString();
                            string From = drbranch[frmdate].ToString();
                            string To = drbranch[tdate].ToString();
                            if (From == "")
                            {
                                From = "0";
                            }
                            if (To == "")
                            {
                                To = "0";
                            }
                            if (Agent == "Total" && From == "0" && To == "0")
                            {
                            }
                            else
                            {
                                space1["SNo"] = drbranch["SNo"].ToString();
                                //space1["Route Name"] = drbranch["Route Name"].ToString();
                                space1["Agent Name"] = drbranch["Agent Name"].ToString();
                                space1["SR Name"] = drbranch["SR Name"].ToString();
                                space1[frmdate] = drbranch[frmdate].ToString();
                                space1[tdate] = drbranch[tdate].ToString();
                                space1["Due"] = drbranch["Due"].ToString();
                                space1["Crates"] = drbranch["Crates"].ToString();
                                if (ddlindenttype.SelectedValue == "All")
                                {
                                    space1["Increase"] = drbranch["Increase"].ToString();
                                    space1["Increase %"] = drbranch["Increase %"].ToString();
                                    space1["Decrease"] = drbranch["Decrease"].ToString();
                                    space1["Decrease %"] = drbranch["Decrease %"].ToString();
                                }
                                if (ddlindenttype.SelectedValue == "Increase")
                                {
                                    space1["Increase"] = drbranch["Increase"].ToString();
                                    space1["Increase %"] = drbranch["Increase %"].ToString();
                                }
                                if (ddlindenttype.SelectedValue == "Decrese")
                                {
                                    space1["Decrease"] = drbranch["Decrease"].ToString();
                                    space1["Decrease %"] = drbranch["Decrease %"].ToString();
                                }
                                dtReport.Rows.Add(space1);
                            }
                        }
                        DataRow space5 = dtReport.NewRow();
                        space5["Agent Name"] = "Total";
                        if (ddlindenttype.SelectedValue == "Increase" || ddlindenttype.SelectedValue == "Decrese")
                        {
                            space5[frmdate] = fromqty;
                            space5[tdate] = toqty;
                            grandtotfromqty += fromqty;
                            grandtottoqty += toqty;
                        }
                        if (ddlindenttype.SelectedValue == "All")
                        {
                            space5[frmdate] = totfromqty;
                            space5[tdate] = tottoqty;
                            grandtotfromqty += totfromqty;
                            grandtottoqty += tottoqty;
                        }
                        space5["Due"] = totamount;
                        space5["Crates"] = totcrates;
                        grandtotamount += totamount;
                        grandtotcrates += totcrates;
                        totamount = 0;
                        totcrates = 0;
                        if (ddlindenttype.SelectedValue == "All")
                        {

                            space5["Increase"] = increseqty;
                            space5["Decrease"] = decreseeqty;
                        }
                        if (ddlindenttype.SelectedValue == "Increase")
                        {
                            space5["Increase"] = increseqty;
                        }
                        if (ddlindenttype.SelectedValue == "Decrese")
                        {
                            space5["Decrease"] = decreseeqty;
                        }
                        dtReport.Rows.Add(space5);
                        DataRow space6 = dtReport.NewRow();
                        space6["Agent Name"] = "";
                        dtReport.Rows.Add(space6);
                        DataRow space3 = dtReport.NewRow();
                        space3["Agent Name"] = "Grand Total";
                        space3[frmdate] = grandtotfromqty;
                        space3[tdate] = grandtottoqty;
                        space3["Due"] = grandtotamount;
                        space3["Crates"] = grandtotcrates;
                        if (ddlindenttype.SelectedValue == "All")
                        {
                            space3["Increase"] = totincreseqty;
                            space3["Decrease"] = totdecreseeqty;
                        }
                        if (ddlindenttype.SelectedValue == "Increase")
                        {
                            space3["Increase"] = totincreseqty;
                        }
                        if (ddlindenttype.SelectedValue == "Decrese")
                        {
                            space3["Decrease"] = totdecreseeqty;
                        }
                        dtReport.Rows.Add(space3);
                        grdReports.DataSource = dtReport;
                        grdReports.DataBind();
                        Session["xportdata"] = dtReport;
                        pnlHide.Visible = true;
                        string test = "";
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "indentincreasedecresereport(" + test + ");", true);
                    }
                }
            }
            if (ddlremarkstype.SelectedValue == "With Remarks")
            {
                Report = new DataTable();
                Report.Columns.Add("SNo");
                //Report.Columns.Add("Route Name");
                Report.Columns.Add("Agent Name");
                Report.Columns.Add("SR Name");
                Report.Columns.Add(frmdate);
                Report.Columns.Add(tdate);
                Report.Columns.Add("Due");
                Report.Columns.Add("Crates");
                Report.Columns.Add("Increase");
                Report.Columns.Add("Increase %");
                Report.Columns.Add("Decrease");
                Report.Columns.Add("Decrease %");
                Report.Columns.Add("Remarks");
                cmd = new MySqlCommand("SELECT indent_monitoring.sno, indent_monitoring.RouteId AS Routesno,branchdata.SalesRepresentative, branchdata.sno as bsno, indent_monitoring.Indent_Date1, indent_monitoring.Indent_Date2, indent_monitoring.Indent_Increase, indent_monitoring.Indent_Decrease, indent_monitoring.Entry_Date, indent_monitoring.Reason, indent_monitoring.Remarks, indent_monitoring.Entered_by, indent_monitoring.Yesterday_Qty, indent_monitoring.Today_Qty, branchdata.BranchName FROM indent_monitoring INNER JOIN branchroutes ON indent_monitoring.RouteId = branchroutes.Sno INNER JOIN branchdata ON indent_monitoring.Agent_ID = branchdata.sno WHERE (branchroutes.BranchID = @BranchID) AND (indent_monitoring.Indent_Date1 BETWEEN @d1 AND @d2)");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(todate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                DataTable dtIndentmonitoring = vdm.SelectQuery(cmd).Tables[0];

                double fromqty = 0;
                double toqty = 0;
                double increseqty = 0;
                double decreseeqty = 0;
                cmd = new MySqlCommand("SELECT sno,salestype FROM salestypemanagement WHERE (status = 1) ORDER BY rank");
                DataTable dtsalesType = vdm.SelectQuery(cmd).Tables[0];
                double totfromqty = 0;
                double tottoqty = 0;
                double totamount = 0;
                double totcrates = 0;
                double grandtotamount = 0;
                double grandtotcrates = 0;
                double totincreseqty = 0;
                double totdecreseeqty = 0;

                double grandtotfromqty = 0;
                double grandtottoqty = 0;
                if (dtsalesType.Rows.Count > 0)
                {
                    int i = 1;
                    int Totalcount = 1;
                    string Routename = "";
                    DataRow newrow111 = Report.NewRow();
                    newrow111["Agent Name"] = "AGENTS";
                    Report.Rows.Add(newrow111);
                    foreach (DataRow dr in dtsalesType.Rows)
                    {
                        string salestype = dr["salestype"].ToString();
                        if (salestype == "DISCONTINUED AGENTS")
                        {
                            cmd = new MySqlCommand("SELECT branchroutes.RouteName,branchdata.SalesType,branchdata.SalesRepresentative, branchroutes.Sno AS Routesno,branchdata.sno as bsno, branchdata.BranchName FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (branchroutes.BranchID = @BranchID) ORDER BY Routesno");
                            cmd.Parameters.AddWithValue("@BranchID", 1);
                        }
                        else
                        {
                            cmd = new MySqlCommand("SELECT branchroutes.RouteName,branchdata.SalesType,branchdata.SalesRepresentative, branchroutes.Sno AS Routesno,branchdata.sno as bsno, branchdata.BranchName FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (branchdata.flag = 1) AND (branchroutes.BranchID = @BranchID) ORDER BY Routesno");
                            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                        }
                        DataTable dtbranch = vdm.SelectQuery(cmd).Tables[0];
                        DataView viewLeaks = new DataView(dtbranch);
                        DataTable distinctroutes = viewLeaks.ToTable(true, "salestype", "RouteName");
                        DataTable distinctagents = viewLeaks.ToTable(true, "RouteName", "bsno");
                        if (Totalcount != 1)
                        {
                            DataRow space1 = Report.NewRow();
                            space1["Agent Name"] = "Total";
                            if (ddlindenttype.SelectedValue == "Increase" || ddlindenttype.SelectedValue == "Decrese")
                            {
                                space1[frmdate] = totfromqty;
                                space1[tdate] = tottoqty;
                                grandtotfromqty += totfromqty;
                                grandtottoqty += tottoqty;
                                totfromqty = 0;
                                tottoqty = 0;
                            }
                            else
                            {
                                space1[frmdate] = totfromqty;
                                space1[tdate] = tottoqty;
                                grandtotfromqty += totfromqty;
                                grandtottoqty += tottoqty;
                                totfromqty = 0;
                                tottoqty = 0;
                            }
                            space1["Due"] = totamount;
                            space1["Crates"] = totcrates;
                            grandtotamount += totamount;
                            grandtotcrates += totcrates;
                            totamount = 0;
                            totcrates = 0;
                            if (ddlindenttype.SelectedValue == "All")
                            {
                                space1["Increase"] = increseqty;
                                space1["Decrease"] = decreseeqty;
                            }
                            if (ddlindenttype.SelectedValue == "Increase")
                            {
                                space1["Increase"] = increseqty;
                            }
                            if (ddlindenttype.SelectedValue == "Decrese")
                            {

                                space1["Decrease"] = decreseeqty;
                            }
                            increseqty = 0;
                            decreseeqty = 0;
                            Report.Rows.Add(space1);
                            DataRow empty = Report.NewRow();
                            empty["Agent Name"] = "";
                            DataRow[] drsalestype = dtbranch.Select("salestype='" + dr["sno"].ToString() + "'");
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
                        foreach (DataRow dragents in distinctroutes.Select("salestype='" + dr["sno"].ToString() + "'"))
                        {

                            foreach (DataRow drbranch in dtbranch.Select("salestype='" + dr["sno"].ToString() + "' and RouteName='" + dragents["RouteName"].ToString() + "'"))
                            {
                                //foreach (DataRow drbranch in dtbranch.Rows)
                                //{
                                DataRow newrow = Report.NewRow();
                                if (Routename != drbranch["RouteName"].ToString())
                                {
                                    if (Totalcount == 1)
                                    {
                                        newrow["Sno"] = i++.ToString();
                                        //newrow["Route Name"] = drbranch["RouteName"].ToString();
                                        Totalcount++;
                                        DataRow newrow13 = Report.NewRow();
                                        newrow13["Agent Name"] = dragents["RouteName"].ToString();
                                        Report.Rows.Add(newrow13);
                                    }
                                    else
                                    {
                                        if (Routename != drbranch["RouteName"].ToString())
                                        {
                                            if (Routename == "")
                                            {
                                            }
                                            else
                                            {
                                                DataRow space1 = Report.NewRow();
                                                space1["Agent Name"] = "Total";
                                                if (ddlindenttype.SelectedValue == "Increase" || ddlindenttype.SelectedValue == "Decrese")
                                                {
                                                    space1[frmdate] = totfromqty;
                                                    space1[tdate] = tottoqty;
                                                    grandtotfromqty += totfromqty;
                                                    grandtottoqty += tottoqty;
                                                    totfromqty = 0;
                                                    tottoqty = 0;
                                                }
                                                else
                                                {
                                                    space1[frmdate] = totfromqty;
                                                    space1[tdate] = tottoqty;
                                                    grandtotfromqty += totfromqty;
                                                    grandtottoqty += tottoqty;
                                                    totfromqty = 0;
                                                    tottoqty = 0;
                                                }
                                                space1["Due"] = totamount;
                                                space1["Crates"] = totcrates;
                                                grandtotamount += totamount;
                                                grandtotcrates += totcrates;
                                                totamount = 0;
                                                totcrates = 0;
                                                if (ddlindenttype.SelectedValue == "All")
                                                {
                                                    space1["Increase"] = increseqty;
                                                    space1["Decrease"] = decreseeqty;
                                                }
                                                if (ddlindenttype.SelectedValue == "Increase")
                                                {
                                                    space1["Increase"] = increseqty;
                                                }
                                                if (ddlindenttype.SelectedValue == "Decrese")
                                                {

                                                    space1["Decrease"] = decreseeqty;
                                                }
                                                Report.Rows.Add(space1);
                                                //newrow["Sno"] = i++.ToString();
                                                //newrow["Route Name"] = drbranch["RouteName"].ToString();
                                                //Totalcount++;
                                                //DataRow space = Report.NewRow();
                                                //space["Agent Name"] = "";
                                                //Report.Rows.Add(space);
                                                //totfromqty += fromqty;
                                                //tottoqty += toqty;
                                                //totincreseqty += increseqty;
                                                //totdecreseeqty += decreseeqty;

                                                fromqty = 0;
                                                toqty = 0;
                                                increseqty = 0;
                                                decreseeqty = 0;
                                            }
                                        }
                                        newrow["Sno"] = i++.ToString();
                                        //newrow["Route Name"] = drbranch["RouteName"].ToString();
                                        Totalcount++;
                                        if (Routename != drbranch["RouteName"].ToString())
                                        {
                                            if (Routename == "")
                                            {
                                            }
                                            else
                                            {
                                                DataRow space = Report.NewRow();
                                                space["Agent Name"] = "";
                                                Report.Rows.Add(space);
                                                DataRow newrow13 = Report.NewRow();
                                                newrow13["Agent Name"] = dragents["RouteName"].ToString();
                                                Report.Rows.Add(newrow13);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //newrow["Route Name"] = "";
                                }
                                Routename = drbranch["RouteName"].ToString();
                                //cmd = new MySqlCommand("SELECT branchroutesubtable.BranchID, ROUND(SUM(indents_subtable.unitQty), 2) AS indentqty, branchroutes.Sno AS RouteSno, branchroutes.RouteName,  branchdata.BranchName, branchdata.sno FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno LEFT OUTER JOIN  indents_subtable INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM indents WHERE (I_date BETWEEN @d1 AND @d2)) indnts ON indents_subtable.IndentNo = indnts.IndentNo ON  branchroutesubtable.BranchID = indnts.Branch_id WHERE  (branchdata.flag = 1) AND (branchroutes.Sno = @RouteID) GROUP BY branchdata.BranchName, branchdata.sno ORDER BY branchdata.sno");
                                //cmd.Parameters.AddWithValue("@RouteID", drbranch["Routesno"].ToString());
                                //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                                //cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                                //DataTable dtindentprev = vdm.SelectQuery(cmd).Tables[0];

                                //cmd = new MySqlCommand("SELECT branchroutesubtable.BranchID, ROUND(SUM(indents_subtable.unitQty), 2) AS indentqty, branchroutes.Sno AS RouteSno, branchroutes.RouteName,  branchdata.BranchName, branchdata.sno FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno LEFT OUTER JOIN  indents_subtable INNER JOIN (SELECT IndentNo, Branch_id, I_date FROM indents WHERE (I_date BETWEEN @d1 AND @d2)) indnts ON indents_subtable.IndentNo = indnts.IndentNo ON  branchroutesubtable.BranchID = indnts.Branch_id WHERE  (branchdata.flag = 1) AND (branchroutes.Sno = @RouteID) GROUP BY branchdata.BranchName, branchdata.sno ORDER BY branchdata.sno");
                                //cmd.Parameters.AddWithValue("@RouteID", drbranch["Routesno"].ToString());
                                //cmd.Parameters.AddWithValue("@d1", GetLowDate(todate));
                                //cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                                //DataTable dtindentpresent = vdm.SelectQuery(cmd).Tables[0];

                                double PrevIndent = 0;
                                double PresentIndent = 0;
                                double DifferenceIndent = 0;
                                double DifInd = 0;
                                if (dtIndentmonitoring.Rows.Count > 0)
                                {
                                    foreach (DataRow drprev in dtIndentmonitoring.Select("bsno='" + drbranch["bsno"].ToString() + "'"))
                                    {
                                        double.TryParse(drprev["Yesterday_Qty"].ToString(), out PrevIndent);
                                        if (ddlindenttype.SelectedValue == "All")
                                        {
                                            totfromqty += PrevIndent;
                                        }
                                    }
                                    string Remarks = "";
                                    foreach (DataRow drpresnt in dtIndentmonitoring.Select("bsno='" + drbranch["bsno"].ToString() + "'"))
                                    {
                                        double.TryParse(drpresnt["Today_Qty"].ToString(), out PresentIndent);
                                        if (ddlindenttype.SelectedValue == "All")
                                        {
                                            tottoqty += PresentIndent;
                                        }
                                        Remarks = drpresnt["Remarks"].ToString();
                                    }

                                    DifferenceIndent = PresentIndent - PrevIndent;
                                    DifInd = PresentIndent - PrevIndent;

                                    newrow["Remarks"] = Remarks;
                                    newrow["Agent Name"] = drbranch["BranchName"].ToString();
                                    newrow["SR Name"] = drbranch["SalesRepresentative"].ToString();
                                    //DataRow[] dragentamount = dtagentbalance.Select("BranchId=" + drbranch["bsno"].ToString());
                                    //foreach (DataRow drc in dragentamount)
                                    //{
                                    //    double dueamount = 0;
                                    //    double.TryParse(drc.ItemArray[1].ToString(), out dueamount);
                                    //    newrow["Due"] = dueamount;
                                    //    totamount += dueamount;
                                    //}
                                    //DataRow[] drinv = dtinvebalance.Select("BranchId=" + drbranch["bsno"].ToString());
                                    //foreach (DataRow drc in drinv)
                                    //{
                                    //    string crates = drc.ItemArray[2].ToString();
                                    //    if (crates == "1")
                                    //    {
                                    //        double dueinventory = 0;
                                    //        double.TryParse(drc.ItemArray[1].ToString(), out dueinventory);
                                    //        newrow["Crates"] = dueinventory;
                                    //        totcrates += dueinventory;
                                    //    }
                                    //}
                                    if (ddlindenttype.SelectedValue == "All")
                                    {
                                        if (DifferenceIndent >= 0)
                                        {
                                            DifferenceIndent = Math.Abs(DifferenceIndent);
                                            DifferenceIndent = Math.Round(DifferenceIndent, 2);
                                            newrow["Increase"] = DifferenceIndent;
                                            increseqty += DifferenceIndent;
                                            totincreseqty += DifferenceIndent;

                                            double per = 0;
                                            per = (DifferenceIndent / PrevIndent) * 100;
                                            per = Math.Round(per, 2);
                                            newrow["Increase %"] = per;
                                            newrow[frmdate] = PrevIndent;
                                            newrow[tdate] = PresentIndent;
                                        }
                                        else
                                        {
                                            if (PrevIndent == PresentIndent)
                                            {
                                            }
                                            else
                                            {
                                                DifferenceIndent = Math.Abs(DifferenceIndent);
                                                DifferenceIndent = Math.Round(DifferenceIndent, 2);
                                                newrow["Decrease"] = DifferenceIndent;
                                                decreseeqty += DifferenceIndent;
                                                totdecreseeqty += DifferenceIndent;
                                                double per = 0;
                                                per = (DifferenceIndent / PrevIndent) * 100;
                                                per = Math.Round(per, 2);
                                                newrow["Decrease %"] = per;
                                                newrow[frmdate] = PrevIndent;
                                                newrow[tdate] = PresentIndent;
                                            }
                                        }
                                    }
                                    if (DifferenceIndent > 0)
                                    {
                                        if (ddlindenttype.SelectedValue == "Increase")
                                        {
                                            DifferenceIndent = Math.Abs(DifferenceIndent);
                                            DifferenceIndent = Math.Round(DifferenceIndent, 2);
                                            newrow["Increase"] = DifferenceIndent;
                                            increseqty += DifferenceIndent;
                                            totincreseqty += DifferenceIndent;

                                            double per = 0;
                                            per = (DifferenceIndent / PrevIndent) * 100;
                                            per = Math.Round(per, 2);
                                            newrow["Increase %"] = per;
                                            newrow[frmdate] = PrevIndent;
                                            newrow[tdate] = PresentIndent;
                                            fromqty += PrevIndent;
                                            if (ddlindenttype.SelectedValue == "Increase")
                                            {
                                                totfromqty += PrevIndent;
                                                tottoqty += PresentIndent;
                                            }
                                            else
                                            {

                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (ddlindenttype.SelectedValue == "Decrese")
                                        {
                                            if (PrevIndent == PresentIndent)
                                            {
                                            }
                                            else
                                            {
                                                DifferenceIndent = Math.Abs(DifferenceIndent);
                                                DifferenceIndent = Math.Round(DifferenceIndent, 2);
                                                newrow["Decrease"] = DifferenceIndent;
                                                decreseeqty += DifferenceIndent;
                                                totdecreseeqty += DifferenceIndent;
                                                double per = 0;
                                                per = (DifferenceIndent / PrevIndent) * 100;
                                                per = Math.Round(per, 2);
                                                newrow["Decrease %"] = per;
                                                newrow[frmdate] = PrevIndent;
                                                newrow[tdate] = PresentIndent;
                                                toqty += PrevIndent;
                                                if (ddlindenttype.SelectedValue == "Decrese")
                                                {
                                                    totfromqty += PrevIndent;
                                                    tottoqty += PresentIndent;
                                                }
                                                else
                                                {
                                                    tottoqty += PresentIndent;
                                                }
                                            }
                                        }
                                    }
                                }
                                //newrow["Route Name"] = drbranch["RouteName"].ToString();
                                Report.Rows.Add(newrow);
                            }
                        }
                    }
                    DataTable dtReport = new DataTable();
                    dtReport.Columns.Add("SNo");
                    //dtReport.Columns.Add("Route Name");
                    dtReport.Columns.Add("Agent Name");
                    dtReport.Columns.Add("SR Name");
                    dtReport.Columns.Add(frmdate);
                    dtReport.Columns.Add(tdate);
                    dtReport.Columns.Add("Due");
                    dtReport.Columns.Add("Crates");
                    dtReport.Columns.Add("Increase");
                    dtReport.Columns.Add("Increase %");
                    dtReport.Columns.Add("Decrease");
                    dtReport.Columns.Add("Decrease %");
                    dtReport.Columns.Add("Remarks");

                    foreach (DataRow drbranch in Report.Rows)
                    {
                        DataRow space1 = dtReport.NewRow();
                        //string route = drbranch["Route Name"].ToString();
                        string Agent = drbranch["Agent Name"].ToString();
                        string From = drbranch[frmdate].ToString();
                        string To = drbranch[tdate].ToString();
                        if (From == "")
                        {
                            From = "0";
                        }
                        if (To == "")
                        {
                            To = "0";
                        }
                        if (Agent == "Total" && From == "0" && To == "0")
                        {
                        }
                        else
                        {
                            space1["SNo"] = drbranch["SNo"].ToString();
                            //space1["Route Name"] = drbranch["Route Name"].ToString();
                            space1["Agent Name"] = drbranch["Agent Name"].ToString();
                            space1["SR Name"] = drbranch["SR Name"].ToString();
                            space1[frmdate] = drbranch[frmdate].ToString();
                            space1[tdate] = drbranch[tdate].ToString();
                            space1["Due"] = drbranch["Due"].ToString();
                            space1["Crates"] = drbranch["Crates"].ToString();
                            space1["Increase"] = drbranch["Increase"].ToString();
                            space1["Increase %"] = drbranch["Increase %"].ToString();
                            space1["Decrease"] = drbranch["Decrease"].ToString();
                            space1["Decrease %"] = drbranch["Decrease %"].ToString();
                            space1["Remarks"] = drbranch["Remarks"].ToString();
                            dtReport.Rows.Add(space1);
                        }
                    }
                    DataRow space5 = dtReport.NewRow();
                    space5["Agent Name"] = "Total";
                    if (ddlindenttype.SelectedValue == "Increase" || ddlindenttype.SelectedValue == "Decrese")
                    {
                        space5[frmdate] = fromqty;
                        space5[tdate] = toqty;
                        grandtotfromqty += fromqty;
                        grandtottoqty += toqty;
                    }
                    if (ddlindenttype.SelectedValue == "All")
                    {
                        space5[frmdate] = totfromqty;
                        space5[tdate] = tottoqty;
                        grandtotfromqty += totfromqty;
                        grandtottoqty += tottoqty;
                    }
                    space5["Due"] = totamount;
                    space5["Crates"] = totcrates;
                    grandtotamount += totamount;
                    grandtotcrates += totcrates;
                    totamount = 0;
                    totcrates = 0;
                    if (ddlindenttype.SelectedValue == "All")
                    {

                        space5["Increase"] = increseqty;
                        space5["Decrease"] = decreseeqty;
                    }
                    if (ddlindenttype.SelectedValue == "Increase")
                    {
                        space5["Increase"] = increseqty;
                    }
                    if (ddlindenttype.SelectedValue == "Decrese")
                    {
                        space5["Decrease"] = decreseeqty;
                    }
                    dtReport.Rows.Add(space5);
                    DataRow space6 = dtReport.NewRow();
                    space6["Agent Name"] = "";
                    dtReport.Rows.Add(space6);
                    DataRow space3 = dtReport.NewRow();
                    space3["Agent Name"] = "Grand Total";
                    space3[frmdate] = grandtotfromqty;
                    space3[tdate] = grandtottoqty;
                    space3["Due"] = grandtotamount;
                    space3["Crates"] = grandtotcrates;
                    if (ddlindenttype.SelectedValue == "All")
                    {
                        space3["Increase"] = totincreseqty;
                        space3["Decrease"] = totdecreseeqty;
                    }
                    if (ddlindenttype.SelectedValue == "Increase")
                    {
                        space3["Increase"] = totincreseqty;
                    }
                    if (ddlindenttype.SelectedValue == "Decrese")
                    {
                        space3["Decrease"] = totdecreseeqty;
                    }
                    dtReport.Rows.Add(space3);
                    grdReports.DataSource = dtReport;
                    grdReports.DataBind();
                    Session["xportdata"] = dtReport;
                    pnlHide.Visible = true;
                    string test = "";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "indentincreasedecresereport(" + test + ");", true);

                }
            }
        }
        catch
        {
        }
    }
}