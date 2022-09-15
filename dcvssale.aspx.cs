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

public partial class dcvssale : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["branch"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                lblTitle.Text = Session["TitleName"].ToString();
                FillAgentName();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("BranchName");
                dtBranch.Columns.Add("sno");
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) and (branchdata.flag<>0) ");
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
                cmd = new MySqlCommand("SELECT BranchName, sno FROM  branchdata WHERE (sno = @BranchID) and branchdata.flag<>0");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtPlant.Rows)
                {
                    DataRow newrow = dtBranch.NewRow();
                    newrow["BranchName"] = dr["BranchName"].ToString();
                    newrow["sno"] = dr["sno"].ToString();
                    dtBranch.Rows.Add(newrow);
                }
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType and branchdata.flag<>0)");
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
            }
            else
            {
                PBranch.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata.flag = 1) AND  ((branchdata_1.SalesOfficeID = @SOID) AND (branchdata_1.flag=@flag)) OR ((branchdata.sno = @BranchID) AND (branchdata.flag=@flag))");
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                cmd.Parameters.AddWithValue("@flag", "1");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
            }
        }
        catch
        {
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
    DataTable Report = new DataTable();
    DataTable dtSortedSubCategory = new DataTable();
    DataTable dtSortedCategory = new DataTable();
    DataTable dttempproducts = new DataTable();
    DataTable produtstbl1 = new DataTable();
    DataTable dtSubCatgory = new DataTable();
    DataTable dtCatgory = new DataTable();
    DataTable dtSortedCategoryAndSubCat = new DataTable();

    DataTable dtCatgoryAndSub = new DataTable();
    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            pnlHide.Visible = true;
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
            lblSalesOffice.Text = ddlSalesOffice.SelectedItem.Text;
            lbl_fromDate.Text = txtdate.Text;
            lbl_selttodate.Text = txtTodate.Text;
            DataTable tempbranchindentsale = new DataTable();
            cmd = new MySqlCommand("SELECT TripInfo.Sno, ROUND(SUM(ProductInfo.Qty), 2) AS dispatchqty, TripInfo.BranchName, TripInfo.Branch_Id, TripInfo.DispName, TripInfo.BranchID, DATE_FORMAT(TripInfo.I_Date, '%d %b %y') AS I_Date  FROM    (SELECT        tripdata.Sno, tripdata.AssignDate, tripdata.I_Date, branchdata_1.BranchName, dispatch.BranchID, dispatch.Branch_Id, dispatch.GroupId, dispatch.CompanyId, dispatch.DispName   FROM            branchdata INNER JOIN  dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN  triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN  tripdata ON triproutes.Tripdata_sno = tripdata.Sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno   WHERE        (dispatch.Branch_Id = @BranchID) AND (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @SUBBRANCH)) TripInfo INNER JOIN (SELECT  Sno,Qty,Categoryname, ProductName, Sno, Qty,uomqty FROM  (SELECT tripdata_1.Sno, tripsubdata.Qty,products_category.Categoryname, productsdata.ProductName,productsdata.Qty as uomqty   FROM            tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno  WHERE        (tripdata_1.AssignDate BETWEEN @d1 AND @d2)) TripSubInfo) ProductInfo ON TripInfo.Sno = ProductInfo.Sno  GROUP BY TripInfo.DispName, TripInfo.BranchID, TripInfo.I_Date  ORDER BY TripInfo.AssignDate");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@SUBBRANCH", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtDispatchesbranches = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT DATE_FORMAT(clotrans.IndDate,'%d %b %y') as inddate,clotrans.BranchId,Sum(closubtranprodcts.StockQty) as openingstock FROM clotrans INNER JOIN closubtranprodcts ON clotrans.Sno = closubtranprodcts.RefNo WHERE (clotrans.BranchId = @BranchID) AND (clotrans.IndDate BETWEEN @d1 AND @d2) AND (clotrans.Transaction_Type = 0) GROUP BY clotrans.IndDate");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-2)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-2)));
            DataTable dtOppening_stock = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty), 2) AS DeliveryQty, ROUND(SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost), 2) AS salevalue,  DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate,  branchmappingtable.SuperBranch FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchmappingtable ON indents.Branch_id = branchmappingtable.SubBranch WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @brnchid) GROUP BY IndentDate,  branchmappingtable.SuperBranch ORDER BY indents.I_date");
            cmd.Parameters.AddWithValue("@brnchid", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtAgent = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT SUM(Leaks.VLeaks) AS vleaks, SUM(Leaks.VReturns) AS vreturns, SUM(Leaks.TotalLeaks) AS totleaks, SUM(Leaks.ReturnQty) AS returnqty, Leaks.AssignDate,Leaks.ReturnDCTime, ff.BranchID FROM (SELECT leakages.VLeaks, leakages.VReturns, leakages.TotalLeaks, productsdata.ProductName, tripdata_1.Sno, leakages.ReturnQty, tripdata_1.AssignDate,tripdata_1.ReturnDCTime FROM tripdata tripdata_1 INNER JOIN leakages ON tripdata_1.Sno = leakages.TripID INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata_1.I_Date BETWEEN @d1 AND @d2)) Leaks INNER JOIN (SELECT DispName, Sno, DespSno, BranchID FROM (SELECT dispatch.DispName, tripdata.Sno, dispatch.sno AS DespSno, dispatch.BranchID FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (dispatch.BranchID = @BranchID)) TripInfo) ff ON ff.Sno = Leaks.Sno GROUP BY Leaks.AssignDate ORDER BY Leaks.AssignDate");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable DtLeksAndReturns = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno, dispatch.BranchID, tripdata.I_Date,tripdata.sno as TripSno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (dispatch.BranchID = @BranchID) AND (tripdata.I_Date BETWEEN @d1 AND @d2)  and (dispatch.DispType='SO') and (tripdata.Status<>'C')  group by tripdata.Sno ORDER BY tripdata.I_Date");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtoldnames = vdm.SelectQuery(cmd).Tables[0];
            DataView view1 = new DataView(dtoldnames);
            DataTable distincttable1 = view1.ToTable(true, "sno");
            DataTable dtShortAndFree = new DataTable();
            DataTable DtTripId = new DataTable();
            foreach (DataRow dr in distincttable1.Rows)
            {
                cmd = new MySqlCommand("SELECT ff.Sno,ff.Empid,DATE_FORMAT(ff.I_date,'%d %b %y') as inddate , ff.FreeMilk, ff.ShortQty FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (RouteID = @dispatchSno)) Triproutes INNER JOIN (SELECT Sno,Empid,I_date, TotalLeaks, VLeaks, VReturns, ReturnQty, ProductName, ProductID, FreeMilk, ShortQty FROM (SELECT tripdata.Sno,tripdata.Empid,tripdata.I_date, leakages.TotalLeaks, leakages.VLeaks, leakages.VReturns, leakages.ReturnQty, productsdata.ProductName, leakages.ProductID, leakages.FreeMilk,  leakages.ShortQty FROM  leakages INNER JOIN tripdata ON leakages.TripID = tripdata.Sno INNER JOIN productsdata ON leakages.ProductID = productsdata.sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2)) tripinfo) ff ON ff.Sno = Triproutes.Tripdata_sno");
                //cmd = new MySqlCommand("SELECT ff.I_date,Triproutes.Tripdata_sno, ff.Sno, ff.TotalLeaks, ff.FreeMilk, ff.ShortQty FROM (SELECT Tripdata_sno, RouteID, Sno FROM triproutes triproutes_1 WHERE (RouteID = @dispatchSno)) Triproutes INNER JOIN (SELECT Sno, TotalLeaks, VLeaks, VReturns, ReturnQty, FreeMilk, ShortQty,I_Date FROM (SELECT tripdata.Sno,tripdata.I_Date, leakages.TotalLeaks, leakages.VLeaks, leakages.VReturns, leakages.ReturnQty,  leakages.ProductID, leakages.FreeMilk,  leakages.ShortQty FROM  leakages INNER JOIN tripdata ON leakages.TripID = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2)) tripinfo) ff ON ff.Sno = Triproutes.Tripdata_sno");
                cmd.Parameters.AddWithValue("@dispatchSno", dr["sno"].ToString());
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                DataTable dtLeakble = vdm.SelectQuery(cmd).Tables[0];
                string TripDataSno = "0";
                if (dtLeakble.Rows.Count > 0)
                {
                    TripDataSno = dtLeakble.Rows[0]["Sno"].ToString();
                }
                else
                {
                    TripDataSno = "0";
                }
                dtShortAndFree.Merge(dtLeakble);
                cmd = new MySqlCommand("SELECT ShortQty,DATE_FORMAT(tripdata.I_date,'%d %b %y') as inddate,ProductID,LeakQty,ReturnQty,FreeMilk FROM leakages  inner join tripdata on tripdata.Sno=leakages.TripID  WHERE (tripdata.DEmpId = @DEmpId) AND (tripdata.I_Date BETWEEN @d1 AND @d2) and (leakages.VarifyStatus IS NULL)");
                cmd.Parameters.AddWithValue("@DEmpId", dtLeakble.Rows[0]["EmpId"].ToString());
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                DtTripId = vdm.SelectQuery(cmd).Tables[0];
            }

            dtShortAndFree.Merge(DtTripId);
            cmd = new MySqlCommand("SELECT branchleaktrans.BranchID as Branch_Id,DATE_FORMAT(tripdata.I_date,'%d %b %y') as inddate, branchleaktrans.ShortQty,branchleaktrans.FreeQty as FreeMilk  FROM branchleaktrans INNER JOIN tripdata ON branchleaktrans.TripId = tripdata.Sno WHERE (tripdata.I_Date BETWEEN @d1 AND @d2) AND (branchleaktrans.BranchID = @BranchID)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtsalesofficeshortfree = vdm.SelectQuery(cmd).Tables[0];
            dtShortAndFree.Merge(dtsalesofficeshortfree);

            cmd = new MySqlCommand("SELECT clotrans.BranchId,Sum(closubtranprodcts.StockQty) as ClosingStock, DATE_FORMAT(clotrans.IndDate,'%d %b %y') as inddate FROM clotrans INNER JOIN closubtranprodcts ON clotrans.Sno = closubtranprodcts.RefNo WHERE (clotrans.BranchId = @BranchID) AND (clotrans.IndDate BETWEEN @d1 AND @d2) AND (clotrans.Transaction_Type = 0) GROUP BY inddate");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtclosing_stock = vdm.SelectQuery(cmd).Tables[0];

            if (dtDispatchesbranches.Rows.Count > 0)
            {
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("Date");
                int count = 0;
                Report.Columns.Add("DC Qty").DataType = typeof(Double);
                Report.Columns.Add("OP Stock").DataType = typeof(Double); ; 
                Report.Columns.Add("Sales").DataType = typeof(Double); ;
                Report.Columns.Add("Sales Value").DataType = typeof(Double); ;
                Report.Columns.Add("Lekages").DataType = typeof(Double); ;
                Report.Columns.Add("Returns").DataType = typeof(Double); ;
                Report.Columns.Add("Short").DataType = typeof(Double); ;
                Report.Columns.Add("Free").DataType = typeof(Double); ;
                Report.Columns.Add("CL Stock").DataType = typeof(Double); ;
                int i = 1;
                DataView view = new DataView(dtDispatchesbranches);
                DataTable distincttable = view.ToTable(true, "I_date");
                string BRANCHID = ddlSalesOffice.SelectedValue;
                foreach (DataRow branch in distincttable.Rows)
                {
                    double totalleakreturn = 0;
                    double totalshortfree = 0;
                    DataRow newrow = Report.NewRow();
                    double shortqty = 0;
                    double freeqty = 0;
                    double leakqty = 0;
                    double saleqty = 0;
                    double salevalue = 0;
                    double opqty = 0;
                    double cloqty = 0;
                    double DispQty = 0;
                    double returnqty = 0;
                    double tfree = 0;
                    double tshort = 0;
                    DateTime Date = Convert.ToDateTime(branch["I_date"].ToString()).AddDays(1);
                    DateTime Date_2 = Convert.ToDateTime(branch["I_date"].ToString()).AddDays(-1);
                    string Date1 = Date.ToString("dd MMM yyyy");
                    string Date2 = Date_2.ToString("dd MMM yy");
                    newrow["Date"] = Date1;
                    foreach (DataRow drrdelivery in dtDispatchesbranches.Select("BranchID='" + BRANCHID + "'AND I_Date='" + branch["I_date"].ToString() + "'"))
                    {
                        double dcqty = 0;
                        double.TryParse(drrdelivery["dispatchqty"].ToString(), out dcqty);
                        DispQty += dcqty;
                        newrow["DC Qty"] = Math.Round(DispQty, 2);
                    }
                    foreach (DataRow drropening in dtOppening_stock.Select("BranchId='" + BRANCHID + "'AND inddate='" + Date2 + "'"))
                    {
                        double.TryParse(drropening["openingstock"].ToString(), out opqty);
                        newrow["Op Stock"] = Math.Round(opqty, 2); //drrdelivery["DeliveryQty"].ToString();
                    }
                    foreach (DataRow drrdelivery in dtAgent.Select("SuperBranch='" + BRANCHID + "'AND IndentDate='" + branch["I_date"].ToString() + "'"))
                    {
                        double.TryParse(drrdelivery["DeliveryQty"].ToString(), out saleqty);
                        newrow["Sales"] = Math.Round(saleqty, 2); //drrdelivery["DeliveryQty"].ToString();

                        double.TryParse(drrdelivery["salevalue"].ToString(), out salevalue);
                        newrow["Sales Value"] = Math.Round(salevalue, 2);
                    }
                    foreach (DataRow drleaks in DtLeksAndReturns.Select("BranchID='" + BRANCHID + "'AND AssignDate='" + branch["I_date"].ToString() + "'"))
                    {
                        double.TryParse(drleaks["totleaks"].ToString(), out leakqty);
                        double.TryParse(drleaks["returnqty"].ToString(), out returnqty);
                        newrow["Lekages"] = Math.Round(leakqty, 2); //drleaks["Leaks"].ToString();
                        newrow["Returns"] = Math.Round(returnqty, 2); //drleaks["Return"].ToString();
                        totalleakreturn += leakqty;
                        totalleakreturn += returnqty;
                    }
                    foreach (DataRow drfree in dtShortAndFree.Select("inddate='" + branch["I_date"].ToString() + "'"))
                    {
                        double.TryParse(drfree["ShortQty"].ToString(), out shortqty);
                        double.TryParse(drfree["FreeMilk"].ToString(), out freeqty);
                        //newrow["Free"] = Math.Round(freeqty, 0); //drfree["ShortQty"].ToString();
                        //newrow["Short"] = Math.Round(shortqty, 0);// drfree["qty"].ToString();
                        totalshortfree += freeqty;
                        totalshortfree += shortqty;
                        tfree += freeqty;
                        tshort += shortqty;
                    }
                    newrow["Free"] = Math.Round(tfree, 2); //drfree["ShortQty"].ToString();
                    newrow["Short"] = Math.Round(tshort, 2);
                    foreach (DataRow drrclosing in dtclosing_stock.Select("BranchId='" + BRANCHID + "'AND inddate='" + branch["I_date"].ToString() + "'"))
                    {
                        double.TryParse(drrclosing["ClosingStock"].ToString(), out cloqty);
                        newrow["CL Stock"] = Math.Round(cloqty, 2); //drrdelivery["DeliveryQty"].ToString();
                    }
                    //double mixedqy = saleqty + totalshortfree + totalleakreturn;
                    //double Total = DispQty - mixedqy;
                    //newrow["TotalClosing"] = Math.Round(Total, 0);
                    Report.Rows.Add(newrow);
                }
                i++;
                foreach (var column in Report.Columns.Cast<DataColumn>().ToArray())
                {
                    if (Report.AsEnumerable().All(dr => dr.IsNull(column)))
                        Report.Columns.Remove(column);
                }
                DataRow newvartical = Report.NewRow();
                newvartical["Date"] = "Total";
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
                foreach (DataColumn col in Report.Columns)
                {
                    string Pname = col.ToString();
                    string ProductName = col.ToString();
                    ProductName = GetSpace(ProductName);
                    Report.Columns[Pname].ColumnName = ProductName;
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
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
            int count = 0;
            foreach (TableCell cell in grdReports.HeaderRow.Cells)
            {
                if (count == 1)
                {
                    dt.Columns.Add(cell.Text);
                }
                else
                {
                    dt.Columns.Add(cell.Text);
                }
                count++;
            }
            foreach (GridViewRow row in grdReports.Rows)
            {
                dt.Rows.Add();
                for (int i = 1; i < row.Cells.Count; i++)
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
                string FileName = "Branch Wse Net Sale";
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