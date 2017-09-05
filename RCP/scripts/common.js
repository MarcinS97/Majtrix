//----- Badania wstęne -----
function setPrawaBadKol(link, pid, par) {
    showAjaxProgress();

    callAjax2(svcUrl, 'UpdateRightBadWstKol', { pid: pid, par: par, currValue: link.innerHTML },
        function(r) {
            hideAjaxProgress();
            if (r.d) {
                link.innerHTML = r.d;
            }
        }
    );
}


//----- Wydruk Portal -----
function clearAFUContents(sender, args) {
    var txts = sender._element.getElementsByTagName("input");
    for (var i = 0; i < txts.length; i++) {
        if (txts[i].type == "text") {
            txts[i].value = "";
            txts[i].style.backgroundColor = "white";
        }
    }
}




//----- Main menu -----
function nop() {
    //return false;
}

function prepareSubMenu(divsubmenu, pasubmenu, css) {
    $(divsubmenu).hover(
    function() {
        var pp = $(this);
        var mm = pp.children(pasubmenu);
        mm.hide(0);
        pp.addClass(css);
        mm.show(100);
    },
    function() {
        var pp = $(this);
        var mm = pp.children(pasubmenu);
        mm.hide(100, function() {
            pp.removeClass(css);
            mm.show(0);
        });
    });
}

//----- Grid -----
function gvRowSelect(cb, selId) {
    var hidSel = document.getElementById(selId);
    var $gv = $(cb).parents('table.GridView1');

    if (hidSel != null) {
        var id = cb.id.substring(1, cb.id.length);
        if (cb.checked) {
            if (id == 'All') {
                $gv.find('td .cbselect').prop('checked', true);
                $gv.find('td .cbselect').each(function () {
                    var id = $(this).attr('id');
                    id = id.substring(1, id.length);
                    hidSel.value += ',' + id;
                });
                hidSel.value += ',';
            }
            else {
                if (hidSel.value.indexOf(',' + id + ',') == -1)
                    if (hidSel.value == '')
                        hidSel.value = ',' + id + ',';
                    else
                        hidSel.value = hidSel.value + id + ',';
                }
        }
        else {
            if (id == 'All') {
                $gv.find('td .cbselect').prop('checked', false);
                hidSel.value = '';
            }
            else {
                hidSel.value = hidSel.value.replace(',' + id + ',', ',');
                if (hidSel.value == ',') 
                    hidSel.value = '';
            }
        }
        console.log(hidSel.value);
    }
    //alert(cb.id);
}

function gvSelectAll(cb) {
    var c = cb.checked;
    var id = cb.id;
    var selId = id.replace("SelectAll", "Selected");
    var hidSel = document.getElementById(selId);
    if (hidSel != null)
        hidSel.value = null;
    $("input.cbselect").prop("checked", !c);
    $("input.cbselect").click();



    //alert(hidSel.value);
}

function gvFind(cnt) {
    if (cnt.id.endsWith('gvReport'))
        return cnt.id;
    else 
        if (cnt.parentElement != null)
            return gvFind(cnt.parentElement);
        else
            return null;
}

function gvFindByID(cnt, gvID) {
    if (cnt.id.endsWith(gvID))
        return cnt.id;
    else 
        if (cnt.parentElement != null)
            return gvFindByID(cnt.parentElement, gvID);
        else
            return null;
}

function gvCommand(cnt, cmd, par) {
    var gv = gvFind(cnt);
    if (gv != null)
    {
        var p = document.getElementById(gv + 'CmdPar');
        if (p != null) {
            p.value = cmd + '|' + par;
            doClick(gv + 'Cmd');
        }
    }
}

function gvCommandByID(cnt, gvID, cmd, par) {
    var gv = gvFindByID(cnt, gvID);
    if (gv != null)
    {
        var p = document.getElementById(gv + 'CmdPar');
        if (p != null) {
            p.value = cmd + '|' + par;
            doClick(gv + 'Cmd');
        }
    }
}

function getParentsDebug(cnt) {
    var parents = $(cnt).parents("*");
    var selector = "";
    for (var i = parents.length - 1; i >= 0; i--) {
        selector += i + '|' + parents[i].id + "\n";
    }
    alert(selector);
}

//----- -----
function getGridID(elem) {
    var depth = 0;
    return getGridIDfunc(elem, depth);
}

function getGridIDfunc(elem, depth) {
    if (depth > 10)
        return null;
    else if (elem.id != null && elem.id != '')
        return elem.id;
    else {
        depth++;
        return getGridIDfunc(elem.parentElement, depth);
    }
}
//----- przypisania rcp -----
function blinkPoints() {
    var dt = 200;
    var tt = 6;
    window.setInterval(function() {
        //$('span.point_blinking').fadeOut(dt).delay(dt).fadeIn(dt);

        $('span.point_blinking').animate({
            backgroundColor: '#C0EB92',
            borderColor: '#8FDB3C'
        }, dt).delay(dt).animate({
            backgroundColor: '#8FDB3C',
            borderColor: '#6DB421'
        }, dt);
    }, dt * tt);
}


function showInfo(lb2, lbInfo) {
    var t11 = 5000;
    var t12 = 3000;
    $('#' + lb2).stop(true, true).hide().delay(t11).fadeIn(t12);
    $('#' + lbInfo).stop(true, true).show().delay(t11).fadeOut(t12);
}

/*----- ajax Editor -----x*/
function ajaxEditorInsertText(edId, text) {
    var cnt = top.document.getElementById(edId);    
    var editorControl = cnt.control;
    var editPanel = editorControl.get_editPanel();
    if (editPanel.get_activeMode() == 0) {          
        var designPanel = editPanel.get_activePanel();
        designPanel._saveContent();                 //Undo
        designPanel.insertHTML(text);               
        setTimeout(function() { designPanel.onContentChanged(); editPanel.updateToolbar(); }, 0); 
        designPanel.focusEditor();
    }
}

function ajaxEditorInsertTextBox(edId, txtId) {
    var text = document.getElementById(txtId).value;
    ajaxEditorInsertText(edId, text);    
}

function ajaxEditorImageJust(just) {
    var f = $(".cntArticleEdit iframe");
    if (f.length > 0)
        switch (just) {
            case -1:
                f.contents().find(".img_right").switchClass("img_right", "img_left");
                f.contents().find(".img_center").switchClass("img_center", "img_left");
                break;
            case 0:
                f.contents().find(".img_left").switchClass("img_left", "img_center");
                f.contents().find(".img_right").switchClass("img_right", "img_center");
                break;
            case 1:
                f.contents().find(".img_left").switchClass("img_left", "img_right");
                f.contents().find(".img_center").switchClass("img_center", "img_right");
                break;
        }
}

function ajaxEditorImageSize(d) {
    var f = $(".cntArticleEdit iframe");
    if (f.length > 0)
        switch (d) {
            case -1:
                f.contents().find(".img30").switchClass("img30", "img20");
                f.contents().find(".img40").switchClass("img40", "img30");
                f.contents().find(".img50").switchClass("img50", "img40");
                f.contents().find(".img60").switchClass("img60", "img50");
                f.contents().find(".img70").switchClass("img70", "img60");
                f.contents().find(".img80").switchClass("img80", "img70");
                f.contents().find(".img90").switchClass("img90", "img80");
                f.contents().find(".img100").switchClass("img100", "img90");
                break;
            case 1:
                f.contents().find(".img90").switchClass("img90", "img100");
                f.contents().find(".img80").switchClass("img80", "img90");
                f.contents().find(".img70").switchClass("img70", "img80");
                f.contents().find(".img60").switchClass("img60", "img70");
                f.contents().find(".img50").switchClass("img50", "img60");
                f.contents().find(".img40").switchClass("img40", "img50");
                f.contents().find(".img30").switchClass("img30", "img40");
                f.contents().find(".img20").switchClass("img20", "img30");
                break;
    }
}

/*----- fileUpload -----*/
var btUpdateId = null;

function fileUploadComplete() {
    if (btUpdateId != null)
        doClick(btUpdateId);
}

/*----- ddl -----*/
function ddlUpdateText(ddlId, tbId) {
    var ddl = document.getElementById(ddlId);
    var tb = document.getElementById(tbId);
    if (ddl != null && tb != null) {
        tb.value = ddl.value;
        ddl.selectedIndex = 0;
        tb.focus();
    }
}

function ddlKlasNadgChange(ddlId, tbId) {
    alert(1);
    return false;
 
    var ddl = document.getElementById(ddlId);
    var tb = document.getElementById(tbId);
    if (ddl != null && tb != null) {
        var par = ddl.value.split('|');
        //var par = ddl.options[ddl.selectedIndex].value.split('|');
        if (par.length >= 5) {
            if ($.trim(tb.value) == '') {
                tb.value = par[4];
                tb.focus();
            }
        }
    }
}

/*----- acordeon menu -----*/
function acordeonSelect(hidCmd, btClick, cmd) {
    var hid = document.getElementById(hidCmd);
    if (hid != null) hid.value = cmd;
    doClick(btClick);
}

/*----- modal popup -----*/
var popType = null;  //1 msg, 2 error, 3 question
var popMessage = null;
var popBtOk = null;
var popBtCancel = null;

function popupEventHandler() {
    if (popMessage != null) {
        switch (popType) {
            case 3:
                confirmClick(popMessage, popBtOk, popBtCancel);
                break;
            default:
                alert(popMessage);
                break;
        }
        popMessage = null;
        popType = null;
    }
}

function popupShowMessage(typ, msg) {
    popType = typ;
    popMessage = msg;
}

function popupShowConfirm(question, btOk, btCancel) {
    popType = 3;
    popMessage = question;
    popBtOk = btOk;
    popBtCancel = btCancel;
}

/*----- wnioski urlopowe -----*/
var wnPrevD1 = '';
var wnPrevD2 = '';
var wnPrevIl = '';
var wnTimer1 = null;
//var wnTimer2 = null;

function checkUrlopDni2(dataOd, dataDo, ilDni, godzZm, ilGodzTb, ilGodz, svcUrl) {
    if (wnTimer1 == null) {
        wnTimer1 = window.setInterval(function () {
            var d1 = document.getElementById(dataOd);
            var d2 = document.getElementById(dataDo);
            var il = document.getElementById(ilDni);
            var gg = document.getElementById(godzZm);
            var ilGTb = document.getElementById(ilGodzTb);
            var ilG = document.getElementById(ilGodz);
            if (d1 != null) {
                var b1 = d1.value != wnPrevD1;
                var b2 = d2.value != wnPrevD2;
                var b3 = il.value != wnPrevIl;
                var sel = 0;
                if (b1 || b2 || b3) {
                    if (b1) {
                        if (d2.disabled)
                            sel = 11;
                        else
                            sel = 1;
                        wnPrevD1 = d1.value;
                    }
                    if (b2) {
                        sel = 2;
                        wnPrevD2 = d2.value;
                    }
                    if (b3) {
                        sel = 3;
                        wnPrevIl = il.value;
                        var g = godzRoundStr(il.value * gg.value);
                        if (ilG != null)
                            ilG.innerHTML = g;
                        if (ilGTb != null)
                            ilGTb.value = g;
                    }
                    if (d1 != null)
                        callAjax(svcUrl, 'IloscDniPrac2', { par: [d1.value, d2.value, il.value, gg.value, sel] },
                            function (r) {
                                var ril = document.getElementById(ilDni);
                                switch (r.d.field) {
                                    case 1:
                                        var rd1 = document.getElementById(dataOd);
                                        if (rd1 != null) {
                                            rd1.value = r.d.value;
                                            wnPrevD1 = r.d.value;
                                        }
                                        break;
                                    case 2:
                                        var rd2 = document.getElementById(dataDo);
                                        if (rd2 != null) {
                                            rd2.value = r.d.value;
                                            wnPrevD2 = r.d.value;
                                        }
                                        break;
                                    case 3:
                                        if (ril != null) {
                                            ril.value = r.d.value;
                                            wnPrevIl = r.d.value;
                                        }
                                        break;
                                }
                                switch (r.d.field) {
                                    case 1:
                                    case 2:
                                    case 3:
                                        var rgg = document.getElementById(godzZm);
                                        var rilG = document.getElementById(ilGodz);
                                        var rilGTb = document.getElementById(ilGodzTb);
                                        var rg = godzRoundStr(ril.value * rgg.value);
                                        if (rilG != null) rilG.innerHTML = rg;
                                        if (rilGTb != null) rilGTb.value = rg;
                                        break;
                                }
                            });
                    wnDebug(sel);
                }
                else wnDebug('.');
            }
            else wnDebug('-');
        }, 250);
        wnDebug('o');
    }
    else wnDebug('x');
}

/*
function checkUrlopDni2(dataOd, dataDo, ilDni, godzZm, ilGodzTb, ilGodz, svcUrl) {
    if (wnTimer1 == null) {
        wnTimer1 = window.setInterval(function () {
            var d1 = document.getElementById(dataOd);
            var d2 = document.getElementById(dataDo);
            var il = document.getElementById(ilDni);
            var gg = document.getElementById(godzZm);
            var ilGTb = document.getElementById(ilGodzTb);
            var ilG = document.getElementById(ilGodz);
            if (d1 != null) {
                var b1 = d1.value != wnPrevD1;
                var b2 = d2.value != wnPrevD2;
                var b3 = il.value != wnPrevIl;
                var sel = 0;
                if (b1 || b2 || b3) {
                    if (b1) {
                        if (d2.disabled)
                            sel = 11;
                        else
                            sel = 1;
                        wnPrevD1 = d1.value;
                    }
                    if (b2) {
                        sel = 2;
                        wnPrevD2 = d2.value;
                    }
                    if (b3) {
                        sel = 3;
                        wnPrevIl = il.value;
                        var g = godzRoundStr(il.value * gg.value);
                        if (ilG != null)
                            ilG.innerHTML = g;
                        if (ilGTb != null)
                            ilGTb.value = g;
                    }
                    if (d1 != null)
                        callAjax(svcUrl, 'IloscDniPrac2', { par: [d1.value, d2.value, il.value, gg.value, sel] },
                            function (r) {
                                var ilG = document.getElementById(ilGodz);
                                switch (r.d.field) {
                                    case 1:
                                        var rd1 = document.getElementById(dataOd);
                                        if (rd1 != null) {
                                            rd1.value = r.d.value;
                                            wnPrevD1 = r.d.value;
                                        }
                                        break;
                                    case 2:
                                        var rd2 = document.getElementById(dataDo);
                                        if (rd2 != null) {
                                            rd2.value = r.d.value;
                                            wnPrevD2 = r.d.value;
                                        }
                                        break;
                                    case 3:
                                        var ril = document.getElementById(ilDni);
                                        var rilGTb = document.getElementById(ilGodzTb);
                                        var rilG = document.getElementById(ilGodz);
                                        var rgg = document.getElementById(godzZm);
                                        if (ril != null) {
                                            ril.value = r.d.value;
                                            wnPrevIl = r.d.value;
                                            var rg = godzRoundStr(ril.value * rgg.value);
                                            if (rilG != null)
                                                rilG.innerHTML = rg;
                                            if (rilGTb != null)
                                                rilGTb.value = rg;
                                        }
                                        break;
                                }
                            });
                    wnDebug(sel);
                }
                else wnDebug('.');
            }
            else wnDebug('-');
        }, 250);
        wnDebug('o');
    }
    else wnDebug('x');
}
*/

/*
function checkUrlopDni2(dataOd, dataDo, ilDni, godzZm, ilGodzTb, ilGodz) {
    if (wnTimer1 == null) {
        wnTimer1 = window.setInterval(function() {
            var d1 = document.getElementById(dataOd);
            var d2 = document.getElementById(dataDo);
            var il = document.getElementById(ilDni);
            var gg = document.getElementById(godzZm);
            var ilGTb = document.getElementById(ilGodzTb);
            var ilG = document.getElementById(ilGodz);

            if (d1 != null) {
                var b1 = d1.value != wnPrevD1;
                var b2 = d2.value != wnPrevD2;
                var b3 = il.value != wnPrevIl;
                var sel = 0;
                if (b1 || b2 || b3) {
                    if (wnTimer2 != null)
                        window.clearTimeout(wnTimer2);
                    if (b1) {
                        sel = 1;
                        wnPrevD1 = d1.value;
                    }
                    if (b2) {
                        sel = 2;
                        wnPrevD2 = d2.value;
                    }
                    if (b3) {
                        sel = 3;
                        wnPrevIl = il.value;
                        var g = godzRoundStr(il.value * gg.value);
                        ilG.innerHTML = g;
                        ilGTb.value = g;
                    }
                    wnTimer2 = window.setTimeout(function() {  // po timeoutach moze juz nie byc kontrolek
                        var td1 = document.getElementById(dataOd);
                        var td2 = document.getElementById(dataDo);
                        var il = document.getElementById(ilDni);
                        var gg = document.getElementById(godzZm);
                        if (td1 != null)
                            callAjax(, 'IloscDniPrac2', { par: [td1.value, td2.value, il.value, gg.value, sel] },
                                function(r) {
                                    var ilG = document.getElementById(ilGodz);
                                    switch (r.d.field) {
                                        case 1:
                                            var rd1 = document.getElementById(dataOd);
                                            if (rd1 != null) {
                                                rd1.value = r.d.value;
                                                wnPrevD1 = r.d.value;
                                            }
                                            break;
                                        case 2:
                                            var rd2 = document.getElementById(dataDo);
                                            if (rd2 != null) {
                                                rd2.value = r.d.value;
                                                wnPrevD2 = r.d.value;
                                            }
                                            break;
                                        case 3:
                                            var ril = document.getElementById(ilDni);
                                            var rilGTb = document.getElementById(ilGodzTb);
                                            var rilG = document.getElementById(ilGodz);
                                            var rgg = document.getElementById(godzZm);
                                            if (ril != null) {
                                                ril.value = r.d.value;
                                                wnPrevIl = r.d.value;
                                                var rg = godzRoundStr(ril.value * rgg.value);
                                                rilG.innerHTML = rg;
                                                rilGTb.value = rg;
                                            }
                                            break;
                                    }
                                });
                    }, 500);
                    wnDebug(sel);
                }
                else wnDebug('.');
            }
            else wnDebug('-');
        }, 250);
        wnDebug('o');
    }
    else wnDebug('x');
}
*/


function checkUrlopDni2Stop() {
    if (wnTimer1 != null)
    {
        window.clearInterval(wnTimer1);
        wnTimer1 = null;
        wnDebug('|');
    }
}

function godzRoundStr(d, prec) {
    if (d.toFixed) //if browser supports toFixed() method IE5.5+
        return d.toFixed(prec).toString();
    else
        return d.toString();
}



