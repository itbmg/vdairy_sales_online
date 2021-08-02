using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ApprovalReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //total indent sale value
//SELECT        dispatch.DispName, dispatch.Branch_Id, dispatch.sno, SUM(indents_subtable.unitQty) AS unitqty, SUM(indents_subtable.DeliveryQty) AS saleqty, 
//                         SUM(indents_subtable.DeliveryQty * indents_subtable.UnitCost) AS salevalue
//FROM            dispatch INNER JOIN
//                         dispatch_sub ON dispatch.sno = dispatch_sub.dispatch_sno INNER JOIN
//                         branchroutesubtable ON dispatch_sub.Route_id = branchroutesubtable.RefNo INNER JOIN
//                         indents ON branchroutesubtable.BranchID = indents.Branch_id INNER JOIN
//                         indents_subtable ON indents.IndentNo = indents_subtable.IndentNo
//WHERE        (dispatch.Branch_Id = 174) AND (indents.I_date BETWEEN @d1 AND @d2)
//GROUP BY dispatch.sno

        //total dispatch

//        SELECT        dispatch.DispName, dispatch.Branch_Id, dispatch.sno, SUM(tripsubdata.Qty) AS dispatchqty
//FROM            dispatch INNER JOIN
//                         triproutes ON dispatch.sno = triproutes.RouteID INNER JOIN
//                         tripsubdata ON triproutes.Tripdata_sno = tripsubdata.Tripdata_sno INNER JOIN
//                         tripdata ON tripsubdata.Tripdata_sno = tripdata.Sno
//WHERE        (dispatch.Branch_Id = 174) AND (tripdata.I_Date BETWEEN @d1 AND @d2)
//GROUP BY dispatch.sno
    }
}