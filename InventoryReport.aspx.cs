using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;
using System.IO;

public partial class InventoryReport : System.Web.UI.Page
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
                txttodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
            }
        }
    }
    protected void GetAgentInventoryDetails_Click(object sender, EventArgs e)
    {
        Panelinventory.Visible = true;
    }
    protected void GetInventoryTransactionsDetails_Click(object sender, EventArgs e)
    {
        Panelinventory.Visible = false;
        Response.Redirect("AgentWiseInventory.aspx", true);
    }
    protected void GetDayWiseInventoryDetails_Click(object sender, EventArgs e)
    {
        Panelinventory.Visible = false;
        Response.Redirect("RouteInventory.aspx", true);
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
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                PBranch.Visible = false;
                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (dispatch.flag=1) AND (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlDispName.DataSource = dtRoutedata;
                ddlDispName.DataTextField = "DispName";
                ddlDispName.DataValueField = "sno";
                ddlDispName.DataBind();
            }
        }
        catch(Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (dispatch.flag=1) AND (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlDispName.DataSource = dtRoutedata;
        ddlDispName.DataTextField = "DispName";
        ddlDispName.DataValueField = "sno";
        ddlDispName.DataBind();
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
            Report = new DataTable();
            Session["RouteName"] = ddlDispName.SelectedItem.Text;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            DateTime Todate = DateTime.Now;

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

            string[] datestrig1 = txttodate.Text.Split(' ');

            if (datestrig1.Length > 1)
            {
                if (datestrig1[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig1[0].Split('-');
                    string[] times = datestrig1[1].Split(':');
                    Todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            lblDate.Text = fromdate.ToString("dd/MMM/yyyy");
            lblRouteName.Text = ddlDispName.SelectedItem.Text;

            // cmd = new MySqlCommand("SELECT inventory_monitor.Inv_Sno, inventory_monitor.BranchId, inventory_monitor.Qty, inventory_monitor.Sno, inventory_monitor.EmpId, inventory_monitor.lostQty FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutesubtable ON dispatch_sub.Route_id = branchroutesubtable.RefNo INNER JOIN inventory_monitor ON branchroutesubtable.BranchID = inventory_monitor.BranchId WHERE (dispatch.sno = @dispsno)");
            cmd = new MySqlCommand("SELECT inventory_monitor.Inv_Sno, inventory_monitor.BranchId, inventory_monitor.Qty, inventory_monitor.Sno, inventory_monitor.EmpId, inventory_monitor.lostQty FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutesubtable ON dispatch_sub.Route_id = modifiedroutesubtable.RefNo INNER JOIN inventory_monitor ON modifiedroutesubtable.BranchID = inventory_monitor.BranchId WHERE (dispatch.sno = @dispsno) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) AND (inventory_monitor.Qty>0) OR (dispatch.sno = @dispsno) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) AND (inventory_monitor.Qty>0)");
            cmd.Parameters.AddWithValue("@dispsno", ddlDispName.SelectedValue);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            DataTable dtinventoryopp = vdm.SelectQuery(cmd).Tables[0];

            //cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, branchroutes.RouteName, branchroutes.Sno AS routesno FROM branchdata INNER JOIN branchroutesubtable ON branchdata.sno = branchroutesubtable.BranchID INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno INNER JOIN dispatch_sub ON branchroutes.Sno = dispatch_sub.Route_id INNER JOIN dispatch ON dispatch_sub.dispatch_sno = dispatch.sno WHERE (dispatch.sno = @routeid) and branchdata.flag='1'");
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.RouteName, modifiedroutes.Sno AS routesno FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutes ON dispatch_sub.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno WHERE (dispatch.sno = @routeid) AND (branchdata.flag = '1') AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (dispatch.sno = @routeid) AND (branchdata.flag = '1') AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime)");
            cmd.Parameters.AddWithValue("@routeid", ddlDispName.SelectedValue);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            DataTable dtbranch = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand(" SELECT InvName FROM invmaster");
            DataTable InvTable = vdm.SelectQuery(cmd).Tables[0];



            Report = new DataTable();
            Report.Columns.Add("Sno");
            Report.Columns.Add("Branch Code");
            Report.Columns.Add("Agent Name");
            Report.Columns.Add("Opp Crates", typeof(Double));
            Report.Columns.Add("Issued Crates", typeof(Double));
            Report.Columns.Add("Received Crates", typeof(Double));
            Report.Columns.Add("Difference Crates", typeof(Double));
            Report.Columns.Add("CB Crates", typeof(Double));
            Report.Columns.Add("Opp Can40ltr", typeof(Double));
            Report.Columns.Add("Issued Can40ltr", typeof(Double));
            Report.Columns.Add("Received Can40ltr", typeof(Double));
            Report.Columns.Add("Difference Can40ltr", typeof(Double));
            Report.Columns.Add("CB Can40ltr", typeof(Double));
            Report.Columns.Add("Opp Can20ltr", typeof(Double));
            Report.Columns.Add("Issued Can20ltr", typeof(Double));
            Report.Columns.Add("Received Can20ltr", typeof(Double));
            Report.Columns.Add("Difference Can20ltr", typeof(Double));
            Report.Columns.Add("CB Can20ltr", typeof(Double));

            Report.Columns.Add("Opp Can10ltr", typeof(Double));
            Report.Columns.Add("Issued Can10ltr", typeof(Double));
            Report.Columns.Add("Received Can10ltr", typeof(Double));
            Report.Columns.Add("Difference Can10ltr", typeof(Double));
            Report.Columns.Add("CB Can10ltr", typeof(Double));
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            int i = 1;

           // cmd = new MySqlCommand("SELECT invtras.TransType, invtras.FromTran, invtras.ToTran, invtras.Qty, invtras.DOE, invmaster.sno AS invsno, invmaster.InvName FROM (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty FROM invtransactions12 WHERE (ToTran = @branchid) AND (DOE BETWEEN @d1 AND @d2) OR (DOE BETWEEN @d1 AND @d2) AND (FromTran = @branchid)) invtras INNER JOIN invmaster ON invtras.B_inv_sno = invmaster.sno ORDER BY invtras.DOE");
            cmd = new MySqlCommand("SELECT invtras.TransType, invtras.FromTran, invtras.ToTran, invtras.Qty, invtras.DOE, invmaster.sno AS invsno, invmaster.InvName FROM (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty FROM invtransactions12 WHERE  (DOE BETWEEN @d1 AND @d2) OR (DOE BETWEEN @d1 AND @d2)) invtras INNER JOIN invmaster ON invtras.B_inv_sno = invmaster.sno ORDER BY invtras.DOE");
           // cmd.Parameters.AddWithValue("@branchid", drroutebrnch["sno"].ToString());
            DateTime dt1 = GetLowDate(fromdate.AddDays(-1));
            DateTime dt2 = GetLowDate(Todate);
            cmd.Parameters.AddWithValue("@d1", dt1.AddHours(15));
            cmd.Parameters.AddWithValue("@d2", dt2.AddHours(15));
            DataTable dtinventoryDC = vdm.SelectQuery(cmd).Tables[0];
            // cmd = new MySqlCommand("SELECT invtransactions12.TransType, invtransactions12.FromTran, invtransactions12.ToTran, invtransactions12.Qty, invtransactions12.DOE, invmaster.sno AS invsno,invmaster.InvName FROM invtransactions12 INNER JOIN invmaster ON invtransactions12.B_inv_sno = invmaster.sno WHERE (invtransactions12.ToTran = @branchid) AND (invtransactions12.DOE BETWEEN @d1 AND @d2) OR (invtransactions12.DOE BETWEEN @d1 AND @d2) AND (invtransactions12.FromTran = @branchid) ORDER BY invtransactions12.DOE");
            
            //cmd = new MySqlCommand("SELECT invtran.TransType, invtran.FromTran, invtran.ToTran, invtran.Qty, invtran.DOE, invmaster.sno AS invsno, invmaster.InvName FROM (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty FROM invtransactions12 WHERE (ToTran = @branchid) AND (DOE BETWEEN @d1 AND @d2) OR (DOE BETWEEN @d1 AND @d2) AND (FromTran = @branchid)) invtran INNER JOIN invmaster ON invtran.B_inv_sno = invmaster.sno ORDER BY invtran.DOE");
            cmd = new MySqlCommand("SELECT invtran.TransType, invtran.FromTran, invtran.ToTran, invtran.Qty, invtran.DOE, invmaster.sno AS invsno, invmaster.InvName FROM (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty FROM invtransactions12 WHERE  (DOE BETWEEN @d1 AND @d2) OR (DOE BETWEEN @d1 AND @d2) ) invtran INNER JOIN invmaster ON invtran.B_inv_sno = invmaster.sno ORDER BY invtran.DOE");
           // cmd.Parameters.AddWithValue("@branchid", drroutebrnch["sno"].ToString());
            DateTime dtmin = GetLowDate(fromdate.AddDays(-1));
            DateTime dtmax = GetLowDate(ServerDateCurrentdate);
            cmd.Parameters.AddWithValue("@d1", dtmin.AddHours(15));
            cmd.Parameters.AddWithValue("@d2", dtmax.AddHours(15));
            DataTable dtprevinventoryDC = vdm.SelectQuery(cmd).Tables[0];

            foreach (DataRow drroutebrnch in dtbranch.Rows)
            {
                DataRow drnew = Report.NewRow();
                drnew["Sno"] = i;
                int oppcrates = 0;
                int oppcan10ltr = 0;
                int oppcan20ltr = 0;
                int oppcan40ltr = 0;
                //cmd = new MySqlCommand("SELECT BranchId, Inv_Sno, Qty, Sno, EmpId, lostQty FROM inventory_monitor WHERE (BranchId = @branchid)");
                //cmd.Parameters.AddWithValue("@branchid", drbranch["sno"].ToString());
                //DataTable dtinventoryopp = vdm.SelectQuery(cmd).Tables[0];

                foreach (DataRow dropp in dtinventoryopp.Select("BranchId='" + drroutebrnch["sno"].ToString() + "'"))
                {
                    if (dropp["Inv_Sno"].ToString() == "1")
                    {
                        int.TryParse(dropp["Qty"].ToString(), out oppcrates);
                    }
                    if (dropp["Inv_Sno"].ToString() == "2")
                    {
                        int.TryParse(dropp["Qty"].ToString(), out oppcan10ltr);
                    }
                    if (dropp["Inv_Sno"].ToString() == "3")
                    {
                        int.TryParse(dropp["Qty"].ToString(), out oppcan20ltr);
                    }
                    if (dropp["Inv_Sno"].ToString() == "4")
                    {
                        int.TryParse(dropp["Qty"].ToString(), out oppcan40ltr);
                    }
                }
               
                drnew["Branch Code"] = drroutebrnch["sno"].ToString();
                drnew["Agent Name"] = drroutebrnch["BranchName"].ToString();
                int Ctotcan40ltr = 0;
                int Ctotcrates = 0;
                int Ctotcan20ltr = 0;
                int Dtotcrates = 0;
                int Dtotcan20ltr = 0;
                int Dtotcan40ltr = 0;
                int prevCtotcan40ltr = 0;
                int prevCtotcrates = 0;
                int prevCtotcan20ltr = 0;
                int prevDtotcrates = 0;
                int prevDtotcan20ltr = 0;
                int prevDtotcan40ltr = 0;
                int prevDtotcan10ltr = 0;
                int prevCtotcan10ltr = 0;
                int Dtotcan10ltr = 0;
                int Ctotcan10ltr = 0;
                foreach (DataRow drprev in dtprevinventoryDC.Select("ToTran='" + drroutebrnch["sno"].ToString() + "' OR FromTran='" + drroutebrnch["sno"].ToString() + "'"))
                {
                    if (drprev["TransType"].ToString() == "2")
                    {
                        if (drprev["invsno"].ToString() == "1")
                        {
                            int prevDcrates = 0;
                            int.TryParse(drprev["Qty"].ToString(), out prevDcrates);
                            prevDtotcrates += prevDcrates;
                        }
                        if (drprev["invsno"].ToString() == "2")
                        {
                            int prevDcan10ltr = 0;
                            int.TryParse(drprev["Qty"].ToString(), out prevDcan10ltr);
                            prevDtotcan10ltr += prevDcan10ltr;
                        }
                        if (drprev["invsno"].ToString() == "3")
                        {
                            int prevDcan20ltr = 0;
                            int.TryParse(drprev["Qty"].ToString(), out prevDcan20ltr);
                            prevDtotcan20ltr += prevDcan20ltr;
                        }
                        if (drprev["invsno"].ToString() == "4")
                        {
                            int prevDcan40ltr = 0;
                            int.TryParse(drprev["Qty"].ToString(), out prevDcan40ltr);
                            prevDtotcan40ltr += prevDcan40ltr;
                        }
                    }
                    if (drprev["TransType"].ToString() == "1" || drprev["TransType"].ToString() == "3")
                    {
                        if (drprev["invsno"].ToString() == "1")
                        {
                            int prevCcrates = 0;
                            int.TryParse(drprev["Qty"].ToString(), out prevCcrates);
                            prevCtotcrates += prevCcrates;
                        }
                        if (drprev["invsno"].ToString() == "2")
                        {
                            int prevCcan10ltr = 0;
                            int.TryParse(drprev["Qty"].ToString(), out prevCcan10ltr);
                            prevCtotcan10ltr += prevCcan10ltr;
                        }
                        if (drprev["invsno"].ToString() == "3")
                        {
                            int prevCcan20ltr = 0;
                            int.TryParse(drprev["Qty"].ToString(), out prevCcan20ltr);
                            prevCtotcan20ltr += prevCcan20ltr;
                        }
                        if (drprev["invsno"].ToString() == "4")
                        {
                            int prevCcan40ltr = 0;
                            int.TryParse(drprev["Qty"].ToString(), out prevCcan40ltr);
                            prevCtotcan40ltr += prevCcan40ltr;
                        }
                    }
                }
                foreach (DataRow dr in dtinventoryDC.Select("ToTran='" + drroutebrnch["sno"].ToString() + "' OR FromTran='" + drroutebrnch["sno"].ToString() + "'"))
                {
                    if (dr["TransType"].ToString() == "2")
                    {
                        if (dr["invsno"].ToString() == "1")
                        {
                            int Dcrates = 0;
                            int.TryParse(dr["Qty"].ToString(), out Dcrates);
                            Dtotcrates += Dcrates;
                        }
                        if (dr["invsno"].ToString() == "2")
                        {
                            int Dcan10ltr = 0;
                            int.TryParse(dr["Qty"].ToString(), out Dcan10ltr);
                            Dtotcan10ltr += Dcan10ltr;
                        }
                        if (dr["invsno"].ToString() == "3")
                        {
                            int Dcan20ltr = 0;
                            int.TryParse(dr["Qty"].ToString(), out Dcan20ltr);
                            Dtotcan20ltr += Dcan20ltr;
                        }
                        if (dr["invsno"].ToString() == "4")
                        {
                            int Dcan40ltr = 0;
                            int.TryParse(dr["Qty"].ToString(), out Dcan40ltr);
                            Dtotcan40ltr += Dcan40ltr;
                        }
                    }
                    if (dr["TransType"].ToString() == "1" || dr["TransType"].ToString() == "3")
                    {
                        if (dr["invsno"].ToString() == "1")
                        {
                            int Ccrates = 0;
                            int.TryParse(dr["Qty"].ToString(), out Ccrates);
                            Ctotcrates += Ccrates;
                        }
                        if (dr["invsno"].ToString() == "2")
                        {
                            int Ccan10ltr = 0;
                            int.TryParse(dr["Qty"].ToString(), out Ccan10ltr);
                            Ctotcan10ltr += Ccan10ltr;
                        }
                        if (dr["invsno"].ToString() == "3")
                        {
                            int Ccan20ltr = 0;
                            int.TryParse(dr["Qty"].ToString(), out Ccan20ltr);
                            Ctotcan20ltr += Ccan20ltr;
                        }
                        if (dr["invsno"].ToString() == "4")
                        {
                            int Ccan40ltr = 0;
                            int.TryParse(dr["Qty"].ToString(), out Ccan40ltr);
                            Ctotcan40ltr += Ccan40ltr;
                        }
                    }
                }
                oppcrates = oppcrates + prevCtotcrates - prevDtotcrates;
                oppcan10ltr = oppcan10ltr + prevCtotcan10ltr - prevDtotcan10ltr;
                oppcan20ltr = oppcan20ltr + prevCtotcan20ltr - prevDtotcan20ltr;
                oppcan40ltr = oppcan40ltr + prevCtotcan40ltr - prevDtotcan40ltr;
                int CratesClo = oppcrates + Dtotcrates - Ctotcrates;
                int Can10ltrClo = oppcan10ltr + Dtotcan10ltr - Ctotcan10ltr;
                int Can20ltrClo = oppcan20ltr + Dtotcan20ltr - Ctotcan20ltr;
                int Can40ltrClo = oppcan40ltr + Dtotcan40ltr - Ctotcan40ltr;

                drnew["Opp Crates"] = oppcrates;
                drnew["Issued Crates"] = Dtotcrates;
                drnew["Received Crates"] = Ctotcrates;
                //drnew["Difference Crates"] = Dtotcrates - Ctotcrates;
                drnew["Difference Crates"] = Ctotcrates - Dtotcrates;
                drnew["CB Crates"] = CratesClo;
                drnew["Opp Can40ltr"] = oppcan40ltr;
                drnew["Issued Can40ltr"] = Dtotcan40ltr;
                drnew["Received Can40ltr"] = Ctotcan40ltr;
                //drnew["Difference Can40ltr"] = Dtotcan40ltr - Ctotcan40ltr;
                drnew["Difference Can40ltr"] = Ctotcan40ltr - Dtotcan40ltr;
                drnew["CB Can40ltr"] = Can40ltrClo;
                drnew["Opp Can20ltr"] = oppcan20ltr;
                drnew["Issued Can20ltr"] = Dtotcan20ltr;
                drnew["Received Can20ltr"] = Ctotcan20ltr;
                //drnew["Difference Can20ltr"] = Dtotcan20ltr - Can20ltrClo;
                drnew["Difference Can20ltr"] = Ctotcan20ltr - Dtotcan20ltr;
                drnew["CB Can20ltr"] = Can20ltrClo;

                drnew["Opp Can10ltr"] = oppcan10ltr;
                drnew["Issued Can10ltr"] = Dtotcan10ltr;
                drnew["Received Can10ltr"] = Ctotcan10ltr;
                //drnew["Difference Can20ltr"] = Dtotcan20ltr - Can20ltrClo;
                drnew["Difference Can10ltr"] = Ctotcan10ltr - Dtotcan10ltr;
                drnew["CB Can10ltr"] = Can10ltrClo;
                Report.Rows.Add(drnew);
                i++;
            }

            DataRow Break = Report.NewRow();
            Break["Agent Name"] = "";
            Report.Rows.Add(Break);
            DataRow newvartical = Report.NewRow();
            newvartical["Agent Name"] = "Total";
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
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable("GridView_Data");
            int count = 1;
            foreach (TableCell cell in grdReports.HeaderRow.Cells)
            {
                if (count == 2)
                {
                    dt.Columns.Add(cell.Text);
                }
                else
                {
                    dt.Columns.Add(cell.Text).DataType = typeof(double);
                }
                count++;
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