function checkUrlopDni(dataOd, dataDo, ilDni, bt, focused, svcUrl) {
    if (wnTimer1 = null) {
        wnTimer1 = window.setInterval(function() {
            var d1 = document.getElementById(dataOd);
            var d2 = document.getElementById(dataDo);
            var il = document.getElementById(ilDni);
            var ff = document.getElementById(focused);
            if (d1 != null && d2 != null && il != null && ff != null) {
                var b1 = d1.value != wnPrevD1;
                var b2 = d2.value != wnPrevD2;
                var b3 = il.value != wnPrevIl;
                var freeze = wnPrevD1 == 'x';
                if (b1 || b2 || b3) {
                    if (wnTimer2 != null)
                        window.clearTimeout(wnTimer2);
                    if (b1) {
                        ff.value = '1';
                        wnPrevD1 = d1.value;
                    }
                    if (b2) {
                        ff.value = '2';
                        wnPrevD2 = d2.value;
                    }
                    if (b3) {
                        ff.value = '3';
                        wnPrevIl = il.value;
                    }
                    if (!freeze) {
                        if (b1 && d2.value != '' || b2 && d1.value != '')
                            wnTimer2 = window.setTimeout(function() {  // po timeoutach moze juz nie byc kontrolek
                                var td1 = document.getElementById(dataOd);
                                var td2 = document.getElementById(dataDo);
                                if (td1 != null && td2 != null)
                                    callAjax(svcUrl, 'IloscDniPrac', { dataOd: td1.value, dataDo: td2.value },
                                        function(r) {
                                            var til = document.getElementById(ilDni);
                                            if (til != null) {
                                                til.value = r.d;
                                            }
                                        });
                            }, 500);
                        else if (b1 && d2.value == '' || b3 && d1.value != '')
                            wnTimer2 = window.setTimeout(function() {  // po timeoutach moze juz nie byc kontrolek
                                var td1 = document.getElementById(dataOd);
                                var til = document.getElementById(ilDni);
                                if (td1 != null && til != null)
                                    callAjax(svcUrl, 'IloscDniPracDataDo', { dataOd: td1.value, ilDni: til.value },
                                        function(r) {
                                            var td2 = document.getElementById(dataDo);
                                            if (td2 != null) {
                                                td2.value = r.d;
                                            }
                                        });
                            }, 500);
                        else if (b2 && d1.value == '' || b3 && d1.value == '')
                            wnTimer2 = window.setTimeout(function() {  // po timeoutach moze juz nie byc kontrolek
                                var td2 = document.getElementById(dataDo);
                                var til = document.getElementById(ilDni);
                                if (td2 != null && til != null)
                                    callAjax(svcUrl, 'IloscDniPracDataOd', { dataDo: td2.value, ilDni: til.value },
                                function(r) {
                                    var td1 = document.getElementById(dataOd);
                                    if (td1 != null) {
                                        td1.value = r.d;
                                    }
                                });
                            }, 500);
                        wnDebug(ff.value);
                    }
                    else wnDebug('f');
                }
                else wnDebug('.');
            }
            else wnDebug('-');
        }, 250);
        wnDebug('o');
    }
    else wnDebug('x');
}




/*
function checkUrlopDni(dataOd, dataDo, ilDni, bt, focused) {
    if (!wnStarted) {
        wnStarted = true;
        window.setInterval(function() {
            var d1 = document.getElementById(dataOd);
            var d2 = document.getElementById(dataDo);
            var il = document.getElementById(ilDni);
            var ff = document.getElementById(focused);
            if (d1 != null && d2 != null && il != null && ff != null) {
                var b1 = d1.value != wnPrevD1;
                var b2 = d2.value != wnPrevD2;
                var b3 = il.value != wnPrevIl;
                var freeze = wnPrevD1 == 'x';
                if (b1 || b2 || b3) {
                    if (wnTimer != null)
                        window.clearTimeout(wnTimer);
                    if (b1) {
                        ff.value = '1';
                        wnPrevD1 = d1.value;
                    }
                    if (b2) {
                        ff.value = '2';
                        wnPrevD2 = d2.value;
                    }
                    if (b3) {
                        ff.value = '3';
                        wnPrevIl = il.value;
                    }
                    if (!freeze) {
                        wnTimer = window.setTimeout(function() {
                            doClick(bt);
                        }, 500);
                        wnDebug(ff.value);
                    }
                    else wnDebug('f');
                }
                else wnDebug('.');
            }
            else wnDebug('-');
        }, 250);
        wnDebug('o');
    }
    else wnDebug('x');
}
*/

function checkUrlopDniFreeze() {
    wnPrevD1 = 'x';
}

function wnDebug(s) {
    return;

    var x = $('#debug').html();
    if (x == null || 
        x.length > 100 && (
            endsWith(x, '....................') ||
            endsWith(x, '--------------------')
        )) x = s;
    else x += s;
    $('#debug').html(x);
}

//------------------
/*
function getRootUrl() {
    var loc = window.location;
    var pathName = loc.pathname.substring(0, loc.pathname.lastIndexOf('/') + 1);
    return loc.href.substring(0, loc.href.length - ((loc.pathname + loc.search + loc.hash).length - pathName.length));
}

function getAppPath() {
    var pathArray = window.location.pathname.split('/');
    var appPath = "/";
    for (var i = 1; i < pathArray.length - 1; i++) {
        appPath += pathArray[i] + "/";
    }
    return appPath;
}
*/

