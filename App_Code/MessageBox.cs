using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for MessageBox
/// </summary>

public static class MessageBox
{

  public static  string getReporttime(string time)
    {

        DateTime dt = DateTime.Parse(time);

        dt = dt.AddMinutes(-15);
        return dt.ToString("hh:mm tt");
        //string[] dtstring = time.Split(':');

        //int hours = 0;
        //int min = 0;

        //int.TryParse(dtstring[0], out hours);
        //int.TryParse(dtstring[1].Split(' ')[0], out min);

        //int totmin= hours * 

    }

    public static void Show(string message, Control owner)
    {

        Page page = (owner as Page) ?? owner.Page;

        if (page == null) return;



        //page.ClientScript.RegisterStartupScript(owner.GetType(),

        //    "ShowMessage", string.Format("<script type='text/javascript'>alert('{0}')</script>",

        //    message));

        ScriptManager.RegisterStartupScript(owner, owner.GetType(), "script", "alert('"+message+"');",true);

    }


   
}