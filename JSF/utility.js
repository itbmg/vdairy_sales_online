function callHandler(d, s, e) {
    $.ajax({
        url: 'FleetManagementHandler.axd',
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
function CallHandlerUsingJson(d, s, e) {
    d = JSON.stringify(d);
    d = d.replace(/&/g, '\uFF06');
    d = d.replace(/#/g, '\uFF03');
    d = d.replace(/\+/g, '\uFF0B');
    d = d.replace(/\=/g, '\uFF1D');
    $.ajax({
        type: "GET",
        url: "FleetManagementHandler.axd?json=",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: d,
        async: true,
        cache: true,
        success: s,
        error: e
    });
}

//------------>Prevent Backspace<--------------------//
$(document).unbind('keydown').bind('keydown', function (event) {
    var doPrevent = false;
    if (event.keyCode === 8) {
        var d = event.srcElement || event.target;
        if ((d.tagName.toUpperCase() === 'INPUT' && (d.type.toUpperCase() === 'TEXT' || d.type.toUpperCase() === 'PASSWORD' || d.type.toUpperCase() === 'NUMBER'))
            || d.tagName.toUpperCase() === 'TEXTAREA') {
            doPrevent = d.readOnly || d.disabled;
        } else {
            doPrevent = true;
        }
    }
    if (doPrevent) {
        event.preventDefault();
    }
});