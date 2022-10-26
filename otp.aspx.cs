using System;
using System.Web;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.IO;

public partial class otp : System.Web.UI.Page
{
    static SqlCommand cmd;
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

}