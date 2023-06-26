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
                txtTodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txtFromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
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
                cmd.Parameters.AddWithValue("@SalesType", "4");
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
                        Button3.Visible = true;

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
            DataTable dt = (DataTable)Session["dtImport"];
            vdm = new VehicleDBMgr();

            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            DataTable dtAgentBal = new DataTable();

            foreach (DataRow drr in dt.Rows)
            {
                string AgentCode = drr["AgentCode"].ToString();
                string clo_balance = drr["clo_balance"].ToString();
                cmd = new MySqlCommand("SELECT ROUND(SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost),2) AS Totalsalevalue,ROUND(SUM(indents_subtable.DeliveryQty),2) AS DeliveryQty,products_category.Categoryname, productsdata.ProductName, DATE_FORMAT(indents.I_date, '%d %b %y') AS IndentDate FROM productsdata INNER JOIN indents_subtable ON productsdata.sno = indents_subtable.Product_sno INNER JOIN indents ON indents_subtable.IndentNo = indents.IndentNo INNER JOIN branchdata ON indents.Branch_id = branchdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (indents.I_date BETWEEN @d1 AND @d2) AND (branchdata.sno = @BranchID) and (indents_subtable.DeliveryQty<>'0') GROUP BY productsdata.sno, IndentDate ORDER BY indents.I_date");
                cmd.Parameters.AddWithValue("@BranchID", AgentCode);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                DataTable dtAgent = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT SUM(AmountPaid) AS AmountPaid, DATE_FORMAT(PaidDate, '%d/%b/%y') AS PDate, CheckStatus,PaymentType FROM collections WHERE (Branchid = @BranchID) AND (PaidDate BETWEEN @d1 AND @d2)  AND ((PaymentType = 'Cash') OR (PaymentType = 'PhonePay') OR (PaymentType = 'Bank Transfer'))  GROUP BY PDate ORDER BY PDate");
                cmd.Parameters.AddWithValue("@BranchID", AgentCode);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                DataTable dtAgentDayWiseCollection = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT SUM(AmountPaid) AS AmountPaid, DATE_FORMAT(PaidDate, '%d/%b/%y') AS PDate FROM collections WHERE (Branchid = @BranchID) AND (PaidDate BETWEEN @d1 AND @d2) and (tripId is NULL) AND ((PaymentType = 'Incentive') OR (PaymentType = 'Journal Voucher')) GROUP BY PDate ORDER BY PDate");
                cmd.Parameters.AddWithValue("@BranchID", AgentCode);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                DataTable dtAgentIncentive = vdm.SelectQuery(cmd).Tables[0];

                cmd = new MySqlCommand("SELECT SUM(AmountPaid) AS AmountPaid, DATE_FORMAT(VarifyDate, '%d/%b/%y') AS VarifyDate, CheckStatus FROM collections WHERE (Branchid = @BranchID) AND (CheckStatus = 'V') AND (VarifyDate BETWEEN @d1 AND @d2) GROUP BY VarifyDate ORDER BY VarifyDate");
                cmd.Parameters.AddWithValue("@BranchID", AgentCode);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(todate));
                DataTable dtAgentchequeCollection = vdm.SelectQuery(cmd).Tables[0];


                TimeSpan dateSpan = todate.Subtract(fromdate);
                int NoOfdays = dateSpan.Days;
                NoOfdays = NoOfdays + 1;
                string ChangedTime2 = "";
                double grand_tot = 0;
                double paidamout = 0;
                double totpaidamt = 0;
                double incen_amt = 0;
                double cheq_amt = 0;
                double salevalue = 0;
                double tot_SaleValue = 0;
                double opbal = 0;
                int k = 0;
                double bracnhAmt = 0;
                for (int j = 0; j < NoOfdays; j++)
                {
                    string dtcount = fromdate.AddDays(j).ToString();
                    DateTime dtDOE = Convert.ToDateTime(dtcount);
                    //string dtdate1 = branch["IndentDate"].ToString();
                    string dtdate1 = dtDOE.AddDays(-1).ToString();
                    DateTime dtDOE1 = Convert.ToDateTime(dtdate1).AddDays(1);
                    string ChangedTime1 = dtDOE1.ToString("dd/MMM/yy");
                    ChangedTime2 = dtDOE.AddDays(-1).ToString("dd MMM yy");

                    foreach (DataRow drdtclubtotal in dtAgentDayWiseCollection.Select("PDate='" + ChangedTime1 + "'"))
                    {
                        double.TryParse(drdtclubtotal["AmountPaid"].ToString(), out paidamout);
                        totpaidamt += paidamout;
                        grand_tot += paidamout;
                    }
                    foreach (DataRow drdtincentive in dtAgentIncentive.Select("PDate='" + ChangedTime1 + "'"))
                    {
                        double.TryParse(drdtincentive["AmountPaid"].ToString(), out incen_amt);
                        totpaidamt += incen_amt;
                        grand_tot += incen_amt;
                    }
                    foreach (DataRow drdtchequeotal in dtAgentchequeCollection.Select("VarifyDate='" + ChangedTime1 + "'"))
                    {
                        double.TryParse(drdtchequeotal["AmountPaid"].ToString(), out cheq_amt);

                        totpaidamt += cheq_amt;
                        grand_tot += cheq_amt;
                    }
                    //foreach (DataRow dr in dtAgent.Rows)
                    foreach (DataRow dr in dtAgent.Select("IndentDate='" + ChangedTime2 + "'"))
                    {
                        //if (ChangedTime2 == dr["IndentDate"].ToString())
                        //{
                        double.TryParse(dr["Totalsalevalue"].ToString(), out salevalue);
                        tot_SaleValue += salevalue;
                        grand_tot += salevalue;
                        //}
                    }
                    //if (dtAgentBal.Rows.Count == 0)
                    //{
                    //    cmd = new MySqlCommand("SELECT agentid,opp_balance,DATE_FORMAT(inddate, '%d %b %y') AS IndentDate,salesvalue,paidamount,clo_balance FROM agent_bal_trans  where  agentid=@Agentid and inddate between @d1 and @d2");
                    //    cmd.Parameters.AddWithValue("@Agentid", AgentCode);
                    //    cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                    //    cmd.Parameters.AddWithValue("@d2", GetHighDate(todate.AddDays(-1)));
                    //    dtAgentBal = vdm.SelectQuery(cmd).Tables[0];
                    //}
                    //foreach (DataRow dr_Due in dtAgentBal.Select("IndentDate='" + ChangedTime2 + "'"))
                    ////foreach (DataRow dr_Due in dtAgentBal.Rows)
                    //{
                    //DateTime duedate = Convert.ToDateTime(dr_Due["IndentDate"].ToString()).AddDays(1);
                    //string duedate1 = duedate.ToString("dd/MMM/yy");
                    //if (duedate1 == ChangedTime2)
                    //{
                    //string clobalance = dr_Due["clo_balance"].ToString();

                    if (grand_tot > 0 || k == 0)
                    {
                        double clo_amt = 0;
                        if (k == 0)
                        {
                            tot_SaleValue = 0;
                            totpaidamt = 0;
                            opbal = 0;
                            k++;
                            clo_amt = 0;
                            double.TryParse(clo_balance, out clo_amt);
                            bracnhAmt = clo_amt;
                            //clo_amt = clo_amt + tot_SaleValue - totpaidamt;
                        }
                        else
                        {
                            opbal = 0;
                            double.TryParse(clo_balance, out opbal);
                            clo_amt = 0;
                            clo_amt = opbal + tot_SaleValue - totpaidamt;
                            clo_balance = clo_amt.ToString();
                            bracnhAmt = clo_amt;
                            k++;
                        }

                        cmd = new MySqlCommand("Update agent_bal_trans set opp_balance=@opp_balance,salesvalue=@salesvalue,paidamount=@paidamount,clo_balance=@clo_balance,createdate=@createdate,entryby=@entryby where (agentid=@agentid) and (inddate between @d1 and @d2)");
                        cmd.Parameters.AddWithValue("@opp_balance", opbal);
                        cmd.Parameters.AddWithValue("@salesvalue", tot_SaleValue);
                        cmd.Parameters.AddWithValue("@paidamount", totpaidamt);
                        cmd.Parameters.AddWithValue("@clo_balance", clo_amt);
                        cmd.Parameters.AddWithValue("@agentid", AgentCode);
                        cmd.Parameters.AddWithValue("@d1", GetLowDate(dtDOE.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@d2", GetHighDate(dtDOE.AddDays(-1)));
                        cmd.Parameters.AddWithValue("@createdate", ServerDateCurrentdate);
                        cmd.Parameters.AddWithValue("@entryby", Session["UserSno"].ToString());
                        if (vdm.Update(cmd) == 0)
                        {
                            if (k == 1)
                            {
                                cmd = new MySqlCommand("insert into agent_bal_trans (agentid,opp_balance,inddate,salesvalue,paidamount,clo_balance,createdate,entryby) values (@agentid,@opp_balance,@inddate,@salesvalue,@paidamount,@clo_balance,@createdate,@entryby)");
                                cmd.Parameters.AddWithValue("@opp_balance", opbal);
                                cmd.Parameters.AddWithValue("@salesvalue", tot_SaleValue);
                                cmd.Parameters.AddWithValue("@paidamount", totpaidamt);
                                cmd.Parameters.AddWithValue("@clo_balance", clo_amt);
                                cmd.Parameters.AddWithValue("@agentid", AgentCode);
                                cmd.Parameters.AddWithValue("@inddate", dtDOE.AddDays(-1));
                                cmd.Parameters.AddWithValue("@createdate", ServerDateCurrentdate);
                                cmd.Parameters.AddWithValue("@entryby", Session["UserSno"].ToString());
                                vdm.insert(cmd);
                            }
                        }
                        tot_SaleValue = 0;
                        totpaidamt = 0;
                        grand_tot = 0;
                    }
                }
                cmd = new MySqlCommand("Update branchaccounts set Amount=@Amount,FineAmount=@FineAmount,Dtripid=@Dtripid,Ctripid=@Ctripid,SaleValue=@SaleValue,cdate=@cdate where (BranchId=@BranchId)");
                cmd.Parameters.AddWithValue("@BranchId", AgentCode);
                cmd.Parameters.AddWithValue("@Amount", bracnhAmt);
                cmd.Parameters.AddWithValue("@FineAmount", 0);
                cmd.Parameters.AddWithValue("@Dtripid", 0);
                cmd.Parameters.AddWithValue("@Ctripid", 0);
                cmd.Parameters.AddWithValue("@SaleValue", 0);
                cmd.Parameters.AddWithValue("@cdate", ServerDateCurrentdate);
                if (vdm.Update(cmd) == 0)
                {
                    cmd = new MySqlCommand("insert into branchaccounts (BranchId,Amount,FineAmount,Dtripid,Ctripid,SaleValue,cdate) values (@BranchId,@Amount,@FineAmount,@Dtripid,@Ctripid,@SaleValue,@cdate)");
                    cmd.Parameters.AddWithValue("@BranchId", AgentCode);
                    cmd.Parameters.AddWithValue("@Amount", bracnhAmt);
                    cmd.Parameters.AddWithValue("@FineAmount", 0);
                    cmd.Parameters.AddWithValue("@Dtripid", 0);
                    cmd.Parameters.AddWithValue("@Ctripid", 0);
                    cmd.Parameters.AddWithValue("@SaleValue", 0);
                    cmd.Parameters.AddWithValue("@cdate", ServerDateCurrentdate);
                    vdm.insert(cmd);
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
    }
}