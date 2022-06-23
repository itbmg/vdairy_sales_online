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

public partial class testoffer : System.Web.UI.Page
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
        GetofferReport();
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
    void GetofferReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            //DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            lblDispatchName.Text = ddlRouteName.SelectedItem.Text;
            vdm = new VehicleDBMgr();
            DataTable OfferReport = new DataTable();
            DataTable OfferTotReport = new DataTable();
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
            cmd = new MySqlCommand("select Route_id,IndentType from dispatch_sub where dispatch_sno=@dispsno");
            cmd.Parameters.AddWithValue("@dispsno", ddlRouteName.SelectedValue);
            DataTable dtrouteindenttype = vdm.SelectQuery(cmd).Tables[0];
            var routeitype = "";
            foreach (DataRow drrouteitype in dtrouteindenttype.Rows)
            {
                var routeid = drrouteitype["Route_id"].ToString();
                routeitype = drrouteitype["IndentType"].ToString();
            }
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
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];

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
                OfferReport = new DataTable();
                OfferTotReport = new DataTable();
                OfferReport.Columns.Add("SNo");
                OfferReport.Columns.Add("RouteName");
                OfferTotReport.Columns.Add("ProductName");
                OfferTotReport.Columns.Add("TotalQty");
                OfferTotReport.Columns.Add("TotCrates");
                OfferTotReport.Columns.Add("TotExtraLtrs");
                OfferTotReport.Columns.Add("TotalCans");
                int count = 0;
                produtstbl.DefaultView.Sort = "Rank ASC";

                foreach (DataRow dr in produtstbl.Rows)
                {
                    if (dr["Categoryname"].ToString() == "MILK")
                    {
                        OfferReport.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                        DataRow newrowTot = OfferTotReport.NewRow();
                        newrowTot["TotalQty"] = "0";
                        newrowTot["TotCrates"] = "0";
                        newrowTot["TotExtraLtrs"] = "0";
                        newrowTot["TotalCans"] = "0";
                        newrowTot["ProductName"] = dr["ProductName"].ToString();
                        OfferTotReport.Rows.Add(newrowTot);
                        count++;
                    }
                    else
                    {
                        OfferReport.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                        DataRow newrowTot = OfferTotReport.NewRow();
                        newrowTot["TotalQty"] = "0";
                        newrowTot["TotCrates"] = "0";
                        newrowTot["TotExtraLtrs"] = "0";
                        newrowTot["TotalCans"] = "0";
                        newrowTot["ProductName"] = dr["ProductName"].ToString();
                        OfferTotReport.Rows.Add(newrowTot);
                    }
                }
                OfferReport.Columns.Add("Total Indent", typeof(Double)).SetOrdinal(count + 2);
                OfferReport.Columns.Add("Total MILK CURD AND BM", typeof(Double));
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
                        DataRow newrow = OfferReport.NewRow();
                        newrow["SNo"] = i;
                        newrow["RouteName"] = branch["RouteName"].ToString();
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

                            foreach (DataRow droffer in dt_offertble.Select("ProductName='" + dr["ProductName"].ToString() + "' AND RouteName='" + branch["RouteName"].ToString() + "'"))
                            {
                                double.TryParse(droffer["unitQty"].ToString(), out offerqty);
                                if (dr["Categoryname"].ToString() == "MILK")
                                {
                                    double.TryParse(droffer["unitQty"].ToString(), out qtyvalue);
                                    total += qtyvalue;
                                }
                                if (dr["Categoryname"].ToString() == "CURD")
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

                            foreach (DataRow drdttotal in OfferTotReport.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
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
                        OfferReport.Rows.Add(newrow);
                        i++;
                    }
                }
                DataRow newvartical = OfferReport.NewRow();
                newvartical["RouteName"] = "Scheme";
                double val = 0.0;
                foreach (DataColumn dc in OfferReport.Columns)
                {
                    if (dc.DataType == typeof(Double))
                    {
                        val = 0.0;
                        double.TryParse(OfferReport.Compute("sum([" + dc.ToString() + "])", "[" + dc.ToString() + "]<>'0'").ToString(), out val);
                        newvartical[dc.ToString()] = val;
                    }
                }
                OfferReport.Rows.Add(newvartical);

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
                            DataRow Break1 = OfferReport.NewRow();
                            Break1["RouteName"] = "";
                            OfferReport.Rows.Add(Break1);
                            DataRow Break2 = OfferReport.NewRow();
                            Break2["RouteName"] = "";
                            OfferReport.Rows.Add(Break2);
                            DataRow newrow = OfferReport.NewRow();
                            newrow["SNo"] = i;
                            newrow["RouteName"] = branch["RouteName"].ToString();
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
                                    foreach (DataRow drdttotal in OfferTotReport.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
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
                            OfferReport.Rows.Add(newrow);
                            i++;
                        }
                    }
                }
                if (status == 1)
                {
                    DataRow Break5 = OfferReport.NewRow();
                    Break5["RouteName"] = "";
                    OfferReport.Rows.Add(Break5);
                    DataRow Break6 = OfferReport.NewRow();
                    Break6["RouteName"] = "";
                    OfferReport.Rows.Add(Break6);
                    DataRow Break7 = OfferReport.NewRow();
                    Break7["RouteName"] = "";
                    OfferReport.Rows.Add(Break7);
                    DataRow newrowtot = OfferReport.NewRow();
                    newrowtot["SNo"] = i;
                    newrowtot["RouteName"] = "Scheme";

                    foreach (DataRow dr in produtstbl.Rows)
                    {
                        foreach (DataRow drtot in OfferTotReport.Rows)
                        {
                            if (dr["ProductName"].ToString() == drtot["ProductName"].ToString())
                            {
                                newrowtot[dr["ProductName"].ToString()] = drtot["TotalQty"].ToString();
                            }
                            newrowtot["Total Indent"] = finaltotmilk;
                            newrowtot["Total MILK CURD AND BM"] = finaltotmilkcurdBM;
                        }
                    }
                    OfferReport.Rows.Add(newrowtot);
                }
                foreach (DataColumn col in OfferReport.Columns)
                {
                    string Pname = col.ToString();
                    string ProductName = col.ToString();
                    ProductName = GetSpace(ProductName);
                    OfferReport.Columns[Pname].ColumnName = ProductName;
                }

                //for (int j = 0; j <= OfferReport.Rows.Count; j++)
                //{
                //    DataRow recRow = OfferReport.Rows[j];
                //    if (recRow[1].ToString() == "Scheme")
                //    {

                //    }
                //    else
                //    {
                //        if (count > 0)
                //        {
                //            recRow[1] = string.Empty;
                //            recRow.Delete();
                //            OfferReport.AcceptChanges();
                //        }
                //    }
                //}
                grdofferReports.DataSource = OfferReport;
                grdofferReports.DataBind();
                //Session["xportdata"] = OfferReport;
            }
            else
            {
                pnlHide.Visible = false;
                lblmsg.Text = "No Indent Found";
                grdofferReports.DataSource = OfferReport;
                grdofferReports.DataBind();
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