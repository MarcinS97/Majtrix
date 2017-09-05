/*
$(function() {
    //$(".datepicker").datepicker({
    //    dateFormat: "yy-mm-dd",
    //    firstDay: 1,

    //});

    //$(".datepickero").on("click", function() {
    //    $(this).parent().find(".datepicker").datepicker("show");
    //});
    prepareCalendar();
});
*/

function prepareCalendar(css) {
    $(".datepicker").datepicker({
        dateFormat: "yy-mm-dd",
        monthNames: ['Styczeń', 'Luty', 'Marzec', 'Kwiecień', 'Maj', 'Czerwiec', 'Lipiec', 'Sierpień', 'Wrzesień', 'Październik', 'Listopad', 'Grudzień'],
        /*dayNamesShort: ['Pn', 'Wt', 'Śr', 'Cz', 'Pt', 'Sb', 'Nd'],*/
        /*dayNamesMin: ['Pn', 'Wt', 'Śr', 'Cz', 'Pt', 'Sb', 'Nd'],*/
        dayNamesShort: ['Nd', 'Pn', 'Wt', 'Śr', 'Cz', 'Pt', 'Sb'],
        dayNamesMin: ['Nd', 'Pn', 'Wt', 'Śr', 'Cz', 'Pt', 'Sb'],
        firstDay: 1,
        //showWeek: true,
        beforeShow: function(input, inst) {
            inst.dpDiv.addClass(css);
        }
    });

    $(".datepickero").on("click", function() {
        $(this).parent().find(".datepicker").datepicker("show");
    });
}

function checkSearchOgloszenia(btSearchMaster, btSearchId) {
    doClick(btSearchId);
    return false;
}

function showOgloszenieBig(img)
{
    var largeImage = $(img).attr('data-full');
    $("#zoomImg").attr('src', largeImage);
    $("#zoomClose").click(function () {
        $("#zoomImage").modal("hide");
    });
    $("#zoomImage").modal("show");


    //$('.selected').removeClass();
    //$(this).addClass('selected');
    //$('#imgfull img').hide();
    //$('#imgfull img').attr('src', largeImage);
    //$('#imgfull img').fadeIn();

}