function wnGetAjax_() {
    // Validating input
    //alert(1);

    $.ajax({
        type: 'POST',
        url: '/test.asmx/IlDni',
        data: JSON.stringify({ dOd: '2013-01-01', dDo: '2013-01-31' }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function(r) {
            alert(r.d);
        },
        error: function(e) {
            alert('error ' + e.status + " " + e.statusText + " " + e.responseText);
        }
    });
}

function callAjax(svcUrl, func, par, fsuccess) {
    //if (controller == null) controller = 'main'
    $.ajax({
        type: 'POST',
        url: svcUrl + '/' + func,
        //url: baseUrl + controller + '.asmx/' + func,
        //url: '@Url.Action("' + func + '", "' + controller + '.asmx")',        

        //data: JSON.stringify({ p1: p1, p2: p2, p3: p3, p4: p4, p5: p5 }),         
        data: JSON.stringify(par),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: fsuccess,
        error: function(e) {
            alert("Błąd podczas komunikacji z serwerem:\nStatus: " + e.status + "\n" + e.statusText + "\n\nOpis:\n" + e.responseText);
        }
    });
}

function callAjax2(svcUrl, func, par, fsuccess) {
    //alert("callAjax2: " + svcUrl);    
    showAjaxProgress();
    $.ajax({
        type: 'POST',
        url: svcUrl + '/' + func,
        data: JSON.stringify(par),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: fsuccess,
        error: function(e) {
            hideAjaxProgress();
            alert("Błąd podczas komunikacji z serwerem:\nStatus: " + e.status + "\n" + e.statusText + "\n\nOpis:\n" + e.responseText);
        }
    });
}



 /*
         //data: {},
         //data: JSON.stringify({ p: { dOd: '2013-01-01', dDo: '2013-01-31'} }),
         ({ person:{ firstName: "Denny", lastName: "Cherian", department: "Microsoft PSS", address: { addressline1: "Microsoft India GTSC", addressline2: "PSS - DSI", city: "Bangalore", state: "Karnataka", country: "India", pin: "560028" }, technologies: ["IIS", "ASP.NET", "JavaScript", "AJAX"] }}),
         contentType: "application/json; charset=utf-8",
         dataType: "json",

        success: function(r) {
            alert(r.d);
        },
 */

/*----- Podział Ludzi -----*/
var svcUrl = '/main.asmx';

function setServiceUrl(url) {
    svcUrl = url;
    //alert(url);
}

function setPrawaCC_1(link, pid) {
    //alert(window.location.pathname + ' ' + getRootUrl() + ' ' + window.location.hostname);
    //alert(svcUrl);
    var p = par.split(' ');
    if (p.length == 3) {
        showAjaxProgress();
        //var svcUrl = getRootUrl() + '/main.asmx';

        callAjax2(svcUrl, 'UpdateRightCC', { typ: p[0], pid: p[1], par: p[2], currvalue: link.innerHTML },
            function(r) {
                hideAjaxProgress();
                if (r.d) {
                    //alert(r.d);
                    link.innerHTML = r.d;
                    /*
                    if (cb.checked)
                    $(cb.parentNode).addClass('checked');
                    else
                    $(cb.parentNode).removeClass('checked');
                    */
                } else {
                    //alert('error' + r.d);
                    //cb.checked = chk;   // restore
                    ////alert('Błąd podczas zapisu do bazy');
                }
            }
        );
    }
}

function setPrawaCC(link, typ, pid, par) {

    //alert(window.location.pathname + ' ' + getRootUrl() + ' ' + window.location.hostname);
    //alert(svcUrl);

    showAjaxProgress();
    //var svcUrl = getRootUrl() + '/main.asmx';

    callAjax2(svcUrl, 'UpdateRightCC', { typ: typ, pid: pid, par: par, currvalue: link.innerHTML },
        function (r) {
            hideAjaxProgress();
            if (r.d) {
                //alert(r.d);
                link.innerHTML = r.d;
                /*
                if (cb.checked)
                $(cb.parentNode).addClass('checked');
                else
                $(cb.parentNode).removeClass('checked');
                */
            } else {
                //alert('error' + r.d);
                //cb.checked = chk;   // restore
                ////alert('Błąd podczas zapisu do bazy');
            }
        }
    );
}

function ajaxHarmonogram(link, gvID, typ, pid, par, upd) {

    //alert(window.location.pathname + ' ' + getRootUrl() + ' ' + window.location.hostname);
    //alert(svcUrl);

    showAjaxProgress();
    //var svcUrl = getRootUrl() + '/main.asmx';

    callAjax2(svcUrl, 'ajaxHarmonogram', { typ: typ, pid: pid, par: par, currvalue: link.innerHTML },
        function (r) {
            hideAjaxProgress();
            if (r.d) {
                //alert(r.d);
                if ((upd & 0x1) != 0)
                    link.innerHTML = r.d;
                if ((upd & 0x2) != 0) {
                    gvCommandByID(link, gvID, 'ajax', typ + '|' + pid + '|' + par + '|' + r.d);
                }
                /*
                if (cb.checked)
                $(cb.parentNode).addClass('checked');
                else
                $(cb.parentNode).removeClass('checked');
                */
            } else {
                //alert('error' + r.d);
                //cb.checked = chk;   // restore
                alert('Błąd podczas zapisu do bazy');
            }
        }
    );
}

function setPrawaCCAll(link, typ, pid, par) {
    if (confirm('Potwierdź ustawienie/wyzerowanie dostępu do wszystkich cc.')) {
        showAjaxProgress();
        callAjax2(svcUrl, 'UpdateRightCC', { typ: typ, pid: pid, par: par, currvalue: link.innerHTML },
            function(r) {
                hideAjaxProgress();
                if (r.d) {
                    //alert(r.d);
                    link.innerHTML = r.d;
                    var id = getGridID(link);
                    if (id != null) {
                        //alert(id);
                        doClick(id + 'Cmd');
                    }
                } 
            }
        );
    }
}

function setPrawaIPO2(link, bind, typ, pid, par, rola) {
    showAjaxProgress();
    callAjax2(svcUrl, 'UpdateRightIPO', { typ: typ, pid: pid, par: par, rola: rola, currvalue: link.innerHTML, aggCC: bind },
        function(r) {
            hideAjaxProgress();
            if (r.d) {
                link.innerHTML = r.d;
                if (bind) {
                    var id = getGridID(link);
                    if (id != null) {
                        //alert(id);
                        doClick(id + 'Cmd');
                    }
                }
            }
        }
    );
}




function xx_setPrawaCC2(link, typ, pid, par, bt) {

    //alert(window.location.pathname + ' ' + getRootUrl() + ' ' + window.location.hostname);
    //alert(svcUrl);

    showAjaxProgress();
    callAjax2(svcUrl, 'UpdateRightCC', { typ: typ, pid: pid, par: par, currvalue: link.innerHTML },
        function(r) {
            hideAjaxProgress();
            if (r.d) {
                //alert(r.d);
                link.innerHTML = r.d;
                if (bt != null) doClick(bt);
                /*
                if (cb.checked)
                $(cb.parentNode).addClass('checked');
                else
                $(cb.parentNode).removeClass('checked');
                */
            } else {
                //alert('error' + r.d);
                //cb.checked = chk;   // restore
                ////alert('Błąd podczas zapisu do bazy');
            }
        }
    );
}


/*----- cntSplityWsp -----*/
var tbSplityWspArray = new Object; //nie ma asoc., dodaje właściwości obiektu

function cntSplityWspSum(tbClass, sumId) {
    window.setInterval(function() {
        var cnt = 0;
        $('.' + tbClass).each(function() {
            cnt++
        });
        var sum = 0;
        $('.' + tbClass).each(function() {
            var prev = tbSplityWspArray[this.id];
            var curr = $(this).val();   // spr curr != undefined
            if (prev == undefined) prev = '';
            if (curr != prev) {
                var v = Number(curr);
                if (curr.indexOf(',') != -1) {
                    curr = curr.replace(',', '.');
                    $(this).val(curr);
                    this.focus();
                    this.setSelectionRange(curr.length, curr.length);
                }
//              if (prev == '' && curr.indexOf('.') == -1 && (cnt == 1 && v > 1 || cnt > 1 && v > 0)) {
//                  curr = '0.' + v;
//                  $(this).val(curr);
//              }
                if (prev == '' && curr.indexOf('.') == -1)
                    if (cnt == 1 && v > 1 || cnt > 1 && v > 0) {
                    curr = '0.' + v;
                    $(this).val(curr);
                }
                else if (v == 0) {
                    curr = '0.';
                    $(this).val(curr);
                }

                tbSplityWspArray[this.id] = curr;
            }
            curr = $(this).val();
            sum += Number(curr);
        });
        //$('#' + sumId).val(sum);

        if (isNaN(sum))
            sum = '?';
        else
            sum = Math.round(sum * 10000) / 10000;
        $('.' + sumId).html(sum);
    }, 250);
}

/*----- cntSplityWsp PL -----*/
var tbSplityWspPLArray = new Object; //nie ma asoc., dodaje właściwości obiektu
var splityWspPLTimer = null;

function cntSplityWspSumPLsum(tbClass, sumId) {
    var cnt = 0;
    $('.' + tbClass).each(function() {
        cnt++;
    });
    var sum = 0;
    $('.' + tbClass).each(function() {
        var prev = tbSplityWspPLArray[this.id];
        //var curr = $(this).val();   // spr curr != undefined

        var curr;
        var el = $(this);
        if (el.is('input'))
            curr = el.val();
        else
            curr = el.text();

        //        if (curr == null)
        //            curr = $(this).text();

        if (prev == undefined) prev = '';
        if (curr != prev) {
            var v = Number(curr);
            if (curr.indexOf(',') != -1) {
                curr = curr.replace(',', '.');
                $(this).val(curr);
                this.focus();
                this.setSelectionRange(curr.length, curr.length);
            }
            if ((prev == '' || v > 1) && curr.indexOf('.') == -1)
                if (cnt == 1 && v > 1 || cnt > 1 && v > 0) {
                    curr = '0.' + v;
                    $(this).val(curr);
                }
                else if (v == 0) {
                    curr = '0.0';
                    $(this).val(curr);
                }
            tbSplityWspPLArray[this.id] = curr;
        }

        //curr = $(this).val();
        sum += Number(curr);
    });
    //$('#' + sumId).val(sum);

    if (isNaN(sum))
        sum = '?';
    else
        sum = Math.round(sum * 10000) / 10000;
    $('.' + sumId).html(sum);
}

function cntSplityWspSumPL(tbClass, sumId) {
    splityWspPLTimer = window.setInterval(function() {
        cntSplityWspSumPLsum(tbClass, sumId)
    }, 250);
}

function cntSplityWspSumPLStop() {
    if (splityWspPLTimer != null) {
        window.clearInterval(splityWspPLTimer);
        splityWspPLTimer = null;
    }
}

/*
function sumPunkty(textbox, allId, sumId, restId) {
    if (textbox.value != undefined) {
        var v = parseInt(textbox.value, 10);
        if (isNaN(v)) textbox.value = '';
        else textbox.value = v;
    }

    var sum = 0;
    $(".sumpunkty").each(function() {
        sum += Number($(this).val());
    });
    var pt = $('#' + allId).val();
    //$('#' + sumId).text(sum);
    //$('#' + restId).text(pt - sum);
    $('#' + sumId).val(sum);
    $('#' + restId).val(pt - sum);
}

function clearPunkty(textbox, allId, sumId, restId) {
    $(".sumpunkty").each(function() {
        $(this).val('');
    });
    var pt = $('#' + allId).val();
    $('#' + sumId).val('0');
    $('#' + restId).val(pt);
}
*/


/*----- scroll div with animation -----*/
function scroll(bt, scrollerId, contentId, pcent) {
    //alert(scrollerId + ' ' + contentId + ' ' + pcent);

    var s1 = document.getElementById(scrollerId);
    var s2 = document.getElementById(contentId);
    if (s1 != null && s2 != null) {
        var p = 0;
        var h1 = s1.offsetHeight;
        var h2 = s2.offsetHeight;
        if (h2 > h1) {
            var d = Math.floor(h1 * pcent / 100);
            p = s1.scrollTop;

            //alert('h1:' + h1 + ' h2:' + h2 + ' d:' + d + ' p:' + p);

            if (pcent > 0) {
                p += d;
                if (p > h2 - h1) p = h2 - h1;
                //s1.scrollTop = p;
            }
            else {
                p += d;
                if (p < 0) p = 0;
                //s1.scrollTop = p;
            }
        }
        $(s1).animate({
            scrollTop: p
        });
    }
}
/*----- zachowywanie pozycji scrollera przy postbacku -----*/
/* div w masterpage */
function debug(c, info) {
    $('#debug').html($('#debug').html() + '<br />' + c + ' ' + info);
}

var scrollControls;
var scrollPos;

function scrollRegister(cntId) {
    var addRq = scrollControls == undefined;
    if (addRq) {
        scrollControls = new Array();
        scrollPos = new Array();
        scrollControls[0] = cntId;
        scrollPos[0] = 0;
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(scrollOnBeginRq);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(scrollOnEndRq);

        //debug('Reg[0] =', cntId);
        //debug('--->', 'add_rq');
    } else {
        //var idx = scrollControls.indexOf(cntId);  nie ma na IE8
        var idx = $.inArray(cntId, scrollControls);
        if (idx < 0) {
            idx = scrollControls.length;
            scrollControls[idx] = cntId;
            scrollPos[idx] = 0;
            //debug('Reg[' + idx + '] =', cntId);
        } else {
            //debug('Reg[' + idx + '] =', cntId + ', exists');
        }
    }
}

function scrollOnBeginRq(sender, args) {
    //debug('BeginRq',scrollControls.length);

    for (var idx = scrollControls.length - 1; idx >= 0; idx--) {
        var cnt = document.getElementById(scrollControls[idx]);
        if (cnt != null) {
            scrollPos[idx] = cnt.scrollTop;
            //debug('- get [' + idx + ']=', cnt.scrollTop);
        }
    }
}

function scrollOnEndRq(sender, args) {
    //debug('EndRq',scrollControls.length);

    for (var idx = scrollControls.length - 1; idx >= 0; idx--) {
        var cnt = document.getElementById(scrollControls[idx]);
        if (cnt != null) {
            var p = scrollPos[idx];
            if (p != -1)
                cnt.scrollTop = scrollPos[idx];
            //debug('- set [' + idx + ']=', scrollPos[idx]);    // + ' -> ' + scrollControls[idx]);         
        }
    }
}

//----- search autoinc -----------
var tbSearchArray = new Object; //nie ma asoc., dodaje właściwości obiektu
var searchTimer = null;         // na razie 1 pole wyszukiwania, pozniej dac petelke po tbSerqachArray i dla kazdego wywolac - jak nie znajdzie edita to usunac z tabeli
var tbSearchId = null;
var btSearchClickId = null;

function startSearch(tb, bt) {
    //alert(tb);
    //wnDebug(tb);

    tbSearchId = tb;
    btSearchClickId = bt;
    if (searchTimer == null)
        searchTimer = window.setInterval(function() {
            var prev = tbSearchArray[tbSearchId];
            var curr = $.trim($('#' + tbSearchId).val()).replace(/\s+/g, ' ');  // spr curr != undefined, wycinam spacje
            if (prev == undefined)
                prev = '';
            if (curr != undefined && curr != prev) {
                tbSearchArray[tbSearchId] = curr;
                doClick(btSearchClickId);
            }
        }, 250);
}

function startSearch2(tb, bt) {
    //alert(tb);
    //wnDebug(tb);

    tbSearchId = tb;
    btSearchClickId = bt;
    if (searchTimer == null)
        searchTimer = window.setInterval(function() {
            var prev = tbSearchArray[tbSearchId];
            var curr = $.trim($('#' + tbSearchId).val()).replace(/\s+/g, ' ');  // spr curr != undefined, wycinam spacje
            if (prev == undefined)
                prev = '';
            if (curr != undefined && curr != prev) {
                tbSearchArray[tbSearchId] = curr;
                doClick(btSearchClickId);
            }
        }, 750);
}

function startSearch3(searchTimer, tb, interval, callback) {
    wnDebug(tb);
    
    //tbSearchId = tb;
    if (searchTimer == null)
        searchTimer = window.setInterval(function() {
            var prev = tbSearchArray[tb];
            var val = $('#' + tb).val();
            var curr = $.trim(val).replace(/\s+/g, ' ');  // spr curr != undefined, wycinam spacje
            if (prev == undefined)
                prev = '';
            if (curr != undefined && curr != prev) {
                tbSearchArray[tb] = curr;
                wnDebug(curr);

                callback(val);
            }
            else
                wnDebug('.');

        }, interval);   //250
}

function keypressEnterClick(bt) {
    var k;
    //var k = window.event ? e.keyCode : e.which;
    if (window.event) k = event.keyCode;    // IE
    else if (e.which) k = e.which;          // Netscape/Firefox/Opera
    else k = e.keyCode;
    if (k == 13) {
        doClick(bt);
        return true;
    }
    else
        return false;
}

function setSelectionRange(input, selectionStart, selectionEnd) {
    if (input.setSelectionRange) {
        //input.focus();
        input.setSelectionRange(selectionStart, selectionEnd);
    }
    else if (input.createTextRange) {
        var range = input.createTextRange();
        range.collapse(true);
        range.moveEnd('character', selectionEnd);
        range.moveStart('character', selectionStart);
        range.select();
    }
}

function focusEnd(tbId) {
    var tb = document.getElementById(tbId);
    if (tb != null) {
        var text = tb.value;
        tb.focus();
        //tb.setSelectionRange(text.length+10, text.length+10);
        setSelectionRange(tb, text.length + 10, text.length + 10);
    }
}

function focus(tbId) {
    var tb = document.getElementById(tbId);
    if (tb != null)
        tb.focus();
}

//----- TimeEdit2 -----
function TimeEdit2_KeyPress(textbox, e) {
    if (window.event) k = event.keyCode;    // IE
    else if (e.which) k = e.which;          // Netscape/Firefox/Opera
    else              k = e.keyCode;
    if (k >= 48 && k <= 57) return true;
    else {
        if (k >= 32) 
            switch (textbox.maxlength) {
                default:
                case 5:
                    if (textbox.value.indexOf(':') == -1)
                        insertAtCursor(textbox, ':');
                    break;
                case 8:
                    var cnt = textbox.value.split(":").length - 1;
                    if (cnt < 2)
                        insertAtCursor(textbox, ':');
                    break;
                case 2:
                    break;
            }
        return false;
    }
}

function TimeEdit2_Click(tab, timeid) {
    var tb = document.getElementById(timeid);
    if (tb != null) {
        tb.value = $(tab).html();
        tb.focus();
    }
}

function strToTime(timestr) {
    var dt = new Date();
    var t = timestr.split(':');
    switch (t.length) {
        case 0:
            dt.setHours(0);
            dt.setMinutes(0);
            dt.setSeconds(0);
            break;
        case 1:
            dt.setHours(t[0]);
            dt.setMinutes(0);
            dt.setSeconds(0);
            break;
        case 2:
            dt.setHours(t[0]);
            dt.setMinutes(t[1]);
            dt.setSeconds(0);
            break;
        case 3:
            dt.setHours(t[0]);
            dt.setMinutes(t[1]);
            dt.setSeconds(t[2]);
            break;
    }
    return dt;
}

function nn(d) {
    return (d > 9 ? '' : '0') + d;
}

function TimeEdit2_NavClick(tab, timeid) {
    var tb = document.getElementById(timeid);
    if (tb != null) {
        var d = parseInt($(tab).attr('opt'));
        switch (d) {
            case -99:
                tb.value = '';
                break;
            case 0: 
                tb.values = '0:00';
                break;
            default:
                var dt = new Date(strToTime(tb.value).getTime() + d * 60000);
                tb.value = dt.getHours() + ':' + nn(dt.getMinutes());
                break;
        }
        tb.focus();
    }
}

function TimeEdit2_Nav2Click(tab, showid, hideid) {
    var show = document.getElementById(showid);
    var hide = document.getElementById(hideid);
    if (show != null && hide != null) {
        $(hide).hide();
        $(show).show();
    }
}
//----- zoom -----
function zoomPracUrlop(title, uPanelId) {
    $('#divZoom').dialog({
        title: title,
        position: ['center', 'center'],
        width: 700,
        modal: true,
        open: function(type, data) {
            $(this).parent().appendTo('#' + uPanelId);
            //$(this).parent().appendTo('form');
        }
    });
}

function showDialog(divDialog, uPanelId, title) {
    $('#' + divDialog).dialog({
        title: title,
        position: ['center', 'center'],
        width: 700,
        modal: true,
        open: function(type, data) {
            $(this).parent().appendTo('#' + uPanelId);
            //$(this).parent().appendTo('form');
        }
    });
}



/*------modal wnioski osobowe---------*/
var BlockButton = false;



/*------modal wnioski osobowe---------*/
function showDialog2(divDialog, uPanelId, title, width, btCloseId) {
    $('#' + divDialog).dialog({
        title: title,
        position: {my :"center", at: "center"}, //['center', 'center'],
        width: width,
        modal: true,
        close: function() {
            if (btCloseId != null) {
                $('#' + btCloseId).prop("onclick", null);
                doClick(btCloseId)
            }
        },
        open: function(type, data) {
            $(this).parent().appendTo('#' + uPanelId);
            //$(this).parent().appendTo('form');
        }
    });
    //alert(1);
    //$('#' + divDialog).dialog('option', 'position', 'center');
    //$('#' + divDialog).dialog('option', 'position', 'center', 'size');
    //$('#' + divDialog).dialog({height:auto});
}




function DistableButtonDialog() {
    BlockButton = "true";
}

function showDialog2confirm(divDialog, uPanelId, title, width, btCloseId, btyID, btnID, mes) {
    var b;
    BlockButton = "false";
    $('#' + divDialog).dialog({
        title: title,
        beforeClose: function(event, ui) {
            if (BlockButton != "true") var r = confirm(mes);


            if (r == true) {


                b = "Y";


            } else {


                b = "N";

            }
        },
        position: ['center', 'center'],
        width: width,
        modal: true,
        close: function() {
            if (btCloseId != null)
                $('#' + btCloseId).prop("onclick", null);
            doClick(btCloseId)

            switch (b) {

                case "Y":

                    $('#' + btyID).prop("onclick", null);
                    doClick(btyID);
                    break;
                case "N":

                    $('#' + btnID).prop("onclick", null);
                    doClick(btnID);
                    break;
            }



        },
        open: function(type, data) {
            $(this).parent().appendTo('#' + uPanelId);
            //$(this).parent().appendTo('form');
        }
    });
}






















//----- maile -----
var mailTextBox = null;

function selectControl(control) {
    mailTextBox = control;
}
/*
function selectZnaczniki(c, s) {
    if (mailTextBox == null)
        mailTextBox = document.getElementById(c);
    if (mailTextBox != null) {
        insertAtCursor(mailTextBox, s);
    }
}
*/
function insertZnacznik(c, s) {
    if (mailTextBox == null)
        mailTextBox = document.getElementById(c);
    if (mailTextBox != null) {
        insertAtCursor(mailTextBox, s + ' ');
    }
}

function insertMailZnacznik(s) {
    if (mailTextBox != null) {
        var tb = document.getElementById(mailTextBox.id);
        if (tb != null) {
            insertAtCursor(mailTextBox, s + ' ');
            return true;
        }
    }
    alert("Proszę wskazać miejsce w temacie lub treści wiadomości, w które ma być wstawiony znacznik.");
    return false;
}



//----- -----
function confirmDelete() {
    return confirm("Potwierdzasz usunięcie rekordu danych ?");
}

function doConfirm(question, resultId) {
    var res = confirm(question);
    var input = document.getElementById(resultId);
    if (input != null) input.value = res ? '1' : '0';
    return true;
}

function confirmClick(msg, btOk, btCancel) {
    if (confirm(msg)) {
        if (btOk != '') doClick(btOk);
        return true;
    }
    else {
        if (btCancel != '') doClick(btCancel);
        return false;
    }
}

//----- -----










var wm = {
    btn: {
        ok: {
            id: "btnOK",
            html: "<input id='btnOk' type='button' value='Ok' class='xbutton100 btnOk btn btn-sm btn-success' />"
        },
        cancel: {
            id: "btnCancel",
            html: "<input id='btnCancel' type='button' value='Anuluj' class='xbutton100 mbtnCancel btn btn-sm btn-default' />"
        }
    },

    def: {
        title: "Title",
        msg: "Message"
    },
    title: null,
    msg: null,
    btnOk: null,
    btnCancel: null,
    pst: null,
    doOldClick: null,
    flag: 0,
    closeFunction: null,

    showMessage: function(msg, title, btnOk, cf) {
        wm.msg = wm.isEmpty(msg) ? wm.def.msg : msg;
        wm.title = wm.isEmpty(title) ? wm.def.title : title;
        wm.btnOk = btnOk; //null;
        wm.btnCancel = null;
        wm.showWindow(true, false);
        wm.setEvents();
        wm.closeFunction = cf;

    },

    showConfirm: function(msg, title, btnOK, btnCancel, pst, doOldClick) {
        wm.msg = wm.isEmpty(msg) ? wm.def.msg : msg;
        wm.title = wm.isEmpty(title) ? wm.def.title : title;
        wm.pst = pst;
        wm.btnOk = btnOK;
        wm.btnCancel = btnCancel;
        wm.doOldClick = doOldClick;
        wm.showWindow(true, true);
        wm.setEvents();

    },

    showConfirm2: function(msg, title, btnOK, btnCancel) {
        //alert(btnOK.id);
        if (wm.flag == 0) {
            wm.flag = 1;
            wm.msg = wm.isEmpty(msg) ? wm.def.msg : msg;
            wm.title = wm.isEmpty(title) ? wm.def.title : title;
            wm.btnOk = btnOK;
            wm.btnCancel = btnCancel;
            wm.showWindow(true, true);
            wm.setEvents2();
            return false;
        } else {
            wm.flag = 0;
            return true;
        }
    },

    showWindow: function(btnOk, btnCancel) {
        $("body").append("<div class='popupBg'></div>");
        $("body").append("<div id='showConfirmPopup' class='popup showConfirmPopup'>" +
                            "<div id='header'>" +
                                "<span class='title'>" + wm.title + "</span>" +
        //                                "<a id='popupCloser' class='fa fa-close popupCloser mbtnCancel' href='javascript://'></a>" +
                            "</div>" +
                         "<div id='content'>" + wm.msg + "</div><div id='footer'>" + (btnCancel ? wm.btn.cancel.html : "") + (btnOk ? wm.btn.ok.html : "") + "</div></div>");
        $("html").css("overflow", "hidden");
    },

    closeWindow: function() {
        $(".showConfirmPopup").remove();
        $(".popupBg").remove();
        $("html").css("overflow", "auto");
        if (!wm.isEmpty(wm.closeFunction)) {
            eval(wm.closeFunction);
        }
    },

    isEmpty: function(s) {
        return s == undefined || s == null || s == "";
    },

    setEvents: function() {
        $(".showConfirmPopup .btnOk").on("click", wm.okEvent);
        $(".showConfirmPopup .mbtnCancel").on("click", wm.cancelEvent);
    },

    setEvents2: function() {
        $(".showConfirmPopup .btnOk").on("click", wm.okEvent2);
        $(".showConfirmPopup .mbtnCancel").on("click", wm.cancelEvent);
    },

    okEvent: function() {
        if (wm.isEmpty(wm.btnOk)) {
            wm.closeWindow();
        } else {
            var btnOldClick = null;
            if (!wm.isEmpty($("#" + wm.btnOk).attr("onclick")) && wm.doOldClick) {
                btnOldClick = $("#" + wm.btnOk).attr("onclick").replace("javascript:", "").replace("return true;", "").replace("return false;");
                //alert(btnOldClick);
                if (!wm.isEmpty(btnOldClick))
                    eval(btnOldClick);
            }
            var btnRep = wm.btnOk.replace(/_/g, "$");
            var post = (!wm.isEmpty(wm.pst)) ? wm.pst : "__doPostBack('" + btnRep + "', '')";
            wm.closeWindow();
            eval(post);
        }
    },


    okEvent2: function() {
        if (wm.isEmpty(wm.btnOk)) {
            wm.closeWindow();
        } else {
            wm.closeWindow();
            var href = $("#" + wm.btnOk).attr("href");
            if(href != undefined && href.indexOf("doPostBack") > -1) /* new */
                eval(href);
            else
                $("#" + wm.btnOk).click();
        }
        wm.flag = 0;
    },

    cancelEvent: function(callback) {
        if (!wm.isEmpty(wm.btnCancel))
            $("#" + wm.btnCancel).click();
        wm.closeWindow();
        wm.flag = 0;
    }



};




/* bootstrap */

var wmb = {
    btn: {
        ok: {
            id: "btnOK",
            html: "<input id='btnOk' type='button' value='Ok' class='btn btn-success btnOk' name='btnOk' />"
        },
        cancel: {
            id: "btnCancel",
            html: "<input id='btnCancel' type='button' value='Anuluj' class='btn btn-default mbtnCancel' />"
        }
    },

    def: {
        title: "Title",
        msg: "Message"
    },
    title: null,
    msg: null,
    btnOk: null,
    btnCancel: null,
    pst: null,
    doOldClick: null,
    flag: 0,
    closeFunction: null,

    showMessage: function (msg, title, btnOk, cf) {
        wmb.msg = wmb.isEmpty(msg) ? wmb.def.msg : msg;
        wmb.title = wmb.isEmpty(title) ? wmb.def.title : title;
        wmb.btnOk = btnOk; //null;
        wmb.btnCancel = null;
        wmb.showWindow(true, false);
        wmb.setEvents();
        wmb.closeFunction = cf;

    },

    showConfirm: function (msg, title, btnOK, btnCancel, pst, doOldClick) {
        //alert(1);
        wmb.msg = wmb.isEmpty(msg) ? wmb.def.msg : msg;
        wmb.title = wmb.isEmpty(title) ? wmb.def.title : title;
        wmb.pst = pst;
        wmb.btnOk = btnOK;
        wmb.btnCancel = btnCancel;
        wmb.doOldClick = doOldClick;
        wmb.showWindow(true, true);
        wmb.setEvents();

    },

    showConfirm2: function (msg, title, btnOK, btnCancel) {
        //return false;
        //alert(wmb.flag);
        //alert(btnOK.id);
        var $btnOk = $("#" + btnOK);
        //alert($btnOk.attr("id"));
        //alert($btnOk.attr('onclick'));
        if (wmb.flag == 0) {
            wmb.flag = 1;
            wmb.msg = wmb.isEmpty(msg) ? wmb.def.msg : msg;
            wmb.title = wmb.isEmpty(title) ? wmb.def.title : title;
            wmb.btnOk = btnOK;
            wmb.btnCancel = btnCancel;
            wmb.showWindow(true, true);


            //var onclick = $btnOk.attr('onclick');
            //var idx = -1;
            //if ((idx = onclick.indexOf('WebForm_DoPostBack')) > -1) {
                //alert('qweqwewqe');
                //alert(idx);
                //afterFunction = onclick.slice(idx, onclick.length);
                //var newOnClick = onclick.slice(0, idx);
                //$btnOk.attr('onclick', newOnClick);
                //alert();

                //$(".showConfirmPopup .btnOk").attr("onclick", afterFunction);

                //$(".showConfirmPopup .btnOk").on("click", function () {
                //    alert(afterFunction);
                //    console.log(afterFunction);
                //    afterFunction();
                //});
                //$(".showConfirmPopup .mbtnCancel").on("click", wmb.cancelEvent);
            //} else {
                wmb.setEvents2();
            //}
            //console.log('false');
            return false;
        } else {
            wmb.flag = 0;
            return true;
        }
    },

    showWindow: function (btnOk, btnCancel) {
        var no = $('.showConfirmPopup').length + 1;
        wmb.id = "showConfirmPopup" + no;// + zIndex;

        $("body").append("<div id='" + wmb.id + "' class='xpopup showConfirmPopup modal cmodal fade' role='dialog'>" +
                            "<div class='modal-dialog'>" +
                                "<div class='modal-content'>" + 
                                "<div id='header' class='modal-header'>" +
                                    "<h4 class='title'>" + wmb.title + "</h4>" +
                                "</div>" +
                         "<div id='xcontent' class='modal-body'>" + wmb.msg + "</div><div id='footer' class='modal-footer'>" +
                          (btnOk ? wmb.btn.ok.html : "") + (btnCancel ? wmb.btn.cancel.html : "") + "</div></div></div></div>");


        //var zIndex = 1040 + (10 * $('.modal:visible').length) + 10;


        $("#" + wmb.id).modal({ backdrop: 'static', keyboard: false });

        prepareModal(wmb.id);
        //$("#" + wmb.id).css('z-index', zIndex);

        //setTimeout(function () {
        //    $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).attr('id', wmb.id + 'back').addClass('modal-stack');
        //}, 0);

        //$(document).on('hidden.bs.modal', '.modal', function () {
        //    $('.modal:visible').length && $(document.body).addClass('modal-open');
            //$("#" + wmb.id + "back").remove()*
        //});
        
    },

    closeWindow: function () {
        //console.log('close' + wmb.id + " " + wmb.msg);
        //$('.cmodal.in').modal('hide');
        console.log('hide');
        $('#' + wmb.id).modal('hide');
        ___remove_id = wmb.id;
        $('#' + wmb.id).on('hidden.bs.modal', function () {
            //console.log('remove' + ___remove_id);
            $("#" + ___remove_id).remove();
            if (!wmb.isEmpty(wmb.closeFunction)) {
                eval(wmb.closeFunction);
            }
            return true;
        });
        return false;
    },

    isEmpty: function (s) {
        return s == undefined || s == null || s == "";
    },

    setEvents: function () {
        $(".showConfirmPopup .btnOk").on("click", wmb.okEvent);
        $(".showConfirmPopup .mbtnCancel").on("click", wmb.cancelEvent);
    },

    setEvents2: function () {
        $(".showConfirmPopup .btnOk").on("click", wmb.okEvent2);
        $(".showConfirmPopup .mbtnCancel").on("click", wmb.cancelEvent);
    },

    okEvent: function () {
        //console.log('1');
        if (wmb.isEmpty(wmb.btnOk)) {
            wmb.closeWindow();
        } else {

            wmb.closeWindow();
            var btnOldClick = null;
            if (!wmb.isEmpty($("#" + wmb.btnOk).attr("onclick")) && wmb.doOldClick) {
                btnOldClick = $("#" + wmb.btnOk).attr("onclick").replace("javascript:", "").replace("return true;", "").replace("return false;");
                //alert(btnOldClick);
                if (!wmb.isEmpty(btnOldClick))
                    eval(btnOldClick);
            }
            var btnRep = wmb.btnOk.replace(/_/g, "$");
            var post = (!wmb.isEmpty(wmb.pst)) ? wmb.pst : "__doPostBack('" + btnRep + "', '')";
            
            eval(post);
        }
    },


    okEvent2: function () {
        //console.log(2);
        if (wmb.isEmpty(wmb.btnOk)) {
            wmb.closeWindow();
        } else {
            wmb.closeWindow();
            var href = $("#" + wmb.btnOk).attr("href");
            if (href != undefined && href.indexOf("PostBack") > -1) {/* new */
                href = href.replace("javascript:", "");
                eval(href);
            }
            else
                $("#" + wmb.btnOk).click();
        }
        wmb.flag = 0;
    },

    cancelEvent: function (callback) {
        if (!wmb.isEmpty(wmb.btnCancel))
            $("#" + wmb.btnCancel).click();
        wmb.closeWindow();
        wmb.flag = 0;
    }



};




//function showConfirm(content, title, btnOk, btnCancel, pst) {

//    //    if ($("#showConfirmPopup").length > 0) {
//    //        $(".popupBg").show();
//    //        $("#showConfirmPopup").show();
//    //    }
//    //    else {
//    //        if(
//    var ok = "<input id='btnOk' type='button' value='Ok' class='button100 btnOk' />";
//    var cancel = (btnCancel != null && btnCancel != undefined && btnCancel != "") ? "<input id='btnCancel' type='button' value='Cancel' class='button100 mbtnCancel' />" : "";

//    if (title == undefined) title = '';

//    $("body").append("<div class='popupBg'></div>");
//    $("body").append("<div id='showConfirmPopup' class='popup showConfirmPopup'><div id='header'><span class='title'>" + title + "</span><a id='popupCloser' class='fa fa-close popupCloser mbtnCancel' href='javascript://'></a></div><div id='content'>" + content + "</div><div id='footer'>" + cancel + ok + "</div></div>");
//    //    }

//    $("html").css("overflow", "hidden");

//    $("#showConfirmPopup .btnOk").on("click", function() {
//        alert(btnOk);
//        if (btnOk != null && btnOk != undefined && btnOk != "") {
//            //$("#" + btnOk).click();
//            //alert(btnOk.replace(/_/g, "$"));
//            var btnOldClick = null;
//            if ($("#" + btnOk).attr("onclick") != undefined)
//                btnOldClick = $("#" + btnOk).attr("onclick").replace("javascript:", "").replace("return true;", "").replace("return false;");
//            //alert(btnOldClick);
//            //alert(btnOldClick);
//            if(btnOldClick != undefined && btnOldClick != null && btnOldClick != "")
//                eval(btnOldClick);
//            //return false;
//            var btnRep = btnOk.replace(/_/g, "$");
//            var post = (pst != null && pst != undefined) ? pst : "__doPostBack('" + btnRep + "', '')";
//            //alert(post);
//            //__doPostBack(post);
//            hidePopup();
//            eval(post);
//        } else {
//            hidePopup();
//        }
//    });

//    $("#showConfirmPopup .mbtnCancel").on("click", function() {
//        //alert('cancel');
//        if (btnCancel != null && btnCancel != "" && btnCancel != undefined)
//            $("#" + btnCancel).click();
//        hidePopup();
//    });

//    //$("#showConfirmPopup .popupCloser").on("click", function() {
//    //hidePopup();
//    //});

//    //$(".popupBg").on("click", function() {
//    //hidePopup();
//    //});

//    //$(window).keyup(function(e) {
//    //var key = e.which;
//    //if (key == 27) {
//    //hidePopup();
//    //}
//    //});

//    function hidePopup() {
//        //console.log("hide");
//        //        $(".popupBg").hide();
//        //        $("#showConfirmPopup").hide();
//        //console.log((".showConfirmPopup").length);
//        $(".showConfirmPopup").remove();
//        $(".popupBg").remove();
//        $("html").css("overflow", "auto");
//    }
//}


//--------------------------------
function insertAtCursor(Field, Text) {
    p = Field.scrollTop;
    Field.focus();
    if (document.selection) {   //IE
        sel = document.selection.createRange();
        sel.text = Text;
    } else if (Field.selectionStart || Field.selectionStart == '0') {  //Mozilla/Firefox/Netscape 7+ 
        var startPos = Field.selectionStart;
        var endPos = Field.selectionEnd;
        Field.value = Field.value.substring(0, startPos) + Text + Field.value.substring(endPos, Field.value.length);
        Field.setSelectionRange(endPos + Text.length, endPos + Text.length);
    } else {
        Field.value += Text;
    }
    Field.scrollTop = p
}

function doClick(btid) {
    //alert(btid);
 
    var bt = document.getElementById(btid);
    if (bt != null) bt.click();
}

function doClickPar(btid, par) {
    var bt = document.getElementById(btid);
    var p = document.getElementById(btid + "Par");
    if (bt != null && p != null) {
        p.value = par;
        bt.click();
    }
}
//-----
var fixFFedit = false;

function doClickSelectFFfix(btid) {
    //alert('sel(' + fixFFedit + '):' + btid);
    if (!fixFFedit) {
        var bt = document.getElementById(btid);
        if (bt != null) bt.click();
    }
}

function x_doClickEditFFfix(btid) {
    //alert('ed(' + fixFFedit + '):' + btid);    
    fixFFedit = true;
    var bt = document.getElementById(btid);
    if (bt != null) {
        alert(bt.name);
        bt.click();
    }
    window.setTimeout(function() { fixFFedit = false; }, 1000);
}

function doClickEditFFfix2() {
    //alert('ed');    
    fixFFedit = true;
    window.setTimeout(function() { fixFFedit = false; }, 1000);
}


//------ HRCheck --------------------------------
function showAItemEdit(show, tdId) {
    $('#' + tdId).children('*').each(
        function() {
            //alert($(this).attr("id"));
            if (show) {
                if ($(this).attr("id").indexOf("lbError") != -1)
                    $(this).hide();
                else
                    $(this).show();
            }
            else
                $(this).hide();
        });
}
//------ RCP ---------------------------------------
function changeColorCPE(sender) {  // ajax color picker extender
    sender.get_element().style.color = "#" + sender.get_selectedColor();
}

function showAjaxProgress() {
    //document.getElementById('ctl00_updProgress1').style.display = 'block';
    var up = document.getElementById('ctl00_ContentPlaceHolder1_updProgress1');
    if (up == null)
        up = document.getElementById('ctl00_ContentPlaceHolderReport_updProgress1');
    if (up == null)
        up = document.getElementById('ctl00_ctl00_updProgress1');
    if (up != null)
        up.style.display = 'block';
    return true;
}

function hideAjaxProgress() {
    var up = document.getElementById('ctl00_ContentPlaceHolder1_updProgress1');
    if (up == null)
        up = document.getElementById('ctl00_ContentPlaceHolderReport_updProgress1');
    if (up == null)
        up = document.getElementById('ctl00_ctl00_updProgress1');
    if (up != null)
        up.style.display = 'none';
    return true;
}

//----- struktura -----
//var structScrollTop = 0;
//var structScrollTopIdx = 0;
var structScrollTop = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
var showStructNotFound = false;
var isScrollSet = false;
var tvStructureId = ['', '', ''];
var paStructureId = ['', '', ''];

function scroll_tvStructureIdx(idx) {
    if (idx >= 0 && idx <= 2) {
        var sid = tvStructureId[idx];
        if (sid != '') {
            var elem = document.getElementById(sid + '_SelectedNode');
            if (elem != null)
                if (elem.value != null && elem.value != '') {
                var node = document.getElementById(elem.value);
                if (node != null) {
                    node.scrollIntoView(false);
                    structScrollTop[idx] = -1;      // nie ustawiaj
                }
            } else {                                // nic nie jest zaznaczone
                structScrollTop[idx] = 0;
                elem = document.getElementById(paStructureId[idx]);
                if (elem != null) elem.scrollTop = structScrollTop[idx];
                else {
                    //tvStructureId[idx] = '';       // wyłączam
                    //paStructureId[idx] = '';
                }
            }
        }
    }
}

function structOnBeginRqIdx(idx) {
    var pid = paStructureId[idx];
    if (pid != '') {
        var elem = document.getElementById(pid);
        if (elem != null) structScrollTop[idx] = elem.scrollTop;
        else {
            //alert('2 ' + pid);
            //tvStructureId[idx] = '';
            //paStructureId[idx] = '';
        }
    }
}

function structOnEndRqIdx(idx) {
    var pid = paStructureId[idx];
    if (pid != '') {
        var elem = document.getElementById(pid);
        if (elem != null) {
            if (structScrollTop[idx] != -1)
                elem.scrollTop = structScrollTop[idx];
        }
        else {
            //alert('1 ' + paStructureId);
            //tvStructureId[idx] = '';
            //paStructureId[idx] = '';
        }
    }
}

function ShowNotFound() {
    showStructNotFound = true;
}

function scroll_tvStructure() {
    for (var i = 0; i < 3; i++)
        scroll_tvStructureIdx(i);
}

function structOnBeginRq(sender, args) {
    //alert(1);
    for (var i = 0; i < 3; i++)
        structOnBeginRqIdx(i);
}

function structOnEndRq(sender, args) {
    //alert(2);
    for (var i = 0; i < 3; i++)
        structOnEndRqIdx(i);

    if (showStructNotFound) {
        showStructNotFound = false;
        alert('Nie znaleziono.');
    }
}


/*
var structScrollTop = 0;
var showStructNotFound = false;
var tvStructureId;
var paStructureId;

function scroll_tvStructure() {
var elem = document.getElementById(tvStructureId + '_SelectedNode');
if (elem != null)
if (elem.value != null && elem.value != '') {
var node = document.getElementById(elem.value);
if (node != null) {
node.scrollIntoView(false);     // tak zadziałało - alignuje do bottom, do top jakby całą strone chciał odswiezyć
structScrollTop = -1;                // nie ustawiaj
}
} else {                                // nic nie jest zaznaczone
structScrollTop = 0;
elem = document.getElementById(paStructureId);
if (elem != null) elem.scrollTop = structScrollTop;
}
}

function ShowNotFound() {
showStructNotFound = true;
}

function structOnBeginRq(sender, args) {
var elem = document.getElementById(paStructureId); // uwaga!!! panel musi miec scroll ustawione na visible nie na auto bo nie dziala !!!
if (elem != null) structScrollTop = elem.scrollTop;
}

function structOnEndRq(sender, args) {
var elem = document.getElementById(paStructureId);
if (elem != null && structScrollTop != -1) elem.scrollTop = structScrollTop;
    
if (showStructNotFound) {
showStructNotFound = false;
alert('Nie znaleziono.');
}
}
*/
//----- accpanel -----
var rcpScrollTop = 0;

function getRcpScrollPos(sender, args) {
    var elem = document.getElementById('paAcceptScroll'); 
    if (elem != null) rcpScrollTop = elem.scrollTop;
}

function setRcpScrollPos() {
    if (rcpScrollTop != 0) {
        var elem = document.getElementById('paAcceptScroll');
        if (elem != null) elem.scrollTop = rcpScrollTop;
    }
}

var blink = false;

function startBlinkPPAcc()
{
    //setInterval(blinkPPAcc, 2000);
    console.log('start blink');
}

function blinkPPAcc() {
    var color = blink ? 'Red' : 'Transparent';
    blink = !blink;
    //$('div.alert').css('background-color', color);
    //$('div.alert').stop().css('background-color', 'red').animate({ backgroundColor: 'Transparent' }, 250);
    $('div.alert').animate({ backgroundColor: 'Red' }, 500).delay(500).animate({ backgroundColor: 'Transparent' }, 1000);
}

//-----
function getInternetExplorerVersion()
// Returns the version of Internet Explorer or a -1
// (indicating the use of another browser).
{
    var rv = -1; // Return value assumes failure.
    if (navigator.appName == 'Microsoft Internet Explorer') {
        var ua = navigator.userAgent;
        var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) != null)
            rv = parseFloat(RegExp.$1);
    }
    return rv;
}

