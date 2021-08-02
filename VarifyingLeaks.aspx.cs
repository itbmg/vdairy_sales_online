using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class VarifyingLeaks : System.Web.UI.Page
{
    MySqlCommand cmd;
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["salestype"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        //vdm = new VehicleDBMgr();
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                lblTitle.Text = Session["TitleName"].ToString();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
    DateTime fromdate = DateTime.Now;
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            vdm = new VehicleDBMgr();
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
            txtdate.Text = fromdate.ToString("dd-MM-yyyy HH:mm");
            lblDate.Text = fromdate.AddDays(-1).ToString("MM-dd-yyyy HH:mm");
            DataTable dtleaks = new DataTable();
            if (Session["salestype"].ToString() == "Plant")
            {
                //cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName, leakages.VLeaks, leakages.VReturns, leakages.TotalLeaks,tripdata.sno as tripsno, leakages.ReturnQty, productsdata.ProductName FROM triproutes INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) AND ((dispatch.DispType = @DispType) OR (dispatch.DispType = @DispTy)) GROUP BY dispatch.DispName, productsdata.ProductName Order by dispatch.sno");
                //Ravi
                cmd = new MySqlCommand("SELECT Leaks.VLeaks, Leaks.ProductName, ff.DispName, ff.DespSno AS Sno, Leaks.VReturns, Leaks.TotalLeaks, Leaks.Sno AS tripsno, Leaks.ReturnQty FROM (SELECT leakages.VLeaks, leakages.VReturns, leakages.TotalLeaks, productsdata.ProductName, tripdata_1.Sno, leakages.ReturnQty FROM tripdata tripdata_1 INNER JOIN leakages ON tripdata_1.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata_1.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno FROM (SELECT dispatch.DispName, tripdata.Sno, dispatch.sno AS DespSno FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID) AND (dispatch.DispType = @DispType OR dispatch.DispType = @DispTy)) TripInfo) ff ON ff.Sno = Leaks.Sno GROUP BY ff.DispName, Leaks.ProductName, ff.DespSno");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@DispType", "SO");
                cmd.Parameters.AddWithValue("@DispTy", "SM");
                dtleaks = vdm.SelectQuery(cmd).Tables[0];
            }
            else
            {
                //cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName, leakages.VLeaks, leakages.VReturns, leakages.TotalLeaks, leakages.ReturnQty, productsdata.ProductName FROM triproutes INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @BranchID) GROUP BY dispatch.DispName, productsdata.ProductName ORDER BY dispatch.sno");
                ////cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName, leakages.VLeaks, leakages.VReturns, leakages.TotalLeaks, leakages.ReturnQty,tripdata.sno as tripsno, productsdata.ProductName FROM triproutes INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN branchdata ON dispatch.BranchID = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.BranchID = branchdata_1.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchdata.sno = @BranchID) OR (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchdata.SalesOfficeID = @BranchID) GROUP BY dispatch.DispName, productsdata.ProductName ORDER BY dispatch.sno");
                cmd = new MySqlCommand("SELECT Leaks.VLeaks, Leaks.ProductName, ff.DispName, ff.DespSno AS Sno, Leaks.VReturns, Leaks.TotalLeaks, Leaks.Sno AS tripsno, Leaks.ReturnQty FROM (SELECT leakages.VLeaks, leakages.VReturns, leakages.TotalLeaks, productsdata.ProductName, tripdata_1.Sno, leakages.ReturnQty FROM tripdata tripdata_1 INNER JOIN leakages ON tripdata_1.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata_1.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, DespSno, tripsno FROM (SELECT dispatch.sno AS DespSno, dispatch.DispName, tripdata.Sno AS tripsno FROM triproutes INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN branchdata ON dispatch.BranchID = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.BranchID = branchdata_1.sno WHERE        (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchdata.sno = @BranchID) OR (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchdata.SalesOfficeID = @BranchID)) TripInfo) ff ON ff.tripsno = Leaks.Sno GROUP BY ff.DispName, ff.DespSno");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@DispType", "SO");
                cmd.Parameters.AddWithValue("@DispTy", "SM");
                dtleaks = vdm.SelectQuery(cmd).Tables[0];

            }
            if (dtleaks.Rows.Count > 0)
            {
                Report = new DataTable();
                Report.Columns.Add("Product Name");
                Report.Columns.Add("Sub Leak");
                Report.Columns.Add("Verified Leak");
                Report.Columns.Add("Sub Returns");
                Report.Columns.Add("Verified Returns");
                DataView view = new DataView(dtleaks);
                DataTable dtdisp = view.ToTable(true, "DispName", "sno");
                foreach (DataRow dr in dtdisp.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Verified Leak"] = dr["DispName"].ToString();
                    Report.Rows.Add(newrow);
                    cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName, dispatch.Branch_Id, invtransactions12.Qty, invtransactions12.VQty, invtransactions12.B_inv_sno, invmaster.InvName FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN invtransactions12 ON tripdat.Sno = invtransactions12.ToTran AND dispatch.BranchID = invtransactions12.FromTran INNER JOIN invmaster ON invtransactions12.B_inv_sno = invmaster.sno WHERE (dispatch.sno = @dispsno)");
                    cmd.Parameters.AddWithValue("@dispsno", dr["sno"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    DataTable DtverifiedData = vdm.SelectQuery(cmd).Tables[0];
                    double Leaks = 0;
                    double VLeaks = 0;
                    double Returns = 0;
                    double VReturns = 0;
                    foreach (DataRow drleakreturn in dtleaks.Rows)
                    {
                        if (drleakreturn["sno"].ToString() == dr["sno"].ToString())
                        {
                            double DLeaks = 0;
                            double DVLeaks = 0;
                            double DReturns = 0;
                            double DVReturns = 0;
                            DataRow drnewleak = Report.NewRow();
                            drnewleak["Product Name"] = drleakreturn["ProductName"].ToString();
                            drnewleak["Sub Leak"] = drleakreturn["TotalLeaks"].ToString();
                            double.TryParse(drleakreturn["TotalLeaks"].ToString(), out DLeaks);
                            Leaks += DLeaks;
                            drnewleak["Verified Leak"] = drleakreturn["VLeaks"].ToString();
                            double.TryParse(drleakreturn["VLeaks"].ToString(), out DVLeaks);
                            VLeaks += DVLeaks;
                            drnewleak["Sub Returns"] = drleakreturn["ReturnQty"].ToString();
                            double.TryParse(drleakreturn["ReturnQty"].ToString(), out DReturns);
                            Returns += DReturns;
                            drnewleak["Verified Returns"] = drleakreturn["VReturns"].ToString();
                            double.TryParse(drleakreturn["VReturns"].ToString(), out DVReturns);
                            VReturns += DVReturns;
                            Report.Rows.Add(drnewleak);
                        }
                    }
                    DataRow drnew1 = Report.NewRow();
                    drnew1["Product Name"] = "Total Leaks";
                    drnew1["Sub Leak"] = Leaks;
                    drnew1["Verified Leak"] = VLeaks;
                    drnew1["Sub Returns"] = Returns;
                    drnew1["Verified Returns"] = VReturns;
                    Report.Rows.Add(drnew1);
                    DataRow drbreak = Report.NewRow();
                    drbreak["Verified Leak"] = "";
                    Report.Rows.Add(drbreak);
                    DataRow drnew2 = Report.NewRow();
                    drnew2["Verified Leak"] = "Inventory";
                    Report.Rows.Add(drnew2);
                    DataRow drbreak1 = Report.NewRow();
                    drbreak1["Verified Leak"] = "";
                    Report.Rows.Add(drbreak1);
                    DataRow drnew3 = Report.NewRow();
                    drnew3["Product Name"] = "Inventory Name";
                    drnew3["Sub Leak"] = "Submited Inventory";
                    drnew3["Verified Leak"] = "Verified Inventory";
                    Report.Rows.Add(drnew3);
                    int Vtotcan40ltr = 0;
                    int Vtotcrates = 0;
                    int Dtotcrates = 0;
                    int Dtotcan20ltr = 0;
                    int Vtotcan20ltr = 0;
                    int Dtotcan40ltr = 0;
                    foreach (DataRow drtripinv in DtverifiedData.Rows)
                    {
                        if (drtripinv["B_inv_sno"].ToString() == "1")
                        {
                            int Dcrates = 0;
                            int.TryParse(drtripinv["Qty"].ToString(), out Dcrates);
                            Dtotcrates += Dcrates;


                            int Vcrates = 0;
                            int.TryParse(drtripinv["VQty"].ToString(), out Vcrates);
                            Vtotcrates += Vcrates;
                            DataRow drnew = Report.NewRow();

                            drnew["Product Name"] = drtripinv["InvName"].ToString();
                            drnew["Sub Leak"] = Dcrates;
                            drnew["Verified Leak"] = Vcrates;
                            Report.Rows.Add(drnew);

                        }
                        if (drtripinv["B_inv_sno"].ToString() == "3")
                        {
                            int Dcan20ltr = 0;
                            int.TryParse(drtripinv["Qty"].ToString(), out Dcan20ltr);
                            Dtotcan20ltr += Dcan20ltr;


                            int Vcan20ltr = 0;
                            int.TryParse(drtripinv["VQty"].ToString(), out Vcan20ltr);
                            Vtotcan20ltr += Vcan20ltr;
                            DataRow drnew = Report.NewRow();

                            drnew["Product Name"] = drtripinv["InvName"].ToString();
                            drnew["Sub Leak"] = Dcan20ltr;
                            drnew["Verified Leak"] = Vcan20ltr;
                            Report.Rows.Add(drnew);



                        }
                        if (drtripinv["B_inv_sno"].ToString() == "4")
                        {
                            int Dcan40ltr = 0;
                            int.TryParse(drtripinv["Qty"].ToString(), out Dcan40ltr);
                            Dtotcan40ltr += Dcan40ltr;


                            int Vcan40ltr = 0;
                            int.TryParse(drtripinv["VQty"].ToString(), out Vcan40ltr);
                            Vtotcan40ltr += Vcan40ltr;
                            DataRow drnew = Report.NewRow();

                            drnew["Product Name"] = drtripinv["InvName"].ToString();
                            drnew["Sub Leak"] = Dcan40ltr;
                            drnew["Verified Leak"] = Vcan40ltr;
                            Report.Rows.Add(drnew);

                        }

                    }
                    DataRow drbreak2 = Report.NewRow();
                    drbreak2["Verified Leak"] = "";
                    Report.Rows.Add(drbreak2);
                }

                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
            else
            {
                pnlHide.Visible = false;
                lblmsg.Text = "No leaks Found:";
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
}