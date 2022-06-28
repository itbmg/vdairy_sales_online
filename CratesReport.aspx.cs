using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Text;
using ClosedXML.Excel;
using System.Configuration;
using System.Globalization;
using System.Drawing;

public partial class CratesReport : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                FillSalesOffice();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            }
        }
    }
    void FillSalesOffice()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Plant")
            {
                PBranch.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchdata.flag = 1) AND (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
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
                PBranch.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata.flag = 1) AND (branchdata_1.SalesOfficeID = @SOID) OR (branchdata.sno = @BranchID)");
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
            if (Session["salestype"].ToString() == "Plant")
            {
                lblDispatchName.Text = ddlSalesOffice.SelectedItem.Text;
                lblDate.Text = txtdate.Text;
            }
            else
            {
                lblDispatchName.Text = "SALES OFFICE";
                lblDate.Text = txtdate.Text;
            }
            DateTime fromdate = DateTime.Now;
            pnlHide.Visible = true;
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
            Session["filename"] = ddlSalesOffice.SelectedItem.Text + " REPORT " + fromdate.AddDays(1).ToString("dd/MM/yyyy");
            string BranchID = ddlSalesOffice.SelectedValue;
            if (BranchID == "572")
            {
                BranchID = "7";
            }
            cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName, branchroutes.RouteName FROM branchdata INNER JOIN branchroutesubtable ON branchdata.sno = branchroutesubtable.BranchID INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno WHERE (branchmappingtable.SuperBranch = @bid) AND (branchdata.SalesType <> '21') AND (branchdata.flag = 1)");
            cmd.Parameters.AddWithValue("@bid", BranchID);
            DataTable dtbranch = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT inventory_monitor.BranchId, inventory_monitor.Inv_Sno, inventory_monitor.Qty, inventory_monitor.Sno, inventory_monitor.EmpId, inventory_monitor.lostQty FROM inventory_monitor INNER JOIN branchmappingtable ON inventory_monitor.BranchId = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @branchid)");
            cmd.Parameters.AddWithValue("@branchid", BranchID);
            DataTable dtinventoryopp = vdm.SelectQuery(cmd).Tables[0];
            Report = new DataTable();
            Report.Columns.Add("Sno");
            Report.Columns.Add("Route Name");
            Report.Columns.Add("Branch Code");
            Report.Columns.Add("Agent Name");
            Report.Columns.Add("Opp Crates", typeof(Double));
            Report.Columns.Add("Issued Crates", typeof(Double));
            Report.Columns.Add("Received Crates", typeof(Double));
            Report.Columns.Add("Difference Crates", typeof(Double));
            Report.Columns.Add("CB Crates", typeof(Double));
            Report.Columns.Add("E/S Crates", typeof(Double));

            Report.Columns.Add("Opp Can40ltr", typeof(Double));
            Report.Columns.Add("Issued Can40ltr", typeof(Double));
            Report.Columns.Add("Received Can40ltr", typeof(Double));
            Report.Columns.Add("Difference Can40ltr", typeof(Double));
            Report.Columns.Add("CB Can40ltr", typeof(Double));
            Report.Columns.Add("E/S Can40ltr", typeof(Double));

            Report.Columns.Add("Opp Can20ltr", typeof(Double));
            Report.Columns.Add("Issued Can20ltr", typeof(Double));
            Report.Columns.Add("Received Can20ltr", typeof(Double));
            Report.Columns.Add("Difference Can20ltr", typeof(Double));
            Report.Columns.Add("CB Can20ltr", typeof(Double));
            Report.Columns.Add("E/S Can20ltr", typeof(Double));

            Report.Columns.Add("Opp Can10ltr", typeof(Double));
            Report.Columns.Add("Issued Can10ltr", typeof(Double));
            Report.Columns.Add("Received Can10ltr", typeof(Double));
            Report.Columns.Add("Difference Can10ltr", typeof(Double));
            Report.Columns.Add("CB Can10ltr", typeof(Double));
            Report.Columns.Add("E/S Can10ltr", typeof(Double));
            int i = 1;
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);


            #region Queries

            cmd = new MySqlCommand("SELECT branchmappingtable.SubBranch, branchmappingtable.SuperBranch, invtras.TransType, invtras.FromTran, invtras.ToTran, invtras.Qty, invtras.DOE, invmaster.sno AS invsno, invmaster.InvName FROM branchmappingtable INNER JOIN (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty FROM invtransactions12 WHERE (DOE BETWEEN @d1 AND @d2)) invtras ON branchmappingtable.SubBranch = invtras.ToTran INNER JOIN invmaster ON invtras.B_inv_sno = invmaster.sno WHERE (branchmappingtable.SuperBranch = @branchid) ORDER BY branchmappingtable.SubBranch");
            cmd.Parameters.AddWithValue("@branchid", BranchID);
            DateTime dt1 = GetLowDate(fromdate.AddDays(-1));
            DateTime dt2 = GetLowDate(fromdate);
            cmd.Parameters.AddWithValue("@d1", dt1.AddHours(15));
            cmd.Parameters.AddWithValue("@d2", dt2.AddHours(15));
            DataTable dtinventoryD = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT branchmappingtable.SubBranch, branchmappingtable.SuperBranch, invtras.TransType, invtras.FromTran, invtras.ToTran, invtras.Qty, invtras.DOE, invmaster.sno AS invsno, invmaster.InvName FROM branchmappingtable INNER JOIN (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty FROM invtransactions12 WHERE (DOE BETWEEN @d1 AND @d2)) invtras ON branchmappingtable.SubBranch = invtras.FromTran INNER JOIN invmaster ON invtras.B_inv_sno = invmaster.sno WHERE (branchmappingtable.SuperBranch = @branchid) ORDER BY branchmappingtable.SubBranch");
            cmd.Parameters.AddWithValue("@branchid", BranchID);
            cmd.Parameters.AddWithValue("@d1", dt1.AddHours(15));
            cmd.Parameters.AddWithValue("@d2", dt2.AddHours(15));
            DataTable dtinventoryC = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT branchmappingtable.SubBranch, branchmappingtable.SuperBranch, invtras.TransType, invtras.FromTran, invtras.ToTran, invtras.Qty, invtras.DOE, invmaster.sno AS invsno, invmaster.InvName FROM branchmappingtable INNER JOIN (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty FROM invtransactions12 WHERE (DOE BETWEEN @d1 AND @d2)) invtras ON branchmappingtable.SubBranch = invtras.ToTran INNER JOIN invmaster ON invtras.B_inv_sno = invmaster.sno WHERE (branchmappingtable.SuperBranch = @branchid) ORDER BY branchmappingtable.SubBranch");
            cmd.Parameters.AddWithValue("@branchid", BranchID);
            DateTime dtmin = GetLowDate(fromdate.AddDays(-1));
            DateTime dtmax = GetLowDate(ServerDateCurrentdate);
            cmd.Parameters.AddWithValue("@d1", dtmin.AddHours(15));
            cmd.Parameters.AddWithValue("@d2", dtmax.AddHours(15));
            DataTable dtprevinventoryD = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT branchmappingtable.SubBranch, branchmappingtable.SuperBranch, invtras.TransType, invtras.FromTran, invtras.ToTran, invtras.Qty, invtras.DOE, invmaster.sno AS invsno, invmaster.InvName FROM branchmappingtable INNER JOIN (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty FROM invtransactions12 WHERE (DOE BETWEEN @d1 AND @d2)) invtras ON branchmappingtable.SubBranch = invtras.FromTran INNER JOIN invmaster ON invtras.B_inv_sno = invmaster.sno WHERE (branchmappingtable.SuperBranch = @branchid) ORDER BY branchmappingtable.SubBranch");
            cmd.Parameters.AddWithValue("@branchid", BranchID);
            cmd.Parameters.AddWithValue("@d1", dtmin.AddHours(15));
            cmd.Parameters.AddWithValue("@d2", dtmax.AddHours(15));
            DataTable dtprevinventoryC = vdm.SelectQuery(cmd).Tables[0];

            #endregion

            #region Transaction Type
            //            Transaction Type

            //branch to Branch -----1 delivery from so or collection from branch

            //Trip to Branch -----2   Delivery to branch

            //branch to Trip-----3   return to trip from branch

            //Trip to Trip -----4   dc or returndc
            //Audit Correction -----6

            #endregion


            int totcrates_Delivered = 0;
            int totcrates_Collected = 0;

            int totcans10_Delivered = 0;
            int totcans10_Collected = 0;

            int totcans20_Delivered = 0;
            int totcans20_Collected = 0;

            int totcans40_Delivered = 0;
            int totcans40_Collected = 0;
            int Totalcount = 1;
            string Routename = "";
            foreach (DataRow drbranch in dtbranch.Rows)
            {
                DataRow newRow = Report.NewRow();
                if (Routename != drbranch["RouteName"].ToString())
                {
                    if (Totalcount == 1)
                    {
                        newRow["Sno"] = i++.ToString();
                        newRow["Route Name"] = drbranch["RouteName"].ToString();
                        Totalcount++;
                    }
                    else
                    {
                        newRow["Sno"] = i++.ToString();
                        newRow["Route Name"] = drbranch["RouteName"].ToString();
                        DataRow space = Report.NewRow();
                        space["Agent Name"] = "";
                        Report.Rows.Add(space);
                    }
                }
                else
                {
                    newRow["Route Name"] = "";
                }
                Routename = drbranch["RouteName"].ToString();
                int oppcrates = 0;
                int oppcan20ltr = 0;
                int oppcan40ltr = 0;
                int oppcan10ltr = 0;
                //cmd = new MySqlCommand("SELECT BranchId, Inv_Sno, Qty, Sno, EmpId, lostQty FROM inventory_monitor WHERE (BranchId = @branchid)");
                //cmd.Parameters.AddWithValue("@branchid", drbranch["sno"].ToString());
                //DataTable dtinventoryopp = vdm.SelectQuery(cmd).Tables[0];

                foreach (DataRow dropp in dtinventoryopp.Select("BranchId='" + drbranch["sno"].ToString() + "'"))
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
                // cmd = new MySqlCommand("SELECT invtransactions12.TransType, invtransactions12.FromTran, invtransactions12.ToTran, invtransactions12.Qty, invtransactions12.DOE, invmaster.sno AS invsno,invmaster.InvName FROM invtransactions12 INNER JOIN invmaster ON invtransactions12.B_inv_sno = invmaster.sno WHERE (invtransactions12.ToTran = @branchid) AND (invtransactions12.DOE BETWEEN @d1 AND @d2) OR (invtransactions12.DOE BETWEEN @d1 AND @d2) AND (invtransactions12.FromTran = @branchid) ORDER BY invtransactions12.DOE");

                //DataRow newRow = Report.NewRow();
                //newRow["Sno"] = i;
                newRow["Branch Code"] = drbranch["sno"].ToString();
                newRow["Agent Name"] = drbranch["BranchName"].ToString();
                int Ctotcan40ltr = 0;
                int Ctotcrates = 0;
                int Ctotcan20ltr = 0;
                int Ctotcan10ltr = 0;
                int Dtotcrates = 0;
                int Dtotcan20ltr = 0;
                int Dtotcan10ltr = 0;
                int Dtotcan40ltr = 0;
                int prevCtotcan40ltr = 0;
                int prevCtotcrates = 0;
                int prevCtotcan20ltr = 0;
                int prevCtotcan10ltr = 0;
                int prevDtotcrates = 0;
                int prevDtotcan20ltr = 0;
                int prevDtotcan10ltr = 0;
                int prevDtotcan40ltr = 0;

                #region PrevInventory Delivery
                foreach (DataRow drprev in dtprevinventoryD.Select("SubBranch='" + drbranch["sno"].ToString() + "'"))
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
                    if (drprev["TransType"].ToString() == "1")
                    {
                        if (drprev["ToTran"].ToString() == drbranch["sno"].ToString())
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
                    }

                }
                #endregion
                #region Prev Inventory Collection
                foreach (DataRow drprev in dtprevinventoryC.Select("SubBranch='" + drbranch["sno"].ToString() + "'"))
                {
                    if (drprev["TransType"].ToString() == "1")
                    {
                        if (drprev["FromTran"].ToString() == drbranch["sno"].ToString())
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
                    if (drprev["TransType"].ToString() == "3")
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
                #endregion

                #region Current Date Inventory Delivery
                foreach (DataRow dr in dtinventoryD.Select("SubBranch='" + drbranch["sno"].ToString() + "'"))
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
                    if (dr["TransType"].ToString() == "1")
                    {
                        if (dr["ToTran"].ToString() == drbranch["sno"].ToString())
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
                    }
                }

                #endregion

                #region Current Date Inventory Collection
                foreach (DataRow dr in dtinventoryC.Select("SubBranch='" + drbranch["sno"].ToString() + "'"))
                {
                    if (dr["TransType"].ToString() == "1")
                    {
                        if (dr["FromTran"].ToString() == drbranch["sno"].ToString())
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
                    if (dr["TransType"].ToString() == "3")
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
                #endregion


                oppcrates = oppcrates + prevCtotcrates - prevDtotcrates;
                oppcan10ltr = oppcan10ltr + prevCtotcan10ltr - prevDtotcan10ltr;
                oppcan20ltr = oppcan20ltr + prevCtotcan20ltr - prevDtotcan20ltr;
                oppcan40ltr = oppcan40ltr + prevCtotcan40ltr - prevDtotcan40ltr;
                int CratesClo = oppcrates + Dtotcrates - Ctotcrates;
                int Can10ltrClo = oppcan10ltr + Dtotcan10ltr - Ctotcan10ltr;
                int Can20ltrClo = oppcan20ltr + Dtotcan20ltr - Ctotcan20ltr;
                int Can40ltrClo = oppcan40ltr + Dtotcan40ltr - Ctotcan40ltr;

                //int cratesopp=
                newRow["Opp Crates"] = oppcrates;
                newRow["Issued Crates"] = Dtotcrates;
                totcrates_Delivered += Dtotcrates;
                newRow["Received Crates"] = Ctotcrates;
                totcrates_Collected += Ctotcrates;
                newRow["Difference Crates"] = Dtotcrates - Ctotcrates;
                newRow["CB Crates"] = CratesClo;
                newRow["E/S Crates"] = CratesClo - Dtotcrates;

                newRow["Opp Can40ltr"] = oppcan40ltr;
                newRow["Issued Can40ltr"] = Dtotcan40ltr;
                totcans40_Delivered += Dtotcan40ltr;
                newRow["Received Can40ltr"] = Ctotcan40ltr;
                totcans40_Collected += Ctotcan40ltr;
                newRow["Difference Can40ltr"] = Dtotcan40ltr - Ctotcan40ltr;
                newRow["CB Can40ltr"] = Can40ltrClo;
                newRow["E/S Can40ltr"] = Can40ltrClo - Dtotcan40ltr;

                newRow["Opp Can20ltr"] = oppcan20ltr;
                newRow["Issued Can20ltr"] = Dtotcan20ltr;
                totcans20_Delivered += Dtotcan20ltr;
                newRow["Received Can20ltr"] = Ctotcan20ltr;
                totcans20_Collected += Ctotcan20ltr;
                newRow["Difference Can20ltr"] = Dtotcan20ltr - Can20ltrClo;
                newRow["CB Can20ltr"] = Can20ltrClo;
                newRow["E/S Can20ltr"] = Can20ltrClo - Dtotcan20ltr;

                newRow["Opp Can10ltr"] = oppcan10ltr;
                newRow["Issued Can10ltr"] = Dtotcan10ltr;
                totcans10_Delivered += Dtotcan10ltr;
                newRow["Received Can10ltr"] = Ctotcan10ltr;
                totcans10_Collected += Ctotcan10ltr;
                newRow["Difference Can10ltr"] = Dtotcan10ltr - Can10ltrClo;
                newRow["CB Can10ltr"] = Can10ltrClo;
                newRow["E/S Can10ltr"] = Can10ltrClo - Dtotcan10ltr;
                Totalcount++;
                Report.Rows.Add(newRow);
                i++;
            }



            DataRow brk = Report.NewRow();
            brk["Agent Name"] = "";
            Report.Rows.Add(brk);
            DataRow totalinventory = Report.NewRow();
            totalinventory["Agent Name"] = "TOTAL";
            double val = 0.0;
            foreach (DataColumn dc in Report.Columns)
            {
                if (dc.DataType == typeof(Double))
                {
                    val = 0.0;
                    double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                    totalinventory[dc.ToString()] = val;
                }
            }
            Report.Rows.Add(totalinventory);
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
            int count = 0;
            foreach (TableCell cell in grdReports.HeaderRow.Cells)
            {
                if (count == 1 || count == 3)
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