function getInternetExplorerVersion2() {
    var rv = -1;
    if (navigator.appName == 'Microsoft Internet Explorer') {
        var ua = navigator.userAgent;
        var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) != null)
            rv = parseFloat(RegExp.$1);
    }
    else if (navigator.appName == 'Netscape') {
        var ua = navigator.userAgent;
        var re = new RegExp("Trident/.*rv:([0-9]{1,}[\.0-9]{0,})");
        if (re.exec(ua) != null)
            rv = parseFloat(RegExp.$1);
    }
    return rv;
}

function setColumnIEofs() {
    var ver = getInternetExplorerVersion();

    if (ver > -1 && ver < 10)
        //ie_xofs = -2;
        ie_xofs = -3;
    else
        //ie_xofs = 0;
        ie_xofs = -3;
    //alert(ie_xofs);
}

var ie_xofs = 0;
$.fn.row = function(i) { return $('tr:nth-child(' + (i + 1) + ') td', this); }
$.fn.column = function(i) { return $('tr td:nth-child(' + (i + 1 + ie_xofs) + ')', this); }
/*
// example - this will make the first column of every table red
$(document).ready(function() {
    $('table').column(0).css('background-color', 'red');
});

function findParentNode(parentName, childObj) {
    var testObj = childObj.parentNode;
    var count = 1;
    while(testObj.getAttribute('name') != parentName) {
        alert('My name is ' + testObj.getAttribute('name') + '. Let\'s try moving up one level to see what we get.');
        testObj = testObj.parentNode;
        count++;
    }
    // now you have the object you are looking for - do something with it
    alert('Finally found ' + testObj.getAttribute('name') + ' after going up ' + count + ' level(s) through the DOM tree');
} 

*/

//------ zaznaczanie dni - wszyscy i pojedynczy ------
function cbDniAllClickPP(cb) {
    if (cb.checked)
        $("input[id$='cbDay']").each(
            function() {
                var th = $(this).closest('th');
                if (!th.hasClass('holiday')) {
                    //$(this).attr('checked', true);
                    $(this).prop('checked', true);
                    var idx = parseInt($(this).parent().attr('idx'));  // cb jest w span
                    $('table.tbPlanPracy').column(idx + 5).addClass('selDay');
                    //th.addClass('selDay');
                }
            });
    else {
        //$("input[id$='cbDay']").attr('checked', false).each(
        $("input[id$='cbDay']").prop('checked', false).each(
            function () {
                var idx = parseInt($(this).parent().attr('idx'));  // cb jest w span
                $('table.tbPlanPracy').column(idx + 5).removeClass('selDay');
                //$(this).closest('th').removeClass('selDay');
            });
    }
}

function cbDayClickPP(cb, index) {
    //var idx = index;
    var idx = parseInt($(cb).parent().attr('idx'));  // cb jest w span 
    if (cb.checked) {
        //$("input[id$='cbDniAll']").attr('checked', true);
        $("input[id$='cbDniAll']").prop('checked', true);
        $('table.tbPlanPracy').column(idx + 5).addClass('selDay');
    } else {
        if (!$("input[id$='cbDay']").is(':checked'))
            //$("input[id$='cbDniAll']").attr('checked', false);
            $("input[id$='cbDniAll']").prop('checked', false);
        $('table.tbPlanPracy').column(idx + 5).removeClass('selDay');
    }
}

