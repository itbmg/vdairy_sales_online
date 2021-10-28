using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using ClosedXML.Excel;
using System.IO;
public partial class AgentInvoiceReport : System.Web.UI.Page
{
    MySqlCommand cmd;
    string BranchID = "";
    VehicleDBMgr vdm;
    string tpo ;
    string tinv ;
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
                //lblTitle.Text = Session["TitleName"].ToString();
                //lblTitle1.Text = Session["TitleName"].ToString();
                //lblTitle2.Text = Session["TitleName"].ToString();

                //lblAddress.Text = Session["Address"].ToString();
                //lbltinNo.Text = Session["TinNo"].ToString();
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
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType) or (branchmappingtable.SuperBranch = @SuperBranch) and (branchdata.SalesType=@SalesType1) ");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                cmd.Parameters.AddWithValue("@SalesType", "21");
                cmd.Parameters.AddWithValue("@SalesType1", "26");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlSalesOfficesumary.DataSource = dtRoutedata;
                ddlSalesOfficesumary.DataTextField = "BranchName";
                ddlSalesOfficesumary.DataValueField = "sno";
                ddlSalesOfficesumary.DataBind();
                ddlSalesOfficesumary.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {

                PBranch.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.sno = @BranchID) AND (branchdata.SalesType IS NOT NULL)");
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlSalesOfficesumary.DataSource = dtRoutedata;
                ddlSalesOfficesumary.DataTextField = "BranchName";
                ddlSalesOfficesumary.DataValueField = "sno";
                ddlSalesOfficesumary.DataBind();
                ddlSalesOfficesumary.Items.Insert(0, new ListItem("Select", "0"));
                //cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
                ////cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
                //cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                //cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
                //DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                //ddlDispNamesumary.DataSource = dtRoutedata;
                //ddlDispNamesumary.DataTextField = "DispName";
                //ddlDispNamesumary.DataValueField = "sno";
                //ddlDispNamesumary.DataBind();
                //ddlDispNamesumary.Items.Insert(0, new ListItem("Select", "0"));
            }
            cmd = new MySqlCommand(" SELECT  sno, Categoryname, flag, userdata_sno, tcategory, categorycode, rank, description, tempcatsno FROM products_category");
            cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
            DataTable dtCategory = vdm.SelectQuery(cmd).Tables[0];
            ddlCategoryName.DataSource = dtCategory;
            ddlCategoryName.DataTextField = "Categoryname";
            ddlCategoryName.DataValueField = "sno";
            ddlCategoryName.DataBind();
            ddlCategoryName.Items.Insert(0, new ListItem("Select", "0"));
            Categorypannel.Visible = false;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void ddlSalesOfficesumary_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch)");
        cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.DispName FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno WHERE (dispatch.Branch_Id = @dispatch)");
        cmd.Parameters.AddWithValue("@dispatch", ddlSalesOfficesumary.SelectedValue);
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlDispNamesumary.DataSource = dtRoutedata;
        ddlDispNamesumary.DataTextField = "DispName";
        ddlDispNamesumary.DataValueField = "sno";
        ddlDispNamesumary.DataBind();
        ddlDispNamesumary.Items.Insert(0, new ListItem("Select", "0"));
        if (ddlSalesOfficesumary.SelectedValue == "282")
        {
            Categorypannel.Visible = true;
        }
        else
        {
            Categorypannel.Visible = false;

        }
    }
    protected void ddlDispNamesumary_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch)");
        cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutes ON dispatch_sub.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (dispatch.sno = @dispsno)");
        cmd.Parameters.AddWithValue("@dispsno", ddlDispNamesumary.SelectedValue);
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlAgentNamesumary.DataSource = dtRoutedata;
        ddlAgentNamesumary.DataTextField = "BranchName";
        ddlAgentNamesumary.DataValueField = "sno";
        ddlAgentNamesumary.DataBind();
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
    DataTable dtreport = new DataTable();
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            PanelHide.Visible = true;
            Panel_button.Visible = true;
            lblmsg.Text = "";
            vdm = new VehicleDBMgr();
            Report = new DataTable();
            vdm = new VehicleDBMgr();
            SalesDBManager SalesDB = new SalesDBManager();
            DataTable Report1 = new DataTable();
            DataTable dtTotalDispatches = new DataTable();
            Report = new DataTable();
            DateTime fromdate = DateTime.Now;
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
            Session["filename"] = "Statement of Account ->" + ddlAgentNamesumary.SelectedItem.Text;
            lblfromdate.Text = fromdate.ToString("dd/MM/yyyy");
            lbltodate.Text = todate.ToString("dd/MM/yyyy");
            string AgentId = ddlAgentNamesumary.SelectedValue;
            string BranchId = Context.Session["Branch"].ToString();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            lblAgentDescription.Text = ddlAgentNamesumary.SelectedItem.Text;
            DateTime dtapril = new DateTime();
            DateTime dtmarch = new DateTime();
            int currentyear = ServerDateCurrentdate.Year;
            int nextyear = ServerDateCurrentdate.Year + 1;
            int currntyearnum = 0;
            int nextyearnum = 0;
            if (ServerDateCurrentdate.Month > 3)
            {
                string apr = "4/1/" + currentyear;
                dtapril = DateTime.Parse(apr);
                string march = "3/31/" + nextyear;
                dtmarch = DateTime.Parse(march);
                currntyearnum = currentyear;
                nextyearnum = nextyear;
            }
            if (ServerDateCurrentdate.Month <= 3)
            {
                string apr = "4/1/" + (currentyear - 1);
                dtapril = DateTime.Parse(apr);
                string march = "3/31/" + (nextyear - 1);
                dtmarch = DateTime.Parse(march);
                currntyearnum = currentyear - 1;
                nextyearnum = nextyear - 1;
            }
            cmd = new MySqlCommand("SELECT branchdata.companycode, branchdata.phonenumber,branchdata.email, branchdata.sno,branchdata.stateid, branchdata.Address, branchdata.TinNumber, branchdata.panno, branchdata.BranchCode,statemastar.statecode, statemastar.statename, statemastar.gststatecode, branchdata.phonenumber, branchdata.emailid,  branchdata.street, branchdata.city, branchdata.mandal, branchdata.district, branchdata.pincode, branchdata.gstno, branchdata.doorno, branchdata.area FROM branchdata INNER JOIN statemastar ON branchdata.stateid = statemastar.sno WHERE (branchdata.sno = @branchsno)");
            cmd.Parameters.AddWithValue("@branchsno", ddlSalesOfficesumary.SelectedValue);
            DataTable dtbrnchaddress = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT   branchdata.stateid, branchdata.SalesType, branchdata.BranchName, branchdata.Address, branchdata.TinNumber, branchdata.Address AS address, branchdata.tbranchname, branchdata.sno, statemastar.statename, statemastar.gststatecode, branchdata.gstno, branchdata.email, branchdata.phonenumber, branchdata.doorno, branchdata.area, branchdata.street, branchdata.city, branchdata.mandal, branchdata.district, branchdata.pincode FROM branchdata INNER JOIN statemastar ON branchdata.stateid = statemastar.sno WHERE  (branchdata.sno = @AgentID)");
            cmd.Parameters.AddWithValue("@AgentID", ddlAgentNamesumary.SelectedValue);
            DataTable dtAddress = vdm.SelectQuery(cmd).Tables[0];
            //cmd = new MySqlCommand("SELECT indents_subtable.IndentNo,IFNULL(branchproducts.VatPercent, 0) AS VatPercent,productsdata.units,productsdata.qty as uomqty, productsdata.ProductName,indents.quatationno, indents.pono, indents.grnno, indents_subtable.DeliveryQty ,indents_subtable.unitQty AS IndentQty, indents_subtable.UnitCost, DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate,branchdata.stateid, productsdata.itemcode,productsdata.sno AS ProductSno,productsdata.hsncode,productsdata.igst,productsdata.cgst,productsdata.sgst,products_subcategory.SubCatName,products_subcategory.description,products_category.rank FROM  productsdata INNER JOIN indents_subtable ON productsdata.sno = indents_subtable.Product_sno INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN  branchdata ON indents.Branch_id = branchdata.sno INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchproducts ON branchmappingtable.SuperBranch = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_category.sno = products_subcategory.category_sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchdata.sno = @BranchID) AND (indents_subtable.DeliveryQty>0)  ORDER BY indents.I_date, products_category.rank");
            if (ddlSalesOfficesumary.SelectedValue == "282")
            {
                cmd = new MySqlCommand("SELECT indents_subtable.IndentNo,indents_subtable.DTripId,IFNULL(branchproducts.VatPercent, 0) AS VatPercent,productsdata.units,productsdata.qty as uomqty, productsdata.ProductName,indents.quatationno, indents.pono, indents.grnno, SUM(indents_subtable.DeliveryQty) AS DeliveryQty ,indents_subtable.unitQty AS IndentQty, indents_subtable.UnitCost, DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate,branchdata.stateid, productsdata.itemcode,productsdata.sno AS ProductSno,productsdata.hsncode,productsdata.igst,productsdata.cgst,productsdata.sgst,products_subcategory.SubCatName,products_subcategory.description,products_category.rank FROM  productsdata INNER JOIN indents_subtable ON productsdata.sno = indents_subtable.Product_sno INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN  branchdata ON indents.Branch_id = branchdata.sno INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchproducts ON branchmappingtable.SuperBranch = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_category.sno = products_subcategory.category_sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchdata.sno = @BranchID) AND (products_category.sno=@Catsno) AND (indents_subtable.DeliveryQty>0) Group by indents.I_date,products_subcategory.description  ORDER BY indents.I_date, products_category.rank");
                cmd.Parameters.AddWithValue("@BranchID", ddlAgentNamesumary.SelectedValue);
                cmd.Parameters.AddWithValue("@Catsno", ddlCategoryName.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
            }
            else
            {
                cmd = new MySqlCommand("SELECT indents_subtable.IndentNo,indents_subtable.DTripId,IFNULL(branchproducts.VatPercent, 0) AS VatPercent,productsdata.units,productsdata.qty as uomqty, productsdata.ProductName,indents.quatationno, indents.pono, indents.grnno, SUM(indents_subtable.DeliveryQty) AS DeliveryQty ,indents_subtable.unitQty AS IndentQty, indents_subtable.UnitCost, DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate,branchdata.stateid, productsdata.itemcode,productsdata.sno AS ProductSno,productsdata.hsncode,productsdata.igst,productsdata.cgst,productsdata.sgst,products_subcategory.SubCatName,products_subcategory.description,products_category.rank FROM  productsdata INNER JOIN indents_subtable ON productsdata.sno = indents_subtable.Product_sno INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN  branchdata ON indents.Branch_id = branchdata.sno INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchproducts ON branchmappingtable.SuperBranch = branchproducts.branch_sno AND productsdata.sno = branchproducts.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_category.sno = products_subcategory.category_sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchdata.sno = @BranchID)  AND (indents_subtable.DeliveryQty>0) Group by indents.I_date,products_subcategory.description  ORDER BY indents.I_date, products_category.rank");
                cmd.Parameters.AddWithValue("@BranchID", ddlAgentNamesumary.SelectedValue);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate).AddDays(-1));
            }
            DataTable dtInvoice1 = vdm.SelectQuery(cmd).Tables[0];
            dtInvoice1.DefaultView.Sort = "rank ASC";
            DataView dv = dtInvoice1.DefaultView;
            DataTable dtInvoice = dv.ToTable();
            //Report.Columns.Add("Sno");
           

            string AgentAddress = "";
            string AgentName = "";
            string quatationno = "";
            string pono = "";
            string grnno = "";
            string salestype = "";
            string buyerTinNumber = "";
            string stateid = "";
            string companycode = "";
            string tostate = "";
            if (dtAddress.Rows.Count > 0)
            {
                AgentAddress = dtAddress.Rows[0]["doorno"].ToString() + "," + dtAddress.Rows[0]["street"].ToString() + "," + dtAddress.Rows[0]["area"].ToString() + "," + dtAddress.Rows[0]["city"].ToString() + "," + dtAddress.Rows[0]["mandal"].ToString() + "," + dtAddress.Rows[0]["district"].ToString() + "," + dtAddress.Rows[0]["pincode"].ToString();// dtAddress.Rows[0]["Address"].ToString();
                AgentName = dtAddress.Rows[0]["BranchName"].ToString();
                //quatationno = dtAddress.Rows[0]["quatationno"].ToString();
                //pono = dtAddress.Rows[0]["pono"].ToString();
                //grnno = dtAddress.Rows[0]["grnno"].ToString();
                salestype = dtAddress.Rows[0]["salestype"].ToString();
                buyerTinNumber = dtAddress.Rows[0]["TinNumber"].ToString();
                lblAgent.Text = dtAddress.Rows[0]["address"].ToString();
                tostate = dtAddress.Rows[0]["stateid"].ToString();
            }
            string fromstate = ""; string branchcode = "";
            if (dtbrnchaddress.Rows.Count > 0)
            {
                fromstate = dtbrnchaddress.Rows[0]["stateid"].ToString();
                branchcode = dtbrnchaddress.Rows[0]["BranchCode"].ToString();
            }
            if (tostate == "3")
            {
                Report.Columns.Add("Date");
                Report.Columns.Add("PONo").DataType = typeof(double); ;
                Report.Columns.Add("GRN No").DataType = typeof(double); ;
                Report.Columns.Add("DcNumber");
                Report.Columns.Add("InvoiceNo");
            }
            else
            {
                Report.Columns.Add("Date");
                Report.Columns.Add("PONo") ;
                Report.Columns.Add("GRN No");
                Report.Columns.Add("DcNumber");
                Report.Columns.Add("InvoiceNo");
            }
            string DcNo = "";
            if (tostate == "24")
            {
                lblbranch.Text = "Chennai";
                lbl1.Visible = true;
                lbl2.Visible = true;
                lblprepared.Visible = false;
                lblaudit.Visible = false;
                lblauthorised.Visible = false;
                lblapproved.Visible = false;
                lbladdress1.Text = "ACCOUNT STATEMENT FROM";
                lblAgent1.Visible = false;
                lblfromdate.Visible = true;
                lbltodate.Visible = true;
                lblTo.Visible = true;

            }
            else if (tostate == "3")
            {
                lblbranch.Text = "Punabaka";
                lblprepared.Visible = true;
                lblaudit.Visible = true;
                lblauthorised.Visible = true;
                lblapproved.Visible = true;
                lblprepared.Text = "Prepared By";
                lblaudit.Text = "Audit By";
                lblauthorised.Text = "Authorised By";
                lblapproved.Text = "Approved By";
                lbl1.Visible = false;
                lbl2.Visible = false;
                lblAgent.Text = Session["Address"].ToString();
                lblfromdate.Visible = false;
                lbltodate.Visible = false;
                lblTo.Visible = false;
                lbladdress1.Text = "SRI VYSHNAVI DAIRY SPECIALITIES (P) LTD";
                //lbladdress1.te
            }
            else
            {
                lblbranch.Text = "Bangalore";
                lblprepared.Visible = false;
                lblaudit.Visible = false;
                lblauthorised.Visible = false;
                lblapproved.Visible = false;
                lbl1.Visible = true;
                lbl2.Visible = true;
                lbladdress1.Text = "ACCOUNT STATEMENT FROM";
                lblAgent1.Visible = false;
                lblfromdate.Visible = true;
                lbltodate.Visible = true;
                lblTo.Visible = true;
            }
            DataView view = new DataView(dtInvoice);
            DataTable distincttable = view.ToTable(true, "description");
            foreach (DataRow drinv in distincttable.Rows)
            {
                Report.Columns.Add(drinv["description"].ToString()).DataType = typeof(Double);
            }
            Report.Columns.Add("Amount"); ;
            DataTable distinctdate1 = view.ToTable(true, "IndentDate");
            distinctdate1.DefaultView.Sort = "IndentDate ASC";
            DataView distinctview = distinctdate1.DefaultView;
            DataTable distinctdate = distinctview.ToTable();
            if (dtInvoice.Rows.Count > 0)
            {
                double gtamount = 0;
                foreach (DataRow drdistinct in distinctdate.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    DateTime date = Convert.ToDateTime(drdistinct["IndentDate"].ToString());
                    newrow["Date"] = date.AddDays(1).ToString("dd/MM/yyyy");
                    double gtotalamount1 = 0;
                    string temppo = ""; string tempgrn = "";
                    string invoiceno = "";
                    foreach (DataRow dr in dtInvoice.Rows)
                    {
                        if (dr["igst"].ToString() == "0")
                        {
                            if (dr["IndentDate"].ToString() == drdistinct["IndentDate"].ToString())
                            {
                                DateTime idate = Convert.ToDateTime(dr["IndentDate"].ToString());
                                string I_Date = idate.ToString("dd/MM/yyyy");
                                int i = 1;
                                cmd = new MySqlCommand("SELECT MAX(agentdcno) as agentdcno FROM  agentdc WHERE (BranchID = @BranchID) AND (IndDate BETWEEN @d1 AND @d2) and (agentdcno>0)");
                                cmd.Parameters.AddWithValue("@BranchID", AgentId);
                                cmd.Parameters.AddWithValue("@d1", GetLowDate(idate));
                                cmd.Parameters.AddWithValue("@d2", GetLowDate(idate));
                                DataTable dtnDc = vdm.SelectQuery(cmd).Tables[0];
                                if (dtnDc.Rows.Count > 0)
                                {
                                    DcNo = dtnDc.Rows[0]["agentdcno"].ToString();
                                }
                                else
                                {
                                    int taxval = 1;
                                    DataRow[] drInvoice = dtInvoice.Select("IGST<'" + taxval + "'");
                                    if (drInvoice.Length > 0)
                                    {
                                        if (ServerDateCurrentdate.ToString("dd/MM/yyyy") == fromdate.ToString("dd/MM/yyyy"))
                                        {
                                            cmd = new MySqlCommand("SELECT IFNULL(MAX(agentdcno), 0) + 1 AS Sno FROM agentdc WHERE (soid = @soid) AND (IndDate BETWEEN @d1 AND @d2)");
                                            cmd.Parameters.AddWithValue("@soid", ddlSalesOfficesumary.SelectedValue);
                                            cmd.Parameters.AddWithValue("@companycode", companycode);
                                            cmd.Parameters.AddWithValue("@d1", GetLowDate(dtapril.AddDays(-1)));
                                            cmd.Parameters.AddWithValue("@d2", GetHighDate(dtmarch.AddDays(-1)));
                                            DataTable dtadcno = vdm.SelectQuery(cmd).Tables[0];
                                            string agentdcNo = dtadcno.Rows[0]["Sno"].ToString();
                                            cmd = new MySqlCommand("Insert Into Agentdc (BranchId,IndDate,agentdcno,soid,stateid,companycode,moduleid,doe) Values(@BranchId,@IndDate,@agentdcno,@soid,@stateid,@companycode,@moduleid,@doe)");
                                            cmd.Parameters.AddWithValue("@BranchId", AgentId);
                                            cmd.Parameters.AddWithValue("@IndDate", GetLowDate(fromdate).AddDays(-1));
                                            cmd.Parameters.AddWithValue("@agentdcno", agentdcNo);
                                            cmd.Parameters.AddWithValue("@soid", ddlSalesOfficesumary.SelectedValue);
                                            cmd.Parameters.AddWithValue("@stateid", stateid);
                                            cmd.Parameters.AddWithValue("@companycode", companycode);
                                            cmd.Parameters.AddWithValue("@doe", ServerDateCurrentdate);
                                            cmd.Parameters.AddWithValue("@moduleid", Session["moduleid"].ToString());
                                            //vdm.insert(cmd);
                                            DcNo = agentdcNo;
                                        }
                                    }
                                }
                                if (DcNo == "")
                                {
                                    int taxval = 1;
                                    DataRow[] drInvoice = dtInvoice.Select("IGST<'" + taxval + "'");
                                    if (drInvoice.Length > 0)
                                    {
                                        if (ServerDateCurrentdate.ToString("dd/MM/yyyy") == fromdate.ToString("dd/MM/yyyy"))
                                        {
                                            cmd = new MySqlCommand("SELECT IFNULL(MAX(agentdcno), 0) + 1 AS Sno FROM agentdc WHERE (soid = @soid) AND (companycode=@companycode) AND (IndDate BETWEEN @d1 AND @d2)");
                                            cmd.Parameters.AddWithValue("@soid", stateid);
                                            cmd.Parameters.AddWithValue("@companycode", companycode);
                                            cmd.Parameters.AddWithValue("@d1", GetLowDate(dtapril.AddDays(-1)));
                                            cmd.Parameters.AddWithValue("@d2", GetHighDate(dtmarch.AddDays(-1)));
                                            DataTable dtadcno = vdm.SelectQuery(cmd).Tables[0];
                                            string agentdcNo = dtadcno.Rows[0]["Sno"].ToString();
                                            cmd = new MySqlCommand("Insert Into Agentdc (BranchId,IndDate,agentdcno,soid,stateid,companycode,moduleid,doe) Values(@BranchId,@IndDate,@agentdcno,@soid,@stateid,@companycode,@moduleid,@doe)");
                                            cmd.Parameters.AddWithValue("@BranchId", AgentId);
                                            cmd.Parameters.AddWithValue("@IndDate", GetLowDate(fromdate).AddDays(-1));
                                            cmd.Parameters.AddWithValue("@agentdcno", agentdcNo);
                                            cmd.Parameters.AddWithValue("@soid", ddlSalesOfficesumary.SelectedValue);
                                            cmd.Parameters.AddWithValue("@stateid", stateid);
                                            cmd.Parameters.AddWithValue("@companycode", companycode);
                                            cmd.Parameters.AddWithValue("@doe", ServerDateCurrentdate);
                                            cmd.Parameters.AddWithValue("@moduleid", Session["moduleid"].ToString());
                                            //vdm.insert(cmd);
                                            DcNo = agentdcNo;
                                        }
                                    }
                                }
                                string DCNO = "0";
                                int countdc = 0;
                                int.TryParse(DcNo, out countdc);
                                if (countdc <= 10)
                                {
                                    DCNO = "0000" + countdc;
                                }
                                if (countdc >= 10 && countdc <= 99)
                                {
                                    DCNO = "000" + countdc;
                                }
                                if (countdc >= 99 && countdc <= 999)
                                {
                                    DCNO = "00" + countdc;
                                }
                                if (countdc > 999 && countdc <= 9999)
                                {
                                    DCNO = "0" + countdc;
                                }
                                if (countdc > 9999)
                                {
                                    DCNO = "" + countdc;
                                }
                                float qty = 0; float rate = 0;
                                if (AgentId == "7804" && dr["ProductSno"].ToString() == "240")
                                {
                                    float tempqty = 0;
                                    float.TryParse(dr["DeliveryQty"].ToString(), out tempqty);
                                    float temprate = 0;
                                    float.TryParse(dr["Unitcost"].ToString(), out temprate);
                                    qty = tempqty / 6;
                                    rate = temprate * 6;
                                }
                                else
                                {
                                    float.TryParse(dr["DeliveryQty"].ToString(), out qty);
                                    float.TryParse(dr["Unitcost"].ToString(), out rate);
                                }
                                double sgst = 0;
                                double sgstamount = 0;
                                double cgst = 0;
                                double cgstamount = 0;
                                double Igst = 0;
                                double Igstamount = 0;
                                double totRate = 0;
                                double.TryParse(dr["Igst"].ToString(), out Igst);
                                double Igstcon = 100 + Igst;
                                Igstamount = (rate / Igstcon) * Igst;
                                Igstamount = Math.Round(Igstamount, 2);
                                totRate = Igstamount;
                                double Vatrate = rate - totRate;
                                Vatrate = Math.Round(Vatrate, 2);
                                double PAmount = qty * Vatrate;
                                double tot_vatamount = (PAmount * Igst) / 100;
                                if (fromstate == tostate)
                                {
                                    sgstamount = (tot_vatamount / 2);
                                    sgstamount = Math.Round(sgstamount, 2);
                                    cgstamount = (tot_vatamount / 2);
                                    cgstamount = Math.Round(cgstamount, 2);
                                }
                                else
                                {
                                    tot_vatamount = Math.Round(tot_vatamount, 2);
                                }
                                double tot_amount = PAmount + tot_vatamount;
                                tot_amount = Math.Round(tot_amount, 2);
                                gtotalamount1 += tot_amount;
                                invoiceno = dtbrnchaddress.Rows[0]["BranchCode"].ToString() + "/" + dtapril.ToString("yy") + "- " + dtmarch.ToString("yy") + "N/" + DCNO + ",";
                                if (dr["pono"].ToString() != "")
                                {
                                    temppo += dr["pono"].ToString() + ",";
                                    tempgrn += dr["pono"].ToString() + ",";
                                    //tempinvoice += dr["pono"].ToString() + ",";
                                    newrow["PONo"] = dr["pono"].ToString();
                                    newrow["GRN No"] = dr["grnno"].ToString();
                                }
                                if (dr["quatationno"].ToString() != "")
                                {
                                    newrow["DcNumber"] = dr["quatationno"].ToString();
                                }
                                else
                                {
                                    if (tostate == "3")
                                    {
                                        newrow["DcNumber"] = dr["DTripId"].ToString();
                                    }
                                    else
                                    {
                                        newrow["DcNumber"] = dr["IndentNo"].ToString();
                                    }
                                
                                }
                                //newrow["InvoiceNo"] = DcNo + "  ";
                                newrow[dr["description"].ToString()] = Math.Round(qty, 2);
                            }
                        }
                        else
                        {
                            if (dr["IndentDate"].ToString() == drdistinct["IndentDate"].ToString())
                            {
                                DateTime idate = Convert.ToDateTime(dr["IndentDate"].ToString());
                                string I_Date = idate.ToString("dd/MM/yyyy");
                                int i = 1;
                                cmd = new MySqlCommand("SELECT MAX(agentdcno) as agentdcno FROM  agenttaxdc WHERE (BranchID = @BranchID) AND (IndDate BETWEEN @d1 AND @d2) and (agentdcno>0)");
                                cmd.Parameters.AddWithValue("@BranchID", AgentId);
                                cmd.Parameters.AddWithValue("@d1", GetLowDate(idate));
                                cmd.Parameters.AddWithValue("@d2", GetLowDate(idate));
                                DataTable dtnDc = vdm.SelectQuery(cmd).Tables[0];
                                if (dtnDc.Rows.Count > 0)
                                {
                                    DcNo = dtnDc.Rows[0]["agentdcno"].ToString();
                                }
                                else
                                {
                                    int taxval = 1;
                                    DataRow[] drInvoice = dtInvoice.Select("IGST<'" + taxval + "'");
                                    if (drInvoice.Length > 0)
                                    {
                                        if (ServerDateCurrentdate.ToString("dd/MM/yyyy") == fromdate.ToString("dd/MM/yyyy"))
                                        {
                                            cmd = new MySqlCommand("SELECT IFNULL(MAX(agentdcno), 0) + 1 AS Sno FROM agenttaxdc WHERE (soid = @soid) AND (IndDate BETWEEN @d1 AND @d2)");
                                            cmd.Parameters.AddWithValue("@soid", ddlSalesOfficesumary.SelectedValue);
                                            cmd.Parameters.AddWithValue("@companycode", companycode);
                                            cmd.Parameters.AddWithValue("@d1", GetLowDate(dtapril.AddDays(-1)));
                                            cmd.Parameters.AddWithValue("@d2", GetHighDate(dtmarch.AddDays(-1)));
                                            DataTable dtadcno = vdm.SelectQuery(cmd).Tables[0];
                                            string agentdcNo = dtadcno.Rows[0]["Sno"].ToString();
                                            cmd = new MySqlCommand("Insert Into agenttaxdc (BranchId,IndDate,agentdcno,soid,stateid,companycode,moduleid,doe) Values(@BranchId,@IndDate,@agentdcno,@soid,@stateid,@companycode,@moduleid,@doe)");
                                            cmd.Parameters.AddWithValue("@BranchId", AgentId);
                                            cmd.Parameters.AddWithValue("@IndDate", GetLowDate(fromdate).AddDays(-1));
                                            cmd.Parameters.AddWithValue("@agentdcno", agentdcNo);
                                            cmd.Parameters.AddWithValue("@soid", ddlSalesOfficesumary.SelectedValue);
                                            cmd.Parameters.AddWithValue("@stateid", stateid);
                                            cmd.Parameters.AddWithValue("@companycode", companycode);
                                            cmd.Parameters.AddWithValue("@doe", ServerDateCurrentdate);
                                            cmd.Parameters.AddWithValue("@moduleid", Session["moduleid"].ToString());
                                            //vdm.insert(cmd);
                                            DcNo = agentdcNo;
                                        }
                                    }
                                }
                                if (DcNo == "")
                                {
                                    int taxval = 1;
                                    DataRow[] drInvoice = dtInvoice.Select("IGST<'" + taxval + "'");
                                    if (drInvoice.Length > 0)
                                    {
                                        if (ServerDateCurrentdate.ToString("dd/MM/yyyy") == fromdate.ToString("dd/MM/yyyy"))
                                        {
                                            cmd = new MySqlCommand("SELECT IFNULL(MAX(agentdcno), 0) + 1 AS Sno FROM agenttaxdc WHERE (soid = @soid) AND (companycode=@companycode) AND (IndDate BETWEEN @d1 AND @d2)");
                                            cmd.Parameters.AddWithValue("@soid", stateid);
                                            cmd.Parameters.AddWithValue("@companycode", companycode);
                                            cmd.Parameters.AddWithValue("@d1", GetLowDate(dtapril.AddDays(-1)));
                                            cmd.Parameters.AddWithValue("@d2", GetHighDate(dtmarch.AddDays(-1)));
                                            DataTable dtadcno = vdm.SelectQuery(cmd).Tables[0];
                                            string agentdcNo = dtadcno.Rows[0]["Sno"].ToString();
                                            cmd = new MySqlCommand("Insert Into agenttaxdc (BranchId,IndDate,agentdcno,soid,stateid,companycode,moduleid,doe) Values(@BranchId,@IndDate,@agentdcno,@soid,@stateid,@companycode,@moduleid,@doe)");
                                            cmd.Parameters.AddWithValue("@BranchId", AgentId);
                                            cmd.Parameters.AddWithValue("@IndDate", GetLowDate(fromdate).AddDays(-1));
                                            cmd.Parameters.AddWithValue("@agentdcno", agentdcNo);
                                            cmd.Parameters.AddWithValue("@soid", ddlSalesOfficesumary.SelectedValue);
                                            cmd.Parameters.AddWithValue("@stateid", stateid);
                                            cmd.Parameters.AddWithValue("@companycode", companycode);
                                            cmd.Parameters.AddWithValue("@doe", ServerDateCurrentdate);
                                            cmd.Parameters.AddWithValue("@moduleid", Session["moduleid"].ToString());
                                            //vdm.insert(cmd);
                                            DcNo = agentdcNo;
                                        }
                                    }
                                }
                                string DCNO = "0";
                                int countdc = 0;
                                int.TryParse(DcNo, out countdc);
                                if (countdc <= 10)
                                {
                                    DCNO = "0000" + countdc;
                                }
                                if (countdc >= 10 && countdc <= 99)
                                {
                                    DCNO = "000" + countdc;
                                }
                                if (countdc >= 99 && countdc <= 999)
                                {
                                    DCNO = "00" + countdc;
                                }
                                if (countdc > 999 && countdc <= 9999)
                                {
                                    DCNO = "0" + countdc;
                                }
                                if (countdc > 9999)
                                {
                                    DCNO = "" + countdc;
                                }
                                float qty = 0;
                                float.TryParse(dr["DeliveryQty"].ToString(), out qty);
                                float rate = 0;
                                float.TryParse(dr["Unitcost"].ToString(), out rate);
                                double sgst = 0;
                                double sgstamount = 0;
                                double cgst = 0;
                                double cgstamount = 0;
                                double Igst = 0;
                                double Igstamount = 0;
                                double totRate = 0;
                                double.TryParse(dr["Igst"].ToString(), out Igst);
                                double Igstcon = 100 + Igst;
                                Igstamount = (rate / Igstcon) * Igst;
                                Igstamount = Math.Round(Igstamount, 2);
                                totRate = Igstamount;
                                double Vatrate = rate - totRate;
                                Vatrate = Math.Round(Vatrate, 2);
                                double PAmount = qty * Vatrate;
                                double tot_vatamount = (PAmount * Igst) / 100;
                                if (fromstate == tostate)
                                {
                                    sgstamount = (tot_vatamount / 2);
                                    sgstamount = Math.Round(sgstamount, 2);
                                    cgstamount = (tot_vatamount / 2);
                                    cgstamount = Math.Round(cgstamount, 2);
                                }
                                else
                                {
                                    tot_vatamount = Math.Round(tot_vatamount, 2);
                                }
                                double tot_amount = PAmount + tot_vatamount;
                                tot_amount = Math.Round(tot_amount, 2);
                                gtotalamount1 += tot_amount;
                                invoiceno = dtbrnchaddress.Rows[0]["BranchCode"].ToString() + "/" + dtapril.ToString("yy") + "- " + dtmarch.ToString("yy") + "T/" + DCNO + ",";
                                if (dr["pono"].ToString() != "")
                                {
                                    temppo += dr["pono"].ToString() + ",";
                                    tempgrn += dr["pono"].ToString() + ",";
                                    //tempinvoice += dr["pono"].ToString() + ",";
                                    newrow["PONo"] = dr["pono"].ToString();
                                    newrow["GRN No"] = dr["grnno"].ToString();
                                }
                                if (dr["quatationno"].ToString() != "")
                                {
                                    newrow["DcNumber"] = dr["quatationno"].ToString();
                                }
                                else
                                {
                                    if (tostate == "3")
                                    {
                                        newrow["DcNumber"] = dr["DTripId"].ToString();
                                    }
                                    else
                                    {
                                        newrow["DcNumber"] = dr["IndentNo"].ToString();
                                    }
                                }
                                //newrow["InvoiceNo"] = DcNo + "  ";
                                newrow[dr["description"].ToString()] = Math.Round(qty, 2);
                            }
                        }
                    }
                    gtamount += gtotalamount1;
                    //newrow["PONo"] = temppo;
                    //newrow["GRN No"] = tempgrn;
                    newrow["InvoiceNo"] = invoiceno + ",";
                    newrow["Amount"] = gtotalamount1.ToString("#,##0.00");// gtotalamount1;
                    Report.Rows.Add(newrow);
                }
                dtreport.Columns.Add("BrandName");
                dtreport.Columns.Add("TotalQty");
                dtreport.Columns.Add("Rate");
                dtreport.Columns.Add("Amount");
                int j = 1;
                double gtotalqty = 0;
                double gtotalamount = 0;
                double gtotalprice = 0;
                if (dtInvoice.Rows.Count > 0)
                {
                    foreach (DataRow drdistinct in distincttable.Rows)
                    {
                        double TotAmount = 0;
                        double Totalqty = 0;
                        double price = 0;
                        DataRow newrow = dtreport.NewRow();
                        newrow["BrandName"] = drdistinct["description"].ToString();
                        foreach (DataRow dr in dtInvoice.Select("description='" + drdistinct["description"].ToString() + "'"))
                        {
                            float qty = 0;
                            float.TryParse(dr["DeliveryQty"].ToString(), out qty);
                            float rate = 0;
                            float.TryParse(dr["Unitcost"].ToString(), out rate);
                            double sgst = 0;
                            double sgstamount = 0;
                            double cgst = 0;
                            double cgstamount = 0;
                            double Igst = 0;
                            double Igstamount = 0;
                            double totRate = 0;
                            double.TryParse(dr["Igst"].ToString(), out Igst);
                            double Igstcon = 100 + Igst;
                            Igstamount = (rate / Igstcon) * Igst;
                            Igstamount = Math.Round(Igstamount, 2);
                            totRate = Igstamount;
                            double Vatrate = rate - totRate;
                            Vatrate = Math.Round(Vatrate, 2);
                            double PAmount = qty * Vatrate;
                            double tot_vatamount = (PAmount * Igst) / 100;
                            if (fromstate == tostate)
                            {
                                sgstamount = (tot_vatamount / 2);
                                sgstamount = Math.Round(sgstamount, 2);
                                cgstamount = (tot_vatamount / 2);
                                cgstamount = Math.Round(cgstamount, 2);
                            }
                            else
                            {
                                tot_vatamount = Math.Round(tot_vatamount, 2);
                            }
                            double tot_amount = PAmount + tot_vatamount;
                            tot_amount = Math.Round(tot_amount, 2);
                            TotAmount += tot_amount;
                            Totalqty += qty;
                            price = rate;
                        }
                        gtotalqty += Totalqty;
                        gtotalamount += TotAmount;
                        gtotalprice = price;
                        newrow["Rate"] = Math.Round(gtotalprice, 2) ;
                        newrow["TotalQty"] = Math.Round(Totalqty, 2);
                        newrow["Amount"] = TotAmount.ToString("#,##0.00");// Math.Round(TotAmount, 2);
                        dtreport.Rows.Add(newrow);
                    }
                    DataRow newrow1 = dtreport.NewRow();
                    newrow1["BrandName"] = "Total";
                    //newrow1["Price"] = Math.Round(gtotalprice, 2);
                    //newrow1["Qty"] = Math.Round(gtotalqty, 2);
                    double amount1 = Math.Round(gtotalamount, 2);
                    newrow1["Amount"] = amount1.ToString("#,##0.00"); ;
                    dtreport.Rows.Add(newrow1);
                }

                foreach (var column in Report.Columns.Cast<DataColumn>().ToArray())
                {
                    if (Report.AsEnumerable().All(dr => dr.IsNull(column)))
                        Report.Columns.Remove(column);
                }

                DataRow newvartical = Report.NewRow();
                newvartical["Date"] = "Total";
                newvartical["Amount"] = gtamount.ToString("#,##0.00");
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
                grdReports1.DataSource = dtreport;
                grdReports1.DataBind();
                grdReports.DataSource = Report;
                grdReports.DataBind();
                grdReports.Rows[0].Cells[4].Width = 140;
                //string Amont = Grandtotal;
                int amount = 0;
                amount = Convert.ToInt32(gtotalamount);
                string[] Ones = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Ninteen" };
                string[] Tens = { "One", "Ten", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninty" };
                int Num = amount;
                lblReceived.Text = NumToWordBD(Num) + " Rupees Only";
            }
            else
            {
                PanelHide.Visible = true;
                Panel_button.Visible = true;
                grdReports1.DataSource = dtreport;
                grdReports1.DataBind();
                grdReports.DataSource = Report;
                grdReports.DataBind();
                lblmsg.Text = "No Data Found";
            }
            
        }
        catch (Exception ex)
        {

        }
    }
    public static string NumToWordBD(Int64 Num)
    {
        string[] Below20 = { "", "One ", "Two ", "Three ", "Four ", 
      "Five ", "Six " , "Seven ", "Eight ", "Nine ", "Ten ", "Eleven ", 
    "Twelve " , "Thirteen ", "Fourteen ","Fifteen ", 
      "Sixteen " , "Seventeen ","Eighteen " , "Nineteen " };
        string[] Below100 = { "", "", "Twenty ", "Thirty ", 
      "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
        string InWords = "";
        if (Num >= 1 && Num < 20)
            InWords += Below20[Num];
        if (Num >= 20 && Num <= 99)
            InWords += Below100[Num / 10] + Below20[Num % 10];
        if (Num >= 100 && Num <= 999)
            InWords += NumToWordBD(Num / 100) + " Hundred " + NumToWordBD(Num % 100);
        if (Num >= 1000 && Num <= 99999)
            InWords += NumToWordBD(Num / 1000) + " Thousand " + NumToWordBD(Num % 1000);
        if (Num >= 100000 && Num <= 9999999)
            InWords += NumToWordBD(Num / 100000) + " Lakh " + NumToWordBD(Num % 100000);
        if (Num >= 10000000)
            InWords += NumToWordBD(Num / 10000000) + " Crore " + NumToWordBD(Num % 10000000);
        return InWords;
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
            lblmsg.Visible = true;
        }
        //finally
        //{
        //    Response.End();
        //}
    }
    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Check your condition here, Cells[1] for ex. is DONE/Not Done column
            string val = e.Row.Cells[1].Text;
            if (string.IsNullOrEmpty(val))
            {
                grdReports.Columns[1].Visible = false;
            }
            string val1 = e.Row.Cells[2].Text;
            if (string.IsNullOrEmpty(val1))
            {
                grdReports.Columns[2].Visible = false;
            }
        }
    }
}
