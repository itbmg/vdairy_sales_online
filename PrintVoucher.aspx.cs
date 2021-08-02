using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class PrintVoucher : System.Web.UI.Page
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
                lblTitle.Text = Session["TitleName"].ToString();
                if (Session["VoucherID"] == null)
                {
                }
                else
                {
                    txtVoucherNo.Text = Session["VoucherID"].ToString();
                }
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
    void getdet()
    {
        try
        {
            vdm = new VehicleDBMgr();
            string BrachSOID = "";
            if (Session["BrachSOID"] == null)
            {
                BrachSOID = Session["branch"].ToString();
            }
            else
            {
                BrachSOID = Session["BrachSOID"].ToString();
            }
           // cmd = new MySqlCommand("SELECT cashpayables.onNameof, cashpayables.DOE, cashpayables.Sno, cashpayables.Amount, cashpayables.Remarks,cashpayables.ApprovalRemarks, cashpayables.VoucherType, empmanage.EmpName FROM cashpayables INNER JOIN empmanage ON cashpayables.Approvedby = empmanage.Sno WHERE (cashpayables.VocherID = @VocherID) AND (cashpayables.BranchID = @BranchID)");
            cmd = new MySqlCommand("SELECT cashpayables.onNameof, cashpayables.DOE,cashpayables.VocherID, cashpayables.Sno, cashpayables.Amount, cashpayables.ApprovedAmount, cashpayables.Remarks, cashpayables.ApprovalRemarks,cashpayables.VoucherType, empmanage.EmpName FROM cashpayables LEFT OUTER JOIN empmanage ON cashpayables.Approvedby = empmanage.Sno WHERE (cashpayables.Sno = @VocherID) AND (cashpayables.BranchID = @BranchID)");
           // cmd = new MySqlCommand("SELECT cashpayables.onNameof, cashpayables.DOE,cashpayables.VocherID, cashpayables.Sno, cashpayables.Amount, cashpayables.ApprovedAmount, cashpayables.Remarks, cashpayables.ApprovalRemarks,cashpayables.VoucherType, empmanage.EmpName FROM cashpayables LEFT OUTER JOIN empmanage ON cashpayables.Approvedby = empmanage.Sno WHERE (cashpayables.Sno = @VocherID) "); 
            cmd.Parameters.AddWithValue("@VocherID", txtVoucherNo.Text);
            cmd.Parameters.AddWithValue("@BranchID", BrachSOID);
            DataTable dtCash = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT sno, BranchName, BranchCode FROM branchdata WHERE (sno = @BranchID)");
            cmd.Parameters.AddWithValue("@BranchID", BrachSOID);
            DataTable dtCode = vdm.SelectQuery(cmd).Tables[0];
            string voucherid = "";
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            DateTime dtapril = new DateTime();
            DateTime dtmarch = new DateTime();
            int currentyear = ServerDateCurrentdate.Year;
            int nextyear = ServerDateCurrentdate.Year + 1;
            int currntyearnum = 0;
            int nextyearnum = 0;
            if (ServerDateCurrentdate.Month > 3)
            {
                string apr = "4/1/" + currentyear;
                dtapril = DateTime.Parse(apr);
                string march = "3/31/" + nextyear;
                dtmarch = DateTime.Parse(march);
                currntyearnum = currentyear;
                nextyearnum = nextyear;
            }
            if (ServerDateCurrentdate.Month <= 3)
            {
                string apr = "4/1/" + (currentyear - 1);
                dtapril = DateTime.Parse(apr);
                string march = "3/31/" + (nextyear - 1);
                dtmarch = DateTime.Parse(march);
                currntyearnum = currentyear - 1;
                nextyearnum = nextyear - 1;
            }
            if (dtCash.Rows.Count > 0)
            {
                //lblVoucherno.Text = txtVoucherNo.Text;
                voucherid = dtCode.Rows[0]["BranchCode"].ToString() + "/VOC/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "/" + dtCash.Rows[0]["VocherID"].ToString();
                lblVoucherno.Text = voucherid;
                string DOE = dtCash.Rows[0]["DOE"].ToString();
                DateTime dtDOE = Convert.ToDateTime(DOE);
                    lblApprove.Text = "Authorised By";
               
                string ChangedTime = dtDOE.ToString("dd/MMM/yyyy");
                lblDate.Text = ChangedTime;
                lblPayCash.Text = dtCash.Rows[0]["onNameof"].ToString();
                string AppRemarks = dtCash.Rows[0]["ApprovalRemarks"].ToString();
                if (AppRemarks == "")
                {
                    lblTowards.Text = dtCash.Rows[0]["Remarks"].ToString();
                }
                else
                {
                    lblTowards.Text = dtCash.Rows[0]["ApprovalRemarks"].ToString();
                }
                lblVoucherType.Text = dtCash.Rows[0]["VoucherType"].ToString().ToUpper() + " VOUCHER";
                string Amont = dtCash.Rows[0]["Amount"].ToString();
                string[] Ones = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Ninteen" };

                string[] Tens = { "Ten", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninty" };

                int Num = int.Parse(Amont);

                lblReceived.Text = NumToWordBD(Num) + " Rupees Only";
            }

        }
        catch(Exception ex)
        {
            lblReceived.Text = ex.Message;
        }
    }
    public static string NumToWordBD(Int64 Num)
    {
        string[] Below20 = { "", "One ", "Two ", "Three ", "Four ", 
      "Five ", "Six " , "Seven ", "Eight ", "Nine ", "Ten ", "Eleven ", 
    "Twelve " , "Thirteen ", "Fourteen ","Fifteen ", 
      "Sixteen " , "Seventeen ","Eighteen " , "Nineteen " };
        string[] Below100 = { "", "", "Twenty ", "Thirty ", 
      "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
        string InWords = "";
        if (Num >= 1 && Num < 20)
            InWords += Below20[Num];
        if (Num >= 20 && Num <= 99)
            InWords += Below100[Num / 10] + Below20[Num % 10];
        if (Num >= 100 && Num <= 999)
            InWords += NumToWordBD(Num / 100) + " Hundred " + NumToWordBD(Num % 100);
        if (Num >= 1000 && Num <= 99999)
            InWords += NumToWordBD(Num / 1000) + " Thousand " + NumToWordBD(Num % 1000);
        if (Num >= 100000 && Num <= 9999999)
            InWords += NumToWordBD(Num / 100000) + " Lakh " + NumToWordBD(Num % 100000);
        if (Num >= 10000000)
            InWords += NumToWordBD(Num / 10000000) + " Crore " + NumToWordBD(Num % 10000000);
        return InWords;
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        getdet();
        ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "PopupOpen(" + txtVoucherNo.Text + ");", true);

    }
}