//------ zaznaczanie pracowników - wszyscy i pojedynczy ------
function cbPracAllClickPP(cb) {
    if (cb.checked)
        //$("input[id$='cbPrac']").attr('checked', true).closest('tr').children('td').addClass('selPrac');
        $("input[id$='cbPrac']").prop('checked', true).closest('tr').children('td').addClass('selPrac');
    else
        //$("input[id$='cbPrac']").attr('checked', false).closest('tr').children('td').removeClass('selPrac');
        $("input[id$='cbPrac']").prop('checked', false).closest('tr').children('td').removeClass('selPrac');
}

function cbPracClickPP(cb, pracId) {
    if (cb.checked) {
        //$("input[id$='cbPracAll']").attr('checked', true);
        $("input[id$='cbPracAll']").prop('checked', true);
        $(cb).closest('tr').children('td').addClass('selPrac');
    } else {
        if (!$("input[id$='cbPrac']").is(':checked'))
            //$("input[id$='cbPracAll']").attr('checked', false);
            $("input[id$='cbPracAll']").prop('checked', false);
        $(cb).closest('tr').children('td').removeClass('selPrac');
    }
}

//----- kliknięcia w celkę -----
function selectCellPP(divsender, pracId, dayIdx, prefix, zmianyId) {        // divsender - sender; pracId, dayIdx - parametry; prefix id kontrolek; zmianyId - hid z parametrami zmiany
    var cbDniAll = document.getElementById(prefix + 'lvPlanPracy_cbDniAll');
    var cbPracAll = document.getElementById(prefix + 'lvPlanPracy_cbPracAll');
    if (cbDniAll != null && cbPracAll != null && (cbDniAll.checked || cbPracAll.checked)) {                            // robię postback z parametrami
        var p = document.getElementById(prefix + 'hidClickPracId');
        var d = document.getElementById(prefix + 'hidClickDayIndex');
        if (p != null && d != null) {
            p.value = pracId;
            d.value = dayIdx;
            doClick(prefix + 'btSelectCell');
        }
    } else {                                                                // przetwarzam po stronie przegladarki
        var data = document.getElementById(prefix + zmianyId);
        var zmiana = document.getElementById(prefix + 'hidSelZmiana');
        if (data != null && zmiana != null) {
            var dv = data.value.split(',');                 // data line -> cell data
            var zm = dv[dayIdx].split('|');                 // cell data -> array
            var zmSel = zmiana.value.split('|');            // sel -> array
            var zmSelId = zmiana.value != '' ? zmSel[0] : '';
            var noprz = zm[3].indexOf('p') != -1;           // brak przypisania 
            if (zm[0] == zmSelId || zmSelId == '' || noprz) {        // zmiana na pusto lub podmiana ale tylko 3 pierwsze: idZmiany, symbol, kolor
                zm[0] = '';
                zm[1] = '';
                zm[2] = '';                                 // [3] to info 
                zm[4] = '';
            } else {
                zm[0] = zmSel[0];
                zm[1] = zmSel[1];
                zm[2] = zmSel[2];
                zm[4] = zmSel[4];
            }
            dv[dayIdx] = zm.join('|');
            data.value = dv.join(',');
            var lbid = divsender.id.substring(0, divsender.id.length - 5) + 'lbTop';
            var lb = document.getElementById(lbid);
            if (lb != null) {
                lb.innerHTML = zm[1];
                lb.title = zm[4];
            }
            else divsender.innerHTML = zm[1];                     // albo po staremu
            divsender.parentNode.style.backgroundColor = zm[2];   // td zmieniam bo cell to div 
            divsender.parentNode.style.backgroundImage = zm[2] != '' ? 'none' : '';
        }
    }
}

//----- plan urlopow -----
function pad(n, width, z) {
    var z = z || '0';
    var n = n + '';
    return n.length >= width ? n : new Array(width - n.length + 1).join(z) + n;
}

Number.prototype.lpad = function(width){
    var z = '0';
    var n = String(this);
    return n.length >= width ? n : new Array(width - n.length + 1).join(z) + n;
}

//var cntPrefix = '';
var puKorekta = 0;
var puCheckMode = 1;    // 0-min 14, 1-min 14 i cały 
var puDaysCount = 37;

function initPlanUrlopow(prefix, kor, cm) {      // ctl00_ContentPlaceHolder1_cntPlanRoczny_lvPlanPracy_  ctrl0  _PlanPracyLine_Repeater1_ctl28_tdPP
    puKorekta = kor;
    puCheckMode = cm;
    getPlanUrlopowSumy(prefix);
}

//ctl00_ContentPlaceHolder1_cntPlanRoczny_lvPlanPracy_ctrl0_PlanPracyLine_hidZmiany 
//ctl00_ContentPlaceHolder1_cntPlanRoczny_lvPlanPracy_ctrl0_PlanPracyLine_Repeater1_ctl28_tdPP

function setColorPUline(prefix, m, d1, d2, kod, color) {
    //ctl00_ContentPlaceHolder1_cntPlanRoczny_
    var ppp = prefix + 'lvPlanPracy_ctrl' + m;
    var hidData = document.getElementById(ppp + '_PlanPracyLine_hidZmiany'); 
    if (hidData != null) {
        var dv = hidData.value.split(',');          // dni      
        for (var d = d1; d <= d2; d++) {
            var zm = dv[d].split('|');            
            var planUW = zm[0] != '';               // kod
            //var noday = zm[3].indexOf('x') != -1;   // info
            //var wolne = zm[3].indexOf('h') != -1;
            if (planUW) {
                zm[0] = kod;
                zm[2] = color;            
                dv[d] = zm.join('|');                
                var td = document.getElementById(ppp + '_PlanPracyLine_Repeater1_ctl' + d.lpad(2) + '_tdPP');
                if (td != null) td.style.backgroundColor = color;
            }    
        }
        hidData.value = dv.join(',');
    }
}

function setPU(prefix, m1, d1, m2, d2, kod, color) {
    if (m1 == m2) {
        setColorPUline(prefix, m1, d1, d2, kod, color);
    }
    else if (m1 < m2) {
        for (var m = m1; m <= m2; m++) {
            if      (m == m1) setColorPUline(prefix, m, d1, puDaysCount - 1, kod, color);
            else if (m == m2) setColorPUline(prefix, m, 0,  d2,              kod, color);
            else              setColorPUline(prefix, m, 0,  puDaysCount - 1, kod, color);  
        }
    }
}

function setValuePU(prefix, no, value, error, warning) {     // prefix powinien mieć '#' z przodu, warning - czy to ma byc tylko ostrzezenie
    $(prefix + 'lbVal' + no).html(value);
    var imgOk   = $(prefix + 'imgOk'  + no);
    var imgErr  = $(prefix + 'imgErr' + no);
    var imgWarn = $(prefix + 'imgWarn' + no);
    var lbErr   = $(prefix + 'lbErr' + no);
    var hidErr  = $(prefix + 'hidErr' + no);
    if (error) {
        imgOk.hide();

        if (warning) {
            imgWarn.show();
            hidErr.val('2');
        } else {
            imgErr.show();
            hidErr.val('1');
        }
        lbErr.show();
    } else {
        imgOk.show();
        imgErr.hide();
        imgWarn.hide();
        lbErr.hide();
        hidErr.val('');
    }
}

//ctl00_ContentPlaceHolder1_cntPlanRoczny_
//ctl00_ContentPlaceHolder1_cntPlanRoczny_lvPlanPracy_ctrl0_PlanPracyLine_hidZmiany 
function getPlanUrlopowSumy(prefix) {   
    var listId = 'lvPlanPracy_ctrl';
    var urlopId = '_PlanPracyLine_hidZmiany'; //urlopyId.substring(p + 1); spr ile cyfr !!!
    var sumyId = '_PlanPracyLine_hidSumy'; //urlopId.replace('hidZmiany', 'hidSumy');

    var sum1 = new Array(12);
    var sum2 = new Array(12);
    for (var m = 0; m < 12; m++) {
        sum1[m] = 0;
        sum2[m] = 0;
    }

    var prefixPI = '#' + prefix + 'lvPlanPracy_cntPracInfo_';
    var prefixSU = '#' + prefix + 'cntSelectUrlop_';

    var nom14 = parseInt($(prefixPI + 'hidNom14').val());
    var warn14;
    if (nom14 <= 0) {
        warn14 = true;
        nom14 = -nom14;
    }
    else
        warn14 = false;
        
    //var nom812 = parseInt($(prefixPI + 'hidNom812').val());   20140706 wyłączam
    var nomZal = parseInt($(prefixPI + 'hidZaleg').val());

    var puKodUW, puKolorUW, puKodUW14, puKolorUW14;

    //ctl00_ContentPlaceHolder1_cntPlanRoczny_
    //ctl00_ContentPlaceHolder1_cntPlanRoczny_cntSelectUrlop_hidPlanParams
    var par = $(prefixSU + 'hidPlanParams').val();
    if (par)
    {
        var P = par.split('|');
        if (par.length >= 4) {
            puKodUW = P[0];
            puKolorUW = P[1]
            puKodUW14 = P[2];
            puKolorUW14 = P[3];
        }
    }
    
    var max = 0;    
    var ciurek = 0;
    var ciurekUW = false;   // był UW a nie tylko sobota, niedz, święto
    var cnt812 = 0;
    var cntZal = 0;
    var cnt9 = 0;
    var ciurekM1, ciurekM2, ciurekD1, ciurekD2;
    //----- pętla po miesiącach -----
    for (var m = 0; m < 12; m++) {
        var ppp = prefix + listId + m;
        var data = document.getElementById(ppp + urlopId);
        var sumy = document.getElementById(ppp + sumyId);
        var lbS1 = document.getElementById(ppp + '_PlanPracyLine_Repeater2_ctl00_lb1');
        var lbS2 = document.getElementById(ppp + '_PlanPracyLine_Repeater2_ctl01_lb1');
                    
        if (data != null && sumy != null) {
            var dv = data.value.split(',');                 // data line -> cell data
            var sum = 0;
            
            var ofs = 0;
            var cnt = 0;
            var first14 = 0;
            var last14 = 0;
            //----- pętla po dniach -----
            for (var idx = 0; idx < puDaysCount; idx++){
                var zm = dv[idx].split('|');                // cell data -> array
                var planUW = zm[0] != '';
                //----- suma rok i do końca sierpnia -----
                if (planUW) {
                    sum++;
                    if (m < 8) cnt812++;
                    //x if (m < 8 && cnt812 < nom812) cnt812++;
                    if (m < 9 && cntZal < nomZal) cntZal++;
                    if (m == 8) cnt9++;
                    setPU(prefix, m, idx, m, idx, puKodUW, puKolorUW);
                }
                //----- 14 dni -----
                var noday = zm[3].indexOf('x') != -1;
                var wolne = zm[3].indexOf('h') != -1;

                if (!noday)
                    if (planUW || wolne) {      // sprawdzany dzień
                        if (ciurek == 0) {      // jeżeli jeszcze nie liczę
                            ciurekM1 = m;
                            ciurekD1 = idx;
                        }
                        ciurek++;
                        if (planUW) ciurekUW = true;
                        if (ciurekUW && ciurek > max) max = ciurek;     // info dodatkowe
                    }
                    else {
                        //----- aktualizacja kolorów -----
                        if (ciurek >= nom14) {
                            ciurekD2 = idx - 1;
                            if (ciurekD2 < 0) {
                                ciurekD2 = puDaysCount - 1;
                                ciurekM2 = m - 1;
                            }
                            else ciurekM2 = m;
                            if (ciurekUW) setPU(prefix, ciurekM1, ciurekD1, ciurekM2, ciurekD2, puKodUW14, puKolorUW14);
                        }
                        ciurek = 0;
                        ciurekUW = false;
                    }                    
                /*
                //----- kolor test -----
                if (td != null) {
                    var color = Math.floor((m * puDaysCount + idx) * 0xFFFFFF / (puDaysCount * 12)).toString(16);
                    color = '#' + color.lpad(6);
                    td.style.backgroundColor = color;
                }
                */
            }
            //----- sumy -----
            sum1[m] = sum;
            if (m == 0)
                sum2[m] = sum;
            else
                sum2[m] = sum2[m - 1] + sum;

            sumy.value = sum1[m] + '|' + sum2[m];
            lbS1.innerHTML = sum1[m];
            lbS2.innerHTML = sum2[m];
        }
    }
    //----- kolor jeżeli 14 w ciurku na końcu -----
    if (ciurekUW && ciurek >= nom14) {
        if (ciurek > max) max = ciurek;     // info dodatkowe
        setPU(prefix, ciurekM1, ciurekD1, 11, puDaysCount - 1, puKodUW14, puKolorUW14);
    }
    
    //----- info - validator -----
    var total = sum2[11];

    var wym = parseInt($(prefixPI + 'lbWymiar').html());
    var zal = parseInt($(prefixPI + 'lbZalegly').html());

    var lbDod = $(prefixPI + 'lbDodatkowy')
    var dod = lbDod.length == 0 ? 0 : parseInt(lbDod.html());

    //var wyk = parseInt($(prefixPI + 'lbWykorzystany').html());

    //$(prefixPI + 'lbPlan').html(total);

    switch (puCheckMode) {
        default:
        case 0:
            setValuePU(prefixPI, 4, total, total == 0, false);  // zaplanowany
            break;
        case 1:
            setValuePU(prefixPI, 4, total, wym + dod + zal - total != 0, false);  // zaplanowany
            break;
    }
    
    setValuePU(prefixPI, 1, wym + dod + zal - total, wym + dod + zal - total < 0, false);   // pozostało
    //setValuePU(prefixPI, 2, max, (wym + dod + zal > 0 || max > 0) && max < nom14, warn14);
    setValuePU(prefixPI, 2, max, (wym + dod + zal > 0 || max > 0) && max < nom14, warn14);
    //setValuePU(prefixPI, 3, cnt812, cnt812 < nom812, false);  20140706 wyłączam
    setValuePU(prefixPI, 5, cntZal, cntZal < nomZal, false); 

    /*
    var lbwym = document.getElementById(ppp + 'lbWymiar');
    var lbzal = document.getElementById(ppp + 'lbZalegly');
    var lbplan = document.getElementById(ppp + 'lbPlan');
    
    if (lbwym != null && lbzal != null && lbplan != null && lbzost != null && lbval14 != null && lbval812 != null) {
        var wym = parseInt(lbwym.innerHTML);
        var zal = parseInt(lbzal.innerHTML);
        lbplan.innerHTML = total;
        lbzost.innerHTML = wym + zal - total;
        lbval14.innerHTML = max;
        lbval812.innerHTML = cnt812;
    }
    */
}

function checkPlanUrlopow(prefix, adm) {
    var prefixPI = '#' + prefix + 'lvPlanPracy_cntPracInfo_';
    var e1 = $(prefixPI + "hidErr1").val();
    var e2 = $(prefixPI + "hidErr2").val();
    var e3 = $(prefixPI + "hidErr3").val();
    var e4 = $(prefixPI + "hidErr4").val();
    var e5 = $(prefixPI + "hidErr5").val();
    if (e1 == '1' || e2 == '1' || e3 == '1' || e4 == '1' || e5 == '1') {
        if (adm)
            return confirm("Uwaga !!!\n\nPlan nie jest poprawny.\nZapisać ?");
        else {
            alert("Uwaga !!!\n\nPlan nie jest poprawny.\nBrak możliwości zapisu.");
            return false;
        }
    }
    if (e1 == '2' || e2 == '2' || e3 == '2' || e4 == '2' || e5 == '2') {
        return confirm("Uwaga !!!\n\nNie są spełnione zaznacznone kryteria.\nZapisać ?");
    }
    return true;
}

function selectCellPU(divsender, dayIdx, prefix, urlopyId) {        // divsender - sender; pracId, dayIdx - parametry; prefix id kontrolek; zmianyId - hid z parametrami zmiany
    var hidData = document.getElementById(prefix + urlopyId);
    var hidSelUrlop = document.getElementById(prefix + 'hidSelUrlop');
    if (hidData != null && hidSelUrlop != null) {
        var dv = hidData.value.split(',');              // data line -> cell data// 0   1      2     3    4     5     6   
        var zm = dv[dayIdx].split('|');                 // cell data -> array:      kod symbol kolor info nazwa dayno modflag
        var zmSel = hidSelUrlop.value.split('|');       // sel -> array:            kod symbol kolor null nazwa
        //var zmSelKod = urlop.value != '' ? zmSel[0] : '';
        var zmSelKod = zmSel[0];
        var color;
        var kk = false;
        if (zm[0] != '' || zmSelKod == '') {        // zmiana na pusto lub podmiana ale tylko 3 pierwsze: idZmiany, symbol, kolor
            color = '';
            zm[0] = '';            // kod        
            zm[1] = '';            // symbol    
            zm[2] = '';            // kolor
            zm[4] = '';            // nazwa
        } else {
            color = zmSel[2];      // kolor 
            zm[0] = zmSelKod;      // kod        
            zm[1] = zmSel[1];      // symbol    
            zm[2] = color;         // kolor
            zm[4] = zmSel[4];      // nazwa
        }
        if (puKorekta) {
            if (zm[3].indexOf('k') == -1) {
                zm[3] += 'k';
                $(divsender).addClass('info');
            }
            else if (zm[0] == zm[6]) {
                zm[3] = zm[3].replace('k', '');
                $(divsender).removeClass('info');
            }
        }
        dv[dayIdx] = zm.join('|');
        hidData.value = dv.join(',');
        var lbid = divsender.id.substring(0, divsender.id.length - 5) + 'lbTop';
        var lb = document.getElementById(lbid);
        if (lb != null) {
            lb.innerHTML = zm[1];
            lb.title = zm[4];
        }
        else divsender.innerHTML = zm[1];                     // albo po staremu
        
        divsender.parentNode.style.backgroundColor = color;   // td zmieniam bo cell to div
        divsender.parentNode.style.backgroundImage = color != '' ? 'none' : '';

        getPlanUrlopowSumy(prefix);                 // 
    }
}



