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


public partial class AgentWiseInventory : System.Web.UI.Page
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
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                //lblDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                FillAgentName();
                lblTitle.Text = Session["TitleName"].ToString();
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType)");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                cmd.Parameters.AddWithValue("@SalesType", "21");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                PBranch.Visible = false;
                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
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
        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlDispName.DataSource = dtRoutedata;
        ddlDispName.DataTextField = "DispName";
        ddlDispName.DataValueField = "sno";
        ddlDispName.DataBind();
        ddlDispName.Items.Insert(0, new ListItem("Select", "0"));
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
    DataTable NewReport = new DataTable();
    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            pnlHide.Visible = true;
            DataTable Report = new DataTable();
            DateTime fromdate = DateTime.Now;
            NewReport = new DataTable();
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
            Session["filename"] = ddlAgentName.SelectedItem.Text + "-> INVENTORY TRANSACTION";
            lblAgent.Text = ddlAgentName.SelectedItem.Text; ;
            cmd = new MySqlCommand("SELECT inventory_monitor.Inv_Sno, inventory_monitor.BranchId, inventory_monitor.Qty, inventory_monitor.Sno, inventory_monitor.EmpId, inventory_monitor.lostQty FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutesubtable ON dispatch_sub.Route_id = modifiedroutesubtable.RefNo INNER JOIN inventory_monitor ON modifiedroutesubtable.BranchID = inventory_monitor.BranchId WHERE (inventory_monitor.BranchId = @BranchId) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) AND (inventory_monitor.Qty>0)");
            cmd.Parameters.AddWithValue("@BranchId", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate).AddDays(-2));
            DataTable dtinventoryopp = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT invtras.TransType, invtras.FromTran, invtras.ToTran, invtras.Qty,DATE_FORMAT(invtras.DOE, '%d/%b/%y') AS PDate, invmaster.sno AS invsno, invmaster.InvName FROM  invtransactions12 as invtras INNER JOIN invmaster ON invtras.B_inv_sno = invmaster.sno WHERE ToTran=@FromTran and invtras.DOE between @d1 and @d2  ORDER BY invtras.DOE");
            cmd.Parameters.AddWithValue("@FromTran", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable dtinventaryissued = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT invtras.TransType, invtras.FromTran, invtras.ToTran, invtras.Qty, DATE_FORMAT(invtras.DOE, '%d/%b/%y') AS PDate, invmaster.sno AS invsno, invmaster.InvName FROM  invtransactions12 as invtras INNER JOIN invmaster ON invtras.B_inv_sno = invmaster.sno WHERE FromTran=@FromTran and invtras.DOE between @d1 and @d2 ORDER BY invtras.DOE");
            cmd.Parameters.AddWithValue("@FromTran", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable dtinventaryreceived = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("select Inv_Sno,Qty,BranchId from inventory_monitor where BranchId=@BranchId;");
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            DataTable dtAgent_presentopp = vdm.SelectQuery(cmd).Tables[0];
            double agentcratespresentopp = 0;
            if (dtAgent_presentopp.Rows[0]["Inv_Sno"].ToString() == "1")
            {
                double.TryParse(dtAgent_presentopp.Rows[0]["Qty"].ToString(), out agentcratespresentopp);
                agentcratespresentopp = agentcratespresentopp;
            }
            dtinventaryissued.Merge(dtinventaryreceived);
            cmd = new MySqlCommand("SELECT SUM(invtras.Qty) as totalissue  FROM  invtransactions12 as invtras INNER JOIN invmaster ON invtras.B_inv_sno = invmaster.sno WHERE invtras.ToTran=@ToTran and invtras.DOE between @d1 and @d2  group by invtras.ToTran");
            cmd.Parameters.AddWithValue("@ToTran", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@TransType", "2");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable dttotalissueinventary = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT SUM(invtras.Qty) as totalreceive FROM  invtransactions12 as invtras INNER JOIN invmaster ON invtras.B_inv_sno = invmaster.sno WHERE invtras.FromTran=@ToTran and invtras.DOE between @d1 and @d2  group by invtras.FromTran");
            cmd.Parameters.AddWithValue("@ToTran", ddlAgentName.SelectedValue);
            cmd.Parameters.AddWithValue("@TransType", "3");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable dttotalreceivedinventary = vdm.SelectQuery(cmd).Tables[0];

            if (dtinventaryissued.Rows.Count > 0)
            {
                DataView view = new DataView(dtinventaryissued);
                DataTable produtstbl = view.ToTable(true, "InvName");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("DeliverDate");
                int count = 0;
                Report.Columns.Add("Opp Crates", typeof(Double));
                //foreach (DataRow dr in produtstbl.Rows)
                //{

                //    Report.Columns.Add(dr["InvName"].ToString()).DataType = typeof(Double);
                //    count++;
                //}
                Report.Columns.Add("Issued Crates", typeof(Double));
                Report.Columns.Add("TotalCrates", typeof(Double));
                Report.Columns.Add("Received Crates", typeof(Double));
                Report.Columns.Add("CB Crates", typeof(Double));

                int i = 1;
                double oppcarry = 0;
                TimeSpan dateSpan = todate.Subtract(fromdate);
                int NoOfdays = dateSpan.Days;
                NoOfdays = NoOfdays + 1;
                double totdebitedamount = 0;
                DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
                double totsale = 0;
                double totamt = 0;
                double totreceivedqty = 0;
                double totalissueqty = 0;
                for (int j = 0; j < NoOfdays; j++)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    string dtcount = fromdate.AddDays(j).ToString();
                    DateTime dtDOE = Convert.ToDateTime(dtcount);
                    //string dtdate1 = branch["IndentDate"].ToString();
                    string dtdate1 = dtDOE.AddDays(-1).ToString();
                    DateTime dtDOE1 = Convert.ToDateTime(dtdate1).AddDays(1);
                    string ChangedTime1 = dtDOE1.ToString("dd/MMM/yy");
                    string ChangedTime2 = dtDOE.AddDays(-1).ToString("dd MMM yy");
                    newrow["DeliverDate"] = ChangedTime1;
                    double Ctotreccrates = 0;
                    double Dtotissuecrates = 0;
                    double oppcrates = 0;
                    foreach (DataRow dr in dtinventaryissued.Select("PDate='" + ChangedTime1 + "'"))
                    {
                        if (dr["TransType"].ToString() == "2")
                        {
                            if (dr["invsno"].ToString() == "1")
                            {
                                double Dcrates = 0;
                                double.TryParse(dr["Qty"].ToString(), out Dcrates);
                                //newrow[dr["InvName"].ToString()] = Dcrates;
                                Dtotissuecrates += Dcrates;
                                totalissueqty += Dcrates;
                            }
                        }
                    }
                    foreach (DataRow drr in dtinventaryissued.Select("PDate='" + ChangedTime1 + "'"))
                    {
                        if (drr["TransType"].ToString() == "1" || drr["TransType"].ToString() == "3")
                        {
                            if (drr["invsno"].ToString() == "1")
                            {
                                int Ccrates = 0;
                                int.TryParse(drr["Qty"].ToString(), out Ccrates);
                                //newrow[drr["InvName"].ToString()] = Ccrates;
                                Ctotreccrates += Ccrates;
                                totreceivedqty += Ccrates;
                            }
                        }
                    }
                    if (dttotalissueinventary.Rows.Count > 0)
                    {
                        double.TryParse(dttotalissueinventary.Rows[0]["totalissue"].ToString(), out totsale);
                    }
                    if (dttotalreceivedinventary.Rows.Count > 0)
                    {
                        double.TryParse(dttotalreceivedinventary.Rows[0]["totalreceive"].ToString(), out totamt);
                    }
                    else
                    {
                        totsale = 0;
                        totamt = 0;
                    }
                    double total = 0;
                    double Amount = 0;
                    double aopp = agentcratespresentopp + totamt - totsale;
                    double actbal = 0;
                    actbal = aopp;
                    if (totreceivedqty == 0.0)
                    {
                        if (oppcarry == 0.0)
                        {
                            aopp = aopp;
                        }
                        else
                        {
                            aopp = oppcarry;
                        }
                    }
                    else
                    {
                        if (Ctotreccrates != 0.0)
                        {
                            if (oppcarry == 0.0)
                            {
                                aopp = aopp;
                            }
                            else
                            {
                                aopp = oppcarry;
                            }
                        }
                        else
                        {
                            aopp = Math.Abs(aopp);
                            aopp = totdebitedamount - aopp;
                            aopp = oppcarry;
                        }
                    }
                    if (totsale == 0)
                    {
                        aopp = oppcarry;
                    }
                    //newrow["Total"] = total;
                    newrow["Issued Crates"] = Math.Round(Dtotissuecrates);

                    newrow["Opp Crates"] = Math.Round(aopp);
                    double totalcrates = aopp + Dtotissuecrates;
                    newrow["TotalCrates"] = Math.Round(totalcrates);
                    //newrow["Paid Amount"] = amtpaid - incentiveamtpaid;
                    newrow["Received Crates"] = Ctotreccrates;
                    double tot_amount = Ctotreccrates;
                    double totalbalance = totalcrates - tot_amount;
                    newrow["CB Crates"] = Math.Round(totalbalance);
                    oppcarry = totalbalance;
                    //if (Amount + amtpaid + debitedamount != 0)
                    //{
                    Report.Rows.Add(newrow);
                    i++;
                    //}
                    totsale = totsale - Amount;
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
                //foreach (DataColumn col in Report.Columns)
                //{
                //    string Pname = col.ToString();
                //    string ProductName = col.ToString();
                //    ProductName = GetSpace(ProductName);
                //    Report.Columns[Pname].ColumnName = ProductName;
                //}
                GridView grd = grdReports;
                grd.DataSource = Report;
                grd.DataBind();
                Session["xportdata"] = Report;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
            grdReports.DataSource = NewReport;
            grdReports.DataBind();
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
    protected void ddlDispName_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch)");
        cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutes ON dispatch_sub.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (dispatch.sno = @dispsno)");
        cmd.Parameters.AddWithValue("@dispsno", ddlDispName.SelectedValue);
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlAgentName.DataSource = dtRoutedata;
        ddlAgentName.DataTextField = "BranchName";
        ddlAgentName.DataValueField = "sno";
        ddlAgentName.DataBind();
    }
}