<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SummaryDashBoard.aspx.cs" Inherits="SummaryDashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="Kendo/jquery.min.js" type="text/javascript"></script>
    <script src="Kendo/kendo.all.min.js" type="text/javascript"></script>
    <link href="Kendo/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="Kendo/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <script src="https://www.amcharts.com/lib/3/amcharts.js"></script>
    <script src="https://www.amcharts.com/lib/3/pie.js"></script>
    <script src="https://www.amcharts.com/lib/3/plugins/export/export.min.js"></script>
    <link rel="stylesheet" href="https://www.amcharts.com/lib/3/plugins/export/export.css"
        type="text/css" media="all" />
    <script src="https://www.amcharts.com/lib/3/themes/light.js"></script>
    <script src="https://www.amcharts.com/lib/3/serial.js"></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" />
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <link href="Css/VyshnaviStyles.css" rel="stylesheet" type="text/css" />
    <link href="Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <link href="Css/RouteWiseTimeLine.css" rel="stylesheet" type="text/css" />
    <link href="Css/AdminLTE.css" rel="stylesheet" type="text/css" />
    <link href="Css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <%-- <script type="text/javascript" src="https://cdn.jsdelivr.net/jquery/latest/jquery.min.js"></script>--%>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/momentjs/latest/moment.min.js"></script>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <section class="content-header">
        <h1>
           Dash Board<small>Preview</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Dash Board</a></li>
            <li><a href="#">Dash Board</a></li>
        </ol>
    </section>
    <section class="content">
        <div class="box box-info">
            <div class="box-body">
                <div style="width: 100%; padding-left: 30%;">
                <table>
                    <tr>
                        <td style="width: 130px;">
                            <select id="ddlType" class="form-control" onchange="ddlTypeChange(this);">
                                <option>Vyshnavi Group</option>
                                <option>SVDS</option>
                                <option>SVF</option>
                                <option>Plant Wise</option>
                            </select>
                        </td>
                         <td style="width:2%;">
                        </td>
                        <td style="width: 130px; display: none;" id="divPlant">
                            <select id="ddlPlant" class="form-control" onchange="ddlPlantNameChange(this);">
                            </select>
                        </td>
                        <td style="width:2%;">
                        </td>
                        <td style="width: 130px;" id="tdddlbarnchCategory">
                           <select id="ddlbarnchCategory" class="form-control" >
                                <option>BranchWise</option>
                                <option>BranchDayComaprison</option>
                                <option>CategoryWise</option>
                            </select>
                        </td>
                        <td style="width:2%;">
                        </td>
                        <td>
                            <div id="reportrange" style="background: #fff; cursor: pointer; padding: 5px 10px;
                                border: 1px solid #ccc; width: 100%">
                                <i class="fa fa-calendar"></i>&nbsp; <span></span><i class="fa fa-caret-down"></i>
                            </div>
                        </td>
                        <td>
                            &nbsp; &nbsp;
                        </td>
                        <td>
                            <input type="button" id="submit" value="Submit" class="btn btn-primary" onclick="GraphicalNetSaleClick()" />
                        </td>
                    </tr>
                </table>
            </div>
             <br />
    <div style="display: none;" id="firstdiv">
            <div class="col-md-12" id="SpanDetails">
              <div class="box box-solid box-warning">
                 <div class="box-header with-border">
                        <h3 class="fa fa-inbox">
                            <i style="padding-right: 5px;"></i>Sales Details
                        </h3>
                        <div class="box-tools pull-left">
                            <button class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                 <div class="box-body">
                        <div class="box-body no-padding">
                            <div  class="col-lg-3 col-xs-6" id="spnDispatchQty" style="width: 82%;padding-left: 250px;">
                                <!-- small box -->
                                <div class="small-box bg-aqua">
                                    <div class="inner">
                                        <h3 id="hdispatchqty">
                                            0
                                        </h3>
                                        <p>
                                            Despatch Qty</p>
                                    </div>
                                    <div class="icon">
                                        <i class="fa fa-cubes"></i>
                                    </div>
                                    <a href="#datagrid" class="small-box-footer" onclick="branchwisedispqty();" data-toggle="modal"
                                        data-target="#myModal">BranchWise Dispatchqty <i class="fa fa-arrow-circle-right">
                                        </i></a>
                                </div>
                            </div>
                            <!-- ./col -->
                            <div class="col-lg-3 col-xs-6" id="spnSaleValue">
                                <!-- small box -->
                                <div class="small-box bg-purple">
                                    <div class="inner">
                                        <h3 id="hsalevalue">
                                            0
                                        </h3>
                                        <p>
                                            Sale Value</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-pie-graph"></i>
                                    </div>
                                    <a href="#datagrid" class="small-box-footer" onclick="branchwisesaledetails();" data-toggle="modal"
                                        data-target="#myModal">BranchWise Sale Value <i class="fa fa-arrow-circle-right">
                                        </i></a>
                                </div>
                            </div>
                            <!-- ./col -->
                            <div class="col-lg-3 col-xs-6" id="spnMilkDispatchQty" style="height: 20px;">
                                <!-- small box -->
                                <div class="small-box bg-olive">
                                    <div class="inner">
                                        <h3 id="hmilkqty">
                                            0
                                        </h3>
                                        <p>
                                            Milk Qty</p>
                                    </div>
                                    <div class="icon">
                                        <i class="fa fa-thumbs-o-up"></i>
                                    </div>
                                    <a href="#datagrid" class="small-box-footer" onclick="branchwisemilkdetails();" data-toggle="modal"
                                        data-target="#myModal">BranchWise Milk Qty <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                            <div class="col-lg-3 col-xs-6" id="spnCurdDispatchQty">
                                <!-- small box -->
                                <div class="small-box bg-teal">
                                    <div class="inner">
                                        <h3 id="hcurdqty">
                                            0
                                        </h3>
                                        <p>
                                            Curd Qty</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-stats-bars"></i>
                                    </div>
                                    <a href="#datagrid" class="small-box-footer" onclick="branchwiseCurddetails();" data-toggle="modal"
                                        data-target="#myModal">BranchWise Curd Qty <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                            <!-- ./col -->
                            <div class="col-lg-3 col-xs-6" id="spnOthersDispatchQty">
                                <!-- small box -->
                                <div class="small-box bg-yellow">
                                    <div class="inner">
                                        <h3 id="hothersqty">
                                            0
                                        </h3>
                                        <p>
                                            Others Qty</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-bag"></i>
                                    </div>
                                    <a href="#datagrid" class="small-box-footer" onclick="branchwiseotherdetails();"
                                        data-toggle="modal" data-target="#myModal">BranchWise Others Qty <i class="fa fa-arrow-circle-right">
                                        </i></a>
                                </div>
                            </div>
                            <div class="col-lg-3 col-xs-6" id="spnCollection">
                                <!-- small box -->
                                <div class="small-box bg-teal">
                                    <div class="inner">
                                        <h3 id="spnamount">
                                            0
                                        </h3>
                                        <p>
                                            Collection</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-bag"></i>
                                    </div>
                                    <a href="#datagrid" class="small-box-footer" onclick="BranchWiseCollection();" data-toggle="modal"
                                        data-target="#myModal">Branch Collection <i class="fa fa-arrow-circle-right"></i>
                                    </a>
                                </div>
                            </div>
                            <div class="col-lg-3 col-xs-6" id="spnDueValue">
                                <!-- small box -->
                                <div class="small-box bg-blue">
                                    <div class="inner">
                                        <h3 id="hduevalue">
                                            0
                                        </h3>
                                        <p>
                                            Due Value</p>
                                    </div>
                                    <div class="icon">
                                        <i class="ion ion-bag"></i>
                                    </div>
                                    <a href="#datagrid" class="small-box-footer" onclick="BranchWiseDueDetails();" data-toggle="modal"
                                        data-target="#myModal">Branch Due details<i class="fa fa-arrow-circle-right"></i></a>
                                </div>
                            </div>
                        </div>
                    </div>
            </div>
            </div>
            <div  class="col-md-12" id="Branch_wise_Dispatch">
                <div id="BranchWiseDespatch">
                </div>
            </div>
            <div class="col-sm-12" style="width:100%;display:none;" id="div_MainPlantComparison">
                    <div class="box box-solid box-info">
                        <div class="box-header with-border">
                            <h3 class="ion ion-clipboard">
                                <i style="padding-right: 5px;"></i>Sale Quantity Comparison
                            </h3>
                            <div class="box-tools pull-left">
                                <button class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="box-body no-padding">
                                <div>
                                    <div id="div_PlantComparison">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
             </div>
            <div class="col-sm-12" style="width:100%;display:none;" id="div_MainBranchComparison">
                    <div class="box box-solid box-info">
                        <div class="box-header with-border">
                            <h3 class="ion ion-clipboard">
                                <i style="padding-right: 5px;"></i>Sale Quantity Comparison
                            </h3>
                            <div class="box-tools pull-left">
                                <button class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="box-body no-padding">
                                <div>
                                    <div id="divComaprison">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
             </div>
            <div id="example1" class="k-content" class="col-sm-12">
                <div class="col-sm-6">
                   <div class="box box-solid box-danger">
                                <div class="box-header with-border">
                                    <h3 class="ion ion-clipboard">
                                        <i style="padding-right: 5px;"></i>SubCategory  Wise Information
                                    </h3>
                                    <div class="box-tools pull-left">
                                        <button class="btn btn-box-tool" data-widget="collapse">
                                            <i class="fa fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="box-body no-padding">
                                        <div>
                                            <div id="tableProductData">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                 </div>
            </div>
        <div id="divHide" style="width: 120%; display: none;">
        <div class="modal fade in" id="div_MainPlantDetails" style="display: none; padding-right: 17px;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true" onclick="CloseClick1();">×</span></button>
                        <h4 class="modal-title">
                            Plant Wise Details</h4>
                    </div>
                    <div class="modal-body" id="div_PlantDetails" style="height: 400px; overflow-y: scroll;">
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" onclick="PlantCloseClick();">
                            Close</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
         <div class="modal fade in" id="divMainAddNewRow1" style="display: none; padding-right: 17px;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true" onclick="CloseClick1();">×</span></button>
                        <h4 class="modal-title">
                            Branch Wise Details</h4>
                    </div>
                    <div class="modal-body" id="divChart" style="height: 400px; overflow-y: scroll;">
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" onclick="CloseClick1();">
                            Close</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        </div>
        <div class="modal fade in" id="div_routewisemain" style="display: none; padding-right: 17px;width: 110%;">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true" onclick="close_routewise();">×</span></button>
                                <h4 class="modal-title">
                                    Route Wise Details</h4>
                            </div>
                            <div class="modal-body" id="div_routewise" style="height: 400px; overflow-y: scroll;">
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-danger" onclick="close_routewise();">
                                    Close</button>
                            </div>
                        </div>
                        <!-- /.modal-content -->
                    </div>
                    <!-- /.modal-dialog -->
                </div>
        <div class="modal fade in" id="div_agentwisemain" style="display: none; padding-right: 17px;">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true" onclick="close_agentwise();">×</span></button>
                                <h4 class="modal-title">
                                    Agent Wise Details</h4>
                            </div>
                            <div class="modal-body" id="div_agentwise" style="height: 400px; overflow-y: scroll;">
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-danger" onclick="close_agentwise();">
                                    Close</button>
                            </div>
                        </div>
                        <!-- /.modal-content -->
                    </div>
                    <!-- /.modal-dialog -->
                </div>
                <div class="modal fade in" id="div_routewisemainCompare" style="display: none; padding-right: 17px;width: 110%;">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true" onclick="close_routewiseCompare();">×</span></button>
                                <h4 class="modal-title">
                                    Product Wise Details</h4>
                            </div>
                            <div class="modal-body" id="div_routewiseCompare" style="height: 400px; overflow-y: scroll;">
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-danger" onclick="close_routewiseCompare();">
                                    Close</button>
                            </div>
                        </div>
                        <!-- /.modal-content -->
                    </div>
                    <!-- /.modal-dialog -->
                </div>
                <div class="modal fade in" id="example" style="display: none; padding-right: 17px;width: 110%;">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true" onclick="close_AgentProductLineChart();">×</span></button>
                                <h4 class="modal-title">
                                    Day Wise Product Details</h4>
                            </div>
                            <div class="modal-body" id="ProductWiseChart" style="height: 400px; overflow-y: scroll;">
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-danger" onclick="close_AgentProductLineChart();">
                                    Close</button>
                            </div>
                            
                        </div>
                        <!-- /.modal-content -->
                    </div>
                    <!-- /.modal-dialog -->
                </div>
        <div class="modal fade in" id="divMainAddNewRow" style="display: none; padding-right: 17px;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true" onclick="PopupCloseClick();">×</span></button>
                        <h4 class="modal-title">
                            Route Wise Details</h4>
                    </div>
                    <div class="modal-body" id="divRouteDetails" style="height: 400px; overflow-y: scroll;">
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" onclick="PopupCloseClick();">
                            Close</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        </div>
    </div>
  </div>
 </section>
</asp:Content>

