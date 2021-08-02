using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Globalization;
using System.Drawing;

public partial class AgentDelivery : System.Web.UI.Page
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
                //lbl_Title.Text = Session["TitleName"].ToString();
            }
        }
    }
    void FillDispName()
    {
        try
        {
            string salestype = Session["salestype"].ToString();

            vdm = new VehicleDBMgr();
            cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
            cmd.Parameters.AddWithValue("@BranchD", Session["branch"].ToString());
            DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
            ddlRouteName.DataSource = dtRoutedata;
            ddlRouteName.DataTextField = "DispName";
            ddlRouteName.DataValueField = "sno";
            ddlRouteName.DataBind();
            ddlRouteName.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void ddlroutename_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch)");
        cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutes ON dispatch_sub.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (dispatch.sno = @dispsno) AND (dispatch.flag = 1) ");
        cmd.Parameters.AddWithValue("@dispsno", ddlRouteName.SelectedValue);
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlagentname.DataSource = dtRoutedata;
        ddlagentname.DataTextField = "BranchName";
        ddlagentname.DataValueField = "sno";
        ddlagentname.DataBind();
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
            //  lblDate.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            //lblDispatchName.Text = ddlagentname.SelectedItem.Text;
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
            Session["RouteName"] = ddlagentname.SelectedItem.Text;
            cmd = new MySqlCommand("SELECT  branchdata.stateid, branchdata.SalesType, branchdata.BranchName, branchdata.TinNumber, branchdata.Address, branchdata.tbranchname, branchdata.sno, statemastar.statename, statemastar.gststatecode, branchdata.gstno, branchdata.email, branchdata.phonenumber, branchdata.doorno, branchdata.area, branchdata.street, branchdata.city, branchdata.mandal, branchdata.district, branchdata.pincode FROM branchdata INNER JOIN statemastar ON branchdata.stateid = statemastar.sno where branchdata.sno=@sno");
            cmd.Parameters.AddWithValue("@sno", ddlagentname.SelectedValue);
            DataTable dtAddress = vdm.SelectQuery(cmd).Tables[0];
            string AgentAddress = "";
            string AgentName = "";
            string salestype = "";
            string buyerTinNumber = "";
            if (dtAddress.Rows.Count > 0)
            {
                AgentAddress = dtAddress.Rows[0]["doorno"].ToString() + "," + dtAddress.Rows[0]["street"].ToString() + "," + dtAddress.Rows[0]["area"].ToString() + "," + dtAddress.Rows[0]["city"].ToString() + "," + dtAddress.Rows[0]["mandal"].ToString() + "," + dtAddress.Rows[0]["district"].ToString() + "," + dtAddress.Rows[0]["pincode"].ToString();// dtAddress.Rows[0]["Address"].ToString();
                AgentName = dtAddress.Rows[0]["BranchName"].ToString();
                salestype = dtAddress.Rows[0]["salestype"].ToString();
                buyerTinNumber = dtAddress.Rows[0]["TinNumber"].ToString();
            }
            cmd = new MySqlCommand("SELECT branchdata.companycode, branchdata.phonenumber,branchdata.email, branchdata.sno,branchdata.stateid, branchdata.Address, branchdata.TinNumber, branchdata.panno, branchdata.BranchCode,statemastar.statecode, statemastar.statename, statemastar.gststatecode, branchdata.phonenumber, branchdata.emailid,  branchdata.street, branchdata.city, branchdata.mandal, branchdata.district, branchdata.pincode, branchdata.gstno, branchdata.doorno, branchdata.area FROM branchdata INNER JOIN statemastar ON branchdata.stateid = statemastar.sno WHERE (branchdata.sno = @branchsno)");
            cmd.Parameters.AddWithValue("@branchsno", Session["branch"].ToString());
            DataTable dtbrnchaddress = vdm.SelectQuery(cmd).Tables[0];
            //string BranchAddress = dtbrnchaddress.Rows[0]["Address"].ToString();
            //companycode = dtbrnchaddress.Rows[0]["companycode"].ToString();
            //stateid = dtbrnchaddress.Rows[0]["gststatecode"].ToString();
            //string BranchId = Session["branch"].ToString();
            //string fromstate = dtbrnchaddress.Rows[0]["stateid"].ToString();
            string address = dtbrnchaddress.Rows[0]["doorno"].ToString() + "," + dtbrnchaddress.Rows[0]["street"].ToString() + "," + dtbrnchaddress.Rows[0]["area"].ToString() + "," + dtbrnchaddress.Rows[0]["city"].ToString() + "," + dtbrnchaddress.Rows[0]["mandal"].ToString() + "," + dtbrnchaddress.Rows[0]["district"].ToString() + "," + dtbrnchaddress.Rows[0]["pincode"].ToString() + "," + dtbrnchaddress.Rows[0]["phonenumber"].ToString() + "," + dtbrnchaddress.Rows[0]["emailid"].ToString();
            //obj1.BranchAddress = address;
            //obj1.city = dtbrnchaddress.Rows[0]["city"].ToString();
            //obj1.invoicedate = fromdate.ToString("dd/MM/yyyy");
            //obj1.frmstatename = dtbrnchaddress.Rows[0]["statename"].ToString();
            //obj1.frmstatecode = dtbrnchaddress.Rows[0]["gststatecode"].ToString();
            //obj1.fromgstn = dtbrnchaddress.Rows[0]["gstno"].ToString();
            //tostate = dtAddress.Rows[0]["stateid"].ToString();
            //obj1.phoneno = dtAddress.Rows[0]["phonenumber"].ToString();
            //obj1.email = dtAddress.Rows[0]["email"].ToString();
            //obj1.companyphone = dtbrnchaddress.Rows[0]["phonenumber"].ToString();
            //obj1.companyemail = dtbrnchaddress.Rows[0]["email"].ToString();
            //lblFromAdress.Text = address;
            lbladdress.Text = address;
            lblToaddress.Text = AgentAddress;
            lbl_AgentName.Text = AgentName;
            lbl_dcdate.Text = fromdate.ToString("dd/MM/yyyy");
            cmd = new MySqlCommand("SELECT   indents.IndentNo, indents.Branch_id, indents.I_date, indents.IndentType,  indents_subtable.DTripId FROM   indents INNER JOIN  indents_subtable ON indents.IndentNo = indents_subtable.IndentNo WHERE  (indents.Branch_id = @brnchid) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY indents_subtable.DTripId");
            cmd.Parameters.AddWithValue("@brnchid", ddlagentname.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate).AddDays(-1));
            DataTable dtallindents = vdm.SelectQuery(cmd).Tables[0];
            lbl_dcno.Text = dtallindents.Rows[0]["IndentNo"].ToString();
            cmd = new MySqlCommand("SELECT invmaster.InvName, invtransactions12.Qty, invtransactions12.DOE, invtransactions12.FromTran, invtransactions12.ToTran FROM invmaster INNER JOIN invtransactions12 ON invmaster.sno = invtransactions12.B_inv_sno WHERE (invtransactions12.ToTran = @agentid) AND (invtransactions12.DOE BETWEEN @d1 AND @d2)");
            cmd.Parameters.AddWithValue("@agentid", ddlagentname.SelectedValue);
             cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate).AddDays(-1));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate).AddDays(-1));
            DataTable dtInventory = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT productsdata.sno, productsdata.ProductName, productsdata.Units, branchproducts.unitprice, invmaster.Qty, products_category.Categoryname, branchproducts.unitprice AS BUnitPrice FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN invmaster ON productsdata.Inventorysno = invmaster.sno INNER JOIN branchdata ON branchproducts.branch_sno = branchdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchdata.sno = @agentid) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
            cmd.Parameters.AddWithValue("@agentid", ddlagentname.SelectedValue);
            DataTable dtbrnchprdts = vdm.SelectQuery(cmd).Tables[0];
            Report = new DataTable();
            Report.Columns.Add("SNo");
            Report.Columns.Add("Product Name");
            Report.Columns.Add("Order Qty");
            Report.Columns.Add("Delivery Qty");
            DataTable dtalldelivery = new DataTable();
            int i = 1;
            foreach (DataRow dr in dtallindents.Rows)
            {
                //DataRow drnew = Report.NewRow();
                //drnew["SNo"] = i;
                //Report.Rows.Add(drnew);
                cmd = new MySqlCommand("SELECT indents_subtable.DeliveryQty, indents_subtable.unitQty, productsdata.ProductName,productsdata.sno AS ProductId FROM indents_subtable INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno WHERE (indents_subtable.IndentNo = @indno)");
                cmd.Parameters.AddWithValue("@indno", dr["IndentNo"].ToString());
                dtalldelivery = vdm.SelectQuery(cmd).Tables[0];
                float torderqty = 0; float tqty = 0;
                if (dtalldelivery.Rows.Count > 0)
                {
                    //foreach (DataRow dralldelivery in dtalldelivery.Rows)
                    //{

                    //    DataRow drnewdelivery = Report.NewRow();
                    //    drnewdelivery["SNo"] = i;
                    //    drnewdelivery["Product Name"] = dralldelivery["ProductName"].ToString();
                    //    drnewdelivery["Order Qty"] = dralldelivery["unitQty"].ToString();
                    //    drnewdelivery["Delivery Qty"] = dralldelivery["DeliveryQty"].ToString();
                    //    //Report.Rows.Add(drnewdelivery);
                    //    i++;
                    //}
                   
                    foreach (DataRow drprdt in dtbrnchprdts.Rows)
                    {
                        foreach (DataRow drr in dtalldelivery.Rows)
                        {
                            if (drr["ProductId"].ToString() == drprdt["sno"].ToString())
                            {
                                DataRow drinvdelivery = Report.NewRow();
                                float qty = 0;
                                float orderqty = 0;
                                float.TryParse(drr["DeliveryQty"].ToString(), out qty);
                                float.TryParse(drr["unitQty"].ToString(), out orderqty);
                                torderqty += orderqty;
                                tqty += qty;
                                
                                string Categoryname = drprdt["Categoryname"].ToString();
                                drinvdelivery["SNo"] = i;
                                drinvdelivery["Product Name"] = drr["ProductName"].ToString();
                                drinvdelivery["Order Qty"] = drr["unitQty"].ToString();
                                drinvdelivery["Delivery Qty"] = drr["DeliveryQty"].ToString();
                                Report.Rows.Add(drinvdelivery);
                                i++;
                            }
                        }
                    }
                    DataRow drinvdelivery1 = Report.NewRow();
                    drinvdelivery1["Product Name"] = "Total";
                    drinvdelivery1["Order Qty"] = torderqty;
                    drinvdelivery1["Delivery Qty"] = tqty;
                    Report.Rows.Add(drinvdelivery1);
                    DataRow drinvdelivery2 = Report.NewRow();
                    Report.Rows.Add(drinvdelivery2);
                    foreach (DataRow drinv in dtInventory.Rows)
                    {
                        DataRow dridelivery = Report.NewRow();
                        //dridelivery["Product Name"] = "Inventory";
                        dridelivery["Product Name"] = drinv["InvName"].ToString();
                        int qty = 0;
                        int.TryParse(drinv["Qty"].ToString(), out qty);
                        if (qty > 0)
                        {
                            dridelivery["Order Qty"] = drinv["Qty"].ToString();
                        }
                        Report.Rows.Add(dridelivery);
                    }
                }
            }
            i++;
            grdReports.DataSource = Report;
            grdReports.DataBind();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}