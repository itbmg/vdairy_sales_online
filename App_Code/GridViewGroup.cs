using System;
using System.Data;
using System.Web.UI.WebControls;

public class GridViewGroup
{
    private int m_columnIndex;
    private GridViewGroup m_parentGroup;
    private string m_groupingColumn;
    private GridView m_gridView;

    private GridViewRow m_firstGroupGridRow = null;
    private DataRow m_firstGroupDataRow = null;
    private int m_rowSpan = 0;
    private bool m_groupChanged;

    public GridViewGroup(GridView gridView, GridViewGroup parentGroup, string groupingColumn)
    {
        if (parentGroup != null)
        {
            m_columnIndex = parentGroup.ColumnIndex + 1;
        }
        m_parentGroup = parentGroup;
        m_groupingColumn = groupingColumn;
        m_gridView = gridView;
        HookGrid();
    }

    private void HookGrid()
    {
        m_gridView.RowDataBound += new GridViewRowEventHandler(GridView_RowDataBound);
        m_gridView.DataBound += new EventHandler(GridView_DataBound);
    }

    private void GridView_DataBound(object sender, EventArgs e)
    {
        if (m_rowSpan > 1)
        {
            m_firstGroupGridRow.Cells[m_columnIndex].RowSpan = m_rowSpan;
        }
    }

    private void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        m_groupChanged = false;
        DataRowView currentDataRowView = (DataRowView)e.Row.DataItem;
        if (currentDataRowView != null)
        {
            DataRow currentDataRow = currentDataRowView.Row;
            if (m_firstGroupDataRow != null)
            {
                object lastValue = m_firstGroupDataRow[m_groupingColumn];
                object currentValue = currentDataRow[m_groupingColumn];
                if (currentValue.Equals(lastValue) && !(m_parentGroup != null && m_parentGroup.GroupChanged))
                {
                    m_rowSpan++;
                    e.Row.Cells[m_columnIndex].Visible = false;
                    return;
                }
                else
                {
                    if (e.Row.RowIndex > 0 && m_parentGroup == null)
                    {
                        GridViewRow row = m_gridView.Rows[e.Row.RowIndex - 1];
                        row.Style.Add("border-bottom", "1px solid lightgray;");
                    }
                }
                if (m_rowSpan > 1)
                {
                    m_firstGroupGridRow.Cells[m_columnIndex].RowSpan = m_rowSpan;
                    if (e.Row.RowIndex > 0 && m_parentGroup == null)
                    {
                        GridViewRow row = m_gridView.Rows[e.Row.RowIndex - 1];
                        row.Style.Add("border-bottom", "1px solid lightgray;");
                    }
                }
            }
            m_firstGroupGridRow = e.Row;
            m_firstGroupDataRow = currentDataRow;
            m_rowSpan = 1;
            m_groupChanged = true;
        }
    }

    public int ColumnIndex
    {
        get
        {
            return m_columnIndex;
        }
    }

    public bool GroupChanged
    {
        get
        {
            return m_groupChanged;
        }
    }
}