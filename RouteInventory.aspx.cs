using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class RouteInventory : System.Web.UI.Page
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
                FillAgentName();

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
                Ptype.Visible = true;
            }
            else
            {
                Ptype.Visible = false;
            }
            cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
            //cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            ddlDispName.DataSource = dtRoutedata;
            ddlDispName.DataTextField = "DispName";
            ddlDispName.DataValueField = "sno";
            ddlDispName.DataBind();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        if (Session["salestype"].ToString() == "Plant")
        {
            getPlantwiseReport();
        }
        else
        {
            GetReport();
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
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            Report = new DataTable();
            lblmsg.Text = "";
            pnlHide.Visible = true;
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
            lblDate.Text = fromdate.ToString("dd/MM/yyyy");
            lbl_selttodate.Text = todate.ToString("dd/MM/yyyy");
            Session["filename"] = "AGENT WISE INVENTORY TRANSACTION";
            lblAgent.Text = ddlDispName.SelectedItem.Text;
            cmd = new MySqlCommand("SELECT  tripinvdata.Qty, tripinvdata.Remaining, invmaster.InvName, invmaster.sno, tripdata.I_Date FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN tripinvdata ON tripdata.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (dispatch.sno = @dispatchSno) AND (tripdata.I_Date BETWEEN @d1 AND @d2) AND invmaster.sno <> 6  order by invmaster.sno,tripdata.I_Date");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@dispatchSno", ddlDispName.SelectedValue);
            DataTable dtInventory = vdm.SelectQuery(cmd).Tables[0];
            if (dtInventory.Rows.Count > 0)
            {
                DataView view = new DataView(dtInventory);
                Report.Columns.Add("Date");
                Report.Columns.Add("Issued crates").DataType = typeof(Int32);
                Report.Columns.Add("Received crates").DataType = typeof(Int32);
                Report.Columns.Add("Issued can 20ltr").DataType = typeof(Int32);
                Report.Columns.Add("Received can 20ltr").DataType = typeof(Int32);
                Report.Columns.Add("Issued can 40ltr").DataType = typeof(Int32);
                Report.Columns.Add("Received can 40ltr").DataType = typeof(Int32);
                int i = 1;
                int k = 0;
                DataTable distincttable = view.ToTable(true, "I_Date");
                foreach (DataRow drinv in distincttable.Rows)
                {
                    DataRow drnew = Report.NewRow();
                    string dtdate1 = drinv["I_Date"].ToString();
                    DateTime dtDOE1 = Convert.ToDateTime(dtdate1).AddDays(1);
                    string ChangedTime1 = dtDOE1.ToString("dd/MMM/yyyy");
                    drnew["Date"] = ChangedTime1;
                    foreach (DataRow drinvc in dtInventory.Rows)
                    {
                        string dtdate2 = drinvc["I_Date"].ToString();
                        DateTime dtDOE2 = Convert.ToDateTime(dtdate2).AddDays(1);
                        string ChangedTime2 = dtDOE2.ToString("dd/MMM/yyyy");
                        string InvName = drinvc["InvName"].ToString();
                        if (ChangedTime1 == ChangedTime2)
                        {
                            if (drinvc["sno"].ToString() == "1")
                            {
                                drnew["Issued crates"] = drinvc["Qty"].ToString();
                                drnew["Received crates"] = drinvc["Remaining"].ToString();
                            }
                            if (drinvc["sno"].ToString() == "3")
                            {
                                drnew["Issued can 20ltr"] = drinvc["Qty"].ToString();
                                drnew["Received can 20ltr"] = drinvc["Remaining"].ToString();
                            }
                            if (drinvc["sno"].ToString() == "4")
                            {
                                drnew["Issued can 40ltr"] = drinvc["Qty"].ToString();
                                drnew["Received can 40ltr"] = drinvc["Remaining"].ToString();
                            }
                            //else
                            //{
                            //    int Qty = 0;
                            //    int.TryParse(drinvc["Qty"].ToString(), out Qty);
                            //    int Remaining = 0;
                            //    int.TryParse(drinvc["Remaining"].ToString(), out Remaining);
                            //    Report.Rows[k][3] = Qty;
                            //    Report.Rows[k][4] = Remaining;
                            //}
                        }
                    }
                    Report.Rows.Add(drnew);
                    k++;
                }
                DataRow newvartical = Report.NewRow();
                newvartical["Date"] = "Total";
                int val = 0;
                foreach (DataColumn dc in Report.Columns)
                {
                    if (dc.DataType == typeof(Int32))
                    {
                        val = 0;
                        Int32.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                        newvartical[dc.ToString()] = val;
                    }
                }
                Report.Rows.Add(newvartical);
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
            }
            else
            {
                pnlHide.Visible = false;
                lblmsg.Text = "No Data Found";
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
    void getPlantwiseReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            lblmsg.Text = "";
            pnlHide.Visible = true;
            DataTable Report = new DataTable();
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
            if (ddlType.SelectedValue == "Day wise")
            {
                //ravi
                cmd = new MySqlCommand("SELECT        InvInfo.InvName, InvInfo.sno AS Invsno, ff.I_Date, InvInfo.VehicleNo, ff.DispName, InvInfo.Qty, InvInfo.Remaining, ff.Despsno FROM            (SELECT        invmaster.InvName, invmaster.sno, tripdata.Sno AS Tripsno, tripdata.VehicleNo, tripinvdata.Qty, tripinvdata.Remaining FROM            tripdata INNER JOIN tripinvdata ON tripdata.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE        (tripdata.I_Date BETWEEN @d1 AND @d2) AND (invmaster.sno <> 6)) InvInfo INNER JOIN (SELECT        I_Date, Branch_Id, Sno, DispName, Despsno FROM            (SELECT        tripdata_1.I_Date, dispatch.Branch_Id, tripdata_1.Sno, dispatch.DispName, dispatch.sno AS Despsno                                                          FROM            dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata tripdata_1 ON triproutes.Tripdata_sno = tripdata_1.Sno WHERE        (tripdata_1.I_Date BETWEEN @d1 AND @d2) AND (dispatch.Branch_Id = @BranchID)) TripInfo) ff ON ff.Sno = InvInfo.Tripsno GROUP BY ff.DispName, InvInfo.InvName ORDER BY Invsno");
                //cmd = new MySqlCommand("SELECT tripdata.Sno,tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, tripdata.Sno, tripinvdata.Qty, tripinvdata.Remaining, invmaster.InvName,invmaster.sno as invsno,tripinvdata.invid FROM tripdata INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripinvdata ON tripdata.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (dispatch.branch_id = @branch) AND (tripdata.I_Date BETWEEN @d1 AND @d2) AND invmaster.sno <> 6 order by invmaster.sno ");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            }
            else
            {
                cmd = new MySqlCommand("SELECT tripdata.Sno,tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, tripdata.Sno, tripinvdata.Qty, tripinvdata.Remaining, invmaster.InvName,invmaster.sno as invsno,tripinvdata.invid FROM tripdata INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripinvdata ON tripdata.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (dispatch.Sno = @DispSno) AND (tripdata.I_Date BETWEEN @d1 AND @d2) AND invmaster.sno <> 6 group by dispatch.DispName order by invmaster.sno ");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                cmd.Parameters.AddWithValue("@DispSno", ddlDispName.SelectedValue);
            }
            //cmd = new MySqlCommand("SELECT tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, tripdata.Sno, tripinvdata.invid, tripinvdata.Qty, tripinvdata.Remaining FROM tripdata INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripinvdata ON tripdata.Sno = tripinvdata.Tripdata_sno WHERE (dispatch.Branch_Id = @branch) AND (tripdata.I_Date BETWEEN @d1 AND @d2)"); 
           
            DataTable dtInventory = vdm.SelectQuery(cmd).Tables[0];
            if (dtInventory.Rows.Count > 0)
            {
                DataView view = new DataView(dtInventory);
                Report.Columns.Add("Date");
                Report.Columns.Add("Route Name");
                Report.Columns.Add("Issued crates").DataType = typeof(Int32);
                Report.Columns.Add("Received crates").DataType = typeof(Int32);
                Report.Columns.Add("Issued 10 ltr can").DataType = typeof(Int32);
                Report.Columns.Add("Received 10 ltr can").DataType = typeof(Int32);
                Report.Columns.Add("Issued 20 ltr can").DataType = typeof(Int32);
                Report.Columns.Add("Received 20 ltr can").DataType = typeof(Int32);
                Report.Columns.Add("Issued 40 ltr can").DataType = typeof(Int32);
                Report.Columns.Add("Received 40 ltr can").DataType = typeof(Int32);
                int i = 1;
                int k = 0;
                DataTable distincttable = view.ToTable(true, "I_Date", "DispName", "Despsno");

                foreach (DataRow drinvc in distincttable.Rows)
                {
                    DataRow drnew = Report.NewRow();
                    string dtdate1 = drinvc["I_Date"].ToString();
                    DateTime dtDOE1 = Convert.ToDateTime(dtdate1);
                    string ChangedTime1 = dtDOE1.ToString("dd/MMM/yyyy");
                    drnew["Date"] = ChangedTime1;

                    drnew["Route Name"] = drinvc["DispName"].ToString();
                    foreach (DataRow dr in dtInventory.Rows)
                    {
                        string dtdate2 = dr["I_Date"].ToString();
                        DateTime dtDOE2 = Convert.ToDateTime(dtdate2);
                        string ChangedTime2 = dtDOE2.ToString("dd/MMM/yyyy");
                        string InvName = dr["InvName"].ToString();
                        string invsno = dr["invsno"].ToString();

                        if (drinvc["Despsno"].ToString() == dr["Despsno"].ToString())
                        {
                            if (ChangedTime1 == ChangedTime2)
                            {
                                if (invsno == "1")
                                {
                                    drnew["Issued crates"] = dr["Qty"].ToString();
                                    drnew["Received crates"] = dr["Remaining"].ToString();
                                    Report.Rows.Add(drnew);

                                }
                                if (invsno == "2")
                                {
                                    //drnew["Issued can"] = drinvc["Qty"].ToString();
                                    //drnew["Received can"] = drinvc["Remaining"].ToString();
                                    int count = Report.Rows.Count - 1;
                                    Report.Rows[count][4] = dr["Qty"].ToString();
                                    Report.Rows[count][5] = dr["Remaining"].ToString();
                                }
                                if (invsno == "3")
                                {
                                    //drnew["Issued can"] = drinvc["Qty"].ToString();
                                    //drnew["Received can"] = drinvc["Remaining"].ToString();
                                    int count = Report.Rows.Count - 1;
                                    Report.Rows[count][6] = dr["Qty"].ToString();
                                    Report.Rows[count][7] = dr["Remaining"].ToString();
                                }
                                if (invsno == "4")
                                {
                                    //drnew["Issued can"] = drinvc["Qty"].ToString();
                                    //drnew["Received can"] = drinvc["Remaining"].ToString();
                                    int count = Report.Rows.Count - 1;
                                    Report.Rows[count][8] = dr["Qty"].ToString();
                                    Report.Rows[count][9] = dr["Remaining"].ToString();
                                }
                                //else
                                //{
                                //    //int Qty = 0;
                                //    //int.TryParse(drinvc["Qty"].ToString(), out Qty);
                                //    //int Remaining = 0;
                                //    //int.TryParse(drinvc["Remaining"].ToString(), out Remaining);
                                //    //Report.Rows[k][3] = Qty;
                                //    //Report.Rows[k][4] = Remaining;
                                //    drnew["Issued can"] = drinvc["Qty"].ToString();
                                //    drnew["Received can"] = drinvc["Remaining"].ToString();
                                //}
                            }

                        }
                    }

                    k++;
                }
                DataRow newvartical = Report.NewRow();
                newvartical["Date"] = "Total";
                int val = 0;
                foreach (DataColumn dc in Report.Columns)
                {
                    if (dc.DataType == typeof(Int32))
                    {
                        val = 0;
                        Int32.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                        newvartical[dc.ToString()] = val;
                    }
                }
                Report.Rows.Add(newvartical);
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
            }
            else
            {
                grdReports.DataSource = Report;
                grdReports.DataBind();
                lblmsg.Text = "No Data Found";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}