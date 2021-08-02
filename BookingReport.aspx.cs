using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class BookingReport : System.Web.UI.Page
{
    MySqlCommand cmd;
    string BranchID = "";
    string UserName = "";
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
                lblDate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
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
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    DataTable Report = new DataTable();
    DateTime fromdate = DateTime.Now;
    DateTime todate = DateTime.Now;
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            vdm = new VehicleDBMgr();
            Report = new DataTable();
            todate = todate.AddDays(5);
            cmd = new MySqlCommand("SELECT bookingtransaction.Transno,bookingsubtable.Qty,bookingsubtable.Cost,DATE_FORMAT(bookingtransaction.DateOfDel, '%d %b %y') AS IndentDate , bookingtransaction.PersonName,bookingtransaction.Address, bookingtransaction.Mobno, bookingtransaction.BookingDate, productsdata.ProductName, products_category.Categoryname FROM bookingtransaction INNER JOIN bookingsubtable ON bookingtransaction.Transno = bookingsubtable.TransNo INNER JOIN productsdata ON bookingsubtable.ProductID = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (bookingtransaction.DateOfDel BETWEEN @d1 AND @d2) ORDER BY bookingtransaction.DateOfDel");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));  
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));  
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            if (dtble.Rows.Count > 0)
            {
                DataView view = new DataView(dtble);
                DataTable produtstbl = view.ToTable(true, "ProductName", "Categoryname");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("DC No");
                Report.Columns.Add("Person Name");
                Report.Columns.Add("Delivery Date");
                Report.Columns.Add("Mob No");
                int count = 0;
                int rowcount = 0;
                foreach (DataRow dr in produtstbl.Rows)
                {

                    if (dr["Categoryname"].ToString() == "MILK")
                    {
                        Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                        count++;
                        rowcount++;
                    }
                    else
                    {
                        Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                        rowcount++;
                    }
                }
                Report.Columns.Add("Total Indent", typeof(Double)).SetOrdinal(count + 4);
                Report.Columns.Add("Sale Value", typeof(Double)).SetOrdinal(rowcount + 5);
                Report.Columns.Add("Address");
                int i = 1;
                DataTable Distinctable = view.ToTable(true, "PersonName", "IndentDate", "Mobno","Address","Transno");

                foreach (DataRow branch in Distinctable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    newrow["DC No"] = branch["Transno"].ToString();
                    newrow["Person Name"] = branch["PersonName"].ToString();
                    newrow["Delivery Date"] = branch["IndentDate"].ToString();
                    newrow["Mob No"] = branch["Mobno"].ToString();
                    newrow["Address"] = branch["Address"].ToString();
                    double total = 0;
                    double totalamount = 0;
                    foreach (DataRow dr in dtble.Rows)
                    {
                        if (branch["PersonName"].ToString() == dr["PersonName"].ToString())
                        {
                            double qtyvalue = 0;
                            newrow[dr["ProductName"].ToString()] = dr["Qty"].ToString();
                            double.TryParse(dr["Qty"].ToString(), out qtyvalue);
                            double cost = 0;
                            double.TryParse(dr["Cost"].ToString(), out cost);
                            totalamount += cost * qtyvalue;
                            totalamount = Math.Round(totalamount, 2);
                            if (dr["Categoryname"].ToString() == "MILK")
                            {
                                total += qtyvalue;
                            }
                        }
                    }
                    newrow["Total Indent"] = total;
                    newrow["Sale Value"] = totalamount;
                    Report.Rows.Add(newrow);
                    i++;
                }
                DataRow newvartical = Report.NewRow();
                newvartical["Person Name"] = "Total";
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
                Session["xportdata"] = Report;
            }
            else
            {
                lblmsg.Text = "No Indent Found";
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
}