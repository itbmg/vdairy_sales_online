using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class tallyscheemsales : System.Web.UI.Page
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
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        FillRouteName();
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
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchdata.flag = @flag) AND (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchdata.flag = @flag) AND (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
            cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
            cmd.Parameters.AddWithValue("@SalesType", "21");
            cmd.Parameters.AddWithValue("@SalesType1", "26");
            cmd.Parameters.AddWithValue("@flag", "1");
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in dtRoutedata.Rows)
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
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.flag = @flag) AND (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.flag = @flag) AND (branchdata.sno = @BranchID) AND (branchdata.SalesType IS NOT NULL)");
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
            Report = new DataTable();
            Session["RouteName"] = ddlSalesOffice.SelectedItem.Text;
            Session["xporttype"] = "TallySales";
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
            Session["filename"] = ddlSalesOffice.SelectedItem.Text + " Tally Sales " + fromdate.ToString("dd/MM/yyyy");

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
            TimeSpan datespan = ReportDate.Subtract(fromdate);
            int NoOfdays = datespan.Days;
            Report = new DataTable();
            if (ddltaxtype.SelectedValue == "Non Tax")
            {
                Report.Columns.Add("Ledger Type");
                Report.Columns.Add("Customer Name");
                Report.Columns.Add("Invoce No.");
                Report.Columns.Add("Invoice Date");
                Report.Columns.Add("HSN CODE");
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
               
                cmd = new MySqlCommand("SELECT branchdata.sno,branchdata.Branchcode,branchdata.companycode, branchdata.incentivename, branchdata.BranchName,branchdata.stateid, statemastar.statename, statemastar.statecode , statemastar.gststatecode FROM branchdata INNER JOIN statemastar ON branchdata.stateid = statemastar.sno WHERE (branchdata.sno = @BranchID)");
                if (Session["salestype"].ToString() == "Plant")
                {
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                }
                DataTable dtstate = vdm.SelectQuery(cmd).Tables[0];
                string statename = "";
                string branchcode = "";
                string statecode = "";
                string fromstateid = "";
                string gststatecode = "";
                string companycode = "";
                if (dtstate.Rows.Count > 0)
                {
                    statename = dtstate.Rows[0]["statename"].ToString();
                    branchcode = dtstate.Rows[0]["branchcode"].ToString();
                    statecode = dtstate.Rows[0]["statecode"].ToString();
                    fromstateid = dtstate.Rows[0]["stateid"].ToString();
                    gststatecode = dtstate.Rows[0]["gststatecode"].ToString();
                    companycode = dtstate.Rows[0]["companycode"].ToString();
                }
                cmd = new MySqlCommand("SELECT  dispatch.DispName, dispatch.sno, dispatch.BranchID, tripdata.I_Date,tripdata.dcno, tripdata.Sno AS TripSno, dispatch.DispMode, branchmappingtable.SuperBranch, triproutes.Tripdata_sno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN branchmappingtable ON dispatch.BranchID = branchmappingtable.SubBranch WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2)and (dispatch.DispType='SO') and (tripdata.Status<>'C') OR (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @SuperBranch)  and (dispatch.DispType='SO')and (tripdata.Status<>'C') GROUP BY tripdata.Sno ORDER BY dispatch.sno");
                cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtDispnames = vdm.SelectQuery(cmd).Tables[0];
                string DCNO = "";
                foreach (DataRow dr in dtDispnames.Rows)
                {
                    string soid = "";

                    cmd = new MySqlCommand("SELECT SUM(offer_indents_sub.offer_delivered_qty) AS FreeMilk, productsdata.tproduct, branchproducts.unitprice, offer_indents_sub.invoiceno, products_category.tcategory, productsdata.hsncode, productsdata.igst, productsdata.cgst, productsdata.sgst FROM offer_indents_sub INNER JOIN tripdata ON offer_indents_sub.DTripId = tripdata.Sno INNER JOIN  productsdata ON offer_indents_sub.product_id = productsdata.sno INNER JOIN   branchproducts ON productsdata.sno = branchproducts.product_sno INNER JOIN    products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN  products_category ON products_subcategory.category_sno = products_category.sno  WHERE        (tripdata.ATripid = @TripID) AND (branchproducts.branch_sno = @BranchID) AND (productsdata.igst = 0)  GROUP BY productsdata.tproduct, products_category.tcategory");
                    cmd.Parameters.AddWithValue("@TripID", dr["TripSno"].ToString());
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    DataTable Dtoffermilk = vdm.SelectQuery(cmd).Tables[0];
                    DataTable newdt = new DataTable();
                    newdt = Dtoffermilk.Copy();
                    int new_countdc = 0;
                    if (newdt.Rows.Count > 0)
                    {
                        int.TryParse(newdt.Rows[0]["invoiceno"].ToString(), out new_countdc);
                    }
                    if (new_countdc > 0)
                    {
                    }
                    else
                    {

                        if (Dtoffermilk.Rows.Count > 0)
                        {
                            
                            double freeqty = 0; 
                            foreach (DataRow branch in newdt.Rows)
                            {
                                //Free
                                if (branch["sgst"].ToString() == "0")
                                {
                                    double Qty = 0;
                                    double.TryParse(branch["FreeMilk"].ToString(), out Qty);
                                    freeqty += Qty;
                                }
                            }
                            if (freeqty > 0)
                            {
                                cmd = new MySqlCommand("SELECT IFNULL(MAX(agentdcno), 0) + 1 AS Sno FROM agentdc WHERE (soid = @soid)  AND (IndDate BETWEEN @d1 AND @d2)");
                                cmd.Parameters.AddWithValue("@soid", soid);
                                cmd.Parameters.AddWithValue("@d1", GetLowDate(dtapril));
                                cmd.Parameters.AddWithValue("@d2", GetHighDate(dtmarch));
                                DataTable dtadcno = vdm.SelectQuery(cmd).Tables[0];
                                string invno = dtadcno.Rows[0]["Sno"].ToString();
                                cmd = new MySqlCommand("Insert Into Agentdc (BranchId,IndDate,agentdcno,soid,stateid,companycode,moduleid,doe,invoicetype) Values(@BranchId,@IndDate,@agentdcno,@soid,@stateid,@companycode,@moduleid,@doe,@invoicetype)");
                                cmd.Parameters.AddWithValue("@BranchId", dr["sno"].ToString());
                                cmd.Parameters.AddWithValue("@IndDate", fromdate.AddDays(-1));
                                cmd.Parameters.AddWithValue("@agentdcno", invno);
                                cmd.Parameters.AddWithValue("@soid", soid);
                                cmd.Parameters.AddWithValue("@stateid", gststatecode);
                                cmd.Parameters.AddWithValue("@companycode", companycode);
                                cmd.Parameters.AddWithValue("@doe", ReportDate);
                                cmd.Parameters.AddWithValue("@moduleid", "4");// Module 4 is Credit Note (Ex...Leaks)
                                cmd.Parameters.AddWithValue("@invoicetype", "TOSales");
                                vdm.insert(cmd);
                                cmd = new MySqlCommand("UPDATE offer_indents_sub t1 JOIN tripdata t2 ON t1.DTripID=t2.Sno JOIN productsdata as t3 ON t3.sno=t1.product_id SET  t1.invoiceno = @invoiceno WHERE t2.ATripid=@TripID and  t3.igst=0");
                                //cmd = new MySqlCommand("UPDATE leakages t1 JOIN tripdata t2 ON t1.TripID=t2 .Sno SET  t1.Invoiceno = @invoiceno WHERE t2 .ATripid=@TripID");
                                cmd.Parameters.AddWithValue("@invoiceno", invno);
                                cmd.Parameters.AddWithValue("@TripID", dr["TripSno"].ToString());
                                vdm.Update(cmd);
                                int.TryParse(invno, out new_countdc);
                            }
                        }
                    }
                    DCNO = "";
                    if (new_countdc <= 10)
                    {
                        DCNO = "0000" + new_countdc;
                    }
                    if (new_countdc >= 10 && new_countdc <= 99)
                    {
                        DCNO = "000" + new_countdc;
                    }
                    if (new_countdc >= 99 && new_countdc <= 999)
                    {
                        DCNO = "00" + new_countdc;
                    }
                    if (new_countdc > 999)
                    {
                        DCNO = "0" + new_countdc;
                    }
                    if (ddlSalesOffice.SelectedValue == "306")
                    {
                        if (fromdate.AddDays(1).Month > 3)
                        {
                            DCNO = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                        }
                        else
                        {
                            if (fromdate.AddDays(1).Month < 3)
                            {
                                DCNO = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                            }
                            else
                            {
                                DCNO = branchcode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                            }
                        }
                    }
                    else
                    {

                        if (fromdate.Month > 3)
                        {
                            DCNO = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                        }
                        else
                        {
                            if (fromdate.Month < 3)
                            {
                                DCNO = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                            }
                            else
                            {
                                DCNO = branchcode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                            }
                        }
                        //if (fromdate.Month > 3)
                        //{
                        //    DCNO = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                        //}
                        //else
                        //{
                        //    DCNO = branchcode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                        //}
                    }
                    foreach (DataRow branch in newdt.Rows)
                    {
                        if (branch["sgst"].ToString() == "0")
                        {
                            //Free
                            DataRow newrow = Report.NewRow();
                            double Qty = 0;
                            double.TryParse(branch["FreeMilk"].ToString(), out Qty);

                            double freeqty = 0;
                            freeqty += Qty;
                            if (Qty == 0.0)
                            {
                            }
                            else
                            {
                                double rate = 0;
                                double.TryParse(branch["unitprice"].ToString(), out rate);
                                Qty = Math.Round(Qty, 2);
                                newrow["Customer Name"] = "Scheme Milk-" + dtstate.Rows[0]["incentivename"].ToString();
                                newrow["Invoce No."] = DCNO;
                                if (ddlSalesOffice.SelectedValue == "306")
                                {
                                    newrow["Invoice Date"] = fromdate.AddDays(1).ToString("dd-MMM-yyyy");
                                }
                                else
                                {
                                    newrow["Invoice Date"] = fromdate.ToString("dd-MMM-yyyy");
                                }
                                newrow["Item Name"] = branch["tproduct"].ToString();
                                newrow["HSN CODE"] = branch["hsncode"].ToString();
                                newrow["Qty"] = Qty;
                                newrow["Rate"] = rate;
                                double invval = 0;
                                double sgstamount = 0;
                                double cgstamount = 0;
                                double taxval = 0;
                                double Igst = 0;
                                double Igstamount = 0;
                                double totRate = 0;
                                double tot_vatamount = 0;
                                double PAmount = 0;
                                double.TryParse(branch["Igst"].ToString(), out Igst);
                                double Igstcon = 100 + Igst;
                                Igstamount = (rate / Igstcon) * Igst;
                                Igstamount = Math.Round(Igstamount, 2);
                                totRate = Igstamount;
                                double igst = 0;
                                double.TryParse(branch["igst"].ToString(), out igst);
                                string tcategory = "";
                                if (igst == null || igst == 0.0)
                                {
                                    tcategory = branch["tcategory"].ToString();
                                }
                                else
                                {
                                    tcategory = branch["tcategory"].ToString() + "-CGST/SGST";
                                }
                                newrow["Ledger Type"] = tcategory.ToString();
                                double Vatrate = rate - totRate;
                                Vatrate = Math.Round(Vatrate, 2);
                                newrow["Rate"] = Vatrate.ToString();
                                PAmount = Qty * Vatrate;
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
                               // newrow["ProductID"] = branch["ProductID"].ToString();
                                invval = Math.Round(invval, 2);
                                double netvalue = 0;
                                netvalue = invval + taxval;
                                netvalue = Math.Round(netvalue, 2);
                                double tot_amount = PAmount + tot_vatamount;
                                tot_amount = Math.Round(tot_amount, 2);
                                newrow["Net Value"] = tot_amount;
                                newrow["Narration"] = "Being the Sale Of Milk Through " + ddlSalesOffice.SelectedItem.Text + ". This is Scheme Milk Vide JV No " + dr["dcno"].ToString() + ",Emp Name  " + Session["EmpName"].ToString();
                                Report.Rows.Add(newrow);
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
                Report.Columns.Add("Ledger Type");
                Report.Columns.Add("Customer Name");
                Report.Columns.Add("Invoce No.");
                Report.Columns.Add("Invoice Date");
                Report.Columns.Add("HSN CODE");
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
                cmd = new MySqlCommand("SELECT branchdata.sno,branchdata.Branchcode,branchdata.companycode, branchdata.incentivename, branchdata.BranchName,branchdata.stateid, statemastar.statename, statemastar.statecode , statemastar.gststatecode FROM branchdata INNER JOIN statemastar ON branchdata.stateid = statemastar.sno WHERE (branchdata.sno = @BranchID)");
                if (Session["salestype"].ToString() == "Plant")
                {
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                }
                DataTable dtstate = vdm.SelectQuery(cmd).Tables[0];
                string statename = "";
                string branchcode = "";
                string statecode = "";
                string fromstateid = "";
                string gststatecode = "";
                string companycode = "";
                if (dtstate.Rows.Count > 0)
                {
                    statename = dtstate.Rows[0]["statename"].ToString();
                    branchcode = dtstate.Rows[0]["branchcode"].ToString();
                    statecode = dtstate.Rows[0]["statecode"].ToString();
                    fromstateid = dtstate.Rows[0]["stateid"].ToString();
                    gststatecode = dtstate.Rows[0]["gststatecode"].ToString();
                    companycode = dtstate.Rows[0]["companycode"].ToString();
                }
                cmd = new MySqlCommand("SELECT  dispatch.DispName, dispatch.sno, dispatch.BranchID, tripdata.I_Date,tripdata.dcno, tripdata.Sno AS TripSno, dispatch.DispMode, branchmappingtable.SuperBranch, triproutes.Tripdata_sno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN branchmappingtable ON dispatch.BranchID = branchmappingtable.SubBranch WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2)and (dispatch.DispType='SO') and (tripdata.Status<>'C') OR (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @SuperBranch)  and (dispatch.DispType='SO')and (tripdata.Status<>'C') GROUP BY tripdata.Sno ORDER BY dispatch.sno");
                cmd.Parameters.AddWithValue("@SuperBranch", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                DataTable dtDispnames = vdm.SelectQuery(cmd).Tables[0];
                string DCNO = "";
                foreach (DataRow dr in dtDispnames.Rows)
                {
                    string soid = "";
                    cmd = new MySqlCommand("SELECT  SUM(leakages.ShortQty) AS ShortQty, SUM(leakages.FreeMilk) AS FreeMilk, productsdata.tproduct, branchproducts.unitprice, leakages.invoiceno, products_category.tcategory, productsdata.hsncode, productsdata.igst, productsdata.cgst, productsdata.sgst FROM leakages INNER JOIN tripdata ON leakages.TripID = tripdata.Sno INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata.ATripid = @TripID) AND (branchproducts.branch_sno = @BranchID) AND (productsdata.igst > 0) GROUP BY productsdata.tproduct, products_category.tcategory");
                    //cmd = new MySqlCommand("SELECT SUM(leakages.ShortQty) AS ShortQty, SUM(leakages.FreeMilk) AS FreeMilk, productsdata.tproduct, branchproducts.unitprice, leakages.invoiceno, products_category.tcategory,productsdata.hsncode, productsdata.igst, productsdata.cgst, productsdata.sgst FROM leakages INNER JOIN tripdata ON leakages.TripID = tripdata.Sno INNER JOIN productsdata ON leakages.ProductID = productsdata.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata.ATripid = @TripID) AND (branchproducts.branch_sno = @BranchID) GROUP BY productsdata.tproduct, products_category.tcategory");
                    cmd.Parameters.AddWithValue("@TripID", dr["TripSno"].ToString());
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    DataTable Dtfreemilk = vdm.SelectQuery(cmd).Tables[0];
                    cmd = new MySqlCommand("SELECT branchleaktrans.ShortQty, branchleaktrans.FreeQty AS FreeMilk, productsdata.tproduct, branchproducts.unitprice, branchleaktrans.invoiceno, products_category.tcategory, productsdata.hsncode,productsdata.igst, productsdata.cgst, productsdata.sgst FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN branchleaktrans ON productsdata.sno = branchleaktrans.ProdId INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID) AND (branchleaktrans.TripId = @TripID) AND (productsdata.igst > 0) GROUP BY productsdata.tproduct, products_category.tcategory");
                    cmd.Parameters.AddWithValue("@TripID", dr["TripSno"].ToString());
                    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    DataTable dtsalesofficeshortfree = vdm.SelectQuery(cmd).Tables[0];
                    DataTable newdt = new DataTable();
                    newdt = Dtfreemilk.Copy();
                    newdt.Merge(dtsalesofficeshortfree, true, MissingSchemaAction.Ignore);
                    int new_countdc = 0;
                    if (newdt.Rows.Count > 0)
                    {
                        int.TryParse(newdt.Rows[0]["invoiceno"].ToString(), out new_countdc);
                    }
                    if (new_countdc > 0)
                    {
                    }
                    else
                    {
                        if (newdt.Rows.Count > 0)
                        {
                            
                            double freeqty = 0; 
                            foreach (DataRow branch in newdt.Rows)
                            {
                                //Free
                                if (branch["sgst"].ToString() != "0")
                                {
                                    double Qty = 0;
                                    double.TryParse(branch["FreeMilk"].ToString(), out Qty);
                                    freeqty += Qty;
                                }
                            }
                            if (freeqty > 0)
                            {
                                cmd = new MySqlCommand("SELECT IFNULL(MAX(agentdcno), 0) + 1 AS Sno FROM agenttaxdc WHERE (soid = @soid)  AND (IndDate BETWEEN @d1 AND @d2)");
                                cmd.Parameters.AddWithValue("@soid", soid);
                                cmd.Parameters.AddWithValue("@d1", GetLowDate(dtapril));
                                cmd.Parameters.AddWithValue("@d2", GetHighDate(dtmarch));
                                DataTable dtadcno = vdm.SelectQuery(cmd).Tables[0];
                                string invno = dtadcno.Rows[0]["Sno"].ToString();
                                cmd = new MySqlCommand("Insert Into agenttaxdc (BranchId,IndDate,agentdcno,soid,stateid,companycode,moduleid,doe,invoicetype) Values(@BranchId,@IndDate,@agentdcno,@soid,@stateid,@companycode,@moduleid,@doe,@invoicetype)");
                                cmd.Parameters.AddWithValue("@BranchId", dr["sno"].ToString());
                                cmd.Parameters.AddWithValue("@IndDate", fromdate.AddDays(-1));
                                cmd.Parameters.AddWithValue("@agentdcno", invno);
                                cmd.Parameters.AddWithValue("@soid", soid);
                                cmd.Parameters.AddWithValue("@stateid", gststatecode);
                                cmd.Parameters.AddWithValue("@companycode", companycode);
                                cmd.Parameters.AddWithValue("@doe", ReportDate);
                                cmd.Parameters.AddWithValue("@moduleid", "4");// Module 4 is Credit Note (Ex...Leaks)
                                cmd.Parameters.AddWithValue("@invoicetype", "TOSales");
                                vdm.insert(cmd);
                                cmd = new MySqlCommand("UPDATE leakages t1 JOIN tripdata t2 ON t1.TripID=t2.Sno JOIN productsdata as t3 ON t3.sno=t1.ProductID SET  t1.Invoiceno = @invoiceno WHERE t2.ATripid=@TripID and  t3.igst>0");
                                //cmd = new MySqlCommand("UPDATE leakages t1 JOIN tripdata t2 ON t1.TripID=t2.Sno JOIN productsdata as t3 ON t3.productid=t1.productid ON  SET  t1.Invoiceno = @invoiceno WHERE t2 .ATripid=@TripID and t3.igst>0)");
                                cmd.Parameters.AddWithValue("@invoiceno", invno);
                                cmd.Parameters.AddWithValue("@TripID", dr["TripSno"].ToString());
                                vdm.Update(cmd);
                                cmd = new MySqlCommand("Update branchleaktrans t1 join productsdata AS t2 ON t1.ProdId=t2.sno  set t1.invoiceno=@invoiceno where t1.TripID=@TripID and  t2.igst>0");
                                cmd.Parameters.AddWithValue("@invoiceno", invno);
                                cmd.Parameters.AddWithValue("@TripID", dr["TripSno"].ToString());
                                vdm.Update(cmd);
                                int.TryParse(invno, out new_countdc);
                            }
                        }
                    }
                    DCNO = "";
                    if (new_countdc <= 10)
                    {
                        DCNO = "0000" + new_countdc;
                    }
                    if (new_countdc >= 10 && new_countdc <= 99)
                    {
                        DCNO = "000" + new_countdc;
                    }
                    if (new_countdc >= 99 && new_countdc <= 999)
                    {
                        DCNO = "00" + new_countdc;
                    }
                    if (new_countdc > 999)
                    {
                        DCNO = "0" + new_countdc;
                    }
                    if (ddlSalesOffice.SelectedValue == "306")
                    {
                        if (fromdate.AddDays(1).Month > 3)
                        {
                            DCNO = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                        }
                        else
                        {
                            if (fromdate.AddDays(1).Month < 3)
                            {
                                DCNO = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                            }
                            else
                            {
                                DCNO = branchcode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "T/" + DCNO;
                            }
                        }
                    }
                    else
                    {

                        if (fromdate.Month > 3)
                        {
                            DCNO = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                        }
                        else
                        {
                            if (fromdate.Month < 3)
                            {
                                DCNO = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                            }
                            else
                            {
                                DCNO = branchcode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "T/" + DCNO;
                            }
                        }
                    }
                    foreach (DataRow branch in newdt.Rows)
                    {
                        //Free
                        if (branch["sgst"].ToString() != "0")
                        {
                            DataRow newrow = Report.NewRow();
                            double Qty = 0;
                            double.TryParse(branch["FreeMilk"].ToString(), out Qty);
                            double freeqty = 0;
                            freeqty += Qty;
                            if (Qty == 0.0)
                            {
                            }
                            else
                            {
                                double rate = 0;
                                double.TryParse(branch["unitprice"].ToString(), out rate);
                                Qty = Math.Round(Qty, 2);
                                newrow["Customer Name"] = "Free Sales-Milk-" + dtstate.Rows[0]["incentivename"].ToString();
                                newrow["Invoce No."] = DCNO;
                                if (ddlSalesOffice.SelectedValue == "306")
                                {
                                    newrow["Invoice Date"] = fromdate.AddDays(1).ToString("dd-MMM-yyyy");
                                }
                                else
                                {
                                    newrow["Invoice Date"] = fromdate.ToString("dd-MMM-yyyy");
                                }
                                newrow["Item Name"] = branch["tproduct"].ToString();
                                newrow["HSN CODE"] = branch["hsncode"].ToString();
                                newrow["Qty"] = Qty;
                                newrow["Rate"] = rate;
                                double invval = 0;
                                double sgstamount = 0;
                                double cgstamount = 0;
                                double taxval = 0;
                                double Igst = 0;
                                double Igstamount = 0;
                                double totRate = 0;
                                double tot_vatamount = 0;
                                double PAmount = 0;
                                double.TryParse(branch["Igst"].ToString(), out Igst);
                                double Igstcon = 100 + Igst;
                                Igstamount = (rate / Igstcon) * Igst;
                                Igstamount = Math.Round(Igstamount, 2);
                                totRate = Igstamount;
                                double igst = 0;
                                double.TryParse(branch["igst"].ToString(), out igst);
                                string tcategory = "";
                                if (igst == null || igst == 0.0)
                                {
                                    tcategory = branch["tcategory"].ToString();
                                }
                                else
                                {
                                    tcategory = branch["tcategory"].ToString() + "-CGST/SGST";
                                }
                                newrow["Ledger Type"] = tcategory.ToString();
                                double Vatrate = rate - totRate;
                                Vatrate = Math.Round(Vatrate, 2);
                                newrow["Rate"] = Vatrate.ToString();
                                PAmount = Qty * Vatrate;
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
                                invval = Math.Round(invval, 2);
                                double netvalue = 0;
                                netvalue = invval + taxval;
                                netvalue = Math.Round(netvalue, 2);
                                double tot_amount = PAmount + tot_vatamount;
                                tot_amount = Math.Round(tot_amount, 2);
                                newrow["Net Value"] = tot_amount;
                                newrow["Narration"] = "Being the Sale Of Milk Through " + ddlSalesOffice.SelectedItem.Text + ". This is Free Milk Vide JV No " + dr["dcno"].ToString() + ",Emp Name  " + Session["EmpName"].ToString();
                                Report.Rows.Add(newrow);
                            }
                        }
                    }
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
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