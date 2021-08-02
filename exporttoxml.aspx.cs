using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using MySql.Data.MySqlClient;

public partial class exporttoxml : System.Web.UI.Page
{
    MySqlCommand cmd;
    string UserName = "";
    VehicleDBMgr vdm;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public void ConnectionXML()
    {
        vdm = new VehicleDBMgr();
        cmd = new MySqlCommand("SELECT products_category.Categoryname, products_subcategory.SubCatName, productsdata.ProductName FROM branchproducts INNER JOIN dispatch ON branchproducts.branch_sno = dispatch.Branch_Id INNER JOIN productsdata ON branchproducts.product_sno = productsdata.sno INNER JOIN products_subcategory ON productsdata.SubCat_sno = products_subcategory.sno INNER JOIN products_category ON products_subcategory.category_sno = products_category.sno WHERE (dispatch.Branch_Id = @BranchId) group by productsdata.ProductName  ORDER BY productsdata.Rank");
        cmd.Parameters.AddWithValue("@BranchId", Session["branch"].ToString());
        DataTable produtstbl = vdm.SelectQuery(cmd).Tables[0];
        if (produtstbl.Rows.Count > 0)
        {
            grdReports.DataSource = produtstbl;
            grdReports.DataBind();
        }
        // Get a StreamWriter object
        using (XmlWriter writer = XmlWriter.Create("~/FileTest/productsdata.xml"))
        {
            writer.WriteStartDocument();
            writer.WriteEndDocument();
        }
        StreamWriter xmlDoc = new StreamWriter(Server.MapPath("~/FileTest/productsdata.xml"), false);

        // Apply the WriteXml method to write an XML document
        produtstbl.WriteXml(xmlDoc);
        xmlDoc.Close();

    }
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        ConnectionXML();
    }
}