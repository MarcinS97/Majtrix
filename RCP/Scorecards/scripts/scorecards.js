scHidden = true;

function loadSum(input, output) {
    sum = 0;
    $(input).each(function() {
        s = $(this).text();
        if (s == "") s = 0;
        sum += parseFloat2(s);
    });
    //jak coś 
    if (isNaN(sum)) sum = 0;
    //
    $(output).text(readable(sum));
    return sum;
}

function loadSumCh(input, output, ch) {
    sum = 0;
    $(input).each(function() {
        s = $(this).text();
        if (s == "") s = 0;
        sum += parseFloat2(s);
    });
    $(output).text(readable(sum) + ' ' + ch);
    return sum;
}

function loadSumInput(input, output) {
    sum = 0;
    $(input).each(function() {
        s = $(this).find("input:text").val();
        if (s == "") s = 0;
        sum += parseFloat2(s);
    });
    $(output).text(readable(sum));
    return sum;
}

function loadSumInputLabel(input, output) {
    sum = 0;
    $(input).each(function() {
        s = $(this).find("input:text").val();
        if (isNaN(parseFloat(s)) || s == "") { // tu była zmiana - parseFloat()
            s = $(this).find("span").text();
        }
        if (s == "") s = 0;
        sum += parseFloat2(s);
    });
    $(output).text(readable(sum));
    return sum;
}

function loadSumInputLabelCh(input, output, ch) {
    sum = 0;
    $(input).each(function() {
        s = $(this).find("input:text").val();
        if (isNaN(parseFloat(s)) || s == "") {
            s = $(this).find("span").text();
        }
        if (s == "") s = 0;
        sum += parseFloat2(s);
    });
    $(output).text(readable(sum) + ' ' + ch);
    return sum;
}

function parseFloat2(x) {
    if (x == null) return -1;
    x = x.toString();
    return parseFloat(x.replace(",", "."));
}

function readable(x) {
    return Math.round(x * 100) / 100;
}

function readableCh(x, ch) {
    return (Math.round(x * 100) / 100) + ch;
}

function getInputLabelValue($obj) {
    var $input = $obj.find("input:text");
    if ($input.length > 0) {
        if ($input.val() != "")
            return $input.val();
    }
    else {
        $label = $obj.find("span");
        if ($label.length > 0) {
            if ($label.text() != "")
                return $label.text();
        }
    }
    return 0;
}

function getLabelValue($label) {
    if ($label.length > 0) {
        if ($label.text() != "" && $label.text() != null) {
            return $label.text();
        }
    }
    return 0;
}

function setText($element, value) {
    if ($element.find("span").length > 0) $element.find("span").text(value);
    else if ($element.find("a").length > 0) $element.find("a").text(value);
    else if ($element.find("input:text").length > 0) $element.find("input:text").val(value);
    else console.log("err");
}

function getSumAttr(inp, out, attr) {
    var sum = 0;
    $(inp).each(function() {
        s = $(this).attr(attr);
        if (s == "") s = 0;
        sum += parseFloat2(s);
    });
    $(out).text(readable(sum));
    return sum;

}

function getAttrValue(el, attr) {
    return parseFloat2($(el).attr(attr));
}

function getInputAttrValue(el, attr) {
    if ($(el).find("input").length > 0) {
        return $(el).find("input").val();
    } else {
        if($(el).length > 0)
            return $(el).attr(attr);
    }
    return 0;
}

