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
using System.Drawing;

public partial class AgentInformation : System.Web.UI.Page
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
        //if (!this.IsPostBack)
        //{
        //    if (!Page.IsCallback)
        //    {
        //        FillAgentName();
        //        //lblTitle.Text = Session["TitleName"].ToString();
        //        //txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
        //        //todate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
        //    }
        //}
    }
    //void FillAgentName()
    //{
    //    try
    //    {
    //        vdm = new VehicleDBMgr();
    //        if (Session["salestype"].ToString() == "Plant")
    //        {
    //            PBranch.Visible = true;
    //            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType)");
    //            cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"].ToString());
    //            cmd.Parameters.AddWithValue("@SalesType", "21");
    //            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
    //            //if (ddlSalesOffice.SelectedIndex == -1)
    //            //{
    //            //    ddlSalesOffice.SelectedItem.Text = "Select";
    //            //}
    //            ddlSalesOffice.DataSource = dtRoutedata;
    //            ddlSalesOffice.DataTextField = "BranchName";
    //            ddlSalesOffice.DataValueField = "sno";
    //            ddlSalesOffice.DataBind();
    //            ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));
    //        }
    //        else
    //        {
    //            PBranch.Visible = false;
    //            cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
    //            //cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
    //            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
    //            cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
    //            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
    //            ddlDispName.DataSource = dtRoutedata;
    //            ddlDispName.DataTextField = "DispName";
    //            ddlDispName.DataValueField = "sno";
    //            ddlDispName.DataBind();
    //            ddlDispName.Items.Insert(0, new ListItem("Select", "0"));
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblmsg.Text = ex.Message;
    //    }
    //}
    //protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    vdm = new VehicleDBMgr();
    //    //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch)");
    //    cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
    //    //cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
    //    cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
    //    cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
    //    DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
    //    ddlDispName.DataSource = dtRoutedata;
    //    ddlDispName.DataTextField = "DispName";
    //    ddlDispName.DataValueField = "sno";
    //    ddlDispName.DataBind();
    //    ddlDispName.Items.Insert(0, new ListItem("Select", "0"));
    //}
    //protected void ddlDispName_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    vdm = new VehicleDBMgr();
    //    //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch)");
    //    cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutes ON dispatch_sub.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (dispatch.sno = @dispsno)");
    //    cmd.Parameters.AddWithValue("@dispsno", ddlDispName.SelectedValue);
    //    DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
    //    ddlAgentName.DataSource = dtRoutedata;
    //    ddlAgentName.DataTextField = "BranchName";
    //    ddlAgentName.DataValueField = "sno";
    //    ddlAgentName.DataBind();
    //}
    ////void fillimages()
    ////{
    ////    try
    ////    {
    ////        vdm = new VehicleDBMgr();

    ////        cmd = new MySqlCommand("SELECT Shop_Photo, Agent_PIC FROM branchdata WHERE (sno = @agentid)");
    ////        cmd.Parameters.AddWithValue("@agentid", ddlAgentName.SelectedValue);
    ////        DataTable dtpics = vdm.SelectQuery(cmd).Tables[0];
    ////        if (dtpics.Rows.Count > 0)
    ////        {
    ////            //shopImage2.ImageUrl = "";
    ////            Image1.ImageUrl = "";
    ////            //if (dtpics.Rows[0]["Shop_Photo"] != "")
    ////            //{
    ////            //    string lengh = dtpics.Rows[0]["Shop_Photo"].ToString();
    ////            //    int count = lengh.Length;
    ////            //    if (count > 0)
    ////            //    {
    ////            //        byte[] imgbyte = (byte[])dtpics.Rows[0]["Shop_Photo"];
    ////            //        string base64code = Convert.ToBase64String(imgbyte);
    ////            //        shopImage2.ImageUrl = "data:Image/png;base64," + base64code;
    ////            //    }
    ////            //}
    ////            if (dtpics.Rows[0]["Agent_PIC"] != "")
    ////            {
    ////                string lengh = dtpics.Rows[0]["Agent_PIC"].ToString();
    ////                int count = lengh.Length;
    ////                if (count > 0)
    ////                {
    ////                    byte[] imgbyte = (byte[])dtpics.Rows[0]["Agent_PIC"];
    ////                    string base64code = Convert.ToBase64String(imgbyte);
    ////                    Image1.ImageUrl = "data:Image/png;base64," + base64code;
    ////                }

    ////            }
    ////        }
    ////        else
    ////        {
    ////            //shopImage2.ImageUrl = "";
    ////            Image1.ImageUrl = "";
    ////        }

    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        lblmsg.Text = ex.Message;
    ////    }
    ////}
    //private DateTime GetLowDate(DateTime dt)
    //{
    //    double Hour, Min, Sec;
    //    DateTime DT = DateTime.Now;
    //    DT = dt;
    //    Hour = -dt.Hour;
    //    Min = -dt.Minute;
    //    Sec = -dt.Second;
    //    DT = DT.AddHours(Hour);
    //    DT = DT.AddMinutes(Min);
    //    DT = DT.AddSeconds(Sec);
    //    return DT;

    //}

    //private DateTime GetHighDate(DateTime dt)
    //{
    //    double Hour, Min, Sec;
    //    DateTime DT = DateTime.Now;
    //    Hour = 23 - dt.Hour;
    //    Min = 59 - dt.Minute;
    //    Sec = 59 - dt.Second;
    //    DT = dt;
    //    DT = DT.AddHours(Hour);
    //    DT = DT.AddMinutes(Min);
    //    DT = DT.AddSeconds(Sec);
    //    return DT;
    //}
    //protected void btnGenerate_Click(object sender, EventArgs e)
    //{
    //    GetReport();
    //}
    //DataTable Report = new DataTable();
    //DataTable dtAgent = new DataTable();

    //void GetReport()
    //{
    //    try
    //    {
    //        lblmsg.Text = "";
    //        vdm = new VehicleDBMgr();
    //        DateTime fromdate = DateTime.Now;
    //        DateTime todate = DateTime.Now;
    //        Report = new DataTable();
    //        dtAgent = new DataTable();
    //        cmd = new MySqlCommand("SELECT  branchdata.sno, branchdata.BranchName, branchdata.Lat, branchdata.Lng, branchdata.phonenumber, branchdata.CollectionType, branchdata.Address,branchdata.DateOfEntry, branchdata.phonenumber2, branchroutes.RouteName, branchdata.duelimit, branchaccounts.Amount, branchdata.Due_Limit_Days, branchdata.Due_Limit_Type, branchdata.SalesRepresentative, freezer_issue.receiver_id, freezer_issue.totalamount, freezer_issue.installamount,freezer_issue.freezer_sno, freezer_detaills.companyname, freezer_detaills.freezer_type FROM branchdata INNER JOIN branchroutesubtable ON branchdata.sno = branchroutesubtable.BranchID INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno INNER JOIN branchaccounts ON branchdata.sno = branchaccounts.BranchId LEFT OUTER JOIN freezer_issue ON branchdata.sno = freezer_issue.receiver_id LEFT OUTER JOIN freezer_detaills ON freezer_issue.freezer_sno = freezer_detaills.sno WHERE  (branchdata.sno = @bid)");
    //        //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName,branchdata.lat,branchdata.lng, branchdata.phonenumber, branchdata.CollectionType, branchdata.Address, branchdata.DateOfEntry,branchdata.phonenumber2, branchroutes.RouteName, branchdata.duelimit, branchaccounts.Amount, branchdata.Due_Limit_Days, branchdata.Due_Limit_Type,branchdata.SalesRepresentative FROM branchdata INNER JOIN branchroutesubtable ON branchdata.sno = branchroutesubtable.BranchID INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno INNER JOIN branchaccounts ON branchdata.sno = branchaccounts.BranchId WHERE (branchdata.sno = @bid)");
    //        cmd.Parameters.AddWithValue("@bid",ddlAgentName.SelectedValue);
    //        DataTable dtagentdetails = vdm.SelectQuery(cmd).Tables[0];
    //        cmd = new MySqlCommand("SELECT BranchId, Inv_Sno, Qty, Sno, EmpId, lostQty FROM inventory_monitor WHERE (BranchId = @bid)");
    //        cmd.Parameters.AddWithValue("@bid", ddlAgentName.SelectedValue);
    //        DataTable dtagentinventory = vdm.SelectQuery(cmd).Tables[0];
    //        cmd = new MySqlCommand("SELECT productsdata.UnitPrice, branchproducts.Rank, productsdata.ProductName, productsdata.Units, productsdata.Qty, branchproducts.unitprice AS BUnitPrice,branchproducts_1.unitprice AS Aunitprice, productsdata.sno FROM branchproducts INNER JOIN branchmappingtable ON branchproducts.branch_sno = branchmappingtable.SuperBranch INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN branchproducts branchproducts_1 ON branchmappingtable.SubBranch = branchproducts_1.branch_sno AND productsdata.sno = branchproducts_1.product_sno WHERE (branchproducts_1.branch_sno = @bsno) AND (branchproducts_1.flag = @flag) GROUP BY branchproducts_1.branch_sno, branchproducts_1.unitprice, productsdata.sno, branchproducts_1.flag ORDER BY branchproducts.Rank");
    //        cmd.Parameters.AddWithValue("@flag", 1);
    //        cmd.Parameters.AddWithValue("@bsno", ddlAgentName.SelectedValue);
    //       DataTable dtBranchproducts = vdm.SelectQuery(cmd).Tables[0];
    //       cmd = new MySqlCommand("SELECT  FromDate, Todate, StructureName,  leakagepercent,TotalDiscount, Remarks FROM incentivetransactions WHERE (BranchId = @BranchID) ORDER BY sno DESC LIMIT 1");
    //       cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
    //       DataTable dtincentiveamt = vdm.SelectQuery(cmd).Tables[0];
    //       float count = 0;
    //       if (dtincentiveamt.Rows.Count > 0)
    //       {
    //           fromdate = Convert.ToDateTime(dtincentiveamt.Rows[0]["FromDate"].ToString());
    //           todate = Convert.ToDateTime(dtincentiveamt.Rows[0]["Todate"].ToString());
    //           cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost), 2) AS Totalsalevalue, ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty,products_category.Categoryname, productsdata.ProductName, productsdata.sno AS prodid  FROM productsdata INNER JOIN indents_subtable ON productsdata.sno = indents_subtable.Product_sno INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchdata.sno = @BranchID) GROUP BY productsdata.sno");
    //           cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
    //           cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
    //           cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
    //           dtAgent = vdm.SelectQuery(cmd).Tables[0];
    //           count = (float)(todate - fromdate.AddDays(-1)).TotalDays;
    //       }
    //        foreach (DataRow dr in dtagentdetails.Rows)
    //        {
    //            lblagent.Text = dr["BranchName"].ToString();
    //            lblmob.Text = dr["phonenumber"].ToString();
    //            lbladdress.Text = dr["Address"].ToString();
    //            lblagentcode.Text = dr["sno"].ToString();
    //            lbl_lat.Text = dr["lat"].ToString();
    //            lbl_long.Text = dr["lng"].ToString();
    //            Hyper_latlong.Text = dr["lat"].ToString() + "," + dr["lng"].ToString();
    //            lblroutname.Text = dr["RouteName"].ToString();
    //            lblsr.Text = dr["SalesRepresentative"].ToString();
    //            lblcompanyname.Text = dr["companyname"].ToString();
    //            lblfreezertype.Text = dr["freezer_type"].ToString();
    //            lblfreezerTamount.Text = dr["totalamount"].ToString();
    //            lblinstallamount.Text = dr["installamount"].ToString();


    //            if (dr["Due_Limit_Type"].ToString() == "")
    //            {

    //                lblduelimit.Text = "Rs  " + "0";

    //            }
    //            if (dr["Due_Limit_Type"].ToString() == "Amount")
    //            {
    //                if (dr["DueLimit"].ToString() == "")
    //                {
    //                    lblduelimit.Text = "Rs  " + "0";
    //                }
    //                if (dr["DueLimit"].ToString() != "")
    //                {
    //                    lblduelimit.Text = "Rs  " + dr["DueLimit"].ToString();
    //                }
    //            }
    //            if (dr["Due_Limit_Type"].ToString() == "Days")
    //            {
    //                if (dr["Due_Limit_Days"].ToString() == "")
    //                {
    //                    lblduelimit.Text = "0" + "  Days";
    //                }
    //                if (dr["Due_Limit_Days"].ToString() != "")
    //                {
    //                    lblduelimit.Text = dr["Due_Limit_Days"].ToString() + "  Days";
    //                }
    //            }

    //            lblcreateddate.Text = dr["DateOfEntry"].ToString();
    //            //lblbalamt.Text = "<u>'" + dr["Amount"].ToString() + "'</u>";
    //            lblbalamt.Text =  dr["Amount"].ToString();

    //        }
    //        if (dtagentinventory.Rows.Count > 0)
    //        {
    //            int crates = 0;
    //            int cans = 0;
    //            foreach (DataRow dr in dtagentinventory.Rows)
    //            {
    //                int tot = 0;
    //                if (dr["Inv_Sno"].ToString() == "1")
    //                {
    //                    int.TryParse(dr["Qty"].ToString(), out crates);
    //                }
    //                if (dr["Inv_Sno"].ToString() == "2")
    //                {
    //                    int.TryParse(dr["Qty"].ToString(), out tot);

    //                }
    //                if (dr["Inv_Sno"].ToString() == "3")
    //                {
    //                    int.TryParse(dr["Qty"].ToString(), out tot);

    //                }
    //                if (dr["Inv_Sno"].ToString() == "4")
    //                {
    //                    int.TryParse(dr["Qty"].ToString(), out tot);

    //                }
    //                if (dr["Inv_Sno"].ToString() == "5")
    //                {
    //                    int.TryParse(dr["Qty"].ToString(), out tot);

    //                }
    //                cans += tot;
    //            }
    //            lblcansdue.Text =cans.ToString();
    //            lblcratesdue.Text = crates.ToString();
    //            lblcratduedate.Text = "";
    //            lblcansduedate.Text = "";
    //        }
    //        if (dtagentinventory.Rows.Count <= 0)
    //        {
    //            lblcansdue.Text = "0";
    //            lblcratesdue.Text = "0";
    //            lblcratduedate.Text = "";
    //            lblcansduedate.Text = "";
    //        }
    //        grdIncentive.DataSource=dtincentiveamt;
    //        grdIncentive.DataBind();
    //        Report = new DataTable();
    //        Report.Columns.Add("PRODUCT NAME");
    //        Report.Columns.Add("RATE");
    //        foreach (DataRow drbp in dtBranchproducts.Rows)
    //        {
    //            DataRow newrow = Report.NewRow();
    //            newrow["PRODUCT NAME"] = drbp["ProductName"].ToString();
    //            if (drbp["Aunitprice"].ToString() == "0")
    //            {
    //                if (drbp["BUnitPrice"].ToString() == "0")
    //                {
    //                    if (drbp["UnitPrice"].ToString() == "0")
    //                    {
    //                        newrow["RATE"] = "0";
    //                    }
    //                    else
    //                    {
    //                        newrow["RATE"] = drbp["UnitPrice"].ToString();

    //                    }
    //                }
    //                else
    //                {
    //                    newrow["RATE"] = drbp["BUnitPrice"].ToString();
    //                }
    //            }
    //            else
    //            {
    //                newrow["RATE"] = drbp["Aunitprice"].ToString();

    //            }
    //            Report.Rows.Add(newrow);
    //        }
    //        //foreach (DataRow dr in dtBranchproducts.Rows)
    //        //{
    //        //    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);

    //        //}

    //        //foreach (DataRow drbp in dtBranchproducts.Rows)
    //        //{
    //        //    DataRow newrow = Report.NewRow();

    //        //    newrow["PRODUCT NAME"] = drbp["ProductName"].ToString();
    //        //    if (dtincentiveamt.Rows.Count > 0)
    //        //    {
    //        //        DataView view = new DataView(dtAgent);
    //        //        DataTable produtstbl = view.ToTable(true, "ProductName", "prodid");
    //        //        double totsaleqty = 0;
    //        //        double incentiveamt = 0;
    //        //        double discountperltr = 0;
    //        //        double milksale = 0;
    //        //        double milksalevalue = 0;
    //        //        foreach (DataRow dr in produtstbl.Rows)
    //        //        {
    //        //            float avgsale = 0;
    //        //            float slotqty = 0;
    //        //            float slotamt = 0;
    //        //            float totalsale = 0;
    //        //            float totalsaleamt = 0;
    //        //            string sltamt = "";
    //        //            double invamount = 0;
    //        //            foreach (DataRow drsale in dtAgent.Select("prodid='" + drbp["sno"].ToString() + "'"))
    //        //            {
    //        //                float.TryParse(drsale["DeliveryQty"].ToString(), out totalsale);
    //        //                float.TryParse(drsale["Totalsalevalue"].ToString(), out totalsaleamt);

    //        //                //newrow["TOTAL QTY"] = Math.Round(totalsale, 2);
    //        //                avgsale = (totalsale / count);
    //        //                if (drsale["Categoryname"].ToString() == "MILK")
    //        //                {
    //        //                    milksale += totalsale;
    //        //                    milksalevalue += totalsaleamt;
    //        //                }

    //        //            }
    //        //            string clubsno = "";
    //        //            int clubsno1 = 0;
    //        //            cmd = new MySqlCommand("SELECT subproductsclubbing.Productid, subproductsclubbing.Clubsno, slabs.SlotQty, slabs.Amt FROM incentive_struct_sub INNER JOIN subproductsclubbing ON incentive_struct_sub.clubbingID = subproductsclubbing.Clubsno INNER JOIN slabs ON subproductsclubbing.Clubsno = slabs.club_sno WHERE (incentive_struct_sub.is_sno = @structsno) AND (subproductsclubbing.Productid=@prdtid) ");
    //        //            cmd.Parameters.AddWithValue("@structsno", dtincentiveamt.Rows[0]["structure_sno"].ToString());
    //        //            cmd.Parameters.AddWithValue("@prdtid", drbp["sno"].ToString());
    //        //            DataTable dtprdtclub = vdm.SelectQuery(cmd).Tables[0];
    //        //            DataView clubview = new DataView(dtprdtclub);
    //        //            DataTable clubtbl = clubview.ToTable(true, "Clubsno");

    //        //            foreach (DataRow drclubs in clubtbl.Rows)
    //        //            {

    //        //                foreach (DataRow drclub in dtprdtclub.Select("Clubsno='" + drclubs["Clubsno"].ToString() + "'"))
    //        //                {

    //        //                        float.TryParse(drclub["SlotQty"].ToString(), out slotqty);
    //        //                        if (avgsale > slotqty)
    //        //                        {
    //        //                            float.TryParse(drclub["Amt"].ToString(), out slotamt);
    //        //                            sltamt = drclub["Amt"].ToString();

    //        //                        }


    //        //                }
    //        //                double invslot = 0;
    //        //                double.TryParse(sltamt, out invslot);
    //        //                invamount += invslot;

    //        //            }
    //        //            //newrow["INCENTIVE"] = Math.Round(totalsale * invamount, 2);
    //        //            double totamt = 0;
    //        //            totamt = Math.Round(totalsale * invamount, 2);
    //        //            if (totamt > 0)
    //        //            {
    //        //                newrow["INCENTIVE"] = Math.Round(totamt / totalsale, 2);
    //        //            }
    //        //            if (totamt <= 0)
    //        //            {
    //        //                newrow["INCENTIVE"] = "0";
    //        //            }
    //        //            //Report.Rows.Add(newrow);
    //        //            totsaleqty += totalsale;
    //        //            incentiveamt += Math.Round(totalsale * invamount, 2);
    //        //            discountperltr += Math.Round(totamt / totalsale, 2);
    //        //        }

    //        //    }
    //        //    if (dtincentiveamt.Rows.Count < 0)
    //        //    {
    //        //        newrow["INCENTIVE"] = "0";

    //        //    }
    //        //    if (drbp["Aunitprice"].ToString() == "0")
    //        //    {
    //        //        if (drbp["BUnitPrice"].ToString() == "0")
    //        //        {
    //        //            if (drbp["UnitPrice"].ToString() == "0")
    //        //            {
    //        //                newrow["RATE"] = "0";
    //        //            }
    //        //            else
    //        //            {
    //        //                newrow["RATE"] = drbp["UnitPrice"].ToString();

    //        //            }
    //        //        }
    //        //        else
    //        //        {
    //        //            newrow["RATE"] = drbp["BUnitPrice"].ToString();
    //        //        }
    //        //    }
    //        //    else
    //        //    {
    //        //        newrow["RATE"] = drbp["Aunitprice"].ToString();

    //        //    }
    //        //    Report.Rows.Add(newrow);

    //        //}
    //        grdReports.DataSource = Report;
    //        grdReports.DataBind();
    //        //fillimages();
    //    }
    //    catch
    //    {


    //    }
    //}
}