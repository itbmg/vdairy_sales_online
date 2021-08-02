using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class creditvoucherdetails : System.Web.UI.Page
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
                FillSalesOffice();
                lblTitle.Text = Session["TitleName"].ToString();
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
                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("BranchName");
                dtBranch.Columns.Add("sno");
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType)  ");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                cmd.Parameters.AddWithValue("@SalesType", "21");
                cmd.Parameters.AddWithValue("@SalesType1", "26");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtRoutedata.Rows)
                {
                    DataRow newrow = dtBranch.NewRow();
                    newrow["BranchName"] = dr["BranchName"].ToString();
                    newrow["sno"] = dr["sno"].ToString();
                    dtBranch.Rows.Add(newrow);
                }
                cmd = new MySqlCommand("SELECT BranchName, sno FROM  branchdata WHERE (sno = @BranchID)");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtPlant.Rows)
                {
                    DataRow newrow = dtBranch.NewRow();
                    newrow["BranchName"] = dr["BranchName"].ToString();
                    newrow["sno"] = dr["sno"].ToString();
                    dtBranch.Rows.Add(newrow);
                }
                ddlSalesOffice.DataSource = dtBranch;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                cmd = new MySqlCommand("SELECT BranchName, sno FROM branchdata WHERE (sno = @BranchID)");
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch
        {
        }
    }
    protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand("SELECT accountheads.HeadName, accountheads.Sno FROM subpayable INNER JOIN accountheads ON subpayable.HeadSno = accountheads.Sno INNER JOIN cashpayables ON subpayable.RefNo = cashpayables.Sno WHERE (cashpayables.BranchID = @BranchID) AND (cashpayables.VoucherType = 'Due') GROUP BY accountheads.HeadName ORDER BY accountheads.HeadName");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        DataTable dtHead = vdm.SelectQuery(cmd).Tables[0];
        ddlHeadOfaccounts.DataSource = dtHead;
        ddlHeadOfaccounts.DataTextField = "HeadName";
        ddlHeadOfaccounts.DataValueField = "Sno";
        ddlHeadOfaccounts.DataBind();
        ddlHeadOfaccounts.Items.Insert(0, new ListItem("Select", "0"));
    }
    //void FillAccountNames()
    //{
    //    try
    //    {
    //        vdm = new VehicleDBMgr();
    //        cmd = new MySqlCommand("SELECT accountheads.HeadName, accountheads.Sno FROM subpayable INNER JOIN accountheads ON subpayable.HeadSno = accountheads.Sno INNER JOIN cashpayables ON subpayable.RefNo = cashpayables.Sno WHERE (cashpayables.BranchID = @BranchID) AND (cashpayables.VoucherType = 'Due') GROUP BY accountheads.HeadName ORDER BY accountheads.HeadName");
    //        cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
    //        DataTable dtHead = vdm.SelectQuery(cmd).Tables[0];
    //        ddlHeadOfaccounts.DataSource = dtHead;
    //        ddlHeadOfaccounts.DataTextField = "HeadName";
    //        ddlHeadOfaccounts.DataValueField = "Sno";
    //        ddlHeadOfaccounts.DataBind();
    //        ddlHeadOfaccounts.Items.Insert(0, new ListItem("Select", "0"));
    //    }
    //    catch
    //    {
    //    }
    //}
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            pnlHide.Visible = true;
            DataTable Report = new DataTable();
            cmd = new MySqlCommand("SELECT DATE_FORMAT(cashpayables.DOE, '%d %b %y') AS EntryDate, cashpayables.VocherID, accountheads.HeadName,cashpayables.VoucherType, cashpayables.Amount, cashpayables.ApprovedAmount as ApprovedAmount FROM accountheads INNER JOIN subpayable ON accountheads.Sno = subpayable.HeadSno INNER JOIN cashpayables ON subpayable.RefNo = cashpayables.Sno WHERE (cashpayables.BranchID = @BranchID)  AND (subpayable.HeadSno = @HeadSno) and (cashpayables.Status=@Status) AND (cashpayables.VoucherType<>'Debit')  ORDER BY cashpayables.DOE");
            cmd.Parameters.AddWithValue("@Status", 'P');
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@HeadSno", ddlHeadOfaccounts.SelectedValue);
            DataTable dtCredit = vdm.SelectQuery(cmd).Tables[0];
            Report.Columns.Add("Date");
            Report.Columns.Add("VoucherID");
            Report.Columns.Add("AccountName");
            Report.Columns.Add("IOUAmount").DataType = typeof(Double);
            Report.Columns.Add("CreditAmount").DataType = typeof(Double);
            double DueAmount = 0;
            double CreditAmount = 0;
            if (dtCredit.Rows.Count > 0)
            {
                foreach (DataRow dr in dtCredit.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["Date"] = dr["EntryDate"].ToString();
                    newrow["VoucherID"] = dr["VocherID"].ToString();
                    newrow["AccountName"] = dr["HeadName"].ToString();
                    string VoucherType = dr["VoucherType"].ToString();
                    if(VoucherType=="Due")
                    {
                        double ReceivedAmount = 0;
                        double.TryParse(dr["Amount"].ToString(), out ReceivedAmount);
                        newrow["IOUAmount"] = ReceivedAmount;
                        DueAmount += ReceivedAmount;
                    }
                    if (VoucherType == "Credit")
                    {
                        double ApprovedAmount = 0;
                        double.TryParse(dr["ApprovedAmount"].ToString(), out ApprovedAmount);
                        newrow["CreditAmount"] = ApprovedAmount;
                        CreditAmount += ApprovedAmount;
                    }
                    Report.Rows.Add(newrow);
                }
            }
            double Amount = 0;
            Amount = DueAmount - CreditAmount;
            lblmsg.Text = "Due Amount: " + Amount.ToString();
            grdReports.DataSource = Report;
            grdReports.DataBind();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}