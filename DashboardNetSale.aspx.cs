using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Text;
using ClosedXML.Excel;
using System.Configuration;
using System.Web.Services;
public partial class DashboardNetSale : System.Web.UI.Page
{
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //btnsess.Attributes.Add("onclick", "return false;");
        }
        //if (Session["BranchComparisonData"] != "")
        //{
        //    DataTable dt = (DataTable)Session["BranchComparisonData"];
        //    grddata.DataSource = dt;
        //    grddata.DataBind();
            
        //}
    }

    protected void btnsessionvalue_Click(object sender, EventArgs e)
    {
        if (Session["BranchComparisonData"] != "")
        {
            DataTable dt = (DataTable)Session["BranchComparisonData"];
            grddata.DataSource = dt;
            grddata.DataBind();
            btnsess.Attributes.Add("onclick", "return false;");
        }
    }

    int i = 0;
    [WebMethod(EnableSession = true)]
    public static string report()
   {
        string Report1 = "Ok";
        DashboardNetSale obj = new DashboardNetSale();
        obj.Report();
        return Report1;
    }
    DataTable dt = new DataTable();
    public void Report()
    {
        Page page = (Page)HttpContext.Current.Handler;
        GridView grd = (GridView)page.FindControl("grdrpt");
        //GridView grd = grdrpt;
        
       // BindGrid(grdrpt, dt);
    }
    public static void BindGrid(GridView grdrpt, DataTable dt)
    {
        VehicleDBMgr vdm = new VehicleDBMgr();
        DataTable dtt = dt;
        if (dtt.Rows.Count > 0)
        {
            grdrpt.DataSource = dtt;
            grdrpt.DataBind();
        }
    }

    protected void grdReports_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            //int rowIndex = Convert.ToInt32(e.CommandArgument);
            //GridViewRow row = grdReports.Rows[rowIndex];
            //string ReceiptNo = row.Cells[1].Text;
            //Report.Columns.Add("sno");
            //Report.Columns.Add("ProductId");
            //Report.Columns.Add("Product Name");
            //Report.Columns.Add("Opening Balance");
            //Report.Columns.Add("Receipt Values");
            //Report.Columns.Add("Issues To Punabaka");
            //Report.Columns.Add("Branch Transfers");
            //Report.Columns.Add("Closing Balance");
            //lblmsg.Text = "";
            //SalesDBManager SalesDB = new SalesDBManager();
            //DateTime fromdate = DateTime.Now;
            //DateTime todate = DateTime.Now;
            //string[] datestrig = dtp_FromDate.Text.Split(' ');
            //if (datestrig.Length > 1)
            //{
            //    if (datestrig[0].Split('-').Length > 0)
            //    {
            //        string[] dates = datestrig[0].Split('-');
            //        string[] times = datestrig[1].Split(':');
            //        fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
            //    }
            //}
            //datestrig = dtp_Todate.Text.Split(' ');
            //if (datestrig.Length > 1)
            //{
            //    if (datestrig[0].Split('-').Length > 0)
            //    {
            //        string[] dates = datestrig[0].Split('-');
            //        string[] times = datestrig[1].Split(':');
            //        todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
            //    }
            //}
            //lblfrom_date.Text = fromdate.ToString("dd/MM/yyyy");
            //lblto_date.Text = todate.ToString("dd/MM/yyyy");
            //string branchid = Session["Po_BranchID"].ToString();
            //// DateTime ServerDateCurrentdate = SalesDBManager.GetTime(vdm.conn);
            //if (ddlconsumption.SelectedValue == "WithQuantity")
            //{
            //    cmd = new SqlCommand("SELECT productmaster.productcode, productmaster.productname, productmaster.productid, categorymaster.category FROM productmaster INNER JOIN categorymaster ON productmaster.categoryid = categorymaster.categoryid WHERE (productmaster.categoryid = @productcode) AND categorymaster.branchid=@branchid GROUP BY productmaster.productcode, categorymaster.category, productmaster.productname, productmaster.productid");
            //    //cmd = new SqlCommand("SELECT productmaster.productcode,productmaster.productname,productmaster.productid, categorymaster.category FROM productmaster INNER JOIN productmoniter ON productmaster.productid = productmoniter.productid INNER JOIN   categorymaster ON productmaster.productcode = categorymaster.cat_code where productmaster.productcode=@productcode AND productmoniter.branchid=@branchid  GROUP BY productmaster.productcode, categorymaster.category,productmaster.productname,productmaster.productid");
            //    cmd.Parameters.AddWithValue("@branchid", branchid);
            //    //cmd = new SqlCommand("SELECT productid, productname,price FROM productmaster where productcode=@productcode");
            //    cmd.Parameters.AddWithValue("@productcode", ReceiptNo);
            //    DataTable dtproducts = SalesDB.SelectQuery(cmd).Tables[0];
            //    cmd = new SqlCommand("SELECT stockclosingdetails.qty,stockclosingdetails.productid FROM stockclosingdetails INNER JOIN productmaster ON stockclosingdetails.productid = productmaster.productid  WHERE  (productmaster.categoryid = @ReceiptNo) AND (stockclosingdetails.doe BETWEEN @d1 AND @d2) AND (stockclosingdetails.branchid=@branchid)");
            //    // cmd = new SqlCommand("SELECT  ff.productid, ff.qty, ff.price FROM (SELECT productmaster.productid FROM  productmaster INNER JOIN categorymaster ON productmaster.categoryid = categorymaster.categoryid INNER JOIN  subcategorymaster ON categorymaster.categoryid = subcategorymaster.categoryid AND  productmaster.subcategoryid = subcategorymaster.subcategoryid WHERE (productmaster.productcode = @ReceiptNo) AND (productmaster.branchid = @branchid) GROUP BY productmaster.productid) AS ProductInfo INNER JOIN (SELECT sno, productid, qty, doe, entryby, price, branchid FROM (SELECT sno, productid, qty, doe, entryby, price, branchid FROM  stockclosingdetails WHERE (doe BETWEEN @d1 AND @d2) AND (branchid=@sbranchid)) AS Transinfo) AS ff ON ff.productid = ProductInfo.productid");
            //    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            //    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            //    cmd.Parameters.AddWithValue("@ReceiptNo", ReceiptNo);
            //    cmd.Parameters.AddWithValue("@branchid", branchid);

            //    DataTable dtInward = SalesDB.SelectQuery(cmd).Tables[0];

            //    if (dtproducts.Rows.Count > 0)
            //    {
            //        double Totalopeningqty = 0;
            //        double Totalreceptqty = 0;
            //        double Totalissueqty = 0;
            //        double Totalbqty = 0;
            //        double Totalclosingqty = 0;
            //        var i = 1;
            //        cmd = new SqlCommand("SELECT  productmaster.productid, SUM(subinwarddetails.quantity) AS inwardqty  FROM  productmaster  INNER JOIN subinwarddetails ON subinwarddetails.productid = productmaster.productid INNER JOIN  inwarddetails  ON  inwarddetails.sno=subinwarddetails.in_refno  where (inwarddetails.inwarddate BETWEEN @fromdate AND @todate) AND (inwarddetails.branchid=@branchid) AND (productmaster.categoryid=@ReceiptNo) GROUP BY productmaster.productid ");
            //        cmd.Parameters.AddWithValue("@fromdate", GetLowDate(fromdate));
            //        cmd.Parameters.AddWithValue("@todate", GetHighDate(todate));
            //        cmd.Parameters.AddWithValue("@ReceiptNo", ReceiptNo);
            //        cmd.Parameters.AddWithValue("@branchid", branchid);
            //        DataTable dtreceipt = SalesDB.SelectQuery(cmd).Tables[0];
            //        cmd = new SqlCommand("SELECT  productmaster.productid, SUM(suboutwarddetails.quantity) AS issuestopunabaka  FROM  productmaster  INNER JOIN suboutwarddetails ON suboutwarddetails.productid = productmaster.productid INNER JOIN outwarddetails ON  outwarddetails.sno= suboutwarddetails.in_refno where (outwarddetails.inwarddate BETWEEN @fromdate AND @todate) AND (outwarddetails.branchid=@branchid) AND (productmaster.categoryid=@ReceiptNo) GROUP BY productmaster.productid");
            //        cmd.Parameters.AddWithValue("@fromdate", GetLowDate(fromdate));
            //        cmd.Parameters.AddWithValue("@todate", GetHighDate(todate));
            //        cmd.Parameters.AddWithValue("@ReceiptNo", ReceiptNo);
            //        cmd.Parameters.AddWithValue("@branchid", branchid);
            //        DataTable dtIsspcode = SalesDB.SelectQuery(cmd).Tables[0];
            //        cmd = new SqlCommand("SELECT productmaster.productid, SUM(stocktransfersubdetails.quantity) AS branchtransfer  FROM  productmaster  INNER JOIN stocktransfersubdetails ON stocktransfersubdetails.productid = productmaster.productid INNER JOIN stocktransferdetails ON stocktransferdetails.sno=stocktransfersubdetails.stock_refno  where  (stocktransferdetails.invoicedate BETWEEN @fromdate AND @todate) AND (stocktransferdetails.branch_id=@branchid) AND (productmaster.categoryid=@ReceiptNo) GROUP BY productmaster.productid");
            //        cmd.Parameters.AddWithValue("@fromdate", GetLowDate(fromdate));
            //        cmd.Parameters.AddWithValue("@todate", GetHighDate(todate));
            //        cmd.Parameters.AddWithValue("@ReceiptNo", ReceiptNo);
            //        cmd.Parameters.AddWithValue("@branchid", branchid);
            //        DataTable dttransferpcode = SalesDB.SelectQuery(cmd).Tables[0];
            //        foreach (DataRow dr in dtproducts.Rows)
            //        {
            //            DataRow newrow = Report.NewRow();
            //            newrow["sno"] = i++.ToString();
            //            double openingqty = 0;
            //            double receptqty = 0;
            //            double issueqty = 0;
            //            double bqty = 0;
            //            newrow["ProductId"] = dr["productid"].ToString();
            //            newrow["Product Name"] = dr["productname"].ToString();

            //            foreach (DataRow dropp in dtInward.Select("productid='" + dr["productid"].ToString() + "'"))
            //            {
            //                double qty = 0;
            //                double.TryParse(dropp["qty"].ToString(), out qty);
            //                openingqty = Math.Round(qty, 2);
            //                Totalopeningqty += openingqty;
            //                newrow["Opening Balance"] = openingqty.ToString();
            //            }
            //            foreach (DataRow drreceipt in dtreceipt.Select("productid='" + dr["productid"].ToString() + "'"))
            //            {
            //                double.TryParse(drreceipt["inwardqty"].ToString(), out receptqty);
            //                double reciptpunabaka = receptqty;
            //                reciptpunabaka = Math.Round(reciptpunabaka, 2);
            //                newrow["Receipt Values"] = reciptpunabaka;
            //                Totalreceptqty += receptqty;
            //            }
            //            if (dr["productid"].ToString() == "0")
            //            {
            //                DataTable dt_diesel = new DataTable();
            //                cmd = new SqlCommand("SELECT SUM(diesel_consumptiondetails.qty) AS qty, diesel_consumptiondetails.productid, productmaster.price FROM diesel_consumptiondetails INNER JOIN productmaster ON diesel_consumptiondetails.productid = productmaster.productid WHERE (diesel_consumptiondetails.branchid = @branchid) AND (diesel_consumptiondetails.doe BETWEEN @fromdate AND @todate) GROUP BY diesel_consumptiondetails.productid, productmaster.price");
            //                cmd.Parameters.AddWithValue("@fromdate", GetLowDate(fromdate));
            //                cmd.Parameters.AddWithValue("@todate", GetHighDate(todate));
            //                cmd.Parameters.AddWithValue("@branchid", branchid);
            //                dt_diesel = SalesDB.SelectQuery(cmd).Tables[0];
            //                if (dt_diesel.Rows.Count > 0)
            //                {
            //                    double.TryParse(dt_diesel.Rows[0]["qty"].ToString(), out issueqty);
            //                    double isspunabaka = issueqty;
            //                    isspunabaka = Math.Round(isspunabaka, 2);
            //                    newrow["Issues To Punabaka"] = isspunabaka;
            //                    Totalissueqty += issueqty;
            //                }
            //            }
            //            else
            //            {
            //                foreach (DataRow drissue in dtIsspcode.Select("productid='" + dr["productid"].ToString() + "'"))
            //                {
            //                    double.TryParse(drissue["issuestopunabaka"].ToString(), out issueqty);
            //                    double isspunabaka = issueqty;
            //                    isspunabaka = Math.Round(isspunabaka, 2);
            //                    newrow["Issues To Punabaka"] = isspunabaka;
            //                    Totalissueqty += issueqty;
            //                }
            //            }

            //            foreach (DataRow drtransfer in dttransferpcode.Select("productid='" + dr["productid"].ToString() + "'"))
            //            {
            //                double.TryParse(drtransfer["branchtransfer"].ToString(), out bqty);
            //                Totalbqty += bqty;
            //                newrow["Branch Transfers"] = drtransfer["branchtransfer"].ToString();
            //            }
            //            double openreceiptvalue = 0;
            //            openreceiptvalue = openingqty + receptqty;
            //            double issueandtransfervalue = 0;
            //            issueandtransfervalue = issueqty + bqty;
            //            double closingqty = 0;
            //            closingqty = openreceiptvalue - issueandtransfervalue;
            //            double closingqty1 = closingqty;
            //            closingqty1 = Math.Round(closingqty1, 2);
            //            newrow["Closing Balance"] = closingqty1;
            //            Report.Rows.Add(newrow);
            //        }
            //        double Totalopenreceiptvalue = 0;
            //        Totalopenreceiptvalue += Totalopeningqty + Totalreceptqty;
            //        double Totalissueandtransfervalue = 0;
            //        Totalissueandtransfervalue += Totalissueqty + Totalbqty;
            //        Totalclosingqty += Totalopenreceiptvalue - Totalissueandtransfervalue;
            //        DataRow stockreport = Report.NewRow();
            //        stockreport["ProductId"] = "TotalValue";
            //        stockreport["Opening Balance"] = Math.Round(Totalopeningqty, 2); //Totalopeningqty;
            //        double Totalreceptqty1 = Totalreceptqty;
            //        Totalreceptqty1 = Math.Round(Totalreceptqty1, 2);
            //        stockreport["Receipt Values"] = Totalreceptqty1;
            //        double Totalissueqty1 = Totalissueqty;
            //        Totalissueqty1 = Math.Round(Totalissueqty1, 2);
            //        stockreport["Issues To Punabaka"] = Math.Round(Totalissueqty1, 2);//Totalissueqty1;
            //        double Totalbqty1 = Totalbqty;
            //        stockreport["Branch Transfers"] = Totalbqty1;
            //        stockreport["Closing Balance"] = Math.Round(Totalclosingqty, 2); //Totalclosingqty;
            //        Report.Rows.Add(stockreport);
            //        GrdProducts.DataSource = Report;
            //        GrdProducts.DataBind();
            //        hidepanel.Visible = true;
            //    }
            //    else
            //    {
            //        lblmsg.Text = "No data were found";
            //        hidepanel.Visible = false;
            //    }
            //}
            //else
            //{
            //    if (ddlconsumption.SelectedValue == "WithAmount")
            //    {
            //        cmd = new SqlCommand("SELECT productmaster.productcode,productmaster.productname,productmaster.productid, categorymaster.category FROM productmaster INNER JOIN productmoniter ON productmaster.productid = productmoniter.productid INNER JOIN   categorymaster ON productmaster.productcode = categorymaster.cat_code where productmaster.productcode=@productcode AND productmoniter.branchid=@branchid  GROUP BY productmaster.productcode, categorymaster.category,productmaster.productname,productmaster.productid");
            //        // cmd = new SqlCommand("SELECT productmaster.productcode,productmaster.productname,productmaster.productid, categorymaster.category FROM productmaster INNER JOIN subcategorymaster ON productmaster.subcategoryid = subcategorymaster.subcategoryid INNER JOIN categorymaster ON productmaster.productcode = categorymaster.cat_code AND subcategorymaster.categoryid = categorymaster.categoryid GROUP BY productmaster.productcode, categorymaster.category,productmaster.productid,productmaster.productname");
            //        cmd.Parameters.AddWithValue("@productcode", ReceiptNo);
            //        cmd.Parameters.AddWithValue("@branchid", branchid);
            //        DataTable dtproducts = SalesDB.SelectQuery(cmd).Tables[0];
            //        cmd = new SqlCommand("SELECT ff.productid, ff.qty, ff.price FROM (SELECT productmaster.productid FROM productmaster INNER JOIN categorymaster ON productmaster.productcode = categorymaster.cat_code INNER JOIN  subcategorymaster ON categorymaster.categoryid = subcategorymaster.categoryid AND  productmaster.sub_cat_code = subcategorymaster.sub_cat_code WHERE (productmaster.productcode = @ReceiptNo) AND (productmaster.branchid = @branchid) GROUP BY productmaster.productid) AS ProductInfo INNER JOIN (SELECT  sno, productid, qty, doe, entryby, price, branchid FROM (SELECT  sno, productid, qty, doe, entryby, price, branchid FROM stockclosingdetails WHERE (doe BETWEEN @d1 AND @d2)) AS Transinfo) AS ff ON ff.productid = ProductInfo.productid");
            //        //cmd = new SqlCommand("SELECT productmaster.productcode, categorymaster.category FROM productmaster INNER JOIN   categorymaster ON productmaster.productcode = categorymaster.cat_code GROUP BY productmaster.productcode, categorymaster.category");
            //        cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            //        cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
            //        cmd.Parameters.AddWithValue("@ReceiptNo", ReceiptNo);
            //        cmd.Parameters.AddWithValue("@branchid", branchid);
            //        //cmd = new SqlCommand("SELECT  productcode, SUM(aqty) AS openingbalance FROM  productmaster GROUP BY  productcode ORDER BY productcode");
            //        DataTable dtInward = SalesDB.SelectQuery(cmd).Tables[0];
            //        if (dtproducts.Rows.Count > 0)
            //        {
            //            double Totalopeningqty = 0;
            //            double Totalreceptqty = 0;
            //            double Totalissueqty = 0;
            //            double Totalbqty = 0;
            //            double Totalclosingqty = 0;
            //            var i = 1;
            //            cmd = new SqlCommand("SELECT productmaster.productid, SUM(subinwarddetails.quantity * subinwarddetails.perunit) AS inwardqty  FROM  productmaster  INNER JOIN subinwarddetails ON subinwarddetails.productid = productmaster.productid INNER JOIN  inwarddetails  ON  inwarddetails.sno=subinwarddetails.in_refno  where (inwarddetails.inwarddate BETWEEN @fromdate AND @todate) AND (productmaster.productcode=@ReceiptNo) AND (inwarddetails.branchid=@branchid) GROUP BY productmaster.productid ");
            //            cmd.Parameters.AddWithValue("@fromdate", GetLowDate(fromdate));
            //            cmd.Parameters.AddWithValue("@todate", GetHighDate(todate));
            //            cmd.Parameters.AddWithValue("@ReceiptNo", ReceiptNo);
            //            cmd.Parameters.AddWithValue("@branchid", branchid);
            //            DataTable dtreceipt = SalesDB.SelectQuery(cmd).Tables[0];
            //            cmd = new SqlCommand("SELECT  productmaster.productid, SUM(suboutwarddetails.quantity * suboutwarddetails.perunit) AS issuestopunabaka  FROM  productmaster  INNER JOIN suboutwarddetails ON suboutwarddetails.productid = productmaster.productid INNER JOIN outwarddetails ON  outwarddetails.sno= suboutwarddetails.in_refno where (outwarddetails.inwarddate BETWEEN @fromdate AND @todate) AND (productmaster.productcode=@ReceiptNo) AND (outwarddetails.branchid=@branchid) GROUP BY productmaster.productid");
            //            cmd.Parameters.AddWithValue("@fromdate", GetLowDate(fromdate));
            //            cmd.Parameters.AddWithValue("@todate", GetHighDate(todate));
            //            cmd.Parameters.AddWithValue("@ReceiptNo", ReceiptNo);
            //            cmd.Parameters.AddWithValue("@branchid", branchid);
            //            DataTable dtIsspcode = SalesDB.SelectQuery(cmd).Tables[0];
            //            cmd = new SqlCommand("SELECT productmaster.productid, SUM(stocktransfersubdetails.quantity * stocktransfersubdetails.price) AS branchtransfer  FROM  productmaster  INNER JOIN stocktransfersubdetails ON stocktransfersubdetails.productid = productmaster.productid INNER JOIN stocktransferdetails ON stocktransferdetails.sno=stocktransfersubdetails.stock_refno  where  (stocktransferdetails.invoicedate BETWEEN @fromdate AND @todate) AND (productmaster.productcode=@ReceiptNo) AND (stocktransferdetails.branch_id=@branchid) GROUP BY productmaster.productid");
            //            cmd.Parameters.AddWithValue("@fromdate", GetLowDate(fromdate));
            //            cmd.Parameters.AddWithValue("@todate", GetHighDate(todate));
            //            cmd.Parameters.AddWithValue("@ReceiptNo", ReceiptNo);
            //            cmd.Parameters.AddWithValue("@branchid", branchid);
            //            DataTable dttransferpcode = SalesDB.SelectQuery(cmd).Tables[0];
            //            foreach (DataRow dr in dtproducts.Rows)
            //            {
            //                DataRow newrow = Report.NewRow();
            //                newrow["sno"] = i++.ToString();
            //                double openingqty = 0;
            //                double receptqty = 0;
            //                double issueqty = 0;
            //                double bqty = 0;
            //                newrow["ProductId"] = dr["productid"].ToString();
            //                newrow["Product Name"] = dr["productname"].ToString();
            //                foreach (DataRow dropp in dtInward.Select("productid='" + dr["productid"].ToString() + "'"))
            //                {
            //                    double qty = 0;
            //                    double.TryParse(dropp["qty"].ToString(), out qty);
            //                    double price = 0;
            //                    double.TryParse(dropp["price"].ToString(), out price);
            //                    openingqty = qty * price;
            //                    openingqty = Math.Round(openingqty, 2);
            //                    Totalopeningqty += openingqty;
            //                    newrow["Opening Balance"] = openingqty.ToString();
            //                }
            //                foreach (DataRow drreceipt in dtreceipt.Select("productid='" + dr["productid"].ToString() + "'"))
            //                {
            //                    double.TryParse(drreceipt["inwardqty"].ToString(), out receptqty);
            //                    double reciptpunabaka = receptqty;
            //                    reciptpunabaka = Math.Round(reciptpunabaka, 2);
            //                    newrow["Receipt Values"] = reciptpunabaka;
            //                    Totalreceptqty += receptqty;
            //                }
            //                if (dr["productid"].ToString() == "2285")
            //                {
            //                    DataTable dt_diesel = new DataTable();
            //                    cmd = new SqlCommand("SELECT SUM(diesel_consumptiondetails.qty * productmoniter.price) AS value FROM diesel_consumptiondetails INNER JOIN productmoniter ON diesel_consumptiondetails.productid = productmoniter.productid WHERE (diesel_consumptiondetails.doe BETWEEN @fromdate AND @todate) AND (diesel_consumptiondetails.branchid = @branch_id) AND (productmoniter.branchid = @branchid) GROUP BY diesel_consumptiondetails.productid, productmoniter.price");
            //                    cmd.Parameters.AddWithValue("@fromdate", GetLowDate(fromdate));
            //                    cmd.Parameters.AddWithValue("@todate", GetHighDate(todate));
            //                    cmd.Parameters.AddWithValue("@branchid", branchid);
            //                    cmd.Parameters.AddWithValue("@branch_id", branchid);
            //                    dt_diesel = SalesDB.SelectQuery(cmd).Tables[0];
            //                    if (dt_diesel.Rows.Count > 0)
            //                    {
            //                        double.TryParse(dt_diesel.Rows[0]["value"].ToString(), out issueqty);
            //                        double isspunabaka = issueqty;
            //                        isspunabaka = Math.Round(isspunabaka, 2);
            //                        newrow["Issues To Punabaka"] = isspunabaka;
            //                        Totalissueqty += issueqty;
            //                    }
            //                }
            //                else
            //                {
            //                    foreach (DataRow drissue in dtIsspcode.Select("productid='" + dr["productid"].ToString() + "'"))
            //                    {
            //                        double.TryParse(drissue["issuestopunabaka"].ToString(), out issueqty);
            //                        double isspunabaka = issueqty;
            //                        isspunabaka = Math.Round(isspunabaka, 2);
            //                        newrow["Issues To Punabaka"] = isspunabaka;
            //                        Totalissueqty += issueqty;
            //                    }
            //                }
            //                foreach (DataRow drtransfer in dttransferpcode.Select("productid='" + dr["productid"].ToString() + "'"))
            //                {
            //                    double.TryParse(drtransfer["branchtransfer"].ToString(), out bqty);
            //                    Totalbqty += bqty;
            //                    newrow["Branch Transfers"] = drtransfer["branchtransfer"].ToString();
            //                }
            //                double openreceiptvalue = 0;
            //                openreceiptvalue = openingqty + receptqty;
            //                double issueandtransfervalue = 0;
            //                issueandtransfervalue = issueqty + bqty;
            //                double closingqty = 0;
            //                closingqty = openreceiptvalue - issueandtransfervalue;
            //                double closingqty1 = closingqty;
            //                closingqty1 = Math.Round(closingqty1, 2);
            //                newrow["Closing Balance"] = closingqty1;
            //                Report.Rows.Add(newrow);
            //            }
            //            double Totalopenreceiptvalue = 0;
            //            Totalopenreceiptvalue += Totalopeningqty + Totalreceptqty;
            //            double Totalissueandtransfervalue = 0;
            //            Totalissueandtransfervalue += Totalissueqty + Totalbqty;
            //            Totalclosingqty += Totalopenreceiptvalue - Totalissueandtransfervalue;
            //            DataRow stockreport = Report.NewRow();
            //            stockreport["ProductId"] = "TotalValue";
            //            stockreport["Opening Balance"] = Math.Round(Totalopeningqty, 2); //Totalopeningqty;
            //            double Totalreceptqty1 = Totalreceptqty;
            //            Totalreceptqty1 = Math.Round(Totalreceptqty1, 2);
            //            stockreport["Receipt Values"] = Totalreceptqty1;
            //            double Totalissueqty1 = Totalissueqty;
            //            Totalissueqty1 = Math.Round(Totalissueqty1, 2);
            //            stockreport["Issues To Punabaka"] = Math.Round(Totalissueqty1, 2);// Totalissueqty1;
            //            double Totalbqty1 = Totalbqty;
            //            stockreport["Branch Transfers"] = Math.Round(Totalbqty1, 2);// Totalbqty1;
            //            stockreport["Closing Balance"] = Math.Round(Totalclosingqty, 2); //Totalclosingqty;
            //            Report.Rows.Add(stockreport);
            //            GrdProducts.DataSource = Report;
            //            GrdProducts.DataBind();
            //            hidepanel.Visible = true;
            //        }

            //        else
            //        {
            //            lblmsg.Text = "No data were found";
            //            hidepanel.Visible = false;
            //        }
            //    }
            //}
            Response.Redirect(Request.RawUrl);
        }
        catch (Exception ex)
        {
            //lblmsg.Text = ex.Message;
            //hidepanel.Visible = false;
        }
    }
}