function prepareSums() {



    var $Scorecard = $(".cntScorecard");
    var $Tasks = $Scorecard.find(".cntTasks");
    var $Values = $Scorecard.find(".cntValues");
    var $CoolStuff = $Scorecard.find(".cntCoolStuff");
    var $Days = $Scorecard.find(".cntDays");
    var $CosmicInfo = $Scorecard.find(".cntCosmicInfo");
    var $cosmicInfo2 = $Scorecard.find(".cntCosmicInfo2");

    //sprawdzenie czy sie oplaca w ogole
    if (!$Scorecard.is(':visible'))
        return false;
    // oplaca sie

    // zliczanie sum ilosci
    var rows = $Values.find("tr").length;
    var cols = $Values.find("tr:first > td").length;
    for (i = 0; i < cols; i++) {
        var colSum = 0;
        for (j = 0; j < rows; j++) {
            var count = 0;
            count = getInputLabelValue($Values.find("tr").eq(j).find("td").eq(i));
            if (count > 0) colSum += parseInt(count);
        }
        $Tasks.find(".task .sum .iSum").eq(i).text(colSum);
    }

    var errors = 0, errorsSum = 0; // to musi byc tutaj, bo po eachu potrzebuje
    var i = 0;
    $Values.find("tr").each(function() {
        var prodHours = 0, prodResult = 0, starsCount = 0, fpy = 0;
        var j = 0;
        $(this).find("td").each(function() {
            var count = getInputLabelValue($(this));
            var time = getLabelValue($Tasks.find(".task .timeLabel").eq(j));
            prodHours += count * parseFloat2(time);
            if ($Tasks.find(".name").eq(j).attr("data-qc") == "True") starsCount += parseInt(count);
            j++;
        });

        // HEHE
        // czas nieproduktywny (TRZYDZIESTA)  
        var czasNieproduktywny = getInputLabelValue($Days.find(".czasnieprod").eq(i));
        if (czasNieproduktywny != nieprodArr[i] && nieprodArr[i] != undefined && nieprodArr[i] != -1337) {
            var diff = parseFloat2(nieprodArr[i] - parseFloat2(czasNieproduktywny));
            var war1 = parseFloat2(getLabelValue($Days.find(".w1").eq(i)));
            var war2 = parseFloat2(getLabelValue($Days.find(".w2").eq(i)));

            war1 += parseFloat2(diff);
            war2 += parseFloat2(diff);

            //            alert(war1);
            //            alert(war2);

            setText($Days.find(".war1").eq(i), readable(war1));
            setText($Days.find(".war2").eq(i), readable(war2));
        }
        nieprodArr[i] = parseFloat2(czasNieproduktywny);
        // /HEHE

        // bo w minutach trzymamy
        prodHours = prodHours / 60;

        var prodTime = $Days.find(".war1").eq(i).text();
        var kanapka = $Days.find(".war2").eq(i).text();
        if (parseFloat2(prodTime) != 0 && parseFloat2(kanapka) != 0) prodResult = parseFloat2(prodHours) / parseFloat2(kanapka);
        else prodResult = 0;
        errors = getInputLabelValue($CoolStuff.find(".bledy").eq(i));
        errorsSum += parseFloat2(errors);

        if (starsCount == 0) fpy = 0;
        else fpy = (1 - parseFloat2(errors) / parseFloat2(starsCount));

        // mordki
        var aim1 = $(".cntDays .aim .aim1").eq(i).text();
        aim1 = aim1.substring(0, aim1.length - 1);
        var aim2 = $(".cntDays .aim .aim2").eq(i).text();
        aim2 = aim2.substring(0, aim2.length - 1);

        var faze1, faze2;
        if ((aim1 == "0" && aim2 == "0") || prodTime == 0) {
            faze1 = faze2 = "images/Skeleton64px.png";
        }
        else {
            faze1 = (prodResult >= parseFloat2(aim1 / 100)) ? "images/Happy64px.png" : "images/Sad64px.png";
            faze2 = (fpy >= parseFloat2(aim2 / 100)) ? "images/Happy64px.png" : "images/Sad64px.png";
        }

        // aim NOWE ZMIANY
        if (starsCount == 0) {
            $Days.find(".aim .aim2").eq(i).text("0%");
            faze2 = "images/Skeleton64px.png";
        }




        // insertowanie do spanow

        $(".cntCoolStuff .godzprod .prodHours").eq(i).text(readable(prodHours));
        $(".cntCoolStuff .godzprod .prodHours").eq(i).attr("data-value", prodHours);
        
        $(".cntCoolStuff .ilosc .starsCount").eq(i).text(starsCount);
        $(".cntCoolStuff .fpy .fpy").eq(i).text(readableCh(fpy * 100, "%"));
        $(".cntDays .result .result1").eq(i).text(readableCh(prodResult * 100, "%"));
        $(".cntDays .result .result2").eq(i).text(readableCh(fpy * 100, "%"));
        $(".cntDays .face .face1").eq(i).attr("src", faze1);
        $(".cntDays .face .face2").eq(i).attr("src", faze2);
        i++;
    });

    // SUMY
    //nowe
    loadSum(".cntDays .nadg .nad", ".cntCosmicInfo .nadg .over");

    //var sumProdHours = loadSum(".cntCoolStuff .godzprod .prodHours", ".cntCosmicInfo2 .godzprod .prodHours");
    var sumProdHours = getSumAttr(".cntCoolStuff .godzprod .prodHours", ".cntCosmicInfo2 .godzprod .prodHours", "data-value");
    
    var sumStarsCount = loadSum(".cntCoolStuff .ilosc .starsCount", ".cntCosmicInfo2 .ilosc .count");
    var sumnominal = loadSum(".cntDays .nominal .nom", ".cntCosmicInfo .nominal .nom");
    loadSum(".cntDays .nieob .abs", ".cntCosmicInfo .nieob .abs");
    loadSumInputLabel(".cntDays .czasnieprod", ".cntCosmicInfo .czasnieprod .unprod");
    var sumpracainnyarkusz = loadSumInputLabel(".cntDays .prinnyarkuszASD", ".cntCosmicInfo .prinnyarkusz .other");
    //alert(sumpracainnyarkusz);
    var sumazczasprod = loadSum(".cntDays .war1 .w1", ".cntCosmicInfo .war1 .w1");
    var sumazkanapek = loadSum(".cntDays .war2 .w2", ".cntCosmicInfo .war2 .w2");
    loadSum(".cntDays .nadg .nad", ".cntCosmicInfo .nadg .nad");
    loadSumInputLabel(".cntCoolStuff .bledy", ".cntCosmicInfo2 .bledy .errors");

    // juan nowe zmiany
    loadSum(".cntDays .tlprac .tlprac", ".cntCosmicInfo .tlprac .tlprac");

    // PRODRESULT
    var prodResult = 0;
    if (sumazkanapek == 0) prodResult = 0;
    else prodResult = parseFloat2(sumProdHours) / parseFloat2(sumazkanapek);
    $(".cntCosmicInfo .result .result1").text(readableCh(prodResult * 100, "%"));

    // FPY
    var qcResult = 0;
    if (sumStarsCount == 0) qcResult = 0;
    else qcResult = 1 - (parseFloat2(errorsSum) / parseFloat2(sumStarsCount));
    $(".cntCosmicInfo .result .result2").text(readableCh(qcResult * 100, "%"));
    $(".cntCosmicInfo2 .fpy .fpyr").text(readableCh(qcResult * 100, "%"));

    // --------------------- HEADER ---------------------
    // sprawdzanie czy sie oplaca


    var $Header = $(".cntHeader");
    if (!$Header.is(':visible')) return false;

    // oplaca sie
    $Header.find(".lblMidProdPodst").text(readableCh(prodResult * 100, "%"));

    //------------- ZMIANA 20151021 Teraz biere z property w headerze
    if (!$Header.find(".teamSize").is(':visible')) {
        sumnominal = $('.cntHeader .tdPremiaProd').attr("data-value");
    }
    //-------------


    // PREMIA ZA PRODUKTYWNOŚC
    var data = $('.cntHeader .hidData').val();
    var splitted = data.split(";");
    var prodPremia = 0;
    for (i = 0; i < splitted.length; i++) {
        if (prodResult < splitted[i].split("|")[0]) break;
        prodPremia = splitted[i].split("|")[1];
    }
    if (isNaN(prodPremia)) prodPremia = 0;
    $('.cntHeader .lblPremiaProdPodst').text(readable(prodPremia) + " zł");

    var data2 = $('.cntHeader .hidData2').val();
    splitted = data2.split(";");
    var qc = 0;
    for (i = 0; i < splitted.length; i++) {
        if (qcResult < splitted[i].split("|")[0]) break;
        qc = splitted[i].split("|")[1];
    }

    var prodPremia2 = prodPremia / sumnominal * sumazczasprod;
    if (isNaN(prodPremia2)) prodPremia2 = 0;

    // grupówka
    if ($Header.find(".teamSize").is(':visible')) {
        var teamSize = getInputLabelValue($Header.find(".teamSize"));
        prodPremia2 *= teamSize;
    }
    $('.cntHeader .lblPremiaProdProp').text(readable(prodPremia2) + " zł");

    // KOREKTA O WSPÓŁCZYNNIK JAKOŚCI
    var cosmos = 0;
    if (sumStarsCount == 0) qc = 1;
    if (prodResult != 0) cosmos = parseFloat2(qc) - 1;
    $('.cntHeader .lblKorektaWsplJakPodst').text(readableCh(cosmos * 100, "%"));


    var qc2 = prodPremia2 * (qc - 1);
    if (isNaN(qc2)) qc2 = 0;
    $('.cntHeader .lblKorektaWsplJakProp').text(readableCh(qc2, " zł"));

    // KOREKTA PREMII ZA DYSPOZYCYJNOŚC
    var data3 = $('.cntHeader .hidData3').val();
    var sumadopd = $('.cntHeader .hidAbs').val();
    if (sumadopd == "") sumadopd = 0;
    splitted = data3.split(";");

    var abs = 0;
    for (i = 0; i < splitted.length; i++) {
        if (sumadopd < parseFloat2(splitted[i].split("|")[0])) break;
        abs = splitted[i].split("|")[1];
    }
    if (isNaN(abs) || abs == null || abs == "") abs = 0;

    var abs2 = 0;
    if (abs >= 1337) {
        abs2 = (abs - 1337);
        $('.cntHeader .lblKorektaDyspPodst').text(readableCh(abs2, " zł"));
    }
    else {
        var output = parseFloat2(abs) - 1;
        abs2 = parseFloat2(abs - 1) * prodPremia2;
        $('.cntHeader .lblKorektaDyspPodst').text(readableCh(output * 100, "%"));
    }
    if (sumProdHours == 0) abs2 = 0; // tu se zaznacze
    $('.cntHeader .lblKorektaDyspProp').text(readableCh(abs2, " zł"));

    // GRUPÓWKA
    if ($Header.find(".teamSize").is(':visible') || !$Header.find(".absKorekta").is(":visible")) {
        abs2 = 0;
    }

    // PRACA INNY ARKUSZ
    var pracainnyarkusz = $('.cntHeader .hidPrInnyArkusz').val();
    var premiainnydzial = parseFloat2(pracainnyarkusz) * parseFloat2(sumpracainnyarkusz);

    $('.cntHeader .lblPremiaInnyDzialPodst').text(readableCh(premiainnydzial, " zł"));
    $('.cntHeader .lblPremiaInnyDzialProp').text(readableCh(premiainnydzial, " zł"));

    // RAZEM PREMIA
    var cosmossum = prodPremia2 + qc2 + abs2 + premiainnydzial;
    $('.cntHeader .lblRazemPremia').text(readableCh(cosmossum, " zł"));

    // TU WPISUJE DO HIDDENA, TRZEBA TO BEDZIE ZABEZPIECZYC ALBO NIE
    if (document.getElementById("ctl00_ContentPlaceHolder2_Spreadsheet_Scorecard_hidRazemPremia") != null) {
        document.getElementById("ctl00_ContentPlaceHolder2_Spreadsheet_Scorecard_hidRazemPremia").value = cosmossum
    }
    return true;
}

