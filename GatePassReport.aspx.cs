using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class GatePassReport : System.Web.UI.Page
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
                lblTitle.Text = Session["TitleName"].ToString();
                //lblAddress.Text = Session["Address"].ToString();
                lbltinNo.Text = Session["TinNo"].ToString();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txttodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            }
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
    protected void btn_getdetails_Click(object sender, EventArgs e)
    {
        getdet();
    }
    void getdet()
    {
        try
        {
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            DateTime todate = DateTime.Now;
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
            cmd = new MySqlCommand("SELECT sno,gatepassno, doe, vehicleno, routename, partyname FROM gatepassdeatails WHERE (branchid = @BranchID) AND (doe BETWEEN @d1 AND @d2)");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            DataTable dtGatePass = vdm.SelectQuery(cmd).Tables[0];
            Gridtripdata.DataSource = dtGatePass;
            Gridtripdata.DataBind();
        }
        catch
        {
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        getgatepassReport();
    }
    void getgatepassReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            lblrefno.Text = txt_gatePassid.Text;
            DataTable Report = new DataTable();
            Report.Columns.Add("Sno");
            Report.Columns.Add("Product Name");
            Report.Columns.Add("Qty");
            Report.Columns.Add("Crates");
            Report.Columns.Add("Cans");
            Report.Columns.Add("Bags");
            cmd = new MySqlCommand("SELECT   gatepassdeatails.sno, gatepassdeatails.gatepassno, gatepassdeatails.doe, gatepassdeatails.vehicleno, gatepassdeatails.routename, gatepassdeatails.partyname, ROUND(SUM(tripsubdata.Qty), 2) AS Qty,tripdata.DCNo,tripdata.BranchID, tripsubdata.Qty,tripdata.Sno AS tripSno,  productsdata.ProductName, products_category.Categoryname FROM gatepassdeatails INNER JOIN gatepass_subtable ON gatepassdeatails.sno = gatepass_subtable.gatepass_refno INNER JOIN tripdata ON gatepass_subtable.refdcno = tripdata.Sno INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (gatepassdeatails.sno = @GatePassID) AND (products_category.sno = 1) AND (productsdata.Inventorysno = 1) GROUP BY products_category.Categoryname ORDER BY productsdata.Rank");
            cmd.Parameters.AddWithValue("@GatePassID", txt_gatePassid.Text);
            DataTable dtProducts = vdm.SelectQuery(cmd).Tables[0];
            int i = 1;
            string gpno = "";
            foreach (DataRow dr in dtProducts.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["Sno"] = i++.ToString();
                newrow["Product Name"] = "Milk";
                newrow["Qty"] = dr["Qty"].ToString();
                gpno = dr["gatepassno"].ToString();
                Report.Rows.Add(newrow);
            }
            cmd = new MySqlCommand("SELECT gatepassdeatails.sno, gatepassdeatails.gatepassno, gatepassdeatails.doe, gatepassdeatails.vehicleno, gatepassdeatails.routename, gatepassdeatails.partyname, ROUND(SUM(tripsubdata.Qty), 2) AS Qty, tripdata.DCNo, tripdata.BranchID, tripsubdata.Qty AS Expr1, tripdata.Sno AS tripSno, productsdata.ProductName, products_category.Categoryname, branchproducts.Rank FROM gatepassdeatails INNER JOIN gatepass_subtable ON gatepassdeatails.sno = gatepass_subtable.gatepass_refno INNER JOIN tripdata ON gatepass_subtable.refdcno = tripdata.Sno INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno WHERE (gatepassdeatails.sno = @GatePassID) AND (products_category.sno = 1) AND (productsdata.Inventorysno = 2) AND (branchproducts.branch_sno = @branchid) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");

            //cmd = new MySqlCommand("SELECT    gatepassdeatails.sno, gatepassdeatails.gatepassno, gatepassdeatails.doe, gatepassdeatails.vehicleno, gatepassdeatails.routename, gatepassdeatails.partyname, ROUND(SUM(tripsubdata.Qty), 2) AS Qty,tripdata.DCNo,tripdata.BranchID, tripsubdata.Qty,tripdata.Sno AS tripSno,  productsdata.ProductName, products_category.Categoryname  FROM gatepassdeatails INNER JOIN gatepass_subtable ON gatepassdeatails.sno = gatepass_subtable.gatepass_refno INNER JOIN tripdata ON gatepass_subtable.refdcno = tripdata.Sno INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (gatepassdeatails.sno = @GatePassID) AND (products_category.sno = 9) AND (productsdata.Inventorysno = 4) GROUP BY productsdata.ProductName ORDER BY productsdata.Rank");
            cmd.Parameters.AddWithValue("@branchid", Session["branch"]);
           
            cmd.Parameters.AddWithValue("@GatePassID", txt_gatePassid.Text);
            DataTable dtMilkCan = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in dtMilkCan.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["Sno"] = i++.ToString();
                newrow["Product Name"] = "Milk Cans";
                newrow["Qty"] = dr["Qty"].ToString();
                gpno = dr["gatepassno"].ToString();
                Report.Rows.Add(newrow);
            }
           
            cmd = new MySqlCommand("SELECT branchdata.BranchCode,branchdata.phonenumber,branchdata.emailid, statemastar.statecode, branchdata.BranchName, branchdata.tbranchname, branchdata.statename, branchdata.city, branchdata.street, branchdata.mandal, branchdata.district, branchdata.pincode, branchdata.cst,  branchdata.gstno, branchdata.doorno, branchdata.area, statemastar.statename AS BranchState FROM branchdata INNER JOIN statemastar ON branchdata.stateid = statemastar.sno WHERE (branchdata.sno = @BranchID)");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            DataTable dtbranchaddress = vdm.SelectQuery(cmd).Tables[0];

            lblAddress.Text = dtbranchaddress.Rows[0]["doorno"].ToString() + "," + dtbranchaddress.Rows[0]["street"].ToString() + "," + dtbranchaddress.Rows[0]["area"].ToString() + "," + dtbranchaddress.Rows[0]["mandal"].ToString() + "," + dtbranchaddress.Rows[0]["city"].ToString() + "," + dtbranchaddress.Rows[0]["district"].ToString() + " District -" + dtbranchaddress.Rows[0]["pincode"].ToString() + ",State:" + dtbranchaddress.Rows[0]["BranchState"].ToString() + ",Phone:" + dtbranchaddress.Rows[0]["phonenumber"].ToString();
            cmd = new MySqlCommand("SELECT       gatepassdeatails.sno, gatepassdeatails.gatepassno, gatepassdeatails.doe, gatepassdeatails.vehicleno, gatepassdeatails.routename, gatepassdeatails.partyname, ROUND(SUM(tripsubdata.Qty), 2) AS Qty, tripdata.DCNo, tripdata.BranchID, tripsubdata.Qty AS Expr1, tripdata.Sno AS tripSno, productsdata.ProductName, products_category.Categoryname FROM gatepassdeatails INNER JOIN gatepass_subtable ON gatepassdeatails.sno = gatepass_subtable.gatepass_refno INNER JOIN tripdata ON gatepass_subtable.refdcno = tripdata.Sno INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno WHERE  (gatepassdeatails.sno = @GatePassID) AND (products_category.sno <> 1) AND (branchproducts.branch_sno = @branchid) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
            cmd.Parameters.AddWithValue("@branchid", Session["branch"]);
            
            //cmd = new MySqlCommand("SELECT    gatepassdeatails.sno, gatepassdeatails.gatepassno, gatepassdeatails.doe, gatepassdeatails.vehicleno, gatepassdeatails.routename, gatepassdeatails.partyname, ROUND(SUM(tripsubdata.Qty), 2) AS Qty,tripdata.DCNo,tripdata.BranchID, tripsubdata.Qty,tripdata.Sno AS tripSno,  productsdata.ProductName, products_category.Categoryname FROM gatepassdeatails INNER JOIN gatepass_subtable ON gatepassdeatails.sno = gatepass_subtable.gatepass_refno INNER JOIN tripdata ON gatepass_subtable.refdcno = tripdata.Sno INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (gatepassdeatails.sno = @GatePassID) AND (products_category.sno <> 9) GROUP BY productsdata.ProductName ORDER BY productsdata.Rank");

            cmd.Parameters.AddWithValue("@GatePassID", txt_gatePassid.Text);
            DataTable dtBiProducts = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in dtBiProducts.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["Sno"] = i++.ToString();
                newrow["Product Name"] = dr["ProductName"].ToString();
                newrow["Qty"] = dr["Qty"].ToString();
                gpno = dr["gatepassno"].ToString();
                Report.Rows.Add(newrow);
            }
            lblGatePassNo.Text = gpno.ToString();
            DataRow newvartical1 = Report.NewRow();
            newvartical1["Sno"] = "Inventory";
            newvartical1["Product Name"] = "";
            Report.Rows.Add(newvartical1);
            cmd = new MySqlCommand("SELECT gatepassdeatails.sno, gatepassdeatails.doe, gatepassdeatails.vehicleno, gatepassdeatails.routename, gatepassdeatails.partyname, tripdata.Sno AS tripSno, tripdata.BranchID, tripdata.DCNo, SUM(tripinvdata.Qty) AS Qty, invmaster.InvName,invmaster.sno as invid  FROM  tripdata INNER JOIN gatepass_subtable ON tripdata.Sno = gatepass_subtable.refdcno INNER JOIN gatepassdeatails ON gatepass_subtable.gatepass_refno = gatepassdeatails.sno INNER JOIN tripinvdata ON tripdata.Sno = tripinvdata.Tripdata_sno INNER JOIN invmaster ON tripinvdata.invid = invmaster.sno WHERE (gatepassdeatails.sno = @GatePassID) GROUP BY invmaster.InvName");
            cmd.Parameters.AddWithValue("@GatePassID", txt_gatePassid.Text);
            DataTable dt = vdm.SelectQuery(cmd).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                DataRow drnew = Report.NewRow();
                drnew["Sno"] = dr["InvName"].ToString();
                drnew["Product Name"] = dr["Qty"].ToString();
                Report.Rows.Add(drnew);
            }
            cmd = new MySqlCommand("SELECT tripdata.VehicleNo,tripdata.DispTime,tripdata.Sno, tripdata.EmpId, tripdata.DCNo,gatepassdeatails.routename,gatepassdeatails.Partyname, tripdata.BranchID FROM  gatepassdeatails INNER JOIN gatepass_subtable ON gatepassdeatails.sno = gatepass_subtable.gatepass_refno INNER JOIN  tripdata ON gatepass_subtable.refdcno = tripdata.Sno WHERE (gatepassdeatails.sno = @GatePassID)");
            cmd.Parameters.AddWithValue("@GatePassID", txt_gatePassid.Text);
            DataTable Dctbl = vdm.SelectQuery(cmd).Tables[0];
            DataView view = new DataView(Dctbl);
            DataTable nametbl = view.ToTable(true, "Sno", "DispTime", "routename", "VehicleNo", "Partyname");
            if (nametbl.Rows.Count > 0)
            {
                string strDate = nametbl.Rows[0]["DispTime"].ToString();
                DateTime dtassigndate = Convert.ToDateTime(strDate);
                string date = dtassigndate.ToString("dd/MMM/yyyy");
                string strassigndate = dtassigndate.ToString();
                string[] DateTime = strassigndate.Split(' ');
                lblDate.Text = date;
                lblTime.Text = DateTime[1];
                lblVehicleNo.Text = nametbl.Rows[0]["VehicleNo"].ToString();
                lblName.Text = nametbl.Rows[0]["Partyname"].ToString();
            }
            DataRow newvartical = Report.NewRow();
            newvartical["Sno"] = "DC No";
            newvartical["Product Name"] = "";
            Report.Rows.Add(newvartical);
            int k = 1;
            foreach (DataRow dr in Dctbl.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["Sno"] = k++.ToString();
                newrow["Product Name"] = "Ref No" + " / " + "Dc No";
                //newrow["Invno"] = "0";

                string BranchId = dr["BranchID"].ToString();
                string DcNo = dr["DCNo"].ToString();
                if (BranchId == "172")
                {
                    DcNo = "P" + DcNo;
                }
                if (BranchId == "1801")
                {
                    DcNo = "K" + DcNo;
                }
                else if (BranchId == "7")
                {
                    DcNo = "W" + DcNo;
                }
                else if (BranchId == "174")
                {
                    DcNo = "CSO" + DcNo;
                }
                else if (BranchId == "285")
                {
                    DcNo = "TPT" + DcNo;
                }
                else if (BranchId == "282")
                {
                    DcNo = "SKHT" + DcNo;
                }
                else if (BranchId == "271")
                {
                    DcNo = "NLR" + DcNo;
                }
                else if (BranchId == "306")
                {
                    DcNo = "KANCHI" + DcNo;
                }
                else if (BranchId == "570")
                {
                    DcNo = "VJD" + DcNo;
                }
                else if (BranchId == "3")
                {
                    DcNo = "KHM" + DcNo;
                }
                else if (BranchId == "159")
                {
                    DcNo = "HYD" + DcNo;
                }
                else if (BranchId == "457")
                {
                    DcNo = "WGL" + DcNo;
                }
                else if (BranchId == "538")
                {
                    DcNo = "BNGLR" + DcNo;
                }
                else if (BranchId == "527")
                {
                    DcNo = "PNR" + DcNo;
                }
                newrow["Qty"] = dr["Sno"].ToString() + " / " + DcNo;
                Report.Rows.Add(newrow);
            }
            grdReports.DataSource = Report;
            grdReports.DataBind();
            pnlHide.Visible = true;
        }
        catch
        {
        }
    }
}