﻿using System;
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
public partial class NewAgentStatement : System.Web.UI.Page
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
      //  Button1.Visible = false;
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) and (branchdata.flag<>0) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) and (branchdata.flag<>0)");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                cmd.Parameters.AddWithValue("@SalesType", "4");
                cmd.Parameters.AddWithValue("@SalesType1", "26");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtRoutedata.Rows)
                {
                    DataRow newrow = dtBranch.NewRow();
                    newrow["BranchName"] = dr["BranchName"].ToString();
                    newrow["sno"] = dr["sno"].ToString();
                    dtBranch.Rows.Add(newrow);
                }
                cmd = new MySqlCommand("SELECT BranchName, sno FROM  branchdata WHERE (sno = @BranchID) and (flag<>0)");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow dr in dtPlant.Rows)
                {
                    DataRow newrow = dtBranch.NewRow();
                    newrow["BranchName"] = dr["BranchName"].ToString();
                    newrow["sno"] = dr["sno"].ToString();
                    dtBranch.Rows.Add(newrow);
                }
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) and (branchdata.flag<>0)");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                cmd.Parameters.AddWithValue("@SalesType", "6");
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
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                PBranch.Visible = false;
                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (dispatch.flag = 1) AND (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
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
        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE   (dispatch.flag = 1) AND (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
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
        cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutes ON dispatch_sub.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (dispatch.sno = @dispsno) AND  (dispatch.flag = 1)");
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
                    string[] dates = todatestrig[0].Split('-');
                    string[] times = todatestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            Session["filename"] = "Statement of Account ->" + ddlAgentName.SelectedItem.Text;
            lblAgent.Text = ddlAgentName.SelectedItem.Text;
            lbl_fromDate.Text = txtFromdate.Text;
            lbl_selttodate.Text = txtTodate.Text;
            cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost),2) AS Totalsalevalue,ROUND(SUM(indents_subtable.DeliveryQty),2) AS DeliveryQty,products_category.Categoryname, productsdata.ProductName, DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate FROM productsdata INNER JOIN indents_subtable ON productsdata.sno = indents_subtable.Product_sno INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchdata.sno = @BranchID) and (indents_subtable.DeliveryQty<>'0') GROUP BY productsdata.sno, IndentDate ORDER BY products_category.sno,indents.I_date");
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtAgent = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT sum(AmountPaid) as AmountPaid,Branchid, PaymentType,DATE_FORMAT(PaidDate, '%d/%b/%y') AS PDate FROM collections WHERE Branchid=@Branchid and  PaidDate between @d1 and @d2  GROUP BY PDate, PaymentType");
            cmd.Parameters.AddWithValue("@Branchid", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable dtcollections = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT opp_balance,salesvalue,paidamount,clo_balance,DATE_FORMAT(inddate, '%d %b %y') AS PDate,agentid FROM agent_bal_trans WHERE agentid=@agentid and inddate between @d1 and @d2 GROUP BY PDate order by inddate");
            cmd.Parameters.AddWithValue("@agentid", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            DataTable dtagenttrans = vdm.SelectQuery(cmd).Tables[0];
            if (dtAgent.Rows.Count > 0)
            {
                DataView view = new DataView(dtAgent);
                DataTable produtstbl = view.ToTable(true, "ProductName", "Categoryname");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("DeliverDate");
                int count = 0;
                foreach (DataRow dr in produtstbl.Rows)
                {

                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                    count++;
                }
                Report.Columns.Add("Total", typeof(Double)).SetOrdinal(count + 2);
                Report.Columns.Add("Sale Value", typeof(Double)).SetOrdinal(count + 3);
                Report.Columns.Add("Amount Debited", typeof(Double)).SetOrdinal(count + 4);
                Report.Columns.Add("Opp Bal").SetOrdinal(count + 5);
                Report.Columns.Add("Total Amount").SetOrdinal(count + 6);
                Report.Columns.Add("Paid Amount", typeof(Double)).SetOrdinal(count + 7);
                Report.Columns.Add("Bank Transfer", typeof(Double)).SetOrdinal(count + 8);
                Report.Columns.Add("Phone/Google Pay", typeof(Double)).SetOrdinal(count + 9);
                Report.Columns.Add("Incentive/JV", typeof(Double)).SetOrdinal(count + 10);
                Report.Columns.Add("Bal Amount").SetOrdinal(count + 11);
                DataTable distincttable = view.ToTable(true, "IndentDate");
                int i = 1;
                double oppcarry = 0;
                TimeSpan dateSpan = todate.Subtract(fromdate);
                int NoOfdays = dateSpan.Days;
                NoOfdays = NoOfdays + 1;
                double totdebitedamount = 0;
                DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);

                double totsale = 0;
                double totamt = 0;
                //int j = 0;

                double ftotaloppbal = 0;
                double ftotalClosingbal = 0;
                double ftotalsalesvalue = 0;
                double ftotalpaidamount = 0;
                double ftotaljv = 0;
                double ftotalPhonePay = 0;

                double grand_totaloppbal = 0;
                double grand_totalClosingbal = 0;
                double grand_totalsalesvalue = 0;
                double grand_totalpaidamount = 0;
                double grand_totalbankTransfer = 0;
                double grand_totaljv = 0;
                double grand_totalPhonepay = 0;
                DataView view1 = new DataView(dtAgent);
                DataTable distinct_Date = view1.ToTable(true, "IndentDate");
                for (int j = 0; j < NoOfdays; j++)
                {
                    //foreach (DataRow dr in distinct_Date.Rows)
                    //{
                    double ftotalbankTransfer = 0;

                    DataRow newrow = Report.NewRow();

                    string dtcount = fromdate.AddDays(j).ToString();
                    DateTime dtDOE = Convert.ToDateTime(dtcount);

                    //DateTime d1 = Convert.ToDateTime(dr["IndentDate"].ToString()).AddDays(1);
                    //string dtcount = d1.ToString();
                    //DateTime dtDOE = Convert.ToDateTime(dtcount);
                    string dtdate1 = dtDOE.AddDays(-1).ToString();
                    DateTime dtDOE1 = Convert.ToDateTime(dtdate1).AddDays(1);
                    string ChangedTime1 = dtDOE1.ToString("dd/MMM/yy");
                    string ChangedTime2 = dtDOE.AddDays(-1).ToString("dd MMM yy");
                    newrow["DeliverDate"] = ChangedTime1;

                    double total_qty = 0;
                    double Amount = 0;
                    foreach (DataRow dragent in dtAgent.Select("IndentDate='" + ChangedTime2 + "'"))
                    {
                        //if (ChangedTime2 == dr["IndentDate"].ToString())
                        //{
                        double qtyvalue = 0;
                        double DQty = 0;
                        double.TryParse(dragent["DeliveryQty"].ToString(), out DQty);
                        newrow[dragent["ProductName"].ToString()] = DQty;
                        double.TryParse(dragent["Totalsalevalue"].ToString(), out qtyvalue);
                        Amount += qtyvalue;
                        total_qty += DQty;
                        //}
                    }
                    newrow["Total"] = total_qty;
                    newrow["Sale Value"] = Amount;
                    double banktransfervalue = 0;
                    double Phonepayvalue = 0;
                    double jvvalue = 0;
                    double salesvalue = 0;
                    double paidamount = 0;

                    foreach (DataRow drcollections in dtcollections.Select("PDate='" + ChangedTime1 + "'"))
                    {
                        string PaymentType = drcollections["PaymentType"].ToString();

                        //if (PaymentType == "Cheque")
                        //{
                        //    double banktransfervalue = 0;
                        //    double.TryParse(drcollections["AmountPaid"].ToString(), out banktransfervalue);
                        //    //newrow["Bank Transfer"] = banktransfervalue;
                        //    ftotalbankTransfer += banktransfervalue;
                        //    grand_totalbankTransfer += banktransfervalue;
                        //}
                        if (PaymentType == "Bank Transfer" || PaymentType == "Cheque" )
                        {
                            //double banktransfervalue = 0;
                            double.TryParse(drcollections["AmountPaid"].ToString(), out banktransfervalue);
                            newrow["Bank Transfer"] = banktransfervalue;
                            ftotalbankTransfer += banktransfervalue;
                            grand_totalbankTransfer += banktransfervalue;
                        }
                        if (PaymentType == "PhonePay")
                        {
                            //double Phonepayvalue = 0;
                            double.TryParse(drcollections["AmountPaid"].ToString(), out Phonepayvalue);
                            newrow["Phone/Google Pay"] = Phonepayvalue;
                            ftotalPhonePay += Phonepayvalue;
                            grand_totalPhonepay += Phonepayvalue;
                        }
                        if (PaymentType == "Journal Voucher" || PaymentType == "Incentive")
                        {
                            //double jvvalue = 0;
                            double.TryParse(drcollections["AmountPaid"].ToString(), out jvvalue);
                            newrow["Incentive/JV"] = jvvalue;
                            ftotaljv += jvvalue;
                            grand_totaljv += jvvalue;
                        }
                    }
                    foreach (DataRow drtrans in dtagenttrans.Select("PDate='" + ChangedTime2 + "'"))
                    {
                        //double salesvalue = 0;
                        double.TryParse(drtrans["salesvalue"].ToString(), out salesvalue);

                        ftotalsalesvalue += salesvalue;
                        grand_totalsalesvalue += salesvalue;
                        //double paidamount = 0;  
                        double.TryParse(drtrans["paidamount"].ToString(), out paidamount);
                        paidamount = paidamount - ftotalbankTransfer;
                        newrow["Paid Amount"] = Math.Round(paidamount); ;
                        ftotalpaidamount += paidamount;
                        grand_totalpaidamount += paidamount;
                        double oppvalue = 0;
                        double.TryParse(drtrans["opp_balance"].ToString(), out oppvalue);
                        newrow["Opp Bal"] = Math.Round(oppvalue);

                        double TotalAmount = Amount + oppvalue;
                        newrow["Total Amount"] = Math.Round(TotalAmount);

                        ftotaloppbal += oppvalue;
                        grand_totaloppbal += oppvalue;
                        double closvalue = 0;
                        double.TryParse(drtrans["clo_balance"].ToString(), out closvalue);
                        newrow["Bal Amount"] = Math.Round(closvalue);
                        ftotalClosingbal += closvalue;
                        grand_totalClosingbal += closvalue;
                    }
                    if (paidamount + salesvalue + banktransfervalue + Phonepayvalue + jvvalue != 0)
                    {
                        Report.Rows.Add(newrow);
                    }

                }
                DataRow newvartical = Report.NewRow();
                newvartical["DeliverDate"] = "Total";
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
                GridView grd = grdReports;
                grd.DataSource = Report;
                grd.DataBind();
                Session["xportdata"] = Report;
            }
            else
            {
                pnlHide.Visible = false;
                lblmsg.Text = "No data were found";
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
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            vdm = new VehicleDBMgr();
            DataTable dtbalance = (DataTable)Session["xportdata"];
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            foreach (DataRow dr in dtbalance.Rows)
            {
                double openingbalance = 0;
                double.TryParse(dr["Opp Bal "].ToString(), out openingbalance);
                double paidamout = 0;
                double.TryParse(dr["Paid Amount "].ToString(), out paidamout);
                double ince_Jv_amount = 0;
                double.TryParse(dr["Incentive/JV "].ToString(), out ince_Jv_amount);
                double closing = 0;
                double.TryParse(dr["Bal Amount "].ToString(), out closing);
                double totalpaidamount = paidamout + ince_Jv_amount;
                //double closingamount = openingbalance - totalpaidamount;
                if (dr["DeliverDate "].ToString() != "Total")
                {
                    DateTime Date = Convert.ToDateTime(dr["DeliverDate "].ToString()).AddDays(-1);
                    string inddate = Date.ToString("yyyy-MM-dd");
                    cmd = new MySqlCommand("update agent_bal_trans  set opp_balance=@opp_balance,salesvalue=@salesvalue,paidamount=@paidamount,clo_balance=@clo_balance  where agentid=@agentid and inddate=@inddate");
                    cmd.Parameters.AddWithValue("@opp_balance", openingbalance);
                    cmd.Parameters.AddWithValue("@salesvalue", dr["Sale Value "].ToString());
                    cmd.Parameters.AddWithValue("@paidamount", totalpaidamount);
                    cmd.Parameters.AddWithValue("@clo_balance", closing);
                    cmd.Parameters.AddWithValue("@agentid", ddlAgentName.SelectedValue);
                    cmd.Parameters.AddWithValue("@inddate", inddate);
                    if (vdm.Update(cmd) == 0)
                    {
                        cmd = new MySqlCommand("insert into agent_bal_trans(agentid,opp_balance,inddate,salesvalue,paidamount,clo_balance,createdate,entryby)values(@agentid,@opp_balance,@inddate,@salesvalue,@paidamount,@clo_balance,@createdate,@entryby)");
                        cmd.Parameters.AddWithValue("@agentid", ddlAgentName.SelectedValue);
                        cmd.Parameters.AddWithValue("@opp_balance", openingbalance);
                        cmd.Parameters.AddWithValue("@inddate", inddate);
                        cmd.Parameters.AddWithValue("@salesvalue", dr["Sale Value "].ToString());
                        cmd.Parameters.AddWithValue("@paidamount", totalpaidamount);
                        cmd.Parameters.AddWithValue("@clo_balance", closing);
                        cmd.Parameters.AddWithValue("@createdate", ServerDateCurrentdate);
                        cmd.Parameters.AddWithValue("@entryby", "1000");
                        vdm.insert(cmd);
                    }
                }
                else
                {
                    string empty = "";
                }
            }
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