function setHeight() {
    var h = $("#row1").height();
    //alert(h);
    $("#tbCosmicInfo").height(h);
    $(".task").height(h);
    $("#tbCosmicInfo2").height(h);
    $("#scrollbox3").height(h);
}

function setWidth() {
    var w1 = $(".cntDays").width();
    var w3 = $(".cntCoolStuff").width();
    var margin = 80;
    var pageWidth = getClientSize()[0];
    var minWidth = 256;
    var maxWidth = $(".cntValues tr:first > td").length * 64;

    var valuesWidth = pageWidth - w1 - w3 - margin;
    if (valuesWidth < minWidth) valuesWidth = minWidth;
    if (valuesWidth > maxWidth) valuesWidth = maxWidth;

    //alert(w1);
    //alert(w3);
    //alert(pageWidth);

    $(".cntTasks").width(valuesWidth);
    $(".scrollbox1").width(valuesWidth);
    $(".cntTasks").css("max-width", valuesWidth);
    $(".scrollbox1").css("max-width", valuesWidth);
}


function prepareStuff() {
    saved = true;

    $(window).bind('beforeunload', function(e) {
        if (!saved) return "Uwaga! Zmiany nie zostały zapisane!";
    });


    // ustawianie wysokości
    setHeight();

    $("#row2").resize(function() {
        setHeight();
    });

    $(window).resize(function() {
        setWidth();
    });


    // kitranie taskow bo filtr
    $("#tbFilter").on("keyup", function() {
        rows = $(".cntValues tr").length;
        cols = $(".cntValues tr:first > td").length;
        elementWidth = 64;
        hideWidth = cols * elementWidth;
        for (x = 0; x < cols; x++) {
            if (!$(".bunker").eq(x).text().toLowerCase().match($("#tbFilter").val().toLowerCase())) {
                $(".cntTasks .task td.name").eq(x).hide();
                // solucja
                $(".cntTasks .task td.cc").eq(x).hide();
                $(".cntTasks .task td.time").eq(x).hide();
                $(".cntTasks .task td.sum").eq(x).hide();
                // koniec solucji
                hideWidth -= elementWidth;
                for (y = 0; y < rows; y++) {
                    $(".cntValues tr").eq(y).find("td").eq(x).hide();
                }
            }
            else {
                $(".cntTasks .task td.name").eq(x).show();
                // solucja
                $(".cntTasks .task td.cc").eq(x).show();
                $(".cntTasks .task td.time").eq(x).show();
                $(".cntTasks .task td.sum").eq(x).show();
                // koniec solucji
                for (y = 0; y < rows; y++) {
                    $(".cntValues tr").eq(y).find("td").eq(x).show();
                }
            }
        }
        maxWdith = 800;
        if (hideWidth > 800) hideWidth = 800;
        //if (hideWidth < 10) hideWidth = 1;
        $(".cntTasks").width(hideWidth);
        $(".scrollbox1").width(hideWidth);

    });

    // śmieszne obracanie

    $(".menuItem").on("hover", function() {
        $(this).find("span.fa, img").toggleClass("funnyRotate");
    });

    // sprytne scrollowanie

    $("#scrollbox1").scroll(function() {
        $(".cntTasks").scrollLeft($("#scrollbox1").scrollLeft());
    });
    $(".cntTasks").scroll(function() {
        $("#scrollbox1").scrollLeft($(".cntTasks").scrollLeft());
    });


    // pr inny arkusz blokada petli

    var wMax = 801;
    var wMin = 483.5;

    if ($(".cntScorecard .cntDays .hidOutsideJob").val() == "1") {
        $(".cntScorecard .cntCosmicInfo .prinnyarkusz").hide();
        $(".cntScorecard .cntCosmicInfo .prinnyarkuszx").hide();
        $(".cntScorecard .cntCosmicInfo .czasnieprod").hide();

        $(".cntScorecard .cntDays .prinnyarkusz").hide();
        $(".cntScorecard .cntDays .prinnyarkuszx").hide();
        $(".cntScorecard .cntDays .czasnieprod").hide();
    }

    // zwijarka do kolumn na sc
    $('.showColumnsTab').on("click", function() {
        if ($('.hid').is(':visible')) {
            $(".hid").hide();
            $(".ultymat").attr("colspan", 11);
            scHidden = true;
        }
        else {
            $('.hid').show();
            if ($(".plannedteamsize").is(":visible")) {
                wMax += 75;
            }
            $(".ultymat").attr("colspan", 17);
            scHidden = false;
        }
    });


    // guziki z lewej

    leanWidth = 46;
    startRight = leanWidth - 1; // bo padding / margin ??

    $('.menuItem').each(function() {
        $(this).css({ right: $(this).width() - startRight });

    });

    $('.menuItem').on("mouseenter", function() {
        if ($(this).attr('hid') == "1") {
            $(this).animate({ left: -1 }, { duration: 500, queue: false });
        }
    });

    $('.menuItem').on("mouseleave", function() {
        if ($(this).attr('hid') == "1") {
            w = $(this).width();
            w -= leanWidth;
            $(this).animate({ left: -w }, { duration: 500, queue: false });
        }
    });

    // obsluga tych guzikow z lewej

    $('.save').on("click", function() {
        $('.btnSave').click();
        saved = true;
    });

    $('.accept').on("click", function() {
        $('.btnAccept').click();
    });

    $('.back').on("click", function() {
        $(".btnBack").click();
    });

    $('.showMine').on("click", function() {
        $(".btnMine").click();
    });

    $(".unaccept").on("click", function() {
        $(".btnUnAccept").click();
    });

    $(".acceptall").on("click", function() {
        $(".btnAcceptAll").click();
    });

    // pokazywanie tabelek z lewej

    $(".cntEmployeeSelect .hiddenTable").on("click", function(e) {
        e.stopPropagation();
    });

    showHiddenTableAgnostic(".cntEmployeeSelect .showSuperiors");
    showHiddenTableAgnostic(".cntEmployeeSelect .showList");
    showHiddenTableAgnostic(".cntEmployeeSelect .showGroups");


    // szukajki

    doSearch(".cntEmployeeSelect .tbSuperiorsSelect .search input", ".cntEmployeeSelect .tbSuperiorsSelect tr.items td");
    doSearch(".cntEmployeeSelect .tbEmployeeSelect .search input", ".cntEmployeeSelect .tbEmployeeSelect tr.items td");
    doSearch(".cntEmployeeSelect .tbGroupsSelect .search input", ".cntEmployeeSelect .tbGroupsSelect tr.items td");

    // klikanie i odklikiwanie td w sc
    lastClickedInputId = "-1";
    lastClickedInputVal = "";

    $('html').click(function(e) {
        if (lastClickedInputId == "") return;
        if (e.target.id == lastClickedInputId) {
        } else {
            if (lastClickedInputVal != $("#" + lastClickedInputId).val()) {
                //$("#" + lastClickedInputId).css("background-color", "red");
            }
        }
    });

    $(".cntScorecard td.edit input").on("focus", function(e) {
        if ($(this).attr("id") == lastClickedInputId) return;
        $(this).select();

        //setSelectionRange(this, 0, 10);

        // nowy fragment kodu
        $selectedTr = $(this).closest("tr");
        $selectedTd = $(this).closest("td");
        var selectedTrIndex = $selectedTr.index();

        toggleClassRow($(".cntValues tr").eq(selectedTrIndex), "selectedN");
        toggleClassRow($(".cntDays .tbDays tr").eq(selectedTrIndex), "selectedN");
        toggleClassRow($(".cntCoolStuff tr").eq(selectedTrIndex), "selectedN");
        toggleClassCol($selectedTd, "selectedN");

        if (lastClickedInputVal != $("#" + lastClickedInputId).val() && lastClickedInputId != "-1") {
            // togglowanie alarmu
            if (!$(".toggleAlarmClass img").hasClass("alarm")) toggleSaveAlarm();
            saved = false;
            if (!$("#" + lastClickedInputId).parent().hasClass("edited")) $("#" + lastClickedInputId).parent().toggleClass("edited");
        }

        // nowy fragment kodu
        if (lastClickedInputId != "-1") {
            $lastClickedInput = $("#" + lastClickedInputId).closest("tr");
            $lastClickedInputTd = $("#" + lastClickedInputId).closest("td");
            var lastClickedInputIndex = $lastClickedInput.index();
            toggleClassRow($(".cntValues tr").eq(lastClickedInputIndex), "selectedN");
            toggleClassRow($(".cntDays .tbDays tr").eq(lastClickedInputIndex), "selectedN");
            toggleClassRow($(".cntCoolStuff tr").eq(lastClickedInputIndex), "selectedN");

            toggleClassCol($lastClickedInputTd, "selectedN");
        }
        lastClickedInputId = $(this).attr("id");
        lastClickedInputVal = $(this).val();

    });

    // przeliczanie sum real-time

    $(".cntScorecard td.edit input").change(function() {
        prepareSums();
    });

    // sterowanie klawiaturą

    var points = 0;
    var run = false;

    var tajne = [38, 38, 40, 40, 37, 39, 37, 39, 66, 65];
    secretIndex = 0;

    $(".cntScorecard td.edit input").keydown(function(e) {

        var $parentCnt = $(this).parents(".cnt2");

        var keycode = (e.keyCode ? e.keyCode : e.which);
        var row = $(this).parent().parent().index();
        var col = $(this).parent().index();
        //console.log(row + " " + col);
        // nic nie znaczacy fragment kodu, pominac
        if (keycode == tajne[secretIndex]) secretIndex++;
        else secretIndex = 0;
        if (secretIndex == tajne.length) {
            secretIndex = 0;
            $("#header").toggleClass("fa-spin");
            run = true;
            //zuseless();
        }


        switch (keycode) {
            case 40: // down
                //case 74: // vi
                //case 78: // emacs    
                while ($parentCnt.find("tr").eq(row + 1).hasClass("noteditable")) ++row;
                if ((row) >= $parentCnt.find("tr").length - 1) row = -1;
                //$(".cntScorecard .tr").eq(++row).find("td").eq(col).find("input").focus();
                $parentCnt.find("tr").eq(++row).find("td").eq(col).find("input").focus();
                e.preventDefault();
                break;
            case 39: //right
                //case 76:
                //case 70:
                //$(this).select();
                if ((col) >= $parentCnt.find("tr").eq(row).find("td").length - 1) col = -1;
                $parentCnt.find("tr").eq(row).find("td").eq(++col).find("input").focus();
                e.preventDefault();
                break;
            case 38: // up
                //case 75:
                //case 80:
                while ($parentCnt.find("tr").eq(row - 1).hasClass("noteditable")) --row;
                $parentCnt.find("tr").eq(--row).find("td").eq(col).find("input").focus();
                e.preventDefault();
                break;
            case 37: // left
                //case 72:
                //case 66:
                $parentCnt.find("tr").eq(row).find("td").eq(--col).find("input").focus();
                e.preventDefault();
                break;
            case 36: // home
                //case 65:
                $(".cntScorecard .cntValues tr").eq(row).find("td").eq(0).find("input").focus();
                e.preventDefault();
                break;
            case 35: // end
                //case 69:
                var tdCount = $(".cntScorecard .cntValues tr").eq(row).find("td").length;
                $(".cntScorecard .cntValues tr").eq(row).find("td").eq(--tdCount).find("input").focus();
                e.preventDefault();
                break;
            case 13:
                if ((col) >= $parentCnt.find("tr").eq(row).find("td").length - 1) col = -1;
                $parentCnt.find("tr").eq(row).find("td").eq(++col).find("input").focus();
                e.preventDefault();
                break;
            default:

                break;
            //case 13:                 
            //e.preventDefault();                 
            //e.stopPropagation();                 
            //break;            
        }

    });

    // MASZ TEGO PLUSA

    $(".cntScorecard .showColumnsTab").on("click", function() {
        $(this).find("a").toggleClass("fa-plus").toggleClass("fa-minus");
    });

    // święta

    $(".cntScorecard .cntDays .tbDays tr").each(function() {
        if ($(this).attr("data-value") == "1") {
            var index = $(this).index();
            $(".cntScorecard .cntDays .tbDays tr").eq(index).toggleClass("xmas");
            $(".cntScorecard .cntValues tr").eq(index).toggleClass("xmas");
            $(".cntScorecard .cntCoolStuff tr").eq(index).toggleClass("xmas");
        }
    });

    // state

    $(".cntScorecard .cntDays .tbDays tr").each(function() {
        if ($(this).attr("data-state") == "0") {
            var index = $(this).index();
            $(".cntScorecard .cntDays .tbDays tr").eq(index).toggleClass("noteditable");
            $(".cntScorecard .cntValues tr").eq(index).toggleClass("noteditable");
            $(".cntScorecard .cntCoolStuff tr").eq(index).toggleClass("noteditable");
        }
    });

    // pokazywanie wykrzyknika

    $(".cntScorecard .cntDays td.prinnyarkusz").each(function() {
        var $tbValue = getInputLabelValue($(this));
        if ($(this).attr("data-value") != "0" || $tbValue != "0") {
            $(this).parent().find(".prinnyarkuszx .other").show();
            if ($tbValue == "0") {
                $(this).parent().find(".prinnyarkuszx .other").toggleClass("blink2");
            } else {
                //$(this).parent().find(".prinnyarkuszx .other").toggleClass();
            }
        }
        else {

            $(this).parent().find(".prinnyarkuszx .other").hide();
        }

    });

    $(".cntScorecard .cntDays .prinnyarkusz input").on("input", function() {
        $(this).parent().parent().find(".prinnyarkuszx .other").show();
        $(this).parent().parent().find(".prinnyarkuszx .other").toggleClass("blink2");
    });

    // pokazywanie row do usunięcia

    $(".cntScorecard .cntCoolStuff .remover a").on("hover", function() {
        var index = $(this).closest("tr").index();
        $(".cntScorecard .cntDays .tbDays tr").eq(index).toggleClass("toRemove");
        $(".cntScorecard .cntValues tr").eq(index).toggleClass("toRemove");
        $(".cntScorecard .cntCoolStuff tr").eq(index).toggleClass("toRemove");
    });

    // ULTYMAT
    //handleSlider();

    $(document).keydown(function(e) {
        var keycode = (e.keyCode ? e.keyCode : e.which);
        if (e.ctrlKey && e.shiftKey && keycode == 70) {
            $("#tbFilter").focus();
        }
    });







    // ustawianie szerokości
    setWidth();
}

