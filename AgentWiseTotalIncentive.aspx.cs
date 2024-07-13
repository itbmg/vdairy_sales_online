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
using System.Net;
public partial class TotalIncentive : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    string BranchID = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["salestype"] == null)
        {
            Response.Redirect("Login.aspx");
        }
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
                FillRouteName();
                lblTitle.Text = Session["TitleName"].ToString();
            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        lblmsg.Text = " ";
        GetReport();
    }
    

    void FillRouteName()
    {
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Plant")
            {
                PBranch.Visible = true;
                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("BranchName");
                dtBranch.Columns.Add("sno");
                cmd = new MySqlCommand(" SELECT branchroutes.RouteName, branchroutes.Sno, branchroutes.BranchID FROM branchroutes INNER JOIN branchdata ON branchroutes.BranchID = branchdata.sno WHERE (branchroutes.BranchID = @brnchid) OR (branchdata.SalesOfficeID = @SOID)");
                //cmd = new MySqlCommand("SELECT RouteName, Sno, BranchID FROM branchroutes WHERE (BranchID = @brnchid)");
                cmd.Parameters.AddWithValue("@SOID", BranchID);
                cmd.Parameters.AddWithValue("@brnchid", BranchID);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "RouteName";
                ddlSalesOffice.DataValueField = "Sno";
                ddlSalesOffice.DataBind();
                ddlSalesOffice.Items.Insert(0, new ListItem("Select Route", "0"));
            }
            else
            {
                PBranch.Visible = false;
                //cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
                //cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@BranchD", Session["branch"].ToString());
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlRouteName.DataSource = dtRoutedata;
                ddlRouteName.DataTextField = "DispName";
                ddlRouteName.DataValueField = "sno";
                ddlRouteName.DataBind();
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
        cmd = new MySqlCommand(" SELECT branchdata.sno, branchdata.BranchName, branchroutes.RouteName FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (branchroutes.Sno = @routesno) AND (branchdata.flag=1) ORDER BY branchdata.BranchName");
        //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName, branchroutes.RouteName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchroutesubtable ON branchmappingtable.SubBranch = branchroutesubtable.BranchID INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno WHERE (branchmappingtable.SuperBranch = @SuperBranch) AND (branchroutes.Sno = @routesno) ORDER BY branchdata.BranchName");
        cmd.Parameters.AddWithValue("@SuperBranch", BranchID);
        cmd.Parameters.AddWithValue("@routesno", ddlSalesOffice.SelectedValue);
        DataTable dtbranchdata = vdm.SelectQuery(cmd).Tables[0];
        ddlRouteName.DataSource = dtbranchdata;
        ddlRouteName.DataTextField = "BranchName";
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
    DateTime fromdate = DateTime.Now;
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            lblmsg.Text = "";
            DateTime fromdate = DateTime.Now;
            Report = new DataTable();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            string[] fromdatestrig = txtdate.Text.Split(' ');
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
            string[] todatestrig = txttodate.Text.Split(' ');
            if (todatestrig.Length > 1)
            {
                if (todatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = todatestrig[0].Split('-');
                    string[] times = todatestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            lblRouteName.Text = ddlRouteName.SelectedItem.Text;
            lblDate.Text = fromdate.ToString("dd/MMM/yyyy");
            lbl_selttodate.Text = todate.ToString("dd/MMM/yyyy");
            Session["filename"] = "Agent Wise Incentive";

            // new incentive query naveen
           // cmd = new MySqlCommand("SELECT incentive_maindetails.sno, incentive_maindetails.incentivetype, incentive_maindetails.branchid, incentive_maindetails.routeid, incentive_maindetails.agentid, branchdata.BranchName,   incentive_maindetails.structuretype, incentive_maindetails.leakage, incentive_maindetails.fromdate, incentive_maindetails.todate, incentive_maindetails.remarks, incentive_maindetails.createdby,   incentive_maindetails.approvedby, incentive_maindetails.createddate, incentive_maindetails.status, incentive_subdetails.refno, incentive_subdetails.productid, incentive_subdetails.amount FROM            incentive_maindetails INNER JOIN incentive_subdetails ON incentive_maindetails.sno = incentive_subdetails.refno INNER JOIN branchdata ON branchdata.sno = incentive_maindetails.agentid  WHERE        (incentive_maindetails.branchid = @branchid) AND (incentive_maindetails.routeid = @routeid)");

            //cmd = new MySqlCommand("SELECT result.BranchName, result.StructureName, result.BranchId, result.EntryDate, result.leakagepercent, result.structure_sno, product_clubbing.ClubName FROM (SELECT branchdata.BranchName, incentivetransactions.StructureName, incentivetransactions.BranchId, incentivetransactions.EntryDate, incentivetransactions.structure_sno, incentivetransactions.leakagepercent FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutes ON dispatch_sub.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN incentivetransactions ON branchdata.sno = incentivetransactions.BranchId WHERE (dispatch.sno = 10)) result INNER JOIN incentive_struct_sub ON result.structure_sno = incentive_struct_sub.is_sno INNER JOIN product_clubbing ON incentive_struct_sub.clubbingID = product_clubbing.sno");
            // cmd = new MySqlCommand("SELECT result.BranchName, result.BranchId, result.StructureName, result.leakagepercent, result.EntryDate, result.structure_sno, product_clubbing.ClubName FROM (SELECT        branchdata.BranchName, incentivetransactions.BranchId, incentivetransactions.StructureName, incentivetransactions.structure_sno, incentivetransactions.leakagepercent, incentivetransactions.EntryDate FROM branchroutesubtable INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN incentivetransactions ON branchdata.sno = incentivetransactions.BranchId WHERE (branchroutes.Sno = @dispsno)) result INNER JOIN incentive_struct_sub ON result.structure_sno = incentive_struct_sub.is_sno INNER JOIN product_clubbing ON incentive_struct_sub.clubbingID = product_clubbing.sno");
            cmd = new MySqlCommand("SELECT product_clubbing.ClubName, incentivetransactions.FromDate, incentivetransactions.Todate FROM dispatch_sub INNER JOIN dispatch ON dispatch_sub.dispatch_sno = dispatch.sno INNER JOIN branchroutes ON dispatch_sub.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN incentivetransactions ON branchdata.sno = incentivetransactions.BranchId INNER JOIN incentive_structure ON incentivetransactions.structure_sno = incentive_structure.sno INNER JOIN incentive_struct_sub ON incentive_structure.sno = incentive_struct_sub.is_sno INNER JOIN product_clubbing ON incentive_struct_sub.clubbingID = product_clubbing.sno WHERE (dispatch.sno = @dispsno) AND (incentivetransactions.FromDate BETWEEN @d1 AND @d2) GROUP BY product_clubbing.ClubName,incentivetransactions.FromDate ");
            cmd.Parameters.AddWithValue("@dispsno", ddlRouteName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable dtincentivetranc = vdm.SelectQuery(cmd).Tables[0];
            DataView Dateview = new DataView(dtincentivetranc);
            DataTable distinctDate = Dateview.ToTable(true, "FromDate", "Todate");
            if (dtincentivetranc.Rows.Count > 0)
            {
                DataView view = new DataView(dtincentivetranc);
                DataTable distinctclubbings = view.ToTable(true, "ClubName");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("Agent Name");
                int count = 1;
                foreach (DataRow dr in distinctclubbings.Rows)
                {
                    Report.Columns.Add(dr["ClubName"].ToString()).DataType = typeof(Double);
                }
                Report.Columns.Add("Leakage").DataType = typeof(Double);
                Report.Columns.Add("Actual Amount");
                Report.Columns.Add("Given Amount");
                Report.Columns.Add("Remarks");
                double actdiscount = 0;
                double Gdiscount = 0;
                foreach (DataRow drdate in distinctDate.Rows)
                {
                    DateTime dtfrom = new DateTime();
                    DateTime dtto = new DateTime();
                    string incentivefromdate = drdate["FromDate"].ToString();
                    string incentivetodate = drdate["Todate"].ToString();
                    dtfrom = DateTime.Parse(incentivefromdate);
                    dtto = DateTime.Parse(incentivetodate);
                    incentivefromdate = dtfrom.ToString("dd-MMMM-yyyy");
                    incentivetodate = dtto.ToString("dd-MMMM-yyyy");
                    DataRow br3 = Report.NewRow();
                    br3["Agent Name"] = "";
                    Report.Rows.Add(br3);
                    DataRow br4 = Report.NewRow();
                    br4["Agent Name"] = incentivefromdate + " " + "TO" + " " + incentivetodate;
                    Report.Rows.Add(br4);
                    DataRow br5 = Report.NewRow();
                    br5["Agent Name"] = "";
                    Report.Rows.Add(br5);

                    //cmd = new MySqlCommand("SELECT branchroutes.RouteName, productsdata.ProductName, ROUND(SUM(indents_subtable.unitQty), 2) AS unitQty, products_category.Categoryname FROM indents INNER JOIN branchroutesubtable ON indents.Branch_id = branchroutesubtable.BranchID INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN dispatch_sub ON branchroutes.Sno = dispatch_sub.Route_id INNER JOIN dispatch ON dispatch_sub.dispatch_sno = dispatch.sno WHERE (indents.I_date BETWEEN @starttime AND @endtime) AND (dispatch.sno = @dispatchSno)GROUP BY branchroutes.RouteName, productsdata.ProductName, products_category.Categoryname");
                    //cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
                    //cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
                    //cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate));
                    //DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
                    //modified cmd = new MySqlCommand("SELECT branchdata.BranchName,branchdata.sno,incentivetransactions.StructureName, incentivetransactions.structure_sno, incentivetransactions.FromDate,incentivetransactions.Todate, incentivetransactions.Remarks, incentivetransactions.leakagepercent, incentivetransactions.ActualDiscount,incentivetransactions.TotalDiscount AS givendiscount  FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutesubtable ON dispatch_sub.Route_id = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN incentivetransactions ON branchdata.sno = incentivetransactions.BranchId WHERE (dispatch.sno = @dispatchSno)");
                    cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, incentivetransactions.StructureName, incentivetransactions.structure_sno, incentivetransactions.FromDate, incentivetransactions.Todate, incentivetransactions.Remarks, incentivetransactions.leakagepercent, incentivetransactions.ActualDiscount,incentivetransactions.TotalDiscount AS givendiscount FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutesubtable ON dispatch_sub.Route_id = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN incentivetransactions ON branchdata.sno = incentivetransactions.BranchId WHERE (dispatch.sno = @dispatchSno) AND (incentivetransactions.FromDate= @d1) AND (incentivetransactions.ToDate= @d2)");
                    cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
                    cmd.Parameters.AddWithValue("@d1", dtfrom);
                    cmd.Parameters.AddWithValue("@d2", dtto);
                    DataTable dtbranchsno = vdm.SelectQuery(cmd).Tables[0];
                    //DataTable dtagents = vdm.SelectQuery(cmd).Tables[0];
                    DataView view1 = new DataView(dtbranchsno);
                    DataTable distincttable = view1.ToTable(true, "BranchName", "sno", "structure_sno", "Remarks", "leakagepercent", "ActualDiscount", "givendiscount");
                    foreach (DataRow drbrnch in distincttable.Rows)
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["SNo"] = count;
                        newrow["Agent Name"] = drbrnch["BranchName"].ToString();
                        double leakagepercent = 0;
                        double.TryParse(drbrnch["leakagepercent"].ToString(), out leakagepercent);
                        newrow["Leakage"] = leakagepercent;
                        newrow["Actual Amount"] = drbrnch["ActualDiscount"].ToString();
                        newrow["Given Amount"] = drbrnch["givendiscount"].ToString();
                        newrow["Remarks"] = drbrnch["Remarks"].ToString();
                        double Adic = 0;
                        double Gdic = 0;
                        double.TryParse(drbrnch["ActualDiscount"].ToString(), out Adic);
                        double.TryParse(drbrnch["givendiscount"].ToString(), out Gdic);
                        actdiscount += Adic;
                        Gdiscount += Gdic;
                        cmd = new MySqlCommand("SELECT productsdata.sno, productsdata.ProductName, product_clubbing.ClubName, incentive_structure.StructureName, product_clubbing.sno AS clubbingsno,products_category.Categoryname, products_subcategory.category_sno FROM incentive_structure INNER JOIN incentive_struct_sub ON incentive_structure.sno = incentive_struct_sub.is_sno INNER JOIN product_clubbing ON incentive_struct_sub.clubbingID = product_clubbing.sno INNER JOIN subproductsclubbing ON product_clubbing.sno = subproductsclubbing.Clubsno INNER JOIN productsdata ON subproductsclubbing.Productid = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (incentive_structure.sno = @StructureID) ");
                        cmd.Parameters.AddWithValue("@StructureID", drbrnch["structure_sno"].ToString());
                        DataTable dtincentivestructure = vdm.SelectQuery(cmd).Tables[0];


                        DataView incentiveview = new DataView(dtincentivestructure);
                        DataTable dticentive = incentiveview.ToTable(true, "ClubName", "clubbingsno", "category_sno");
                        cmd = new MySqlCommand("SELECT result.deliveryqty, result.ClubName, result.Clubsno FROM (SELECT ROUND(SUM(indents_subtable.DeliveryQty), 2) AS deliveryqty, subproductsclubbing.Clubsno, product_clubbing.ClubName FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN subproductsclubbing ON indents_subtable.Product_sno = subproductsclubbing.Productid INNER JOIN product_clubbing ON subproductsclubbing.Clubsno = product_clubbing.sno WHERE (indents.Branch_id = @selectedbrnch) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY subproductsclubbing.Clubsno) result INNER JOIN slabs ON result.Clubsno = slabs.club_sno Group By result.Clubsno");
                        cmd.Parameters.AddWithValue("@selectedbrnch", drbrnch["sno"].ToString());
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(dtfrom.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(dtto.AddDays(-1)));
                        DataTable dtclubtotal = vdm.SelectQuery(cmd).Tables[0];
                        foreach (DataRow drincetiveclub in dticentive.Rows)
                        {
                            float avgsale = 0;
                            float slotqty = 0;
                            float slotamt = 0;
                            float totalsale = 0;
                            string sltamt = "";
                            //clubbingname = drincetiveclub["ClubName"].ToString();
                            //categoryserial = drincetiveclub["category_sno"].ToString();
                            foreach (DataRow drdtclubtotal in dtclubtotal.Select("Clubsno='" + drincetiveclub["clubbingsno"].ToString() + "'"))
                            {
                                float.TryParse(drdtclubtotal["deliveryqty"].ToString(), out totalsale);

                            }

                            newrow[drincetiveclub["ClubName"].ToString()] = Math.Round(totalsale, 2);

                        }
                        Report.Rows.Add(newrow);
                        count++;
                    }
                    DataRow br = Report.NewRow();
                    br["Agent Name"] = "";
                    Report.Rows.Add(br);
                    DataRow br1 = Report.NewRow();
                    br1["Agent Name"] = "";
                    Report.Rows.Add(br1);





                }
                DataRow newvartical = Report.NewRow();
                newvartical["Agent Name"] = "Total";
                double val = 0.0;
                foreach (DataColumn dc in Report.Columns)
                {
                    if (dc.DataType == typeof(Double))
                    {
                        val = 0.0;
                        double.TryParse(Report.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                        newvartical[dc.ToString()] = val;
                    }
                }
                newvartical["Actual Amount"] = actdiscount;
                newvartical["Given Amount"] = Gdiscount;

                Report.Rows.Add(newvartical);
                cmd = new MySqlCommand("SELECT SUM(incentivetransactions.ActualDiscount) AS actdisc, SUM(incentivetransactions.TotalDiscount) AS totdiscount,branchdata.sno,branchdata.BranchName FROM dispatch_sub INNER JOIN dispatch ON dispatch_sub.dispatch_sno = dispatch.sno INNER JOIN branchroutes ON dispatch_sub.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN incentivetransactions ON branchdata.sno = incentivetransactions.BranchId WHERE (dispatch.sno = @dispsno) AND (incentivetransactions.FromDate BETWEEN @d1 AND @d2) GROUP BY branchdata.sno, branchdata.BranchName");
                cmd.Parameters.AddWithValue("@dispsno", ddlRouteName.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                DataTable dtbranchtotincen = vdm.SelectQuery(cmd).Tables[0];
                DataRow br6 = Report.NewRow();
                br6["Agent Name"] = "";
                Report.Rows.Add(br6);
                DataRow totincentive = Report.NewRow();
                totincentive["Agent Name"] = "TOTAL INCENTIVE";
                Report.Rows.Add(totincentive);
                DataRow br7 = Report.NewRow();
                br7["Agent Name"] = "";
                Report.Rows.Add(br7);
                //int t=2;
                DataRow br8 = Report.NewRow();
                br8["SNo"] = "Agent Code";
                br8["Agent Name"] = "Agent Name";
                br8["Actual Amount"] = "Actual Incentive";
                br8["Given Amount"] = "Given Incentive";
                Report.Rows.Add(br8);
                double finalact = 0;
                double finalgiven = 0;
                foreach (DataRow drbrnchincentive in dtbranchtotincen.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = drbrnchincentive["sno"].ToString();
                    newrow["Agent Name"] = drbrnchincentive["BranchName"].ToString();
                    newrow["Actual Amount"] = drbrnchincentive["actdisc"].ToString();
                    newrow["Given Amount"] = drbrnchincentive["totdiscount"].ToString();
                    double finalactual = 0;
                    double fingiven = 0;
                    double.TryParse(drbrnchincentive["actdisc"].ToString(), out finalactual);
                    double.TryParse(drbrnchincentive["totdiscount"].ToString(), out fingiven);
                    finalact += finalactual;
                    finalgiven += fingiven;
                    Report.Rows.Add(newrow);

                }
                DataRow brtot = Report.NewRow();
                brtot["Agent Name"] = "TOTAL";
                brtot["Actual Amount"] = finalact;
                brtot["Given Amount"] = finalgiven;
                Report.Rows.Add(brtot);
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["Report"] = dtbranchtotincen;
            }

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdReports.DataSource = Report;
            grdReports.DataBind();
        }
    }
    protected void btnMessage_Click(object sender, EventArgs e)
    {
        try
        {
            lblmsg.Text = "";
            if (Session["Report"] != null)
            {
                vdm = new VehicleDBMgr();
                DataTable dtIncentive = (DataTable)Session["Report"];
                foreach (DataRow dr in dtIncentive.Rows)
                {
                    if (dr["sno"].ToString() == "")
                    {

                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT sno,BranchName, phonenumber FROM branchdata where sno=@BranchName");
                        cmd.Parameters.AddWithValue("@BranchName", dr["sno"].ToString());
                        DataTable dtBranchdata = vdm.SelectQuery(cmd).Tables[0];
                        if (dtBranchdata.Rows.Count > 0)
                        {
                            string BranchName = dtBranchdata.Rows[0]["BranchName"].ToString();
                            string phonenumber = dtBranchdata.Rows[0]["phonenumber"].ToString();
                            string IncentiveAmount = dr["totdiscount"].ToString();
                            
                            
                            DateTime fromdate = DateTime.Now;

                            string[] fromdatestrig = txtdate.Text.Split(' ');
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
                            string[] todatestrig = txttodate.Text.Split(' ');
                            if (todatestrig.Length > 1)
                            {
                                if (todatestrig[0].Split('-').Length > 0)
                                {
                                    string[] dates = todatestrig[0].Split('-');
                                    string[] times = todatestrig[1].Split(':');
                                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                                }
                            }
                            string fDate = fromdate.ToString("dd/MM/yyyy");
                            string tDate = todate.ToString("dd/MM/yyyy");
                            if (phonenumber.Length == 10)
                            {
                                WebClient client = new WebClient();
                                //string strUrl = " http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + no + "&msg=" + message1 + "&type=1 ";

                                string baseurl = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + phonenumber + "&message=Dear%20" + BranchName + "%20,%20 Your Incentive Amount For Date%20%20" + fDate + "%20To%20" + tDate + "%20Amount:%20" + IncentiveAmount + "&sender=VYSNVI&type=1&route=2"; 

                               // string baseurl = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + phonenumber + "&msg=Dear%20" + BranchName + "%20,%20 Your Incentive Amount For Date%20%20" + fDate + "%20To%20" + tDate + "%20Amount:%20" + IncentiveAmount + "&type=1";
                                Stream data = client.OpenRead(baseurl);
                                StreamReader reader = new StreamReader(data);
                                string ResponseID = reader.ReadToEnd();
                                data.Close();
                                reader.Close();
                            }
                        }
                    }
                }
                lblmsg.Text = "SMS Sent Successfully";
            }
            else
            {
                lblmsg.Text = "No data were found";
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
}