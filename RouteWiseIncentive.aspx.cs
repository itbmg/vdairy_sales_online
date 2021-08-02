using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;
using ClosedXML.Excel;
using System.IO;

public partial class RouteWiseIncentive : System.Web.UI.Page
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
                lblTitle.Text = Session["TitleName"].ToString();
                //lblDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                FillAgentName();

            }
        }
    }
    void FillAgentName()
    {

        vdm = new VehicleDBMgr();
        try
        {
            vdm = new VehicleDBMgr();
            if (Session["salestype"].ToString() == "Plant")
            {
                PBranch.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch)");
                cmd.Parameters.AddWithValue("@SuperBranch", Session["branch"]);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
            }
            else
            {
                PBranch.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) OR (branchdata.sno = @BranchID)");
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
            }
        }
        catch
        {
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
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            vdm = new VehicleDBMgr();
            DataTable Report = new DataTable();
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
            Session["filename"] = "SalesOffice wise Activity Report ->" + ddlSalesOffice.SelectedItem.Text;
            lblAgent.Text = ddlSalesOffice.SelectedItem.Text;
            lbl_fromDate.Text = txtFromdate.Text;
            lbl_selttodate.Text = txtTodate.Text;
            cmd = new MySqlCommand("SELECT incentivetransaction.StructureName, product_clubbing.ClubName, incentive_structure.sno, product_clubbing.sno AS clubbingsno FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN (SELECT sno, FromDate, Todate, StructureName, BranchId, EntryDate, ActualDiscount, TotalDiscount, Remarks, structure_sno, leakagepercent FROM incentivetransactions WHERE (FromDate BETWEEN @d1 AND @d2)) incentivetransaction ON branchroutesubtable.BranchID = incentivetransaction.BranchId INNER JOIN incentive_structure ON incentivetransaction.structure_sno = incentive_structure.sno INNER JOIN incentive_struct_sub ON incentive_structure.sno = incentive_struct_sub.is_sno INNER JOIN product_clubbing ON incentive_struct_sub.clubbingID = product_clubbing.sno WHERE (branchroutes.BranchID = @branchid) GROUP BY product_clubbing.ClubName, product_clubbing.sno");
            cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable dtClubbings = vdm.SelectQuery(cmd).Tables[0];

            if (dtClubbings.Rows.Count > 0)
            {
                DataView view = new DataView(dtClubbings);
                DataTable distinctclubbings = view.ToTable(true, "ClubName");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("Route Name");
                int count = 1;
                foreach (DataRow dr in dtClubbings.Rows)
                {

                    Report.Columns.Add(dr["ClubName"].ToString()).DataType = typeof(Double);
                    Report.Columns.Add(dr["ClubName"].ToString() + "Amount").DataType = typeof(Double);
                    Report.Columns.Add(dr["ClubName"].ToString() + "Amount Per Ltr").DataType = typeof(Double);
                }
                Report.Columns.Add("Leakage Amount").DataType = typeof(Double);
                Report.Columns.Add("Other Amount").DataType = typeof(Double);
                Report.Columns.Add("Total Ltrs").DataType = typeof(Double);
                Report.Columns.Add("Total Amount").DataType = typeof(Double);
                Report.Columns.Add("P.L.C");//per ltr cost
            }
            cmd = new MySqlCommand("SELECT branchroutes.RouteName, branchroutes.Sno, branchroutesubtable.BranchID, incentivetransactions.structure_sno, incentivetransactions.ActualDiscount,incentivetransactions.TotalDiscount, incentivetransactions.leakagepercent FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN incentivetransactions ON branchroutesubtable.BranchID = incentivetransactions.BranchId WHERE (branchroutes.BranchID = @branchid) AND (incentivetransactions.FromDate BETWEEN @d1 AND @d2) ORDER BY branchroutes.Sno");
            cmd.Parameters.AddWithValue("@branchid", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
            DataTable dtRouteAgentStructure = vdm.SelectQuery(cmd).Tables[0];

            DataView routeagentsview = new DataView(dtRouteAgentStructure);
            DataTable dtview = routeagentsview.ToTable(true, "RouteName", "Sno");
            int i = 1;
            

            foreach (DataRow drrouteagentstructure in dtview.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["SNo"] = i;
                newrow["Route Name"] = drrouteagentstructure["RouteName"].ToString();
                Report.Rows.Add(newrow);
                float finalleakamt = 0;
                float finalotheramt = 0;
                float finaltotLtr = 0;
                float finaltotLtrAmt = 0;
                float totleakpercent = 0;
                float totActualincentive = 0;
                float totGivenincentive = 0;
                float milkincentive = 0;
                foreach (DataRow dragents in dtRouteAgentStructure.Select("Sno='" + drrouteagentstructure["Sno"].ToString() + "'"))
                {
                    cmd = new MySqlCommand("SELECT productsdata.sno, productsdata.ProductName, product_clubbing.ClubName, incentive_structure.StructureName, product_clubbing.sno AS clubbingsno,products_category.Categoryname, products_subcategory.category_sno FROM incentive_structure INNER JOIN incentive_struct_sub ON incentive_structure.sno = incentive_struct_sub.is_sno INNER JOIN product_clubbing ON incentive_struct_sub.clubbingID = product_clubbing.sno INNER JOIN subproductsclubbing ON product_clubbing.sno = subproductsclubbing.Clubsno INNER JOIN productsdata ON subproductsclubbing.Productid = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (incentive_structure.sno = @StructureID) ");
                    cmd.Parameters.AddWithValue("@StructureID", dragents["structure_sno"].ToString());
                    DataTable dtincentivestructure = vdm.SelectQuery(cmd).Tables[0];

                    DataView incentiveview = new DataView(dtincentivestructure);
                    DataTable dticentive = incentiveview.ToTable(true, "ClubName", "clubbingsno", "category_sno");

                    cmd = new MySqlCommand("SELECT result.deliveryqty, result.ClubName, result.Clubsno, slabs.SlotQty, slabs.Amt FROM (SELECT ROUND(SUM(indents_subtable.DeliveryQty), 2) AS deliveryqty, subproductsclubbing.Clubsno, product_clubbing.ClubName FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN subproductsclubbing ON indents_subtable.Product_sno = subproductsclubbing.Productid INNER JOIN product_clubbing ON subproductsclubbing.Clubsno = product_clubbing.sno WHERE (indents.Branch_id = @selectedbrnch) AND (indents.I_date BETWEEN @d1 AND @d2) GROUP BY subproductsclubbing.Clubsno) result INNER JOIN slabs ON result.Clubsno = slabs.club_sno");
                    cmd.Parameters.AddWithValue("@selectedbrnch", dragents["BranchID"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                    DataTable dtclubtotal = vdm.SelectQuery(cmd).Tables[0];

                    cmd = new MySqlCommand("SELECT SUM(indents_subtable.DeliveryQty) AS totmilk, SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS Amount FROM indents INNER JOIN indents_subtable ON indents.IndentNo = indents_subtable.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN productsdata ON indents_subtable.Product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (branchdata.sno = @BranchID) AND (indents.I_date BETWEEN @d1 AND @d2) AND (products_category.sno = 9)");
                    cmd.Parameters.AddWithValue("@BranchID", dragents["BranchID"].ToString());
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                    DataTable dtdelivered = vdm.SelectQuery(cmd).Tables[0];

                    float totalmilk = 0;
                    float totalmilkamt = 0;
                    float.TryParse(dtdelivered.Rows[0]["totmilk"].ToString(), out totalmilk);
                    float.TryParse(dtdelivered.Rows[0]["Amount"].ToString(), out totalmilkamt);
                    float TotMilkandMilkAmt = 0;
                    TotMilkandMilkAmt = totalmilkamt / totalmilk;

                    float totleakincentive = 0;
                    float totalmilksale = 0;

                    float leakpercent = 0;
                    float Actualincentive = 0;
                    float Givenincentive = 0;
                    float.TryParse(dragents["leakagepercent"].ToString(), out leakpercent);
                    float.TryParse(dragents["ActualDiscount"].ToString(), out Actualincentive);
                    float.TryParse(dragents["TotalDiscount"].ToString(), out Givenincentive);

                    totActualincentive += Actualincentive;
                    totGivenincentive+= Givenincentive;

                    finalotheramt = totGivenincentive - totActualincentive;
                   if (leakpercent != 0)
                   {
                       totalmilksale = leakpercent / 100 * totalmilk;
                       totleakincentive = totalmilksale * TotMilkandMilkAmt;
                   }
                    string clubbingname = "";
                    string categoryserial = "";
                    float count = 0;
                    count = (float)(todate - fromdate.AddDays(-1)).TotalDays;
                    foreach (DataRow drincetiveclub in dticentive.Rows)
                    {
                        float avgsale = 0;
                        float slotqty = 0;
                        float slotamt = 0;
                        float totalsale = 0;
                        string sltamt = "";
                        clubbingname = drincetiveclub["ClubName"].ToString();
                        categoryserial = drincetiveclub["category_sno"].ToString();
                        foreach (DataRow drdtclubtotal in dtclubtotal.Select("Clubsno='" + drincetiveclub["clubbingsno"].ToString() + "'"))
                        {
                            float.TryParse(drdtclubtotal["deliveryqty"].ToString(), out totalsale);
                            avgsale = (totalsale / count);
                            float.TryParse(drdtclubtotal["SlotQty"].ToString(), out slotqty);
                            if (avgsale > slotqty)
                            {
                                float.TryParse(drdtclubtotal["Amt"].ToString(), out slotamt);
                                sltamt = drdtclubtotal["Amt"].ToString();
                            }
                        }

                        

                        float totclubltrs = 0;
                        float totclubAmt = 0;
                        float totclubAmtPerLtr = 0;
                        float tototheramt = 0;
                        float totleakamt = 0;
                        float totLtr = 0;
                        float totLtrAmt = 0;
                        float.TryParse(Report.Rows[i - 1][clubbingname].ToString(), out totclubltrs);
                        float.TryParse(Report.Rows[i - 1][clubbingname + "Amount"].ToString(), out totclubAmt);
                        float.TryParse(Report.Rows[i - 1][clubbingname + "Amount Per Ltr"].ToString(), out totclubAmtPerLtr);
                        float.TryParse(Report.Rows[i - 1]["Leakage Amount"].ToString(), out totleakamt);
                        float.TryParse(Report.Rows[i - 1]["Other Amount"].ToString(), out tototheramt);
                        float.TryParse(Report.Rows[i - 1]["Total Ltrs"].ToString(), out totLtr);
                        float.TryParse(Report.Rows[i - 1]["Total Amount"].ToString(), out totLtrAmt);

                        float finaltotsale = (float) Math.Round(totalsale * slotamt, 2);

                        float finalclubltrs = totclubltrs + totalsale;
                        float finalclubAmt = totclubAmt + finaltotsale;

                        //totActualincentive += Actualincentive;
                        //totGivenincentive += Givenincentive;

                        finalleakamt += totleakincentive;
                        finalotheramt = totGivenincentive - totActualincentive;
                        finaltotLtr += totalsale;
                        finaltotLtrAmt += finaltotsale;
                        float amtperltr = 0;
                        float finalamtperltr = 0;
                        if (finalclubltrs == 0)
                        {
                             amtperltr = 0;
                        }
                        else
                        {
                             amtperltr = (finalclubAmt / finalclubltrs);

                        }
                        if (finaltotLtr == 0)
                        {
                            finalamtperltr = 0;
                        }
                        else
                        {
                            finalamtperltr = (finaltotLtrAmt / finaltotLtr);

                        }

                        Report.Rows[i - 1][clubbingname] = Math.Round(finalclubltrs, 2);
                        Report.Rows[i - 1][clubbingname + "Amount"] = Math.Round(finalclubAmt, 2);

                        Report.Rows[i - 1][clubbingname + "Amount Per Ltr"] = Math.Round(amtperltr, 2);
                        Report.Rows[i - 1]["Leakage Amount"] = Math.Round(finalleakamt, 2);
                        Report.Rows[i - 1]["Other Amount"] = Math.Round(finalotheramt,2);
                        Report.Rows[i - 1]["Total Ltrs"] = Math.Round(finaltotLtr, 2);
                        Report.Rows[i - 1]["Total Amount"] = Math.Round(finaltotLtrAmt, 2);
                        Report.Rows[i - 1]["P.L.C"] = Math.Round(finalamtperltr, 2);
                        //Leakage Amount
                        //Other Amount
                        //Total Ltrs")P.L.C
                        //Total Amount
                        //newrow["ClubbingName"] = clubbingname;
                        //newrow["TotalSale"] = Math.Round(totalsale, 2);
                        //newrow["AverageSale"] = Math.Round(avgsale, 2);
                        //newrow["DiscountSlot"] = sltamt;
                        //newrow["TotalAmount"] = Math.Round(totalsale * slotamt, 2);


                        //Report.Rows.Add(newrow);
                    }
                }
                i++;
            }
            DataRow Break = Report.NewRow();
            Break["Route Name"] = "";
            Report.Rows.Add(Break);
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
            

            grdReports.DataSource = Report;
            grdReports.DataBind();
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