function useless() {
    $header = $("#header");
    var x = ($header.position().top + $header.height()) / 2;
    var y = ($header.position().left + $header.width()) / 2;
    var w = $header.width();
    $values = $(".cntValues");
    for (var i = 0; i < 55; i++) {
        makeTarget($values, x, y, w);
    }
}

function jetFighter() {
    $(".showMine").on("mouseenter", function() {
        $(this).find(".fa").toggleClass("funnyRotate");
        $(this).find(".fa").css("position", "fixed");
        $(this).find(".fa").animate({ left: 2000 }, { duration: 5000, queue: true, complete: function() {
            $(".showMine").find(".fa").css({ left: -50 });
            $(".showMine").find(".fa").animate({ left: 72 }, { duration: 1000, queue: true, complete: function() {

                $(".showMine").find(".fa").css("position", "static");
            }
            });
        }
        });
    });
}

function makeTarget($values, x, y, w) {
    var index = getRand($values, x, y, w);
    $values.find("td").eq(index).css("background-color", "red");
}

function getRand($values, x, y, w) {
    var rows = $values.find("tr").length;
    var cols = $values.find("tr:first > td").length;
    var max = rows * cols;
    while (true) {
        var i = Math.floor(Math.random() * max);
        $td = $values.find("td").eq(i);
        if ($td.position().top <= y + (w / 2)) {
            return i;
        }
    }
}

