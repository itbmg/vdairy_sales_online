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

public partial class AgentAcknoledgement : System.Web.UI.Page
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
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
                PBranch.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType)");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@SalesType", "21");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                //if (ddlSalesOffice.SelectedIndex == -1)
                //{
                //    ddlSalesOffice.SelectedItem.Text = "Select";
                //}
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));
            }
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
        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
        //cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
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
        cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutes ON dispatch_sub.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (dispatch.sno = @dispsno)");
        cmd.Parameters.AddWithValue("@dispsno", ddlDispName.SelectedValue);
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
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            Report = new DataTable();
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
                    string[] datess = todatestrig[0].Split('-');
                    string[] timess = todatestrig[1].Split(':');
                    todate = new DateTime(int.Parse(datess[2]), int.Parse(datess[1]), int.Parse(datess[0]), int.Parse(timess[0]), int.Parse(timess[1]), 0);
                }
            }
            Session["filename"] = "Statement of Account ->" + ddlAgentName.SelectedItem.Text;
            lblAgent.Text = ddlAgentName.SelectedItem.Text;
            lblroutename.Text = ddlDispName.SelectedItem.Text;
            lbl_fromDate.Text = txtFromdate.Text;
            lbl_selttodate.Text = txtTodate.Text;
            cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost), 2) AS Totalsalevalue, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,products_category.Categoryname, productsdata.ProductName, productsdata.sno AS prodid  FROM productsdata INNER JOIN indents_subtable ON productsdata.sno = indents_subtable.Product_sno INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchdata.sno = @BranchID) GROUP BY productsdata.sno"); 
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtAgent = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT sno, FromDate, Todate, StructureName, BranchId, ActualDiscount, TotalDiscount, Remarks, structure_sno, leakagepercent FROM incentivetransactions WHERE (FromDate BETWEEN @d1 AND @d2) AND (Todate BETWEEN @d3 AND @d4) AND (BranchId = @BranchID)");
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@d3", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d4", GetHighDate(todate));
            DataTable dtincentiveamt = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName, branchroutesubtable.BranchID, inventory_monitor.Inv_Sno, inventory_monitor.Qty FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutesubtable ON dispatch_sub.Route_id = branchroutesubtable.RefNo INNER JOIN inventory_monitor ON branchroutesubtable.BranchID = inventory_monitor.BranchId WHERE (dispatch.sno = @routeid)");
            cmd.Parameters.AddWithValue("@routeid", ddlDispName.SelectedValue);
            DataTable dtinventoryopp = vdm.SelectQuery(cmd).Tables[0];
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);

            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.Sno AS routesno, modifiedroutes.RouteName, invtran.B_inv_sno, SUM(invtran.Qty) AS deliveryqty,modifidroutssubtab.BranchID, branchdata_2.BranchName AS Agentname FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN branchdata branchdata_2 ON modifidroutssubtab.BranchID = branchdata_2.sno LEFT OUTER JOIN (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty, CBFromTran, CBToTran, DeliveryTime,CollectionTime, Remarks FROM invtransactions12 WHERE (TransType = 1) AND (DOE BETWEEN @d1 AND @d2) OR (TransType = 2) AND (DOE BETWEEN @d1 AND @d2)) invtran ON modifidroutssubtab.BranchID = invtran.ToTran WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifidroutssubtab.BranchID, invtran.B_inv_sno, branchdata_2.BranchName ORDER BY branchdata.sno, routesno");
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            DateTime dt3 = GetLowDate(ServerDateCurrentdate.AddDays(-1));
            DateTime dt4 = GetLowDate(ServerDateCurrentdate);
            cmd.Parameters.AddWithValue("@d1", dt3.AddHours(15));
            cmd.Parameters.AddWithValue("@d2", dt4.AddHours(15));
            cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            DataTable dtInvdelivery = vdm.SelectQuery(cmd).Tables[0];

            //cmd = new MySqlCommand("SELECT Sno, SalesOfficeId, RouteId, AgentId,  OppBalance, ClosingBalance, DiffAmount FROM duetransactions WHERE (AgentId = @BranchID) AND (IndentDate BETWEEN @d1 AND @d2)");
            cmd = new MySqlCommand("SELECT BranchId, Amount AS ClosingBalance, FineAmount FROM branchaccounts WHERE (BranchId = @BranchID)");
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            //cmd.Parameters.AddWithValue("@d1", GetLowDate(todate.AddDays(-1)));
            //cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtdueason = vdm.SelectQuery(cmd).Tables[0];
            if (dtAgent.Rows.Count > 0)
            {
                
                cmd = new MySqlCommand("SELECT Amount FROM branchaccounts WHERE (BranchId = @BranchID)");
                cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
                DataTable dtAgent_presentopp = vdm.SelectQuery(cmd).Tables[0];
                double agentpresentopp = 0;
                double.TryParse(dtAgent_presentopp.Rows[0]["Amount"].ToString(), out agentpresentopp);
                int i = 1;
                float count = 0;
                count = (float)(todate - fromdate.AddDays(-1)).TotalDays;
                if (dtincentiveamt.Rows.Count > 0)
                {
                    DataView view = new DataView(dtAgent);
                    DataTable produtstbl = view.ToTable(true, "ProductName", "prodid");
                    Report = new DataTable();
                    Report.Columns.Add("S.NO");
                    Report.Columns.Add("PRODUCT NAME");
                    Report.Columns.Add("TOTAL QTY");
                    Report.Columns.Add("DISCOUNT/LTR");
                    Report.Columns.Add("INCENTIVE AMOUNT");
                    Report.Columns.Add(" ");
                    Report.Columns.Add("  ");
                    Report.Columns.Add("   ");
                    Report.Columns.Add("    ");
                    double totsaleqty = 0;
                    double incentiveamt = 0;
                    double discountperltr = 0;
                    double milksale = 0;
                    double milksalevalue = 0;
                    foreach (DataRow dr in produtstbl.Rows)
                    {
                        float avgsale = 0;
                        float slotqty = 0;
                        float slotamt = 0;
                        float totalsale = 0;
                        float totalsaleamt = 0;
                        string sltamt = "";
                        double invamount = 0;
                        DataRow newrow = Report.NewRow();
                        newrow["S.NO"] = i;
                        newrow["PRODUCT NAME"] = dr["ProductName"].ToString();
                        foreach (DataRow drsale in dtAgent.Select("prodid='" + dr["prodid"].ToString() + "'"))
                        {
                            float.TryParse(drsale["DeliveryQty"].ToString(), out totalsale);
                            float.TryParse(drsale["Totalsalevalue"].ToString(), out totalsaleamt);

                            newrow["TOTAL QTY"] = Math.Round(totalsale, 2);
                            avgsale = (totalsale / count);
                            if (drsale["Categoryname"].ToString() == "MILK")
                            {
                                milksale += totalsale;
                                milksalevalue += totalsaleamt;
                            }
                            
                        }
                        cmd = new MySqlCommand("SELECT subproductsclubbing.Productid, subproductsclubbing.Clubsno, slabs.SlotQty, slabs.Amt FROM incentive_struct_sub INNER JOIN subproductsclubbing ON incentive_struct_sub.clubbingID = subproductsclubbing.Clubsno INNER JOIN slabs ON subproductsclubbing.Clubsno = slabs.club_sno WHERE (incentive_struct_sub.is_sno = @structsno) AND (subproductsclubbing.Productid=@prdtid) ");
                        cmd.Parameters.AddWithValue("@structsno", dtincentiveamt.Rows[0]["structure_sno"].ToString());
                        cmd.Parameters.AddWithValue("@prdtid", dr["prodid"].ToString());
                        DataTable dtprdtclub = vdm.SelectQuery(cmd).Tables[0];
                        DataView clubview = new DataView(dtprdtclub);
                        DataTable clubtbl = clubview.ToTable(true, "Clubsno");
                        //foreach (DataRow drclub in dtprdtclub.Select("Productid='" + dr["prodid"].ToString() + "'"))
                        //{
                        //    if (clubsno != drclub["Clubsno"].ToString())
                        //    {
                        //        clubsno = drclub["Clubsno"].ToString();
                        //        foreach (DataRow drclubsno in dtprdtclub.Select("Clubsno='" + drclub["Clubsno"].ToString() + "'"))
                        //        {
                        //            if (drclub["Productid"].ToString() == drclubsno["Productid"].ToString())
                        //            {
                        //                float.TryParse(drclub["SlotQty"].ToString(), out slotqty);
                        //                if (avgsale > slotqty)
                        //                {
                        //                    float.TryParse(drclub["Amt"].ToString(), out slotamt);
                        //                    sltamt = drclub["Amt"].ToString();
                        //                    double invslot = 0;
                        //                    double.TryParse(sltamt, out invslot);
                        //                    invamount += invslot;
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        foreach (DataRow drclubs in clubtbl.Rows)
                        {

                            foreach (DataRow drclub in dtprdtclub.Select("Clubsno='" + drclubs["Clubsno"].ToString() + "'"))
                            {

                                float.TryParse(drclub["SlotQty"].ToString(), out slotqty);
                                if (avgsale > slotqty)
                                {
                                    float.TryParse(drclub["Amt"].ToString(), out slotamt);
                                    sltamt = drclub["Amt"].ToString();
                                    
                                }

                            }
                            double invslot = 0;
                            double.TryParse(sltamt, out invslot);
                            invamount += invslot;

                        }
                        newrow["INCENTIVE AMOUNT"] = Math.Round(totalsale * invamount, 2);
                        double totamt=0;
                        totamt = Math.Round(totalsale * invamount, 2);
                        newrow["DISCOUNT/LTR"] = Math.Round(totamt / totalsale, 2);
                        Report.Rows.Add(newrow);
                        totsaleqty += totalsale;
                        incentiveamt += Math.Round(totalsale * invamount, 2);
                        discountperltr += Math.Round(totamt / totalsale, 2);
                        i++;
                    }
                    DataRow drleak = Report.NewRow();
                    drleak["PRODUCT NAME"] = "Leakage Incentive";
                    double leakinv = 0;
                    double.TryParse(dtincentiveamt.Rows[0]["leakagepercent"].ToString(), out leakinv);
                    double totalmilksale = 0;
                    totalmilksale = leakinv / 100 * milksale;
                    double TotMilkandMilkAmt = 0;
                    TotMilkandMilkAmt = milksalevalue / milksale;
                    double totleakincentive = 0;

                    totleakincentive = totalmilksale * TotMilkandMilkAmt;

                    drleak["DISCOUNT/LTR"] = leakinv;
                    drleak["INCENTIVE AMOUNT"] = Math.Round(totleakincentive, 2);

                    incentiveamt += totleakincentive;
                    Report.Rows.Add(drleak);
                    DataRow drspl = Report.NewRow();
                    drspl["PRODUCT NAME"] = "Spl Incentive";
                    double actinv = 0;
                    double giveninv = 0;
                    double.TryParse(dtincentiveamt.Rows[0]["ActualDiscount"].ToString(), out actinv);
                    double.TryParse(dtincentiveamt.Rows[0]["TotalDiscount"].ToString(), out giveninv);
                    double totdiff = 0;
                    totdiff = giveninv - actinv;
                    if (totdiff > 1)
                    {
                        drspl["INCENTIVE AMOUNT"] = Math.Round(totdiff, 2);
                        incentiveamt += totdiff;

                    }
                    if (totdiff < 1)
                    {
                        drspl["INCENTIVE AMOUNT"] = "0";
                    }
                    Report.Rows.Add(drspl);
                    DataRow newtotal = Report.NewRow();
                    newtotal["PRODUCT NAME"] = "Total";
                    newtotal["TOTAL QTY"] = Math.Round(totsaleqty, 2);
                    newtotal["DISCOUNT/LTR"] = discountperltr;
                    newtotal["INCENTIVE AMOUNT"] = Math.Round(incentiveamt, 2);
                    Report.Rows.Add(newtotal);
                    DataRow newblank = Report.NewRow();
                    newblank["PRODUCT NAME"] = "";
                    Report.Rows.Add(newblank);
                    DataRow newblank1 = Report.NewRow();
                    newblank1["PRODUCT NAME"] = "";
                    Report.Rows.Add(newblank1);
                    DataRow newblank2 = Report.NewRow();
                    newblank2["PRODUCT NAME"] = "";
                    Report.Rows.Add(newblank2);
                    DataRow newtitle = Report.NewRow();
                    newtitle["DISCOUNT/LTR"] = "ADJUSTMENT DETAILS";
                    Report.Rows.Add(newtitle);
                    DataRow newblank3 = Report.NewRow();
                    newblank3["PRODUCT NAME"] = "";
                    Report.Rows.Add(newblank3);
                    DataRow newreport = Report.NewRow();
                    newreport["S.NO"] = "AMOUNT BALANCE ON " + ServerDateCurrentdate.ToString("dd/MM/yyyy");
                    newreport["PRODUCT NAME"] = "INCENTIVE FOR " + fromdate.ToString("MMMM") + "-" + fromdate.ToString("yyyy");
                    newreport["TOTAL QTY"] = "ACTUAL AMOUNT BALANCE";
                    newreport["DISCOUNT/LTR"] = "CRATES BALANCE";
                    newreport["INCENTIVE AMOUNT"] = "CRATES ALLOWED";
                    newreport[" "] = "CRATES EXCESS";
                    newreport["  "] = "CANS BALANCE";
                    newreport["   "] = "CANS ALLOWED";
                    newreport["    "] = "CANS EXCESS";
                    Report.Rows.Add(newreport);
                    DataRow newadjustment = Report.NewRow();
                    double dueason = 0;
                    double incen = 0;
                    double.TryParse(dtdueason.Rows[0]["ClosingBalance"].ToString(), out dueason);
                    double.TryParse(dtincentiveamt.Rows[0]["TotalDiscount"].ToString(), out incen);
                    newadjustment["S.NO"] = dueason;
                    newadjustment["PRODUCT NAME"] = dtincentiveamt.Rows[0]["TotalDiscount"].ToString();
                    newadjustment["TOTAL QTY"] = Math.Round(dueason - incen, 2);
                    int crates = 0;
                    int cans = 0;
                    int cratesdelivered = 0;
                    int cansdelivered = 0;
                    foreach (DataRow dr in dtinventoryopp.Select("BranchID='" + ddlAgentName.SelectedValue + "'"))
                    {
                        int totcrates = 0;
                        int totcans = 0;
                        if (dr["Inv_Sno"].ToString() == "1")
                        {
                            int.TryParse(dr["Qty"].ToString(), out totcrates);
                            crates += totcrates;
                        }
                        if (dr["Inv_Sno"].ToString() == "2")
                        {
                            int.TryParse(dr["Qty"].ToString(), out totcans);
                            cans += totcans;
                        }
                        if (dr["Inv_Sno"].ToString() == "3")
                        {
                            int.TryParse(dr["Qty"].ToString(), out totcans);
                            cans += totcans;
                        }
                        if (dr["Inv_Sno"].ToString() == "4")
                        {
                            int.TryParse(dr["Qty"].ToString(), out totcans);
                            cans += totcans;
                        }

                    }
                    foreach (DataRow drinvdel in dtInvdelivery.Select("BranchID='" + ddlAgentName.SelectedValue + "'"))
                    {
                        int del = 0;
                        if (drinvdel["B_inv_sno"].ToString() == "1")
                        {
                            int.TryParse(drinvdel["deliveryqty"].ToString(), out del);
                            cratesdelivered += del;
                        }
                        if (drinvdel["B_inv_sno"].ToString() == "2")
                        {
                            int.TryParse(drinvdel["deliveryqty"].ToString(), out del);
                            cansdelivered += del;
                        }
                        if (drinvdel["B_inv_sno"].ToString() == "3")
                        {
                            int.TryParse(drinvdel["deliveryqty"].ToString(), out del);
                            cansdelivered += del;
                        }
                        if (drinvdel["B_inv_sno"].ToString() == "4")
                        {
                            int.TryParse(drinvdel["deliveryqty"].ToString(), out del);
                            cansdelivered += del;
                        }
                    }
                    newadjustment["DISCOUNT/LTR"] = crates;
                    newadjustment["INCENTIVE AMOUNT"] = cratesdelivered;
                    newadjustment[" "] = crates-cratesdelivered;
                    newadjustment["  "] =cans;
                    newadjustment["   "]= cansdelivered;
                    newadjustment["    "] = cans - cansdelivered;
                    Report.Rows.Add(newadjustment);
                    DataRow newblank5 = Report.NewRow();
                    newblank5["PRODUCT NAME"] = "";
                    Report.Rows.Add(newblank5);
                    DataRow newblank6 = Report.NewRow();
                    newblank6["PRODUCT NAME"] = "";
                    Report.Rows.Add(newblank6);
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
            else
            {
                grdReports.DataSource = Report;
                grdReports.DataBind();
                lblmsg.Text = "No Indent Found";
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