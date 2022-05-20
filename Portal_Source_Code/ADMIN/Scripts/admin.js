

function getE(name) {
    if (document.getElementById)
        var elem = document.getElementById(name);
    else if (document.all)
        var elem = document.all[name];
    else if (document.layers)
        var elem = document.layers[name];
    return elem;
}

function OpenWindow(query, w, h, scroll) {
    var l = (screen.width - w) / 2;
    var t = (screen.height - h) / 2;

    winprops = 'resizable=1, height=' + h + ',width=' + w + ',top=' + t + ',left=' + l + 'w';
    if (scroll) winprops += ',scrollbars=1';
    var f = window.open(query, "_blank", winprops);
}


function F2Help(event,query, w, h, scroll) {
    var f2Key = 113;
    if (event.which == f2Key) {
        var l = (screen.width - w) / 2;
        var t = (screen.height - h) / 2;

        winprops = 'resizable=1, height=' + h + ',width=' + w + ',top=' + t + ',left=' + l + 'w';
        if (scroll) winprops += ',scrollbars=1';
        var f = window.open(query, "_blank", winprops);
    }
}

//function SendToPopup() {
//    if (popup != null && !popup.closed) {
//        var lblFirstName = popup.document.getElementById('<%=hiddenValue.ClientID%>').value;
//        var lblLastName = popup.document.getElementById("lblLastName");
//        lblFirstName.innerHTML = document.getElementById("txtFirstName").value;
//        lblLastName.innerHTML = document.getElementById("txtLastName").value;
//        popup.focus();
//    } else {
//        alert("Popup has been closed.");
//    }
//}
