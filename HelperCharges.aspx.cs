using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class HelperCharges : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["branch"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                lblTitle.Text = Session["TitleName"].ToString();
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
            vdm = new VehicleDBMgr();
            pnlHide.Visible = true;
            int number = 11000;
            int roundednumber = RoundOff(number, 10);
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
            Session["xporttype"] = "TallySales";
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
            lbl_selfromdate.Text = fromdate.ToString();
            lbl_selttodate.Text = todate.ToString();
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName, dispatch.DispType, dispatch.DispMode FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (dispatch.DispType ='SM') AND (branchdata.sno = @BranchID) OR (dispatch.DispType ='SM') AND (branchdata_1.SalesOfficeID = @SOID)");
            }
            else
            {
                cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName, dispatch.DispType, dispatch.DispMode FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (dispatch.DispType IS NULL) AND (branchdata.sno = @BranchID) OR (dispatch.DispType IS NULL) AND (branchdata_1.SalesOfficeID = @SOID)");

            }
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtRoutes = vdm.SelectQuery(cmd).Tables[0];
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName, SUM(tripsubdata.Qty) AS dispatchqty, tripdat.AssignDate, tripdat.I_Date FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (dispatch.DispType ='SM') AND (branchdata.sno = @BranchID) OR (dispatch.DispType ='SM') AND (branchdata_1.SalesOfficeID = @SOID) GROUP BY dispatch.sno, tripdat.I_Date ORDER BY dispatch.sno");
            }
            else
            {
                cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName, SUM(tripsubdata.Qty) AS dispatchqty, tripdat.AssignDate, tripdat.I_Date FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (dispatch.DispType IS NULL) AND (branchdata.sno = @BranchID) OR (dispatch.DispType IS NULL) AND (branchdata_1.SalesOfficeID = @SOID) GROUP BY dispatch.sno, tripdat.I_Date ORDER BY dispatch.sno");

            }
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtRoutesData = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT ROUND(SUM(t2.Qty), 2) AS dispatchqty, t1.AssignDate AS I_Date, t1.BranchID AS sno, t1.BranchName AS DispName FROM (SELECT triproutes.Tripdata_sno, tripdata.AssignDate, branchdata.BranchName, branchdata.sno AS Expr1, dispatch.BranchID FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN branchdata ON branchdata.sno = dispatch.BranchID INNER JOIN branchdata branchdata_1 ON dispatch.BranchID = branchdata_1.sno WHERE (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)) t1 INNER JOIN (SELECT  SUM(tripsubdata.Qty) AS Qty, tripdata_1.Sno FROM tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno WHERE (tripdata_1.AssignDate BETWEEN @d1 AND @d2) AND (tripdata_1.Status <> 'C') GROUP BY tripdata_1.Sno) t2 ON t1.Tripdata_sno = t2.Sno GROUP BY DATE_FORMAT(t1.AssignDate, '%d/%b/%y'), t1.BranchID");
            //cmd = new MySqlCommand("SELECT  dispatch.sno, dispatch.DispName, SUM(tripsubdata.Qty) AS dispatchqty, tripdat.AssignDate, tripdat.I_Date, dispatch.BranchID, branchdata.SalesOfficeID, branchdata.BranchName FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN branchdata ON dispatch.BranchID = branchdata.sno WHERE  (dispatch.BranchID = @BranchID) OR (branchdata.SalesOfficeID = @SOID) GROUP BY dispatch.BranchID, tripdat.I_Date, branchdata.BranchName ORDER BY tripdat.I_Date");
            //cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName, SUM(tripsubdata.Qty) AS dispatchqty, tripdat.AssignDate, tripdat.I_Date, dispatch.BranchID, branchdata.SalesOfficeID FROM  dispatch INNER JOIN  triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN branchdata ON dispatch.BranchID = branchdata.sno WHERE (dispatch.BranchID = @BranchID) OR (branchdata.SalesOfficeID = @SOID)GROUP BY dispatch.sno, tripdat.I_Date ORDER BY dispatch.sno");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtPlantData = vdm.SelectQuery(cmd).Tables[0];
            if (dtRoutesData.Rows.Count > 0)
            {
                if (ddlreporttype.Text == "Helper Charges")
                {
                    DataView view = new DataView(dtRoutesData);
                    DataTable distinctproducts = view.ToTable(true, "DispName", "sno");
                    DataView view1 = new DataView(dtRoutesData);
                    DataTable distinctdate = view1.ToTable(true, "I_Date");
                    Report = new DataTable();
                    Report.Columns.Add("SNo");
                    Report.Columns.Add("Date");
                    int count = 0;
                    foreach (DataRow dr in distinctproducts.Rows)
                    {
                        Report.Columns.Add(dr["DispName"].ToString()).DataType = typeof(Double);
                        count++;
                    }
                    DataView viewPlant = new DataView(dtPlantData);
                    DataTable Plantproducts = viewPlant.ToTable(true, "DispName", "sno");
                    foreach (DataRow dr in Plantproducts.Rows)
                    {
                        string Disp = dr["DispName"].ToString();
                        //string[] strName = Disp.Split('_');
                        Report.Columns.Add(Disp).DataType = typeof(Double);
                        count++;
                        //string Disp = dr["DispName"].ToString();
                        //Report.Columns.Add(Disp).DataType = typeof(Double);
                    }
                    Report.Columns.Add("TOTAL").DataType = typeof(Double);
                    int i = 1;
                    foreach (DataRow branch in distinctdate.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["SNo"] = i++.ToString();
                        string dtdate1 = branch["I_Date"].ToString();
                        DateTime dtDOE1 = Convert.ToDateTime(dtdate1).AddDays(1);
                        string ChangedTime1 = dtDOE1.ToString("dd/MMM/yy");
                        newrow["Date"] = ChangedTime1;
                        double total = 0;
                        foreach (DataRow dr in dtRoutesData.Rows)
                        {
                            string dtdate2 = dr["I_Date"].ToString();
                            DateTime dtDOE2 = Convert.ToDateTime(dtdate2).AddDays(1);
                            string ChangedTime2 = dtDOE2.ToString("dd/MMM/yy");
                            int helpers = 0;
                            if (ChangedTime1 == ChangedTime2)
                            {
                                double dispatchqty = 0;
                                double.TryParse(dr["dispatchqty"].ToString(), out dispatchqty);
                                cmd = new MySqlCommand("SELECT helpermaster.sno, helpermaster.despsno, helpermaster.first, helpermaster.second, helpermaster.third, helpermaster.fourth, helpermaster.amount, helpermaster.flag, helpermaster.doe, dispatch.DispName FROM helpermaster INNER JOIN dispatch ON helpermaster.despsno = dispatch.sno WHERE (dispatch.DispName = @DispName)");
                                cmd.Parameters.AddWithValue("@DispName", dr["DispName"].ToString());
                                DataTable dtHelper = vdm.SelectQuery(cmd).Tables[0];
                                if (dtHelper.Rows.Count > 0)
                                {
                                    string fst = dtHelper.Rows[0]["first"].ToString();
                                    double first = 0;
                                    double.TryParse(fst, out first);
                                    string snd = dtHelper.Rows[0]["second"].ToString();
                                    double second = 0;
                                    double.TryParse(snd, out second);
                                    string thrd = dtHelper.Rows[0]["third"].ToString();
                                    double third = 0;
                                    double.TryParse(thrd, out third);
                                    string foth = dtHelper.Rows[0]["fourth"].ToString();
                                    double fourth = 0;
                                    double.TryParse(foth, out fourth);
                                    if (dispatchqty <= first)
                                    {
                                        helpers = 0;
                                    }
                                    else if (dispatchqty <= second)
                                    {
                                        helpers = 1;
                                    }
                                    else if (dispatchqty <= third)
                                    {
                                        helpers = 2;
                                    }
                                    else if (dispatchqty <= fourth)
                                    {
                                        helpers = 3;
                                    }
                                    else if (dispatchqty > fourth)
                                    {
                                        helpers = 4;
                                    }
                                    newrow[dr["DispName"].ToString()] = helpers;
                                    total += helpers;
                                }
                            }
                        }
                        newrow["TOTAL"] = total;
                        Report.Rows.Add(newrow);
                    }
                    foreach (DataRow drR in Report.Rows)
                    {
                        foreach (DataRow drP in dtPlantData.Rows)
                        {
                            string dtdate2 = drP["I_Date"].ToString();
                            DateTime dtDOE2 = Convert.ToDateTime(dtdate2).AddDays(1);
                            string ChangedTime2 = dtDOE2.ToString("dd/MMM/yy");
                            int helpers = 0;
                            string Disp = drP["DispName"].ToString();
                            //string[] strName = Disp.Split('_');
                            if (drR["Date"].ToString() == ChangedTime2)
                            {
                                double dispatchqty = 0;
                                double.TryParse(drP["dispatchqty"].ToString(), out dispatchqty);
                                cmd = new MySqlCommand("SELECT  helpermaster.sno, helpermaster.despsno, helpermaster.first, helpermaster.second, helpermaster.third, helpermaster.fourth, helpermaster.amount, helpermaster.flag, helpermaster.doe, dispatch.DispName, dispatch.Branch_Id FROM helpermaster INNER JOIN dispatch ON helpermaster.despsno = dispatch.sno WHERE (dispatch.Branchid = @Branchid)");
                                cmd.Parameters.AddWithValue("@Branchid", drP["sno"].ToString());
                                //cmd = new MySqlCommand("SELECT helpermaster.sno, helpermaster.despsno, helpermaster.first, helpermaster.second, helpermaster.third, helpermaster.fourth, helpermaster.amount, helpermaster.flag, helpermaster.doe, dispatch.DispName FROM helpermaster INNER JOIN dispatch ON helpermaster.despsno = dispatch.sno WHERE (dispatch.DispName = @DispName)");
                                //cmd.Parameters.AddWithValue("@DispName", drP["DispName"].ToString());
                                DataTable dtHelper = vdm.SelectQuery(cmd).Tables[0];
                                if (dtHelper.Rows.Count > 0)
                                {
                                    string fst = dtHelper.Rows[0]["first"].ToString();
                                    double first = 0;
                                    double.TryParse(fst, out first);
                                    string snd = dtHelper.Rows[0]["second"].ToString();
                                    double second = 0;
                                    double.TryParse(snd, out second);
                                    string thrd = dtHelper.Rows[0]["third"].ToString();
                                    double third = 0;
                                    double.TryParse(thrd, out third);
                                    string foth = dtHelper.Rows[0]["fourth"].ToString();
                                    double fourth = 0;
                                    double.TryParse(foth, out fourth);
                                    if (dispatchqty <= first)
                                    {
                                        helpers = 1;
                                    }
                                    else if (dispatchqty <= second)
                                    {
                                        helpers = 2;
                                    }
                                    else if (dispatchqty <= third)
                                    {
                                        helpers = 3;
                                    }
                                    else if (dispatchqty <= fourth)
                                    {
                                        helpers = 4;
                                    }
                                    else if (dispatchqty >= fourth)
                                    {
                                        helpers = 4;
                                    }
                                    drR[Disp] = helpers;
                                    double Emp = 0;
                                    double.TryParse(drR["TOTAL"].ToString(), out Emp);
                                    double totalemp = Emp + helpers;
                                    drR["TOTAL"] = totalemp;/////
                                }
                            }
                        }
                    }
                    DataRow New = Report.NewRow();
                    New["Date"] = "Total";
                    double valnewCash = 0.0;

                    DataRow newAvg = Report.NewRow();
                    newAvg["Date"] = "Amount";
                    double Avgval = 0.0;
                    double Totamount = 0.0;

                    DataRow newCharges = Report.NewRow();
                    newCharges["Date"] = "Charges";

                    cmd = new MySqlCommand("SELECT  helpermaster.despsno, dispatch.DispName FROM helpermaster INNER JOIN dispatch ON helpermaster.despsno = dispatch.sno");
                    DataTable dtdispatchsno = vdm.SelectQuery(cmd).Tables[0];
                    foreach (DataColumn dc in Report.Columns)
                    {
                        if (dc.DataType == typeof(Double))
                        {
                            var cell = dc.ColumnName;
                            if (cell == "TOTAL")
                            {
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out valnewCash);
                                New[dc.ToString()] = valnewCash;
                                newAvg[dc.ToString()] = Math.Round(Totamount, 2);
                                newCharges[dc.ToString()] = 0;
                            }
                            else
                            {
                                foreach (DataRow drdispsno in dtdispatchsno.Rows)
                                {
                                    cmd = new MySqlCommand("SELECT helpermaster.sno, helpermaster.despsno, helpermaster.first, helpermaster.second, helpermaster.third, helpermaster.fourth, helpermaster.amount, helpermaster.flag, helpermaster.doe, dispatch.DispName FROM helpermaster INNER JOIN dispatch ON helpermaster.despsno = dispatch.sno WHERE (helpermaster.despsno = @DispName)");
                                    cmd.Parameters.AddWithValue("@DispName", drdispsno["despsno"].ToString());
                                    DataTable dtHelper = vdm.SelectQuery(cmd).Tables[0];
                                    if (dtHelper.Rows.Count > 0)
                                    {
                                        string fstamount = dtHelper.Rows[0]["amount"].ToString();
                                        double amount = 0;
                                        double.TryParse(fstamount, out amount);
                                        valnewCash = 0.0;
                                        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out valnewCash);
                                        New[dc.ToString()] = valnewCash;
                                        Avgval = valnewCash * amount;
                                        Totamount += Avgval;
                                        newAvg[dc.ToString()] = Math.Round(Avgval, 2);
                                        newCharges[dc.ToString()] = Math.Round(amount, 2);
                                    }
                                }
                            }
                        }
                    }
                    Report.Rows.Add(New);
                    Report.Rows.Add(newAvg);
                    Report.Rows.Add(newCharges);
                    cmd = new MySqlCommand("SELECT SUM(tripinvdata.Qty) AS issued, SUM(tripinvdata.Remaining) AS returnqty, invmaster.InvName, invmaster.sno, dispatch.sno AS dispatchsno,dispatch.DispName FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripinvdata ON tripdat.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno WHERE (dispatch.Branch_Id = @brnchid) AND (dispatch.DispType IS NULL) OR (branchdata.SalesOfficeID = @brnchid) AND (dispatch.DispType IS NULL) GROUP BY dispatch.sno, invmaster.sno"); 
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@brnchid", Session["branch"].ToString());
                    DataTable dtInventory = vdm.SelectQuery(cmd).Tables[0];
                    DataView view2 = new DataView(dtInventory);
                    DataTable distincttable = view2.ToTable(true, "DispName", "dispatchsno");
                    DataRow newCrates = Report.NewRow();
                    newCrates["Date"] = "Crates";
                    foreach (DataRow drinv in distincttable.Rows)
                    {
                        string dtdate1 = drinv["dispatchsno"].ToString();
                        string dispatchname = drinv["DispName"].ToString();
                        foreach (DataRow drinvc in dtInventory.Rows)
                        {
                            string dtdate2 = drinvc["DispName"].ToString();
                            if (dispatchname == dtdate2)
                            {
                                if (drinvc["sno"].ToString() == "1")
                                {
                                    int issuedcrates = 0;
                                    int receivedcrates = 0;
                                    int diff = 0;
                                    int.TryParse(drinvc["issued"].ToString(), out issuedcrates);
                                    int.TryParse(drinvc["returnqty"].ToString(), out receivedcrates);
                                    diff = issuedcrates - receivedcrates;
                                    newCrates[drinvc["DispName"].ToString()] = diff;
                                }
                            }
                        }
                    }
                    Report.Rows.Add(newCrates);
                    DataRow newCans = Report.NewRow();
                    newCans["Date"] = "Cans";
                    foreach (DataRow drinv in distincttable.Rows)
                    {
                        string dtdate1 = drinv["dispatchsno"].ToString();
                        string dispatchname = drinv["DispName"].ToString();
                        int totcansissued = 0;
                        int totcansreceived = 0;
                        int diffcans = 0;
                        foreach (DataRow drinvc in dtInventory.Rows)
                        {
                            string dtdate2 = drinvc["DispName"].ToString();
                            if (dispatchname == dtdate2)
                            {
                                if (drinvc["sno"].ToString() == "2")
                                {
                                    int issuedcrates = 0;
                                    int receivedcrates = 0;
                                    int.TryParse(drinvc["issued"].ToString(), out issuedcrates);
                                    int.TryParse(drinvc["returnqty"].ToString(), out receivedcrates);
                                    totcansissued += issuedcrates;
                                    totcansreceived += receivedcrates;
                                    
                                }
                                if (drinvc["sno"].ToString() == "3")
                                {
                                    int issuedcrates = 0;
                                    int receivedcrates = 0;
                                    int.TryParse(drinvc["issued"].ToString(), out issuedcrates);
                                    int.TryParse(drinvc["returnqty"].ToString(), out receivedcrates);
                                    totcansissued += issuedcrates;
                                    totcansreceived += receivedcrates;
                                }
                                if (drinvc["sno"].ToString() == "4")
                                {
                                    int issuedcrates = 0;
                                    int receivedcrates = 0;
                                    int.TryParse(drinvc["issued"].ToString(), out issuedcrates);
                                    int.TryParse(drinvc["returnqty"].ToString(), out receivedcrates);
                                    totcansissued += issuedcrates;
                                    totcansreceived += receivedcrates;
                                }
                                if (drinvc["sno"].ToString() == "5")
                                {
                                    int issuedcrates = 0;
                                    int receivedcrates = 0;
                                    int.TryParse(drinvc["issued"].ToString(), out issuedcrates);
                                    int.TryParse(drinvc["returnqty"].ToString(), out receivedcrates);
                                    totcansissued += issuedcrates;
                                    totcansreceived += receivedcrates;
                                }

                            }
                        }
                        diffcans = totcansissued - totcansreceived;
                        newCans[drinv["DispName"].ToString()] = diffcans;
                    }
                    Report.Rows.Add(newCans);
                    DataRow lastRow = Report.Rows[Report.Rows.Count - 1];
                    DataRow newAmount = Report.NewRow();
                    newAmount["Date"] = "Amount";
                    foreach (DataRow drinv in distincttable.Rows)
                    {
                        string dtdate1 = drinv["dispatchsno"].ToString();
                        string dispatchname = drinv["DispName"].ToString();
                        foreach (DataRow drinvc in dtInventory.Rows)
                        {
                            string dtdate2 = drinvc["DispName"].ToString();
                            if (dispatchname == dtdate2)
                            {
                                    int crates = 0;
                                    int cans = 0;
                                    int diff = 0;
                                    int.TryParse(Report.Rows[Report.Rows.Count - 2][dtdate2].ToString(), out crates);
                                    int.TryParse(Report.Rows[Report.Rows.Count - 1][dtdate2].ToString(), out cans);
                                    crates = Math.Abs(crates) * 250;
                                    cans = Math.Abs(cans) * 400;
                                    diff = crates + cans;
                                    newAmount[drinvc["DispName"].ToString()] = diff;

                            }
                        }
                    }

                    Report.Rows.Add(newAmount);
                    //DataRow invCharges = Report.NewRow();
                    //invCharges["Date"] = "Charges";
                    //foreach (DataRow drinv in distincttable.Rows)
                    //{
                    //    string dtdate1 = drinv["dispatchsno"].ToString();
                    //    string dispatchname = drinv["DispName"].ToString();
                    //    foreach (DataRow drinvc in dtInventory.Rows)
                    //    {
                    //        string dtdate2 = drinvc["DispName"].ToString();
                    //        if (dispatchname == dtdate2)
                    //        {
                    //            if (drinvc["sno"].ToString() == "1")
                    //            {
                    //                invCharges[drinvc["DispName"].ToString()] = "200.400";
                    //            }

                    //        }
                    //    }
                    //}
                    //Report.Rows.Add(invCharges);
                    DataRow finalamount = Report.NewRow();
                    finalamount["Date"] = "Final Amount";
                    Report.Rows.Add(finalamount);
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;
                }
                if (ddlreporttype.Text == "Dispatch Qty")
                {
                    DataView view = new DataView(dtRoutesData);
                    DataTable distinctproducts = view.ToTable(true, "DispName", "sno");
                    DataView view1 = new DataView(dtRoutesData);
                    DataTable distinctdate = view1.ToTable(true, "I_Date");
                    Report = new DataTable();
                    Report.Columns.Add("SNo");
                    Report.Columns.Add("Date");
                    int count = 0;
                    foreach (DataRow dr in distinctproducts.Rows)
                    {
                        Report.Columns.Add(dr["DispName"].ToString()).DataType = typeof(Double);
                        count++;
                    }
                    DataView viewPlant = new DataView(dtPlantData);
                    DataTable Plantproducts = viewPlant.ToTable(true, "DispName", "sno");
                    foreach (DataRow dr in Plantproducts.Rows)
                    {
                        string Disp = dr["DispName"].ToString();
                        //string[] strName = Disp.Split('_');
                        Report.Columns.Add(Disp).DataType = typeof(Double);
                        count++;
                        //string Disp = dr["DispName"].ToString();
                        //Report.Columns.Add(Disp).DataType = typeof(Double);
                    }
                    Report.Columns.Add("TOTAL").DataType = typeof(Double);
                    int i = 1;
                    foreach (DataRow branch in distinctdate.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["SNo"] = i++.ToString();
                        string dtdate1 = branch["I_Date"].ToString();
                        DateTime dtDOE1 = Convert.ToDateTime(dtdate1).AddDays(1);
                        string ChangedTime1 = dtDOE1.ToString("dd/MMM/yy");
                        newrow["Date"] = ChangedTime1;
                        double total = 0;
                        foreach (DataRow dr in dtRoutesData.Rows)
                        {
                            string dtdate2 = dr["I_Date"].ToString();
                            DateTime dtDOE2 = Convert.ToDateTime(dtdate2).AddDays(1);
                            string ChangedTime2 = dtDOE2.ToString("dd/MMM/yy");
                            int helpers = 0;
                            if (ChangedTime1 == ChangedTime2)
                            {
                                double dispatchqty = 0;
                                double.TryParse(dr["dispatchqty"].ToString(), out dispatchqty);
                                newrow[dr["DispName"].ToString()] = Math.Round(dispatchqty, 2);
                                total += dispatchqty;
                            }
                        }
                         newrow["TOTAL"] = Math.Round(total, 2);
                         Report.Rows.Add(newrow);
                    }
                    foreach (DataRow drR in Report.Rows)
                    {
                        foreach (DataRow drP in dtPlantData.Rows)
                        {
                            string dtdate2 = drP["I_Date"].ToString();
                            DateTime dtDOE2 = Convert.ToDateTime(dtdate2).AddDays(1);
                            string ChangedTime2 = dtDOE2.ToString("dd/MMM/yy");
                            int helpers = 0;
                            string Disp = drP["DispName"].ToString();
                            //string[] strName = Disp.Split('_');
                            if (drR["Date"].ToString() == ChangedTime2)
                            {
                                double dispatchqty = 0;
                                double.TryParse(drP["dispatchqty"].ToString(), out dispatchqty);
                                cmd = new MySqlCommand("SELECT  helpermaster.sno, helpermaster.despsno, helpermaster.first, helpermaster.second, helpermaster.third, helpermaster.fourth, helpermaster.amount, helpermaster.flag, helpermaster.doe, dispatch.DispName, dispatch.Branch_Id FROM helpermaster INNER JOIN dispatch ON helpermaster.despsno = dispatch.sno WHERE (dispatch.BranchID = @Branchid)");
                                cmd.Parameters.AddWithValue("@Branchid", drP["sno"].ToString());

                                //cmd = new MySqlCommand("SELECT helpermaster.sno, helpermaster.despsno, helpermaster.first, helpermaster.second, helpermaster.third, helpermaster.fourth, helpermaster.amount, helpermaster.flag, helpermaster.doe, dispatch.DispName FROM helpermaster INNER JOIN dispatch ON helpermaster.despsno = dispatch.sno WHERE (dispatch.DispName = @DispName)");
                                //cmd.Parameters.AddWithValue("@DispName", drP["DispName"].ToString());
                                DataTable dtHelper = vdm.SelectQuery(cmd).Tables[0];
                                if (dtHelper.Rows.Count > 0)
                                {
                                    string fst = dtHelper.Rows[0]["first"].ToString();
                                    double first = 0;
                                    double.TryParse(fst, out first);
                                    string snd = dtHelper.Rows[0]["second"].ToString();
                                    double second = 0;
                                    double.TryParse(snd, out second);
                                    string thrd = dtHelper.Rows[0]["third"].ToString();
                                    double third = 0;
                                    double.TryParse(thrd, out third);
                                    string foth = dtHelper.Rows[0]["fourth"].ToString();
                                    double fourth = 0;
                                    double.TryParse(foth, out fourth);
                                    if (dispatchqty <= first)
                                    {
                                        helpers = 1;
                                    }
                                    else if (dispatchqty <= second)
                                    {
                                        helpers = 2;
                                    }
                                    else if (dispatchqty <= third)
                                    {
                                        helpers = 3;
                                    }
                                    else if (dispatchqty <= fourth)
                                    {
                                        helpers = 4;
                                    }
                                    else if (dispatchqty >= fourth)
                                    {
                                        helpers = 4;
                                    }
                                    drR[Disp] = Math.Round(dispatchqty, 2);
                                    double Emp = 0;
                                    double.TryParse(drR["TOTAL"].ToString(), out Emp);
                                    double totalemp = Emp + dispatchqty;
                                    drR["TOTAL"] = Math.Round(totalemp, 2);
                                }
                            }
                        }
                    }
                    DataRow New = Report.NewRow();
                    New["Date"] = "Total";
                    double valnewCash = 0.0;
                    foreach (DataColumn dc in Report.Columns)
                    {
                        if (dc.DataType == typeof(Double))
                        {
                            var cell = dc.ColumnName;
                           
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out valnewCash);
                                New[dc.ToString()] = Math.Round(valnewCash, 2);
                                //newAvg[dc.ToString()] = Math.Round(Totamount, 2);
                                //newCharges[dc.ToString()] = 0;
                        }
                    }
                    Report.Rows.Add(New);
                    //Report.Rows.Add(newAvg);
                    //Report.Rows.Add(newCharges);
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;
                }
                
            }
        }
        catch(Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdReports.DataSource = Report;
            grdReports.DataBind();
        }
    }
    public int RoundOff(int number, int interval)
    {
        int remainder = number % interval;
        number += (remainder < interval / 2) ? -remainder : (interval - remainder);
        return number;
    }
}