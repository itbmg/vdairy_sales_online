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
public partial class DueManage : System.Web.UI.Page
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
        //UserName = Session["field1"].ToString();
        //vdm = new VehicleDBMgr();
        if (!this.IsPostBack)
        {
            if (!Page.IsCallback)
            {
                FillRouteName();
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    string id = "";
    void FillRouteName()
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
                ddlSalesOffice.DataSource = dtRoutedata;
                ddlSalesOffice.DataTextField = "BranchName";
                ddlSalesOffice.DataValueField = "sno";
                ddlSalesOffice.DataBind();
            }
            else
            {
                PBranch.Visible = true;
                cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno FROM  branchdata INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE (branchdata_1.SalesOfficeID = @SOID) AND (branchdata.SalesType IS NOT NULL) OR (branchdata.sno = @BranchID)");
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
    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            DataTable Report = new DataTable();
            string branchname = Session["branchname"].ToString();
            Session["filename"] = branchname + " RateSheet " + DateTime.Now.ToString("dd/MM/yyyy");
            //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName, branchproducts.product_sno, branchproducts.unitprice, productsdata.ProductName FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN branchproducts ON branchdata.sno = branchproducts.branch_sno INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno WHERE (branchroutes.Sno = 73) ORDER BY branchproducts.Rank");
            cmd = new MySqlCommand("SELECT branchdata.flag, branchdata.BranchName, branchproducts.product_sno, productsdata.ProductName, branchproducts.unitprice, branchdata.sno FROM branchdata INNER JOIN branchproducts ON branchdata.sno = branchproducts.branch_sno INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno WHERE ((branchmappingtable.SuperBranch = @BranchID) and (branchdata.flag='1') ) OR ((branchdata_1.SalesOfficeID = @SOID) and (branchdata.flag='1') ) ORDER BY branchdata.sno");
            cmd.Parameters.AddWithValue("@Flag", "1");
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            DataTable dtAgents = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT productsdata.ProductName, branchproducts.product_sno, branchproducts.unitprice, branchdata.BranchName, branchdata.sno FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN branchdata ON branchproducts.branch_sno = branchdata.sno INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE ((branchdata.sno = @BranchID) and (branchdata.flag='1')) OR ((branchdata_1.SalesOfficeID = @SOID) and (branchdata_1.flag='1')) ORDER BY branchproducts.Rank");
            cmd.Parameters.AddWithValue("@Flag", "1");
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            DataTable dtBranch = vdm.SelectQuery(cmd).Tables[0];
            if (dtBranch.Rows.Count > 0)
            {
                foreach (DataRow dr in dtBranch.Rows)
                {
                    DataRow drnew = dtAgents.NewRow();
                    drnew["BranchName"] = dr["BranchName"].ToString();
                    drnew["product_sno"] = dr["product_sno"].ToString();
                    drnew["ProductName"] = dr["ProductName"].ToString();
                    drnew["unitprice"] = dr["unitprice"].ToString();
                    drnew["sno"] = dr["sno"].ToString();
                    dtAgents.Rows.Add(drnew);
                }
            }
            cmd = new MySqlCommand("SELECT products_category.Categoryname, productsdata.sno, productsdata.ProductName, branchproducts.product_sno FROM productsdata INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno WHERE (branchproducts.branch_sno = @BranchID)  ORDER BY branchproducts.Rank");
            //cmd.Parameters.AddWithValue("@Flag", "1");
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
            if (produtstbl.Rows.Count > 0)
            {
                DataView view = new DataView(dtAgents);
                DataTable distincttable = view.ToTable(true, "BranchName", "sno");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("Agent Code");
                Report.Columns.Add("Agent Name");
                foreach (DataRow dr in produtstbl.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                }
                int i = 1;
                foreach (DataRow branch in distincttable.Rows)
                {
                    DataRow newrow = Report.NewRow();
                    newrow["SNo"] = i;
                    newrow["Agent Code"] = branch["sno"].ToString();
                    newrow["Agent Name"] = branch["BranchName"].ToString();
                    foreach (DataRow dr in dtAgents.Rows)
                    {
                        try
                        {
                            if (branch["BranchName"].ToString() == dr["BranchName"].ToString())
                            {
                                id = dr["BranchName"].ToString();
                                id += branch["sno"].ToString();
                                double unitprice = 0;
                                double.TryParse(dr["unitprice"].ToString(), out unitprice);
                                newrow[dr["ProductName"].ToString()] = unitprice;
                            }
                        }
                        catch
                        {
                        }
                    }
                    Report.Rows.Add(newrow);
                    i++;
                }
            }
            for (int i = Report.Rows.Count - 1; i >= 0; i--)
            {
                if (Report.Rows[i][1] == DBNull.Value)
                    Report.Rows[i].Delete();
            }
            grdReports.DataSource = Report;
            grdReports.DataBind();
            Session["xportdata"] = Report;
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            msg += id;
            lblmsg.Text = msg;
        }
    }
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable("GridView_Data");
            foreach (TableCell cell in grdReports.HeaderRow.Cells)
            {
                if (cell.Text == "Agent Name")
                {
                    dt.Columns.Add(cell.Text);
                }
                else
                {
                    dt.Columns.Add(cell.Text).DataType = typeof(double);
                }
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
            Session["dtImport"] = dt;
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
    protected void btn_Import_Click(object sender, EventArgs e)
    {
        string FilePath = ConfigurationManager.AppSettings["FilePath"].ToString();
        string filename = string.Empty;
        //To check whether file is selected or not to uplaod
        if (FileUploadToServer.HasFile)
        {
            try
            {
                string[] allowdFile = { ".xls", ".xlsx" };
                //Here we are allowing only excel file so verifying selected file pdf or not
                string FileExt = System.IO.Path.GetExtension(FileUploadToServer.PostedFile.FileName);
                //Check whether selected file is valid extension or not
                bool isValidFile = allowdFile.Contains(FileExt);
                if (!isValidFile)
                {
                    lblmsg.ForeColor = System.Drawing.Color.Red;
                    lblmsg.Text = "Please upload only Excel";
                }
                else
                {
                    // Get size of uploaded file, here restricting size of file
                    int FileSize = FileUploadToServer.PostedFile.ContentLength;
                    if (FileSize <= 1048576)//1048576 byte = 1MB
                    {
                        //Get file name of selected file
                        filename = Path.GetFileName(Server.MapPath(FileUploadToServer.FileName));

                        //Save selected file into server location
                        FileUploadToServer.SaveAs(Server.MapPath(FilePath) + filename);
                        //Get file path
                        string filePath = Server.MapPath(FilePath) + filename;
                        //Open the connection with excel file based on excel version
                        OleDbConnection con = null;
                        if (FileExt == ".xls")
                        {
                            con = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0;");

                        }
                        else if (FileExt == ".xlsx")
                        {
                            con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;");
                        }

                        con.Close(); con.Open();
                        //Get the list of sheet available in excel sheet
                        DataTable dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        //Get first sheet name
                        string getExcelSheetName = dt.Rows[0]["Table_Name"].ToString();
                        //Select rows from first sheet in excel sheet and fill into dataset "SELECT * FROM [Sheet1$]";  
                        OleDbCommand ExcelCommand = new OleDbCommand(@"SELECT * FROM [" + getExcelSheetName + @"]", con);
                        OleDbDataAdapter ExcelAdapter = new OleDbDataAdapter(ExcelCommand);
                        DataSet ExcelDataSet = new DataSet();
                        ExcelAdapter.Fill(ExcelDataSet);
                        //Bind the dataset into gridview to display excel contents
                        grdReports.DataSource = ExcelDataSet;
                        grdReports.DataBind();
                        Session["dtImport"] = ExcelDataSet.Tables[0];
                        BtnSave.Visible = true;

                    }
                    else
                    {
                        lblmsg.Text = "Attachment file size should not be greater then 1 MB!";
                    }
                }
            }
            catch (Exception ex)
            {
                lblmsg.Text = "Error occurred while uploading a file: " + ex.Message;
            }
        }
        else
        {
            lblmsg.Text = "Please select a file to upload.";
        }
    }
    private void Import_To_Grid(string FilePath, string Extension, string isHDR)
    {
        try
        {
            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            conStr = String.Format(conStr, FilePath, isHDR);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            //Get the name of First Sheet
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            //Read Data from First Sheet
            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            //Bind Data to GridView
            grdReports.Caption = Path.GetFileName(FilePath);
            grdReports.DataSource = dt;
            grdReports.DataBind();
            Session["dtImport"] = dt;
            BtnSave.Visible = true;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }

    }
    protected void btn_WIDB_Click(object sender, EventArgs e)
    {

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


        string myc = "";
        string pname = "";
        try
        {
            vdm = new VehicleDBMgr();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            DataTable dt = (DataTable)Session["dtImport"];
            cmd = new MySqlCommand("SELECT agent_bal_trans.agentid,agent_bal_trans.opp_balance,agent_bal_trans.inddate,agent_bal_trans.salesvalue,agent_bal_trans.paidamount,agent_bal_trans.clo_balance FROM agent_bal_trans inner join branchmappingtable on branchmappingtable.SubBranch = agent_bal_trans.agentid where branchmappingtable.SuperBranch=@Branchid and agent_bal_trans.inddate between @d1 and @d2");
            cmd.Parameters.AddWithValue("@Branchid", ddlSalesOffice.SelectedValue);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1))); 
            cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
            DataTable dtAgentBal = vdm.SelectQuery(cmd).Tables[0];


            cmd = new MySqlCommand("SELECT * FROM  branchaccounts inner join branchmappingtable on branchmappingtable.SubBranch =branchaccounts.Branchid where branchmappingtable.SuperBranch=@Branchid");
            cmd.Parameters.AddWithValue("@Branchid", ddlSalesOffice.SelectedValue);
            DataTable dtbranchacc = vdm.SelectQuery(cmd).Tables[0];

            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                string AgentCode = dr["AgentCode"].ToString();
                string clo_balance = dr["clo_balance"].ToString();
                myc = AgentCode;
                DataRow[] drBp = dtAgentBal.Select("agentid='" + AgentCode + "'");
                if (drBp.Length > 0)
                {
                    cmd = new MySqlCommand("Update agent_bal_trans set opp_balance=@opp_balance,salesvalue=@salesvalue,paidamount=@paidamount,clo_balance=@clo_balance,createdate=@createdate,entryby=@entryby where (agentid=@agentid) and (inddate between @d1 and @d2)");
                    cmd.Parameters.AddWithValue("@opp_balance", 0);
                    cmd.Parameters.AddWithValue("@salesvalue", 0);
                    cmd.Parameters.AddWithValue("@paidamount", 0);
                    cmd.Parameters.AddWithValue("@clo_balance", clo_balance);
                    cmd.Parameters.AddWithValue("@agentid", AgentCode);
                    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@d2", GetHighDate(fromdate.AddDays(-1)));
                    cmd.Parameters.AddWithValue("@createdate", ServerDateCurrentdate);
                    cmd.Parameters.AddWithValue("@entryby", Session["UserSno"].ToString());
                    vdm.Update(cmd);

                }
                else
                {
                    cmd = new MySqlCommand("insert into agent_bal_trans (agentid,opp_balance,inddate,salesvalue,paidamount,clo_balance,createdate,entryby) values (@agentid,@opp_balance,@inddate,@salesvalue,@paidamount,@clo_balance,@createdate,@entryby)");
                    cmd.Parameters.AddWithValue("@opp_balance", 0);
                    cmd.Parameters.AddWithValue("@salesvalue", 0);
                    cmd.Parameters.AddWithValue("@paidamount", 0);
                    cmd.Parameters.AddWithValue("@clo_balance", clo_balance);
                    cmd.Parameters.AddWithValue("@agentid", AgentCode);
                    cmd.Parameters.AddWithValue("@inddate", fromdate.AddDays(-1));
                    cmd.Parameters.AddWithValue("@createdate", ServerDateCurrentdate);
                    cmd.Parameters.AddWithValue("@entryby", Session["UserSno"].ToString());
                    vdm.insert(cmd);
                }

                DataRow[] dracc = dtbranchacc.Select("BranchId='" + AgentCode + "'");

                if (dracc.Length > 0)
                {
                    cmd = new MySqlCommand("Update branchaccounts set Amount=@Amount,FineAmount=@FineAmount,Dtripid=@Dtripid,Ctripid=@Ctripid,SaleValue=@SaleValue,cdate=@cdate where (BranchId=@BranchId)");
                    cmd.Parameters.AddWithValue("@BranchId", AgentCode);
                    cmd.Parameters.AddWithValue("@Amount", clo_balance);
                    cmd.Parameters.AddWithValue("@FineAmount", 0);
                    cmd.Parameters.AddWithValue("@Dtripid", 0);
                    cmd.Parameters.AddWithValue("@Ctripid", 0);
                    cmd.Parameters.AddWithValue("@SaleValue", 0);
                    cmd.Parameters.AddWithValue("@cdate", ServerDateCurrentdate);
                    vdm.Update(cmd);
                }
                else
                {
                    cmd = new MySqlCommand("insert into branchaccounts (BranchId,Amount,FineAmount,Dtripid,Ctripid,SaleValue,cdate) values (@BranchId,@Amount,@FineAmount,@Dtripid,@Ctripid,@SaleValue,@cdate)");
                    cmd.Parameters.AddWithValue("@BranchId", AgentCode);
                    cmd.Parameters.AddWithValue("@Amount", clo_balance);
                    cmd.Parameters.AddWithValue("@FineAmount", 0);
                    cmd.Parameters.AddWithValue("@Dtripid", 0);
                    cmd.Parameters.AddWithValue("@Ctripid", 0);
                    cmd.Parameters.AddWithValue("@SaleValue", 0);
                    cmd.Parameters.AddWithValue("@cdate", ServerDateCurrentdate);
                    vdm.insert(cmd);
                }

            }
            lblmsg.Text = "Updated Successfully";
        }
        catch (Exception ex)
        {
            string sg = myc;
            string pid = pname;
            if (ex.Message == "Object reference not set to an instance of an object.")
            {
                lblmsg.Text = "Session Expired";
                Response.Redirect("Login.aspx");
            }
            else
            {
                lblmsg.Text = ex.Message;

            }
        }
    }
}