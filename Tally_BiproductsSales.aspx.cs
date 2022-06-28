using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class Tally_BiproductsSales : System.Web.UI.Page
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
            PBranch.Visible = true;
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
        }
        else
        {
            PBranch.Visible = true;
            cmd = new MySqlCommand("SELECT BranchName, sno FROM branchdata WHERE (sno = @BranchID)");
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
            cmd = new MySqlCommand("SELECT  products_category.sno AS categoryid, branchdata.regtype, branchdata.tbranchname, branchdata_1.sno, branchdata.BranchName,branchdata.stateid, branchdata.sno AS BSno, indent.IndentType, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.UnitCost, productsdata.tproduct, productsdata.ProductName,productsdata.hsncode,productsdata.igst,productsdata.sgst,productsdata.cgst, productsdata.Units, productsdata.sno AS productsno, branchdata_1.SalesOfficeID, products_category.tcategory, branchproducts.VatPercent FROM (SELECT IndentNo, Branch_id, I_date, Status, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent INNER JOIN branchdata ON indent.Branch_id = branchdata.sno INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchproducts ON branchmappingtable.SuperBranch = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno WHERE (branchmappingtable.SuperBranch = @BranchID)  AND (indents_subtable.DeliveryQty <> 0) OR  (branchdata_1.SalesOfficeID = @SOID) AND (indents_subtable.DeliveryQty <> 0) GROUP BY productsdata.sno, BSno, branchmappingtable.SuperBranch, productsdata.igst ORDER BY branchdata.BranchName");
            if (Session["salestype"].ToString() == "Plant")
            {
                string BranchID = ddlSalesOffice.SelectedValue;
                if (BranchID == "572")
                {
                    BranchID = "7";
                }
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@SOID", BranchID);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
            }
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];

            //////cmd = new MySqlCommand("SELECT  products_category.sno AS categoryid, branchdata.tbranchname, branchdata.BranchName, branchdata.sno AS BSno, indent.IndentType,indents_subtable.DeliveryQty AS DeliveryQty, indents_subtable.UnitCost, productsdata.tproduct, productsdata.ProductName, productsdata.Units,productsdata.sno AS productsno, products_category.tcategory, branchproducts.VatPercent, addresstable.companyname AS tbranchname,addresstable.customercode FROM (SELECT IndentNo, Branch_id, I_date, Status, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent INNER JOIN branchdata ON indent.Branch_id = branchdata.sno INNER JOIN tripdata ON tripdata.BranchID = branchdata.sno INNER JOIN addresstable ON addresstable.sno = tripdata.to_adr_Id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchproducts ON branchdata.sno = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno WHERE  (indents_subtable.DeliveryQty <> 0) AND (branchdata.sno = @BranchID) OR  (indents_subtable.DeliveryQty <> 0) AND (branchdata.sno = @SOID) GROUP BY productsdata.sno, BSno, branchproducts.VatPercent ORDER BY branchdata.BranchName");
            //////cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            //////cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
            //////cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
            //////cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate));
            //////DataTable dtothers = vdm.SelectQuery(cmd).Tables[0];

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
            if (ddlsalestype.SelectedValue == "NonTax")
            {
                if (dtble.Rows.Count > 0)
                {
                    DataView view = new DataView(dtble);
                    Report = new DataTable();
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
                    int i = 1;
                    cmd = new MySqlCommand("SELECT branchdata.sno,branchdata.Branchcode,branchdata.companycode,  branchdata.BranchName,branchdata.stateid, statemastar.statename, statemastar.statecode , statemastar.gststatecode FROM branchdata INNER JOIN statemastar ON branchdata.stateid = statemastar.sno WHERE (branchdata.sno = @BranchID)");
                    if (Session["salestype"].ToString() == "Plant")
                    {
                        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                    }
                    DataTable dtstatename = vdm.SelectQuery(cmd).Tables[0];
                    string statename = "";
                    string statecode = "";
                    string fromstateid = "";
                    string Branchcode = "";
                    string gststatecode = "";
                    string companycode = "";
                    if (dtstatename.Rows.Count > 0)
                    {
                        Branchcode = dtstatename.Rows[0]["Branchcode"].ToString();
                        statename = dtstatename.Rows[0]["statename"].ToString();
                        statecode = dtstatename.Rows[0]["statecode"].ToString();
                        fromstateid = dtstatename.Rows[0]["stateid"].ToString();
                        gststatecode = dtstatename.Rows[0]["gststatecode"].ToString();
                        companycode = dtstatename.Rows[0]["companycode"].ToString();
                    }
                    foreach (DataRow branch in dtble.Rows)
                    {
                        if (branch["igst"].ToString() == "0")
                        {
                            DataRow newrow = Report.NewRow();
                            string DCNO = "0";
                            long DcNo = 0;
                            cmd = new MySqlCommand("SELECT agentdcno FROM  agentdc WHERE (BranchId = @BranchId) AND (IndDate BETWEEN @d1 AND @d2)");
                            cmd.Parameters.AddWithValue("@BranchId", branch["BSno"].ToString());
                            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                            DataTable dtDcnumber = vdm.SelectQuery(cmd).Tables[0];
                            string dcnumber = "";
                            if (dtDcnumber.Rows.Count > 0)
                            {
                                dcnumber = dtDcnumber.Rows[0]["agentdcno"].ToString();
                                DCNO = dcnumber.ToString();
                            }
                            else
                            {
                                //if (NoOfdays < 2)
                                //{
                                if (ddlSalesOffice.SelectedValue == "572" || ddlSalesOffice.SelectedValue == "3")
                                {
                                    ddlSalesOffice.SelectedValue = "7";
                                }
                                else if (ddlSalesOffice.SelectedValue == "4626")
                                {
                                    ddlSalesOffice.SelectedValue = "159";
                                }
                                cmd = new MySqlCommand("SELECT IFNULL(MAX(agentdcno), 0) + 1 AS Sno FROM agentdc WHERE (soid = @BranchId) AND (IndDate BETWEEN @d1 AND @d2)");
                                cmd.Parameters.AddWithValue("@BranchId", ddlSalesOffice.SelectedValue);
                                cmd.Parameters.AddWithValue("@d1", GetLowDate(dtapril).AddDays(-1));
                                cmd.Parameters.AddWithValue("@d2", GetHighDate(dtmarch).AddDays(-1));
                                DataTable dtadcno = vdm.SelectQuery(cmd).Tables[0];
                                string agentdcNo = dtadcno.Rows[0]["Sno"].ToString();
                                cmd = new MySqlCommand("Insert Into Agentdc (BranchId,IndDate,soid,agentdcno,stateid,companycode,moduleid,doe,invoicetype) Values(@BranchId,@IndDate,@soid,@agentdcno,@stateid,@companycode,@moduleid,@doe,@invoicetype)");
                                cmd.Parameters.AddWithValue("@BranchId", branch["BSno"].ToString());
                                cmd.Parameters.AddWithValue("@IndDate", GetLowDate(fromdate.AddDays(-1)));
                                cmd.Parameters.AddWithValue("@soid", ddlSalesOffice.SelectedValue);
                                cmd.Parameters.AddWithValue("@agentdcno", agentdcNo);
                                cmd.Parameters.AddWithValue("@stateid", gststatecode);
                                cmd.Parameters.AddWithValue("@companycode", companycode);
                                cmd.Parameters.AddWithValue("@doe", ReportDate);
                                cmd.Parameters.AddWithValue("@moduleid", Session["moduleid"].ToString());
                                cmd.Parameters.AddWithValue("@invoicetype", "TSales");
                                DcNo = vdm.insertScalar(cmd);
                                cmd = new MySqlCommand("SELECT agentdcno FROM  agentdc WHERE (BranchID = @BranchID) AND (IndDate BETWEEN @d1 AND @d2)");
                                cmd.Parameters.AddWithValue("@BranchID", branch["BSno"].ToString());
                                cmd.Parameters.AddWithValue("@soid", ddlSalesOffice.SelectedValue);
                                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                                DataTable dtsubDc = vdm.SelectQuery(cmd).Tables[0];
                                if (dtsubDc.Rows.Count > 0)
                                {
                                    DCNO = dtsubDc.Rows[0]["agentdcno"].ToString();
                                }
                                DCNO = DCNO.ToString();
                                //}
                            }
                            int countdc = 0;
                            int.TryParse(DCNO, out countdc);
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
                                    DCNO = Branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                }
                                else
                                {
                                    if (fromdate.AddDays(1).Month <= 3)
                                    {
                                        DCNO = Branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                    }
                                    else
                                    {
                                        DCNO = Branchcode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                                    }
                                }
                            }
                            else
                            {
                                if (fromdate.Month > 3)
                                {
                                    DCNO = Branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                }
                                else
                                {
                                    if (fromdate.Month <= 3)
                                    {
                                        DCNO = Branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                    }
                                    else
                                    {
                                        DCNO = Branchcode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                                    }
                                }
                            }
                            newrow["Customer Name"] = branch["tBranchName"].ToString();
                            newrow["Invoce No."] = DCNO;
                            if (ddlSalesOffice.SelectedValue == "306")
                            {
                                newrow["Invoice Date"] = fromdate.AddDays(1).ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                newrow["Invoice Date"] = fromdate.ToString("dd-MMM-yyyy");
                            }
                            newrow["HSN CODE"] = branch["hsncode"].ToString();
                            newrow["Item Name"] = branch["tProduct"].ToString();
                            double igst = 0;
                            double.TryParse(branch["igst"].ToString(), out igst);
                            double delqty = 0;
                            double.TryParse(branch["DeliveryQty"].ToString(), out delqty);
                            string tcategory = "";
                            newrow["Qty"] = branch["DeliveryQty"].ToString();
                            double UnitCost = 0;
                            double Unitprice = 0;
                            double.TryParse(branch["UnitCost"].ToString(), out UnitCost);
                            Unitprice = UnitCost;
                            double.TryParse(branch["igst"].ToString(), out igst);
                            float rate = 0;
                            double invval = 0;
                            double qty = 0;
                            double.TryParse(branch["DeliveryQty"].ToString(), out qty);
                            double taxval = 0;
                            float.TryParse(branch["UnitCost"].ToString(), out rate);
                            double tot_vatamount = 0;
                            double PAmount = 0;
                            string tostateid = branch["stateid"].ToString();
                            if (fromstateid == tostateid)
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
                                if (igst == null || igst == 0.0)
                                {
                                    tcategory = branch["tcategory"].ToString();
                                }
                                else
                                {
                                    tcategory = branch["tcategory"].ToString() + "-IGST";
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
                            newrow["Narration"] = "Being the sale of milk to  " + branch["tBranchName"].ToString() + " vide DC No " + DCNO + ",DC Date " + fromdate.ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                            Report.Rows.Add(newrow);
                            i++;
                            //}
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
                if (dtble.Rows.Count > 0)
                {
                    DataView view = new DataView(dtble);
                    Report = new DataTable();
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
                    int i = 1;
                    cmd = new MySqlCommand("SELECT branchdata.sno,branchdata.Branchcode,branchdata.companycode,  branchdata.BranchName,branchdata.stateid, statemastar.statename, statemastar.statecode , statemastar.gststatecode FROM branchdata INNER JOIN statemastar ON branchdata.stateid = statemastar.sno WHERE (branchdata.sno = @BranchID)");
                    if (Session["salestype"].ToString() == "Plant")
                    {
                        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                    }
                    DataTable dtstatename = vdm.SelectQuery(cmd).Tables[0];
                    string statename = "";
                    string statecode = "";
                    string fromstateid = "";
                    string Branchcode = "";
                    string gststatecode = "";
                    string companycode = "";
                    if (dtstatename.Rows.Count > 0)
                    {
                        Branchcode = dtstatename.Rows[0]["Branchcode"].ToString();
                        statename = dtstatename.Rows[0]["statename"].ToString();
                        statecode = dtstatename.Rows[0]["statecode"].ToString();
                        fromstateid = dtstatename.Rows[0]["stateid"].ToString();
                        gststatecode = dtstatename.Rows[0]["gststatecode"].ToString();
                        companycode = dtstatename.Rows[0]["companycode"].ToString();
                    }
                    foreach (DataRow branch in dtble.Rows)
                    {
                        if (branch["igst"].ToString() != "0")
                        {
                            DataRow newrow = Report.NewRow();
                            string DCNO = "0";
                            long DcNo = 0;

                            cmd = new MySqlCommand("SELECT agentdcno FROM  agenttaxdc WHERE (BranchId = @BranchId) AND (IndDate BETWEEN @d1 AND @d2)");
                            cmd.Parameters.AddWithValue("@BranchId", branch["BSno"].ToString());
                            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                            DataTable dtDcnumber = vdm.SelectQuery(cmd).Tables[0];
                            string dcnumber = "";
                            if (dtDcnumber.Rows.Count > 0)
                            {
                                dcnumber = dtDcnumber.Rows[0]["agentdcno"].ToString();
                                DCNO = dcnumber.ToString();
                            }
                            else
                            {
                                if (ddlSalesOffice.SelectedValue == "572" || ddlSalesOffice.SelectedValue == "3")
                                {
                                    ddlSalesOffice.SelectedValue = "7";
                                }
                                else if (ddlSalesOffice.SelectedValue == "4626")
                                {
                                    ddlSalesOffice.SelectedValue = "159";
                                }
                                //if (NoOfdays < 2)
                                //{
                                cmd = new MySqlCommand("SELECT IFNULL(MAX(agentdcno), 0) + 1 AS Sno FROM agenttaxdc WHERE (soid = @BranchId) AND (IndDate BETWEEN @d1 AND @d2)");
                                cmd.Parameters.AddWithValue("@BranchId", ddlSalesOffice.SelectedValue);
                                cmd.Parameters.AddWithValue("@d1", GetLowDate(dtapril).AddDays(-1));
                                cmd.Parameters.AddWithValue("@d2", GetHighDate(dtmarch).AddDays(-1));
                                DataTable dtadcno = vdm.SelectQuery(cmd).Tables[0];
                                string agentdcNo = dtadcno.Rows[0]["Sno"].ToString();
                                cmd = new MySqlCommand("Insert Into agenttaxdc (BranchId,IndDate,soid,agentdcno,stateid,companycode,moduleid,doe,invoicetype) Values(@BranchId,@IndDate,@soid,@agentdcno,@stateid,@companycode,@moduleid,@doe,@invoicetype)");
                                cmd.Parameters.AddWithValue("@BranchId", branch["BSno"].ToString());
                                cmd.Parameters.AddWithValue("@IndDate", GetLowDate(fromdate.AddDays(-1)));
                                cmd.Parameters.AddWithValue("@soid", ddlSalesOffice.SelectedValue);
                                cmd.Parameters.AddWithValue("@agentdcno", agentdcNo);
                                cmd.Parameters.AddWithValue("@stateid", gststatecode);
                                cmd.Parameters.AddWithValue("@companycode", companycode);
                                cmd.Parameters.AddWithValue("@doe", ReportDate);
                                cmd.Parameters.AddWithValue("@moduleid", Session["moduleid"].ToString());
                                cmd.Parameters.AddWithValue("@invoicetype", "TSales");
                                DcNo = vdm.insertScalar(cmd);
                                cmd = new MySqlCommand("SELECT agentdcno FROM  agenttaxdc WHERE (BranchID = @BranchID) AND (IndDate BETWEEN @d1 AND @d2)");
                                cmd.Parameters.AddWithValue("@BranchID", branch["BSno"].ToString());
                                cmd.Parameters.AddWithValue("@soid", ddlSalesOffice.SelectedValue);
                                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                                cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                                DataTable dtsubDc = vdm.SelectQuery(cmd).Tables[0];
                                if (dtsubDc.Rows.Count > 0)
                                {
                                    DCNO = dtsubDc.Rows[0]["agentdcno"].ToString();
                                }
                                DCNO = DCNO.ToString();
                                //}
                            }
                            int countdc = 0;
                            int.TryParse(DCNO, out countdc);
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
                                    DCNO = Branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;

                                }
                                else
                                {
                                    if (fromdate.AddDays(1).Month <= 3)
                                    {
                                        DCNO = Branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                    }
                                    else
                                    {
                                        DCNO = Branchcode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "T/" + DCNO;
                                    }
                                }
                            }
                            else
                            {
                                if (fromdate.Month > 3)
                                {
                                    DCNO = Branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                }
                                else
                                {
                                    if (fromdate.Month <= 3)
                                    {
                                        DCNO = Branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                    }
                                    else
                                    {
                                        DCNO = Branchcode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "T/" + DCNO;
                                    }
                                }
                            }
                           // DCNO = Branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                            newrow["Customer Name"] = branch["tBranchName"].ToString();
                            newrow["Invoce No."] = DCNO;
                            if (ddlSalesOffice.SelectedValue == "306")
                            {
                                newrow["Invoice Date"] = fromdate.AddDays(1).ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                newrow["Invoice Date"] = fromdate.ToString("dd-MMM-yyyy");
                            }
                            newrow["HSN CODE"] = branch["hsncode"].ToString();
                            newrow["Item Name"] = branch["tProduct"].ToString();
                            double igst = 0;
                            double.TryParse(branch["igst"].ToString(), out igst);
                            double delqty = 0;
                            double.TryParse(branch["DeliveryQty"].ToString(), out delqty);
                            string tcategory = "";
                            newrow["Qty"] = branch["DeliveryQty"].ToString();
                            double UnitCost = 0;
                            double Unitprice = 0;
                            double.TryParse(branch["UnitCost"].ToString(), out UnitCost);
                            Unitprice = UnitCost;
                            double.TryParse(branch["igst"].ToString(), out igst);
                            float rate = 0;
                            double invval = 0;
                            double qty = 0;
                            double.TryParse(branch["DeliveryQty"].ToString(), out qty);
                            double taxval = 0;
                            float.TryParse(branch["UnitCost"].ToString(), out rate);
                            double tot_vatamount = 0;
                            double PAmount = 0;
                            string tostateid = branch["stateid"].ToString();
                            string regtype = branch["regtype"].ToString();
                            if (fromstateid == tostateid)
                            {
                                if (regtype == "Special Economic Zone")
                                {
                                    double Igst = 0;
                                    double Igstamount = 0;
                                    double totRate = 0;
                                    double.TryParse(branch["Igst"].ToString(), out Igst);
                                    double Igstcon = 100 + Igst;
                                    Igstamount = (rate / Igstcon) * Igst;
                                    Igstamount = Math.Round(Igstamount, 2);
                                    totRate = Igstamount;
                                    if (igst == null || igst == 0.0)
                                    {
                                        tcategory = branch["tcategory"].ToString();
                                    }
                                    else
                                    {
                                        tcategory = branch["tcategory"].ToString() + "-IGST";
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
                                else
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
                                    if (igst == null || igst == 0.0)
                                    {
                                        tcategory = branch["tcategory"].ToString();
                                    }
                                    else
                                    {
                                        tcategory = branch["tcategory"].ToString() + "-CGST/SGST";
                                        if (tcategory == "Sale Of FM-CGST/SGST")
                                        {
                                            tcategory = "Sale Of FM-CGST/SGST 12%";
                                        }
                                    }
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
                                if (igst == null || igst == 0.0)
                                {
                                    tcategory = branch["tcategory"].ToString();
                                }
                                else
                                {
                                    tcategory = branch["tcategory"].ToString() + "-IGST";
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
                            newrow["Narration"] = "Being the sale of milk to  " + branch["tBranchName"].ToString() + " vide DC No " + DCNO + ",DC Date " + fromdate.ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                            Report.Rows.Add(newrow);
                            i++;
                            //}
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
