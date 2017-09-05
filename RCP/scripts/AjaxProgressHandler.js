function close_popup(pop_id)
 {
    var popup = pop_id;
    document.getElementById(popup).style.display = 'none';
}
function openSpecXY(pop_id, posX, posY)
 {
    var popup = pop_id;
    document.getElementById(popup).style.left = posX + 'px';
    document.getElementById(popup).style.top = posY + 'px';
    document.getElementById(popup).style.display = 'block';
}

function showhide(id)
 {
    if (document.getElementById) {
        obj = document.getElementById(id);
        if (obj.style.display == "none") {
            obj.style.display = "";
        }
        else {
            obj.style.display = "none";
        }
    }
}
// Detect if the browser is IE or not.
// If it is not IE, we assume that the browser is NS.
var IE = document.all ? true : false
// If NS -- that is, !IE -- then set up for mouse capture
if (!IE) document.captureEvents(Event.MOUSEMOVE)
// Set-up to use getMouseXY function onMouseMove
document.onclick = getMouseXY;
// Temporary variables to hold mouse x-y pos.s
var mousePosX = 0
var mousePosY = 0
var lastPopUp;
// Main function to retrieve mouse x-y pos.s

function getMouseXY(e) {
    try {
        if (IE) { // grab the x-y pos.s if browser is IE
            mousePosX = event.clientX + getScrollXY()[0]
            mousePosY = event.clientY + getScrollXY()[1]
        } else {  // grab the x-y pos.s if browser is NS
            mousePosX = e.pageX
            mousePosY = e.pageY
        }
        // catch possible negative values in NS4
        if (mousePosX < 0) { mousePosX = 0 }
        if (mousePosY < 0) { mousePosY = 0 }
    } catch (e) {
    }
}
function getScrollXY() {
    var scrOfX = 0, scrOfY = 0;
    if (typeof (window.pageYOffset) == 'number') {
        //Netscape compliant
        scrOfY = window.pageYOffset;
        scrOfX = window.pageXOffset;
    } else if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
        //DOM compliant
        scrOfY = document.body.scrollTop;
        scrOfX = document.body.scrollLeft;
    } else if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
        //IE6 standards compliant mode
        scrOfY = document.documentElement.scrollTop;
        scrOfX = document.documentElement.scrollLeft;
    }
    return [scrOfX, scrOfY];
}
function open_popupRelative(popUpDivID, divApproxWidth, divApproxHeight, closeLastPopUp) {
    //alert(getScrollXY()[1]);alert("open_popupRelative is" + popUpDivID);
    if (!divApproxWidth) {
        divApproxWidth = 0;
    }
    if (!divApproxHeight) {
        divApproxHeight = 0;
    }
    if (closeLastPopUp) {
        if (lastPopUp) {
            //alert(lastPopUp);
            close_popup(lastPopUp);
        }
    }
    var popup = popUpDivID;
    if (popup != "flexInner" && popup != "productlist") {
        lastPopUp = popup;
    }
    var divElement = document.getElementById(popup);
    divElement.style.left = mousePosX - divApproxWidth + 'px';
    divElement.style.top = mousePosY - divApproxHeight + 'px';
    //alert('x:'+mousePosX+'::y:'+mousePosY+'divX:'+divApproxWidth+'divY:'+divApproxHeight);
    divElement.style.display = 'block';
}
if (document.images) {
    img1 = new Image();
    img2 = new Image();
    img1.src = "4-0.gif";
    img2.src = "4-0.gif";
}
function cancelMyAjaxPostBack() {
    try {
        Sys.WebForms.PageRequestManager.getInstance().abortPostBack();
    } catch (e) { }
}
function pageLoad() {
    Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(showMyLoading);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(stopMyLoading);
}
function showMyLoading(sender, args) {
    open_popupRelative('ajax_req_process_div', 0, 0, 1);
}
function stopMyLoading(sender, args) {
    close_popup('ajax_req_process_div');
    if (args.get_error() != undefined) {
        args.set_errorHandled(true);
    }
}


