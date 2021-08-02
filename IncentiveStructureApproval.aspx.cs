using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;


public partial class IncentiveStructureApproval : System.Web.UI.Page
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
                ddlSalesOffice.DataSource = dtBranch;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));

            }
            else
            {
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) OR (branchdata.sno = @BranchID)");
                cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
                ddlSalesOffice.Items.Insert(0, new ListItem("Select", "0"));

            }
        }
        catch
        {
        }
    }

    protected void ddlrptType_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();

        if (ddlrptType.Text == "Approved")
        {
            cmd = new MySqlCommand("SELECT sno, StructureName, BranchID, EntryDate, ModifiedDate, ModifySno, Flag, ApprovalStatus FROM incentive_structure WHERE (BranchID = @BranchID) AND (ApprovalStatus = @As)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@As", 'A');

        }
        if (ddlrptType.Text == "Pending")
        {
            cmd = new MySqlCommand("SELECT sno, StructureName, BranchID, EntryDate, ModifiedDate, ModifySno, Flag, ApprovalStatus FROM incentive_structure WHERE (BranchID = @BranchID) AND (ApprovalStatus = @As)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@As", 'P');

        }
        if (ddlrptType.Text == "Rejected")
        {
            cmd = new MySqlCommand("SELECT sno, StructureName, BranchID, EntryDate, ModifiedDate, ModifySno, Flag, ApprovalStatus FROM incentive_structure WHERE (BranchID = @BranchID) AND (ApprovalStatus = @As)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@As", 'R');

        }
        if (ddlrptType.Text == "Raised")
        {
            cmd = new MySqlCommand("SELECT sno, StructureName, BranchID, EntryDate, ModifiedDate, ModifySno, Flag, ApprovalStatus FROM incentive_structure WHERE (BranchID = @BranchID) AND (ApprovalStatus is NULL)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);

        }
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlIncentiveStructure.DataSource = dtRoutedata;
        ddlIncentiveStructure.DataTextField = "StructureName";
        ddlIncentiveStructure.DataValueField = "sno";
        ddlIncentiveStructure.DataBind();
        ddlIncentiveStructure.Items.Insert(0, new ListItem("Select", "0"));
    }
    //void ChangeRoute()
    //{
    //    vdm = new VehicleDBMgr();

    //    if (ddlrptType.Text == "Approved")
    //    {
    //        cmd = new MySqlCommand("SELECT sno, StructureName, BranchID, EntryDate, ModifiedDate, ModifySno, Flag, ApprovalStatus FROM incentive_structure WHERE (BranchID = @BranchID) AND (ApprovalStatus = @As)");
    //        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
    //        cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
    //        cmd.Parameters.AddWithValue("@As", 'A');

    //    }
    //    if (ddlrptType.Text == "Pending")
    //    {
    //        cmd = new MySqlCommand("SELECT sno, StructureName, BranchID, EntryDate, ModifiedDate, ModifySno, Flag, ApprovalStatus FROM incentive_structure WHERE (BranchID = @BranchID) AND (ApprovalStatus = @As)");
    //        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
    //        cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
    //        cmd.Parameters.AddWithValue("@As", 'P');

    //    }
    //    if (ddlrptType.Text == "Rejected")
    //    {
    //        cmd = new MySqlCommand("SELECT sno, StructureName, BranchID, EntryDate, ModifiedDate, ModifySno, Flag, ApprovalStatus FROM incentive_structure WHERE (BranchID = @BranchID) AND (ApprovalStatus = @As)");
    //        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
    //        cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
    //        cmd.Parameters.AddWithValue("@As", 'R');

    //    }
    //    if (ddlrptType.Text == "Raised")
    //    {
    //        cmd = new MySqlCommand("SELECT sno, StructureName, BranchID, EntryDate, ModifiedDate, ModifySno, Flag, ApprovalStatus FROM incentive_structure WHERE (BranchID = @BranchID) AND (ApprovalStatus is NULL)");
    //        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
    //        cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);

    //    }
    //    DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
    //    ddlIncentiveStructure.DataSource = dtRoutedata;
    //    ddlIncentiveStructure.DataTextField = "StructureName";
    //    ddlIncentiveStructure.DataValueField = "sno";
    //    ddlIncentiveStructure.DataBind();
    //    ddlIncentiveStructure.Items.Insert(0, new ListItem("Select", "0"));
    //}
    protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
      
        if (ddlrptType.Text == "Approved")
        {
            cmd = new MySqlCommand("SELECT sno, StructureName, BranchID, EntryDate, ModifiedDate, ModifySno, Flag, ApprovalStatus FROM incentive_structure WHERE (BranchID = @BranchID) AND (ApprovalStatus = @As)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@As", 'A');

        }
        if (ddlrptType.Text == "Pending")
        {
            cmd = new MySqlCommand("SELECT sno, StructureName, BranchID, EntryDate, ModifiedDate, ModifySno, Flag, ApprovalStatus FROM incentive_structure WHERE (BranchID = @BranchID) AND (ApprovalStatus = @As)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@As", 'P');

        }
        if (ddlrptType.Text == "Rejected")
        {
            cmd = new MySqlCommand("SELECT sno, StructureName, BranchID, EntryDate, ModifiedDate, ModifySno, Flag, ApprovalStatus FROM incentive_structure WHERE (BranchID = @BranchID) AND (ApprovalStatus = @As)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@As", 'R');

        }
        if (ddlrptType.Text == "Raised")
        {
            cmd = new MySqlCommand("SELECT sno, StructureName, BranchID, EntryDate, ModifiedDate, ModifySno, Flag, ApprovalStatus FROM incentive_structure WHERE (BranchID = @BranchID) AND (ApprovalStatus is NULL)");
            cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);

        }
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlIncentiveStructure.DataSource = dtRoutedata;
        ddlIncentiveStructure.DataTextField = "StructureName";
        ddlIncentiveStructure.DataValueField = "sno";
        ddlIncentiveStructure.DataBind();
        ddlIncentiveStructure.Items.Insert(0, new ListItem("Select", "0"));

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
            vdm = new VehicleDBMgr();
            DataTable Report = new DataTable();
            lblmsg.Text = "";
            lblStructureName.Text = ddlIncentiveStructure.SelectedItem.Text;
            //cmd = new MySqlCommand("SELECT incentive_structure.StructureName, incentive_structure.sno, incentive_structure.ApprovalStatus,incentive_structure.ApprovedBy, product_clubbing.sno AS clubsno, product_clubbing.ClubName,subproductsclubbing.Productid, productsdata.ProductName FROM incentive_structure INNER JOIN incentive_struct_sub ON incentive_structure.sno = incentive_struct_sub.is_sno INNER JOIN product_clubbing ON incentive_struct_sub.clubbingID = product_clubbing.sno INNER JOIN subproductsclubbing ON product_clubbing.sno = subproductsclubbing.Clubsno INNER JOIN productsdata ON subproductsclubbing.Productid = productsdata.sno WHERE (incentive_structure.sno = @structuresno)");
            cmd = new MySqlCommand("SELECT incentive_structure.StructureName, incentive_structure.sno, incentive_structure.ApprovalStatus, incentive_structure.ApprovedBy, product_clubbing.sno AS clubsno,product_clubbing.ClubName, subproductsclubbing.Productid, productsdata.ProductName, incentive_structure.ApprovedDate, empmanage.EmpName FROM incentive_structure INNER JOIN incentive_struct_sub ON incentive_structure.sno = incentive_struct_sub.is_sno INNER JOIN product_clubbing ON incentive_struct_sub.clubbingID = product_clubbing.sno INNER JOIN subproductsclubbing ON product_clubbing.sno = subproductsclubbing.Clubsno INNER JOIN productsdata ON subproductsclubbing.Productid = productsdata.sno LEFT OUTER JOIN empmanage ON incentive_structure.ApprovedBy = empmanage.Sno WHERE (incentive_structure.sno = @structuresno)");  
            cmd.Parameters.AddWithValue("@structuresno", ddlIncentiveStructure.SelectedValue);
            DataTable dtclubproducts = vdm.SelectQuery(cmd).Tables[0];
            DataView incentiveview = new DataView(dtclubproducts);
            DataView incentiveview1 = new DataView(dtclubproducts);
            DataTable dticentive = incentiveview.ToTable(true, "clubsno", "ClubName");
            DataTable dticentive1 = incentiveview.ToTable(true, "ApprovalStatus", "EmpName", "ApprovedDate");
            string ApprovalStatus = dticentive1.Rows[0]["ApprovalStatus"].ToString();
            string ApprovedBy = dticentive1.Rows[0]["EmpName"].ToString();
            string ApprovedDate = dticentive1.Rows[0]["ApprovedDate"].ToString();

            Report = new DataTable();
            Report.Columns.Add("Clubbing Name");
            Report.Columns.Add("Product Name");
            Report.Columns.Add("Slabs");
            foreach (DataRow branch in dticentive.Rows)
            {
                DataRow newrow = Report.NewRow();
                newrow["Clubbing Name"] = branch["ClubName"].ToString();
                string products = "";
                string slabs = "";
                cmd = new MySqlCommand("SELECT club_sno, SlotQty, Amt FROM slabs WHERE (club_sno = @clubsno)");
                cmd.Parameters.AddWithValue("@clubsno", branch["clubsno"].ToString());
                DataTable dtslabs = vdm.SelectQuery(cmd).Tables[0];
                foreach (DataRow clubs in dtclubproducts.Rows)
                {
                    if (branch["clubsno"].ToString() == clubs["clubsno"].ToString())
                    {
                        products += clubs["ProductName"].ToString() + "\r\n";
                    }
                }
                foreach (DataRow drslabs in dtslabs.Rows)
                {
                    if (branch["clubsno"].ToString() == drslabs["club_sno"].ToString())
                    {
                        slabs += drslabs["SlotQty"].ToString() + "   ->" + drslabs["Amt"].ToString() + "    Rs" + "\r\n";
                    }
                }
                newrow["Product Name"] = products;
                newrow["Slabs"] = slabs;
                Report.Rows.Add(newrow);
                if (ApprovalStatus == "A")
                {
                    btnApprove.Text = "Disable";
                }
                if (ApprovalStatus == "P")
                {
                    btnApprove.Text = "Approval";
                }
                //if (ApprovedDate != "")
                //{
                    lblDate.Text = ApprovedDate;
                //}
                //if (ApprovedBy != "")
                //{
                    lblApprovedBy.Text = ApprovedBy;
                //}
            }
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

        catch
        {

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
    protected void grdReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow row = e.Row;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            row.Cells[0].Text = row.Cells[0].Text.Replace("\n", "<br />");
            row.Cells[1].Text = row.Cells[1].Text.Replace("\n", "<br />");
            row.Cells[2].Text = row.Cells[2].Text.Replace("\n", "<br />");
        }
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            vdm = new VehicleDBMgr();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            string empid = Session["UserSno"].ToString();
            string buttontext = btnApprove.Text;
            if (buttontext == "Approval")
            {
                // cmd = new MySqlCommand("UPDATE incentive_structure SET ApprovalStatus = 'A' WHERE (sno = @incentivesno)");
                cmd = new MySqlCommand("UPDATE incentive_structure SET ApprovalStatus = 'A', ApprovedDate = @dt, ApprovedBy = @empid WHERE (sno = @incentivesno)");
                cmd.Parameters.AddWithValue("@incentivesno", ddlIncentiveStructure.SelectedValue);
                cmd.Parameters.AddWithValue("@dt", ServerDateCurrentdate);
                cmd.Parameters.AddWithValue("@empid", empid);
                vdm.Update(cmd);
                lblmsg.Text = "Selected Structure Aprroved";
            }
            if (buttontext == "Disable")
            {
                cmd = new MySqlCommand("UPDATE incentive_structure SET ApprovalStatus = 'P', ApprovedDate = @dt, ApprovedBy = @empid WHERE (sno = @incentivesno)");
                cmd.Parameters.AddWithValue("@incentivesno", ddlIncentiveStructure.SelectedValue);
                cmd.Parameters.AddWithValue("@dt", ServerDateCurrentdate);
                cmd.Parameters.AddWithValue("@empid", empid);
                vdm.Update(cmd);
                lblmsg.Text = "Selected Structure Aprroval Cancelled";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {
            vdm = new VehicleDBMgr();

            cmd = new MySqlCommand("UPDATE incentive_structure SET ApprovalStatus = 'R' WHERE (sno = @incentivesno)");
            cmd.Parameters.AddWithValue("@incentivesno", ddlIncentiveStructure.SelectedValue);
            vdm.Update(cmd);
            lblmsg.Text = "Selected Structure Rejected";
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    
}