function toggleClassRow($tr, css) {
    $tr.find("td").each(function() {
        $(this).toggleClass(css);
    });
}

function toggleClassCol($td, css) {
    var i = $td.index();
    $control = $td.closest(".selectable");
    $control.find("tr").each(function() {
        $(this).find("td").eq(i).toggleClass(css);
    });
}

function doSearch(input, toHide) {
    $(input).on("keyup", function() {
        $(toHide).each(function() {
            if (!$(this).find("a").text().toLowerCase().match($(input).val().toLowerCase())) {
                $(this).hide();
            }
            else {
                $(this).show();
            }
        });
    });
}

function toggleSaveAlarm() {
    $(".toggleAlarmClass").animate({ left: -1 }, { duration: 500, queue: false });
    $(".toggleAlarmClass").toggleClass("blink");
    $(".toggleAlarmClass img").toggleClass("alarm");
}

function showHiddenTableAgnostic(button) {
    $(button).on("click", function(event) {
        buttonWidth = $(this).width() - 45;
        hidden = $(button).attr('hid');
        if (hidden == null) hidden = "1";
        show = showPending(button);
        showButtonWidth = $(show).width() - 45;

        table = "." + $(button).attr("data-table");
        $("html").one('click', function() {
            hideHiddenTable(button, table, buttonWidth);
        });
        if (show != null) {
            hideHiddenTable(show, "." + show.attr("data-table"), function() {
                showHiddenTable(button, "." + $(button).attr("data-table"));
            }, showButtonWidth);
        }
        else if (hidden == "1") showHiddenTable(button, table, buttonWidth);
        else hideHiddenTable(button, table, buttonWidth);
        event.stopPropagation();
    });
}

