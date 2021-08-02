using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
public partial class TripEnd : System.Web.UI.Page
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
        //vdm = new VehicleDBMgr();12-04-2014 00:00
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                lblTitle.Text = Session["TitleName"].ToString();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:MM");

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
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdReports.EditIndex = e.NewEditIndex;
        GetReport();
    }
    protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdReports.EditIndex = -1;
        GetReport();
    }
    protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

        vdm = new VehicleDBMgr();
        DateTime fromdate = DateTime.Now;
        string[] fromdatestrig = txtdate.Text.Split(' ');
        if (fromdatestrig.Length > 1)
        {
            if (fromdatestrig[0].Split('-').Length > 0)
            {
                string[] dates = fromdatestrig[0].Split('-');
                string[] times = fromdatestrig[1].Split(':');
                fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
            }
        }

        GridViewRow row = grdReports.Rows[e.RowIndex];
        string UserName = (row.FindControl("txtUserName") as TextBox).Text;
        string DispName = (row.FindControl("txtDispName") as TextBox).Text;
        string Sno = (row.FindControl("txtSno") as TextBox).Text;
        string Status = (row.FindControl("txtStatus") as TextBox).Text;
        string Canceldate = (row.FindControl("txtCanceldate") as TextBox).Text;
        string CollectedAmount = (row.FindControl("txtCollectedAmount") as TextBox).Text;
        string SubmittedAmount = (row.FindControl("txtSubmittedAmount") as TextBox).Text;
        string ReceivedAmount = (row.FindControl("txtReceivedAmount") as TextBox).Text;
        cmd = new MySqlCommand("SELECT Sno, EmpId, AssignDate, Status, Cdate, ReceivedAmount, RecieptNo FROM  tripdata WHERE (Sno = @tripid) AND (BranchID = @branchid)");
        cmd.Parameters.AddWithValue("@tripid", Sno);
        cmd.Parameters.AddWithValue("@branchid", Session["branch"].ToString());
        DataTable dtreceiptno = vdm.SelectQuery(cmd).Tables[0];
        DateTime Receiptdate = new DateTime();
        string RecieptNo = "";
        if (dtreceiptno.Rows.Count > 0)
        {
            string Tripenddate = dtreceiptno.Rows[0]["Cdate"].ToString();
             RecieptNo = dtreceiptno.Rows[0]["RecieptNo"].ToString();
             if (Tripenddate != "")
             {
                 Receiptdate = Convert.ToDateTime(Tripenddate);
             }
        }
        //int customerId = Convert.ToInt32(grdReports.DataKeys[e.RowIndex].Values[0]);
        cmd = new MySqlCommand("Update Tripdata set Status=@Status,SyncStatus=@SyncStatus,Cdate=@Cdate,ReceivedAmount=@ReceivedAmount where  Sno=@sno");
        cmd.Parameters.AddWithValue("@Status", Status);
        cmd.Parameters.AddWithValue("@Sno", Sno);
        cmd.Parameters.AddWithValue("@Cdate", fromdate);
        cmd.Parameters.AddWithValue("@SyncStatus", "1");
        cmd.Parameters.AddWithValue("@ReceivedAmount", ReceivedAmount);
        vdm.Update(cmd);
        cmd = new MySqlCommand("SELECT   Sno, BranchId, ReceivedFrom, AgentID, Empid, Amountpayable, AmountPaid, DOE, Create_by, Modified_by, Remarks, OppBal, dispatchid, Receipt, PaymentStatus, ChequeNo, Tripid, GroupRecieptNo, GroupRef,TransactionType, AmountDebited, newreceipt FROM cashreceipts WHERE (Receipt = @receipt) AND (BranchId = @branchid) AND (ReceivedFrom = @ReceivedFrom) AND (DOE BETWEEN @d1 AND @d2)");
        cmd.Parameters.AddWithValue("@receipt", RecieptNo);
        cmd.Parameters.AddWithValue("@branchid", Session["branch"].ToString());
        cmd.Parameters.AddWithValue("@ReceivedFrom", "SalesMen");
        cmd.Parameters.AddWithValue("@d1", GetLowDate(Receiptdate));
        cmd.Parameters.AddWithValue("@d2", GetHighDate(Receiptdate));
        DataTable dtreceiptdetails = vdm.SelectQuery(cmd).Tables[0];
        if (dtreceiptdetails.Rows.Count > 0)
        {
            string receiptsno = dtreceiptdetails.Rows[0]["Sno"].ToString();
            cmd = new MySqlCommand("Update cashreceipts set DOE=@DOE where  Sno=@sno");
            cmd.Parameters.AddWithValue("@DOE", fromdate);
            cmd.Parameters.AddWithValue("@sno", receiptsno);
            vdm.Update(cmd);
        }
        lblmsg.Text = "Assign Successfully";
        grdReports.EditIndex = -1;
        GetReport();
    }
    protected void GridViews_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            //vdm = new VehicleDBMgr();
            //DateTime fromdate = DateTime.Now;
            //string[] fromdatestrig = txtdate.Text.Split(' ');
            //if (fromdatestrig.Length > 1)
            //{
            //    if (fromdatestrig[0].Split('-').Length > 0)
            //    {
            //        string[] dates = fromdatestrig[0].Split('-');
            //        string[] times = fromdatestrig[1].Split(':');
            //        fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
            //    }
            //}
            ////int Sno = Convert.ToInt32(grdReports.DataKeys[e.RowIndex].Value.ToString());
            ////int Sno = Convert.ToInt32(grdReports.DataKeys[e.RowIndex].Value.ToString());
            //if (e.CommandName == "Comment")
            //{
            //    string[] arg = new string[2];
            //    arg = e.CommandArgument.ToString().Split(';');
            //    int Sno = int.Parse(arg[0]);
            //    string Status = ddlReportType.SelectedValue;
            //    //string Status = arg[1];
            //    //Session["IdEntity"] = arg[1];
            //    if (Status == "Assign")
            //    {
            //        Status = "A";
            //    }
            //    else if (Status == "Pending")
            //    {
            //        Status = "P";
            //    }
            //    else if (Status == "Verified")
            //    {
            //        Status = "V";
            //    }
            //    else
            //    {
            //        Status = "C";
            //    }
            //    //DateTime dtFromdate = Convert.ToDateTime(txtdate.Text);
            //    cmd = new MySqlCommand("Update Tripdata set Status=@Status,SyncStatus=@SyncStatus,Cdate=@Cdate where  Sno=@sno");
            //    cmd.Parameters.AddWithValue("@Status", Status);
            //    cmd.Parameters.AddWithValue("@Sno", Sno);
            //    cmd.Parameters.AddWithValue("@Cdate", fromdate);
            //    cmd.Parameters.AddWithValue("@SyncStatus", "1");
            //    vdm.Update(cmd);
            //    lblmsg.Text = "Assign Successfully";
            //    grdReports.EditIndex = -1;
            //   // GetReport();
            //}
        }
        catch
        {
        }
    }
    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            lblmsg.Text = "";
            DataTable Report = new DataTable();
            DateTime fromdate = DateTime.Now;
            string[] fromdatestrig = txtdate.Text.Split(' ');
            if (fromdatestrig.Length > 1)
            {
                if (fromdatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = fromdatestrig[0].Split('-');
                    string[] times = fromdatestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            //cmd = new MySqlCommand("SELECT empmanage.UserName, dispatch.DispName,tripdata.Sno , tripdata.Status, DATE_FORMAT(tripdata.Cdate,'%d %b %y') as Canceldate, tripdata.CollectedAmount, tripdata.SubmittedAmount FROM tripdata INNER JOIN empmanage ON tripdata.EmpId = empmanage.Sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) and (empmanage.Branch=@BranchID)");
            cmd = new MySqlCommand("SELECT empmanage.UserName, dispatch.DispName, tripdata.Sno, tripdata.Status, DATE_FORMAT(tripdata.Cdate, '%d %b %y') AS Canceldate, tripdata.CollectedAmount, tripdata.SubmittedAmount,tripdata.ReceivedAmount FROM tripdata INNER JOIN empmanage ON tripdata.EmpId = empmanage.Sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN branchdata ON empmanage.Branch = branchdata.sno INNER JOIN branchdata branchdata_1 ON empmanage.Branch = branchdata_1.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchdata.sno = @BranchID) OR (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchdata_1.SalesOfficeID = @BranchID) order by dispatch.DispName");  
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            DataTable dtTrip = vdm.SelectQuery(cmd).Tables[0];
            if (dtTrip.Rows.Count > 0)
            {
                grdReports.DataSource = dtTrip;
                grdReports.DataBind();
            }
            else
            {
                grdReports.DataSource = dtTrip;
                grdReports.DataBind();
                lblmsg.Text = "No data found";
            }
        }
        catch(Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}