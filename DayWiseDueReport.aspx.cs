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

public partial class DayWiseDueReport : System.Web.UI.Page
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
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        pnlHide.Visible = true;
        DateTime fromdate = DateTime.Now;
        string[] datestrig = txtdate.Text.Split(' ');
        lblpreparedby.Text = "";
        if (datestrig.Length > 1)
        {
            if (datestrig[0].Split('-').Length > 0)
            {
                string[] dates = datestrig[0].Split('-');
                string[] times = datestrig[1].Split(':');
                fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
            }
        }
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
            Report = new DataTable();
            vdm = new VehicleDBMgr();
                lblDate.Text = txtdate.Text;
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
            Session["RouteName"] = "DAY WISE DUE REPORT " + fromdate.AddDays(1).ToString("dd/MM/yyyy");
            Session["filename"] =  "DAY WISE DUE REPORT " + fromdate.ToString("dd/MM/yyyy");
            cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName, branchdata.SalesType, branchdata.flag FROM branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno WHERE (branchmappingtable.SuperBranch = 172)");
            DataTable dtsalesoffice = vdm.SelectQuery(cmd).Tables[0];
            Report = new DataTable();
            Report.Columns.Add("Sno");
            //Report.Columns.Add("RouteCode");
            Report.Columns.Add("Route Name");
            Report.Columns.Add("Sale Value").DataType = typeof(Double);
            Report.Columns.Add("Received Amount").DataType = typeof(Double);
            Report.Columns.Add("Balance Amount").DataType = typeof(Double);
            Report.Columns.Add("Excess Amount").DataType = typeof(Double);
            Report.Columns.Add("Due Amount").DataType = typeof(Double);
            Report.Columns.Add("Remarks");
            foreach (DataRow dr in dtsalesoffice.Rows)
            {
                if (dr["sno"].ToString() == "527" || dr["sno"].ToString() == "554" || dr["sno"].ToString() == "760" || dr["sno"].ToString() == "925" || dr["sno"].ToString() == "1349")
                {
                    
                }
                else
                {
                    cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno,branchdata.CollectionType, branchroutes.Sno AS routesno, branchroutes.RouteName FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN branchroutes ON branchdata.sno = branchroutes.BranchID WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) AND (branchroutes.flag <> 0) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) AND (branchroutes.flag <> 0) ORDER BY branchdata.sno");
                    cmd.Parameters.AddWithValue("@SOID", dr["sno"].ToString());
                    cmd.Parameters.AddWithValue("@BranchID", dr["sno"].ToString());
                    DataTable dtroutes = vdm.SelectQuery(cmd).Tables[0];

                    cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno, triproutes.Tripdata_sno, tripdata.SubmittedAmount, tripdata.ReceivedAmount, dispatch.Route_id FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE (branchdata.sno = @BranchID) AND (dispatch.DispType IS NULL) AND (tripdata.I_Date BETWEEN @d1 AND @d2) OR (dispatch.DispType IS NULL) AND (branchdata_1.SalesOfficeID = @SOID) AND (tripdata.I_Date BETWEEN @d1 AND @d2)");
                    cmd.Parameters.AddWithValue("@SOID", dr["sno"].ToString());
                    cmd.Parameters.AddWithValue("@BranchID", dr["sno"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetLowDate(fromdate.AddDays(-1)));
                    DataTable dttripcollection = vdm.SelectQuery(cmd).Tables[0];

                    //cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, SUM(indents_subtable.DeliveryQty) AS saleQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue,modifiedroutes.Sno AS routesno, modifidroutssubtab.BranchID, branchdata_2.BranchName, branchdata_2.flag,SUM(branchdata_2.DueLimit) AS Duelimit FROM branchdata branchdata_2 RIGHT OUTER JOIN branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo ON branchdata_2.sno = modifidroutssubtab.BranchID LEFT OUTER JOIN indents_subtable INNER JOIN (SELECT IndentNo, I_date, Branch_id FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indt ON indents_subtable.IndentNo = indt.IndentNo ON modifidroutssubtab.BranchID = indt.Branch_id WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifiedroutes.Sno ORDER BY routesno");
                    cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, SUM(indents_subtable.DeliveryQty) AS saleQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue,modifiedroutes.Sno AS routesno, modifidroutssubtab.BranchID, branchdata_2.BranchName, branchdata_2.flag, SUM(branchdata_2.duelimit) AS Duelimit FROM branchdata branchdata_2 RIGHT OUTER JOIN branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo ON branchdata_2.sno = modifidroutssubtab.BranchID LEFT OUTER JOIN indents_subtable INNER JOIN (SELECT IndentNo, I_date, Branch_id FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indt ON indents_subtable.IndentNo = indt.IndentNo ON modifidroutssubtab.BranchID = indt.Branch_id WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) AND (branchdata_2.Due_Limit_Days = '0') OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) AND (branchdata_2.Due_Limit_Days = '0') GROUP BY modifiedroutes.Sno ORDER BY routesno");
                    cmd.Parameters.AddWithValue("@SOID", dr["sno"].ToString());
                    cmd.Parameters.AddWithValue("@BranchID", dr["sno"].ToString());
                    cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
                    DataTable dtroutecollection = vdm.SelectQuery(cmd).Tables[0];

                    cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, SUM(indents_subtable.DeliveryQty) AS saleQty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue,modifiedroutes.Sno AS routesno, modifidroutssubtab.BranchID, branchdata_2.BranchName, branchdata_2.flag, SUM(branchdata_2.duelimit) AS Duelimit FROM branchdata branchdata_2 RIGHT OUTER JOIN branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo ON branchdata_2.sno = modifidroutssubtab.BranchID LEFT OUTER JOIN indents_subtable INNER JOIN (SELECT IndentNo, I_date, Branch_id FROM indents WHERE (I_date BETWEEN @starttime AND @endtime)) indt ON indents_subtable.IndentNo = indt.IndentNo ON modifidroutssubtab.BranchID = indt.Branch_id WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) AND (branchdata_2.Due_Limit_Days <> '0') OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) AND (branchdata_2.Due_Limit_Days <> '0') GROUP BY branchdata_2.sno ORDER BY routesno");
                    cmd.Parameters.AddWithValue("@SOID", dr["sno"].ToString());
                    cmd.Parameters.AddWithValue("@BranchID", dr["sno"].ToString());
                    cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate.AddDays(-1)));
                    DataTable dtDue_routecollection = vdm.SelectQuery(cmd).Tables[0];

                    //cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routesno, SUM(colltion.AmountPaid) AS amtpaid, branchdata_1.SalesType, branchdata_2.CollectionType FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT  RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT Branchid, AmountPaid, PaidDate FROM collections WHERE (PaymentType <> 'Cheque') AND (PaidDate BETWEEN @d1 AND @d2)) colltion ON modifidroutssubtab.BranchID = colltion.Branchid INNER JOIN branchdata branchdata_2 ON modifidroutssubtab.BranchID = branchdata_2.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) GROUP BY modifiedroutes.Sno  ORDER BY modifiedroutes.Sno ");
                    cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routesno, SUM(colltion.AmountPaid) AS amtpaid, branchdata_1.SalesType, branchdata_2.CollectionType FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT Branchid, AmountPaid, PaidDate FROM collections WHERE (PaymentType <> 'Cheque') AND (PaidDate BETWEEN @d1 AND @d2)) colltion ON modifidroutssubtab.BranchID = colltion.Branchid INNER JOIN branchdata branchdata_2 ON modifidroutssubtab.BranchID = branchdata_2.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) AND (branchdata_2.Due_Limit_Days = '0') OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) AND (branchdata_2.Due_Limit_Days = '0') GROUP BY modifiedroutes.Sno ORDER BY routesno");
                    cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                    cmd.Parameters.AddWithValue("@SOID", dr["sno"].ToString());
                    cmd.Parameters.AddWithValue("@BranchID", dr["sno"].ToString());
                    DataTable dtrouteamount = vdm.SelectQuery(cmd).Tables[0];

                    cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, modifiedroutes.RouteName, modifidroutssubtab.BranchID, modifiedroutes.Sno AS routesno, SUM(colltion.AmountPaid) AS amtpaid, branchdata_1.SalesType, branchdata_2.CollectionType, branchdata_2.BranchName AS agentname FROM branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno INNER JOIN modifiedroutes ON branchdata.sno = modifiedroutes.BranchID INNER JOIN (SELECT RefNo, Rank, LevelType, BranchID, CDate, EDate FROM modifiedroutesubtable WHERE (EDate IS NULL) AND (CDate <= @starttime) OR (EDate > @starttime) AND (CDate <= @starttime)) modifidroutssubtab ON modifiedroutes.Sno = modifidroutssubtab.RefNo INNER JOIN (SELECT Branchid, AmountPaid, PaidDate FROM collections WHERE (PaymentType <> 'Cheque') AND (PaidDate BETWEEN @d1 AND @d2)) colltion ON modifidroutssubtab.BranchID = colltion.Branchid INNER JOIN branchdata branchdata_2 ON modifidroutssubtab.BranchID = branchdata_2.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) AND (branchdata_2.Due_Limit_Days <> '0') OR (branchdata.SalesType IS NOT NULL) AND (branchdata.sno = @BranchID) AND (branchdata_2.Due_Limit_Days <> '0') GROUP BY branchdata_2.sno ORDER BY routesno");
                    cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate));
                    cmd.Parameters.AddWithValue("@SOID", dr["sno"].ToString());
                    cmd.Parameters.AddWithValue("@BranchID", dr["sno"].ToString());
                    DataTable dtDue_routeamount = vdm.SelectQuery(cmd).Tables[0];

                    double totalsaleqty = 0;
                    double totalsalevalue = 0;
                    double totalamountpaid = 0;
                    double totalbalanceamt = 0;
                    double totalexcessamount = 0;
                    double totaldueamount = 0;
                    int sno = 1;
                    DataRow newrowso = Report.NewRow();
                    newrowso["Route Name"] = dr["BranchName"].ToString();
                    Report.Rows.Add(newrowso);
                    
                    DataRow break1 = Report.NewRow();
                    break1["Route Name"] = "";
                    Report.Rows.Add(break1);

                    

                    foreach (DataRow drroutes in dtroutes.Rows)
                    {


                        foreach (DataRow drsale in dtroutecollection.Select("routesno='" + drroutes["routesno"].ToString() + "'"))
                        {
                            DataRow newrow1 = Report.NewRow();

                            newrow1["Sno"] = sno++;
                            //newrow1["RouteCode"] = drsale["routesno"].ToString();
                            //newrow1["Route Code"] = drsale["routesno"].ToString();
                            newrow1["Route Name"] = drsale["RouteName"].ToString();
                            double saleqty = 0;
                            double salevalue = 0;
                            double amountpaid = 0;
                            double chequeamountpaid = 0;
                            
                            
                            //double.TryParse(drsale["saleQty"].ToString(), out saleqty);
                            double.TryParse(drsale["salevalue"].ToString(), out salevalue);
                            //newrow1["Sale Qty"] = Math.Round(saleqty, 2);
                            newrow1["Sale Value"] = Math.Round(salevalue, 2);
                            totalsaleqty += Math.Round(saleqty, 2);
                            totalsalevalue += Math.Round(salevalue, 2);
                            foreach (DataRow drcoll in dtrouteamount.Select("routesno='" + drsale["routesno"].ToString() + "'"))
                            {
                                double.TryParse(drcoll["amtpaid"].ToString(), out amountpaid);
                                totalamountpaid += Math.Round(amountpaid, 2);

                            }
                            //foreach (DataRow drChequecoll in dtrouteChequeamount.Select("routesno='" + drsale["routesno"].ToString() + "'"))
                            //{
                            //    double.TryParse(drChequecoll["amtpaid"].ToString(), out chequeamountpaid);
                            //    totalamountpaid += Math.Round(chequeamountpaid, 2);
                            //    //collectiontype = drChequecoll["CollectionType"].ToString();


                            //}
                            
                            
                            double totamt = 0;
                            double balamount = 0;
                            double excessamount = 0;
                            double dueamount = 0;
                            double tripamt = 0;
                            totamt = amountpaid + chequeamountpaid;
                            newrow1["Received Amount"] = Math.Round(totamt, 2);
                            foreach (DataRow drtrip in dtroutecollection.Select("Route_id='" + drroutes["routesno"].ToString() + "'"))
                            {

                                double.TryParse(drtrip["ReceivedAmount"].ToString(), out tripamt);
                            }
                           
                            balamount = salevalue - totamt;
                            excessamount = totamt - salevalue;
                            if (excessamount < 0)
                            {
                                excessamount = 0;
                            }
                            newrow1["Balance Amount"] = Math.Round(balamount, 2);
                            totalbalanceamt += balamount;
                            totalexcessamount += excessamount;
                            totaldueamount += dueamount;
                            newrow1["Excess Amount"] = Math.Round(excessamount, 2);
                            newrow1["Due Amount"] = dueamount;
                            Report.Rows.Add(newrow1);

                        }


                    }
                    foreach (DataRow drduesale in dtDue_routecollection.Rows)
                    {
                        DataRow duerow1 = Report.NewRow();

                        duerow1["Sno"] = sno++;
                        duerow1["Route Name"] = drduesale["BranchName"].ToString();
                        double saleqty = 0;
                        double salevalue = 0;
                        double amountpaid = 0;
                        double chequeamountpaid = 0;
                        double.TryParse(drduesale["salevalue"].ToString(), out salevalue);
                        duerow1["Sale Value"] = Math.Round(salevalue, 2);
                        totalsaleqty += Math.Round(saleqty, 2);
                        totalsalevalue += Math.Round(salevalue, 2);
                        foreach (DataRow drcoll in dtDue_routeamount.Select("BranchID='" + drduesale["BranchID"].ToString() + "'"))
                        {
                            double.TryParse(drcoll["amtpaid"].ToString(), out amountpaid);
                            totalamountpaid += Math.Round(amountpaid, 2);

                        }
                        //foreach (DataRow drChequecoll in dtrouteChequeamount.Select("routesno='" + drsale["routesno"].ToString() + "'"))
                        //{
                        //    double.TryParse(drChequecoll["amtpaid"].ToString(), out chequeamountpaid);
                        //    totalamountpaid += Math.Round(chequeamountpaid, 2);
                        //    //collectiontype = drChequecoll["CollectionType"].ToString();


                        //}


                        double totamt = 0;
                        totamt = amountpaid + chequeamountpaid;
                        duerow1["Received Amount"] = Math.Round(totamt, 2);
                        double balamount = 0;
                        double excessamount = 0;
                        double dueamount = 0;
                        balamount = salevalue - totamt;
                        excessamount = totamt - salevalue;
                        if (excessamount < 0)
                        {
                            excessamount = 0;
                        }
                        duerow1["Balance Amount"] = Math.Round(balamount, 2);
                        totalbalanceamt += balamount;
                        totalexcessamount += excessamount;
                        totaldueamount += dueamount;
                        duerow1["Excess Amount"] = Math.Round(excessamount, 2);
                        duerow1["Due Amount"] = dueamount;
                        Report.Rows.Add(duerow1);
                    }
                    



                    DataRow TotRow = Report.NewRow();
                    TotRow["Route Name"] = "Total";
                    TotRow["Sale Value"] = totalsalevalue;
                    TotRow["Received Amount"] = totalamountpaid;
                    TotRow["Balance Amount"] = Math.Round(totalbalanceamt, 2);
                    TotRow["Excess Amount"] = Math.Round(totalexcessamount, 2);
                    TotRow["Due Amount"] = totaldueamount;
                    Report.Rows.Add(TotRow);
                    DataRow break2 = Report.NewRow();
                    break2["Route Name"] = "";
                    Report.Rows.Add(break2);
                    DataRow break3 = Report.NewRow();
                    break3["Route Name"] = "";
                    Report.Rows.Add(break3);

                }
            }
            grdReports.DataSource = Report;
            grdReports.DataBind();

        }
        catch (Exception ex)
        {

        }
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
                    dt.Columns.Add(cell.Text).DataType = typeof(double);
                }
                count++;
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