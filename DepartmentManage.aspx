<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DepartmentManage.aspx.cs" Inherits="DepartmentManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="jquery-ui-1.10.3.custom/css/ui-lightness/jquery-ui-1.10.3.custom.css"
        rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="jquery.jqGrid-4.5.2/plugins/searchFilter.css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
     <link href="Styles/Style.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
         <style>
        .divselectedclass
        {
            border: 1px solid gray;
            padding-top: 2px;
            padding-bottom: 2px;
        }
        .back-red
        {
            background-color: #ffffcc;
        }
        .back-white
        {
            background-color: #ffffff;
        }
        
         .btn-glyphicon {
    padding:8px;
    background:#ffffff;
    margin-right:2px;
    color:blue;
}
        .btn-glyphicon1 {
    padding:8px;
    background:#ffffff;
    margin-right:2px;
    color:red;
}
.icon-btn {
    padding: 1px 13px 3px 2px;
    border-radius:50px;
}
    </style>
    <script type="text/javascript">
        $(function () {
            window.history.forward(1);

        });
    </script>
    <script>
        $(function () {
            updatedepartment_manage();
        });
        function departmentsave_click() {
            var username = document.getElementById("txt_department_name").value;
            if (username == "") {
                alert("Please Provide Department Name");
                $("#txt_department_name").focus();
                return false;
            }
            else {
                departmentsave();
            }
        }
        function departmentsave() {
            var txtdepartmentname = document.getElementById('txt_department_name').value;
            var operationtype = document.getElementById('btn_dept_save').value;
            var sno = serial;
            var data = { 'operation': 'departmentsave', 'sno': sno, 'txtdepartmentname': txtdepartmentname, 'operationtype': operationtype };

            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    department_clear();
                    updatedepartment_manage();
                }
                else {
                }
            };
            var e = function (x, h, e) {
                if (x.status && x.status == 400) {
                    alert(x.responseText);
                    window.location.assign("Login.aspx");
                }
                else {
                    alert("something went wrong");
                }
            };
            callHandler(data, s, e);
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        }
        function department_clear() {
            document.getElementById('txt_department_name').value = "";
            document.getElementById('btn_dept_save').value = "SAVE";
            serial = 0;
        }
        function updatedepartment_manage() {
            var data = { 'operation': 'updatedepartment_manage' };
            var s = function (msg) {
                if (msg) {
                    BindGrid_department_manage(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
                if (x.status && x.status == 400) {
                    alert(x.responseText);
                    window.location.assign("Login.aspx");
                }
                else {
                    alert("something went wrong");
                }
            };
            callHandler(data, s, e);
        }
        var serial = 0;
        function BindGrid_department_manage(msg) {
            var l = 0;
            var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
            var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
            results += '<thead><tr><th scope="col"></th><th scope="col">Department Name</th></tr></thead></tbody>';
            for (var i = 0; i < msg.length; i++) {
                results += '<tr style="background-color:' + COLOR[l] + '"><td><input id="btn_poplate" type="button"  onclick="getme(this)" name="submit" class="btn btn-primary" value="Edit" /></td>';
                results += '<td scope="row" class="1" >' + msg[i].DeptName + '</td>';
                results += '<td style="display:none" class="2">' + msg[i].sno + '</td></tr>';
                l = l + 1;
                if (l == 4) {
                    l = 0;
                }
            }
            results += '</table></div>';
            $("#div_Deptdata").html(results);
        }
        function getme(thisid) {
            var DeptName = $(thisid).parent().parent().children('.1').html();
            var sno = $(thisid).parent().parent().children('.2').html();
            document.getElementById('txt_department_name').value = DeptName;
            document.getElementById('btn_dept_save').value = "Modify";
            serial = sno;
        }
        function callHandler(d, s, e) {
            $.ajax({
                url: 'DairyFleet.axd',
                data: d,
                type: 'GET',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            Dept Master<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Masters</a></li>
            <li><a href="#">Dept Master</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Dept Master Details
                </h3>
            </div>
            <div class="box-body">
                <table align="center">
                    <tr>
                        <td>
                            <label id="lbldepartment">
                                Department Name:</label>
                        </td>
                        <td style="height:40px;">
                            <input type="text" id="txt_department_name" class="form-control" placeholder="Enter Department Name" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label id="lbldeptflag">
                                Flag:</label>
                        </td>
                        <td style="height:40px;">
                            <select id="cmb_dept_flag" class="form-control">
                                <option>Active</option>
                                <option>InActive</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <input type="button" id="btn_dept_save" value="SAVE" class="btn btn-primary" onclick="return departmentsave_click();" />
                            <input type="button" id="btn_dept_clear" value="CLEAR" class="btn btn-warning" onclick="return department_clear();" />
                        </td>
                    </tr>
                </table>
                <br />
                </div>
        </div>
                <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>Dept  list
                </h3>
            </div>
                <div id="div_Deptdata"></div>
            </div>
    </section>
</asp:Content>
