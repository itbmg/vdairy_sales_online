using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net;
using System.IO;
using ClosedXML.Excel;
public partial class TotalDCReport : System.Web.UI.Page
{
    MySqlCommand cmd;
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
                lblTitle.Text = Session["TitleName"].ToString();
                txtfromdate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                txttodate.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            }
        }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        GetReport();
    }
    protected void btnSMS_Click(object sender, EventArgs e)
    {
        try
        {
            vdm = new VehicleDBMgr();
            string MobNo = txtMobNo.Text;
            DateTime fromdate = DateTime.Now;
            DateTime ServerDateCurrentdate = VehicleDBMgr.GetTime(vdm.conn);
            string leveltype = Session["LevelType"].ToString();
            if (leveltype == "PlantDispatcher")
            {
                string[] dateFromstrig = txtfromdate.Text.Split(' ');
                if (dateFromstrig.Length > 1)
                {
                    if (dateFromstrig[0].Split('-').Length > 0)
                    {
                        string[] dates = dateFromstrig[0].Split('-');
                        string[] times = dateFromstrig[1].Split(':');
                        fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                    }
                }
                DateTime Todate = DateTime.Now;
                string[] dateTostrig = txttodate.Text.Split(' ');
                if (dateTostrig.Length > 1)
                {
                    if (dateTostrig[0].Split('-').Length > 0)
                    {
                        string[] dates = dateTostrig[0].Split('-');
                        string[] times = dateTostrig[1].Split(':');
                        Todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                    }
                }
                string DispatchName = Session["branchname"].ToString();
                DataTable Report = new DataTable();
                lbl_selfromdate.Text = fromdate.ToString("dd/MM/yyyy");
                lbl_selttodate.Text = Todate.ToString("dd/MM/yyyy");
                Session["filename"] = "TOTAL DC REPORT";
                ///Ravi
                cmd = new MySqlCommand("SELECT ProductInfo.ProductName, SUM(ProductInfo.Qty) AS Qty FROM (SELECT tripdata.Sno, tripdata.DCNo, tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, dispatch.DispType, dispatch.DispMode FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE  (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2)) TripInfo INNER JOIN (SELECT  ProductName, Sno, Qty FROM (SELECT productsdata.ProductName, tripdata_1.Sno, tripsubdata.Qty FROM tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE (tripdata_1.AssignDate BETWEEN @d1 AND @d2)) TripSubInfo) ProductInfo ON TripInfo.Sno = ProductInfo.Sno GROUP BY ProductInfo.ProductName");
                //// cmd = new MySqlCommand("SELECT ROUND(SUM(tripsubdata.Qty), 2) AS Qty, productsdata.ProductName FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, Status FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN (SELECT branch_sno, product_sno, unitprice, flag, userdata_sno, DTarget, WTarget, MTarget, BranchQty, LeakQty, Rank FROM branchproducts WHERE (branch_sno = @branch)) brnchprdt ON tripsubdata.ProductId = brnchprdt.product_sno INNER JOIN productsdata ON brnchprdt.product_sno = productsdata.sno WHERE (dispatch.Branch_Id = @branch) GROUP BY productsdata.sno ORDER BY brnchprdt.Rank");
                cmd.Parameters.AddWithValue("@branch", Session["branch"]);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                DataTable dtTotalDespatch = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT ProductInfo.ProductName, SUM(ProductInfo.Qty) AS Qty FROM (SELECT tripdata.Sno, tripdata.DCNo, tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, dispatch.DispType, dispatch.DispMode FROM branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE  (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2)) TripInfo INNER JOIN (SELECT  ProductName, Sno, Qty FROM (SELECT productsdata.ProductName, tripdata_1.Sno, tripsubdata.Qty FROM tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno WHERE (tripdata_1.AssignDate BETWEEN @d1 AND @d2)) TripSubInfo) ProductInfo ON TripInfo.Sno = ProductInfo.Sno GROUP BY ProductInfo.ProductName");
                ////cmd = new MySqlCommand("SELECT ROUND(SUM(tripsubdata.Qty), 2) AS Qty, productsdata.ProductName FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, Status FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN (SELECT branch_sno, product_sno, unitprice, flag, userdata_sno, DTarget, WTarget, MTarget, BranchQty, LeakQty, Rank FROM branchproducts WHERE (branch_sno = @branch)) brnchprdt ON tripsubdata.ProductId = brnchprdt.product_sno INNER JOIN productsdata ON brnchprdt.product_sno = productsdata.sno WHERE (dispatch.Branch_Id = @branch) GROUP BY productsdata.sno ORDER BY brnchprdt.Rank");
                cmd.Parameters.AddWithValue("@branch", Session["branch"]);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
                DataTable dtPreviousTotalDespatch = vdm.SelectQuery(cmd).Tables[0];
                double TotalQty = 0;
                double PrevTotalQty = 0;
                string ProductName = "";
                ProductName += DispatchName + " Total Dispatch For Date  " + fromdate.ToString("dd/MM/yyyy") + ":" + "\r\n";

                if (dtTotalDespatch.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTotalDespatch.Rows)
                    {
                        double unitQty = 0;
                        double PrevunitQty = 0;
                        double diffprdt = 0;
                        double.TryParse(dr["Qty"].ToString(), out unitQty);
                        foreach (DataRow drdtclubtotal in dtPreviousTotalDespatch.Select("ProductName='" + dr["ProductName"].ToString() + "'"))
                        {
                            double.TryParse(drdtclubtotal["Qty"].ToString(), out PrevunitQty);
                        }
                        diffprdt = Math.Round(unitQty - PrevunitQty, 2);
                        ProductName += dr["ProductName"].ToString() + "->" + Math.Round(unitQty, 2) + "(" + Math.Round(diffprdt, 2) + ")" + ";" + "\r\n";
                        TotalQty += Math.Round(unitQty, 2);
                        PrevTotalQty += Math.Round(PrevunitQty, 2);
                    }
                }
                double diffproduct = 0;
                diffproduct = Math.Round(TotalQty - PrevTotalQty, 2);
                if (MobNo.Length == 10)
                {
                    string Date = DateTime.Now.ToString("dd/MM/yyyy");
                    if (Session["TitleName"].ToString() == "Sri Vyshnavi Dairy Spl Pvt Ltd.")
                    {
                        WebClient client = new WebClient();
                        //string strUrl = " http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + no + "&msg=" + message1 + "&type=1 ";

                        string baseurl = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + MobNo + "&message=%20" + ProductName + "TotalQty ->" + TotalQty + "(" + diffproduct + ")" + "&sender=VYSNVI&type=1&route=2"; 
                        
                        
                       // string baseurl = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VSALES&to=" + MobNo + "&msg=%20" + ProductName + "TotalQty ->" + TotalQty + "(" + diffproduct + ")" + "&type=1";
                        Stream data = client.OpenRead(baseurl);
                        StreamReader reader = new StreamReader(data);
                        string ResponseID = reader.ReadToEnd();
                        data.Close();
                        reader.Close();
                    }
                    else 
                    {
                        WebClient client = new WebClient();
                        //string strUrl = " http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + no + "&msg=" + message1 + "&type=1 ";
                        string baseurl = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + MobNo + "&message=%20" + ProductName + "TotalQty ->" + TotalQty + "(" + diffproduct + ")" + "&sender=VYSNVI&type=1&route=2"; 
                       
                        //string baseurl = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VFWYRA&to=" + MobNo + "&msg=%20" + ProductName + "TotalQty ->" + TotalQty + "(" + diffproduct + ")" + "&type=1";
                        Stream data = client.OpenRead(baseurl);
                        StreamReader reader = new StreamReader(data);
                        string ResponseID = reader.ReadToEnd();
                        data.Close();
                        reader.Close();
                    }


                    string message = " " + MobNo + " " + ProductName + "TotalQty ->" + TotalQty + "(" + diffproduct + ")" + " ";
                    // string text = message.Replace("\n", "\n" + System.Environment.NewLine);
                    cmd = new MySqlCommand("insert into smsinfo (agentid,branchid,mainbranch,msg,mobileno,msgtype,branchname,doe) values (@agentid,@branchid,@mainbranch,@msg,@mobileno,@msgtype,@branchname,@doe)");
                    cmd.Parameters.AddWithValue("@agentid", Session["branch"].ToString());
                    cmd.Parameters.AddWithValue("@branchid", Session["branch"].ToString());
                    cmd.Parameters.AddWithValue("@mainbranch", Session["SuperBranch"].ToString());
                    cmd.Parameters.AddWithValue("@msg", message);
                    cmd.Parameters.AddWithValue("@mobileno", MobNo);
                    cmd.Parameters.AddWithValue("@msgtype", "TripEdnd");
                    cmd.Parameters.AddWithValue("@branchname", DispatchName);
                    cmd.Parameters.AddWithValue("@doe", ServerDateCurrentdate);
                    vdm.insert(cmd);
                }
                else
                {
                    cmd = new MySqlCommand("SELECT Sno, UserName, Mobno FROM empmanage WHERE (Branch = @BranchID) AND (LevelType = 'Manager' OR LevelType = 'Director')");
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                    DataTable DtPhone = vdm.SelectQuery(cmd).Tables[0];
                    if (DtPhone.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DtPhone.Rows)
                        {
                            string PhoneNumber = dr["Mobno"].ToString();
                            if (PhoneNumber.Length == 10)
                            {
                                string Date = DateTime.Now.ToString("dd/MM/yyyy");
                                if (Session["TitleName"].ToString() == "Sri Vyshnavi Dairy Spl Pvt Ltd.")
                                {
                                    WebClient client = new WebClient();
                                    //string strUrl = " http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + no + "&msg=" + message1 + "&type=1 ";

                                    string baseurl = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + MobNo + "&message=%20" + ProductName + "TotalQty ->" + TotalQty + "(" + diffproduct + ")" + "&sender=VYSNVI&type=1&route=2"; 

                                   // string baseurl = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VSALES&to=" + MobNo + "&msg=%20" + ProductName + "TotalQty ->" + TotalQty + "(" + diffproduct + ")" + "&type=1";
                                    Stream data = client.OpenRead(baseurl);
                                    StreamReader reader = new StreamReader(data);
                                    string ResponseID = reader.ReadToEnd();
                                    data.Close();
                                    reader.Close();
                                }
                                else
                                {
                                    WebClient client = new WebClient();
                                    //string strUrl = " http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + no + "&msg=" + message1 + "&type=1 ";
                                    string baseurl = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + MobNo + "&message=%20" + ProductName + "TotalQty ->" + TotalQty + "(" + diffproduct + ")" + "&sender=VYSNVI&type=1&route=2"; 

                                    //string baseurl = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VFWYRA&to=" + MobNo + "&msg=%20" + ProductName + "TotalQty ->" + TotalQty + "(" + diffproduct + ")" + "&type=1";
                                    Stream data = client.OpenRead(baseurl);
                                    StreamReader reader = new StreamReader(data);
                                    string ResponseID = reader.ReadToEnd();
                                    data.Close();
                                    reader.Close();
                                }

                                string message = " " + MobNo + " " + ProductName + "TotalQty ->" + TotalQty + "(" + diffproduct + ")" + " ";
                                // string text = message.Replace("\n", "\n" + System.Environment.NewLine);
                                cmd = new MySqlCommand("insert into smsinfo (agentid,branchid,mainbranch,msg,mobileno,msgtype,branchname,doe) values (@agentid,@branchid,@mainbranch,@msg,@mobileno,@msgtype,@branchname,@doe)");
                                cmd.Parameters.AddWithValue("@agentid", Session["branch"].ToString());
                                cmd.Parameters.AddWithValue("@branchid", Session["branch"].ToString());
                                cmd.Parameters.AddWithValue("@mainbranch", Session["SuperBranch"].ToString());
                                cmd.Parameters.AddWithValue("@msg", message);
                                cmd.Parameters.AddWithValue("@mobileno", MobNo);
                                cmd.Parameters.AddWithValue("@msgtype", "TripEdnd");
                                cmd.Parameters.AddWithValue("@branchname", DispatchName);
                                cmd.Parameters.AddWithValue("@doe", ServerDateCurrentdate);
                                vdm.insert(cmd);
                            }
                        }
                    }
                }
                // cmd = new MySqlCommand("SELECT ROUND(SUM(tripsubdata.Qty), 2) AS Qty, products_subcategory.SubCatName, products_category.Categoryname, products_category.sno FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN branchroutes ON dispatch.Route_id = branchroutes.Sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2) GROUP BY products_subcategory.sno");
                cmd = new MySqlCommand("SELECT ROUND(SUM(tripsubdata.Qty), 2) AS Qty, products_subcategory.SubCatName, products_category.Categoryname, products_category.sno AS categorysno, products_subcategory.sno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, Status FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.Branch_Id = @branch) GROUP BY categorysno, products_subcategory.sno ORDER BY categorysno");
                cmd.Parameters.AddWithValue("@branch", Session["branch"]);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                DataTable dtTotalDespatch_subcategorywise = vdm.SelectQuery(cmd).Tables[0];
                //previous day dispatch subcategory wise
                cmd = new MySqlCommand("SELECT ROUND(SUM(tripsubdata.Qty), 2) AS Qty, products_subcategory.SubCatName, products_category.Categoryname, products_category.sno AS categorysno, products_subcategory.sno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate, Status FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2) AND (Status <> 'C')) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.Branch_Id = @branch) GROUP BY categorysno, products_subcategory.sno ORDER BY categorysno");
                cmd.Parameters.AddWithValue("@branch", Session["branch"]);
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
                DataTable dtPreviousday_subcategorywise = vdm.SelectQuery(cmd).Tables[0];

                double SubCategoryTotalQty = 0;
                double PreviousSubTotalQty = 0;
                double prevsubdiff = 0;
                double PreviousSubCategoryTotalQty = 0;
                string subcategoryName = "";
                subcategoryName += DispatchName + " Total Dispatch For Date  " + fromdate.ToString("dd/MM/yyyy") + ":" + "\r\n";

                if (dtTotalDespatch_subcategorywise.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTotalDespatch_subcategorywise.Rows)
                    {
                        double unitQty = 0;
                        double subcategorydiff = 0;
                        double.TryParse(dr["Qty"].ToString(), out unitQty);
                        if (dr["categorysno"].ToString() == "10")
                        {
                            foreach (DataRow drdtclubtotal in dtPreviousday_subcategorywise.Select("SubCatName='" + dr["SubCatName"].ToString() + "'"))
                            {
                                if (dr["SubCatName"].ToString() == drdtclubtotal["SubCatName"].ToString())
                                {
                                    double.TryParse(drdtclubtotal["Qty"].ToString(), out PreviousSubCategoryTotalQty);
                                }
                            }
                            subcategorydiff = Math.Round(unitQty - PreviousSubCategoryTotalQty, 2);
                            subcategoryName += dr["SubCatName"].ToString() + "CURD" + "->" + Math.Round(unitQty, 2) + "(" + Math.Round(subcategorydiff, 2) + ")" + ";" + "\r\n";
                        }
                        else
                        {
                            foreach (DataRow drdtclubtotal in dtPreviousday_subcategorywise.Select("SubCatName='" + dr["SubCatName"].ToString() + "'"))
                            {
                                if (drdtclubtotal["categorysno"].ToString() != "10")
                                {
                                    if (dr["SubCatName"].ToString() == drdtclubtotal["SubCatName"].ToString())
                                    {
                                        double.TryParse(drdtclubtotal["Qty"].ToString(), out PreviousSubCategoryTotalQty);
                                    }
                                }
                            }
                            subcategorydiff = Math.Round(unitQty - PreviousSubCategoryTotalQty, 2);

                            subcategoryName += dr["SubCatName"].ToString() + "->" + Math.Round(unitQty, 2) + "(" + Math.Round(subcategorydiff, 2) + ")" + ";" + "\r\n";
                        }
                        PreviousSubTotalQty += Math.Round(PreviousSubCategoryTotalQty, 2);

                        SubCategoryTotalQty += Math.Round(unitQty, 2);
                    }
                }
                prevsubdiff = Math.Round(SubCategoryTotalQty - PreviousSubTotalQty);
                if (MobNo.Length == 10)
                {
                    if (Session["TitleName"].ToString() == "Sri Vyshnavi Dairy Spl Pvt Ltd.")
                    {
                        WebClient client1 = new WebClient();
                        //string strUrl = " http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + no + "&msg=" + message1 + "&type=1 ";
                        string baseurl1 = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + MobNo + "&message=%20" + subcategoryName + "TotalQty ->" + SubCategoryTotalQty + "(" + prevsubdiff + ")" + "&sender=VYSNVI&type=1&route=2"; 
                        
                       // string baseurl1 = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VSALES&to=" + MobNo + "&msg=%20" + subcategoryName + "TotalQty ->" + SubCategoryTotalQty + "(" + prevsubdiff + ")" + "&type=1";
                        Stream data1 = client1.OpenRead(baseurl1);
                        StreamReader reader1 = new StreamReader(data1);
                        string ResponseID1 = reader1.ReadToEnd();
                        data1.Close();
                        reader1.Close();
                    }
                    else
                    {
                        WebClient client1 = new WebClient();
                        //string strUrl = " http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + no + "&msg=" + message1 + "&type=1 ";
                        string baseurl1 = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + MobNo + "&message=%20" + subcategoryName + "TotalQty ->" + SubCategoryTotalQty + "(" + prevsubdiff + ")" + "&sender=VYSNVI&type=1&route=2"; 
                        
                        //string baseurl1 = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VFWYRA&to=" + MobNo + "&msg=%20" + subcategoryName + "TotalQty ->" + SubCategoryTotalQty + "(" + prevsubdiff + ")" + "&type=1";
                        Stream data1 = client1.OpenRead(baseurl1);
                        StreamReader reader1 = new StreamReader(data1);
                        string ResponseID1 = reader1.ReadToEnd();
                        data1.Close();
                        reader1.Close();
                    }
                    string message = " " + subcategoryName + " TotalQty ->" + SubCategoryTotalQty + "(" + prevsubdiff + ")" + " ";
                    // string text = message.Replace("\n", "\n" + System.Environment.NewLine);
                    cmd = new MySqlCommand("insert into smsinfo (agentid,branchid,mainbranch,msg,mobileno,msgtype,branchname,doe) values (@agentid,@branchid,@mainbranch,@msg,@mobileno,@msgtype,@branchname,@doe)");
                    cmd.Parameters.AddWithValue("@agentid", Session["branch"].ToString());
                    cmd.Parameters.AddWithValue("@branchid", Session["branch"].ToString());
                    cmd.Parameters.AddWithValue("@mainbranch", Session["SuperBranch"].ToString());
                    cmd.Parameters.AddWithValue("@msg", message);
                    cmd.Parameters.AddWithValue("@mobileno", MobNo);
                    cmd.Parameters.AddWithValue("@msgtype", "TripEdnd");
                    cmd.Parameters.AddWithValue("@branchname", DispatchName);
                    cmd.Parameters.AddWithValue("@doe", ServerDateCurrentdate);
                    vdm.insert(cmd);
                }
                else
                {
                    cmd = new MySqlCommand("SELECT Sno, UserName, Mobno FROM empmanage WHERE (Branch = @BranchID) AND (LevelType = 'Manager' OR LevelType = 'Director')");
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                    DataTable DtPhone = vdm.SelectQuery(cmd).Tables[0];
                    if (DtPhone.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DtPhone.Rows)
                        {
                            string PhoneNumber = dr["Mobno"].ToString();
                            if (PhoneNumber.Length == 10)
                            {
                                if (Session["TitleName"].ToString() == "Sri Vyshnavi Dairy Spl Pvt Ltd.")
                                {
                                    WebClient client1 = new WebClient();

                                    //string strUrl = " http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + no + "&msg=" + message1 + "&type=1 ";
                                    string baseurl1 = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + MobNo + "&message=%20" + subcategoryName + "TotalQty ->" + SubCategoryTotalQty + "(" + prevsubdiff + ")" + "&sender=VYSNVI&type=1&route=2"; 


                                   // string baseurl1 = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VSALES&to=" + MobNo + "&msg=%20" + subcategoryName + "TotalQty ->" + SubCategoryTotalQty + "(" + prevsubdiff + ")" + "&type=1";
                                    Stream data1 = client1.OpenRead(baseurl1);
                                    StreamReader reader1 = new StreamReader(data1);
                                    string ResponseID1 = reader1.ReadToEnd();
                                    data1.Close();
                                    reader1.Close();
                                }
                                else
                                {
                                    WebClient client1 = new WebClient();

                                    //string strUrl = " http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + no + "&msg=" + message1 + "&type=1 ";

                                    string baseurl1 = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + MobNo + "&message=%20" + subcategoryName + "TotalQty ->" + SubCategoryTotalQty + "(" + prevsubdiff + ")" + "&sender=VYSNVI&type=1&route=2"; 

                                    //string baseurl1 = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VFWYRA&to=" + MobNo + "&msg=%20" + subcategoryName + "TotalQty ->" + SubCategoryTotalQty + "(" + prevsubdiff + ")" + "&type=1";
                                    Stream data1 = client1.OpenRead(baseurl1);
                                    StreamReader reader1 = new StreamReader(data1);
                                    string ResponseID1 = reader1.ReadToEnd();
                                    data1.Close();
                                    reader1.Close();
                                }

                                string message = " " + subcategoryName + " TotalQty ->" + SubCategoryTotalQty + "(" + prevsubdiff + ")" + " ";
                                // string text = message.Replace("\n", "\n" + System.Environment.NewLine);
                                cmd = new MySqlCommand("insert into smsinfo (agentid,branchid,mainbranch,msg,mobileno,msgtype,branchname,doe) values (@agentid,@branchid,@mainbranch,@msg,@mobileno,@msgtype,@branchname,@doe)");
                                cmd.Parameters.AddWithValue("@agentid", Session["branch"].ToString());
                                cmd.Parameters.AddWithValue("@branchid", Session["branch"].ToString());
                                cmd.Parameters.AddWithValue("@mainbranch", Session["SuperBranch"].ToString());
                                cmd.Parameters.AddWithValue("@msg", message);
                                cmd.Parameters.AddWithValue("@mobileno", MobNo);
                                cmd.Parameters.AddWithValue("@msgtype", "TripEdnd");
                                cmd.Parameters.AddWithValue("@branchname", DispatchName);
                                cmd.Parameters.AddWithValue("@doe", ServerDateCurrentdate);
                                vdm.insert(cmd);
                            }
                        }
                    }
                }

                //milk and curd
                cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.BranchID, SUM(tripsubdata.Qty) AS dispatchqty, products_category.Categoryname, products_category.sno AS categorysno,branchdata.BranchName FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchdata ON dispatch.BranchID = branchdata.sno WHERE (dispatch.Branch_Id = @BranchID) AND (dispatch.DispMode IS NULL) AND (products_category.sno = 10) OR (dispatch.Branch_Id = @BranchID) AND (dispatch.DispMode = 'SPL') AND (products_category.sno = 10) GROUP BY dispatch.BranchID, categorysno, branchdata.BranchName ORDER BY dispatch.BranchID");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtTotalCurd_BranchWise = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.BranchID, SUM(tripsubdata.Qty) AS dispatchqty, products_category.Categoryname, products_category.sno AS categorysno,branchdata.BranchName FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchdata ON dispatch.BranchID = branchdata.sno WHERE (dispatch.Branch_Id = @BranchID) AND (dispatch.DispMode IS NULL) AND (products_category.sno = 10) OR (dispatch.Branch_Id = @BranchID) AND (dispatch.DispMode = 'SPL') AND (products_category.sno = 10) GROUP BY dispatch.BranchID, categorysno, branchdata.BranchName ORDER BY dispatch.BranchID");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable Previous_dtTotalCurd_BranchWise = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.BranchID, SUM(tripsubdata.Qty) AS dispatchqty, products_category.Categoryname, products_category.sno AS categorysno,branchdata.BranchName FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchdata ON dispatch.BranchID = branchdata.sno WHERE (dispatch.Branch_Id = @BranchID) AND (dispatch.DispMode IS NULL) AND (products_category.sno = 9) OR (dispatch.Branch_Id = @BranchID) AND (dispatch.DispMode = 'SPL') AND (products_category.sno = 9) GROUP BY dispatch.BranchID, categorysno, branchdata.BranchName ORDER BY dispatch.BranchID");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable dtTotalMilk_BranchWise = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.BranchID, SUM(tripsubdata.Qty) AS dispatchqty, products_category.Categoryname, products_category.sno AS categorysno,branchdata.BranchName FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchdata ON dispatch.BranchID = branchdata.sno WHERE (dispatch.Branch_Id = @BranchID) AND (dispatch.DispMode IS NULL) AND (products_category.sno = 9) OR (dispatch.Branch_Id = @BranchID) AND (dispatch.DispMode = 'SPL') AND (products_category.sno = 9) GROUP BY dispatch.BranchID, categorysno, branchdata.BranchName ORDER BY dispatch.BranchID");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                DataTable Previous_dtTotalMilk_BranchWise = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.BranchID, SUM(tripsubdata.Qty) AS dispatchqty, products_category.Categoryname, products_category.sno AS categorysno,branchdata.BranchName FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchdata ON dispatch.BranchID = branchdata.sno WHERE (dispatch.Branch_Id = @branch) AND (dispatch.DispMode IS NOT NULL) AND (products_category.sno = 10) AND (dispatch.DispMode <> 'SPL') UNION SELECT dispatch_1.sno, dispatch_1.BranchID, SUM(tripsubdata_1.Qty) AS dispatchqty, products_category_1.Categoryname, products_category_1.sno AS categorysno,branchdata_1.BranchName FROM dispatch dispatch_1 INNER JOIN triproutes triproutes_1 ON dispatch_1.sno = triproutes_1.RouteID INNER JOIN (SELECT Sno, AssignDate FROM tripdata tripdata_1 WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat_1 ON triproutes_1.Tripdata_sno = tripdat_1.Sno INNER JOIN tripsubdata tripsubdata_1 ON tripdat_1.Sno = tripsubdata_1.Tripdata_sno INNER JOIN productsdata productsdata_1 ON tripsubdata_1.ProductId = productsdata_1.sno INNER JOIN products_subcategory products_subcategory_1 ON productsdata_1.SubCat_sno = products_subcategory_1.sno INNER JOIN products_category products_category_1 ON products_subcategory_1.category_sno = products_category_1.sno INNER JOIN branchdata branchdata_1 ON dispatch_1.BranchID = branchdata_1.sno WHERE (dispatch_1.Branch_Id = @branch) AND (dispatch_1.DispMode IS NOT NULL) AND (products_category_1.sno = 9) AND (dispatch_1.DispMode <> 'SPL')");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                cmd.Parameters.AddWithValue("@branch", Session["branch"]);
                DataTable dtdirectdispatch = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT dispatch.sno, dispatch.BranchID, SUM(tripsubdata.Qty) AS dispatchqty, products_category.Categoryname, products_category.sno AS categorysno,branchdata.BranchName FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchdata ON dispatch.BranchID = branchdata.sno WHERE (dispatch.Branch_Id = @branch) AND (dispatch.DispMode IS NOT NULL) AND (products_category.sno = 10) AND (dispatch.DispMode <> 'SPL') UNION SELECT dispatch_1.sno, dispatch_1.BranchID, SUM(tripsubdata_1.Qty) AS dispatchqty, products_category_1.Categoryname, products_category_1.sno AS categorysno,branchdata_1.BranchName FROM dispatch dispatch_1 INNER JOIN triproutes triproutes_1 ON dispatch_1.sno = triproutes_1.RouteID INNER JOIN (SELECT Sno, AssignDate FROM tripdata tripdata_1 WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat_1 ON triproutes_1.Tripdata_sno = tripdat_1.Sno INNER JOIN tripsubdata tripsubdata_1 ON tripdat_1.Sno = tripsubdata_1.Tripdata_sno INNER JOIN productsdata productsdata_1 ON tripsubdata_1.ProductId = productsdata_1.sno INNER JOIN products_subcategory products_subcategory_1 ON productsdata_1.SubCat_sno = products_subcategory_1.sno INNER JOIN products_category products_category_1 ON products_subcategory_1.category_sno = products_category_1.sno INNER JOIN branchdata branchdata_1 ON dispatch_1.BranchID = branchdata_1.sno WHERE (dispatch_1.Branch_Id = @branch) AND (dispatch_1.DispMode IS NOT NULL) AND (products_category_1.sno = 9) AND (dispatch_1.DispMode <> 'SPL')");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@branch", Session["branch"]);
                DataTable Previous_dtdirectdispatch = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT SUM(tripsubdata.Qty) AS dispatchqty, products_category.Categoryname, products_category.sno AS categorysno, productsdata.sno AS productsno, products_subcategory.sno AS subcategory, products_subcategory.SubCatName, productsdata.ProductName, productsdata.Inventorysno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchdata ON dispatch.BranchID = branchdata.sno WHERE (dispatch.Branch_Id = @branch) GROUP BY products_category.sno, productsdata.sno ORDER BY productsdata.Inventorysno");
                //cmd = new MySqlCommand("SELECT SUM(tripsubdata.Qty) AS dispatchqty, products_category.Categoryname, products_category.sno AS categorysno, productsdata.sno AS productsno, products_subcategory.sno AS subcategory, products_subcategory.SubCatName, productsdata.ProductName, productsdata.Inventorysno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchdata ON dispatch.BranchID = branchdata.sno WHERE (dispatch.Branch_Id = @branch) GROUP BY productsdata.Inventorysno, products_category.sno ORDER BY productsdata.Inventorysno");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
                cmd.Parameters.AddWithValue("@branch", Session["branch"]);
                DataTable dtBiProducts = vdm.SelectQuery(cmd).Tables[0];
                cmd = new MySqlCommand("SELECT SUM(tripsubdata.Qty) AS dispatchqty, products_category.Categoryname, products_category.sno AS categorysno, productsdata.sno AS productsno, products_subcategory.sno AS subcategory, products_subcategory.SubCatName, productsdata.ProductName, productsdata.Inventorysno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchdata ON dispatch.BranchID = branchdata.sno WHERE (dispatch.Branch_Id = @branch) GROUP BY products_category.sno, productsdata.sno ORDER BY productsdata.Inventorysno");
                //cmd = new MySqlCommand("SELECT SUM(tripsubdata.Qty) AS dispatchqty, products_category.Categoryname, products_category.sno AS categorysno, productsdata.sno AS productsno, products_subcategory.sno AS subcategory, products_subcategory.SubCatName, productsdata.ProductName, productsdata.Inventorysno FROM dispatch INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN (SELECT Sno, AssignDate FROM tripdata WHERE (AssignDate BETWEEN @d1 AND @d2)) tripdat ON triproutes.Tripdata_sno = tripdat.Sno INNER JOIN tripsubdata ON tripdat.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchdata ON dispatch.BranchID = branchdata.sno WHERE (dispatch.Branch_Id = @branch) GROUP BY productsdata.Inventorysno, products_category.sno ORDER BY productsdata.Inventorysno");
                cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate.AddDays(-1)));
                cmd.Parameters.AddWithValue("@branch", Session["branch"]);
                DataTable Previous_dtBiProducts = vdm.SelectQuery(cmd).Tables[0];
                double SubCategoryTotalQty1 = 0;
                double PrevSubCategoryTotalQty1 = 0;
                double GTotalDiff = 0;
                double milktotal = 0;
                double Previousmilktotal = 0;
                double curdtotal = 0;
                double Previouscurdtotal = 0;
                string subcategoryName1 = "";
                subcategoryName1 += DispatchName + "\r\n";
                subcategoryName1 += "TOTAL MILK AND CURD" + "   For Date:   " + fromdate.ToString("dd/MM/yyyy") + "\r\n";

                if (dtTotalMilk_BranchWise.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTotalMilk_BranchWise.Rows)
                    {
                        double milkdiff = 0;
                        double milkQty = 0;
                        double PreviousmilkQty = 0;
                        double milkQtyCurdqty = 0;
                        double PreviousmilkQtyCurdqty = 0;
                        double DiffmilkQtyCurdqty = 0;
                        string branchid = dr["BranchID"].ToString();
                        double.TryParse(dr["dispatchqty"].ToString(), out milkQty);
                        subcategoryName1 += dr["BranchName"].ToString() + ":" + "\r\n";
                        foreach (DataRow drdtclubtotal in Previous_dtTotalMilk_BranchWise.Select("BranchID='" + dr["BranchID"].ToString() + "'"))
                        {
                            double.TryParse(drdtclubtotal["dispatchqty"].ToString(), out PreviousmilkQty);
                        }
                        milkdiff = Math.Round(milkQty - PreviousmilkQty, 2);
                        //subcategoryName1 += "MILK" + "  ->" + Math.Round(milkQty, 2) + "(" + milkdiff + ")" + "\r\n";
                        subcategoryName1 += "MILK" + "  ->" + Math.Round(milkQty, 2) + "\r\n";
                        SubCategoryTotalQty1 += Math.Round(milkQty, 2);
                        milktotal += Math.Round(milkQty, 2);
                        Previousmilktotal += Math.Round(PreviousmilkQty, 2);
                        foreach (DataRow drcurd in dtTotalCurd_BranchWise.Rows)
                        {
                            if (dr["BranchID"].ToString() == drcurd["BranchID"].ToString())
                            {
                                double unitQty = 0;
                                double PreviousQty = 0;
                                double DiffQty = 0;
                                double.TryParse(drcurd["dispatchqty"].ToString(), out unitQty);
                                foreach (DataRow drdtclubtotal in Previous_dtTotalCurd_BranchWise.Select("BranchID='" + dr["BranchID"].ToString() + "'"))
                                {
                                    double.TryParse(drdtclubtotal["dispatchqty"].ToString(), out PreviousQty);
                                }
                                DiffQty = Math.Round(unitQty - PreviousQty, 2);

                                //subcategoryName1 += "CURD" + "  ->" + Math.Round(unitQty, 2) + "(" + DiffQty + ")" + "\r\n";
                                subcategoryName1 += "CURD" + "  ->" + Math.Round(unitQty, 2) + "\r\n";
                                SubCategoryTotalQty1 += Math.Round(unitQty, 2);
                                curdtotal += Math.Round(unitQty, 2);
                                Previouscurdtotal += Math.Round(PreviousQty, 2);
                                milkQtyCurdqty = Math.Round(milkQty + unitQty, 2);
                                PreviousmilkQtyCurdqty = Math.Round(PreviousmilkQty + PreviousQty, 2);
                            }
                        }
                        DiffmilkQtyCurdqty = Math.Round(milkQtyCurdqty - PreviousmilkQtyCurdqty, 2);
                        subcategoryName1 += "TOTAL" + "  ->" + Math.Round(milkQtyCurdqty, 2) + "(" + Math.Round(DiffmilkQtyCurdqty, 2) + ")" + "\r\n";
                    }
                    subcategoryName1 += "Direct Sale" + ":" + "\r\n";
                    double DirectmilkQtyCurdqty = 0;

                    foreach (DataRow dr in dtdirectdispatch.Rows)
                    {
                        double DirectmilkQty = 0;
                        double PreviousDirectmilkQty = 0;
                        double.TryParse(dr["dispatchqty"].ToString(), out DirectmilkQty);
                        if (dr["categorysno"].ToString() == "9")
                        {
                            subcategoryName1 += "MILK" + "  ->" + Math.Round(DirectmilkQty, 2) + "\r\n";
                            SubCategoryTotalQty1 += Math.Round(DirectmilkQty, 2);
                            foreach (DataRow drdtclubtotal in Previous_dtdirectdispatch.Select("categorysno='" + dr["categorysno"].ToString() + "'"))
                            {
                                double.TryParse(drdtclubtotal["dispatchqty"].ToString(), out PreviousDirectmilkQty);
                            }
                            milktotal += Math.Round(DirectmilkQty, 2);
                            Previousmilktotal += Math.Round(PreviousDirectmilkQty, 2);
                            DirectmilkQtyCurdqty += Math.Round(DirectmilkQty, 2);
                        }
                        if (dr["categorysno"].ToString() == "10")
                        {
                            subcategoryName1 += "CURD" + "  ->" + Math.Round(DirectmilkQty, 2) + "\r\n";
                            SubCategoryTotalQty1 += Math.Round(DirectmilkQty, 2);
                            foreach (DataRow drdtclubtotal in Previous_dtdirectdispatch.Select("categorysno='" + dr["categorysno"].ToString() + "'"))
                            {
                                double.TryParse(drdtclubtotal["dispatchqty"].ToString(), out PreviousDirectmilkQty);
                            }
                            curdtotal += Math.Round(DirectmilkQty, 2);
                            Previouscurdtotal += Math.Round(PreviousDirectmilkQty, 2);
                            DirectmilkQtyCurdqty += Math.Round(DirectmilkQty, 2);
                        }

                    }
                    subcategoryName1 += "TOTAL" + "  ->" + Math.Round(DirectmilkQtyCurdqty, 2) + "\r\n";

                    double totbuttermilk = 0;
                    double Previoustotbuttermilk = 0;
                    double totCupcurd200 = 0;
                    double totCupcurd100 = 0;
                    double totCupcurd500 = 0;
                    double totMilkcan10ltr = 0;
                    double totMilkcan5ltr = 0;
                    double totMilkcan20ltr = 0;
                    double totMilkcan40ltr = 0;
                    double totCurdcan40ltr = 0;
                    double totCurdcan20ltr = 0;
                    double totCurdcan10ltr = 0;
                    double totCurdcan5ltr = 0;
                    double totBkts10kgs = 0;
                    double totBkts5kgs = 0;
                    foreach (DataRow dr in dtBiProducts.Rows)
                    {
                        if (dr["Inventorysno"].ToString() == "1")
                        {
                            if (dr["categorysno"].ToString() == "12")
                            {
                                double buttermilk = 0;
                                double Previousbuttermilk = 0;
                                double.TryParse(dr["dispatchqty"].ToString(), out buttermilk);
                                foreach (DataRow drdtclubtotal in Previous_dtBiProducts.Select("categorysno='" + dr["categorysno"].ToString() + "'"))
                                {
                                    double.TryParse(drdtclubtotal["dispatchqty"].ToString(), out Previousbuttermilk);
                                }
                                totbuttermilk += buttermilk;
                                Previoustotbuttermilk += Previousbuttermilk;
                            }
                            if (dr["productsno"].ToString() == "101")
                            {
                                double Cupcurd200 = 0;
                                double.TryParse(dr["dispatchqty"].ToString(), out Cupcurd200);
                                totCupcurd200 += Cupcurd200;
                            }
                            if (dr["productsno"].ToString() == "100")
                            {
                                double Cupcurd100 = 0;
                                double.TryParse(dr["dispatchqty"].ToString(), out Cupcurd100);
                                totCupcurd100 += Cupcurd100;
                            }
                        }
                        if (dr["Inventorysno"].ToString() == "2")
                        {
                            if (dr["categorysno"].ToString() == "9")
                            {
                                double Milkcan10ltr = 0;
                                double.TryParse(dr["dispatchqty"].ToString(), out Milkcan10ltr);
                                totMilkcan10ltr += Milkcan10ltr;
                            }
                            if (dr["categorysno"].ToString() == "10")
                            {
                                double Curdcan10ltr = 0;
                                double.TryParse(dr["dispatchqty"].ToString(), out Curdcan10ltr);
                                totCurdcan10ltr += Curdcan10ltr;
                            }
                        }
                        if (dr["Inventorysno"].ToString() == "3")
                        {
                            if (dr["categorysno"].ToString() == "9")
                            {
                                double Milkcan20ltr = 0;
                                double.TryParse(dr["dispatchqty"].ToString(), out Milkcan20ltr);
                                totMilkcan20ltr += Milkcan20ltr;
                            }
                            if (dr["categorysno"].ToString() == "10")
                            {
                                double Curdcan20ltr = 0;
                                double.TryParse(dr["dispatchqty"].ToString(), out Curdcan20ltr);
                                totCurdcan20ltr += Curdcan20ltr;
                            }
                        }

                        if (dr["Inventorysno"].ToString() == "4")
                        {
                            if (dr["categorysno"].ToString() == "9")
                            {
                                double Milkcan40ltr = 0;
                                double.TryParse(dr["dispatchqty"].ToString(), out Milkcan40ltr);
                                totMilkcan40ltr += Milkcan40ltr;
                            }
                            if (dr["categorysno"].ToString() == "10")
                            {
                                double Curdcan40ltr = 0;
                                double.TryParse(dr["dispatchqty"].ToString(), out Curdcan40ltr);
                                totCurdcan40ltr += Curdcan40ltr;
                            }
                        }
                        if (dr["Inventorysno"].ToString() == "5")
                        {
                            if (dr["categorysno"].ToString() == "9")
                            {
                                double Milkcan5ltr = 0;
                                double.TryParse(dr["dispatchqty"].ToString(), out Milkcan5ltr);
                                totMilkcan5ltr += Milkcan5ltr;
                            }
                            if (dr["categorysno"].ToString() == "10")
                            {
                                double Curdcan5ltr = 0;
                                double.TryParse(dr["dispatchqty"].ToString(), out Curdcan5ltr);
                                totCurdcan5ltr += Curdcan5ltr;
                            }
                        }
                        if (dr["Inventorysno"].ToString() == "7")
                        {
                            if (dr["categorysno"].ToString() == "10")
                            {
                                double bkt10kgs = 0;
                                double.TryParse(dr["dispatchqty"].ToString(), out bkt10kgs);
                                totBkts10kgs += bkt10kgs;
                            }
                        }
                        if (dr["Inventorysno"].ToString() == "8")
                        {
                            if (dr["categorysno"].ToString() == "10")
                            {
                                double bkt5kgs = 0;
                                double.TryParse(dr["dispatchqty"].ToString(), out bkt5kgs);
                                totBkts5kgs += bkt5kgs;
                            }
                        }
                        if (dr["Inventorysno"].ToString() == "6")
                        {
                            if (dr["productsno"].ToString() == "140")
                            {
                                double Cupcurd500 = 0;
                                double.TryParse(dr["dispatchqty"].ToString(), out Cupcurd500);
                                totCupcurd500 += Cupcurd500;
                            }
                        }
                    }
                    double totmilkcan = totMilkcan10ltr + totMilkcan20ltr + totMilkcan40ltr + totMilkcan5ltr;
                    double totcurdcan = totCurdcan10ltr + totCurdcan20ltr + totCurdcan40ltr + totCurdcan5ltr;
                    double totcurdbucket = totBkts10kgs + totBkts5kgs;
                    double previousMilkdifftot = 0;
                    double previousCurddifftot = 0;
                    previousMilkdifftot = Math.Round(milktotal - Previousmilktotal, 2);
                    previousCurddifftot = Math.Round(curdtotal - Previouscurdtotal, 2);
                    //subcategoryName1 += "TOTAL MILK" + "  :" + Math.Round(milktotal, 2) + "(" + previousMilkdifftot + ")" + "\r\n";
                    //subcategoryName1 += "TOTAL CURD" + "  :" + Math.Round(curdtotal, 2) + "(" + previousCurddifftot + ")" + "\r\n";
                    //if (totbuttermilk > 0)
                    //{
                    //    subcategoryName1 += "BUTTER MILK" + "  :" + Math.Round(totbuttermilk, 2) + "\r\n";
                    //    SubCategoryTotalQty1 += Math.Round(totbuttermilk, 2);
                    //}
                    //PrevSubCategoryTotalQty1 = Previousmilktotal + Previouscurdtotal + Previoustotbuttermilk;
                    //GTotalDiff = Math.Round(SubCategoryTotalQty1 - PrevSubCategoryTotalQty1, 2);
                    //subcategoryName1 += "G. TOTAL" + "  :" + Math.Round(SubCategoryTotalQty1, 2) + "(" + GTotalDiff + ")" + "\r\n";
                    subcategoryName1 += "100mlBox" + "  :" + Math.Round(totCupcurd100, 2) + "\r\n";
                    subcategoryName1 += "200mlBox" + "  :" + Math.Round(totCupcurd200, 2) + "\r\n";
                    subcategoryName1 += "500mlBox" + "  :" + Math.Round(totCupcurd500, 2) + "\r\n";

                    if (totmilkcan > 0)
                    {
                        subcategoryName1 += "CAN MILK" + "  :" + Math.Round(totmilkcan, 2) + "\r\n";
                    }
                    if (totCurdcan5ltr > 0)
                    {
                        subcategoryName1 += "CURD CAN 5 ltr" + "  :" + Math.Round(totCurdcan5ltr / 5, 2) + "\r\n";
                    }
                    if (totCurdcan10ltr > 0)
                    {
                        subcategoryName1 += "CURD CAN 10 ltr" + "  :" + Math.Round(totCurdcan10ltr / 10, 2) + "\r\n";
                    }
                    if (totCurdcan20ltr > 0)
                    {
                        subcategoryName1 += "CURD CAN 20 ltr" + "  :" + Math.Round(totCurdcan20ltr / 20, 2) + "\r\n";
                    }
                    if (totCurdcan40ltr > 0)
                    {
                        subcategoryName1 += "CURD CAN 40 ltr" + "  :" + Math.Round(totCurdcan40ltr / 40, 2) + "\r\n";
                    }
                    if (totBkts5kgs > 0)
                    {
                        subcategoryName1 += "CURD BUCKET 5 kgs" + "  :" + Math.Round(totBkts5kgs / 5, 2) + "\r\n";
                    }
                    if (totBkts10kgs > 0)
                    {
                        subcategoryName1 += "CURD BUCKET 10 kgs" + "  :" + Math.Round(totBkts10kgs / 10, 2) + "\r\n";
                    }
                    subcategoryName1 += "T.MILK" + "  :" + Math.Round(milktotal, 2) + "(" + previousMilkdifftot + ")" + "\r\n";
                    subcategoryName1 += "T.CURD" + "  :" + Math.Round(curdtotal, 2) + "(" + previousCurddifftot + ")" + "\r\n";
                    if (totbuttermilk > 0)
                    {
                        subcategoryName1 += "BUTTER MILK" + "  :" + Math.Round(totbuttermilk, 2) + "\r\n";
                        SubCategoryTotalQty1 += Math.Round(totbuttermilk, 2);
                    }
                    PrevSubCategoryTotalQty1 = Previousmilktotal + Previouscurdtotal + Previoustotbuttermilk;
                    GTotalDiff = Math.Round(SubCategoryTotalQty1 - PrevSubCategoryTotalQty1, 2);
                    subcategoryName1 += "G. TOTAL" + "  :" + Math.Round(SubCategoryTotalQty1, 2) + "(" + GTotalDiff + ")" + "\r\n";
                }
                if (MobNo.Length == 10)
                {
                    if (Session["TitleName"].ToString() == "Sri Vyshnavi Dairy Spl Pvt Ltd.")
                    {
                        WebClient client2 = new WebClient();
                        //string strUrl = " http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + no + "&msg=" + message1 + "&type=1 ";
                        string baseurl2 = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + MobNo + "&message=%20" + subcategoryName1 + "&sender=VYSNVI&type=1&route=2"; 
                        //string baseurl2 = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VSALES&to=" + MobNo + "&msg=%20" + subcategoryName1 + "&type=1";
                        Stream data2 = client2.OpenRead(baseurl2);
                        StreamReader reader2 = new StreamReader(data2);
                        string ResponseID2 = reader2.ReadToEnd();
                        data2.Close();
                        reader2.Close();
                    }
                    else
                    {
                        WebClient client2 = new WebClient();
                        //string strUrl = " http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + no + "&msg=" + message1 + "&type=1 ";
                        string baseurl2 = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + MobNo + "&message=%20" + subcategoryName1 + "&sender=VYSNVI&type=1&route=2"; 
                        //string baseurl2 = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VFWYRA&to=" + MobNo + "&msg=%20" + subcategoryName1 + "&type=1";
                        Stream data2 = client2.OpenRead(baseurl2);
                        StreamReader reader2 = new StreamReader(data2);
                        string ResponseID2 = reader2.ReadToEnd();
                        data2.Close();
                        reader2.Close();
                    }
                    string message = " " + subcategoryName1 + " ";
                    // string text = message.Replace("\n", "\n" + System.Environment.NewLine);
                    cmd = new MySqlCommand("insert into smsinfo (agentid,branchid,mainbranch,msg,mobileno,msgtype,branchname,doe) values (@agentid,@branchid,@mainbranch,@msg,@mobileno,@msgtype,@branchname,@doe)");
                    cmd.Parameters.AddWithValue("@agentid", Session["branch"].ToString());
                    cmd.Parameters.AddWithValue("@branchid", Session["branch"].ToString());
                    cmd.Parameters.AddWithValue("@mainbranch", Session["SuperBranch"].ToString());
                    cmd.Parameters.AddWithValue("@msg", message);
                    cmd.Parameters.AddWithValue("@mobileno", MobNo);
                    cmd.Parameters.AddWithValue("@msgtype", "TripEdnd");
                    cmd.Parameters.AddWithValue("@branchname", DispatchName);
                    cmd.Parameters.AddWithValue("@doe", ServerDateCurrentdate);
                    vdm.insert(cmd);
                }
                else
                {
                    cmd = new MySqlCommand("SELECT Sno, UserName, Mobno FROM empmanage WHERE (Branch = @BranchID) AND (LevelType = 'Manager' OR LevelType = 'Director')");
                    cmd.Parameters.AddWithValue("@BranchID", Session["branch"]);
                    DataTable DtPhone = vdm.SelectQuery(cmd).Tables[0];
                    if (DtPhone.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DtPhone.Rows)
                        {
                            string PhoneNumber = dr["Mobno"].ToString();
                            if (PhoneNumber.Length == 10)
                            {
                                if (Session["TitleName"].ToString() == "Sri Vyshnavi Dairy Spl Pvt Ltd.")
                                {
                                    WebClient client2 = new WebClient();
                                    //string strUrl = " http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + no + "&msg=" + message1 + "&type=1 ";

                                    string baseurl2 = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + MobNo + "&message=%20" + subcategoryName1 + "&sender=VYSNVI&type=1&route=2"; 

                                   // string baseurl2 = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VSALES&to=" + MobNo + "&msg=%20" + subcategoryName1 + "&type=1";
                                    Stream data2 = client2.OpenRead(baseurl2);
                                    StreamReader reader2 = new StreamReader(data2);
                                    string ResponseID2 = reader2.ReadToEnd();
                                    data2.Close();
                                    reader2.Close();
                                }
                                else
                                {
                                    WebClient client2 = new WebClient();
                                    //string strUrl = " http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VYSNVI&to=" + no + "&msg=" + message1 + "&type=1 ";

                                    string baseurl2 = "http://roundsms.com/api/sendhttp.php?authkey=Y2U3NGE2MGFkM2V&mobiles=" + MobNo + "&message=%20" + subcategoryName1 + "&sender=VYSNVI&type=1&route=2"; 

                                    //string baseurl2 = "http://www.smsstriker.com/API/sms.php?username=vaishnavidairy&password=vyshnavi@123&from=VFWYRA&to=" + MobNo + "&msg=%20" + subcategoryName1 + "&type=1";
                                    Stream data2 = client2.OpenRead(baseurl2);
                                    StreamReader reader2 = new StreamReader(data2);
                                    string ResponseID2 = reader2.ReadToEnd();
                                    data2.Close();
                                    reader2.Close();
                                }

                                string message = " " + subcategoryName1 + " ";
                                // string text = message.Replace("\n", "\n" + System.Environment.NewLine);
                                cmd = new MySqlCommand("insert into smsinfo (agentid,branchid,mainbranch,msg,mobileno,msgtype,branchname,doe) values (@agentid,@branchid,@mainbranch,@msg,@mobileno,@msgtype,@branchname,@doe)");
                                cmd.Parameters.AddWithValue("@agentid",  Session["branch"].ToString());
                                cmd.Parameters.AddWithValue("@branchid",  Session["branch"].ToString());
                                cmd.Parameters.AddWithValue("@mainbranch",  Session["SuperBranch"].ToString());
                                cmd.Parameters.AddWithValue("@msg", message);
                                cmd.Parameters.AddWithValue("@mobileno", MobNo);
                                cmd.Parameters.AddWithValue("@msgtype", "TripEdnd");
                                cmd.Parameters.AddWithValue("@branchname", DispatchName);
                                cmd.Parameters.AddWithValue("@doe", ServerDateCurrentdate);
                                vdm.insert(cmd);
                            }
                        }
                    }
                }
                lblmsg.Text = "Message Sent Successfully";
                txtMobNo.Text = "";
            }
            else
            {
                lblmsg.Text = "You don't have access this operation.Please contact admin person";
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
    DataTable Report = new DataTable();
    void GetReport()
    {
        try
        {
            lblmsg.Text = "";
            Report = new DataTable();
            pnlHide.Visible = true;
            Session["IDate"] = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            vdm = new VehicleDBMgr();
            DateTime fromdate = DateTime.Now;
            string[] dateFromstrig = txtfromdate.Text.Split(' ');
            if (dateFromstrig.Length > 1)
            {
                if (dateFromstrig[0].Split('-').Length > 0)
                {
                    string[] dates = dateFromstrig[0].Split('-');
                    string[] times = dateFromstrig[1].Split(':');
                    fromdate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            DateTime Todate = DateTime.Now;
            string[] dateTostrig = txttodate.Text.Split(' ');
            if (dateTostrig.Length > 1)
            {
                if (dateTostrig[0].Split('-').Length > 0)
                {
                    string[] dates = dateTostrig[0].Split('-');
                    string[] times = dateTostrig[1].Split(':');
                    Todate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]), int.Parse(times[0]), int.Parse(times[1]), 0);
                }
            }
            lbl_selfromdate.Text = fromdate.ToString("dd/MM/yyyy");
            lbl_selttodate.Text = Todate.ToString("dd/MM/yyyy");
            Session["filename"] = "TOTAL DC REPORT";
            ///Ravi
            ///274854
            string bid = Session["branch"].ToString();
            cmd = new MySqlCommand("SELECT TripInfo.Sno, TripInfo.DCNo,ProductInfo.ProductName,ProductInfo.Categoryname, ProductInfo.Qty, TripInfo.I_Date, TripInfo.VehicleNo, TripInfo.Status, TripInfo.DispName, TripInfo.DispType, TripInfo.DispMode FROM (SELECT tripdata.Sno, tripdata.DCNo, tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, dispatch.DispType, dispatch.DispMode FROM            branchdata INNER JOIN dispatch ON branchdata.sno = dispatch.Branch_Id INNER JOIN triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN tripdata ON triproutes.Tripdata_sno = tripdata.Sno WHERE        (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2)) TripInfo INNER JOIN (SELECT Categoryname, ProductName, Sno, Qty FROM (SELECT products_category.Categoryname, productsdata.ProductName, tripdata_1.Sno, tripsubdata.Qty FROM            tripdata tripdata_1 INNER JOIN tripsubdata ON tripdata_1.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata_1.AssignDate BETWEEN @d1 AND @d2)) TripSubInfo) ProductInfo ON TripInfo.Sno = ProductInfo.Sno");
            //cmd = new MySqlCommand("SELECT tripdata.Sno,tripdata.Dcno, tripsubdata.Qty, productsdata.ProductName, tripdata.I_Date, tripdata.VehicleNo, tripdata.Status, dispatch.DispName, products_category.Categoryname,dispatch.DispType, dispatch.DispMode FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN triproutes ON tripdata.Sno = triproutes.Tripdata_sno INNER JOIN dispatch ON triproutes.RouteID = dispatch.sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno INNER JOIN branchdata ON dispatch.Branch_Id = branchdata.sno WHERE (dispatch.Branch_Id = @branch) AND (tripdata.AssignDate BETWEEN @d1 AND @d2) OR (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (branchdata.SalesOfficeID = @SOID)");
            cmd.Parameters.AddWithValue("@branch", Session["branch"]);
            cmd.Parameters.AddWithValue("@SOID", Session["branch"]);
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            DataTable dtble = vdm.SelectQuery(cmd).Tables[0];
            //cmd = new MySqlCommand("SELECT products_category.Categoryname, products_subcategory.SubCatName, branchproducts.Rank, productsdata.ProductName, branchproducts.branch_sno FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno INNER JOIN empmanage ON tripdata.DEmpId = empmanage.Sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (empmanage.Branch = @BranchID) AND (branchproducts.branch_sno = @Branch) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
            //cmd = new MySqlCommand("SELECT products_category.Categoryname, products_subcategory.SubCatName, branchproducts.Rank, productsdata.ProductName, branchproducts.branch_sno FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (branchproducts.branch_sno = @Branch) AND (tripdata.BranchID = @BranchID) GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
            cmd = new MySqlCommand("SELECT products_category.Categoryname, products_subcategory.SubCatName, branchproducts.Rank, productsdata.ProductName, branchproducts.branch_sno FROM tripdata INNER JOIN tripsubdata ON tripdata.Sno = tripsubdata.Tripdata_sno INNER JOIN productsdata ON tripsubdata.ProductId = productsdata.sno INNER JOIN branchproducts ON productsdata.sno = branchproducts.product_sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (tripdata.AssignDate BETWEEN @d1 AND @d2) AND (branchproducts.branch_sno = @Branch)  GROUP BY productsdata.ProductName ORDER BY branchproducts.Rank");
            //cmd.Parameters.AddWithValue("@Flag", "1");
            cmd.Parameters.AddWithValue("@d1", GetLowDate(fromdate));
            cmd.Parameters.AddWithValue("@d2", GetHighDate(Todate));
            cmd.Parameters.AddWithValue("@BranchID", Session["branch"].ToString());
            cmd.Parameters.AddWithValue("@Branch", Session["branch"].ToString());
            DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
            if (produtstbl.Rows.Count > 0)
            {
                DataView view = new DataView(dtble);
                //DataTable distinctproducts = view.ToTable(true, "ProductName");
                Report = new DataTable();
                Report.Columns.Add("SNo");
                Report.Columns.Add("VehicleNo");
                Report.Columns.Add("Ref DC No");
                Report.Columns.Add("DC No");
                Report.Columns.Add("DC Date");
                Report.Columns.Add("Route Name");
                Report.Columns.Add("DC Type");
                foreach (DataRow dr in produtstbl.Rows)
                {
                    Report.Columns.Add(dr["ProductName"].ToString()).DataType = typeof(Double);
                }
                Report.Columns.Add("Total Milk", typeof(Double));
                Report.Columns.Add("Total Curd&BM", typeof(Double));
                Report.Columns.Add("Total Lts", typeof(Double));
                Report.Columns.Add("Issued Crates", typeof(Double));
                Report.Columns.Add("Issued Cans", typeof(Double));
                Report.Columns.Add("Total Amount", typeof(Double));
                // DataTable distincttable = view.ToTable(true, "DispName", "VehicleNo", "Sno", "Status", "I_Date", "Dcno");
                DataTable distincttable = view.ToTable(true, "DispName", "VehicleNo", "Sno", "Status", "I_Date", "Dcno", "DispType", "DispMode");
                int i = 1;
                foreach (DataRow branch in distincttable.Rows)
                {
                    if (branch["Status"].ToString() == "C")
                    {
                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT invid, Qty FROM tripinvdata WHERE (Tripdata_sno = @tripid)");
                        cmd.Parameters.AddWithValue("@tripid", branch["Sno"].ToString());
                        DataTable dtissuedinv = vdm.SelectQuery(cmd).Tables[0];
                        DataRow newrow = Report.NewRow();
                        newrow["SNo"] = i;
                        newrow["VehicleNo"] = branch["VehicleNo"].ToString();
                        newrow["Ref DC No"] = branch["Sno"].ToString();
                        string BranchId = Session["branch"].ToString();
                        string DcNo = branch["Dcno"].ToString();

                        if (BranchId == "172")
                        {
                            DcNo = "P" + DcNo;
                        }
                        if (BranchId == "1801")
                        {
                            DcNo = "K" + DcNo;
                        }
                        else if (BranchId == "158")
                        {
                            DcNo = "W" + DcNo;
                        }
                        else if (BranchId == "174")
                        {
                            DcNo = "CSO" + DcNo;
                        }
                        else if (BranchId == "285")
                        {
                            DcNo = "TPT" + DcNo;
                        }
                        else if (BranchId == "282")
                        {
                            DcNo = "SKHT" + DcNo;
                        }
                        else if (BranchId == "271")
                        {
                            DcNo = "NLR" + DcNo;
                        }
                        else if (BranchId == "306")
                        {
                            DcNo = "KANCHI" + DcNo;
                        }
                        else if (BranchId == "570")
                        {
                            DcNo = "VJD" + DcNo;
                        }
                        else if (BranchId == "3")
                        {
                            DcNo = "KHM" + DcNo;
                        }
                        else if (BranchId == "159")
                        {
                            DcNo = "HYD" + DcNo;
                        }
                        else if (BranchId == "457")
                        {
                            DcNo = "WGL" + DcNo;
                        }
                        else if (BranchId == "538")
                        {
                            DcNo = "BNGLR" + DcNo;
                        }
                        else if (BranchId == "527")
                        {
                            DcNo = "PNR" + DcNo;
                        }
                        else if (BranchId == "2749")
                        {
                            DcNo = "MDPL" + DcNo;
                        }
                        else if (BranchId == "2909")
                        {
                            DcNo = "VLR" + DcNo;
                        }
                        else if (BranchId == "3559")
                        {
                            DcNo = "CTR" + DcNo;
                        }
                        newrow["DC No"] = DcNo;
                        newrow["Route Name"] = branch["DispName"].ToString();
                        string DispType = branch["DispType"].ToString();
                        string DispMode = branch["DispMode"].ToString();
                        string dctype = DispMode;
                        if (DispMode == "")
                        {
                            dctype = DispType;
                        }
                        newrow["DC Type"] = dctype;

                        string AssignDate = branch["I_Date"].ToString();
                        DateTime dtAssignDate = Convert.ToDateTime(AssignDate);
                        string ChangedTime = dtAssignDate.ToString("dd/MMM/yyyy");
                        newrow["DC Date"] = ChangedTime;

                        double total = 0;
                        double totalcurdandBM = 0;
                        foreach (DataRow dr in dtble.Rows)
                        {

                            if (branch["Sno"].ToString() == dr["Sno"].ToString())
                            {
                                double assqty = 0;
                                double curdBm = 0;
                                double Buttermilk = 0;
                                double AssignQty = 0;
                                double.TryParse(dr["Qty"].ToString(), out AssignQty);
                                newrow[dr["ProductName"].ToString()] = AssignQty;
                                if (dr["Categoryname"].ToString() == "MILK")
                                {
                                    double.TryParse(dr["Qty"].ToString(), out assqty);
                                    total += assqty;
                                }
                                if (dr["Categoryname"].ToString() == "CURD" || dr["Categoryname"].ToString() == "OTHERS" || dr["Categoryname"].ToString() == "Curd Cups" || dr["Categoryname"].ToString() == "Curd Buckets")
                                {
                                    double.TryParse(dr["Qty"].ToString(), out curdBm);
                                    totalcurdandBM += curdBm;
                                }
                                if (dr["Categoryname"].ToString() == "ButterMilk")
                                {
                                    double.TryParse(dr["Qty"].ToString(), out Buttermilk);
                                    totalcurdandBM += Buttermilk;
                                }
                            }
                        }
                        newrow["Total Milk"] = total;
                        newrow["Total Curd&BM"] = totalcurdandBM;
                        newrow["Total Lts"] = total + totalcurdandBM;
                        double cans = 0;
                        foreach (DataRow drinv in dtissuedinv.Rows)
                        {
                            string invid = drinv["invid"].ToString();
                            if (invid == "2")
                            {
                                invid = "4";
                            }
                            if (invid == "3")
                            {
                                invid = "4";
                            }
                            if (invid == "1")
                            {
                                double issuedcrates = 0;
                                double.TryParse(drinv["Qty"].ToString(), out issuedcrates);
                                newrow["Issued Crates"] = issuedcrates;
                            }
                            if (invid == "4")
                            {
                                double issuedcans = 0;
                                double.TryParse(drinv["Qty"].ToString(), out issuedcans);
                                cans += issuedcans;
                                newrow["Issued Cans"] = cans;
                            }
                        }
                        Report.Rows.Add(newrow);
                        i++;
                    }
                }

                foreach (var column in Report.Columns.Cast<DataColumn>().ToArray())
                {
                    if (Report.AsEnumerable().All(dr => dr.IsNull(column)))
                        Report.Columns.Remove(column);
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
                        if (val > 0)
                        {
                            newvartical[dc.ToString()] = val;
                        }
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
                pnlHide.Visible = false;
                lblmsg.Text = "No DC Found";
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
    //protected void btn_Export_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable("GridView_Data");
    //        int count = 0;
    //        foreach (TableCell cell in grdReports.HeaderRow.Cells)
    //        {
    //            //if (count == 5 || count == 2 || count == 3 || count == 4)
    //            //{
    //                dt.Columns.Add(cell.Text);
    //            //}
    //            //else
    //            //{
    //            //    dt.Columns.Add(cell.Text).DataType = typeof(double);
    //            //}
    //            //count++;
    //        }
    //        foreach (GridViewRow row in grdReports.Rows)
    //        {
    //            dt.Rows.Add();
    //            for (int i = 0; i < row.Cells.Count; i++)
    //            {
    //                if (row.Cells[i].Text == "&nbsp;")
    //                {
    //                    row.Cells[i].Text = "0";
    //                }
    //                dt.Rows[dt.Rows.Count - 1][i] = row.Cells[i].Text;
    //            }
    //        }
    //        using (XLWorkbook wb = new XLWorkbook())
    //        {
    //            wb.Worksheets.Add(dt);

    //            Response.Clear();
    //            Response.Buffer = true;
    //            Response.Charset = "";
    //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    //            string FileName = Session["filename"].ToString();
    //            Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");
    //            using (MemoryStream MyMemoryStream = new MemoryStream())
    //            {
    //                wb.SaveAs(MyMemoryStream);
    //                MyMemoryStream.WriteTo(Response.OutputStream);
    //                Response.Flush();
    //                Response.End();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblmsg.Text = ex.Message;
    //    }
    //}
}