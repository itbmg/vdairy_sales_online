
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

public partial class Agent_wise_rates : System.Web.UI.Page
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
                FillAgentName();
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
                PBranch.Visible = false;
                //cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE (dispatch.flag = 1) AND (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
                ////cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
                //cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                //cmd.Parameters.AddWithValue("@SOID", Session["branch"].ToString());
                cmd = new MySqlCommand("SELECT RouteName, Sno,BranchID FROM branchroutes WHERE (BranchID = @BranchID) and  (flag=@flag)");
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
                cmd.Parameters.AddWithValue("@flag", "1");
                DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
                ddlDispName.DataSource = dtRoutedata;
                ddlDispName.DataTextField = "RouteName";
                ddlDispName.DataValueField = "Sno";
                ddlDispName.DataBind();
                ddlDispName.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
    protected void ddlSalesOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch)");
        //cmd = new MySqlCommand("SELECT dispatch.DispName, dispatch.sno FROM dispatch INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno INNER JOIN branchdata branchdata_1 ON dispatch.Branch_Id = branchdata_1.sno WHERE   (dispatch.flag = 1) AND (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID)");
        ////cmd = new MySqlCommand("SELECT DispName, sno FROM dispatch WHERE (Branch_Id = @BranchD)");
        //cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        //cmd.Parameters.AddWithValue("@SOID", ddlSalesOffice.SelectedValue);

        cmd = new MySqlCommand("SELECT RouteName, Sno,BranchID FROM branchroutes WHERE (BranchID = @BranchID) and  (flag=@flag)");
        cmd.Parameters.AddWithValue("@BranchID", ddlSalesOffice.SelectedValue);
        cmd.Parameters.AddWithValue("@flag", "1");

        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlDispName.DataSource = dtRoutedata;
        ddlDispName.DataTextField = "RouteName";
        ddlDispName.DataValueField = "Sno";
        ddlDispName.DataBind();
        ddlDispName.Items.Insert(0, new ListItem("Select", "0"));
    }
    protected void ddlDispName_SelectedIndexChanged(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        ////cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM branchdata INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch WHERE (branchmappingtable.SuperBranch = @SuperBranch)");
        //cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName FROM dispatch INNER JOIN dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN branchroutes ON dispatch_sub.Route_id = branchroutes.Sno INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (dispatch.sno = @dispsno) AND  (dispatch.flag = 1)");
        //cmd.Parameters.AddWithValue("@dispsno", ddlDispName.SelectedValue);
        cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName,branchroutes.Sno FROM branchroutesubtable INNER JOIN branchroutes ON branchroutesubtable.RefNo = branchroutes.Sno INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno WHERE (branchroutes.Sno = @RouteID) AND branchdata.flag=@flag");
        cmd.Parameters.AddWithValue("@RouteID", ddlDispName.SelectedValue);
        cmd.Parameters.AddWithValue("@flag", "1");
        DataTable dtRoutedata = vdm.SelectQuery(cmd).Tables[0];
        ddlAgentName.DataSource = dtRoutedata;
        ddlAgentName.DataTextField = "BranchName";
        ddlAgentName.DataValueField = "Sno";
        ddlAgentName.DataBind();
        ddlAgentName.Items.Insert(0, new ListItem("All_Agents", "0"));

    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    string id = "";

    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            DataTable Report = new DataTable();
            string branchname = ddlAgentName.SelectedItem.Text;
            Session["filename"] = branchname + " RateSheet " + DateTime.Now.ToString("dd/MM/yyyy");
            //cmd = new MySqlCommand("SELECT branchdata.flag, branchdata.BranchName, branchproducts.product_sno, productsdata.ProductName, branchproducts.unitprice, branchdata.sno FROM branchdata INNER JOIN branchproducts ON branchdata.sno = branchproducts.branch_sno INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno WHERE branchdata.sno = @BranchID and branchdata.flag='1' ORDER BY branchdata.sno");
            //cmd.Parameters.AddWithValue("@Flag", "1");
            //cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            //DataTable dtAgents = vdm.SelectQuery(cmd).Tables[0];
            if (branchname == "All_Agents")
            {
                cmd = new MySqlCommand("SELECT branchdata.sno, branchdata.BranchName, branchproducts.product_sno, branchproducts.unitprice, productsdata.ProductName FROM branchroutes INNER JOIN branchroutesubtable ON branchroutes.Sno = branchroutesubtable.RefNo INNER JOIN branchdata ON branchroutesubtable.BranchID = branchdata.sno INNER JOIN branchproducts ON branchdata.sno = branchproducts.branch_sno INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno WHERE (branchroutes.Sno = @sno) ORDER BY branchproducts.Rank");
                //cmd = new MySqlCommand("SELECT branchdata.flag, branchdata.BranchName, branchproducts.product_sno, productsdata.ProductName, branchproducts.unitprice, branchdata.sno FROM branchdata INNER JOIN branchproducts ON branchdata.sno = branchproducts.branch_sno INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno  branchdata.flag='1' ORDER BY branchdata.sno");
                cmd.Parameters.AddWithValue("@sno", ddlDispName.SelectedValue);
                cmd.Parameters.AddWithValue("@Flag", "1");
            }
            else
            {
                cmd = new MySqlCommand("SELECT branchdata.flag, branchdata.BranchName, branchproducts.product_sno, productsdata.ProductName, branchproducts.unitprice, branchdata.sno FROM branchdata INNER JOIN branchproducts ON branchdata.sno = branchproducts.branch_sno INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN branchmappingtable ON branchdata.sno = branchmappingtable.SubBranch INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno WHERE branchdata.sno = @BranchID and branchdata.flag='1' ORDER BY branchdata.sno");
                cmd.Parameters.AddWithValue("@Flag", "1");
                cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
            }
            DataTable dtAgents = vdm.SelectQuery(cmd).Tables[0];

            cmd = new MySqlCommand("SELECT productsdata.ProductName, branchproducts.product_sno, branchproducts.unitprice, branchdata.BranchName, branchdata.sno FROM branchproducts INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN branchdata ON branchproducts.branch_sno = branchdata.sno INNER JOIN branchdata branchdata_1 ON branchdata.sno = branchdata_1.sno WHERE branchdata.sno = @BranchID and branchdata.flag='1' ORDER BY branchproducts.Rank");
            cmd.Parameters.AddWithValue("@Flag", "1");
            cmd.Parameters.AddWithValue("@BranchID", ddlAgentName.SelectedValue);
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
        string myc = "";
        string pname = "";
        try
        {
            vdm = new VehicleDBMgr();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            DataTable dt = (DataTable)Session["dtImport"];
            cmd = new MySqlCommand("SELECT branch_sno, product_sno, unitprice FROM branchproducts  ");
            DataTable dtBrnchPrdt = vdm.SelectQuery(cmd).Tables[0];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                string AgentCode = dr["Agent Code"].ToString();
                myc = AgentCode;
                DataTable dtAgentprdt = new DataTable();
                dtAgentprdt.Columns.Add("branch_sno");
                dtAgentprdt.Columns.Add("product_sno");
                dtAgentprdt.Columns.Add("unitprice");
                DataRow[] drBp = dtBrnchPrdt.Select("branch_sno='" + dr["Agent Code"].ToString() + "'");
                for (int k = 0; k < drBp.Length; k++)
                {
                    DataRow newrow = dtAgentprdt.NewRow();
                    newrow["branch_sno"] = drBp[k][0].ToString();
                    newrow["product_sno"] = drBp[k][1].ToString();

                    newrow["unitprice"] = drBp[k][2].ToString();
                    dtAgentprdt.Rows.Add(newrow);
                }
                int j = 3;
                foreach (DataColumn dc in dt.Columns)
                {
                    var cell = dc.ColumnName;
                    if (cell == "SNo" || cell == "Agent Code" || cell == "Agent Name")
                    {
                    }
                    else
                    {
                        string UnitPrice = dt.Rows[i][j].ToString();
                        if (UnitPrice == "&nbsp;")
                        {
                            UnitPrice = "0";
                        }
                        cmd = new MySqlCommand("Select Sno from productsdata where ProductName=@ProductName");
                        cmd.Parameters.AddWithValue("@ProductName", dc.ColumnName);
                        pname = dc.ColumnName;
                        DataTable dtProduct = vdm.SelectQuery(cmd).Tables[0];
                        string ProductID = dtProduct.Rows[0]["Sno"].ToString();
                        DataTable oldunitprice = new DataTable();
                        oldunitprice.Columns.Add("unitprice");
                        DataRow[] drAp = dtAgentprdt.Select("product_sno='" + ProductID + "'");
                        if (drAp.Length == 0)
                        {
                            if (UnitPrice == "0")
                            {

                            }
                            else
                            {
                                cmd = new MySqlCommand("insert into branchproducts (branch_sno,product_sno,unitprice,userdata_sno,DTarget,WTarget,MTarget) values (@branchname,@productname,@unitprice, @username,@DTarget,@WTarget,@MTarget)");
                                cmd.Parameters.AddWithValue("@branchname", AgentCode);
                                cmd.Parameters.AddWithValue("@productname", ProductID);
                                float UntCost = 0;
                                float.TryParse(UnitPrice, out UntCost);
                                cmd.Parameters.AddWithValue("@unitprice", UntCost);
                                cmd.Parameters.AddWithValue("@username", Session["userdata_sno"]);
                                int productDaytarget = 0;
                                int productWeektarget = 0;
                                int productMonthtarget = 0;
                                cmd.Parameters.AddWithValue("@DTarget", productDaytarget);
                                cmd.Parameters.AddWithValue("@WTarget", productWeektarget);
                                cmd.Parameters.AddWithValue("@MTarget", productMonthtarget);
                                vdm.insert(cmd);
                            }
                        }
                        else
                        {
                            for (int ap = 0; ap < drAp.Length; ap++)
                            {
                                DataRow newaprow = oldunitprice.NewRow();
                                newaprow["unitprice"] = drAp[ap][2].ToString();
                                oldunitprice.Rows.Add(newaprow);
                            }
                            string oldprice = "0";

                            if (oldunitprice.Rows.Count > 0)
                            {
                                oldprice = oldunitprice.Rows[0]["unitprice"].ToString();
                            }
                            float UnitCost = 0;
                            float.TryParse(UnitPrice, out UnitCost);
                            float oldUnitCost = 0;
                            float.TryParse(oldprice, out oldUnitCost);
                            if (UnitCost == oldUnitCost)
                            {

                            }
                            else
                            {
                                cmd = new MySqlCommand("Update branchproducts set UnitPrice=@UnitPrice where Branch_sno=@Branch_sno and Product_sno=@Product_sno");
                                cmd.Parameters.AddWithValue("@UnitPrice", UnitCost);
                                cmd.Parameters.AddWithValue("@Branch_sno", AgentCode);
                                cmd.Parameters.AddWithValue("@Product_sno", ProductID);
                                vdm.Update(cmd);
                                cmd = new MySqlCommand("insert into productsrateslogs (PrdtSno,BranchId,OldPrice,EditedPrice,EditedUserid,DateOfEdit) values (@PrdtSno,@BranchId,@OldPrice,@EditedPrice,@EditedUserid,@DateOfEdit)");
                                cmd.Parameters.AddWithValue("@PrdtSno", ProductID);
                                cmd.Parameters.AddWithValue("@BranchId", AgentCode);
                                cmd.Parameters.AddWithValue("@OldPrice", oldUnitCost);
                                cmd.Parameters.AddWithValue("@EditedPrice", UnitCost);
                                cmd.Parameters.AddWithValue("@EditedUserid", Session["UserSno"]);
                                cmd.Parameters.AddWithValue("@DateOfEdit", ServerDateCurrentdate);
                                vdm.insert(cmd);
                            }
                        }
                        j++;
                    }
                }
                i++;
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