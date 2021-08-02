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

public partial class InventarySummary : System.Web.UI.Page
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
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                FillAgentName();
            }
        }
    }
    void FillAgentName()
    {
        try
        {
            vdm = new VehicleDBMgr();
            //cmd = new MySqlCommand(" SELECT  sno, Categoryname, flag, userdata_sno, tcategory, categorycode, rank, description, tempcatsno FROM products_category");
            cmd = new MySqlCommand("SELECT BranchName, sno FROM  branchdata WHERE (sno = @BranchID)");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);

            DataTable dtCategory = vdm.SelectQuery(cmd).Tables[0];
            ddlCategoryName.DataSource = dtCategory;
            ddlCategoryName.DataTextField = "BranchName";
            ddlCategoryName.DataValueField = "sno";
            ddlCategoryName.DataBind();
            ddlCategoryName.Items.Insert(0, new ListItem("Select", "0"));
            //Categorypannel.Visible = false;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
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

            cmd = new MySqlCommand("SELECT  inventarytransactions.sno, inventarytransactions.invsno, branchdata.branchcode, SUM(inventarytransactions.openinginv) AS openinginv, SUM(inventarytransactions.isuue_invqty) AS isuue_invqty,SUM(inventarytransactions.receive_invqty) AS receive_invqty, SUM(inventarytransactions.closing_invqty) AS closing_invqty, inventarytransactions.doe, inventarytransactions.closing_date,inventarytransactions.branchid, branchdata.BranchName, invmaster.InvName, branchmappingtable.SuperBranch FROM inventarytransactions INNER JOIN branchdata ON inventarytransactions.branchid = branchdata.sno INNER JOIN invmaster ON inventarytransactions.invsno = invmaster.sno INNER JOIN branchmappingtable ON inventarytransactions.branchid = branchmappingtable.SubBranch WHERE (inventarytransactions.closing_date BETWEEN @d1 AND @d2) AND (branchmappingtable.SuperBranch = @BranchId)  GROUP BY inventarytransactions.branchid");
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
            //cmd.Parameters.AddWithValue("@InvSno", "1");
            DataTable dtdue = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT    sno, InvName, Userdata_sno, flag, Qty, tempinvname FROM  invmaster order by sno");
            //cmd.Parameters.AddWithValue("@sno", "1");
            produtstbl1 = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT   branchdata.BranchName, branchdata.sno,branchdata.branchcode FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE  (branchmappingtable.SuperBranch = @SuperBranch) AND (branchdata.SalesType = @SalesType) AND (branchdata.flag=@flag) order by branchdata.sno");
            cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
            cmd.Parameters.AddWithValue("@SalesType", "21");
            cmd.Parameters.AddWithValue("@flag", "1");
            DataTable dtBranches = vdm.SelectQuery(cmd).Tables[0];

            if (produtstbl1.Rows.Count > 0)
            {
                DataView view1 = new DataView(produtstbl1);
                DataTable distinctproducts = view1.ToTable(true, "InvName", "sno");

                DataView Inventaryview = new DataView(dtdue);
                DataTable distinctinventary = Inventaryview.ToTable(true, "invsno", "branchid", "branchcode");


                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("InventaryName");
                Report.Columns.Add("Opening");

                int count = 0;
                //foreach (DataRow dr in distinctproducts.Rows)
                //{
                //    Report.Columns.Add(dr["BranchName"].ToString()).DataType = typeof(Double);
                //    count++;
                //}
                foreach (DataRow dr in distinctinventary.Rows)
                {
                    Report.Columns.Add(dr["branchcode"].ToString() + "_" + "Issue" + "");
                    Report.Columns.Add(dr["branchcode"].ToString() + "_" + "Receive" + "");
                }
                Report.Columns.Add("Closing");
                double gtotissue = 0; double gtotrecive = 0;
                foreach (DataRow drbranch in distinctproducts.Rows)
                {
                    int i = 0;
                    DataRow newrow = Report.NewRow();
                    string Branchcode = "";
                    string columndate = "";
                    string columndate1 = "";
                    string invName = "";
                    invName = drbranch["InvName"].ToString();
                    double totIssue = 0; double totReceive = 0;
                    foreach (DataRow drdue in dtdue.Rows)
                    {
                        if (drbranch["sno"].ToString() == drdue["invsno"].ToString())
                        {
                            string issue = drdue["isuue_invqty"].ToString();
                            string Receive = drdue["receive_invqty"].ToString();
                            double issueqty = 0, receiveqty = 0; ;
                            double.TryParse(drdue["isuue_invqty"].ToString(), out issueqty);
                            totIssue += issueqty;
                            double.TryParse(drdue["receive_invqty"].ToString(), out receiveqty);
                            totReceive += receiveqty;
                            Branchcode = drdue["branchcode"].ToString();
                            columndate = Branchcode + "_" + "Issue" + "";
                            columndate1 = Branchcode + "_" + "Receive" + "";
                            //}
                        }
                        newrow["InventaryName"] = invName;
                        if (columndate1 != "")
                        {
                            newrow[columndate] = totIssue;
                            newrow[columndate1] = totReceive;
                        }
                    }
                    Report.Rows.Add(newrow);
                }
                grdReports.DataSource = Report;
                grdReports.DataBind();
                grdReports.Visible = true;
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


            //    DataTable dt = new DataTable("GridView_Data");
            //    int count = 0;
            //    foreach (TableCell cell in grdReports.HeaderRow.Cells)
            //    {
            //        if (count == 1)
            //        {
            //            dt.Columns.Add(cell.Text);
            //        }
            //        else
            //        {
            //            dt.Columns.Add(cell.Text);
            //        }
            //        count++;
            //    }
            //    foreach (GridViewRow row in grdReports.Rows)
            //    {
            //        dt.Rows.Add();
            //        for (int i = 1; i < row.Cells.Count; i++)
            //        {
            //            if (row.Cells[i].Text == "&nbsp;")
            //            {
            //                row.Cells[i].Text = "0";
            //            }
            //            dt.Rows[dt.Rows.Count - 1][i] = row.Cells[i].Text;
            //        }
            //    }
            //    using (XLWorkbook wb = new XLWorkbook())
            //    {
            //        wb.Worksheets.Add(dt);
            //        Response.Clear();
            //        Response.Buffer = true;
            //        Response.Charset = "";
            //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //        //string FileName = Session["filename"].ToString();
            //        string FileName = "Branch Wse Net Sale";
            //        Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");
            //        using (MemoryStream MyMemoryStream = new MemoryStream())
            //        {
            //            wb.SaveAs(MyMemoryStream);
            //            MyMemoryStream.WriteTo(Response.OutputStream);
            //            Response.Flush();
            //            Response.End();
            //        }
            //    }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }

    protected void grdReports_RowCreated(object sender, GridViewRowEventArgs e)
    {
       
    }
}