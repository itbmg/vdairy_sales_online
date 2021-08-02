using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;

public partial class Invoice_dashboard : System.Web.UI.Page
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
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txt_todate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
                FillRouteName();
            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    void FillRouteName()
    {
        vdm = new VehicleDBMgr();
        PBranch.Visible = true;
        DataTable dtBranch = new DataTable();
        dtBranch.Columns.Add("BranchName");
        dtBranch.Columns.Add("sno");
        cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
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
        cmd = new MySqlCommand("SELECT BranchName, sno FROM  branchdata WHERE (sno = @BranchID)");
        cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
        DataTable dtPlant = vdm.SelectQuery(cmd).Tables[0];
        foreach (DataRow dr in dtPlant.Rows)
        {
            DataRow newrow = dtBranch.NewRow();
            newrow["BranchName"] = dr["BranchName"].ToString();
            newrow["sno"] = dr["sno"].ToString();
            dtBranch.Rows.Add(newrow);
        }
        cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType)  ");
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
        ddlstateName.DataSource = dtBranch;
        ddlstateName.DataTextField = "BranchName";
        ddlstateName.DataValueField = "sno";
        ddlstateName.DataBind();
        //cmd = new MySqlCommand("SELECT  statename, statecode, ecode, gststatecode FROM statemastar");
        //DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        //ddlstateName.DataSource = dtRoutedata;
        //ddlstateName.DataTextField = "statename";
        //ddlstateName.DataValueField = "gststatecode";
        //ddlstateName.DataBind();
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
    DataTable dtReport = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            Session["RouteName"] = ddlstateName.SelectedItem.Text;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
           
            SqlCommand cm_d;
            DateTime fromdate = DateTime.Now;
            string[] dateFromstrig = txtFromdate.Text.Split(' ');
            if (dateFromstrig.Length > 1)
            {
                if (dateFromstrig[0].Split('-').Length > 0)
                {
                    string[] dates = dateFromstrig[0].Split('-');
                    string[] times = dateFromstrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            DateTime todate = DateTime.Now;
            string[] todatestrig = txt_todate.Text.Split(' ');
            if (todatestrig.Length > 1)
            {
                if (todatestrig[0].Split('-').Length > 0)
                {
                    string[] dates = todatestrig[0].Split('-');
                    string[] times = todatestrig[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            DataTable Report = new DataTable();
            Report.Columns.Add("DOE");
            Report.Columns.Add("Invoice No");
            Report.Columns.Add("State");
            Report.Columns.Add("Company");
            Report.Columns.Add("Module");
            Report.Columns.Add("BranchName");
            lbl_selfromdate.Text = fromdate.ToString("dd/MM/yyyy");
            DataTable dtAgent = new DataTable();
            Session["filename"] = ddlstateName.SelectedItem.Text + " Invoice DashBoard" + fromdate.ToString("dd/MM/yyyy");
            string agentdcNo = TextBox1.Text;
            if (agentdcNo != "")
            {
                if (ddlcompny.SelectedValue == "ALL")
                {
                    if (ddlinvoicetype.SelectedValue == "NonTax")
                    {
                        cmd = new MySqlCommand("SELECT  DcNo, BranchID, IndDate, agentdcno, soid, stateid, companycode, moduleid, doe FROM  agentdc where  (agentdcno=@agentdcno)");
                        cmd.Parameters.AddWithValue("@agentdcno", agentdcNo);
                        dtAgent = vdm.SelectQuery(cmd).Tables[0];
                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT  DcNo, BranchID, IndDate, agentdcno, soid, stateid, companycode, moduleid, doe FROM  agenttaxdc where  (agentdcno=@agentdcno)");
                        cmd.Parameters.AddWithValue("@agentdcno", agentdcNo);
                        dtAgent = vdm.SelectQuery(cmd).Tables[0];
                    }
                }
                else
                {
                    if (ddlinvoicetype.SelectedValue == "NonTax")
                    {
                        cmd = new MySqlCommand("SELECT  DcNo, BranchID, IndDate, agentdcno, soid, stateid, companycode, moduleid, doe FROM  agentdc where (companycode=@companycode) AND (agentdcno=@agentdcno)");
                        cmd.Parameters.AddWithValue("@agentdcno", agentdcNo);
                        cmd.Parameters.AddWithValue("@companycode", ddlcompny.SelectedValue);
                        dtAgent = vdm.SelectQuery(cmd).Tables[0];
                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT  DcNo, BranchID, IndDate, agentdcno, soid, stateid, companycode, moduleid, doe FROM  agenttaxdc where (companycode=@companycode) AND (agentdcno=@agentdcno)");
                        cmd.Parameters.AddWithValue("@agentdcno", agentdcNo);
                        cmd.Parameters.AddWithValue("@companycode", ddlcompny.SelectedValue);
                        dtAgent = vdm.SelectQuery(cmd).Tables[0];
                    }
                }
            }
            else
            {
                if (ddlcompny.SelectedValue == "ALL")
                {
                    if (ddltype.SelectedValue == "Stock Transfer")
                    {
                        if (ddlinvoicetype.SelectedValue == "NonTax")
                        {
                            cmd = new MySqlCommand("SELECT  BranchID, IndDate, agentstno AS agentdcno, soid, stateid, companycode, moduleid FROM agentst WHERE (IndDate BETWEEN @d1 AND @d2) AND (soid = @soid) ORDER BY agentstno");
                        }
                        else
                        {
                            cmd = new MySqlCommand("SELECT  BranchID, IndDate, agentstno AS agentdcno, soid, stateid, companycode, moduleid FROM agenttaxst WHERE (IndDate BETWEEN @d1 AND @d2) AND (soid = @soid) ORDER BY agentstno");

                        }
                    }
                    else
                    {
                        if (ddltype.SelectedValue == "Date Based")
                        {
                            if (ddlinvoicetype.SelectedValue == "NonTax")
                            {
                                cmd = new MySqlCommand("SELECT DcNo, BranchID, IndDate, agentdcno, soid, stateid, companycode, moduleid FROM agentdc WHERE (IndDate BETWEEN @d1 AND @d2) AND (soid = @soid) ORDER BY agentdcno");
                            }
                            else
                            {
                                cmd = new MySqlCommand("SELECT DcNo, BranchID, IndDate, agentdcno, soid, stateid, companycode, moduleid FROM agenttaxdc WHERE (IndDate BETWEEN @d1 AND @d2) AND (soid = @soid) ORDER BY agentdcno");

                            }
                        }
                        else
                        {
                            if (ddlinvoicetype.SelectedValue == "NonTax")
                            {
                                cmd = new MySqlCommand("SELECT DcNo, BranchID, IndDate, agentdcno, soid, stateid, companycode, moduleid FROM agentdc WHERE   (agentdcno BETWEEN @i1 AND @i2) AND (soid = @soid) ORDER BY agentdcno");
                                cmd.Parameters.AddWithValue("@i1", txt_fromno.Text);
                                cmd.Parameters.AddWithValue("@i2", txt_tono.Text);
                            }
                            {
                                cmd = new MySqlCommand("SELECT DcNo, BranchID, IndDate, agentdcno, soid, stateid, companycode, moduleid FROM agenttaxdc WHERE   (agentdcno BETWEEN @i1 AND @i2) AND (soid = @soid) ORDER BY agentdcno");
                                cmd.Parameters.AddWithValue("@i1", txt_fromno.Text);
                                cmd.Parameters.AddWithValue("@i2", txt_tono.Text);
                            }
                        }
                    }
                }
                else
                {
                    if (ddltype.SelectedValue == "Date Based")
                    {
                        if (ddlinvoicetype.SelectedValue == "NonTax")
                        {
                            cmd = new MySqlCommand("SELECT DcNo, BranchID, IndDate, agentdcno, soid, stateid, companycode, moduleid FROM agentdc WHERE (companycode=@companycode) AND  (IndDate BETWEEN @d1 AND @d2) AND (soid = @soid) ORDER BY agentdcno");
                            cmd.Parameters.AddWithValue("@companycode", ddlcompny.SelectedValue);
                        }
                        else
                        {
                            cmd = new MySqlCommand("SELECT DcNo, BranchID, IndDate, agentdcno, soid, stateid, companycode, moduleid FROM agenttaxdc WHERE (companycode=@companycode) AND  (IndDate BETWEEN @d1 AND @d2) AND (soid = @soid) ORDER BY agentdcno");
                            cmd.Parameters.AddWithValue("@companycode", ddlcompny.SelectedValue);
                        }
                    }
                    else if (ddltype.SelectedValue == "Stock Transfer")
                    {
                        if (ddlinvoicetype.SelectedValue == "NonTax")
                        {
                            cmd = new MySqlCommand("SELECT  BranchID, IndDate, agentstno AS agentdcno, soid, stateid, companycode, moduleid FROM agentst WHERE (companycode=@companycode) AND  (agentstno BETWEEN @i1 AND @i2) AND (soid = @soid) ORDER BY agentstno");
                            cmd.Parameters.AddWithValue("@companycode", ddlcompny.SelectedValue);
                            cmd.Parameters.AddWithValue("@i1", txt_fromno.Text);
                            cmd.Parameters.AddWithValue("@i2", txt_tono.Text);
                        }
                        else
                        {
                            cmd = new MySqlCommand("SELECT  BranchID, IndDate, agentstno AS agentdcno, soid, stateid, companycode, moduleid FROM agenttaxst WHERE (companycode=@companycode) AND  (agentstno BETWEEN @i1 AND @i2) AND (soid = @soid) ORDER BY agentstno");
                            cmd.Parameters.AddWithValue("@companycode", ddlcompny.SelectedValue);
                            cmd.Parameters.AddWithValue("@i1", txt_fromno.Text);
                            cmd.Parameters.AddWithValue("@i2", txt_tono.Text);
                        }
                    }
                    else
                    {
                        if (ddlinvoicetype.SelectedValue == "NonTax")
                        {
                            cmd = new MySqlCommand("SELECT DcNo, BranchID, IndDate, agentdcno, soid, stateid, companycode, moduleid FROM agentdc WHERE (companycode=@companycode) AND  (agentdcno BETWEEN @i1 AND @i2) AND (soid = @soid) ORDER BY agentdcno");
                            cmd.Parameters.AddWithValue("@companycode", ddlcompny.SelectedValue);
                            cmd.Parameters.AddWithValue("@i1", txt_fromno.Text);
                            cmd.Parameters.AddWithValue("@i2", txt_tono.Text);
                        }
                        else
                        {
                            cmd = new MySqlCommand("SELECT DcNo, BranchID, IndDate, agentdcno, soid, stateid, companycode, moduleid FROM agenttaxdc WHERE (companycode=@companycode) AND  (agentdcno BETWEEN @i1 AND @i2) AND (soid = @soid) ORDER BY agentdcno");
                            cmd.Parameters.AddWithValue("@companycode", ddlcompny.SelectedValue);
                            cmd.Parameters.AddWithValue("@i1", txt_fromno.Text);
                            cmd.Parameters.AddWithValue("@i2", txt_tono.Text);
                        }
                    }
                }
                cmd.Parameters.AddWithValue("@soid", ddlstateName.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                dtAgent = vdm.SelectQuery(cmd).Tables[0];
            }
            int i = 1;
            SalesDBManager SalesDB = new SalesDBManager();
            cm_d = new SqlCommand("SELECT sno, branchname, address, branchtype, tinno, venorid, branchcode, cstno, mitno, whcode FROM  branch_info");
            DataTable dtproduction = SalesDB.SelectQuery(cm_d).Tables[0];
            SalesDB = new SalesDBManager(i);
            cm_d = new SqlCommand("SELECT  branchid, branchname, address, phone, tino, stno, cstno, emailid, whcode, type, tbranchname, branchcode, GSTIN, statename, regtype, branchledgername,companycode FROM branchmaster");
            DataTable dtpo = SalesDB.SelectQuery(cm_d).Tables[0];
            cmd = new MySqlCommand("SELECT  sno, BranchName, stateid, whcode FROM branchdata");
            DataTable dtsalebranches = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT  sno, DispName, Branch_Id, Route_id, flag, DispType, BranchID, Dispdate, DispMode, DispTime, IndTime, routecolor FROM  dispatch");
            DataTable dtcreditbnames = vdm.SelectQuery(cmd).Tables[0];
            if (dtAgent.Rows.Count > 0)
            {
                foreach (DataRow branch in dtAgent.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    string indentdate = branch["IndDate"].ToString();
                    DateTime dtinddate = Convert.ToDateTime(indentdate);
                    newrow["DOE"] = dtinddate.ToString("dd-MMM-yyyy");
                    newrow["Invoice No"] = branch["agentdcno"].ToString();
                    newrow["State"] = branch["stateid"].ToString();
                    string companycode = branch["companycode"].ToString();
                    string companyname = "";
                    if (companycode == "1")
                    {
                        companyname = "SVDS";
                    }
                    else if (companycode == "2")
                    {
                        companyname = "SVD";
                    }
                    else if (companycode == "3")
                    {
                        companyname = "SVF";
                    }
                    else
                    {
                        companyname = "Others";
                    }
                    newrow["Company"] = companyname;
                    string moduleid = branch["moduleid"].ToString();
                    string BranchID = branch["BranchID"].ToString();
                    string modulename = "";
                    string BranchName = "";
                    if (moduleid == "1")
                    {
                        modulename = "Sales";
                        foreach (DataRow drsbrances in dtsalebranches.Select("sno='" + BranchID + "'"))
                        {
                            BranchName = drsbrances["BranchName"].ToString();
                        }
                    }
                    if (moduleid == "2")
                    {
                        modulename = "P&S";
                        foreach (DataRow drbrances in dtpo.Select("branchid='" + BranchID + "'"))
                        {
                            BranchName = drbrances["branchname"].ToString();
                        }
                    }
                    if (moduleid == "3")
                    {
                        modulename = "Production";
                        foreach (DataRow drbrances in dtproduction.Select("sno='" + BranchID + "'"))
                        {
                            BranchName = drbrances["branchname"].ToString();
                        }
                    }
                    if (moduleid == "4")
                    {
                        modulename = "Credit Note";
                        foreach (DataRow drsbrances in dtcreditbnames.Select("sno='" + BranchID + "'"))
                        {
                            BranchName = drsbrances["DispName"].ToString();
                        }
                    }
                    if (moduleid == "5")
                    {
                        modulename = "Fleet";
                    }
                    newrow["Module"] = modulename;
                    newrow["BranchName"] = BranchName;
                    Report.Rows.Add(newrow);
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
                Session["xportdata"] = Report;
                TextBox1.Text = "";
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
            grdReports.DataSource = dtReport;
            grdReports.DataBind();
        }
    }
    protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        if (ddltype.SelectedValue == "Date Based")
        {
            panel_invoice.Visible = false;
            panel_date.Visible = true;
        }
        else
        {
            panel_invoice.Visible = true;
            panel_date.Visible = false;
        }
    }

}