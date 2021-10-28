<%@ WebHandler Language="C#" Class="exporttoxl_utility" %>

using System;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

public class exporttoxl_utility : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
        string filename = "filename";
        if (context.Session["filename"] != null)
        {
            filename = context.Session["filename"].ToString();
        }
        string attachment = "attachment; filename=" + filename + ".xls";
        context.Response.AddHeader("content-disposition", attachment);
        context.Response.ContentType = "application/ms-excel";
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        System.Data.DataTable dtt = (System.Data.DataTable)context.Session["xportdata"];
        string maintitle = "";
        string AgentName = "";
        string RouteName = "";
        string SalesMan = "";
        string xporttype = "";
        string Date = "";
        if (context.Session["xporttype"] != null)
        {
            xporttype = context.Session["xporttype"].ToString();
        }
        if (xporttype == "Incentive Report")
        {
            if (context.Session["agentname"] != null)
            {
                AgentName = context.Session["agentname"].ToString();
            }
            if (context.Session["routename"] != null)
            {
                RouteName = context.Session["routename"].ToString();
            }
            if (context.Session["date"] != null)
            {
                Date = context.Session["date"].ToString();
            }

            string title = "Sri Vyshnavi Dairy Specialities (P) Ltd";
            if (context.Session["title"] != null)
            {
                title = context.Session["title"].ToString();
            }
            // ExportToExcel(dtt);
            string exportContent = ExportToExcel(xporttype, title, dtt, maintitle, AgentName, RouteName, Date);
            response.Write(exportContent);
        }
        else if (xporttype == "TallySales" || xporttype == "TallyReceipts" || xporttype == "TallyPayments" || xporttype == "TallyInward" || xporttype == "TallyLekas" || xporttype == "TallyIncentive" || xporttype == "TallyCashStaff")
        {
            string title = "";
            string exportContent = ExportToExcel(xporttype, title, dtt, maintitle, AgentName, RouteName, Date);
            response.Write(exportContent);
        }
        else
        {
            if (context.Session["IDate"] != null)
            {
                AgentName = context.Session["IDate"].ToString();
            }
            if (context.Session["RouteName"] != null)
            {
                RouteName = context.Session["RouteName"].ToString();
            }
            if (context.Session["UserName"] != null)
            {
                SalesMan = context.Session["UserName"].ToString();
            }
            string title = "Sri Vyshnavi Dairy Specialities (P) Ltd";
            if (context.Session["title"] != null)
            {
                title = context.Session["title"].ToString();
            }
            string exportContent = ExportToExcel(xporttype, title, dtt, maintitle, AgentName, RouteName, Date);
            response.Write(exportContent);
        }
    }
    private string ExportToExcel(string xporttype, string title, System.Data.DataTable dtt, string maintitle, string AgentName, string RouteName,string Date)
    {
        string _exportContent = "";
        Table table = new Table();
        using (StringWriter sb = new StringWriter())
        {
            using (HtmlTextWriter htmlWriter = new HtmlTextWriter(sb))
            {
                table.GridLines = GridLines.Both;
                if (xporttype == "TallySales" || xporttype == "TallyReceipts" || xporttype == "TallyPayments" || xporttype == "TallyInward" || xporttype == "TallyLekas" || xporttype == "TallyIncentive" || xporttype == "TallyCashStaff")
                {
                }
                else
                {
                    TableRow xltitlerow = new TableRow();
                    TableCell xltitlerowcell = new TableCell();
                    xltitlerowcell.Text = xporttype;
                    xltitlerowcell.Font.Bold = true;
                    xltitlerowcell.BackColor = System.Drawing.Color.White;
                    xltitlerowcell.ForeColor = System.Drawing.Color.Blue;
                    xltitlerowcell.HorizontalAlign = HorizontalAlign.Center;
                    xltitlerowcell.BorderStyle = BorderStyle.None;
                    xltitlerowcell.Font.Size = 14;
                    xltitlerow.Cells.Add(xltitlerowcell);
                    table.Rows.Add(xltitlerow);
                    TableRow trow2 = new TableRow();
                    TableCell tcell = new TableCell();
                    tcell.Text = title;
                    tcell.Font.Bold = true;
                    tcell.HorizontalAlign = HorizontalAlign.Center;
                    tcell.BackColor = System.Drawing.Color.White;
                    tcell.ForeColor = System.Drawing.Color.Blue;
                    tcell.BorderStyle = BorderStyle.None;
                    tcell.Font.Bold = true;
                    tcell.Font.Size = 32;
                    trow2.Cells.Add(tcell);
                    table.Rows.Add(trow2);
                    TableRow trow1 = new TableRow();
                    tcell = new TableCell();
                    tcell.Text = "Agent Name";
                    tcell.Font.Bold = true;
                    tcell.HorizontalAlign = HorizontalAlign.Left;
                    tcell.BackColor = System.Drawing.Color.White;
                    tcell.ForeColor = System.Drawing.Color.Blue;
                    tcell.BorderStyle = BorderStyle.None;
                    tcell.Font.Bold = true;
                    tcell.Font.Size = 14;
                    trow1.Cells.Add(tcell);
                    table.Rows.Add(trow1);
                    tcell = new TableCell();
                    tcell.Text = AgentName;
                    tcell.Font.Bold = true;
                    // tcell.HorizontalAlign = HorizontalAlign.Left;
                    tcell.BackColor = System.Drawing.Color.White;
                    tcell.ForeColor = System.Drawing.Color.Red;
                    tcell.BorderStyle = BorderStyle.None;
                    tcell.Font.Bold = true;
                    tcell.Font.Size = 10;
                    trow1.Cells.Add(tcell);
                    table.Rows.Add(trow1);

                    tcell = new TableCell();
                    tcell.Text = "Route Name";
                    tcell.Font.Bold = true;
                    tcell.HorizontalAlign = HorizontalAlign.Center;
                    tcell.BackColor = System.Drawing.Color.White;
                    tcell.ForeColor = System.Drawing.Color.Blue;
                    tcell.BorderStyle = BorderStyle.None;
                    tcell.Font.Bold = true;
                    tcell.Font.Size = 14;
                    trow1.Cells.Add(tcell);
                    table.Rows.Add(trow1);
                    tcell = new TableCell();
                    tcell.Text = RouteName;
                    tcell.Font.Bold = true;
                    //tcell.HorizontalAlign = HorizontalAlign.Center;
                    tcell.BackColor = System.Drawing.Color.White;
                    tcell.ForeColor = System.Drawing.Color.Red;
                    tcell.BorderStyle = BorderStyle.None;
                    tcell.Font.Bold = true;
                    tcell.Font.Size = 10;
                    trow1.Cells.Add(tcell);
                    table.Rows.Add(trow1);
                    tcell = new TableCell();
                    tcell.Text = "Date";
                    tcell.Font.Bold = true;
                    tcell.HorizontalAlign = HorizontalAlign.Right;

                    tcell.BackColor = System.Drawing.Color.White;
                    tcell.ForeColor = System.Drawing.Color.Blue;
                    tcell.BorderStyle = BorderStyle.None;
                    tcell.Font.Bold = true;
                    tcell.Font.Size = 14;
                    trow1.Cells.Add(tcell);
                    table.Rows.Add(trow1);
                    tcell = new TableCell();
                    tcell.Text = Date;
                    tcell.Font.Bold = true;
                    //tcell.HorizontalAlign = HorizontalAlign.Right;
                    tcell.BackColor = System.Drawing.Color.White;
                    tcell.ForeColor = System.Drawing.Color.Red;
                    tcell.BorderStyle = BorderStyle.None;
                    tcell.Font.Bold = true;
                    tcell.Font.Size = 10;
                    trow1.Cells.Add(tcell);
                    table.Rows.Add(trow1);
                }
                TableRow trow = new TableRow();
                for (int j = 0; j < dtt.Columns.Count; j++)
                {
                    TableCell cell123 = new TableCell();
                    cell123.Text = dtt.Columns[j].ColumnName;
                    trow.Cells.Add(cell123);
                }
                table.Rows.Add(trow);

                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    TableRow row = new TableRow();
                    //row.BackColor = (i % 2 == 0) ? System.Drawing.Color.BlanchedAlmond :
                    //                           System.Drawing.Color.BurlyWood;
                    for (int j = 0; j < dtt.Columns.Count; j++)
                    {
                        TableCell cell = new TableCell();
                        cell.Text = dtt.Rows[i][j].ToString();
                        row.Cells.Add(cell);

                    }
                    table.Rows.Add(row);
                }
                if (xporttype == "TallySales" || xporttype == "TallyReceipts" || xporttype == "TallyPayments" || xporttype == "TallyInward" || xporttype == "TallyLekas" || xporttype == "TallyIncentive" || xporttype == "TallyCashStaff")
                {
                }
                else
                {
                    table.Rows[0].Cells[0].ColumnSpan = dtt.Columns.Count;
                    table.Rows[1].Cells[0].ColumnSpan = dtt.Columns.Count;
                }
                //table.Rows[0].Cells.RemoveAt(1);
                table.RenderControl(htmlWriter);
                _exportContent = sb.ToString();
            }
        }

        //System.IO.StringWriter tw = new System.IO.StringWriter();
        //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        //DataGrid dgGrid = new DataGrid();
        //dgGrid.DataSource = table;
        //dgGrid.DataBind();

        ////Get the HTML for the control.
        //dgGrid.RenderControl(hw);

        //_exportContent = tw.ToString();
        return _exportContent;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}