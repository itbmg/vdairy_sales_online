using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class exporttoxl : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["xportdata"] != null)
        {
            DataTable dtt = (DataTable)Session["xportdata"];
            ExportToExcel(dtt);
        }

    }

    //public void ExportToExcel(DataTable dt)
    //{
    //    try
    //    {
    //        if (dt.Rows.Count > 0)
    //        {
    //            string filename = "Report.xls";
    //            System.IO.StringWriter tw = new System.IO.StringWriter();
    //            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
    //            DataGrid dgGrid = new DataGrid();
    //            dgGrid.DataSource = dt;
    //            dgGrid.DataBind();

    //            //Get the HTML for the control.
    //            dgGrid.RenderControl(hw);
    //            //Write the HTML back to the browser.
    //            //Response.ContentType = application/vnd.ms-excel;
    //            Response.ContentType = "application/vnd.ms-excel";
    //            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
    //            this.EnableViewState = false;
    //            Response.Write(tw.ToString());
    //            Response.End();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    public void ExportToExcel(DataTable dtt)
    {
        try
        {
            if (dtt.Rows.Count > 0)
            {
                //string filename = "Report.xls";
                //System.IO.StringWriter tw = new System.IO.StringWriter();
                //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                //DataGrid dgGrid = new DataGrid();
                //dgGrid.DataSource = dt;
                //dgGrid.DataBind();

                ////Get the HTML for the control.
                //dgGrid.RenderControl(hw);
                ////Write the HTML back to the browser.
                ////Response.ContentType = application/vnd.ms-excel;
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                //this.EnableViewState = false;
                //Response.Write(tw.ToString());
                //Response.End();
                string filena = Session["filename"].ToString();
                //string title = Session["title"].ToString();
                string filename = "";
                if (filena != "" && filena != null)
                {
                    filename = filena;
                }
                else
                {
                    filename = "Report";
                }

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");

                HttpContext.Current.Response.Charset = "utf-8";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
                //sets font
                HttpContext.Current.Response.Write("<font style='font-size:10.0pt;'>");
                HttpContext.Current.Response.Write("<BR><BR><BR>");
                //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
                HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                  "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
                  "style='font-size:10.0pt; background:white;'> <TR>");
                int columnscount = dtt.Columns.Count;
                //For Header

                //HttpContext.Current.Response.Write("<Td colspan='" + columnscount + "' align='center' style='font-size:30.0pt;'>Sri Vyshnavi Dairy Specialities (P) Ltd</Td><TR>");
                //HttpContext.Current.Response.Write("<Td colspan='" + columnscount + "' align='center' style='font-size:20.0pt;'>" + title + "</Td><TR>");

                //am getting my grid's column headers


                for (int j = 0; j < columnscount; j++)
                {      //write in new column
                    HttpContext.Current.Response.Write("<Td style='font-size:14.0pt;'>");
                    //Get column headers  and make it as bold in excel columns
                    HttpContext.Current.Response.Write("<B>");
                    HttpContext.Current.Response.Write(dtt.Columns[j].ColumnName.ToString());
                    HttpContext.Current.Response.Write("</B>");
                    HttpContext.Current.Response.Write("</Td>");
                }
                HttpContext.Current.Response.Write("</TR>");
                foreach (DataRow row in dtt.Rows)
                {//write in new row
                    HttpContext.Current.Response.Write("<TR>");
                    for (int i = 0; i < dtt.Columns.Count; i++)
                    {
                        HttpContext.Current.Response.Write("<Td>");
                        HttpContext.Current.Response.Write(row[i].ToString());
                        HttpContext.Current.Response.Write("</Td>");
                    }

                    HttpContext.Current.Response.Write("</TR>");
                }
                HttpContext.Current.Response.Write("</Table>");
                HttpContext.Current.Response.Write("</font>");
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();





            }
        }
        catch (Exception ex)
        {
        }
    }
}