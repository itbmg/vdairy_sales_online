using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

public partial class TallyCash_staffsales : System.Web.UI.Page
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
        ddlSalesOffice.Items.Insert(0, new ListItem("Cash Sale", "0"));
        ddlSalesOffice.Items.Insert(0, new ListItem("Staff Sale", "1"));
        ddlSalesOffice.Items.Insert(0, new ListItem("Free Sale", "2"));
        ddlSalesOffice.Items.Insert(0, new ListItem("Others", "3"));
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
            Session["xporttype"] = "TallyCashStaff";
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
            Session["filename"] = ddlSalesOffice.SelectedItem.Text + " Tally Inward" + fromdate.ToString("dd/MM/yyyy");
            if (ddlSalesOffice.SelectedItem.Text == "Cash Sale" || ddlSalesOffice.SelectedItem.Text == "Staff Sale" || ddlSalesOffice.SelectedItem.Text == "Free Sale")
            {
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
                if (ddlsalestype.SelectedValue == "NonTax")
                {
                    cmd = new MySqlCommand("SELECT branchdata.sno,branchdata.branchcode, branchdata.BranchName, statemastar.statename ,statemastar.statecode FROM branchdata INNER JOIN statemastar ON branchdata.stateid = statemastar.sno WHERE (branchdata.sno = @BranchID)");
                    if (Session["salestype"].ToString() == "Plant")
                    {
                        cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                    }
                    DataTable dtstatename = vdm.SelectQuery(cmd).Tables[0];
                    string statename = "";
                    string statecode = "";
                    string branchcode = "";
                    if (dtstatename.Rows.Count > 0)
                    {
                        statename = dtstatename.Rows[0]["statename"].ToString();
                        statecode = dtstatename.Rows[0]["statecode"].ToString();
                        branchcode = dtstatename.Rows[0]["branchcode"].ToString();
                    }
                    if (ddlSalesOffice.SelectedItem.Text == "Cash Sale")
                    {
                        cmd = new MySqlCommand("SELECT TripInfo.Sno, TripInfo.DCNo,TripInfo.taxdcno, ProductInfo.tproduct,ProductInfo.hsncode,ProductInfo.igst,ProductInfo.cgst,ProductInfo.sgst, ProductInfo.Prodsno, ProductInfo.Categoryname,ProductInfo.categoryid, ProductInfo.Qty, TripInfo.I_Date, TripInfo.VehicleNo, TripInfo.Status, TripInfo.DispName, TripInfo.DispType, TripInfo.DispMode FROM (SELECT tripdata.Sno, tripdata.DCNo,tripdata.taxdcno, tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, dispatch.DispType, dispatch.DispMode FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.DispMode = 'LOCAL') AND (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2)) TripInfo INNER JOIN (SELECT Categoryname,categoryid, ProductName, Sno, Qty, tproduct,Prodsno,hsncode,igst,sgst,cgst FROM  (SELECT products_category.Categoryname,products_category.sno as categoryid, productsdata.ProductName, tripdata_1.Sno, tripsubdata.Qty,productsdata.hsncode,productsdata.igst,productsdata.cgst,productsdata.sgst, productsdata.tproduct,productsdata.sno as Prodsno FROM tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata_1.AssignDate BETWEEN @d1 AND @d2)) TripSubInfo) ProductInfo ON TripInfo.Sno = ProductInfo.Sno");
                    }
                    else if (ddlSalesOffice.SelectedItem.Text == "Staff Sale")
                    {
                        cmd = new MySqlCommand("SELECT TripInfo.Sno, TripInfo.DCNo,TripInfo.taxdcno, ProductInfo.tproduct,ProductInfo.hsncode,ProductInfo.igst,ProductInfo.cgst,ProductInfo.sgst, ProductInfo.Prodsno, ProductInfo.Categoryname,ProductInfo.categoryid, ProductInfo.Qty, TripInfo.I_Date, TripInfo.VehicleNo, TripInfo.Status, TripInfo.DispName, TripInfo.DispType, TripInfo.DispMode FROM (SELECT tripdata.Sno, tripdata.DCNo,tripdata.taxdcno, tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, dispatch.DispType, dispatch.DispMode FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.DispMode = 'Staff') AND (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2)) TripInfo INNER JOIN (SELECT Categoryname,categoryid, ProductName, Sno, Qty, tproduct,Prodsno,hsncode,igst,sgst,cgst FROM  (SELECT products_category.Categoryname,products_category.sno as categoryid, productsdata.ProductName, tripdata_1.Sno, tripsubdata.Qty,productsdata.hsncode,productsdata.igst,productsdata.cgst,productsdata.sgst, productsdata.tproduct,productsdata.sno as Prodsno FROM tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata_1.AssignDate BETWEEN @d1 AND @d2)) TripSubInfo) ProductInfo ON TripInfo.Sno = ProductInfo.Sno");
                    }
                    else if (ddlSalesOffice.SelectedItem.Text == "Free Sale")
                    {
                        cmd = new MySqlCommand("SELECT  TripInfo.Sno, TripInfo.DCNo,TripInfo.taxdcno, ProductInfo.tproduct, ProductInfo.hsncode, ProductInfo.igst, ProductInfo.cgst, ProductInfo.sgst, ProductInfo.Prodsno, ProductInfo.Categoryname,ProductInfo.categoryid, ProductInfo.Qty, TripInfo.I_Date, TripInfo.VehicleNo, TripInfo.Status, TripInfo.DispName, TripInfo.DispType, TripInfo.DispMode FROM (SELECT tripdata.Sno, tripdata.DCNo,tripdata.taxdcno, tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, dispatch.DispType, dispatch.DispMode FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.DispMode = 'Free') AND (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2)) TripInfo INNER JOIN (SELECT Categoryname,categoryid, ProductName, Sno, Qty, tproduct, Prodsno, hsncode, igst, sgst, cgst FROM (SELECT products_category.Categoryname, products_category.sno as categoryid,productsdata.ProductName, tripdata_1.Sno, tripsubdata.Qty, productsdata.hsncode, productsdata.igst,productsdata.cgst, productsdata.sgst, productsdata.tproduct, productsdata.sno AS Prodsno FROM  tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata_1.AssignDate BETWEEN @d1 AND @d2)) TripSubInfo) ProductInfo ON TripInfo.Sno = ProductInfo.Sno");
                    }
                    cmd.Parameters.AddWithValue("@branch", Session["branch"]);
                    cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate).AddDays(-1));
                    DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
                    cmd = new MySqlCommand("SELECT branchproducts.branch_sno, branchproducts.product_sno, branchproducts.unitprice, branchproducts.VatPercent, products_category.tcategory FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID)");
                    if (ddlSalesOffice.SelectedItem.Text == "Cash Sale" || ddlSalesOffice.SelectedItem.Text == "Free Sale" || ddlSalesOffice.SelectedItem.Text == "Others")
                    {
                        cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                    }
                    else
                    {

                        if (Session["branch"].ToString() == "7")
                        {
                            string barnch = "702";
                            cmd.Parameters.AddWithValue("@BranchID", barnch);
                        }
                        else
                        {
                            string barnch = "760";
                            cmd.Parameters.AddWithValue("@BranchID", barnch);
                        }
                    }
                    DataTable dtprodcts = vdm.SelectQuery(cmd).Tables[0];
                    int i = 1;
                    foreach (DataRow branch in dtble.Rows)
                    {
                        if (branch["igst"].ToString() == "0")
                        {
                            DataRow newrow = Report.NewRow();
                            if (ddlSalesOffice.SelectedItem.Text == "Staff Sale")
                            {
                                newrow["Customer Name"] = branch["DispName"].ToString();
                            }
                            else if (ddlSalesOffice.SelectedItem.Text == "Free Sale")
                            {
                                newrow["Customer Name"] = branch["DispName"].ToString();
                            }
                            else if (ddlSalesOffice.SelectedItem.Text == "Others")
                            {
                                newrow["Customer Name"] = branch["DispName"].ToString();
                            }
                            else
                            {
                                newrow["Customer Name"] = "Cash sales-" + branchcode;
                            }
                            newrow["HSN CODE"] = branch["hsncode"].ToString();
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


                            if (fromdate.Month > 3)
                            {
                                newrow["Invoce No."] = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                            }
                            else
                            {
                                if (fromdate.Month < 3)
                                {
                                    newrow["Invoce No."] = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                }
                                else
                                {
                                    newrow["Invoce No."] = branchcode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                                }
                            }

                            newrow["Invoice Date"] = fromdate.AddDays(-1).ToString("dd-MMM-yyyy");
                            newrow["Item Name"] = branch["tproduct"].ToString();
                            newrow["Qty"] = branch["qty"].ToString();
                            double UnitCost = 0;
                            double Unitprice = 0;
                            string tcategory = "";
                            double igst = 0;
                            double.TryParse(branch["igst"].ToString(), out igst);
                            double cgst = 0;
                            double.TryParse(branch["cgst"].ToString(), out cgst);
                            double sgst = 0;
                            double.TryParse(branch["sgst"].ToString(), out sgst);
                            foreach (DataRow dr in dtprodcts.Select("product_sno='" + branch["Prodsno"].ToString() + "'"))
                            {
                                double.TryParse(dr["unitprice"].ToString(), out UnitCost);
                                if (igst == null || igst == 0.0)
                                {
                                    tcategory = dr["tcategory"].ToString();
                                }
                                else
                                {
                                    string category = dr["tcategory"].ToString();
                                    if (category == "Sale Of Milk" || category == "Sale Of Curd " || category == "Sale Of Buttermilk" || category == "Sale Of Lassi")
                                    {
                                        tcategory = dr["tcategory"].ToString();
                                    }
                                    else
                                    {
                                        tcategory = dr["tcategory"].ToString() + "-CGST/SGST";
                                    }
                                }
                            }
                            newrow["Ledger Type"] = tcategory.ToString();
                            Unitprice = UnitCost;
                            double percent = 0;
                            double.TryParse(branch["igst"].ToString(), out igst);
                            double invval = 0;
                            double qty = 0;
                            double.TryParse(branch["qty"].ToString(), out qty);
                            double taxval = 0;
                            double netvalue = 0;
                            netvalue = invval + taxval;
                            netvalue = Math.Round(netvalue, 2);
                            newrow["Taxable Value"] = invval;
                            double sgstamount = 0;
                            double cgstamount = 0;
                            double Igst = 0;
                            double Igstamount = 0;
                            double totRate = 0;
                            double.TryParse(branch["Igst"].ToString(), out Igst);
                            double Igstcon = 100 + Igst;
                            float rate = 0;
                            double tot_vatamount = 0;
                            double PAmount = 0;
                            float.TryParse(UnitCost.ToString(), out rate);
                            Igstamount = (rate / Igstcon) * Igst;
                            Igstamount = Math.Round(Igstamount, 2);
                            totRate = Igstamount;
                            newrow["Ledger Type"] = tcategory.ToString();
                            double Vatrate = rate - totRate;
                            Vatrate = Math.Round(Vatrate, 2);
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
                            invval = Math.Round(invval, 2);
                            netvalue = invval + taxval;
                            netvalue = Math.Round(netvalue, 2);
                            double tot_amount = PAmount + tot_vatamount;
                            tot_amount = Math.Round(tot_amount, 2);
                            newrow["Net Value"] = tot_amount;
                            if (ddlSalesOffice.SelectedItem.Text == "Cash Sale")
                            {
                                newrow["Narration"] = "Being the stock transfer to  " + ddlSalesOffice.SelectedItem.Text + " vide dc No " + branch["sno"].ToString() + ",DC Date " + fromdate.AddDays(-1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                            }
                            else
                            {
                                newrow["Narration"] = "Being the stock transfer to  " + branch["DispName"].ToString() + " from " + ddlSalesOffice.SelectedItem.Text + " vide dc No " + branch["sno"].ToString() + ",DC Date " + fromdate.AddDays(-1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();

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
                                if (catarr.Contains(branch["categoryid"].ToString()))
                                {
                                    DataRow newrow = Report.NewRow();
                                    if (ddlSalesOffice.SelectedItem.Text == "Staff Sale")
                                    {
                                        newrow["Customer Name"] = branch["DispName"].ToString();
                                    }
                                    else if (ddlSalesOffice.SelectedItem.Text == "Free Sale")
                                    {
                                        newrow["Customer Name"] = branch["DispName"].ToString();
                                    }
                                    else if (ddlSalesOffice.SelectedItem.Text == "Others")
                                    {
                                        newrow["Customer Name"] = branch["DispName"].ToString();
                                    }
                                    else
                                    {
                                        newrow["Customer Name"] = "Cash sales-" + branchcode;
                                    }
                                    newrow["HSN CODE"] = branch["hsncode"].ToString();
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


                                    if (fromdate.Month > 3)
                                    {
                                        newrow["Invoce No."] = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                    }
                                    else
                                    {
                                        if (fromdate.Month < 3)
                                        {
                                            newrow["Invoce No."] = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "N/" + DCNO;
                                        }
                                        else
                                        {
                                            newrow["Invoce No."] = branchcode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "N/" + DCNO;
                                        }
                                    }

                                    newrow["Invoice Date"] = fromdate.AddDays(-1).ToString("dd-MMM-yyyy");
                                    newrow["Item Name"] = branch["tproduct"].ToString();
                                    newrow["Qty"] = branch["qty"].ToString();
                                    double UnitCost = 0;
                                    double Unitprice = 0;
                                    string tcategory = "";
                                    double igst = 0;
                                    double.TryParse(branch["igst"].ToString(), out igst);
                                    double cgst = 0;
                                    double.TryParse(branch["cgst"].ToString(), out cgst);
                                    double sgst = 0;
                                    double.TryParse(branch["sgst"].ToString(), out sgst);
                                    foreach (DataRow dr in dtprodcts.Select("product_sno='" + branch["Prodsno"].ToString() + "'"))
                                    {
                                        double.TryParse(dr["unitprice"].ToString(), out UnitCost);
                                        if (igst == null || igst == 0.0)
                                        {
                                            tcategory = dr["tcategory"].ToString();
                                        }
                                        else
                                        {
                                            string category = dr["tcategory"].ToString();
                                            if (category == "Sale Of Milk" || category == "Sale Of Curd " || category == "Sale Of Buttermilk" || category == "Sale Of Lassi")
                                            {
                                                tcategory = dr["tcategory"].ToString();
                                            }
                                            else
                                            {
                                                tcategory = dr["tcategory"].ToString() + "-CGST/SGST";
                                            }
                                        }
                                    }
                                    newrow["Ledger Type"] = tcategory.ToString();
                                    Unitprice = UnitCost;
                                    double percent = 0;
                                    double.TryParse(branch["igst"].ToString(), out igst);
                                    double invval = 0;
                                    double qty = 0;
                                    double.TryParse(branch["qty"].ToString(), out qty);
                                    double taxval = 0;
                                    double netvalue = 0;
                                    netvalue = invval + taxval;
                                    netvalue = Math.Round(netvalue, 2);
                                    newrow["Taxable Value"] = invval;
                                    double sgstamount = 0;
                                    double cgstamount = 0;
                                    double Igst = 0;
                                    double Igstamount = 0;
                                    double totRate = 0;
                                    double.TryParse(branch["Igst"].ToString(), out Igst);
                                    double Igstcon = 100 + Igst;
                                    float rate = 0;
                                    double tot_vatamount = 0;
                                    double PAmount = 0;
                                    float.TryParse(UnitCost.ToString(), out rate);
                                    Igstamount = (rate / Igstcon) * Igst;
                                    Igstamount = Math.Round(Igstamount, 2);
                                    totRate = Igstamount;
                                    newrow["Ledger Type"] = tcategory.ToString();
                                    double Vatrate = rate - totRate;
                                    Vatrate = Math.Round(Vatrate, 2);
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
                                    invval = Math.Round(invval, 2);
                                    netvalue = invval + taxval;
                                    netvalue = Math.Round(netvalue, 2);
                                    double tot_amount = PAmount;
                                    tot_amount = Math.Round(tot_amount, 2);
                                    newrow["Net Value"] = tot_amount;
                                    if (ddlSalesOffice.SelectedItem.Text == "Cash Sale")
                                    {
                                        newrow["Narration"] = "Being the stock transfer to  " + ddlSalesOffice.SelectedItem.Text + " vide dc No " + branch["sno"].ToString() + ",DC Date " + fromdate.AddDays(-1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                                    }
                                    else
                                    {
                                        newrow["Narration"] = "Being the stock transfer to  " + branch["DispName"].ToString() + " from " + ddlSalesOffice.SelectedItem.Text + " vide dc No " + branch["sno"].ToString() + ",DC Date " + fromdate.AddDays(-1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();

                                    }
                                    Report.Rows.Add(newrow);
                                    i++;
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
                    cmd = new MySqlCommand("SELECT branchdata.sno,branchdata.companycode,branchdata.branchcode, branchdata.BranchName, statemastar.statename ,statemastar.statecode FROM branchdata INNER JOIN statemastar ON branchdata.stateid = statemastar.sno WHERE (branchdata.sno = @BranchID)");
                    if (Session["salestype"].ToString() == "Plant")
                    {
                        cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                    }
                    DataTable dtstatename = vdm.SelectQuery(cmd).Tables[0];
                    string statename = "";
                    string statecode = "";
                    string branchcode = "";
                    string companycode = "";
                    if (dtstatename.Rows.Count > 0)
                    {
                        statename = dtstatename.Rows[0]["statename"].ToString();
                        statecode = dtstatename.Rows[0]["statecode"].ToString();
                        branchcode = dtstatename.Rows[0]["branchcode"].ToString();
                        companycode = dtstatename.Rows[0]["companycode"].ToString();
                    }
                    if (ddlSalesOffice.SelectedItem.Text == "Cash Sale")
                    {
                        cmd = new MySqlCommand("SELECT TripInfo.Sno, TripInfo.DCNo,TripInfo.taxdcno, ProductInfo.tproduct,ProductInfo.hsncode,ProductInfo.igst,ProductInfo.cgst,ProductInfo.sgst, ProductInfo.Prodsno, ProductInfo.Categoryname, ProductInfo.categoryid,ProductInfo.Qty, TripInfo.I_Date, TripInfo.VehicleNo, TripInfo.Status, TripInfo.DispName, TripInfo.DispType, TripInfo.DispMode FROM (SELECT tripdata.Sno, tripdata.DCNo,tripdata.taxdcno, tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, dispatch.DispType, dispatch.DispMode FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.DispMode = 'LOCAL') AND (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2)) TripInfo INNER JOIN (SELECT Categoryname, categoryid,ProductName, Sno, Qty, tproduct,Prodsno,hsncode,igst,sgst,cgst FROM  (SELECT products_category.Categoryname, productsdata.ProductName,products_category.sno as categoryid, tripdata_1.Sno, tripsubdata.Qty,productsdata.hsncode,productsdata.igst,productsdata.cgst,productsdata.sgst, productsdata.tproduct,productsdata.sno as Prodsno FROM tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata_1.AssignDate BETWEEN @d1 AND @d2)) TripSubInfo) ProductInfo ON TripInfo.Sno = ProductInfo.Sno");
                    }
                    else if (ddlSalesOffice.SelectedItem.Text == "Staff Sale")
                    {
                        cmd = new MySqlCommand("SELECT TripInfo.Sno,TripInfo.DCNo,TripInfo.taxdcno, ProductInfo.tproduct,ProductInfo.hsncode,ProductInfo.igst,ProductInfo.cgst,ProductInfo.sgst, ProductInfo.Prodsno, ProductInfo.Categoryname, ProductInfo.categoryid,ProductInfo.Qty, TripInfo.I_Date, TripInfo.VehicleNo, TripInfo.Status, TripInfo.DispName, TripInfo.DispType, TripInfo.DispMode FROM (SELECT tripdata.Sno, tripdata.DCNo,tripdata.taxdcno, tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, dispatch.DispType, dispatch.DispMode FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.DispMode = 'Staff') AND (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2)) TripInfo INNER JOIN (SELECT Categoryname, categoryid,ProductName, Sno, Qty, tproduct,Prodsno,hsncode,igst,sgst,cgst FROM  (SELECT products_category.Categoryname, productsdata.ProductName,products_category.sno as categoryid, tripdata_1.Sno, tripsubdata.Qty,productsdata.hsncode,productsdata.igst,productsdata.cgst,productsdata.sgst, productsdata.tproduct,productsdata.sno as Prodsno FROM tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata_1.AssignDate BETWEEN @d1 AND @d2)) TripSubInfo) ProductInfo ON TripInfo.Sno = ProductInfo.Sno");
                    }
                    else if (ddlSalesOffice.SelectedItem.Text == "Free Sale")
                    {
                        cmd = new MySqlCommand("SELECT  TripInfo.Sno,TripInfo.DCNo,TripInfo.taxdcno, ProductInfo.tproduct, ProductInfo.hsncode, ProductInfo.igst, ProductInfo.cgst, ProductInfo.sgst, ProductInfo.Prodsno, ProductInfo.Categoryname,ProductInfo.categoryid, ProductInfo.Qty, TripInfo.I_Date, TripInfo.VehicleNo, TripInfo.Status, TripInfo.DispName, TripInfo.DispType, TripInfo.DispMode FROM (SELECT tripdata.Sno, tripdata.DCNo,tripdata.taxdcno, tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, dispatch.DispType, dispatch.DispMode FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.DispMode = 'Free') AND (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2)) TripInfo INNER JOIN (SELECT Categoryname,categoryid, ProductName, Sno, Qty, tproduct, Prodsno, hsncode, igst, sgst, cgst FROM (SELECT products_category.Categoryname,products_category.sno as categoryid, productsdata.ProductName, tripdata_1.Sno, tripsubdata.Qty, productsdata.hsncode, productsdata.igst,productsdata.cgst, productsdata.sgst, productsdata.tproduct, productsdata.sno AS Prodsno FROM  tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata_1.AssignDate BETWEEN @d1 AND @d2)) TripSubInfo) ProductInfo ON TripInfo.Sno = ProductInfo.Sno");
                    }
                    //else
                    //{
                    //    cmd = new MySqlCommand("SELECT  TripInfo.Sno, TripInfo.DCNo, ProductInfo.tproduct, ProductInfo.hsncode, ProductInfo.igst, ProductInfo.cgst, ProductInfo.sgst, ProductInfo.Prodsno, ProductInfo.Categoryname, ProductInfo.Qty, TripInfo.I_Date, TripInfo.VehicleNo, TripInfo.Status, TripInfo.DispName, TripInfo.DispType, TripInfo.DispMode FROM (SELECT tripdata.Sno, tripdata.DCNo, tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, dispatch.DispType, dispatch.DispMode FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.DispType = 'AGENT') AND(dispatch.DispMode = 'AGENT') AND (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2)) TripInfo INNER JOIN (SELECT Categoryname, ProductName, Sno, Qty, tproduct, Prodsno, hsncode, igst, sgst, cgst FROM (SELECT products_category.Categoryname, productsdata.ProductName, tripdata_1.Sno, tripsubdata.Qty, productsdata.hsncode, productsdata.igst,productsdata.cgst, productsdata.sgst, productsdata.tproduct, productsdata.sno AS Prodsno FROM  tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata_1.AssignDate BETWEEN @d1 AND @d2)) TripSubInfo) ProductInfo ON TripInfo.Sno = ProductInfo.Sno");
                    //}
                    cmd.Parameters.AddWithValue("@branch", Session["branch"]);
                    cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate).AddDays(-1));
                    DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
                    cmd = new MySqlCommand("SELECT branchproducts.branch_sno, branchproducts.product_sno, branchproducts.unitprice, branchproducts.VatPercent, products_category.tcategory FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchproducts.branch_sno = @BranchID)");
                    if (ddlSalesOffice.SelectedItem.Text == "Cash Sale" || ddlSalesOffice.SelectedItem.Text == "Free Sale" || ddlSalesOffice.SelectedItem.Text == "Others")
                    {
                        cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                    }
                    else
                    {
                        if (Session["branch"].ToString() == "7")
                        {
                            string barnch = "702";
                            cmd.Parameters.AddWithValue("@BranchID", barnch);
                        }
                        else
                        {
                            string barnch = "760";
                            cmd.Parameters.AddWithValue("@BranchID", barnch);
                        }
                    }
                    DataTable dtprodcts = vdm.SelectQuery(cmd).Tables[0];
                    int i = 1;
                    string DCNO = "0";
                    foreach (DataRow branch in dtble.Rows)
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
                                if (branch["igst"].ToString() != "0")
                                {
                                    DataRow newrow = Report.NewRow();
                                    if (ddlSalesOffice.SelectedItem.Text == "Staff Sale")
                                    {
                                        newrow["Customer Name"] = branch["DispName"].ToString();
                                    }
                                    else if (ddlSalesOffice.SelectedItem.Text == "Free Sale")
                                    {
                                        newrow["Customer Name"] = branch["DispName"].ToString();
                                    }
                                    else if (ddlSalesOffice.SelectedItem.Text == "Others")
                                    {
                                        newrow["Customer Name"] = branch["DispName"].ToString();
                                    }
                                    else
                                    {
                                        newrow["Customer Name"] = "Cash sales-" + branchcode;
                                    }
                                    newrow["HSN CODE"] = branch["hsncode"].ToString();
                                    //string DCNO = "0";
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
                                    if (countdc > 999 && countdc <= 9999)
                                    {
                                        DCNO = "0" + countdc;
                                    }
                                    if (countdc > 9999)
                                    {
                                        DCNO = "" + countdc;
                                    }
                                    if (fromdate.Month > 3)
                                    {
                                        newrow["Invoce No."] = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                    }
                                    else
                                    {
                                        if (fromdate.Month < 3)
                                        {
                                            newrow["Invoce No."] = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                        }
                                        else
                                        {
                                            newrow["Invoce No."] = branchcode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "T/" + DCNO;
                                        }
                                    }


                                    newrow["Invoice Date"] = fromdate.AddDays(-1).ToString("dd-MMM-yyyy");
                                    newrow["Item Name"] = branch["tproduct"].ToString();
                                    newrow["Qty"] = branch["qty"].ToString();
                                    double UnitCost = 0;
                                    double Unitprice = 0;
                                    string tcategory = "";
                                    double igst = 0;
                                    double.TryParse(branch["igst"].ToString(), out igst);
                                    double cgst = 0;
                                    double.TryParse(branch["cgst"].ToString(), out cgst);
                                    double sgst = 0;
                                    double.TryParse(branch["sgst"].ToString(), out sgst);
                                    foreach (DataRow dr in dtprodcts.Select("product_sno='" + branch["Prodsno"].ToString() + "'"))
                                    {
                                        double.TryParse(dr["unitprice"].ToString(), out UnitCost);
                                        if (igst == null || igst == 0.0)
                                        {
                                            tcategory = dr["tcategory"].ToString();
                                        }
                                        else
                                        {
                                            string category = dr["tcategory"].ToString();
                                            if (category == "Sale Of Milk" || category == "Sale Of Curd " || category == "Sale Of Buttermilk" || category == "Sale Of Lassi")
                                            {
                                                tcategory = dr["tcategory"].ToString();
                                            }
                                            else
                                            {
                                                tcategory = dr["tcategory"].ToString() + "-CGST/SGST";
                                            }
                                        }
                                        newrow["Ledger Type"] = tcategory.ToString();
                                        Unitprice = UnitCost;
                                        double percent = 0;
                                        double.TryParse(branch["igst"].ToString(), out igst);
                                        double invval = 0;
                                        double qty = 0;
                                        double.TryParse(branch["qty"].ToString(), out qty);
                                        double taxval = 0;
                                        double netvalue = 0;
                                        netvalue = invval + taxval;
                                        netvalue = Math.Round(netvalue, 2);
                                        newrow["Taxable Value"] = invval;
                                        double sgstamount = 0;
                                        double cgstamount = 0;
                                        double Igst = 0;
                                        double Igstamount = 0;
                                        double totRate = 0;
                                        double.TryParse(branch["Igst"].ToString(), out Igst);
                                        double Igstcon = 100 + Igst;
                                        float rate = 0;
                                        double tot_vatamount = 0;
                                        double PAmount = 0;
                                        float.TryParse(UnitCost.ToString(), out rate);
                                        Igstamount = (rate / Igstcon) * Igst;
                                        Igstamount = Math.Round(Igstamount, 2);
                                        totRate = Igstamount;
                                        newrow["Ledger Type"] = tcategory.ToString();
                                        double Vatrate = rate - totRate;
                                        Vatrate = Math.Round(Vatrate, 2);
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
                                        invval = Math.Round(invval, 2);
                                        netvalue = invval + taxval;
                                        netvalue = Math.Round(netvalue, 2);
                                        double tot_amount = PAmount + tot_vatamount;
                                        tot_amount = Math.Round(tot_amount, 2);
                                        newrow["Net Value"] = tot_amount;
                                        if (ddlSalesOffice.SelectedItem.Text == "Cash Sale")
                                        {
                                            newrow["Narration"] = "Being the stock transfer to  " + ddlSalesOffice.SelectedItem.Text + " vide dc No " + branch["sno"].ToString() + ",DC Date " + fromdate.AddDays(-1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                                        }
                                        else
                                        {
                                            newrow["Narration"] = "Being the stock transfer to  " + branch["DispName"].ToString() + " from " + ddlSalesOffice.SelectedItem.Text + " vide dc No " + branch["sno"].ToString() + ",DC Date " + fromdate.AddDays(-1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();

                                        }
                                        Report.Rows.Add(newrow);
                                        i++;
                                    }
                                    
                                }
                            }
                        }
                        else
                        {
                            if (branch["igst"].ToString() != "0")
                            {
                                DataRow newrow = Report.NewRow();
                                if (ddlSalesOffice.SelectedItem.Text == "Staff Sale")
                                {
                                    newrow["Customer Name"] = branch["DispName"].ToString();
                                }
                                else if (ddlSalesOffice.SelectedItem.Text == "Free Sale")
                                {
                                    newrow["Customer Name"] = branch["DispName"].ToString();
                                }
                                else if (ddlSalesOffice.SelectedItem.Text == "Others")
                                {
                                    newrow["Customer Name"] = branch["DispName"].ToString();
                                }
                                else
                                {
                                    newrow["Customer Name"] = "Cash sales-" + branchcode;
                                }
                                newrow["HSN CODE"] = branch["hsncode"].ToString();
                                //string DCNO = "0";
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
                                if (countdc > 999 && countdc <= 9999)
                                {
                                    DCNO = "0" + countdc;
                                }
                                if (countdc > 9999)
                                {
                                    DCNO = "" + countdc;
                                }
                                if (fromdate.Month > 3)
                                {
                                    newrow["Invoce No."] = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                }
                                else
                                {
                                    if (fromdate.Month < 3)
                                    {
                                        newrow["Invoce No."] = branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "T/" + DCNO;
                                    }
                                    else
                                    {
                                        newrow["Invoce No."] = branchcode + "/" + dtapril.AddYears(-1).ToString("yy") + "-" + dtmarch.AddYears(-1).ToString("yy") + "T/" + DCNO;
                                    }
                                }


                                newrow["Invoice Date"] = fromdate.AddDays(-1).ToString("dd-MMM-yyyy");
                                newrow["Item Name"] = branch["tproduct"].ToString();
                                newrow["Qty"] = branch["qty"].ToString();
                                double UnitCost = 0;
                                double Unitprice = 0;
                                string tcategory = "";
                                double igst = 0;
                                double.TryParse(branch["igst"].ToString(), out igst);
                                double cgst = 0;
                                double.TryParse(branch["cgst"].ToString(), out cgst);
                                double sgst = 0;
                                double.TryParse(branch["sgst"].ToString(), out sgst);
                                foreach (DataRow dr in dtprodcts.Select("product_sno='" + branch["Prodsno"].ToString() + "'"))
                                {
                                    double.TryParse(dr["unitprice"].ToString(), out UnitCost);
                                    if (igst == null || igst == 0.0)
                                    {
                                        tcategory = dr["tcategory"].ToString();
                                    }
                                    else
                                    {
                                        string category = dr["tcategory"].ToString();
                                        if (category == "Sale Of Milk" || category == "Sale Of Curd " || category == "Sale Of Buttermilk" || category == "Sale Of Lassi")
                                        {
                                            tcategory = dr["tcategory"].ToString();
                                        }
                                        else
                                        {
                                            tcategory = dr["tcategory"].ToString() + "-CGST/SGST";
                                        }
                                    }
                                    newrow["Ledger Type"] = tcategory.ToString();
                                    Unitprice = UnitCost;
                                    double percent = 0;
                                    double.TryParse(branch["igst"].ToString(), out igst);
                                    double invval = 0;
                                    double qty = 0;
                                    double.TryParse(branch["qty"].ToString(), out qty);
                                    double taxval = 0;
                                    double netvalue = 0;
                                    netvalue = invval + taxval;
                                    netvalue = Math.Round(netvalue, 2);
                                    newrow["Taxable Value"] = invval;
                                    double sgstamount = 0;
                                    double cgstamount = 0;
                                    double Igst = 0;
                                    double Igstamount = 0;
                                    double totRate = 0;
                                    double.TryParse(branch["Igst"].ToString(), out Igst);
                                    double Igstcon = 100 + Igst;
                                    float rate = 0;
                                    double tot_vatamount = 0;
                                    double PAmount = 0;
                                    float.TryParse(UnitCost.ToString(), out rate);
                                    Igstamount = (rate / Igstcon) * Igst;
                                    Igstamount = Math.Round(Igstamount, 2);
                                    totRate = Igstamount;
                                    newrow["Ledger Type"] = tcategory.ToString();
                                    double Vatrate = rate - totRate;
                                    Vatrate = Math.Round(Vatrate, 2);
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
                                    invval = Math.Round(invval, 2);
                                    netvalue = invval + taxval;
                                    netvalue = Math.Round(netvalue, 2);
                                    double tot_amount = PAmount + tot_vatamount;
                                    tot_amount = Math.Round(tot_amount, 2);
                                    newrow["Net Value"] = tot_amount;
                                    if (ddlSalesOffice.SelectedItem.Text == "Cash Sale")
                                    {
                                        newrow["Narration"] = "Being the stock transfer to  " + ddlSalesOffice.SelectedItem.Text + " vide dc No " + branch["sno"].ToString() + ",DC Date " + fromdate.AddDays(-1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                                    }
                                    else
                                    {
                                        newrow["Narration"] = "Being the stock transfer to  " + branch["DispName"].ToString() + " from " + ddlSalesOffice.SelectedItem.Text + " vide dc No " + branch["sno"].ToString() + ",DC Date " + fromdate.AddDays(-1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();

                                    }
                                    Report.Rows.Add(newrow);
                                    i++;
                                }
                            }
                        }
                    }
                    grdReports.DataSource = Report;
                    grdReports.DataBind();
                    Session["xportdata"] = Report;
                }
            }
            else
            {
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
                cmd = new MySqlCommand("SELECT    products_category.sno AS categoryid, branchdata.tbranchname, branchdata.BranchName, branchdata.stateid, branchdata.sno AS BSno, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, indents_subtable.UnitCost, productsdata.tproduct, productsdata.ProductName, productsdata.hsncode, productsdata.igst, productsdata.sgst, productsdata.cgst, productsdata.Units, productsdata.sno AS productsno, branchdata_1.SalesOfficeID, products_category.tcategory, branchproducts.VatPercent, branchdata.BranchCode FROM  (SELECT   IndentNo, Branch_id, I_date, Status, IndentType FROM  indents WHERE  (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent INNER JOIN branchdata ON indent.Branch_id = branchdata.sno INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchproducts ON branchmappingtable.SuperBranch = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno WHERE    (branchmappingtable.SuperBranch = @BranchID) AND (indents_subtable.DeliveryQty <> 0) GROUP BY productsdata.sno, BSno, branchmappingtable.SuperBranch, productsdata.igst, branchdata.BranchCode ORDER BY branchdata.BranchName");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate).AddDays(-1));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate).AddDays(-1));
                DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT branchdata.sno,branchdata.Branchcode,branchdata.companycode,  branchdata.BranchName,branchdata.stateid, statemastar.statename, statemastar.statecode , statemastar.gststatecode FROM branchdata INNER JOIN statemastar ON branchdata.stateid = statemastar.sno WHERE (branchdata.sno = @BranchID)");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
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
                int i = 1;
                foreach (DataRow branch in dtble.Rows)
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
                        //if (ReportDate.ToString("dd/MM/yyyy") == fromdate.ToString("dd/MM/yyyy"))
                        //{
                        cmd = new MySqlCommand("SELECT IFNULL(MAX(agentdcno), 0) + 1 AS Sno FROM agentdc WHERE (soid = @BranchId) AND (IndDate BETWEEN @d1 AND @d2)");
                        cmd.Parameters.AddWithValue("@BranchId", ddlSalesOffice.SelectedValue);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(dtapril).AddDays(-1));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(dtmarch).AddDays(-1));
                        DataTable dtadcno = vdm.SelectQuery(cmd).Tables[0];
                        string agentdcNo = dtadcno.Rows[0]["Sno"].ToString();
                        cmd = new MySqlCommand("Insert Into Agentdc (BranchId,IndDate,soid,agentdcno,stateid,companycode,moduleid,doe) Values(@BranchId,@IndDate,@soid,@agentdcno,@stateid,@companycode,@moduleid,@doe)");
                        cmd.Parameters.AddWithValue("@BranchId", branch["BSno"].ToString());
                        cmd.Parameters.AddWithValue("@IndDate", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@soid", ddlSalesOffice.SelectedValue);
                        cmd.Parameters.AddWithValue("@agentdcno", agentdcNo);
                        cmd.Parameters.AddWithValue("@stateid", gststatecode);
                        cmd.Parameters.AddWithValue("@companycode", companycode);
                        cmd.Parameters.AddWithValue("@doe", ReportDate);
                        cmd.Parameters.AddWithValue("@moduleid", Session["moduleid"].ToString());
                        DcNo = vdm.insertScalar(cmd);
                        cmd = new MySqlCommand("SELECT IndentNo FROM indents WHERE (Branch_id = @BranchId) AND (I_date BETWEEN @d1 AND @d2)");
                        cmd.Parameters.AddWithValue("@BranchId", branch["BSno"].ToString());
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                        DataTable dtindentno = vdm.SelectQuery(cmd).Tables[0];
                        if (dtindentno.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtindentno.Rows)
                            {
                                cmd = new MySqlCommand("Insert Into dcsubTable (DcNo,IndentNo) Values(@DcNo,@IndentNo)");
                                cmd.Parameters.AddWithValue("@DcNo", DcNo);
                                cmd.Parameters.AddWithValue("@IndentNo", dr["IndentNo"].ToString());
                                vdm.insert(cmd);
                            }
                        }
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
                    newrow["Customer Name"] = branch["tbranchname"].ToString();
                    newrow["HSN CODE"] = branch["hsncode"].ToString();
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
                    newrow["Invoce No."] = Branchcode + "/" + dtapril.ToString("yy") + "-" + dtmarch.ToString("yy") + "/" + DCNO;
                    newrow["Invoice Date"] = fromdate.AddDays(-1).ToString("dd-MMM-yyyy");
                    newrow["Item Name"] = branch["tproduct"].ToString();
                    newrow["Qty"] = branch["DeliveryQty"].ToString();
                    double UnitCost = 0;
                    double Unitprice = 0;
                    string tcategory = "";
                    double igst = 0;
                    double.TryParse(branch["UnitCost"].ToString(), out UnitCost);
                    if (igst == null || igst == 0.0)
                    {
                        tcategory = branch["tcategory"].ToString();
                    }
                    else
                    {
                        string category = branch["tcategory"].ToString();
                        if (category == "G.Sale Of Milk" || category == "G.Sale Of Curd " || category == "Sale Of Buttermilk" || category == "Sale Of Lassi")
                        {
                            tcategory = branch["tcategory"].ToString();
                        }
                        else
                        {
                            tcategory = branch["tcategory"].ToString() + "-CGST/SGST-" + statecode;
                        }
                    }
                    double percent = 0;
                    newrow["Qty"] = branch["DeliveryQty"].ToString();
                    Unitprice = UnitCost;
                    double.TryParse(branch["igst"].ToString(), out igst);
                    float rate = 0;
                    double invval = 0;
                    double qty = 0;
                    double.TryParse(branch["DeliveryQty"].ToString(), out qty);
                    double taxval = 0;
                    float.TryParse(UnitCost.ToString(), out rate);
                    double tot_vatamount = 0;
                    double PAmount = 0;
                    string tostateid = branch["stateid"].ToString();
                    if (fromstateid == tostateid)
                    {
                        double sgst = 0;
                        double sgstamount = 0;
                        double cgst = 0;
                        double cgstamount = 0;
                        double Igst = 0;
                        double Igstamount = 0;
                        double totRate = 0;
                        double.TryParse(branch["Igst"].ToString(), out Igst);
                        double Igstcon = 100 + Igst;
                        Igstamount = (rate / Igstcon) * Igst;
                        Igstamount = Math.Round(Igstamount, 2);
                        totRate = Igstamount;
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
                    newrow["Narration"] = "Being the stock transfer to  " + branch["tbranchname"].ToString() + " from " + ddlSalesOffice.SelectedItem.Text + " vide dc No " + branch["BSno"].ToString() + ",DC Date " + fromdate.AddDays(-1).ToString("dd/MM/yyyy") + ",Emp Name " + Session["EmpName"].ToString();
                    Report.Rows.Add(newrow);
                    i++;
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
            }
        }
        catch
        {
        }
    }
}
