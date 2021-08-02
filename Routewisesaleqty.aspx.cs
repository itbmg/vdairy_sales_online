using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;


public partial class RoutewiseSaleQty : System.Web.UI.Page
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
                FillRouteName();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txttodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
            }
        }

    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    void FillRouteName()
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
                //if (ddlSalesOffice.SelectedIndex == -1)
                //{
                //    ddlSalesOffice.SelectedItem.Text = "Select";
                //}
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                PBranch.Visible = false;

                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID)  and (DispType is NULL) AND (dispatch.flag=@flag)) OR ((branchdata_1.SalesOfficeID = @SOID)  and (DispType is NULL) AND (dispatch.flag=@flag))");
                //cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@flag", "1");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlRouteName.DataSource = dtRoutedata;
                ddlRouteName.DataTextField = "DispName";
                ddlRouteName.DataValueField = "sno";
                ddlRouteName.DataBind();
            }
        }
        catch
        {
        }
    }
    protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE ((branchdata.sno = @BranchID)  and (DispType is NULL) AND (dispatch.flag=@flag)) OR ((branchdata_1.SalesOfficeID = @SOID)  and (DispType is NULL) AND (dispatch.flag=@flag))");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@flag", "1");
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlRouteName.DataSource = dtRoutedata;
        ddlRouteName.DataTextField = "DispName";
        ddlRouteName.DataValueField = "sno";
        ddlRouteName.DataBind();
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
    DateTime todate = DateTime.Now;
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            Session["RouteName"] = ddlRouteName.SelectedItem.Text;
            lblRouteName.Text = ddlRouteName.SelectedItem.Text;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
            Report = new DataTable();
            string[] datestrig = txtdate.Text.Split(' ');
            string[] datestrig1 = txttodate.Text.Split(' ');
            if (datestrig.Length > 1)
            {
                if (datestrig[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig[0].Split('-');
                    string[] times = datestrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            if (datestrig1.Length > 1)
            {
                if (datestrig1[0].Split('-').Length > 0)
                {
                    string[] dates = datestrig1[0].Split('-');
                    string[] times = datestrig1[1].Split(':');
                    todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            //lblDate.Text = fromdate.AddDays(1).ToString("dd/MMM/yyyy");
            lblDate.Text = txtdate.Text;
            lbltodate.Text = txttodate.Text;
            Session["filename"] = ddlRouteName.SelectedItem.Text + fromdate.AddDays(1).ToString("dd/MM/yyyy");
            cmd = new MySqlCommand("SELECT DATE_FORMAT(indents.I_date, '%m-%d-%Y') AS IndentDate FROM dispatch INNER JOIN branchroutesubtable ON dispatch.Route_id = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN indents ON branchdata.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (dispatch.sno = @dispsno) GROUP BY IndentDate ORDER BY IndentDate ");
            cmd.Parameters.AddWithValue("@dispsno", ddlRouteName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
            DataTable dtindentdates = vdm.SelectQuery(cmd).Tables[0];
            //cmd = new MySqlCommand("SELECT branchdata.BranchName, SUM(indents_subtable.DeliveryQty) AS dqty, indents.I_date, branchdata.sno FROM dispatch INNER JOIN branchroutesubtable ON dispatch.Route_id = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN indents ON branchdata.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (dispatch.sno = @dispsno) GROUP BY indents.I_date ORDER BY branchdata.sno, indents.I_date");
            cmd = new MySqlCommand("SELECT branchdata.BranchName, SUM(indents_subtable.DeliveryQty) AS dqty, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue, indents.I_date,branchdata.sno FROM dispatch INNER JOIN branchroutesubtable ON dispatch.Route_id = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN indents ON branchdata.sno = indents.Branch_id INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (dispatch.sno = @dispsno) GROUP BY indents.IndentNo ORDER BY branchdata.sno, indents.I_date"); 
            cmd.Parameters.AddWithValue("@dispsno", ddlRouteName.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
            DataTable dtsaleqty = vdm.SelectQuery(cmd).Tables[0];

            Report = new DataTable();
            Report.Columns.Add("SNo");
            Report.Columns.Add("Agent Name");
            int count = 0;
            foreach (DataRow dr in dtindentdates.Rows)
            {
                string Ind = dr["IndentDate"].ToString();
                DateTime dtInd = Convert.ToDateTime(Ind);
                dtInd = dtInd.AddDays(1);
                string DelDate = dtInd.ToString("dd/MM/yyyy");
                Report.Columns.Add(DelDate).DataType = typeof(Double);
                count++;
            }
            Report.Columns.Add("Total Qty", typeof(Double)).SetOrdinal(count + 2);
            Report.Columns.Add("Sale Value", typeof(Double));
            DataView view = new DataView(dtsaleqty);

            DataTable distincttable = view.ToTable(true, "BranchName");

            //foreach
            int i = 1;
            foreach (DataRow branch in distincttable.Rows)
            {

                DataRow newrow = Report.NewRow();
                double totalsale = 0;
                newrow["SNo"] = i;
                newrow["Agent Name"] = branch["BranchName"].ToString();
                string branchname = branch["BranchName"].ToString();
                double salevalue = 0;
                foreach (DataRow drsaleqty in dtsaleqty.Rows)
                {

                    if (branchname == drsaleqty["BranchName"].ToString())
                    {
                        string dtdate1 = drsaleqty["I_date"].ToString();
                        DateTime dtDOE1 = Convert.ToDateTime(dtdate1).AddDays(1);
                        // DATE_FORMAT(NOW(),'%m-%d-%Y')

                        string ChangedTime1 = dtDOE1.ToString("dd/MM/yyyy");
                        double dqtym = 0;
                        double.TryParse(drsaleqty["dqty"].ToString(), out dqtym);
                        newrow[ChangedTime1] = Math.Round(dqtym, 2);
                        double totsale = 0;
                        double.TryParse(drsaleqty["dqty"].ToString(), out totsale);
                        double totsalevalue = 0;
                        double.TryParse(drsaleqty["salevalue"].ToString(), out totsalevalue);
                        totalsale += totsale;
                        salevalue += totsalevalue;
                    }
                }
                newrow["Total Qty"] = Math.Round(totalsale, 2);
                newrow["Sale Value"] = Math.Round(salevalue, 2);
                Report.Rows.Add(newrow);
                i++;
            }
            DataRow newvartical = Report.NewRow();
            newvartical["Agent Name"] = "Total";
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
        catch(Exception ex)
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
}