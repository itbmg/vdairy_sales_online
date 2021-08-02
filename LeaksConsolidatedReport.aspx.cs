using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
public partial class LeaksConsolidatedReport : System.Web.UI.Page
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
                lblTitle.Text = Session["TitleName"].ToString();
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) and (branchdata.flag=@flag) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) and (branchdata.flag=@flag)");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                cmd.Parameters.AddWithValue("@SalesType", "21");
                cmd.Parameters.AddWithValue("@SalesType1", "26");
                cmd.Parameters.AddWithValue("@flag", "1");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) and (branchdata.flag=@flag) OR (branchdata.sno = @BranchID) and (branchdata.flag=@flag)");
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                cmd.Parameters.AddWithValue("@flag", "1");
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
            DateTime fromdate = DateTime.Now;
            pnlHide.Visible = true;
            string[] datestrig = txtFromdate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            DateTime todate = DateTime.Now;
            string[] todatestrig = txtTodate.Text.Split(' ');
            if (todatestrig.Length > 1)
            {
                if (todatestrig[0].Split('-').Length > 0)
                {
                    string[] datess = todatestrig[0].Split('-');
                    string[] timess = todatestrig[1].Split(':');
                    todate = new DateTime(int.Parse(datess[2]), int.Parse(datess[1]), int.Parse(datess[0]), int.Parse(timess[0]), int.Parse(timess[1]), 0);
                }
            }
            Report = new DataTable();
            Report.Columns.Add("Sno");
            Report.Columns.Add("Name");
            Report.Columns.Add("Despatch Qty").DataType = typeof(Double);
            Report.Columns.Add("Leaks").DataType = typeof(Double);
            Report.Columns.Add("Leaks %");
            cmd = new MySqlCommand("SELECT empmanage.Branch, empmanage.Sno,empmanage.EmpName, empmanage.UserName, tripdata.Permissions, tripdata.BranchID FROM tripdata INNER JOIN empmanage ON tripdata.EmpId = empmanage.Sno INNER JOIN  branchdata ON tripdata.BranchID = branchdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (tripdata.BranchID = @BranchID) OR (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchdata.SalesOfficeID = @SalesOfficeID) GROUP BY empmanage.UserName, tripdata.BranchID");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@SalesOfficeID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable dtSalesman = vdm.SelectQuery(cmd).Tables[0];
            if (dtSalesman.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dtSalesman.Rows)
                {
                    cmd = new MySqlCommand("SELECT tripdat.Sno, tripdat.Status, tripsubdata.ProductId, SUM(tripsubdata.Qty) AS dispatchqty, productsdata.ProductName FROM triproutes INNER JOIN (SELECT Sno, Status, I_Date, BranchID, EmpId FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE (tripdat.EmpId = @dispid) AND (tripdat.Status <> 'C') GROUP BY tripdat.EmpId");
                    cmd.Parameters.AddWithValue("@dispid", dr["Sno"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                    DataTable dtdispqty = vdm.SelectQuery(cmd).Tables[0];
                    cmd = new MySqlCommand("SELECT ROUND(SUM(leakages.TotalLeaks), 2) AS LeakQty, productsdata.ProductName, dispatch.sno, dispatch.DispName, tripdata.EmpId, empmanage.EmpName,DATE_FORMAT(tripdata.I_Date, '%d %b %y') AS IndentDATE, branchprdt.Rank FROM tripdata INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN leakages ON tripdata.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN empmanage ON tripdata.EmpId = empmanage.Sno INNER JOIN (SELECT product_sno, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) branchprdt ON productsdata.sno = branchprdt.product_sno WHERE (leakages.VarifyStatus = 'V') AND (empmanage.sno = @EMPID) AND (tripdata.I_Date BETWEEN @d1 AND @d2)  GROUP BY tripdata.EmpId");
                    cmd.Parameters.AddWithValue("@EMPID", dr["Sno"].ToString());
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                    DataTable dtLeaks = vdm.SelectQuery(cmd).Tables[0];
                    double despqty = 0;
                    double leakqty = 0;
                    if (dtdispqty.Rows.Count > 0)
                    {
                        double.TryParse(dtdispqty.Rows[0]["dispatchqty"].ToString(), out despqty);
                        despqty = Math.Round(despqty, 2);
                    }
                    if (dtLeaks.Rows.Count > 0)
                    {
                        double.TryParse(dtLeaks.Rows[0]["LeakQty"].ToString(), out leakqty);
                        leakqty = Math.Round(leakqty, 2);
                    }
                    DataRow newrow = Report.NewRow();
                    newrow["Sno"] = i++.ToString();
                    newrow["Name"] = dr["EmpName"].ToString();
                    newrow["Despatch Qty"] = despqty;
                    newrow["Leaks"] = leakqty;
                    double per = 0;
                    per = (leakqty / despqty) * 100;
                    per = Math.Round(per, 2);
                    newrow["Leaks %"] = per;
                    Report.Rows.Add(newrow);
                }
            }
            DataRow totalinventory = Report.NewRow();
            totalinventory["Name"] = "TOTAL";
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
            grdReports.DataSource = Report;
            grdReports.DataBind();
        }
        catch
        {
        }
    }
}