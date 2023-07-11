using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Web.Services;
using System.Net.Mail;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.IO;
public partial class Login : System.Web.UI.Page
{
    MySqlCommand cmd;
    VehicleDBMgr vdm;
    SqlCommand a_cmd;
    string ipaddress;
    public string username, pwd;
    protected void Page_Load(object sender, EventArgs e)
    {

        vdm = new VehicleDBMgr();
        var Empid = Request.QueryString["id"];
        username = string.Empty;
        pwd = string.Empty;
        username = Request.QueryString["username"];
        pwd = Request.QueryString["pwd"];
        try
        {
            if (username.Length > 0 && username != null && username != "")
            {

                txtUserName.Text = username.Trim();
                txtPassword.Text = pwd.Trim();
                Loginfo();
            }
        }
        catch (Exception ex)
        {
        }

    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        Loginfo();
    }
    DataTable dtemp = new DataTable();
    private void Loginfo()
    {
        try
        {
            String UserName = txtUserName.Text, PassWord = txtPassword.Text;
            cmd = new MySqlCommand("SELECT empmanage.Sno, IFNULL(empmanage.otpstatus,0) otpstatus, empmanage.UserName,empmanage.grouplogin, empmanage.Password, empmanage.LevelType, empmanage.flag, empmanage.Userdata_sno, empmanage.Owner, empmanage.EmpName, empmanage.Address, empmanage.Mobno, empmanage.Email, empmanage.LWC, empmanage.RefName, empmanage.Dept_Sno, branchdata.BranchName,branchdata.Radius, empmanage.Branch, salestypemanagement.salestype FROM empmanage INNER JOIN branchdata ON empmanage.Branch = branchdata.sno INNER JOIN salestypemanagement ON branchdata.SalesType = salestypemanagement.sno WHERE (empmanage.UserName = @UN) AND (empmanage.Password = @Pwd) AND (empmanage.flag ='1')");
            cmd.Parameters.Add("@UN", UserName);
            cmd.Parameters.Add("@Pwd", PassWord);
            dtemp = vdm.SelectQuery(cmd).Tables[0];
            string LevelType = "";
            if (dtemp.Rows.Count > 0)
            {

                DateTime Currentdate = VehicleDBMgr.GetTime(vdm.conn);
                string hostName = Dns.GetHostName(); // Retrive the Name of HOST
                //get ip address and device type
                ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (ipaddress == "" || ipaddress == null)
                {
                    ipaddress = Request.ServerVariables["REMOTE_ADDR"];
                }
                HttpBrowserCapabilities browser = Request.Browser;
                string devicetype = "";
                string userAgent = Request.ServerVariables["HTTP_USER_AGENT"];
                Regex OS = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Regex device = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                string device_info = string.Empty;
                if (OS.IsMatch(userAgent))
                {
                    device_info = OS.Match(userAgent).Groups[0].Value;
                }
                if (device.IsMatch(userAgent.Substring(0, 4)))
                {
                    device_info += device.Match(userAgent).Groups[0].Value;
                }
                if (!string.IsNullOrEmpty(device_info))
                {
                    devicetype = device_info;
                    string[] words = devicetype.Split(')');
                    devicetype = words[0].ToString();
                }
                else
                {
                    devicetype = "Desktop";
                }
                cmd = new MySqlCommand("INSERT INTO logininfo(UserId, UserName, Logintime, IpAddress,devicetype,status) values (@userid, @UserName, @logintime, @ipaddress,@devicetype,@status)");
                cmd.Parameters.Add("@userid", dtemp.Rows[0]["sno"].ToString());
                cmd.Parameters.Add("@UserName", dtemp.Rows[0]["EmpName"].ToString());
                cmd.Parameters.Add("@logintime", Currentdate);
                cmd.Parameters.Add("@ipaddress", ipaddress);
                cmd.Parameters.Add("@devicetype", devicetype);
                cmd.Parameters.Add("@status", "1");
                vdm.insert(cmd);
                //Session["stateid"] = dtBranch.Rows[0]["stateid"].ToString();
                Session["UserSno"] = dtemp.Rows[0]["Sno"].ToString();

                string empid = dtemp.Rows[0]["Sno"].ToString();
                //Session["moduleid"] = ModuleId;
                LevelType = dtemp.Rows[0]["LevelType"].ToString();
                string otpstatus = dtemp.Rows[0]["otpstatus"].ToString();
                if (otpstatus == "1")
                {
                    string Id = string.Empty;
                    string no = "";
                    no = dtemp.Rows[0]["Mobno"].ToString();
                    string numbers = "1234567890";
                    string characters = numbers;
                    int length = 6;
                    string otp = string.Empty;
                    for (int i = 0; i < length; i++)
                    {
                        string character = string.Empty;
                        do
                        {
                            int index = new Random().Next(0, characters.Length);
                            character = characters.ToCharArray()[index].ToString();
                        } while (otp.IndexOf(character) != -1);
                        otp += character;
                    }
                    DateTime sdt = VehicleDBMgr.GetTime(vdm.conn);//DBManager.GetTime(vdm.conn);
                    int h = Convert.ToInt32(sdt.ToString("HH"));
                    int m = 0;
                    string otpexptime = string.Empty;
                    string sss = string.Empty;
                    string mm = string.Empty;
                    m = Convert.ToInt32(sdt.ToString("mm")) + 3;
                    int ss = Convert.ToInt32(sdt.ToString("ss"));
                    if (ss > 60)
                    {
                        ss = ss - 60;
                    }
                    if (ss < 10)
                    {
                        sss = "0" + m.ToString();
                    }
                    if (m > 60)
                    {
                        m = m - 60;
                    }
                    if (m < 10)
                    {

                        if (ss < 10)
                        {
                            mm = "0" + m.ToString();
                            otpexptime = h.ToString() + ":" + mm.ToString() + ":" + sss.ToString();
                        }
                        else
                        {
                            mm = m.ToString();
                            otpexptime = h.ToString() + ":" + mm.ToString() + ":" + ss.ToString();
                        }
                    }
                    else
                    {
                        if (ss < 10)
                        {
                            otpexptime = h.ToString() + ":" + m.ToString() + ":" + sss.ToString();
                        }
                        else
                        {
                            otpexptime = h.ToString() + ":" + m.ToString() + ":" + ss.ToString();
                        }

                    }
                    Id = Encrypt(no.Trim());
                    string hyperlink = " http://182.18.162.51/fp/OTP.aspx?Id=" + Id.Trim();
                    string message1 = "OTP for  " + empid + "  Billproceed  transaction is : " + otp + ". Valid till " + otpexptime + "  Do not share OTP for security reasons.";
                    string strUrl = "http://103.16.101.52:8080/sendsms/bulksms?username=kapd-vyshnavi&password=vysavi&type=0&dlr=1&destination=" + no + "&source=VYSHRM&message=" + message1 + "";
                    WebRequest request = HttpWebRequest.Create(strUrl);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream s = (Stream)response.GetResponseStream();
                    StreamReader readStream = new StreamReader(s);
                    string dataString = readStream.ReadToEnd();
                    response.Close();
                    s.Close();
                    readStream.Close();
                    string msg = hyperlink;
                    //string hyperlink = " http://182.18.162.51/fp/OTP.aspx?Id=" + Id.Trim();
                    Response.Redirect("OTP.aspx?Id=" + Id.Trim());
                }
                else
                {
                    Session["userdata_sno"] = dtemp.Rows[0]["UserData_sno"].ToString();
                    Session["empid"] = dtemp.Rows[0]["Sno"].ToString();
                    cmd = new MySqlCommand("SELECT sno, branchid, empid FROM branchmonitering WHERE  (empid = @empid)");
                    cmd.Parameters.Add("@empid", dtemp.Rows[0]["Sno"].ToString());
                    DataTable empmonitor = vdm.SelectQuery(cmd).Tables[0];
                    if (empmonitor.Rows.Count > 1)
                    {
                        Response.Redirect("SwitchAccount.aspx", false);
                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT branchmappingtable.SuperBranch, branchdata.BranchName, branchdata.stateid, branchdata.gstno, branchdata.statename, statemastar.statename AS BranchState, statemastar.gststatecode FROM branchmappingtable INNER JOIN empmanage ON branchmappingtable.SubBranch = empmanage.Branch INNER JOIN branchdata ON empmanage.Branch = branchdata.sno INNER JOIN statemastar ON branchdata.stateid = statemastar.sno WHERE (empmanage.UserName = @UN)");
                        cmd.Parameters.Add("@UN", UserName);
                        DataTable dtBranch = vdm.SelectQuery(cmd).Tables[0];
                        string PlantID = "";
                        string Branch = dtemp.Rows[0]["Branch"].ToString();
                        if (dtBranch.Rows.Count > 0)
                        {
                            PlantID = dtBranch.Rows[0]["SuperBranch"].ToString();
                            Session["SuperBranch"] = dtBranch.Rows[0]["SuperBranch"].ToString();

                            Session["TitleName"] = "SRI VYSHNAVI FOODS (P) LTD";
                            Session["Address"] = " Near Ayyappa Swamy Temple, Shasta Nagar, WYRA-507165,KHAMMAM (District), TELANGANA (State).Phone: 08749 – 251326, Fax: 08749 – 252198.";
                            Session["TinNo"] = "36550160129";

                            Session["BranchName"] = dtBranch.Rows[0]["BranchName"].ToString();
                        }
                        Session["EmpSno"] = dtemp.Rows[0]["sno"].ToString();
                        Session["gstin"] = dtBranch.Rows[0]["gstno"].ToString();
                        Session["statename"] = dtBranch.Rows[0]["BranchState"].ToString();
                        Session["statecode"] = dtBranch.Rows[0]["gststatecode"].ToString();
                        Session["stateid"] = dtBranch.Rows[0]["stateid"].ToString();
                        Session["UserSno"] = dtemp.Rows[0]["Sno"].ToString();
                        Session["userdata_sno"] = dtemp.Rows[0]["UserData_sno"].ToString();
                        Session["UserName"] = dtemp.Rows[0]["UserName"].ToString();
                        Session["EmpName"] = dtemp.Rows[0]["EmpName"].ToString();
                        if ("232" == dtemp.Rows[0]["Sno"].ToString())
                        {
                            Session["branch"] = "172";
                            Session["salestype"] = "Plant";
                        }
                        else
                        {

                            Session["branch"] = dtemp.Rows[0]["Branch"].ToString();
                            Session["salestype"] = dtemp.Rows[0]["salestype"].ToString();
                        }
                        Session["empid"] = dtemp.Rows[0]["Sno"].ToString();
                        Session["LevelType"] = dtemp.Rows[0]["LevelType"].ToString();
                        Session["GroupLogin"] = dtemp.Rows[0]["grouplogin"].ToString();
                        Session["branchname"] = dtemp.Rows[0]["BranchName"].ToString();
                        string ModuleId = "1";
                        Session["moduleid"] = ModuleId;
                        if (LevelType == "Opperations" || LevelType == "SODispatcher" || LevelType == "Users" || LevelType == "Dispatcher")
                        {
                            Response.Redirect("Order_Report.aspx", false);
                        }
                        if (LevelType == "PlantDispatcher")
                        {
                            Response.Redirect("PlanDespatch.aspx", false);
                        }
                        if (LevelType == "Accountant")
                        {
                            Response.Redirect("TripEnd.aspx", false);
                        }
                        if (LevelType == "Manager" || LevelType == "MAdmin" || LevelType == "Director")
                        {
                            cmd = new MySqlCommand("SELECT formname FROM  formdetails ORDER BY sno  DESC LIMIT 1");
                            DataTable dtform = vdm.SelectQuery(cmd).Tables[0];

                            string formname = dtform.Rows[0]["formname"].ToString();
                            //Response.Redirect("liveboard.aspx", false);
                            Response.Redirect(formname, false);
                        }
                        if (LevelType == "Admin")
                        {
                            Response.Redirect("Delivery_Collection_Report.aspx", false);
                        }
                        if (LevelType == "AccountsOfficer")
                        {
                            Response.Redirect("liveboard.aspx", false);
                        }
                        if (LevelType == "SalesManager")
                        {
                            Response.Redirect("Delivery_Collection_Report.aspx", false);
                        }
                        if (LevelType == "Casheir")
                        {
                            Response.Redirect("cashform.aspx", false);
                        }
                        if (LevelType == "Security")
                        {
                            Response.Redirect("Gatepass.aspx", false);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Enter Correct UserID and Password", this);
            }
        }
        catch (Exception ex)
        {
            lbl_validation.Text = ex.Message;
        }
    }
    private string Encrypt(string clearText)
    {
        string EncryptionKey = "V99Y34S44H9N0A0V6I";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    protected void sessionsclick_Close(object sender, EventArgs e)
    {
        //this.AlertBox.Visible = false;
        Response.Redirect("Login.aspx");
    }
}