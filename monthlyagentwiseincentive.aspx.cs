using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using ClosedXML.Excel;
using System.IO;


public partial class monthlyagentwiseincentive : System.Web.UI.Page
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
                //lblDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                FillAgentName();
                lblTitle.Text = Session["TitleName"].ToString();
            }
        }
    }
    void FillAgentName()
    {
        try
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
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));

            }
            //if (Session["salestype"].ToString() == "Plant")
            //{
            //    PBranch.Visible = true;
            //    cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType)");
            //    cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"].ToString());
            //    cmd.Parameters.AddWithValue("@SalesType", "21");
            //    DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            //    //if (ddlSalesOffice.SelectedIndex == -1)
            //    //{
            //    //    ddlSalesOffice.SelectedItem.Text = "Select";
            //    //}
            //    ddlSalesOffice.DataSource = dtRoutedata;
            //    ddlSalesOffice.DataTextField = "BranchName";
            //    ddlSalesOffice.DataValueField = "sno";
            //    ddlSalesOffice.DataBind();
            //    ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));
            //}
            else
            {
                PBranch.Visible = false;
                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
                //cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlDispName.DataSource = dtRoutedata;
                ddlDispName.DataTextField = "DispName";
                ddlDispName.DataValueField = "sno";
                ddlDispName.DataBind();
                ddlDispName.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch)");
        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) AND (dispatch.flag=@flag) OR (branchdata_1.SalesOfficeID = @SOID) AND (dispatch.flag=@flag)");
        //cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@flag", "1");
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlDispName.DataSource = dtRoutedata;
        ddlDispName.DataTextField = "DispName";
        ddlDispName.DataValueField = "sno";
        ddlDispName.DataBind();
        ddlDispName.Items.Insert(0, new ListItem("Select", "0"));
    }
    protected void ddlDispName_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch)");
        cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutes ON dispatch_sub.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (dispatch.sno = @dispsno) AND dispatch.flag=@flag");
        cmd.Parameters.AddWithValue("@dispsno", ddlDispName.SelectedValue);
        cmd.Parameters.AddWithValue("@flag", "1");
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlAgentName.DataSource = dtRoutedata;
        ddlAgentName.DataTextField = "BranchName";
        ddlAgentName.DataValueField = "sno";
        ddlAgentName.DataBind();
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
            lblRoute.Text = ddlDispName.SelectedItem.Text;
            
            //lblroute.Text = ddlRouteName.SelectedItem.Text;
            lbl_fromDate.Text = fromdate.ToString("dd-MM-yyyy hh:mm");
            lbl_selttodate.Text = todate.ToString("dd-MM-yyyy hh:mm");
            cmd = new MySqlCommand("SELECT sno, FromDate, Todate, StructureName, BranchId,transport,rent, EntryDate, ActualDiscount, TotalDiscount, Remarks, structure_sno, leakagepercent, DueClear FROM incentivetransactions WHERE (FromDate BETWEEN @d1 AND @d2) AND (BranchId = @BranchId)");
            cmd.Parameters.AddWithValue("@branchid", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtstructuresno = vdm.SelectQuery(cmd).Tables[0];

            string structuresno = dtstructuresno.Rows[0]["structure_sno"].ToString();
            string leakpercent = dtstructuresno.Rows[0]["leakagepercent"].ToString();
            string incentivegiven = dtstructuresno.Rows[0]["TotalDiscount"].ToString();
            string remarks = dtstructuresno.Rows[0]["Remarks"].ToString(); 
            string transport = dtstructuresno.Rows[0]["transport"].ToString(); 
            string rent = dtstructuresno.Rows[0]["rent"].ToString(); 
            cmd = new MySqlCommand("SELECT productsdata.sno, productsdata.ProductName, product_clubbing.ClubName, incentive_structure.StructureName, product_clubbing.sno AS clubbingsno,products_category.Categoryname, products_subcategory.category_sno FROM incentive_structure INNER JOIN incentive_struct_sub ON incentive_structure.sno = incentive_struct_sub.is_sno INNER JOIN product_clubbing ON incentive_struct_sub.clubbingID = product_clubbing.sno INNER JOIN subproductsclubbing ON product_clubbing.sno = subproductsclubbing.Clubsno INNER JOIN productsdata ON subproductsclubbing.Productid = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (incentive_structure.sno = @StructureID) ");
            cmd.Parameters.AddWithValue("@StructureID", structuresno);
            DataTable dtincentivestructure = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS Amount,productsdata.ProductName, productsdata.sno AS prdtsno,Inventorysno as invsno, sum(indents_subtable.DeliveryQty) AS DeliveryQty, indents_subtable.UnitCost, products_category.Categoryname,products_category.sno AS categorysno FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchdata.sno = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty <> ' ')  GROUP BY DATE(indents.I_date), branchdata.BranchName, productsdata.ProductName");
            //cmd = new MySqlCommand("SELECT DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate, branchdata.BranchName, indents_subtable.DeliveryQty * indents_subtable.UnitCost AS Amount,productsdata.ProductName, productsdata.sno AS prdtsno,productsdata.Inventorysno as invsno, indents_subtable.DeliveryQty, indents_subtable.UnitCost, products_category.Categoryname,products_category.sno AS categorysno FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchdata.sno = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (indents_subtable.DeliveryQty <> ' ')  GROUP BY indents.I_date, branchdata.BranchName, productsdata.ProductName, indents_subtable.DeliveryQty");
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
                                double UnitCost = 0;
                                double.TryParse(dr["UnitCost"].ToString(), out UnitCost);
                                DeliveryQty = Math.Round(DeliveryQty, 2);
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
                string categoryserial = "9";
                float milkincentive = 0;

                DataView incentiveview = new DataView(dtincentivestructure);
              //  DataTable dticentive = incentiveview.ToTable(true, "ClubName", "clubbingsno", "category_sno");
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
                   // categoryserial = drincetiveclub["category_sno"].ToString();
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

                    if (categoryserial == "9")
                    {
                        milkincentive += (float)Math.Round(totalsale * slotamt, 2);
                    }
                    dtTotincentive.Rows.Add(newrow);
                }
                double totalmilksale = 0;
                string leak = leakpercent;
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
                lblincentivegiven.Text = incentivegiven;
                lblTransport1.Text = transport;
                lblRent1.Text = rent;
                lblincentivegiven.Text = incentivegiven;
                txtremarks.Text = remarks;
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
                
            }
        }
        catch
        {

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
}