//---------------------
function selectCellPP_old(cell, index, dataid, zmid) {          // cell - sender; index - data; dataid - id hid z danymi zmian; zmid - hid z parametrami zmiany
    var data = document.getElementById(dataid);
    var zmiana = document.getElementById(zmid);
    if (data != null && zmiana != null) {
        var dv = data.value.split(',');                 // data line -> cell data
        var zm = dv[index].split('|');                  // cell data -> array
        var zmSel = zmiana.value.split('|');            // sel -> array
        var zmSelId = zmiana.value != '' ? zmSel[0] : '';
        
        if (zm[0] == zmSelId || zmSelId == '') {        // zmiana na pusto
            zm[0] = '';
            zm[1] = '';
            zm[2] = '';
        } else {
            zm[0] = zmSel[0];
            zm[1] = zmSel[1];
            zm[2] = zmSel[2];
        }
        dv[index] = zm.join('|');
        data.value = dv.join(',');

        var lbid = cell.id.substring(0, cell.id.length - 5) + 'lbTop';
        var lb = document.getElementById(lbid);
        if (lb != null) 
             lb.innerHTML = zm[1];                    
        else cell.innerHTML = zm[1];                    // albo po staremu
        cell.parentNode.style.backgroundColor = zm[2];  // td zmieniam bo cell to div 
        cell.parentNode.style.backgroundImage = zm[2] != '' ? 'none' : '';
    }
}



function selectCellPPAcc(divsender, pracId, dayIdx, prefix) {   // divsender - sender; pracId, dayIdx - parametry; prefix id kontrolek
    var p = document.getElementById(prefix + 'hidClickPracId');
    var d = document.getElementById(prefix + 'hidClickDayIndex');
    if (p != null && d != null) {
        p.value = pracId;
        d.value = dayIdx;
        doClick(prefix + 'btSelectCell');
    }
}


function selectCellPPAcc_old(cell, pracId, index, ctrlId) {
    var parPracId = document.getElementById(ctrlId + 'hidSelPracId');
    var parIndex = document.getElementById(ctrlId + 'hidSelDayIndex');
    var bt = document.getElementById(ctrlId + 'btSelectAcc');
    if (parPracId != null && parIndex != null && bt != null) {
        parPracId.value = pracId;
        parIndex.value = index;
        bt.click();
    }
}







function xx_selectCellPP_1(cell, index, dataid, zmid) {

}



function x_selectCellPPAccxxx(cell, pracId, index, ctrlId) {
    var parPracId = document.getElementById(ctrlId + 'hidSelPracId');
    var parIndex = document.getElementById(ctrlId + 'hidSelDayIndex');
    var bt = document.getElementById(ctrlId + 'btSelectAcc');
    if (parPracId != null && parIndex != null && bt != null) {
        parPracId.value = pracId;
        parIndex.value = index;
        bt.Click();
        alert(0);
    }
    else 
        if (parPracId == null) alert(1);
        else if (parIndex == null) alert(2);
        else alert(3);

}



function x_selectCellPP_1(cell, index, dataid, zmid) {
    //alert(dataid);

    var data = document.getElementById(dataid);
    if (data != null) {
        //var p = dataid.indexOf('_');
        //var root = p > 0 ? dataid.substring(0, p) : dataid;
        //var zmiana = document.getElementById(root + '_hidSelZmiana');
        var zmiana = document.getElementById(zmid);
        if (zmiana != null) {
            var dv = data.value.split(',');
            var zm = dv[index].split('|');
            var h, c, i;
            //alert(dv[index] + '\n' + zmiana.value);
            if (dv[index] == zmiana.value || zmiana.value == '') {
                dv[index] = '';   //'||||'; 
                h = '';
                c = '';
            }
            else {
                dv[index] = zmiana.value;
                var zm = zmiana.value.split('|');
                h = zm[1];
                c = zm[2];
            }
            data.value = dv.join(',');
            var zm = zmiana.value.split('|');
            cell.innerHTML = h;
            cell.style.backgroundColor = c;
            cell.style.backgroundImage = c != '' ? 'none' : '';
        }
    }
}




//---------------------------------------------
function setClassById(id, css) {
    elem = document.getElementById(id);
    if (elem != null)
        elem.className = css;
       //elem.setAttribute('class', css); <<< to nie działa na IE - zmienia ale nie odświeża wyglądu !!!
}

function setDdlParentClass(ddl, hidId, cls) {
    if (ddl.selectedIndex == 0) {
        //$('#' + ddl.parentNode.id).removeClass('colWyjasnicRed');
        $(ddl.parentNode).removeClass(cls);
    }
    else {
        //$('#' + ddl.parentNode.id).addClass('colWyjasnicRed');
        $(ddl.parentNode).addClass(cls);
    }
    var hid = document.getElementById(hidId);
    if (hid != null) hid.value = $(ddl.parentNode).attr("class");
}

//---------------------------------------------
function showModal(form, settings) {
    var Args = window.showModalDialog(form, null, settings);
}

//---------------------------------------------
/*
String.prototype.startsWith = function(str) {
    return this.substr(0, str.length) == str;
}
*/

function startsWith(str, start) {
    //return str.substr(0, start.length).toLowerCase() == start;
    return str.substr(0, start.length) == start;
}

function endsWith(str, ends) {
    var p = str.length - ends.length;
    if (p >= 0)
        return str.substr(p, ends.length) == ends;
    else
        return false;
}

String.prototype.startsWith = function(prefix) {
    return this.indexOf(prefix) === 0;
}

String.prototype.endsWith = function(suffix) {
    return this.match(suffix+"$") == suffix;
};

//---------------------------------------------
function isMaxLen(textBox, maxLen) {   // multiline textbox fix
    return textBox.value.length < maxLen;
}

function cutMaxLen(textBox, maxLen) {   // multiline textbox fix
    window.setTimeout(                  // trick bo jeszcze nie widać zmian
        function() {
            if (textBox.value.length > maxLen) {
                textBox.value = textBox.value.substring(0, maxLen);  // index - index-1; tu nie moze byc alertu bo sie onblur wykona na ie
            }
        }
    , 1);    
}

function checkMaxLen(textBox, maxLen) {
    if (textBox.value.length > maxLen) {
        textBox.focus();
        alert("Wprowadzony tekst jest zbyt długi (ilość znaków: " + textBox.value.length + ").\nMaksymalna ilość znaków to: " + maxLen + ".");
        return false;
    }
    else
        return true;
}

//---------------------------------------------
function printAnkieta(form) {
    var settings = "center:yes;resizable:yes;status:no;dialogHeight:800px;dialogWidth:1050px;"
    /*
    form = 'PrintAnkietaForm.aspx?aid=42&p=Z';
    alert(form);
    */
    showModal(form, settings);
}

function printAnkieta3(form) {
    window.open(form, "_blank");
}

function printPreviewIE() {
    var OLECMDID = 7;
    /* OLECMDID values:
    * 6 – print
    * 7 – print preview
    * 1 – open window
    * 4 – Save As
    */

    var PROMPT = 1; // 2 DONTPROMPTUSER
    var WebBrowser = '<OBJECT ID="WebBrowser1" WIDTH=0 HEIGHT=0 CLASSID="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"></OBJECT>';

    document.body.insertAdjacentHTML('beforeEnd', WebBrowser);
    WebBrowser1.ExecWB(OLECMDID, PROMPT);
    WebBrowser1.outerHTML = "";
}

function printPreview() {
    var ver = getInternetExplorerVersion();
    if (ver > -1 && ver < 10)
        try {
        printPreviewIE();
    } catch (err) {
        window.print();
    }
    else
        window.print();
}

function printPrint() {
    window.print();
}
//---------------------------------------------

function toCSV(el, name) {        // header, report, footer ...
    var csvData = [];
    //var tag = el.tagName;
    //----- tabela -----
    if ($(el).is('table')) {
        //----- nagłówek tabeli colspan,rowspan -----
        var tb = $(el)[0];
        var r = tb.rows.length;        
        var c = 0;
        var h = 0;
        //----- rozmiar -----
        for (i = 0; i < r; i++) {
            var cells = tb.rows[i].cells; 
            if (cells[0].tagName == 'TH') {
                h++;
                var k = 0;
                for (j = 0; j < cells.length; j++)
                    k += cells[j].colSpan;
                if (k > c) c = k;
            }
            else break;
        }
        if (h > 0) {
            //----- tablica th -----
            var tmpHeader = new Array(h);
            for (var i = 0; i < h; i++) {
                tmpHeader[i] = new Array(c);
                for (j = 0; j < c; j++)
                    tmpHeader[i][j] = null;
            }
            //----- wypełnianie -----
            for (i = 0; i < h; i++) {
                var cells = tb.rows[i].cells;
                var xx = 0;
                for (j = 0; j < cells.length; j++) {
                    var cs = cells[j].colSpan;
                    var rs = cells[j].rowSpan;
                    var data = formatData(cells[j].innerHTML);
                    while (xx < c && tmpHeader[i][xx] != null) xx++;
                    for (ys = i; ys < rs + i && ys < h; ys++)
                        for (xs = xx; xs < cs + xx; xs++)
                            if (ys == i && xs == xx)
                                tmpHeader[ys][xs] = data;
                            else
                                tmpHeader[ys][xs] = "";
                    xx += cs;
                }
            }
            for (i = 0; i < h; i++)
                row2CSV(tmpHeader[i]);
        }
        //----- table.tbody -----
        //$(el).find('tr').each(
        $(el).children('tbody,thead').children('tr').each(
            function() {
                var tmpRow = [];
                var ret = null;
                if ($(this).hasClass("group"))
                    tmpRow[tmpRow.length] = '#group#';
                //$(this).filter(':visible').find('th,td').each(
                //$(this).children('th,td').each(    //---- bez th
                $(this).children('td').each(
                    function () {
                        if ($(this).css('display') != 'none') {
                            //----- nagłówki tabeli -----
                            if ($(this).is('th')) {
                                var t = $(this).children('table');      // tabela zagnieżdżona
                                if (t.length != 0) {
                                    ret = //'#table#\n' +
                                          toCSV(t, null);               // null bo tabela w tabeli zwraca podwojnie
                                    tmpRow[tmpRow.length] = '';
                                }
                                else
                                    tmpRow[tmpRow.length] = formatData($(this).html());
                            }
                            //----- pola tabeli -----
                            else if ($(this).is('td')) {
                                var cb = $(this).find('input[type=checkbox]');
                                if (cb.length != 0) {
                                    var checked = $(cb).is(':checked');
                                    tmpRow[tmpRow.length] = checked ? 'Tak' : 'Nie';
                                }
                                else {
                                    var tb = $(this).find('input[type=text]');
                                    if (tb.length != 0)
                                        tmpRow[tmpRow.length] = tb.val();
                                    else {
                                        //var t = $(this).find('table');
                                        var t = $(this).children('table');  // tabela zagnieżdżona
                                        if (t.length != 0) {
                                            ret = //'#table#\n' +
                                                  toCSV(t, null);           //null bo tabela w tabeli zwraca podwojnie
                                            return false;                   // nic wiecej nie ma w td, dodane do CSV w środku
                                        }
                                        else
                                            //----- karta roczna -----
                                            if ($(this).hasClass('day')) {
                                                var v = [];
                                                $(this).find('span').each(
                                                    function() {
                                                        if ($(this).css('display') != 'none') {
                                                            v[v.length] = formatData($(this).html());
                                                        }
                                                    });
                                                if (v.length > 0)
                                                    tmpRow[tmpRow.length] = jQuery.trim(v.join(' '));
                                                else
                                                    tmpRow[tmpRow.length] = formatData($(this).html());   // jakby nie wyszło ...
                                            } else
                                            //----- zawartość td -----
                                                tmpRow[tmpRow.length] = formatData($(this).html());
                                    }
                                }
                            }
                            //----- uzupełnienie na pusto -----
                            var cs = $(this).attr('colspan');
                            if (cs != null && cs > 1)
                                for (var k = 1; k < cs; k++)
                                    tmpRow[tmpRow.length] = "";
                        }
                    });
                if (ret == null)
                    row2CSV(tmpRow);
                else {
                    ret = ret.slice(0, -1);
                    add2CSV(ret);
                    if (tmpRow.length > 0) {
                        csvData[csvData.length] = ''; // 1 linia przerwy                    
                        row2CSV(tmpRow);
                    }
                }
            });
    }
    //----- div -----
    else if ($(el).is('div')) {
        $(el).find('span').each(
            function() {
                if ($(this).css('display') != 'none') {
                    var tmpRow = [];
                    tmpRow[tmpRow.length] = formatData($(this).html());
                    row2CSV(tmpRow);
                }
            });
    }
    //----- span -----
    else if ($(el).is('span')) {
        if ($(el).css('display') != 'none') {
            var tmpRow = [];
            tmpRow[tmpRow.length] = formatData($(el).html());
            row2CSV(tmpRow);
        }
    }    

    var mydata;
    if (csvData.length > 0)
        if (name != null)
            mydata = '[' + name + ']\n' + csvData.join('\n') + '\n';
        else
            mydata = csvData.join('\n') + '\n';
       //mydata = '[' + name + ']\r' + csvData.join('\r') + '\r';  // nie robi, po stronie serwera zawsze jest cr lf na końcu
    else
        mydata = '';
    return mydata;

    //----- funkcje dodatkowe ----------------------   
    function row2CSV(tmpRow) {
        var tmp = tmpRow.join('') // to remove any blank rows
        if (tmpRow.length > 0 && tmp != '') {
            var mystr = tmpRow.join('\t');
            csvData[csvData.length] = mystr;
        }
    }

    function add2CSV(row) {
        if (row != '') 
            csvData[csvData.length] = row;
    }

    function formatData(input) {
        /*
        var regexp = new RegExp(/["]/g);
        var output = input.replace(regexp, "“");
        */
        var regexp = new RegExp(/[\n] */g);
        var output = input.replace(regexp, "");             // cr lf i spacje z początku następnej linii ! uwaga input a nie output.replace
        
        //*
        regexp = new RegExp(/\<[Bb][Rr]\>/g);
        output = jQuery.trim(output.replace(regexp, "\b")); // <br> na bs ...    po stronie serwera będzie to zamieniane na crlf
        /**/
        /*
        regexp = new RegExp(/\<[Bb][Rr]\>/g);
        output = jQuery.trim(output.replace(regexp, " ")); // 
        /**/
        
        
        regexp = new RegExp(/\<[Ll][Ii]\>/g);
        output = jQuery.trim(output.replace(regexp, "• ")); // ul-li

        regexp = new RegExp(/\<script.*script\>/gi);
        output = jQuery.trim(output.replace(regexp, ""));   // javascript - KomisjaControl.ascx

        regexp = new RegExp(/\<[^\<]+\>/g);
        output = jQuery.trim(output.replace(regexp, ""));   // html tags

        //if (output == "") return '';
        //return '"' + output + '"'; 
        return output;
    }
}

function exportExcel(repId) {
    var rep = "";
    var title = "";
    //for (var i = 0; i < document.all.length; i++) {   // nie działa na FireFox
    var elems = document.getElementsByTagName("*");
    for (var i = 0; i < elems.length; i++) {
        var el = elems[i];
        //var name = el.name;   // nie dziala na chrome
        var name = $(el).attr('name');
        if (name != null && name != '') {
            if (startsWith(name, 'header') ||
                startsWith(name, 'report') ||
                startsWith(name, 'footer'))
                rep += toCSV(el, name);
            else if (title == '' && startsWith(name, 'title')) 
                title = $(el).text();
        }
    }    
    var t = document.getElementById(repId + 'Title');
    if (t != null) t.value = title;

    var h = document.getElementById(repId);
    //var h1 = document.getElementById(repId + '1');
    if (h != null) {
        //alert(repId);
        h.value = rep;      // przechowuje na postback
        /*
        if (h1 == null)         
            h.value = rep;      // przechowuje na postback
        else {
            var len = rep.length % 5;
            alert(rep.length);
            alert(len);
            alert(rep.length - len * 4);

            h.value = rep.substring(0, len);
            h1.value = rep.substring(len, len);
            h = document.getElementById(repId + '2');
            h.value = rep.substring(len * 2, len);
            h = document.getElementById(repId + '3');
            h.value = rep.substring(len * 3, len);
            h = document.getElementById(repId + '4');
            h.value = rep.substring(len * 4, rep.length - len * 4);

            alert(0);
        }
        */
        /*alert(rep);
        return false;/**/
        return true;
    }
    else {
        alert("Wystąpił błąd podczas eksportu danych.");
        return false;
    }
}
//--------------------------------------------
function getClientSize() {
    var cWidth = 0, cHeight = 0;
    if (typeof (window.innerWidth) == 'number') {
        //Non-IE
        cWidth = window.innerWidth;
        cHeight = window.innerHeight;
    } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
        //IE 6+ in 'standards compliant mode'
        cWidth = document.documentElement.clientWidth;
        cHeight = document.documentElement.clientHeight;
    } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
        //IE 4 compatible
        cWidth = document.body.clientWidth;
        cHeight = document.body.clientHeight;
    }
    return [cWidth, cHeight];
}

function hideFooter() {
    //console.log('hide');
    $('#footer').hide();
}

function showFooter_1() {
    var f = $('#footer');
    if (!f.is(':visible')) // removeAttr psuje przy przelaczaniu tabow
        f.fadeIn(250, function() { this.style.removeAttribute("filter"); });
    //$('#footer').fadeIn(250);
}

function showFooter() {
    var f = $('#footer');
    if (!f.is(':visible')) // removeAttr psuje przy przelaczaniu tabow
        f.fadeIn(250, function() {
            if (this.style.removeProperty)
                this.style.removeProperty("filter"); //!IE
            else
                this.style.removeAttribute("filter");
        });
    //$('#footer').fadeIn(250);
}




/* dla form bez Master */
function resize2() {
    var wh = getClientSize();
    var h = wh[1];
    if (h < 400) h = 400;

    var c = $('#content');
    c.height('auto');

    var ctop = c.offset().top;
    if (ctop + c.height() + 25 < h) {
        var h2 = h - ctop - 25;
        c.height(h2);
    }
    showFooter();
}

//---------------------------------------------------


/* cntSqlMenu3 */

/* Polyfill żeby pod chromem działało */

if (!String.prototype.contains) {
    String.prototype.contains = function(s) {
        return this.indexOf(s) > -1
    }
}

function prepareMenu3() {
    //$(function() {
    $('.cntSqlMenu3 .toggler').click(function() {
        $(this).parent().children('ul').toggle(300);
    });

    $('.hamburger').click(function() {
        $('td.menu').toggle();
    });

    var url = window.location.pathname;
    var name = url.substring(url.lastIndexOf('/') + 1) + window.location.search;
    var found = false;
    $(".cntSqlMenu3 a").each(function() {
        if ($(this).attr("href").contains(name) && !found) {
            found = true;
            $(this).toggleClass("sel");
            $(this).parents("li").find("a.toggler").parent().children('ul').toggle(0);
        }
    });
    //});
}

$(function() {
    //$(".datepicker").datepicker({
    //    dateFormat: "yy-mm-dd",
    //    firstDay: 1,

    //});

    //$(".datepickero").on("click", function() {
    //    $(this).parent().find(".datepicker").datepicker("show");
    //});
    //prepareCalendar();
});

