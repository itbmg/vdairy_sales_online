using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;


public partial class SalesOfficeWiseDue : System.Web.UI.Page
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
            cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, SUM(indents_subtable.DeliveryQty) AS saleQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue,modifiedroutes.Sno AS routesno FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT IndentNo, I_date, Branch_id FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indt ON modifidroutssubtab.BranchID = indt.Branch_id INNER JOIN indents_subtable ON indt.IndentNo = indents_subtable.IndentNo WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifiedroutes.Sno ORDER BY branchdata.sno, routesno");
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(Todate.AddDays(-1)));
            DataTable dtroutecollection = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routesno, SUM(colltion.AmountPaid) AS amtpaid FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT Branchid, AmountPaid, PaidDate FROM collections WHERE (PaymentType = 'CASH') AND (PaidDate BETWEEN @d1 AND @d2)) colltion ON modifidroutssubtab.BranchID = colltion.Branchid WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifiedroutes.Sno ORDER BY branchdata.sno, routesno");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            DataTable dtrouteamount = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routesno, inventory_monitor.Inv_Sno,SUM(inventory_monitor.Qty) AS balqty, invmaster.InvName FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN inventory_monitor ON modifidroutssubtab.BranchID = inventory_monitor.BranchId INNER JOIN invmaster ON inventory_monitor.Inv_Sno = invmaster.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifiedroutes.Sno, invmaster.sno ORDER BY branchdata.sno, routesno");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            DataTable dtrouteinventory = vdm.SelectQuery(cmd).Tables[0];
            Report = new DataTable();
            Report.Columns.Add("Route Name");
            Report.Columns.Add("Sale Qty").DataType = typeof(Double);
            Report.Columns.Add("Sale Value").DataType = typeof(Double);
            Report.Columns.Add("Received Amount").DataType = typeof(Double);
            Report.Columns.Add("Due Amount").DataType = typeof(Double);
            Report.Columns.Add("Crates Balance").DataType = typeof(Double);
            Report.Columns.Add("Cans Balance").DataType = typeof(Double);
            //Report.Columns.Add("Collected Amount").DataType = typeof(Double);
            //Report.Columns.Add("Payment Type");
            //Report.Columns.Add("Collection Type");

            foreach (DataRow branch in dtroutecollection.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["Route Name"] = branch["RouteName"].ToString();
                double salevalue = 0;
                double saleqty = 0;
                double crates = 0;
                double cans = 0;
                double.TryParse(branch["salevalue"].ToString(), out salevalue);
                double.TryParse(branch["saleQty"].ToString(), out saleqty);
                foreach (DataRow drdtclubtotal in dtrouteamount.Select("routesno='" + branch["routesno"].ToString() + "'"))
                {
                    double receivedamt = 0;
                    double due = 0;
                    double.TryParse(drdtclubtotal["amtpaid"].ToString(), out receivedamt);
                    newrow["Received Amount"] = Math.Round(receivedamt, 2);
                    due = salevalue - receivedamt;
                    newrow["Due Amount"] = Math.Round(due, 2);
                }
                foreach (DataRow drinventory in dtrouteinventory.Select("routesno='" + branch["routesno"].ToString() + "'"))
                {
                    if (drinventory["Inv_Sno"].ToString() == "1")
                    {
                        double invcrates = 0;
                        double.TryParse(drinventory["balqty"].ToString(), out invcrates);
                        crates += invcrates;
                    }
                    if (drinventory["Inv_Sno"].ToString() == "2")
                    {
                        double invcans = 0;
                        double.TryParse(drinventory["balqty"].ToString(), out invcans);
                        cans += invcans;
                    }
                    if (drinventory["Inv_Sno"].ToString() == "3")
                    {
                        double invcans = 0;
                        double.TryParse(drinventory["balqty"].ToString(), out invcans);
                        cans += invcans;
                    }
                    if (drinventory["Inv_Sno"].ToString() == "4")
                    {
                        double invcans = 0;
                        double.TryParse(drinventory["balqty"].ToString(), out invcans);
                        cans += invcans;
                    }
                    if (drinventory["Inv_Sno"].ToString() == "5")
                    {
                        double invcans = 0;
                        double.TryParse(drinventory["balqty"].ToString(), out invcans);
                        cans += invcans;
                    }
                }
                newrow["Sale Qty"] = Math.Round(saleqty, 2);
                newrow["Sale Value"] = Math.Round(salevalue, 2);
                newrow["Crates Balance"] = crates;
                newrow["Cans Balance"] = cans;
                
               
                Report.Rows.Add(newrow);
            }
            DataRow TotRow = Report.NewRow();
            TotRow["Route Name"] = "Total";
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