function showPending(button) {
    pending = null;
    $(".cntEmployeeSelect .showHiddenTable").not(button).each(function() {
        if ($(this).attr("hid") == "0") pending = $(this);
    });
    return pending;
}

function showHiddenTable(button, table, buttonWidth) {
    // skitraj wszystkie najpierw
    $('.cntEmployeeSelect .hiddenTable').each(function() {
        $(this).css("display", "none");
    });
    $(table).css("display", "inline-block");
    $(button).attr("hid", "0");
    $(".cntEmployeeSelect").animate({ left: 0 }, { duration: 500, queue: true });
    $(button).animate({ left: -buttonWidth }, { duration: 500, queue: false });
}

function hideHiddenTable(button, table, show, buttonWidth) {
    $(button).attr("hid", "1");
    $(".cntEmployeeSelect").animate({ left: -300 }, { duration: 1000, queue: false, complete: function() {
        if (show != null && typeof show === 'function') show();
    }
    });
    $(button).animate({ left: -buttonWidth }, { duration: 1000, queue: false });
}




// BARDZO WAŻNE

$(document).ready(function() {
    prepareJS();
});

function prepareJS() {

    nieprodArr = [];
    for (i = 0; i < 31; i++) nieprodArr[i] = -1337;

    prepareSums();
    prepareStuff();

    var wMax = 801;

    if (scHidden) {
        $(".hid").hide();
        $(".ultymat").attr("colspan", 11);
        $(".cntScorecard .showColumnsTab a").removeClass("fa-minus").addClass("fa-plus");
    }
    else {
        $('.hid').show();
        if ($(".plannedteamsize").is(":visible")) {
            wMax += 75;
        }
        $(".ultymat").attr("colspan", 17);
        $(".cntScorecard .showColumnsTab a").removeClass("fa-plus").toggleClass("fa-minus");
    }
}

function handleSlider() {
    var h = $(".cntScorecard div.row2 .cntCoolStuff").height();
    $sliderWrapper = $("div#daySelectorWrapper");
    $sliderWrapper.height(h);
    $slider = $("div#daySelector");

    var tdH = $(".cntScorecard .cntCoolStuff td:first").height();

    var top = $slider.offset().top;
    var left = $slider.offset().left;
    var bottom = $slider.offset().top + $slider.height();
    var right = $slider.offset().left + $slider.width();
    var maxOffset = 8;

    var isDragging = false;
    // 0 top 1 dol
    var draggingPoint = 0;
    $slider.mousedown(function(e) {
        isDragging = true;

        if (e.pageY >= $slider.offset().top && e.pageY <= ($slider.offset().top + maxOffset)) {
            draggingPoint = 0;
        }
        else if (e.pageY >= ($slider.offset().top + $slider.height() - maxOffset) && e.pageY <= ($slider.offset().top + $slider.height() + maxOffset)) {
            draggingPoint = 1;
        }
    })

    $(window).mouseup(function() {
        isDragging = false;
        var str = $slider.css("top");
        var t = parseInt(str.substring(0, str.length - 2));
        var th = Math.round(t / tdH) * tdH;
    });

    var lastShiftTop = 0;
    var lastShiftBottom = 0;
    var shift = 0;
    $(window).mousemove(function(e) {
        if (isDragging) {
            if (draggingPoint == 0) {

                shift = e.pageY - top;
                console.log("shift: " + shift);
                console.log("good: " + Math.ceil(shift / tdH) * tdH);
                shift = Math.ceil(shift / tdH) * tdH;
                console.log("tdH: " + tdH);

                var deltaShift = shift - lastShiftTop;
                if (shift < 0) {
                    shift = 0;
                    deltaShift = 0;
                }
                $slider.css("top", shift);
                $slider.height($slider.height() - deltaShift);
                lastShiftTop = shift;
            }
            else {
                shift = bottom - e.pageY;
                var deltaShift = shift - lastShiftBottom;
                if (shift < 0) {
                    shift = 0;
                    deltaShift = 0;
                }
                $slider.height($slider.height() - deltaShift);
                lastShiftBottom = shift;
            }
        }
    });

    $slider.mousemove(function(e) {
        if ((e.pageY >= $slider.offset().top && e.pageY <= ($slider.offset().top + maxOffset)) || (e.pageY >= ($slider.offset().top + $slider.height() - maxOffset) && e.pageY <= ($slider.offset().top + $slider.height() + maxOffset))) {
            $(this).css("cursor", "n-resize");
        }
        else {
            $(this).css("cursor", "default");
        }
    });
}
// TU JUŻ NIE ARKUSZZE

function prepareSearch(tbId, tableName, tdName) {
    $tb = $("#" + tbId);
    $tb.on("keyup", function() {
        $input = $(this);
        $table = $("." + tableName);
        $table.find("." + tdName).each(function() {
            if (!$(this).find("span").text().toLowerCase().match($input.val().toLowerCase())) {
                $(this).parent("tr").hide();
            }
            else {
                $(this).parent("tr").show();
            }
        });
    });
    $xButton = $tb.parent().find(".xbutton");
    $xButton.on("click", function() {
        $("#" + tbId).val('');
        $table.find("." + tdName).each(function() {
            $(this).parent("tr").show();
        });
    });
}

// SUMOWANIE WNIOSKÓW

function setMiesiac(el) {
    if (el.length > 0) {


        var dateDisp = el.find("option:selected").text();
        var dateId = el.find("option:selected").val().substring(0, 10);
        var psx = Date.parse(dateId);
        var date = new Date(psx);
        //alert(dateId);
        //alert(psx);
        //alert(date);
        date.setMonth(date.getMonth() - 1);
        //alert(date.toLocaleFormat("%Y-%m"));
        $tbRequestHeader.find(".lblMiesiacWypracowania").text(date.toLocaleFormat("%Y-%m"));
    }
}

