using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class ReceiptBook : System.Web.UI.Page
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
                lblSignTitle.Text = Session["TitleName"].ToString();
                if (Session["ReceiptNo"] == null)
                {
                }
                else
                {
                    txtReceiptNo.Text = Session["ReceiptNo"].ToString();
                    txthiddentype.Value = Session["Type"].ToString();
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
            lblmsg.Text = "";
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
            cmd = new MySqlCommand("SELECT sno, BranchName, BranchCode FROM branchdata WHERE (sno = @BranchID)");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            DataTable dtCode = vdm.SelectQuery(cmd).Tables[0];
            string Receiptid = "";
            //cmd = new MySqlCommand("SELECT Sno, BranchId, ReceivedFrom, AgentID, Empid, Amountpayable, AmountPaid, DOE, Create_by, Modified_by, Remarks, OppBal, dispatchid, Receipt FROM cashreceipts WHERE (BranchId = @BranchID) AND (Receipt = @Receipt)");
            //cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            //cmd.Parameters.AddWithValue("@Receipt", txtReceiptNo.Text);
            //DataTable dtReceipt = vdm.SelectQuery(cmd).Tables[0];
            //if (dtReceipt.Rows.Count > 0)
            //{
                //string Status = dtReceipt.Rows[0]["ReceivedFrom"].ToString();
                string Status = txthiddentype.Value;
                if (Status == "SalesMen")
                {
                    cmd = new MySqlCommand("SELECT cashreceipts.Remarks,DATE_FORMAT(cashreceipts.DOE, '%d %b %y') AS DOE, cashreceipts.Sno AS RefNo, cashreceipts.Receipt, dispatch.DispName, empmanage.EmpName, cashreceipts.AmountPaid,cashreceipts.GroupRecieptNo FROM cashreceipts INNER JOIN dispatch ON cashreceipts.dispatchid = dispatch.sno INNER JOIN empmanage ON cashreceipts.Empid = empmanage.Sno WHERE  (cashreceipts.Sno = @Receipt)");
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                    cmd.Parameters.AddWithValue("@Receipt", txtReceiptNo.Text);
                    DataTable dtReceiptBook = vdm.SelectQuery(cmd).Tables[0];
                    if (dtReceiptBook.Rows.Count > 0)
                    {
                        //lblreceiptno.Text = txtReceiptNo.Text;
                        //lblreceiptno.Text = dtReceiptBook.Rows[0]["GroupRecieptNo"].ToString();
                        Receiptid = dtCode.Rows[0]["BranchCode"].ToString() + "/ROUTE_RCPT/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "/" + dtReceiptBook.Rows[0]["GroupRecieptNo"].ToString();
                        lblreceiptno.Text = Receiptid;

                        lblDate.Text = dtReceiptBook.Rows[0]["DOE"].ToString();
                        lblPayCash.Text = dtReceiptBook.Rows[0]["DispName"].ToString();
                        lbltowards.Text = "MILK";
                        lblAmount.Text = dtReceiptBook.Rows[0]["AmountPaid"].ToString();
                        lblCheque.Text = "Cash";
                        lblChequeDate.Text = dtReceiptBook.Rows[0]["DOE"].ToString();
                        lblRemarks.Text = dtReceiptBook.Rows[0]["Remarks"].ToString();
                    }
                }
                if (Status == "Agent" || Status == "Cash")
                {
                    //cmd = new MySqlCommand("SELECT cashreceipts.Remarks,cashreceipts.Sno, DATE_FORMAT(cashreceipts.DOE, '%d %b %y') AS DOE, cashreceipts.Sno AS RefNo,cashreceipts.Receipt,cashreceipts.PaymentStatus,cashreceipts.ChequeNo,  branchdata.BranchName, cashreceipts.AmountPaid FROM cashreceipts INNER JOIN branchdata ON cashreceipts.AgentID = branchdata.sno WHERE (cashreceipts.BranchId = @BranchID) AND (cashreceipts.Receipt = @Receipt)");
                    cmd = new MySqlCommand("SELECT cashreceipts.Remarks, cashreceipts.Sno, DATE_FORMAT(cashreceipts.DOE, '%d %b %y') AS DOE, cashreceipts.Sno AS RefNo, cashreceipts.Receipt,cashreceipts.PaymentStatus, cashreceipts.ChequeNo, branchdata.BranchName, cashreceipts.AmountPaid, branchroutes.RouteName FROM cashreceipts INNER JOIN branchdata ON cashreceipts.AgentID = branchdata.sno INNER JOIN branchroutesubtable ON branchdata.sno = branchroutesubtable.BranchID INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno WHERE  (cashreceipts.Sno = @Receipt)");
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                    cmd.Parameters.AddWithValue("@Receipt", txtReceiptNo.Text);
                    DataTable dtReceiptBook = vdm.SelectQuery(cmd).Tables[0];
                    if (dtReceiptBook.Rows.Count > 0)
                    {
                        //lblreceiptno.Text = txtReceiptNo.Text;
                        //lblreceiptno.Text = dtReceiptBook.Rows[0]["Receipt"].ToString();
                        Receiptid = dtCode.Rows[0]["BranchCode"].ToString() + "/AGENT_RCPT/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "/" + dtReceiptBook.Rows[0]["Receipt"].ToString();
                        lblreceiptno.Text = Receiptid;
                        lblDate.Text = dtReceiptBook.Rows[0]["DOE"].ToString();
                        string Branch=Session["branch"].ToString();
                       
                            lblPayCash.Text = dtReceiptBook.Rows[0]["BranchName"].ToString();
                        string PaymentStatus = dtReceiptBook.Rows[0]["PaymentStatus"].ToString();
                        if (PaymentStatus == "Cheque")
                        {
                            lblCheque.Text = dtReceiptBook.Rows[0]["ChequeNo"].ToString();
                        }
                        else
                        {
                            lblCheque.Text = "Cash";
                        }
                        lbltowards.Text = "MILK";
                        lblAmount.Text = dtReceiptBook.Rows[0]["AmountPaid"].ToString();
                        lblChequeDate.Text = dtReceiptBook.Rows[0]["DOE"].ToString();
                        lblRemarks.Text = dtReceiptBook.Rows[0]["Remarks"].ToString();
                    }
                }
                if (Status == "Others" || Status == "freezer deposit" || Status == "Journal Voucher" || Status == "Cheque" || Status == "Bank Transfer")
                {
                    cmd = new MySqlCommand("SELECT DATE_FORMAT(DOE, '%d %b %y') AS DOE, Receiptno, Name, Amount,remarks,CollectionType,PaymentType  FROM cashcollections WHERE (Sno = @Receipt)");
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                    cmd.Parameters.AddWithValue("@Receipt", txtReceiptNo.Text);
                    DataTable dtReceiptBook = vdm.SelectQuery(cmd).Tables[0];
                    if (dtReceiptBook.Rows.Count > 0)
                    {
                        //lblreceiptno.Text = txtReceiptNo.Text;
                        //lblreceiptno.Text = dtReceiptBook.Rows[0]["Receiptno"].ToString();
                        Receiptid = dtCode.Rows[0]["BranchCode"].ToString() + "/OTH_RCPT/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "/" + dtReceiptBook.Rows[0]["Receiptno"].ToString();
                        lblreceiptno.Text = Receiptid;
                        lblDate.Text = dtReceiptBook.Rows[0]["DOE"].ToString();
                        lblPayCash.Text = dtReceiptBook.Rows[0]["Name"].ToString();
                        //lbltowards.Text = "Others";
                        lbltowards.Text = dtReceiptBook.Rows[0]["PaymentType"].ToString();
                        lblAmount.Text = dtReceiptBook.Rows[0]["Amount"].ToString();
                        //lblCheque.Text = "Cash";
                        lblCheque.Text = dtReceiptBook.Rows[0]["CollectionType"].ToString();
                        lblChequeDate.Text = dtReceiptBook.Rows[0]["DOE"].ToString();
                        lblRemarks.Text = dtReceiptBook.Rows[0]["remarks"].ToString();
                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT branchdata.BranchName,collections.Remarks, DATE_FORMAT(collections.PaidDate, '%d %b %y') AS DOE, collections.PaymentType, collections.AmountPaid, collections.Sno, collections.ReceiptNo FROM collections INNER JOIN branchmappingtable ON collections.Branchid = branchmappingtable.SubBranch INNER JOIN branchdata ON collections.Branchid = branchdata.sno WHERE (collections.Sno=@ReceiptNo) ORDER BY branchdata.BranchName");
                        cmd.Parameters.AddWithValue("@ReceiptNo", txtReceiptNo.Text);
                        DataTable dtReceip= vdm.SelectQuery(cmd).Tables[0];
                        if (dtReceip.Rows.Count > 0)
                        {
                            //lblreceiptno.Text = txtReceiptNo.Text;
                            //lblreceiptno.Text = dtReceiptBook.Rows[0]["Receiptno"].ToString();
                            Receiptid = dtCode.Rows[0]["BranchCode"].ToString() + "/OTH_RCPT/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "/" + dtReceip.Rows[0]["Sno"].ToString();
                            lblreceiptno.Text = Receiptid;
                            lblDate.Text = dtReceip.Rows[0]["DOE"].ToString();
                            lblPayCash.Text = dtReceip.Rows[0]["BranchName"].ToString();
                            //lbltowards.Text = "Others";
                            lbltowards.Text = dtReceip.Rows[0]["PaymentType"].ToString();
                            lblAmount.Text = dtReceip.Rows[0]["AmountPaid"].ToString();
                            //lblCheque.Text = "Cash";
                            lblCheque.Text = dtReceip.Rows[0]["PaymentType"].ToString();
                            lblChequeDate.Text = dtReceip.Rows[0]["DOE"].ToString();
                            lblRemarks.Text = dtReceip.Rows[0]["Remarks"].ToString();
                        }
                    }
                }
                string Amont = lblAmount.Text;
                string[] Ones = { "","One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Ninteen" };

                string[] Tens = { "Ten", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninty" };

                int Num = int.Parse(Amont);
                //string InWords = "";
                //if (Num >= 1 && Num < 20)
                //    InWords += Below20[Num];
                //if (Num >= 20 && Num <= 99)
                //    InWords += Below100[Num / 10] + Below20[Num % 10];
                //if (Num >= 100 && Num <= 999)
                //    InWords += NumToWordBD(Num / 100) + " Hundred " + NumToWordBD(Num % 100);
                //if (Num >= 1000 && Num <= 99999)
                //    InWords += NumToWordBD(Num / 1000) + " Thousand " + NumToWordBD(Num % 1000);
                //if (Num >= 100000 && Num <= 9999999)
                //    InWords += NumToWordBD(Num / 100000) + " Lac " + NumToWordBD(Num % 100000);
                //if (Num >= 10000000)
                //    InWords += NumToWordBD(Num / 10000000) + " Crore " + NumToWordBD(Num % 10000000);
                ////return InWords;
               // NumToWordBD(Num);
                lblRupess.Text = NumToWordBD(Num) +" Rupees Only";
            //}
            //else
            //{
            //    lblmsg.Text = "No Receipt were found";
            //}
        }
        catch(Exception ex) 
        {
            lblmsg.Text = ex.Message;
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
    }
}