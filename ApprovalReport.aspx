<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ApprovalReport.aspx.cs" Inherits="ApprovalReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="Js/jquery.blockUI.js?v=3005" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Css/VyshnaviStyles.css" />
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="src/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
     <link href="js/DateStyles.css?v=3004" rel="stylesheet" type="text/css" />
    <script src="js/1.8.6.jquery.ui.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#datepicker").datepicker({ dateFormat: 'yy-mm-dd', numberOfMonths: 1, showButtonPanel: false, maxDate: '+13M +0D',
            onSelect: function (selectedDate) {
            }
        });
        var date = new Date();
        var day = date.getDate();
        var month = date.getMonth() + 1;
        var year = date.getFullYear();
        if (month < 10) month = "0" + month;
        if (day < 10) day = "0" + day;
        today = year + "-" + month + "-" + day;
        $('#datepicker').val(today);
    });
    function bindapprovals(msg) {
        var length = msg.length;
        for (i = 0; i < length; i++) {
            addTable(msg[i])
        }
    }
    function addTable(msg) {
        for (var test = 0; test < 1; test++) {
            var myTableDiv = document.getElementById("myDynamicTable");

            var table = document.createElement('TABLE');
            table.border = '5';
            table.className = 'cp';
            

            var tableBody = document.createElement('TBODY');
           
            table.appendChild(tableBody);

            
                var tr = document.createElement('TR');
                tableBody.appendChild(tr);
                var header = table.createTHead();
                var row = header.insertRow(0);
                var cell = row.insertCell(0);
                cell.innerHTML = "<b>ROUTE NAME</b>";


                var cell1 = row.insertCell(1);
                var txt = msg.dispName;
                cell1.innerHTML = txt.fontcolor("green");
                cell1.style.fontWeight = "bold";
                //cell1.style.fontcolor = "green"; 

               
                    var td = document.createElement('TD');
                    td.width = '175';
                    td.appendChild(document.createTextNode("Route Name  :"));
                    tr.appendChild(td);

                    var td1 = document.createElement('TD');
                    td1.width = '175';
                    td1.appendChild(document.createTextNode(msg.dispName));
                    tr.appendChild(td1);

                    var tr1 = document.createElement('TR');
                    tableBody.appendChild(tr1);
                    var td2 = document.createElement('TD');
                    td2.width = '175';
                    td2.appendChild(document.createTextNode("Route Name  :"));
                    tr1.appendChild(td2);

                    var td3 = document.createElement('TD');
                    td3.width = '175';
                    td3.appendChild(document.createTextNode(msg.dispName));
                    tr1.appendChild(td3);

                    var tr2 = document.createElement('TR');
                    tableBody.appendChild(tr2);
                    var td4 = document.createElement('TD');
                    td4.width = '175';
                    td4.appendChild(document.createTextNode("Route Name  :"));
                    tr2.appendChild(td4);

                    var td5 = document.createElement('TD');
                    td5.width = '175';
                    td5.appendChild(document.createTextNode(msg.dispName));
                    tr2.appendChild(td5);

                    var tr3 = document.createElement('TR');
                    tableBody.appendChild(tr3);
                    var td6 = document.createElement('TD');
                    td6.width = '175';
                    td6.appendChild(document.createTextNode("Route Name  :"));
                    tr3.appendChild(td6);

                    var td7 = document.createElement('TD');
                    td7.width = '175';
                    td7.appendChild(document.createTextNode(msg.dispName));
                    tr3.appendChild(td7);

                    var tr4 = document.createElement('TR');
                    tableBody.appendChild(tr4);
                    var td8 = document.createElement('TD');
                    td8.width = '175';
                    td8.appendChild(document.createTextNode("Route Name  :"));
                    tr4.appendChild(td8);

                    var td9 = document.createElement('TD');
                    td9.width = '175';
                    td9.appendChild(document.createTextNode(msg.dispName));
                    tr4.appendChild(td9);

                    var tr5 = document.createElement('TR');
                    tableBody.appendChild(tr5);
                    var td10 = document.createElement('TD');
                    td10.width = '175';
                    td10.appendChild(document.createTextNode("Route Name  :"));
                    tr5.appendChild(td10);

                    var td11 = document.createElement('TD');
                    td11.width = '175';
                    td11.appendChild(document.createTextNode(msg.dispName));
                    tr5.appendChild(td11);

                    var tr6 = document.createElement('TR');
                    tableBody.appendChild(tr6);
                    var td12 = document.createElement('TD');
                    td12.width = '175';
                    td12.appendChild(document.createTextNode("Route Name  :"));
                    tr6.appendChild(td12);

                    var td13 = document.createElement('TD');
                    td13.width = '175';
                    td13.appendChild(document.createTextNode(msg.dispName));
                    tr6.appendChild(td13);

                    var tr7 = document.createElement('TR');
                    tableBody.appendChild(tr7);
                    var td14 = document.createElement('TD');
                    td14.width = '175';
                    td14.appendChild(document.createTextNode("Route Name  :"));
                    tr7.appendChild(td14);

                    var td15 = document.createElement('TD');
                    td15.width = '175';
                    td15.appendChild(document.createTextNode(msg.dispName));
                    tr7.appendChild(td15);

                    var tr8 = document.createElement('TR');
                    tableBody.appendChild(tr8);
                    var td16 = document.createElement('TD');
                    td16.width = '175';
                    td16.appendChild(document.createTextNode("Route Name  :"));
                    tr8.appendChild(td16);
                    var td19 = document.createElement('TD');
                    td19.width = '175';
                    td19.appendChild(document.createTextNode(msg.dispName));
                    tr8.appendChild(td19);

                    var tr9 = document.createElement('TR');
                    tableBody.appendChild(tr9);
                    var td17 = document.createElement('TD');
                    td17.width = '175';
                    var btn = document.createElement("BUTTON");
                    var t = document.createTextNode("CLICK ME");
                    btn.appendChild(t);
                    td17.appendChild(btn);
                    tr9.appendChild(td17);

                    var td18 = document.createElement('TD');
                    td18.width = '175';
                    var btn1 = document.createElement("BUTTON");
                    var t1 = document.createTextNode("Raise Query");
                    btn1.appendChild(t1);
                    td18.appendChild(btn1);

                    tr9.appendChild(td18);
                //}
           // }
            myTableDiv.appendChild(table);

        }


    }
    function btnGeneratedetails() {

        var date = document.getElementById('datepicker').value;
        if (date == "" || date == "mm/dd/yyyy") {
            alert("Please Select Date");
            return false;
        }
        var data = { 'operation': 'GetApprovalDetails', 'IndentDate': date };
        var s = function (msg) {
            if (msg) {
                bindapprovals(msg);
            }
            else {
            }
        };
        var e = function (x, h, e) {
        };
        $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);

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
<style type="text/css">
   
.cp{
    /* these styles will let the divs line up next to each other
       while accepting dimensions */
    display: inline-block;

    width: 300px;
    height: 300px;
    /*background: black;*/

    /* a small margin to separate the blocks */
    margin-left: 10px;
    margin-right: 10px;
    margin-top: 10px;
    margin-bottom: 10px;
    font-family:Candara;
    color:#333333;
    border-width: 1px;
	border-color: #999999;
	border-collapse: collapse;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div style="width: 100%; background-color: #fff">
<input type="button" id="create" value="Click here" onclick="addTable()" />
<div id="myDynamicTable"  >
<table>
        <tr>
            <td>
                <label>
                     Date:</label>
            </td>
            <td>
                <input type="text" name="journey_date" class="datepicker" tabindex="3" readonly="readonly"
                    id="datepicker" placeholder="DD-MM-YYYY" />
            </td>
            <td>
                <input type="button" id="btnGenerate" value="Generate" class="SaveButton" onclick="btnGeneratedetails();"
                    style="width: 120px; height: 30px; font-size: 16px;" />
            </td>
        </tr>
    </table>
    </div>
</div>
</asp:Content>

