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
public partial class TallyLeaks : System.Web.UI.Page
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
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
                FillRouteName();
            }
        }


    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    void FillRouteName()
    {
        vdm = new VehicleDBMgr();
        if (Session["salestype"].ToString() == "Plant")
        {
            PBranch.Visible = true;
            DataTable dtBranch = new DataTable();
            dtBranch.Columns.Add("BranchName");
            dtBranch.Columns.Add("sno");
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
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
        }
        else
        {
            PBranch.Visible = true;
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.sno = @BranchID) AND (branchdata.SalesType IS NOT NULL)");
            cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            ddlSalesOffice.DataSource = dtRoutedata;
            ddlSalesOffice.DataTextField = "BranchName";
            ddlSalesOffice.DataValueField = "sno";
            ddlSalesOffice.DataBind();
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
            lblmsg.Text = "";
            pnlHide.Visible = true;
            Session["RouteName"] = ddlSalesOffice.SelectedItem.Text;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            string[] dateFromstrig = txtFromdate.Text.Split(' ');
            if (dateFromstrig.Length > 1)
            {
                if (dateFromstrig[0].Split('-').Length > 0)
                {
                    string[] dates = dateFromstrig[0].Split('-');
                    string[] times = dateFromstrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            Report.Columns.Add("Voucher Date");
            Report.Columns.Add("Voucher No");
            Report.Columns.Add("Item Name");
            Report.Columns.Add("Qty");
            Report.Columns.Add("Rate");
            Report.Columns.Add("Amount");
            Report.Columns.Add("Narration");
            lbl_selfromdate.Text = fromdate.ToString("dd/MM/yyyy");
            lblRoutName.Text = ddlSalesOffice.SelectedItem.Text;
            Session["xporttype"] = "TallyLekas";
            string ledger = "";
            DateTime ReportDate = fromdate;
            DateTime dtapril = new DateTime();
            DateTime dtmarch = new DateTime();
            int currentyear = ReportDate.Year;
            int nextyear = ReportDate.Year + 1;
            if (ReportDate.Month > 3)
            {
                string apr = "4/1/" + currentyear;
                dtapril = DateTime.Parse(apr);
                string march = "3/31/" + nextyear;
                dtmarch = DateTime.Parse(march);
            }
            if (ReportDate.Month <= 3)
            {
                string apr = "4/1/" + (currentyear - 1);
                dtapril = DateTime.Parse(apr);
                string march = "3/31/" + (nextyear - 1);
                dtmarch = DateTime.Parse(march);
            }
            string DCNO = "";
            cmd = new MySqlCommand("SELECT branchdata.sno,branchdata.Branchcode,branchdata.companycode,branchdata.incentivename,  branchdata.BranchName,branchdata.stateid, statemastar.statename, statemastar.statecode , statemastar.gststatecode FROM branchdata INNER JOIN statemastar ON branchdata.stateid = statemastar.sno WHERE (branchdata.sno = @BranchID)");

            //cmd = new MySqlCommand("SELECT sno, BranchName,branchcode, incentivename FROM branchdata WHERE (sno = @BranchID)");
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            }
            DataTable dtincetivename = vdm.SelectQuery(cmd).Tables[0];
            string statecode = "";
            string companycode = "";
            string gststatecode = "";
            if (dtincetivename.Rows.Count > 0)
            {
                statecode = dtincetivename.Rows[0]["statecode"].ToString();
                companycode = dtincetivename.Rows[0]["companycode"].ToString();
                gststatecode = dtincetivename.Rows[0]["gststatecode"].ToString();
            }
            string fromstateid = Session["statecode"].ToString();
            //DCNO = dtincetivename.Rows[0]["branchcode"].ToString() + dtapril.ToString("yy") + "" + dtmarch.ToString("yy") + "" + DCNO;
            Session["filename"] = ddlSalesOffice.SelectedItem.Text + " Tally Lekas" + fromdate.ToString("dd/MM/yyyy");
            cmd = new MySqlCommand("SELECT  dispatch.DispName, dispatch.sno, dispatch.BranchID, tripdata.I_Date,tripdata.dcno, tripdata.Sno AS TripSno, dispatch.DispMode, branchmappingtable.SuperBranch, triproutes.Tripdata_sno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN branchmappingtable ON dispatch.BranchID = branchmappingtable.SubBranch WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2)and (dispatch.DispType='SO') and (tripdata.Status<>'C') OR (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @SuperBranch)  and (dispatch.DispType='SO')and (tripdata.Status<>'C') GROUP BY tripdata.Sno ORDER BY dispatch.sno");
            cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtDispnames = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in dtDispnames.Rows)
            {
                cmd = new MySqlCommand("SELECT leakages.invoiceno, leakages.TotalLeaks, leakages.TripID, leakages.VLeaks, leakages.VReturns, leakages.ReturnQty, productsdata.tproduct,  branchproducts.unitprice, leakages.ProductID, leakages.FreeMilk, leakages.ShortQty, branchproducts.branch_sno FROM productsdata INNER JOIN leakages ON productsdata.sno = leakages.ProductID INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno WHERE (leakages.TripID = @tripID) AND (branchproducts.branch_sno = @BranchID)");
                cmd.Parameters.AddWithValue("@tripID", dr["TripSno"].ToString());
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtLeakble = vdm.SelectQuery(cmd).Tables[0];
                string tripID = "";
                if (dtLeakble.Rows.Count > 0)
                {
                    //Returns
                    if (ddl_type.SelectedValue == "Shorts & Returns & Leaks")
                    {
                        double totreturnamount = 0;
                        foreach (DataRow branch in dtLeakble.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            double ReturnQty = 0;
                            double.TryParse(branch["ReturnQty"].ToString(), out ReturnQty);
                            if (ReturnQty == 0.0)
                            {
                            }
                            else
                            {
                                double Rate = 0;
                                double.TryParse(branch["unitprice"].ToString(), out Rate);
                                newrow["Qty"] = ReturnQty;
                                newrow["Rate"] = 0;
                                double amount = 0;
                                amount = ReturnQty * Rate;
                                totreturnamount += amount;
                                tripID = dr["dcno"].ToString();
                                if (fromstateid == gststatecode)
                                {
                                    newrow["Voucher No"] = fromdate.ToString("dd") + "" + fromdate.ToString("MM") + "" + fromdate.ToString("yy");
                                }
                                else
                                {
                                    newrow["Voucher No"] = DCNO + "R" + dr["dcno"].ToString();
                                }
                                if (ddlSalesOffice.SelectedValue == "306")
                                {
                                    newrow["Voucher Date"] = fromdate.AddDays(1).ToString("dd-MMM-yyyy");
                                }
                                else
                                {
                                    newrow["Voucher Date"] = fromdate.ToString("dd-MMM-yyyy");
                                }
                                newrow["Item Name"] = branch["tproduct"].ToString();
                                newrow["Amount"] = 0;
                                newrow["Narration"] = "Being the Sale Of Milk Through " + ddlSalesOffice.SelectedItem.Text + ". This is Return Milk Vide JV No " + dr["dcno"].ToString() + ",Emp Name  " + Session["EmpName"].ToString();
                                Report.Rows.Add(newrow);
                            }
                        }
                        /// 19/12/2017
                        double totleakamount = 0;
                        foreach (DataRow branch in dtLeakble.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            double leakQty = 0;
                            double.TryParse(branch["TotalLeaks"].ToString(), out leakQty);
                            if (leakQty == 0.0)
                            {
                            }
                            else
                            {
                                double Rate = 0;
                                double.TryParse(branch["unitprice"].ToString(), out Rate);
                                newrow["Qty"] = leakQty;
                                newrow["Rate"] = 0;
                                double amount = 0;
                                amount = leakQty * Rate;
                                totreturnamount += amount;
                                tripID = dr["dcno"].ToString();

                                if (fromstateid == gststatecode)
                                {
                                    newrow["Voucher No"] = fromdate.ToString("dd") + "" + fromdate.ToString("MM") + "" + fromdate.ToString("yy");
                                }
                                else
                                {
                                    newrow["Voucher No"] = DCNO + "L" + dr["dcno"].ToString();
                                }

                                //newrow["Voucher No"] = DCNO + "L" + dr["dcno"].ToString();
                                if (ddlSalesOffice.SelectedValue == "306")
                                {
                                    newrow["Voucher Date"] = fromdate.AddDays(1).ToString("dd-MMM-yyyy");
                                }
                                else
                                {
                                    newrow["Voucher Date"] = fromdate.ToString("dd-MMM-yyyy");
                                }
                                newrow["Item Name"] = branch["tproduct"].ToString();
                                newrow["Amount"] = 0;
                                newrow["Narration"] = "Being the Sale Of Milk Through " + ddlSalesOffice.SelectedItem.Text + ". This is Lekage Milk Vide JV No " + dr["dcno"].ToString() + ",Emp Name  " + Session["EmpName"].ToString();
                                Report.Rows.Add(newrow);
                            }
                        }
                    }
                }
                cmd = new MySqlCommand("SELECT SUM(leakages.ShortQty) AS ShortQty, SUM(leakages.FreeMilk) AS FreeMilk, productsdata.tproduct, branchproducts.unitprice,leakages.invoiceno FROM leakages INNER JOIN tripdata ON leakages.TripID = tripdata.Sno INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno WHERE (tripdata.ATripid = @TripID) AND (branchproducts.branch_sno = @BranchID) GROUP BY productsdata.tproduct");
                cmd.Parameters.AddWithValue("@TripID", dr["TripSno"].ToString());
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                DataTable Dtfreemilk = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT branchleaktrans.ShortQty  AS ShortQty,branchleaktrans.FreeQty AS FreeMilk, productsdata.tproduct, branchproducts.unitprice,branchleaktrans.invoiceno FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN branchleaktrans ON productsdata.sno = branchleaktrans.ProdId WHERE (branchproducts.branch_sno = @BranchID) AND (branchleaktrans.TripId = @TripID)GROUP BY productsdata.tproduct");
                cmd.Parameters.AddWithValue("@TripID", dr["TripSno"].ToString());
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                DataTable dtsalesofficeshortfree = vdm.SelectQuery(cmd).Tables[0];
                DataTable newdt = new DataTable();
                newdt = Dtfreemilk.Copy();
                newdt.Merge(dtsalesofficeshortfree, true, MissingSchemaAction.Ignore);
                if (ddl_type.SelectedValue == "Shorts & Returns & Leaks")
                {
                    double totshortamount = 0;
                    foreach (DataRow branch in newdt.Rows)
                    {
                        //Shorts
                        DataRow newrow = Report.NewRow();
                        double ShortQty = 0;
                        double.TryParse(branch["ShortQty"].ToString(), out ShortQty);
                        if (ShortQty == 0.0)
                        {
                        }
                        else
                        {
                            double Rate = 0;
                            double.TryParse(branch["unitprice"].ToString(), out Rate);
                            ShortQty = Math.Round(ShortQty, 2);
                            newrow["Qty"] = ShortQty;
                            newrow["Rate"] = 0;
                            double amount = 0;
                            amount = ShortQty * Rate;
                            totshortamount += amount;
                            amount = Math.Round(amount, 2);
                            tripID = dr["dcno"].ToString();
                            // newrow["Voucher No"] = DCNO + "S" + dr["dcno"].ToString();

                            if (fromstateid == gststatecode)
                            {
                                newrow["Voucher No"] = fromdate.ToString("dd") + "" + fromdate.ToString("MM") + "" + fromdate.ToString("yy");
                            }
                            else
                            {
                                newrow["Voucher No"] = DCNO + "S" + dr["dcno"].ToString();
                            }

                            if (ddlSalesOffice.SelectedValue == "306")
                            {
                                newrow["Voucher Date"] = fromdate.AddDays(1).ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                newrow["Voucher Date"] = fromdate.ToString("dd-MMM-yyyy");
                            }
                            newrow["Item Name"] = branch["tproduct"].ToString();
                            newrow["Amount"] = 0;
                            newrow["Narration"] = "Being the Sale Of Milk Through " + ddlSalesOffice.SelectedItem.Text + ". This is Short Milk Vide JV No " + dr["dcno"].ToString() + ",Emp Name  " + Session["EmpName"].ToString();
                            Report.Rows.Add(newrow);
                        }
                    }
                }
            }
            grdReports.DataSource = Report;
            grdReports.DataBind();
            Session["xportdata"] = Report;
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
}