function prepareCalendar() {
    $(".datepicker").datepicker({
        dateFormat: "yy-mm-dd",
        monthNames: ['Styczeń', 'Luty', 'Marzec', 'Kwiecień', 'Maj', 'Czerwiec', 'Lipiec', 'Sierpień', 'Wrzesień', 'Październik', 'Listopad', 'Grudzień'],
        /*dayNamesShort: ['Pn', 'Wt', 'Śr', 'Cz', 'Pt', 'Sb', 'Nd'],*/
        /*dayNamesMin: ['Pn', 'Wt', 'Śr', 'Cz', 'Pt', 'Sb', 'Nd'],*/
        dayNamesShort: ['Nd', 'Pn', 'Wt', 'Śr', 'Cz', 'Pt', 'Sb'],
        dayNamesMin: ['Nd', 'Pn', 'Wt', 'Śr', 'Cz', 'Pt', 'Sb'],
        firstDay: 1
    });

    $(".datepickero").on("click", function() {
        $(this).parent().find(".datepicker").datepicker( "show" );
    });
}

function prepareCalendar2(id, options) {
    preventDatePickerChange = true;
    $('#' + id + ' .datepicker').datepicker(
        options
    ).on('changeDate', function (ev) {
        $(this).blur();
        $(this).datepicker('hide');
    });
    preventDatePickerChange = false;
}

/* cntModalPopup2 */
var mp2 = {
    $popup: null,
    showPopup: function($pop) {
        //        $popupBg = $("#" + popupBg);
                $popup = $("#" + $pop);
//        $popup = $pop;
        $popup.show();
        $("html").css("overflow", "hidden");
        $("body").append("<div class='popupBg'></div>");

        $popup.find(".popupCloser").on("click", function() {
            mp2.closePopup();
        });
        //        $popupBg.on("click", function() {
        //            mp2.closePopup();
        //        });

        $(window).keyup(function(e) {
            var key = e.which;
            if (key == 27) {
                mp2.closePopup();
            }
        });
    },
    closePopup: function() {
        $("html").css("overflow", "auto");
        $popup.hide();
        $(".popupBg").remove();
        //$popupBg.hide();
    }
}

$(function () {
    $("[data-toggle='popup']").on("click", function() {
        var sel = $(this).attr("popup-selector");
        var $p = $(sel);
        mp2.showPopup($p);

    });
});

/*
function showPopup(popupBg, popup) {
    var $popupBg = $("#" + popupBg);
    var $popup = $("#" + popup);

    $popupBg.show();
    $popup.show();
    $("html").css("overflow", "hidden");

    $popup.find(".popupCloser").on("click", function() {
        $popup.hide();
        $popupBg.hide();
        $("html").css("overflow", "auto");
    });
    $popupBg.on("click", function() {
        $popup.hide();
        $popupBg.hide();
        $("html").css("overflow", "auto");
    });

    $(window).keyup(function(e) {
        var key = e.which;
        if (key == 27) {
            $popup.hide();
            $popupBg.hide();
            $("html").css("overflow", "auto");
        }
    });
}
*/

/*----- Master Page 3 -----*/
function resize3() {
    var wh = getClientSize();
    var h = wh[1];
    if (h < 400) h = 400;

    var c = $('#content');
    c.height('auto');

    var ctop = c.offset().top;
    if (ctop + c.height() + 30 < h) {
        var h2 = h - ctop - 30;
        c.height(h2);

        var b = $('body');
        b.height(h2 + 30);

        var f = $('form');
        b.height(h2 + 30);
    }
    showFooter();
}

/*----- Start Form - InfoBox -----*/
function UpdateInfoBox(id, paId, cnt1Id, cnt2Id) {
    showAjaxProgress();
    callAjax2(svcUrl, 'GetInfoBoxContent', { id: id },
        function (r) {
            hideAjaxProgress();
            if (r.d) {
                var data = JSON.parse(r.d);
                /*
                alert(r.d);
                alert(data.html1);
                alert(data.html2);
                */
                if (data.html1 == null && data.html2 == null) {
                    //var cnt = document.getElementById(paId);
                    //cnt.style = "display: none;";
                    $('#' + paId).animate({ width: '0px' }, 500).hide(200, function () { resize(); });
                } else {
                    var cnt1 = document.getElementById(cnt1Id);
                    if (cnt1 != null) cnt1.innerHTML = data.html1;
                    var cnt2 = document.getElementById(cnt2Id);
                    if (cnt2 != null) cnt2.innerHTML = data.html2;
                    var fn = "iboxPrepare_" + data.id;
                    eval("if (typeof " + fn + " === 'function') " + fn + "();");
                }
            }
        }
    );
}

/*----- voice html5 -----*/
var recActive = false;

function startRecording(tbId, formId) {
    if (window.hasOwnProperty('webkitSpeechRecognition')) {
        recActive = true;

        var recognition = new webkitSpeechRecognition();

        recognition.continuous = false;
        recognition.interimResults = false;

        //recognition.lang = "en-US";
        recognition.lang = "pl-PL";
        recognition.start();

        recognition.onresult = function (e) {
            document.getElementById(tbId).value = e.results[0][0].transcript;
            recognition.stop();
            var f = document.getElementById(formId);
            if (f != null) f.submit();
        };

        recognition.onerror = function (e) {
            recognition.stop();
        }
    }
    else {
        alert("Przeglądarka nie wspiera rozpoznawania mowy.");
    }
}

/*----- modal -----*/
function prepareModal(id, options) {
    //$(document).on('show.bs.modal', /*'.modal'*/id, function () {
    //console.log('modal show' + $(this).attr('id'));
    //console.log($('.modal:visible').length);
    //console.log($('.modal:visible').attr('id'));

    if ($('#' + id + ':visible').length > 0) {

    } else {

        //console.log('Modal to show length: ' + $('#' + id).length);


        //console.log('PREEEE modal visible length: ' + $('.modal:visible').length);

        var zIndex = 1040 + (10 * $('.modal:visible').length);
        $('#' + id).modal(options);

        //console.log('modal visible length: ' + $('.modal:visible').length);
        //console.log('modal visible controls:');
        //console.log($('.modal:visible'));

     //   $(document).on('shown.bs.modal', '#' + id, function () {

            //console.log("###SHOW NEW MODAL###");
            //console.log("ID: " + id);
            //console.log("Z-INDEX: " + zIndex);

            //console.log($(this));
            //setTimeout(function () {
            //  console.log(zIndex);
            //console.log("BACKGROUND COUNT: " + $('.modal-backdrop').not('.modal-stack').length);
            $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).attr('id', id + 'back').addClass('modal-stack');
            $('#' + id).css('z-index', zIndex);
            //}, 0);
       // });
    }

    //setTimeout(function () {
    //    console.log(zIndex);
    //    console.log(":(" + $('.modal-backdrop').not('.modal-stack').length);
    //    $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).attr('id', id + 'back').addClass('modal-stack');
    //}, 0);
}

$(document).on('hidden.bs.modal', '.modal', function () {
    $('.modal:visible').length && $(document.body).addClass('modal-open');
});

function cbSelectAllRight(cb) {
    var checked = cb.checked;
    $(".cbSelectRight input[type=checkbox]").prop('checked', checked);//.closest('tr').children('td').addClass('selPrac');
}

function preparePP() {

    $("input[id$='cbPrac']:checked").each(function () {
        $(this).closest('tr').children('td').addClass('selPrac');
    });

    $("input[id$='cbDay']").each(function () {
        //1723

        if ($(this).is(':checked')) {
            var th = $(this).closest('th');
            var idx = parseInt($(this).parent().attr('idx'));  // cb jest w span
            $('table.tbPlanPracy').column(idx + 5).addClass('selDay');
        }
        //th.addClass('selDay');
    });
}

function ValidateCheckBox(sender, args) {
    var $validator = $(sender);
    var $checkbox = $validator.parent().find("input[type=checkbox]");
    if ($checkbox.prop('checked')) {
        args.IsValid = true;
    } else {
        args.IsValid = false;
    }
}



/////////////////////////////////////////////////////////////
/////////////////////CONTROLS////////////////////////////////
/////////////////////////////////////////////////////////////

function cntNagodzinyWnioskiModal() {
    $('.emp-Wnioski input[type=checkbox]').on('click', function () {
        var that = $(this);
        var checked = that.prop('checked');
        that.parents("tr").toggleClass("danger", !checked);
    });
}


/* COLOR PICKER */

function prepareColorPicker(hidId) {
    var hidColor = $('#' + hidId);
    var color = hidColor.val();
    //console.log(color);
    var preview = $(".zmiana .symbol.edit");

    if (color != null && color != undefined) {
        //console.log('asd');
        preview.css('background-color', color);
    }

    //console.log($(".zmiana .symbol.edit"));
    $('#cp4').colorpicker().on('changeColor', function (e) {

      //  document.getElementById(hidId).value = e.color.toHex();
       hidColor.val(e.color.toHex());

        preview.css("background-color", e.color.toHex());
    });
    //console.log();

    //var control = $("#" + id);
    //jscolor.hidePicker();
    //jscolor.init2(control[0]);
    var value = $(".tbSymbol input").val();
    var $spanVal = $(".zmiana .symbol.edit").text(value);
    $(".tbSymbol input").on('keyup', function(){
        var val = $(this).val();
        var $span = $(".zmiana .symbol.edit").text(val);
    });
}

