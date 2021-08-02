function checkbrnch_gridload() {

    var brnchmngt_gridload_oncheckchange = "";
    var checkinputs = $('#divcategiriesdata').find('.checkinput');
    checkinputs.each(function (list) {
        var checkbox = checkinputs[list];
        if (checkbox.checked) {
            brnchmngt_gridload_oncheckchange += checkbox.value;
        }
    });
    if (brnchmngt_gridload_oncheckchange.length > 0) {
        checkedbranch = brnchmngt_gridload_oncheckchange;
        //        if (lvltype == "Admin") {
        var data = { 'operation': 'Update_grid_oncheck_change_branchmanagement', 'checkedbranch': checkedbranch };
        var s = function (msg) {
            if (msg) {
                BindingBranchManagement(msg);
            }
            else {
            }
        };
        var e = function (x, h, e) {
        };
        $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
        //        brnchplantname = document.getElementById('cmb_brnch_plantname').value;
        //        }
    }
}
function brnchprdt_checkbrnch_gridload() {

    var brnchprdt_gridload_oncheckchange = "";
    var checkinputss = $('#divcategiriesdatas').find('.checkinput');
    checkinputss.each(function (list) {
        var checkboxs = checkinputss[list];
        if (checkboxs.checked) {
            brnchprdt_gridload_oncheckchange = checkboxs.value;
        }
    });
    if (brnchprdt_gridload_oncheckchange.length > 0) {
        checkedbranch = brnchprdt_gridload_oncheckchange;
        var data = { 'operation': 'updatebrnchprdt_check_togrid', 'checkedbranch': checkedbranch };
        var s = function (msg) {
            if (msg) {
                BindingBranchProducts(msg);
            }
            else {
            }
        };
        var e = function (x, h, e) {
        };
        $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }
}
//function branchmgntmapping() {
//    branches_manages_salestype();
//    var data = { 'operation': 'Initilize_branchmapping' };
//    var s = function (msg) {
//        if (msg) {
//            count = 0;
////            BindCategories(msg);
//        }
//        else {
//        }
//    };
//    var e = function (x, h, e) {
//    };
//    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//    callHandler(data, s, e);
//}
function FillSalesOffice() {
    var data = { 'operation': 'GetPlantSalesOffice' };
    var s = function (msg) {
        if (msg) {
            if (msg == "Session Expired") {
                alert(msg);
                window.location = "Login.aspx";
            }
            BindSalesOffice(msg);
//            bindproductsalesoffice(msg);
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
}
function BindSalesOffice(msg) {
    var ddlBranchName = document.getElementById('ddlBranchName');
    var length = ddlBranchName.options.length;
    ddlBranchName.options.length = null;
    var opt = document.createElement('option');
    opt.innerHTML = "select";
    ddlBranchName.appendChild(opt);
    for (var i = 0; i < msg.length; i++) {
        if (msg[i].BranchName != null) {
            var opt = document.createElement('option');
            opt.innerHTML = msg[i].BranchName;
            opt.value = msg[i].Sno;
            ddlBranchName.appendChild(opt);
        }
    }
}
//function bindproductsalesoffice(msg) {
//    var ddlProductsBranch = document.getElementById('ddlProductsBranch');
//    var length = ddlBranchName.options.length;
//    ddlProductsBranch.options.length = null;
//    var opt = document.createElement('option');
//    opt.innerHTML = "select";
//    ddlProductsBranch.appendChild(opt);
//    for (var i = 0; i < msg.length; i++) {
//        if (msg[i].BranchName != null) {
//            var opt = document.createElement('option');
//            opt.innerHTML = msg[i].BranchName;
//            opt.value = msg[i].Sno;
//            ddlProductsBranch.appendChild(opt);
//        }
//    }
//}

//function ddlBranchNameChange(ID) {
//    var BranchID = ID.value;
//    var data = { 'operation': 'get_BranchData', 'BranchID': BranchID };
//    var s = function (msg) {
//        if (msg) {
//            if (msg == "Session Expired") {
//                alert(msg);
//                window.location = "Login.aspx";
//            }
//            bindagentnames(msg);
//            searchagentdetais(msg)
//        }
//        else {
//        }
//    };
//    var e = function (x, h, e) {
//    };
//    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//    callHandler(data, s, e);
//}
//function bindagentnames(msg) {
//    document.getElementById('ddlAgentName').options.length = "";
//    var veh = document.getElementById('ddlAgentName');
//    var length = veh.options.length;
//    for (i = length - 1; i >= 0; i--) {
//        veh.options[i] = null;
//    }
//    var opt = document.createElement('option');
//    opt.innerHTML = "Select Agent Name";
//    opt.value = "";
//    veh.appendChild(opt);
//    for (var i = 0; i < msg.length; i++) {
//        if (msg[i] != null) {
//            var opt = document.createElement('option');
//            opt.innerHTML = msg[i].brncName;
//            opt.value = msg[i].sno;
//            veh.appendChild(opt);
//        }
//    }
//}


//var compiledList = [];
//function searchagentdetais(msg) {
//    //var compiledList = [];
//    for (var i = 0; i < msg.length; i++) {
//        var brncName = msg[i].brncName;
//        compiledList.push(brncName);
//    }
//    $('#txtAgentName').autocomplete({
//        source: compiledList,
//        //        change: changeagentname,
//        autoFocus: true
//    });
//}





//function ddlBranchProductNameChange(ID) {
//    var BranchID = ID.value;
//    var data = { 'operation': 'get_BranchData', 'BranchID': BranchID };
//    var s = function (msg) {
//        if (msg) {
//            if (msg == "Session Expired") {
//                alert(msg);
//                window.location = "Login.aspx";
//            }
//            bindproductagentnames(msg);
//            searchproductagentnames(msg)
//        }
//        else {
//        }
//    };
//    var e = function (x, h, e) {
//    };
//    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//    callHandler(data, s, e);
//}
//function bindproductagentnames(msg) {
//    document.getElementById('ddlAgentProducts').options.length = "";
//    var veh = document.getElementById('ddlAgentProducts');
//    var length = veh.options.length;
//    for (i = length - 1; i >= 0; i--) {
//        veh.options[i] = null;
//    }
//    var opt = document.createElement('option');
//    opt.innerHTML = "Select Agent Name";
//    opt.value = "";
//    veh.appendChild(opt);
//    for (var i = 0; i < msg.length; i++) {
//        if (msg[i] != null) {
//            var opt = document.createElement('option');
//            opt.innerHTML = msg[i].brncName;
//            opt.value = msg[i].sno;
//            veh.appendChild(opt);
//        }
//    }
//}


//var compiledList1 = [];
//function searchproductagentnames(msg) {
//    //var compiledList = [];
//    for (var i = 0; i < msg.length; i++) {
//        var brncName = msg[i].brncName;
//        compiledList1.push(brncName);
//    }
//    $('#txtAgentProducts').autocomplete({
//        source: compiledList1,
//        //        change: changeagentname,
//        autoFocus: true
//    });
//}






function branchprdtcategories() {
    branches_products_branchname();
    var data = { 'operation': 'Initilize_branchmapping' };
    var s = function (msg) {
        if (msg) {
            counts = 0;
            BindCategoriesss(msg);
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
}
function branchDocuments() {
    FillSalesOffice();

}


//function FillSalesOffice() {
//    var data = { 'operation': 'GetPlantSalesOffice' };
//    var s = function (msg) {
//        if (msg) {
//            if (msg == "Session Expired") {
//                alert(msg);
//                window.location = "Login.aspx";
//            }
//            BindSalesOffice(msg);
//        }
//        else {
//        }
//    };
//    var e = function (x, h, e) {
//    };
//    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//    callHandler(data, s, e);
//}
//function BindSalesOffice(msg) {
//    var ddlsalesOffice = document.getElementById('ddlSalesOffice');
//    var length = ddlsalesOffice.options.length;
//    ddlsalesOffice.options.length = null;
//    var opt = document.createElement('option');
//    opt.innerHTML = "select";
//    ddlsalesOffice.appendChild(opt);
//    for (var i = 0; i < msg.length; i++) {
//        if (msg[i].BranchName != null) {
//            var opt = document.createElement('option');
//            opt.innerHTML = msg[i].BranchName;
//            opt.value = msg[i].Sno;
//            ddlsalesOffice.appendChild(opt);
//        }
//    }
//}
//function ddlSalesOfficeChange(ID) {
//    var BranchID = ID.value;
//    var data = { 'operation': 'GetSalesRoutes', 'BranchID': BranchID };
//    var s = function (msg) {
//        if (msg) {
//            if (msg == "Session Expired") {
//                alert(msg);
//                window.location = "Login.aspx";
//            }
//            BindRouteName(msg);
//        }
//        else {
//        }
//    };
//    var e = function (x, h, e) {
//    };
//    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//    callHandler(data, s, e);
//}
//function BindRouteName(msg) {
//    document.getElementById('ddlRouteName').options.length = "";
//    var veh = document.getElementById('ddlRouteName');
//    var length = veh.options.length;
//    for (i = length - 1; i >= 0; i--) {
//        veh.options[i] = null;
//    }
//    var opt = document.createElement('option');
//    opt.innerHTML = "Select Route Name";
//    opt.value = "";
//    veh.appendChild(opt);
//    for (var i = 0; i < msg.length; i++) {
//        if (msg[i] != null) {
//            var opt = document.createElement('option');
//            opt.innerHTML = msg[i].RouteName;
//            opt.value = msg[i].rid;
//            veh.appendChild(opt);
//        }
//    }
//}
//function ddlRouteNameChange(id) {
//    FillAgentName(id.value);
//}

//function FillAgentName(RouteID) {
//    var data = { 'operation': 'GetAgents', 'RouteID': RouteID };
//    var s = function (msg) {
//        if (msg) {
//            BindAgentName(msg);
//        }
//        else {
//        }
//    };
//    var e = function (x, h, e) {
//    };
//    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
//    callHandler(data, s, e);
//}
//function BindAgentName(msg) {
//    document.getElementById('ddlAgentName').options.length = "";
//    var ddlAgentName = document.getElementById('ddlAgentName');
//    var length = ddlAgentName.options.length;
//    for (i = length - 1; i >= 0; i--) {
//        ddlAgentName.options[i] = null;
//    }
//    var opt = document.createElement('option');
//    opt.innerHTML = "Select Agent Name";
//    opt.value = "";
//    ddlAgentName.appendChild(opt);
//    for (var i = 0; i < msg.length; i++) {
//        if (msg[i] != null) {
//            var opt = document.createElement('option');
//            opt.innerHTML = msg[i].BranchName;
//            opt.value = msg[i].Sno;
//            ddlAgentName.appendChild(opt);
//        }
//    }
//}

//function getFile() {
//    document.getElementById("file").click();
//}
//function hasExtension(fileName, exts) {
//    return (new RegExp('(' + exts.join('|').replace(/\./g, '\\.') + ')$')).test(fileName);
//}
//function readURL(input) {
//    if (input.files && input.files[0]) {

//        var reader = new FileReader();
//        reader.readAsDataURL(input.files[0]);
//        document.getElementById("yourBtn").innerHTML = '<img src="Images/barloader.GIF" style="height:25px;"/>';

//        reader.onload = function (e) {
//            if (hasExtension(input.files[0].name, ['.jpg', '.gif', '.png', '.jpeg', '.bmp', '.BMP', '.JPEG', '.GIF', '.JPG', '.PNG'])) {
//                $('#imgemp').attr('src', e.target.result).width(200).height(200);
//                var source_obj = document.getElementById('imgemp');
//                var output = jic.compress(source_obj, 30, "jpg").src;
//                $('.prw_img').attr('src', output).width(200).height(200);
//                document.getElementById("yourBtn").innerHTML = input.files[0].name;
//                document.getElementById('btn_upload_profilepic').disabled = false;
//                $('#btn_upload_profilepic').css('display', 'block');

//            }
//            else {
//                alert("Unsupported format \n Supported formats are .jpg .jpeg .png .gif .bmp");
//                document.getElementById("yourBtn").innerHTML = 'Click to Choose Image';
//                return;
//            }
//        };

//    }
//}
//function upload_profile_pic() {
//    var dataURL = document.getElementById('main_img').src;
//    var div_text = $('#yourBtn').text().trim();
//    if (div_text == "Choose Image") {
//        alert('Please Choose an Image to Upload');
//        return;
//    }
//    var ddlAgentName = document.getElementById('ddlAgentName').value;
//    if (ddlAgentName == "" || ddlAgentName == "select") {
//        alert("Please Select Agent Name");
//        $("#ddlAgentName").focus();
//        return false;
//    }

//    var fileUpload = $("#file").get(0);
//    var files = fileUpload.files;

//    var test = new FormData();
//    test.append("operation", "Agent_profile_pic_files_upload");
//    test.append("BranchID", ddlAgentName);
//   // test.append("id", 1);
//    for (var i = 0; i < files.length; i++) {
//        test.append(files[i].name, files[i]);
//    }

////    var blob = dataURItoBlob(dataURL);
////    var test1 = new FormData();
////    test1.append("operation", "Agent_profile_pic_files_upload");
////    test1.append("BranchID", ddlAgentName);
////    test1.append("imagecode", blob);
//    var s = function (msg) {
//        if (msg) {
//            alert(msg);
//            $('#btn_upload_profilepic').css('display', 'none');

//        }
//        else {
//            alert(msg);
//        }
//    };
//    var e = function (x, h, e) {
//        //alert(e.toString());
//    };

//    callHandler_nojson_post(test, s, e);
//}
//function upload_profile_picssss() {
//    var dataURL = document.getElementById('main_img').src;

//    var div_text = $('#yourBtn').text().trim();
//    if (div_text == "Choose Image") {
//        alert('Please Choose an Image to Upload');
//        return;
//    }
//    var ddlAgentName = document.getElementById('ddlAgentName').value;
//    if (ddlAgentName == "" || ddlAgentName == "select") {
//        alert("Please Select Agent Name");
//        $("#ddlAgentName").focus();
//        return false;
//    }
//   
//    //var blob = dataURItoBlob(dataURL);
//   // var Data = { 'operation': 'Agent_profile_pic_files_upload', 'BranchID': ddlAgentName, 'imagecode': blob };
//    var blob = dataURItoBlob(dataURL);
//    var test = new FormData();
//    test.append("operation", "Agent_profile_pic_files_upload");
//    test.append("BranchID", ddlAgentName);
//    //test.append("imagecode", blob);
//    var s = function (msg) {
//        if (msg) {
//            alert(msg);
//            $('#btn_upload_profilepic').css('display', 'none');

//        }
//        else {
//            alert(msg);
//        }
//    };
//    var e = function (x, h, e) {
//        //alert(e.toString());
//    };

//    callHandler_post(test, s, e);
//}
//function dataURItoBlob(dataURI) {
//    // convert base64/URLEncoded data component to raw binary data held in a string
//    var byteString;
//    if (dataURI.split(',')[0].indexOf('base64') >= 0)
//        byteString = atob(dataURI.split(',')[1]);
//    else
//        byteString = unescape(dataURI.split(',')[1]);

//    // separate out the mime component
//    var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];

//    // write the bytes of the string to a typed array
//    var ia = new Uint8Array(byteString.length);
//    for (var i = 0; i < byteString.length; i++) {
//        ia[i] = byteString.charCodeAt(i);
//    }

//    return new Blob([ia], { type: 'image/jpeg' });
//}



function BindCategoriesss(data) {
    //$("#divcategiriesdata").unload();
   
    $("#divcategiriesdata").html("");
 var prnt = $("#divcategiriesdatas");
    for (var i = 0; i < data.length; i++) {
        if (data[i].brncName != null) {

            filltrees(data[i].sno, data[i].brncName, data[i].lstbrnch, prnt);
        }
    }
}
var counts = 0;
function filltrees(sno, branchname, lstbrnch, liparent) {
    var ZZZ = branchname;
    var branchnm = ZZZ.split(" ").join("_");
    liparent.append("<div id='div" + branchnm + "' class='divcategory'>");
    if (counts == 0) {
        $("#div" + branchnm + "").append("<div class='titledivcls'><table style='width:100%;'><tr style='width:100%;'><td style='width:1%;'><input autocomplete='off' class='checkinput' type='checkbox'  value=" + sno + " onclick='Ravi(this);'/></td><td><h2 class='unitline'>" + branchnm + "</h2></td><td style='padding-right: 20px;vertical-align: middle;'><span class='iconminus' title='Hide' onclick='minusclick(this);'></span></td></tr></table></div>");
    }
    counts++;
    $("#div" + branchnm + "").append("<ul id='ul" + branchnm + "' class='ulclass'>");
    for (var j = 0; j < lstbrnch.length; j++) {
        if (lstbrnch[j].lstbrnch.length > 0) {
            //                       $("#div" + branchnm + "").append("<table style='width:100%;'><tr><td></td><td style='padding-right: 20px;vertical-align: middle;'><span class='iconminus' title='Hide' onclick='minusclick(this);'></span></td></tr></table></div>");
            $("#ul" + branchnm + "").append("<div class='uldiv'><table style='width:100%;'><tr><td><li><a class='activeanchor'><input autocomplete='off' class='checkinput' type='checkbox' value=" + lstbrnch[j].sno + " onclick='Ravi(this);' /><span class='livalue'>" + lstbrnch[j].brncName + "</span></a></li></td><td style='padding-right: 20px;vertical-align: middle;'><span class='iconminusli' title='Hide' onclick='liminusclick(this);'></span></td></tr></table></div>");
        }
        else {
            $("#ul" + branchnm + "").append("<li><a class='activeanchor'><input autocomplete='off' class='checkinput' type='checkbox' value=" + lstbrnch[j].sno + " onclick='Ravi(this);'/><span class='livalue'>" + lstbrnch[j].brncName + "</span></a></li>");
        }
        var prnts = $("#ul" + branchnm + "");
        filltrees(lstbrnch[j].sno, lstbrnch[j].brncName, lstbrnch[j].lstbrnch, prnts);
    }
}
function Ravi(thisid) {
    
    $('.checkinput').each(function (i, obj) {
        if ($(this).val() == thisid.value) {
            $(this).attr('checked', 'checked');
        }
        else {
            $(this).removeAttr('checked')
        }
    });
     checkbrnch_gridload();
    brnchprdt_checkbrnch_gridload();
}
function checked(thisid) {

    $('.checkinput').each(function (i, obj) {
        if ($(this).val() == thisid.value) {
            $(this).attr('checked', 'checked');
        }
        else {
            $(this).removeAttr('checked')
        }
    });
    brnchprdt_checkbrnch_gridload();
}

//check box filling 4/6/2017 begin
//function BindCategories(data) {
//    //$("#divcategiriesdatas").html("");
//    $("#divcategiriesdata").empty();
//    var prnt = $("#divcategiriesdata");
//    for (var i = 0; i < data.length; i++) {
//        if (data[i].brncName != null) {
//            filltree(data[i].sno, data[i].brncName, data[i].lstbrnch, prnt);
//        }
//    }
//}


//var count = 0;
//function filltree(sno, branchname, lstbrnch, liparent) {
//    var ZZZ = branchname;
//    var branchnm = ZZZ.split(" ").join("_");
//    liparent.append("<div id='div" + branchnm + "' class='divcategory'>");
//    if (count == 0) {
//        $("#div" + branchnm + "").append("<div class='titledivcls'><table style='width:100%;'><tr style='width:100%;'><td style='width:1%;'><input autocomplete='off' class='checkinput' type='checkbox'  value=" + sno + " onclick='Ravi(this);' /></td><td><h2 class='unitline'>" + branchnm + "</h2></td><td style='padding-right: 20px;vertical-align: middle;'><span class='iconminus' title='Hide' onclick='minusclick(this);'></span></td></tr></table></div>");
//    }
//    count++;
//    $("#div" + branchnm + "").append("<ul id='ul" + branchnm + "' class='ulclass'>");
//    for (var j = 0; j < lstbrnch.length; j++) {
//        if (lstbrnch[j].lstbrnch.length > 0) {
//            //                       $("#div" + branchnm + "").append("<table style='width:100%;'><tr><td></td><td style='padding-right: 20px;vertical-align: middle;'><span class='iconminus' title='Hide' onclick='minusclick(this);'></span></td></tr></table></div>");
//            $("#ul" + branchnm + "").append("<div class='uldiv'><table style='width:100%;'><tr><td><li><a class='activeanchor'><input autocomplete='off' class='checkinput' type='checkbox' value=" + lstbrnch[j].sno + " onclick='Ravi(this);' /><span class='livalue'>" + lstbrnch[j].brncName + "</span></a></li></td><td style='padding-right: 20px;vertical-align: middle;'><span class='iconminusli' title='Hide' onclick='liminusclick(this);'></span></td></tr></table></div>");
//        }
//        else {
//            $("#ul" + branchnm + "").append("<li><a class='activeanchor'><input autocomplete='off' class='checkinput' type='checkbox' value=" + lstbrnch[j].sno + " onclick='Ravi(this);' /><span class='livalue'>" + lstbrnch[j].brncName + "</span></a></li>");
//        }
//        var prnts = $("#ul" + branchnm + "");
//        filltree(lstbrnch[j].sno, lstbrnch[j].brncName, lstbrnch[j].lstbrnch, prnts);
//    }
//}
//end
function minusclick(thisid) {
    var prntdiv = $(thisid).parents(".titledivcls");
    ul = prntdiv.next("ul");
    if (thisid.title == "Hide") {
        ul.slideUp("slow");
        $(thisid).attr('title', "Show");
        $(thisid).css("background", "url('Images/plus.png') no-repeat");
    }
    else {
        ul.slideDown("slow");
        $(thisid).attr('title', "Hide");
        $(thisid).css("background", "url('Images/minus.png') no-repeat");
    }
}

function liminusclick(thisid) {
    var prntdiv = $(thisid).closest("div").next("div");
    if (thisid.title == "Hide") {
        // prntdiv.slideUp("slow");
        prntdiv.toggle();

        $(thisid).attr('title', "Show");
        $(thisid).css("background", "url('Images/plus.png') no-repeat");
    }
    else {
        prntdiv.toggle();
        $(thisid).attr('title', "Hide");
        $(thisid).css("background", "url('Images/minus.png') no-repeat");
    }
}
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
//$ bower install alertify;
                                                    //VALIDATIONS
//save click validation
function salestypevalidation() {
    var x = document.getElementById("txt_salestype").value;
    if (x == "" ) {
       alert("Please Provide SalesType");
        $("#txt_salestype").focus();
        return false;
    }
    var y = document.getElementById("cmb_flag").value;
    if (y == "") {
        alert("Please Select Flag");
        $("#cmb_flag").focus();
        return false;
    }
   else
   {
       salestypemanage();
   }
}
//product management validations
function prdtmngt_catgryValidation() {
    var x = document.getElementById("txt_productname").value;
    if (x == "") {
        alert("Please Provide CategoryName");
        $("#txt_productname").focus();
        return false;
    }
    var y = document.getElementById("cmb_prdmngflag").value;
    if (y == "") {
        alert("Please Select Flag");
        $("#cmb_prdmngflag").focus();
        return false;
    }
    else {
        producttypemanagement();
    }
}
function prdtmngt_subcatgryvalidation() {
    var x = document.getElementById("txt_catgry_subname").value;
    if (x == "") {
        alert("Please Provide SubCategoryName");
        $("#txt_catgry_subname").focus();
        return false;
    }
    var y = document.getElementById("cmb_catgry_catgryname").value;
    if (y == "" || y=="Select") {
        alert("Please Select CategoryName");
        $("#cmb_catgry_catgryname").focus();
        return false;
    }
    var z = document.getElementById("cmb_catgry_flag").value;
    if (z == "") {
        alert("Please Select Flag");
        $("#cmb_catgry_flag").focus();
        return false;
    }
    else {
        subcategorymanagement();
    }
}
function prdtmgnt_productsvalidation() {
    var x = document.getElementById("cmb_products_category").value;
    if (x == ""||x=="Select") {
        alert("Please Select CategoryName");
        $("#cmb_products_category").focus();
        return false;
    }
    var y = document.getElementById("cmb_products_subcatgry").value;
    if (y == "" || y == "Select") {
        alert("Please Select SubCategoryName");
        $("#cmb_products_subcatgry").focus();
        return false;
    }
    var z = document.getElementById("txt_products_prdtsname").value;
    if (z == "") {
        alert("Please Provide ProductName");
        $("#txt_products_prdtsname").focus();
        return false;
    }
    
    var s = document.getElementById("txt_products_qty").value;
    if (s == "") {
        alert("Please Provide Quantity");
        $("#txt_products_qty").focus();
        return false;
    }
    var a = document.getElementById("cmb_products_qtymeasurement").value;
    if (a == "") {
        alert("Please Select Measurement");
        $("#cmb_products_qtymeasurement").focus();
        return false;
    }
    var a = document.getElementById("txt_products_unitprice").value;
    if (a == "") {
        alert("Please Provide UnitPrice");
        $("#txt_products_unitprice").focus();
        return false;
    }
    var n = document.getElementById("cmb_products_flag").value;
    if (n == "") {
        alert("Please Select Flag");
        $("#cmb_products_flag").focus();
        return false;
    }
    var cmb_product_inv = document.getElementById("cmb_product_inv").value;
    if (cmb_product_inv == "" || cmb_product_inv == "Select Inventory") {
        alert("Please Select Inventory");
        return false;
    }
    else {
        productunitsmanagement();
    }
}
//End product Management validations

//Branches Management validations
function brnchmgnt_validation() {

    var x = document.getElementById("txt_branchname").value;
    if (x == "") {
        alert("Please Provide BranchName");
        $("#txt_branchname").focus();
        return false;
    }
    var y = document.getElementById("cmb_salestype").value;
    if (y == "" || y == "select") {
        alert("Please Select SalesType");
        $("#cmb_salestype").focus();
        return false;
    }
    var z = document.getElementById("cmb_branchesManageflag").value;
    if (z == "") {
        alert("Please Select Flag");
        $("#cmb_branchesManageflag").focus();
        return false;
    }
    var cmbcollectiontype = document.getElementById('cmb_collectiontype').value;
    if (cmbcollectiontype == "" || cmbcollectiontype == "select") {
        alert("Please Select CollectionType");
        $("#cmb_collectiontype").focus();
        return false;
    }
    else {
        branchesmanagement();
    }
}
//End Branches Management validations

//Branch Products Validations
function brnchprdts_validations() {
//    var x = document.getElementById("cmb_branchname").value;
//    if (x == "" || x=="select") {
//        alert("Please Select BranchName");
//        $("#cmb_branchname").focus();
//        return false;
//    }
    var y = document.getElementById("cmb_brchprdt_Catgry_name").value;
    if (y == ""  ||y=="select") {
        alert("Please Select CategoryName");
        $("#cmb_brchprdt_Catgry_name").focus();
        return false;
    }
    var z = document.getElementById("cmb__brnch_subcatgry").value;
    if (z == ""||z=="select") {
        alert("Please Select SubCategoryName");
        $("#cmb__brnch_subcatgry").focus();
        return false;
    }
    var p = document.getElementById("cmb_productname").value;
    if (p == ""|| p=="select") {
        alert("Please Select ProductName");
        $("#cmb_productname").focus();
        return false;
    }
    var u = document.getElementById("txt_productunitprice").value;
    if (u == "") {
        alert("Please Provide UnitPrice");
        $("#txt_productunitprice").focus();
        return false;
    }
    var f = document.getElementById("cmb_branchesproductsflag").value;
    if (f == "") {
        alert("Please Select Flag");
        $("#cmb_branchesproductsflag").focus();
        return false;
    }
    else {
        branchproducts();
    }
}

//End Branch products validations


//End save click validation

                                                        //VALIDATIONS END

function product_manages_subcatgry() {
    updatesubcategorytype();
 var data = { 'operation': 'intialize_productsmanagement_subcatgry'};
    var s = function (msg) {
        if (msg) {
            fillproduct_manage_subcatgry(msg);
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
};
function getproductinv() {
    var data = { 'operation': 'intialize_Prdt_inventory' };
    var s = function (msg) {
        if (msg) {
            fill_prdt_inv(msg);
        }
        else {
        }
    };
    var e = function (x, h, e) {
       
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
}
function fill_prdt_inv(msg) {
    var productinv = document.getElementById('cmb_product_inv');
    var length = productinv.options.length;
    document.getElementById("cmb_product_inv").options.length = null;
    //    for (i = 0; i < length; i++) {
    //        productcategory.options[i] = null;
    //    }
    var opt = document.createElement('option');
    opt.innerHTML = "Select Inventory";
    productinv.appendChild(opt);
    for (var i = 0; i < msg.length; i++) {
        if (msg[i].categoryname != null) {
            var opt = document.createElement('option');
            opt.innerHTML = msg[i].categoryname;
            opt.value = msg[i].sno;
            productinv.appendChild(opt);
        }
    } 
}
function product_manages_products() {
    Bindproductunits();
    getproductinv();
 var data = { 'operation': 'intialize_productsmanagement_products'};
    var s = function (msg) {
        if (msg) {
            fillproduct_manage_products(msg);
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
};
var lvltype = "";
function branches_manages_salestype() {

    if (lvltype == "Admin") {
//        $('#lblplantname').css('display', 'block');
//        $('#cmb_brnch_plantname').css('display', 'block');
    }
    else {
        $('#lblplantname').css('display', 'none');
        $('#cmb_brnch_plantname').css('display', 'none');
    }
    
   Bindbranchmanagement();

    var data = { 'operation': 'intialize_branchesmanages_salestype' };
    var s = function (msg) {
        if (msg) {
            fillbranches_manage_salestype(msg);
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
};
function branches_products_branchname() {
   // BindBranchProducts();
    var data = { 'operation': 'intialize_branchesproducts_branchname' };
    var s = function (msg) {
        if (msg) {
            fillbranches_products_branchname(msg);
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
};


                                                           //FILL DROPDOWN ONCHANGE

function products_cateegoryname_onchange() {
    var cmbcatgryname = document.getElementById("cmb_products_category").value;
    var buttonval = document.getElementById("btn_products_save").value;
    var data = { 'operation': 'get_subcategory_data', 'cmbcatgryname': cmbcatgryname };
    var s = function (msg) {
        if (msg) {
            fillproducts_subcatgry(msg);
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    
    //callHandler(data, s, e);
    AscCallHandler(data, s, e);
};
function brnches_plantname_onchange(names) {
    if (lvltype == "Admin") {
        var cmbplantname = names.value;
        if (cmbplantname = "undefined") {
            cmbplantname = document.getElementById("cmb_brnch_plantname").value;
        }
        var data = { 'operation': 'get_salesoffice_data', 'cmbplantname': cmbplantname };
    }
    else {
        var data = { 'operation': 'get_salesoffice_data'};
    }
    var s = function (msg) {
        if (msg) {
            fillbrnches_mgnt_salesoffice(msg);
        } 
        else {
        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    AscCallHandler(data, s, e);
};
function productsdata_categoryname_onchange() {
    var cmbcatgryname = document.getElementById("cmb_brchprdt_Catgry_name").value;
    var data = { 'operation': 'get_product_subcategory_data', 'cmbcatgryname': cmbcatgryname };
    var s = function (msg) {
        if (msg) {
            fillproductsdata_subcatgry(msg);
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    //callHandler(data, s, e);
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    AscCallHandler(data, s, e);
};
function productsdata_subcategory_onchange() {
    var cmbsubcatgryname = document.getElementById("cmb__brnch_subcatgry").value;
//    var e = document.getElementById("cmb__brnch_subcatgry");
//    var cmbsubcatgryname = e.options[e.selectedIndex].value;
    var data = { 'operation': 'get_products_data', 'cmbsubcatgryname': cmbsubcatgryname};
    var s = function (msg) {
        if (msg) {
            fillproductsdata_products(msg);
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    //callHandler(data, s, e);
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    AscCallHandler(data, s, e);
};

 



                                                        //END FILL DROPDOWN ONCHANGE



function fillalldata(msg) {


}
                                                      //START ONCHANGE FUNCTIONS

function fillproduct_manage_subcatgry(msg) {
 var categryname = document.getElementById('cmb_catgry_catgryname');
 var length = categryname.options.length;
 document.getElementById("cmb_catgry_catgryname").options.length = null;
//    for (i = 0; i < length; i++) {
//        categryname.options[i] = null;
 //    }
 var opt = document.createElement('option');
 opt.innerHTML = "Select";
 categryname.appendChild(opt);
    for (var i = 0; i < msg.length; i++) {
        if (msg[i].categoryname != null && msg[i].subproduct == null) {
            var opt = document.createElement('option');
            
            opt.innerHTML = msg[i].categoryname;
            opt.value = msg[i].sno;
            categryname.appendChild(opt);
        }
    }
}
function fillproduct_manage_products(msg)
{
 var productcategory = document.getElementById('cmb_products_category');
    var length = productcategory.options.length;
    document.getElementById("cmb_products_category").options.length = null;
//    for (i = 0; i < length; i++) {
//        productcategory.options[i] = null;
    //    }
    var opt = document.createElement('option');
    opt.innerHTML = "Select";
    productcategory.appendChild(opt);
    for (var i = 0; i < msg.length; i++) {
        if (msg[i].categoryname != null) {
            var opt = document.createElement('option');
            opt.innerHTML = msg[i].categoryname;
            opt.value = msg[i].sno;
            productcategory.appendChild(opt);
        }
    } 
    }
    function fillproducts_subcatgry(msg) {
    var prdtsubcategory = document.getElementById('cmb_products_subcatgry');
    var length = prdtsubcategory.options.length;
    document.getElementById("cmb_products_subcatgry").options.length = null;
//    for (i = 0; i < length; i++) {
//        prdtsubcategory.options[i] = null;
    //    }
    var opt = document.createElement('option');
    opt.innerHTML = "Select";
    prdtsubcategory.appendChild(opt);
    for (var i = 0; i < msg.length; i++) {
        if (msg[i].subcategorynames != null) {
            var opt = document.createElement('option');
            opt.innerHTML = msg[i].subcategorynames;
            opt.value = msg[i].sno;
            prdtsubcategory.appendChild(opt);
        }
    }
}
function fillbrnches_mgnt_salesoffice(msg) {
    var cmbbrnch_salesoffice = document.getElementById('cmb_brnch_salesoffice');
    var length = cmbbrnch_salesoffice.options.length;
    cmbbrnch_salesoffice.options.length = null;
    var opt = document.createElement('option');
    opt.innerHTML = "select";
    cmbbrnch_salesoffice.appendChild(opt);
    for (var i = 0; i < msg.length; i++) {
        if (msg[i].SubregionName != null) {
            var opt = document.createElement('option');
            opt.innerHTML = msg[i].SubregionName;
            opt.value = msg[i].sno;
            cmbbrnch_salesoffice.appendChild(opt);
        }
    }
}
function fillproductsdata_subcatgry(msg) {
    var brnchsubcategory = document.getElementById('cmb__brnch_subcatgry');
    var length = brnchsubcategory.options.length;
    document.getElementById("cmb__brnch_subcatgry").options.length = null;
   document.getElementById("cmb__brnch_subcatgry").value= "select";
var opt = document.createElement('option');
        opt.innerHTML = "Select";
        brnchsubcategory.appendChild(opt);
    for (var i = 0; i < msg.length; i++) {
        if (msg[i].subcategorynames != null) {
            var opt = document.createElement('option');
            opt.innerHTML = msg[i].subcategorynames;
            opt.value = msg[i].sno;
            brnchsubcategory.appendChild(opt);
        }
    }
}
function fillproductsdata_products(msg) {
    var cmbprdtname = document.getElementById('cmb_productname');
    var length = cmbprdtname.options.length;
    document.getElementById("cmb_productname").options.length = null;
    document.getElementById("cmb_productname").value = "select";
    var opt = document.createElement('option');
    opt.innerHTML = "Select";
    cmbprdtname.appendChild(opt);
    for (var i = 0; i < msg.length; i++) {
        if (msg[i].ProductName != null) {
            var opt = document.createElement('option');
            opt.innerHTML = msg[i].ProductName;
            opt.value = msg[i].sno;
            cmbprdtname.appendChild(opt);
        }
    }
}
function fillbranches_manage_salestype(msg) {
    var branchtype = document.getElementById('cmb_salestype');
    var length = branchtype.options.length;
    branchtype.options.length = null;
    var opt = document.createElement('option');
    opt.innerHTML = "select";
    branchtype.appendChild(opt);
    for (var i = 0; i < msg.length; i++) {
        if (msg[i].salestype != null) {
            var opt = document.createElement('option');
            opt.innerHTML = msg[i].salestype;
            opt.value = msg[i].sno;
            branchtype.appendChild(opt);
        }
    }
    if (lvltype == "Admin") {
    }
    else {
        var names = "";
        //brnches_plantname_onchange(names);
    }
}
function fillbranches_products_branchname(msg) {
    var brnchprdtcatgryname = document.getElementById('cmb_brchprdt_Catgry_name');
    var length = brnchprdtcatgryname.options.length;
    brnchprdtcatgryname.options.length = null;
    var opt = document.createElement('option');
    opt.innerHTML = "select";
    brnchprdtcatgryname.appendChild(opt);
    for (var i = 0; i < msg.length; i++) {
        if (msg[i].categoryname != null) {
            var opt = document.createElement('option');
            opt.innerHTML = msg[i].categoryname;
            opt.value = msg[i].sno;
            brnchprdtcatgryname.appendChild(opt);
        }
    }
    var subcatgry = document.getElementById('cmb__brnch_subcatgry');
    var length = subcatgry.options.length;
    subcatgry.options.length = null;
    var opt = document.createElement('option');
    opt.innerHTML = "select";
    subcatgry.appendChild(opt);
    for (var i = 0; i < msg.length; i++) {
        if (msg[i].subcategorynam != null) {
            var opt = document.createElement('option');
            opt.innerHTML = msg[i].subcategorynam;
            opt.value = msg[i].sno;
            subcatgry.appendChild(opt);
        }
    }
    var cmbprdtname = document.getElementById('cmb_productname');
    var length = cmbprdtname.options.length;
    cmbprdtname.options.length = null;
    var opt = document.createElement('option');
    opt.innerHTML = "select";
    cmbprdtname.appendChild(opt);
    for (var i = 0; i < msg.length; i++) {
        if (msg[i].ProductName != null && msg[i].cmb_productname == null) {
            var opt = document.createElement('option');
            opt.innerHTML = msg[i].ProductName;
            opt.value = msg[i].sno;
            cmbprdtname.appendChild(opt);
        }
    }
}
function salestypemanage() {
    var salestype = document.getElementById('txt_salestype').value;
    var salestypeflag = document.getElementById('cmb_flag').value;
    var operationtype = document.getElementById('btn_save').value;
    var sno=serial;
    var data = { 'operation': 'salestypemanage', 'sno':sno,'salestype': salestype, 'salestypeflag': salestypeflag, 'operationtype': operationtype };
    var s = function (msg) {
        if (msg) {
          alert(msg);
            salestypeclear();
            updatesalestype();
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
}
function salestypeclear() {
    document.getElementById('txt_salestype').value = "";
    document.getElementById('cmb_flag').value = "";
    document.getElementById('btn_save').value = "SAVE";
}
function updatesalestype() {
    var data = { 'operation': 'updatesalestypemanage' };
    var s = function (msg) {
        if (msg) {
            BindGridSales(msg);
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
}
var serial = 0;
function BindGridSales(databind) {
    $("#grd_brchtypemangement").jqGrid("clearGridData");
    var newarray = [];
    var Headarray = [];
    var headdatacol = databind[1];
    var datacol = databind;
    for (var Booking in databind) {
        var status = 'InActive';
        if (databind[Booking].flag == '1') {
            status = 'Active';
        }
        newarray.push({ 'Sno': newarray.length + 1, 'SalesType': databind[Booking].salestype, 'Flag': status, 'r_sno': databind[Booking].sno });
    }
    $("#grd_brchtypemangement").jqGrid({
        datatype: "local",
        height: 'auto',
        autowidth: true,
        colNames: Headarray,
        colModel: [{ name: 'Sno', index: 'note', width: 100, sortable: false, align: 'center', search: true, searchtype: "number" },
        { name: 'SalesType', index: 'note', width: 100, sortable: false, align: 'center', search: true },
{ name: 'Flag', index: 'note', width: 100, sortable: false, align: 'center' },
{ name: 'r_sno', index: 'note', width: 100, sortable: false, align: 'center', hidden: true}],
        rowNum: 10,
        rowList: [5, 10, 30],
        // rownumbers: true,height: 'auto', 
        gridview: true,
        search: false,
        loadonce: true,
        pager: "#page",
        Find: "Find",
        //viewrecords: true,
        caption: "Sales Type Manage"
    }).jqGrid('navGrid', '#page', { edit: false, add: false, del: false, refresh: false });
    var mydata = newarray;
    for (var i = 0; i <= mydata.length; i++) {
        jQuery("#grd_brchtypemangement").jqGrid('addRowData', i + 1, mydata[i]);
    }
    //jQuery("#grd_brchtypemangement").jqGrid('searchGrid', { Find: true });
    // jQuery("#mysearch").jqGrid('filterGrid', '#grd_brchtypemangement', options);
    $("#grd_brchtypemangement").jqGrid('setGridParam', { onSelectRow: function (rowid, iRow, iCol, e) {
        var salestype = $('#grd_brchtypemangement').getCell(rowid, 'SalesType');
        var flag = $('#grd_brchtypemangement').getCell(rowid, 'Flag');
        document.getElementById('txt_salestype').value = salestype;
        document.getElementById('cmb_flag').value = flag;
        document.getElementById('btn_save').value = "MODIFY";
        serial = $('#grd_brchtypemangement').getCell(rowid, 'r_sno');
    }
    });
    $("#grd_brchtypemangement").find('input[type=button]').hide();
}
var serial1 = 0;
function producttypemanagement() {
    var productname = document.getElementById('txt_productname').value;
    var producttypeflag = document.getElementById('cmb_prdmngflag').value;
    var tcategoryname = document.getElementById('txttcategoryName').value;
    var operationtype = document.getElementById('btn_productmanagesave').value; s
    tcategoryname
    var sno = serial1;
    var data = { 'operation': 'producttypemanagement', 'sno': sno, 'productname': productname, 'tcategoryname': tcategoryname, 'producttypeflag': producttypeflag, 'operationtype': operationtype };
    var s = function (msg) {
        if (msg) {
            alert(msg);
            producttypeclear();
            updatecategorytype();
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
}
function producttypeclear() {
    document.getElementById('txt_productname').value = "";
    document.getElementById('cmb_prdmngflag').value = "";
    document.getElementById('btn_productmanagesave').value = "SAVE";
}
function updatecategorytype() {
    var target_tab_selector = $('#divsubcategory').attr('li');
    var target_tab_selector1 = $('#divcategory_products').attr('li');
    var target_tab_selector2 = $('#divproductsManage').attr('li');
    $(target_tab_selector).addClass('hide');
    $(target_tab_selector1).addClass('hide');
    $(target_tab_selector2).addClass('ui-tabs-active');
    var data = { 'operation': 'Updateproducttypemanage' };
    var s = function (msg) {
        if (msg) {
            updateproducttypemanagement(msg);
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
}
function getcatgeory(thisid) {
    var Categoryname = $(thisid).parent().parent().children('.1').html();
    var status = $(thisid).parent().parent().children('.2').html();
    var sno = $(thisid).parent().parent().children('.3').html();
    var tcategory = $(thisid).parent().parent().children('.4').html();
    document.getElementById('txt_productname').value = Categoryname;
    document.getElementById('cmb_prdmngflag').value = status;
    document.getElementById('txttcategoryName').value = tcategory;
    document.getElementById('btn_productmanagesave').value = "MODIFY";
    serial1 = sno;
}
function updateproducttypemanagement(msg) {
    var l = 0;
    var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
    var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
    results += '<thead><tr><th scope="col"></th><th scope="col">Department Name</th><th scope="col">Flag</th></tr></thead></tbody>';
    for (var i = 0; i < msg.length; i++) {
        var status = 'InActive';
        if (msg[i].flag == '1') {
            status = 'Active';
        }
        results += '<tr style="background-color:' + COLOR[l] + '"><td><input id="btn_poplate" type="button"  onclick="getcatgeory(this)" name="submit" class="btn btn-primary" value="Edit" /></td>';
        results += '<td scope="row" class="1" >' + msg[i].Categoryname + '</td>';
        results += '<td scope="row" class="2" >' + status + '</td>';
        results += '<td scope="row" style="display:none" class="4" >' + msg[i].tcategory + '</td>';
        results += '<td style="display:none" class="3">' + msg[i].sno + '</td></tr>';
        l = l + 1;
        if (l == 4) {
            l = 0;
        }
    }
    results += '</table></div>';
    $("#div_categorydata").html(results);
}
var serial2 = 0;
function subcategorymanagement() {
    var subcategoryname = document.getElementById('txt_catgry_subname').value;
    var categoryname = document.getElementById('cmb_catgry_catgryname').value;
    var subproductflag = document.getElementById('cmb_catgry_flag').value;
    var description = document.getElementById('txtSubCategoryDescription').value;
    var operationtype = document.getElementById('btn_subcatgry_save').value;
    var sno = serial2;
    var data = { 'operation': 'subcategorytypemanagement', 'sno': sno, 'subcategoryname': subcategoryname, 'categoryname': categoryname, 'subproductflag': subproductflag, 'description': description, 'operationtype': operationtype };
    var s = function (msg) {
        if (msg) {
            alert(msg);
            subcategoryclear();
            updatesubcategorytype();
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
}
function subcategoryclear() {
    document.getElementById('txt_catgry_subname').value = "";
    document.getElementById('cmb_catgry_catgryname').value = "";
    document.getElementById('txtSubCategoryDescription').value = "";
    document.getElementById('cmb_catgry_flag').value = "";
    document.getElementById('btn_subcatgry_save').value = "SAVE";
}
function updatesubcategorytype() {
    var data = { 'operation': 'Updatesubcategorytypemanage' };
    var s = function (msg) {
        if (msg) {
            updatesubcategorytypemanagement(msg);
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
}
function getsubcatgeory(thisid) {
    var subcatname = $(thisid).parent().parent().children('.1').html();
    var Categoryname = $(thisid).parent().parent().children('.2').html();
    var status = $(thisid).parent().parent().children('.3').html();
    var sno = $(thisid).parent().parent().children('.4').html();
    var description = $(thisid).parent().parent().children('.5').html();
    document.getElementById('txt_catgry_subname').value = subcatname;
    document.getElementById('cmb_catgry_flag').value = status;
    document.getElementById('txtSubCategoryDescription').value = description;
    document.getElementById('btn_subcatgry_save').value = "MODIFY";
    $("#cmb_catgry_catgryname").find("option:contains('" + Categoryname + "')").each(function () {
        if ($(this).text() == Categoryname) {
            $(this).attr("selected", "selected");
        }
    });
    serial2 = sno;
}

function updatesubcategorytypemanagement(msg) {
    var l = 0;
    var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
    var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
    results += '<thead><tr><th scope="col"></th><th scope="col">Subcategory Name</th><th>Category Name</th><th>Description</th><th>Status</th></tr></thead></tbody>';

    for (var i = 0; i < msg.length; i++) {
        var status = 'InActive';
        if (msg[i].flag == '1') {
            status = 'Active';
        }
        results += '<tr style="background-color:' + COLOR[l] + '"><td><input id="btn_poplate" type="button"  onclick="getsubcatgeory(this)" name="submit" class="btn btn-primary" value="Edit" /></td>';
        results += '<td scope="row" class="1" >' + msg[i].subcatname + '</td>';
        results += '<td scope="row" class="2" >' + msg[i].Categoryname + '</td>';
        results += '<td scope="row" class="5" >' + msg[i].description + '</td>';
        results += '<td scope="row" class="3" >' + status + '</td>';
        results += '<td style="display:none" class="4">' + msg[i].sno + '</td></tr>';
        l = l + 1;
        if (l == 4) {
            l = 0;
        }

    }

    results += '</table></div>';
    $("#div_sub_categorydata").html(results);
}
var serial3 = 0;
function productunitsmanagement() {
    var cmbproductcategory = document.getElementById('cmb_products_category').value;
    var productsubcategory = document.getElementById('cmb_products_subcatgry').value;
    var productname = document.getElementById('txt_products_prdtsname').value;
    var tally_productname = document.getElementById('txt_tally_prdtsname').value;
    var ProductCode = document.getElementById('txtProductCode').value;
    var productsqty = document.getElementById('txt_products_qty').value;

    var pieces = document.getElementById('txtpieces').value;

    var productsunits = document.getElementById('cmb_products_qtymeasurement').value;
    var productsunitprice = document.getElementById('txt_products_unitprice').value;
    var operationtype = document.getElementById('btn_products_save').value;
    var branchesproductsflag = document.getElementById('cmb_products_flag').value;
    var prdtinvsno = document.getElementById('cmb_product_inv').value;
    var invqty = document.getElementById('txt_invqty').value;


    var ifdflag = document.getElementById('ddlifdflag').value;

    var specification = document.getElementById('txtSpecifications').value;
    var materialtype = document.getElementById('ddlmaterialtype').value;
    var perunitprice = document.getElementById('txtPerUnitPrice').value;

    var HSNCode = document.getElementById('txtHSNCode').value;
    var igst = document.getElementById('txt_igst').value;
    var cgst = document.getElementById('txt_cgst').value;
    var sgst = document.getElementById('txt_sgst').value;
    var gsttaxcategory = document.getElementById('slct_gsttaxcategory').value;
    var description = document.getElementById('txtDescription').value;

    var sno = serial3;
    var data = { 'operation': 'productunitsmanagement', 'sno': sno, 'ProductCode': ProductCode, 'cmbproductcategory': cmbproductcategory, 'productsubcategory': productsubcategory, 'tproductname': tally_productname, 'productname': productname, 'productsqty': productsqty, 'productsunits': productsunits, 'productsunitprice': productsunitprice, 'operationtype': operationtype, 'branchesproductsflag': branchesproductsflag, 'prdtinvsno': prdtinvsno, 'specification': specification, 'materialtype': materialtype, 'perunitprice': perunitprice, 'HSNCode': HSNCode, 'igst': igst, 'cgst': cgst, 'sgst': sgst, 'gsttaxcategory': gsttaxcategory, 'pieces': pieces, 'invqty': invqty, 'description': description, 'ifdflag': ifdflag };
    var s = function (msg) {
        if (msg) {
            alert(msg);
            productunitsclear();
            Bindproductunits();
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
}
function productunitsclear() {
    document.getElementById('cmb_products_category').value = "";
    document.getElementById('cmb_products_subcatgry').value = "";
    document.getElementById('txt_products_prdtsname').value = "";
    document.getElementById('txt_tally_prdtsname').value = "";
    document.getElementById('txt_products_qty').value = "";
   document.getElementById('txtpieces').value="";
    document.getElementById('cmb_products_qtymeasurement').value = "";
    document.getElementById('txt_products_unitprice').value = "";
    document.getElementById('txtProductCode').value = "";
    document.getElementById('cmb_products_flag').value = "";
    document.getElementById('ddlifdflag').value = "";
    
    document.getElementById('txtSpecifications').value = "";
    document.getElementById('ddlmaterialtype').value = "";
    document.getElementById('txtPerUnitPrice').value = "";
    document.getElementById('lblspecifiactions').innerHTML = "";
    document.getElementById('btn_products_save').value = "SAVE";
    document.getElementById('cmb_product_inv').value = "";

    document.getElementById('txt_invqty').value = "";
    document.getElementById('txtHSNCode').value = "";
    document.getElementById('txtDescription').value = "";
    document.getElementById('txt_igst').value = "";
    document.getElementById('txt_cgst').value = "";
    document.getElementById('txt_sgst').value = "";
    document.getElementById('slct_gsttaxcategory').value = "";
}
function Bindproductunits() {
    var data = { 'operation': 'Updateproductunitsmanage'};
    var s = function (msg) {
        if (msg) {
            UpdateProductUnitmanagement(msg);
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
}
function getproductsdata(thisid) {
    var Categoryname = $(thisid).parent().parent().children('.1').html();
    var SubCatName = $(thisid).parent().parent().children('.2').html();
    var ProductName = $(thisid).parent().parent().children('.3').html();
    var Qty = $(thisid).parent().parent().children('.4').html();
    var ProductUnit = $(thisid).parent().parent().children('.5').html();
    var UnitPrice = $(thisid).parent().parent().children('.6').html();
//    var VatPercent = $(thisid).parent().parent().children('.7').html();
    var status = $(thisid).parent().parent().children('.8').html();
    var InvName = $(thisid).parent().parent().children('.9').html();
    var sno = $(thisid).parent().parent().children('.10').html();
    var TProductName = $(thisid).parent().parent().children('.11').html();
    var ProductCode = $(thisid).parent().parent().children('.12').html();
    var image = $(thisid).parent().parent().children('.13').html();
    var ftplocation = $(thisid).parent().parent().children('.14').html();

    var specification = $(thisid).parent().parent().children('.15').html();
    var materialtype = $(thisid).parent().parent().children('.16').html();
    var perunitprice = $(thisid).parent().parent().children('.17').html();

    var categorysno = $(thisid).parent().parent().children('.18').html();
    var subcatsno = $(thisid).parent().parent().children('.19').html();


    var hsncode = $(thisid).parent().parent().children('.20').html();
    var igst = $(thisid).parent().parent().children('.21').html();
    var cgst = $(thisid).parent().parent().children('.22').html();
    var sgst = $(thisid).parent().parent().children('.23').html();
    var gsttaxcategory = $(thisid).parent().parent().children('.24').html();
    var pieces = $(thisid).parent().parent().children('.25').html();

    var invqty = $(thisid).parent().parent().children('.26').html();
    var description = $(thisid).parent().parent().children('.27').html();
    var ifdflag = $(thisid).parent().parent().children('.28').html();
    
    //    document.getElementById('cmb_products_category').value = Categoryname;
    //    document.getElementById('txt_catgry_subname').value = SubCatName;
    document.getElementById('cmb_catgry_flag').value = status;
    document.getElementById('btn_subcatgry_save').value = "MODIFY";


    document.getElementById('cmb_products_category').value = categorysno;
    products_cateegoryname_onchange();
    document.getElementById('cmb_products_subcatgry').value = subcatsno;
    //    $("#cmb_products_category").find("option:contains('" + Categoryname + "')").each(function () {
    //        if ($(this).text() == Categoryname) {
    //            $(this).attr("selected", "selected");
    //        }
    //    });
    //    
    //    $("#cmb_products_subcatgry").find("option:contains('" + SubCatName + "')").each(function () {
    //        if ($(this).text() == SubCatName) {
    //            $(this).attr("selected", "selected");
    //        }
    //    });
    $("#cmb_product_inv").find("option:contains('" + InvName + "')").each(function () {
        if ($(this).text() == InvName) {
            $(this).attr("selected", "selected");
        }
    });
    var rndmnum = Math.floor((Math.random() * 10) + 1);
    img_url = ftplocation + image + '?v=' + rndmnum;
    if (image != "") {
        $('#main_img').attr('src', img_url).width(200).height(200);
    }
    else {
        $('#main_img').attr('src', 'Images/Employeeimg.jpg').width(200).height(200);
    }
    document.getElementById('txt_products_prdtsname').value = ProductName;
    document.getElementById('txtProductCode').value = ProductCode;
    document.getElementById('txt_tally_prdtsname').value = TProductName;
    document.getElementById('txt_products_qty').value = Qty;
    document.getElementById('txtpieces').value = pieces;
    document.getElementById('cmb_products_qtymeasurement').value = ProductUnit;
    document.getElementById('txt_products_unitprice').value = UnitPrice;
    //document.getElementById('txt_vat_percent').value = VatPercent;
    document.getElementById('cmb_products_flag').value = status;
    document.getElementById('ddlifdflag').value = ifdflag;
    
    serial3 = sno;
    document.getElementById('btn_products_save').value = "MODIFY";
    document.getElementById('txtSpecifications').value = specification;
    document.getElementById('ddlmaterialtype').value = materialtype;
    document.getElementById('txtPerUnitPrice').value = perunitprice;
    document.getElementById('lblspecifiactions').innerHTML = specification;
    document.getElementById('lbl_topempname').innerHTML = ProductName;

    document.getElementById('txtHSNCode').value = hsncode;
    document.getElementById('txt_igst').value = igst;
    document.getElementById('txt_cgst').value = cgst;
    document.getElementById('txt_sgst').value = sgst;
    document.getElementById('slct_gsttaxcategory').value = gsttaxcategory;
    document.getElementById('txt_invqty').value = invqty;
    document.getElementById('txtDescription').value = description;
}
function getproductslineChart(thisid) {
    var productname = $(thisid).parent().parent().children('.3').html();
    var productid = $(thisid).parent().parent().children('.10').html();
    //    window.open("AgentRemarks.aspx");
    var data = { 'operation': 'get_Product', 'productname': productname, 'productid': productid };
    var s = function (msg) {
        if (msg) {
            window.open("ProductLineChart.aspx", "_self");
        }
        else {

        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
}
function UpdateProductUnitmanagement(msg) {
    var l = 0;
    var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
    var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
    results += '<thead><tr><th scope="col"></th><th>Category Name</th><th scope="col">Subcategory Name</th><th scope="col">Product Name</th><th scope="col">ProductCode</th><th scope="col">Qty</th><th scope="col">Product Unit</th><th scope="col">Unit Price</th><th scope="col">Status</th><th scope="col">Inventory Name</th><th scope="col">Tally Product</th></tr></thead></tbody>';
    for (var i = 0; i < msg.length; i++) {
        var status = 'InActive';
        var ifdflag = 'InActive';
        if (msg[i].flag == '1') {
            status = 'Active';
        }
        if (msg[i].ifdflag == '1') {
            ifdflag = 'Active';
        }
        results += '<tr style="background-color:' + COLOR[l] + '"><td><input id="btn_poplate" type="button"  onclick="getproductsdata(this)" name="submit" class="btn btn-primary" value="Edit" /></td>';
        results += '<td scope="row" class="1" >' + msg[i].Categoryname + '</td>';
        results += '<td scope="row" class="2" >' + msg[i].SubCatName + '</td>';
        results += '<td scope="row" class="3" >' + msg[i].ProductName + '</td>';
        results += '<td scope="row" class="12" >' + msg[i].ProductCode + '</td>';
        results += '<td scope="row" class="4" >' + msg[i].Qty + '</td>';
        results += '<td scope="row" class="5" >' + msg[i].ProductUnit + '</td>';
        results += '<td scope="row" class="6" >' + msg[i].UnitPrice + '</td>';
        results += '<td scope="row" style="display:none" class="7" >' + msg[i].VatPercent + '</td>';
        results += '<td scope="row" class="8" >' + status + '</td>';
        results += '<td scope="row" class="9" >' + msg[i].InvName + '</td>';

        results += '<td style="display:none" class="26" >' + msg[i].invqty + '</td>';

        results += '<td  class="11">' + msg[i].TProductName + '</td>';
        results += '<td style="display:none" class="10">' + msg[i].sno + '</td>';
        results += '<td style="display:none" scope="row" class="13" >' + msg[i].images + '</td>';
        results += '<td style="display:none" scope="row" class="14" >' + msg[i].ftplocation + '</td>';
        results += '<td style="display:none" class="15" >' + msg[i].specification + '</td>';
        results += '<td style="display:none" class="16" >' + msg[i].materialtype + '</td>';

        results += '<td style="display:none" class="18" >' + msg[i].categorysno + '</td>';
        results += '<td style="display:none" class="19" >' + msg[i].SubCatsno + '</td>';
        results += '<td style="display:none" class="17" >' + msg[i].perunitprice + '</td>';

        results += '<td style="display:none" class="20" >' + msg[i].hsncode + '</td>';
        results += '<td style="display:none" class="21" >' + msg[i].igst + '</td>';
        results += '<td style="display:none" class="22" >' + msg[i].cgst + '</td>';
        results += '<td style="display:none" class="23" >' + msg[i].sgst + '</td>';
        results += '<td style="display:none" class="24" >' + msg[i].gsttaxcategory + '</td>'; 
        results += '<td style="display:none" class="25" >' + msg[i].pieces + '</td>';
        results += '<td style="display:none" class="27" >' + msg[i].description + '</td>';
        results += '<td style="display:none" class="28" >' + ifdflag + '</td>';
        results += '<td><input id="btn_poplate" style="display:none" type="button"  onclick="getproductslineChart(this);"  name="submit" class="btn btn-primary" value="View" /></td></tr>';
        l = l + 1;
        if (l == 4) {
            l = 0;
        }
    }
    results += '</table></div>';
    $("#div_ProductsData").html(results);
}

var serial4 = 0;
var superbranch = 0;
function branchesmanagement() {
    //    var brnchplantname = "";
    var subcategorystrng = "";
    var checkinputs = $('#divcategiriesdata').find('.checkinput');
    checkinputs.each(function (list) {
        var checkbox = checkinputs[list];
        if (checkbox.checked) {
            subcategorystrng = checkbox.value;
        }
    });
    if (subcategorystrng.length > 0) {
        checkedbranch = subcategorystrng;
        if (lvltype == "Admin") {
            //        brnchplantname = document.getElementById('cmb_brnch_plantname').value;
        }
        var brnchsname = document.getElementById('txt_branchname').value;
        var branchcode = document.getElementById('txt_branchcode').value;
        var tinno = document.getElementById('txt_tinno').value;
        var ledgerdr = document.getElementById('txt_ledgerdr').value;
        var state = document.getElementById('txt_state').value;
        var incentive = document.getElementById('txtIncentive').value;
        var panno = document.getElementById('txtpanno').value;

        var whccode = document.getElementById('txtWhccode').value;
        var customercode = document.getElementById('txtCustomerCode').value;
      
        var tally_branchname = document.getElementById('txt_tally_branchname').value;
        var txtsr = document.getElementById('txtsr').value;
        var cmbsalestype = document.getElementById('cmb_salestype').value;
        var cmbcollectiontype = document.getElementById('cmb_collectiontype').value;
        var cmblimittype = document.getElementById('cmblimittype').value;
        var txtdaytarget = document.getElementById('txt_daytarget').value;
        var txtweektarget = document.getElementById('txt_weektarget').value;
        var txtduelimit = document.getElementById('txtduelimit').value;
        var txtmonthtarget = document.getElementById('txt_monthtarget').value;
        var branchesManageflag = document.getElementById('cmb_branchesManageflag').value;
        var branchesManage_Phone = document.getElementById('txt_branch_mobile').value;
         var LedgerCode = document.getElementById('txtLedgerCode').value;
        var txtaddress = document.getElementById('txtaddress').value;
        var operationtype = document.getElementById('btn_brnchs_mng_save').value;
        var brnchlat = document.getElementById('txtbrnchlat').value;
        var brnchlong = document.getElementById('txtbrnchlong').value;
        var txtotherprdt = document.getElementById('txtotherprdt').value;
        var brncsno = serial4;
        var superbrnch = superbranch;
        var data = { 'operation': 'branchsmanagement', 'LedgerCode': LedgerCode, 'branchcode': branchcode, 'whccode': whccode, 'customercode': customercode, 'tinno': tinno, 'ledgerdr': ledgerdr, 'state': state, 'incentive': incentive, 'panno': panno, 'brncsno': brncsno, 'tally_branchname': tally_branchname, 'superbrnch': superbrnch, 'checkedbranch': checkedbranch, 'brnchsname': brnchsname, 'cmbsalestype': cmbsalestype, 'cmbcollectiontype': cmbcollectiontype, 'cmblimittype': cmblimittype, 'txtsr': txtsr, 'txtdaytarget': txtdaytarget, 'txtweektarget': txtweektarget, 'txtmonthtarget': txtmonthtarget, 'txtduelimit': txtduelimit, 'branchesManageflag': branchesManageflag, 'branchesManage_Phone': branchesManage_Phone, 'txtaddress': txtaddress, 'operationtype': operationtype, 'brnchlat': brnchlat, 'brnchlong': brnchlong, 'txtotherprdt': txtotherprdt };
        var s = function (msg) {
            if (msg) {
                alert(msg);
                branchesmanagementclear();
                Bindbranchmanagement();
//                branchmgntmapping();
            }
            else {
            }
        };
        var e = function (x, h, e) {
        };
        $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
        callHandler(data, s, e);
    }
    else {
        alert("Please fill required fields");
    }
}
function branchesmanagementclear() {
    //    if (lvltype == "Admin") {
    ////        document.getElementById('cmb_brnch_plantname').value = "";
    //    }
    serial4 = 0;
    bindBranch();

    //    document.getElementById('cmb_brnch_salesoffice').value = "";
    document.getElementById('txtotherprdt').value = "";
    document.getElementById('txtaddress').value = "";
    document.getElementById('txt_branchname').value = "";
    document.getElementById('txt_tally_branchname').value = "";
    document.getElementById('cmb_salestype').selectedIndex = 0;
    document.getElementById('cmb_collectiontype').selectedIndex = 0;
    document.getElementById('txt_daytarget').value = "0";
    document.getElementById('txt_weektarget').value = "0";
    document.getElementById('txt_monthtarget').value = "0";
    document.getElementById('cmb_branchesManageflag').selectedIndex = 0;
    document.getElementById('txt_branch_mobile').value = "";
    document.getElementById('txtLedgerCode').value = "";

    document.getElementById('btn_brnchs_mng_save').value = "SAVE";
    document.getElementById('txtbrnchlat').value = "0";
    document.getElementById('txtbrnchlong').value = "0";
    //akbar
     document.getElementById('txtWhccode').value = "";
    document.getElementById('txtCustomerCode').value = "";
    document.getElementById('txt_branchcode').value = "";
    document.getElementById('txt_tinno').value = "";
    document.getElementById('txt_ledgerdr').value = "";
    document.getElementById('txtIncentive').value = "";
    document.getElementById('txt_state').value = "";
    document.getElementById('txtpanno').value = "";
    //akbar
}
function Bindbranchmanagement() {
   
    var data = { 'operation': 'Updatebranchmanagement' };
    var s = function (msg) {
        if (msg) {
            BindingBranchManagement(msg);
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
}
function getme(thisid) {
    var refsno = $(thisid).parent().parent().children('.2').html();
    var BranchName = $(thisid).parent().parent().children('.3').html();
    var Salestype = $(thisid).parent().parent().children('.4').html();
    var collectiontyp = $(thisid).parent().parent().children('.5').html();
    var phone = $(thisid).parent().parent().children('.6').html();
    var address = $(thisid).parent().parent().children('.7').html();
    var DTarget = $(thisid).parent().parent().children('.8').html();
    var WTarget = $(thisid).parent().parent().children('.9').html();
    var MTarget = $(thisid).parent().parent().children('.10').html();
    var status = $(thisid).parent().parent().children('.11').html();
    //    var sno = $(thisid).parent().parent().children('.12').html();
    var lat = $(thisid).parent().parent().children('.13').html();
    var lng = $(thisid).parent().parent().children('.14').html();
    var Otherbrands = $(thisid).parent().parent().children('.15').html();
    var duelimit = $(thisid).parent().parent().children('.16').html();
    var Salesrep = $(thisid).parent().parent().children('.17').html();
    var Due_Limit_Type = $(thisid).parent().parent().children('.18').html();
    var LimitDays = $(thisid).parent().parent().children('.19').html();
    var sno = $(thisid).parent().parent().children('.20').html();
    var TBranchName = $(thisid).parent().parent().children('.21').html();

    //akbar
    var branchcode = $(thisid).parent().parent().children('.22').html();
    var tinno = $(thisid).parent().parent().children('.23').html();
    var ledgerdr = $(thisid).parent().parent().children('.24').html();
    var incentive = $(thisid).parent().parent().children('.25').html();
    var panno = $(thisid).parent().parent().children('.26').html();
    var state = $(thisid).parent().parent().children('.27').html();

    var whccode = $(thisid).parent().parent().children('.28').html();
    var customercode = $(thisid).parent().parent().children('.29').html();
    var ledgercode = $(thisid).parent().parent().children('.30').html();

      document.getElementById('txtWhccode').value = whccode;
    document.getElementById('txtCustomerCode').value = customercode;
    document.getElementById('txt_branchcode').value = branchcode;
    document.getElementById('txt_tinno').value = tinno;
    document.getElementById('txt_ledgerdr').value = ledgerdr;
    document.getElementById('txtIncentive').value = incentive;
    document.getElementById('txt_state').value = state;
    document.getElementById('txtpanno').value = panno;
    //akbar

    document.getElementById('txt_branchname').value = BranchName;
    document.getElementById('txt_tally_branchname').value = TBranchName;
    document.getElementById('txtsr').value = Salesrep;
    document.getElementById('txt_branch_mobile').value = phone;
    document.getElementById('txtLedgerCode').value = ledgercode;

    
    document.getElementById('txtaddress').value = address;
    document.getElementById('txt_daytarget').value = DTarget;
    document.getElementById('txt_weektarget').value = WTarget;
    document.getElementById('txt_monthtarget').value = MTarget;
    document.getElementById('cmb_collectiontype').value = collectiontyp;
    document.getElementById('cmblimittype').value = Due_Limit_Type;
    if (Due_Limit_Type == "Amount") {
        document.getElementById('txtduelimit').value = duelimit;
    }
    if (Due_Limit_Type == "Days") {
        document.getElementById('txtduelimit').value = LimitDays;

    }
    $("#cmb_salestype").find("option:contains('" + Salestype + "')").each(function () {
        if ($(this).text() == Salestype) {
            $(this).attr("selected", "selected");
        }
    });
    document.getElementById('cmb_branchesManageflag').value = status;
    document.getElementById('btn_brnchs_mng_save').value = "MODIFY";
    document.getElementById('txtbrnchlat').value = lat;
    document.getElementById('txtbrnchlong').value = lng;
    document.getElementById('txtotherprdt').value = Otherbrands;
    superbranch = sno;
    serial4 = refsno;
    bindBranch();
}
function BindingBranchManagement(databind) {
    var l = 0;
    var COLOR = ["#b3ffe6", "AntiqueWhite", "#daffff", "MistyRose", "Bisque"];
    var results = '<div class="divcontainer" style="overflow:auto;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
    results += '<thead><tr><th scope="col"></th><th scope="col">#</th><th scope="col">Branch Code</th><th scope="col">Branch Name</th><th scope="col">Sales Type</th><th scope="col">Mobile</th><th scope="col">Address</th><th scope="col">Flag</th><th scope="col">Tally Name</th></tr></thead></tbody>';
    for (var i = 0; i < databind.length; i++) {


        if (databind[i].Flag == '1') {
            var status = 'Active';
        }
        else {
            var status = 'InActive';
        }
        results += '<tr style="background-color:' + COLOR[l] + '"><td><input id="btn_poplate" type="button"  onclick="getme(this)" name="submit" class="btn btn-primary" value="Edit" /></td>';
        results += '<td scope="row" class="1" style="text-align:center;">' + i + '</td>';
        results += '<td data-title="Capacity" class="2">' + databind[i].refsno + '</td>';
        results += '<td data-title="Capacity" class="3">' + databind[i].BranchName + '</td>';
        results += '<td  class="4">' + databind[i].Salestype + '</td>';
        results += '<td style="display:none" class="5">' + databind[i].collectiontyp + '</td>';
        results += '<td  class="6">' + databind[i].phone + '</td>';
        results += '<td   class="7">' + databind[i].address + '</td>';
        results += '<td style="display:none" class="8">' + databind[i].DTarget + '</td>';
        results += '<td style="display:none" class="9">' + databind[i].WTarget + '</td>';
        results += '<td style="display:none" class="10">' + databind[i].MTarget + '</td>';
        results += '<td  class="11">' + status + '</td>';
        results += '<td  class="21">' + databind[i].TBranchName + '</td>';
        results += '<td style="display:none" class="13">' + databind[i].lat + '</td>';
        results += '<td style="display:none"  class="14">' + databind[i].lng + '</td>';
        results += '<td style="display:none" class="15">' + databind[i].Otherbrands + '</td>';
        results += '<td style="display:none" class="16">' + databind[i].duelimit + '</td>';
        results += '<td style="display:none" class="17">' + databind[i].Salesrep + '</td>';
        results += '<td  style="display:none" class="18">' + databind[i].Due_Limit_Type + '</td>';
        results += '<td style="display:none" class="19">' + databind[i].LimitDays + '</td>';
        //akbar
        results += '<td style="display:none"  class="22">' + databind[i].branchcode + '</td>';
        results += '<td style="display:none" class="23">' + databind[i].tinno + '</td>';
        results += '<td style="display:none" class="24">' + databind[i].ledgerdr + '</td>';
        results += '<td style="display:none" class="25">' + databind[i].incentive + '</td>';
        results += '<td  style="display:none" class="26">' + databind[i].panno + '</td>';
        results += '<td style="display:none" class="27">' + databind[i].state + '</td>';
        results += '<td  style="display:none" class="28">' + databind[i].whcode + '</td>';
        results += '<td style="display:none" class="29">' + databind[i].customercode + '</td>';
        results += '<td style="display:none" class="30">' + databind[i].ladger_dr_code + '</td>';
        //akbar
        results += '<td style="display:none" class="20">' + databind[i].sno + '</td></tr>';
        l = l + 1;
        if (l == 4) {
            l = 0;
        }
    }
    results += '</table></div>';
    $("#div_BranchData").html(results);


//    $("#grd_branchesmanage").jqGrid("clearGridData");
//    var newarray = [];
//    var Headarray = [];
//    var headdatacol = databind[1];
//    var datacol = databind;
//    for (var Booking in databind) {
//        var status = 'InActive';
//        if (databind[Booking].Flag == '1') {

//            status = 'Active';
//        }
//        newarray.push({ 'Sno': newarray.length + 1, 'BranchCode': databind[Booking].refsno, 'BranchName': databind[Booking].BranchName,
//            'SalesType': databind[Booking].Salestype, 'Collection Type': databind[Booking].collectiontyp, 'Mobile': databind[Booking].phone,
//            'Address': databind[Booking].address, 'DayTarget': databind[Booking].DTarget, 'WeekTarget': databind[Booking].WTarget, 'MonthTarget': databind[Booking].MTarget,
//            'Flag': status, 'Branchsno': databind[Booking].refsno, 'Lat': databind[Booking].lat, 'Lng': databind[Booking].lng, 'Otbrnds': databind[Booking].Otherbrands,
//            'DueLimit': databind[Booking].duelimit, 'Salesrep': databind[Booking].Salesrep, 'Due_Limit_Type': databind[Booking].Due_Limit_Type, 'LimitDays': databind[Booking].LimitDays, 
//        'r_sno': databind[Booking].sno });
//    }
//    $("#grd_branchesmanage").jqGrid({
//        datatype: "local",
//        height: 255,
//        autowidth: true,
//        colNames: Headarray,
//        colModel: [{ name: 'Sno', index: 'invdate', width: 20, sortable: false, align: 'center' },
//        { name: 'BranchCode', index: 'note', width: 40, sortable: false, align: 'center'},
//        {name: 'BranchName', index: 'invdate', width: 80, sortable: false, align: 'center' },
//        { name: 'SalesType', index: 'invdate', width: 60, sortable: false, align: 'center' },
//        { name: 'Collection Type', index: 'invdate', width: 50, sortable: false, align: 'center' },
//        { name: 'Mobile', index: 'invdate', width: 60, sortable: false, align: 'center' },
//        { name: 'Address', index: 'invdate', width: 60, sortable: false, align: 'center' },
//        { name: 'DayTarget', index: 'invdate', width: 40, sortable: false, align: 'center' },
//        { name: 'WeekTarget', index: 'invdate', width: 40, sortable: false, align: 'center' },
//        { name: 'MonthTarget', index: 'invdate', width: 40, sortable: false, align: 'center' },
//{ name: 'Flag', index: 'note', width: 30, sortable: false, align: 'center' },
//{ name: 'Branchsno', index: 'note', width: 100, sortable: false, align: 'center', hidden: true },
//{ name: 'Lat', index: 'note', width: 100, sortable: false, align: 'center', hidden: true },
//{ name: 'Lng', index: 'note', width: 100, sortable: false, align: 'center', hidden: true },
//{ name: 'Otbrnds', index: 'note', width: 100, sortable: false, align: 'center', hidden: true },
//{ name: 'DueLimit', index: 'note', width: 100, sortable: false, align: 'center', hidden: true },
//{ name: 'Salesrep', index: 'note', width: 100, sortable: false, align: 'center', hidden: true },
//{ name: 'Due_Limit_Type', index: 'note', width: 100, sortable: false, align: 'center', hidden: true },
//{ name: 'LimitDays', index: 'note', width: 100, sortable: false, align: 'center', hidden: true },
//{ name: 'r_sno', index: 'note', width: 100, sortable: false, align: 'center',hidden:true}],
// rowNum: 10,
//        rowList: [5, 10, 30],
//        // rownumbers: true,
//        gridview: true,
//        loadonce: true,
//        pager: "#page4",
//        caption: "Branch Details Manage"
//    }).jqGrid('navGrid', '#page4', { edit: false, add: false, del: false, search: false, refresh: false });
//    var mydata = newarray;
//    for (var i = 0; i <= mydata.length; i++) {

//        jQuery("#grd_branchesmanage").jqGrid('addRowData', i + 1, mydata[i]);
//    }
//    $("#grd_branchesmanage").jqGrid('setGridParam', { onSelectRow: function (rowid, iRow, iCol, e) {
//        if (lvltype == "Admin") {
//        }
//        // var SalesOffice = $('#grd_branchesmanage').getCell(rowid, 'SalesOffice');
//        var BranchName = $('#grd_branchesmanage').getCell(rowid, 'BranchName');
//        var Salestype = $('#grd_branchesmanage').getCell(rowid, 'SalesType');
//        var colectiontype = $('#grd_branchesmanage').getCell(rowid, 'Collection Type');
//        var mobileph = $('#grd_branchesmanage').getCell(rowid, 'Mobile');
//        var address = $('#grd_branchesmanage').getCell(rowid, 'Address');
//        var dt = $('#grd_branchesmanage').getCell(rowid, 'DayTarget');
//        var wt = $('#grd_branchesmanage').getCell(rowid, 'WeekTarget');
//        var mt = $('#grd_branchesmanage').getCell(rowid, 'MonthTarget');
//        var Flag = $('#grd_branchesmanage').getCell(rowid, 'Flag');
//        var lat = $('#grd_branchesmanage').getCell(rowid, 'Lat');
//        var lng = $('#grd_branchesmanage').getCell(rowid, 'Lng');
//        var otbrnds = $('#grd_branchesmanage').getCell(rowid, 'Otbrnds');
//        var DueLimit = $('#grd_branchesmanage').getCell(rowid, 'DueLimit');
//        var Salesrep = $('#grd_branchesmanage').getCell(rowid, 'Salesrep');
//        var Due_Limit_Type = $('#grd_branchesmanage').getCell(rowid, 'Due_Limit_Type');
//        var LimitDays = $('#grd_branchesmanage').getCell(rowid, 'LimitDays');
//        
//        document.getElementById('txt_branchname').value = BranchName;
//        document.getElementById('txtsr').value = Salesrep;
//        document.getElementById('txt_branch_mobile').value = mobileph;
//        document.getElementById('txtaddress').value = address;
//        document.getElementById('txt_daytarget').value = dt;
//        document.getElementById('txt_weektarget').value = wt;
//        document.getElementById('txt_monthtarget').value = mt;
//        document.getElementById('cmb_collectiontype').value = colectiontype;
//        document.getElementById('cmblimittype').value = Due_Limit_Type;
//        if (Due_Limit_Type == "Amount") {
//            document.getElementById('txtduelimit').value = DueLimit;
//        }
//        if (Due_Limit_Type == "Days") {
//            document.getElementById('txtduelimit').value = LimitDays;

//        }
//        $("#cmb_salestype").find("option:contains('" + Salestype + "')").each(function () {
//            if ($(this).text() == Salestype) {
//                $(this).attr("selected", "selected");
//            }
//        });
//        document.getElementById('cmb_branchesManageflag').value = Flag;
//        document.getElementById('btn_brnchs_mng_save').value = "MODIFY";
//        document.getElementById('txtbrnchlat').value = lat;
//        document.getElementById('txtbrnchlong').value = lng;
//        document.getElementById('txtotherprdt').value = otbrnds;
//        superbranch = $('#grd_branchesmanage').getCell(rowid, 'r_sno');
//        serial4 = $('#grd_branchesmanage').getCell(rowid, 'Branchsno');
//        bindBranch();
//    }
//    });
//    $("#grd_branchesmanage").find('input[type=button]').hide();
}
function bindBranch() {
    $('.checkinput').each(function (i, obj) {
        if ($(this).val() == superbranch) {
            $(this).attr('checked', 'checked');
        }
        else {
            $(this).removeAttr('checked')
        }
    });
}
function bindBranchprdt() {
    $('.checkinput').each(function (i, obj) {
        if ($(this).val() == serial5) {
            $(this).attr('checked', 'checked');
        }
        else {
            $(this).removeAttr('checked')
        }
    });
}
var serial5 = 0;
var pdtsno = 0;
function branchproducts() {
    var categoryname = document.getElementById('cmb_brchprdt_Catgry_name').value;
    if (categoryname == "" || categoryname == "select") {
        alert("Please Select CategoryName");
        $("#cmb_brchprdt_Catgry_name").focus();
        return false;
    }

    var cmbsubcatgry = document.getElementById('cmb__brnch_subcatgry').value;
    if (cmbsubcatgry == "" || cmbsubcatgry == "select") {
        alert("Please Select SubCategoryName");
        $("#cmb__brnch_subcatgry").focus();
        return false;
    }
    var cmbproductname = document.getElementById('cmb_productname').value;
    if (cmbproductname == "" || cmbproductname == "select") {
        alert("Please Select ProductName");
        $("#cmb_productname").focus();
        return false;
    }
    var productunitprice = document.getElementById('txt_productunitprice').value;
    if (productunitprice == "") {
        alert("Please Provide UnitPrice");
        $("#txt_productunitprice").focus();
        return false;
    }
    var mrp = document.getElementById('txt_mrp').value;
    
    var branchproductflag = document.getElementById('cmb_branchesproductsflag').value;
    if (branchproductflag == "") {
        alert("Please Select Flag");
        $("#cmb_branchesproductsflag").focus();
        return false;
    }
    var operationtype = document.getElementById('btn_brnch_prodtuct_save').innerHTML;
    var VatPercent = document.getElementById('txt_vatPercent').value;
    if (VatPercent == "") {
        VatPercent = 0;
    }
    var cgst = document.getElementById('txtCustomerCGST').value;
    var sgst = document.getElementById('txtCustomerSGST').value;
    var igst = document.getElementById('txtCustomerIGST').value;

    var checkedbranch = CustomerSno; //
    var sno = serial5;
    var prdtsno = productsno;
    var data = { 'operation': 'save_branchproducts_click', 'checkedbranch': checkedbranch, 'categoryname': categoryname, 'cmbsubcatgry': cmbsubcatgry, 'cmbproductname': cmbproductname, 'productunitprice': productunitprice, 'branchproductflag': branchproductflag, 'operationtype': operationtype, 'sno': sno, 'prdtsno': prdtsno, 'VatPercent': VatPercent, 'cgst': cgst, 'sgst': sgst, 'igst': igst, 'mrp': mrp };
    //  var data = { 'operation': 'branchproducts', 'checkedbranch': checkedbranch, 'categoryname': categoryname, 'cmbsubcatgry': cmbsubcatgry, 'cmbproductname': cmbproductname, 'productunitprice': productunitprice, 'branchproductflag': branchproductflag, 'operationtype': operationtype, 'sno': sno, 'prdtsno': prdtsno, 'VatPercent': VatPercent };
    var s = function (msg) {
        if (msg) {
            alert(msg);
            $('#CustomerProductFillform').css('display', 'none');
            $('#Customer_Product').css('display', 'block');
            $('#div_Branch_Products').css('display', 'block');
//                document.getElementById('cmb_brchprdt_Catgry_name').selectedIndex = 0;
//                document.getElementById('cmb__brnch_subcatgry').selectedIndex = 0;
//                document.getElementById('cmb_productname').selectedIndex = 0;
            document.getElementById('txt_productunitprice').value = "";
            document.getElementById('txt_vatPercent').value = "";
            document.getElementById('cmb_branchesproductsflag').selectedIndex = 0;
            document.getElementById('txtCustomerCGST').value = "";
            document.getElementById('txtCustomerSGST').value = "";
            document.getElementById('txtCustomerIGST').value = "";
            document.getElementById('btn_brnch_prodtuct_save').innerHTML = "SAVE";
            BindBranchProducts();
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
}
function branchproductsclear() {

    $('#CustomerProductFillform').css('display', 'none');
    $('#Customer_Product').css('display', 'block');
    $('#div_Branch_Products').css('display', 'block');
    document.getElementById('cmb_brchprdt_Catgry_name').selectedIndex = 0;
    document.getElementById('cmb__brnch_subcatgry').selectedIndex = 0;
    document.getElementById('cmb_productname').selectedIndex = 0;
    document.getElementById('txt_productunitprice').value = "";
    document.getElementById('txt_vatPercent').value = "";
    document.getElementById('cmb_branchesproductsflag').selectedIndex = 0;
    document.getElementById('txtCustomerCGST').value = "";
    document.getElementById('txtCustomerSGST').value = "";
    document.getElementById('txtCustomerIGST').value = "";
    document.getElementById('btn_brnch_prodtuct_save').innerHTML = "SAVE";
}
function BindBranchProducts() {
    var data = { 'operation': 'UpdateBranchProducts', 'CustomerSno': CustomerSno };
    var s = function (msg) {
        if (msg) {
            BindingBranchProducts(msg);
        }
        else {
        }
    };
    var e = function (x, h, e) {
    };
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    callHandler(data, s, e);
}
var productsno = 0;
function getbranchproduct(thisid) {
    $('#CustomerProductFillform').css('display', 'block');
    $('#Customer_Product').css('display', 'none');
    $('#div_Branch_Products').css('display', 'none');
    var BranchName = $(thisid).parent().parent().children('.1').html();
    var Categoryname = $(thisid).parent().parent().children('.2').html();
    var SubCatName = $(thisid).parent().parent().children('.3').html();
    var ProductName = $(thisid).parent().parent().children('.4').html();
    var UnitPrice = $(thisid).parent().parent().children('.5').html();
    var VatPercent = $(thisid).parent().parent().children('.6').html();
    var DTarget = $(thisid).parent().parent().children('.7').html();
    var WTarget = $(thisid).parent().parent().children('.8').html();
    var MTarget = $(thisid).parent().parent().children('.9').html();
    var status = $(thisid).parent().parent().children('.10').html();
    var branchsno = $(thisid).parent().parent().children('.11').html();
    var prdsno = $(thisid).parent().parent().children('.12').html();
    var cgst = $(thisid).parent().parent().children('.15').html();
    var sgst = $(thisid).parent().parent().children('.16').html();
    var igst = $(thisid).parent().parent().children('.17').html();
    var mrp = $(thisid).parent().parent().children('.18').html();
   
    $("#cmb_branchname").find("option:contains('" + BranchName + "')").each(function () {
        if ($(this).text() == BranchName) {
            $(this).attr("selected", "selected");
        }
    });
    $("#cmb_brchprdt_Catgry_name").find("option:contains('" + Categoryname + "')").each(function () {
        if ($(this).text() == Categoryname) {
            $(this).attr("selected", "selected");
        }
    });
    productsdata_categoryname_onchange();
    $("#cmb__brnch_subcatgry").find("option:contains('" + SubCatName + "')").each(function () {
        if ($(this).text() == SubCatName) {
            $(this).attr("selected", "selected");
        }
    });
    productsdata_subcategory_onchange();
    $("#cmb_productname").find("option:contains('" + ProductName + "')").each(function () {
        if ($(this).text() == ProductName) {
            $(this).attr("selected", "selected");
        }
    });
    document.getElementById('cmb_branchesproductsflag').value = status;
    document.getElementById('txt_productunitprice').value = UnitPrice;
    document.getElementById('txt_vatPercent').value = VatPercent;
    //    document.getElementById('txt_Branchprdt_daytarget').value = DTarget;
    //    document.getElementById('txt_Branchprdt_weektarget').value = WTarget;
    //    document.getElementById('txt_Branchprdt_monthtarget').value = MTarget;
    document.getElementById('btn_brnch_prodtuct_save').value = "MODIFY";
    document.getElementById('txtHiddenName').value = branchsno;
    document.getElementById('cmb_branchesproductsflag').value = status;
    document.getElementById('txt_productunitprice').value = UnitPrice;
    document.getElementById('txt_vatPercent').value = VatPercent;
    document.getElementById('txtCustomerCGST').value = cgst;
    document.getElementById('txtCustomerSGST').value = sgst;
    document.getElementById('txtCustomerIGST').value = igst;
    document.getElementById('txt_mrp').value = mrp;
    document.getElementById('btn_brnch_prodtuct_save').innerHTML = "MODIFY";
    serial5 = branchsno;
    productsno = prdsno;
    bindBranchprdt();
}
function BindingBranchProducts(msg) {
    var l = 0;
//    searchproductagentnames(msg)
    var COLOR = ["#f3f5f7", "#cfe2e0", "", "#cfe2e0"];
    var results = '<div class="divcontainer" style="overflow:auto;width: 1100px;"><table class="table table-bordered table-hover dataTable no-footer" role="grid" aria-describedby="example2_info">';
    results += '<thead><tr style="background:#cbc6dd;"><th scope="col"></th><th scope="col">Category Name</th><th scope="col">SubCategory Name</th><th scope="col">Product Name</th><th scope="col">Price</th><th scope="col">Vat</th><th scope="col">Status</th></tr></thead></tbody>';
    for (var i = 0; i < msg.length; i++) {
        var status = 'InActive';
        if (msg[i].flag == '1') {
            status = 'Active';
        }
        results += '<tr style="background-color:' + COLOR[l] + '"><td data-title="brandstatus"><button type="button" title="Click Here To Edit!" class="btn btn-info btn-outline btn-circle btn-lg m-r-5 editcls"  onclick="getbranchproduct(this)"><span class="glyphicon glyphicon-edit" style="top: 0px !important;"></span></button></td>';
        results += '<td scope="row" style="display:none" class="1" >' + msg[i].BranchName + '</td>';
        results += '<td scope="row" class="2" >' + msg[i].Categoryname + '</td>';
        results += '<td scope="row" class="3" >' + msg[i].SubCatName + '</td>';
        results += '<td scope="row" style="font-weight:600;" class="4" >' + msg[i].ProductName + '</td>';
        results += '<td scope="row" class="5" >' + msg[i].UnitPrice + '</td>';
        results += '<td scope="row" class="6" >' + msg[i].VatPercent + '</td>';
        results += '<td  style="display:none"  scope="row" class="7" >' + msg[i].DTarget + '</td>';
        results += '<td  style="display:none"  scope="row" class="8" >' + msg[i].WTarget + '</td>';
        results += '<td style="display:none"  scope="row" class="9" >' + msg[i].MTarget + '</td>';
        results += '<td scope="row" class="10" >' + status + '</td>';
        results += '<td style="display:none" scope="row" class="14" >' + msg[i].CatSno + '</td>';
        results += '<td style="display:none" scope="row" class="13" >' + msg[i].SubCatSno + '</td>';
        results += '<td style="display:none" scope="row" class="11" >' + msg[i].branchsno + '</td>';
        results += '<td style="display:none" scope="row" class="15" >' + msg[i].cgst + '</td>';
        results += '<td style="display:none" scope="row" class="16" >' + msg[i].sgst + '</td>';
        results += '<td style="display:none" scope="row" class="17" >' + msg[i].igst + '</td>';
        results += '<td style="display:none" scope="row" class="18" >' + msg[i].mrp + '</td>';
        results += '<td style="display:none" class="12">' + msg[i].pdtsno + '</td></tr>';
        l = l + 1;
        if (l == 4) {
            l = 0;
        }
    }
    results += '</table></div>';
    $("#div_Branch_Products").html(results);
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
function AscCallHandler(d, s, e) {
    $.ajax({
        url: 'DairyFleet.axd',
        data: d,
        type: 'GET',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        cache: true,
        success: s,
        error: e
    });
}
function CallHandlerUsingJson(d, s, e) {
    $.ajax({
        type: "GET",
        url: "DairyFleet.axd?json=",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(d),
        async: true,
        cache: true,
        success: s,
        error: e
    });
}

function callHandler_nojson_post(d, s, e) {
    $.ajax({
        url: 'DairyFleet.axd',
        type: "POST",
        // dataType: "json",
        contentType: false,
        processData: false,
        data: d,
        success: s,
        error: e
    });
}
function callHandler_post(d, s, e) {
    $.ajax({
        url: 'DairyFleet.axd',
        data: d,
        type: 'POST',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: true,
        cache: true,
        success: s,
        error: e
    });
}