function prepareRequests() {
    $cntRequest = $(".cntRequest");
    $tbRequest = $cntRequest.find(".tbRequest");
    $cntRequestHeader = $cntRequest.find(".cntRequestHeader");
    $tbRequestHeader = $cntRequestHeader.find(".tbRequestHeader");

    var sQuatro0 = $tbRequestHeader.find(".quatro0td").attr("data-value");  //$(".ddlQuatro0").find("option:selected").val();
    var sQuatro1 = $tbRequestHeader.find(".quatro1td").attr("data-value");  //$(".ddlQuatro1").find("option:selected").val();

    if (sQuatro0 != undefined) {

        // tu były zmiany

        var ddlQ0 = $(".ddlQuatro0").find("option:selected").val();
        var ddlQ1 = $(".ddlQuatro1").find("option:selected").val();

        //console.log(ddlQ0 + " " + ddlQ1);

        var ddlQuatro0 = ddlQ0 != undefined ? ddlQ0.split("|")[0] : -1337;
        var ddlQuatro1 = ddlQ1 != undefined ? ddlQ1.split("|")[0] : -1337;

        //console.log(ddlQuatro0 + " " + ddlQuatro1);
       
        var Quatro0 = ddlQuatro0 != -1337 ? ddlQuatro0 : sQuatro0.split("|")[0];
        var Quatro1 = ddlQuatro1 != -1337 ? ddlQuatro1 : sQuatro1.split("|")[0];

        //console.log(Quatro0);
        //console.log(Quatro1);

        $tbRequestHeader.find(".quatro0").text(readableCh(Quatro0, " zł"));
        $tbRequestHeader.find(".quatro1").text(readableCh(Quatro1, " zł"));
    }

    prepareRequestsSums();

    $(".cntRequest .tbRequest input").keyup(function() {
        prepareRequestsSums();
    });

    $(".cntRequestHeader input").keyup(function() {
        prepareRequestsSums();
    });

    $(".ddlQuatro0").change(function() {
        $tbRequestHeader.find(".quatro0").text(readableCh($(this).find("option:selected").val().split("|")[0], " zł"));
        prepareRequestsSums();
    });

    $(".ddlQuatro1").change(function() {
        $tbRequestHeader.find(".quatro1").text(readableCh($(this).find("option:selected").val().split("|")[0], " zł"));
        prepareRequestsSums();
    });

    $(".ddlDataWyplaty").change(function() {
        setMiesiac($(this));
    });
    
    setMiesiac($(".ddlDataWyplaty"));

    

    // rbl

    $(".rbl").find("input:radio").click(function() {
        var type = $(this).val();
        switch (type) {
            case "0":
                $(".type .ddlSpreadsheets").attr("disabled", false);
                $(".type .tbName").attr("disabled", "disabled");
                break;
            case "1":
                $(".ddlSpreadsheets").attr("disabled", "disabled");
                $(".type .tbName").attr("disabled", false);
                break;
        }
    });

}