//autocomplete
!function (a) { var b = "0.9.3", c = { isMsie: function () { var a = /(msie) ([\w.]+)/i.exec(navigator.userAgent); return a ? parseInt(a[2], 10) : !1 }, isBlankString: function (a) { return !a || /^\s*$/.test(a) }, escapeRegExChars: function (a) { return a.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&") }, isString: function (a) { return "string" == typeof a }, isNumber: function (a) { return "number" == typeof a }, isArray: a.isArray, isFunction: a.isFunction, isObject: a.isPlainObject, isUndefined: function (a) { return "undefined" == typeof a }, bind: a.proxy, bindAll: function (b) { var c; for (var d in b) a.isFunction(c = b[d]) && (b[d] = a.proxy(c, b)) }, indexOf: function (a, b) { for (var c = 0; c < a.length; c++) if (a[c] === b) return c; return -1 }, each: a.each, map: a.map, filter: a.grep, every: function (b, c) { var d = !0; return b ? (a.each(b, function (a, e) { return (d = c.call(null, e, a, b)) ? void 0 : !1 }), !!d) : d }, some: function (b, c) { var d = !1; return b ? (a.each(b, function (a, e) { return (d = c.call(null, e, a, b)) ? !1 : void 0 }), !!d) : d }, mixin: a.extend, getUniqueId: function () { var a = 0; return function () { return a++ } }(), defer: function (a) { setTimeout(a, 0) }, debounce: function (a, b, c) { var d, e; return function () { var f, g, h = this, i = arguments; return f = function () { d = null, c || (e = a.apply(h, i)) }, g = c && !d, clearTimeout(d), d = setTimeout(f, b), g && (e = a.apply(h, i)), e } }, throttle: function (a, b) { var c, d, e, f, g, h; return g = 0, h = function () { g = new Date, e = null, f = a.apply(c, d) }, function () { var i = new Date, j = b - (i - g); return c = this, d = arguments, 0 >= j ? (clearTimeout(e), e = null, g = i, f = a.apply(c, d)) : e || (e = setTimeout(h, j)), f } }, tokenizeQuery: function (b) { return a.trim(b).toLowerCase().split(/[\s]+/) }, tokenizeText: function (b) { return a.trim(b).toLowerCase().split(/[\s\-_]+/) }, getProtocol: function () { return location.protocol }, noop: function () { } }, d = function () { var a = /\s+/; return { on: function (b, c) { var d; if (!c) return this; for (this._callbacks = this._callbacks || {}, b = b.split(a) ; d = b.shift() ;) this._callbacks[d] = this._callbacks[d] || [], this._callbacks[d].push(c); return this }, trigger: function (b, c) { var d, e; if (!this._callbacks) return this; for (b = b.split(a) ; d = b.shift() ;) if (e = this._callbacks[d]) for (var f = 0; f < e.length; f += 1) e[f].call(this, { type: d, data: c }); return this } } }(), e = function () { function b(b) { b && b.el || a.error("EventBus initialized without el"), this.$el = a(b.el) } var d = "typeahead:"; return c.mixin(b.prototype, { trigger: function (a) { var b = [].slice.call(arguments, 1); this.$el.trigger(d + a, b) } }), b }(), f = function () { function a(a) { this.prefix = ["__", a, "__"].join(""), this.ttlKey = "__ttl__", this.keyMatcher = new RegExp("^" + this.prefix) } function b() { return (new Date).getTime() } function d(a) { return JSON.stringify(c.isUndefined(a) ? null : a) } function e(a) { return JSON.parse(a) } var f, g; try { f = window.localStorage, f.setItem("~~~", "!"), f.removeItem("~~~") } catch (h) { f = null } return g = f && window.JSON ? { _prefix: function (a) { return this.prefix + a }, _ttlKey: function (a) { return this._prefix(a) + this.ttlKey }, get: function (a) { return this.isExpired(a) && this.remove(a), e(f.getItem(this._prefix(a))) }, set: function (a, e, g) { return c.isNumber(g) ? f.setItem(this._ttlKey(a), d(b() + g)) : f.removeItem(this._ttlKey(a)), f.setItem(this._prefix(a), d(e)) }, remove: function (a) { return f.removeItem(this._ttlKey(a)), f.removeItem(this._prefix(a)), this }, clear: function () { var a, b, c = [], d = f.length; for (a = 0; d > a; a++) (b = f.key(a)).match(this.keyMatcher) && c.push(b.replace(this.keyMatcher, "")); for (a = c.length; a--;) this.remove(c[a]); return this }, isExpired: function (a) { var d = e(f.getItem(this._ttlKey(a))); return c.isNumber(d) && b() > d ? !0 : !1 } } : { get: c.noop, set: c.noop, remove: c.noop, clear: c.noop, isExpired: c.noop }, c.mixin(a.prototype, g), a }(), g = function () { function a(a) { c.bindAll(this), a = a || {}, this.sizeLimit = a.sizeLimit || 10, this.cache = {}, this.cachedKeysByAge = [] } return c.mixin(a.prototype, { get: function (a) { return this.cache[a] }, set: function (a, b) { var c; this.cachedKeysByAge.length === this.sizeLimit && (c = this.cachedKeysByAge.shift(), delete this.cache[c]), this.cache[a] = b, this.cachedKeysByAge.push(a) } }), a }(), h = function () { function b(a) { c.bindAll(this), a = c.isString(a) ? { url: a } : a, i = i || new g, h = c.isNumber(a.maxParallelRequests) ? a.maxParallelRequests : h || 6, this.url = a.url, this.wildcard = a.wildcard || "%QUERY", this.filter = a.filter, this.replace = a.replace, this.ajaxSettings = { type: "get", cache: a.cache, timeout: a.timeout, dataType: a.dataType || "json", beforeSend: a.beforeSend }, this._get = (/^throttle$/i.test(a.rateLimitFn) ? c.throttle : c.debounce)(this._get, a.rateLimitWait || 300) } function d() { j++ } function e() { j-- } function f() { return h > j } var h, i, j = 0, k = {}; return c.mixin(b.prototype, { _get: function (a, b) { function c(c) { var e = d.filter ? d.filter(c) : c; b && b(e), i.set(a, c) } var d = this; f() ? this._sendRequest(a).done(c) : this.onDeckRequestArgs = [].slice.call(arguments, 0) }, _sendRequest: function (b) { function c() { e(), k[b] = null, f.onDeckRequestArgs && (f._get.apply(f, f.onDeckRequestArgs), f.onDeckRequestArgs = null) } var f = this, g = k[b]; return g || (d(), g = k[b] = a.ajax(b, this.ajaxSettings).always(c)), g }, get: function (a, b) { var d, e, f = this, g = encodeURIComponent(a || ""); return b = b || c.noop, d = this.replace ? this.replace(this.url, g) : this.url.replace(this.wildcard, g), (e = i.get(d)) ? c.defer(function () { b(f.filter ? f.filter(e) : e) }) : this._get(d, b), !!e } }), b }(), i = function () { function d(b) { c.bindAll(this), c.isString(b.template) && !b.engine && a.error("no template engine specified"), b.local || b.prefetch || b.remote || a.error("one of local, prefetch, or remote is required"), this.name = b.name || c.getUniqueId(), this.limit = b.limit || 5, this.minLength = b.minLength || 1, this.header = b.header, this.footer = b.footer, this.valueKey = b.valueKey || "value", this.template = e(b.template, b.engine, this.valueKey), this.local = b.local, this.prefetch = b.prefetch, this.remote = b.remote, this.itemHash = {}, this.adjacencyList = {}, this.storage = b.name ? new f(b.name) : null } function e(a, b, d) { var e, f; return c.isFunction(a) ? e = a : c.isString(a) ? (f = b.compile(a), e = c.bind(f.render, f)) : e = function (a) { return "<p>" + a[d] + "</p>" }, e } var g = { thumbprint: "thumbprint", protocol: "protocol", itemHash: "itemHash", adjacencyList: "adjacencyList" }; return c.mixin(d.prototype, { _processLocalData: function (a) { this._mergeProcessedData(this._processData(a)) }, _loadPrefetchData: function (d) { function e(a) { var b = d.filter ? d.filter(a) : a, e = m._processData(b), f = e.itemHash, h = e.adjacencyList; m.storage && (m.storage.set(g.itemHash, f, d.ttl), m.storage.set(g.adjacencyList, h, d.ttl), m.storage.set(g.thumbprint, n, d.ttl), m.storage.set(g.protocol, c.getProtocol(), d.ttl)), m._mergeProcessedData(e) } var f, h, i, j, k, l, m = this, n = b + (d.thumbprint || ""); return this.storage && (f = this.storage.get(g.thumbprint), h = this.storage.get(g.protocol), i = this.storage.get(g.itemHash), j = this.storage.get(g.adjacencyList)), k = f !== n || h !== c.getProtocol(), d = c.isString(d) ? { url: d } : d, d.ttl = c.isNumber(d.ttl) ? d.ttl : 864e5, i && j && !k ? (this._mergeProcessedData({ itemHash: i, adjacencyList: j }), l = a.Deferred().resolve()) : l = a.getJSON(d.url).done(e), l }, _transformDatum: function (a) { var b = c.isString(a) ? a : a[this.valueKey], d = a.tokens || c.tokenizeText(b), e = { value: b, tokens: d }; return c.isString(a) ? (e.datum = {}, e.datum[this.valueKey] = a) : e.datum = a, e.tokens = c.filter(e.tokens, function (a) { return !c.isBlankString(a) }), e.tokens = c.map(e.tokens, function (a) { return a.toLowerCase() }), e }, _processData: function (a) { var b = this, d = {}, e = {}; return c.each(a, function (a, f) { var g = b._transformDatum(f), h = c.getUniqueId(g.value); d[h] = g, c.each(g.tokens, function (a, b) { var d = b.charAt(0), f = e[d] || (e[d] = [h]); !~c.indexOf(f, h) && f.push(h) }) }), { itemHash: d, adjacencyList: e } }, _mergeProcessedData: function (a) { var b = this; c.mixin(this.itemHash, a.itemHash), c.each(a.adjacencyList, function (a, c) { var d = b.adjacencyList[a]; b.adjacencyList[a] = d ? d.concat(c) : c }) }, _getLocalSuggestions: function (a) { var b, d = this, e = [], f = [], g = []; return c.each(a, function (a, b) { var d = b.charAt(0); !~c.indexOf(e, d) && e.push(d) }), c.each(e, function (a, c) { var e = d.adjacencyList[c]; return e ? (f.push(e), (!b || e.length < b.length) && (b = e), void 0) : !1 }), f.length < e.length ? [] : (c.each(b, function (b, e) { var h, i, j = d.itemHash[e]; h = c.every(f, function (a) { return ~c.indexOf(a, e) }), i = h && c.every(a, function (a) { return c.some(j.tokens, function (b) { return 0 === b.indexOf(a) }) }), i && g.push(j) }), g) }, initialize: function () { var b; return this.local && this._processLocalData(this.local), this.transport = this.remote ? new h(this.remote) : null, b = this.prefetch ? this._loadPrefetchData(this.prefetch) : a.Deferred().resolve(), this.local = this.prefetch = this.remote = null, this.initialize = function () { return b }, b }, getSuggestions: function (a, b) { function d(a) { f = f.slice(0), c.each(a, function (a, b) { var d, e = g._transformDatum(b); return d = c.some(f, function (a) { return e.value === a.value }), !d && f.push(e), f.length < g.limit }), b && b(f) } var e, f, g = this, h = !1; a.length < this.minLength || (e = c.tokenizeQuery(a), f = this._getLocalSuggestions(e).slice(0, this.limit), f.length < this.limit && this.transport && (h = this.transport.get(a, d)), !h && b && b(f)) } }), d }(), j = function () { function b(b) { var d = this; c.bindAll(this), this.specialKeyCodeMap = { 9: "tab", 27: "esc", 37: "left", 39: "right", 13: "enter", 38: "up", 40: "down" }, this.$hint = a(b.hint), this.$input = a(b.input).on("blur.tt", this._handleBlur).on("focus.tt", this._handleFocus).on("keydown.tt", this._handleSpecialKeyEvent), c.isMsie() ? this.$input.on("keydown.tt keypress.tt cut.tt paste.tt", function (a) { d.specialKeyCodeMap[a.which || a.keyCode] || c.defer(d._compareQueryToInputValue) }) : this.$input.on("input.tt", this._compareQueryToInputValue), this.query = this.$input.val(), this.$overflowHelper = e(this.$input) } function e(b) { return a("<span></span>").css({ position: "absolute", left: "-9999px", visibility: "hidden", whiteSpace: "nowrap", fontFamily: b.css("font-family"), fontSize: b.css("font-size"), fontStyle: b.css("font-style"), fontVariant: b.css("font-variant"), fontWeight: b.css("font-weight"), wordSpacing: b.css("word-spacing"), letterSpacing: b.css("letter-spacing"), textIndent: b.css("text-indent"), textRendering: b.css("text-rendering"), textTransform: b.css("text-transform") }).insertAfter(b) } function f(a, b) { return a = (a || "").replace(/^\s*/g, "").replace(/\s{2,}/g, " "), b = (b || "").replace(/^\s*/g, "").replace(/\s{2,}/g, " "), a === b } return c.mixin(b.prototype, d, { _handleFocus: function () { this.trigger("focused") }, _handleBlur: function () { this.trigger("blured") }, _handleSpecialKeyEvent: function (a) { var b = this.specialKeyCodeMap[a.which || a.keyCode]; b && this.trigger(b + "Keyed", a) }, _compareQueryToInputValue: function () { var a = this.getInputValue(), b = f(this.query, a), c = b ? this.query.length !== a.length : !1; c ? this.trigger("whitespaceChanged", { value: this.query }) : b || this.trigger("queryChanged", { value: this.query = a }) }, destroy: function () { this.$hint.off(".tt"), this.$input.off(".tt"), this.$hint = this.$input = this.$overflowHelper = null }, focus: function () { this.$input.focus() }, blur: function () { this.$input.blur() }, getQuery: function () { return this.query }, setQuery: function (a) { this.query = a }, getInputValue: function () { return this.$input.val() }, setInputValue: function (a, b) { this.$input.val(a), !b && this._compareQueryToInputValue() }, getHintValue: function () { return this.$hint.val() }, setHintValue: function (a) { this.$hint.val(a) }, getLanguageDirection: function () { return (this.$input.css("direction") || "ltr").toLowerCase() }, isOverflow: function () { return this.$overflowHelper.text(this.getInputValue()), this.$overflowHelper.width() > this.$input.width() }, isCursorAtEnd: function () { var a, b = this.$input.val().length, d = this.$input[0].selectionStart; return c.isNumber(d) ? d === b : document.selection ? (a = document.selection.createRange(), a.moveStart("character", -b), b === a.text.length) : !0 } }), b }(), k = function () { function b(b) { c.bindAll(this), this.isOpen = !1, this.isEmpty = !0, this.isMouseOverDropdown = !1, this.$menu = a(b.menu).on("mouseenter.tt", this._handleMouseenter).on("mouseleave.tt", this._handleMouseleave).on("click.tt", ".tt-suggestion", this._handleSelection).on("mouseover.tt", ".tt-suggestion", this._handleMouseover) } function e(a) { return a.data("suggestion") } var f = { suggestionsList: '<span class="tt-suggestions"></span>' }, g = { suggestionsList: { display: "block" }, suggestion: { whiteSpace: "nowrap", cursor: "pointer" }, suggestionChild: { whiteSpace: "normal" } }; return c.mixin(b.prototype, d, { _handleMouseenter: function () { this.isMouseOverDropdown = !0 }, _handleMouseleave: function () { this.isMouseOverDropdown = !1 }, _handleMouseover: function (b) { var c = a(b.currentTarget); this._getSuggestions().removeClass("tt-is-under-cursor"), c.addClass("tt-is-under-cursor") }, _handleSelection: function (b) { var c = a(b.currentTarget); this.trigger("suggestionSelected", e(c)) }, _show: function () { this.$menu.css("display", "block") }, _hide: function () { this.$menu.hide() }, _moveCursor: function (a) { var b, c, d, f; if (this.isVisible()) { if (b = this._getSuggestions(), c = b.filter(".tt-is-under-cursor"), c.removeClass("tt-is-under-cursor"), d = b.index(c) + a, d = (d + 1) % (b.length + 1) - 1, -1 === d) return this.trigger("cursorRemoved"), void 0; -1 > d && (d = b.length - 1), f = b.eq(d).addClass("tt-is-under-cursor"), this._ensureVisibility(f), this.trigger("cursorMoved", e(f)) } }, _getSuggestions: function () { return this.$menu.find(".tt-suggestions > .tt-suggestion") }, _ensureVisibility: function (a) { var b = this.$menu.height() + parseInt(this.$menu.css("paddingTop"), 10) + parseInt(this.$menu.css("paddingBottom"), 10), c = this.$menu.scrollTop(), d = a.position().top, e = d + a.outerHeight(!0); 0 > d ? this.$menu.scrollTop(c + d) : e > b && this.$menu.scrollTop(c + (e - b)) }, destroy: function () { this.$menu.off(".tt"), this.$menu = null }, isVisible: function () { return this.isOpen && !this.isEmpty }, closeUnlessMouseIsOverDropdown: function () { this.isMouseOverDropdown || this.close() }, close: function () { this.isOpen && (this.isOpen = !1, this.isMouseOverDropdown = !1, this._hide(), this.$menu.find(".tt-suggestions > .tt-suggestion").removeClass("tt-is-under-cursor"), this.trigger("closed")) }, open: function () { this.isOpen || (this.isOpen = !0, !this.isEmpty && this._show(), this.trigger("opened")) }, setLanguageDirection: function (a) { var b = { left: "0", right: "auto" }, c = { left: "auto", right: " 0" }; "ltr" === a ? this.$menu.css(b) : this.$menu.css(c) }, moveCursorUp: function () { this._moveCursor(-1) }, moveCursorDown: function () { this._moveCursor(1) }, getSuggestionUnderCursor: function () { var a = this._getSuggestions().filter(".tt-is-under-cursor").first(); return a.length > 0 ? e(a) : null }, getFirstSuggestion: function () { var a = this._getSuggestions().first(); return a.length > 0 ? e(a) : null }, renderSuggestions: function (b, d) { var e, h, i, j, k, l = "tt-dataset-" + b.name, m = '<div class="tt-suggestion">%body</div>', n = this.$menu.find("." + l); 0 === n.length && (h = a(f.suggestionsList).css(g.suggestionsList), n = a("<div></div>").addClass(l).append(b.header).append(h).append(b.footer).appendTo(this.$menu)), d.length > 0 ? (this.isEmpty = !1, this.isOpen && this._show(), i = document.createElement("div"), j = document.createDocumentFragment(), c.each(d, function (c, d) { d.dataset = b.name, e = b.template(d.datum), i.innerHTML = m.replace("%body", e), k = a(i.firstChild).css(g.suggestion).data("suggestion", d), k.children().each(function () { a(this).css(g.suggestionChild) }), j.appendChild(k[0]) }), n.show().find(".tt-suggestions").html(j)) : this.clearSuggestions(b.name), this.trigger("suggestionsRendered") }, clearSuggestions: function (a) { var b = a ? this.$menu.find(".tt-dataset-" + a) : this.$menu.find('[class^="tt-dataset-"]'), c = b.find(".tt-suggestions"); b.hide(), c.empty(), 0 === this._getSuggestions().length && (this.isEmpty = !0, this._hide()) } }), b }(), l = function () { function b(a) { var b, d, f; c.bindAll(this), this.$node = e(a.input), this.datasets = a.datasets, this.dir = null, this.eventBus = a.eventBus, b = this.$node.find(".tt-dropdown-menu"), d = this.$node.find(".tt-query"), f = this.$node.find(".tt-hint"), this.dropdownView = new k({ menu: b }).on("suggestionSelected", this._handleSelection).on("cursorMoved", this._clearHint).on("cursorMoved", this._setInputValueToSuggestionUnderCursor).on("cursorRemoved", this._setInputValueToQuery).on("cursorRemoved", this._updateHint).on("suggestionsRendered", this._updateHint).on("opened", this._updateHint).on("closed", this._clearHint).on("opened closed", this._propagateEvent), this.inputView = new j({ input: d, hint: f }).on("focused", this._openDropdown).on("blured", this._closeDropdown).on("blured", this._setInputValueToQuery).on("enterKeyed tabKeyed", this._handleSelection).on("queryChanged", this._clearHint).on("queryChanged", this._clearSuggestions).on("queryChanged", this._getSuggestions).on("whitespaceChanged", this._updateHint).on("queryChanged whitespaceChanged", this._openDropdown).on("queryChanged whitespaceChanged", this._setLanguageDirection).on("escKeyed", this._closeDropdown).on("escKeyed", this._setInputValueToQuery).on("tabKeyed upKeyed downKeyed", this._managePreventDefault).on("upKeyed downKeyed", this._moveDropdownCursor).on("upKeyed downKeyed", this._openDropdown).on("tabKeyed leftKeyed rightKeyed", this._autocomplete) } function e(b) { var c = a(g.wrapper), d = a(g.dropdown), e = a(b), f = a(g.hint); c = c.css(h.wrapper), d = d.css(h.dropdown), f.css(h.hint).css({ backgroundAttachment: e.css("background-attachment"), backgroundClip: e.css("background-clip"), backgroundColor: e.css("background-color"), backgroundImage: e.css("background-image"), backgroundOrigin: e.css("background-origin"), backgroundPosition: e.css("background-position"), backgroundRepeat: e.css("background-repeat"), backgroundSize: e.css("background-size") }), e.data("ttAttrs", { dir: e.attr("dir"), autocomplete: e.attr("autocomplete"), spellcheck: e.attr("spellcheck"), style: e.attr("style") }), e.addClass("tt-query").attr({ autocomplete: "off", spellcheck: !1 }).css(h.query); try { !e.attr("dir") && e.attr("dir", "auto") } catch (i) { } return e.wrap(c).parent().prepend(f).append(d) } function f(a) { var b = a.find(".tt-query"); c.each(b.data("ttAttrs"), function (a, d) { c.isUndefined(d) ? b.removeAttr(a) : b.attr(a, d) }), b.detach().removeData("ttAttrs").removeClass("tt-query").insertAfter(a), a.remove() } var g = { wrapper: '<span class="twitter-typeahead"></span>', hint: '<input class="tt-hint" type="text" autocomplete="off" spellcheck="off" disabled>', dropdown: '<span class="tt-dropdown-menu"></span>' }, h = { wrapper: { position: "relative", display: "inline-block" }, hint: { position: "absolute", top: "0", left: "0", borderColor: "transparent", boxShadow: "none" }, query: { position: "relative", verticalAlign: "top", backgroundColor: "transparent" }, dropdown: { position: "absolute", top: "100%", left: "0", zIndex: "100", display: "none" } }; return c.isMsie() && c.mixin(h.query, { backgroundImage: "url(data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7)" }), c.isMsie() && c.isMsie() <= 7 && (c.mixin(h.wrapper, { display: "inline", zoom: "1" }), c.mixin(h.query, { marginTop: "-1px" })), c.mixin(b.prototype, d, { _managePreventDefault: function (a) { var b, c, d = a.data, e = !1; switch (a.type) { case "tabKeyed": b = this.inputView.getHintValue(), c = this.inputView.getInputValue(), e = b && b !== c; break; case "upKeyed": case "downKeyed": e = !d.shiftKey && !d.ctrlKey && !d.metaKey } e && d.preventDefault() }, _setLanguageDirection: function () { var a = this.inputView.getLanguageDirection(); a !== this.dir && (this.dir = a, this.$node.css("direction", a), this.dropdownView.setLanguageDirection(a)) }, _updateHint: function () { var a, b, d, e, f, g = this.dropdownView.getFirstSuggestion(), h = g ? g.value : null, i = this.dropdownView.isVisible(), j = this.inputView.isOverflow(); h && i && !j && (a = this.inputView.getInputValue(), b = a.replace(/\s{2,}/g, " ").replace(/^\s+/g, ""), d = c.escapeRegExChars(b), e = new RegExp("^(?:" + d + ")(.*$)", "i"), f = e.exec(h), this.inputView.setHintValue(a + (f ? f[1] : ""))) }, _clearHint: function () { this.inputView.setHintValue("") }, _clearSuggestions: function () { this.dropdownView.clearSuggestions() }, _setInputValueToQuery: function () { this.inputView.setInputValue(this.inputView.getQuery()) }, _setInputValueToSuggestionUnderCursor: function (a) { var b = a.data; this.inputView.setInputValue(b.value, !0) }, _openDropdown: function () { this.dropdownView.open() }, _closeDropdown: function (a) { this.dropdownView["blured" === a.type ? "closeUnlessMouseIsOverDropdown" : "close"]() }, _moveDropdownCursor: function (a) { var b = a.data; b.shiftKey || b.ctrlKey || b.metaKey || this.dropdownView["upKeyed" === a.type ? "moveCursorUp" : "moveCursorDown"]() }, _handleSelection: function (a) { var b = "suggestionSelected" === a.type, d = b ? a.data : this.dropdownView.getSuggestionUnderCursor(); d && (this.inputView.setInputValue(d.value), b ? this.inputView.focus() : a.data.preventDefault(), b && c.isMsie() ? c.defer(this.dropdownView.close) : this.dropdownView.close(), this.eventBus.trigger("selected", d.datum, d.dataset)) }, _getSuggestions: function () { var a = this, b = this.inputView.getQuery(); c.isBlankString(b) || c.each(this.datasets, function (c, d) { d.getSuggestions(b, function (c) { b === a.inputView.getQuery() && a.dropdownView.renderSuggestions(d, c) }) }) }, _autocomplete: function (a) { var b, c, d, e, f; ("rightKeyed" !== a.type && "leftKeyed" !== a.type || (b = this.inputView.isCursorAtEnd(), c = "ltr" === this.inputView.getLanguageDirection() ? "leftKeyed" === a.type : "rightKeyed" === a.type, b && !c)) && (d = this.inputView.getQuery(), e = this.inputView.getHintValue(), "" !== e && d !== e && (f = this.dropdownView.getFirstSuggestion(), this.inputView.setInputValue(f.value), this.eventBus.trigger("autocompleted", f.datum, f.dataset))) }, _propagateEvent: function (a) { this.eventBus.trigger(a.type) }, destroy: function () { this.inputView.destroy(), this.dropdownView.destroy(), f(this.$node), this.$node = null }, setQuery: function (a) { this.inputView.setQuery(a), this.inputView.setInputValue(a), this._clearHint(), this._clearSuggestions(), this._getSuggestions() } }), b }(); !function () { var b, d = {}, f = "ttView"; b = { initialize: function (b) { function g() { var b, d = a(this), g = new e({ el: d }); b = c.map(h, function (a) { return a.initialize() }), d.data(f, new l({ input: d, eventBus: g = new e({ el: d }), datasets: h })), a.when.apply(a, b).always(function () { c.defer(function () { g.trigger("initialized") }) }) } var h; return b = c.isArray(b) ? b : [b], 0 === b.length && a.error("no datasets provided"), h = c.map(b, function (a) { var b = d[a.name] ? d[a.name] : new i(a); return a.name && (d[a.name] = b), b }), this.each(g) }, destroy: function () { function b() { var b = a(this), c = b.data(f); c && (c.destroy(), b.removeData(f)) } return this.each(b) }, setQuery: function (b) { function c() { var c = a(this).data(f); c && c.setQuery(b) } return this.each(c) } }, jQuery.fn.typeahead = function (a) { return b[a] ? b[a].apply(this, [].slice.call(arguments, 1)) : b.initialize.apply(this, arguments) } }() }(window.jQuery);
