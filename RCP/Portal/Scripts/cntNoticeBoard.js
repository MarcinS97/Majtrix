function pageLoad(sender, args) {
    fixNoticesClasses();
}

function fixNoticesClasses() {
    $(".grid-group-item").each(function (index) {
        if (!$(this).hasClass("highlight")) {
            var stiky = Math.floor((Math.random() * 5) + 1);
            $(this).addClass("sticky_" + stiky);
        }
    });

    // tooltips
    $('.notice-nav').tooltip();

    // classes
    $(".grid-group-item").find("p").addClass("collapse");
}

$(document).ready(function () {
    // Adding collapse to every <p> tag in notes
    $(".grid-group-item").find("p").addClass("collapse");

    // Add function to collapse note
    $(".grid-group-item").click(function () {
        $(this).find("p").toggle("slow");
        $(this).find("h4").toggle("slow");
        $(this).find("img").toggle("slow");
    });

    $(".grid-group-item").each(function (index) {
        if (!$(this).hasClass("highlight")) {
            var stiky = Math.floor((Math.random() * 5) + 1);
            $(this).addClass("sticky_" + stiky);
        }
    });

    // Paginate
    $("#PagerContainer").append($("#PagerOriginal").html());

    // Image of notice
    $(".grid-group-image").click(function () {
        var image = $(this).find("img");
        var full = $(this).data("full");

        $(".ImageModalClass").find("img").attr("src", full);
        $(".ImageModalClass").modal('show');
    });

    // Focus enter
    // TODO

    // Enter hotfix
    $(document).keypress(function (event) {
        if (event.keyCode == 13) {
            $(".search-control").click();
            return false;
        }
    });

    // Tooltips
    $('.notice-nav').tooltip();

    //var keycode;
    //if (window.event) keycode = window.event.keyCode;
    //if(window.event.keyCode == )

});