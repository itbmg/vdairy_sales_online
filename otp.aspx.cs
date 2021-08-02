using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using System.Net;
using System.Xml;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO;
using System.Web.SessionState;


public partial class otp : System.Web.UI.Page
{
    static SqlCommand cmd;
    static AccessControldbmanger db = new AccessControldbmanger();
    static SqlConnection con;
    string currdatetime = string.Empty;
    string type = string.Empty;
    static string QId = string.Empty;
    static string msg = string.Empty;
    static int status = 0;


    protected void Page_Load(object sender, EventArgs e)
    {

        if (IsPostBack != true)
        {
            try
            {
                if (Request.QueryString["Id"] != null)
                {
                    currdatetime = System.DateTime.Now.ToString();
                    Lbl_Date.Text = currdatetime;
                    QId = Decrypt(HttpUtility.UrlDecode(Request.QueryString["Id"].Trim()));//mobile no       
                    // QId = "9490009933";
                    Session["Id"] = QId.ToString();
                    LoadData();
                    // QId = "9092691491"; 
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

    }

    private string Decrypt(string cipherText)
    {
        string EncryptionKey = "V99Y34S44H9N0A0V6I";
        cipherText = cipherText.Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }

    private void LoadData()
    {
        try
        {
            Lbl_Approvaltype.ForeColor = System.Drawing.Color.Red;
            DataTable dt = new DataTable();
            Lbl_Approvaltype.Text = " ";
            using (con = db.GetConnection())
            {
                string str = "SELECT Top 1 * FROM VysAuthentication  where val1='" + QId.Trim() + "' AND val10='1' ORDER BY ID DESC ";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Lbl_Approvaltype.Text = dr["val4"].ToString() + "  _  " + dr["val5"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }
    // Method called from client-side JavaScript
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public static string save(string otpval)
    {
        try
        {
            string otp = string.Empty;
            string projid = string.Empty;
            string frmid = string.Empty;
            string uniqid = string.Empty;
            string dt1 = string.Empty;
            int diff1 = 0;
            int diff2 = 0;
            int diff3 = 0;
            int h = 0;
            int m = 0;
            int s = 0;
            int h2 = 0;
            int m2 = 0;
            int s2 = 0;
            DataTable dt = new DataTable();
            DataTable dat1 = new DataTable();
            DataTable dt3 = new DataTable();
            string[] t1 = new string[3];
            string[] t2 = new string[3];
            string successstatus = string.Empty;
            //server Time 
            DateTime dt2 = serverdt();

            int h1 = Convert.ToInt32(dt2.ToString("HH"));
            int m1 = 0;
            string otpexptime = string.Empty;
            string sss = string.Empty;
            string mm = string.Empty;
            m1 = Convert.ToInt32(dt2.ToString("mm"));
            int ss = Convert.ToInt32(dt2.ToString("ss"));
            if (ss > 60)
            {
                ss = ss - 60;
            }
            if (ss < 10)
            {
                sss = "0" + m1.ToString();
            }

            if (m1 > 60)
            {
                m1 = m1 - 60;
            }
            if (m1 < 10)
            {
                mm = "0" + m1.ToString();
                if (ss < 10)
                {
                    otpexptime = h1.ToString() + ":" + mm.ToString() + ":" + sss.ToString();
                }
                else
                {
                    otpexptime = h1.ToString() + ":" + mm.ToString() + ":" + ss.ToString();
                }
            }
            else
            {
                if (ss < 10)
                {
                    otpexptime = h1.ToString() + ":" + m1.ToString() + ":" + sss.ToString();
                }
                else
                {
                    otpexptime = h1.ToString() + ":" + m1.ToString() + ":" + ss.ToString();
                }
            }
            using (con = db.GetConnection())
            {
                string str = "SELECT Top 1 * FROM VysAuthentication  where val1='" + QId.Trim() + "' AND val10='1' ORDER BY ID DESC ";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        otp = dr["val2"].ToString();
                        dt1 = dr["val3"].ToString();
                        projid = dr["val4"].ToString();
                        frmid = dr["val5"].ToString();
                        uniqid = dr["val7"].ToString();

                        t1 = dt1.Split(':');
                        h = Convert.ToInt32(t1[0]);
                        m = Convert.ToInt32(t1[1]);
                        s = Convert.ToInt32(t1[2]);

                        t2 = otpexptime.Split(':');
                        h2 = Convert.ToInt32(t2[0]);
                        m2 = Convert.ToInt32(t2[1]);
                        s2 = Convert.ToInt32(t2[2]);

                        diff1 = h - h2;
                        diff2 = m - m2;
                        diff3 = s - s2;
                        if (diff1 == 0)
                        {
                            if (diff2 >= 0 && diff2 <= 3)
                            {
                                string str3 = "SELECT ID,VAl2 FROM VysAuthentication  where val1='" + QId.Trim() + "' AND val10='1' AND VAl2='" + otpval + "'  ORDER BY ID DESC ";
                                SqlDataAdapter da3 = new SqlDataAdapter(str3, con);
                                da3.Fill(dt3);
                                if (dt3.Rows.Count > 0)
                                {
                                    foreach (DataRow dr3 in dt3.Rows)
                                    {
                                        successstatus = dr3["ID"].ToString();
                                        //Otpupdate(projid, frmid, uniqid);
                                        Successstausupdate(successstatus);
                                        msg = "Transaction Completed Sccessfully...";
                                    }
                                }
                                else
                                {
                                    msg = "Please,Check the OTP Data...";
                                }
                            }
                            else
                            {
                                msg = "Timeout Error...";
                            }
                        }
                        else
                        {
                            msg = "Please try again...";
                        }
                    }
                }
                else
                {
                    msg = "Please Enter the Valid OTP...";
                }
            }
            return msg;
        }
        catch (Exception ex)
        {
            return msg;
        }
    }

    private static DateTime serverdt()
    {
        DateTime dt1 = new DateTime();
        try
        {
            using (con = db.GetConnection())
            {
                DataTable dt = new DataTable();
                string str = "SELECT   getdate() AS CurrDate";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dt1 = Convert.ToDateTime(dr["CurrDate"]);
                        return dt1;
                    }
                }
            }
            return dt1;
        }
        catch (Exception ex)
        {
            return dt1;
        }

    }

    //private static void Otpupdate(string projid, string frmid, string uniqid)
    //{
    //    try
    //    {
    //        SqlCommand cmd = new SqlCommand();
    //        SqlConnection con1 = new SqlConnection();
    //        if (projid == "Procurement" && frmid == "Billproceed")
    //        {
    //            using (con = db.GetConnection1())
    //            {
    //                cmd = new SqlCommand("update AdminApproval set Astatus='2' where Plant_code=@pcode ", con);
    //                cmd.Parameters.AddWithValue("@pcode", uniqid);
    //                cmd.ExecuteNonQuery();
    //            }
    //        }
    //        else if (projid == "HRMS" && frmid == "EmployeeJoinApproval")
    //        {
    //            string staus = string.Empty;
    //            using (con = db.GetConnection())
    //            {
    //                string str = string.Empty;
    //                DataTable dt = new DataTable();
    //                str = "SELECT status from employedetails  where empid='" + uniqid + "'";
    //                dt = db.GetDatatable(str);
    //                if (dt.Rows.Count > 0)
    //                {
    //                    foreach (DataRow dr in dt.Rows)
    //                    {
    //                        staus = dr["status"].ToString();
    //                    }
    //                }
    //                //if (staus == "No")
    //                //{
    //                //    cmd = new SqlCommand("update employedetails set status='Yes' where empid=@empid ", con);
    //                //    cmd.Parameters.AddWithValue("@empid", uniqid);
    //                //    cmd.ExecuteNonQuery();
    //                //}
    //                //else
    //                //{
    //                //    cmd = new SqlCommand("update employedetails set status='No' where empid=@empid ", con);
    //                //    cmd.Parameters.AddWithValue("@empid", uniqid);
    //                //    cmd.ExecuteNonQuery();
    //                //}
    //            }
    //        }
    //        else if (projid == "PurchaseStores" && frmid == "poApproval")
    //        {
    //            string staus = string.Empty;


    //            string str = string.Empty;
    //            DataTable dt = new DataTable();
    //            str = "SELECT status from po_entrydetailes  where sno='" + uniqid + "'";
    //            //cmd = new SqlCommand(str, con1);
    //            con1 = db.GetConnection2();
    //            SqlDataAdapter da = new SqlDataAdapter(str, con1);
    //            da.Fill(dt);


    //            if (dt.Rows.Count > 0)
    //            {
    //                foreach (DataRow dr in dt.Rows)
    //                {
    //                    staus = dr["status"].ToString();
    //                }
    //            }

    //            if (staus == "P")
    //            {
    //                cmd = new SqlCommand("update po_entrydetailes set status='A' where sno=@sno ", con1);
    //                cmd.Parameters.AddWithValue("@sno", uniqid);
    //                cmd.ExecuteNonQuery();
    //            }
    //            else
    //            {
    //                cmd = new SqlCommand("update po_entrydetailes set status='P' where sn=@sno ", con1);
    //                cmd.Parameters.AddWithValue("@sno", uniqid);
    //                cmd.ExecuteNonQuery();
    //            }

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ex.ToString();
    //    }
    //}
    private static void Successstausupdate(string id)
    {
        try
        {
            using (con = db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("update VysAuthentication set val9='2' where ID=@id ", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public static string Resent(string otpval)
    {
        try
        {
            string msg = string.Empty;
            string no = string.Empty;
            string message1 = string.Empty;

            DataTable dt = new DataTable();
            using (con = db.GetConnection())
            {
                string str = "SELECT Top 1 * FROM VysAuthentication  where val1='" + QId.Trim() + "' AND val10='1' ORDER BY ID DESC ";
                SqlDataAdapter da = new SqlDataAdapter(str, con);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        message1 = "OTP for  " + dr["val4"].ToString() + "  _  " + dr["val5"].ToString() + "  transaction is : " + dr["val2"].ToString() + ". Valid till " + dr["val3"].ToString() + "  Do not share OTP for security reasons.";
                        no = dr["val1"].ToString();
                        string strUrl = "http://103.16.101.52:8080/sendsms/bulksms?username=kapd-vyshnavi&password=vysavi&type=0&dlr=1&destination=" + no + "&source=VFLEET&message=%20" + message1 + "";
                        WebRequest request = HttpWebRequest.Create(strUrl);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        Stream s = (Stream)response.GetResponseStream();
                        StreamReader readStream = new StreamReader(s);
                        string dataString = readStream.ReadToEnd();
                        response.Close();
                        s.Close();
                        readStream.Close();
                        msg = "OTP Resend Successfully...";
                    }
                }
            }
            return msg;
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
}