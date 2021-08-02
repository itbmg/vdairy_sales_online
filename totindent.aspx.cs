using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Net;

public partial class totindent : System.Web.UI.Page
{
    MySqlCommand cmd;
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["salestype"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        //vdm = new VehicleDBMgr();
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                FillDispName();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
            }
        }


    }
    void FillDispName()
    {
        try
        {
            string salestype = Session["salestype"].ToString();
            if (salestype == "Plant")
            {
                vdm = new VehicleDBMgr();
                cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD) and (DispMode Is NULL) and (flag=@flag)");
                cmd.Parameters.AddWithValue("@BranchD", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@flag", "1");

                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlRouteName.DataSource = dtRoutedata;
                ddlRouteName.DataTextField = "DispName";
                ddlRouteName.DataValueField = "sno";
                ddlRouteName.DataBind();
                ddlRouteName.Items.Insert(0, new ListItem("Select", "0"));
                PanelDen.Visible = false;
            }
            if (salestype == "SALES OFFICE")
            {
                vdm = new VehicleDBMgr();
                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM branchdata INNER JOIN branchroutes ON branchdata.sno = branchroutes.BranchID INNER JOIN dispatch ON branchroutes.BranchID = dispatch.BranchID WHERE (branchdata.SalesOfficeID = @SOID) OR (branchroutes.BranchID = @BranchID)  and  (dispatch.flag=@flag) GROUP BY dispatch.DispName");
                //cmd = new MySqlCommand("SELECT DispName, sno, Branch_Id, BranchID FROM (SELECT dispatch.DispName, dispatch.sno, dispatch.Branch_Id, branchroutes.BranchID FROM dispatch_sub INNER JOIN branchroutes ON dispatch_sub.Route_id = branchroutes.Sno RIGHT OUTER JOIN dispatch ON dispatch_sub.dispatch_sno = dispatch.sno WHERE (NOT (branchroutes.BranchID = @BranchID)) OR (NOT (dispatch.Branch_Id = @branchid))) Result  WHERE (BranchID = @BranchID)GROUP BY DispName");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@flag", "1");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlRouteName.DataSource = dtRoutedata;
                ddlRouteName.DataTextField = "DispName";
                ddlRouteName.DataValueField = "sno";
                ddlRouteName.DataBind();
                PanelDen.Visible = true;
            }
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
    protected void btnSMS_Click(object sender, EventArgs e)
    {
        try
        {

            string MobNo = txtMobNo.Text;
            lblmsg.Text = "";
            if (MobNo.Length == 10)
            {
                vdm = new VehicleDBMgr();
                DataTable Report = new DataTable();
                DateTime fromdate = DateTime.Now;
                string[] datestrig = txtdate.Text.Split(' ');
                if (datestrig.Length > 1)
                {
                    if (datestrig[0].Split('-').Length > 0)
                    {
                        string[] dates = datestrig[0].Split('-');
                        string[] times = datestrig[1].Split(':');
                        fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                    }
                }
                Session["filename"] = "Total Indent REPORT";


                string branchid = Session["branch"].ToString();

                cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, ROUND(SUM(indents_subtable.unitQty), 2) AS unitQty, modifiedroutes.Sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutes ON dispatch_sub.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT IndentNo, Branch_id, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indent ON modifiedroutesubtable.BranchID = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo WHERE (branchdata.sno = @BranchID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (branchdata_1.SalesOfficeID = @SOID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY modifiedroutes.Sno");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate));
                DataTable dtble = vdm.SelectQuery(cmd).Tables[0];




                cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, modifiedroutes.Sno, ROUND(SUM(offer_indents_sub.offer_indent_qty), 2) AS unitQty FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutes ON dispatch_sub.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT idoffer_indents, idoffers_assignment, salesoffice_id, route_id, agent_id, indent_date, indents_id, IndentType, I_modified_by FROM offer_indents WHERE (indent_date BETWEEN @starttime AND @endtime)) offerindent ON modifiedroutesubtable.BranchID = offerindent.agent_id INNER JOIN offer_indents_sub ON offerindent.idoffer_indents = offer_indents_sub.idoffer_indents WHERE (branchdata.sno = @BranchID) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) AND (branchdata_1.SalesOfficeID = @SOID) OR (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY modifiedroutes.Sno");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate));
                DataTable dt_offertble = vdm.SelectQuery(cmd).Tables[0];
                double TotalQty = 0;
                string ProductName = "";
                if (dtble.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtble.Rows)
                    {
                        double offerindent = 0;
                        foreach (DataRow drindt in dt_offertble.Select("Sno='" + dr["Sno"].ToString() + "'"))
                        {
                            double.TryParse(drindt["unitQty"].ToString(), out offerindent);
                        }
                        double unitQty = 0;
                        double totunitQty = 0;
                        double.TryParse(dr["unitQty"].ToString(), out unitQty);
                        totunitQty = unitQty + offerindent;
                        //ProductName += dr["RouteName"].ToString() + "->" + Math.Round(unitQty, 2) + ";" + "\r\n";
                        ProductName += dr["RouteName"].ToString() + "->" + Math.Round(totunitQty, 2) + ";" + "\r\n";
                        //TotalQty += Math.Round(unitQty, 2);
                        TotalQty += Math.Round(totunitQty, 2);
                    }
                }
                string Date = DateTime.Now.ToString("dd/MM/yyyy");
                WebClient client = new WebClient();
                //string SalesOfficeName = ddlSalesOffice.SelectedItem.Text;
                string SalesOfficeName = Session["branchname"].ToString();
                string baseurl = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VSALES&to=" + MobNo + "&msg=%20" + SalesOfficeName + "%20,%20 + TOTAL INDENT FOR TODAY" + "%20:%20" + fromdate + "%20%20" + ProductName + "TotalQty ->" + TotalQty + "&type=1";
                Stream data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                string ResponseID = reader.ReadToEnd();
                data.Close();
                reader.Close();
                lblmsg.Text = "Message Sent Successfully";
                txtMobNo.Text = "";
            }
            else
            {
                lblmsg.Text = "Please Enter 10 digit Number";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;

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
    DateTime fromdate = DateTime.Now;
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            //DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            lblDispatchName.Text = ddlRouteName.SelectedItem.Text;
            vdm = new VehicleDBMgr();
            DataTable Report = new DataTable();
            DataTable TotReport = new DataTable();
            string[] datestrig = txtdate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            lblDate.Text = fromdate.AddDays(1).ToString("dd/MM/yyyy");
            Session["RouteName"] = ddlRouteName.SelectedItem.Text + fromdate.AddDays(1).ToString("dd/MM/yyyy");
            Session["filename"] = ddlRouteName.SelectedItem.Text + fromdate.AddDays(1).ToString("dd/MM/yyyy");


            string branchid = Session["branch"].ToString();

            DataTable dtble = new DataTable();
            cmd = new MySqlCommand("select Route_id,IndentType from dispatch_sub where dispatch_sno=@dispsno");
            cmd.Parameters.AddWithValue("@dispsno", ddlRouteName.SelectedValue);
            DataTable dtrouteindenttype = vdm.SelectQuery(cmd).Tables[0];
            var routeitype = "";
            foreach (DataRow drrouteitype in dtrouteindenttype.Rows)
            {
                var routeid = drrouteitype["Route_id"].ToString();
                routeitype = drrouteitype["IndentType"].ToString();
            }


            if (ddlRouteName.SelectedValue == "14" || ddlRouteName.SelectedValue == "18" || ddlRouteName.SelectedValue == "31")
            {
                cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, ROUND(SUM(indents_subtable.unitQty), 2) AS unitQty, productsdata.ProductName, productsdata.Units, products_category.Categoryname, invmaster.Qty, brnchprdt.Rank,productsdata.sno AS productid FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutes ON dispatch_sub.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT IndentNo, Branch_id, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (IndentType IN ('Puff1'))) indent ON modifiedroutesubtable.BranchID = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno INNER JOIN (SELECT branch_sno, product_sno, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno WHERE (dispatch.sno IN ('14')) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (dispatch.sno IN ('14')) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY modifiedroutes.RouteName, products_category.Categoryname, productsdata.sno ORDER BY brnchprdt.Rank");
                if (Session["salestype"].ToString() == "Plant")
                {
                    cmd.Parameters.AddWithValue("@BranchID", Session["BranchID"]);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                }
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate));
                DataTable dtPUFF1 = vdm.SelectQuery(cmd).Tables[0];


                cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, ROUND(SUM(indents_subtable.unitQty), 2) AS unitQty, productsdata.ProductName, productsdata.Units, products_category.Categoryname, invmaster.Qty, brnchprdt.Rank,productsdata.sno AS productid FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutes ON dispatch_sub.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT IndentNo, Branch_id, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (IndentType IN ('Puff2'))) indent ON modifiedroutesubtable.BranchID = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno INNER JOIN (SELECT branch_sno, product_sno, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno WHERE (dispatch.sno IN ('18')) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (dispatch.sno IN ('18')) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY modifiedroutes.RouteName, products_category.Categoryname, productsdata.sno ORDER BY brnchprdt.Rank");
                if (Session["salestype"].ToString() == "Plant")
                {
                    cmd.Parameters.AddWithValue("@BranchID", Session["BranchID"]);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                }
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate));
                DataTable dTPUFF2 = vdm.SelectQuery(cmd).Tables[0];


                cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, ROUND(SUM(indents_subtable.unitQty), 2) AS unitQty, productsdata.ProductName, productsdata.Units, products_category.Categoryname, invmaster.Qty, brnchprdt.Rank,productsdata.sno AS productid FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutes ON dispatch_sub.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT IndentNo, Branch_id, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (IndentType IN ('Puff3'))) indent ON modifiedroutesubtable.BranchID = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno INNER JOIN (SELECT branch_sno, product_sno, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno WHERE (dispatch.sno IN ('31')) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (dispatch.sno IN ('31')) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY modifiedroutes.RouteName, products_category.Categoryname, productsdata.sno ORDER BY brnchprdt.Rank");
                if (Session["salestype"].ToString() == "Plant")
                {
                    cmd.Parameters.AddWithValue("@BranchID", Session["BranchID"]);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                }
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate));
                DataTable dTPUFF3 = vdm.SelectQuery(cmd).Tables[0];

                 dtPUFF1.Merge(dTPUFF2);
                 dtPUFF1.Merge(dTPUFF3);
                 dtble = dtPUFF1;
            }
            else
            {
                cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, ROUND(SUM(indents_subtable.unitQty), 2) AS unitQty, productsdata.ProductName, productsdata.Units, products_category.Categoryname, invmaster.Qty, brnchprdt.Rank,productsdata.sno AS productid FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutes ON dispatch_sub.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT IndentNo, Branch_id, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (IndentType = @itype)) indent ON modifiedroutesubtable.BranchID = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno INNER JOIN (SELECT branch_sno, product_sno, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno WHERE (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY modifiedroutes.RouteName, products_category.Categoryname, productsdata.sno ORDER BY brnchprdt.Rank");
                if (Session["salestype"].ToString() == "Plant")
                {
                    cmd.Parameters.AddWithValue("@BranchID", Session["BranchID"]);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                }
                cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
                cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate));
                cmd.Parameters.AddWithValue("@itype", routeitype);
                dtble = vdm.SelectQuery(cmd).Tables[0];
            }
            

            cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, ROUND(SUM(offer_indents_sub.offer_indent_qty), 2) AS unitQty, productsdata.ProductName, productsdata.Units, products_category.Categoryname, brnchprdt.Rank, invmaster.Qty,productsdata.sno AS productid FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutes ON dispatch_sub.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT idoffer_indents, idoffers_assignment, salesoffice_id, route_id, agent_id, indent_date, indents_id, IndentType, I_modified_by FROM offer_indents WHERE (indent_date BETWEEN @starttime AND @endtime) AND (IndentType = @itype)) offerindents ON modifiedroutesubtable.BranchID = offerindents.agent_id INNER JOIN offer_indents_sub ON offerindents.idoffer_indents = offer_indents_sub.idoffer_indents INNER JOIN productsdata ON offer_indents_sub.product_id = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN (SELECT branch_sno, product_sno, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno WHERE (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY modifiedroutes.RouteName, productsdata.sno ORDER BY brnchprdt.Rank");
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["BranchID"]);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            }
            cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@itype", routeitype);
            DataTable dt_offertble = vdm.SelectQuery(cmd).Tables[0];



            if (dtble.Rows.Count > 0)
            {
                DataView view = new DataView(dtble);
                DataView viewOffers = new DataView(dt_offertble);
                DataTable produtstbl = view.ToTable(true, "productid", "ProductName", "Categoryname", "Units", "Qty", "Rank");
                DataTable Offersprodutstbl = viewOffers.ToTable(true, "productid", "ProductName", "Categoryname", "Units", "Qty", "Rank");
                foreach (DataRow drprdt in Offersprodutstbl.Rows)
                {
                    DataRow[] data_exist = produtstbl.Select("productid='" + drprdt["productid"].ToString() + "'");
                    if (data_exist.Length > 0)
                    {

                    }
                    else
                    {
                        DataRow newrow = produtstbl.NewRow();
                        newrow["productid"] = drprdt["productid"].ToString();
                        newrow["ProductName"] = drprdt["ProductName"].ToString();
                        newrow["Categoryname"] = drprdt["Categoryname"].ToString();
                        newrow["Units"] = drprdt["Units"].ToString();
                        newrow["Qty"] = drprdt["Qty"].ToString();
                        newrow["Rank"] = drprdt["Rank"].ToString();
                        produtstbl.Rows.Add(newrow);
                    }
                }
                Report = new DataTable();
                TotReport = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("Route Name");
                TotReport.Columns.Add("ProductName");
                TotReport.Columns.Add("TotalQty");
                TotReport.Columns.Add("TotCrates");
                TotReport.Columns.Add("TotExtraLtrs");
                TotReport.Columns.Add("TotalCans");
                int count = 0;
                produtstbl.DefaultView.Sort = "Rank ASC";

                foreach (DataRow dr in produtstbl.Rows)
                {
                    if (dr["Categoryname"].ToString() == "MILK")
                    {
                        Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                        DataRow newrowTot = TotReport.NewRow();
                        newrowTot["TotalQty"] = "0";
                        newrowTot["TotCrates"] = "0";
                        newrowTot["TotExtraLtrs"] = "0";
                        newrowTot["TotalCans"] = "0";
                        newrowTot["ProductName"] = dr["ProductName"].ToString();
                        TotReport.Rows.Add(newrowTot);
                        count++;
                    }
                    else
                    {
                        Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                        DataRow newrowTot = TotReport.NewRow();
                        newrowTot["TotalQty"] = "0";
                        newrowTot["TotCrates"] = "0";
                        newrowTot["TotExtraLtrs"] = "0";
                        newrowTot["TotalCans"] = "0";
                        newrowTot["ProductName"] = dr["ProductName"].ToString();
                        TotReport.Rows.Add(newrowTot);
                    }
                }
                Report.Columns.Add("Total Indent", typeof(Double)).SetOrdinal(count + 2);
                Report.Columns.Add("Total MILK CURD AND BM", typeof(Double));
                DataTable distincttable = view.ToTable(true, "RouteName");
                int i = 1;
                int status = 0;
                double finaltotmilk = 0;
                double finaltotmilkcurdBM = 0;
                foreach (DataRow branch in distincttable.Rows)
                {
                    if (branch["RouteName"].ToString() == "Thiruthani")
                    {
                        status = 1;
                    }
                    else
                    {
                        DataRow newrow = Report.NewRow();
                        newrow["SNo"] = i;
                        newrow["Route Name"] = branch["RouteName"].ToString();
                        double total = 0;
                        double totalcurd = 0;
                        double totalbuttermilk = 0;
                        //foreach (DataRow dr in dtble.Rows)
                        foreach (DataRow dr in produtstbl.Rows)
                        {
                            //if (branch["RouteName"].ToString() == dr["RouteName"].ToString())
                            //{
                            double qtyvalue = 0;
                            double curdqtyvalue = 0;
                            double BMqtyvalue = 0;
                            double allQTY = 0;
                            double offerqty = 0;
                            double totofferandind = 0;
                            foreach (DataRow drindt in dtble.Select("ProductName='" + dr["ProductName"].ToString() + "' AND RouteName='" + branch["RouteName"].ToString() + "'"))
                            {
                                double.TryParse(drindt["unitQty"].ToString(), out allQTY);
                                if (dr["Categoryname"].ToString() == "MILK")
                                {
                                    double.TryParse(drindt["unitQty"].ToString(), out qtyvalue);
                                    total += qtyvalue;
                                }
                                if (dr["Categoryname"].ToString() == "CURD" || dr["Categoryname"].ToString() == "Curd Cups" || dr["Categoryname"].ToString() == "Curd Buckets")
                                {
                                    double.TryParse(drindt["unitQty"].ToString(), out curdqtyvalue);
                                    totalcurd += curdqtyvalue;
                                }
                                if (dr["Categoryname"].ToString() == "ButterMilk")
                                {
                                    double.TryParse(drindt["unitQty"].ToString(), out BMqtyvalue);
                                    totalbuttermilk += BMqtyvalue;
                                }
                            }
                            foreach (DataRow droffer in dt_offertble.Select("ProductName='" + dr["ProductName"].ToString() + "' AND RouteName='" + branch["RouteName"].ToString() + "'"))
                            {
                                double.TryParse(droffer["unitQty"].ToString(), out offerqty);
                                if (dr["Categoryname"].ToString() == "MILK")
                                {
                                    double.TryParse(droffer["unitQty"].ToString(), out qtyvalue);
                                    total += qtyvalue;
                                }
                                if (dr["Categoryname"].ToString() == "CURD" || dr["Categoryname"].ToString() == "Curd Cups" || dr["Categoryname"].ToString() == "Curd Buckets")
                                {
                                    double.TryParse(droffer["unitQty"].ToString(), out curdqtyvalue);
                                    totalcurd += curdqtyvalue;
                                }
                                if (dr["Categoryname"].ToString() == "ButterMilk")
                                {
                                    double.TryParse(droffer["unitQty"].ToString(), out BMqtyvalue);
                                    totalbuttermilk += BMqtyvalue;
                                }

                            }
                            totofferandind = allQTY + offerqty;
                            //newrow[dr["ProductName"].ToString()] = dr["unitQty"].ToString();
                            if (totofferandind == 0)
                            {

                            }
                            else
                            {
                                newrow[dr["ProductName"].ToString()] = totofferandind;
                            }

                            foreach (DataRow drdttotal in TotReport.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
                            {
                                double totind = 0;
                                double finaltotind = 0;
                                double.TryParse(drdttotal["TotalQty"].ToString(), out totind);
                                //finaltotind = totind + allQTY;
                                finaltotind = totind + totofferandind;
                                drdttotal["TotalQty"] = finaltotind.ToString();
                            }
                            //}
                        }
                        newrow["Total Indent"] = total;
                        finaltotmilk += total;
                        newrow["Total MILK CURD AND BM"] = total + totalcurd + totalbuttermilk;
                        finaltotmilkcurdBM += total + totalcurd + totalbuttermilk;
                        Report.Rows.Add(newrow);
                        i++;
                    }
                }
                DataRow newvartical = Report.NewRow();
                newvartical["Route Name"] = "Total";
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
                Report.Rows.Add(newvartical);
                DataRow Break = Report.NewRow();
                Break["Route Name"] = "";
                Report.Rows.Add(Break);
                DataRow newInventory = Report.NewRow();
                newInventory["Route Name"] = "CRATES";
                double TotCreatsQty = 0;
                double TotCansQty = 0;
                foreach (DataRow dr in produtstbl.Rows)
                {
                    var lastRow = Report.Rows[Report.Rows.Count - 2][dr["ProductName"].ToString()];
                    int First = 0;
                    int.TryParse(lastRow.ToString(), out First);
                    double test = 0;
                    double.TryParse(lastRow.ToString(), out test);
                    int Qty = 0;
                    int.TryParse(dr["Qty"].ToString(), out Qty);
                    double InvQty = 0;
                    string prd = dr["ProductName"].ToString();
                    if (dr["ProductName"].ToString() == "CURD175" || dr["ProductName"].ToString() == "CURD175(P)")
                    {
                        double qt = 10.5;
                        Qty = (int)qt;
                        InvQty = First / Qty;

                        if (First == 0)
                        {
                            double test1 = 0;
                            test1 = test / 10.5;
                            InvQty = (int)test1;
                        }
                    }
                    if (dr["ProductName"].ToString() == "CURD-450ml")
                    {
                        double qt = 10.8;
                        Qty = (int)qt;
                        InvQty = First / Qty;

                        if (First == 0)
                        {
                            double test1 = 0;
                            test1 = test / 10.8;
                            InvQty = (int)test1;
                        }
                    }
                    if (dr["ProductName"].ToString() == "CURD 10 MRP")
                    {

                        double qt = 10.8;
                        InvQty = First / qt;
                        InvQty = Math.Round(InvQty);

                        if (First == 0)
                        {
                            double test1 = 0;
                            test1 = test / 10.8;
                            test1 = Math.Round(test1);
                            InvQty = (int)test1;
                        }
                    }
                    if (dr["ProductName"].ToString() != "CURD175" || dr["ProductName"].ToString() != "CURD 10 MRP" || dr["ProductName"].ToString() != "CURD175(P)" || dr["ProductName"].ToString() != "CURD-450ml")
                    {
                        InvQty = First / Qty;
                    }
                    int result = 0;
                    double quotient = 0;
                    if (First != 0)
                    {
                        if (dr["ProductName"].ToString() != "CURD175" || dr["ProductName"].ToString() != "CURD 10 MRP" || dr["ProductName"].ToString() != "CURD175(P)" || dr["ProductName"].ToString() != "CURD-450ml")
                        {
                            quotient = Math.DivRem(First, Qty, out result);
                        }
                        else
                        {
                            if (dr["ProductName"].ToString() == "CURD 10 MRP")
                            {
                                quotient = Math.Floor(InvQty);
                            }
                            else
                            {
                                quotient = Math.DivRem(First, Qty, out result);
                                quotient = quotient - 1;
                            }
                        }
                    }
                    if (First == 0)
                    {
                        if (dr["ProductName"].ToString() == "CURD 10 MRP" || dr["ProductName"].ToString() == "CURD-450ml")
                        {
                            double qt = 10.8;
                            InvQty = test / qt;
                            InvQty = Math.Round(InvQty);
                            quotient = Math.Round(InvQty);
                        }
                        else
                        {
                            quotient = Math.DivRem((int)test, Qty, out result);
                            if (quotient > 19)
                            {
                                quotient = quotient - 1;
                            }
                        }
                    }
                    if (dr["Categoryname"].ToString() == "MILK")
                    {
                        if (dr["Qty"].ToString() != "12")
                        {
                        }
                        else
                        {
                            TotCreatsQty += quotient;
                            newInventory[dr["ProductName"].ToString()] = quotient;
                            foreach (DataRow drdttotal in TotReport.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
                            {
                                double totind = 0;
                                double finaltotind = 0;
                                double.TryParse(drdttotal["TotCrates"].ToString(), out totind);
                                finaltotind = totind + quotient;
                                drdttotal["TotCrates"] = finaltotind.ToString();
                            }
                            newInventory["TOTAL INDENT"] = TotCreatsQty;
                        }
                    }
                    else
                    {
                        if (dr["Qty"].ToString() != "12")
                        {
                        }
                        else
                        {
                            if (dr["Units"].ToString() == "gms")
                            {
                            }
                            else
                            {
                                newInventory[dr["ProductName"].ToString()] = quotient;
                                foreach (DataRow drdttotal in TotReport.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
                                {
                                    double totind = 0;
                                    double finaltotind = 0;
                                    double.TryParse(drdttotal["TotCrates"].ToString(), out totind);
                                    finaltotind = totind + quotient;
                                    drdttotal["TotCrates"] = finaltotind.ToString();
                                }
                            }
                        }
                    }
                }
                Report.Rows.Add(newInventory);



                DataRow newExtraLtrs = Report.NewRow();
                newExtraLtrs["Route Name"] = "Extra Ltrs";
                double totextraltrs = 0;
                foreach (DataRow dr in produtstbl.Rows)
                {
                    var lastRow = Report.Rows[Report.Rows.Count - 3][dr["ProductName"].ToString()];
                    int First = 0;
                    int.TryParse(lastRow.ToString(), out First);
                    int Qty = 0;
                    int.TryParse(dr["Qty"].ToString(), out Qty);
                    int InvQty = 0;
                    InvQty = First / Qty;
                    int result = 0;
                    float quotient = 0;
                    if (dr["ProductName"].ToString() != "CURD175")
                    {
                        quotient = Math.DivRem(First, Qty, out result);
                    }
                    if (dr["Categoryname"].ToString() == "MILK")
                    {
                        if (dr["Qty"].ToString() != "12")
                        {
                        }
                        else
                        {
                            totextraltrs += result;
                            newExtraLtrs[dr["ProductName"].ToString()] = result;
                            newExtraLtrs["TOTAL INDENT"] = totextraltrs;
                            foreach (DataRow drdttotal in TotReport.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
                            {
                                double totind = 0;
                                double finaltotind = 0;
                                double.TryParse(drdttotal["TotExtraLtrs"].ToString(), out totind);
                                finaltotind = totind + result;
                                drdttotal["TotExtraLtrs"] = finaltotind.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (dr["Qty"].ToString() != "12")
                        {
                        }
                        else
                        {
                            if (dr["Units"].ToString() == "gms")
                            {
                            }
                            else
                            {
                                newExtraLtrs[dr["ProductName"].ToString()] = result;
                                foreach (DataRow drdttotal in TotReport.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
                                {
                                    double totind = 0;
                                    double finaltotind = 0;
                                    double.TryParse(drdttotal["TotExtraLtrs"].ToString(), out totind);
                                    finaltotind = totind + result;
                                    drdttotal["TotExtraLtrs"] = finaltotind.ToString();
                                }
                            }
                        }
                    }
                }
                Report.Rows.Add(newExtraLtrs);




                DataRow newInventoryCans = Report.NewRow();
                newInventoryCans["Route Name"] = "CANS";
                foreach (DataRow dr in produtstbl.Rows)
                {
                    var lastRow = Report.Rows[Report.Rows.Count - 4][dr["ProductName"].ToString()];
                    float First = 0;
                    float.TryParse(lastRow.ToString(), out First);
                    float Qty = 0;
                    float.TryParse(dr["Qty"].ToString(), out Qty);
                    double InvQty = 0;
                    InvQty = Math.Round(First / Qty);
                    //InvQty=  Math.Round(InvQty,2);
                    //InvQty = Math.Ceiling(InvQty);
                    if (dr["Categoryname"].ToString() == "MILK")
                    {
                        if (dr["Qty"].ToString() != "12")
                        {
                            TotCansQty += InvQty;
                            newInventoryCans[dr["ProductName"].ToString()] = InvQty;
                            newInventoryCans["TOTAL INDENT"] = TotCansQty;
                            foreach (DataRow drdttotal in TotReport.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
                            {
                                double totind = 0;
                                double finaltotind = 0;
                                double.TryParse(drdttotal["TotalCans"].ToString(), out totind);
                                finaltotind = totind + InvQty;
                                drdttotal["TotalCans"] = finaltotind.ToString();
                            }
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        if (dr["Qty"].ToString() != "12")
                        {
                            TotCansQty += InvQty;
                            newInventoryCans[dr["ProductName"].ToString()] = InvQty;
                            foreach (DataRow drdttotal in TotReport.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
                            {
                                double totind = 0;
                                double finaltotind = 0;
                                double.TryParse(drdttotal["TotalCans"].ToString(), out totind);
                                finaltotind = totind + InvQty;
                                drdttotal["TotalCans"] = finaltotind.ToString();
                            }
                        }
                        else
                        {
                        }
                    }
                }
                Report.Rows.Add(newInventoryCans);



                DataRow newInventoryscheem = Report.NewRow();
                newInventoryscheem["Route Name"] = "Scheem";
                foreach (DataRow dr in produtstbl.Rows)
                {
                    var lastRow = Report.Rows[Report.Rows.Count - 5][dr["ProductName"].ToString()];
                    double itemwisetotal = 0;
                    string itemname = dr["ProductName"].ToString();
                    foreach (DataRow droffer in dt_offertble.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
                    {
                        double qty = 0;
                        double.TryParse(droffer["unitQty"].ToString(), out qty);
                        itemwisetotal += qty;
                    }
                    newInventoryscheem[dr["ProductName"].ToString()] = itemwisetotal;
                }
                Report.Rows.Add(newInventoryscheem);
                double TotCreatsQty1 = 0;
                double TotCansQty1 = 0;
                if (status == 1)
                {

                    foreach (DataRow branch in distincttable.Rows)
                    {
                        if (branch["RouteName"].ToString() != "Thiruthani")
                        {
                            //status = 1;
                        }
                        else
                        {
                            DataRow Break1 = Report.NewRow();
                            Break1["Route Name"] = "";
                            Report.Rows.Add(Break1);
                            DataRow Break2 = Report.NewRow();
                            Break2["Route Name"] = "";
                            Report.Rows.Add(Break2);
                            DataRow newrow = Report.NewRow();
                            newrow["SNo"] = i;
                            newrow["Route Name"] = branch["RouteName"].ToString();
                            double total = 0;
                            double totalcurd = 0;
                            double totalbuttermilk = 0;
                            foreach (DataRow dr in dtble.Rows)
                            {
                                if (branch["RouteName"].ToString() == dr["RouteName"].ToString())
                                {
                                    double qtyvalue = 0;
                                    double curdqtyvalue = 0;
                                    double BMqtyvalue = 0;
                                    double allQTY = 0;
                                    double.TryParse(dr["unitQty"].ToString(), out allQTY);
                                    newrow[dr["ProductName"].ToString()] = dr["unitQty"].ToString();
                                    if (dr["Categoryname"].ToString() == "MILK")
                                    {
                                        double.TryParse(dr["unitQty"].ToString(), out qtyvalue);
                                        total += qtyvalue;
                                    }

                                    if (dr["Categoryname"].ToString() == "CURD")
                                    {
                                        double.TryParse(dr["unitQty"].ToString(), out curdqtyvalue);
                                        totalcurd += curdqtyvalue;
                                    }
                                    if (dr["Categoryname"].ToString() == "ButterMilk")
                                    {
                                        double.TryParse(dr["unitQty"].ToString(), out BMqtyvalue);
                                        totalbuttermilk += BMqtyvalue;
                                    }
                                    foreach (DataRow drdttotal in TotReport.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
                                    {
                                        double totind = 0;
                                        double finaltotind = 0;
                                        double.TryParse(drdttotal["TotalQty"].ToString(), out totind);
                                        finaltotind = totind + allQTY;
                                        drdttotal["TotalQty"] = finaltotind.ToString();
                                    }
                                }
                            }
                            newrow["Total Indent"] = total;
                            finaltotmilk += total;
                            newrow["Total MILK CURD AND BM"] = total + totalcurd + totalbuttermilk;
                            finaltotmilkcurdBM += total + totalcurd + totalbuttermilk;
                            Report.Rows.Add(newrow);
                            i++;
                        }
                    }
                    DataRow Break3 = Report.NewRow();
                    Break3["Route Name"] = "";
                    Report.Rows.Add(Break3);
                    DataRow newInventory1 = Report.NewRow();
                    newInventory1["Route Name"] = "CRATES";
                    foreach (DataRow dr in produtstbl.Rows)
                    {
                        var lastRow = Report.Rows[Report.Rows.Count - 2][dr["ProductName"].ToString()];
                        int First = 0;
                        int.TryParse(lastRow.ToString(), out First);
                        int Qty = 0;
                        int.TryParse(dr["Qty"].ToString(), out Qty);
                        int InvQty = 0;
                        InvQty = First / Qty;
                        int result = 0;
                        float quotient = Math.DivRem(First, Qty, out result);
                        if (dr["Categoryname"].ToString() == "MILK")
                        {
                            if (dr["Qty"].ToString() != "12")
                            {
                            }
                            else
                            {
                                TotCreatsQty1 += quotient;
                                newInventory1[dr["ProductName"].ToString()] = quotient;
                                newInventory1["TOTAL INDENT"] = TotCreatsQty1;
                                foreach (DataRow drdttotal in TotReport.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
                                {
                                    double totind = 0;
                                    double finaltotind = 0;
                                    double.TryParse(drdttotal["TotCrates"].ToString(), out totind);
                                    finaltotind = totind + quotient;
                                    drdttotal["TotCrates"] = finaltotind.ToString();
                                }
                            }
                        }
                        else
                        {
                            if (dr["Qty"].ToString() != "12")
                            {
                            }
                            else
                            {
                                if (dr["Units"].ToString() == "gms")
                                {
                                }
                                else
                                {
                                    newInventory1[dr["ProductName"].ToString()] = quotient;
                                    foreach (DataRow drdttotal in TotReport.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
                                    {
                                        double totind = 0;
                                        double finaltotind = 0;
                                        double.TryParse(drdttotal["TotCrates"].ToString(), out totind);
                                        finaltotind = totind + quotient;
                                        drdttotal["TotCrates"] = finaltotind.ToString();
                                    }
                                }
                            }
                        }
                    }
                    Report.Rows.Add(newInventory1);
                    DataRow newExtraLtrs1 = Report.NewRow();
                    newExtraLtrs1["Route Name"] = "Extra Ltrs";
                    double totextraltrs1 = 0;
                    foreach (DataRow dr in produtstbl.Rows)
                    {
                        var lastRow = Report.Rows[Report.Rows.Count - 3][dr["ProductName"].ToString()];
                        int First = 0;
                        int.TryParse(lastRow.ToString(), out First);
                        int Qty = 0;
                        int.TryParse(dr["Qty"].ToString(), out Qty);
                        int InvQty = 0;
                        InvQty = First / Qty;
                        int result = 0;
                        float quotient = Math.DivRem(First, Qty, out result);
                        if (dr["Categoryname"].ToString() == "MILK")
                        {
                            if (dr["Qty"].ToString() != "12")
                            {
                            }
                            else
                            {
                                totextraltrs1 += result;
                                newExtraLtrs1[dr["ProductName"].ToString()] = result;
                                newExtraLtrs1["TOTAL INDENT"] = totextraltrs1;
                                foreach (DataRow drdttotal in TotReport.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
                                {
                                    double totind = 0;
                                    double finaltotind = 0;
                                    double.TryParse(drdttotal["TotExtraLtrs"].ToString(), out totind);
                                    finaltotind = totind + result;
                                    drdttotal["TotExtraLtrs"] = finaltotind.ToString();
                                }
                            }
                        }
                        else
                        {
                            if (dr["Qty"].ToString() != "12")
                            {
                            }
                            else
                            {
                                if (dr["Units"].ToString() == "gms")
                                {
                                }
                                else
                                {
                                    newExtraLtrs1[dr["ProductName"].ToString()] = result;
                                    foreach (DataRow drdttotal in TotReport.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
                                    {
                                        double totind = 0;
                                        double finaltotind = 0;
                                        double.TryParse(drdttotal["TotExtraLtrs"].ToString(), out totind);
                                        finaltotind = totind + result;
                                        drdttotal["TotExtraLtrs"] = finaltotind.ToString();
                                    }
                                }
                            }
                        }
                    }
                    Report.Rows.Add(newExtraLtrs1);
                    DataRow newInventoryCans1 = Report.NewRow();
                    newInventoryCans1["Route Name"] = "CANS";
                    foreach (DataRow dr in produtstbl.Rows)
                    {
                        var lastRow = Report.Rows[Report.Rows.Count - 4][dr["ProductName"].ToString()];
                        float First = 0;
                        float.TryParse(lastRow.ToString(), out First);
                        float Qty = 0;
                        float.TryParse(dr["Qty"].ToString(), out Qty);
                        double InvQty = 0;
                        InvQty = Math.Round(First / Qty);
                        if (dr["Categoryname"].ToString() == "MILK")
                        {
                            if (dr["Qty"].ToString() != "12")
                            {
                                TotCansQty1 += InvQty;
                                newInventoryCans1[dr["ProductName"].ToString()] = InvQty;
                                newInventoryCans1["TOTAL INDENT"] = TotCansQty1;
                                foreach (DataRow drdttotal in TotReport.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
                                {
                                    double totind = 0;
                                    double finaltotind = 0;
                                    double.TryParse(drdttotal["TotalCans"].ToString(), out totind);
                                    finaltotind = totind + InvQty;
                                    drdttotal["TotalCans"] = finaltotind.ToString();
                                }
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            if (dr["Qty"].ToString() != "12")
                            {
                                TotCansQty1 += InvQty;
                                newInventoryCans1[dr["ProductName"].ToString()] = InvQty;
                                foreach (DataRow drdttotal in TotReport.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
                                {
                                    double totind = 0;
                                    double finaltotind = 0;
                                    double.TryParse(drdttotal["TotalCans"].ToString(), out totind);
                                    finaltotind = totind + InvQty;
                                    drdttotal["TotalCans"] = finaltotind.ToString();
                                }
                            }
                            else
                            {
                            }
                        }
                    }
                    Report.Rows.Add(newInventoryCans1);
                }
                if (status == 1)
                {
                    DataRow Break5 = Report.NewRow();
                    Break5["Route Name"] = "";
                    Report.Rows.Add(Break5);
                    DataRow Break6 = Report.NewRow();
                    Break6["Route Name"] = "";
                    Report.Rows.Add(Break6);
                    DataRow Break7 = Report.NewRow();
                    Break7["Route Name"] = "";
                    Report.Rows.Add(Break7);
                    DataRow newrowtot = Report.NewRow();
                    newrowtot["SNo"] = i;
                    newrowtot["Route Name"] = "TOTAL";

                    foreach (DataRow dr in produtstbl.Rows)
                    {
                        foreach (DataRow drtot in TotReport.Rows)
                        {
                            if (dr["ProductName"].ToString() == drtot["ProductName"].ToString())
                            {
                                newrowtot[dr["ProductName"].ToString()] = drtot["TotalQty"].ToString();
                            }
                            newrowtot["Total Indent"] = finaltotmilk;
                            newrowtot["Total MILK CURD AND BM"] = finaltotmilkcurdBM;
                        }
                    }
                    Report.Rows.Add(newrowtot);
                    DataRow newrowCrates = Report.NewRow();
                    newrowCrates["Route Name"] = "CRATES";
                    foreach (DataRow dr in produtstbl.Rows)
                    {
                        foreach (DataRow drtot in TotReport.Rows)
                        {
                            if (dr["ProductName"].ToString() == drtot["ProductName"].ToString())
                            {
                                newrowCrates[dr["ProductName"].ToString()] = drtot["TotCrates"].ToString();
                            }
                            newrowCrates["Total Indent"] = TotCreatsQty + TotCreatsQty1;
                        }
                    }
                    Report.Rows.Add(newrowCrates);
                    DataRow newrowextra = Report.NewRow();
                    newrowextra["Route Name"] = "EXTRA LTRS";

                    foreach (DataRow dr in produtstbl.Rows)
                    {
                        foreach (DataRow drtot in TotReport.Rows)
                        {
                            if (dr["ProductName"].ToString() == drtot["ProductName"].ToString())
                            {
                                newrowextra[dr["ProductName"].ToString()] = drtot["TotExtraLtrs"].ToString();
                            }
                        }
                    }
                    Report.Rows.Add(newrowextra);
                    DataRow newrowCans = Report.NewRow();
                    newrowCans["Route Name"] = "CANS";
                    foreach (DataRow dr in produtstbl.Rows)
                    {
                        foreach (DataRow drtot in TotReport.Rows)
                        {
                            if (dr["ProductName"].ToString() == drtot["ProductName"].ToString())
                            {
                                newrowCans[dr["ProductName"].ToString()] = drtot["TotalCans"].ToString();
                            }
                            newrowCans["Total Indent"] = TotCansQty + TotCansQty1;
                        }
                    }
                    Report.Rows.Add(newrowCans);
                }
                foreach (DataColumn col in Report.Columns)
                {
                    string Pname = col.ToString();
                    string ProductName = col.ToString();
                    ProductName = GetSpace(ProductName);
                    Report.Columns[Pname].ColumnName = ProductName;
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
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
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
        if (Session["salestype"].ToString() == "Plant")
        {
            vdm = new VehicleDBMgr();
            cmd = new MySqlCommand("SELECT sno, DispName, BranchID, Dispdate, DispMode FROM dispatch WHERE (sno = @dispSno)");
            cmd.Parameters.AddWithValue("@dispSno", ddlRouteName.SelectedValue);
            DataTable dtBranch = vdm.SelectQuery(cmd).Tables[0];
            if (dtBranch.Rows.Count > 0)
            {
                Session["BranchID"] = dtBranch.Rows[0]["BranchID"].ToString();
            }
        }
        else
        {
        }
    }
}