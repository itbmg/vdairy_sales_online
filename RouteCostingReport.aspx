<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RouteCostingReport.aspx.cs" Inherits="RouteCostingReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
        <h1>
            RouteWiseCostingReport<small>Preview</small>
        </h1>
        <ol class="breadcrumb" >
            <li><a href="#">RouteWiseCostingReport</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <i style="padding-right: 5px;" class="fa fa-cog"></i>RouteWiseCostingReport
                </h3>
            </div>
            <div class="box-body">
                <br />
                <table align="center">
                <tr>
                  <td>
                                    From Date
                                </td>
                                <td>
                                    <input type="date" id="txtFromDate" class="formDate" />
                                </td>
                                <td>
                                    To Date
                                </td>
                                <td>
                                    <input type="date" id="txtToDate" class="formDate" />
                                </td>
                                <td  style="width:6px;"></td>
                                <td> 
                                    <input type="button" id="btnGeneretae" value="Generate" onclick="BtnGenerateRoutecostingClick();"
                                        class="btn btn-primary" />
                                </td>
                </tr>
                </table>
                <br />
                  <div id="divPrint">
                 <div id="div_Routecostingdata"></div>
                </div>
                <br />
                 <button type="button" class="btn btn-primary" style="margin-right: 5px;" onclick="javascript:CallPrint('divPrint');"><i class="fa fa-print"></i> Print </button>
                </div>
                </div>
                </section>
</asp:Content>

