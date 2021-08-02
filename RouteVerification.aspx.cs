using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;
using System.Drawing;

public partial class RouteVerification : System.Web.UI.Page
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
            PBranch.Visible = true;
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) ");
            cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
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
            cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID)  and (DispType is NULL)) OR ((branchdata_1.SalesOfficeID = @SOID)  and (DispType is NULL))");
            //cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            ddlRouteName.DataSource = dtRoutedata;
            ddlRouteName.DataTextField = "DispName";
            ddlRouteName.DataValueField = "sno";
            ddlRouteName.DataBind();
        }
    }
    protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID)  and (DispType is NULL)) OR ((branchdata_1.SalesOfficeID = @SOID)  and (DispType is NULL))");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlRouteName.DataSource = dtRoutedata;
        ddlRouteName.DataTextField = "DispName";
        ddlRouteName.DataValueField = "sno";
        ddlRouteName.DataBind();
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
    string routeid = "";
    string routeitype = "";
    string tripid = "";
    string employeename = "";
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            Report = new DataTable();
            Session["RouteName"] = ddlRouteName.SelectedItem.Text;
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
            lblRoutName.Text = ddlRouteName.SelectedItem.Text;
            Session["filename"] = "AGENT WISE DELIVERY REPORT";
            //cmd = new MySqlCommand("select Route_id,IndentType from dispatch_sub where dispatch_sno=@dispsno");
            cmd = new MySqlCommand("SELECT dispatch_sub.Route_id, dispatch_sub.IndentType, tripdat.Sno AS tripid, tripdat.EmpId, empmanage.EmpName FROM (SELECT Sno, EmpId, I_Date FROM tripdata WHERE (I_Date BETWEEN @d1 AND @d2)) tripdat INNER JOIN triproutes ON tripdat.Sno = triproutes.Tripdata_sno INNER JOIN dispatch_sub ON triproutes.RouteID = dispatch_sub.dispatch_sno INNER JOIN empmanage ON tripdat.EmpId = empmanage.Sno WHERE (dispatch_sub.dispatch_sno = @dispsno)"); 
            cmd.Parameters.AddWithValue("@dispsno", ddlRouteName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtrouteindenttype = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow drrouteitype in dtrouteindenttype.Rows)
            {
                routeid = drrouteitype["Route_id"].ToString();
                routeitype = drrouteitype["IndentType"].ToString();
                tripid = drrouteitype["tripid"].ToString();
                employeename = drrouteitype["EmpName"].ToString();
            }
            lbltripid.Text = tripid;
            lblsalesmen.Text = employeename;
            cmd = new MySqlCommand("SELECT branchdata.BranchName, modifiedroutes.RouteName, branchdata.sno AS BSno, indent.IndentType, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,indents_subtable.UnitCost, productsdata.ProductName, productsdata.Units, productsdata.sno, products_category.Categoryname, invmaster.Qty,brnchprdt.Rank FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN branchdata ON modifiedroutesubtable.BranchID = branchdata.sno INNER JOIN (SELECT IndentNo, Branch_id, I_date, Status, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (Status <> 'D')) indent ON branchdata.sno = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno INNER JOIN (SELECT branch_sno, product_sno, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno WHERE (modifiedroutes.Sno = @TripID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (modifiedroutes.Sno = @TripID) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY productsdata.sno, BSno ORDER BY brnchprdt.Rank");
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            }
            cmd.Parameters.AddWithValue("@TripID", routeid);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];


            cmd = new MySqlCommand("SELECT SUM(collction.AmountPaid) AS amountpaid, branchdata.sno, branchdata.BranchName FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT Branchid, UserData_sno, AmountPaid, PaidDate, PaymentType, tripId, CheckStatus, VarifyDate, ChequeDate FROM collections WHERE (PaidDate BETWEEN @d1 AND @d2) AND (CheckStatus IS NULL)) collction ON modifiedroutesubtable.BranchID = collction.Branchid INNER JOIN branchdata ON collction.Branchid = branchdata.sno WHERE (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @d2) OR (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate > @d1) AND (modifiedroutesubtable.CDate <= @d2) GROUP BY branchdata.sno, branchdata.BranchName");
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            }
            cmd.Parameters.AddWithValue("@RouteID", routeid);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            DataTable dtCashcollection = vdm.SelectQuery(cmd).Tables[0];


            cmd = new MySqlCommand("SELECT SUM(collction.AmountPaid) AS amountpaid, branchdata.sno, branchdata.BranchName FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT Branchid, UserData_sno, AmountPaid, PaidDate, PaymentType, tripId, CheckStatus, VarifyDate, ChequeDate FROM collections WHERE (PaidDate BETWEEN @d1 AND @d2) AND (CheckStatus = 'P')) collction ON modifiedroutesubtable.BranchID = collction.Branchid INNER JOIN branchdata ON collction.Branchid = branchdata.sno WHERE (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @d2) OR (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate > @d1) AND (modifiedroutesubtable.CDate <= @d2) GROUP BY branchdata.sno, branchdata.BranchName");
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            }
            cmd.Parameters.AddWithValue("@RouteID", routeid);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            DataTable dtChequeCollected = vdm.SelectQuery(cmd).Tables[0];



            cmd = new MySqlCommand("SELECT SUM(collction.AmountPaid) AS amountpaid, branchdata.sno, branchdata.BranchName FROM modifiedroutes INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT Branchid, UserData_sno, AmountPaid, PaidDate, PaymentType, tripId, CheckStatus, VarifyDate, ChequeDate FROM collections WHERE (VarifyDate BETWEEN @d1 AND @d2) AND (CheckStatus = 'V')) collction ON modifiedroutesubtable.BranchID = collction.Branchid INNER JOIN branchdata ON collction.Branchid = branchdata.sno WHERE (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @d2) OR (modifiedroutes.Sno = @RouteID) AND (modifiedroutesubtable.EDate > @d1) AND (modifiedroutesubtable.CDate <= @d2) GROUP BY branchdata.sno, branchdata.BranchName");
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            }
            cmd.Parameters.AddWithValue("@RouteID", routeid);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            DataTable dtChequeVerified = vdm.SelectQuery(cmd).Tables[0];

            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);

            cmd = new MySqlCommand("SELECT branchaccounts.Amount, branchaccounts.BranchId FROM branchroutesubtable INNER JOIN branchaccounts ON branchroutesubtable.BranchID = branchaccounts.BranchId WHERE (branchroutesubtable.RefNo = @RouteID)");
            cmd.Parameters.AddWithValue("@RouteID", routeid);
            DataTable dtbalance = vdm.SelectQuery(cmd).Tables[0];

            
            if (dtble.Rows.Count > 0)
            {
                DataView view = new DataView(dtble);
                DataTable produtstbl = view.ToTable(true, "ProductName", "sno");
                DataTable distincttable = view.ToTable(true, "BranchName", "BSno");
                double totalopp = 0;
                Report = new DataTable();
                Report.Columns.Add("Code");
                Report.Columns.Add("Agent Name Description");
                Report.Columns.Add("O.Bal");
                Report.Columns.Add("Indent Qty");
                Report.Columns.Add("Delivered Qty");
                Report.Columns.Add("Unit Price");
                Report.Columns.Add("Sale Value");
                Report.Columns.Add("Cash Collected");
                Report.Columns.Add("Cheque Collected");
                Report.Columns.Add("Cheque Verified");
                Report.Columns.Add("C.Bal");
                Report.Columns.Add("Agent Signature");
                foreach (DataRow branch in distincttable.Rows)
                {
                    cmd = new MySqlCommand("SELECT invtras.TransType, invtras.FromTran, invtras.ToTran, invtras.Qty, invtras.DOE, invmaster.sno AS invsno, invmaster.InvName FROM (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty FROM invtransactions12 WHERE (ToTran = @branchid) AND (DOE BETWEEN @d1 AND @d2) AND (Qty <> 0) OR (DOE BETWEEN @d1 AND @d2) AND (Qty <> 0) AND (FromTran = @branchid)) invtras INNER JOIN invmaster ON invtras.B_inv_sno = invmaster.sno ORDER BY invtras.DOE");
                    DateTime dtmin = GetLowDate(fromdate.AddDays(-1));
                    DateTime dtmax = GetLowDate(fromdate);
                    cmd.Parameters.AddWithValue("@d1", dtmin.AddHours(15));
                    cmd.Parameters.AddWithValue("@d2", dtmax.AddHours(15));
                    cmd.Parameters.AddWithValue("@branchid", branch["BSno"].ToString());
                    DataTable dtInventoryDC = vdm.SelectQuery(cmd).Tables[0];
                    cmd = new MySqlCommand("SELECT invtran.TransType, invtran.FromTran, invtran.ToTran, invtran.Qty, invtran.DOE, invmaster.sno AS invsno, invmaster.InvName FROM (SELECT TransType, FromTran, ToTran, Qty, EmpID, VarifyStatus, VTripId, VEmpId, Sno, B_inv_sno, DOE, VQty FROM invtransactions12 WHERE (ToTran = @branchid) AND (DOE BETWEEN @d1 AND @d2) OR (DOE BETWEEN @d1 AND @d2) AND (FromTran = @branchid)) invtran INNER JOIN invmaster ON invtran.B_inv_sno = invmaster.sno ORDER BY invtran.DOE");
                    DateTime dt1 = GetLowDate(fromdate.AddDays(-1));
                    DateTime dt2 = GetLowDate(ServerDateCurrentdate);
                    cmd.Parameters.AddWithValue("@d1", dt1.AddHours(15));
                    cmd.Parameters.AddWithValue("@d2", dt2.AddHours(15));
                    cmd.Parameters.AddWithValue("@branchid", branch["BSno"].ToString());
                    DataTable dtprevinventoryDC = vdm.SelectQuery(cmd).Tables[0];

                    cmd = new MySqlCommand("SELECT inventory_monitor.Inv_Sno, invmaster.InvName, inventory_monitor.Qty FROM inventory_monitor INNER JOIN invmaster ON inventory_monitor.Inv_Sno = invmaster.sno WHERE (inventory_monitor.BranchId = @branchid)");
                    cmd.Parameters.AddWithValue("@branchid", branch["BSno"].ToString());
                    DataTable dtAgentinventory = vdm.SelectQuery(cmd).Tables[0];

                    DataRow newrow = Report.NewRow();
                    newrow["Code"] = branch["BSno"].ToString();
                    newrow["Agent Name Description"] = branch["BranchName"].ToString();
                    Report.Rows.Add(newrow);
                    DataRow newrowbrk = Report.NewRow();
                    newrowbrk["Code"] = "";
                    newrowbrk["Agent Name Description"] = "";
                    Report.Rows.Add(newrowbrk);
                    double indqty = 0;
                    double saleqty = 0;
                    double salevalue = 0;
                    double amountcollected = 0;
                    double Cheque_amountcollected = 0;
                    double Cheque_amount_received = 0;
                    double agentpresentopp = 0;
                    foreach (DataRow drprdts in dtble.Select("BSno='" + branch["BSno"].ToString() + "'"))
                    {
                        DataRow newprdts = Report.NewRow();
                        newprdts["Code"] = drprdts["sno"].ToString();
                        newprdts["Agent Name Description"] = drprdts["ProductName"].ToString();
                        newprdts["Indent Qty"] = drprdts["DeliveryQty"].ToString();
                        newprdts["Delivered Qty"] = drprdts["DeliveryQty"].ToString();
                        newprdts["Unit Price"] = drprdts["UnitCost"].ToString();
                        double indentqty = 0;
                        double deliveryqty = 0;
                        double unitprice = 0;
                        double.TryParse(drprdts["DeliveryQty"].ToString(), out indentqty);
                        double.TryParse(drprdts["DeliveryQty"].ToString(), out deliveryqty);
                        double.TryParse(drprdts["UnitCost"].ToString(), out unitprice);
                        double finalvalue = 0;
                        finalvalue = deliveryqty * unitprice;
                        newprdts["Sale Value"] = Math.Round(finalvalue, 2);
                        salevalue += Math.Round(finalvalue, 2);
                        saleqty += Math.Round(deliveryqty, 2);
                        indqty += Math.Round(deliveryqty, 2);
                        Report.Rows.Add(newprdts);


                    }
                    foreach (DataRow drcashcoll in dtCashcollection.Select("sno='" + branch["BSno"].ToString() + "'"))
                    {
                        double.TryParse(drcashcoll["amountpaid"].ToString(), out amountcollected);
                    }
                    foreach (DataRow drcashcoll in dtChequeCollected.Select("sno='" + branch["BSno"].ToString() + "'"))
                    {
                        double.TryParse(drcashcoll["amountpaid"].ToString(), out Cheque_amountcollected);
                    }
                    foreach (DataRow drcashcoll in dtChequeVerified.Select("sno='" + branch["BSno"].ToString() + "'"))
                    {
                        double.TryParse(drcashcoll["amountpaid"].ToString(), out Cheque_amount_received);
                    }
                    foreach (DataRow dropp in dtbalance.Select("BranchId='" + branch["BSno"].ToString() + "'"))
                    {
                        double.TryParse(dropp["Amount"].ToString(), out agentpresentopp);

                    }
                    cmd = new MySqlCommand("SELECT totalsaleamount.totalsale, totalsaleamount.Branch_id, SUM(collections.AmountPaid) AS amountpaid FROM (SELECT        SUM(indentssub.DeliveryQty * indentssub.UnitCost) AS totalsale, indents.Branch_id FROM indents INNER JOIN (SELECT IndentNo, Product_sno, Qty, Cost, Remark, DeliveryQty, Status, D_date, unitQty, UnitCost, Sno, PaymentStatus, LeakQty, OTripId, DTripId, DelTime FROM indents_subtable) indentssub ON indents.IndentNo = indentssub.IndentNo WHERE (indents.Branch_id = @BranchID) AND (indents.I_date BETWEEN @starttime1 AND @endtime) GROUP BY indents.Branch_id) totalsaleamount INNER JOIN (SELECT Branchid, UserData_sno, AmountPaid, Denominations, Remarks, Sno, PaidDate, PaymentType, tripId, CheckStatus, ReturnDenomin, PayTime, VEmpID, ChequeNo, EmpID, ReceiptNo FROM collections collections_1 WHERE (Branchid = @BranchID) AND (PaidDate BETWEEN @starttime AND @endtime) AND (CheckStatus IS NULL) OR (Branchid = @BranchID) AND (CheckStatus = 'V') AND (VarifyDate BETWEEN @starttime AND @endtime)) collections ON totalsaleamount.Branch_id = collections.Branchid");
                    cmd.Parameters.AddWithValue("@BranchID", branch["BSno"].ToString());
                    cmd.Parameters.AddWithValue("@starttime1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
                    cmd.Parameters.AddWithValue("@endtime", GetHighDate(ServerDateCurrentdate));
                    DataTable dtSaleCollection = vdm.SelectQuery(cmd).Tables[0];
                    double totsale = 0;
                    double totamt = 0;
                    if (dtSaleCollection.Rows.Count > 0)
                    {
                        double.TryParse(dtSaleCollection.Rows[0]["totalsale"].ToString(), out totsale);
                        double.TryParse(dtSaleCollection.Rows[0]["amountpaid"].ToString(), out totamt);
                    }
                    else
                    {
                        totsale = 0;
                        totamt = 0;
                    }
                    double aopp = agentpresentopp + totamt - totsale;
                    DataRow drtotal = Report.NewRow();
                    drtotal["Agent Name Description"] = "Total";
                    drtotal["O.Bal"] = Math.Round(aopp, 2);
                    totalopp += Math.Round(aopp, 2);
                    double totalamt = aopp + salevalue;
                    double amtpaid = 0;
                    amtpaid = amountcollected + Cheque_amount_received;
                    double totalbalance = totalamt - amtpaid;

                    drtotal["Indent Qty"] = indqty;
                    drtotal["Delivered Qty"] = saleqty;
                    drtotal["Sale Value"] = salevalue;
                    drtotal["Cash Collected"] = amountcollected;
                    drtotal["Cheque Collected"] = Cheque_amountcollected;
                    drtotal["Cheque Verified"] = Cheque_amount_received;
                    drtotal["C.Bal"] = Math.Round(totalbalance, 2);

                    Report.Rows.Add(drtotal);

                    DataRow drinventory = Report.NewRow();
                    drinventory["Agent Name Description"] = "Inventory Name";
                    drinventory["O.Bal"] = "O.Bal";
                    drinventory["Indent Qty"] = "Issued";
                    drinventory["Delivered Qty"] = "Received";
                    drinventory["Unit Price"] = "C.Bal";
                    Report.Rows.Add(drinventory);

                    foreach (DataRow dragentinv in dtAgentinventory.Rows)
                    {
                        int oppcrates = 0;
                        int Ctotcrates = 0;
                        int Dtotcrates = 0;
                        int prevDtotcrates = 0;
                        int prevCtotcrates = 0;
                        string invname = dragentinv["InvName"].ToString();
                        int.TryParse(dragentinv["Qty"].ToString(), out oppcrates);
                        foreach (DataRow drprev in dtprevinventoryDC.Select("invsno='" + dragentinv["Inv_Sno"].ToString() + "'"))
                        {
                            if (drprev["TransType"].ToString() == "2")
                            {
                                
                                int prevDcrates = 0;
                                int.TryParse(drprev["Qty"].ToString(), out prevDcrates);
                                prevDtotcrates += prevDcrates;
                                
                            }
                            if (drprev["TransType"].ToString() == "1" || drprev["TransType"].ToString() == "3")
                            {
                                
                                int prevCcrates = 0;
                                int.TryParse(drprev["Qty"].ToString(), out prevCcrates);
                                prevCtotcrates += prevCcrates;
                               
                            }
                        }
                        foreach (DataRow dr in dtInventoryDC.Select("invsno='" + dragentinv["Inv_Sno"].ToString() + "'"))
                        {
                            if (dr["TransType"].ToString() == "2")
                            {
                                
                                int Dcrates = 0;
                                int.TryParse(dr["Qty"].ToString(), out Dcrates);
                                Dtotcrates += Dcrates;
                                

                            }
                            if (dr["TransType"].ToString() == "1" || dr["TransType"].ToString() == "3")
                            {
                                
                                int Ccrates = 0;
                                int.TryParse(dr["Qty"].ToString(), out Ccrates);
                                Ctotcrates += Ccrates;
                                
                            }
                        }
                        oppcrates = oppcrates + prevCtotcrates - prevDtotcrates;
                        int CratesClo = oppcrates + Dtotcrates - Ctotcrates;
                        
                        DataRow drnew = Report.NewRow();

                        drnew["Agent Name Description"] = invname;
                        drnew["O.Bal"] = oppcrates;
                        drnew["Indent Qty"] = Dtotcrates;
                        drnew["Delivered Qty"] = Ctotcrates;
                        drnew["Unit Price"] = CratesClo;
                        Report.Rows.Add(drnew);
                    }

                    DataRow drnewbreak = Report.NewRow();
                    drnewbreak["Agent Name Description"] = "  ";
                    Report.Rows.Add(drnewbreak);
                }
                lbl_oppamount.Text = totalopp.ToString();
                
            }
            

            grdReports.DataSource = Report;
            grdReports.DataBind();

            for (int ii = 0; ii < grdReports.Rows.Count; ii++)
            {
                GridViewRow dgvr = grdReports.Rows[ii];
                if (dgvr.Cells[1].Text.Contains("  "))
                {

                   // grdReports.Rows[ii].GridLines = GridLines.Horizontal;
                    GridViewRow compare = grdReports.Rows[ii];
                    for (int rowcnt = 0; rowcnt < dgvr.Cells.Count; rowcnt++)
                    {

                        compare.Cells[rowcnt].BackColor = Color.DarkGray;

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