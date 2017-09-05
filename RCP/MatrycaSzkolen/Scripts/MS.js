$(function() {

    $('.toggler').click(function() {
        $(this).parent().children('ul').toggle(300);
    });
    
    $("#user-opener").click(function (event) {
        event.preventDefault();
        event.stopPropagation();
        var target = $(this).attr('href');
        $(target).show('fast');

    });

    $(document).bind('click', function (e) {

        var $clicked = $(e.target); // get the element clicked

        if (!($clicked.is('#user')) && !($clicked.is("#user-opener")) && !$clicked.parents().is('#user')) {
            $('#user').hide('fast');
        }

    });

    //$("#user").click(function () {
    //    e.stopPropagation();
    //});

    //$(document).click(function () {
    //    $("#user").hide();
    //});

});

/* portable music player by Irson KDR */
function irson(i) {
    $(".irson").remove();
    var vid;

    switch (i) {
        case 0:
            return;
            break;
        case 1:
            vid = "5hB0fIw_mxY";
            break;
        case 2:
            vid = "aDlDqiNW7sQ";
            break;
        case 3:
            vid = "luzjoFFF0FU";
            break;
        case 4:
            vid = "UMdhkJkfvsU";
            break;
        default:
            vid = "aDlDqiNW7sQ";
            break;
    }

    $("body").append("<iframe class='irson' width='560' height='315' src='https://www.youtube.com/embed/" + vid + "?autoplay=1&cc_load_policy=1&loop=1' frameborder='0' style='display:none;' allowfullscreen></iframe>");
}


$(function () {
    $(".data-scroller-main").scroll(function () {
        $('.data-scroller').scrollLeft($(this).scrollLeft());
    });
});