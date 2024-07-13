using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Text;
using ClosedXML.Excel;
using System.Configuration;
public partial class IncentiveReport : System.Web.UI.Page
{
    MySqlCommand cmd;
    string BranchID = "";
    string Branchname = "";
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
                lblTitle.Text = Session["TitleName"].ToString();
                FillAgentName();
                FillStructures();

            }
        }

    }
    void FillAgentName()
    {
        try
        {
            vdm = new VehicleDBMgr();
            cmd = new MySqlCommand(" SELECT branchroutes.RouteName, branchroutes.Sno, branchroutes.BranchID FROM branchroutes INNER JOIN branchdata ON branchroutes.BranchID = branchdata.sno WHERE (branchroutes.BranchID = @brnchid) OR (branchdata.SalesOfficeID = @SOID)");
            //cmd = new MySqlCommand("SELECT RouteName, Sno, BranchID FROM branchroutes WHERE (BranchID = @brnchid)");
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            cmd.Parameters.AddWithValue("@brnchid", BranchID);
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            ddlRouteName.DataSource = dtRoutedata;
            ddlRouteName.DataTextField = "RouteName";
            ddlRouteName.DataValueField = "Sno";
            ddlRouteName.DataBind();
            ddlRouteName.Items.Insert(0, new ListItem("Select Route", "0"));

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    void FillStructures()
    {
        try
        {
            vdm = new VehicleDBMgr();
            // cmd = new MySqlCommand("SELECT StructureName, sno FROM incentive_structure WHERE (BranchID  = @brnchid)");
            cmd = new MySqlCommand("SELECT StructureName, sno FROM incentive_structure WHERE (BranchID = @brnchid) AND (Flag = @flag) AND (ApprovalStatus = @as)");
            cmd.Parameters.AddWithValue("@brnchid", BranchID);
            cmd.Parameters.AddWithValue("@flag", "1");
            cmd.Parameters.AddWithValue("@as", 'A');
            DataTable dtstructuredata = vdm.SelectQuery(cmd).Tables[0];
            ddlstructure.DataSource = dtstructuredata;
            ddlstructure.DataTextField = "StructureName";
            ddlstructure.DataValueField = "sno";
            ddlstructure.DataBind();
            ddlstructure.Items.Insert(0, new ListItem("Select Structure", "0"));

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
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
    float leakpercentage = 0.0f;
    double totmilkamt = 0;
    DataTable Report = new DataTable();

    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (ddlincentivetype.SelectedItem.Text == "Normal Incentive")
            {
                if (btnicentivesave.Text == "Save")
                {

                    double Totalmilksale = 0;
                    double Totalbulkmilksale = 0;
                    double Totalbulkmilksaleamount = 0;
                    DateTime fromdate = DateTime.Now;
                    Report = new DataTable();
                    DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
                    string[] fromdatestrig = txtFromdate.Text.Split(' ');
                    if (fromdatestrig.Length > 1)
                    {
                        if (fromdatestrig[0].Split('-').Length > 0)
                        {
                            string[] dates = fromdatestrig[0].Split('-');
                            string[] times = fromdatestrig[1].Split(':');
                            fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                        }
                    }
                    DateTime todate = DateTime.Now;

                    string[] todatestrig = txtTodate.Text.Split(' ');
                    if (todatestrig.Length > 1)
                    {
                        if (todatestrig[0].Split('-').Length > 0)
                        {
                            string[] dates = todatestrig[0].Split('-');
                            string[] times = todatestrig[1].Split(':');
                            todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                        }
                    }
                    Session["filename"] = "Incentive For -> " + ddlAgentName.SelectedItem.Text;
                    lblAgent.Text = ddlAgentName.SelectedItem.Text;
                    lblroute.Text = ddlRouteName.SelectedItem.Text;
                    lbldate.Text = ServerDateCurrentdate.ToString("dd-MM-yyyy hh:mm");

                    cmd = new MySqlCommand("SELECT MAX(FromDate) AS fromdate, MAX(sno) AS incentivetranssno, MAX(Todate) AS todate FROM incentivetransactions WHERE (BranchId = @brnchid)");
                    cmd.Parameters.AddWithValue("@brnchid", ddlAgentName.SelectedValue);

                    DataTable dtincentiveedit = vdm.SelectQuery(cmd).Tables[0];
                    string previousincentive = "";
                    string fromdt = "";
                    string todt = "";
                    if (dtincentiveedit.Rows.Count > 0)
                    {
                        fromdt = dtincentiveedit.Rows[0]["fromdate"].ToString();
                        todt = dtincentiveedit.Rows[0]["todate"].ToString();
                    }
                    if (todt == "")
                    {
                        todt = fromdate.AddDays(-1).ToString();
                    }
                    if (fromdt == "")
                    {
                        txtprevDate.Text = "Previous Incentive Not Found";
                    }
                    if (fromdt != "")
                    {
                        txtprevDate.Text = fromdt + " " + "TO" + " " + todt;
                    }
                    DateTime dtprevdate = new DateTime();
                    dtprevdate = DateTime.Parse(todt);
                    //if (fromdate > dtprevdate)
                    //{
                        //cmd = new MySqlCommand("SELECT productsdata.sno,productsdata.ProductName, product_clubbing.ClubName, incentive_structure.StructureName, product_clubbing.sno AS clubbingsno FROM incentive_structure INNER JOIN incentive_struct_sub ON incentive_structure.sno = incentive_struct_sub.is_sno INNER JOIN product_clubbing ON incentive_struct_sub.clubbingID = product_clubbing.sno INNER JOIN subproductsclubbing ON product_clubbing.sno = subproductsclubbing.Clubsno INNER JOIN productsdata ON subproductsclubbing.Productid = productsdata.sno WHERE (incentive_structure.sno = @StructureID)");
                        cmd = new MySqlCommand("SELECT productsdata.sno, productsdata.ProductName, product_clubbing.ClubName, incentive_structure.StructureName, product_clubbing.sno AS clubbingsno,products_category.Categoryname, products_subcategory.category_sno FROM incentive_structure INNER JOIN incentive_struct_sub ON incentive_structure.sno = incentive_struct_sub.is_sno INNER JOIN product_clubbing ON incentive_struct_sub.clubbingID = product_clubbing.sno INNER JOIN subproductsclubbing ON product_clubbing.sno = subproductsclubbing.Clubsno INNER JOIN productsdata ON subproductsclubbing.Productid = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (incentive_structure.sno = @StructureID) ");
                        cmd.Parameters.AddWithValue("@StructureID", ddlstructure.SelectedValue);
                        DataTable dtincentivestructure = vdm.SelectQuery(cmd).Tables[0];
                        // cmd = new MySqlCommand("SELECT DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, indents_subtable.DeliveryQty * indents_subtable.UnitCost AS Amount,productsdata.ProductName, productsdata.sno AS prdtsno, indents_subtable.DeliveryQty, indents_subtable.UnitCost FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (branchdata.sno = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY indents.I_date, branchdata.BranchName, productsdata.ProductName, indents_subtable.DeliveryQty");
                        cmd = new MySqlCommand("SELECT DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS Amount,productsdata.ProductName, productsdata.sno AS prdtsno,Inventorysno as invsno, sum(indents_subtable.DeliveryQty) AS DeliveryQty, indents_subtable.UnitCost, products_category.Categoryname,products_category.sno AS categorysno FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchdata.sno = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty <> ' ')  GROUP BY DATE(indents.I_date), branchdata.BranchName, productsdata.ProductName");
                        cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                        DataTable dtdelivered = vdm.SelectQuery(cmd).Tables[0];

                        cmd = new MySqlCommand("SELECT invtras.TransType, invtras.FromTran, invtras.ToTran, SUM(invtras.Qty) AS qty, invtras.DOE, invmaster.sno AS invsno, invmaster.InvName FROM (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty FROM invtransactions12 WHERE (ToTran = @branchid) AND (DOE BETWEEN @d1 AND @d2) OR (DOE BETWEEN @d1 AND @d2) AND (FromTran = @branchid)) invtras INNER JOIN invmaster ON invtras.B_inv_sno = invmaster.sno GROUP BY invtras.TransType, invmaster.sno");
                        cmd.Parameters.AddWithValue("@branchid", ddlAgentName.SelectedValue);
                        DateTime dt1 = GetLowDate(fromdate.AddDays(-1));
                        DateTime dt2 = GetLowDate(todate);
                        cmd.Parameters.AddWithValue("@d1", dt1.AddHours(15));
                        cmd.Parameters.AddWithValue("@d2", dt2.AddHours(15));
                        DataTable dtinventoryDC = vdm.SelectQuery(cmd).Tables[0];

                        cmd = new MySqlCommand("SELECT idIndent_Permissions, SalesOfficeId, AgentId, Permission_From, Permission_To, Permission_For, Entry_Date, Given_By, Remarks FROM indent_permissions WHERE (AgentId = @branchid) AND (Permission_For = @permissionfor)");
                        cmd.Parameters.AddWithValue("@branchid", ddlAgentName.SelectedValue);
                        cmd.Parameters.AddWithValue("@permissionfor", "Incentive");
                        DataTable dtpermission = vdm.SelectQuery(cmd).Tables[0];

                        int dtrowscount = dtdelivered.Rows.Count;
                        if (dtrowscount == 0)
                        {
                            if (txtTodate.Text == "")
                            {

                            }
                            else
                            {
                                lblmsg.Text = "No Indent Found Between These Days";
                            }
                            grdReports.DataSource = Report;
                            grdReports.DataBind();
                        }
                        else
                        {
                            float count = 0;
                            count = (float)(todate - fromdate.AddDays(-1)).TotalDays;
                            Report = new DataTable();
                            Report.Columns.Add("IndentDate");
                            //Report.Columns.Add("Branch Name");
                            DataView view = new DataView(dtdelivered);
                            DataTable distinctProduct = view.ToTable(true, "ProductName");
                            foreach (DataRow dr in distinctProduct.Rows)
                            {
                                Report.Columns.Add(dr["ProductName"].ToString());
                            }
                            Report.Columns.Add("Total  Ltrs");
                            DataTable distincttable = view.ToTable(true, "BranchName", "IndentDate");
                            DataTable distincttotal = view.ToTable(true, "ProductName", "DeliveryQty");
                            int i = 1;
                            double Total = 0;
                            double TotalQty = 0;
                            double prdtwisetotal = 0;
                            double prdtwiseamount = 0;
                            int categorysno = 0;
                            foreach (DataRow branch in distincttable.Rows)
                            {
                                DataRow newrow = Report.NewRow();
                                string IndentDate = branch["IndentDate"].ToString();
                                DateTime dtIndentDate = Convert.ToDateTime(IndentDate).AddDays(1);
                                string ChangedTime = dtIndentDate.ToString("dd/MMM/yyyy");
                                newrow["IndentDate"] = ChangedTime;
                                //newrow["Branch Name"] = branch["BranchName"].ToString();
                                Total = 0;
                                foreach (DataRow dr in dtdelivered.Rows)
                                {
                                    if (branch["IndentDate"].ToString() == dr["IndentDate"].ToString())
                                    {
                                        if (dr["DeliveryQty"].ToString() != "")
                                        {
                                            double DeliveryQty = 0;
                                            double.TryParse(dr["DeliveryQty"].ToString(), out DeliveryQty);
                                            DeliveryQty = Math.Round(DeliveryQty, 2);
                                            double UnitCost = 0;
                                            double.TryParse(dr["UnitCost"].ToString(), out UnitCost);
                                            newrow[dr["ProductName"].ToString()] = DeliveryQty;
                                            // Total += DeliveryQty * UnitCost;
                                            int.TryParse(dr["categorysno"].ToString(), out categorysno);
                                            if (categorysno == 1)
                                            {
                                                string invsno = dr["invsno"].ToString();
                                                if (invsno == "4")
                                                {
                                                    Totalbulkmilksale += DeliveryQty;
                                                }
                                                Totalmilksale += DeliveryQty;
                                            }
                                            Total += DeliveryQty;
                                        }
                                    }
                                }
                                newrow["Total  Ltrs"] = Total;
                                TotalQty += Total;
                                Report.Rows.Add(newrow);
                            }
                            DataRow newvartical = Report.NewRow();
                            newvartical["IndentDate"] = "Total";
                            foreach (DataRow dr in distinctProduct.Rows)
                            {
                                prdtwisetotal = 0;
                                foreach (DataRow drtotprdt in dtdelivered.Rows)
                                {
                                    if (dr["ProductName"].ToString() == drtotprdt["ProductName"].ToString())
                                    {
                                        double prdtQty = 0;
                                        double.TryParse(drtotprdt["DeliveryQty"].ToString(), out prdtQty);
                                        prdtQty = Math.Round(prdtQty, 2);
                                        if (drtotprdt["categorysno"].ToString() == "1")
                                        {
                                            double prdtamt = 0;
                                            double.TryParse(drtotprdt["Amount"].ToString(), out prdtamt);
                                            prdtwiseamount += prdtamt;

                                            string invsno = drtotprdt["invsno"].ToString();
                                            if (invsno == "4")
                                            {
                                                Totalbulkmilksaleamount += prdtamt;
                                            }
                                        }
                                        prdtwisetotal += prdtQty;

                                    }
                                }
                                newvartical[dr["ProductName"].ToString()] = prdtwisetotal;
                            }
                            totmilkamt = (float)prdtwiseamount;
                            //double val = 0;
                            //double.TryParse(Report.Compute("sum([Total  Amount])", "[Total  Amount]<>'0'").ToString(), out val);
                            newvartical["Total  Ltrs"] = TotalQty;

                            double val1 = 0;
                            foreach (DataColumn dc in Report.Columns)
                            {
                                if (dc.DataType == typeof(Double))
                                {
                                    val1 = 0.0;
                                    double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val1);
                                    newvartical[dc.ToString()] = val1;
                                }
                            }
                            Report.Rows.Add(newvartical);
                            string productnames = "";
                            if (dtincentivestructure.Rows.Count > 0)
                            {
                                foreach (DataColumn dc in Report.Columns)
                                {
                                    DataRow[] drstr = dtincentivestructure.Select("ProductName='" + dc.ToString() + "'");
                                    if (drstr.Length > 0)
                                    {
                                    }
                                    else
                                    {
                                        if (dc.ToString() == "IndentDate" || dc.ToString() == "Total  Ltrs")
                                        {
                                        }
                                        else
                                        {
                                            productnames += dc.ToString() + " ,";
                                        }
                                    }
                                }
                            }
                            lbl_warn.Text = productnames;

                            foreach (DataColumn col in Report.Columns)
                            {
                                string Pname = col.ToString();
                                string ProductName = col.ToString();
                                ProductName = GetSpace(ProductName);
                                Report.Columns[Pname].ColumnName = ProductName;
                            }
                            if (Report.Columns.Count == 3)
                            {
                                Report.Columns.Add("  ");
                                Report.Columns.Add("   ");
                                Report.Columns.Add("    ");
                            }
                            if (Report.Columns.Count == 4)
                            {
                                Report.Columns.Add("  ");
                            }

                            DataTable dtTotincentive = new DataTable();
                            dtTotincentive.Columns.Add("ClubbingName");
                            dtTotincentive.Columns.Add("TotalSale").DataType = typeof(Double);
                            dtTotincentive.Columns.Add("AverageSale").DataType = typeof(Double);
                            dtTotincentive.Columns.Add("DiscountSlot");
                            dtTotincentive.Columns.Add("TotalAmount").DataType = typeof(Double);

                            string clubbingname = "";
                            string categoryserial = "1";
                            float milkincentive = 0;

                            DataView incentiveview = new DataView(dtincentivestructure);
                            DataTable dticentive = incentiveview.ToTable(true, "ClubName", "clubbingsno");

                            //cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty), 2) AS deliveryqty,subproductsclubbing. FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN subproductsclubbing ON indents_subtable.Product_sno = subproductsclubbing.Productid WHERE (indents.Branch_id = @selectedbrnch) AND (indents_subtable.D_date BETWEEN @d1 AND @d2)");
                            cmd = new MySqlCommand("SELECT result.deliveryqty, result.ClubName, result.Clubsno, slabs.SlotQty, slabs.Amt FROM (SELECT ROUND(SUM(indents_subtable.DeliveryQty), 2) AS deliveryqty, subproductsclubbing.Clubsno, product_clubbing.ClubName FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN subproductsclubbing ON indents_subtable.Product_sno = subproductsclubbing.Productid INNER JOIN product_clubbing ON subproductsclubbing.Clubsno = product_clubbing.sno WHERE (indents.Branch_id = @selectedbrnch) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY subproductsclubbing.Clubsno) result INNER JOIN slabs ON result.Clubsno = slabs.club_sno");
                            cmd.Parameters.AddWithValue("@selectedbrnch", ddlAgentName.SelectedValue);
                            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                            DataTable dtclubtotal = vdm.SelectQuery(cmd).Tables[0];
                            foreach (DataRow drincetiveclub in dticentive.Rows)
                            {
                                float avgsale = 0;
                                float slotqty = 0;
                                float slotamt = 0;
                                float totalsale = 0;
                                string sltamt = "";
                                clubbingname = drincetiveclub["ClubName"].ToString();
                                //categoryserial = drincetiveclub["category_sno"].ToString();
                                foreach (DataRow drdtclubtotal in dtclubtotal.Select("Clubsno='" + drincetiveclub["clubbingsno"].ToString() + "'"))
                                {
                                    float.TryParse(drdtclubtotal["deliveryqty"].ToString(), out totalsale);

                                    avgsale = (totalsale / count);
                                    float.TryParse(drdtclubtotal["SlotQty"].ToString(), out slotqty);
                                    //if (avgsale > slotqty)
                                    //{
                                        float.TryParse(drdtclubtotal["Amt"].ToString(), out slotamt);
                                        sltamt = drdtclubtotal["Amt"].ToString();
                                    //}
                                }
                                DataRow newrow = dtTotincentive.NewRow();
                                newrow["ClubbingName"] = clubbingname;
                                newrow["TotalSale"] = Math.Round(totalsale, 2);
                                newrow["AverageSale"] = Math.Round(avgsale, 2);
                                newrow["DiscountSlot"] = sltamt;
                                newrow["TotalAmount"] = Math.Round(totalsale * slotamt, 2);

                                if (categoryserial == "1")
                                {
                                    milkincentive += (float)Math.Round(totalsale * slotamt, 2);
                                }
                                dtTotincentive.Rows.Add(newrow);
                            }
                            double totalmilksale = 0;
                            string leak = txtleakage.Text;
                            leakpercentage = (float)Convert.ToDouble(leak);
                            Session["leak"] = leakpercentage;
                            double TotMilkandMilkAmt = 0;
                            Totalmilksale = Totalmilksale - Totalbulkmilksale;
                            totmilkamt = totmilkamt - Totalbulkmilksaleamount;
                            TotMilkandMilkAmt = totmilkamt / Totalmilksale;
                            double totleakincentive = 0;
                            //int.TryParse(txtleakage.Text, out leakpercentage);
                            if (leakpercentage != 0)
                            {
                                totalmilksale = leakpercentage / 100 * Totalmilksale;
                                totleakincentive = totalmilksale * TotMilkandMilkAmt;
                                DataRow newrow = dtTotincentive.NewRow();
                                newrow["ClubbingName"] = "LEAKAGE";
                                newrow["TotalSale"] = Math.Round(Totalmilksale, 2);
                                newrow["AverageSale"] = Math.Round(totalmilksale, 2);
                                newrow["DiscountSlot"] = Math.Round(TotMilkandMilkAmt, 2);
                                newrow["TotalAmount"] = Math.Round(totleakincentive, 2);
                                dtTotincentive.Rows.Add(newrow);

                            }
                            DataRow newrowtotal = dtTotincentive.NewRow();
                            newrowtotal["DiscountSlot"] = "TotalDiscount";
                            float incentive = 0;
                            float.TryParse(dtTotincentive.Compute("sum([TotalAmount])", "[TotalAmount]<>'0'").ToString(), out incentive);
                            newrowtotal["TotalAmount"] = Math.Round(incentive, 2);
                            dtTotincentive.Rows.Add(newrowtotal);
                            lblactualdiscount1.Text = Math.Round(incentive, 2).ToString();
                            txtincentivegiven.Text = Math.Round(incentive).ToString();
                            DataRow headerrow = Report.NewRow();
                            headerrow[0] = "ClubbingName";
                            headerrow[1] = "TotalSale";
                            headerrow[2] = "AverageSale";
                            headerrow[3] = "DiscountSlot";
                            headerrow[4] = "TotalAmount";
                            Report.Rows.Add(headerrow);

                            //DataTable dtsum = new DataTable();
                            //dtsum = Report.Copy();
                            foreach (DataRow drr in dtTotincentive.Rows)
                            {
                                DataRow newrow = Report.NewRow();
                                newrow[0] = drr["ClubbingName"].ToString();
                                newrow[1] = drr["TotalSale"].ToString();
                                newrow[2] = drr["AverageSale"].ToString();
                                newrow[3] = drr["DiscountSlot"].ToString();
                                newrow[4] = drr["TotalAmount"].ToString();
                                Report.Rows.Add(newrow);
                            }

                            #region------------>Inventory Balance<-----------------
                            int totcrates_del = 0;
                            int totcrates_col = 0;
                            int totcans_del = 0;
                            int totcans_col = 0;
                            foreach (DataRow drinv in dtinventoryDC.Rows)
                            {
                                if (drinv["TransType"].ToString() == "2")
                                {
                                    if (drinv["invsno"].ToString() == "1")
                                    {
                                        int Dcrates = 0;
                                        int.TryParse(drinv["qty"].ToString(), out Dcrates);
                                        totcrates_del += Dcrates;
                                    }
                                    if (drinv["invsno"].ToString() == "2")
                                    {
                                        int Dcans = 0;
                                        int.TryParse(drinv["qty"].ToString(), out Dcans);
                                        totcans_del += Dcans;
                                    }
                                    if (drinv["invsno"].ToString() == "3")
                                    {
                                        int Dcans = 0;
                                        int.TryParse(drinv["qty"].ToString(), out Dcans);
                                        totcans_del += Dcans;
                                    }
                                    if (drinv["invsno"].ToString() == "4")
                                    {
                                        int Dcans = 0;
                                        int.TryParse(drinv["qty"].ToString(), out Dcans);
                                        totcans_del += Dcans;
                                    }
                                    if (drinv["invsno"].ToString() == "5")
                                    {
                                        int Dcans = 0;
                                        int.TryParse(drinv["qty"].ToString(), out Dcans);
                                        totcans_del += Dcans;
                                    }

                                }
                                if (drinv["TransType"].ToString() == "1" || drinv["TransType"].ToString() == "3")
                                {
                                    if (drinv["invsno"].ToString() == "1")
                                    {
                                        int Ccrates = 0;
                                        int.TryParse(drinv["qty"].ToString(), out Ccrates);
                                        totcrates_col += Ccrates;
                                    }
                                    if (drinv["invsno"].ToString() == "2")
                                    {
                                        int Ccans = 0;
                                        int.TryParse(drinv["qty"].ToString(), out Ccans);
                                        totcans_col += Ccans;
                                    }
                                    if (drinv["invsno"].ToString() == "3")
                                    {
                                        int Ccans = 0;
                                        int.TryParse(drinv["qty"].ToString(), out Ccans);
                                        totcans_col += Ccans;
                                    }
                                    if (drinv["invsno"].ToString() == "4")
                                    {
                                        int Ccans = 0;
                                        int.TryParse(drinv["qty"].ToString(), out Ccans);
                                        totcans_col += Ccans;
                                    }
                                    if (drinv["invsno"].ToString() == "5")
                                    {
                                        int Ccans = 0;
                                        int.TryParse(drinv["qty"].ToString(), out Ccans);
                                        totcans_col += Ccans;
                                    }

                                }
                            }
                            int totcratesbal = 0;
                            int totcansbal = 0;
                            totcratesbal = totcrates_del - totcrates_col;
                            totcansbal = totcans_del - totcans_col;
                            if (totcratesbal < 0)
                            {
                                totcratesbal = 0;
                            }
                            if (totcansbal < 0)
                            {
                                totcansbal = 0;
                            }
                            int totbalanceinventory = 0;
                            totbalanceinventory = totcratesbal + totcansbal;
                            if (totbalanceinventory <= 0)
                            {
                                lblmsg.Text = "";

                            }
                            if (totbalanceinventory > 0)
                            {
                                if (dtpermission.Rows.Count > 0)
                                {
                                    lblmsg.Text = "";

                                }
                                else
                                {
                                    lblmsg.Text = "Crates Balance   :" + totcratesbal + "  Cans Balance    :" + totcansbal;
                                }
                            }
                            #endregion

                            grdReports.DataSource = Report;
                            grdReports.DataBind();
                            Session["xportdata"] = Report;
                            Session["xporttype"] = "Incentive Report";
                            Session["agentname"] = ddlAgentName.SelectedItem.Text;
                            Session["routename"] = ddlRouteName.SelectedItem.Text;
                            Session["date"] = ServerDateCurrentdate.ToString("dd-MM-yyyy hh:mm");

                        }

                    //}
                    //else
                    //{
                    //    lblmsg.Text = "Please select Correct FromDate";
                    //}
                }
                else
                {
                    DataTable Report = new DataTable();
                    double Totalmilksale = 0;
                    double Totalbulkmilksale = 0;
                    double Totalbulkmilksaleamount = 0;
                    DateTime fromdate = DateTime.Now;
                    DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
                    string[] fromdatestrig = txtFromdate.Text.Split(' ');
                    if (fromdatestrig.Length > 1)
                    {
                        if (fromdatestrig[0].Split('-').Length > 0)
                        {
                            string[] dates = fromdatestrig[0].Split('-');
                            string[] times = fromdatestrig[1].Split(':');
                            fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                        }
                    }
                    DateTime todate = DateTime.Now;

                    string[] todatestrig = txtTodate.Text.Split(' ');
                    if (todatestrig.Length > 1)
                    {
                        if (todatestrig[0].Split('-').Length > 0)
                        {
                            string[] dates = todatestrig[0].Split('-');
                            string[] times = todatestrig[1].Split(':');
                            todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                        }
                    }
                    Session["filename"] = "Incentive For -> " + ddlAgentName.SelectedItem.Text;
                    // lblAgent.Text = ddlAgentName.SelectedItem.Text;
                    //lblroute.Text = ddlRouteName.SelectedItem.Text;
                    //lbldate.Text = ServerDateCurrentdate.ToString("dd-MM-yyyy hh:mm");
                    //cmd = new MySqlCommand("SELECT productsdata.sno,productsdata.ProductName, product_clubbing.ClubName, incentive_structure.StructureName, product_clubbing.sno AS clubbingsno FROM incentive_structure INNER JOIN incentive_struct_sub ON incentive_structure.sno = incentive_struct_sub.is_sno INNER JOIN product_clubbing ON incentive_struct_sub.clubbingID = product_clubbing.sno INNER JOIN subproductsclubbing ON product_clubbing.sno = subproductsclubbing.Clubsno INNER JOIN productsdata ON subproductsclubbing.Productid = productsdata.sno WHERE (incentive_structure.sno = @StructureID)");
                    cmd = new MySqlCommand("SELECT productsdata.sno, productsdata.ProductName, product_clubbing.ClubName, incentive_structure.StructureName, product_clubbing.sno AS clubbingsno,products_category.Categoryname, products_subcategory.category_sno FROM incentive_structure INNER JOIN incentive_struct_sub ON incentive_structure.sno = incentive_struct_sub.is_sno INNER JOIN product_clubbing ON incentive_struct_sub.clubbingID = product_clubbing.sno INNER JOIN subproductsclubbing ON product_clubbing.sno = subproductsclubbing.Clubsno INNER JOIN productsdata ON subproductsclubbing.Productid = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (incentive_structure.sno = @StructureID) ");
                    cmd.Parameters.AddWithValue("@StructureID", ddlstructure.SelectedValue);
                    DataTable dtincentivestructure = vdm.SelectQuery(cmd).Tables[0];
                    // cmd = new MySqlCommand("SELECT DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, indents_subtable.DeliveryQty * indents_subtable.UnitCost AS Amount,productsdata.ProductName, productsdata.sno AS prdtsno, indents_subtable.DeliveryQty, indents_subtable.UnitCost FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (branchdata.sno = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY indents.I_date, branchdata.BranchName, productsdata.ProductName, indents_subtable.DeliveryQty");
                    cmd = new MySqlCommand("SELECT DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS Amount,productsdata.ProductName, productsdata.sno AS prdtsno,Inventorysno as invsno, sum(indents_subtable.DeliveryQty) AS DeliveryQty, indents_subtable.UnitCost, products_category.Categoryname,products_category.sno AS categorysno FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchdata.sno = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty <> ' ')  GROUP BY DATE(indents.I_date), branchdata.BranchName, productsdata.ProductName");
                    //cmd = new MySqlCommand("SELECT DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, indents_subtable.DeliveryQty * indents_subtable.UnitCost AS Amount,productsdata.ProductName,productsdata.Inventorysno as invsno, productsdata.sno AS prdtsno, indents_subtable.DeliveryQty, indents_subtable.UnitCost, products_category.Categoryname,products_category.sno AS categorysno FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchdata.sno = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty <> ' ')  GROUP BY indents.I_date, branchdata.BranchName, productsdata.ProductName, indents_subtable.DeliveryQty");
                    cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                    DataTable dtdelivered = vdm.SelectQuery(cmd).Tables[0];
                    int dtrowscount = dtdelivered.Rows.Count;
                    if (dtrowscount == 0)
                    {
                        lblmsg.Text = "No Indent Found Between These Days";
                        grdReports.DataSource = Report;
                        grdReports.DataBind();
                    }
                    else
                    {
                        float count = 0;
                        count = (float)(todate - fromdate.AddDays(-1)).TotalDays;
                        Report = new DataTable();
                        Report.Columns.Add("IndentDate");
                        //Report.Columns.Add("Branch Name");
                        DataView view = new DataView(dtdelivered);
                        DataTable distinctProduct = view.ToTable(true, "ProductName");
                        foreach (DataRow dr in distinctProduct.Rows)
                        {
                            Report.Columns.Add(dr["ProductName"].ToString());
                        }
                        Report.Columns.Add("Total  Ltrs");
                        DataTable distincttable = view.ToTable(true, "BranchName", "IndentDate");
                        DataTable distincttotal = view.ToTable(true, "ProductName", "DeliveryQty");
                        int i = 1;
                        double Total = 0;
                        double TotalQty = 0;
                        double prdtwisetotal = 0;
                        double prdtwiseamount = 0;
                        int categorysno = 0;
                        foreach (DataRow branch in distincttable.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            string IndentDate = branch["IndentDate"].ToString();
                            DateTime dtIndentDate = Convert.ToDateTime(IndentDate).AddDays(1);
                            string ChangedTime = dtIndentDate.ToString("dd/MMM/yyyy");
                            newrow["IndentDate"] = ChangedTime;
                            //newrow["Branch Name"] = branch["BranchName"].ToString();
                            Total = 0;
                            foreach (DataRow dr in dtdelivered.Rows)
                            {
                                if (branch["IndentDate"].ToString() == dr["IndentDate"].ToString())
                                {
                                    if (dr["DeliveryQty"].ToString() != "")
                                    {
                                        double DeliveryQty = 0;
                                        double.TryParse(dr["DeliveryQty"].ToString(), out DeliveryQty);
                                        DeliveryQty = Math.Round(DeliveryQty, 2);
                                        double UnitCost = 0;
                                        double.TryParse(dr["UnitCost"].ToString(), out UnitCost);
                                        newrow[dr["ProductName"].ToString()] = DeliveryQty;
                                        // Total += DeliveryQty * UnitCost;
                                        int.TryParse(dr["categorysno"].ToString(), out categorysno);

                                        if (categorysno == 1)
                                        {
                                            string invsno = dr["invsno"].ToString();
                                            if (invsno == "4")
                                            {
                                                Totalbulkmilksale += DeliveryQty;
                                            }
                                            Totalmilksale += DeliveryQty;
                                        }
                                        Total += DeliveryQty;
                                    }
                                }
                            }

                            newrow["Total  Ltrs"] = Total;
                            TotalQty += Total;
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["IndentDate"] = "Total";
                        foreach (DataRow dr in distinctProduct.Rows)
                        {
                            prdtwisetotal = 0;
                            foreach (DataRow drtotprdt in dtdelivered.Rows)
                            {
                                if (dr["ProductName"].ToString() == drtotprdt["ProductName"].ToString())
                                {
                                    double prdtQty = 0;
                                    double.TryParse(drtotprdt["DeliveryQty"].ToString(), out prdtQty);
                                    prdtQty = Math.Round(prdtQty, 2);

                                    if (drtotprdt["categorysno"].ToString() == "1")
                                    {
                                        double prdtamt = 0;
                                        double.TryParse(drtotprdt["Amount"].ToString(), out prdtamt);
                                        prdtwiseamount += prdtamt;
                                        string invsno = drtotprdt["invsno"].ToString();
                                        if (invsno == "4")
                                        {
                                            Totalbulkmilksaleamount += prdtamt;
                                        }
                                    }

                                    prdtwisetotal += prdtQty;

                                }
                            }
                            newvartical[dr["ProductName"].ToString()] = prdtwisetotal;
                        }
                        totmilkamt = (float)prdtwiseamount;
                        //double val = 0;
                        //double.TryParse(Report.Compute("sum([Total  Amount])", "[Total  Amount]<>'0'").ToString(), out val);
                        newvartical["Total  Ltrs"] = TotalQty;

                        double val1 = 0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val1 = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val1);
                                newvartical[dc.ToString()] = val1;
                            }
                        }
                        Report.Rows.Add(newvartical);
                        string productnames = "";
                        if (dtincentivestructure.Rows.Count > 0)
                        {
                            foreach (DataColumn dc in Report.Columns)
                            {
                                DataRow[] drstr = dtincentivestructure.Select("ProductName='" + dc.ToString() + "'");
                                if (drstr.Length > 0)
                                {
                                }
                                else
                                {
                                    if (dc.ToString() == "IndentDate" || dc.ToString() == "Total  Ltrs")
                                    {
                                    }
                                    else
                                    {
                                        productnames += dc.ToString() + " ,";
                                    }
                                }
                            }
                        }
                        lbl_warn.Text = productnames;
                        foreach (DataColumn col in Report.Columns)
                        {
                            string Pname = col.ToString();
                            string ProductName = col.ToString();
                            ProductName = GetSpace(ProductName);
                            Report.Columns[Pname].ColumnName = ProductName;
                        }
                        if (Report.Columns.Count == 3)
                        {
                            Report.Columns.Add("  ");
                            Report.Columns.Add("   ");
                            Report.Columns.Add("    ");
                        }
                        if (Report.Columns.Count == 4)
                        {
                            Report.Columns.Add("  ");
                        }
                        DataTable dtTotincentive = new DataTable();
                        dtTotincentive.Columns.Add("ClubbingName");
                        dtTotincentive.Columns.Add("TotalSale").DataType = typeof(Double);
                        dtTotincentive.Columns.Add("AverageSale").DataType = typeof(Double);
                        dtTotincentive.Columns.Add("DiscountSlot");
                        dtTotincentive.Columns.Add("TotalAmount").DataType = typeof(Double);

                        string clubbingname = "";
                        string categoryserial = "1";
                        float milkincentive = 0;

                        DataView incentiveview = new DataView(dtincentivestructure);
                        DataTable dticentive = incentiveview.ToTable(true, "ClubName", "clubbingsno");

                        //cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty), 2) AS deliveryqty,subproductsclubbing. FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN subproductsclubbing ON indents_subtable.Product_sno = subproductsclubbing.Productid WHERE (indents.Branch_id = @selectedbrnch) AND (indents_subtable.D_date BETWEEN @d1 AND @d2)");
                        cmd = new MySqlCommand("SELECT result.deliveryqty, result.ClubName, result.Clubsno, slabs.SlotQty, slabs.Amt FROM (SELECT ROUND(SUM(indents_subtable.DeliveryQty), 2) AS deliveryqty, subproductsclubbing.Clubsno, product_clubbing.ClubName FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN subproductsclubbing ON indents_subtable.Product_sno = subproductsclubbing.Productid INNER JOIN product_clubbing ON subproductsclubbing.Clubsno = product_clubbing.sno WHERE (indents.Branch_id = @selectedbrnch) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY subproductsclubbing.Clubsno) result INNER JOIN slabs ON result.Clubsno = slabs.club_sno");
                        cmd.Parameters.AddWithValue("@selectedbrnch", ddlAgentName.SelectedValue);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                        DataTable dtclubtotal = vdm.SelectQuery(cmd).Tables[0];
                        foreach (DataRow drincetiveclub in dticentive.Rows)
                        {
                            float avgsale = 0;
                            float slotqty = 0;
                            float slotamt = 0;
                            float totalsale = 0;
                            string sltamt = "";
                            clubbingname = drincetiveclub["ClubName"].ToString();
                            //categoryserial = drincetiveclub["category_sno"].ToString();
                            foreach (DataRow drdtclubtotal in dtclubtotal.Select("Clubsno='" + drincetiveclub["clubbingsno"].ToString() + "'"))
                            {
                                float.TryParse(drdtclubtotal["deliveryqty"].ToString(), out totalsale);
                                avgsale = (totalsale / count);
                                float.TryParse(drdtclubtotal["SlotQty"].ToString(), out slotqty);
                                if (avgsale > slotqty)
                                {
                                    float.TryParse(drdtclubtotal["Amt"].ToString(), out slotamt);
                                    sltamt = drdtclubtotal["Amt"].ToString();
                                }
                            }
                            DataRow newrow = dtTotincentive.NewRow();
                            newrow["ClubbingName"] = clubbingname;
                            newrow["TotalSale"] = Math.Round(totalsale, 2);
                            newrow["AverageSale"] = Math.Round(avgsale, 2);
                            newrow["DiscountSlot"] = sltamt;
                            newrow["TotalAmount"] = Math.Round(totalsale * slotamt, 2);

                            if (categoryserial == "1")
                            {
                                milkincentive += (float)Math.Round(totalsale * slotamt, 2);
                            }
                            dtTotincentive.Rows.Add(newrow);
                        }
                        double totalmilksale = 0;
                        string leak = txtleakage.Text;
                        leakpercentage = (float)Convert.ToDouble(leak);
                        Session["leak"] = leakpercentage;
                        double TotMilkandMilkAmt = 0;
                        Totalmilksale = Totalmilksale - Totalbulkmilksale;
                        totmilkamt = totmilkamt - Totalbulkmilksaleamount;
                        TotMilkandMilkAmt = totmilkamt / Totalmilksale;
                        double totleakincentive = 0;
                        //int.TryParse(txtleakage.Text, out leakpercentage);
                        if (leakpercentage != 0)
                        {
                            totalmilksale = leakpercentage / 100 * Totalmilksale;
                            totleakincentive = totalmilksale * TotMilkandMilkAmt;
                            DataRow newrow = dtTotincentive.NewRow();
                            newrow["ClubbingName"] = "LEAKAGE";
                            newrow["TotalSale"] = Math.Round(Totalmilksale, 2);
                            newrow["AverageSale"] = Math.Round(totalmilksale, 2);
                            newrow["DiscountSlot"] = Math.Round(TotMilkandMilkAmt, 2);
                            newrow["TotalAmount"] = Math.Round(totleakincentive, 2);
                            dtTotincentive.Rows.Add(newrow);

                        }
                        DataRow newrowtotal = dtTotincentive.NewRow();
                        newrowtotal["DiscountSlot"] = "TotalDiscount";
                        float incentive = 0;
                        float.TryParse(dtTotincentive.Compute("sum([TotalAmount])", "[TotalAmount]<>'0'").ToString(), out incentive);
                        newrowtotal["TotalAmount"] = Math.Round(incentive, 2);
                        dtTotincentive.Rows.Add(newrowtotal);
                        //lblactualdiscount1.Text = Math.Round(incentive, 2).ToString();
                        //txtincentivegiven.Text = Math.Round(incentive).ToString();
                        DataRow headerrow = Report.NewRow();
                        headerrow[0] = "ClubbingName";
                        headerrow[1] = "TotalSale";
                        headerrow[2] = "AverageSale";
                        headerrow[3] = "DiscountSlot";
                        headerrow[4] = "TotalAmount";
                        Report.Rows.Add(headerrow);

                        //DataTable dtsum = new DataTable();
                        //dtsum = Report.Copy();
                        foreach (DataRow drr in dtTotincentive.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow[0] = drr["ClubbingName"].ToString();
                            newrow[1] = drr["TotalSale"].ToString();
                            newrow[2] = drr["AverageSale"].ToString();
                            newrow[3] = drr["DiscountSlot"].ToString();
                            newrow[4] = drr["TotalAmount"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        grdReports.DataSource = Report;
                        grdReports.DataBind();
                        lblmsg.Text = "";

                        Session["xportdata"] = Report;
                        Session["xporttype"] = "Incentive Report";
                        Session["agentname"] = ddlAgentName.SelectedItem.Text;
                        Session["routename"] = ddlRouteName.SelectedItem.Text;
                        Session["date"] = ServerDateCurrentdate.ToString("dd-MM-yyyy hh:mm");
                    }
                }
            }
            if (ddlincentivetype.SelectedItem.Text == "Leakage Incentive")
            {
                if (btnicentivesave.Text == "Save")
                {

                    double Totalmilksale = 0;
                    double Totalbulkmilksale = 0;
                    double Totalbulkmilksaleamount = 0;
                    DateTime fromdate = DateTime.Now;
                    Report = new DataTable();
                    DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
                    string[] fromdatestrig = txtFromdate.Text.Split(' ');
                    if (fromdatestrig.Length > 1)
                    {
                        if (fromdatestrig[0].Split('-').Length > 0)
                        {
                            string[] dates = fromdatestrig[0].Split('-');
                            string[] times = fromdatestrig[1].Split(':');
                            fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                        }
                    }
                    DateTime todate = DateTime.Now;

                    string[] todatestrig = txtTodate.Text.Split(' ');
                    if (todatestrig.Length > 1)
                    {
                        if (todatestrig[0].Split('-').Length > 0)
                        {
                            string[] dates = todatestrig[0].Split('-');
                            string[] times = todatestrig[1].Split(':');
                            todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                        }
                    }
                    Session["filename"] = "Incentive For -> " + ddlAgentName.SelectedItem.Text;
                    lblAgent.Text = ddlAgentName.SelectedItem.Text;
                    lblroute.Text = ddlRouteName.SelectedItem.Text;
                    lbldate.Text = ServerDateCurrentdate.ToString("dd-MM-yyyy hh:mm");

                    cmd = new MySqlCommand("SELECT MAX(FromDate) AS fromdate, MAX(sno) AS incentivetranssno, MAX(Todate) AS todate FROM incentivetransactions WHERE (BranchId = @brnchid)");
                    cmd.Parameters.AddWithValue("@brnchid", ddlAgentName.SelectedValue);

                    DataTable dtincentiveedit = vdm.SelectQuery(cmd).Tables[0];
                    string previousincentive = "";
                    string fromdt = "";
                    string todt = "";
                    if (dtincentiveedit.Rows.Count > 0)
                    {
                        fromdt = dtincentiveedit.Rows[0]["fromdate"].ToString();
                        todt = dtincentiveedit.Rows[0]["todate"].ToString();
                    }
                    if (todt == "")
                    {
                        todt = fromdate.AddDays(-1).ToString();
                    }
                    if (fromdt == "")
                    {
                        txtprevDate.Text = "Previous Incentive Not Found";
                    }
                    if (fromdt != "")
                    {
                        txtprevDate.Text = fromdt + " " + "TO" + " " + todt;
                    }
                    DateTime dtprevdate = new DateTime();
                    dtprevdate = DateTime.Parse(todt);
                    //if (fromdate > dtprevdate)
                    //{
                    //cmd = new MySqlCommand("SELECT productsdata.sno,productsdata.ProductName, product_clubbing.ClubName, incentive_structure.StructureName, product_clubbing.sno AS clubbingsno FROM incentive_structure INNER JOIN incentive_struct_sub ON incentive_structure.sno = incentive_struct_sub.is_sno INNER JOIN product_clubbing ON incentive_struct_sub.clubbingID = product_clubbing.sno INNER JOIN subproductsclubbing ON product_clubbing.sno = subproductsclubbing.Clubsno INNER JOIN productsdata ON subproductsclubbing.Productid = productsdata.sno WHERE (incentive_structure.sno = @StructureID)");
                    //cmd = new MySqlCommand("SELECT productsdata.sno, productsdata.ProductName, product_clubbing.ClubName, incentive_structure.StructureName, product_clubbing.sno AS clubbingsno,products_category.Categoryname, products_subcategory.category_sno FROM incentive_structure INNER JOIN incentive_struct_sub ON incentive_structure.sno = incentive_struct_sub.is_sno INNER JOIN product_clubbing ON incentive_struct_sub.clubbingID = product_clubbing.sno INNER JOIN subproductsclubbing ON product_clubbing.sno = subproductsclubbing.Clubsno INNER JOIN productsdata ON subproductsclubbing.Productid = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (incentive_structure.sno = @StructureID) ");
                    //cmd.Parameters.AddWithValue("@StructureID", ddlstructure.SelectedValue);
                    //DataTable dtincentivestructure = vdm.SelectQuery(cmd).Tables[0];
                    // cmd = new MySqlCommand("SELECT DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, indents_subtable.DeliveryQty * indents_subtable.UnitCost AS Amount,productsdata.ProductName, productsdata.sno AS prdtsno, indents_subtable.DeliveryQty, indents_subtable.UnitCost FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (branchdata.sno = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY indents.I_date, branchdata.BranchName, productsdata.ProductName, indents_subtable.DeliveryQty");
                    cmd = new MySqlCommand("SELECT DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS Amount,productsdata.ProductName, productsdata.sno AS prdtsno,Inventorysno as invsno, sum(indents_subtable.DeliveryQty) AS DeliveryQty, indents_subtable.UnitCost, products_category.Categoryname,products_category.sno AS categorysno FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchdata.sno = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty <> ' ')  GROUP BY DATE(indents.I_date), branchdata.BranchName, productsdata.ProductName");
                    //cmd = new MySqlCommand("SELECT DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, indents_subtable.DeliveryQty * indents_subtable.UnitCost AS Amount,productsdata.ProductName, productsdata.sno AS prdtsno, productsdata.sno AS prdtsno, indents_subtable.DeliveryQty, indents_subtable.UnitCost, products_category.Categoryname,products_category.sno AS categorysno FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchdata.sno = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty <> ' ')  GROUP BY indents.I_date, branchdata.BranchName, productsdata.ProductName, indents_subtable.DeliveryQty");
                    cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                    DataTable dtdelivered = vdm.SelectQuery(cmd).Tables[0];
                    int dtrowscount = dtdelivered.Rows.Count;
                    if (dtrowscount == 0)
                    {
                        if (txtTodate.Text == "")
                        {

                        }
                        else
                        {
                            lblmsg.Text = "No Indent Found Between These Days";
                        }
                        grdReports.DataSource = Report;
                        grdReports.DataBind();
                    }
                    else
                    {
                        float count = 0;
                        count = (float)(todate - fromdate.AddDays(-1)).TotalDays;
                        Report = new DataTable();
                        Report.Columns.Add("IndentDate");
                        //Report.Columns.Add("Branch Name");
                        DataView view = new DataView(dtdelivered);
                        DataTable distinctProduct = view.ToTable(true, "ProductName");
                        foreach (DataRow dr in distinctProduct.Rows)
                        {
                            Report.Columns.Add(dr["ProductName"].ToString());
                        }
                        Report.Columns.Add("Total  Ltrs");
                        DataTable distincttable = view.ToTable(true, "BranchName", "IndentDate");
                        DataTable distincttotal = view.ToTable(true, "ProductName", "DeliveryQty");
                        int i = 1;
                        double Total = 0;
                        double TotalQty = 0;
                        double prdtwisetotal = 0;
                        double prdtwiseamount = 0;
                        int categorysno = 0;
                        foreach (DataRow branch in distincttable.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            string IndentDate = branch["IndentDate"].ToString();
                            DateTime dtIndentDate = Convert.ToDateTime(IndentDate).AddDays(1);
                            string ChangedTime = dtIndentDate.ToString("dd/MMM/yyyy");
                            newrow["IndentDate"] = ChangedTime;
                            //newrow["Branch Name"] = branch["BranchName"].ToString();
                            Total = 0;
                            foreach (DataRow dr in dtdelivered.Rows)
                            {
                                if (branch["IndentDate"].ToString() == dr["IndentDate"].ToString())
                                {
                                    if (dr["DeliveryQty"].ToString() != "")
                                    {
                                        double DeliveryQty = 0;
                                        double.TryParse(dr["DeliveryQty"].ToString(), out DeliveryQty);
                                        DeliveryQty = Math.Round(DeliveryQty, 2);

                                        double UnitCost = 0;
                                        double.TryParse(dr["UnitCost"].ToString(), out UnitCost);
                                        newrow[dr["ProductName"].ToString()] = DeliveryQty;
                                        // Total += DeliveryQty * UnitCost;
                                        int.TryParse(dr["categorysno"].ToString(), out categorysno);

                                        if (categorysno == 1)
                                        {
                                            string invsno = dr["invsno"].ToString();
                                            if (invsno == "4")
                                            {
                                                Totalbulkmilksale += DeliveryQty;
                                            }
                                            Totalmilksale += DeliveryQty;
                                        }
                                        Total += DeliveryQty;
                                    }
                                }
                            }

                            newrow["Total  Ltrs"] = Total;
                            TotalQty += Total;
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["IndentDate"] = "Total";
                        foreach (DataRow dr in distinctProduct.Rows)
                        {
                            prdtwisetotal = 0;
                            foreach (DataRow drtotprdt in dtdelivered.Rows)
                            {
                                if (dr["ProductName"].ToString() == drtotprdt["ProductName"].ToString())
                                {
                                    double prdtQty = 0;
                                    double.TryParse(drtotprdt["DeliveryQty"].ToString(), out prdtQty);
                                    prdtQty = Math.Round(prdtQty, 2);

                                    if (drtotprdt["categorysno"].ToString() == "1")
                                    {
                                        double prdtamt = 0;
                                        double.TryParse(drtotprdt["Amount"].ToString(), out prdtamt);
                                        prdtwiseamount += prdtamt;
                                        string invsno = drtotprdt["invsno"].ToString();
                                        if (invsno == "4")
                                        {
                                            Totalbulkmilksaleamount += prdtamt;
                                        }
                                    }

                                    prdtwisetotal += prdtQty;

                                }
                            }
                            newvartical[dr["ProductName"].ToString()] = prdtwisetotal;
                        }
                        totmilkamt = (float)prdtwiseamount;
                        //double val = 0;
                        //double.TryParse(Report.Compute("sum([Total  Amount])", "[Total  Amount]<>'0'").ToString(), out val);
                        newvartical["Total  Ltrs"] = TotalQty;

                        double val1 = 0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val1 = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val1);
                                newvartical[dc.ToString()] = val1;
                            }
                        }
                        Report.Rows.Add(newvartical);
                        foreach (DataColumn col in Report.Columns)
                        {
                            string Pname = col.ToString();
                            string ProductName = col.ToString();
                            ProductName = GetSpace(ProductName);
                            Report.Columns[Pname].ColumnName = ProductName;
                        }
                        if (Report.Columns.Count == 3)
                        {
                            Report.Columns.Add("  ");
                            Report.Columns.Add("   ");
                            Report.Columns.Add("    ");
                        }
                        if (Report.Columns.Count == 4)
                        {
                            Report.Columns.Add("  ");
                        }

                        DataTable dtTotincentive = new DataTable();
                        dtTotincentive.Columns.Add("ClubbingName");
                        dtTotincentive.Columns.Add("TotalSale").DataType = typeof(Double);
                        dtTotincentive.Columns.Add("AverageSale").DataType = typeof(Double);
                        dtTotincentive.Columns.Add("DiscountSlot");
                        dtTotincentive.Columns.Add("TotalAmount").DataType = typeof(Double);

                        string clubbingname = "";
                        string categoryserial = "1";
                        float milkincentive = 0;

                        double totalmilksale = 0;
                        string leak = txtleakage.Text;
                        leakpercentage = (float)Convert.ToDouble(leak);
                        Session["leak"] = leakpercentage;
                        double TotMilkandMilkAmt = 0;
                        Totalmilksale = Totalmilksale - Totalbulkmilksale;
                        totmilkamt = totmilkamt - Totalbulkmilksaleamount;
                        TotMilkandMilkAmt = totmilkamt / Totalmilksale;
                        double totleakincentive = 0;
                        //int.TryParse(txtleakage.Text, out leakpercentage);
                        if (leakpercentage != 0)
                        {
                            totalmilksale = leakpercentage / 100 * Totalmilksale;
                            totleakincentive = totalmilksale * TotMilkandMilkAmt;
                            DataRow newrow = dtTotincentive.NewRow();
                            newrow["ClubbingName"] = "LEAKAGE";
                            newrow["TotalSale"] = Math.Round(Totalmilksale, 2);
                            newrow["AverageSale"] = Math.Round(totalmilksale, 2);
                            newrow["DiscountSlot"] = Math.Round(TotMilkandMilkAmt, 2);
                            newrow["TotalAmount"] = Math.Round(totleakincentive, 2);
                            dtTotincentive.Rows.Add(newrow);

                        }
                        DataRow newrowtotal = dtTotincentive.NewRow();
                        newrowtotal["DiscountSlot"] = "TotalDiscount";
                        float incentive = 0;
                        float.TryParse(dtTotincentive.Compute("sum([TotalAmount])", "[TotalAmount]<>'0'").ToString(), out incentive);
                        newrowtotal["TotalAmount"] = Math.Round(incentive, 2);
                        dtTotincentive.Rows.Add(newrowtotal);
                        lblactualdiscount1.Text = Math.Round(incentive, 2).ToString();
                        txtincentivegiven.Text = Math.Round(incentive).ToString();
                        DataRow headerrow = Report.NewRow();
                        headerrow[0] = "ClubbingName";
                        headerrow[1] = "TotalSale";
                        headerrow[2] = "AverageSale";
                        headerrow[3] = "DiscountSlot";
                        headerrow[4] = "TotalAmount";
                        Report.Rows.Add(headerrow);

                        //DataTable dtsum = new DataTable();
                        //dtsum = Report.Copy();
                        foreach (DataRow drr in dtTotincentive.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow[0] = drr["ClubbingName"].ToString();
                            newrow[1] = drr["TotalSale"].ToString();
                            newrow[2] = drr["AverageSale"].ToString();
                            newrow[3] = drr["DiscountSlot"].ToString();
                            newrow[4] = drr["TotalAmount"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        grdReports.DataSource = Report;
                        grdReports.DataBind();
                        lblmsg.Text = "";

                        Session["xportdata"] = Report;
                        Session["xporttype"] = "Incentive Report";
                        Session["agentname"] = ddlAgentName.SelectedItem.Text;
                        Session["routename"] = ddlRouteName.SelectedItem.Text;
                        Session["date"] = ServerDateCurrentdate.ToString("dd-MM-yyyy hh:mm");
                    }

                    //}
                    //else
                    //{
                    //    lblmsg.Text = "Please select Correct FromDate";
                    //}
                }
                else
                {
                    DataTable Report = new DataTable();
                    double Totalmilksale = 0;
                    double Totalbulkmilksale = 0;
                    double Totalbulkmilksaleamount = 0;
                    DateTime fromdate = DateTime.Now;
                    DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
                    string[] fromdatestrig = txtFromdate.Text.Split(' ');
                    if (fromdatestrig.Length > 1)
                    {
                        if (fromdatestrig[0].Split('-').Length > 0)
                        {
                            string[] dates = fromdatestrig[0].Split('-');
                            string[] times = fromdatestrig[1].Split(':');
                            fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                        }
                    }
                    DateTime todate = DateTime.Now;

                    string[] todatestrig = txtTodate.Text.Split(' ');
                    if (todatestrig.Length > 1)
                    {
                        if (todatestrig[0].Split('-').Length > 0)
                        {
                            string[] dates = todatestrig[0].Split('-');
                            string[] times = todatestrig[1].Split(':');
                            todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                        }
                    }
                    Session["filename"] = "Incentive For -> " + ddlAgentName.SelectedItem.Text;
                    // lblAgent.Text = ddlAgentName.SelectedItem.Text;
                    //lblroute.Text = ddlRouteName.SelectedItem.Text;
                    //lbldate.Text = ServerDateCurrentdate.ToString("dd-MM-yyyy hh:mm");
                    //cmd = new MySqlCommand("SELECT productsdata.sno,productsdata.ProductName, product_clubbing.ClubName, incentive_structure.StructureName, product_clubbing.sno AS clubbingsno FROM incentive_structure INNER JOIN incentive_struct_sub ON incentive_structure.sno = incentive_struct_sub.is_sno INNER JOIN product_clubbing ON incentive_struct_sub.clubbingID = product_clubbing.sno INNER JOIN subproductsclubbing ON product_clubbing.sno = subproductsclubbing.Clubsno INNER JOIN productsdata ON subproductsclubbing.Productid = productsdata.sno WHERE (incentive_structure.sno = @StructureID)");
                    cmd = new MySqlCommand("SELECT productsdata.sno, productsdata.ProductName, product_clubbing.ClubName, incentive_structure.StructureName, product_clubbing.sno AS clubbingsno,products_category.Categoryname, products_subcategory.category_sno FROM incentive_structure INNER JOIN incentive_struct_sub ON incentive_structure.sno = incentive_struct_sub.is_sno INNER JOIN product_clubbing ON incentive_struct_sub.clubbingID = product_clubbing.sno INNER JOIN subproductsclubbing ON product_clubbing.sno = subproductsclubbing.Clubsno INNER JOIN productsdata ON subproductsclubbing.Productid = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (incentive_structure.sno = @StructureID) ");
                    cmd.Parameters.AddWithValue("@StructureID", ddlstructure.SelectedValue);
                    DataTable dtincentivestructure = vdm.SelectQuery(cmd).Tables[0];
                    // cmd = new MySqlCommand("SELECT DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, indents_subtable.DeliveryQty * indents_subtable.UnitCost AS Amount,productsdata.ProductName, productsdata.sno AS prdtsno, indents_subtable.DeliveryQty, indents_subtable.UnitCost FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (branchdata.sno = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY indents.I_date, branchdata.BranchName, productsdata.ProductName, indents_subtable.DeliveryQty");
                    //cmd = new MySqlCommand("SELECT DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName,productsdata.Inventorysno as invsno, indents_subtable.DeliveryQty * indents_subtable.UnitCost AS Amount,productsdata.ProductName, productsdata.sno AS prdtsno, indents_subtable.DeliveryQty, indents_subtable.UnitCost, products_category.Categoryname,products_category.sno AS categorysno FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchdata.sno = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty <> ' ')  GROUP BY indents.I_date, branchdata.BranchName, productsdata.ProductName, indents_subtable.DeliveryQty");
                    cmd = new MySqlCommand("SELECT DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS Amount,productsdata.ProductName, productsdata.sno AS prdtsno,Inventorysno as invsno, sum(indents_subtable.DeliveryQty) AS DeliveryQty, indents_subtable.UnitCost, products_category.Categoryname,products_category.sno AS categorysno FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchdata.sno = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty <> ' ')  GROUP BY DATE(indents.I_date), branchdata.BranchName, productsdata.ProductName");
                    cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                    DataTable dtdelivered = vdm.SelectQuery(cmd).Tables[0];
                    int dtrowscount = dtdelivered.Rows.Count;
                    if (dtrowscount == 0)
                    {
                        lblmsg.Text = "No Indent Found Between These Days";
                        grdReports.DataSource = Report;
                        grdReports.DataBind();
                    }
                    else
                    {
                        float count = 0;
                        count = (float)(todate - fromdate.AddDays(-1)).TotalDays;
                        Report = new DataTable();
                        Report.Columns.Add("IndentDate");
                        //Report.Columns.Add("Branch Name");
                        DataView view = new DataView(dtdelivered);
                        DataTable distinctProduct = view.ToTable(true, "ProductName");
                        foreach (DataRow dr in distinctProduct.Rows)
                        {
                            Report.Columns.Add(dr["ProductName"].ToString());
                        }
                        Report.Columns.Add("Total  Ltrs");
                        DataTable distincttable = view.ToTable(true, "BranchName", "IndentDate");
                        DataTable distincttotal = view.ToTable(true, "ProductName", "DeliveryQty");
                        int i = 1;
                        double Total = 0;
                        double TotalQty = 0;
                        double prdtwisetotal = 0;
                        double prdtwiseamount = 0;
                        int categorysno = 0;
                        foreach (DataRow branch in distincttable.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            string IndentDate = branch["IndentDate"].ToString();
                            DateTime dtIndentDate = Convert.ToDateTime(IndentDate).AddDays(1);
                            string ChangedTime = dtIndentDate.ToString("dd/MMM/yyyy");
                            newrow["IndentDate"] = ChangedTime;
                            //newrow["Branch Name"] = branch["BranchName"].ToString();
                            Total = 0;
                            foreach (DataRow dr in dtdelivered.Rows)
                            {
                                if (branch["IndentDate"].ToString() == dr["IndentDate"].ToString())
                                {
                                    if (dr["DeliveryQty"].ToString() != "")
                                    {
                                        double DeliveryQty = 0;
                                        double.TryParse(dr["DeliveryQty"].ToString(), out DeliveryQty);
                                        DeliveryQty = Math.Round(DeliveryQty, 2);

                                        double UnitCost = 0;
                                        double.TryParse(dr["UnitCost"].ToString(), out UnitCost);
                                        newrow[dr["ProductName"].ToString()] = DeliveryQty;
                                        // Total += DeliveryQty * UnitCost;
                                        int.TryParse(dr["categorysno"].ToString(), out categorysno);

                                        if (categorysno == 1)
                                        {
                                            string invsno = dr["invsno"].ToString();
                                            if (invsno == "4")
                                            {
                                                Totalbulkmilksale += DeliveryQty;
                                            }
                                            Totalmilksale += DeliveryQty;
                                        }
                                        Total += DeliveryQty;
                                    }
                                }
                            }

                            newrow["Total  Ltrs"] = Total;
                            TotalQty += Total;
                            Report.Rows.Add(newrow);
                        }
                        DataRow newvartical = Report.NewRow();
                        newvartical["IndentDate"] = "Total";
                        foreach (DataRow dr in distinctProduct.Rows)
                        {
                            prdtwisetotal = 0;
                            foreach (DataRow drtotprdt in dtdelivered.Rows)
                            {
                                if (dr["ProductName"].ToString() == drtotprdt["ProductName"].ToString())
                                {
                                    double prdtQty = 0;
                                    double.TryParse(drtotprdt["DeliveryQty"].ToString(), out prdtQty);
                                    prdtQty = Math.Round(prdtQty, 2);
                                    if (drtotprdt["categorysno"].ToString() == "1")
                                    {
                                        double prdtamt = 0;
                                        double.TryParse(drtotprdt["Amount"].ToString(), out prdtamt);
                                        prdtwiseamount += prdtamt;
                                        string invsno = drtotprdt["invsno"].ToString();
                                        if (invsno == "4")
                                        {
                                            Totalbulkmilksaleamount += prdtamt;
                                        }
                                    }

                                    prdtwisetotal += prdtQty;

                                }
                            }
                            newvartical[dr["ProductName"].ToString()] = prdtwisetotal;
                        }
                        totmilkamt = (float)prdtwiseamount;
                        //double val = 0;
                        //double.TryParse(Report.Compute("sum([Total  Amount])", "[Total  Amount]<>'0'").ToString(), out val);
                        newvartical["Total  Ltrs"] = TotalQty;

                        double val1 = 0;
                        foreach (DataColumn dc in Report.Columns)
                        {
                            if (dc.DataType == typeof(Double))
                            {
                                val1 = 0.0;
                                double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val1);
                                newvartical[dc.ToString()] = val1;
                            }
                        }
                        Report.Rows.Add(newvartical);
                        string productnames = "";
                        if (dtincentivestructure.Rows.Count > 0)
                        {
                            foreach (DataColumn dc in Report.Columns)
                            {
                                DataRow[] drstr = dtincentivestructure.Select("ProductName='" + dc.ToString() + "'");
                                if (drstr.Length > 0)
                                {
                                }
                                else
                                {
                                    if (dc.ToString() == "IndentDate" || dc.ToString() == "Total  Ltrs")
                                    {
                                    }
                                    else
                                    {
                                        productnames += dc.ToString() + " ,";
                                    }
                                }
                            }
                        }
                        lbl_warn.Text = productnames;
                        foreach (DataColumn col in Report.Columns)
                        {
                            string Pname = col.ToString();
                            string ProductName = col.ToString();
                            ProductName = GetSpace(ProductName);
                            Report.Columns[Pname].ColumnName = ProductName;
                        }
                        if (Report.Columns.Count == 3)
                        {
                            Report.Columns.Add("  ");
                            Report.Columns.Add("   ");
                            Report.Columns.Add("    ");
                        }
                        if (Report.Columns.Count == 4)
                        {
                            Report.Columns.Add("  ");
                        }

                        DataTable dtTotincentive = new DataTable();
                        dtTotincentive.Columns.Add("ClubbingName");
                        dtTotincentive.Columns.Add("TotalSale").DataType = typeof(Double);
                        dtTotincentive.Columns.Add("AverageSale").DataType = typeof(Double);
                        dtTotincentive.Columns.Add("DiscountSlot");
                        dtTotincentive.Columns.Add("TotalAmount").DataType = typeof(Double);

                        string clubbingname = "";
                        string categoryserial = "1";
                        float milkincentive = 0;

                        DataView incentiveview = new DataView(dtincentivestructure);
                        DataTable dticentive = incentiveview.ToTable(true, "ClubName", "clubbingsno");

                        //cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty), 2) AS deliveryqty,subproductsclubbing. FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN subproductsclubbing ON indents_subtable.Product_sno = subproductsclubbing.Productid WHERE (indents.Branch_id = @selectedbrnch) AND (indents_subtable.D_date BETWEEN @d1 AND @d2)");
                        cmd = new MySqlCommand("SELECT result.deliveryqty, result.ClubName, result.Clubsno, slabs.SlotQty, slabs.Amt FROM (SELECT ROUND(SUM(indents_subtable.DeliveryQty), 2) AS deliveryqty, subproductsclubbing.Clubsno, product_clubbing.ClubName FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN subproductsclubbing ON indents_subtable.Product_sno = subproductsclubbing.Productid INNER JOIN product_clubbing ON subproductsclubbing.Clubsno = product_clubbing.sno WHERE (indents.Branch_id = @selectedbrnch) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY subproductsclubbing.Clubsno) result INNER JOIN slabs ON result.Clubsno = slabs.club_sno");
                        cmd.Parameters.AddWithValue("@selectedbrnch", ddlAgentName.SelectedValue);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                        DataTable dtclubtotal = vdm.SelectQuery(cmd).Tables[0];
                        foreach (DataRow drincetiveclub in dticentive.Rows)
                        {
                            float avgsale = 0;
                            float slotqty = 0;
                            float slotamt = 0;
                            float totalsale = 0;
                            string sltamt = "";
                            clubbingname = drincetiveclub["ClubName"].ToString();
                            //categoryserial = drincetiveclub["category_sno"].ToString();
                            foreach (DataRow drdtclubtotal in dtclubtotal.Select("Clubsno='" + drincetiveclub["clubbingsno"].ToString() + "'"))
                            {
                                float.TryParse(drdtclubtotal["deliveryqty"].ToString(), out totalsale);
                                avgsale = (totalsale / count);
                                float.TryParse(drdtclubtotal["SlotQty"].ToString(), out slotqty);
                                if (avgsale > slotqty)
                                {
                                    float.TryParse(drdtclubtotal["Amt"].ToString(), out slotamt);
                                    sltamt = drdtclubtotal["Amt"].ToString();
                                }
                            }
                            DataRow newrow = dtTotincentive.NewRow();
                            newrow["ClubbingName"] = clubbingname;
                            newrow["TotalSale"] = Math.Round(totalsale, 2);
                            newrow["AverageSale"] = Math.Round(avgsale, 2);
                            newrow["DiscountSlot"] = sltamt;
                            newrow["TotalAmount"] = Math.Round(totalsale * slotamt, 2);

                            if (categoryserial == "1")
                            {
                                milkincentive += (float)Math.Round(totalsale * slotamt, 2);
                            }
                            dtTotincentive.Rows.Add(newrow);
                        }
                        double totalmilksale = 0;
                        string leak = txtleakage.Text;
                        leakpercentage = (float)Convert.ToDouble(leak);
                        Session["leak"] = leakpercentage;
                        double TotMilkandMilkAmt = 0;
                        Totalmilksale = Totalmilksale - Totalbulkmilksale;
                        totmilkamt = totmilkamt - Totalbulkmilksaleamount;
                        TotMilkandMilkAmt = totmilkamt / Totalmilksale;
                        double totleakincentive = 0;
                        //int.TryParse(txtleakage.Text, out leakpercentage);
                        if (leakpercentage != 0)
                        {
                            totalmilksale = leakpercentage / 100 * Totalmilksale;
                            totleakincentive = totalmilksale * TotMilkandMilkAmt;
                            DataRow newrow = dtTotincentive.NewRow();
                            newrow["ClubbingName"] = "LEAKAGE";
                            newrow["TotalSale"] = Math.Round(Totalmilksale, 2);
                            newrow["AverageSale"] = Math.Round(totalmilksale, 2);
                            newrow["DiscountSlot"] = Math.Round(TotMilkandMilkAmt, 2);
                            newrow["TotalAmount"] = Math.Round(totleakincentive, 2);
                            dtTotincentive.Rows.Add(newrow);

                        }
                        DataRow newrowtotal = dtTotincentive.NewRow();
                        newrowtotal["DiscountSlot"] = "TotalDiscount";
                        float incentive = 0;
                        float.TryParse(dtTotincentive.Compute("sum([TotalAmount])", "[TotalAmount]<>'0'").ToString(), out incentive);
                        newrowtotal["TotalAmount"] = Math.Round(incentive, 2);
                        dtTotincentive.Rows.Add(newrowtotal);
                        //lblactualdiscount1.Text = Math.Round(incentive, 2).ToString();
                        //txtincentivegiven.Text = Math.Round(incentive).ToString();
                        DataRow headerrow = Report.NewRow();
                        headerrow[0] = "ClubbingName";
                        headerrow[1] = "TotalSale";
                        headerrow[2] = "AverageSale";
                        headerrow[3] = "DiscountSlot";
                        headerrow[4] = "TotalAmount";
                        Report.Rows.Add(headerrow);

                        //DataTable dtsum = new DataTable();
                        //dtsum = Report.Copy();
                        foreach (DataRow drr in dtTotincentive.Rows)
                        {
                            DataRow newrow = Report.NewRow();
                            newrow[0] = drr["ClubbingName"].ToString();
                            newrow[1] = drr["TotalSale"].ToString();
                            newrow[2] = drr["AverageSale"].ToString();
                            newrow[3] = drr["DiscountSlot"].ToString();
                            newrow[4] = drr["TotalAmount"].ToString();
                            Report.Rows.Add(newrow);
                        }
                        grdReports.DataSource = Report;
                        grdReports.DataBind();
                        lblmsg.Text = "";

                        Session["xportdata"] = Report;
                        Session["xporttype"] = "Incentive Report";
                        Session["agentname"] = ddlAgentName.SelectedItem.Text;
                        Session["routename"] = ddlRouteName.SelectedItem.Text;
                        Session["date"] = ServerDateCurrentdate.ToString("dd-MM-yyyy hh:mm");
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
    protected void ddlRouteName_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand(" SELECT branchdata.sno, branchdata.BranchName, branchroutes.RouteName FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (branchroutes.Sno = @routesno) AND (branchdata.flag=1) ORDER BY branchdata.BranchName");
        //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName, branchroutes.RouteName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchroutesubtable ON branchmappingtable.SubBranch = branchroutesubtable.BranchID INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno WHERE (branchmappingtable.SuperBranch = @SuperBranch) AND (branchroutes.Sno = @routesno) ORDER BY branchdata.BranchName");
        cmd.Parameters.AddWithValue("@SuperBranch", BranchID);
        cmd.Parameters.AddWithValue("@routesno", ddlRouteName.SelectedValue);
        DataTable dtbranchdata = vdm.SelectQuery(cmd).Tables[0];
        ddlAgentName.DataSource = dtbranchdata;
        ddlAgentName.DataTextField = "BranchName";
        ddlAgentName.DataValueField = "sno";
        ddlAgentName.DataBind();
        ddlAgentName.Items.Insert(0, new ListItem("Select Agent", "0"));
        btnicentivesave.Text = "Save";
        txtincentivegiven.ReadOnly = false;
        txtremarks.ReadOnly = false;
        txtleakage.ReadOnly = false;
        ddlstructure.Enabled = true;

    }
    protected void btnicentivesave_Click(object sender, EventArgs e)
    {
        try
        {
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            string[] fromdatestrig = txtFromdate.Text.Split(' ');
            if (fromdatestrig.Length > 1)
            {
                if (fromdatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = fromdatestrig[0].Split('-');
                    string[] times = fromdatestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            DateTime todate = DateTime.Now;

            string[] todatestrig = txtTodate.Text.Split(' ');
            if (todatestrig.Length > 1)
            {
                if (todatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = todatestrig[0].Split('-');
                    string[] times = todatestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            if (btnicentivesave.Text == "Save")
            {
                if (ddlincentivetype.SelectedItem.Text == "Normal Incentive")
                {
                    cmd = new MySqlCommand("SELECT MAX(FromDate) AS fromdate, MAX(sno) AS incentivetranssno, MAX(Todate) AS todate FROM incentivetransactions WHERE (BranchId = @brnchid)");
                    cmd.Parameters.AddWithValue("@brnchid", ddlAgentName.SelectedValue);
                    DataTable dtincentiveedit = vdm.SelectQuery(cmd).Tables[0];
                    string fromdt = "";
                    string todt = "";
                    if (dtincentiveedit.Rows.Count > 0)
                    {
                        fromdt = dtincentiveedit.Rows[0]["fromdate"].ToString();
                        todt = dtincentiveedit.Rows[0]["todate"].ToString();
                    }
                    if (todt == "")
                    {
                        todt = fromdate.AddDays(-1).ToString();
                    }
                    DateTime dtprevdate = new DateTime();
                    dtprevdate = DateTime.Parse(todt);
                    if (fromdate > dtprevdate)
                    {
                        double BranchAmount = 0;
                        double.TryParse(txtincentivegiven.Text, out BranchAmount);
                        BranchAmount = Math.Round(BranchAmount, 2);
                        double actualamt = 0;
                        //DateTime fromdate = Convert.ToDateTime(cleardate);
                        double opdifference = 0;
                        //cmd = new MySqlCommand("SELECT   sno, OppBalance, SaleValue, paidamount, ClosingBalance, IndentDate, EntryDate, agentid, salesofficeid, SaleQty, ReceivedAmount, DiffAmount, RouteId, status FROM tempduetrasactions WHERE (agentid = @Agentid) AND (IndentDate BETWEEN @d1 AND @d2)");
                        //cmd.Parameters.AddWithValue("@Agentid", ddlAgentName.SelectedValue);
                        //cmd.Parameters.AddWithValue("@d1", GetLowDate(ServerDateCurrentdate).AddDays(-1));
                        //cmd.Parameters.AddWithValue("@d2", GetLowDate(ServerDateCurrentdate).AddDays(-1));
                        //DataTable dtpresentverifieddue = vdm.SelectQuery(cmd).Tables[0];
                        //if (dtpresentverifieddue.Rows.Count > 0)
                        //{
                        //    double salevalue = 0;
                        //    double.TryParse(dtpresentverifieddue.Rows[0]["salevalue"].ToString(), out salevalue);
                        //    double prevreceived = 0;
                        //    double.TryParse(dtpresentverifieddue.Rows[0]["ReceivedAmount"].ToString(), out prevreceived);
                        //    double opp = 0;
                        //    double.TryParse(dtpresentverifieddue.Rows[0]["OppBalance"].ToString(), out opp);
                        //    double clo = 0;
                        //    double.TryParse(dtpresentverifieddue.Rows[0]["ClosingBalance"].ToString(), out clo);
                        //    double prsentamount = 0;
                        //    double.TryParse(txtincentivegiven.Text, out prsentamount);
                        //    double opamt = opp + salevalue;/// - BranchAmount;
                        //    double Closing = opamt + prevreceived - BranchAmount;
                        //    if (BranchAmount >= prevreceived)
                        //    {
                        //        actualamt = BranchAmount - prevreceived;
                        //    }
                        //    else
                        //    {
                        //        actualamt = prevreceived - BranchAmount;
                        //    }
                        //    //cmd = new MySqlCommand("Update  tempduetrasactions set  ClosingBalance=ClosingBalance-@ClosingBalance,ReceivedAmount=ReceivedAmount+@ReceivedAmount where  agentid=@agentid AND indentdate=@indentdate");
                        //    //cmd.Parameters.AddWithValue("@ClosingBalance", BranchAmount);
                        //    //cmd.Parameters.AddWithValue("@ReceivedAmount", BranchAmount);
                        //    //cmd.Parameters.AddWithValue("@agentid", ddlAgentName.SelectedValue);
                        //    //cmd.Parameters.AddWithValue("@indentdate", GetLowDate(fromdate).AddDays(-1));
                        //    //vdm.Update(cmd);

                        //    cmd = new MySqlCommand("Update  tempduetrasactions set  ClosingBalance=ClosingBalance-@ClosingBalance,ReceivedAmount=ReceivedAmount+@ReceivedAmount,incentiveamount=incentiveamount+@incentiveamount where  (agentid=@agentid) AND (IndentDate=@indentdate)");
                        //    cmd.Parameters.AddWithValue("@ClosingBalance", BranchAmount);
                        //    cmd.Parameters.AddWithValue("@ReceivedAmount", BranchAmount);
                        //    cmd.Parameters.AddWithValue("@incentiveamount", BranchAmount);
                        //    cmd.Parameters.AddWithValue("@agentid", ddlAgentName.SelectedValue);
                        //    cmd.Parameters.AddWithValue("@indentdate", GetLowDate(fromdate).AddDays(-1));
                        //    vdm.Update(cmd);
                        //}
                        //cmd = new MySqlCommand("SELECT   sno, OppBalance, SaleValue, paidamount, ClosingBalance, IndentDate, EntryDate, agentid, salesofficeid, SaleQty, ReceivedAmount, DiffAmount, RouteId, status FROM tempduetrasactions WHERE (agentid = @Agentid) AND (IndentDate BETWEEN @d1 AND @d2)");
                        //cmd.Parameters.AddWithValue("@Agentid", ddlAgentName.SelectedValue);
                        //cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                        //cmd.Parameters.AddWithValue("@d2", GetHighDate(ServerDateCurrentdate));
                        //DataTable dtDueTransactions = vdm.SelectQuery(cmd).Tables[0];
                        //foreach (DataRow drr in dtDueTransactions.Rows)
                        //{
                        //    string indentdate = drr["indentdate"].ToString();
                        //    DateTime indent_date = Convert.ToDateTime(indentdate);
                        //    //cmd = new MySqlCommand("Update  tempduetrasactions set  OppBalance=OppBalance-@opningamount,ClosingBalance=ClosingBalance-@ClosingBalance where agentid=@Branchid AND indentdate=@indentdate");
                        //    //cmd.Parameters.AddWithValue("@opningamount", BranchAmount);
                        //    //cmd.Parameters.AddWithValue("@ClosingBalance", BranchAmount);
                        //    //cmd.Parameters.AddWithValue("@indentdate", GetLowDate(indent_date));
                        //    //cmd.Parameters.AddWithValue("@Branchid", ddlAgentName.SelectedValue);
                        //    //vdm.Update(cmd);

                        //    cmd = new MySqlCommand("Update  tempduetrasactions set  OppBalance=OppBalance-@opningamount,ClosingBalance=ClosingBalance-@ClosingBalance where (agentid=@Branchid) AND (IndentDate=@indentdate)");
                        //    cmd.Parameters.AddWithValue("@opningamount", BranchAmount);
                        //    cmd.Parameters.AddWithValue("@ClosingBalance", BranchAmount);
                        //    cmd.Parameters.AddWithValue("@indentdate", GetLowDate(indent_date));
                        //    cmd.Parameters.AddWithValue("@Branchid", ddlAgentName.SelectedValue);
                        //    vdm.Update(cmd);
                        //}
                        cmd = new MySqlCommand("insert into incentivetransactions (FromDate,Todate,StructureName,BranchId,EntryDate,ActualDiscount,TotalDiscount,Remarks,structure_sno,leakagepercent,rent,transport) values (@FromDate,@Todate,@StructureName,@BranchId,@EntryDate,@ActualDiscount,@TotalDiscount,@Remarks,@structuresno,@leakagepercent,@rent,@transport)");
                        cmd.Parameters.AddWithValue("@FromDate", fromdate);
                        cmd.Parameters.AddWithValue("@Todate", todate);
                        cmd.Parameters.AddWithValue("@StructureName", ddlstructure.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@structuresno", ddlstructure.SelectedValue);
                        cmd.Parameters.AddWithValue("@BranchId", ddlAgentName.SelectedValue);
                        cmd.Parameters.AddWithValue("@EntryDate", ServerDateCurrentdate);
                        cmd.Parameters.AddWithValue("@ActualDiscount", lblactualdiscount1.Text);
                        cmd.Parameters.AddWithValue("@TotalDiscount", txtincentivegiven.Text);
                        cmd.Parameters.AddWithValue("@Remarks", txtremarks.Text);
                        cmd.Parameters.AddWithValue("@leakagepercent", Session["leak"]);
                        cmd.Parameters.AddWithValue("@rent", txtRent.Text);
                        cmd.Parameters.AddWithValue("@transport", txtTransport.Text);
                        vdm.insert(cmd);
                    }
                    else
                    {
                        lblmsg.Text = "Please select Correct FromDate";
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select Correct FromDate')", true);
                    }
                }
                if (ddlincentivetype.SelectedItem.Text == "Leakage Incentive")
                {
                    cmd = new MySqlCommand("insert into incentivetransactions (FromDate,Todate,StructureName,BranchId,EntryDate,ActualDiscount,TotalDiscount,Remarks,structure_sno,leakagepercent) values (@FromDate,@Todate,@StructureName,@BranchId,@EntryDate,@ActualDiscount,@TotalDiscount,@Remarks,@structuresno,@leakagepercent)");
                    cmd.Parameters.AddWithValue("@FromDate", fromdate);
                    cmd.Parameters.AddWithValue("@Todate", todate);
                    cmd.Parameters.AddWithValue("@StructureName", ddlstructure.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@structuresno", ddlstructure.SelectedValue);
                    cmd.Parameters.AddWithValue("@BranchId", ddlAgentName.SelectedValue);
                    cmd.Parameters.AddWithValue("@EntryDate", ServerDateCurrentdate);
                    cmd.Parameters.AddWithValue("@ActualDiscount", lblactualdiscount1.Text);
                    cmd.Parameters.AddWithValue("@TotalDiscount", txtincentivegiven.Text);
                    cmd.Parameters.AddWithValue("@Remarks", txtremarks.Text);
                    cmd.Parameters.AddWithValue("@leakagepercent", Session["leak"]);
                    vdm.insert(cmd);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Inserted Successfully')", true);
                }
                string DenominationString = "";
                string ReturnDenominationString = "";
                string CashReceiptNo = "0";
                string ChequeNo = "0";
                string PaymentType = "Incentive";
                string Username = "1";
                string HeadSno = "";
                string Remarks = txtremarks.Text;
                string UserSno = Session["UserSno"].ToString();
                string BranchID = Session["branch"].ToString();
                    cmd = new MySqlCommand("SELECT Sno, BranchId, HeadName, LimitAmount, AccountType, AgentID, EmpID, accountcode, flag FROM accountheads WHERE (BranchId = @BranchID) AND (HeadName LIKE '%Sales Discount%')");
                    cmd.Parameters.AddWithValue("@BranchID", BranchID);
                    DataTable dtincentive = vdm.SelectQuery(cmd).Tables[0];
                    if (dtincentive.Rows.Count > 0)
                    {
                        HeadSno = dtincentive.Rows[0]["Sno"].ToString();
                    }

                    //as for instrectins of apurva sir and chairman sir

                    //cmd = new MySqlCommand("INSERT INTO collections (Branchid, AmountPaid, Denominations, Remarks, PaidDate, UserData_sno, PaymentType, ReturnDenomin, PayTime, EmpID, ChequeNo, ReceiptNo,headsno) VALUES (@Branchid, @AmountPaid, @Denominations, @Remarks, @PaidDate, @UserData_sno, @PaymentType, @ReturnDenomin, @PayTime, @EmpID, @ChequeNo,@ReceiptNo,@headsno)");
                    //cmd.Parameters.AddWithValue("@Branchid", ddlAgentName.SelectedValue);
                    //cmd.Parameters.AddWithValue("@AmountPaid", txtincentivegiven.Text);
                    //cmd.Parameters.AddWithValue("@Remarks", Remarks);
                    //cmd.Parameters.AddWithValue("@headsno", HeadSno);
                    //cmd.Parameters.AddWithValue("@PaidDate", ServerDateCurrentdate);
                    //cmd.Parameters.AddWithValue("@PayTime", ServerDateCurrentdate);
                    //cmd.Parameters.AddWithValue("@UserData_sno", Username);
                    //cmd.Parameters.AddWithValue("@PaymentType", PaymentType);
                    //cmd.Parameters.AddWithValue("@EmpID", UserSno);
                    //cmd.Parameters.AddWithValue("@ChequeNo", ChequeNo);
                    //cmd.Parameters.AddWithValue("@ReceiptNo", CashReceiptNo);
                    //cmd.Parameters.AddWithValue("@Denominations", DenominationString);
                    //cmd.Parameters.AddWithValue("@ReturnDenomin", ReturnDenominationString);
                    //vdm.insert(cmd);
                lblmsg.Text = "Record Inserted Successfully";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Inserted Successfully')", true);
                // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select Correct FromDate')", true);
            }
            else
            {
                btnicentivesave.Text = "Save";
                txtincentivegiven.ReadOnly = false;
                txtremarks.ReadOnly = false;
                txtleakage.ReadOnly = false;
                ddlstructure.Enabled = true;

            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable("GridView_Data");
            foreach (TableCell cell in grdReports.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in grdReports.Rows)
            {
                dt.Rows.Add();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (row.Cells[i].Text == "&nbsp;")
                    {
                        row.Cells[i].Text = "0";
                    }
                    dt.Rows[dt.Rows.Count - 1][i] = row.Cells[i].Text;
                }
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string FileName = Session["filename"].ToString();
                Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void ddlAgentName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtleakage.Text = "0";
        //txtremarks.Text = " ";
        //btnicentivesave.Text = "Save";
        //txtincentivegiven.ReadOnly = false;
        //txtremarks.ReadOnly = false;
        //txtleakage.ReadOnly = false;
        //ddlstructure.Enabled = true;
        //GetReport();
        try
        {
            vdm = new VehicleDBMgr();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);

            cmd = new MySqlCommand("SELECT MAX(FromDate) AS fromdate, MAX(sno) AS incentivetranssno, MAX(Todate) AS todate FROM incentivetransactions WHERE (BranchId = @brnchid)");
            cmd.Parameters.AddWithValue("@brnchid", ddlAgentName.SelectedValue);

            DataTable dtincentiveedit = vdm.SelectQuery(cmd).Tables[0];
            string previousincentive = "";
            string fromdt = "";
            string todt = "";
            grdReports.DataSource = Report;
            grdReports.DataBind();
            lblAgent.Text = "";
            lblroute.Text = "";
            lbldate.Text = "";
            lblactualdiscount1.Text = "";
            txtincentivegiven.Text = "";
            txtremarks.Text = "";
            lblmsg.Text = "";

            if (dtincentiveedit.Rows.Count > 0)
            {
                fromdt = dtincentiveedit.Rows[0]["fromdate"].ToString();
                todt = dtincentiveedit.Rows[0]["todate"].ToString();
            }
            if (fromdt == "")
            {
                txtprevDate.Text = "Previous Incentive Not Found";
            }
            if (fromdt != "")
            {
                txtprevDate.Text = fromdt + " " + "TO" + " " + todt;
            }
            Session["incentivesno"] = null;
            btnicentivesave.Visible = true;
            btnPrint.Visible = true;
            Button3.Visible = true;
            //    //DateTime fromdate = DateTime.Now;

            //    //      cmd = new MySqlCommand("SELECT BranchId, StructureName, EntryDate, ActualDiscount, TotalDiscount, Remarks, structure_sno, leakagepercent, FromDate, Todate FROM incentivetransactions WHERE (BranchId = @brnchid) AND (EntryDate BETWEEN @d1 AND @d2)");
            //    cmd = new MySqlCommand("SELECT incentivetransactions.StructureName, incentivetransactions.EntryDate, incentivetransactions.ActualDiscount, incentivetransactions.TotalDiscount,incentivetransactions.DueClear,incentivetransactions.Remarks, incentivetransactions.structure_sno, incentivetransactions.leakagepercent, incentivetransactions.FromDate,incentivetransactions.sno,incentivetransactions.Todate, branchdata.BranchName, incentivetransactions.BranchId FROM incentivetransactions INNER JOIN branchdata ON incentivetransactions.BranchId = branchdata.sno WHERE (incentivetransactions.BranchId = @brnchid) AND (incentivetransactions.EntryDate BETWEEN @d1 AND @d2)");
            //    cmd.Parameters.AddWithValue("@brnchid", ddlAgentName.SelectedValue);
            //    cmd.Parameters.AddWithValue("@d1", GetLowDate(ServerDateCurrentdate));
            //    cmd.Parameters.AddWithValue("@d2", GetHighDate(ServerDateCurrentdate));
            //    DataTable dtincentiveedit = vdm.SelectQuery(cmd).Tables[0];
            //    int dtrowscount = dtincentiveedit.Rows.Count;

            //    cmd = new MySqlCommand("SELECT BranchId, Amount, FineAmount FROM branchaccounts WHERE (BranchId = @branchid)");
            //    cmd.Parameters.AddWithValue("@branchid", ddlAgentName.SelectedValue);
            //    DataTable dtagentdue = vdm.SelectQuery(cmd).Tables[0];
            //    float dueamt = (float)dtagentdue.Rows[0]["Amount"];
            //   // lblagentdue.Text = dueamt.ToString();
            //    //txtdueclear.Text = "0";
            //    //lbl_final_incentive.Text = "0";
            //    Session["incentivesno"] = null;
            //    if (dtrowscount > 0)
            //    {
            //        //Session["incentivesno"] = null;
            //        btnicentivesave.Text = "Edit";
            //        // btnicentivesave.Enabled = false;
            //        float dueclear = 0;
            //        float.TryParse(dtincentiveedit.Rows[0]["DueClear"].ToString(), out dueclear);
            //        float totdue = dueamt + dueclear;
            //        float total_discount = 0;
            //        float.TryParse(dtincentiveedit.Rows[0]["TotalDiscount"].ToString(),out total_discount) ;
            //        float agentfinaldue = totdue - dueclear;
            //        ////lblagentdue.Text = totdue.ToString();
            //        ////txtdueclear.Text = dueclear.ToString();
            //        ////lbl_final_incentive.Text = "0";
            //        float leakage = (float)dtincentiveedit.Rows[0]["leakagepercent"];
            //        int incentivesno = (int)dtincentiveedit.Rows[0]["sno"];
            //        Session["incentivesno"] = incentivesno;
            //        txtleakage.Text = leakage.ToString();
            //        txtleakage.ReadOnly = true;
            //        string EntryDate = dtincentiveedit.Rows[0]["EntryDate"].ToString();
            //        string FromDate = dtincentiveedit.Rows[0]["FromDate"].ToString();
            //        string Todate = dtincentiveedit.Rows[0]["Todate"].ToString();
            //        double ActualDiscount = (double)dtincentiveedit.Rows[0]["ActualDiscount"];
            //        double TotalDiscount = (double)dtincentiveedit.Rows[0]["TotalDiscount"];
            //        string Remarks = dtincentiveedit.Rows[0]["Remarks"].ToString();
            //        int structure_sno = (int)dtincentiveedit.Rows[0]["structure_sno"];
            //        string BranchName = dtincentiveedit.Rows[0]["BranchName"].ToString();
            //        ddlstructure.SelectedValue = structure_sno.ToString();
            //        ddlstructure.Enabled = false;
            //        lbldate.Text = EntryDate;
            //        lblroute.Text = ddlRouteName.SelectedItem.Text;
            //        lblAgent.Text = BranchName;
            //        // txtFromdate.Text = DateTime.Parse(FromDate).ToShortDateString();
            //        DateTime dtfrom = DateTime.Parse(FromDate);
            //        txtFromdate.Text = dtfrom.ToString("dd-MM-yyyy HH:mm");
            //        //txtTodate.Text = DateTime.Parse(Todate).ToShortDateString();
            //        DateTime dtto = DateTime.Parse(Todate);
            //        txtTodate.Text = dtto.ToString("dd-MM-yyyy HH:mm");
            //        lblactualdiscount1.Text = ActualDiscount.ToString();
            //        txtincentivegiven.Text = TotalDiscount.ToString();
            //        txtincentivegiven.ReadOnly = true;
            //        txtremarks.Text = Remarks;
            //        txtremarks.ReadOnly = true;
            //        GetReport();
            //    }
        }
        catch
        {

        }

    }
}