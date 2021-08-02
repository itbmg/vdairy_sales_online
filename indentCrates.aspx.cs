using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class indentCrates : System.Web.UI.Page
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
        //vdm = new VehicleDBMgr();
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                FillDispName();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                lblTitle.Text = Session["TitleName"].ToString();
            }
        }


    }
    void FillDispName()
    {
        try
        {
            string salestype = Session["salestype"].ToString();
            if (salestype == "Plant")
            {
                vdm = new VehicleDBMgr();
                cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD) and (DispMode Is NULL)");
                cmd.Parameters.AddWithValue("@BranchD", Session["branch"].ToString());
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlRouteName.DataSource = dtRoutedata;
                ddlRouteName.DataTextField = "DispName";
                ddlRouteName.DataValueField = "sno";
                ddlRouteName.DataBind();
                ddlRouteName.Items.Insert(0, new ListItem("Select", "0"));
            }
            if (salestype == "SALES OFFICE")
            {
                vdm = new VehicleDBMgr();
                cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM branchdata INNER JOIN branchroutes ON branchdata.sno = branchroutes.BranchID INNER JOIN dispatch ON branchroutes.BranchID = dispatch.BranchID WHERE (dispatch.flag = 1) AND (branchdata.SalesOfficeID = @SOID) OR (branchroutes.BranchID = @BranchID) GROUP BY dispatch.DispName");
                //cmd = new MySqlCommand("SELECT DispName, sno, Branch_Id, BranchID FROM (SELECT dispatch.DispName, dispatch.sno, dispatch.Branch_Id, branchroutes.BranchID FROM dispatch_sub INNER JOIN branchroutes ON dispatch_sub.Route_id = branchroutes.Sno RIGHT OUTER JOIN dispatch ON dispatch_sub.dispatch_sno = dispatch.sno WHERE (NOT (branchroutes.BranchID = @BranchID)) OR (NOT (dispatch.Branch_Id = @branchid))) Result  WHERE (BranchID = @BranchID)GROUP BY DispName");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlRouteName.DataSource = dtRoutedata;
                ddlRouteName.DataTextField = "DispName";
                ddlRouteName.DataValueField = "sno";
                ddlRouteName.DataBind();
            }
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
    DateTime fromdate = DateTime.Now;
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            pnlHide.Visible = true;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            //lblDate.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            lblDispatchName.Text = ddlRouteName.SelectedItem.Text;
            vdm = new VehicleDBMgr();
            DataTable Report = new DataTable();

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
            lblDate.Text = fromdate.AddDays(1).ToString("dd/MM/yyyy");
            Session["RouteName"] = ddlRouteName.SelectedItem.Text + fromdate.AddDays(1).ToString("dd/MM/yyyy");
            Session["filename"] = ddlRouteName.SelectedItem.Text + fromdate.AddDays(1).ToString("dd/MM/yyyy");
            //cmd = new MySqlCommand("SELECT branchroutes.RouteName, productsdata.ProductName, ROUND(SUM(indents_subtable.unitQty), 2) AS unitQty, products_category.Categoryname FROM indents INNER JOIN branchroutesubtable ON indents.Branch_id = branchroutesubtable.BranchID INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (indents.I_date >= @starttime) AND (indents.I_date <= @endtime) AND (branchroutes.Sno BETWEEN 29 AND 33) GROUP BY branchroutes.RouteName, productsdata.ProductName, products_category.Categoryname");
            cmd = new MySqlCommand("select Route_id,IndentType from dispatch_sub where dispatch_sno=@dispsno");
            cmd.Parameters.AddWithValue("@dispsno", ddlRouteName.SelectedValue);
            DataTable dtrouteindenttype = vdm.SelectQuery(cmd).Tables[0];
            var routeitype = "";
            foreach (DataRow drrouteitype in dtrouteindenttype.Rows)
            {
                var routeid = drrouteitype["Route_id"].ToString();
                routeitype = drrouteitype["IndentType"].ToString();
            }
            //cmd = new MySqlCommand("SELECT branchroutes.RouteName, productsdata.ProductName, ROUND(SUM(indents_subtable.unitQty), 2) AS unitQty, products_category.Categoryname,productsdata.Units, branchproducts.Rank, invmaster.Qty FROM indents INNER JOIN branchroutesubtable ON indents.Branch_id = branchroutesubtable.BranchID INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN dispatch_sub ON branchroutes.Sno = dispatch_sub.Route_id INNER JOIN dispatch ON dispatch_sub.dispatch_sno = dispatch.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno WHERE (indents.I_date BETWEEN @starttime AND @endtime) AND (indents.IndentType = @itype) AND (dispatch.sno = @dispatchSno) AND (branchproducts.branch_sno=@BranchID) GROUP BY branchroutes.RouteName, productsdata.ProductName, products_category.Categoryname ORDER BY branchproducts.Rank");
            cmd = new MySqlCommand("SELECT modifiedroutes.RouteName, ROUND(SUM(indents_subtable.unitQty), 2) AS unitQty, productsdata.ProductName, productsdata.Units, products_category.Categoryname, invmaster.Qty, brnchprdt.Rank FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN modifiedroutes ON dispatch_sub.Route_id = modifiedroutes.Sno INNER JOIN modifiedroutesubtable ON modifiedroutes.Sno = modifiedroutesubtable.RefNo INNER JOIN (SELECT IndentNo, Branch_id, I_date, IndentType FROM indents WHERE (I_date BETWEEN @starttime AND @endtime) AND (IndentType = @itype)) indent ON modifiedroutesubtable.BranchID = indent.Branch_id INNER JOIN indents_subtable ON indent.IndentNo = indents_subtable.IndentNo INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno INNER JOIN (SELECT branch_sno, product_sno, Rank FROM branchproducts WHERE (branch_sno = @BranchID)) brnchprdt ON productsdata.sno = brnchprdt.product_sno WHERE (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate IS NULL) AND (modifiedroutesubtable.CDate <= @starttime) OR (dispatch.sno = @dispatchSno) AND (modifiedroutesubtable.EDate > @starttime) AND (modifiedroutesubtable.CDate <= @starttime) GROUP BY modifiedroutes.RouteName, products_category.Categoryname, productsdata.sno ORDER BY brnchprdt.Rank"); 
            if (Session["salestype"].ToString() == "Plant")
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["BranchID"]);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            }
            cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            cmd.Parameters.AddWithValue("@starttime", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@endtime", GetHighDate(fromdate));
            cmd.Parameters.AddWithValue("@itype", routeitype);
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            // cmd = new MySqlCommand("SELECT products_category.Categoryname, products_subcategory.SubCatName, productsdata.ProductName, productsdata.Units, invmaster.Qty FROM branchproducts INNER JOIN dispatch ON branchproducts.branch_sno = dispatch.Branch_Id INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno WHERE (dispatch.sno = @dispatchSno) GROUP BY productsdata.ProductName ORDER BY productsdata.Rank");
            //cmd = new MySqlCommand("SELECT products_category.Categoryname, products_subcategory.SubCatName, productsdata.ProductName, invmaster.Qty, productsdata.Units FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutes ON dispatch_sub.Route_id = branchroutes.Sno INNER JOIN branchproducts ON branchroutes.BranchID = branchproducts.branch_sno INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno WHERE (dispatch.sno = @dispatchSno) GROUP BY productsdata.ProductName ORDER BY productsdata.Rank");
            //cmd.Parameters.AddWithValue("@dispatchSno", ddlRouteName.SelectedValue);
            //DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
            if (dtble.Rows.Count > 0)
            {
                DataView view = new DataView(dtble);
                DataTable produtstbl = view.ToTable(true, "ProductName", "Categoryname", "Units", "Qty");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("Route Name");
                int count = 0;
                foreach (DataRow dr in produtstbl.Rows)
                {

                    if (dr["Categoryname"].ToString() == "MILK")
                    {
                        Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                        count++;
                    }
                    else
                    {
                        Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                    }
                }
                Report.Columns.Add("Total Indent", typeof(Double)).SetOrdinal(count + 2);
                Report.Columns.Add("Total MILK CURD AND BM", typeof(Double));
                DataTable distincttable = view.ToTable(true, "RouteName");
                int i = 1;
                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    newrow["Route Name"] = branch["RouteName"].ToString();

                    double total = 0;
                    foreach (DataRow dr in dtble.Rows)
                    {
                        if (branch["RouteName"].ToString() == dr["RouteName"].ToString())
                        {
                            double qtyvalue = 0;
                            double curdqtyvalue = 0;
                            double BMqtyvalue = 0;
                            //newrow[dr["ProductName"].ToString()] = dr["unitQty"].ToString();
                            if (dr["Categoryname"].ToString() == "MILK")
                            {
                                double.TryParse(dr["unitQty"].ToString(), out qtyvalue);
                                double Qty = 0;
                                double cratesQty = 0;
                                double.TryParse(dr["Qty"].ToString(), out Qty);
                                cratesQty = qtyvalue / Qty;
                                if (Qty == 12)
                                {
                                    total += cratesQty;

                                }

                                newrow[dr["ProductName"].ToString()] = Math.Round(cratesQty, 2);//cratesQty.ToString();
                            }
                            if (dr["Categoryname"].ToString() == "CURD")
                            {

                                if (dr["ProductName"].ToString() == "CURD 10 MRP")
                                {
                                    double.TryParse(dr["unitQty"].ToString(), out curdqtyvalue);
                                    double Qty = 0;
                                    double cratesQty = 0;
                                    double.TryParse(dr["Qty"].ToString(), out Qty);
                                    cratesQty = curdqtyvalue / 10.5;
                                    if (Qty == 12)
                                    {
                                        total += cratesQty;

                                    }
                                    //totalcurd += curdqtyvalue;
                                    newrow[dr["ProductName"].ToString()] = Math.Round(cratesQty, 2);//cratesQty.ToString();
                                }
                                if (dr["ProductName"].ToString() == "CURD-450ml")
                                {
                                    double.TryParse(dr["unitQty"].ToString(), out curdqtyvalue);
                                    double Qty = 0;
                                    double cratesQty = 0;
                                    double.TryParse(dr["Qty"].ToString(), out Qty);
                                    cratesQty = curdqtyvalue / 10.8;
                                    if (Qty == 12)
                                    {
                                        total += cratesQty;

                                    }
                                    //totalcurd += curdqtyvalue;
                                    newrow[dr["ProductName"].ToString()] = Math.Round(cratesQty, 2);  // cratesQty.ToString();
                                }
                                else
                                {
                                    double.TryParse(dr["unitQty"].ToString(), out curdqtyvalue);
                                    double Qty = 0;
                                    double cratesQty = 0;
                                    double.TryParse(dr["Qty"].ToString(), out Qty);
                                    cratesQty = curdqtyvalue / Qty;
                                    if (Qty == 12)
                                    {
                                        total += cratesQty;
                                    }
                                    //totalcurd += curdqtyvalue;
                                    newrow[dr["ProductName"].ToString()] = Math.Round(cratesQty, 2);//cratesQty.ToString();
                                }
                            }
                            if (dr["Categoryname"].ToString() == "ButterMilk")
                            {
                                double.TryParse(dr["unitQty"].ToString(), out BMqtyvalue);
                                double Qty = 0;
                                double cratesQty = 0;
                                double.TryParse(dr["Qty"].ToString(), out Qty);
                                cratesQty = BMqtyvalue / Qty;
                                if (Qty == 12)
                                {
                                    total += cratesQty;

                                }
                               // totalbuttermilk += BMqtyvalue;
                                newrow[dr["ProductName"].ToString()] = Math.Round(cratesQty, 2);//cratesQty.ToString();
                            }
                        }
                    }
                    //newrow["Total Indent"] = total;
                    newrow["Total MILK CURD AND BM"] = total;
                    Report.Rows.Add(newrow);
                    i++;
                }
                DataRow newvartical = Report.NewRow();
                newvartical["Route Name"] = "Total";
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
                DataRow Break = Report.NewRow();
                Break["Route Name"] = "";
                Report.Rows.Add(Break);
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
                pnlHide.Visible = false;
                lblmsg.Text = "No Indent Found";
                grdReports.DataSource = Report;
                grdReports.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
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


    protected void ddlRouteName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["salestype"].ToString() == "Plant")
        {
            vdm = new VehicleDBMgr();
            cmd = new MySqlCommand("SELECT sno, DispName, BranchID, Dispdate, DispMode FROM dispatch WHERE (sno = @dispSno)");
            cmd.Parameters.AddWithValue("@dispSno", ddlRouteName.SelectedValue);
            DataTable dtBranch = vdm.SelectQuery(cmd).Tables[0];
            if (dtBranch.Rows.Count > 0)
            {
                Session["BranchID"] = dtBranch.Rows[0]["BranchID"].ToString();
            }
        }
        else
        {
        }
    }
}