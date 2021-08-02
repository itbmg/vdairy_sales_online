using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using ClosedXML.Excel;
using System.Configuration;
using System.Data.OleDb;

public partial class InventoryUpdate : System.Web.UI.Page
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
            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    void GetReport()
    {
        try
        {
            vdm = new VehicleDBMgr();
            DataTable Report = new DataTable();
            string branchname = Session["branchname"].ToString();
            Session["filename"] = branchname + " InventorySheetUpdate " + DateTime.Now.ToString("dd/MM/yyyy");
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, invmaster.InvName, inventory_monitor.Qty,inventory_monitor.Inv_Sno  FROM  branchmappingtable INNER JOIN branchdata ON branchmappingtable.SubBranch = branchdata.sno INNER JOIN inventory_monitor ON branchdata.sno = inventory_monitor.BranchId INNER JOIN invmaster ON inventory_monitor.Inv_Sno = invmaster.sno INNER JOIN branchdata branchdata_1 ON branchmappingtable.SuperBranch = branchdata_1.sno WHERE (branchmappingtable.SuperBranch = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID) ORDER BY invmaster.sno");
            //cmd.Parameters.AddWithValue("@Flag", "1");
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            DataTable dtAgents = vdm.SelectQuery(cmd).Tables[0];
            cmd = new MySqlCommand("SELECT branchdata.BranchName, branchdata.sno, invmaster.InvName, inventory_monitor.Qty,inventory_monitor.Inv_Sno FROM branchdata branchdata_1 INNER JOIN branchdata ON branchdata_1.sno = branchdata.sno INNER JOIN inventory_monitor ON branchdata.sno = inventory_monitor.BranchId INNER JOIN invmaster ON inventory_monitor.Inv_Sno = invmaster.sno WHERE (branchdata.sno = @BranchID) OR (branchdata_1.SalesOfficeID = @SOID) ORDER BY invmaster.sno");
            //cmd.Parameters.AddWithValue("@Flag", "1");
            cmd.Parameters.AddWithValue("@SOID", BranchID);
            cmd.Parameters.AddWithValue("@BranchID", BranchID);
            DataTable dtBranch = vdm.SelectQuery(cmd).Tables[0];
            if (dtBranch.Rows.Count > 0)
            {
                foreach (DataRow dr in dtBranch.Rows)
                {
                    DataRow drnew = dtAgents.NewRow();
                    drnew["BranchName"] = dr["BranchName"].ToString();
                    drnew["Inv_Sno"] = dr["Inv_Sno"].ToString();
                    drnew["InvName"] = dr["InvName"].ToString();
                    drnew["Qty"] = dr["Qty"].ToString();
                    drnew["sno"] = dr["sno"].ToString();
                    dtAgents.Rows.Add(drnew);
                }
            }
            cmd = new MySqlCommand("SELECT sno, InvName FROM invmaster  ORDER BY sno");
            DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
            if (produtstbl.Rows.Count > 0)
            {
                DataView view = new DataView(dtAgents);
                DataTable distincttable = view.ToTable(true, "BranchName", "sno");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("Agent Code");
                Report.Columns.Add("Agent Name");
                int count = 0;
                foreach (DataRow dr in produtstbl.Rows)
                {
                    string InvSno = dr["sno"].ToString();
                    if (InvSno == "6" || InvSno == "7" || InvSno == "8")
                    {
                    }
                    else
                    {
                        Report.Columns.Add(dr["InvName"].ToString()).DataType = typeof(Int32);
                        count++;
                    }
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
                        if (branch["BranchName"].ToString() == dr["BranchName"].ToString())
                        {
                            string InvSno = dr["Inv_Sno"].ToString();
                            if (InvSno == "6" || InvSno == "7" || InvSno == "8")
                            {
                            }
                            else
                            {
                                int Qty = 0;
                                int.TryParse(dr["Qty"].ToString(), out Qty);
                                newrow[dr["InvName"].ToString()] = Qty;
                            }
                        }
                    }
                    Report.Rows.Add(newrow);
                    i++;
                }
            }
            grdReports.DataSource = Report;
            grdReports.DataBind();
            Session["xportdata"] = Report;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
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
                    dt.Columns.Add(cell.Text).DataType = typeof(Int32);
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
        try
        {
            vdm = new VehicleDBMgr();
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            DataTable dt = (DataTable)Session["dtImport"];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                string AgentCode = dr["Agent Code"].ToString();
                int j = 3;
                foreach (DataColumn dc in dt.Columns)
                {
                    var cell = dc.ColumnName;
                    if (cell == "SNo" || cell == "Agent Code" || cell == "Agent Name")
                    {
                    }
                    else
                    {
                        cmd = new MySqlCommand("Select sno from invmaster where InvName=@InvName");
                        cmd.Parameters.AddWithValue("@InvName", dc.ColumnName);
                        DataTable dtProduct = vdm.SelectQuery(cmd).Tables[0];
                        string invsno = dtProduct.Rows[0]["Sno"].ToString();
                        string Qty = dt.Rows[i][j].ToString();
                        cmd = new MySqlCommand("update inventory_monitor set Qty=@Qty where BranchId=@BranchId and Inv_Sno=@Inv_Sno");
                        cmd.Parameters.AddWithValue("@Qty", Qty);
                        cmd.Parameters.AddWithValue("@BranchId", AgentCode);
                        cmd.Parameters.AddWithValue("@Inv_Sno", invsno);
                        vdm.Update(cmd);
                    }
                    j++;
                }
                i++;
            }
            lblmsg.Text = "Updated Successfully";
        }
        catch (Exception ex)
        {
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