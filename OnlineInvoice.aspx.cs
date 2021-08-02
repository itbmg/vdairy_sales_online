using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class OnlineInvoice : System.Web.UI.Page
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
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                //txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                //FillAgentName();
                lblTitle.Text = Session["TitleName"].ToString();
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
     DataTable Report = new DataTable();
     void GetReport()
     {
         try
         {
             PanelHide.Visible = true;
             lblmsg.Text = "";
             vdm = new VehicleDBMgr();
             Report = new DataTable();
             //DateTime fromdate = DateTime.Now;
             //string[] fromdatestrig = txtFromdate.Text.Split(' ');
             //if (fromdatestrig.Length > 1)
             //{
             //    if (fromdatestrig[0].Split('-').Length > 0)
             //    {
             //        string[] dates = fromdatestrig[0].Split('-');
             //        string[] times = fromdatestrig[1].Split(':');
             //        fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
             //    }
             //}
             cmd = new MySqlCommand("SELECT productsdata.ProductName,bookingtransaction.MobNo,bookingtransaction.address, bookingtransaction.PersonName, bookingsubtable.Qty, bookingsubtable.Cost, bookingtransaction.DateOfDel, bookingtransaction.Transno FROM bookingsubtable INNER JOIN bookingtransaction ON bookingsubtable.TransNo = bookingtransaction.Transno INNER JOIN productsdata ON bookingsubtable.ProductID = productsdata.sno WHERE (bookingtransaction.Transno = @DcNo)");
             cmd.Parameters.AddWithValue("@DcNo", txtDcNo.Text);
             DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
             Report.Columns.Add("Sno");
             Report.Columns.Add("Product Name");
             Report.Columns.Add("Qty");
             Report.Columns.Add("Rate");
             Report.Columns.Add("Amount");
             double TotDelQty = 0;
             double TotAmount = 0;
             if (dtble.Rows.Count > 0)
             {
                 lblDcNo.Text = txtDcNo.Text;
                 lblAgent.Text = dtble.Rows[0]["PersonName"].ToString();
                 lbl_fromDate.Text = dtble.Rows[0]["DateOfDel"].ToString();
                 lblMobNo.Text = dtble.Rows[0]["MobNo"].ToString();
                 lblAddress.Text = dtble.Rows[0]["address"].ToString();
                 
                 int i = 1;
                 foreach (DataRow branch in dtble.Rows)
                 {
                     DataRow newrow = Report.NewRow();
                     string IndentDate = branch["DateOfDel"].ToString();
                     DateTime dtIndentDate = Convert.ToDateTime(IndentDate).AddDays(1);
                     string ChangedTime = dtIndentDate.ToString("dd/MMM/yyyy");
                     newrow["Sno"] = i++.ToString();
                     newrow["Product Name"] = branch["ProductName"].ToString();
                     double DeliveryQty = 0;
                     double.TryParse(branch["Qty"].ToString(), out DeliveryQty);
                     newrow["Qty"] = Math.Round(DeliveryQty, 2);
                     TotDelQty += DeliveryQty;
                     double UnitCost = 0;
                     double.TryParse(branch["Cost"].ToString(), out UnitCost);
                     newrow["Rate"] = branch["Cost"].ToString();
                     double Rate = DeliveryQty * UnitCost;
                     newrow["Amount"] = Math.Round(Rate, 2);
                     TotAmount += Rate;
                     Report.Rows.Add(newrow);
                 }
                 DataRow newvartical = Report.NewRow();
                 newvartical["Product Name"] = "Total";
                 newvartical["Qty"] = Math.Round(TotDelQty, 2);
                 newvartical["Amount"] = Math.Round(TotAmount, 2);
                 Report.Rows.Add(newvartical);
                 grdReports.DataSource = Report;
                 grdReports.DataBind();
             }
             else
             {
                 lblmsg.Text = "No Dc were found";
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