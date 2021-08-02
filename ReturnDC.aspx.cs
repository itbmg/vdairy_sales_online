using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class ReturnDC : System.Web.UI.Page
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
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txttodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
                //getdet();
            }
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
    DataTable dttripdata = new DataTable();
    protected void btn_getdetails_Click(object sender, EventArgs e)
    {
        try
        {
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
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
            string[] todatestrig = txttodate.Text.Split(' ');
            if (todatestrig.Length > 1)
            {
                if (todatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = todatestrig[0].Split('-');
                    string[] times = todatestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            cmd = new MySqlCommand("SELECT dispatch.DispName,tripdata.AssignDate, tripdata.Sno AS ReturnDCno, empmanage.UserName, tripdata.VehicleNo FROM triproutes INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN empmanage ON tripdata.EmpId = empmanage.Sno INNER JOIN branchdata ON dispatch.BranchID = branchdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND ((dispatch.BranchID = @BranchID) OR (branchdata.SalesOfficeID = @SOID))");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
             dttripdata = vdm.SelectQuery(cmd).Tables[0];
            if (dttripdata.Rows.Count == 0)
            {
                lbldateValidation.Text = "No DC Found Between These Dates ";
            }
            else
            {
                Gridtripdata.DataSource = dttripdata;
                Gridtripdata.DataBind();
            }
        }
        catch(Exception ex)
        {
            lbldateValidation.Text = ex.Message;
            Gridtripdata.DataSource = dttripdata;
            Gridtripdata.DataBind();
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReturnDC();
    }
    string assigndate = "";
    string vehicleno = "";
    string DispatchName = "";
    string Dispatchsno = "";
    string Employee = "";
    string branchsno = "";
    string dcno = "";
    DataTable dtTotQty = new DataTable();
    void GetReturnDC()
    {
        try
        {
            vdm = new VehicleDBMgr();
            pnlHide.Visible = true;
            DateTime fromdate = DateTime.Now;
            string TripId = txt_tripid.Text;
            string BranchID = "";
            cmd = new MySqlCommand("SELECT tripdata.AssignDate,tripdata.ReturnDCTime,tripdata.Sno, tripdata.VehicleNo,dispatch.BranchID, dispatch.DispName AS DispatchName,dispatch.sno as dispsno, empmanage.EmpName AS Employee FROM tripdata INNER JOIN empmanage ON tripdata.EmpId = empmanage.Sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno WHERE (tripdata.Sno = @tripsno)");
            cmd.Parameters.AddWithValue("@tripsno", TripId);
            DataTable dtdetails = vdm.SelectQuery(cmd).Tables[0];
            if (dtdetails.Rows.Count > 0)
            {
                foreach (DataRow dr in dtdetails.Rows)
                {
                    assigndate = dr["ReturnDCTime"].ToString();
                    vehicleno = dr["VehicleNo"].ToString();
                    DispatchName = dr["DispatchName"].ToString();
                    dcno = dr["Sno"].ToString();
                    Dispatchsno = dr["dispsno"].ToString();
                    Employee = dr["Employee"].ToString();
                    BranchID = dr["BranchID"].ToString();
                }
                lblvehicleno.Text = vehicleno;
                lblroutename.Text = DispatchName;
                lbldcno.Text = dcno;
                DateTime dtassigndate = Convert.ToDateTime(assigndate);
                string date = dtassigndate.ToString("dd/MMM/yyyy");
                string strassigndate = dtassigndate.ToString();
                string[] strTime = strassigndate.Split(' ');
                lblassigndate.Text = date;
                lbldisptime.Text = strTime[1];
                lblpartyname.Text = Employee;
            }
            cmd = new MySqlCommand("SELECT leakages.TotalLeaks, leakages.ReturnQty, productsdata.ProductName, leakages.TripID FROM leakages INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (leakages.TripID = @TripID) order by productsdata.Rank");
            cmd.Parameters.AddWithValue("@TripID", TripId);
            DataTable dtIndent = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT invmaster.InvName, invtransactions12.Qty FROM invtransactions12 INNER JOIN invmaster ON invtransactions12.B_inv_sno = invmaster.sno WHERE (invtransactions12.ToTran  = @TripID) and(invtransactions12.FromTran =@BranchID) ");
            cmd.Parameters.AddWithValue("@TripID", TripId);
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            DataTable dtInventory = vdm.SelectQuery(cmd).Tables[0];
            dtTotQty.Columns.Add("Sl No");
            dtTotQty.Columns.Add("Product Name");
            dtTotQty.Columns.Add("Leak Qty").DataType = typeof(Double);
            dtTotQty.Columns.Add("Return Qty").DataType = typeof(Double);
            int i = 1;
            if (dtIndent.Rows.Count > 0)
            {
                foreach (DataRow dr in dtIndent.Rows)
                {
                    DataRow newrow = dtTotQty.NewRow();
                    newrow["Sl No"] = i++.ToString();
                    newrow["Product Name"] = dr["ProductName"].ToString();
                    newrow["Leak Qty"] = dr["TotalLeaks"].ToString();
                    newrow["Return Qty"] = dr["ReturnQty"].ToString();
                    dtTotQty.Rows.Add(newrow);
                }
            }
            DataRow newvartical = dtTotQty.NewRow();
            newvartical["Product Name"] = "Total";
            double val = 0.0;
            foreach (DataColumn dc in dtTotQty.Columns)
            {
                if (dc.DataType == typeof(Double))
                {
                    val = 0.0;
                    double.TryParse(dtTotQty.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                    newvartical[dc.ToString()] = val;
                }
            }
            dtTotQty.Rows.Add(newvartical);
            DataRow newempty = dtTotQty.NewRow();
            newempty["Product Name"] = "";
            dtTotQty.Rows.Add(newempty);
            DataRow newInventory = dtTotQty.NewRow();
            newInventory["Sl No"] = "Inventory";
            newInventory["Product Name"] = "Qty";
            dtTotQty.Rows.Add(newInventory);
            foreach (DataRow dr in dtInventory.Rows)
            {
                DataRow drnew = dtTotQty.NewRow();
                drnew["Sl No"] = dr["InvName"].ToString();
                drnew["Product Name"] = dr["Qty"].ToString();
                dtTotQty.Rows.Add(drnew);
            }
            grdReports.DataSource = dtTotQty;
            grdReports.DataBind();
            Session["xportdata"] = dtTotQty;
        }
        catch(Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdReports.DataSource = dtTotQty;
            grdReports.DataBind();
        }
    }
}