function prepareRequestsSums() {
    //console.log('asd');

    loadSum(".cntRequest .tbRequest tr.ntl .premiaMiesieczna", ".cntRequest .tbRequest .sumPremiaMiesieczna");
    loadSumInputLabel(".cntRequest .tbRequest .premiaUznaniowa", ".cntRequest .tbRequest .sumPremiaUznaniowa");

    // razem premia
    $tbRequest.find("tr.it").each(function() {
        var premMies = getLabelValue($(this).find(".premiaMiesieczna"));
        var premUzn = getInputLabelValue($(this).find(".premiaUznaniowa"));
        var premAbs = getInputLabelValue($(this).find(".absKor"));
        var premRazem = parseFloat2(premMies) + parseFloat2(premUzn) + parseFloat2(premAbs);
        if (premRazem == null || isNaN(premRazem)) premRazem = 0;
        $(this).find(".razemPremia").text(readable(premRazem));
    });
    loadSum(".cntRequest .tbRequest tr.ntl .razemPremia", ".cntRequest .tbRequest .sumRazemPremia");

    $premiaNaKolejny = $tbRequest.find(".premiaNaKolejny");
    var pulaUznaniowa = parseFloat2($tbRequestHeader.find(".pulaUznaniowa").text());
    var premiaUznaniowa = parseFloat2($tbRequest.find(".sumPremiaUznaniowa").text());
    var kolejny = pulaUznaniowa - premiaUznaniowa;
    if (isNaN(kolejny)) kolejny = 0;
    $tbRequestHeader.find(".premiaNaKolejny").text(readable(kolejny) + " zł");



    // ind
    var sumGodzProd = loadSum(".cntRequest .tbRequest tr.ntl .godzProd", ".cntRequest .tbRequest .sumGodzProd");
    //    var sumCzasPracy = loadSumInputLabel(".cntRequest .tbRequest tr.ntl .pracownik .czasPracy", ".cntRequest .tbRequest .sumCzasPracy");
    //    var iloscSztuk = loadSum(".cntRequest .tbRequest tr.ntl .iloscSztuk", ".cntRequest .tbRequest .sumIloscSztuk");
    //    var iloscBledow = loadSum(".cntRequest .tbRequest tr.ntl .iloscBledow", ".cntRequest .tbRequest .sumIloscBledow");

    var sumCzasPracy = 0;
    var iloscSztuk = 0;
    var iloscBledow = 0;

    $tbRequest.find("tr.ntl").each(function() {
        var czasPracy = $(this).find(".pracownik .czasPracy").val();
        if (isNaN(parseFloat2(czasPracy))) czasPracy = 0;
        //console.log(czasPracy);
        //alert(czasPracy);
        sumCzasPracy += parseFloat2(czasPracy);

        var sztuki = $(this).find(".pracownik .iloscSztuk").val();
        if (isNaN(parseFloat2(sztuki))) sztuki = 0;
        iloscSztuk += parseFloat2(sztuki);

        var bledy = $(this).find(".pracownik .iloscBledow").val();
        if (isNaN(parseFloat2(bledy))) bledy = 0;
        iloscBledow += parseFloat2(bledy);
        //if(isNaN(czasPracy)) cz
        //alert(czasPracy);
    });

    // gr
    var sumAbs = loadSum(".cntRequest .tbRequest .abs", ".cntRequest .tbRequest .sumAbs");
    var sumAbsKor = loadSum(".cntRequest .tbRequest .absKor", ".cntRequest .tbRequest .sumAbsKor");
    var sumCzas = loadSum(".cntRequest .tbRequest .czas", ".cntRequest .tbRequest .sumCzas");
    if (isNaN(sumAbsKor)) sumAbsKor = 0;

    var a = $tbRequestHeader.find(".hidGodzProd").val();
    if (parseFloat2(a) != -1) {
        var b = $tbRequestHeader.find(".hidCzasPracy").val();
        var c = $tbRequestHeader.find(".hidIloscSztuk").val();
        var d = $tbRequestHeader.find(".hidIloscBledow").val();
        
        sumGodzProd = parseFloat2(a);
        sumCzasPracy = parseFloat2(b);
        iloscSztuk = parseFloat2(c);
        iloscBledow = parseFloat2(d);
    }

    var prod = 0;
    if (sumCzasPracy != 0) {
        prod = (sumGodzProd / sumCzasPracy);
    }
    //alert(prod);
    $tbRequest.find(".sumProd").text(readable(prod * 100) + "%");
    var fpy = 0;
    if (iloscSztuk != 0) fpy = ((iloscSztuk - iloscBledow) / iloscSztuk) * 100;
    $tbRequest.find(".sumFpy").text(readable(fpy) + "%");

    // HEADER TRZECI (TRZYDZIESTA)

    // Średniomiesięczna produktywność
    $tbRequestHeader.find(".prodPodst").text(readable(prod * 100) + "%");

    // Premia za produktywność podst.
    var data = $tbRequestHeader.find(".hidData").val();
    if (data == undefined) return;
    //alert(data);
    var splitted = data.split(";");
    var prodPremia = 0;
    for (i = 0; i < splitted.length; i++) {
        if (prod < splitted[i].split("|")[0]) break;
        prodPremia = splitted[i].split("|")[1];
    }
    if (isNaN(prodPremia)) prodPremia = 0;

    $tbRequestHeader.find(".prodPremiaPodst").text(readable(prodPremia) + " zł");
    //$tbRequestHeader.find(".prodPremiaProp").text(readable(prodPremia) + " zł");


    // Korekta o współczynnik jakości
    var data2 = $tbRequestHeader.find(".hidData2").val();
    splitted = data2.split(";");
    var qc = 0;
    for (i = 0; i < splitted.length; i++) {
        //alert(fpy + " < " + splitted[i].split("|")[0]);
        if ((fpy / 100) < splitted[i].split("|")[0]) break;
        qc = splitted[i].split("|")[1];
    }



    var qcKorektaPodst = 0;
    if (prod != 0) qcKorektaPodst = parseFloat2(qc) - 1;
    $tbRequestHeader.find(".qcKorektaPodst").text((Math.round(qcKorektaPodst * 10000) / 100) + '%');

    var qc2 = prodPremia * (qc - 1);
    if (isNaN(qc2)) qc2 = 0;
    $tbRequestHeader.find(".qcKorektaProp").text(readable(qc2) + " zł");


    // WPZ

    //var wpz = getLabelValue($tbRequestHeader.find(".wpzPodst"));
    var wpz = $tbRequestHeader.find(".hidWPZ").val();

    //    if (wpz[wpz.length - 1] != "%")
    //        $tbRequestHeader.find(".wpzPodst").text(readable(wpz * 100) + "%");
    var wpzProp = (parseFloat2(prodPremia) + parseFloat2(qc2)) * parseFloat2(wpz);
    if (isNaN(wpzProp)) wpzProp = 0;
    $tbRequestHeader.find(".wpzProp").text(readable(wpzProp) + " zł");



    // WPI
    //var wpi = getLabelValue($tbRequestHeader.find(".wpiPodst"));
    var wpi = $tbRequestHeader.find(".hidWPI").val();

    //    if(wpi[wpi.length - 1] != "%")
    //        `$tbRequestHeader.find(".wpiPodst").text(readable(wpi * 100) + "%");

    //var tlPremMies = getLabelValue($tbRequest.find("tr.tl .premmies"));

    var tlPremMies = $tbRequestHeader.find(".prodPremiaPodstInd").attr("data-value"); //getLabelValue($tbRequestHeader.find(".prodPremiaPodstInd"));
    if (tlPremMies == "-1 zł") {
        var q = 0;
        var d = $tbRequestHeader.find(".hidData3").val();
        if (d == undefined) return;
        //alert(data);
        var sp = d.split(";");
        var prodPremia = 0;
        for (i = 0; i < sp.length; i++) {
            if (prod < sp[i].split("|")[0]) break;
            q = sp[i].split("|")[1];
        }
        if (isNaN(q)) q = 0;
        tlPremMies = q;
   
    
        $tbRequestHeader.find(".prodPremiaPodstInd").text(readableCh(q, " zł"));
    }

    //alert(tlPremMies);
    var wpiProp = Math.round(parseFloat2(tlPremMies) * parseFloat2(wpi));

    if (isNaN(wpiProp)) wpiProp = 0;
    $tbRequestHeader.find(".wpiProp").text(readable(wpiProp) + " zł");

    // RAZEM PREMIA
    var quatro0 = parseFloat2($tbRequestHeader.find(".quatro0").text());
    var quatro1 = parseFloat2($tbRequestHeader.find(".quatro1").text());

    var premiaZad = parseFloat2(getInputLabelValue($tbRequestHeader.find(".premiaZad")));
    var premiaUzn = parseFloat2(getInputAttrValue(".premia_uzn_tl", "data-value"));
    if (isNaN(premiaUzn)) premiaUzn = 0;
    //alert(premiaUzn);

    var razemPremia = parseFloat2(wpiProp) + parseFloat2(wpzProp) + quatro0 + quatro1 + premiaZad + premiaUzn;
    
    if (isNaN(razemPremia)) razemPremia = 0;
    $tbRequestHeader.find(".razemPremia").text(readable(razemPremia) + " zł");
    //var pm = getLabelValue($tbRequest.find("tr.tl .premmies"));
    //$tbRequest.find("tr.tl .premmies").text(readable(parseFloat2(razemPremia)) + " (" + parseFloat2(pm) + ")");



}





/*
function showConfirm(content, btnOk, btnCancel) {

    //    if ($("#showConfirmPopup").length > 0) {
    //        $(".popupBg").show();
    //        $("#showConfirmPopup").show();
    //    }
    //    else {
    //        if(

    var ok = "<input id='btnOk' type='button' value='Ok' class='button100' />";
    var cancel = (btnCancel != null && btnCancel != undefined) ? "<input id='btnCancel' type='button' value='Cancel' class='button100 mbtnCancel' />" : "";


    $("body").append("<div class='popupBg'></div>");
    $("body").append("<div id='showConfirmPopup' class='popup'><div id='header'><span class='title'>Pytanie</span><a id='popupCloser' class='fa fa-close popupCloser mbtnCancel' href='javascript://'></a></div><div id='content'>" + content + "</div><div id='footer'>" + cancel + ok + "</div></div>");
    //    }

    $("html").css("overflow", "hidden");

    $("#showConfirmPopup #btnOk").on("click", function() {
        $("#" + btnOk).click();
        hidePopup();
    });

    $("#showConfirmPopup .mbtnCancel").on("click", function() {
        //alert('cancel');
        if (btnCancel != null && btnCancel != "" && btnCancel != undefined)
            $("#" + btnCancel).click();
        hidePopup();
    });

    //$("#showConfirmPopup .popupCloser").on("click", function() {
        //hidePopup();
    //});

    //$(".popupBg").on("click", function() {
        //hidePopup();
    //});

    //$(window).keyup(function(e) {
        //var key = e.which;
        //if (key == 27) {
            //hidePopup();
        //}
    //});

    function hidePopup() {
        //        $(".popupBg").hide();
        //        $("#showConfirmPopup").hide();
        $(".popupBg").remove();
        $("#showConfirmPopup").remove();
        $("html").css("overflow", "auto");
    }
}
*/