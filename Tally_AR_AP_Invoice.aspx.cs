using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class Tally_AR_AP_Invoice : System.Web.UI.Page
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
        //UserName = Session["field1"].ToString();
        //vdm = new VehicleDBMgr();
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
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType)  ");
            cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
            cmd.Parameters.AddWithValue("@SalesType", "23");
            DataTable dtNewPlant = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in dtNewPlant.Rows)
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
            //ddlSalesOffice.Items.Insert(0, new ListItem("Cash Sale", "0"));
            //ddlSalesOffice.Items.Insert(0, new ListItem("Staff Sale", "1"));
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
            //ddlSalesOffice.Items.Insert(0, new ListItem("Cash Sale", "0"));
            //ddlSalesOffice.Items.Insert(0, new ListItem("Staff Sale", "1"));
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

            lbl_selfromdate.Text = fromdate.ToString("dd/MM/yyyy");
            lblRoutName.Text = ddlSalesOffice.SelectedItem.Text;
            Session["xporttype"] = "TallyInward";
            Session["filename"] = ddlSalesOffice.SelectedItem.Text + " Tally Inward" + fromdate.ToString("dd/MM/yyyy");

            cmd = new MySqlCommand("SELECT branchdata.sno,branchdata.branchcode,branchdata.stateid,  branchdata.TbranchName, statemastar.statename ,statemastar.statecode FROM branchdata INNER JOIN statemastar ON branchdata.stateid = statemastar.sno WHERE (branchdata.sno = @BranchID)");
            cmd.Parameters.AddWithValue("@Branchid", ddlSalesOffice.SelectedValue);
            DataTable dtbranch = vdm.SelectQuery(cmd).Tables[0];
            string to_statecode = "";
            string to_branchcode = "";
            string to_branchName = "";
            string fromstateid = "";
            if (dtbranch.Rows.Count > 0)
            {
                to_statecode = dtbranch.Rows[0]["statecode"].ToString();
                to_branchcode = dtbranch.Rows[0]["branchcode"].ToString();
                to_branchName = dtbranch.Rows[0]["TbranchName"].ToString();
                fromstateid = dtbranch.Rows[0]["stateid"].ToString();
            }
            cmd = new MySqlCommand("SELECT branchdata.sno,branchdata.branchcode,branchdata.stateid,  branchdata.TbranchName, statemastar.statename ,statemastar.statecode FROM branchdata INNER JOIN statemastar ON branchdata.stateid = statemastar.sno WHERE (branchdata.sno = @BranchID)");
            cmd.Parameters.AddWithValue("@Branchid", Session["branch"]);
            DataTable dt_frombranch = vdm.SelectQuery(cmd).Tables[0];
            string from_statecode = "";
            string from_branchName = "";
            string from_branchCode = "";
            if (dt_frombranch.Rows.Count > 0)
            {
                from_statecode = dt_frombranch.Rows[0]["statecode"].ToString();
                from_branchName = dt_frombranch.Rows[0]["TbranchName"].ToString();
                from_branchCode = dt_frombranch.Rows[0]["branchcode"].ToString();
            }
            DateTime ReportDate = VehicleDBMgr.GetTime(vdm.conn);
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
            string tostate = Session["stateid"].ToString();
            if (ddlsalestype.SelectedValue == "NonTax")
            {
                cmd = new MySqlCommand("SELECT productsdata.hsncode, productsdata.igst, productsdata.cgst,products_category.tcategory,products_category.sno as catsno, productsdata.sgst, productsdata.tproduct, ROUND(SUM(tripsubdata.Qty), 2) AS qty,tripdata.dcno,tripdata.taxdcno,tripdata.taxdcno, tripdata.Sno, tripdata.I_Date, empmanage.UserName, branchproducts.unitprice, products_category.Categoryname FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN empmanage ON tripdata.EmpId = empmanage.Sno INNER JOIN branchproducts ON empmanage.Branch = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE  (tripdata.I_Date BETWEEN @d1 AND @d2) AND (empmanage.Branch = @BranchID) AND (tripdata.BranchID = @PlantID) GROUP BY productsdata.tproduct, branchproducts.unitprice");
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@PlantID", Session["branch"]);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate).AddDays(-1));
                cmd.Parameters.AddWithValue("@Type", "Agent");
                DataTable dtAgent = vdm.SelectQuery(cmd).Tables[0];
                Report.Columns.Add("Customer Name");
                Report.Columns.Add("Invoice No");
                Report.Columns.Add("Invoice Date");
                Report.Columns.Add("Ledger Type");
                Report.Columns.Add("HSN Code");
                Report.Columns.Add("Item Name");
                Report.Columns.Add("Qty");
                Report.Columns.Add("Rate");
                Report.Columns.Add("Taxable Value");
                Report.Columns.Add("SGST%");
                Report.Columns.Add("SGST Amount");
                Report.Columns.Add("CGST%");
                Report.Columns.Add("CGST Amount");
                Report.Columns.Add("IGST%");
                Report.Columns.Add("IGST Amount");
                Report.Columns.Add("Net Value");
                Report.Columns.Add("Narration");
                int i = 1;
                if (dtAgent.Rows.Count > 0)
                {
                    foreach (DataRow branch in dtAgent.Rows)
                    {
                        if (branch["Igst"].ToString() == "0")
                        {
                            DataRow newrow = Report.NewRow();
                            if (ddlSalesOffice.SelectedValue == "306")
                            {
                                newrow["Invoice Date"] = fromdate.AddDays(1).ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                newrow["Invoice Date"] = fromdate.ToString("dd-MMM-yyyy");
                            }
                            if (ddltype.SelectedValue == "AR Invoice")
                            {
                                newrow["Customer Name"] = to_branchName;
                            }
                            else
                            {
                                newrow["Customer Name"] = from_branchName;
                            }
                            string DCNO = "0";
                            int countdc = 0;
                            int.TryParse(branch["DCNo"].ToString(), out countdc);
                            if (countdc <= 10)
                            {
                                DCNO = "0000" + countdc;
                            }
                            if (countdc >= 10 && countdc <= 99)
                            {
                                DCNO = "000" + countdc;
                            }
                            if (countdc >= 99 && countdc <= 999)
                            {
                                DCNO = "00" + countdc;
                            }
                            if (countdc > 999 && countdc <= 9999)
                            {
                                DCNO = "0" + countdc;
                            }
                            if (countdc > 9999)
                            {
                                DCNO = "" + countdc;
                            }

                            if (ddlSalesOffice.SelectedValue == "306")
                            {
                                if (fromdate.AddDays(1).Month > 3)
                                {
                                    newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                }
                                else
                                {
                                    if (fromdate.AddDays(1).Month < 3)
                                    {
                                        newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                    }
                                    else
                                    {
                                        newrow["Invoice No"] = from_branchCode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                                    }
                                }
                            }
                            else
                            {
                                if (fromdate.Month > 3)
                                {
                                    newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                }
                                else
                                {
                                    if (fromdate.Month < 3)
                                    {
                                        newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                    }
                                    else
                                    {
                                        newrow["Invoice No"] = from_branchCode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                                    }
                                }


                                //if (fromdate.Month > 3)
                                //{
                                //    newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                //}
                                //else
                                //{
                                //    //DCNO = Branchcode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                                //    newrow["Invoice No"] = from_branchCode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                                //}
                            }
                            // newrow["Ledger Type"] = branch["sno"].ToString();
                            newrow["HSN Code"] = branch["hsncode"].ToString();
                            newrow["Item Name"] = branch["tproduct"].ToString();
                            newrow["Qty"] = branch["qty"].ToString();
                            // newrow["Rate"] = "0";
                            double rate = 0;
                            double.TryParse(branch["unitprice"].ToString(), out rate);
                            string tcategory = "";
                            double tot_vatamount = 0;
                            double PAmount = 0;
                            double invval = 0;
                            double qty = 0;
                            double taxval = 0;
                            double.TryParse(branch["qty"].ToString(), out qty);
                            if (fromstateid == tostate)
                            {
                                double sgstamount = 0;
                                double cgstamount = 0;
                                double Igst = 0;
                                double Igstamount = 0;
                                double totRate = 0;
                                double.TryParse(branch["Igst"].ToString(), out Igst);
                                double Igstcon = 100 + Igst;
                                Igstamount = (rate / Igstcon) * Igst;
                                Igstamount = Math.Round(Igstamount, 2);
                                totRate = Igstamount;
                                tcategory = branch["tcategory"].ToString() + "-CGST/SGST-" + to_statecode;
                                newrow["Ledger Type"] = tcategory.ToString();
                                double Vatrate = rate - totRate;
                                Vatrate = Math.Round(Vatrate, 2);
                                newrow["Rate"] = Vatrate.ToString();
                                PAmount = qty * Vatrate;
                                newrow["Taxable Value"] = Math.Round(PAmount, 2);
                                tot_vatamount = (PAmount * Igst) / 100;
                                sgstamount = (tot_vatamount / 2);
                                sgstamount = Math.Round(sgstamount, 2);
                                newrow["sgst%"] = "'" + branch["sgst"].ToString();
                                newrow["sgst amount"] = sgstamount.ToString();
                                cgstamount = (tot_vatamount / 2);
                                cgstamount = Math.Round(cgstamount, 2);
                                newrow["cgst%"] = "'" + branch["cgst"].ToString();
                                newrow["cgst amount"] = cgstamount.ToString();
                                newrow["Igst%"] = "'" + 0;
                                newrow["Igst amount"] = 0;

                            }
                            else
                            {

                                double Igst = 0;
                                double Igstamount = 0;
                                double totRate = 0;
                                double.TryParse(branch["Igst"].ToString(), out Igst);
                                double Igstcon = 100 + Igst;
                                Igstamount = (rate / Igstcon) * Igst;
                                Igstamount = Math.Round(Igstamount, 2);
                                totRate = Igstamount;
                                if (ddltype.SelectedValue == "AR Invoice")
                                {
                                    tcategory = "Sale of " + branch["Categoryname"].ToString() + " inter branches";
                                }
                                else
                                {
                                    tcategory = "Purchase of " + branch["Categoryname"].ToString() + " inter branches";

                                }
                                newrow["Ledger Type"] = tcategory.ToString();
                                double Vatrate = rate - totRate;
                                Vatrate = Math.Round(Vatrate, 2);
                                newrow["Rate"] = Vatrate.ToString();
                                PAmount = qty * Vatrate;
                                newrow["Taxable Value"] = Math.Round(PAmount, 2);
                                tot_vatamount = (PAmount * Igst) / 100;
                                newrow["sgst%"] = "'" + 0;
                                newrow["sgst amount"] = 0;
                                newrow["cgst%"] = "'" + 0;
                                newrow["cgst amount"] = 0;
                                newrow["Igst%"] = "'" + branch["Igst"].ToString();
                                tot_vatamount = Math.Round(tot_vatamount, 2);
                                newrow["Igst amount"] = tot_vatamount.ToString();
                            }
                            invval = Math.Round(invval, 2);
                            double netvalue = 0;
                            netvalue = invval + taxval;
                            netvalue = Math.Round(netvalue, 2);

                            double tot_amount = PAmount + tot_vatamount;
                            tot_amount = Math.Round(tot_amount, 2);
                            newrow["Net Value"] = tot_amount;
                            if (ddltype.SelectedValue == "AR Invoice")
                            {
                                newrow["Narration"] = "Being the sale of milk to  " + ddlSalesOffice.SelectedItem.Text + " vide invoice no " + branch["dcno"].ToString() + ",DC Date " + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                            }
                            else
                            {
                                newrow["Narration"] = "Being the purchase of milk to  " + ddlSalesOffice.SelectedItem.Text + " vide invoice no " + branch["dcno"].ToString() + ",DC Date " + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                            }
                            Report.Rows.Add(newrow);
                            i++;
                        }
                        else
                        {
                            DateTime dtjuly = new DateTime();
                            string jul = "7/18/2022";
                            dtjuly = DateTime.Parse(jul);
                            if (dtjuly > fromdate)
                            {
                                string[] catarr = { "2", "12", "39", "47", "48" };
                                if (catarr.Contains(branch["catsno"].ToString()))
                                {
                                    DataRow newrow = Report.NewRow();
                                    if (ddlSalesOffice.SelectedValue == "306")
                                    {
                                        newrow["Invoice Date"] = fromdate.AddDays(1).ToString("dd-MMM-yyyy");
                                    }
                                    else
                                    {
                                        newrow["Invoice Date"] = fromdate.ToString("dd-MMM-yyyy");
                                    }
                                    if (ddltype.SelectedValue == "AR Invoice")
                                    {
                                        newrow["Customer Name"] = to_branchName;
                                    }
                                    else
                                    {
                                        newrow["Customer Name"] = from_branchName;
                                    }
                                    string DCNO = "0";
                                    int countdc = 0;
                                    int.TryParse(branch["DCNo"].ToString(), out countdc);
                                    if (countdc <= 10)
                                    {
                                        DCNO = "0000" + countdc;
                                    }
                                    if (countdc >= 10 && countdc <= 99)
                                    {
                                        DCNO = "000" + countdc;
                                    }
                                    if (countdc >= 99 && countdc <= 999)
                                    {
                                        DCNO = "00" + countdc;
                                    }
                                    if (countdc > 999 && countdc <= 9999)
                                    {
                                        DCNO = "0" + countdc;
                                    }
                                    if (countdc > 9999)
                                    {
                                        DCNO = "" + countdc;
                                    }

                                    if (ddlSalesOffice.SelectedValue == "306")
                                    {
                                        if (fromdate.AddDays(1).Month > 3)
                                        {
                                            newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                        }
                                        else
                                        {
                                            if (fromdate.AddDays(1).Month < 3)
                                            {
                                                newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                            }
                                            else
                                            {
                                                newrow["Invoice No"] = from_branchCode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (fromdate.Month > 3)
                                        {
                                            newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                        }
                                        else
                                        {
                                            if (fromdate.Month < 3)
                                            {
                                                newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                            }
                                            else
                                            {
                                                newrow["Invoice No"] = from_branchCode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                                            }
                                        }


                                        //if (fromdate.Month > 3)
                                        //{
                                        //    newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                        //}
                                        //else
                                        //{
                                        //    //DCNO = Branchcode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                                        //    newrow["Invoice No"] = from_branchCode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                                        //}
                                    }
                                    // newrow["Ledger Type"] = branch["sno"].ToString();
                                    newrow["HSN Code"] = branch["hsncode"].ToString();
                                    newrow["Item Name"] = branch["tproduct"].ToString();
                                    newrow["Qty"] = branch["qty"].ToString();
                                    // newrow["Rate"] = "0";
                                    double rate = 0;
                                    double.TryParse(branch["unitprice"].ToString(), out rate);
                                    string tcategory = "";
                                    double tot_vatamount = 0;
                                    double PAmount = 0;
                                    double invval = 0;
                                    double qty = 0;
                                    double taxval = 0;
                                    double.TryParse(branch["qty"].ToString(), out qty);
                                    if (fromstateid == tostate)
                                    {
                                        double sgstamount = 0;
                                        double cgstamount = 0;
                                        double Igst = 0;
                                        double Igstamount = 0;
                                        double totRate = 0;
                                        double.TryParse(branch["Igst"].ToString(), out Igst);
                                        double Igstcon = 100 + Igst;
                                        Igstamount = (rate / Igstcon) * Igst;
                                        Igstamount = Math.Round(Igstamount, 2);
                                        totRate = Igstamount;
                                        tcategory = branch["tcategory"].ToString() + "-CGST/SGST-" + to_statecode;
                                        newrow["Ledger Type"] = tcategory.ToString();
                                        double Vatrate = rate - totRate;
                                        Vatrate = Math.Round(Vatrate, 2);
                                        newrow["Rate"] = Vatrate.ToString();
                                        PAmount = qty * Vatrate;
                                        newrow["Taxable Value"] = Math.Round(PAmount, 2);
                                        tot_vatamount = (PAmount * Igst) / 100;
                                        sgstamount = (tot_vatamount / 2);
                                        sgstamount = Math.Round(sgstamount, 2);
                                        newrow["sgst%"] = "'" + 0;
                                        newrow["sgst amount"] = 0;
                                        cgstamount = (tot_vatamount / 2);
                                        cgstamount = Math.Round(cgstamount, 2);
                                        newrow["cgst%"] = "'" + 0;
                                        newrow["cgst amount"] = 0;
                                        newrow["Igst%"] = "'" + 0;
                                        newrow["Igst amount"] = 0;

                                    }
                                    else
                                    {

                                        double Igst = 0;
                                        double Igstamount = 0;
                                        double totRate = 0;
                                        double.TryParse(branch["Igst"].ToString(), out Igst);
                                        double Igstcon = 100 + Igst;
                                        Igstamount = (rate / Igstcon) * Igst;
                                        Igstamount = Math.Round(Igstamount, 2);
                                        totRate = Igstamount;
                                        if (ddltype.SelectedValue == "AR Invoice")
                                        {
                                            tcategory = "Sale of " + branch["Categoryname"].ToString() + " inter branches";
                                        }
                                        else
                                        {
                                            tcategory = "Purchase of " + branch["Categoryname"].ToString() + " inter branches";

                                        }
                                        newrow["Ledger Type"] = tcategory.ToString();
                                        double Vatrate = rate - totRate;
                                        Vatrate = Math.Round(Vatrate, 2);
                                        newrow["Rate"] = Vatrate.ToString();
                                        PAmount = qty * Vatrate;
                                        newrow["Taxable Value"] = Math.Round(PAmount, 2);
                                        tot_vatamount = (PAmount * Igst) / 100;
                                        newrow["sgst%"] = "'" + 0;
                                        newrow["sgst amount"] = 0;
                                        newrow["cgst%"] = "'" + 0;
                                        newrow["cgst amount"] = 0;
                                        newrow["Igst%"] = "'" + 0;
                                        tot_vatamount = Math.Round(tot_vatamount, 2);
                                        newrow["Igst amount"] = tot_vatamount.ToString();
                                    }
                                    invval = Math.Round(invval, 2);
                                    double netvalue = 0;
                                    netvalue = invval + taxval;
                                    netvalue = Math.Round(netvalue, 2);

                                    double tot_amount = PAmount + 0;
                                    tot_amount = Math.Round(tot_amount, 2);
                                    newrow["Net Value"] = tot_amount;
                                    if (ddltype.SelectedValue == "AR Invoice")
                                    {
                                        newrow["Narration"] = "Being the sale of milk to  " + ddlSalesOffice.SelectedItem.Text + " vide invoice no " + branch["dcno"].ToString() + ",DC Date " + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                                    }
                                    else
                                    {
                                        newrow["Narration"] = "Being the purchase of milk to  " + ddlSalesOffice.SelectedItem.Text + " vide invoice no " + branch["dcno"].ToString() + ",DC Date " + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                                    }
                                    Report.Rows.Add(newrow);
                                    i++;
                                }
                            }
                        }
                    }
                }
                cmd = new MySqlCommand("SELECT dispatch.Branch_ID,  dispatch.DispName, dispatch.sno, dispatch.BranchID, tripdata.I_Date, tripdata.Sno AS TripSno, dispatch.DispMode,  triproutes.Tripdata_sno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno  WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2)and (dispatch.DispType ='SO') and (tripdata.Status<>'C') ORDER BY dispatch.sno");
                cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtDispnames = vdm.SelectQuery(cmd).Tables[0];
                if (dtDispnames.Rows.Count > 0)
                {
                    foreach (DataRow drSub in dtDispnames.Rows)
                    {
                        if (drSub["DispMode"].ToString() == "" || drSub["DispMode"].ToString() == "SPL")
                        {
                        }
                        else
                        {
                            if (drSub["Branch_ID"].ToString() == ddlSalesOffice.SelectedValue)
                            {
                            }
                            else
                            {
                                cmd = new MySqlCommand("SELECT ff.TripID, Triproutes.RouteID, ff.Qty, ff.ProductId, Triproutes.Tripdata_sno, ff.I_Date, ff.tproduct, ff.hsncode, ff.igst, ff.cgst, ff.sgst, ff.UnitPrice, ff.DCNo,ff.taxdcno, ff.Categoryname,ff.sno as catsno FROM (SELECT  Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (Tripdata_sno = @TripSno)) Triproutes INNER JOIN (SELECT TripID, Qty, ProductId, I_Date, tproduct, hsncode, igst, cgst, sgst, UnitPrice, Categoryname,sno, DCNo,taxdcno FROM (SELECT tripdata.Sno AS TripID, tripsubdata.Qty, tripsubdata.ProductId, tripdata.I_Date, productsdata.hsncode, productsdata.igst, productsdata.cgst, productsdata.sgst, productsdata.UnitPrice, productsdata.tproduct, products_category.Categoryname,products_category.sno, tripdata.DCNo,tripdata.taxdcno FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE  (tripdata.I_Date BETWEEN @starttime AND @endtime)) tripinfo) ff ON ff.TripID = Triproutes.Tripdata_sno");
                                cmd.Parameters.AddWithValue("@TripSno", drSub["TripSno"].ToString());
                                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                                cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
                                DataTable DtTripSubData = vdm.SelectQuery(cmd).Tables[0];
                                foreach (DataRow branch in DtTripSubData.Rows)
                                {
                                    if (branch["Igst"].ToString() == "0")
                                    {

                                        DataRow newrow = Report.NewRow();
                                        newrow["Customer Name"] = to_branchName;
                                        newrow["Invoice Date"] = fromdate.ToString("dd-MMM-yyyy");
                                        string DCNO = "0";
                                        int countdc = 0;
                                        int.TryParse(branch["DCNo"].ToString(), out countdc);
                                        if (countdc <= 10)
                                        {
                                            DCNO = "0000" + countdc;
                                        }
                                        if (countdc >= 10 && countdc <= 99)
                                        {
                                            DCNO = "000" + countdc;
                                        }
                                        if (countdc >= 99 && countdc <= 999)
                                        {
                                            DCNO = "00" + countdc;
                                        }
                                        if (countdc > 999)
                                        {
                                            DCNO = "0" + countdc;
                                        }
                                        if (ddlSalesOffice.SelectedValue == "306")
                                        {
                                            if (fromdate.AddDays(1).Month > 3)
                                            {
                                                newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                            }
                                            else
                                            {
                                                if (fromdate.AddDays(1).Month < 3)
                                                {
                                                    newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                                }
                                                else
                                                {
                                                    newrow["Invoice No"] = from_branchCode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (fromdate.Month > 3)
                                            {
                                                newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                            }
                                            else
                                            {
                                                if (fromdate.Month < 3)
                                                {
                                                    newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                                }
                                                else
                                                {
                                                    newrow["Invoice No"] = from_branchCode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                                                }
                                            }
                                        }
                                        newrow["HSN Code"] = branch["hsncode"].ToString();
                                        newrow["Item Name"] = branch["tproduct"].ToString();
                                        newrow["Qty"] = branch["Qty"].ToString();
                                        double rate = 0;
                                        double.TryParse(branch["unitprice"].ToString(), out rate);
                                        string tcategory = "";
                                        double tot_vatamount = 0;
                                        double PAmount = 0;
                                        double invval = 0;
                                        double qty = 0;
                                        double taxval = 0;
                                        double.TryParse(branch["qty"].ToString(), out qty);
                                        if (fromstateid == tostate)
                                        {
                                            double sgstamount = 0;
                                            double cgstamount = 0;
                                            double Igst = 0;
                                            double Igstamount = 0;
                                            double totRate = 0;
                                            double.TryParse(branch["Igst"].ToString(), out Igst);
                                            double Igstcon = 100 + Igst;
                                            Igstamount = (rate / Igstcon) * Igst;
                                            Igstamount = Math.Round(Igstamount, 2);
                                            totRate = Igstamount;
                                            tcategory = branch["tcategory"].ToString() + "-CGST/SGST-" + to_statecode;
                                            newrow["Ledger Type"] = tcategory.ToString();
                                            double Vatrate = rate - totRate;
                                            Vatrate = Math.Round(Vatrate, 2);
                                            newrow["Rate"] = Vatrate.ToString();
                                            PAmount = qty * Vatrate;
                                            newrow["Taxable Value"] = Math.Round(PAmount, 2);
                                            tot_vatamount = (PAmount * Igst) / 100;
                                            sgstamount = (tot_vatamount / 2);
                                            sgstamount = Math.Round(sgstamount, 2);
                                            newrow["sgst%"] = "'" + branch["sgst"].ToString();
                                            newrow["sgst amount"] = sgstamount.ToString();
                                            cgstamount = (tot_vatamount / 2);
                                            cgstamount = Math.Round(cgstamount, 2);
                                            newrow["cgst%"] = "'" + branch["cgst"].ToString();
                                            newrow["cgst amount"] = cgstamount.ToString();
                                            newrow["Igst%"] = "'" + 0;
                                            newrow["Igst amount"] = 0;
                                        }
                                        else
                                        {

                                            double Igst = 0;
                                            double Igstamount = 0;
                                            double totRate = 0;
                                            double.TryParse(branch["Igst"].ToString(), out Igst);
                                            double Igstcon = 100 + Igst;
                                            Igstamount = (rate / Igstcon) * Igst;
                                            Igstamount = Math.Round(Igstamount, 2);
                                            totRate = Igstamount;
                                            if (ddltype.SelectedValue == "AR Invoice")
                                            {
                                                tcategory = "Sale of " + branch["Categoryname"].ToString() + " inter branches";
                                            }
                                            else
                                            {
                                                tcategory = "Purchase of " + branch["Categoryname"].ToString() + " inter branches";

                                            }
                                            newrow["Ledger Type"] = tcategory.ToString();
                                            double Vatrate = rate - totRate;
                                            Vatrate = Math.Round(Vatrate, 2);
                                            newrow["Rate"] = Vatrate.ToString();
                                            PAmount = qty * Vatrate;
                                            newrow["Taxable Value"] = Math.Round(PAmount, 2);
                                            tot_vatamount = (PAmount * Igst) / 100;
                                            newrow["sgst%"] = "'" + 0;
                                            newrow["sgst amount"] = 0;
                                            newrow["cgst%"] = "'" + 0;
                                            newrow["cgst amount"] = 0;
                                            newrow["Igst%"] = "'" + branch["Igst"].ToString();
                                            tot_vatamount = Math.Round(tot_vatamount, 2);
                                            newrow["Igst amount"] = tot_vatamount.ToString();
                                        }
                                        invval = Math.Round(invval, 2);
                                        double netvalue = 0;
                                        netvalue = invval + taxval;
                                        netvalue = Math.Round(netvalue, 2);
                                        double tot_amount = PAmount + tot_vatamount;
                                        tot_amount = Math.Round(tot_amount, 2);
                                        newrow["Net Value"] = tot_amount;
                                        if (ddltype.SelectedValue == "AR Invoice")
                                        {
                                            newrow["Narration"] = "Being the sale of milk to  " + ddlSalesOffice.SelectedItem.Text + " vide invoice no " + branch["dcno"].ToString() + ",DC Date " + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                                        }
                                        else
                                        {
                                            newrow["Narration"] = "Being the purchase of milk to  " + ddlSalesOffice.SelectedItem.Text + " vide invoice no " + branch["dcno"].ToString() + ",DC Date " + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                                        }
                                        Report.Rows.Add(newrow);
                                        i++;
                                    }
                                    else
                                    {
                                        DateTime dtjuly = new DateTime();
                                        string jul = "7/18/2022";
                                        dtjuly = DateTime.Parse(jul);
                                        if (dtjuly > fromdate)
                                        {
                                            string[] catarr = { "2", "12", "39", "47", "48" };
                                            if (catarr.Contains(branch["catsno"].ToString()))
                                            {
                                                DataRow newrow = Report.NewRow();
                                                newrow["Customer Name"] = to_branchName;
                                                newrow["Invoice Date"] = fromdate.ToString("dd-MMM-yyyy");
                                                string DCNO = "0";
                                                int countdc = 0;
                                                int.TryParse(branch["DCNo"].ToString(), out countdc);
                                                if (countdc <= 10)
                                                {
                                                    DCNO = "0000" + countdc;
                                                }
                                                if (countdc >= 10 && countdc <= 99)
                                                {
                                                    DCNO = "000" + countdc;
                                                }
                                                if (countdc >= 99 && countdc <= 999)
                                                {
                                                    DCNO = "00" + countdc;
                                                }
                                                if (countdc > 999)
                                                {
                                                    DCNO = "0" + countdc;
                                                }
                                                if (ddlSalesOffice.SelectedValue == "306")
                                                {
                                                    if (fromdate.AddDays(1).Month > 3)
                                                    {
                                                        newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                                    }
                                                    else
                                                    {
                                                        if (fromdate.AddDays(1).Month < 3)
                                                        {
                                                            newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                                        }
                                                        else
                                                        {
                                                            newrow["Invoice No"] = from_branchCode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (fromdate.Month > 3)
                                                    {
                                                        newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                                    }
                                                    else
                                                    {
                                                        if (fromdate.Month < 3)
                                                        {
                                                            newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                                        }
                                                        else
                                                        {
                                                            newrow["Invoice No"] = from_branchCode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                                                        }
                                                    }
                                                }
                                                newrow["HSN Code"] = branch["hsncode"].ToString();
                                                newrow["Item Name"] = branch["tproduct"].ToString();
                                                newrow["Qty"] = branch["Qty"].ToString();
                                                double rate = 0;
                                                double.TryParse(branch["unitprice"].ToString(), out rate);
                                                string tcategory = "";
                                                double tot_vatamount = 0;
                                                double PAmount = 0;
                                                double invval = 0;
                                                double qty = 0;
                                                double taxval = 0;
                                                double.TryParse(branch["qty"].ToString(), out qty);
                                                if (fromstateid == tostate)
                                                {
                                                    double sgstamount = 0;
                                                    double cgstamount = 0;
                                                    double Igst = 0;
                                                    double Igstamount = 0;
                                                    double totRate = 0;
                                                    double.TryParse(branch["Igst"].ToString(), out Igst);
                                                    double Igstcon = 100 + Igst;
                                                    Igstamount = (rate / Igstcon) * Igst;
                                                    Igstamount = Math.Round(Igstamount, 2);
                                                    totRate = Igstamount;
                                                    tcategory = branch["tcategory"].ToString() + "-CGST/SGST-" + to_statecode;
                                                    newrow["Ledger Type"] = tcategory.ToString();
                                                    double Vatrate = rate - totRate;
                                                    Vatrate = Math.Round(Vatrate, 2);
                                                    newrow["Rate"] = Vatrate.ToString();
                                                    PAmount = qty * Vatrate;
                                                    newrow["Taxable Value"] = Math.Round(PAmount, 2);
                                                    tot_vatamount = (PAmount * Igst) / 100;
                                                    sgstamount = (tot_vatamount / 2);
                                                    sgstamount = Math.Round(sgstamount, 2);
                                                    newrow["sgst%"] = "'" + 0;
                                                    newrow["sgst amount"] = 0;
                                                    cgstamount = (tot_vatamount / 2);
                                                    cgstamount = Math.Round(cgstamount, 2);
                                                    newrow["cgst%"] = "'" + 0;
                                                    newrow["cgst amount"] = 0;
                                                    newrow["Igst%"] = "'" + 0;
                                                    newrow["Igst amount"] = 0;
                                                }
                                                else
                                                {

                                                    double Igst = 0;
                                                    double Igstamount = 0;
                                                    double totRate = 0;
                                                    double.TryParse(branch["Igst"].ToString(), out Igst);
                                                    double Igstcon = 100 + Igst;
                                                    Igstamount = (rate / Igstcon) * Igst;
                                                    Igstamount = Math.Round(Igstamount, 2);
                                                    totRate = Igstamount;
                                                    if (ddltype.SelectedValue == "AR Invoice")
                                                    {
                                                        tcategory = "Sale of " + branch["Categoryname"].ToString() + " inter branches";
                                                    }
                                                    else
                                                    {
                                                        tcategory = "Purchase of " + branch["Categoryname"].ToString() + " inter branches";

                                                    }
                                                    newrow["Ledger Type"] = tcategory.ToString();
                                                    double Vatrate = rate - totRate;
                                                    Vatrate = Math.Round(Vatrate, 2);
                                                    newrow["Rate"] = Vatrate.ToString();
                                                    PAmount = qty * Vatrate;
                                                    newrow["Taxable Value"] = Math.Round(PAmount, 2);
                                                    tot_vatamount = (PAmount * Igst) / 100;
                                                    newrow["sgst%"] = "'" + 0;
                                                    newrow["sgst amount"] = 0;
                                                    newrow["cgst%"] = "'" + 0;
                                                    newrow["cgst amount"] = 0;
                                                    newrow["Igst%"] = "'" + 0;
                                                    tot_vatamount = Math.Round(tot_vatamount, 2);
                                                    newrow["Igst amount"] = 0;
                                                }
                                                invval = Math.Round(invval, 2);
                                                double netvalue = 0;
                                                netvalue = invval + taxval;
                                                netvalue = Math.Round(netvalue, 2);
                                                double tot_amount = PAmount;
                                                tot_amount = Math.Round(tot_amount, 2);
                                                newrow["Net Value"] = tot_amount;
                                                if (ddltype.SelectedValue == "AR Invoice")
                                                {
                                                    newrow["Narration"] = "Being the sale of milk to  " + ddlSalesOffice.SelectedItem.Text + " vide invoice no " + branch["dcno"].ToString() + ",DC Date " + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                                                }
                                                else
                                                {
                                                    newrow["Narration"] = "Being the purchase of milk to  " + ddlSalesOffice.SelectedItem.Text + " vide invoice no " + branch["dcno"].ToString() + ",DC Date " + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                                                }
                                                Report.Rows.Add(newrow);
                                                i++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;
                }
                else
                {
                    pnlHide.Visible = false;
                    lblmsg.Text = "No Indent Found";
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                }
            }
            else
            {
                if (ddlSalesOffice.SelectedValue == "172")
                {
                    cmd = new MySqlCommand("SELECT productsdata.hsncode, productsdata.igst, productsdata.cgst, productsdata.sgst, productsdata.tproduct, ROUND(SUM(tripsubdata.Qty), 2) AS qty,tripdata.dcno,tripdata.taxdcno, tripdata.Sno, tripdata.I_Date, empmanage.UserName, branchproducts.unitprice, products_category.Categoryname,products_category.tcategory FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN empmanage ON tripdata.EmpId = empmanage.Sno INNER JOIN branchproducts ON empmanage.Branch = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE  (tripdata.I_Date BETWEEN @d1 AND @d2) AND (empmanage.Branch = @BranchID) AND (tripdata.BranchID = @PlantID) GROUP BY productsdata.tproduct, branchproducts.unitprice");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@PlantID", Session["branch"]);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                    cmd.Parameters.AddWithValue("@Type", "Agent");
                }
                else
                {
                    cmd = new MySqlCommand("SELECT productsdata.hsncode, productsdata.igst, productsdata.cgst, productsdata.sgst, productsdata.tproduct, ROUND(SUM(tripsubdata.Qty), 2) AS qty,tripdata.dcno,tripdata.taxdcno, tripdata.Sno, tripdata.I_Date, empmanage.UserName, branchproducts.unitprice, products_category.Categoryname,products_category.tcategory,products_category.sno as categoryid FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN empmanage ON tripdata.EmpId = empmanage.Sno INNER JOIN branchproducts ON empmanage.Branch = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE  (tripdata.I_Date BETWEEN @d1 AND @d2) AND (empmanage.Branch = @BranchID) AND (tripdata.BranchID = @PlantID) GROUP BY productsdata.tproduct, branchproducts.unitprice");
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    cmd.Parameters.AddWithValue("@PlantID", Session["branch"]);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate).AddDays(-1));
                    cmd.Parameters.AddWithValue("@Type", "Agent");
                }
                DataTable dtAgent = vdm.SelectQuery(cmd).Tables[0];
                Report.Columns.Add("Customer Name");
                Report.Columns.Add("Invoice No");
                Report.Columns.Add("Invoice Date");
                Report.Columns.Add("Ledger Type");
                Report.Columns.Add("HSN Code");
                Report.Columns.Add("Item Name");
                Report.Columns.Add("Qty");
                Report.Columns.Add("Rate");
                Report.Columns.Add("Taxable Value");
                Report.Columns.Add("SGST%");
                Report.Columns.Add("SGST Amount");
                Report.Columns.Add("CGST%");
                Report.Columns.Add("CGST Amount");
                Report.Columns.Add("IGST%");
                Report.Columns.Add("IGST Amount");
                Report.Columns.Add("Net Value");
                Report.Columns.Add("Narration");
                int i = 1;
                if (dtAgent.Rows.Count > 0)
                {
                    foreach (DataRow branch in dtAgent.Rows)
                    {

                        DateTime dtjuly = new DateTime();
                        string jul = "7/18/2022";
                        dtjuly = DateTime.Parse(jul);
                        if (dtjuly > fromdate)
                        {
                            string[] catarr = { "2", "12", "39", "47", "48" };
                            if (catarr.Contains(branch["categoryid"].ToString()))
                            {

                            }
                            else
                            {
                                if (branch["Igst"].ToString() != "0")
                                {
                                    DataRow newrow = Report.NewRow();
                                    if (ddlSalesOffice.SelectedValue == "306")
                                    {
                                        newrow["Invoice Date"] = fromdate.AddDays(1).ToString("dd-MMM-yyyy");
                                    }
                                    else
                                    {
                                        newrow["Invoice Date"] = fromdate.ToString("dd-MMM-yyyy");
                                    }
                                    if (ddltype.SelectedValue == "AR Invoice")
                                    {
                                        newrow["Customer Name"] = to_branchName;
                                    }
                                    else
                                    {
                                        newrow["Customer Name"] = from_branchName;
                                    }
                                    string DCNO = "0";
                                    int countdc = 0;
                                    int.TryParse(branch["taxdcno"].ToString(), out countdc);
                                    if (countdc <= 10)
                                    {
                                        DCNO = "0000" + countdc;
                                    }
                                    if (countdc >= 10 && countdc <= 99)
                                    {
                                        DCNO = "000" + countdc;
                                    }
                                    if (countdc >= 99 && countdc <= 999)
                                    {
                                        DCNO = "00" + countdc;
                                    }
                                    if (countdc > 999)
                                    {
                                        DCNO = "0" + countdc;
                                    }
                                    if (ddlSalesOffice.SelectedValue == "306")
                                    {
                                        if (fromdate.AddDays(1).Month > 3)
                                        {
                                            newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                        }
                                        else
                                        {
                                            if (fromdate.AddDays(1).Month < 3)
                                            {
                                                newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                            }
                                            else
                                            {
                                                newrow["Invoice No"] = from_branchCode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "T/" + DCNO;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (fromdate.Month > 3)
                                        {
                                            newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                        }
                                        else
                                        {
                                            if (fromdate.Month < 3)
                                            {
                                                newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                            }
                                            else
                                            {
                                                newrow["Invoice No"] = from_branchCode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "T/" + DCNO;
                                            }
                                        }
                                    }
                                    // newrow["Ledger Type"] = branch["sno"].ToString();
                                    newrow["HSN Code"] = branch["hsncode"].ToString();
                                    newrow["Item Name"] = branch["tproduct"].ToString();
                                    newrow["Qty"] = branch["qty"].ToString();
                                    // newrow["Rate"] = "0";
                                    double rate = 0;
                                    double.TryParse(branch["unitprice"].ToString(), out rate);
                                    string tcategory = "";
                                    double tot_vatamount = 0;
                                    double PAmount = 0;
                                    double invval = 0;
                                    double qty = 0;
                                    double taxval = 0;
                                    double.TryParse(branch["qty"].ToString(), out qty);
                                    if (fromstateid == tostate)
                                    {
                                        double sgstamount = 0;
                                        double cgstamount = 0;
                                        double Igst = 0;
                                        double Igstamount = 0;
                                        double totRate = 0;
                                        double.TryParse(branch["Igst"].ToString(), out Igst);
                                        double Igstcon = 100 + Igst;
                                        Igstamount = (rate / Igstcon) * Igst;
                                        Igstamount = Math.Round(Igstamount, 2);
                                        totRate = Igstamount;
                                        tcategory = branch["tcategory"].ToString() + "-CGST/SGST-" + to_statecode;
                                        newrow["Ledger Type"] = tcategory.ToString();
                                        double Vatrate = rate - totRate;
                                        Vatrate = Math.Round(Vatrate, 2);
                                        newrow["Rate"] = Vatrate.ToString();
                                        PAmount = qty * Vatrate;
                                        newrow["Taxable Value"] = Math.Round(PAmount, 2);
                                        tot_vatamount = (PAmount * Igst) / 100;
                                        sgstamount = (tot_vatamount / 2);
                                        sgstamount = Math.Round(sgstamount, 2);
                                        newrow["sgst%"] = "'" + branch["sgst"].ToString();
                                        newrow["sgst amount"] = sgstamount.ToString();
                                        cgstamount = (tot_vatamount / 2);
                                        cgstamount = Math.Round(cgstamount, 2);
                                        newrow["cgst%"] = "'" + branch["cgst"].ToString();
                                        newrow["cgst amount"] = cgstamount.ToString();
                                        newrow["Igst%"] = "'" + 0;
                                        newrow["Igst amount"] = 0;
                                    }
                                    else
                                    {

                                        double Igst = 0;
                                        double Igstamount = 0;
                                        double totRate = 0;
                                        double.TryParse(branch["Igst"].ToString(), out Igst);
                                        double Igstcon = 100 + Igst;
                                        Igstamount = (rate / Igstcon) * Igst;
                                        Igstamount = Math.Round(Igstamount, 2);
                                        totRate = Igstamount;
                                        if (ddltype.SelectedValue == "AR Invoice")
                                        {
                                            tcategory = "Sale of " + branch["Categoryname"].ToString() + " inter branches";
                                        }
                                        else
                                        {
                                            tcategory = "Purchase of " + branch["Categoryname"].ToString() + " inter branches";

                                        }
                                        newrow["Ledger Type"] = tcategory.ToString();
                                        double Vatrate = rate - totRate;
                                        Vatrate = Math.Round(Vatrate, 2);
                                        newrow["Rate"] = Vatrate.ToString();
                                        PAmount = qty * Vatrate;
                                        newrow["Taxable Value"] = Math.Round(PAmount, 2);
                                        tot_vatamount = (PAmount * Igst) / 100;
                                        newrow["sgst%"] = "'" + 0;
                                        newrow["sgst amount"] = 0;
                                        newrow["cgst%"] = "'" + 0;
                                        newrow["cgst amount"] = 0;
                                        newrow["Igst%"] = "'" + branch["Igst"].ToString();
                                        tot_vatamount = Math.Round(tot_vatamount, 2);
                                        newrow["Igst amount"] = tot_vatamount.ToString();
                                    }
                                    invval = Math.Round(invval, 2);
                                    double netvalue = 0;
                                    netvalue = invval + taxval;
                                    netvalue = Math.Round(netvalue, 2);
                                    double tot_amount = PAmount + tot_vatamount;
                                    tot_amount = Math.Round(tot_amount, 2);
                                    newrow["Net Value"] = tot_amount;
                                    if (ddltype.SelectedValue == "AR Invoice")
                                    {
                                        newrow["Narration"] = "Being the sale of milk to  " + ddlSalesOffice.SelectedItem.Text + " vide invoice no " + branch["dcno"].ToString() + ",DC Date " + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                                    }
                                    else
                                    {
                                        newrow["Narration"] = "Being the purchase of milk to  " + ddlSalesOffice.SelectedItem.Text + " vide invoice no " + branch["dcno"].ToString() + ",DC Date " + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                                    }
                                    Report.Rows.Add(newrow);
                                    i++;
                                }
                            }
                        }
                        else
                        {
                            if (branch["Igst"].ToString() != "0")
                            {
                                DataRow newrow = Report.NewRow();
                                if (ddlSalesOffice.SelectedValue == "306")
                                {
                                    newrow["Invoice Date"] = fromdate.AddDays(1).ToString("dd-MMM-yyyy");
                                }
                                else
                                {
                                    newrow["Invoice Date"] = fromdate.ToString("dd-MMM-yyyy");
                                }
                                if (ddltype.SelectedValue == "AR Invoice")
                                {
                                    newrow["Customer Name"] = to_branchName;
                                }
                                else
                                {
                                    newrow["Customer Name"] = from_branchName;
                                }
                                string DCNO = "0";
                                int countdc = 0;
                                int.TryParse(branch["taxdcno"].ToString(), out countdc);
                                if (countdc <= 10)
                                {
                                    DCNO = "0000" + countdc;
                                }
                                if (countdc >= 10 && countdc <= 99)
                                {
                                    DCNO = "000" + countdc;
                                }
                                if (countdc >= 99 && countdc <= 999)
                                {
                                    DCNO = "00" + countdc;
                                }
                                if (countdc > 999)
                                {
                                    DCNO = "0" + countdc;
                                }
                                if (ddlSalesOffice.SelectedValue == "306")
                                {
                                    if (fromdate.AddDays(1).Month > 3)
                                    {
                                        newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                    }
                                    else
                                    {
                                        if (fromdate.AddDays(1).Month < 3)
                                        {
                                            newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                        }
                                        else
                                        {
                                            newrow["Invoice No"] = from_branchCode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "T/" + DCNO;
                                        }
                                    }
                                }
                                else
                                {
                                    if (fromdate.Month > 3)
                                    {
                                        newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                    }
                                    else
                                    {
                                        if (fromdate.Month < 3)
                                        {
                                            newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                        }
                                        else
                                        {
                                            newrow["Invoice No"] = from_branchCode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "T/" + DCNO;
                                        }
                                    }
                                }
                                // newrow["Ledger Type"] = branch["sno"].ToString();
                                newrow["HSN Code"] = branch["hsncode"].ToString();
                                newrow["Item Name"] = branch["tproduct"].ToString();
                                newrow["Qty"] = branch["qty"].ToString();
                                // newrow["Rate"] = "0";
                                double rate = 0;
                                double.TryParse(branch["unitprice"].ToString(), out rate);
                                string tcategory = "";
                                double tot_vatamount = 0;
                                double PAmount = 0;
                                double invval = 0;
                                double qty = 0;
                                double taxval = 0;
                                double.TryParse(branch["qty"].ToString(), out qty);
                                if (fromstateid == tostate)
                                {
                                    double sgstamount = 0;
                                    double cgstamount = 0;
                                    double Igst = 0;
                                    double Igstamount = 0;
                                    double totRate = 0;
                                    double.TryParse(branch["Igst"].ToString(), out Igst);
                                    double Igstcon = 100 + Igst;
                                    Igstamount = (rate / Igstcon) * Igst;
                                    Igstamount = Math.Round(Igstamount, 2);
                                    totRate = Igstamount;
                                    tcategory = branch["tcategory"].ToString() + "-CGST/SGST-" + to_statecode;
                                    newrow["Ledger Type"] = tcategory.ToString();
                                    double Vatrate = rate - totRate;
                                    Vatrate = Math.Round(Vatrate, 2);
                                    newrow["Rate"] = Vatrate.ToString();
                                    PAmount = qty * Vatrate;
                                    newrow["Taxable Value"] = Math.Round(PAmount, 2);
                                    tot_vatamount = (PAmount * Igst) / 100;
                                    sgstamount = (tot_vatamount / 2);
                                    sgstamount = Math.Round(sgstamount, 2);
                                    newrow["sgst%"] = "'" + branch["sgst"].ToString();
                                    newrow["sgst amount"] = sgstamount.ToString();
                                    cgstamount = (tot_vatamount / 2);
                                    cgstamount = Math.Round(cgstamount, 2);
                                    newrow["cgst%"] = "'" + branch["cgst"].ToString();
                                    newrow["cgst amount"] = cgstamount.ToString();
                                    newrow["Igst%"] = "'" + 0;
                                    newrow["Igst amount"] = 0;
                                }
                                else
                                {

                                    double Igst = 0;
                                    double Igstamount = 0;
                                    double totRate = 0;
                                    double.TryParse(branch["Igst"].ToString(), out Igst);
                                    double Igstcon = 100 + Igst;
                                    Igstamount = (rate / Igstcon) * Igst;
                                    Igstamount = Math.Round(Igstamount, 2);
                                    totRate = Igstamount;
                                    if (ddltype.SelectedValue == "AR Invoice")
                                    {
                                        tcategory = "Sale of " + branch["Categoryname"].ToString() + " inter branches";
                                    }
                                    else
                                    {
                                        tcategory = "Purchase of " + branch["Categoryname"].ToString() + " inter branches";

                                    }
                                    newrow["Ledger Type"] = tcategory.ToString();
                                    double Vatrate = rate - totRate;
                                    Vatrate = Math.Round(Vatrate, 2);
                                    newrow["Rate"] = Vatrate.ToString();
                                    PAmount = qty * Vatrate;
                                    newrow["Taxable Value"] = Math.Round(PAmount, 2);
                                    tot_vatamount = (PAmount * Igst) / 100;
                                    newrow["sgst%"] = "'" + 0;
                                    newrow["sgst amount"] = 0;
                                    newrow["cgst%"] = "'" + 0;
                                    newrow["cgst amount"] = 0;
                                    newrow["Igst%"] = "'" + branch["Igst"].ToString();
                                    tot_vatamount = Math.Round(tot_vatamount, 2);
                                    newrow["Igst amount"] = tot_vatamount.ToString();
                                }
                                invval = Math.Round(invval, 2);
                                double netvalue = 0;
                                netvalue = invval + taxval;
                                netvalue = Math.Round(netvalue, 2);
                                double tot_amount = PAmount + tot_vatamount;
                                tot_amount = Math.Round(tot_amount, 2);
                                newrow["Net Value"] = tot_amount;
                                if (ddltype.SelectedValue == "AR Invoice")
                                {
                                    newrow["Narration"] = "Being the sale of milk to  " + ddlSalesOffice.SelectedItem.Text + " vide invoice no " + branch["dcno"].ToString() + ",DC Date " + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                                }
                                else
                                {
                                    newrow["Narration"] = "Being the purchase of milk to  " + ddlSalesOffice.SelectedItem.Text + " vide invoice no " + branch["dcno"].ToString() + ",DC Date " + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                                }
                                Report.Rows.Add(newrow);
                                i++;
                            }
                        }
                    }
                }
                cmd = new MySqlCommand("SELECT dispatch.Branch_ID,  dispatch.DispName, dispatch.sno, dispatch.BranchID, tripdata.I_Date, tripdata.Sno AS TripSno, dispatch.DispMode,  triproutes.Tripdata_sno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno  WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2)and (dispatch.DispType ='SO') and (tripdata.Status<>'C') ORDER BY dispatch.sno");
                cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtDispnames = vdm.SelectQuery(cmd).Tables[0];
                if (dtDispnames.Rows.Count > 0)
                {
                    foreach (DataRow drSub in dtDispnames.Rows)
                    {
                        if (drSub["DispMode"].ToString() == "" || drSub["DispMode"].ToString() == "SPL")
                        {
                        }
                        else
                        {
                            if (drSub["Branch_ID"].ToString() == ddlSalesOffice.SelectedValue)
                            {
                            }
                            else
                            {
                                cmd = new MySqlCommand("SELECT ff.TripID, Triproutes.RouteID, ff.Qty, ff.ProductId, Triproutes.Tripdata_sno, ff.I_Date, ff.tproduct, ff.hsncode, ff.igst, ff.cgst, ff.sgst, ff.UnitPrice, ff.DCNo,ff.taxdcno, ff.Categoryname,ff.categoryid FROM (SELECT  Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (Tripdata_sno = @TripSno)) Triproutes INNER JOIN (SELECT TripID, Qty, ProductId, I_Date, tproduct, hsncode, igst, cgst, sgst, UnitPrice, Categoryname, categoryid, DCNo,taxdcno, FROM (SELECT tripdata.Sno AS TripID, tripsubdata.Qty, tripsubdata.ProductId, tripdata.I_Date, productsdata.hsncode, productsdata.igst, productsdata.cgst, productsdata.sgst, productsdata.UnitPrice, productsdata.tproduct, products_category.Categoryname,products_category.sno as categoryid,  tripdata.DCNo,tripdata.taxdcno FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE  (tripdata.I_Date BETWEEN @starttime AND @endtime)) tripinfo) ff ON ff.TripID = Triproutes.Tripdata_sno");
                                cmd.Parameters.AddWithValue("@TripSno", drSub["TripSno"].ToString());
                                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                                cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
                                DataTable DtTripSubData = vdm.SelectQuery(cmd).Tables[0];
                                foreach (DataRow branch in DtTripSubData.Rows)
                                {
                                    DateTime dtjuly = new DateTime();
                                    string jul = "7/18/2022";
                                    dtjuly = DateTime.Parse(jul);
                                    if (dtjuly > fromdate)
                                    {
                                        string[] catarr = { "2", "12", "39", "47", "48" };
                                        if (catarr.Contains(branch["categoryid"].ToString()))
                                        {

                                        }
                                        else
                                        {
                                            if (branch["Igst"].ToString() != "0")
                                            {
                                                DataRow newrow = Report.NewRow();
                                                newrow["Customer Name"] = to_branchName;
                                                newrow["Invoice Date"] = fromdate.ToString("dd-MMM-yyyy");
                                                string DCNO = "0";
                                                int countdc = 0;
                                                int.TryParse(branch["taxdcno"].ToString(), out countdc);
                                                if (countdc <= 10)
                                                {
                                                    DCNO = "0000" + countdc;
                                                }
                                                if (countdc >= 10 && countdc <= 99)
                                                {
                                                    DCNO = "000" + countdc;
                                                }
                                                if (countdc >= 99 && countdc <= 999)
                                                {
                                                    DCNO = "00" + countdc;
                                                }
                                                if (countdc > 999)
                                                {
                                                    DCNO = "0" + countdc;
                                                }

                                                if (ddlSalesOffice.SelectedValue == "306")
                                                {
                                                    if (fromdate.AddDays(1).Month > 3)
                                                    {
                                                        newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                                    }
                                                    else
                                                    {
                                                        if (fromdate.AddDays(1).Month < 3)
                                                        {
                                                            newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                                        }
                                                        else
                                                        {
                                                            newrow["Invoice No"] = from_branchCode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "T/" + DCNO;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (fromdate.Month > 3)
                                                    {
                                                        newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                                    }
                                                    else
                                                    {
                                                        if (fromdate.Month < 3)
                                                        {
                                                            newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                                        }
                                                        else
                                                        {
                                                            newrow["Invoice No"] = from_branchCode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "T/" + DCNO;
                                                        }
                                                    }
                                                }
                                                //newrow["Ledger Type"] = branch["sno"].ToSt    ring();
                                                newrow["HSN Code"] = branch["hsncode"].ToString();
                                                newrow["Item Name"] = branch["tproduct"].ToString();
                                                newrow["Qty"] = branch["Qty"].ToString();
                                                double rate = 0;
                                                double.TryParse(branch["unitprice"].ToString(), out rate);
                                                string tcategory = "";
                                                double tot_vatamount = 0;
                                                double PAmount = 0;
                                                double invval = 0;
                                                double qty = 0;
                                                double taxval = 0;
                                                double.TryParse(branch["qty"].ToString(), out qty);
                                                if (fromstateid == tostate)
                                                {
                                                    double sgstamount = 0;
                                                    double cgstamount = 0;
                                                    double Igst = 0;
                                                    double Igstamount = 0;
                                                    double totRate = 0;
                                                    double.TryParse(branch["Igst"].ToString(), out Igst);
                                                    double Igstcon = 100 + Igst;
                                                    Igstamount = (rate / Igstcon) * Igst;
                                                    Igstamount = Math.Round(Igstamount, 2);
                                                    totRate = Igstamount;
                                                    tcategory = branch["tcategory"].ToString() + "-CGST/SGST-" + to_statecode;
                                                    newrow["Ledger Type"] = tcategory.ToString();
                                                    double Vatrate = rate - totRate;
                                                    Vatrate = Math.Round(Vatrate, 2);
                                                    newrow["Rate"] = Vatrate.ToString();
                                                    PAmount = qty * Vatrate;
                                                    newrow["Taxable Value"] = Math.Round(PAmount, 2);
                                                    tot_vatamount = (PAmount * Igst) / 100;
                                                    sgstamount = (tot_vatamount / 2);
                                                    sgstamount = Math.Round(sgstamount, 2);
                                                    newrow["sgst%"] = "'" + branch["sgst"].ToString();
                                                    newrow["sgst amount"] = sgstamount.ToString();
                                                    cgstamount = (tot_vatamount / 2);
                                                    cgstamount = Math.Round(cgstamount, 2);
                                                    newrow["cgst%"] = "'" + branch["cgst"].ToString();
                                                    newrow["cgst amount"] = cgstamount.ToString();
                                                    newrow["Igst%"] = "'" + 0;
                                                    newrow["Igst amount"] = 0;
                                                }
                                                else
                                                {

                                                    double Igst = 0;
                                                    double Igstamount = 0;
                                                    double totRate = 0;
                                                    double.TryParse(branch["Igst"].ToString(), out Igst);
                                                    double Igstcon = 100 + Igst;
                                                    Igstamount = (rate / Igstcon) * Igst;
                                                    Igstamount = Math.Round(Igstamount, 2);
                                                    totRate = Igstamount;
                                                    if (ddltype.SelectedValue == "AR Invoice")
                                                    {
                                                        tcategory = "Sale of " + branch["Categoryname"].ToString() + " inter branches";
                                                    }
                                                    else
                                                    {
                                                        tcategory = "Purchase of " + branch["Categoryname"].ToString() + " inter branches";

                                                    }
                                                    newrow["Ledger Type"] = tcategory.ToString();
                                                    double Vatrate = rate - totRate;
                                                    Vatrate = Math.Round(Vatrate, 2);
                                                    newrow["Rate"] = Vatrate.ToString();
                                                    PAmount = qty * Vatrate;
                                                    newrow["Taxable Value"] = Math.Round(PAmount, 2);
                                                    tot_vatamount = (PAmount * Igst) / 100;
                                                    newrow["sgst%"] = "'" + 0;
                                                    newrow["sgst amount"] = 0;
                                                    newrow["cgst%"] = "'" + 0;
                                                    newrow["cgst amount"] = 0;
                                                    newrow["Igst%"] = "'" + branch["Igst"].ToString();
                                                    tot_vatamount = Math.Round(tot_vatamount, 2);
                                                    newrow["Igst amount"] = tot_vatamount.ToString();
                                                }
                                                invval = Math.Round(invval, 2);
                                                double netvalue = 0;
                                                netvalue = invval + taxval;
                                                netvalue = Math.Round(netvalue, 2);
                                                double tot_amount = PAmount + tot_vatamount;
                                                tot_amount = Math.Round(tot_amount, 2);
                                                newrow["Net Value"] = tot_amount;
                                                if (ddltype.SelectedValue == "AR Invoice")
                                                {
                                                    newrow["Narration"] = "Being the sale of milk to  " + ddlSalesOffice.SelectedItem.Text + " vide invoice no " + branch["dcno"].ToString() + ",DC Date " + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                                                }
                                                else
                                                {
                                                    newrow["Narration"] = "Being the purchase of milk to  " + ddlSalesOffice.SelectedItem.Text + " vide invoice no " + branch["dcno"].ToString() + ",DC Date " + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                                                }
                                                Report.Rows.Add(newrow);
                                                i++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (branch["Igst"].ToString() != "0")
                                        {
                                            DataRow newrow = Report.NewRow();
                                            newrow["Customer Name"] = to_branchName;
                                            newrow["Invoice Date"] = fromdate.ToString("dd-MMM-yyyy");
                                            string DCNO = "0";
                                            int countdc = 0;
                                            int.TryParse(branch["taxdcno"].ToString(), out countdc);
                                            if (countdc <= 10)
                                            {
                                                DCNO = "0000" + countdc;
                                            }
                                            if (countdc >= 10 && countdc <= 99)
                                            {
                                                DCNO = "000" + countdc;
                                            }
                                            if (countdc >= 99 && countdc <= 999)
                                            {
                                                DCNO = "00" + countdc;
                                            }
                                            if (countdc > 999)
                                            {
                                                DCNO = "0" + countdc;
                                            }

                                            if (ddlSalesOffice.SelectedValue == "306")
                                            {
                                                if (fromdate.AddDays(1).Month > 3)
                                                {
                                                    newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                                }
                                                else
                                                {
                                                    if (fromdate.AddDays(1).Month < 3)
                                                    {
                                                        newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                                    }
                                                    else
                                                    {
                                                        newrow["Invoice No"] = from_branchCode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "T/" + DCNO;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (fromdate.Month > 3)
                                                {
                                                    newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                                }
                                                else
                                                {
                                                    if (fromdate.Month < 3)
                                                    {
                                                        newrow["Invoice No"] = from_branchCode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                                    }
                                                    else
                                                    {
                                                        newrow["Invoice No"] = from_branchCode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "T/" + DCNO;
                                                    }
                                                }
                                            }
                                            //newrow["Ledger Type"] = branch["sno"].ToSt    ring();
                                            newrow["HSN Code"] = branch["hsncode"].ToString();
                                            newrow["Item Name"] = branch["tproduct"].ToString();
                                            newrow["Qty"] = branch["Qty"].ToString();
                                            double rate = 0;
                                            double.TryParse(branch["unitprice"].ToString(), out rate);
                                            string tcategory = "";
                                            double tot_vatamount = 0;
                                            double PAmount = 0;
                                            double invval = 0;
                                            double qty = 0;
                                            double taxval = 0;
                                            double.TryParse(branch["qty"].ToString(), out qty);
                                            if (fromstateid == tostate)
                                            {
                                                double sgstamount = 0;
                                                double cgstamount = 0;
                                                double Igst = 0;
                                                double Igstamount = 0;
                                                double totRate = 0;
                                                double.TryParse(branch["Igst"].ToString(), out Igst);
                                                double Igstcon = 100 + Igst;
                                                Igstamount = (rate / Igstcon) * Igst;
                                                Igstamount = Math.Round(Igstamount, 2);
                                                totRate = Igstamount;
                                                tcategory = branch["tcategory"].ToString() + "-CGST/SGST-" + to_statecode;
                                                newrow["Ledger Type"] = tcategory.ToString();
                                                double Vatrate = rate - totRate;
                                                Vatrate = Math.Round(Vatrate, 2);
                                                newrow["Rate"] = Vatrate.ToString();
                                                PAmount = qty * Vatrate;
                                                newrow["Taxable Value"] = Math.Round(PAmount, 2);
                                                tot_vatamount = (PAmount * Igst) / 100;
                                                sgstamount = (tot_vatamount / 2);
                                                sgstamount = Math.Round(sgstamount, 2);
                                                newrow["sgst%"] = "'" + branch["sgst"].ToString();
                                                newrow["sgst amount"] = sgstamount.ToString();
                                                cgstamount = (tot_vatamount / 2);
                                                cgstamount = Math.Round(cgstamount, 2);
                                                newrow["cgst%"] = "'" + branch["cgst"].ToString();
                                                newrow["cgst amount"] = cgstamount.ToString();
                                                newrow["Igst%"] = "'" + 0;
                                                newrow["Igst amount"] = 0;
                                            }
                                            else
                                            {

                                                double Igst = 0;
                                                double Igstamount = 0;
                                                double totRate = 0;
                                                double.TryParse(branch["Igst"].ToString(), out Igst);
                                                double Igstcon = 100 + Igst;
                                                Igstamount = (rate / Igstcon) * Igst;
                                                Igstamount = Math.Round(Igstamount, 2);
                                                totRate = Igstamount;
                                                if (ddltype.SelectedValue == "AR Invoice")
                                                {
                                                    tcategory = "Sale of " + branch["Categoryname"].ToString() + " inter branches";
                                                }
                                                else
                                                {
                                                    tcategory = "Purchase of " + branch["Categoryname"].ToString() + " inter branches";

                                                }
                                                newrow["Ledger Type"] = tcategory.ToString();
                                                double Vatrate = rate - totRate;
                                                Vatrate = Math.Round(Vatrate, 2);
                                                newrow["Rate"] = Vatrate.ToString();
                                                PAmount = qty * Vatrate;
                                                newrow["Taxable Value"] = Math.Round(PAmount, 2);
                                                tot_vatamount = (PAmount * Igst) / 100;
                                                newrow["sgst%"] = "'" + 0;
                                                newrow["sgst amount"] = 0;
                                                newrow["cgst%"] = "'" + 0;
                                                newrow["cgst amount"] = 0;
                                                newrow["Igst%"] = "'" + branch["Igst"].ToString();
                                                tot_vatamount = Math.Round(tot_vatamount, 2);
                                                newrow["Igst amount"] = tot_vatamount.ToString();
                                            }
                                            invval = Math.Round(invval, 2);
                                            double netvalue = 0;
                                            netvalue = invval + taxval;
                                            netvalue = Math.Round(netvalue, 2);
                                            double tot_amount = PAmount + tot_vatamount;
                                            tot_amount = Math.Round(tot_amount, 2);
                                            newrow["Net Value"] = tot_amount;
                                            if (ddltype.SelectedValue == "AR Invoice")
                                            {
                                                newrow["Narration"] = "Being the sale of milk to  " + ddlSalesOffice.SelectedItem.Text + " vide invoice no " + branch["dcno"].ToString() + ",DC Date " + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                                            }
                                            else
                                            {
                                                newrow["Narration"] = "Being the purchase of milk to  " + ddlSalesOffice.SelectedItem.Text + " vide invoice no " + branch["dcno"].ToString() + ",DC Date " + fromdate.AddDays(1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                                            }
                                            Report.Rows.Add(newrow);
                                            i++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;
                }
                else
                {
                    if (Report.Rows.Count > 0)
                    {
                        grdReports.DataSource = Report;
                        grdReports.DataBind();
                        Session["xportdata"] = Report;
                    }
                    else
                    {
                        pnlHide.Visible = false;
                        lblmsg.Text = "No Indent Found";
                        grdReports.DataSource = Report;
                        grdReports.DataBind();
                    }
                }
            }
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
