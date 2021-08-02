using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AgentSales : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //INSERT INTO Book (book_name, book_copy) VALUES ('AAAA',(SELECT copy FROM ( SELECT IFNULL(MAX(book_copy),0)+1 as copy FROM Book) AS d));
    }
}